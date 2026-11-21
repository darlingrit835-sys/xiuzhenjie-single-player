using System;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;
using static XiuZhenJie.AuctionHallState;

namespace XiuZhenJie
{
    internal sealed partial class AuctionHall : MonoBehaviour
    {
        private enum Page { Market, Catalogue, Room, Consign, Account }
        private const MsgType Wire = (MsgType)4901040;
        private static readonly string[] Tiers = { "daily","weekly","monthly" };
        private static readonly string[] TierTitles = { "每日小拍","每周大拍","每月巨拍" };
        
        private static readonly string[] TierNames = { "晚灯小集", "云阙宝会", "万象天拍" };
        private static readonly StoreType[] Shops = { StoreType.GongFa, StoreType.FaQi, StoreType.DanYao, StoreType.FuLu, StoreType.FaShu, StoreType.ZhenFa, StoreType.QiTa };
        private static readonly string[] ShopNames = { "功法", "法器", "丹药", "符箓", "法术", "阵法", "材料" };
        private static readonly string[] ShopArt = { "Manual", "Sword", "Elixir", "Talisman", "Scroll", "Compass", "Ore" };
        private bool busy, readError;
        private int tier, shop;
        private Page page;
        private Action close;
        private Window owner;
        private Font font;
        private AuctionHallState state;
        private Order pending;
        private string prefKey, notice, lotID, noticeLotID;
        private float receivedAt, pollAt, clockAt;
        private long atDeadline;
        private Text clock, receipt, seatCounter, chant;
        private int chantBand=-1;
        private string firstBidLot;
        private float firstBidUntil;
        private RectTransform root, body, hero, modal;
        private float cursor;
        private AuctionHallLights stageLight;
        private long Now => state == null ? 0 : state.now + (long)((Time.unscaledTime-receivedAt)*1000);
        private static string Money(long n) => n.ToString("N0");
        private static string TimeAt(long ms) => DateTimeOffset.FromUnixTimeMilliseconds(ms).ToOffset(TimeSpan.FromHours(8)).ToString("MM.dd HH:mm");
        private static string Phase(string p) => p=="collect" ? "收宝中" : p=="preview" ? "预展中" : p=="live" ? "正在竞拍" : "已闭场";
        private static string Status(string p) => p=="pending" ? "待遴选" : p=="preview" ? "预展" : p=="queued" ? "候拍" :
            p=="live" ? "举牌中" : p=="sold" ? "已落槌" : p=="passed" ? "流拍" : "已登记";
        private static string Effect(string s) => s=="gold" || s=="room" || s=="celestial" ? "金箔" : s=="wine" || s=="amber" ? "琥珀" :
            s=="jade" || s=="fruit" ? "青玉" : "素笺";
        private Lot LiveLot => state?.lots?.FirstOrDefault(x=>x.status=="live");
        private Lot Selected => state?.lots?.FirstOrDefault(x=>x.id==lotID);
        private Lot Featured => page==Page.Room ? LiveLot : Selected ?? LiveLot ?? state?.lots?.FirstOrDefault(x=>x.status!="pending");
        private bool Topmost => Singleton<UIManager>.Instance.IsFrontmost(owner);

        internal static AuctionHall Open(Window owner, Font font, bool market, Action close)
        {
            var host = RoleFolio.Box(owner.Transform,market ? "常驻自由市场" : "金阙拍卖会"); RoleFolio.Fill(host);
            var view = host.gameObject.AddComponent<AuctionHall>(); view.font=font; view.close=close;view.owner=owner;
            view.page=market ? Page.Market : Page.Catalogue;
            view.prefKey="auction.pending."+Singleton<RoleManager>.Instance.Role_Entity?.Id;
            if (!market && PlayerPrefs.HasKey(view.prefKey))
            {
                try { view.pending=JsonUtility.FromJson<Order>(PlayerPrefs.GetString(view.prefKey)); } catch (ArgumentException) { }
                int saved=Array.IndexOf(Tiers,view.pending?.tier);
                if (saved>=0) view.tier=saved;
            }
            view.Render(); if (!market) view.Refresh();
            return view;
        }

        private void SelectTier(int index)
        {
            if (busy || pending!=null || tier==index) return;
            tier=index; state=null; lotID=null; notice=null; readError=false; page=Page.Catalogue; Render(); Refresh();
        }

