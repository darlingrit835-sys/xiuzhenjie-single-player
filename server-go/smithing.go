package main

import (
	"crypto/rand"
	"crypto/sha256"
	"encoding/binary"
	"encoding/hex"
	"encoding/json"
	"errors"
	"fmt"
	"maps"
	"os"
	"path/filepath"
	"slices"
	"strconv"
	"strings"
	"unicode"
	"unicode/utf8"

	"google.golang.org/protobuf/reflect/protoreflect"
	"xiuzhenjie/server/internal/affinity"
)

const (
	smithingCatalogCmd  = 4901020
	smithingCreateCmd   = 4901021
	smithingSupplyCmd   = 4901022
	smithingTemplateCmd = 4901023
	smithingPlanCmd     = 4901024
	smithingCastPlanCmd = 4901025
	smithingCastCmd     = 4901026
	smithingStorePrefix = "$smithing:"
	smithingSupplyPrice = 120
)

type smithingLabel struct{ Name string }
type smithingLocale struct {
	CatalogVersion                                           string
	ItemTemplates, Stats, Grades, LegacyGrades               map[string]smithingLabel
	Polarities, Elements, QiTypes, ForgeFlames, ForgeEffects map[string]smithingLabel
}
type smithingWorkshop struct {
	methods                   *methodCatalog
	patterns                  map[string]Record
	engravings                map[string]patternWork
	patternOrder              []string
	rules                     *affinity.Catalog
	text                      smithingLocale
	blanks, materials, flames []smithingOption
	grades                    []Record
	professions               map[int]smithingProfession
}
type smithingOption struct {
	SlotType       int    `json:"slotType,omitempty"`
	ControlText    string `json:"controlText,omitempty"`
	ID             string `json:"id"`
	Name           string `json:"name"`
	GradeID        int    `json:"gradeId"`
	GradeName      string `json:"gradeName"`
	ElementID      int    `json:"elementId,omitempty"`
	AffinityLabel  string `json:"affinityLabel,omitempty"`
	VeinLabel      string `json:"veinLabel,omitempty"`
	Difficulty     int    `json:"difficulty,omitempty"`
	DifficultyName string `json:"difficultyName,omitempty"`
	DifficultyText string `json:"difficultyText,omitempty"`
	Count          int64  `json:"count"`
}

