package main

import (
	"maps"
	"slices"
	"strconv"

	"google.golang.org/protobuf/reflect/protoreflect"
)

const (
	friendLinks      = "DaoYouFriends"
	friendInbox      = "DaoYouRequests"
	trialFriendLimit = 200
)

func friendPreviewRoutes() []previewRoute {
	const module = "社交与宗门"
	return []previewRoute{
		{cmd: 1014001, module: module, response: "EnterFriendResp_1014001", summary: "查看双向道友关系、在线状态和待处理名刺数", build: previewEmptySocialList, live: (*Server).trialFriendList},
		{cmd: 1014003, module: module, response: "EnterAddFriendResp_1014003", summary: "查看寄给当前角色的名刺", build: previewEmptySocialList, live: (*Server).trialFriendInbox},
		{cmd: 1014005, module: module, request: "RemoteSendGoodsReq_1014005", response: "RemoteSendGoodsResp_1014005", summary: "按原价或称号折扣向道友传物并更新双方纳戒", blocked: true, live: (*Server).trialFriendTransfer},
		{cmd: 1014006, module: module, request: "AddFriendReq_1014006", response: "AddFriendResp_1014006", summary: "同意收到的名刺并建立双向道友关系", blocked: true, live: (*Server).trialFriendAccept},
		{cmd: 1014007, module: module, request: "AddFriendByNameReq_1014007", response: "AddFriendByNameResp_1014007", summary: "按角色名发送名刺", blocked: true, live: (*Server).trialFriendSendByName},
		{cmd: 1014008, module: module, request: "AddFriendByRoleIdReq_1014008", response: "AddFriendByRoleIdResp_1014008", summary: "按角色 ID 发送名刺", blocked: true, live: (*Server).trialFriendSendByID},
		{cmd: 1014009, module: module, request: "RefuseFriendReq_1014009", response: "RefuseFriendResp_1014009", summary: "拒绝寄给自己的名刺", blocked: true, live: (*Server).trialFriendRefuse},
		{cmd: 1014010, module: module, request: "DeleteFriendReq_1014010", response: "DeleteFriendResp_1014010", summary: "解除双方道友关系", blocked: true, live: (*Server).trialFriendRemove},
	}
}

func friendMap(role Record, key string) Record {
	entries, _ := role[key].(Record)
	return entries
}

func putFriendEntry(role Record, key, id string, value any) {
	entries := friendMap(role, key)
	if entries == nil {
		entries = Record{}
		role[key] = entries
	}
	entries[id] = value
}

var friendProfileFields = []string{"Name", "HeadPortraitId", "Level", "Sex", "Live1Type", "Live2Type"}

func (s *Server) fillFriendList(entries Record, out protoreflect.Message, list string) {
	for _, id := range slices.Sorted(maps.Keys(entries)) {
		role := s.roles.project(id, friendProfileFields)
		if role == nil || reservedStoreID(id) {
			continue
		}
		_, online := s.online.Load(id)
		entry := addMessage(out, list)
		putPB(entry, Record{
			"Id": id, "Name": roleString(role, "Name"), "Level": max(1, roleInt(role, "Level")),
			"HeadPortraitId": strconv.FormatInt(max(1, roleInt(role, "HeadPortraitId")), 10),
			"Sex":            roleInt(role, "Sex") == 1 || roleBool(role, "Sex"), "IsOnLine": online,
		})
		types := entry.Mutable(field(entry, "LiveType")).List()
		for _, key := range [...]string{"Live1Type", "Live2Type"} {
			types.Append(protoreflect.ValueOfInt32(int32(roleInt(role, key))))
		}
		if list == "Friend" {
			set(entry, "Intimacy", roleInt(entries, id))
		}
	}
}

func (s *Server) trialFriendList(peer *Peer, _, out protoreflect.Message) (func(), error) {
	role := s.roles.project(peer.currentRoleID(), []string{friendLinks, friendInbox})
	if role == nil {
		return nil, previewReject(4401)
	}
	set(out, "StrangerCount", len(friendMap(role, friendInbox)))
	s.fillFriendList(friendMap(role, friendLinks), out, "Friend")
	return nil, nil
}

func (s *Server) trialFriendInbox(peer *Peer, _, out protoreflect.Message) (func(), error) {
	role := s.roles.project(peer.currentRoleID(), []string{friendInbox})
	if role == nil {
		return nil, previewReject(4401)
	}
	s.fillFriendList(friendMap(role, friendInbox), out, "Stranger")
	return nil, nil
}

func (s *Server) trialFriendSendByName(peer *Peer, req, out protoreflect.Message) (func(), error) {
	id, name := s.resolveSocialName(getString(req, "Name"))
	if id == "" {
		return nil, previewReject(4402)
	}
	return nil, s.sendFriendRequest(peer.currentRoleID(), id, name, out)
}

func (s *Server) trialFriendSendByID(peer *Peer, req, out protoreflect.Message) (func(), error) {
	return nil, s.sendFriendRequest(peer.currentRoleID(), getString(req, "TargetFriendId"), "", out)
}

