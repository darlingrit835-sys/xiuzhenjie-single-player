# 更新记录

## 2026-11-20

### 新增（炼丹规则引擎 + 1005027 接入草案）

- `internal/affinity/alchemy.go`：把 `alchemy` 规则表编译成运行期索引并校验（`limits`、`potencyByQuality` 必须覆盖 1–3、`ranks` 必须 0–9、`flames` 必须引用已登记炉火、`relations` 必须 6 项齐全、`recipes` 的成品必须是常规品阶丹药模板且主药在用料里）。
- 结算口径与 `tools/preview_alchemy.py` 逐位一致：主药 75% 权重、其余按份数加权；功法加成按 `methodInfluences.alchemy` 的路径/品阶比例缩放；成丹率、出丹倍率、药效区间按 `炼丹结算` 上下限截断。`ResolveAlchemy` 用「基础粒数等概率 + 余数出多一粒」的票数分布抽取粒数，成功后在同一药效档内取整数基点。
- `internal/game/h_alchemy.go`：`1005027` 服务端结算——客户端上报的 `IsRefiningSuccess` 不采信，成功与否由服务端用同一套表重算；按组件份数校验工序、扣精力、按成功/失败累计熟练度，回包 `Code/IsSuccess/Count/Rank/CurrentVigor/LiveLevel/LiveProficiency`。
- 由于本仓库是切片（没有 `gateway`/`store`/`pb` 的具体实现），背包转移与规则目录用 `AlchemyBag`/`AlchemyCatalog` 两个装配钩子留出，完整工程里注入即可。
- 补 `go.mod`（`module xzjserver`），使 `internal/affinity` 可独立 `go vet` / `go test`。

### 实测记录（Go 1.27.4，Windows）

```
go test ./internal/affinity/... -v
--- PASS: TestAlchemyChiyangMatchesPreview        # 7946 bps，13020，2/3/4 粒票数 3960/6980/9060，药效 69–73
--- PASS: TestAlchemySpringFlameLowesOdds         # 7546 bps，青泉阴焰水克火 −160
--- PASS: TestAlchemyWeightsSecondaryElements     # 霜脉清毒丹 87/195/68 加权，6997 bps
--- PASS: TestAlchemyRankZeroRecipe              # 凝露丹零级丹师 8360 bps
--- PASS: TestAlchemyRejectsImpossibleBatches
--- PASS: TestAlchemyResolveIsStableAndBounded    # 2000 次抽样成功率 0.77–0.83
--- PASS: TestAlchemyCatalogValidation
--- PASS: TestAlchemySettleValidatesClientReport
--- PASS: TestAlchemyUnitsFeedTheVigorCost
```

同一入参用 Python 参考实现复核：`--pill dan.chiyang --rank 4 --flame flame.solar --method gf_3_9 --region 1211 --period 7` 得 8021 bps（含紫霞郡地利 +75），与文档的 80.21% 一致。

### 协议契约的实物核对（1.3.5 发布包）

拿 `修真界-完整安装包-1.3.5-32.apk` 里热更加载的 `Assembly-CSharp.dll`（5.3 MB，真托管代码）做交叉验证：字符串表里 `RefiningElixirReq_1005027`/`RefiningElixirResp_1005027`、`StepDetailReq`、`IsRefiningSuccess`、`MethodElixirId`、`BestTime` 全部命中，`VerifyIsCanRefiningElixir`（1005026）、`PuppetRefiningElixir`（1005045）也在。也就是说本次还原的请求/回包字段与线上 1.3.5 一致，不是推测。

同时统计出该 DLL 共 591 个不同命令号、1005 族 61 条——本切片已迁移的仍是个位数。

### 尚未完成

- `1005026`（能否炼丹预检）与 `1005045`（傀儡炼丹）未接。
- 精力消耗（现按 `2×组件份数`）与熟练度系数是临时值，待平衡表审定后挪进配置。
- 服用效果、批次入袋的字段落库（`role_bag` 的 uid 与药效快照）依赖完整 `store`。

## 2026-11-19

### 修复（Go 服务端，与服务端 Java 版语义对齐）

- `1001007` 切换修炼方式：由“直接覆盖”改为**异或切换**；回包改为 `ActionBoolResp_1001001`，`4058` 成功 / `4059` 失败；元婴（31 级）前限制只能同时运行一门，且不允许全部关闭。
- `1001008` 突破询问：`Code` 由错误的 `2000` 改为 `4062`（不可突破）/ `4063`（可突破），`HasThunder=0`。
- `1001010` 突破境界：`Code` 改为 `4306`（成功）/ `4062`（失败），回包补全 16 个字段（含 `Age`、`CurrentPower`、`CurrentPureBlood`、`CurrentClever`、`CurrentPoisonVal`）。
- 测试探针 `cmd/probe`：读回包改为**按 cmd 匹配、跳过无关包**，最多跳过 8 个。

### 关于「复登录数据归零」的复核结论

上一轮探针报告的“二次登录后灵石/修为/签到全为 0”**是探针读错包造成的假象**，不是存档 bug。

证据：直接查 SQLite 时，`role_attrs_s0`（`Level=2`、`CurrentPower=13801`）、`role_currency_s0`（`shiny_stone=101000`）、`role_signin_s0`（`SignInDay=1`）均在库且正确；修好探针读包后复登录回包为 `灵石=101000 修为=15 签到日=1 战力=13801`，与库完全一致。

### 实测记录（干净库，先 HTTPS `/login` 建账号再进游戏服创角）

```
1002001 创角 Code=2000 Success=true
1002002 登录 Code=2000 lv=1 灵石=100000 战力=11010 引导=100
1007001 历练地图 12 格：6 城 [1-10]..[51-60] + 林地1..6 [1-10]..[51-60]
1007003 林地1 子区域：外围[1-3] 深处[4-6] 兽王巢[7-10]
1002003 离线面板 Code=4181
1002004 领离线 Code=2000
1002018 签到列表 Code=2000 共 7 天，第1天 灵石 x1000
1002021 领签到 Code=2000 Success=true
1001008 突破询问 Code=4063 HasThunder=0
1001010 突破 Code=4306 Success=true 修为=15 悟性=590 体魄=590
1001007 切换修炼方式(纳气→纳气) Code=4059 Success=false   # 会变成全关，按设计拒绝
1002002 复登录 灵石=101000 灵玉=0 修为=15 签到日=1 战力=13801
```

把等级改到 31 后复测切换（纳气 + 炼神 双修）：

```
1001007 切换修炼方式 Code=4058 Success=true
role_misc_s0: RunMask=5, RunLocation=6
```

### 已知未完成

- 86 条指令目前只迁移了 12 条（创角/登录/离线/签到/突破/切换/历练地图/子地图/心跳）；玄天楼、纳戒装备、洞府、灵田、参悟、战斗、任务仍走通用兜底。
- 客户端的 `1002002` 回包里 `SkillPoint=0`，与 Java 版一致，无需改。