func (s *Server) loadSmithingWorkshop() error {
	dir := filepath.Join(s.dataDir, "config")
	f, err := os.Open(filepath.Join(dir, "cultivation_affinities.json"))
	// Older installations can still use their other trial routes.
	if errors.Is(err, os.ErrNotExist) {
		return nil
	}
	if err != nil {
		return err
	}
	defer f.Close()
	rules, err := affinity.Load(f)
	if err != nil {
		return err
	}
	w := &smithingWorkshop{rules: rules, methods: s.methods()}
	data, err := os.ReadFile(filepath.Join(dir, "cultivation_affinities.zh-CN.json"))
	if err != nil {
		return err
	}
	if err = json.Unmarshal(data, &w.text); err != nil {
		return err
	}
	if rules.Version() != w.text.CatalogVersion {
		return errors.New("炼器运行表与中文表版本不同")
	}
	options := func(ids []string) []smithingOption {
		out := make([]smithingOption, 0, len(ids))
		for _, id := range ids {
			item, _ := rules.Item(id)
			profile, _ := rules.Profile(item.ProfileID)
			out = append(out, smithingOption{
				ID: id, Name: w.text.ItemTemplates[id].Name,
				GradeID: item.GradeID, GradeName: w.gradeName(item.GradeID),
				ElementID: profile.PrimaryElementID, AffinityLabel: w.profileLabel(profile),
			})
		}
		return out
	}
	w.blanks, w.materials = options(rules.ForgeBlanks()), options(rules.ForgeMaterials())
	for i := range w.blanks {
		option := &w.blanks[i]
		item, _ := rules.Item(option.ID)
		option.SlotType = rules.ForgeEquipmentType(item.CategoryID)
		c := w.ratePattern(option.ID, 0, nil, patternWork{})
		option.Difficulty, option.DifficultyName, option.DifficultyText = c.Level, c.Name, c.Text
	}
	for i := range w.materials {
		option := &w.materials[i]
		stat, bps := w.materialVein(option.ID)
		if stat != "" {
			option.VeinLabel = w.affixText(Record{stat: bps})
		}
	}
	for _, id := range rules.ForgeFlames() {
		flame, _ := rules.ForgeFlame(id)
		w.flames = append(w.flames, smithingOption{
			ID: id, Name: w.text.ForgeFlames[id].Name,
			GradeID: flame.GradeID, GradeName: w.gradeName(flame.GradeID),
			ElementID: flame.ElementID, AffinityLabel: w.text.Elements[strconv.Itoa(flame.ElementID)].Name,
		})
	}
	slices.SortStableFunc(w.flames, func(a, b smithingOption) int { return a.GradeID - b.GradeID })
	for id := range w.text.Grades {
		grade, err := strconv.Atoi(id)
		if err != nil {
			return err
		}
		w.grades = append(w.grades, Record{"id": grade, "name": w.gradeName(grade)})
	}
	slices.SortFunc(w.grades, func(a, b Record) int { return int(roleInt(a, "id") - roleInt(b, "id")) })
	profData, err := os.ReadFile(filepath.Join(dir, "xzjlianqilist.json"))
	if err != nil {
		return err
	}
	var ranks []smithingProfession
	if err = json.Unmarshal(profData, &ranks); err != nil {
		return err
	}
	w.professions = make(map[int]smithingProfession)
	for _, row := range ranks {
		w.professions[row.ID] = row
	}
	for i := 1; i <= 9; i++ {
		if w.professions[i].MaxExperience <= 0 {
			return fmt.Errorf("缺少炼器师第 %d 品的晋阶条件", i)
		}
	}
	if err = w.preparePatterns(s.roles); err != nil {
		return err
	}
	s.smithing = w
	return nil
}

func intsRecord(src map[string]int) Record {
	out := make(Record, len(src))
	for k, v := range src {
		out[k] = v
	}
	return out
}
func (w *smithingWorkshop) profileLabel(p affinity.Profile) string {
	label := w.text.Polarities[strconv.Itoa(p.PolarityID)].Name + w.text.Elements[strconv.Itoa(p.PrimaryElementID)].Name
	if qi := w.text.QiTypes[strconv.Itoa(p.PrimaryQiID)].Name; qi != "" {
		if label != "" {
			label += "·"
		}
		label += qi
	}
	return label
}

func (w *smithingWorkshop) gradeName(id int) string {
	key := strconv.Itoa(id)
	if g, ok := w.text.Grades[key]; ok {
		return g.Name
	}
	return w.text.LegacyGrades[key].Name
}
func (w *smithingWorkshop) affixText(stats Record) string {
	labels := make([]string, 0, len(stats))
	for _, k := range slices.Sorted(maps.Keys(stats)) {
		name := w.text.Stats[k].Name
		if name == "" {
			name = k
		}
		value := roleInt(stats, k)
		labels = append(labels, fmt.Sprintf("%s +%d.%02d%%", name, value/100, value%100))
	}
	return strings.Join(labels, " · ")
}
func (s *Server) smithingAssociation(_ *Peer, _, out protoreflect.Message) (func(), error) {
	previewEmptyContribution(nil, nil, out)
	if s.smithing != nil {
		out.SetUnknown(offerNumber(out.GetUnknown(), 4, 1))
	}
	return nil, nil
}

func forgeUsable(g Record) bool {
	return g != nil && !inventoryFlag(g, "IsLock") && !inventoryFlag(g, "IsUsing") && !inventoryFlag(g, "IsBinding") && !hasFriendOfferLock(g) && transferStackCount(g) > 0
}

type smithingProfession struct {
	ID            int     `json:"id"`
	Name          string  `json:"name"`
	MaxExperience float64 `json:"plantTopValue"`
}

