package main

import (
	"encoding/json"
	"math"
	"reflect"
	"testing"
)

func TestCaveMatricesPaidUpgradesAndAllThreeRates(t *testing.T) {
	dir := t.TempDir()
	s := auctionTestServer(t, dir)
	s.dataDir = "../server/data"
	s.config.CultivationSpeed = 1
	if err := s.loadGameTables(); err != nil {
		t.Fatal(err)
	}
	p, _ := auctionTestRole(t, s, "owner", nil)
	call := func(req caveMatrixRequest, want int) Record {
		t.Helper()
		b, err := json.Marshal(req)
		if err != nil {
			t.Fatal(err)
		}
		var result Record
		if err = json.Unmarshal(auctionJSONCommandReply(t, s, p, caveMatrixCommand, b, want), &result); err != nil {
			t.Fatal(err)
		}
		return result
	}
	old := s.roles.Get("owner")
	q := call(caveMatrixRequest{Kind: "gather"}, 2000)
	if roleBool(q, "canBuy") || roleInt(q, "price") != 240 {
		t.Fatal(q)
	}
	call(caveMatrixRequest{"gather", 1, 240}, 4008)
	if !reflect.DeepEqual(old, s.roles.Get("owner")) {
		t.Fatal("locked cave or quote mutated role")
	}
	if err := s.roles.Update("owner", func(r Record) error { r["DongFuRank"], r["HasGold"], r["ShinyStone"] = 4, true, 239; return nil }); err != nil {
		t.Fatal(err)
	}
	call(caveMatrixRequest{"gather", 1, 240}, 4008)
	call(caveMatrixRequest{"gather", 1, 1}, 4008)
	call(caveMatrixRequest{"gather", 4, 6500}, 4008)
	if roleInt(s.roles.Get("owner"), "ShinyStone") != 239 {
		t.Fatal("failed orders charged")
	}
	if err := s.roles.Update("owner", func(r Record) error { r["ShinyStone"] = 30000; return nil }); err != nil {
		t.Fatal(err)
	}
	source := s.roles.Get("owner")
	for idx, kind := range []string{"gather", "control"} {
		for target := 1; target <= 4; target++ {
			price := matrixStonePrices[idx][target]
			q := call(caveMatrixRequest{Kind: kind}, 2000)
			if !roleBool(q, "canBuy") || roleInt(q, "target") != int64(target) || roleInt(q, "price") != price {
				t.Fatal(q)
			}
			before := roleInt(s.roles.Get("owner"), "ShinyStone")
			req := caveMatrixRequest{kind, target, price}
			call(req, 2000)
			call(req, 4008)
			if roleInt(s.roles.Get("owner"), "ShinyStone") != before-price {
				t.Fatal("duplicate purchase or price drift")
			}
			state := s.roles.Get("owner")
			expected := (1 + .1*float64(roleInt(state, "JuLingRank"))) * (1 + .1*float64(roleInt(state, "KongLingRank")))
			for route := 1; route <= 3; route++ {
				base := s.gameRate(source, route)
				if !(base > 0) || math.Abs(s.gameRate(state, route)/base-expected) > 1e-8 {
					t.Fatalf("route %d upgrade factor %v, expected %v", route, s.gameRate(state, route)/base, expected)
				}
			}
		}
		if roleBool(call(caveMatrixRequest{Kind: kind}, 2000), "canBuy") {
			t.Fatal("above maximum")
		}
	}
	if err := s.roles.Update("owner", func(r Record) error { r["HasWater"] = true; return nil }); err != nil {
		t.Fatal(err)
	}
	reloaded, err := OpenStore(dir, "roles")
	if err != nil {
		t.Fatal(err)
	}
	role := reloaded.Get("owner")
	if math.Abs(matrixBenefits(role)-1.68) > 1e-8 || roleInt(role, "ShinyStone") != 5050 {
		t.Fatalf("saved role, multiplier %v balance %v", matrixBenefits(role), roleInt(role, "ShinyStone"))
	}
}

func TestCaveMatrixConcurrentConfirmation(t *testing.T) {
	s := auctionTestServer(t, t.TempDir())
	p, _ := auctionTestRole(t, s, "owner", nil)
	if err := s.roles.Update("owner", func(r Record) error { r["DongFuRank"] = 1; return nil }); err != nil {
		t.Fatal(err)
	}
	out := make(chan error, 2)
	for i := 0; i < 2; i++ {
		go func() { _, err := s.caveMatrixOrder("owner", caveMatrixRequest{"gather", 1, 240}); out <- err }()
	}
	first, second := <-out, <-out
	r := s.roles.Get("owner")
	if (first == nil) == (second == nil) || roleInt(r, "ShinyStone") != 760 || roleInt(r, "JuLingRank") != 1 {
		t.Fatal("charged duplicate confirmation", first, second)
	}
	s.previewWrites = false
	b := []byte(`{"kind":"control","target":1,"price":360}`)
	auctionJSONCommandReply(t, s, p, caveMatrixCommand, b, 4008)
	if roleInt(s.roles.Get("owner"), "ShinyStone") != 760 {
		t.Fatal("read-only action charged")
	}
}
