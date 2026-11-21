package main

import (
	"bytes"
	"sync"
	"testing"

	"google.golang.org/protobuf/reflect/protoreflect"
)

func TestChamberGradeUpgradeExpandsExistingBox(t *testing.T) {
	s := auctionTestServer(t, t.TempDir())
	if err := s.roles.Put("owner", Record{
		"Name": "清和", "Level": 31, "Live1Type": 3, "Live2Type": 8, "ShinyStone": 40,
		"StorageBoxes": []any{Record{"Name": "旧藏", "Goods": []any{
			Record{"GoodsId": "old", "Name": "旧玉", "TemplateId": "jade", "Type": 9, "Count": 200, "Volume": 1},
		}}},
		"Goods": []any{Record{"GoodsId": "new", "Name": "灵砂", "TemplateId": "sand", "Type": 9, "Count": 201, "Volume": 1}},
	}); err != nil {
		t.Fatal(err)
	}
	p, c := auctionTestRole(t, s, "owner", nil)
	move := func(cmd, seq int, id string, count int, code int64) protoreflect.Message {
		t.Helper()
		msg := auctionTestMessage(t, s, cmd, Record{"GoodsId": id, "Location": 1, "Count": count})
		msg.Msid = seq
		s.handleGame(p, msg)
		return auctionTestReply(t, s, c, msg, code)
	}
	read := func(version []byte) protoreflect.Message {
		t.Helper()
		msg := inventoryReadMessage(1004002, 1, version)
		s.handleGame(p, msg)
		return c.receive(t, s, msg.Cmd, msg.Msid, "SpaceGoodsResp_1004002")
	}
	reply := read([]byte{})
	if getInt(reply, "NowCapacity") != 200 || getInt(reply, "MaxCapacity") != 200 {
		t.Fatal(reply)
	}
	move(1005022, 1, "new", 1, 4155)
	for grade, capacity := range []int{250, 300, 350, 400} {
		next := grade + 1
		previousVersion, _ := inventoryReadExtension(t, reply)
		s.game.homes[next] = homeGrade{Price: 10}
		s.handleGame(p, Message{Cmd: 1005002, Msid: next, Data: []byte{8, byte(next)}})
		upgrade := c.receive(t, s, 1005002, next, "UpgradeHomeResp_1005002")
		if !getBool(upgrade, "IsSuccess") {
			t.Fatal(upgrade)
		}
		s.handleGame(p, Message{Cmd: 1005001, Msid: next})
		home := c.receive(t, s, 1005001, next, "EnterHomeResp_1005001")
		if getInt(home, "Rank") != int64(next*4) || getInt(home, "Live1Type") != 3 || getInt(home, "Live2Type") != 8 {
			t.Fatal(home)
		}
		reply = read(previousVersion)
		version, unchanged := inventoryReadExtension(t, reply)
		if unchanged || bytes.Equal(version, previousVersion) || getInt(reply, "MaxCapacity") != int64(capacity) ||
			getInt(reply, "NowCapacity") != int64(capacity-50) {
			t.Fatal("upgrading must invalidate the old inventory snapshot and preserve stored goods", reply)
		}
		put := move(1005022, 10*next, "new", 50, 4156)
		if getInt(put, "NowBoxCapacity") != int64(capacity) {
			t.Fatal(put)
		}
		move(1005022, 10*next+1, "new", 1, 4155)
		reply = read([]byte{})
	}
	if got := getInt(move(1005023, 50, "old", 200, 4159), "NowBoxCapacity"); got != 200 {
		t.Fatalf("old goods could not be withdrawn intact: remaining volume %d", got)
	}
}

func TestChamberUpgradeRespectsWorldMode(t *testing.T) {
	s := auctionTestServer(t, t.TempDir())
	s.game.homes[1] = homeGrade{Price: 100}
	peer, client := auctionTestRole(t, s, "owner", nil)
	upgrade := func(wantCode int64, wantRank, wantStones int64) {
		t.Helper()
		s.handleGame(peer, Message{Cmd: 1005002, Msid: 1, Data: []byte{8, 1}})
		reply := client.receive(t, s, 1005002, 1, "UpgradeHomeResp_1005002")
		role := s.roles.Get("owner")
		if getInt(reply, "Code") != wantCode || getBool(reply, "IsSuccess") != (wantCode == 4111) ||
			getInt(reply, "Rank") != 4*wantRank || getInt(reply, "ShinyStone") != wantStones ||
			gameHomeRank(role) != wantRank || roleInt(role, "ShinyStone") != wantStones {
			t.Fatalf("unexpected upgrade result %v with rank %d and balance %d", reply, gameHomeRank(role), roleInt(role, "ShinyStone"))
		}
	}
	s.previewWrites = false
	upgrade(previewDeniedCode, 0, 1000)
	s.preview = nil
	upgrade(4111, 1, 900)
}

