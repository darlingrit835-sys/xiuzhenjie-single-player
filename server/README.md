# 修真界单机服务端

本目录是 Java 实现。[Go 服务端](../server-go/README.md) 同样使用现有客户端协议和存档目录。

Unity 1.3.5 客户端与当前 Java 源码的对应关系见 [界面与 PB 归档](../docs/interface-pb/README.md)。下方架构示例保留早期记录，现有处理器与 PB 类型见 [Cmd 索引](../docs/interface-pb/commands.md)。

## 架构

```
客户端 APK (改包: Constants.LoginServerIP=127.0.0.1, LoginServerPort=9443)
   │
   ├─ HTTPS 9443 → AccountServer (账号服)
   │     /login /user/register /user/sendCheckCode /user/pwdUpdate
   │     /user/roleSuicide /announcement/* /gameService/statusSearch
   │     /client/data/queryClientDataModel /client/log/error /login/roleCdKey/*
   │     响应封装: {msgBody:{sysInfo:{httpCode:200},bizInfo:{code:"2000"~"2999",msg,dataString}}}
   │     登录后下发服务器列表 → serverIp/port (游戏服地址)
   │
   └─ TCP 17666 → GameServer (Netty 游戏服)
        帧格式: [varint32 长度][protobuf Msg]
        Msg = { Cmd(1)=int, Msid(2)=int, Data(3)=bytes }
        已实现 handler:
          9000000  心跳
          1002001  RoleCreate   创建角色
          1002002  RoleLogin    角色登录 (49字段完整返回)
          1002003  RoleOffline  离线收益
          1007003  MapChildren  子地图
        其他 cmd 打日志待实现 (协议已全量生成, 见 src/xzj/Xzj.java)
```

## 目录

- `src/xzj/Xzj.java` protoc 生成的 468 个协议消息类
- `src/server/account/` HTTP 账号服
- `src/server/game/` Netty TCP 游戏服、会话和协议分派
- `src/server/game/modules/` 按功能存放 Java PB 专用处理和业务服务，源文件保持 `server.game` 包名
- `data/` JSON 持久化 (users.json 账号, roles.json 角色) + server.p12 TLS 证书
- `lib/` 依赖 (protobuf-java 4.36.2 / netty 4.1.115 / gson)

## 构建与启动

启动前请通过私有渠道取得 `server.p12`，放入服务端数据目录（默认 `server/data/`），并将该证书的口令设为环境变量 `XZJ_TLS_PASSWORD`。

本仓库启动脚本默认选择 Go。需要启动 Java 版时，先退出正在运行的 Go。macOS / Linux 从仓库根目录运行 `XZJ_BACKEND=java ./server/start-local.sh`。脚本会按需编译并监听 `127.0.0.1` 的 `9443`、`17666`、`17667`。存档保存在 `server/data/`。macOS / Linux 的 Java 入口支持用 `XZJ_DATA_DIR` 指定包含配置和证书的独立目录。

```bat
build.bat     :: 编译
set "XZJ_BACKEND=java"
start.bat     :: 启动 Java 账号服、游戏服和战斗服
```

启动脚本默认使用单机轻量档，JVM 初始堆为 32 MiB、最大堆为 256 MiB，启用 Serial GC 并按 2 个逻辑处理器配置默认线程。启动前设置环境变量 `XZJ_JAVA_PROFILE=default` 可恢复 JVM 默认值，已有进程继续使用启动时的参数。

本机隔离对照使用同一份 Java 构建和独立存档，执行两轮登录、建角、查地图、创建副本、进战、施放技能与退出。12 次连续流程后的进程 RSS 从 338–351 MiB 降至 189–198 MiB。结果只覆盖本机单机流程。

## 客户端连接

本仓基线 APK 通过 ADB 反向映射访问本机服务。Unity 工程的位置和本地启动方式见 [仓库说明](../README.md)。云端直连的 HTTPS 与 PB 接入方式见 [Go 服务端说明](../server-go/README.md#云端直连试用)。

## 当前状态

- [x] 协议全量逆向 (468 消息, 与源码 tag 交叉验证 0 错误)
- [x] HTTPS 账号服 + 登录/注册/服务器列表
- [x] TCP 游戏服 + 创建角色/登录/地图 (Python 模拟客户端验证通过)
- [x] 真机/模拟器验证
- [x] 修炼/突破/离线收益
- [x] 黄/玄/地/天 四阶功法（48 本）
- [x] 战斗服（17667）与副本战斗
- [x] 怪物数值重做（练气期可正常战斗，外围无领主）
- [x] 新手引导（1002013 / 1002014）
- [x] 126 条主线任务（1016002 领奖 + 1016001 完成通知）
- [x] 纳戒物品 / 新手礼包 / 功法书使用（1004002 / 1004004 / 1004008 / 1004024）
- [x] 装备与技能基础界面（1009001 / 1009005 / 1009007）
- [x] 9 技能战斗、怪物多技能、战斗掉落与拾取
- [x] 多件装备、套装加成、卸下装备、技能栏配置（1009002/1009003/1009004）
- [ ] 功法/法术/丹方等“创造”黑盒算法
- [ ] 商店/宗门/拍卖行等完整玩法

> 本次修复的详细说明、可调参数、玩法路径见 `FIXES.md`。
