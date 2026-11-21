package main

import (
	"encoding/json"
	"errors"
	"fmt"
	"math"
	"sort"
	"strconv"
	"strings"
)

const concordCommand = 4901072
const concordCopyCost = 120

type concordRequest struct {
	Action    string `json:"action"`
	Main      string `json:"main"`
	Second    string `json:"second"`
	Quote     string `json:"quote"`
	RequestID string `json:"requestId"`
	Revision  int64  `json:"revision"`
}

type concordBook struct {
	ID        string `json:"id"`
	Name      string `json:"name"`
	Kind      string `json:"kind"`
	Element   string `json:"element"`
	Grade     int64  `json:"grade"`
	Count     int64  `json:"count"`
	Free      int64  `json:"free"`
	Note      string `json:"note"`
	elementID int64
}

var concordBooks = map[int64]string{1: "功法", 2: "丹方", 3: "阵图", 4: "器图", 5: "符书", 6: "术法", 17: "攻击器图", 18: "防御器图"}
var concordElements = [...]string{"无", "金", "木", "水", "火", "土"}

// Some older saves use the numeric defensive-manual type for crafting materials.
func voidMaterialTemplate(g Record) bool {
	id := roleString(g, "TemplateId")
	// Affinity, shop and battle tables use these namespaces for materials, finished goods and consumables.
	for _, prefix := range [...]string{"mat.", "obj.", "wp.", "eq_", "pill_", "yun_tie_", "wu_dao_shi_", "de_jing_", "dao_jing_", "shen_shu_", "xian_jin_", "jing_mai_dan_", "shou_yuan_dan_", "sys_bu_tian_dan_", "sys_qing_du_dan_", "sys_zhu_ji_dan_"} {
		if strings.HasPrefix(id, prefix) {
			return true
		}
	}
	switch id {
	case "made.frost_iron", "made.frost_sword", "wu_xing_ju_ling_jian", "wu_xing_yun_ling_jian", "wu_xing_zhan_ling_jian", "hun_li_dan", "jing_li_dan", "ti_li_dan", "wu_xing_dan", "preview-seed-1", "preview-soil-1", "preview-sign", "preview-matrix":
		return true
	}
	if strings.HasPrefix(id, "drop_") {
		fields := strings.SplitN(id, "_", 4)
		if len(fields) == 4 {
			if kind, err := strconv.ParseInt(fields[1], 10, 64); err == nil && kind > 0 {
				_, book := concordBooks[kind]
				return !book
			}
		}
	}
	return false
}

func (s *Server) concordBook(r, g Record) (concordBook, bool) {
	kind := roleInt(g, "Type")
	family, ok := concordBooks[kind]
	if !ok || voidMaterialTemplate(g) || roleString(g, "GoodsId") == "" {
		return concordBook{}, false
	}
	grade := max(1, min(9, (roleInt(g, "Rank")+3)/4))
	elem := max(0, min(5, roleInt(g, "FiveElement")))
	b := concordBook{ID: roleString(g, "GoodsId"), Name: roleString(g, "Name"), Kind: family, Element: concordElements[elem], Grade: grade, Count: transferStackCount(g), elementID: elem}
	b.Free = b.Count
	equipped := false
	if kind == 1 {
		if method := s.methods().book(g); method != nil {
			equipped = roleString(r, gameEquipField(method.kind)) == method.id
		}
	}
	switch {
	case !forgeUsable(g):
		b.Free, b.Note = 0, "已保护"
	case equipped:
		b.Free, b.Note = max(0, b.Count-1), "在修功法留末本"
	case grade >= 4 || roleString(g, "TemplateId") == practiceStarterMethodID:
		b.Free, b.Note = max(0, b.Count-1), "保留末本"
	}
	return b, true
}

// If a beginner manual is currently equipped, its alternate is another real first-tier manual.
func (s *Server) concordPrimers(r Record) [2]*gameMethod {
	primers := [2]*gameMethod{}
	for i, pair := range [][2]string{{"gf_1_1", "gf_1_2"}, {"gf_2_3", "gf_2_4"}} {
		id := pair[0]
		if roleString(r, gameEquipField(i+1)) == id {
			id = pair[1]
		}
		primers[i] = s.methods().byID[id]
	}
	return primers
}

