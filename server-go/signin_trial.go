package main

import (
	"fmt"
	"strconv"
	"time"

	"google.golang.org/protobuf/encoding/protowire"
	"google.golang.org/protobuf/reflect/protoreflect"
)

var trialSignInStones = [...]int64{50, 80, 100, 120, 150, 180, 300}

func previewSignInAwards(_ Record, _, out protoreflect.Message) {
	for day, amount := range trialSignInStones {
		entry := addMessage(out, "SignInAward")
		set(entry, "SignInDay", day+1)
		putPB(addMessage(entry, "Award"), Record{"GoodsName": "灵石（试用）", "Num": amount, "Type": 100, "Rank": 4})
	}
}

// Older saves have a day count and a boolean but no calendar date. LastOnline
// is the latest day on which that save could have claimed.
func trialSignInState(role Record, today string) (days int64, last string) {
	days = min(max(roleInt(role, "SignInDay"), 0), int64(len(trialSignInStones)))
	last = roleString(role, "TrialSignInDate")
	if last == "" && roleBool(role, "IsGetSignIn") {
		last = today
		if online := roleInt(role, "LastOnline"); online > 0 {
			last = mentorGameDay(time.UnixMilli(online))
		}
		days = max(days, 1)
	}
	return days, last
}

func syncTrialSignInLogin(role Record, now time.Time) {
	today := mentorGameDay(now)
	days, last := trialSignInState(role, today)
	role["SignInDay"], role["IsGetSignIn"] = days, last >= today
	if last != "" {
		role["TrialSignInDate"] = last
	}
}

func (s *Server) trialSignInCatalog(peer *Peer, req, out protoreflect.Message) (func(), error) {
	role := s.roles.project(peer.currentRoleID(), []string{"SignInDay", "IsGetSignIn", "TrialSignInDate", "LastOnline"})
	if role == nil {
		return nil, previewReject(4007)
	}
	today := mentorGameDay(time.Now())
	days, last := trialSignInState(role, today)
	previewSignInAwards(nil, req, out)
	// Optional fields 4 and 5 refresh existing clients with the sign-in patch
	// when they reopen the window after midnight without logging out.
	data := protowire.AppendVarint(protowire.AppendTag(nil, 4, protowire.VarintType), uint64(days))
	var claimed uint64
	if last >= today {
		claimed = 1
	}
	out.SetUnknown(protowire.AppendVarint(protowire.AppendTag(data, 5, protowire.VarintType), claimed))
	return nil, nil
}

func (s *Server) trialSignInClaim(peer *Peer, _, out protoreflect.Message) (func(), error) {
	owner := peer.currentRoleID()
	if err := s.issueTrialSignInLetter(owner, time.Now()); err != nil {
		return nil, err
	}
	statusPB(out, true, 4368)
	return func() { s.pushGame(owner, 1003007, s.pb("LetterNotifyResp_1003007")) }, nil
}

func (s *Server) issueTrialSignInLetter(owner string, now time.Time) error {
	today := mentorGameDay(now)
	return s.roles.Transact(func(tx *StoreTx) error {
		role := tx.View(owner)
		if role == nil {
			return previewReject(4007)
		}
		days, last := trialSignInState(role, today)
		if last >= today || days == int64(len(trialSignInStones)) {
			return previewReject(4368)
		}
		amount := trialSignInStones[days]
		if roleInt(role, "VipLevel") >= 6 {
			amount *= 2 // The original QianDaoItem shows doubled rewards from VIP 6.
		}
		role = tx.Get(owner)
		role["TrialSignInDate"], role["SignInDay"], role["IsGetSignIn"] = today, days+1, true
		letters := trialLetters(role)
		if letters == nil {
			letters = Record{}
			role[mailBoxField] = letters
		}
		id := "trial-signin-" + strconv.FormatInt(days+1, 10)
		if letters[id] != nil {
			return previewReject(4368)
		}
		letters[id] = Record{
			"Title": fmt.Sprintf("第%d天签到奖励", days+1), "Content": "七日签到奖励已发放，请领取附件。", "SentAt": now.UnixMilli(), "Status": false,
			"Awards": []any{Record{"Id": "stone", "GoodsName": "灵石（试用）", "GoodsType": 100, "GoodsCount": amount, "Rank": 4, "Status": false}},
		}
		event := newHistoryEvent("signin.reward_issued", role, nil, "letter", id, Record{"day": days + 1, "calendarDate": today, "shinyStone": amount})
		event.Actor.RoleID = owner
		return tx.RecordHistory(event)
	})
}
