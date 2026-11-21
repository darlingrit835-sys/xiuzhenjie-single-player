using System;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;
using static XiuZhenJie.AuctionHallState;

namespace XiuZhenJie
{
    internal sealed partial class AuctionHall
    {
        private void Render(bool keep=false)
        {
            float offset=keep && body!=null ? body.anchoredPosition.y : 0;
            if (root!=null) { root.gameObject.SetActive(false); Destroy(root.gameObject); }
            root=RoleFolio.Box(transform,"交易簿"); root.SetAsFirstSibling(); RoleFolio.Fill(root);
            var paper=root.gameObject.AddComponent<Image>(); InkInterfaceArt.Set(paper,"InkLedgerPaper",false); paper.raycastTarget=true;
            var head=Band(root,"檐头",0,42); Surface(head,Night,true);
            Button(Slice(head,0,.18f,3,3),"返回",close,Ink);
            Text(Slice(head,.19f,.80f),page==Page.Market ? "坊市 · 自由市场" : "金阙 · 拍卖会",20,Ivory,TextAnchor.MiddleCenter,true);
            if (page!=Page.Market) Button(Slice(head,.81f,1,2,3),pending!=null ? "核对上笔" : "刷新",()=>
            {
                if (busy) return;
                if (pending!=null) { Send(pending); return; }
                Refresh();
            },Ink);
            var pane=RoleFolio.Box(root,"账册"); RoleFolio.Fill(pane,0,0,42,page==Page.Market ? 0 : string.IsNullOrEmpty(notice) ? 49 : 82);
            Surface(pane,Color.clear,true); pane.gameObject.AddComponent<RectMask2D>();
            var scroll=pane.gameObject.AddComponent<ScrollRect>(); scroll.viewport=pane; scroll.horizontal=false;
            scroll.movementType=ScrollRect.MovementType.Clamped;
            body=RoleFolio.Box(pane,"卷页"); body.anchorMin=Vector2.up; body.anchorMax=Vector2.one; body.pivot=Vector2.up;
            body.sizeDelta=Vector2.zero; scroll.content=body; cursor=0; clock=null; chant=null; seatCounter=null; stageLight=null; receipt=null;
            if (page==Page.Market) MarketPage();
            else
            {
                if (page==Page.Catalogue)
                    Choices(TierTitles,tier,SelectTier);
                if (state==null)
                {
                    Hero(null);
                    Note(readError ? "尚未读到本场拍卖簿，可刷新再试。" : "掌柜正在展开本场宝录…");
                }
                else
                {
                    atDeadline=state.phase=="collect" ? state.previewAt : state.phase=="preview" ? state.liveAt :
                        state.phase=="live" ? LiveLot?.endsAt ?? state.endsAt : state.nextCollectAt;
                    switch (page)
                    {
                        case Page.Catalogue: Catalogue(); break;
                        case Page.Room: Room(); break;
                        case Page.Consign: Consign(); break;
                        case Page.Account: Account(); break;
                    }
                }
                var dock=RoleFolio.Box(root,"堂内去处"); dock.anchorMin=Vector2.zero; dock.anchorMax=Vector2.right;
                dock.offsetMin=Vector2.zero; dock.offsetMax=new Vector2(0,49); Surface(dock,Night,true);
                string[] names={"预展图录","竞价现场","自愿送拍","我的"};
                for(int i=0;i<4;i++) { var dest=(Page)(i+1); var r=Slice(dock,i/4f,(i+1)/4f,2,4); Button(r,names[i],()=>Switch(dest),page==dest ? Lacquer : Ink); }
            }
            if (!string.IsNullOrEmpty(notice))
            {
                var bar=RoleFolio.Box(root,"回执"); bar.anchorMin=Vector2.zero; bar.anchorMax=Vector2.right; bar.offsetMin=new Vector2(3,49); bar.offsetMax=new Vector2(-3,82); Surface(bar,Night);
                receipt=Text(bar,notice,13,Gold,TextAnchor.MiddleCenter,true);
            }
            body.sizeDelta=new Vector2(0,cursor+12);
            body.anchoredPosition=new Vector2(0,Mathf.Clamp(offset,0,Mathf.Max(0,cursor-pane.rect.height)));
            clockAt=0;
        }

