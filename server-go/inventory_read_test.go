package main

import (
	"bytes"
	"crypto/md5"
	"encoding/binary"
	"fmt"
	"testing"

	"google.golang.org/protobuf/encoding/protowire"
	"google.golang.org/protobuf/reflect/protoreflect"
)

type inventoryTestConnection struct {
	previewSink
	frame []byte
}

func (c *inventoryTestConnection) Write(frame []byte) (int, error) {
	c.frame = frame
	return len(frame), nil
}

func inventoryTestServer(t testing.TB) (*Server, *Peer, *inventoryTestConnection) {
	t.Helper()
	p, err := loadProtocol()
	if err != nil {
		t.Fatal(err)
	}
	store, err := OpenStore(t.TempDir(), "roles")
	if err != nil {
		t.Fatal(err)
	}
	s := &Server{protocol: p, roles: store}
	c := new(inventoryTestConnection)
	peer := &Peer{conn: c}
	s.bindGameRole(peer, "owner")
	return s, peer, c
}

// A nil revision is a legacy request. A present zero-length bytes field opts in.
func inventoryReadMessage(cmd int, location uint64, version []byte) Message {
	req := protowire.AppendVarint(protowire.AppendTag(nil, 1, protowire.VarintType), location)
	if version != nil {
		req = protowire.AppendBytes(protowire.AppendTag(req, 2, protowire.BytesType), version)
	}
	return Message{Cmd: cmd, Msid: 17, Data: req}
}

func inventoryReadResponse(t testing.TB, s *Server, p *Peer, c *inventoryTestConnection, msg Message) protoreflect.Message {
	t.Helper()
	c.frame = nil
	s.handleGame(p, msg)
	length, offset := binary.Uvarint(c.frame)
	if offset <= 0 || length != uint64(len(c.frame)-offset) {
		t.Fatalf("invalid inventory frame (%d bytes)", len(c.frame))
	}
	envelope, err := s.decode("Msg", c.frame[offset:])
	if err != nil || getInt(envelope, "Cmd") != int64(msg.Cmd) || getInt(envelope, "Msid") != int64(msg.Msid) {
		t.Fatalf("invalid inventory envelope: %v %v", envelope, err)
	}
	payload, err := s.decode(gameInventoryProtocols[msg.Cmd].resp, envelope.Get(field(envelope, "Data")).Bytes())
	if err != nil {
		t.Fatal(err)
	}
	return payload
}

// Parse the opt-in extension as the upgraded client does. The checked-in PB
// descriptor remains the original one and treats fields 7 and 8 as unknown.
func inventoryReadExtension(t testing.TB, out protoreflect.Message) (version []byte, unchanged bool) {
	t.Helper()
	for raw := out.GetUnknown(); len(raw) > 0; {
		number, kind, n := protowire.ConsumeTag(raw)
		if n < 0 {
			t.Fatal("invalid response tag")
		}
		raw = raw[n:]
		switch {
		case number == 7 && kind == protowire.BytesType:
			var v []byte
			v, n = protowire.ConsumeBytes(raw)
			version = bytes.Clone(v)
		case number == 8 && kind == protowire.VarintType:
			var v uint64
			v, n = protowire.ConsumeVarint(raw)
			unchanged = v == 1
		default:
			n = protowire.ConsumeFieldValue(number, kind, raw)
		}
		if n < 0 {
			t.Fatal("invalid response field")
		}
		raw = raw[n:]
	}
	return
}

