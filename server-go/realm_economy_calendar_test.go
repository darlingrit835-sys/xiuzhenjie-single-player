package main

import (
	"fmt"
	"maps"
	"math"
	"strings"
	"testing"
	"time"
)

const practiceForest = "1111W11"

type realmEconomyPoint struct {
	income, clearBonus, property, materials, wallet int64
	fightVigor, craftVigor, clears, wards, packs    int64
	home, gather, control, operationSeconds         int64
}

type realmEconomyCalendar struct {
	s                *Server
	peer             *Peer
	field            *battleDungeon
	ward             sigilChoice
	nextOrder        int
	totals, complete realmEconomyPoint
	stages           [9]realmEconomyPoint
}

func (p *realmEconomyCalendar) morningPending(clock int64) bool {
	return clock%86400 < 8*60
}

// Two minutes to provision, six minutes for the hunt, two for daily orders.
// The evening marks two hands-on minutes within its separate 15-minute visit.
func (p *realmEconomyCalendar) snapshot(r Record, clock int64) realmEconomyPoint {
	point := p.totals
	point.wallet, point.home = roleInt(r, "ShinyStone"), gameHomeRank(r)
	point.gather, point.control = roleInt(r, "JuLingRank"), roleInt(r, "KongLingRank")
	day := clock % 86400
	point.operationSeconds = (clock/86400)*12*60 + min(day, 10*60) + min(max(0, day-43200), 2*60)
	return point
}

func (p *realmEconomyCalendar) nextEvent(clock int64) int64 {
	day := clock % 86400
	for _, event := range [...]int64{120, 180, 240, 300, 360, 420, 480, 43200, 86400} {
		if day < event {
			return event - day
		}
	}
	panic("calendar day is out of range")
}

func (p *realmEconomyCalendar) event(t *testing.T, r Record, clock int64) {
	t.Helper()
	day := clock % 86400
	switch {
	case day == 0:
		if clock == 0 {
			r["Id"], r["MapId"], r["ShinyStone"] = p.peer.roleID, "111101", int64(500)
		}
		p.makeWard(t, r, clock/86400)
	case day == 120:
		p.enter(t, r)
	case day >= 180 && day <= 480 && day%60 == 0:
		p.kill(t, r, int(day/60-3))
		if day == 480 {
			p.buy(t, r)
		}
	case day == 43200:
		p.buy(t, r)
	}
}

// The production craft/admission entry points use the wall clock internally.
// Settle any earned energy on the simulation clock first (the caller does so),
// then give the action a fresh wall anchor to prevent a second recovery. Restore
// the calendar anchor with its original partial interval after paying.
func (p *realmEconomyCalendar) makeWard(t *testing.T, r Record, day int64) {
	t.Helper()
	wallet, vigor := roleInt(r, "ShinyStone"), roleInt(r, "CurrentVigor")
	mutate := func(q sigilRequest) {
		t.Helper()
		if _, err := sigilMutate(r, p.ward, q); err != nil {
			t.Fatalf("day %d %s/%s: %v", day+1, q.Action, q.Recipe, err)
		}
	}
	if day%4 == 0 {
		mutate(sigilRequest{Action: "supply", ExpectedPrice: sigilSupplyPrice(r)})
		p.totals.packs++
	}
	materials := sigilCounts(r)
	for _, line := range []struct{ id, recipe string }{{sigilPaper, "paper"}, {sigilInk, "ink"}} {
		if materials[line.id] < 1 {
			mutate(sigilRequest{Action: "make", Recipe: line.recipe})
		}
	}
	anchor := r["VigorRecoveredAt"]
	r["VigorRecoveredAt"] = time.Now().Unix()
	mutate(sigilRequest{Action: "draw", Strokes: p.ward.strokes()})
	r["VigorRecoveredAt"] = anchor
	for _, item := range goodsList(r) {
		g, _ := item.(Record)
		if roleString(g, "TemplateId") == p.ward.key() {
			if _, err := useInscribedTalisman(r, g); err != nil {
				t.Fatal(err)
			}
			break
		}
	}
	if roleInt(r, "AlchemyWard") <= 0 || vigor-roleInt(r, "CurrentVigor") != 1 {
		t.Fatal("missing daily ward or incorrect energy")
	}
	p.totals.wards++
	p.totals.craftVigor += vigor - roleInt(r, "CurrentVigor")
	p.totals.materials += wallet - roleInt(r, "ShinyStone")
}

