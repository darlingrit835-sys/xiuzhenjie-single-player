package main

import (
	"encoding/json"
	"errors"
	"fmt"
	"log"
	"strconv"
	"strings"
	"time"
)

const (
	gardenCommand       = 4901061
	gardenStorePrefix   = "$garden:"
	gardenWardPrice     = 6
	gardenDailyAttempts = 3
)

type gardenRequest struct {
	Action    string `json:"action"`
	RequestID string `json:"requestId"`
	Plot      int    `json:"plot"`
	Seed      string `json:"seed"`
	Soil      string `json:"soil"`
	Target    string `json:"target"`
	Scope     string `json:"scope"`
	CropID    string `json:"cropId"`
	KeepSeed  bool   `json:"keepSeed"`
}

type gardenSeed struct {
	id, name, material, use, soil           string
	price, seconds, yield, element, starter int
}

var gardenSeeds = []gardenSeed{
	{"dew", "凝露草", "mat.ninglu_herb", "叶入凝露丹、青脉丹，晨露研符墨", "loam", 12, 60, 4, 2, 2},
	{"reed", "霜节芦", "mat.frost_reed", "芦纤抄符纸，也是霜脉清毒丹的主药", "sand", 28, 300, 4, 1, 1},
	{"lotus", "梦泽莲", "mat.dream_lotus", "莲汁制符墨，花瓣可作炼丹辅材", "silt", 54, 900, 5, 3, 0},
}

type gardenSoil struct {
	id, name, description string
	cost                  int
}

var gardenSoils = []gardenSoil{
	{"loam", "青壤", "适合凝露草", 0},
	{"sand", "寒砂土", "适合霜节芦", 8},
	{"silt", "灵淤泥", "适合梦泽莲", 14},
}

func gardenVariety(id string) *gardenSeed {
	for i := range gardenSeeds {
		if gardenSeeds[i].id == id {
			return &gardenSeeds[i]
		}
	}
	return nil
}

func gardenSoilByID(id string) *gardenSoil {
	for i := range gardenSoils {
		if gardenSoils[i].id == id {
			return &gardenSoils[i]
		}
	}
	return nil
}

func (seed gardenSeed) growSeconds(role Record) int {
	rank, _, _ := artisanTraining(role, 8, "GardenRank", "GardenExperience")
	return (seed.seconds*(100-2*max(0, rank-1)) + 50) / 100
}

func gardenWardCost(role Record) int64 {
	return int64(gardenWardPrice - (artisanResearchRank(role, 5)+1)/3)
}

func gardenStock(role Record, seed *gardenSeed) int64 {
	stock := farmMap(role, "GardenSeeds")
	if stock == nil || stock[seed.id] == nil {
		return int64(seed.starter)
	}
	return roleInt(stock, seed.id)
}

func gardenPlots(role Record) Record { return farmMap(farmMap(role, farmKey), "Plots") }

func gardenLimit(role Record) int { return max(1, int(gameHomeRank(role))) * 4 }

func gardenSoilAt(role Record, key string) string {
	if id := roleString(farmMap(role, "GardenSoils"), key); gardenSoilByID(id) != nil {
		return id
	}
	return "loam"
}

func gardenCropID(plot Record) string {
	if roleString(plot, "SeedId") == previewSeedID {
		return fmt.Sprintf("qingya-%d", roleInt(plot, "HarvestAt"))
	}
	return roleString(plot, "CropId")
}

func gardenYield(plot Record) int64 {
	if roleString(plot, "SeedId") == previewSeedID {
		return 1
	}
	return roleInt(plot, "GardenYield") - roleInt(plot, "Picked")
}

func gardenDay(now time.Time) int64 { return (now.Unix() + 8*3600) / 86400 }

func gardenAttempts(role Record, now time.Time) int64 {
	if roleInt(role, "GardenVisitDay") != gardenDay(now) {
		return gardenDailyAttempts
	}
	return max(0, gardenDailyAttempts-roleInt(role, "GardenVisitCount"))
}

func gardenCanPick(plot Record, visitorID string, attempts int64, now time.Time) bool {
	return visitorID != "" && gardenVariety(roleString(plot, "SeedId")) != nil &&
		now.UnixMilli() >= roleInt(plot, "HarvestAt") && roleInt(plot, "Picked") == 0 &&
		attempts > 0 && !roleBool(farmMap(plot, "Visitors"), visitorID)
}

