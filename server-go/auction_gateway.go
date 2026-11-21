package main

import (
	"context"
	"crypto/sha256"
	"encoding/hex"
	"encoding/json"
	"errors"
	"log"
	"time"
)

const scheduledAuctionCommand = 4901040

// Include the private projection in the tag. Personal escrow, new deliveries or
// an upgraded seat must invalidate an otherwise quiet public room.
func auctionWireView(view auctionView, previous string) (any, error) {
	now := view.Now
	view.Now = 0
	content, err := json.Marshal(view)
	if err != nil {
		return nil, err
	}
	digest := sha256.Sum256(content)
	tag := hex.EncodeToString(digest[:16])
	if previous == tag {
		return Record{"unchanged": true, "eventId": view.EventID, "tier": view.Tier, "now": now, "viewTag": tag}, nil
	}
	view.Now = now
	return struct {
		auctionView
		ViewTag string `json:"viewTag"`
	}{view, tag}, nil
}

func sendAuctionJSON(peer *Peer, msg Message, code int, body any) {
	data, err := json.Marshal(body)
	if err != nil {
		log.Printf("encode auction reply: %v", err)
		code, data = 5000, []byte(`{"error":"拍册暂时无法读取"}`)
	}
	wire := offerNumber(nil, 1, uint64(code))
	if code == 2000 {
		wire = offerNumber(wire, 2, 1)
	}
	_ = peer.sendPayload(msg.Cmd, msg.Msid, offerString(wire, 3, string(data)))
}

// The game connection supplies the identity. Auction clients never select a
// wallet or a character by including a role ID in their JSON.
func (s *Server) handleScheduledAuction(peer *Peer, msg Message) {

	role := peer.currentRoleID()
	current, online := s.online.Load(role)
	if !s.previewWrites || role == "" || !online || current != peer {
		sendAuctionJSON(peer, msg, 4008, Record{"error": "会场尚未开放，或角色已离线"})
		return
	}
	var order auctionAction
	if len(msg.Data) > 2048 || json.Unmarshal([]byte(offerToken(msg.Data)), &order) != nil {
		sendAuctionJSON(peer, msg, 4008, Record{"error": "拍卖请求格式无效"})
		return
	}
	result, err := s.scheduledAuctionAction(role, order, time.Now())
	if err == nil {
		var body any
		previous := ""
		if order.Action == "view" {
			previous = order.ViewTag
		}
		body, err = auctionWireView(result, previous)
		if err == nil {
			sendAuctionJSON(peer, msg, 2000, body)
			return
		}
	}
	var refusal auctionRefusal
	if errors.As(err, &refusal) {
		sendAuctionJSON(peer, msg, 4008, Record{"error": refusal.Error()})
		return
	}
	log.Printf("auction %q: %v", order.Action, err)
	sendAuctionJSON(peer, msg, 5000, Record{"error": "拍册回执尚待核对，请稍后重试", "retry": true})
}

// One calendar timer serves all fairs, including empty halls. Between events
// it checks at most once a minute. Client requests also advance a due event.
func (s *Server) runScheduledAuctions(ctx context.Context) {
	timer := time.NewTimer(0)
	defer timer.Stop()
	for {
		select {
		case <-ctx.Done():
			return
		case <-timer.C:
		}
		now := time.Now()
		if err := s.advanceScheduledAuctions(now); err != nil {
			log.Printf("拍卖日程未能保存，将重试: %v", err)
			timer.Reset(5 * time.Second)
			continue
		}
		next := now.Add(time.Minute).UnixMilli()
		s.roles.mu.RLock()
		calendar := s.roles.records[scheduledAuctionStoreID]
		for _, tier := range auctionTiers {
			event := auctionMap(calendar, tier)
			for _, field := range [...]string{"preview", "nextSettlement"} {
				if deadline := roleInt(event, field); deadline > now.UnixMilli() && deadline < next {
					next = deadline
				}
			}
		}
		s.roles.mu.RUnlock()
		timer.Reset(max(0, time.Until(time.UnixMilli(next))))
	}
}
