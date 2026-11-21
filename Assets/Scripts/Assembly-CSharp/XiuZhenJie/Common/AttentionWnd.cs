using System;
using UnityEngine;
using UnityEngine.UI;

namespace XiuZhenJie
{
	public class AttentionWnd : Window
	{
		public AttentionWndPanel Panel;

		public Action m_QueRenBtnActionNoParams;

		public Action m_CloseWndAction;

		private Text heading;
		private string defaultHeading;
		private bool defaultRichText;
		private Button declineButton;
		private float center;
		private Vector2 originalSize;

		public override void Awake(params object[] paralist)
		{
			Panel = base.GameObject.GetComponent<AttentionWndPanel>();
			InkDialogArt.StyleAttention(Panel);
			heading = Panel.ContentText.transform.parent.Find("Title")?.GetComponentInChildren<Text>(true);
			defaultHeading = heading?.text;
			defaultRichText = Panel.ContentText.supportRichText;
			var button = (RectTransform)Panel.QueRenBtn.transform;
			center = button.anchoredPosition.x;
			originalSize = button.sizeDelta;
			AddButtonClickListener(Panel.CloseBtn, ClostBtn);
			AddButtonClickListener(Panel.QueRenBtn, QueRenBtn);
		}

		public override void OnShow(params object[] paralist)
		{
			base.OnShow(paralist);
			if (paralist != null && paralist.Length > 0)
			{
				ChangeValue(paralist[0]?.ToString(), paralist.Length > 1 ? paralist[1] as Action : null,
					paralist.Length > 2 ? paralist[2] as Action : null, paralist.Length > 3 ? paralist[3] as string : null);
			}
		}

		public void ChangeValue(string text, Action _action, Action _closeAction = null, string title = null)
		{
			RestoreActions();
			Panel.ContentText.supportRichText = defaultRichText;
			if (heading != null)
				heading.text = string.IsNullOrEmpty(title) ? defaultHeading : title;
			Panel.ContentText.text = text;
			m_QueRenBtnActionNoParams = _action;
			m_CloseWndAction = _closeAction;
		}

		internal void SetChoices(string accept, string decline, Action onDecline)
		{
			var primary = (RectTransform)Panel.QueRenBtn.transform;
			if (declineButton == null)
				declineButton = primary.parent.Find("DeclineChoice")?.GetComponent<Button>();
			if (declineButton == null)
			{
				declineButton = UnityEngine.Object.Instantiate(Panel.QueRenBtn, primary.parent, false);
				declineButton.name = "DeclineChoice";
			}
			declineButton.onClick = new Button.ButtonClickedEvent();
			declineButton.onClick.AddListener(() =>
			{
				Singleton<UIManager>.Instance.CloseWnd(this);
				onDecline();
			});
			declineButton.gameObject.SetActive(true);
			Panel.ContentText.supportRichText = false;
			DaoYouItem.StyleAction(Panel.QueRenBtn, accept, primary: true);
			DaoYouItem.StyleAction(declineButton, decline);
			primary.sizeDelta = new Vector2(104f, 34f);
			((RectTransform)declineButton.transform).sizeDelta = primary.sizeDelta;
			((RectTransform)declineButton.transform).anchoredPosition = new Vector2(center - 58f, primary.anchoredPosition.y);
			primary.anchoredPosition = new Vector2(center + 58f, primary.anchoredPosition.y);
			// The dedicated decline control is the only negative action in this choice.
			Panel.CloseBtn.gameObject.SetActive(false);
		}

		private void RestoreActions()
		{
			if (declineButton == null)
				return;
			var primary = (RectTransform)Panel.QueRenBtn.transform;
			primary.anchoredPosition = new Vector2(center, primary.anchoredPosition.y);
			primary.sizeDelta = originalSize;
			DaoYouItem.StyleAction(Panel.QueRenBtn, "确定", primary: true);
			declineButton.gameObject.SetActive(false);
			Panel.CloseBtn.gameObject.SetActive(true);
		}

		public override void OnClose()
		{
			RestoreActions();
			base.OnClose();
		}

		public void ClostBtn()
		{
			Singleton<UIManager>.Instance.CloseWnd("AttentionWnd.prefab", destory: false);
			if (m_CloseWndAction != null)
			{
				m_CloseWndAction();
			}
		}

		public void QueRenBtn()
		{
			Singleton<UIManager>.Instance.CloseWnd("AttentionWnd.prefab", destory: false);
			if (m_QueRenBtnActionNoParams != null)
			{
				m_QueRenBtnActionNoParams();
			}
		}

		public override void OnUpdate()
		{
			base.OnUpdate();
			base.Transform.SetAsLastSibling();
		}
	}
}
