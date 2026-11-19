# Go 协议缺口清单

基线为 2026-11-19 检查的完整 Unity 客户端与 Go 测试工程。以客户端已定位的调用和 Go 试用注册表为范围。本表按 Cmd 汇总，具体功能与优先级见[功能状态与协作待办](功能状态与协作待办.md)。

游戏服定位到 407 个客户端 Cmd。和试用注册表合并后，本表列出 281 条业务缺口，其中兼容回包 252 条，缺少回包映射 4 条，明确拒绝 14 条，只读或示例 11 条。另外 53 条原生分发和 76 条已接入试用业务的条目需按模块确认完整度。

- **明确拒绝**有协议响应和失败分支，待接入业务。**只读或示例**可以显示已有状态、样例或空列表。
- **兼容回包**按 PB 描述符处理。**缺少回包映射**尚未找到匹配的专用处理或默认响应类型。
- 查看试用写入需使用完整工程及隔离存档，同时启用 `-interface-preview`、`-interface-writes`。
- 窗口名是定位线索，业务修改前请继续检查字段、回调和持久化。协议表中的英文名称沿用客户端 `MsgType`。

## 修行、广告与权益（1001）

| Cmd | Go 状态 | Unity 调用位置 | 操作或协议名称 | Go 响应类型 |
| --- | --- | --- | --- | --- |
| `1001002` | 明确拒绝 | `GuangGaoWnd` | 查询广告可用性，暂未接入广告渠道 | `ActionBoolResp_1001001` |
| `1001003` | 兼容回包 | `GuangGaoWnd` | `ROLE_LOOK_ADVERTISING` | `LookAdvertisingResp_1001003` |
| `1001004` | 兼容回包 | `XuanTianLingWnd` | `ROLE_BUY_YUEKA` | `BuyYuekaResp_1001004` |
| `1001006` | 兼容回包 | `CanWuGongFaWnd` | `USED_ACHIEVEMENT_PART` | `UsedAchievementPartResp_1001006` |
| `1001009` | 兼容回包 | `KongLingFaZhenWnd` | `ROLE_CHANGE_CONTROLLER_MATRIX` | `ChangeControllerMatrixResp_1001009` |
| `1001013` | 明确拒绝 | `CanWuWnd` | 遗忘参悟，暂未接入完整参悟状态 | `ActionBoolResp_1001001` |
| `1001015` | 兼容回包 | `SFH_CreateChooseWnd` | `UNDERSTAND_SKILL` | `UnderstandSkillResp_1001015` |
| `1001018` | 兼容回包 | `CashGoodsLiBaoPanel`、`VipPowerPanel` | `BuyLiBao` | `BuyLiBaoResp_1001018` |
| `1001021` | 兼容回包 | `TiShengPingJieWnd` | `UpgradeEquipmentConversionRate` | `UpgradeEquipmentConversionRateResp_1001021` |
| `1001022` | 兼容回包 | `TiShengPingJieWnd` | `UpgradeSkillConversionRate` | `UpgradeSkillConversionRateResp_1001022` |

## 角色、签到与排行榜（1002）

| Cmd | Go 状态 | Unity 调用位置 | 操作或协议名称 | Go 响应类型 |
| --- | --- | --- | --- | --- |
| `1002008` | 兼容回包 | `ChongShenWnd` | `ROLE_SUICIDE` | `RoleSuicideResp_1002008` |
| `1002009` | 兼容回包 | `TuPoDuJieWnd` | `ENTER_THUNDER_CALAMITY` | `EnterThunderCalamityResp_1002009` |
| `1002022` | 兼容回包 | `CashGoodsWnd` | `LiBaoBuyInfo` | `LiBaoBuyInfoResp_1002022` |
| `1002023` | 兼容回包 | `CashGoodsLiBaoPanel` | `ShouChongLiBaoGet` | `ShouChongLiBaoGetResp_1002023` |
| `1002024` | 只读或示例 | `LevelRankWnd` | 按当前角色显示单机境界榜，其余类别为空 | `RealmRankedResp_1002024` |
| `1002026` | 明确拒绝 | `LevelRankWnd` | 膜拜排行榜 | `MoBaiResp_1002026` |
| `1002027` | 兼容回包 | `ChangShengLuPanel` | `ENTERCSL` | `EnterCSLResp_1002027` |
| `1002028` | 兼容回包 | `ChangShengLuItem` | `GETCSL_GOODS` | `GetCSLGoodsResp_1002028` |
| `1002029` | 兼容回包 | `VipPowerPanel` | `ENTER_VIP` | `VipLevelResp_1002029` |
| `1002032` | 兼容回包 | `ZongMen_ShiKongMiJingWnd` | `GetShiKongMiJing` | `GetShiKongMiJingResp_1002032` |

## 纳戒、道具和玄天楼（1004）

| Cmd | Go 状态 | Unity 调用位置 | 操作或协议名称 | Go 响应类型 |
| --- | --- | --- | --- | --- |
| `1004011` | 兼容回包 | `XzjBoxEntity` | `GET_BOOK_EQUIPMENT_DETAIL` | `GoodsBookEquipmentDetailResp_1004011` |
| `1004012` | 兼容回包 | `XzjBoxEntity` | `GET_BOOK_SIGN_DETAIL` | `GoodsBookSignDetailResp_1004012` |
| `1004013` | 兼容回包 | `XzjBoxEntity` | `GET_BOOK_SKILL_DETAIL` | `GoodsBookSkillDetailResp_1004013` |
| `1004014` | 兼容回包 | `XzjBoxEntity` | `GET_ARTICLE_DEFENSE_EQUIPMENT_DETAIL` | `GoodsArticleDefenseEquipmentResp_1004014` |
| `1004015` | 兼容回包 | `XzjBoxEntity` | `GET_ARTICLE_ELIXIR_DETAIL` | `GoodsArticleElixirResp_1004015` |
| `1004016` | 兼容回包 | `XzjBoxEntity` | `GET_ARTICLE_ELIXIR_CHANGE` | `GoodsArticleElixirChangeResp_1004016` |
| `1004017` | 兼容回包 | `XzjBoxEntity` | `GET_ARTICLE_SECRET_AREA_DETAIL` | `GoodsArticleSecretAreaResp_1004017` |
| `1004020` | 兼容回包 | `XzjBoxEntity` | `GET_ARTICLE_SIGN_DETAIL` | `GoodsArticleSignResp_1004020` |
| `1004021` | 兼容回包 | `XzjBoxEntity` | `GET_ARTICLE_SIGN_USE_TIME` | `GoodsArticleSignUseTimeResp_1004021` |
| `1004022` | 兼容回包 | `NaJieOneKeySoleWnd`、`NaJieSpaceWnd` | `SELL_GOODS` | `GoodsSellResp_1004022` |
| `1004025` | 兼容回包 | `NaJieSpaceWnd` | `UPDATE_NAJIE` | `GradeRingResp_1004025` |
| `1004027` | 兼容回包 | `BattleWnd` | `DESTROY_GOODS` | `DestroyGoodsResp_1004027` |
| `1004028` | 兼容回包 | `DiHuoGeCaiLiaoShaiXuanWnd`、`DiHuoGeCaiLiaoShaiXuan_LK_Wnd`、`LianQiCaiLiaoShaiXuanWnd`、`LianQiCaiLiaoShaiXuan_LK_Wnd` | `SEARCH_LMSTOCK` | `SearchLMStockResp_1004028` |
| `1004030` | 兼容回包 | `DiHuoGeCaiLiaoJiaGeWnd` | `ONEKEY_BUY_MATERIALS` | `OneKeyBuyMaterialsResp_1004030` |
| `1004031` | 兼容回包 | `NaJieSpaceWnd` | `USED_SECRET` | `UsedSecretResp_1004031` |
| `1004036` | 兼容回包 | `ZhanDouSettlementWnd` | `BATTLEEND_RINGOUT_GOODS` | `BattleEndRingOutGoodsResp_1004036` |
| `1004038` | 明确拒绝 | `SheLiZongMenWnd`、`XzjBoxEntity` | 查询宗门地块物品，暂未接入地块属性表 | `GoodsGuildMapDetailsReap_1004038` |
| `1004041` | 兼容回包 | `HeChengWnd` | `GOODS_COMPOUND` | `GoodsCompoundResp_1004041` |
| `1004043` | 兼容回包 | `XzjBoxEntity` | `GOODS_ARTICLE_MATRIX` | `GoodsArticleMatrixResp_1004043` |
| `1004044` | 兼容回包 | `XzjBoxEntity` | `GOODS_ARTICLE_BOOK_MATRIX` | `GoodsBookMatrixDetailResp_1004044` |
| `1004045` | 兼容回包 | `BattleWnd` | `GOODS_MATRIX_USE` | `GoodsMatrixUseResp_1004045` |

