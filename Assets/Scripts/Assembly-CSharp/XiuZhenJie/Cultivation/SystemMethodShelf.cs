using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

namespace XiuZhenJie
{
    internal sealed class SystemMethodShelf
    {
        [Serializable] internal class Method
        {
            public string id, name, description;
            public int polarity, element, qi, kind;
        }
        [Serializable] private class Catalog { public Method[] methods; }

        private static Dictionary<string, Method> definitions;
        private static readonly string[] Poles = { "全部", "阴", "阳" };
        private static readonly string[] Elements = { "全部", "金", "木", "水", "火", "土" };
        private static readonly string[] Breaths = { "全部", "风", "雷", "寒", "炎", "润", "燥", "生", "煞", "清", "浊", "罡", "幻" };
        private static readonly string[] Practices = { "全部", "炼神", "锻体", "纳气" };
        internal enum Shelf { Ready, Locked, Learned }
        private static readonly string[] Titles = { "可参悟", "待突破", "已参悟" };
        private const int PageSize = 12;
        private readonly CanWuPanel panel;
        private readonly Font font;
        private readonly Action refresh;
        private readonly Button[] tabs = new Button[3];
        private readonly RectTransform pager;
        private readonly Button previous, next;
        private readonly Text pageNumber, emptyMessage;
        private GameObject overlay;
        private int page;
        private Shelf selected;
        internal string Title => Titles[(int)selected];

        internal static bool IsSystem(string id) => id != null && id.StartsWith("sys_", StringComparison.Ordinal);

        internal static Method Find(string id)
        {
            if (!IsSystem(id)) return null;
            if (definitions == null)
            {
                definitions = new Dictionary<string, Method>();
                var source = ReleasedResources.Load<TextAsset>("InkCultivation/SystemMethods");
                var table = source != null ? JsonUtility.FromJson<Catalog>(source.text) : null;
                if (table?.methods != null) foreach (var method in table.methods)
                {
                    string[] parts = method.id.Split('_');
                    if (parts.Length != 4 || !int.TryParse(parts[1],out method.polarity) || !int.TryParse(parts[2],out method.element)
                        || !int.TryParse(parts[3],out method.qi) || method.polarity < 1 || method.polarity >= Poles.Length
                        || method.element < 1 || method.element >= Elements.Length || method.qi < 1 || method.qi >= Breaths.Length) continue;
                    definitions[method.id] = method;
                }
            }
            definitions.TryGetValue(id, out var item);
            return item;
        }

        private static RoleInfoEntity Role => Singleton<RoleManager>.Instance.Role_Entity;

        internal static bool MatchesRoots(CanWuEntity book)
        {
            if (book.Type != (int)CanWuTitleType.GongFa || book.Id == PracticeGuideFlow.StarterId) return true;
            var role = Role;
            if (role == null) return false;
            switch ((WuXing)book.FiveElement)
            {
                case WuXing.Jin: return role.JinLingGen;
                case WuXing.Mu: return role.MuLingGen;
                case WuXing.Shui: return role.ShuiLingGen;
                case WuXing.Huo: return role.HuoLingGen;
                case WuXing.Tu: return role.TuLingGen;
                default: return false;
            }
        }

        internal static Shelf? State(CanWuEntity book)
        {
            if (book.IsCanWu) return Shelf.Learned;
            if (Role == null || !MatchesRoots(book)) return null;
            return Role.RoleLevel >= book.Level ? Shelf.Ready : Shelf.Locked;
        }

        internal static string EquipHint(CanWuEntity book)
        {
            if (Role.RoleLevel < book.Level)
                return "达到" + WorldSetting.GetRoleLevelNameByRoleLevel(book.Level) + "后可在练功房更换。";
            return MatchesRoots(book) ? "已参悟，可在练功房更换功法。" : "此前已悟，可在练功房换回。";
        }

        internal static string PracticeName(CanWuEntity book)
        {
            var system = Find(book.Id);
            int kind = system?.kind ?? 0;
            if (kind == 0 && book.Id != null && book.Id.StartsWith("gf_", StringComparison.Ordinal))
            {
                var parts = book.Id.Split('_');
                if (parts.Length == 3) int.TryParse(parts[1], out kind);
            }
            return kind > 0 && kind < Practices.Length ? Practices[kind] : "";
        }

