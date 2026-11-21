package main

import (
	"encoding/json"
	"maps"
	"strings"
	"testing"

	"google.golang.org/protobuf/encoding/protowire"
	"google.golang.org/protobuf/proto"
	"google.golang.org/protobuf/reflect/protoreflect"
)

func smithingInventoryCall(t *testing.T, s *Server, p *Peer, cmd int, args Record) protoreflect.Message {
	t.Helper()
	names := gameInventoryProtocols[cmd]
	req := s.pb(names.req)
	putPB(req, args)
	data, err := proto.Marshal(req)
	if err != nil {
		t.Fatal(err)
	}
	s.handleGame(p, Message{Cmd: cmd, Msid: 8, Data: data})
	return p.conn.(*auctionTestConnection).receive(t, s, cmd, 8, names.resp)
}

func TestSmithingWeaponTemplateAcrossAuctionAndReload(t *testing.T) {
	dir := t.TempDir()
	s := auctionTestServer(t, dir)
	id := smithingTemplateID("seller", strings.Repeat("a", 32))
	template := Record{"TemplateId": id, "Name": "测试霜纹剑", "Type": 15, "Rank": 13, "Level": 1, "Stage": 1,
		"PolarityId": 1, "FiveElement": 2, "QiId": 3, "CreatedBy": "炼器人", "BaseAttack": 45, "BaseDefense": 12,
		"Bonuses": Record{"attack": 250, "defense": 130, "healing": 89, "max_hp": 52}}
	maps.Copy(template, buildSmithingEquipmentFields(template))
	if err := s.roles.Put(smithingStorePrefix+id, template); err != nil {
		t.Fatal(err)
	}
	goods := cloneRecord(template)
	goods["GoodsId"] = "private-sword"
	seller, _ := auctionTestRole(t, s, "seller", []any{goods})
	buyer, _ := auctionTestRole(t, s, "buyer", nil)
	assertDetail := func(p *Peer, key string) {
		t.Helper()
		response := smithingInventoryCall(t, s, p, 1004018, Record{"Id": key})
		tag, typ, n := protowire.ConsumeTag(response.GetUnknown())
		if !getBool(response, "IsSuccess") || tag != smithingEquipmentField || typ != protowire.BytesType ||
			getString(response, "CreatedBy") != "炼器人" || getInt(response, "ToughTenacity") != 12 {
			t.Fatalf("wrong weapon detail for %s: %v", key, response)
		}
		if templateID, _ := protowire.ConsumeString(response.GetUnknown()[n:]); templateID != id {
			t.Fatalf("wrong public template %q", templateID)
		}
	}
	assertBag := func(p *Peer) {
		t.Helper()
		res := smithingInventoryCall(t, s, p, 1004002, Record{"Location": 10})
		rows := res.Get(field(res, "GoodsInfos")).List()
		if rows.Len() != 1 || getString(rows.Get(0).Message(), "TemplateId") != id {
			t.Fatal("bag lost template", res)
		}
		assertDetail(p, getString(rows.Get(0).Message(), "GoodsId"))
	}
	assertBag(seller)
	if getBool(smithingInventoryCall(t, s, buyer, 1004018, Record{"Id": "private-sword"}), "IsSuccess") {
		t.Fatal("private instance exposed")
	}
	fixedAuctionCall(t, s, seller, 1006008, 10, Record{"GoodsId": "private-sword", "ShopType": 83, "Count": 1, "FixedPrice": 300}, 4168)
	lookup := func(search string, subtype int) protoreflect.List {
		t.Helper()
		page := fixedAuctionCall(t, s, buyer, 1006001, 11, Record{"ShopType": 83, "GoodsType": 14, "Type": subtype, "Name": search}, 4160)
		return page.Get(field(page, "Object")).List()
	}
	byID, byName := lookup(id, 15), lookup("霜纹", 0)
	if byID.Len() != 1 || byName.Len() != 1 || lookup(id[:12], 15).Len() != 0 || lookup(id, 26).Len() != 0 {
		t.Fatal("weapon filter or template search incorrect")
	}
	listing := getString(byID.Get(0).Message(), "Id")
	if !strings.HasPrefix(listing, fixedAuctionPrefix) || getString(byID.Get(0).Message(), "GoodsId") != id ||
		getString(byName.Get(0).Message(), "Id") != listing {
		t.Fatal("publication lost listing or template identity")
	}
	assertDetail(buyer, id)
	assertDetail(buyer, listing)
	fixedAuctionCall(t, s, buyer, 1006010, 12, Record{"AuctionId": id}, 4038)
	fixedAuctionCall(t, s, buyer, 1006010, 13, Record{"AuctionId": listing}, 4046)
	s = auctionTestServer(t, dir)
	buyer, _ = auctionTestRole(t, s, "buyer", nil)
	assertBag(buyer)
	if lookup(id, 15).Len() != 0 || s.inventoryDetailItem("buyer", listing, false) != nil {
		t.Fatal("settled listing remained accessible")
	}
	carried := goodsList(s.roles.Get("buyer"))[0].(Record)
	if roleString(carried, "CreatedBy") != "炼器人" || len(carried["Bonuses"].(Record)) != 4 ||
		roleInt(carried, "BaseAttack") != 45 || roleInt(s.roles.Get("seller"), "ShinyStone") != 1270 {
		t.Fatal("settlement or persistent instance data incorrect", carried)
	}
	s.smithing = &smithingWorkshop{text: smithingLocale{Polarities: map[string]smithingLabel{"1": {Name: "阴"}},
		QiTypes: map[string]smithingLabel{"3": {Name: "寒"}}}}
	req, _ := json.Marshal(forgeRequest{TemplateID: id})
	s.handleGame(buyer, Message{Cmd: smithingTemplateCmd, Msid: 14, Data: offerString(nil, 3, string(req))})
	reply := buyer.conn.(*auctionTestConnection).receive(t, s, smithingTemplateCmd, 14, "ActionBoolResp_1001001")
	var details Record
	if !getBool(reply, "IsSuccess") || json.Unmarshal([]byte(offerToken(reply.GetUnknown())), &details) != nil ||
		roleString(details, "templateId") != id || roleString(details, "polarityName") != "阴" ||
		roleString(details, "qiName") != "寒" || roleInt(details, "baseAttack") != 45 || roleInt(details, "baseDefense") != 12 ||
		len(strings.Split(roleString(details, "bonusText"), " · ")) != 4 {
		t.Fatal("template detail omitted creator attributes or affixes", reply, details)
	}
}
