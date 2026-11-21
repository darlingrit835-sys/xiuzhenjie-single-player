package main

import (
	_ "embed"
	"encoding/json"
	"fmt"
	"strconv"
	"strings"
)

const voidCatalogVersion = 1

// v1 remains available for reproducing signed manuscripts after future revisions.
// Hall, brief, route and adjustment positions are part of the manuscript format.
//
//go:embed void_realm_v1.json
var voidCatalogV1 []byte

type voidOption struct {
	Name   string
	Values [2]int
}

type voidBrief struct {
	Name    string
	Targets [2]int
}

type voidHall struct {
	ID, Job, Room string
	Type          int
	Axes          [2]string
	Routes        [3]voidOption
	Adjusts       [3]voidOption
	Briefs        [3]voidBrief
}

var voidHalls = func() []voidHall {
	var halls []voidHall
	if err := json.Unmarshal(voidCatalogV1, &halls); err != nil {
		panic(err)
	}
	return append(halls, voidHall{ID: "qiao", Type: 7, Job: "奇巧师", Room: "奇巧室"}, voidHall{ID: "shou", Type: 9, Job: "御兽师", Room: "御兽室"})
}()

func voidFindHall(id string) *voidHall {
	for i := range voidHalls {
		if voidHalls[i].ID == id {
			return &voidHalls[i]
		}
	}
	return nil
}

func voidRank(role Record, prof int) int {
	// A workshop can still award training after its residence room is rebuilt.
	// Explicit retirement excludes those awards from hall access. An old save
	// without a retirement record retains its original workshop qualification.
	if roleBool(farmMap(role, "ResidenceRetiredCareers"), strconv.Itoa(prof)) {
		rank, _, _ := artisanTraining(role, prof, "", "")
		return rank
	}
	switch prof {
	case 1:
		rank, _, _ := elixirTraining(role)
		return rank
	case 2:
		rank, _, _ := artisanTraining(role, 2, "ForgeRank", "ForgeExperience")
		return rank
	case 3:
		rank, _, _ := sigilTraining(role)
		return rank
	case 8:
		rank, _, _ := artisanTraining(role, 8, "GardenRank", "GardenExperience")
		return rank
	case 4, 5, 6, 7, 9:
		return artisanResearchRank(role, prof)
	}
	return 0
}

func voidRealmStage(role Record) int {
	return int(max(0, min(9, (roleInt(role, "Level")+9)/10)))
}

func voidAccess(role Record, hall *voidHall, grade int) error {
	if hall == nil {
		return forgeFailure("此馆尚未开设")
	}
	rank := voidRank(role, hall.Type)
	if rank == 0 {
		return forgeFailure("入馆须为" + hall.Job + "，可先到洞府" + hall.Room + "修习")
	}
	cap := min(rank, voidRealmStage(role))
	if grade < 1 || grade > cap {
		return forgeFailure(fmt.Sprintf("按职业品阶与目前境界，作品可选一至%d阶", cap))
	}
	return nil
}

func voidLobby(owner string, role Record) Record {
	halls, slots := make([]Record, 0, len(voidHalls)), []Record{}
	for i := 1; i <= 4; i++ {
		k := "Live" + strconv.Itoa(i)
		if typ := roleInt(role, k+"Type"); typ > 0 {
			slots = append(slots, Record{"type": typ, "level": roleInt(role, k+"Level")})
		}
	}
	for _, h := range voidHalls {
		rank := voidRank(role, h.Type)
		halls = append(halls, Record{"id": h.ID, "rank": rank, "cap": min(rank, voidRealmStage(role))})
	}
	return Record{"roleId": owner, "name": roleString(role, "Name"), "roleLevel": roleInt(role, "Level"), "slots": slots, "halls": halls}
}

func voidDistance(n int) int {
	if n < 0 {
		return -n
	}
	return n
}

func voidAxes(h *voidHall, route, adjust int) (values [2]int, overrun int) {
	for i := range values {
		raw := h.Routes[route].Values[i] + h.Adjusts[adjust].Values[i]
		values[i] = min(2, max(0, raw))
		overrun += voidDistance(raw - values[i])
	}
	return
}

