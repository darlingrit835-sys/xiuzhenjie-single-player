"""Refresh the Markdown archive from the local Java and Unity sources."""

import argparse
from collections import defaultdict
from dataclasses import dataclass
from functools import lru_cache
import os
from pathlib import Path
import re
from urllib.parse import quote


HERE = Path(__file__).resolve().parent
REPO = HERE.parents[1]
GAME = REPO / "server/src/server/game"
PROTO = REPO / "server/proto/xzj.proto"
JAVA_TYPES = REPO / "server/src/xzj/Xzj.java"
REGISTRY = GAME / "HandlerRegistry.java"
GENERIC = GAME / "GenericHandler.java"
ACCOUNT = GAME.parent / "account/AccountServer.java"

MODULES = {
    "Account": "启动、登录、选服与角色",
    "MainQuest": "主界面、引导与任务",
    "Map": "地图、城池与传送",
    "Cultivation": "洞府、修炼、参悟与突破",
    "Inventory": "纳戒、装备与技能",
    "Battle": "历练、副本与战斗",
    "Social": "好友、师徒、邮件与聊天",
    "Guild": "宗门",
    "Crafting": "虚神界、创造与生产",
    "Commerce": "商店、拍卖、排行与活动",
    "Common": "通用弹窗与调试辅助",
}

CHANNELS = {"GamesNormalServer": "游戏服", "BattleServer": "战斗服", "ChatServer": "聊天服"}
CS_LITERALS = re.compile(
    r'//[^\n]*|/\*[\s\S]*?\*/|(?:\$@|@\$|@)"(?:""|[^"])*"'
    r'|\$?"(?:\\.|[^"\\\n])*"|\'(?:\\.|[^\'\\\n])*\''
)
NET_MANAGER = r"Singleton\s*<\s*NetManager\s*>\s*\.Instance"
NET_ALIAS = re.compile(rf"\b(?:var|NetManager)\s+(\w+)\s*=\s*{NET_MANAGER}\s*;")
# These typed helpers delegate to the game socket and accept a PB parser.
SOCKET_CALL = re.compile(
    rf"\b(?:{NET_MANAGER}|(?P<alias>\w+))\s*\.\s*(?:SendMsg|TrySendGameRequest)\s*\("
    r"|(?<![.\w])(?P<no_payload>SendBreakRequest)\s*\((?=\s*MsgType\.)"
    r"|(?<![.\w])(?P<typed>Request|Send|SendAction)\s*\((?=\s*MsgType\.)"
    r"|\b(?P<entry>SendServerEntryRequest|ChengZhuFuMainWnd\s*\.\s*Request)\s*\((?=\s*MsgType\.)"
)
HTTPS = re.compile(r"\bHttpsRequest\.(UnityGetRequest|UnityPostRequest)\s*\(")


@dataclass(frozen=True)
class Call:
    file: Path
    line: int
    cmd: int
    symbol: str
    channel: str
    offset: int
    args: tuple[str, ...]
    typed: bool
    branching: bool


@lru_cache(maxsize=None)
def read(path):
    return path.read_text(encoding="utf-8-sig")


def index_files(root, pattern):
    files = {}
    for path in sorted(root.rglob(pattern)):
        if path.name in files:
            raise ValueError(f"{path.name} 有多个匹配路径：{files[path.name]} 与 {path}")
        files[path.name] = path
    return files


def line_at(source, position):
    return source.count("\n", 0, position) + 1


def link(path, label=None, line=None):
    target = quote(os.path.relpath(path, HERE), safe="/")
    if line:
        target += f"#L{line}"
    return f"[{label or path.name}]({target})"


def location(file, line):
    return link(file, f"{file.stem}:{line}", line)


def cmd_link(cmd):
    return f"[{cmd}](commands.md#{str(cmd)[:4]})"


def links(items):
    return "<br>".join(items) or "—"


def header(title, *notes):
    return [f"# {title}", "", "> 由 [refresh.py](refresh.py) 从当前工作区生成。", "", *notes, ""]


