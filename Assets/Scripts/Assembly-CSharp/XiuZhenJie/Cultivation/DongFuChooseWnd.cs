using System.Collections.Generic;
using Google.Protobuf.WellKnownTypes;
using UnityEngine;
using UnityEngine.UI;
using static XiuZhenJie.CaveGradeArt;

namespace XiuZhenJie
{
    public class DongFuChooseWnd : Window
    {
        public DongFuChoosePanel _panel;
        public Dictionary<string, WorldHomeModel> MoBanData;

        private static readonly Color Night = new Color32(24, 40, 39, 255);
        private static readonly Color Paper = new Color32(246, 238, 215, 255);
        private static readonly Color Quiet = new Color32(190, 204, 185, 255);
        private Font font;
        private Text current, heroTitle, price, actionLabel;
        private readonly Text[] effectsText = new Text[4], status = new Text[4];
        private readonly Image[] plates = new Image[4];
        private Button action;
        private CaveChamberEffects effects;
        private string roleId;
        private int actual, selected, session;
        private bool busy, readFailed;
        private Sprite plaque;

        public override void Awake(params object[] paralist)
        {
            base.Awake(paralist);
            _panel = GameObject.GetComponent<DongFuChoosePanel>();
            font = _panel.TitleText.font;
            var root = (RectTransform)_panel.transform;
            foreach (Transform child in root) child.gameObject.SetActive(false);
            RoleFolio.Flat(root.GetComponent<Image>(), Night, true);
            plaque = ReleasedResources.Load<Sprite>("InkInterface/InkPlaque");

            var head = Band(root, "UpgradeLintel", 0, 42);
            Label(head, "洞府晋阶", 21, Paper, TextAnchor.MiddleCenter, true);
            var back = RoleFolio.Box(head, "ReturnFromUpgrade");
            RoleFolio.Fixed(back, 0, 3, 2, 61, 38);
            Bind(back, Leave);
            Label(back, "‹  返回", 16, Paper, TextAnchor.MiddleCenter);

            var stage = Band(root, "UpgradeMural", 42, 145);
            stage.gameObject.AddComponent<RectMask2D>();
            var picture = RoleFolio.Image(stage, "VaultSanctum", Color.white);
            picture.sprite = InkCultivationInterface.Get("VaultSanctum");
            RoleFolio.Fill(picture.rectTransform);
            if (picture.sprite != null)
            {
                var fit = picture.gameObject.AddComponent<AspectRatioFitter>();
                fit.aspectMode = AspectRatioFitter.AspectMode.EnvelopeParent;
                fit.aspectRatio = picture.sprite.rect.width / picture.sprite.rect.height;
            }
            var light = RoleFolio.Box(picture.transform, "UpgradeEffects");
            RoleFolio.Fill(light);
            effects = light.gameObject.AddComponent<CaveChamberEffects>();
            effects.raycastTarget = false;
            var seal = RoleFolio.Box(stage, "PreviewPlaque"); RoleFolio.Fixed(seal, 0, 6, 6, 124, 34);
            var silk = RoleFolio.Image(seal, "Ink", Color.white); silk.sprite = plaque; RoleFolio.Fill(silk.rectTransform);
            heroTitle = Label(seal, "", 17, Paper, TextAnchor.MiddleCenter, true);
            current = Label(Band(root, "ResidenceStatus", 190, 33, 12), "", 17, Paper, TextAnchor.MiddleLeft, true);

            for (int i = 0; i < 4; i++)
            {
                int grade = i + 1;
                var row = Band(root, "UpgradeGrade" + grade, 226 + 53 * i, 49, 7);
                var btn = Bind(row, () => { selected = grade; Draw(); effects.PlayPulse(); });
                plates[i] = btn.image;
                var ring = RoleFolio.Image(row, "GradeSeal", Color.white);
                ring.sprite = InkCultivationInterface.Get("CaveGrade" + grade);
                RoleFolio.Fixed(ring.rectTransform, 0, 4, 4, 52, 41);
                ring.preserveAspect = true;
                var name = RoleFolio.Box(row, "GradeName"); RoleFolio.Fixed(name, 0, 5, 4, 50, 41);
                Label(name, Grades[grade], 15, SealColors[grade], TextAnchor.MiddleCenter, true);
                var facts = RoleFolio.Box(row, "GradeDetail"); RoleFolio.Fill(facts, 61, 61);
                effectsText[i] = Label(facts, "", 15, Paper, TextAnchor.MiddleLeft);
                var state = RoleFolio.Box(row, "GradeStatus"); RoleFolio.Fixed(state, 1, -5, 1, 53, 47);
                status[i] = Label(state, "", 13, SealColors[grade], TextAnchor.MiddleCenter);
            }
            var footer = RoleFolio.Box(root, "Payment");
            footer.anchorMin = Vector2.zero; footer.anchorMax = Vector2.right;
            footer.offsetMin = new Vector2(8, 6); footer.offsetMax = new Vector2(-8, 79);
            price = Label(Band(footer, "Cost", 0, 27), "", 14, Quiet, TextAnchor.MiddleCenter);
            var control = Band(footer, "ConfirmUpgrade", 29, 44);
            action = Bind(control, PayOrRetry);
            actionLabel = Label(control, "", 17, Paper, TextAnchor.MiddleCenter, true);
        }

