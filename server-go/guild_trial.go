package main

import (
	"slices"
	"sort"
	"strconv"
	"time"

	"google.golang.org/protobuf/reflect/protoreflect"
)

const trialGuildWorld = "world"

// Membership and pending applications share one durable record, so approving
// someone and removing their pending applications cannot be partially committed.
type trialGuild struct {
	id  int64
	row Record
}

func trialGuilds(world Record) Record {
	guilds, _ := world["Guilds"].(Record)
	return guilds
}

func trialGuildByID(guilds Record, id int64) trialGuild {
	row, _ := guilds[strconv.FormatInt(id, 10)].(Record)
	return trialGuild{id: id, row: row}
}

func trialGuildIDs(guilds Record) []int64 {
	ids := make([]int64, 0, len(guilds))
	for key := range guilds {
		if id, err := strconv.ParseInt(key, 10, 32); err == nil && id > 0 {
			ids = append(ids, id)
		}
	}
	slices.Sort(ids)
	return ids
}

func (g trialGuild) leader() string { return roleString(g.row, "LeaderId") }

func (g trialGuild) members() []string {
	var members []string
	add := func(id string) {
		if id != "" && !slices.Contains(members, id) {
			members = append(members, id)
		}
	}
	add(g.leader())
	switch source := g.row["Members"].(type) {
	case []string:
		for _, id := range source {
			add(id)
		}
	case []any:
		for _, value := range source {
			if id, ok := value.(string); ok {
				add(id)
			}
		}
	}
	return members
}

func (g trialGuild) applications() Record {
	pending, _ := g.row["Applications"].(Record)
	return pending
}

func trialGuildForRole(guilds Record, roleID string) trialGuild {
	if roleID == "" {
		return trialGuild{}
	}
	for _, id := range trialGuildIDs(guilds) {
		guild := trialGuildByID(guilds, id)
		if slices.Contains(guild.members(), roleID) {
			return guild
		}
	}
	return trialGuild{}
}

func (s *Server) currentGuild(peer *Peer) trialGuild {
	return trialGuildForRole(trialGuilds(s.guilds.Get(trialGuildWorld)), peer.currentRoleID())
}

func fillTrialGuildIdentity(out protoreflect.Message, guild trialGuild) {
	id := int64(0)
	if guild.row != nil {
		id = guild.id
	}
	tokenIndex := int64(0)
	if guild.row != nil {
		tokenIndex = max(1, min(4, roleInt(guild.row, "TokenIndex")))
	}
	putPB(out, Record{
		"GuildId": id, "GuildName": roleString(guild.row, "Name"), "GuildSimpleName": roleString(guild.row, "ShortName"),
		"TokenIndex": tokenIndex, "LastNameIndex": max(0, min(5, roleInt(guild.row, "LastNameIndex"))),
	})
}

func (s *Server) fillGuildLogin(id string, out protoreflect.Message) {
	if s.guilds != nil {
		fillTrialGuildIdentity(out, trialGuildForRole(trialGuilds(s.guilds.Get(trialGuildWorld)), id))
	}
}

func (s *Server) trialListGuilds(peer *Peer, req, out protoreflect.Message) (func(), error) {
	guilds := trialGuilds(s.guilds.Get(trialGuildWorld))
	own := trialGuildForRole(guilds, peer.currentRoleID())
	mapID := getString(req, "MapId")
	for _, id := range trialGuildIDs(guilds) {
		guild := trialGuildByID(guilds, id)
		if roleString(guild.row, "MapId") != mapID {
			continue
		}
		if id == own.id {
			self := out.Mutable(field(out, "SelfGuild")).Message()
			putPB(self, Record{"GuildId": id, "Name": roleString(guild.row, "Name"), "IsCreatedGuild": true, "GuildMapId": mapID})
		} else {
			putPB(addMessage(out, "GuildList"), Record{"GuildId": id, "Name": roleString(guild.row, "Name")})
		}
	}
	return nil, nil
}

func (s *Server) trialGuildDetail(_ *Peer, req, out protoreflect.Message) (func(), error) {
	guild := trialGuildByID(trialGuilds(s.guilds.Get(trialGuildWorld)), getInt(req, "GuildId"))
	if guild.row == nil {
		return nil, previewReject(5001)
	}
	putPB(out, Record{
		"GuildId": guild.id, "GuildName": roleString(guild.row, "Name"), "GuildLevel": max(1, roleInt(guild.row, "Stage")),
		"GuildRoleCount": len(guild.members()), "AiDiZiNum": roleInt(guild.row, "AiMemberCount"),
		"ZhanDouLi": roleInt(guild.row, "Fighting"), "YingXiangLi": roleInt(guild.row, "Influence"),
	})
	return nil, nil
}

