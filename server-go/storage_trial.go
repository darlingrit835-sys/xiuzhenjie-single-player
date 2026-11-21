package main

import (
	"errors"
	"math"
	"strconv"
	"strings"
	"unicode"
	"unicode/utf8"

	"google.golang.org/protobuf/reflect/protoreflect"
)

const (
	baseStorageCapacity = 200
	storageSlots        = 5
)

var chamberStorageRoutes = []previewRoute{
	{cmd: 1005005, module: "洞府与修炼", response: "EnterSpaceBoxResp_1005005", summary: "读取本人已拥有的储物箱", roleFields: []string{"StorageBoxes"}, build: fillStorageList, live: (*Server).trialStorageList},
	{cmd: 1005007, module: "洞府与修炼", request: "RenameBoxReq_1005007", response: "RenameBoxResp_1005007", summary: "重命名已拥有的储物箱", blocked: true, live: (*Server).trialStorageRename},
	{cmd: 1005021, module: "洞府与修炼", response: "BuySpaceBoxResp_1005021", summary: "购买下一个储物箱并读取实际余额", blocked: true, live: (*Server).trialStorageBuy},
	{cmd: 1005022, module: "洞府与修炼", request: "PutInSpaceBoxReq_1005022", response: "PutInSpaceBoxResp_1005022", summary: "将指定数量的纳戒物品存入储物箱", blocked: true, live: (*Server).trialStoragePut},
	{cmd: 1005023, module: "洞府与修炼", request: "GetOutSpaceBoxReq_1005023", response: "GetOutSpaceBoxResp_1005023", summary: "从指定储物箱取回物品到纳戒", blocked: true, live: (*Server).trialStorageGet},
}

func storageRoutes() []previewRoute {
	return append([]previewRoute{
		{cmd: 1004022, module: "角色与纳戒", request: "GoodsSellReq_1004022", response: "GoodsSellResp_1004022", summary: "按回收单价出售所选物品并更新纳戒与余额", blocked: true, live: (*Server).sellRingGoods},
		{cmd: 1004025, module: "角色与纳戒", response: "GradeRingResp_1004025", summary: "消耗灵玉增加本人纳戒的格数", blocked: true, live: (*Server).expandRing},
		{cmd: 1004039, module: "角色与纳戒", request: "GoodsLockReq_1004039", response: "GoodsLockResp_1004039", summary: "锁定本人纳戒中的物品", blocked: true, live: (*Server).trialLockGoods},
		{cmd: 1004040, module: "角色与纳戒", request: "GoodsUnLockReq_1004040", response: "GoodsUnLockResp_1004040", summary: "解锁本人纳戒中的物品", blocked: true, live: (*Server).trialUnlockGoods},
	}, chamberStorageRoutes...)
}

func (s *Server) handleChamberStorage(peer *Peer, msg Message) {
	for _, route := range chamberStorageRoutes {
		if route.cmd != msg.Cmd {
			continue
		}
		owner := peer.currentRoleID()
		if active, ok := s.online.Load(owner); !ok || active != peer || owner == "" || reservedStoreID(owner) {
			s.sendGame(peer, msg, s.gameResponse(route.response, false, 4007))
			return
		}
		if s.preview != nil {
			s.handlePreview(peer, msg)
			return
		}
		if len(msg.Data) > maxPreviewRequest || (route.request == "" && len(msg.Data) != 0) {
			s.sendGame(peer, msg, s.gameResponse(route.response, false, 4008))
			return
		}
		var req protoreflect.Message
		if route.request != "" {
			decoded, err := s.decode(route.request, msg.Data)
			if err != nil {
				s.sendGame(peer, msg, s.gameResponse(route.response, false, 4008))
				return
			}
			req = decoded
		}
		s.runLiveRoute(peer, msg, route, req)
		return
	}
}

func storageBoxes(role Record) []any {
	boxes, _ := role["StorageBoxes"].([]any)
	return boxes[:min(len(boxes), storageSlots)]
}

func defaultStorageName(location int64) string {
	return "储物箱 " + strconv.FormatInt(location, 10)
}

func storageBoxCapacity(role Record) int {
	return baseStorageCapacity + 50*int(gameHomeRank(role))
}

func storageBox(role Record, location int64) (Record, bool) {
	if location < 1 || location > storageSlots || role == nil {
		return nil, false
	}
	boxes := storageBoxes(role)
	if len(boxes) == 0 {
		return nil, location == 1
	}
	if location > int64(len(boxes)) {
		return nil, false
	}
	box, _ := boxes[location-1].(Record)
	return box, box != nil
}

