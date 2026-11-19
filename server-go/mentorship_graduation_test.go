package main

import (
	"os"
	"path/filepath"
	"slices"
	"testing"

	"google.golang.org/protobuf/reflect/protoreflect"
)

func TestGraduateDisciplePBRewardsAndPersistentReceipt(t *testing.T) {
	dir := t.TempDir()
	s := auctionTestServer(t, dir)
	s.game.graduationTitleLuck = map[int]int64{501: 201}
	for id, save := range map[string]Record{
		"teacher":  {"UserId": "a1", "Name": "松风", "Level": 41, "MentorDisciples": Record{"disciple": true}, "QiYun": 17, "TitleId": []any{101}},
		"disciple": {"UserId": "a2", "Name": "溪雨", "Level": 22, "MentorID": "teacher", "MentorIntimacy": 299, "MentorPracticeDay": "today", "MentorPendingPractice": Record{"Type": 1, "Amount": 10, "Teacher": Record{"Id": "teacher", "Name": "松风", "UserId": "a1"}}},
		"stranger": {"Name": "路人"},
	} {
		if err := s.roles.Put(id, save); err != nil {
			t.Fatal(err)
		}
	}
	type result struct {
		reply protoreflect.Message
		conn  *auctionTestConnection
	}
	call := func(from string, cmd int, data Record, code int64, want bool) result {
		t.Helper()
		peer, conn := auctionTestRole(t, s, from, nil)
		msg := auctionTestMessage(t, s, cmd, data)
		s.handleGame(peer, msg)
		reply := auctionTestReply(t, s, conn, msg, code)
		if getBool(reply, "IsSuccess") != want {
			t.Fatal(reply)
		}
		return result{reply, conn}
	}
	request := Record{"RoleId": "disciple"}
	call("stranger", 1014027, request, 4582, false)
	call("teacher", 1014027, request, 4584, false)
	if err := s.roles.Update("disciple", func(role Record) error { role["MentorIntimacy"] = 300; return nil }); err != nil {
		t.Fatal(err)
	}
	originalPath := s.roles.path
	badDir := filepath.Join(dir, "blocked")
	if err := os.WriteFile(badDir, []byte{}, 0600); err != nil {
		t.Fatal(err)
	}
	s.roles.path = filepath.Join(badDir, "roles.json")
	failure := call("teacher", 1014027, request, previewDeniedCode, false)
	s.roles.path = originalPath
	if len(failure.conn.frames) != 0 || roleInt(s.roles.Get("teacher"), "QiYun") != 17 || roleString(s.roles.Get("disciple"), "MentorID") != "teacher" {
		t.Fatal("rejected save altered graduation state or announced a title")
	}
	grad := call("teacher", 1014027, request, 4585, true)
	if getInt(grad.reply, "QiYun") != 268 {
		t.Fatal(grad.reply)
	}
	title := grad.conn.receive(t, s, 1002006, 0, "InformTitleResp_1002006")
	if getInt(title, "Title") != 501 || !getBool(title, "IsSuccess") {
		t.Fatal(title)
	}
	s = auctionTestServer(t, dir)
	panel := call("teacher", 1014012, nil, 2000, true).reply
	if getInt(panel, "ChuShiNum") != 1 || panel.Get(field(panel, "TuDis")).List().Len() != 0 {
		t.Fatal(panel)
	}
	student := s.roles.Get("disciple")
	graduate := mentorSet(s.roles.Get("teacher"), "MentorGraduates")["disciple"].(Record)
	from := student["MentorLastGraduation"].(Record)
	if roleInt(graduate, "GraduatedAt") <= 0 || roleInt(from, "GraduatedAt") != roleInt(graduate, "GraduatedAt") ||
		roleInt(from, "Intimacy") != 300 || roleInt(graduate, "Intimacy") != 300 || roleString(from, "RoleName") != "松风" ||
		roleString(graduate, "RoleName") != "溪雨" || roleString(from, "RoleId") != "teacher" || roleString(graduate, "RoleId") != "disciple" {
		t.Fatal("graduation snapshots differ from the participants at graduation")
	}
	if roleString(student, "MentorID") != "" || roleString(student, "MentorPracticeDay") != "today" || roleInt(student["MentorPendingPractice"].(Record), "Amount") != 10 {
		t.Fatal("graduation must preserve already earned instruction")
	}
	if !slices.Equal(friendTitleIDs(s.roles.Get("teacher")), []int{101, 501}) || roleInt(s.roles.Get("teacher"), "QiYun") != 268 {
		t.Fatal("title or luck did not persist")
	}
	duplicate := call("teacher", 1014027, request, 4585, true)
	if len(duplicate.conn.frames) != 0 || getInt(duplicate.reply, "QiYun") != 268 {
		t.Fatal("duplicate graduation awarded a title again")
	}
	events, err := s.roles.pendingHistory()
	if err != nil || len(events) != 3 {
		t.Fatalf("history %#v, %v", events, err)
	}
	if events[0].Kind != "title.earned" || events[0].Actor.RoleID != "teacher" || events[0].EntityID != "501" ||
		events[1].Kind != "mentorship.graduated" || events[1].Actor.RoleID != "disciple" || events[1].Subject.RoleID != "teacher" ||
		events[2].Kind != "mentorship.trained_graduate" || events[2].Actor.AccountID != "a1" || events[2].Subject.AccountID != "a2" {
		t.Fatalf("incorrect graduate attribution: %#v", events)
	}
	claim := call("disciple", 1014025, nil, 4581, true).reply
	if getInt(claim, "Intimacy") != 0 || roleInt(s.roles.Get("disciple"), "AcupointValue") != 10 {
		t.Fatal("graduate could not claim earned meridians independently")
	}
}

