using System.Collections.Generic;
using System.Linq;
using Google.Protobuf;
using UnityEngine;
using static XiuZhenJie.RoleFolio;
using Entry = OpenTianShuBeiResp_1016004.Types.TianShuBeiObjResp_1016005;

namespace XiuZhenJie
{
    public class CZF_TSBContentPanel : MonoBehaviour
    {
        public RectTransform Content;
        private QuestHallView view;
        private ChengZhuFuMainWnd hall;
        private readonly List<Entry> entries = new List<Entry>();
        private int requestVersion;
        private int claiming;

        internal void Initialize(ChengZhuFuMainWnd owner, QuestHallView layout)
        {
            hall = owner;
            view = layout;
        }

        internal void RefreshOnUnlock()
        {
            if (hall != null && hall.IsPage(2) && claiming == 0) Open();
        }

        internal void Open(string message = null)
        {
            int revision = hall.Revision;
            int version = ++requestVersion;
            var role = Singleton<RoleManager>.Instance.Role_Entity;
            view.Clear();
            view.Line(view.Content, "正在读取天书碑…");
            ChengZhuFuMainWnd.Request(MsgType.OPEN_TIANSHUBEI, null, bytes =>
            {
                if (!hall.IsPage(2) || revision != hall.Revision || version != requestVersion
                    || role != Singleton<RoleManager>.Instance.Role_Entity) return;
                var reply = bytes == null ? null : OpenTianShuBeiResp_1016004.Parser.ParseFrom(bytes);
                if (reply == null || !reply.IsSuccess)
                {
                    view.Clear();
                    view.Empty("暂时无法读取天书碑", "请稍后重新加载。", () => Open(message));
                    return;
                }
                entries.Clear();
                entries.AddRange(reply.TianShuBeiObj);
                if (role != null) UpdateBadge(role, entries.Exists(entry => entry.Status == 1));
                Draw(message);
            });
        }

        private static (string category, string condition, string progress) Describe(Entry entry)
        {
            int target = entry.MaxVal;
            string progress = $"{Mathf.Clamp(entry.CurrentVal, 0, target):N0} / {target:N0}";
            switch (entry.Type)
            {
                case 1: return ("寻宝", $"历练累计获得 {target:N0} 灵石", progress);
                case 2: return ("道友", $"曾同时拥有 {target} 位道友", progress);
                case 3: return ("参悟", $"参悟 {target} 本书籍", progress);
                case 4: return ("洞府", $"练功房灵气效率达到 {target}", progress);
                case 5: return ("除妖", $"累计击杀 {target:N0} 只妖兽", progress);
                case 6: return ("奇遇", $"完成 {target} 次随机事件", progress);
                case 7: return ("境界", $"突破至 {Realm(target)}", $"{Realm(entry.CurrentVal)} / {Realm(target)}");
                case 8: return ("历练", $"累计通关 {target} 次历练", progress);
                case 9: return ("斩首", $"累计击败 {target} 位妖兽领主", progress);
                case 10: return ("签到", $"累计签到 {target} 天", progress);
                case 11: return ("洞府", $"将洞府升至 {target} 级", progress);
                default: return ("修行", "修行记录", progress);
            }
        }

        private static string Realm(int level) => level < 1 ? "未入道" : WorldSetting.GetRoleLevelNameByRoleLevel(level);

        private void Draw(string message = null)
        {
            view.Clear();
            if (entries.Count == 0)
            {
                view.Empty("天书碑暂无记录", "新的修行记录会在这里显示。");
                return;
            }
            int ready = entries.Count(entry => entry.Status == 1);
            int claimed = entries.Count(entry => entry.Status == 2);
            var summary = view.CardRow("成就总览");
            view.Line(summary, $"已达成 {ready + claimed} / {entries.Count} 项", 20, true);
            view.Line(summary, $"待领取 {ready} 项    已领取 {claimed} 项", 14, false, true);
            if (!string.IsNullOrEmpty(message)) view.Line(summary, message);
            foreach (var entry in entries.OrderBy(entry => entry.Status == 1 ? 0 : entry.Status == 2 ? 2 : 1))
            {
                var row = view.CardRow("天书碑记录");
                var (category, condition, progress) = Describe(entry);
                string state = entry.Status == 1 ? "可领取" : entry.Status == 2 ? "已领取" : "进行中";
                view.Line(row, category + "  ·  " + state, 13, true, entry.Status != 0);
                view.Line(row, condition, 17, true);
                if (entry.MaxVal > 0)
                {
                    view.Line(row, progress, 13);
                    var track = Image(row, "进度条", Rule).rectTransform;
                    Height(track, 4);
                    float fraction = entry.Status == 2 ? 1f : Mathf.Clamp01((float)entry.CurrentVal / entry.MaxVal);
                    Span(Image(track, "当前进度", Jade).rectTransform, 0, fraction);
                }
                if (entry.Status == 1)
                    view.Action(row, claiming == entry.Id ? "正在领取…" : "领取奖励", () => Claim(entry), claiming == 0);
            }
        }

        private void Claim(Entry entry)
        {
            if (claiming != 0 || entry.Status != 1) return;
            claiming = entry.Id;
            var role = Singleton<RoleManager>.Instance.Role_Entity;
            Draw();
            var request = new TianShuBeiReceiveAwardReq_1016006 { Id = entry.Id, Type = entry.Type };
            ChengZhuFuMainWnd.Request(MsgType.TIANSHUBEI_RECEIVE_AWARD, request, bytes =>
            {
                claiming = 0;
                if (role != Singleton<RoleManager>.Instance.Role_Entity) return;
                var reply = bytes == null ? null : TianShuBeiReceiveAwardResp_1016006.Parser.ParseFrom(bytes);
                bool success = reply != null && reply.IsSuccess && reply.Id == entry.Id && reply.Status == 2;
                if (success && role != null) ApplyReceipt(bytes, role);
                if (hall.IsPage(2))
                    Open(success ? "奖励已领取" : "暂未确认领取结果，请查看刷新后的状态。");
            });
        }

        private static void ApplyReceipt(ByteString bytes, RoleInfoEntity role)
        {
            // Go appends the wallet total and badge to the existing protobuf reply.
            var input = bytes.CreateCodedInput();
            uint tag;
            while ((tag = input.ReadTag()) != 0)
            {
                switch (tag)
                {
                    case 48:
                        role.ShinyStone = input.ReadInt64();
                        Singleton<UIManager>.Instance.FindWndByName<MainTitleWnd>("MainTitleWnd.prefab")?.ReFlashData();
                        break;
                    case 56:
                        UpdateBadge(role, input.ReadBool());
                        break;
                    default:
                        input.SkipLastField();
                        break;
                }
            }
        }

        private static void UpdateBadge(RoleInfoEntity role, bool hasUnclaimed)
        {
            role.TianShuBeiStatus = hasUnclaimed ? 1 : 0;
            Singleton<UIManager>.Instance.FindWndByName<MainCityWnd>("MainCityWnd.prefab")?.ReflashMissionStatus();
        }
    }
}
