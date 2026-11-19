package main

import (
	"cmp"
	"math"
	"slices"
	"strings"
	"time"

	"google.golang.org/protobuf/reflect/protoreflect"
)

const commerceModule = "商店与活动"

func commercePreviewRoutes() []previewRoute {
	return []previewRoute{
		{cmd: 1002016, module: commerceModule, response: "SearchShinyStoneRatioResp_1002016", summary: "查询试用兑换汇率和可兑换总量", build: previewExchangeRate},
		{cmd: 1002018, module: commerceModule, response: "SearchSignInAwardResp_1002018", summary: "查看七日签到奖励示例", build: previewSignInAwards},
		{cmd: 1002024, module: commerceModule, request: "RealmRankedReq_1002024", response: "RealmRankedResp_1002024", summary: "按当前角色显示单机境界榜，其余类别为空", roleFields: []string{"Id", "Name", "Level", "Heat"}, build: previewRealmRanking},
		{cmd: 1004033, module: commerceModule, response: "EnterXuanTianLouResp_1004033", summary: "显示玄天楼限量商品价格和热销推荐", build: previewXuanTianLou},
		{cmd: 1006001, module: commerceModule, request: "EnterAuctionReq_1006001", response: "EnterAuctionResp_1006001", summary: "按分类、关键词及价格等条件查询奇珍阁试用挂牌", build: previewBrowseAuctions, live: (*Server).listPreviewAuction},
		{cmd: 1006003, module: commerceModule, request: "EnterMyAuctionReq_1006003", response: "EnterMyAuctionResp_1006003", summary: "我的挂牌空态及分页", build: previewMyAuctions},
		{cmd: 1006011, module: commerceModule, request: "EnterTreasureRoomReq_1006011", response: "EnterTreasureRoomResp_1006011", summary: "查看拍卖会试用拍品和场次", build: previewTreasureRoom, live: (*Server).listPreviewAuctionRoom},
		{cmd: 1006014, module: commerceModule, response: "OnShelfListResp_1006014", summary: "我的拍卖会挂牌空态", build: previewNoListings},
		{cmd: 1006017, module: commerceModule, request: "EnterTreasureReq_1006017", response: "EnterTreasureResp_1006017", summary: "进入拍卖场次并查询当前灵石和竞价状态", roleFields: []string{"ShinyStone"}, build: previewEnterTreasure, live: (*Server).enterPreviewAuction},
		{cmd: 1006018, module: commerceModule, response: "TreasureGoodsListResp_1006018", summary: "查看拍卖场次的拍品、现价和完成状态", build: previewTreasureGoods, live: (*Server).listPreviewAuctionLots},
		{cmd: 1006021, module: commerceModule, response: "TreasureCurrentGoodsResp_1006021", summary: "查看当前试用拍品、价格和出价者", build: previewCurrentTreasure, live: (*Server).currentPreviewAuctionLot},
		{cmd: 1008002, module: commerceModule, request: "EnterRushTowerTierNumRankedReq_1008002", response: "EnterRushTowerTierNumRankedResp_1008002", summary: "显示虚神塔道境榜示例", build: previewTowerLevelRanking},
		{cmd: 1008004, module: commerceModule, request: "EnterRushTowerAgeRankedReq_1008004", response: "EnterRushTowerAgeRankedResp_1008004", summary: "显示虚神塔天骄榜示例", build: previewTowerAgeRanking},
		{cmd: 1017001, module: commerceModule, request: "EnterChouJiangChiReq_1017001", response: "EnterChouJiangChiResp_1017001", summary: "查看试用抽奖池的次数和积分", build: previewEventPool},
		{cmd: 1017004, module: commerceModule, response: "EnterJiFenResp_1017004", summary: "查看试用积分兑换档位", build: previewPointAwards},

		{cmd: 1002017, module: commerceModule, request: "ShinyJadeExchangeShinyStoneReq_1002017", response: "ShinyJadeExchangeShinyStoneResp_1002017", summary: "灵玉兑换灵石", blocked: true},
		{cmd: 1002021, module: commerceModule, response: "GetSignInAwardResp_1002021", summary: "领取签到奖励（签到窗口自动发出）", blocked: true},
		{cmd: 1002026, module: commerceModule, request: "MoBaiReq_1002026", response: "MoBaiResp_1002026", summary: "膜拜排行榜", blocked: true},
		{cmd: 1004035, module: commerceModule, request: "BuyXuanTianLouGoodsReq_1004035", response: "BuyXuanTianLouGoodsResp_1004035", summary: "购买玄天楼物品", blocked: true},
		{cmd: 1006008, module: commerceModule, request: "EntrustAuctionReq_1006008", response: "EntrustAuctionResp_1006008", summary: "委托奇珍阁上架", blocked: true},
		{cmd: 1006009, module: commerceModule, request: "AuctionGoodsCancelReq_1006009", response: "AuctionGoodsCancelResp_1006009", summary: "撤销奇珍阁挂牌", blocked: true},
		{cmd: 1006010, module: commerceModule, request: "AuctionBuyReq_1006010", response: "AuctionBuyResp_1006010", summary: "买入奇珍阁的限量试用丹药", blocked: true, live: (*Server).buyPreviewAuction},
		{cmd: 1006013, module: commerceModule, request: "OnShelfTreasureReq_1006013", response: "OnShelfTreasureResp_1006013", summary: "上架拍卖会拍品", blocked: true},
		{cmd: 1006016, module: commerceModule, request: "TreasureGoodsCancelReq_1006016", response: "TreasureGoodsCancelResp_1006016", summary: "撤销拍卖会挂牌", blocked: true},
		{cmd: 1006020, module: commerceModule, request: "TreasureBuyReq_1006020", response: "TreasureBuyResp_1006020", summary: "拍卖会竞价，超价返还及到期结算", blocked: true, live: (*Server).bidPreviewAuction},
		{cmd: 1008037, module: commerceModule, request: "RushTowerTierNumRankedAwardReq_1008037", response: "RushTowerTierNumRankedAwardResp_1008037", summary: "领取道境榜奖励", blocked: true},
		{cmd: 1008038, module: commerceModule, request: "RushTowerAgeRankedAwardReq_1008038", response: "RushTowerAgeRankedAwardResp_1008038", summary: "领取天骄榜奖励", blocked: true},
		{cmd: 1017002, module: commerceModule, request: "ChouJiangReq_1017002", response: "ChouJiangResp_1017002", summary: "抽奖", blocked: true},
		{cmd: 1017006, module: commerceModule, request: "GetJiFenReq_1017006", response: "GetJiFenResp_1017006", summary: "兑换积分奖励", blocked: true},
	}
}

