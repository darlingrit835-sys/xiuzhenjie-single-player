package main

import (
	"strconv"
	"time"

	"google.golang.org/protobuf/proto"
	"google.golang.org/protobuf/reflect/protoreflect"
)

// The legacy request has no guild ID. A recent retry on the same connection
// must not disband a guild that its sender has since joined and come to lead.
type guildDisbandReceipt struct {
	roleID  string
	msid    int
	expires time.Time
}

func (s *Server) trialGuildDisband(peer *Peer, _, out protoreflect.Message) (func(), error) {
	roleID := peer.currentRoleID()
	now := time.Now()
	if r := peer.guildDisbandReceipt; r != nil && r.roleID == roleID && r.msid == peer.gameRequestID && now.Before(r.expires) {
		return nil, nil
	}
	actor := s.trialGuildHistoryRole(roleID)
	if actor == nil {
		return nil, previewReject(4007)
	}
	var members []string
	err := s.guilds.Transact(func(tx *StoreTx) error {
		guild := trialGuildForRole(trialGuilds(tx.View(trialGuildWorld)), roleID)
		if guild.row == nil {
			return previewReject(4420)
		}
		if guild.leader() != roleID {
			return previewReject(4687)
		}
		members = guild.members()
		id, name := strconv.FormatInt(guild.id, 10), roleString(guild.row, "Name")
		events := []historyEvent{newHistoryEvent("guild.disbanded", actor, nil, "guild", id,
			Record{"guildName": name, "memberCount": len(members), "applicationCount": len(guild.applications())})}
		for _, memberID := range members {
			if memberID == roleID {
				continue
			}
			subject := s.trialGuildHistoryRole(memberID)
			if subject == nil {
				// Even a legacy membership whose role record is missing keeps its original ID in history.
				subject = Record{"Id": memberID}
			}
			events = append(events, newHistoryEvent("guild.membership_ended", actor, subject, "guild", id,
				Record{"guildName": name, "oldOffice": guild.officeOf(memberID), "reason": "disbanded"}))
		}
		world := tx.Get(trialGuildWorld)
		world["LastCourtGuildId"] = max(roleInt(world, "LastCourtGuildId"), guild.id)
		delete(trialGuilds(world), id)
		return tx.RecordHistory(events...)
	})
	if err != nil {
		return nil, err
	}
	if peer.gameRequestID > 0 {
		peer.guildDisbandReceipt = &guildDisbandReceipt{roleID, peer.gameRequestID, now.Add(5 * time.Minute)}
	}
	return func() {
		// The requester already leaves in the 1015158 reply callback. Offline members
		// get the saved membership on login. Slow sockets never hold the sender's game loop.
		online := make([]string, 0, len(members))
		for _, member := range members {
			if member != roleID {
				if _, ok := s.online.Load(member); ok {
					online = append(online, member)
				}
			}
		}
		const workers = 4
		for first := range min(workers, len(online)) {
			go func() {
				for i := first; i < len(online); i += workers {
					s.pushGuildMembership(online[i], 0, 1015159, nil)
				}
			}()
		}
	}, nil
}

// Serialize the membership check with writes to the destination connection.
// The two empty departure notifications otherwise clear any newer guild in Unity.
func (s *Server) pushGuildMembership(roleID string, expectedGuild int64, cmd int, data proto.Message) {
	s.pushGameIf(roleID, cmd, data, func() bool {
		s.guilds.mu.RLock()
		defer s.guilds.mu.RUnlock()
		current := trialGuildForRole(trialGuilds(s.guilds.records[trialGuildWorld]), roleID)
		return current.id == expectedGuild
	})
}
