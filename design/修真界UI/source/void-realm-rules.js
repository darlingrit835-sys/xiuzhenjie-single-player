/* 虚神界设计样稿。一局只读示例角色；工艺的两项刻度用于比较方案。 */
(() => {
  'use strict';

  const numerals = '〇一二三四五六七八九';
  const clamp = (value, low, high) => Math.max(low, Math.min(value, high));
  const stage = grade => Number.isInteger(grade) && grade >= 1 && grade <= 9 ? numerals[grade] + '品' : '未入品';
  const option = (name, detail, a, b) => ({name, detail, values: [a, b]});
  const brief = (name, detail, a, b) => ({name, detail, targets: [a, b]});

  const halls = [
    {
      id: 'dan', type: 1, hall: '丹师塔', job: '丹师', work: '丹方', glyph: '丹', room: '炼丹室', tone: '#a25238',
      theme: '配伍药性', constraint: '主药定药性，辅药与炉火改变这一方的出丹与药力。',
      routeLabel: '选配伍', adjustLabel: '定火候', axes: ['出丹', '药力'],
      routes: [
        option('分液合丹', '药液分合数次，便于多结几粒。', 2, 0),
        option('主药独煎', '先尽取主药精华，单粒药力厚。', 0, 2),
        option('寒露调和', '用寒露调和草汁，粒数、药力兼顾。', 1, 1)
      ],
      adjusts: [
        option('循原火', '依方收火，保留原有药性。', 0, 0),
        option('缓收丹', '多凝一息，药力集中，出丹减少。', -1, 1),
        option('早分丸', '药液尚可分时结丸，粒数增加。', 1, -1)
      ],
      briefs: [
        brief('巡山散药', '采药队分行数路，每人都要分得一粒。', 2, 0),
        brief('守炉养气', '一名守炉弟子只带一丸，需把药力收足。', 0, 2),
        brief('行脚药囊', '药囊大小有限，各丸也要有足够药力。', 1, 1)
      ]
    },
    {
      id: 'qi', type: 2, hall: '炼器协会', job: '炼器师', work: '器图', glyph: '器', room: '炼器室', tone: '#826448',
      theme: '塑形留模', constraint: '器胚定形，落锤偏锋或偏脊改变攻守；实物首炼在炼器工坊结算。',
      routeLabel: '画器形', adjustLabel: '落定形锤', axes: ['攻势', '护持'],
      routes: [
        option('双脊匀锋', '两侧对锻，锋口与护脊各有分量。', 1, 1),
        option('窄脊长锋', '留材于尖端，锋锐而护手轻。', 2, 0),
        option('宽脊环格', '加厚护脊，接招最稳。', 0, 2)
      ],
      adjusts: [
        option('守中锤', '锤落器胚中段，保留原图攻守。', 0, 0),
        option('打薄前锋', '将护脊余材锻入锋口。', 1, -1),
        option('回锤护脊', '收去一线锋口，锻实护脊。', -1, 1)
      ],
      briefs: [
        brief('山门护器', '守门弟子需接住试剑者的重斩。', 0, 2),
        brief('破壳短兵', '猎队要切开妖甲，本炉专取锋锐。', 2, 0),
        brief('切磋佩剑', '同门轮流攻守，攻守不可偏废。', 1, 1)
      ]
    },
    {
      id: 'fu', type: 3, hall: '神符会', job: '符师', work: '符书', glyph: '符', room: '画符室', tone: '#99712d',
      theme: '运笔封术', constraint: '以符纸封存术式，依笔序行墨；转折与收锋决定何时释符。',
      routeLabel: '选笔势', adjustLabel: '定收笔', axes: ['引发', '持效'],
      routes: [
        option('折笔藏锋', '转折留墨，符力释出更缓。', 0, 2),
        option('一笔穿心', '起收相接，触符便可引动。', 2, 0),
        option('双折回墨', '前段引术，回笔蓄力。', 1, 1)
      ],
      adjusts: [
        option('照式提锋', '循原笔序收尾。', 0, 0),
        option('挑笔开口', '放出一道墨口，起符更快。', 1, -1),
        option('回钩封尾', '尾画收紧，符力徐徐释出。', -1, 1)
      ],
      briefs: [
        brief('巡夜警符', '触门立刻示警，只取头一瞬。', 2, 0),
        brief('夜渡护符', '舟行缓慢，护符需维持到对岸。', 0, 2),
        brief('换阵接符', '既要及时接阵，也要撑过换位。', 1, 1)
      ]
    },
    {
      id: 'shu', type: 4, hall: '万法阁', job: '术师', work: '法术', glyph: '术', room: '战法室', tone: '#47718a',
      theme: '依序结印', constraint: '一阶从五个手印起，每升一阶添一印。术师品阶与修士境界共同限制创法等阶。',
      routeLabel: '编手印', adjustLabel: '选末印', axes: ['疾发', '威力'],
      routes: [
        option('聚元起印', '先蓄灵再发，势重而成印慢。', 0, 2),
        option('引转相接', '引灵接转印，兼顾起手与威力。', 1, 1),
        option('疾行连印', '缩短印间过渡，术成得快。', 2, 0)
      ],
      adjusts: [
        option('正印收式', '沿印路收式，保持原来节奏。', 0, 0),
        option('急印截流', '提早截流，使术式快一拍。', 1, -1),
        option('合印凝元', '压住末印，将余灵聚入术式。', -1, 1)
      ],
      briefs: [
        brief('拦截飞梭', '飞梭一闪即过，须在其出阵前起术。', 2, 0),
        brief('击裂石傀', '石傀行动缓慢，护甲只吃重击。', 0, 2),
        brief('擂台接招', '对手攻守轮换，起手和威力都要留足。', 1, 1)
      ]
    },
    {
      id: 'zhen', type: 5, hall: '阵法协会', job: '阵法师', work: '阵图', glyph: '阵', room: '阵法室', tone: '#756082',
      theme: '九宫布阵', constraint: '依九宫安阵眼，用阴阳、卦位推演阵势。',
      routeLabel: '布阵眼', adjustLabel: '调阵脚', axes: ['覆盖', '阵固'],
      routes: [
        option('中宫合守', '四方阵势向中心收束，守眼牢固。', 0, 2),
        option('四隅展阵', '阵眼分居四隅，以广覆盖换阵固。', 2, 0),
        option('对宫相应', '用两处对宫互为支点。', 1, 1)
      ],
      adjusts: [
        option('原位定盘', '依原位锁定阵脚。', 0, 0),
        option('收脚护眼', '调回外缘阵脚，厚实阵心。', -1, 1),
        option('开脚拓界', '将阵脚推向外宫，扩展覆盖。', 1, -1)
      ],
      briefs: [
        brief('封谷口', '谷口横宽，需要让两侧崖壁都入阵。', 2, 0),
        brief('护住泉眼', '阵内一眼灵泉，阵脚务求扎实。', 0, 2),
        brief('商队驻夜', '营帐遍布半坡，也要挡住一次冲阵。', 1, 1)
      ]
    },
    {
      id: 'dao', type: 6, hall: '道境圣殿', job: '布道师', work: '功法', glyph: '道', room: '悟道台', tone: '#3a7775',
      theme: '连穴行周天', constraint: '一阶从八穴起，每升一阶添一穴。成稿消耗穴数乘品阶的悟性，败稿按穴数计。',
      routeLabel: '择运气路', adjustLabel: '定归元法', axes: ['纳气', '行功'],
      routes: [
        option('缓息归元', '留气于丹田，重纳气而周转慢。', 2, 0),
        option('周流贯脉', '沿经脉贯通周天，提高行功节奏。', 0, 2),
        option('吐纳相随', '一纳一行，使周天平顺。', 1, 1)
      ],
      adjusts: [
        option('循息收功', '依原呼吸走完一周天。', 0, 0),
        option('留息归田', '在归元处留一息，增加纳气。', 1, -1),
        option('借气行脉', '将一缕余气送入下一轮行功。', -1, 1)
      ],
      briefs: [
        brief('寒潭静修', '潭边灵气散得快，盼先多纳一口气。', 2, 0),
        brief('行旅周天', '行路间吐纳短促，求周天运转迅捷。', 0, 2),
        brief('晨课传本', '入门弟子要学会吐纳，也要走全周天。', 1, 1)
      ]
    },
    {
      id: 'zhi', type: 8, hall: '灵植协会', job: '灵植师', work: '灵植谱', glyph: '植', room: '育种室', tone: '#5c784c',
      theme: '择期嫁接', constraint: '先选根茎，分生长期嫁接其他灵植。一阶用五种植物，每升一阶添一种。',
      routeLabel: '选根与穗', adjustLabel: '定嫁接时机', axes: ['扎根', '萌枝'],
      routes: [
        option('留干接双穗', '一根供双穗，枝芽易发。', 0, 2),
        option('壮根留芽', '以壮根养一芽，先稳住根系。', 2, 0),
        option('同气合枝', '选气性相近的根与穗，根芽齐长。', 1, 1)
      ],
      adjusts: [
        option('当期接穗', '随原生长期接合。', 0, 0),
        option('提前接穗', '趁生长期催枝，将养分导向嫩芽。', -1, 1),
        option('缓接养根', '延后接穗，先把养分归根。', 1, -1)
      ],
      briefs: [
        brief('固沙栽苗', '沙坡风大，幼苗得先把根扎深。', 2, 0),
        brief('春圃取穗', '来春要分接新苗，这一季先养穗。', 0, 2),
        brief('迁圃保种', '老根须成活，新枝也要留种。', 1, 1)
      ]
    }
  ];

  function nodeCount(hallId, grade) {
    if (!validGrade(grade)) return 0;
    if (hallId === 'dao') return 7 + grade;
    if (hallId === 'shu' || hallId === 'zhi') return 4 + grade;
    return 0;
  }

  function validGrade(grade) {
    return Number.isInteger(grade) && grade >= 1 && grade <= 9;
  }

  function access(hallId, actor, grade = 1) {
    const hall = halls.find(item => item.id === hallId);
    const level = Number(actor?.roleLevel ?? 0);
    const realm = Number.isInteger(level) ? clamp(Math.ceil(level / 10), 0, 9) : 0;
    const slots = Array.isArray(actor?.slots) ? actor.slots : [];
    const rank = slots.reduce((highest, slot) => {
      if (!hall || Number(slot?.type) !== hall.type) return highest;
      const value = Number(slot.level);
      return validGrade(value) ? Math.max(highest, value) : highest;
    }, 0);
    const cap = Math.min(rank, realm);
    let reason = '';
    if (!hall) reason = '此馆尚未开设。';
    else if (realm < 3) reason = '结丹一阶可进入虚神界。';
    else if (!rank) reason = '限' + hall.job + '入馆，须持一品或以上本职职印。';
    else if (!validGrade(grade)) reason = '请选择一至九阶的作品。';
    else if (grade > cap) reason = '现为' + stage(rank) + hall.job + '，按品阶与境界最高可推演' + numerals[cap] + '阶作品。';
    return {eligible: !reason, reason, rank, realm, cap};
  }

  function resultAxes(hall, route, adjust, brief) {
    return hall.axes.map((name, i) => ({
      name, value: clamp(route.values[i] + adjust.values[i], 0, 2), target: brief.targets[i]
    }));
  }

  function revision(hall, brief, routeIndex, adjustIndex) {
    let best;
    hall.routes.forEach((route, r) => hall.adjusts.forEach((adjust, a) => {
      const fits = resultAxes(hall, route, adjust, brief).every(axis => axis.value === axis.target);
      if (!fits) return;
      // 先保留原法门，只调整收尾。若必须改法门，尽量沿用当前收尾。
      const weight = (r === routeIndex ? 0 : 2) + (a === adjustIndex ? 0 : 1);
      if (!best || weight < best.weight) best = {r, a, weight};
    }));
    if (!best) return '另择题目，再行试演。';
    if (best.r === routeIndex) return '保留「' + hall.routes[routeIndex].name + '」，改试「' + hall.adjusts[best.a].name + '」。';
    if (best.a === adjustIndex) return '改用「' + hall.routes[best.r].name + '」，沿用「' + hall.adjusts[adjustIndex].name + '」。';
    return '改用「' + hall.routes[best.r].name + '」，收尾时试「' + hall.adjusts[best.a].name + '」。';
  }

  function simulate(hallId, grade, routeIndex, adjustIndex, briefIndex = 0, actor) {
    const hall = halls.find(item => item.id === hallId);
    const permission = actor === undefined ? null : access(hallId, actor, grade);
    const route = hall?.routes[routeIndex], adjust = hall?.adjusts[adjustIndex], problem = hall?.briefs[briefIndex];
    if (!validGrade(grade) || !route || !adjust || !problem || (permission && !permission.eligible)) {
      return {success: false, eligible: false, score: 0, scoreText: '', title: '尚不能开案',
        reason: permission?.reason || '先选作品等阶、题目、法门与收尾方式。', next: '', axes: [], nodes: 0, cost: 0, costText: ''};
    }
    const axes = resultAxes(hall, route, adjust, problem);
    const distance = axes.reduce((sum, axis) => sum + Math.abs(axis.value - axis.target), 0);
    const success = distance === 0;
    const overrun = axes.reduce((sum, axis, i) => sum + Math.abs(route.values[i] + adjust.values[i] - axis.value), 0);
    const hasAdjustment = adjust.values.some(value => value !== 0);
    const economy = Math.max(0, 5 - (hasAdjustment ? 1 : 0) - overrun * 2);
    const score = success ? 50 + grade * 5 + economy : 45 - distance * 10;
    const scoreText = success ? '合题 50 ＋ 作品等阶 ' + grade * 5 + ' ＋ 工艺余裕 ' + economy +
      '。余裕满 5，转工减 1，超出刻度每格减 2。' : '草案尚未合题，每偏离一格，试演分减少 10。';
    const nodes = nodeCount(hallId, grade);
    const cost = success ? nodes * grade : nodes;
    const reason = axes.map(({name, value, target}) => name + (value === target ? '合度' : value > target ? '偏高' : '偏低') +
      '（' + value + '/' + target + '）').join('，') + '。';
    const costText = nodes ? '悟性预估 ' + cost + ' 点（本次实扣 0）' : '使用虚拟试材，本次实扣 0';
    return {success, eligible: true, score, scoreText, title: success ? '推演合题' : '草案待修',
      reason, next: success ? '可为这份' + hall.work + '题名留稿。' : revision(hall, problem, routeIndex, adjustIndex),
      axes, nodes, cost, costText};
  }

  window.voidRealmRules = {halls, access, simulate, nodeCount, stage};
})();
