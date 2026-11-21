package main

import (
	"context"
	"encoding/json"
	"errors"
	"log"
	"slices"
	"sort"
	"time"
)

const (
	guildCourtRead = 4901030
	guildCourtAct  = 4901031
	mineBeat       = int64(15 * 60)
	courtStockCap  = int64(1_000_000_000)
)

var sectZone = time.FixedZone("UTC+8", 8*3600)

// All rounds close at 20:00 local. At a coincident deadline the monthly wave
// replaces the Sunday wave. The exact closing instant belongs to the new round.
func nextSectDeadline(at time.Time) time.Time {
	at = at.In(sectZone)
	candidate := time.Date(at.Year(), at.Month(), at.Day(), 20, 0, 0, 0, sectZone)
	if !candidate.After(at) {
		candidate = candidate.AddDate(0, 0, 1)
	}
	for candidate.Weekday() != time.Sunday && candidate.AddDate(0, 0, 1).Day() != 1 {
		candidate = candidate.AddDate(0, 0, 1)
	}
	return candidate
}

func sectLarge(deadline int64) bool {
	return time.Unix(deadline, 0).In(sectZone).AddDate(0, 0, 1).Day() == 1
}

func sectMap(parent Record, key string) Record {
	row, _ := parent[key].(Record)
	if row == nil {
		row = Record{}
		parent[key] = row
	}
	return row
}

func sectStage(g trialGuild) int64 { return min(9, max(1, roleInt(g.row, "Stage"))) }

func sectBuildCosts(stage int64) (ore, crystal int64) {
	scope := 1 + (stage-1)/3
	return 80 * stage * stage * scope, 32 * stage * stage * scope
}

// Reject maps that have no county ancestor when deciding whether a player
// actually lives at a proposed site. A continent or world ID is not a residence.
func sectCountyID(location string) string {
	for node := gameMapFind(location); node != nil; node = gameMapFind(node.parent) {
		if node.kind == 3 {
			return node.id
		}
	}
	return ""
}

// Old guild saves can lack a valid forest. Use their leader's home county
// for that migration and persist it in Court; keep the original MapId intact.
func (s *Server) legacySectCounty(g trialGuild) string {
	if county := sectCountyID(roleString(g.row, "MapId")); county != "" {
		return county
	}
	home := s.roles.project(g.leader(), []string{"MapId"})
	return fieldCounty(roleString(home, "MapId"))
}

func sectRegion(g trialGuild) (county, province, land, bound *gameMap) {
	state, _ := g.row["Court"].(Record)
	county = gameMapFind(fieldCounty(roleString(state, "County"), roleString(g.row, "MapId")))
	province = gameMapFind(county.parent)
	land = gameMapFind(province.parent)
	bound = []*gameMap{county, province, land}[(sectStage(g)-1)/3]
	return
}

func startSectRound(g trialGuild, state Record, at time.Time) {
	deadline := nextSectDeadline(at).Unix()
	stage := sectStage(g)
	target := 90*stage + min(4, max(0, int64(len(g.members())-1)))*18*stage
	if sectLarge(deadline) {
		target = target * 3 / 2
	}
	names := []string{"苍狼群", "赤羽兽群", "铁鬃兽群", "雾隐蛇群"}
	foe := names[(deadline/86400+g.id)%int64(len(names))]
	if sectLarge(deadline) {
		foe = "兽王·" + foe
	}
	state["Round"] = Record{"Deadline": deadline, "Stage": stage, "Target": target, "Foe": foe, "Echoes": Record{}}
}

func initSectCourt(g trialGuild, at time.Time, county string) Record {
	state := Record{"Born": at.Unix(), "LastVisit": at.Unix(), "Plan": "balanced", "Formation": 1, "LastMine": at.Unix() / mineBeat * mineBeat,
		"Revision": 1, "Accounts": Record{}, "County": county}
	g.row["Court"] = state
	_, _, _, region := sectRegion(g)
	state["RegionId"] = region.id
	if g.row["AiMemberCount"] == nil {
		g.row["AiMemberCount"] = 6
	}
	startSectRound(g, state, at)
	return state
}

func sectLaborAt(g trialGuild, state Record, stage int64) (workers, miners, guards int64) {
	workers = min(1000, max(0, roleInt(g.row, "AiMemberCount"))) + 2*(stage-1)
	miners = workers / 2
	switch roleString(state, "Plan") {
	case "mine":
		miners = workers * 3 / 4
	case "guard":
		miners = workers / 3
	}
	if workers > 0 {
		miners = max(1, miners)
	}
	return workers, miners, workers - miners
}

