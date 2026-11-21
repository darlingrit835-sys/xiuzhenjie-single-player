package main

import (
	"fmt"
	"math"
	"strings"
	"testing"
	"time"
)

type realmCalendarPoint struct {
	clock, online, visits, age, spent int64
}
type realmCalendar struct {
	stages   [9]realmCalendarPoint
	complete realmCalendarPoint
}

// Re-evaluate the route only while the player is present. An offline interval
// keeps its original route even after the first of two training lanes fills.
func modelRealmCalendar(t *testing.T, s *Server, interval, goal int64) realmCalendar {
	return runRealmCalendar(t, s, interval, goal, nil)
}

func runRealmCalendar(t *testing.T, s *Server, interval, goal int64, practice *realmEconomyCalendar) realmCalendar {
	t.Helper()
	first := s.levelInfo(1)
	r := Record{"HasGold": true, "RunMask": 4, "Level": int64(1), "CurrentAge": 16, "MaxAge": 100, "CurrentVigor": 100, "Vigor": 100,
		"Perception": int64(first.perception), "Physique": int64(first.toughTenacity), "SuperAir": realmStartingAir(1, first)}
	const session int64 = 15 * 60
	const epoch int64 = 1000000
	var clock, online, visits, spent int64
	var result realmCalendar
	var carry [3]float64
	keys := [3]string{"Perception", "Physique", "SuperAir"}
	for clock < 10*365*86400 {
		if (interval > 0 && clock%interval == 0) || (interval == 0 && clock == 0) {
			visits++
		}
		l := roleInt(r, "Level")
		lvl := s.levelInfo(l)
		targetPct := max(goal, breakthroughAttributePercent(l))
		target := [3]int64{attributeTarget(lvl, 1, targetPct), attributeTarget(lvl, 2, targetPct), int64(lvl.upgradeLimit)}
		recoverRoleVigor(r, epoch+clock)
		if practice != nil {
			practice.event(t, r, clock)
		}
		inField := practice != nil && practice.field != nil
		attrsReady := roleInt(r, keys[0]) >= target[0] && roleInt(r, keys[1]) >= target[1]
		if l == 90 && attrsReady && (practice == nil || !practice.morningPending(clock)) {
			result.complete = realmCalendarPoint{clock, online, visits, roleInt(r, "CurrentAge"), spent}
			if practice != nil {
				practice.complete = practice.snapshot(r, clock)
			}
			return result
		}
		if !inField && l < 90 && attrsReady && s.canBreak(r) && s.breakthroughAttributeCode(r) == 0 && breakthroughResourceCode(r) == 0 {
			_, cost := breakthroughCost(l)
			spent += cost
			s.levelUpGameRoleAt(r, epoch+clock)
			if l%10 == 0 || l == 89 {
				result.stages[(l-1)/10] = realmCalendarPoint{clock, online, visits, roleInt(r, "CurrentAge"), spent}
				if practice != nil {
					practice.stages[(l-1)/10] = practice.snapshot(r, clock)
				}
			}
			l++
			lvl = s.levelInfo(l)
			targetPct = max(goal, breakthroughAttributePercent(l))
			target = [3]int64{attributeTarget(lvl, 1, targetPct), attributeTarget(lvl, 2, targetPct), int64(lvl.upgradeLimit)}
			carry = [3]float64{}
			if l == 90 && roleInt(r, keys[0]) >= target[0] && roleInt(r, keys[1]) >= target[1] && (practice == nil || !practice.morningPending(clock)) {
				result.complete = result.stages[8]
				if practice != nil {
					practice.complete = practice.stages[8]
				}
				return result
			}
		}
		mask := int64(0)
		// Qi plus one deficient attribute in dual training. Before level 31,
		// meet the chosen attribute target first so there is no fictitious triple training.
		if l >= 31 && l < 90 && roleInt(r, "SuperAir") < target[2] {
			mask = 4
		}
		for i := 0; i < 2; i++ {
			if roleInt(r, keys[i]) < target[i] {
				mask |= 1 << i
				if l < 31 || mask&(mask-1) != 0 {
					break
				}
			}
		}
		if mask == 0 {
			mask = 4
		}
		if inField {
			mask = roleInt(r, "RunMask")
		}
		r["RunMask"] = mask
		offline := interval > 0 && clock%interval >= session
		var rates [3]float64
		dt := int64(86400 * 12)
		if interval > 0 {
			if offline {
				dt = interval - clock%interval
				carry = [3]float64{}
			} else {
				dt = session - clock%interval
			}
		}
		if practice != nil {
			dt = min(dt, practice.nextEvent(clock))
		}
		for i, key := range keys {
			if mask&(1<<i) == 0 {
				continue
			}
			r[gameEquipField(i+1)] = bestFreeMethod(s, r, i+1).id
			rates[i] = s.gameRate(r, i+1)
			if i < 2 {
				rates[i] *= .5
			}
			if offline {
				rates[i] *= .8
				continue
			}
			if missing := target[i] - roleInt(r, key); missing > 0 && rates[i] > 0 {
				step := 2 * int64(math.Ceil((float64(missing)-carry[i])/(2*rates[i])))
				dt = min(dt, max(2, step))
			}
		}
		_, cost := breakthroughCost(l)
		if !offline && roleInt(r, "CurrentVigor") < cost {
			dt = min(dt, 2)
		}
		if dt <= 0 {
			t.Fatal("simulation step", clock, interval)
		}
		active := dt
		if offline {
			active = min(dt, int64(s.config.OfflineCapHours)*3600)
		} else {
			online += dt
		}
		limits := [3]int64{lvl.trainingCap(1), lvl.trainingCap(2), int64(lvl.upgradeLimit)}
		var lived int64
		for i, key := range keys {
			if mask&(1<<i) == 0 {
				continue
			}
			seconds := productiveTrainingSeconds(r, key, float64(limits[i]), rates[i], active)
			lived = max(lived, seconds)
			whole, fraction := math.Modf(float64(seconds)*rates[i] + carry[i])
			carry[i] = fraction
			if offline {
				carry[i] = 0
			}
			r[key] = min(limits[i], roleInt(r, key)+int64(whole))
		}
		ageCultivation(r, lived)
		clock += dt
	}
	t.Fatal("calendar did not finish", r)
	return result
}

