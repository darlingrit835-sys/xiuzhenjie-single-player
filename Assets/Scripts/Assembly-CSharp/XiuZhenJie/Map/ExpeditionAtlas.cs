using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using static XiuZhenJie.RoleFolio;

namespace XiuZhenJie
{
    internal sealed class ExpeditionAtlas : MonoBehaviour
    {
        internal static readonly Color Gold = new Color32(255, 218, 127, 255);
        private static readonly Color Muted = new Color32(103, 95, 83, 255);
        private static readonly Color CoolLight = new Color32(196, 221, 226, 255);
        // River flats for settlements, wooded banks for wilderness. Coordinates are
        // normalized to the painted chart, whose rect also owns all the hit targets.
        private static readonly Vector2[] Towns = { new(.19f,.85f), new(.50f,.68f), new(.78f,.62f), new(.49f,.43f), new(.20f,.36f), new(.52f,.17f) };
        private static readonly Vector2[] Forests = { new(.81f,.87f), new(.18f,.61f), new(.53f,.94f), new(.81f,.38f), new(.18f,.12f), new(.81f,.14f) };
        private static readonly Vector2[] Depths = { new(.23f,.23f), new(.67f,.48f), new(.30f,.78f), new(.77f,.82f) };

        private sealed class Pin
        {
            internal RectTransform Root;
            internal Image Stamp, Plate, Seal;
            internal Text Name, Status;
            internal ExpeditionGlow Glow;
            internal CSZ_CityItemPanel Site;
            internal ExpeditionPlace Info;
        }

        private readonly List<Pin> pins = new List<Pin>();
        private readonly List<CSZ_CityItemPanel> places = new List<CSZ_CityItemPanel>();
        private WorldMapPanel panel;
        private CSZ_MapPanel map;
        private Font font;
        private RectTransform chart, dossier, guild;
        private Text title, facts, description, submitText, empty, guildStatus;
        private Button submit, sweep, retry;
        private Image landscape;
        private Pin selected;
        private bool seekGuild;
        private System.Action retryGuild;
        private bool Interior => map != null && (map.mapType == MapType.MapWaiWei_1 || map.mapType == MapType.MapWaiWei_2);
        private static readonly Color Cream = new Color32(255, 248, 224, 255);

        internal static ExpeditionAtlas Build(WorldMapPanel panel)
        {
            var root = Box(panel.transform.Find("Content"), "历练行旅图");
            Fill(root);
            var atlas = root.gameObject.AddComponent<ExpeditionAtlas>();
            atlas.panel = panel;
            atlas.font = panel.TitleText.font;
            var paper = root.gameObject.AddComponent<Image>();
            InkInterfaceArt.Set(paper, "Paper", false);
            atlas.BuildMap(root);
            atlas.BuildGuild(root);
            var tabs = Box(root, "舆图分册");
            Band(tabs, 0, 0, 0, 36);
            var group = tabs.gameObject.AddComponent<ToggleGroup>();
            panel.LiLianDiTuTog.group = panel.ZongPaiShanMenTog.group = group;
            atlas.Tab(panel.LiLianDiTuTog, tabs, 0);
            atlas.Tab(panel.ZongPaiShanMenTog, tabs, .5f);
            root.gameObject.SetActive(false);
            return atlas;
        }

        private void Tab(Toggle tab, RectTransform parent, float x)
        {
            var rt = (RectTransform)tab.transform;
            rt.SetParent(parent, false);
            Span(rt, x, x + .5f, 3, 3);
            tab.gameObject.SetActive(true);
            InkInterfaceArt.Set(tab.targetGraphic as Image, "BrushPlaque", false);
            InkInterfaceArt.Set(tab.graphic as Image, "InkPlaque", false);
            var label = tab.GetComponent<ToggleItemTextChangeColor>();
            label.IsOnFalseColor = Ink;
            label.IsOnTrueColor = Cream;
            label.ContentText.color = tab.isOn ? Cream : Ink;
            Style(label.ContentText, 16, label.ContentText.color, TextAnchor.MiddleCenter, true);
        }