        private void Switch(Page dest)
        {
            if (busy && state==null) return;
            page=dest; notice=null;noticeLotID=null; Render();
            if (dest==Page.Room && state!=null && !busy) Refresh();
        }

        private void Refresh()
        {
            if (busy || page==Page.Market) return;
            Send(new Order { action="view", tier=Tiers[tier], viewTag=state?.tier==Tiers[tier] ? state.viewTag : null });
        }

        private void StartOrder(Order order)
        {
            if (busy || state==null || pending!=null) return;
            order.tier=state.tier; order.eventId=state.eventId;
            order.requestId=Guid.NewGuid().ToString("N"); pending=order;
            PlayerPrefs.SetString(prefKey,JsonUtility.ToJson(order)); PlayerPrefs.Save();
            CloseSheet(); Send(order);
        }

        private void ForgetOrder()
        {
            pending=null; PlayerPrefs.DeleteKey(prefKey); PlayerPrefs.Save();
        }

        private void Send(Order order)
        {
            if (busy) return;
            bool write=order.action!="view";
            busy=true;
            if (write) {noticeLotID=null;Feedback("正在核对玉印…");}
            ForgeWire.Send(Wire,JsonUtility.ToJson(order),(ok,json) =>
            {
                if (this==null) return;
                busy=false; pollAt=Time.unscaledTime+3f;
                if (!ok)
                {
                    Failure failure=null;
                    if (!string.IsNullOrEmpty(json))
                        try { failure=JsonUtility.FromJson<Failure>(json); } catch (ArgumentException) { }
                    string error=failure?.error;
                    if (write && error!=null && !failure.retry) ForgetOrder();
                    readError=true;
                    notice=error ?? (write ? "回执尚未抵达，请点「核对上笔」重取原单。" : "暂未取得拍卖簿，可点右上角刷新。");
                    Render(true); return;
                }
                AuctionHallState next=null;
                try { next=JsonUtility.FromJson<AuctionHallState>(json); } catch (ArgumentException) { }
                if (next==null || string.IsNullOrEmpty(next.eventId) || next.tier!=order.tier)
                {
                    readError=true; notice="拍卖簿回执不完整，请重新核对。"; Render(true); return;
                }
                if (!write && next.unchanged)
                {
                    if (state==null || state.eventId!=next.eventId || state.viewTag!=next.viewTag)
                    {
                        if (state!=null) state.viewTag=null;
                        readError=true; notice="本场簿册已换，请刷新重读。";Render(true);return;
                    }
                    state.now=next.now;receivedAt=Time.unscaledTime;
                    if (readError) { readError=false;notice=null;Render(true); }
                    return;
                }
                var before=state; var priorLive=LiveLot;
                var lastMessage=before?.messages?.LastOrDefault()?.id;
                var oldSignature=Signature(before);
                bool recovered=readError; readError=false; state=next; receivedAt=Time.unscaledTime;
                var role=Singleton<RoleManager>.Instance.Role_Entity;
                if (role!=null && role.ShinyStone!=state.balance)
                {
                    role.ShinyStone=state.balance;
                    Singleton<UIManager>.Instance.FindWndByName<MainTitleWnd>("MainTitleWnd.prefab")?.ReFlashData();
                }
                if (write)
                {
                    ForgetOrder();
                    if ((order.action=="submit" || order.action=="claim") && Singleton<RoleManager>.Instance.RoleBoxDic.TryGetValue(10,out var bag))
                        bag.GetAllGoods(null,false);
                    noticeLotID=order.action=="bid" || order.action=="chat" ? order.lotId : null;
                    notice=order.action=="bid" ? (SelectedBidWon(order) ? "玉牌已应，当前由你领价。" : "此价已被其他道友的委托超越。") :
                        order.action=="submit" ? "送拍凭单已入册，可在「我的」查看。" :
                        order.action=="service" ? "本场席位已更新。" : order.action=="chat" ? "传音已送达。" : "交割完成，宝物已入纳戒。";
                }
                else if (recovered) notice=null;
                var finished=priorLive==null ? null : (state.lots??Array.Empty<Lot>()).FirstOrDefault(x=>x.id==priorLive.id && (x.status=="sold" || x.status=="passed"));
                var current=LiveLot;
                if (page==Page.Room && noticeLotID!=null && noticeLotID!=current?.id && pending==null)
                {
                    notice=null;noticeLotID=null;
                }
                if (!write && current!=null && current.id==priorLive?.id && priorLive.leading && !current.leading)
                {
                    noticeLotID=current.id;notice=$"「{current.name}」已被超价，原委托灵石已退回。";
                }
                bool raised=current!=null && current.id==priorLive?.id && (current.price>priorLive.price || current.leader!=priorLive.leader);
                if (raised && current.price==current.start && string.IsNullOrEmpty(priorLive.leader) && !string.IsNullOrEmpty(current.leader))
                {
                    firstBidLot=current.id;
                    firstBidUntil=Time.unscaledTime+Mathf.Clamp((current.endsAt-next.now)/2000f,.3f,3.3f);
                }
                else if (current?.id!=firstBidLot || raised) firstBidLot=null;
                bool redraw=write || recovered || oldSignature!=Signature(next);
                if (redraw) Render(true);
                if (page==Page.Room && stageLight!=null)
                {
                    if (finished!=null) CloseLotToast(finished);
                    else if (raised || write && order.action=="bid") stageLight.Burst("应  价",false);
                    var messages=(next.messages??Array.Empty<Message>()).Where(m=>m.lotId==LiveLot?.id).ToArray();
                    int after=Array.FindIndex(messages,m=>m.id==lastMessage);
                    int start=before==null ? messages.Length : Math.Max(after+1,messages.Length-3);
                    for (int i=start; i<messages.Length; i++) stageLight.Speak(messages[i],i-start);
                }
                if (write && order.action=="claim")
                {
                    var item=before?.deliveries?.FirstOrDefault(x=>x.id==order.lotId);
                    ReceiptSheet(item);
                }
            });
        }

