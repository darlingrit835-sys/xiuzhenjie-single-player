package main

import (
	"fmt"
	"strings"

	"google.golang.org/protobuf/reflect/protoreflect"
)

type gameMap struct {
	id, parent, name string
	kind, depth      int
	minimum, maximum int
}

var geography = newGeography()

func newGeography() struct {
	nodes    map[string]*gameMap
	children map[string][]*gameMap
} {
	nodes := make(map[string]*gameMap)
	children := make(map[string][]*gameMap)
	link := func(id, parent, name string, kind, depth, minimum, maximum int) {
		node := &gameMap{id, parent, name, kind, depth, minimum, maximum}
		nodes[id] = node
		children[parent] = append(children[parent], node)
	}
	link("1", "0", "修真界", 0, 0, 0, 999)
	link("11", "1", "东荒", 1, 1, 0, 999)
	link("12", "1", "南岭", 1, 1, 0, 999)
	link("13", "1", "西漠", 1, 1, 0, 999)
	link("14", "1", "北冥", 1, 1, 0, 999)
	link("111", "11", "青云洲", 2, 2, 0, 40)
	link("112", "11", "落日洲", 2, 2, 0, 40)
	link("121", "12", "紫霞洲", 2, 2, 0, 40)
	link("131", "13", "黄沙洲", 2, 2, 0, 40)
	link("141", "14", "玄冰洲", 2, 2, 0, 40)

	county := func(province, id, name, towns string) {
		link(id, province, name, 3, 3, 1, 40)
		for index, town := range strings.Fields(towns) {
			cityID := fmt.Sprintf("%s%02d", id, index+1)
			link(cityID, id, town, 51, 4, index*10+1, index*10+10)
			for offset, facility := range strings.Fields("城主府 玄天楼 虚神界 中阶坊市 传送大阵 灵泉酒肆") {
				facilityID := fmt.Sprintf("%s%02d", cityID, offset+1)
				link(facilityID, cityID, town+"·"+facility, 4, 5, index*10+1, index*10+10)
			}
		}
	}
	county("111", "1111", "临仙郡", "新手村 临仙城 翠微城 听雨城 落霞城 望月城 飞星城 烟波城 问剑城 青冥城 紫电城 浮光城")
	county("111", "1112", "朝露郡", "朝露城 晨光城 清溪城 白鹭城 松风城 竹影城 梅香城 兰溪城 菊隐城 荷风城 桂月城 雪梅城")
	county("112", "1121", "云梦郡", "云梦城 楚云城 碧涛城 沧浪城 凌波城 归雁城 栖霞城 映月城 回风城 断虹城 流云城 惊鸿城")
	county("121", "1211", "紫霞郡", "紫霞城 丹霞城 赤霄城 朱明城 绛云城 彤光城 瑞彩城 宝华城 流金城 鎏光城 琉璃城 琥珀城")
	county("131", "1311", "沙海郡", "沙海城 绿洲城 鸣沙城 断魂城 孤烟城 落日城 瀚海城 苍狼城 古楼城 驼铃城 金沙城 月牙城")
	county("141", "1411", "寒渊郡", "寒渊城 冰魄城 雪岭城 霜华城 凝碧城 玉衡城 太清城 广寒城 玄冥城 寒星城 朔风城 琼华城")
	return struct {
		nodes    map[string]*gameMap
		children map[string][]*gameMap
	}{nodes, children}
}

func gameMapFind(id string) *gameMap {
	if node := geography.nodes[id]; node != nil {
		return node
	}
	countyID, rest, ok := strings.Cut(id, "W")
	county := geography.nodes[countyID]
	if !ok || county == nil || len(rest) == 0 || len(rest) > 2 || rest[0] < '1' || rest[0] > '6' {
		return nil
	}
	forest := gameForest(county, int(rest[0]-'0'))
	if len(rest) == 1 {
		return forest
	}
	if rest[1] < '1' || rest[1] > '3' {
		return nil
	}
	return gameForestArea(forest, int(rest[1]-'0'))
}

func gameMapName(id string) string {
	if node := gameMapFind(id); node != nil {
		return node.name
	}
	return ""
}

func gameCityName(id string) string {
	if node := geography.nodes[id]; node != nil && node.kind == 51 {
		return node.name
	}
	return ""
}

func gameForest(county *gameMap, index int) *gameMap {
	suffix := fmt.Sprint(index)
	return &gameMap{county.id + "W" + suffix, county.id, county.name + "·妖兽林地" + suffix, 4, 4, 1, 999}
}

func gameForestArea(forest *gameMap, index int) *gameMap {
	return &gameMap{forest.id + fmt.Sprint(index), forest.id, forest.name + "·" + [...]string{"外围", "深处", "兽王巢"}[index-1], 4, 5, 1, 999}
}

