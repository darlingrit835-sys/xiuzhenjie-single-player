using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace XiuZhenJie
{
	public class Common_XuShenJieItem : MonoBehaviour
	{
		public UI_GoodsItem GoodsItem;

		public Text TitleNameText;

		public Button RegisterButton;

		private string m_ID;

		private int Leixin;

		private Common_XiShenJie m_state;

		private object DataSource;

		public void SetItemRegister(object dataSource, string names, int fiveElement, int level, Sprite rankSpr, Sprite iconSpr, UnityAction<object, GameObject> clickAction, Common_XiShenJie state, int leixin)
		{
			DataSource = dataSource;
			UI_GoodsItem component = GoodsItem.gameObject.GetComponent<UI_GoodsItem>();
			m_state = state;
			Leixin = leixin;
			component.SetItem(dataSource, names, fiveElement, level, rankSpr, iconSpr, clickAction);
			TitleNameText.text = names;
			RegisterButton.onClick.AddListener(Register);
		}

		private void Register()
		{
			switch (m_state)
			{
			case Common_XiShenJie.LZ:
			{
				DST_CommonEntity dST_CommonEntity5 = DataSource as DST_CommonEntity;
				Singleton<UIManager>.Instance.PopUpWnd("LZ_ZhuCeTipWnd.prefab", true, dST_CommonEntity5);
				break;
			}
			case Common_XiShenJie.LianDan:
				if (DataSource is DST_CommonEntity dST_CommonEntity4)
				{
					DST_DanFangModel dST_DanFangModel = new DST_DanFangModel
					{
						b = dST_CommonEntity4.Name,
						a = dST_CommonEntity4.Id
					};
					Singleton<UIManager>.Instance.PopUpWnd("DST_ZhuCeTipWnd.prefab", true, dST_DanFangModel, 0);
				}
				break;
			case Common_XiShenJie.FuShu:
			{
				DST_CommonEntity dST_CommonEntity3 = DataSource as DST_CommonEntity;
				Singleton<UIManager>.Instance.PopUpWnd("SFH_ZhuCeTipWnd.prefab", true, dST_CommonEntity3, Leixin, 0);
				break;
			}
			case Common_XiShenJie.FaShu:
			{
				DST_CommonEntity dST_CommonEntity2 = DataSource as DST_CommonEntity;
				Singleton<UIManager>.Instance.PopUpWnd("WFG_ZhuCeTipWnd.prefab", true, dST_CommonEntity2, Leixin, 0);
				break;
			}
			case Common_XiShenJie.LianQi:
			{
				LianQi_CommonEntity lianQi_CommonEntity = DataSource as LianQi_CommonEntity;
				Singleton<UIManager>.Instance.PopUpWnd("LQ_ZhuCeTipWnd.prefab", true, lianQi_CommonEntity, Leixin, 0);
				break;
			}
			case Common_XiShenJie.ZhenFa:
			{
				DST_CommonEntity dST_CommonEntity = DataSource as DST_CommonEntity;
				Singleton<UIManager>.Instance.PopUpWnd("ZS_ZhuCeTipWnd.prefab", true, dST_CommonEntity, Leixin, 0);
				break;
			}
			}
		}
	}
}