## 洞府、经脉和生产（1005）

| Cmd | Go 状态 | Unity 调用位置 | 操作或协议名称 | Go 响应类型 |
| --- | --- | --- | --- | --- |
| `1005003` | 兼容回包 | `DongFuRoomWnd` | `BUILD_LIVE_TYPE` | `BuildLiveTypeResp_1005003` |
| `1005004` | 兼容回包 | `DongFuRoomWnd` | `REBUILD_LIVE_TYPE` | `RebuildLiveTypeResp_1005004` |
| `1005006` | 兼容回包 | `DongFuRoomWnd` | `RENAME_DONGFU` | `RenameHomeResp_1005006` |
| `1005012` | 兼容回包 | `DongFuWnd` | `BROKEN_STATE` | `BrokenStateResp_1005012` |
| `1005013` | 兼容回包 | `GengHuanFaZhenDengJielWnd`、`JuLingFaZhenWnd` | `UPGRADE_GATHER_MATRIX` | `UpgradeGatherMatrixResp_1005013` |
| `1005014` | 兼容回包 | `DongFuWnd` | `OPEN_CONTROLLER_MATRIX` | `OpenControllerMatrixResp_1005014` |
| `1005015` | 兼容回包 | `GengHuanFaZhenDengJielWnd`、`KongLingFaZhenWnd` | `UPGRADE_CONTROLLER_MATRIX` | `UpgradeControllerMatrixResp_1005015` |
| `1005016` | 只读或示例 | `NeiShiWnd` | 内视读取角色已保存的经脉点与穴位 | `OpenAcupointResp_1005016` |
| `1005019` | 兼容回包 | `DiHuoGeWnd` | `SEARCH_UNDERSTAND_LIST` | `SearchUnderstandElixirListResp_1005019` |
| `1005020` | 兼容回包 | `ZongLanWnd` | `ACUPOINT_DETAIL` | `AcupointDetailResp_1005020` |
| `1005024` | 兼容回包 | `KongLingFaZhenWnd` | `ADD_TIME_CONTROLLER_MATRIX` | `AddTimeControllerMatrixResp_1005024` |
| `1005025` | 兼容回包 | `JuLingFaZhenWnd` | `ADD_TIME_GATHER_MATRIX` | `AddTimeGatherMatrixResp_1005025` |
| `1005026` | 兼容回包 | `DiHuoGeCaiLiaoShaiXuanWnd`、`DiHuoGeCaiLiaoShaiXuan_LK_Wnd` | `VERIFY_IS_CAN_REFINING_ELIXIR` | `VerifyIsCanRefiningElixirResp_1005026` |
| `1005027` | 兼容回包 | `DiHuoGeKaiShiLianZhiWnd` | `REFINING_ELIXIR` | `RefiningElixirResp_1005027` |
| `1005030` | 兼容回包 | `DiHuoGeWnd` | `SEARCH_METHOD_REFINING_STEPLIST` | `SearchElixirRefiningStepListResp_1005030` |
| `1005032` | 兼容回包 | `HuaFuChooseWnd` | `SEARCH_UNDERSTAND_SIGNLIST` | `SearchUnderstandSignListResp_1005032` |
| `1005034` | 兼容回包 | `HuaFuChooseWnd` | `SEARCH_SIGN_REFINING_STEPLIST` | `SearchSignRefiningStepListResp_1005034` |
| `1005036` | 兼容回包 | `HuaFuWnd` | `REFINING_SIGN` | `RefiningSignResp_1005036` |
| `1005037` | 兼容回包 | `HuaFuChooseWnd` | `VERIFY_ISCAN_REFINING_SIGN` | `VerifyIsCanRefiningSignResp_1005037` |
| `1005038` | 兼容回包 | `ZhanFaShiWnd` | `ENTER_BATTLEHOME` | `EnterBattleHomeResp_1005038` |
| `1005040` | 兼容回包 | `ZhanFaShiItem` | `PRACTICE_SKILL` | `PracticeSkillResp_1005040` |
| `1005041` | 兼容回包 | `WuDaoTaiWnd` | `ENTER_WUDAOTAI` | `EnterWuDaoTaiResp_1005041` |
| `1005042` | 兼容回包 | `WuDaoTaiWnd` | `RECHOOSE_ACHIEVEMENT_EFFECT` | `RechooseAchievementEffectResp_1005042` |
| `1005043` | 缺少回包映射 | `HuaFuWnd`、`SFH_CreateWnd` | `HUAFU_CHECK` | — |
| `1005044` | 兼容回包 | `FaQiXiLianWnd` | `RECHOOSE_EQUIPMENT_EFFECT` | `RechooseEquipmentEffectResp_1005044` |
| `1005045` | 兼容回包 | `DiHuoGeCaiLiaoShaiXuan_LK_Wnd` | `PUPPET_REFINING_ELIXIR` | `PuppetRefiningElixirResp_1005045` |
| `1005046` | 兼容回包 | `HuaFuChoose_LK_Wnd` | `PUPPET_REFINING_SIGN` | `PuppetRefiningSignResp_1005046` |
| `1005047` | 兼容回包 | `FaQiXiLianWnd` | `CHOOSE_EQUIPMENT` | `ChooseEquipmentResp_1005047` |
| `1005048` | 兼容回包 | `LianQiCaiLiaoShaiXuanWnd`、`LianQiCaiLiaoShaiXuan_LK_Wnd` | `VERIFY_IS_CAN_REFINING_EQUIPMENT` | `VerifyIsCanRefiningEquipmentResp_1005048` |
| `1005049` | 兼容回包 | `LianQiFangWnd` | `REFINING_EQUIPMENT` | `RefiningEquipmentResp_1005049` |
| `1005050` | 缺少回包映射 | `LianQiFangWnd` | `RefiningEquipmentRecord` | — |
| `1005051` | 兼容回包 | `LianQiChooseWnd` | `SEARCH_UNDERSTAND_EQUIPMENT_LIST` | `SearchUnderstandEquipmentListResp_1005051` |
| `1005053` | 兼容回包 | `LianQiChooseWnd` | `SEARCH_EQUIPMENT_REFINING_STEPLIST` | `SearchEquipmentRefiningStepListResp_1005053` |
| `1005055` | 兼容回包 | `LianQiCaiLiaoShaiXuan_LK_Wnd` | `PUPPET_REFINING_EQUIPMENT` | `PuppetRefiningEquipmentResp_1005055` |
| `1005056` | 兼容回包 | `ZF_CreateChooseWnd` | `VERIFYISCANREFININGMATRIX` | `VerifyIsCanRefiningMatrixResp_1005056` |
| `1005057` | 兼容回包 | `ZF_LianZhiWnd` | `REFININGMATRIX` | `RefiningMatrixResp_1005057` |
| `1005058` | 兼容回包 | `ZF_CreateChooseWnd` | `SEARCHUNDERSTANDMATRIXLIST` | `SearchUnderstandMatrixListResp_1005058` |
| `1005060` | 兼容回包 | `ZF_LianZhiWnd` | `SEARCHMATRIXREFININGSTEPLIST` | `SearchMatrixRefiningStepListResp_1005060` |
| `1005061` | 兼容回包 | `ZF_Choose_LK_Wnd` | `PUPPETREFININGMATRIX` | `PuppetRefiningMatrixResp_1005061` |