def table(headers, rows):
    return [
        "| " + " | ".join(headers) + " |",
        "| " + " | ".join("---" for _ in headers) + " |",
        *("| " + " | ".join(str(cell) for cell in row) + " |" for row in rows),
        "",
    ]


def arguments(source, masked, start):
    """Split at commas outside nested expressions and callback bodies."""
    depth = 0
    args = []
    offset = start
    for position in range(start, len(masked)):
        char = masked[position]
        if char == ")" and depth == 0:
            return args + [source[offset:position].strip()]
        if char in "([{":
            depth += 1
        elif char in ")]}":
            depth -= 1
        elif char == "," and depth == 0:
            args.append(source[offset:position].strip())
            offset = position + 1
    raise ValueError(f"未闭合的调用参数，起始行 {line_at(source, start)}")


def lexical_scope(source):
    stack = []
    for match in re.finditer(r"[{}]", source):
        if match[0] == "{":
            stack.append(match.start())
        elif stack:
            stack.pop()
    return tuple(stack)


def socket_calls(file, source, masked, enums):
    aliases = defaultdict(list)
    for declaration in NET_ALIAS.finditer(masked):
        aliases[declaration[1]].append((declaration.end(), lexical_scope(masked[:declaration.start()])))
    for match in SOCKET_CALL.finditer(masked):
        if alias := match["alias"]:
            if alias not in aliases:
                continue
            scope = lexical_scope(masked[:match.start()])
            if not any(end <= match.start() and owner == scope[:len(owner)] for end, owner in aliases[alias]):
                continue
        args = tuple(arguments(source, masked, match.end()))
        if match["no_payload"] and len(args) == 2:
            args = (args[0], "null", args[1])
        line = line_at(source, match.start())
        if len(args) < 3:
            raise ValueError(f"{file}:{line} 无法识别发送参数")
        if match["typed"]:
            expected = 5 if match["typed"] == "Request" else 4
            if len(args) != expected or not re.fullmatch(r"\w+_\d{7}\.Parser", args[2]):
                raise ValueError(f"{file}:{line} 无法识别带 PB 解析器的请求")

        symbol = re.fullmatch(r"MsgType\.(\w+)", args[0])
        choice = re.fullmatch(r"\w+\s*\?\s*MsgType\.(\w+)\s*:\s*MsgType\.(\w+)", args[0])
        if not choice and re.fullmatch(r"\w+", args[0]):
            assignment = rf"\b(?:var|MsgType)\s+{args[0]}\s*=\s*\w+\s*\?\s*MsgType\.(\w+)\s*:\s*MsgType\.(\w+)\s*;"
            scope = lexical_scope(masked[:match.start()])
            for assigned in reversed(list(re.finditer(assignment, masked[:match.start()]))):
                owner = lexical_scope(masked[:assigned.start()])
                if owner == scope[:len(owner)]:
                    choice = assigned
                    break
        if symbol:
            names = (symbol[1],)
        elif choice:
            names = tuple(dict.fromkeys(choice.groups()))
        elif file.name == "XuShenTaBangDanWnd.cs" and args[0] == "m_MsgType":
            names = ({"data": "XSJ_XST_DAOJINGBANG", "data2": "XSJ_XST_TIANJIAOBANG"}[args[1]],)
        elif re.fullmatch(r"\w+", args[0]) and re.search(rf"\bMsgType\s+{args[0]}\b", masked[:match.start()]):
            # The forwarding helper receives its Cmd from the typed call site.
            continue
        else:
            raise ValueError(f"{file}:{line} 未归档的 Cmd 表达式 {args[0]}")

        channels = ("GamesNormalServer",)
        if not (match["typed"] or match["entry"]):
            explicit = next((arg for arg in args[3:] if re.match(r"serverType\s*:", arg)), None)
            fifth = args[4] if len(args) > 4 and not re.match(r"\w+\s*:", args[4]) else None
            route = explicit or fifth or "ServerType.GamesNormalServer"
            route = re.sub(r"^serverType\s*:\s*", "", route)
            servers = re.fullmatch(r"ServerType\.(\w+)", route) or re.fullmatch(
                r"[^?:]+\?\s*ServerType\.(\w+)\s*:\s*ServerType\.(\w+)", route)
            if not servers or any(channel not in CHANNELS for channel in servers.groups()):
                raise ValueError(f"{file}:{line} 未归档的连接类型 {route}")
            channels = tuple(dict.fromkeys(servers.groups()))
        for name in names:
            for channel in channels:
                yield Call(file, line, enums[name][0], name, channel, match.start(), args, bool(match["typed"]), len(names) > 1)


