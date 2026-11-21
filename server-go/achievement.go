package main

import (
	"errors"
	"fmt"
	"log"
	"math"
	"strconv"

	"google.golang.org/protobuf/encoding/protowire"
	"google.golang.org/protobuf/reflect/protoreflect"
)

const (
	achievementNoticeCmd = 1016003
	achievementListCmd   = 1016004
	achievementClaimCmd  = 1016006

	achievementReady   = 1
	achievementClaimed = 2
	achievementClaims  = "TianShuBeiClaims"
	achievementStones  = "TianShuBeiBattleStones"
	achievementFriends = "TianShuBeiFriendPeak"
)

type achievementDefinition struct {
	id, kind, target, reward int64
}

// IDs are save keys. Keep them stable when adding or rearranging milestones.
var achievements = [...]achievementDefinition{
	{701, 7, 11, 100}, {702, 7, 21, 200}, {703, 7, 31, 300}, {704, 7, 41, 400},
	{705, 7, 51, 600}, {706, 7, 61, 800}, {707, 7, 71, 1100}, {708, 7, 81, 1500}, {709, 7, 90, 2200},
	{501, 5, 1, 100}, {502, 5, 50, 200}, {503, 5, 300, 300},
	{801, 8, 1, 100}, {802, 8, 10, 200}, {803, 8, 50, 300},
	{901, 9, 1, 100}, {902, 9, 10, 200}, {903, 9, 30, 300},
	{101, 1, 100, 100}, {102, 1, 1000, 200}, {103, 1, 10000, 300},
	{1101, 11, 1, 100}, {1102, 11, 2, 200}, {1103, 11, 4, 300},
	{1001, 10, 1, 100}, {1002, 10, 3, 200}, {1003, 10, 7, 300},
	{201, 2, 1, 100}, {202, 2, 3, 200},
}

func preserveAchievementFriendPeak(r Record) {
	r[achievementFriends] = max(roleInt(r, achievementFriends), int64(len(friendMap(r, friendLinks))))
}

func achievementProgress(r Record) [12]int64 {
	return [12]int64{
		1:  roleInt(r, achievementStones),
		2:  max(roleInt(r, achievementFriends), int64(len(friendMap(r, friendLinks)))),
		5:  max(roleInt(r, "QuestTotalKills"), roleInt(r, "QuestKills")),
		7:  roleInt(r, "Level"),
		8:  max(roleInt(r, "QuestTotalSweeps"), roleInt(r, "QuestSweeps")),
		9:  max(roleInt(r, "QuestTotalBossKills"), roleInt(r, "QuestBossKills")),
		10: roleInt(r, "SignInDay"),
		11: gameHomeRank(r),
	}
}

func (a achievementDefinition) status(progress [12]int64, claims Record) int {
	if roleBool(claims, a.claimKey()) {
		return achievementClaimed
	}
	if progress[a.kind] >= a.target {
		return achievementReady
	}
	return 0
}

func (a achievementDefinition) claimKey() string { return strconv.FormatInt(a.id, 10) }

func achievementSummary(r Record) (earned int64, ready bool) {
	progress := achievementProgress(r)
	claims, _ := r[achievementClaims].(Record)
	for _, a := range achievements {
		status := a.status(progress, claims)
		if status != 0 {
			earned++
		}
		ready = ready || status == achievementReady
	}
	return earned, ready
}

func (s *Server) listAchievements(peer *Peer, msg Message) {
	out := s.gameResponse("OpenTianShuBeiResp_1016004", false, 4007)
	if r := s.roles.Get(peer.currentRoleID()); r != nil {
		statusPB(out, true, 2000)
		progress := achievementProgress(r)
		claims, _ := r[achievementClaims].(Record)
		for _, a := range achievements {
			status := a.status(progress, claims)
			current := max(0, min(progress[a.kind], a.target))
			if status == achievementClaimed {
				current = a.target
			}
			row := addMessage(out, "TianShuBeiObj")
			putPB(row, Record{"Id": a.id, "Type": a.kind, "Status": status, "CurrentVal": current, "MaxVal": a.target})
		}
	}
	s.sendGame(peer, msg, out)
}

func appendAchievementReceipt(out protoreflect.Message, balance int64, hasUnclaimed bool) {
	// Older clients ignore the balance and badge fields appended to the claim response.
	data := protowire.AppendVarint(protowire.AppendTag(nil, 6, protowire.VarintType), uint64(balance))
	out.SetUnknown(protowire.AppendVarint(protowire.AppendTag(data, 7, protowire.VarintType), protowire.EncodeBool(hasUnclaimed)))
}

func (s *Server) claimAchievement(peer *Peer, msg Message) {
	out := s.gameResponse("TianShuBeiReceiveAwardResp_1016006", false, 4008)
	req, err := s.decode("TianShuBeiReceiveAwardReq_1016006", msg.Data)
	if err == nil {
		for _, a := range achievements {
			if a.id == getInt(req, "Id") && a.kind == getInt(req, "Type") {
				balance, hasUnclaimed, awarded, err := s.awardAchievement(peer.currentRoleID(), a)
				if err == nil {
					statusPB(out, true, 2000)
					putPB(out, Record{"Id": a.id, "MaxVal": a.target, "Status": achievementClaimed})
					appendAchievementReceipt(out, balance, hasUnclaimed)
				} else {
					var code previewReject
					if errors.As(err, &code) {
						statusPB(out, false, int(code))
					} else {
						log.Printf("天书碑奖励保存失败: %v", err)
						statusPB(out, false, 5000)
					}
				}
				s.sendGame(peer, msg, out)
				if awarded {
					s.gameNotice(peer, fmt.Sprintf("天书碑奖励：灵石×%d", a.reward))
				}
				return
			}
		}
	}
	s.sendGame(peer, msg, out)
}

func (s *Server) awardAchievement(roleID string, a achievementDefinition) (balance int64, hasUnclaimed, awarded bool, err error) {
	err = s.roles.Transact(func(tx *StoreTx) error {
		r := tx.View(roleID)
		if r == nil {
			return previewReject(4007)
		}
		balance = roleInt(r, "ShinyStone")
		claims, _ := r[achievementClaims].(Record)
		switch a.status(achievementProgress(r), claims) {
		case achievementClaimed:
			_, hasUnclaimed = achievementSummary(r)
			return nil
		case 0:
			return previewReject(4008)
		}
		if balance < 0 || balance > math.MaxInt64-a.reward {
			return previewReject(5000)
		}
		r = tx.Get(roleID)
		claims, _ = r[achievementClaims].(Record)
		if claims == nil {
			claims = Record{}
			r[achievementClaims] = claims
		}
		claims[a.claimKey()] = true
		_, hasUnclaimed = achievementSummary(r)
		balance += a.reward
		r["ShinyStone"] = balance
		awarded = true
		event := newHistoryEvent("achievement.reward_claimed", r, nil, "achievement", a.claimKey(),
			Record{"type": a.kind, "target": a.target, "shinyStone": a.reward})
		event.Actor.RoleID = roleID
		return tx.RecordHistory(event)
	})
	return balance, hasUnclaimed, awarded && err == nil, err
}
