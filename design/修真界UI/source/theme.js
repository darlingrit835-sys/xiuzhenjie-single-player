(() => {
  const modules = {
    Account:'启程', MainQuest:'主线', Map:'舆图', Battle:'历练', Cultivation:'修行',
    Inventory:'纳戒与装备', Crafting:'炼制与创作', Guild:'宗门', Commerce:'坊市', Social:'道友', Common:'通用'
  };
  const sources = {window:'现有窗口', registered:'代码注册', reference:'代码引用', runtime:'运行时界面', concept:'策划提案', component:'界面组件'};
  const assets = new Set('book cauldron pill ring sword letter crystal herb talisman armor brush coin pendant purse scroll lock'.split(' '));
  const artPaths = {mountain:'mountain.jpg', cave:'cave.jpg', forge:'forge.jpg', sect:'sect.jpg', market:'journey/city.jpg', battle:'journey/region.jpg'};
  const esc = (t='') => String(t).replace(/[&<>"']/g, c => ({'&':'&amp;','<':'&lt;','>':'&gt;','"':'&quot;',"'":'&#39;'}[c]));
  const relic = (id, size=54) => '<img class="ui-relic" alt="" src="assets/relics/'+(assets.has(id)?id:'pendant')+'.webp" width="'+size+'" height="'+size+'">';
  const glyphs = {back:'‹', close:'×', next:'›', check:'✓', search:'⌕', add:'＋', minus:'−', settings:'⚙', filter:'≡', map:'⌖'};
  const icon = (id,size=24) => '<span class="ui-icon" style="font-size:'+size+'px" aria-hidden="true">'+(glyphs[id]||'◇')+'</span>';
  const btn = (label, kind='') => label?'<button class="ui-button '+esc(kind)+'" type="button">'+esc(label)+'</button>':'';
  const group = (title, body) => '<section class="ui-group"><h2>'+esc(title)+'</h2>'+body+'</section>';
  const rows = (items) => '<div class="ui-rows">'+items.map(([primary, secondary, stat, image]) =>
    '<div class="ui-row">'+(image?relic(image,48):'')+'<div><b>'+esc(primary)+'</b>'+(secondary?'<small>'+esc(secondary)+'</small>':'')+'</div>'+
    (stat?'<em>'+esc(stat)+'</em>':'')+'</div>').join('')+'</div>';
  const stats = (items) => '<div class="ui-stats">'+items.map(([label,value])=>'<div><span>'+esc(label)+'</span><strong>'+esc(value)+'</strong></div>').join('')+'</div>';
  const tabs = (items, active=0) => '<div class="ui-tabs">'+items.map((label,index)=>'<span class="'+(index===active?'active':'')+'">'+esc(label)+'</span>').join('')+'</div>';
  const grid = (items) => '<div class="ui-grid">'+items.map(([label,description,image])=>'<div>'+relic(image,62)+'<b>'+esc(label)+'</b><small>'+esc(description)+'</small></div>').join('')+'</div>';
  const image = (art) => artPaths[art]||artPaths.mountain;
  function page(d,{art,kicker,caption,body,foot,secondary,cls='',compact=false}={}) {
    const location = image(art);
    const illustrated = art && !compact;
    const action=foot||secondary?'<footer class="ui-footer">'+btn(secondary,'secondary')+btn(foot)+'</footer>':'';
    return '<article class="ui-page '+(illustrated?'illustrated':'compact')+' '+esc(cls)+'" style="--scene:url(&quot;../assets/'+location+'&quot;)">'+
      '<header class="ui-header"><div class="ui-nav"><span class="ui-back">'+icon('back',28)+' 返回</span><span>'+modules[d.module]+'</span></div>'+
      '<div class="ui-heading">'+(kicker?'<div class="ui-kicker">'+esc(kicker)+'</div>':'')+'<h1>'+esc(d.title)+'</h1>'+
      (caption?'<p>'+esc(caption)+'</p>':'')+'</div></header><main class="ui-body">'+(body||'')+'</main>'+action+'</article>';
  }
  function modal(d,{art,kicker,body,foot,secondary,cls=''}={}) {
    return '<div class="ui-overlaid '+esc(cls)+'" style="--scene:url(&quot;../assets/'+image(art)+'&quot;)"><div class="ui-underlying"><div class="ui-nav"><span>'+icon('back',26)+' 返回</span><span>'+modules[d.module]+'</span></div></div><div class="ui-scrim"></div>'+
      '<article class="ui-modal"><header><div><div class="ui-kicker">'+esc(kicker||modules[d.module])+'</div><h1>'+esc(d.title)+'</h1></div>'+icon('close',26)+'</header>'+
      '<div class="ui-modal-body">'+(body||'')+'</div><footer>'+btn(secondary,'secondary')+btn(foot)+'</footer></article></div>';
  }
  window.ui = {modules,sources,esc,relic,icon,btn,page,modal,rows,stats,tabs,grid,group};
})();