        public override void OnShow(params object[] paralist)
        {
            base.OnShow(paralist);
            roleId = DongFuResidenceState.CurrentRoleId;
            MoBanData = Singleton<FacadeManager>.Instance.SelectDongFuLevelInfo();
            selected = 1;
            Load();
        }

        private bool Active(int ticket, string role) => ticket == session && GameObject != null &&
            GameObject.activeSelf && role == DongFuResidenceState.CurrentRoleId;

        private void Load()
        {
            int ticket = ++session;
            string role = roleId;
            actual = -1;
            busy = true;
            readFailed = false;
            Draw();
            if (Singleton<NetManager>.Instance.TrySendGameRequest(MsgType.ENTER_DONGFU, null, bytes =>
            {
                if (!Active(ticket, role)) return;
                var reply = bytes == null ? null : EnterHomeResp_1005001.Parser.ParseFrom(bytes);
                busy = false;
                if (reply != null && reply.IsSuccess)
                {
                    Apply(role, reply.Rank, reply.Name);
                    selected = Mathf.Min(actual + 1, DongFuResidenceState.MaxLevel);
                }
                else readFailed = true;
                Draw();
            })) return;
            busy = false;
            readFailed = true;
            Draw();
        }

        private void Apply(string role, int rank, string name)
        {
            actual = DongFuResidenceState.ApplyRank(role, rank);
            Singleton<UIManager>.Instance.FindWndByName<DongFuRoomWnd>("DongFuRoomWnd.prefab")?.ApplyResidenceState(actual, name);
        }

        private long Cost(WorldHomeModel home)
        {
            long value = (long)(double)home.price;
            return Singleton<RoleManager>.Instance.Role_Entity.VipLevel >= 4 ? value * 9 / 10 : value;
        }

        private bool CanPay(int grade)
        {
            if (busy || actual < 0 || grade != actual + 1 || MoBanData == null ||
                !MoBanData.TryGetValue(grade.ToString(), out var home)) return false;
            var role = Singleton<RoleManager>.Instance.Role_Entity;
            return role != null && WorldSetting.GetStageByLevel(Mathf.Max(1, role.RoleLevel)) >= grade && role.ShinyStone >= Cost(home);
        }

        private void Draw()
        {
            var role = Singleton<RoleManager>.Instance.Role_Entity;
            current.text = actual < 0 ? (readFailed ? "洞府品阶读取失败" : "正在读取洞府…") :
                actual == 4 ? "当前天阶  ·  已达最高品阶" : "当前" + Grades[actual] + "  →  下一阶 " + Grades[actual + 1];
            effects.SetGrade(selected);
            heroTitle.text = Grades[selected] + " · " + Motifs[selected];
            heroTitle.color = SealColors[selected];
            for (int i = 0; i < 4; i++)
            {
                int grade = i + 1;
                string rate = MoBanData != null && MoBanData.TryGetValue(grade.ToString(), out var info)
                    ? "灵气转化 " + ((double)info.conversionRate).ToString("0%") : "";
                effectsText[i].text = "单箱 " + BoxCapacity(grade) + " 立方尺\n" + rate;
                status[i].text = actual < 0 ? "" : grade == actual ? "当前" : grade < actual ? "已达成" :
                    grade == actual + 1 ? "下一阶" : "待晋升";
                plates[i].color = grade == selected ? SealColors[grade] : new Color32(150, 174, 159, 255);
            }
            bool haveData = MoBanData != null && MoBanData.TryGetValue(selected.ToString(), out _);
            action.interactable = readFailed || CanPay(selected);
            if (!haveData || role == null || actual < 0)
            {
                price.text = "";
                actionLabel.text = readFailed ? "重新读取洞府" : !haveData ? "品阶资料未载入" : "正在读取品阶…";
                return;
            }
            var offer = MoBanData[selected.ToString()];
            long cost = Cost(offer);
            bool reached = selected <= actual;
            string minimum = WorldSetting.GetRoleLevelNameByRoleLevel(WorldSetting.GetLevelByStage(selected));
            price.text = reached ? "当前单箱容量 " + BoxCapacity(actual) + " 立方尺" : minimum + "起  ·  " + cost.ToString("N0") + " 灵石";
            actionLabel.text = busy ? "晋阶中…" : reached ? "已达" + Grades[selected] :
                selected != actual + 1 ? "需先晋升" + Grades[actual + 1] :
                WorldSetting.GetStageByLevel(Mathf.Max(1, role.RoleLevel)) < selected ? "修为需达" + minimum :
                role.ShinyStone < cost ? "尚缺 " + (cost - role.ShinyStone).ToString("N0") + " 灵石" : "晋升" + Grades[selected] + "洞府";
        }