func validateFriendPair(actorID, otherID string, tx *StoreTx) (Record, Record, error) {
	if actorID == "" || otherID == "" || reservedStoreID(actorID) || reservedStoreID(otherID) {
		return nil, nil, previewReject(4401)
	}
	if actorID == otherID {
		return nil, nil, previewReject(4413)
	}
	actor, other := tx.View(actorID), tx.View(otherID)
	if actor == nil || other == nil {
		return nil, nil, previewReject(4401)
	}
	return actor, other, nil
}

func (s *Server) sendFriendRequest(actorID, otherID, name string, out protoreflect.Message) error {
	err := s.roles.Transact(func(tx *StoreTx) error {
		actor, other, err := validateFriendPair(actorID, otherID, tx)
		if err != nil {
			return err
		}
		if name != "" && name != roleString(other, "Name") {
			return previewReject(4402)
		}
		friends, inbox := friendMap(other, friendLinks), friendMap(other, friendInbox)
		if _, exists := friends[actorID]; exists {
			return previewReject(4403)
		}
		if _, exists := inbox[actorID]; exists {
			return nil
		}
		if len(inbox) >= trialFriendLimit || len(friends) >= trialFriendLimit || len(friendMap(actor, friendLinks)) >= trialFriendLimit {
			return previewReject(4409)
		}
		putFriendEntry(tx.Get(otherID), friendInbox, actorID, true)
		return tx.RecordHistory(socialHistory("friend.requested", actorID, actor, otherID, other, nil))
	})
	if err == nil {
		statusPB(out, true, 4404)
	}
	return err
}

func (s *Server) trialFriendAccept(peer *Peer, req, out protoreflect.Message) (func(), error) {
	actorID, otherID := peer.currentRoleID(), getString(req, "TargetFriendId")
	err := s.roles.Transact(func(tx *StoreTx) error {
		actor, other, err := validateFriendPair(actorID, otherID, tx)
		if err != nil {
			return err
		}
		friends := friendMap(actor, friendLinks)
		if _, exists := friends[otherID]; exists {
			return previewReject(4403)
		}
		if _, pending := friendMap(actor, friendInbox)[otherID]; !pending {
			return previewReject(4008)
		}
		if len(friends) >= trialFriendLimit || len(friendMap(other, friendLinks)) >= trialFriendLimit {
			return previewReject(4409)
		}
		actor, other = tx.Get(actorID), tx.Get(otherID)
		putFriendEntry(actor, friendLinks, otherID, 0)
		putFriendEntry(other, friendLinks, actorID, 0)
		preserveAchievementFriendPeak(actor)
		preserveAchievementFriendPeak(other)
		delete(friendMap(actor, friendInbox), otherID)
		delete(friendMap(other, friendInbox), actorID)
		// Each player's first friend belongs to that player, including the applicant.
		return tx.RecordHistory(
			socialHistory("friend.added", actorID, actor, otherID, other, Record{"accepted": true}),
			socialHistory("friend.added", otherID, other, actorID, actor, Record{"accepted": false}),
		)
	})
	if err == nil {
		statusPB(out, true, 4400)
	}
	return nil, err
}

func (s *Server) trialFriendRefuse(peer *Peer, req, out protoreflect.Message) (func(), error) {
	actorID, otherID := peer.currentRoleID(), getString(req, "TargetFriendId")
	err := s.roles.Transact(func(tx *StoreTx) error {
		actor, other, err := validateFriendPair(actorID, otherID, tx)
		if err != nil {
			return err
		}
		if _, pending := friendMap(actor, friendInbox)[otherID]; !pending {
			return previewReject(4008)
		}
		delete(friendMap(tx.Get(actorID), friendInbox), otherID)
		return tx.RecordHistory(socialHistory("friend.rejected", actorID, actor, otherID, other, nil))
	})
	if err == nil {
		statusPB(out, true, 2004)
	}
	return nil, err
}

func (s *Server) trialFriendRemove(peer *Peer, req, out protoreflect.Message) (func(), error) {
	actorID, otherID := peer.currentRoleID(), getString(req, "TargetFriendId")
	err := s.roles.Transact(func(tx *StoreTx) error {
		actor, other, err := validateFriendPair(actorID, otherID, tx)
		if err != nil {
			return err
		}
		if _, exists := friendMap(actor, friendLinks)[otherID]; !exists {
			return previewReject(4407)
		}
		actor, other = tx.Get(actorID), tx.Get(otherID)
		preserveAchievementFriendPeak(actor)
		preserveAchievementFriendPeak(other)
		delete(friendMap(actor, friendLinks), otherID)
		delete(friendMap(other, friendLinks), actorID)
		return tx.RecordHistory(socialHistory("friend.removed", actorID, actor, otherID, other, nil))
	})
	if err == nil {
		statusPB(out, true, 2004)
	}
	return nil, err
}
