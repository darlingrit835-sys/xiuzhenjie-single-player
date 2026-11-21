package main

import (
	"fmt"
	"google.golang.org/protobuf/proto"
	"math"
	"os"
	"strings"
	"testing"
)

func bestFreeMethod(s *Server, r Record, kind int) *gameMethod {
	var best *gameMethod
	for i := range s.methods().all {
		m := &s.methods().all[i]
		if m.system && m.kind == kind && m.matchesRoot(r) && int64(m.learningLevel) <= roleInt(r, "Level") && (best == nil || m.baseCircle > best.baseCircle) {
			best = m
		}
	}
	return best
}

func TestRealmBalanceTable(t *testing.T) {
	s := &Server{dataDir: "../server/data"}
	cfgServer, err := readConfig(s.dataDir)
	if err != nil {
		t.Fatal(err)
	}
	s.config = cfgServer
	if err := s.loadGameTables(); err != nil {
		t.Fatal(err)
	}
	cfg := readMonsterConfiguration(s.dataDir)
	now := func(n float64) float64 { return 2 * math.Ceil(n/2) / 60 }
	var out, hunting strings.Builder
	fmt.Fprintln(&hunting, "\n## 同一场历练值多少修炼时间\n\n取每境第五层，按同层深处四普、一精、一王全数击败、修为有足够空间入账计算。等值时间是将所得修为除以该层实际每秒纳气量；另有灵石与材料，赶路、拾取和失败均会影响实际回报。\n\n| 境界第五层 | 清场修为等值在线单修 | 灵石期望 |\n|---|---:|---:|")
	var cumulative, stageTime float64
	for l := int64(1); l <= 90; l++ {
		level := s.levelInfo(l)
		r := Record{"HasGold": true, "RunMask": 4, "Level": l}
		qi, mind, body := bestFreeMethod(s, r, 3), bestFreeMethod(s, r, 1), bestFreeMethod(s, r, 2)
		if qi == nil || mind == nil || body == nil {
			t.Fatal("missing equivalent-rank free option", l)
		}
		r["GongFaNaQi"], r["GongFaLianShen"], r["GongFaDuanTi"] = qi.id, mind.id, body.id
		points := int64(level.upgradeLimit) - realmStartingAir(l, level)
		if l == 1 {
			points = 0
		}
		rate := s.gameRate(r, 3)
		online, offline := now(float64(points)/rate), now(float64(points)/(.8*rate))
		if l > 1 && l < 90 && (online < lateRealmTimeFactor(l) || online > 90*lateRealmTimeFactor(l)) {
			t.Fatalf("level %d outside scaled minute budget: %.2f", l, online)
		}
		r["RunMask"] = 1
		mindTime := now(float64(level.trainingCap(1)-int64(level.perception)) / (s.gameRate(r, 1) * .5))
		r["RunMask"] = 2
		bodyTime := now(float64(level.trainingCap(2)-int64(level.toughTenacity)) / (s.gameRate(r, 2) * .5))
		if mindTime < .5 || level.trainingCap(1) < int64(level.perception) {
			t.Fatal(l, mindTime)
		}
		if (l-1)%10 == 0 {
			fmt.Fprintf(&out, "\n## %s\n\n", strings.TrimSuffix(gameLevelName(l), "1层"))
			fmt.Fprintln(&out, "| 层 | 纳气／神体倍率 | 突破耗寿／精力 | 满层修为 | 神识／体魄底值 | 单项上限 | 大关神识／体魄各需 | 突破点数¹ | 在线纳气 分 | 离线纳气 分 | 打3场后余 分 | 炼神／锻体单练 分 | 深处每场灵石 |")
			fmt.Fprintln(&out, "|---:|---:|---:|---:|---:|---:|---:|---:|---:|---:|---:|---:|---:|")
		}
		gold := 30 + 12*l
		var oneClear float64
		for _, m := range []struct {
			key         string
			tier, count int
		}{{"normal", 1, cfg.Waves["deep"].Normal}, {"elite", 3, cfg.Waves["deep"].Elite}, {"boss", 4, cfg.Waves["deep"].Boss}} {
			tier := cfg.Tiers[m.key]
			gold += int64((tier.GoldBase + tier.GoldPerLevel*int(l)) * m.count)
			oneClear += float64(m.count) * battleAirReward(l, level.upgradeLimit, battleTrainingShare(m.tier))
		}
		if l%10 == 5 {
			fmt.Fprintf(&hunting, "| %s | %.1f 分钟 | %d |\n", gameLevelName(l), float64(oneClear)/rate/60, gold)
		}
		years, energy := breakthroughCost(l)
		remaining := points - int64(math.Floor(3*oneClear))
		fmt.Fprintf(&out, "| %d | %.2f／%.2f× | %d／%d | %d | %d | %d | ", (l-1)%10+1, lateRealmTimeFactor(l), attributeTimeFactor(l), years, energy, int64(level.upgradeLimit), int64(level.perception), level.trainingCap(1))
		if l%10 == 0 && l < 90 {
			fmt.Fprintf(&out, "%d | ", attributeTarget(level, 1, breakthroughAttributePercent(l)))
		} else {
			fmt.Fprint(&out, "— | ")
		}
		if l < 90 {
			fmt.Fprintf(&out, "%d | %.1f | %.1f | %.1f | ", s.levelInfo(l+1).acupoints, online, offline, now(float64(max(0, remaining))/rate))
			stageTime += online
		} else {
			fmt.Fprint(&out, "— | 封顶 | — | — | ")
		}
		fmt.Fprintf(&out, "%.1f／%.1f | %d |\n", mindTime, bodyTime, gold)
		if l%10 == 0 {
			cumulative += stageTime
			fmt.Fprintf(&out, "\n逐层单修纳气合计 **%.1f 分钟**，从建角累计 **%.2f 小时**。¹ 点数在成功升入下一层时发放。\n", stageTime, cumulative/60)
			stageTime = 0
		}
	}
	t.Logf("solo clock to 90: %.2f hours", cumulative/60)
	calendar := realmCalendarReport(t, s)
	if dst := os.Getenv("XZJ_REALM_REPORT"); dst != "" {
		doc, err := os.ReadFile(dst)
		if err != nil {
			t.Fatal(err)
		}
		head, _, ok := strings.Cut(string(doc), "<!-- 境界数据 -->")
		if !ok {
			t.Fatal("missing report marker")
		}
		if err := os.WriteFile(dst, []byte(head+"<!-- 境界数据 -->\n"+calendar+hunting.String()+out.String()), 0644); err != nil {
			t.Fatal(err)
		}
	}
}

