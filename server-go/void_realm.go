package main

import (
	"crypto/sha256"
	"encoding/hex"
	"encoding/json"
	"errors"
	"log"
	"slices"
	"sort"
	"strings"
	"time"
	"unicode"
	"unicode/utf8"
)

const voidCommand = 4901070
const voidStorePrefix = "$void:"
const voidPageSize = 20

type voidRequest struct {
	Action    string `json:"action"`
	HallID    string `json:"hallId"`
	Grade     int    `json:"grade"`
	Brief     int    `json:"brief"`
	Route     int    `json:"route"`
	Adjust    int    `json:"adjust"`
	Name      string `json:"name"`
	Text      string `json:"text"`
	Tag       string `json:"tag"`
	WorkID    string `json:"workId"`
	Mode      string `json:"mode"`
	Page      int    `json:"page"`
	RequestID string `json:"requestId"`
	// Omit v2-only fields from v1 receipts. Old retries must retain the original signature.
	Version    int             `json:"version,omitempty"`
	Steps      []int           `json:"steps,omitempty"`
	QuoteID    string          `json:"quoteId,omitempty"`
	AltarItems []voidAltarPick `json:"altarItems,omitempty"`
}

func (s *Server) handleVoidRealm(peer *Peer, msg Message) {
	roleID := s.worldChatRole(peer)
	var q voidRequest
	switch {
	case roleID == "" || reservedStoreID(roleID):
		sendAuctionJSON(peer, msg, 4007, Record{"error": "请先登录角色"})
		return
	case len(msg.Data) > 4096 || json.Unmarshal([]byte(offerToken(msg.Data)), &q) != nil:
		sendAuctionJSON(peer, msg, 4008, Record{"error": "虚神界请求格式无效"})
		return
	case s.preview != nil && !s.previewWrites && (q.Action == "post" || q.Action == "publish" || q.Action == "study" || q.Action == "altarOffer"):
		sendAuctionJSON(peer, msg, 4008, Record{"error": "本服当前为只读预览"})
		return
	}
	out, err := s.voidAction(roleID, q, time.Now())
	if err != nil {
		var denied forgeFailure
		if errors.As(err, &denied) {
			sendAuctionJSON(peer, msg, 4008, Record{"error": string(denied)})
		} else {
			log.Printf("虚神界存档: %v", err)
			sendAuctionJSON(peer, msg, 5000, Record{"error": "虚神界未能保存，请重试", "retry": true})
		}
		return
	}
	sendAuctionJSON(peer, msg, 2000, out)
}

func voidDigest(parts ...string) string {
	sum := sha256.Sum256([]byte(strings.Join(parts, "\x00")))
	return hex.EncodeToString(sum[:16])
}

// Every game instance reads and writes exclusively inside its own role store.
// Neither the shard nor the author/account identity can be selected in a request.
func (s *Server) voidAction(owner string, q voidRequest, now time.Time) (out Record, err error) {
	switch q.Action {
	case "altar", "altarPreview", "altarOffer":
		return s.voidAltarAction(owner, q, now)
	}
	err = s.roles.Transact(func(tx *StoreTx) error {
		role := tx.View(owner)
		if role == nil || owner == "" || reservedStoreID(owner) {
			return forgeFailure("请先登录角色")
		}
		if roleInt(role, "Level") < 21 {
			return forgeFailure("达到结丹一阶后可进入虚神界")
		}
		user := roleString(role, "UserId")
		if user == "" {
			user = "role:" + owner
		} else {
			user = "account:" + user
		}
		viewer := voidDigest(user)
		var e error
		switch q.Action {
		case "lobby":
			out = voidLobby(owner, role)
			out["wallet"] = voidWallet(role)
		case "enter":
			h := voidFindHall(q.HallID)
			if e = voidAccess(role, h, max(1, q.Grade)); e == nil {
				rank := voidRank(role, h.Type)
				out = Record{"id": h.ID, "rank": rank, "cap": min(rank, voidRealmStage(role))}
				out["wallet"] = voidWallet(role)
			}
		case "simulate":
			out, e = voidPreview(role, q)
		case "publish", "post":
			out, e = voidWrite(tx, owner, viewer, role, q, now)
		case "board", "rankings":
			out, e = voidList(tx, q, viewer)
		case "work", "study":
			work := voidWork(tx, q.WorkID)
			if work == nil {
				return forgeFailure("本服尚无此作品")
			}
			if q.Action == "work" {
				out = Record{"work": voidWorkView(work, viewer)}
			} else {
				out, e = voidStudy(tx, role, viewer, work, q.WorkID, owner, now)
			}
		default:
			e = forgeFailure("未知的虚神界操作")
		}
		return e
	})
	return
}

