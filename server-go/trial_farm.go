package main

import (
	"strconv"
	"time"

	"google.golang.org/protobuf/reflect/protoreflect"
)

// The client specifies the tilling cost and time. The trial crop has a short
// growth period and yields one plant. No irrigation recipe is configured.
const (
	farmTilling  = 10 * time.Minute
	farmGrowth   = 15 * time.Second
	farmCost     = 10
	farmCropName = "【试用】青芽"
	farmKey      = "TrialFarm"
)

func farmMap(row Record, key string) Record {
	value, _ := row[key].(Record)
	return value
}

func farmPlotState(plot Record, now int64) (status, remaining, total int64) {
	if plot == nil {
		return 0, 0, 0
	}
	if roleString(plot, "SeedId") != "" {
		remaining = max(0, roleInt(plot, "HarvestAt")-now)
		if remaining == 0 {
			return 6, 0, farmGrowth.Milliseconds()
		}
		return 5, remaining, farmGrowth.Milliseconds()
	}
	if remaining = roleInt(plot, "TilledAt") - now; remaining > 0 {
		return 1, remaining, farmTilling.Milliseconds()
	}
	if roleString(plot, "SoilId") != "" {
		return 3, 0, 0
	}
	return 2, 0, 0
}

func farmSoilUsedByOther(plots Record, id string) bool {
	for key := range plots {
		if key != id && roleString(farmMap(plots, key), "SoilId") == previewSoilID {
			return true
		}
	}
	return false
}

func trialFarmRead(cmd int) func(*Server, *Peer, protoreflect.Message, protoreflect.Message) (func(), error) {
	return func(s *Server, peer *Peer, req, out protoreflect.Message) (func(), error) {
		id := peer.currentRoleID()
		if reservedStoreID(id) {
			return nil, previewReject(4007)
		}
		role := s.roles.project(id, []string{"DongFuRank", farmKey})
		if role == nil {
			return nil, previewReject(4007)
		}
		limit := gameHomeRank(role) * 4
		if limit == 0 {
			return nil, previewReject(4008)
		}
		farm := farmMap(role, farmKey)
		plots := farmMap(farm, "Plots")
		switch cmd {
		case 1013001:
			now := time.Now().UnixMilli()
			for i := int64(1); i <= limit; i++ {
				plot := farmMap(plots, strconv.FormatInt(i, 10))
				if plot == nil {
					continue
				}
				state, remaining, total := farmPlotState(plot, now)
				row := addMessage(out, "Land")
				putPB(row, Record{"Id": i, "Status": state, "TimeRemaining": remaining, "TotalWasteTime": total})
				if roleString(plot, "SeedId") == previewSeedID {
					set(row, "PlantId", previewSeedID)
					set(row, "PlantName", farmCropName)
				}
			}
		case 1013004:
			i := getInt(req, "LandId")
			if i < 1 || i > limit {
				return nil, previewReject(5001)
			}
			key := strconv.FormatInt(i, 10)
			plot := farmMap(plots, key)
			status, _, _ := farmPlotState(plot, time.Now().UnixMilli())
			if status != 2 && status != 3 {
				return nil, previewReject(4375)
			}
			if roleString(plot, "SoilId") == previewSoilID {
				fillPreviewSoil(out.Mutable(field(out, "CurrentSoil")).Message())
			}
			if roleBool(farmMap(farm, "Soils"), previewSoilID) && !farmSoilUsedByOther(plots, key) {
				fillPreviewSoil(addMessage(out, "SoilAll"))
			}
		case 1013022:
			if roleBool(farmMap(farm, "Soils"), previewSoilID) {
				fillPreviewSoil(addMessage(out, "SoilAll"))
			}
		case 1013011:
			if roleBool(farmMap(farm, "Seeds"), previewSeedID) {
				previewSeeds(nil, nil, out)
			}
		}
		return nil, nil
	}
}

func trialFarmWrite(cmd int) func(*Server, *Peer, protoreflect.Message, protoreflect.Message) (func(), error) {
	return func(s *Server, peer *Peer, req, out protoreflect.Message) (func(), error) {
		return nil, s.changeFarm(peer.currentRoleID(), cmd, req, out, time.Now())
	}
}