func previewExchangeRate(_ Record, _, out protoreflect.Message) {
	// The exchange button stays disabled because there is no live liquidity.
	fixtureTime := time.Date(2026, time.November, 18, 0, 0, 0, 0, time.UTC).UnixMilli()
	putPB(out, Record{"ExchangeRatio": 100, "ExchangeShinyStoneSum": 0, "UpdateAt": fixtureTime})
}

func previewSignInAwards(_ Record, _, out protoreflect.Message) {
	for day, amount := range [...]int{50, 80, 100, 120, 150, 180, 300} {
		entry := addMessage(out, "SignInAward")
		set(entry, "SignInDay", day+1)
		putPB(addMessage(entry, "Award"), Record{"GoodsName": "灵石（试用）", "Num": amount, "Type": 100, "Rank": 4})
	}
}

func previewXuanTianLou(_ Record, _, out protoreflect.Message) {
	// IDs and prices match the bundled xuantianlougoods.json so Unity can
	// resolve them against its local goods dictionary.
	for _, id := range [...]int{21, 22, 23} {
		putPB(addMessage(out, "Goods"), Record{"Id": id, "SystemPrice": 20, "Count": 0})
	}
	for _, id := range [...]int32{1, 21, 22, 23} {
		out.Mutable(field(out, "ReXiaoGoodsId")).List().Append(protoreflect.ValueOfInt32(id))
	}
}

type previewListing struct {
	id, template, name  string
	shop, kind, element int64
	rank, stage, level  int64
	price               int64
}

