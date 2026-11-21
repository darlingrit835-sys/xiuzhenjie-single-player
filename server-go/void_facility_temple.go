package main

import (
	"encoding/json"
	"errors"
	"fmt"
	"sort"
	"strings"
)

const voidFacilityCommand = 4901076

type voidFacilityRequest struct {
	Action string
	Hall   string
	Brief  int
	Answer int
	Stage  int
	Mode   string
	Page   int
}

func (s *Server) handleVoidFacility(peer *Peer, msg Message) {
	id := s.worldChatRole(peer)
	var req voidFacilityRequest
	if id == "" || reservedStoreID(id) {
		sendAuctionJSON(peer, msg, 4007, Record{"error": "请先登录角色"})
		return
	}
	if len(msg.Data) > 2048 || json.Unmarshal([]byte(offerToken(msg.Data)), &req) != nil {
		sendAuctionJSON(peer, msg, 4008, Record{"error": "呈送的文书无法识别"})
		return
	}
	if req.Action == "seal" && s.preview != nil && !s.previewWrites {
		sendAuctionJSON(peer, msg, 4008, Record{"error": "此服只开放阅读"})
		return
	}
	reply, err := s.voidFacilityAction(id, req)
	if err != nil {
		var reject forgeFailure
		if errors.As(err, &reject) {
			sendAuctionJSON(peer, msg, 4008, Record{"error": string(reject)})
			return
		}
		sendAuctionJSON(peer, msg, 5000, Record{"error": "未能读写殿籍，请重试"})
		return
	}
	sendAuctionJSON(peer, msg, 2000, reply)
}
func (s *Server) voidFacilityAction(owner string, q voidFacilityRequest) (Record, error) {
	var reply Record
	err := s.roles.Transact(func(tx *StoreTx) error {
		role := tx.View(owner)
		if role == nil || voidRealmStage(role) < 3 {
			return forgeFailure("需结丹一阶方可进入虚神界")
		}
		switch q.Action {
		case "tower":
			stage := q.Stage
			if stage == 0 {
				stage = voidRealmStage(role)
			}
			if stage < 3 || stage > 9 || q.Mode != "" && q.Mode != "height" && q.Mode != "age" {
				return forgeFailure("请选择结丹至大乘中的一个境界")
			}
			rows, pos := towerStandings(tx, owner, stage, q.Mode == "age")
			reply = Record{"stage": stage, "best": towerBest(role, voidRealmStage(role)), "next": towerNextFloor(role), "mine": pos, "ranks": rows}
		case "temple":
			halls := []Record{}
			for _, h := range voidHalls {
				hits := 0
				for i := 0; i < 3; i++ {
					if roleBool(farmMap(role, "VoidTempleSeals"), templeSealKey(h.ID, i)) {
						hits++
					}
				}
				halls = append(halls, Record{"id": h.ID, "rank": voidRank(role, h.Type), "seals": hits})
			}
			reply = Record{"halls": halls}
		case "folio":
			if q.Page < 0 {
				return forgeFailure("卷页无效")
			}
			rows := []Record{}
			for _, key := range tx.store.order {
				if !strings.HasPrefix(key, voidStorePrefix) {
					continue
				}
				w := tx.View(key)
				if roleString(w, "kind") == "work" && roleString(w, "authorId") == owner {
					rows = append(rows, Record{"id": roleString(w, "id"), "hallId": roleString(w, "hallId"), "name": roleString(w, "name"), "grade": roleInt(w, "grade"), "score": roleInt(w, "score"), "time": roleInt(w, "time")})
				}
			}
			sort.Slice(rows, func(i, j int) bool {
				a, b := roleInt(rows[i], "time"), roleInt(rows[j], "time")
				if a != b {
					return a > b
				}
				return roleString(rows[i], "id") < roleString(rows[j], "id")
			})
			start := len(rows)
			if q.Page <= len(rows)/voidPageSize {
				start = q.Page * voidPageSize
			}
			end := min(len(rows), start+voidPageSize)
			reply = Record{"works": rows[start:end], "more": end < len(rows), "count": len(rows)}
		case "lecture", "seal":
			hall := voidCraftHallFor(q.Hall)
			if hall == nil || q.Brief < 0 || q.Brief >= len(hall.Briefs) {
				return forgeFailure("这篇经义未入典藏")
			}
			brief := hall.Briefs[q.Brief]
			key := templeSealKey(q.Hall, q.Brief)
			correct := []int{brief.Element, brief.Polarity, brief.Qi}[q.Brief]
			var choices []string
			switch q.Brief {
			case 0:
				choices = voidCraftV2.Elements[:]
			case 1:
				choices = voidCraftV2.Polarities[:]
			case 2:
				choices = voidCraftV2.Qi[:]
			}
			done := roleBool(farmMap(role, "VoidTempleSeals"), key)
			if q.Action == "seal" && q.Answer == correct && !done {
				auctionMapEdit(tx.Get(owner), "VoidTempleSeals")[key] = true
				done = true
			}
			reply = Record{"name": brief.Name, "hint": brief.Hint, "teaching": fmt.Sprintf("%s。此卷取%s行，以%s立性，纳%s为主气。", hall.Mechanic, voidCraftV2.Elements[brief.Element-1], voidCraftV2.Polarities[brief.Polarity-1], voidCraftV2.Qi[brief.Qi-1]), "question": []string{"依题归经，此卷以何行为本？", "校对阴阳，此卷取何种气性？", "辨识主气，此卷所纳为何气？"}[q.Brief], "choices": choices, "passed": done, "correct": q.Action == "seal" && q.Answer == correct}
		default:
			return forgeFailure("这项殿务尚未开放")
		}
		return nil
	})
	return reply, err
}
func templeSealKey(h string, i int) string { return fmt.Sprintf("%s/%d", h, i) }
