/* 界面样稿中出现的人物、库存和进度为示例存档数据。 */
(() => {
  'use strict';
  const guildPages = {
    BaiRuShanMenWnd: ['拜帖', '云岫宗', '朝露郡 · 青溪山门', '青溪谷地向赴山散修开放。', '宗门等阶/三阶|长老/6 位|弟子/124 位|影响力/2,460', '递入拜帖'],
    SheLiZongMenWnd: ['建宗', '择山立宗', '宗名 · 山门 · 信物', '云岫宗', '宗门简称/云岫|建宗地点/临仙郡|可用灵峰/3 座|宗门令牌/日曜令牌 ×1', '设立宗门'],
    ZongMenChatWnd: ['传讯', '宗门传讯', '云岫宗 · 同门 124 人', '今日山门', '姜晚照/暮鼓之后，共赴演武台。|沈师兄/东峰阵石已补齐。|陆青禾/新收的凝露草在藏宝阁。', '发送传讯'],
    ZongMenChengYuanWnd: ['名册', '山门弟子', '云岫宗 · 在籍 124 位', '宗内名录', '陆青禾/内门弟子 · 筑基后期|谢闻川/外门弟子 · 筑基初期|裴知秋/长老 · 金丹初期|沈砚/内门弟子 · 筑基中期', '查看入宗申请'],
    ZongMenChouJiangEventWnd: ['抽签', '山门赐福', '宗门抽奖 · 今日余 3 次', '福签', '经脉丹/丹药|宗门贡献/贡献点|仙金/灵材|神术/秘卷', '抽取一次'],
    ZongMenShouChaoWnd: ['阵地', '兽潮来袭', '东山关 · 第三波', '东线守阵', '震位／东/青木猿 · 7 只|巽位／东南/焰纹狼 · 4 只|当前防护/7,260|山门守卫/42 人', '前往守阵'],
    ZongMenWnd: ['山门', '云岫宗', '青溪山门 · 三阶宗门', '主峰与诸峰', '主峰/宗门大殿 · 长老殿|东峰/护宗大阵 · 三阶|南峰/丹房 · 灵田|西峰/矿脉 · 冶炼', '进入主峰'],
    ZongMenZhuFengWnd: ['山门', '栖云峰', '云岫宗 · 主峰', '主峰殿阁', '宗门大殿/宗主议事|长老殿/任命与职务|执法殿/弟子与战力|藏宝阁/贡献兑换', '查看宗门升级'],
    ZongMen_CangBaoGeWnd: ['藏品', '山门珍藏', '可用贡献 · 680 点', '贡献兑换', '青脉丹/80 贡献 · 余 6 瓶|玄铁剑胚/160 贡献 · 余 3 件|轻身符/40 贡献 · 余 8 张|护山阵盘/240 贡献 · 余 2 面', '兑换选中藏品'],
    ZongMen_DaDianWnd: ['山门', '宗门大殿', '云岫宗 · 栖云峰', '宗主　姜晚照', '宗门等阶/三阶|影响力/2,460|战斗力/18,670|弟子人数/124 人', '查看宗务'],
    ZongMen_DaDianXieRenWnd: ['名册', '推举继任宗主', '现任宗主 · 姜晚照', '继任人须在宗内长老中选出。', '裴知秋/护宗长老 · 入宗 260 日|方闻舟/内务长老 · 入宗 194 日|何昭/外务长老 · 入宗 182 日', '确认继任人'],
    ZongMen_HuZongDaZhenUpgradeWnd: ['升级', '护宗大阵', '阵法晋阶 · 三阶至四阶', '升阶所需', '基础防御/1,000 → 1,450|灵石/3,800 ／ 3,000|四阶阵盘/1 ／ 1|山门等阶/已达要求', '确认升阶'],
    ZongMen_HuZongDaZhenWnd: ['阵地', '四象护山阵', '护宗长老 · 裴知秋', '三阶护阵', '阵法防御/7,260|开启消耗/灵石 ×200|阵法核心/护山阵盘|当前阵势/守势', '开启大阵'],
    ZongMen_NeiMenWnd: ['名册', '内门弟子', '听松峰 · 内门', '总战力　11,650', '精英弟子/16 人|内门弟子/48 人|可选弟子/12 人|每人供奉/120 灵石', '分派弟子'],
    ZongMen_NeiWuDianWnd: ['捐赠', '内务捐赠', '内务长老 · 方闻舟', '今日仍可捐赠 8 次', '一袋灵石/100 灵石|一束灵植/凝露草 ×5|一盒灵材/寒纹铁 ×3|宗门贡献/当前 680 点', '捐赠所选物资'],
    ZongMen_ShiKongMiJingWnd: ['秘境', '时空秘境', '宗门秘境 · 静修', '一日静修', '投入贡献/100 点|收益时间/一日|神识增长/+24|真元增长/+36', '进入秘境'],
    ZongMen_SpeedUpWnd: ['加速', '缩短营造时间', '主峰 · 正在升阶', '玉漏', '剩余工期/02时 20分|投入灵玉/12|缩减时间/01时 00分|加速后剩余/01时 20分', '确认投入'],
    ZongMen_WaiWuDianWnd: ['传令', '宗门悬赏', '外务长老 · 何昭', '当前悬赏', '凝露草/搜寻灵植 · 120 灵石|玄水鳞/寻妖取材 · 240 灵石|赤烈砂/采矿委托 · 180 灵石', '发布悬赏'],
    ZongMen_WaiWuDianXiangQingWnd: ['捐赠', '悬赏物品', '外务殿 · 新委托', '寻找凝露草', '物品类别/灵植|目标数量/6 株|悬赏灵石/120|消耗贡献/20 点', '确认发布'],
    ZongMen_YanWuTaiWnd: ['比武', '宗门大比', '演武台 · 每日十九时', '今日奖池', '筹备进度/已定 2 项|比武场地/栖云峰|夺魁奖励/灵石 ×300|次席奖励/青脉丹 ×2', '锁定今日奖励'],
    ZongMen_ZhangLaoRenMingWnd: ['名册', '长老任命', '候选人 · 沈砚', '选择职务', '执法长老/统领弟子|矿脉主事/掌管矿产|炼器峰主/负责器坊|灵兽峰主/照料灵兽', '确认任命'],
    ZongMen_ZhangLaoWnd: ['名册', '宗门长老', '云岫宗 · 六席', '在任长老', '裴知秋/护宗长老 · 金丹初期|方闻舟/内务长老 · 金丹中期|何昭/外务长老 · 金丹初期|姜晚照/宗主 · 元婴初期', '查看候选弟子'],
    ZongMen_ZhiFaDianWnd: ['名册', '山门执法', '执法殿 · 弟子清册', '弟子巡防', '内门弟子/48 人|外门弟子/76 人|宗门战力/18,670|今日巡山/12 人', '查看弟子情况']
  };
  const production = {
    GongFang: ['工坊', '灵材', '青纹木', '工坊长老', '锯材台'],
    KuangMai: ['矿脉', '灵石', '寒纹铁', '矿脉长老', '西山矿井'],
    LianDan: ['丹峰', '丹药', '凝露丹', '炼丹长老', '地火室'],
    LianQi: ['器峰', '法器', '霜纹剑', '炼器长老', '锻造台'],
    LingTian: ['灵田', '灵植', '凝露草', '灵田长老', '朝露药圃'],
    YeLian: ['冶炼峰', '灵矿', '寒渊银砂', '冶炼长老', '熔矿炉'],
    YuShou: ['御兽园', '兽材', '逐风兽筋', '御兽长老', '灵兽苑'],
    ZhenFa: ['阵峰', '阵盘', '四象阵盘', '阵法长老', '刻阵室'],
    ZhiFu: ['符峰', '符箓', '轻身符', '制符长老', '制符静室']
  };
  for (const [id, [place, goods, item, officer, site]] of Object.entries(production)) {
    guildPages[`ZongMen_${id}Wnd`] = ['营产', place, `云岫宗 · ${officer}`, site,
      `${goods}库存/64 ／ 120|当前产物/${item}|生产弟子/8 ／ 12|当前产量/每时 6 份`, '调配弟子'];
  }
  window.sectFacilitySpecs = guildPages;
  // 不同技艺共享兑换、登记与创作流程，各自独立使用本门作品和材料。
  const arts = {
    dan: ['丹师塔', '丹方', '丹师', '凝露丹', '青脉丹', '赤阳振元丹', '凝露草', '寒露液', '赤阳炉火'],
    forge: ['炼器协会', '器图', '炼器师', '听泉杖', '霜纹剑', '星纹环', '云纹木', '寒露液', '松炭炉火'],
    fu: ['神符会', '符书', '符师', '润泽符', '轻身符', '引雷符', '水纹符纸', '寒露墨', '玄鳞符笔'],
    array: ['阵师协会', '阵图', '阵师', '四象护山阵', '聚灵阵', '锁风阵', '青石阵盘', '定星砂', '九宫阵心'],
    seed: ['灵植协会', '灵植', '灵植师', '凝露草', '赤绒苔', '霜节芦', '凝露草种', '寒露液', '息壤'],
    sutra: ['道境圣殿', '功法', '布道师', '槐阴养生诀', '照影定心诀', '听泉引气诀', '足阳明', '任脉', '纳气'],
    spell: ['万法阁', '法术', '术师', '不动明王印', '引雷诀', '御风术', '人迎穴', '五行印', '手印']
  };
  const craft = {};
  function add(id, art, mode, hero, context, section, rows, primary) {
    craft[id] = {art, mode, hero, context, section, rows, primary};
  }
  const hubs = {DanShiTaWnd:'dan', LianQi_ChuangZaoMianWnd:'forge', SFH_ShenFuHuiWnd:'fu', ZS_MianWnd:'array', LZ_MianWnd:'seed', DaoJingShengDianWnd:'sutra', WanFaGeWnd:'spell'};
  for (const [id, art] of Object.entries(hubs)) {
    const [hall, kind, rank, one] = arts[art];
    const actions = [`模拟创造${kind}/推演新作`, `${kind}兑换/当前贡献 680 点`, `${kind}登记/待登记 2 件`];
    if (art === 'forge') actions.unshift('创制工坊/投料开炉');
    if (art === 'dan') actions.push('丹师考核/晋升品阶');
    add(id, art, 'hall', hall, `虚神殿 · 二品${rank}`, '堂内事务', actions.join('|'), art === 'forge' ? '进入创制工坊' : `开始${kind}推演`);
  }
  function school(id) {
    if (/^(LQ_|LianQi|Forge|Creation|Affix)/.test(id)) return 'forge';
    if (/^(DST_|DiHuo|DanShi|ChuangZhao_LianZhi)/.test(id)) return 'dan';
    if (/^(SFH_|HuaFu)/.test(id)) return 'fu';
    if (/^(ZS_|ZF_)/.test(id)) return 'array';
    if (/^(LT_|LZ_|WorldCaiLiao|CraftCodex)/.test(id)) return 'seed';
    if (/^(DJSD_|ChuangZhaoGongFa)/.test(id)) return 'sutra';
    if (/^(WFG_|ChuangZaoFaShu|ChuangZhaoFaShu)/.test(id)) return 'spell';
    return 'celestial';
  }
  function archivePage(id) {
    const art = school(id), family = arts[art];
    if (!family) return null;
    const [hall, kind, rank, one, two, three] = family;
    const meta = `${hall} · 二品${rank}`;
    if (/DuiHuanShaiXuan/.test(id)) return {art,mode:'filter',hero:`择取${kind}`,context:meta,section:'筛选条件',rows:`等阶/一阶　 二阶　 三阶|五行/金　木　水　火　土|品质/下品　 中品　 上品|${kind === '丹方' ? '药效/疗伤　 回气　 解毒' : kind === '功法' ? '途径/炼气　 炼体　 炼神' : '法系/攻击　 护盾　 增益'}`,primary:'应用筛选'};
    if (/DuiHuanTip/.test(id)) return {art,mode:'confirm',hero:one,context:`${hall} · 兑换确认`,section:'兑换明细',rows:`${kind}品阶/黄阶上品|所需贡献/120 点|现有贡献/680 点|兑换后余额/560 点`,primary:`兑换${kind}`};
    if (/DuiHuan/.test(id)) return {art,mode:'catalog',hero:`${kind}藏录`,context:`${hall} · 贡献 680 点`,section:'可兑换',rows:`${one}/黄阶上品 · 120 贡献|${two}/玄阶下品 · 220 贡献|${three}/玄阶中品 · 320 贡献`,primary:`查看${kind}`};
    if (/ZhuCeTip/.test(id)) return {art,mode:'name',hero:one,context:`${hall} · 命名登记`,section:'登记信息',rows:`作品名称/${one}|门类/${kind}|创作人/陆青禾|所属/${hall}`,primary:'确认登记'};
    if (/YiZhuCeList/.test(id)) return {art,mode:'catalog',hero:'已登记功法',context:`${hall} · 我的道藏`,section:'登记簿',rows:`${one}/第一卷 · 已登记|${two}/第二卷 · 已登记|${three}/第三卷 · 已登记`,primary:'查看功法详情'};
    if (/ZhuCeWnd/.test(id)) return {art,mode:'catalog',hero:`待登记${kind}`,context:`${hall} · 我的手稿`,section:'选择作品',rows:`${one}/黄阶上品 · 未命名|${two}/玄阶下品 · 草稿|${three}/玄阶中品 · 草稿`,primary:'命名并登记'};
    if (/JieGuo|CreateResult/.test(id)) return {art,mode:'result',hero:one,context:`${hall} · 推演完成`,section:'本次推演',rows:`所得${kind}/${one}|创作品阶/黄阶上品|悟性消耗/50 点|下一步/可前往${hall}登记`,primary:'继续创造'};
    return null;
  }
  add('ChuangZaoFaShuWnd','spell','circuit','不动明王印','万法阁 · 印法推演','印法配置','当前手印/五行印|引灵穴位/人迎穴|归属五行/土|运转次序/起印 → 导灵 → 结印','模拟印法');
  add('ChuangZhaoGongFaWnd','sutra','circuit','槐阴养生诀','道境圣殿 · 周天推演','经络配置','选中穴位/承泣|归经/足阳明胃经|归脉/阳跷脉|导灵方向/承泣 → 四白 → 巨髎','模拟运行');
  add('ChuangZhao_LianZhiDengJieWnd','dan','grade','选择炼制等阶','地火室 · 试炼准备','可选等阶','一阶/开炉入门|二阶/凝液小成|三阶/引火融丹|四阶/进阶试炼','确定等阶');
  add('DST_DanShiRenZhengWnd','dan','grade','晋升三阶丹师','丹师塔 · 职业考核','考核内容','当前等阶/二阶丹师|目标等阶/三阶丹师|考核丹方/青脉丹|剩余机会/3 次','开始考核');
  add('DST_ChuangTaWnd','dan','tower','丹塔九层','丹师塔 · 闯塔','闯塔记录','最高纪录/第九层|本层试炼/调火凝液|道境榜/前往查看|岁月榜/前往查看','开始闯塔');
  add('DiHuoMiShiWnd','dan','hall','地火密室','地火阁 · 内室','丹室物件','地火炉/引火口已开启|炉火/松炭炉火|丹方/凝露丹|药柜/查看炉料','查看地火炉');
  add('DiHuoGeCaiLiaoChooseWnd','dan','pick','凝露草','凝露丹 · 主药选取','可用药材','凝露草/朝露郡 · 已有 6 株|赤绒苔/紫霞郡 · 已有 3 株|霜节芦/寒渊郡 · 已有 2 株','加入丹方');
  add('DiHuoGeCaiLiaoJiaGeWnd','dan','purchase','龙墨商行','药材采购 · 当前灵石 820','购买清单','凝露草 ×2/40 灵石|寒露液 ×1/28 灵石|合计/68 灵石|购后剩余/752 灵石','确认购买');
  add('DiHuoGeLianDanJieSuanWnd','dan','result','凝露丹','地火阁 · 炼制成功','本炉记录','获得丹药/黄阶上品 ×2|药材消耗/凝露草 ×2 · 寒露液 ×1|精力消耗/10 点|入袋/纳戒 · 丹药栏','收取丹药');
  add('DiHuoGeLianZhiBuZhouSheZhiWnd','dan','steps','凝露丹','地火阁 · 炼制步骤','工序顺序','点炉/松炭起火|投料/凝露草 ×2|凝液/加入寒露液|收丹/文火温养','开始炼制');
  add('DiHuoGeLianZhiWnd','dan','furnace','凝露丹','地火阁 · 炼制中','炉内状态','当前步骤/凝液|炉温/640 度|下一步/温养收丹|已投入/凝露草 ×2 · 寒露液 ×1','控制炉火');
  add('HeDaoTaiWnd','celestial','circuit','阴阳合道','虚神界 · 合道台','合道准备','正位/已置入道印|辅位/尚待选取|本次投入/一枚道印|可选物品/查看纳戒','选择合道物品');
  add('HuaFuWnd','fu','talisman','润泽符','洞府静室 · 手绘','落笔次序','第一笔/开符首|第二笔/引水纹|第三笔/合符胆|符墨/寒露墨','落笔行符');
  add('SFH_CreateWnd','fu','talisman','水纹符式','神符会 · 创造符书','推演笔路','起笔/聚灵|行笔/引水|收笔/封纹|本次消耗/精力 10 点','模拟绘制');
  add('SFH_CreateChooseWnd','fu','pick','润泽符','神符会 · 创造准备','选择符书原型','润泽符/水行 · 疗伤|轻身符/木行 · 行旅|引雷符/火行 · 雷伤','开始创作');
  add('ZF_LianZhiWnd','array','circuit','四象阵盘','静室 · 阵盘刻制','阵盘刻印','当前方位/震位|剩余移动/10 步|阵心/青石盘|已连阵位/3 ／ 8','落位刻阵');
  add('ZS_ChuangZhaoWnd','array','circuit','八门九星阵','阵师协会 · 阵图推演','所选阵位','九星/天辅|八门/杜门|阵位/巽四宫|值符/天心','推演阵图');
  add('LT_LingTianMainWnd','seed','fields','朝露药圃','洞府 · 灵田','地块记录','壹号田/凝露草 · 可采|贰号田/霜节芦 · 生长中|叁号田/赤绒苔 · 待浇灌|肆号田/空地 · 可播种','管理所选灵田');
  add('LT_JiaoGuanWnd','seed','grow','赤绒苔','三号灵田 · 浇灌','浇灌记录','作物状态/待浇灌|投入灵液/天灵液 ×1|可加速/100 分钟|预计成熟/还需 42 分钟','浇灌灵田');
  add('LT_LingTuWnd','seed','mix','息壤灵土','洞府 · 土料调配','调配用材','土基/息壤 ×1|辅料/兽骨 ×2|矿质/听风青石 ×1|存土/2 份','调配灵土');
  add('LT_LingYeWnd','seed','mix','天灵液','洞府 · 灵液调配','调配用材','液基/寒露液 ×1|辅料/灵泉水 ×2|精血/青木猿精血 ×1|存量/3 瓶','调配灵液');
  add('LT_ShoHuoWnd','seed','result','凝露草','一号灵田 · 成熟待采','本季收获','成熟作物/凝露草|预计收成/6 株|种植地块/一号灵田|收取位置/纳戒 · 灵植','收获入袋');
  add('LT_TuRangGenHuanWnd','seed','pick','息壤','一号灵田 · 更换土壤','可用灵土','原有土壤/普通灵土|息壤/保水 · 2 份|青砂土/透气 · 3 份|赤玉土/蓄热 · 1 份','确认更换');
  add('LT_ZhongZiWnd','seed','catalog','育种室','洞府 · 藏种 8 ／ 50','种苗收藏','凝露草种/朝露郡 · 6 粒|霜节芦种/寒渊郡 · 3 粒|赤绒苔种/紫霞郡 · 4 粒','查看种苗');
  add('LZ_ChuangZaoWnd','seed','grow','培育新种','灵植协会 · 模拟培育','育种记录','母株/凝露草|父株/霜节芦|目标等阶/一阶|栽培土壤/息壤','下一步');
  add('LianQiBuZhouSheZhiWnd','forge','steps','听泉杖','炼器协会 · 工艺编排','工序顺序','点火/松炭炉火|入炉/云纹木 ×2|淬炼/加入寒露液|成形/锻打至定型','开始尝试');
  add('LianQiFangWnd','forge','hammer','听泉杖','炼器房 · 锻打中','锻造记录','完成度/64%|锻打次数/12 次|当前器胚/听泉杖|目标品阶/黄阶下品','继续锻打');
  add('LianQiJieSuan_ChengGongWnd','forge','result','听泉杖','炼器房 · 炼制成功','成品与消耗','获得法器/听泉杖 ×1|所耗材料/云纹木 ×2 · 寒露液 ×1|精力消耗/10 点|成品品阶/黄阶下品','收取法器');
  add('LianQiJieSuan_ShiBaiWnd','forge','failed','器胚未成形','炼器房 · 本炉未成','损耗与返还','所得经验/2 点|返还材料/云纹木 ×1|损耗/云纹木 ×1 · 寒露液 ×1|精力消耗/10 点','再试一次');
  add('LianQiCreateChooseStageWnd','forge','grade','选择器图等阶','炼器协会 · 推演准备','目标等阶','一阶/初识器理|二阶/熔材成形|三阶/引灵铸胚|四阶/刻纹淬灵','确定等阶');
  add('WangJiMiLingWnd','celestial','name','重设升仙密令','升仙司 · 账号验证','验证身份','升仙号牌/请输入升仙号牌|暗号/输入收到的暗号|新密令/输入新密令|确认密令/再次输入密令','重设密令');
  add('XuShenJieWnd','celestial','hall','虚神界','神游 · 虚境','虚神界地标','虚神殿/百艺会馆|虚神塔/试炼与榜单|虚神祭坛/供奉祭物|合道台/阴阳合道','前往虚神殿');
  add('XuShenDianWnd','celestial','hall','虚神殿','虚神界 · 百艺会馆','七门百艺','丹师塔 · 万法阁/丹法与术法|道境圣殿 · 神符会/功法与符书|灵植协会 · 炼器协会/栽培与炼器|阵师协会/阵图推演','进入所选会馆');
  add('XuShenJiTanWnd','celestial','circuit','虚神祭坛','虚神界 · 献祭','供案','祭品/已选择 1 件|祭坛灵气/充盈|可得典籍/道德经|来源/纳戒','确认献祭');
  add('XuShenTaWnd','celestial','tower','虚神塔','虚神界 · 闯塔试炼','闯塔记录','个人纪录/第六十层|挑战目标/第六十一层|道境榜/查看|天骄榜/查看','进入试炼');
  add('XuShenTaBangDanWnd','celestial','rank','岁月榜','虚神塔 · 结丹境','本期名次','姜晚照/第 1 名 · 72 层|裴知秋/第 2 名 · 70 层|方闻舟/第 3 名 · 68 层|我的排行/第 40 名 · 60 层','查看排行奖励');
  for (const [id, art, place, manual, auto] of [
    ['DiHuoGeWnd','dan','地火阁','自行炼制','灵傀炼制'], ['HuaFuChooseWnd','fu','洞府静室','自行绘制','灵傀绘制'],
    ['LianQiChooseWnd','forge','炼器室','自行炼制','灵傀炼制'], ['ZF_CreateChooseWnd','array','阵盘刻制','自行刻制','灵傀刻制']
  ]) {
    const [,kind,,one,,,mat,aid] = arts[art];
    add(id, art, 'path', one, `${place} · ${kind}选择`, '制作方式', `${manual}/手动完成本次工序|${auto}/交由灵傀执行|当前${kind}/${one}|所需材料/${mat} · ${aid}`, '选择制作方式');
  }
  for (const [id, art, puppet] of [
    ['DiHuoGeCaiLiaoShaiXuanWnd','dan',false],['DiHuoGeCaiLiaoShaiXuan_LK_Wnd','dan',true],
    ['LianQiCaiLiaoShaiXuanWnd','forge',false],['LianQiCaiLiaoShaiXuan_LK_Wnd','forge',true],
    ['HuaFuChoose_LK_Wnd','fu',true],['ZF_Choose_LK_Wnd','array',true]
  ]) {
    const [hall,,,one,,,mat,aid,tool] = arts[art];
    add(id,art,'materials',one,`${hall} · ${puppet ? '灵傀代工' : '手动备料'}`,'所需材料',`${mat}/需要 2 · 持有 6|${aid}/需要 1 · 持有 3|${tool}/已就绪|${puppet ? '灵傀耗材/需要 1 · 持有 2' : '缺少的材料/可由龙墨商行补齐'}`, puppet ? '委托灵傀' : '确认用料');
  }
  add('LZ_CaiLiaoShaiXuanWnd','seed','filter','寻觅灵植','灵植协会 · 材料筛选','植物条件','五行/金　木　水　火　土|四气/寒　凉　温　热|五味/酸　苦　甘　辛　咸|产地/朝露郡 · 临仙郡','应用筛选');
  add('LianQiCaiLiaoXuanZeShaiXuanWnd','forge','filter','炼器选材','炼器协会 · 材料筛选','材料条件','材料门类/矿物　植物　妖兽|五行/金　木　水　火　土|材性/强度　 韧度|产地/临仙郡 · 寒渊郡','应用筛选');
  add('LZ_WorldCaiLiaoWnd','seed','specimen','凝露草','灵植图鉴 · 药用灵植','本草档案','产地/朝露郡|属性/木行|药性/凉|可制丹药/凝露丹 · 青脉丹','选定灵植');
  add('WorldCaiLiaoWnd','seed','specimen','霜节芦','世界材料 · 寒渊郡','材料档案','产地/寒渊郡|采集途径/水岸湿地|适用丹方/霜脉清毒丹|用量/每炉 2 份','选定材料');
  add('ForgeWorkshop','forge','workshop','听泉杖','创制工坊 · 配料台','工坊配料','器胚/听泉杖|炉火/松炭炉火|目标品阶/黄阶下品|云纹木/投入 2 份|寒露液/投入 1 份|作品名/溪声', '开炉');
  add('ForgeFlameChoice','forge','pick','松炭炉火','创制工坊 · 选炉火','炉火目录','松炭炉火/黄阶下品 · 凝形|青泉阴焰/玄阶中品 · 护材、淬灵|赤阳炉火/火行 · 可选炉火|星陨真火/可选炉火','选用此火');
  add('ForgeGradeChoice','forge','grade','玄阶上品','创制工坊 · 目标品阶','品阶与成品词条','黄阶/1 个词条|玄阶/2 个词条|地阶/3 个词条|天阶/4 个词条','确认目标品阶');
  add('ForgeReceipt','forge','receipt','溪声','创制工坊 · 开炉回执','武器记录','成品名称/溪声 · 听泉杖|成品词条/疗伤效果 +1.20%|工艺词条/凝形|炼器经验/+10 点','收入纳戒');
  add('CraftCodex','seed','specimen','凝露草','百物图鉴 · 草木','来源与用途','产地/朝露郡 · 临仙郡|获取方式/采集药草|丹药用途/凝露丹 · 青脉丹|加工用途/寒露液 · 需 3 份','查阅相关配方');
  add('AffixPreview','forge','range','武器词条','炼器工坊 · 开炉前预览','词条预算','目标品阶/玄阶上品|可得词条/2 条|攻击力区间/+2.70% 至 +4.00%|揭示时点/成功开炉后','返回配料台');
  add('CreationNaming','forge','name','为此作题名','创制工坊 · 命名','题名信息','作品名称/溪声|器胚/听泉杖|创作者/陆青禾|记录/沿用此名称建立作品档案','确认题名');
  add('CreationDetail','forge','receipt','溪声','玩家作品 · 法器档案','首版记录','作者/陆青禾|器胚/听泉杖|品阶/黄阶下品|成品词条/疗伤效果 +1.20%','查看所用材料');

  add('DanFangXinXiWnd','dan','specimen','凝露丹','丹师塔 · 丹方详情','一炉所需','主药/凝露草 ×2|辅料/寒露液 ×1|品阶/黄阶上品|取材途径/朝露郡采集、加工','选择此丹方');
  add('DST_BangDanWnd','dan','rank','道境榜','丹师塔 · 筑基境','本期名次','裴知秋/第 1 名 · 第十二层|姜晚照/第 2 名 · 第十一层|方闻舟/第 3 名 · 第十层|我的排行/第 40 名 · 第九层','切换岁月榜');
  add('HuoDongWnd','celestial','confirm','超值月卡','活动中心 · 灵玉月卡','月卡领取规则','首次领取/450 灵玉|每日领取/50 灵玉|领取期限/30 日|累计可领/1,950 灵玉','查看购买确认');
  function parseRows(value) {
    return value.split('|').map(row => {
      const slash = row.indexOf('/');
      return [row.slice(0, slash), row.slice(slash + 1)];
    });
  }
  const landscape = {dan:'forge', forge:'forge', fu:'cave', array:'cave', seed:'mountain', sutra:'cave', spell:'cave', celestial:'sect', guild:'sect'};
  const emblems = {dan:'丹', forge:'器', fu:'符', array:'阵', seed:'植', sutra:'经', spell:'术', celestial:'虚', guild:'宗'};
  const guildModes = {拜帖:'confirm', 建宗:'name', 传讯:'chat', 名册:'roster', 抽签:'lot', 阵地:'compass', 山门:'gate', 藏品:'catalog', 升级:'grade', 营产:'production', 捐赠:'purchase', 秘境:'circuit', 加速:'time', 传令:'catalog', 比武:'rank'};

  // Art studies are rendered in editable type and CSS, using the common landscape plate for the hero.
  const starNames = ['景','坤','兑','乾','坎','艮','震','巽'];
  const pathPoints = [[50,13],[70,24],[82,47],[68,73],[50,85],[29,76],[17,50],[28,26]];
  function diagram(p, e) {
    if (['circuit','compass'].includes(p.mode)) {
      const names = p.art === 'sutra' ? ['承泣','四白','巨髎','地仓','颊车','下关','头维','人迎'] : p.art === 'spell' ? ['起','结','震','承','运','定','收','落'] : starNames;
      const nodes = names.map((name,index) => `<i style="left:${pathPoints[index][0]}%;top:${pathPoints[index][1]}%">${name}</i>`).join('');
      return `<div class="wk-geometric"><div class="wk-array"><b></b><b></b><b></b>${nodes}<strong>${emblems[p.art]}</strong></div><div class="wk-side"><small>所选阵位</small><h3>${e(p.hero)}</h3><p>${e(parseRows(p.rows)[0][1])}</p></div></div>`;
    }
    if (p.mode === 'talisman') return `<div class="wk-paperart"><div class="wk-charm"><small>太上敕令</small><span>水</span><b>灵</b><em>符</em></div><div><small>笔路</small><h3>${e(p.hero)}</h3><p>起笔　→　引灵　→　收锋</p></div></div>`;
    if (p.mode === 'fields') return `<div class="wk-fields">${['凝露草','霜节芦','赤绒苔','空田'].map((n,i)=>`<div><span class="wk-plant plant-${i}"></span><strong>${n}</strong><small>${['可采','生长','待浇','可种'][i]}</small></div>`).join('')}</div>`;
    if (['furnace','hammer','workshop'].includes(p.mode)) return `<div class="wk-meter"><span>${p.mode==='hammer'?'锻打':'炉火'}</span><div class="wk-meter-bars">${Array.from({length:28},(_,i)=>`<b class="${i<18?'on':''}" style="height:${10+Math.round(21*Math.sin(i/27*Math.PI))}px"></b>`).join('')}</div><strong>${p.mode==='hammer'?'64%':'文火'}</strong></div>`;
    if (p.mode === 'range') return `<div class="wk-range"><label>攻击力 <b>+2.70% 至 +4.00%</b></label><div></div><small>上品区间</small></div>`;
    if (p.mode === 'grade' && p.art==='forge' && p.hero==='玄阶上品') return `<div class="wk-tier">${['黄','玄','地','天'].map((v,i)=>`<b class="${i===1?'selected':''}">${v}<small>${i+1} 词条</small></b>`).join('')}</div>`;
    if (p.mode === 'tower') return `<div class="wk-tower">${['壹','贰','叁','肆','伍'].map((v,i)=>`<b style="height:${30+i*13}px">${v}</b>`).join('')}<span>登塔记录</span></div>`;
    if (p.mode === 'lot') return `<div class="wk-lots">${['灵玉','经脉丹','仙金','神术'].map((v,i)=>`<div style="transform:rotate(${(i-1.5)*5}deg)">${v}<i>福</i></div>`).join('')}</div>`;
    return '';
  }
  function tablets(rows, e, mode) {
    return `<div class="wk-tablets ${mode}">${rows.map(([label,value],i)=>`<div><i>${['壹','贰','叁','肆','伍','陆'][i]}</i><strong>${e(label)}</strong><small>${e(value)}</small></div>`).join('')}</div>`;
  }
  function ledger(rows,e,mode) {
    return `<div class="wk-ledger ${mode}">${rows.map(([label,value],i)=>`<div><span>${mode==='steps'?`<i>${['壹','贰','叁','肆'][i]}</i>`:''}${e(label)}</span><strong>${e(value)}</strong></div>`).join('')}</div>`;
  }
  function body(p,ui) {
    const e=ui.esc, pairs=parseRows(p.rows);
    const picture = diagram(p,e);
    const artIcon = {dan:'cauldron',forge:'sword',fu:'talisman',seed:'herb',array:'crystal',sutra:'book',spell:'scroll',guild:'pendant',celestial:'brush'}[p.art];
    const specimen = {ForgeWorkshop:'sword',ForgeReceipt:'sword',DiHuoGeLianDanJieSuanWnd:'pill',LT_ZhongZiWnd:'herb',HuoDongWnd:'crystal'}[p.id] || artIcon;
    const plate = typeof ui.relic === 'function' ? ui.relic(specimen,72) : emblems[p.art];
    const typeline = `<div class="wk-idline"><i>${plate}</i><div><small>${e(p.context)}</small><h2>${e(p.hero)}</h2></div></div>`;
    let detail;
    if (['catalog','pick','roster','production','specimen','materials','path','gate','hall','chat'].includes(p.mode)) detail=tablets(pairs,e,p.mode);
    else if (p.mode==='filter') detail=`<div class="wk-filters">${pairs.map(([label,value])=>`<div><small>${e(label)}</small>${value.split(/\s+| · /).filter(Boolean).map((name,i)=>`<b class="${i===1?'active':''}">${e(name)}</b>`).join('')}</div>`).join('')}</div>`;
    else if (p.mode==='name') detail=`<div class="wk-inscribe"><small>${e(pairs[0][0])}</small><strong>${e(pairs[0][1])}</strong><span>✎</span></div>${ledger(pairs.slice(1),e,p.mode)}`;
    else detail=ledger(pairs,e,p.mode);
    const secondary = {ForgeWorkshop:'领取入门材料　　｜　　估火候',ZongMenChouJiangEventWnd:'十连抽取',ZongMenShouChaoWnd:'前往宗门抽奖',ZongMenZhuFengWnd:'宗门大殿　　｜　　内务殿　　｜　　外务殿',WangJiMiLingWnd:'获取暗号'}[p.id];
    return `<div class="wk wk-${p.mode} wk-art-${p.art}">${typeline}${picture}<div class="wk-section"><b>${e(p.section)}</b><i></i></div>${detail}${secondary?`<div class="wk-other">${e(secondary)}</div>`:''}</div>`;
  }
  window.workshopDesign = function(d,ui) {
    if (!['Guild','Crafting'].includes(d.module) || d.kind==='component') return null;
    const id=d.id.replace(/\.prefab$/,'');
    let p=craft[id];
    if (!p && guildPages[id]) {
      const [type,hero,context,section,rows,primary]=guildPages[id];
      p={art:'guild',mode:guildModes[type],hero,context,section,rows,primary};
    }
    if (!p) p=archivePage(id);
    if (!p) return null;
    if(id==='HuaFuWnd'){
      const drawing='<div class="wk-draw-hud">'+ui.tabs(['引水纹','符墨','炼制记录'])+'</div>'+
        '<div class="wk-draw-stage"><div class="wk-draw-paper"><div class="wk-draw-ink">'+
        '<b>敕</b><i></i><em></em><span>水</span><i></i><strong>令</strong></div><small>寒露墨</small></div></div>'+
        '<div class="wk-draw-step">'+ui.stats([['已完成笔画','2 / 3'],['符纸余量','12 张']])+'</div>';
      return ui.page(d,{art:'cave',kicker:'洞府静室 · 润泽符',body:drawing,foot:'落笔行符',secondary:'清除笔画',cls:'wk-draw'});
    }
    const detail={...p,id};
    const options={art:landscape[p.art], kicker:p.context, body:body(detail,ui), foot:p.primary, cls:`wk-shell wk-shell-${p.mode}`};
    return ui.page(d,options);
  };
})();
