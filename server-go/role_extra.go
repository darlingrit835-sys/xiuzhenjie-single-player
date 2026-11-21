package main

import (
	"slices"
	"strconv"

	"google.golang.org/protobuf/reflect/protoreflect"
)

func roleExtraRoutes() []previewRoute {
	return []previewRoute{
		{cmd: 1002007, module: "角色与纳戒", request: "TitleUseReq_1002007", response: "TitleUseResp_1002007", summary: "佩戴角色已获得的称号", blocked: true, live: (*Server).useOwnedTitle},
		goodsDetailRoute(1004005, "GoodsMaterialAnimalDetail", "动物材料", 7, 11),
		goodsDetailRoute(1004006, "GoodsMaterialMineralDetail", "矿物材料", 13, 13),
		goodsDetailRoute(1004007, "GoodsMaterialPlantDetail", "植物材料", 12, 12),
		goodsDetailRoute(1004009, "GoodsBookElixirDetail", "丹方", 2, 2),
	}
}

func (s *Server) useOwnedTitle(peer *Peer, req, out protoreflect.Message) (func(), error) {
	roleID := peer.currentRoleID()
	title := getInt(req, "Title")
	if title <= 0 {
		return nil, previewReject(4285)
	}
	err := s.roles.Transact(func(tx *StoreTx) error {
		role := tx.View(roleID)
		if role == nil || reservedStoreID(roleID) {
			return previewReject(4007)
		}
		if !slices.Contains(friendTitleIDs(role), int(title)) {
			return previewReject(4285)
		}
		previous := roleInt(role, "Title")
		if previous == title {
			return nil
		}
		tx.Get(roleID)["Title"] = title
		event := newHistoryEvent("role.title_selected", role, nil, "title", strconv.FormatInt(title, 10), Record{"previousTitle": previous})
		event.Actor.RoleID = roleID
		return tx.RecordHistory(event)
	})
	if err == nil {
		statusPB(out, true, 4286)
	}
	return nil, err
}

func goodsDetailRoute(cmd int, message, label string, firstType, lastType int64) previewRoute {
	suffix := "_" + strconv.Itoa(cmd)
	return previewRoute{
		cmd: cmd, module: "角色与纳戒", request: message + "Req" + suffix, response: message + "Resp" + suffix,
		summary: "读取本人纳戒、储物箱或有效挂牌的" + label + "详情", roleFields: []string{"Goods"},
		build: func(role Record, req, out protoreflect.Message) {
			id := getString(req, "Id")
			if id == "" {
				statusPB(out, false, 4070)
				return
			}
			fillOwnedGoodsDetail(out, locateGoods(role, id), firstType, lastType)
		},
		live: func(s *Server, peer *Peer, req, out protoreflect.Message) (func(), error) {
			goods := s.inventoryDetailItem(peer.currentRoleID(), getString(req, "Id"), false)
			fillOwnedGoodsDetail(out, goods, firstType, lastType)
			return nil, nil
		},
	}
}

func fillOwnedGoodsDetail(out protoreflect.Message, goods Record, firstType, lastType int64) {
	kind := roleInt(goods, "Type")
	if goods == nil {
		statusPB(out, false, 4070)
		return
	}
	if kind < firstType || kind > lastType {
		statusPB(out, false, 4069)
		return
	}
	copyGoodsDetail(out, goods)
	statusPB(out, true, 4071)
}

// Detail PB messages and inventory records use the same stat names. Do not
// derive crafting stats from rank or send any other inventory information.
func copyGoodsDetail(out protoreflect.Message, goods Record) {
	fields := out.Descriptor().Fields()
	for index := 0; index < fields.Len(); index++ {
		f := fields.Get(index)
		name := string(f.Name())
		if name == "Code" || name == "IsSuccess" || goods[name] == nil {
			continue
		}
		if !f.IsList() {
			copyRolePB(out, goods, name)
			continue
		}
		// MaterialNames is the only repeated field in these detail messages.
		names := out.Mutable(f).List()
		switch values := goods[name].(type) {
		case []string:
			for _, value := range values {
				names.Append(protoreflect.ValueOfString(value))
			}
		case []any:
			for _, value := range values {
				if text, ok := value.(string); ok {
					names.Append(protoreflect.ValueOfString(text))
				}
			}
		}
	}
}
