package main

import "google.golang.org/protobuf/reflect/protoreflect"

const (
	previewMailID    = "trial-mail-welcome"
	previewGuildMap  = "1111W1"
	previewGuildID   = 900001
	previewGuildName = "【试用】临仙山门"
)

func socialPreviewRoutes() []previewRoute {
	const module = "社交与宗门"
	return []previewRoute{
		{cmd: 1003001, module: module, response: "LetterQueryResp_1003001", summary: "列出一封已读、无附件的演示信件", build: previewMailList},
		{cmd: 1003004, module: module, request: "DeleteLetterReq_1003004", response: "ActionBoolResp_1001001", summary: "拒绝删除试用信件", blocked: true},
		{cmd: 1003005, module: module, request: "ReadLetterReq_1003005", response: "ActionBoolResp_1001001", summary: "打开已读演示信件的正文", build: previewReadMail},
		{cmd: 1003006, module: module, request: "ReceiveLetterReq_1003006", response: "ReceiveLetterResp_1003006", summary: "拒绝领取试用邮件附件", blocked: true},
		{cmd: 1015001, module: module, request: "GuildListReq_1015001", response: "GuildListResp_1015001", summary: "查看当前林地宗门及自己的成员关系", build: previewGuildList, live: (*Server).trialListGuilds},
		{cmd: 1015003, module: module, request: "GuildDetailReq_1015003", response: "GuildDetailResp_1015003", summary: "查看宗门的品阶、人数与数值", build: previewGuildDetail, live: (*Server).trialGuildDetail},
		{cmd: 1015004, module: module, request: "GuildJoinReq_1015004", response: "GuildJoinResp_1015004", summary: "在写入试用中递交宗门拜帖", blocked: true, live: (*Server).trialGuildApply},
		{cmd: 1015005, module: module, response: "GuildEnterResp_1015005", summary: "由成员进入试用宗门及山峰场景", blocked: true, live: (*Server).trialGuildEnter},
		{cmd: 1015008, module: module, response: "GuildRoleListResp_1015008", summary: "查看本宗门实际成员", blocked: true, live: (*Server).trialGuildMembers},
		{cmd: 1015010, module: module, response: "GuildJoinListResp_1015010", summary: "由宗主查看尚待审批的拜帖", blocked: true, live: (*Server).trialGuildApplications},
		{cmd: 1015012, module: module, request: "GuildJoinHandleReq_1015012", response: "GuildJoinHandleResp_1015012", summary: "由宗主同意或拒绝拜帖", blocked: true, live: (*Server).trialGuildDecide},
		{cmd: 1015015, module: module, response: "GuildInfoResp_1015015", summary: "查看本宗门的位置与成员数", blocked: true, live: (*Server).trialGuildInfo},
		{cmd: 1015020, module: module, response: "IntoDaDianResp_1015020", summary: "读取主峰入口和宗主信息", blocked: true, live: (*Server).trialGuildHall},
	}
}

func previewMailList(_ Record, _ protoreflect.Message, out protoreflect.Message) {
	statusPB(out, true, 4055)
	letter := addMessage(out, "Letters")
	set(letter, "Id", previewMailID)
	set(letter, "Title", "【试用】传信示例")
	set(letter, "Content", "这是一封已读的联调信件。打开即可预览传信正文。")
	set(letter, "Status", true)
	set(letter, "IsGetAll", true)
}

func previewReadMail(_ Record, request, out protoreflect.Message) {
	if getString(request, "Id") != previewMailID {
		statusPB(out, false, 5001)
		return
	}
	statusPB(out, true, 4066)
}

// A successful empty repeated field is the wire representation of an empty list.
func previewEmptySocialList(_ Record, _, _ protoreflect.Message) {}

func previewMentorship(_ Record, _ protoreflect.Message, out protoreflect.Message) {
	// The client treats false as an invitation to collect a pending transfer.
	set(out, "IsGetTmcg", true)
}

func previewGuildList(_ Record, request, out protoreflect.Message) {
	if getString(request, "MapId") != previewGuildMap {
		return
	}
	guild := addMessage(out, "GuildList")
	set(guild, "GuildId", previewGuildID)
	set(guild, "Name", previewGuildName)
}

func previewGuildDetail(_ Record, request, out protoreflect.Message) {
	if getInt(request, "GuildId") != previewGuildID {
		statusPB(out, false, 5001)
		return
	}
	putPB(out, Record{
		"GuildId": previewGuildID, "GuildName": previewGuildName, "GuildLevel": 1,
		"GuildRoleCount": 2, "AiDiZiNum": 12, "ZhanDouLi": 800, "YingXiangLi": 120,
	})
}
