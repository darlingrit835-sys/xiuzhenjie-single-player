"""Build an interface matrix from Unity call sites and the Go runtime's catalog."""

from __future__ import annotations

import argparse
from collections import defaultdict
from dataclasses import dataclass
from functools import lru_cache
import json
import os
from pathlib import Path
import re
import subprocess

import refresh as archive


GO = archive.REPO / "server-go"
OUTPUT = archive.HERE / "go-interfaces.md"
PB = r"[A-Za-z_]\w*_\d{7}"
ASSIGN = re.compile(
    rf"\b(?P<declared>var|{PB})\s+(?P<name>\w+)\s*=\s*"
    rf"(?:(?:new\s+(?P<ctor>{PB})\b)|(?P<parser>{PB})\.Parser\.ParseFrom\b)?"
)
GO_CASE = re.compile(r"^\s*case (?P<ids>\d+(?:,\s*\d+)*):(?P<body>[\s\S]*?)(?=^\s*case \d|\Z)", re.M)
HTTP_ROUTE = re.compile(
    r'^\s*\{"([^"]+)",\s*http\.Method(Get|Post),\s*(accountStatic\("[^"]*",\s*"[^"]*",\s*"[^"]*"\)|[\w.]+)\},',
    re.M,
)
CHANNELS = {
    "GamesNormalServer": ("游戏服", 17666),
    "BattleServer": ("战斗服", 17667),
    "ChatServer": ("聊天服", None),
}


@dataclass(frozen=True)
class ClientCall:
    path: Path
    line: int
    offset: int
    symbol: str
    request: str | None
    callback: bool
    response_fields: tuple[tuple[str, tuple[str, ...]], ...]


@lru_cache(maxsize=None)
def masked(source):
    return archive.CS_LITERALS.sub(lambda match: re.sub(r"[^\n]", " ", match[0]), source)


def closing_bracket(source, start, opening, closing):
    depth = 1
    for position in range(start, len(source)):
        char = source[position]
        if char == opening:
            depth += 1
        elif char == closing:
            depth -= 1
            if depth == 0:
                return position
    raise ValueError("C# 块或调用未闭合")


def request_at(source, position, argument, classes):
    if argument == "null":
        return ""
    if match := re.match(rf"new\s+({PB})\b", argument):
        return match[1] if match[1] in classes else None
    if not re.fullmatch(r"\w+", argument):
        return None
    scope = archive.lexical_scope(source[:position])
    for match in reversed(list(ASSIGN.finditer(source, 0, position))):
        if match["name"] != argument:
            continue
        declaration = archive.lexical_scope(source[:match.start()])
        if declaration == scope[:len(declaration)]:
            name = match["ctor"] or match["declared"]
            return name if name in classes else None
    parameter = rf"\b({PB})\s+{re.escape(argument)}(?=\s*[,)]).*?\)\s*(\{{)"
    for match in reversed(list(re.finditer(parameter, source[:position], re.S))):
        if match.start(2) in scope:
            return match[1] if match[1] in classes else None
    return None


def callback_body(argument, source):
    snippet = masked(argument)
    if not re.fullmatch(r"\w+", snippet.strip()):
        return snippet
    # Method groups such as CreateRoleCallBack share the caller's source file.
    name = re.escape(snippet.strip())
    pattern = rf"^\s*(?:private|public|internal|protected)\s+[^\n;=]+?\b{name}\s*\([^)]*\)\s*\{{"
    definitions = list(re.finditer(pattern, source, re.M))
    if len(definitions) != 1:
        return ""
    start = definitions[0].end()
    return source[start:closing_bracket(source, start, "{", "}")]


@lru_cache(maxsize=None)
def fields_in(path):
    return set(re.findall(r"\bpublic\s+[A-Za-z_]\S*\s+(\w+)\s*(?:=>|\{)", archive.read(path)))


