using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace XiuZhenJie
{
	public class Common_XuShenJieSItem : MonoBehaviour
	{
		public UI_GoodsItem GoodsItem;

		public Text TitleNameText;

		public Text ContributionText;

		public Button DuiHuanButton;

		private object DataSource;

		private Common_XiShenJie m_state;

		public void SetItems(object dataSource, string names, int fiveElement, int level, Sprite rankSpr, Sprite iconSpr, UnityAction<object, GameObject> clickAction, Common_XiShenJie state, int convertible)
		{
			DataSource = dataSource;
			ContributionText.text = $"{convertible}贡献点";
			TitleNameText.text = names;
			m_state = state;
			GoodsItem.gameObject.GetComponent<UI_GoodsItem>().SetItem(dataSource, names, fiveElement, level, rankSpr, iconSpr, clickAction);
			DuiHuanButton.onClick.AddListener(DuiHuan);
		}

		private void DuiHuan()
		{
			switch (m_state)
			{
			case Common_XiShenJie.LZ:
			{
				LZ_LingZhiEntity lZ_LingZhiEntity = DataSource as LZ_LingZhiEntity;
				Singleton<UIManager>.Instance.PopUpWnd("LZ_LingZhiDuiHuanTipWnd.prefab", true, lZ_LingZhiEntity);
				break;
			}
			case Common_XiShenJie.LianDan:
			{
				DST_DanFangEntity dST_DanFangEntity = DataSource as DST_DanFangEntity;
				Singleton<UIManager>.Instance.PopUpWnd("DST_DanFangDuiHuanTipWnd.prefab", true, dST_DanFangEntity);
				break;
			}
			case Common_XiShenJie.FuShu:
			{
				SFH_FuShuEntity sFH_FuShuEntity = DataSource as SFH_FuShuEntity;
				Singleton<UIManager>.Instance.PopUpWnd("SFH_FuShuDuiHuanTipWnd.prefab", true, sFH_FuShuEntity);
				break;
			}
			case Common_XiShenJie.FaShu:
			{
				WFG_FaShuEntity wFG_FaShuEntity = DataSource as WFG_FaShuEntity;
				Singleton<UIManager>.Instance.PopUpWnd("WFG_FaShuDuiHuanTipWnd.prefab", true, wFG_FaShuEntity);
				break;
			}
			case Common_XiShenJie.LianQi:
			{
				LQ_LianQiEntity lQ_LianQiEntity = DataSource as LQ_LianQiEntity;
				Singleton<UIManager>.Instance.PopUpWnd("LQ_QiTuDuiHuanTipWnd.prefab", true, lQ_LianQiEntity);
				break;
			}
			case Common_XiShenJie.ZhenFa:
			{
				ZS_ZhenTuEntity zS_ZhenTuEntity = DataSource as ZS_ZhenTuEntity;
				Singleton<UIManager>.Instance.PopUpWnd("ZS_DuiHuanTipWnd.prefab", true, zS_ZhenTuEntity);
				break;
			}
			}
		}
	}
}
