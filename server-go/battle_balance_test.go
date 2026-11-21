package main

import (
	"fmt"
	"math"
	"math/rand"
	"os"
	"slices"
	"strings"
	"testing"

	"google.golang.org/protobuf/proto"
)

// Use the shipping coefficients. Time steps model close range after acquisition.
// No network frame timing, human inputs or random field traversal are measured.
type combatTrial struct {
	wins            int
	seconds, health []float64
}

func testBattleRole(s *Server, level int, dressed bool) Record {
	l := s.levelInfo(int64(level))
	r := Record{"Level": level, "Perception": int64(l.perception), "Physique": int64(l.toughTenacity), "HasGold": true}
	if dressed {
		rank := battleMaterialRank(level, 1)
		r["EquippedGoods"] = []any{}
		for _, kind := range []int{15, 26, 16, 29} {
			r["EquippedGoods"] = append(r["EquippedGoods"].([]any), Record{"Type": kind, "TemplateId": "field.sample", "Level": level, "Rank": rank})
		}
	}
	return r
}

func elementalMultiplier(source, target int) float64 {
	overcomes := [...]int{0, 2, 5, 4, 1, 3}
	if overcomes[source] == target {
		return 1.5
	}
	if overcomes[target] == source {
		return .5
	}
	return 1
}

func resolveTrialHit(shield, blood *float64, damage float64) {
	taken := min(*shield, damage)
	*shield -= taken
	*blood -= damage - taken
}

func trialDuel(rng *rand.Rand, s *Server, role battleRoleStats, level int64, tier string, cls int) (float64, float64) {
	cfg := s.battleState.balance
	element := 1 + rng.Intn(5)
	m := monsterAtLevel(level, s.levelInfo(level).perception, cfg.Tiers[tier], cfg.Difficulty*s.config.MonsterDifficulty, 1+cfg.Variance*(2*rng.Float64()-1), 1)
	m.objectType, m.unusual, m.element = 2002, cls, element
	shield, hp := float64(role.defense), float64(role.blood)
	armor, mhp := float64(m.armor), float64(m.hp)
	sword, fire, weapon := battleSkills[0], battleSkills[1], battleSkills[8]
	outScale, inScale := elementalMultiplier(sword.element, element), elementalMultiplier(element, 1)
	// One cast channel, simultaneous autonomous flying sword. One miss per
	// action represents range/aim interruptions; the field itself has no hit roll.
	nextCast, nextSword, nextFire, nextWeapon, nextEnemy := 0.0, 0.0, 0.0, .7, .6
	mana := 2 * float64(role.perception)
	fireDPS, enemyDPS, fireEnd, enemyEnd := 0.0, 0.0, 0.0, 0.0
	enemyCycle, mitigation := m.skillTypes(), 1.0
	enemyTurn := 0
	for tick := 0; tick < 900; tick++ {
		t := float64(tick) / 10
		shield = min(float64(role.defense), shield+.1*.02*float64(role.defense))
		armor = min(float64(m.armor), armor+.1*.005*float64(m.armor))
		mana = min(2*float64(role.perception), mana+.1*.02*float64(role.perception))
		if t >= nextCast {
			switch {
			case t >= nextFire:
				if rng.Float64() < .90 {
					fireEnd = t + 5
					fireDPS = float64(role.attack) * fire.multiplier * elementalMultiplier(fire.element, element) / 5
				}
				nextCast = t + float64(fire.castMS)/1000 + .2
				nextFire = t + float64(fire.cooldownS) + float64(fire.castMS)/1000
			case t >= nextSword:
				if rng.Float64() < .90 {
					resolveTrialHit(&armor, &mhp, float64(role.attack)*sword.multiplier*outScale*mitigation)
				}
				nextCast = t + float64(sword.castMS)/1000 + .2
				nextSword = t + float64(sword.cooldownS) + float64(sword.castMS)/1000 + .1
			}
		}
		if t < fireEnd {
			resolveTrialHit(&armor, &mhp, .1*fireDPS*mitigation)
		}
		if t >= nextWeapon && mana >= float64(role.perception/20) {
			mana -= float64(role.perception / 20)
			if rng.Float64() < .85 {
				resolveTrialHit(&armor, &mhp, float64(role.attack)*weapon.multiplier*outScale*mitigation)
			}
			nextWeapon = t + 2 + float64(weapon.castMS)/1000
		}
		if mhp <= 0 {
			return t, max(0, shield+hp) / float64(role.defense+role.blood)
		}
		if t >= nextEnemy {
			id := enemyCycle[enemyTurn%len(enemyCycle)]
			skill := m.skill(id)
			damage := m.skillDamage(skill, [6]int{}, 0) * inScale
			switch id {
			case 1, 4:
				if rng.Float64() < .78 {
					resolveTrialHit(&shield, &hp, damage)
				}
			case 2:
				if rng.Float64() < .78 {
					enemyDPS, enemyEnd = damage/10, t+10
				}
			case 3:
				if rng.Float64() < .60 {
					nextCast = max(nextCast, t+1.5)
				}
			case 5:
				mitigation = .8
			case 8:
				nextCast = max(nextCast, t+.5)
			}
			enemyTurn++
			nextEnemy = t + 3.1 + float64(skill.castMS)/1000
		}
		if t < enemyEnd {
			resolveTrialHit(&shield, &hp, .1*enemyDPS)
		}
		if hp <= 0 {
			return t, 0
		}
	}
	return 90, 0
}

