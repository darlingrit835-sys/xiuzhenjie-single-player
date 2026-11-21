using UnityEngine;
using UnityEngine.UI;

namespace XiuZhenJie
{
	public class DiHuoGeLianDanJieSuanPanel : MonoBehaviour
	{
		public GameObject SuccessPanel;

		public GameObject FailPanel;

		public GameObject CaiLiaoListItem;

		public Text Success_TitleText;

		public Text Success_NameText;

		public Text Success_JingLiXiaoHaoText;

		public RectTransform Success_Content;

		public Button Success_SureBtn;

		public Text Fail_TitleText;

		public Text Fail_JingLiXiaoHaoText;

		public RectTransform Fail_Content;

		public Button Fail_SureBtn;

		public RectTransform KaoHeFailRect;

		public Button KaoHeZaiShiYiCiBtn;

		public Text KaoHeShengYuTimesText;

		public Button KaoHeFailLiKaiBtn;

		public RectTransform KaoHeSuccessRect;

		public Button KaoHeSuccessLiKaiBtn;

		public RectTransform ChuangZhaoFailRect;

		public Text WuXingXiaoHaoText;

		public Button TiaoZhengDanFangBtn;

		public Button LiKaiBtn;

		public Text FailContentText;

		public RectTransform ChuangZhaoSuccess;

		public Button ZhuCeBtn;

		public Button ShaoHouZhuCeBtn;

		public Text SuccessWuXingXiaoHaoText;
	}
}