func TestStorageRoutesRoundTripAndPersistence(t *testing.T) {
	dir := t.TempDir()
	s := auctionTestServer(t, dir)
	if err := s.roles.Put("owner", Record{"ShinyStone": 20000, "ShinyJade": 50, "Goods": []any{
		Record{"GoodsId": "owned-item", "Name": "fixture", "TemplateId": "fixture", "Type": 9, "Count": 4, "Volume": 1.5},
	}}); err != nil {
		t.Fatal(err)
	}
	peer, c := auctionTestRole(t, s, "owner", nil)
	request := func(cmd, seq int, data Record, code int64) protoreflect.Message {
		t.Helper()
		msg := auctionTestMessage(t, s, cmd, data)
		msg.Msid = seq
		s.handleGame(peer, msg)
		return auctionTestReply(t, s, c, msg, code)
	}
	list := request(1005005, 1, nil, 4120)
	if names := list.Get(field(list, "Name")).List(); names.Len() != 1 || names.Get(0).String() != defaultStorageName(1) {
		t.Fatal(list)
	}
	lock := Record{"GoodsId": "owned-item"}
	request(1004039, 20, lock, 4550)
	request(1004039, 21, lock, 4550)
	request(1005022, 22, Record{"GoodsId": "owned-item", "Location": 1, "Count": 2}, 4509)
	request(1004040, 23, lock, 4551)
	material := Record{"GoodsId": "owned-item", "Location": 2, "Count": 2}
	request(1005022, 2, material, 4154)
	bought := request(1005021, 3, nil, 4149)
	if getInt(bought, "ShinyStone") != 10000 || getInt(bought, "ShinyJade") != 50 {
		t.Fatal(bought)
	}
	request(1005021, 3, nil, 4149)
	renamed := request(1005007, 4, Record{"Location": 2, "NewName": "材料"}, 2002)
	if getInt(renamed, "ShinyJade") != 45 {
		t.Fatal(renamed)
	}
	put := request(1005022, 5, material, 4156)
	if getInt(put, "NowRingCapacity") != 1 || getInt(put, "NowBoxCapacity") != 3 {
		t.Fatal(put)
	}
	storageID := getString(put, "GoodsId")
	if storageID == "" || storageID == "owned-item" {
		t.Fatal("split stack must have its own instance ID")
	}
	request(1005022, 5, material, 4156)
	s = auctionTestServer(t, dir)
	peer, c = auctionTestRole(t, s, "owner", nil)
	list = request(1005005, 8, nil, 4120)
	if names := list.Get(field(list, "Name")).List(); names.Len() != 2 || names.Get(1).String() != "材料" {
		t.Fatal(list)
	}
	read := Message{Cmd: 1004002, Msid: 9, Data: []byte{8, 2}}
	s.handleGame(peer, read)
	box := c.receive(t, s, 1004002, 9, "SpaceGoodsResp_1004002")
	goods := box.Get(field(box, "GoodsInfos")).List()
	if goods.Len() != 1 || getInt(box, "NowCapacity") != 3 || getInt(box, "MaxCapacity") != 200 ||
		getInt(goods.Get(0).Message(), "Count") != 2 || getString(goods.Get(0).Message(), "GoodsId") != storageID {
		t.Fatal(box)
	}
	get := request(1005023, 10, Record{"Location": 2, "GoodsId": storageID, "Count": 2}, 4159)
	if getString(get, "GoodsId") != "owned-item" || getInt(get, "NowBoxCapacity") != 0 {
		t.Fatal(get)
	}
	role := s.roles.Get("owner")
	stored, _ := storageBox(role, 2)
	if len(goodsList(stored)) != 0 || transferStackCount(locateGoods(role, "owned-item")) != 4 ||
		roleInt(role, "ShinyStone") != 10000 {
		t.Fatalf("wrong state after transfer and retries: %v", role)
	}
}

func TestChamberStorageInNormalAndReadOnlyModes(t *testing.T) {
	s := auctionTestServer(t, t.TempDir())
	p, c := auctionTestRole(t, s, "owner", nil)
	if err := s.roles.Update("owner", func(r Record) error {
		r["ShinyStone"], r["ShinyJade"] = 10000, 5
		return nil
	}); err != nil {
		t.Fatal(err)
	}
	registry := s.preview
	buy := auctionTestMessage(t, s, 1005021, nil)
	rename := auctionTestMessage(t, s, 1005007, Record{"Location": 2, "NewName": "灵植"})
	put := auctionTestMessage(t, s, 1005022, Record{"Location": 2, "GoodsId": "absent", "Count": 1})
	get := auctionTestMessage(t, s, 1005023, Record{"Location": 2, "GoodsId": "absent", "Count": 1})
	request := func(msg Message, code int64) protoreflect.Message {
		t.Helper()
		s.handleGame(p, msg)
		out := c.receive(t, s, msg.Cmd, msg.Msid, registry[msg.Cmd].response)
		if got := getInt(out, "Code"); got != code {
			t.Fatalf("cmd %d: got %v, want %d", msg.Cmd, out, code)
		}
		return out
	}
	s.preview, s.previewWrites = nil, false
	request(buy, 4149)
	request(buy, 4149)
	request(rename, 2002)
	request(put, 4154)
	request(get, 4157)
	list := Message{Cmd: 1005005}
	boxes := request(list, 4120)
	if names := boxes.Get(field(boxes, "Name")).List(); names.Len() != 2 || names.Get(1).String() != "灵植" {
		t.Fatal(boxes)
	}
	s.preview = registry
	request(list, 4120)
	for _, msg := range []Message{buy, rename, put, get} {
		if getBool(request(msg, previewDeniedCode), "IsSuccess") {
			t.Fatalf("read-only preview accepted cmd %d", msg.Cmd)
		}
	}
	s.preview = nil
	replacement, _ := auctionTestRole(t, s, "owner", nil)
	if replacement == p || getBool(request(list, 4007), "IsSuccess") {
		t.Fatal("detached session still has access to storage")
	}
}