// These are read-only fixtures. The writable market uses only the bundled elixir.
var sampleAuctionListings = [...]previewListing{
	{id: "preview-auction-sign", template: "preview-sign", name: "试用·轻身符", shop: 81, kind: 21, element: 2, rank: 4, stage: 1, level: 1, price: 120},
	previewFixedAuctionListing,
	{id: "preview-auction-armor", template: "preview-armor", name: "试用·青布道袍", shop: 83, kind: 26, element: 2, rank: 4, stage: 1, level: 1, price: 260},
	{id: "preview-auction-blade", template: "wu_xing_ju_ling_jian", name: "试用·五行聚灵剑", shop: 83, kind: 15, rank: 12, stage: 1, level: 1, price: 1200},
	{id: "preview-auction-matrix", template: "preview-matrix", name: "试用·聚灵阵", shop: 84, kind: 20, element: 5, rank: 4, stage: 1, level: 1, price: 350},
	{id: "preview-auction-method", template: "preview-method", name: "试用·启灵经", shop: 86, kind: 1, element: 2, rank: 4, stage: 1, level: 1, price: 250},
	{id: "preview-auction-skill", template: "preview-skill", name: "试用·引雷术", shop: 87, kind: 6, element: 4, rank: 4, stage: 1, level: 1, price: 280},
	{id: "preview-auction-stone", template: "wu_dao_shi_1", name: "试用·一阶悟道石", shop: 88, kind: 34, rank: 16, stage: 1, level: 1, price: 250},
}

type previewAuctionFilter struct {
	shop, kind, element, stage, rank, minPrice, maxPrice int64
	name                                                 string
}

func (f previewAuctionFilter) matches(item previewListing) bool {
	return (f.shop <= 0 || f.shop == item.shop) &&
		(f.kind <= 0 || f.kind == item.kind) &&
		(f.element <= 0 || f.element == item.element) &&
		(f.stage <= 0 || f.stage == item.stage) &&
		(f.rank <= 0 || f.rank == item.rank) &&
		(f.minPrice <= 0 || f.minPrice <= item.price) &&
		(f.maxPrice <= 0 || item.price <= f.maxPrice) &&
		strings.Contains(item.name, f.name)
}

func previewBrowseAuctions(_ Record, req, out protoreflect.Message) {
	fillPreviewAuctionListings(req, out, sampleAuctionListings[:])
}

func fillPreviewAuctionListings(req, out protoreflect.Message, listings []previewListing) {
	filter := previewAuctionFilter{
		shop: getInt(req, "ShopType"), kind: getInt(req, "GoodsType"), element: getInt(req, "FiveElement"),
		stage: getInt(req, "Stage"), rank: getInt(req, "Rank"), minPrice: getInt(req, "MinFinalPrice"),
		maxPrice: getInt(req, "MaxFinalPrice"), name: strings.TrimSpace(getString(req, "Name")),
	}
	var rows []previewListing
	if getInt(req, "Type") <= 0 {
		// Functional subtypes in the live auction are not available offline.
		for _, item := range listings {
			if filter.matches(item) {
				rows = append(rows, item)
			}
		}
	}
	priceOrder, rankOrder := getString(req, "FinalPriceSort"), getString(req, "RankSort")
	slices.SortStableFunc(rows, func(a, b previewListing) int {
		if price := previewSortOrder(cmp.Compare(a.price, b.price), priceOrder); price != 0 {
			return price
		}
		return previewSortOrder(cmp.Compare(a.rank, b.rank), rankOrder)
	})
	page, size := previewAuctionPage(req, out, len(rows))
	for _, item := range rows[(page-1)*size : min(page*size, len(rows))] {
		putPB(addMessage(out, "Object"), Record{
			"Id": item.id, "GoodsId": item.template, "Name": item.name, "GoodsType": item.kind,
			"FiveElement": item.element, "FixedPrice": item.price, "RemainHours": 24,
			"Stage": item.stage, "Rank": item.rank, "Level": item.level, "Count": 1,
		})
	}
}

func previewSortOrder(order int, direction string) int {
	switch direction {
	case "asc":
		return order
	case "desc":
		return -order
	default:
		return 0
	}
}

