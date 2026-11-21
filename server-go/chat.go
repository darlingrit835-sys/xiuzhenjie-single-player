package main

import (
	"crypto/sha256"
	"errors"
	"log"
	"math"
	"path/filepath"
	"strings"
	"sync"
	"time"

	"google.golang.org/protobuf/encoding/protowire"
	"google.golang.org/protobuf/proto"
)

const (
	privateChatSend     = 4001007
	privateChatPush     = 4001008
	privateChatHistory  = 4901001
	maxPrivateChatBytes = 2048
)

// The original client numbers requests within a game connection.
type privateChatReceipt struct {
	actor     string
	requestID int
	digest    [32]byte
	timestamp int64
}

type privateChatReceipts struct {
	recent [16]privateChatReceipt
	next   int
}

func (s *Server) privateChatAck(peer *Peer, msg Message, at int64) {
	ack := s.gameResponse("SendMessageRoleResp_4001007", true, 4348)
	set(ack, "MsgTime", at)
	s.sendGame(peer, msg, ack)
}

func (s *Server) privateChatAllowed(peer *Peer, target string) bool {
	actor := peer.currentRoleID()
	if !s.previewWrites || actor == "" || target == "" || actor == target || reservedStoreID(target) {
		return false
	}
	current, ok := s.online.Load(actor)
	if !ok || current != peer {
		return false
	}
	s.roles.mu.RLock()
	defer s.roles.mu.RUnlock()
	_, from := friendMap(s.roles.records[actor], friendLinks)[target]
	_, to := friendMap(s.roles.records[target], friendLinks)[actor]
	return from && to
}

func privateChatBefore(extra []byte) (int64, error) {
	var before uint64
	for len(extra) > 0 {
		n, typ, length := protowire.ConsumeTag(extra)
		if length < 0 {
			return 0, protowire.ParseError(length)
		}
		extra = extra[length:]
		if n == 3 {
			if typ != protowire.VarintType {
				return 0, errors.New("invalid history cursor")
			}
			before, length = protowire.ConsumeVarint(extra)
			if before > math.MaxInt64 {
				return 0, errors.New("invalid history cursor")
			}
		} else {
			length = protowire.ConsumeFieldValue(n, typ, extra)
		}
		if length < 0 {
			return 0, protowire.ParseError(length)
		}
		extra = extra[length:]
	}
	return int64(before), nil
}

func privateHistoryReply(peer *Peer, msid, code int, newestFirst [][]byte, more bool) {
	out := protowire.AppendTag(nil, 1, protowire.VarintType)
	out = protowire.AppendVarint(out, uint64(code))
	if code == 2000 {
		out = protowire.AppendVarint(protowire.AppendTag(out, 2, protowire.VarintType), 1)
	}
	for i := len(newestFirst) - 1; i >= 0; i-- {
		out = protowire.AppendBytes(protowire.AppendTag(out, 3, protowire.BytesType), newestFirst[i])
	}
	if more {
		out = protowire.AppendVarint(protowire.AppendTag(out, 4, protowire.VarintType), 1)
	}
	_ = peer.sendPayload(privateChatHistory, msid, out)
}

