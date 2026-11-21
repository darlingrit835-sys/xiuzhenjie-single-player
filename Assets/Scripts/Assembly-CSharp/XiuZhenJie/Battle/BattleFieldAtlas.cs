using System.Text;
using UnityEngine;
using UnityEngine.UI;

namespace XiuZhenJie
{
    public sealed partial class BattleFieldView
    {
        RectTransform atlasParent, atlasSheet, atlasChart;
        GameObject atlasVeil;
        RectTransform atlasPlayer;
        Text atlasSubtitle, atlasPlaces;
        Font atlasFont;
        Texture2D atlasSurvey;
        RectTransform[] atlasPins;
        int listedSites = -1;
        static readonly Color32[] Cartography = {
            new Color32(115,121,118,255), new Color32(220,196,137,255), new Color32(104,151,122,255),
            new Color32(166,151,130,255), new Color32(105,171,179,255), new Color32(187,144,110,255),
            new Color32(162,159,177,255)
        };

        void EnableAtlas(RectTransform container)
        {
            atlasParent = container.parent.parent as RectTransform;
            atlasFont = atlasParent.GetComponentInChildren<Text>(true).font;
            miniImage.raycastTarget = true;
            var open = miniImage.gameObject.AddComponent<Button>();
            open.targetGraphic = miniImage;
            open.transition = Selectable.Transition.None;
            open.onClick.AddListener(OpenAtlas);
        }

        void OpenAtlas()
        {
            if (!atlasVeil) BuildAtlas();
            atlasVeil.SetActive(true);
            atlasVeil.transform.SetAsLastSibling();
            DrawAtlas();
        }

        void BuildAtlas()
        {
            var veil = InkBattleHud.Make<Image>(atlasParent,"战场大地图");
            InkDialogArt.Inset(veil.rectTransform,0,0,0,0);
            veil.color = new Color(.04f,.09f,.10f,.78f);
            atlasVeil = veil.gameObject;
            atlasSheet = InkBattleHud.Make<Image>(veil.transform,"舆图纸页").rectTransform;
            InkDialogArt.SetPaper(atlasSheet.GetComponent<Image>(),"InkLedgerPaper");
            var title = InkBattleHud.Label(atlasSheet,atlasFont,"抬头","山川舆图",24,InkInterfaceArt.Ink);
            title.fontStyle = FontStyle.Bold;
            InkBattleHud.Band(title.rectTransform,18,60,9,34);
            atlasSubtitle = InkBattleHud.Label(atlasSheet,atlasFont,"进度","",13,new Color32(78,94,78,255));
            InkBattleHud.Band(atlasSubtitle.rectTransform,19,16,46,19);
            var close = InkBattleHud.Make<Image>(atlasSheet,"收图").gameObject.AddComponent<Button>();
            InkBattleHud.Place((RectTransform)close.transform,new Vector2(1,1),-9,-8,44,44);
            close.targetGraphic = close.GetComponent<Image>();
            InkDialogArt.StyleButton(close,"Close",InkInterfaceArt.Ink);
            close.onClick.AddListener(()=>atlasVeil.SetActive(false));
            var chart = InkBattleHud.Make<RawImage>(atlasSheet,"已勘地形");
            atlasSurvey = Texture(layout.Width,layout.Height,"勘绘地貌",FilterMode.Bilinear);
            var ink = atlasSurvey.GetRawTextureData<Color32>();
            for (int cell = 0; cell < explored.Length; cell++) ink[cell] = SurveyColor(cell);
            atlasSurvey.Apply(false);
            chart.texture = atlasSurvey;
            var material = Own(new Material(ReleasedResources.Load<Shader>("shaders/BattleAtlas")));
            material.SetTexture("_CloudInk",ReleasedResources.Load<Texture2D>("BattleField/CloudInk"));
            var types = Own(new Texture2D(layout.Width,layout.Height,TextureFormat.Alpha8,false,true));
            types.filterMode = FilterMode.Point;
            types.wrapMode = TextureWrapMode.Clamp;
            types.LoadRawTextureData(layout.Tiles);
            types.Apply(false,true);
            material.SetTexture("_Ground",types);
            chart.material = material;
            chart.raycastTarget = false;
            atlasChart = chart.rectTransform;
            atlasPins = new RectTransform[vistas.Length];
            for (int i = 0; i < atlasPins.Length; i++)
            {
                var pin = InkBattleHud.Make<Image>(atlasChart,"地标印记");
                pin.color = new Color32(126,58,42,255);
                pin.raycastTarget = false;
                int cell = vistas[i].Site.Cell;
                PlaceMapPin(pin.rectTransform,cell,16);
                var number = InkBattleHud.Label(pin.transform,atlasFont,"序号",(i+1).ToString(),12,InkDialogArt.Ivory);
                number.fontStyle = FontStyle.Bold;
                number.alignment = TextAnchor.MiddleCenter;
                InkDialogArt.Inset(number.rectTransform,0,0,0,0);
                atlasPins[i] = pin.rectTransform;
            }
            var compass = InkBattleHud.Label(atlasChart,atlasFont,"指北","北",12,InkDialogArt.Ivory);
            InkBattleHud.Place(compass.rectTransform,Vector2.one,-6,-3,18,18);
            atlasPlayer = InkBattleHud.Make<Image>(atlasChart,"所在位置").rectTransform;
            atlasPlayer.GetComponent<Image>().color = new Color32(255,221,138,255);
            atlasPlayer.GetComponent<Image>().raycastTarget = false;
            atlasPlayer.localRotation = Quaternion.Euler(0,0,45);
            atlasPlaces = InkBattleHud.Label(atlasSheet,atlasFont,"地标名录","",14,InkInterfaceArt.Ink);
            atlasPlaces.alignment = TextAnchor.UpperLeft;
            var key = InkBattleHud.Label(atlasSheet,atlasFont,"图例","金菱 · 我的位置　　 朱印 · 已发现地标",12,new Color32(78,89,78,255));
            InkBattleHud.Place(key.rectTransform,new Vector2(0,0),18,9,290,20);
        }

