using UnityEngine;
using UnityEngine.UI;

namespace XiuZhenJie
{
	public class OtherChatItem : MonoBehaviour
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
			string text = "";
			for (int i = 0; i < chatContent.Length; i++)
			{
				text += chatContent[i];
				if (i != 0 && i % 14 == 0)
				{
					text += "\n";
				}
			}
			ChatText.text = text;
			SendTime.text = time;

			TouXiang.sprite = GetCurrentRoleSprite(sex, imageId, isOnline);
			Vector2 contentSizeFitterPreferredSize = GetContentSizeFitterPreferredSize(ChatBgRect, ChatBgSizeFitter);
			base.gameObject.GetComponent<RectTransform>().sizeDelta = contentSizeFitterPreferredSize;
			base.gameObject.transform.parent.gameObject.GetComponent<RectTransform>();
			ChatBgSizeFitter.SetLayoutVertical();
		}

		public Sprite GetCurrentRoleSprite(int sex, int headId, bool isOnline)
		{
			return Singleton<RoleManager>.Instance.GetTouXiangSprite(sex == 1, headId.ToString(), isOnline);
		}

	}
}
