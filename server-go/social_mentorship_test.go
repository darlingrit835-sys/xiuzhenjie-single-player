package main

import (
	"slices"
	"strconv"
	"testing"
	"time"

	"google.golang.org/protobuf/reflect/protoreflect"
)

func TestMentorshipClientMessagesAndPersistentOwnership(t *testing.T) {
	dir := t.TempDir()
	s := auctionTestServer(t, dir)
	for id, level := range map[string]int{"pupil": 5, "master": 16, "other": 40, "low": 15} {
		if err := s.roles.Put(id, Record{"Id": id, "Name": "角色-" + id, "UserId": "账号-" + id, "Level": level, "Sex": 1, "HeadPortraitId": 6}); err != nil {
			t.Fatal(err)
		}
	}

	// Each request uses a freshly bound peer, as after a reconnect.
	call := func(actor string, cmd int, req Record, success bool, code int64) protoreflect.Message {
		t.Helper()
		peer, conn := auctionTestRole(t, s, actor, nil)
		msg := auctionTestMessage(t, s, cmd, req)
		s.handleGame(peer, msg)
		out := auctionTestReply(t, s, conn, msg, code)
		if getBool(out, "IsSuccess") != success {
			t.Fatalf("actor %s cmd %d: %v", actor, cmd, out)
		}
		return out
	}
	apply := func(target string, ok bool, code int64) {
		t.Helper()
		call("pupil", 1014021, Record{"RoleName": "角色-" + target}, ok, code)
	}
	pupil := Record{"RoleId": "pupil"}
	apply("missing", false, 4569)
	apply("pupil", false, 4569)
	apply("low", false, 4572)
	call("master", 1014020, pupil, false, 4008)
	call("pupil", 1014021, Record{"RoleName": "\u3000角色-master  "}, true, 4570)
	apply("master", true, 4570)
	apply("other", true, 4570)
	s = auctionTestServer(t, dir)
	panel := call("master", 1014012, nil, true, 2000)
	if getInt(panel, "BaiShiTieNum") != 1 || !getBool(panel, "IsGetTmcg") {
		t.Fatalf("unexpected application count or spurious transfer: %v", panel)
	}
	inbox := call("master", 1014018, nil, true, 2000)
	entries := inbox.Get(field(inbox, "BaiShiDiZis")).List()
	if entries.Len() != 1 || getString(entries.Get(0).Message(), "Id") != "pupil" ||
		getInt(entries.Get(0).Message(), "Level") != 5 || !getBool(entries.Get(0).Message(), "Sex") {
		t.Fatalf("unexpected applicant details: %v", inbox)
	}
	call("master", 1014023, pupil, true, 4576)
	call("master", 1014020, pupil, false, 4008)
	apply("master", true, 4570)
	call("master", 1014020, pupil, true, 4568)
	call("master", 1014020, pupil, true, 4568)
	call("other", 1014020, pupil, false, 4567)
	apply("other", false, 4586)
	s = auctionTestServer(t, dir)
	panel = call("master", 1014012, nil, true, 2000)
	students := panel.Get(field(panel, "TuDis")).List()
	if getInt(panel, "JiMingNum") != 1 || getInt(panel, "BaiShiTieNum") != 0 || students.Len() != 1 || getString(students.Get(0).Message(), "Id") != "pupil" {
		t.Fatalf("missing accepted pupil: %v", panel)
	}
	panel = call("pupil", 1014012, nil, true, 2000)
	teacher := panel.Get(field(panel, "ShiFu")).Message()
	if getString(teacher, "Name") != "角色-master" || getInt(teacher, "Level") != 16 || getString(teacher, "HeadPortraitId") != "6" || !getBool(teacher, "IsOnLine") {
		t.Fatalf("incorrect master profile: %v", panel)
	}
	if getInt(call("other", 1014012, nil, true, 2000), "BaiShiTieNum") != 0 {
		t.Fatal("accepted pupil still listed with another prospective master")
	}
	call("other", 1014026, pupil, false, 4582)
	call("master", 1014026, pupil, true, 4583)
	apply("master", true, 4570)
	call("master", 1014020, pupil, true, 4568)
	call("pupil", 1014022, nil, true, 4574)
	s = auctionTestServer(t, dir)
	apply("other", false, 4571)
	events, err := s.roles.pendingHistory()
	if err != nil {
		t.Fatal(err)
	}
	var got []string
	for _, e := range events {
		if e.Subject == nil || e.Actor.AccountID != "账号-"+e.Actor.RoleID || e.Subject.AccountID != "账号-"+e.Subject.RoleID {
			t.Fatalf("missing counterpart identity: %#v", e)
		}
		got = append(got, e.Kind+" "+e.Actor.RoleID+" "+e.Subject.RoleID)
	}
	want := []string{
		"mentorship.applied pupil master", "mentorship.applied pupil other", "mentorship.rejected master pupil",
		"mentorship.applied pupil master", "mentorship.accepted master pupil", "mentorship.joined pupil master",
		"mentorship.expelled master pupil", "mentorship.applied pupil master", "mentorship.accepted master pupil",
		"mentorship.joined pupil master", "mentorship.left pupil master",
	}
	if !slices.Equal(got, want) {
		t.Fatalf("events: %v, want %v", got, want)
	}
}

