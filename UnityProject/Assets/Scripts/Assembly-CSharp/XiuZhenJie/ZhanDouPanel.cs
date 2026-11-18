using UnityEngine;
using UnityEngine.UI;

namespace XiuZhenJie
{
	public class ZhanDouPanel : MonoBehaviour
	{
		public Text SceneName;

		public Button LiKaiButton;

		public Button ZiDongZhanDouButton;

		public Button CloseZiDongZhanDouBtn;

		public RectTransform ZiDongZhanDouMaskImg;

		public Button ZiDongZhanDouCancleMaskBtn;

		public RectTransform MonsterIconParent;

		public Image MonsterIcon1;

		public Image MonsterIcon2;

		public Image MonsterIcon3;

		public RectTransform CurrentFloorRect;

		public RectTransform DiaoLuoPanel;

		public Text CurrentFloorText;

		public Image BackGround_YeWai;

		public Image BackGround_ChuangTa;

		public RectTransform ChuangTaChengGongWnd;

		public Button ChuangTaSuccessNextFloorBtn;

		public Button ChuangTaSuccessLiKaiBtn;

		public RectTransform ChuangTaShiBaiWnd;

		public Button ChuangTaFailLiKaiBtn;

		public Image MonsterIcon;

		public Image MonsterFangHu;

		public Image MonsterZhenYuan;

		public Image MonsterShenNian;

		public Text ChangJianNum;

		public Text XiYouNum;

		public Text QiZhenNum;

		public Text ChuanShuoNum;

		public Text MonsterName;

		public Text ShangHaiZhi;

		public Image Monster;

		public Monster MonsterInstance;

		public People PeopleInstance;

		public CaiLiao CaiLiaoInstance;

		public RectTransform JiNengListContent;

		public Image PlayerIcon;

		public Image PlayerFangHu;

		public Image PlayerZhenYuan;

		public Image PlayerShenNian;

		public GameObject ZiDongZhanDouObject;

		public Button CloseButton;

		public Button QuXiaoButton;

		public Button QueRenKaiQiButton;

		public Toggle BuZaiTiShiToggle;

		public ZhanDouJiNengItem FaShu_1Btn;

		public ZhanDouJiNengItem FaShu_2Btn;

		public ZhanDouJiNengItem FaShu_3Btn;

		public ZhanDouJiNengItem FaShu_4Btn;

		public ZhanDouJiNengItem FaQi_1Btn;

		public ZhanDouJiNengItem FaQi_2Btn;

		public ZhanDouJiNengItem FaQi_3Btn;

		public ZhanDouJiNengItem FaQi_4Btn;

		public ZhanDouJiNengItem FuLu_1Btn;

		public ZhanDouJiNengItem FuLu_2Btn;

		public ZhanDouJiNengItem FuLu_3Btn;

		public ZhanDouJiNengItem FuLu_4Btn;

		public RectTransform SkillGroupContent;

		public Button NextGroupBtn;

		public DiaoLuoImgGroupPanel DiaoLuoContentPanel;

		public XuLiProgressPanel XuLiProgressRect;

		public Text SelectingText;

		public Image SelectingImg;
	}
}
