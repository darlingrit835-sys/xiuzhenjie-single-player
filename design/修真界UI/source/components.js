(() => {
  // The examples preserve the component's role in its parent screen.
  // Values and character names belong to a sample save.
  const specs = {};
  function set(mode,table) {
    for (const [key,value] of Object.entries(table)) specs[key] = [mode,...value.split('|')];
  }
  set('portrait',{
    ChengHaoEffectItem:'云阶初渡|登临筑基后获得|解锁|佩戴中|pendant',
    MingCiItem:'谢清辞|炼气八阶 · 散修|陌生人|已发名刺|letter',
    DaoYouItem:'谢清辞|炼气八阶 · 紫霞城|在线|离线|pendant',
    ShiTuItem:'方闻舟|记名弟子 · 炼气初阶|请安|已请安|pendant',
    ShoTuItem:'方闻舟|炼气初阶 · 无师徒关系|待回应|已收徒|letter',
    ZongMenChengYuanItem:'沈青禾|内门弟子 · 筑基前期|今日回宗|三日未归|pendant',
    ZongMenMingCiItem:'谢清辞|道龄 100 年 · 筑基前期|待审批|已入宗|letter',
    ZongMen_ZhangLaoDianItem:'裴知秋|护宗长老 · 筑基后期|战力 3,440|外出|pendant',
    ZongMen_ZhangLaoItem:'裴知秋|筑基后期 · 在宗 260 日|可任命|已任命|scroll',
    CeHuaItem:'炼丹师|宗门副职|学徒|二阶丹师|cauldron'
  });
  set('row',{
    ChooseServerItem:'紫霄天|青云洲 · 角色 1 名|可进入|维护中|pendant',
    AH_Item:'九品修元丹|天阶 · 火行|第 4 件|已落槌|pill',
    QZG_WoDeJingPaiItem:'九霄符书|当前 80,000 · 我的 78,000|出价落后|出价领先|talisman',
    QZG_WoDePaiMaiItem:'青竹剑|灵石 32,000 · 剩余 1 小时|在售|待领款|sword',
    QiZhenGeItem:'无上心经|天阶中品 · 一口价|100,000 灵石|已下架|book',
    ShangPuItem:'青竹剑胚|法器 · 余 4 件|2,400 灵石|售罄|sword',
    XuanTianLouItem:'玄天令|扫荡与离线修行权益|450 灵玉|已持有|pendant',
    PayContentItem:'闲叙竹筹|可在酒肆发送 20 句|100 灵石|余额不足|purse',
    ChuanXinItem:'坊市寄存通知|系统 · 今日 09:12|新信|已领取|letter',
    DanFangItem:'春水回元丹方|作者 沈青禾|1,000 布道贡献|已注册|book',
    DanFang_DaiShenHe:'回元丹方|三阶 · 炼制成功 2 炉|审核中|审核通过|book',
    DanFang_KeZhuCe:'青脉丹方|二阶 · 可登记|未注册|已提交|book',
    DanFang_YiZhuCe:'青脉丹方|累计兑换 18 次|1,000 贡献|已下架|book',
    DiHuoCaiLiaoJiaGeItem:'虎妖精血|药材 · 余 9 份|10,000 灵石|售罄|crystal',
    DiHuoGe_MapItem:'青云洲 · 临仙郡|寒潭矿床|可追踪|已追踪|scroll',
    Common_XuShenJieItem:'纳气引灵诀|作品 · 沈青禾|可注册|已注册|book',
    Common_XuShenJieSItem:'清霄纳气诀|已发布 · 版本 2|500 布道贡献|已兑换|book',
    Common_ZhuCeItem:'春水回元丹|玩家原创 · 丹方|草稿|发布成功|book',
    DJSD_XuShenJieItem:'清霄纳气诀|已兑换 21 卷 · 完整度 82%|可查阅|已收录|book',
    WorldCaiLiaoItem:'寒潭玉|临仙郡 · 涧底矿脉|可采|已收录|crystal',
    XuShenJieItem:'旬阳残卷|完整度 62%|尝试补全|已补全|scroll',
    GengHuanGongFaItem:'纳气诀|木灵根 · 黄阶上品|可装配|已装配|book',
    YiCanWuItem:'御物诀|叠加悟性 15 点|已参悟|可查看|book',
    KeCanWuItem:'火球术|所需悟性 60 点|可参悟|悟性不足|book',
    DongFuItem:'人阶洞府|灵脉支峰 · 灵气浓度 130%|空闲|已入驻|ring',
    CSZ_JunItem:'临仙郡|青云洲 · 一阶阵台|可传送|未解锁|crystal',
    MapZongMenItem:'云岫宗|青溪山 · 三阶宗门|可拜山|已入宗|pendant'
  });
  set('stat',{
    QZG_InfoItem:'五行|木属性|木|火',
    ShangPuXiangQingItem:'激发消耗|每次施放|10 点真气|12 点真气',
    VipPowerEffectItem:'玄天令|补气散购买次数|+10 次|剩余 60 天',
    DST_DanFangDuiHuanTipItem:'丹方等阶|门类限制|二阶|三阶',
    DstZhuCeItem:'丹方等阶|注册档案|二阶|尚未填写',
    CZGF_Tip_SuccessItem:'悟性消耗|自创功法|50 点|剩余 30 点',
    CanWuXiangQingItem:'激发消耗|参悟法术|10 点真气|12 点真气',
    DongFuBuffItem:'聚灵增益|洞府运转中|灵气 +20%|已失效',
    BattleSettingDetailTextItem:'法术品阶|战法室|一阶|二阶',
    NaJieTipItem:'物品属性|五行伤害|+18|+22',
    RoleZhuangBeiShuXingContentItem:'真元|装备属性类别|1,000|1,120',
    RoleZhuangBeiXiangQingItem:'真元|装备替换对比|1,120|+120',
    DanShiItem:'炼制考核|二阶修元丹 · 共两炉|要求 80%|通过',
    DJSD_GongFaDuiHuanItem:'沈青禾|清霄纳气诀|500 贡献|已转入',
    DST_DanFangDuiHuanItem:'谢清辞|青脉丹方|80 贡献|已转入',
    SFH_FuShuDuiHuanItem:'方闻舟|青木引灵符书|240 贡献|已转入',
    LZ_DuiHuanItem:'沈青禾|凝露草样本|120 贡献|已转入',
    WFG_FaShuDuiHuanItem:'陆闻舟|木缚术|300 贡献|已转入'
  });
  set('tile',{
    GuiWuDiaoLuoItem:'天罗羽衣|战利品|未拾取|已入纳戒|armor',
    Cash_LingYuItem:'灵玉小匣|50 灵玉|可兑换|已兑换|crystal',
    DuiHuanXianYuItem:'仙玉小匣|60 仙玉|待购|已获得|crystal',
    HuoDongItem:'玄天月卡|生效 30 天|可购|生效中|pendant',
    ChouJiangEventItem:'道经残卷|天阶下品|奖池样式|已抽中|book',
    LiPinItem:'灵石补给|灵石 ×1,000|可领取|已领取|coin',
    QiZhenGePaiMaiItem:'火球术|火行 · 可委托|未选|已选|scroll',
    ChangShengLuItem:'炼气一层|灵石 ×100|可领取|已领取|coin',
    ChuanXinGoodsItem:'回元散|邮件附件 ×2|待领取|已领取|pill',
    BoxItem:'天罗羽衣|储物 · 数量 1|锁定|已取出|armor',
    ChuWuXiangItem:'青木丹方|洞府宝箱 · 数量 1|可取出|已取出|book',
    ChuanDaiZhuangBeiItem:'轻灵土甲|土行 · 黄阶上品|可穿戴|已穿戴|armor',
    RoleZhuangBeiItem:'青竹剑|主法器槽|空闲|已装备|sword',
    UI_GoodsItem:'凝露草|灵植 · 数量 24|完整|锁定|herb',
    UI_GoodsTogItem:'寒潭玉|矿物 · 数量 7|未选|已选|crystal',
    AllFaShuItem:'木缚术|木行 · 玄阶|可选|已选|scroll',
    JiNengItem:'惊雷圣闪|玄阶 · 23 灵气|就绪|冷却 3 秒|talisman',
    DouMenSheZhiFaShuItem:'青木引灵|战法槽位 1|已配置|停用|scroll',
    DouMenXuanXeFaShuItem:'火球术|火行 · 玄阶|可配置|已配置|scroll',
    DanFang_SuoXuYaoCaiItem:'凝露草|配方需 4 株|库存 12|材料不足|herb',
    DiHuoGeCaiLiaoChooseItem:'赤炎砂|预估转换率 75%|未选|已选|purse',
    DiHuoGeCaiLiaoItem:'虎妖精血|世界掉落 · 数量 9|可投入|已投入|crystal',
    DiHuoGeChooseBigItem:'地表之火|10,000 灵石／次|可启用|当前炉火|cauldron',
    DiHuoGeLianZhiMuBiaoItem:'修元丹|目标产物 · 品阶二|可选|已选|pill',
    LianZhiCaiLiaoXuanZe:'一阶虎妖精血|炼制材料 · 需求 2|可替换|已投料|crystal',
    ShouChao_MonsterItem:'青岚螳|外峰 · 第三波|防护 60%|已击退|crystal',
    TSB_Item:'天书碑|完成临仙郡初访|可领取|已领取|scroll'
  });
  set('chip',{
    ChooseChengHaoItem:'云阶初渡|称号 · 筑基|未佩戴|已佩戴',
    Common_ShaiXuanBigItem:'物品类别|法器 · 功法 · 材料|全部|法器',
    Common_TogItem:'丹方|通用选择|未选|已选',
    ShaiXuanShuXingItem:'金属性|五行筛选|未选|已选',
    ShaiXuanWeiZhiItem:'恒元大陆|材料产地|未选|已选',
    XuanTianLouScrollItem:'功法|玄天楼分类|可切换|已选',
    CanWuScrollItem:'功法|参悟分类|可切换|已选',
    BoxTitleTogItem:'丹方|储物分类|可切换|已选',
    DiHuoGeChooseSmallItem:'青脉丹方|可炼制 · 二阶|未选|已选',
    HuaFuCeHuaItem:'金篆符箓|符箓分类|已折叠|已展开',
    HuaFuCeHuaItemChild:'护体符|金篆符箓|未选|已选'
  });
  set('message',{
    ChatItem:'裴知秋|晚间演武在西峰。|世界频道|宗门频道',
    DanMuItem:'玉虚仙人|二十级副本组队。|滚入屏幕|移出屏幕',
    OtherChatItem:'谢清辞|西门见，我带了引灵符。|已收到|带时间戳',
    SelfChatItem:'沈青禾|我已到紫霞城，稍候。|已发送|发送失败',
    ScrollingTextItem:'宗门传告|外峰阵石已补齐。|弹出|收起'
  });
  set('progress',{
    PaiHangBangItemPanel:'陆闻舟|境界排行 · 元婴九阶|第 1 位|第 2 位',
    XuShenTaBangDanItem:'沈青禾|虚神塔 · 登塔记录|100 层|刷新纪录',
    ZhanFaShiItem:'木缚术|熟练度 480／600 · 小成|修行中|已完成',
    ChenZhuFuRenWuItem:'缉拿通缉犯|临仙郡 · 目标 2 名|完成 1／2|待领奖',
    LT_ZhoZhiItem:'凝露草|东垄 · 水分 64%|生长中|可采摘',
    LianZhiBuZhouSheZhiItem:'点火|温度升至 380°|可配置|已确认',
    LianZhiBuZhouShunXuItem:'投掷草药|点火后的第二步|待执行|已完成',
    StepBgItem:'二、投料|第一轮火候稳定后|待执行|当前步骤',
    TouZhiLeftCaiLiao:'向左投料|凝露草 ×1|待投入|已投入',
    TouZhiRightCaiLiao:'向右投料|赤炎砂 ×1|待投入|已投入'
  });
  set('effect',{
    LoadingCrane:'飞鹤|加载动效静帧|起翼|展翅|crane',
    JingMaiItem:'胞中|经脉穴位|未通|已贯通|point',
    PointImg:'符头|描线起点|待描|落笔|point',
    PointImg_0:'符尾|描线收束|虚位|已成|point',
    DeBuffIconItem:'缚足|移动受限|生效|剩余 2 秒|debuff',
    PeopleUnderAttackAni:'受击反馈|角色闪色|命中|恢复|hit',
    SkillIconAction:'法术充能|战斗技能热区|聚气|释放|hit',
    UnderAttackEffectItem:'受击特效|冲击环|起势|衰减|hit',
    ChuangZaoFaShuItem:'引雷手印|创法动作关键帧|结印|引灵|point',
    ChuangZhaoGongFaItem:'承泣|自创功法线路节点|未贯通|贯通|point',
    ChuangZhaoGongFaItemBall:'周天灵流|功法运行光点|行气|回穴|point',
    HuaFuLineItem:'引灵笔画|符纹线段|参考线|描画完成|stroke',
    LineAni:'引灵线路|路径连通视觉|连线中|线路贯通|stroke',
    TuPo_ShanDian:'破境雷劫|受击范围内雷光|预警|落雷|thunder',
    HuoFaShuItem:'火球术|火灵根 · 法术|就绪|选中|huo',
    JinFaShuItem:'锐金诀|金灵根 · 法术|就绪|选中|jin',
    MuFaShuItem:'木缚术|木灵根 · 法术|就绪|选中|mu',
    ShuiFaShuItem:'水幕诀|水灵根 · 法术|就绪|选中|shui',
    TuFaShuItem:'土遁术|土灵根 · 法术|就绪|选中|tu'
  });

  function effect(pic, index) {
    if (pic==='crane'||pic==='thunder') return '<img src="assets/parts/'+pic+(index+1)+'.png" alt="">';
    if (['jin','mu','shui','huo','tu','debuff'].includes(pic)) return '<img src="assets/parts/'+pic+'.png" alt="">';
    return '<i class="c-effect c-'+pic+'" aria-hidden="true"></i>';
  }

  function sample(mode,s,index,ui) {
    const [name,detail,stateA,stateB,pic]=s;
    const e=ui.esc;
    const state=index?stateB:stateA;
    const art=()=>ui.relic(pic,mode==='tile'?78:57);
    if(mode==='effect') return '<div class="c-fx">'+effect(pic,index)+'</div><strong>'+e(name)+'</strong><small>'+e(detail)+'</small>';
    if(mode==='tile') return '<div class="c-tile">'+art()+'<div><strong>'+e(name)+'</strong><small>'+e(detail)+'</small></div></div>';
    if(mode==='chip') return '<div class="c-chip">'+(index?'✓　':'◇　')+e(name)+'</div><small>'+e(detail)+'</small>';
    if(mode==='stat') return '<div class="c-stat"><span><strong>'+e(name)+'</strong><small>'+e(detail)+'</small></span><em>'+e(state)+'</em></div>';
    if(mode==='message') return '<div class="c-message"><small>'+e(name)+'　21:06</small><p>'+e(detail)+'</p></div>';
    if(mode==='progress') return '<div class="c-progress"><strong>'+e(name)+'</strong><div><small>'+e(detail)+'</small><span>'+e(state)+'</span></div><i></i></div>';
    return '<div class="c-row">'+(mode==='portrait'&&/青禾|清辞|闻舟|方闻舟|知秋/.test(name)?'<img class="c-avatar" src="assets/journey/'+(name==='沈青禾'?'woman':'man')+'.png" alt="">':art())+
      '<div><strong>'+e(name)+'</strong><small>'+e(detail)+'</small></div><em>'+e(state)+'</em></div>';
  }

  window.componentDesign=(d,ui)=>{
    if(d.kind!=='component') return null;
    const spec=specs[d.id];
    if(!spec) throw Error('组件缺少设计 '+d.id);
    const [mode,...s]=spec;
    if(d.id==='JingMaiItem'&&d.path.startsWith('Resources/')) {s[1]='内视 · 奇经节点';s[2]='当前选中';s[3]='气脉已通';}
    if(d.module==='Cultivation') return window.cultivationComponent(d,s,mode);
    const layouts={effect:'c-tiles',tile:'c-tiles',chip:'c-tiles',portrait:'c-lines',row:'c-lines',progress:'c-lines',message:'c-lines',stat:'c-lines'};
    const body='<section class="c-op c-mode-'+mode+'"><div class="c-intro"><h2>'+ui.esc(s[0])+'</h2><small>'+ui.esc(s[1])+'</small></div>'+
      '<div class="c-previews '+layouts[mode]+'">'+[0,1].map(i=>'<article class="'+(i?'alt':'')+'"><label>'+ui.esc(s[2+i])+'</label>'+
        '<div class="c-example">'+sample(mode,s,i,ui)+'</div></article>').join('')+'</div></section>';
    return ui.page(d,{art:{Account:'mountain',Map:'mountain',Battle:'battle',Guild:'sect',Crafting:'forge',Cultivation:'cave',Inventory:'cave',Commerce:'market',Social:'sect'}[d.module]||'mountain',
      kicker:'组件 · '+ui.modules[d.module],body,cls:'component-board'});
  };
})();
