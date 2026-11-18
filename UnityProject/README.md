# 修真界 Unity 工程

用 Unity Hub 添加本目录，使用 Unity **2022.3.62f3** 打开 `Assets/Scenes/StartLoadScene.unity`。账号登录与角色进度由仓库中的本地 Java 服务处理，Unity 连接 `127.0.0.1:9443`。

水墨地图、城池、按钮和五行素材在 `Assets/Resources/InkInterface`。地图保留地名与等级和导航逻辑。建角页为五种灵根提供中文名称、分配规则和对应的选中动画。`Assets/Resources/InkPortraits` 包含 12 个保持原存档编号的玩家头像及离线版本。

Android 测试打包入口为 `RecoveryAndroidBuild.Run`，目标 SDK 为 36。目前依赖 ARMv7 版 xLua 与 SQLite，使用原本的测试包标识及本机调试签名。
