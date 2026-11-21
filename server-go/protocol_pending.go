package main

// These callbacks need a typed failure when a writable interface is unavailable.
// Some legacy response names do not follow the usual Resp_<Cmd> convention.
var pendingGameReplies = [...]previewRoute{
	{cmd: 1001002, module: "商店与活动", response: "ActionBoolResp_1001001", summary: "查询广告可用性，暂未接入广告渠道", blocked: true},
	{cmd: 1001013, module: "洞府与修炼", request: "UnderstandMethodForgetReq_1001013", response: "ActionBoolResp_1001001", summary: "遗忘参悟，暂未接入完整参悟状态", blocked: true},
	{cmd: 1004038, module: "社交与宗门", request: "GoodsGuildMapDetailsReq_1004038", response: "GoodsGuildMapDetailsReap_1004038", summary: "核对本郡待立宗山门地块", blocked: true, live: (*Server).guildSiteDetails},
	{cmd: 1015007, module: "社交与宗门", request: "GuildCreateReq_1015007", response: "GuildCreateResp_1015007", summary: "在本郡空地持印开宗并绑定郡域", blocked: true, live: (*Server).guildFound},
}

func pendingGameRoutes() []previewRoute { return pendingGameReplies[:] }

func explicitlyDeniedGameCommand(cmd int) bool {
	for _, route := range pendingGameReplies {
		if route.cmd == cmd {
			return true
		}
	}
	return false
}