        internal static string Signature(Method def) => Poles[def.polarity] + Elements[def.element] + " · " + Breaths[def.qi] + "气";
        private static void Bounds(RectTransform rect, float l, float b, float r, float t)
        {
            rect.anchorMin = new Vector2(l, b); rect.anchorMax = new Vector2(r, t);
            rect.offsetMin = rect.offsetMax = Vector2.zero;
        }
        private Text Label(Transform parent, string name, string value, int size, float l, float b, float r, float t, TextAnchor align = TextAnchor.MiddleLeft)
        {
            var text = RoleFolio.Label(parent, font, value, size, RoleFolio.Ink, align);
            text.name = name; Bounds(text.rectTransform, l, b, r, t);
            return text;
        }
        private Button ButtonAt(Transform parent, string name, string caption, float l, float b, float r, float t, Action act)
        {
            var face = RoleFolio.Image(parent, name, Color.white, true);
            Bounds(face.rectTransform, l, b, r, t);
            var button = face.gameObject.AddComponent<Button>(); button.targetGraphic = face;
            Label(face.transform, "Label", caption, 13, .02f, .02f, .98f, .98f, TextAnchor.MiddleCenter);
            Mark(button, false);
            button.onClick.AddListener(() => act());
            return button;
        }
        private static void Mark(Button button, bool selected)
        {
            InkDialogArt.StyleButton(button, selected ? "JadeButton" : "LightPlaque", selected ? InkDialogArt.Ivory : InkInterfaceArt.Ink);
        }

        internal SystemMethodShelf(CanWuPanel panel, Action refresh)
        {
            this.panel = panel; this.refresh = refresh; font = panel.SmallTitleText.font;
            var bar = RoleFolio.Box(panel.transform, "MethodShelves");
            bar.anchorMin = new Vector2(0, 1); bar.anchorMax = Vector2.one;
            bar.offsetMin = new Vector2(12, -115); bar.offsetMax = new Vector2(-12, -80);
            string[] names = { "Ready", "Locked", "Learned" };
            for (int i = 0; i < tabs.Length; i++)
            {
                var state = (Shelf)i;
                tabs[i] = ButtonAt(bar, names[i], Titles[i], i / 3f, 0, (i + 1) / 3f - .008f, 1, () =>
                {
                    selected = state;
                    page = 0;
                    refresh();
                });
            }
            pager = RoleFolio.Box(panel.transform, "StudyPages");
            pager.anchorMin = Vector2.zero; pager.anchorMax = Vector2.right;
            pager.offsetMin = new Vector2(12, 4); pager.offsetMax = new Vector2(-12, 37);
            previous = ButtonAt(pager, "Previous", "上页", .17f, 0, .38f, 1, () => { page--; refresh(); });
            next = ButtonAt(pager, "Next", "下页", .62f, 0, .83f, 1, () => { page++; refresh(); });
            pageNumber = Label(pager, "Page", "", 13, .38f, 0, .62f, 1, TextAnchor.MiddleCenter);
            emptyMessage = Label(panel.ScrollView.transform, "EmptyShelf", "", 14, .07f, .63f, .93f, .91f, TextAnchor.MiddleCenter);
            emptyMessage.gameObject.SetActive(false);
            panel.TiShiPanel.SetAsLastSibling();
            UpdateNavigation(0);
        }

        internal void Reset()
        {
            selected = Shelf.Ready;
            page = 0;
            ClosePaper();
        }

        private void UpdateNavigation(int pages)
        {
            for (int i = 0; i < tabs.Length; i++) Mark(tabs[i], selected == (Shelf)i);
            pager.gameObject.SetActive(pages > 1);
            previous.interactable = page > 0;
            next.interactable = page + 1 < pages;
            pageNumber.text = (page+1) + "/" + pages;
            var list = (RectTransform)panel.ScrollView.transform;
            list.offsetMin = new Vector2(12, pages > 1 ? 41 : 4);
            list.offsetMax = new Vector2(-12, -119);
        }

