package main

import (
	"bytes"
	"crypto/sha256"
	"reflect"

	"google.golang.org/protobuf/encoding/protowire"
	"google.golang.org/protobuf/proto"
	"google.golang.org/protobuf/reflect/protoreflect"
)

const (
	inventoryRequestVersionField = 2
	inventoryReplyLocationField  = 6
	inventoryReplyVersionField   = 7
	inventoryReplyUnchangedField = 8
	inventoryVersionLength       = 16
)

type inventoryView struct {
	rows           []any
	used, capacity int
	firstRoot      int
}

type inventorySnapshot struct {
	owner    string
	view     inventoryView
	md5      string
	version  [inventoryVersionLength]byte
	full     []byte
	metadata []byte
}

// Store publishes immutable records after a successful write. The returned
// slice is for read-only encoding and is independent of later transactions.
func (s *Store) inventoryRows(id string, location int64) (inventoryView, bool) {
	s.mu.RLock()
	defer s.mu.RUnlock()
	role := s.records[id]
	if role == nil {
		return inventoryView{}, false
	}
	if location == 0 || location == 10 {
		rows := goodsList(role)
		return inventoryView{rows: rows, used: len(rows), capacity: ringCapacity(role), firstRoot: firstCultivationRoot(role)}, true
	}
	if box, owned := storageBox(role, location); owned {
		return inventoryView{rows: goodsList(box), used: storageDisplayUsed(box), capacity: storageBoxCapacity(role), firstRoot: firstCultivationRoot(role)}, true
	}
	return inventoryView{capacity: 100}, true
}

func (s *Store) inventoryItem(roleID, goodsID string, equipped bool) Record {
	if goodsID == "" {
		return nil
	}
	s.mu.RLock()
	defer s.mu.RUnlock()
	role := s.records[roleID]
	if goods := locateGoods(role, goodsID); goods != nil {
		return cloneRecord(goods)
	}
	for _, value := range storageBoxes(role) {
		box, _ := value.(Record)
		if goods := locateGoods(box, goodsID); goods != nil {
			return cloneRecord(goods)
		}
	}
	if equipped {
		for _, value := range goodsListForField(role, "EquippedGoods") {
			if goods, ok := value.(Record); ok && roleString(goods, "GoodsId") == goodsID {
				return cloneRecord(goods)
			}
		}
	}
	return nil
}

func (s *Server) inventorySnapshot(peer *Peer, owner string, location int64, view inventoryView) (*inventorySnapshot, error) {
	slot := -1
	switch {
	case location == 0 || location == 10:
		slot = 0
	case location >= 1 && location <= storageSlots:
		slot = int(location)
	}
	if slot >= 0 {
		if cached := peer.inventory[slot]; cached != nil && cached.owner == owner &&
			cached.view.used == view.used && cached.view.capacity == view.capacity && cached.view.firstRoot == view.firstRoot && reflect.DeepEqual(cached.view.rows, view.rows) {
			cached.view = view
			return cached, nil
		}
	}
	reply := s.gameResponse("SpaceGoodsResp_1004002", true, 2000)
	putPB(reply, Record{"NowCapacity": view.used, "MaxCapacity": view.capacity})
	options := proto.MarshalOptions{Deterministic: true}
	metadata, err := options.Marshal(reply)
	if err != nil {
		return nil, err
	}
	for _, entry := range view.rows {
		if goods, ok := entry.(Record); ok {
			fillGameGoods(addMessage(reply, "GoodsInfos"), goods, view.firstRoot)
		}
	}
	full, err := options.Marshal(reply)
	if err != nil {
		return nil, err
	}
	snapshot := &inventorySnapshot{
		owner: owner, view: view, full: full, metadata: metadata,
	}
	digest := sha256.Sum256(full)
	copy(snapshot.version[:], digest[:])
	if slot >= 0 {
		peer.inventory[slot] = snapshot
	}
	return snapshot, nil
}

func inventoryRequestVersion(req protoreflect.Message) ([]byte, bool) {
	for data := req.GetUnknown(); len(data) != 0; {
		num, kind, n := protowire.ConsumeTag(data)
		if n < 0 {
			break
		}
		data = data[n:]
		if num == inventoryRequestVersionField && kind == protowire.BytesType {
			version, n := protowire.ConsumeBytes(data)
			if n >= 0 && (len(version) == 0 || len(version) == inventoryVersionLength) {
				return version, true
			}
			break
		}
		n = protowire.ConsumeFieldValue(num, kind, data)
		if n < 0 {
			break
		}
		data = data[n:]
	}
	return nil, false
}

func (s *Server) readInventory(peer *Peer, msg Message, req protoreflect.Message, response string) {
	owner, location := peer.currentRoleID(), getInt(req, "Location")
	view, exists := s.roles.inventoryRows(owner, location)
	if !exists {
		s.sendGame(peer, msg, s.gameResponse(response, false, 4007))
		return
	}
	snapshot, err := s.inventorySnapshot(peer, owner, location, view)
	if err != nil {
		s.sendGame(peer, msg, s.gameResponse(response, false, 5000))
		return
	}
	if msg.Cmd == 1004004 {
		if snapshot.md5 == "" {
			snapshot.md5 = gameGoodsMD5(Record{"Goods": snapshot.view.rows})
		}
		reply := s.gameResponse(response, true, 2000)
		putPB(reply, Record{"Location": location, "Md5": snapshot.md5})
		s.sendGame(peer, msg, reply)
		return
	}
	version, supportsVersion := inventoryRequestVersion(req)
	unchanged := supportsVersion && bytes.Equal(version, snapshot.version[:])
	body := snapshot.full
	if unchanged {
		body = snapshot.metadata
	}
	result := make([]byte, 0, len(body)+28)
	result = append(result, body...)
	if location != 0 {
		result = protowire.AppendVarint(protowire.AppendTag(result, inventoryReplyLocationField, protowire.VarintType), uint64(location))
	}
	if supportsVersion {
		result = protowire.AppendBytes(protowire.AppendTag(result, inventoryReplyVersionField, protowire.BytesType), snapshot.version[:])
		if unchanged {
			result = protowire.AppendVarint(protowire.AppendTag(result, inventoryReplyUnchangedField, protowire.VarintType), 1)
		}
	}
	_ = peer.sendPayload(msg.Cmd, msg.Msid, result)
}
