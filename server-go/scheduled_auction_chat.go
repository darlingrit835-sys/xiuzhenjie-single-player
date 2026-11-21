package main

import (
	"bytes"
	"encoding/binary"
	"encoding/json"
	"fmt"
	"path/filepath"
	"strconv"
	"strings"
	"time"
	"unicode"
	"unicode/utf8"

	bolt "go.etcd.io/bbolt"
)

type auctionChatLine struct {
	ID     string `json:"id"`
	LotID  string `json:"lotId"`
	Name   string `json:"name"`
	Room   string `json:"room"`
	Text   string `json:"text"`
	Effect string `json:"effect"`
	At     int64  `json:"at"`
}

type auctionChatWindow struct {
	event, lot string
	sequence   uint64
	lines      []auctionChatLine
}

type auctionService struct {
	ID       string `json:"id"`
	Name     string `json:"name"`
	Price    int64  `json:"price"`
	Quota    int64  `json:"quota"`
	Cooldown int64  `json:"cooldown"`
	Effect   string `json:"effect"`
}

var auctionServices = []auctionService{
	{"free", "散席", 0, 3, 20, "ink"},
	{"fruit", "灵果盘", 180, 8, 12, "jade"},
	{"wine", "琥珀灵酿", 680, 18, 6, "amber"},
	{"room", "天字号雅间", 2400, 36, 3, "gold"},
}

func auctionServiceByID(id string) auctionService {
	for _, svc := range auctionServices {
		if svc.ID == id {
			return svc
		}
	}
	return auctionServices[0]
}

func auctionSeat(e Record, roleID string) Record {
	seat := auctionMap(auctionMap(e, "seats"), roleID)
	svc := auctionServiceByID(roleString(seat, "service"))
	room := roleString(seat, "room")
	if room == "" {
		room = "散席"
	}
	return Record{"service": svc.ID, "room": room, "effect": svc.Effect, "remaining": svc.Quota, "cooldown": svc.Cooldown, "nextAt": int64(0)}
}

func buyAuctionService(tx *StoreTx, e Record, roleID, target string, ms int64) error {
	if phase := auctionPhase(e, ms); roleBool(e, "finished") || phase != "live" && phase != "preview" {
		return auctionRefusal("筵席在预展及开槌期间开放")
	}
	old := auctionServiceByID(roleString(auctionSeat(e, roleID), "service"))
	svc := auctionServiceByID(target)
	if svc.Price <= old.Price {
		return auctionRefusal("请选择更高一档的席面")
	}
	actor := tx.Get(roleID)
	balance := roleInt(actor, "ShinyStone")
	if balance < svc.Price-old.Price {
		return auctionRefusal("灵石不足以升级席面")
	}
	actor["ShinyStone"] = balance - (svc.Price - old.Price)
	seat := auctionMapEdit(auctionMapEdit(e, "seats"), roleID)
	seat["service"] = target
	switch target {
	case "room":
		number := roleInt(e, "nextRoom") + 1
		e["nextRoom"] = number
		seat["room"] = fmt.Sprintf("天字%d号", number)
	case "wine":
		seat["room"] = "酒席"
	case "fruit":
		seat["room"] = "果席"
	}
	return nil
}

func auctionLiveLot(e Record, ms int64) string {
	if auctionPhase(e, ms) != "live" {
		return ""
	}
	n := (ms - roleInt(e, "live")) / (roleInt(e, "seconds") * 1000)
	lots := goodsListForField(e, "lots")
	if n < 0 || n >= int64(len(lots)) {
		return ""
	}
	return roleString(lots[n].(Record), "id")
}