## 奇珍阁与拍卖会（1006）

| Cmd | Go 状态 | Unity 调用位置 | 操作或协议名称 | Go 响应类型 |
| --- | --- | --- | --- | --- |
| `1006013` | 明确拒绝 | `AH_PaiPinXiangQingWnd` | 上架拍卖会拍品 | `OnShelfTreasureResp_1006013` |
| `1006014` | 只读或示例 | `AH_WoDeGuaPaiWnd` | 我的拍卖会挂牌空态 | `OnShelfListResp_1006014` |
| `1006016` | 明确拒绝 | `AH_WoDeGuaPaiWnd` | 撤销拍卖会挂牌 | `TreasureGoodsCancelResp_1006016` |

## 虚神界、竞技和协会（1008）

| Cmd | Go 状态 | Unity 调用位置 | 操作或协议名称 | Go 响应类型 |
| --- | --- | --- | --- | --- |
| `1008001` | 兼容回包 | `XuShenTaWnd` | `XSJ_XST_OPEN` | `EnterUnrealTowerResp_1008001` |
| `1008002` | 只读或示例 | `LevelRankWnd`、`XuShenTaBangDanWnd` | 显示虚神塔道境榜示例 | `EnterRushTowerTierNumRankedResp_1008002` |
| `1008004` | 只读或示例 | `LevelRankWnd`、`XuShenTaBangDanWnd` | 显示虚神塔天骄榜示例 | `EnterRushTowerAgeRankedResp_1008004` |
| `1008006` | 兼容回包 | `DanShiTaWnd` | `XSJ_DST_OPEN` | `EnterElixirTowerResp_1008006` |
| `1008007` | 兼容回包 | `DST_RenZhengWnd` | `XSJ_DST_DANSHIRENZHENG` | `EnterElixirAuthResp_1008007` |
| `1008009` | 兼容回包 | `DST_RenZhengWnd`、`DiHuoGeLianDanJieSuanWnd` | `XSJ_DST_DANFANGKAOHE` | `ExamineElixirStepResp_1008009` |
| `1008011` | 兼容回包 | `DiHuoGeKaiShiLianZhiWnd` | `XSJ_DST_DANSHIKAOHERESULT` | `ElixirExamineResultResp_1008011` |
| `1008012` | 兼容回包 | `DST_DanFangDuiHuanWnd` | `XSJ_DST_OPEN_DANFANGDUIHUAN` | `EnterMethodElixirExchangeResp_1008012` |
| `1008014` | 兼容回包 | `DST_DanFangDuiHuanTipWnd` | `XSJ_DST_DANFANGDUIHUAN` | `ExchangeMethodElixirResp_1008014` |
| `1008015` | 兼容回包 | `DST_ZhuCeWnd` | `XSJ_DST_OPEN_DANFANGZHUCE` | `EnterElixirRegisterResp_1008015` |
| `1008017` | 兼容回包 | `DST_ZhuCeTipWnd` | `XSJ_DST_ZHUCEDANFANG` | `RegisterMethodElixirResp_1008017` |
| `1008018` | 兼容回包 | `DiHuoGeLianZhiBuZhouSheZhiWnd` | `XSJ_DST_CREATE_DANFANG` | `VerifyCreateMethodElixirResp_1008018` |
| `1008020` | 兼容回包 | `DiHuoGeKaiShiLianZhiWnd`、`DiHuoGeLianZhiBuZhouSheZhiWnd` | `XSJ_DST_CREATE_DANFANG_RESULT` | `CreateMethodElixirRefiningResultResp_1008020` |
| `1008021` | 兼容回包 | `WanFaGeWnd` | `XSJ_WFG_OPEN` | `EnterSkillLoftResp_1008021` |
| `1008022` | 兼容回包 | `WFG_FaShuDuiHuanWnd` | `XSJ_WFG_OPEN_DUIHUAN` | `EnterMethodSkillExchangeResp_1008022` |
| `1008024` | 兼容回包 | `WFG_FaShuDuiHuanTipWnd` | `XSJ_WFG_DUIHUAN` | `ExchangeMethodSkillResp_1008024` |
| `1008025` | 兼容回包 | `ChuangZaoFaShuWnd` | `XSJ_WFG_CREATE` | `VerifyCreateMethodSkillResp_1008025` |
| `1008026` | 兼容回包 | `WFG_ZhuCeWnd` | `XSJ_WFG_OPEN_FASHUZHUCE` | `EnterSkillRegisterResp_1008026` |
| `1008028` | 兼容回包 | `WFG_ZhuCeTipWnd` | `XSJ_WFG_FASHUZHUCE` | `RegisterMethodSkillResp_1008028` |
| `1008029` | 兼容回包 | `DaoJingShengDianWnd` | `XSJ_DJSD_OPEN` | `EnterAchievementHallResp_1008029` |
| `1008030` | 兼容回包 | `DJSD_GongFaDuiHuanWnd` | `XSJ_DJSD_OPEN_GONGFADUIHUAN` | `EnterMethodAchievementExchangeResp_1008030` |
| `1008032` | 兼容回包 | `DJSD_GongFaDuiHuanTipWnd` | `XSJ_DJSD_GONGFADUIHUAN` | `ExchangeMethodAchievementResp_1008032` |
| `1008033` | 兼容回包 | `ChuangZhaoGongFaWnd` | `XSJ_DJSD_CREATE` | `VerifyCreateMethodAchievementResp_1008033` |
| `1008034` | 兼容回包 | `DJSD_ZhuCeWnd` | `XSJ_DJSD_OPEN_GONGFAZHUCE` | `EnterAchievementRegisterResp_1008034` |
| `1008036` | 兼容回包 | `DJSD_ZhuCeTipWnd` | `XSJ_DJSD_GONGFAZHUCE` | `RegisterMethodAchievementResp_1008036` |
| `1008037` | 明确拒绝 | `XuShenTaBangDanWnd` | 领取道境榜奖励 | `RushTowerTierNumRankedAwardResp_1008037` |
| `1008038` | 明确拒绝 | `XuShenTaBangDanWnd` | 领取天骄榜奖励 | `RushTowerAgeRankedAwardResp_1008038` |
| `1008039` | 兼容回包 | `CaiLiaoShaiXuanWnd`、`LZ_CaiLiaoShaiXuanWnd` | `SEARCH_PLANT_MATERIAL` | `SearchPlantMaterialResp_1008039` |
| `1008041` | 兼容回包 | `CaiLiaoShaiXuanWnd` | `SEARCH_ANIMAL_MATERIAL` | `SearchAnimalMaterialResp_1008041` |
| `1008043` | 只读或示例 | `SFH_ShenFuHuiWnd` | 神符会贡献空态 | `EnterSignUnionResp_1008043` |
| `1008044` | 兼容回包 | `SFH_FuShuDuiHuanWnd` | `OPEN_FUSHU_DUIHUAN` | `EnterMethodSignExchangeResp_1008044` |
| `1008046` | 兼容回包 | `SFH_FuShuDuiHuanTipWnd` | `FUSHU_DUIHUAN` | `ExchangeMethodSignResp_1008046` |
| `1008047` | 兼容回包 | `SFH_CreateWnd` | `FUSHU_CHUANGZHAO_CHECK` | `VerifyCreateMethodSignResp_1008047` |
| `1008048` | 兼容回包 | `SFH_ZhuCeWnd` | `OPEN_FUSHU_ZHUCE` | `EnterSignRegisterResp_1008048` |
| `1008050` | 兼容回包 | `SFH_ZhuCeTipWnd` | `FUSHU_ZHUCE` | `RegisterMethodSignResp_1008050` |
| `1008052` | 兼容回包 | `SFH_CreateWnd` | `FUSHU_CREATE_RESULT` | `CreateMethodSignRefiningResultResp_1008052` |
| `1008053` | 兼容回包 | `ChuangZaoFaShuWnd` | `YIN_SELECT` | `HandSealListResp_1008053` |
| `1008054` | 只读或示例 | `LZ_MianWnd` | 灵植协会贡献空态 | `EnterPlantAssociationResp_1008054` |
| `1008055` | 兼容回包 | `LZ_LingZhiDuiHuanWnd` | `ENTER_TEMPLATE_PLANT_EXCHANGE_RESP` | `EnterTemplatePlantExchangeResp_1008055` |
| `1008057` | 兼容回包 | `LZ_LingZhiDuiHuanTipWnd` | `EXCHANGE_TEMPLATE_PLANT_RESP` | `ExchangeTemplatePlantResp_1008057` |
| `1008058` | 兼容回包 | `LZ_ChuangZaoWnd` | `VERIFY_CREATE_TEMPLATE_PLANT_RESP` | `VerifyCreateTemplatePlantResp_1008058` |
| `1008059` | 兼容回包 | `LZ_ZhuCeWnd` | `ENTER_TEMPLATE_PLANT_REGISTER_RESP` | `EnterTemplatePlantRegisterResp_1008059` |
| `1008061` | 兼容回包 | `LZ_ZhuCeTipWnd` | `REGISTER_TEMPLATE_PLANT_RESP` | `RegisterTemplatePlantResp_1008061` |
| `1008062` | 缺少回包映射 | `MainCityWnd` | `Into_Unreal_World` | — |
| `1008066` | 只读或示例 | `LianQi_ChuangZaoMianWnd` | 炼器协会贡献空态 | `EnterEquipmentAssociationResp_1008066` |
| `1008067` | 兼容回包 | `LianQi_DuiHuanWnd` | `ENTER_METHOD_EQUIPMENT_EXCHANGE` | `EnterMethodEquipmentExchangeResp_1008067` |
| `1008069` | 兼容回包 | `LQ_QiTuDuiHuanTipWnd` | `EXCHANGE_METHOD_EQUIPMENT` | `ExchangeMethodEquipmentResp_1008069` |
| `1008070` | 兼容回包 | `LianQi_ZhuCeWnd` | `ENTER_EQUIPMENT_REGISTER` | `EnterEquipmentRegisterResp_1008070` |
| `1008072` | 兼容回包 | `LQ_ZhuCeTipWnd` | `REGISTER_METHOD_EQUIPMENT` | `RegisterMethodEquipmentResp_1008072` |
| `1008073` | 兼容回包 | `LianQiBuZhouSheZhiWnd` | `VERIFY_CREATE_METHOD_EQUIPMENT` | `VerifyCreateMethodEquipmentResp_1008073` |
| `1008075` | 兼容回包 | `LianQiCaiLiaoXuanZeShaiXuanWnd` | `SearchCreateEquipmentPlantMaterial` | `SearchCreateEquipmentPlantMaterialResp_1008075` |
| `1008077` | 兼容回包 | `LianQiCaiLiaoXuanZeShaiXuanWnd` | `SearchCreateEquipmentAnimalMaterial` | `SearchCreateEquipmentAnimalMaterialResp_1008077` |
| `1008079` | 兼容回包 | `LianQiCaiLiaoXuanZeShaiXuanWnd` | `SearchCreateEquipmentMineralMaterial` | `SearchCreateEquipmentMineralMaterialResp_1008079` |
| `1008080` | 兼容回包 | `ChuangZhaoGongFaWnd` | `VerifyCompletionMethodAchievement` | `VerifyCompletionMethodAchievementResp_1008080` |
| `1008081` | 兼容回包 | `DJSD_YiZhuCeListWnd` | `EnterAchievementCompletionReq` | `EnterAchievementCompletionResp_1008081` |
| `1008083` | 兼容回包 | `ChuangZhaoGongFaWnd` | `ACHIEVEMENT_ACUPOINT` | `AchievementAcupointResp_1008083` |
| `1008084` | 兼容回包 | `ZS_MianWnd` | `ENTERMATRIXASSOCIATION` | `EnterMatrixAssociationResp_1008084` |
| `1008085` | 兼容回包 | `ZS_DuiHuanWnd` | `ENTERMETHODMATRIXEXCHANGE` | `EnterMethodMatrixExchangeResp_1008085` |
| `1008087` | 兼容回包 | `ZS_DuiHuanTipWnd` | `EXCHANGEMETHODMATRIX` | `ExchangeMethodMatrixResp_1008087` |
| `1008088` | 兼容回包 | `ZS_ChuangZhaoWnd` | `VERIFYCREATEMETHODMATRIXREQ` | `VerifyCreateMethodMatrixResp_1008088` |
| `1008089` | 兼容回包 | `ZS_ZhuCeTipWnd` | `DeleteMethodMatrix` | `DeleteMethodMatrixResp_1008089` |
| `1008090` | 兼容回包 | `ZS_ZhuCeWnd` | `ENTERMETHODMATRIXREGISTER` | `EnterMethodMatrixRegisterResp_1008090` |
| `1008092` | 兼容回包 | `ZS_ZhuCeTipWnd` | `REGISTERMETHODMATRIX` | `RegisterMethodMatrixResp_1008092` |
| `1008093` | 兼容回包 | `DJSD_ZhuCeTipWnd` | `DeleteMethodAchievement` | `DeleteMethodAchievementResp_1008093` |
| `1008094` | 兼容回包 | `DST_ZhuCeTipWnd` | `DeleteMethodElixir` | `DeleteMethodElixirResp_1008094` |
| `1008095` | 兼容回包 | `LQ_ZhuCeTipWnd` | `DeleteMethodEquipment` | `DeleteMethodEquipmentResp_1008095` |
| `1008096` | 兼容回包 | `LZ_ZhuCeTipWnd` | `DeleteMethodPlant` | `DeleteMethodPlantResp_1008096` |
| `1008097` | 兼容回包 | `SFH_ZhuCeTipWnd` | `DeleteMethodSign` | `DeleteMethodSignResp_1008097` |
| `1008098` | 兼容回包 | `WFG_ZhuCeTipWnd` | `DeleteMethodSkill` | `DeleteMethodSkillResp_1008098` |

