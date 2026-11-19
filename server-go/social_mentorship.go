package main

import (
	"maps"
	"math"
	"slices"
	"time"

	"google.golang.org/protobuf/encoding/protowire"
	"google.golang.org/protobuf/reflect/protoreflect"
)

// The client requests the entire inbox in one PB message. Bounding both
// directions also bounds the work of withdrawing other applications on acceptance.
const maxMentorRequests = 200

const mentorGreetingDayKey = "MentorGreetingDay"

var mentorGameZone = time.FixedZone("UTC+8", 8*60*60)

func mentorshipPreviewRoutes() []previewRoute {
	const module = "社交与宗门"
	return []previewRoute{
		{cmd: 1014012, module: module, response: "EnterShiTuResp_1014012", summary: "查询师尊、弟子及待处理拜师帖数量", build: previewMentorship, live: (*Server).mentorPanel},
		{cmd: 1014015, module: module, response: "QingAnResp_1014015", summary: "弟子每日向师尊请安并增加双方亲密度", blocked: true, live: (*Server).mentorGreeting},
		{cmd: 1014018, module: module, response: "BaiShiTieResp_1014018", summary: "查询当前角色收到的拜师帖", build: previewEmptySocialList, live: (*Server).mentorInbox},
		{cmd: 1014020, module: module, request: "ShouTuReq_1014020", response: "ShouTuResp_1014020", summary: "同意拜师申请并建立师徒关系", blocked: true, live: (*Server).mentorAccept},
		{cmd: 1014021, module: module, request: "BaiShiReq_1014021", response: "BaiShiResp_1014021", summary: "按角色名向师尊递交拜师帖", blocked: true, live: (*Server).mentorApply},
		{cmd: 1014022, module: module, response: "TuoLiShiMenResp_1014022", summary: "弟子离开师门并记录三天冷却", blocked: true, live: (*Server).mentorLeave},
		{cmd: 1014023, module: module, request: "BaiShiHuLueReq_1014023", response: "BaiShiHuLueResp_1014023", summary: "拒绝指定的拜师帖", blocked: true, live: (*Server).mentorReject},
		{cmd: 1014026, module: module, request: "ZhuChuShiMenReq_1014026", response: "ZhuChuShiMenResp_1014026", summary: "师尊逐出名下弟子", blocked: true, live: (*Server).mentorExpel},
	}
}

func mentorSet(role Record, name string) Record {
	set, _ := role[name].(Record)
	return set
}

func mentorInsert(role Record, field, id string) {
	set := mentorSet(role, field)
	if set == nil {
		set = Record{}
		role[field] = set
	}
	set[id] = true
}

func mentorValidate(tx *StoreTx, studentID, teacherID string, now time.Time) error {
	student, teacher := tx.View(studentID), tx.View(teacherID)
	if student == nil || teacher == nil || reservedStoreID(studentID) || reservedStoreID(teacherID) || studentID == teacherID {
		return previewReject(4569)
	}
	if roleInt(student, "MentorReapplyAfter") > now.UnixMilli() {
		return previewReject(4571)
	}
	if roleInt(teacher, "Level")-roleInt(student, "Level") <= 10 {
		return previewReject(4572)
	}
	if len(mentorSet(teacher, "MentorDisciples")) >= 3 {
		return previewReject(4573)
	}
	if mentorSet(teacher, "MentorFormerTeachers")[studentID] != nil || mentorSet(teacher, "MentorGraduates")[studentID] != nil {
		return previewReject(4588)
	}
	// A pupil can outlevel a former teacher. Always verify the ancestry when accepting.
	visited := map[string]bool{studentID: true}
	for id := teacherID; id != ""; id = roleString(tx.View(id), "MentorID") {
		if visited[id] {
			return previewReject(4588)
		}
		visited[id] = true
	}
	return nil
}

func (s *Server) mentorApply(peer *Peer, req, out protoreflect.Message) (func(), error) {
	studentID := peer.currentRoleID()
	teacherID, name := s.resolveSocialName(getString(req, "RoleName"))
	err := s.roles.Transact(func(tx *StoreTx) error {
		student, teacher := tx.View(studentID), tx.View(teacherID)
		if teacherID == "" || teacher == nil || roleString(teacher, "Name") != name {
			return previewReject(4569)
		}
		if roleString(student, "MentorID") != "" {
			return previewReject(4586)
		}
		if err := mentorValidate(tx, studentID, teacherID, time.Now()); err != nil {
			return err
		}
		if mentorSet(teacher, "MentorRequests")[studentID] != nil {
			return nil
		}
		if len(mentorSet(teacher, "MentorRequests")) >= maxMentorRequests ||
			len(mentorSet(student, "MentorApplications")) >= maxMentorRequests {
			return previewReject(4008)
		}
		mentorInsert(tx.Get(teacherID), "MentorRequests", studentID)
		mentorInsert(tx.Get(studentID), "MentorApplications", teacherID)
		return tx.RecordHistory(socialHistory("mentorship.applied", studentID, student, teacherID, teacher, nil))
	})
	statusPB(out, true, 4570)
	return nil, err
}

