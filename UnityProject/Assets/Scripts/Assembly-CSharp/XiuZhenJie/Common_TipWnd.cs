using UnityEngine;
using UnityEngine.UI;

namespace XiuZhenJie
{
	public class Common_TipWnd : Window
	{
		public Common_TipPanel Panel;

		private Common_TipWndParamEntity m_DataSource;

		private CanWuEntity CanWuEntity = new CanWuEntity();

		private GameObject m_InsObj;

		private string m_LingYanZhi;

		public override void Awake(params object[] paralist)
		{
			base.Awake(paralist);
			Panel = base.GameObject.GetComponent<Common_TipPanel>();
			AddButtonClickListener(Panel.CloseBtn, CloseWindow);
			AddButtonClickListener(Panel.QueRenBtn, SureBtnClick);
			m_InsObj = Singleton<ResourceManager>.Instance.LoadResource<GameObject>("Assets/GameData/Prefabs/Items/NaJieTipItem.prefab");
			if (paralist.Length == 0)
			{
				return;
			}
			m_DataSource = paralist[0] as Common_TipWndParamEntity;
			if (m_DataSource != null)
			{
				CreateContentList();
				Panel.TitleText.text = (string.IsNullOrEmpty(m_DataSource.TitleName) ? "标题" : m_DataSource.TitleName);
				CanWuEntity = m_DataSource.DataSource as CanWuEntity;
				if (m_DataSource.BtnName == "法术")
				{
					Panel.QueRenBtn.gameObject.SetActive(value: false);
					Panel.BeiJing.gameObject.SetActive(value: true);
					AddButtonClickListener(Panel.TiShengPingJi, TiShengPingJibut);
					AddButtonClickListener(Panel.QueRenBtn2, SureBtnClick);
				}
				else
				{
					Panel.BeiJing.gameObject.SetActive(value: false);
					Panel.ButtonText.text = (string.IsNullOrEmpty(m_DataSource.BtnName) ? "确定" : m_DataSource.BtnName);
				}
			}
		}

		private void TiShengPingJibut()
		{
			Singleton<UIManager>.Instance.PopUpWnd("TiShengPingJieWnd.prefab", true, m_DataSource.ID, GoodsTypeEnum.SHEN_SHU, CanWuEntity.Level, m_LingYanZhi, m_DataSource.Rank);
		}

		private void CreateContentList()
		{
			float y = m_InsObj.GetComponent<RectTransform>().sizeDelta.y;
			if (m_DataSource.ContentList != null)
			{
				foreach (string content in m_DataSource.ContentList)
				{
					string[] array = content.Split(',');
					if (array[0] == "灵衍值")
					{
						m_LingYanZhi = array[1];
					}
					if (array.Length == 2)
					{
						CreateListItem(array[0], array[1], "");
					}
					else if (array.Length == 3)
					{
						CreateListItem(array[0], array[1], array[2]);
					}
				}
			}
			VerticalLayoutGroup component = Panel.ScrollRectContent.gameObject.GetComponent<VerticalLayoutGroup>();
			int itemCount = ((m_DataSource.ContentList == null) ? 1 : m_DataSource.ContentList.Count);
			Panel.ScrollRectContent.sizeDelta = new Vector2(Panel.ScrollRectContent.sizeDelta.x, SetVerticalListContentHight(itemCount, y, component));
		}

		private void CreateListItem(string title, string value, string helpContent)
		{
			Object.Instantiate(m_InsObj, Panel.ScrollRectContent).GetComponent<NaJieTipItem>().SetItem(title, value, helpContent);
		}

		public override void CloseWindow()
		{
			if (m_DataSource != null && m_DataSource.CloseBtnClick != null)
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
