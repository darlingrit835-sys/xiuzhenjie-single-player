using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

namespace XiuZhenJie
{
    internal sealed class EquipmentPicker : MonoBehaviour
    {
        private static readonly int[] Slots = { 0, 15, 16, 26, 29 };
        private static readonly string[][] Options = {
            new[] { "全部", "可穿戴", "待认主", "不可用", "已锁定" },
            new[] { "全部", "黄", "玄", "地", "天" },
            new[] { "全部", "金", "木", "水", "火", "土" }
        };
        private static readonly string[] Facets = { "状态", "品阶", "五行" };
        private static readonly Color Jade = new Color32(29, 67, 63, 255);
        private static readonly Color Gold = new Color32(220, 194, 133, 255);
        private static readonly Color Ink = new Color32(44, 52, 50, 255);
        private static readonly Color Muted = new Color32(87, 94, 87, 255);
        private static readonly Color Seal = new Color32(145, 60, 45, 255);
        private static readonly Color[] RankColors = {
            new Color32(147,117,69,255), new Color32(60,118,104,255),
            new Color32(119,88,143,255), new Color32(185,111,45,255)
        };
        private readonly int[] selected = new int[3];
        private Font font;
        private RectTransform page, rows;
        private ScrollRect scroll;
        private RectTransform choices;
        private Text count, empty;
        private readonly List<Button> slots = new List<Button>();
        private readonly List<Button> facets = new List<Button>();
        private IList<BaseNaJieGoodEntity> inventory;
        private Action<BaseNaJieGoodEntity> inspect;
        private int slot, expanded = -1;
        private string query = "";

        internal static int Slot(BaseNaJieGoodEntity item)
        {
            if (item == null) return 0;
            switch (item.Type)
            {
                case 15: case 26: case 29: return item.Type;
                case 16:
                    var template = item.TemplateId ?? "";
                    return template == "eq_armor_qingbu" || template.StartsWith("drop_16_", StringComparison.Ordinal) ? 26 : 16;
                default: return 0;
            }
        }

        internal static string SlotName(int type)
        {
            switch (type)
            {
                case 15: return "主手";
                case 16: return "护身";
                case 26: return "衣甲";
                case 29: return "足部";
                default: return "全部";
            }
        }

        internal static string BlockReason(BaseNaJieGoodEntity item)
        {
            if (Slot(item) == 0) return "此物无法穿戴";
            if (item.OfferPending) return "待道友交接";
            int stage = WorldSetting.GetStageByLevel(Mathf.Max(1, item.Level));
            int required = WorldSetting.GetLevelByStage(stage);
            if (Mathf.Max(1, Singleton<RoleManager>.Instance.Role_Entity.RoleLevel) < required)
                return "需" + WorldSetting.GetStageChineseName(stage) + "境（" + required + "级）";
            return "";
        }

        internal static void Show(Transform parent, Font font, IList<BaseNaJieGoodEntity> inventory,
            int goodsType, Action<BaseNaJieGoodEntity> inspect)
        {
            if (parent.Find("EquipmentPicker") != null) return;
            var view = InkBattleHud.Make<EquipmentPicker>(parent, "EquipmentPicker");
            view.font = font;
            view.inventory = inventory;
            view.slot = goodsType;
            view.inspect = inspect;
            view.Build();
        }

        internal static void Dismiss(Transform parent)
        {
            var picker = parent.Find("EquipmentPicker");
            if (picker == null) return;
            picker.gameObject.SetActive(false);
            Destroy(picker.gameObject);
        }

        private static void Band(RectTransform r, float from, float to, float left, float right, float top, float h)
        {
            r.anchorMin = new Vector2(from, 1); r.anchorMax = new Vector2(to, 1);
            r.offsetMin = new Vector2(left, -top - h); r.offsetMax = new Vector2(-right, -top);
        }

        private static Image Surface(Transform parent, string name, Color color)
        {
            var image = InkBattleHud.Make<Image>(parent, name);
            image.color = color; image.raycastTarget = false;
            return image;
        }

        private Text Label(Transform parent, string name, string value, int size, Color color, TextAnchor alignment)
        {
            var t = InkBattleHud.Label(parent, font, name, value, size, color);
            t.alignment = alignment; t.supportRichText = false;
            return t;
        }

        private Button Tab(Transform parent, string name, string title, Action tap)
        {
            var surface = Surface(parent, name, Color.white);
            surface.raycastTarget = true;
            var b = surface.gameObject.AddComponent<Button>();
            b.targetGraphic = surface;
            var text = Label(surface.transform, "Label", title, 14, Ink, TextAnchor.MiddleCenter);
            InkDialogArt.Inset(text.rectTransform, 2, 2, 0, 0);
            b.onClick.AddListener(() => tap());
            Highlight(b, false);
            return b;
        }

