(() => {
  const data = {
    AH_MainSceneWnd:['list','拍卖会 · 练气场','购票入场','开场时间/今日 19:00/18:50 开放/scroll|练气场/拍品 12 件/5 灵玉/pendant|筑基场/拍品 8 件/10 灵玉/crystal|我的挂拍/公开拍卖委托/2 件/book','入场须知','拍品列表'],
    AH_PaiMaiHuWnd:['auction','正在竞拍 · 第 4 件','参与竞价','九品修元丹/天阶上品 · 练气可用/丹药/pill|陆闻舟/当前最高出价/90,000 灵石|竞拍结束/还可出价/00:01:34','现场'],
    AH_PaiPinListWnd:['list','拍品公示 · 共 12 件','查看拍品','九品修元丹/天阶上品/竞价中/pill|无上心经/天阶中品/待开拍/book|青竹剑/天阶下品/待开拍/sword|青玉护腕/练气可用/待开拍/armor','练气场'],
    AH_PaiPinXiangQingWnd:['form','委托书 · 公开拍卖','确认发布','拍品/九品修元丹/已选/pill|竞拍时间/今日 19:00|手续费/成交价的 10%|起拍价/60,000 灵石/修改'],
    AH_PaiPingXiangQingWnd:['modal','竞拍 · 现场出价','确认出价','九品修元丹/当前 90,000 灵石/丹药/pill|最低加价/10,000 灵石|我的出价/100,000 灵石/修改'],
    AH_WoDeGuaPaiWnd:['list','拍卖会 · 我的委托','查看委托','九品修元丹/练气场 · 今日 19:00/已发布/pill|九霄符书/练气场 · 明日 19:00/待甄选/talisman','待甄选 · 已挂拍 · 已结束'],
    AH_WoDePaiMaoWnd:['grid','送拍 · 珍品甄选','选定拍卖','无上心经/天阶功法/已选/book|九品修元丹/天阶丹方/可送拍/pill|青竹剑图/天阶器图/可送拍/scroll|九霄符书/天阶符书/可送拍/talisman','选择拍品'],
    CashGoodsWnd:['grid','仙缘 · 好礼','选择礼包','玄天月卡/30 天 · 每日奖励/450 灵玉/pendant|修行补给/补气散 · 禅悟丹/ 100 灵玉/pill|纳气小札/功法与灵石/60 灵玉/book','月卡 · 礼包'],
    ChouJiangEventWnd:['lot','仙缘 · 抽签','抽一次','道经残卷/保底珍品/天阶/book|转运符/活动奖池/稀有/talisman|经脉丹/活动奖池/常见/pill|灵石匣/活动奖池/常见/purse','已抽取 28 次','抽十次'],
    Common_ZhiFuWnd:['modal','交易 · 支付','确认支付','本次支付/灵石/1,200/coin|支付后余额/灵石/14,800','支付信息'],
    DuiHuanLingShiNewWnd:['form','钱庄 · 实时汇率','兑换灵石','当前汇率/1 灵玉可换/300 灵石|更新时间/今日 10:00/刷新|花费/10 灵玉/修改|可兑换/3,000 灵石','实时汇率'],
    DuiHuanLingShiWnd:['modal','钱庄 · 灵玉兑换','确认兑换','兑换数量/1,500 灵石/修改/coin|兑换消耗/5 灵玉|兑换后剩余/95 灵玉','兑换全部'],
    DuiHuanLingYuWnd:['grid','钱庄 · 灵玉','前往兑换','一匣灵玉/50 灵玉/兑换/crystal|两匣灵玉/100 灵玉/兑换/crystal|一袋灵玉/450 灵玉/兑换/purse','灵玉档位'],
    GuangGaoWnd:['detail','观影台 · 活动奖励','观看广告','灵玉/完成一次观看后领取/×20/crystal|今日已领取/活动奖励/0 次','观影奖励'],
    HuiGuiDuiHuanWnd:['form','回归记录 · 元明文举天','领取回归礼','原注册手机号/请输入原注册手机号|灵胎经/回归奖励/×1/book|五行道诀/回归奖励/×1/scroll|五行轻灵剑/回归奖励/×1/sword','核验原账号'],
    JiFenDuiHuanWnd:['list','仙缘 · 积分','领取奖励','好运符 ×1/已领取/200 分/talisman|好运符 ×2/可领取/500 分/talisman|天阶经脉丹/积 1,000 分解锁/1,000 分/pill|五阶寿元丹/积 5,000 分解锁/5,000 分/pill','可用 860 积分'],
    LevelRankWnd:['rank','赛季 · 第十季','膜拜魁首','陆闻舟/炼虚前期/28,730|谢清辞/化神后期/26,480|沈青禾/化神中期/25,680|温无咎/化神中期/25,050|周观澜/化神前期/22,540','境界榜 · 副职榜 · 宗门榜 · 天骄榜'],
    QZG_ChooseWnd:['modal','奇珍阁 · 交易簿','我的竞拍','我的拍卖/委托中 2 件/查看/book|我的竞拍/出价中 1 件/查看/pendant','','我的拍卖'],
    QZG_GoodsInformationWnd:['detail','珍品 · 可委托','委托拍卖','无上心经/天阶中品 · 功法/可交易/book|纳气速度/每周天/+8%|当前库存/可委托数量/1 册','委托物品'],
    QZG_ItmeInformation:['modal','奇珍阁 · 一口价','购买','无上心经/功法 · 天阶中品/100,000 灵石/book|交易方式/一口价|购买后/自动送入纳戒',''],
    QZG_PaiMaiWnd:['grid','奇珍阁 · 委托','选定拍卖','无上心经/天阶中品/已选/book|青竹剑/玄阶上品/可委托/sword|青脉丹/玄阶下品/可委托/pill|护山阵盘/玄阶中品/可委托/crystal','挑选纳戒物品'],
    QZG_PaiMaiWuPinXinXiWnd:['form','奇珍阁 · 委托设置','确认发布','拍品/无上心经/×1/book|交易方式/一口价 · 竞价/竞价|起拍单价/100,000 灵石/修改|拍卖时长/1 天|手续费/委托价的 10%','数量 · 价格'],
    QZG_ShaiXuan:['filter','奇珍阁 · 搜寻','','类别/功法 法术 丹方 法器 符箓 材料|品阶/一阶 二阶 三阶 四阶 五阶|五行/全部 金 木 水 火 土|拍卖类型/一口价 竞拍|价格区间/最低价 最高价','筛选条件','清空'],
    QZG_WoDeJingPaiWnd:['list','奇珍阁 · 竞价记录','查看出价','九品修元丹/我的出价 82,000 灵石/已被超价/pill|九霄符书/我的出价 60,000 灵石/出价领先/talisman','竞价中 · 已结束'],
    QZG_WoDePaiMaiWnd:['list','奇珍阁 · 委托记录','查看委托','无上心经/剩 14 小时/待成交/book|青竹剑/买家已付款/可领款/sword','在售 · 已成交 · 已流拍'],
    QiZhenGeWnd:['market','临仙郡 · 珍品交易','委托拍卖','无上心经/天阶中品 · 纳气功法/100,000 灵石/book|九品修元丹/玄阶上品 · 当场竞价/82,000 灵石/pill|青竹剑/玄阶上品 · 木行法器/24,800 灵石/sword|引灵符/黄阶上品 · 符箓/800 灵石/talisman','全部珍品 · 一口价 · 竞拍','我的拍卖'],
    QianDaoWnd:['calendar','每日 · 仙缘签到','签到领取','第一天/灵石 ×1,000/已领/coin|第二天/灵玉 ×25/已领/crystal|第三天/灵石 ×3,000/可领/coin|第四天/灵玉 ×50/待签/crystal|第五天/解毒丹 ×1/待签/pill|第六天/灵玉 ×75/待签/crystal|第七天/还精补气散 ×1/宝箱/purse','本轮签到 2／7'],
    ShangPuWnd:['market','临仙郡 · 百器坊','委托炼制','铁木剑胚/下品法器原胚/2,400 灵石/sword|凝露草/炼制常用药材/110 灵石/herb|赤炎炉/一阶地火通用/12,000 灵石/cauldron|护腕/基础护具/900 灵石/armor','一楼 · 二楼'],
    ShangPuWuPinXiangQingWnd:['detail','百器坊 · 商货','购买','青竹剑胚/法器原胚 · 可炼制/2,400 灵石/sword|可购数量/库存 4 件/×1|剩余灵石/购买前/15,600','物品详情'],
    XuanTianLingWnd:['pass','玄天楼 · 通行令','延期 30 天','玄天令/持令剩余 60 天/生效中/pendant|离线收益时长/6 小时 → 12 小时|扫荡收益/50% → 75%|每日俸禄/还精补气散、悟性丹、灵玉','通行令权益'],
    XuanTianLouWnd:['market','玄天坊 · 灵宝','查看商品','玄天令/扫荡与离线权益/450 灵玉/pendant|青玉纳戒/扩展纳戒容量/160 灵玉/ring|悟性丹/恢复 10 点悟性/25 灵玉/pill|经脉古卷/通用养成/3,600 灵石/book','灵玉 · 灵石'],
    XuanTianLouXiangQingWnd:['detail','玄天楼 · 灵宝详情','购买','悟性丹/服用后恢复 10 点悟性/25 灵玉/pill|购买数量/×1/修改|剩余灵玉/购买后/215','选购数量'],
    ChuWuXiangWnd:['storage','洞府 · 收纳','一键取出','青木丹方/×1/丹方/book|寒潭玉/×12/矿物/crystal|凝露草/×36/药材/herb|青纹护符/×2/符箓/talisman|紫铜炉/×1/法器/cauldron|洗炼砂/×7/材料/purse','储物空间 36／120','打开纳戒'],
    FaQiXiLianWnd:['detail','法器 · 特效重铸','普通锻造','青竹剑/玄阶上品 · 木行/已装备/sword|固灵/施法消耗 −4%/已锁定|穿云/法术攻击 +18/可更换|青木守/木抗 +5%/可更换|洗炼消耗/灵石 1,200 · 陨铁 2','锻造预览','高级锻造'],
    HeChengWnd:['recipe','合成 · 材料转化','开始合成','寒潭玉/投入/×3/crystal|清泉露/投入/×2/pill|凝霜玉/获得/×1/pendant|合成数量/当前材料最多合成 4 件/1','配方','设为最大'],
    NaJieOneKeySoleWnd:['filter','纳戒 · 批量处理','确认出售','类别/全部 法器 材料 书籍 阵盘 符箓|品阶/黄阶 玄阶 地阶|五行/其他 金 木 水 火 土|等阶/练气 筑基 结丹 元婴|预计售价/共 8 件，获得 240 灵石','出售条件','取消'],
    NaJieSpaceWnd:['storage','沈青禾 · 随身收纳','升级纳戒','青竹剑/×1/玄阶/sword|纳气诀/×1/黄阶/book|回元散/×12/药品/pill|凝露草/×24/灵植/herb|寒潭玉/×7/矿物/crystal|引灵符/×3/符箓/talisman|信物/×1/任务/pendant|陨铁/×3/材料/purse|紫铜炉/×1/法器/cauldron','储物空间 110／120','一键出售'],
    NaJieTipWnd:['detail','纳戒 · 物品详情','使用','回元散/服用后立即回复部分真元/×12/pill|来源/坊市、炼制|品阶/黄阶上品|售价/60 灵石','药效','锁定'],
    RoleZhuangBeiTipWnd:['modal','装备 · 替换确认','穿戴','青竹剑/新装备 · 玄阶上品/攻击 +28/sword|青木剑/当前 · 黄阶上品/攻击 +16/sword|穿戴变化/攻击 +12，木抗 +4%','装备对比'],
    RoleZhuangBeiWnd:['loadout','沈青禾 · 炼气八阶','配置技能','青竹剑/主法器/已装备/sword|青玉护甲/护体/已装备/armor|引灵符/符箓/×3/talisman|聚灵阵盘/阵盘/已装备/crystal|木缚术/法术一/小成/scroll|轻身诀/法术二/入门/book','总览 · 法术 · 法器 · 符箓 · 阵盘'],
    ZhanFaShiWnd:['list','洞府 · 法术熟练','设定修行','木缚术/小成 · 熟练 480／600/修行中/scroll|轻身诀/入门 · 熟练 70／100/可修行/book|御物诀/入门 · 熟练 0／100/可修行/pendant','修行法术'],
    ZhanFaShiXiangQingWnd:['form','战法室 · 修行计划','确认修行','木缚术/熟练度 480／600/小成/scroll|施法时间/缩短 10%|修行次数/5 次/调整|预计消耗/15 点精力|预计收益/+75 熟练度','自动修行'],
    ChuanXinWnd:['mail','信匣 · 未读 2 封','一键领取','坊市寄存通知/奇珍阁 · 今日 09:12/有附件/letter|宗门传书/云岫宗 · 昨日 19:45/未读/scroll|离线修行收益/洞府管事 · 昨日 06:00/已领取/purse','全部 · 未领取','一键删除'],
    DaoYouChatWnd:['chat','道友 · 与谢清辞的传音','发送','谢清辞/今日借道白石岭，可要同去？/18:21|沈青禾/我先去城中补几张引灵符，酉时见。/18:24|谢清辞/好，我在西门等你。/18:25','谢清辞 · 在线'],
    DaoYouChuanWuWnd:['grid','道友 · 传递物品','支付并传物','凝露草/×4/已选/herb|回元散/×2/未选/pill|寒潭玉/×3/未选/crystal','委托费 10 灵石'],
    DaoYouWnd:['friends','道友 · 名录','结识道友','谢清辞/炼气八阶 · 紫霞城/在线/pendant|陆闻舟/筑基中期 · 青竹岭/在线/book|周观澜/筑基初期 · 归云渡/离线 2 小时/scroll','道友 · 名刺 · 师徒 · 道侣'],
    LingQuanJiuSiWnd:['chat','坊市 · 品酒闲聊','加入闲叙','陆闻舟/东市有人收凝露草。/19:01|谢清辞/今晚宗门演武，有空来观战。/19:02|店家/二楼新设一席，可上楼入座。/19:05','一楼 · 酒肆传音','上楼'],
    ShiTuToZiWnd:['list','师门 · 日常互动','请安','请安/亲密度 +10/可执行/letter|传功/弟子修为 +1,000 · 自身精力 −5/可执行/crystal|出师/弟子可另立门户/需确认/scroll|逐出师门/结束师徒关系/需确认/pendant','对谢清辞行礼'],
    ShiTuWnd:['friends','师门 · 谱牒','发出拜师帖','温无咎/师尊 · 筑基中期/亲密度 1,280/pendant|方闻舟/记名弟子 · 炼气初阶/待请安/scroll|礼帖/一份拜师帖待查看/新/letter','师尊 · 弟子 · 拜师帖'],
    CaiLiaoXuanZeShaiXuanWnd:['filter','材料 · 属性筛选','应用筛选','类型/植物 自创植物 妖兽|五行/金 木 水 火 土|四气/寒 凉 温 热|五味/酸 苦 甘 辛 咸|补泻/补 泻|产地/恒元大陆 青云洲 临仙郡','材料属性','清空'],
    CeHuaWnd:['profile','道籍 · 个人信息','返回修行','沈青禾/炼气八阶 · 散修/金木双灵根/pendant|寿元/12／200 年|精血/2,400|体魄/980|真元/1,000|神识/640|灵石/16,000|灵玉/245','角色属性','设置'],
    Common_ChooseGoodsNumWnd:['quantity','物品 · 数量','确认数量','回元散/当前库存/12 瓶/pill|本次数量/−　　3　　+/最大'],
    Common_QiRenWnd:['modal','操作 · 确认','确认开垦','开垦第四块灵田/预计耗时 1 小时/精力 −10/herb|开始后/在灵田查看进度'],
    Common_ShaiXuanWnd:['filter','选项 · 搜寻','应用筛选','类别/全部 法器 材料 丹药 功法|等阶/练气 筑基 结丹 元婴|品阶/黄 玄 地 天|五行/金 木 水 火 土','筛选条件','重置'],
    Common_TipWnd:['detail','道具 · 用法','提升品阶','青玉引灵符/木行 · 玄阶中品/×1/talisman|用途/聚拢周身灵气|提升消耗/引灵砂 ×1','符箓说明','遗忘'],
    Common_WebWnd:['web','山门 · 公告','返回','公示/服务器维护完成|维护时间/今日 05:00—06:00|补给/补给已由传信发放，请在信匣中领取。'],
    KeyBoardWnd:['keyboard','输入 · 数值','确认','当前输入/240/0—999','输入数量'],
    MaskWnd:['mask','界面 · 遮罩','',''],
    NetMessageWnd:['loading','连接 · 登录','','正在登录/正在验证升仙令|稍候/正在建立连接'],
    OptionWnd:['settings','账号与游戏','保存设置','背景音乐/开|环境音效/开|屏幕常亮/关|礼品兑换/输入兑换码|升仙号牌/管理账号|回归礼包/填写原手机号','偏好设置','登出'],
    PopUpWnd:['modal','山门 · 传告','知道了','今日例行维护/服务器已恢复开放/公告/scroll|维护补给/在传信中领取'],
    XiaoWenHaoWnd:['manual','洞府 · 玩法说明','知道了','控火/按住可升高炉温，松手可降温。/炼丹/cauldron|投入材料/按箭头提示滑动，投入对应药材。/炼丹/herb|五行遁术/灵根与地形相合时可提速，会持续消耗灵气。/历练/scroll|血遁/精血不足时，可消耗 2 点精血撤回附近城池。/战斗/sword','炼丹 · 遁术 · 法术'],
    JingShiWnd:['modal','山门 · 警示','返回','进入前确认/当前精血较低，请先恢复。/需要处理/pill|当前精血/240／2,400']
  };

  function parseRows(text) {
    return text ? text.split('|').map(item=>item.split('/')) : [];
  }
  function shelf(items,ui,small=false) {
    return '<section class="m-shelf '+(small?'m-small':'')+'">'+items.map(([name,description,state,pic],index)=>
      '<div class="'+(index===0?'focused':'')+'">'+ui.relic(pic,small?60:72)+'<b>'+ui.esc(name)+'</b><small>'+ui.esc(description)+'</small><em>'+ui.esc(state||'')+'</em></div>').join('')+'</section>';
  }
  function specimen(row,ui) {
    const [title,desc,stat,pic]=row;
    return '<div class="m-specimen">'+ui.relic(pic,112)+'<div><span>'+ui.esc(desc)+'</span><h2>'+ui.esc(title)+'</h2><b>'+ui.esc(stat||'')+'</b></div></div>';
  }
  function insets(p,items,d,ui) {
    const [mode,,, ,section]=p;
    const e=ui.esc;
    const tabs=section?.includes(' · ')?ui.tabs(section.split(' · ')):'';
    if (mode==='filter') return '<div class="m-filters">'+items.map(([label,values],i)=>'<div><strong>'+e(label)+'</strong><section>'+values.split(' ').map((x,j)=>'<span class="'+(i<4&&j===0?'m-on':'')+'">'+e(x)+'</span>').join('')+'</section></div>').join('')+'</div>';
    if (mode==='chat') return '<div class="m-conversation"><div class="m-chatday">今日</div>'+items.map(([who,line,when],i)=>'<div class="'+(i===1?'own':'')+'"><small>'+e(who)+'　'+e(when)+'</small><p>'+e(line)+'</p></div>').join('')+'</div><div class="m-search">写下要传的消息<span>送出 ›</span></div>';
    if (mode==='keyboard') return ui.stats([['当前输入',items[0][1]],['范围',items[0][2]]])+'<section class="m-keyboard">'+['1','2','3','4','5','6','7','8','9','清除','0','⌫'].map(k=>'<b>'+k+'</b>').join('')+'</section>';
    if (mode==='storage') return '<div class="m-search">⌕　搜索物品<span>筛选</span></div><div class="m-capacity"><label>'+e(section)+'</label><i></i></div>'+ui.tabs(['全部','装备','丹药','材料','秘卷'])+shelf(items,ui,true);
    if (mode==='loadout') return tabs+'<div class="m-outfit"><img src="assets/journey/woman.png" alt=""><aside><small>已穿戴</small><h2>青木行装</h2><p>战斗力 2,608</p></aside></div>'+shelf(items,ui,true);
    if (mode==='market') return '<div class="m-search">⌕　搜索拍品与物资<span>筛选</span></div>'+tabs+shelf(items,ui);
    if (mode==='calendar') return '<div class="m-calendar">'+items.map(([title,detail,state,pic],i)=>'<div class="'+(i===2?'today':'')+'"><small>'+e(title)+'</small>'+ui.relic(pic,52)+'<b>'+e(detail)+'</b><em>'+e(state)+'</em></div>').join('')+'</div>';
    if (mode==='detail'||mode==='auction'||mode==='pass'||mode==='profile') return specimen(items[0],ui)+ui.group(section||'详情',ui.rows(items.slice(1)));
    if (mode==='grid'||mode==='lot') return (tabs||'<div class="m-sectionlabel">'+e(section||'可选物品')+'</div>')+shelf(items,ui)+(mode==='lot'?ui.stats([['单抽','20 灵玉'],['十连','200 灵玉']]):'');
    if (mode==='rank') return tabs+'<div class="m-ranking">'+items.map(([person,realm,heat],i)=>'<div><i>'+String(i+1).padStart(2,'0')+'</i><b>'+e(person)+'</b><small>'+e(realm)+'</small><em>'+e(heat)+'</em></div>').join('')+'</div>';
    if (mode==='settings') return '<div class="m-settings">'+items.map(([name,value],i)=>'<div><b>'+e(name)+'</b>'+(i<3?'<span class="m-toggle '+(value==='开'?'on':'')+'">'+value+'</span>':'<small>'+e(value)+'　›</small>')+'</div>').join('')+'</div>';
    if (mode==='friends') return '<div class="m-search">⌕　输入道友姓名<span>查找</span></div>'+tabs+ui.rows(items);
    if (mode==='recipe') return '<div class="m-recipe">'+items.slice(0,3).map(([name,,state,pic])=>'<div>'+ui.relic(pic,66)+'<b>'+e(name)+'</b><small>'+e(state)+'</small></div>').join('')+'</div>'+ui.rows(items.slice(3));
    if (mode==='form') return '<div class="m-form">'+ui.rows(items)+'</div>';
    return (tabs||'')+ui.rows(items);
  }
  window.marketDesign = (d,ui) => {
    const p=data[d.id];
    if(!p||d.kind==='component') return null;
    const [mode,kicker,foot,input,section,secondary]=p;
    const items=parseRows(input);
    const art={Commerce:'market',Social:'sect',Inventory:'cave',Common:'mountain'}[d.module];
    if (mode==='mask') return '<article class="m-mask"><span class="m-mask-context">修真界</span></article>';
    if (['modal','quantity','keyboard','loading'].includes(mode)) {
      const body=mode==='keyboard'?insets(p,items,d,ui):mode==='loading'?'<div class="m-network"><i></i><h2>正在登录</h2><p>正在验证升仙令</p></div>':ui.rows(items);
      return ui.modal(d,{art,kicker,body,foot,secondary:secondary||(mode==='loading'?'':'取消')});
    }
    return ui.page(d,{art,kicker,body:insets(p,items,d,ui),foot:foot||(mode==='filter'?'确定':''),secondary,cls:'m-page m-page--'+mode,compact:['profile','settings','filter','web'].includes(mode)});
  };
})();