func (s *Server) mentorAccept(peer *Peer, req, out protoreflect.Message) (func(), error) {
	teacherID, studentID := peer.currentRoleID(), getString(req, "RoleId")
	err := s.roles.Transact(func(tx *StoreTx) error {
		teacher, student := tx.View(teacherID), tx.View(studentID)
		if studentID == "" || student == nil || teacher == nil || studentID == teacherID || reservedStoreID(studentID) {
			return previewReject(4569)
		}
		if current := roleString(student, "MentorID"); current != "" {
			if current == teacherID && mentorSet(teacher, "MentorDisciples")[studentID] != nil {
				return nil
			}
			return previewReject(4567)
		}
		if mentorSet(teacher, "MentorRequests")[studentID] == nil {
			return previewReject(4008)
		}
		if err := mentorValidate(tx, studentID, teacherID, time.Now()); err != nil {
			return err
		}
		for id := range mentorSet(student, "MentorApplications") {
			if other := tx.Get(id); other != nil {
				delete(mentorSet(other, "MentorRequests"), studentID)
			}
		}
		mentorInsert(tx.Get(teacherID), "MentorDisciples", studentID)
		updated := tx.Get(studentID)
		delete(updated, "MentorApplications")
		updated["MentorID"], updated["MentorIntimacy"] = teacherID, 0
		return tx.RecordHistory(socialHistory("mentorship.accepted", teacherID, teacher, studentID, student, nil),
			socialHistory("mentorship.joined", studentID, student, teacherID, teacher, nil))
	})
	statusPB(out, true, 4568)
	return nil, err
}

func (s *Server) mentorReject(peer *Peer, req, out protoreflect.Message) (func(), error) {
	teacherID, studentID := peer.currentRoleID(), getString(req, "RoleId")
	err := s.roles.Transact(func(tx *StoreTx) error {
		teacher, student := tx.View(teacherID), tx.View(studentID)
		if student == nil || mentorSet(teacher, "MentorRequests")[studentID] == nil {
			return previewReject(4008)
		}
		delete(mentorSet(tx.Get(teacherID), "MentorRequests"), studentID)
		delete(mentorSet(tx.Get(studentID), "MentorApplications"), teacherID)
		return tx.RecordHistory(socialHistory("mentorship.rejected", teacherID, teacher, studentID, student, nil))
	})
	statusPB(out, true, 4576)
	return nil, err
}

func (s *Server) mentorLeave(peer *Peer, _, out protoreflect.Message) (func(), error) {
	statusPB(out, true, 4574)
	return nil, s.endMentorship(peer.currentRoleID(), "", true)
}

func (s *Server) mentorExpel(peer *Peer, req, out protoreflect.Message) (func(), error) {
	statusPB(out, true, 4583)
	return nil, s.endMentorship(getString(req, "RoleId"), peer.currentRoleID(), false)
}

func mentorGameDay(now time.Time) string {
	return now.In(mentorGameZone).Format(time.DateOnly)
}

func (s *Server) mentorGreeting(peer *Peer, _, out protoreflect.Message) (func(), error) {
	intimacy, err := s.greetMentor(peer.currentRoleID(), time.Now())
	if err == nil {
		statusPB(out, true, 4566)
		set(out, "Intimacy", intimacy)
	}
	return nil, err
}

func (s *Server) greetMentor(studentID string, now time.Time) (int64, error) {
	var intimacy int64
	day := mentorGameDay(now)
	err := s.roles.Transact(func(tx *StoreTx) error {
		student := tx.View(studentID)
		teacherID := roleString(student, "MentorID")
		teacher := tx.View(teacherID)
		if student == nil || teacher == nil || studentID == teacherID || reservedStoreID(studentID) ||
			reservedStoreID(teacherID) || mentorSet(teacher, "MentorDisciples")[studentID] == nil {
			return previewReject(4008)
		}
		if roleString(student, mentorGreetingDayKey) >= day {
			return previewReject(4565)
		}

		gain := s.mentorAffinityGain(teacherID, teacher, student)
		previous := min(max(0, roleInt(student, "MentorIntimacy")), math.MaxInt32)
		intimacy = min(previous+gain, math.MaxInt32)
		updated := tx.Get(studentID)
		updated["MentorIntimacy"] = intimacy
		updated[mentorGreetingDayKey] = day
		return tx.RecordHistory(socialHistory("mentorship.greeted", studentID, student, teacherID, teacher,
			Record{"intimacyGain": intimacy - previous, "intimacy": intimacy, "promoted": previous < 100 && intimacy >= 100}))
	})
	return intimacy, err
}