        private string Guide(string key,string fallback,Lot lot=null)
        {
            string session=state?.eventId ?? DateTime.UtcNow.AddHours(8).ToString("yyyyMMdd")+"-"+shop;
            return AuctionGuide.Select(key,session,lot,state?.limit??0,fallback);
        }

        private void MarketPage()
        {
            Hero(null);
            Note(Guide("market","诸位请沿街选铺，挂牌、查价或翻阅自己的买入簿。"),38);
            var menu=Line(48);
            for(int i=0;i<ShopNames.Length;i++)
            {
                int at=i; Button(Slice(menu,i/7f,(i+1)/7f,1,3),ShopNames[i],()=>{shop=at;Render();},i==shop ? Lacquer : Ink);
            }
            var shopType=Shops[shop];
            Tile("浏览"+ShopNames[shop],"查看挂牌、按价筛选，确认总价后即可买下。",InkInterfaceArt.Get("Goods/"+ShopArt[shop]),"入市",
                ()=>Singleton<UIManager>.Instance.PopUpWnd("QiZhenGeWnd.prefab",true,shopType));
            Tile("标价上架","从纳戒选取"+ShopNames[shop]+"，填写数量与单价。",InkInterfaceArt.Get("Satchel"),"挂牌",
                ()=>Singleton<UIManager>.Instance.PopUpWnd("QZG_PaiMaiWnd.prefab",true,shopType));
            Tile("我的挂牌","查看已挂出的"+ShopNames[shop]+"与到期物品。",InkInterfaceArt.Get("Scroll"),"查看",
                ()=>Singleton<UIManager>.Instance.PopUpWnd("QZG_WoDePaiMaiWnd.prefab",true,shopType));
            Tile("我的买入","查阅自由市场购入记录。",InkInterfaceArt.Get("SilverIngot"),"账簿",
                ()=>Singleton<UIManager>.Instance.PopUpWnd("QZG_WoDeJingPaiWnd.prefab",true));
        }

        private void Catalogue()
        {
            Hero(Featured);
            var time=Line(46); Surface(time,Pale);
            Text(Slice(time,0,.51f,8,0),"截收　"+TimeAt(state.previewAt)+"\n开槌　"+TimeAt(state.liveAt),13,Ink);
            clock=Text(Slice(time,.52f,1,3,0),"",13,Lacquer,TextAnchor.MiddleRight,true);
            Title("本场图录",$"入选 {state.limit} 件");
            Note(state.phase=="collect" ? $"自 {TimeAt(state.collectAt)} 征集。按品阶、等级及送达次序取前 {state.limit} 件，空席由阁中藏品补齐。" :
                "点选图录可验看拍品。预展期间留下最高委托，开槌后仍可到场应价。");
            if (state.phase=="collect") Note(Guide("collect","今场藏品正在征集，截收后依序入册。"));
            if (state.phase=="preview") Note(Guide("preview","宝录已展，诸位请先过目。",Featured));
            Lots(false);
            if (state.phase=="collect") MainAction("前往自愿送拍",()=>Switch(Page.Consign));
            else if (state.phase=="live") MainAction("进入现场",()=>Switch(Page.Room));
        }