def client_calls(scripts, enums):
    calls, http = [], defaultdict(list)
    for file in scripts:
        source = read(file)
        if not any(key in source for key in ("Send", "Request")):
            continue
        masked = CS_LITERALS.sub(lambda match: re.sub(r"[^\n]", " ", match[0]), source)
        calls.extend(socket_calls(file, source, masked, enums))
        for match in HTTPS.finditer(masked):
            host, _, literal, _ = arguments(source, masked, match.end())
            method = "GET" if match[1] == "UnityGetRequest" else "POST"
            http[method, literal.strip('"')].append((file, line_at(source, match.start()), host))
    return calls, http


def server_index():
    server_files = index_files(GAME, "*.java")
    source = read(REGISTRY)
    registry = {
        int(match[1]): (server_files[f"{match[2]}.java"], line_at(source, match.start()))
        for match in re.finditer(r"GameServer\.register\((\d+), new (\w+)\(", source)
    }
    battle_server = server_files["BattleServer.java"]
    source = read(battle_server)
    dispatch = re.search(r"switch \(msg\.getCmd\(\)\)\s*{([\s\S]*?)\bdefault:", source)
    battle = {
        int(match[1]): (battle_server, match[2], line_at(source, dispatch.start(1) + match.start()))
        for match in re.finditer(r"case (\d+):\s*(?:(\w+)\(ctx, msg\);)?", dispatch[1])
    }
    schema = defaultdict(list)
    source = read(JAVA_TYPES)
    line, previous = 1, 0
    # Xzj 的顶层描述符对应外层类直接包含的消息，缩进为两个空格。
    for match in re.finditer(
        r"^( +)public static final class (\w+_(\d{7})) extends\s*\n\s*com\.google\.protobuf\.GeneratedMessage\b",
        source, re.M,
    ):
        line += source.count("\n", previous, match.start())
        previous = match.start()
        schema[int(match[3])].append((match[2], line, len(match[1]) > 2))

    pushes = defaultdict(list)
    for file in server_files.values():
        source = read(file)
        for match in re.finditer(r"\bsend\(\s*(?:ctx,\s*)?(\d{7}),\s*0,", source):
            pushes[int(match[1])].append((file, line_at(source, match.start())))
    return registry, battle, schema, pushes


def game_handler(cmd, registry, schema):
    if cmd in registry:
        return link(registry[cmd][0])
    if any(not nested and re.search(r"Resp_\d+$", name) for name, _, nested in schema.get(cmd, [])):
        return link(GENERIC, "通用回包")
    return "兜底无响应类型"


def battle_handler(cmd, battle):
    if cmd not in battle:
        return "忽略"
    file, method, line = battle[cmd]
    return link(file, method or "静默处理", line)


def local_handler(call, registry, battle, schema):
    if call.channel == "ChatServer":
        return "本地无聊天服"
    if call.channel == "BattleServer":
        return battle_handler(call.cmd, battle)
    return game_handler(call.cmd, registry, schema)


def prefab_scripts(file, guid_map):
    guids = re.findall(r"m_Script: \{fileID: 11500000, guid: (\w+),", read(file))
    return sorted({guid_map[guid] for guid in guids if guid in guid_map}, key=lambda script: script.name.casefold())


