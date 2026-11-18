# 修真界私服 - 续接提示词

> 复制以下整段，在新会话开头粘贴即可续上工作。

---

继续"修真界"手游私服项目。项目背景：给《修真界》APK（com.LMGame.XiuXianJie）配自建服务端，APK 只改连接 IP。客户端是 Unity Mono（C# 全可反编译），协议是 protobuf（varint 帧 + Msg{Cmd,Msid,Data} 信封）。

## 成果位置（全部）

| 内容 | 路径 |
|---|---|
| 服务端代码（Java 8，可编译可跑） | `E:\a_y\修真界1\server\`（build.bat 编译 / start.bat 启动 / README.md） |
| 服务端数据（users/roles JSON + 配置表） | `E:\a_y\修真界1\server\data\` |
| 策划配置表 Excel（32 张，中文表名） | `E:\a_y\修真界1\server\data\config_excel\` |
| 客户端反编译源码（base 1.3.5 版，905 消息/470 cmd） | `E:\a_y\修真界1\server\dev\src_base\` |
| 客户端反编译源码（1.2.6 旧版，勿用） | `E:\a_y\修真界1\server\dev\src_126\` |
| 协议 proto + 生成的 Java/Python | `E:\a_y\修真界1\server\dev\proto\`、`gen_py\`、服务端 `src\xzj\Xzj.java` |
| APK 原始包 / 私服成品包 / 签名证书 | `E:\a_y\修真界1\server\dev\base_src.zip` / `修真界base私服.apk` / `sign.keystore`（口令由私有渠道提供） |
| 打包与 patch 工具链 | `E:\a_y\修真界1\server\dev\`（ilspycmd91、cecil、protoc、patch_base_clean.py、repack_base.py 等脚本） |
| 逆向工具 rea | `E:\a_y\真深\rea-main` |
| 参照服务端（仅参考框架） | snhz：`E:\a_y\snhzprojectx11\原始源码server2024`（Java+Netty+protobuf）；kdqb：`E:\a_y\kdqb\kdqb`（Elex 分服） |
| 原 APK 目录 | `E:\a_y\修真界1\`（base.apk 137MB=完整资源包，1.2.6 zip=资源不全别用） |

## 架构现状（已完成）

- **客户端判定**：base.apk（1.3.5，Unity 2022.3，资源全内置）。用 base 包做基底，1.2.6 包 UI 靠已下线的官方热更、不可用。
- **协议**：470 cmd 全提取，生成 `xzj.proto`（905 消息），tag 交叉验证 0 错误。帧格式 = varint32 长度 + protobuf Msg。
- **服务端**：Java 8 + Netty 4.1.115（分模块 jar）+ protobuf-java 4.36.2 + Gson。零 Maven（javac 直编，lib\ 有依赖）。
  - AccountServer：HTTPS 9443（自签证书 data/server.p12，口令通过 XZJ_TLS_PASSWORD 提供），接口 /login /user/register /gameService/statusSearch 等已实现；/client/update/local/download 返回 404（故意，走客户端错误分支）。
  - GameServer：TCP 17666。已实现真逻辑 handler：心跳9000000、创建角色1002001、角色登录1002002、离线收益1002003、查名1002005、背包空间1004002、洞府1005001、兄弟地图1007001、子地图1007003、始祖地图1007008、聊天服1012001（返回失败让客户端跳过）。**其余全部 cmd 走 GenericHandler 兜底**（反射构造 IsSuccess=true/Code=2000）。
  - 角色/账号存 JSON 文件（data/users.json、roles.json），后续可换 MySQL。
  - 地图树：MapTree.java（始祖→4大陆→5洲→6郡→每郡12城→每城6建筑节点：城主府/玄天楼/虚神界/坊市/传送大阵/灵泉酒肆）。
- **改包**：patch_base_clean.py（Mono.Cecil 改 Constants.LoginServerIP=10.0.2.2、端口 9443；Resource.dll 热更 URL）→ repack_base.py → zipalign → apksigner 签名。改包流程已验证。
- **验证**：用户 MuMu 模拟器（adb emulator-5560）已能：登录→选服→创建角色→进主城。

## 已踩坑（勿重踩）

1. 服务端 java 进程启动用 `python D:\Temp\opencode\start_srv.py` 这类 detach 方式，PowerShell Start-Process 会被会话 kill。
2. protobuf 4.x 没有 writeTo(CodedOutputStream)，netty 的 ProtobufVarint32LengthFieldPrepender 会静默失败——服务端发送用自写 RawSender（varint 手工帧）。
3. 中文路径传给 protoc/javac 会乱码——服务端放英文路径 E:\a_y\修真界1\server。
4. Cecil patch IL：Mono 验证器很严，NOP/删指令/短跳转都会 InvalidProgramException。只做过 operand 替换（ldstr/ldc.i4）是安全的；改控制流要长跳转并验证。
5. 客户端地图：洲/郡按钮在创建角色模式会被禁用（MaxLevel>40 或 MinLevel>1 或 Type 2/3 时）；郡子节点必须 9~12 个（12 个才走客户端"复制面板"正常分支，9/10/11 有 bug 会隐藏面板）；城市需 Level=4/Type=51/MinLevel≤1；每城要挂建筑节点否则主城按钮不显示。
6. Gson 读 JSON 数字默认 Double——输出给客户端（LitJson 强类型）前要转 int（曾导致 currentRoleVO.level 报 Can't assign Double to Int32）。
7. 热更 XML 版本号必须等于客户端版本（base=1.3.5），不匹配客户端弹"版本不匹配"。
8. base 的 RoleLoginResp 字段与 1.2.6 不同（Code/IsSuccess 前置），服务端协议必须用 base 版。

## 待办（阶段 6 重点）

> 待办分两类（详见 DESIGN.md）：A 类 = 照客户端代码补逻辑（体力活）；B 类 = 自行设计（算法/数据，官方未下发）。布道黑盒算法完整设计、同类创造系统清单、工程背景决策都在 DESIGN.md。

- **A 类**：按用户游戏内操作补业务 handler 真逻辑：装备1009005、信件1003001、签到1002018、好友1014001、参悟列表1001011、榜单1008002 等。
- **B 类**：布道/创造功法黑盒算法（Julia 集方案已设计，协议 1008033，见 DESIGN.md 第二节）、战斗数值/掉落、服务端专属数据表（怪物/副本/拍卖行）。

> 当前已补：怪物数值表（`data/config/monsters.json` + `MonsterBalance`）、新手引导（1002013/1002014）、126 条主线任务（1016002/1016001）、纳戒/新手礼包/装备/技能（1004002/1004004/1004008/1004024/1009001/1009005/1009007）、9 技能战斗与战斗掉落（`SkillService`/`BattleRewardService`），详见 FIXES.md 第 5~10 节。

1. 按用户游戏内操作补业务 handler 真逻辑（当前全兜底空数据）：装备1009005、信件1003001、签到1002018、好友1014001、参悟列表1001011、榜单1008002 等。
2. **布道/创造功法黑盒算法**（用户最在意）：协议 = VerifyCreateMethodAchievementReq_1008033{AcupointId[]穴位序列, Stage品阶} → Resp 20 字段（CreatedIsSuccess/FiveElement/Rank/MinLevel/MaxLevel/EffectType1/2+EffectValue1/2/ConversionRate/CurrentClever 等）。方案已定：穴位序列+品阶编码成复数 c → Julia 迭代 z_{n+1}=z_n²+c → 逃逸=失败/收敛=成功 → 幅角映射五行（按玩家灵根加权）、迭代质量映射等级效果。同类创造系统（丹方1008018/法术1008025/符书1008047/法器1008073/阵盘1008088）套同一模式。
3. 服务端专属数据自建：怪物属性、掉落池、副本、拍卖行（官方未下发客户端，无法还原，需自造）。
4. 数据库升级 MySQL（可选，改 JsonStore 层即可）。
5. 战斗系统（BattleServer 战斗服协议 800x 系列尚未实现，进副本时客户端会连战斗服）。

## 操作环境备忘

- JDK：`D:\Tool\Java\jdk-1.8`（或 kdqb 的 jdk1.8.0_161）；adb/apksigner/zipalign：`D:\Tool\Android\SDK\platform-tools`、`build-tools\30.0.3`。
- 模拟器：MuMu（adb emulator-5560），模拟器访问宿主机用 10.0.2.2。
- 本地测试请创建独立账号。
- 服务端启动：`cd E:\a_y\修真界1\server && java -cp out;lib/* server.Main`（或 start.bat）。
