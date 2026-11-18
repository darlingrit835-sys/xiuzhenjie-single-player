using UnityEngine;
using UnityEngine.UI;

namespace XiuZhenJie
{
	public class CashGoodsYueKaPanel : MonoBehaviour
	{
		public Button XTL_Btn;

		public Button TDL_Btn;

		private CashGoodsWnd m_CashGoodsWnd;

		public void Init(CashGoodsWnd cashGoodsWnd)
		{
			m_CashGoodsWnd = cashGoodsWnd;
			if (Singleton<RoleManager>.Instance.Role_Entity.TianDaoLing)
			{
				TDL_Btn.interactable = false;
				Text text = TDL_Btn.transform.Find("Text")?.GetComponent<Text>();
				if (text != null)
				{
					text.text = "已购买";
				}
			}
			else
			{
				TDL_Btn.interactable = true;
			}
			XTL_Btn.onClick.RemoveAllListeners();
			TDL_Btn.onClick.RemoveAllListeners();
			XTL_Btn.onClick.AddListener(GouMaiXuanTianLing);
			TDL_Btn.onClick.AddListener(GouMaiTianDaoLing);
		}

		private void GouMaiXuanTianLing()
		{
		}

		private void GouMaiTianDaoLing()
		{
		}
	}
}