def prefab_cmds(scripts, by_file):
    commands = sorted({call.cmd for script in scripts for call in by_file.get(script, [])})
    return ", ".join(cmd_link(cmd) for cmd in commands) or "—"


def windows_doc(unity, scripts, by_file, client_files):
    assets = unity / "Assets"
    ui_root = assets / "gamedata/prefabs/uiprefabs"
    resource_windows = assets / "Resources/prefab/window"
    windows = sorted(path for folder in (ui_root / "window", resource_windows) for path in folder.rglob("*.prefab"))
    items = sorted((ui_root / "item").rglob("*.prefab"), key=lambda file: file.name.casefold())
    game_start = client_files["GameStart.cs"]
    source = read(game_start)
    registrations = {
        match[2]: (client_files[f"{match[1]}.cs"], line_at(source, match.start()))
        for match in re.finditer(r'uiMag\.Register<(\w+)>\("([^"]+\.prefab)"\)', source)
    }
    guid_map = {}
    for script in scripts:
        meta = script.with_suffix(".cs.meta")
        if meta.is_file():
            guid = re.search(r"^guid: (\w+)", read(meta), re.M)
            if guid:
                guid_map[guid[1]] = script

    groups = defaultdict(list)
    for file in sorted(windows, key=lambda file: file.name.casefold()):
        attached = prefab_scripts(file, guid_map)
        controller, registered_at = registrations.get(file.name, (None, None))
        related = [controller, *attached] if controller else attached
        module = file.relative_to(ui_root / "window").parts[0] if file.is_relative_to(ui_root / "window") else (
            "Account" if file.name.startswith("Start") else "Common"
        )
        registration = link(controller) + " · " + link(game_start, "注册", registered_at) if controller else "—"
        groups[module].append((link(file), registration, links(link(script) for script in attached),
                               prefab_cmds(related, by_file)))

    settings = unity / "ProjectSettings/EditorBuildSettings.asset"
    scenes = re.findall(r"^  - enabled: (\d)\s+path: (.+\.unity)$", read(settings), re.M)
    lines = header(
        "Unity 场景与窗口归档",
        f"构建场景 **{len(scenes)}** 个，窗口 Prefab **{len(windows)}** 个，界面子组件 Prefab **{len(items)}** 个。场景顺序来自 {link(settings)}。",
        "窗口控制器来自 GameStart 注册。组件栏按 Prefab GUID 解析。Cmd 栏合并控制器和直接挂载组件的 PB 发送点。",
        "经 Manager、数据实体、嵌套 Prefab 或运行时打开其他窗口的链路，见 [玩法流程](flows.md) 和 [客户端调用](calls.md)。",
        "表中的「—」表示本栏未扫描到直接引用。",
    )
    lines.extend(["## 场景构建顺序", "", *table(("顺序", "场景", "启用"), [
        (index, link(unity / scene), "是" if enabled == "1" else "否")
        for index, (enabled, scene) in enumerate(scenes)
    ])])
    for group in dict.fromkeys((*MODULES, *sorted(groups))):
        if groups[group]:
            lines.extend([f"## {MODULES.get(group, group)}", "", *table(
                ("窗口 Prefab", "控制器", "直接挂载的 C# 组件", "直接发送的 Cmd"), groups[group]
            )])
    other_names = sorted(registrations.keys() - {file.name for file in windows})
    if other_names:
        lines.extend([
            "## 有注册但窗口目录无同名 Prefab", "",
            "下表记录源码和恢复资产目录之间的差异。运行时还有 AssetBundle 加载路径。", "",
            *table(("注册名称", "控制器", "直接发送的 Cmd"), [
                (link(game_start, name, registrations[name][1]), link(registrations[name][0]),
                 ", ".join(cmd_link(cmd) for cmd in sorted({
                     call.cmd for call in by_file.get(registrations[name][0], [])
                 })) or "—")
                for name in other_names
            ]),
        ])
    if items:
        rows = []
        for file in sorted(items, key=lambda file: (file.parent.name, file.name.casefold())):
            attached = prefab_scripts(file, guid_map)
            module = MODULES.get(file.parent.name, file.parent.name)
            rows.append((module, link(file), links(link(script) for script in attached), prefab_cmds(attached, by_file)))
        lines.extend(["## 界面子组件 Prefab", "", *table(("模块", "Prefab", "直接挂载的 C# 组件", "直接发送的 Cmd"), rows)])
    return lines


