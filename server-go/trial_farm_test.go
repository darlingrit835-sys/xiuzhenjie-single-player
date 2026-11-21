package main

import (
	"errors"
	"strconv"
	"sync"
	"testing"
	"time"

	"google.golang.org/protobuf/reflect/protoreflect"
)

func TestFarmCycleAfterRestartAndConcurrentHarvest(t *testing.T) {
	dir := t.TempDir()
	s := auctionTestServer(t, dir)
	for _, id := range []string{"a", "b"} {
		if err := s.roles.Put(id, Record{"Id": id, "DongFuRank": 1, "CurrentVigor": 20, "Live3Type": 8, "Live3Level": 2, "Live3Value": 27}); err != nil {
			t.Fatal(err)
		}
	}
	start := time.Now().Add(-farmTilling - time.Second)
	request := func(owner string, cmd int, fields Record, when time.Time, want int) protoreflect.Message {
		t.Helper()
		route := s.preview[cmd]
		req := s.pb(route.request)
		putPB(req, fields)
		out := s.gameResponse(route.response, true, 2000)
		err := s.changeFarm(owner, cmd, req, out, when)
		if (want == 0 && err != nil) || (want != 0 && !errors.Is(err, previewReject(want))) {
			t.Fatalf("%d %v: got %v, expected %d", cmd, fields, err, want)
		}
		return out
	}
	read := func(owner string, cmd int, fields Record) protoreflect.Message {
		t.Helper()
		peer, c := auctionTestRole(t, s, owner, nil)
		msg := auctionTestMessage(t, s, cmd, fields)
		s.handleGame(peer, msg)
		out := auctionTestReply(t, s, c, msg, 2000)
		if !getBool(out, "IsSuccess") {
			t.Fatal(out)
		}
		return out
	}
	entries := func(out protoreflect.Message, key string) protoreflect.List { return out.Get(field(out, key)).List() }
	land := Record{"LandIndex": 2}
	soil := Record{"LandId": 2, "SoilId": previewSoilID}
	seed := Record{"LandId": 2, "SeedId": previewSeedID}
	crop := Record{"LandId": 2}
	request("a", 1013003, Record{"LandIndex": 5}, start, 5001)
	op := request("a", 1013003, land, start, 0)
	if getInt(op, "CurrentVigor") != 10 || getInt(op, "TimeRemaining") != farmTilling.Milliseconds() {
		t.Fatal(op)
	}
	request("a", 1013003, land, start, 4376)
	request("a", 1013006, soil, start.Add(time.Second), 4375)
	if entries(read("b", 1013001, nil), "Land").Len() != 0 || entries(read("b", 1013011, nil), "SeedInfo").Len() != 0 {
		t.Fatal("another role's farm leaked")
	}
	s = auctionTestServer(t, dir)
	plots := entries(read("a", 1013001, nil), "Land")
	if plots.Len() != 1 || getInt(plots.Get(0).Message(), "Id") != 2 || getInt(plots.Get(0).Message(), "Status") != 2 {
		t.Fatal(plots)
	}
	soilList := read("a", 1013004, crop)
	if soilList.Has(field(soilList, "CurrentSoil")) || entries(soilList, "SoilAll").Len() != 1 || entries(read("a", 1013011, nil), "SeedInfo").Len() != 1 {
		t.Fatal("starter kit or empty plot inconsistent")
	}
	now := time.Now()
	request("a", 1013013, seed, now, 4377)
	request("a", 1013006, Record{"LandId": 2, "SoilId": "other"}, now, 4008)
	request("a", 1013006, soil, now, 0)
	request("a", 1013006, soil, now, 0)
	request("a", 1013003, Record{"LandIndex": 1}, start, 0)
	request("a", 1013003, Record{"LandIndex": 3}, now, 4391)
	request("a", 1013006, Record{"LandId": 1, "SoilId": previewSoilID}, now, 4008)
	request("a", 1013013, Record{"LandId": 2, "SeedId": "other"}, now, 4371)
	planted := request("a", 1013013, seed, now, 0)
	if getInt(planted, "LandStatus") != 5 || getInt(planted, "TimeRemaining") != farmGrowth.Milliseconds() {
		t.Fatal(planted)
	}
	request("a", 1013018, crop, now, 4379)
	request("a", 1013006, soil, now, 4375)
	s = auctionTestServer(t, dir)
	savedPlots := entries(read("a", 1013001, nil), "Land")
	if savedPlots.Len() != 2 || getInt(savedPlots.Get(0).Message(), "Id") != 1 || getString(savedPlots.Get(1).Message(), "PlantId") != previewSeedID {
		t.Fatal(savedPlots)
	}
	type result struct {
		msg protoreflect.Message
		err error
	}
	replies := make(chan result, 2)
	var wg sync.WaitGroup
	for range 2 {
		wg.Add(1)
		go func() {
			defer wg.Done()
			req := s.pb("HarvestPlantReq_1013018")
			set(req, "LandId", 2)
			out := s.gameResponse("HarvestPlantResp_1013018", true, 2000)
			err := s.changeFarm("a", 1013018, req, out, now.Add(farmGrowth))
			replies <- result{out, err}
		}()
	}
	wg.Wait()
	close(replies)
	var itemID string
	success, denied := 0, 0
	for r := range replies {
		if errors.Is(r.err, previewReject(4379)) {
			denied++
			continue
		}
		if r.err != nil {
			t.Fatal(r.err)
		}
		success++
		rows := entries(r.msg, "PlantInfo")
		if rows.Len() != 1 || getInt(r.msg, "LiveLevel") != 2 || getInt(r.msg, "LiveProficiency") != 27 {
			t.Fatal(r.msg)
		}
		itemID = getString(rows.Get(0).Message(), "Id")
	}
	if success != 1 || denied != 1 || itemID == "" {
		t.Fatal("harvest count", success, denied)
	}
	s = auctionTestServer(t, dir)
	r := s.roles.Get("a")
	g := locateGoods(r, itemID)
	if roleString(g, "Name") != farmCropName || roleInt(g, "Count") != 1 || roleInt(r, "CurrentVigor") != 0 || entries(read("a", 1013001, nil), "Land").Len() != 1 {
		t.Fatal("bad reloaded harvest")
	}
	request("a", 1013006, Record{"LandId": 1, "SoilId": previewSoilID}, now.Add(farmGrowth), 0)
	history, err := s.roles.pendingHistory()
	if err != nil || len(history) != 6 {
		t.Fatalf("history count: %v %v", history, err)
	}
	for _, h := range history {
		if h.Actor.RoleID != "a" {
			t.Fatal("wrong actor", h)
		}
	}
}