func sectMineRateAt(g trialGuild, state Record, stage int64) (ore, crystal int64) {
	_, miners, _ := sectLaborAt(g, state, stage)
	if miners == 0 {
		return 0, 0
	}
	ore = miners * stage
	return ore, max(1, ore/4)
}

func sectCredit(state Record, key string, amount int64) {
	state[key] = min(courtStockCap, max(0, roleInt(state, key)+amount))
}

func accrueSectMine(g trialGuild, state Record, until int64) {
	start := roleInt(state, "LastMine")
	end := until / mineBeat * mineBeat
	if end <= start {
		return
	}
	steps := (end - start) / mineBeat
	// A pulse ending exactly at the deadline belongs to production before it.
	slow := max(int64(0), min(end, roleInt(state, "SlowUntil"))-max(start, roleInt(state, "SlowFrom"))) / mineBeat
	ore, crystal := sectMineRateAt(g, state, sectStage(g))
	// Keep fractional units between reads: polling each quarter earns the same as one catch-up.
	for _, v := range []struct {
		key  string
		rate int64
	}{{"Ore", ore}, {"Crystal", crystal}} {
		half := v.rate*(2*steps-slow) + roleInt(state, v.key+"Half")
		sectCredit(state, v.key, half/2)
		state[v.key+"Half"] = half % 2
	}
	state["LastMine"] = end
}

func sectRewards(state, round Record) (ore, fang, merit, lossOre, lossMerit int64, protected bool) {
	m := int64(1)
	if sectLarge(roleInt(round, "Deadline")) {
		m = 2
	}
	stage := roleInt(round, "Stage")
	protected = roleInt(round, "Deadline")-roleInt(state, "Born") < 86400
	lossOre = min(8*m*stage, roleInt(state, "Ore")*m/10)
	if protected {
		lossOre = 0
	}
	return 40 * m * stage, 4 * m * stage, 6 * m, lossOre, 2 * m, protected
}

func sectEchoes(round Record, limit int) []Record {
	echoes, _ := round["Echoes"].(Record)
	rows := make([]Record, 0, len(echoes))
	for _, v := range echoes {
		if e, ok := v.(Record); ok {
			rows = append(rows, e)
		}
	}
	sort.Slice(rows, func(i, j int) bool {
		a, b := roleInt(rows[i], "at"), roleInt(rows[j], "at")
		if a != b {
			return a > b
		}
		return roleString(rows[i], "member") < roleString(rows[j], "member")
	})
	if len(rows) > limit {
		rows = rows[:limit]
	}
	return rows
}

func settleSectRound(state Record) {
	round := state["Round"].(Record)
	entries := round["Echoes"].(Record)
	due := roleInt(round, "Deadline")
	ore, fang, merit, loss, pity, protected := sectRewards(state, round)
	won := len(entries) > 0 && roleInt(round, "Damage") >= roleInt(round, "Target")
	slowUntil := int64(0)
	if !won {
		ore, fang, merit = -loss, 0, pity
		if len(entries) == 0 {
			ore, merit = 0, 0
			if !protected {
				slowUntil = due + 3600
				if sectLarge(due) {
					slowUntil += 3600
				}
				state["SlowFrom"], state["SlowUntil"] = due, slowUntil
			}
		}
	}
	sectCredit(state, "Ore", ore)
	sectCredit(state, "Fang", fang)
	for id := range entries {
		sectCredit(sectMap(sectMap(state, "Accounts"), id), "Merit", merit)
	}
	kind := "small"
	if sectLarge(due) {
		kind = "large"
	}
	report := Record{"foe": round["Foe"], "at": due, "victory": won, "damage": roleInt(round, "Damage"), "target": round["Target"],
		"participants": len(entries), "ore": ore, "fang": fang, "merit": merit, "waveKind": kind, "idle": len(entries) == 0,
		"protected": protected, "slowUntil": slowUntil, "echoes": sectEchoes(round, 12)}
	reports, _ := state["Reports"].([]any)
	state["Reports"] = append([]any{report}, reports[:min(len(reports), 6)]...)
}