        private void BuildMap(RectTransform root)
        {
            landscape = RoleFolio.Image(root, "山川底图", Color.white, true);
            chart = landscape.rectTransform;
            Fill(chart, 0, 0, 37, 83);
            empty = Label(chart, font, "", 15, Ink, TextAnchor.MiddleCenter);
            Fill(empty.rectTransform);
            var paper = RoleFolio.Image(root, "行旅签", Color.white, true);
            InkInterfaceArt.Set(paper, "Paper", false);
            dossier = paper.rectTransform;
            Bottom(dossier, 83);
            var line = RoleFolio.Image(dossier, "卷边", new Color32(162, 136, 88, 200));
            Band(line.rectTransform, 0, 0, 0, 2);
            title = Label(dossier, font, "", 19, Ink, TextAnchor.MiddleLeft, true);
            Style(title, 19, Ink, TextAnchor.MiddleLeft, true, 12);
            Band(title.rectTransform, 13, 95, 4, 26);
            facts = Label(dossier, font, "", 13, Jade);
            Style(facts, 13, Jade, TextAnchor.MiddleLeft, false, 10);
            Band(facts.rectTransform, 14, 97, 30, 19);
            description = Label(dossier, font, "", 13, Secondary);
            Style(description, 13, Secondary, TextAnchor.MiddleLeft, false, 10);
            Band(description.rectTransform, 14, 96, 49, 29);
            submit = ActionButton(dossier, "行旅动作", out submitText);
            submit.onClick.AddListener(() => Act(false));
            sweep = ActionButton(dossier, "扫荡", out var sweepText);
            sweepText.text = "扫荡";
            sweep.onClick.AddListener(() => Act(true));
        }

        private void BuildGuild(RectTransform root)
        {
            var scene = RoleFolio.Image(root, "山门驻地", Color.white, true);
            InkInterfaceArt.Set(scene, "GuildHall", false);
            guild = scene.rectTransform;
            Fill(guild, 0, 0, 37, 0);
            var ledger = panel.MapWaiWei_1_Extend;
            ledger.SetParent(guild, false);
            ledger.gameObject.SetActive(true);
            ledger.anchorMin = Vector2.zero; ledger.anchorMax = new Vector2(1, .63f);
            ledger.offsetMin = new Vector2(6, 6); ledger.offsetMax = new Vector2(-6, 0);
            var face = ledger.GetComponent<Image>() ?? ledger.gameObject.AddComponent<Image>();
            InkInterfaceArt.Set(face, "Paper", false);
            var scroll = ledger.GetComponentInChildren<ScrollRect>(true);
            Fill((RectTransform)scroll.transform, 6, 6, 35, 5);
            Flat(scroll.GetComponent<Image>(), Color.clear, true);
            Fill(scroll.viewport);
            scroll.horizontal = false;
            var group = scroll.content.GetComponent<VerticalLayoutGroup>();
            group.padding = new RectOffset(1, 1, 2, 2);
            group.spacing = 3; group.childControlWidth = true; group.childForceExpandWidth = true;
            group.childControlHeight = false; group.childForceExpandHeight = false;
            guildStatus = Label(ledger, font, "", 14, Jade, TextAnchor.MiddleLeft, true);
            Band(guildStatus.rectTransform, 12, 12, 5, 27);
            retry = ActionButton(ledger, "重查驻地", out var retryText);
            retryText.text = "重查";
            Fixed((RectTransform)retry.transform, 1, -10, 5, 58, 27);
            retry.onClick.AddListener(() => retryGuild?.Invoke());
            GuildStatus("正在查访当地山门…");
        }

        internal void GuildStatus(string message, System.Action again = null, int count = 0)
        {
            // A single entry needs one page strip. Long registers scroll within the lower two-thirds.
            guildCount = count;
            SizeRegister();
            guildStatus.text = message;
            retryGuild = again;
            retry.gameObject.SetActive(again != null);
        }

        private int guildCount;
        private void SizeRegister()
        {
            if (guild == null || panel == null) return;
            var ledger = panel.MapWaiWei_1_Extend;
            float available = guild.rect.height;
            if (available <= 0) return;
            ledger.anchorMin = Vector2.zero; ledger.anchorMax = Vector2.right;
            ledger.offsetMin = new Vector2(6, 6);
            ledger.offsetMax = new Vector2(-6, 6 + Mathf.Min(available * .68f, 46 + 95 * guildCount));
        }

        private void OnRectTransformDimensionsChange() { SizeRegister(); }

