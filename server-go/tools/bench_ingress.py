#!/usr/bin/env python3
"""Measure the authorized Tencent trial without generating mail or changing saves."""

import argparse
from collections import Counter, defaultdict
from concurrent.futures import FIRST_COMPLETED, ThreadPoolExecutor, wait
from contextlib import ExitStack, contextmanager
import hashlib
import hmac
import http.client
import io
import json
import math
from pathlib import Path
import re
import socket
import ssl
import sys
import time
from urllib.parse import urlencode


PORT = 443
KEY_HEADER = "X-XZJ-Trial"
TIMEOUT = 6
LANES = ("game", "battle")
STATUS_PATH = "/gameService/statusSearch"
METRICS_PATH = "/_xzj/metrics"
MAX_BODY = 1 << 20


class CheckFailed(Exception):
    """Only fixed diagnostics and status numbers belong in this exception."""


def check(condition, reason):
    if not condition:
        raise CheckFailed(reason)


def bounded(low, high):
    def parse(text):
        value = int(text)
        if not low <= value <= high:
            raise argparse.ArgumentTypeError(f"expected an integer from {low} to {high}")
        return value

    return parse


def arguments(argv=None):
    parser = argparse.ArgumentParser(description=__doc__)
    parser.add_argument("--client-config", type=Path, required=True)
    parser.add_argument("--confirm-host", required=True, help="explicit trial DNS name or IPv4 address, matching the private client config")
    parser.add_argument("--legacy-fixture", type=Path, required=True)
    parser.add_argument("--fixture-name", choices=("leader", "player"), default="leader")
    parser.add_argument("--baseline-requests", type=bounded(1, 300), default=60)
    parser.add_argument("--mixed-requests", type=bounded(1, 500), default=120)
    parser.add_argument("--workers", type=bounded(1, 16), default=8)
    parser.add_argument("--rps", type=bounded(1, 32), default=16)
    parser.add_argument("--held-game", type=bounded(1, 64), default=8)
    parser.add_argument("--held-battle", type=bounded(1, 64), default=8)
    parser.add_argument("--lower-game", type=bounded(0, 63))
    parser.add_argument("--lower-battle", type=bounded(0, 63))
    parser.add_argument("--reload-timeout", type=bounded(5, 120), default=60)
    parser.add_argument("--wait-restore", action="store_true", help="wait for the operator to restore lowered limits")
    args = parser.parse_args(argv)
    if args.held_game + args.held_battle > 96:
        parser.error("at most 96 held tunnels across both lanes")
    for lane in LANES:
        selected = getattr(args, f"lower_{lane}")
        if selected is not None and selected >= getattr(args, f"held_{lane}"):
            parser.error(f"--lower-{lane} must be below --held-{lane}")
    if args.wait_restore and args.lower_game is None and args.lower_battle is None:
        parser.error("--wait-restore requires at least one --lower-* option")
    return args


def varint(value):
    result = bytearray()
    while value >= 128:
        result.append((value & 127) | 128)
        value >>= 7
    result.append(value)
    return bytes(result)


def read_exact(reader, size):
    result = reader.read(size)
    check(len(result) == size, "PB stream closed")
    return result


def read_varint(reader):
    value = 0
    for shift in range(0, 70, 7):
        byte = read_exact(reader, 1)[0]
        value |= (byte & 127) << shift
        if byte < 128:
            return value
    raise CheckFailed("invalid PB varint")


def number_field(field, value):
    return varint(field << 3) + varint(value)


def read_fields(data):
    fields = {}
    stream = io.BytesIO(data)
    while stream.tell() < len(data):
        field, wire = divmod(read_varint(stream), 8)
        if wire == 0:
            value = read_varint(stream)
        elif wire == 2:
            length = read_varint(stream)
            check(length <= len(data) - stream.tell(), "PB field exceeds frame")
            value = read_exact(stream, length)
        elif wire in (1, 5):
            value = read_exact(stream, 8 if wire == 1 else 4)
        else:
            raise CheckFailed("unexpected PB wire type")
        fields[field] = value
    return fields


