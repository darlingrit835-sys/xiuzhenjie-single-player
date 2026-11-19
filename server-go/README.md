# 修真界 Go 服务端

Go 后端与现有 Unity 1.3.5 客户端使用同一套账号服 HTTPS、游戏服 TCP 和战斗服 TCP 协议。代码从当前 Java 实际使用的 `Xzj.java` 导出 PB 描述符，保留 `Msg.Cmd`、`Msid`、`Data`、字段编号和 varint32 帧。Go 在运行时加载内嵌描述符。 [Go dynamicpb](https://pkg.go.dev/google.golang.org/protobuf/types/dynamicpb)

## 启动

源码构建需要 Go 1.26 或更新版本。编译出的可执行文件可以独立运行。

| 平台 | 从仓库根目录启动 |
| --- | --- |
| macOS / Linux | `./server/start-local.sh` |
| Windows USB 客户端 | 双击 `启动游戏.bat` |
| Windows 只启动后端 | `.\server-go\start.ps1` |

本机已构建 macOS arm64 二进制，Linux amd64 和 Windows amd64 交叉编译也已通过。

`./server-go/start.sh` 可直接启动 Go。共有入口可通过环境变量 `XZJ_BACKEND=java` 临时选择 Java。Windows 的 `停止服务端.bat` 只处理本仓库后端在默认端口的进程。

默认监听本机 `9443`、`17666` 和配置中的战斗端口（默认 `17667`），并使用 `server/data`。需要独立联调时传入 `-data` 和三个独立端口。账号与客户端选服地址由 `-public-ip` 或 `config/server.json` 指定。启动脚本也接受 `XZJ_DATA_DIR`。启动前请通过私有渠道取得 `server.p12`，放入所选数据目录，并将该证书的口令设为环境变量 `XZJ_TLS_PASSWORD`。

Go 直接读取存档目录内的 `server.p12`、`users.json`、`roles.json` 和 `config`。旧 GBK JSON 可读取，后续保存为 UTF-8。同一账号有多个角色时，沿用 Java 按存档记录顺序选角的行为，保存和重启后保留该顺序。切换实际存档时，先让当前后端退出，再用另一版打开原存档目录。并行联调为两版各准备一个数据副本。

## 客户端兼容验证

Go 版提供对应 Java 注册的 51 个游戏服 Cmd、9 个战斗服分支和 12 条 HTTPS 路由。Unity 2022.3.62f3 在独立工程和存档上完成了登录、选服、校验角色名、建角、进入主城、进入副本和退回主城。Java GBK 存档可被 Go 打开，Go 保存后的 UTF-8 存档可由 Java 再次登录原角色。 [性能对照与兼容验证](BENCHMARK.md)

## 源码入口

| 功能 | 文件 |
| --- | --- |
| 监听、生命周期与端口 | [main.go](main.go) |
| TCP 帧、连接与回包 | [transport.go](transport.go) · [protocol.go](protocol.go) |
| HTTPS 账号接口、PKCS#12 和 JSON 存档 | [account.go](account.go) · [store.go](store.go) |
| 登录建角、地图和玩法 | [game.go](game.go) · `game_*.go` |
| 副本、怪物、战斗和掉落 | [battle.go](battle.go) · `battle_*.go` |
| 独立试用模块与接口目录 | [preview.go](preview.go) · [商店活动](preview_commerce.go) · [社交宗门](preview_social.go) · [灵田生产](preview_crafting.go) |

## 接口对应表与试用

[Unity 与 Go 接口对应表](../docs/interface-pb/go-interfaces.md) 根据前端调用点、回调和 Go 协议目录生成，记录请求类型、响应类型、界面读取的字段及现有实现。生成器的 `--check` 可用于检查表格是否落后于代码。

试用模式通过 `-interface-preview` 提供玄天楼、奇珍阁、拍卖会、榜单、签到页、传信、宗门详情、灵田、种子和灵土等查询回包。只读试用按客户端使用的 PB 类型拒绝写入，保留原 Cmd 与 Msid。活动入口依旧受客户端的活动配置控制。

隔离启动示例从仓库根目录执行。用对应的 Unity 测试副本把登录端口指到 `19443`，游戏和战斗端口由服务器下发。

```sh
trial_data=$(mktemp -d)
cp -R server/data/config "$trial_data/config"
cp server/data/server.p12 "$trial_data/server.p12"
./server/start-local.sh -interface-preview -data "$trial_data" \
  -account-port 19443 -game-port 19666 -battle-port 19667
```

写入试用在同一条启动命令中加上 `-interface-writes`，并使用独立存档。HTTPS 登录签发限时账号凭证，客户端用现有 PB 字段回传，服务端核对待登录角色的账号归属。试用服的模拟短信找回密码关闭。

拍卖行可购买一件 NPC 挂牌。拍卖会按境界分场，支持多角色竞价。受理出价时扣除当前最高价的押金，被超价后退回原押金，首个有效出价后 45 秒结算。到期发货或退款和角色余额在 `roles.json` 中一次提交，拍卖状态保存在保留键 `$preview:auction` 下。

宗门试用的 `guilds.json` 使用 `world.Guilds` 按宗门 ID 保存记录。每条记录可包含 `Name`、`ShortName`、`MapId`、`LeaderId`、`Members` 和 `Applications`。初始化时将独立存档中的掌门角色 ID 放入 `LeaderId` 和 `Members`，申请记录用空对象。宗门写入集中在该文件，登录和宗门界面从同一份成员记录读取归属。

好友和师徒走原游戏连接。角色可以按名字发送名刺，对方在「拜会名刺」中同意后，双方的道友列表同时更新。试用服为每个角色保留最多 200 位好友和 200 份待处理名刺。

### 道友传书和传物

开启写入试用后，新版直连客户端从「道友」选择好友即可传书或传物。传书走已验证身份的游戏 PB 连接。服务端先保存消息，再给发送者回执、向在线收件人推送。离线好友下次打开窗口可读取同一份会话记录。每条消息最多 2048 个 UTF-8 字节，每页读取 100 条，较早的消息通过「查看更早的传书」翻页。

| Cmd | 用途 |
| --- | --- |
| `4001007` | 发送原 `SendMessageRoleReq_4001007`，回执返回服务端消息时间 |
| `4001008` | 在线推送，`RoleId` 是发件角色 ID，`MsgTime` 是该会话内唯一递增的毫秒时间 |
| `4901001` | 查询本人与目标好友的传书。请求字段 2 为目标角色字符串，字段 3 为可选的排他性毫秒游标。响应字段 1、2 为状态码和成功标记，字段 3 为按时间升序排列的 `ReceiveMessageRoleResp_4001008` 列表，字段 4 表示还有更早的记录 |
| `1014005` | 传送所选物品。服务器校验好友关系、物品、数量、绑定与锁定状态、纳戒空间和手续费，将双方库存与灵石一起提交 |

聊天数据库位于 `<data>/chat/messages.db`，按双方角色 ID 建立会话索引。备份时在停服后复制整个存档目录，包括 `chat/` 和 `history/`。消息正文只写入聊天数据库。传物在游戏历史中记录发送人、接收人、物品数量与手续费。一次可选 1–5 类物品，手续费按物品品阶、境界档和数量计算，并应用发送方已拥有称号的最高传物折扣。

### 师徒修行

在「道友 → 师徒」输入师父名字递帖，师父在「拜师帖」中收徒。师父须高出徒弟 11 个或更多小境界，同时最多收 3 位弟子。试用服每个角色已发、已收的待处理拜师帖各限 200 份。收徒成功后，弟子递给其他师父的待审帖自动撤回。双方关系和历史事件在同一次角色存档事务中提交。旧存档在首次社交写入时自动添加相应字段。具体操作、验证结果和后续功能见[道友与待实现玩法](../docs/interface-pb/social-status.md)。

弟子可通过 `1014015` 每日请安一次。师尊离线、在线、在线且同城时分别增加 5、10、20 点亲密度。每日以北京时间午夜为界，服务器将师徒亲密度和请安记录一并写入存档，双方读取同一数值。

| 原客户端操作 | 服务端处理与存档 |
| --- | --- |
| 弟子列表 → 传功（`1014017`） | 师尊消耗 5 精力。试用服按弟子等级表 `increaseSuperAir` 取整确定修为，双方在线同城翻倍。弟子领取时增加 `SuperAir`，通过 `9001004` 同步当前修为。 |
| 弟子列表 → 通脉（`1014016`） | 亲密满 100 可用，消耗师尊 20 精力。弟子领取 1 点经脉点，双方在线同城时为 2 点。洞府内视（`1005016`）读取实际余额。 |
| 打开师徒 → 领取（`1014025`） | 同一弟子的两种修行每日共享一次机会。在线提醒使用 `1014024`，离线或跨天由 `1014012` 读回待领数。未领取的修行一直保留，换师或出师后仍可领取一次。 |
| 弟子列表 → 管理 → 出师（`1014027`） | 亲密满 300 后可出师，师尊空出名额并增加 50 气运。双方保存对方身份、时间与最终亲密。师父通过 `ChuShiNum` 查询累计人数，弟子通过 `1014012` 新增的可选字段 20（师父道号，string）与 21（最终亲密，int32）查看最近一次出师。 |

传功和通脉要求师尊至少比弟子高一个大境界，传功还需相差至少 10 个小境界。指导时增加的亲密与请安采用相同的离线、在线、在线同城三档。首次获得师徒称号时，服务端按原称号表增加气运。原表未提供人数门槛，试用版按累计 1、5、10 名不同的出师弟子解锁传道授业、桃李满门、德高望重，各增加 200、500、1000 气运。重复出师不会重复发奖。

### 内视点穴与突破

「洞府 → 练功房 → 内视」使用 `1005016` 加载余额与已通穴位，使用原 `1005018` 提交当前穴位 ID。服务端从存档目录的 `config/b.json` 一次加载基础消耗和穴位前驱。首穴可直接点通，其他穴位至少要有一个已通前驱。已通穴位可升至三层。

三层费用依次为基础值的 1、3、5 倍，基础成功率依次为 100%、50%、10%。后二层使用角色拥有的最高经脉突破称号加成。有效尝试先扣点再判定，失败保留层数。余额不足、穴位锁定或满层时保留经脉点。角色存档事务同时保存余额、穴位层数和对应历史。点穴写入使用 `-interface-preview -interface-writes`。

试用路由启动时核对 Cmd 和 PB 描述符。注册表按 Cmd 查找，预编码固定回包。只读模块通过 `build` 处理，按 `roleFields` 读取需要的角色字段。接入持久状态的路由另外提供 `live` 处理函数，业务更新在各自模块提交后才回包和推送。生成器根据注册表更新接口表。

```sh
python3 docs/interface-pb/refresh_go.py
cd server-go
go test -race ./...
go vet ./...
go test -run '^$' -bench '^BenchmarkPreviewPB$' -benchmem
```

Apple M4 Pro 上的进程内微基准，固定回包三轮为 70.75–73.00 ns/次、64 B/次、1 次分配，带参数的灵土查询为 2.84–2.89 µs/次、2796 B/次。该测试包含 PB 回包和帧编码，写入丢弃连接以隔离网络 I/O。Unity Play Mode 使用隔离账号进主城后打开传信窗口，渲染了演示邮件及详情，领取按钮禁用。

## 邮箱账号

Go 启动时增加 `-email-registration` 启用邮箱注册。Unity 客户端通过 `Constants.UseEmailRegistration` 打开对应界面。已有手机号账号沿用原密码和角色 ID，邮箱匹配忽略大小写，新邮箱账号的密码以 bcrypt 哈希保存。

客户端先调用 `POST /user/sendEmailCode` 并传入 `email`。SMTP 中继接受邮件后，客户端提示检查邮箱。验证码为六位，仅用于该邮箱注册，10 分钟有效。用户通过 `POST /user/register` 提交 `email`、`password` 和 `checkCode`。同一邮箱的发送间隔为 60 秒，每份验证码最多验证五次，注册成功后不能再次使用。

登录使用 `POST /login`。邮箱账号使用 `email` 和 `password`，原手机号账号使用 `mobilePhone` 和 `password`。角色登录继续沿用原 PB 字段传递账号凭证。先前邮箱试用中保存的未验证账号可在注册界面补验，服务器保留用户与角色的关联。

发信配置由服务进程读取以下环境变量。发件邮箱使用邮件服务允许的地址，授权凭据放在部署目录的私有配置中。

| 环境变量 | 配置 |
| --- | --- |
| `XZJ_SMTP_HOST` | SMTP 服务主机名 |
| `XZJ_SMTP_PORT` | 默认 `465`，也可使用 `587` |
| `XZJ_SMTP_SECURITY` | `tls` 或 `starttls`，默认根据端口选择 |
| `XZJ_SMTP_FROM` | 发件邮箱 |
| `XZJ_SMTP_USERNAME`、`XZJ_SMTP_PASSWORD` | 中继认证账号与授权码，按提供商要求配置 |

发信配置缺失或投递失败时，客户端会收到发信失败提示，注册仍要求有效的邮箱验证码。邮箱注册与找回密码是独立流程，当前只接入注册。

## 记住登录

本机和证书指纹固定的云端直连客户端在 `POST /login` 发送 `rememberLogin=1`。成功响应在原登录数据中附加 `rememberToken` 和 Unix 秒格式的 `rememberTokenExpiresAt`。

再次打开客户端时，`POST /user/session/resume` 使用 `userId` 和令牌取得最新选服列表，并按当前服务配置签发游戏会话。凭据有效期为 30 天，失效时返回 `2105`。`POST /user/session/logout` 使用相同参数撤销当前凭据。服务端只持久化凭据的摘要，每个账号最多保存五份。

## 云端直连试用

直连包从手机访问云端 HTTPS 入口。[xzj-ingress](cmd/xzj-ingress) 把 HTTPS 账号请求转给回环账号服。游戏和战斗各使用一条 TLS 连接，通过 `/_xzj/game` 和 `/_xzj/battle` 升级为原 PB 字节流。客户端校验服务端证书指纹。入口只接受试用包携带的访问凭据，转发账号请求时移除该头。

公网使用一个 HTTPS 监听，后端账号、游戏、战斗仍使用各自端口。入口按请求类别设置独立并发额度，超额立即返回忙碌状态。验证码发信与登录注册各用一套到账号服的连接池。

| 类别 | 最大并发 |
| --- | --- |
| 账号查询等常规 HTTPS 请求 | 16 |
| 登录、恢复登录、登出和注册 | 4 |
| 验证码发送 | 2 |
| 游戏 PB 长连接 | 64 |
| 战斗 PB 长连接 | 64 |

入口二进制通过 `go build ./cmd/xzj-ingress` 构建，使用 `-listen`、`-cert-p12`、`-account-sha256` 指定监听地址、服务证书和后端账号证书指纹。部署私有配置提供 `XZJ_INGRESS_KEY` 和 `XZJ_TLS_PASSWORD`。Unity 直连配置仅注入试用构建，源码保留默认本地连接方式。

通过 `-limits-file` 指定并发配置。配置完整校验后按约两秒的周期生效，错误配置继续使用上一版。减少额度时，已建立的连接和已受理请求可以继续完成。将某类额度设为 `0` 会暂停该类新请求。

```json
{"account":16,"auth":4,"otp":2,"game":64,"battle":64,"otpPerMinute":24}
```

带试用访问凭据的 `GET /_xzj/metrics` 返回有效配置以及各类请求的在途数、受理数和拒绝数。新增业务时，根据是否长连接、请求耗时及上游依赖分配独立的并发额度。

## 日志与历史事件

| 数据 | 位置与轮换 |
| --- | --- |
| Go 服务运行日志 | 启动时传入 `-log-dir <目录>`，写入 `server-YYYY-MM-DD.NNN.log` |
| 公网入口运行日志 | 启动时传入 `-log-dir <目录>`，写入 `ingress-YYYY-MM-DD.NNN.log` |
| 游戏历史和首次记录 | 默认写入存档目录下的 `history/events-YYYY-MM-DD.jsonl`，也可用 `-history-dir <目录>` 指定 |

日期和时间均为 UTC。运行日志按日期和 16 MiB 分卷，重启保留原卷。新日志文件权限为 `0600`。日志组件保留旧文件，部署时按磁盘容量定期转存已封卷的运行日志。游戏历史与角色、宗门存档一起备份和恢复。归档保留完整的事件序列，首次索引在重启时从归档重建。可在停服后复制完整的存档目录与另行指定的归档目录。

| 业务 | 已记录事件 |
| --- | --- |
| 角色 | `role.created` 建角、`role.login` 首次观测到的成功登录 |
| 修炼 | `cultivation.breakthrough` 突破成功，记录原等级和新等级 |
| 副本 | `dungeon.entered` 进入副本、`dungeon.cleared` 通关、`boss.defeated` 击败首领 |
| 拍卖行 | `auction.purchase` 购买、`auction.bid` 出价、`auction.outbid` 被超价退款、`auction.won` 竞拍结算、`auction.refunded` 结算退款 |
| 宗门 | `guild.applied` 递拜帖、`guild.approved` 通过、`guild.rejected` 驳回、`guild.joined` 入门 |
| 道友 | `friend.requested` 递名刺、`friend.rejected` 拒绝、`friend.added` 成为好友、`friend.removed` 解除好友。结交成功时双方各记一条，保留审批方向 |
| 道友传物 | `friend.goods.sent` 发出物品、`friend.goods.received` 收到物品，发送和接收角色各自拥有首次记录 |
| 师徒 | `mentorship.applied` 递帖、`mentorship.rejected` 拒绝、`mentorship.accepted` 收徒、`mentorship.joined` 拜师成功、`mentorship.greeted` 每日请安、`mentorship.left` 离开师门、`mentorship.expelled` 逐出师门 |
| 内视点穴 | `cultivation.acupoint_broken` 突破成功、`cultivation.acupoint_failed` 概率失败，记录角色、穴位、层数和经脉点消耗 |
| 师徒修行 | `mentorship.power_sent` 传功、`mentorship.meridians_sent` 通脉、`mentorship.practice_claimed` 领取。出师时分别为弟子记录 `mentorship.graduated`、为师尊记录 `mentorship.trained_graduate`。获得称号记录 `title.earned` |

每条记录包含结构版本、稳定事件 ID、发生时间、主体角色 ID、存档账号 ID 和当时的角色名。涉及两人的操作会额外记录对象角色快照。宗门审批事件以审批人为主体，入门事件以入门角色为主体。金币、物品等明细以业务字段保存。邮箱、密码、验证码、会话凭据和原始 PB 包不进入游戏历史。

`first` 标记同一个事件类型的全服首次、账号首次、角色首次，以及指定对象的全服首次和角色首次。例如全服首次通关某个副本、指定角色首次突破到某一层。统计从本功能上线开始。旧存档没有原始事件的操作不会补算，老角色首次重登只记为上线后首次观测到的登录。测试存档和历史目录应独立。

业务变更与待归档事件一起保存。后台批量将事件追加至归档，落盘后确认队列，重启后以来源和序号去重。成就和大事记可以按事件类型、对象和首次标记读取历史。新增功能可在自己的存档事务中调用 `RecordHistory`，为事件指定稳定类型和对象 ID。

用服务端二进制只读查询，无需启动监听或读取发信配置。输出为按归档顺序排列的 JSONL，默认返回最近 100 条匹配记录。

```sh
./xzj-server -data ./data -history-query firsts -history-scope world-entity -history-kind dungeon.cleared
./xzj-server -data ./data -history-query events -history-role '<角色ID>' -history-limit 20
```

普通事件查询匹配主体或对象角色，带角色的首次查询匹配主体。历史查询没有公网路由，由具有归档读取权限的运维账号执行。

## 重现对照测试

[compare.py](tools/compare.py) 使用标准库，在 IPv6 回环和临时目录中运行两个后端。每轮使用相同的 Java GBK 存档作为起点，按固定速率执行相同的 HTTPS 登录及 PB 建角、地图查询、创建与加入副本、施法和退出。脚本还会检查 Java 能否重新打开 Go 写出的存档。测量进程 RSS 和相同请求数量的累计用户态与内核态 CPU 时间。

从仓库根目录运行，传入 Go 二进制、Java 可执行文件和本次 Java 类文件输出目录。

```sh
python3 server-go/tools/compare.py \
  --go-server server-go/.build/xzj-server \
  --java /path/to/jdk/bin/java \
  --java-classes /path/to/compiled-java
```

## 更新协议快照

Java 消息或专用 Cmd 变动后，从仓库根目录用 JDK 11+ 和当前已编译的 Java 类型重新生成 `schema` 文件。

```sh
java -cp "/path/to/compiled-java:server/lib/*" \
  server-go/tools/ExportProtocol.java server-go/schema server/src/server/game/HandlerRegistry.java
```
