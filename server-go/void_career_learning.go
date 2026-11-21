package main

import (
	"bytes"
	"strconv"

	"google.golang.org/protobuf/reflect/protoreflect"
)

// Only the first two Live slots correspond to a residence room. Existing
// ranks in slots three and four, including legacy rolls, retain their identity.
func residenceCareerPlan(r Record, rebuild bool, location, profession int64, seen []byte) (int, int64) {
	if location < 1 || location > 2 || profession < 1 || profession > 9 {
		return 4008, 0
	}
	first, second := roleInt(r, "Live1Type"), roleInt(r, "Live2Type")
	// New clients include their confirmed two-room state in optional field 3.
	// A delayed rebuild must never replace a newly chosen profession.
	if len(seen) > 0 && !bytes.Equal(seen, offerNumber(nil, 3, uint64(10*first+second))) {
		return 4008, 0
	}
	old := roleInt(r, artisanSlots[location-1]+"Type")
	if rebuild && old == 0 {
		return 4116, 0
	}
	if !rebuild {
		if old != 0 {
			return 4113, 0
		}
		if gameHomeRank(r) < location {
			return 4331, 0
		}
	}
	for _, slot := range artisanSlots {
		if roleInt(r, slot+"Type") == profession && roleInt(r, slot+"Level") > 0 {
			return 4118, 0
		}
	}
	if rebuild {
		if roleInt(r, "ShinyJade") < 20 {
			return 4117, 0
		}
		return 4119, 20
	}
	price := int64(1000)
	if first != 0 || second != 0 {
		price = 10000
	}
	if roleInt(r, "ShinyStone") < price {
		return 4115, 0
	}
	return 4114, price
}

func clearResidenceCareerTraining(r Record, profession int64) {
	delete(farmMap(r, "ArtisanResearch"), strconv.FormatInt(profession, 10))
	var workshop string
	switch profession {
	case 1:
		workshop = "Alchemy"
	case 2:
		workshop = "Forge"
	case 3:
		workshop = "Talisman"
	case 8:
		workshop = "Garden"
	}
	if workshop != "" {
		delete(r, workshop+"Rank")
		delete(r, workshop+"Experience")
	}
}

func (s *Server) changeResidenceCareer(peer *Peer, cmd int, req, reply protoreflect.Message) {
	roleID := peer.currentRoleID()
	rebuild := cmd == 1005004
	key, currency := "LiveType", "ShinyStone"
	if rebuild {
		key, currency = "NewLiveType", "ShinyJade"
	}
	profession, location := getInt(req, key), getInt(req, "Location")
	code, balance := 4007, int64(0)
	active, online := s.online.Load(roleID)
	if roleID == "" || !online || active != peer {
		statusPB(reply, false, code)
		return
	}
	if s.preview != nil && !s.previewWrites {
		statusPB(reply, false, previewDeniedCode)
		return
	}
	err := s.roles.Transact(func(tx *StoreTx) error {
		role := tx.View(roleID)
		if role == nil {
			return nil
		}
		balance = roleInt(role, currency)
		var price int64
		code, price = residenceCareerPlan(role, rebuild, location, profession, req.GetUnknown())
		if price == 0 {
			return nil
		}
		r := tx.Get(roleID)
		slot := artisanSlots[location-1]
		old := roleInt(r, slot+"Type")
		balance -= price
		r[currency], r[slot+"Type"], r[slot+"Level"], r[slot+"Value"] = balance, profession, 1, 0
		professionID := strconv.FormatInt(profession, 10)
		retired := farmMap(r, "ResidenceRetiredCareers")
		if roleBool(retired, professionID) {
			clearResidenceCareerTraining(r, profession)
			delete(retired, professionID)
		}
		if rebuild {
			stillHeld := false
			for _, other := range artisanSlots {
				stillHeld = stillHeld || (roleInt(r, other+"Type") == old && roleInt(r, other+"Level") > 0)
			}
			if !stillHeld {
				clearResidenceCareerTraining(r, old)
				// Unearned placeholders in old saves are not a second profession.
				// Workshop practice must not resurrect one after this retirement.
				for _, other := range artisanSlots {
					if roleInt(r, other+"Type") == old && roleInt(r, other+"Level") == 0 {
						r[other+"Type"], r[other+"Value"] = 0, 0
					}
				}
				putFriendEntry(r, "ResidenceRetiredCareers", strconv.FormatInt(old, 10), true)
			}
		}
		if old == 4 || profession == 4 {
			recomputeGamePower(r)
		}
		return nil
	})
	if err != nil {
		code = 5000
		balance = roleInt(s.roles.Get(roleID), currency)
	}
	statusPB(reply, code == 4114 || code == 4119, code)
	set(reply, currency, balance)
}
