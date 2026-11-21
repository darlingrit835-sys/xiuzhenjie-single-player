using System;
using UnityEngine;
using UnityEngine.UI;

namespace XiuZhenJie
{
	public class PopUpWnd : Window
	{
		public PopUpWndPanel popUpPanel;

		public Action SureBtnActionEvent;

		private string defaultTitle;
		private string defaultConfirm;

		public override void Awake(params object[] paralist)
		{
			base.Awake(paralist);
			popUpPanel = base.GameObject.GetComponent<PopUpWndPanel>();
			InkDialogArt.StylePopup(popUpPanel);
			defaultTitle = popUpPanel.TitleText.text;
			defaultConfirm = popUpPanel.SureBtnText.text;
			AddButtonClickListener(popUpPanel.CloseBtn, CloseThisWnd);
			AddButtonClickListener(popUpPanel.SureBtn, SureBtnClick);
		}

		public override void OnShow(params object[] paralist)
		{
			base.OnShow(paralist);
			object[] args = paralist ?? Array.Empty<object>();
			// Crafting windows pass (copy, callback, title, row hint). Other windows can pass (copy, title, button, callback).
			bool rules = args.Length >= 3 && (args[1] == null || args[1] is Action);
			popUpPanel.TitleText.text = rules ? args[2] as string ?? defaultTitle
				: args.Length > 1 ? args[1] as string ?? defaultTitle : defaultTitle;
			popUpPanel.SureBtnText.text = !rules && args.Length > 2 ? args[2] as string ?? defaultConfirm : defaultConfirm;
			SureBtnActionEvent = rules ? args[1] as Action : args.Length > 3 ? args[3] as Action : null;
			ZiShiYingHight(args.Length > 0 ? args[0]?.ToString() : string.Empty);
		}

		public void ZiShiYingHight(string contentText)
		{
			var copy = popUpPanel.ContentText.GetComponent<Text>();
			copy.text = contentText ?? string.Empty;
			LayoutRebuilder.ForceRebuildLayoutImmediate(popUpPanel.ContentParent);
			float available = ((RectTransform)Transform).rect.height - 32f;
			float height = Mathf.Clamp(copy.preferredHeight + 156f, 186f, Mathf.Max(186f, available));
			popUpPanel.ContentParent.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, height);
			ScrollRect scroll = popUpPanel.ContentScrollView.GetComponent<ScrollRect>();
			scroll.StopMovement();
			scroll.verticalNormalizedPosition = 1f;
		}

		public void SureBtnClick()
		{
			Action confirm = SureBtnActionEvent;
			CloseThisWnd();
			confirm?.Invoke();
		}

		public void CloseThisWnd()
		{
			Singleton<UIManager>.Instance.CloseWnd("PopUpWnd.prefab");
		}
	}
}