func sampleCombat(s *Server, role battleRoleStats, level int64, tier string, cls int) combatTrial {
	rng := rand.New(rand.NewSource(43017))
	var result combatTrial
	for i := range 500 {
		// Match the same 500 opponents across builds. Ending a duel early must
		// not advance the random stream used to generate the next opponent.
		rng.Seed(43017 + int64(cls) + int64(i)*1000003)
		duration, health := trialDuel(rng, s, role, level, tier, cls)
		if health > 0 {
			result.wins++
			result.seconds = append(result.seconds, duration)
			result.health = append(result.health, health)
		}
	}
	slices.Sort(result.seconds)
	slices.Sort(result.health)
	return result
}

func (r combatTrial) summary() string {
	if r.wins == 0 {
		return "0%／—／—"
	}
	return fmt.Sprintf("%.1f%%／%.1f／%.0f%%", float64(r.wins)/5, r.seconds[r.wins/2], 100*r.health[r.wins/2])
}

func TestBattleCurveAndReport(t *testing.T) {
	s := &Server{dataDir: "../server/data"}
	config, err := readConfig(s.dataDir)
	if err != nil {
		t.Fatal(err)
	}
	s.config = config
	if err := s.loadGameTables(); err != nil {
		t.Fatal(err)
	}
	cfg := readMonsterConfiguration(s.dataDir)
	s.battleState.balance = cfg
	var table, monsters, builds, training strings.Builder
	builds.WriteString("| 角色 | 同层裸装 | 同层四件 | 同层四件＋双属性各修满 | 四件修满挑战下一大境首层 |\n|---|---|---|---|---|\n")
	training.WriteString("| 境界 | 投入 | 实战攻击 | 韧性 | 血量 | 初始真元 | 对王胜率／胜局秒／剩余血韧 |\n|---|---|---:|---:|---:|---:|---|\n")
	table.WriteString("| 层 | 入境神识／体魄 | 裸装战力 | 换装战力 | 换装攻击／韧性／血量 | 普通 | 精英 | 兽王 | 清场灵石期望 | 倍率／深处一场修为／升层场数 |\n|---|---:|---:|---:|---|---|---|---|---:|---|\n")
	monsters.WriteString("| 层 | 普通 | 精英 | 兽王 | 通用素材、装备品阶 |\n|---|---|---|---|---|\n")
	last := int64(0)
	out := os.Getenv("XZJ_COMBAT_REPORT")
	for level := 1; level <= 90; level++ {
		l := int64(level)
		bare, wear := testBattleRole(s, level, false), testBattleRole(s, level, true)
		basePower, power := battlePower(bare), battlePower(wear)
		stats := equippedBattleStats(wear)
		if basePower <= last || power <= basePower || power >= math.MaxInt32 {
			t.Fatalf("level %d power %d/%d after %d", level, basePower, power, last)
		}
		last = basePower
		if level%10 == 5 {
			lvl := s.levelInfo(l)
			type investment struct {
				label                string
				perception, physique int64
			}
			perception, physique := roleInt(wear, "Perception"), roleInt(wear, "Physique")
			var victories [4]int
			for i, choice := range []investment{
				{"入境", perception, physique},
				{"炼神至上限", lvl.trainingCap(1), physique},
				{"炼体至上限", perception, lvl.trainingCap(2)},
				{"两项皆满", lvl.trainingCap(1), lvl.trainingCap(2)},
			} {
				role := cloneRecord(wear)
				role["Perception"], role["Physique"] = choice.perception, choice.physique
				stats := equippedBattleStats(role)
				trial := sampleCombat(s, stats, l, "boss", 4)
				victories[i] = trial.wins
				fmt.Fprintf(&training, "| %s | %s | %d | %d | %d | %d | %s |\n", gameLevelName(l), choice.label,
					stats.attack, stats.defense, stats.blood, 2*stats.perception, trial.summary())
			}
			if min(victories[1], victories[2]) <= victories[0] || victories[3] < max(victories[1], victories[2]) {
				t.Fatalf("level %d: receive no fighting benefit from an investment: %v", level, victories)
			}
		}
		if out == "" && level%10 != 1 && level != 90 {
			continue
		}
		if level%10 == 0 && out != "" {
			full := cloneRecord(wear)
			lvl := s.levelInfo(l)
			full["Perception"], full["Physique"] = lvl.trainingCap(1), lvl.trainingCap(2)
			trained := equippedBattleStats(full)
			cross := "已至顶层"
			if level < 90 {
				cross = sampleCombat(s, trained, l+1, "boss", 4).summary()
			}
			fmt.Fprintf(&builds, "| %s | %s | %s | %s | %s |\n", gameLevelName(l), sampleCombat(s, equippedBattleStats(bare), l, "boss", 4).summary(),
				sampleCombat(s, stats, l, "boss", 4).summary(), sampleCombat(s, trained, l, "boss", 4).summary(), cross)
		}
		var results, profiles []string
		gold := int64(30 + 12*level)
		limit := s.levelInfo(l).upgradeLimit
		clearAir := 0.0
		for i, tier := range []string{"normal", "elite", "boss"} {
			cls := []int{1, 3, 4}[i]
			r := sampleCombat(s, stats, l, tier, cls)
			if (i == 0 && r.wins < 480) || (i == 1 && r.wins < 300) || (i == 2 && (r.wins < 50 || r.wins >= 500)) {
				t.Fatalf("level %d %s unexpected fight %s", level, tier, r.summary())
			}
			results = append(results, r.summary())
			m := monsterAtLevel(l, s.levelInfo(l).perception, cfg.Tiers[tier], cfg.Difficulty*config.MonsterDifficulty, 1, config.DropMultiplier)
			if m.gold <= 0 || m.hp <= 0 || m.attack <= 0 {
				t.Fatal(m)
			}
			count := []int{cfg.Waves["deep"].Normal, cfg.Waves["deep"].Elite, cfg.Waves["deep"].Boss}[i]
			gold += int64(count) * m.gold
			clearAir += float64(count) * battleAirReward(l, limit, battleTrainingShare(cls))
			profiles = append(profiles, fmt.Sprintf("%d／%d／%d／%d", m.hp, m.armor, m.attack, m.gold))
		}
		clears := fmt.Sprint(int64(math.Ceil(float64(int64(limit)-int64(limit*.1)) / clearAir)))
		switch level {
		case 1:
			clears = "开局已满"
		case 90:
			clears = "封顶"
		}
		fmt.Fprintf(&table, "| %s | %d | %d | %d | %d／%d／%d | %s | %s | %s | %d | %.2f×／%.4f%%／%s |\n", gameLevelName(l), roleInt(bare, "Perception"), basePower, power, stats.attack, stats.defense, stats.blood, results[0], results[1], results[2], gold, lateRealmTimeFactor(l), 100*clearAir/limit, clears)
		fmt.Fprintf(&monsters, "| %s | %s | %s | %s | %d–%d |\n", gameLevelName(l), profiles[0], profiles[1], profiles[2], battleMaterialRank(level, 0), battleMaterialRank(level, 2))
	}
	if out != "" {
		doc, err := os.ReadFile(out)
		if err != nil {
			t.Fatal(err)
		}
		intro, _, found := strings.Cut(string(doc), "<!-- 逐层数据 -->")
		if !found {
			t.Fatal("missing table marker")
		}
		content := intro + "<!-- 逐层数据 -->\n\n## 九境炼神、炼体的实战差异\n\n取每境第五层，每组都穿同层中间品阶四件。每种投入各打 500 场同层兽王，九境共 18,000 场。各组使用相同的五行、属性波动与命中随机种子，逐组读取服务端当前的实战属性与训练上限。其余走位、技能组和命中假设与逐层表一致。胜局秒数只计打赢的场次。\n\n" + training.String() + "\n## 九十层角色与击杀成本\n\n" + table.String() + "\n## 同层怪物基准\n\n每格依次为血量、韧性、基础攻击、掉落灵石均值。前三项为配置中值。\n\n" + monsters.String() + "\n## 大境交界与准备收益\n\n每格仍为 500 场兽王单挑的胜率／胜局秒数／胜时剩余血韧。双属性修满使用该层实际训练上限。跨境保持本层角色，选择下一层兽王。\n\n" + builds.String()
		if err := os.WriteFile(out, []byte(content), 0644); err != nil {
			t.Fatal(err)
		}
	}
}

