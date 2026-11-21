# 修真界 UI 接入顺序

**第一批做登录背景、公告、玩法说明、离线收益和设置的样式。第二批再做选服、纳戒、传信与道友。** 建角、主城和地图、战斗、画符炼丹的布局变化最大，先核对所有玩家操作，再排整页改造。

本次对照使用 Unity 恢复工程当前源码与资源、已归档的 Android 31 版启动截图、较早的登录和选服真机截图，以及 Unity 界面截图。涉及运行时样式的判断以当前代码为准。历史截图用于比较构图，交易、战斗、生产和宗门的接入难度根据字段与回调判断。 [六组截图对照](现有界面与新稿对照.jpg)

## 第一批 · 使用现有控件换样式

| 优先 | 界面 | 可以先接入 | 接入位置与需要覆盖的功能 |
| --- | --- | --- | --- |
| 1 | [登录](../高清稿/Account/SignInWnd.png) | 山水底图、米纸表单、黛青主按钮 | `SignInWnd.StyleSignIn` 已有样式入口。用登录页专属底图，现有 `InkBrightLandscape` 也用于启动和社交。沿用账号密码、公告、注册、找回密令、协议和登录等待状态。 |
| 2 | [公告](../高清稿/Common/PopUpWnd.png)、[玩法说明](../高清稿/Common/XiaoWenHaoWnd.png) | 遮罩、纸面、按钮和标题层级 | `InkDialogArt.StylePopup`、`StyleHelp`。用真实公告与原帮助内容填充，可滚动的说明需要覆盖长文本和关闭。 |
| 3 | [离线收益](../高清稿/Account/LiXianShouYiWnd.png) | 把新稿的收益行和按钮样式套到现有弹层 | `LiXianShouYiWnd.StyleRewardPanel`。保留领取和放弃两条路径，数量和可领状态继续用现有回包。新稿的全屏构图可后排。 |
| 4 | [设置](../高清稿/Common/OptionWnd.png) | 开关、分组标题、兑换码输入框 | `OptionWnd.Awake` 已绑定音乐、音效、常亮、兑换、回归和登出。沿用现有开关即时生效机制，新稿底部“保存设置”按现有操作改为“返回”。 |
| 5 | [选服行](../高清稿/Account/ChooseServerItem.png)、[物品详情](../高清稿/Inventory/NaJieTipWnd.png) | 小范围试用选中态、价格和属性的排版 | `ChooseServerItem.StyleRow`、`NaJieTipWnd.Awake`。选服保留上次登录行和列表。详情保留滚动属性，以及物品类型决定的使用、出售、锁定和品阶操作。 |

首次接入可以先做 **登录、公告、玩法说明**。这三处容易从现有入口连续检查，也能直接确认新底图、纸面和按钮实际呈现。离线收益、设置和物品详情可复用同一套视觉规范。现有通用样式先按窗口局部使用，避免一次改动所有调用通用纸面和按钮的窗口。

## 第二批 · 保留玩法，重排现有控件

| 界面 | 新旧差异 | 接入边界 |
| --- | --- | --- |
| [启动加载](../高清稿/Account/StartLoadSceneWnd.png) | 旧版是淡青留白和底部进度，新稿有整屏山水与大标题。构图差异大，换一层背景相对独立。 | 正式全量包启动前段是 `ReleaseLoadingView`，它直接从安装包读取底图。游戏内的 `StartLoadSceneWnd` 和转场 `LoadingWnd` 分开处理。长状态文字、进度与失败重试需要在同一屏容纳。若要求安装即见新皮肤，启动前段需要进新版 APK。 |
| [选服](../高清稿/Account/ChooseServerWnd.png) | 现有是覆盖在登录页上的服务器卡片，新稿是整屏择界。 | 先换列表行和标题。整页再决定如何显示上次登录、服务器状态、空列表与关闭返回。服名与状态从现有回包读取。 |
| [纳戒](../高清稿/Inventory/NaJieSpaceWnd.png)、[储物箱](../高清稿/Inventory/ChuWuXiangWnd.png) | 两边都有容量和物品区。新稿加入显眼的搜索、分类标签和大图格，信息密度变化明显。 | `NaJieSpaceWnd.StyleStorage` 可先换容量条和边框。`UI_GoodsItem` 保留真实图标、数量、锁定及可用态；搜索和快捷标签在现有分类、五行、品阶、等阶筛选以外单独确认。储物箱还需保留存取中状态。 |
| [传信](../高清稿/Social/ChuanXinWnd.png)、[道友](../高清稿/Social/DaoYouWnd.png)、[私聊](../高清稿/Social/DaoYouChatWnd.png)、[师徒](../高清稿/Social/ShiTuWnd.png) | 已经有水墨风。新稿加高画头，列表和消息区会缩短，现有动作在新稿中有省略。 | 从单行、头像框、气泡和纸面开始。保留传信附件与批量操作，私聊滚动历史、长消息及键盘避让，道友名刺与传物按钮，师徒请安、传功、通脉等实际入口。 |
| [七日签到](../高清稿/Commerce/QianDaoWnd.png) | 新稿主操作是点按钮领取，现有逻辑在满足条件时从 `GetQianDaoData` 自动调用领取。 | 可以换 7 个奖励格、印章及颜色，继续以当前领取时机、奖励数据与已有动画显示。改为手动领取时另做流程设计。 |
| [奇珍阁](../高清稿/Commerce/QiZhenGeWnd.png)、[商铺](../高清稿/Commerce/ShangPuWnd.png) | 新稿把陈列做得更像商品货架，奇珍阁的价格、品阶排序和翻页在画板上弱化。 | 优先接拍品或商品行。整页改造时把筛选、排序、搜索、翻页、一口价、竞拍和我的委托各自回到已有参数与回调。支付和发布保留确认与失败状态。 |