func editStorageBox(role Record, location int64) Record {
	if len(storageBoxes(role)) == 0 {
		role["StorageBoxes"] = []any{Record{"Name": defaultStorageName(1)}}
	}
	box, _ := storageBox(role, location)
	return box
}

func storageGoodsVolume(goods Record) float64 {
	volume := roleFloat(goods, "Volume")
	if volume <= 0 || math.IsNaN(volume) || math.IsInf(volume, 0) {
		return 1
	}
	return volume
}

func storageUsed(box Record) float64 {
	var total float64
	for _, row := range goodsList(box) {
		if goods, ok := row.(Record); ok {
			total += storageGoodsVolume(goods) * float64(max(1, transferStackCount(goods)))
		}
	}
	return total
}

func storageDisplayUsed(box Record) int {
	return int(math.Ceil(min(storageUsed(box), float64(math.MaxInt32)) - 1e-9))
}

func (s *Server) trialStorageList(peer *Peer, _, out protoreflect.Message) (func(), error) {
	role := s.roles.project(peer.currentRoleID(), []string{"StorageBoxes"})
	if role == nil {
		return nil, previewReject(4007)
	}
	fillStorageList(role, nil, out)
	return nil, nil
}

func fillStorageList(role Record, _ protoreflect.Message, out protoreflect.Message) {
	names := out.Mutable(field(out, "Name")).List()
	for index, value := range storageBoxes(role) {
		box, _ := value.(Record)
		name := roleString(box, "Name")
		if name == "" {
			name = defaultStorageName(int64(index + 1))
		}
		names.Append(protoreflect.ValueOfString(name))
	}
	if names.Len() == 0 {
		names.Append(protoreflect.ValueOfString(defaultStorageName(1)))
	}
	statusPB(out, true, 4120)
}

func (s *Server) trialStorageRename(peer *Peer, req, out protoreflect.Message) (func(), error) {
	owner, location := peer.currentRoleID(), getInt(req, "Location")
	name := strings.TrimSpace(getString(req, "NewName"))
	if name == "" || utf8.RuneCountInString(name) > 20 || strings.ContainsAny(name, "<>") ||
		strings.ContainsFunc(name, unicode.IsControl) {
		return nil, previewReject(5001)
	}
	key := strconv.FormatInt(location, 10) + ":" + name
	if replay, err := peer.replayTrialReceipt(1005007, key, out); replay || err != nil {
		return nil, err
	}
	var fields Record
	err := s.roles.Transact(func(tx *StoreTx) error {
		role := tx.View(owner)
		if _, exists := storageBox(role, location); !exists {
			return previewReject(5001)
		}
		jade := roleInt(role, "ShinyJade")
		if jade < 5 {
			return previewReject(4298)
		}
		role = tx.Get(owner)
		editStorageBox(role, location)["Name"] = name
		role["ShinyJade"] = jade - 5
		fields = Record{"Code": 2002, "IsSuccess": true, "ShinyJade": jade - 5}
		return nil
	})
	if err == nil {
		peer.rememberTrialReceipt(1005007, key, fields, out)
	}
	return nil, err
}

func (s *Server) trialStorageBuy(peer *Peer, _, out protoreflect.Message) (func(), error) {
	owner := peer.currentRoleID()
	if replay, err := peer.replayTrialReceipt(1005021, "", out); replay || err != nil {
		return nil, err
	}
	var fields Record
	err := s.roles.Transact(func(tx *StoreTx) error {
		role := tx.View(owner)
		if role == nil {
			return previewReject(4007)
		}
		owned := max(1, len(storageBoxes(role)))
		if owned >= storageSlots {
			return previewReject(4147)
		}
		prices := [...]struct {
			currency string
			amount   int64
		}{{"ShinyStone", 10000}, {"ShinyStone", 100000}, {"ShinyJade", 1000}, {"ShinyJade", 10000}}
		cost := prices[owned-1]
		balance := roleInt(role, cost.currency)
		if balance < cost.amount {
			return previewReject(4148)
		}
		role = tx.Get(owner)
		editStorageBox(role, 1)
		name := defaultStorageName(int64(owned + 1))
		role["StorageBoxes"] = append(storageBoxes(role), Record{"Name": name})
		role[cost.currency] = balance - cost.amount
		fields = Record{"Code": 4149, "IsSuccess": true, "Name": name,
			"ShinyStone": roleInt(role, "ShinyStone"), "ShinyJade": roleInt(role, "ShinyJade")}
		event := newHistoryEvent("storage.purchased", role, nil, "storage", strconv.Itoa(owned+1),
			Record{"currency": cost.currency, "amount": cost.amount})
		event.Actor.RoleID = owner
		return tx.RecordHistory(event)
	})
	if err == nil {
		peer.rememberTrialReceipt(1005021, "", fields, out)
	}
	return nil, err
}