func TestBattleBalanceAffixesAndCaps(t *testing.T) {
	s := &Server{dataDir: "../server/data"}
	if err := s.loadGameTables(); err != nil {
		t.Fatal(err)
	}
	r := testBattleRole(s, 90, true)
	stats := equippedBattleStats(r)
	spell := battleSkills[0]
	before := resolvePlayerSpell(r, stats.attack, spell)
	r["ArtisanResearch"] = Record{"4": Record{"Rank": 9}}
	after := resolvePlayerSpell(r, stats.attack, spell)
	if after.skill.castMS != 91 || before.skill.castMS != 100 {
		t.Fatal(before, after)
	}
	p0 := battlePower(r)
	r["EquippedMethodEffects"] = Record{"106": 35, "301": 22, "601": 20}
	if battlePower(r) <= p0 {
		t.Fatal("targeted method affixes should affect neutral rating")
	}
	m := battleMonster{attack: 100, element: 1, objectType: 2002, unusual: 4}
	skill := m.skill(1)
	if m.skillDamage(skill, [6]int{1: 22}, 0) != 78 {
		t.Fatal("gold resistance not applied to bite")
	}
	rates := dropRates(3, 20)
	if math.Abs(rates.equipment-.216) > .000001 {
		t.Fatal(rates)
	}
	for _, u := range []int{1, 3, 4} {
		g := rollBattleDrops(battleMonster{unusual: u, level: 50}, 0, 0, 0, func() float64 { return 0 })
		if len(g) != 3 || g[1].goodsType != 19 || battleEquipmentSlot(Record{"Type": g[2].goodsType}) == noEquipmentSlot {
			t.Fatal(g)
		}
	}
	for _, mapid := range []string{"1111W11", "1111W12", "1111W13", "1111W63"} {
		l, wave := encounterLevel(10, mapid)
		if want := map[string]int64{"1111W11": 9, "1111W12": 10, "1111W13": 11, "1111W63": 13}[mapid]; l != want || wave == "default" {
			t.Fatal(mapid, l, wave)
		}
	}
	level := s.levelInfo(90)
	r["SuperAir"] = int64(level.upgradeLimit * 1.1)
	for range 150 {
		s.grantBattleCultivation(r, battleMonster{level: 90, unusual: 4})
	}
	if roleInt(r, "SuperAir") != int64(level.upgradeLimit*1.1) || roleInt(r, "Perception") != level.trainingCap(1) || roleInt(r, "Physique") != level.trainingCap(2) {
		t.Fatal("late-stage kill cap", r)
	}
}