func voidWork(tx *StoreTx, id string) Record {
	if !validOfferToken(id) {
		return nil
	}
	r := tx.View(voidStorePrefix + id)
	if roleString(r, "kind") != "work" {
		return nil
	}
	return r
}

func voidWorkView(row Record, viewer string) Record {
	out := Record{}
	for _, k := range []string{"id", "name", "hallId", "author", "authorId", "grade", "brief", "route", "adjust", "score", "time", "version"} {
		out[k] = row[k]
	}
	if roleInt(row, "version") == 2 {
		for _, k := range voidCreationFields {
			out[k] = row[k]
		}
	}
	readers, _ := row["readers"].(Record)
	_, studied := readers[viewer]
	out["readerCount"], out["studied"], out["place"] = len(readers), studied, 0
	out["own"] = viewer == roleString(row, "maker")
	return out
}

func voidPostView(row Record) Record {
	out := Record{}
	for _, k := range []string{"id", "author", "authorId", "text", "tag", "workId", "time"} {
		out[k] = row[k]
	}
	return out
}

var voidNameEndings = map[string]string{
	"dan": "散药丹丸", "qi": "爪斧弓剑靴钺衣裳戟琴印镯刀枪锤棒盾旗索矛珠幡氅绫鼎针钟铃尺甲",
	"dao": "本诀谱录经功卷法书典", "shu": "闪爆破斩劲杀轰劈吟击盾御体术印诀伤咒封解散毒瘴遁",
	"zhen": "阵", "zhi": "竹芽叶兰果芝参根藤花枝木草子",
	"qiao": "机轮锁匣弩傀儡枢", "shou": "令契谱诀印",
}
var voidBoardTags = []string{"求教", "心得", "邀试"}

func voidName(q voidRequest) (string, error) {
	if q.HallID == "fu" {
		return [...]string{"惊铃诀", "渡水盾", "换枢印"}[q.Brief], nil
	}
	name := strings.TrimSpace(q.Name)
	runes := []rune(name)
	if len(runes) < 3 || len(runes) > 5 || strings.IndexFunc(name, func(r rune) bool { return !unicode.Is(unicode.Han, r) }) >= 0 {
		return "", forgeFailure("作品名须为三至五个汉字")
	}
	ends := voidNameEndings[q.HallID]
	if !strings.ContainsRune(ends, runes[len(runes)-1]) {
		return "", forgeFailure("作品末字可用：" + ends)
	}
	return name, nil
}

