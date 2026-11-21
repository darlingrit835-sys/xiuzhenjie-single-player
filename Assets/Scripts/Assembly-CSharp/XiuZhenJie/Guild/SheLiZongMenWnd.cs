using Google.Protobuf;
using UnityEngine.UI;

namespace XiuZhenJie
{
    public partial class SheLiZongMenWnd : Window
    {
        public SheLiZongMenPanel Panel;
        private SectCharter charter;
        private Toggle[] seals;
        private int selected, epoch;
        private string site;
        private bool siteReady, sending, established;
        private static readonly string[] Suffixes = { "派","门","宗","帮","教","宫" };

        public override void Awake(params object[] args)
        {
            base.Awake(args);
            Panel = GameObject.GetComponent<SheLiZongMenPanel>();
            charter = new SectCharter(Panel);
            charter.onPageChanged=SetSubmitText;
            seals = new[] { Panel.RiYaoLingPaiTog, Panel.YueHuiLingPaiTog, Panel.XingGuangLingPaiTog, Panel.XuanTieLingPaiTog };
            AddButtonClickListener(Panel.CloseBtn,CloseWindow);
            AddButtonClickListener(Panel.LiZongBtn,Submit);
            for (int i=0; i<seals.Length; i++)
            {
                int index=i;
                AddToggleClickListener(seals[i],on => { if (on) { selected=index; Stamp(); } });
            }
            Panel.ZongMenJianChengInput.onValueChanged.AddListener(OnShortName);
            Panel.ZongMenLastName.onValueChanged.AddListener(OnSuffix);
        }

        public override void OnShow(params object[] args)
        {
            base.OnShow(args);
            if (sending || args.Length == 0) return;
            site = args[0]?.ToString();
            epoch++; siteReady=false; established=false; supplies=null;
            charter.DisplaySupplies(false); charter.ResetSupplies("正在核对开山令…");
            selected=0; seals[0].isOn=true; Panel.ZongMenLastName.value=0;
            Panel.ZongMenMingChenInput.text=""; Panel.ZongMenJianChengInput.text=""; Stamp();
            Panel.ZongMenDiDianText.text="核对所选山址…";
            Panel.LingFengShuLiangText.text="灵峰 —"; Panel.LingShiKuangMai.text="灵石脉 —"; Panel.LingYuKuangMai.text="灵玉脉 —";
            LoadSite();
        }

        private void Stamp() => charter.Stamp(Panel.ZongMenJianChengInput.text,Suffixes[Panel.ZongMenLastName.value],selected);
        private void OnShortName(string _) => Stamp();
        private void OnSuffix(int _) => Stamp();

        private void Busy(bool value)
        {
            sending=value;
            Panel.LiZongBtn.interactable=!value;
            Panel.CloseBtn.interactable=!value || !siteReady;
        }

        private void LoadSite()
        {
            int turn=epoch;
            Busy(true); charter.status.text="正在核对山址…";
            var req = new GoodsGuildMapDetailsReq_1004038 { Id=site ?? "" };
            void Reply(ByteString bytes)
            {
                if (turn!=epoch || GameObject==null) return;
                Busy(false);
                var reply=bytes==null ? null : GoodsGuildMapDetailsReap_1004038.Parser.ParseFrom(bytes);
                siteReady=reply!=null && reply.IsSuccess;
                if (!siteReady)
                {
                    charter.status.text="山址暂未确认。点下方重查，或返回舆图另选本郡空地。";
                    Panel.ZongMenDiDianText.text="尚未核定";
                    Panel.LiZongBtn.GetComponentInChildren<Text>().text="重查山址";
                    return;
                }
                Panel.ZongMenDiDianText.text=reply.DaLuName+" / "+reply.ZhouName+" / "+reply.YeWaiName;
                Panel.LingFengShuLiangText.text="灵峰 "+reply.HillCount+" 座";
                Panel.LingShiKuangMai.text="灵石脉 "+reply.StoneCount+" 座";
                Panel.LingYuKuangMai.text="灵玉脉 "+reply.JadeCount+" 座";
                LoadSupplies(true);
            }
            if (!Singleton<NetManager>.Instance.TrySendGameRequest(MsgType.GOODS_GUILDMAP_DETAILS,req,Reply)) Reply(null);
        }