        Color32 SurveyColor(int cell)
        {
            if (explored[cell] == 0) return new Color32(0,0,0,0);
            Color32 ink = Cartography[layout.Tiles[cell]];
            ink.a = visible[cell] ? (byte)255 : (byte)181;
            return ink;
        }

        void UpdateSurvey()
        {
            if (!atlasSurvey || dirtyMini.Count == 0) return;
            var ink = atlasSurvey.GetRawTextureData<Color32>();
            foreach (int cell in dirtyMini) ink[cell] = SurveyColor(cell);
            atlasSurvey.Apply(false);
        }

        void PlaceMapPin(RectTransform pin, int cell, float size)
        {
            var anchor = new Vector2((cell % layout.Width + .5f) / layout.Width,(cell / layout.Width + .5f) / layout.Height);
            InkBattleHud.Place(pin,anchor,0,0,size,size);
            pin.pivot = new Vector2(.5f,.5f);
        }

        void DrawAtlas()
        {
            float width = Mathf.Min(438,atlasParent.rect.width - 28);
            float available = Mathf.Max(85,atlasParent.rect.height - 192);
            float aspect = (float)layout.Width / layout.Height;
            float chartWidth = Mathf.Min(width - 28, available * aspect);
            float chartHeight = chartWidth / aspect;
            InkBattleHud.Place(atlasSheet,new Vector2(.5f,.5f),0,0,width,chartHeight + 147);
            InkBattleHud.Place(atlasChart,new Vector2(.5f,1),0,-70,chartWidth,chartHeight);
            var p = layout.Index(observer.position);
            if (p >= 0) PlaceMapPin(atlasPlayer,p,6);
            atlasSubtitle.text = $"已探 {ExploredPercent}%　　地标 {DiscoveredLandmarks} / {vistas.Length}";
            if (listedSites != DiscoveredLandmarks)
            {
                listedSites = DiscoveredLandmarks;
                var names = new StringBuilder();
                for (int i = 0; i < vistas.Length; i++)
                {
                    atlasPins[i].gameObject.SetActive(vistas[i].Discovered);
                    if (!vistas[i].Discovered) continue;
                    if (names.Length > 0) names.Append("　 ");
                    names.Append(i+1).Append(" · ").Append(LandmarkNames[vistas[i].Site.Kind-1]);
                }
                atlasPlaces.text = names.ToString();
            }
            InkBattleHud.Band(atlasPlaces.rectTransform,18,18,chartHeight + 79,58);
        }
    }
}
