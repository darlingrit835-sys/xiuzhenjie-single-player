/* 09 地火阁互动样稿。丹方、药材与五行来自 cultivation_affinities。 */
(() => {
  "use strict";
  const {materials, book, elements, flame, flameElement, ranks: training, bands, relations, limits, realms} = window.alchemyBoardCatalog;
  const stages = ['温炉','合药','凝丹'];
  const verbs = ['点火','合药','收丹'];
  const tips = ['铜针入金弧时点火。','灵针逆转，入弧时将药液合流。','金弧收窄，在弧心收丹。'];
  const names = ['','金','木','水','火','土'];
  const ranks = ['未入品','一品','二品','三品','四品','五品','六品','七品','八品','九品'];
  const grades = ['','下品','中品','上品'];
  const key = 'xzj.alchemy.board.v1';
  const group = new Set(['DiHuoGeWnd','DiHuoGeCaiLiaoChooseWnd','DiHuoGeLianZhiBuZhouSheZhiWnd','DiHuoGeLianZhiWnd','DiHuoGeLianDanJieSuanWnd']);
  const clamp = (n,min,max) => Math.min(max,Math.max(min,n));
  const percent = bps => Number((bps/100).toFixed(2))+'%';
  const badge = id => '<i class="da-element da-e'+materials[id].element+'">'+names[materials[id].element]+'</i>';
  const matName = id => materials[id].name;
  function relation(source, target) {
    const kind=source===target?'same':elements[source].generates===target?'generates':elements[target].generates===source?'generatedBy':elements[source].overcomes===target?'overcomes':'overcomeBy';
    const values=relations[kind];
    const [score,name]={same:[60,'同气'],generates:[100,'生主药'],generatedBy:[35,'主药生辅'],overcomes:[-65,'克主药'],overcomeBy:[-90,'主药克辅']}[kind];
    return [score,name,values.successBps,values.yieldBps,values.potencyBps];
  }
  function initial() {
    return {rank:4,stock:Object.fromEntries(Object.keys(materials).map(id=>[id,8])),memory:{},memoryMix:{},mastery:{},products:[]};
  }
  function read() {
    try { const data=JSON.parse(localStorage.getItem(key)); if(data && data.stock && data.memory && data.mastery && Array.isArray(data.products)) return data; } catch {}
    return initial();
  }
  let player=read(), id='dan.ninglu', aux=['mat.cold_dew'], page='prep', dialog='', guide='', taps=[], steps=[], stage=0, elapsedAt=0, frame=0, result=null, feedback='', lastTap=-Infinity;
  const recipe=()=>book[id], mastery=()=>player.mastery[id]||0, memory=()=>player.memory[id]||0;
  const stock=m=>player.stock[m]||0;
  const selected=m=>aux.filter(v=>v===m).length;
  function save() { try { localStorage.setItem(key,JSON.stringify(player)); } catch {} }
  function affinity() {
    const el=materials[recipe().main].element;
    return aux.length ? Math.trunc(aux.reduce((n,m)=>n+relation(materials[m].element,el)[0],0)/aux.length) : 0;
  }
  function flameModifier(column) {
    const modifier=m=>relation(flameElement,materials[m].element)[column];
    const main=modifier(recipe().main);
    return aux.length ? Math.trunc((limits.primaryWeightBps*main+Math.trunc((10000-limits.primaryWeightBps)*aux.reduce((n,m)=>n+modifier(m),0)/aux.length))/10000) : main;
  }
  function chance(score) {
    const r=recipe(), factor=flameModifier(2);
    return clamp(r.base+flame.successBps+training[player.rank].successBps+factor+mastery()*5+affinity()*8+(score-60)*32,500,limits.successMaxBps);
  }
  function makeSteps() {
    const ease=player.rank*45+mastery()*4+affinity()*2-recipe().rank*115;
    return stages.map((name,i)=>({period:clamp(2400-260*i+ease,850,3800),
      width:clamp(1550-235*i+ease,450,2300), target:1800+Math.floor(Math.random()*6800),direction:i===1?-1:1}));
  }
  function effortGrade(score) {
    const q=score+player.rank*2+Math.trunc(mastery()/10)+Math.trunc(affinity()/12);
    return q>=90?3:q>=65?2:1;
  }
  function potency(grade, imprinted=false) {
    const [lo,hi]=bands[grade];
    const mod=(imprinted && player.memoryTraining?.[id]!=null) ? player.memoryTraining[id] : training[player.rank].potencyBps+flame.potencyBps+flameModifier(4);
    const pos=clamp(45+Math.trunc(affinity()/2)+aux.length*3+Math.trunc(mod/120),0,100);
    return Math.max(1,Math.round(recipe().strength*(lo+Math.trunc((hi-lo)*pos/100))/10000));
  }
  function available() {
    return player.rank>=recipe().rank && stock(recipe().main)>=recipe().mainQty && aux.every(m=>stock(m)>=selected(m));
  }
  const pctLabel = n => n>0?'+'+n:String(n);
  function assessment() {
    const a=affinity();
    return a>0?'相济':a<0?'相冲':'平和';
  }
  function redraw() {
    window.draw('DiHuoGeWnd');
    if(page==='run') animate();
  }
  function fillStock() {
    Object.keys(materials).forEach(m=>player.stock[m]=stock(m)+6);
    feedback='示例药匣已补充，各添六份。'; save();
  }
  function settle(replay=false) {
    if (!available() || (replay && !memory())) return;
    const score=replay?0:Math.round(taps.reduce((n,s,i)=>n+s*[.25,.35,.4][i],0));
    const bps=replay?10000:chance(score);
    const brewed=replay || Math.random()*10000<bps;
    const grade=brewed?(replay?memory():effortGrade(score)):0;
    const r=recipe(), baseQty=r.minQty+Math.floor(Math.random()*(r.maxQty-r.minQty+1));
    const adjusted=baseQty*clamp(10000+training[player.rank].yieldBps+flame.yieldBps+flameModifier(3),limits.yieldMultiplierMinBps,limits.yieldMultiplierMaxBps);
    const qty=brewed?clamp(Math.floor(adjusted/10000)+(Math.random()*10000<adjusted%10000?1:0),1,r.cap):0;
    player.stock[r.main]-=r.mainQty;
    aux.forEach(m=>player.stock[m]--);
    result={id,score,bps,brewed,grade,replay,qty,basis:r.basis,effect:brewed?potency(grade,replay):0};
    if (brewed) {
      const batch=player.products.find(v=>v.id===id && v.grade===grade && (v.basis||'flat')===r.basis && v.effect===result.effect);
      if(batch) batch.qty+=qty; else player.products.push({id,grade,basis:r.basis,effect:result.effect,qty});
      if(!replay && grade>memory()) {player.memory[id]=grade;(player.memoryMix||={})[id]=aux.slice();(player.memoryTraining||={})[id]=training[player.rank].potencyBps+flame.potencyBps+flameModifier(4);}
    }
    if(!replay) {player.mastery[id]=Math.min(100,mastery()+(brewed?6:2));if(brewed && player.rank===0) player.rank=1;}
    player.last={result,taps:taps.slice()};save(); page='result'; dialog=''; cancelAnimationFrame(frame); redraw();
  }
  function hit() {
    if(page!=='run' || performance.now()-lastTap<250) return;
    lastTap=performance.now();
    const s=steps[stage];
    const phase=(((lastTap-elapsedAt)*s.direction/s.period%1)+1)%1*10000;
    const gap=Math.abs(phase-s.target);
    const score=clamp(100-Math.floor(50*Math.min(gap,10000-gap)/s.width),0,100);
    taps.push(score);
    if(stage===2) {settle();return;}
    stage++; elapsedAt=performance.now(); redraw();
  }
  function animate() {
    cancelAnimationFrame(frame);
    const needle=document.querySelector('.da-needle');
    if(!needle||page!=='run') return;
    const s=steps[stage];
    function tick(now) {
      if(!needle.isConnected || page!=='run') return;
      needle.style.transform='rotate('+((now-elapsedAt)*360*s.direction/s.period)+'deg)';
      frame=requestAnimationFrame(tick);
    }
    tick(performance.now());
  }
  function begin() {
    if(!available()) return;
    page='run';dialog='';stage=0;taps=[];steps=makeSteps();elapsedAt=performance.now();lastTap=elapsedAt-100;redraw();
  }
  function button(text,action,css='',off=false) {
    return '<button type="button" class="'+css+'" data-dan="'+action+'" '+(off?'disabled':'')+'>'+text+'</button>';
  }
  function top() {
    return '<header class="da-top"><a href="设计总览.html" aria-label="回设计总览">‹</a><span>洞府 <b>／</b> 地火阁</span>'+button(ranks[player.rank]+'丹师','profile','da-rank',page==='run')+'</header>';
  }
  const hearth='<div class="da-flame" aria-hidden="true"></div>';
  function banner() {
    return '<div class="da-hero">'+hearth+'<div class="da-sparks" aria-hidden="true"></div><div class="da-hero-type"><span>松炭炉火</span><h1>地火丹房</h1></div></div>';
  }
  function stagesBar(active) {
    return '<ol class="da-stages">'+stages.map((s,i)=>'<li class="'+(i===active?'current':i<active?'done':'')+'"><i></i>'+s+(i<active?' <b>'+taps[i]+' 分</b>':'')+'</li>').join('')+'</ol>';
  }
  function mix() {
    const r=recipe(), main=r.main;
    return '<div class="da-line"><h2>本炉用料</h2><span>辅材 '+aux.length+' / 3</span></div>'+
      '<div class="da-main">'+badge(main)+button('<strong>'+matName(main)+'</strong><small>主药固定 · 查产地 ›</small>','source/'+main,'da-source-name')+'<b>× '+r.mainQty+'<small>持有 '+stock(main)+'</small></b></div>'+
      '<div class="da-slots">'+Array.from({length:3},(_,i)=>button(aux[i]?badge(aux[i])+'<span>'+matName(aux[i])+'</span>':'<i>＋</i><span>选辅材</span>', 'materials',aux[i]?'filled':'')).join('')+'</div>';
  }
  function prep() {
    const r=recipe();
    const why=player.rank<r.rank?'丹师需达'+ranks[r.rank]:!available()?'本炉余料不足，可在药匣补充':'';
    return banner()+'<section class="da-paper"><div class="da-prescription"><div><small>当前丹方</small><h2>'+r.name+'</h2></div>'+button('丹方簿 ›','recipes','da-soft')+'</div>'+
      '<p class="da-effect">'+effectText(r,r.strength)+'／粒 <span>'+button('丹效与售价 ›','recipeInfo','da-inline')+'</span></p>'+mix()+
      '<div class="da-forecast"><div><small>手炼成丹率</small><strong>'+percent(chance(0))+' – '+percent(chance(100))+'</strong></div><div><small>辅材相性</small><strong>'+assessment()+' '+pctLabel(affinity())+'</strong></div></div>'+
      '<div class="da-notes"><div><small>'+ (memory()?'留影药力':'中品药力预估')+'</small><b>'+effectText(r,potency(memory()||2,!!memory()))+'／粒</b></div><button type="button" data-dan="steps"><small>开炉次序</small><b>温炉 · 合药 · 凝丹 ›</b></button></div>'+
      '<p class="da-status" role="status">'+(why||feedback||'三次火候共同决定本炉成色。')+'</p></section>'+
      '<footer class="da-actions">'+button('药匣','materials','da-quiet')+button(memory()?'留影 · '+grades[memory()]:'尚无留影','replay','da-secondary',!memory()||!available())+button('开炉手炼','begin','da-primary',!available())+'</footer>';
  }
  function run() {
    const s=steps[stage], r=recipe();
    const ring='--start:'+((s.target-s.width)*.036)+'deg;--arc:'+(s.width*.072)+'deg';
    return '<section class="da-ritual"><div class="da-phase"><small>'+r.name+' · 第'+['一','二','三'][stage]+'道火候</small><h1>'+stages[stage]+'</h1><p>'+tips[stage]+'</p></div>'+
      '<button class="da-wheel" data-dan="hit" aria-label="点击锁定火候" style="'+ring+'"><span class="da-halo"></span><span class="da-target"></span><span class="da-ticks"></span><img alt="" class="da-vessel" src="assets/alchemy/bronze-furnace.webp">'+hearth+'<span class="da-needle"><i></i></span></button>'+
      '<div class="da-speed">'+(s.direction>0?'顺转':'逆转')+' · '+(s.period/1000).toFixed(2)+' 秒一周</div>'+
      (stage?'<div class="da-hit-note" role="status">'+stages[stage-1]+' '+taps[stage-1]+' 分</div>':'')+'</section>'+
      '<section class="da-paper da-inset">'+stagesBar(stage)+'<div class="da-line"><h2>炉中药材</h2></div><p>'+[matName(r.main)+' ×'+r.mainQty,...aux.map(matName)].join(' · ')+'</p></section>'+
      '<footer class="da-actions">'+button('待会儿再炼','cancel','da-secondary')+button(verbs[stage]+' · 定针','hit','da-primary')+'</footer>';
  }
  function receipt() {
    const o=result,r=book[o.id];
    return '<section class="da-result '+(o.brewed?'good':'lost')+'"><small>'+r.name+' · '+(o.replay?'留影复炼':'手炼出炉')+'</small><h1>'+(o.brewed?grades[o.grade]+'成丹':'炉火已散')+'</h1><div class="da-orb">'+(o.brewed?'<img src="assets/parts/pill.png" alt="成丹">':'<img src="assets/alchemy/bronze-furnace.webp" alt="丹炉">')+'</div></section>'+
      '<section class="da-paper da-inset"><div class="da-line"><h2>本炉丹记</h2><span>'+(o.replay?'固定品质':o.score+' 分')+'</span></div>'+
      '<div class="da-forecast"><div><small>入匣</small><strong>'+o.qty+' 粒</strong></div><div><small>本炉成丹率</small><strong>'+percent(o.bps)+'</strong></div></div>'+
      (o.brewed?'<p>'+effectText(r,o.effect,o.basis||'flat')+'／粒 · '+grades[o.grade]+' · 标准回收 '+r.prices[o.grade-1]+' 灵石／粒'+'</p>':'<p>主辅材已用尽。下炉可换相生辅材，或更靠近金弧中心出手。</p>')+
      (!o.replay?stagesBar(3):'')+'<p>'+(memory()?'此方已留影 · '+grades[memory()]+'。可换辅材直接复炼。':'成功一炉即可留下丹影。')+'</p></section>'+
      '<footer class="da-actions">'+button('查看丹匣','products','da-secondary')+button('返回配方','back','da-primary')+'</footer>';
  }
  function effectText(r,value,basis=r.basis) {
    if (basis==='realm_air') return r.effect+' '+percent(value)+' 境界修为门槛';
    if (basis==='max_blood') return r.effect+' '+percent(value)+' 气血上限';
    return r.effect+' '+value+' 点';
  }
  const qtyRange=(lo,hi)=>lo===hi?lo+' 份':lo+'–'+hi+' 份';
  const bundle={'mat.ninglu_herb':8,'mat.cold_dew':3,'mat.wild_herb':2,'mat.ember_moss':2,'mat.wind_slate':2};
  function sourceView(id) {
    const m=materials[id];
    const text=[];
    text.push(m.buyPrice?'药柜单买　'+m.buyPrice+' 灵石／份':'药柜单买　暂未上架');
    text.push('材料参考价值　'+m.value+' 灵石／份');
    if(m.gather.length || m.drops.length) text.push('野外取得的此模板药材，按品阶回收 '+m.fieldSell+' 灵石／份。');
    if(bundle[id]) text.push('丹房药包　每包 '+bundle[id]+' 份，首次免费，续包 90 灵石。');
    if(['mat.frost_reed','mat.ninglu_herb','mat.cold_dew'].includes(id)) text.push('符案　入门料包可得。');
    if(id==='mat.cold_dew') text.push('符案凝露　凝露草 3 份炼成寒露液 1 份。');
    const garden={'mat.ninglu_herb':'凝露草种 12 灵石，基础 60 秒、4 株','mat.frost_reed':'霜节芦种 28 灵石，基础 5 分钟、4 株','mat.dream_lotus':'梦泽莲种 54 灵石，基础 15 分钟、5 株'};
    if(garden[id]) text.push('灵田　'+garden[id]);
    m.gather.forEach(g=>text.push(g.place+' · '+g.method+'　同类节点 '+percent(g.bps)+'，每处 '+qtyRange(g.min,g.max)));
    if(m.legacy.length) text.push('历练通用掉落　普通怪 7.5% ×1，精英约 14.17% ×1，首领约 16.67% ×2；这类战利品按自身品阶回收。'+(id==='mat.common_core'?'清场另得 1 份妖丹。':''));
    if(m.drops.length) text.push('击败指定怪物时，以下三档依次为普通、精英、首领。');
    m.drops.forEach(d=>text.push(d.place+' · '+d.name+'　'+d.bps.map(v=>v?percent(v):'—').join(' / ')+'，每次 '+qtyRange(d.min,d.max)));
    return text.map(v=>'<p>'+v+'</p>').join('')+button('回到药匣','materials','da-soft');
  }
  function recipeView(r) {
    const total=r.mainQty*materials[r.main].value+aux.reduce((n,m)=>n+materials[m].value,0);
    const strength=potency(memory()||2,!!memory());
    let example='';
    if(r.basis!=='flat') {
      const index=clamp(player.realm??0,0,realms.length-1), realm=realms[index];
      const base=r.basis==='realm_air'?realm.air:realm.blood;
      const amount=Math.max(1,Math.round(base*strength/10000));
      const unit=r.basis==='realm_air'?'修为门槛':'精血上限';
      example='<label>服用者境界试算 <select data-realm>'+realms.map((v,i)=>'<option value="'+i+'" '+(i===index?'selected':'')+'>'+v.name+'</option>').join('')+'</select></label><p>'+realm.name+' · '+unit+' '+base.toLocaleString('zh-CN')+'，按当前'+grades[memory()||2]+'药力 '+percent(strength)+'，单粒约 '+amount.toLocaleString('zh-CN')+' 点。</p>';
    }
    let recovery='野采主药按标准回收。';
    if(bundle[r.main]){
      const perUnit=Math.floor(90/Object.values(bundle).reduce((a,n)=>a+n,0));
      const perPill=Math.min(r.prices[2],Math.floor(perUnit*r.mainQty/r.cap));
      recovery+='主药全来自免费丹房药包时，成品回收 0；主药全来自 90 灵石丹房药包时，上品每粒实际回收 '+perPill+' 灵石。';
    }
    return '<p>'+r.note+'</p>'+example+'<p>熟练 '+mastery()+'/100 · '+ranks[r.rank]+'丹师可炼。</p><p>基础药效　'+effectText(r,r.strength)+'／粒</p><p>主药　'+matName(r.main)+' ×'+r.mainQty+'　·　本炉用料估值 '+total+' 灵石</p><p>预计基础产量 '+r.minQty+'–'+r.maxQty+' 粒，单炉上限 '+r.cap+' 粒。</p>'+(r.poison?'<p>每粒增加 '+r.poison+' 点丹毒。服后将超过 100 点时无法服用；清毒丹可降毒。</p>':'')+'<p>标准回收价　'+r.prices.map((price,i)=>grades[i+1]+' '+price).join('　')+' 灵石／粒。</p><p>'+recovery+' 混用时按实际主药批次折算，最终单价见纳戒。</p>'+button('查主药来源','source/'+r.main,'da-soft');
  }
  function modal() {
    if(!dialog) return '';
    const r=recipe(); let title='',html='';
    if(dialog==='source') {
      title=matName(guide)+' · 采买';html=sourceView(guide);
    } else if(dialog==='recipeInfo') {
      title=r.name+' · 丹方';html=recipeView(r);
    } else if(dialog==='materials') {
      title='随身药匣';
      html='<p>主药 '+matName(r.main)+' ×'+r.mainQty+' · 已选辅材 '+aux.length+'/3</p>'+Object.keys(materials).filter(m=>m!==r.main).map(m=>{
        const qty=selected(m);return '<div class="da-matrow">'+badge(m)+button('<strong>'+matName(m)+'</strong><small>'+relation(materials[m].element,materials[r.main].element)[1]+' · 余 '+stock(m)+' 份 · 查产地 ›</small>', 'source/'+m,'da-source-name')+button('−','minus/'+m,'da-count',qty<1)+'<b>'+qty+'</b>'+button('＋','plus/'+m,'da-count',aux.length>=3||qty>=stock(m))+'</div>';
      }).join('')+button('补充样稿药材','stock','da-soft')+(memory()?button('调回留影用料','restore','da-soft'):'');
    } else if(dialog==='recipes') {
      title='丹方簿'; html=Object.values(book).map(v=>button('<div><strong>'+v.name+'</strong><small>'+matName(v.main)+' ×'+v.mainQty+' · '+effectText(v,v.strength)+'</small></div><b>'+(v.id===id?'已选':ranks[v.rank]+'可炼')+'</b>', 'pick/'+v.id,'da-option')).join('');
    } else if(dialog==='steps') {
      title='本炉工序';html='<p>'+r.name+' · '+assessment()+' '+pctLabel(affinity())+' · 熟练 '+mastery()+'/100</p>';
      html+=makeSteps().map((s,i)=>'<div class="da-matrow"><i class="da-order">'+['一','二','三'][i]+'</i><div><strong>'+stages[i]+'</strong><small>'+tips[i]+'</small></div><b>'+ (s.period/1000).toFixed(2)+' 秒／圈<br>'+Number((s.width*.072).toFixed(1))+'° 金弧</b></div>').join('');
      html+=button('开炉手炼','begin','da-soft',!available());
    } else if(dialog==='products') {
      title='本地丹匣';html=player.products.map(v=>'<div class="da-matrow"><div><strong>'+grades[v.grade]+book[v.id].name+'</strong><small>'+effectText(book[v.id],v.effect,v.basis||'flat')+'／粒 · 标准回收 '+book[v.id].prices[v.grade-1]+' 灵石／粒</small></div><b>×'+v.qty+'</b></div>').join('')||'<p>尚未收丹。</p>';
    } else {
      title='丹师手札';html='<p>此画板使用本机示例角色。切换丹阶可比较同一丹方的火候速度与容错。</p><label>丹师品阶 <select data-rank>'+ranks.map((n,i)=>'<option value="'+i+'" '+(player.rank===i?'selected':'')+'>'+n+'</option>').join('')+'</select></label><p>温炉、合药、凝丹依次占操作评分的 25%、35%、40%。换辅材会改变相性、命中弧宽与单粒药力。</p>'+button('查看本地丹匣','products','da-soft');
    }
    return '<div class="da-shade"><section role="dialog" aria-modal="true" aria-label="'+title+'" class="da-dialog"><header><h2>'+title+'</h2>'+button('完成','dismiss','da-soft')+'</header><div class="da-scroll">'+html+'</div></section></div>';
  }
  let lastRoute='';
  window.alchemyRoomDesign = function(d) {
    if(!group.has(d.id)) return;
    const route=location.hash;
    if(route!==lastRoute) {
      lastRoute=route;page='prep';dialog='';feedback='';cancelAnimationFrame(frame);
      if(d.id==='DiHuoGeCaiLiaoChooseWnd') dialog='materials';
      if(d.id==='DiHuoGeLianZhiBuZhouSheZhiWnd') dialog='steps';
      if(d.id==='DiHuoGeLianZhiWnd' && available()) {
        steps=makeSteps();stage=0;taps=[];page='run';elapsedAt=performance.now();lastTap=-Infinity;
        requestAnimationFrame(animate);
      }
      if(d.id==='DiHuoGeLianDanJieSuanWnd') {
        const saved=player.last;
        if(saved && book[saved.result?.id]) {result=saved.result;id=result.id;taps=saved.taps||[];page='result';}
        else feedback='丹记尚为空。先开一炉，出炉后即可查看。';
      }
    }
    return '<article class="da-room">'+top()+'<div class="da-content">'+(page==='run'?run():page==='result'?receipt():prep())+'</div>'+modal()+'</article>';
  };
  document.addEventListener('click',e=>{
    const btn=e.target.closest('[data-dan]'); if(!btn||btn.disabled) return;
    const [cmd,chosen]=btn.dataset.dan.split('/');
    if(cmd==='hit') {hit();return;}
    if(cmd==='begin') {begin();return;}
    if(cmd==='replay') {settle(true);return;}
    if(cmd==='pick') {id=chosen;aux=aux.filter(m=>m!==recipe().main);dialog='';feedback='';}
    else if(cmd==='source') { guide=chosen;dialog='source'; }
    else if(cmd==='plus') {if(aux.length<3 && selected(chosen)<stock(chosen)) aux.push(chosen);}
    else if(cmd==='minus') {const at=aux.indexOf(chosen);if(at>=0) aux.splice(at,1);}
    else if(cmd==='restore') {aux=(player.memoryMix?.[id]||[]).slice();dialog='';feedback='已调回留影用料。';}
    else if(cmd==='stock') fillStock();
    else if(cmd==='dismiss') dialog='';
    else if(cmd==='cancel'||cmd==='back') {cancelAnimationFrame(frame);page='prep';taps=[];feedback='';}
    else dialog=cmd;
    const scroll=document.querySelector('.da-scroll')?.scrollTop||0;
    redraw();
    if(cmd==='plus'||cmd==='minus'||cmd==='stock') document.querySelector('.da-scroll').scrollTop=scroll;
  });
  document.addEventListener('change',e=>{
    if(e.target.matches('[data-realm]')){player.realm=clamp(Number(e.target.value),0,realms.length-1);save();redraw();return;}
    if(!e.target.matches('[data-rank]')) return;
    player.rank=clamp(Number(e.target.value),0,9);save();redraw();
  });
  document.addEventListener('keydown',e=>{
    if(!document.querySelector('.da-room')) return;
    if(e.code==='Space' && page==='run' && !e.repeat) {e.preventDefault();hit();}
    if(e.key==='Escape' && dialog) {dialog='';redraw();}
  });
})();
