// 第十一组画板在本机保留一份试玩田簿。正式角色由 Go 服务端结算。
(() => {
  'use strict';
  const key = 'xzj.sigil-garden.v1';
  const herbs = {
    dew: {name:'凝露草',use:'炼凝露丹、青脉丹，亦可凝露调墨',soil:'loam',time:60,price:12,qty:4},
    reed: {name:'霜节芦',use:'抄霜芦符纸，也可炼霜脉清毒丹',soil:'sand',time:300,price:28,qty:4},
    lotus: {name:'梦泽莲',use:'莲汁研浓墨，花瓣作炼丹辅材',soil:'silt',time:900,price:54,qty:5}
  };
  const soils = {loam:['青壤',0],sand:['寒砂土',8],silt:['灵淤泥',14]};
  const effects = {
    ward:['护身','下次进入历练，获得一次护盾。',30,'折锋护身',[[25,35],[75,35],[72,54],[50,68],[29,54]]],
    cleanse:['净秽','当场消去毒性，无毒时留符。',12,'撇捺涤秽',[[31,38],[61,45],[39,55],[66,66]]],
    field:['护田','访客偷采时抵挡一次，灵田最多存三重护持。',1,'围田结界',[[28,36],[72,36],[72,63],[28,63],[28,36]]]
  };
  const material = {reed:'霜节芦',dew:'凝露草',lotus:'梦泽莲',water:'寒露液',ink:'澄露墨',paper:'霜芦符纸',kit:'金翎与云纹木',brush:'金翎符笔',stones:'灵石'};
  const crafts = {
    paper:['抄纸',{reed:3},{paper:3}], ink:['调墨',{dew:2,water:1},{ink:2}],
    lotusInk:['研莲墨',{lotus:1,water:1},{ink:3}], water:['凝露',{dew:3},{water:1}],
    kit:['购入笔料',{stones:90},{kit:1}], brush:['扎制符笔',{kit:1},{brush:1}]
  };
  const visitors = {friend:['林青梧','道友','reed'], stranger:['白石散人','陌生修士','lotus']};
  const blank = () => ({soil:'loam',seed:'',ward:false,wardUsed:false,picked:false,visits:[]});
  function start() {
    const guests={};
    for (const [id,info] of Object.entries(visitors)) guests[id]={...blank(),seed:info[2],at:0,qty:5,ward:id==='stranger'};
    return {inv:{reed:0,dew:0,lotus:0,water:0,ink:0,paper:0,kit:0,brush:0,stones:180},seeds:{dew:2,reed:1,lotus:0},
      plots:Array.from({length:4},blank),guests,starter:false,book:{},goods:[],rank:0,xp:0,realm:0,charges:0,
      poison:36,shield:0,logs:[],offset:0,day:-1,attempts:0};
  }
  function readMain() {
    try {const value=JSON.parse(localStorage.getItem(key)); if (value?.guests && value?.inv && Array.isArray(value.plots)) return value;} catch {}
    return start();
  }
  let s,preview=false,receipt=false,tab,effect,strong,step,waypoint,drag,feedback,plot,seed,soil,scope,lastBoard='';
  const now=()=>Math.floor(Date.now()/1000)+s.offset;
  const save=()=>{ if (!preview) try {localStorage.setItem(key,JSON.stringify(s));} catch {} };
  const formatTime=n=>n<60?n+'秒':Math.floor(n/60)+'分'+(n%60?(n%60)+'秒':'');
  const active=()=>s.plots[plot], cost=()=>({paper:1,ink:strong?2:1}), ref=()=>effect+(strong?'.strong':'.simple');
  const sheet=()=>[['点锋引气',[[32,22],[48,12],[65,23]]],[effects[effect][3],effects[effect][4]],['横画平收',[[29,78],[73,78]]],
    ...(strong?[['回钩重封',[[53,71],[53,90],[65,84]]]]:[])];
  const owned=parts=>Object.entries(parts).every(([k,v])=>s.inv[k]>=v);
  const add=(parts,sign=1)=>Object.entries(parts).forEach(([k,v])=>s.inv[k]+=sign*v);
  const partsText=parts=>Object.entries(parts).map(([k,v])=>material[k]+' ×'+v).join('、');
  const power=()=>effect==='field'?1+Number(strong)+Number(s.rank>=3&&s.inv.brush>0):
    Math.round(effects[effect][2]*Math.floor((100+s.realm*5+s.rank*3+(s.inv.brush?15:0))*(strong?1.5:1))/100);
  const labelPower=(e,n)=>e==='field'?'护田 '+n+' 重':e==='ward'?'入战护盾 '+n+' 点':'消去毒性 '+n+' 点';
  const curName=()=>effects[effect][0]+'符'+(strong?'·重封':'');
  const art='assets/artisan11/';
  const image=(name,part=false)=>'<img alt="" src="assets/'+(part?'parts/':'relics/')+name+(part?'.png':'.webp')+'">';
  const herbArt=(key)=>'<i class="herb-art herb-'+key+'" aria-hidden="true"></i>';
  const btn=(text,action,cls='soft',allow=true)=>'<button type="button" data-sg="'+action+'" class="'+cls+'"'+(allow?'':' disabled')+'>'+text+'</button>';
  const chip=(label,action,selected)=>btn(label,action,'chip'+(selected?' on':''));
  const group=(title,content,cls='')=>'<section class="panel '+cls+'"><h2>'+title+'</h2>'+content+'</section>';
  const metrics=pairs=>'<div class="metrics">'+pairs.map(([k,v])=>'<span>'+k+'<b>'+v+'</b></span>').join('')+'</div>';
  const relicIcon={scroll:0,liquid:1,rune:2,talisman:3};
  const line=(icon,title,desc,action,part=false)=>'<div class="ledger"><i class="token '+(icon in relicIcon?'relic':'')+'" style="--relic:'+(relicIcon[icon]||0)+'">'+image(icon,part)+'</i><div><h3>'+title+'</h3><p>'+desc+'</p></div>'+action+'</div>';
  const labels={ward:'御',cleanse:'净',field:'守'}, notices={ward:'历练护盾',cleanse:'消除毒性',field:'灵田护持'};
  const hero=(title,subtitle,garden=false)=>'<div class="hero'+(garden?' grove':'')+'"><img class="scene" src="'+art+(garden?'garden-hall':'sigil-hall')+'.jpg" alt=""><div class="scene-shade"></div><div class="engraving"></div><div class="motes" aria-hidden="true">'+[0,1,2,3,4,5,6,7].map(n=>'<i style="--n:'+n+'"></i>').join('')+'</div><div class="hero-type"><small class="eyebrow"><i></i>'+(garden?'山居 · 百草圃':'洞府 · 神符会')+'</small><h1>'+title+'</h1><p>'+subtitle+'</p></div></div>';
  function studio() {
    const pattern=sheet(), writing=step>=0;
    const realmNames=['练气','筑基','结丹','元婴','分神','炼虚','合体','渡劫','大乘'];
    const profile='<details><summary>'+realmNames[s.realm]+'境 · '+(s.rank?'符师'+s.rank+'品':'符道初学')+' · '+(s.inv.brush?'金翎符笔':'云木符笔')+'</summary><p>每大境符力加 5%，每品再加 3%。金翎笔提升符力与描线容差。</p><label>试览境界 <select data-realm>'+realmNames.map((n,i)=>'<option value="'+i+'"'+(i===s.realm?' selected':'')+'>'+n+'</option>').join('')+'</select></label></details>';
    const pouch=()=>group('随身符匣',s.goods.map((g,i)=>line('talisman',g.name+' ×'+g.n,labelPower(g.effect,g.power),btn('用符','use/'+i))).join('')+
      metrics([['毒性',s.poison],['入战护盾',s.shield],['护田',s.charges+' 重']])+'<p class="sub character-balance">试用角色 · 毒性 '+s.poison+'　入战护盾 '+s.shield+'　护田 '+s.charges+' 重</p>');
    if (receipt) return hero('符书已成',curName()+' · '+labelPower(effect,power()))+group('已留符谱','<div class="receipt-slip"><i class="receipt-art" aria-hidden="true"></i><div><h3>'+curName()+'</h3><p>'+labelPower(effect,power())+' · 可按谱复写</p></div></div><p>复写此式每张仍需'+partsText(cost())+'。</p>'+btn('返回符案','tab/符案','action'))+pouch();
    let top=hero(writing?'落笔成符':lastBoard==='SFH_CreateChooseWnd'?'自拟符式':'山房符案',writing?curName()+' · '+labelPower(effect,power()):'符意定笔势，收笔定符力。');
    if (!writing) top+=group('择符意','<div class="effect-grid">'+Object.keys(effects).map(k=>btn('<i aria-hidden="true">'+labels[k]+'</i><b>'+effects[k][0]+'</b><small>'+notices[k]+'</small>','effect/'+k,'effect-card '+(k===effect?'on':''))).join('')+'</div><p class="effect-detail">'+effects[effect][1]+'</p><div class="chips"><span class="minor">收笔</span>'+chip('平收 · 三笔','seal/simple',!strong)+chip('重封 · 四笔','seal/strong',strong)+'</div>'+profile);
    const caption=writing?'第 '+(step+1)+' / '+pattern.length+' 笔　'+pattern[step][0]:curName();
    top+=group(caption,'<div class="writing-table"><div class="sheet"><span class="paper-seal" aria-hidden="true">'+labels[effect]+'</span><canvas width="500" height="500" aria-label="'+caption+'，可用下方辅笔按钮逐笔书写"></canvas></div></div>'+
      (writing?'<div class="stitch" aria-label="书写进度">'+pattern.map((_,i)=>'<i class="'+(i<step?'done':i===step?'current':'')+'"></i>').join('')+'</div><p>从朱点起笔，沿淡纹依次经过墨点。可以松手重来。</p><div class="pair">'+btn('停笔','cancel')+btn('辅笔写这一笔','assist','action')+'</div>':
      metrics([['用料','纸 1 · 墨 '+cost().ink],['纳戒','纸 '+s.inv.paper+' · 墨 '+s.inv.ink],['符效',labelPower(effect,power())]])+
      '<div class="pair">'+btn('按谱复写','copy','soft',owned(cost())&&s.book[ref()])+btn(owned(cost())?'铺纸落笔':'去备纸墨',owned(cost())?'begin':'tab/纸墨','action')+'</div>'),'drawing');
    if (!writing && s.goods.length) top+=pouch();
    return top;
  }
  function materials() {
    const icons={paper:'scroll',ink:'liquid',lotusInk:'leaf',water:'liquid',kit:'coin',brush:'rune'};
    return hero(preview && lastBoard==='SFH_ShenFuHuiWnd'?'神符会 · 纸墨坊':'纸墨作坊','抄一纸霜芦，研一砚澄露。')+
      group('案头用料',metrics([['灵石',s.inv.stones],['成纸',s.inv.paper+' 张'],['成墨',s.inv.ink+' 份']])+
        '<div class="stock">'+['reed','dew','lotus','water','kit'].map(k=>'<span>'+material[k]+'<b>'+s.inv[k]+'</b></span>').join('')+'</div><div class="pair">'+btn('去灵田种植','tab/灵田')+
        btn(s.starter?'36 灵石补料':'领取入门原料','supply','action',!s.starter||s.inv.stones>=36)+'</div><p class="sub">一包有霜节芦 6 株、凝露草 4 株和寒露液 2 份。</p>')+
      group('加工',Object.entries(crafts).map(([k,[name,input,out]])=>line(icons[k],name,partsText(input)+' → '+partsText(out),btn('制作','craft/'+k,'soft',owned(input)),true)).join(''));
  }
  function stage(p) {return !p.seed?'空畦':p.at<=now()?'成熟':'生长中';}
  function daily() {
    const day=Math.floor((now()+28800)/86400); if(day!==s.day){s.day=day;s.attempts=0;}
    return 3-s.attempts;
  }
  const cropFeature=(key,title,note)=>'<div class="crop-feature">'+herbArt(key)+'<div><h3>'+title+'</h3><p>'+note+'</p></div></div>';
  function beds() {
    const p=active(), kind=herbs[seed], matched=kind.soil===soil, fee=p.soil===soil?0:soils[soil][1];
    const title=preview && tab==='灵田' ? ({LT_ZhongZiWnd:'育种室',LT_ShoHuoWnd:'成熟采收',LT_TuRangGenHuanWnd:'更换土壤',LT_LingTuWnd:'灵土调配',LZ_ChuangZaoWnd:'培育灵植'}[lastBoard]||'溪畔灵田'):'溪畔灵田';
    const out=hero(title,'选种、择土，收成可入药、制符或留种。',true);
    const plots=group('我的田畦','<div class="beds">'+s.plots.map((v,i)=>'<button data-sg="plot/'+i+'" class="bed '+(plot===i?'on ':'')+(v.seed?'planted':'empty')+(v.ward?' warded':'')+'">'+(v.seed?herbArt(v.seed):'<i class="furrow" aria-hidden="true"></i>')+'<span><small>第 '+(i+1)+' 畦</small><b>'+(v.seed?herbs[v.seed].name:soils[v.soil][0])+'</b></span><em>'+stage(v)+'</em></button>').join('')+'</div>');
    let detail;
    if (!p.seed) {
      detail=group('播种 · 第 '+(plot+1)+' 畦','<div class="seed-grid">'+Object.entries(herbs).map(([k,v])=>btn(herbArt(k)+'<span><b>'+v.name+'</b><small>种 ×'+s.seeds[k]+'</small></span>','seed/'+k,'seed-card '+(k===seed?'on':''))).join('')+'</div><p class="effect-detail">'+kind.use+'。</p><div class="chips soil"><span class="minor">土壤</span>'+Object.entries(soils).map(([k,v])=>chip(v[0],'soil/'+k,k===soil)).join('')+'</div>'+
        '<p class="sub">适土为'+soils[kind.soil][0]+'，'+(matched?'当前适土，成熟快两成，多收一株。':'当前土壤按基础时长与收成结算。')+'</p>'+
        metrics([['成熟',formatTime(kind.time*(matched?.8:1))],['产出',kind.qty+Number(matched)+' 株'],['换土',fee+' 灵石']])+
        '<div class="pair">'+btn('购种 · '+kind.price+' 灵石','buy','soft',s.inv.stones>=kind.price)+btn('播种一粒','plant','action',s.seeds[seed]>0&&s.inv.stones>=fee)+'</div>');
    } else {
      detail=group(herbs[p.seed].name+' · 第 '+(plot+1)+' 畦',cropFeature(p.seed,p.at<=now()?'可以采收':'正在生长',herbs[p.seed].use+'。')+
        metrics([['可收',p.qty+' 株'],['成熟',p.at<=now()?'可采收':'<span data-clock>'+formatTime(Math.max(0,p.at-now()))+'</span>'],['防守',p.ward?'有阵':s.charges+' 重符护']])+
        (p.at>now()?btn('试玩 · 等至成熟','ripen'):'<div class="pair">'+btn('两株留一粒种','harvest/keep','soft',p.qty>2)+btn('收获入纳戒','harvest/all','action')+'</div>')+
        '<div class="chips">'+btn('布草木阵 · 6 灵石','ward','soft',s.inv.stones>=6&&!p.picked&&!p.wardUsed&&!p.ward)+
        btn('试玩 · 邀人偷采','raid','soft',p.at<=now()&&!p.picked&&p.visits.length<5)+'</div>');
    }
    return out+(preview?detail+plots:plots+detail)+group('田簿','<p>灵石 '+s.inv.stones+'　全田符护 '+s.charges+' 重</p>'+s.logs.slice(-3).reverse().map(t=>'<p>'+t+'</p>').join(''));
  }
  function visiting() {
    const v=visitors[scope],p=s.guests[scope];
    return hero(preview && lastBoard==='LZ_MianWnd'?'灵植协会 · 访田':'山间访田','道友与陌生修士都能来访。',true)+group('访客名册','<div class="chips">'+chip('我的道友','scope/friend',scope==='friend')+chip('陌生修士','scope/stranger',scope==='stranger')+'</div>'+
      cropFeature(p.seed,v[0],v[1]+' · '+herbs[p.seed].name+'已熟')+metrics([['今日余次',daily()],['地块阵',p.ward?'已布阵':'无'],['待收',p.qty+' 株']])+
      '<p>每日可尝试三次，同一访客每茬限一次。每茬最多失一株，地块阵先于护田符生效。</p><div class="pair">'+btn('回我的灵田','tab/灵田')+btn('偷采一株','steal','action',daily()>0&&!p.picked&&!p.visits.includes('me'))+'</div>')+
      group('往来记录',s.logs.length?s.logs.slice(-6).reverse().map(t=>'<p>'+t+'</p>').join(''):'<p>尚无访田记录。</p>');
  }
  const talBoards=new Set(['HuaFuChooseWnd','HuaFuWnd','SFH_CreateChooseWnd','SFH_CreateWnd','SFH_CreateResultWnd','SFH_ShenFuHuiWnd']);
  const fieldBoards=new Set(['LT_LingTianMainWnd','LT_ZhongZiWnd','LT_ShoHuoWnd','LT_TuRangGenHuanWnd','LT_LingTuWnd','LZ_MianWnd','LZ_ChuangZaoWnd']);
  // Detail links are repeatable examples. The two main entries share the player's saved desk and farm.
  function enterBoard(id) {
    lastBoard=id; preview=id!=='HuaFuChooseWnd'&&id!=='LT_LingTianMainWnd'; s=preview?start():readMain();
    tab=fieldBoards.has(id)?'灵田':'符案'; receipt=false; effect='ward'; strong=false; step=-1; waypoint=0; drag=null;
    feedback=''; plot=0; seed='dew'; soil=s.plots[0].soil; scope='friend';
    if (!preview) return;
    Object.assign(s.inv,{reed:6,dew:4,water:2,paper:3,ink:4});
    switch(id) {
      case 'HuaFuWnd': step=0; break;
      case 'SFH_CreateChooseWnd': effect='field'; strong=true; break;
      case 'SFH_CreateWnd': effect='field'; strong=true; step=1; break;
      case 'SFH_CreateResultWnd':
        effect='field';strong=true;receipt=true;s.book[ref()]=true;s.rank=1;s.xp=10;
        s.goods.push({name:curName(),effect,power:power(),n:1});break;
      case 'SFH_ShenFuHuiWnd': tab='纸墨'; break;
      case 'LT_ZhongZiWnd': seed='lotus';break;
      case 'LT_TuRangGenHuanWnd': seed='reed';soil='sand';break;
      case 'LT_LingTuWnd': seed='lotus';soil='silt';s.seeds.lotus=1;break;
      case 'LT_ShoHuoWnd': s.plots[0]={...blank(),seed:'reed',soil:'sand',at:now()-1,qty:5,ward:true};break;
      case 'LZ_MianWnd': tab='访田';scope='stranger';break;
      case 'LZ_ChuangZaoWnd': s.plots[0]={...blank(),seed:'lotus',soil:'silt',at:now()+720,qty:6};break;
    }
  }
  window.sigilGardenDesign = (d,ui) => {
    if (!talBoards.has(d.id)&&!fieldBoards.has(d.id)) {
      lastBoard='';
      if (d.module!=='Crafting' || d.kind!=='window' || !/^(HuaFu|SFH_|LZ_|LT_)/.test(d.id)) return;
      const garden=/^(LT_|LZ_)/.test(d.id);
      const markup=window.workshopDesign(d,ui);
      if (!markup) return;
      const back='<a class="archive-back" href="画板.html#'+(garden?'LT_LingTianMainWnd':'HuaFuChooseWnd')+'">‹　'+(garden?'我的灵田':'我的符案')+'</a>';
      return markup.replace('class="ui-page ', 'class="ui-page sg-archive '+(garden?'verdant ':'lacquer ')).replace(/<span class="ui-back">[\s\S]*?返回<\/span>/,back);
    }
    if (d.id!==lastBoard) enterBoard(d.id);
    const view={'符案':studio,'纸墨':materials,'灵田':beds,'访田':visiting};
    const garden=tab==='灵田'||tab==='访田';
    const banner=preview?'示例 · '+d.title:'修真界　／　洞府百艺';
    const link=preview?'<a href="11 制符与灵植.html">我的田簿 ↗</a>':'<a href="设计总览.html">设计总览 ↗</a>';
    const icons={'符案':'rune','纸墨':'scroll','灵田':'leaf','访田':'map'};
    requestAnimationFrame(paint);
    return '<div class="sg '+(garden?'garden':'sigil')+'" data-step="'+(step>=0?'writing':receipt?'receipt':'browse')+'" data-page="'+tab+'"><header><span><i class="brand-mark" aria-hidden="true"></i>'+banner+'</span>'+link+'</header><article>'+view[tab]()+'</article>'+
      '<div class="feedback" role="status">'+(feedback||(preview?'示例田簿 · 可直接试玩':'本地试玩田簿'))+'</div><nav aria-label="洞府百艺">'+Object.keys(view).map(n=>btn(image(icons[n],true)+'<span>'+n+'</span>','tab/'+n,tab===n?'active':'')).join('')+'</nav></div>';
  };
  function redraw(message='',toTop=false) {
    const scroll=document.querySelector('.sg article')?.scrollTop||0;
    feedback=message; save();window.draw(lastBoard);
    if (!toTop) document.querySelector('.sg article').scrollTop=scroll;
    if (message && /已成|已收入符匣|护田符|草木阵|已布|灵植已熟|已入纳戒/.test(message)) {
      const pulse=document.createElement('i');
      pulse.className='flash'+(/护田符|草木阵/.test(message)?' ward-fx':'');
      pulse.setAttribute('aria-hidden','true');document.querySelector('.sg').append(pulse);
      setTimeout(()=>pulse.remove(),960);
    }
  }
  function finish(copy) {
    if (!owned(cost()) || (copy&&!s.book[ref()])) return redraw('纸墨或符谱尚缺。');
    const value=power(), name=curName();add(cost(),-1);
    const prev=s.goods.find(x=>x.effect===effect&&x.power===value&&x.name===name);
    if(prev) prev.n++; else s.goods.push({name,effect,power:value,n:1});
    if(!copy) {
      s.book[ref()]=true; s.rank=Math.max(1,s.rank);s.xp+=10;
      const thresholds=[100,100,300,800,1600,3000,6250,10000,20000,40000];
      if(s.rank<9&&s.xp>=thresholds[s.rank]) {s.xp-=thresholds[s.rank];s.rank++;}
    }
    step=-1;waypoint=0;drag=null;redraw(name+'已收入符匣。'+(copy?'':'此谱已留，可直接复写。'));
  }
  function nextStroke() {drag=null;waypoint=0;if(++step===sheet().length) finish(false);else redraw('此笔已成。');}
  function use(i) {
    const g=s.goods[i]; if(!g)return; const v=g.power;
    if(g.effect==='ward') {if(s.shield>=v)return redraw('已有同等或更强的入战护盾，此符保留。');s.shield=v;}
    if(g.effect==='cleanse') {if(!s.poison)return redraw('身上无毒，此符保留。');s.poison=Math.max(0,s.poison-v);}
    if(g.effect==='field') {if(s.charges===3)return redraw('三重符护已满，此符保留。');s.charges=Math.min(3,s.charges+v);}
    if(--g.n===0)s.goods.splice(i,1);redraw(g.name+'已用。'+labelPower(g.effect,g.effect==='ward'?s.shield:g.effect==='field'?s.charges:v));
  }
  function take(p,me) {
    if (p.picked||!p.seed||p.at>now())return;
    if(me && (daily()===0||p.visits.includes('me')))return;
    const actor=me?'me':'demo'+p.visits.length; p.visits.push(actor); if(me)s.attempts++;
    let result;
    if(p.ward){p.ward=false;p.wardUsed=true;result='触发草木阵，未取走作物';}
    else if(!me&&s.charges){s.charges--;result='消耗一重护田符，作物完好';}
    else {p.picked=true;p.qty--;if(me)s.inv[p.seed]++;result='偷采'+herbs[p.seed].name+'一株';}
    const msg=(me?'你拜访'+visitors[scope][0]:'游方修士来访')+'，'+result+'。';s.logs.push(msg);redraw(msg);
  }
  document.addEventListener('change',e=>{if(e.target.matches('.sg [data-realm]')) {s.realm=Number(e.target.value);redraw('已按所选境界估算符效。');}});
  document.addEventListener('click',e=>{
    const b=e.target.closest('.sg [data-sg]');if(!b||b.disabled)return;
    const [a,id]=b.dataset.sg.split('/'),p=active(),h=herbs[seed];
    switch(a){
      case 'tab':tab=id;step=-1;drag=null;receipt=false;redraw('',true);break;
      case 'effect':effect=id;redraw();break;
      case 'seal':strong=id==='strong';redraw();break;
      case 'begin':if(owned(cost())){step=0;waypoint=0;redraw('照淡纹描写，或点辅笔。',true);}break;
      case 'cancel':step=-1;drag=null;redraw('已停笔，纸墨仍在案头。');break;
      case 'copy':finish(true);break;
      case 'assist':if(step>=0)nextStroke();break;
      case 'use':use(Number(id));break;
      case 'supply':if(s.starter&&s.inv.stones<36)return;if(s.starter)s.inv.stones-=36;add({reed:6,dew:4,water:2});s.starter=true;redraw('已收基础原料，可抄纸、调墨。');break;
      case 'craft':{const [name,input,out]=crafts[id];if(owned(input)){add(input,-1);add(out);redraw(name+'完成，得到'+partsText(out)+'。');}break;}
      case 'plot':plot=Number(id);soil=active().soil;redraw();break;
      case 'seed':seed=id;redraw();break;
      case 'soil':soil=id;redraw();break;
      case 'buy':if(s.inv.stones>=h.price){s.inv.stones-=h.price;s.seeds[seed]++;redraw('购得'+h.name+'种一粒。');}break;
      case 'plant':{const fee=p.soil===soil?0:soils[soil][1],match=soil===h.soil;if(p.seed||s.inv.stones<fee||!s.seeds[seed])return;
        s.seeds[seed]--;s.inv.stones-=fee;s.plots[plot]={...blank(),soil,seed,at:now()+h.time*(match?.8:1),qty:h.qty+Number(match)};redraw('已播种'+h.name+'。');break;}
      case 'ripen':s.offset+=Math.max(0,p.at-now());redraw('试玩时间已推至此茬成熟。');break;
      case 'harvest':{if(!p.seed||p.at>now())return;const keep=id==='keep';if(keep&&p.qty<=2)return;
        const count=p.qty-(keep?2:0);s.inv[p.seed]+=count;if(keep)s.seeds[p.seed]++;s.plots[plot]={...blank(),soil:p.soil};
        redraw(herbs[p.seed].name+' ×'+count+' 已入纳戒。'+(keep?'另留种一粒。':''));break;}
      case 'ward':if(p.seed&&s.inv.stones>=6&&!p.wardUsed&&!p.ward&&!p.picked){s.inv.stones-=6;p.ward=true;redraw('草木阵守护本茬，可抵一次偷采。');}break;
      case 'raid':take(p,false);break;
      case 'scope':scope=id;redraw();break;
      case 'steal':take(s.guests[scope],true);break;
    }
  });
  function paint() {
    const canvas=document.querySelector('.sg canvas');if(!canvas)return;
    const ctx=canvas.getContext('2d'),pattern=sheet();ctx.setTransform(5,0,0,5,0,0);ctx.clearRect(0,0,100,100);
    ctx.strokeStyle='#937e5a44';ctx.lineWidth=.35;ctx.setLineDash([1.6,2]);
    for(const [a,b,c,d] of [[50,5,50,95],[5,50,95,50],[7,7,93,93],[7,93,93,7]]){ctx.beginPath();ctx.moveTo(a,b);ctx.lineTo(c,d);ctx.stroke();}
    ctx.setLineDash([]);ctx.lineJoin=ctx.lineCap='round';
    pattern.forEach(([,pts],i)=>{
      ctx.lineWidth=step<0||i<step?2.7:1.6;ctx.strokeStyle=step<0||i<step?'#753d2b':i===step?'#a27146':'#c7ac72';
      if(step<0||i<step) {
        const ink=ctx.createLinearGradient(26,18,72,80);ink.addColorStop(0,'#a64827');ink.addColorStop(.35,'#783321');ink.addColorStop(.72,'#643421');ink.addColorStop(1,'#973f26');ctx.fillStyle=ink;
        for(let j=1;j<pts.length;j++){
          const [x,y]=pts[j-1],[u,v]=pts[j],len=Math.hypot(u-x,v-y),dx=(v-y)/len,dy=(x-u)/len;
          const wa=j===1?.45:1.13,wb=j===pts.length-1?.67:1.32;
          ctx.beginPath();ctx.moveTo(x-dx*wa,y-dy*wa);ctx.quadraticCurveTo((x+u)/2-dx*1.8,(y+v)/2-dy*1.8,u-dx*wb,v-dy*wb);
          ctx.lineTo(u+dx*wb,v+dy*wb);ctx.quadraticCurveTo((x+u)/2+dx*1.85,(y+v)/2+dy*1.85,x+dx*wa,y+dy*wa);ctx.closePath();ctx.fill();
        }
      } else {
        ctx.beginPath();pts.forEach(([x,y],j)=>j?ctx.lineTo(x,y):ctx.moveTo(x,y));ctx.stroke();
      }
      if(i===step && waypoint>0) {
        ctx.save();ctx.strokeStyle='#9e4126';ctx.shadowColor='#ffce49';ctx.shadowBlur=8;ctx.lineWidth=2.6;
        ctx.beginPath();pts.slice(0,waypoint).forEach(([x,y],j)=>j?ctx.lineTo(x,y):ctx.moveTo(x,y));ctx.stroke();ctx.restore();
      }
      if(i===step) pts.forEach(([x,y],j)=>{ctx.beginPath();ctx.fillStyle=j<waypoint?'#375a45':j===waypoint?'#a02f22':'#786848';ctx.arc(x,y,j===waypoint?2.6:1.5,0,Math.PI*2);ctx.fill();});
    });
  }
  let trail=[];
  function inkTrace(canvas, x, y) {
    trail.push([x,y]);
    if(trail.length>10)trail.shift();
    paint();
    if(trail.length<2) return;
    const ctx=canvas.getContext('2d');ctx.save();ctx.lineCap='round';ctx.lineJoin='round';
    for(let i=1;i<trail.length;i++){
      const [u,v]=trail[i-1],[tx,ty]=trail[i];ctx.beginPath();ctx.moveTo(u,v);ctx.lineTo(tx,ty);
      ctx.strokeStyle='rgba(138,48,28,'+(.25+.65*i/trail.length)+')';ctx.lineWidth=.8+i/trail.length;ctx.stroke();
      if(!matchMedia('(prefers-reduced-motion: reduce)').matches){
        ctx.strokeStyle='rgba(255,202,106,'+(.68*i/trail.length)+')';ctx.lineWidth=.23;ctx.stroke();
      }
    }
    ctx.restore();
  }
  function trace(e) {
    if(drag!==e.pointerId||step<0)return;
    const canvas=e.target,rect=canvas.getBoundingClientRect(), x=(e.clientX-rect.left)*100/rect.width,y=(e.clientY-rect.top)*100/rect.height;
    inkTrace(canvas,x,y);
    const pts=sheet()[step][1],q=pts[waypoint];
    if(Math.hypot(x-q[0],y-q[1])<=10+Math.floor(s.rank/2)+(s.inv.brush?6:0)){
      waypoint++; if(waypoint===pts.length){nextStroke();return;}paint();
    }
  }
  document.addEventListener('pointerdown',e=>{if(!e.target.matches('.sg canvas')||step<0||drag!==null)return;e.target.setPointerCapture(e.pointerId);drag=e.pointerId;trail=[];waypoint=0;trace(e);});
  document.addEventListener('pointermove',e=>{if(e.target.matches('.sg canvas'))trace(e);});
  function release(e){if(drag!==e.pointerId)return;drag=null;trail=[];waypoint=0;paint();const status=document.querySelector('.sg .feedback');if(status)status.textContent='可重描本笔，先前完成的笔画已保留。';}
  document.addEventListener('pointerup',release);document.addEventListener('pointercancel',release);
  setInterval(()=>{const target=document.querySelector('.sg [data-clock]');if(!target)return;const n=active().at-now();if(n<=0)redraw('灵植已熟，可以采收。');else target.textContent=formatTime(n);},1000);
})();