func TestBattleElementalLoadoutOnWire(t *testing.T) {
	s := auctionTestServer(t, t.TempDir())
	s.dataDir = "../server/data"
	var err error
	s.config, err = readConfig(s.dataDir)
	if err != nil {
		t.Fatal(err)
	}
	if err = s.loadGameTables(); err != nil {
		t.Fatal(err)
	}
	p, conn := auctionTestRole(t, s, "balance", nil)
	r := testBattleRole(s, 41, true)
	r["Id"], r["UserId"], r["EquippedMethodEffects"] = "balance", "test", Record{"106": 25, "110": 12, "301": 20, "305": 35}
	r["ArtisanResearch"] = Record{"4": Record{"Rank": 9}}
	if err := s.roles.Put("balance", r); err != nil {
		t.Fatal(err)
	}
	joined := spellTestCall(t, s, p, 1010002, "RoleJoinDungeonReq_1010002", "RoleJoinDungeonResp_1010002", Record{"MapId": "1111W13"})
	slots := joined.Get(field(joined, "Skills")).List()
	if getInt(slots.Get(0).Message(), "CastTime") != 91 {
		t.Fatal("scribe bonus missing", joined)
	}
	flying := slots.Get(2).Message()
	if getFloat(flying, "GoldDamageAdd") != 1.25 || getFloat(flying, "EarthDamageAdd") != 1.12 || getFloat(flying, "WoodDamageAdd") != 1 ||
		getInt(flying, "UsedPerception") != roleInt(r, "Perception")/10 || getInt(flying, "UsedSuperAir") != roleInt(r, "Perception")/20 {
		t.Fatal("artifact or elemental affinities not delivered", flying)
	}
	d := s.dungeonForRole(getString(joined, "DungeonId"), "balance", false)
	req := s.pb("BattleRoleEnterReq_8001001")
	putPB(req, Record{"RoleId": "balance", "DungeonId": d.id})
	body, err := proto.Marshal(req)
	if err != nil {
		t.Fatal(err)
	}
	s.handleBattle(p, Message{Cmd: 8001001, Msid: 57, Data: body})
	wire := conn.receive(t, s, 8001001, 57, "BattleRoleEnterResp_8001001")
	for _, obj := range repeatedBagItems(wire, "Objects") {
		m := d.monsters[getInt(obj, "ObjectIndex")-1001]
		skills := repeatedBagItems(obj, "Skills")
		for _, row := range skills {
			skill := m.skill(int(getInt(row, "SkillIndex")))
			want := m.skillDamage(skill, [6]int{1: 20, 5: 35}, 0)
			if getFloat(row, "BaseDamage") != want {
				t.Fatal(m, skill, want, row)
			}
		}
	}
}

