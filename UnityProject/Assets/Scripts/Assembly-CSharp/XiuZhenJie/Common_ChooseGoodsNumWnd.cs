using System.Collections.Generic;
using UnityEngine;

namespace XiuZhenJie
{
	public class Common_ChooseGoodsNumWnd : Window
	{
		public Common_ChooseGoodsNumPanel Panel;

		private Common_ChooseNumEntity m_DataSource;

		private int m_CurrentChooseCount;

		private int m_MaxCount;

		private Dictionary<int, Sprite> m_GoodsIconSprite;

		private Dictionary<int, Sprite> m_RankBgSprite;

		public override void Awake(params object[] paralist)
		{
			base.Awake(paralist);
			Panel = base.GameObject.GetComponent<Common_ChooseGoodsNumPanel>();
			m_GoodsIconSprite = new Dictionary<int, Sprite>();
			m_RankBgSprite = new Dictionary<int, Sprite>();
			AddButtonClickListener(Panel.CloseBtn, CloseWindow);
			AddButtonClickListener(Panel.QueRenBtn, QueRenClick);
			AddButtonClickListener(Panel.AddBtn, AddMethod);
			AddButtonClickListener(Panel.JianBtn, SubtractMethod);
			AddButtonClickListener(Panel.MaxBtn, MaxNum);
		}

		public override void OnShow(params object[] paralist)
		{
			base.OnShow(paralist);
			if (paralist.Length != 0)
			{
				m_DataSource = paralist[0] as Common_ChooseNumEntity;
			}
			if (m_DataSource != null)
			{
				m_CurrentChooseCount = 1;
				m_MaxCount = m_DataSource.Goods.GoodsNum;
				Panel.ContentText.text = $"{m_CurrentChooseCount}/{m_MaxCount}";
				ResourceLoad();
			}
		}

		private void ResourceLoad()
		{
			m_GoodsIconSprite = Singleton<FacadeManager>.Instance.SelectGoodsSprite();
			m_RankBgSprite = Singleton<FacadeManager>.Instance.SelectGoodsRankBg();
			Sprite value = null;
			Sprite value2 = null;
			m_GoodsIconSprite.TryGetValue(m_DataSource.Goods.Type, out value2);
			m_RankBgSprite.TryGetValue(WorldSetting.GetLevelByRank(m_DataSource.Goods.Rank), out value);
			Panel.Goods.SetItem(m_DataSource, m_DataSource.Goods.Name, m_DataSource.Goods.FiveElement, m_DataSource.Goods.Level, value, value2, null);
		}

		private void AddMethod()
		{
			if (m_CurrentChooseCount != m_MaxCount)
			{
				m_CurrentChooseCount++;
				Panel.ContentText.text = $"{m_CurrentChooseCount}/{m_MaxCount}";
				Panel.Goods.ResetNum(m_CurrentChooseCount);
			}
		}

		private void SubtractMethod()
		{
			if (m_CurrentChooseCount != 1)
			{
				m_CurrentChooseCount--;
				Panel.ContentText.text = $"{m_CurrentChooseCount}/{m_MaxCount}";
				Panel.Goods.ResetNum(m_CurrentChooseCount);
			}
		}

		private void MaxNum()
		{
			m_CurrentChooseCount = m_MaxCount;
			Panel.ContentText.text = $"{m_CurrentChooseCount}/{m_MaxCount}";
			Panel.Goods.ResetNum(m_CurrentChooseCount);
		}

		private void QueRenClick()
		{
			if (m_DataSource.QueRenCallBack != null)
			{
				m_DataSource.QueRenCallBack(m_CurrentChooseCount);
			}
			CloseWindow();
		}
	}
}