var smithingCharacterFields = append([]string{"EquippedGoods", "ForgeRank", "ForgeExperience", "GongFaNaQi", "CurrentVigor", "VigorRecoveredAt", "Vigor", "Level"}, artisanCareerFields...)

func smithTraining(role Record) (rank, exp int, slot string) {
	rank, exp, slot = artisanTraining(role, 2, "ForgeRank", "ForgeExperience")
	return max(1, rank), exp, slot
}

func (w *smithingWorkshop) awardExperience(role Record, amount int) string {
	before := w.trainingText(role)
	rank, exp, slot := smithTraining(role)
	previousRank := rank
	amount = w.earnedExperience(role, amount)
	exp += amount
	for rank < 9 && exp >= int(w.professions[rank].MaxExperience) {
		exp -= int(w.professions[rank].MaxExperience)
		rank++
	}
	exp = min(exp, int(w.professions[rank].MaxExperience))
	role["ForgeRank"], role["ForgeExperience"] = rank, exp
	if slot != "" {
		role[slot+"Level"], role[slot+"Value"] = rank, exp
	}
	after := w.trainingText(role)
	if before == after {
		return after
	}
	if rank > previousRank {
		return before + " → 晋品！" + after
	}
	return before + " → " + after
}

func (w *smithingWorkshop) context(role Record, req forgeRequest) (affinity.ForgeContext, string, string) {
	rank, _, slot := smithTraining(role)
	ctx := affinity.ForgeContext{PracticeBase: artisanLesson(int(w.professions[rank].MaxExperience), rank), WorkshopProgressBPS: int(forgeAffix(role, "smithing")), SmithRank: rank, FlameID: req.FlameID, GradeID: req.GradeID}
	smithName := w.professions[rank].Name
	if slot == "" {
		smithName += "（工坊修习）"
	}
	methodName := "未运转纳气功法"
	if def := w.methods.equipped(role, 3); def != nil && def.kind == 3 {
		ctx.MethodElement, ctx.MethodRank = def.element, def.rank
		methodName = def.name + "（" + w.text.Elements[strconv.Itoa(def.element)].Name + "）"
	}
	return ctx, smithName, methodName
}

func (s *Server) workshopDirectory(owner string) Record {
	r := s.roles.project(owner, append(slices.Clone(smithingCharacterFields), "Goods", "ForgeStarterClaimed", "ForgeMolds", "WorkshopSamples"))
	_, rankName, methodName := s.smithing.context(r, forgeRequest{})
	counts := make(map[string]int64)
	for _, val := range goodsList(r) {
		g, _ := val.(Record)
		if forgeUsable(g) {
			counts[roleString(g, "TemplateId")] += transferStackCount(g)
		}
	}
	flames := slices.Clone(s.smithing.flames)
	rank, _, _ := smithTraining(r)
	for i, flame := range flames {
		mastery, _ := s.smithing.rules.FlameMastery(flame.ID, rank)
		flames[i].ControlText = s.smithing.flameControlText(mastery)
	}
	materials := slices.Clone(s.smithing.materials)
	for i := range materials {
		materials[i].Count = counts[materials[i].ID]
	}
	return Record{
		"patterns": s.smithing.patternDirectory(r), "moldVersion": 1, "molds": s.smithing.moldDirectory(r), "batchLimit": forgeBatchLimit(r),
		"smithRankName": rankName, "methodName": methodName,
		"flames": flames, "grades": s.smithing.grades, "trainingText": s.smithing.trainingText(r),
		"blanks": s.smithing.blanks, "materials": materials,
		"canClaimStarter": !roleBool(r, "ForgeStarterClaimed"), "supplyPrice": smithingSupplyPrice,
	}
}

