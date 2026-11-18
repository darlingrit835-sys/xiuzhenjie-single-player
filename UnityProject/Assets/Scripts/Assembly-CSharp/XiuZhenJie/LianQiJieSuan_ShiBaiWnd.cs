using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace XiuZhenJie
{
	public class LianQiJieSuan_ShiBaiWnd : Window
	{
		private LianQiJieSuan_ShiBaiPanel m_Panel;

		private List<QiTuCaiLiaoEntity> m_DataSource;

		public override void Awake(params object[] paralist)
		{
			base.Awake(paralist);
			m_Panel = base.GameObject.GetComponent<LianQiJieSuan_ShiBaiPanel>();
			AddButtonClickListener(m_Panel.QueRenButton, QueRen);
			m_DataSource = new List<QiTuCaiLiaoEntity>();
			m_DataSource = paralist[0] as List<QiTuCaiLiaoEntity>;
			InitSuccessPanel();
		}

		private void QueRen()
		{
			LianQiFangWnd lianQiFangWnd = Singleton<UIManager>.Instance.FindWndByName<LianQiFangWnd>("LianQiFangWnd.prefab");
			if (lianQiFangWnd != null)
			{
				Singleton<UIManager>.Instance.CloseWnd(lianQiFangWnd);
			}
			Singleton<UIManager>.Instance.CloseWnd(this);
		}

		private void InitSuccessPanel()
		{
			int num = m_DataSource?.Count ?? 0;
			InitCaiLiaoList();
			m_Panel.JingLiXiaoHaoText.text = num + "点";
		}

		private void InitCaiLiaoList()
		{
			RectTransform caiLiaoXiaoHaoListRectTransform = m_Panel.CaiLiaoXiaoHaoListRectTransform;
			if (m_DataSource == null || m_DataSource.Count <= 0)
			{
				return;
			}
			foreach (QiTuCaiLiaoEntity item in m_DataSource)
			{
				if (item.TargetType != 0)
				{
					InitCaiLiaoItem(item.TargetName, caiLiaoXiaoHaoListRectTransform);
				}
			}
		}

		private void InitCaiLiaoItem(string data, RectTransform parentRect)
		{
			if (data != null && m_DataSource != null)
			{
				GameObject gameObject = SetListContentParent(parentRect, m_Panel.CaiLiaoListItem);
				gameObject.gameObject.SetActive(value: true);
				gameObject.GetComponent<Text>().text = data;
			}
		}
	}
}
