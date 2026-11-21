package main

import (
	"encoding/json"
	"fmt"
	"sync"
	"testing"
	"time"
)

func TestGardenSeedSoilAndOldCrop(t *testing.T) {
	dir := t.TempDir()
	s := auctionTestServer(t, dir)
	at := time.Date(2026, 11, 20, 10, 0, 0, 0, time.UTC)
	legacy := Record{"SeedId": previewSeedID, "SoilId": previewSoilID, "HarvestAt": at.UnixMilli()}
	role := Record{"Name": "田主", "ShinyStone": 100, farmKey: Record{"Plots": Record{"3": legacy}}}
	if err := s.roles.Put("owner", role); err != nil {
		t.Fatal(err)
	}
	run := func(req gardenRequest, when time.Time) Record {
		t.Helper()
		if req.RequestID == "" {
			req.RequestID = newGameID(16)
		}
		v, err := s.gardenAction("owner", req, when)
		if err != nil {
			t.Fatal(req.Action, err)
		}
		return v
	}
	plot := func(v Record, i int) Record { return v["plots"].([]Record)[i-1] }
	old := plot(run(gardenRequest{Action: "state"}, at), 3)
	if roleInt(old, "yield") != 1 || roleString(old, "state") != "ripe" || roleBool(old, "canKeepSeed") {
		t.Fatal(old)
	}
	run(gardenRequest{Action: "harvest", Plot: 3, CropID: roleString(old, "cropId")}, at)
	if forgeTestCount(s.roles.Get("owner"), previewSeedID) != 1 || farmMap(gardenPlots(s.roles.Get("owner")), "3") != nil {
		t.Fatal("old crop not settled")
	}
	order := gardenRequest{Action: "buy", Seed: "lotus", RequestID: newGameID(16)}
	run(order, at)
	s = auctionTestServer(t, dir)
	run(order, at)
	if gardenStock(s.roles.Get("owner"), gardenVariety("lotus")) != 1 {
		t.Fatal("replay bought twice")
	}
	order.Seed = "reed"
	if _, err := s.gardenAction("owner", order, at); err == nil {
		t.Fatal("receipt reused for another crop")
	}
	p := plot(run(gardenRequest{Action: "plant", Seed: "lotus", Soil: "silt", Plot: 1}, at), 1)
	if roleInt(p, "remaining") != 720 || roleInt(p, "yield") != 6 || roleInt(s.roles.Get("owner"), "ShinyStone") != 32 {
		t.Fatal(p)
	}
	finish := gardenRequest{Action: "harvest", Plot: 1, CropID: roleString(p, "cropId"), KeepSeed: true, RequestID: newGameID(16)}
	if _, err := s.gardenAction("owner", finish, at); err == nil {
		t.Fatal("early harvest")
	}
	run(finish, at.Add(12*time.Minute))
	till := s.roles.Get("owner")
	if forgeTestCount(till, "mat.dream_lotus") != 4 || gardenStock(till, gardenVariety("lotus")) != 1 {
		t.Fatal("seed/crop split")
	}
	run(gardenRequest{Action: "plant", Seed: "lotus", Soil: "silt", Plot: 1}, at.Add(13*time.Minute))
	if roleInt(s.roles.Get("owner"), "ShinyStone") != 32 {
		t.Fatal("same soil billed twice")
	}
	finish.RequestID = newGameID(16)
	if _, err := s.gardenAction("owner", finish, at.Add(time.Hour)); err == nil {
		t.Fatal("stale harvest collected next crop")
	}
}

func TestGardenVisitorsWardsAndProtectedYield(t *testing.T) {
	s := auctionTestServer(t, t.TempDir())
	at := time.Now()
	for i, id := range []string{"host", "friend", "second", "third", "fourth", "fifth"} {
		r := Record{"Name": id, "ShinyStone": 50}
		if i == 0 {
			r[friendLinks] = Record{"friend": 0}
			r["TalismanFieldWard"] = 1
		}
		if i == 1 {
			r[friendLinks] = Record{"host": 0}
		}
		if err := s.roles.Put(id, r); err != nil {
			t.Fatal(err)
		}
	}
	run := func(id string, req gardenRequest) Record {
		t.Helper()
		if req.RequestID == "" {
			req.RequestID = newGameID(16)
		}
		v, err := s.gardenAction(id, req, at)
		if err != nil {
			t.Fatal(id, req.Action, err)
		}
		return v
	}
	sown := run("host", gardenRequest{Action: "plant", Plot: 1, Seed: "dew", Soil: "loam"})
	crop := roleString(sown["plots"].([]Record)[0], "cropId")
	run("host", gardenRequest{Action: "ward", Plot: 1, CropID: crop})
	at = at.Add(time.Minute)
	for _, c := range []struct {
		id, scope string
		n         int
	}{{"friend", "friends", 1}, {"friend", "strangers", 0}, {"second", "strangers", 1}} {
		res := run(c.id, gardenRequest{Action: "neighbors", Scope: c.scope})["neighbors"].([]Record)
		if len(res) != c.n || len(res) > 0 && (roleString(res[0], "id") != "host" || roleInt(res[0], "ripe") != 1) {
			t.Fatal(c, res)
		}
	}
	view := run("second", gardenRequest{Action: "visit", Target: "host"})
	if _, ok := view["logs"]; ok || view["stones"] != nil {
		t.Fatal("owner information leaked")
	}
	attempt := gardenRequest{Action: "steal", Target: "host", Plot: 1, CropID: crop, RequestID: newGameID(16)}
	run("friend", attempt)
	run("friend", attempt)
	actor, host := s.roles.Get("friend"), s.roles.Get("host")
	if gardenAttempts(actor, at) != 2 || roleInt(host, "TalismanFieldWard") != 1 || forgeTestCount(actor, "mat.ninglu_herb") != 0 {
		t.Fatal("ward or replay counted incorrectly")
	}
	if _, err := s.gardenAction("friend", gardenRequest{Action: "steal", Target: "host", Plot: 1, CropID: crop, RequestID: newGameID(16)}, at); err == nil {
		t.Fatal("visitor retried after being blocked")
	}
	run("second", attempt)
	if roleInt(s.roles.Get("host"), "TalismanFieldWard") != 0 || forgeTestCount(s.roles.Get("second"), "mat.ninglu_herb") != 0 {
		t.Fatal("field talisman did not block")
	}
	var wg sync.WaitGroup
	results := make(chan error, 3)
	for _, id := range []string{"third", "fourth", "fifth"} {
		wg.Add(1)
		go func() { defer wg.Done(); _, err := s.gardenAction(id, attempt, at); results <- err }()
	}
	wg.Wait()
	close(results)
	n := 0
	for err := range results {
		if err == nil {
			n++
		}
	}
	if n != 1 {
		t.Fatalf("%d visitors collected the crop", n)
	}
	run("host", gardenRequest{Action: "harvest", Plot: 1, CropID: crop})
	if forgeTestCount(s.roles.Get("host"), "mat.ninglu_herb") != 4 {
		t.Fatal("owner lost protected portion")
	}
}