// Catch up only the round that had an opportunity to accept submissions and
// the most recent missed deadline. No series of historical idle penalties is applied.
func advanceSectCourt(g trialGuild, state Record, at time.Time) {
	now := at.Unix()
	due := roleInt(state["Round"].(Record), "Deadline")
	// The calendar runs even when every member is offline. Its background
	// settlements must not restart the 72-hour production allowance.
	endMine := min(now, max(roleInt(state, "LastVisit"), roleInt(state, "Born"))+72*3600)
	if due <= now {
		accrueSectMine(g, state, min(endMine, due))
		settleSectRound(state)
		// A Sunday occurs at least once per seven days. Search this bounded window
		// for the latest closure, including a month-end between two Sundays.
		latest := nextSectDeadline(at.AddDate(0, 0, -8))
		for n := nextSectDeadline(latest); !n.After(at); n = nextSectDeadline(latest) {
			latest = n
		}
		if latest.Unix() > due {
			startSectRound(g, state, latest.Add(-time.Second))
			accrueSectMine(g, state, min(endMine, latest.Unix()))
			settleSectRound(state)
		}
		startSectRound(g, state, at)
		seq := roleInt(state, "Revision") + 1
		state["Revision"], state["SharedRevision"] = seq, seq
	}
	accrueSectMine(g, state, endMine)
}

func (s *Server) settleSectWorld(at time.Time) error {
	return s.guilds.Transact(func(tx *StoreTx) error {
		guilds := trialGuilds(tx.View(trialGuildWorld))
		for _, id := range trialGuildIDs(guilds) {
			g := trialGuildByID(guilds, id)
			st, _ := g.row["Court"].(Record)
			round, _ := st["Round"].(Record)
			if round == nil || roleInt(round, "Deadline") > at.Unix() {
				continue
			}
			g = trialGuildByID(trialGuilds(tx.Get(trialGuildWorld)), id)
			advanceSectCourt(g, g.row["Court"].(Record), at)
		}
		return nil
	})
}

func (s *Server) runSectCalendar(ctx context.Context) {
	for {
		now := time.Now()
		next := nextSectDeadline(now)
		if err := s.settleSectWorld(now); err != nil {
			log.Printf("宗门兽潮结算暂缓: %v", err)
			next = now.Add(time.Minute)
		}
		timer := time.NewTimer(time.Until(next))
		select {
		case <-ctx.Done():
			timer.Stop()
			return
		case <-timer.C:
		}
	}
}

type sectTreasure struct {
	id, name, tactic string
	cost             int64
}

var sectGear = []sectTreasure{
	{"tonic", "破阵丹", "strike", 3}, {"sigil", "金刚符", "ward", 4}, {"lens", "寻踪镜", "scout", 5},
}

type sectOrder struct {
	Action   string `json:"action"`
	Value    string `json:"value"`
	Revision int64  `json:"revision"`
	GuildID  int64  `json:"guildId"`
}

type sectRefusal string

func (e sectRefusal) Error() string { return string(e) }

