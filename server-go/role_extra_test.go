package main

import (
	"os"
	"path/filepath"
	"testing"
	"time"

	"google.golang.org/protobuf/proto"
	"google.golang.org/protobuf/reflect/protoreflect"
)

func TestSkipAcupointGuideOnlyChangesLoginProjection(t *testing.T) {
	s := auctionTestServer(t, t.TempDir())
	if err := s.roles.Put("pupil", Record{
		"Id": "pupil", "UserId": "owner", "Level": 2, "StarterKitVersion": 2,
		"AcupointGuideVal": 2, "PracticeGuideVal": 3, "AcupointValue": 5,
	}); err != nil {
		t.Fatal(err)
	}
	client := &auctionTestConnection{}
	peer := &Peer{conn: client}
	for i, tc := range []struct {
		skip bool
		want int64
	}{{false, 2}, {true, 100}} {
		s.config.SkipAcupointGuide = tc.skip
		login := s.pb("RoleLoginReq_1002002")
		putPB(login, Record{
			"RoleId": "pupil", "GameServerIpManageId": s.issuePreviewSession("owner", time.Now()),
		})
		body, err := proto.Marshal(login)
		if err != nil {
			t.Fatal(err)
		}
		s.handleGame(peer, Message{Cmd: 1002002, Msid: i + 1, Data: body})
		reply := client.receive(t, s, 1002002, i+1, "RoleLoginResp_1002002")
		if !getBool(reply, "IsSuccess") || getInt(reply, "AcupointGuideVal") != tc.want || getInt(reply, "PracticeGuideVal") != 3 {
			t.Fatalf("guide skip=%t: %v", tc.skip, reply)
		}
		role := s.roles.Get("pupil")
		if roleInt(role, "AcupointGuideVal") != 2 || roleInt(role, "AcupointValue") != 5 {
			t.Fatal("the login workaround mutated meridian progress")
		}
	}
}

func TestOwnedTitleSwitchAndRelog(t *testing.T) {
	dir := t.TempDir()
	s := auctionTestServer(t, dir)
	if err := s.roles.Put("a", Record{"Id": "a", "UserId": "owner", "TitleId": []any{501, 502}, "Title": 501, "TitleFrame": 17}); err != nil {
		t.Fatal(err)
	}
	peer, client := auctionTestRole(t, s, "a", nil)
	selectTitle := func(id, code int, want bool) {
		t.Helper()
		msg := auctionTestMessage(t, s, 1002007, Record{"Title": id})
		s.handleGame(peer, msg)
		got := auctionTestReply(t, s, client, msg, int64(code))
		if getBool(got, "IsSuccess") != want {
			t.Fatalf("select title %d: %v", id, got)
		}
	}
	selectTitle(0, 4285, false)
	selectTitle(503, 4285, false)
	s.previewWrites = false
	selectTitle(502, previewDeniedCode, false)
	s.previewWrites = true
	selectTitle(502, 4286, true)
	selectTitle(502, 4286, true)
	history, err := s.roles.pendingHistory()
	if err != nil || len(history) != 1 || history[0].Kind != "role.title_selected" || history[0].Actor.RoleID != "a" {
		t.Fatalf("title history: %v, %v", history, err)
	}

	s = auctionTestServer(t, dir)
	client = &auctionTestConnection{}
	peer = &Peer{conn: client}
	login := s.pb("RoleLoginReq_1002002")
	putPB(login, Record{"RoleId": "a", "GameServerIpManageId": s.issuePreviewSession("owner", time.Now())})
	body, err := proto.Marshal(login)
	if err != nil {
		t.Fatal(err)
	}
	s.handleGame(peer, Message{Cmd: 1002002, Msid: 1, Data: body})
	reply := client.receive(t, s, 1002002, 1, "RoleLoginResp_1002002")
	titles := reply.Get(field(reply, "TitleId")).List()
	if !getBool(reply, "IsSuccess") || getInt(reply, "Title") != 502 || getInt(reply, "TitleFrame") != 17 ||
		titles.Len() != 2 || titles.Get(0).Int() != 501 || titles.Get(1).Int() != 502 {
		t.Fatalf("relogged title state: %v", reply)
	}
}

func TestOwnedGoodsDetailRoutes(t *testing.T) {
	dir := t.TempDir()
	s := auctionTestServer(t, dir)
	peer, client := auctionTestRole(t, s, "a", []any{
		Record{"GoodsId": "animal", "Type": 10, "FourGas": "温", "ReceiveVal": 0.75, "IsSuccess": false, "Code": 5000},
		Record{"GoodsId": "ore", "Type": 13, "Intensity": 2.5, "Unit": "块"},
		Record{"GoodsId": "plant", "Type": 12, "MapName": "试用产地", "CreatedBy": "测试角色"},
		Record{"GoodsId": "book", "Type": 2, "ElixirLevel": 4, "EffectValue": int64(3456789012), "MaterialNames": []string{"甲", "乙"}},
	})
	_, _ = auctionTestRole(t, s, "b", []any{Record{"GoodsId": "their-ore", "Type": 13, "Unit": "块"}})
	s.previewWrites = false
	read := func(cmd int, id string, code int, success bool) protoreflect.Message {
		t.Helper()
		msg := auctionTestMessage(t, s, cmd, Record{"Id": id})
		s.handleGame(peer, msg)
		out := auctionTestReply(t, s, client, msg, int64(code))
		if getBool(out, "IsSuccess") != success {
			t.Fatalf("read %s: %v", id, out)
		}
		return out
	}
	path := filepath.Join(dir, "roles.json")
	before, err := os.ReadFile(path)
	if err != nil {
		t.Fatal(err)
	}
	animal := read(1004005, "animal", 4071, true)
	ore := read(1004006, "ore", 4071, true)
	plant := read(1004007, "plant", 4071, true)
	book := read(1004009, "book", 4071, true)
	names := book.Get(field(book, "MaterialNames")).List()
	if getString(animal, "FourGas") != "温" || getFloat(animal, "ReceiveVal") != 0.75 ||
		getFloat(ore, "Intensity") != 2.5 || getString(ore, "Unit") != "块" || getString(plant, "CreatedBy") != "测试角色" ||
		getInt(book, "ElixirLevel") != 4 || getInt(book, "EffectValue") != 3456789012 ||
		names.Len() != 2 || names.Get(0).String() != "甲" || names.Get(1).String() != "乙" {
		t.Fatalf("wrong detail fields: %v %v %v %v", animal, ore, plant, book)
	}
	read(1004006, "animal", 4069, false)
	for _, id := range []string{"", "missing", "their-ore"} {
		if out := read(1004006, id, 4070, false); getString(out, "Unit") != "" {
			t.Fatalf("rejected lookup returned details: %v", out)
		}
	}
	peer = &Peer{conn: client}
	read(1004009, "book", previewDeniedCode, false)
	after, err := os.ReadFile(path)
	if err != nil || string(before) != string(after) {
		t.Fatalf("details modified the save: %v", err)
	}
}