type forgeInput struct {
	TemplateID string `json:"templateId"`
	Quantity   int    `json:"quantity"`
}
type forgeRequest struct {
	PatternID     string       `json:"patternId,omitempty"`
	Strokes       []int        `json:"strokes,omitempty"`
	MoldID        string       `json:"moldId,omitempty"`
	Quantity      int          `json:"quantity,omitempty"`
	RequestID     string       `json:"requestId"`
	FlameID       string       `json:"flameId"`
	GradeID       int          `json:"gradeId"`
	BlankID       string       `json:"blankId"`
	Name          string       `json:"name"`
	Inputs        []forgeInput `json:"inputs"`
	ExpectedPrice int          `json:"expectedPrice"`
	TemplateID    string       `json:"templateId"`
}

func smithingTemplateID(owner, token string) string {
	digest := sha256.Sum256([]byte(owner + "\x00" + token))
	return "ugc." + hex.EncodeToString(digest[:16])
}
func (s *Server) smithingTemplate(id string) Record {
	if !strings.HasPrefix(id, "ugc.") || !validOfferToken(strings.TrimPrefix(id, "ugc.")) {
		return nil
	}
	return s.roles.Get(smithingStorePrefix + id)
}

// Freeze each source separately so a later rebalance cannot rewrite a weapon's provenance.
func forgeEffectRecord(f affinity.ForgeFactor) Record {
	effects := Record{}
	for key, value := range map[string]int{"successBps": f.SuccessBPS, "returnBps": f.ReturnBPS, "rollBiasBps": f.RollBiasBPS, "expBps": f.ExpBPS} {
		if value != 0 {
			effects[key] = value
		}
	}
	return effects
}

func (w *smithingWorkshop) processAffixText(t Record) string {
	groups, _ := t["ProcessAffixes"].(Record)
	var lines []string
	for _, source := range []struct{ key, name string }{{"smith", "师阶"}, {"flame", "炉火"}, {"target", "目标品阶"}} {
		effects, _ := groups[source.key].(Record)
		var terms []string
		for _, key := range []string{"successBps", "returnBps", "rollBiasBps", "expBps"} {
			if value := roleInt(effects, key); value != 0 {
				label := w.text.ForgeEffects[key].Name
				terms = append(terms, fmt.Sprintf("%s %+.2f%%", label, float64(value)/100))
			}
		}
		if len(terms) > 0 {
			lines = append(lines, source.name+"　"+strings.Join(terms, " · "))
		}
	}
	return strings.Join(lines, "\n")
}

func (w *smithingWorkshop) weaponView(t Record) Record {
	grade, p, e, q := int(roleInt(t, "Rank")), int(roleInt(t, "PolarityId")), int(roleInt(t, "FiveElement")), int(roleInt(t, "QiId"))
	stats, _ := t["Bonuses"].(Record)
	material, _ := t["MaterialAffixes"].(Record)
	resonance := moldResonance(roleString(t, "MoldQuality"))
	effect := ""
	if resonance > 0 {
		effect = fmt.Sprintf("同%s行术法威力 +%d%%，增益术延长持续时间；多件共鸣取最高值", w.text.Elements[strconv.Itoa(e)].Name, resonance)
	}
	return Record{"slotType": t["Type"], "effectText": effect, "materialText": w.affixText(material), "moldId": t["MoldId"], "moldQuality": t["MoldQuality"], "shapeBalance": roleInt(t, "ShapeBalance"), "templateId": t["TemplateId"], "name": t["Name"], "blankId": t["BlankId"], "gradeId": grade, "gradeName": w.gradeName(grade),
		"elementId": e, "elementName": w.text.Elements[strconv.Itoa(e)].Name, "polarityId": p, "polarityName": w.text.Polarities[strconv.Itoa(p)].Name,
		"qiId": q, "qiName": w.text.QiTypes[strconv.Itoa(q)].Name, "baseAttack": t["BaseAttack"], "baseDefense": t["BaseDefense"], "creator": t["CreatedBy"],
		"bonusText": w.affixText(stats), "bonuses": stats, "processAffixText": w.processAffixText(t), "experience": roleInt(t, "Experience")}
}