func sectAct(g trialGuild, state Record, id string, role Record, req sectOrder, now int64) error {
	accounts, _ := state["Accounts"].(Record)
	prior, _ := accounts[id].(Record)
	if req.GuildID != g.id || req.Revision < max(1, roleInt(state, "SharedRevision"), roleInt(prior, "LastRevision")) || req.Revision > roleInt(state, "Revision") {
		return sectRefusal("宗务有更新，请刷新重试")
	}
	office := g.officeOf(id)
	stage := sectStage(g)
	formation := roleInt(state, "Formation")
	account := prior
	if account == nil {
		account = Record{}
	}
	switch req.Action {
	case "assign":
		if office != 15 && office != 2 && office != 4 {
			return sectRefusal("须由宗主、内务或矿脉长老派工")
		}
		if !slices.Contains([]string{"balanced", "mine", "guard"}, req.Value) {
			return sectRefusal("未知的派工方案")
		}
		state["Plan"] = req.Value
	case "formation":
		if office != 15 && office != 5 && office != 12 {
			return sectRefusal("须由宗主、护宗或阵法长老布阵")
		}
		if formation >= stage+2 {
			return sectRefusal("阵法已达到当前品阶上限")
		}
		if roleInt(state, "Ore") < 20*formation || roleInt(state, "Crystal") < 8*formation {
			return sectRefusal("升阵所需矿砂或灵晶不足")
		}
		sectCredit(state, "Ore", -20*formation)
		sectCredit(state, "Crystal", -8*formation)
		state["Formation"] = formation + 1
	case "promote":
		if office != 15 {
			return sectRefusal("须由宗主主持晋阶")
		}
		if stage >= 9 {
			return sectRefusal("已达九阶")
		}
		switch req.Value {
		case "", "defense":
			if roleInt(state, "Ore") < 40*stage || roleInt(state, "Fang") < 4*stage {
				return sectRefusal("护山晋阶所需矿砂或兽牙不足")
			}
			sectCredit(state, "Ore", -40*stage)
			sectCredit(state, "Fang", -4*stage)
		case "build":
			if formation < stage {
				return sectRefusal("护宗阵须达到当前宗阶")
			}
			ore, crystal := sectBuildCosts(stage)
			if roleInt(state, "Ore") < ore || roleInt(state, "Crystal") < crystal {
				return sectRefusal("营造晋阶所需矿砂或灵晶不足")
			}
			sectCredit(state, "Ore", -ore)
			sectCredit(state, "Crystal", -crystal)
		default:
			return sectRefusal("须选择护山或营造晋阶")
		}
		g.row["Stage"] = stage + 1
		_, _, _, region := sectRegion(g)
		state["RegionId"] = region.id
	case "redeem":
		index := slices.IndexFunc(sectGear, func(v sectTreasure) bool { return v.id == req.Value })
		if index < 0 {
			return sectRefusal("阁内没有这件战备")
		}
		gear := sectGear[index]
		if roleInt(account, gear.id) > 0 {
			return sectRefusal("该战备已随身携带")
		}
		if roleInt(account, "Merit") < gear.cost {
			return sectRefusal("个人功勋不足")
		}
		sectCredit(account, "Merit", -gear.cost)
		account[gear.id] = 1
	case "fight":
		round := state["Round"].(Record)
		echoes := round["Echoes"].(Record)
		if echoes[id] != nil {
			return sectRefusal("本轮已留影出阵")
		}
		_, miners, guards := sectLaborAt(g, state, sectStage(g))
		stage = roleInt(round, "Stage")
		strength := min(int64(300), max(0, roleInt(role, "CurrentPower"))/60)
		level := min(int64(90), max(1, roleInt(role, "Level")))
		base := stage*65 + level*2 + strength
		var damage int64
		switch req.Value {
		case "strike":
			damage = base + stage*30 + strength/2
		case "ward":
			damage = base + guards*3 + formation*8
		case "scout":
			damage = base + miners*4 + min(50, roleInt(state, "Crystal")/4)
		default:
			return sectRefusal("须选择破阵、固守或探踪")
		}
		for _, gear := range sectGear {
			if gear.tactic == req.Value && roleInt(account, gear.id) > 0 {
				account[gear.id] = 0
				damage = damage * 5 / 4
			}
		}
		echoes[id] = Record{"member": id, "name": roleString(role, "Name"), "tactic": req.Value, "damage": damage, "at": now}
		round["Damage"] = roleInt(round, "Damage") + damage
	default:
		return sectRefusal("没有这项宗务")
	}
	sectMap(state, "Accounts")[id] = account
	seq := roleInt(state, "Revision") + 1
	state["Revision"], account["LastRevision"] = seq, seq
	if req.Action != "fight" && req.Action != "redeem" {
		state["SharedRevision"] = seq
	}
	return nil
}

