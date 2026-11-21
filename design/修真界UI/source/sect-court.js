(() => {
  'use strict';
  const W = {
    home:'ZongMenWnd', halls:'ZongMenZhuFengWnd', tide:'ZongMenShouChaoWnd', mine:'ZongMen_KuangMaiWnd',
    vault:'ZongMen_CangBaoGeWnd', array:'ZongMen_HuZongDaZhenWnd', improve:'ZongMen_HuZongDaZhenUpgradeWnd',
    office:'ZongMen_ZhangLaoWnd', appoint:'ZongMen_ZhangLaoRenMingWnd', members:'ZongMenChengYuanWnd',
    work:'ZongMen_NeiMenWnd', rule:'ZongMen_DaDianWnd', transfer:'ZongMen_DaDianXieRenWnd'
  };
  const ranks = [
    ['foothills','松麓山门','#315e51'],['bamboo','竹隐别院','#506740'],['river','临川山庄','#3c6967'],
    ['falls','悬瀑仙府','#306474'],['peaks','群峰云府','#3d595e'],['observatory','观星道院','#554f73'],
    ['skybridge','凌霄圣境','#485d83'],['starpalace','星阙仙庭','#594660'],['celestial','九霄天宫','#755135']
  ];
  // 离线画板使用 game_map.go 的地图 ID 和父节点。名称、父链与运行配置 regions 一致。
  const regions = {
    '1': ['0', '修真界'],
    '11': ['1', '东荒'],
    '12': ['1', '南岭'],
    '13': ['1', '西漠'],
    '14': ['1', '北冥'],
    '111': ['11', '青云洲'],
    '112': ['11', '落日洲'],
    '121': ['12', '紫霞洲'],
    '131': ['13', '黄沙洲'],
    '141': ['14', '玄冰洲'],
    '1111': ['111', '临仙郡'],
    '1112': ['111', '朝露郡'],
    '1121': ['112', '云梦郡'],
    '1211': ['121', '紫霞郡'],
    '1311': ['131', '沙海郡'],
    '1411': ['141', '寒渊郡'],
  };
  const countyIDs = Object.keys(regions).filter(id=>id.length===4);
  function regionPath(countyID){
    const [provinceID,county]=regions[countyID];
    const [landID,province]=regions[provinceID];
    return [county,province,regions[landID][1]];
  }
  const plans=['均衡','采掘','守山'];
  const tactics=['破阵','固守','探踪'];
  const gear=['破阵丹','金刚符','寻踪镜'];
  const officer=['门人','执法长老','内务长老','外务长老','矿脉长老','护宗长老','灵田长老','内门长老','冶炼长老','炼丹长老','炼器长老','御兽长老','阵法长老','制符长老','工坊长老','宗主'];
  const rawDirectory=[
    ['主峰宗务',[[W.rule,'宗门大殿','gate'],[W.members,'门人名册','members'],[W.office,'长老殿','order'],[W.appoint,'长老任命','order'],[W.transfer,'宗主交接','scroll'],['ZongMenChatWnd','宗门传讯','scroll'],['ZongMen_ZhiFaDianWnd','执法殿','ward'],['ZongMen_NeiWuDianWnd','内务殿','work']]],
    ['生产与守山',[[W.mine,'灵晶矿脉','mine'],[W.work,'人力调度','work'],[W.vault,'藏宝阁','treasury'],[W.array,'护宗大阵','formation'],[W.tide,'宗门兽潮','tide'],[W.improve,'阵法升阶','sigil'],['ZongMen_LingTianWnd','灵田','work'],['ZongMen_YeLianWnd','冶炼峰','ore'],['ZongMen_LianDanWnd','炼丹峰','sigil'],['ZongMen_LianQiWnd','炼器峰','ward'],['ZongMen_YuShouWnd','御兽园','tide'],['ZongMen_ZhenFaWnd','阵峰','formation'],['ZongMen_ZhiFuWnd','符峰','sigil'],['ZongMen_GongFangWnd','工坊','work']]],
    ['外务与往来',[['ZongMen_WaiWuDianWnd','外务悬赏','order'],['ZongMen_WaiWuDianXiangQingWnd','悬赏详情','scroll'],['ZongMen_ShiKongMiJingWnd','时空秘境','replay'],['ZongMen_YanWuTaiWnd','演武台','ward'],['ZongMenChouJiangEventWnd','山门赐福','merit'],['ZongMen_SpeedUpWnd','营造加速','sigil'],['BaiRuShanMenWnd','拜入山门','gate'],['SheLiZongMenWnd','设立宗门','region']]]
  ];
  
  const previewStart=Date.UTC(2026,10,28,10,45);
  const DAY=86400000, QUARTER=900000, EAST=8*3600000;
  const prices=[3,4,5];
  const seals=['日曜','月辉','星光','玄铁'];
  const sealColors=['#965d31','#4d716e','#625477','#4d5353'];
  const levels=[8,7,6,4], power=[1260,1080,660,480];
  function round(at,rank,members=4){
    const next=nextTide(at);
    const strength=(90+Math.min(4,members-1)*18)*rank;
    return {...next,rank,target:Math.floor(strength*(next.big?1.5:1)),foe:(next.big?'兽王 · ':'')+['苍狼群','赤羽兽群','铁鬃兽群','雾隐蛇群'][(Math.floor(next.at/DAY)+1)%4]};
  }
  function nextTide(now){
    const local=new Date(now+EAST);
    const day=Date.UTC(local.getUTCFullYear(),local.getUTCMonth(),local.getUTCDate());
    for(let n=0;n<8;n++){
      const at=day+n*DAY+12*3600000, date=new Date(at+EAST);
      const endMonth=new Date(at+EAST+DAY).getUTCMonth()!==date.getUTCMonth();
      if(at>now && (endMonth||date.getUTCDay()===0)) return {at,big:endMonth};
    }
  }
  const fmt=(n)=>String(n).padStart(2,'0');
  function stamp(ms){const d=new Date(ms+EAST);return (d.getUTCMonth()+1)+'月'+d.getUTCDate()+'日 '+fmt(d.getUTCHours())+':'+fmt(d.getUTCMinutes());}
  const tideName=t=>t.big?'月末大潮':'周日小潮';
  function createDemo(rank,county='1111'){
    return {rank,county,name:'云岫宗',seal:0,sandHalf:0,crystalHalf:0,plan:0,array:1,sand:108,crystal:42,fang:12,wallets:[[12,0,0,0],[12,0,0,0],[12,0,0,0],[12,0,0,0]],damagedUntil:0,
      now:previewStart,lastMine:previewStart,period:round(previewStart,rank),echoes:[],history:[],tactic:0,tab:0,
      roles:[['姜晚照',15],['裴知秋',5],['方闻舟',2],['沈砚',0]],actor:0,birth:previewStart-7*DAY,notes:[],ledger:[],hallsTab:0,legacy:{stones:2000,herbs:30,metal:15,contribution:680,jade:24,minutes:140,reward:'',bounty:'',draws:3},completed:{}};
  }
  const accounts=new Map();
  let surveyRun=null, charterStep=0, promotionPath='ward';
  function player(){
    const name=person();
    if(!accounts.has(name))accounts.set(name,{permits:0,sweeps:0,spentMaps:0,issued:false,stones:2000,paidDays:new Set()});
    return accounts.get(name);
  }
  const availableMaps=account=>Math.max(0,account.sweeps-account.spentMaps);
  const payday=()=>new Date(s.now+EAST).toISOString().slice(0,10);
  const stipend=(n=s.rank)=>30+10*n;
  const charter={name:'青溪宗',shortName:'青溪',suffix:2,county:'1111',forest:1,seal:0};
  const suffixes=['派','门','宗','帮','教','宫'];
  const selected=(value,wanted)=>value===wanted?' selected':'';
  const named=(value,min,max)=>new RegExp('^[\\p{L}\\p{N}]{'+min+','+max+'}$','u').test(value);
  const examples=new Map();
  let s=createDemo(1), anchor, modal='', option=null, flash='', toastTimer, pageID='';
  examples.set(1,s);
  const grade=()=>s.rank<4?0:s.rank<7?1:2;
  const limit=(rank=s.rank)=>6+(rank-1)*2;
  function hands(rank=s.rank){const ratio=[.5,.75,1/3][s.plan];const workers=limit(rank),miners=Math.max(1,Math.floor(workers*ratio));return [miners,workers-miners];}
  function yieldPerTick(rank=s.rank){const sand=hands(rank)[0]*rank;return [sand,Math.max(1,Math.floor(sand/4))];}
  const canGovern=()=>s.roles[s.actor][1]===15;
  const area=()=>regionPath(s.county)[grade()];
  const stage=()=>['山门','仙府','圣地'][grade()];
  const person=()=>s.roles[s.actor][0];
  const wallet=()=>s.wallets[s.actor];
  const posted=()=>s.echoes.some(e=>e.name===person());
  const currentJob=()=>officer[s.roles[s.actor][1]];
  const iconNames=new Set('gate tide mine treasury formation region members work replay order ore sigil ward merit halls scroll warrant survey stipend building'.split(' '));
  const artDir='assets/sect-court/';
  const esc=x=>window.ui.esc(x);
  const ico=(key)=>'<i class="sc-ico" aria-hidden="true" style="background-image:url(&quot;'+artDir+'icons/'+(iconNames.has(key)?key:'scroll')+'.png&quot;)"></i>';
  const button=(a,v,text,cls='',dis=false)=>'<button type="button" class="'+cls+'" data-sc="'+a+'" data-v="'+esc(v)+'"'+(dis?' disabled':'')+'>'+text+'</button>';
  const open=(id,label,cls='')=>button('open',id,label,cls);
  const h=(title,right='')=>'<header class="sc-head"><h2>'+title+'</h2>'+right+'</header>';
  const note=t=>'<p class="sc-note">'+t+'</p>';
  const stats=items=>'<div class="sc-stats">'+items.map(([k,v])=>'<div><small>'+k+'</small><strong>'+v+'</strong></div>').join('')+'</div>';
  const act=(a,b='')=>'<div class="sc-act">'+a+b+'</div>';
  const cta=(a,v,t,disabled=false)=>button(a,v,t,'sc-cta',disabled);
  const sub=(a,v,t,disabled=false)=>button(a,v,t,'sc-sub',disabled);
  const row=(key,title,desc,end,a,v)=>button(a,v,ico(key)+'<div><b>'+title+'</b><small>'+desc+'</small></div><em>'+end+' <i>›</i></em>','sc-row');
  const list=rows=>'<div class="sc-list">'+rows.join('')+'</div>';
  const panel=(title,desc,action)=>'<section class="sc-panel">'+h(title)+'<p>'+desc+'</p>'+action+'</section>';
  function tabs(labels,active,a){return '<div class="sc-tabs">'+labels.map((label,i)=>'<button type="button" data-sc="'+a+'" data-v="'+i+'" aria-pressed="'+(i===active)+'">'+label+'</button>').join('')+'</div>';}
  const tile=(icon,label,a,v)=>button(a,v,ico(icon)+'<b>'+label+'</b>');
  function hero(title,scene,over,subtitle,foot,tag,home=false){
    const pins=home?[[W.mine,'灵晶矿脉',19,59],[W.rule,['正殿','仙府正殿','九霄道宫'][grade()],58,44],[W.array,['护山阵','洲府阵台','天枢大阵'][grade()],65,70]]:[];
    const sparks=scene==='mine'?'<div aria-hidden="true" class="sc-glints"><i></i><i></i><i></i></div>':'';
    return '<div class="sc-hero sc-'+(home?'home':scene)+'" style="background-image:url(&quot;'+artDir+scene+'.jpg&quot;),url(&quot;assets/sect.jpg&quot;)"><div class="sc-htext"><div class="sc-over">'+over+'</div><h1>'+title+'</h1><small>'+subtitle+'</small></div>'+sparks+(home?'<div class="sc-halo" aria-hidden="true"></div>':['mine','treasury','tide'].includes(scene)?sprite(scene):'')+pins.map(([v,label,x,y])=>'<button data-sc="open" data-v="'+v+'" type="button" class="sc-pin" style="left:'+x+'%;top:'+y+'%"><span>'+label+'</span></button>').join('')+'<div class="sc-hfoot"><div><small>'+foot[0]+'</small><b>'+foot[1]+'</b></div><span class="sc-crest">'+tag+'</span></div></div>';
  }
  function home(){
    const r=ranks[s.rank-1];
    const duty = grade()===0?'':grade()===1?'<div class="sc-duties">'+row('region','洲府辖区',area()+' · 根基在'+regionPath(s.county)[0],'舆图','modal','region')+row('work','洲府派工',plans[s.plan]+'预案','调度','open',W.work)+'</div>':
      '<div class="sc-dominion">'+ico('formation')+'<div><small>'+area()+' · 大陆圣地</small><b>天枢守境</b></div>'+button('open',W.array,'护宗大阵　›')+button('open',W.office,'执事长老　›')+'</div>';
    return hero(esc(s.name),r[0],regionPath(s.county).slice().reverse().join('　/　'),r[1],['宗门品阶',s.rank+' 阶 · '+stage()],seals[s.seal]+'印',true)+body(
      stats([['当前辖区',area()],['在册门人',s.roles.length+' 人'],['当值职务',currentJob()]])+duty+
      h(['今日宗务','仙府宗务','圣地宗务'][grade()],open('SheLiZongMenWnd','试玩建宗　›'))+
      '<div class="sc-tiles">'+[tile('tide','宗门兽潮','open',W.tide),tile('mine','灵晶矿脉','open',W.mine),tile('treasury','藏宝阁','open',W.vault),tile('formation','护宗大阵','open',W.array)].join('')+'</div>'+
      panel(tideName(s.period)+' · '+(posted()?'留影已录':'山关备战'),stamp(s.period.at)+' 统一结算。各门人可先后出阵。',open(W.tide,posted()?'查看本期留影　›':'赴关出阵　›'))+
      list([row('work','山门值守',plans[s.plan]+'调度 · 矿工 '+hands()[0]+' 人，守山 '+hands()[1]+' 人','调配','open',W.work),row('region',['根基舆图','一洲道府','大陆镇守'][grade()],area()+' · '+r[1],'查看','modal','region'),row('stipend','门人日俸',person()+' · 每日 '+stipend()+' 灵石',player().paidDays.has(payday())?'已领':'领取','modal','pay'),row('order','晋阶与收益','护山与营造 · 晋阶前后对照','查看','open',W.rule),row('halls','主峰诸殿','宗门职务、生产与外务','全部','open',W.halls)]));
  }
  const body=x=>'<div class="sc-body">'+x+'</div>';
  function facilityHero(title,icon,detail,tag,scene=ranks[s.rank-1][0]){
    return hero(title,scene,esc(s.name)+'　/　'+area(),detail,['当值',person()+' · '+currentJob()],tag);
  }
  function sprite(kind){return '<span aria-hidden="true" class="sc-sprite sc-sprite-'+kind+'"></span>';}
  function ledger(icon,title,detail){s.ledger.unshift([icon,title,detail,s.now]);s.ledger=s.ledger.slice(0,8);}
  function mine(){
    const [sand,crystal]=yieldPerTick();
    return facilityHero(['青璧晶脉','流霞玉髓','曜金龙脉'][grade()],'mine','矿灯引脉 · 每刻自动入阁','矿契','mine')+body(
      stats([['采掘弟子',hands()[0]+' 人'],['每刻矿砂','+'+(s.damagedUntil>s.now?sand/2:sand)],['每刻灵晶','+'+(s.damagedUntil>s.now?crystal/2:crystal)]])+
      h('采掘班次','<small>一刻十五分钟</small>')+(s.damagedUntil>s.now?note('山关失守，矿场减产中。'+stamp(s.damagedUntil)+' 恢复原产量。'):'')+
      list([row('work','调度预案',plans[s.plan]+' · 守山余 '+hands()[1]+' 人','调整','open',W.work),row('ore','矿砂入库','用于升阵、护山与营造晋阶',s.sand+' 份','open',W.vault),row('sigil','灵晶入库','用于升阵与营造晋阶',s.crystal+' 枚','open',W.vault)])+
      panel('矿契运转中','下次收矿 '+stamp(s.lastMine+QUARTER)+'。离线至多计入七十二小时。',button('clock','tick','试演一刻　›'))+
      h('当班去向')+list([row('treasury','收入公共库','本刻的两种矿物将自动合账','宝阁账册','open',W.vault)])); 
  }
  function work(){
    const [sand,crystal]=yieldPerTick();return facilityHero('人力调度','work','生产与守山共用一支弟子队伍','值簿','mine')+body(
      stats([['可派弟子',limit()+' 人'],['矿脉值守',hands()[0]+' 人'],['护山值守',hands()[1]+' 人']])+h('值守预案')+
      list(plans.map((p,i)=>row(['order','mine','ward'][i],p,['半数采掘，其余守山','四分之三采掘，其余守山','三分之一采掘，其余守山'][i],i===s.plan?'已采用':'详情','plan',i)))+
      panel('当前产量','每刻矿砂 '+sand+' 份，灵晶 '+crystal+' 枚。',open(W.mine,'查看矿脉　›')));
  }
  function formation(){
    const capped=s.array>=s.rank+2;return facilityHero('护宗大阵','formation','护山弟子轮值，阵力计入兽潮','阵契','tide')+body(stats([['阵法等级',s.array+' 级'],['当前上限',(s.rank+2)+' 级'],['守山弟子',hands()[1]+' 人']])+h('修缮阵眼')+
      list([row('ore','矿砂','阵基铺设',s.sand+' / '+(20*s.array),'item','sand'),row('sigil','灵晶','阵心供灵',s.crystal+' / '+(8*s.array),'item','crystal')])+
      act(cta('modal','upgradeArray',capped?'已达上限':'修缮升阵',capped),sub('open',W.work,'安排守山'))+note('升阵消耗当前等级 × 20 份矿砂、× 8 枚灵晶。品阶提高后可继续突破阵法上限。'));
  }
  function vault(){
    const contents=[
      [['sand','ore','灵脉矿砂','宗门晋阶 · 修缮阵基',s.sand],['crystal','sigil','凝光灵晶','护阵灵材',s.crystal]],
      [['fang','tide','镇妖兽牙','兽潮战获 · 宗门晋阶',s.fang]],
      gear.map((name,i)=>['gear'+i,['sigil','ward','replay'][i],name,prices[i]+' 功勋 / 件',wallet()[i+1]])
    ][s.tab];
    return facilityHero('藏宝阁','treasury','分阁典藏 · 账册随宗务更新','藏契','treasury')+body(stats([['公共矿砂',s.sand],['公共兽牙',s.fang],['我的功勋',wallet()[0]]])+
      h('山门珍藏')+tabs(['矿材','兽获','战备'],s.tab,'tab')+
      '<div class="sc-materials">'+contents.map(([id,icon,title,desc,count])=>button('item',id,'<span class="sc-quantity">× '+count+'</span>'+ico(icon)+'<b>'+title+'</b><small>'+desc+'</small>')).join('')+'</div>'+
      note(['开采所得直接进入公共库存，宗务按需取用。','每次守住兽潮，战获自动记入宗门。','个人功勋兑换的战备，在本人下次采用对应战法时自动使用。'][s.tab])+
      h('阁中流水')+list((s.ledger.length?s.ledger:[['treasury','开阁盘点','矿砂 108 · 灵晶 42 · 兽牙 12',s.birth]]).slice(0,3).map(([i,title,desc,at])=>'<div class="sc-row">'+ico(i)+'<div><b>'+esc(title)+'</b><small>'+esc(desc)+'</small></div><em>'+stamp(at).split(' ')[1]+'</em></div>')));
  }
  function tide(){
    const me=s.echoes.find(e=>e.name===person());return facilityHero(tideName(s.period),'tide','宗门劫难 · '+stamp(s.period.at)+' 结算','镇妖','tide')+body(
      stats([['本期敌影',s.period.foe],['守山人力',hands()[1]+' 人'],['护宗大阵',s.array+' 级']])+
      '<div class="sc-meter"><i style="width:'+Math.min(100,s.echoes.reduce((total,e)=>total+e.score,0)*100/s.period.target)+'%"></i></div>'+note('山关伤害 '+s.echoes.reduce((total,e)=>total+e.score,0)+' / '+s.period.target+'　·　'+s.echoes.length+' 份留影')+
      h('门人出阵',button('modal','switchActor','切换示例门人　›'))+
      (me?panel('此役已留影',me.name+' · '+me.tactic+' · '+me.score+' 伤害',button('echo',s.echoes.indexOf(me),'回看留影　›')):
      tabs(tactics,s.tactic,'tactic')+note(['借门人自身战力破阵，冲击兽群前锋。','借守山弟子与护宗阵力，稳住山关防线。','借矿工与灵晶勘测，截断妖兽后援。'][s.tactic])+act(cta('modal','fight','以「'+tactics[s.tactic]+'」出阵'),sub('open',W.vault,'前往藏宝阁')))+
      h('山关留影',button('modal','history','历期战报　›'))+
      (s.echoes.length?list(s.echoes.map((e,i)=>row('replay',esc(e.name)+' · '+e.tactic,stamp(e.at),e.score+' 伤害','echo',i))):note('本期尚无门人留影。每位门人可在结算前独立出阵一次。'))+
      panel('统一结算','每周日小潮，每月末日大潮，均于东八区二十时结算。重合时只结大潮。',button('modal','settle','试演至本期结算　›')));
  }
  function compareGains(){
    const rank=s.rank,next=Math.min(9,rank+1), currentMine=yieldPerTick(rank),nextMine=yieldPerTick(next);
    const beats=s.damagedUntil>s.now?2:4;
    const compare=[
      ['辖区',regionPath(s.county)[Math.floor((rank-1)/3)],regionPath(s.county)[Math.floor((next-1)/3)]],
      ['生产与守山人力',limit(rank)+' 人',limit(next)+' 人'],
      ['矿砂 / 小时',currentMine[0]*beats,nextMine[0]*beats],['灵晶 / 小时',currentMine[1]*beats,nextMine[1]*beats],
      ['阵法上限',(rank+2)+' 级',(next+2)+' 级'],['个人日俸',stipend(rank)+' 灵石',stipend(next)+' 灵石'],
      ['小潮胜 · 矿砂 / 兽牙',40*rank+' / '+4*rank,40*next+' / '+4*next],
      ['大潮胜 · 矿砂 / 兽牙',80*rank+' / '+8*rank,80*next+' / '+8*next]
    ];
    return '<table class="sc-compare"><thead><tr><th>宗门收益</th><th>'+rank+' 阶</th>'+(rank!==next?'<th>晋至 '+next+' 阶</th>':'')+'</tr></thead><tbody>'+
      compare.map(([k,a,b])=>'<tr><th>'+k+'</th><td>'+a+'</td>'+(rank!==next?'<td>'+b+'</td>':'')+'</tr>').join('')+'</tbody></table>'+
      note('矿产按「'+plans[s.plan]+'」预案及当前矿场状态计算。已开启的兽潮按开潮时的品阶结算。');
  }
  function dailyPay(){
    const acct=player(), claimed=acct.paidDays.has(payday());
    return h('门人日俸','<small>东八区 · 每日一份</small>')+list([row('stipend',claimed?'今日俸禄已入账':'领取今日俸禄',person()+' · 身上灵石 '+acct.stones,stipend()+' 灵石','modal','pay')]);
  }
  function upgradePrice(path=promotionPath){
    const civil=path==='build',rank=s.rank;
    const construction=rank*rank*(grade()+1);
    return {sand:civil?80*construction:40*rank,other:civil?32*construction:4*rank,key:civil?'crystal':'fang',label:civil?'灵晶':'兽牙',formation:civil?rank:0};
  }
  function rule(){
    return facilityHero('宗门大殿','gate',ranks[s.rank-1][1]+' · '+stage(),'宗契')+body(
      stats([['品阶',s.rank+' 阶'],['根基',regionPath(s.county)[0]],['辖区',area()]])+h('山门议事')+
      list([row('region','宗门辖区','根基逐级上溯至洲、陆',area(),'modal','region'),row('building','宗门晋阶',s.rank>=9?'九阶已满，可查本阶收益':'护山、营造二选一，确认后扣料',s.rank+' 阶','modal','upgradeRank'),row('members','人员职务','名册、长老与任免',s.roles.length+' 人','open',W.office),row('scroll','交接宗主','选定门人后确认交接','查看','open',W.transfer)])+dailyPay()+h('本阶与下阶')+compareGains()+act(sub('modal','preview','九阶山门试览')));
  }
  function halls(){
    const [section,items]=rawDirectory[s.hallsTab];
    return facilityHero('主峰诸殿','halls',ranks[s.rank-1][1]+' · '+area(),'殿录')+body(tabs(['主峰宗务','生产守山','外务往来'],s.hallsTab,'hallsTab')+
      '<div class="sc-directory">'+h(section)+list(items.map(([id,n,k])=>row(k,n,'','前往','open',id)))+'</div>');
  }
  function members(id){
    const appointment=id===W.appoint||id===W.transfer, handing=id===W.transfer;
    return facilityHero(({[W.appoint]:'长老任命',[W.office]:'长老殿',[W.transfer]:'宗主交接'})[id]||'门人名册','members','职务名录 · 当值权限','职牒')+body(
      h(handing?'选择继任人':appointment?'选择受任门人':'宗门在籍','<small>'+s.roles.length+' 位</small>')+
      list(s.roles.map(([name,role],i)=>row(role?'order':'members',esc(name)+(i===s.actor?' · 我':''),officer[role],role===15?'掌宗':role?'在任':'在籍',handing?'transfer':appointment?'appoint':'member',i)))+
      (appointment?note('任免和交接由宗主确认。'):act(sub('open',W.appoint,'长老任命'),sub('open',W.transfer,'宗主交接'))));
  }
  function warrantDesk(){
    const a=player(), maps=availableMaps(a);
    return h('开山令', '<small>立宗消耗一枚</small>')+
      stats([['持有开山令',a.permits+' 枚'],['堪舆残图',maps+' 张'],['个人灵石',a.stones]])+
      list([row('warrant','城主府首授','筹建页代办 · 每位修士一生可领一枚',a.issued?'已授':'免费','modal','grant'),
        row('survey','凭图勘印','历练成功清场一次得一图 · 三图换一令',maps+' / 3','modal','survey'),
        row('treasury','灵石请印','向城主府请印一枚开山令','600 灵石','modal','buyPermit')]);
  }
  function charterForm(){
    const {name,shortName,county,forest,suffix,seal}=charter;
    const field=(label,key,value,limit)=>'<label>'+label+'<input class="sc-input" data-charter="'+key+'" maxlength="'+limit+'" value="'+esc(value)+'"></label>';
    const select=(label,key,options)=>'<label>'+label+'<select class="sc-input" data-charter="'+key+'">'+options+'</select></label>';
    const picks=(labels,current)=>labels.map((x,i)=>'<option value="'+i+'"'+selected(i,current)+'>'+x+'</option>').join('');
    return '<div class="sc-charter-form">'+field('宗名 · 3 至 5 字','name',name,5)+field('宗门简称 · 1 至 2 字','shortName',shortName,2)+
      select('宗门末字','suffix',picks(suffixes,suffix))+select('印契外观','seal',picks(seals,seal))+
      select('建宗所在郡','county',countyIDs.map(id=>'<option value="'+id+'"'+selected(id,county)+'>'+regionPath(id).reverse().join(' · ')+'</option>').join(''))+
      select('本郡空闲山址','forest',Array.from({length:6},(_,i)=>'<option value="'+(i+1)+'"'+selected(i+1,forest)+'>妖兽林地 '+(i+1)+'</option>').join(''))+'</div>';
  }
  function foundSite(){return regionPath(charter.county)[0]+' · 妖兽林地 '+charter.forest;}
  function supporting(id){
    const spec=window.sectFacilitySpecs[id];
    if(!spec)return '';
    const [mode,title,,topic,raw,action]=spec;
    let entries=raw.split('|').map(pair=>pair.split('/'));
    const choice=s.completed[id+'-choice']||0, done=s.completed[id]||0;
    const facility=rawDirectory.flatMap(group=>group[1]).find(item=>item[0]===id);
    const emblem=facility?facility[2]:'scroll';
    let extra='', section=topic, details='';
    if(id==='ZongMenChatWnd'){
      section='同门传讯';
      details=list([['裴知秋','下次兽潮，我去守正阵。'],['方闻舟','新采的灵晶已经入阁。'],...s.notes].slice(-10).map(([n,text])=>'<div class="sc-row">'+ico('scroll')+'<div><b>'+esc(n)+'</b><small>'+esc(text)+'</small></div></div>'));
      extra='<label>传讯内容 <input class="sc-input" id="sc-chat" maxlength="80" placeholder="写给同门"></label>'+act(cta('chat','','发送传讯'));
    }else if(id==='SheLiZongMenWnd'){
      section='山门筹建';details=tabs(['一　筹集开山令','二　择址立宗'],charterStep,'charterStep');
      if(charterStep===0){
        details+=warrantDesk();
        extra=act(cta('charterStep',1,'填写立宗文书'));
      }else{
        details+=charterForm();
        extra=note('先在本郡舆图勘定空闲山址，再题宗名、简称并选印。四种印契可免费选用。')+
          act(cta('modal','found','核对立宗文书'),sub('charterStep',0,'查看令簿'));
      }
    }else{
      if(id==='BaiRuShanMenWnd'){section='山门拜帖';entries=[['宗名',s.name],['山门品阶',s.rank+' 阶'],['建宗根基',regionPath(s.county)[0]],['门人',s.roles.length+' 位']];}
      if(id==='ZongMen_NeiWuDianWnd')entries=[['一袋灵石','100 灵石 · 有 '+player().stones],['一束灵植','凝露草 ×5 · 有 '+s.legacy.herbs],['一盒灵材','寒纹铁 ×3 · 有 '+s.legacy.metal]];
      if(id==='ZongMen_WaiWuDianXiangQingWnd')entries=[['委托物',s.legacy.bounty||'凝露草'],['征收数量','6 份'],['悬赏灵石','120 灵石'],['发布贡献','20 点']];
      if(id==='ZongMen_SpeedUpWnd')entries=[['剩余工期',s.legacy.minutes+' 分钟'],['投入灵玉','12 枚 · 有 '+s.legacy.jade],['加速额度','至多 60 分钟'],['加速后剩余',Math.max(0,s.legacy.minutes-60)+' 分钟']];
      if(id==='ZongMenChouJiangEventWnd')section='今日余 '+s.legacy.draws+' 签';
      if(mode==='营产')section='当值设施';
      if(mode==='营产'){
        details=list([row(emblem,'本峰出产',entries[1][1],'查看','legacyRow',1),row('work','生产值守','统一人力预案 · '+plans[s.plan],'调度','open',W.work),row('treasury','物资总账','宗门共有库存','查阅','open',W.vault)]);
        extra=act(cta('open',W.work,'前往人力调度'),sub('open',W.halls,'返回诸殿'));
      }else{
        details=list(entries.map(([label,value],i)=>row(emblem,esc(label),esc(value||''),i===choice?'已选':'选择','legacyRow',i)));
        const related={
          ZongMen_ZhiFaDianWnd:W.members,ZongMen_WaiWuDianWnd:'ZongMen_WaiWuDianXiangQingWnd',ZongMen_YanWuTaiWnd:W.tide,
          ZongMen_SpeedUpWnd:W.rule,ZongMen_NeiWuDianWnd:W.vault
        };
        const forward=id==='ZongMen_ZhiFaDianWnd'?W.members:id==='ZongMen_WaiWuDianWnd'?'ZongMen_WaiWuDianXiangQingWnd':'';
        const single=['BaiRuShanMenWnd','ZongMen_YanWuTaiWnd','ZongMen_ShiKongMiJingWnd','ZongMen_WaiWuDianXiangQingWnd'].includes(id);
        const blocked=(single&&done)||(id==='ZongMenChouJiangEventWnd'&&s.legacy.draws===0)||(id==='ZongMen_SpeedUpWnd'&&(s.legacy.minutes===0||s.legacy.jade<12));
        extra=act(cta(forward?'legacyGo':'modal',forward||'legacy',single&&done?'已办理':esc(action),blocked),
          related[id]&&related[id]!==forward?sub('open',related[id],'关联宗务'):'')+(s.completed[id+'-receipt']?note(esc(s.completed[id+'-receipt'])):'');
      }
    }
    const banner=id==='SheLiZongMenWnd'?hero('择山立宗','foothills','城主府　/　筹建文书','本郡勘址 · 立宗开山',['筹建修士',esc(person())],'立宗'):facilityHero(esc(title),emblem,esc(facility?facility[1]:title)+' · '+esc(s.name),'殿录');
    return banner+body(h(esc(section))+details+extra);
  }
  function component(d){
    const cases={
      DanMuItem:['兽潮传讯','scroll','方闻舟','外峰留影已记入本期山关。',W.tide],
      ShouChao_MonsterItem:['兽潮敌影','tide','山关前锋','破阵、固守、探踪均可为本期积累伤害。',W.tide],
      ZongMenChengYuanItem:['门人名册','members','沈砚','在籍门人 · 可随时出阵',W.members],
      ZongMenMingCiItem:['入宗名刺','scroll','谢闻川','临仙郡 · 拜会山门','BaiRuShanMenWnd'],
      ZongMen_ZhangLaoDianItem:['长老席位','order','裴知秋','护宗长老 · 掌护山阵务',W.office],
      ZongMen_ZhangLaoItem:['任命名册','members','沈砚','在籍 · 尚未任职',W.appoint]
    };
    const [group,icon,title,desc,id]=cases[d.id];
    return facilityHero(group,icon,d.title,'名录',id===W.tide?'tide':ranks[s.rank-1][0])+body(h(d.title)+list([row(icon,title,desc,'查看','open',id)])+act(sub('open',id,'进入'+group)));
  }
  const panels={
    [W.home]:home,[W.mine]:mine,[W.vault]:vault,[W.work]:work,[W.array]:formation,[W.improve]:formation,
    [W.tide]:tide,[W.rule]:rule,[W.halls]:halls,[W.members]:members,[W.office]:members,[W.transfer]:members,[W.appoint]:members
  };
  function renderDialog(){
    let title='',content='';
    const plain=t=>'<p>'+t+'</p>';
    const enumRows=(arr,a,current)=>list(arr.map((x,i)=>row('order',esc(x),'',i===current?'当前':'选择',a,i)));
    if(modal==='preview'){
      title='九阶山门试览';
      content=plain('选择一个品阶查看对应山门、辖区与宗务。每个试览品阶各有一份会话存档。')+list(ranks.map(([scene,n],i)=>row('gate',fmt(i+1)+' 阶 · '+n,['郡属山门','洲属仙府','大陆圣地'][Math.floor(i/3)],s.rank===i+1?'当前':'试览','rankPreview',i+1)));
    }else if(modal==='region'){
      title='宗门辖区';
      content=plain('宗门根基扎在'+regionPath(s.county)[0]+'，晋阶时沿该郡的地图归属自动升格。')+list(['郡属山门　1–3 阶','洲属仙府　4–6 阶','大陆圣地　7–9 阶'].map((label,i)=>'<div class="sc-row">'+ico('region')+'<div><b>'+label+'</b><small>'+regionPath(s.county)[i]+'</small></div><em>'+(i===grade()?'当前辖区':'')+'</em></div>'))+act(sub('modal','preview','查看九阶山门'));
    }else if(modal==='fight'){
      title='确认出阵';
      const has=wallet()[s.tactic+1]>0;
      content=plain(esc(person())+' 前往山关，采用「'+tactics[s.tactic]+'」。本期每人可出阵一次。')+plain(has?'随身'+gear[s.tactic]+'将自动消耗，本次伤害提高 25%。':'未携对应战备，本次按基础战法出阵。')+act(cta('fight','','出阵并留影',posted()),sub('close','','返回'));
    }else if(modal==='settle'){
      title='推进试演时间';
      content=plain('当前示例时间 '+stamp(s.now)+'。推进到 '+stamp(s.period.at)+' 后统一结算本期'+tideName(s.period)+'，同时计入途中的矿脉收益。')+act(cta('clock','settle','推进并结算'),sub('close','','再等等'));
    }else if(modal==='switchActor'){
      title='切换示例门人';content=plain('切换身份可让另一位门人稍后出阵，所有人的留影会汇入同一期。')+enumRows(s.roles.map(([name,role])=>name+' · '+officer[role]),'switch',s.actor);
    }else if(modal==='history'){
      title='山关战报';content=plain('保存最近七次战报，每期最多保留十二份门人留影。')+(s.history.length?list(s.history.map((r,i)=>row('replay',tideName(r)+' · '+(r.win?'守成':'失守'),stamp(r.at),r.echoes.length+' 影','report',i))):plain('尚无战报。统一结算后可在此回看。'));
    }else if(modal==='report'){
      const r=s.history[option];
      title=tideName(r)+' · '+(r.win?'守成':'失守');
      content=plain(stamp(r.at)+'　东八区')+stats([['累计伤害',r.damage],['守关所需',r.target],['门人出阵',r.count+' 人']])+plain(esc(r.result))+
        list(r.echoes.map((e,i)=>row('replay',esc(e.name)+' · '+e.tactic,stamp(e.at),e.score+' 伤害','archivedEcho',option+','+i)));
    }else if(modal==='echo'){
      const e=option;title='山关留影';content='<div class="sc-echo-film">'+sprite('tide')+'<span>'+esc(e.name)+'　·　'+e.tactic+'</span></div>'+plain(stamp(e.at)+'　东八区')+list([['出阵门人',esc(e.name)],['所用战法',e.tactic],['留影伤害',e.score],['随身战备',e.gear||'未使用']].map(([k,v])=>'<div class="sc-row">'+ico('replay')+'<div><small>'+k+'</small><b>'+v+'</b></div></div>'));
    }else if(modal==='pay'){
      const a=player(),claimed=a.paidDays.has(payday());
      title='领取门人日俸';content=stats([['当前品阶',s.rank+' 阶'],['今日可得',stipend()+' 灵石'],['个人灵石',a.stones]])+
        plain('门人每自然日可领一份，按领取时的宗门品阶发放。东八区零时换日，同日转宗仍共用领取记录。')+
        act(cta('pay','',claimed?'今日已领取':'领取至个人钱包',claimed));
    }else if(modal==='grant'||modal==='buyPermit'){
      const a=player(),free=modal==='grant';title=free?'城主府首授':'灵石请印';
      content=plain(free?(a.issued?'本角色已领取过免费开山令。':'由筹建页代办城主府授令，每位修士可免费领取一次。'):'消耗 600 枚个人灵石，换取一枚开山令。')+
        stats([['持有开山令',a.permits],['个人灵石',a.stones]])+
        act(cta('permit',free?'grant':'buy',free?'申领首枚开山令':'支付 600 灵石',free?a.issued:a.stones<600));
    }else if(modal==='survey'){
      const a=player(),maps=availableMaps(a);title='堪舆残图';content=plain('成功清空一场历练战场，记一张堪舆残图。累计清场也会计入。兑换三张后可得一枚开山令。')+
        stats([['累计清场',a.sweeps],['可用残图',maps],['开山令',a.permits]])+
        act(cta('permit','exchange','三图换一令',maps<3),sub('surveyStart','','试演一场历练'));
    }else if(modal==='surveyRun'){
      const enemies=['青岚螳','焰纹狼'];
      title='历练清场试演';content=plain(foundSite()+'　/　示例战场')+
        stats([['已击退',surveyRun+' / 2'],['待结算残图',surveyRun===2?1:0]])+
        plain(surveyRun===2?'场内妖兽已肃清，可以结算此次历练。':'前方遭遇'+enemies[surveyRun]+'，清空两组妖兽后结算。')+
        act(cta('surveyFight','',surveyRun===2?'结算清场':'迎战'+enemies[surveyRun]),sub('surveyLeave','','离开试演'));
    }else if(modal==='plan'){
      title=plans[option]+'调度';content=plain('采用后重新分配矿工和守山弟子，下一完整矿契周期按该预案计产。')+act(cta('applyPlan',option,s.plan===option?'已采用':'采用此预案',!allow('work')||s.plan===option))+authority('work');
    }else if(modal==='upgradeRank'){
      title='宗门晋阶';const p=upgradePrice(),ready=s.sand>=p.sand&&s[p.key]>=p.other&&s.array>=p.formation;
      content=s.rank===9?plain('已登九阶。'):(tabs(['护山晋阶','营造晋阶'],promotionPath==='build'?1:0,'promotionPath')+
        plain(p.formation?'以矿砂与灵晶营造殿宇，护宗阵须达当前宗阶。':'以矿砂与兽牙加固山关，兽牙由守住兽潮获得。')+
        stats([['矿砂 · 需 / 有',p.sand+' / '+s.sand],[p.label+' · 需 / 有',p.other+' / '+s[p.key]],...(p.formation?[['阵阶 · 需 / 有',p.formation+' / '+s.array]]:[])])+
        (!ready?plain('尚缺'+[s.sand<p.sand?'矿砂 '+(p.sand-s.sand)+' 份':'',s[p.key]<p.other?p.label+' '+(p.other-s[p.key])+' 份':'',s.array<p.formation?'阵阶 '+(p.formation-s.array)+' 级':''].filter(Boolean).join('、')+'。'):'')+
        act(cta('upgrade','rank','确认'+(p.formation?'营造':'护山')+'晋阶',!canGovern()||!ready))+authority('rule'));
      content+=compareGains();
    }else if(modal==='upgradeArray'){
      const level=s.array,sand=20*level,crystal=8*level;
      title='修缮升阵';content=level>=s.rank+2?plain('当前已达上限。'):plain('阵法从 '+level+' 提升至 '+(level+1)+'，固守战法增益提高 8 点。')+
        stats([['矿砂 · 需 / 有',sand+' / '+s.sand],['灵晶 · 需 / 有',crystal+' / '+s.crystal]])+
        act(cta('upgrade','array','确认消耗并提升',s.sand<sand||s.crystal<crystal||!allow('array')))+authority('array');
    }else if(modal==='item'){
      const item=option,idx=Number(item.slice(4));
      if(item.startsWith('gear')){title=gear[idx];content=plain('对应战法「'+tactics[idx]+'」。出阵时自动消耗并增加 25% 伤害。每种可携一份。')+stats([['功勋价格',prices[idx]],['我的功勋',wallet()[0]],['随身持有',wallet()[idx+1]]])+act(cta('buy',idx,wallet()[idx+1]?'已持有':'兑换战备',wallet()[idx+1]>0||wallet()[0]<prices[idx]));}
      else{const data={sand:['灵脉矿砂',s.sand,'灵晶矿脉产出。宗门晋阶与阵基修缮时扣除。'],crystal:['凝光灵晶',s.crystal,'灵晶矿脉产出。用于护宗阵法升级或营造晋阶。'],fang:['镇妖兽牙',s.fang,'镇守兽潮获胜时入阁。用于宗门晋阶。']}[item];
        title=data[0];content=stats([['公共库存',data[1]]])+plain(data[2])+act(sub('open',item==='fang'?W.tide:W.mine,'前往产地'));}
    }else if(modal==='member'||modal==='appoint'||modal==='transfer'){
      const [name,role]=s.roles[option];
      title=modal==='member'?'门人职牒':modal==='appoint'?'长老任命':'宗主交接';
      content=plain(esc(name)+'　/　'+officer[role]);
      if(modal==='member')content+=plain(role===15?'统筹宗务、晋升品阶及任免长老。':role===5?'统领护宗大阵，可参与兽潮。':'可参与兽潮、积累功勋与兑换战备。');
      else if(!canGovern()||role===15)content+=plain(role===15?'该门人已任宗主。':'宗主任职期间可办理此事。');
      else if(modal==='appoint')content+='<label>任命职务<select id="sc-office">'+officer.slice(0,15).map((v,i)=>'<option value="'+i+'" '+(i===role?'selected':'')+'>'+v+'</option>').join('')+'</select></label>'+act(cta('appointConfirm',option,'确认任命'));
      else content+=plain('交接后对方成为宗主，你将卸任。')+act(cta('transferConfirm',option,'确认交接'),sub('close','','返回'));
    }else if(modal==='legacy'){
      const spec=window.sectFacilitySpecs[pageID];
      title=spec[5];const state=ancillary(pageID);
      content=plain(state.detail)+act(cta('legacyConfirm','',title,!state.ready),sub('close','','返回'));
    }else if(modal==='found'){
      title='立宗文书';content=plain('宗名「'+esc(option.name)+'」 · 简称「'+esc(option.shortName)+'」 · '+suffixes[option.suffix]+'。根基在'+foundSite()+'，启用'+seals[option.seal]+'印契。')+
        stats([['立宗需开山令','1 枚'],['当前持有',player().permits+' 枚']])+plain('立宗成功时扣一枚开山令，开设一阶山门并派驻六名基础弟子。')+
        act(cta('found','','用一枚开山令立宗',player().permits<1),sub('close','',player().permits<1?'返回筹令':'返回'));
    }else return '';
    return '<dialog class="sc-dialog" aria-labelledby="sc-dialog-title"><div class="sc-modal"><header><h2 id="sc-dialog-title">'+title+'</h2>'+button('close','','×','sc-x').replace('class="sc-x"','class="sc-x" aria-label="关闭"')+'</header>'+content+'</div></dialog>';
  }
  function ancillary(id,commit=false){
    const a=s.legacy,account=player(), choice=s.completed[id+'-choice']||0;
    const result=(ready,detail,receipt,apply=()=>{})=>{if(ready&&commit){apply();s.completed[id]=(s.completed[id]||0)+1;s.completed[id+'-receipt']=receipt;}return {ready,detail,receipt};};
    if(id==='ZongMen_NeiWuDianWnd'){
      const [key,name,qty]=[['stones','灵石',100],['herbs','凝露草',5],['metal','寒纹铁',3]][choice];
      const holder=key==='stones'?account:a;return result(holder[key]>=qty,'捐出 '+name+' ×'+qty+'，个人贡献增加 20。','已捐赠'+name+'，增加 20 点贡献。',()=>{holder[key]-=qty;a.contribution+=20;});
    }
    if(id==='ZongMen_WaiWuDianXiangQingWnd'){
      const item=a.bounty||'凝露草';return result(account.stones>=120&&a.contribution>=20,'发布 '+item+' ×6 的悬赏，预付 120 灵石及 20 点贡献。','已发布 '+item+' 委托，悬赏 120 灵石。',()=>{account.stones-=120;a.contribution-=20;});
    }
    if(id==='ZongMen_ShiKongMiJingWnd')return result(a.contribution>=100,'消耗 100 点贡献进入秘境，静修一日后获得神识 24、真元 36。','本次秘境静修已完成，神识 +24、真元 +36。',()=>{a.contribution-=100;});
    if(id==='ZongMen_SpeedUpWnd')return result(a.jade>=12&&a.minutes>0,'消耗 12 灵玉，将本期营造缩短至 '+Math.max(0,a.minutes-60)+' 分钟。','已扣 12 灵玉，营造进度已更新。',()=>{a.jade-=12;a.minutes=Math.max(0,a.minutes-60);});
    if(id==='ZongMenChouJiangEventWnd'){
      const reward=['经脉丹','宗门仙金','神术残卷'][Math.max(0,3-a.draws)];return result(a.draws>0,'抽取一签，今日剩余 '+a.draws+' 次。','已获得'+reward+'。',()=>{a.draws--;a.reward=reward;});
    }
    if(id==='BaiRuShanMenWnd')return result(!s.completed[id],'以访山散修的身份向「'+esc(s.name)+'」递交示例拜帖。','拜帖已送达，等待宗门审核。');
    if(id==='ZongMen_YanWuTaiWnd')return result(!s.completed[id],'锁定本期宗门大比的魁首、次席奖励。','已封存本期演武奖励。');
    return result(false,'此设施可从主峰诸殿查阅。','');
  }
  function allow(area){
    const role=s.roles[s.actor][1];
    return role===15||(area==='work'&&[2,4].includes(role))||(area==='array'&&[5,12].includes(role));
  }
  const authority=area=>allow(area)?'':note(area==='rule'?'只有宗主可晋阶。':'需要当值长老或宗主权限。');
  const nav=[['gate','山门',W.home],['tide','兽潮',W.tide],['mine','矿脉',W.mine],['treasury','宝阁',W.vault]];
  window.sectCourtDesign=function(d){
    if(d.module!=='Guild')return null;
    if(anchor?.id!==d.id){modal='';option=null;flash='';}
    anchor=d;pageID=d.id;
    const core=panels[d.id], active=[W.tide,W.mine,W.vault].includes(d.id)?d.id:W.home;
    const center=d.kind==='component'?component(d):core?core(d.id):supporting(d.id);
    return '<article class="sect-court" style="--jade:'+ranks[s.rank-1][2]+';--seal:'+sealColors[s.seal]+'" data-stage="'+grade()+'"><header class="sc-top">'+(d.id===W.home?'<a href="设计总览.html">‹　设计总览</a>':open(W.home,'‹　山门'))+'<small>修真界　/　宗门</small>'+button('modal','preview',s.rank+' 阶　'+stage()+' ⌄')+'</header><div class="sc-scroll">'+center+'</div><nav class="sc-bottom" aria-label="宗门导航">'+nav.map(([icon,title,id])=>'<button type="button" data-sc="open" data-v="'+id+'" '+(active===id?'aria-current="page"':'')+'>'+ico(icon)+title+'</button>').join('')+'</nav>'+renderDialog()+(flash?'<div role="status" class="sc-toast">'+esc(flash)+'</div>':'')+'</article>';
  };
  function showFlash(message){flash=message;clearTimeout(toastTimer);toastTimer=setTimeout(()=>{flash='';document.querySelector('.sc-toast')?.remove();},2600);}
  function redraw(){const before=document.querySelector('.sc-scroll')?.scrollTop||0;window.draw(anchor.key);document.querySelector('.sc-scroll').scrollTop=before;const dialog=document.querySelector('.sc-dialog');if(dialog){dialog.showModal();dialog.querySelector('.sc-x').focus({preventScroll:true});}}
  function go(id){
    const d=catalog.find(entry=>entry.id===id);if(!d)return;
    modal='';option=null;flash='';
    if(anchor.key===d.key){redraw();return;}
    location.hash=encodeURIComponent(d.key);
  }
  function production(until){
    const elapsed=Math.floor((until-s.lastMine)/QUARTER);
    if(elapsed<=0)return;
    const ticks=Math.min(72*4,elapsed), last=s.lastMine+elapsed*QUARTER;
    const impaired=Math.max(0,Math.min(ticks,Math.floor((Math.min(last,s.damagedUntil)- (last-ticks*QUARTER))/QUARTER)));
    const [sand,crystal]=yieldPerTick();
    for(const [key,rate] of [['sand',sand],['crystal',crystal]]){
      const units=(2*ticks-impaired)*rate+s[key+'Half'];
      s[key]+=Math.floor(units/2);s[key+'Half']=units%2;
    }
    s.lastMine=last;ledger('ore','矿契入阁',ticks+' 刻累计 · 矿砂与灵晶已入库');
  }
  function settle(){
    const {at,big}=s.period,mult=big?2:1;
    const target=s.period.target,rank=s.period.rank;
    const damage=s.echoes.reduce((sum,e)=>sum+e.score,0),win=s.echoes.length>0&&damage>=target;
    let result='';
    if(win){s.sand+=40*rank*mult;s.fang+=4*rank*mult;result='山关守成，矿砂 +'+(40*rank*mult)+'、兽牙 +'+(4*rank*mult)+'。个人功勋每人 +'+6*mult+'。';}
    else if(at-s.birth<DAY){result='立宗首日受庇护，宗门库存和矿脉完好。';}
    else if(!s.echoes.length){s.damagedUntil=at+mult*3600000;result='空关失守，库存保全。矿脉减产一半，'+mult+' 小时后恢复。';}
    else {const loss=Math.min(Math.floor(s.sand*mult/10),8*rank*mult);s.sand-=loss;result='山关失守，损失矿砂 '+loss+' 份。个人参战保底功勋每人 +'+2*mult+'。';}
    s.echoes.forEach(e=>{s.wallets[e.actor][0]+=(win?6:2)*mult;});
    ledger('tide',tideName(s.period)+'结算',result);
    s.history.unshift({at,big,win,target,damage,count:s.echoes.length,echoes:s.echoes.slice(-12),result});
    s.history=s.history.slice(0,7);s.echoes=[];s.period=round(at,s.rank,s.roles.length);
  }
  function advance(until){
    while(s.period.at<=until){production(s.period.at);s.now=s.period.at;settle();}
    production(until);s.now=until;
  }
  document.addEventListener('input',event=>{
    const input=event.target.closest('.sc-charter-form [data-charter]');
    if(!input)return;
    const key=input.dataset.charter;
    charter[key]=['forest','seal','suffix'].includes(key)?Number(input.value):input.value.trim();
  });
  document.addEventListener('cancel',event=>{if(event.target.matches('.sc-dialog')){event.preventDefault();modal='';redraw();}},true);
  document.addEventListener('click',event=>{
    const b=event.target.closest('.sect-court [data-sc]');
    if(!b||b.disabled||!anchor)return;
    const a=b.dataset.sc,v=b.dataset.v,n=Number(v);
    if(a==='open')return go(v);
    if(a==='legacyGo'){if(pageID==='ZongMen_WaiWuDianWnd')s.legacy.bounty=['凝露草','玄水鳞','赤烈砂'][s.completed[pageID+'-choice']||0];return go(v);}
    if(a==='modal'){
      if(v==='found'){
        if(!named(charter.name,3,5)||!named(charter.shortName,1,2)){showFlash('宗名需 3 至 5 个文字或数字，简称需 1 至 2 个');redraw();return;}
        option={...charter};
      }
      modal=v;
    }else if(a==='close'){modal='';}
    else if(a==='rankPreview'){if(examples.get(n)?.rank!==n){const sample=createDemo(n,s.county);sample.name=s.name;sample.seal=s.seal;sample.roles=s.roles.map(pair=>pair.slice());sample.actor=s.actor;sample.period=round(sample.now,n,sample.roles.length);examples.set(n,sample);}s=examples.get(n);modal='';go(W.home);return;}
    else if(a==='clock'){const isTide=v==='settle',start=s.now;advance(isTide?s.period.at:start+QUARTER);modal=isTide?'report':'';option=0;if(!isTide)showFlash('已过一刻，产物自动入阁');}
    else if(a==='tab'){s.tab=n;}
    else if(a==='hallsTab'){s.hallsTab=n;}
    else if(a==='tactic'){s.tactic=n;}
    else if(a==='switch'){s.actor=n;modal='';}
    else if(a==='fight'){
      if(posted())return;
      const gearUsed=wallet()[s.tactic+1]===1, strength=Math.floor(power[s.actor]/60), phase=s.period.rank;
      const base=phase*65+levels[s.actor]*2+strength;
      let hit=base+[phase*30+Math.floor(strength/2),hands()[1]*3+s.array*8,hands()[0]*4+Math.min(50,Math.floor(s.crystal/4))][s.tactic];
      if(gearUsed){wallet()[s.tactic+1]=0;hit=Math.floor(hit*1.25);}
      s.echoes.push({actor:s.actor,name:person(),at:s.now,tactic:tactics[s.tactic],score:hit,gear:gearUsed?gear[s.tactic]:''});modal='echo';option=s.echoes.at(-1);
    }else if(a==='pay'){
      const account=player();if(account.paidDays.has(payday()))return;
      account.stones+=stipend();account.paidDays.add(payday());modal='';showFlash('今日俸禄 +'+stipend()+' 灵石，已入个人钱包');
    }else if(a==='permit'){
      const account=player();
      if(v==='grant'&&!account.issued)account.issued=true;
      else if(v==='exchange'&&availableMaps(account)>=3)account.spentMaps+=3;
      else if(v==='buy'&&account.stones>=600)account.stones-=600;
      else return;
      account.permits++;modal='';showFlash('开山令 +1');
    }else if(a==='surveyStart'){surveyRun=0;modal='surveyRun';}
    else if(a==='surveyLeave'){surveyRun=null;modal='survey';}
    else if(a==='surveyFight'&&modal==='surveyRun'&&surveyRun!==null){
      if(surveyRun<2)surveyRun++;
      else{player().sweeps++;surveyRun=null;modal='survey';}
    }else if(a==='charterStep'){charterStep=n;}
    else if(a==='promotionPath'){promotionPath=n===1?'build':'ward';}
    else if(a==='plan'){modal='plan';option=n;}
    else if(a==='applyPlan'&&allow('work')){s.plan=n;modal='';showFlash('已采用'+plans[n]+'预案');}
    else if(a==='upgrade'){
      const rank=v==='rank',level=rank?s.rank:s.array,price=rank?upgradePrice():{sand:20*level,other:8*level,key:'crystal',label:'灵晶',formation:0};
      if(!allow(rank?'rule':'array')||level>=(rank?9:s.rank+2)||s.array<price.formation||s.sand<price.sand||s[price.key]<price.other)return;
      s.sand-=price.sand;s[price.key]-=price.other;ledger('formation',rank?'宗门晋阶':'修缮护阵','耗矿砂 '+price.sand+'，'+price.label+' '+price.other);
      if(rank)s.rank++;else s.array++;modal='';showFlash(rank?'已升至 '+s.rank+' 阶，辖区 '+area():'护宗大阵已升至 '+s.array+' 级');
    }else if(a==='item'){modal='item';option=v;}
    else if(a==='buy'){
      if(wallet()[n+1]||wallet()[0]<prices[n])return;wallet()[0]-=prices[n];wallet()[n+1]=1;ledger('merit',person()+'兑换'+gear[n],'耗用个人功勋 '+prices[n]);modal='';showFlash('已兑换'+gear[n]);
    }else if(a==='report'){modal='report';option=n;}
    else if(a==='echo'){modal='echo';option=s.echoes[n];}
    else if(a==='archivedEcho'){const [idx,i]=v.split(',').map(Number);modal='echo';option=s.history[idx].echoes[i];}
    else if(['member','appoint','transfer'].includes(a)){modal=a;option=n;}
    else if(a==='appointConfirm'&&canGovern()&&s.roles[n][1]!==15){const role=Number(document.getElementById('sc-office').value);if(role)s.roles.forEach(m=>{if(m[1]===role)m[1]=0;});s.roles[n][1]=role;modal='';showFlash('职牒已更新');}
    else if(a==='transferConfirm'&&canGovern()&&n!==s.actor){s.roles[s.actor][1]=0;s.roles[n][1]=15;modal='';showFlash('宗主已交接给'+s.roles[n][0]);}
    else if(a==='legacyRow'){s.completed[pageID+'-choice']=n;showFlash('已选 '+window.sectFacilitySpecs[pageID][4].split('|')[n].split('/')[0]);}
    else if(a==='legacyConfirm'){const effect=ancillary(pageID,true);if(effect.ready){modal='';showFlash(effect.receipt);}}
    else if(a==='chat'){const input=document.getElementById('sc-chat');if(!input.value.trim())return;s.notes.push([person(),input.value.trim()]);showFlash('已传讯');}
    else if(a==='found'&&modal==='found'){
      const acct=player();if(acct.permits<1)return;
      const owner=person(),now=s.now,trial=createDemo(1,option.county);acct.permits--;
      trial.roles=[[owner,15]];trial.wallets=[[0,0,0,0]];trial.name=option.name;trial.seal=option.seal;
      trial.now=now;trial.lastMine=Math.floor(now/QUARTER)*QUARTER;trial.birth=now;trial.period=round(now,1,1);trial.sand=trial.crystal=trial.fang=0;
      trial.ledger=[['warrant','立宗开阁','开山令已核销，宗库从零开始',now]];
      s=trial;charterStep=0;examples.clear();examples.set(1,s);modal='';go(W.home);return;
    }
    redraw();
  });
})();
