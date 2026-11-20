# 修真界

修真界 Unity 修仙放置 RPG，支持本地单机和 Android 云端测试，提供 Java 服务端、Go 迁移资料、功能状态与协作开发指南。

## 选择运行方式

| 方式 | 所需文件 | 启动方式 |
| --- | --- | --- |
| 云端联机测试 | 已发放的 Android 测试 APK，当前为 **1.3.5（30）** | 手机联网后直接安装。首次启动从安装包读取完整游戏资源。后续更新支持断点续传。 |
| 本地 USB 单机 | 完整 Windows 本地发行目录，含单机 APK、Java 运行文件和 ADB | 手机开启 USB 调试后连接电脑，运行 `启动游戏.bat`。服务通过 `adb reverse` 转发到手机。 |
| Unity 开发 | Unity **2022.3.62f3**、项目资源和对应的测试服务端 | 在 Unity Hub 打开 `UnityProject/`，运行 `StartLoadScene`。 |

两个 Android 包都要求手机支持 32 位 ARM 应用。本地和云端测试的角色数据各自保存在对应服务端。APK 通过单独的测试或本地发行目录提供。

## 目录

| 路径 | 内容 |
| --- | --- |
| [UnityProject/](UnityProject/) | Unity 客户端工程。包含脚本、场景、预制体、界面素材和 Android 构建入口。 |
| [server/src/](server/src/) | 本地 Java 服务端源码。 |
| [server-go/](server-go/) | Go 服务端的分模块设计、协议迁移记录和已同步的实现文件。 |
| [data/](data/) | 本地服务端配置样例。 |
| [docs/](docs/) | 功能状态、协议缺口、Android 全量热更新及历史单机改造说明。 |
| [scripts/](scripts/) 和 [tools/](tools/) | 旧单机包的参悟计数补丁和 APK 改址工具。 |
| [启动游戏.bat](启动游戏.bat)、[停止服务端.bat](停止服务端.bat)、[重装客户端.bat](重装客户端.bat) | Windows 本地发行目录的启动、停服和重装入口。 |

## 共同开发

- [功能状态与协作待办](docs/功能状态与协作待办.md)
- [Go 协议缺口清单](docs/Go协议缺口.md)
- [协作指南](CONTRIBUTING.md)
- [修仙策划总表（Excel）](server/data/config_excel/修仙属性基础表.xlsx)
- [世界内容总表与玩家创作](docs/content-framework.md) · [怪物数值](docs/monster-stats.md) · [人物、秘籍与法器掉落](docs/character-encounters.md) · [历史投影](docs/historical-projections.md)
- [属性框架和地区产出](docs/cultivation-affinities.md) · [炼丹规则协作草稿](docs/alchemy-design.md) · [画符规则协作草稿](docs/inscription-design.md)

## 使用文档

- [玩家使用说明](使用说明.txt)
- [联机测试与全量热更新](docs/联机测试与热更新.md)
- [Unity 工程入口与模块目录](UnityProject/README.md)
- [Go 服务端迁移说明](server-go/README.md)
- [历史单机改造记录](docs/单机化改造.md)
