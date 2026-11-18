using System;
using DG.Tweening;
using UnityEngine;

namespace XiuZhenJie
{
    internal sealed class InkWindowMotion : MonoBehaviour
    {
        private CanvasGroup group;
        private Tween tween;
        private Action release;
        private Vector3 restingScale;
        private float restingAlpha;
        private bool blocksRaycasts;
        private bool interactable;

        internal static bool AppliesTo(string name, GameObject view)
        {
            if (view == null || !view.activeInHierarchy || !(view.transform is RectTransform))
                return false;
            switch (name)
            {
                case "StartLogoWnd.prefab":
                case "StartLoadSceneWnd.prefab":
                case "LoadingWnd.prefab":
                case "NetMessageWnd.prefab":
                case "GuideWnd.prefab":
                case "GuideNewWnd.prefab":
                case "MainTitleWnd.prefab":
                case "MainCityWnd.prefab":
                case "BattleWnd.prefab":
                    return false;
                default:
                    return true;
            }
        }

        internal static InkWindowMotion For(GameObject view)
        {
            return view.TryGetComponent(out InkWindowMotion motion) ? motion : view.AddComponent<InkWindowMotion>();
        }

        private void Awake()
        {
            if (!TryGetComponent(out group))
                group = gameObject.AddComponent<CanvasGroup>();
            CaptureRestingState();
        }

        internal void Open()
        {
            Stop();
            CaptureRestingState();
            group.alpha = 0f;
            transform.localScale = restingScale * 0.983f;
            Play(restingAlpha, restingScale, 0.22f, Ease.OutCubic);
        }

        internal void Close(Action complete)
        {
            Stop();
            release = complete;
            group.blocksRaycasts = false;
            group.interactable = false;
            Play(0f, restingScale * 0.989f, 0.16f, Ease.InQuad);
        }

        private void Play(float alpha, Vector3 scale, float seconds, Ease ease)
        {
            tween = DOTween.Sequence()
                .Append(group.DOFade(alpha, seconds).SetEase(ease))
                .Join(transform.DOScale(scale, seconds).SetEase(ease))
                .SetUpdate(true)
                .OnComplete(() =>
                {
                    tween = null;
                    RestoreAndRelease();
                });
        }

        private void CaptureRestingState()
        {
            restingScale = transform.localScale;
            restingAlpha = group.alpha;
            blocksRaycasts = group.blocksRaycasts;
            interactable = group.interactable;
        }

        private void Stop()
        {
            tween?.Kill();
            tween = null;
        }

        private void Restore()
        {
            group.alpha = restingAlpha;
            group.blocksRaycasts = blocksRaycasts;
            group.interactable = interactable;
            transform.localScale = restingScale;
        }

        private void OnDisable()
        {
            Stop();
            RestoreAndRelease();
        }

        private void RestoreAndRelease()
        {
            Action completed = release;
            release = null;
            if (group != null)
                Restore();
            completed?.Invoke();
        }
    }
}
