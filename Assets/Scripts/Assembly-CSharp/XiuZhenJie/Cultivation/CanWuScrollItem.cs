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

		private float headingHeight;

		public Button HelpBtn;

		public float GetContentHight()
		{
			return m_SelfRect.sizeDelta.y + 10f;
		}

		private void Awake()
		{
			m_SelfRect = base.transform.gameObject.GetComponent<RectTransform>();
			m_ListGoods = new List<UI_GoodsItem>();
		}

		public void SetValue(string titleName, Window canWuWindow, string helpContent = "")
		{
			m_GridLayoutGroup = ContentRect.gameObject.GetComponent<GridLayoutGroup>();
			ContentRect.anchorMin = new Vector2(0, 1);
			ContentRect.anchorMax = Vector2.one;
			ContentRect.pivot = new Vector2(.5f, 1f);
			headingHeight = string.IsNullOrEmpty(titleName) ? 0 : 25;
			ContentRect.anchoredPosition = new Vector2(0, -headingHeight);
			ContentRect.sizeDelta = new Vector2(0, ContentRect.sizeDelta.y);
			m_GridLayoutGroup.constraint = GridLayoutGroup.Constraint.FixedColumnCount;
			m_GridLayoutGroup.padding = new RectOffset(4, 4, 4, 4);
			m_GridLayoutGroup.spacing = new Vector2(5, 4);
			ResizeColumns();
			UIManager.FitTextToRect(TitleText);
			InkInterfaceArt.Set(TitleText.transform.parent.GetComponent<Image>(), "ParchmentHeading", false);
			InkInterfaceArt.Set(ContentRect.GetComponent<Image>(), "InkLedgerPaper", false);
			TitleText.color = InkInterfaceArt.Ink;
			TitleText.text = titleName;
			TitleText.transform.parent.gameObject.SetActive(headingHeight > 0);
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

		private void OnRectTransformDimensionsChange()
		{
			if (m_GridLayoutGroup != null) ResizeColumns();
		}

		private void ResizeColumns()
		{
			float width = m_SelfRect.rect.width;
			int columns = width < 248 ? 1 : 2;
			m_GridLayoutGroup.constraintCount = columns;
			m_GridLayoutGroup.cellSize = new Vector2(Mathf.Max(1, (width - 8 - 5 * (columns - 1)) / columns), 50);
			if (m_CanWuWindow != null && m_ListGoods.Count != 0)
				ResizeSection();
		}

		private void ResizeSection()
		{
			int columns = m_GridLayoutGroup.constraintCount;
			int rows = (m_ListGoods.Count + columns - 1) / columns;
			float height = rows * 50 + Mathf.Max(0, rows - 1) * 4 + 8;
			ContentRect.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, height);
			m_SelfRect.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, headingHeight + height);
		}

		public void AddItem(UI_GoodsItem uI_GoodsItem)
		{
			uI_GoodsItem.transform.SetParent(ContentRect, false);
			m_ListGoods.Add(uI_GoodsItem);
			ResizeSection();
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
