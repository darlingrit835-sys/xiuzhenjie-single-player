# 修真界 1.2.6 私服服务端

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
- `src/server/game/` Netty TCP 游戏服 + handler
- `data/` JSON 持久化 (users.json 账号, roles.json 角色) + server.p12 TLS 证书
- `lib/` 依赖 (protobuf-java 4.36.2 / netty 4.1.115 / gson)

## 构建与启动

启动前请通过私有渠道取得 `server.p12`，放入服务端数据目录（默认 `server/data/`），并将该证书的口令设为环境变量 `XZJ_TLS_PASSWORD`。

```bat
build.bat     :: 编译
start.bat     :: 启动 (账号服 9443 + 游戏服 17666)
```

## 修改连接地址

1. 改 APK: `patch_dll.py` 用 Mono.Cecil 改 Assembly-CSharp.dll 的
   `Constants.LoginServerIP` / `LoginServerPort` (原 60.205.190.135:443)
2. 重打包: `repack_apk.py` 替换 DLL 并去掉旧签名
3. zipalign 对齐后，使用私有渠道配置的签名证书和口令完成签名。

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