def calls_doc(by_file, http, registry, battle, schema, net_svc):
    source = read(ACCOUNT)
    routes = {
        (match[3], match[1]): (match[2], line_at(source, match.start()))
        for match in re.finditer(r'route\("([^"]+)", this::(\w+), "(GET|POST)"\);', source)
    }
    http_rows = []
    for method, path in sorted(http.keys() | routes.keys(), key=lambda key: (key[1], key[0])):
        local = routes.get((method, path))
        callers = [
            location(file, line) + ("（支付地址）" if host == "Constants.PayIp" else "")
            for file, line, host in http.get((method, path), [])
        ]
        http_rows.append((method, path, links(callers),
                          link(ACCOUNT, local[0], local[1]) if local else "账号服未注册"))

    total = len({(call.file, call.offset) for items in by_file.values() for call in items})
    lines = header(
        "客户端网络调用",
        f"共 **{total}** 个 PB 发送调用点，分布于 **{len(by_file)}** 个脚本。",
        "按脚本名查找。调用点链接可定位请求和回调。虚神塔两条变量 Cmd 通过发送分支对应到具体榜单。",
        f"心跳由 {link(net_svc, 'NetSvc.SendHeartPack')} 直接构造 {cmd_link(9000000)}。",
    )
    lines.extend([
        "## HTTPS", "", "客户端列覆盖 HttpsRequest.UnityGetRequest/UnityPostRequest。热更下载行来自服务端注册。", "",
        *table(("方法", "路径", "Unity 调用位置", "本地账号服"), http_rows), "## PB 发送点", "",
    ])
    rows = []
    for file, calls in sorted(by_file.items(), key=lambda item: item[0].name.casefold()):
        grouped = defaultdict(list)
        for call in calls:
            grouped[call.cmd, call.symbol, call.channel].append(call)
        for (cmd, symbol, channel), items in grouped.items():
            locations = "、".join(link(file, f"L{call.line}", call.line) for call in items)
            rows.append((link(file), cmd_link(cmd), symbol, CHANNELS[channel], locations,
                         local_handler(items[0], registry, battle, schema)))
    lines.extend(table(("脚本", "Cmd", "MsgType", "连接", "调用行", "本地服务端"), rows))
    return lines


