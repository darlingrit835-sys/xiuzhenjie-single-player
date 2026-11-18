using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace XiuZhenJie
{
	public class XuanTianLouScrollItem : MonoBehaviour
	{
		public Text TitleText;

		public RectTransform ContentRect;

		private RectTransform m_SelfRect;

		private Window m_CanWuWindow;

		private List<XuanTianLouItem> m_DataSource;

		private GridLayoutGroup m_GridLayoutGroup;

		public RectTransform HotRect;

		public float GetContentHight()
		{
			return m_SelfRect.sizeDelta.y + 25f;
		}

		private void Awake()
		{
			m_SelfRect = base.transform.gameObject.GetComponent<RectTransform>();
			m_DataSource = new List<XuanTianLouItem>();
		}

		public void SetValue(bool isHot, string titleName, Window xuanTianLouWnd)
		{
			if (isHot)
			{
				HotRect.gameObject.SetActive(value: true);
			}
			m_GridLayoutGroup = ContentRect.gameObject.GetComponent<GridLayoutGroup>();
			ContentRect.sizeDelta = new Vector2(Singleton<UIManager>.Instance.m_UiRoot.sizeDelta.x, 0f);
			TitleText.text = titleName;
			m_CanWuWindow = xuanTianLouWnd;
		}

		public void AddItem(XuanTianLouItem uI_GoodsItem)
		{
			uI_GoodsItem.gameObject.transform.SetParent(ContentRect);
			m_DataSource.Add(uI_GoodsItem);
			m_CanWuWindow.SetGrticalListContentHight(m_DataSource.Count, ContentRect, m_GridLayoutGroup);
			m_SelfRect.sizeDelta = new Vector2(m_SelfRect.sizeDelta.x, TitleText.rectTransform.sizeDelta.y + ContentRect.sizeDelta.y);
		}

		public void ClearData()
		{
			m_DataSource.Clear();
			foreach (Transform item in ContentRect)
			{
				Object.Destroy(item.gameObject);
			}
		}
	}
}