func TestInventoryReadsLegacyAndConditionalPB(t *testing.T) {
	s, p, c := inventoryTestServer(t)
	if err := s.roles.Put("owner", Record{"Goods": []any{
		Record{"GoodsId": "B", "Count": 3, "Name": "甲物", "Type": 15, "Level": 4, "Rank": 2, "FiveElement": 3, "IsUsing": true, "Volume": 1.5, "TemplateId": "template-b", "ConversionRate": 0.75, "RecyclePrice": 21},
		Record{"GoodsId": "A", "Count": 5, "Name": "乙物"},
	}}); err != nil {
		t.Fatal(err)
	}
	list := func(loc uint64, v []byte, want int) protoreflect.Message {
		t.Helper()
		out := inventoryReadResponse(t, s, p, c, inventoryReadMessage(1004002, loc, v))
		capacity := int64(100)
		if loc == 1 { // The first warehouse slot already belongs to the role.
			capacity = baseStorageCapacity
		}
		if !getBool(out, "IsSuccess") || getInt(out, "Code") != 2000 || getInt(out, "Location") != int64(loc) || getInt(out, "MaxCapacity") != capacity || out.Get(field(out, "GoodsInfos")).List().Len() != want {
			t.Fatalf("unexpected list at %d: %v", loc, out)
		}
		return out
	}
	checksum := func(loc uint64, want string) {
		t.Helper()
		out := inventoryReadResponse(t, s, p, c, inventoryReadMessage(1004004, loc, nil))
		if !getBool(out, "IsSuccess") || getInt(out, "Location") != int64(loc) || getString(out, "Md5") != want {
			t.Fatalf("legacy checksum at %d: %v, want %s", loc, out, want)
		}
	}
	// The legacy algorithm sorts IDs first and lowercases the concatenated ID/count pairs.
	bagMD5 := fmt.Sprintf("%x", md5.Sum([]byte("a5b3")))
	emptyMD5 := fmt.Sprintf("%x", md5.Sum(nil))
	checksum(10, bagMD5)
	legacy := list(10, nil, 2)
	if v, hit := inventoryReadExtension(t, legacy); v != nil || hit || getInt(legacy, "NowCapacity") != 2 {
		t.Fatal(legacy)
	}
	goods := legacy.Get(field(legacy, "GoodsInfos")).List().Get(0).Message()
	if getString(goods, "GoodsId") != "B" || getString(goods, "Name") != "甲物" || getInt(goods, "Count") != 3 ||
		!getBool(goods, "IsUsing") || goods.Get(field(goods, "Volume")).Float() != 1.5 || getInt(goods, "RecyclePrice") != 21 {
		t.Fatal(goods)
	}
	for _, loc := range []uint64{1, 2, 3, 4, 5} {
		checksum(loc, emptyMD5)
		if out := list(loc, nil, 0); getInt(out, "NowCapacity") != 0 {
			t.Fatal(out)
		}
	}
	checksum(0, bagMD5)
	first := list(10, []byte{}, 2)
	version, hit := inventoryReadExtension(t, first)
	if len(version) != 16 || hit {
		t.Fatal("first opted-in request did not return full inventory and a 16-byte version")
	}
	fullSize := len(c.frame)
	unchanged := list(10, version, 0)
	tinySize := len(c.frame)
	if v, hit := inventoryReadExtension(t, unchanged); !hit || !bytes.Equal(v, version) || getInt(unchanged, "NowCapacity") != 2 || tinySize >= fullSize {
		t.Fatalf("conditional response was not a smaller metadata frame: %v", unchanged)
	}
	otherLocation := list(1, version, 0)
	if v, hit := inventoryReadExtension(t, otherLocation); hit || len(v) != 16 || bytes.Equal(v, version) || getInt(otherLocation, "NowCapacity") != 0 {
		t.Fatal("empty storage reused the bag's revision")
	}
	// A request without opt-in must still get the entire bag on an already warm connection.
	list(10, nil, 2)
	if v, hit := inventoryReadExtension(t, list(10, []byte{1, 2}, 2)); v != nil || hit {
		t.Fatal("malformed revision opted in")
	}
	for _, change := range []struct {
		field string
		value any
	}{
		{"IsLock", true}, {"IsBinding", true}, {"IsUsing", false}, {"FiveElement", 1},
		{"Rank", 3}, {"Level", 5}, {"Type", 16}, {"Name", "已更新"},
		{"RecyclePrice", 22}, {"TemplateId", "template-c"}, {"ConversionRate", 0.5},
		{"Volume", 2.5}, {"Count", 4}, {"GoodsId", "C"},
	} {
		if err := s.roles.Update("owner", func(r Record) error { locateGoods(r, "B")[change.field] = change.value; return nil }); err != nil {
			t.Fatal(err)
		}
		out := list(10, version, 2)
		v, hit := inventoryReadExtension(t, out)
		g := out.Get(field(out, "GoodsInfos")).List().Get(0).Message()
		if hit || len(v) != 16 || bytes.Equal(v, version) {
			t.Fatalf("%s not refreshed", change.field)
		}
		if got := g.Get(field(g, change.field)); fmt.Sprint(got.Interface()) != fmt.Sprint(change.value) {
			t.Fatalf("wrong %s: %v, want %v", change.field, got, change.value)
		}
		switch change.field {
		case "Count":
			bagMD5 = fmt.Sprintf("%x", md5.Sum([]byte("a5b4")))
		case "GoodsId":
			bagMD5 = fmt.Sprintf("%x", md5.Sum([]byte("a5c4")))
		}
		checksum(10, bagMD5)
		version = v
	}
	if err := s.roles.Put("other", Record{"Goods": []any{Record{"GoodsId": "C", "Name": "另一角色", "Count": 4}, Record{"GoodsId": "A", "Count": 5}}}); err != nil {
		t.Fatal(err)
	}
	s.bindGameRole(p, "other")
	out := list(10, version, 2)
	if v, hit := inventoryReadExtension(t, out); hit || bytes.Equal(v, version) || getString(out.Get(field(out, "GoodsInfos")).List().Get(0).Message(), "Name") != "另一角色" {
		t.Fatal("switched role received previous role's cached inventory")
	}
	s.bindGameRole(p, "missing")
	missing := inventoryReadResponse(t, s, p, c, inventoryReadMessage(1004002, 10, version))
	if getBool(missing, "IsSuccess") || getInt(missing, "Code") != 4007 {
		t.Fatal(missing)
	}
}