func TestEquippedMethodAffixLimitsAndReplacement(t *testing.T) {
	s := &Server{dataDir: "../server/data"}
	if err := s.loadGameTables(); err != nil {
		t.Fatal(err)
	}
	r := Record{}
	// The top legacy metal weapon bonus on two routes is 21 + 21, capped at 35.
	r["GongFaLianShen"], r["GongFaNaQi"] = "gf_1_16", "gf_3_16"
	s.refreshMethodAffixes(r)
	if methodAffixPercent(r, 106) != 35 || methodAffixPercent(r, 601) != 10 || methodAffixPercent(r, 110) != 0 {
		t.Fatal(r)
	}
	delete(r, "GongFaNaQi")
	s.refreshMethodAffixes(r)
	if methodAffixPercent(r, 106) != 21 || methodAffixPercent(r, 601) != 0 {
		t.Fatal(r)
	}
}

func TestPillFromBagAndMedicinePanel(t *testing.T) {
	s := auctionTestServer(t, t.TempDir())
	s.dataDir = "../server/data"
	if err := s.loadGameTables(); err != nil {
		t.Fatal(err)
	}
	peer, conn := auctionTestRole(t, s, "reader", nil)
	if err := s.roles.Update("reader", func(r Record) error {
		r["Level"], r["SuperAir"], r["PureBlood"], r["CurrentPureBlood"] = 10, 0, 100, 20
		r["Goods"] = []any{
			Record{"Type": 19, "GoodsId": "heal", "TemplateId": "pill_huichun", "Name": "回春散", "Count": 3},
			Record{"Type": 19, "GoodsId": "qi", "TemplateId": "pill_naqi", "Name": "纳气丹", "Count": 3},
		}
		return nil
	}); err != nil {
		t.Fatal(err)
	}
	for i, cmd := range []int{1004024, 1004032} {
		for j, id := range []string{"heal", "qi"} {
			p := gameInventoryProtocols[cmd]
			req := s.pb(p.req)
			key := "GoodsId"
			if field(req, key) == nil {
				key = "Id"
			}
			set(req, key, id)
			data, err := proto.Marshal(req)
			if err != nil {
				t.Fatal(err)
			}
			seq := i*2 + j + 1
			s.handleGame(peer, Message{Cmd: cmd, Msid: seq, Data: data})
			b := conn.receive(t, s, cmd, seq, p.resp)
			if !getBool(b, "IsSuccess") {
				t.Fatal(cmd, id, b)
			}
		}
	}
	r := s.roles.Get("reader")
	if roleInt(r, "CurrentPureBlood") != 60 || roleInt(r, "SuperAir") != 2*(int64(s.levelInfo(10).upgradeLimit)*8/100) || transferStackCount(locateGoods(r, "qi")) != 1 {
		t.Fatal(r)
	}
	r["SuperAir"] = int64(s.levelInfo(10).upgradeLimit)
	if code := s.consumeGamePill(r, locateGoods(r, "qi")); code != 4971 {
		t.Fatal("full bar must keep dose", code)
	}
}
