using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;
using static XiuZhenJie.RoleFolio;

namespace XiuZhenJie
{
    // The labelled markers forward to the bound map controls, including their level checks and server requests.
    internal sealed class TravelLandmarks : MonoBehaviour
    {
        private sealed class Marker
        {
            internal RectTransform Root;
            internal RectTransform Art;
            internal Button Hit;
            internal Image Stamp;
            internal Image Nameplate;
            internal Text Name;
            internal Text Range;
            internal Text Here;
            internal Button Sweep;
            internal CSZ_CityItemPanel Place;
        }

        private readonly List<Marker> markers = new List<Marker>();
        private RectTransform view;
        private CanvasGroup visibility;
        private Text empty;
        private Font font;
        private Marker selected;
        private Tween arrival;
        private const string ViewName = "行旅地标";

        internal static void Hide(CSZ_MapPanel panel)
        {
            var existing = panel.transform.Find(ViewName);
            if (existing == null || !existing.gameObject.activeSelf) return;
            existing.gameObject.SetActive(false);
            foreach (var site in panel.BtnDataList)
                if (site.TryGetComponent(out CanvasGroup group)) { group.alpha = 1; group.blocksRaycasts = true; }
        }

        internal static void Show(CSZ_MapPanel panel, bool returning = false)
        {
            if (!panel.TryGetComponent(out TravelLandmarks map)) map = panel.gameObject.AddComponent<TravelLandmarks>();
            map.Present(panel, returning);
        }

        private void Present(CSZ_MapPanel panel, bool returning)
        {
            if (view == null)
            {
                // A recursive map level can be instantiated from a previously populated template.
                Transform inherited = transform.Find(ViewName);
                if (inherited != null) { inherited.name += "(旧)"; inherited.gameObject.SetActive(false); Destroy(inherited.gameObject); }
                font = GetComponentInParent<WorldMapPanel>().TitleText.font;
                view = Box(transform, ViewName); Fill(view);
                view.gameObject.AddComponent<LayoutElement>().ignoreLayout = true;
                visibility = view.gameObject.AddComponent<CanvasGroup>();
                empty = Label(view, font, "暂无可前往的地点", 16, Ink, TextAnchor.MiddleCenter); Fill(empty.rectTransform);
            }
            view.gameObject.SetActive(true);
            view.SetAsLastSibling();
            foreach (var site in panel.BtnDataList)
            {
                if (!site.TryGetComponent(out CanvasGroup group)) group = site.gameObject.AddComponent<CanvasGroup>();
                group.alpha = 0; group.blocksRaycasts = false;
            }
            int count = panel.PopulatedCount;
            empty.gameObject.SetActive(count == 0);
            while (markers.Count < count) markers.Add(CreateMarker());
            if (!returning) selected = null;
            for (int i = 0; i < markers.Count; i++)
            {
                var marker = markers[i];
                marker.Root.gameObject.SetActive(i < count);
                if (i >= count) { marker.Place = null; continue; }
                marker.Place = panel.BtnDataList[i];
                MapEntity place = marker.Place.dataSource;
                marker.Root.name = "地标·" + place.Id;
                PlaceMarker(marker.Root, i, count);
                string name = CSZ_CityItemPanel.LocalMapName(place);
                marker.Name.text = name;
                marker.Range.text = place.MaxLevel <= 0 && place.MinLevel <= 0 ? "" :
                    place.MaxLevel >= 999 || place.MaxLevel < place.MinLevel ? $"{Mathf.Max(1,place.MinLevel)} 级起" :
                    $"{Mathf.Max(1,place.MinLevel)}–{Mathf.Max(1,place.MaxLevel)} 级";
                var sweep = marker.Place.ShaoDangBtn;
                marker.Sweep.gameObject.SetActive(sweep != null && sweep.gameObject.activeSelf && sweep.interactable);
                string illustration = ArtFor(place);
                InkInterfaceArt.Set(marker.Stamp, illustration);
                AtlasEaveLight.Set(marker.Stamp, marker.Place.IsCurrentLocation &&
                    illustration != "FoldedMap" && illustration != "Swords");
                marker.Hit.interactable = marker.Place.GetComponent<Button>().interactable;
                SetTint(marker, marker == selected);
            }
            arrival?.Kill(); visibility.alpha = .76f;
            arrival = visibility.DOFade(1, .23f).SetEase(Ease.OutQuad).SetUpdate(true);
        }