def direct_callback_body(argument, source):
    body = callback_body(argument, source)
    # Nested calls own their callback types separately.
    chunks = []
    end = 0
    for match in archive.SOCKET_CALL.finditer(body):
        if match.start() < end:
            continue
        chunks.extend((body[end:match.start()], " "))
        end = closing_bracket(body, match.end(), "(", ")") + 1
    chunks.append(body[end:])
    return "".join(chunks)


def direct_fields(argument, source, classes):
    body = direct_callback_body(argument, source)
    by_type = defaultdict(set)
    for match in ASSIGN.finditer(body):
        name = match["ctor"] or match["parser"]
        if name not in classes:
            continue
        variable = re.escape(match["name"])
        accesses = re.findall(rf"\b{variable}\s*(?:\.|\?\.)\s*(\w+)", body[match.end():])
        if match["parser"] or "MergeFrom" in accesses:
            by_type[name].update(fields_in(classes[name]).intersection(accesses))
    for match in re.finditer(rf"\b({PB})\.Parser\.ParseFrom\s*\(", body):
        if match[1] in classes:
            by_type.setdefault(match[1], set())
    return tuple((name, tuple(sorted(fields))) for name, fields in sorted(by_type.items()))


def collect(unity):
    scripts_dir = unity / "Assets/Scripts/Assembly-CSharp"
    scripts = sorted(scripts_dir.rglob("*.cs"))
    classes = {script.stem: script for script in scripts if re.fullmatch(PB, script.stem)}
    enum_file = scripts_dir / "XiuZhenJie/Network/MsgType.cs"
    enums = {
        match[1]: (int(match[2]), archive.line_at(archive.read(enum_file), match.start()))
        for match in re.finditer(r"^[ \t]*(\w+)\s*=\s*(\d+),?[ \t]*$", archive.read(enum_file), re.M)
    }
    baseline, http = archive.client_calls(scripts, enums)
    calls = defaultdict(list)
    for call in baseline:
        source = masked(archive.read(call.file))
        args = call.args
        request = request_at(source, call.offset, args[1], classes)
        callback_arg = args[-1] if call.typed else args[2]
        callback = callback_arg != "null"
        if call.typed and callback:
            name = args[2].removesuffix(".Parser")
            body = direct_callback_body(callback_arg, source)
            parameter = re.match(r"\s*(\w+)\s*=>", body)
            if name not in classes or not parameter:
                raise ValueError(f"{call.file}:{call.line} 未识别的 PB 解析器或回调")
            accesses = re.findall(rf"\b{re.escape(parameter[1])}\s*(?:\.|\?\.)\s*(\w+)", body[parameter.end():])
            responses = ((name, tuple(sorted(fields_in(classes[name]).intersection(accesses)))),)
        else:
            responses = direct_fields(callback_arg, source, classes) if callback else ()
            if call.branching:
                matching = tuple(row for row in responses if row[0].endswith(f"_{call.cmd}"))
                responses = matching or responses
        calls[call.channel, call.cmd].append(ClientCall(call.file, call.line, call.offset, call.symbol, request, callback, responses))
    return calls, http, classes, enum_file, enums


def source_entries(catalog):
    locations = {}
    trial_locations = {}
    for path in sorted(GO.glob("*.go")):
        if path.name.endswith("_test.go"):
            continue
        source = archive.read(path)
        if path.name == "battle.go":
            source = source.split("func (s *Server) handleBattle(", 1)[0]
        for case in re.finditer(r"(?m)^\s*case (\d+(?:,\s*\d+)*):", source):
            for cmd in map(int, re.findall(r"\d+", case[1])):
                locations.setdefault(cmd, (path, archive.line_at(source, case.start(1))))
        for match in re.finditer(r"(?m)^\s*(\d+):\s*\{", source):
            locations[int(match[1])] = (path, archive.line_at(source, match.start(1)))
        for match in re.finditer(r"\bcmd:\s*(\d+)\b", source):
            trial_locations[int(match[1])] = (path, archive.line_at(source, match.start(1)))
        for match in re.finditer(r"(?m)^\s*privateChat(?:Send|History)\s*=\s*(\d+)", source):
            locations[int(match[1])] = (path, archive.line_at(source, match.start(1)))
    native = {cmd: locations[cmd] for cmd in catalog["game_native"]}
    previews = {
        row["cmd"]: (row, trial_locations.get(row["cmd"], (GO / "preview.go", None)))
        for row in catalog["preview"]
    }
    return native, previews