        private Button ActionButton(RectTransform parent, string name, out Text caption)
        {
            var image = RoleFolio.Image(parent, name, Color.white, true);
            InkInterfaceArt.Set(image, "InkPlaque", false);
            var button = image.gameObject.AddComponent<Button>();
            button.targetGraphic = image;
            caption = Label(image.transform, font, "", 14, Cream, TextAnchor.MiddleCenter, true);
            Fill(caption.rectTransform, 4, 4);
            return button;
        }

        internal void Present(CSZ_MapPanel current, bool gates)
        {
            bool changed = map != current || gates != seekGuild || !gameObject.activeSelf;
            string previousId = changed ? null : selected?.Site?.dataSource?.Id;
            map = current; seekGuild = gates;
            gameObject.SetActive(true);
            transform.SetAsLastSibling();
            bool showGuild = Interior && gates;
            guild.gameObject.SetActive(showGuild);
            chart.gameObject.SetActive(!showGuild); dossier.gameObject.SetActive(!showGuild);
            if (showGuild)
            {
                SizeRegister();
                return;
            }
            InkInterfaceArt.Set(landscape, Interior ? "MountainHall" : "RegionMap", false);
            places.Clear();
            for (int i = 0; i < current.PopulatedCount; i++)
                places.Add(current.BtnDataList[i]);
            // Keep the same place on the same bank when the player changes tab.
            places.Sort((a, b) => string.CompareOrdinal(a.dataSource.Id, b.dataSource.Id));
            int forests = 0;
            foreach (var place in places) if (ExpeditionPlace.IsForest(place.dataSource)) forests++;
            bool charted = !Interior && forests <= Forests.Length && places.Count - forests <= Towns.Length;
            int wood = 0, town = 0, used = 0;
            selected = null;
            var role = Singleton<RoleManager>.Instance.Role_Entity;
            foreach (var site in places)
            {
                var data = site.dataSource;
                bool forest = ExpeditionPlace.IsForest(data);
                int index = forest ? wood++ : town++;
                if (seekGuild && !forest) continue;
                if (used >= pins.Count) pins.Add(NewPin());
                var pin = pins[used];
                var pos = Interior && places.Count <= Depths.Length ? Depths[used] :
                    charted ? (forest ? Forests[index] : Towns[index]) : Grid(used, seekGuild ? forests : places.Count);
                var info = ExpeditionPlace.Read(data, role, seekGuild);
                pin.Site = site; pin.Info = info;
                pin.Root.gameObject.SetActive(true);
                pin.Root.name = "地标·" + data.Id;
                bool dense = !Interior && !seekGuild && places.Count > 6;
                float h = dense ? .18f : .22f;
                float w = dense ? .28f : .31f;
                // Keep edge labels inside the chart, even on compact portrait screens.
                pos.y = Mathf.Clamp(pos.y, h * .5f, 1 - h * .5f);
                Rect(pin.Root, pos.x - w*.5f, pos.y - h*.5f, pos.x + w*.5f, pos.y + h*.5f);
                InkInterfaceArt.Set(pin.Stamp, info.Art);
                pin.Stamp.color = info.Available || info.Here ? Color.white : new Color32(152, 143, 129, 175);
                pin.Name.text = CSZ_CityItemPanel.LocalMapName(data);
                pin.Status.text = info.Status;
                pin.Seal.color = info.Here ? new Color32(157, 62, 46, 255) :
                    info.Available ? new Color32(44, 92, 72, 255) : new Color32(102, 91, 80, 255);
                InkInterfaceArt.Set(pin.Plate, "BrushPlaque", false);
                pin.Name.color = Ink;
                pin.Status.color = Cream;
                pin.Glow.Bind(pin.Stamp, info.Available || info.Here ? Gold : CoolLight, !info.Available && !info.Here, used);
                if (previousId == data.Id || (previousId == null && (selected == null || info.Here))) selected = pin;
                used++;
            }
            for (int i = used; i < pins.Count; i++) pins[i].Root.gameObject.SetActive(false);
            empty.gameObject.SetActive(used == 0);
            empty.text = seekGuild ? "此地舆图未列出可查访的林地" : "此地暂无历练地点";
            if (selected == null && used > 0) selected = pins[0];
            if (selected != null) Select(selected, false);
            else { title.text = seekGuild ? "宗派山门" : "历练地图"; facts.text = description.text = ""; submit.gameObject.SetActive(false); sweep.gameObject.SetActive(false); }
        }

