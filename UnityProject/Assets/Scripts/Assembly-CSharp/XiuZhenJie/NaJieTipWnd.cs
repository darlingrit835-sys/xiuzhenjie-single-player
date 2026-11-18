using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace XiuZhenJie
{
	public class NaJieTipWnd : Window
	{
		public NaJieTipPanel NaJieTipPanel;

		public NaJieTipModel naJieTipModel;

		public BaseNaJieGoodEntity m_DataSource;

		private List<string> m_DataList;

		public int OpenType;

		public override void Awake(params object[] paralist)
		{
			base.Awake(paralist);
			m_DataList = new List<string>();
			NaJieTipPanel = base.GameObject.GetComponent<NaJieTipPanel>();
			AddButtonClickListener(NaJieTipPanel.TipCloseButton, CloseWindow);
		}

		public override void OnShow(params object[] paralist)
		{
			base.OnShow(paralist);
			if (paralist.Length == 1)
			{
				naJieTipModel = paralist[0] as NaJieTipModel;
			}
			if (naJieTipModel == null)
			{
				Debug.Log("装备详情无法显示");
				return;
			}
			m_DataSource = naJieTipModel.dataSource as BaseNaJieGoodEntity;
			if (m_DataSource != null && (m_DataSource.Type == 26 || m_DataSource.Type == 29 || m_DataSource.Type == 14 || m_DataSource.Type == 15 || m_DataSource.Type == 16))
			{
				NaJieTipPanel.TiSheng.gameObject.SetActive(value: true);
				AddButtonClickListener(NaJieTipPanel.TiSheng, TiShengPingJibut);
			}
			else
			{
				NaJieTipPanel.TiSheng.gameObject.SetActive(value: false);
			}
			GetGoodsInfo();
		}

		private void TiShengPingJibut()
		{
			Singleton<UIManager>.Instance.PopUpWnd("TiShengPingJieWnd.prefab", true, m_DataSource.GoodsId, GoodsTypeEnum.XIAN_JIN, m_DataSource.Level, $"{m_DataSource.ConversionRate:F}", m_DataSource.Rank);
		}

		private void GetGoodsInfo()
		{
			if (naJieTipModel.LeftBtnClickEvent == null)
			{
				NaJieTipPanel.LeftBtn.interactable = false;
			}
			if (naJieTipModel.MiddleBtnClickEvent == null)
			{
				NaJieTipPanel.MiddleBtn.interactable = false;
			}
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
					ReflashValue();
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
							else
							{
								m_DataList.Add(content);
							}
						}
						ReflashValue();
					});
					return;
				}
				if (19 == m_DataSource.Type)
				{
					Singleton<RoleManager>.Instance.RoleBoxDic[10].GetDanYaoTimesXiangQing(m_DataSource.GoodsId, m_DataSource.Level, m_DataSource.Rank, m_DataSource.FiveElement, (List<string> strList) =>
					{
						foreach (string content2 in m_DataSource.ContentList)
						{
							if (content2.Contains("药效值") && !content2.Contains("（原）") && strList != null && strList.Count >= 2)
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
						ReflashValue();
					});
					return;
				}
				foreach (string content3 in m_DataSource.ContentList)
				{
					m_DataList.Add(content3);
				}
				ReflashValue();
			}
		}

		public void ReflashValue()
		{
			try
			{
				NaJieTipPanel.ItemName.text = naJieTipModel.Name;
				NaJieTipPanel.MiddleBtn.gameObject.SetActive(value: true);
				if (!string.IsNullOrEmpty(naJieTipModel.MiddleBtnText))
				{
					NaJieTipPanel.MiddleBtn.gameObject.SetActive(value: true);
					NaJieTipPanel.MiddleBtnText.text = naJieTipModel.MiddleBtnText;
				}
				else
				{
					NaJieTipPanel.MiddleBtn.gameObject.SetActive(value: false);
				}
				if (!string.IsNullOrEmpty(naJieTipModel.LeftBtnText))
				{
					NaJieTipPanel.LeftBtn.gameObject.SetActive(value: true);
					NaJieTipPanel.LeftBtnText.text = naJieTipModel.LeftBtnText;
				}
				else
				{
					NaJieTipPanel.LeftBtn.gameObject.SetActive(value: false);
				}
				if (naJieTipModel.IsLock)
				{
					NaJieTipPanel.LeftBtn.interactable = false;
					NaJieTipPanel.RightBtnText.text = "解锁";
				}
				else
				{
					NaJieTipPanel.LeftBtn.interactable = true;
					NaJieTipPanel.RightBtnText.text = "锁定";
				}
				NaJieTipPanel.LeftBtn.onClick.RemoveAllListeners();
				NaJieTipPanel.MiddleBtn.onClick.RemoveAllListeners();
				NaJieTipPanel.RightBtn.onClick.RemoveAllListeners();
				AddButtonClickListener(NaJieTipPanel.MiddleBtn, () =>
				{
					if (naJieTipModel.MiddleBtnClickEvent != null)
					{
						naJieTipModel.MiddleBtnClickEvent(naJieTipModel.type, naJieTipModel.dataSource);
					}
					Singleton<UIManager>.Instance.CloseWnd("NaJieTipWnd.prefab");
				});
				AddButtonClickListener(NaJieTipPanel.LeftBtn, () =>
				{
					if (naJieTipModel.LeftBtnClickEvent != null)
					{
						naJieTipModel.LeftBtnClickEvent(naJieTipModel.type, naJieTipModel.dataSource);
					}
					Singleton<UIManager>.Instance.CloseWnd("NaJieTipWnd.prefab");
				});
				AddButtonClickListener(NaJieTipPanel.RightBtn, () =>
				{
					if (naJieTipModel.RightBtnClickEvent != null)
					{
						naJieTipModel.RightBtnClickEvent(naJieTipModel.type, naJieTipModel.dataSource);
					}
					Singleton<UIManager>.Instance.CloseWnd("NaJieTipWnd.prefab");
				});
				LoadTipItem();
			}
			catch (Exception ex)
			{
				Debug.Log("najieTip  reflash失败:" + ex.Message);
			}
		}

		public void LoadTipItem()
		{
			GameObject gameObject = Singleton<ResourceManager>.Instance.LoadResource<GameObject>("Assets/GameData/Prefabs/Items/NaJieTipItem.prefab");
			if (gameObject == null)
			{
				Debug.Log("納戒详细信息无法显示");
				return;
			}
			RectTransform content = NaJieTipPanel.Content;
			float y = gameObject.GetComponent<RectTransform>().sizeDelta.y;
			foreach (Transform item in content)
			{
				UnityEngine.Object.Destroy(item.gameObject);
			}
			if (m_DataList != null)
			{
				foreach (string data in m_DataList)
				{
					NaJieTipItem component = SetListContentParent(content, gameObject).GetComponent<NaJieTipItem>();
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
			}
			VerticalLayoutGroup component2 = content.gameObject.GetComponent<VerticalLayoutGroup>();
			int itemCount = ((m_DataList == null) ? 1 : m_DataList.Count);
			content.sizeDelta = new Vector2(content.sizeDelta.x, SetVerticalListContentHight(itemCount, y, component2));
		}

		public new void CloseWindow()
		{
			Singleton<UIManager>.Instance.CloseWnd("NaJieTipWnd.prefab");
		}

		public void XiaoHuiClick()
		{
			Singleton<UIManager>.Instance.PopUpWnd("NaJieTipWnd.prefab", true, 2);
		}

		public override void OnDisable()
		{
			base.OnDisable();
		}
	}
}
