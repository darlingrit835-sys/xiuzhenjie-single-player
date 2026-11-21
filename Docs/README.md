# 修真界客户端维护手册

当前开发工程是 `xiuzhenjie-unity-recovery/UnityProject`。Android 联调使用 Unity 2022.3.62f3。场景从 `Assets/Scenes/StartLoadScene.unity` 启动。

按玩家流程查具体功能、窗口、Go 处理和回归点时，从 [游戏流程与模块索引](../../../xiuzhenjie-single-player/docs/gameplay.md) 进入。

## 模块入口

| 模块 | 从哪里改 | 职责 |
| --- | --- | --- |
| 启动与界面装配 | `Assets/Scripts/Assembly-CSharp/XiuZhenJie/Account` 和 `Common/UIManager.cs` | 初始化资源、建立界面根节点和注册窗口。窗口由注册名定位控制器 |
| 玩法和界面 | `Assets/Scripts/Assembly-CSharp/XiuZhenJie/<模块>`，`Assets/gamedata/prefabs/uiprefabs/window/<模块>` 和 `item/<模块>` | 按登录、主线、地图、洞府、纳戒、战斗、宗门、生产和商店维护 C#、窗口与列表项 |
| 启动资源与水墨素材 | `Assets/Resources/prefab`、`InkInterface`、`InkPortraits`、`InkCultivation` | 启动页面和当前随包发布的共用图稿。跨界面素材从此复用 |
| 游戏内容 | `Assets/gamedata`、`Assets/StreamingAssets/Assetbundles`、`Assets/Scripts/Assembly-CSharp/XiuZhenJie/Content` | 编辑用源资源、Android 种子包、启动与窗口资源提供方 |
| 协议与连接 | `Assets/Scripts/Client/Protocol` 与 `Assets/Scripts/Assembly-CSharp/XiuZhenJie/Network` | 独立程序集负责 Protobuf 长度前缀编码。恢复层负责 HTTPS、套接字连接及消息分派 |
| 构建与原生依赖 | `Assets/Editor/Build`、`Assets/Plugins`、`ProjectSettings` | 构建入口、目标检查、SDK 及其导入设置 |
| 服务端 | 同级服务端仓库的 `server-go` 和 `server` | Go 是当前本地运行入口，Java 用于历史对照。界面到 Cmd 和处理器的关系以 [PB 归档](../../../xiuzhenjie-single-player/docs/interface-pb/README.md) 为准 |