func (s *Server) trialGuildApply(peer *Peer, req, out protoreflect.Message) (func(), error) {
	roleID := peer.currentRoleID()
	actor := s.trialGuildHistoryRole(roleID)
	if actor == nil {
		return nil, previewReject(4007)
	}
	id := getInt(req, "GuildId")
	err := s.guilds.Transact(func(tx *StoreTx) error {
		world := tx.View(trialGuildWorld)
		if world == nil {
			return ErrRecordNotFound
		}
		guilds := trialGuilds(world)
		guild := trialGuildByID(guilds, id)
		if guild.row == nil {
			return previewReject(5001)
		}
		if trialGuildForRole(guilds, roleID).row != nil {
			return previewReject(4694)
		}
		if _, exists := guild.applications()[roleID]; exists {
			return nil
		}
		guild = trialGuildByID(trialGuilds(tx.Get(trialGuildWorld)), id)
		pending := guild.applications()
		if pending == nil {
			pending = make(Record)
			guild.row["Applications"] = pending
		}
		pending[roleID] = time.Now().UnixMilli()
		return tx.RecordHistory(newHistoryEvent("guild.applied", actor, nil, "guild", strconv.FormatInt(id, 10),
			Record{"guildName": roleString(guild.row, "Name")}))
	})
	if err == nil {
		statusPB(out, true, 4417)
	}
	return nil, err
}

func (s *Server) trialGuildDecide(peer *Peer, req, out protoreflect.Message) (func(), error) {
	actorID := peer.currentRoleID()
	targetID, approved := getString(req, "RoleId"), getBool(req, "IsAgree")
	actor := s.trialGuildHistoryRole(actorID)
	if actor == nil {
		return nil, previewReject(4007)
	}
	subject := s.trialGuildHistoryRole(targetID)
	if subject == nil || actorID == targetID {
		return nil, previewReject(5001)
	}
	var joined trialGuild
	err := s.guilds.Transact(func(tx *StoreTx) error {
		world := tx.View(trialGuildWorld)
		if world == nil {
			return ErrRecordNotFound
		}
		guilds := trialGuilds(world)
		guild := trialGuildForRole(guilds, actorID)
		if guild.row == nil || guild.leader() != actorID {
			return previewReject(4008)
		}
		if _, exists := guild.applications()[targetID]; !exists {
			if approved && slices.Contains(guild.members(), targetID) {
				return nil
			}
			return previewReject(5001)
		}
		if approved && trialGuildForRole(guilds, targetID).row != nil {
			return previewReject(4694)
		}
		guildID := strconv.FormatInt(guild.id, 10)
		details := Record{"guildName": roleString(guild.row, "Name")}
		guilds = trialGuilds(tx.Get(trialGuildWorld))
		guild = trialGuildByID(guilds, guild.id)
		if !approved {
			delete(guild.applications(), targetID)
			return tx.RecordHistory(newHistoryEvent("guild.rejected", actor, subject, "guild", guildID, details))
		}
		guild.row["Members"] = append(guild.members(), targetID)
		for _, id := range trialGuildIDs(guilds) {
			delete(trialGuildByID(guilds, id).applications(), targetID)
		}
		joined = guild
		return tx.RecordHistory(
			newHistoryEvent("guild.approved", actor, subject, "guild", guildID, details),
			newHistoryEvent("guild.joined", subject, actor, "guild", guildID, details),
		)
	})
	if err != nil {
		return nil, err
	}
	statusPB(out, true, 2004)
	if joined.row == nil {
		return nil, nil
	}
	notice := s.gameResponse("GuildInformRoleJoinSuccessResp_1015018", true, 2004)
	fillTrialGuildIdentity(notice, joined)
	return func() { s.pushGame(targetID, 1015018, notice) }, nil
}

func (s *Server) trialGuildHistoryRole(id string) Record {
	if id == "" {
		return nil
	}
	role := s.roles.project(id, []string{"Name", "UserId"})
	if role != nil {
		role["Id"] = id
	}
	return role
}

