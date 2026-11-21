package main

import (
	"math"
	"strconv"

	"google.golang.org/protobuf/reflect/protoreflect"
)

const (
	trialExchangeID   = "$preview:exchange"
	trialExchangeRate = 100
)

type trialReceipt struct {
	cmd, seq int
	owner    string
	key      string
	fields   Record
}

type trialReceipts struct {
	recent [16]trialReceipt
	next   int
}

func (peer *Peer) replayTrialReceipt(cmd int, key string, out protoreflect.Message) (bool, error) {
	if peer.trialReceipts == nil || peer.gameRequestID <= 0 {
		return false, nil
	}
	for _, previous := range peer.trialReceipts.recent {
		if previous.cmd != cmd || previous.seq != peer.gameRequestID || previous.owner != peer.currentRoleID() {
			continue
		}
		if previous.key != key {
			return false, previewReject(4008)
		}
		putPB(out, previous.fields)
		return true, nil
	}
	return false, nil
}

func (peer *Peer) rememberTrialReceipt(cmd int, key string, fields Record, out protoreflect.Message) {
	putPB(out, fields)
	if peer.gameRequestID <= 0 {
		return
	}
	if peer.trialReceipts == nil {
		peer.trialReceipts = new(trialReceipts)
	}
	receipts := peer.trialReceipts
	receipts.recent[receipts.next] = trialReceipt{cmd, peer.gameRequestID, peer.currentRoleID(), key, fields}
	receipts.next = (receipts.next + 1) % len(receipts.recent)
}

func (s *Server) trialExchangeReserve(state Record) int64 {
	if state == nil {
		return max(0, s.config.TrialExchangeStoneReserve)
	}
	return max(0, roleInt(state, "Remaining"))
}

func (s *Server) readTrialExchange(_ *Peer, _, out protoreflect.Message) (func(), error) {
	state := s.roles.project(trialExchangeID, []string{"Remaining"})
	previewExchangeRate(nil, nil, out)
	set(out, "ExchangeShinyStoneSum", s.trialExchangeReserve(state))
	return nil, nil
}

func (s *Server) trialExchange(peer *Peer, req, out protoreflect.Message) (func(), error) {
	owner, jade := peer.currentRoleID(), getInt(req, "ShinyJade")
	if jade < 10 || jade > math.MaxInt32 {
		return nil, previewReject(4361)
	}
	key := strconv.FormatInt(jade, 10)
	if replay, err := peer.replayTrialReceipt(1002017, key, out); replay || err != nil {
		if replay {
			copyRolePB(out, s.roles.project(owner, []string{"ShinyStone", "ShinyJade"}), "ShinyStone", "ShinyJade")
		}
		return nil, err
	}
	stone := jade * trialExchangeRate
	var fields Record
	err := s.roles.Transact(func(tx *StoreTx) error {
		role := tx.View(owner)
		if role == nil {
			return previewReject(4007)
		}
		balance := roleInt(role, "ShinyJade")
		if balance < jade || balance > math.MaxInt32 {
			return previewReject(4362)
		}
		remaining := s.trialExchangeReserve(tx.View(trialExchangeID))
		if remaining < stone {
			return previewReject(4363)
		}
		ownedStone := roleInt(role, "ShinyStone")
		if ownedStone < 0 || ownedStone > math.MaxInt64-stone {
			return previewReject(4365)
		}
		role = tx.Get(owner)
		role["ShinyJade"], role["ShinyStone"] = balance-jade, ownedStone+stone
		tx.GetOrCreate(trialExchangeID)["Remaining"] = remaining - stone
		fields = Record{"Code": 2000, "IsSuccess": true, "ShinyStone": ownedStone + stone, "ShinyJade": balance - jade}
		event := newHistoryEvent("wallet.exchanged", role, nil, "wallet", "jade-stone",
			Record{"jadeSpent": jade, "stoneReceived": stone})
		event.Actor.RoleID = owner
		return tx.RecordHistory(event)
	})
	if err == nil {
		peer.rememberTrialReceipt(1002017, key, fields, out)
	}
	return nil, err
}
