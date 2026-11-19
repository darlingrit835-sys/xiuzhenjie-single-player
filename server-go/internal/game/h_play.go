package game

import (
	"math/bits"

	"google.golang.org/protobuf/proto"

	"xzjserver/internal/gateway"
	"xzjserver/internal/pb"
	"xzjserver/internal/store"
)

// registerPlay 玩法模块注册入口。
// 每迁移完一个模块就把指令挂到这里；没挂的走通用兜底，客户端不会卡。
func registerPlay() {
	gateway.Register(1001007, onChangeRunLocation)
	gateway.Register(1001008, onBreakAsk)
	gateway.Register(1001010, onBreakRealm)
	gateway.Register(1007001, onSibingsMap)
	gateway.Register(1007003, onMapChildren)
}

// ------------------------------------------------ 1001007 切换修炼方式

// 客户端传 1=炼神 2=锻体 3=纳气，服务端做异或切换；
// 元婴（31 级）之前只能同时运行一门。回包沿用 ActionBoolResp_1001001：
// 4058 成功，4059 失败。
func onChangeRunLocation(s *gateway.Session, msg *pb.Msg) error {
	req := &pb.ChangeRunLocationReq_1001007{}
	_ = proto.Unmarshal(msg.Data, req)

	resp := &pb.ActionBoolResp_1001001{}
	r := s.Role
	if r == nil {
		resp.Code = 3001
		resp.IsSuccess = false
		s.Send(1001007, msg.Msid, resp)
		return nil
	}
	EnsureDefaults(r)

	loc := int(req.NewLocation)
	if loc < 1 || loc > 3 {
		resp.Code = 4059
		resp.IsSuccess = false
	} else {
		bit := 1 << (loc - 1)
		mask := activeMask(r) ^ bit
		count := bits.OnesCount(uint(mask & 7))
		level := r.Num("Level")
		switch {
		case count == 0:
			resp.Code = 4059
			resp.IsSuccess = false
		case count > 1 && level < 31:
			// 元婴之前只能在一处修行
			resp.Code = 4059
			resp.IsSuccess = false
		default:
			r.Set("RunMask", mask)
			r.Set("RunLocation", locationType(mask))
			_ = store.Save(r)
			resp.Code = 4058
			resp.IsSuccess = true
		}
	}
	s.Send(1001007, msg.Msid, resp)
	return nil
}

// ------------------------------------------------ 1001008 突破询问

// HasThunder=0：本服不做雷劫小游戏，客户端走普通突破流程。
// 4062 不能突破，4063 可以突破。
func onBreakAsk(s *gateway.Session, msg *pb.Msg) error {
	resp := &pb.RoleBreakAskResp_1001008{}
	r := s.Role
	if r == nil {
		resp.Code = 3001
		resp.IsSuccess = false
		s.Send(1001008, msg.Msid, resp)
		return nil
	}
	EnsureDefaults(r)

	if r.Num("Level") >= MaxLevel() || !canBreak(r) {
		resp.Code = 4062
		resp.IsSuccess = false
	} else {
		resp.Code = 4063
		resp.IsSuccess = true
		resp.HasThunder = 0
	}
	s.Send(1001008, msg.Msid, resp)
	return nil
}

// ------------------------------------------------ 1001010 突破境界

// 成功升一级并按等级表重算神识/体魄/精血/寿元/战力；4306 成功，4062 失败。
func onBreakRealm(s *gateway.Session, msg *pb.Msg) error {
	req := &pb.RoleBreakRealmReq_1001010{}
	_ = proto.Unmarshal(msg.Data, req)

	resp := &pb.RoleBreakRealmResp_1001010{}
	r := s.Role
	if r == nil {
		resp.Code = 3001
		resp.IsSuccess = false
		s.Send(1001010, msg.Msid, resp)
		return nil
	}
	EnsureDefaults(r)

	if !canBreak(r) {
		resp.Code = 4062
		resp.IsSuccess = false
	} else {
		LevelUp(r)
		resp.Code = 4306
		resp.IsSuccess = true
	}
	resp.PureBlood = int32(r.Num("PureBlood"))
	resp.FlexibleRoot = r.Str("FlexibleRoot")
	resp.SuperAir = r.Lng("SuperAir")
	resp.Perception = r.Lng("Perception")
	resp.Physique = r.Lng("Physique")
	resp.CurrentSuperAir = r.Lng("CurrentSuperAir")
	resp.CurrentPerception = r.Lng("CurrentPerception")
	resp.CurrentPhysique = r.Lng("CurrentPhysique")
	resp.CurrentVigor = int32(r.Num("CurrentVigor"))
	resp.CurrentClever = int32(r.Num("CurrentClever"))
	resp.CurrentPoisonVal = int32(r.Num("CurrentPoisonVal"))
	resp.CurrentPureBlood = int32(r.Num("CurrentPureBlood"))
	resp.Age = int32(r.Num("MaxAge"))
	resp.CurrentPower = int32(r.Num("CurrentPower"))
	s.Send(1001010, msg.Msid, resp)

	if resp.IsSuccess {
		notifyText(s, "突破成功，当前境界："+levelName(r.Num("Level")))
	}
	return nil
}
