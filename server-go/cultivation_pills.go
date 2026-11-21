package main

import (
	"math"
	"strings"
	"time"
)

// Persistent items and unclaimed mentorship records hold their nominal value.
// Apply the curve exactly once at the recipient's present level.
func scaledActiveAir(level, nominal int64) int64 {
	if nominal <= 0 || level >= 90 {
		return 0
	}
	return max(1, int64(math.Round(float64(nominal)/lateRealmTimeFactor(level))))
}

func (s *Server) availableDoseAir(role Record, nominal int64) int64 {
	level := roleInt(role, "Level")
	room := max(0, int64(s.levelInfo(level).upgradeLimit)-roleInt(role, "SuperAir"))
	return min(room, scaledActiveAir(level, nominal))
}

func ordinaryAirPill(id, name string) bool {
	return id == "pill_naqi" || strings.Contains(name, "纳气") || strings.Contains(name, "聚气") || strings.Contains(name, "补气")
}

func (s *Server) ordinaryPillAir(role, goods Record) int64 {
	level := roleInt(role, "Level")
	limit := int64(s.levelInfo(level).upgradeLimit)
	if strings.HasPrefix(roleString(goods, "TemplateId"), "sys_zhu_ji_dan_") {
		if level != 10 {
			return 0
		}
		grade := max(0, min(3, roleInt(goods, "Rank")/4-1))
		return s.availableDoseAir(role, max(1, limit*(grade+1)/5))
	}
	return s.availableDoseAir(role, max(1, limit*8/100))
}

// Both the backpack and the dedicated medicine panel settle the same dose.
func (s *Server) consumeGamePill(r, g Record) int {
	if inventoryFlag(g, "IsLock") || inventoryFlag(g, "IsUsing") || hasFriendOfferLock(g) || transferStackCount(g) <= 0 {
		return 4509
	}
	if roleString(g, "BrewEffect") != "" {
		return s.consumeBrewDose(r, g)
	}
	id, name := roleString(g, "TemplateId"), roleString(g, "Name")
	level := roleInt(r, "Level")
	grade := max(0, min(3, roleInt(g, "Rank")/4-1))
	air := int64(s.levelInfo(level).upgradeLimit)
	heal := func(base int64) int {
		return addDose(r, "CurrentPureBlood", scaleForgeStat(base, forgeAffix(r, "healing")), roleInt(r, "PureBlood"))
	}
	switch {
	case strings.HasPrefix(id, "sys_zhu_ji_dan_"):
		if level != 10 {
			return 4057
		}
		return addDose(r, "SuperAir", s.ordinaryPillAir(r, g), air)
	case id == "ti_li_dan":
		return heal(100)
	case id == "jing_li_dan":
		recoverRoleVigor(r, time.Now().Unix())
		return addDose(r, "CurrentVigor", 100, roleInt(r, "Vigor"))
	case id == "wu_xing_dan":
		return addDose(r, "CurrentClever", 100, roleInt(r, "Clever"))
	case id == "hun_li_dan":
		return addDose(r, "HunLiVal", 5, math.MaxInt32)
	case strings.HasPrefix(id, "sys_qing_du_dan_") || strings.Contains(name, "解毒"):
		dose := int64(20)
		if strings.HasPrefix(id, "sys_qing_du_dan_") {
			dose = [...]int64{10, 20, 50, 100}[grade]
		}
		if roleInt(r, "CurrentPoisonVal") <= 0 {
			return 4971
		}
		r["CurrentPoisonVal"] = max(0, roleInt(r, "CurrentPoisonVal")-dose)
	case strings.Contains(name, "经脉丹"):
		points := int64(1)
		if strings.HasPrefix(id, "jing_mai_dan_") {
			points = [...]int64{2, 6, 13, 28}[grade]
		}
		r["AcupointValue"] = acupointBalance(r)
		return addDose(r, "AcupointValue", points, math.MaxInt32)
	case strings.HasPrefix(id, "sys_bu_tian_dan_"):
		if level < 81 {
			return 4057
		}
		if roleBool(r, "UsedRootPill") {
			return 4971
		}
		elem := map[string]string{"jin": "Gold", "mu": "Wood", "shui": "Water", "huo": "Fire", "tu": "Earth"}[strings.TrimPrefix(id, "sys_bu_tian_dan_")]
		if elem == "" || roleBool(r, "Has"+elem) {
			return 4971
		}
		r["Has"+elem], r[elem], r["UsedRootPill"] = true, 1, true
	case strings.HasPrefix(id, "shou_yuan_dan_"):
		if roleBool(r, "Used_"+id) {
			return 4971
		}
		tier := roleInt(g, "Level")
		if level < tier || tier < 11 || tier > 81 {
			return 4057
		}
		gain := int64(20) << ((tier - 11) / 10)
		prior := roleInt(r, "MaxAge")
		if prior > math.MaxInt32-gain {
			return 4971
		}
		r["Used_"+id] = true
		r["BonusLifespan"], r["MaxAge"] = roleInt(r, "BonusLifespan")+gain, prior+gain
	case strings.Contains(name, "回春") || strings.Contains(name, "疗伤"):
		return heal(max(20, roleInt(r, "PureBlood")/5))
	case ordinaryAirPill(id, name):
		if level >= 90 {
			return 4971
		}
		return addDose(r, "SuperAir", s.ordinaryPillAir(r, g), air)
	default:
		return 4605
	}
	return 2000
}

func addDose(r Record, key string, amount, cap int64) int {
	before := roleInt(r, key)
	if amount <= 0 || before >= cap {
		return 4971
	}
	r[key] = before + min(amount, cap-before)
	return 2000
}