## 差别特别大 · 先补完流程稿

| 优先核对 | 当前界面与新稿的差别 | 改造涉及 |
| --- | --- | --- |
| **[创建角色](../高清稿/Account/CreateRoleWnd.png)** | 当前分步填写姓名、选性别和头像、再选五行灵根和出生地。新稿用大立绘和三段标签，首屏只画代表状态。 | 多头像网格、名称验证、五行选择规则、出生地可选态、协议和返回后的已填内容。可先换已有选项边框或立绘容器。 |
| **[主城](../高清稿/Map/MainCityWnd.png)、[世界地图](../高清稿/Map/WorldMapWnd.png)、[常驻主界面](../高清稿/MainQuest/MainTitleWnd.png)** | 当前城内建筑和快捷按钮各有入口，主界面仍承载头像、资源数和常用导航。新稿改成地图节点、路线和下方摘要，一部分入口要重新摆放。 | 建筑及地图节点必须用真实位置和城池数据，覆盖地图层级、城际传送、宗门、历练和新手引导热点。对照图的城池样例是沙海城，设计稿是临仙城，差异判断取布局与入口结构。 |
| **[战斗 HUD](../高清稿/Battle/BattleWnd.png)** | 新稿集中展示锁定目标、三条状态与少量施法键。实际 HUD 还包括小地图、技能组切换、摇杆的移动及释放事件，以及结算返回。 | 触控区、技能状态、目标、入口遮挡和角色路径。先从贴图和已显示状态样式做起；战后结果、重伤、扫荡要按原窗口各自的结算与选择操作适配。 |
| **[练功房](../高清稿/Cultivation/DongFuWnd.png)、[经脉](../高清稿/Cultivation/JingMaiWnd.png)、[渡劫](../高清稿/Cultivation/TuPoDuJieWnd.png)** | 练功房原本把功法、阵位与突破控件放在人物周边，新稿挪到图下。经脉和渡劫画板展示的是局部和某一时刻。 | 现有五行背景、灵气效果、实时状态、穴位点击、闪躲冷却、落雷动画和结果链。人物与场景素材可先分项替换。 |
| **[画符](../高清稿/Crafting/HuaFuWnd.png)、[炼丹](../高清稿/Crafting/DiHuoGeLianZhiWnd.png)、[炼器工坊](../高清稿/Crafting/ForgeWorkshop.png)** | 画符新稿像自由书写面板，现有玩法逐段判定滑动方向和时机。炼丹新稿像炉温仪表，现有流程包含控火与方向投料。炼器工坊由代码动态生成，样稿按物料与炉火重排。 | 画符补齐等待、允许落笔、错笔、超时和回执。炼丹补齐控火、投料与收丹步骤。炼器先用现有布局试独立炉鼎图和按钮，整页再核对材料、命名、供料、炉火、目标品阶和回执。 |
| **[宗门山门](../高清稿/Guild/ZongMenWnd.png)、[宗门主峰](../高清稿/Guild/ZongMenZhuFengWnd.png)** | 新稿按主峰和诸峰组织，现有入口分大殿、内门、生产设施、矿脉与护宗等多个交互。 | 先改山门底图和印章。整页要回填各设施入口、权限、资源不足、兽潮和战报状态。 |
| **新玩法提案与旧资源缺件** | 4 张策划提案需要绑定对应的游戏数据。11 张来自注册键、3 张来自旧调用的设计，恢复目录里缺独立预制体。 | 先按实际运行时资源表核对窗口能否实例化，再评估接入。图鉴、词条预览、作品命名及作品详情按功能需求排期。 |

## Unity 接入位置

- [UIManager.cs](../../../../xiuzhenjie-unity-recovery/UnityProject/Assets/Scripts/Assembly-CSharp/XiuZhenJie/Common/UIManager.cs) 实例化窗口后，先调用全局 `InkInterfaceArt.Apply`，再执行窗口 `Awake`。已有窗口样式函数适合作为局部外观入口。
- [RecoveredBundleContent.cs](../../../../xiuzhenjie-unity-recovery/UnityProject/Assets/Scripts/Assembly-CSharp/XiuZhenJie/Content/RecoveredBundleContent.cs) 先查发布内容，再走原 AssetBundle 或内置 Resources。涉及整页 prefab 的改造要保留资源键、组件引用和脚本查询的子路径，并验证实际运行入口加载的是更新后的资源。
- [ReleaseLoadingView.cs](../../../../xiuzhenjie-unity-recovery/UnityProject/Assets/Scripts/Client/Release/ReleaseLoadingView.cs) 自己维护启动进度和失败重试。画板中的固定进度与示例文字由现有运行状态替换。
- [完整画板索引](../设计总览.html) 可用窗口名查询原尺寸稿。恢复工程的窗口与协议路径见 [场景与窗口归档](../../../docs/interface-pb/windows.md)。
