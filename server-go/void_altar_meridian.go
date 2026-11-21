package main

import (
	"math"
	"slices"

	"google.golang.org/protobuf/reflect/protoreflect"
)

// Record the number of successfully applied books per owner and technique.
// The original item table specifies +0.05..0.12 灵衍 per copy. Use the central 0.08.
func altarMethodImprovements(role Record, def *gameMethod) int {
	upgrades, _ := role["MethodScriptureRefinements"].(Record)
	return max(0, min(16-def.rank, int(roleInt(upgrades, def.id))))
}

func altarMethodRank(role Record, def *gameMethod) int {
	return def.rank + altarMethodImprovements(role, def)
}
func altarMethodCircle(role Record, def *gameMethod) float64 {
	return def.baseCircle + .08*float64(altarMethodImprovements(role, def))
}

func (s *Server) applyMethodScripture(owner, id string, reply protoreflect.Message) {
	def := s.methods().byID[id]
	if def == nil {
		statusPB(reply, false, 4326)
		return
	}
	code, rank, capacity, conversion := 4007, def.rank, 0, def.baseCircle
	err := s.roles.Transact(func(tx *StoreTx) error {
		role := tx.View(owner)
		if role == nil || owner == "" || reservedStoreID(owner) {
			return nil
		}
		code = 4326
		if !slices.Contains(ownedGameMethods(role), id) {
			return nil
		}
		rank, conversion = altarMethodRank(role, def), altarMethodCircle(role, def)
		code = 4704
		if rank >= 16 {
			return nil
		}
		code = 4703
		for _, raw := range goodsList(role) {
			g, _ := raw.(Record)
			if roleInt(g, "Type") != 39 || roleInt(g, "Level") != int64(def.learningLevel) || inventoryFlag(g, "IsUsing") || inventoryFlag(g, "IsLock") || hasFriendOfferLock(g) || transferStackCount(g) <= 0 {
				continue
			}
			changed := tx.Get(owner)
			removeGoods(changed, roleString(g, "GoodsId"))
			auctionMapEdit(changed, "MethodScriptureRefinements")[id] = altarMethodImprovements(role, def) + 1
			code, rank, capacity, conversion = 4705, altarMethodRank(changed, def), len(goodsList(changed)), altarMethodCircle(changed, def)
			return nil
		}
		return nil
	})
	if err != nil {
		statusPB(reply, false, 4007)
		return
	}
	statusPB(reply, code == 4705, code)
	putPB(reply, Record{"Rank": rank, "ConversionRate": math.Round(conversion*10000) / 10000, "NowCapacity": capacity})
	def.writeEffects(reply)
}
