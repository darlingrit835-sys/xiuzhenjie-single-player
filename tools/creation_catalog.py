"""策划目录、存档数字键与 Unity 图标之间的桥接。仅在导出时扫描策划行与资源。"""

from pathlib import Path
import re
import struct

if __package__:
    from .ecology_tables import integer, store
else:
    from ecology_tables import integer, store


SHEETS = {
    "创作门类": ("contentKinds", "门类ID 名称 目录路径 要求图标 开放创制 需要原型",
             "id name catalogPath needsIcon creatable needsPrototype"),
    "模板索引": ("contentTemplates", "门类ID 模板ID 目录键 图标ID", "kindId id catalogKey iconId"),
    "图标目录": ("contentIcons", "图标ID 式样名 资源键 适用门类ID", "id name resource kindId"),
}
UI_POOL_SHEET = "界面图池"
UI_POOL_SCHEMA = ("iconPools", "图池键 适用门类ID 图标ID", "pool kindId iconId")

OPTIONAL = {"模板索引": {"图标ID"}}
PLAYER_ID_START = 1000000000
RESOURCES = Path(__file__).resolve().parents[1] / "UnityProject/Assets/Resources"


def compile_registry(runtime, sheets, resources=RESOURCES):
    kinds, labels, catalogs = {}, {}, {}
    icons, icon_labels, paths, guids = {}, {}, set(), set()
    icon_kinds = {}
    for where, row in sheets["图标目录"]:
        icon_id = str(integer(row["id"], where, "图标ID", 1))
        resource = row["resource"]
        if not re.fullmatch(r"CatalogIcons/(?:[A-Za-z0-9_-]+/)*[A-Za-z0-9_-]+", resource) or resource.casefold() in paths:
            raise ValueError(f"{where} 图标资源键须唯一且位于 CatalogIcons/ 内")
        image = resources / (resource + ".png")
        meta = resources / (resource + ".png.meta")
        try:
            with image.open("rb") as stream:
                header = stream.read(26)
            importer = meta.read_text(encoding="utf-8")
        except OSError as error:
            raise ValueError(f"{where} 图标文件或 Unity .meta 缺失 {resource}") from error
        guid = re.search(r"^guid: ([0-9a-f]{32})$", importer, re.M)
        if (header[:16] != b"\x89PNG\r\n\x1a\n\x00\x00\x00\rIHDR" or len(header) < 26
                or struct.unpack(">II", header[16:24]) != (256, 256) or header[25] not in (4, 6)
                or not guid or guid[1] in guids or not re.search(r"^  spriteMode: 1$", importer, re.M)):
            raise ValueError(f"{where} 需要 256×256 透明通道 PNG、独立 GUID 和 Single Sprite 导入设置")
        store(icons, (icon_id,), resource, where)
        icon_kinds[icon_id] = integer(row["kindId"], where, "适用门类ID", 0)
        paths.add(resource.casefold())
        guids.add(guid[1])
        icon_labels[icon_id] = {"name": row["name"]}

    for where, row in sheets["创作门类"]:
        kind = str(integer(row["id"], where, "门类ID", 1))
        path = row["catalogPath"]
        if not re.fullmatch(r"[a-zA-Z][a-zA-Z0-9]*(?:\.[a-zA-Z][a-zA-Z0-9]*)*", path) or path in catalogs:
            raise ValueError(f"{where} 创作门类的目录路径无效或重复 {path}")
        source = runtime
        for part in path.split("."):
            source = source.get(part) if isinstance(source, dict) else None
        if not isinstance(source, dict) or not source:
            raise ValueError(f"{where} 目录路径无对应数据 {path}")
        catalogs[path] = {str(key): value for key, value in source.items()}
        store(kinds, (kind,), {"catalogPath": path, "needsIcon": bool(integer(row["needsIcon"], where, "要求图标", 0, 1)),
                              "needsPrototype": bool(integer(row["needsPrototype"], where, "需要原型", 0, 1)),
                              "creatable": bool(integer(row["creatable"], where, "开放创制", 0, 1))}, where)
        labels[kind] = {"name": row["name"]}

    for icon, target in icon_kinds.items():
        if target and str(target) not in kinds:
            raise ValueError(f"图标 {icon} 的适用门类未登记")
    templates, reverse = {}, {}
    for where, row in sheets["模板索引"]:
        kind = str(integer(row["kindId"], where, "门类ID", 1))
        if kind not in kinds:
            raise ValueError(f"{where} 未知创作门类 {kind}")
        record = kinds[kind]
        template = integer(row["id"], where, "内置模板ID", 1, PLAYER_ID_START - 1)
        key = row["catalogKey"]
        source = catalogs[record["catalogPath"]]
        if key not in source or (key.isdecimal() and str(template) != key):
            raise ValueError(f"{where} 目录键不存在或与数值模板ID不同 {key}")
        entry = {"key": key}
        if row["iconId"] is not None:
            icon = str(integer(row["iconId"], where, "图标ID", 1))
            if icon not in icons or icon_kinds[icon] not in (0, int(kind)):
                raise ValueError(f"{where} 图标 {icon} 未登记或不适用此门类")
            if record["catalogPath"] == "methods.templates" and int(icon) != source[key]["iconId"]:
                raise ValueError(f"{where} 图标与功法目录不符")
            entry["iconId"] = int(icon)
        elif record["needsIcon"]:
            raise ValueError(f"{where} 此门类需要图标")
        store(templates, (kind, str(template)), entry, where)
        store(reverse, (kind, key), template, where)
    for kind, record in kinds.items():
        missing = catalogs[record["catalogPath"]].keys() - reverse.get(kind, {}).keys()
        if missing:
            raise ValueError(f"模板索引缺少 {record['catalogPath']} 的目录键 {sorted(missing)}")
    registry = {"kinds": kinds, "templates": templates, "idsByKey": reverse, "icons": icons,
                "iconKinds": icon_kinds, "playerIdStart": PLAYER_ID_START}
    if UI_POOL_SHEET in sheets:
        pools = {}
        for where, row in sheets[UI_POOL_SHEET]:
            key = row["pool"]
            kind = integer(row["kindId"], where, "适用门类ID", 1)
            icon = integer(row["iconId"], where, "图标ID", 1)
            if not re.fullmatch(r"[a-z][a-z0-9_]*", key) or str(kind) not in kinds or str(icon) not in icons:
                raise ValueError(f"{where} 图池键、门类或图标无效")
            pool = pools.setdefault(key, {"kind": kind, "icons": []})
            if pool["kind"] != kind or icon_kinds[str(icon)] not in (kind, 0) or icon in pool["icons"]:
                raise ValueError(f"{where} 图池成员重复或门类不符")
            pool["icons"].append(icon)
        registry["iconPools"] = pools
    return registry, {"kinds": labels, "icons": icon_labels}


def client_icon_index(runtime):
    """客户端只需资源、外观引用与旧协议索引，避免解析完整服务端数值包。"""
    creation = runtime["creation"]
    kinds = {record["catalogPath"]: key for key, record in creation["kinds"].items()}
    icons = [{"id": int(icon), "kind": creation["iconKinds"][icon], "path": path}
             for icon, path in creation["icons"].items()]
    legacy = {template: key for key, template in runtime["methods"]["byLegacyId"].items()}
    methods = [{"id": int(key), "icon": row["iconId"], "legacy": legacy.get(int(key), "")}
               for key, row in creation["templates"][kinds["methods.templates"]].items()]
    monsters = []
    for key, row in creation["templates"][kinds["monsterSpecies"]].items():
        old = runtime["monsterSpecies"][row["key"]]
        monsters.append({"id": int(key), "icon": row["iconId"], "oldType": old["objectTypeId"], "oldIcon": old["iconId"]})
    pools = [{"key": key, **pool} for key, pool in sorted(creation.get("iconPools", {}).items())]
    return {"icons": icons, "methods": methods, "monsters": monsters, "pools": pools}
