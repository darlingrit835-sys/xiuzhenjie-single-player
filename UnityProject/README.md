# 修真界 Unity 工程

用 Unity Hub 添加本目录并选择 **Unity 2022.3.62f3**。打开 `Assets/Scenes/StartLoadScene.unity` 进入登录场景。仓库中的本地开发配置连接账号服 `127.0.0.1:9443`，服务端准备方法见[玩家使用说明](../使用说明.txt)。

## 模块目录

| 路径 | 用途 |
| --- | --- |
| `Assets/Scenes/` | 启动、主城和历练等游戏场景。 |
| `Assets/Scripts/Assembly-CSharp/` | 客户端玩法、窗口和网络相关的 C# 脚本。 |
| `Assets/gamedata/` | 游戏预制体、材质和资源。 |
| `Assets/Resources/InkInterface/` | 水墨地图、城池、按钮和五行素材。地图保留地名、等级和导航。 |
| `Assets/Resources/CatalogIcons/` | 怪物与功法无字图标、导表生成的取图索引。战斗头像、宗门兽潮和功法列表按 ID 选图。 |
| `Assets/Resources/InkPortraits/` | 12 个玩家头像及离线样式，沿用原编号。 |
| `Assets/StreamingAssets/Assetbundles/` | 本地发行配置使用的恢复版资源包。 |
| `Assets/Editor/` | Android 构建和编辑器验证入口。 |
| `Packages/` 与 `ProjectSettings/` | Unity 依赖和工程设置。 |

建角页带五行名称、分配规则和选中动画。城内快捷操作图标保留文字说明。修改素材时把对应的 `.meta` 一起提交，保持场景和预制体引用。

## Android 构建

仓库中的本地测试入口为 `RecoveryAndroidBuild.Run`，目标 SDK 为 36，使用本机调试签名。APK 依赖 ARMv7 原生库。

云端全量热更新通过完整测试发行工程生成，使用独立的启动基线和签名频道。发布步骤见[联机测试与全量热更新](../docs/联机测试与热更新.md)。
