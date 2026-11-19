"""Run Java/Go PB compatibility and resource comparisons on isolated saves."""

import argparse
from contextlib import closing, contextmanager
import http.client
from io import BytesIO
import json
import os
from pathlib import Path
import shutil
import socket
import ssl
import subprocess
import tempfile
import time
from urllib.parse import urlencode


ROOT = Path(__file__).resolve().parents[2]
HOST = "::1"
ACCOUNT, GAME, BATTLE = 9443, 17666, 17667
TLS = ssl._create_unverified_context()  # The repository's test keystore is self-signed.


def varint(value):
    output = bytearray()
    while value > 127:
        output.append((value & 127) | 128)
        value >>= 7
    return bytes(output) + bytes([value])


def read_varint(data):
    number = 0
    for shift in range(0, 70, 7):
        piece = data.read(1)
        if not piece:
            raise EOFError("PB stream ended")
        number |= (piece[0] & 127) << shift
        if piece[0] < 128:
            return number
    raise ValueError("invalid PB varint")


def integer(tag, value):
    return varint(tag << 3) + varint(value)


def blob(tag, value):
    if isinstance(value, str):
        value = value.encode()
    return varint((tag << 3) | 2) + varint(len(value)) + value


def fields(payload):
    data = BytesIO(payload)
    result = {}
    while data.tell() < len(payload):
        tag = read_varint(data)
        wire = tag & 7
        if wire == 0:
            value = read_varint(data)
        elif wire == 2:
            value = data.read(read_varint(data))
        elif wire in (1, 5):
            value = data.read(8 if wire == 1 else 4)
        else:
            raise ValueError(f"unsupported test field wire type {wire}")
        result[tag >> 3] = value
    return result


def call(conn, reader, cmd, msid, payload=b""):
    envelope = integer(1, cmd) + integer(2, msid) + blob(3, payload)
    conn.sendall(varint(len(envelope)) + envelope)
    while True:
        packet = fields(reader.read(read_varint(reader)))
        if packet.get(2, 0) == msid:
            assert packet[1] == cmd, packet
            return packet.get(3, b"")


def account(path, form=None):
    with closing(http.client.HTTPSConnection(HOST, ACCOUNT, timeout=4, context=TLS)) as connection:
        if form is None:
            connection.request("GET", path)
        else:
            connection.request("POST", path, body=urlencode(form),
                               headers={"Content-Type": "application/x-www-form-urlencoded"})
        response = connection.getresponse()
        assert response.status == 200, response.status
        body = json.load(response)["msgBody"]
        assert body["sysInfo"]["httpCode"] == 200
        business = body["bizInfo"]
        assert business["code"] == "2000", business
        return json.loads(business["dataString"])


def scenario(number, existing=False, name_prefix="GoProbe"):
    handle = account("/login", {
        "mobilePhone": f"19900000{number:03d}", "password": "local-test", "clientVersion": "1.3.5",
    })
    server = handle["gameServersInfoList"][0]
    assert server["port"] == GAME and server["serverVersion"] == "1.3.5"
    name = f"{name_prefix}{number}"
    town_id, map_id = "111101", "1111W11"
    with socket.create_connection((HOST, GAME), timeout=4) as game, game.makefile("rb") as game_reader:
        if existing:
            role = server["currentRoleVO"]["serversRoleId"].encode()
            assert server["currentRoleVO"]["name"] == name
        else:
            payload = blob(1, handle["userId"]) + blob(3, "1") + blob(4, town_id)
            payload += integer(6, 1) + blob(7, name)
            created = fields(call(game, game_reader, 1002001, 1, payload))
            assert created[2] == 1, created
            role = created[3]
        login = call(game, game_reader, 1002002, 2, blob(3, role))
        assert role in login and name.encode() in login
        maps = fields(call(game, game_reader, 1007003, 3, blob(1, town_id)))
        assert maps[2] == 1 and maps[1], maps
        dungeon = fields(call(game, game_reader, 1010001, 4, blob(1, map_id) + integer(2, 4)))
        assert dungeon[2] == 1 and dungeon[6] == BATTLE, dungeon
        joined = call(game, game_reader, 1010002, 5, blob(1, dungeon[3]) + blob(2, map_id))
        assert dungeon[3] in joined and role in joined
        with socket.create_connection((HOST, BATTLE), timeout=4) as battle, battle.makefile("rb") as battle_reader:
            entered = fields(call(battle, battle_reader, 8001001, 1, blob(1, dungeon[3]) + blob(2, role)))
            assert entered[2] == 1 and entered[10], entered
            assert fields(call(battle, battle_reader, 8001005, 2))[2] == 1
            assert fields(call(battle, battle_reader, 8001008, 3, blob(1, dungeon[3])))[2] == 1
    return role.decode()