func TestMentorshipConcurrentFourthPupil(t *testing.T) {
	s := auctionTestServer(t, t.TempDir())
	master, _ := auctionTestRole(t, s, "master", nil)
	if err := s.roles.Update("master", func(r Record) error { r["Level"] = 60; return nil }); err != nil {
		t.Fatal(err)
	}
	studentIDs := []string{"a", "b", "c", "d"}
	for _, id := range studentIDs {
		student, _ := auctionTestRole(t, s, id, nil)
		req := s.pb("BaiShiReq_1014021")
		set(req, "RoleName", "master")
		if _, err := s.mentorApply(student, req, s.pb("BaiShiResp_1014021")); err != nil {
			t.Fatal(err)
		}
	}
	results := make(chan error, len(studentIDs))
	for _, id := range studentIDs {
		go func() {
			req := s.pb("ShouTuReq_1014020")
			set(req, "RoleId", id)
			_, err := s.mentorAccept(master, req, s.pb("ShouTuResp_1014020"))
			results <- err
		}()
	}
	accepted := 0
	for range studentIDs {
		switch err := <-results; err {
		case nil:
			accepted++
		case previewReject(4573):
		default:
			t.Fatal(err)
		}
	}
	if role := s.roles.Get("master"); accepted != 3 || len(mentorSet(role, "MentorDisciples")) != 3 || len(mentorSet(role, "MentorRequests")) != 1 {
		t.Fatalf("master exceeded capacity or lost unaccepted application")
	}
}

func TestMentorshipLimitsPendingApplicationWork(t *testing.T) {
	s := auctionTestServer(t, t.TempDir())
	student, _ := auctionTestRole(t, s, "student", nil)
	auctionTestRole(t, s, "teacher", nil)
	if err := s.roles.Update("teacher", func(r Record) error { r["Level"] = 60; return nil }); err != nil {
		t.Fatal(err)
	}
	req := s.pb("BaiShiReq_1014021")
	set(req, "RoleName", "teacher")
	fill := func(roleID, field, lastID string) {
		t.Helper()
		if err := s.roles.Update(roleID, func(r Record) error {
			r[field] = Record{}
			for i := 1; i < maxMentorRequests; i++ {
				mentorInsert(r, field, strconv.Itoa(i))
			}
			mentorInsert(r, field, lastID)
			return nil
		}); err != nil {
			t.Fatal(err)
		}
	}
	apply := func() error {
		_, err := s.mentorApply(student, req, s.pb("BaiShiResp_1014021"))
		return err
	}
	fill("student", "MentorApplications", "someone-else")
	if err := apply(); err != previewReject(4008) {
		t.Fatalf("outgoing limit not enforced: %v", err)
	}
	if err := s.roles.Update("student", func(r Record) error { delete(r, "MentorApplications"); return nil }); err != nil {
		t.Fatal(err)
	}
	fill("teacher", "MentorRequests", "someone-else")
	if err := apply(); err != previewReject(4008) {
		t.Fatalf("incoming limit not enforced: %v", err)
	}
	fill("teacher", "MentorRequests", "student")
	fill("student", "MentorApplications", "teacher")
	if err := apply(); err != nil {
		t.Fatalf("duplicate request should succeed even at capacity: %v", err)
	}
	events, err := s.roles.pendingHistory()
	if err != nil || len(events) != 0 {
		t.Fatalf("duplicate or rejected requests created history: %d, %v", len(events), err)
	}
}

