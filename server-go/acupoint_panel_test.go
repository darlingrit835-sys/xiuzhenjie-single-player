package main

import (
	"os"
	"path/filepath"
	"testing"

	"google.golang.org/protobuf/reflect/protoreflect"
)

func acupointTestServer(t *testing.T, saveDir string) *Server {
	t.Helper()
	s := auctionTestServer(t, saveDir)
	// Load the project's actual 1 -> 2 path. Player writes go to the temporary saveDir.
	s.dataDir = filepath.Join("..", "server", "data")
	if err := s.loadGameTables(); err != nil {
		t.Fatal(err)
	}
	return s
}

func acupointTestBreak(t *testing.T, s *Server, peer *Peer, requestID int, id string) protoreflect.Message {
	t.Helper()
	msg := auctionTestMessage(t, s, 1005018, Record{"AcupointId": id})
	msg.Msid = requestID
	s.handleGame(peer, msg)
	return peer.conn.(*auctionTestConnection).receive(t, s, msg.Cmd, msg.Msid, "BreakAcupointResp_1005018")
}

func checkAcupointPanel(t *testing.T, s *Server, peer *Peer, wantBalance int64, wantLevels map[string]int64) {
	t.Helper()
	msg := auctionTestMessage(t, s, 1005016, nil)
	s.handleGame(peer, msg)
	out := peer.conn.(*auctionTestConnection).receive(t, s, msg.Cmd, msg.Msid, "OpenAcupointResp_1005016")
	if !getBool(out, "IsSuccess") || getInt(out, "AcupointValue") != wantBalance {
		t.Fatalf("inner-view balance: %v, want %d", out, wantBalance)
	}
	list := out.Get(field(out, "Acupoints")).List()
	if list.Len() != len(wantLevels) {
		t.Fatalf("expected %v, got %v", wantLevels, out)
	}
	seen := make(map[string]bool)
	for i := 0; i < list.Len(); i++ {
		point := list.Get(i).Message()
		id := getString(point, "Id")
		level, exists := wantLevels[id]
		if !exists || seen[id] || getInt(point, "BrokenLevel") != level {
			t.Fatalf("unexpected acupoint entries: %v", out)
		}
		seen[id] = true
	}
}

func TestAcupointPanelOldSaveAndAttemptValidation(t *testing.T) {
	dir := t.TempDir()
	s := acupointTestServer(t, dir)
	if err := s.roles.Put("pupil", Record{"Name": "独行", "Level": 31, "AcupointValue": 1}); err != nil {
		t.Fatal(err)
	}
	peer, _ := auctionTestRole(t, s, "pupil", nil)
	for i, id := range []string{"", "unknown", "2"} {
		if out := acupointTestBreak(t, s, peer, i+1, id); getBool(out, "IsSuccess") || getInt(out, "Code") != 4146 {
			t.Fatalf("unexpected response for invalid or locked point %q: %v", id, out)
		}
	}
	checkAcupointPanel(t, s, peer, 1, nil)
	first := acupointTestBreak(t, s, peer, 4, "1")
	if !getBool(first, "IsSuccess") || !getBool(first, "BreakSuccess") || getInt(first, "Code") != 4142 || getInt(first, "BrokenLevel") != 1 || getInt(first, "AcupointValue") != 0 {
		t.Fatal(first)
	}
	for i, id := range []string{"1", "2"} {
		if out := acupointTestBreak(t, s, peer, i+5, id); getBool(out, "IsSuccess") || getInt(out, "Code") != 4144 {
			t.Fatal(out)
		}
	}
	s = acupointTestServer(t, dir)
	peer, _ = auctionTestRole(t, s, "pupil", nil)
	checkAcupointPanel(t, s, peer, 0, map[string]int64{"1": 1})
	if err := s.roles.Update("pupil", func(r Record) error { r["AcupointValue"] = 2; return nil }); err != nil {
		t.Fatal(err)
	}
	second := acupointTestBreak(t, s, peer, 1, "2")
	if !getBool(second, "IsSuccess") || !getBool(second, "BreakSuccess") || getInt(second, "AcupointValue") != 0 {
		t.Fatal(second)
	}
	s = acupointTestServer(t, dir)
	peer, _ = auctionTestRole(t, s, "pupil", nil)
	checkAcupointPanel(t, s, peer, 0, map[string]int64{"1": 1, "2": 1})
}

