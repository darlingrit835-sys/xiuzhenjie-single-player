# 修真界单机版

本仓库保存 Go 服务端、Java 回退实现、Windows 本地启动脚本、基线安卓包和接口文档。

## 文档入口

| 要查的内容 | 入口 |
| --- | --- |
| 玩家流程、模块修改入口与回归点 | [游戏流程与模块索引](docs/gameplay.md) |
| 客户端源码位置和内视补丁 | 下方「Unity 工程与 Git」 |
| 功能目录对照、文档与版本来源 | [界面与 PB 归档总览](docs/interface-pb/README.md) |
| 从玩家操作查到 Unity 窗口和 Java 处理 | [玩法流程](docs/interface-pb/flows.md) |
| 从窗口或界面子组件查 C# 和 Cmd | [场景与窗口归档](docs/interface-pb/windows.md) |
| 从 C# 脚本查调用位置、连接与回包 | [客户端网络调用](docs/interface-pb/calls.md) |
| 从 Cmd 查 PB 消息、调用方与 Java 处理 | [PB Cmd 与服务端处理](docs/interface-pb/commands.md) |
| 从 Unity 调用点查 Go 的功能与试用回包 | [Unity 与 Go 接口对应表](docs/interface-pb/go-interfaces.md) |
| 启动 Go 后端或运行对照测试 | [Go 服务端](server-go/README.md) |
| 启动、重装与排查 | [玩家使用说明](使用说明.txt) · [服务端启动说明](server/README.md) |

## 本地启动

- macOS / Linux 在仓库根目录运行 `./server/start-local.sh`。安装 Go 1.26 或更新版本。需要运行 Unity 时，用 Unity 2022.3.62f3 打开同级工程的 `StartLoadScene` 并进入 Play Mode。
- Windows 双击 `启动游戏.bat`，脚本启动 Go，通过 ADB 建立 USB 映射并打开仓库附带的本地 APK。手机直连云端使用单独构建的试用包，安装后可通过网络连接。
- 需要回退 Java 时先停止 Go。macOS / Linux 运行 `XZJ_BACKEND=java ./server/start-local.sh`。Windows 在同一个 PowerShell 窗口执行 `$env:XZJ_BACKEND = 'java'` 后运行 `.\start.ps1`。两版共用存档格式和默认端口。
- 测试试用玩法时使用独立存档，并启用 `-interface-preview -interface-writes`。并行测试为账号、游戏和战斗各指定空闲端口。点穴与突破的本机验收结果见 [功能记录](docs/interface-pb/social-status.md#用通脉所得经脉点突破穴位)。

本机存档默认写入 `server/data/`。新检出初次运行用空账号和角色数据，可在客户端注册和建角。备份与迁移请先停服，再备份完整存档目录。仓库保留运行用配置表与本地开发证书。账号、角色、宗门、聊天、历史、部署配置与云端证书按各环境单独备份。

## Unity 工程与 Git

Unity 1.3.5 工程在本机同级目录 `../xiuzhenjie-unity-recovery/UnityProject`，其维护手册为 `Docs/README.md`。请从已有工程备份恢复该目录后再查看文档中的 Unity 源码链接。构建包位于该工程旁的 `Builds`，本地验收日志位于 `server-go/.build`。普通 Git 克隆包含本仓的代码与接口表，这两处本机目录由各自的备份提供。

本次内视修改随仓提供 [C# 补丁](client/patches/neishi-acupoint.patch)。补丁针对已有内视余额读取、尚未接入点穴写入的客户端基线。用自己的 Unity 工程路径替换下方路径，从本仓根目录先检查，再应用。

```sh
patch_file="$PWD/client/patches/neishi-acupoint.patch"
git -C ../xiuzhenjie-unity-recovery/UnityProject apply --check "$patch_file"
git -C ../xiuzhenjie-unity-recovery/UnityProject apply "$patch_file"
```

当前主仓从本机 Git bundle 导入。同级 `xiuzhenjie-single-player-public` 是独立检出，有自己的历史与远端。提交前用 `git rev-parse --show-toplevel` 确认目标目录，用 `git diff --cached --stat` 确认待提交范围。
