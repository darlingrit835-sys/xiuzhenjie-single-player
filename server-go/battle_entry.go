package main

import (
	"errors"
	"log"
	"time"
)

func dungeonVigorCost(mapID string) int64 {
	_, wave := encounterLevel(1, mapID)
	switch wave {
	case "deep":
		return 4
	case "lair":
		return 6
	default:
		return 2
	}
}

func spendDungeonAdmission(role Record, mapID string) error {
	cost := dungeonVigorCost(mapID)
	if !roleBool(role, "FirstDungeonAdmission") {
		cost = 0
	}
	if !spendArtisanVigor(role, cost) {
		return previewReject(4290)
	}
	role["FirstDungeonAdmission"] = true
	return nil
}

func dungeonEntryErrorCode(err error) int {
	var code previewReject
	if errors.As(err, &code) {
		return int(code)
	}
	if errors.Is(err, ErrRecordNotFound) {
		return 4503
	}
	log.Printf("battle entry save: %v", err)
	return 5000
}

// Frozen when the game connection requests the player's battle attributes.
type dungeonSupplies struct {
	ward, hastePercent int64
	methodPercent      int
	moveSpeed          float64
}

func (s *Server) admitDungeon(d *battleDungeon) (dungeonSupplies, int) {
	d.mu.Lock()
	defer d.mu.Unlock()
	if d.closed || !time.Now().Before(d.expires) {
		return dungeonSupplies{}, 4503
	}
	if d.suppliesDelivered {
		return d.supplies, 4502
	}
	var supplies dungeonSupplies
	err := s.roles.Update(d.roleID, func(role Record) error {
		if !d.admitted {
			if err := spendDungeonAdmission(role, d.mapID); err != nil {
				return err
			}
		}
		// The normal game join carries the ward in its role packet. A legacy
		// direct scene entry has no corresponding role payload, so it preserves it.
		supplies.ward = roleInt(role, "AlchemyWard")
		delete(role, "AlchemyWard")
		supplies.hastePercent = consumeDungeonHaste(role)
		supplies.methodPercent = methodAffixPercent(role, methodMoveSpeedEffect)
		supplies.moveSpeed = battleMovementSpeed(role, supplies.methodPercent, supplies.hastePercent)
		return nil
	})
	if err != nil {
		return dungeonSupplies{}, dungeonEntryErrorCode(err)
	}
	d.admitted, d.suppliesDelivered, d.supplies = true, true, supplies
	return supplies, 4502
}
