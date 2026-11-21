package main

import (
	"math"
	"os"
	"path/filepath"
	"testing"
	"time"

	"google.golang.org/protobuf/reflect/protoreflect"
)

func TestMentorshipPracticeWireOfflineAndClaim(t *testing.T) {
	dir := t.TempDir()
	s := auctionTestServer(t, dir)
	for id, r := range map[string]Record{
		"master":  {"Name": "青山", "UserId": "master-account", "Level": 32, "CurrentVigor": 60, "MapId": "121101", "MentorDisciples": Record{"student": true}},
		"student": {"Name": "青柳", "UserId": "pupil-account", "Level": 13, "MentorID": "master", "MentorIntimacy": 95, "SuperAir": 120, "CurrentSuperAir": 60, "RunMask": 4, "AcupointValue": 3, "MapId": "121101"},
		"other":   {"Name": "路人", "Level": 50, "CurrentVigor": 100},
	} {
		if err := s.roles.Put(id, r); err != nil {
			t.Fatal(err)
		}
	}
	s.game.levels[13] = gameLevel{increaseSuperAir: 35}
	teacher, teacherConn := auctionTestRole(t, s, "master", nil)
	other, otherConn := auctionTestRole(t, s, "other", nil)
	call := func(p *Peer, c *auctionTestConnection, cmd int, params Record, code int64) protoreflect.Message {
		t.Helper()
		msg := auctionTestMessage(t, s, cmd, params)
		s.handleGame(p, msg)
		return auctionTestReply(t, s, c, msg, code)
	}
	target := Record{"RoleId": "student"}
	if out := call(other, otherConn, 1014017, target, 4582); getBool(out, "IsSuccess") {
		t.Fatal(out)
	}
	if out := call(teacher, teacherConn, 1014016, target, 4008); getBool(out, "IsSuccess") {
		t.Fatal(out)
	}
	out := call(teacher, teacherConn, 1014017, target, 4581)
	if !getBool(out, "IsSuccess") || getInt(out, "Intimacy") != 100 || getInt(out, "TuDiSuperAir") != 35 || getInt(out, "CurrentVigor") != 55 {
		t.Fatal(out)
	}
	panel := call(teacher, teacherConn, 1014012, nil, 2000)
	if getInt(panel, "QinChuanNum") != 1 || !getBool(panel.Get(field(panel, "TuDis")).List().Get(0).Message(), "IsTongMaiOrChuanGong") {
		t.Fatal(panel)
	}
	if out := call(teacher, teacherConn, 1014016, target, 4580); getBool(out, "IsSuccess") {
		t.Fatal(out)
	}
	if roleInt(s.roles.Get("student"), "SuperAir") != 120 {
		t.Fatal("unclaimed power applied prematurely")
	}
	s = auctionTestServer(t, dir)
	student, studentConn := auctionTestRole(t, s, "student", nil)
	s.game.levels[13] = gameLevel{upgradeLimit: 130, increaseSuperAir: 35, trainingPerception: 10}
	panel = call(student, studentConn, 1014012, nil, 2000)
	if getBool(panel, "IsGetTmcg") || getInt(panel, "TmcgType") != 2 || getInt(panel, "TmcgNum") != 35 {
		t.Fatal(panel)
	}
	if out := call(student, studentConn, 1014025, nil, 4581); !getBool(out, "IsSuccess") || getInt(out, "Intimacy") != 100 {
		t.Fatal(out)
	}
	update := studentConn.receive(t, s, 9001004, 0, "SuperAirUpResp_9001004")
	if getInt(update, "SuperAir") != 155 || getInt(update, "CurrentSuperAir") != 60 || !getBool(update, "IsBreak") {
		t.Fatalf("stale live cultivation: %v", update)
	}
	s.cultivatePeerAs(student, student.currentRoleID(), &[3]float64{0, 0, 1}, 15)
	studentConn.receive(t, s, 9001004, 0, "SuperAirUpResp_9001004")
	if out := call(student, studentConn, 1014025, nil, 4577); getBool(out, "IsSuccess") {
		t.Fatal(out)
	}
	if balance := roleInt(s.roles.Get("student"), "SuperAir"); balance != 155 {
		t.Fatalf("claimed balance %d", balance)
	}

	teacher, teacherConn = auctionTestRole(t, s, "master", nil)
	if out := call(teacher, teacherConn, 1014016, target, 4580); getBool(out, "IsSuccess") {
		t.Fatal(out)
	}
	if err := s.roles.Update("student", func(r Record) error {
		r["MentorPracticeDay"] = mentorGameDay(time.Now().Add(-48 * time.Hour))
		return nil
	}); err != nil {
		t.Fatal(err)
	}
	out = call(teacher, teacherConn, 1014016, target, 4581)
	if getInt(out, "TuDiAcupointPoint") != 2 || getInt(out, "CurrentVigor") != 35 || getInt(out, "Intimacy") != 120 {
		t.Fatal(out)
	}
	notice := studentConn.receive(t, s, 1014024, 0, "TongZhiTMCGResp_1014024")
	if getInt(notice, "TmcgNum") != 2 || getInt(notice, "TmcgType") != 1 {
		t.Fatal(notice)
	}
	// A paid reward survives the prior teacher's name change and the end of the relation.
	if err := s.roles.Update("master", func(r Record) error { r["Name"] = "改名"; return nil }); err != nil {
		t.Fatal(err)
	}
	call(teacher, teacherConn, 1014026, target, 4583)
	s = auctionTestServer(t, dir)
	student, studentConn = auctionTestRole(t, s, "student", nil)
	if out := call(student, studentConn, 1014025, nil, 4581); !getBool(out, "IsSuccess") || getInt(out, "Intimacy") != 0 {
		t.Fatal(out)
	}
	if balance := roleInt(s.roles.Get("student"), "AcupointValue"); balance != 5 {
		t.Fatalf("claimed points %d", balance)
	}
	panel = call(student, studentConn, 1014012, nil, 2000)
	if !getBool(panel, "IsGetTmcg") || getInt(panel, "TmcgNum") != 0 {
		t.Fatal(panel)
	}
	events, err := s.roles.pendingHistory()
	if err != nil || len(events) != 5 {
		t.Fatalf("events %v, err %v", events, err)
	}
	if events[0].Kind != "mentorship.power_sent" || events[0].Actor.AccountID != "master-account" ||
		events[0].Subject.RoleID != "student" || events[0].Details["promoted"] != true ||
		events[4].Kind != "mentorship.practice_claimed" || events[4].Actor.AccountID != "pupil-account" ||
		events[4].Subject.RoleID != "master" || events[4].Subject.RoleName != "青山" {
		t.Fatalf("wrong event actors or awards %#v", events)
	}
}