func realmCalendarReport(t *testing.T, s *Server) string {
	fast, onceDaily := modelRealmCalendar(t, s, 43200, 100), modelRealmCalendar(t, s, 86400, 100)
	twice, once := fast.stages, onceDaily.stages
	balanced := modelRealmCalendar(t, s, 43200, 120)
	trained := modelRealmCalendar(t, s, 43200, 140)
	routes := [3]realmCalendar{modelRealmCalendar(t, s, 0, 100), modelRealmCalendar(t, s, 0, 120), modelRealmCalendar(t, s, 0, 140)}
	var out strings.Builder
	fmt.Fprint(&out, "\n## 日历耗时、寿元与精力\n\n每天固定上线一次或早晚各一次，每次在线 15 分钟，期间修满便手动突破。离线时最多结算 12 小时，满层后等待下次上线。模拟从 16 岁满精力开始，按真实逐层门槛、恢复、寿元预留和在线两秒结算推进，洞府与阵法零投入，未服丹和打怪，雷劫均一次通过。旧新版连续纳气列仅用于比较修为速度。两种登录习惯均在大境关口补齐两项属性再突破，筑基十层起双属性最低由底值 102% 逐境增到 114%。每境十层计入升入下境，大乘计到九十级。\n\n| 完成本境 | 旧版在线小时 | 新版在线小时 | 早晚各一次本境天数 | 早晚各一次累计天数 | 每日一次累计天数 | 早晚档累计在游戏内小时 | 早晚档年龄／新寿限 | 本境冲关耗精力 |\n|---|---:|---:|---:|---:|---:|---:|---:|---:|\n")
	var oldTotal, newTotal float64
	for stage := range twice {
		var oldMin, newMin float64
		for l := int64(10*stage + 1); l <= int64(10*stage+10) && l < 90; l++ {
			r := Record{"Level": l, "HasGold": true, "RunMask": 4}
			r["GongFaNaQi"] = bestFreeMethod(s, r, 3).id
			lvl := s.levelInfo(l)
			points := float64(int64(lvl.upgradeLimit) - realmStartingAir(l, lvl))
			sec := points / s.gameRate(r, 3)
			newMin += 2 * math.Ceil(sec/2) / 60
			oldMin += 2 * math.Ceil(sec/lateRealmTimeFactor(l)/2) / 60
		}
		newTotal += newMin / 60
		oldTotal += oldMin / 60
		v := twice[stage]
		prev := realmCalendarPoint{}
		if stage > 0 {
			prev = twice[stage-1]
		}
		l := min(int64(90), int64(stage+1)*10+1)
		fmt.Fprintf(&out, "| %s | %.2f | %.2f | %.2f | %.2f | %.2f | %.2f | %d／%d | %d |\n", strings.TrimSuffix(gameLevelName(int64(stage*10+1)), "1层"), oldMin/60, newMin/60, float64(v.clock-prev.clock)/86400, float64(v.clock)/86400, float64(once[stage].clock)/86400, float64(v.online)/3600, v.age, gameLifespan(l), v.spent-prev.spent)
	}
	v, w := twice[8], once[8]
	fmt.Fprintf(&out, "\n连续在线纯纳气由 %.2f 小时变为 %.2f 小时（%.1f 天）。速冲样本早晚档共 %d 次登录、%.1f 天、游戏打开 %.1f 小时；每日一次共 %d 次登录、%.1f 天、游戏打开 %.1f 小时。若每次用约 3 分钟处理突破、领产物和派下一批，早晚档约 %.1f 小时手动操作，每日一次约 %.1f 小时。3 分钟是操作预算，未作玩家实测；单次 15 分钟中的其余时间可以挂机。额外刷装备、重打雷劫或装修洞府所花的时间视个人目标另计。\n", oldTotal, newTotal, newTotal/24, v.visits, float64(v.clock)/86400, float64(v.online)/3600, w.visits, float64(w.clock)/86400, float64(w.online)/3600, float64(v.visits)*3/60, float64(w.visits)*3/60)
	fmt.Fprint(&out, "\n### 把炼神、锻体和纳气一起计算\n\n速冲每到大境关口才补双属性。均衡每层双属性至少达到底值的120%，备战每层两项都达到140%。突破继承已练出的数值，下一层底值较高时自动补足。31级前轮换单修；31级起优先纳气加一项未达标属性，两项有缺口且修为已满时改为炼神兼锻体。只在玩家在线时切换，离线可能练过目标，也可能一路先满、另一条继续七成运转。连续在线列假设到目标立刻换路，早晚档每次15分钟。\n\n| 终点 | 速冲本境在线小时 | 均衡本境在线小时 | 备战本境在线小时 | 速冲累计天 | 均衡累计天 | 备战累计天 | 双满档年龄／寿限 |\n|---|---:|---:|---:|---:|---:|---:|---:|\n")
	for i := range twice {
		var hours [3]float64
		for j, run := range routes {
			prior := int64(0)
			if i > 0 {
				prior = run.stages[i-1].clock
			}
			hours[j] = float64(run.stages[i].clock-prior) / 3600
		}
		fmt.Fprintf(&out, "| %s | %.2f | %.2f | %.2f | %.2f | %.2f | %.2f | %d／%d |\n", strings.TrimSuffix(gameLevelName(int64(10*i+1)), "1层"), hours[0], hours[1], hours[2], float64(twice[i].clock)/86400, float64(balanced.stages[i].clock)/86400, float64(trained.stages[i].clock)/86400, trained.stages[i].age, gameLifespan(min(90, int64(10*i+11))))
	}
	for i, run := range []realmCalendar{fast, balanced, trained} {
		r := run.complete
		fmt.Fprintf(&out, "\n%s路线到九十级且完成本路线的末层目标，连续在线 %.2f 小时，早晚各一次 %.2f 天，累计上线 %.2f 小时。", []string{"速冲", "均衡", "备战"}[i], float64(routes[i].complete.clock)/3600, float64(r.clock)/86400, float64(r.online)/3600)
	}
	fmt.Fprint(&out, "\n\n两类属性培养时间使用纳气时长倍率的平方根，最高十倍。真实击杀会同时补神识和体魄，打过后再切换修行可减少上述纯修行预算。神识、体魄对气血、攻防、真元和首领胜率的独立贡献见[逐层战斗平衡](逐层战斗平衡.md)。\n")

	fmt.Fprint(&out, "\n初始寿限 100 岁，各大境翻倍。行功每累计 2 小时长一岁，三条修行都参与计时，兼修按实际修行时间取较长一路。所有运行项已满、未登录且超过离线上限的时间不计。首层免费，此后每次突破消耗本境基础寿限的 4%，同时消耗 2 至 10 精力。接近寿限会预留本境余下各层冲关所需年数及 1 年生命，暂停被动行功；历练、服丹、传功仍能推进。筑基起玄天楼各境寿元丹终生各服一次，依次加 20 至 2,560 年。\n\n精力初始 100，二层起上限为 100＋5×等级。在线、离线都每 120 秒恢复 1 点，上限 550。满精停止积攒，突破提升容量。首场历练免费，后续外围／深处／兽王巢每场耗 2／4／6。同一副本重连沿用已付门票。手炼、符谱、器模批量费按各自生产单结算。师尊传功／通脉每次耗 5／20，弟子领取的修为按自己领取时的时长倍率折算，通脉点独立。\n")
	return out.String() + realmEconomyReport(t, s, balanced.complete)
}

