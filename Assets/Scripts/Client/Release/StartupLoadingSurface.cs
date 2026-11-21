using UnityEngine;
using UnityEngine.UI;

namespace XiuZhenJie.Client.Release
{
    public sealed class StartupLoadingSurface : MonoBehaviour
    {
        private static readonly Color Ink = new Color32(42, 57, 46, 255);
        private static readonly Color Jade = new Color32(58, 91, 73, 255);
        private static readonly Color Paper = new Color32(247, 239, 219, 255);
        private static readonly Color Vermilion = new Color32(145, 71, 52, 255);
        private static readonly int[] Wingbeat = { 0, 1, 2, 1 };

        private static float sessionOrigin = -1;
        private static Sprite white;
        private static Sprite mistBrush;
        private static Sprite paperWash;

        public Text Status { get; private set; }
        public Image Progress { get; private set; }
        public Button Retry { get; private set; }

        private RectTransform layout;
        private Image landscape;
        private Graphic title;
        private RectTransform farMist;
        private RectTransform nearMist;
        private readonly Image[] cranes = new Image[2];
        private Sprite[] wings;
        private Text percentage;
        private RectTransform tip;
        private RectTransform footer;
        private Vector2 lastSize;
        private Rect lastSafeArea;
        private float age;
        private bool failed;
        private bool titleSettled;
        private int lastPercent = -1;

        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.SubsystemRegistration)]
        private static void ResetSession()
        {
            sessionOrigin = -1;
        }

        public static StartupLoadingSurface Create(Transform root, Font font)
        {
            var node = new GameObject("山门行旅", typeof(RectTransform), typeof(StartupLoadingSurface));
            node.layer = root.gameObject.layer;
            node.transform.SetParent(root, false);
            var surface = node.GetComponent<StartupLoadingSurface>();
            surface.Build(font);
            return surface;
        }

        private void Build(Font font)
        {
            layout = (RectTransform)transform;
            Stretch(layout);
            if (font == null)
                font = Resources.GetBuiltinResource<Font>("LegacyRuntime.ttf");
            if (sessionOrigin < 0)
                sessionOrigin = Time.unscaledTime;
            // Both startup stages share the same flight phase; handing off never restarts the entrance.
            age = Time.unscaledTime - sessionOrigin;
            var art = Resources.Load<StartupLoadingArtwork>("StartupLoadingArtwork");
            PrepareBrushes();

            var ground = MakeImage(layout, "宣纸", Paper);
            Stretch(ground.rectTransform);
            landscape = MakeImage(layout, "山门晨景", Color.white, art != null ? art.landscape : null);
            landscape.enabled = landscape.sprite != null;
            landscape.rectTransform.anchorMin = landscape.rectTransform.anchorMax = new Vector2(.5f, .5f);

            farMist = Mist("远岫流云", .58f, .67f, new Color32(246, 239, 214, 109));
            nearMist = Mist("溪谷薄雾", .68f, .26f, new Color32(249, 238, 216, 116));
            wings = art != null ? art.wings : null;
            if (wings != null && wings.Length >= 3)
            {
                cranes[0] = MakeImage(layout, "远鹤", new Color32(110, 113, 98, 164), wings[0]);
                cranes[1] = MakeImage(layout, "近鹤", new Color32(118, 114, 97, 223), wings[0]);
                for (int i = 0; i < cranes.Length; i++)
                {
                    cranes[i].preserveAspect = true;
                    cranes[i].rectTransform.sizeDelta = i == 0 ? new Vector2(25, 18) : new Vector2(43, 31);
                }
            }

            if (art != null && art.logo != null)
            {
                var lettering = MakeImage(layout, "修真界题字", Ink, art.logo);
                lettering.preserveAspect = true;
                title = lettering;
            }
            else
                title = MakeText(layout, "修真界", font, 35, TextAnchor.MiddleCenter, Ink);
            title.rectTransform.anchorMin = new Vector2(.22f, .76f);
            title.rectTransform.anchorMax = new Vector2(.78f, .91f);
            title.rectTransform.offsetMin = title.rectTransform.offsetMax = Vector2.zero;

            var wash = MakeImage(layout, "书口留白", new Color32(248, 240, 219, 222), paperWash);
            wash.rectTransform.anchorMin = Vector2.zero;
            wash.rectTransform.anchorMax = new Vector2(1, .25f);
            wash.rectTransform.offsetMin = wash.rectTransform.offsetMax = Vector2.zero;

            footer = Node(layout, "加载信息");
            footer.pivot = new Vector2(.5f, 0);
            footer.anchorMin = new Vector2(.085f, 0);
            footer.anchorMax = new Vector2(.915f, 0);
            footer.sizeDelta = new Vector2(0, 145);

            Status = MakeText(footer, "ProgressText", font, 16, TextAnchor.MiddleLeft, Ink);
            Status.text = "正在准备游戏…";
            Status.resizeTextForBestFit = true;
            Status.resizeTextMinSize = 12;
            Status.resizeTextMaxSize = 16;
            Status.horizontalOverflow = HorizontalWrapMode.Wrap;
            Status.rectTransform.anchorMin = Vector2.zero;
            Status.rectTransform.anchorMax = new Vector2(.75f, 0);
            Status.rectTransform.offsetMin = new Vector2(0, 29);
            Status.rectTransform.offsetMax = new Vector2(-4, 79);

            percentage = MakeText(footer, "总进度", font, 18, TextAnchor.MiddleRight, Jade);
            percentage.fontStyle = FontStyle.Bold;
            percentage.rectTransform.anchorMin = new Vector2(.75f, 0);
            percentage.rectTransform.anchorMax = Vector2.right;
            percentage.rectTransform.offsetMin = new Vector2(0, 31);
            percentage.rectTransform.offsetMax = new Vector2(0, 76);

            var track = MakeImage(footer, "ProgressBar", new Color32(161, 157, 135, 189));
            track.rectTransform.anchorMin = Vector2.zero;
            track.rectTransform.anchorMax = Vector2.right;
            track.rectTransform.offsetMin = new Vector2(0, 26);
            track.rectTransform.offsetMax = new Vector2(0, 29);
            Progress = MakeImage(track.transform, "ProgressFill", Jade, white);
            Stretch(Progress.rectTransform);
            Progress.type = Image.Type.Filled;
            Progress.fillMethod = Image.FillMethod.Horizontal;
            Progress.fillOrigin = (int)Image.OriginHorizontal.Left;
            Progress.fillAmount = 0;

            tip = MakeImage(track.transform, "进度朱砂点", Vermilion).rectTransform;
            tip.sizeDelta = new Vector2(5, 5);
            tip.localRotation = Quaternion.Euler(0, 0, 45);
            var retryFace = MakeImage(footer, "Retry", Jade);
            retryFace.raycastTarget = true;
            retryFace.rectTransform.anchorMin = retryFace.rectTransform.anchorMax = new Vector2(.5f, 0);
            retryFace.rectTransform.sizeDelta = new Vector2(97, 36);
            retryFace.rectTransform.anchoredPosition = new Vector2(0, 108);
            Retry = retryFace.gameObject.AddComponent<Button>();
            Retry.targetGraphic = retryFace;
            var retryLabel = MakeText(retryFace.transform, "重试", font, 16, TextAnchor.MiddleCenter, Paper);
            Stretch(retryLabel.rectTransform);
            Retry.gameObject.SetActive(false);

            FitPage();
            DrawMotion();
            UpdateProgress();
        }

        public void SetFailure(bool active)
        {
            if (failed == active) return;
            if (!active)
                sessionOrigin = Time.unscaledTime - age;
            failed = active;
            Status.color = active ? Vermilion : Ink;
        }

        private RectTransform Mist(string name, float width, float y, Color color)
        {
            var rect = MakeImage(layout, name, color, mistBrush).rectTransform;
            rect.anchorMin = new Vector2(.5f - width * .5f, y - .049f);
            rect.anchorMax = new Vector2(.5f + width * .5f, y + .049f);
            rect.offsetMin = rect.offsetMax = Vector2.zero;
            return rect;
        }

        private void LateUpdate()
        {
            FitPage();
            if (!failed)
            {
                age = Time.unscaledTime - sessionOrigin;
                DrawMotion();
            }
            UpdateProgress();
        }

        private void OnRectTransformDimensionsChange()
        {
            // The game window is attached to its real portrait canvas after Awake.
            // Fit immediately so its first rendered frame already has the final framing.
            if (footer != null)
                FitPage();
        }

        private void FitPage()
        {
            var size = layout.rect.size;
            var safe = Screen.safeArea;
            if (size == lastSize && safe == lastSafeArea) return;
            lastSize = size;
            lastSafeArea = safe;
            if (landscape.sprite != null)
            {
                var art = landscape.sprite.rect.size;
                float cover = Mathf.Max(size.x / art.x, size.y / art.y);
                landscape.rectTransform.sizeDelta = art * (cover * 1.045f);
            }
            float inset = Screen.height > 0 ? size.y * safe.yMin / Screen.height : 0;
            footer.anchoredPosition = new Vector2(0, Mathf.Max(12, inset + 6));
        }

        private void DrawMotion()
        {
            float width = layout.rect.width;
            // A few pixels of parallax keep the illustrated stairway and pavilion anchored in the scene.
            landscape.rectTransform.anchoredPosition = new Vector2(
                Mathf.Sin(age * .18f) * Mathf.Min(3, width * .01f), Mathf.Sin(age * .13f) * 2);
            farMist.anchoredPosition = new Vector2(width * (.14f + .10f * Mathf.Sin(age * .22f)), 0);
            nearMist.anchoredPosition = new Vector2(width * (-.15f + .13f * Mathf.Sin(.6f - age * .26f)), 0);
            if (cranes[0] != null)
            {
                Fly(cranes[0], Mathf.Repeat(.26f + age / 18f, 1), .695f, age * 3.4f + 1);
                Fly(cranes[1], Mathf.Repeat(.42f + age / 13f, 1), .638f, age * 4.6f);
            }
            if (!titleSettled)
            {
                float reveal = Mathf.SmoothStep(0, 1, Mathf.Clamp01(age / .72f));
                title.canvasRenderer.SetAlpha(Mathf.Lerp(.68f, 1, reveal));
                title.rectTransform.anchoredPosition = new Vector2(0, (1 - reveal) * -7);
                titleSettled = reveal >= 1;
            }
        }

        private void Fly(Image bird, float flight, float altitude, float beat)
        {
            var at = new Vector2(Mathf.Lerp(1.10f, -.10f, flight),
                altitude + .035f * Mathf.Sin(flight * Mathf.PI) + .003f * Mathf.Sin(beat));
            bird.rectTransform.anchorMin = bird.rectTransform.anchorMax = at;
            bird.rectTransform.anchoredPosition = Vector2.zero;
            var frame = wings[Wingbeat[Mathf.FloorToInt(beat) % Wingbeat.Length]];
            if (bird.sprite != frame) bird.sprite = frame;
        }

        private void UpdateProgress()
        {
            float fraction = Progress.fillAmount;
            int percent = Mathf.FloorToInt(fraction * 100 + .0001f);
            if (lastPercent != percent)
            {
                lastPercent = percent;
                percentage.text = percent.ToString() + "%";
            }
            tip.anchorMin = tip.anchorMax = new Vector2(fraction, .5f);
            tip.anchoredPosition = Vector2.zero;
        }

        private static RectTransform Node(Transform parent, string name)
        {
            var go = new GameObject(name, typeof(RectTransform));
            go.layer = parent.gameObject.layer;
            var rect = (RectTransform)go.transform;
            rect.SetParent(parent, false);
            return rect;
        }

        private static void Stretch(RectTransform rect)
        {
            rect.anchorMin = Vector2.zero;
            rect.anchorMax = Vector2.one;
            rect.offsetMin = rect.offsetMax = Vector2.zero;
        }

        private static Image MakeImage(Transform parent, string name, Color color, Sprite art = null)
        {
            var image = Node(parent, name).gameObject.AddComponent<Image>();
            image.sprite = art;
            image.color = color;
            image.raycastTarget = false;
            return image;
        }

        private static Text MakeText(Transform parent, string name, Font font, int size, TextAnchor align, Color color)
        {
            var t = Node(parent, name).gameObject.AddComponent<Text>();
            t.font = font;
            t.text = name;
            t.fontSize = size;
            t.alignment = align;
            t.color = color;
            t.supportRichText = false;
            t.raycastTarget = false;
            return t;
        }

        private static void PrepareBrushes()
        {
            if (white != null) return;
            white = Sprite.Create(Texture2D.whiteTexture, new Rect(0, 0, 1, 1), new Vector2(.5f, .5f));
            white.name = "进度墨线";
            const int width = 128, height = 40;
            var pixels = new Color32[width * height];
            for (int y = 0; y < height; y++)
            {
                for (int x = 0; x < width; x++)
                {
                    float u = x / (width - 1f), v = y / (height - 1f);
                    float taper = Mathf.Sin(u * Mathf.PI);
                    float center = .5f + .11f * Mathf.Sin(u * 8f) + .05f * Mathf.Sin(u * 17f);
                    float band = Mathf.Max(0, 1 - Mathf.Abs(v - center) / (.09f + .32f * taper));
                    float alpha = taper * taper * Mathf.SmoothStep(0, 1, band);
                    pixels[y * width + x] = new Color32(255, 255, 255, (byte)(alpha * 255));
                }
            }
            mistBrush = MakeBrush("云带", width, height, pixels);
            var fade = new Color32[64];
            for (int i = 0; i < fade.Length; i++)
                fade[i] = new Color32(255, 255, 255, (byte)(255 * (1 - Mathf.SmoothStep(0, 1, i / 63f))));
            paperWash = MakeBrush("书口渐染", 1, fade.Length, fade);
        }

        private static Sprite MakeBrush(string name, int width, int height, Color32[] pixels)
        {
            var texture = new Texture2D(width, height, TextureFormat.RGBA32, false)
                { name = name, wrapMode = TextureWrapMode.Clamp, filterMode = FilterMode.Bilinear };
            texture.SetPixels32(pixels);
            texture.Apply(false, true);
            var sprite = Sprite.Create(texture, new Rect(0, 0, width, height), new Vector2(.5f, .5f),
                100, 0, SpriteMeshType.FullRect);
            sprite.name = name;
            return sprite;
        }
    }
}
