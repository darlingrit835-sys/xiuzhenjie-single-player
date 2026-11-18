using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

namespace XiuZhenJie
{
	public class LianQiJieSuan_ChengGongWnd : Window
	{
		private LianQiJieSuan_ChengGongPanel m_Panel;

		private List<QiTuCaiLiaoEntity> m_DataSource;

		private Dictionary<int, Sprite> m_GoodsIconSprite;

		private Dictionary<int, Sprite> m_RankBgSprite;

		private BaseNaJieGoodEntity m_FaQi;

		public override void Awake(params object[] paralist)
		{
			base.Awake(paralist);
			m_Panel = base.GameObject.GetComponent<LianQiJieSuan_ChengGongPanel>();
			ResourceLoad();
			AddButtonClickListener(m_Panel.ShoQuButton, ShoQuButton);
			if (paralist.Length >= 2)
			{
				m_DataSource = paralist[0] as List<QiTuCaiLiaoEntity>;
				m_FaQi = paralist[1] as BaseNaJieGoodEntity;
				InitSuccessPanel();
			}
		}

		private void ShoQuButton()
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
			if (m_FaQi != null)
			{
				m_Panel.WuPingNameText.text = m_FaQi.Name + "1件";
				string levelByRankName = WorldSetting.GetLevelByRankName(m_FaQi.Rank);
				m_Panel.BiaoTiNameText.text = "恭喜！成功炼制 '" + levelByRankName + "' " + m_FaQi.Name + " 1件";
				m_GoodsIconSprite.TryGetValue(m_FaQi.Type, out var value);
				m_Panel.WuBingImage.sprite = value;
			}
		}

		private void InitCaiLiaoList()
		{
			RectTransform caiLiaoXiaoHaoListRectTransform = m_Panel.CaiLiaoXiaoHaoListRectTransform;
			if (m_DataSource == null || m_DataSource.Count <= 0)
			{
				return;
			}
			foreach (QiTuCaiLiaoEntity item in m_DataSource.Where((QiTuCaiLiaoEntity item) => item.TargetType != 0))
			{
				InitCaiLiaoItem(item.TargetName, caiLiaoXiaoHaoListRectTransform);
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

		private void ResourceLoad()
		{
			m_GoodsIconSprite = new Dictionary<int, Sprite>();
			m_RankBgSprite = new Dictionary<int, Sprite>();
			Sprite value = Singleton<ResourceManager>.Instance.LoadResource<Sprite>("Assets/GameData/Textures/Item/Goods_Xie.png");
			Sprite value2 = Singleton<ResourceManager>.Instance.LoadResource<Sprite>("Assets/GameData/Textures/Item/Goods_WuQi.png");
			Sprite value3 = Singleton<ResourceManager>.Instance.LoadResource<Sprite>("Assets/GameData/Textures/Item/Goods_FangJu.png");
			Sprite value4 = Singleton<ResourceManager>.Instance.LoadResource<Sprite>("Assets/GameData/Textures/Item/Goods_DefenceWeapon.png");
			m_GoodsIconSprite.Add(29, value);
			m_GoodsIconSprite.Add(15, value2);
			m_GoodsIconSprite.Add(16, value3);
			m_GoodsIconSprite.Add(26, value4);
			Sprite value5 = Singleton<ResourceManager>.Instance.LoadResource<Sprite>("Assets/GameData/Textures/Item/Rank_Huang.png");
			Sprite value6 = Singleton<ResourceManager>.Instance.LoadResource<Sprite>("Assets/GameData/Textures/Item/Rank_Xuan.png");
			Sprite value7 = Singleton<ResourceManager>.Instance.LoadResource<Sprite>("Assets/GameData/Textures/Item/Rank_Di.png");
			Sprite value8 = Singleton<ResourceManager>.Instance.LoadResource<Sprite>("Assets/GameData/Textures/Item/Rank_Tian.png");
			m_RankBgSprite.Add(1, value5);
			m_RankBgSprite.Add(2, value6);
			m_RankBgSprite.Add(3, value7);
			m_RankBgSprite.Add(4, value8);
		}
	}
}
