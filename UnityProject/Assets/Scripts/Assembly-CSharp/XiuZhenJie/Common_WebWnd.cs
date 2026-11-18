using UnityEngine;

namespace XiuZhenJie
{
	public class Common_WebWnd : Window
	{
		public Common_WebPanel Panel;

		private UniWebView m_UniWebView;

		public override void Awake(params object[] paralist)
		{
			base.Awake(paralist);
			Panel = base.GameObject.GetComponent<Common_WebPanel>();
			if (paralist.Length != 0)
			{
				string url = paralist[0].ToString();
				if (Panel.WebRect.gameObject.GetComponent<UniWebView>() != null)
				{
					Object.Destroy(Panel.WebRect.gameObject.GetComponent<UniWebView>());
				}
				m_UniWebView = Panel.WebRect.gameObject.AddComponent<UniWebView>();
				m_UniWebView.SetShowToolbar(show: true);
				m_UniWebView.ReferenceRectTransform = Panel.WebRect;
				m_UniWebView.Load(url);
				m_UniWebView.Show();
				AddButtonClickListener(Panel.CloseBtn, CloseWindow);
			}
		}

		public override void CloseWindow()
		{
			m_UniWebView.Stop();
			m_UniWebView = null;
			base.CloseWindow();
		}
	}
}