func TestMentorshipPracticeEligibilityAndAtomicClaims(t *testing.T) {
	dir := t.TempDir()
	s := auctionTestServer(t, dir)
	if err := s.roles.Put("s", Record{"Level": 12, "MentorID": "m", "MentorIntimacy": 150, "AcupointValue": 2}); err != nil {
		t.Fatal(err)
	}
	if err := s.roles.Put("m", Record{"Level": 13, "CurrentVigor": 19, "MentorDisciples": Record{"s": true}}); err != nil {
		t.Fatal(err)
	}
	m, _ := auctionTestRole(t, s, "m", nil)
	p, _ := auctionTestRole(t, s, "s", nil)
	req := s.pb("TongMaiReq_1014016")
	set(req, "RoleId", "s")
	send := func() error { _, err := s.mentorOpenMeridians(m, req, s.pb("TongMaiResp_1014016")); return err }
	if err := send(); err != previewReject(4683) {
		t.Fatalf("same stage accepted %v", err)
	}
	if err := s.roles.Update("m", func(r Record) error { r["Level"] = 28; return nil }); err != nil {
		t.Fatal(err)
	}
	if err := send(); err != previewReject(4618) {
		t.Fatalf("insufficient vigor %v", err)
	}
	if err := s.roles.Update("m", func(r Record) error { r["CurrentVigor"] = 20; return nil }); err != nil {
		t.Fatal(err)
	}
	// Make the atomic rename fail without touching an existing save.
	real := s.roles.path
	s.roles.path = filepath.Join(dir, "blocked")
	if err := os.Mkdir(s.roles.path, 0700); err != nil {
		t.Fatal(err)
	}
	if err := send(); err == nil {
		t.Fatal("send succeeded despite failing write")
	}
	if roleInt(s.roles.Get("m"), "CurrentVigor") != 20 || mentorSet(s.roles.Get("s"), "MentorPendingPractice") != nil {
		t.Fatal("failed save published partial training")
	}
	s.roles.path = real
	if err := send(); err != nil {
		t.Fatal(err)
	}
	if err := s.roles.Update("s", func(r Record) error {
		r["MentorPracticeDay"] = "2000-01-01"
		r["AcupointValue"] = math.MaxInt32
		return nil
	}); err != nil {
		t.Fatal(err)
	}
	if err := send(); err != previewReject(4580) {
		t.Fatalf("overwrote unclaimed reward on next day: %v", err)
	}
	claim := func() error { _, err := s.mentorClaimPractice(p, nil, s.pb("JieShouTMCGResp_1014025")); return err }
	if err := claim(); err != previewReject(4008) {
		t.Fatalf("overflowing balance accepted: %v", err)
	}
	if err := s.roles.Update("s", func(r Record) error { r["AcupointValue"] = 2; return nil }); err != nil {
		t.Fatal(err)
	}
	s.roles.path = filepath.Join(dir, "blocked")
	if err := claim(); err == nil {
		t.Fatal("claim succeeded despite failing write")
	}
	if roleInt(s.roles.Get("s"), "AcupointValue") != 2 || mentorSet(s.roles.Get("s"), "MentorPendingPractice") == nil {
		t.Fatal("failed claim lost pending reward")
	}
	s.roles.path = real
	done := make(chan error, 2)
	for range 2 {
		go func() { done <- claim() }()
	}
	first, second := <-done, <-done
	if (first != nil || second != previewReject(4577)) && (second != nil || first != previewReject(4577)) {
		t.Fatalf("claim errors %v and %v", first, second)
	}
	if balance := roleInt(s.roles.Get("s"), "AcupointValue"); balance != 3 {
		t.Fatalf("concurrent claim balance %d", balance)
	}
}
