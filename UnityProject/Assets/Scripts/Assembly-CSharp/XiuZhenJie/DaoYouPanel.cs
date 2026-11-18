using UnityEngine;
using UnityEngine.UI;

namespace XiuZhenJie
{
	public class DaoYouPanel : MonoBehaviour
	{
		public ScrollRect DaoYouScroll;

		public ScrollRect MingCiScroll;

		public RectTransform DaoYouScrollContent;

		public RectTransform MingCiScrollContent;

		public Toggle BaiHuiMingCiBtn;

		public Text BaiHuiMingCiText;

		public Button JieShiDaoYouBtn;

		public Text MinTitleText;

		public Image MingCiNumImg;

		public Text MingCiNumText;

		public RectTransform JieShiDaoYouPanel;

		public Button CloseBtn;

		public InputField JueSeMing;

		public RectTransform PlayerInfoRect;

		public Image PlayerTouXiang;

		public Text PlayerName;

		public Text PlayerLevel;

		public Button SouSuoBtn;

		public Button SendBtn;

		public Button DaoLvBtn;

		public Button ShiTuBtn;

		public ScrollViewInfiniteVertical DaoYouVerticalContent;

		public ScrollViewInfiniteVertical MingCiVerticalContent;
	}
}