def probe_pb(lane, connection, reader, sequence):
    # World map is a static read. Empty battle-enter has no role and returns 4503 before mutation.
    cmd, expected_success, expected_code = (1007008, 1, 2000) if lane == "game" else (8001001, 0, 4503)
    envelope = number_field(1, cmd) + number_field(2, sequence)
    connection.sendall(varint(len(envelope)) + envelope)
    size = read_varint(reader)
    check(size <= MAX_BODY, "PB reply exceeds test bound")
    reply = read_fields(read_exact(reader, size))
    check(reply.get(1) == cmd and reply.get(2) == sequence, f"{lane} PB reply did not match request")
    status = read_fields(reply[3])
    check(status.get(1) == expected_code and status.get(2, 0) == expected_success,
          f"{lane} PB reply code differs")
    return expected_code


class Trial:
    def __init__(self, args):
        config = json.loads(args.client_config.read_text())
        self.host = config["host"]
        check(isinstance(self.host, str) and 0 < len(self.host) <= 253
              and all(re.fullmatch(r"[A-Za-z0-9](?:[A-Za-z0-9-]{0,61}[A-Za-z0-9])?", label)
                      for label in self.host.split(".")), "invalid trial host")
        check(self.host == args.confirm_host and config["port"] == PORT,
              "private client config must match the explicitly confirmed host on port 443")
        self.key = config["key"]
        check(isinstance(self.key, str) and self.key.isascii() and self.key.isprintable() and self.key,
              "invalid ingress capability")
        pin_text = config["certificateSha256"].replace(":", "")
        check(re.fullmatch(r"[0-9a-fA-F]{64}", pin_text), "invalid TLS certificate pin")
        self.pin = bytes.fromhex(pin_text)
        fixture = json.loads(args.legacy_fixture.read_text())[args.fixture_name]
        self.login_form = {"mobilePhone": fixture["phone"], "password": fixture["password"], "clientVersion": "1.3.5"}
        self.role_id = fixture["roleId"]
        self.context = ssl.SSLContext(ssl.PROTOCOL_TLS_CLIENT)
        self.context.minimum_version = ssl.TLSVersion.TLSv1_2
        # The published trial leaf is self-signed. Its pin is mandatory before any bytes are sent.
        self.context.check_hostname = False
        self.context.verify_mode = ssl.CERT_NONE
        self.context.set_alpn_protocols(["http/1.1"])

    def verify_peer(self, tls):
        presented = hashlib.sha256(tls.getpeercert(binary_form=True)).digest()
        check(hmac.compare_digest(presented, self.pin), "public TLS certificate pin mismatch")

    def account(self, path, form=None):
        headers = {KEY_HEADER: self.key}
        body = None
        if form is not None:
            headers["Content-Type"] = "application/x-www-form-urlencoded"
            body = urlencode(form).encode("ascii")
        connection = http.client.HTTPSConnection(self.host, PORT, timeout=TIMEOUT, context=self.context)
        try:
            connection.connect()
            self.verify_peer(connection.sock)
            connection.request("GET" if form is None else "POST", path, body, headers)
            response = connection.getresponse()
            check(response.status == 200, f"account request HTTP {response.status}")
            payload = response.read(MAX_BODY + 1)
            check(len(payload) <= MAX_BODY, "account response exceeds test bound")
            return json.loads(payload)
        finally:
            connection.close()

    def account_probe(self, lane):
        started = time.perf_counter()
        response = self.account("/login", self.login_form) if lane == "auth" else self.account(STATUS_PATH)
        biz = response["msgBody"]["bizInfo"]
        code = str(biz["code"])
        if code != "2000":
            safe_code = code if re.fullmatch(r"[0-9]{3,4}", code) else "invalid"
            raise CheckFailed(f"{lane} account business code {safe_code}")
        if lane == "auth":
            server = json.loads(biz["dataString"])["gameServersInfoList"][0]
            check(server["currentRoleVO"]["serversRoleId"] == self.role_id, "legacy role differs")
            ticket = server["gameServerIpManageId"]
            check(isinstance(ticket, str) and "." in ticket and len(ticket) <= 64, "legacy login ticket missing")
        return lane, "HTTP 200/biz 2000", (time.perf_counter() - started) * 1000

    def metrics(self):
        response = self.account(METRICS_PATH)
        check(all(lane in response["limits"] and lane in response["lanes"] for lane in LANES), "metrics schema differs")
        return response

    @contextmanager
    def tunnel(self, lane):
        with socket.create_connection((self.host, PORT), timeout=TIMEOUT) as raw:
            with self.context.wrap_socket(raw, server_hostname=self.host) as tls:
                self.verify_peer(tls)
                request = (
                    f"GET /_xzj/{lane} HTTP/1.1\r\nHost: {self.host}\r\nConnection: Upgrade\r\n"
                    f"Upgrade: xzj-pb\r\n{KEY_HEADER}: {self.key}\r\n\r\n"
                )
                tls.sendall(request.encode("ascii"))
                with tls.makefile("rb") as reader:
                    line = reader.readline(8192).split()
                    check(len(line) >= 2 and line[0] == b"HTTP/1.1" and line[1].isdigit(), "invalid tunnel status")
                    status = int(line[1])
                    headers = {}
                    for _ in range(40):
                        header = reader.readline(8192)
                        if header == b"\r\n":
                            break
                        name, value = header.split(b":", 1)
                        headers[name.lower()] = value.strip().lower()
                    else:
                        raise CheckFailed("tunnel response headers exceed test bound")
                    if status == 101:
                        check(headers.get(b"upgrade") == b"xzj-pb", "wrong PB upgrade type")
                    yield status, tls, reader