## 副本（1010）

| Cmd | Go 状态 | Unity 调用位置 | 操作或协议名称 | Go 响应类型 |
| --- | --- | --- | --- | --- |
| `1010004` | 兼容回包 | `ShaoDangWnd` | `CLEAN_MAP` | `CleanMapResp_1010004` |

## 材料模板（1011）

| Cmd | Go 状态 | Unity 调用位置 | 操作或协议名称 | Go 响应类型 |
| --- | --- | --- | --- | --- |
| `1011001` | 兼容回包 | `RoleManager` | `TEMPLATE_MATERIAL_ANIMAL_DETAIL` | `TemplateMaterialAnimalDetailResp_1011001` |
| `1011002` | 兼容回包 | `RoleManager` | `TEMPLATE_MATERIAL_MINERAL_DETAIL` | `TemplateMaterialMineralDetailResp_1011002` |
| `1011003` | 只读或示例 | `LZ_WorldCaiLiaoWnd`、`RoleManager` | 查看试用种子的产地、五行与材料属性 | `TemplateMaterialPlantDetailResp_1011003` |
| `1011005` | 兼容回包 | `RoleManager` | `TEMPLATE_ELIXIR_DETAIL` | `MethodElixirDetailResp_1011005` |
| `1011006` | 兼容回包 | `RoleManager` | `METHOD_EQUIPMENT_DETAIL` | `MethodEquipmentDetailResp_1011006` |
| `1011007` | 兼容回包 | `RoleManager` | `TEMPLATE_MATRIX_DETAIL` | `MethodMatrixDetailResp_1011007` |
| `1011008` | 兼容回包 | `RoleManager` | `TEMPLATE_SIGN_DETAIL` | `MethodSignDetailResp_1011008` |
| `1011009` | 兼容回包 | `RoleManager` | `TEMPLATE_SKILL_DETAIL` | `MethodSkillDetailResp_1011009` |