func (s *Server) changeFarm(owner string, cmd int, req, out protoreflect.Message, now time.Time) error {
	if owner == "" || reservedStoreID(owner) {
		return previewReject(4007)
	}
	indexField := "LandId"
	if cmd == 1013003 {
		indexField = "LandIndex"
	}
	i := getInt(req, indexField)
	key := strconv.FormatInt(i, 10)
	return s.roles.Transact(func(tx *StoreTx) error {
		role := tx.View(owner)
		if role == nil {
			return previewReject(4007)
		}
		if i < 1 || i > gameHomeRank(role)*4 {
			return previewReject(5001)
		}
		farm := farmMap(role, farmKey)
		plots := farmMap(farm, "Plots")
		plot := farmMap(plots, key)
		status, _, _ := farmPlotState(plot, now.UnixMilli())
		var event string
		details := Record{"plot": i}
		switch cmd {
		case 1013003:
			if status != 0 {
				return previewReject(4376)
			}
			updated := tx.Get(owner)
			if !spendArtisanVigor(updated, farmCost) {
				return previewReject(4391)
			}
			farm = farmMap(updated, farmKey)
			if farm == nil {
				// One reusable nursery variety and one reusable soil lot per trial grower.
				farm = Record{"Seeds": Record{previewSeedID: true}, "Soils": Record{previewSoilID: true}, "Plots": Record{}}
				updated[farmKey] = farm
			}
			plots = farmMap(farm, "Plots")
			if plots == nil {
				return previewReject(4008)
			}
			plots[key] = Record{"TilledAt": now.Add(farmTilling).UnixMilli()}
			vigor := roleInt(updated, "CurrentVigor")
			putPB(out, Record{"LandStatus": 1, "CurrentVigor": vigor, "TimeRemaining": farmTilling.Milliseconds(), "TotalWasteTime": farmTilling.Milliseconds()})
			event = "farm.plot_tilled"
			details["vigorSpent"] = farmCost
		case 1013006:
			if status != 2 && status != 3 {
				return previewReject(4375)
			}
			soil := getString(req, "SoilId")
			if soil != previewSoilID || !roleBool(farmMap(farm, "Soils"), soil) || farmSoilUsedByOther(plots, key) {
				return previewReject(4008)
			}
			set(out, "LandStatus", 3)
			if roleString(plot, "SoilId") == soil {
				return nil
			}
			farmMap(farmMap(farmMap(tx.Get(owner), farmKey), "Plots"), key)["SoilId"] = soil
			event = "farm.soil_selected"
			details["soilId"] = soil
		case 1013013:
			if status != 3 || roleString(plot, "SoilId") != previewSoilID {
				return previewReject(4377)
			}
			seed := getString(req, "SeedId")
			if seed != previewSeedID || !roleBool(farmMap(farm, "Seeds"), seed) {
				return previewReject(4371)
			}
			plot = farmMap(farmMap(farmMap(tx.Get(owner), farmKey), "Plots"), key)
			plot["SeedId"], plot["HarvestAt"] = seed, now.Add(farmGrowth).UnixMilli()
			putPB(out, Record{"LandStatus": 5, "TimeRemaining": farmGrowth.Milliseconds(), "TotalWasteTime": farmGrowth.Milliseconds()})
			event = "farm.crop_planted"
			details["seedId"] = seed
		case 1013018:
			if status != 6 || roleString(plot, "SeedId") != previewSeedID {
				return previewReject(4379)
			}
			updated := tx.Get(owner)
			goods := Record{"Name": farmCropName, "Type": 12, "Rank": 1, "Level": 1, "FiveElement": 2, "TemplateId": previewSeedID, "MapName": "【试用】灵田", "Unit": "株", "Volume": 1.0, "ConversionRate": 1.0}
			goodsID, err := receiveInventoryGoods(updated, goods, 1)
			if err != nil {
				return err
			}
			delete(farmMap(farmMap(updated, farmKey), "Plots"), key)
			yield := addMessage(out, "PlantInfo")
			copyRolePB(yield, goods, "Name", "Type", "Rank", "Level", "FiveElement")
			set(yield, "Id", goodsID)
			for _, slot := range []string{"Live3", "Live4"} {
				if roleInt(role, slot+"Type") == 8 {
					set(out, "LiveLevel", roleInt(role, slot+"Level"))
					set(out, "LiveProficiency", roleInt(role, slot+"Value"))
					break
				}
			}
			statusPB(out, true, 4380)
			event = "farm.crop_harvested"
			details["goodsId"], details["seedId"], details["count"] = goodsID, previewSeedID, 1
		default:
			return previewReject(5001)
		}
		history := newHistoryEvent(event, role, nil, "farm_plot", key, details)
		history.Actor.RoleID = owner
		return tx.RecordHistory(history)
	})
}
