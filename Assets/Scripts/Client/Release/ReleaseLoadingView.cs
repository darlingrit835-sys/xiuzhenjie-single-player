using System;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace XiuZhenJie.Client.Release
{
    internal sealed class ReleaseLoadingView : MonoBehaviour
    {
        [SerializeField] private Font font = null;

        private StartupLoadingSurface surface;
        private GameObject input;
        private Action onRetry;

        private void Start()
        {
            var page = new GameObject("LoadingPage", typeof(RectTransform), typeof(Canvas),
                typeof(CanvasScaler), typeof(GraphicRaycaster));
            page.transform.SetParent(transform, false);
            var canvas = page.GetComponent<Canvas>();
            canvas.renderMode = RenderMode.ScreenSpaceOverlay;
            canvas.sortingOrder = 100;
            var scaler = page.GetComponent<CanvasScaler>();
            scaler.uiScaleMode = CanvasScaler.ScaleMode.ScaleWithScreenSize;
            scaler.referenceResolution = new Vector2(1334, 750);
            scaler.matchWidthOrHeight = 1f;

            surface = StartupLoadingSurface.Create(page.transform, font);
            surface.Retry.onClick.AddListener(Retry);
            surface.Retry.gameObject.SetActive(false);

            // The bootstrap scene has no EventSystem. The game scene supplies its own after handoff.
            input = new GameObject("RetryInput");
            input.SetActive(false);
            input.transform.SetParent(transform, false);
            input.AddComponent<EventSystem>();
            input.AddComponent<StandaloneInputModule>();
            gameObject.AddComponent<ReleaseBootstrap>().Initialize(this);
        }

        internal void Show(string message, float value)
        {
            if (onRetry != null)
                SetRetry(null);
            surface.SetFailure(false);
            surface.Status.text = message;
            surface.Progress.fillAmount = Mathf.Clamp01(value);
        }

        internal void ShowFailure(string message, Action retryAction)
        {
            surface.Status.text = message;
            surface.SetFailure(true);
            SetRetry(retryAction);
        }

        private void SetRetry(Action action)
        {
            onRetry = action;
            bool visible = action != null;
            surface.Retry.gameObject.SetActive(visible);
            input.SetActive(visible);
        }

        private void Retry()
        {
            Action action = onRetry;
            if (action == null)
                return;
            SetRetry(null);
            action();
        }
    }
}
