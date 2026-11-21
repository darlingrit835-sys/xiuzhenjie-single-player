using System;
using DG.Tweening;
using DG.Tweening.Core;
using DG.Tweening.Plugins.Options;
using UnityEngine;
using UnityEngine.Events;

namespace XiuZhenJie
{
	public class MaskWnd : Window
	{
		public MaskPanel _panel;

		public Color oldColor;

		public override void Awake(params object[] paralist)
		{
			base.Awake(paralist);
			_panel = base.GameObject.GetComponent<MaskPanel>();
			oldColor = _panel.MaskImg.color;
		}

		public override void OnUpdate()
		{
			base.OnUpdate();
			base.Transform.SetAsLastSibling();
		}

		public void ToBlack(float time, UnityAction unityAction, UnityAction showCompeleteAction = null)
		{
			_panel.MaskImg.color = new Color(oldColor.r, oldColor.g, oldColor.b, 0f);
			TweenerCore<Color, Color, ColorOptions> tweenerCore = _panel.MaskImg.DOColor(new Color(oldColor.r, oldColor.g, oldColor.b, 1f), time);
			tweenerCore.onComplete = (TweenCallback)Delegate.Combine(tweenerCore.onComplete, (TweenCallback)(() =>
			{
				unityAction?.Invoke();
			}));
		}

		public void ToShow(float time, UnityAction unityAction)
		{
			_panel.MaskImg.color = new Color(oldColor.r, oldColor.g, oldColor.b, 1f);
			TweenerCore<Color, Color, ColorOptions> tweenerCore = _panel.MaskImg.DOColor(new Color(oldColor.r, oldColor.g, oldColor.b, 0f), time);
			tweenerCore.onComplete = (TweenCallback)Delegate.Combine(tweenerCore.onComplete, (TweenCallback)(() =>
			{
				unityAction?.Invoke();
			}));
		}
	}
}
