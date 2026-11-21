package main

import (
	"math"
	"testing"
	"time"

	"google.golang.org/protobuf/encoding/protowire"
)

func TestMovementSpeedFromArmoryToDungeon(t *testing.T) {
	dir := t.TempDir()
	s := auctionTestServer(t, dir)
	s.config.BattleDurationSeconds = 120
	p, _ := auctionTestRole(t, s, "runner", nil)
	if err := s.roles.Update(p.roleID, func(r Record) error {
		r["EquippedGoods"] = []any{Record{"GoodsId": "shoes", "Type": 29, "Rank": 2}}
		r["EquippedMethodEffects"] = Record{"202": 4}
		r["TalismanDungeonHaste"], r["CurrentVigor"] = 12, 8
		r["SkillSettings"] = []any{Record{"SkillId": "skill_tafeng", "CombatSettingId": 0}}
		return nil
	}); err != nil {
		t.Fatal(err)
	}
	panel := spellTestCall(t, s, p, 1009005, "", "EnterEquipmentResp_1009005", nil)
	bonus := fieldEntrySupplies(t, panel.GetUnknown())
	if math.Abs(getFloat(panel, "CurrentMoveSpeed")-5.7*1.04) > 1e-8 || bonus[14] != 12 || bonus[15] != 4 {
		t.Fatal("armory must distinguish permanent speed from the prepared talisman", panel, bonus)
	}
	join := func(id string, wantCode int64, speed float64, sigil, method int64) {
		t.Helper()
		out := spellTestCall(t, s, p, 1010002, "RoleJoinDungeonReq_1010002", "RoleJoinDungeonResp_1010002", Record{"DungeonId": id})
		fields := fieldEntrySupplies(t, out.GetUnknown())
		if getInt(out, "Code") != wantCode || fields[32] != sigil || fields[33] != method || math.Abs(getFloat(out, "CurrentMoveSpeed")-speed) > 1e-8 {
			t.Fatal("dungeon movement snapshot", out, fields)
		}
	}
	first := s.createDungeon(s.roles.Get(p.roleID), "1111W11")
	path := s.roles.path
	s.roles.path = dir
	join(first.id, 5000, 0, 0, 0)
	s.roles.path = path
	if preparedDungeonHaste(s.roles.Get(p.roleID)) != 12 {
		t.Fatal("failed admission consumed the talisman")
	}
	join(first.id, 4502, 5.7*1.16, 12, 4)
	if preparedDungeonHaste(s.roles.Get(p.roleID)) != 0 {
		t.Fatal("successful entry retained the talisman")
	}
	if err := s.roles.Update(p.roleID, func(r Record) error {
		delete(r, "EquippedGoods")
		delete(r, "EquippedMethodEffects")
		r["TalismanDungeonHaste"] = 18
		return nil
	}); err != nil {
		t.Fatal(err)
	}
	join(first.id, 4502, 5.7*1.16, 12, 4)
	if preparedDungeonHaste(s.roles.Get(p.roleID)) != 18 {
		t.Fatal("retry consumed a talisman intended for the next expedition")
	}
	second := s.createDungeon(s.roles.Get(p.roleID), "1111W11")
	if code := s.enterField(second, 0); code != 4506 || preparedDungeonHaste(s.roles.Get(p.roleID)) != 18 {
		t.Fatal("legacy scene entry cannot deliver a speed stat", code)
	}
	join(second.id, 4502, 5*1.18, 18, 0)
}

func TestMovementSpellMagnitudeDurationAndCooldown(t *testing.T) {
	s := auctionTestServer(t, t.TempDir())
	s.config.BattleDurationSeconds = 120
	p, _ := auctionTestRole(t, s, "wind", nil)
	ids := []string{"skill_tafeng", "skill_hanbing", "skill_dingshen"}
	if err := s.roles.Update(p.roleID, func(r Record) error {
		r["Physique"], r["Perception"] = 50000, 50000
		r["EquippedGoods"] = []any{Record{"Type": 15, "TemplateId": "ugc.wind", "FiveElement": 2, "MoldQuality": "精模", "GoodsId": "w"}}
		r["SpellPractice"] = Record{"skill_tafeng": Record{"Effort": 10, "Force": 10, "Affix": 1}}
		for i, id := range ids {
			r["SkillSettings"] = append(goodsListForField(r, "SkillSettings"), Record{"SkillId": id, "CombatSettingId": i})
		}
		return nil
	}); err != nil {
		t.Fatal(err)
	}
	world := s.createDungeon(s.roles.Get(p.roleID), "1111W11")
	reply := spellTestCall(t, s, p, 1010002, "RoleJoinDungeonReq_1010002", "RoleJoinDungeonResp_1010002", Record{"DungeonId": world.id})
	slots := reply.Get(field(reply, "Skills")).List()
	durations := []int64{8044, 2000, 1000} // 6 seconds × 1.23 practice × 1.09 resonance.
	for i, magnitude := range []float64{.25, .30, 0} {
		row := slots.Get(i).Message()
		info := spellTestCall(t, s, p, 1011009, "MethodSkillDetailReq_1011009", "MethodSkillDetailResp_1011009", Record{"Id": ids[i]})
		value := 0.0
		if wire := info.GetUnknown(); len(wire) > 0 {
			tag, typ, n := protowire.ConsumeTag(wire)
			if tag != 10 || typ != protowire.Fixed64Type || n <= 0 {
				t.Fatal(info)
			}
			bits, n := protowire.ConsumeFixed64(wire[n:])
			if n != 8 {
				t.Fatal("incomplete magnitude field")
			}
			value = math.Float64frombits(bits)
		}
		if getFloat(row, "BaseDamage") != magnitude || value != magnitude || getInt(info, "ContinueTime") != durations[i] || getInt(row, "ContinueTime") != durations[i] {
			t.Fatal("spell detail and battle disagree or scale speed by attack power", info, row)
		}
	}
	if code := s.enterField(world, 0); code != 4506 {
		t.Fatal(code)
	}
	now := time.Now()
	if world.castFieldSkill(1, 0, now) != 4534 || world.castFieldSkill(1, 0, now) != 4535 || world.castFieldSkill(1, 0, now.Add(12*time.Second)) != 4534 ||
		world.castFieldSkill(2, 0, now) != 4535 || world.castFieldSkill(2, 1001, now) != 4534 {
		t.Fatal("haste must work without a target, honor cooldown and leave targeted slows available")
	}
	human := battleMonster{objectType: 2001, attack: 50000, element: 3}
	for _, index := range []int{7, 8} {
		ability := human.skill(index)
		if damage := human.skillDamage(ability, [6]int{}, 0); damage != ability.multiplier || damage >= 1 || ability.durationMS <= 0 {
			t.Fatal("hostile motion effect sent attack power instead of a fraction", ability, damage)
		}
	}
}