func TestMentorshipGreetingWireAndSharedIntimacy(t *testing.T) {
	dir := t.TempDir()
	s := auctionTestServer(t, dir)
	for id, role := range map[string]Record{
		"pupil":  {"Id": "pupil", "Name": "松间小友", "MentorID": "master", "MentorIntimacy": 81, "MapId": "121101"},
		"master": {"Id": "master", "Name": "青山散人", "MentorDisciples": Record{"pupil": true}, "MapId": "121101"},
	} {
		if err := s.roles.Put(id, role); err != nil {
			t.Fatal(err)
		}
	}
	student, studentConn := auctionTestRole(t, s, "pupil", nil)
	master, masterConn := auctionTestRole(t, s, "master", nil)
	call := func(peer *Peer, conn *auctionTestConnection, cmd int, code int64) protoreflect.Message {
		t.Helper()
		msg := auctionTestMessage(t, s, cmd, nil)
		s.handleGame(peer, msg)
		return auctionTestReply(t, s, conn, msg, code)
	}
	if !s.preview[1014015].blocked {
		t.Fatal("greeting is not gated with the other trial writes")
	}
	s.previewWrites = false
	if out := call(student, studentConn, 1014015, previewDeniedCode); getBool(out, "IsSuccess") {
		t.Fatalf("read-only trial awarded a greeting: %v", out)
	}
	s.previewWrites = true
	if out := call(student, studentConn, 1014015, 4566); !getBool(out, "IsSuccess") || getInt(out, "Intimacy") != 101 {
		t.Fatalf("unexpected same-city greeting: %v", out)
	}
	if out := call(student, studentConn, 1014015, 4565); getBool(out, "IsSuccess") {
		t.Fatalf("duplicate greeting was accepted: %v", out)
	}
	pupilPanel := call(student, studentConn, 1014012, 2000)
	teacher := pupilPanel.Get(field(pupilPanel, "ShiFu")).Message()
	if !getBool(teacher, "IsQingAn") || getInt(teacher, "Intimacy") != 101 {
		t.Fatalf("pupil panel did not show today's greeting: %v", pupilPanel)
	}
	masterPanel := call(master, masterConn, 1014012, 2000)
	disciples := masterPanel.Get(field(masterPanel, "TuDis")).List()
	if disciples.Len() != 1 || getInt(disciples.Get(0).Message(), "Intimacy") != 101 || getInt(masterPanel, "QinChuanNum") != 1 {
		t.Fatalf("master panel did not show the shared intimacy: %v", masterPanel)
	}
	s = auctionTestServer(t, dir)
	student, studentConn = auctionTestRole(t, s, "pupil", nil)
	call(student, studentConn, 1014015, 4565)
	events, err := s.roles.pendingHistory()
	if err != nil || len(events) != 1 || events[0].Kind != "mentorship.greeted" || events[0].Actor.RoleID != "pupil" {
		t.Fatalf("unexpected greeting history: %#v %v", events, err)
	}
}

func TestMentorshipGreetingUsesGameMidnightAndCurrentRelation(t *testing.T) {
	s := auctionTestServer(t, t.TempDir())
	auctionTestRole(t, s, "pupil", nil)
	master, _ := auctionTestRole(t, s, "master", nil)
	if err := s.roles.Update("pupil", func(r Record) error {
		r["MentorID"], r["MapId"] = "master", "121101"
		return nil
	}); err != nil {
		t.Fatal(err)
	}
	beforeMidnight := time.Date(2026, time.November, 19, 15, 59, 59, 0, time.UTC)
	if _, err := s.greetMentor("pupil", beforeMidnight); err != previewReject(4008) {
		t.Fatalf("greeting without reciprocal relation returned %v", err)
	}
	if err := s.roles.Update("master", func(r Record) error {
		r["MentorDisciples"], r["MapId"] = Record{"pupil": true}, "121102"
		return nil
	}); err != nil {
		t.Fatal(err)
	}
	s.online.Delete("master")
	if intimacy, err := s.greetMentor("pupil", beforeMidnight); err != nil || intimacy != 5 {
		t.Fatalf("offline greeting returned %d (%v)", intimacy, err)
	}
	s.online.Store("master", master)
	if intimacy, err := s.greetMentor("pupil", beforeMidnight.Add(time.Second)); err != nil || intimacy != 15 {
		t.Fatalf("online greeting at UTC+8 midnight returned %d (%v)", intimacy, err)
	}
	if _, err := s.greetMentor("pupil", beforeMidnight.Add(time.Hour)); err != previewReject(4565) {
		t.Fatalf("repeat after game midnight returned %v", err)
	}
}