class Results:
    def __init__(self):
        self.entries = defaultdict(list)

    def add(self, lane, status, elapsed):
        self.entries[lane].append((status, elapsed))

    def show(self, phase):
        for lane in sorted(self.entries):
            entries = self.entries[lane]
            samples = sorted(elapsed for _, elapsed in entries)
            def percentile(percent):
                return samples[math.ceil(len(samples) * percent) - 1]
            statuses = dict(Counter(status for status, _ in entries))
            print(f"{phase} {lane} n={len(samples)} p50={percentile(.50):.1f}ms p95={percentile(.95):.1f}ms "
                  f"p99={percentile(.99):.1f}ms max={samples[-1]:.1f}ms statuses={json.dumps(statuses)}", flush=True)


def http_load(trial, args, count, results, pb_connections=()):
    pending = set()
    last_send = 0.0
    started = time.perf_counter()
    pb_connections = iter(pb_connections)
    with ThreadPoolExecutor(max_workers=args.workers) as pool:
        def completed(timeout=None):
            done, remaining = wait(pending, timeout=timeout, return_when=FIRST_COMPLETED)
            for operation in done:
                results.add(*operation.result())
            return remaining

        for index in range(count):
            rate_wait = max(0.0, last_send + 1 / args.rps - time.monotonic())
            if pending:
                pending = completed(timeout=rate_wait)
            rate_wait = max(0.0, last_send + 1 / args.rps - time.monotonic())
            if rate_wait:
                time.sleep(rate_wait)
            if pending:
                pending = completed(timeout=0)
            if len(pending) >= args.workers:
                pending = completed()
            lane = "auth" if index % 10 == 0 else "account"
            pending.add(pool.submit(trial.account_probe, lane))
            if len(pending) < args.workers:
                connection = next(pb_connections, None)
                if connection is not None:
                    pending.add(pool.submit(timed_pb, connection, 2))
            last_send = time.monotonic()
        while pending:
            pending = completed()
    return count / (time.perf_counter() - started)


def reach_limits(trial, target, timeout):
    deadline = time.monotonic() + timeout
    while time.monotonic() < deadline:
        metrics = trial.metrics()
        if all(metrics["limits"][lane] == limit for lane, limit in target.items()):
            return metrics
        time.sleep(2)
    raise CheckFailed("operator limit change did not become effective in time")