def sample(pid):
    rss = int(subprocess.check_output(["ps", "-p", str(pid), "-o", "rss="], text=True))
    proc = Path(f"/proc/{pid}/stat")
    if proc.is_file():
        stats = proc.read_text().rsplit(")", 1)[1].split()
        cpu = (int(stats[11]) + int(stats[12])) / os.sysconf("SC_CLK_TCK")
    else:
        time_string = subprocess.check_output(["ps", "-p", str(pid), "-o", "time="], text=True).strip()
        cpu = 0.0
        for part in time_string.split(":"):
            cpu = cpu * 60 + float(part)
    return rss / 1024, cpu


@contextmanager
def run_backend(label, command, data):
    log = data.with_suffix(f".{label}.log")
    with log.open("w") as output:
        env = dict(os.environ, GOMAXPROCS="2")
        worker = subprocess.Popen(command(data), stdout=output, stderr=subprocess.STDOUT, env=env)
        try:
            deadline = time.monotonic() + 15
            while True:
                if worker.poll() is not None:
                    raise RuntimeError(f"{label} exited during startup")
                try:
                    assert account("/gameService/statusSearch")[0]["status"] == 1
                    for port in (GAME, BATTLE):
                        with socket.create_connection((HOST, port), timeout=0.5):
                            pass
                except (OSError, http.client.HTTPException):
                    if time.monotonic() >= deadline:
                        raise
                    time.sleep(0.05)
                else:
                    break
            yield worker.pid
        except Exception:
            print(log.read_text()[-6500:])
            raise
        finally:
            if worker.poll() is None:
                worker.terminate()
                try:
                    worker.wait(timeout=8)
                except subprocess.TimeoutExpired:
                    worker.kill()
                    worker.wait()


def main():
    parser = argparse.ArgumentParser(description=__doc__)
    parser.add_argument("--go-server", type=Path, required=True)
    parser.add_argument("--java", type=Path, required=True)
    parser.add_argument("--java-classes", type=Path, required=True)
    parser.add_argument("--scenarios", type=int, default=60)
    parser.add_argument("--trials", type=int, default=3)
    parser.add_argument("--rate", type=float, default=12, help="complete scenarios per second")
    args = parser.parse_args()
    if not 1 <= args.scenarios <= 900 or args.trials < 1 or args.rate <= 0:
        parser.error("use 1–900 scenarios, a positive rate and at least one trial")

    for port in (ACCOUNT, GAME, BATTLE):
        with socket.socket(socket.AF_INET6) as check:
            check.setsockopt(socket.IPPROTO_IPV6, socket.IPV6_V6ONLY, 1)
            check.bind((HOST, port))

    def java(data):
        return [
            str(args.java.resolve()), "-Xms32m", "-Xmx256m", "-XX:+UseSerialGC", "-XX:ActiveProcessorCount=2",
            f"-Dxzj.bindAddress={HOST}", f"-Dxzj.dataDir={data}",
            "-cp", os.pathsep.join((str(args.java_classes.resolve()), str(ROOT / "server/lib/*"))), "server.Main",
        ]

    def go(data):
        return [str(args.go_server.resolve()), "-bind", HOST, "-data", str(data)]

    with tempfile.TemporaryDirectory(prefix="xzj-compare-") as directory:
        folder = Path(directory)
        seed = folder / "seed"
        shutil.copytree(ROOT / "server/data/config", seed / "config")
        shutil.copy2(ROOT / "server/data/server.p12", seed / "server.p12")
        config = seed / "config/server.json"
        overlay = json.loads(config.read_text())
        overlay["battlePort"] = BATTLE
        config.write_text(json.dumps(overlay))
        with run_backend("java-seed", java, seed):
            seed_id = scenario(0, name_prefix="旧存档")
        for name in ("roles", "users"):
            path = seed / (name + ".json")
            path.write_bytes(path.read_text().encode("gbk"))

        for trial in range(1, args.trials + 1):
            order = [("java", java), ("go", go)]
            if trial % 2 == 0:
                order.reverse()
            for label, command in order:
                data = folder / f"{label}-{trial}"
                shutil.copytree(seed, data)
                with run_backend(label, command, data) as pid:
                    assert scenario(0, existing=True, name_prefix="旧存档") == seed_id
                    for number in range(1, 4):
                        scenario(number)
                    rss_start, cpu_start = sample(pid)
                    started = time.monotonic()
                    for index in range(args.scenarios):
                        scenario(index + 4)
                        remaining = started + (index + 1) / args.rate - time.monotonic()
                        if remaining > 0:
                            time.sleep(remaining)
                    elapsed = time.monotonic() - started
                    rss_end, cpu_end = sample(pid)
                print(json.dumps({
                    "trial": trial, "backend": label, "scenarios": args.scenarios,
                    "cpu_seconds": round(cpu_end - cpu_start, 3), "wall_seconds": round(elapsed, 3),
                    "avg_cpu_percent": round(100 * (cpu_end - cpu_start) / elapsed, 1),
                    "rss_before_mib": round(rss_start, 1), "rss_after_mib": round(rss_end, 1),
                }), flush=True)
                if label == "go" and trial == 1:
                    with run_backend("java-read-go-save", java, data):
                        scenario(4, existing=True)
                    print("SAVE_COMPAT_OK java-gbk-to-go go-utf8-to-java", flush=True)


if __name__ == "__main__":
    main()