func TestAcupointFailedSaveAllowsStableRetry(t *testing.T) {
	dir := t.TempDir()
	s := acupointTestServer(t, dir)
	if err := s.roles.Put("actor", Record{"Id": "actor", "Name": "清音", "Level": 31, "AcupointValue": 12}); err != nil {
		t.Fatal(err)
	}
	peer, _ := auctionTestRole(t, s, "actor", nil)
	path := s.roles.path
	blocked := filepath.Join(dir, "blocked")
	if err := os.WriteFile(blocked, []byte("file, not directory"), 0600); err != nil {
		t.Fatal(err)
	}
	s.roles.path = filepath.Join(blocked, "roles.json")
	out := acupointTestBreak(t, s, peer, 8, "1")
	s.roles.path = path
	if getBool(out, "IsSuccess") {
		t.Fatal("save failure reported success")
	}
	checkAcupointPanel(t, s, peer, 12, nil)
	if pending, err := s.roles.pendingHistory(); err != nil || len(pending) != 0 {
		t.Fatalf("failed save leaked history: %v, %v", pending, err)
	}
	first := acupointTestBreak(t, s, peer, 8, "1")
	if !getBool(first, "IsSuccess") || !getBool(first, "BreakSuccess") || getInt(first, "AcupointValue") != 11 {
		t.Fatal(first)
	}
	replay := acupointTestBreak(t, s, peer, 8, "1")
	if !getBool(replay, "IsSuccess") || !getBool(replay, "BreakSuccess") || getInt(replay, "AcupointValue") != 11 {
		t.Fatal(replay)
	}
	s = acupointTestServer(t, dir)
	peer, _ = auctionTestRole(t, s, "actor", nil)
	checkAcupointPanel(t, s, peer, 11, map[string]int64{"1": 1})
}

func TestAcupointConcurrentSessionsCannotSpendOnePointTwice(t *testing.T) {
	dir := t.TempDir()
	s := acupointTestServer(t, dir)
	if err := s.roles.Put("actor", Record{"Id": "actor", "Level": 31, "AcupointValue": 1}); err != nil {
		t.Fatal(err)
	}
	peer1, c1 := auctionTestRole(t, s, "actor", nil)
	peer2, c2 := auctionTestRole(t, s, "actor", nil)
	msg := auctionTestMessage(t, s, 1005018, Record{"AcupointId": "1"})
	start, done := make(chan struct{}), make(chan struct{})
	go func() { defer close(done); <-start; s.handleGame(peer1, msg) }()
	close(start)
	s.handleGame(peer2, msg)
	<-done
	wins := 0
	for _, conn := range []*auctionTestConnection{c1, c2} {
		out := conn.receive(t, s, msg.Cmd, msg.Msid, "BreakAcupointResp_1005018")
		if getBool(out, "IsSuccess") && getBool(out, "BreakSuccess") {
			wins++
		} else if getInt(out, "Code") != 4144 {
			t.Fatal(out)
		}
	}
	if wins != 1 {
		t.Fatalf("one available point, %d successes", wins)
	}
	s = acupointTestServer(t, dir)
	peer1, _ = auctionTestRole(t, s, "actor", nil)
	checkAcupointPanel(t, s, peer1, 0, map[string]int64{"1": 1})
}

func TestAcupointPanelUsesSavedBalance(t *testing.T) {
	dir := t.TempDir()
	s := acupointTestServer(t, dir)
	// Read-only preview can still display legacy IDs even if the current table omits them.
	if err := s.roles.Put("pupil", Record{"AcupointValue": 7, "Acupoints": []any{Record{"Id": "302", "BrokenLevel": 2}}}); err != nil {
		t.Fatal(err)
	}
	s = acupointTestServer(t, dir)
	s.previewWrites = false
	peer, _ := auctionTestRole(t, s, "pupil", nil)
	out := acupointTestBreak(t, s, peer, 1, "1")
	if getBool(out, "IsSuccess") || getInt(out, "Code") != previewDeniedCode {
		t.Fatal(out)
	}
	checkAcupointPanel(t, s, peer, 7, map[string]int64{"302": 2})
}

func TestAcupointMaxedLevelNeverConsumesPoints(t *testing.T) {
	dir := t.TempDir()
	s := acupointTestServer(t, dir)
	if err := s.roles.Put("pupil", Record{"Id": "pupil", "AcupointValue": 20, "Acupoints": []any{Record{"Id": "1", "BrokenLevel": 3}}}); err != nil {
		t.Fatal(err)
	}
	peer, _ := auctionTestRole(t, s, "pupil", nil)
	out := acupointTestBreak(t, s, peer, 1, "1")
	if getBool(out, "IsSuccess") || getInt(out, "Code") != 4141 {
		t.Fatal(out)
	}
	checkAcupointPanel(t, s, peer, 20, map[string]int64{"1": 3})
}