        private void Lots(bool mine)
        {
            var items=state.lots??Array.Empty<Lot>();
            bool ShowLot(Lot l)=>!mine || l.mine || l.participated || l.myMax>0 || l.leading;
            if(!items.Any(ShowLot)) { Note(mine ? "本场暂无你的寄售或出价记录。" : "图录正在编排。截收后可查看入选拍品和各件开拍时刻。"); return; }
            for(int idx=0;idx<items.Length;idx++)
            {
                var l=items[idx];if(!ShowLot(l))continue;
                var old=l; string price=string.IsNullOrEmpty(l.leader) ? "起拍" : "现价";
                Tile($"{idx+1:D2}　{l.name} ×{l.count}",$"{RingLedger.Kind(l.kind)} · {WorldSetting.GetRankAllName(l.rank)}　{price} {Money(l.price)} 灵石",Art(l),
                    l.status=="sold" || l.status=="passed" ? Status(l.status) : l.leading ? "我领价" : l.mine ? "我送拍" : l.participated ? "已被超价" : Status(l.status),()=>LotSheet(old));
            }
        }

        private int ChantBand(Lot lot)
        {
            long remain=lot.endsAt-Now;
            if (lot.id==firstBidLot && Time.unscaledTime<firstBidUntil && remain>0) return 4;
            return remain<=4000 ? 3 : remain<=10000 ? 2 : remain<=20000 ? 1 : 0;
        }

        // Use the inventory's item classification for the words spoken at the dais.
        // Manuals, recipes and diagrams keep their own names, even when their products share a shop.
        private static string PresentLot(Lot lot,out string opening,out string remark)
        {
            string name=lot.name, kind=RingLedger.Kind(lot.kind);
            switch(kind)
            {
                case "功法": case "经卷": case "法术书":
                    opening=$"展卷！请看这部{kind}《{name}》"; remark=$"这部《{name}》可否再展一页，让我细看？"; return "openBook";
                case "丹方": case "符书": case "阵图": case "器图":
                    opening=$"展开{kind}，请诸位过目《{name}》"; remark=$"《{name}》这份{kind}，我也想再看一眼。"; return "openRecipe";
                case "丹药":
                    opening=$"请看药案，「{name}」共{lot.count}份"; remark=$"这批「{name}」一共{lot.count}份，我记下了。"; return "openPill";
                case "法器": case "兵器": case "防具": case "足具":
                    opening=$"呈器上台！请看{kind}「{name}」"; remark=$"请再转一转「{name}」，我想细看器身。"; return "openGear";
                case "符箓":
                    opening=$"奉符上案！「{name}」共{lot.count}张"; remark=$"台上「{name}」的符面，请让我再看一眼。"; return "openTalisman";
                case "阵盘":
                    opening=$"呈阵盘！诸位请看「{name}」"; remark=$"「{name}」这方阵盘，容我再辨一辨盘面。"; return "openArray";
                case "矿材": case "陨铁": case "仙金": case "灵植": case "兽材":
                case "皮毛": case "精血": case "兽骨": case "妖丹": case "符笔": case "符纸": case "符墨":
                    opening=$"开箱点货！{kind}「{name}」共{lot.count}份"; remark=$"这批「{name}」共{lot.count}份，请容我算算价。"; return "openMaterial";
                default:
                    opening=$"请宝上台！诸位验看「{name}」"; remark=$"台上的「{name}」，我想再细看一回。"; return "openOther";
            }
        }

        private string Caller(Lot l)
        {
            if (l==null)
                return state.phase=="closed" ? "今场宝录已封，道友可至交割处提宝。" :
                    Guide("liveWait","宝台尚待开槌。可先查看预展图录与开场时刻。",Featured);
            string openKey=PresentLot(l,out var opening,out _);
            string kind=RingLedger.Kind(l.kind), price=Money(l.price);
            int band=ChantBand(l);
            if (string.IsNullOrEmpty(l.leader))
                return Guide(openKey,opening,l)+"　"+Money(l.start)+" 灵石起。";
            if (band==4) return Guide("firstBid",$"首牌有主！「{l.name}」，{l.leader}应价 {price} 灵石。",l);
            if (band==3) return Guide("finalCall",$"就要落槌！{kind}「{l.name}」，{l.leader}以 {price} 灵石领价。",l);
            if (band==2) return Guide("secondCall",$"{kind}「{l.name}」，{price} 灵石第二声。诸位请亮最后一牌。",l);
            if (band==1) return Guide("firstCall",$"{l.leader}的牌在前，{kind}「{l.name}」{price} 灵石第一声！",l);
            return Guide("leading",$"{opening}。{l.leader}应价 {price} 灵石，还有哪位举牌？",l);
        }

