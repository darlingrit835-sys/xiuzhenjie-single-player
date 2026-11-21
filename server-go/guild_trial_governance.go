package main

import (
	"slices"
	"strconv"
	"time"

	"google.golang.org/protobuf/reflect/protoreflect"
)

const (
	guildLeaderOffice = 15
	guildRejoinRecord = "rejoin-cooldowns"
)

func guildGovernanceRoutes() []previewRoute {
	const module = "社交与宗门"
	return []previewRoute{
		{cmd: 1015013, module: module, request: "GuildExpelReq_1015013", response: "GuildExpelResp_1015013", summary: "宗主逐出成员并通知在线角色", blocked: true, live: (*Server).trialGuildExpel},
		{cmd: 1015014, module: module, response: "GuildExitResp_1015014", summary: "成员退出宗门并释放职务", blocked: true, live: (*Server).trialGuildExit},
		{cmd: 1015022, module: module, request: "XieRenReq_1015022", response: "XieRenResp_1015022", summary: "宗主将职位转让给本宗成员", blocked: true, live: (*Server).trialGuildTransfer},
		{cmd: 1015023, module: module, response: "EnterRenMingResp_1015023", summary: "查看每个宗门职务的当前任职者", blocked: true, live: (*Server).trialGuildOfficeList},
		{cmd: 1015025, module: module, request: "RenMingReq_1015025", response: "RenMingResp_1015025", summary: "宗主任命成员、撤销职务或交接宗主", blocked: true, live: (*Server).trialGuildAppoint},
		{cmd: 1015158, module: module, response: "GuildJieSanResp_1015158", summary: "宗主解散本宗门并通知在线成员", blocked: true, live: (*Server).trialGuildDisband},
	}
}

// The client's office enum reserves 0 for unassigned and 15 for the leader.
// Only the fourteen single-holder offices are stored here. LeaderId remains authoritative.
func (g trialGuild) offices() Record {
	offices, _ := g.row["Offices"].(Record)
	return offices
}

func (g trialGuild) officeOf(roleID string) int {
	if roleID == g.leader() {
		return guildLeaderOffice
	}
	for office := 1; office < guildLeaderOffice; office++ {
		if roleString(g.offices(), strconv.Itoa(office)) == roleID {
			return office
		}
	}
	return 0
}

func (s *Server) trialGuildOfficeList(peer *Peer, _, out protoreflect.Message) (func(), error) {
	guild := s.currentGuild(peer)
	if guild.row == nil {
		return nil, previewReject(4420)
	}
	for office := 0; office <= guildLeaderOffice; office++ {
		id := roleString(guild.offices(), strconv.Itoa(office))
		if office == guildLeaderOffice {
			id = guild.leader()
		}
		entry := addMessage(out, "HillZhangLaoInfo")
		set(entry, "Type", office)
		if id != "" && office != 0 {
			set(entry, "RoleName", roleString(s.roles.project(id, []string{"Name"}), "Name"))
		}
	}
	return nil, nil
}

func (s *Server) trialGuildTransfer(peer *Peer, req, out protoreflect.Message) (func(), error) {
	return nil, s.changeGuildOffice(peer, getString(req, "RoleId"), guildLeaderOffice, out)
}

func (s *Server) trialGuildAppoint(peer *Peer, req, out protoreflect.Message) (func(), error) {
	office := getInt(req, "Type")
	if office < 0 || office > guildLeaderOffice {
		return nil, previewReject(4008)
	}
	return nil, s.changeGuildOffice(peer, getString(req, "RoleId"), int(office), out)
}