func TestStorageRejectsProtectedItemsAndCapacity(t *testing.T) {
	s := auctionTestServer(t, t.TempDir())
	items := []any{
		Record{"GoodsId": "locked", "Type": 19, "Count": 1, "IsLock": 1},
		Record{"GoodsId": "oversized", "Type": 19, "Count": 2, "Volume": 150},
		Record{"GoodsId": "stack", "Type": 19, "Count": 3},
	}
	if err := s.roles.Put("owner", Record{"Goods": items}); err != nil {
		t.Fatal(err)
	}
	peer, c := auctionTestRole(t, s, "owner", nil)
	for _, r := range []struct {
		id   string
		code int64
	}{{"locked", 4509}, {"oversized", 4155}, {"absent", 4154}} {
		msg := auctionTestMessage(t, s, 1005022, Record{"Location": 1, "GoodsId": r.id, "Count": 2})
		if r.id == "locked" {
			msg = auctionTestMessage(t, s, 1005022, Record{"Location": 1, "GoodsId": r.id, "Count": 1})
		}
		s.handleGame(peer, msg)
		if resp := auctionTestReply(t, s, c, msg, r.code); getBool(resp, "IsSuccess") {
			t.Fatal(resp)
		}
	}
	if got := s.roles.Get("owner"); len(goodsList(got)) != 3 || transferStackCount(locateGoods(got, "stack")) != 3 || len(storageBoxes(got)) != 0 {
		t.Fatal(got)
	}
}

func TestTrialExchangeSingleReserveAndReplay(t *testing.T) {
	dir := t.TempDir()
	s := auctionTestServer(t, dir)
	s.config.TrialExchangeStoneReserve = 1500
	peers := make([]*Peer, 2)
	conns := make([]*auctionTestConnection, 2)
	for i, id := range []string{"first", "second"} {
		peers[i], conns[i] = auctionTestRole(t, s, id, nil)
	}
	read := auctionTestMessage(t, s, 1002016, nil)
	s.handleGame(peers[0], read)
	if got := auctionTestReply(t, s, conns[0], read, 2000); getInt(got, "ExchangeShinyStoneSum") != 1500 || getInt(got, "ExchangeRatio") != 100 {
		t.Fatal(got)
	}
	bad := auctionTestMessage(t, s, 1002017, Record{"ShinyJade": 1})
	s.handleGame(peers[0], bad)
	auctionTestReply(t, s, conns[0], bad, 4361)
	var wg sync.WaitGroup
	for i := range peers {
		wg.Go(func() {
			msg := auctionTestMessage(t, s, 1002017, Record{"ShinyJade": 10})
			msg.Msid = 8
			s.handleGame(peers[i], msg)
		})
	}
	wg.Wait()
	winner := -1
	for i, connection := range conns {
		out := connection.receive(t, s, 1002017, 8, "ShinyJadeExchangeShinyStoneResp_1002017")
		switch getInt(out, "Code") {
		case 2000:
			if winner >= 0 || !getBool(out, "IsSuccess") || getInt(out, "ShinyStone") != 2000 || getInt(out, "ShinyJade") != 10 {
				t.Fatalf("oversold reserve or wrong balance: %v", out)
			}
			winner = i
		case 4363:
			if getBool(out, "IsSuccess") {
				t.Fatal(out)
			}
		default:
			t.Fatal(out)
		}
	}
	if winner < 0 {
		t.Fatal("no conversion was committed")
	}
	again := auctionTestMessage(t, s, 1002017, Record{"ShinyJade": 10})
	again.Msid = 8
	s.handleGame(peers[winner], again)
	auctionTestReply(t, s, conns[winner], again, 2000)
	s = auctionTestServer(t, dir)
	s.config.TrialExchangeStoneReserve = 99999
	winnerRole := s.roles.Get(peers[winner].currentRoleID())
	if roleInt(winnerRole, "ShinyStone") != 2000 || roleInt(winnerRole, "ShinyJade") != 10 ||
		s.trialExchangeReserve(s.roles.Get(trialExchangeID)) != 500 {
		t.Fatal("balances or reserve did not persist")
	}
}