func TestAcupointCanOpenViaAlternativeParent(t *testing.T) {
	dir := t.TempDir()
	s := acupointTestServer(t, dir)
	// b.json lists both 璇玑 (19) and 期门 (29) as parents of 天突 (20).
	if err := s.roles.Put("pupil", Record{"Id": "pupil", "AcupointValue": 8, "Acupoints": []any{Record{"Id": "29", "BrokenLevel": 1}}}); err != nil {
		t.Fatal(err)
	}
	peer, _ := auctionTestRole(t, s, "pupil", nil)
	out := acupointTestBreak(t, s, peer, 11, "20")
	if !getBool(out, "IsSuccess") || !getBool(out, "BreakSuccess") || getInt(out, "BrokenLevel") != 1 || getInt(out, "AcupointValue") != 0 {
		t.Fatal(out)
	}
	checkAcupointPanel(t, s, peer, 0, map[string]int64{"29": 1, "20": 1})
}

func TestAcupointSecondAndThirdLevelChanceAndCost(t *testing.T) {
	dir := t.TempDir()
	s := acupointTestServer(t, dir)
	if err := s.roles.Put("pupil", Record{"Id": "pupil", "UserId": "account-1", "Name": "清音", "AcupointValue": 20, "Acupoints": []any{Record{"Id": "1", "BrokenLevel": 1}}}); err != nil {
		t.Fatal(err)
	}
	// Check both sides of the 50% and 10% thresholds without a random test.
	for _, tc := range []struct {
		roll, code     int
		level, balance int64
	}{
		{50, 4145, 1, 17}, {49, 4142, 2, 14}, {10, 4145, 2, 9}, {9, 4142, 3, 4},
	} {
		result, err := s.applyAcupointBreak("pupil", "1", func(n int) int {
			if n != 100 {
				t.Fatalf("unexpected roll range %d", n)
			}
			return tc.roll
		})
		if err != nil {
			t.Fatal(err)
		}
		out := s.pb("BreakAcupointResp_1005018")
		result.write(out)
		if !getBool(out, "IsSuccess") || getBool(out, "BreakSuccess") != (tc.code == 4142) || getInt(out, "Code") != int64(tc.code) || getInt(out, "BrokenLevel") != tc.level || getInt(out, "AcupointValue") != tc.balance {
			t.Fatalf("roll %d: %v", tc.roll, out)
		}
	}
	s = acupointTestServer(t, dir)
	peer, _ := auctionTestRole(t, s, "pupil", nil)
	checkAcupointPanel(t, s, peer, 4, map[string]int64{"1": 3})
	events, err := s.roles.pendingHistory()
	if err != nil || len(events) != 4 {
		t.Fatalf("history count %d, %v", len(events), err)
	}
	for _, ev := range events {
		if ev.Actor.RoleID != "pupil" || ev.Actor.AccountID != "account-1" || ev.EntityID != "1" || ev.Kind != "cultivation.acupoint_failed" && ev.Kind != "cultivation.acupoint_broken" {
			t.Fatalf("unexpected history %+v", ev)
		}
	}
	// Only the highest applicable type 14 title counts (+20, not +30).
	if err := s.roles.Update("pupil", func(r Record) error {
		r["TitleId"] = []any{801, 802}
		r["Acupoints"] = []any{Record{"Id": "1", "BrokenLevel": 1}}
		r["AcupointValue"] = 6
		return nil
	}); err != nil {
		t.Fatal(err)
	}
	for _, tc := range []struct{ roll, code int }{{70, 4145}, {69, 4142}} {
		result, err := s.applyAcupointBreak("pupil", "1", func(int) int { return tc.roll })
		if err != nil || result.code != tc.code {
			t.Fatalf("title bonus roll %d: %+v, %v", tc.roll, result, err)
		}
	}
}

func TestAcupointExistingInjuryAndMissingTableCannotSpend(t *testing.T) {
	dir := t.TempDir()
	s := acupointTestServer(t, dir)
	if err := s.roles.Put("pupil", Record{"Id": "pupil", "AcupointValue": 2, "IsBrokenAcupoint": true}); err != nil {
		t.Fatal(err)
	}
	peer, _ := auctionTestRole(t, s, "pupil", nil)
	if out := acupointTestBreak(t, s, peer, 1, "1"); getBool(out, "IsSuccess") || getInt(out, "Code") != 4143 {
		t.Fatal(out)
	}
	checkAcupointPanel(t, s, peer, 2, nil)
	s = auctionTestServer(t, dir)
	s.dataDir = dir
	if err := s.loadAcupoints(); err != nil {
		t.Fatal(err)
	}
	peer, _ = auctionTestRole(t, s, "pupil", nil)
	if out := acupointTestBreak(t, s, peer, 1, "1"); getBool(out, "IsSuccess") || getInt(out, "Code") != previewDeniedCode {
		t.Fatal(out)
	}
	checkAcupointPanel(t, s, peer, 2, nil)
}
