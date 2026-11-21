package main

import (
	"fmt"
	"google.golang.org/protobuf/proto"
	"google.golang.org/protobuf/reflect/protoreflect"
	"google.golang.org/protobuf/types/known/wrapperspb"
	"math"
	"sort"
	"strconv"
	"time"
)

func towerMapID(stage int) string { return fmt.Sprintf("voidTower%d", stage) }
func towerStageForMap(id string) int {
	if len(id) == 10 && id[:9] == "voidTower" && id[9] >= '3' && id[9] <= '9' {
		return int(id[9] - '0')
	}
	return 0
}

func towerBest(role Record, stage int) int {
	mark := farmMap(farmMap(role, "VoidTowerRecords"), strconv.Itoa(stage))
	best := int(roleInt(mark, "floor"))
	// Old saves have one floor, and no archived records for previous realms.
	legacyStage := int(roleInt(role, "VoidTowerLegacyStage"))
	if legacyStage == 0 {
		legacyStage = voidRealmStage(role)
	}
	if stage == legacyStage {
		best = max(best, int(roleInt(role, "RushTowerNum")))
	}
	return best
}

func towerNextFloor(role Record) int {
	stage := voidRealmStage(role)
	return max((stage-3)*40, towerBest(role, stage)) + 1
}

func towerRecordClear(role Record, stage, floor int) {
	if roleInt(role, "RushTowerNum") > 0 && roleInt(role, "VoidTowerLegacyStage") == 0 {
		role["VoidTowerLegacyStage"] = stage
	}
	records := auctionMapEdit(role, "VoidTowerRecords")
	mark := auctionMapEdit(records, strconv.Itoa(stage))
	if floor > int(roleInt(mark, "floor")) {
		mark["floor"], mark["level"] = floor, max(1, roleInt(role, "Level"))
	}
	// 天骄榜按每四十层关口的过关年龄留名，按真实通关记，起始层不算过关。
	if floor%40 == 0 && floor <= 280 {
		checkpoint := auctionMapEdit(auctionMapEdit(role, "VoidTowerMilestones"), strconv.Itoa(floor/40+2))
		age := max(16, roleInt(role, "CurrentAge"))
		if roleInt(checkpoint, "age") == 0 || age < roleInt(checkpoint, "age") {
			checkpoint["age"], checkpoint["floor"] = age, floor
		}
	}
}

func (s *Server) towerMonsters(floor int) []battleMonster {
	level := min(90, 21+(floor-1)/4)
	monsters := s.createMonsters(Record{"Level": level}, "")
	// 每层保留本服战斗配置的前三名守关者，第三名占用原有首领席位。
	if len(monsters) > 3 {
		monsters[2] = monsters[len(monsters)-1]
		monsters = monsters[:3]
	}
	for i := range monsters {
		m := &monsters[i]
		m.index = 1001 + i
		m.name = fmt.Sprintf("%d层守影·%s", floor, m.name)
		pressure := 1 + float64((floor-1)%4)*.04 + float64(max(0, floor-280))*.025
		m.hp = int64(math.Min(math.MaxInt32, float64(m.hp)*pressure))
		m.armor = int64(math.Min(math.MaxInt32, float64(m.armor)*pressure))
	}
	return monsters
}

func (s *Server) towerNextBattleFloor(peer *Peer, msg Message) {
	resp := s.gameResponse("EnterTowerNextLevelResp_8001009", false, 4503)
	requested := &wrapperspb.Int32Value{}
	if err := proto.Unmarshal(msg.Data, requested); err == nil {
		if d := s.dungeonForRole(peer.dungeonID, peer.roleID, false); d != nil {
			d.mu.Lock()
			active := !d.closed && d.entered && d.towerFloor > 0 && d.activePeer == peer && len(d.monsters) > 0
			floor := int(requested.Value)
			advance := active && len(d.dead) == len(d.monsters) && (floor == 0 || floor == d.towerFloor+1)
			retry := active && floor != 0 && floor == d.towerFloor && len(d.dead) == 0
			if advance {
				next := s.towerMonsters(d.towerFloor + 1)
				for i := range next {
					next[i].index = d.monsters[len(d.monsters)-1].index + 1 + i
					next[i].x, next[i].y = d.monsters[i%len(d.monsters)].x, d.monsters[i%len(d.monsters)].y
				}
				d.towerFloor++
				d.monsters = next
				d.dead = make(map[int]struct{})
			}
			if advance || retry {
				statusPB(resp, true, 4506)
				set(resp, "OverTime", max(int64(1), time.Until(d.expires).Milliseconds()))
				for _, monster := range d.monsters {
					d.putMonster(addMessage(resp, "Objects"), monster)
				}
			}
			d.mu.Unlock()
		}
	}
	_ = peer.send(msg.Cmd, msg.Msid, resp)
}

