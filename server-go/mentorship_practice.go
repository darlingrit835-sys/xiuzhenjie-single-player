package main

import (
	"math"
	"time"

	"google.golang.org/protobuf/reflect/protoreflect"
)

const (
	mentorMeridians = 1
	mentorPower     = 2
)

func mentorshipPracticeRoutes() []previewRoute {
	const module = "社交与宗门"
	return []previewRoute{
		{cmd: 1014016, module: module, request: "TongMaiReq_1014016", response: "TongMaiResp_1014016", summary: "师尊为亲传弟子通脉，保存待领取经脉点", blocked: true, live: (*Server).mentorOpenMeridians},
		{cmd: 1014017, module: module, request: "ChuanGongReq_1014017", response: "ChuanGongResp_1014017", summary: "师尊向弟子传功，保存待领取修为", blocked: true, live: (*Server).mentorTeachPower},
		{cmd: 1014025, module: module, response: "JieShouTMCGResp_1014025", summary: "弟子领取已保存的通脉或传功收益", blocked: true, live: (*Server).mentorClaimPractice},
	}
}

func mentorPracticeUnavailable(student Record, day string) bool {
	return roleString(student, "MentorPracticeDay") >= day || mentorSet(student, "MentorPendingPractice") != nil
}

func mentorPracticeAmount(student, reward Record) int64 {
	raw := roleInt(reward, "Amount")
	if roleInt(reward, "Type") != mentorPower || raw <= 0 || raw > math.MaxInt32 {
		return raw
	}
	return max(1, int64(math.Round(float64(raw)/lateRealmTimeFactor(roleInt(student, "Level")))))
}

func fillMentorPending(student Record, out protoreflect.Message) {
	reward := mentorSet(student, "MentorPendingPractice")
	set(out, "IsGetTmcg", reward == nil)
	if reward != nil {
		set(out, "TmcgType", roleInt(reward, "Type"))
		set(out, "TmcgNum", mentorPracticeAmount(student, reward))
	}
}

// The caller is acting through a live game peer. Only the counterpart's
// presence needs checking. Realm, county and forest map IDs do not count as cities.
func (s *Server) mentorAffinityGain(otherID string, teacher, student Record) int64 {
	if _, online := s.online.Load(otherID); !online {
		return 5
	}
	if cityID := roleString(student, "MapId"); cityID == roleString(teacher, "MapId") {
		if city := gameMapFind(cityID); city != nil && city.kind == 51 {
			return 20
		}
	}
	return 10
}

func (s *Server) mentorOpenMeridians(peer *Peer, req, out protoreflect.Message) (func(), error) {
	return s.mentorSendPractice(peer, req, out, mentorMeridians)
}

func (s *Server) mentorTeachPower(peer *Peer, req, out protoreflect.Message) (func(), error) {
	return s.mentorSendPractice(peer, req, out, mentorPower)
}

