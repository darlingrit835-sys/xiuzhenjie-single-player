package main

import (
	"math"
	"slices"
	"testing"

	"google.golang.org/protobuf/proto"
	"google.golang.org/protobuf/reflect/protoreflect"
)

func TestSystemMethodsStudyEquipAndRelog(t *testing.T) {
	s := auctionTestServer(t, t.TempDir())
	s.dataDir = "../server/data" // Only static tables are loaded here. The role store is temporary.
	if err := s.loadGameTables(); err != nil {
		t.Fatal(err)
	}
	s.config.CultivationSpeed = 1
	const roleID = "system-method-pupil"
	peer, client := auctionTestRole(t, s, roleID, nil)
	if err := s.roles.Update(roleID, func(r Record) error {
		r["Level"], r["PracticeGuideVal"] = 1, 100
		r["Wood"], r["HasFire"] = 80, true // Older saves can contain only the root weight.
		s.ensureGameRole(r)
		return nil
	}); err != nil {
		t.Fatal(err)
	}

	call := func(cmd int, values Record) protoreflect.Message {
		t.Helper()
		route, ok := gameCultivationProtocols[cmd]
		if !ok {
			route = gameInventoryProtocols[cmd]
		}
		var payload []byte
		if route.req != "" {
			req := s.pb(route.req)
			putPB(req, values)
			var err error
			payload, err = proto.Marshal(req)
			if err != nil {
				t.Fatal(err)
			}
		}
		client.frames = nil
		s.handleGame(peer, Message{Cmd: cmd, Msid: 3, Data: payload})
		return client.receive(t, s, cmd, 3, route.resp)
	}
	list := call(1001011, nil)
	entries := list.Get(field(list, "Object")).List()
	if entries.Len() != 48 || len(ownedGameMethods(s.roles.Get(roleID))) != 0 {
		t.Fatal("wrong catalogue or automatic grant", entries.Len())
	}
	for i := 0; i < entries.Len(); i++ {
		entry := entries.Get(i).Message()
		element := getInt(entry, "FiveElement")
		if element != 2 && element != 4 || getBool(entry, "IsUnderstand") {
			t.Fatal("off-root or pre-learned book", entry)
		}
	}
	study := func(id string) protoreflect.Message { return call(1001005, Record{"Id": id, "Type": 2}) }
	equip := func(d *gameMethod) protoreflect.Message {
		return call(1005011, Record{"AchievementId": d.id, "ScopeType": d.kind})
	}
	var starter, advanced *gameMethod
	for i := range s.methods().all {
		d := &s.methods().all[i]
		if d.system && d.element == 4 && d.kind == 3 && d.learningLevel == 1 && starter == nil {
			starter = d
		}
		if d.system && d.element == 2 && d.learningLevel == 31 && advanced == nil {
			advanced = d
		}
	}
	if starter == nil || advanced == nil {
		t.Fatal("missing expected level bands")
	}
	if out := study(advanced.id); getBool(out, "IsSuccess") || getInt(out, "Code") != 4057 {
		t.Fatal("level gate", out)
	}
	if out := study("sys_1_1_1"); getBool(out, "IsSuccess") || getInt(out, "Code") != studyRootMismatch {
		t.Fatal("off-root system method", out)
	}
	if out := study("gf_2_7"); getBool(out, "IsSuccess") || getInt(out, "Code") != studyBookRequired {
		t.Fatal("unread ordinary method", out)
	}
	if getBool(equip(starter), "IsSuccess") {
		t.Fatal("equipped unread method")
	}
	info := call(1011004, Record{"Id": starter.id})
	if getString(info, "CreatedBy") != "天道传承" || getInt(info, "StudyLevel") != 1 || int(getInt(info, "Level")) != starter.learningLevel || getFloat(info, "ConversionRate") != starter.baseCircle {
		t.Fatal("method details", info)
	}
	old := roleString(s.roles.Get(roleID), "GongFaNaQi")
	if !getBool(study(starter.id), "IsSuccess") || !getBool(study(starter.id), "IsSuccess") {
		t.Fatal("study or retry")
	}
	r := s.roles.Get(roleID)
	if len(ownedGameMethods(r)) != 1 || roleInt(r, "QuestGongFaCount") != 1 || roleString(r, "GongFaNaQi") != old {
		t.Fatal("study duplicated or equipped implicitly")
	}
	panel := call(1005009, Record{"AchievementType": 3})
	choices := panel.Get(field(panel, "Achievements")).List()
	found := false
	for i := 0; i < choices.Len(); i++ {
		if getString(choices.Get(i).Message(), "Id") == starter.id {
			found = true
			break
		}
	}
	if !found || !getBool(equip(starter), "IsSuccess") {
		t.Fatal("new entry missing from equipment list")
	}
	peer, client = auctionTestRole(t, s, roleID, nil)
	saved := s.roles.Get(roleID)
	s.ensureGameRole(saved)
	room := call(1005008, nil)
	if !slices.Contains(ownedGameMethods(saved), starter.id) || roleString(saved, "GongFaNaQi") != starter.id ||
		getString(room, "SuperairName") != starter.name || math.Abs(getFloat(room, "SuperairCircle")-s.gameRate(saved, 3)*2) > 1e-8 {
		t.Fatal("equipped method or cultivation gain lost on reconnect", room)
	}

	// An item from a legacy reward can advertise a different element from the actual technique.
	books := []any{
		Record{"GoodsId": "wood", "Type": 1, "Name": "不灭金身诀", "Count": 2},
		Record{"GoodsId": "gold", "Type": 1, "TemplateId": "gf_2_1", "FiveElement": 2, "Count": 2},
	}
	if err := s.roles.Update(roleID, func(r Record) error { r["Goods"] = books; return nil }); err != nil {
		t.Fatal(err)
	}
	read := func(id string) protoreflect.Message { return call(1004024, Record{"GoodsId": id}) }
	if out := read("gold"); getBool(out, "IsSuccess") || getInt(out, "Code") != studyRootMismatch {
		t.Fatal("off-root physical book", out)
	}
	if out := read("wood"); !getBool(out, "IsSuccess") {
		t.Fatal("could not record an advanced book for later", out)
	}
	if out := read("wood"); getBool(out, "IsSuccess") || getInt(out, "Code") != studyBookOnShelf {
		t.Fatal("duplicate reading", out)
	}
	r = s.roles.Get(roleID)
	if roleInt(locateGoods(r, "gold"), "Count") != 2 || roleInt(locateGoods(r, "wood"), "Count") != 1 ||
		!slices.Contains(stringRecords(r["GongFaRead"]), "gf_2_7") || slices.Contains(ownedGameMethods(r), "gf_2_7") ||
		roleString(r, "GongFaDuanTi") != "" {
		t.Fatal("reading should only file the matching book once")
	}
	if out := study("gf_2_7"); getBool(out, "IsSuccess") || getInt(out, "Code") != 4057 {
		t.Fatal("ordinary method level gate", out)
	}
	if err := s.roles.Update(roleID, func(r Record) error {
		r["Level"], r["GongFaLianShen"] = 11, "gf_1_1" // Legacy off-root equipped book.
		s.ensureGameRole(r)
		return nil
	}); err != nil {
		t.Fatal(err)
	}
	peer, client = auctionTestRole(t, s, roleID, nil)
	list = call(1001011, nil)
	rows := map[string]bool{}
	entries = list.Get(field(list, "Object")).List()
	for i := 0; i < entries.Len(); i++ {
		entry := entries.Get(i).Message()
		rows[getString(entry, "Id")] = getBool(entry, "IsUnderstand")
	}
	if learned, listed := rows["gf_2_7"]; !listed || learned || !rows["gf_1_1"] {
		t.Fatal("shelf lost a read book or an old equipped technique", rows)
	}
	if !getBool(study("gf_2_7"), "IsSuccess") || !getBool(equip(s.methods().byID["gf_1_1"]), "IsSuccess") {
		t.Fatal("breakthrough or legacy equipment")
	}
}

