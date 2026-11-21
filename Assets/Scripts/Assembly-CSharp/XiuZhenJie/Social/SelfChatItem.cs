using UnityEngine;
using UnityEngine.UI;

namespace XiuZhenJie
{
	public class SelfChatItem : MonoBehaviour
	{
		public Text ChatText;

		public Image TouXiang;

		public Text SendTime;

		public RectTransform ChatBgRect;

		public ContentSizeFitter ChatBgSizeFitter;

		public Vector2 GetContentSizeFitterPreferredSize(RectTransform rect, ContentSizeFitter contentSizeFitter)
		{
			LayoutRebuilder.ForceRebuildLayoutImmediate(rect);
			return new Vector2(HandleSelfFittingAlongAxis(0, rect, contentSizeFitter), HandleSelfFittingAlongAxis(1, rect, contentSizeFitter));
		}

		private float HandleSelfFittingAlongAxis(int axis, RectTransform rect, ContentSizeFitter contentSizeFitter)
		{
			if (((axis == 0) ? contentSizeFitter.horizontalFit : contentSizeFitter.verticalFit) == ContentSizeFitter.FitMode.MinSize)
			{
				return LayoutUtility.GetMinSize(rect, axis);
			}
			return LayoutUtility.GetPreferredSize(rect, axis);
		}

		public void SetValue(string chatContent, string time, int sex, int imageId, bool isOnline)
		{
			ChatText.supportRichText = false;
			ChatText.text = FriendEntity.WrapContent(chatContent);
			SendTime.text = time;

			TouXiang.sprite = GetCurrentRoleSprite(sex, imageId, isOnline);
			InkSocialArt.LayoutChatBubble((RectTransform)transform, ChatBgRect, ChatBgSizeFitter,
				ChatText, TouXiang, SendTime, own: true);
		}

		public Sprite GetCurrentRoleSprite(int sex, int headId, bool isOnline)
		{
			return Singleton<RoleManager>.Instance.GetTouXiangSprite(sex == 1, headId.ToString(), isOnline);
		}

	}
}
