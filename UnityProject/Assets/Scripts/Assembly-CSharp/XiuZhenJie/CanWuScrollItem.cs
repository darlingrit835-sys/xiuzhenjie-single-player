using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace XiuZhenJie
{
	public class CanWuScrollItem : MonoBehaviour
	{
		public Text TitleText;

		public RectTransform ContentRect;

		private RectTransform m_SelfRect;

		private Window m_CanWuWindow;

		private List<UI_GoodsItem> m_ListGoods;

		private GridLayoutGroup m_GridLayoutGroup;

		public Button HelpBtn;

		public float GetContentHight()
		{
			return m_SelfRect.sizeDelta.y + 25f;
		}

		private void Awake()
		{
			m_SelfRect = base.transform.gameObject.GetComponent<RectTransform>();
			m_ListGoods = new List<UI_GoodsItem>();
		}

		public void SetValue(string titleName, Window canWuWindow, string helpContent = "")
		{
			m_GridLayoutGroup = ContentRect.gameObject.GetComponent<GridLayoutGroup>();
			TitleText.text = titleName;
			m_CanWuWindow = canWuWindow;
			if (!string.IsNullOrEmpty(helpContent))
			{
				HelpBtn.gameObject.SetActive(value: true);
				HelpBtn.onClick.AddListener(() =>
				{
					string text = helpContent;
					Singleton<UIManager>.Instance.PopUpWnd("XiaoWenHaoWnd.prefab", true, text, "功法");
				});
			}
		}

		public void AddItem(UI_GoodsItem uI_GoodsItem)
		{
			uI_GoodsItem.gameObject.transform.SetParent(ContentRect);
			m_ListGoods.Add(uI_GoodsItem);
			m_CanWuWindow.SetGrticalListContentHight(m_ListGoods.Count, ContentRect, m_GridLayoutGroup);
			m_SelfRect.sizeDelta = new Vector2(m_SelfRect.sizeDelta.x, TitleText.rectTransform.sizeDelta.y + ContentRect.sizeDelta.y);
		}

		public void ClearData()
		{
			m_ListGoods.Clear();
			foreach (Transform item in ContentRect)
			{
				Object.Destroy(item.gameObject);
			}
		}
	}
}
