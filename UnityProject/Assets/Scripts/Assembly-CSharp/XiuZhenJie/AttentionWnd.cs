using System;

namespace XiuZhenJie
{
	public class AttentionWnd : Window
	{
		public AttentionWndPanel Panel;

		public Action m_QueRenBtnActionNoParams;

		public Action m_CloseWndAction;

		public override void Awake(params object[] paralist)
		{
			Panel = base.GameObject.GetComponent<AttentionWndPanel>();
			AddButtonClickListener(Panel.CloseBtn, ClostBtn);
			AddButtonClickListener(Panel.QueRenBtn, QueRenBtn);
		}

		public override void OnShow(params object[] paralist)
		{
			base.OnShow(paralist);
			if (paralist != null && paralist.Length > 1)
			{
				string text = paralist[0]?.ToString();
				Panel.ContentText.text = text;
				if (paralist[1] != null)
				{
					m_QueRenBtnActionNoParams = paralist[1] as Action;
				}
				if (paralist[2] != null)
				{
					m_CloseWndAction = paralist[2] as Action;
				}
			}
		}

		public void ChangeValue(string text, Action _action, Action _closeAction = null)
		{
			Panel.ContentText.text = text;
			m_QueRenBtnActionNoParams = _action;
			m_CloseWndAction = _closeAction;
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
