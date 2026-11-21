/* 虚神界画板。角色与作品为本机试演数据。 */
(() => {
  'use strict';
  const rules = window.voidRealmRules;
  const journal = window.voidRealmJournal;
  const art = {dan:'cauldron', qi:'sword', fu:'talisman', shu:'crystal', zhen:'pendant', dao:'book', zhi:'herb'};
  const order = ['dan','qi','dao','shu','fu','zhen','zhi'];
  const halls = order.map(id => rules.halls.find(h => h.id === id));
  const hallFor = id => halls.find(h => h.id === id);
  const profiles = [
    {id:'qinghe',name:'青禾',roleLevel:31,realm:'元婴一阶',slots:[{type:1,level:3},{type:6,level:2}]},
    {id:'yunlu',name:'云麓',roleLevel:21,realm:'结丹一阶',slots:[{type:1,level:1},{type:8,level:1}]},
    {id:'chixiao',name:'赤霄',roleLevel:51,realm:'炼虚一阶',slots:[{type:2,level:6}]},
    {id:'shuying',name:'疏影',roleLevel:31,realm:'元婴一阶',slots:[{type:3,level:3},{type:5,level:3}]},
    {id:'tinglan',name:'听澜',roleLevel:41,realm:'分神一阶',slots:[{type:4,level:5},{type:6,level:4}]},
    {id:'baizhi',name:'白芷',roleLevel:21,realm:'结丹一阶',slots:[]},
    {id:'xiaoshan',name:'小山',roleLevel:20,realm:'筑基圆满',slots:[{type:1,level:2}]}
  ];
  const tags = ['求教','心得','邀试'];
  const esc = value => String(value ?? '').replace(/[&<>"']/g, c => ({'&':'&amp;','<':'&lt;','>':'&gt;','"':'&quot;',"'":'&#39;'}[c]));
  const attr = (k,v) => ' data-'+k+'="'+esc(v)+'"';
  const btn = (action,text,cls='',extra='') => '<button type="button" class="'+cls+'" data-do="'+action+'"'+extra+'>'+text+'</button>';
  const itemImage = (h, size='') => '<span class="vr-item '+size+'"><img src="assets/relics/'+art[h.id]+'.webp" alt=""></span>';
  const gradeText = n => rules.stage(n);
  const workStage = n => gradeText(n).replace('品','阶');
  const rankFor = (h,p) => Math.max(0,...p.slots.filter(s=>s.type===h.type).map(s=>s.level));
  const stageClass = n => n>=5?'gold':n>=3?'jade':'ink';
  const stageMark = n => n>=5?'金阙重轮':n>=3?'青玉双轮':'素墨单印';
  const strip = (label,count) => '<div class="vr-section"><h2>'+label+'</h2><span class="vr-hairline"></span><small>'+count+'</small></div>';
  let attached = null;

  function mount(root) {
    if (attached) attached();
    let profile = profiles[0];
    try { profile=profiles.find(p=>p.id===sessionStorage.getItem('xzj-void-profile'))||profile; } catch {}
    let page = 'hall', hall = halls[0], draft, result, studying, fx, resultFx, runTimer, noticeTimer, busy = false;
    let tag = '全部', rankMode = 'quality', rankHall = 'all', rankGrade = 0, boardDraft = '', boardTag = tags[0], attachment = '';
    let lastFocus;
    const openRealm = () => profile.roleLevel >= 21;
    const myHalls = () => halls.filter(h => rules.access(h.id,profile).eligible);
    const allWorks = () => journal.works();
    const workFor = id => allWorks().find(w=>w.id===id);
    const identity = () => profile.slots.map(s=>{const h=halls.find(h=>h.type===s.type);return gradeText(s.level)+h.job}).join(' · ') || '尚无职业';
    const homeHall = () => myHalls()[0] || halls[0];
    const fresh = () => ({grade:1,brief:0,route:0,adjust:0});
    const seal = text => '<span class="vr-stamp">'+esc(text)+'</span>';
    root.classList.add('vr-app');
    root.innerHTML = '<div class="vr-frame"><section class="vr-scene"></section><section class="vr-ledger"><nav class="vr-nav" aria-label="虚神界分区"></nav><div class="vr-content"></div></section></div><dialog class="vr-dialog"></dialog><div class="vr-notice" role="status" aria-live="polite"></div>';
    const scene=root.querySelector('.vr-scene'), nav=root.querySelector('.vr-nav'), content=root.querySelector('.vr-content');
    const dialog=root.querySelector('dialog'), notice=root.querySelector('.vr-notice');
    const message = text => {
      notice.textContent = text;
      notice.classList.add('show');
      clearTimeout(noticeTimer);
      noticeTimer=setTimeout(()=>notice.classList.remove('show'),3200);
    };
    function openDialog(title,body) {
      if(!dialog.open) lastFocus=document.activeElement;
      dialog.innerHTML='<header><h2>'+esc(title)+'</h2>'+btn('close','×','vr-close',' aria-label="关闭"')+'</header><div class="vr-dialog-body">'+body+'</div>';
      if(!dialog.open) dialog.showModal();
    }
    function renderScene() {
      const room = page==='room';
      const h = room ? hall : homeHall();
      const power = rankFor(h,profile) || 1;
      const title = !openRealm()?'虚神界':room?h.hall:page==='board'?'留音壁':page==='rank'?'百工金榜':'虚神界';
      const feature = h.briefs[0];
      const sub = !openRealm()?'结丹启境':room?(studying?'照谱复演':h.work+'推演'):page==='board'?'诸馆留言':page==='rank'?'作品题名':'虚神殿';
      const lead = page==='rank' ? journal.ranking(rankHall,rankMode,rankGrade)[0] : null;
      const invite = page==='board' ? journal.posts().find(p=>p.tag==='邀试' && workFor(p.workId)) : null;
      const invitedWork = invite && workFor(invite.workId);
      scene.dataset.scene=room?'room':'hall';
      scene.dataset.power=stageClass(power);
      scene.innerHTML='<canvas data-vr-fx data-vr-art-x="'+(room?'.60':'.50')+'" data-vr-art-y="'+(room?'.61':'.18')+'" aria-hidden="true"></canvas>'+
        '<div class="vr-scene-top"><a href="设计总览.html" class="vr-brand" target="_top" aria-label="返回修真界设计总览"><b>修</b><span>修真界</span></a>'+
        btn('profiles',esc(profile.name)+' <span>· 道籍</span>','vr-identity',' aria-label="切换试玩身份"')+'</div>'+
        '<div class="vr-titles"><h1>'+esc(title)+'</h1><span>'+esc(sub)+'</span></div>'+
        '<div class="vr-scene-foot">'+(!openRealm()?'<small>入界境界</small><strong>结丹一阶</strong>':
        room?'<small>'+esc(h.job)+' · '+gradeText(power)+'</small><strong>'+esc(h.theme)+'</strong><i>'+stageMark(power)+'</i>':
        page==='hall' && myHalls().length?'<small>'+esc(h.hall)+' · 论艺题</small><strong>'+esc(feature.name)+'</strong>'+btn('featured','揭题入馆　→','vr-scene-action'):
        invitedWork?'<small>'+esc(invite.author)+' · 邀同道复演</small><strong>'+esc(invitedWork.name)+'</strong>'+btn('work','展开附卷　→','vr-scene-action',attr('id',invitedWork.id)):
        lead?'<small>'+ (rankMode==='quality'?'造诣榜':'传习榜')+' · 榜首作品</small><strong>'+esc(lead.name)+'</strong>'+btn('work','阅榜首作品　→','vr-scene-action',attr('id',lead.id)):
        page==='rank'?'<small>百工金榜</small><strong>此阶虚位待题</strong>':'<small>虚神殿</small><strong>七馆名录</strong>')+'</div>';
      if(fx) fx.destroy();
      fx=window.voidRealmFx.mount(scene,{tone:room?h.tone:'#e5bb69',tier:power,mode:room && result && result.success?'reveal':room?'room':'hall'});
    }
    function navigation() {
      const selected=page==='room'?'hall':page;
      nav.innerHTML=[['hall','殿','大厅'],['board','言','留言板'],['rank','榜','排行榜']].map(([v,g,t])=>
        btn('tab','<i>'+g+'</i>'+t,selected===v?'active':'',attr('tab',v)+(selected===v?' aria-current="page"':'')+(!openRealm()?' disabled':''))).join('');
    }
    function header(kicker,title,side='') {
      return '<header class="vr-heading"><div><small>'+kicker+'</small><h2>'+title+'</h2></div>'+side+'</header>';
    }
    function lobby() {
      const owned=myHalls();
      return header('虚神殿','七馆名录',seal('百工'))+
        '<div class="vr-idline"><span>'+esc(profile.name)+' <b>·</b> '+profile.realm+'</span><span>'+esc(identity())+'</span></div>'+
        '<div class="vr-halls">'+[...owned,...halls.filter(h=>!owned.includes(h))].map(h=>{
          const rank=rankFor(h,profile);
          return btn('enter',itemImage(h)+'<span class="vr-row-body"><strong>'+h.hall+'</strong><small>'+h.job+' <b>·</b> '+h.work+'</small></span>'+
            '<span class="vr-admit">'+(rank?gradeText(rank)+'职印':'须为'+h.job)+'</span><span aria-hidden="true" class="vr-end">'+(rank?'›':'⌑')+'</span>',
            'vr-hall '+(rank?'admitted '+stageClass(rank):'locked'),attr('id',h.id)+(rank?'':' aria-label="'+h.hall+'，未持职印，查看条件"'));
        }).join('')+'</div><details class="vr-details vr-destinations"><summary>界内其他去处</summary><div><a target="_top" href="画板.html#XuShenTaWnd">虚神塔 · 角色榜</a><a target="_top" href="画板.html#XuShenJiTanWnd">虚神祭坛</a><span>合道台 · 尚未开放</span></div></details>';
    }
    function gate() {
      return header('主城 · 界门','入界条件',seal('界门'))+'<div class="vr-gate"><span class="vr-large-glyph">关</span><h3>结丹方可入界</h3><p>'+profile.name+'如今是'+profile.realm+'。达到结丹一阶，可凭神识进入大厅。</p>'+btn('profiles','切换试玩身份','vr-primary')+'</div>';
    }
    function profileSheet() {
      openDialog('道籍试演','<p class="vr-desc">选择示例修士，查看各职业与品阶的入馆效果。作品和留言保存在当前浏览器。</p>'+
        '<div class="vr-profiles">'+profiles.map(p=>btn('profile','<strong>'+p.name+'</strong><span>'+p.realm+' · '+(p.slots.map(s=>halls.find(h=>h.type===s.type).job+gradeText(s.level)).join('、')||'无职业')+'</span>'+
          (p===profile?'<em>当前</em>':'<em>›</em>'),'vr-profile',attr('id',p.id))).join('')+'</div>');
    }
    function selectOptions(items,selected,indexLabel) {
      return items.map((v,i)=>'<option value="'+i+'"'+(i===selected?' selected':'')+'>'+esc(v.name||v)+(indexLabel?' '+esc(indexLabel):'')+'</option>').join('');
    }
    function roomPage() {
      const access=rules.access(hall.id,profile,draft.grade);
      if (!access.eligible) return gate();
      const rank=rankFor(hall,profile), brief=hall.briefs[draft.brief];
      let out=header(esc(hall.job)+' · '+gradeText(rank)+'职印',studying?'照谱复演':hall.work+'推演',btn('leave','‹ 返殿','vr-textbutton'))+
        '<div class="vr-trial-head">'+itemImage(hall,'big')+'<div><small>'+ (studying?'依卷重演':'本次论题')+'</small><h3>'+esc(brief.name)+'</h3><p>'+esc(brief.detail)+'</p><div class="vr-brief-axes">'+
        hall.axes.map((a,i)=>'<span>'+esc(a)+' <b>'+['低','中','高'][brief.targets[i]]+'</b></span>').join('')+'</div></div></div>';
      if(studying) out+='<div class="vr-idline"><b>《'+esc(studying.name)+'》</b><span>'+esc(studying.author)+' 题</span></div>';
      out+='<fieldset class="vr-fields"'+(studying||busy?' disabled':'')+'><div class="vr-form-duo"><label>论艺题<select data-field="brief">'+selectOptions(hall.briefs,draft.brief)+'</select></label><label>作品阶位<select data-field="grade">'+
        Array.from({length:access.cap},(_,i)=>'<option value="'+(i+1)+'"'+(draft.grade===i+1?' selected':'')+'>'+workStage(i+1)+'</option>').join('')+'</select></label></div>'+
        strip(esc(hall.routeLabel),'择一')+'<div class="vr-choices">'+hall.routes.map((o,i)=>btn('route','<strong>'+esc(o.name)+'</strong><small>'+esc(o.detail)+'</small>','vr-choice'+(i===draft.route?' selected':''),attr('i',i)+' aria-pressed="'+(i===draft.route)+'"')).join('')+'</div>'+
        strip(esc(hall.adjustLabel),'择一')+'<div class="vr-controls">'+hall.adjusts.map((o,i)=>btn('adjust',esc(o.name),i===draft.adjust?'selected':'',attr('i',i)+' aria-pressed="'+(i===draft.adjust)+'"')).join('')+'</div><p class="vr-hint">'+esc(hall.adjusts[draft.adjust].detail)+'</p></fieldset>';
      const sim=rules.simulate(hall.id,draft.grade,draft.route,draft.adjust,draft.brief,profile);
      if(!result) {
        out+='<div class="vr-target"><span>题目要求</span><b>'+sim.axes.map(a=>esc(a.name)+' '+a.target).join('　·　')+'</b></div>'+
          '<p class="vr-hint">'+(sim.nodes?esc(sim.nodes+' '+(hall.id==='dao'?'处穴位':hall.id==='shu'?'道手印':'株灵植'))+' · ':'')+'使用虚拟试材，本机试演。</p>'+
          btn('run',busy?'灵机流转中…':studying?'依谱验证':'开阵推演','vr-primary',busy?' disabled':'');
      } else {
        out+='<div class="vr-outcome '+(result.success?'success':'misfire')+'" role="status"><header><span class="vr-outcome-ring"><canvas data-vr-fx data-vr-x=".50" data-vr-y=".50" aria-hidden="true"></canvas><b>'+ (result.success?'成':'修')+'</b></span><div><small>'+ (result.success?'论艺评定':'本轮推演')+'</small><h3>'+esc(result.title)+'</h3></div><b>'+result.score+'<small> 分</small></b></header><p>'+esc(result.reason)+'</p>'+
          '<div class="vr-readings">'+result.axes.map(a=>'<span>'+esc(a.name)+' <b>'+a.value+'</b> / 目标 '+a.target+'</span>').join('')+'</div><p>'+(studying?'照谱结果已校验，可查看传习回执。':esc(result.next))+'</p><small class="vr-scoring">'+esc(result.scoreText)+'</small>'+(result.cost?'<p>'+esc(result.costText)+'</p>':'')+'</div>'+
          (studying?btn('readback',studying.authorId===profile.id?'自己的作品':studying.readers.includes(profile.id)?'已留传习回执':'留传习回执','vr-primary',(!result.success||studying.authorId===profile.id||studying.readers.includes(profile.id))?' disabled':''):
          result.success?btn('name','署名入册','vr-primary'):btn('retry','调整再试','vr-primary'));
      }
      return out+'<details class="vr-details"><summary>本馆规约</summary><p>'+esc(hall.constraint)+'</p><p>职印与修士境界共同限定作品阶位。题目、调校与论艺分用于本机试演。</p></details>';
    }
    function enter(id, work) {
      const target=hallFor(id); if(!target) return;
      const permission=rules.access(id,profile,work?work.grade:1);
      if(!permission.eligible) {
        openDialog(target.hall,'<div class="vr-locked-info">'+itemImage(target,'big')+'<h3>'+esc(permission.reason)+'</h3><p>'+target.job+'凭职印入馆，可在此推演'+target.work+'。</p>'+btn('profiles','查看试玩道籍','vr-primary')+'</div>');
        return;
      }
      if(dialog.open) dialog.close();
      hall=target;draft=work?{grade:work.grade,brief:work.brief,route:work.route,adjust:work.adjust}:fresh();studying=work||null;result=null;setPage('room');
    }
    function board() {
      const posts=journal.posts().filter(p=>tag==='全部'||p.tag===tag);
      return header('诸馆共阅','留音壁',btn('compose','执笔留言','vr-small-primary'))+
        '<div class="vr-filters">'+['全部',...tags].map(t=>btn('filter-post',t,t===tag?'selected':'',attr('tag',t))).join('')+'</div>'+
        '<div class="vr-posts">'+(posts.length?posts.map(p=>{
          const work=p.workId && workFor(p.workId), stamp=p.author.slice(-1);
          return '<article class="vr-post"><div class="vr-author-seal">'+esc(stamp)+'</div><div><header><strong>'+esc(p.author)+'</strong><span>'+esc(p.tag)+'</span></header><p>'+esc(p.text)+'</p>'+
            (work?btn('work','阅卷 · '+esc(work.name)+'　›','vr-worklink',attr('id',work.id)):'')+'<small>'+ (p.seed?'示例墨迹':esc(dateLabel(p.time)))+'</small></div></article>';
        }).join(''):'<p class="vr-empty">此栏尚无墨迹。</p>')+'</div>';
    }
    function dateLabel(time) {
      const d=new Date(time);
      return isNaN(d.valueOf())?'本机墨迹':d.toLocaleDateString('zh-CN',{month:'long',day:'numeric'})+' · 本机墨迹';
    }
    function composeSheet() {
      const works=allWorks();
      openDialog('执笔留言','<form data-form="post"><div class="vr-form-duo"><label>类别<select name="tag">'+tags.map(t=>'<option'+(t===boardTag?' selected':'')+'>'+t+'</option>').join('')+'</select></label><label>附上作品<select name="work"><option value="">无附卷</option>'+works.map(w=>'<option value="'+esc(w.id)+'"'+(w.id===attachment?' selected':'')+'>'+esc(w.name)+'</option>').join('')+'</select></label></div><label>留言<textarea name="text" required maxlength="120" rows="4" placeholder="写下馆中所得，或向同道请教">'+esc(boardDraft)+'</textarea></label><p class="vr-hint">署名 '+esc(profile.name)+' · 最多 120 字 · 本机可见</p><button class="vr-primary" type="submit">题上留音壁</button></form>');
    }
    function ranking() {
      const list=journal.ranking(rankHall,rankMode,rankGrade);
      return header('虚神殿','百工金榜',seal('题名'))+'<div class="vr-rank-toolbar"><div class="vr-filters">'+[['quality','造诣榜'],['study','传习榜']].map(([id,t])=>btn('rank-mode',t,rankMode===id?'selected':'',attr('mode',id))).join('')+'</div>'+
        '<div class="vr-rank-selects"><label><span class="vr-sr">筛选职业馆</span><select data-field="rankHall"><option value="all">七馆总览</option>'+halls.map(h=>'<option value="'+h.id+'"'+(h.id===rankHall?' selected':'')+'>'+h.hall+'</option>').join('')+'</select></label><label><span class="vr-sr">筛选作品等阶</span><select data-field="rankGrade"><option value="0">所有等阶</option>'+Array.from({length:9},(_,i)=>'<option value="'+(i+1)+'"'+(rankGrade===i+1?' selected':'')+'>'+workStage(i+1)+'</option>').join('')+'</select></label></div></div>'+
        '<p class="vr-hint">'+(rankMode==='quality'?'每位作者取最高论艺分的一件作品。':'按作品获得的不同修士复演回执数排序。')+'</p>'+
        '<div class="vr-rank-list">'+(list.length?list.map((w,i)=>{const h=hallFor(w.hallId);return btn('work','<span class="vr-place">'+esc(w.place)+'</span>'+itemImage(h)+'<div><strong>'+esc(w.name)+'</strong><small>'+esc(w.author)+' · '+h.hall+(w.seed?' · 示例':'')+'</small></div><span class="vr-score">'+(rankMode==='quality'?w.score:w.readers.length)+'<small>'+(rankMode==='quality'?'论艺分':'人传习')+'</small></span>','vr-rank-row'+(i===0?' first':''),attr('id',w.id))}).join(''):'<div class="vr-empty">虚位待题。同馆作品署名后可见。</div>')+'</div>';
    }
    function workSheet(w) {
      if(!w) return;
      const h=hallFor(w.hallId);
      openDialog('署名作品','<div class="vr-trial-head">'+itemImage(h,'big')+'<div><small>'+h.hall+' · '+workStage(w.grade)+h.work+'</small><h3>'+esc(w.name)+'</h3><p>'+esc(w.author)+' 题'+(w.seed?' · 示例作品':'')+'</p></div></div>'+
        '<dl class="vr-work-dl"><dt>论题</dt><dd>'+esc(h.briefs[w.brief].name)+'</dd><dt>'+esc(h.routeLabel)+'</dt><dd>'+esc(h.routes[w.route].name)+'</dd><dt>'+esc(h.adjustLabel)+'</dt><dd>'+esc(h.adjusts[w.adjust].name)+'</dd><dt>论艺</dt><dd>'+w.score+' 分 · '+w.readers.length+' 人传习</dd></dl>'+
        btn('study','入馆复演','vr-primary',attr('id',w.id))+btn('attach','附此卷留言','vr-outline',attr('id',w.id)));
    }
    function nameSheet() {
      if(!result?.success || studying) return;
      openDialog('署名入册','<form data-form="publish"><div class="vr-idline"><b>'+esc(hall.hall)+'</b><span>'+workStage(draft.grade)+hall.work+' · '+result.score+' 分</span></div><label>'+ (hall.id==='fu'?'所封法术':'作品名')+'<input name="name" autocomplete="off" maxlength="10" required'+(hall.id==='fu'?' readonly value="'+esc(journal.sigilName(draft.brief))+'"':'')+'></label><p class="vr-hint">'+esc(journal.nameHint(hall.id))+'</p><p>署名 '+esc(profile.name)+'。入册后可上榜，其他同职业修士可照谱复演。</p><button type="submit" class="vr-primary">落印入册</button></form>');
    }
    function render(updateScene=true, resetScroll=false) {
      if(updateScene) renderScene();
      resultFx?.destroy();
      resultFx=null;
      if(resetScroll) content.scrollTop=0;
      navigation();
      content.innerHTML=!openRealm()?gate():page==='hall'?lobby():page==='room'?roomPage():page==='board'?board():ranking();
      const ring=content.querySelector('.vr-outcome-ring');
      if(ring) resultFx=window.voidRealmFx.mount(ring,{tone:hall.tone,tier:rankFor(hall,profile),mode:result.success?'reveal':'room'});
    }
    function setPage(next) {
      clearTimeout(runTimer);busy=false;scene.classList.remove('casting');page=next;render(true,true);
      if (window.matchMedia('(max-width:760px)').matches) window.scrollTo(0,0);
    }
    function revise() {result=null;render(true);}
    function run() {
      if(busy||!rules.access(hall.id,profile,draft.grade).eligible) return;
      busy=true;result=null;render(false);
      scene.classList.add('casting');
      const wait=window.matchMedia('(prefers-reduced-motion: reduce)').matches?0:700;
      runTimer=setTimeout(()=>{
        busy=false;result=rules.simulate(hall.id,draft.grade,draft.route,draft.adjust,draft.brief,profile);
        scene.classList.remove('casting');render();
        content.querySelector('.vr-outcome')?.scrollIntoView({block:'nearest',behavior:'instant'});
      },wait);
    }
    function click(e) {
      if(e.target===dialog){dialog.close();return;}
      const b=e.target.closest('button[data-do]'); if(!b||!root.contains(b)||b.disabled) return;
      const a=b.dataset.do;
      if(a==='profiles'){profileSheet();return;}
      if(a==='profile'){
        profile=profiles.find(p=>p.id===b.dataset.id)||profile;
        try{sessionStorage.setItem('xzj-void-profile',profile.id)}catch{}
        boardDraft='';attachment='';boardTag=tags[0];studying=null;result=null;dialog.close();setPage('hall');return;
      }
      if(a==='close'){dialog.close();return;}
      if(!openRealm()) return;
      if(a==='tab'||a==='leave'){setPage(a==='tab'?b.dataset.tab:studying?'rank':'hall');return;}
      if(a==='enter'||a==='featured'){enter(a==='enter'?b.dataset.id:homeHall().id);return;}
      if(a==='compose'){composeSheet();return;}
      if(a==='filter-post'){tag=b.dataset.tag;render(false);return;}
      if(a==='rank-mode'){rankMode=b.dataset.mode;render();return;}
      if(a==='work'){workSheet(workFor(b.dataset.id));return;}
      if(a==='study'){const work=workFor(b.dataset.id);if(work) enter(work.hallId,work);return;}
      if(a==='attach'){attachment=b.dataset.id;setPage('board');composeSheet();return;}
      if(page!=='room'||!rules.access(hall.id,profile,draft.grade).eligible||busy) return;
      if((a==='route'||a==='adjust')&&!studying){draft[a]=Number(b.dataset.i);revise();return;}
      if(a==='retry'){result=null;render();content.querySelector('.vr-fields select')?.focus();return;}
      if(a==='run'){run();return;}
      if(a==='name'){nameSheet();return;}
      if(a==='readback'&&studying&&result?.success){
        const r=journal.endorse(profile,studying.id);
        if(r.ok){studying=workFor(studying.id);rankMode='study';rankHall=studying.hallId;rankGrade=0;setPage('rank');}
        message(r.reason);
      }
    }
    function change(e) {
      if(!openRealm())return;
      const field=e.target.dataset.field;
      if(field==='rankHall'){rankHall=e.target.value;render();return;}
      if(field==='rankGrade'){rankGrade=Number(e.target.value);render();return;}
      if(page==='room'&&!studying&&!busy&&(field==='brief'||field==='grade')){
        draft[field]=Number(e.target.value);revise();
      }
    }
    function submit(e) {
      const form=e.target.closest('[data-form]');if(!form)return;e.preventDefault();
      if(!openRealm())return;
      const data=new FormData(form);
      if(form.dataset.form==='post'){
        const r=journal.post(profile,data.get('tag'),data.get('text'),data.get('work'));
        if(!r.ok){formError(form,r.reason);return;}
        boardDraft='';attachment='';tag='全部';dialog.close();setPage('board');message('墨迹已留在本机留音壁。');
      }else if(form.dataset.form==='publish'&&page==='room'&&!studying&&result?.success){
        const r=journal.publish(profile,{...draft,name:data.get('name'),hallId:hall.id,score:result.score});
        if(!r.ok){formError(form,r.reason);return;}
        dialog.close();rankMode='quality';rankHall=hall.id;rankGrade=0;setPage('rank');workSheet(r.work);message('已署名入册。');
      }
    }
    function formError(form,text) {
      let p=form.querySelector('[role=alert]');
      if(!p){p=document.createElement('p');p.setAttribute('role','alert');p.className='vr-error';form.insertBefore(p,form.querySelector('[type=submit]'));}
      p.textContent=text;
    }
    function input(e){
      const form=e.target.closest('[data-form="post"]');if(!form)return;
      boardDraft=form.elements.text.value;boardTag=form.elements.tag.value;attachment=form.elements.work.value;
    }
    function onClose(){if(lastFocus?.isConnected)lastFocus.focus({preventScroll:true});}
    function teardown(){
      clearTimeout(runTimer);clearTimeout(noticeTimer);fx?.destroy();resultFx?.destroy();
      root.removeEventListener('click',click);root.removeEventListener('change',change);root.removeEventListener('input',input);root.removeEventListener('submit',submit);dialog.removeEventListener('close',onClose);attached=null;
    }
    root.addEventListener('click',click);root.addEventListener('change',change);root.addEventListener('input',input);root.addEventListener('submit',submit);dialog.addEventListener('close',onClose);
    attached=teardown;render();
    return teardown;
  }
  window.voidRealmApp={mount,unmount:()=>attached?.()};
})();