func towerStandings(tx *StoreTx, owner string, stage int, age bool) ([]Record, int) {
	key := strconv.Itoa(stage)
	rows := []Record{}
	for _, id := range tx.store.order {
		if reservedStoreID(id) {
			continue
		}
		role := tx.View(id)
		if role == nil || roleString(role, "Name") == "" {
			continue
		}
		mark := farmMap(farmMap(role, "VoidTowerRecords"), key)
		floor, years := towerBest(role, stage), int64(0)
		if age {
			mark = farmMap(farmMap(role, "VoidTowerMilestones"), key)
			floor = int(roleInt(mark, "floor"))
			years = roleInt(mark, "age")
		}
		if floor <= 0 || age && years <= 0 {
			continue
		}
		rankLevel := roleInt(mark, "level")
		if rankLevel == 0 {
			rankLevel = roleInt(role, "Level")
		}
		rows = append(rows, Record{"id": id, "Name": roleString(role, "Name"), "Level": max(1, rankLevel), "Age": years, "Faction": roleString(role, "FactionName"), "TierNum": floor})
	}
	sort.Slice(rows, func(i, j int) bool {
		if age && roleInt(rows[i], "Age") != roleInt(rows[j], "Age") {
			return roleInt(rows[i], "Age") < roleInt(rows[j], "Age")
		}
		if roleInt(rows[i], "TierNum") != roleInt(rows[j], "TierNum") {
			return roleInt(rows[i], "TierNum") > roleInt(rows[j], "TierNum")
		}
		return roleString(rows[i], "id") < roleString(rows[j], "id")
	})
	place := 0
	for i, row := range rows {
		if roleString(row, "id") == owner {
			place = i + 1
			break
		}
	}
	rows = rows[:min(10, len(rows))]
	for _, row := range rows {
		delete(row, "id")
	}
	return rows, place
}

func (s *Server) handleTowerRecord(peer *Peer, msg Message) {
	owner := s.worldChatRole(peer)
	var out protoreflect.Message
	if msg.Cmd == 1008001 {
		out = s.gameResponse("EnterUnrealTowerResp_1008001", false, 4008)
		if role := s.roles.Get(owner); role != nil && voidRealmStage(role) >= 3 {
			statusPB(out, true, 4191)
			set(out, "RushTowerNum", towerNextFloor(role)-1)
		}
	} else {
		spec, code := "EnterRushTowerTierNumRanked", 4190
		age := msg.Cmd == 1008004
		if age {
			spec, code = "EnterRushTowerAgeRanked", 4192
		}
		cmd := strconv.Itoa(msg.Cmd)
		out = s.gameResponse(spec+"Resp_"+cmd, false, 4008)
		req, err := s.decode(spec+"Req_"+cmd, msg.Data)
		if err == nil {
			stage := int(getInt(req, "Stage"))
			if stage >= 3 && stage <= 9 {
				_ = s.roles.Transact(func(tx *StoreTx) error {
					// These rank messages are also used by the main-city rank board.
					if tx.View(owner) == nil {
						return nil
					}
					rows, mine := towerStandings(tx, owner, stage, age)
					statusPB(out, true, code)
					set(out, "MyRanked", mine)
					for _, row := range rows {
						item := addMessage(out, "Object")
						copyRolePB(item, row, "Name", "Faction", "Level", "Age", "TierNum")
					}
					return nil
				})
			}
		}
	}
	_ = peer.send(msg.Cmd, msg.Msid, out.Interface())
}
