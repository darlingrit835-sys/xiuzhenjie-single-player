package main

import "google.golang.org/protobuf/reflect/protoreflect"

// The isolated farm uses the same trial variety and soil as the read-only preview.
const (
	previewPlotID = 1
	previewSoilID = "preview-soil-1"
	previewSeedID = "preview-seed-1"
)

func craftingPreviewRoutes() []previewRoute {
	const module = "灵田与生产"
	return []previewRoute{
		{cmd: 1013001, module: module, response: "EnterPlantResp_1013001",
			summary: "按角色读取地块、作物和剩余时间", build: previewPlots, live: trialFarmRead(1013001)},
		{cmd: 1013004, module: module, request: "EnterSoilReq_1013004", response: "EnterSoilResp_1013004",
			summary: "按地块读取已配置和可选择的试用灵土", build: previewPlotSoil, live: trialFarmRead(1013004)},
		{cmd: 1013011, module: module, response: "EnterSeedResp_1013011",
			summary: "读取本角色育种室的试用品种", build: previewSeeds, live: trialFarmRead(1013011)},
		{cmd: 1011003, module: module, request: "TemplateMaterialPlantDetailReq_1011003", response: "TemplateMaterialPlantDetailResp_1011003",
			summary: "查看试用种子的产地、五行与材料属性", build: previewSeedDetail},
		{cmd: 1013022, module: module, response: "SearchSoilAllResp_1013022",
			summary: "读取本角色的试用灵土", build: previewSoils, live: trialFarmRead(1013022)},
		{cmd: 1008043, module: module, response: "EnterSignUnionResp_1008043",
			summary: "神符会贡献空态", build: previewEmptyContribution},
		{cmd: 1008054, module: module, response: "EnterPlantAssociationResp_1008054",
			summary: "灵植协会贡献空态", build: previewEmptyContribution},
		{cmd: 1008066, module: module, response: "EnterEquipmentAssociationResp_1008066",
			summary: "炼器协会贡献及可用工坊入口", build: previewEmptyContribution, live: (*Server).smithingAssociation},
		{cmd: 1013003, module: module, request: "KaiKenLandReq_1013003", response: "KaiKenLandResp_1013003",
			summary: "开垦地块", blocked: true, live: trialFarmWrite(1013003)},
		{cmd: 1013006, module: module, request: "ChooseSoilReq_1013006", response: "ChooseSoilResp_1013006",
			summary: "更换灵土", blocked: true, live: trialFarmWrite(1013006)},
		{cmd: 1013007, module: module, request: "SoilDeployReq_1013007", response: "SoilDeployResp_1013007",
			summary: "配置灵土", blocked: true},
		{cmd: 1013010, module: module, request: "DiscardSoilReq_1013010", response: "DiscardSoilResp_1013010",
			summary: "移除灵土", blocked: true},
		{cmd: 1013013, module: module, request: "SeedChooseReq_1013013", response: "SeedChooseResp_1013013",
			summary: "种植种子", blocked: true, live: trialFarmWrite(1013013)},
		{cmd: 1013017, module: module, request: "WaterDeployReq_1013017", response: "WaterDeployResp_1013017",
			summary: "配置灵液", blocked: true},
		{cmd: 1013018, module: module, request: "HarvestPlantReq_1013018", response: "HarvestPlantResp_1013018",
			summary: "收获灵植", blocked: true, live: trialFarmWrite(1013018)},
		{cmd: 1013020, module: module, request: "ForgetSeedReq_1013020", response: "ForgetSeedResp_1013020",
			summary: "遗忘种子", blocked: true},
	}
}

func previewPlots(_ Record, _, out protoreflect.Message) {
	plot := addMessage(out, "Land")
	set(plot, "Id", previewPlotID)
	set(plot, "Status", 3) // The client shows the soil and seed selectors.
}

func fillPreviewSoil(out protoreflect.Message) {
	set(out, "Id", previewSoilID)
	set(out, "Name", "【试用】青壤")
	set(out, "FiveElement", 2)
	set(out, "Level", 1)
	set(out, "Rank", 1)
	set(out, "ConversionRate", 1.0)
}

func previewPlotSoil(_ Record, req, out protoreflect.Message) {
	if getInt(req, "LandId") != previewPlotID {
		return
	}
	fillPreviewSoil(out.Mutable(field(out, "CurrentSoil")).Message())
	fillPreviewSoil(addMessage(out, "SoilAll"))
}

func previewSoils(_ Record, _, out protoreflect.Message) {
	fillPreviewSoil(addMessage(out, "SoilAll"))
}

func previewSeeds(_ Record, _, out protoreflect.Message) {
	seed := addMessage(out, "SeedInfo")
	set(seed, "Id", previewSeedID)
	set(seed, "Name", "【试用】青芽种子")
	set(seed, "FiveElement", 2)
	set(seed, "Level", 1)
	set(seed, "Rank", 1)
}

func previewSeedDetail(_ Record, req, out protoreflect.Message) {
	if getString(req, "Id") != previewSeedID {
		statusPB(out, false, previewDeniedCode)
		return
	}
	set(out, "MapName", "【试用】灵田")
	set(out, "Stage", 1)
	set(out, "FiveElement", 2)
	set(out, "FourGas", 1.0)
	set(out, "FiveFlavour", 0.4)
	set(out, "ReinforcingDiarrhoea", 0.5)
	set(out, "AffiliationAcupoint", 6)
	set(out, "Intensity", 1.0)
	set(out, "Tenacity", 2.0)
}

func previewEmptyContribution(_ Record, _, out protoreflect.Message) {
	set(out, "Contribution", 0)
}
