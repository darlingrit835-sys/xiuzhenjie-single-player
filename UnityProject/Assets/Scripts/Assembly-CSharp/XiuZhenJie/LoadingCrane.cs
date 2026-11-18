using UnityEngine;
using UnityEngine.UI;

namespace XiuZhenJie
{
    [RequireComponent(typeof(Image))]
    public sealed class LoadingCrane : MonoBehaviour
    {
        [SerializeField] private Sprite[] frames;
        private static readonly int[] Wingbeat = { 0, 1, 2, 1 };
        private Image image;
        private RectTransform rect;
        private float openedAt;

        internal static void AddTo(Transform root, Image mask, Image background)
        {
            Sprite landscape = Resources.Load<Sprite>("InkLoadingBackground");
            if (background != null && landscape != null)
            {
                background.sprite = landscape;
                background.type = Image.Type.Simple;
                background.color = Color.white;
                if (background.transform != root)
                {
                    RectTransform backdrop = background.rectTransform;
                    backdrop.anchorMin = Vector2.zero;
                    backdrop.anchorMax = Vector2.one;
                    backdrop.offsetMin = backdrop.offsetMax = Vector2.zero;
                }
            }
            GameObject prefab = Resources.Load<GameObject>("prefab/LoadingCrane");
            if (prefab == null)
                return;

            GameObject crane = Instantiate(prefab, root, false);
            if (mask != null && mask.transform.parent == root)
                crane.transform.SetSiblingIndex(mask.transform.GetSiblingIndex());
        }

        private void Awake()
        {
            image = GetComponent<Image>();
            rect = image.rectTransform;
        }

        private void OnEnable()
        {
            openedAt = Time.unscaledTime;
            Update();
        }

        private void Update()
        {
            float elapsed = Time.unscaledTime - openedAt;
            float flight = Mathf.Repeat(0.27f + elapsed / 7.5f, 1f);
            float x = Mathf.Lerp(1.24f, -0.24f, flight);
            float y = 0.77f + 0.045f * Mathf.Sin(flight * Mathf.PI) + 0.007f * Mathf.Sin(elapsed * 3.6f);
            rect.anchorMin = rect.anchorMax = new Vector2(x, y);
            int index = Wingbeat[Mathf.FloorToInt(elapsed * 8f) % Wingbeat.Length];
            image.sprite = frames[index];
        }
    }
}