def battle_entries():
    path = GO / "battle.go"
    source = archive.read(path)
    switch = re.search(r"\bswitch msg\.Cmd\s*\{", source[source.index("func (s *Server) handleBattle("):])
    body_start = source.index("func (s *Server) handleBattle(") + switch.end()
    body_end = closing_bracket(source, body_start, "{", "}")
    result = {}
    for match in GO_CASE.finditer(source[body_start:body_end]):
        line = archive.line_at(source, body_start + match.start("ids"))
        behavior = "静默处理" if not re.search(r"\bs\.", match["body"]) else "专用处理"
        for cmd in map(int, re.findall(r"\d+", match["ids"])):
            if cmd == 8002003:
                behavior = "击杀结算 · 主动推送掉落"
            result[cmd] = (behavior, path, line)
    return result


def http_summary(handler):
    summaries = {
        "accountHotfix": "XML `ServerInfo/GameVersion`",
        "s.accountLogin": "账号、区服列表，按存档顺序附带已有角色。邮箱模式接受已注册邮箱与已有手机号，签发角色登录凭证",
        "s.accountStatus": "区服、端口、状态与客户端版本",
        "s.accountRegister": "注册后返回 `true`。邮箱模式使用 `email`、`password`、`checkCode` 并校验邮箱验证码",
        "s.accountSendEmailCode": "按填写邮箱通过 SMTP 发送六位验证码，发送成功后返回 `true`",
        "s.accountPassword": "更新账号密码后返回 `true`。写入试用和邮箱模式关闭模拟找回",
    }
    if match := re.fullmatch(r'accountStatic\("([^"]*)",\s*"[^"]*",\s*"([^"]*)"\)', handler):
        code, body = match.groups()
        return f"固定兼容回包 · `code={code}` · `dataString={body or '空串'}`"
    return summaries.get(handler, "见处理函数")


def http_rows(calls):
    path = GO / "account.go"
    source = archive.read(path)
    routes = {}
    for match in HTTP_ROUTE.finditer(source):
        route, method, handler = match.groups()
        routes[method.upper(), route] = (archive.line_at(source, match.start(1)), handler)
    for method, route in sorted(calls.keys() | routes.keys(), key=lambda key: (key[1], key[0])):
        sites = calls.get((method, route), [])
        local = routes.get((method, route))
        if sites and all(host == "Constants.PayIp" for _, _, host in sites):
            status = "客户端独立支付地址"
        elif local:
            status = archive.link(path, "路由", local[0]) + " · " + http_summary(local[1])
        else:
            prefix = next((key for key in routes if route.startswith(key[1])), None)
            if prefix:
                matched = archive.link(path, prefix[1], routes[prefix][0])
                condition = "且方法不匹配" if prefix[0] != method else ""
                status = f"无对应路由 · 命中前缀 {matched}{condition}"
            else:
                status = "无对应路由"
        yield f"`{method} {route}`", archive.links(archive.location(file, line) for file, line, _ in sites), status


def pb_link(name, classes):
    return archive.link(classes[name], name) if name in classes else f"`{name}`"


def request_links(entries, classes):
    types = sorted({entry.request for entry in entries if entry.request})
    rendered = [pb_link(name, classes) for name in types]
    if any(entry.request == "" for entry in entries):
        rendered.append("空载荷")
    if any(entry.request is None for entry in entries):
        rendered.append("调用表达式待核对")
    return archive.links(rendered)


