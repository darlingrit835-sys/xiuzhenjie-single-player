package main

import (
	"math"
	"testing"
	"time"

	"google.golang.org/protobuf/proto"
	"google.golang.org/protobuf/reflect/protoreflect"
)

func spellTestCall(t *testing.T, s *Server, p *Peer, cmd int, reqName, respName string, values Record) protoreflect.Message {
	t.Helper()
	m := Message{Cmd: cmd, Msid: 23}
	if reqName != "" {
		req := s.pb(reqName)
		putPB(req, values)
		var err error
		m.Data, err = proto.Marshal(req)
		if err != nil {
			t.Fatal(err)
		}
	}
	s.handleGame(p, m)
	return p.conn.(*auctionTestConnection).receive(t, s, cmd, m.Msid, respName)
}

func TestSpellPracticeUnlockRerollAndBattle(t *testing.T) {
	dir := t.TempDir()
	s := auctionTestServer(t, dir)
	p, _ := auctionTestRole(t, s, "scribe", nil)
	if err := s.roles.Update("scribe", func(r Record) error { r["CurrentVigor"] = 30; return nil }); err != nil {
		t.Fatal(err)
	}
	open := func() protoreflect.Message {
		return spellTestCall(t, s, p, 1005038, "", "EnterBattleHomeResp_1005038", nil)
	}
	practice := func(id string, action, count, revision int) protoreflect.Message {
		return spellTestCall(t, s, p, 1005040, "PracticeSkillReq_1005040", "PracticeSkillResp_1005040",
			Record{"MethodSkillId": id, "PracticeNum": count, "PracticeAction": action, "ExpectedRevision": revision})
	}
	entry := func() protoreflect.Message {
		out := open()
		return out.Get(field(out, "Skill")).List().Get(0).Message()
	}
	if out := open(); getInt(out, "CurrentVigor") != 30 || out.Get(field(out, "Skill")).List().Len() != 21 {
		t.Fatal(out)
	}
	if row := entry(); getInt(row, "Proficiency") != 0 || getInt(row, "BaseCastMS") != 100 {
		t.Fatal(row)
	}
	for _, bad := range []struct {
		id            string
		action, count int
	}{{"skill_qingfeng", 1, 1}, {"skill_yujian", 3, 0}, {"skill_yujian", 1, -1}, {"skill_yujian", 1, 61}, {"skill_yujian", 1, 31}} {
		if out := practice(bad.id, bad.action, bad.count, 0); getBool(out, "IsSuccess") {
			t.Fatal(out)
		}
	}
	const id = "skill_yujian"
	out := practice(id, 1, 10, 0)
	if !getBool(out, "IsSuccess") || getInt(out, "Proficiency") != 50 || getInt(out, "Level") != 2 || getInt(out, "PracticeAffix") != 1 || getInt(out, "CurrentVigor") != 20 {
		t.Fatal(out)
	}
	if retry := practice(id, 1, 10, 0); getBool(retry, "IsSuccess") {
		t.Fatal("charged a stale revision", retry)
	}
	role := s.roles.Get("scribe")
	role["SkillSettings"] = []any{Record{"SkillId": id, "CombatSettingId": 0}}
	s.config.BattleDurationSeconds = 120
	d := s.createDungeon(role, "1")
	if !d.expires.After(time.Now()) {
		t.Fatal(d.expires)
	}
	battle := spellTestCall(t, s, p, 1010002, "RoleJoinDungeonReq_1010002", "RoleJoinDungeonResp_1010002", Record{"DungeonId": d.id})
	slot := battle.Get(field(battle, "Skills")).List().Get(0).Message()
	if getInt(slot, "PracticeAffix") != 1 || getFloat(slot, "BaseDamage") != 80*1.23 || getFloat(slot, "ConversionRate") != 1.23 {
		t.Fatal(slot)
	}
	out = practice(id, 3, 0, 1)
	candidate := getInt(out, "PendingAffix")
	if getInt(out, "CurrentVigor") != 14 || candidate == 1 || candidate == 0 || getInt(out, "PracticeAffix") != 1 {
		t.Fatal(out)
	}
	if retry := practice(id, 3, 0, 2); getBool(retry, "IsSuccess") {
		t.Fatal("free reroll", retry)
	}
	s = auctionTestServer(t, dir)
	p, _ = auctionTestRole(t, s, "scribe", nil)
	if row := entry(); getInt(row, "PendingAffix") != candidate || getInt(row, "Proficiency") != 50 {
		t.Fatal(row)
	}
	if accept := practice(id, 4, 0, 2); getInt(accept, "PracticeAffix") != candidate || getInt(accept, "PendingAffix") != 0 || getInt(accept, "CurrentVigor") != 14 || getInt(accept, "Proficiency") != 50 {
		t.Fatal(accept)
	}
	out = practice(id, 3, 0, 3)
	if getInt(out, "PendingAffix") == candidate || getInt(out, "CurrentVigor") != 8 {
		t.Fatal(out)
	}
	kept := practice(id, 5, 0, 4)
	if !getBool(kept, "IsSuccess") || getInt(kept, "PendingAffix") != 0 || getInt(kept, "PracticeAffix") != candidate || getInt(kept, "CurrentVigor") != 8 {
		t.Fatal(kept)
	}
	if rejected := practice(id, 2, 9, 5); getInt(rejected, "Code") != 4290 {
		t.Fatal(rejected)
	}
	if p := practiceFor(s.roles.Get("scribe"), id); p.revision != 5 || p.force != 10 || p.effort != 10 {
		t.Fatal(p)
	}
}

