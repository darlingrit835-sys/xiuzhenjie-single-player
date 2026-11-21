package main

import (
	"strconv"
	"strings"
	"time"

	"google.golang.org/protobuf/proto"
)

const (
	worldChatJoin     = 4001001
	worldChatPush     = 4001002
	worldChatSend     = 4001003
	worldChatLeave    = 4001004
	worldChatID       = "world"
	worldChatCredit   = 100
	worldChatInterval = 3 * time.Second
	maxWorldChatBytes = 512
)

var worldChatPlainText = strings.NewReplacer("<", "＜", ">", "＞")

func (s *Server) handleWorldChat(peer *Peer, msg Message) {
	switch msg.Cmd {
	case worldChatJoin:
		s.joinWorldChat(peer, msg)
	case worldChatSend:
		s.sendWorldChat(peer, msg)
	case worldChatLeave:
		req, err := s.decode("WineOutReq_4001004", msg.Data)
		ok := err == nil && getString(req, "ChatId") == worldChatID && s.worldChatRole(peer) != ""
		if ok {
			peer.worldChatJoined.Store(false)
		}
		out := s.gameResponse("WineOutResp_4001004", ok, 4347)
		if ok {
			set(out, "Code", 2000)
		}
		s.sendGame(peer, msg, out)
	}
}

func (s *Server) worldChatRole(peer *Peer) string {
	id := peer.currentRoleID()
	if current, ok := s.online.Load(id); id != "" && ok && current == peer {
		return id
	}
	return ""
}

func (s *Server) joinWorldChat(peer *Peer, msg Message) {
	out := s.gameResponse("ChatJoinResp_4001001", false, 4347)
	req, err := s.decode("ChatJoinReq_4001001", msg.Data)
	id := s.worldChatRole(peer)
	if err == nil && id != "" && getString(req, "ChatId") == worldChatID {
		if requested := getString(req, "RoleId"); requested == "" || requested == id {
			peer.worldChatJoined.Store(true)
			statusPB(out, true, 4345)
			set(out, "CreditPoints", worldChatCredit)
			set(out, "TimeLag", worldChatInterval.Milliseconds())
			if peer.worldChatNextSend.After(time.Now()) {
				set(out, "NextSendTime", peer.worldChatNextSend.UnixMilli())
			}
		}
	}
	s.sendGame(peer, msg, out)
}

func (s *Server) sendWorldChat(peer *Peer, msg Message) {
	out := s.gameResponse("SendMessageResp_4001003", false, 4347)
	defer func() { s.sendGame(peer, msg, out) }()
	id := s.worldChatRole(peer)
	if id == "" || !peer.worldChatJoined.Load() {
		return
	}
	if len(msg.Data) > maxWorldChatBytes+64 {
		set(out, "Code", 4008)
		return
	}
	req, err := s.decode("SendMessageReq_4001003", msg.Data)
	if err != nil || getString(req, "ChatId") != worldChatID {
		return
	}
	text := strings.TrimSpace(getString(req, "Msg"))
	if text == "" || len(text) > maxWorldChatBytes {
		set(out, "Code", 4008)
		return
	}
	now := time.Now()
	if now.Before(peer.worldChatNextSend) {
		set(out, "Code", 4357)
		return
	}
	role := s.roles.project(id, []string{"Name", "Level", "HeadPortraitId", "Sex", "HeadFrameId", "ChengHaoFrameId"})
	if role == nil {
		return
	}
	push := s.gameResponse("ReceiveMessageResp_4001002", true, 2000)
	putPB(push, Record{
		"ChatId": worldChatID, "RoleId": id, "RoleName": worldChatPlainText.Replace(roleString(role, "Name")),
		"Msg": worldChatPlainText.Replace(text), "Level": max(1, roleInt(role, "Level")),
		"HeadPortraitId": strconv.FormatInt(roleInt(role, "HeadPortraitId"), 10),
		"HeadFrame":      roleInt(role, "HeadFrameId"), "TitleFrame": roleInt(role, "ChengHaoFrameId"),
	})
	if roleInt(role, "Sex") == 1 || roleBool(role, "Sex") {
		set(push, "Sex", 1)
	}
	payload, err := proto.Marshal(push)
	if err != nil {
		set(out, "Code", 5000)
		return
	}
	peer.worldChatNextSend = now.Add(worldChatInterval)
	statusPB(out, true, 4348)
	set(out, "CreditPoints", worldChatCredit)
	set(out, "TimeLag", worldChatInterval.Milliseconds())
	set(out, "NextSendTime", peer.worldChatNextSend.UnixMilli())
	frame := encodeFrame(worldChatPush, 0, payload)
	s.online.Range(func(key, value any) bool {
		recipient := value.(*Peer)
		if recipient.worldChatJoined.Load() {
			recipient.queueRoleFrame(key.(string), frame, recipient.worldChatJoined.Load)
		}
		return true
	})
}