func gardenState(actorID, ownerID string, actor, owner Record, now time.Time) Record {
	own := actorID == ownerID
	attempts := gardenAttempts(actor, now)
	rows := []Record{}
	for i := 1; i <= gardenLimit(owner); i++ {
		key := strconv.Itoa(i)
		plot := farmMap(gardenPlots(owner), key)
		seedID := roleString(plot, "SeedId")
		row := Record{"id": i, "soil": gardenSoilAt(owner, key), "state": "empty", "remaining": 0, "yield": 0, "ward": false, "wardUsed": false, "canSteal": false}
		if id := gardenCropID(plot); id != "" {
			left := (max(0, roleInt(plot, "HarvestAt")-now.UnixMilli()) + 999) / 1000
			name := "青芽"
			if seed := gardenVariety(seedID); seed != nil {
				name = seed.name
			}
			row["seed"], row["name"], row["cropId"] = seedID, name, id
			row["state"], row["remaining"], row["yield"] = "growing", left, gardenYield(plot)
			row["ward"], row["wardUsed"] = roleBool(plot, "Ward"), roleBool(plot, "WardUsed")
			row["canWard"] = own && gardenVariety(seedID) != nil && !roleBool(plot, "WardUsed") && roleInt(plot, "Picked") == 0
			row["canKeepSeed"] = own && left == 0 && gardenVariety(seedID) != nil && gardenYield(plot) > 2
			if left == 0 {
				row["state"] = "ripe"
			}
			row["canSteal"] = !own && gardenCanPick(plot, actorID, attempts, now)
		}
		rows = append(rows, row)
	}
	out := Record{"own": own, "owner": ownerID, "name": roleString(owner, "Name"), "serverTime": now.Unix(), "plots": rows, "attemptsLeft": attempts}
	if own {
		var seeds, soils []Record
		for _, seed := range gardenSeeds {
			seeds = append(seeds, Record{"id": seed.id, "name": seed.name, "material": seed.material, "use": seed.use, "price": seed.price, "count": gardenStock(owner, &seed), "seconds": seed.growSeconds(owner), "yield": seed.yield, "soil": seed.soil})
		}
		for _, soil := range gardenSoils {
			soils = append(soils, Record{"id": soil.id, "name": soil.name, "description": soil.description, "cost": soil.cost})
		}
		logs, _ := owner["GardenLogs"].([]any)
		if logs == nil {
			logs = []any{}
		}
		out["seeds"], out["soils"], out["logs"] = seeds, soils, logs
		out["rank"], out["training"], _ = artisanTraining(owner, 8, "GardenRank", "GardenExperience")
		out["stones"], out["wardCost"], out["wardCharges"] = roleInt(owner, "ShinyStone"), gardenWardCost(owner), roleInt(owner, "TalismanFieldWard")
	}
	return out
}

// Old trial plots and the new garden share their occupied plots. A harvest from
// either client consumes one crop in the same transaction.
func gardenPutCrop(role Record, key string, crop Record) {
	farm := farmMap(role, farmKey)
	if farm == nil {
		farm = Record{"Seeds": Record{previewSeedID: true}, "Soils": Record{previewSoilID: true}}
		role[farmKey] = farm
	}
	putFriendEntry(farm, "Plots", key, crop)
}

func gardenSow(role Record, key string, req gardenRequest, now time.Time) (string, error) {
	seed, soil := gardenVariety(req.Seed), gardenSoilByID(req.Soil)
	if seed == nil || soil == nil {
		return "", forgeFailure("请先选好种子和土壤")
	}
	if roleString(farmMap(gardenPlots(role), key), "SeedId") != "" {
		return "", forgeFailure("这块地中已有灵植")
	}
	if gardenStock(role, seed) < 1 {
		return "", forgeFailure("这袋种子已经用完")
	}
	price := 0
	if gardenSoilAt(role, key) != soil.id {
		price = soil.cost
	}
	if roleInt(role, "ShinyStone") < int64(price) {
		return "", forgeFailure("灵石不足以更换这畦灵土")
	}
	seconds, yield := seed.growSeconds(role), seed.yield
	if soil.id == seed.soil {
		seconds, yield = seconds*4/5, yield+1
	}
	role["ShinyStone"] = roleInt(role, "ShinyStone") - int64(price)
	putFriendEntry(role, "GardenSeeds", seed.id, gardenStock(role, seed)-1)
	putFriendEntry(role, "GardenSoils", key, soil.id)
	gardenPutCrop(role, key, Record{"CropId": newGameID(12), "SeedId": seed.id, "SoilId": soil.id, "HarvestAt": now.Add(time.Duration(seconds) * time.Second).UnixMilli(), "GardenYield": yield})
	return fmt.Sprintf("第 %s 畦已播下%s，预计收获 %d 株", key, seed.name, yield), nil
}

func gardenProduce(plot Record) Record {
	seed := gardenVariety(roleString(plot, "SeedId"))
	if seed == nil {
		return Record{"Name": farmCropName, "Type": 12, "Rank": 1, "Level": 1, "FiveElement": 2, "TemplateId": previewSeedID, "MapName": "【试用】灵田", "Unit": "株", "Volume": 1.0, "ConversionRate": 1.0}
	}
	return Record{"BrewSource": "garden", "TemplateId": seed.material, "Name": seed.name, "Type": 12, "Rank": 2, "Level": 1, "Stage": 1, "FiveElement": seed.element, "Volume": 0.1, "ConversionRate": 1.0, "RecyclePrice": 0, "IsUsing": false, "IsBinding": false, "IsLock": false}
}

