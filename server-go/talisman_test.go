package main

import (
	"encoding/json"
	"fmt"
	"strings"
	"testing"
	"time"
)

func sigilCall(t *testing.T, s *Server, peer *Peer, req sigilRequest, wantOK bool) Record {
	t.Helper()
	payload, err := json.Marshal(req)
	if err != nil {
		t.Fatal(err)
	}
	s.handleGame(peer, Message{Cmd: talismanCommand, Msid: 11, Data: offerString(nil, 3, string(payload))})
	out := peer.conn.(*auctionTestConnection).receive(t, s, talismanCommand, 11, "ActionBoolResp_1001001")
	var body Record
	if json.Unmarshal([]byte(offerToken(out.GetUnknown())), &body) != nil || getBool(out, "IsSuccess") != wantOK {
		t.Fatalf("%s: %v %v", req.Action, out, body)
	}
	return body
}
func TestTalismanWireDrawCopyUseAndRestart(t *testing.T) {
	dir := t.TempDir()
	s, p := workshopTestServer(t, dir)
	seq := 0
	req := func(act string) sigilRequest {
		seq++
		return sigilRequest{Action: act, RequestID: fmt.Sprintf("%032x", seq), Effect: "field", Seal: "strong"}
	}
	catalog := sigilCall(t, s, p, req("catalog"), true)
	if roleInt(catalog, "supplyPrice") != 0 || roleInt(catalog, "brushTier") != 1 || len(battleRecords(catalog["effects"])) != 4 {
		t.Fatal(catalog)
	}
	supply := req("supply")
	sigilCall(t, s, p, supply, true)
	sigilCall(t, s, p, supply, true)
	sigilCall(t, s, p, req("supply"), false)
	for _, id := range []string{"paper", "ink", "ink"} {
		r := req("make")
		r.Recipe = id
		sigilCall(t, s, p, r, true)
	}
	sigilCall(t, s, p, req("copy"), false)
	c, _ := sigilSelect("field", "strong")
	draw := req("draw")
	draw.Strokes = c.strokes()
	draw.Strokes[1].Points = []sigilPoint{{75, 95}}
	sigilCall(t, s, p, draw, false)
	if got := forgeTestCount(s.roles.Get("artist"), sigilInk); got != 4 {
		t.Fatal(got)
	}
	draw.Strokes = c.strokes()
	first := sigilCall(t, s, p, draw, true)
	plan := farmMap(first, "plan")
	if !roleBool(plan, "learned") || roleInt(plan, "power") != 2 || len(battleRecords(plan["strokes"])) != 4 || roleInt(first, "rank") != 1 {
		t.Fatal(first)
	}
	s, p = workshopTestServer(t, dir)
	first = sigilCall(t, s, p, draw, true)
	if len(battleRecords(first["goods"])) != 1 || forgeTestCount(s.roles.Get("artist"), sigilInk) != 2 {
		t.Fatal(first)
	}
	forged := draw
	forged.Seal = "simple"
	sigilCall(t, s, p, forged, false)
	copied := sigilCall(t, s, p, req("copy"), true)
	owned := battleRecords(copied["goods"])
	if len(owned) != 2 || roleInt(copied, "training") != 15 {
		t.Fatal(copied)
	}
	use := req("use")
	use.GoodsID = roleString(owned[0], "id")
	if roleInt(sigilCall(t, s, p, use, true), "ward") != 2 || roleInt(sigilCall(t, s, p, use, true), "ward") != 2 {
		t.Fatal("double use")
	}
	use = req("use")
	use.GoodsID = roleString(owned[1], "id")
	if roleInt(sigilCall(t, s, p, use, true), "ward") != 3 {
		t.Fatal("field ward overflow")
	}
	// Strong mastery never grants the freehand recipe with a different ending.
	basic := req("copy")
	basic.Seal = "simple"
	sigilCall(t, s, p, basic, false)
	if len(battleRecords(sigilCall(t, s, p, req("catalog"), true)["goods"])) != 0 {
		t.Fatal("spent items still present")
	}
}