        private string HammerCall(Lot l)
        {
            return l.status=="sold" ? Guide("sold",$"落槌！{l.leader}以 {Money(l.price)} 灵石得「{l.name}」。",l) :
                Guide("passed",$"「{l.name}」无人应价，此件流拍。",l);
        }

        private static string SeatBidRemark(Lot l)
        {
            string kind=RingLedger.Kind(l.kind);
            if (l.mine) return $"这件{kind}「{l.name}」是我送拍，诸位请过目。";
            if (l.leading) return $"「{l.name}」这件{kind}，我暂领一手。";
            if (string.IsNullOrEmpty(l.leader)) return $"这件{kind}「{l.name}」尚无首价，我再斟酌。";
            return $"「{l.name}」眼下已到 {Money(l.price)} 灵石，且看下一牌。";
        }

        private void Room()
        {
            var lot=LiveLot; Hero(lot);
            var caller=Line(79);Surface(caller,Pale);
            var portrait=RoleFolio.Image(caller,"唱宝掌柜",Color.white);
            portrait.sprite=ReleasedResources.Load<Sprite>("InkPortraits/Man_5");portrait.preserveAspect=true;
            RoleFolio.Fixed(portrait.rectTransform,0,4,9,48,55);
            Text(Band(caller,"掌柜名牌",3,24,57,7),"司仪唱宝",15,Lacquer,TextAnchor.MiddleLeft,true);
            chant=Text(Band(caller,"唱词",27,49,57,6),Caller(lot),14,Ink);chantBand=lot==null ? -1 : ChantBand(lot);
            if(lot!=null)
            {
                var info=Line(42); Surface(info,Pale);
                Text(Slice(info,0,.47f,7,0),"当前 "+Money(lot.price)+" 灵石",16,Lacquer,TextAnchor.MiddleLeft,true);
                clock=Text(Slice(info,.47f,1,2,0),"",13,Ink,TextAnchor.MiddleRight);
                if(!lot.mine) MainAction(lot.leading ? "提高委托上限" : "举牌应价",()=>BidSheet(lot));
                else Note("这是你送拍的藏品，可在此观礼。");
                Title("席间传音",state.seat?.room??"散席");
                seatCounter=Text(Line(29),"",13,Ink);
                var buttons=Line(42);
                Button(Slice(buttons,0,.52f,1,1),"传音弹幕",()=>ChatSheet(lot),Ink);
                Button(Slice(buttons,.52f,1,1,1),"瓜果酒水 · 入雅间",ServiceSheet,Lacquer);
                foreach(var m in (state.messages??Array.Empty<Message>()).Where(x=>x.lotId==lot.id).Reverse().Take(4))
                    Note($"{m.room} · {m.name}　{m.text}",34);
            }
            else
            {
                var info=Line(36); clock=Text(info,"",14,Lacquer,TextAnchor.MiddleCenter,true);
                MainAction("回到本场图录",()=>Switch(Page.Catalogue));
                if (state.phase=="preview") MainAction("预订席间服务",ServiceSheet);
            }
        }

        private void Consign()
        {
            Hero(null);
            Title("自愿送拍",Phase(state.phase));
            Note($"每人至多送三件。{TimeAt(state.previewAt)} 截收，依品阶、等级和送达次序入选。未入选与流拍原物到交割处领回。");
            if(state.phase!="collect") { Note("本场已停止收宝，可到预展切换场次。"); return; }
            Title("纳戒可送藏品","选择一件");
            foreach(var g in state.goods??Array.Empty<Goods>())
                Tile(g.name,$"{RingLedger.Kind(g.kind)} · {WorldSetting.GetRankAllName(g.rank)}　持有 {g.count}",Art(g),"送拍",()=>SubmitSheet(g));
            if(state.goods==null || state.goods.Length==0) Note("纳戒中暂无可送拍物品。");
        }

