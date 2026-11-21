using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace XiuZhenJie
{
	public class QiZhenGeGoodsInformationWnd : Window
	{
		public QiZhenGeGoodsInfomationPanel _panel;

		private BaseNaJieGoodEntity m_DataSource;

		private StoreType m_CurrentStoreType;

		private List<string> m_DataList;

		public override void Awake(params object[] paralist)
		{
			base.Awake(paralist);
			_panel = base.GameObject.GetComponent<QiZhenGeGoodsInfomationPanel>();
			m_DataList = new List<string>();
			AddButtonClickListener(_panel.CloseBtn, CloseWnd);
			AddButtonClickListener(_panel.BgBtn, CloseWnd);
			AddButtonClickListener(_panel.PaiMaiBtn, PaiMai);
		}

		public override void OnShow(params object[] paralist)
		{
			base.OnShow(paralist);
			if (paralist.Length != 0)
			{
				m_DataSource = paralist[0] as BaseNaJieGoodEntity;
				m_CurrentStoreType = (StoreType)paralist[1];
			}
			GetGoodsInfo();
		}

		private void PaiMai()
		{
			if (m_DataSource != null)
			{
				Singleton<UIManager>.Instance.PopUpWnd("QZG_PaiMaiWuPinXinXiWnd.prefab", true, m_DataSource, m_CurrentStoreType);
				CloseWnd();
			}
		}

		private void CloseWnd()
		{
			Singleton<UIManager>.Instance.CloseWnd(this);
		}

		private void CreateData()
		{
			_panel.TitleName.text = m_DataSource.Name;
			GameObject gameObject = Singleton<ResourceManager>.Instance.LoadResource<GameObject>("Assets/GameData/Prefabs/Items/NaJieTipItem.prefab");
			if (gameObject == null)
			{
				Debug.Log("納戒详细信息无法显示");
				return;
			}
			RectTransform scrollContent = _panel.ScrollContent;
			float y = gameObject.GetComponent<RectTransform>().sizeDelta.y;
			foreach (string data in m_DataList)
			{
				NaJieTipItem component = SetListContentParent(scrollContent, gameObject).GetComponent<NaJieTipItem>();
				string[] array = data.Split(',');
				if (array.Length == 2)
				{
					component.SetItem(array[0], array[1], "");
				}
				else if (array.Length == 3)
				{
					component.SetItem(array[0], array[1], array[2]);
				}
			}
			VerticalLayoutGroup component2 = scrollContent.gameObject.GetComponent<VerticalLayoutGroup>();
			scrollContent.sizeDelta = new Vector2(scrollContent.sizeDelta.x, SetVerticalListContentHight(m_DataList.Count, y, component2));
		}

		private void GetGoodsInfo()
		{
			m_DataList.Clear();
			if (m_DataSource.ContentList == null || m_DataSource.ContentList.Count == 0)
			{
				Singleton<RoleManager>.Instance.RoleBoxDic[10].GetGoodsInfomation(m_DataSource, (List<string> strList) =>
				{
					if (strList != null)
					{
						foreach (string str in strList)
						{
							m_DataList.Add(str);
						}
					}
					CreateData();
				});
			}
			else
			{
				if (m_DataSource.ContentList == null && m_DataSource.ContentList.Count <= 0)
				{
					return;
				}
				if (21 == m_DataSource.Type)
				{
					Singleton<RoleManager>.Instance.RoleBoxDic[10].GetFuLuTimesXiangQing(m_DataSource.GoodsId, m_DataSource.Level, m_DataSource.Rank, m_DataSource.FiveElement, (List<string> strList) =>
					{
						foreach (string content in m_DataSource.ContentList)
						{
							if (content.Contains("剩余使用") && strList != null && strList.Count >= 1)
							{
								m_DataList.Add(strList[0]);
							}
						}
						CreateData();
					});
				}
				else
				{
					if (19 != m_DataSource.Type)
					{
						return;
					}
					Singleton<RoleManager>.Instance.RoleBoxDic[10].GetDanYaoTimesXiangQing(m_DataSource.GoodsId, m_DataSource.Level, m_DataSource.Rank, m_DataSource.FiveElement, (List<string> strList) =>
					{
						foreach (string content2 in m_DataSource.ContentList)
						{
							if (content2.Contains("药效值") && strList != null && strList.Count >= 2)
							{
								m_DataList.Add(strList[0]);
							}
							else if (content2.Contains("已服用次数") && strList != null && strList.Count >= 2)
							{
								m_DataList.Add(strList[1]);
							}
							else
							{
								m_DataList.Add(content2);
							}
						}
						CreateData();
					});
				}
			}
		}
	}
}