func TestSpellPracticeHasteAndComplete(t *testing.T) {
	s := auctionTestServer(t, t.TempDir())
	p, _ := auctionTestRole(t, s, "tester", nil)
	if err := s.roles.Update("tester", func(r Record) error { r["CurrentVigor"] = 100; return nil }); err != nil {
		t.Fatal(err)
	}
	const id = "skill_zhuyang"
	out := spellTestCall(t, s, p, 1005040, "PracticeSkillReq_1005040", "PracticeSkillResp_1005040",
		Record{"MethodSkillId": id, "PracticeAction": 2, "PracticeNum": 60})
	if getInt(out, "Level") != 5 || getInt(out, "CurrentVigor") != 40 || getInt(out, "PracticeAffix") != 2 {
		t.Fatal(out)
	}
	state := practiceFor(s.roles.Get("tester"), id)
	if power, fast := state.stats(); power != 160 || fast != 44 || state.castMS(620) != 347 {
		t.Fatal(state, power, fast)
	}
	setting := spellTestCall(t, s, p, 1009003, "SetSkillReq_1009003", "SetSkillResp_1009003", Record{"SkillId": id, "CombatSettingId": 0})
	if !getBool(setting, "IsSuccess") {
		t.Fatal(setting)
	}
	s.config.BattleDurationSeconds = 120
	battle := spellTestCall(t, s, p, 1010002, "RoleJoinDungeonReq_1010002", "RoleJoinDungeonResp_1010002", nil)
	slot := battle.Get(field(battle, "Skills")).List().Get(0).Message()
	if getString(slot, "TemplateId") != id || getInt(slot, "PracticeAffix") != 2 ||
		getInt(slot, "CastTime") != 347 || math.Abs(getFloat(slot, "BaseDamage")-339.2) > 0.001 {
		t.Fatal(slot)
	}
	more := spellTestCall(t, s, p, 1005040, "PracticeSkillReq_1005040", "PracticeSkillResp_1005040",
		Record{"MethodSkillId": id, "PracticeAction": 2, "PracticeNum": 1, "ExpectedRevision": 1})
	if getBool(more, "IsSuccess") || roleInt(s.roles.Get("tester"), "CurrentVigor") != 40 {
		t.Fatal(more)
	}
}

// A weapon's elemental resonance and a spell's inscription multiply on the
// same skill slot. Re-equipping neither can replace the other's wire effect.
func TestSpellPracticeCombinesWithForgeResonance(t *testing.T) {
	s := auctionTestServer(t, t.TempDir())
	s.config.BattleDurationSeconds = 120
	p, _ := auctionTestRole(t, s, "adept", nil)
	if err := s.roles.Update("adept", func(r Record) error {
		r["CurrentVigor"] = 12 // Ten practice sessions and the second field entry.
		r["EquippedGoods"] = []any{Record{"Type": 15, "TemplateId": "ugc.test", "Rank": 1, "Level": 1, "MoldQuality": "精模", "FiveElement": 1, "BaseAttack": 25}}
		return nil
	}); err != nil {
		t.Fatal(err)
	}
	join := func() protoreflect.Message {
		out := spellTestCall(t, s, p, 1010002, "RoleJoinDungeonReq_1010002", "RoleJoinDungeonResp_1010002", nil)
		return out.Get(field(out, "Skills")).List().Get(0).Message()
	}
	before := join()
	if getInt(before, "ForgeResonancePercent") != 9 || getInt(before, "PracticeAffix") != 0 {
		t.Fatal(before)
	}
	spellTestCall(t, s, p, 1005040, "PracticeSkillReq_1005040", "PracticeSkillResp_1005040",
		Record{"MethodSkillId": "skill_yujian", "PracticeAction": 1, "PracticeNum": 10})
	after := join()
	want := getFloat(before, "BaseDamage") * 1.23
	if getInt(after, "ForgeResonancePercent") != 9 || getInt(after, "PracticeAffix") != 1 || math.Abs(getFloat(after, "BaseDamage")-want) > 1e-6 {
		t.Fatal(before, after)
	}
}

func TestLegacySpellPracticeIgnoresRepeatedCharge(t *testing.T) {
	s := auctionTestServer(t, t.TempDir())
	p, _ := auctionTestRole(t, s, "legacy", nil)
	if err := s.roles.Update("legacy", func(r Record) error { r["CurrentVigor"] = 5; return nil }); err != nil {
		t.Fatal(err)
	}
	call := func() protoreflect.Message {
		return spellTestCall(t, s, p, 1005040, "PracticeSkillReq_1005040", "PracticeSkillResp_1005040", Record{"MethodSkillId": "skill_yujian", "PracticeNum": 1})
	}
	if out := call(); !getBool(out, "IsSuccess") || getInt(out, "CurrentVigor") != 4 {
		t.Fatal(out)
	}
	if out := call(); getBool(out, "IsSuccess") || roleInt(s.roles.Get("legacy"), "CurrentVigor") != 4 {
		t.Fatal(out)
	}
}
