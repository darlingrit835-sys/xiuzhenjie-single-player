# 修真界单机版 · Go 服务端

> 本目录记录 Go 服务端的迁移架构、已同步的协议实现和历史验证。构建需要完整的 Go 工程，其中包含 `go.mod`、协议生成文件和命令入口。云端测试和全量热更见[联机测试与热更新](../docs/联机测试与热更新.md)。当前玩法状态见[功能状态与协作待办](../docs/功能状态与协作待办.md)，接口按 [Cmd 清单](../docs/Go协议缺口.md) 跟进。

原工程服务端是 Java（Netty）那一套：协议 OK 但存档是单文件 `roles.json`，所有模块挤在一起，
加个玩法就要动全局，还经常出现进程静默退出。这里用 Go 重写，目标：

1. **沿用原有协议** —— 直接复用 `server/dev/proto/xzj.proto` 生成 pb 包，客户端 APK 不用再改；
2. **总表套分表** —— 身份/账号/分片路由放总表，业务数据按模块拆成 12 张分表，按角色 ID 哈希分片；
3. **模块化隔离** —— 每个域一个文件、一张表、一组字段，新增玩法只加域，不动其它模块；
4. **一次跑通全链路** —— 账号服、游戏服、战斗服 + 存档落库。

## 目录

```
server-go/
├── proto/xzj.proto            # 从 Java 工程复制，仅追加 option go_package
├── internal/pb/               # protoc 生成（2.38MB，未入库，需自行生成）
├── internal/config/           # 端口、数据目录、开关
├── internal/netx/             # varint32 长度前缀帧，和原 Netty 的 ProtobufVarint32FrameDecoder 一致
├── internal/store/            # SQLite：总表 + 分表 DDL、迁移、分域读写
├── internal/model/            # Role{Data map[string]any} + Num/Str/Bool/MapList 助手
├── internal/gateway/          # 路由、会话、在线表、帧收发、通用兜底回包
├── internal/account/          # HTTPS 9443 账号服（12 条路由，server.p12 复用）
├── internal/battle/           # 17667 战斗服接入
├── internal/game/             # 规则表、新号初始化、离线结算、修炼心跳、各指令 handler
├── cmd/xzjserver/             # 进程入口
└── cmd/probe/                 # 协议级自测探针
```

## 表结构：总表 + 分表

总表（全服一份）：`account`（账号）、`role_index`（角色身份 + 分片路由）、`shard_meta`（schema 版本 / 分片数）、`login_log`（入口日志）。

分表（后缀 `_s0`、`_s1`…，当前 1 片）：

| 表 | 域 |
| --- | --- |
| `role_attrs_sN` | 等级、修为、悟性、体魄、寿元、体力、战力 |
| `role_currency_sN` | 灵石、灵玉、贡献 |
| `role_bag_sN` | 背包，一件一行 `(role_id, uid)` |
| `role_equip_sN` | 装备，一个槽位一行 `(role_id, slot)` |
| `role_kungfu_sN` | 功法，每角色一行 |
| `role_map_sN` | 当前地图 |
| `role_home_sN` | 洞府品阶、房型 |
| `role_farm_sN` | 灵田，一块地一行 `(role_id, land_id)` |
| `role_quest_sN` | 任务与各类计数，每角色一行 |
| `role_signin_sN` | 签到 |
| `role_battle_sN` | 战斗快照，每角色一行 |
| `role_misc_sN` | 兜底 k/v 域，新字段先落这里 |

字段到域的映射集中在 `store/role_repo.go` 的 `domainFields`，新增字段写进对应域即可自动落库。

## 已迁移指令

| cmd | 说明 |
| --- | --- |
| 9000000 | 心跳（不回包） |
| 1002001 | 创建角色 |
| 1002002 | 登录（63 字段，含 BattleGuideVal=100） |
| 1002003 / 1002004 | 离线收益面板 / 领取 |
| 1002005 | 名字查重 |
| 1002018 / 1002021 | 七日签到列表 / 领取 |
| 1001007 | 切换运行功法组合 |
| 1001008 / 1001010 | 突破询问 / 突破 |
| 1007001 | 历练地图（12 格：当前城 + 5 邻城 + 6 片林地） |
| 1007003 | 地图子节点（林地 → 外围/深处/兽王巢） |

早期迁移方案中，其他指令走通用兜底。按 `Resp_<cmd>` 后缀在 pb 注册表找响应类型，返回通用成功码。当前测试服的失败、只读和兼容回包见上述 Cmd 清单。

## 关键修复（对比 Java 版）

- **分表 DDL 与读写不一致**：`role_kungfu` / `role_quest` / `role_battle` 原来是「一条记录一行」，读取却按「每角色一行 payload」做，报 `no such column: payload` 和 `ON CONFLICT clause does not match any PRIMARY KEY or UNIQUE constraint`。已改为每角色一行。
- **int32 数值取成 0**：`model.Num/Lng` 只认 int/int64/float64，`recomputePower` 写的是 int32，战力显示 0。已补齐 int32/float32。
- **数据目录**：原来固定取 exe 同级 `data`，`server.p12` 找不到、9443 起不来。改为优先取工作目录下 `data`。
- **历练地图文案**：格子名统一「林地1…林地6」，等级按 10 级一段（1-10/11-20…51-60），解决格子只有 4 个汉字宽被裁成乱码、等级一律 `[1-999]` 的问题。

## 完整 Go 工程的构建与运行

```powershell
protoc --plugin=protoc-gen-go=$env:GOPATH\bin\protoc-gen-go.exe `
       --go_out=internal/pb --go_opt=paths=source_relative `
       -I proto proto/xzj.proto
go mod tidy
go build -o bin\xjzserver.exe ./cmd/xzjserver
.\bin\xjzserver.exe
```

端口：账号服 HTTPS **9443**、游戏服 **17666**、战斗服 **17667**。首次启动建库 `data/db/xiuzhenjie.db`；`data/server.p12` 用于 HTTPS（密码 `123456`）。

## 自测探针

```powershell
go run ./cmd/probe
```

实测输出（2026-11-18）：

```
[probe] 1002001 创角 Code=2000 Success=true RoleId=cca6400ea005c02aeac130aed68e38da
[probe] 1002002 登录 Code=2000 Success=true id=cca6400e... name=测道君 lv=1 map=112101/新手村
[probe]   灵石=100000 修为=100 悟性=500 体魄=500 战力=11010 引导=100 签到日=0
[probe] 1007001 历练地图 12 格:
   - 云梦城 [1-10] / 楚云城 [11-20] / 碧涛城 [21-30] / 沧浪城 [31-40] / 凌波城 [41-50] / 归雁城 [51-60]
   - 林地1 [1-10] / 林地2 [11-20] / 林地3 [21-30] / 林地4 [31-40] / 林地5 [41-50] / 林地6 [51-60]
[probe] 1007003 林地1 子区域 3 个:
   - 林地1外围 [1-3] / 林地1深处 [4-6] / 林地1兽王巢 [7-10]
```

落库校验（`data/db/xiuzhenjie.db`）：17 张表；`role_index` 1 行；`role_attrs_s0.payload` 含 `CurrentPower=11010`；`role_currency_s0.payload` 为 `{"ShinyJade":0,"ShinyStone":100000}`。

## 待办

- 按[功能状态与协作待办](../docs/功能状态与协作待办.md)维护模块进度和验收范围。
- 分片数 >1 的补建与迁移入口已在 `store` 留好（`XZJ_SHARDS`）。