        private static void Highlight(Button b, bool on)
        {
            b.image.color = on ? Jade : new Color32(231, 226, 209, 255);
            b.GetComponentInChildren<Text>().color = on ? InkDialogArt.Ivory : Ink;
        }

        private void Build()
        {
            var root = (RectTransform)transform;
            InkDialogArt.Inset(root, 0, 0, 0, 0);
            gameObject.AddComponent<Image>().color = new Color32(18, 28, 28, 212);
            var paper = Surface(root, "Cabinet", Color.white);
            InkDialogArt.SetPaper(paper);
            paper.raycastTarget = true;
            page = paper.rectTransform;
            InkDialogArt.Inset(page, 5, 5, 4, 5);

            var header = Surface(page, "Lintel", Jade);
            Band(header.rectTransform, 0, 1, 0, 0, 0, 51);
            var title = Label(header.transform, "Title", "行装库", 23, InkDialogArt.Ivory, TextAnchor.MiddleLeft);
            Band(title.rectTransform, 0, 1, 14, 125, 0, 49);
            count = Label(header.transform, "Quantity", "", 14, Gold, TextAnchor.MiddleRight);
            Band(count.rectTransform, .45f, 1, 0, 63, 0, 49);
            var close = Tab(header.transform, "Close", "返回", () => Destroy(gameObject));
            Band(close.image.rectTransform, 1, 1, -55, 8, 9, 32);
            Band(Surface(page, "GoldRule", Gold).rectTransform, 0, 1, 0, 0, 51, 2);
            for (int i = 0; i < Slots.Length; i++)
            {
                int key = Slots[i];
                var b = Tab(page, "Slot" + key, SlotName(key), () => { slot = key; expanded = -1; Draw(); });
                Band(b.image.rectTransform, i / 5f, (i + 1) / 5f, 3, 3, 61, 33);
                slots.Add(b);
            }
            var search = DefaultControls.CreateInputField(new DefaultControls.Resources());
            search.name = "Search"; search.transform.SetParent(page, false);
            foreach (var child in search.GetComponentsInChildren<Transform>(true)) child.gameObject.layer = gameObject.layer;
            Band((RectTransform)search.transform, 0, 1, 6, 6, 101, 32);
            var input = search.GetComponent<InputField>();
            InkInterfaceArt.Set(input.image, "ParchmentHeading", false);
            input.textComponent.font = font; input.textComponent.fontSize = 14; input.textComponent.color = Ink;
            var hint = (Text)input.placeholder;
            hint.font = font; hint.fontSize = 14; hint.fontStyle = FontStyle.Normal;
            hint.text = "搜索装备名称"; hint.color = Muted;
            input.onValueChanged.AddListener(value => { query = value.Trim(); DrawRows(); });
            for (int i = 0; i < Facets.Length; i++)
            {
                int f = i;
                var b = Tab(page, "Filter" + f, "", () => { expanded = expanded == f ? -1 : f; Draw(); });
                Band(b.image.rectTransform, i / 3f, (i + 1) / 3f, 3, 3, 139, 31);
                facets.Add(b);
            }
            choices = InkBattleHud.Make<RectTransform>(page, "Options");
            Band(choices, 0, 1, 3, 3, 175, 32);

            scroll = InkBattleHud.Make<ScrollRect>(page, "EquipmentList");
            var viewport = InkBattleHud.Make<RectTransform>(scroll.transform, "Viewport");
            InkDialogArt.Inset(viewport, 0, 0, 0, 0);
            viewport.gameObject.AddComponent<RectMask2D>();
            var hit = viewport.gameObject.AddComponent<Image>(); hit.color = Color.clear;
            rows = InkBattleHud.Make<RectTransform>(viewport, "Rows");
            rows.anchorMin = new Vector2(0, 1); rows.anchorMax = Vector2.one;
            rows.pivot = new Vector2(.5f, 1); rows.anchoredPosition = Vector2.zero;
            scroll.horizontal = false; scroll.movementType = ScrollRect.MovementType.Clamped;
            scroll.viewport = viewport; scroll.content = rows;
            empty = Label(viewport, "Empty", "没有符合条件的装备", 17, Muted, TextAnchor.MiddleCenter);
            InkDialogArt.Inset(empty.rectTransform, 5, 5, 0, 0);
            Draw();
        }

        private static void Clear(Transform parent)
        {
            foreach (Transform child in parent)
            {
                child.gameObject.SetActive(false);
                Destroy(child.gameObject);
            }
        }