func (s *Server) trialGuildEnter(peer *Peer, _, out protoreflect.Message) (func(), error) {
	guild := s.currentGuild(peer)
	if guild.row == nil {
		return nil, previewReject(4008)
	}
	putPB(out, Record{
		"IsGuildMaster": guild.leader() == peer.currentRoleID(),
		"Fighting":      roleInt(guild.row, "Fighting"), "Defend": roleInt(guild.row, "Defend"),
	})
	// ZongMenWnd uses fixed slots 0..7 and 9. Type 10 identifies the main peak.
	for index, name := range [...]string{"灵田峰", "冶炼峰", "工坊峰", "内门峰", "炼丹峰", "炼器峰", "制符峰", "阵法峰", "御兽峰", "主峰"} {
		putPB(addMessage(out, "GuildHills"), Record{"Type": index + 1, "HillName": name, "Stage": max(1, roleInt(guild.row, "Stage"))})
	}
	return nil, nil
}

func fillTrialGuildRole(out protoreflect.Message, id string, role Record) {
	putPB(out, Record{
		"RoleId": id, "RoleName": roleString(role, "Name"), "RoleImage": strconv.FormatInt(max(1, roleInt(role, "HeadPortraitId")), 10),
		"Level": max(1, roleInt(role, "Level")), "Sex": roleInt(role, "Sex") == 1 || roleBool(role, "Sex"),
	})
}

func (s *Server) trialGuildMembers(peer *Peer, _, out protoreflect.Message) (func(), error) {
	guild := s.currentGuild(peer)
	if guild.row == nil {
		return nil, previewReject(4008)
	}
	for _, id := range guild.members() {
		role := s.roles.Get(id)
		if role == nil {
			continue
		}
		member := addMessage(out, "RoleList")
		fillTrialGuildRole(member, id, role)
		_, online := s.online.Load(id)
		putPB(member, Record{"IsOnLine": online, "ZhanDouLi": max(0, roleInt(role, "CurrentPower"))})
		if guild.leader() == id {
			set(member, "ZhiWei", 15) // ZhangLaoRenMing.宗主
		}
	}
	return nil, nil
}

func (s *Server) trialGuildApplications(peer *Peer, _, out protoreflect.Message) (func(), error) {
	guild := s.currentGuild(peer)
	if guild.row == nil {
		return nil, previewReject(4008)
	}
	if guild.leader() != peer.currentRoleID() {
		// The member window requests this list while opening for regular members.
		// Show an empty list without exposing other characters' applications.
		return nil, nil
	}
	pending := guild.applications()
	ids := make([]string, 0, len(pending))
	for id := range pending {
		ids = append(ids, id)
	}
	sort.Slice(ids, func(i, j int) bool {
		a, b := roleInt(pending, ids[i]), roleInt(pending, ids[j])
		return a < b || a == b && ids[i] < ids[j]
	})
	for _, id := range ids {
		role := s.roles.Get(id)
		if role == nil {
			continue
		}
		entry := addMessage(out, "JoinList")
		fillTrialGuildRole(entry, id, role)
		set(entry, "RoleAge", max(1, roleInt(role, "CurrentAge")))
	}
	return nil, nil
}

func (s *Server) trialGuildInfo(peer *Peer, _, out protoreflect.Message) (func(), error) {
	guild := s.currentGuild(peer)
	if guild.row == nil {
		return nil, previewReject(4008)
	}
	putPB(out, Record{
		"Name": gameMapName(roleString(guild.row, "MapId")), "BelongHillName": "主峰", "GuildStage": max(1, roleInt(guild.row, "Stage")),
		"GuildName": roleString(guild.row, "Name"), "GuildSimpleName": roleString(guild.row, "ShortName"), "MemberCount": len(guild.members()),
		"AiMemberCount": roleInt(guild.row, "AiMemberCount"), "ZhanDouLi": roleInt(guild.row, "Fighting"), "YingXiangLi": roleInt(guild.row, "Influence"),
	})
	return nil, nil
}

func (s *Server) trialGuildHall(peer *Peer, _, out protoreflect.Message) (func(), error) {
	guild := s.currentGuild(peer)
	leader := s.roles.Get(guild.leader())
	if guild.row == nil || leader == nil {
		return nil, previewReject(4008)
	}
	role := out.Mutable(field(out, "ZongZhuInfo")).Message()
	fillTrialGuildRole(role, guild.leader(), leader)
	set(role, "Fighting", roleInt(leader, "CurrentPower"))
	putPB(out, Record{
		"Stage": max(1, roleInt(guild.row, "Stage")), "Influence": roleInt(guild.row, "Influence"), "Fighting": roleInt(guild.row, "Fighting"),
		"FangYu": roleInt(guild.row, "Defend"), "ZhangLaoShuLiang": len(guild.members()), "DiZiShuLiang": roleInt(guild.row, "AiMemberCount"),
	})
	return nil, nil
}
