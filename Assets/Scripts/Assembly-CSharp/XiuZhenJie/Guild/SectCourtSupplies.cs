using System;
using UnityEngine;
using UnityEngine.UI;

namespace XiuZhenJie
{
    public partial class SheLiZongMenWnd
    {
        private const MsgType ReadSupplies=(MsgType)4901032, PrepareOrder=(MsgType)4901033;
        [Serializable] private sealed class Supplies
        {
            public long permits,fragments,sweeps,issued,revision,price,fragmentsCost;
            public long stones;
            public bool canGrant,canExchange,canBuy;
        }
        [Serializable] private sealed class Prepare { public string action; public long revision; }
        private Supplies supplies;

        private void SetSubmitText()
        {
            string text = established ? "进入山门" : !siteReady ? "重查山址" :
                supplies==null ? "重查建宗道具" : charter.IsPreparing ? "填写山门宗契" :
                supplies.permits<1 ? "筹备开山令" : "持一枚开山令立宗";
            Panel.LiZongBtn.GetComponentInChildren<Text>().text=text;
        }

        private void LoadSupplies(bool choosePage)
        {
            int turn=epoch;
            Busy(true); charter.status.text="正在打开筹建簿…";
            ForgeWire.Send(ReadSupplies,null,(ok,json) =>
            {
                if(turn!=epoch || GameObject==null) return;
                Busy(false);
                if (!ok || !ReceiveSupplies(json))
                {
                    supplies=null;
                    charter.status.text="筹建簿暂未取得，可点下方重查。";
                    charter.ResetSupplies(charter.status.text);
                    charter.SupplyLine("重查筹建簿","核对角色名下的开山令、堪舆残图与灵石。","warrant",() => LoadSupplies(true),"重查");
                }
                else
                {
                    charter.status.text="";
                    if(choosePage) charter.DisplaySupplies(supplies.permits==0);
                }
                SetSubmitText();
            });
        }

        private bool ReceiveSupplies(string json)
        {
            if(string.IsNullOrEmpty(json)) return false;
            Supplies state;
            try { state=JsonUtility.FromJson<Supplies>(json); }
            catch(ArgumentException) { return false; }
            if(state==null || state.fragmentsCost<=0 || state.price<=0) return false;
            supplies=state;
            SectCourt.SyncStones(state.stones);
            string inventory=$"开山令 {state.permits} 枚　·　堪舆残图 {state.fragments} 张";
            charter.ResetSupplies(inventory);
            charter.SupplyLine(inventory,"开山令绑定本角色。宗契立成时消耗一枚，四种门徽均可自由选择。","warrant");
            charter.SupplyLine("城主府授令",state.canGrant ? "初次开宗可在此代领首枚开山令，无需灵石。" : "本角色的首枚开山令已授。",
                "halls",state.canGrant ? () => PlaceOrder("grant") : (Action)null,"代领首枚开山令");
            charter.SupplyLine("堪舆残图合令",$"每次有效历练清场得 1 张，{state.fragmentsCost} 张可换 1 枚。现存 {state.fragments} 张，已清场 {state.sweeps} 次。",
                "survey",state.canExchange ? () => ConfirmSupply("exchange",$"消耗 {state.fragmentsCost} 张堪舆残图换取一枚开山令？") : (Action)null,"以图换令");
            charter.SupplyLine("灵石请印",$"每枚需 {state.price} 灵石，角色现有 {state.stones:N0} 灵石。",
                "merit",state.canBuy ? () => ConfirmSupply("buy",$"支付 {state.price} 灵石请一枚开山令？") : (Action)null,"支付灵石请印");
            return true;
        }

        private void ConfirmSupply(string action,string question)
        {
            if(sending || supplies==null) return;
            var observed=supplies;
            MonoSingleton<GameStart>.Instance.PopUpAttention(question,() =>
            {
                if(GameObject!=null && !sending && ReferenceEquals(observed,supplies)) PlaceOrder(action);
            },null,"开山令");
        }

        private void PlaceOrder(string action)
        {
            if(sending || supplies==null) return;
            Busy(true); int turn=epoch;
            var order=new Prepare { action=action,revision=supplies.revision };
            ForgeWire.Send(PrepareOrder,JsonUtility.ToJson(order),(ok,json) =>
            {
                if(turn!=epoch || GameObject==null) return;
                Busy(false);
                if (!ok || !ReceiveSupplies(json))
                {
                    string why="请核对筹建簿，再决定是否重试。";
                    if(!string.IsNullOrEmpty(json))
                    {
                        try { why=JsonUtility.FromJson<SectCourtState.Failure>(json)?.error ?? why; }
                        catch(ArgumentException) {}
                    }
                    MonoSingleton<GameStart>.Instance.PopUpAttention(why);
                    LoadSupplies(false);
                    return;
                }
                charter.status.text="开山令已收入筹建簿。";
                SetSubmitText();
                InkRitualEffect.Play(Panel.LiZongBtn.transform,InkRitualEffect.Mark.Resonance,RoleFolio.Jade,true);
            });
        }
    }
}