        private void Draw()
        {
            for (int i = 0; i < slots.Count; i++) Highlight(slots[i], slot == Slots[i]);
            for (int i = 0; i < facets.Count; i++)
            {
                facets[i].GetComponentInChildren<Text>().text = selected[i] == 0 ? Facets[i] : Facets[i] + "·" + Options[i][selected[i]];
                Highlight(facets[i], selected[i] != 0 || expanded == i);
            }
            Clear(choices);
            choices.gameObject.SetActive(expanded >= 0);
            if (expanded >= 0)
            {
                int f = expanded;
                var options = Options[f];
                for (int i = 0; i < options.Length; i++)
                {
                    int value = i;
                    var button = Tab(choices, "Value" + value, options[i], () => { selected[f] = value; expanded = -1; Draw(); });
                    Band(button.image.rectTransform, i / (float)options.Length, (i + 1f) / options.Length, 2, 2, 0, 30);
                    Highlight(button, selected[f] == i);
                }
            }
            InkDialogArt.Inset((RectTransform)scroll.transform, 5, 5, expanded >= 0 ? 213 : 177, 7);
            DrawRows();
        }

        private void DrawRows()
        {
            Clear(rows);
            int visible = 0;
            foreach (var item in inventory.Where(g => Slot(g) != 0).OrderByDescending(g => g.Rank).ThenBy(g => g.Name, StringComparer.Ordinal))
            {
                if (slot != 0 && Slot(item) != slot || selected[1] != 0 && selected[1] != item.GetGoodsRank()
                    || selected[2] != 0 && selected[2] != item.FiveElement || (item.Name ?? "").IndexOf(query, StringComparison.OrdinalIgnoreCase) < 0) continue;
                string reason = BlockReason(item);
                bool available = reason.Length == 0;
                switch (selected[0])
                {
                    case 1: if (!available || !item.IsBinding) continue; break;
                    case 2: if (!available || item.IsBinding) continue; break;
                    case 3: if (available) continue; break;
                    case 4: if (!item.IsLock && !item.OfferPending) continue; break;
                }
                MakeRow(item, reason, visible++);
            }
            rows.sizeDelta = new Vector2(0, visible * 58);
            scroll.StopMovement(); scroll.verticalNormalizedPosition = 1;
            empty.gameObject.SetActive(visible == 0); count.text = visible + " 件";
        }

        private void MakeRow(BaseNaJieGoodEntity item, string reason, int index)
        {
            bool barred = reason.Length > 0;
            Color rank = RankColors[item.GetGoodsRank() - 1];
            var face = Surface(rows, "Item_" + item.GoodsId, barred ? new Color32(218,216,207,255) : new Color32(246,242,229,255));
            face.raycastTarget = true;
            Band(face.rectTransform, 0, 1, 0, 0, index * 58, 55);
            var link = face.gameObject.AddComponent<Button>(); link.targetGraphic = face;
            link.onClick.AddListener(() => inspect(item));
            Band(Surface(face.transform, "QualityLine", rank).rectTransform, 0, 0, 0, -3, 0, 55);
            var icon = Surface(face.transform, "Artwork", barred ? new Color(.54f,.55f,.54f) : Color.white);
            icon.sprite = RingLedger.Artwork(item); icon.preserveAspect = true;
            Band(icon.rectTransform, 0, 0, 6, -51, 4, 47);
            if (item.IsLock || item.OfferPending) LockMark(face.transform);
            var name = Label(face.transform, "Name", item.Name, 17, barred ? Muted : Ink, TextAnchor.MiddleLeft);
            Band(name.rectTransform, 0, 1, 57, 87, 3, 25);
            var state = Label(face.transform, "Status", barred ? "不可用" : item.IsBinding ? "可穿戴" : "待认主", 14, barred ? Seal : Jade, TextAnchor.MiddleRight);
            Band(state.rectTransform, 1, 1, -85, 8, 3, 25);
            string traits = SlotName(Slot(item)) + "  ·  " + item.GetGoodsRankName() + "阶  ·  " + item.GetFiveElementName();
            var info = Label(face.transform, "Traits", traits, 12, Muted, TextAnchor.MiddleLeft);
            Band(info.rectTransform, 0, .64f, 57, 0, 28, 22);
            string note = barred ? reason : item.IsLock ? "已锁定保护" : WorldSetting.GetStageChineseName(Mathf.Max(1, item.GetGoodsStage())) + "境";
            var detail = Label(face.transform, "Reason", note, 12, barred ? Seal : Muted, TextAnchor.MiddleRight);
            Band(detail.rectTransform, .64f, 1, 0, 8, 28, 22);
        }

        private static void LockMark(Transform row)
        {
            var plaque = Surface(row, "Lock", Jade);
            Band(plaque.rectTransform, 0, 0, 33, -53, 32, 20);
            var metal = new Color32(251, 221, 147, 255);
            Band(Surface(plaque.transform, "Loop", metal).rectTransform, 0, 0, 6, -14, 3, 7);
            Band(Surface(plaque.transform, "Opening", Jade).rectTransform, 0, 0, 8, -12, 5, 4);
            Band(Surface(plaque.transform, "Bolt", metal).rectTransform, 0, 0, 4, -16, 9, 8);
        }
    }
}