func TestHasteTalismanDrawingBagAndReservation(t *testing.T) {
	s, p := workshopTestServer(t, t.TempDir())
	const owner = "artist"
	if err := s.roles.Update(owner, func(r Record) error { r["Level"] = 1; return nil }); err != nil {
		t.Fatal(err)
	}
	seq := 0
	call := func(action, recipe, seal string) Record {
		seq++
		req := sigilRequest{Action: action, Recipe: recipe, Effect: "haste", Seal: seal, RequestID: fmt.Sprintf("%032x", seq)}
		if action == "draw" {
			c, _ := sigilSelect(req.Effect, req.Seal)
			req.Strokes = c.strokes()
		}
		return sigilCall(t, s, p, req, true)
	}
	call("supply", "", "simple")
	for _, step := range []string{"paper", "ink", "ink"} {
		call("make", step, "simple")
	}
	draw := call("draw", "", "simple")
	itemID := roleString(battleRecords(draw["goods"])[0], "id")
	use := func(id string, want bool) {
		out := smithingInventoryCall(t, s, p, 1004024, Record{"GoodsId": id})
		if getBool(out, "IsSuccess") != want {
			t.Fatal(id, out)
		}
	}
	use(itemID, true)
	if got := preparedDungeonHaste(s.roles.Get(owner)); got != 12 || locateGoods(s.roles.Get(owner), itemID) != nil {
		t.Fatal("ordinary written talisman did not reserve one expedition", got)
	}
	var legacy string
	if err := s.roles.Update(owner, func(r Record) error {
		var err error
		legacy, err = receiveInventoryGoods(r, Record{"Type": 21, "TemplateId": "preview-sign", "Name": "试用·轻身符"}, 1)
		return err
	}); err != nil {
		t.Fatal(err)
	}
	use(legacy, false)
	if locateGoods(s.roles.Get(owner), legacy) == nil {
		t.Fatal("equal reserve consumed the old trial talisman")
	}
	strong := call("draw", "", "strong")
	for _, item := range battleRecords(strong["goods"]) {
		if roleString(item, "templateId") == "sigil.haste.strong" {
			use(roleString(item, "id"), true)
		}
	}
	if got := roleInt(call("catalog", "", "simple"), "battleHaste"); got != 18 {
		t.Fatal("stronger talisman did not replace the reservation", got)
	}
	if err := s.roles.Update(owner, func(r Record) error {
		if consumeDungeonHaste(r) != 18 || consumeDungeonHaste(r) != 0 {
			t.Error("reservation was consumed twice")
		}
		return nil
	}); err != nil {
		t.Fatal(err)
	}
	use(legacy, true)
	if preparedDungeonHaste(s.roles.Get(owner)) != 12 {
		t.Fatal("old trial talisman did not reserve its advertised speed")
	}
}