func TestWindMethodLearningAndEquippingMoveSpeed(t *testing.T) {
	s := auctionTestServer(t, t.TempDir())
	s.dataDir = "../server/data"
	if err := s.loadGameTables(); err != nil {
		t.Fatal(err)
	}
	const owner = "wind-pupil"
	p, _ := auctionTestRole(t, s, owner, nil)
	if err := s.roles.Update(owner, func(role Record) error {
		role["Level"], role["HasGold"], role["PracticeGuideVal"] = 1, true, 100
		s.ensureGameRole(role)
		learnGameMethod(role, s.methods().byID["gf_1_1"])
		return nil
	}); err != nil {
		t.Fatal(err)
	}
	const id = "sys_1_1_1"
	read := spellTestCall(t, s, p, 1011004, gameCultivationProtocols[1011004].req, gameCultivationProtocols[1011004].resp, Record{"Id": id})
	if !getBool(read, "IsSuccess") || getInt(read, "EffectType3") != methodMoveSpeedEffect || getInt(read, "EffectValue3") != 4 {
		t.Fatal("missing wind movement benefit in public technique details", read)
	}
	study := spellTestCall(t, s, p, 1001005, gameCultivationProtocols[1001005].req, gameCultivationProtocols[1001005].resp, Record{"Id": id, "Type": 2})
	if !getBool(study, "IsSuccess") || methodAffixPercent(s.roles.Get(owner), methodMoveSpeedEffect) != 0 {
		t.Fatal("learning alone should not grant movement", study)
	}
	equip := func(id string, percent int) {
		out := spellTestCall(t, s, p, 1005011, gameCultivationProtocols[1005011].req, gameCultivationProtocols[1005011].resp, Record{"ScopeType": 1, "AchievementId": id})
		if !getBool(out, "IsSuccess") || methodAffixPercent(s.roles.Get(owner), methodMoveSpeedEffect) != percent {
			t.Fatal("equipped method speed", id, out)
		}
	}
	equip(id, 4)
	equip("gf_1_1", 0)
}