func concordPrimerText(primers [2]*gameMethod) string {
	if primers[0] == nil || primers[1] == nil {
		return ""
	}
	return fmt.Sprintf("《%s》《%s》各一册", primers[0].name, primers[1].name)
}

func (s *Server) concordList(r Record) Record {
	books := []concordBook{}
	for _, it := range goodsList(r) {
		g, _ := it.(Record)
		if b, ok := s.concordBook(r, g); ok {
			books = append(books, b)
		}
	}
	sort.SliceStable(books, func(i, j int) bool {
		if (books[i].Free > 0) != (books[j].Free > 0) {
			return books[i].Free > 0
		}
		if books[i].Grade != books[j].Grade {
			return books[i].Grade < books[j].Grade
		}
		if books[i].Name != books[j].Name {
			return books[i].Name < books[j].Name
		}
		return books[i].ID < books[j].ID
	})
	primerText := concordPrimerText(s.concordPrimers(r))
	return Record{"primerText": primerText, "books": books, "stones": roleInt(r, "ShinyStone"), "revision": roleInt(r, "ConcordRevision"), "copyCost": concordCopyCost}
}

func concordResonance(a, b int64) (string, int) {
	switch {
	case a == 0 || b == 0:
		return "旁证", 0
	case a == b:
		return "同源", 2
	case int64(voidCraftV2.Generates[a-1]) == b:
		return "顺生", 3
	case int64(voidCraftV2.Generates[b-1]) == a:
		return "逆推", 1
	default:
		return "相克辨义", 0
	}
}

func (s *Server) concordQuote(r Record, q concordRequest) (Record, error) {
	a, ok := s.concordBook(r, locateGoods(r, q.Main))
	b, ok2 := s.concordBook(r, locateGoods(r, q.Second))
	if !ok || !ok2 {
		return nil, forgeFailure("从纳戒选一册主卷、一册参卷")
	}
	if a.Free < 1 || b.Free < 1 || a.ID == b.ID && a.Free < 2 {
		return nil, forgeFailure("可投入的册数不足，请保留受保护的书卷")
	}
	relation, bonus := concordResonance(a.elementID, b.elementID)
	points := a.Grade + b.Grade + int64(bonus)
	pills := 1 + points/5
	cost := 4 + 2*(a.Grade+b.Grade)
	sig := voidDigest(q.Main, q.Second, a.Name, b.Name, strconv.FormatInt(roleInt(r, "ConcordRevision"), 10), fmt.Sprint(a.Grade, b.Grade, a.elementID, b.elementID, points))
	return Record{"quote": sig, "main": a, "second": b, "relation": relation, "bonus": bonus, "points": points, "pills": pills, "ink": cost, "affordable": roleInt(r, "ShinyStone") >= cost}, nil
}

func (s *Server) concordSettle(r Record, q concordRequest) (Record, error) {
	if q.Action == "copy" {
		if roleInt(r, "ShinyStone") < concordCopyCost {
			return nil, forgeFailure("抄两册基础典籍需要120灵石")
		}
		primers := s.concordPrimers(r)
		label := concordPrimerText(primers)
		if label == "" {
			return nil, forgeFailure("本服基础抄本暂缺，灵石已保留")
		}
		if q.Quote != label {
			return nil, forgeFailure("书目已变，请刷新后核对抄本")
		}
		for _, m := range primers {
			template := addGoods(Record{}, m.name, 1, m.rank, m.learningLevel, m.element, m.id, 1, 1, 10*m.rank)
			if _, err := receiveInventoryGoods(r, template, 1); err != nil {
				return nil, forgeFailure("纳戒空间不足，请腾出两格")
			}
		}
		r["ShinyStone"] = roleInt(r, "ShinyStone") - concordCopyCost
		return Record{"text": label + "已收入纳戒。", "ink": concordCopyCost}, nil
	}
	quote, err := s.concordQuote(r, q)
	if err != nil {
		return nil, err
	}
	if q.Quote == "" || q.Quote != roleString(quote, "quote") {
		return nil, forgeFailure("书卷或试读结果有变，请重新试读")
	}
	if !roleBool(quote, "affordable") {
		return nil, forgeFailure("墨钱不足，书卷已保留")
	}
	removeGoods(r, q.Main)
	removeGoods(r, q.Second)
	pills := roleInt(quote, "pills")
	template := addGoods(Record{}, "禅悟丹", 19, 16, 1, 1, "wu_xing_dan", 1, .3, 20)
	if _, err := receiveInventoryGoods(r, template, pills); err != nil {
		return nil, forgeFailure("纳戒装不下所得禅悟丹，请腾出一格")
	}
	r["ShinyStone"] = roleInt(r, "ShinyStone") - roleInt(quote, "ink")
	return Record{"text": fmt.Sprintf("%s成篇，%d枚禅悟丹已收入纳戒。", roleString(quote, "relation"), pills), "pills": pills, "ink": quote["ink"], "points": quote["points"], "relation": quote["relation"]}, nil
}