        private Marker CreateMarker()
        {
            var root = Box(view, "地标");
            var surface = root.gameObject.AddComponent<Image>();
            var hit = root.gameObject.AddComponent<Button>(); hit.targetGraphic = surface; Touch(hit);
            var art = Box(root, "地标图文"); Fill(art);
            var stamp = RoleFolio.Image(art, "地点插画", Color.white);
            var nameplate = RoleFolio.Image(art, "题签", new Color32(248,243,223,239));
            var lowerEdge = RoleFolio.Image(nameplate.transform, "纸边", new Color32(115,102,69,104));
            lowerEdge.rectTransform.anchorMin = new Vector2(.05f, 0);
            lowerEdge.rectTransform.anchorMax = new Vector2(.95f, 0);
            lowerEdge.rectTransform.pivot = new Vector2(.5f, 0);
            lowerEdge.rectTransform.sizeDelta = new Vector2(0, 1);
            var title = Label(art,font,"地点",15,Ink,TextAnchor.MiddleCenter,true);
            var range = Label(art,font,"境界",12,Secondary,TextAnchor.MiddleCenter);
            Style(title,15,Ink,TextAnchor.MiddleCenter,true,11);
            var seal = RoleFolio.Image(art, "所在标记", new Color32(229,237,214,251));
            var here = Label(seal.transform,font,"所在",11,Jade,TextAnchor.MiddleCenter,true);
            var sweepFace=RoleFolio.Image(root,"扫荡",new Color32(232,235,214,255),true);
            var sweep=sweepFace.gameObject.AddComponent<Button>();sweep.targetGraphic=sweepFace;
            var sweepText=Label(sweepFace.transform,font,"扫荡",13,Jade,TextAnchor.MiddleCenter,true);Fill(sweepText.rectTransform);
            Place(stamp.rectTransform,.21f,.42f,.79f,.98f);
            Place(nameplate.rectTransform,.01f,.025f,.99f,.46f);
            Place(title.rectTransform,.035f,.20f,.965f,.465f);Place(range.rectTransform,.025f,.025f,.975f,.22f);
            Place(seal.rectTransform,.68f,.73f,.99f,.98f);Fill(here.rectTransform);
            Place(sweepFace.rectTransform,.20f,-.30f,.80f,0f);
            var marker=new Marker{Root=root,Art=art,Hit=hit,Stamp=stamp,Nameplate=nameplate,Name=title,Range=range,Here=here,Sweep=sweep};
            hit.onClick.AddListener(()=>Choose(marker,false));sweep.onClick.AddListener(()=>Choose(marker,true));
            return marker;
        }

        private void Choose(Marker marker,bool sweep)
        {
            if (marker.Place?.dataSource == null) return;
            if(selected!=null) SetTint(selected,false);
            selected=marker;SetTint(marker,true);
            marker.Art.DOKill();marker.Art.localScale=Vector3.one*.972f;
            marker.Art.DOScale(1,.16f).SetUpdate(true).SetEase(Ease.OutQuad);
            var action = sweep ? marker.Place.ShaoDangBtn : marker.Place.EnterMapBtn;
            if (!sweep && action == null) action = marker.Place.GetComponent<Button>();
            if(action!=null && action.interactable) action.onClick.Invoke();
        }

        private static void SetTint(Marker marker,bool chosen)
        {
            marker.Nameplate.color=chosen?new Color32(219,231,207,253):new Color32(248,243,223,239);
            marker.Name.color=chosen?Jade:Ink;
            marker.Here.transform.parent.gameObject.SetActive(marker.Place.IsCurrentLocation);
        }

        private static string ArtFor(MapEntity map)
        {
            if(map.Depth==1) return "FoldedMap";
            if(map.Depth==2) return "Pavilion";
            if(map.Depth==3) return "Town";
            switch((MapLevelType)map.MapType){case MapLevelType.CITY_SUPER:return "CityGate";case MapLevelType.CITY_STATE:return "Town";
                case MapLevelType.CITY_PREFECTURE:return "Pavilion";case MapLevelType.MASSIF:case MapLevelType.MASSIFAREA:
                case MapLevelType.MASSIF_INHERIT_SECRET:case MapLevelType.MASSIF_GOODS_SECRET:return "Swords";default:return "Village";}
        }

        private static void PlaceMarker(RectTransform rt,int i,int count)
        {
            int columns=count>6?3:(count>1?2:1),rows=(count+columns-1)/columns,row=i/columns;
            int lastCount=Mathf.Min(columns,count-row*columns),column=i%columns;
            float x=.5f+(column-(lastCount-1)*.5f)*(columns==3?.315f:.44f);
            float y=.50f+((rows-1)*.5f-row)*Mathf.Min(.26f,.88f/rows);
            if(count>1)y+=(column%2==0?.02f:-.02f);
            float w=columns==3?.29f:columns==2?.38f:.47f;
            float h=Mathf.Min(.225f,.80f/rows);
            Place(rt,x-w*.5f,y-h*.5f,x+w*.5f,y+h*.5f);
        }

        private static void Place(RectTransform rt,float left,float bottom,float right,float top)
        {
            rt.anchorMin=new Vector2(left,bottom);rt.anchorMax=new Vector2(right,top);rt.offsetMin=rt.offsetMax=Vector2.zero;
        }

        private void OnDisable()
        {
            arrival?.Kill();if(visibility!=null)visibility.alpha=1;
            foreach(var m in markers){m.Art.DOKill();m.Art.localScale=Vector3.one;}
        }
    }
}
