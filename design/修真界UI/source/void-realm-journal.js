/* 虚神界本机试玩簿。各次留帖、署名和传习分别落盘，切换道友可共读。 */
(() => {
  'use strict';

  const rules = window.voidRealmRules;
  const prefix = 'xzj.voidRealm.journal.v1.';
  const tags = ['求教', '心得', '邀试'];
  const sourceSpells = ['惊铃诀', '渡水盾', '换枢印'];
  const endings = {
    dan: '散药丹丸', qi: '爪斧弓剑靴钺衣裳戟琴印镯刀枪锤棒盾旗索矛珠幡氅绫鼎针钟铃尺甲',
    dao: '本诀谱录经功卷法书典', shu: '闪爆破斩劲杀轰劈吟击盾御体术印诀伤咒封解散毒瘴遁',
    zhen: '阵', zhi: '竹芽叶兰果芝参根藤花枝木草子'
  };

  function sigilName(brief) {
    return Number.isInteger(brief) ? sourceSpells[brief] || '' : '';
  }

  function nameHint(hallId) {
    if (hallId === 'fu') return '符书沿用所封法术的名称，由作者与制法辨别同名传本。';
    const tail = endings[hallId];
    return tail ? '三至五个汉字，末字可用「' + tail + '」。' : '';
  }

  const models = [
    {hallId: 'dan', author: '素问', name: '分露养气丹', grade: 1, brief: 0, route: 0, adjust: 0, count: 3},
    {hallId: 'qi', author: '陆砧', name: '照壁盾', grade: 2, brief: 0, route: 2, adjust: 0, count: 2},
    {hallId: 'dao', author: '闻溪', name: '渡溪周天诀', grade: 2, brief: 1, route: 1, adjust: 0, count: 1},
    {hallId: 'shu', author: '照庭', name: '掠星闪', grade: 3, brief: 0, route: 2, adjust: 0, count: 2},
    {hallId: 'zhi', author: '折兰', name: '双穗兰', grade: 1, brief: 1, route: 0, adjust: 0, count: 1}
  ];
  const sampleWorks = models.map(({count, ...draft}) => {
    const hall = rules.halls.find(item => item.id === draft.hallId);
    const actor = {roleLevel: 21, slots: [{type: hall.type, level: draft.grade}]};
    const result = trial(actor, draft);
    if (!result.success) throw new Error('示例方案未通过推演：' + draft.name);
    return {...draft, id: 'sample-' + draft.hallId, authorId: 'sample-author-' + draft.hallId,
      score: result.score, readers: Array.from({length: count}, (_, i) => 'sample-reader-' + i), seed: true};
  });
  const sampleNotes = [
    ['dan', '求教', '我照着分液法合丹，若最后多凝一息，巡山的四队还能各分到药吗？', '采藜', 'herb'],
    ['qi', '心得', '守门最怕护脊吃不住一击。这一图我留了宽脊，最后一锤守中。', '陆砧', 'qi'],
    ['dao', '邀试', '行路间换气急，我试了贯脉的路子。请同道按这卷走一遍，看周天能否接续。', '闻溪', 'dao']
  ];
  const samplePosts = sampleNotes.map(([hall, tag, text, author, authorKey], i) => ({
    id: 'sample-note-' + hall, author, authorId: 'sample-author-' + authorKey,
    tag, text, workId: 'sample-' + hall, time: Date.UTC(2026, 10, 20, 7 - i), seed: true
  }));

  function trial(actor, work) {
    return rules.simulate(work.hallId, work.grade, work.route, work.adjust, work.brief, actor);
  }

  function planKey(work) {
    return JSON.stringify([work.authorId, work.hallId, work.grade, work.brief, work.route, work.adjust]);
  }

  function load() {
    const works = sampleWorks.map(work => ({...work, readers: [...work.readers]}));
    const posts = samplePosts.map(post => ({...post}));
    const marks = [], manuscripts = [];
    // 独立键可避免不同道友在两个页签同时留言或传习时相互覆盖。
    const storage = window.localStorage;
    for (let i = 0; i < storage.length; i++) {
      const key = storage.key(i);
      if (!key || !key.startsWith(prefix)) continue;
      const value = storage.getItem(key);
      if (value === null) continue;
      const row = JSON.parse(value);
      if (key.startsWith(prefix + 'work/')) manuscripts.push(row);
      else if (key.startsWith(prefix + 'post/')) posts.push(row);
      else if (key.startsWith(prefix + 'study/')) marks.push(row);
    }
    // 两个页签同时题名时，按最早成稿保留同名或同方案的第一件。
    manuscripts.sort((a, b) => a.time - b.time || compareId(a, b));
    const titles = new Set(works.map(w => w.hallId + '/' + w.name));
    const plans = new Set(works.map(planKey));
    for (const work of manuscripts) {
      const title = work.hallId + '/' + work.name, plan = planKey(work);
      if ((work.hallId !== 'fu' && titles.has(title)) || plans.has(plan)) continue;
      titles.add(title);
      plans.add(plan);
      works.push({...work, readers: []});
    }
    const byId = new Map(works.map(w => [w.id, w]));
    for (const {workId, actorId} of marks) {
      const work = byId.get(workId);
      if (work && work.authorId !== actorId && !work.readers.includes(actorId)) work.readers.push(actorId);
    }
    works.sort((a, b) => (b.time || 0) - (a.time || 0) || compareId(a, b));
    posts.sort((a, b) => Number(a.seed) - Number(b.seed) || b.time - a.time || compareId(a, b));
    return {works, posts};
  }

  function compareId(a, b) {
    return a.id < b.id ? -1 : a.id > b.id ? 1 : 0;
  }

  function list(key) {
    try { return load()[key]; }
    catch {
      return key === 'posts' ? samplePosts.map(p => ({...p})) : sampleWorks.map(w => ({...w, readers: [...w.readers]}));
    }
  }

  function id() {
    if (window.crypto?.randomUUID) return window.crypto.randomUUID();
    return Date.now().toString(36) + '-' + Math.random().toString(36).slice(2);
  }

  const fail = reason => ({ok: false, reason});
  function write(actor, operation) {
    if (!actor || !actor.id || !String(actor.name || '').trim()) return fail('请先选定署名道友。');
    let state;
    try { state = load(); }
    catch { return fail('无法读取本机试玩记录，请检查浏览器存储后重试。'); }
    const entry = operation(state);
    if (!entry.key) return entry;
    try { window.localStorage.setItem(prefix + entry.key, JSON.stringify(entry.value)); }
    catch { return fail('本机存储写入失败，请检查可用空间或浏览器存储权限后重试。'); }
    return {ok: true, reason: entry.reason, ...(entry.work ? {work: entry.work} : {})};
  }

  function publish(actor, draft) {
    return write(actor, ({works}) => {
      if (!draft || !['brief', 'route', 'adjust'].every(k => Number.isInteger(draft[k]) && draft[k] >= 0 && draft[k] <= 2)) {
        return fail('请选定命题与两步工法后再署名。');
      }
      const result = trial(actor, draft);
      if (!result.success) return fail(result.reason);
      const name = draft.hallId === 'fu' ? sigilName(draft.brief) : String(draft.name || '').trim().normalize('NFC');
      const tail = endings[draft.hallId];
      if (!/^\p{Script=Han}{3,5}$/u.test(name) || (tail && !tail.includes([...name].pop()))) return fail(nameHint(draft.hallId));
      const {hallId, grade, brief, route, adjust} = draft;
      const work = {id: id(), authorId: actor.id, author: actor.name, name, hallId, grade, brief, route,
        adjust, score: result.score, readers: [], seed: false, time: Date.now()};
      if (hallId !== 'fu' && works.some(w => w.hallId === hallId && w.name === name)) return fail('本馆已有同名作品，请另题一名。');
      if (works.some(w => planKey(w) === planKey(work))) return fail('这套方案已由你署名，换个命题或工法再试。');
      return {key: 'work/' + work.id, value: work, work, reason: '作品已署名，列入本馆名录。'};
    });
  }

  function post(actor, tag, words, workId = null) {
    return write(actor, ({works}) => {
      if (!Number.isInteger(Number(actor.roleLevel)) || Number(actor.roleLevel) < 21) return fail('结丹一阶可入虚神界留言。');
      if (!tags.includes(tag)) return fail('请选择求教、心得或邀试。');
      const text = String(words || '').trim();
      if (!text || [...text].length > 120) return fail('留言可写一至一百二十字。');
      if (workId && !works.some(w => w.id === workId)) return fail('所附作品已无法找到，请重新选择。');
      const note = {id: id(), authorId: actor.id, author: actor.name, tag, text, workId: workId || null, time: Date.now(), seed: false};
      return {key: 'post/' + note.id, value: note, reason: '留言已记入本机玉简。'};
    });
  }

  function endorse(actor, workId) {
    return write(actor, ({works}) => {
      const work = works.find(w => w.id === workId);
      if (!work) return fail('未找到这部作品。');
      if (work.authorId === actor.id) return fail('自己的作品可自验，传习人数只计其他道友。');
      if (work.readers.includes(actor.id)) return fail('你已验过此作，传习人数只计一次。');
      const result = trial(actor, work);
      if (!result.success) return fail(result.reason);
      const mark = {workId, actorId: actor.id};
      const key = 'study/' + encodeURIComponent(workId) + '/' + encodeURIComponent(actor.id);
      return {key, value: mark, work: {...work, readers: [...work.readers, actor.id]}, reason: '复验合题，此作传习添一人。'};
    });
  }

  function ranking(hallId, mode = 'quality', grade = 0) {
    const value = mode === 'study' ? work => work.readers.length : work => work.score;
    const works = list('works').filter(w => (!hallId || hallId === 'all' || w.hallId === hallId) && (!grade || w.grade === grade));
    works.sort((a, b) => value(b) - value(a) || compareId(a, b));
    const authors = new Set(), rows = [];
    let last, place;
    for (const work of works) {
      if (mode === 'quality' && authors.has(work.authorId)) continue;
      authors.add(work.authorId);
      const score = value(work);
      if (score !== last) place = rows.length + 1;
      rows.push({...work, place});
      last = score;
    }
    return rows;
  }

  window.voidRealmJournal = {works: () => list('works'), posts: () => list('posts'), publish, post, endorse, ranking, nameHint, sigilName};
})();