func (s *Server) handleSmithing(peer *Peer, msg Message) {
	owner := peer.currentRoleID()
	active, online := s.online.Load(owner)
	reply := func(code int, body Record) {
		data, _ := json.Marshal(body)
		wire := offerNumber(nil, 1, uint64(code))
		if code == 2000 {
			wire = offerNumber(wire, 2, 1)
		}
		_ = peer.sendPayload(msg.Cmd, msg.Msid, offerString(wire, 3, string(data)))
	}
	if (s.preview != nil && !s.previewWrites) || s.smithing == nil || owner == "" || reservedStoreID(owner) || !online || active != peer {
		reply(4008, Record{"error": "本服未开放炼器工坊，或角色已离线"})
		return
	}
	if msg.Cmd == smithingCatalogCmd {
		reply(2000, s.workshopDirectory(owner))
		return
	}
	var req forgeRequest
	if len(msg.Data) > 4096 || json.Unmarshal([]byte(offerToken(msg.Data)), &req) != nil {
		reply(4008, Record{"error": "工坊请求格式有误"})
		return
	}
	if msg.Cmd == smithingTemplateCmd {
		t := s.smithingTemplate(req.TemplateID)
		if t == nil {
			reply(4070, Record{"error": "没有这件创制武器的模板"})
			return
		}
		reply(2000, s.smithing.weaponView(t))
		return
	}
	if msg.Cmd != smithingPlanCmd && msg.Cmd != smithingCastPlanCmd && !validOfferToken(req.RequestID) {
		reply(4008, Record{"error": "炼器请求编号无效"})
		return
	}
	var result Record
	var err error
	if msg.Cmd == smithingSampleCmd {
		result, err = s.claimWorkshopSample(owner, req.TemplateID)
	} else if msg.Cmd == smithingSupplyCmd {
		err = s.obtainForgeMaterials(owner, req)
		if err == nil {
			result = s.workshopDirectory(owner)
		}
	} else if msg.Cmd == smithingCastCmd || msg.Cmd == smithingCastPlanCmd {
		result, err = s.castFromMold(owner, req, msg.Cmd == smithingCastCmd)
	} else if msg.Cmd == smithingPlanCmd {
		result, err = s.forgePreview(owner, req)
	} else {
		result, err = s.craftWeapon(owner, req)
	}
	if err != nil {
		var reason forgeFailure
		if errors.As(err, &reason) {
			reply(4008, Record{"error": string(reason)})
		} else {
			reply(5000, Record{"error": "工坊未能保存本次操作，请重试"})
		}
		return
	}
	reply(2000, result)
}

type forgeFailure string

func (f forgeFailure) Error() string { return string(f) }

// One starter bundle can furnish a robe, a ward and a pair of boots from their original recipes.
var forgeSupplyBundle = [...]struct {
	id          string
	count, kind int
}{
	{"mat.hanwen_iron", 3, 13}, {"mat.cloudwood", 3, 12}, {"mat.cold_dew", 2, 12}, {"mat.common_hide", 2, 8},
}