func TestCombatMeridiansFromSavedAcupoints(t *testing.T) {
	s := acupointTestServer(t, t.TempDir())
	s.config.BattleDurationSeconds = 120
	peer, conn := auctionTestRole(t, s, "veins", nil)
	r := Record{"Id": peer.roleID, "Level": 1, "Perception": 10000, "Physique": 10000, "AcupointValue": 1}
	if err := s.roles.Put(peer.roleID, r); err != nil {
		t.Fatal(err)
	}
	if _, err := s.applyAcupointBreak(peer.roleID, "1", func(int) int { return 0 }); err != nil {
		t.Fatal(err)
	}
	r = s.roles.Get(peer.roleID)
	if stats := equippedBattleStats(r); stats.blood != 40200 || roleInt(r, "CurrentPower") != battlePower(r) {
		t.Fatalf("first 1-point acupoint should give +0.5%% battle blood immediately: %+v", stats)
	}
	for id := range s.game.acupoints {
		setAcupointLevel(r, id, 3)
	}
	delete(r, "BattleMeridianBonuses")
	s.ensureGameRole(r) // Existing saves acquire bonuses from their recorded acupoints.
	base := cloneRecord(r)
	delete(base, "BattleMeridianBonuses")
	for key, want := range map[string]int64{"PureBloodValueUp": 1200, "PureBloodRecoveryUp": 200000, "PerceptionValueUp": 1000,
		"FiveElementAttackUp": 3300, "PhysiqueValueUp": 800, "FiveElementDefenseUp": 2700} {
		if got := battleMeridianBPS(r, key); got != want {
			t.Fatalf("%s %d, expected %d", key, got, want)
		}
	}
	stats := equippedBattleStats(r)
	if stats.perception != 11000 || stats.attack != 8800 || stats.defense != 10800 || stats.blood != 48608 ||
		battlePower(r) <= battlePower(base) || roleInt(r, "Physique") != 10000 || roleInt(r, "Perception") != 10000 {
		t.Fatal(stats, r)
	}
	if got := resolvePlayerSpell(r, stats.attack, battleSkills[0]).damage; math.Abs(got-11704) > 1e-8 {
		t.Fatal("elemental attack not applied exactly once", got)
	}
	if got := resolvePlayerSpell(r, stats.attack, battleSkills[3]).damage; got != 17600 {
		t.Fatal("damage bonus should not also amplify defensive spells", got)
	}
	if err := s.roles.Put(peer.roleID, r); err != nil {
		t.Fatal(err)
	}
	joined := spellTestCall(t, s, peer, 1010002, "RoleJoinDungeonReq_1010002", "RoleJoinDungeonResp_1010002", Record{"MapId": "1111W13"})
	if getInt(joined, "CurrentPerception") != 11000 || getInt(joined, "CurrentSuperAir") != 22000 ||
		getInt(joined, "CurrentPureBloodDefense") != 48608 || getInt(joined, "CurrentPhysiqueDefense") != 10800 ||
		fieldEntrySupplies(t, joined.GetUnknown())[31] != 10500 || getFloat(joined, "DefenseRecoverValueOnBattle") != 216 {
		t.Fatal("incorrect effective stats or rest rate on battle admission", joined)
	}
	equipped := spellTestCall(t, s, peer, 1009005, "", "EnterEquipmentResp_1009005", nil)
	if getInt(equipped, "FullLoadPerception") != 11000 || getFloat(equipped, "SuperAirPower") != 11704 {
		t.Fatal("equipment sheet disagrees with combat", equipped)
	}
	d := s.dungeonForRole(getString(joined, "DungeonId"), peer.roleID, false)
	d.monsters = []battleMonster{{index: 1001, level: 1, objectType: 2002, unusual: 1, element: 1, attack: 1000}}
	req := s.pb("BattleRoleEnterReq_8001001")
	putPB(req, Record{"RoleId": peer.roleID, "DungeonId": d.id})
	data, err := proto.Marshal(req)
	if err != nil {
		t.Fatal(err)
	}
	s.handleBattle(peer, Message{Cmd: 8001001, Msid: 47, Data: data})
	fieldReply := conn.receive(t, s, 8001001, 47, "BattleRoleEnterResp_8001001")
	enemy := repeatedBagItems(fieldReply, "Objects")[0]
	if bite := repeatedBagItems(enemy, "Skills")[0]; getFloat(bite, "BaseDamage") != 787 {
		t.Fatal("27% five-element defense should reduce 1000 to round(1000/1.27)", bite)
	}
}