        private static bool ValidName(string text,int minimum,int maximum)
        {
            int count=0;
            for (int i=0;i<text.Length;i++)
            {
                if (!char.IsLetterOrDigit(text,i)) return false;
                if (char.IsSurrogatePair(text,i)) i++;
                count++;
            }
            return count>=minimum && count<=maximum;
        }

        private void Submit()
        {
            if (sending) return;
            if (!siteReady) { LoadSite(); return; }
            if (established) { Enter(); return; }
            if (supplies==null) { LoadSupplies(true); return; }
            if (charter.IsPreparing) { charter.DisplaySupplies(false); return; }
            if (supplies.permits<1) { charter.DisplaySupplies(true); return; }
            string name=Panel.ZongMenMingChenInput.text.Trim(), shortName=Panel.ZongMenJianChengInput.text.Trim();
            if (!ValidName(name,3,5) || !ValidName(shortName,1,2))
            {
                const string message="宗名需 3 至 5 字，简称需 1 至 2 字，可用汉字、字母或数字。";
                charter.status.text=message;
                MonoSingleton<GameStart>.Instance.PopRollingTextShow(message);
                return;
            }
            var req=new GuildCreateReq_1015007 { Name=name, SimpleName=shortName, TokenIndex=selected+1, LastNameIndex=Panel.ZongMenLastName.value };
            int turn=epoch;
            Busy(true); charter.status.text="正在立宗…";
            void Reply(ByteString bytes)
            {
                if (turn!=epoch || GameObject==null) return;
                Busy(false);
                if (bytes==null)
                {
                    MonoSingleton<GameStart>.Instance.PopUpAttention("立宗回执尚未收到，可用相同宗契重试。");
                    return;
                }
                var result=GuildCreateResp_1015007.Parser.ParseFrom(bytes);
                if (!result.IsSuccess)
                {
                    string why=result.Code==4008 ? "请核对开山令、宗名与山址，重名或已被占用的地块无法立宗。" : MonoSingleton<GameStart>.Instance.GetHintMsgByCode(result.Code);
                    charter.status.text=why; MonoSingleton<GameStart>.Instance.PopUpAttention(why); LoadSupplies(false); return;
                }
                var role=Singleton<RoleManager>.Instance.Role_Entity;
                role.ShinyStone=result.ShinyStone; role.ShinyJade=result.ShinyJade;
                role.ZongMenId=result.GuildId.ToString(); role.ZongMenName=req.Name;
                role.ZongMenSimpleName=req.SimpleName; role.ZongMenIconIndex=req.TokenIndex;
                role.ZongMenLastNameImgIndex=req.LastNameIndex;
                established=true;
                Panel.LiZongBtn.GetComponentInChildren<Text>().text="进入山门";
                Singleton<ChatManager>.Instance.JoinZongMenChatServer(role.ZongMenId);
                Enter();
            }
            if (!Singleton<NetManager>.Instance.TrySendGameRequest(MsgType.GUILD_CREATE,req,Reply)) Reply(null);
        }

        private void Enter()
        {
            int turn=epoch;
            Busy(true); charter.status.text="山门已立，正在入山…";
            void Reply(ByteString bytes)
            {
                if (turn!=epoch || GameObject==null) return;
                Busy(false);
                var reply=bytes==null ? null : GuildEnterResp_1015005.Parser.ParseFrom(bytes);
                if (reply==null || !reply.IsSuccess)
                { charter.status.text="山门已立，可重试进入宗门。"; return; }
                var role=Singleton<RoleManager>.Instance.Role_Entity;
                Singleton<GameMapManager>.Instance.LoadScene("LoadingWnd.prefab","ZongMenScene",role.ZongMenId,role.ZongMenName,reply);
            }
            if (!Singleton<NetManager>.Instance.TrySendGameRequest(MsgType.GUILD_DENTER,null,Reply)) Reply(null);
        }

        public override void OnClose()
        {
            epoch++; sending=false;
            Panel.ZongMenJianChengInput.onValueChanged.RemoveListener(OnShortName);
            Panel.ZongMenLastName.onValueChanged.RemoveListener(OnSuffix);
            base.OnClose();
        }
    }
}
