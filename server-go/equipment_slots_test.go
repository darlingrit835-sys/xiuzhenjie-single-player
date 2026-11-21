package main

import (
	"fmt"
	"reflect"
	"testing"

	"google.golang.org/protobuf/reflect/protoreflect"
)

func TestEquipmentSlotsAndDungeonStats(t *testing.T) {
	s := auctionTestServer(t, t.TempDir())
	s.config.BattleDurationSeconds = 120
	p, _ := auctionTestRole(t, s, "armory", nil)
	role := Record{"Level": 10, "Name": "试装", "Perception": 100, "Physique": 100, "Goods": []any{
		Record{"GoodsId": "sword", "Type": 15, "Level": 10, "Rank": 1},
		Record{"GoodsId": "old-robe", "Type": 16, "TemplateId": "eq_armor_qingbu"},
		Record{"GoodsId": "old-mail", "Type": 16, "TemplateId": "drop_16_3_10", "Rank": 3},
		Record{"GoodsId": "mirror", "Type": 16, "TemplateId": "eq_armor_xuantie", "Rank": 2, "IsLock": true},
		Record{"GoodsId": "boots", "Type": 29, "Rank": 2},
		Record{"GoodsId": "high-robe", "Type": 26, "Level": 20},
		Record{"GoodsId": "sigil", "Type": 21},
		Record{"GoodsId": "map", "Type": 32},
		Record{"GoodsId": "generic", "Type": 14},
		Record{"GoodsId": "offered", "Type": 15, "IsLock": true, friendOfferLockKey: "active"},
	}}
	if err := s.roles.Put("armory", role); err != nil {
		t.Fatal(err)
	}
	move := func(cmd int, id string, code int64) {
		t.Helper()
		route := battleEquipmentProtocols[cmd]
		out := spellTestCall(t, s, p, cmd, route.req, route.resp, Record{"EquipmentId": id})
		if getInt(out, "Code") != code || getBool(out, "IsSuccess") != (code == 2000) {
			t.Fatalf("equipment %d %s: %v", cmd, id, out)
		}
	}
	for _, item := range []string{"sword", "mirror", "boots", "old-robe", "old-mail"} {
		move(1009001, item, 2000)
	}
	for _, item := range []string{"sigil", "map", "generic"} {
		move(1009001, item, 4510)
	}
	move(1009001, "offered", 4509)
	move(1009001, "high-robe", 4105)
	bag := spellTestCall(t, s, p, 1004002, "SpaceGoodsReq_1004002", "SpaceGoodsResp_1004002", Record{"Location": 10})
	pending := false
	for _, item := range repeatedBagItems(bag, "GoodsInfos") {
		if getString(item, "GoodsId") == "offered" {
			pending = getBool(item, "IsLock") && getBool(item, "OfferPending")
		}
	}
	if !pending {
		t.Fatal("missing pending-transfer flag in inventory", bag)
	}
	open := spellTestCall(t, s, p, 1009005, "", "EnterEquipmentResp_1009005", nil)
	join := spellTestCall(t, s, p, 1010002, "RoleJoinDungeonReq_1010002", "RoleJoinDungeonResp_1010002", Record{"MapId": "1211W13"})
	for _, out := range []protoreflect.Message{open, join} {
		// 26 armor + 9 ward + 18 boots = 53; body and boots grant 8 more.
		if getInt(out, "CurrentPhysiqueDefense") != 161 || getInt(out, "CurrentPureBloodDefense") != 1090 || getFloat(out, "CurrentMoveSpeed") != 5.7 {
			t.Fatal("wearing four slots should change the panel and dungeon equally", out)
		}
	}
	if getFloat(open, "SuperAirPower") != 113 || open.Get(field(open, "DressedEquipments")).List().Len() != 4 {
		t.Fatal("missing offensive set bonus or incorrect slots", open)
	}
	locked := false
	for _, item := range repeatedBagItems(open, "DressedEquipments") {
		if getString(item, "GoodsId") == "mirror" {
			locked = getBool(item, "IsLock")
		}
	}
	if !locked || locateGoods(s.roles.Get("armory"), "old-robe") == nil {
		t.Fatal("older robe should return to the bag and mirror should stay locked")
	}
	if err := s.roles.Update("armory", func(r Record) error { r["Level"] = 11; return nil }); err != nil {
		t.Fatal(err)
	}
	move(1009001, "high-robe", 2000)
	if err := s.roles.Update("armory", func(r Record) error { r["Level"] = 1; return nil }); err != nil {
		t.Fatal(err)
	}
	move(1009002, "high-robe", 2000)
	move(1009002, "boots", 2000)
	move(1009002, "mirror", 2000)
	open = spellTestCall(t, s, p, 1009005, "", "EnterEquipmentResp_1009005", nil)
	if getInt(open, "CurrentPhysiqueDefense") != 100 || getInt(open, "CurrentPureBloodDefense") != 1000 || getFloat(open, "CurrentMoveSpeed") != 5 || getFloat(open, "SuperAirPower") != 110 {
		t.Fatal("unequip left defenses, movement or set effects active", open)
	}
}

func repeatedBagItems(msg protoreflect.Message, name string) []protoreflect.Message {
	rows := msg.Get(field(msg, name)).List()
	out := make([]protoreflect.Message, rows.Len())
	for i := range out {
		out[i] = rows.Get(i).Message()
	}
	return out
}

func TestEquipmentLegacyOverflowAndStackSwap(t *testing.T) {
	role := Record{"Goods": []any{Record{"GoodsId": "stack", "Type": 15, "Count": 2}}, "EquippedGoods": []any{
		Record{"GoodsId": "sword", "Type": 15}, Record{"GoodsId": "map", "Type": 32, "IsUsing": true},
		Record{"GoodsId": "robe", "Type": 26}, Record{"GoodsId": "old", "Type": 16, "TemplateId": "drop_16_1_1"},
	}}
	for i := 1; i < 100; i++ {
		role["Goods"] = append(goodsList(role), Record{"GoodsId": fmt.Sprint(i)})
	}
	err := restoreLegacyEquipment(role)
	migrated := cloneRecord(role)
	if err != nil || len(goodsList(role)) != 102 || inventoryFlag(locateGoods(role, "map"), "IsUsing") || locateGoods(role, "old") == nil {
		t.Fatal("previously worn possessions must return, including from full bags", err)
	}
	if err := restoreLegacyEquipment(role); err != nil || !reflect.DeepEqual(role, migrated) {
		t.Fatal("migration must be idempotent")
	}
	if err := moveBattleEquipment(role, "stack", true); err != errBattleBagFull || !reflect.DeepEqual(role, migrated) {
		t.Fatal("blocked stack swap changed possessions", err)
	}
	locateGoods(role, "stack")["Count"] = 1
	if err := moveBattleEquipment(role, "stack", true); err != nil || len(goodsList(role)) != 102 || locateGoods(role, "sword") == nil {
		t.Fatal("single-piece exchange consumes no bag space", err)
	}
	role["Goods"] = []any{Record{"GoodsId": "legacy", "Type": 15, "Count": 2}}
	if err := moveBattleEquipment(role, "legacy", true); err != nil || locateGoods(role, "legacy") != nil || len(goodsList(role)) != 2 {
		t.Fatal("equipped and remaining stack must have distinct IDs", err)
	}
}