func (s *Server) mentorSendPractice(peer *Peer, req, out protoreflect.Message, kind int) (func(), error) {
	teacherID, studentID := peer.currentRoleID(), getString(req, "RoleId")
	day := mentorGameDay(time.Now())
	var amount, vigor, intimacy int64
	err := s.roles.Transact(func(tx *StoreTx) error {
		teacher, student := tx.View(teacherID), tx.View(studentID)
		if teacher == nil || student == nil || teacherID == studentID || roleString(student, "MentorID") != teacherID || mentorSet(teacher, "MentorDisciples")[studentID] == nil {
			return previewReject(4582)
		}
		if mentorPracticeUnavailable(student, day) {
			return previewReject(4580)
		}
		tLevel, sLevel := roleInt(teacher, "Level"), roleInt(student, "Level")
		if tLevel < 1 || sLevel < 1 || (tLevel-1)/10 <= (sLevel-1)/10 || (kind == mentorPower && tLevel-sLevel < 10) {
			return previewReject(4683)
		}
		previous := min(max(0, roleInt(student, "MentorIntimacy")), math.MaxInt32)
		if kind == mentorMeridians && previous < 100 {
			return previewReject(4008)
		}
		gain := s.mentorAffinityGain(studentID, teacher, student)
		cost := int64(5)
		event := "mentorship.power_sent"
		if kind == mentorMeridians {
			cost, amount, event = 20, 1, "mentorship.meridians_sent"
		} else {
			// The level table gives cultivation growth for this pupil's level.
			amount = int64(max(1, min(math.Round(s.levelInfo(sLevel).increaseSuperAir), math.MaxInt32)))
		}
		if gain == 20 {
			amount = min(2*amount, math.MaxInt32)
		}
		updatedTeacher := tx.Get(teacherID)
		if !spendArtisanVigor(updatedTeacher, cost) {
			return previewReject(4618)
		}
		vigor = roleInt(updatedTeacher, "CurrentVigor")
		intimacy = min(previous+gain, math.MaxInt32)
		pupil := tx.Get(studentID)
		pupil["MentorIntimacy"] = intimacy
		pupil["MentorPracticeDay"] = day
		// The sender snapshot survives renaming, graduation and leaving the lineage.
		pupil["MentorPendingPractice"] = Record{"Type": kind, "Amount": amount, "Teacher": Record{
			"Id": teacherID, "UserId": roleString(teacher, "UserId"), "Name": roleString(teacher, "Name"),
		}}
		amount = mentorPracticeAmount(pupil, mentorSet(pupil, "MentorPendingPractice"))
		return tx.RecordHistory(socialHistory(event, teacherID, teacher, studentID, student, Record{
			"amount": amount, "vigorCost": cost, "intimacyGain": intimacy - previous, "intimacy": intimacy, "promoted": previous < 100 && intimacy >= 100,
		}))
	})
	if err != nil {
		return nil, err
	}
	statusPB(out, true, 4581)
	putPB(out, Record{"CurrentVigor": vigor, "Intimacy": intimacy})
	if kind == mentorPower {
		set(out, "TuDiSuperAir", amount)
	} else {
		set(out, "TuDiAcupointPoint", amount)
	}
	notice := s.gameResponse("TongZhiTMCGResp_1014024", true, 4581)
	putPB(notice, Record{"TmcgType": kind, "TmcgNum": amount})
	return func() { s.pushGame(studentID, 1014024, notice) }, nil
}

func (s *Server) mentorClaimPractice(peer *Peer, _, out protoreflect.Message) (func(), error) {
	studentID := peer.currentRoleID()
	var intimacy int64
	var afterReply func()
	err := s.roles.Transact(func(tx *StoreTx) error {
		student := tx.View(studentID)
		reward := mentorSet(student, "MentorPendingPractice")
		if reward == nil {
			return previewReject(4577)
		}
		teacher := mentorSet(reward, "Teacher")
		teacherID, kind, amount := roleString(teacher, "Id"), roleInt(reward, "Type"), mentorPracticeAmount(student, reward)
		if teacherID == "" || amount < 1 || amount > math.MaxInt32 || (kind != mentorMeridians && kind != mentorPower) {
			return previewReject(5000)
		}
		field, maxBalance := "SuperAir", int64(math.MaxInt64)
		if kind == mentorMeridians {
			field, maxBalance = "AcupointValue", math.MaxInt32
		}
		balance := roleInt(student, field)
		if balance < 0 || balance > maxBalance-amount {
			return previewReject(4008)
		}
		pupil := tx.Get(studentID)
		pupil[field] = balance + amount
		if kind == mentorPower {
			recomputeGamePower(pupil)
			rate := 0.0
			if roleInt(pupil, "RunMask")&4 != 0 {
				rate = s.gameRate(pupil, 3) * 2
			}
			update := s.pb("SuperAirUpResp_9001004")
			putPB(update, Record{"SuperAir": balance + amount, "CurrentSuperAir": roleInt(pupil, "CurrentSuperAir"),
				"SuperAirRefiningValue": roleInt(pupil, "SuperAirRefiningValue"), "CurrentRunValue": rate,
				"IsBreak": !s.canBreak(student) && s.canBreak(pupil)})
			afterReply = func() { s.pushGame(studentID, 9001004, update) }
		}
		delete(pupil, "MentorPendingPractice")
		intimacy = roleInt(pupil, "MentorIntimacy")
		return tx.RecordHistory(socialHistory("mentorship.practice_claimed", studentID, student, teacherID, teacher,
			Record{"practiceType": kind, "amount": amount, "balance": balance + amount}))
	})
	if err != nil {
		return nil, err
	}
	statusPB(out, true, 4581)
	set(out, "Intimacy", intimacy)
	return afterReply, nil
}