// Financial operations stay in roles.json. Room chatter uses separate bounded
// Bolt buckets so it never rewrites that file or takes the economic write lock.
func (s *Server) sendScheduledChat(roleID string, req auctionAction, now time.Time) error {
	msg := strings.TrimSpace(req.Text)
	if !utf8.ValidString(msg) || msg == "" || utf8.RuneCountInString(msg) > 60 || strings.IndexFunc(msg, unicode.IsControl) >= 0 || strings.ContainsAny(msg, "<>") {
		return auctionRefusal("请发一至六十字的纯文字传音")
	}
	ms := now.UnixMilli()
	s.roles.mu.RLock()
	e := auctionMap(s.roles.records[scheduledAuctionStoreID], req.Tier)
	liveLot := auctionLiveLot(e, ms)
	role := s.roles.records[roleID]
	name := roleString(role, "Name")
	seat := auctionSeat(e, roleID)
	valid := role != nil && req.EventID == roleString(e, "id") && liveLot != "" && liveLot == req.LotID
	s.roles.mu.RUnlock()
	if !valid {
		return auctionRefusal("传音已封，请刷新当前拍品")
	}
	db, err := s.privateChat.open(filepath.Dir(s.roles.path))
	if err != nil {
		return err
	}
	return db.Update(func(tx *bolt.Tx) error {
		key := []byte("auction-chat/" + req.Tier)
		room := tx.Bucket(key)
		if room != nil {
			stored := string(room.Get([]byte("event")))
			if stored > req.EventID {
				return auctionRefusal("传音场次已过")
			}
			if stored != req.EventID {
				if err := tx.DeleteBucket(key); err != nil {
					return err
				}
				room = nil
			}
		}
		if room == nil {
			var err error
			room, err = tx.CreateBucket(key)
			if err != nil {
				return err
			}
			if err = room.Put([]byte("event"), []byte(req.EventID)); err != nil {
				return err
			}
		}
		actors, err := room.CreateBucketIfNotExists([]byte("actors"))
		if err != nil {
			return err
		}
		actor, err := actors.CreateBucketIfNotExists([]byte(roleID))
		if err != nil {
			return err
		}
		digest := []byte(auctionFingerprint(req))
		receipt := []byte("receipt/" + req.RequestID)
		if prior := actor.Get(receipt); prior != nil {
			if bytes.Equal(prior, digest) {
				return nil
			}
			return auctionRefusal("传音凭据重复")
		}
		used, last := auctionChatInt(actor, "used"), auctionChatInt(actor, "last")
		if used >= roleInt(seat, "remaining") {
			return auctionRefusal("本场传音次数已用尽")
		}
		if ms < last+roleInt(seat, "cooldown")*1000 {
			return auctionRefusal("传音调息中，请稍候")
		}
		logs, err := room.CreateBucketIfNotExists([]byte("messages"))
		if err != nil {
			return err
		}
		seq, err := logs.NextSequence()
		if err != nil {
			return err
		}
		row := Record{"id": req.EventID + "-" + strconv.FormatUint(seq, 10), "lotId": liveLot, "name": name, "text": msg,
			"effect": roleString(seat, "effect"), "room": roleString(seat, "room"), "at": ms}
		payload, err := json.Marshal(row)
		if err != nil {
			return err
		}
		if err = logs.Put(binary.BigEndian.AppendUint64(nil, seq), payload); err != nil {
			return err
		}
		if seq > 32 {
			if err = logs.Delete(binary.BigEndian.AppendUint64(nil, seq-32)); err != nil {
				return err
			}
		}
		if err = actor.Put([]byte("used"), binary.BigEndian.AppendUint64(nil, uint64(used+1))); err != nil {
			return err
		}
		if err = actor.Put([]byte("last"), binary.BigEndian.AppendUint64(nil, uint64(ms))); err != nil {
			return err
		}
		return actor.Put(receipt, digest)
	})
}

func auctionChatInt(b *bolt.Bucket, key string) int64 {
	if b != nil {
		if p := b.Get([]byte(key)); len(p) == 8 {
			return int64(binary.BigEndian.Uint64(p))
		}
	}
	return 0
}

func (s *Server) readScheduledChat(v *auctionView, roleID string) error {
	if v.Phase != "live" {
		return nil
	}
	var lotID string
	for _, lot := range v.Lots {
		if lot.Status == "live" {
			lotID = lot.ID
			break
		}
	}
	db, err := s.privateChat.open(filepath.Dir(s.roles.path))
	if err != nil {
		return err
	}
	return db.View(func(tx *bolt.Tx) error {
		room := tx.Bucket([]byte("auction-chat/" + v.Tier))
		if room == nil || string(room.Get([]byte("event"))) != v.EventID {
			return nil
		}
		if actors := room.Bucket([]byte("actors")); actors != nil {
			actor := actors.Bucket([]byte(roleID))
			v.Seat["remaining"] = max(0, roleInt(v.Seat, "remaining")-auctionChatInt(actor, "used"))
			if last := auctionChatInt(actor, "last"); last > 0 {
				v.Seat["nextAt"] = last + roleInt(v.Seat, "cooldown")*1000
			}
		}
		logs := room.Bucket([]byte("messages"))
		if logs == nil {
			return nil
		}
		seq := logs.Sequence()
		if value, ok := s.auctionChatCache.Load(v.Tier); ok {
			cached := value.(auctionChatWindow)
			if cached.event == v.EventID && cached.lot == lotID && cached.sequence == seq {
				v.Messages = cached.lines
				return nil
			}
		}
		err := logs.ForEach(func(_, p []byte) error {
			var line auctionChatLine
			if err := json.Unmarshal(p, &line); err != nil {
				return err
			}
			if line.LotID == lotID {
				v.Messages = append(v.Messages, line)
			}
			return nil
		})
		if err != nil {
			return err
		}
		s.auctionChatCache.Store(v.Tier, auctionChatWindow{v.EventID, lotID, seq, v.Messages})

		return nil
	})
}
