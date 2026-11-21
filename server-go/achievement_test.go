package main

import (
	"sync"
	"testing"

	"google.golang.org/protobuf/encoding/protowire"
	"google.golang.org/protobuf/proto"
	"google.golang.org/protobuf/reflect/protoreflect"
)

func TestTianShuBeiProtocolAndLegacyProgress(t *testing.T) {
	dir := t.TempDir()
	s := auctionTestServer(t, dir)
	peer, wire := auctionTestRole(t, s, "role", nil)
	if err := s.roles.Update("role", func(r Record) error {
		r["Level"], r["TaskId"], r["QuestKills"], r["QuestSweeps"] = 11, 46, 7, 1
		return nil
	}); err != nil {
		t.Fatal(err)
	}
	call := func(cmd int, body []byte, response string) protoreflect.Message {
		t.Helper()
		wire.frames = nil
		s.handleGame(peer, Message{Cmd: cmd, Msid: 1, Data: body})
		return wire.receive(t, s, cmd, 1, response)
	}
	list := func() map[int64]protoreflect.Message {
		response := call(achievementListCmd, nil, "OpenTianShuBeiResp_1016004")
		rows := response.Get(field(response, "TianShuBeiObj")).List()
		if !getBool(response, "IsSuccess") || rows.Len() != len(achievements) {
			t.Fatalf("failed to list records: %v", response)
		}
		byID := make(map[int64]protoreflect.Message)
		for i := 0; i < rows.Len(); i++ {
			row := rows.Get(i).Message()
			id := getInt(row, "Id")
			if id <= 0 || byID[id] != nil {
				t.Fatalf("invalid or duplicate ID: %d", id)
			}
			byID[id] = row
		}
		return byID
	}
	claim := func(id, kind int64) protoreflect.Message {
		req := s.pb("TianShuBeiReceiveAwardReq_1016006")
		putPB(req, Record{"Id": id, "Type": kind})
		payload, err := proto.Marshal(req)
		if err != nil {
			t.Fatal(err)
		}
		return call(achievementClaimCmd, payload, "TianShuBeiReceiveAwardResp_1016006")
	}
	rows := list()
	if getInt(rows[501], "Status") != 1 || getInt(rows[502], "CurrentVal") != 7 ||
		getInt(rows[801], "Status") != 1 || getInt(rows[701], "Status") != 1 {
		t.Fatal("existing save progress was not counted")
	}
	for _, attempt := range [][2]int64{{503, 5}, {501, 8}, {99999, 5}} {
		if getBool(claim(attempt[0], attempt[1]), "IsSuccess") {
			t.Fatalf("ineligible or mismatched claim succeeded: %v", attempt)
		}
	}
	checkClaim := func() {
		t.Helper()
		out := claim(501, 5)
		if !getBool(out, "IsSuccess") || getInt(out, "Status") != 2 || getInt(out, "Id") != 501 {
			t.Fatalf("bad claim receipt: %v", out)
		}
		tag, kind, n := protowire.ConsumeTag(out.GetUnknown())
		if tag != 6 || kind != protowire.VarintType || n <= 0 {
			t.Fatalf("missing wallet field: %v", out)
		}
		balance, size := protowire.ConsumeVarint(out.GetUnknown()[n:])
		if balance != 1100 || size <= 0 {
			t.Fatalf("bad wallet balance: %d", balance)
		}
		tag, kind, n = protowire.ConsumeTag(out.GetUnknown()[n+size:])
		if tag != 7 || kind != protowire.VarintType || n <= 0 || out.GetUnknown()[len(out.GetUnknown())-1] != 1 {
			t.Fatal("missing pending-reward badge in receipt")
		}
	}
	checkClaim()
	checkClaim()
	restored, err := OpenStore(dir, "roles")
	if err != nil {
		t.Fatal(err)
	}
	s.roles = restored
	checkClaim()
	if getInt(list()[501], "Status") != 2 || roleInt(restored.Get("role"), "ShinyStone") != 1100 {
		t.Fatal("claim did not survive restart or was awarded more than once")
	}
	rule := inferQuest("完成3个天书碑成就")
	r := restored.Get("role")
	s.game.rules = map[int64]questRule{46: rule}
	if !s.checkQuest(r) {
		t.Fatal("main quest did not count three earned achievements")
	}
	r = Record{"TaskId": 46, "Level": 11, "TaskAutoAt": 1}
	if s.checkQuest(r) {
		t.Fatal("main quest completed before three achievements")
	}
}

func TestTianShuBeiConcurrentClaimAndRollback(t *testing.T) {
	dir := t.TempDir()
	s := auctionTestServer(t, dir)
	auctionTestRole(t, s, "role", nil)
	a := achievements[0]
	path := s.roles.path
	s.roles.path = dir
	if _, _, awarded, err := s.awardAchievement("role", a); err == nil || awarded {
		t.Fatal("failed save was acknowledged")
	}
	s.roles.path = path
	if roleInt(s.roles.Get("role"), "ShinyStone") != 1000 {
		t.Fatal("failed save changed the balance")
	}
	var group sync.WaitGroup
	for range 10 {
		group.Go(func() {
			if _, _, _, err := s.awardAchievement("role", a); err != nil {
				t.Errorf("eligible claim or retry: %v", err)
			}
		})
	}
	group.Wait()
	if roleInt(s.roles.Get("role"), "ShinyStone") != 1100 {
		t.Fatal("parallel requests granted the same reward multiple times")
	}
	history, err := s.roles.pendingHistory()
	if err != nil || len(history) != 1 {
		t.Fatalf("expected one durable claim event, got %d with %v", len(history), err)
	}
}