func (s *Server) obtainForgeMaterials(owner string, req forgeRequest) error {
	if req.ExpectedPrice != 0 && req.ExpectedPrice != smithingSupplyPrice {
		return forgeFailure("领料价格已变，请刷新工坊")
	}
	key := smithingStorePrefix + "supply." + smithingTemplateID(owner, req.RequestID)
	return s.roles.Transact(func(tx *StoreTx) error {
		if receipt := tx.View(key); receipt != nil {
			if roleInt(receipt, "Price") != int64(req.ExpectedPrice) {
				return forgeFailure("领料请求编号已使用")
			}
			return nil
		}
		role := tx.Get(owner)
		if role == nil {
			return ErrRecordNotFound
		}
		price := 0
		if roleBool(role, "ForgeStarterClaimed") {
			price = smithingSupplyPrice
		}
		if req.ExpectedPrice != price {
			return forgeFailure("领料状态已变化，请刷新工坊后确认价格")
		}
		if roleInt(role, "ShinyStone") < int64(price) {
			return forgeFailure("灵石不足，无法补领材料")
		}
		bundleUnits := 0
		for _, entry := range forgeSupplyBundle {
			bundleUnits += entry.count
		}
		for _, supply := range forgeSupplyBundle {
			item, ok := s.smithing.rules.Item(supply.id)
			if !ok {
				if supply.id == "mat.common_hide" { // Data packs predating animal hides have no boot pattern.
					continue
				}
				return forgeFailure("当前配置缺少入门材料")
			}
			profile, _ := s.smithing.rules.Profile(item.ProfileID)
			g := Record{"TemplateId": supply.id, "Name": s.smithing.text.ItemTemplates[supply.id].Name, "Type": supply.kind, "Rank": item.GradeID, "Level": 1, "Stage": 1,
				"FiveElement": profile.PrimaryElementID, "Volume": 0.1, "ConversionRate": 1.0, "RecyclePrice": 0, "IsUsing": false, "IsLock": false, "IsBinding": false}
			g["ForgePaidUnit"] = price / bundleUnits
			if s.alchemy != nil && slices.Contains(s.alchemy.materials, supply.id) {
				g["BrewPaidUnit"] = price / bundleUnits
			}
			if _, err := receiveInventoryGoods(role, g, int64(supply.count)); err != nil {
				return forgeFailure("纳戒空间不足，整包材料仍可领取")
			}
		}
		role["ShinyStone"] = roleInt(role, "ShinyStone") - int64(price)
		role["ForgeStarterClaimed"] = true
		tx.GetOrCreate(key)["Price"] = price
		return nil
	})
}

func selectForgeInputs(req forgeRequest) (map[string]int, error) {
	if len(req.Inputs) == 0 || len(req.Inputs) > affinity.MaxForgeUnits {
		return nil, forgeFailure("请选择合计 2 至 12 份炼器材料")
	}
	inputs := make(map[string]int, len(req.Inputs))
	total := 0
	for _, line := range req.Inputs {
		if line.TemplateID == "" || line.Quantity < 1 || line.Quantity > affinity.MaxForgeUnits || inputs[line.TemplateID] != 0 {
			return nil, forgeFailure("材料或份数无效")
		}
		inputs[line.TemplateID] = line.Quantity
		total += line.Quantity
	}
	if total < affinity.MinForgeUnits || total > affinity.MaxForgeUnits {
		return nil, forgeFailure("请选择合计 2 至 12 份炼器材料")
	}
	return inputs, nil
}

func consumeForgeInputs(role Record, inputs map[string]int) error {
	remaining := maps.Clone(inputs)
	role["Goods"] = slices.DeleteFunc(goodsList(role), func(v any) bool {
		g, _ := v.(Record)
		id := roleString(g, "TemplateId")
		need := remaining[id]
		if need == 0 || !forgeUsable(g) {
			return false
		}
		count := transferStackCount(g)
		used := min(count, int64(need))
		remaining[id] -= int(used)
		g["Count"] = count - used
		return count == used
	})
	for _, n := range remaining {
		if n != 0 {
			return forgeFailure("可用材料不足，已锁定、绑定或正在使用的材料不能投入")
		}
	}
	return nil
}
func (s *Server) forgePreview(owner string, req forgeRequest) (Record, error) {
	inputs, err := selectForgeInputs(req)
	if err != nil {
		return nil, err
	}
	role := s.roles.project(owner, smithingCharacterFields)
	if role == nil {
		return nil, ErrRecordNotFound
	}
	w := s.smithing
	if err = w.validatePatternOrder(req); err != nil {
		return nil, err
	}
	targetGrade := req.GradeID
	challenge := w.orderDifficulty(req, inputs)
	shape, err := w.shapeForgeOrder(&req)
	if err != nil {
		return nil, err
	}
	ctx, _, _ := w.context(role, req)
	ctx.PatternDifficulty = challenge.Level
	plan, err := w.rules.PlanForge(req.BlankID, inputs, ctx)
	if err != nil {
		return nil, forgeFailure("请选择目录中的器胚、材料、炉火和常规品阶")
	}
	return w.firstForgeView(role, req, targetGrade, inputs, challenge, shape, plan), nil
}