func TestTalismanBrushRealmGardenMaterialsAndEffects(t *testing.T) {
	r := Record{"Level": 21, "Live1Type": 3, "Live1Level": 3, "Live1Value": 23, "ShinyStone": 90}
	c, _ := sigilSelect("ward", "strong")
	for _, v := range []sigilUnit{{sigilReed, 3}, {sigilLotus, 1}, {sigilDew, 1}} {
		// Garden harvests and trade offers may carry different metadata for the same template.
		if _, err := receiveInventoryGoods(r, Record{"TemplateId": v.id, "Type": 12, "MapName": "灵田"}, int64(v.n)); err != nil {
			t.Fatal(err)
		}
	}
	for _, id := range []string{"paper", "lotusInk", "brushKit", "brush"} {
		if _, err := sigilMutate(r, c, sigilRequest{Action: "make", Recipe: id}); err != nil {
			t.Fatal(err)
		}
	}
	if !sigilHasBrush(r) || roleInt(r, "ShinyStone") != 0 || sigilTolerance(r, true) != 17 || c.power(r, true) != 60 || c.power(r, false) != 53 {
		t.Fatal(r, c.power(r, true), c.power(r, false))
	}
	if _, err := sigilMutate(r, c, sigilRequest{Action: "draw", Strokes: c.strokes()}); err != nil {
		t.Fatal(err)
	}
	if roleInt(r, "Live1Value") != 81 || roleInt(r, "Live1Level") != 3 {
		t.Fatal("profession progress not written back")
	}
	war := Record{"TemplateId": c.key(), "Type": 21, "SigilPower": 60, "GoodsId": "ward"}
	makeItem := func(g Record) Record {
		id, err := receiveInventoryGoods(r, g, 1)
		if err != nil {
			t.Fatal(err)
		}
		return locateGoods(r, id)
	}
	if _, err := useInscribedTalisman(r, makeItem(war)); err != nil || roleInt(r, "AlchemyWard") != artisanWard(r, 60, 1250) {
		t.Fatal(err)
	}
	fresh := makeItem(war)
	if _, err := useInscribedTalisman(r, fresh); err == nil || locateGoods(r, roleString(fresh, "GoodsId")) == nil {
		t.Fatal("useless ward consumed")
	}
	c, _ = sigilSelect("cleanse", "simple")
	clean := makeItem(Record{"TemplateId": c.key(), "Type": 21, "SigilPower": 15})
	if _, err := useInscribedTalisman(r, clean); err == nil {
		t.Fatal("no poison")
	}
	r["CurrentPoisonVal"] = 18
	if _, err := useInscribedTalisman(r, clean); err != nil || roleInt(r, "CurrentPoisonVal") != 3 {
		t.Fatal(err)
	}
	field, _ := sigilSelect("field", "strong")
	if field.power(r, true) != 3 {
		t.Fatal("brush and rank did not affect ward")
	}
}
func TestTalismanRejectsFullBagAndLockedMaterials(t *testing.T) {
	s, p := workshopTestServer(t, t.TempDir())
	err := s.roles.Update("artist", func(r Record) error {
		r["Goods"] = []any{}
		if err := sigilReceive(r, []sigilUnit{{sigilPaper, 3}, {sigilInk, 3}}); err != nil {
			return err
		}
		for i := len(goodsList(r)); i < ringCapacity(r); i++ {
			r["Goods"] = append(goodsList(r), Record{"GoodsId": fmt.Sprintf("full%d", i), "TemplateId": fmt.Sprintf("f%d", i), "Type": 15})
		}
		return nil
	})
	if err != nil {
		t.Fatal(err)
	}
	c, _ := sigilSelect("ward", "simple")
	req := sigilRequest{Action: "draw", RequestID: strings.Repeat("f", 32), Strokes: c.strokes()}
	sigilCall(t, s, p, req, false)
	r := s.roles.Get("artist")
	if forgeTestCount(r, sigilPaper) != 3 || roleBool(farmMap(r, "TalismanPatterns"), c.key()) {
		t.Fatal("full bag lost inputs or unlocked recipe")
	}
	if err := s.roles.Update("artist", func(r Record) error { removeGoods(r, "full2"); goodsList(r)[0].(Record)["IsLock"] = true; return nil }); err != nil {
		t.Fatal(err)
	}
	sigilCall(t, s, p, req, false)
	if forgeTestCount(s.roles.Get("artist"), sigilInk) != 3 {
		t.Fatal("failed recipe spent ink")
	}
}

func TestTalismanFromGardenHarvestBlocksGardenVisitor(t *testing.T) {
	s, _ := workshopTestServer(t, t.TempDir())
	if err := s.roles.Put("guest", Record{"Name": "访客"}); err != nil {
		t.Fatal(err)
	}
	now := time.Unix(100000, 0)
	garden := func(actor string, req gardenRequest) Record {
		t.Helper()
		req.RequestID = newGameID(16)
		out, err := s.gardenAction(actor, req, now)
		if err != nil {
			t.Fatal(req, err)
		}
		return out
	}
	plant := func(i int, seed, soil string) string {
		out := garden("artist", gardenRequest{Action: "plant", Plot: i, Seed: seed, Soil: soil})
		return roleString(out["plots"].([]Record)[i-1], "cropId")
	}
	reed := plant(1, "reed", "sand")
	herb := plant(2, "dew", "loam")
	spare := plant(3, "dew", "loam")
	now = now.Add(5 * time.Minute)
	garden("artist", gardenRequest{Action: "harvest", Plot: 1, CropID: reed})
	garden("artist", gardenRequest{Action: "harvest", Plot: 2, CropID: herb})
	order := func(req sigilRequest) Record {
		t.Helper()
		req.RequestID = newGameID(16)
		req.Effect = "field"
		out, err := s.sigilAction("artist", req)
		if err != nil {
			t.Fatal(req, err)
		}
		return out
	}
	for _, recipe := range []string{"dew", "ink", "paper"} {
		order(sigilRequest{Action: "make", Recipe: recipe})
	}
	field, _ := sigilSelect("field", "simple")
	out := order(sigilRequest{Action: "draw", Strokes: field.strokes()})
	goods := out["goods"].([]Record)
	order(sigilRequest{Action: "use", GoodsID: roleString(goods[0], "id")})
	garden("guest", gardenRequest{Action: "steal", Target: "artist", Plot: 3, CropID: spare})
	role := s.roles.Get("artist")
	if roleInt(role, "TalismanFieldWard") != 0 || roleInt(farmMap(gardenPlots(role), "3"), "Picked") != 0 || forgeTestCount(s.roles.Get("guest"), sigilHerb) != 0 {
		t.Fatal("the run did not spend the grown ward to protect its crop")
	}
}