func gardenHarvest(role, plot Record, key string, keep bool, now time.Time) (string, error) {
	if now.UnixMilli() < roleInt(plot, "HarvestAt") {
		return "", forgeFailure("灵植尚未成熟")
	}
	yield := gardenYield(plot)
	if keep {
		seed := gardenVariety(roleString(plot, "SeedId"))
		if seed == nil || yield <= 2 {
			return "", forgeFailure("这一茬不足以留种")
		}
		putFriendEntry(role, "GardenSeeds", seed.id, gardenStock(role, seed)+1)
		yield -= 2
	}
	goods := gardenProduce(plot)
	if _, err := receiveInventoryGoods(role, goods, yield); err != nil {
		return "", forgeFailure("纳戒已满，请腾出空位再来采收")
	}
	rankLabel := ""
	if seed := gardenVariety(roleString(plot, "SeedId")); seed != nil {
		weight := 20
		if seed.id == "reed" {
			weight = 55
		} else if seed.id == "lotus" {
			weight = 100
		}
		rank, xp := artisanAddLesson(role, 8, "GardenRank", "GardenExperience", weight)
		rankLabel = fmt.Sprintf(" · 灵植%d品 %d/%d", rank, xp, sigilRanks[rank])
	}
	delete(gardenPlots(role), key)
	message := fmt.Sprintf("%s ×%d 已收入纳戒", roleString(goods, "Name"), yield)
	if keep {
		message += "，另留种一粒"
	}
	return message + rankLabel, nil
}

func gardenBuy(role Record, id string) (string, error) {
	seed := gardenVariety(id)
	if seed == nil {
		return "", forgeFailure("种圃没有这袋种子")
	}
	if roleInt(role, "ShinyStone") < int64(seed.price) {
		return "", forgeFailure("购买种子的灵石不足")
	}
	role["ShinyStone"] = roleInt(role, "ShinyStone") - int64(seed.price)
	putFriendEntry(role, "GardenSeeds", id, gardenStock(role, seed)+1)
	return fmt.Sprintf("以 %d 灵石购得%s种子一粒", seed.price, seed.name), nil
}

func gardenWard(role, plot Record) (string, error) {
	if gardenVariety(roleString(plot, "SeedId")) == nil {
		return "", forgeFailure("这畦青芽无需布阵")
	}
	if roleInt(plot, "Picked") != 0 {
		return "", forgeFailure("这茬已经被偷采过一次，余下灵植受保护")
	}
	if roleBool(plot, "WardUsed") {
		return "", forgeFailure("这茬灵植已经布过阵")
	}
	price := gardenWardCost(role)
	if roleInt(role, "ShinyStone") < price {
		return "", forgeFailure(fmt.Sprintf("布阵需要 %d 灵石", price))
	}
	role["ShinyStone"] = roleInt(role, "ShinyStone") - price
	plot["Ward"], plot["WardUsed"] = true, true
	return "已布守田阵，可拦下本畦第一次偷采", nil
}

func gardenLog(role Record, line string) {
	logs, _ := role["GardenLogs"].([]any)
	role["GardenLogs"] = append([]any{line}, logs[:min(len(logs), 11)]...)
}

func gardenSteal(actorID string, actor, owner, plot Record, key string, now time.Time) (string, error) {
	remaining := gardenAttempts(actor, now)
	if !gardenCanPick(plot, actorID, remaining, now) {
		return "", forgeFailure("今日次数已尽，或这茬灵植已不能再偷采")
	}
	item := gardenProduce(plot)
	var outcome string
	switch {
	case roleBool(plot, "Ward"):
		plot["Ward"] = false
		outcome = "被守田阵拦下，灵植无损"
	case roleInt(owner, "TalismanFieldWard") > 0:
		owner["TalismanFieldWard"] = roleInt(owner, "TalismanFieldWard") - 1
		outcome = "触发护田符，灵植无损"
	default:
		if _, err := receiveInventoryGoods(actor, item, 1); err != nil {
			return "", forgeFailure("纳戒已满，腾出空位才能采下灵植")
		}
		plot["Picked"] = 1
		outcome = "偷采" + roleString(item, "Name") + "一株"
	}
	putFriendEntry(plot, "Visitors", actorID, true)
	actor["GardenVisitDay"], actor["GardenVisitCount"] = gardenDay(now), gardenDailyAttempts-remaining+1
	line := fmt.Sprintf("%s 来访第 %s 畦，%s", roleString(actor, "Name"), key, outcome)
	gardenLog(owner, line)
	gardenLog(actor, fmt.Sprintf("拜访%s的第 %s 畦，%s", roleString(owner, "Name"), key, outcome))
	return outcome, nil
}