新增与旧玩法无关的基础能力可以放进 `Assets/Scripts/Client/<能力>`。独立程序集显式声明依赖，入口留在启动、网络或界面装配层。现有窗口、组件和生成的 PB 消息继续使用恢复时的程序集身份。移动这些类型前，先核对场景、Prefab、原始 AssetBundle 和反射查找里的类型引用。资源改名或移动时同时维护 `.meta` 和原逻辑键。 [Unity 程序集定义](https://docs.unity3d.com/2022.3/Documentation/Manual/ScriptCompilationAssemblyDefinitionFiles.html)

`GameStart` 负责选择 `IContentBootstrap` 和 `IWindowContent` 的实现。前者准备随包文件并挂载索引，后者承接窗口创建、预加载、图标和回收。当前两者使用 `RecoveredBundleContent`。战斗、物品等业务仍有直接访问旧 `Resource.dll` 的路径，迁移时按实际调用逐步接入。

启动、登录和转场共用 `Account/StartupArtwork.cs` 的山水背景与飞鹤。舆图和洞府可从 `Resources/InkInterface`、`Resources/InkCultivation` 查找。五行灵气只读取当前用到的图案。Android 的地图和洞府不透明大图使用 ETC2 RGB，启动图保持原分辨率，源图与 GUID 随工程维护。旧启动稿存放在 `Assets/ArtArchive`。

随包资源在加载画面显示后开始校验与准备。本机和直连试用版回登录时，可以复用同一进程里已挂载的资源索引，每次冷启动仍会校验包文件。

## Git、测试和产物

- Unity 源码沿用既有 GitHub 仓库的 `UnityProject/` 布局，维护 `Assets` 及其 `.meta`、`Packages`、`ProjectSettings` 和本手册。同步前以目标分支的 Unity 子树为基准查看差异。
- `Builds` 保存 APK 等交付物，建议新包放到 `Builds/Android/<渠道>/<构建号>/`。`RecoveryData` 存恢复原料，`RecoveryLogs` 存验证记录。当前本机联调包标识为 `com.LMGame.XiuXianJie.recovery`，腾讯云试用和邮箱试用各有独立包名及配置。
- `Library`、`Temp`、构建输出和 IDE 文件可重新生成。设备联调用独立测试账号，后端集成测试使用另存的测试数据和独立端口。运行账号、角色存档、证书和签名材料保存在各自环境。

## 构建入口

在 `UnityProject` 根目录执行。将 `XZJ_UNITY` 指到对应编辑器可执行文件。预检输出中的 `CLIENT_BUILD_PREFLIGHT` 记录目标、检查项和结果。

```sh
mkdir -p "$PWD/../RecoveryLogs"
"$XZJ_UNITY" -batchmode -quit -buildTarget Android -projectPath "$PWD" \
  -executeMethod ClientBuildPreflight.Android -logFile "$PWD/../RecoveryLogs/Android-Preflight.log"
"$XZJ_UNITY" -batchmode -quit -projectPath "$PWD" \
  -executeMethod ClientBuildPreflight.OpenHarmony -logFile "$PWD/../RecoveryLogs/OpenHarmony-Preflight.log"
```

Android 打包入口继续使用 `RecoveryAndroidBuild.Run`。运行时传入 `XIU_RECOVERY_APK` 的绝对输出路径并指定 `-buildTarget Android`。构建使用 API 36、IL2CPP、ARMv7、OpenGL ES 3。首次构建前需为当前工程安装 HybridCLR。

```sh
XIU_RECOVERY_APK="$PWD/../Builds/Android/local/21/xiuzhenjie-1.3.5-21.apk" \
  "$XZJ_UNITY" -batchmode -quit -buildTarget Android -projectPath "$PWD" \
  -executeMethod RecoveryAndroidBuild.Run -logFile "$PWD/../RecoveryLogs/Android-Build.log"
```

## C# 代码热更新

工程使用 HybridCLR 9.0.3。`Assets/HotUpdate/XZJ.HotUpdate.asmdef` 是可更新代码的程序集，首个接入点为师徒演出字幕和对话。旧 xLua 文件和 Lua 资源包已退役。现有随包 AssetBundle 流程保留，首次解包边写边校验 274 个资源包。

1. 在 Unity 切到目标平台，然后运行「修真界 → 代码热更新 → 1 安装项目内 HybridCLR」和「2 生成随包基线」。生成目录是 `Assets/StreamingAssets/HotUpdate`，包含 `manifest.json`、`XZJ.HotUpdate.dll.bytes` 和匹配该包的 `AOT/` 元数据。Android 构建入口会自动重新生成基线。保存每个已发布基线的清单，用于给同一批主包导出补丁。
2. 如果需要远程分发，在构建主包前把 HTTPS 清单地址与发布公钥写入随包 `config.json`。私钥仅供发布端签名。运行时先选择已验证的本机补丁或随包 DLL，再在后台检查更新。下载并验证签名和 DLL 的 SHA-256 后，新版本下次启动生效。应用启动与登录无需等待网络更新。
3. 修改 `Assets/HotUpdate` 后，在目标平台导出一个比渠道现有版本更高的补丁，签名后将 DLL、签名、清单放在 HTTPS 同一目录，最后切换清单。只部署已导出的三个文件。涉及 AOT 接口、主包代码或资源结构变化时，重新生成并发布对应的主包基线。

```sh
python3 Tools/hot_update.py configure \
  --public-key /absolute/path/update-public.pem \
  --manifest-url https://updates.example.com/xzj/android/manifest.json \
  --output Assets/StreamingAssets/HotUpdate/config.json

XIU_HOTUPDATE_BASELINE=/absolute/path/published-baseline/manifest.json \
XIU_HOTUPDATE_OUTPUT=/absolute/path/patch-2 \
XIU_HOTUPDATE_VERSION=2 \
  "$XZJ_UNITY" -batchmode -quit -buildTarget Android -projectPath "$PWD" \
  -executeMethod ClientHotUpdateBuild.ExportPatch -logFile /absolute/path/patch-export.log

python3 Tools/hot_update.py sign \
  --patch-dir /absolute/path/patch-2 --private-key /absolute/path/update-private.pem
```

本地验收已生成 Android IL2CPP 基线和补丁。在 Unity Play Mode 验证了 HTTPS 签名下载、断开测试服务后重启读缓存，以及用正式导出入口产出的 DLL 替换师徒台词。源码的通用配置默认留空，腾讯云直连包生成时注入此试用频道的地址和公钥。 [Android 基线日志](../../RecoveryLogs/Unity-HybridCLR-AndroidBaseline-Repaired.log) · [下载验签日志](../../RecoveryLogs/Unity-HybridCLR-PlayMode-HTTPS.log) · [导出补丁加载日志](../../RecoveryLogs/Unity-HybridCLR-PlayMode-ExportedPatch.log)

腾讯云直连试用包 1.3.5（27）预置 `https://82.156.174.166/_xzj/hot/android/manifest.json`。安装后联网打开游戏，登录或选服页底部显示 `v2 已下载，重启生效` 时，从后台结束游戏进程并重新打开，版本应为 `热更新 v2`。师徒传功演出字幕随 v2 改为「金息入体」。后续打该频道的补丁应以该 APK 内的基线清单为输入。 [APK 基线清单](../../RecoveryLogs/Tencent-Android-1.3.5-27-baseline.json) · [APK 核验](../../RecoveryLogs/Unity-Tencent-27-APK-Verification.txt)

腾讯云直连试用包 1.3.5（28）临时停用练功房的内视入口和强制经脉引导。新主包从 `XZJ.HotUpdate.ClientFeatures.MeridianEntryEnabled()` 读取入口开关，加载失败时保持关闭。经脉场景完成目标设备验证后，可以给 28 版基线发布签名补丁，重新开放入口。 [28 版基线清单](../../RecoveryLogs/Tencent-Android-1.3.5-28-baseline.json) · [Unity 练功房验证](../../RecoveryLogs/Unity-Meridian-Pause-PlayMode.log)

## 原生鸿蒙构建

1. 先在隔离的工程副本中用带 OpenHarmony 构建模块的团结引擎打开项目。确认目标设备对应的 SDK、IL2CPP 后端和 ABI，保留 Android 工作树的设置。 [团结引擎环境与导出](https://docs.unity.cn/cn/tuanjiemanual/Manual/openharmony-sdksetup.html) · [OpenHarmony Player 设置](https://docs.unity.cn/cn/tuanjiemanual/Manual/class-PlayerSettingsOpenHarmony.html)
2. 按目标平台重建种子资源和索引。验证团结引擎对应版本的 HybridCLR、IL2CPP 基线、SQLite、广告和 WebView。现有 `libsqlite3.so` 是 Android ARMv7 的恢复产物，鸿蒙需使用对应 SDK 架构的原生实现。 [OpenHarmony 原生插件](https://docs.unity.cn/cn/tuanjiemanual/Manual/openharmony-native-plugins-introducing.html)
3. 导出到 DevEco Studio，使用独立签名。用鸿蒙设备验证首次安装、资源初始化、登录、读档、场景切换和实际输入，再接远程内容与平台 SDK。预检日志用来定位缺项，鸿蒙包以签名导出和设备回归为验收依据。 [团结引擎 DevEco 流程](https://docs.unity.cn/cn/tuanjiemanual/Manual/openharmony-sdksetup.html)

## Android 全量热更新

新版 Android 在线测试包使用 `ClientBootstrap` 启动。签名发布同时包含 `Assembly-CSharp`、项目 PB 协议程序集、师徒和经脉开关程序集、各游戏场景、UI 预制体和资源，以及恢复版资源库。首次启动在加载页校验并解压安装包内置版本。之后在同一加载页读取频道清单。版本号未增加时直接使用通过安装校验的本地资源，网络不可用时也可加载上一次已安装的版本。新版本仍需签名和摘要校验，分段下载中断后可续传。

### 建立测试基线

1. 通过 `Tools/hot_update.py configure` 生成启动频道配置。测试服直连使用 `--manifest-url`、`--public-key`、`--certificate-sha256` 和 `--gateway-capability-file`。云端频道默认使用邮箱注册。需要旧注册方式时，传入 `--no-use-email-registration`。签名私钥保持在发布电脑，频道配置放在项目外。
2. 用 Unity 2022.3.62f3 执行 `RecoveryAndroidBuild.RunFull`，指定 `-buildTarget Android`。传入 APK 绝对路径 `XIU_RECOVERY_APK`、频道配置绝对路径 `XIU_FULL_UPDATE_CONFIG`、递增版本号 `XIU_ANDROID_VERSION_CODE`，以及 `XIU_HOTUPDATE_VERSION`、`XIU_HOTUPDATE_OUTPUT`、`XIU_FULL_UPDATE_SIGNING_KEY`。构建会导出并嵌入完整签名版本。保存热更输出目录中的 `base.json`。
3. 继续发布同基线热更时，用 Unity 执行 `ClientReleaseBuild.ExportRelease`。传入上一步生成的 `XIU_FULL_UPDATE_BASELINE`、递增热更版本号 `XIU_HOTUPDATE_VERSION` 和空目录 `XIU_HOTUPDATE_OUTPUT`。
4. 用 `Tools/hot_update.py sign-full --patch-dir <导出目录> --private-key <发布私钥>` 生成签名。部署时将 `client-release.zip` 放在 `releases/<archiveSha256>/` 下，再发布 `manifest.sig` 与 `manifest.json`。网关通过 `-full-hot-update-dir` 提供整个频道。每个新的 APK 基线使用 `channels/<APK 版本号>/` 下的独立目录。
5. 使用 HTTPS 读回三个文件并核对 SHA-256，然后重启客户端检查下载流程、登录、建角和进城。

同一个 Android 基线可以发布多次玩法、界面和资源更新。Unity 引擎、IL2CPP 原生层、Android 插件、启动图标，以及启动频道的地址和证书指纹随 APK 发布。

### APK 32 起按文件更新

线上完整安装包从 APK 32 起使用 Android ARM64。对应原生库位于 `Assets/Plugins/Android/arm64-v8a/libsqlite3.so`，由 SQLite 3.50.4 源码编译。 [SQLite 源码校验值](https://www.sqlite.org/releaselog/3_50_4.html)

安装包内仍带完整种子。联网更新验证签名清单后，会按文件大小和 SHA-256 复用已安装资源，仅下载本地缺少或校验失败的文件。已完成的下载分段可以续传。校验完成后切换游戏版本，更新中断时启动已安装版本。

对 APK 32 及后续基线，签名后执行 `python3 Tools/hot_update.py export-files --patch-dir <导出目录> --objects-dir <对象目录>`。把对象目录发布至对应频道的 `objects/`，按摘要保留旧文件供下载中的客户端使用。确认清单所列对象均可读取后再发布 `manifest.sig` 与 `manifest.json`。发行 ZIP 保留给打包和签名校验，客户端从每个文件的 `objects/<sha256>` 地址下载。APK 31 及以前的频道继续使用发行 ZIP。

### Android 启动图标

图标位于 `Assets/Brand/LauncherIcon.png`，使用内置 ImageGen 生成。

> Use case: logo-brand. Asset type: a full-bleed square Android launcher icon image for the Chinese cultivation RPG 修真界. A single white ink-painted crane with spread wings, seen in profile in upward flight, above one bold jade-green Chinese mountain peak, framed by a soft circular sweep of pale spring-jade ink wash on warm ivory Xuan paper. A tiny vermilion seal-red sun disk near the mountain offsets the greens. Sophisticated bright Chinese shan shui ink-wash, brush grain only on larger shapes, editorial restraint, crisp high-contrast elegant crane silhouette, calm luminous ivory and celadon. Strong central composition with crane and peak wholly in the innermost 60 percent so it reads at 48 by 48 pixels and survives adaptive launcher masks. Ivory brush-wash background fills all four edges without a border or visible rounded corners. No writing, no glyphs, no watermarks, no people, no mockup. 1:1 square.