func (s *Server) endMentorship(studentID, teacherID string, voluntary bool) error {
	return s.roles.Transact(func(tx *StoreTx) error {
		student := tx.View(studentID)
		current := roleString(student, "MentorID")
		if current == "" || (!voluntary && current != teacherID) {
			return previewReject(4582)
		}
		teacher := tx.View(current)
		if teacher == nil || mentorSet(teacher, "MentorDisciples")[studentID] == nil {
			return previewReject(4582)
		}
		updated := tx.Get(studentID)
		delete(updated, "MentorID")
		delete(updated, "MentorIntimacy")
		mentorInsert(updated, "MentorFormerTeachers", current)
		delete(mentorSet(tx.Get(current), "MentorDisciples"), studentID)
		if voluntary {
			updated["MentorReapplyAfter"] = time.Now().Add(72 * time.Hour).UnixMilli()
			return tx.RecordHistory(socialHistory("mentorship.left", studentID, student, current, teacher, nil))
		}
		return tx.RecordHistory(socialHistory("mentorship.expelled", current, teacher, studentID, student, nil))
	})
}

func (s *Server) mentorIdentity(out protoreflect.Message, id string, role Record) {
	copyRolePB(out, role, "Name", "Level", "HeadPortraitId")
	set(out, "Sex", roleInt(role, "Sex") != 0 || roleBool(role, "Sex"))
	if out.Descriptor().Fields().ByName("Id") != nil {
		set(out, "Id", id)
	}
	if out.Descriptor().Fields().ByName("IsOnLine") != nil {
		_, online := s.online.Load(id)
		set(out, "IsOnLine", online)
	}
}

func (s *Server) mentorPanel(peer *Peer, _, out protoreflect.Message) (func(), error) {
	today := mentorGameDay(time.Now())
	s.roles.mu.RLock()
	defer s.roles.mu.RUnlock()
	self := s.roles.records[peer.currentRoleID()]
	if self == nil {
		return nil, previewReject(4007)
	}
	fillMentorPending(self, out)
	set(out, "ChuShiNum", roleInt(self, "ChuShiNum"))
	// Additive panel fields for the pupil's last graduation. Legacy clients skip them.
	if last := mentorSet(self, "MentorLastGraduation"); last != nil {
		extra := protowire.AppendString(protowire.AppendTag(nil, 20, protowire.BytesType), roleString(last, "RoleName"))
		out.SetUnknown(protowire.AppendVarint(protowire.AppendTag(extra, 21, protowire.VarintType), uint64(roleInt(last, "Intimacy"))))
	}
	set(out, "BaiShiTieNum", len(mentorSet(self, "MentorRequests")))
	if id := roleString(self, "MentorID"); id != "" {
		if teacher := s.roles.records[id]; teacher != nil {
			entry := out.Mutable(field(out, "ShiFu")).Message()
			s.mentorIdentity(entry, id, teacher)
			set(entry, "Intimacy", roleInt(self, "MentorIntimacy"))
			set(entry, "IsQingAn", roleString(self, mentorGreetingDayKey) >= today)
		}
	}
	registered, personal := 0, 0
	for _, id := range slices.Sorted(maps.Keys(mentorSet(self, "MentorDisciples"))) {
		student := s.roles.records[id]
		if student == nil {
			continue
		}
		entry := addMessage(out, "TuDis")
		s.mentorIdentity(entry, id, student)
		intimacy := roleInt(student, "MentorIntimacy")
		set(entry, "Intimacy", intimacy)
		set(entry, "IsTongMaiOrChuanGong", mentorPracticeUnavailable(student, today))
		if intimacy >= 100 {
			personal++
		} else {
			registered++
		}
	}
	set(out, "JiMingNum", registered)
	set(out, "QinChuanNum", personal)
	return nil, nil
}

func (s *Server) mentorInbox(peer *Peer, _, out protoreflect.Message) (func(), error) {
	s.roles.mu.RLock()
	defer s.roles.mu.RUnlock()
	self := s.roles.records[peer.currentRoleID()]
	if self == nil {
		return nil, previewReject(4007)
	}
	for _, id := range slices.Sorted(maps.Keys(mentorSet(self, "MentorRequests"))) {
		if student := s.roles.records[id]; student != nil {
			s.mentorIdentity(addMessage(out, "BaiShiDiZis"), id, student)
		}
	}
	return nil, nil
}
