using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace XiuZhenJie
{
    // The window owns the lesson. This view only draws it and leaves the lit control
    // in the original EventSystem path (including its press, release and scroll handlers).
    public sealed class PracticeGuideOverlay : MonoBehaviour
    {
        private static readonly int FocusRect = Shader.PropertyToID("_FocusRect");
        private static readonly int FocusMotion = Shader.PropertyToID("_FocusMotion");
        private static readonly int Drift = Shader.PropertyToID("_Drift");

        private RectTransform root, card, target;
        private Canvas canvas, targetCanvas;
        private RectMask2D[] clips;
        private Mask[] masks;
        private readonly Vector3[] corners = new Vector3[4];
        private Image shade, painting;
        private Material fog;
        private Text chapter, title, instruction, actionText, skipText;
        private Button primary, skip;
        private UnityAction onContinue, onSkip;
        private Rect lastSafe, lastFocus;
        private float openedAt;
        private int artStage;
        private bool modal;

        public static PracticeGuideOverlay Create(RectTransform parent, Font font)
        {
            var host = new GameObject("PracticeGuideOverlay", typeof(RectTransform));
            var rect = (RectTransform)host.transform;
            rect.SetParent(parent, false);
            host.layer = parent.gameObject.layer;
            Stretch(rect);
            var view = host.AddComponent<PracticeGuideOverlay>();
            view.Build(rect, font);
            view.Hide();
            return view;
        }

        private void Build(RectTransform rect, Font font)
        {
            root = rect;
            canvas = rect.GetComponentInParent<Canvas>().rootCanvas;
            if (!font)
                font = ReleasedResources.Load<GameObject>("prefab/window/StartLoadSceneWnd")?.GetComponentInChildren<Text>(true)?.font;
            shade = Graphic<Image>("InkMist", root);
            Stretch(shade.rectTransform);
            shade.raycastTarget = true;
            var filter = shade.gameObject.AddComponent<PracticeGuideHitFilter>();
            filter.View = this;
            var shader = ReleasedResources.Load<Shader>("shaders/GuideFog");
            if (shader)
            {
                fog = new Material(shader);
                fog.SetFloat("_ButtonLight", 0.18f);
                shade.material = fog;
            }
            else
                shade.color = new Color(0.05f, 0.09f, 0.08f, 0.45f);

            var paper = Graphic<Image>("IllustratedScroll", root);
            card = paper.rectTransform;
            card.anchorMin = card.anchorMax = root.pivot;
            card.pivot = Vector2.zero;
            InkInterfaceArt.Set(paper, "LightPaper", false);
            painting = Graphic<Image>("Landscape", card);
            SetChapterArt(1);
            Stretch(painting.rectTransform);
            painting.rectTransform.offsetMin = new Vector2(3, 3);
            painting.rectTransform.offsetMax = new Vector2(-3, -3);

            chapter = Label("Chapter", card, font, 14, InkInterfaceArt.Seal);
            title = Label("Title", card, font, 24, InkInterfaceArt.Ink);
            instruction = Label("Instruction", card, font, 17, InkInterfaceArt.Ink);
            instruction.horizontalOverflow = HorizontalWrapMode.Wrap;
            instruction.lineSpacing = 1.08f;
            primary = MakeButton("Continue", font, true, out actionText);
            skip = MakeButton("Skip", font, false, out skipText);
            primary.onClick.AddListener(() => onContinue?.Invoke());
            skip.onClick.AddListener(() => onSkip?.Invoke());
        }

        public void SetChapterArt(int stage)
        {
            stage = Mathf.Clamp(stage, 1, 3);
            if (stage == artStage)
                return;
            artStage = stage;
            string key = stage == 1 ? "PracticeGuideScroll" : stage == 2 ? "PracticeGuideStudy" : "PracticeGuideBreath";
            painting.sprite = InkInterfaceArt.Get(key);
            painting.enabled = painting.sprite != null;
        }

        public void Show(string chapter, string title, string instruction, RectTransform target,
            UnityAction onSkip)
        {
            SetPage(chapter, title, instruction, target, false, null, null, onSkip, "跳过");
        }

        public void ShowMessage(string chapter, string title, string instruction, string actionLabel,
            UnityAction onContinue, UnityAction onSkip, string skipLabel = "跳过")
        {
            SetPage(chapter, title, instruction, null, true, actionLabel, onContinue, onSkip, skipLabel);
        }

        private void SetPage(string chapterName, string heading, string body, RectTransform control, bool isModal,
            string actionLabel, UnityAction nextAction, UnityAction skipAction, string skipLabel)
        {
            target = control;
            targetCanvas = control ? control.GetComponentInParent<Canvas>()?.rootCanvas : null;
            clips = control ? control.GetComponentsInParent<RectMask2D>() : null;
            masks = control ? control.GetComponentsInParent<Mask>() : null;
            chapter.text = chapterName;
            title.text = heading;
            instruction.text = body;
            actionText.text = actionLabel;
            skipText.text = skipLabel;
            onContinue = nextAction;
            onSkip = skipAction;
            primary.gameObject.SetActive(nextAction != null && !string.IsNullOrEmpty(actionLabel));
            skip.gameObject.SetActive(skipAction != null);
            modal = isModal;
            openedAt = Time.unscaledTime;
            gameObject.SetActive(true);
            transform.SetAsLastSibling();
            Refresh(true);
        }

        public void Hide()
        {
            gameObject.SetActive(false);
        }

        private void OnDisable()
        {
            onContinue = null;
            onSkip = null;
            target = null;
        }

        private void OnDestroy()
        {
            if (fog)
                Destroy(fog);
        }

        private void LateUpdate() => Refresh(false);

        // Only this Graphic rejects the raycast in the opening. The card's own actions
        // stay usable, and a dialogue above the real control still handles input normally.
        internal bool Blocks(Vector2 point)
        {
            if (modal)
                return true;
            if (!target || !target.gameObject.activeInHierarchy || lastFocus.width <= 0)
                return false;
            return !lastFocus.Contains(point) ||
                !RectTransformUtility.RectangleContainsScreenPoint(target, point, CameraOf(targetCanvas));
        }

        private void Refresh(bool force)
        {
            Rect safe = Overlap(Screen.safeArea, ScreenBounds(root, canvas));
            if (safe.width < 1 || safe.height < 1)
                return;

            Rect focus = default;
            if (target && target.gameObject.activeInHierarchy)
            {
                focus = Overlap(ScreenBounds(target, targetCanvas), safe);
                foreach (RectMask2D clip in clips)
                    if (clip && clip.isActiveAndEnabled)
                        focus = Overlap(focus, ScreenBounds(clip.rectTransform, targetCanvas));
                foreach (Mask mask in masks)
                    if (mask && mask.isActiveAndEnabled)
                        focus = Overlap(focus, ScreenBounds(mask.rectTransform, targetCanvas));
            }
            bool lit = focus.width > 0 && focus.height > 0;
            shade.enabled = modal || lit;
            if (force || safe != lastSafe || focus != lastFocus)
            {
                lastSafe = safe;
                lastFocus = focus;
                if (fog)
                {
                    float w = Mathf.Max(1, Screen.width);
                    float h = Mathf.Max(1, Screen.height);
                    fog.SetVector(FocusRect, lit ? new Vector4(focus.xMin / w, focus.yMin / h,
                        focus.xMax / w, focus.yMax / h) : Vector4.zero);
                }
                PlaceCard(ToLocal(safe), lit ? ToLocal(focus) : default, lit);
            }
            if (fog && shade.enabled)
            {
                float elapsed = Time.unscaledTime - openedAt;
                float pulse = 0.5f + 0.5f * Mathf.Sin(elapsed * 3.1f);
                float angle = elapsed * 1.8f;
                fog.SetVector(FocusMotion, new Vector4(Mathf.SmoothStep(0.78f, 1f, elapsed / 0.25f),
                    pulse, Mathf.Cos(angle), Mathf.Sin(angle)));
                fog.SetFloat(Drift, Time.unscaledTime);
            }
        }

        private Rect ScreenBounds(RectTransform rect, Canvas owner)
        {
            rect.GetWorldCorners(corners);
            Vector2 min = new Vector2(float.PositiveInfinity, float.PositiveInfinity);
            Vector2 max = new Vector2(float.NegativeInfinity, float.NegativeInfinity);
            Camera cam = CameraOf(owner);
            foreach (var corner in corners)
            {
                Vector2 p = RectTransformUtility.WorldToScreenPoint(cam, corner);
                min = Vector2.Min(min, p);
                max = Vector2.Max(max, p);
            }
            return Rect.MinMaxRect(min.x, min.y, max.x, max.y);
        }

        private static Camera CameraOf(Canvas owner) =>
            owner && owner.renderMode != RenderMode.ScreenSpaceOverlay ? owner.worldCamera : null;

        private Rect ToLocal(Rect screen)
        {
            Camera cam = CameraOf(canvas);
            RectTransformUtility.ScreenPointToLocalPointInRectangle(root, screen.min, cam, out var min);
            RectTransformUtility.ScreenPointToLocalPointInRectangle(root, screen.max, cam, out var max);
            return Rect.MinMaxRect(min.x, min.y, max.x, max.y);
        }

        private static Rect Overlap(Rect a, Rect b)
        {
            Vector2 min = Vector2.Max(a.min, b.min);
            Vector2 max = Vector2.Min(a.max, b.max);
            return max.x <= min.x || max.y <= min.y ? default : Rect.MinMaxRect(min.x, min.y, max.x, max.y);
        }

        private void PlaceCard(Rect safe, Rect focus, bool lit)
        {
            const float margin = 12, gap = 14;
            float width = Mathf.Max(1, Mathf.Min(modal ? 600 : 440, safe.width - 2 * margin));
            float pad = width < 360 ? 14 : 22;
            float textWidth = Mathf.Max(1, width * (width < 360 ? 0.83f : 0.67f) - pad);
            title.fontSize = modal ? 28 : 22;
            instruction.fontSize = modal ? 18 : 16;
            instruction.rectTransform.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, textWidth);
            float bodyHeight = Mathf.Ceil(instruction.preferredHeight + 3);
            float titleHeight = title.fontSize + 10;
            float actionsHeight = primary.gameObject.activeSelf || skip.gameObject.activeSelf ? 40 : 0;
            float height = Mathf.Max(width / 3f, pad + 22 + titleHeight + 4 + bodyHeight + actionsHeight + pad);
            height = Mathf.Min(height, Mathf.Max(1, safe.height - margin * 2));
            card.sizeDelta = new Vector2(width, height);
            float top = height - pad;
            Box(chapter.rectTransform, pad, top - 19, width - 2 * pad, 19);
            Box(title.rectTransform, pad, top - 20 - titleHeight, width - pad * 2, titleHeight);
            float bodyTop = top - 20 - titleHeight - 3;
            Box(instruction.rectTransform, pad, pad + actionsHeight, textWidth,
                Mathf.Max(1, bodyTop - pad - actionsHeight));

            float primaryWidth = Mathf.Max(104, actionText.preferredWidth + 30);
            float skipWidth = Mathf.Max(70, skipText.preferredWidth + 22);
            Box((RectTransform)primary.transform, pad, pad - 2, primaryWidth, 34);
            Box((RectTransform)skip.transform, pad + (primary.gameObject.activeSelf ? primaryWidth + 10 : 0),
                pad - 2, skipWidth, 34);

            Vector2 center = safe.center;
            if (!modal && !lit)
                center.y = safe.yMin + margin + height * 0.5f;
            if (lit)
            {
                // Prefer above/below the target so the instruction reads in tap order.
                // Near the edges, try both sides before falling back to the roomier half.
                center = focus.center;
                float above = safe.yMax - focus.yMax;
                float below = focus.yMin - safe.yMin;
                if (below >= height + gap + margin)
                    center.y = focus.yMin - gap - height * 0.5f;
                else if (above >= height + gap + margin)
                    center.y = focus.yMax + gap + height * 0.5f;
                else if (safe.xMax - focus.xMax >= width + gap + margin)
                    center.x = focus.xMax + gap + width * 0.5f;
                else if (focus.xMin - safe.xMin >= width + gap + margin)
                    center.x = focus.xMin - gap - width * 0.5f;
                else
                    center.y = above >= below ? safe.yMax - margin - height * 0.5f : safe.yMin + margin + height * 0.5f;
            }
            card.anchoredPosition = new Vector2(
                Mathf.Clamp(center.x - width * 0.5f, safe.xMin + margin, safe.xMax - width - margin),
                Mathf.Clamp(center.y - height * 0.5f, safe.yMin + margin, safe.yMax - height - margin));
        }

        private Button MakeButton(string name, Font font, bool emphasized, out Text caption)
        {
            var plaque = Graphic<Image>(name, card);
            InkInterfaceArt.Set(plaque, emphasized ? "SealButton" : "LightPlaque", false);
            plaque.raycastTarget = true;
            var button = plaque.gameObject.AddComponent<Button>();
            button.targetGraphic = plaque;
            caption = Label("Label", plaque.rectTransform, font, 16, emphasized ? Color.white : InkInterfaceArt.SoftInk);
            caption.alignment = TextAnchor.MiddleCenter;
            Stretch(caption.rectTransform);
            return button;
        }

        private static T Graphic<T>(string name, RectTransform parent) where T : Graphic
        {
            var go = new GameObject(name, typeof(RectTransform), typeof(CanvasRenderer));
            go.layer = parent.gameObject.layer;
            var rect = (RectTransform)go.transform;
            rect.SetParent(parent, false);
            rect.anchorMin = rect.anchorMax = rect.pivot = Vector2.zero;
            var graphic = go.AddComponent<T>();
            graphic.raycastTarget = false;
            return graphic;
        }

        private static Text Label(string name, RectTransform parent, Font font, int size, Color color)
        {
            var label = Graphic<Text>(name, parent);
            label.font = font;
            label.fontSize = size;
            label.color = color;
            label.supportRichText = false;
            label.alignment = TextAnchor.MiddleLeft;
            label.horizontalOverflow = HorizontalWrapMode.Overflow;
            label.verticalOverflow = VerticalWrapMode.Truncate;
            return label;
        }

        private static void Box(RectTransform rect, float x, float y, float width, float height)
        {
            rect.anchoredPosition = new Vector2(x, y);
            rect.sizeDelta = new Vector2(width, height);
        }

        private static void Stretch(RectTransform rect)
        {
            rect.anchorMin = Vector2.zero;
            rect.anchorMax = Vector2.one;
            rect.offsetMin = rect.offsetMax = Vector2.zero;
        }
    }

    internal sealed class PracticeGuideHitFilter : MonoBehaviour, ICanvasRaycastFilter
    {
        internal PracticeGuideOverlay View;
        public bool IsRaycastLocationValid(Vector2 point, Camera eventCamera) => View && View.Blocks(point);
    }

}