func appendPBString(m protoreflect.Message, field, value string) {
	desc := m.Descriptor().Fields().ByName(protoreflect.Name(field))
	m.Mutable(desc).List().Append(protoreflect.ValueOfString(value))
}

func fillMapMessage(m protoreflect.Message, n *gameMap) {
	putPB(m, Record{
		"Id": n.id, "ParentId": n.parent, "ParentName": gameMapName(n.parent),
		"Name": n.name, "Type": n.kind, "MinLevel": n.minimum, "MaxLevel": n.maximum,
		"Status": 1, "BasedConcentration": 1.0, "Sort": 1, "Level": n.depth,
	})
}

type gameMapProtocol struct{ req, resp string }

var gameMapProtocols = map[int]gameMapProtocol{
	1007001: {"SibingsMapReq_1007001", "SibingsMapResp_1007001"},
	1007003: {"ChildrenMapReq_1007003", "ChildrenMapResp_1007003"},
	1007005: {"ParentMapIdReq_1007005", "ParentMapIdResp_1007005"},
	1007006: {"ParentMapReq_1007006", "ParentMapResp_1007006"},
	1007008: {"", "AncestorMapResp_1007008"},
	1007009: {"MapInfoReq_1007009", "MapInfoResp_1007009"},
	1007010: {"MapTransmitReq_1007010", "MapTransmitResp_1007010"},
	1007011: {"MapCityMigrateReq_1007011", "MapCityMigrateResp_1007011"},
	1007012: {"ParentMapAndOneselfMapReq_1007012", "ParentMapAndOneselfMapResp_1007012"},
	1007013: {"LiLianChildrenMapReq_1007013", "LiLianChildrenMapResp_1007013"},
}

func (s *Server) handleMap(peer *Peer, msg Message) bool {
	p, known := gameMapProtocols[msg.Cmd]
	if !known {
		return false
	}
	b := s.gameResponse(p.resp, true, 2000)
	if msg.Cmd == 1007008 {
		putPB(b, Record{"Id": "1", "Name": gameMapName("1")})
		s.sendGame(peer, msg, b)
		return true
	}
	req, err := s.decode(p.req, msg.Data)
	if err != nil {
		return true
	}
	id := getString(req, "MapId")
	current := gameMapFind(id)
	switch msg.Cmd {
	case 1007001:
		if current == nil {
			break
		}
		if current.kind == 51 {
			fillMapMessage(addMessage(b, "Object"), current)
		}
		n := 0
		for _, sibling := range geography.children[current.parent] {
			if sibling.id == id {
				continue
			}
			fillMapMessage(addMessage(b, "Object"), sibling)
			n++
			if current.kind == 51 && n == 5 {
				break
			}
		}
		if current.kind == 51 {
			county := gameMapFind(current.parent)
			for i := 1; i <= 6; i++ {
				fillMapMessage(addMessage(b, "Object"), gameForest(county, i))
			}
		}
	case 1007003:
		for _, child := range geography.children[id] {
			fillMapMessage(addMessage(b, "Object"), child)
		}
	case 1007005:
		set(b, "Code", 4177)
		for node := current; node != nil; node = gameMapFind(node.parent) {
			appendPBString(b, "Id", node.id)
		}
	case 1007006:
		set(b, "Code", 4178)
		if current != nil {
			for node := gameMapFind(current.parent); node != nil; node = gameMapFind(node.parent) {
				fillMapMessage(addMessage(b, "Object"), node)
			}
		}
	case 1007009:
		if current == nil {
			statusPB(b, false, 4259)
		} else {
			set(b, "Code", 4180)
			fillMapMessage(b, current)
		}
	case 1007010, 1007011:
		var role Record
		if current != nil {
			err = s.roles.Update(peer.currentRoleID(), func(r Record) error {
				r["MapId"], r["MapName"] = current.id, current.name
				role = r
				return nil
			})
		} else {
			role = s.roles.Get(peer.currentRoleID())
		}
		switch {
		case err != nil:
			statusPB(b, false, 4007)
		case msg.Cmd == 1007011:
			set(b, "CurrentAge", roleInt(role, "CurrentAge"))
			set(b, "CurrentPower", roleInt(role, "CurrentPower"))
		case current == nil:
			statusPB(b, false, 4259)
		default:
			set(b, "Code", 4185)
			set(b, "ShinyStone", roleInt(role, "ShinyStone"))
		}
	case 1007012:
		set(b, "Code", 4312)
		for node := current; node != nil; node = gameMapFind(node.parent) {
			appendPBString(b, "Name", node.name)
		}
	case 1007013:
		set(b, "Code", 4312)
		if current != nil {
			for i := 1; i <= 3; i++ {
				child := gameForestArea(current, i)
				if current.kind == 51 {
					child = gameForest(gameMapFind(current.parent), i)
				}
				fillMapMessage(addMessage(b, "Object"), child)
			}
		}
	}
	s.sendGame(peer, msg, b)
	return true
}
