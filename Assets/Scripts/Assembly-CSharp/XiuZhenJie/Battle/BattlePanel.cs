using UnityEngine;
using UnityEngine.UI;

namespace XiuZhenJie
{
	public class BattlePanel : MonoBehaviour
	{
		public ETCJoystick EtcJoyImg;

		public ETCArea EtcAreaImg;

		public Image EtcAngleImg;

		public Text MapNameText;

		public Button LiKaiBtn;

		public Button LiKaiKillBossBtn;

		public Image PlayerTouXiangImg;

		public Image FangHuValueImg;

		public Image BloodValueImg;

		public Text FangHuValueText;

		public Image ZhenYuanValueImg;

		public Text ZhenYuanTitleText;

		public Text ZhenYuanValueText;

		public Image ShenShiValueImg;

		public Text ShenShiValueText;

		public Image TiFiveElementIcon;

		public Image ShenFiveElementIcon;

		public Image ZhenFiveElementIcon;

		public Text PosText;

		public Toggle AutoTog;

		public Button ChangeSkillGroupBtn;

		public RectTransform SkillGroupContnet;

		public RectTransform SkillGroupWatingContnet;

		public Text MonsterDieProgressText;

		public Text MonsterDieProgressBossText;

		public RectTransform ChuangTaSuccess;

		public RectTransform ChuangTaFail;

		public Button ChuangTaSuccessNextFloorBtn;

		public Button ChuangTaSuccessNextFloorLiKaiBtn;

		public Button ChuangTaFailBtn;

		public RectTransform BossWarningBg;

		public RectTransform WarningBg;

		public RectTransform SmallPlayerItem;

		public RectTransform SmallMapTran;

		public Button OpenNaJieBtn;

		public Button CloseNaJieBtn;

		public RectTransform NaJiePanel;

		public ScrollRect NaJieScrollView;

		public RectTransform NaJieScrollViewContent;

		public RectTransform Buff_Content;

		public RectTransform ContinueClearYaoSou;

		public Text OverTimeText;

		public Text OverTimeText_AfterKillBoss;
	}
}
