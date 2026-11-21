package main

import (
	"fmt"
	"math"
	"math/rand"
	"slices"
	"strconv"
	"strings"
	"time"

	"google.golang.org/protobuf/reflect/protoreflect"
)

const bountySequence = "$preview:guild-bounties"

type bountyMaterial struct {
	id, name                 string
	kind, level, rank, price int
}

var bountyMaterials = func() []bountyMaterial {
	var list []bountyMaterial
	for kind := 9; kind <= 13; kind++ {
		for level := 1; level <= 90; level++ {
			lastRank := 0
			for offset := range 3 {
				rank := battleMaterialRank(level, offset)
				if rank != lastRank {
					list = append(list, bountyMaterial{battleDropTemplateID(kind, rank, level), battleMaterials[kind-8], kind, level, rank, battleMaterialPrice(rank)})
					lastRank = rank
				}
			}
		}
	}
	return list
}()

// Membership and offices live in the guild file. Keep them stable until the
// role transaction commits its contribution, wallet, order and inventory together.
func (s *Server) withBountyGuild(peer *Peer, action func(trialGuild, *StoreTx, string) error) error {
	owner := peer.currentRoleID()
	if owner == "" || reservedStoreID(owner) || s.guilds == nil {
		return previewReject(4007)
	}
	s.guilds.mu.RLock()
	defer s.guilds.mu.RUnlock()
	guild := trialGuildForRole(trialGuilds(s.guilds.records[trialGuildWorld]), owner)
	if guild.row == nil {
		return previewReject(4420)
	}
	return s.roles.Transact(func(tx *StoreTx) error {
		if tx.View(owner) == nil {
			return previewReject(4007)
		}
		return action(guild, tx, owner)
	})
}

func bountyAccount(owner Record, guildID int64, create bool) Record {
	accounts, _ := owner["GuildAccounts"].(Record)
	if accounts == nil && create {
		accounts = Record{}
		owner["GuildAccounts"] = accounts
	}
	key := strconv.FormatInt(guildID, 10)
	account, _ := accounts[key].(Record)
	if account == nil && create {
		account = Record{}
		accounts[key] = account
	}
	return account
}

func bountyOrders(account Record) []any {
	orders, _ := account["Bounties"].([]any)
	return orders
}

func bountyElder(guild trialGuild, tx *StoreTx) (string, Record, int64) {
	id := roleString(guild.offices(), "3")
	if id == "" || !guild.hasMember(id) {
		return "", nil, 0
	}
	elder := tx.View(id)
	if elder == nil {
		return "", nil, 0
	}
	return id, elder, (max(1, roleInt(elder, "Level"))-1)/10 + 1
}

func (s *Server) guildBountyList(peer *Peer, _, out protoreflect.Message) (func(), error) {
	return nil, s.withBountyGuild(peer, func(guild trialGuild, tx *StoreTx, owner string) error {
		account := bountyAccount(tx.View(owner), guild.id, false)
		set(out, "CurrentContribution", roleInt(account, "Contribution"))
		if id, elder, bonus := bountyElder(guild, tx); elder != nil {
			info := out.Mutable(field(out, "ZhangLaoInfo")).Message()
			fillTrialGuildRole(info, id, elder)
			putPB(info, Record{"Fighting": roleInt(elder, "CurrentPower"), "TeXiaoVal": bonus})
		}
		now := time.Now().UnixMilli()
		for _, value := range bountyOrders(account) {
			order, _ := value.(Record)
			remaining := max(0, roleInt(order, "Due")-now)
			status := 0
			if remaining == 0 {
				status = 1
			}
			putPB(addMessage(out, "XuanShangInfo"), Record{"Id": roleInt(order, "Id"), "GoodsName": roleString(order, "Name"), "Stone": roleInt(order, "Stone"), "Status": status, "SuccessRate": roleInt(order, "Rate"), "ShengYuTime": remaining})
		}
		return nil
	})
}

func (s *Server) guildBountySearch(peer *Peer, req, out protoreflect.Message) (func(), error) {
	kind, name := int(getInt(req, "GoodsType")), strings.TrimSpace(getString(req, "Name"))
	if kind < 9 || kind > 13 || name == "" {
		return nil, previewReject(5001)
	}
	return nil, s.withBountyGuild(peer, func(_ trialGuild, _ *StoreTx, _ string) error {
		for _, item := range bountyMaterials {
			if item.kind == kind && item.name == name {
				// This world's drop template rolls its element per instance.
				putPB(addMessage(out, "GoodsInfoList"), Record{"Id": item.id, "Name": item.name, "Rank": item.rank, "Level": item.level, "RecyclePrice": item.price})
			}
		}
		return nil
	})
}

