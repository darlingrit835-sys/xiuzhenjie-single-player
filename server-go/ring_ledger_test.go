package main

import (
	"testing"

	"google.golang.org/protobuf/proto"
	"google.golang.org/protobuf/reflect/protoreflect"
)

func TestRingLedgerSaleAndExpansion(t *testing.T) {
	dir := t.TempDir()
	s := auctionTestServer(t, dir)
	owner := "ring-owner"
	if err := s.roles.Put(owner, Record{"PracticeGuideVal": 1, "ShinyJade": 250, "ShinyStone": 7, "EquippedGoods": []any{
		Record{"GoodsId": "dressed", "Type": 15, "TemplateId": "ugc.test", "BlankId": "wp.minglei_spear"},
		Record{"GoodsId": "old-robe", "Type": 16, "TemplateId": "eq_armor_qingbu"},
	}, "Goods": []any{
		Record{"GoodsId": "ore", "Count": 4, "Type": 13, "RecyclePrice": 3},
		Record{"GoodsId": "keepsake", "Count": 1, "Type": 6, "IsLock": true, "RecyclePrice": 900},
		Record{"GoodsId": "bound", "IsBinding": true, "BlankId": "wp.minglei_spear"},
		Record{"GoodsId": "equipped", "IsUsing": true},
		Record{"GoodsId": "offered", friendOfferLockKey: "pending"},
		Record{"GoodsId": "guide", "TemplateId": practiceStarterMethodID},
		Record{"GoodsId": "legacy", "Type": 19, "RecyclePrice": 2},
	}}); err != nil {
		t.Fatal(err)
	}
	peer, c := auctionTestRole(t, s, owner, nil)
	inventory := Message{Cmd: 1004002, Msid: 90, Data: []byte{8, 10}}
	s.handleGame(peer, inventory)
	snapshot := c.receive(t, s, inventory.Cmd, inventory.Msid, "SpaceGoodsResp_1004002")
	stock := snapshot.Get(field(snapshot, "GoodsInfos")).List()
	checkShape := func(id, blank string) bool {
		for i := 0; i < stock.Len(); i++ {
			row := stock.Get(i).Message()
			if getString(row, "GoodsId") == id {
				return getString(row, "BlankId") == blank
			}
		}
		return false
	}
	if !checkShape("bound", "wp.minglei_spear") || !checkShape("ore", "") {
		t.Fatal("lost forged shape or broke legacy rows", snapshot)
	}
	equipment := Message{Cmd: 1009005, Msid: 91}
	s.handleGame(peer, equipment)
	dressed := c.receive(t, s, equipment.Cmd, equipment.Msid, "EnterEquipmentResp_1009005")
	entries := dressed.Get(field(dressed, "DressedEquipments")).List()
	if entries.Len() != 2 ||
		getString(entries.Get(0).Message(), "BlankId") != "wp.minglei_spear" ||
		getString(entries.Get(0).Message(), "TemplateId") != "ugc.test" ||
		getString(entries.Get(1).Message(), "BlankId") != "" ||
		getString(entries.Get(1).Message(), "TemplateId") != "eq_armor_qingbu" {
		t.Fatal("equipped and stored items must use identical artwork identifiers", dressed)
	}
	sell := func(seq int, code int64, lines ...Record) protoreflect.Message {
		t.Helper()
		req := s.pb("GoodsSellReq_1004022")
		for _, line := range lines {
			putPB(addMessage(req, "GoodsInfos"), line)
		}
		payload, err := proto.Marshal(req)
		if err != nil {
			t.Fatal(err)
		}
		msg := Message{Cmd: 1004022, Msid: seq, Data: payload}
		s.handleGame(peer, msg)
		return auctionTestReply(t, s, c, msg, code)
	}
	ore := Record{"GoodsId": "ore", "Count": 2}
	sell(1, 4552, ore, Record{"GoodsId": "keepsake", "Count": 1})
	for i, id := range []string{"bound", "equipped", "offered"} {
		sell(10+i, 4552, ore, Record{"GoodsId": id, "Count": 1})
	}
	sell(13, 4074, ore, Record{"GoodsId": "guide", "Count": 1})
	sell(14, 4073, Record{"GoodsId": "ore", "Count": 5})
	sell(2, 4073, ore, ore)
	if got := s.roles.Get(owner); transferStackCount(locateGoods(got, "ore")) != 4 || roleInt(got, "ShinyStone") != 7 {
		t.Fatalf("failed sale touched inventory: %v", got)
	}
	legacy := Record{"GoodsId": "legacy", "Count": 1}
	result := sell(3, 4077, ore, legacy)
	if getInt(result, "SuccessCount") != 3 || getInt(result, "NowCapacity") != 6 || getInt(result, "AddShinyStone") != 8 || getInt(result, "ShinyStone") != 15 {
		t.Fatal(result)
	}
	sell(3, 4077, ore, legacy)
	sell(3, 4008, ore)
	expand := func(seq int, code int64) protoreflect.Message {
		msg := Message{Cmd: 1004025, Msid: seq}
		s.handleGame(peer, msg)
		return auctionTestReply(t, s, c, msg, code)
	}
	result = expand(4, 2002)
	if getInt(result, "MaxCapacity") != 110 || getInt(result, "ShinyJade") != 50 {
		t.Fatal(result)
	}
	expand(4, 2002)
	expand(5, 4298)
	s = auctionTestServer(t, dir)
	got := s.roles.Get(owner)
	view, ok := s.roles.inventoryRows(owner, 10)
	if !ok || view.capacity != 110 || view.used != 6 || transferStackCount(locateGoods(got, "ore")) != 2 || roleInt(got, "ShinyStone") != 15 || roleInt(got, "ShinyJade") != 50 {
		t.Fatal(got, view)
	}
}