func TestGardenBagRollbackAndDailyAttempts(t *testing.T) {
	s := auctionTestServer(t, t.TempDir())
	at := time.Date(2026, 11, 20, 15, 59, 0, 0, time.UTC)
	var full []any
	for i := range 100 {
		full = append(full, Record{"GoodsId": fmt.Sprint(i), "Type": 15})
	}
	r := Record{"Name": "田主", "Goods": full}
	crop := Record{"SeedId": "dew", "GardenYield": 5, "CropId": "current", "HarvestAt": at.UnixMilli()}
	gardenPutCrop(r, "1", crop)
	if err := s.roles.Put("host", r); err != nil {
		t.Fatal(err)
	}
	if err := s.roles.Put("visitor", Record{"Name": "客人", "Goods": full}); err != nil {
		t.Fatal(err)
	}
	steal := gardenRequest{Action: "steal", Target: "host", Plot: 1, CropID: "current", RequestID: newGameID(16)}
	harvest := gardenRequest{Action: "harvest", Plot: 1, CropID: "current", RequestID: newGameID(16), KeepSeed: true}
	if _, err := s.gardenAction("host", harvest, at); err == nil {
		t.Fatal("full harvest succeeded")
	}
	if _, err := s.gardenAction("visitor", steal, at); err == nil {
		t.Fatal("full theft succeeded")
	}
	if gardenAttempts(s.roles.Get("visitor"), at) != 3 || gardenStock(s.roles.Get("host"), gardenVariety("dew")) != 2 || gardenYield(farmMap(gardenPlots(s.roles.Get("host")), "1")) != 5 {
		t.Fatal("full bag changed crop or inventory")
	}
	if err := s.roles.Update("visitor", func(r Record) error {
		delete(r, "Goods")
		r["GardenVisitDay"], r["GardenVisitCount"] = gardenDay(at), 3
		return nil
	}); err != nil {
		t.Fatal(err)
	}
	if _, err := s.gardenAction("visitor", steal, at); err == nil {
		t.Fatal("fourth attempt permitted")
	}
	if _, err := s.gardenAction("visitor", steal, at.Add(time.Minute)); err != nil {
		t.Fatal("new local day", err)
	}
	if err := s.roles.Update("host", func(r Record) error { delete(r, "Goods"); return nil }); err != nil {
		t.Fatal(err)
	}
	if _, err := s.gardenAction("host", harvest, at.Add(time.Minute)); err != nil {
		t.Fatal(err)
	}
	if forgeTestCount(s.roles.Get("host"), "mat.ninglu_herb") != 2 {
		t.Fatal("wrong remainder")
	}
}

func TestGardenGameCommand(t *testing.T) {
	s := auctionTestServer(t, t.TempDir())
	player, _ := auctionTestRole(t, s, "grower", nil)
	payload := auctionJSONCommandReply(t, s, player, gardenCommand, []byte(`{"action":"state"}`), 2000)
	var state Record
	if err := json.Unmarshal(payload, &state); err != nil || !roleBool(state, "own") || roleString(state, "owner") != "grower" || len(state["seeds"].([]any)) != 3 {
		t.Fatal("garden state command", err, state)
	}
	auctionJSONCommandReply(t, s, player, gardenCommand, []byte(`{"action":"plant","seed":"dew","soil":"loam","plot":1}`), 4008)
	auctionJSONCommandReply(t, s, player, gardenCommand, []byte(`{"action":"visit","target":"$history:outbox"}`), 4008)
	s.previewWrites = false
	auctionJSONCommandReply(t, s, player, gardenCommand, []byte(`{"action":"state"}`), 2000)
	auctionJSONCommandReply(t, s, player, gardenCommand, []byte(`{"action":"buy","seed":"dew","requestId":"1234567890abcdef1234567890abcdef"}`), 4008)
}