func (s *Server) trialStoragePut(peer *Peer, req, out protoreflect.Message) (func(), error) {
	return s.trialStorageMove(peer, req, out, true)
}

func (s *Server) trialStorageGet(peer *Peer, req, out protoreflect.Message) (func(), error) {
	return s.trialStorageMove(peer, req, out, false)
}

func (s *Server) trialStorageMove(peer *Peer, req, out protoreflect.Message, deposit bool) (func(), error) {
	owner, id := peer.currentRoleID(), getString(req, "GoodsId")
	location, count := getInt(req, "Location"), getInt(req, "Count")
	cmd, missing, full, success := 1005023, 4157, 4158, 4159
	if deposit {
		cmd, missing, full, success = 1005022, 4154, 4155, 4156
	}
	if count <= 0 || id == "" {
		return nil, previewReject(missing)
	}
	key := strconv.FormatInt(location, 10) + ":" + strconv.FormatInt(count, 10) + ":" + id
	if replay, err := peer.replayTrialReceipt(cmd, key, out); replay || err != nil {
		return nil, err
	}
	var fields Record
	err := s.roles.Transact(func(tx *StoreTx) error {
		role := tx.View(owner)
		box, exists := storageBox(role, location)
		if !exists {
			return previewReject(missing)
		}
		source := box
		if deposit {
			source = role
		}
		goods := locateGoods(source, id)
		available := transferStackCount(goods)
		if goods == nil || count > available || (isGameEquipment(roleInt(goods, "Type")) && count != 1) {
			return previewReject(missing)
		}
		if inventoryFlag(goods, "IsUsing") || inventoryFlag(goods, "IsLock") {
			return previewReject(4509)
		}
		if deposit && storageUsed(box)+storageGoodsVolume(goods)*float64(count) > float64(storageBoxCapacity(role))+1e-9 {
			return previewReject(full)
		}
		role = tx.Get(owner)
		box = editStorageBox(role, location)
		source, destination, slots := box, role, ringCapacity(role)
		if deposit {
			source, destination, slots = role, box, 0
		}
		destinationID, err := receiveInventoryStack(destination, goods, count, slots)
		if errors.Is(err, previewReject(4527)) {
			return previewReject(full)
		}
		if err != nil {
			return err
		}
		removeStorageQuantity(source, id, available-count)
		fields = Record{"Code": success, "IsSuccess": true, "GoodsId": destinationID,
			"NowRingCapacity": len(goodsList(role)), "NowBoxCapacity": storageDisplayUsed(box)}
		kind := "storage.withdrawn"
		if deposit {
			kind = "storage.deposited"
		}
		event := newHistoryEvent(kind, role, nil, "storage", strconv.FormatInt(location, 10),
			Record{"goodsId": id, "count": count, "destinationId": destinationID})
		event.Actor.RoleID = owner
		return tx.RecordHistory(event)
	})
	if err == nil {
		peer.rememberTrialReceipt(cmd, key, fields, out)
	}
	return nil, err
}

func removeStorageQuantity(source Record, id string, left int64) {
	rows := goodsList(source)
	for i, entry := range rows {
		goods, _ := entry.(Record)
		if roleString(goods, "GoodsId") == id {
			if left > 0 {
				goods["Count"] = left
			} else {
				source["Goods"] = append(rows[:i], rows[i+1:]...)
			}
			return
		}
	}
}

func (s *Server) trialLockGoods(peer *Peer, req, out protoreflect.Message) (func(), error) {
	return s.trialSetGoodsLock(peer, req, out, true)
}

func (s *Server) trialUnlockGoods(peer *Peer, req, out protoreflect.Message) (func(), error) {
	return s.trialSetGoodsLock(peer, req, out, false)
}

func (s *Server) trialSetGoodsLock(peer *Peer, req, out protoreflect.Message, locked bool) (func(), error) {
	owner, id := peer.currentRoleID(), getString(req, "GoodsId")
	s.expireFriendLocks(owner)
	if id == "" {
		return nil, previewReject(5001)
	}
	err := s.roles.Transact(func(tx *StoreTx) error {
		goods := locateGoods(tx.View(owner), id)
		if goods == nil {
			return previewReject(4070)
		}
		if !locked && hasFriendOfferLock(goods) {
			return previewReject(4509)
		}
		if inventoryFlag(goods, "IsLock") != locked {
			locateGoods(tx.Get(owner), id)["IsLock"] = locked
		}
		return nil
	})
	if err == nil {
		code := 4551
		if locked {
			code = 4550
		}
		statusPB(out, true, code)
	}
	return nil, err
}
