using UnityEngine;
using UnityEngine.UI;

namespace XiuZhenJie
{
	public class ZongMenChatPanel : MonoBehaviour
	{
		public Button LiKaiBtn;

		public Button ZongMenXiangQingBtn;

		public Button SendBtn;

		public InputField SendInputField;

		public Text MinTitleText;

		public RectTransform ChatScrollContent;

		public RectTransform ChatScrollStandByContent;

		public RectTransform ZongMenInfoPanel;

		public Text ZongMenInfo_TitleText;

		public Text ZongMenInfo_SuoZaiDiText;

		public Text ZongMenInfo_ZongMenLevelText;

		public Text ZongMenInfo_ZhangLaoRenShuText;

		public Text ZongMenInfo_DiZiRenShuText;

		public Text ZongMenInfo_YingXiangLiText;

		public Text ZongMenInfo_ZhanDouLiDiText;

		public Button ZongMenInfo_ChuanSongBtn;

		public Button ZongMenInfo_CloseBtn;

		public LingQuanJiuSiJieShiPanel JieShiPanel;

		public RectTransform JieShiDaoYou_Panel;

		public Button JieShiDaoYou_CloseBtn;

		public Button JieShiDaoYou_FaChuMingCiBtn;

		public Image JieShiDaoYou_TouXiangImg;

		public Text JieShiDaoYou_NameText;

		public Text JieShiDaoYou_LevelText;
	}
}