func (p *realmEconomyCalendar) save(t *testing.T, r Record) {
	t.Helper()
	if err := p.s.roles.Put(p.peer.roleID, r); err != nil {
		t.Fatal(err)
	}
}
func (p *realmEconomyCalendar) reload(r Record) {
	saved := p.s.roles.Get(p.peer.roleID)
	clear(r)
	maps.Copy(r, saved)
}

func (p *realmEconomyCalendar) enter(t *testing.T, r Record) {
	t.Helper()
	p.field = p.s.createDungeon(r, practiceForest)
	if len(p.field.monsters) != 6 {
		t.Fatal("outer encounter should have six opponents")
	}
	cfg := p.s.battleState.balance
	for i, m := range p.field.monsters {
		tier := "normal"
		if i == 5 {
			tier = "elite"
		}
		if i < 5 && m.unusual != 1 || i == 5 && m.unusual != 3 {
			t.Fatal("outer wave has changed", p.field.monsters)
		}
		// Replace only the random symmetric gold payout with its expected value.
		rule := cfg.Tiers[tier]
		p.field.monsters[i].gold = int64(math.Round(float64(rule.GoldBase+m.level*rule.GoldPerLevel) * p.s.config.DropMultiplier))
	}
	vigor, anchor := roleInt(r, "CurrentVigor"), r["VigorRecoveredAt"]
	r["VigorRecoveredAt"] = time.Now().Unix()
	p.save(t, r)
	supplies, code := p.s.admitDungeon(p.field)
	if code != 4502 || supplies.ward <= 0 {
		t.Fatalf("admission %d, ward %d", code, supplies.ward)
	}
	p.reload(r)
	r["VigorRecoveredAt"] = anchor
	p.totals.fightVigor += vigor - roleInt(r, "CurrentVigor")
}

func (p *realmEconomyCalendar) kill(t *testing.T, r Record, index int) {
	t.Helper()
	if p.field == nil {
		t.Fatal("kill scheduled outside an expedition")
	}
	p.save(t, r)
	wallet := roleInt(r, "ShinyStone")
	drops, _, ok := p.s.settleMonster(p.field, p.field.monsters[index], 0, 0)
	if !ok {
		t.Fatal("scheduled kill was not awarded")
	}
	p.reload(r)
	p.totals.clearBonus += roleInt(r, "ShinyStone") - wallet
	for _, drop := range drops {
		if drop.goodsType == 100 {
			if _, code := collectBattleDrop(r, drop.id); code != 4526 {
				t.Fatal("gold pickup", code)
			}
		}
	}
	p.totals.income += roleInt(r, "ShinyStone") - wallet
	if index == len(p.field.monsters)-1 {
		p.save(t, r)
		if code := p.s.closeDungeon(p.field.id, p.peer.roleID, nil); code != 4537 {
			t.Fatal("leave", code)
		}
		p.reload(r)
		p.field = nil
		p.totals.clears++
	}
}

// Finish one complete home/gather/control tier before funding the next.
// Keep one refill of ink supplies in cash so the next morning can always craft.
func (p *realmEconomyCalendar) buy(t *testing.T, r Record) {
	t.Helper()
	for rank := 1; rank <= 4; rank++ {
		if int64(rank) > (roleInt(r, "Level")-1)/10+1 {
			return
		}
		for i, key := range []string{"DongFuRank", "JuLingRank", "KongLingRank"} {
			if roleInt(r, key) >= int64(rank) {
				continue
			}
			cost := int64(p.s.game.homes[rank].Price)
			if i > 0 {
				cost = matrixStonePrices[i-1][rank]
			}
			before := roleInt(r, "ShinyStone")
			if before-cost < 36 {
				return
			}
			p.save(t, r)
			if i == 0 {
				p.nextOrder++
				b := p.s.pb("UpgradeHomeResp_1005002")
				p.s.upgradeGameHome(p.peer, Message{Cmd: 1005002, Msid: p.nextOrder}, b)
				if getInt(b, "Code") != 4111 {
					t.Fatal("cave purchase", b)
				}
			} else {
				kind := []string{"gather", "control"}[i-1]
				if _, err := p.s.caveMatrixOrder(p.peer.roleID, caveMatrixRequest{kind, rank, cost}); err != nil {
					t.Fatal(err)
				}
			}
			p.reload(r)
			if roleInt(r, key) != int64(rank) || before-roleInt(r, "ShinyStone") != cost {
				t.Fatal("incorrect purchase or debit", key, rank)
			}
			p.totals.property += cost
		}
	}
}