func (s *Server) concordAction(owner string, q concordRequest) (out Record, err error) {
	err = s.roles.Transact(func(tx *StoreTx) error {
		r := tx.View(owner)
		if owner == "" || reservedStoreID(owner) || r == nil {
			return forgeFailure("请先登录角色")
		}
		if roleInt(r, "Level") < 21 {
			return forgeFailure("达到结丹一阶后可入合道台")
		}
		switch q.Action {
		case "list":
			out = s.concordList(r)
			return nil
		case "preview":
			var e error
			out, e = s.concordQuote(r, q)
			return e
		case "commit", "copy":
		default:
			return forgeFailure("未知的合道台操作")
		}
		if !validOfferToken(q.RequestID) {
			return forgeFailure("请求编号无效，请重新确认")
		}
		fingerprint := voidDigest(q.Action, q.Main, q.Second, q.Quote, fmt.Sprint(q.Revision))
		last := farmMap(r, "ConcordLast")
		if q.RequestID == roleString(last, "id") {
			if fingerprint != roleString(last, "fingerprint") {
				return forgeFailure("该请求编号已经使用")
			}
			out = s.concordList(r)
			out["receipt"] = last["receipt"]
			return nil
		}
		if q.Revision != roleInt(r, "ConcordRevision") || q.Revision < 0 || q.Revision == math.MaxInt64 {
			return forgeFailure("书案已有新的记录，请刷新后重试")
		}
		edit := tx.Get(owner)
		result, e := s.concordSettle(edit, q)
		if e != nil {
			return e
		}
		edit["ConcordRevision"] = q.Revision + 1
		edit["ConcordLast"] = Record{"id": q.RequestID, "fingerprint": fingerprint, "receipt": result}
		out = s.concordList(edit)
		out["receipt"] = result
		return nil
	})
	return
}

func (s *Server) handleConcordDesk(peer *Peer, msg Message) {
	id := s.worldChatRole(peer)
	var q concordRequest
	if id == "" {
		sendAuctionJSON(peer, msg, 4007, Record{"error": "请先登录角色"})
		return
	}
	if len(msg.Data) > 2048 || json.Unmarshal([]byte(offerToken(msg.Data)), &q) != nil || strings.TrimSpace(q.Action) == "" {
		sendAuctionJSON(peer, msg, 4008, Record{"error": "合参请求无效"})
		return
	}
	if s.preview != nil && !s.previewWrites && (q.Action == "commit" || q.Action == "copy") {
		sendAuctionJSON(peer, msg, 4008, Record{"error": "本服处于只读预览"})
		return
	}
	out, err := s.concordAction(id, q)
	if err != nil {
		var rejection forgeFailure
		if errors.As(err, &rejection) {
			sendAuctionJSON(peer, msg, 4008, Record{"error": string(rejection)})
			return
		}
		sendAuctionJSON(peer, msg, 5000, Record{"error": "书案回执暂未取到，可原单重试", "retry": true})
		return
	}
	sendAuctionJSON(peer, msg, 2000, out)
}
