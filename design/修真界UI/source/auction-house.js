(() => {
  'use strict';
  const W = {
    street:'ZhongJieFangShiWnd',lobby:'AH_MainSceneWnd', room:'AH_PaiMaiHuWnd', catalog:'AH_PaiPinListWnd',
    consign:'AH_PaiPinXiangQingWnd', bid:'AH_PaiPingXiangQingWnd', sent:'AH_WoDeGuaPaiWnd', pick:'AH_WoDePaiMaoWnd',
    market:'QiZhenGeWnd', account:'QZG_ChooseWnd', item:'QZG_GoodsInformationWnd', buy:'QZG_ItmeInformation',
    bag:'QZG_PaiMaiWnd', list:'QZG_PaiMaiWuPinXinXiWnd', filter:'QZG_ShaiXuan', bids:'QZG_WoDeJingPaiWnd', posted:'QZG_WoDePaiMaiWnd'
  };
  const screens = Object.fromEntries(Object.entries(W).map(([k,v])=>[v,k]));
  const components = {AH_Item:'lot',QZG_InfoItem:'info',QZG_WoDeJingPaiItem:'bid',QZG_WoDePaiMaiItem:'posted',QiZhenGeItem:'fixed',QiZhenGePaiMaiItem:'bag'};
  const tiers = {
    daily:{name:'每日小拍',poem:'临仙小集',seal:'日集',limit:8,duration:45,clock:'每日 19:00',stop:'每日 18:00',start:'每日 09:00',rival:'竹字七席'},
    weekly:{name:'每周大拍',poem:'云间大集',seal:'周会',limit:16,duration:60,clock:'周六 20:00',stop:'周六 12:00',start:'周一 09:00',rival:'松字二席'},
    monthly:{name:'每月巨拍',poem:'万宝天会',seal:'天会',limit:24,duration:75,clock:'月末 21:00',stop:'月末 12:00',start:'月初 09:00',rival:'天字一席'}
  };
  const services = [
    {id:'free',name:'散席',room:'散席',price:0,uses:3,cd:20,style:'墨字',desc:'入场即有'},
    {id:'fruit',name:'灵果盘',room:'果席',price:180,uses:8,cd:12,style:'青玉流光',desc:'一盘新摘青梅'},
    {id:'wine',name:'琥珀灵酿',room:'酒席',price:680,uses:18,cd:6,style:'琥珀金辉',desc:'桂花新酿一壶'},
    {id:'room',name:'天字号雅间',room:'天字1号',price:2400,uses:36,cd:3,style:'紫金印纹',desc:'楼上独席'}
  ];
  const goods = [
    ['太虚行剑录','功法','book','地阶上品','剑气过三关，回锋时一息分作两道。',3600],
    ['辟寒青玉佩','法器','pendant','玄阶上品','佩玉遇寒生温，适宜入山历练。',2400],
    ['九转凝元散','丹药','pill','地阶中品','青瓷小瓶蜡封完好，瓶底有丹师押印。',1800],
    ['照影青锋','法器','sword','地阶下品','剑脊渡银，临水可辨剑气虚实。',4200],
    ['山河入梦符','符箓','talisman','玄阶中品','符角封有一粒朱砂，笔势尚清。',900],
    ['紫府青晶','材料','crystal','玄阶上品','矿心通透，适合嵌入灵阵。',1200],
    ['玄水残卷','功法','scroll','玄阶上品','卷上载有水行纳气图，可供参悟。',2100],
    ['春山灵芝','材料','herb','玄阶下品','远山药圃新采，根须留有湿土。',480],
    ['流霞驭火诀','术法','brush','玄阶中品','火行术式以朱笔封存，解封可阅。',1500],
    ['四象聚灵阵盘','阵盘','ring','玄阶上品','四向嵌玉，可开洞府聚灵阵。',2700],
    ['白露回元丹方','丹方','scroll','玄阶中品','纸上留有丹师校过的火候与分量。',1350],
    ['绣云香囊','杂项','purse','玄阶下品','织锦未褪色，囊口以青绳系合。',520],
    ['听泉聚灵阵图','阵图','scroll','玄阶上品','卷上标出八处阵眼，墨色清晰。',1700]
  ].map(([name,type,art,grade,desc,base],id)=>({id,name,type,art,grade,desc,base,rank:({'地':3,'玄':2}[grade[0]]*3+{上:3,中:2,下:1}[grade[2]]),level:[20,16,18,19,13,15,15,12,14,16,15,12,16][id]}));
  const catList=['全部','功法','法器','丹药','符箓','术法','阵法','材料','丹方','杂项'];
  const phaseName={collect:'征集中',preview:'预展中',live:'正在开槌',ended:'已落幕'};
  const initialPhase={daily:'preview',weekly:'collect',monthly:'collect'};
  const nfmt=n=>Number(n).toLocaleString('zh-CN'), esc=s=>window.ui.esc(s);
  const state={tier:'daily',money:68000,bag:[3,1,3,2,4,2,1,5,1,1,1,1,1],category:'全部',sort:'默认',query:'',item:0,fixedID:0,lotIndex:0, modal:'',feedback:'',page:'',board:null,mode:'auction', selectedBid:null,records:[], deliveries:[], lastRefresh:-Infinity};
  const fixed=goods.map((g,i)=>({id:i,g:g.id,price:g.base*2+100,owner:['云岫客','含章','沈舟'][i%3],status:'active'}));
  function lot(g,index=0,owner='阁藏',value=goods[g].base) {
    return {g,index,owner,count:1,bidCount:0,base:value,price:value,step:Math.max(1,Math.floor(value/20)),me:0,held:0,other:0,leader:'',closed:false};
  }
  function createHouse(key) {
    const meta=tiers[key],tierIndex=Object.keys(tiers).indexOf(key);
    return {phase:initialPhase[key],lots:[],submissions:[],index:0,service:0,used:0,lastSpeak:-Infinity,
      serial:0,chats:[],logs:[],timerEnd:0,nextAt:0,receipt:null,defaults:Array.from({length:meta.limit},(_,i)=>lot((i+tierIndex*2)%goods.length,i))};
  }
  const houses=Object.fromEntries(Object.keys(tiers).map(k=>[k,createHouse(k)]));
  function selectBook(house,key) {
    const limit=tiers[key].limit;
    const ranked=house.submissions.slice().sort((a,b)=>goods[b.g].rank-goods[a.g].rank||goods[b.g].level-goods[a.g].level||a.order-b.order);
    house.lots=ranked.slice(0,limit).map((x,i)=>{x.status='入选图录';return lot(x.g,i,'我',x.base);});
    ranked.slice(limit).forEach(x=>{x.status='未入选 · 待取回';holdDelivery(x.g,'未入选退回');});
    while(house.lots.length<limit) {
      const i=house.lots.length;house.lots.push({...house.defaults[i]});
    }
    house.phase='preview';
  }
  selectBook(houses.daily,'daily');
  const h=()=>houses[state.tier],meta=()=>tiers[state.tier],gOf=l=>goods[l.g];
  const current=()=>h().lots[state.lotIndex]||h().lots[h().index];
  // Pick a line from the shared Unity library once per public context. Repaints,
  // animation frames and private edits all keep the same line on the lectern.
  const guideGroups=new Map((window.AuctionGuideLines?.groups||[]).map(group=>[group.key,group.lines]));
  const guideSeed=Math.floor(Math.random()*0x100000000);
  function guide(group,lot=null,count=0){
    const lines=guideGroups.get(group);
    if(!lines?.length)return '';
    const g=lot?gOf(lot):null;
    const values={name:g?.name||meta().name,price:nfmt(lot?.price||0),leader:lot?.leader==='我'?'道友':lot?.leader||'尚无领价',count,limit:meta().limit};
    const context=[state.tier,group,lot?.index,g?.id,lot?.price,lot?.leader,count].join('|');
    let hash=guideSeed;
    for(let i=0;i<context.length;i++)hash=Math.imul(hash^context.charCodeAt(i),16777619);
    const text=lines[(hash>>>0)%lines.length].replace(/\{(name|price|leader|count|limit)\}/g,(_,key)=>values[key]);
    return '<p class="ah-guide" data-guide="'+group+'"><b>司仪</b> '+esc(text)+'</p>';
  }
  const seat=()=>services[h().service],available=s=>Math.max(0,s.uses-h().used);
  const escAttr=esc;
  function button(text,a,v='',cls='',disabled=false){return '<button type="button" class="'+cls+'" data-ah="'+a+'" data-v="'+escAttr(v)+'" '+(disabled?'disabled':'')+'>'+text+'</button>';}
  const open=(page,text,cls='')=>button(text,'go',page,cls);
  const art=(item,size=54)=>window.ui.relic(item.art,size).replace('class="ui-relic"','class="ui-relic" data-illustration="'+item.art+'"');
  function heading(title,side=''){return '<div class="ah-title"><h2>'+title+'</h2>'+side+'</div>';}
  function line(label,value){return '<div class="ah-line"><span>'+label+'</span><strong>'+value+'</strong></div>';}
  function note(s){return '<p class="ah-note">'+s+'</p>';}
  function blank(s){return '<p class="ah-empty">'+s+'</p>';}
  function tag(s,style=''){return '<span class="ah-tag '+style+'">'+s+'</span>';}
  const tiersBar=()=>'<div class="ah-tiers">'+Object.entries(tiers).map(([k,m])=>button(m.name,'tier',k,k===state.tier?'on':'')).join('')+'</div>';
  const time=()=>performance.now()/1000;
  function seconds(){return Math.max(0,Math.ceil(h().timerEnd-time()));}
  const labels={street:'中街坊市',lobby:'拍卖会',room:'鸣钟竞宝',catalog:'预展图录',consign:'填写送拍笺',bid:'竞价委托',sent:'送拍记录',pick:'从纳戒送拍',market:'自由市场',account:'我的交易',item:'珍品详情',buy:'直购确认',bag:'选择上架物品',list:'一口价挂牌',filter:'筛选在售珍品',bids:'我的竞价',posted:'我的挂牌'};
  function nav(){
    return '<header class="ah-bar"><a href="设计总览.html" aria-label="返回设计总览">‹</a><div><span class="ah-brandmark">珍</span>奇珍阁</div><small>试演 · 可用灵石 <b>'+nfmt(state.money)+'</b></small></header>'+
      '<nav class="ah-mode" aria-label="交易入口">'+open('market','自由市场',state.mode==='market'?'sel':'')+open('lobby','拍卖会',state.mode==='auction'?'sel':'')+'</nav>';
  }
  function bottom(){
    const entries=state.mode==='auction'?[['lobby','会期'],['catalog','图录'],['pick','送拍'],['account','我的']]:[['market','逛集'],['filter','筛选'],['bag','上架'],['posted','我的挂牌']];
    const pos={room:'lobby',consign:'pick',bid:'catalog',sent:'account',bids:'account',item:'bag',list:'bag',buy:'market'};
    return '<nav class="ah-foot" aria-label="本馆导航">'+entries.map(([k,t])=>open(k,t,(pos[state.page]||state.page)===k?'sel':'')).join('')+'</nav>';
  }
  function hero(type,title,kicker,metaText){
    return '<section class="ah-hero ah-hero-'+type+'"><div class="ah-heroframe"></div><div class="ah-herotext"><small>'+kicker+'</small><h1>'+title+'</h1><p>'+metaText+'</p></div><span class="ah-tallseal" aria-hidden="true">'+(type==='market'?'通宝':'鉴藏')+'</span></section>';
  }
  function compact(kicker,title,detail){return '<header class="ah-compact"><small>'+kicker+'</small><h1>'+title+'</h1>'+(detail?'<p>'+detail+'</p>':'')+'</header>';}
  function levelMark(phase){return '<span class="ah-phase '+phase+'"><i></i>'+phaseName[phase]+'</span>';}
  function calendar(){
    const p=h().phase;
    const index=['collect','preview','live','ended'].indexOf(p);
    return '<div class="ah-phases">'+['征集','预展','竞拍','结算'].map((x,i)=>'<span class="'+(index===i?'now':index>i?'done':'')+'"><i></i>'+x+'</span>').join('')+'</div>';
  }
  function row(g,small,action,val,end,kind=''){
    return button('<span class="ah-itempic">'+art(g,57)+'</span><span class="ah-itemname"><b>'+g.name+'</b><small>'+small+'</small></span><span class="ah-rowend">'+end+' <i>›</i></span>',action,val,'ah-row '+kind);
  }
  function lobby(){
    const ev=h(),m=meta(),p=ev.phase;
    const group=p==='collect'?ev.defaults:ev.lots;
    return hero('auction',m.poem,'临仙城　／　奇珍阁拍卖会',m.clock+' 开槌　·　'+m.limit+' 件珍藏')+
      '<section class="ah-content">'+tiersBar()+heading('本期会程',levelMark(p))+calendar()+
      '<div class="ah-schedule">'+line('封册公示',m.stop)+line('鸣钟开槌',m.clock)+'</div>'+
      '<div class="ah-actions">'+(p==='collect'?open('pick','择宝送拍','ah-primary'):p==='live'?open('room','进入竞价现场','ah-primary'):open('catalog',p==='ended'?'查看成交图录':'翻阅预展图录','ah-primary'))+button('会规与试演','rules','','ah-secondary')+'</div>'+
      (p==='preview'?'<div class="ah-preview-links">'+open('room','入席候拍','ah-link')+button('预订本场席面','services','','ah-link')+'</div>':'')+
      heading(p==='collect'?'拟备阁藏':'本场珍赏',open('catalog','查看图录 ›','ah-link'))+
      '<div class="ah-showcase">'+group.slice(0,3).map((l,i)=>button('<span class="ah-lotno">LOT '+String(i+1).padStart(2,'0')+'</span>'+art(gOf(l),64)+'<b>'+gOf(l).name+'</b>','detail',i,'ah-tile')).join('')+'</div>'+
      (p==='ended'?note('槌落灯收，请在图录查阅各件成交价。'):p==='live'?note('楼上灯影已开，道友可入场举牌。'):guide(p==='collect'?'collect':'preview',null,p==='collect'?ev.submissions.length:ev.lots.length))+'</section>';
  }
  function catalog(){
    const ev=h(),m=meta(),p=ev.phase;
    return compact(m.poem+'　／　'+phaseName[p],p==='collect'?'征集簿':'拍品图录',m.clock+' 开槌　·　取前 '+m.limit+' 件')+
      '<section class="ah-content">'+tiersBar()+calendar()+(p==='collect'?guide('collect',null,ev.submissions.length):p==='preview'?guide('preview',null,ev.lots.length):'')+
      (p==='collect'?heading('已送本场',tag(ev.submissions.length+' 件'))+(ev.submissions.length?ev.submissions.map(x=>row(goods[x.g],'我的送拍 · '+x.status,'go','sent',nfmt(x.base))).join(''):blank('尚无玩家送拍。封册时由阁藏补齐余席。'))+open('pick','从纳戒择宝送拍','ah-primary ah-full'):
       heading('拍号　／　珍品',tag('灵石计价'))+ev.lots.map((l,i)=>row(gOf(l),'拍号 '+String(i+1).padStart(2,'0')+' · '+l.owner+' · '+(l.closed?'已落槌':p==='live'&&ev.index===i?'台上竞价':l.leader?'已有人留价':'待开槌'),'detail',i,nfmt(l.price))).join(''))+
      button('会规与试演','rules','','ah-link ah-rulelink')+'</section>';
  }
  const languageByType={
    功法:['openBook','品卷','这门功法，可有人参过相近的路数？'],
    法器:['openGear','观器','我想再看看这件法器的纹路。'],
    丹药:['openPill','论丹','这炉丹药，哪位道友识得？'],
    符箓:['openTalisman','赏符','好干净的符脚，我再看一眼。'],
    材料:['openMaterial','验材','这份材料的成色很合我的用处。'],
    术法:['openBook','论法','有修这一路术法的道友在席么？'],
    阵盘:['openArray','看阵','此阵的落眼处，值得细看。'],
    阵图:['openRecipe','读图','这幅图上的阵眼，我想细看。'],
    丹方:['openRecipe','看方','这张丹方的火候，有道友试过么？'],
    杂项:['openOther','赏物','这件东西的来历，哪位道友知道？']
  };
  function houseGuide(l,receipt){
    const remaining=seconds();
    let group;
    if(receipt)group=l.leader?'sold':'passed';
    else if(remaining<=9&&l.leader)group=remaining<=3?'finalCall':remaining<=6?'secondCall':'firstCall';
    else if(l.bidCount)group=l.bidCount===1?'firstBid':'leading';
    else group=languageByType[gOf(l).type]?.[0]||'openOther';
    return guide(group,l,l.count);
  }
  function suggestions(l){
    const g=gOf(l),[,label,words]=languageByType[g.type]||languageByType['杂项'];
    const statePhrase=l.leader==='我'?['守价','此件我已举牌，且听下一声。']:l.me?['追价','这件已过我的留价，容我再想一想。']:l.leader?['观价','现在是 '+nfmt(l.price)+' 灵石，我先看一轮。']:['问价','哪位道友先举牌？我也在看。'];
    return [[label,g.name+'，'+words],statePhrase];
  }
  function chatComposer(l,receipt){
    if(receipt)return '<small class="ah-counter">本拍号已落槌，传音随下一件开放。</small>';
    return '<div class="ah-phrases" aria-label="应景传音短句"><small>应景短句</small>'+suggestions(l).map(([label,words])=>button(esc(label),'phrase',words)).join('')+'</div>'+
      '<form data-ahchat="'+l.index+'" class="ah-chatform"><input maxlength="40" aria-label="传音内容" placeholder="选句或自拟，确认后传音" value="'+esc(l.messageDraft||'')+'"><button type="submit" class="ah-secondary">确认传音</button></form><small class="ah-counter">'+seat().style+' · 剩 '+available(seat())+' 次 · 冷却 '+seat().cd+' 秒</small>';
  }
  function room(){
    const ev=h();
    if(ev.phase!=='live'&&!ev.receipt)return compact(meta().poem,'鸣钟竞宝',phaseName[ev.phase])+'<section class="ah-content">'+(ev.phase==='ended'?note('本场已结束，可在图录复看各件成交价。'):guide('liveWait',null,ev.lots.length))+open('catalog','翻阅图录','ah-primary')+button('会规与试演','rules','','ah-secondary')+'</section>';
    const l=ev.receipt?.lot||ev.lots[ev.index],g=gOf(l);
    const messages=ev.chats.filter(x=>x.lot===l.index);
    const latest=messages.at(-1), elapsed=latest?time()-latest.at:Infinity;
    const ribbon=!ev.receipt&&elapsed<6?'<span class="tone'+latest.tone+'" style="animation-delay:-'+Math.max(0,elapsed).toFixed(2)+'s">'+esc(latest.who)+'　'+esc(latest.text)+'</span>':'';
    const lotNum=String(l.index+1).padStart(2,'0'),receipt=ev.receipt;
    return '<section class="ah-stage '+(receipt?'struck':'')+'"><div class="ah-stage-top"><span>'+meta().poem+'</span><span>'+ (receipt?'已落槌':'● 正在竞价')+'</span></div>'+
      '<div class="ah-light"></div><div class="ah-orbit"><i></i><i></i></div><div class="ah-hover-item">'+art(g,120)+'</div>'+
      '<div class="ah-fx"></div><div class="ah-flying" aria-hidden="true">'+ribbon+'</div>'+
      '<div class="ah-stagetext"><small>LOT '+lotNum+'　／　'+String(ev.lots.length).padStart(2,'0')+'</small><h1>'+g.name+'</h1><span>'+g.grade+'　·　'+(l.owner==='阁藏'?'奇珍阁藏':l.owner+'送藏')+'</span></div></section>'+
      '<section class="ah-content ah-roomcontent"><div class="ah-price"><div><small>'+(receipt?'落槌价':'台上现价')+'</small><strong>'+nfmt(l.price)+'</strong><span>灵石</span></div><div>'+tag(receipt?receipt.result:(l.leader||'尚无人举牌'))+'<small>'+(receipt?'本件已结算':'余 <i data-clock>'+seconds()+'</i> 秒')+'</small></div></div>'+
      houseGuide(l,receipt)+(receipt&&l.delivery!=null&&!state.deliveries[l.delivery].claimed?button('领取'+g.name,'claim',l.delivery,'ah-primary ah-full'):'')+(receipt?button(ev.phase==='ended'?'查看全场图录':'登台下一件','next','','ah-primary ah-full'):button('举牌　'+nfmt(l.leader?l.price+l.step:l.base)+' 灵石','livebid','','ah-primary ah-full',l.owner==='我'||l.leader==='我'))+
      heading('席间传音',button(seat().name+' · 添置席面 ›','services','','ah-link'))+
      '<div class="ah-chats">'+(messages.slice(-3).map(x=>'<p><b class="tone'+x.tone+'">'+esc(x.who)+'</b> '+esc(x.text)+'</p>').join('')||'<p>席间安静，等候第一声传音。</p>')+'</div>'+
      chatComposer(l,receipt)+
      button('会规与试演','rules','','ah-link ah-rulelink')+'</section>';
  }
  function fixedView(){
    const q=state.query.trim();
    const subset=fixed.filter(x=>x.status==='active'&&(state.category==='全部'||(state.category==='阵法'?['阵盘','阵图'].includes(goods[x.g].type):goods[x.g].type===state.category))&&(!q||goods[x.g].name.includes(q)));
    if(state.sort==='价低优先')subset.sort((a,b)=>a.price-b.price);
    if(state.sort==='价高优先')subset.sort((a,b)=>b.price-a.price);
    return subset;
  }
  const categoryBar=()=>'<div class="ah-categories" aria-label="物品分类">'+catList.map(v=>button(v,'cat',v,v===state.category?'on':'')).join('')+'</div>';
  const stalls=[['神符阁','符箓','talisman'],['灵丹坊','丹药','pill'],['百器坊','法器','sword'],['道经阁','功法','book'],['术法阁','术法','brush'],['杂货铺','材料','crystal'],['阵法阁','阵法','ring']];
  function street(){
    return hero('market','中街坊市','临仙城　／　东市','沿街寻货　循类入阁')+
      '<section class="ah-content"><div class="ah-market-tools">'+open('market','逛自由市场','ah-primary')+open('lobby','赴奇珍拍会','ah-secondary')+'</div>'+
      heading('沿街商阁')+'<div class="ah-stalls">'+stalls.map(([name,category,icon])=>button(art({art:icon},46)+'<span><b>'+name+'</b><small>'+category+'</small></span>','stall',category)).join('')+'</div></section>';
  }
  function market(){
    const list=fixedView();
    return hero('market','临仙坊市','临仙城　／　奇珍阁自由市场','玩家挂牌　一口成交　常年开市')+
      '<section class="ah-content"><div class="ah-market-tools">'+open('bag','我来上架','ah-primary')+open('posted','我的挂牌','ah-secondary')+'</div>'+guide('market',null,list.length)+
      heading('在售珍品',open('filter','搜寻 / 排序 ›','ah-link'))+categoryBar()+
      '<div class="ah-fixedrows">'+(list.map(x=>row(goods[x.g],x.owner+' · '+goods[x.g].grade,'viewFixed',x.id,nfmt(x.price))).join('')||blank('此分类暂时没有在售珍品。'))+'</div>'+
      button('刷新货架','refresh','','ah-link ah-rulelink')+open('street','逛中街各阁','ah-link ah-rulelink')+'</section>';
  }
  function filterView(){return compact('奇珍阁　／　自由市场','寻宝','按类别、名称与价格翻查当期在售珍品。')+
    '<form class="ah-content ah-form" data-ahfilter><label>拍品名称<input name="query" maxlength="30" value="'+escAttr(state.query)+'" placeholder="输入物品名"></label><label>类别<select name="category">'+catList.map(v=>'<option '+(v===state.category?'selected':'')+'>'+v+'</option>').join('')+'</select></label><label>排序<select name="sort">'+['默认','价低优先','价高优先'].map(v=>'<option '+(v===state.sort?'selected':'')+'>'+v+'</option>').join('')+'</select></label><button class="ah-primary">查看在售珍品</button></form>';}
  function bag(sending){
    return compact(sending?meta().poem+'　／　自愿送拍':'常驻自由市场　／　玩家挂牌',sending?'择宝送拍':'纳戒上架',
      sending?'从纳戒选一件送入本场征集簿。':'自定售价。买家确认后，一口价成交。')+
      '<section class="ah-content">'+(sending?tiersBar()+line('当前会期',levelMark(h().phase)):'')+
      heading('纳戒内可交易物品',tag('按件'))+
      (sending&&h().phase!=='collect'?note('本期征集已截止。可切换仍在征集的会期。'):'')+
      goods.filter(g=>state.bag[g.id]>0).map(g=>row(g,g.type+' · '+g.grade,'chooseGoods',g.id,'存 '+state.bag[g.id])).join('')+'</section>';
  }
  function identify(g,extra){return '<div class="ah-identify"><div class="ah-iconmat">'+art(g,94)+'</div><div><small>'+g.grade+'　／　'+g.type+'</small><h2>'+g.name+'</h2><p>'+g.desc+'</p></div></div>'+(extra||'');}
  function itemInspect(){
    const g=goods[state.item];
    return compact('奇珍阁　／　纳戒鉴物','珍品资料','看清物品与库存，再决定是否挂牌。')+
      '<section class="ah-content">'+identify(g)+line('品类',g.type)+line('品阶',g.grade)+line('纳戒库存',state.bag[g.id]+' 件')+open('list','填写挂牌价','ah-primary ah-full')+'</section>';
  }
  function orderForm(sending){
    const g=goods[state.item],m=meta(),p=h().phase;
    return compact(sending?m.poem:'奇珍阁　／　自由市场',sending?'缮写送拍笺':'一口价挂牌',sending?'封册后公布入选拍号。':'挂牌后，其他道友可直接购买。')+
      '<section class="ah-content">'+identify(g)+(sending?line('收件截止',m.stop)+line('选录名额','前 '+m.limit+' 件')+line('成交佣金','成交价的 5%'):line('寄出数量','1 件')+line('上架手续费','标价的 10%（本例）'))+
      '<form data-ahorder="'+(sending?'consign':'fixed')+'" class="ah-form"><label>'+(sending?'起拍价':'一口价')+'（灵石）<input type="number" name="amount" required min="100" max="1000000000" step="1" value="'+g.base+'"></label><button '+((sending&&p!=='collect')||!state.bag[g.id]?'disabled':'')+' class="ah-primary">'+(sending?'核对送拍':'核对挂牌')+'</button></form>'+
      (sending?note('按品阶、等级、提交先后选录。未入选和流拍可在交易簿取回。'):note('仅从本次纳戒中取走一件。下架可取回在售物品。'))+'</section>';
  }
  function fixedDetail(){
    const x=fixed.find(v=>v.id===state.fixedID)||fixed[0],g=goods[x.g];
    return compact('自由市场　／　一口价','查验货品','物品由卖家寄存在奇珍阁。')+'<section class="ah-content">'+identify(g)+line('货主',x.owner)+line('标价',nfmt(x.price)+' 灵石')+line('当前可用',nfmt(state.money)+' 灵石')+
      (x.owner==='我'?open('posted','管理我的挂牌','ah-secondary ah-full'):button(x.status==='active'?'购入此物':'此物已售','buyFixed',x.id,'ah-primary ah-full',x.status!=='active'))+'</section>';
  }
  function bidForm(){
    const ev=h(),l=current();
    if(!l)return catalog();
    const g=gOf(l),p=ev.phase,eligible=!l.closed&&l.owner!=='我'&&(p==='preview'||p==='live'&&ev.index===l.index);
    const min=l.me>0?l.me+l.step:l.leader?l.price+l.step:l.base;
    return compact(meta().poem+'　／　拍号 '+String(l.index+1).padStart(2,'0'),'竞价笺',l.closed?'此件已落槌。':p==='live'?'现场按当前价举牌，亦可提高委托上限。':'公开图录展示现价。最高委托额仅自己可见。')+
      '<section class="ah-content">'+identify(g)+(eligible?guide('proxy',l,l.count):'')+line('现价',nfmt(l.price)+' 灵石')+line('加价幅度',nfmt(l.step)+' 灵石')+
      line('我的委托上限',l.me?nfmt(l.me)+' 灵石':'尚未出价')+line('已托管',nfmt(l.held)+' 灵石')+
      (eligible?'<form class="ah-form" data-ahproxy><label>最高委托额（灵石）<input name="amount" required type="number" min="'+min+'" step="1" value="'+Math.max(min,l.base+l.step*3)+'"></label><button class="ah-primary">核对委托金额</button></form>':note(l.owner==='我'?'此件为自己的拍品。':l.closed?'落槌后已结算。':'尚未轮到本件，现场竞价将在此件登台时开放。'))+
      open(p==='live'?'room':'catalog',p==='live'?'返回现场':'返回图录','ah-link ah-rulelink')+'</section>';
  }
  function account(sub){
    let body;
    if(sub==='posted'){
      const list=fixed.filter(x=>x.owner==='我').reverse();
      body=heading('我的挂牌',tag(list.length+' 件'))+(list.length?list.map(x=>row(goods[x.g],nfmt(x.price)+' 灵石 · '+(x.status==='active'?'在售':'已取回'),x.status==='active'?'delist':'noop',x.id,x.status==='active'?'下架':'已取回')).join(''):blank('你还没有挂牌。去纳戒挑选第一件要出售的物品。'))+open('bag','我来上架','ah-primary ah-full');
    }else if(sub==='sent'){
      const entries=Object.entries(houses).flatMap(([k,house])=>house.submissions.map(x=>[k,x]));
      body=heading('自愿送拍',tag(entries.length+' 笺'))+(entries.length?entries.map(([k,x])=>row(goods[x.g],tiers[k].name+' · 起拍 '+nfmt(x.base),'sentRecord',k,x.status)).join(''):blank('征集期间可从纳戒送拍。'))+open('pick','选择送拍物品','ah-primary ah-full');
    }else if(sub==='bids'){
      const entries=Object.entries(houses).flatMap(([k,house])=>house.lots.filter(x=>x.me).map(l=>[k,l]));
      body=heading('我的留价',tag(entries.length+' 件'))+(entries.length?entries.map(([k,l])=>row(gOf(l),tiers[k].name+' · 上限 '+nfmt(l.me),'bidRecord',k+'-'+l.index,l.closed?(l.leader==='我'?'已获得':'已结束'):l.leader==='我'?'领先':'被超价')).join(''):blank('看中拍品后，可以在预展图录留下最高出价。'))+open('catalog','翻阅本场图录','ah-primary ah-full');
    }else body=heading('待领寄存',tag(state.deliveries.filter(x=>!x.claimed).length+' 件'))+
      (state.deliveries.some(x=>!x.claimed)?state.deliveries.filter(x=>!x.claimed).map(x=>row(goods[x.g],x.kind,'claim',x.id,'收入纳戒')).join(''):blank('当前没有寄存待领的珍品。'))+heading('交易簿')+open('bids','我的竞价　›','ah-menu')+open('sent','送拍记录　›','ah-menu')+open('posted','自由市场挂牌　›','ah-menu')+heading('灵石往来')+(state.records.length?state.records.slice(-12).reverse().map(x=>line(x[0],x[1])).join(''):blank('尚无成交记录。'));
    return compact('临仙城　／　个人交易簿',labels[sub],'可用灵石 '+nfmt(state.money))+'<section class="ah-content">'+body+'</section>';
  }
  const modalTitles={rules:'会规与试演',services:'席间款待',bid:'核对出价',fixed:'确认挂牌',consign:'确认送拍',purchase:'核对购买',delist:'收回挂牌',service:'核对席面',prize:'已收入纳戒',status:'阁中回执'};
  function ruleBody(){
    const m=meta(),p=h().phase;
    return line('收件',m.start)+line('截册与预展',m.stop)+line('开槌',m.clock)+line('席次','前 '+m.limit+' 件，每件 '+m.duration+' 秒')+
      note('时间均为东八区。按品阶、等级、提交先后选录，阁藏补齐空位。自愿送拍成交收取 5% 佣金。')+
      note('提前委托须交足上限。有人竞价时逐档代举，超价即解冻，得标退回差额。同额由先到者领先。')+
      note('这是本地试演，可快进到下一阶段。自由市场一直开放。')+
      (p==='live'&&!h().receipt?button('试演邻席举牌','neighbor','','ah-secondary ah-full'):'')+
      (p!=='ended'&&(!h().receipt)?button({collect:'试演至截册预展',preview:'试演至鸣钟开场',live:'试演至本件落槌'}[p],'advance','','ah-primary ah-full'):'');
  }
  function servicesBody(){
    return line('当前席次',seat().room)+services.map((s,i)=>{
      const diff=s.price-seat().price;
      return button('<span><b>'+s.name+'</b><small>'+s.style+' · '+s.uses+' 次 · '+s.cd+' 秒</small></span><strong>'+(i===h().service?'本席':diff>0?'补 '+nfmt(diff):'已越席')+'</strong>','chooseSeat',i,'ah-seat tone'+i,i<=h().service);
    }).join('')+note('购置后本场生效，已用次数保留，房号随席次显示。');
  }
  let draft=null, reward=null, flash='', flashTimer;
  function modalBody(){
    const a=state.modal,d=draft;
    if(a==='rules')return ruleBody();if(a==='services')return servicesBody();
    if(a==='prize')return '<div class="ah-prize"><div class="ah-prize-rays"></div><div class="ah-fx"></div><span class="ah-prize-seal" aria-hidden="true">入藏</span><div class="ah-prizepic">'+art(reward.g,104)+'</div><h2>'+reward.g.name+'</h2><p>'+reward.message+'</p></div>'+button('收好宝物','close','','ah-primary ah-full');
    if(a==='status')return note(state.feedback)+button('知道了','close','','ah-primary ah-full');
    if(!d)return '';
    const summary={bid:'托管上限 '+nfmt(d.value)+' 灵石，本次冻结 '+nfmt(d.extra)+' 灵石。',fixed:'一件寄存在阁，挂牌价 '+nfmt(d.value)+' 灵石。本次收取上架费 '+nfmt(d.fee)+' 灵石。',consign:'一件寄入本场，起拍价 '+nfmt(d.value)+' 灵石。成交后从卖款扣 5% 佣金。',purchase:'一口价购入一件，本次扣除 '+nfmt(d.value)+' 灵石。',service:'本场升级为'+(d.s?.name||'')+'，本次扣除 '+nfmt(d.value)+' 灵石。',delist:'将此件从自由市场取回纳戒，上架费已收取。'};
    return '<p class="ah-confirmname">'+(d.g?d.g.name:d.s?.name||'')+'</p>'+note(summary[a])+
       '<div class="ah-actions">'+button('再看看','close','','ah-secondary')+button('确认'+({bid:'出价',fixed:'挂牌',consign:'送拍',purchase:'购买',service:'入席',delist:'下架'}[a]||''),'commit','','ah-primary')+'</div>';
  }
  function overlay(){return state.modal?'<dialog class="ah-dialog" aria-label="'+modalTitles[state.modal]+'"><div class="ah-dialoghead"><span>奇珍阁</span>'+button('×','close','','ah-close')+'</div><h2>'+modalTitles[state.modal]+'</h2>'+modalBody()+'</dialog>':'';}
  function specimen(kind){
    const entry={lot:['拍卖图录','拍号 01 · 阁藏提供',0],info:['珍品属性','地阶上品 · 鉴宝笺',0],bid:['委托留价','上限 4,680 · 现价 3,600',0],posted:['挂牌记录','在售 · 由我寄存',1],fixed:['市场在售','云岫客 · 一口价',3],bag:['纳戒选物','存 3 件 · 可上架',2]}[kind];
    return compact('奇珍阁　／　'+entry[0],state.board.title,'')+'<section class="ah-content">'+row(goods[entry[2]],entry[1],'go',kind==='lot'?'catalog':'market','查阅')+identify(goods[entry[2]])+'</section>';
  }
  function draw(){
    const k=state.page;
    const body=components[state.board.id]?specimen(components[state.board.id]):{
      street,lobby,room,catalog,bid:bidForm,market,filter:filterView,buy:fixedDetail,pick:()=>bag(true),bag:()=>bag(false),item:itemInspect,consign:()=>orderForm(true),list:()=>orderForm(false),...Object.fromEntries(['account','posted','sent','bids'].map(v=>[v,()=>account(v)]))
    }[k]();
    queueMicrotask(()=>{const dialog=document.querySelector('.ah-dialog');if(dialog&&!dialog.open)dialog.showModal();});
    return '<article class="ah-house" data-page="'+k+'">'+nav()+'<div class="ah-scroll">'+body+'</div>'+bottom()+(flash?'<div class="ah-toast" role="status">'+esc(flash)+'</div>':'')+overlay()+'</article>';
  }
  function showLive(ev){
    if(!ev.lots.length)selectBook(ev,state.tier);
    ev.phase='live';ev.receipt=null;ev.index=0;ev.timerEnd=time()+meta().duration;
  }
  window.auctionHouseDesign=(d)=>{
    if(!screens[d.id]&&!components[d.id])return null;
    if(state.board?.id!==d.id){state.modal='';draft=null;flash='';}
    const direct=!state.board;state.board=d;state.page=screens[d.id]||'component';
    if(['lobby','room','catalog','consign','bid','sent','pick','bids'].includes(state.page))state.mode='auction';
    else if(state.page!=='account')state.mode='market';
    if(direct&&state.page==='room')showLive(h());
    if(direct&&['consign','pick'].includes(state.page))state.tier='weekly';
    return draw();
  };
  function redraw(scrollTop=false){
    if(!document.querySelector('.ah-house'))return;
    const container=document.querySelector('#screen'),previous=document.querySelector('.ah-scroll')?.scrollTop||0;
    container.innerHTML=draw();if(!scrollTop)container.querySelector('.ah-scroll').scrollTop=previous;
  }
  function go(key){
    state.modal='';draft=null;
    if(W[key]===state.board?.id){redraw(true);return;}
    location.hash=W[key];
  }
  function tell(message){flash=message;clearTimeout(flashTimer);flashTimer=setTimeout(()=>{flash='';document.querySelector('.ah-toast')?.remove();},3100);}
  function reject(message){state.feedback=message;state.modal='status';redraw();}
  function sparkle(){
    requestAnimationFrame(()=>{const target=document.querySelector('.ah-prize .ah-fx,.ah-stage .ah-fx');if(target){target.classList.remove('play');void target.offsetWidth;target.classList.add('play');}});
  }
  function prize(g,message){reward={g,message};state.modal='prize';redraw();sparkle();}
  function ledger(label,amount){state.records.push([label,(amount>=0?'+':'−')+nfmt(Math.abs(amount))+' 灵石']);}
  function holdDelivery(g,kind){
    const id=state.deliveries.length;
    state.deliveries.push({id,g,kind,claimed:false});return id;
  }
  function claimDelivery(id){
    const delivery=state.deliveries[id];if(!delivery||delivery.claimed)return reject('此件已从寄存处取回。');
    delivery.claimed=true;state.bag[delivery.g]++;
    prize(goods[delivery.g],delivery.kind+' · 已取出一件');
  }
  function settle(ev){
    if(ev.phase!=='live'||ev.receipt)return;
    const l=ev.lots[ev.index];
    l.closed=true;
    const won=l.leader==='我';let result='流拍';
    if(won){state.money+=l.held-l.price;l.held=0;l.delivery=holdDelivery(l.g,'竞拍得宝');ledger('得标 '+gOf(l).name,-l.price);result='你已得标';}
    else if(l.leader){result=l.leader+'得标';if(l.owner==='我'){const proceeds=l.price-Math.floor(l.price*5/100);state.money+=proceeds;ledger('送拍成交 '+gOf(l).name,proceeds);}}
    else if(l.owner==='我')l.delivery=holdDelivery(l.g,'流拍退回');
    if(l.owner==='我'){const x=ev.submissions.find(s=>s.g===l.g&&s.status==='入选图录');if(x)x.status=l.leader?'已成交':'流拍 · 待取回';}
    ev.receipt={lot:l,result};ev.nextAt=time()+6;if(ev.index===ev.lots.length-1)ev.phase='ended';
    if(ev===h()&&state.page==='room'&&document.querySelector('.ah-house')){state.modal='';redraw();sparkle();}
  }
  function neighboringBid(){
    const ev=h(),l=ev.lots[ev.index];
    if(ev.phase!=='live'||ev.receipt||!l||!seconds())return reject('本件已止价。');
    const oldLeader=l.leader,oldPrice=l.price;
    l.other=l.leader?l.price+l.step:l.base;
    visiblePrice(l);
    if(oldLeader!==l.leader||oldPrice!==l.price)l.bidCount++;
    if(oldLeader==='我'&&l.leader!=='我'){state.money+=l.held;l.held=0;tell('已被邻席超价，委托款已退。');}
    ev.logs.push([l.index,l.price]);
    state.modal='';redraw();sparkle();
  }
  function nextLot(ev=h(),key=state.tier){
    if(!ev.receipt)return;
    if(ev.phase==='ended'){if(ev===h()){ev.receipt=null;go('catalog');}return;}
    ev.receipt=null;ev.index++;ev.timerEnd=time()+tiers[key].duration;
    if(ev===h()&&state.page==='room'&&document.querySelector('.ah-house')){
      state.lotIndex=ev.index;
      if(state.modal!=='prize')state.modal='';
      redraw(true);sparkle();
    }
  }
  function visiblePrice(l){
    if(!l.other&&!l.me){l.price=l.base;l.leader='';return;}
    if(l.other>l.me||l.other===l.me&&l.leader!=='我'){
      l.leader=meta().rival;l.price=l.me?Math.min(l.other,l.me+l.step):l.base;
    }else{l.leader='我';l.price=l.other?Math.min(l.me,l.other+l.step):l.base;}
  }
  function prepareBid(value,l){
    const ev=h();
    if(!l||l.closed||l.owner==='我'||!(ev.phase==='preview'||ev.phase==='live'&&!ev.receipt&&ev.index===l.index&&seconds()>0))return reject('本件当前不可出价。');
    const min=l.me?Math.max(l.me+1,l.price+(l.leader==='我'?0:l.step)):l.leader?l.price+l.step:l.base;
    if(!Number.isSafeInteger(value)||value<min||value>1e9)return reject('请填写 '+nfmt(min)+' 至 1,000,000,000 灵石的整数。');
    const extra=value-l.held;
    if(extra>state.money)return reject('灵石不足以托管这次最高出价。');
    draft={value,extra,l,g:gOf(l)};state.modal='bid';redraw();
  }
  function commit(){
    const a=state.modal,d=draft;if(!d)return;
    const ev=h();
    if(a==='bid'){
      const l=d.l;
      if(l.closed||ev.phase==='live'&&(ev.receipt||ev.index!==l.index||!seconds())||d.value<=l.me||d.extra>state.money)return reject('拍品状态已变动，请重新核价。');
      const oldLeader=l.leader,oldPrice=l.price;
      state.money-=d.extra;l.held=d.value;l.me=d.value;visiblePrice(l);
      if(oldLeader!==l.leader||oldPrice!==l.price)l.bidCount++;
      if(l.leader!=='我'){state.money+=l.held;l.held=0;}
      if(ev.phase==='live')ev.logs.push([l.index,l.price]);
      tell(l.leader==='我'?'阁中已记价，你暂时领先。':'委托额已被超过，托管已退还。');
    }else if(a==='fixed'||a==='consign'){
      if(state.bag[d.g.id]<1||a==='fixed'&&d.fee>state.money||a==='consign'&&ev.phase!=='collect')return reject('物品、余额或会期已变化，请重新核对。');
      state.bag[d.g.id]--;
      if(a==='fixed'){state.money-=d.fee;fixed.push({id:fixed.length,g:d.g.id,owner:'我',price:d.value,status:'active'});ledger('挂牌费 '+d.g.name,-d.fee);go('posted');return;}
      ev.submissions.push({g:d.g.id,base:d.value,order:ev.submissions.length,status:'待选录'});go('sent');return;
    }else if(a==='purchase'){
      const x=fixed.find(x=>x.id===d.id);
      if(!x||x.status!=='active'||x.owner==='我'||x.price>state.money)return reject('当前余额不足或货品已售出。');
      x.status='sold';state.money-=x.price;state.bag[x.g]++;ledger('买入 '+d.g.name,-x.price);draft=null;prize(d.g,'一口价 '+nfmt(x.price)+' 灵石 · 数量一件');return;
    }else if(a==='delist'){
      const x=fixed.find(x=>x.id===d.id);if(!x||x.status!=='active'||x.owner!=='我')return reject('此条挂牌已结束。');
      x.status='withdrawn';state.bag[x.g]++;tell('货品已取回纳戒。');
    }else if(a==='service'){
      if(!['preview','live'].includes(ev.phase)||ev.receipt||d.i<=ev.service||d.value>state.money)return reject('当前无法升级席次。');
      state.money-=d.value;ledger(d.s.name,-d.value);ev.service=d.i;tell('已入'+d.s.room+'。');
    }
    state.modal='';draft=null;redraw();if(a==='bid')sparkle();
  }
  function postChat(input){
    const ev=h(),text=input.value.trim(),s=seat();
    if(!text)return;
    if(ev.phase!=='live'||ev.receipt||ev.index!==Number(input.form.dataset.ahchat)||seconds()<=0)return reject('请在当前拍品竞价期间传音。');
    if(!available(s))return reject('本场传音次数已用尽。');
    const cd=Math.ceil(s.cd-(time()-ev.lastSpeak));if(cd>0)return reject('传音尚需调息 '+cd+' 秒。');
    if(text.length>40)return reject('传音最多四十字。');
    const item=ev.lots[ev.index];item.messageDraft='';ev.chats.push({lot:item.index,at:time(),who:s.room,tone:ev.service,text});ev.used++;ev.lastSpeak=time();redraw();

  }
  document.addEventListener('input',event=>{
    const input=event.target;
    if(input.matches('.ah-house [data-ahchat] input'))h().lots[Number(input.form.dataset.ahchat)].messageDraft=input.value;
  });
  document.addEventListener('keydown',event=>{
    if(!event.isComposing&&event.key==='Enter'&&event.target.matches('.ah-house [data-ahchat] input'))event.preventDefault();
  });
  document.addEventListener('submit',event=>{
    const form=event.target;if(!form.closest('.ah-house'))return;event.preventDefault();
    if(form.matches('[data-ahchat]'))return postChat(form.querySelector('input'));
    const values=new FormData(form);
    if(form.matches('[data-ahfilter]')){state.category=values.get('category');state.sort=values.get('sort');state.query=values.get('query').trim();go('market');return;}
    const value=Number(values.get('amount'));
    if(form.matches('[data-ahproxy]'))return prepareBid(value,current());
    if(form.matches('[data-ahorder]')){
      if(!Number.isSafeInteger(value)||value<100||value>1e9)return reject('起价须为 100 至 1,000,000,000 的整数。');
      const kind=form.dataset.ahorder,fee=kind==='fixed'?Math.max(1,Math.floor(value*.10)):0;
      if(fee>state.money)return reject('当前灵石不足以支付挂牌费。');
      draft={value,fee,g:goods[state.item]};state.modal=kind;redraw();
    }
  });
  document.addEventListener('cancel',event=>{if(event.target.matches('.ah-dialog')){event.preventDefault();state.modal='';draft=null;redraw();}},true);
  document.addEventListener('click',event=>{
    const b=event.target.closest('.ah-house [data-ah]');if(!b||b.disabled)return;
    const a=b.dataset.ah,v=b.dataset.v,i=Number(v),ev=h();
    if(a==='go'){go(v);return;}
    if(a==='noop')return;
    if(a==='tier'){state.tier=v;state.lotIndex=0;redraw(true);return;}
    if(a==='rules'||a==='services'){state.modal=a;redraw();return;}
    if(a==='close'){state.modal='';draft=null;redraw();return;}
    if(a==='commit'){commit();return;}
    if(a==='claim'){claimDelivery(i);return;}
    if(a==='phrase'){
      const input=document.querySelector('.ah-house [data-ahchat] input'),l=ev.lots[ev.index];
      if(!input||!l||ev.receipt||Number(input.form.dataset.ahchat)!==ev.index)return;
      l.messageDraft=v;input.value=v;input.focus();input.setSelectionRange(v.length,v.length);
      return;
    }
    if(a==='cat'){state.category=v;redraw();return;}
    if(a==='refresh'){const wait=Math.max(0,Math.ceil(15-(time()-state.lastRefresh)));tell(wait?'货架已更新，'+wait+' 秒后可再刷新。':'已重新查看在售珍品。');if(!wait)state.lastRefresh=time();redraw();return;}
    if(a==='stall'){state.category=v;state.query='';go('market');return;}
    if(a==='chooseGoods'){
      if(state.page==='pick'&&ev.phase!=='collect')return reject('本期已截册，请切换仍在征集的周会或月会。');
      state.item=i;go(state.page==='pick'?'consign':'item');return;
    }
    if(a==='viewFixed'){state.fixedID=i;go('buy');return;}
    if(a==='buyFixed'||a==='delist'){
      const x=fixed.find(x=>x.id===i);if(!x)return;
      draft={id:i,value:x.price,g:goods[x.g]};state.modal=a==='buyFixed'?'purchase':'delist';redraw();return;
    }
    if(a==='advance'){
      state.modal='';if(ev.phase==='collect'){selectBook(ev,state.tier);go('catalog');}
      else if(ev.phase==='preview'){showLive(ev);go('room');}
      else if(ev.phase==='live'){settle(ev);if(state.page!=='room')go('room');}
      return;
    }
    if(a==='detail'){
      if(ev.phase==='collect'){tell('阁藏候选，截册后可委托竞价。');redraw();return;}
      state.lotIndex=i;go('bid');return;
    }
    if(a==='sentRecord'){state.tier=v;state.lotIndex=0;go('catalog');return;}
    if(a==='bidRecord'){const [k,num]=v.split('-');state.tier=k;state.lotIndex=Number(num);go('bid');return;}
    if(a==='livebid'){const l=ev.lots[ev.index];prepareBid(l.leader?l.price+l.step:l.base,l);return;}
    if(a==='next'){nextLot();return;}
    if(a==='neighbor'){neighboringBid();return;}
    if(a==='chooseSeat'){const s=services[i];draft={s,i,value:s.price-seat().price};state.modal='service';redraw();}
  });
  // Only change the remaining seconds in place, preserving text fields and focus.
  setInterval(()=>{
    for(const [key,ev] of Object.entries(houses)){
      if(ev.phase!=='live')continue;
      if(ev.receipt){if(time()>=ev.nextAt)nextLot(ev,key);continue;}
      if(time()>=ev.timerEnd){settle(ev);continue;}
      if(ev===h()&&state.page==='room'){
        const clock=document.querySelector('[data-clock]');
        if(clock){clock.textContent=seconds();if(seconds()<=9){const guide=document.querySelector('.ah-guide'),key=String(Math.ceil(seconds()/3));if(guide&&guide.dataset.beat!==key){guide.outerHTML=houseGuide(ev.lots[ev.index],null);document.querySelector('.ah-guide').dataset.beat=key;}}}
      }
    }
  },500);
})();