func realmEconomyReport(t *testing.T, s *Server, baseline realmCalendarPoint) string {
	t.Helper()
	world := auctionTestServer(t, t.TempDir())
	world.config, world.dataDir, world.game = s.config, s.dataDir, s.game
	world.roles.historyEnabled = false
	peer, _ := auctionTestRole(t, world, "calendar", nil)
	ward, err := sigilSelect("ward", "simple")
	if err != nil {
		t.Fatal(err)
	}
	activity := &realmEconomyCalendar{s: world, peer: peer, ward: ward}
	calendar := runRealmCalendar(t, world, 43200, 120, activity)
	var out strings.Builder
	fmt.Fprint(&out, `
### 早晚各十五分钟，每天制一符、打一场、攒钱布阵

沿用均衡路线和单金灵根，各层两项永久属性都瞄准底值的 120%。开局 500 灵石。早上用 2 分钟抄纸、调墨、手画并预备一张平收护身符，随后用 6 分钟走第一林外围，每分钟击败一只，依次五普一精。按入场时的境界生成敌人及其掉落。战场中沿用入场前修行路线并继续行功，回城才改修或突破。每只的三项成长按实际击杀结算，所得灵石按实服掉落的期望值全部拾取，清场另领固定奖。假设每次都打赢，无重打。灵石是本样本唯一收入，材料留在地面。

每四天补一包符材，首包免费，后续每包 36，余料沿用。收工及晚间查一次账，预留下包的 36 灵石，达到境界和预算就逐阶买洞府、同阶聚灵阵、同阶控灵阵，再攒下一阶。洞府和阵法购入后立即作用于接下来的行功。早晨另留 2 分钟处理购买和突破，晚间用 2 分钟排功、处理突破。每个完整日合计 30 分钟在线，估算手动 12 分钟。操作时长、清场成功与雷劫一次通过均为情景假设。表中各行取升境瞬间的余额。晨间已制符、尚未回城的行会多计一张符。

| 到达终点 | 累计天数 | 已清场／已制符 | 历练收入（其中清场奖） | 洞府与两阵投资 | 符材费 | 余款 | 洞府／聚灵／控灵 | 入场／制符／突破耗精 |
|---|---:|---:|---:|---:|---:|---:|---:|---:|
`)
	write := func(label string, v realmCalendarPoint, m realmEconomyPoint) {
		if 500+m.income-m.property-m.materials != m.wallet {
			t.Fatalf("wallet mismatch at %s: %+v", label, m)
		}
		fmt.Fprintf(&out, "| %s | %.2f | %d／%d | %d（%d） | %d | %d | %d | %d／%d／%d | %d／%d／%d |\n", label, float64(v.clock)/86400, m.clears, m.wards, m.income, m.clearBonus, m.property, m.materials, m.wallet, m.home, m.gather, m.control, m.fightVigor, m.craftVigor, v.spent)
	}
	for i, v := range calendar.stages {
		write(strings.TrimSuffix(gameLevelName(int64(i*10+1)), "1层"), v, activity.stages[i])
	}
	m, v := activity.complete, calendar.complete
	write("90 级双属性达 120%", v, m)
	if m.clears == 0 || m.clears != m.wards || m.fightVigor != 2*(m.clears-1) || m.craftVigor != m.wards || m.materials != 36*(m.packs-1) || m.packs != (m.wards+3)/4 || v.clock >= baseline.clock {
		t.Fatalf("practice route accounting %+v vs %+v", m, baseline)
	}
	fmt.Fprintf(&out, "\n到 90 级且双属性都达到本层底值 120%%，这条路线用了 **%.2f 天**，同期均衡纯修为 %.2f 天，相差 %.2f 天。累计在线 %.2f 小时，其中按上述分配约 %.2f 小时为手动操作。共 %d 场外围、%d 张护身符、%d 包符材。灵石掉落的波动或战斗失败会改变购买时点与最终天数。\n", float64(v.clock)/86400, float64(baseline.clock)/86400, float64(baseline.clock-v.clock)/86400, float64(v.online)/3600, float64(m.operationSeconds)/3600, m.clears, m.wards, m.packs)
	return out.String()
}