func TestRealmResources(t *testing.T) {
	r := Record{"Level": 2, "CurrentVigor": int64(0), "Vigor": 110, "VigorRecoveredAt": int64(1000)}
	if n := recoverRoleVigor(r, 1239); n != 1 || roleInt(r, "CurrentVigor") != 1 || recoverRoleVigor(r, 1240) != 1 {
		t.Fatal(r)
	}
	r["VigorRecoveredAt"] = time.Now().Unix()
	r["CurrentVigor"] = 1
	if spendArtisanVigor(r, 2) || !spendArtisanVigor(r, 1) || spendArtisanVigor(r, 1) {
		t.Fatal(r)
	}
	r["CurrentAge"], r["MaxAge"] = 62, 100
	if remainingTrainingLifetime(r) != cultivationYearSeconds || productiveTrainingSeconds(r, "SuperAir", 1000, 1, 20) != 20 {
		t.Fatal(r)
	}
	ageCultivation(r, cultivationYearSeconds)
	if remainingTrainingLifetime(r) != 0 || roleInt(r, "CurrentAge") != 63 {
		t.Fatal(r)
	}
	for _, tc := range []struct {
		level  int64
		factor float64
	}{{40, 1}, {50, math.Sqrt(10)}, {60, 10}, {70, 10 * math.Sqrt(10)}, {80, 100}, {89, 100}, {90, 100}} {
		if math.Abs(lateRealmTimeFactor(tc.level)-tc.factor) > 1e-9 {
			t.Fatal(tc)
		}
		pending := Record{"Type": mentorPower, "Amount": 10000}
		r["Level"] = tc.level
		if mentorPracticeAmount(r, pending) != int64(math.Round(10000/tc.factor)) {
			t.Fatal(tc)
		}
	}
}