## 杂项玩法（1012）

| Cmd | Go 状态 | Unity 调用位置 | 操作或协议名称 | Go 响应类型 |
| --- | --- | --- | --- | --- |
| `1012002` | 兼容回包 | `LingQuanJiuSiWnd` | `BUY_WINE` | `BuyWineResp_1012002` |
| `1012005` | 缺少回包映射 | `LingQuanJiuSiWnd` | `CHAT_INFORM` | — |

## 灵田、种子和灵土（1013）

| Cmd | Go 状态 | Unity 调用位置 | 操作或协议名称 | Go 响应类型 |
| --- | --- | --- | --- | --- |
| `1013007` | 明确拒绝 | `LT_LingTuWnd` | 配置灵土 | `SoilDeployResp_1013007` |
| `1013010` | 明确拒绝 | `LT_LingTuWnd` | 移除灵土 | `DiscardSoilResp_1013010` |
| `1013014` | 兼容回包 | `LT_JiaoGuanWnd` | `ENTER_IRRIGATE_RESP` | `EnterIrrigateResp_1013014` |
| `1013016` | 兼容回包 | `LT_JiaoGuanWnd` | `WATER_CHOOSE_REQ` | `WaterChooseResp_1013016` |
| `1013017` | 明确拒绝 | `LT_LingYeWnd` | 配置灵液 | `WaterDeployResp_1013017` |
| `1013020` | 明确拒绝 | `LT_ZhongZiWnd` | 遗忘种子 | `ForgetSeedResp_1013020` |
| `1013021` | 兼容回包 | `NaJieSpaceWnd` | `GATHER_SEED_RESP` | `GatherSeedResp_1013021` |
| `1013024` | 兼容回包 | `LT_LingYeWnd` | `SearchWaterAllResp` | `SearchWaterAllResp_1013024` |
| `1013026` | 兼容回包 | `LT_LingYeWnd` | `DISCARD_WATERREQ` | `DiscardWaterResp_1013026` |
| `1013027` | 兼容回包 | `LT_LingTuWnd` | `SearchXiRangResp` | `SearchXiRangResp_1013027` |
| `1013029` | 兼容回包 | `LT_LingYeWnd` | `SearchLingQuanResp` | `SearchLingQuanResp_1013029` |

## 宗门（1015）

