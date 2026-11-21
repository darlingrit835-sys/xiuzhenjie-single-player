(() => {
  const featured = [
    'SignInWnd','CreateRoleWnd','MainCityWnd','WorldMapWnd','BattleWnd','DongFuRoomWnd',
    'NaJieSpaceWnd','QiZhenGeWnd','ForgeWorkshop','HuaFuWnd','ZongMenWnd','DaoYouChatWnd'
  ].map(key=>catalog.find(d=>d.id===key));
  const stem = d => d.id==='JingMaiItem'&&d.path.startsWith('Resources/')?'JingMaiItem-Resources':d.id;
  const png = d => '高清稿/'+d.module+'/'+stem(d)+'.png';
  const mini = d => '缩略图/'+stem(d)+'.webp';
  const step = 36;
  const find = sel => document.querySelector(sel);
  const list = find('.catalog');
  const dialog = find('#inspect');
  const search = find('#search');
  const module = find('#module');
  const kind = find('#kind');
  const more = find('#more');
  let matching = [], shown = 0, previews = [], opened = 0;

  for (const [id,name] of Object.entries(ui.modules)) module.add(new Option(name,id));
  for (const [id,name] of Object.entries(ui.sources)) kind.add(new Option(name,id));

  function card(d,all,index) {
    const button=document.createElement('button');
    button.className='card';
    button.type='button';
    button.innerHTML='<img width="180" height="400" loading="lazy" src="'+mini(d)+'" alt=""><h3>'+ui.esc(d.title)+'</h3><div>'+ui.modules[d.module]+' · '+ui.sources[d.kind]+'</div>';
    button.addEventListener('click',()=>open(all,index));
    return button;
  }
  function open(items,index) {
    const d=items[index];
    if(!d) return;
    previews=items;
    opened=index;
    find('#title').textContent=d.title;
    find('#meta').textContent=ui.modules[d.module]+' · '+ui.sources[d.kind];
    find('#assetPath').textContent=d.path;
    find('#fullImage').src=png(d);
    find('#fullImage').alt=d.title;
    find('#original').href=png(d);
    find('#html').href='画板.html#'+encodeURIComponent(d.key);
    find('#previous').disabled=index===0;
    find('#next').disabled=index===items.length-1;
    if(!dialog.open)dialog.showModal();
  }
  function batch() {
    const end=Math.min(matching.length,shown+step);
    for(let index=shown;index<end;index++) list.append(card(matching[index],matching,index));
    shown=end;
    more.hidden=shown===matching.length;
  }
  function filter() {
    const term=search.value.trim().toLowerCase();
    matching=catalog.filter(d=>(!module.value||d.module===module.value)&&(!kind.value||d.kind===kind.value)&&
      (!term||(d.id+' '+d.title+' '+d.path).toLowerCase().includes(term)));
    find('#count').textContent=matching.length+' 张';
    list.replaceChildren();
    shown=0;
    if(!matching.length) list.textContent='没有符合条件的画板';
    batch();
  }
  featured.forEach((d,i)=>find('.featured').append(card(d,featured,i)));
  search.addEventListener('input',filter);
  module.addEventListener('change',filter);
  kind.addEventListener('change',filter);
  more.addEventListener('click',batch);
  find('.close').addEventListener('click',()=>dialog.close());
  find('#previous').addEventListener('click',()=>open(previews,opened-1));
  find('#next').addEventListener('click',()=>open(previews,opened+1));
  dialog.addEventListener('keydown',event=>{
    if(event.key==='ArrowRight')open(previews,opened+1);
    if(event.key==='ArrowLeft')open(previews,opened-1);
  });
  dialog.addEventListener('click',event=>{if(event.target===dialog)dialog.close()});
  filter();
})();