func (s *Server) handlePrivateChat(peer *Peer, msg Message) {
	history := msg.Cmd == privateChatHistory
	fail := func(code int) {
		if history {
			privateHistoryReply(peer, msg.Msid, code, nil, false)
		} else {
			s.sendGame(peer, msg, s.gameResponse("SendMessageRoleResp_4001007", false, code))
		}
	}
	if len(msg.Data) > maxPrivateChatBytes+512 {
		fail(4008)
		return
	}
	req, err := s.decode("SendMessageRoleReq_4001007", msg.Data)
	if err != nil {
		fail(4008)
		return
	}
	actor, target := peer.currentRoleID(), getString(req, "RoleId")
	if !s.privateChatAllowed(peer, target) {
		fail(4407)
		return
	}
	conversation := privateChatKey(actor, target)
	text := getString(req, "Msg")
	if !history && (len(text) > maxPrivateChatBytes || strings.TrimSpace(text) == "") {
		fail(4008)
		return
	}
	db, err := s.privateChat.open(filepath.Dir(s.roles.path))
	if err != nil {
		log.Printf("打开私聊存储失败: %v", err)
		fail(5000)
		return
	}
	if history {
		before, err := privateChatBefore(req.GetUnknown())
		if err != nil {
			fail(4008)
			return
		}
		page, more, err := readPrivateChat(db, conversation, before)
		if err != nil {
			log.Printf("读取私聊存储失败: %v", err)
			fail(5000)
			return
		}
		privateHistoryReply(peer, msg.Msid, 2000, page, more)
		return
	}
	receipt := privateChatReceipt{actor: actor, requestID: msg.Msid, digest: sha256.Sum256(protowire.AppendString(protowire.AppendString(nil, target), text))}
	if msg.Msid > 0 && peer.privateReceipts != nil {
		for _, old := range peer.privateReceipts.recent {
			if old.actor == actor && old.requestID == msg.Msid {
				if old.digest != receipt.digest {
					fail(4008)
				} else {
					s.privateChatAck(peer, msg, old.timestamp)
				}
				return
			}
		}
	}
	at, payload, err := appendPrivateChat(db, conversation, time.Now().UnixMilli(), func(at int64) ([]byte, error) {
		pushed := s.gameResponse("ReceiveMessageRoleResp_4001008", true, 2000)
		putPB(pushed, Record{"RoleId": actor, "Msg": text, "MsgTime": at})
		return proto.Marshal(pushed)
	})
	if err != nil {
		log.Printf("保存私聊消息失败: %v", err)
		fail(5000)
		return
	}
	if msg.Msid > 0 {
		if peer.privateReceipts == nil {
			peer.privateReceipts = &privateChatReceipts{}
		}
		receipts := peer.privateReceipts
		receipt.timestamp = at
		receipts.recent[receipts.next] = receipt
		receipts.next = (receipts.next + 1) % len(receipts.recent)
	}
	s.privateChatAck(peer, msg, at)
	if client, ok := s.online.Load(target); ok {
		receiver := client.(*Peer)
		receiver.queuePrivatePush(target, payload)
	}
}

// A slow receiver only occupies its own delivery worker. Messages that exceed
// the bounded live queue are already durable and available through history.
type privateChatDelivery struct {
	role    string
	frame   []byte
	current func() bool
}

type privateChatPushes struct {
	mu      sync.Mutex
	pending []privateChatDelivery
	running bool
}

func (p *Peer) queuePrivatePush(role string, payload []byte) {
	p.queueRolePush(role, privateChatPush, payload)
}

func (p *Peer) queueRolePush(role string, cmd int, payload []byte) {
	p.queueRoleFrame(role, encodeFrame(cmd, 0, payload), nil)
}

func (p *Peer) queueRoleFrame(role string, frame []byte, current func() bool) {
	q := &p.privatePushes
	q.mu.Lock()
	defer q.mu.Unlock()
	if len(q.pending) >= 32 {
		return
	}
	q.pending = append(q.pending, privateChatDelivery{role: role, frame: frame, current: current})
	if !q.running {
		q.running = true
		go p.deliverPrivatePush()
	}
}

func (p *Peer) deliverPrivatePush() {
	q := &p.privatePushes
	for {
		q.mu.Lock()
		if len(q.pending) == 0 {
			q.pending = nil
			q.running = false
			q.mu.Unlock()
			return
		}
		delivery := q.pending[0]
		q.pending[0] = privateChatDelivery{}
		q.pending = q.pending[1:]
		q.mu.Unlock()

		p.writeFrameForRole(delivery.role, delivery.frame, delivery.current)
	}
}