| Cmd | Go 状态 | Unity 调用位置 | 操作或协议名称 | Go 响应类型 |
| --- | --- | --- | --- | --- |
| `1015007` | 兼容回包 | `SheLiZongMenWnd` | `GUILD_CREATE` | `GuildCreateResp_1015007` |
| `1015026` | 兼容回包 | `ZongMenZhuFengWnd` | `INTOZHIFA` | `IntoZhiFaResp_1015026` |
| `1015028` | 兼容回包 | `ZongMen_ZhiFaDianWnd` | `SHEZHIZHIFACELUE` | `SheZhiZhiFaCeLueDiZiNumResp_1015028` |
| `1015029` | 兼容回包 | `ZongMenZhuFengWnd` | `GuildUpgrade` | `GuildUpgradeResp_1015029` |
| `1015030` | 兼容回包 | `ZongMenZhuFengWnd` | `INTOYANWU` | `IntoYanWuResp_1015030` |
| `1015031` | 兼容回包 | `ZongMen_YanWuTaiWnd` | `YANWUSUODINGJIANGLI` | `YanWuSuoDingJiangLiResp_1015031` |
| `1015032` | 兼容回包 | `ZongMen_YanWuTaiWnd` | `CREATEYANWUTAI` | `CreateYanWuTaiResp_1015032` |
| `1015033` | 兼容回包 | `ZongMen_YanWuTaiWnd` | `UPGRADEYANWUMATRIX` | `UpgradeYanWuMatrixResp_1015033` |
| `1015034` | 兼容回包 | `ZongMenZhuFengWnd` | `INTONEIWU` | `IntoNeiWuResp_1015034` |
| `1015036` | 兼容回包 | `ZongMen_NeiWuDianWnd` | `NEIWUJUANZENGSTONE` | `NeiWuJuanZengStoneResp_1015036` |
| `1015037` | 兼容回包 | `ZongMen_NeiWuDianWnd` | `NEIWUJUANZENGJADE` | `NeiWuJuanZengJadeResp_1015037` |
| `1015038` | 兼容回包 | `ZongMenZhuFengWnd` | `INTOWAIWU` | `IntoWaiWuResp_1015038` |
| `1015041` | 兼容回包 | `ZongMen_WaiWuDianWnd` | `WAIWUJIESUANXUANSHANG` | `WaiWuJieSuanXuanShangResp_1015041` |
| `1015042` | 兼容回包 | `ZongMenWnd` | `IntoKuangMai` | `IntoKuangMaiResp_1015042` |
| `1015044` | 兼容回包 | `ZongMen_KuangMaiWnd` | `KuangMaiStoneDiZiNum` | `KuangMaiStoneDiZiNumResp_1015044` |
| `1015045` | 兼容回包 | `ZongMen_KuangMaiWnd` | `KuangMaiJadeDiZiNum` | `KuangMaiJadeDiZiNumResp_1015045` |
| `1015046` | 兼容回包 | `ZongMen_KuangMaiWnd` | `UpgradeKuangMaiSpace` | `UpgradeKuangMaiSpaceResp_1015046` |
| `1015047` | 兼容回包 | `ZongMen_KuangMaiWnd` | `UpgradeKuangMaiFaQi` | `UpgradeKuangMaiFaQiResp_1015047` |
| `1015048` | 兼容回包 | `ZongMenWnd` | `IntoHuZongMatrix` | `IntoHuZongMatrixResp_1015048` |
| `1015050` | 兼容回包 | `ZongMen_HuZongDaZhenUpgradeWnd` | `UpgradeHuZongMatrix` | `UpgradeHuZongMatrixResp_1015050` |
| `1015051` | 兼容回包 | `ZongMen_HuZongDaZhenWnd` | `UpgradeHuZongMatrixJadeBuyTime` | `UpgradeHuZongMatrixJadeBuyTimeResp_1015051` |
| `1015052` | 兼容回包 | `ZongMen_KuangMaiWnd` | `UpgradeKuangMaiFaQiJadeBuyTime` | `UpgradeKuangMaiFaQiJadeBuyTimeResp_1015052` |
| `1015053` | 兼容回包 | `ZongMen_KuangMaiWnd` | `UpgradeKuangMaiSpaceJadeBuyTime` | `UpgradeKuangMaiSpaceJadeBuyTimeResp_1015053` |
| `1015054` | 兼容回包 | `ZongMen_YanWuTaiWnd` | `UPGRADEYANWUMATRIXJADEBUYTIME` | `UpgradeYanWuMatrixJadeBuyTimeResp_1015054` |
| `1015055` | 兼容回包 | `ZongMen_YanWuTaiWnd` | `UPGRADEYANWUTAIJADEBUYTIME` | `UpgradeYanWuTaiJadeBuyTimeResp_1015055` |
| `1015056` | 兼容回包 | `ZongMen_YanWuTaiWnd` | `CREATEYANWUTAIJADEBUYTIME` | `CreateYanWuTaiJadeBuyTimeResp_1015056` |
| `1015057` | 兼容回包 | `ZongMenWnd`、`ZongMen_LingTianWnd` | `GUILD_INTOLINGTIAN` | `IntoLingTianResp_1015057` |
| `1015059` | 兼容回包 | `ZongMen_WaiWuDianXiangQingWnd` | `WaiWuFaBuXuanShang` | `WaiWuFaBuXuanShangResp_1015059` |
| `1015060` | 兼容回包 | `ZongMen_LingTianWnd` | `GUILD_UPGRADELINGTIANSPACE` | `UpgradeLingTianSpaceResp_1015060` |
| `1015061` | 兼容回包 | `ZongMen_LingTianWnd` | `GUILD_UPGRADE_LINGTIAN_SPACEJADE_BUYTIME` | `UpgradeLingTianSpaceJadeBuyTimeResp_1015061` |
| `1015062` | 兼容回包 | `ZongMen_LingTianWnd` | `GUILD_UPGRADE_LINGTIAN_MATRIX` | `UpgradeLingTianMatrixResp_1015062` |
| `1015063` | 兼容回包 | `ZongMen_LingTianWnd` | `GUILD_UPGRADELINGTIANMATRIXJADEBUYTIME` | `UpgradeLingTianMatrixJadeBuyTimeResp_1015063` |
| `1015064` | 兼容回包 | `ZongMen_LingTianWnd` | `GUILD_UpgradeLingTian` | `UpgradeLingTianResp_1015064` |
| `1015065` | 兼容回包 | `ZongMen_LingTianWnd` | `GUILD_UpgradeLingTianJadeBuyTime` | `UpgradeLingTianJadeBuyTimeResp_1015065` |
| `1015066` | 兼容回包 | `ZongMen_LingTianWnd` | `GUILD_LingTianDiZiNum` | `LingTianDiZiNumResp_1015066` |
| `1015067` | 兼容回包 | `ZongMenWnd`、`ZongMen_YeLianWnd` | `GUILD_IntoYeLian` | `IntoYeLianResp_1015067` |
| `1015069` | 兼容回包 | `ZongMen_YeLianWnd` | `GUILD_UpgradeYeLianSpace` | `UpgradeYeLianSpaceResp_1015069` |
| `1015070` | 兼容回包 | `ZongMen_YeLianWnd` | `GUILD_UpgradeYeLianSpaceJadeBuyTime` | `UpgradeYeLianSpaceJadeBuyTimeResp_1015070` |
| `1015071` | 兼容回包 | `ZongMen_YeLianWnd` | `GUILD_UpgradeYeLianMatrix` | `UpgradeYeLianMatrixResp_1015071` |
| `1015072` | 兼容回包 | `ZongMen_YeLianWnd` | `GUILD_UpgradeYeLianMatrixJadeBuyTime` | `UpgradeYeLianMatrixJadeBuyTimeResp_1015072` |
| `1015073` | 兼容回包 | `ZongMen_YeLianWnd` | `GUILD_UpgradeYeLian` | `UpgradeYeLianResp_1015073` |
| `1015074` | 兼容回包 | `ZongMen_YeLianWnd` | `GUILD_UpgradeYeLianJadeBuyTime` | `UpgradeYeLianJadeBuyTimeResp_1015074` |
| `1015075` | 兼容回包 | `ZongMen_YeLianWnd` | `GUILD_YeLianDiZiNum` | `YeLianDiZiNumResp_1015075` |
| `1015076` | 兼容回包 | `ZongMenWnd`、`ZongMen_LianDanWnd` | `GUILD_IntoElixir` | `IntoElixirResp_1015076` |
| `1015078` | 兼容回包 | `ZongMen_LianDanWnd` | `GUILD_UpgradeElixirSpace` | `UpgradeElixirSpaceResp_1015078` |
| `1015079` | 兼容回包 | `ZongMen_LianDanWnd` | `GUILD_UpgradeElixirSpaceJadeBuyTime` | `UpgradeElixirSpaceJadeBuyTimeResp_1015079` |
| `1015080` | 兼容回包 | `ZongMen_LianDanWnd` | `GUILD_UpgradeElixirMatrix` | `UpgradeElixirMatrixResp_1015080` |
| `1015081` | 兼容回包 | `ZongMen_LianDanWnd` | `GUILD_UpgradeElixirMatrixJadeBuyTime` | `UpgradeElixirMatrixJadeBuyTimeResp_1015081` |
| `1015082` | 兼容回包 | `ZongMen_LianDanWnd` | `GUILD_UpgradeElixir` | `UpgradeElixirResp_1015082` |
| `1015083` | 兼容回包 | `ZongMen_LianDanWnd` | `GUILD_UpgradeElixirJadeBuyTime` | `UpgradeElixirJadeBuyTimeResp_1015083` |
| `1015084` | 兼容回包 | `ZongMen_LianDanWnd` | `GUILD_ElixirDiZiNum` | `ElixirDiZiNumResp_1015084` |
| `1015085` | 兼容回包 | `ZongMenWnd`、`ZongMen_LianQiWnd` | `GUILD_IntoEquipment` | `IntoEquipmentResp_1015085` |
| `1015087` | 兼容回包 | `ZongMen_LianQiWnd` | `GUILD_UpgradeEquipmentSpace` | `UpgradeEquipmentSpaceResp_1015087` |
| `1015088` | 兼容回包 | `ZongMen_LianQiWnd` | `GUILD_UpgradeEquipmentSpaceJadeBuyTime` | `UpgradeEquipmentSpaceJadeBuyTimeResp_1015088` |
| `1015089` | 兼容回包 | `ZongMen_LianQiWnd` | `GUILD_UpgradeEquipmentMatrix` | `UpgradeEquipmentMatrixResp_1015089` |
| `1015090` | 兼容回包 | `ZongMen_LianQiWnd` | `GUILD_UpgradeEquipmentMatrixJadeBuyTime` | `UpgradeEquipmentMatrixJadeBuyTimeResp_1015090` |
| `1015091` | 兼容回包 | `ZongMen_LianQiWnd` | `GUILD_UpgradeEquipment` | `UpgradeEquipmentResp_1015091` |
| `1015092` | 兼容回包 | `ZongMen_LianQiWnd` | `GUILD_UpgradeEquipmentJadeBuyTime` | `UpgradeEquipmentJadeBuyTimeResp_1015092` |
| `1015093` | 兼容回包 | `ZongMen_LianQiWnd` | `GUILD_EquipmentDiZiNum` | `EquipmentDiZiNumResp_1015093` |
| `1015094` | 兼容回包 | `ZongMenWnd`、`ZongMen_ZhenFaWnd` | `GUILD_IntoMatrix` | `IntoMatrixResp_1015094` |
| `1015096` | 兼容回包 | `ZongMen_ZhenFaWnd` | `GUILD_UpgradeMatrixSpace` | `UpgradeMatrixSpaceResp_1015096` |
| `1015098` | 兼容回包 | `ZongMen_ZhenFaWnd` | `GUILD_UpgradeMatrixMatrix` | `UpgradeMatrixMatrixResp_1015098` |
| `1015099` | 兼容回包 | `ZongMen_ZhenFaWnd` | `GUILD_UpgradeMatrixMatrixJadeBuyTime` | `UpgradeMatrixMatrixJadeBuyTimeResp_1015099` |
| `1015100` | 兼容回包 | `ZongMen_ZhenFaWnd`、`ZongMen_ZhiFuWnd` | `GUILD_UpgradeMatrix` | `UpgradeMatrixResp_1015100` |
| `1015102` | 兼容回包 | `ZongMen_ZhenFaWnd` | `GUILD_MatrixDiZiNum` | `MatrixDiZiNumResp_1015102` |
| `1015103` | 兼容回包 | `ZongMenWnd`、`ZongMen_GongFangWnd` | `GUILD_IntoGongFang` | `IntoGongFangResp_1015103` |
| `1015105` | 兼容回包 | `ZongMen_GongFangWnd` | `GUILD_UpgradeGongFangSpace` | `UpgradeGongFangSpaceResp_1015105` |
| `1015106` | 兼容回包 | `ZongMen_GongFangWnd`、`ZongMen_ZhenFaWnd` | `GUILD_UpgradeGongFangSpaceJadeBuyTime` | `UpgradeGongFangSpaceJadeBuyTimeResp_1015106` |
| `1015107` | 兼容回包 | `ZongMen_GongFangWnd` | `GUILD_UpgradeGongFangMatrix` | `UpgradeGongFangFaQiResp_1015107` |
| `1015108` | 兼容回包 | `ZongMen_GongFangWnd` | `GUILD_UpgradeGongFangMatrixJadeBuyTime` | `UpgradeGongFangFaQiJadeBuyTimeResp_1015108` |
| `1015109` | 兼容回包 | `ZongMen_GongFangWnd` | `GUILD_UpgradeGongFang` | `UpgradeGongFangResp_1015109` |
| `1015110` | 兼容回包 | `ZongMen_GongFangWnd` | `GUILD_UpgradeGongFangJadeBuyTime` | `UpgradeGongFangJadeBuyTimeResp_1015110` |
| `1015111` | 兼容回包 | `ZongMen_GongFangWnd` | `GUILD_GongFangDiZiNum` | `GongFangDiZiNumResp_1015111` |
| `1015112` | 兼容回包 | `ZongMenWnd`、`ZongMen_ZhiFuWnd` | `GUILD_IntoSign` | `IntoSignResp_1015112` |
| `1015114` | 兼容回包 | `ZongMen_ZhiFuWnd` | `GUILD_UpgradeFuZhi` | `UpgradeFuZhiResp_1015114` |
| `1015115` | 兼容回包 | `ZongMen_ZhiFuWnd` | `GUILD_UpgradeFuZhiJadeBuyTime` | `UpgradeFuZhiJadeBuyTimeResp_1015115` |
| `1015116` | 兼容回包 | `ZongMen_ZhiFuWnd` | `GUILD_UpgradeFuBi` | `UpgradeFuBiResp_1015116` |
| `1015117` | 兼容回包 | `ZongMen_ZhiFuWnd` | `GUILD_UpgradeFuBiJadeBuyTime` | `UpgradeFuBiJadeBuyTimeResp_1015117` |
| `1015119` | 兼容回包 | `ZongMen_ZhiFuWnd` | `GUILD_UpgradeSignJadeBuyTime` | `UpgradeSignJadeBuyTimeResp_1015119` |
| `1015120` | 兼容回包 | `ZongMen_ZhiFuWnd` | `GUILD_SignDiZiNum` | `SignDiZiNumResp_1015120` |
| `1015130` | 兼容回包 | `ZongMenWnd`、`ZongMen_NeiMenWnd` | `GUILD_IntoNeiMen` | `IntoNeiMenResp_1015130` |
| `1015132` | 兼容回包 | `ZongMen_NeiMenWnd` | `GUILD_NeiMenDiZiNum` | `NeiMenDiZiNumResp_1015132` |
| `1015133` | 兼容回包 | `ZongMen_NeiMenWnd` | `GUILD_JuanZengGongFa` | `JuanZengGongFaResp_1015133` |
| `1015134` | 兼容回包 | `ZongMen_NeiMenWnd` | `GUILD_JuanZengFaQi` | `JuanZengFaQiResp_1015134` |
| `1015135` | 兼容回包 | `ZongMen_NeiMenWnd` | `GUILD_JuanZengFaShu` | `JuanZengFaShuResp_1015135` |
| `1015136` | 兼容回包 | `ZongMen_NeiMenWnd` | `GUILD_PeiFaGongFa` | `PeiFaGongFaResp_1015136` |
| `1015138` | 兼容回包 | `ZongMen_NeiMenWnd` | `GUILD_PeiFaFaQi` | `PeiFaFaQiResp_1015138` |
| `1015140` | 兼容回包 | `ZongMen_NeiMenWnd` | `GUILD_PeiFaFaShu` | `PeiFaFaShuResp_1015140` |
| `1015142` | 兼容回包 | `ZongMen_YanWuTaiWnd` | `UPGRADEYANWUTAI` | `UpgradeYanWuTaiResp_1015142` |
| `1015143` | 兼容回包 | `ZongMen_WaiWuDianXiangQingWnd` | `WaiWuXuanShangGoodsSearch` | `WaiWuXuanShangGoodsSearchResp_1015143` |
| `1015145` | 兼容回包 | `ZongMenZhuFengWnd` | `EnterCangBaoGe` | `EnterCangBaoGeResp_1015145` |
| `1015146` | 兼容回包 | `ZongMen_CangBaoGeWnd` | `GouMaiWuPin` | `GouMaiWuPinResp_1015146` |
| `1015147` | 兼容回包 | `ZongMenZhuFengWnd` | `GuildUpgradeJadeBuyTime` | `GuildUpgradeJadeBuyTimeResp_1015147` |
| `1015148` | 兼容回包 | `ZongMenZhuFengWnd` | `GuildHillUpdateName` | `GuildHillUpdateNameResp_1015148` |
| `1015149` | 兼容回包 | `ZongMen_HuZongDaZhenWnd` | `HuZongMatrixUpdateEye` | `HuZongMatrixUpdateEyeResp_1015149` |
| `1015153` | 兼容回包 | `ZongMen_NeiWuDianWnd` | `NeiWuSalarySetting` | `NeiWuSalarySettingResp_1015153` |
| `1015154` | 兼容回包 | `ZongMen_ShiKongMiJingWnd` | `EnterShiKongMiJing` | `EnterShiKongMiJingResp_1015154` |
| `1015155` | 兼容回包 | `ZongMenChouJiangEventWnd` | `EnterGuildChouJiangChi` | `EnterGuildChouJiangChiResp_1015155` |
| `1015156` | 兼容回包 | `ZongMenChouJiangEventWnd` | `GuildChouJiang` | `GuildChouJiangResp_1015156` |

