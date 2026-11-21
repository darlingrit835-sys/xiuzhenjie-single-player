package main

import (
	"fmt"
	"google.golang.org/protobuf/reflect/protoreflect"
	"strings"
	"time"
)

var brewEffectTypes = map[string]int{
	"recover_blood": 13, "gain_super_air": 22, "recover_vigor": 30, "reduce_poison": 32, "absorb_damage": 38,
}

func validElixirBasis(r elixirRecipe) bool {
	if r.PoisonCost < 0 || r.PoisonCost > 12 || r.EffectID == "reduce_poison" && r.PoisonCost != 0 {
		return false
	}
	switch r.EffectBasis {
	case "flat":
		return true
	case "realm_air":
		return r.EffectID == "gain_super_air" && r.BaseEffect <= 7500
	case "max_blood":
		return r.EffectID == "recover_blood" && r.BaseEffect <= 7500
	}
	return false
}

func (s *Server) elixirPoints(role Record, basis string, value int64) int64 {
	var base int64
	switch basis {
	case "realm_air":
		base = int64(s.levelInfo(roleInt(role, "Level")).upgradeLimit)
	case "max_blood":
		base = max(0, roleInt(role, "PureBlood"))
	default:
		return value
	}
	// Split before multiplying so large legitimate late-realm stats stay exact.
	return max(1, base/10000*value+(base%10000*value+5000)/10000)
}

// Wards use the wearer's actual dungeon blood, including equipment. The catalog
// strength for a pill is its shield at 2,000 blood, and a sigil at 1,250.
func artisanWard(role Record, strength, basis int64) int64 {
	hp := equippedBattleStats(role).blood
	points := (hp*strength + basis/2) / basis
	points = (points*int64(100+artisanResearchRank(role, 5)) + 50) / 100
	return max(1, min(points, hp*48/100))
}

func (s *Server) effectiveBrewPoints(role Record, effect, basis string, value int64) int64 {
	if value <= 0 {
		return 0
	}
	points := s.elixirPoints(role, basis, value)
	switch effect {
	case "gain_super_air":
		return s.availableDoseAir(role, points)
	case "absorb_damage":
		return artisanWard(role, points, 2000)
	case "recover_blood":
		return scaleForgeStat(points, forgeAffix(role, "healing"))
	}
	return points
}

// Older clients can only display whole basis points. Late realms and partly
// filled bars use the existing flat-point representation for an exact quote.
func (s *Server) brewDisplayBasis(role Record, r elixirRecipe, value int) string {
	if r.EffectBasis == "realm_air" && (lateRealmTimeFactor(roleInt(role, "Level")) != 1 ||
		s.elixirPoints(role, r.EffectBasis, int64(value)) != s.effectiveBrewPoints(role, r.EffectID, r.EffectBasis, int64(value))) {
		return "flat"
	}
	return r.EffectBasis
}

func (s *Server) brewDisplayValue(role Record, r elixirRecipe, value int) int {
	if s.brewDisplayBasis(role, r, value) == "flat" {
		return int(s.effectiveBrewPoints(role, r.EffectID, r.EffectBasis, int64(value)))
	}
	return value
}

func (s *Server) elixirEffectText(role Record, effect, basis, name string, value int) string {
	unit := ""
	switch basis {
	case "realm_air":
		points := s.effectiveBrewPoints(role, effect, basis, int64(value))
		if s.brewDisplayBasis(role, elixirRecipe{EffectID: effect, EffectBasis: basis}, value) == "flat" {
			return fmt.Sprintf("%s %d 点（按当前境界与剩余缺口结算）", name, points)
		}
		unit = "本境突破所需修为"
	case "max_blood":
		unit = "气血上限"
	}
	if unit != "" {
		return fmt.Sprintf("%s %s的 %d.%02d%%（当前约 %d 点）", name, unit, value/100, value%100, s.effectiveBrewPoints(role, effect, basis, int64(value)))
	}
	return fmt.Sprintf("%s %d 点", name, s.effectiveBrewPoints(role, effect, basis, int64(value)))
}

