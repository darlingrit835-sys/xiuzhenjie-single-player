using System;
using Google.Protobuf;
using UnityEngine;
using UnityEngine.UI;

namespace XiuZhenJie
{
    internal sealed class CultivationRouteSheet : MonoBehaviour
    {
        private static readonly string[] Routes = { "", "炼神", "锻体", "纳气", "纳气 · 锻体", "锻体 · 炼神", "纳气 · 炼神" };
        private static readonly int[] Masks = { 0, 1, 2, 4, 6, 3, 5 };
        private static readonly string[] Attributes = { "", "神识", "体魄", "修为" };
        private const string ViewName = "CultivationRouteSheet";
        private readonly Button[] choices = new Button[7];
        private readonly Text[] choiceLabels = new Text[7];
        private readonly string[] equippedNames = new string[4];
        private readonly int[] elements = new int[4];
        private DongFuWnd room;
        private string roleId;
        private Font font;
        private Text title, details, effects, applyLabel;
        private Button apply;
        private int active, selected, revision;
        private bool ready, sending;
        private EnterExerciseRoomResp_1005008 preview;

        internal static void Attach(DongFuWnd room)
        {
            DongFuPanel panel = room.Panel;
            var entry = RoleFolio.Image(panel.transform, "CultivationRouteEntry", Color.white, true);
            RoleFolio.Fixed(entry.rectTransform, .5f, 0, 34, 86, 29);
            InkInterfaceArt.Set(entry, "IvoryPlaque", false);
            var label = RoleFolio.Label(entry.transform, panel.DongFuNameText.font, "周天推演", 14, RoleFolio.Ink, TextAnchor.MiddleCenter);
            RoleFolio.Fill(label.rectTransform);
            var button = entry.gameObject.AddComponent<Button>();
            button.targetGraphic = entry;
            button.onClick.AddListener(() => {
                if (!room.PracticeRoomLoaded || panel.transform.Find(ViewName) != null) return;
                var shade = RoleFolio.Image(panel.transform, ViewName, new Color32(21, 38, 34, 194), true);
                RoleFolio.Fill(shade.rectTransform);
                var view = shade.gameObject.AddComponent<CultivationRouteSheet>();
                view.Open(room);
            });
        }

        private void Open(DongFuWnd owner)
        {
            room = owner;
            roleId = DongFuResidenceState.CurrentRoleId;
            active = (int)room.PracticeRunning;
            font = room.Panel.DongFuNameText.font;
            var paper = RoleFolio.Image(transform, "行功谱", RoleFolio.Paper, true);
            Area(paper.rectTransform, .035f, .06f, .965f, .94f);
            var p = paper.transform;
            var banner = RoleFolio.Image(p, "题头", RoleFolio.Jade);
            Area(banner.rectTransform, 0, .78f, 1, 1);
            var ink = RoleFolio.Image(banner.transform, "洞天", Color.white);
            InkInterfaceArt.Set(ink, "InkLandscapePaper", false);
            RoleFolio.Fill(ink.rectTransform);
            var glaze = RoleFolio.Image(banner.transform, "墨色", new Color32(25, 49, 43, 214));
            RoleFolio.Fill(glaze.rectTransform);
            Label(p, "洞府  /  练功房", 13, RoleFolio.Paper, .065f, .908f, .7f, .975f);
            Label(p, "周天推演", 29, Color.white, .06f, .80f, .72f, .92f);
            var close = Control(p, "返回", .76f, .86f, .95f, .955f, out _);
            close.onClick.AddListener(() => { if (!sending) Destroy(gameObject); });

            bool dual = Singleton<RoleManager>.Instance.Role_Entity.RoleLevel >= 31;
            Label(p, "单修", 15, RoleFolio.Jade, .06f, .699f, .44f, .765f);
            Label(p, dual ? "双修" : "双修 · 元婴开启", 15, RoleFolio.Jade, .06f, .53f, .94f, .60f);
            // Display the cultivation goal first while retaining the server's original route IDs.
            int[] order = { 3, 2, 1, 4, 5, 6 };
            for (int i = 0; i < order.Length; i++) {
                int route = order[i];
                float x = .055f + .30f * (i % 3);
                float y = i < 3 ? .61f : .44f;
                choices[route] = Control(p, Routes[route], x, y, x + .284f, y + .081f, out choiceLabels[route]);
                choices[route].interactable = route <= 3 || dual;
                choices[route].onClick.AddListener(() => Select(route));
            }
            var rule = RoleFolio.Image(p, "分隔", RoleFolio.Rule);
            Area(rule.rectTransform, .055f, .412f, .945f, .415f);
            title = Label(p, "", 17, RoleFolio.Jade, .06f, .348f, .94f, .409f);
            details = Label(p, "", 15, RoleFolio.Ink, .06f, .239f, .94f, .344f);
            effects = Label(p, "", 13, RoleFolio.Secondary, .06f, .133f, .94f, .236f);
            apply = Control(p, "", .055f, .035f, .945f, .122f, out applyLabel);
            apply.image.color = RoleFolio.Jade;
            applyLabel.color = Color.white;
            apply.onClick.AddListener(Apply);
            Select(active >= 1 && active <= 6 ? active : 3);
            for (int kind = 1; kind <= 3; kind++) LoadMethod(kind);
        }

