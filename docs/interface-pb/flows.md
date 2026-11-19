# 界面与 PB 玩法流程

本页服务端列记录 Java 对照实现。当前 Go 玩法入口与试用开关见 [游戏流程与模块索引](../gameplay.md)，逐条状态见 [Unity 与 Go 接口对应表](go-interfaces.md)。

表内 Cmd 链接定位到 [协议索引](commands.md) 的对应编号组。完整的逐行发送位置见 [客户端网络调用](calls.md)，Prefab 与控制器的对应关系见 [窗口归档](windows.md)。

## 启动、登录、选服与角色

| 玩家操作 | Unity 入口 | 本地服务端与数据 |
| --- | --- | --- |
| 启动、读取服务状态与公告 | [StartLoadSceneWnd](../../../xiuzhenjie-unity-recovery/UnityProject/Assets/Scripts/Assembly-CSharp/XiuZhenJie/Account/StartLoadSceneWnd.cs) 发起 `GET /gameService/statusSearch`。[SignInWnd](../../../xiuzhenjie-unity-recovery/UnityProject/Assets/Scripts/Assembly-CSharp/XiuZhenJie/Account/SignInWnd.cs) 查询公告 | [账号服 HTTPS 路由](calls.md#https) |
| 登录、注册、找回密码 | [SignInWnd](../../../xiuzhenjie-unity-recovery/UnityProject/Assets/Scripts/Assembly-CSharp/XiuZhenJie/Account/SignInWnd.cs)、[SignUpWnd](../../../xiuzhenjie-unity-recovery/UnityProject/Assets/Scripts/Assembly-CSharp/XiuZhenJie/Account/SignUpWnd.cs)、[ForgetPasWnd](../../../xiuzhenjie-unity-recovery/UnityProject/Assets/Scripts/Assembly-CSharp/XiuZhenJie/Account/ForgetPasWnd.cs) | [AccountServer](../../server/src/server/account/AccountServer.java) 处理 `POST /login`、`POST /user/register`、`GET /user/sendCheckCode` 和 `POST /user/pwdUpdate`。账号由 `users.json` 持久化 |
| 选服、连接游戏服 | [ChooseServerWnd](../../../xiuzhenjie-unity-recovery/UnityProject/Assets/Scripts/Assembly-CSharp/XiuZhenJie/Account/ChooseServerWnd.cs) 检查版本并把 `serverIp`、`port` 交给 `SignInWnd` 的回调。TCP 建连后按是否已有角色分流 | 账号服登录响应 `gameServersInfoList`。公布地址来自 [ServerConfig](../../server/src/server/common/ServerConfig.java) |
| 建角、检查名字、读取角色 | [CreateRoleWnd](../../../xiuzhenjie-unity-recovery/UnityProject/Assets/Scripts/Assembly-CSharp/XiuZhenJie/Account/CreateRoleWnd.cs) 发送 [1002005](commands.md#1002) 查名、`1002001` 建角。新建和已有角色均通过 [GameStart.EnterMainCity](../../../xiuzhenjie-unity-recovery/UnityProject/Assets/Scripts/Assembly-CSharp/XiuZhenJie/Account/GameStart.cs) 发送 `1002002` 读取角色 | [CheckNameHandler](../../server/src/server/game/modules/account/CheckNameHandler.java)、[RoleCreateHandler](../../server/src/server/game/modules/account/RoleCreateHandler.java)、[RoleLoginHandler](../../server/src/server/game/modules/account/RoleLoginHandler.java)。角色由 [RoleStore](../../server/src/server/game/modules/account/RoleStore.java) 持久化至 `roles.json` |
| 查看、领取离线收益 | `SignInWnd` 视修炼引导状态请求 `1002003`。[LiXianShouYiWnd](../../../xiuzhenjie-unity-recovery/UnityProject/Assets/Scripts/Assembly-CSharp/XiuZhenJie/Account/LiXianShouYiWnd.cs) 发送 `1002004` | [RoleOfflineHandler](../../server/src/server/game/modules/account/RoleOfflineHandler.java) 计算，[RoleGetOfflineHandler](../../server/src/server/game/modules/account/RoleGetOfflineHandler.java) 负责领取 |

## 主界面、引导与任务

| 玩家操作 | Unity 入口 | 本地服务端与数据 |
| --- | --- | --- |
| 主界面功能入口 | `MainScene` 中，[MainTitleWnd](../../../xiuzhenjie-unity-recovery/UnityProject/Assets/Scripts/Assembly-CSharp/XiuZhenJie/MainQuest/MainTitleWnd.cs) 打开洞府、纳戒、装备和外出地图。[MainCityWnd](../../../xiuzhenjie-unity-recovery/UnityProject/Assets/Scripts/Assembly-CSharp/XiuZhenJie/Map/MainCityWnd.cs) 打开城主府、邮件等 | 打开后的窗口或数据实体发起对应业务请求 |
| 按步骤引导、跳过引导 | [GuideManager](../../../xiuzhenjie-unity-recovery/UnityProject/Assets/Scripts/Assembly-CSharp/XiuZhenJie/MainQuest/GuideManager.cs) 组织弹窗与步骤，[GuideMaskItem](../../../xiuzhenjie-unity-recovery/UnityProject/Assets/Scripts/Assembly-CSharp/XiuZhenJie/MainQuest/GuideMaskItem.cs) 等发送 [1002013](commands.md#1002)。[GuideNewWnd](../../../xiuzhenjie-unity-recovery/UnityProject/Assets/Scripts/Assembly-CSharp/XiuZhenJie/MainQuest/GuideNewWnd.cs) 发送 `1002014` | [GuideInfoUpdateHandler](../../server/src/server/game/modules/mainquest/GuideInfoUpdateHandler.java)、[GuideInfoSkipHandler](../../server/src/server/game/modules/mainquest/GuideInfoSkipHandler.java) 与 [GuideRewardService](../../server/src/server/game/modules/mainquest/GuideRewardService.java) |
| 主线状态与领奖 | `MainCityWnd` → `ChengZhuFuMainWnd` → [CZF_MainContentPanel](../../../xiuzhenjie-unity-recovery/UnityProject/Assets/Scripts/Assembly-CSharp/XiuZhenJie/MainQuest/CZF_MainContentPanel.cs) 发送 [1016002](commands.md#1016)。[NetManager](../../../xiuzhenjie-unity-recovery/UnityProject/Assets/Scripts/Assembly-CSharp/XiuZhenJie/Network/NetManager.cs) 接收 `1016001` 并更新城内任务提示 | [ReceiveAwardHandler](../../server/src/server/game/modules/mainquest/ReceiveAwardHandler.java) 调 [QuestService](../../server/src/server/game/modules/mainquest/QuestService.java) 结算。任务条目见 [mission.json](../../server/data/config/mission.json)，条件见 [quests.json](../../server/data/config/quests.json) |

## 地图、城池与传送

| 玩家操作 | Unity 入口与 Cmd | 本地服务端 |
| --- | --- | --- |
| 城池列表、地图层级与详情 | [MainCityWnd](../../../xiuzhenjie-unity-recovery/UnityProject/Assets/Scripts/Assembly-CSharp/XiuZhenJie/Map/MainCityWnd.cs)、[CreateRoleWnd](../../../xiuzhenjie-unity-recovery/UnityProject/Assets/Scripts/Assembly-CSharp/XiuZhenJie/Account/CreateRoleWnd.cs)、[WorldMapWnd](../../../xiuzhenjie-unity-recovery/UnityProject/Assets/Scripts/Assembly-CSharp/XiuZhenJie/Map/WorldMapWnd.cs)。[1007001](commands.md#1007) 同级、`1007003` 子级、`1007005` 父级编号、`1007008` 根节点、`1007009` 详情 | 地图查询由各专用 Handler 使用 [MapTree](../../server/src/server/game/modules/map/MapTree.java) |
| 传送、迁城 | `WorldMapWnd` 发 `1007010` 传送或 `1007011` 迁城，回调再用 `1007003` 刷新 | [MapTransmitHandler](../../server/src/server/game/modules/map/MapTransmitHandler.java)、[MapCityMigrateHandler](../../server/src/server/game/modules/map/MapCityMigrateHandler.java) |
| 展开外出历练区域 | [CSZ_MapPanel](../../../xiuzhenjie-unity-recovery/UnityProject/Assets/Scripts/Assembly-CSharp/XiuZhenJie/Map/CSZ_MapPanel.cs) 发送 `1007013` | [LiLianChildrenMapHandler](../../server/src/server/game/modules/map/LiLianChildrenMapHandler.java) 返回可进入的历练区域 |

## 洞府、修炼、参悟与突破

| 玩家操作 | Unity 入口与 Cmd | 本地服务端与数据 |
| --- | --- | --- |
| 进入洞府、修炼室 | [DongFuRoomWnd](../../../xiuzhenjie-unity-recovery/UnityProject/Assets/Scripts/Assembly-CSharp/XiuZhenJie/Cultivation/DongFuRoomWnd.cs) 发 [1005001](commands.md#1005)。[DongFuWnd](../../../xiuzhenjie-unity-recovery/UnityProject/Assets/Scripts/Assembly-CSharp/XiuZhenJie/Cultivation/DongFuWnd.cs) 发 `1005008` | [EnterHomeHandler](../../server/src/server/game/modules/cultivation/EnterHomeHandler.java)、[ExerciseRoomHandler](../../server/src/server/game/modules/cultivation/ExerciseRoomHandler.java) |
| 查看与切换功法、调整修炼 | `DongFuWnd` 发 `1005009`、[1001007](commands.md#1001)。[GengHuanGongFaWnd](../../../xiuzhenjie-unity-recovery/UnityProject/Assets/Scripts/Assembly-CSharp/XiuZhenJie/Cultivation/GengHuanGongFaWnd.cs) 发 `1005011` | [ChangeAchievementPanelHandler](../../server/src/server/game/modules/cultivation/ChangeAchievementPanelHandler.java)、[ChangeAchievementHandler](../../server/src/server/game/modules/cultivation/ChangeAchievementHandler.java)、[ChangeRunLocationHandler](../../server/src/server/game/modules/cultivation/ChangeRunLocationHandler.java)。在线修炼由 [CultivationService](../../server/src/server/game/modules/cultivation/CultivationService.java) 更新，属性推送含 `9001004`、`9001005`、`9001006` |
| 参悟、提升功法品阶 | [CanWuWnd](../../../xiuzhenjie-unity-recovery/UnityProject/Assets/Scripts/Assembly-CSharp/XiuZhenJie/Cultivation/CanWuWnd.cs) 发 `1001011` 列表与 `1001005` 参悟。[TiShengPingJieWnd](../../../xiuzhenjie-unity-recovery/UnityProject/Assets/Scripts/Assembly-CSharp/XiuZhenJie/Cultivation/TiShengPingJieWnd.cs) 发 `1001020` | [CanWuListHandler](../../server/src/server/game/modules/cultivation/CanWuListHandler.java) 下发功法列表。[UnderstandMethodHandler](../../server/src/server/game/modules/cultivation/UnderstandMethodHandler.java) 处理 `Type=2` 功法，其他类型明确返回失败。[UpgradeAchievementHandler](../../server/src/server/game/modules/cultivation/UpgradeAchievementHandler.java) 返回失败提示 |
| 突破、渡劫准备 | `DongFuWnd` 发 `1001008` 检查、`1002015` 查询装备配置。[TuPoDuJieWnd](../../../xiuzhenjie-unity-recovery/UnityProject/Assets/Scripts/Assembly-CSharp/XiuZhenJie/Cultivation/TuPoDuJieWnd.cs) 和 `DongFuWnd` 可发 `1001010` | [BreakthroughAskHandler](../../server/src/server/game/modules/cultivation/BreakthroughAskHandler.java)、[SearchBattleEquipmentHandler](../../server/src/server/game/modules/inventory/equipment/SearchBattleEquipmentHandler.java)、[BreakthroughHandler](../../server/src/server/game/modules/cultivation/BreakthroughHandler.java)。等级表见 [xzjrolelevellist.json](../../server/data/config/xzjrolelevellist.json) |

## 纳戒、装备与技能

| 玩家操作 | Unity 入口与 Cmd | 本地服务端 |
| --- | --- | --- |
| 打开纳戒、核对容量、查功法书 | [NaJieSpaceWnd](../../../xiuzhenjie-unity-recovery/UnityProject/Assets/Scripts/Assembly-CSharp/XiuZhenJie/Inventory/NaJieSpaceWnd.cs) 用 `RoleBoxDic[10]` 取纳戒，委托 [XzjBoxEntity](../../../xiuzhenjie-unity-recovery/UnityProject/Assets/Scripts/Assembly-CSharp/XiuZhenJie/Inventory/XzjBoxEntity.cs) 发 [1004002](commands.md#1004)、`1004004`、`1004008` | [SpaceGoodsHandler](../../server/src/server/game/modules/inventory/SpaceGoodsHandler.java)、[SpaceGoodsCheckHandler](../../server/src/server/game/modules/inventory/SpaceGoodsCheckHandler.java)、[GoodsBookAchievementDetailHandler](../../server/src/server/game/modules/inventory/GoodsBookAchievementDetailHandler.java) |
| 使用、绑定物品 | `NaJieSpaceWnd` 发 `1004024` 使用、`1004023` 绑定、`1004032` 服用丹药 | [GoodsUseHandler](../../server/src/server/game/modules/inventory/GoodsUseHandler.java)、[GoodsBindingHandler](../../server/src/server/game/modules/inventory/GoodsBindingHandler.java)、[UsedElixirHandler](../../server/src/server/game/modules/inventory/UsedElixirHandler.java)，物品规则见 [GoodsService](../../server/src/server/game/modules/inventory/GoodsService.java) |
| 装备列表、穿戴与卸下 | [RoleZhuangBeiWnd](../../../xiuzhenjie-unity-recovery/UnityProject/Assets/Scripts/Assembly-CSharp/XiuZhenJie/Inventory/RoleZhuangBeiWnd.cs) 发 [1009005](commands.md#1009)、`1009001`、`1009002` | [EnterEquipmentHandler](../../server/src/server/game/modules/inventory/equipment/EnterEquipmentHandler.java)、[DressEquipmentHandler](../../server/src/server/game/modules/inventory/equipment/DressEquipmentHandler.java)、[UndressEquipmentHandler](../../server/src/server/game/modules/inventory/equipment/UndressEquipmentHandler.java) |
| 技能列表与技能栏 | `RoleZhuangBeiWnd` 发 `1009007`、`1009003`、`1009004` | [EnterSkillHandler](../../server/src/server/game/modules/inventory/equipment/EnterSkillHandler.java)、[SetSkillHandler](../../server/src/server/game/modules/inventory/equipment/SetSkillHandler.java)、[RemoveSkillHandler](../../server/src/server/game/modules/inventory/equipment/RemoveSkillHandler.java) |

## 历练、副本与战斗

| 阶段 | Unity 入口与 Cmd | 本地服务端 |
| --- | --- | --- |
| 创建、加入副本 | [BattleManager](../../../xiuzhenjie-unity-recovery/UnityProject/Assets/Scripts/Assembly-CSharp/XiuZhenJie/Battle/BattleManager.cs) 等向游戏服发 [1010001](commands.md#1010) 和 `1010002`。客户端使用创建结果中的 `Address` 和 `Port` 启动战斗连接 | [BuildDungeonHandler](../../server/src/server/game/modules/battle/BuildDungeonHandler.java)、[JoinDungeonHandler](../../server/src/server/game/modules/battle/JoinDungeonHandler.java)、[DungeonManager](../../server/src/server/game/modules/battle/DungeonManager.java) |
| 进入战斗、技能 | `BattleManager` 向战斗服发 [8001001](commands.md#8001)，读取地图范围和对象列表。[BattleWnd](../../../xiuzhenjie-unity-recovery/UnityProject/Assets/Scripts/Assembly-CSharp/XiuZhenJie/Battle/BattleWnd.cs) 发 `8001005` 技能请求 | [BattleServer](../../server/src/server/game/modules/battle/BattleServer.java) 的 `onEnter`、`onUseSkill`。移动 `8001004`、怪物受伤 `8001006`、角色受伤 `8001014` 静默处理 |
| 击杀、掉落与拾取 | [MonsterEntity](../../../xiuzhenjie-unity-recovery/UnityProject/Assets/Scripts/Assembly-CSharp/XiuZhenJie/Battle/MonsterEntity.cs) 发 [8002003](commands.md#8002)。[NetManager](../../../xiuzhenjie-unity-recovery/UnityProject/Assets/Scripts/Assembly-CSharp/XiuZhenJie/Network/NetManager.cs) 分发同 Cmd 的掉落推送。[PlayerEntity](../../../xiuzhenjie-unity-recovery/UnityProject/Assets/Scripts/Assembly-CSharp/XiuZhenJie/Battle/PlayerEntity.cs) 或结算窗发 `1004026`，玩家对象还可向战斗服发 `8001007` | `BattleServer.onMonsterDead` 与 [BattleRewardService](../../server/src/server/game/modules/battle/BattleRewardService.java) 生成奖励和掉落。[PickUpGoodsHandler](../../server/src/server/game/modules/battle/PickUpGoodsHandler.java) 在游戏服处理入包，战斗服 `8001007` 分支只回确认。怪物参数见 [monsters.json](../../server/data/config/monsters.json) |
| 结算与离开 | [BattleSceneManager](../../../xiuzhenjie-unity-recovery/UnityProject/Assets/Scripts/Assembly-CSharp/XiuZhenJie/Battle/BattleSceneManager.cs)、[ZhanDouSettlementWnd](../../../xiuzhenjie-unity-recovery/UnityProject/Assets/Scripts/Assembly-CSharp/XiuZhenJie/Battle/ZhanDouSettlementWnd.cs)、[ZhanDouJieGuoWnd](../../../xiuzhenjie-unity-recovery/UnityProject/Assets/Scripts/Assembly-CSharp/XiuZhenJie/Battle/ZhanDouJieGuoWnd.cs) | 游戏服 [1004037](commands.md#1004) 用 [BattleEndLeaveHandler](../../server/src/server/game/modules/battle/BattleEndLeaveHandler.java) 删除副本，战斗服 `8001008` 用 `onLeave` 清理副本与掉落。结算窗 `1004036` 走通用回包 |

`BattleWnd.LiKaiBtnClick` 使用战斗引导状态判断能否离开。[server.json](../../server/data/config/server.json) 的 `completeBattleGuide` 开启时，[RoleLoginHandler](../../server/src/server/game/modules/account/RoleLoginHandler.java) 下发 `BattleGuideVal=100`。

## 好友、师徒、邮件与聊天

| 玩家操作 | Unity 入口与 Cmd | 本地服务端 |
| --- | --- | --- |
| 好友、拜师 | [DaoYouWnd](../../../xiuzhenjie-unity-recovery/UnityProject/Assets/Scripts/Assembly-CSharp/XiuZhenJie/Social/DaoYouWnd.cs) 发送 [1014001](commands.md#1014)，[ShiTuWnd](../../../xiuzhenjie-unity-recovery/UnityProject/Assets/Scripts/Assembly-CSharp/XiuZhenJie/Social/ShiTuWnd.cs) 发送 `1014021` 等请求 | 示例 Cmd 走游戏服通用回包 |
| 邮件 | [ChuanXinWnd](../../../xiuzhenjie-unity-recovery/UnityProject/Assets/Scripts/Assembly-CSharp/XiuZhenJie/Social/ChuanXinWnd.cs) 发送 [1003001](commands.md#1003) 等邮件请求 | `1003001` 走游戏服通用回包 |
| 打开聊天 | [ChatManager](../../../xiuzhenjie-unity-recovery/UnityProject/Assets/Scripts/Assembly-CSharp/XiuZhenJie/Social/ChatManager.cs) 先发 [1012001](commands.md#1012) 向游戏服请求地址，再根据响应建立独立聊天连接 | [BuildChatHandler](../../server/src/server/game/modules/social/BuildChatHandler.java) 保留聊天任务进度更新，响应 `IsSuccess=false`、`Code=3003`。本地未启动聊天服 |

## 宗门

[WorldMapWnd](../../../xiuzhenjie-unity-recovery/UnityProject/Assets/Scripts/Assembly-CSharp/XiuZhenJie/Map/WorldMapWnd.cs) 发 [1015001](commands.md#1015) 查宗门、`1015005` 进宗门。宗门场景及相关业务窗口见 `ZongMenScene` 和 [窗口归档](windows.md#宗门)。[ZongMenWnd](../../../xiuzhenjie-unity-recovery/UnityProject/Assets/Scripts/Assembly-CSharp/XiuZhenJie/Guild/ZongMenWnd.cs) 汇集大殿、灵田、工坊、炼丹和其他分支。`1015xxx` 未注册游戏服专用处理，逐条通用回包状态见 [Cmd 表](commands.md#1015)。

## 虚神界、创造与生产

| 玩家操作 | Unity 入口与 Cmd | 本地服务端 |
| --- | --- | --- |
| 创造功法、法术、丹方、符书 | [ChuangZhaoGongFaWnd](../../../xiuzhenjie-unity-recovery/UnityProject/Assets/Scripts/Assembly-CSharp/XiuZhenJie/Crafting/ChuangZhaoGongFaWnd.cs) `1008033`、[ChuangZaoFaShuWnd](../../../xiuzhenjie-unity-recovery/UnityProject/Assets/Scripts/Assembly-CSharp/XiuZhenJie/Crafting/ChuangZaoFaShuWnd.cs) `1008025`、[DiHuoGeLianZhiBuZhouSheZhiWnd](../../../xiuzhenjie-unity-recovery/UnityProject/Assets/Scripts/Assembly-CSharp/XiuZhenJie/Crafting/DiHuoGeLianZhiBuZhouSheZhiWnd.cs) `1008018`、[SFH_CreateWnd](../../../xiuzhenjie-unity-recovery/UnityProject/Assets/Scripts/Assembly-CSharp/XiuZhenJie/Crafting/SFH_CreateWnd.cs) `1008047` | 走通用回包。创造算法讨论见 [DESIGN.md](../../server/DESIGN.md) |
| 创造法器与阵盘、打开灵田 | [LianQiBuZhouSheZhiWnd](../../../xiuzhenjie-unity-recovery/UnityProject/Assets/Scripts/Assembly-CSharp/XiuZhenJie/Crafting/LianQiBuZhouSheZhiWnd.cs) `1008073`、[ZS_ChuangZhaoWnd](../../../xiuzhenjie-unity-recovery/UnityProject/Assets/Scripts/Assembly-CSharp/XiuZhenJie/Crafting/ZS_ChuangZhaoWnd.cs) `1008088`、[LT_LingTianMainWnd](../../../xiuzhenjie-unity-recovery/UnityProject/Assets/Scripts/Assembly-CSharp/XiuZhenJie/Crafting/LT_LingTianMainWnd.cs) `1013001` | 示例 Cmd 走通用回包。灵田相关调用见 [1013 组](commands.md#1013)，其余见 [1008 组](commands.md#1008) |

## 商店、拍卖、排行与活动

| 玩家操作 | Unity 入口与 Cmd | 本地服务端 |
| --- | --- | --- |
| 玄天楼、拍卖 | [XuanTianLouWnd](../../../xiuzhenjie-unity-recovery/UnityProject/Assets/Scripts/Assembly-CSharp/XiuZhenJie/Commerce/XuanTianLouWnd.cs) `1004033`、[XuanTianLouXiangQingWnd](../../../xiuzhenjie-unity-recovery/UnityProject/Assets/Scripts/Assembly-CSharp/XiuZhenJie/Commerce/XuanTianLouXiangQingWnd.cs) `1004035`、[QZGItemInformationWnd](../../../xiuzhenjie-unity-recovery/UnityProject/Assets/Scripts/Assembly-CSharp/XiuZhenJie/Commerce/QZGItemInformationWnd.cs) `1006010` | 示例 Cmd 走通用回包 |
| 签到、榜单 | [MainCityWnd](../../../xiuzhenjie-unity-recovery/UnityProject/Assets/Scripts/Assembly-CSharp/XiuZhenJie/Map/MainCityWnd.cs) `1002018`、[QianDaoWnd](../../../xiuzhenjie-unity-recovery/UnityProject/Assets/Scripts/Assembly-CSharp/XiuZhenJie/Commerce/QianDaoWnd.cs) `1002021`、[XuShenTaBangDanWnd](../../../xiuzhenjie-unity-recovery/UnityProject/Assets/Scripts/Assembly-CSharp/XiuZhenJie/Crafting/XuShenTaBangDanWnd.cs) `1008002`、`1008004` | 示例 Cmd 走通用回包 |
| 活动抽奖与支付查询 | [ChouJiangEventWnd](../../../xiuzhenjie-unity-recovery/UnityProject/Assets/Scripts/Assembly-CSharp/XiuZhenJie/Commerce/ChouJiangEventWnd.cs) `1017002`、[CashGoodsWnd](../../../xiuzhenjie-unity-recovery/UnityProject/Assets/Scripts/Assembly-CSharp/XiuZhenJie/Commerce/CashGoodsWnd.cs) `GET /pay/firstRechargeList` | 抽奖 Cmd 走通用回包。支付查询使用客户端的支付地址，本地账号服无此路由 |

## 通用弹窗与界面表现

[UIManager](../../../xiuzhenjie-unity-recovery/UnityProject/Assets/Scripts/Assembly-CSharp/XiuZhenJie/Common/UIManager.cs) 通过窗口注册表创建控制器，装载 Prefab 后执行窗口生命周期，调 [InkInterfaceArt](../../../xiuzhenjie-unity-recovery/UnityProject/Assets/Scripts/Assembly-CSharp/XiuZhenJie/Common/InkInterfaceArt.cs) 与 [InkWindowMotion](../../../xiuzhenjie-unity-recovery/UnityProject/Assets/Scripts/Assembly-CSharp/XiuZhenJie/Common/InkWindowMotion.cs) 处理美术和弹窗动效。公用提示、选数量和筛选面板的业务动作通常由打开窗口时传入的回调承担。跨窗口交互可从调用方的 `PopUpWnd` 参数继续跟踪。