def response_links(entries, classes):
    by_type = defaultdict(set)
    for entry in entries:
        for name, fields in entry.response_fields:
            by_type[name].update(fields)
    rendered = []
    for name, fields in sorted(by_type.items()):
        label = pb_link(name, classes)
        if fields:
            label += " · " + "、".join(f"`{field}`" for field in sorted(fields))
        rendered.append(label)
    if any(entry.callback and not entry.response_fields for entry in entries):
        rendered.append("有回调，未识别直接解析")
    if any(not entry.callback for entry in entries):
        rendered.append("无请求回调")
    return archive.links(rendered)


def sites_of(entries):
    groups = defaultdict(list)
    for entry in entries:
        groups[entry.path].append(entry.line)
    return archive.links(
        archive.link(path, path.stem, lines[0])
        + ("（" + "、".join(archive.link(path, f"L{line}", line) for line in lines[1:]) + "）" if len(lines) > 1 else "")
        for path, lines in groups.items()
    )


def behavior_for(channel, cmd, native, previews, battle, fallback, classes, default_denied):
    if channel == "ChatServer":
        return "Go 无独立聊天服务", "—"
    if channel == "BattleServer":
        if cmd not in battle:
            return "无回包", "—"
        status, path, line = battle[cmd]
        return archive.link(path, status, line), "—"
    if cmd in native:
        path, line = native[cmd]
        if cmd in (4001007, 4901001):
            action = "发出道友私聊并回执" if cmd == 4001007 else "分页读取双向私聊记录"
            return "需要写入试用与已登录的双向好友", archive.link(path, action, line)
        status = {
            9000000: "心跳 · 无回包",
            1012001: "聊天连接不可用 · 3003",
            1016004: "读取角色成就、进度和领取状态",
            1016006: "领取灵石奖励并回传余额与待领状态",
        }.get(cmd, "专用处理")
        return archive.link(path, status, line), "—"
    descriptor = fallback.get(str(cmd))
    default = "无回包类型"
    trial = "无回包类型"
    if descriptor:
        label = "明确拒绝（4008）" if cmd in default_denied else "兼容回包"
        default = f"{label} · {pb_link(descriptor, classes)}"
        response_source = classes.get(descriptor)
        if response_source and not fields_in(response_source).intersection({"IsSuccess", "Code"}):
            trial = "无状态字段，试用模式不回包"
        else:
            trial = "描述符兜底 · 有状态字段时返回失败"
    if cmd in previews:
        metadata, (path, line) = previews[cmd]
        status = "试用拒绝" if metadata.get("blocked") else "试用返回"
        body = pb_link(metadata["response"], classes)
        if metadata.get("live"):
            status += " / 写入试用处理"
        return default, f"{archive.link(path, status, line)} · {body}<br>{metadata['summary']}"
    return default, trial


def cmd_rows(channel, commands, calls, metadata):
    native, previews, battle, fallback, classes, enum_file, enums, default_denied = metadata
    symbols_by_cmd = defaultdict(list)
    for symbol, (cmd, _) in enums.items():
        symbols_by_cmd[cmd].append(symbol)
    for cmd in commands:
        entries = calls.get((channel, cmd), [])
        symbols = sorted({entry.symbol for entry in entries} or symbols_by_cmd[cmd])
        key = f"**{cmd}**"
        if symbols:
            key += "<br>" + archive.links(archive.link(enum_file, symbol, enums[symbol][1]) for symbol in symbols)
        default, preview = behavior_for(channel, cmd, native, previews, battle, fallback, classes, default_denied)
        yield key, sites_of(entries), request_links(entries, classes), response_links(entries, classes), default, preview