        private void LoadMethod(int kind)
        {
            var request = new ChangeAchievementPanelReq_1005009 { AchievementType = kind };
            Singleton<NetManager>.Instance.TrySendGameRequest(MsgType.CHANGE_ACHIEVEMENT_PANEL, request, bytes => {
                if (!Alive || bytes == null) return;
                var result = ChangeAchievementPanelResp_1005009.Parser.ParseFrom(bytes);
                if (!result.IsSuccess || result.CurrentAchievement == null) return;
                equippedNames[kind] = result.CurrentAchievement.Name;
                elements[kind] = result.CurrentAchievement.FiveElement;
                if (ready) ShowProjection();
            });
        }

        private bool Alive => this != null && gameObject.activeInHierarchy && roleId == DongFuResidenceState.CurrentRoleId;

        private void Select(int route)
        {
            if (sending) return;
            selected = route;
            ready = false;
            preview = null;
            int version = ++revision;
            for (int i = 1; i < choices.Length; i++) {
                bool marked = i == route;
                choices[i].image.color = marked ? RoleFolio.Jade : RoleFolio.Card;
                choiceLabels[i].color = marked ? Color.white : choices[i].interactable ? RoleFolio.Ink : RoleFolio.Secondary;
            }
            title.text = Routes[route] + "  /  每周天所得";
            details.text = "推演中…";
            effects.text = "";
            apply.interactable = false;
            applyLabel.text = route == active ? "正在运行" : "按此行功";
            var req = new ChangeRunLocationReq_1001007 { NewLocation = route };
            bool queued = Singleton<NetManager>.Instance.TrySendGameRequest(MsgType.ENTER_EXERCISE_ROOM, req, bytes => {
                if (!Alive || version != revision) return;
                if (bytes == null) { details.text = "推演超时，点选路线可重试。"; return; }
                var reply = EnterExerciseRoomResp_1005008.Parser.ParseFrom(bytes);
                if (!reply.IsSuccess || reply.Code != 2000 || reply.LocationType != route) {
                    details.text = reply.IsSuccess ? "此服务器尚未开放推演。请在练功房切换功法。" : "这条路线尚未开放。";
                    return;
                }
                preview = reply;
                ready = true;
                ShowProjection();
                apply.interactable = selected != active;
            });
            if (!queued) details.text = "连接已断开，重连后可继续推演。";
        }

        internal static string BreakthroughTrainingNote()
        {
            var role = Singleton<RoleManager>.Instance.Role_Entity;
            int level = role.RoleLevel;
            if (level < 20 || level >= 90 || level % 10 != 0)
                return "每大境十层查验神识和体魄，筑基十层起生效。";
            var levels = Singleton<FacadeManager>.Instance.SelectRoleLevelInfo();
            if (levels == null || !levels.TryGetValue(level.ToString(), out var row))
                return "本次破境需同时提升神识和体魄。";
            int percent = 100 + 2 * (level / 10 - 1);
            return $"本次破境需神识 {role.Perception}/{Math.Ceiling(row.perception * percent / 100):0}、体魄 {role.Physique}/{Math.Ceiling(row.toughTenacity * percent / 100):0}。";
        }

        private static string TrainingEstimate(double current, double target, double perCircle)
        {
            if (current >= target) return "已满";
            if (perCircle <= 0) return "暂停";
            double minutes = Math.Ceiling((target - current) / perCircle) / 30;
            return minutes < 60 ? $"{Math.Ceiling(minutes):0}分" : $"{minutes/60:0.#}时";
        }