func TestRealmGateResourcesAndConcurrentTraining(t *testing.T) {
	s := auctionTestServer(t, t.TempDir())
	s.dataDir = "../server/data"
	cfg, err := readConfig(s.dataDir)
	if err != nil {
		t.Fatal(err)
	}
	s.config = cfg
	if err = s.loadGameTables(); err != nil {
		t.Fatal(err)
	}
	const level int64 = 80
	lvl := s.levelInfo(level)
	target := attributeTarget(lvl, 1, breakthroughAttributePercent(level))
	peer, conn := auctionTestRole(t, s, "both", nil)
	if err = s.roles.Update("both", func(r Record) error {
		r["Level"], r["SuperAir"], r["MaxAge"], r["CurrentAge"] = level, int64(lvl.upgradeLimit), gameLifespan(level), 100
		r["Perception"], r["Physique"], r["CurrentVigor"] = target, target-1, 0
		r["VigorRecoveredAt"] = time.Now().Unix()
		return nil
	}); err != nil {
		t.Fatal(err)
	}
	breakWith := func(want int64) {
		t.Helper()
		for _, cmd := range []int{1001008, 1001010} {
			conn.frames = nil
			s.handleGame(peer, Message{Cmd: cmd, Msid: 1})
			reply := conn.receive(t, s, cmd, 1, gameCultivationProtocols[cmd].resp)
			success := want == 4306
			if getBool(reply, "IsSuccess") != success || (!success || cmd == 1001010) && getInt(reply, "Code") != want {
				t.Fatal(reply)
			}
		}
	}
	breakWith(4985)
	s.roles.Update("both", func(r Record) error { r["Physique"] = target; return nil })
	breakWith(4290)
	s.roles.Update("both", func(r Record) error { r["CurrentVigor"] = 20; return nil })
	breakWith(4306)
	r := s.roles.Get("both")
	if roleInt(r, "Level") != 81 || roleInt(r, "CurrentAge") != 612 || roleInt(r, "CurrentVigor") != 11 {
		t.Fatal(r)
	}
	if age := conn.receive(t, s, 9001007, 0, "AgeUpResp_9001007"); getInt(age, "Age") != 612 || getInt(age, "MaxAge") != 25600 {
		t.Fatal(age)
	}

	// Two lanes run for one hour together. Neither reaches cap in this interval.
	const unix int64 = 1900000000
	lvl = s.levelInfo(85)
	r = Record{"Level": 85, "HasGold": true, "RunMask": 3, "CurrentAge": 800, "MaxAge": gameLifespan(85), "CurrentVigor": 0, "Vigor": 525,
		"SuperAir": int64(0), "Perception": int64(lvl.perception), "Physique": int64(lvl.toughTenacity), "LastOnline": (unix - 3600) * 1000, "CultivationYearSeconds": int64(3600), "VigorRecoveredAt": unix - 3600}
	r["GongFaLianShen"], r["GongFaDuanTi"] = bestFreeMethod(s, r, 1).id, bestFreeMethod(s, r, 2).id
	s.applyOfflineAt(r, unix*1000)
	for i, key := range []string{"Perception", "Physique"} {
		want := int64(s.gameRate(r, i+1) * .5 * .8 * 3600)
		if got := roleInt(r, "Offline"+key); got != want {
			t.Fatal(key, got, want)
		}
	}
	if roleInt(r, "CurrentAge") != 801 || roleInt(r, "CurrentVigor") != 30 || roleInt(r, "CultivationYearSeconds") != 0 {
		t.Fatal(r)
	}
	r["RunMask"], r["LastOnline"] = 4, (unix-48*3600)*1000
	s.applyOfflineAt(r, unix*1000)
	if roleInt(r, "OfflineTime") != 43200 || roleInt(r, "CurrentAge") != 807 {
		t.Fatal(r)
	}
	value := roleInt(r, "SuperAir")
	s.applyOfflineAt(r, unix*1000)
	if roleInt(r, "SuperAir") != value || roleInt(r, "CurrentAge") != 807 {
		t.Fatal("relogin duplicated offline time", r)
	}
}