func (s *Server) sectView(g trialGuild, state Record, id string, now int64) Record {
	stage := sectStage(g)
	tier := (stage - 1) / 3
	county, province, land, bound := sectRegion(g)
	names := []string{"青璧晶脉", "流霞玉髓", "曜金龙脉"}
	office := g.officeOf(id)
	workers, miners, guards := sectLaborAt(g, state, sectStage(g))
	ore, crystal := sectMineRateAt(g, state, sectStage(g))
	slowUntil := roleInt(state, "SlowUntil")
	beats := int64(4)
	if slowUntil <= now {
		slowUntil = 0
	} else {
		beats = 2
	}
	ore, crystal = ore*beats, crystal*beats
	next := min(9, stage+1)
	nextWorkers, nextMiners, nextGuards := sectLaborAt(g, state, next)
	buildOre, buildCrystal := sectBuildCosts(stage)
	nextOre, nextCrystal := sectMineRateAt(g, state, next)
	nextTier := (next - 1) / 3
	nextRegion := []*gameMap{county, province, land}[nextTier]
	round := state["Round"].(Record)
	echoes := round["Echoes"].(Record)
	accounts, _ := state["Accounts"].(Record)
	account, _ := accounts[id].(Record)
	formation := roleInt(state, "Formation")
	winOre, winFang, winMerit, loss, pity, protected := sectRewards(state, round)
	kind := "small"
	if sectLarge(roleInt(round, "Deadline")) {
		kind = "large"
	}
	treasures := make([]Record, 0, len(sectGear))
	for _, gear := range sectGear {
		tactic := map[string]string{"strike": "破阵", "ward": "固守", "scout": "探踪"}[gear.tactic]
		treasures = append(treasures, Record{"id": gear.id, "name": gear.name, "category": "战备", "cost": gear.cost, "owned": roleInt(account, gear.id),
			"description": "下次" + tactic + "自动消耗，留影伤害提高25%"})
	}
	roles := make([]Record, 0, len(g.members()))
	for _, who := range g.members() {
		r := s.roles.project(who, []string{"Name"})
		if r != nil {
			roles = append(roles, Record{"name": roleString(r, "Name"), "office": g.officeOf(who)})
		}
	}
	reports, _ := state["Reports"].([]any)
	if reports == nil {
		reports = []any{}
	}
	return Record{"guildId": g.id, "guildName": roleString(g.row, "Name"), "stage": stage, "tier": tier + 1, "tierName": []string{"山门", "仙府", "圣地"}[tier],
		"regionId": bound.id, "regionName": bound.name, "regionKind": []string{"郡", "洲", "大陆"}[tier], "regionPath": land.name + " / " + province.name + " / " + county.name,
		"office": office, "revision": roleInt(state, "Revision"), "canAssign": office == 15 || office == 2 || office == 4, "canForge": office == 15 || office == 5 || office == 12, "canPromote": office == 15 && stage < 9,
		"workers": workers, "miners": miners, "guards": guards, "plan": roleString(state, "Plan"), "ore": roleInt(state, "Ore"), "crystal": roleInt(state, "Crystal"),
		"fang": roleInt(state, "Fang"), "formation": formation, "formationOre": 20 * formation, "formationCrystal": 8 * formation, "promoteOre": 40 * stage, "promoteFang": 4 * stage,
		"merit": roleInt(account, "Merit"), "deadline": roleInt(round, "Deadline"), "serverTime": now, "damage": roleInt(round, "Damage"), "target": roleInt(round, "Target"),
		"joined": echoes[id] != nil, "members": len(g.members()), "foe": round["Foe"], "echoes": sectEchoes(round, 64), "reports": reports, "roles": roles,
		"treasures": treasures, "oreRate": ore, "crystalRate": crystal, "mineName": names[tier], "waveKind": kind, "winOre": winOre, "winFang": winFang,
		"nextStage": next, "nextWorkers": nextWorkers, "nextMiners": nextMiners, "nextGuards": nextGuards, "nextOreRate": nextOre * beats, "nextCrystalRate": nextCrystal * beats,
		"formationCap": stage + 2, "nextFormationCap": next + 2, "nextRegionName": nextRegion.name, "nextRegionKind": []string{"郡", "洲", "大陆"}[nextTier],
		"buildOre": buildOre, "buildCrystal": buildCrystal, "buildFormation": stage, "stipend": sectSalary(stage), "nextStipend": sectSalary(next),
		"winMerit": winMerit, "lossOre": loss, "lossMerit": pity, "protected": protected, "slowUntil": slowUntil}
}