        private void ShowProjection()
        {
            string[] names = { "", preview.PerceptionName, preview.PhysiqueName, preview.SuperairName };
            double[] gain = { 0, preview.PerceptionCircle * .5, preview.PhysiqueCircle * .5, preview.SuperairCircle };
            var role = Singleton<RoleManager>.Instance.Role_Entity;
            bool[] roots = { false, role.JinLingGen, role.MuLingGen, role.ShuiLingGen, role.HuoLingGen, role.TuLingGen };
            int[] generates = { 0, 3, 4, 2, 5, 1 };
            var lines = new System.Collections.Generic.List<string>();
            var notes = new System.Collections.Generic.List<string>();
            int mask = Masks[selected];
            var levels = Singleton<FacadeManager>.Instance.SelectRoleLevelInfo();
            RoleLevelModel row = null;
            levels?.TryGetValue(role.RoleLevel.ToString(), out row);
            for (int kind = 3; kind >= 1; kind--) {
                if ((mask & (1 << (kind-1))) == 0) continue;
                int e = names[kind] == equippedNames[kind] ? elements[kind] : 0;
                bool known = e >= 1 && e <= 5;
                string mark = known ? "金木水火土"[e-1] + " · " : "";
                double target = row == null ? 0 : kind == 1 ? Math.Round(row.perception * 1.4, MidpointRounding.AwayFromZero) :
                    kind == 2 ? Math.Round(row.toughTenacity * 1.4, MidpointRounding.AwayFromZero) : row.upgradeLimit;
                double current = kind == 1 ? role.Perception : kind == 2 ? role.Physique : role.SuperAir;
                string wait = target <= 0 ? "" : " · " + TrainingEstimate(current, target, gain[kind]);
                if (kind == 3 && role.RoleLevel >= 90) wait = " · 封顶";
                lines.Add(mark + Attributes[kind] + " +" + gain[kind].ToString("0.##") + wait);
                if (!known) continue;
                if (roots[e]) notes.Add(Attributes[kind] + "同根 ×1.2");
                for (int other = 1; other <= 3; other++) {
                    int from = elements[other];
                    if (other != kind && (mask & (1 << (other-1))) != 0 && equippedNames[other] == names[other] &&
                        from >= 1 && from <= 5 && generates[from] == e)
                        notes.Add("金木水火土"[from-1] + "生" + "金木水火土"[e-1] + " ×1.2");
                }
            }
            details.text = string.Join("\n", lines);
            string baseNote = selected <= 3 ? "单修全效" : "双修各七成";
            effects.text = baseNote + (notes.Count > 0 ? "  /  " + string.Join(" · ", notes) : "") +
                           "\n" + BreakthroughTrainingNote();
        }

        private void Apply()
        {
            if (!ready || sending || selected == active) return;
            sending = true;
            apply.interactable = false;
            applyLabel.text = "起势中…";
            bool queued = Singleton<NetManager>.Instance.TrySendGameRequest(MsgType.ROLE_CHANGE_LOCATION,
                new ChangeRunLocationReq_1001007 { NewLocation = selected }, bytes => {
                    if (!Alive) return;
                    sending = false;
                    if (bytes == null) {
                        // The change may already have been committed. Reload instead of assuming the old route.
                        Destroy(gameObject);
                        room.OnShow();
                        return;
                    }
                    var reply = ActionBoolResp_1001001.Parser.ParseFrom(bytes);
                    if (!reply.IsSuccess) {
                        ready = false;
                        applyLabel.text = "请重新推演";
                        details.text = "未能运行，点选路线可重新推演。";
                        return;
                    }
                    Destroy(gameObject);
                    room.OnShow();
                });
            if (!queued) {
                sending = false;
                applyLabel.text = "重试行功";
                details.text = "连接已断开。";
                apply.interactable = true;
            }
        }

        private static void Area(RectTransform rect, float left, float bottom, float right, float top)
        {
            rect.anchorMin = new Vector2(left, bottom);
            rect.anchorMax = new Vector2(right, top);
            rect.offsetMin = rect.offsetMax = Vector2.zero;
        }

        private Text Label(Transform parent, string text, int size, Color color, float l, float b, float r, float t)
        {
            var label = RoleFolio.Label(parent, font, text, size, color);
            Area(label.rectTransform, l, b, r, t);
            return label;
        }

        private Button Control(Transform parent, string caption, float l, float b, float r, float t, out Text label)
        {
            var image = RoleFolio.Image(parent, caption, RoleFolio.Card, true);
            Area(image.rectTransform, l, b, r, t);
            label = RoleFolio.Label(image.transform, font, caption, 14, RoleFolio.Ink, TextAnchor.MiddleCenter);
            RoleFolio.Fill(label.rectTransform);
            var button = image.gameObject.AddComponent<Button>();
            button.targetGraphic = image;
            return button;
        }
    }
}
