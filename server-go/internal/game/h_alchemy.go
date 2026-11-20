package game

import (
	"fmt"

	"google.golang.org/protobuf/proto"

	"xzjserver/internal/affinity"
	"xzjserver/internal/gateway"
	"xzjserver/internal/pb"
	"xzjserver/internal/store"
)

// 炼丹（地火阁）采用「服务端结算」：客户端只上报工序与火候，成功与否由服务端
// 用丹方表重算，改包加不了丹。
//
// 请求 1005027：MethodElixirId（丹方键或成品模板键）、StepDetailList（每步
// StepId、GoodsId、BestTime 火候）。客户端自带的 IsRefiningSuccess 不采信。
// 回包 1005027：Code、IsSuccess、Count（粒数）、Rank（品阶）、CurrentVigor、
// LiveLevel、LiveProficiency（丹师等级与熟练度）。
const (
	alchemyCmd = 1005027

	// 职业编号沿用原工程：Live1Type / Live2Type 记职业类型，1 = 炼丹，
	// 对应等级字段就是丹师品阶（0～9）。
	professionAlchemy = 1

	// 精力消耗按组件份数折算。正式数值待平衡表审定后挪进配置。
	vigorPerUnit = 2
)

// 两个装配钩子由完整 Go 工程注入。切片仓库缺 gateway/pb/store 的具体实现，
// 这里不硬编码背包与规则目录的取法。
var (
	// AlchemyCatalog 返回已装载的规则目录；nil 表示本进程没有炼丹规则。
	AlchemyCatalog func() *affinity.Catalog
	// AlchemyBag 扣掉丹方全部组件，成功时把 Count 粒成品（带药效快照）入袋。
	// role 是会话里的角色对象，交给 store 处理，避免在这里写死背包 API。
	AlchemyBag func(role any, recipe affinity.AlchemyRecipe, outcome affinity.AlchemyOutcome) error
)

func registerAlchemy() {
	gateway.Register(alchemyCmd, onRefiningElixir)
}

func onRefiningElixir(s *gateway.Session, msg *pb.Msg) error {
	req := &pb.RefiningElixirReq_1005027{}
	_ = proto.Unmarshal(msg.Data, req)

	resp := &pb.RefiningElixirResp_1005027{}
	r := s.Role
	fail := func(code int32) error {
		resp.Code = code
		resp.IsSuccess = false
		resp.CurrentVigor = int32(r.Num("CurrentVigor"))
		s.Send(alchemyCmd, msg.Msid, resp)
		return nil
	}
	if r == nil {
		return fail(3001)
	}
	EnsureDefaults(r)

	rank, ok := alchemyProfessionRank(r.Num("Live1Type"), r.Num("Live1Level"), r.Num("Live2Type"), r.Num("Live2Level"))
	if !ok {
		return fail(4059) // 未修行炼丹
	}
	if AlchemyCatalog == nil || AlchemyBag == nil {
		return fail(4302)
	}
	catalog := AlchemyCatalog()
	if catalog == nil {
		return fail(4302)
	}

	ctx := affinity.AlchemyContext{
		AlchemistRank: rank,
		FlameID:       alchemyFlame(r.Str("AlchemyFlame")),
		PlaceSuccess:  int(r.Num("AlchemyPlaceBps")),
		PlaceRegionID: int(r.Num("MapId")),
	}
	if element, grade := int(r.Num("MethodElement")), int(r.Num("MethodGradeId")); element != 0 && grade != 0 {
		ctx.MethodElement, ctx.MethodRank = element, grade
	}

	steps := make([]affinity.AlchemyStep, 0, len(req.GetStepDetailList()))
	for _, step := range req.GetStepDetailList() {
		steps = append(steps, affinity.AlchemyStep{
			StepID: step.GetStepId(), GoodsID: step.GetGoodsId(), BestTime: step.GetBestTime(),
		})
	}

	// 种子取角色与消息号：同一次上报重算结果一致，客户端重发也不会变。
	seed := r.Lng("RoleId") ^ int64(msg.Msid) ^ int64(len(steps))
	outcome, recipeID, err := catalog.SettleAlchemy(req.GetMethodElixirId(), ctx, steps, seed)
	if err != nil {
		return fail(4302)
	}
	recipe, _ := catalog.AlchemyRecipe(recipeID)

	cost := vigorPerUnit * max(1, affinity.AlchemyUnits(catalog, recipe))
	if r.Num("CurrentVigor") < float64(cost) {
		return fail(4302) // 精力不足
	}
	if err := AlchemyBag(r, recipe, outcome); err != nil {
		return fail(4302)
	}
	r.Set("CurrentVigor", r.Num("CurrentVigor")-float64(cost))

	// 熟练度：成功按单粒药效累计，失败给保底 1。系数待平衡审定。
	proficiency := r.Num("Live2Proficiency")
	if outcome.Success {
		proficiency += float64(outcome.EffectValue * outcome.Count)
	} else {
		proficiency += 1
	}
	r.Set("Live2Proficiency", proficiency)
	_ = store.Save(r)

	if outcome.Success {
		resp.Code = 4301
		resp.IsSuccess = true
		resp.Count = int32(outcome.Count)
		resp.Rank = int32(alchemyQualityRank(catalog, recipe))
	} else {
		resp.Code = 4302
		resp.IsSuccess = false
	}
	resp.CurrentVigor = int32(r.Num("CurrentVigor"))
	resp.LiveLevel = int32(r.Num("Live2Level"))
	resp.LiveProficiency = int32(proficiency)
	s.Send(alchemyCmd, msg.Msid, resp)

	if outcome.Success {
		notifyText(s, fmt.Sprintf("炼丹成功，得%d粒", outcome.Count))
	} else {
		notifyText(s, "丹火不稳，本炉未成")
	}
	return nil
}

// 丹师品阶：两个职业槽里命中类型 1 的槽位取对应等级，上限 9。
func alchemyProfessionRank(typeA, levelA, typeB, levelB float64) (int, bool) {
	for _, slot := range [...][2]float64{{typeA, levelA}, {typeB, levelB}} {
		if int(slot[0]) == professionAlchemy {
			return min(9, max(0, int(slot[1]))), true
		}
	}
	return 0, false
}

// 炉火取角色配置，缺省退回最易起炉的松炭炉火。
func alchemyFlame(configured string) string {
	if configured != "" {
		return configured
	}
	return "flame.ember"
}

func alchemyQualityRank(catalog *affinity.Catalog, recipe affinity.AlchemyRecipe) int {
	item, ok := catalog.Item(recipe.OutputID)
	if !ok {
		return 1
	}
	grade, ok := catalog.Grade(item.GradeID)
	if !ok {
		return 1
	}
	return grade.QualityID
}