        private void Account()
        {
            Title("交割处",$"可用 {Money(state.balance)} 灵石");
            foreach(var d in state.deliveries??Array.Empty<Delivery>())
                Tile(d.name,(d.reason=="won" ? "竞得交割" : d.reason=="passed" ? "流拍退回" : d.reason=="notSelected" ? "未入选退回" : "待领入纳戒")+"　×"+d.count,Art(d),"领取",()=>Confirm("核对交割",d.name+" ×"+d.count+" 将收入纳戒。","收取宝物",
                    ()=>StartOrder(new Order {action="claim",lotId=d.id})));
            if(state.deliveries==null || state.deliveries.Length==0) Note("暂无待领取的藏品。");
            Title("本场我的记录",TierTitles[tier]); Lots(true);
        }

        private void LotSheet(Lot lot)
        {
            lotID=lot.id;
            bool finished=lot.status=="sold" || lot.status=="passed";
            var card=Sheet(lot.name,finished ? 317 : 272); var icon=Art(lot);
            var picture=RoleFolio.Image(card,"拍品",Color.white); RoleFolio.Fixed(picture.rectTransform,0,17,55,74,76); picture.sprite=icon; picture.preserveAspect=true;
            Text(Band(card,"铭签",50,89,99,12),$"{RingLedger.Kind(lot.kind)} · {WorldSetting.GetRankAllName(lot.rank)}　×{lot.count}\n{(lot.source=="system" ? "阁藏补位" : "寄售 · "+lot.seller)}\n{Status(lot.status)}　{(lot.opensAt>0 ? TimeAt(lot.opensAt) : "")}",14,Ink);
            Text(Band(card,"报价",136,52,15,15),$"起拍 {Money(lot.start)}　当前 {Money(lot.price)}　加价 {Money(lot.step)}\n"+(lot.myMax>0 ? "我的托管上限 "+Money(lot.myMax)+" 灵石" : lot.participated ? "我的末次委托 "+Money(lot.lastBid)+" 灵石，当前冻结 0。" : "成交按落槌价交割，多缴灵石退回。"),14,Lacquer);
            bool active=lot.status=="preview" || lot.status=="live";
            if(finished) Text(Band(card,"落槌记",188,52,15,15),HammerCall(lot),14,Ink);
            Button(Band(card,"举牌",finished ? 251 : 207,44,12,12),active && !lot.mine ? "填写最高委托" : "收起宝签",()=>
            { CloseSheet(); if(active && !lot.mine) BidSheet(lot); },active && !lot.mine ? Lacquer : Ink);
        }

        private void BidSheet(Lot l)
        {
            if(pending!=null) { Feedback("请先核对上笔操作。"); return; }
            long min=l.leading ? l.myMax+1 : string.IsNullOrEmpty(l.leader) ? l.start : l.price+l.step;
            var card=Sheet("委托举牌",310);
            Text(Band(card,"宝名",46,53,15,15),$"{l.name}　当前 {Money(l.price)} 灵石\n每手 {Money(l.step)}　可用 {Money(state.balance)}",15,Ink);
            var max=Input(card,103,"最高委托额",true,min.ToString());
            Text(Band(card,"托管",153,44,15,15),"全额托管，领先时只补差额。被超越即退还，得宝按实际落槌价结算。",13,Ink);
            Text(Band(card,"委托叮嘱",193,42,15,15),Guide("proxy","先核对最高额，再盖印举牌。",l),13,Lacquer);
            Button(Band(card,"核价",246,45,12,12),"核对灵石",()=>
            {
                if (!long.TryParse(max.text,out long amount) || amount<min || amount>1000000000L || amount-l.myMax>state.balance)
                { CardError(card,"请输入可支付且达到最低要求的最高额。"); return; }
                Confirm("确认托管",l.name+"　最高 "+Money(amount)+" 灵石\n本次再缴 "+Money(amount-l.myMax)+" 灵石。","盖印举牌",
                    ()=>StartOrder(new Order {action="bid",lotId=l.id,amount=amount}));
            },Lacquer);
        }