func (s *Server) guildBountyPublish(peer *Peer, req, out protoreflect.Message) (func(), error) {
	kind, id, offer := int(getInt(req, "GoodsType")), getString(req, "GoodsTemplateId"), getInt(req, "Award")
	index := slices.IndexFunc(bountyMaterials, func(m bountyMaterial) bool { return m.kind == kind && m.id == id })
	if index < 0 || offer <= 0 {
		return nil, previewReject(5001)
	}
	item := bountyMaterials[index]
	key := fmt.Sprintf("%s/%d", id, offer)
	if done, err := peer.replayTrialReceipt(1015059, key, out); done || err != nil {
		return nil, err
	}
	var fields Record
	err := s.withBountyGuild(peer, func(guild trialGuild, tx *StoreTx, owner string) error {
		role := tx.Get(owner)
		account := bountyAccount(role, guild.id, true)
		orders := bountyOrders(account)
		if len(orders) >= 2 {
			return previewReject(4666)
		}
		cost := int64(item.level * 20)
		if roleInt(account, "Contribution") < cost {
			return previewReject(4664)
		}
		if roleInt(role, "ShinyStone") < offer {
			return previewReject(4665)
		}
		_, _, bonus := bountyElder(guild, tx)
		chance := min(100, offer/int64(item.price*20)*5+bonus)
		seq := tx.GetOrCreate(bountySequence)
		next := roleInt(seq, "LastId") + 1
		if next <= 0 || next > math.MaxInt32 {
			return previewReject(5000)
		}
		seq["LastId"] = next
		role["ShinyStone"] = roleInt(role, "ShinyStone") - offer
		account["Contribution"] = roleInt(account, "Contribution") - cost
		order := Record{"Id": next, "Name": item.name, "TemplateId": item.id, "Type": kind, "Rank": item.rank, "Level": item.level,
			"Stone": offer, "Rate": chance, "Due": time.Now().Add(time.Hour).UnixMilli(), "Found": int64(rand.Intn(100)) < chance,
			"FiveElement": rand.Intn(5) + 1}
		account["Bounties"] = append(orders, order)
		fields = Record{"Code": 4684, "CurrentStone": roleInt(role, "ShinyStone")}
		return nil
	})
	if err == nil {
		peer.rememberTrialReceipt(1015059, key, fields, out)
	}
	return nil, err
}

func (s *Server) guildBountySettle(peer *Peer, req, out protoreflect.Message) (func(), error) {
	id := getInt(req, "Id")
	if id <= 0 {
		return nil, previewReject(5001)
	}
	key := strconv.FormatInt(id, 10)
	if done, err := peer.replayTrialReceipt(1015041, key, out); done || err != nil {
		return nil, err
	}
	var fields Record
	var message string
	err := s.withBountyGuild(peer, func(guild trialGuild, tx *StoreTx, owner string) error {
		role := tx.Get(owner)
		account := bountyAccount(role, guild.id, false)
		orders := bountyOrders(account)
		i := slices.IndexFunc(orders, func(v any) bool { o, _ := v.(Record); return roleInt(o, "Id") == id })
		if i < 0 {
			return previewReject(4663)
		}
		order := orders[i].(Record)
		if roleInt(order, "Due") > time.Now().UnixMilli() {
			return previewReject(4662)
		}
		name := roleString(order, "Name")
		message = "寻物已结束，宗门弟子未能寻到" + name + "。"
		if roleBool(order, "Found") {
			rank := int(roleInt(order, "Rank"))
			goods := Record{"Name": name, "Type": roleInt(order, "Type"), "TemplateId": roleString(order, "TemplateId"),
				"Rank": rank, "Level": roleInt(order, "Level"), "FiveElement": roleInt(order, "FiveElement"), "RecyclePrice": battleMaterialPrice(rank),
				"Volume": 1.0, "ConversionRate": 1.0, "IsLock": false, "IsUsing": false, "IsBinding": false}
			if _, err := receiveInventoryGoods(role, goods, 1); err != nil {
				return err
			}
			message = "悬赏寻得" + name + "，已收入纳戒。"
		}
		account["Bounties"] = slices.Delete(orders, i, i+1)
		fields = Record{"CurrentContribution": roleInt(account, "Contribution")}
		return nil
	})
	if err != nil {
		return nil, err
	}
	peer.rememberTrialReceipt(1015041, key, fields, out)
	return func() { s.gameNotice(peer, message) }, nil
}
