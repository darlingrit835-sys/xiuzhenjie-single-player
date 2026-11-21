package main

import (
	"maps"
	"math"
	"slices"
	"strings"

	"google.golang.org/protobuf/reflect/protoreflect"
)

const (
	mailBoxField  = "Letters"
	mailListLimit = 200
)

func trialLetters(role Record) Record {
	letters, _ := role[mailBoxField].(Record)
	return letters
}

func trialLetter(role Record, id string) Record {
	letter, _ := trialLetters(role)[id].(Record)
	return letter
}

func mailUnread(role Record) bool {
	for _, value := range trialLetters(role) {
		if letter, ok := value.(Record); ok && !roleBool(letter, "Status") {
			return true
		}
	}
	return false
}

func mailAwards(letter Record) []any {
	awards, _ := letter["Awards"].([]any)
	return awards
}

func mailPending(awards []any) bool {
	for _, value := range awards {
		award, ok := value.(Record)
		if !ok || !roleBool(award, "Status") {
			return true
		}
	}
	return false
}

func (s *Server) trialMailList(peer *Peer, _, out protoreflect.Message) (func(), error) {
	role := s.roles.project(peer.currentRoleID(), []string{mailBoxField})
	if role == nil {
		return nil, previewReject(4007)
	}
	letters := trialLetters(role)
	ids := slices.Collect(maps.Keys(letters))
	slices.SortFunc(ids, func(a, b string) int {
		ta := roleInt(trialLetter(role, a), "SentAt")
		tb := roleInt(trialLetter(role, b), "SentAt")
		if ta > tb {
			return -1
		}
		if ta < tb {
			return 1
		}
		return strings.Compare(a, b)
	})
	for _, id := range ids[:min(len(ids), mailListLimit)] {
		if letter := trialLetter(role, id); letter != nil {
			fillMail(addMessage(out, "Letters"), id, letter)
		}
	}
	statusPB(out, true, 4055)
	return nil, nil
}

func fillMail(out protoreflect.Message, id string, letter Record) {
	awards := mailAwards(letter)
	set(out, "Id", id)
	copyRolePB(out, letter, "Title", "Content", "Status")
	set(out, "HasAward", len(awards) != 0)
	set(out, "IsGetAll", !mailPending(awards))
	for _, value := range awards {
		award, ok := value.(Record)
		if !ok {
			continue
		}
		entry := addMessage(out, "Awards")
		copyRolePB(entry, award, "Id", "GoodsId", "GoodsName", "GoodsType", "GoodsCount", "Rank", "Status")
	}
}

func (s *Server) trialMailRead(peer *Peer, req, out protoreflect.Message) (func(), error) {
	owner, id := peer.currentRoleID(), getString(req, "Id")
	err := s.roles.Transact(func(tx *StoreTx) error {
		letter := trialLetter(tx.View(owner), id)
		if letter == nil {
			return previewReject(4203)
		}
		if !roleBool(letter, "Status") {
			trialLetter(tx.Get(owner), id)["Status"] = true
		}
		return nil
	})
	if err == nil {
		statusPB(out, true, 4066)
	}
	return nil, err
}

func (s *Server) trialMailDelete(peer *Peer, req, out protoreflect.Message) (func(), error) {
	owner, ids := peer.currentRoleID(), req.Get(field(req, "Id")).List()
	if ids.Len() < 1 || ids.Len() > mailListLimit {
		return nil, previewReject(5001)
	}
	err := s.roles.Transact(func(tx *StoreTx) error {
		role := tx.View(owner)
		if role == nil {
			return previewReject(4007)
		}
		existing := make([]string, 0, ids.Len())
		for i := 0; i < ids.Len(); i++ {
			id := ids.Get(i).String()
			if id == "" {
				return previewReject(5001)
			}
			if letter := trialLetter(role, id); letter != nil {
				if !roleBool(letter, "Status") || mailPending(mailAwards(letter)) {
					return previewReject(4008)
				}
				existing = append(existing, id)
			}
		}
		if len(existing) != 0 {
			box := trialLetters(tx.Get(owner))
			for _, id := range existing {
				delete(box, id)
			}
		}
		return nil
	})
	if err == nil {
		statusPB(out, true, 4054)
	}
	return nil, err
}

func (s *Server) trialMailClaim(peer *Peer, req, out protoreflect.Message) (func(), error) {
	owner, id := peer.currentRoleID(), getString(req, "Id")
	var stone, jade int64
	err := s.roles.Transact(func(tx *StoreTx) error {
		role := tx.View(owner)
		letter := trialLetter(role, id)
		if letter == nil {
			return previewReject(4203)
		}
		awards := mailAwards(letter)
		if len(awards) == 0 {
			return previewReject(4204)
		}
		if mailPending(awards) {
			role = tx.Get(owner)
			letter = trialLetter(role, id)
			var count int
			for _, value := range mailAwards(letter) {
				award, ok := value.(Record)
				if !ok {
					return previewReject(5001)
				}
				if roleBool(award, "Status") {
					continue
				}
				if err := receiveMailAward(role, award); err != nil {
					return err
				}
				award["Status"] = true
				count++
			}
			letter["Status"] = true
			event := newHistoryEvent("mail.attachments_claimed", role, nil, "letter", id, Record{"attachments": count})
			event.Actor.RoleID = owner
			if err := tx.RecordHistory(event); err != nil {
				return err
			}
		}
		stone, jade = roleInt(role, "ShinyStone"), roleInt(role, "ShinyJade")
		return nil
	})
	if err == nil {
		statusPB(out, true, 4206)
		set(out, "ShinyStone", stone)
		set(out, "ShinyJade", jade)
	}
	return nil, err
}

func receiveMailAward(role, award Record) error {
	count, kind := roleInt(award, "GoodsCount"), roleInt(award, "GoodsType")
	if count <= 0 {
		return previewReject(5001)
	}
	if kind == 99 || kind == 100 { // Original PB inventory kinds for jade and stone.
		field, limit := "ShinyStone", int64(math.MaxInt64)
		if kind == 99 {
			field, limit = "ShinyJade", math.MaxInt32 // Unity stores the balance as int.
		}
		balance := roleInt(role, field)
		if balance < 0 || count > limit-balance {
			return previewReject(4205)
		}
		role[field] = balance + count
		return nil
	}
	goods, _ := award["Goods"].(Record)
	if kind <= 0 || goods == nil || roleInt(goods, "Type") != kind || count > math.MaxInt32 ||
		(isGameEquipment(kind) && count != 1) || roleString(goods, "Name") == "" {
		return previewReject(5001)
	}
	if _, err := receiveInventoryGoods(role, goods, count); err != nil {
		return previewReject(4205)
	}
	return nil
}