func TestGraduatingExtraDisciplesAwardsEachTitleOnce(t *testing.T) {
	s := auctionTestServer(t, t.TempDir())
	for _, tc := range []struct {
		count, expectedLuck int
		expected            []int
	}{
		{3, 50, []int{501}}, {4, 550, []int{501, 502}}, {9, 1050, []int{501, 502, 503}},
	} {
		existing := []int{501}
		if tc.count == 9 {
			existing = append(existing, 502)
		}
		if err := s.roles.Put("teacher", Record{"Id": "teacher", "ChuShiNum": tc.count, "TitleId": existing, "MentorDisciples": Record{"student": true}}); err != nil {
			t.Fatal(err)
		}
		if err := s.roles.Put("student", Record{"Id": "student", "MentorID": "teacher", "MentorIntimacy": 301}); err != nil {
			t.Fatal(err)
		}
		peer, _ := auctionTestRole(t, s, "teacher", nil)
		msg := auctionTestMessage(t, s, 1014027, Record{"RoleId": "student"})
		s.handleGame(peer, msg)
		now := s.roles.Get("teacher")
		if roleInt(now, "QiYun") != int64(tc.expectedLuck) || !slices.Equal(friendTitleIDs(now), tc.expected) {
			t.Fatalf("graduation %d: %#v", tc.count+1, now)
		}
	}
}

func TestConcurrentGraduationAwardsOneReward(t *testing.T) {
	s := auctionTestServer(t, t.TempDir())
	if err := s.roles.Put("teacher", Record{"Id": "teacher", "MentorDisciples": Record{"pupil": true}}); err != nil {
		t.Fatal(err)
	}
	if err := s.roles.Put("pupil", Record{"Id": "pupil", "MentorID": "teacher", "MentorIntimacy": 300}); err != nil {
		t.Fatal(err)
	}
	one, conn1 := auctionTestRole(t, s, "teacher", nil)
	two, conn2 := auctionTestRole(t, s, "teacher", nil)
	req := auctionTestMessage(t, s, 1014027, Record{"RoleId": "pupil"})
	start, done := make(chan struct{}), make(chan struct{})
	go func() { defer close(done); <-start; s.handleGame(one, req) }()
	close(start)
	s.handleGame(two, req)
	<-done
	pushes := 0
	for _, conn := range []*auctionTestConnection{conn1, conn2} {
		response := auctionTestReply(t, s, conn, req, 4585)
		if !getBool(response, "IsSuccess") || getInt(response, "QiYun") != 250 {
			t.Fatal(response)
		}
		pushes += len(conn.frames)
	}
	record := s.roles.Get("teacher")
	if pushes != 1 || roleInt(record, "ChuShiNum") != 1 || roleInt(record, "QiYun") != 250 {
		t.Fatalf("concurrent rewards: %v", record)
	}
}
