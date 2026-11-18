using System;
using UnityEngine.UI;

namespace XiuZhenJie
{
	public class PopUpWnd : Window
	{
		public PopUpWndPanel popUpPanel;

		public Action SureBtnActionEvent;

		private int m_RowNum;

		public override void Awake(params object[] paralist)
		{
			base.Awake(paralist);
			popUpPanel = base.GameObject.GetComponent<PopUpWndPanel>();
			AddButtonClickListener(popUpPanel.CloseBtn, CloseThisWnd);
			AddButtonClickListener(popUpPanel.SureBtn, SureBtnClick);
		}

		public override void OnShow(params object[] paralist)
		{
			base.OnShow(paralist);
			if (paralist != null && paralist.Length != 0)
			{
				ZiShiYingHight(paralist[0].ToString());
			}
			else if (paralist != null && paralist.Length > 1)
			{
				popUpPanel.TitleText.text = paralist[1].ToString();
				ZiShiYingHight(paralist[0].ToString());
			}
			else if (paralist != null && paralist.Length > 2)
			{
				ZiShiYingHight(paralist[0].ToString());
				popUpPanel.TitleText.text = paralist[1].ToString();
				popUpPanel.SureBtnText.text = paralist[2].ToString();
			}
			else if (paralist != null && paralist.Length > 3)
			{
				ZiShiYingHight(paralist[0].ToString());
				popUpPanel.TitleText.text = paralist[1].ToString();
				popUpPanel.SureBtnText.text = paralist[2].ToString();
				SureBtnActionEvent = paralist[3] as Action;
			}
		}

		public void ZiShiYingHight(string contentText)
		{
			popUpPanel.ContentText.GetComponent<Text>().text = contentText;
		}

		public void SureBtnClick()
		{
			if (SureBtnActionEvent != null)
			{
				SureBtnActionEvent();
			}
			CloseThisWnd();
		}

		public void CloseThisWnd()
		{
			Singleton<UIManager>.Instance.CloseWnd("PopUpWnd.prefab");
		}
	}
}