func gardenChange(actorID string, actor, owner Record, req gardenRequest, now time.Time) (string, error) {
	if req.Action == "buy" {
		return gardenBuy(actor, req.Seed)
	}
	if req.Plot < 1 || req.Plot > gardenLimit(owner) {
		return "", forgeFailure("没有这块灵田")
	}
	key := strconv.Itoa(req.Plot)
	if req.Action == "plant" {
		return gardenSow(owner, key, req, now)
	}
	plot := farmMap(gardenPlots(owner), key)
	if req.CropID == "" || gardenCropID(plot) != req.CropID {
		return "", forgeFailure("这畦作物已经变化，请重新查看")
	}
	switch req.Action {
	case "ward":
		return gardenWard(owner, plot)
	case "harvest":
		return gardenHarvest(owner, plot, key, req.KeepSeed, now)
	case "steal":
		return gardenSteal(actorID, actor, owner, plot, key, now)
	default:
		return "", forgeFailure("没有这项灵田操作")
	}
}

func (s *Server) gardenAction(actorID string, req gardenRequest, now time.Time) (Record, error) {
	var reply Record
	err := s.roles.Transact(func(tx *StoreTx) error {
		actor := tx.View(actorID)
		if actorID == "" || strings.HasPrefix(actorID, "$") || actor == nil {
			return forgeFailure("请先进入角色")
		}
		ownerID := actorID
		if req.Action == "visit" || req.Action == "steal" {
			if req.Target == actorID || req.Target == "" || strings.HasPrefix(req.Target, "$") {
				return forgeFailure("请选择别人的灵田")
			}
			ownerID = req.Target
		} else if req.Target != "" {
			return forgeFailure("只能管理自己的灵田")
		}
		owner := tx.View(ownerID)
		if owner == nil {
			return forgeFailure("未找到这位修士的灵田")
		}
		switch req.Action {
		case "state", "visit":
			reply = gardenState(actorID, ownerID, actor, owner, now)
			return nil
		case "neighbors":
			if req.Scope != "friends" && req.Scope != "strangers" {
				return forgeFailure("请选择道友或陌生人的灵田")
			}
			reply = gardenNeighbors(tx, actorID, actor, req.Scope, now)
			return nil
		case "buy", "plant", "harvest", "ward", "steal":
		default:
			return forgeFailure("没有这项灵田操作")
		}
		if !validOfferToken(req.RequestID) {
			return forgeFailure("灵田操作单号无效")
		}
		receiptID := gardenStorePrefix + smithingTemplateID(actorID, req.RequestID)
		order, _ := json.Marshal(req)
		message := ""
		if receipt := tx.View(receiptID); receipt != nil {
			if roleString(receipt, "Order") != string(order) {
				return forgeFailure("这张灵田回执对应另一项操作")
			}
			message = roleString(receipt, "Message")
		} else {
			actor, owner = tx.Get(actorID), tx.Get(ownerID)
			var err error
			message, err = gardenChange(actorID, actor, owner, req, now)
			if err != nil {
				return err
			}
			receipt := tx.GetOrCreate(receiptID)
			receipt["Order"], receipt["Message"] = string(order), message
		}
		reply = gardenState(actorID, ownerID, actor, owner, now)
		reply["message"] = message
		return nil
	})
	return reply, err
}

func (s *Server) handleGarden(peer *Peer, msg Message) {
	id := peer.currentRoleID()
	active, online := s.online.Load(id)
	if !online || active != peer || id == "" || reservedStoreID(id) {
		sendAuctionJSON(peer, msg, 4008, Record{"error": "灵田暂未开放，或角色已离线"})
		return
	}
	var req gardenRequest
	if len(msg.Data) > 2048 || json.Unmarshal([]byte(offerToken(msg.Data)), &req) != nil {
		sendAuctionJSON(peer, msg, 4008, Record{"error": "灵田请求格式有误"})
		return
	}
	if s.preview != nil && !s.previewWrites && req.Action != "state" && req.Action != "neighbors" && req.Action != "visit" {
		sendAuctionJSON(peer, msg, 4008, Record{"error": "当前为只读灵田"})
		return
	}
	reply, err := s.gardenAction(id, req, time.Now())
	if err == nil {
		sendAuctionJSON(peer, msg, 2000, reply)
		return
	}
	var refused forgeFailure
	if errors.As(err, &refused) {
		sendAuctionJSON(peer, msg, 4008, Record{"error": string(refused)})
		return
	}
	log.Printf("灵田存档: %v", err)
	sendAuctionJSON(peer, msg, 5000, Record{"error": "灵田存档失败，请重试"})
}