def preview_rows(previews, classes):
    for cmd, (metadata, (path, line)) in sorted(previews.items(), key=lambda item: (item[1][0]["module"], item[0])):
        status = "拒绝（4008）" if metadata.get("blocked") else "试用读取"
        if metadata.get("live"):
            status += " / 启用写入试用后处理"
        request = pb_link(metadata["request"], classes) if metadata["request"] else "空载荷"
        yield (
            metadata["module"], cmd, metadata["summary"], request,
            pb_link(metadata["response"], classes), archive.link(path, status, line),
        )


def push_rows(network, classes):
    entries = [
        (1001014, "RADIO_NOTICE", "BroadcastResp_1001014", "game.go", "Message{Cmd: 1001014}"),
        (1002006, "InformTitle", "InformTitleResp_1002006", "mentorship_graduation.go", "peer.send(1002006"),
        (1014024, "TONG_ZHI_TMCG", "TongZhiTMCGResp_1014024", "mentorship_practice.go", "s.pushGame(studentID, 1014024"),
        (1016001, "INFORM_TASK_COMPLETION", "", "game.go", "Message{Cmd: 1016001}"),
        (1016003, "INFORM_TIANSHUBEI", "InformTianShuBeiResp_1016003", "game_cultivation.go", "s.pushGame(roleID, achievementNoticeCmd"),
        (1015018, "GUILD_INFORM_ROLE_JOINSUCCESS", "GuildInformRoleJoinSuccessResp_1015018", "guild_trial.go", "s.pushGame(targetID, 1015018"),
        (4001002, "RECEIVE_MESSAGE", "ReceiveMessageResp_4001002", "preview_auction.go", "peer.send(4001002"),
        (4001008, "ReceiveMessageRole", "ReceiveMessageRoleResp_4001008", "chat.go", "privateChatPush"),
        (9001004, "NOTICE_SUPERAIR_UP", "SuperAirUpResp_9001004", "game_cultivation.go", "SuperAirUpResp_9001004"),
        (9001005, "NOTICE_PHYSIQUE_UP", "PhysiqueUpResp_9001005", "game_cultivation.go", "9001006, 9001005"),
        (9001006, "NOTICE_PERCEPTION_UP", "PerceptionUpResp_9001006", "game_cultivation.go", "9001006, 9001005"),
        (8002003, "MONSTER_DEAD", "MonsterDeadResp_8002003", "battle_rewards.go", "MonsterDeadResp_8002003"),
    ]
    for cmd, symbol, response, file, finds in entries:
        source = GO / file
        receiver = archive.read(network)
        match = re.search(rf"case MsgType\.{symbol}\b", receiver)
        consumer = archive.location(network, archive.line_at(receiver, match.start())) if match else "—"
        position = archive.read(source).find(finds)
        if position >= 0:
            yield (cmd, pb_link(response, classes) if response else "空载荷", consumer,
                   archive.location(source, archive.line_at(archive.read(source), position)))


