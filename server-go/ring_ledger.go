package main

import (
	"math"

	"google.golang.org/protobuf/proto"
	"google.golang.org/protobuf/reflect/protoreflect"
)

func ringCapacity(role Record) int {
	// Saves created before ring upgrades have a hundred slots.
	return int(min(200, max(100, roleInt(role, "RingCapacity"))))
}

func (s *Server) expandRing(peer *Peer, _, out protoreflect.Message) (func(), error) {
	if replay, err := peer.replayTrialReceipt(1004025, "", out); replay || err != nil {
		if replay {
			role := s.roles.project(peer.currentRoleID(), []string{"RingCapacity", "ShinyJade"})
			putPB(out, Record{"MaxCapacity": ringCapacity(role), "ShinyJade": roleInt(role, "ShinyJade")})
		}
		return nil, err
	}
	var result Record
	err := s.roles.Transact(func(tx *StoreTx) error {
		role := tx.Get(peer.currentRoleID())
		if role == nil {
			return previewReject(4007)
		}
		capacity := ringCapacity(role)
		base := capacity
		if roleInt(role, "VipLevel") >= 11 {
			base -= 50
		}
		costs := [...]int64{10, 30, 60, 100, 150, 200, 300, 400, 500, 600}
		if base < 50 || base >= 150 || base%10 != 0 {
			return previewReject(4008)
		}
		price := costs[(base-50)/10]
		jade := roleInt(role, "ShinyJade")
		if jade < price || jade > math.MaxInt32 {
			return previewReject(4298)
		}
		role["RingCapacity"], role["ShinyJade"] = capacity+10, jade-price
		result = Record{"IsSuccess": true, "Code": 2002, "MaxCapacity": capacity + 10, "ShinyJade": jade - price}
		return nil
	})
	if err == nil {
		peer.rememberTrialReceipt(1004025, "", result, out)
	}
	return nil, err
}

func (s *Server) sellRingGoods(peer *Peer, req, out protoreflect.Message) (func(), error) {
	rows := req.Get(field(req, "GoodsInfos")).List()
	if rows.Len() == 0 || rows.Len() > 200 {
		return nil, previewReject(4008)
	}
	wire, err := proto.MarshalOptions{Deterministic: true}.Marshal(req.Interface())
	if err != nil {
		return nil, err
	}
	key := string(wire)
	if replay, err := peer.replayTrialReceipt(1004022, key, out); replay || err != nil {
		if replay {
			role := s.roles.project(peer.currentRoleID(), []string{"Goods", "ShinyStone"})
			putPB(out, Record{"NowCapacity": len(goodsList(role)), "ShinyStone": roleInt(role, "ShinyStone")})
		}
		return nil, err
	}
	var result Record
	err = s.roles.Transact(func(tx *StoreTx) error {
		role := tx.Get(peer.currentRoleID())
		if role == nil {
			return previewReject(4007)
		}
		balance := roleInt(role, "ShinyStone")
		seen := make(map[string]bool, rows.Len())
		var count, earnings int64
		for i := 0; i < rows.Len(); i++ {
			line := rows.Get(i).Message()
			id, quantity := getString(line, "GoodsId"), getInt(line, "Count")
			g := locateGoods(role, id)
			if g == nil || seen[id] || quantity < 1 || quantity > transferStackCount(g) {
				return previewReject(4073)
			}
			if inventoryFlag(g, "IsLock") || inventoryFlag(g, "IsUsing") || inventoryFlag(g, "IsBinding") || hasFriendOfferLock(g) {
				return previewReject(4552)
			}
			if roleInt(role, "PracticeGuideVal") == 1 && roleString(g, "TemplateId") == practiceStarterMethodID {
				return previewReject(4074)
			}
			unit := roleInt(g, "RecyclePrice")
			if unit < 0 || unit > (math.MaxInt32-earnings)/quantity || count > math.MaxInt32-quantity {
				return previewReject(4008)
			}
			seen[id] = true
			earnings += quantity * unit
			count += quantity
			removeStorageQuantity(role, id, transferStackCount(g)-quantity)
		}
		if balance < 0 || balance > math.MaxInt64-earnings {
			return previewReject(4008)
		}
		role["ShinyStone"] = balance + earnings
		result = Record{"IsSuccess": true, "Code": 4077, "NowCapacity": len(goodsList(role)), "ShinyStone": balance + earnings, "SuccessCount": count, "AddShinyStone": earnings}
		return nil
	})
	if err == nil {
		peer.rememberTrialReceipt(1004022, key, result, out)
	}
	return nil, err
}
