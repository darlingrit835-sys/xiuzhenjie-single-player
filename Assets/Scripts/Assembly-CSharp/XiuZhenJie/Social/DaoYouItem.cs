using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace XiuZhenJie
{
	public class DaoYouItem : MonoBehaviour
	{
		public Image TouXiangIcon;
		public Image QinMiDuIcon;
		public Image ReceiveMsgNumIcon;
		public Text DaoYouName;
		public Text QinMiDuText;
		public Text RoleLevel;
		public Text NoReadMsgNum;
		public Button ChuanYinBtn;
		public Button ChuanWuBtn;
		public Text FuZhiYi_Text;
		public Image FuZhiYi_Img;
		public Image FuZhiEr_Img;
		public Text FuZhiEr_Text;
		public bool IsOnLine;
		public string FriendRoleId;
		public int NoReadCount;
		public int index;
		private Button removeButton;

		internal void StyleCard()
		{
			StyleIdentity(transform, TouXiangIcon, DaoYouName, RoleLevel, QinMiDuText, QinMiDuIcon,
				FuZhiYi_Img.transform.parent, rightInset: 91f);
			Place(ChuanYinBtn.GetComponent<RectTransform>(), 1f, 1f, 1f, -82f, -8f, -37f, -8f);
			Place(ChuanWuBtn.GetComponent<RectTransform>(), 1f, 1f, 0f, -82f, -8f, 8f, 37f);
			StyleAction(ChuanYinBtn, "传书", primary: true);
			StyleAction(ChuanWuBtn, "传物");
			var oldTransferBadge = ChuanWuBtn.transform.Find("MsgNum");
			if (oldTransferBadge != null)
				oldTransferBadge.gameObject.SetActive(false);
			removeButton = Instantiate(ChuanWuBtn, ChuanWuBtn.transform.parent, false);
			removeButton.name = "FriendActions";
			Place(removeButton.GetComponent<RectTransform>(), 1f, 1f, 1f, -121f, -89f, -36f, -8f);
			InkSocialArt.StyleQuietMenu(removeButton);
		}

		public void SetValue(int listIndex, string roleId, bool isOnline, int unReadMsgNum, bool sex, string touXiangId,
			string name, int level, int intimacy, UnityAction<DaoYouFirendEntity> chuanYinAction,
			UnityAction<DaoYouFirendEntity> chuanWuAction, RoleFuZhiType occupationOne, RoleFuZhiType occupationTwo,
			UnityAction<DaoYouFirendEntity> removeAction = null)
		{
			index = listIndex;
			FriendRoleId = roleId;
			IsOnLine = isOnline;
			DaoYouName.text = name;
			RoleLevel.text = (isOnline ? "在线" : "离线") + " · " + WorldSetting.GetRoleLevelNameByRoleLevel(level);
			RoleLevel.color = isOnline ? new Color32(43, 108, 80, 255) : InkInterfaceArt.SoftInk;
			QinMiDuText.text = $"亲密 {intimacy}";
			TouXiangIcon.sprite = Singleton<RoleManager>.Instance.GetTouXiangSprite(sex, touXiangId, isOnline);
			int.TryParse(touXiangId, out int headId);
			var friend = new DaoYouFirendEntity
			{
				DaoYouItemObj = gameObject,
				RoleHeadId = headId,
				RoleId = roleId,
				RoleName = name,
				RoleSex = sex ? 1 : 0
			};

			ChuanYinBtn.onClick.RemoveAllListeners();
			ChuanWuBtn.onClick.RemoveAllListeners();
			ChuanYinBtn.onClick.AddListener(() =>
			{
				EnterChatRoom();
				chuanYinAction?.Invoke(friend);
			});
			ChuanWuBtn.onClick.AddListener(() => chuanWuAction?.Invoke(friend));
			if (removeButton != null)
			{
				removeButton.onClick.RemoveAllListeners();
				removeButton.gameObject.SetActive(removeAction != null);
				if (removeAction != null)
					removeButton.onClick.AddListener(() => removeAction(friend));
			}
			SetUnread(unReadMsgNum);
			// Keep the identity legible at small widths. Replace the old overlapping badges with subtitle metadata.
			string primary = OccupationName(occupationOne);
			string secondary = OccupationName(occupationTwo);
			if (primary.Length > 0)
				QinMiDuText.text += " · " + primary;
			if (secondary.Length > 0)
				QinMiDuText.text += " · " + secondary;
		}

		internal static void StyleIdentity(Transform root, Image portrait, Text name, Text level, Text detail,
			Image decorativeIcon, Transform occupationBadges, float rightInset)
		{
			InkDialogArt.SetPaper(root.GetComponent<Image>());
			var frame = (RectTransform)portrait.transform.parent;
			Place(frame, 0f, 0f, .5f, 8f, 64f, -28f, 28f);
			InkInterfaceArt.Set(frame.GetComponent<Image>(), "LightPlaque", false);
			InkDialogArt.Inset(portrait.rectTransform, 4f, 4f, 4f, 4f);
			portrait.preserveAspect = true;

			Place(name.rectTransform, 0f, 1f, 1f, 73f, -rightInset, -32f, -8f);
			Place(level.rectTransform, 0f, 1f, 1f, 73f, -rightInset, -51f, -32f);
			Place(detail.rectTransform, 0f, 1f, 1f, 73f, -rightInset, -67f, -51f);
			StyleText(name, 17, InkInterfaceArt.Ink, TextAnchor.MiddleLeft);
			StyleText(level, 12, InkInterfaceArt.SoftInk, TextAnchor.MiddleLeft);
			StyleText(detail, 11, InkInterfaceArt.SoftInk, TextAnchor.MiddleLeft);
			decorativeIcon.gameObject.SetActive(false);
			occupationBadges.gameObject.SetActive(false);
		}

		internal static void StyleAction(Button button, string label, bool primary = false)
		{
			Color color = primary ? InkDialogArt.Ivory : InkInterfaceArt.Ink;
			InkDialogArt.StyleButton(button, primary ? "LightAction" : "LightPlaque", color);
			var text = button.transform.Find("Text")?.GetComponent<Text>() ?? button.GetComponentInChildren<Text>(true);
			if (text == null)
				return;
			text.text = label;
			InkDialogArt.Inset(text.rectTransform, 3f, 3f, 2f, 2f);
			StyleText(text, 14, color);
		}

		internal static void StyleText(Text text, int size, Color color, TextAnchor alignment = TextAnchor.MiddleCenter)
		{
			text.color = color;
			text.fontSize = size;
			text.resizeTextForBestFit = true;
			text.resizeTextMinSize = size > 12 ? 11 : 10;
			text.resizeTextMaxSize = size;
			text.alignment = alignment;
			text.horizontalOverflow = HorizontalWrapMode.Wrap;
			text.verticalOverflow = VerticalWrapMode.Truncate;
		}

		internal static void Place(RectTransform rect, float anchorLeft, float anchorRight, float anchorY,
			float left, float right, float bottom, float top)
		{
			rect.anchorMin = new Vector2(anchorLeft, anchorY);
			rect.anchorMax = new Vector2(anchorRight, anchorY);
			rect.offsetMin = new Vector2(left, bottom);
			rect.offsetMax = new Vector2(right, top);
		}

		private static string OccupationName(RoleFuZhiType occupation)
		{
			return occupation switch
			{
				RoleFuZhiType.LianDan => "炼丹",
				RoleFuZhiType.ZhiFu => "画符",
				RoleFuZhiType.FaShu => "修法",
				RoleFuZhiType.BuDao => "布道",
				RoleFuZhiType.LingZhi => "种植",
				RoleFuZhiType.LianQi => "炼器",
				RoleFuZhiType.ZhenTu => "阵法",
				_ => string.Empty
			};
		}

		private void SetUnread(int count)
		{
			NoReadCount = count;
			NoReadMsgNum.text = count > 99 ? "99+" : count.ToString();
			ReceiveMsgNumIcon.gameObject.SetActive(count > 0);
		}

		public void EnterChatRoom()
		{
			SetUnread(0);
		}

		public void AddOneNoReadMsg()
		{
			SetUnread(NoReadCount + 1);
		}
	}
}