        private void SubmitSheet(Goods g)
        {
            var card=Sheet("寄存送拍",298);
            Text(Band(card,"物件",45,35,15,15),g.name+"　持有 "+g.count,17,Ink);
            var count=Input(card,86,"拍卖数量",true,"1");
            var amount=Input(card,136,"整组起拍价",true,"100");
            Text(Band(card,"规则",184,42,15,15),"整组起拍 100 至 10 亿灵石，无报名费。成交收取 5% 货款。",13,Ink);
            Button(Band(card,"核单",237,43,12,12),"核对送拍凭单",()=>
            {
                if(!int.TryParse(count.text,out int n)||n<1||n>g.count||!long.TryParse(amount.text,out long price)||price<100||price>1000000000L)
                {CardError(card,"数量须足额，起拍价为 100 至 10 亿灵石。");return;}
                Confirm("送拍凭单",$"{g.name} ×{n}\n整组起拍 {Money(price)} 灵石。确认后从纳戒移交本场。","寄存候选",
                    ()=>StartOrder(new Order {action="submit",goodsId=g.id,count=n,amount=price}));
            },Lacquer);
        }

        private void ChatSheet(Lot l)
        {
            var s=state.seat;
            if(s==null || s.remaining<=0 || s.nextAt>Now)
            { Confirm("席间传音",s!=null && s.remaining<=0 ? "本场传音次数已用完，可添置席间服务。" : "请待传音冷却完毕。","收起",CloseSheet);return; }
            var card=Sheet("席间传音",337);
            Text(Band(card,"席牌",48,38,15,15),$"{s.room}　余 {s.remaining} 次　冷却 {s.cooldown}秒",14,Ink);
            var msg=Input(card,81,"给本件拍品留一句话",false,"",54);msg.characterLimit=60;
            msg.lineType=InputField.LineType.MultiLineSubmit;
            Text(Band(card,"快捷说明",132,24,12,12),"点选一句填入，可修改后发送",13,Ink);
            PresentLot(l,out _,out var remark);
            string offer=SeatBidRemark(l);
            Button(Band(card,"鉴宝短句",159,40,12,12),remark,()=>msg.text=remark,Ink);
            Button(Band(card,"应价短句",204,40,12,12),offer,()=>msg.text=offer,Ink);
            Button(Band(card,"发出",268,45,12,12),"发往「"+l.name+"」席间",()=>
            {
                if(string.IsNullOrWhiteSpace(msg.text)) {CardError(card,"请先写下传音。");return;}
                StartOrder(new Order {action="chat",lotId=l.id,text=msg.text.Trim()});
            },Ink);
        }

        private void ServiceSheet()
        {
            var list=state.services??Array.Empty<Service>();
            var card=Sheet("席间雅供",Mathf.Min(420,95+list.Length*70));
            long paid=list.FirstOrDefault(x=>x.id==state.seat?.service)?.price??0; int i=0;
            foreach(var s in list)
            {
                long due=s.price-paid;
                var r=Band(card,s.name,50+i++*70,63,12,12);Surface(r,Pale);
                Text(Slice(r,0,.72f,7,1),$"{s.name}　{Money(s.price)} 灵石\n{s.quota} 次 · {s.cooldown} 秒 · {Effect(s.effect)}传音",13,Ink);
                Button(Slice(r,.73f,1,2,7),due>0 ? "升席" : "已享",()=>
                {
                    if(due<=0)return;
                    Confirm("添置"+s.name,"本场补缴 "+Money(due)+" 灵石。\n席位共 "+s.quota+" 次传音，每次间隔 "+s.cooldown+" 秒。","付灵石入席",
                        ()=>StartOrder(new Order {action="service",service=s.id}));
                },due>0 ? Lacquer : Ink);
            }
        }
    }
}