func verifyForgeStock(role Record, inputs map[string]int) error {
	stock := make(map[string]int64, len(inputs))
	for _, entry := range goodsList(role) {
		g, _ := entry.(Record)
		id := roleString(g, "TemplateId")
		if inputs[id] != 0 && forgeUsable(g) {
			stock[id] += transferStackCount(g)
		}
	}
	for id, n := range inputs {
		if stock[id] < int64(n) {
			return forgeFailure("可用材料不足，请核对纳戒中可投入的份数")
		}
	}
	return nil
}

func (w *smithingWorkshop) failureView(t Record) Record {
	returned, _ := t["Returned"].(Record)
	pieces := make([]string, 0, len(returned))
	for _, id := range slices.Sorted(maps.Keys(returned)) {
		pieces = append(pieces, fmt.Sprintf("%s ×%d", w.text.ItemTemplates[id].Name, roleInt(returned, id)))
	}
	summary := "本炉没有收回材料"
	if len(pieces) > 0 {
		summary = "收回 " + strings.Join(pieces, "、")
	}
	exp := roleInt(t, "Experience")
	return addFirstForgeReceipt(Record{"forged": false, "message": fmt.Sprintf("本炉未成器，获得 %d 点炼器经验", exp), "returned": returned, "returnedText": summary, "experience": exp}, t)
}

func (s *Server) craftWeapon(owner string, req forgeRequest) (Record, error) {
	var entropy [8]byte
	if _, err := rand.Read(entropy[:]); err != nil {
		return nil, err
	}
	return s.resolveSmithing(owner, req, int64(binary.LittleEndian.Uint64(entropy[:])))
}

