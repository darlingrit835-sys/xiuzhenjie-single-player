using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace XiuZhenJie
{
	public class ShoTuItem : MonoBehaviour
	{
		public Image TouXiangIcon;
		public Text DaoYouName;
		public Text RoleLevel;
		public Button HuLueBtn;
		public Button JieShoBtn;
		public bool IsOnLine;
		public string FriendRoleId;
		public int index;

		private bool m_Styled;

		public void SetValue(int listIndex, TuDiEntity applicant, UnityAction<string> accept, UnityAction<string> reject)
		{
			var row = (RectTransform)transform;
			if (!m_Styled)
			{
				ShiTuArt.StylePerson(row, TouXiangIcon, DaoYouName, RoleLevel, 170f);
				row.Find("FuZhiYi")?.gameObject.SetActive(false);
				var relation = row.Find("QinMiDu")?.GetComponent<Text>();
				if (relation != null)
					relation.gameObject.SetActive(false);
				ShiTuArt.PlaceTop((RectTransform)JieShoBtn.transform, true, 9f, 23f, 76f, 33f);
				ShiTuArt.PlaceTop((RectTransform)HuLueBtn.transform, true, 91f, 23f, 66f, 33f);
				ShiTuArt.ButtonStyle(JieShoBtn, "收徒", true);
				ShiTuArt.ButtonStyle(HuLueBtn, "婉拒", false);
				m_Styled = true;
			}

			index = listIndex;
			FriendRoleId = applicant.Id;
			DaoYouName.text = applicant.Name;
			RoleLevel.text = WorldSetting.GetRoleLevelNameByRoleLevel(applicant.Level);
			// The inbox response has no presence indicator; use the normal portrait.
			TouXiangIcon.sprite = Singleton<RoleManager>.Instance.GetTouXiangSprite(applicant.Sex == 1, applicant.HeadPortraitId, isOnline: true);
			ShiTuArt.AtTop(row, 0f, 0f, listIndex * ShiTuArt.RowPitch, 77f);
			BindButton(JieShoBtn, () => accept(FriendRoleId));
			BindButton(HuLueBtn, () => reject(FriendRoleId));
		}

		private static void BindButton(Button button, UnityAction action)
		{
			button.onClick.RemoveAllListeners();
			button.onClick.AddListener(action);
		}
	}
}
