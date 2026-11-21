package main

import (
	"errors"
	"sync"
	"testing"
	"time"

	"google.golang.org/protobuf/reflect/protoreflect"
)

func TestGuildBountyTransactions(t *testing.T) {
	dir := t.TempDir()
	s, err := openServer(Config{}, dir)
	if err != nil {
		t.Fatal(err)
	}
	s.guilds, err = OpenStore(dir, "guilds")
	if err != nil {
		t.Fatal(err)
	}
	registry, err := loadPreview(s.protocol)
	if err != nil {
		t.Fatal(err)
	}
	for _, id := range []string{"owner", "other", "outsider", "poor", "novice"} {
		role := Record{"Id": id, "ShinyStone": int64(1) << 33, "Name": id, "Level": 11}
		if id != "novice" {
			bountyAccount(role, 8, true)["Contribution"] = 100
		}
		if id == "poor" {
			role["ShinyStone"] = 0
		}
		if err := s.roles.Put(id, role); err != nil {
			t.Fatal(err)
		}
	}
	if err := s.guilds.Put(trialGuildWorld, Record{"Guilds": Record{"8": Record{"LeaderId": "owner", "Members": []string{"owner", "other", "poor", "novice"}, "Offices": Record{"3": "other"}}}}); err != nil {
		t.Fatal(err)
	}
	peerFor := func(id string) *Peer { peer := new(Peer); peer.onlineRole.Store(id); return peer }
	owner, other, outsider, poor, novice := peerFor("owner"), peerFor("other"), peerFor("outsider"), peerFor("poor"), peerFor("novice")
	call := func(peer *Peer, seq, cmd int, fields Record) (protoreflect.Message, error) {
		route := registry[cmd]
		var req protoreflect.Message
		if route.reqDesc != nil {
			req = s.pb(route.request)
			putPB(req, fields)
		}
		out := responseStatus(route.respDesc, true, 2000)
		peer.gameRequestID = seq
		_, err := route.live(s, peer, req, out)
		return out, err
	}
	run := func(peer *Peer, seq, cmd int, fields Record, want previewReject) protoreflect.Message {
		t.Helper()
		out, err := call(peer, seq, cmd, fields)
		if want == 0 && err != nil || want != 0 && !errors.Is(err, want) {
			t.Fatalf("%d: got %v expected %d", cmd, err, want)
		}
		return out
	}
	repeated := func(out protoreflect.Message, k string) protoreflect.List { return out.Get(field(out, k)).List() }
	run(outsider, 1, 1015038, nil, 4420)
	run(outsider, 2, 1015143, Record{"Name": "妖骨", "GoodsType": 10}, 4420)
	list := run(owner, 1, 1015038, nil, 0)
	if getInt(list, "CurrentContribution") != 100 || getInt(list.Get(field(list, "ZhangLaoInfo")).Message(), "Level") != 11 {
		t.Fatal("guild account/elder")
	}
	for kind := 9; kind <= 13; kind++ {
		out := run(owner, 2, 1015143, Record{"Name": battleMaterials[kind-8], "GoodsType": kind}, 0)
		first := repeated(out, "GoodsInfoList").Get(0).Message()
		if getInt(first, "RecyclePrice") != 15 || getString(first, "Id") != battleDropTemplateID(kind, 1, 1) {
			t.Fatal("world template mismatch")
		}
	}
	if repeated(run(owner, 3, 1015143, Record{"Name": "妖", "GoodsType": 10}, 0), "GoodsInfoList").Len() != 0 {
		t.Fatal("partial search matched")
	}
	publish := Record{"GoodsType": 10, "GoodsTemplateId": "drop_10_1_1", "Award": 6000} // 100 percent with two points from the elder.
	run(owner, 3, 1015059, Record{"GoodsType": 12, "GoodsTemplateId": "drop_10_1_1", "Award": 6000}, 5001)
	run(novice, 3, 1015059, publish, 4664)
	run(poor, 3, 1015059, publish, 4665)
	if getInt(run(novice, 4, 1015038, nil, 0), "CurrentContribution") != 0 {
		t.Fatal("unfunded account received contribution")
	}
	out := run(owner, 4, 1015059, publish, 0)
	if getInt(out, "CurrentStone") != int64(1)<<33-6000 {
		t.Fatal("wallet truncated or incorrect")
	}
	run(owner, 4, 1015059, publish, 0)
	list = run(owner, 5, 1015038, nil, 0)
	orders := repeated(list, "XuanShangInfo")
	if orders.Len() != 1 || getInt(list, "CurrentContribution") != 80 {
		t.Fatal("replay charged twice")
	}
	first := orders.Get(0).Message()
	id := getInt(first, "Id")
	if getInt(first, "Status") != 0 || getInt(first, "SuccessRate") != 100 || getInt(first, "ShengYuTime") < 3590000 {
		t.Fatal("invalid countdown or rate")
	}
	claim := Record{"Id": id}
	run(owner, 6, 1015041, claim, 4662)
	run(other, 6, 1015041, claim, 4663)
	low := Record{"GoodsType": 10, "GoodsTemplateId": "drop_10_1_1", "Award": 1}
	run(owner, 7, 1015059, low, 0)
	run(owner, 8, 1015059, low, 4666)
	second := repeated(run(owner, 9, 1015038, nil, 0), "XuanShangInfo").Get(1).Message()
	if getInt(second, "SuccessRate") != 2 {
		t.Fatal("elder rate not applied")
	}
	// Ring capacity must not consume the order. Reopening the store must not reroll its outcome.
	if err = s.roles.Update("owner", func(r Record) error {
		items := make([]any, 100)
		for i := range items {
			items[i] = Record{"GoodsId": "occupied", "Type": 15}
		}
		r["Goods"] = items
		o := bountyOrders(bountyAccount(r, 8, false))
		for i, v := range o {
			order := v.(Record)
			order["Due"] = time.Now().Add(-time.Minute).UnixMilli()
			if i == 1 {
				order["Found"] = false
			}
		}
		return nil
	}); err != nil {
		t.Fatal(err)
	}
	s.roles, err = OpenStore(dir, "roles")
	if err != nil {
		t.Fatal(err)
	}
	if getInt(repeated(run(owner, 10, 1015038, nil, 0), "XuanShangInfo").Get(0).Message(), "Status") != 1 {
		t.Fatal("due order after restart")
	}
	run(owner, 11, 1015041, claim, 4527)
	if err = s.roles.Update("owner", func(r Record) error { r["Goods"] = []any{}; return nil }); err != nil {
		t.Fatal(err)
	}
	// Independent authenticated connections race to settle the same ID.
	var wg sync.WaitGroup
	results := make(chan error, 2)
	for range 2 {
		wg.Add(1)
		go func() { defer wg.Done(); _, err := call(peerFor("owner"), 12, 1015041, claim); results <- err }()
	}
	wg.Wait()
	close(results)
	success := 0
	for err := range results {
		if err == nil {
			success++
		} else if !errors.Is(err, previewReject(4663)) {
			t.Fatal(err)
		}
	}
	if success != 1 {
		t.Fatalf("settled %d times", success)
	}
	run(owner, 13, 1015041, Record{"Id": getInt(second, "Id")}, 0)
	result := s.roles.Get("owner")
	items := goodsList(result)
	if len(items) != 1 || roleString(items[0].(Record), "TemplateId") != "drop_10_1_1" || roleInt(items[0].(Record), "Count") != 1 || len(bountyOrders(bountyAccount(result, 8, false))) != 0 {
		t.Fatal("inventory/claim history inconsistent")
	}
	if roleInt(bountyAccount(result, 8, false), "Contribution") != 60 || roleInt(result, "ShinyStone") != int64(1)<<33-6001 {
		t.Fatal("incorrect final charges")
	}
}
