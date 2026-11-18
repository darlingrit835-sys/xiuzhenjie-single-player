using UnityEngine;

namespace XiuZhenJie
{
	public class Common_QiRenWnd : Window
	{
		public Common_QiRenPanel Panel;

		private Common_QiRenParamEntity m_DataSource;

		private GameObject m_InsObj;

		public override void Awake(params object[] paralist)
		{
			base.Awake(paralist);
			Panel = base.GameObject.GetComponent<Common_QiRenPanel>();
			AddButtonClickListener(Panel.GuanBiButton, CloseWindow);
			AddButtonClickListener(Panel.SurButton, SureBtnClick);
			m_InsObj = Singleton<ResourceManager>.Instance.LoadResource<GameObject>("Assets/GameData/Prefabs/Items/NaJieTipItem.prefab");
			if (paralist.Length != 0)
			{
				m_DataSource = paralist[0] as Common_QiRenParamEntity;
				if (m_DataSource != null)
				{
					Panel.XianQingText.text = (string.IsNullOrEmpty(m_DataSource.ContentList) ? " " : m_DataSource.ContentList);
					Panel.TitleNameText.text = (string.IsNullOrEmpty(m_DataSource.TitleName) ? "标题" : m_DataSource.TitleName);
					Panel.SurNameText.text = (string.IsNullOrEmpty(m_DataSource.BtnName) ? "确定" : m_DataSource.BtnName);
				}
			}
		}

		public new void CloseWindow()
		{
			base.CloseWindow();
			if (m_DataSource.CloseBtnClick != null)
			{
				m_DataSource.CloseBtnClick(m_DataSource.DataSource);
			}
			Singleton<UIManager>.Instance.CloseWnd(this);
		}

		private void SureBtnClick()
		{
			if (m_DataSource.SureBtnClick != null)
			{
				m_DataSource.SureBtnClick(m_DataSource.DataSource);
			}
			CloseWindow();
		}
	}
}
