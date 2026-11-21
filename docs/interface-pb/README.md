# 界面与 PB 归档

这份归档基于同级 Unity 1.3.5 恢复工程与当前单机仓库的 Java 源码。可以从中文玩法、Unity 窗口名、C# 文件名或 PB 的 Cmd 编号开始查。

Go 后端沿用这些 PB 编号与存档字段，[Go 源码与运行说明](../../server-go/README.md) 提供协议、账号、游戏和战斗模块入口。

按玩家流程或功能找修改入口，先看 [游戏流程与模块索引](../gameplay.md)。本文的 Unity 源码与资源链接按本机同级目录生成。单独克隆时可按表中的文件名查阅 Git 内的 PB 和服务端代码。目录位置和恢复方式见 [仓库说明](../../README.md#unity-工程与-git)。

## 索引

| 文档 | 查阅方式 |
| --- | --- |
| [玩法流程](flows.md) | 按登录建角、主线、地图、修炼、战斗、宗门等玩法查入口、间接调用、处理与数据 |
| [场景与窗口归档](windows.md) | 按模块查 15 个构建场景、234 个窗口 Prefab 和 124 个界面子组件 Prefab。控制器按 `GameStart` 注册定位，组件按 Prefab GUID 定位 |
| [客户端网络调用](calls.md) | 按脚本定位 `SendMsg`、`TrySendGameRequest` 和已识别的窗口封装，也可查 HTTPS 路径和账号服路由 |
| [PB Cmd 与服务端处理](commands.md) | 按 Cmd 编号反查枚举、C# 和 Java 消息、发送方、服务端分支与本地推送 |
| [Unity 与 Go 接口对应表](go-interfaces.md) | 核对前端请求、回调类型、读取字段，以及 Go 默认与试用模式下的回包 |
| [炼器工坊](../smithing-workshop.md) | 查工坊命令 `4901020`～`4901024` 的目录、开炉、领料、作品详情及火候预览，以及纳戒、奇珍阁如何引用同一作品模板 |
| [道友与待实现玩法](social-status.md) | 查询好友、师徒的操作入口，以及其他模块尚缺的业务处理 |

## 项目目录

| 工作区 | 内容位置 |
| --- | --- |
| [单机服务端仓库](../../README.md) | `server/src/server/account` 处理 HTTPS，`server/src/server/game` 放游戏服分派与会话基础，`server/src/server/game/modules` 放专用处理和业务服务。`server/src/xzj/Xzj.java` 是构建使用的 PB 类型，`server/proto/xzj.proto` 保留较早的协议整理稿 |
| 单机配置与运行数据 | `server/data/config` 存运行用 JSON，`server/data/config_excel` 存策划表副本。账号、角色及其备份见 `server/data`。本地编译输出见 `server/.local-build`，Windows 启动脚本使用 `server/out` |
| 同级 Unity 工程 | `UnityProject/Assets/Scenes` 放场景，`gamedata/prefabs/uiprefabs/window/<模块>` 放可编辑窗口，`gamedata/prefabs/uiprefabs/item/<模块>` 放界面子组件，`Scripts/Assembly-CSharp/XiuZhenJie/<模块>` 放业务 C#，`StreamingAssets/Assetbundles` 放客户端运行时包 |
| Unity 恢复资料与交付 | `RecoveryData` 存恢复原料，`RecoveryLogs` 存编辑器、联调和真机记录，`Builds` 为本机构建输出。安装包由发行环境单独保管，安装步骤见[玩家使用说明](../../使用说明.txt) |
| 独立的公开说明仓库 | 当前本地检出包含面向玩家的说明、启动工具与配置样例 |

历史记录归档在 `server` 下。[FIXES.md](../../server/FIXES.md) 记录既往修复与参数，[DESIGN.md](../../server/DESIGN.md) 记录玩法设计与待实现方案，[CONTINUE.md](../../server/CONTINUE.md) 记录旧环境路径与早期调查。当前源码状态见本目录的调用表。

## 功能目录对照

| 功能 | Unity 资源与业务脚本目录 | Java 专用处理目录 |
| --- | --- | --- |
| 启动、登录、选服、角色 | `Account` | `modules/account` 与 `server/src/server/account`（HTTPS） |
| 主界面、引导与任务 | `MainQuest` | `modules/mainquest` |
| 地图、城池、传送 | `Map` | `modules/map` |
| 洞府、修炼、参悟与突破 | `Cultivation` | `modules/cultivation` |
| 纳戒、装备与技能 | `Inventory` | `modules/inventory` 与 `modules/inventory/equipment` |
| 历练、副本与战斗 | `Battle` | `modules/battle` |
| 好友、师徒、邮件与聊天 | `Social` | `modules/social`（聊天连接请求） |
| 宗门、虚神界与生产、商店与活动 | `Guild`、`Crafting`、`Commerce` | 逐条见 [Cmd 索引](commands.md) |
| 通用弹窗和网络 | `Common`，`Network`（仅脚本） | `server/src/server/game`（注册、分派、通用回包） |

Unity 原 `.meta` 随对应文件移动，GUID 与 AssetBundle 标记随之保留。`StartLoadSceneWnd`、`StartLogoWnd` 和 `AttentionWnd` 的可编辑 Prefab 位于 `Assets/Resources/prefab/window`，供 `Resources.Load` 按固定键读取。普通窗口和列表项继续按原始逻辑键（如 `Assets/GameData/Prefabs/UIPrefabs/Window/SignInWnd.prefab`）读取现有 AssetBundle。以后重做运行时包时，资源索引仍需提供这些逻辑键。 [UIManager](../../../xiuzhenjie-unity-recovery/UnityProject/Assets/Scripts/Assembly-CSharp/XiuZhenJie/Common/UIManager.cs)

C# 业务类沿用原命名空间和程序集，生成的 PB 类型集中在 `Assets/Scripts/Assembly-CSharp`。Java 模块沿用 `server.game` 包名，Windows `build.bat` 和 macOS/Linux `start-local.sh` 都递归收集 Java 源文件。

## 服务与状态

| 连接 | 源码入口 | 处理方式 |
| --- | --- | --- |
| HTTPS `9443` | [AccountServer.java](../../server/src/server/account/AccountServer.java) | 登录、注册、公告、服务状态与资源等 HTTP 路由。登录返回选服数据 |
| TCP `17666` | [GameChannelHandler.java](../../server/src/server/game/GameChannelHandler.java) · [HandlerRegistry.java](../../server/src/server/game/HandlerRegistry.java) | 按 Cmd 查游戏服专用处理。未注册的 Cmd 由 [GenericHandler.java](../../server/src/server/game/GenericHandler.java) 查找 Java 顶层 `*Resp_<Cmd>`，按可用字段置 `IsSuccess=true`、`Code=2000`。查不到类型时无回包 |
| TCP `17667` | [BattleServer.java](../../server/src/server/game/modules/battle/BattleServer.java) | 战斗连接单独分派，移动、受伤等分支静默处理，未匹配的 Cmd 忽略 |
| 聊天连接 | [ChatManager.cs](../../../xiuzhenjie-unity-recovery/UnityProject/Assets/Scripts/Assembly-CSharp/XiuZhenJie/Social/ChatManager.cs) · [BuildChatHandler.java](../../server/src/server/game/modules/social/BuildChatHandler.java) | 客户端有独立聊天连接类型。本机 `1012001` 专用处理会更新相关任务进度并返回失败与空地址 |

TCP 帧是 `[varint32 长度][PB Msg]`。`Msg` 包含 `Cmd=1`、`Msid=2` 和 `Data=3`。客户端在 `SendMsg` 序列化请求，按 `Msid` 分发回调。任务完成、属性变更与怪物掉落走推送分支。 [客户端收发](../../../xiuzhenjie-unity-recovery/UnityProject/Assets/Scripts/Assembly-CSharp/XiuZhenJie/Network/NetManager.cs) · [帧封装](../../../xiuzhenjie-unity-recovery/UnityProject/Assets/Scripts/Assembly-CSharp/XiuZhenJie/Network/NetSvc.cs) · [Msg 定义](../../server/proto/xzj.proto#L2167)

表中“专用处理”表示源码有注册或分支。返回结果、存档变化与实际界面行为要看相应方法。“通用回包”仅提供 PB 默认数据和可用的状态字段。编号索引也收录消息类后缀，带“嵌套”的行可能只对应某个消息字段。

## 更新归档

从单机仓库根目录运行以下命令。[refresh.py](refresh.py) 根据实际模块子目录、Prefab GUID 和控制器注册更新表格与源码路径。

```sh
python3 docs/interface-pb/refresh.py
python3 docs/interface-pb/refresh.py --check
python3 docs/interface-pb/refresh_go.py --check
```

Unity 工程移到其他路径时，可以把 `UnityProject` 的实际路径传给脚本。玩法流程中的间接调用和处理语义按源码人工维护。

## 目录调整验证

Unity 2022.3.62f3 在隔离工程中编译，并导入 231 个窗口 Prefab 和 124 个界面子组件 Prefab。Play Mode 已从启动场景进入登录界面，验证了 9 个窗口和 2 个列表项的原运行时键。Java 服务端用本机 JDK 从新目录全量编译，并用独立存档走通登录、建角、查地图、进入和退出副本。Windows 批处理本轮通过静态检查。