func (s *Server) elixirDetail(roleID string, out protoreflect.Message, g Record) {
	fillOwnedGoodsDetail(out, g, 19, 19)
	basis := roleString(g, "BrewBasis")
	effect := roleString(g, "BrewEffect")
	if roleInt(g, "Type") != 19 {
		return
	}
	id := roleString(g, "TemplateId")
	isOrdinaryAir := effect == "" && (strings.HasPrefix(id, "sys_zhu_ji_dan_") || ordinaryAirPill(id, roleString(g, "Name")))
	if !isOrdinaryAir && basis != "realm_air" && basis != "max_blood" && effect != "gain_super_air" && effect != "absorb_damage" && effect != "recover_blood" {
		return
	}
	role := s.roles.project(roleID, append([]string{"Level", "SuperAir", "PureBlood", "Perception", "Physique", "EquippedGoods", "BattleMeridianBonuses", "ArtisanResearch"}, artisanCareerFields...))
	if isOrdinaryAir {
		count := float64(s.ordinaryPillAir(role, g))
		putPB(out, Record{"EffectType": brewEffectTypes["gain_super_air"], "EffectValue": count, "RealityEffectValue": count})
		return
	}
	val := roleInt(g, "EffectValue")
	set(out, "EffectValue", float64(s.effectiveBrewPoints(role, effect, basis, val)))
	set(out, "RealityEffectValue", float64(s.effectiveBrewPoints(role, effect, basis, roleInt(g, "RealityEffectValue"))))
	if basis == "max_blood" || basis == "realm_air" && s.brewDisplayBasis(role, elixirRecipe{EffectID: effect, EffectBasis: basis}, int(val)) == "realm_air" {
		out.SetUnknown(offerNumber(offerString(out.GetUnknown(), 13, basis), 14, uint64(val)))
	}
}

// Return a native inventory response code. Check the whole dose before mutating the role.
func (s *Server) consumeBrewDose(role, goods Record) int {
	if transferStackCount(goods) <= 0 || inventoryFlag(goods, "IsLock") || inventoryFlag(goods, "IsUsing") || hasFriendOfferLock(goods) {
		return 4509
	}
	value := s.effectiveBrewPoints(role, roleString(goods, "BrewEffect"), roleString(goods, "BrewBasis"), roleInt(goods, "EffectValue"))
	if value <= 0 {
		if roleString(goods, "BrewEffect") == "gain_super_air" {
			return 4971
		}
		return 4605
	}
	poison, cost := max(0, roleInt(role, "CurrentPoisonVal")), max(0, roleInt(goods, "PoisonValue"))
	if cost > 0 && poison > 100-cost {
		return 4972
	}
	var key string
	var next int64
	switch roleString(goods, "BrewEffect") {
	case "recover_blood":
		key = "CurrentPureBlood"
		next = min(roleInt(role, "PureBlood"), roleInt(role, key)+value)
	case "recover_vigor":
		recoverRoleVigor(role, time.Now().Unix())
		key = "CurrentVigor"
		next = min(roleInt(role, "Vigor"), roleInt(role, key)+value)
	case "gain_super_air":
		key = "SuperAir"
		if roleInt(role, "Level") >= 90 {
			return 4971
		}
		next = min(int64(s.levelInfo(roleInt(role, "Level")).upgradeLimit), roleInt(role, key)+value)
	case "reduce_poison":
		if poison == 0 {
			return 4971
		}
		role["CurrentPoisonVal"] = max(0, poison-value)
		return 2000
	case "absorb_damage":
		key = "AlchemyWard"
		next = value
	default:
		return 4605
	}
	if next <= roleInt(role, key) {
		return 4971
	}
	role[key] = next
	if cost > 0 {
		role["CurrentPoisonVal"] = poison + cost
	}
	return 2000
}
