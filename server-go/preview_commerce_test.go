package main

import (
	"math"
	"testing"

	"google.golang.org/protobuf/proto"
	"google.golang.org/protobuf/reflect/protoreflect"
)

func TestCommercePreviewMessagesMatchClientLayout(t *testing.T) {
	p, err := loadProtocol()
	if err != nil {
		t.Fatal(err)
	}
	role := Record{"Id": "role", "Name": "检验角色", "Level": 3, "Heat": 10, "ShinyStone": 123}
	for _, route := range commercePreviewRoutes() {
		if route.blocked {
			continue
		}
		t.Run(route.response, func(t *testing.T) {
			out := p.message(route.response)
			var req protoreflect.Message
			if route.request != "" {
				req = p.message(route.request)
				switch route.cmd {
				case 1002024:
					set(req, "Type", -1)
				case 1006011, 1006017:
					set(req, "Stage", 4)
				}
			}
			statusPB(out, true, 2000)
			route.build(role, req, out)
			wire, err := proto.Marshal(out)
			if err != nil {
				t.Fatal(err)
			}
			decoded := p.message(route.response)
			if err := proto.Unmarshal(wire, decoded); err != nil {
				t.Fatal(err)
			}
			if !getBool(decoded, "IsSuccess") {
				t.Fatal("success status lost on wire")
			}
			switch route.cmd {
			case 1002018:
				if n := decoded.Get(field(decoded, "SignInAward")).List().Len(); n != 7 {
					t.Fatalf("sign-in panel expects seven rows, got %d", n)
				}
			case 1004033:
				if n := decoded.Get(field(decoded, "ReXiaoGoodsId")).List().Len(); n != 4 {
					t.Fatalf("hot goods panel expects four rows, got %d", n)
				}
			case 1006011, 1006018:
				lots := decoded.Get(field(decoded, "GoodsList")).List()
				priceField := "FinalPrice"
				if route.cmd == 1006011 {
					priceField = "StartPrice"
					if getInt(decoded, "Stage") != 4 {
						t.Fatalf("hall did not preserve the selected stage: %v", decoded)
					}
				}
				if lots.Len() != 1 {
					t.Fatalf("hall expects one preview lot, got %d", lots.Len())
				}
				lot := lots.Get(0).Message()
				if getString(lot, "GoodsName") != "试用·筑基丹" || getInt(lot, "FiveElement") != 0 ||
					getInt(lot, "Rank") != 4 || getInt(lot, priceField) != 350 {
					t.Fatalf("hall lot did not match the current preview elixir: %v", lot)
				}
			case 1006017:
				if getInt(decoded, "TreasureId") != 4 || getBool(decoded, "IsMayAddPrice") || getInt(decoded, "ShinyStone") != 123 {
					t.Fatal("preview auction changed balance or enabled bidding")
				}
			case 1006021:
				if getString(decoded, "GoodsId") != "sys_zhu_ji_dan_huang" || getInt(decoded, "GoodsType") != 19 ||
					getString(decoded, "GoodsName") != "试用·筑基丹" || getInt(decoded, "FiveElement") != 0 ||
					getInt(decoded, "Rank") != 4 || getInt(decoded, "Level") != 1 || getInt(decoded, "Stage") != 1 ||
					getInt(decoded, "StartPrice") != 350 || getInt(decoded, "CurrentPrice") != 350 {
					t.Fatalf("current preview lot cannot render the bundled elixir details: %v", decoded)
				}
			case 1017004:
				if n := decoded.Get(field(decoded, "JiFen")).List().Len(); n != 11 {
					t.Fatalf("points panel expects eleven rows, got %d", n)
				}
			}
		})
	}
}

func TestCommercePreviewAuctionFiltersAndPaging(t *testing.T) {
	p, err := loadProtocol()
	if err != nil {
		t.Fatal(err)
	}
	req := p.message("EnterAuctionReq_1006001")
	putPB(req, Record{"ShopType": 83, "PageNum": math.MaxInt32, "PageSize": 1, "FinalPriceSort": "desc"})
	response := p.message("EnterAuctionResp_1006001")
	previewBrowseAuctions(nil, req, response)
	rows := response.Get(field(response, "Object")).List()
	if getInt(response, "PageNum") != 2 || getInt(response, "Pages") != 2 ||
		rows.Len() != 1 || getInt(rows.Get(0).Message(), "FixedPrice") != 260 {
		t.Fatalf("second page should contain the lower-priced listing, got %v", response)
	}
	set(req, "Name", "不存在")
	response.Reset()
	previewBrowseAuctions(nil, req, response)
	if getInt(response, "PageNum") != 1 || response.Get(field(response, "Object")).List().Len() != 0 {
		t.Fatalf("empty result should retain valid page metadata, got %v", response)
	}
}