func TestFarmFullBagDoesNotConsumeHarvest(t *testing.T) {
	s := auctionTestServer(t, t.TempDir())
	var goods []any
	for i := range 100 {
		goods = append(goods, Record{"GoodsId": strconv.Itoa(i), "Type": 15})
	}
	role := Record{"DongFuRank": 1, "Goods": goods, farmKey: Record{"Plots": Record{"1": Record{"SeedId": previewSeedID, "HarvestAt": 1, "SoilId": previewSoilID}}}}
	if err := s.roles.Put("r", role); err != nil {
		t.Fatal(err)
	}
	peer, c := auctionTestRole(t, s, "r", nil)
	req := auctionTestMessage(t, s, 1013018, Record{"LandId": 1})
	s.handleGame(peer, req)
	out := auctionTestReply(t, s, c, req, 4527)
	if getBool(out, "IsSuccess") || farmMap(farmMap(farmMap(s.roles.Get("r"), farmKey), "Plots"), "1") == nil {
		t.Fatal("full bag consumed plant")
	}
	if err := s.roles.Update("r", func(r Record) error { r["Goods"] = goodsList(r)[1:]; return nil }); err != nil {
		t.Fatal(err)
	}
	s.handleGame(peer, req)
	out = auctionTestReply(t, s, c, req, 4380)
	if !getBool(out, "IsSuccess") {
		t.Fatal(out)
	}
}