## 任务及天书碑（1016）

| Cmd | Go 状态 | Unity 调用位置 | 操作或协议名称 | Go 响应类型 |
| --- | --- | --- | --- | --- |
| `1016004` | 兼容回包 | `CZF_TSBContentPanel` | `OPEN_TIANSHUBEI` | `OpenTianShuBeiResp_1016004` |
| `1016006` | 兼容回包 | `CZF_TSBContentPanel` | `TIANSHUBEI_RECEIVE_AWARD` | `TianShuBeiReceiveAwardResp_1016006` |

## 活动与积分（1017）

| Cmd | Go 状态 | Unity 调用位置 | 操作或协议名称 | Go 响应类型 |
| --- | --- | --- | --- | --- |
| `1017001` | 只读或示例 | `ChouJiangEventWnd` | 查看试用抽奖池的次数和积分 | `EnterChouJiangChiResp_1017001` |
| `1017002` | 明确拒绝 | `ChouJiangEventWnd` | 抽奖 | `ChouJiangResp_1017002` |
| `1017004` | 只读或示例 | `ChouJiangEventWnd` | 查看试用积分兑换档位 | `EnterJiFenResp_1017004` |
| `1017006` | 明确拒绝 | `JiFenDuiHuanWnd` | 兑换积分奖励 | `GetJiFenResp_1017006` |

