package main

import (
	"math"
	"strconv"
	"strings"
	"time"
	"unicode"
	"unicode/utf8"

	"google.golang.org/protobuf/reflect/protoreflect"
)

func (s *Server) validGuildSite(owner, site string) bool {
	role := s.roles.project(owner, []string{"MapId"})
	countyID := sectCountyID(roleString(role, "MapId"))
	forest := gameMapFind(site)
	if countyID == "" || forest == nil || forest.depth != 4 || forest.kind != 4 {
		return false
	}
	county := gameMapFind(forest.parent)
	return county != nil && county.kind == 3 && countyID == county.id
}

func (s *Server) guildSiteDetails(peer *Peer, req, out protoreflect.Message) (func(), error) {
	id, site := peer.currentRoleID(), getString(req, "Id")
	if peer.guildSiteOwner != id || site != peer.guildSite || !s.validGuildSite(id, site) {
		return nil, previewReject(4008)
	}
	peer.guildSiteReady = true
	county := gameMapFind(gameMapFind(site).parent)
	province := gameMapFind(county.parent)
	putPB(out, Record{"Stage": 1, "HillCount": 10, "StoneCount": 1, "JadeCount": 1, "DaLuName": gameMapName(province.parent), "ZhouName": province.name, "YeWaiName": gameMapName(site)})
	return nil, nil
}

func validSectName(s string, low, high int) bool {
	n := utf8.RuneCountInString(s)
	return n >= low && n <= high && strings.IndexFunc(s, func(r rune) bool { return !unicode.IsLetter(r) && !unicode.IsDigit(r) }) < 0
}

func (s *Server) guildFound(peer *Peer, req, out protoreflect.Message) (func(), error) {
	id, site := peer.currentRoleID(), peer.guildSite
	if active, ok := s.online.Load(id); !ok || active != peer {
		return nil, previewReject(4008)
	}
	name, short := getString(req, "Name"), getString(req, "SimpleName")
	token, suffix := getInt(req, "TokenIndex"), getInt(req, "LastNameIndex")
	if !validSectName(name, 3, 5) || !validSectName(short, 1, 2) || token < 1 || token > 4 || suffix < 0 || suffix > 5 {
		return nil, previewReject(4008)
	}
	now := time.Now()
	err := s.guilds.Transact(func(tx *StoreTx) error {
		// Other guild operations that need player state use this same guild -> role lock order.
		role := s.roles.project(id, []string{"Id", "UserId", "Name", "ShinyJade", "ShinyStone", "SectPermitsIssued", "SectPermitGranted"})
		if role == nil || peer.guildSiteOwner != id || !s.validGuildSite(id, site) {
			return previewReject(4008)
		}
		role["Id"] = id
		guilds := trialGuilds(tx.View(trialGuildWorld))
		current := trialGuildForRole(guilds, id)
		if current.row != nil {
			if current.leader() != id || roleString(current.row, "Name") != name || roleString(current.row, "ShortName") != short || roleInt(current.row, "TokenIndex") != token || roleInt(current.row, "LastNameIndex") != suffix || roleString(current.row, "MapId") != site {
				return previewReject(4694)
			}
			putPB(out, Record{"GuildId": current.id, "ShinyStone": roleInt(role, "ShinyStone"), "ShinyJade": roleInt(role, "ShinyJade")})
			return nil
		}
		if !peer.guildSiteReady {
			return previewReject(4008)
		}
		if err := guildRejoinCheck(tx, id, now); err != nil {
			return err
		}
		latest := max(int64(1_000_000), roleInt(tx.View(trialGuildWorld), "LastCourtGuildId"))
		for _, guildID := range trialGuildIDs(guilds) {
			g := trialGuildByID(guilds, guildID)
			if roleString(g.row, "MapId") == site || roleString(g.row, "Name") == name {
				return previewReject(4008)
			}
			latest = max(latest, guildID)
		}
		if latest >= math.MaxInt32 {
			return previewReject(4008)
		}
		used := sectPermitSpent(tx.View(trialGuildWorld), id)
		issued := roleInt(role, "SectPermitsIssued")
		if issued <= used {
			// The original founding window predates the charter counter. Hand
			// out its first free permit, after all other checks have passed.
			if peer.sectPermitOwner == id || roleBool(role, "SectPermitGranted") {
				return previewReject(4008)
			}
			permitRole, err := s.issueSectPermit(id, sectOrder{Action: "grant", Revision: issued})
			if err != nil {
				return err
			}
			issued = roleInt(permitRole, "SectPermitsIssued")
			if issued <= used {
				return previewReject(4008)
			}
		}
		g := trialGuild{id: latest + 1, row: Record{"Name": name, "ShortName": short, "Stage": 1, "LeaderId": id, "Members": []string{id},
			"MapId": site, "TokenIndex": token, "LastNameIndex": suffix, "AiMemberCount": 6}}
		world := tx.GetOrCreate(trialGuildWorld)
		sectMap(world, "PermitsUsed")[id] = used + 1
		guilds = sectMap(world, "Guilds")
		// Creating a guild resolves outstanding applications just as accepting one does.
		for _, v := range guilds {
			row, _ := v.(Record)
			pending, _ := row["Applications"].(Record)
			delete(pending, id)
		}
		key := strconv.FormatInt(g.id, 10)
		guilds[key] = g.row
		world["LastCourtGuildId"] = g.id
		initSectCourt(g, now, sectCountyID(site))
		putPB(out, Record{"GuildId": g.id, "ShinyStone": roleInt(role, "ShinyStone"), "ShinyJade": roleInt(role, "ShinyJade")})
		return tx.RecordHistory(newHistoryEvent("guild.founded", role, nil, "guild", key, Record{"guildName": name, "site": site}))
	})
	if err == nil {
		peer.guildSiteReady = false
	}
	return nil, err
}