        private void PayOrRetry()
        {
            if (readFailed) { Load(); return; }
            int grade = selected, ticket = session;
            string role = roleId;
            if (!CanPay(grade)) return;
            long cost = Cost(MoBanData[grade.ToString()]);
            MonoSingleton<GameStart>.Instance.PopUpAttention(
                "花费 " + cost.ToString("N0") + " 灵石晋升" + Grades[grade] + "洞府？",
                () => { if (Active(ticket, role) && CanPay(grade)) Buy(grade); });
        }

        private void Buy(int target)
        {
            string role = roleId;
            int ticket = session;
            busy = true;
            Draw();
            var request = new Int32Value { Value = target };
            if (Singleton<NetManager>.Instance.TrySendGameRequest(MsgType.UPGRADE_DONGFU, request, bytes =>
            {
                if (role != DongFuResidenceState.CurrentRoleId) return;
                var reply = bytes == null ? null : UpgradeHomeResp_1005002.Parser.ParseFrom(bytes);
                if (reply != null && (reply.IsSuccess || reply.Code == 4008 || reply.Code == 4105 || reply.Code == 4110 || reply.Code == 4112))
                {
                    Singleton<RoleManager>.Instance.Role_Entity.ShinyStone = reply.ShinyStone;
                    Apply(role, reply.Rank, reply.Name);
                    Singleton<UIManager>.Instance.ReFlashAllOpenedWnd();
                }
                if (!Active(ticket, role)) return;
                busy = false;
                if (reply == null) { Load(); return; }
                Draw();
                if (reply.IsSuccess) effects.PlayPulse();
                string message = reply.IsSuccess ? Grades[target] + "洞府已启用。" : reply.Code switch
                {
                    4008 => "洞府品阶已更新，请重新选择。",
                    4105 => "当前修为不足。",
                    4110 => "洞府已达到天阶。",
                    4112 => "灵石不足。",
                    _ => MonoSingleton<GameStart>.Instance.GetHintMsgByCode(reply.Code)
                };
                MonoSingleton<GameStart>.Instance.PopUpAttention(message);
            })) return;
            Load();
        }

        public override void OnDisable() { ++session; base.OnDisable(); }

        private void Leave()
        {
            Singleton<UIManager>.Instance.FindWndByName<DongFuWnd>("DongFuWnd.prefab")?.reflash();
            Singleton<UIManager>.Instance.CloseWnd(this);
        }

        private static RectTransform Band(Transform parent, string name, float top, float height, float inset = 0)
        {
            var rect = RoleFolio.Box(parent, name);
            RoleFolio.Band(rect, inset, inset, top, height);
            return rect;
        }

        private Text Label(Transform parent, string title, int size, Color ink, TextAnchor align, bool bold = false)
        {
            var text = RoleFolio.Label(parent, font, title, size, ink, align, bold);
            RoleFolio.Style(text, size, ink, align, bold, 12);
            RoleFolio.Fill(text.rectTransform, 1, 1);
            return text;
        }

        private Button Bind(RectTransform area, UnityEngine.Events.UnityAction click)
        {
            var face = area.gameObject.AddComponent<Image>();
            face.sprite = plaque;
            var button = area.gameObject.AddComponent<Button>();
            button.targetGraphic = face;
            AddButtonClickListener(button, click);
            return button;
        }
    }
}
