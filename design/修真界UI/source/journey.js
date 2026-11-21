// The original layouts supply controls, not the sample character or the illustrated state.
// Exact source groups: GameStart.RegisterUI, Assets/gamedata/prefabs/uiprefabs/window,
// Resources/prefab/window, and the matching Account, Map, Battle, MainQuest and Cultivation controllers.
// Place names follow server-go/game_map.go. Field-guide examples follow docs/region-ecology.md and docs/bestiary.md.
(() => {
  const sheets = {};
  function sheet(id, mode, heading, overline, subject, detail, rows, action, options = {}) {
    sheets[id] = {mode, heading, overline, subject, detail, rows, action, ...options};
  }

  // Account. Email is the trial registration option in SignUpWnd.cs.
  sheet('SignInWnd','login','山门','升仙令','修真界','', [
    ['升仙号牌','请输入账号'],['升仙密令','请输入密令']
  ], '登录', {secondary:'获取升仙令', link:'忘记密令'});
  sheet('SignUpWnd','form','领取升仙令','升仙司 · 身份登记','初入仙门','完成验证后即可持令入界。', [
    ['邮箱','输入常用邮箱'],['暗号','六位验证码'],['升仙密令','设置密令'],['确认密令','再次输入密令']
  ], '进入修真界',{link:'获取暗号'});
  sheet('ForgetPasWnd','form','找回密令','升仙司 · 身份验证','重置密令','验证升仙号牌，再为此账号设定新密令。', [
    ['升仙号牌','请输入号牌'],['验证码','输入收到的暗号'],['新密令','输入新密令'],['确认密令','再次输入新密令']
  ], '提交新密令',{link:'获取暗号'});
  sheet('ChongShengWnd','form','重生确认','升仙司 · 转世','重入轮回','此举将结束当前角色的修行。请核对身份并输入确认词。', [
    ['升仙号牌','请输入号牌'],['暗号','输入验证码'],['确认词','输入「确认重生」']
  ], '确认重生',{link:'获取暗号', footnote:'□　我已了解并自愿承担损失'});
  sheet('ChooseServerWnd','servers','择界','择一方天地','青云洲','上次登录的服务器',[
    ['东荒 · 青云','上次登录'],['南岭 · 紫霞','可进入'],['西漠 · 黄沙','可进入'],['北冥 · 玄冰','可进入']
  ], '进入所选服务器');
  sheet('CreateRoleWnd','role','塑造仙身','入世 · 生辰录','初定仙骨','形貌、灵根与出生地',[
    ['道号','沈青禾'],['性别','女修　　男修'],['出生地','东荒 · 临仙郡']
  ], '下一步',{tabs:['形貌','灵根','出生地'],image:'woman'});
  sheet('ChooseChengHaoWnd','ledger','更换称号','道号 · 名录','云游散人','当前佩戴称号',[
    ['云游散人','已选择'],['初窥仙门','可佩戴'],['临仙客卿','可佩戴'],['同类效果','取最高值']
  ], '佩戴称号',{image:'seal'});
  sheet('LiXianShouYiWnd','receipt','离线收益','闭关 · 修行录','六时归元','离开期间的修习所得',[
    ['修炼时间','06 时 12 分'],['真元','＋246'],['体魄','＋84'],['神识','＋72'],['寿元','＋1 年'],['精力','＋18']
  ], '领取收益',{glyph:'息'});
  sheet('StartLogoWnd','splash','修真界','卷首','修真界','',[],'');
  sheet('StartLoadSceneWnd','loading','入境','山门 · 启卷','修真界','正在校验修行典籍',[],'',{progress:64});
  sheet('LoadingWnd','loading','行旅途中','地图 · 转场','入山','正在载入场景',[],'',{progress:78});
  sheet('WangJiMiLingWnd','form','忘记密令','升仙司 · 旧式文书','重取凭据','凭号牌验证身份。',[
    ['升仙号牌','输入号牌'],['暗号','输入验证码'],['新密令','输入新密令']
  ], '确认重置',{link:'获取暗号'});

  // City, county atlas and locations.
  sheet('MainCityWnd','city','临仙城','东荒 · 青云洲','城中行止','临仙郡',[
    ['城主府','天书碑与公示'],['中街坊市','八阁开市'],['传送大阵','通往各郡']
  ], '出城历练',{image:'city',pins:[['城主府',59,22],['虚神界',24,43],['中街坊市',63,72],['传送大阵',21,83]]});
  sheet('WorldMapWnd','map','山河舆图','舆地志 · 东荒','临仙郡','东荒大陆　／　青云洲',[
    ['妖兽林 · 外围','可进入'],['妖兽林 · 腹地','可进入'],['妖兽林 · 核心','未探索']
  ], '进入历练',{image:'world',pins:[['寒渊郡',65,15],['临仙郡',57,46],['朝露郡',73,61],['沙海郡',19,64],['紫霞郡',44,82]]});
  sheet('ZhongJieFangShiWnd','market','中街坊市','临仙城 · 东市','八阁开市','按所需物资拜访商阁。', [
    ['神符阁','符箓'],['灵丹坊','丹药'],['百器坊','法器'],['道境阁','功法'],['术法阁','术法'],['杂货铺','杂货'],['奇珍阁','珍品'],['阵法阁','阵图']
  ], '进入灵丹坊',{image:'city'});
  sheet('WaiChuLiLianWnd','route','外出历练','临仙郡 · 行旅','妖兽林外','第一战区 · 外围',[
    ['目的地','妖兽林'],['消耗体力','10 点'],['行动方式','手动历练']
  ], '离开',{image:'region',pins:[['城门',20,76],['古道',46,54],['妖兽林',72,26]]});
  sheet('ChengNeiMiJingWnd','route','城内秘境','临仙城 · 秘境','城中别境','临仙郡 · 城域',[
    ['所在','临仙城'],['区域','城内秘境'],['归途','原路返回']
  ], '离开秘境',{image:'city',pins:[['城门',18,72],['仙径',47,49],['秘境',76,25]]});
  sheet('ChuanSongDaZhenWnd','array','传送大阵','临仙城 · 阵枢','大阵中枢','城池间的传送入口',[
    ['所在城池','临仙城'],['阵枢归属','青云洲'],['传送范围','同界城池']
  ], '离开大阵',{glyph:'迁'});
  sheet('ChuanSongTiShiWnd','confirm','传送确认','行旅 · 驿令','由临仙城前往翠微城','请核对目的地与灵石。',[
    ['出发','临仙城'],['抵达','翠微城'],['消耗','100 灵石']
  ], '确认传送',{secondary:'留在原地',glyph:'驿'});

  // Combat. The HUD and result views follow BattleWnd, ShaoDangWnd and settlement prefabs.
  sheet('BattleWnd','battle','妖兽林','临仙郡 · 外围','青岚螳','普通 · 木',[], '自动',{image:'region'});
  sheet('ShaoDangWnd','sweep','扫荡妖兽林','历练 · 扫荡','林间扫荡','预计约十秒，可清理部分地图资源。', [
    ['区域','妖兽林 · 外围'],['本次消耗','精力 10'],['预计清理','约 50%']
  ], '开始扫荡',{progress:0,image:'region'});
  sheet('ZhanDouSettlementWnd','loot','战斗结算','历练 · 战利录','此行所获','临仙郡妖兽林',[
    ['凝露草','×3'],['灵石','×42'],['岚纹镰刃','×1'],['灵木','×2']
  ], '一键领取',{footnote:'储物空间　16 / 120'});
  sheet('GuaiWuDiaoLuo','loot','妖兽掉落','战利品 · 待拾取','青岚螳','已击败 · 外围',[
    ['岚纹镰刃','×1'],['灵石','×12']
  ], '返回战场',{footnote:'当前战场掉落'});
  sheet('ZhaoDouJieGuo','receipt','战斗结果','历练 · 战报','得胜而归','此战已经结束',[
    ['所在战区','临仙郡外围'],['清理进度','45%'],['战斗用时','01 分 38 秒'],['击破妖兽','6 只']
  ], '返回',{glyph:'胜'});
  sheet('ZhongShangShowWnd','receipt','重伤','历练 · 伤势','气血亏损','此战受创，须留意当前状态。',[
    ['当前精血','240'],['当前真气','128'],['当前体魄','64'],['当前神识','80'],['当前寿元','78 年']
  ], '确定',{glyph:'伤',danger:true});

  // MainQuest and controls shared across the player's persistent HUD.
  sheet('MainTitleWnd','hud','行囊与修为','角色 · 常驻栏','沈青禾','云游散人　／　炼气四层',[
    ['体力','72 / 100'],['灵石','1,280'],['灵玉','16']
  ], '前往洞府',{image:'woman'});
  sheet('ChengZhuFuMainWnd','quests','城主府','临仙城 · 府衙','天书碑','张榜示众，论功行赏。',[
    ['山外除祟','进行中　4 / 6'],['收集灵草','进行中　2 / 5'],['初谒城主','已完成　可领取']
  ], '领取奖励',{image:'city'});
  sheet('ChengZhuFuWnd','quests','谒见城主','临仙城 · 府衙','府中公示','查阅城主委托与天书碑。',[
    ['来访','沈青禾'],['身份','云游散人'],['府中事项','天书碑　／　公示']
  ], '查看公示',{image:'city'});
  sheet('ChengZhuFuChengWeiKeQingWnd','confirm','受聘客卿','城主府 · 聘书','临仙城客卿','城主府聘请道友入府。', [
    ['受聘修士','沈青禾'],['供职','城主府'],['名衔','客卿']
  ],'查看聘书',{glyph:'聘'});
  sheet('GuideWnd','guide','初入仙途','入门 · 指引','初到临仙','随向导熟悉城中设施。',[
    ['当前目标','前往洞府'],['下一处','练功房']
  ], '继续',{secondary:'跳过',image:'city'});
  sheet('GuideNewWnd','guide','修行指引','入门 · 引导','纳戒藏珍','修行所需已发放至纳戒，请用神识查探。',[
    ['当前目标','打开纳戒'],['可查看','功法与修行物资']
  ], '查看纳戒',{secondary:'跳过',image:'woman'});

  // Cultivation: distinct rituals, meridians, retreats, room management and technique states.
  sheet('DongFuWnd','meditate','练功房','洞天 · 修习','周天运转','地阶聚灵阵已运转',[
    ['纳气','玄玉宝典　67.5 / 周天'],['锻体','猿魔圣典　4.3 / 周天'],['炼神','玉恒诀　3.7 / 周天']
  ], '闭关突破',{glyph:'气',progress:72,tabs:['纳气','锻体','炼神']});
  sheet('DongFuExmpleWnd','meditate','练功演示','入门 · 练功房','引气入体','选取已参悟的功法，开始一个周天。',[
    ['纳气','玄玉宝典　67.5 / 周天'],['聚灵阵','地阶　剩余 2 小时'],['控灵阵','地阶　剩余 1 小时']
  ], '更换功法',{glyph:'引',progress:42,tabs:['纳气','锻体','炼神']});
  sheet('DongFuChooseWnd','places','择取洞府','临仙郡 · 洞天','普通洞府','选择要进入的洞府。',[
    ['松风居','普通洞府'],['所在地','临仙郡'],['用途','修行与储物']
  ], '进入洞府',{image:'region'});
  sheet('DongFuRoomWnd','rooms','洞府','洞天 · 营造','松风居','按用途管理洞府各室。',[
    ['练功房','已开辟'],['密室','已开辟'],['灵田','待开辟'],['地火室','待开辟'],['悟道台','待开辟']
  ], '进入练功房',{glyph:'府'});
  sheet('DongFuMiShiWnd','storage','密室','洞天 · 藏珍','松风居密室','选择一只宝箱查看所藏。',[
    ['灵植匣','12 / 30'],['器物匣','8 / 30'],['空宝箱','0 / 30']
  ], '开启宝箱',{secondary:'购买宝箱',image:'chest'});
  sheet('YouJianDongFuWnd','places','幽涧洞府','洞天 · 行迹','幽涧深处','山间洞府',[
    ['区域','幽涧洞府'],['归途','城池'],['状态','可以离开']
  ], '离开洞府',{image:'region'});
  sheet('DiHuoMiShiWnd','places','地火密室','洞天 · 地脉','地火石室','地火沿石脉汇入炉室。',[
    ['所在','地火密室'],['地脉','火属'],['归途','离开石室']
  ], '离开密室',{image:'region'});
  sheet('DongFuYongTuChooseWnd','choices','房间用途','洞天 · 营造','选定用途','用途选定后不可轻易更改。',[
    ['炼丹','炉火炼药'],['画符','运笔凝符'],['修法','研习术法'],['悟道','参悟功法'],['阵法','布置法阵'],['炼器','锻造法器']
  ], '确认用途',{selection:0,image:'pavilion'});
  sheet('DongFuZhiFuWnd','confirm','洞府支付','洞天 · 账册','确认支付','请核对此次洞府开支。',[
    ['支付灵玉','100'],['当前灵玉','220'],['支付后剩余','120']
  ], '确认支付',{secondary:'取消',glyph:'契'});
  sheet('JuLingFaZhenWnd','array','聚灵法阵','洞天 · 阵法','地阶聚灵阵','汇聚灵气，提升洞府的灵气浓郁程度。',[
    ['灵气浓郁度','＋20%'],['投入灵石','100'],['增加运转','2 小时']
  ], '确认投入',{secondary:'升级法阵',glyph:'聚'});
  sheet('KongLingFaZhenWnd','array','控灵法阵','洞天 · 阵法','地阶控灵阵','以阵纹约束洞府灵流。',[
    ['当前效率','20%'],['投入灵石','50'],['维持运转','1 小时']
  ], '确认投入',{secondary:'升级法阵',glyph:'控'});
  sheet('GengHuanFaZhenDengJieWnd','choices','升级阵法','洞天 · 阵阶','选择阵阶','当前为玄阶，可比较各阶阵法。',[
    ['黄阶','基础'],['玄阶','当前阵阶'],['地阶','可更换'],['天阶','尚未开放']
  ], '更换阵阶',{selection:2,glyph:'阵'});
  sheet('FaZhenXiaoHaoLingShiWnd','confirm','法阵消耗','洞天 · 阵簿','确认消耗灵石','灵石将用于维持法阵运转。',[
    ['本次消耗','100 灵石'],['已有灵石','1,280'],['扣除后','1,180']
  ], '确认投入',{secondary:'取消',glyph:'阵'});
  sheet('CanWuWnd','library','参悟','修行 · 藏书','研读典籍','已习三卷，另有二卷可参悟。',[
    ['玄玉宝典','纳气　已参悟'],['猿魔圣典','锻体　可参悟'],['玉恒诀','炼神　可参悟']
  ], '选择典籍',{tabs:['全部','功法','法术','丹方'],image:'book'});
  sheet('CanWuGongFaWnd','book','功法残卷','修行 · 古籍','玄玉宝典','上篇已录，合十卷可补全中篇。',[
    ['现有残卷','9 / 10'],['已录篇目','上篇'],['仍需残卷','1 张']
  ], '补齐功法',{secondary:'提升品阶',image:'book'});
  sheet('CanWuZhongWnd','meditate','参悟中','修行 · 冥想','玄玉宝典','神识入卷，心法渐明。',[
    ['当前状态','冥想中'],['参悟进度','68%'],['剩余','3 秒']
  ],'返回洞府',{glyph:'悟',progress:68});
  sheet('GengHuanGongFaWnd','library','更换功法','修行 · 心法','当前功法','玄玉宝典 · 纳气',[
    ['玄玉宝典','纳气　当前'],['照影定心诀','炼神　已习'],['玄龟炼体术','炼体　已习']
  ], '确认更换',{tabs:['全部','纳气','炼神','锻体'],image:'book'});
  sheet('JingMaiWnd','meridian','经脉','内景 · 经络','任脉','已通两穴，可继续通脉。',[
    ['剩余经脉点','30 点'],['突破能量','72%'],['下一处','胞中']
  ], '继续通脉',{glyph:'任',progress:72});
  sheet('NeiShiWnd','meridian','内视','内景 · 穴位','胞中','任脉　／　未通穴',[
    ['当前层数','第一层'],['通脉消耗','10 经脉点'],['成功几率','100%']
  ], '通穴',{glyph:'穴',secondary:'总览'});
  sheet('ZongLanWnd','meridian','经脉总览','内景 · 八脉','奇经八脉','按脉查看穴位进度与加成。',[
    ['任脉','2 / 8'],['督脉','1 / 8'],['冲脉 · 带脉','0 / 12'],['阴阳跷脉','0 / 12'],['阴阳维脉','0 / 12']
  ], '查看任脉',{glyph:'脉'});
  sheet('WuDaoTaiWnd','array','悟道台','心法 · 悟道','玉恒诀','选定要保留的特效，再行悟道。',[
    ['特效一','神识凝聚　锁定'],['特效二','纳气增益　可选'],['特效三','灵流贯通　可选']
  ], '普通悟道',{secondary:'高级悟道',glyph:'悟'});
  sheet('WuDaoTaiIXiangQingWnd','book','确认参悟','悟道台 · 心法','玉恒诀','本次悟道预览',[
    ['精力消耗','100 点'],['周天变化','3.7 → 4.2'],['当前途径','炼神']
  ], '确认参悟',{image:'book'});
  sheet('TiShengPingJieWnd','book','提升品阶','心法 · 晋升','玄玉宝典','聚集灵衍，提升品阶。',[
    ['当前品阶','黄阶上品'],['晋升目标','玄阶下品'],['灵衍值','8,200 / 10,000']
  ], '查看晋升',{image:'book'});
  sheet('TuPoDuJieWnd','tribulation','渡劫','破境 · 雷关','第一道天雷','守住防御，抵御雷劫。',[
    ['防御值','3,004'],['灵气','14,321'],['法器','已就绪']
  ], '硬撼雷劫',{glyph:'劫',secondary:'瞬移'});
  sheet('TuPoDuJieExmpleWnd','tribulation','渡劫演示','入门 · 破境','雷劫将至','长按可硬撼雷劫，防御耗尽则渡劫失败。',[
    ['防御','3,004'],['真气','14,321'],['可用应对','阵法 · 瞬移']
  ], '开始演示',{glyph:'雷'});
  sheet('TuPoDuJieFailWnd','receipt','渡劫失败','破境 · 劫录','渡劫未成','雷势未散，先稳固自身气机。',[
    ['当前精血','240'],['当前真气','128'],['当前体魄','64'],['当前神识','80'],['当前寿元','78 年']
  ], '确认',{danger:true,glyph:'劫'});
  sheet('TuPoDuJieSuccessWnd','receipt','渡劫成功','破境 · 劫录','破境功成','雷云消散，修为更进一层。',[
    ['寿元上限','＋10 年'],['真元上限','＋120'],['体魄上限','＋60'],['神识上限','＋50'],['精血上限','＋80']
  ], '确认',{glyph:'成'});

  const asset = name => 'assets/journey/' + name;
  const commonArt = key => key === 'market' ? asset('city.jpg') : key === 'battle' ? asset('region.jpg') : 'assets/' + key + '.jpg';
  const localImages = {
    city:'city.jpg', world:'world.jpg', region:'region.jpg', woman:'woman.png', man:'man.png',
    book:'technique.png', seal:'compass.png', pavilion:'residence.jpg', chest:'chest.png'
  };
  const img = (name,cls='') => '<img class="'+cls+'" src="'+asset(name)+'" alt="">';
  const imagePath = (s,key) => key && localImages[key] ? asset(localImages[key]) : commonArt('mountain');
  function surface(src,extra='') {
    return '<div class="j-paint '+extra+'" style="background-image:url(&quot;'+src+'&quot;)"></div>';
  }
  function stroke(path,cls='') {
    return '<svg class="j-line '+cls+'" viewBox="0 0 100 100" preserveAspectRatio="none" aria-hidden="true"><path d="'+path+'" fill="none" stroke="currentColor" stroke-width=".48" stroke-dasharray="1.1 1.3"/></svg>';
  }
  function pin(label,x,y,active) {
    return '<span class="j-pin '+(active?'is-current':'')+'" style="left:'+x+'%;top:'+y+'%"><i></i><b>'+label+'</b></span>';
  }
  const unit = ['壹','贰','叁','肆','伍','陆','柒','捌'];
  const footer = s => '<footer class="j-footer">'+(s.footnote?'<p>'+s.footnote+'</p>':'')+'<div>'+(s.secondary?'<span>'+s.secondary+'</span>':'')+(s.action?'<strong>'+s.action+'<i>›</i></strong>':'')+'</div></footer>';
  const head = (s,light=false) => '<header class="j-head '+(light?'is-light':'')+'"><div class="j-back">‹</div><span>'+s.overline+'</span><i></i></header>';
  function record(rows,cls='') {
    return '<div class="j-record '+cls+'">'+rows.map(([l,v],i)=>'<div><span>'+l+'</span><b>'+v+'</b></div>').join('')+'</div>';
  }
  const tabs = arr => '<div class="j-tabs">'+arr.map((n,i)=>'<span class="'+(!i?'is-active':'')+'">'+n+'</span>').join('')+'</div>';
  const intro = (cap,title,note='') => '<div class="j-intro"><span>'+cap+'</span><h2>'+title+'</h2>'+(note?'<p>'+note+'</p>':'')+'</div>';
  const tag = word=>'<span class="j-stamp">'+word+'</span>';
  function fields(s) {
    return '<div class="j-fields">'+s.rows.map(([label,hint],i)=>'<div><label>'+label+'</label><span>'+hint+'</span>'+((s.mode==='form'&&s.link&&label.match(/暗号|验证码/))?'<b>'+s.link+'</b>':'')+'</div>').join('')+'</div>';
  }
  function authentication(s) {
    if(s.mode==='splash'||s.mode==='loading') {
      return '<section class="j-root j-splash">'+surface(commonArt('mountain'))+
        '<div class="j-bookstrap"><b>'+s.subject+'</b>'+tag('入境')+'</div>'+
        (s.mode==='loading'?'<div class="j-loading"><span>'+s.detail+'</span><b>'+s.progress+'%</b><i style="--p:'+s.progress+'%"></i></div>':'<div class="j-colophon">修 真 界</div>')+'</section>';
    }
    if(s.mode==='login') {
      return '<section class="j-root j-login">'+surface(commonArt('mountain'))+
        '<div class="j-bookstrap"><b>修真界</b>'+tag('山门')+'</div>'+
        '<div class="j-authbox">'+intro('升仙令','持令入界')+fields(s)+
        '<div class="j-links"><span>获取升仙令</span><span>忘记密令</span></div><div class="j-agree">□　我已阅读并同意用户协议</div></div>'+footer({...s,secondary:''})+'</section>';
    }
    return '<section class="j-root j-form">'+head(s)+
      '<div class="j-formheading">'+surface(commonArt('mountain'))+intro(s.heading,s.subject,s.detail)+'</div>'+
      '<div class="j-formbody">'+fields(s)+'</div>'+footer(s)+'</section>';
  }
  function cartographic(s) {
    const line = s.mode==='city'?'M21 83 Q35 65 63 72 T59 22 M24 43 Q34 35 59 22':
      s.mode==='map'?'M19 64 Q38 52 57 46 T65 15 M57 46 L73 61 L44 82':'M20 76 Q27 63 46 54 T72 26';
    const district = s.mode==='city'?'青云洲 · 临仙郡':s.mode==='map'?'四境 · 六郡':'临仙郡';
    return '<section class="j-root j-atlas j-'+s.mode+'">'+head(s)+
      '<div class="j-atlasheading"><span>'+district+'</span><h1>'+s.heading+'</h1>'+tag(s.mode==='city'?'城域':'舆图')+'</div>'+
      '<div class="j-mapdrawing">'+surface(imagePath(s,s.image))+stroke(line)+
      s.pins.map((p,i)=>pin(...p,i===(s.mode==='map'?1:0))).join('')+
      '<span class="j-north">北<br>⌖</span></div><div class="j-mapindex">'+
      intro(s.detail,s.subject)+record(s.rows)+'</div>'+footer(s)+'</section>';
  }
  function combat(s) {
    return '<section class="j-root j-combat">'+surface(asset('region.jpg'))+
      head(s)+ '<div class="j-battle-title"><span>临仙郡 · 第一战区</span><h1>松风竹海</h1><b>战斗中　01:38</b></div>'+
      '<div class="j-target"><label>锁定目标</label><b>'+s.subject+'</b><span>'+s.detail+'</span><i></i></div>'+
      '<div class="j-fightspace"><div class="j-range"></div><div class="j-enemy">'+img('mantis.png')+'<b>青岚螳</b></div>'+
      '<div class="j-fighter">'+img('meditator.png')+'<b>沈青禾</b></div><div class="j-damage">−128</div></div>'+
      '<div class="j-battlecontrol"><div class="j-bars"><span>防护<i style="--p:82%"></i><b>820</b></span><span>真元<i style="--p:68%"></i><b>486</b></span><span>神识<i style="--p:94%"></i><b>216</b></span></div>'+
      '<div class="j-controls"><div class="j-stick"><b>✥</b></div><div class="j-skills"><i>符</i><i>器</i><i>术</i><strong>击</strong></div></div>'+
      '<div class="j-combat-foot"><span>纳戒</span><b>自动战斗</b><span>离开</span></div></div></section>';
  }
  function retreat(s) {
    const isTest=s.mode==='tribulation',ritual=s.mode==='array',med=s.mode==='meditate';
    const background=isTest?commonArt('mountain'):med?asset('chamber.jpg'):commonArt('cave');
    const figure=isTest?'breakthrough.png':med?'meditator.png':(s.glyph==='控'?'control.png':'gather.png');
    return '<section class="j-root j-retreat '+(isTest?'j-storm':'')+'">'+head(s)+
      '<div class="j-ritualart">'+surface(background)+ '<div class="j-ritualhead">'+tag(isTest?'劫录':ritual?'阵录':'洞天')+'<h1>'+s.heading+'</h1></div>'+
      '<div class="j-orbit"><i></i><i></i></div>'+img(figure,'j-ritualfigure')+
      '<div class="j-markers"><span>乾</span><span>坤</span></div></div><div class="j-retreatbody">'+
      intro(s.overline,s.subject,s.detail)+(s.tabs?tabs(s.tabs):'')+record(s.rows)+
      (s.progress!==undefined?'<div class="j-meter"><i style="--p:'+s.progress+'%"></i><span>'+s.progress+'%</span></div>':'')+'</div>'+footer(s)+'</section>';
  }
  function channel(s) {
    return '<section class="j-root j-channel">'+head(s)+'<div class="j-channelhead">'+intro(s.heading,s.subject,s.detail)+tag('内景')+'</div>'+
      '<div class="j-acu">'+img('meditator.png')+stroke('M50 10 Q44 21 50 27 T48 46 T48 60 Q52 68 51 82','j-spine')+
      [['百会',50,13],['大椎',49,32],['命门',50,53],['胞中',50,74]].map(([n,x,y],i)=>'<div class="j-acupoint p'+i+'" style="left:'+x+'%;top:'+y+'%"><i></i><span>'+n+'</span></div>').join('')+
      '</div><div class="j-channelrows">'+record(s.rows)+'</div>'+footer(s)+'</section>';
  }
  function role(s) {
    return '<section class="j-root j-identity">'+head(s)+'<div class="j-identityheader">'+intro(s.mode==='role'?'形貌 · 灵根 · 出生地':'角色常驻栏',s.heading)+'</div>'+
      '<div class="j-profile">'+surface(commonArt('mountain'))+img('woman.png')+
      '<div>'+tag(s.mode==='role'?'入世':'道籍')+'<b>沈青禾</b><span>'+s.detail+'</span></div></div>'+
      '<div class="j-profiledata">'+(s.tabs?tabs(s.tabs):'')+record(s.rows)+
      (s.mode==='role'?'<div class="j-smallhdr">选择形貌</div><div class="j-avatars">'+img('woman.png')+img('man.png')+'</div>':
        '<div class="j-smallhdr">常用入口</div><div class="j-grid">'+['洞府','纳戒','装备','参悟','历练','道友'].map((x,i)=>'<div><i>'+unit[i]+'</i><b>'+x+'</b></div>').join('')+'</div>')+
      '</div>'+footer(s)+'</section>';
  }
  sheet('AttentionWnd','confirm','提示','山门 · 通知','请先核验升仙令','身份验证成功后，可继续进入修真界。',[
    ['操作','验证身份']
  ], '确定',{secondary:'返回',glyph:'示'});

  function designArt(s) {
    if(s.mode==='market') return commonArt('market');
    if(s.mode==='loot'||s.mode==='sweep'||s.danger) return commonArt('battle');
    if(s.mode==='book'||s.mode==='library'||s.mode==='rooms'||s.mode==='storage') return commonArt('cave');
    if(s.mode==='quests'||s.mode==='servers'||s.mode==='choices') return commonArt('sect');
    if(s.mode==='places') return asset(s.heading.includes('地火')?'chamber-fire.jpg':'residence.jpg');
    return commonArt('mountain');
  }
  function token(ui,word,i) {
    const icon = /丹|灵丹|药/.test(word)?'pill':/符/.test(word)?'talisman':/灵石|灵玉/.test(word)?'coin':
      /阵/.test(word)?'crystal':/草|灵植/.test(word)?'herb':/书|法|诀|卷|典|经/.test(word)?'book':
      /刃|剑|器/.test(word)?'sword':/库|匣/.test(word)?'purse':['pendant','scroll','ring','crystal'][i%4];
    return ui.relic ? ui.relic(icon,34) : '<i class="j-token">'+word.charAt(0)+'</i>';
  }
  function indexRows(s,ui) {
    return '<div class="j-indexrows">'+s.rows.map(([k,v],i)=>'<div><i class="j-indexpic">'+token(ui,k,i)+
      '</i><b>'+k+'</b><small>'+v+'</small><span>›</span></div>').join('')+'</div>';
  }
  function tileGrid(s,ui) {
    return '<div class="j-grid j-itemtiles">'+s.rows.map(([k,v],i)=>'<div class="'+(s.selection===i?'selected':'')+'">'+
      token(ui,k,i)+'<b>'+k+'</b><small>'+v+'</small></div>').join('')+'</div>';
  }
  function general(s,ui) {
    const isReceipt=s.mode==='receipt', isBook=s.mode==='book'||s.mode==='library',
      isGrid=['choices','loot','market','storage'].includes(s.mode), isNotice=s.mode==='confirm';
    if(isNotice) {
      return '<section class="j-root j-notice">'+surface(designArt(s))+head(s,true)+
        '<article class="j-noticecard"><div class="j-formalhead">'+tag(s.glyph||'契')+'<span>'+s.overline+'</span></div>'+
        '<h1>'+s.heading+'</h1><div class="j-thumbtitle">'+s.subject+'</div><p>'+s.detail+'</p>'+record(s.rows)+footer(s)+'</article></section>';
    }
    const artwork=designArt(s), title= isReceipt?s.subject:s.heading;
    return '<section class="j-root j-sheet j-'+s.mode+'">'+head(s)+
      '<div class="j-vignette">'+surface(artwork)+'<div class="j-vhead">'+tag(isReceipt?(s.glyph||'录'):(isBook?'藏书':s.mode==='market'?'东市':s.mode==='loot'?'战录':'记'))+
      '<p>'+s.overline+'</p><h1>'+title+'</h1></div>'+
      (isBook?img('technique.png','j-bookimg'):s.mode==='storage'?img('chest.png','j-bookimg'):'')+'</div>'+
      '<div class="j-sheetbody">'+intro(s.heading===title?s.overline:s.heading,s.subject===title?s.overline.split(' · ').at(-1):s.subject,s.detail)+
      (s.mode==='servers'?'<div class="j-smallhdr">服务器名录</div>':'')+(s.tabs?tabs(s.tabs):'')+
      (s.mode==='sweep'?'<div class="j-survey"><span>地图清理</span><strong>'+s.progress+'%</strong><i></i></div>':'')+
      (s.mode==='guide'?'<div class="j-teacher">'+img('man.png')+'<div><b>引路人</b><p>'+s.detail+'</p></div></div>':'')+
      (s.mode==='quests'?tabs(['府中公示','天书碑']):'')+
      (isGrid?tileGrid(s,ui):isReceipt||s.mode==='guide'||s.mode==='book'||s.mode==='places'||s.mode==='sweep'?record(s.rows):
        indexRows(s,ui))+'</div>'+footer(s)+'</section>';
  }
  window.journeyDesign = function(d,ui) {
    if(!['Account','Map','Battle','Cultivation','MainQuest'].includes(d.module)||d.kind==='component') return null;
    const key = d.id.replace(/\.prefab$/i,'');
    const s = sheets[key]; if(!s) return null;
    if(d.module==='Cultivation') return window.cultivationPage(d,s);
    if(['form','login','splash','loading'].includes(s.mode)) return authentication(s);
    if(['city','map','route'].includes(s.mode)) return cartographic(s);
    if(s.mode==='battle') return combat(s);
    if(['meditate','array','tribulation'].includes(s.mode)) return retreat(s);
    if(s.mode==='meridian') return channel(s);
    if(s.mode==='role'||s.mode==='hud') return role(s);
    return general(s,ui);
  };
})();
