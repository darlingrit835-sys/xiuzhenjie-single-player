using System.Collections;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace XiuZhenJie
{
	public class NetMessageWnd : Window
	{
		private NetMessagePanel _panel;

		private UnityAction CloseCallBack;

		public override void Awake(params object[] paralist)
		{
			base.Awake(paralist);
			_panel = base.GameObject.GetComponent<NetMessagePanel>();
			StyleMessage();
			if (paralist.Length == 1)
			{
				_panel.MessageText.text = paralist[0].ToString();
			}
			AddButtonClickListener(_panel.CloseBtn, () =>
			{
				Singleton<UIManager>.Instance.CloseWnd("NetMessageWnd.prefab", destory: false);
				if (CloseCallBack != null)
				{
					CloseCallBack();
				}
			});
		}

		private void StyleMessage()
		{
			InkDialogArt.SetWash(_panel.transform);
			Text message = _panel.MessageText;
			var card = (RectTransform)message.transform.parent;
			card.sizeDelta = new Vector2(288f, 64f);
			InkDialogArt.SetPaper(card.GetComponent<Image>());

			message.color = InkInterfaceArt.Ink;
			message.alignment = TextAnchor.MiddleCenter;
			message.rectTransform.anchorMin = Vector2.zero;
			message.rectTransform.anchorMax = Vector2.one;
			message.rectTransform.offsetMin = new Vector2(18f, 6f);
			message.rectTransform.offsetMax = new Vector2(-52f, -6f);
			UIManager.FitTextToRect(message);
			message.resizeTextMinSize = 14;

			var close = (RectTransform)_panel.CloseBtn.transform;
			close.SetParent(card, false);
			close.anchorMin = close.anchorMax = new Vector2(1f, 0.5f);
			close.anchoredPosition = new Vector2(-24f, 0f);
			close.sizeDelta = new Vector2(30f, 30f);
			InkDialogArt.StyleButton(_panel.CloseBtn, "Close", InkInterfaceArt.Ink);
		}

		public void AddCloseCallBack(UnityAction _callBack)
		{
			CloseCallBack = _callBack;
		}

		public void ChangeValue(string value)
		{
			_panel.MessageText.text = value;
		}

		public IEnumerator Loading()
		{
			int i = 0;
			string old = _panel.MessageText.text;
			while (true)
			{
				if (i == 4)
				{
					_panel.MessageText.text = old;
				}
				else
				{
					_panel.MessageText.text += ".";
				}
				yield return new WaitForSeconds(1f);
			}
		}
	}
}
