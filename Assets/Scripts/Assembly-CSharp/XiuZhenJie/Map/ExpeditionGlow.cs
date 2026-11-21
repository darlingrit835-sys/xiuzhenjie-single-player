using UnityEngine;
using UnityEngine.UI;

namespace XiuZhenJie
{
    // One shared soft brush. Only illustrations and light move, leaving type and touch targets still.
    internal sealed class ExpeditionGlow : MonoBehaviour
    {
        private static Sprite brush, ripple;
        private Image light, ring;
        private RectTransform stamp;
        private Color tint;
        private float phase, strength, pressed = -10;
        private bool quiet;

        internal void Bind(Image art, Color hue, bool subdued, int order)
        {
            stamp = art.rectTransform;
            if (light == null)
            {
                ring = RoleFolio.Image(stamp, "地脉涟漪", Color.clear);
                ring.sprite = Brush(true);
                ring.rectTransform.anchorMin = ring.rectTransform.anchorMax = new Vector2(.5f, .18f);
                ring.rectTransform.sizeDelta = new Vector2(44, 11);
                light = RoleFolio.Image(stamp, "灵光", Color.clear);
                light.sprite = Brush(false);
                light.rectTransform.sizeDelta = new Vector2(23, 12);
            }
            tint = hue;
            quiet = subdued;
            phase = order * 1.73f;
            strength = quiet ? .34f : .85f;
            Sample();
        }

        internal void Press() { pressed = Time.unscaledTime; }
        private void Update() { if (stamp != null) Sample(); }
        private void Sample()
        {
            float time = Time.unscaledTime;
            float beat = time * (quiet ? 1.12f : 1.65f) + phase;
            float wave = Mathf.Sin(beat);
            float touch = Mathf.Clamp01(1f - (time - pressed) / .32f);
            stamp.anchoredPosition = new Vector2(0, wave * (quiet ? .45f : 1.35f));
            stamp.localScale = Vector3.one * (1 + .012f * wave - .055f * touch);
            float glide = Mathf.Sin(beat * .61f);
            light.rectTransform.anchorMin = light.rectTransform.anchorMax = new Vector2(.48f + .24f * glide, .45f + .09f * wave);
            float spread = Mathf.Repeat((time + phase) / (quiet ? 4.4f : 3f), 1);
            if (touch > 0) spread = 1 - touch;
            ring.rectTransform.localScale = Vector3.one * (.65f + .60f * spread);
            ring.color = new Color(tint.r, tint.g, tint.b, Mathf.Sin(spread * Mathf.PI) * (quiet ? .22f : .68f));
            light.color = new Color(tint.r, tint.g, tint.b, Mathf.Min(1, strength * (.55f + .4f * wave) + touch));
        }

        private static Sprite Brush(bool outline)
        {
            Sprite cached = outline ? ripple : brush;
            if (cached != null) return cached;
            const int size = 48;
            var tex = new Texture2D(size, size, TextureFormat.RGBA32, false);
            var pixels = new Color32[size * size];
            for (int i = 0; i < pixels.Length; i++)
            {
                float x = (i % size + .5f) * 2 / size - 1, y = (i / size + .5f) * 2 / size - 1;
                float r = Mathf.Sqrt(x*x + y*y);
                float a = Mathf.Pow(Mathf.Max(0, 1 - r), 2) + .42f * Mathf.Exp(-38 * r * r);
                if (outline) a = Mathf.Clamp01(1 - Mathf.Abs(r - .85f) * 9);
                pixels[i] = new Color(1, 1, 1, Mathf.Clamp01(a));
            }
            tex.wrapMode = TextureWrapMode.Clamp;
            tex.SetPixels32(pixels); tex.Apply(false, true);
            var sprite = Sprite.Create(tex, new Rect(0, 0, size, size), Vector2.one * .5f);
            if (outline) ripple = sprite; else brush = sprite;
            return sprite;
        }
    }
}