def commands_doc(enum_file, enums, calls, registry, battle, schema, pushes, net_svc, net_manager, scripts):
    aliases, inbound = defaultdict(list), defaultdict(list)
    for symbol, (cmd, line) in enums.items():
        aliases[cmd].append(link(enum_file, symbol, line))
    for call in calls:
        inbound[call.cmd].append(call)
    client_types = defaultdict(dict)
    for file in scripts:
        suffix = re.search(r"_(\d{7})$", file.stem)
        if suffix:
            client_types[int(suffix[1])][file.stem] = file
    commands = sorted(
        aliases.keys() | inbound.keys() | registry.keys() | battle.keys() | schema.keys() | pushes.keys() | client_types.keys()
    )
    lines = header(
        "PB Cmd 与服务端处理索引",
        f"共 **{len(commands)}** 个不同编号，来自 MsgType、消息定义、调用点或服务端分支。",
        f"游戏服专用注册 **{len(registry)}** 条，战斗服分支 **{len(battle)}** 条。同一个 Cmd 可在多条连接上有不同处理。",
        f"服务端消息以当前构建使用的 {link(JAVA_TYPES)} 为准。{link(PROTO)} 的定义较少，供查阅。",
        "「通用回包」按服务端顶层响应类型构造默认响应。「嵌套」表示消息字段类型。「C#」链接指向同名客户端消息。",
        f"客户端推送分发见 {link(net_manager, 'NetManager.dealWithResMsg')}。本地推送列收录源码中以 Msid = 0 发送的位置。",
    )
    families = defaultdict(list)
    for cmd in commands:
        game = "—"
        clients = inbound[cmd]
        if cmd in registry or any(call.channel == "GamesNormalServer" for call in clients):
            game = game_handler(cmd, registry, schema)
            if cmd in registry:
                game += " · " + link(REGISTRY, "注册", registry[cmd][1])
        combat = "—"
        if cmd in battle or any(call.channel == "BattleServer" for call in clients):
            combat = battle_handler(cmd, battle)
        sources = defaultdict(list)
        for call in clients:
            sources[call.file, call.channel].append(call.line)
        callers = [
            link(file, file.stem, numbers[0]) + f"（{CHANNELS[channel]} ×{len(numbers)}）"
            for (file, channel), numbers in sources.items()
        ]
        if cmd == 9000000:
            callers.append(link(net_svc, "NetSvc 心跳"))
        messages = []
        cs_types = client_types.get(cmd, {})
        java_names = set()
        for name, line, nested in schema.get(cmd, []):
            java_names.add(name)
            item = link(JAVA_TYPES, name, line) + ("（嵌套）" if nested else "")
            if name in cs_types and not nested:
                item += " · " + link(cs_types[name], "C#")
            messages.append(item)
        messages.extend(
            link(file, name) + "（客户端定义）" for name, file in sorted(cs_types.items()) if name not in java_names
        )
        families[str(cmd)[:4]].append((
            cmd, links(aliases[cmd]), links(messages), links(callers),
            game, combat, links(location(file, line) for file, line in pushes.get(cmd, [])),
        ))
    for family, rows in families.items():
        lines.extend([f"## {family}", "", *table(
            ("Cmd", "客户端 MsgType", "PB 定义", "客户端发送脚本", "游戏服", "战斗服", "本地推送"), rows
        )])
    return lines


def main():
    parser = argparse.ArgumentParser(description=__doc__)
    parser.add_argument("unity", type=Path, nargs="?", default=REPO.parent / "xiuzhenjie-unity-recovery/UnityProject")
    parser.add_argument("--check", action="store_true", help="check that the generated documents match the sources")
    options = parser.parse_args()
    unity = options.unity.resolve()
    scripts_dir = unity / "Assets/Scripts/Assembly-CSharp"
    client_files = index_files(scripts_dir / "XiuZhenJie", "*.cs")
    enum_file = client_files["MsgType.cs"]
    source = read(enum_file)
    enums = {
        match[1]: (int(match[2]), line_at(source, match.start()))
        for match in re.finditer(r"^[ \t]*(\w+)\s*=\s*(\d+),?[ \t]*$", source, re.M)
    }
    scripts = sorted(scripts_dir.rglob("*.cs"))
    calls, http = client_calls(scripts, enums)
    by_file = defaultdict(list)
    for call in calls:
        by_file[call.file].append(call)
    registry, battle, schema, pushes = server_index()
    net_svc, net_manager = client_files["NetSvc.cs"], client_files["NetManager.cs"]
    outputs = {
        "windows.md": windows_doc(unity, scripts, by_file, client_files),
        "calls.md": calls_doc(by_file, http, registry, battle, schema, net_svc),
        "commands.md": commands_doc(enum_file, enums, calls, registry, battle, schema, pushes, net_svc, net_manager, scripts),
    }
    changed = []
    for name, lines in outputs.items():
        path = HERE / name
        content = "\n".join(lines).rstrip() + "\n"
        if path.is_file() and read(path) == content:
            continue
        changed.append(name)
        if not options.check:
            path.write_text(content, encoding="utf-8")
    if options.check and changed:
        parser.exit(1, "需要重新生成 " + ", ".join(changed) + "\n")
    print("核对通过" if options.check else ("已更新 " + ", ".join(changed) if changed else "归档已是最新"))


if __name__ == "__main__":
    main()
