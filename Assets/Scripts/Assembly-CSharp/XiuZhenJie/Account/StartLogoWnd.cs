using System;
using DG.Tweening;
using DG.Tweening.Core;
using DG.Tweening.Plugins.Options;
using UnityEngine;

namespace XiuZhenJie
{
	public class StartLogoWnd : Window
	{
		public StartLogoPanel m_panel;

		private int times;

		public override void Awake(params object[] paralist)
		{
			base.Awake(paralist);
			m_panel = base.GameObject.GetComponent<StartLogoPanel>();
		}

		public override void OnShow(params object[] paralist)
		{
			base.OnShow(paralist);
			Singleton<UIManager>.Instance.PopUpWnd("StartLoadSceneWnd.prefab", true, true);
			Singleton<UIManager>.Instance.CloseWnd("StartLogoWnd.prefab");
		}

		private void StartChange()
		{
			TweenerCore<Color, Color, ColorOptions> tweenerCore = m_panel.Mask.DOColor(new Color(m_panel.Mask.color.r, m_panel.Mask.color.g, m_panel.Mask.color.b, 0f), 1.5f);
			tweenerCore.onComplete = (TweenCallback)Delegate.Combine(tweenerCore.onComplete, (TweenCallback)(() =>
			{
				Timer.Register(1.5f, () =>
				{
					EndColorChange();
				});
			}));
		}

		private void EndColorChange()
		{
			TweenerCore<Color, Color, ColorOptions> tweenerCore = m_panel.Mask.DOColor(new Color(m_panel.Mask.color.r, m_panel.Mask.color.g, m_panel.Mask.color.b, 1f), 1.5f);
			tweenerCore.onComplete = (TweenCallback)Delegate.Combine(tweenerCore.onComplete, (TweenCallback)(() =>
			{
				Singleton<UIManager>.Instance.PopUpWnd("StartLoadSceneWnd.prefab", true, true);
				Singleton<UIManager>.Instance.CloseWnd("StartLogoWnd.prefab");
			}));
		}
	}
}
