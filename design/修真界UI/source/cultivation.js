/* Sample character for the editable local boards. Changes last for this page session. */
(() => {
  const elements = {
    金:{art:'jin', creates:'水', controls:'木'}, 木:{art:'mu', creates:'火', controls:'土'},
    水:{art:'shui', creates:'木', controls:'火'}, 火:{art:'huo', creates:'土', controls:'金'},
    土:{art:'tu', creates:'金', controls:'水'}
  };
  const root = '木';
  const demoLevel = new URLSearchParams(location.search).get('view') === 'system-methods' ? 11 : 31;
  const realm = level => ['练气','筑基','结丹','元婴'][Math.floor((level - 1) / 10)] + '一层';
  const pulse = [
    {name:'纳气', id:'gf_3_2', book:'聚元功', element:'木', value:56, gain:'修为'},
    {name:'锻体', id:'gf_2_5', book:'玄龟炼体术', element:'土', value:32, gain:'体魄'},
    {name:'炼神', id:'gf_1_4', book:'太乙炼神篇', element:'火', value:38, gain:'神识'}
  ];
  const nav = [['DongFuWnd','练功'],['CanWuWnd','参悟'],['NeiShiWnd','内视'],['TuPoDuJieExmpleWnd','渡劫']];
  const numerals = ['一','二','三','四','五','六'];
  const sectionNames = ['周天修习','典籍参悟','经脉内景','破境雷关'];
  const state = {running:[0], preview:[0], selected:{}, notice:{}, locks:[0]};
  let active = null;
  const pic = (path, css='') => '<img class="'+css+'" src="assets/'+path+'" alt="" draggable="false">';
  const art = (e,css='') => pic('parts/'+elements[e].art+'.png',css);
  const button = (text, action, value='', cls='', attrs='') => '<button type="button" class="'+cls+'" data-x-action="'+action+'" data-x-value="'+value+'" '+attrs+'>'+text+'</button>';
  const go = (text,id,cls='') => button(text,'open',id,cls);
  const more = '<span class="x-chevron" aria-hidden="true">›</span>';
  const heading = (name, aside='') => '<div class="x-block-title"><h2>'+name+'</h2><span>'+aside+'</span></div>';
  const record = rows => '<dl class="x-record">'+rows.map(([k,v])=>'<div><dt>'+k+'</dt><dd>'+v+'</dd></div>').join('')+'</dl>';
  const meter = (value,max) => '<span class="x-meter"><i style="width:'+Math.min(100,100*value/max)+'%"></i></span>';
  const action = (label, event, value='', disabled=false) => button(label+'<span aria-hidden="true">→</span>',event,value,'x-major',disabled?'disabled':'');
  const minor = (label, event, value='') => button(label,event,value,'x-minor');
  const dataId = d => 'data-board="'+d.id+'"';
  function sectionOf(id) {
    if(id.startsWith('TuPo')) return 3;
    if(/JingMai|NeiShi|ZongLan/.test(id)) return 2;
    return /CanWu|GongFa|WuDao|TiSheng/.test(id) ? 1 : 0;
  }
  function frame(d, {hero, body, foot='', group=sectionOf(d.id), moreClass=''}){
    return '<article class="x-cult '+moreClass+'" '+dataId(d)+'><header class="x-top"><span class="x-chapter">修真界<i></i>修炼与参悟</span><span>'+sectionNames[group]+'</span></header>'+
      hero+'<div class="x-body">'+body+'</div>'+(foot?'<footer class="x-foot">'+foot+'</footer>':'')+
      '<nav class="x-nav" aria-label="修行去处">'+nav.map(([id,name],i)=>button(name,'open',id,i===group?'active':'',i===group?'aria-current="page"':'')).join('')+'</nav></article>';
  }
  function scenery(title,sub,scene='breath', emblem='', badge='') {
    return '<div class="x-hero x-scene-'+scene+'"><div class="x-paint"></div><div class="x-hero-type"><p>'+sub+'</p><h1>'+title+'</h1></div>'+
      (emblem?pic(emblem,'x-hero-emblem'):scene==='breath'?pic('journey/meditator.png','x-hero-meditator'):scene==='study'?pic('journey/technique.png','x-hero-volume'):'')+(badge?'<div class="x-hero-caption">'+badge+'</div>':'')+'</div>';
  }
  const roleSeal = '<span class="x-role-seal" aria-hidden="true">沈</span>';
  const cultivator = '<div class="x-identity">'+roleSeal+'<div><b>沈青禾</b><small>'+realm(demoLevel)+'</small></div><span>'+art(root)+'木灵根</span></div>';
  function selectedRows(id, rows, icons, interactive=false) {
    const current = state.selected[id] ?? 0;
    return '<div class="x-list">'+rows.map(([title,note],i)=>{
      const inner=(icons?.[i]?pic(icons[i],'x-list-icon'):'')+'<span><b>'+title+'</b><small>'+note+'</small></span>'+
        (interactive?'<i aria-hidden="true">'+(current===i?'●':'○')+'</i>':'');
      return interactive?button(inner,'select',i,'x-line'+(i===current?' chosen':''),'aria-pressed="'+(i===current)+'"'):'<div class="x-line">'+inner+'</div>';
    }).join('')+'</div>';
  }
  const rowIcons = names=>names.map(n=>'relics/'+n+'.webp');
  function practice(d) {
    const chosen=state.preview;
    const names=chosen.map(i=>pulse[i].name).join('、');
    const gain = (i)=>{
      const p=pulse[i];
      const generated=chosen.some(j=>j!==i && elements[pulse[j].element].creates===p.element);
      return (p.value*(p.element===root?1.2:1)*(chosen.length===2?.7:1)*(generated?1.2:1));
    };
    const planned=chosen.map(i=>'<div><span>'+pulse[i].gain+'</span><b>＋'+gain(i).toFixed(1)+'</b></div>').join('');
    const options=pulse.map((p,i)=>button(art(p.element)+'<span><b>'+p.name+'</b><small>'+p.book+'</small></span><em>'+p.element+'行</em><i>'+ (chosen.includes(i)?'✓':'')+'</i>','practice',i,'x-line x-method'+(chosen.includes(i)?' chosen':''),'aria-pressed="'+chosen.includes(i)+'"')).join('');
    const linked=chosen.length===2&&chosen.find(i=>chosen.some(j=>i!==j&&elements[pulse[j].element].creates===pulse[i].element));
    const rules=[demoLevel >= 31 ? '元婴可兼修两门，各取七成。' : '元婴一层起可兼修两门。'];
    if(chosen.some(i=>pulse[i].element===root)) rules.push('木灵根与木行功法同属，收益增两成。');
    if(linked!==false && linked!==undefined) rules.push(pulse[linked].name+'受相生，额外增两成。');
    return frame(d,{hero:scenery(d.id==='DongFuWnd'?'练功房':'练功演示','松风居 · 地阶洞府','breath','','当前运行　'+state.running.map(i=>pulse[i].name).join('、')),
      body:cultivator+heading('周天推演','已选 '+chosen.length+' / '+(demoLevel >= 31 ? 2 : 1))+'<div class="x-list">'+options+'</div>'+
        '<div class="x-estimate"><span>每周天预计所得</span><div>'+planned+'</div><p>'+rules.join('')+'</p></div>',
      foot:minor('更换功法','open','GengHuanGongFaWnd')+action(state.notice[d.id]||'运转'+names,'run')});
  }
  const systemBooks = window.systemMethodCatalog;
  const techniqueKinds = {1:'炼神',2:'锻体',3:'纳气'};
  const demoCycle = [56,32,38];
  const tabs = ['可参悟','待突破','已参悟'];
  const starter = 'gf_3_1';
  function legacyBook(id, name) {
    const [,kind,rank] = id.split('_').map(Number);
    const tier = Math.floor((rank - 1) / 4), grade = (rank - 1) % 4;
    return {id, name, kind, rank, rankName:['黄','玄','地','天'][tier]+'阶'+['下品','中品','上品','极品'][grade],
      element:id === starter ? root : systemBooks.element[(rank - 1) % 5 + 1],
      learningLevel:1 + 10 * tier, baseCircle:.8 * [1,4,16,64][tier] * [1,1.1,1.2,1.3][grade]};
  }
  const methods = [
    ...pulse.map(p => legacyBook(p.id, p.book)), legacyBook(starter,'启灵经'),
    legacyBook('gf_2_2','淬体诀'), legacyBook('gf_1_7','神魂淬炼诀'), legacyBook('gf_1_12','混沌神魂诀'),
    legacyBook('gf_2_1','铁骨功'), ...systemBooks.methods.map(m => ({...m, system:true,
      signature:systemBooks.polarity[m.polarity]+systemBooks.element[m.element]+' · '+systemBooks.qi[m.qi]+'气',
      element:systemBooks.element[m.element], rankName:systemBooks.rank[m.rank]}))
  ];
  const byId = new Map(methods.map(m => [m.id,m]));
  const shelf = {tab:tabs[0], page:0, view:'', current:starter, from:'CanWuWnd', notice:'',
    read:new Set([starter,'gf_1_7']), learned:new Set(pulse.map(p => p.id)),
    bag:new Map([['gf_2_2',2],['gf_1_12',1],['gf_2_1',1]])};
  function studyState(m) {
    if (shelf.learned.has(m.id)) return tabs[2];
    if (m.id !== starter && m.element !== root || !m.system && !shelf.read.has(m.id)) return '';
    return m.learningLevel <= demoLevel ? tabs[0] : tabs[1];
  }
  const bookLine = m => techniqueKinds[m.kind]+' · '+m.element+'行 · '+m.rankName;
  const source = m => m.system ? '天道传承' : '普通功法';
  const stamp = '沈青禾 · '+root+'灵根 · '+realm(demoLevel);
  function library(d) {
    const equip = d.id === 'GengHuanGongFaWnd';
    if (!equip && shelf.view === 'bag') {
      const bagRows = [...shelf.bag].filter(([,count]) => count > 0).map(([id,count]) => {
        const m = byId.get(id);
        const tip = m.element !== root ? '灵根不合' : shelf.read.has(id) || shelf.learned.has(id) ? '已收录' : '阅读';
        return button(art(m.element,'x-list-icon')+'<span><b>'+m.name+' ×'+count+'</b><small>'+bookLine(m)+'</small></span><em>'+tip+'</em>',
          'read',id,'x-line',tip === '阅读' ? '' : 'disabled');
      }).join('');
      return frame(d,{hero:scenery('纳戒','随身功法书','study','',stamp),body:heading('功法书')+'<div class="x-list">'+bagRows+'</div>'+
        (shelf.notice ? '<p class="x-note" role="status">'+shelf.notice+'</p>' : ''),foot:action('回参悟书目','shelf')});
    }
    if (!equip && shelf.view === 'rules') return frame(d,{hero:scenery('参悟规则','藏经阁','study','',stamp),
      body:record([['普通功法','先在纳戒读书收录'],['天道传承','灵根相合即可入册'],['参悟条件','五行相合，达到所需境界'],['入门心法','《启灵经》随本命灵根'],['学成之后','到练功房选择装配']]),
      foot:action('回书目','shelf')});
    const tab = equip ? tabs[2] : shelf.tab;
    const visible = methods.filter(m => studyState(m) === tab).sort((a,b) => Number(b.id === starter) - Number(a.id === starter)
      || a.learningLevel - b.learningLevel || Number(!!a.system) - Number(!!b.system) || a.rank - b.rank || a.id.localeCompare(b.id));
    const pageSize = 12, pages = Math.max(1,Math.ceil(visible.length / pageSize));
    shelf.page = Math.min(Math.max(0,shelf.page),pages-1);
    const tabBar = equip ? '' : '<div class="x-shelf-switch" aria-label="参悟状态">'+tabs.map(t =>
      button(t,'tab',t,tab === t ? 'selected' : '','aria-pressed="'+(tab === t)+'"')).join('')+'</div>';
    const rows = visible.slice(shelf.page*pageSize,(shelf.page+1)*pageSize).map(m => {
      const status = tab === tabs[1] ? realm(m.learningLevel)+'可悟' : tab === tabs[2] ? (pulse.some(p => p.id === m.id) ? '已装配' : '已参悟') : source(m);
      return button(art(m.element,'x-list-icon')+'<span><b>'+m.name+'</b><small>'+bookLine(m)+'</small></span><em>'+status+' ›</em>','book',m.id,'x-line');
    }).join('');
    const empty = ['暂无可悟典籍，可在纳戒阅读相合的功法书。','没有等待突破的典籍。','尚未参悟典籍。'][tabs.indexOf(tab)];
    const pager = pages < 2 ? '' : '<div class="x-system-pager">'+button('上页','page',-1,'',shelf.page===0?'disabled':'')+
      '<span>'+ (shelf.page+1)+' / '+pages+'</span>'+button('下页','page',1,'',shelf.page===pages-1?'disabled':'')+'</div>';
    return frame(d,{moreClass:'x-library x-system',hero:scenery(equip?'更换功法':'参悟书目','藏经阁','study','',stamp),
      body:tabBar+heading(equip?'已悟功法':tab,visible.length+' 门')+'<div class="x-list">'+(rows || '<p class="x-note">'+empty+'</p>')+'</div>'+pager,
      foot:equip ? minor('回练功房','open','DongFuWnd') : minor('规则','rules')+action('纳戒读书','bag')});
  }
  function bookDetail(d) {
    const m = byId.get(shelf.current), status = studyState(m);
    const back = minor('返回书目','open',shelf.from);
    const title = scenery(m.name,source(m)+' · '+techniqueKinds[m.kind],'study','',stamp);
    if (d.id === 'TiShengPingJieWnd') return frame(d,{hero:title,body:heading('功法品阶')+record([['当前品阶',m.rankName]]),
      foot:action('返回典籍','open','CanWuGongFaWnd')});
    if (d.id === 'CanWuZhongWnd') {
      const complete = status === tabs[2];
      return frame(d,{hero:scenery('冥想参悟',m.name,'breath','',stamp),body:heading(m.name,complete?'已入识海':'静心入定')+meter(complete?100:68,100),
        foot:back+(complete ? action('去练功房','open','GengHuanGongFaWnd') : action('完成参悟','finish',m.id,status!==tabs[0]))});
    }
    const facts = [['修途',techniqueKinds[m.kind]],['品阶',m.rankName],['所需境界',realm(m.learningLevel)],['五行',m.element+'行'],['灵衍值',String(Number(m.baseCircle.toFixed(3)))]];
    if (m.signature) facts.push(['气相',m.signature]);
    if (m.id === starter) facts.push(['入门心法','各灵根均可修习']);
    if (status === tabs[2] && m.element !== root) facts.push(['修习记录','此前已悟，可在练功房换回']);
    const detail = m.description ? '<div class="x-system-text">'+art(m.element)+'<p>'+ui.esc(m.description)+'</p></div>' : '';
    const learned = status === tabs[2], equipped = pulse.some(p => p.id === m.id);
    let cta;
    if (shelf.from === 'GengHuanGongFaWnd' && learned) cta = action(equipped?'已装配':'装配此功法','equip',m.id,equipped || m.learningLevel>demoLevel);
    else if (learned) cta = action('去练功房','open','GengHuanGongFaWnd');
    else cta = action(status === tabs[1] ? realm(m.learningLevel)+'可悟' : m.system ? '免费参悟' : '参悟','study',m.id,status !== tabs[0]);
    return frame(d,{moreClass:'x-library x-system',hero:title,body:detail+record(facts),foot:back+cta});
  }
  const specific = {
    DongFuChooseWnd:['居所','residence','', 'DongFuRoomWnd'],YouJianDongFuWnd:['行迹','residence','','DongFuChooseWnd'],
    DongFuRoomWnd:['洞府内室','residence','','DongFuWnd'],DongFuMiShiWnd:['所藏宝箱','residence','journey/chest.png'],
    DongFuYongTuChooseWnd:['择室用途','residence','','DongFuZhiFuWnd'],DongFuZhiFuWnd:['洞府开支','scroll','','DongFuRoomWnd'],
    JuLingFaZhenWnd:['运阵簿','chamber','journey/gather.png','FaZhenXiaoHaoLingShiWnd'],
    KongLingFaZhenWnd:['运阵簿','chamber','journey/control.png','FaZhenXiaoHaoLingShiWnd'],
    GengHuanFaZhenDengJieWnd:['阵法品阶','chamber','journey/gather.png','JuLingFaZhenWnd'],
    FaZhenXiaoHaoLingShiWnd:['灵石账目','chamber','journey/gather.png','DongFuWnd'],
    WuDaoTaiWnd:['选定保留特效','study','journey/technique.png','WuDaoTaiIXiangQingWnd'],
    WuDaoTaiIXiangQingWnd:['悟道推演','study','journey/technique.png','WuDaoTaiWnd']
  };
  function supporting(d,s) {
    const [title,scene,illustration,target]=specific[d.id];
    const canSelect=['choices','rooms','storage'].includes(s.mode)||d.id==='WuDaoTaiWnd';
    const icons=d.id==='DongFuRoomWnd'?['scroll','purse','herb','cauldron','book']:d.id==='DongFuYongTuChooseWnd'?['cauldron','talisman','book','scroll','crystal','sword']:d.id==='WuDaoTaiWnd'?['pendant','crystal','scroll']:d.id==='DongFuMiShiWnd'?['herb','ring','purse']:null;
    const index=state.selected[d.id]??0;
    const roomTargets=['DongFuWnd','DongFuMiShiWnd','DongFuYongTuChooseWnd','DongFuYongTuChooseWnd','DongFuYongTuChooseWnd'];
    const primaryTarget=d.id==='DongFuRoomWnd'?roomTargets[index]:target;
    const lineData=d.id==='YouJianDongFuWnd'?[['所在','临仙郡'],['洞府','有间洞府'],['去向','选择洞府']]:s.rows;
    const condensed=d.id==='DongFuYongTuChooseWnd';
    const body=heading(title,d.id==='YouJianDongFuWnd'?'有间洞府':s.subject)+(canSelect?selectedRows(d.id,lineData,icons?rowIcons(icons):null,true):record(lineData))+
      (state.notice[d.id]?'<p class="x-note" role="status">'+state.notice[d.id]+'</p>':'');
    const primary=d.id==='YouJianDongFuWnd'?'选择洞府':d.id==='DongFuRoomWnd'?(index<2?'进入':'开辟')+s.rows[index][0]:d.id==='DongFuMiShiWnd'?'开启'+s.rows[index][0]:s.action;
    const secondary=d.id.includes('LingFaZhenWnd')?minor('升级法阵','open','GengHuanFaZhenDengJieWnd'):d.id==='DongFuRoomWnd'?minor('选择洞府','open','DongFuChooseWnd'):'';
    return frame(d,{hero:scenery(d.title,s.overline,scene,illustration,s.detail), body,foot:secondary+action(primary,primaryTarget?'open':'chest',primaryTarget||''),moreClass:condensed?'x-dense':''});
  }
  const passes=['探脉','叠雷','破关'];
  const force=[30,38,48];
  let trial=null;
  function freshTrial(randomize) {
    const deck=['火','土','金','木','水'];
    if(randomize) for(let i=deck.length-1;i>0;i--){
      const j=Math.floor(Math.random()*(i+1));
      [deck[i],deck[j]]=[deck[j],deck[i]];
    }
    return {deck:deck.slice(0,3),round:0,guard:66,qi:2,mark:null,feedback:null,log:[]};
  }
  function forecast(t, mark) {
    const e=t.deck[t.round], f=force[t.round];
    let name='护脉', ratio=.8, energy=0;
    if(elements[mark].controls===e){name='截雷'; ratio=.2; energy=-1;}
    else if(elements[e].creates===mark){name='引息'; ratio=.55; energy=1;}
    else if(elements[e].controls===mark){name='逆冲'; ratio=1;}
    const damage=Math.ceil(f*ratio)-(mark===root?3:0);
    return {name, damage, energy};
  }
  function trialLandscape(title, sub, round, element, receipt=false) {
    const phase = (round===undefined?'':('第'+numerals[round]+'劫'));
    return '<div class="x-storm-art '+(receipt?'calm':'')+'"><div class="x-mountains"></div><div class="x-light"></div>'+
      '<div class="x-storm-title"><p>'+sub+'</p><h1>'+title+'</h1></div>'+
      '<div class="x-aura"></div>'+pic('journey/meditator.png','x-sitter')+
      (element?'<div class="x-cloud-mark">'+art(element)+'<span>'+phase+' · '+element+'雷</span></div>':'')+'</div>';
  }
  function trialPanel(d) {
    if(!trial) trial=freshTrial(false);
    const t=trial, intro=d.id==='TuPoDuJieExmpleWnd';
    if(intro){
      return frame(d,{hero:trialLandscape('五行雷关','破境 · 无耗试演',0,t.deck[0]),moreClass:'x-trial',
        body:cultivator+heading('应劫试演','三道天雷')+
        '<ol class="x-passes">'+passes.map((p,i)=>'<li><span>'+numerals[i]+'</span><div><b>'+p+'</b><small>雷威 '+force[i]+'</small></div></li>').join('')+'</ol>'+
        '<p class="x-rule">以克雷之印截雷，耗一真元。承雷所生之印可引息，补一真元。木印合本命，再减三点伤害。</p>',
        foot:action('入阵试演','start')});
    }
    const receipt=d.id==='TuPoDuJieSuccessWnd'||d.id==='TuPoDuJieFailWnd';
    if(receipt){
      const win=d.id==='TuPoDuJieSuccessWnd', actual=t.log.length&&t.feedback&&(t.guard===0||t.round===2)&&((t.guard>0)===win);
      const rows=actual?t.log.map(x=>[passes[x.round]+' · '+x.element+'雷',x.mark+'印'+x.name+'　−'+x.damage]):(win?[['探脉 · 火雷','水印截雷　−6'],['叠雷 · 土雷','金印引息　−21'],['破关 · 金雷','火印截雷　−10']]:[['探脉 · 火雷','金印逆冲　−30'],['叠雷 · 土雷','水印逆冲　−38']]);
      return frame(d,{hero:trialLandscape(win?'劫云已散':'护脉已破','五行雷关 · 应劫回顾',undefined,null,win),moreClass:'x-trial',
        body:cultivator+heading(win?'渡劫成功':'渡劫失败',rows.length+' 道应劫记录')+record(rows)+
          '<div class="x-recap"><span>剩余护脉</span><b>'+(actual?t.guard:win?29:0)+'</b><small>/ 66</small></div>'+
          '<p class="x-note">'+(win?'三雷过后，护脉犹存。':'试试留一真元，在重雷前以相克之印截雷。')+'</p>',
        foot:minor('返回练功房','open','DongFuWnd')+action('再试一局','start')});
    }
    const e=t.deck[t.round], next=t.deck[t.round+1], f=t.mark?forecast(t,t.mark):null;
    const unavailable=f&&f.energy<0&&t.qi<1;
    const options=Object.keys(elements).map(n=>button(art(n)+'<b>'+n+'印</b>','mark',n,n===t.mark?'selected':'','aria-pressed="'+(n===t.mark)+'" '+(t.feedback?'disabled':''))).join('');
    const current=t.feedback||f;
    const cast=t.feedback, text=cast?(t.guard===0||t.round===2?'查看劫录':'迎下一道雷'):'结印应劫';
    let detail='选一枚印，查看护脉与真元的变化。';
    if(current) {
      const delta=cast?current.energyDelta:Math.min(3,t.qi+current.energy)-t.qi;
      detail=(cast?'护脉 −':'预计承伤 ')+current.damage+'　·　真元 '+(delta>0?'＋'+delta:delta<0?'−'+Math.abs(delta):'不变')+(t.mark===root?'　·　本命减伤 3':'');
      if(!cast&&unavailable)detail='真元不足，换一枚印应劫。';
    }
    return frame(d,{hero:trialLandscape(passes[t.round], '五行雷关 · '+ (cast?'雷势已落':'劫云先兆'),t.round,e),moreClass:'x-trial',
      body:'<div class="x-waves">'+passes.map((p,i)=>'<span class="'+(i===t.round?'current':i<t.round?'passed':'')+'"><i></i>'+p+'</span>').join('')+'</div>'+
      '<div class="x-vitals"><div>'+roleSeal+'<span><b>沈青禾</b><small>木灵根 · 木印减伤 3</small></span></div><div><small>护脉</small><b>'+t.guard+'<i>/66</i></b></div><div><small>真元</small><b>'+t.qi+'<i>/3</i></b></div></div>'+meter(t.guard,66)+
      heading(e+'行劫雷','雷威 '+force[t.round])+'<p class="x-next">'+(next?'下道先兆　'+next+'雷 · 雷威 '+force[t.round+1]:'末道雷关　护脉须高于零')+'</p>'+
      '<div class="x-seals" aria-label="应劫五行印">'+options+'</div><div class="x-response" role="status"><span>'+(current?(t.mark+'印 · '+current.name):'尚未结印')+'</span><p>'+detail+'</p></div>',
      foot:action(text,cast?'advance':'cast','',!cast&&(!t.mark||unavailable))});
  }
  const componentArt = {
    CanWuScrollItem:'journey/technique.png',CanWuXiangQingItem:'parts/shui.png',
    ChangShengLuItem:'relics/coin.webp',DongFuBuffItem:'journey/gather.png',DongFuItem:'journey/chest.png',
    GengHuanGongFaItem:'parts/mu.png',KeCanWuItem:'parts/huo.png',YiCanWuItem:'journey/technique.png',TuPo_ShanDian:'parts/thunder1.png'
  };
  window.cultivationComponent=(d,s)=>{
    active=d;
    if(d.id==='JingMaiItem')return window.meridianBoard(d);
    const id=d.id, [name, detail, first, second]=s;
    const states=[first,second], image=componentArt[id], thunder=id==='TuPo_ShanDian';
    const scene=sectionOf(id)===1?'study':sectionOf(id)===2?'scroll':'breath';
    const hero=thunder?trialLandscape('渡劫雷电','五行雷关 · 雷相',0,'火'):scenery(d.title,'修炼与参悟 · 组件',scene);
    const variant = (i,label)=>{
      if(id==='CanWuScrollItem') return '<div class="x-component-tabs"><span class="'+(!i?'active':'')+'">可参悟</span><span class="'+(i?'active':'')+'">待突破</span><span>已参悟</span></div>';
      if(id==='CanWuXiangQingItem'||id==='DongFuBuffItem') return '<div>'+pic(image)+'<span><b>'+name+'</b><small>'+detail+'</small></span><em>'+label+'</em></div>';
      if(thunder)return '<div>'+pic('parts/thunder'+(i+1)+'.png','x-lightning-sample')+'<span><b>'+name+'</b><small>'+label+'</small></span></div>';
      return '<div>'+pic(image)+'<span><b>'+name+'</b><small>'+detail+'</small></span><em>'+label+'</em></div>';
    };
    const body=heading(name)+'<div class="x-samples">'+states.map((label,i)=>
      '<article class="'+(i?'active':'')+'"><header><i></i>'+label+'</header>'+variant(i,label)+'</article>').join('')+'</div>';
    return frame(d,{hero,body,moreClass:thunder?'x-trial':''});
  };
  window.cultivationPage=(d,s)=>{
    active=d;
    if(d.id.startsWith('TuPo')) return trialPanel(d);
    if(d.id==='DongFuWnd'||d.id==='DongFuExmpleWnd')return practice(d);
    if(d.id==='CanWuWnd'||d.id==='GengHuanGongFaWnd')return library(d);
    if(['CanWuGongFaWnd','CanWuZhongWnd','TiShengPingJieWnd'].includes(d.id))return bookDetail(d);
    if(sectionOf(d.id)===2)return window.meridianBoard(d);
    return supporting(d,s);
  };
  function open(id) {
    const key=catalog.find(d=>d.id===id)?.key;
    if(!key)return;
    if(decodeURIComponent(location.hash.slice(1))===key){window.draw(key);return;}
    location.hash=encodeURIComponent(key);
  }
  document.addEventListener('click',event=>{
    const b=event.target.closest('.x-cult button[data-x-action]');
    if(!b||b.disabled||!active)return;
    const {xAction:a,xValue:v}=b.dataset, id=active.id;
    if(a==='open')return open(v);
    if(a==='start'){trial=freshTrial(true); return open('TuPoDuJieWnd');}
    if(a==='tab' && tabs.includes(v)){shelf.tab=v;shelf.page=0;}
    if(a==='page')shelf.page+=Number(v);
    if(a==='rules'||a==='bag'||a==='shelf'){shelf.view=a;shelf.notice='';}
    if(a==='book' && byId.has(v)){
      const status=studyState(byId.get(v));
      if(!status || id==='GengHuanGongFaWnd' && status!==tabs[2]) return;
      shelf.current=v;shelf.from=id;return open('CanWuGongFaWnd');
    }
    if(a==='read'){
      const m=byId.get(v), count=shelf.bag.get(v)||0;
      if(!m || !count || m.system || m.element!==root || shelf.read.has(v) || shelf.learned.has(v)) return;
      shelf.read.add(v);shelf.bag.set(v,count-1);shelf.tab=studyState(m);shelf.page=0;
      shelf.notice='《'+m.name+'》已收录至'+shelf.tab+'。';
    }
    if(a==='study'){
      const m=byId.get(v);
      if(!m || shelf.current!==v || studyState(m)!==tabs[0]) return;
      return open('CanWuZhongWnd');
    }
    if(a==='finish'){
      const m=byId.get(v);
      if(!m || shelf.current!==v || studyState(m)!==tabs[0]) return;
      shelf.learned.add(v);shelf.tab=tabs[2];shelf.page=0;
    }
    if(a==='equip'){
      const m=byId.get(v);
      if(!m || !shelf.learned.has(v) || m.learningLevel>demoLevel) return;
      const slot=3-m.kind;
      pulse[slot]={...pulse[slot],id:m.id,book:m.name,element:m.element,value:demoCycle[slot]*m.baseCircle/.8};
      state.preview=[slot];delete state.notice.DongFuWnd;return open('DongFuWnd');
    }
    if(a==='practice'){
      const i=Number(v), prev=state.preview;
      if(prev.includes(i)&&prev.length>1)state.preview=prev.filter(x=>x!==i);
      if(!prev.includes(i))state.preview=demoLevel<31?[i]:prev.length===2?[prev[0],i]:[...prev,i];
      delete state.notice[id];
    }
    if(a==='run'){state.running=[...state.preview];state.notice[id]='已运转'+state.running.map(i=>pulse[i].name).join('、');}
    if(a==='select'){state.selected[id]=Number(v);delete state.notice[id];}
    if(a==='chest'){state.notice[id]=['灵植匣中存有凝露草、青芝。','器物匣中存有青竹剑、乾元环。','宝箱尚无藏品。'][state.selected[id]??0];}
    if(a==='mark'&&trial&&!trial.feedback&&elements[v])trial.mark=v;
    if(a==='cast'&&trial?.mark&&!trial.feedback){
      const t=trial, f=forecast(t,t.mark), previous=t.qi;
      if(f.energy<0&&t.qi<1)return;
      t.guard=Math.max(0,t.guard-f.damage);t.qi=Math.min(3,t.qi+f.energy);
      t.feedback={...f,energyDelta:t.qi-previous};
      t.log.push({...f,round:t.round,element:t.deck[t.round],mark:t.mark});
    }
    if(a==='advance'&&trial?.feedback){
      if(trial.guard===0||trial.round===2)return open(trial.guard>0?'TuPoDuJieSuccessWnd':'TuPoDuJieFailWnd');
      trial.round++;trial.mark=null;trial.feedback=null;
    }
    window.draw(active.key);
  });
})();