func (s *Server) resolveSmithing(owner string, req forgeRequest, seed int64) (Record, error) {
	name := strings.TrimSpace(req.Name)
	if name == "" || utf8.RuneCountInString(name) > 20 || !utf8.ValidString(name) || strings.IndexFunc(name, func(r rune) bool { return !unicode.IsPrint(r) || r == '<' || r == '>' }) >= 0 {
		return nil, forgeFailure("请填写 1 至 20 字的武器名，不含标记或控制字符")
	}
	inputs, err := selectForgeInputs(req)
	if err != nil {
		return nil, err
	}
	id := smithingTemplateID(owner, req.RequestID)
	signature := []any{req.BlankID, req.FlameID, req.GradeID, name, inputs}
	if len(req.Strokes) > 0 {
		signature = append(signature, req.Strokes)
	}
	if req.PatternID != "" {
		signature = append(signature, req.PatternID)
	}
	canonical, _ := json.Marshal(signature)
	hash := fmt.Sprintf("%x", sha256.Sum256(canonical))
	key, failedKey := smithingStorePrefix+id, smithingStorePrefix+"miss."+id
	w := s.smithing
	var result Record
	successView := func(t Record) Record {
		view := w.weaponView(t)
		view["forged"], view["goodsId"], view["experience"] = true, t["OriginalGoodsId"], t["Experience"]
		return addFirstForgeReceipt(view, t)
	}
	err = s.roles.Transact(func(tx *StoreTx) error {
		for _, receiptKey := range []string{key, failedKey} {
			if prev := tx.View(receiptKey); prev != nil {
				if roleString(prev, "RequestHash") != hash {
					return forgeFailure("炼器请求编号已用于其他配方")
				}
				if receiptKey == key {
					result = successView(prev)
				} else {
					result = w.failureView(prev)
				}
				return nil
			}
		}
		role := tx.Get(owner)
		if role == nil {
			return ErrRecordNotFound
		}
		if err := verifyForgeStock(role, inputs); err != nil {
			return err
		}
		if err := w.validatePatternOrder(req); err != nil {
			return err
		}
		targetGrade := req.GradeID
		challenge := w.orderDifficulty(req, inputs)
		shape, err := w.shapeForgeOrder(&req)
		if err != nil {
			return err
		}
		ctx, _, _ := w.context(role, req)
		ctx.PatternDifficulty = challenge.Level
		outcome, err := w.rules.ResolveForge(req.BlankID, inputs, ctx, seed)
		if err != nil {
			return forgeFailure("请选择目录中的器胚、材料、炉火和常规品阶")
		}
		info := w.firstForgeView(role, req, targetGrade, inputs, challenge, shape, outcome.Plan)
		vigor := w.forgeVigor(targetGrade, challenge.Level, 1, true)
		if !spendArtisanVigor(role, vigor) {
			return forgeFailure(fmt.Sprintf("本次首炼需 %d 精力", vigor))
		}
		materialValue := forgeMaterialValue(role, inputs)
		if outcome.Success {
			outcome.Experience += req.studyBonus(challenge)
		}
		outcome.Experience = w.earnedExperience(role, outcome.Experience)
		if !outcome.Success {
			consumed := maps.Clone(inputs)
			for mat, n := range outcome.Returned {
				consumed[mat] -= n
			}
			if err := consumeForgeInputs(role, consumed); err != nil {
				return err
			}
			t := tx.GetOrCreate(failedKey)
			maps.Copy(t, Record{"RequestHash": hash, "Returned": intsRecord(outcome.Returned), "Experience": outcome.Experience})
			info["trainingText"] = w.awardExperience(role, outcome.Experience)
			info["studyExperience"] = 0
			info["spentText"] = w.materialUseText(consumed)
			t["FirstForgeReceipt"] = info
			result = w.failureView(t)
			return nil
		}
		if err := consumeForgeInputs(role, inputs); err != nil {
			return err
		}
		weapon := outcome.Weapon
		shape.apply(weapon)
		t := tx.GetOrCreate(key)
		smith, _ := w.rules.ForgeSmithRank(ctx.SmithRank)
		flame, _ := w.rules.ForgeFlame(ctx.FlameID)
		target, _ := w.rules.ForgeTarget(ctx.GradeID)
		t["ProcessAffixes"] = Record{"smith": forgeEffectRecord(smith), "flame": forgeEffectRecord(flame.ForgeFactor), "target": forgeEffectRecord(target.ForgeFactor)}
		maps.Copy(t, Record{"Name": name, "TemplateId": id, "CreatedBy": roleString(role, "Name"), "CreatorRoleId": owner, "RequestHash": hash,
			"Experience": outcome.Experience, "FlameId": ctx.FlameID, "SmithRank": ctx.SmithRank, "MethodId": roleString(role, "GongFaNaQi")})
		t["MoldQuality"] = shape.quality
		w.setForgedWeapon(t, weapon)
		challenge.DescribeInto(t)
		if len(req.Strokes) > 0 {
			saveForgeMold(role, t, inputs, shape)
		}
		g := forgedGoods(t)
		estimate, recycle := w.quoteForgedValue(t, materialValue, 1)
		g["RecyclePrice"] = recycle
		info["appraisal"], info["recyclePrice"] = estimate, recycle
		info["processText"] = forgeQuoteText(vigor, estimate, recycle)
		goodsID, err := receiveInventoryGoods(role, g, 1)
		if err != nil {
			return forgeFailure("纳戒空间不足，本次炼器未消耗材料")
		}
		t["OriginalGoodsId"] = goodsID
		info["trainingText"] = w.awardExperience(role, outcome.Experience)
		info["spentText"] = w.materialUseText(inputs)
		t["FirstForgeReceipt"] = info
		e := newHistoryEvent("smithing.weapon_created", role, nil, "weapon", id, Record{"goodsId": goodsID, "blankId": weapon.PrototypeID, "gradeId": weapon.GradeID})
		e.Actor.RoleID = owner
		if err = tx.RecordHistory(e); err != nil {
			return err
		}
		result = successView(t)
		return nil
	})
	return result, err
}