// Preserve the current route whenever a different finishing step can meet the brief.
func voidRevision(h *voidHall, brief, route, adjust int) string {
	bestR, bestA, weight := 0, 0, 4
	for r := range h.Routes {
		for a := range h.Adjusts {
			v, _ := voidAxes(h, r, a)
			if v != h.Briefs[brief].Targets {
				continue
			}
			w := 0
			if r != route {
				w += 2
			}
			if a != adjust {
				w++
			}
			if w < weight {
				bestR, bestA, weight = r, a, w
			}
		}
	}
	return "试用「" + h.Routes[bestR].Name + "」，收尾选「" + h.Adjusts[bestA].Name + "」。"
}

func voidSimulate(role Record, q voidRequest) (Record, error) {
	h := voidFindHall(q.HallID)
	if err := voidAccess(role, h, q.Grade); err != nil {
		return nil, err
	}
	if h.Type == 7 || h.Type == 9 {
		return nil, forgeFailure("本馆采用新版创作工序")
	}
	if q.Brief < 0 || q.Brief >= len(h.Briefs) || q.Route < 0 || q.Route >= len(h.Routes) || q.Adjust < 0 || q.Adjust >= len(h.Adjusts) {
		return nil, forgeFailure("请选择有效的论题、法门和收尾")
	}
	values, overrun := voidAxes(h, q.Route, q.Adjust)
	axes, reasons, distance := make([]Record, 0, 2), make([]string, 0, 2), 0
	for i, name := range h.Axes {
		target, value := h.Briefs[q.Brief].Targets[i], values[i]
		distance += voidDistance(value - target)
		state := "合度"
		if value > target {
			state = "偏高"
		} else if value < target {
			state = "偏低"
		}
		axes = append(axes, Record{"name": name, "target": target, "value": value})
		reasons = append(reasons, fmt.Sprintf("%s%s（%d/%d）", name, state, value, target))
	}
	nodes := 0
	switch h.ID {
	case "dao":
		nodes = 7 + q.Grade
	case "shu", "zhi":
		nodes = 4 + q.Grade
	}
	success := distance == 0
	score, cost := 45-10*distance, nodes
	title, next := "草案待修", ""
	scoreText := "草案尚未合题，每偏离一格，试演分减少 10。"
	if success {
		economy := 5 - 2*overrun
		if h.Adjusts[q.Adjust].Values != [2]int{} {
			economy--
		}
		economy = max(0, economy)
		score, cost = 50+q.Grade*5+economy, nodes*q.Grade
		scoreText = fmt.Sprintf("合题 50 ＋ 作品等阶 %d ＋ 工艺余裕 %d。余裕满 5，转工减 1，超出刻度每格减 2。", q.Grade*5, economy)
		title, next = "推演合题", "可为这份作品题名入册。"
	} else {
		next = voidRevision(h, q.Brief, q.Route, q.Adjust)
	}
	costText := "推演使用虚拟试材，本次实扣 0"
	if nodes > 0 {
		costText = fmt.Sprintf("悟性预估 %d 点（本次实扣 0）", cost)
	}
	if h.Type == 4 || h.Type == 5 || h.Type == 6 {
		fee := manuscriptTuition(role, h, q.Grade, q.Brief)
		descr := map[int]string{4: "每品减少1%术法施法时长", 5: "每品增强1%丹符护盾；二、五、八品各省1枚守田阵灵石", 6: "每品增加1%纳气修为速度"}
		costText += fmt.Sprintf("。本题署名研习 %d 灵石；一至八品各完成三题可晋品，%s", fee, descr[h.Type])
	}
	return Record{"eligible": true, "success": success, "score": score, "scoreText": scoreText, "title": title, "reason": strings.Join(reasons, "，") + "。",
		"next": next, "axes": axes, "nodes": nodes, "cost": cost, "costText": costText}, nil
}
