using System.Collections;
using UnityEngine;
using UnityEngine.Events;

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