func previewAuctionPage(req, out protoreflect.Message, total int) (page, size int) {
	size = int(getInt(req, "PageSize"))
	if size <= 0 || size > 10 {
		size = 10
	}
	pages := max(1, (total+size-1)/size)
	page = min(max(int(getInt(req, "PageNum")), 1), pages)
	putPB(out, Record{"PageNum": page, "PageSize": size, "Pages": pages})
	return page, size
}

func previewMyAuctions(_ Record, req, out protoreflect.Message) {
	previewAuctionPage(req, out, 0)
}

func previewNoListings(Record, protoreflect.Message, protoreflect.Message) {}

const (
	// The original client resolves this elixir's details from XuanTianLouGoods.json.
	previewTreasureTemplate = "sys_zhu_ji_dan_huang"
	previewTreasureName     = "试用·筑基丹"
	previewTreasureRank     = 4
	previewTreasurePrice    = 350
)

func previewTreasureRoom(_ Record, req, out protoreflect.Message) {
	set(out, "Stage", max(1, getInt(req, "Stage")))
	putPB(addMessage(out, "GoodsList"), Record{"GoodsName": previewTreasureName, "Rank": previewTreasureRank, "StartPrice": previewTreasurePrice})
}

func previewEnterTreasure(role Record, req, out protoreflect.Message) {
	value := roleInt(role, "ShinyStone")
	if value < 0 || value > math.MaxInt32 {
		// The client assigns this int32 response directly to its displayed balance.
		statusPB(out, false, previewDeniedCode)
		return
	}
	putPB(out, Record{"TreasureId": max(1, getInt(req, "Stage")), "IsMayAddPrice": false, "ShinyStone": value})
}

func previewTreasureGoods(_ Record, _, out protoreflect.Message) {
	set(out, "Stage", 1)
	putPB(addMessage(out, "GoodsList"), Record{"GoodsName": previewTreasureName, "Rank": previewTreasureRank, "FinalPrice": previewTreasurePrice, "Status": 0})
}

func previewCurrentTreasure(_ Record, _, out protoreflect.Message) {
	putPB(out, Record{
		"Id": 1, "GoodsId": previewTreasureTemplate, "GoodsName": previewTreasureName, "GoodsType": 19,
		"Level": 1, "Rank": previewTreasureRank, "Stage": 1, "StartPrice": previewTreasurePrice, "CurrentPrice": previewTreasurePrice,
	})
}

func previewRealmRanking(role Record, req, out protoreflect.Message) {
	id := roleString(role, "Id")
	if getInt(req, "Type") != -1 || id == "" {
		return
	}
	heat := min(max(0, roleInt(role, "Heat")), math.MaxInt32)
	putPB(out, Record{"MyRanked": 1, "MyHeat": heat})
	level := min(max(1, roleInt(role, "Level")), 90)
	putPB(addMessage(out, "Object"), Record{"Id": id, "Name": roleString(role, "Name"), "Level": level, "Heat": heat})
}

func previewTowerLevelRanking(_ Record, req, out protoreflect.Message) {
	stage := min(max(getInt(req, "Stage"), 1), 9)
	putPB(addMessage(out, "Object"), Record{"Name": "试用·凌云", "Faction": "试用", "Level": (stage-1)*10 + 1, "TierNum": 3})
}

func previewTowerAgeRanking(_ Record, _, out protoreflect.Message) {
	putPB(addMessage(out, "Object"), Record{"Name": "试用·青禾", "Faction": "试用", "Age": 18, "TierNum": 2})
}

func previewEventPool(_ Record, _, out protoreflect.Message) {
	putPB(out, Record{"CurrentChouJiangJiFen": 0, "Count": 0})
}

func previewPointAwards(_ Record, _, out protoreflect.Message) {
	for index, points := range [...]int{10, 20, 30, 40, 50, 60, 80, 100, 120, 150, 200} {
		putPB(addMessage(out, "JiFen"), Record{"Id": index + 1, "JiFen": points, "Name": "灵石（试用）", "Count": points * 10, "IsGet": false})
	}
}
