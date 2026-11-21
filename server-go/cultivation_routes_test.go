package main

import (
	"math"
	"testing"
	"time"

	"google.golang.org/protobuf/proto"
	"google.golang.org/protobuf/reflect/protoreflect"
)

func TestCultivationProjectionAndSavedRoute(t *testing.T) {
	s := auctionTestServer(t, t.TempDir())
	s.config.CultivationSpeed, s.config.OfflineCapHours = 1, 8
	const level = 31
	s.game.levels[level] = gameLevel{upgradeLimit: 50000, toughTenacity: 40000, perception: 40000}
	peer, client := auctionTestRole(t, s, "cultivator", nil)
	err := s.roles.Update("cultivator", func(r Record) error {
		r["Level"], r["RunMask"], r["SuperAir"], r["Perception"], r["Physique"] = level, 4, 1000, 100, 100
		r["HasGold"] = true
		// Metal qi practice feeds the water body technique.
		r["GongFaNaQi"], r["GongFaDuanTi"] = "gf_3_6", "gf_2_3"
		return nil
	})
	if err != nil {
		t.Fatal(err)
	}
	call := func(cmd, location int) protoreflect.Message {
		t.Helper()
		var data []byte
		if location != 0 {
			req := s.pb("ChangeRunLocationReq_1001007")
			set(req, "NewLocation", location)
			data, err = proto.Marshal(req)
			if err != nil {
				t.Fatal(err)
			}
		}
		client.frames = nil
		s.handleGame(peer, Message{Cmd: cmd, Msid: 1, Data: data})
		return client.receive(t, s, cmd, 1, gameCultivationProtocols[cmd].resp)
	}
	room, body, dual := call(1005008, 0), call(1005008, 2), call(1005008, 4)
	check := func(value, want float64) {
		t.Helper()
		if math.Abs(value-want) > 1e-7 {
			t.Fatalf("got %v want %v", value, want)
		}
	}
	soloQi := getFloat(room, "SuperairCircle")
	check(getFloat(dual, "SuperairCircle"), soloQi*.70)
	check(getFloat(dual, "PhysiqueCircle"), getFloat(body, "PhysiqueCircle")*.70*1.20)
	r := s.roles.Get("cultivator")
	// Retain the single-root base factor but change which element matches.
	r["HasGold"], r["HasFire"] = false, true
	check(soloQi, 2*s.gameRate(r, 3)*1.20)
	if getInt(room, "Code") != 4128 || getInt(dual, "Code") != 2000 || roleInt(s.roles.Get("cultivator"), "RunMask") != 4 {
		t.Fatal("preview changed saved practice or legacy reply")
	}
	if !getBool(call(1001007, 4), "IsSuccess") || !getBool(call(1001007, 4), "IsSuccess") {
		t.Fatal("choose double route")
	}
	r = s.roles.Get("cultivator")
	if getInt(call(1005008, 0), "LocationType") != 4 {
		t.Fatal("saved route")
	}
	airRate, bodyRate := getFloat(dual, "SuperairCircle"), getFloat(dual, "PhysiqueCircle")*.5
	carry := [3]float64{}
	s.cultivatePeerAs(peer, "cultivator", &carry, 1)
	next := s.roles.Get("cultivator")
	if roleInt(next, "SuperAir")-roleInt(r, "SuperAir") != int64(airRate) ||
		roleInt(next, "Physique")-roleInt(r, "Physique") != int64(bodyRate) || roleInt(next, "Perception") != roleInt(r, "Perception") {
		t.Fatal("online tick disagrees with projection")
	}
	next["LastOnline"] = time.Now().Add(-time.Minute).UnixMilli()
	s.applyOffline(next)
	seconds := float64(roleInt(next, "OfflineTime"))
	if roleInt(next, "OfflineSuperAir") != int64(airRate*.4*seconds) || roleInt(next, "OfflinePhysique") != int64(bodyRate*.4*seconds) || roleInt(next, "OfflinePerception") != 0 {
		t.Fatal("offline applied wrong route", next)
	}
	capped := cloneRecord(next)
	capped["SuperAir"] = 50001
	capped["LastOnline"] = time.Now().Add(-time.Hour).UnixMilli()
	s.applyOffline(capped)
	if roleInt(capped, "SuperAir") != 50001 || roleInt(capped, "OfflineSuperAir") != 0 {
		t.Fatal("past-cap bonus cultivation lost")
	}
	if err = s.roles.Update("cultivator", func(r Record) error { r["Level"], r["RunMask"] = 10, 4; return nil }); err != nil {
		t.Fatal(err)
	}
	if getBool(call(1005008, 4), "IsSuccess") || getBool(call(1001007, 4), "IsSuccess") || roleInt(s.roles.Get("cultivator"), "RunMask") != 4 {
		t.Fatal("locked double practice")
	}
}

func TestLateOfflineReceiptKeepsAgeAndLargeAir(t *testing.T) {
	s := auctionTestServer(t, t.TempDir())
	s.dataDir = "../server/data"
	var err error
	s.config, err = readConfig(s.dataDir)
	if err != nil {
		t.Fatal(err)
	}
	if err = s.loadGameTables(); err != nil {
		t.Fatal(err)
	}
	peer, conn := auctionTestRole(t, s, "late-offline", nil)
	lvl := s.levelInfo(88)
	now := time.Now().UnixMilli()
	err = s.roles.Update("late-offline", func(r Record) error {
		r["Level"], r["GongFaNaQi"], r["RunMask"], r["HasGold"] = 88, "gf_3_16", 4, true
		r["CurrentAge"], r["MaxAge"], r["LastOnline"], r["SuperAir"] = 2000, gameLifespan(88), now-86400000, int64(lvl.upgradeLimit*.1)
		s.applyOfflineAt(r, now)
		return nil
	})
	if err != nil {
		t.Fatal(err)
	}
	s.handleGame(peer, Message{Cmd: 1002003, Msid: 2})
	receipt := conn.receive(t, s, 1002003, 2, "RoleOfflineResp_1002003")
	if !getBool(receipt, "IsFullTime") || getFloat(receipt, "Age") != 6 || getInt(receipt, "SuperAir") <= math.MaxInt32 || getInt(receipt, "OfflineTime") != 43200 {
		t.Fatal(receipt)
	}
}