def render(catalog, unity):
    calls, http, classes, enum_file, enums = collect(unity)
    native, previews = source_entries(catalog)
    battle = battle_entries()
    channels = {name: {cmd for channel, cmd in calls if channel == name} for name in CHANNELS}
    channels["GamesNormalServer"].update(native.keys() | previews.keys())
    channels["BattleServer"].update(battle)
    count = len({(entry.path, entry.offset) for entries in calls.values() for entry in entries})
    read_only = sum(not row.get("blocked") for row, _ in previews.values())
    live = sum(bool(row.get("live")) for row, _ in previews.values())
    game_calls = {cmd for channel, cmd in calls if channel == "GamesNormalServer"}
    fallback_only = game_calls.difference(native, previews)
    blocked_calls = {cmd for cmd in game_calls & previews.keys()
                     if previews[cmd][0].get("blocked") and not previews[cmd][0].get("live")}
    network = enum_file.with_name("NetManager.cs")
    lines = [
        "# Unity 前端与 Go 后端接口对应表", "",
        "> 由 [refresh_go.py](refresh_go.py) 从 Unity 源码和 Go 接口目录生成。", "",
        f"共识别 **{count} 个 PB 调用点**。Go 游戏服有 **{len(native)} 条专用处理**，"
        f"只读试用有 **{read_only} 条功能返回**和 **{len(previews) - read_only} 条拒绝回包**。"
        f"加上 `-interface-writes` 后有 **{live} 条业务路由**读取或更新持久状态。", "",
        f"在原客户端源码中定位到 **{len(game_calls)} 个不同的游戏服 Cmd**。其中 **{len(fallback_only)} 个**在 Go 默认处理和试用目录中均无业务分支，"
        f"另有 **{len(blocked_calls)} 个**已注册试用回包但仍显式拒绝。按玩法查看[道友与待实现玩法](social-status.md)。", "",
        "- 「兼容回包」沿用默认描述符回包，只填写可用的成功和状态字段。业务字段以实际回调和处理代码为准。",
        "- 请求按调用点的空载荷或变量声明识别。回调仅列出直接解析的 PB 和直接访问的字段，传入其他函数后使用的字段需按源码继续追踪。",
        "- 试用列适用于 `-interface-preview` 模式。「写入试用处理」需同时启用 `-interface-writes`。"
        "未注册请求按响应描述符兜底，通用拒绝码为 `4008`。",
        "- 未扫描到发送点的服务端条目保留空调用栏。心跳由客户端直接构造。 "
        + archive.link(enum_file.with_name("NetSvc.cs"), "NetSvc"),
    ]
    lines.extend(["", "## 试用模块一览", "",
                  "活动窗口的登录入口仍由账号回包 `huoDongList` 决定。活动试用响应可用具体 Cmd 验证。", ""])
    lines.extend(archive.table(
        ("模块", "Cmd", "操作", "请求", "响应", "处理"), preview_rows(previews, classes)
    ))
    lines.extend(["## HTTPS 账号与客户端路由", ""])
    lines.extend(archive.table(("方法与路径", "Unity 调用位置", "Go 返回"), http_rows(http)))
    metadata = native, previews, battle, catalog["fallback"], classes, enum_file, enums, set(catalog.get("default_denied", []))
    for channel, (label, port) in CHANNELS.items():
        lines.extend([f"## {label}" + (f" PB（{port}）" if port else " PB"), ""])
        lines.extend(archive.table(
            ("Cmd / MsgType", "Unity 调用位置", "客户端请求", "回调 PB 与直接字段", "Go 默认返回", "启用试用后的返回"),
            cmd_rows(channel, sorted(channels[channel]), calls, metadata),
        ))
    lines.extend(["## Go 主动推送", "", "推送使用 `Msid=0`，由客户端接收分发处理。", ""])
    lines.extend(archive.table(("Cmd", "PB 载荷", "Unity 接收位置", "Go 推送"), push_rows(network, classes)))
    return "\n".join(lines).rstrip() + "\n"


def main():
    parser = argparse.ArgumentParser(description=__doc__)
    parser.add_argument("unity", type=Path, nargs="?", default=archive.REPO.parent / "xiuzhenjie-unity-recovery/UnityProject")
    parser.add_argument("--go", default=os.environ.get("XZJ_GO", "go"), help="Go 可执行文件")
    parser.add_argument("--check", action="store_true", help="校验对应表是否最新")
    options = parser.parse_args()
    catalog = json.loads(subprocess.check_output(
        [options.go, "run", ".", "-interface-catalog"], cwd=GO, text=True,
    ))
    result = render(catalog, options.unity.resolve())
    if OUTPUT.exists() and OUTPUT.read_text(encoding="utf-8") == result:
        print("Go 对应表核对通过")
    elif options.check:
        parser.exit(1, "Go 对应表需要重新生成\n")
    else:
        OUTPUT.write_text(result, encoding="utf-8")
        print(f"已更新 {OUTPUT.name}")


if __name__ == "__main__":
    main()
