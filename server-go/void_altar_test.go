package main

import (
	"math"
	"strconv"
	"testing"
	"time"
)

func TestVoidAltarSettlementAndNativeScripture(t *testing.T) {
	dir := t.TempDir()
	s := auctionTestServer(t, dir)
	s.dataDir = "../server/data"
	owner := "altar-reader"
	method := s.methods().byID["gf_3_1"]
	role := Record{"Id": owner, "Level": 31, "RingCapacity": 12, "GongFaOwned": []string{method.id}}
	books := addGoods(role, "旧功法卷", 1, 13, 1, 1, "altar-test-book", 7, 1, 0)
	locked := addGoods(role, "锁定阵图", 3, 16, 1, 1, "locked-book", 1, 1, 0)
	locked["IsLock"] = true
	addGoods(role, "凝露草", 12, 1, 1, 2, "herb", 2, 1, 0)
	if err := s.roles.Put(owner, role); err != nil {
		t.Fatal(err)
	}
	call := func(q voidRequest) Record {
		t.Helper()
		out, err := s.voidAction(owner, q, time.Now())
		if err != nil {
			t.Fatal(err)
		}
		return out
	}
	reject := func(q voidRequest) {
		t.Helper()
		if _, err := s.voidAction(owner, q, time.Now()); err == nil {
			t.Fatal("accepted", q)
		}
	}
	pick := voidAltarPick{GoodsID: roleString(books, "GoodsId"), Count: 1}
	q := voidRequest{Action: "altarPreview", Grade: 1, AltarItems: []voidAltarPick{pick}}
	if len(call(voidRequest{Action: "altar"})["books"].([]Record)) != 1 {
		t.Fatal("wrong shelf")
	}
	reject(voidRequest{Action: "altarPreview", Grade: 2, AltarItems: q.AltarItems})
	reject(voidRequest{Action: "altarPreview", Grade: 1, AltarItems: []voidAltarPick{pick, pick}})
	q.QuoteID = roleString(call(q), "quoteId")
	q.RequestID = newGameID(16)
	q.Action = "altarOffer"
	if got := call(q); roleInt(got, "after") != 4 || roleInt(got, "copies") != 0 {
		t.Fatal(got)
	}
	reloaded := auctionTestServer(t, dir)
	reloaded.dataDir = s.dataDir
	s = reloaded
	if got := call(q); roleInt(got, "after") != 4 || roleInt(locateGoods(s.roles.Get(owner), pick.GoodsID), "Count") != 6 {
		t.Fatal("retry after restart", got)
	}
	q.RequestID = newGameID(16)
	reject(q)
	q.Action = "altarPreview"
	result := call(q)
	if roleInt(result, "copies") != 1 || roleInt(result, "before") != 4 {
		t.Fatal(result)
	}
	q.Action = "altarOffer"
	q.QuoteID = roleString(result, "quoteId")
	call(q)
	stored := s.roles.Get(owner)
	var scripture Record
	for _, e := range goodsList(stored) {
		g := e.(Record)
		if roleInt(g, "Type") == 39 {
			scripture = g
		}
	}
	if scripture == nil || !roleBool(scripture, "IsBinding") || roleInt(scripture, "Level") != 1 {
		t.Fatal(scripture)
	}
	reject(voidRequest{Action: "altarPreview", Grade: 1, AltarItems: []voidAltarPick{{GoodsID: roleString(scripture, "GoodsId"), Count: 1}}})
	out := s.gameResponse("UpgradeAchievementConversionRateResp_1001020", true, 2000)
	s.applyMethodScripture(owner, method.id, out)
	if !getBool(out, "IsSuccess") || getInt(out, "Code") != 4705 || getInt(out, "Rank") != int64(method.rank+1) || math.Abs(getFloat(out, "ConversionRate")-(method.baseCircle+.08)) > 1e-6 {
		t.Fatal(out)
	}
	stored = s.roles.Get(owner)
	if locateGoods(stored, roleString(scripture, "GoodsId")) != nil || altarMethodRank(stored, method) != method.rank+1 {
		t.Fatal("scripture not applied")
	}
	out = s.gameResponse("UpgradeAchievementConversionRateResp_1001020", true, 2000)
	s.applyMethodScripture(owner, method.id, out)
	if getInt(out, "Code") != 4703 {
		t.Fatal("missing book must not grant a second refinement", out)
	}
}