        private Pin NewPin()
        {
            var root = Box(chart, "地标");
            var face = root.gameObject.AddComponent<Image>();
            var button = root.gameObject.AddComponent<Button>(); button.targetGraphic = face; Touch(button);
            var art = RoleFolio.Image(root, "地貌", Color.white);
            Rect(art.rectTransform, .08f, .38f, .92f, 1);
            var plate = RoleFolio.Image(root, "地名签", Color.white);
            Rect(plate.rectTransform, 0, 0, 1, .44f);
            var name = Label(plate.transform, font, "", 15, Ink, TextAnchor.MiddleCenter, true);
            Style(name, 15, Ink, TextAnchor.MiddleCenter, true, 10);
            Rect(name.rectTransform, .04f,.43f,.96f,1);
            var seal = RoleFolio.Image(plate.transform, "通行印", Color.white);
            InkInterfaceArt.Set(seal, "BrushPlaque", false);
            Rect(seal.rectTransform, .15f, 0, .85f, .48f);
            var status = Label(seal.transform, font, "", 13, Cream, TextAnchor.MiddleCenter, true);
            Style(status, 13, Cream, TextAnchor.MiddleCenter, true, 10);
            Fill(status.rectTransform);
            var pin = new Pin { Root = root, Stamp = art, Plate = plate, Seal = seal, Name = name, Status = status,
                Glow = art.gameObject.AddComponent<ExpeditionGlow>() };
            button.onClick.AddListener(() => Select(pin, true));
            return pin;
        }

        private void Select(Pin pin, bool press)
        {
            if (selected != null)
            {
                InkInterfaceArt.Set(selected.Plate, "BrushPlaque", false);
                selected.Name.color = Ink;
            }
            selected = pin;
            if (press) pin.Glow.Press();
            var info = pin.Info;
            InkInterfaceArt.Set(pin.Plate, "InkPlaque", false);
            pin.Name.color = Cream;
            var data = pin.Site.dataSource;
            title.text = CSZ_CityItemPanel.LocalMapName(data);
            string range = ExpeditionPlace.Range(data);
            facts.text = info.Kind + (range.Length > 0 ? "  ·  " + range : "") +
                (data.FiveElement >= 1 && data.FiveElement <= 5 ? "  ·  " + WorldSetting.GetWuXingName((WuXing)data.FiveElement) + "行" : "");
            description.text = info.Detail;
            submit.gameObject.SetActive(true); submit.interactable = info.Available;
            submitText.text = info.Action;
            InkInterfaceArt.Set(submit.image, info.Available ? "InkPlaque" : "BrushPlaque", false);
            submitText.color = info.Available ? Cream : Muted;
            bool clean = Interior && pin.Site.ShaoDangBtn != null && pin.Site.ShaoDangBtn.gameObject.activeSelf && pin.Site.ShaoDangBtn.interactable;
            sweep.gameObject.SetActive(clean);
            Fixed((RectTransform)submit.transform, 1, -8, clean ? 6 : 20, 83, clean ? 33 : 44);
            Fixed((RectTransform)sweep.transform, 1, -8, 43, 83, 33);
        }

        private void Act(bool sweeping)
        {
            if (selected == null || selected.Site.dataSource == null) return;
            var info = ExpeditionPlace.Read(selected.Site.dataSource, Singleton<RoleManager>.Instance.Role_Entity, seekGuild);
            if (!info.Available) { Present(map, seekGuild); return; }
            var action = sweeping ? selected.Site.ShaoDangBtn : selected.Site.EnterMapBtn ?? selected.Site.GetComponent<Button>();
            if (action != null && action.isActiveAndEnabled && action.interactable) action.onClick.Invoke();
        }

        private static Vector2 Grid(int i, int total)
        {
            int columns = total > 4 ? 3 : 2, rows = (total + columns - 1) / columns;
            return new Vector2((i % columns + .5f) / columns, 1 - (i / columns + .5f) / rows);
        }

        private static void Bottom(RectTransform rt, float height)
        {
            rt.anchorMin = Vector2.zero; rt.anchorMax = Vector2.right;
            rt.offsetMin = Vector2.zero; rt.offsetMax = new Vector2(0, height);
        }

        private static void Rect(RectTransform rt, float left, float bottom, float right, float top)
        {
            rt.anchorMin = new Vector2(left, bottom); rt.anchorMax = new Vector2(right, top);
            rt.offsetMin = rt.offsetMax = Vector2.zero;
        }
    }
}
