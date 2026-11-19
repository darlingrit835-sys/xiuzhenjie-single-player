package main

import (
	"math"
	"slices"
	"strconv"
	"time"

	"google.golang.org/protobuf/reflect/protoreflect"
)

func mentorshipGraduationRoutes() []previewRoute {
	return []previewRoute{
		{cmd: 1014027, module: "社交与宗门", request: "ChuShiReq_1014027", response: "ChuShiResp_1014027", summary: "师尊安排亲密满 300 的弟子出师，累计出师人数、气运和师徒称号", blocked: true, live: (*Server).graduateDisciple},
	}
}

// Client help specifies the minimum intimacy and 50 luck per graduate. The
// title table provides their effects; the admission counts are trial rules.
var graduationTitles = [...]struct {
	count       int64
	id          int
	defaultLuck int64
}{
	{1, 501, 200}, {5, 502, 500}, {10, 503, 1000},
}

func (s *Server) graduateDisciple(peer *Peer, req, out protoreflect.Message) (func(), error) {
	teacherID, studentID := peer.currentRoleID(), getString(req, "RoleId")
	if studentID == "" || teacherID == studentID || reservedStoreID(studentID) {
		return nil, previewReject(4582)
	}

	var qiYun int64
	var unlocked []int
	err := s.roles.Transact(func(tx *StoreTx) error {
		teacher, student := tx.View(teacherID), tx.View(studentID)
		if teacher == nil || student == nil {
			return previewReject(4582)
		}
		if mentorSet(teacher, "MentorGraduates")[studentID] != nil {
			// A retry remains harmless even after the student joins another teacher.
			qiYun = roleInt(teacher, "QiYun")
			return nil
		}
		if roleString(student, "MentorID") != teacherID || mentorSet(teacher, "MentorDisciples")[studentID] == nil {
			return previewReject(4582)
		}
		intimacy := roleInt(student, "MentorIntimacy")
		if intimacy < 300 {
			return previewReject(4584)
		}
		previous := max(roleInt(teacher, "ChuShiNum"), int64(len(mentorSet(teacher, "MentorGraduates"))))
		if previous >= math.MaxInt32 {
			return previewReject(4008)
		}
		count := previous + 1
		reward := int64(50)
		titles := slices.Clone(friendTitleIDs(teacher))
		events := make([]historyEvent, 0, 5)
		for _, title := range graduationTitles {
			if count < title.count || slices.Contains(titles, title.id) {
				continue
			}
			effect, ok := s.game.graduationTitleLuck[title.id]
			if !ok {
				effect = title.defaultLuck
			}
			reward += effect
			titles = append(titles, title.id)
			unlocked = append(unlocked, title.id)
			event := socialHistory("title.earned", teacherID, teacher, studentID, student, Record{"qiYunGain": effect, "graduates": count})
			event.EntityType, event.EntityID = "title", strconv.Itoa(title.id)
			events = append(events, event)
		}
		qiYun = roleInt(teacher, "QiYun")
		if qiYun < 0 || qiYun > math.MaxInt32-reward {
			return previewReject(4008)
		}
		qiYun += reward
		teacher, student = tx.Get(teacherID), tx.Get(studentID)
		teacher["ChuShiNum"], teacher["QiYun"] = count, qiYun
		if len(unlocked) > 0 {
			teacher["TitleId"] = titles
		}
		delete(mentorSet(teacher, "MentorDisciples"), studentID)
		graduatedAt := time.Now().UTC().UnixMilli()
		graduate := Record{"RoleId": studentID, "RoleName": roleString(student, "Name"), "Intimacy": intimacy, "GraduatedAt": graduatedAt}
		mentorInsert(teacher, "MentorGraduates", studentID)
		mentorSet(teacher, "MentorGraduates")[studentID] = graduate
		student["MentorLastGraduation"] = Record{"RoleId": teacherID, "RoleName": roleString(teacher, "Name"), "Intimacy": intimacy, "GraduatedAt": graduatedAt}
		mentorInsert(student, "MentorFormerTeachers", teacherID)
		delete(student, "MentorID")
		delete(student, "MentorIntimacy")
		details := Record{"graduates": count, "intimacy": intimacy, "qiYun": qiYun, "qiYunGain": reward}
		events = append(events, socialHistory("mentorship.graduated", studentID, student, teacherID, teacher, details),
			socialHistory("mentorship.trained_graduate", teacherID, teacher, studentID, student, details))
		return tx.RecordHistory(events...)
	})
	if err != nil {
		return nil, err
	}
	statusPB(out, true, 4585)
	set(out, "QiYun", qiYun)
	if len(unlocked) == 0 {
		return nil, nil
	}
	return func() {
		for _, id := range unlocked {
			msg := s.gameResponse("InformTitleResp_1002006", true, 2000)
			set(msg, "Title", id)
			_ = peer.send(1002006, 0, msg)
		}
	}, nil
}