func TestVoidAltarRingFullRollsBack(t *testing.T) {
	s := auctionTestServer(t, t.TempDir())
	r := Record{"Id": "small", "Level": 21}
	var offered Record
	for i := 0; i < ringCapacity(r); i++ {
		g := addGoods(r, "测试"+strconv.Itoa(i), 1, 16, 1, 1, newGameID(16), 3, 1, 0)
		if i == 0 {
			offered = g
		}
	}
	s.roles.Put("small", r)
	q := voidRequest{Action: "altarPreview", Grade: 1, AltarItems: []voidAltarPick{{GoodsID: roleString(offered, "GoodsId"), Count: 2}}}
	if _, err := s.voidAction("small", q, time.Now()); err == nil {
		t.Fatal("quoted rewards requiring an extra cell")
	}
	q.AltarItems[0].Count = 3
	p, err := s.voidAction("small", q, time.Now())
	if err != nil {
		t.Fatal(err)
	}
	q.Action, q.QuoteID, q.RequestID = "altarOffer", roleString(p, "quoteId"), newGameID(16)
	if _, err = s.voidAction("small", q, time.Now()); err != nil {
		t.Fatal("full stack frees room for reward", err)
	}
}

func TestVoidAltarRechecksFreeStockAtSettlement(t *testing.T) {
	for _, marker := range []string{"IsLock", "IsUsing", "IsBinding", friendOfferLockKey, "Count"} {
		t.Run(marker, func(t *testing.T) {
			s := auctionTestServer(t, t.TempDir())
			r := Record{"Id": "owner", "Level": 21}
			g := addGoods(r, "阵图", 3, 8, 1, 1, "t3", 4, 1, 0)
			id := roleString(g, "GoodsId")
			if err := s.roles.Put("owner", r); err != nil {
				t.Fatal(err)
			}
			q := voidRequest{Action: "altarPreview", Grade: 1, AltarItems: []voidAltarPick{{GoodsID: id, Count: 2}}}
			quote, err := s.voidAction("owner", q, time.Now())
			if err != nil {
				t.Fatal(err)
			}
			if err = s.roles.Update("owner", func(role Record) error {
				g := locateGoods(role, id)
				switch marker {
				case friendOfferLockKey:
					g[marker] = "friend-order"
				case "Count":
					g[marker] = 0
				default:
					g[marker] = 1
				}
				return nil
			}); err != nil {
				t.Fatal(err)
			}
			out, err := s.voidAction("owner", voidRequest{Action: "altar"}, time.Now())
			if err != nil || len(out["books"].([]Record)) != 0 {
				t.Fatal("protected on shelf", out, err)
			}
			if _, err = s.voidAction("owner", q, time.Now()); err == nil {
				t.Fatal("fresh quote accepted protected book")
			}
			q.Action, q.QuoteID, q.RequestID = "altarOffer", roleString(quote, "quoteId"), newGameID(16)
			if _, err = s.voidAction("owner", q, time.Now()); err == nil {
				t.Fatal("old quote consumed protected book")
			}
			stored := s.roles.Get("owner")
			if _, ok := stored["VoidAltarIncense"]; ok {
				t.Fatal("failed offer credited incense")
			}
			if len(goodsList(stored)) != 1 {
				t.Fatal("changed protected inventory")
			}
		})
	}
}

func TestVoidAltarHistoricalMaterialIsNotABook(t *testing.T) {
	s := auctionTestServer(t, t.TempDir())
	r := Record{"Id": "owner", "Level": 21}
	templates := []string{"mat.ninglu_herb", "yun_tie_1", "made.frost_iron", "obj.ninglu_pill", "de_jing_1", "drop_13_2_1"}
	for _, template := range templates {
		addGoods(r, template, 18, 9, 1, 1, template, 2, 1, 0)
	}
	book := addGoods(r, "遗本防器图", 18, 1, 1, 1, "", 1, 1, 0)
	if err := s.roles.Put("owner", r); err != nil {
		t.Fatal(err)
	}
	now := time.Now()
	out, err := s.voidAction("owner", voidRequest{Action: "altar"}, now)
	if err != nil {
		t.Fatal(err)
	}
	shelf := out["books"].([]Record)
	if len(shelf) != 1 || shelf[0]["goodsId"] != book["GoodsId"] {
		t.Fatal(shelf)
	}
	q := voidRequest{Action: "altarPreview", Grade: 1, AltarItems: []voidAltarPick{{GoodsID: roleString(book, "GoodsId"), Count: 1}}}
	p, err := s.voidAction("owner", q, now)
	if err != nil {
		t.Fatal("legacy book without template", err)
	}
	if err = s.roles.Update("owner", func(r Record) error {
		locateGoods(r, q.AltarItems[0].GoodsID)["TemplateId"] = "mat.ninglu_herb"
		return nil
	}); err != nil {
		t.Fatal(err)
	}
	if _, err = s.voidAction("owner", q, now); err == nil {
		t.Fatal("quoted material in book slot")
	}
	q.Action, q.QuoteID, q.RequestID = "altarOffer", roleString(p, "quoteId"), newGameID(16)
	if _, err = s.voidAction("owner", q, now); err == nil {
		t.Fatal("spent material against prior book quote")
	}
	if got := s.roles.Get("owner"); locateGoods(got, q.AltarItems[0].GoodsID) == nil || got["VoidAltarIncense"] != nil {
		t.Fatal("failed settlement changed inventory")
	}
}