        internal List<CanWuEntity> Books(List<CanWuEntity> source)
        {
            var matching = source.Where(book => State(book) == selected)
                .OrderByDescending(book => book.Id == PracticeGuideFlow.StarterId)
                .ThenBy(book => book.Level).ThenBy(book => IsSystem(book.Id)).ThenBy(book => book.Rank)
                .ThenBy(book => book.FiveElement).ThenBy(book => book.Id, StringComparer.Ordinal).ToList();
            int pages = (matching.Count + PageSize - 1) / PageSize;
            page = Mathf.Clamp(page, 0, Mathf.Max(0, pages - 1));
            UpdateNavigation(pages);
            panel.SmallTitleText.text = Title + " " + matching.Count + " 门";
            emptyMessage.gameObject.SetActive(matching.Count == 0);
            if (matching.Count == 0)
            {
                if (selected == Shelf.Ready)
                    emptyMessage.text = "暂无可悟典籍\n可在纳戒阅读与灵根相合的功法书。";
                else if (selected == Shelf.Locked)
                    emptyMessage.text = "没有等待突破的典籍。";
                else
                    emptyMessage.text = "尚未参悟典籍。";
            }
            return matching.Skip(page * PageSize).Take(PageSize).ToList();
        }

        private RectTransform OpenPaper(string title)
        {
            ClosePaper();
            var shade = RoleFolio.Image(panel.transform, "TransmissionPaper", new Color32(22,38,34,194), true);
            overlay = shade.gameObject; RoleFolio.Fill(shade.rectTransform);
            var paper = RoleFolio.Image(shade.transform, "Sheet", Color.white, true);
            InkInterfaceArt.Set(paper,"InkLedgerPaper",false); Bounds(paper.rectTransform,.035f,.085f,.965f,.914f);
            var heading = Label(paper.transform,"Title",title,23,.055f,.852f,.76f,.979f);
            heading.fontStyle = FontStyle.Bold;
            ButtonAt(paper.transform,"Back","返回",.78f,.884f,.95f,.96f,ClosePaper);
            return paper.rectTransform;
        }

        private void ClosePaper()
        {
            if (overlay == null) return;
            overlay.SetActive(false); UnityEngine.Object.Destroy(overlay); overlay = null;
        }

        internal void Details(CanWuEntity book, Action accept, Action equip)
        {
            var sheet = OpenPaper(book.Name);
            var def = Find(book.Id);
            string sign = def == null ? WorldSetting.GetWuXingName((WuXing)book.FiveElement) : Signature(def);
            var glyph = RoleFolio.Image(sheet,"Element",Color.white);
            glyph.sprite = InkInterfaceArt.Get("Element" + (WuXing)book.FiveElement); glyph.preserveAspect = true;
            Bounds(glyph.rectTransform,.055f,.64f,.225f,.83f);
            Label(sheet,"Affinity",sign,19,.25f,.739f,.94f,.835f);
            Label(sheet,"State",WorldSetting.GetRankAllName(book.Rank) + "　·　天道传承",14,.25f,.65f,.94f,.735f);
            string gate = WorldSetting.GetRoleLevelNameByRoleLevel(book.Level);
            Label(sheet,"Description",def?.description ?? "",17,.066f,.46f,.934f,.628f);
            Label(sheet,"Gate","所需境界　" + gate,15,.067f,.345f,.935f,.432f);
            var facts = Label(sheet,"Effect","正在查阅传承…",15,.067f,.20f,.935f,.337f);
            var state = State(book);
            string action = book.IsCanWu ? "去练功房" : state == Shelf.Ready ? "免费参悟" : gate + "可悟";
            var button = ButtonAt(sheet,"Study",action,.13f,.047f,.87f,.151f,() => { ClosePaper(); if (book.IsCanWu) equip(); else accept(); });
            Mark(button,true); button.interactable = false;
            var req = new MethodAchievementDetailReq_1011004 { Id = book.Id };
            bool queued = Singleton<NetManager>.Instance.TrySendGameRequest(MsgType.TEMPLATE_ACHIEVEMENT_DETAIL,req,bytes =>
            {
                if (sheet == null || !sheet.gameObject.activeInHierarchy) return;
                if (bytes == null) { facts.text = "查阅超时，请返回书目再试。"; return; }
                var info = MethodAchievementDetailResp_1011004.Parser.ParseFrom(bytes);
                if (!info.IsSuccess) { facts.text = "此传承暂不可查阅。"; return; }
                string practice = info.ActionScope >= 1 && info.ActionScope <= 3 ? Practices[info.ActionScope] : "";
                string instruction = "天道传承无需功法书。";
                if (book.IsCanWu)
                    instruction = EquipHint(book);
                facts.text = practice + "　灵衍值 " + info.ConversionRate.ToString("0.##") + "\n" + instruction;
                button.interactable = book.IsCanWu || State(book) == Shelf.Ready;
            });
            if (!queued) facts.text = "连接中断，请返回后重新查阅。";
        }
    }
}
