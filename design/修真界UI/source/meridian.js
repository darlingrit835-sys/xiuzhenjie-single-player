/* The chart uses local preview progress. All 63 identities, graph links and costs come from the project tables. */
(() => {
  const points = window.meridianPoints.map(([id,name,element,groups,next,base,x,y]) => ({
    id,name,element,groups:groups.split(','),next,base,x:x-512,y:y-318,parents:[]
  }));
  const byId=new Map(points.map(p=>[p.id,p]));
  points.forEach(p=>p.next.forEach(id=>byId.get(id).parents.push(p.id)));
  const routes = {
    任脉:['纳气速率',[0,.5,.8,1]],督脉:['纳气效率',[0,.5,.8,1]],
    冲脉:['精血上限',[0,.5,.8,1]],带脉:['精血恢复',[0,200,500,2000]],
    阴跷脉:['神识上限',[0,.5,.8,1]],阳跷脉:['五行伤害',[0,1,2,3]],
    阴维脉:['体魄上限',[0,.5,.8,1]],阳维脉:['五行防御',[0,1,2,3]]
  };
  const elementAssets={木:'mu',火:'huo',土:'tu',金:'jin',水:'shui'};
  const tints={木:'156,239,177',火:'252,167,114',土:'224,193,137',金:'253,218,144',水:'141,220,235'};
  const saveKey='xiuzhenjie.design.meridians.v1';
  const initial=()=>({balance:40,levels:{}});
  function load() {
    try {
      const stored=JSON.parse(localStorage.getItem(saveKey));
      if(!stored||!Number.isSafeInteger(stored.balance)||stored.balance<0||!stored.levels)return initial();
      return {balance:stored.balance,levels:Object.fromEntries(Object.entries(stored.levels).filter(([id,n])=>byId.has(+id)&&[1,2,3].includes(n)))};
    }catch{return initial();}
  }
  const progress=load();
  let chosen=1, route='任脉', zoom=1, cash=null, active=null, run=0, sampleStatus=0, sampleKey='';
  const level=p=>cash&&cash.id===p.id&&!cash.done?cash.from:(progress.levels[p.id]||0);
  const available=p=>level(p)>0||p.id===1||p.parents.some(id=>level(byId.get(id))>0);
  const status=p=>level(p)?'已贯通':available(p)?'可冲穴':'待前穴';
  const fee=p=>p.base*(2*level(p)+1);
  const linked=p=>[...new Set([...p.parents,...p.next])].map(id=>byId.get(id)).filter(q=>level(q)>0);
  const concord=p=>level(p)>0&&level(p)<3?Math.min(20,linked(p).length*10):0;
  const rate=p=>Math.min(100,[100,50,10,0][level(p)]+concord(p));
  const mode=d=>d.kind==='component'?'sample':d.id==='ZongLanWnd'?'summary':'chart';
  const btn=(label,a,value='',cls='',attrs='')=>'<button type="button" class="'+cls+'" data-meridian="'+a+'" data-value="'+value+'" '+attrs+'>'+label+'</button>';
  const nextPoints=()=>points.filter(p=>p.groups.includes(route));
  function select(p,r) {
    if(!p)return;
    chosen=p.id;
    route=r|| (p.groups.includes(route)?route:p.groups[0]);
  }
  const caption=p=>p.groups.join(' · ');
  const save=()=>{try{localStorage.setItem(saveKey,JSON.stringify(progress));}catch{}};
  function shell(d,contents,cls='') {
    return '<article class="x-cult m-atlas '+cls+'" data-board="'+d.id+'"><header class="x-top"><span class="x-chapter">修真界<i></i>修炼与参悟</span><span>经脉内景</span></header>'+contents+
      '<nav class="x-nav" aria-label="修行去处">'+[['DongFuWnd','练功'],['CanWuWnd','参悟'],['NeiShiWnd','内视'],['TuPoDuJieExmpleWnd','渡劫']].map(([id,title])=>'<button type="button" data-x-action="open" data-x-value="'+id+'" class="'+(id==='NeiShiWnd'?'active':'')+'">'+title+'</button>').join('')+'</nav></article>';
  }
  const totalOpen=()=>points.filter(p=>level(p)>0).length;
  const opt=(v,t,sel)=>'<option value="'+v+'" '+(sel?'selected':'')+'>'+t+'</option>';
  function controls(p) {
    return '<div class="m-controls"><label>循脉<select aria-label="切换经脉" data-meridian-select="route">'+Object.keys(routes).map(r=>opt(r,r,r===route)).join('')+'</select></label>'+
      '<label>寻穴<select aria-label="选择穴位" data-meridian-select="point">'+nextPoints().map(n=>opt(n.id,n.name+' · '+status(n),n.id===p.id)).join('')+'</select></label></div>';
  }
  function effects(p) {
    const n=level(p), changes=new Map();
    for(const routeName of p.groups){
      const [label,values]=routes[routeName];
      const delta='＋'+values[n]+'%'+(n<3?' → ＋'+values[n+1]+'%':'');
      if(!changes.has(delta))changes.set(delta,[]);
      changes.get(delta).push(label);
    }
    return [...changes].map(([delta,names])=>names.join('、')+'　'+delta).join('<br>');
  }
  const art=new Image();
  let plate=null;
  function spiritPlate() {
    if(plate||!art.complete||!art.naturalWidth)return plate;
    plate=document.createElement('canvas');plate.width=plate.height=1024;
    const c=plate.getContext('2d');c.drawImage(art,0,0,1024,1024);
    c.globalCompositeOperation='destination-in';
    for(const horizontal of [false,true]){
      const fade=c.createLinearGradient(0,0,horizontal?1024:0,horizontal?0:1024);
      fade.addColorStop(0,'transparent');fade.addColorStop(.09,'black');
      fade.addColorStop(.90,'black');fade.addColorStop(1,'transparent');
      c.fillStyle=fade;c.fillRect(0,0,1024,1024);
    }
    return plate;
  }
  function chart(d) {
    const p=byId.get(chosen), sample=mode(d)==='sample', working=!!cash&&cash.id===p.id, busy=working&&!cash.done;
    const levelNow=level(p), locked=!available(p), limit=levelNow===3, broke=progress.balance<fee(p);
    const cls=working?' m-'+cash.phase:'',h1=sample?'穴位灵枢':d.id==='JingMaiWnd'?'冲脉':'内视经脉';
    if(sample&&sampleKey!==d.key){sampleStatus=d.path.startsWith('Resources/')?1:2;sampleKey=d.key;}
    const phase=sample?['未启','可冲','已通'][sampleStatus]:working?({gather:'真气汇聚',strike:'灵气冲关',success:'经脉贯通',failure:'冲穴未成'}[cash.phase]):status(p);
    const header='<div class="m-pagehead"><div><p>沈青禾 · 灵身内景</p><h1>'+h1+'</h1></div><div class="m-counter"><b>'+progress.balance+'</b><span>经脉点</span></div></div>';
    const stage='<section class="m-stage'+cls+'" aria-label="经脉图">'+
      '<canvas role="img" aria-label="'+p.name+'，'+caption(p)+'。可点击选穴，或使用下方寻穴选择。"></canvas>'+
      '<div class="m-map-tools">'+btn(zoom===1?'放大':'全身','zoom','','m-zoom','aria-label="'+(zoom===1?'放大选中穴位':'查看全身')+'"')+'</div>'+
      '<div class="m-map-legend"><span><i class="closed"></i>未启</span><span><i class="ready"></i>可冲</span><span><i class="open"></i>已通</span></div>'+
      '<div class="m-mark"><b>'+p.name+'</b><small>'+phase+'</small></div></section>';
    let body,foot;
    if(sample) {
      body='<div class="m-sheet"><h2>'+p.name+'穴</h2><p>选择状态，观察灵枢的形状与气流。</p><div class="m-variants">'+['未启','可冲','已通'].map((v,i)=>btn(v,'sample',i,i===sampleStatus?'active':'','aria-pressed="'+(i===sampleStatus)+'"')).join('')+'</div></div>';
      foot='<div class="x-foot">'+btn('返回内视　→','back','','m-primary')+'</div>';
    } else {
      const reason=locked?'先贯通'+p.parents.map(i=>byId.get(i).name).join('或'):limit?'已修至三层':broke?'经脉点不足':levelNow===0?'首层必成':concord(p)?'邻穴合流 ＋'+concord(p)+' 个百分点 · '+linked(p).map(q=>q.name).join('、'):'相邻穴每通一处，突破成功率加 10 个百分点，最多加 20。';
      const note=working?(cash.done?(cash.phase==='success'?'贯通第'+cash.to+'层 · 已消耗 '+cash.cost+' 点':'冲穴未成 · 保留原层数，消耗 '+cash.cost+' 点'):phase):reason;
      body='<div class="m-sheet">'+controls(p)+'<div class="m-selected"><img src="assets/parts/'+elementAssets[p.element]+'.png" alt=""><div><p>'+caption(p)+'</p><h2>'+p.name+'<small>'+p.element+'属</small></h2></div><span class="m-level">'+[1,2,3].map(n=>'<i class="'+(n<=levelNow?'on':'')+'"></i>').join('')+'<small>'+levelNow+' / 3 层</small></span></div>'+
        '<div class="m-effects">'+effects(p)+'</div><div class="m-cost"><span>冲穴消耗 <b>'+(limit?'—':fee(p)+' 点')+'</b></span><span>成功率 <b>'+(limit?'—':rate(p)+'%')+'</b></span></div>'+
        '<div class="m-feedback" aria-live="polite">'+note+(working&&cash.done&&cash.phase==='success'&&levelNow<3?btn('继续修此穴','stay','','m-stay'):'')+'</div></div>';
      foot='<div class="x-foot">'+btn('八脉总览','summary','','m-secondary')+
        (working&&cash.done&&cash.phase==='success'&&p.next.length?btn('循气探下穴　→','next','','m-primary'):btn(busy?phase:limit?'穴位已圆满':'凝气冲穴　→','cast','','m-primary',busy||locked||broke||limit?'disabled':''))+'</div>';
    }
    return shell(d,header+stage+body+foot,cls);
  }
  function overview(d) {
    const head='<div class="m-pagehead"><div><p>沈青禾 · 灵身内景</p><h1>八脉总览</h1></div><div class="m-counter"><b>'+totalOpen()+'<small>/63</small></b><span>已通穴位</span></div></div>';
    const rows=Object.entries(routes).map(([name,[gain,values]])=>{
      const nodes=points.filter(p=>p.groups.includes(name)), opened=nodes.filter(p=>level(p));
      const bonus=opened.reduce((acc,p)=>acc+values[level(p)],0);
      return btn('<span class="m-seal">'+name[0]+'</span><span class="m-routename"><b>'+name+'</b><small>'+gain+' ＋'+Number(bonus.toFixed(1))+'%</small></span><span class="m-route-count">'+opened.length+' / '+nodes.length+' 穴　›</span>','route',name);
    }).join('');
    return shell(d,head+'<div class="m-ledger"><div class="m-ledger-art"></div><div class="m-summary-list">'+rows+'</div></div><div class="x-foot">'+btn('返回内视　→','back','','m-primary')+'</div>','m-summary');
  }
  function redraw() {
    const doc=active&&document.querySelector('.m-atlas');
    if(doc)window.draw(active.key);
  }
  function scrollTo(id) {
    const key=catalog.find(d=>d.id===id)?.key;
    if(!key)return;
    if(decodeURIComponent(location.hash.slice(1))===key)window.draw(key);else location.hash=encodeURIComponent(key);
  }
  function cast() {
    const p=byId.get(chosen), cost=fee(p), n=level(p);
    if(cash&&!cash.done||!available(p)||n>=3||progress.balance<cost)return;
    const won=Math.random()*100<rate(p);
    progress.balance-=cost;if(won)progress.levels[p.id]=n+1;save();
    const result={id:p.id,phase:'gather',startedAt:performance.now()/1000,done:false,from:n,to:n+1,cost,won};cash=result;redraw();
    const reduced=matchMedia('(prefers-reduced-motion: reduce)').matches;
    setTimeout(()=>{if(cash===result){cash.phase='strike';redraw();}},reduced?20:680);
    setTimeout(()=>{if(cash===result){cash.phase=won?'success':'failure';cash.done=true;redraw();}},reduced?80:1380);
  }
  function focus(p,r) {
    if(cash&&!cash.done)return;
    cash=null;select(p,r);redraw();
  }
  document.addEventListener('change',e=>{
    const control=e.target.closest('[data-meridian-select]');if(!control)return;
    if(cash&&!cash.done){redraw();return;}
    if(control.dataset.meridianSelect==='route'){
      const r=control.value, list=points.filter(p=>p.groups.includes(r));
      focus(list.find(p=>available(p)&&!level(p))||list[0],r);
    }else focus(byId.get(+control.value));
  });
  document.addEventListener('click',e=>{
    const b=e.target.closest('.m-atlas [data-meridian]');if(!b||b.disabled)return;
    const {meridian:a,value:v}=b.dataset;
    if(a==='cast'){cast();return;}
    if(a==='zoom'){zoom=zoom===1?1.9:1;redraw();return;}
    if(a==='sample'){sampleStatus=+v;redraw();return;}
    if(a==='summary'){scrollTo('ZongLanWnd');return;}
    if(a==='back'){scrollTo('NeiShiWnd');return;}
    if(cash&&!cash.done)return;
    if(a==='stay'){focus(byId.get(chosen));return;}
    if(a==='next'){const p=byId.get(chosen);focus(byId.get(p.next.find(id=>!progress.levels[id])||p.next[0]));}
    if(a==='route'){route=v;const list=nextPoints();focus(list.find(p=>available(p)&&!level(p))||list[0],v);scrollTo('NeiShiWnd');}
  });
  const edges=points.flatMap(p=>p.next.map(id=>{
    const q=byId.get(id);
    const bend=Math.min(22,Math.abs(q.y-p.y)*.13)*(q.x>=p.x?1:-1);
    return {p,q,cx:(p.x+q.x)/2+bend,cy:(p.y+q.y)/2};
  }));
  const particleSeeds=Array.from({length:34},(_,i)=>({x:((i*277+41)%997)/997,y:((i*331+61)%991)/991,phase:i*1.83}));
  function startChart(canvas, token) {
    const ctx=canvas.getContext('2d'), stage=canvas.parentElement, marker=stage.querySelector('.m-mark'), selected=byId.get(chosen);
    const reduced=matchMedia('(prefers-reduced-motion: reduce)').matches;
    let last=0, cssW=0, cssH=0, ratio=1;
    function view() {
      const w=canvas.clientWidth,h=canvas.clientHeight;
      if(w!==cssW||h!==cssH){cssW=w;cssH=h;ratio=Math.min(2,devicePixelRatio||1);canvas.width=Math.round(w*ratio);canvas.height=Math.round(h*ratio);}
      const s=Math.min(w/940,(h-23)/975)*zoom;
      const centerX=zoom===1?512:Math.max(355,Math.min(675,selected.x));
      const centerY=zoom===1?508:selected.y;
      return {w,h,s,dx:w*.5-centerX*s,dy:(h-19)*.5-centerY*s};
    }
    function stroke(cx,cy,r,start,end,color,lineWidth=1) {
      ctx.beginPath();ctx.arc(cx,cy,r,start,end);ctx.strokeStyle=color;ctx.lineWidth=lineWidth;ctx.stroke();
    }
    function glow(x,y,r,center,edge='rgba(0,0,0,0)'){
      const light=ctx.createRadialGradient(x,y,0,x,y,r);light.addColorStop(0,center);light.addColorStop(1,edge);
      ctx.fillStyle=light;ctx.fillRect(x-r,y-r,2*r,2*r);
    }
    function stars(v,t) {
      const {w,h}=v;
      const radius=Math.min(w*.39,h*.4),cx=w*.51,cy=(h-19)*.46;
      ctx.save();
      const corona=ctx.createRadialGradient(cx,cy,0,cx,cy,radius*1.35);
      corona.addColorStop(0,'#5baf9830');corona.addColorStop(.54,'#548e7840');corona.addColorStop(1,'#18342e00');
      ctx.fillStyle=corona;ctx.fillRect(0,0,w,h);
      for(let i=0;i<3;i++)stroke(cx,cy,radius*(1+i*.08),0,Math.PI*2,['#a8a66935','#cdb97b55','#c0bc7736'][i],.7);
      ctx.strokeStyle='#d8c58285';ctx.lineWidth=.75;ctx.beginPath();
      for(let i=0;i<72;i++){
        const a=i*Math.PI/36+t*.008,l=i%6===0?7:i%3===0?4:2;
        ctx.moveTo(cx+radius*1.09*Math.cos(a),cy+radius*1.09*Math.sin(a));ctx.lineTo(cx+(radius*1.09+l)*Math.cos(a),cy+(radius*1.09+l)*Math.sin(a));
      }ctx.stroke();
      for(let i=0;i<7;i++)stroke(cx,cy,radius*.94,i*.91-t*.023,i*.91+.4-t*.023,'#e1ca825d',1.1);
      ctx.globalCompositeOperation='screen';
      for(const v of particleSeeds){
        const x=v.x*w+Math.cos(t*.32+v.phase)*5,y=((v.y*h-t*(4+v.x*6))%h+h)%h,r=.4+.75*v.x;
        ctx.fillStyle='rgba(220,211,151,'+(.2+.5*(.5+.5*Math.sin(t+v.phase)))+')';
        ctx.beginPath();ctx.arc(x,y,r,0,Math.PI*2);ctx.fill();
        if(v.x>.83)glow(x,y,5,'rgba(159,227,197,.26)');
      }
      ctx.restore();
    }
    function line(edge,color,width){
      ctx.beginPath();ctx.moveTo(edge.p.x,edge.p.y);ctx.quadraticCurveTo(edge.cx,edge.cy,edge.q.x,edge.q.y);
      ctx.strokeStyle=color;ctx.lineWidth=width;ctx.stroke();
    }
    function spark(edge,progress,brightness) {
      const a=1-progress, x=a*a*edge.p.x+2*a*progress*edge.cx+progress*progress*edge.q.x;
      const y=a*a*edge.p.y+2*a*progress*edge.cy+progress*progress*edge.q.y;
      glow(x,y,11,brightness?'#edc872a8':'#64cab78f');
      ctx.fillStyle=brightness?'#ffecb3':'#bcf9d8';ctx.beginPath();ctx.arc(x,y,brightness?2.8:2,0,Math.PI*2);ctx.fill();
    }
    function strands(t) {
      ctx.lineCap='round';ctx.lineJoin='round';
      for(const edge of edges){
        const inRoute=edge.p.groups.includes(route)&&edge.q.groups.includes(route), open=!!level(edge.p)&&!!level(edge.q);
        const concordant=open&&(edge.p.id===chosen||edge.q.id===chosen);
        if(inRoute||open)line(edge,'#15362fba',7.2);
        line(edge,concordant?'#ffe4a1':open?'#f4ce83ef':inRoute?'#b2f1cfed':'#a8bbaa94',concordant?5.4:open?3.8:inRoute?3.4:1.7);
        if(inRoute||open){
          line(edge,open?'#d9a7473d':'#62ccb32e',8);
          const phase=(t*.25+(edge.p.id*.389)%1)%1;
          if(edge.p.id%3===0||open||edge.p.id===chosen||edge.q.id===chosen)spark(edge,phase,open);
        }
      }
    }
    function drawNode(p,t,sample) {
      const selectedNode=p.id===chosen, inRoute=p.groups.includes(route);
      const lit=selectedNode&&sample?sampleStatus===2:level(p)>0;
      const usable=selectedNode&&sample?sampleStatus===1:available(p)&&!lit;
      const r=lit?6.4:usable?6.2:inRoute?4.1:2.6;
      ctx.save();ctx.translate(p.x,p.y);ctx.lineWidth=inRoute?1.6:1;
      ctx.strokeStyle=lit?'#ffda85':usable?'#baf0d1':inRoute?'#a4bbb0':'#79958c';ctx.fillStyle=lit?'#eed79a':usable?'#567e69':'#203b37';
      ctx.beginPath();
      if(usable){ctx.moveTo(0,-r*1.45);ctx.lineTo(r*1.45,0);ctx.lineTo(0,r*1.45);ctx.lineTo(-r*1.45,0);ctx.closePath();}
      else if(lit){ctx.arc(0,0,r,0,Math.PI*2);}
      else ctx.rect(-r,-r,2*r,2*r);
      ctx.fill();ctx.stroke();if(lit)stroke(0,0,r+4,0,Math.PI*2,'#f4d893af',1);
      if(selectedNode){
        const pulse=(Math.sin(t*2.1)+1)*.5;
        glow(0,0,33+11*pulse,'rgba('+tints[p.element]+',.43)');
        stroke(0,0,16+pulse*3,0,Math.PI*2,'#ffdda5d9',1.7);
        stroke(0,0,23,-t*.65,.65-t*.65,'#fce5aa',2.4);
        stroke(0,0,23,Math.PI-t*.65,Math.PI+.65-t*.65,'#fce5aa',2.4);
        for(let i=0;i<4;i++){
          const a=i*Math.PI/2;ctx.beginPath();ctx.moveTo(Math.cos(a)*26,Math.sin(a)*26);ctx.lineTo(Math.cos(a)*34,Math.sin(a)*34);ctx.strokeStyle='#ffe6a1';ctx.stroke();
        }
      }
      ctx.restore();
    }
    function burst(t) {
      if(!cash||cash.id!==chosen)return;
      const {x,y}=selected;
      const phase=cash.phase;
      const duration=t-(cash.startedAt||t);
      if(duration>3.5||duration<0)return;
      ctx.save();ctx.globalCompositeOperation='screen';
      const color=phase==='failure'?'#f8ba83':'#ffe5a4';
      const fade=Math.min(1,Math.max(0,3.2-duration));
      ctx.globalAlpha=fade;
      if(phase==='gather'){
        for(let i=0;i<16;i++){
          const a=i*Math.PI/8+t*1.9, distance=30+((i*19+154-duration*96)%145+145)%145;
          const xx=x+Math.cos(a)*distance,yy=y+Math.sin(a)*distance;
          glow(xx,yy,11,'rgba('+tints[selected.element]+',.77)');
        }
        glow(x,y,58,'#e6cc7f90');
      }else{
        for(let i=0;i<3;i++){
          const r=(phase==='strike'?36:15)+((duration*85+i*30)%110);
          stroke(x,y,r,0,Math.PI*2,phase==='failure'?'#eea47d83':'#eccb9579',2);
        }
        for(let i=0;i<18;i++){
          const a=i*Math.PI/9+selected.id*.18,len=34+((i*19+duration*58)%100);
          ctx.beginPath();ctx.moveTo(x+Math.cos(a)*(len-9),y+Math.sin(a)*(len-9));ctx.lineTo(x+Math.cos(a)*len,y+Math.sin(a)*len);ctx.strokeStyle=color;ctx.lineWidth=1.8;ctx.stroke();
        }
        glow(x,y,58,phase==='failure'?'#e69b6260':'#e5d889a0');
      }
      ctx.restore();
    }
    const sample=mode(active)==='sample';
    function paint(ms) {
      if(token!==run||!canvas.isConnected)return;
      if(!cssW||ms-last>=32||reduced){
        last=ms;
        const v=view();if(v.w<=0||v.h<=0){requestAnimationFrame(paint);return;}
        const t=reduced?1:ms/1000;
        ctx.setTransform(ratio,0,0,ratio,0,0);ctx.clearRect(0,0,v.w,v.h);
        ctx.save();ctx.translate(v.dx,v.dy);ctx.scale(v.s,v.s);
        const painting=spiritPlate();if(painting)ctx.drawImage(painting,0,0);
        ctx.restore();
        ctx.globalAlpha=.78;stars(v,t);ctx.globalAlpha=1;
        ctx.save();ctx.translate(v.dx,v.dy);ctx.scale(v.s,v.s);
        strands(t);for(const p of points)if(p.id!==chosen)drawNode(p,t,sample);
        drawNode(selected,t,sample);burst(t);ctx.restore();
        const x=v.dx+v.s*selected.x,y=v.dy+v.s*selected.y;
        const mx=x>v.w*.64?Math.max(4,x-72):Math.min(v.w-54,x+18);
        marker.style.left=mx+'px';marker.style.top=Math.max(10,Math.min(v.h-64,y-15))+'px';
      }
      if(!reduced||!art.complete)requestAnimationFrame(paint);
    }
    canvas.addEventListener('pointerup',e=>{
      if(mode(active)==='sample'||cash&&!cash.done)return;
      const v=view(),rect=canvas.getBoundingClientRect(),x=(e.clientX-rect.left-v.dx)/v.s,y=(e.clientY-rect.top-v.dy)/v.s;
      const nearest=points.map(p=>[Math.hypot(p.x-x,p.y-y),p]).sort((a,b)=>a[0]-b[0])[0];
      if(nearest[0]*v.s<=17)focus(nearest[1]);
    });
    if(reduced&&!art.complete)art.addEventListener('load',()=>paint(performance.now()),{once:true});
    paint(performance.now());
  }
  window.meridianBoard=d=>{
    active=d;
    const token=++run;
    if(mode(d)==='summary')return overview(d);
    if(!art.src)art.src='assets/cultivation/meridian-body.webp';
    requestAnimationFrame(()=>{const c=document.querySelector('.m-atlas canvas');if(c&&token===run)startChart(c,token);});
    return chart(d);
  };
})();