// Prove sub-point kill rewards survive separate transactions and cannot be
// rounded up per kill. Once a bar fills it cannot bank fractions for a breakthrough.
func TestLateEncounterAirAndAttributeRewards(t *testing.T) {
	s := &Server{dataDir: "../server/data"}
	if err := s.loadGameTables(); err != nil {
		t.Fatal(err)
	}
	for level := int64(1); level <= 90; level++ {
		lvl := s.levelInfo(level)
		for _, tier := range []int{1, 3, 4} {
			start := testBattleRole(s, int(level), false)
			s.grantBattleCultivation(start, battleMonster{level: int(level), unusual: tier})
			share := battleTrainingShare(tier)
			expectedAir := math.Round(lvl.upgradeLimit * share)
			if level > 40 {
				expectedAir = lvl.upgradeLimit * share / lateRealmTimeFactor(level)
			}
			got := float64(roleInt(start, "SuperAir")) + roleFloat(start, "BattleAirRemainder")
			if math.Abs(got-expectedAir) > 1e-6 {
				t.Fatalf("level %d tier %d: air %.10f want %.10f", level, tier, got, expectedAir)
			}
			base := int64(lvl.perception)
			attr := float64(lvl.trainingCap(1)-base) * share
			if level <= 40 {
				attr = max(1, math.Round(attr))
			} else {
				attr /= math.Sqrt(lateRealmTimeFactor(level))
			}
			for _, key := range []string{"Perception", "Physique"} {
				actual := float64(roleInt(start, key)-base) + roleFloat(start, "Battle"+key+"Remainder")
				if math.Abs(actual-attr) > 1e-6 || roleInt(start, "Current"+key) != roleInt(start, key) {
					t.Fatalf("level %d tier %d: %s %.10f want %.10f", level, tier, key, actual, attr)
				}
			}
		}
	}
	dir := t.TempDir()
	db, err := OpenStore(dir, "roles")
	if err != nil {
		t.Fatal(err)
	}
	if err := db.Put("fraction", Record{"SuperAir": 8}); err != nil {
		t.Fatal(err)
	}
	grant := func() {
		if err := db.Update("fraction", func(r Record) error {
			creditBattleProgress(r, "SuperAir", "BattleAirRemainder", .125, 10)
			return nil
		}); err != nil {
			t.Fatal(err)
		}
	}
	for range 7 {
		grant()
	}
	db, err = OpenStore(dir, "roles")
	if err != nil {
		t.Fatal(err)
	}
	role := db.Get("fraction")
	if roleInt(role, "SuperAir") != 8 || roleFloat(role, "BattleAirRemainder") != .875 {
		t.Fatal(role)
	}
	grant()
	if roleInt(db.Get("fraction"), "SuperAir") != 9 {
		t.Fatal("saved fraction did not grant the next point")
	}
	for range 12 {
		grant()
	}
	role = db.Get("fraction")
	if roleInt(role, "SuperAir") != 10 || role["BattleAirRemainder"] != nil {
		t.Fatal("banked gains over the cap", role)
	}
	role = testBattleRole(s, 80, false)
	base := roleInt(role, "Perception")
	role["Physique"] = s.levelInfo(80).trainingCap(2)
	s.grantBattleCultivation(role, battleMonster{level: 1, unusual: 1})
	if roleInt(role, "Perception") != base || roleFloat(role, "BattlePerceptionRemainder") <= 0 || role["BattlePhysiqueRemainder"] != nil {
		t.Fatalf("low-level encounters should bank only uncapped attributes: %v", role)
	}
}
