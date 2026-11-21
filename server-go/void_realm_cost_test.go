package main

import (
	"reflect"
	"sync"
	"testing"
	"time"
)

func TestVoidRealmPaidV2PublicationAndFreeStudy(t *testing.T) {
	dir := t.TempDir()
	st, err := OpenStore(dir, "roles")
	if err != nil {
		t.Fatal(err)
	}
	s := &Server{roles: st}
	at := time.Now()
	actor := Record{"Id": "writer", "Name": "作图人", "UserId": "A", "Level": 31, "Live4Type": 7, "Live4Level": 1, "ShinyStone": 0, "CurrentClever": 0}
	reader := Record{"Id": "reader", "Name": "读图人", "UserId": "B", "Level": 31, "Live2Type": 7, "Live2Level": 1, "ShinyStone": 0, "CurrentClever": 0}
	for key, rec := range map[string]Record{"writer": actor, "reader": reader} {
		if err := st.Put(key, rec); err != nil {
			t.Fatal(err)
		}
	}
	// 灵雀传书: one spring turn × two drive teeth / direct drive = two turns.
	req := voidRequest{Action: "simulate", Version: 2, HallID: "qiao", Grade: 1, Brief: 0,
		Route: 2, Adjust: 3, Steps: []int{0, 0, 0, 1}}
	run := func(owner string, q voidRequest) Record {
		t.Helper()
		r, err := s.voidAction(owner, q, at)
		if err != nil {
			t.Fatal(q, err)
		}
		return r
	}
	deny := func(owner string, q voidRequest) {
		t.Helper()
		if _, err := s.voidAction(owner, q, at); err == nil {
			t.Fatal("unexpected success", q)
		}
	}
	price := run("writer", req)
	if roleString(price, "quoteId") == "" || roleBool(price, "affordable") || roleInt(price, "value") < 1 || roleInt(price, "tuition") != 12 {
		t.Fatal(price)
	}
	if roleInt(st.Get("writer"), "ShinyStone") != 0 {
		t.Fatal("preview debited")
	}
	submit := req
	submit.Action, submit.RequestID, submit.Name, submit.QuoteID = "publish", newGameID(16), "长青枢", roleString(price, "quoteId")
	deny("writer", submit)
	mats := price["materials"].([]Record)
	if len(mats) == 0 {
		t.Fatal("expected physical mechanism material")
	}
	budget, insight := roleInt(price, "stoneCost")+roleInt(price, "tuition"), roleInt(price, "insightCost")
	// All but one stack are available. The locked lot must survive a failed registration.
	if err := st.Update("writer", func(r Record) error {
		r["ShinyStone"], r["CurrentClever"] = budget+123, insight+77
		var goods []any
		for i, m := range mats {
			goods = append(goods, Record{"GoodsId": newGameID(8), "TemplateId": m["templateId"], "Count": roleInt(m, "count"), "IsLock": i == 0})
		}
		r["Goods"] = goods
		return nil
	}); err != nil {
		t.Fatal(err)
	}
	deny("writer", submit)
	if roleInt(st.Get("writer"), "ShinyStone") != budget+123 || len(goodsList(st.Get("writer"))) != len(mats) {
		t.Fatal("failed settlement mutated holdings")
	}
	if err := st.Update("writer", func(r Record) error { goodsList(r)[0].(Record)["IsLock"] = false; return nil }); err != nil {
		t.Fatal(err)
	}
	priced := run("writer", req)
	if !roleBool(priced, "affordable") || roleString(priced, "quoteId") != submit.QuoteID {
		t.Fatal(priced)
	}
	stale := submit
	stale.QuoteID = "bad"
	deny("writer", stale)
	var wg sync.WaitGroup
	done := make(chan error, 8)
	for range 8 {
		wg.Add(1)
		go func() { defer wg.Done(); _, e := s.voidAction("writer", submit, at); done <- e }()
	}
	wg.Wait()
	close(done)
	for e := range done {
		if e != nil {
			t.Fatal(e)
		}
	}
	result := run("writer", submit)
	w := result["work"].(Record)
	wallet := result["wallet"].(Record)
	if roleInt(wallet, "stones") != 123 || roleInt(wallet, "insight") != 77 || len(goodsList(st.Get("writer"))) != 0 || roleInt(w, "version") != 2 || roleInt(w, "value") != roleInt(priced, "value") {
		t.Fatal(result, st.Get("writer"))
	}
	for _, m := range w["materials"].([]Record) {
		if m["owned"] != nil {
			t.Fatal("exposed inventory balance", m)
		}
	}
	id := roleString(w, "id")
	reopen, err := OpenStore(dir, "roles")
	if err != nil {
		t.Fatal(err)
	}
	s = &Server{roles: reopen}
	reading := run("reader", voidRequest{Action: "work", WorkID: id})["work"].(Record)
	steps, e := voidSavedSteps(reading)
	if e != nil || !reflect.DeepEqual(steps, req.Steps) || roleString(reading, "effectText") != roleString(priced, "effectText") {
		t.Fatal(reading, e)
	}
	for range 2 {
		study := run("reader", voidRequest{Action: "study", WorkID: id})
		if roleInt(study["work"].(Record), "readerCount") != 1 || roleInt(study["result"].(Record), "score") != roleInt(w, "score") {
			t.Fatal(study)
		}
	}
	if roleInt(reopen.Get("reader"), "CurrentClever") != 0 || roleInt(reopen.Get("reader"), "ShinyStone") != 0 {
		t.Fatal("reproduction billed the reader")
	}
	dup := submit
	dup.RequestID, dup.Name = newGameID(16), "苍灵枢"
	deny("writer", dup)
}
