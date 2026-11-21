using System;
using UnityEngine;
using UnityEngine.UI;

namespace XiuZhenJie
{
    // The guild registers and exchange dialogs still use their original serialized controls.
    internal static class ArtisanArchiveArt
    {
        private static readonly Color32 Gold = new Color32(210, 166, 91, 255);
        private static readonly Color32 Ivory = new Color32(255, 240, 207, 255);
        private static readonly Sprite[] Plates = new Sprite[3];
        private static readonly Sprite[] Scenes = new Sprite[2];

        internal static void Apply(GameObject root, string window)
        {
            bool garden = window.StartsWith("LZ_", StringComparison.Ordinal) || window.StartsWith("LT_", StringComparison.Ordinal);
            if (!garden && !window.StartsWith("SFH_", StringComparison.Ordinal) &&
                !window.StartsWith("HuaFu", StringComparison.Ordinal)) return;

            // A nine-sliced edge leaves variable-length captions, scroll regions and drawing targets intact.
            Transform panel = root.transform;
            StyleHeading(panel.Find("Title_image"), garden);
            StyleHeading(panel.Find("Bg/Title_image"), garden);
            StyleHeading(panel.Find("Bg/Title"), garden);
            foreach (Button button in root.GetComponentsInChildren<Button>(true))
            {
                var label = button.transform.Find("Text")?.GetComponent<Text>();
                if (label == null || string.IsNullOrWhiteSpace(label.text) || label.text.Length < 2 ||
                    button.image == null || button.image.rectTransform.rect.width < 42f ||
                    button.transition == Selectable.Transition.SpriteSwap) continue;
                SetPlate(button.image, garden ? 1 : 0);
                label.color = Ivory;
            }

            if (window == "SFH_ShenFuHuiWnd.prefab" || window == "LZ_MianWnd.prefab")
                StyleAssociation(panel, garden);
        }

        private static void StyleHeading(Transform bar, bool garden)
        {
            if (bar == null || !bar.TryGetComponent(out Image plaque)) return;
            SetPlate(plaque, garden ? 1 : 0);
            foreach (Transform item in bar)
                if (item.TryGetComponent(out Text label)) { label.color = Ivory; label.fontStyle = FontStyle.Bold; }
        }

        private static void Frame(RectTransform rect, float left, float bottom, float right, float top)
        {
            rect.anchorMin = new Vector2(left, bottom);
            rect.anchorMax = new Vector2(right, top);
            rect.offsetMin = rect.offsetMax = Vector2.zero;
        }

        private static void StyleAssociation(Transform root, bool garden)
        {
            var art = root.Find("CityBg_image")?.GetComponent<Image>();
            if (art == null) return;
            int index = garden ? 1 : 0;
            if (Scenes[index] == null)
            {
                var tex = ReleasedResources.Load<Texture2D>(garden ? "Artisan11/GardenHall" : "Artisan11/SigilHall");
                if (tex != null)
                {
                    int w = Mathf.RoundToInt(tex.height * .78f);
                    float x = Mathf.Clamp(.65f * tex.width - w * .5f, 0, tex.width - w);
                    Scenes[index] = Sprite.Create(tex, new Rect(x, 0, w, tex.height), Vector2.one * .5f);
                }
            }
            if (Scenes[index] != null)
            {
                art.sprite = Scenes[index]; art.type = Image.Type.Simple; art.preserveAspect = false;
                art.color = Color.white;
            }
            Frame(art.rectTransform, 0, 0, 1, .866f);
            var description = art.transform.Find("Des")?.GetComponent<Image>();
            if (description != null)
            {
                Frame(description.rectTransform, .035f, .04f, .965f, .30f);
                SetPlate(description, 2);
                var title = description.transform.Find("Title_text")?.GetComponent<Text>();
                var body = description.transform.Find("Des_text")?.GetComponent<Text>();
                if (title != null)
                {
                    Frame(title.rectTransform, .05f,.68f,.95f,.94f);
                    title.color = InkInterfaceArt.Ink; title.alignment = TextAnchor.MiddleLeft;
                }
                if (body != null)
                {
                    Frame(body.rectTransform, .05f,.08f,.95f,.67f);
                    body.color = InkInterfaceArt.Ink; body.alignment = TextAnchor.MiddleLeft;
                    body.resizeTextForBestFit = true; body.resizeTextMinSize = 10; body.resizeTextMaxSize = 15;
                }
            }

            string[] routes = { "MoNiChuangZao", "DuiHuan", "ZhuCe" };
            for (int i = 0; i < routes.Length; i++)
            {
                var rect = art.transform.Find(routes[i]) as RectTransform;
                if (rect == null) continue;
                float l = .037f + i * .315f;
                Frame(rect, l,.335f,l+.296f,.455f);
                if (rect.TryGetComponent(out Image button)) SetPlate(button, garden ? 1 : 0);
                var originalMark = rect.Find("Image")?.GetComponent<Image>();
                if (originalMark != null) originalMark.enabled = false;
                var caption = rect.Find("Text")?.GetComponent<Text>();
                if (caption != null)
                {
                    Frame(caption.rectTransform,.04f,.06f,.96f,.94f);
                    int subtitle = caption.text.IndexOf('\n');
                    if (subtitle > 0) caption.text = caption.text.Substring(0, subtitle);
                    caption.alignment = TextAnchor.MiddleCenter; caption.color = Ivory;
                    caption.resizeTextForBestFit = true; caption.resizeTextMinSize = 11; caption.resizeTextMaxSize = 17;
                }
            }
        }

        private static void SetPlate(Image image, int style)
        {
            if (Plates[style] == null) Plates[style] = MakePlate(style);
            image.sprite = Plates[style]; image.type = Image.Type.Sliced; image.preserveAspect = false; image.color = Color.white;
        }

        private static Sprite MakePlate(int style)
        {
            const int n = 64;
            Color top = style == 2 ? new Color32(251, 238, 203, 255) :
                style == 1 ? new Color32(50, 90, 74, 255) : new Color32(113, 54, 42, 255);
            Color bottom = style == 2 ? new Color32(220, 197, 151, 255) :
                style == 1 ? new Color32(21, 52, 45, 255) : new Color32(64, 30, 31, 255);
            var pixels = new Color32[n*n];
            for (int y=0; y<n; y++)
                for (int x=0; x<n; x++)
                {
                    int dx = Math.Min(x,n-1-x), dy = Math.Min(y,n-1-y), edge = Math.Min(dx,dy);
                    Color color = Color.Lerp(bottom, top, (float)y/(n-1));
                    if (edge == 1 || (edge == 4 && (dx < 13 || dy < 13))) color = Gold;
                    if ((dx == 8 && dy >= 7 && dy <= 13) || (dy == 8 && dx >= 7 && dx <= 13))
                        color = Color.Lerp(color, Gold, .80f);
                    if (dx+dy < 4) color.a = 0;
                    pixels[y*n+x] = color;
                }
            var texture = new Texture2D(n,n,TextureFormat.RGBA32,false) { wrapMode = TextureWrapMode.Clamp, filterMode = FilterMode.Bilinear };
            texture.SetPixels32(pixels); texture.Apply(false,true);
            return Sprite.Create(texture,new Rect(0,0,n,n),Vector2.one*.5f,100,0,SpriteMeshType.FullRect,Vector4.one*14);
        }
    }
}