        private bool SelectedBidWon(Order order) => (state.lots??Array.Empty<Lot>()).Any(x=>x.id==order.lotId && x.leading);

        // Ignore the moving server clock when comparing snapshots. A quiet room keeps its
        // scroll position, focused controls and local animation without rebuilding every poll.
        private static string Signature(AuctionHallState s)
        {
            if (s==null) return null;
            if (!string.IsNullOrEmpty(s.viewTag)) return s.viewTag;
            long now=s.now; s.now=0; string signature=JsonUtility.ToJson(s); s.now=now;
            return signature;
        }

        private void Update()
        {
            if (state==null || Time.unscaledTime<clockAt) return;
            clockAt=Time.unscaledTime+.25f;
            if (clock!=null)
            {
                long sec=Math.Max(0,(atDeadline-Now+999)/1000); var t=TimeSpan.FromSeconds(sec);
                string next=state.phase=="collect" ? "距截收" : state.phase=="preview" ? "距开槌" : state.phase=="closed" ? "距再开市" : "距落槌";
                clock.text=sec==0 ? "正在换场" : t.Days>0 ? $"{next}  {t.Days}日 {t.Hours}时" : $"{next}  {(int)t.TotalHours:D2}:{t.Minutes:D2}:{t.Seconds:D2}";
            }
            if (chant!=null && LiveLot is Lot onStage)
            {
                int band=ChantBand(onStage);
                if (band!=chantBand) {chantBand=band;chant.text=Caller(onStage);}
            }
            if (seatCounter!=null && state.seat!=null)
            {
                var seat=state.seat; long seconds=Math.Max(0,(seat.nextAt-Now+999)/1000);
                seatCounter.text=$"{seat.room}　余 {seat.remaining} 次　"+(seconds>0 ? $"{seconds} 秒后可发言" : $"每 {seat.cooldown} 秒可发言");
            }
            if (!Topmost || busy || readError || modal!=null || Time.unscaledTime<pollAt) return;
            bool live=page==Page.Room && state.phase=="live";
            // Read a new snapshot once at a phase boundary. Quiet catalogues never run
            // a continuous polling loop. A failed read leaves the manual refresh available.
            bool boundary=atDeadline>state.now && Now>=atDeadline;
            if (live || boundary) Refresh();
        }

        private void Feedback(string msg)
        {
            notice=msg;
            if (receipt!=null) receipt.text=msg;
        }
    }
}
