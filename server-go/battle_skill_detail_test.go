package main

import "testing"

func TestBattleLoadoutDetails(t *testing.T) {
	s := auctionTestServer(t, t.TempDir())
	p, _ := auctionTestRole(t, s, "cultivator", nil)
	for _, skill := range battleSkills {
		cmd := 1011009
		if skill.kind == 2 {
			cmd = 1004018
		}
		info := smithingInventoryCall(t, s, p, cmd, Record{"Id": skill.id})
		if !getBool(info, "IsSuccess") {
			t.Fatalf("battle detail for %q: %v", skill.name, info)
		}
		if skill.kind == 1 && (getInt(info, "SkillType") != int64(skill.skillType) ||
			getInt(info, "CastTime") != int64(skill.castMS) || getInt(info, "ContinueTime") != int64(skill.durationMS)) {
			t.Fatalf("spell template mismatch for %q: %v", skill.name, info)
		}
		if skill.kind == 2 && getInt(info, "CoolDown") != int64(skill.cooldownS)*1000 {
			t.Fatalf("weapon cooldown mismatch for %q: %v", skill.name, info)
		}
	}
	for cmd, id := range map[int]string{1011009: "skill_qingfeng", 1004018: "skill_liehuo"} {
		if getBool(smithingInventoryCall(t, s, p, cmd, Record{"Id": id}), "IsSuccess") {
			t.Fatalf("wrong category accepted by %d for %q", cmd, id)
		}
	}
}
