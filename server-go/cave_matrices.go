package main

import (
	"encoding/json"
	"errors"
	"fmt"
)

const caveMatrixCommand = 4901073

var matrixStonePrices = [2][5]int64{{0, 240, 900, 2700, 6500}, {0, 360, 1350, 3900, 9000}}

type caveMatrixRequest struct {
	Kind   string
	Target int
	Price  int64
}

func matrixControlPercent(r Record) int64 {
	roots := 0
	for _, key := range cultivationRootFields[1:] {
		if roleBool(r, key) {
			roots++
		}
	}
	if roots == 1 {
		return 10
	}
	return 5
}
func matrixBenefits(role Record) float64 {
	gather := max(0, min(4, roleInt(role, "JuLingRank")))
	control := max(0, min(4, roleInt(role, "KongLingRank")))
	return (1 + 0.1*float64(gather)) * (1 + float64(matrixControlPercent(role)*control)/100)
}

func (s *Server) caveMatrixOrder(owner string, req caveMatrixRequest) (Record, error) {
	if req.Kind != "gather" && req.Kind != "control" {
		return nil, forgeFailure("阵法不存在")
	}
	index, field, name := 0, "JuLingRank", "聚灵阵"
	if req.Kind == "control" {
		index, field, name = 1, "KongLingRank", "控灵阵"
	}
	var reply Record
	err := s.roles.Transact(func(tx *StoreTx) error {
		r := tx.View(owner)
		if r == nil {
			return forgeFailure("请先登录角色")
		}
		rank := int(max(0, min(4, roleInt(r, field))))
		step := int64(10)
		if index == 1 {
			step = matrixControlPercent(r)
		}
		cave := int(gameHomeRank(r))
		if req.Target != 0 {
			if req.Target != rank+1 || req.Target > 4 || req.Target > cave || req.Price != matrixStonePrices[index][req.Target] {
				return forgeFailure("阵阶、洞府或报价已改变，请重新打开阵图")
			}
			coins := roleInt(r, "ShinyStone")
			if coins < req.Price {
				return forgeFailure("灵石不足，无法布阵")
			}
			r = tx.Get(owner)
			r[field] = req.Target
			r["ShinyStone"] = coins - req.Price
			rank = req.Target
		}
		target := min(4, rank+1)
		price := matrixStonePrices[index][target]
		canBuy := rank < 4 && target <= cave && roleInt(r, "ShinyStone") >= price
		text := fmt.Sprintf("%s %d 阶 · 洞府 %d 阶\n纳气、炼神、炼体均提高 %d%%。布阵后持续生效。", name, rank, cave, int64(rank)*step)
		if rank < 4 {
			text += fmt.Sprintf("\n升至 %d 阶需 %d 灵石，三路加成提高到 %d%%。", target, price, int64(target)*step)
			if target > cave {
				text += fmt.Sprintf("\n须先将洞府升至 %d 阶。", target)
			} else if !canBuy {
				text += "\n当前灵石不足。"
			}
		} else {
			text += "\n已达天阶。"
		}
		reply = Record{"kind": req.Kind, "name": name, "rank": rank, "target": target, "price": price, "stones": roleInt(r, "ShinyStone"), "canBuy": canBuy, "description": text}
		return nil
	})
	return reply, err
}

func (s *Server) handleCaveMatrix(peer *Peer, msg Message) {
	code := 2000
	var body Record
	var req caveMatrixRequest
	role := peer.currentRoleID()
	current, online := s.online.Load(role)
	switch {
	case role == "" || !online || current != peer || (s.preview != nil && !s.previewWrites):
		code, body = 4008, Record{"error": "请在开放阵法的世界登录后重试"}
	case len(msg.Data) > 256 || json.Unmarshal([]byte(offerToken(msg.Data)), &req) != nil:
		code, body = 4008, Record{"error": "阵图请求无效"}
	default:
		var err error
		body, err = s.caveMatrixOrder(role, req)
		if err != nil {
			var reason forgeFailure
			if errors.As(err, &reason) {
				code, body = 4008, Record{"error": string(reason)}
			} else {
				code, body = 5000, Record{"error": "布阵未能保存，请重试"}
			}
		}
	}
	data, _ := json.Marshal(body)
	wire := offerNumber(nil, 1, uint64(code))
	if code == 2000 {
		wire = offerNumber(wire, 2, 1)
	}
	_ = peer.sendPayload(msg.Cmd, msg.Msid, offerString(wire, 3, string(data)))
}