func voidWrite(tx *StoreTx, owner, viewer string, role Record, q voidRequest, now time.Time) (Record, error) {
	if !validOfferToken(q.RequestID) {
		return nil, forgeFailure("请使用有效的登记请求编号")
	}
	id := voidDigest(owner, q.RequestID)
	key := voidStorePrefix + id
	raw, _ := json.Marshal(q)
	hash := voidDigest(string(raw))
	if row := tx.View(key); row != nil {
		if roleString(row, "hash") != hash {
			return nil, forgeFailure("此请求编号已经使用，请重新登记")
		}
		if q.Action == "post" {
			return Record{"post": voidPostView(row)}, nil
		}
		// Return the confirmed receipt even if the player has since changed profession.
		// Access checks apply when creating or studying a manuscript.
		return Record{"work": voidWorkView(row, viewer), "wallet": voidWallet(role)}, nil
	}
	if q.Action == "post" {
		text := strings.TrimSpace(q.Text)
		if !slices.Contains(voidBoardTags, q.Tag) || text == "" || utf8.RuneCountInString(text) > 120 {
			return nil, forgeFailure("请选择留言类别，正文可写一至一百二十字")
		}
		if q.WorkID != "" && voidWork(tx, q.WorkID) == nil {
			return nil, forgeFailure("本服尚无所附作品")
		}
		row := tx.GetOrCreate(key)
		row["id"], row["kind"], row["hash"], row["time"] = id, "post", hash, now.Unix()
		row["authorId"], row["author"] = owner, roleString(role, "Name")
		row["text"], row["tag"], row["workId"] = worldChatPlainText.Replace(text), q.Tag, q.WorkID
		return Record{"post": voidPostView(row)}, nil
	}
	if q.Version != 2 {
		return nil, forgeFailure("新版创作已开放，请按本馆工序重新推演后署名")
	}

	result, err := voidPreview(role, q)
	if err != nil {
		return nil, err
	}
	if !roleBool(result, "success") {
		return nil, forgeFailure("方案尚未合题，请先调整再试")
	}
	name, err := voidName(q)
	if err != nil {
		return nil, err
	}
	// The recorded signature includes the original brief and choices, even if two plans score equally.
	plan := voidPlanV2(owner, q)
	if q.QuoteID == "" || q.QuoteID != roleString(result, "quoteId") {
		return nil, forgeFailure("请重新推演并确认本次成本")
	}
	for _, recID := range tx.store.order {
		if !strings.HasPrefix(recID, voidStorePrefix) {
			continue
		}
		other := tx.View(recID)
		if roleString(other, "kind") != "work" || roleString(other, "hallId") != q.HallID {
			continue
		}
		if roleString(other, "plan") == plan {
			return nil, forgeFailure("该题的同阶同法已经由你署名")
		}
		if q.HallID != "fu" && roleString(other, "name") == name {
			return nil, forgeFailure("本馆已有同名作品，请另题一名")
		}
	}
	row := tx.GetOrCreate(key)
	row["id"], row["hash"], row["kind"], row["version"], row["time"] = id, hash, "work", q.Version, now.Unix()
	row["authorId"], row["author"], row["maker"], row["name"], row["plan"] = owner, roleString(role, "Name"), viewer, name, plan
	row["hallId"], row["grade"], row["brief"], row["route"], row["adjust"] = q.HallID, q.Grade, q.Brief, q.Route, q.Adjust
	row["score"] = roleInt(result, "score")
	maker := tx.Get(owner)
	if err := voidPayCreation(maker, row, q, result); err != nil {
		return nil, err
	}
	if err := artisanManuscript(maker, voidFindHall(q.HallID), q.Grade, q.Brief); err != nil {
		return nil, err
	}
	return Record{"work": voidWorkView(row, viewer), "wallet": voidWallet(maker)}, nil
}