func BenchmarkInventoryReads100(b *testing.B) {
	s, p, c := inventoryTestServer(b)
	goods := make([]any, 0, 100)
	for i := range 100 {
		goods = append(goods, Record{"GoodsId": fmt.Sprintf("item-%03d", i), "Name": fmt.Sprintf("测试物品%03d", i),
			"Type": 15, "Level": 4, "Rank": 2, "FiveElement": 1 + i%5, "Count": i + 1, "TemplateId": fmt.Sprintf("template-%02d", i%10),
			"IsLock": i%2 == 0, "IsUsing": i%3 == 0, "IsBinding": i%5 == 0, "Volume": 1.5, "ConversionRate": 0.75, "RecyclePrice": 21})
	}
	quests := make([]any, 0, 24)
	for i := range 24 {
		quests = append(quests, Record{"Id": i + 1, "Status": 1, "Progress": i, "Name": fmt.Sprintf("任务%d", i)})
	}
	if err := s.roles.Put("owner", Record{"Name": "基准角色", "Level": 52, "Goods": goods, "Tasks": quests,
		"EquippedGoods": []any{cloneRecord(goods[0].(Record))}, "ShinyStone": 10000, "ShinyJade": 100, "CurrentAge": 208, "CurrentPower": 9527}); err != nil {
		b.Fatal(err)
	}
	legacy := inventoryReadMessage(1004002, 10, nil)
	opted := inventoryReadMessage(1004002, 10, []byte{})
	out := inventoryReadResponse(b, s, p, c, opted)
	v, _ := inventoryReadExtension(b, out)
	if len(v) != 16 {
		b.Fatal("no version returned")
	}
	hit := inventoryReadMessage(1004002, 10, v)
	for _, path := range []string{"LegacyFullReencode", "NewFirstFull", "CachedLegacyFull", "ConditionalHit"} {
		b.Run(path, func(b *testing.B) {
			b.ReportAllocs()
			var operation func()
			switch path {
			case "LegacyFullReencode":
				// Mirror the pre-optimization 1004002 handler with the same PB codec and TCP envelope.
				operation = func() {
					req, err := s.decode("SpaceGoodsReq_1004002", legacy.Data)
					if err != nil {
						b.Fatal(err)
					}
					role := s.roles.Get(p.currentRoleID())
					reply := s.gameResponse("SpaceGoodsResp_1004002", true, 2000)
					firstRoot := firstCultivationRoot(role)
					for _, entry := range goodsList(role) {
						fillGameGoods(addMessage(reply, "GoodsInfos"), entry.(Record), firstRoot)
					}
					putPB(reply, Record{"Location": getInt(req, "Location"), "NowCapacity": len(goodsList(role)), "MaxCapacity": 100})
					s.sendGame(p, legacy, reply)
				}
			case "NewFirstFull":
				operation = func() { clear(p.inventory[:]); s.handleInventory(p, opted) }
			case "CachedLegacyFull":
				s.handleInventory(p, legacy)
				operation = func() { s.handleInventory(p, legacy) }
			case "ConditionalHit":
				s.handleInventory(p, opted)
				operation = func() { s.handleInventory(p, hit) }
			}
			for b.Loop() {
				operation()
			}
			b.ReportMetric(float64(len(c.frame)), "wire-B/op")
		})
	}
}