def open_tunnel(trial, stack, lane, results, expected=101):
    started = time.perf_counter()
    status, connection, reader = stack.enter_context(trial.tunnel(lane))
    check(status == expected, f"{lane} upgrade HTTP {status}, expected {expected}")
    results.add(f"{lane}_upgrade", f"HTTP {status}", (time.perf_counter() - started) * 1000)
    return lane, connection, reader


def timed_pb(held_connection, sequence):
    lane, connection, reader = held_connection
    started = time.perf_counter()
    code = probe_pb(lane, connection, reader, sequence)
    return f"{lane}_pb", f"PB {code}", (time.perf_counter() - started) * 1000


def probe_held(held, results, sequence):
    for connection in held:
        results.add(*timed_pb(connection, sequence))


def dynamic(trial, args, expected, held):
    targets = {lane: getattr(args, f"lower_{lane}") for lane in LANES if getattr(args, f"lower_{lane}") is not None}
    print(f"READY_TO_LOWER limits={json.dumps(targets)}", flush=True)
    before = reach_limits(trial, targets, args.reload_timeout)
    results = Results()
    probe_held(held, results, sequence=4)
    for lane in LANES:
        check(before["lanes"][lane]["inFlight"] >= expected[lane], f"held {lane} tunnels missing from metrics")
        with ExitStack() as probe:
            status = 503 if lane in targets else 101
            open_tunnel(trial, probe, lane, results, expected=status)
    for lane in ("account", "auth"):
        results.add(*trial.account_probe(lane))
    after = trial.metrics()
    for lane in targets:
        check(after["lanes"][lane]["rejected"] > before["lanes"][lane]["rejected"], f"{lane} rejection not counted")
    results.show("lowered")
    print("LOWERED_PASS existing PB connections and unaffected lanes respond", flush=True)
    return targets


def main(argv=None):
    args = arguments(argv)
    stage = "configuration"
    try:
        trial = Trial(args)
        counts = {lane: getattr(args, f"held_{lane}") for lane in LANES}
        lowering = any(getattr(args, f"lower_{lane}") is not None for lane in LANES)
        stage = "baseline account load"
        baseline = Results()
        baseline_rate = http_load(trial, args, args.baseline_requests, baseline)
        print(f"baseline achieved_https_rps={baseline_rate:.2f}", flush=True)
        baseline.show("baseline")

        stage = "open held tunnels"
        initial_limits = trial.metrics()["limits"] if lowering else {}
        with ExitStack() as held_connections:
            mixed = Results()
            held = [open_tunnel(trial, held_connections, lane, mixed) for lane in LANES for _ in range(counts[lane])]
            stage = "mixed traffic"
            # Check every held connection around the load and one from each lane during it.
            probe_held(held, mixed, sequence=1)
            mixed_rate = http_load(trial, args, args.mixed_requests, mixed,
                                   pb_connections=(held[0], held[counts["game"]]))
            probe_held(held, mixed, sequence=3)
            print(f"mixed achieved_https_rps={mixed_rate:.2f}", flush=True)
            mixed.show("mixed")
            if lowering:
                stage = "dynamic limits"
                targets = dynamic(trial, args, counts, held)

        if args.wait_restore:
            stage = "restored limits"
            initial = {lane: initial_limits[lane] for lane in targets}
            print(f"READY_TO_RESTORE limits={json.dumps(initial)}", flush=True)
            reach_limits(trial, initial, args.reload_timeout)
            restored = Results()
            for lane in LANES:
                with ExitStack() as probe:
                    held = [open_tunnel(trial, probe, lane, restored)]
                    probe_held(held, restored, sequence=1)
            restored.show("restored")

        print("PASS direct HTTPS and PB ingress; no OTP requests sent", flush=True)
        return 0
    except CheckFailed as error:
        print(f"FAIL {stage}: {error}", file=sys.stderr, flush=True)
    except (Exception, KeyboardInterrupt) as error:
        print(f"FAIL {stage}: {type(error).__name__}", file=sys.stderr, flush=True)
    return 1


if __name__ == "__main__":
    sys.exit(main())