func voidStudy(tx *StoreTx, role Record, viewer string, work Record, id, owner string, now time.Time) (Record, error) {
	if owner == roleString(work, "authorId") || viewer == roleString(work, "maker") {
		return nil, forgeFailure("自己的作品可自验，回执须由其他道友完成")
	}
	version := int(roleInt(work, "version"))
	if version != voidCatalogVersion && version != 2 {
		return nil, forgeFailure("此版本作品暂不能复演")
	}
	q := voidRequest{HallID: roleString(work, "hallId"), Grade: int(roleInt(work, "grade")), Brief: int(roleInt(work, "brief")),
		Route: int(roleInt(work, "route")), Adjust: int(roleInt(work, "adjust"))}
	var result Record
	var err error
	if version == 2 {
		q.Version = 2
		q.Steps, err = voidSavedSteps(work)
		if err == nil {
			result, err = voidCreateV2(role, q)
		}
	} else {
		result, err = voidSimulate(role, q)
	}
	if err != nil {
		return nil, err
	}
	if !roleBool(result, "success") {
		return nil, forgeFailure("照谱复演未能合题")
	}
	if version == 2 {
		// The outcome signed by the author stays with this edition. Reproduction
		// validates the steps but does not issue another quote or charge the reader.
		for _, k := range []string{"affinityText", "affixText", "score", "scoreText", "craftText", "effectText", "value", "valueText", "axes"} {
			if work[k] != nil {
				result[k] = work[k]
			}
		}
		result["cost"], result["stoneCost"], result["insightCost"], result["tuition"] = 0, 0, 0, 0
		result["materials"], result["costText"] = []Record{}, "照谱复演免费，未扣灵石、悟性或材料。"
	}
	if readers, _ := work["readers"].(Record); readers[viewer] == nil {
		work = tx.Get(voidStorePrefix + id)
		auctionMapEdit(work, "readers")[viewer] = Record{"roleId": owner, "time": now.Unix()}
	}
	return Record{"work": voidWorkView(work, viewer), "result": result, "wallet": voidWallet(role)}, nil
}

func voidList(tx *StoreTx, q voidRequest, viewer string) (Record, error) {
	if q.Page < 0 {
		return nil, forgeFailure("页码无效")
	}
	board := q.Action == "board"
	mode := q.Mode
	if mode == "" {
		mode = "quality"
	}
	if board && q.Tag != "" && q.Tag != "全部" && !slices.Contains(voidBoardTags, q.Tag) ||
		!board && (q.Grade < 0 || q.Grade > 9 || mode != "quality" && mode != "study" || q.HallID != "" && q.HallID != "all" && voidFindHall(q.HallID) == nil) {
		return nil, forgeFailure("未知的筛选条件")
	}
	rows := []Record{}
	for _, id := range tx.store.order {
		if !strings.HasPrefix(id, voidStorePrefix) {
			continue
		}
		rec := tx.View(id)
		if board {
			if roleString(rec, "kind") == "post" && (q.Tag == "" || q.Tag == "全部" || roleString(rec, "tag") == q.Tag) {
				rows = append(rows, voidPostView(rec))
			}
		} else if roleString(rec, "kind") == "work" && (q.HallID == "" || q.HallID == "all" || roleString(rec, "hallId") == q.HallID) &&
			(q.Grade == 0 || int(roleInt(rec, "grade")) == q.Grade) {
			rows = append(rows, voidWorkView(rec, viewer))
		}
	}
	field, listKey := "score", "works"
	if board {
		field, listKey = "time", "posts"
	} else if mode == "study" {
		field = "readerCount"
	}
	sort.Slice(rows, func(i, j int) bool {
		a, b := roleInt(rows[i], field), roleInt(rows[j], field)
		if a != b {
			return a > b
		}
		return roleString(rows[i], "id") < roleString(rows[j], "id")
	})
	if !board {
		authors := make(map[string]bool)
		ranked, last, place := rows[:0], int64(-1), 0
		for _, row := range rows {
			author := roleString(row, "authorId")
			if mode == "quality" && authors[author] {
				continue
			}
			authors[author] = true
			value := roleInt(row, field)
			if value != last {
				place, last = len(ranked)+1, value
			}
			row["place"] = place
			ranked = append(ranked, row)
		}
		rows = ranked
	}
	start := len(rows)
	if q.Page <= len(rows)/voidPageSize {
		start = q.Page * voidPageSize
	}
	end := min(len(rows), start+voidPageSize)
	return Record{listKey: rows[start:end], "more": end < len(rows)}, nil
}