// The shared store serializes governance, spending, mining, and the calendar.
// Stage a world copy only when the request actually changes durable state.
func (s *Server) sectCourt(id string, req *sectOrder, at time.Time) (Record, error) {
	// Commit any due guild calendar work before paying the independent role
	// wallet. Once that wallet has been credited, there is no second save.
	var salaryOrder *sectOrder
	if req != nil && req.Action == "stipend" {
		salaryOrder, req = req, nil
	}
	var result Record
	var declined error
	err := s.guilds.Transact(func(tx *StoreTx) error {
		g := trialGuildForRole(trialGuilds(tx.View(trialGuildWorld)), id)
		role := s.roles.project(id, []string{"Name", "Level", "CurrentPower", "ShinyStone", "SectStipendDay"})
		if role == nil || g.row == nil {
			return sectRefusal("请先加入本地宗门")
		}
		state, _ := g.row["Court"].(Record)
		var round Record
		if state != nil {
			round, _ = state["Round"].(Record)
		}
		advanceDue := state == nil || at.Unix()/mineBeat*mineBeat > roleInt(state, "LastMine") || at.Unix() >= roleInt(round, "Deadline")
		if req != nil || advanceDue {
			g = trialGuildByID(trialGuilds(tx.Get(trialGuildWorld)), g.id)
			state, _ = g.row["Court"].(Record)
			if state == nil {
				state = initSectCourt(g, at, s.legacySectCounty(g))
			}
			advanceSectCourt(g, state, at)
			// A member has returned. Skip exhausted offline time before resuming
			// production, so it cannot be reclaimed by a second read.
			state["LastMine"] = max(roleInt(state, "LastMine"), at.Unix()/mineBeat*mineBeat)
			state["LastVisit"] = max(roleInt(state, "LastVisit"), at.Unix())
			if req != nil {
				world := tx.Get(trialGuildWorld)
				round = state["Round"].(Record)
				due := roleInt(round, "Deadline")
				represented := sectRepresented(world, id, due)
				if req.Action == "fight" && represented != 0 && represented != g.id {
					declined = sectRefusal("本轮已代表原宗门出阵")
				} else {
					declined = sectAct(g, state, id, role, *req, at.Unix())
				}
				if declined != nil {
					// A stale order is refused while already-due calendar and
					// production results must still commit, once, at that instant.
					if advanceDue {
						return nil
					}
					return declined
				}
				if req.Action == "fight" {
					roll, _ := world["WaveAttendance"].(Record)
					if roleInt(roll, "Deadline") != due {
						roll = Record{"Deadline": due, "Members": Record{}}
						world["WaveAttendance"] = roll
					}
					sectMap(roll, "Members")[id] = g.id
				}
			}
		}
		result = s.sectView(g, state, id, at.Unix())
		day := sectSalaryDay(at)
		result["stipendDay"], result["stipendClaimed"], result["walletStones"] = day, roleString(role, "SectStipendDay") >= day, roleInt(role, "ShinyStone")
		represented := sectRepresented(tx.View(trialGuildWorld), id, roleInt(result, "deadline"))
		if represented != 0 && represented != g.id {
			result["joined"], result["joinedElsewhere"] = true, true
		}
		return nil
	})
	if err != nil {
		return nil, err
	}
	if salaryOrder != nil {
		if err := s.claimSectSalary(id, *salaryOrder, result); err != nil {
			return nil, err
		}
	}
	return result, declined
}

func sectRepresented(world Record, id string, deadline int64) int64 {
	roll, _ := world["WaveAttendance"].(Record)
	if roleInt(roll, "Deadline") != deadline {
		return 0
	}
	members, _ := roll["Members"].(Record)
	return roleInt(members, id)
}

func (s *Server) handleSectCourt(peer *Peer, msg Message) {
	role := peer.currentRoleID()
	active, ok := s.online.Load(role)
	reply := func(code int, body Record) {
		data, _ := json.Marshal(body)
		wire := offerNumber(nil, 1, uint64(code))
		if code == 2000 {
			wire = offerNumber(wire, 2, 1)
		}
		_ = peer.sendPayload(msg.Cmd, msg.Msid, offerString(wire, 3, string(data)))
	}
	if !s.previewWrites || s.guilds == nil || role == "" || !ok || active != peer {
		reply(4008, Record{"error": "宗门事务未开放或角色已离线"})
		return
	}
	var order *sectOrder
	if msg.Cmd == guildCourtAct || msg.Cmd == guildCharterAct {
		order = new(sectOrder)
		if len(msg.Data) > 1024 || json.Unmarshal([]byte(offerToken(msg.Data)), order) != nil {
			reply(4008, Record{"error": "宗务指令无效"})
			return
		}
	}
	var result Record
	var err error
	if msg.Cmd == guildCharterRead || msg.Cmd == guildCharterAct {
		peer.sectPermitOwner = role
		result, err = s.sectCharter(role, order)
	} else {
		result, err = s.sectCourt(role, order, time.Now())
	}
	if err != nil {
		var reason sectRefusal
		if errors.As(err, &reason) {
			reply(4008, Record{"error": string(reason)})
		} else {
			log.Printf("宗门事务保存失败: %v", err)
			reply(5000, Record{"error": "宗务未能保存，请重试"})
		}
		return
	}
	reply(2000, result)
}