## 其他系统（1018）

| Cmd | Go 状态 | Unity 调用位置 | 操作或协议名称 | Go 响应类型 |
| --- | --- | --- | --- | --- |
| `1018002` | 兼容回包 | `ZongMenShouChaoWnd` | `GuildMonsterFightingBroadcast` | `GuildMonsterFightingBroadcastResp_1018002` |
| `1018003` | 兼容回包 | `ZongMenShouChaoWnd` | `GuildMonsterFightingRoleHelp` | `GuildMonsterFightingRoleHelpResp_1018003` |

## 战斗服与独立聊天服

| 通道 | Cmd | 状态 | Unity 调用位置或消息名 |
| --- | --- | --- | --- |
| 战斗服 | `8001004` | 静默处理 | `PlayerEntity` · `BATTLE_ROLE_MOVE` |
| 战斗服 | `8001006` | 静默处理 | `MonsterEntity` · `MONSTER_GET_DAMAGE` |
| 战斗服 | `8001009` | 无回包 | `BattleSceneManager` · `ENTER_TOWER_NEXT_LEVEL` |
| 战斗服 | `8001011` | 无回包 | `PlayerEntity` · `Revive` |
| 战斗服 | `8001014` | 静默处理 | `PlayerEntity` · `RoleGetDamage` |
| 独立聊天服 | `4001001` | 未接入独立通道 | `ChatManager` · `CHAT_JOIN` |
| 独立聊天服 | `4001003` | 未接入独立通道 | `AH_PaiMaiHuWnd`、`LingQuanJiuSiWnd`、`ZongMenChatWnd` · `SEND_MESSAGE` |
| 独立聊天服 | `4001004` | 未接入独立通道 | `ChatManager` · `WINE_OUT` |

战斗心跳为预期的静默路径。道友私聊通过游戏服的专用接口实现，独立聊天服条目另行跟进。