func (s *Server) changeGuildOffice(peer *Peer, targetID string, office int, out protoreflect.Message) error {
	actorID := peer.currentRoleID()
	actor, subject := s.trialGuildHistoryRole(actorID), s.trialGuildHistoryRole(targetID)
	if actor == nil {
		return previewReject(4007)
	}
	return s.guilds.Transact(func(tx *StoreTx) error {
		guild := trialGuildForRole(trialGuilds(tx.View(trialGuildWorld)), actorID)
		if guild.row == nil || guild.leader() != actorID {
			return previewReject(4687)
		}
		members := guild.members()
		if subject == nil || !slices.Contains(members, targetID) {
			return previewReject(4644)
		}
		old := guild.officeOf(targetID)
		if targetID == actorID || old == office {
			return previewReject(4646)
		}
		if office > 0 && office < guildLeaderOffice && roleString(guild.offices(), strconv.Itoa(office)) != "" {
			// Leave the incumbent in place until the leader explicitly removes them.
			return previewReject(4646)
		}
		guild = trialGuildByID(trialGuilds(tx.Get(trialGuildWorld)), guild.id)
		offices := guild.offices()
		delete(offices, strconv.Itoa(old))
		kind := "guild.office_assigned"
		code := 4647
		switch office {
		case 0:
			kind = "guild.office_revoked"
		case guildLeaderOffice:
			// Legacy saves may omit the original leader from Members.
			guild.row["Members"] = members
			guild.row["LeaderId"] = targetID
			kind, code = "guild.leadership_transferred", 4645
		default:
			if offices == nil {
				offices = Record{}
				guild.row["Offices"] = offices
			}
			offices[strconv.Itoa(office)] = targetID
		}
		statusPB(out, true, code)
		return tx.RecordHistory(newHistoryEvent(kind, actor, subject, "guild", strconv.FormatInt(guild.id, 10),
			Record{"guildName": roleString(guild.row, "Name"), "oldOffice": old, "newOffice": office}))
	})
}

func (s *Server) trialGuildExit(peer *Peer, _, out protoreflect.Message) (func(), error) {
	id := peer.currentRoleID()
	return nil, s.removeGuildMember(id, id, false)
}

func (s *Server) trialGuildExpel(peer *Peer, req, out protoreflect.Message) (func(), error) {
	target := getString(req, "RoleId")
	if err := s.removeGuildMember(peer.currentRoleID(), target, true); err != nil {
		return nil, err
	}
	// The original 1015019 push has an empty PB body.
	return func() { s.pushGuildMembership(target, 0, 1015019, nil) }, nil
}

func (s *Server) removeGuildMember(actorID, targetID string, expel bool) error {
	actor := s.trialGuildHistoryRole(actorID)
	if actor == nil {
		return previewReject(4007)
	}
	var subject Record
	if expel {
		subject = s.trialGuildHistoryRole(targetID)
	}
	return s.guilds.Transact(func(tx *StoreTx) error {
		guild := trialGuildForRole(trialGuilds(tx.View(trialGuildWorld)), actorID)
		if guild.row == nil {
			return previewReject(4420)
		}
		if expel && actorID != guild.leader() {
			return previewReject(4687)
		}
		if targetID == guild.leader() {
			return previewReject(4437)
		}
		members := guild.members()
		index := slices.Index(members, targetID)
		if index < 0 || expel && subject == nil {
			return previewReject(4644)
		}
		office := guild.officeOf(targetID)
		guild = trialGuildByID(trialGuilds(tx.Get(trialGuildWorld)), guild.id)
		guild.row["Members"] = slices.Delete(members, index, index+1)
		delete(guild.offices(), strconv.Itoa(office))
		kind, delay, code := "guild.left", 72*time.Hour, 4696
		if expel {
			kind, delay, code = "guild.expelled", 24*time.Hour, 4695
		}
		now := time.Now()
		until := now.Add(delay).UnixMilli()
		setGuildRejoinCooldown(tx, targetID, code, now, until)
		return tx.RecordHistory(newHistoryEvent(kind, actor, subject, "guild", strconv.FormatInt(guild.id, 10),
			Record{"guildName": roleString(guild.row, "Name"), "oldOffice": office, "rejoinAt": until}))
	})
}

func guildRejoinCheck(tx *StoreTx, id string, now time.Time) error {
	roles, _ := tx.View(guildRejoinRecord)["Roles"].(Record)
	cooldown, _ := roles[id].(Record)
	if now.UnixMilli() < roleInt(cooldown, "Until") {
		return previewReject(roleInt(cooldown, "Code"))
	}
	return nil
}

func setGuildRejoinCooldown(tx *StoreTx, id string, code int, now time.Time, until int64) {
	record := tx.GetOrCreate(guildRejoinRecord)
	roles, _ := record["Roles"].(Record)
	if roles == nil {
		roles = Record{}
		record["Roles"] = roles
	}
	// Lazy daily pruning avoids a timer and a full cooldown scan on every departure.
	if now.UnixMilli() >= roleInt(record, "PruneAt") {
		for key, value := range roles {
			entry, _ := value.(Record)
			if roleInt(entry, "Until") <= now.UnixMilli() {
				delete(roles, key)
			}
		}
		record["PruneAt"] = now.Add(24 * time.Hour).UnixMilli()
	}
	roles[id] = Record{"Until": until, "Code": code}
}
