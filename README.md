# 修真界单机版

本仓库保存 Go 服务端、Java 回退实现、Windows 本地启动脚本、客户端补丁和接口文档。

## 文档入口

| 要查的内容 | 入口 |
| --- | --- |
| 玩家流程、模块修改入口与回归点 | [游戏流程与模块索引](docs/gameplay.md) |
| 阴阳、五行、十二气、品阶与浮动加成 | [框架总纲与抽样方法](docs/cultivation-affinities.md) · [策划总表与细表（Excel）](server/data/config_excel/修仙属性基础表.xlsx) · [运行配置](server/data/config/cultivation_affinities.json) · [简体中文资源](server/data/config/cultivation_affinities.zh-CN.json) |
| 材料、武器、普通成品与玩家造物 | [分类、模板与创制用料](docs/item-creation.md) |
| 开山令、两种晋阶法、九阶收益与周月兽潮 | [宗门玩法与收益表](docs/sect-court.md) · [07 宗门互动画板](design/修真界UI/07%20宗门.html) · [Unity 宗门补丁](client/patches/sect-court.patch) |
| 洞府建筑直达、藏珍密室与四阶扩容 | [Unity 运行画面](docs/洞府与密室运行界面.jpg) · [四阶阵盘动效](docs/密室四阶动效.gif) · [Unity 洞府补丁](client/patches/cave-residence.patch) |
| 自由市挂牌、拍卖图录、委托举牌与交割 | [08 坊市与交易试玩](design/修真界UI/08%20坊市与交易.html) · [场次与托管规则](server-go/README.md#坊市与拍卖会) · [Unity 联机画面](docs/坊市与拍卖运行界面.jpg) · [Unity 坊市与拍卖补丁](client/patches/market-auction-hall.patch) |
| 大陆、洲、郡的加成与怪物、灵植、矿物产出 | [地区地利、修士共鸣与材料来源](docs/region-ecology.md) |
| 地火丹房、手炼火候、辅材相性与留影 | [二十方药效与收支](docs/丹房收支与用药.md) · [炼丹规则](docs/alchemy-design.md#09-地火丹房) · [09 炼丹试玩](design/修真界UI/09%20炼丹.html) · [Unity 炼丹补丁](client/patches/alchemy-workshop.patch) |
| 符纸、符墨、画符、灵田与道友偷采 | [制符与灵植玩法](docs/sigil-garden.md) · [11 制符与灵植试玩](design/修真界UI/11%20制符与灵植.html) · [Unity 联机画面](docs/制符与灵植运行界面.jpg) · [Unity 制符与灵田补丁](client/patches/sigil-garden.patch) |
| 九馆职业创作、留音壁与百工金榜 | [12 虚神界玩法与存档](docs/void-realm.md) · [初版七馆试玩](design/修真界UI/12%20虚神界.html) · [Unity 虚神界补丁](client/patches/void-realm.patch) |
| 审定炼器的词条、工艺来源与作品模板 | [炼器规则样例与引用关系](docs/smithing-workshop.md) |
| 客户端源码位置和内视补丁 | 下方「Unity 工程与 Git」 |
| 功能目录对照、文档与版本来源 | [界面与 PB 归档总览](docs/interface-pb/README.md) |
| 从玩家操作查到 Unity 窗口和 Java 处理 | [玩法流程](docs/interface-pb/flows.md) |
| 从窗口或界面子组件查 C# 和 Cmd | [场景与窗口归档](docs/interface-pb/windows.md) |
| 从 C# 脚本查调用位置、连接与回包 | [客户端网络调用](docs/interface-pb/calls.md) |
| 从 Cmd 查 PB 消息、调用方与 Java 处理 | [PB Cmd 与服务端处理](docs/interface-pb/commands.md) |
| 从 Unity 调用点查 Go 的功能与试用回包 | [Unity 与 Go 接口对应表](docs/interface-pb/go-interfaces.md) |
| 启动 Go 后端或运行对照测试 | [Go 服务端](server-go/README.md) |
| 启动、重装与排查 | [玩家使用说明](使用说明.txt) · [服务端启动说明](server/README.md) |

从 Excel 的「框架总纲」查细表。物品通过模板键连接分类、属性和用料，各玩法复用这些键。修改后在仓库根目录运行 `python3 tools/export_affinities.py` 导出运行配置与中文资源，再运行 `python3 tools/export_affinities.py --check` 核对。用 `python3 tools/generate_affinity_classes.py --seed 20261120` 可复现首版 48 个衍生类别。

## 本地启动

- macOS / Linux 在仓库根目录运行 `./server/start-local.sh`。安装 Go 1.26 或更新版本。需要运行 Unity 时，用 Unity 2022.3.62f3 打开同级工程的 `StartLoadScene` 并进入 Play Mode。
- Windows 双击 `启动游戏.bat`，脚本启动 Go，通过 ADB 建立 USB 映射并打开本地版客户端。首次安装或重装时，通过环境变量 `XZJ_CLIENT_APK` 指定本地版 APK 的完整路径，也可使用默认位置 `client/修真界单机版.apk`。手机安装步骤见[玩家使用说明](使用说明.txt)。手机直连云端使用单独构建的试用包。
- 需要回退 Java 时先停止 Go。macOS / Linux 运行 `XZJ_BACKEND=java ./server/start-local.sh`。Windows 在同一个 PowerShell 窗口执行 `$env:XZJ_BACKEND = 'java'` 后运行 `.\start.ps1`。两版共用存档格式和默认端口。
- 测试试用玩法时使用独立存档，并启用 `-interface-preview -interface-writes`。并行测试为账号、游戏和战斗各指定空闲端口。点穴与突破的本机验收结果见 [功能记录](docs/interface-pb/social-status.md#用通脉所得经脉点突破穴位)。

本机存档默认写入 `server/data/`。新检出初次运行用空账号和角色数据，可在客户端注册和建角。备份与迁移请先停服，再备份完整存档目录。运行配置表随源码提供。启动前将自己的开发证书放到 `server/data/server.p12`，并设置证书口令环境变量 `XZJ_TLS_PASSWORD`。账号、角色、宗门、聊天、历史、部署配置与云端证书按各环境单独备份。

## Unity 工程与 Git

完整 Unity 客户端位于同一仓库的 `codex/full-unity-source` 分支。在服务端源码根目录运行以下命令，文档中的跨仓源码链接即可按原路径打开。

```sh
git clone --single-branch --branch codex/full-unity-source https://github.com/darlingrit835-sys/xiuzhenjie-single-player.git ../xiuzhenjie-unity-recovery/UnityProject
```

客户端维护手册位于该工程的 `Docs/README.md`。APK、热更新基线和签名材料均由发行环境单独保存。

本轮纳戒分类与选售、器胚塑形和模具量产、法术修习与感悟的客户端改动，随[纳戒、炼器与修法补丁](client/patches/ring-forge-spell.patch)提供。补丁含器物图标，基线为本轮改动前已接入纳戒同步和炼器工坊的 Unity 工程，需搭配本仓 Go 服务端与协议表。

[纳戒分类与整理补丁](client/patches/ring-categories-and-sorting.patch)更新同一客户端的物品分类、图标、可滚动的分类栏、多条件排序及选售反馈。它针对已接入纳戒详情与符匣的工程快照；交互与设置说明见[纳戒玩法](docs/gameplay.md)。

在上述客户端基线上继续应用[炼器流程与器图难度补丁](client/patches/forge-process-and-mastery.patch)，可进入五步首制、临图练习和批铸选火。师阶、炉火、材料及器图评级的结算由本仓 Go 服务端提供。[玩法与配方说明](docs/smithing-workshop.md)

[护身行装与炼器图录补丁](client/patches/equipment-loadout.patch)配套当前 Go 服务端，应用基线为已接入装备材纹详情与五步炼器的 Unity 工程。护身行装提供四个部位、独立筛选页和八种逐帧器效，炼器图录可按部位领取样品或铸造。[穿戴规则](docs/gameplay.md#纳戒兵器和法术) · [动效实录](docs/护身行装动效.gif) · [筛选状态](docs/行装库筛选.png) · [足部器图](docs/足部炼器图.png)

靴履、风主气功法、轻身符与踏风诀共同影响历练走位，战斗中可查看当前速度与限时效果。移速补丁基于已接入护身行装、传承书目、符案及历练入场属性的 Unity 工程。[移速叠加规则](docs/逐层战斗平衡.md#移动速度) · [Unity 战斗移速增量补丁](client/patches/battle-movement.patch)

新版历练战场的 Unity 源码、角色与地景图集和着色器随[拼接战场补丁](client/patches/stitched-battlefield.patch)保存，与本仓 Go 战斗服配套。各郡的妖兽、修士和采集点按[地域生态](docs/region-ecology.md)生成。男女主角与人形散修按实际位移播放三向八帧步态，同类角色共用图集。地图通行、遮挡与小地图共用同一份格子布局，新的阵法或陷阱可以订阅 `BattleFieldLayout.CellChanged` 获取单位跨格事件。

[虚神界客户端补丁](client/patches/void-realm.patch) 在洞府山水图、炼器通信基础上接入九馆创作、专职内室开辟、留言板、排行榜及对应美术资源。玩家按真实职业资格入馆，新作品由 Go 游戏服 `4901070` 报价并扣费。[虚神界玩法与接入说明](docs/void-realm.md)

客户端修改随仓提供 [内视补丁](client/patches/neishi-acupoint.patch)、[宗门交接补丁](client/patches/guild-handover.patch) 和 [纳戒同步补丁](client/patches/inventory-sync.patch)、[储物箱同步补丁](client/patches/storage-sync.patch) 和 [签到补丁](client/patches/sign-in.patch)、[固定价委托补丁](client/patches/fixed-auction.patch)、[兑换刷新补丁](client/patches/exchange-refresh.patch) 和 [灵田倒计时补丁](client/patches/field-countdown.patch)。内视补丁针对已有内视余额读取的基线。宗门和纳戒补丁可分别对对应改动前的 1.3.5 源文件应用。用自己的 Unity 工程路径替换下方路径，从本仓根目录先检查，再应用。

```sh
patch_file="$PWD/client/patches/neishi-acupoint.patch"
git -C ../xiuzhenjie-unity-recovery/UnityProject apply --check "$patch_file"
git -C ../xiuzhenjie-unity-recovery/UnityProject apply "$patch_file"
```

完整服务端的发布分支是 `codex/full-server-source`。仓库 `main` 保留原有公开项目的历史。
