using UnityEngine;
using UnityEngine.UI;

namespace XiuZhenJie
{
    // Shared typography and geometry for the role sheet and its independently bound profession row.
    internal static class RoleFolio
    {
        internal static readonly Color Paper = new Color32(249, 245, 232, 255);
        internal static readonly Color Card = new Color32(242, 237, 219, 255);
        internal static readonly Color Jade = new Color32(53, 86, 67, 255);
        internal static readonly Color Ink = new Color32(42, 54, 46, 255);
        internal static readonly Color Secondary = new Color32(77, 86, 72, 255);
        internal static readonly Color Rule = new Color32(207, 202, 179, 255);

        internal static RectTransform Box(Transform parent, string name)
        {
            var go = new GameObject(name, typeof(RectTransform));
            go.layer = parent.gameObject.layer;
            var rect = (RectTransform)go.transform;
            rect.SetParent(parent, false);
            return rect;
        }

        internal static Image Image(Transform parent, string name, Color color, bool hit = false)
        {
            var image = Box(parent, name).gameObject.AddComponent<Image>();
            image.color = color;
            image.raycastTarget = hit;
            return image;
        }

        internal static void Flat(Image image, Color color, bool hit = false)
        {
            if (image == null) return;
            image.overrideSprite = null;
            image.sprite = null;
            image.type = UnityEngine.UI.Image.Type.Simple;
            image.color = color;
            image.raycastTarget = hit;
        }

        internal static Text Label(Transform parent, Font font, string text, int size, Color color,
            TextAnchor alignment = TextAnchor.MiddleLeft, bool bold = false)
        {
            var t = Box(parent, text).gameObject.AddComponent<Text>();
            t.font = font;
            t.text = text;
            Style(t, size, color, alignment, bold);
            return t;
        }

        internal static void Style(Text t, int size, Color color, TextAnchor alignment, bool bold = false, int minimum = 0)
        {
            t.fontSize = size;
            t.fontStyle = bold ? FontStyle.Bold : FontStyle.Normal;
            t.color = color;
            t.alignment = alignment;
            t.resizeTextForBestFit = minimum > 0;
            if (minimum > 0)
            {
                t.resizeTextMinSize = minimum;
                t.resizeTextMaxSize = size;
            }
            t.horizontalOverflow = HorizontalWrapMode.Wrap;
            t.verticalOverflow = VerticalWrapMode.Truncate;
            t.raycastTarget = false;
            t.supportRichText = false;
        }

        internal static void Fill(RectTransform rect, float left = 0, float right = 0, float top = 0, float bottom = 0)
        {
            InkDialogArt.Inset(rect, left, right, top, bottom);
        }

        internal static void Band(RectTransform rect, float left, float right, float top, float height)
        {
            rect.anchorMin = Vector2.up;
            rect.anchorMax = Vector2.one;
            rect.offsetMin = new Vector2(left, -top - height);
            rect.offsetMax = new Vector2(-right, -top);
        }

        internal static void Fixed(RectTransform rect, float anchorX, float dx, float top, float width, float height)
        {
            rect.pivot = new Vector2(anchorX, 1);
            rect.anchorMin = rect.anchorMax = new Vector2(anchorX, 1);
            rect.sizeDelta = new Vector2(width, height);
            rect.anchoredPosition = new Vector2(dx, -top);
        }

        internal static void Span(RectTransform rect, float start, float end, float left = 0, float right = 0)
        {
            rect.anchorMin = new Vector2(start, 0);
            rect.anchorMax = new Vector2(end, 1);
            rect.offsetMin = new Vector2(left, 0);
            rect.offsetMax = new Vector2(-right, 0);
        }

        internal static void Height(RectTransform rect, float height)
        {
            var layout = rect.GetComponent<LayoutElement>() ?? rect.gameObject.AddComponent<LayoutElement>();
            layout.minHeight = layout.preferredHeight = height;
            layout.flexibleHeight = 0;
        }

        internal static void Touch(Button button)
        {
            Flat(button.image, Color.white, true);
            if (button.image == null)
                button.targetGraphic = button.gameObject.AddComponent<Image>();
            button.transition = Selectable.Transition.ColorTint;
            var colors = ColorBlock.defaultColorBlock;
            colors.normalColor = colors.selectedColor = colors.disabledColor = Color.clear;
            colors.highlightedColor = Color.clear;
            colors.pressedColor = new Color32(79, 103, 77, 30);
            colors.fadeDuration = .06f;
            button.colors = colors;
            button.image.canvasRenderer.SetColor(Color.clear);
        }
    }
}
