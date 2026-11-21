using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace XiuZhenJie
{
    internal sealed class EquipmentVestmentView : MonoBehaviour
    {
        // Rows in EquipmentSpiritFrames, from the top. Each motif occupies two rows of six frames.
        private static readonly int[] Types = { 15, 16, 26, 29 };
        private static readonly string[] Motifs = { "剑芒", "镜轮", "流纹", "流云" };
        private static readonly string[] EmptyIcons = { "Goods/Sword", "Goods/Compass", "Goods/Robe", "Goods/Boots" };
        private static readonly Color[] Colors = {
            new Color32(251, 207, 137, 255), new Color32(151, 231, 218, 255),
            new Color32(228, 194, 245, 255), new Color32(172, 231, 185, 255)
        };
        private static readonly Color Night = new Color32(18, 38, 43, 234);
        private static readonly Color Cream = new Color32(255, 242, 208, 255);

        private sealed class Slot
        {
            internal int Type, Available, Blocked, FrameRow;
            internal BaseNaJieGoodEntity Worn;
            internal Image Icon, Accent;
            internal Text Name, Tag, Link;
            internal RawImage Spirit;
            internal float Flash = -10;
            internal bool Removing;
        }

        private readonly List<Slot> slots = new List<Slot>();
        private Action<int> choose;
        private Action<BaseNaJieGoodEntity> inspectWorn;
        private Font font;
        private Text harmony, supplies, feedback;
        private RectTransform dial;
        private Image floatingWeapon;
        private static readonly string[] WeaponMotifs = { "剑芒", "刀轮", "枪芒", "箭影", "杖辉" };
        private float feedbackUntil, nextFrame;
        internal Button InventoryButton { get; private set; }

        internal static EquipmentVestmentView Build(RoleZhuangBeiWnd_ZhuangBeiPanel page, Font font,
            Action<int> choose, Action<BaseNaJieGoodEntity> inspectWorn)
        {
            page.transform.Find("StageContent").gameObject.SetActive(false);
            page.GoodsScrollView.gameObject.SetActive(false);
            var formerWorn = page.transform.Find("YiChuanDai");
            var ledger = (RectTransform)formerWorn.Find("ShuXing");
            ledger.SetParent(page.transform, false);
            formerWorn.gameObject.SetActive(false);
            Fit(ledger, .55f, 0, 1, .34f, 2, 6, -6, -3);
            var insetTitle = ledger.Find("Title/Text")?.GetComponent<Text>();
            if (insetTitle != null) insetTitle.text = "修为详录";

            var stage = InkBattleHud.Make<Image>(page.transform, "EquipmentConstellation");
            Fit(stage.rectTransform, 0, .34f, 1, 1, 6, 2, -6, -47);
            stage.color = Night;
            stage.raycastTarget = false;
            stage.gameObject.AddComponent<RectMask2D>();
            var view = stage.gameObject.AddComponent<EquipmentVestmentView>();
            view.font = font;
            view.choose = choose;
            view.inspectWorn = inspectWorn;
            view.BuildStage(stage.rectTransform);
            view.BuildInventoryShelf(page.transform);
            view.Refresh();
            return view;
        }

        private void BuildStage(RectTransform stage)
        {
            var setting = Layer(stage, "观星台");
            setting.sprite = ReleasedResources.Load<Sprite>("SectCourt/observatory");
            setting.color = new Color32(157, 161, 170, 255);
            // Cover the viewport, preserving the painted landscape's proportions.
            setting.gameObject.AddComponent<AspectRatioFitter>().aspectMode = AspectRatioFitter.AspectMode.EnvelopeParent;
            if (setting.sprite != null)
                setting.GetComponent<AspectRatioFitter>().aspectRatio = setting.sprite.rect.width / setting.sprite.rect.height;
            var shade = Layer(stage, "夜色");
            shade.color = new Color32(14, 30, 43, 104);

            var orbit = InkBattleHud.Make<RectTransform>(stage, "星轨");
            Fit(orbit, .22f, .16f, .78f, .97f);
            var disc = Layer(orbit, "金线周天仪");
            disc.sprite = InkInterfaceArt.Get("EquipmentAstrolabe");
            disc.color = new Color(1, .92f, .71f, .83f);
            Square(disc.rectTransform);
            dial = disc.rectTransform;

            var figure = Layer(stage, "修士法相");
            figure.sprite = ReleasedResources.Load<Sprite>("InkCultivation/Meditator");
            figure.preserveAspect = true;
            Fit(figure.rectTransform, .25f, .16f, .75f, .84f);
            floatingWeapon = Layer(stage, "本命兵器绘像");
            floatingWeapon.preserveAspect = true;
            Fit(floatingWeapon.rectTransform, .245f, .40f, .445f, .86f);
            floatingWeapon.enabled = false;

            var atlas = ReleasedResources.Load<Texture2D>("InkInterface/EquipmentSpiritFrames");
            for (int i = 0; i < Types.Length; i++)
            {
                var field = InkBattleHud.Make<RectTransform>(stage, Motifs[i] + "光域");
                switch (Types[i])
                {
                    case 15: Fit(field, .20f, .29f, .54f, .93f); break;
                    case 16: Fit(field, .36f, .36f, .72f, .86f); break;
                    case 26: Fit(field, .29f, .18f, .71f, .81f); break;
                    case 29: Fit(field, .26f, .035f, .74f, .37f); break;
                }
                var fx = InkBattleHud.Make<RawImage>(field, Motifs[i] + "逐帧");
                fx.raycastTarget = false;
                fx.texture = atlas;
                Square(fx.rectTransform);
                fx.enabled = false;
                slots.Add(new Slot { Type = Types[i], Spirit = fx, FrameRow = i });
            }
            for (int i = 0; i < slots.Count; i++) BuildCard(stage, slots[i], i);

            var status = Layer(stage, "法相状态底");
            status.color = new Color32(14, 31, 37, 221);
            Fit(status.rectTransform, .25f, 0, .75f, .122f, 0, 3, 0, -1);
            feedback = Caption(status.transform, "法相状态", "行装待整", 12, Cream);
            InkDialogArt.Inset(feedback.rectTransform, 3, 3, 0, 0);
        }

        private void BuildCard(RectTransform stage, Slot slot, int i)
        {
            bool right = i == 1 || i == 3, top = i < 2;
            var card = Layer(stage, "Slot_" + slot.Type);
            Fit(card.rectTransform, right ? .72f : 0, top ? .52f : .09f, right ? 1 : .28f, top ? .94f : .51f,
                right ? 0 : 5, 2, right ? -5 : 0, -2);
            card.color = Night;
            card.raycastTarget = true;
            var inspect = card.gameObject.AddComponent<Button>();
            inspect.targetGraphic = card;
            inspect.onClick.AddListener(() => { if (slot.Worn != null) inspectWorn(slot.Worn); else choose(slot.Type); });
            slot.Accent = Layer(card.transform, "金属镶边");
            Fit(slot.Accent.rectTransform, 0, 1, 1, 1, 0, -1, 0, 0);
            var title = Caption(card.transform, "部位", EquipmentPicker.SlotName(slot.Type), 14, Colors[i]);
            Fit(title.rectTransform, 0, .80f, 1, 1, 2, 0, -2, 0);
            slot.Icon = Layer(card.transform, "器物绘像");
            slot.Icon.preserveAspect = true;
            Fit(slot.Icon.rectTransform, .08f, .34f, .92f, .81f);
            slot.Name = Caption(card.transform, "器名", "", 13, Cream);
            Fit(slot.Name.rectTransform, 0, .17f, 1, .36f, 2, 0, -2, 0);
            var foot = Layer(card.transform, "选择同部位");
            foot.color = new Color32(48, 79, 78, 245);
            Fit(foot.rectTransform, 0, 0, 1, .19f);
            foot.raycastTarget = true;
            var change = foot.gameObject.AddComponent<Button>();
            change.targetGraphic = foot;
            change.onClick.AddListener(() => choose(slot.Type));
            slot.Link = Caption(foot.transform, "选择", "", 11, Cream);
            InkDialogArt.Inset(slot.Link.rectTransform, 1, 1, 0, 0);
            slot.Tag = Caption(card.transform, "锁定", "", 11, new Color32(255, 221, 136, 255));
            Fit(slot.Tag.rectTransform, .45f, .58f, 1, .8f, 0, 0, -2, 0);
            slot.Tag.alignment = TextAnchor.MiddleRight;
        }

        private void BuildInventoryShelf(Transform page)
        {
            var shelf = Layer(page, "护持与纳戒");
            Fit(shelf.rectTransform, 0, 0, .55f, .34f, 6, 6, -2, -3);
            InkInterfaceArt.Set(shelf, "Parchment", false);
            var head = Caption(shelf.transform, "护持题签", "行装护持", 16, InkInterfaceArt.Ink);
            head.alignment = TextAnchor.MiddleLeft;
            Fit(head.rectTransform, 0, .73f, 1, 1, 9, 0, -6, 0);
            harmony = Caption(shelf.transform, "护持效果", "", 13, InkInterfaceArt.Ink);
            harmony.alignment = TextAnchor.MiddleLeft;
            Fit(harmony.rectTransform, 0, .40f, 1, .75f, 9, 0, -5, 0);
            supplies = Caption(shelf.transform, "纳戒状态", "", 12, InkInterfaceArt.SoftInk);
            supplies.alignment = TextAnchor.MiddleLeft;
            Fit(supplies.rectTransform, 0, .235f, 1, .42f, 9, 0, -5, 0);
            var open = Layer(shelf.transform, "EquipmentLibrary");
            InkInterfaceArt.Set(open, "JadeButton", false);
            open.raycastTarget = true;
            Fit(open.rectTransform, 0, 0, 1, .24f, 5, 3, -5, -1);
            open.gameObject.tag = "Guide";
            InventoryButton = open.gameObject.AddComponent<Button>();
            InventoryButton.targetGraphic = open;
            InventoryButton.onClick.AddListener(() => choose(0));
            var label = Caption(open.transform, "查看", "挑选装备", 15, Cream);
            InkDialogArt.Inset(label.rectTransform, 2, 2, 0, 0);
        }

        internal BaseNaJieGoodEntity Worn(int type) => slots.Find(s => s.Type == type)?.Worn;

        internal void ClearWorn()
        {
            foreach (var slot in slots) slot.Worn = null;
        }

        internal void SetWorn(BaseNaJieGoodEntity gear)
        {
            var slot = slots.Find(s => s.Type == EquipmentPicker.Slot(gear));
            if (slot != null) slot.Worn = gear;
        }

        internal void SetCandidates(IList<BaseNaJieGoodEntity> items)
        {
            int total = 0, locked = 0, unavailable = 0;
            foreach (var slot in slots) slot.Available = slot.Blocked = 0;
            foreach (var item in items)
            {
                int type = EquipmentPicker.Slot(item);
                var slot = slots.Find(s => s.Type == type);
                if (slot == null) continue;
                total++;
                if (item.IsLock) locked++;
                if (string.IsNullOrEmpty(EquipmentPicker.BlockReason(item))) slot.Available++;
                else { slot.Blocked++; unavailable++; }
            }
            supplies.text = "纳戒 " + total + " 件" + (locked > 0 ? " · 锁 " + locked : "") + (unavailable > 0 ? " · 不可用 " + unavailable : "");
            Refresh();
        }

        internal void PlayChange(BaseNaJieGoodEntity gear, bool remove)
        {
            int type = EquipmentPicker.Slot(gear);
            var slot = slots.Find(s => s.Type == type);
            if (slot == null) return;
            if (type == 15) slot.FrameRow = WeaponFrameRow(gear);
            slot.Removing = remove;
            slot.Flash = Time.unscaledTime;
            feedbackUntil = slot.Flash + 1.5f;
            string motif = type == 15 ? WeaponMotifs[slot.FrameRow == 0 ? 0 : slot.FrameRow - 3] : Motifs[slots.IndexOf(slot)];
            feedback.text = EquipmentPicker.SlotName(type) + (remove ? " · 已卸下" : " · " + motif + "显化");
        }

        internal void Refresh()
        {
            for (int i = 0; i < slots.Count; i++)
            {
                var s = slots[i];
                bool filled = s.Worn != null;
                s.Icon.sprite = filled ? RingLedger.Artwork(s.Worn) : InkInterfaceArt.Get(EmptyIcons[i]);
                s.Icon.color = filled ? Color.white : new Color(.78f, .85f, .82f, .52f);
                s.Accent.color = filled ? Colors[i] : new Color(.66f, .64f, .52f, .64f);
                s.Name.text = filled ? s.Worn.Name : "未穿戴";
                s.Tag.text = filled && s.Worn.IsLock ? "已锁" : "";
                s.Link.text = s.Available > 0 ? (filled ? "更换" : "挑选") + " · " + s.Available :
                    s.Blocked > 0 ? "暂不可用 · " + s.Blocked : "查看同位";
            }
            var weapon = Worn(15);
            floatingWeapon.enabled = weapon != null;
            if (weapon != null)
            {
                floatingWeapon.sprite = RingLedger.Artwork(weapon);
                slots[0].FrameRow = WeaponFrameRow(weapon);
            }
            bool paired = Worn(26) != null && Worn(29) != null;
            bool armed = paired && Worn(15) != null;
            harmony.text = (paired ? "◆ " : "◇ ") + "衣甲与足部　装备防御 +15%\n" + (armed ? "◆ " : "◇ ") + "再配主手　装备攻击 +10%";
            if (Time.unscaledTime >= feedbackUntil) ShowWornCount();
        }

        // Historical drops can lack a blank id. Only known artwork families override the straight blade.
        private static int WeaponFrameRow(BaseNaJieGoodEntity gear)
        {
            switch (ForgeWorkshop.PatternArt(gear.BlankId))
            {
                case "Saber": return 4;
                case "Spear": return 5;
                case "Bow": return 6;
                case "Staff": return 7;
                default: return 0;
            }
        }

        internal static string SlotEffect(BaseNaJieGoodEntity gear)
        {
            switch (EquipmentPicker.Slot(gear))
            {
                case 15:
                    int row = WeaponFrameRow(gear);
                    return WeaponMotifs[row == 0 ? 0 : row - 3] + "随器显化，主手提供装备攻击。";
                case 16:
                    long blood = 40L * Mathf.Max(1, gear.Rank) + 10L * Mathf.Max(1, gear.Level);
                    return "镜轮守心，气血上限 +" + blood + "（词条增幅前）。";
                case 26: return "衣纹护体，与足具同穿后装备防御提高 15%。";
                case 29:
                    float speed = .5f + .1f * Mathf.Clamp(gear.Rank, 1, 16);
                    return "步下流云，移动速度 +" + speed.ToString("0.0") + "。";
                default: return "";
            }
        }

        private void ShowWornCount()
        {
            int count = 0;
            foreach (var slot in slots) if (slot.Worn != null) count++;
            feedback.text = "已穿戴   " + count + " / 4";
        }

        private void Update()
        {
            float now = Time.unscaledTime;
            if (now < nextFrame || dial == null) return;
            nextFrame = now + 1f / 24f;
            dial.localRotation = Quaternion.Euler(0, 0, -now * 3f % 360);
            if (floatingWeapon.enabled)
            {
                floatingWeapon.rectTransform.anchoredPosition = new Vector2(0, 2.5f * Mathf.Sin(now * 1.8f));
                floatingWeapon.rectTransform.localRotation = Quaternion.Euler(0, 0, 3f * Mathf.Sin(now * 1.3f));
            }
            if (feedbackUntil > 0 && now > feedbackUntil) { feedbackUntil = 0; ShowWornCount(); }
            for (int i = 0; i < slots.Count; i++)
            {
                var s = slots[i];
                float elapsed = now - s.Flash;
                bool flash = elapsed >= 0 && elapsed < 1.05f;
                bool visible = s.Spirit.texture != null && (s.Worn != null || flash);
                s.Spirit.enabled = visible;
                if (!visible) continue;
                int frame = Mathf.FloorToInt((flash ? elapsed * 17 : now * 8) + i * 3) % 12;
                if (flash && s.Removing) frame = 11 - frame;
                s.Spirit.uvRect = new Rect(frame % 6 / 6f, 1 - (s.FrameRow * 2 + frame / 6 + 1) / 16f, 1f / 6f, 1f / 16f);
                float envelope = flash ? Mathf.Sin(Mathf.Clamp01(elapsed) * Mathf.PI) : 0;
                float alpha = flash && s.Removing ? 1f - elapsed / 1.05f : (flash ? 1f : .76f);
                s.Spirit.color = new Color(1, 1, 1, alpha);
                s.Spirit.rectTransform.localScale = Vector3.one * (flash && s.Removing ? 1f - elapsed * .4f : 1f + .27f * envelope);
            }
        }

        private Text Caption(Transform parent, string name, string text, int size, Color color)
        {
            var label = InkBattleHud.Label(parent, font, name, text, size, color);
            label.alignment = TextAnchor.MiddleCenter;
            label.supportRichText = false;
            return label;
        }

        private static Image Layer(Transform parent, string name)
        {
            var image = InkBattleHud.Make<Image>(parent, name);
            image.raycastTarget = false;
            Fit(image.rectTransform, 0, 0, 1, 1);
            return image;
        }

        private static void Square(RectTransform rect)
        {
            var fit = rect.gameObject.AddComponent<AspectRatioFitter>();
            fit.aspectMode = AspectRatioFitter.AspectMode.FitInParent;
            fit.aspectRatio = 1;
        }

        private static void Fit(RectTransform rect, float x, float y, float x2, float y2,
            float left = 0, float bottom = 0, float right = 0, float top = 0)
        {
            rect.anchorMin = new Vector2(x, y);
            rect.anchorMax = new Vector2(x2, y2);
            rect.offsetMin = new Vector2(left, bottom);
            rect.offsetMax = new Vector2(right, top);
        }
    }
}
