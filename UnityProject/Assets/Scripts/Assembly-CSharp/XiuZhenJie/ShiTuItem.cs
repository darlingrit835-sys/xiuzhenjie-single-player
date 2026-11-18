using UnityEngine;
using UnityEngine.UI;

namespace XiuZhenJie
{
	public class ShiTuItem : MonoBehaviour
	{
		public Image TouXiangIcon;

		public Text DaoYouName;

		public Text RoleLevel;

		public Text ShiTuGuanXi;

		public Text QingMiDu;

		public Image ChuShiImg;

		public Button ChuShiBtn;

		public Image ZhuChuImg;

		public Button ZhuChuBtn;

		public Button ChuangGongBtn;

		public Image ToMaiImg;

		public Button ToMaiBtn;

		public bool IsOnLine;

		private bool IsTongMaiOrChuanGong;

		public string FriendRoleId;

		public int index;

		public void SetValue(int listIndex, TuDiEntity tuDiEntity)
		{
			GuanBiYeMan();
			IsTongMaiOrChuanGong = tuDiEntity.IsTongMaiOrChuanGong;
			index = listIndex;
			FriendRoleId = tuDiEntity.Id;
			IsOnLine = tuDiEntity.IsOnline;
			DaoYouName.text = tuDiEntity.Name;
			QingMiDu.text = $"亲密: {tuDiEntity.Intimacy}";
			ShiTuGuanXi.text = IntimacyGradePanDuan(tuDiEntity.Intimacy) ?? "";
			RoleLevel.text = "境界:" + WorldSetting.GetRoleLevelNameByRoleLevel(tuDiEntity.Level);
			Sprite sprite2 = Singleton<RoleManager>.Instance.GetTouXiangSprite(tuDiEntity.Sex == 1, tuDiEntity.HeadPortraitId, tuDiEntity.IsOnline);
			TouXiangIcon.sprite = sprite2;
			float y = -listIndex * 81;
			RectTransform component = base.gameObject.GetComponent<RectTransform>();
			component.anchoredPosition = new Vector2(0f, y);
			component.sizeDelta = new Vector2(0f, component.sizeDelta.y);
			PanDuanQingMiDu(tuDiEntity);
		}

		public void ZuChuSetValue(int listIndex, TuDiEntity tuDiEntity)
		{
			GuanBiYeMan();
			index = listIndex;
			FriendRoleId = tuDiEntity.Id;
			IsOnLine = tuDiEntity.IsOnline;
			DaoYouName.text = tuDiEntity.Name;
			QingMiDu.text = $"亲密: {tuDiEntity.Intimacy}";
			ShiTuGuanXi.text = IntimacyGradePanDuan(tuDiEntity.Intimacy) ?? "";
			RoleLevel.text = "境界:" + WorldSetting.GetRoleLevelNameByRoleLevel(tuDiEntity.Level);
			Sprite sprite2 = Singleton<RoleManager>.Instance.GetTouXiangSprite(tuDiEntity.Sex == 1, tuDiEntity.HeadPortraitId, tuDiEntity.IsOnline);
			TouXiangIcon.sprite = sprite2;
			float y = -listIndex * 81;
			RectTransform component = base.gameObject.GetComponent<RectTransform>();
			component.anchoredPosition = new Vector2(0f, y);
			component.sizeDelta = new Vector2(0f, component.sizeDelta.y);
			if (tuDiEntity.Intimacy < 300)
			{
				ZhuChuImg.gameObject.SetActive(value: true);
				ZhuChuBtn.onClick.RemoveListener(ZhuChu);
				ZhuChuBtn.onClick.AddListener(ZhuChu);
			}
			else
			{
				ChuShiImg.gameObject.SetActive(value: true);
				ChuShiBtn.onClick.RemoveListener(ChuShi);
				ChuShiBtn.onClick.AddListener(ChuShi);
			}
		}

		private string IntimacyGradePanDuan(int intimacy)
		{
			if (intimacy >= 100)
			{
				return "亲传弟子";
			}
			return "记名弟子";
		}

		private void GuanBiYeMan()
		{
			ToMaiImg.gameObject.SetActive(value: false);
			ZhuChuImg.gameObject.SetActive(value: false);
			ChuShiImg.gameObject.SetActive(value: false);
			ToMaiBtn.gameObject.SetActive(value: false);
		}

		private void PanDuanQingMiDu(TuDiEntity tuDiEntity)
		{
			if (WorldSetting.GetStageByLevel(Singleton<RoleManager>.Instance.Role_Entity.RoleLevel) > WorldSetting.GetStageByLevel(tuDiEntity.Level))
			{
				ToMaiImg.gameObject.SetActive(value: true);
				ChuangGongBtn.onClick.RemoveListener(ChuangGong);
				ChuangGongBtn.onClick.AddListener(ChuangGong);
				if (tuDiEntity.Intimacy >= 100)
				{
					ToMaiBtn.gameObject.SetActive(value: true);
					ToMaiBtn.onClick.RemoveListener(ToMai);
					ToMaiBtn.onClick.AddListener(ToMai);
				}
			}
		}

		private void ChuShi()
		{
			Singleton<UIManager>.Instance.PopUpWnd("ShiTuToZiWnd.prefab", false, true, ShiTuToZiList.ChuShi, FriendRoleId);
		}

		private void ChuangGong()
		{
			if (!IsTongMaiOrChuanGong)
			{
				Singleton<UIManager>.Instance.PopUpWnd("ShiTuToZiWnd.prefab", false, true, ShiTuToZiList.ChuangGong, FriendRoleId);
			}
			else
			{
				MonoSingleton<GameStart>.Instance.PopUpAttention("通脉和传功每日只能选择一项！");
			}
		}

		private void ToMai()
		{
			if (!IsTongMaiOrChuanGong)
			{
				Singleton<UIManager>.Instance.PopUpWnd("ShiTuToZiWnd.prefab", false, true, ShiTuToZiList.ToMai, FriendRoleId);
			}
			else
			{
				MonoSingleton<GameStart>.Instance.PopUpAttention("通脉和传功每日只能选择一项！");
			}
		}

		private void ZhuChu()
		{
			Singleton<UIManager>.Instance.PopUpWnd("ShiTuToZiWnd.prefab", false, true, ShiTuToZiList.ZhuChu, FriendRoleId);
		}

	}
}
