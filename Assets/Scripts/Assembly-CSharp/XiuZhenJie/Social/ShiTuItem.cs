using UnityEngine;
using UnityEngine.Events;
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
		public string FriendRoleId;
		public int index;

		private bool m_Styled;

		public void SetValue(int listIndex, TuDiEntity disciple)
		{
			Bind(listIndex, disciple);
			int teacherLevel = Singleton<RoleManager>.Instance.Role_Entity.RoleLevel;
			string commonBlock = null;
			if (disciple.IsTongMaiOrChuanGong)
				commonBlock = "该弟子今日已受指导，或还有收益待领。";
			else if (WorldSetting.GetStageByLevel(teacherLevel) <= WorldSetting.GetStageByLevel(disciple.Level))
				commonBlock = "你的大境界目前未超过弟子。";

			string powerBlock = commonBlock ?? (teacherLevel - disciple.Level < 10 ? "双方目前相差不足 10 个小境界。" : null);
			string meridianBlock = commonBlock ?? (disciple.Intimacy < 100 ? $"当前亲密 {disciple.Intimacy}，尚未达到 100。" : null);
			ToMaiImg.gameObject.SetActive(true);
			ChuangGongBtn.gameObject.SetActive(true);
			ToMaiBtn.gameObject.SetActive(true);
			ShiTuArt.ButtonStyle(ChuangGongBtn, "传功", powerBlock == null);
			BindButton(ChuangGongBtn, () => OpenAction(ShiTuToZiList.ChuangGong, powerBlock));
			BindButton(ToMaiBtn, () => OpenAction(ShiTuToZiList.ToMai, meridianBlock));
		}

		public void ZuChuSetValue(int listIndex, TuDiEntity disciple)
		{
			Bind(listIndex, disciple);
			bool ready = disciple.Intimacy >= 300;
			ChuShiImg.gameObject.SetActive(true);
			ShiTuArt.PlaceTop(ChuShiImg.rectTransform, true, 9f, ready ? 23f : 9f, 96f, 28f);
			BindButton(ChuShiBtn, () => OpenAction(ShiTuToZiList.ChuShi, ready ? null : $"当前亲密 {disciple.Intimacy}，尚未达到 300。"));
			if (!ready)
			{
				ZhuChuImg.gameObject.SetActive(true);
				ShiTuArt.PlaceTop(ZhuChuImg.rectTransform, true, 9f, 40f, 96f, 28f);
				BindButton(ZhuChuBtn, () => OpenAction(ShiTuToZiList.ZhuChu));
			}
		}

		private void Bind(int listIndex, TuDiEntity disciple)
		{
			if (!m_Styled)
			{
				var row = (RectTransform)transform;
				ShiTuArt.StylePerson(row, TouXiangIcon, DaoYouName, RoleLevel, 117f);
				ShiTuArt.StyleRelation(row, ShiTuGuanXi, QingMiDu);
				StyleAction(ChuShiImg, ChuShiBtn, "出师");
				StyleAction(ZhuChuImg, ZhuChuBtn, "逐出师门");
				PlaceTeaching();
				m_Styled = true;
			}

			FriendRoleId = disciple.Id;
			index = listIndex;
			IsOnLine = disciple.IsOnline;
			DaoYouName.text = disciple.Name;
			RoleLevel.text = WorldSetting.GetRoleLevelNameByRoleLevel(disciple.Level) + (disciple.IsOnline ? "  ·  在线" : "  ·  离线");
			ShiTuGuanXi.text = disciple.Intimacy >= 100 ? "亲传弟子" : "记名弟子";
			QingMiDu.text = $"亲密 {disciple.Intimacy}";
			TouXiangIcon.sprite = Singleton<RoleManager>.Instance.GetTouXiangSprite(disciple.Sex == 1, disciple.HeadPortraitId, disciple.IsOnline);
			ToMaiImg.gameObject.SetActive(false);
			ToMaiBtn.gameObject.SetActive(false);
			ChuShiImg.gameObject.SetActive(false);
			ZhuChuImg.gameObject.SetActive(false);
			ShiTuArt.AtTop((RectTransform)transform, 0f, 0f, listIndex * ShiTuArt.RowPitch, 77f);
		}

		private static void StyleAction(Image group, Button button, string label)
		{
			ShiTuArt.PlaceTop(group.rectTransform, true, 9f, 23f, 96f, 33f);
			if (button.transform != group.transform)
			{
				InkDialogArt.Inset((RectTransform)button.transform, 0f, 0f, 0f, 0f);
				group.color = Color.clear;
			}
			ShiTuArt.ButtonStyle(button, label, false);
		}

		private void PlaceTeaching()
		{
			ShiTuArt.PlaceTop(ToMaiImg.rectTransform, true, 8f, 12f, 96f, 57f);
			ToMaiImg.color = Color.clear;
			ShiTuArt.PlaceTop((RectTransform)ChuangGongBtn.transform, true, 0f, 0f, 87f, 28f);
			ShiTuArt.PlaceTop((RectTransform)ToMaiBtn.transform, true, 0f, 30f, 87f, 28f);
			ShiTuArt.ButtonStyle(ChuangGongBtn, "传功", true);
			ShiTuArt.ButtonStyle(ToMaiBtn, "通脉", false);
		}

		private void OpenAction(ShiTuToZiList action, string blocked = null)
		{
			Singleton<UIManager>.Instance.PopUpWnd("ShiTuToZiWnd.prefab", false, true, action, FriendRoleId, blocked);
		}

		private static void BindButton(Button button, UnityAction action)
		{
			button.onClick.RemoveAllListeners();
			button.onClick.AddListener(action);
		}
	}
}
