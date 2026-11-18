using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace XiuZhenJie
{
	public class ChatItem : MonoBehaviour
	{
		public Image TitleImg;

		public Text TitleText;

		public Text ContentText;

		public Button ClickBtn;

		public int m_Index;

		private RectTransform m_SelfRect;

		public Image VIP_TitleImg;

		public Text VIP_TitleText;

		private void Awake()
		{
			m_SelfRect = base.gameObject.GetComponent<RectTransform>();
		}

		private void Update()
		{
			if (m_SelfRect.sizeDelta.y != ContentText.rectTransform.sizeDelta.y + 10f)
			{
				m_SelfRect.sizeDelta = new Vector2(m_SelfRect.sizeDelta.x, ContentText.rectTransform.sizeDelta.y + 10f);
			}
		}

		public void SetValue(int headFrameId, int chengHaoFrameId, int sex, int level, string headId, string TitleName, string RoleName, string Content, UnityAction<ChatItemEntity> ClickAction)
		{
			if (string.IsNullOrEmpty(TitleName))
			{
				ContentText.text = $"<color=#FF8001>{RoleName}：</color>{Content}";
			}
			else
			{
				if (chengHaoFrameId == 0)
				{
					VIP_TitleImg.gameObject.SetActive(value: false);
					TitleImg.gameObject.SetActive(value: true);
					TitleText.text = TitleName;
				}
				else
				{
					TitleImg.gameObject.SetActive(value: false);
					VIP_TitleImg.gameObject.SetActive(value: true);
					VIP_TitleText.text = TitleName;
				}
				ContentText.text = string.Format("<color=#FFFFFF00>{0}</color><color=#FF8001>{1}：</color>{2}", "啊啊啊啊啊", RoleName, Content);
			}
			if (ClickAction != null)
			{
				ChatItemEntity chatItemEntity = new ChatItemEntity
				{
					ChengHaoFrameId = chengHaoFrameId,
					HeadFrameId = headFrameId,
					HeadId = headId,
					Level = level,
					RoleName = RoleName,
					Sex = sex
				};
				ClickBtn.onClick.AddListener(() =>
				{
					ClickAction(chatItemEntity);
				});
			}
		}

		public void ClearData()
		{
			TitleImg.gameObject.SetActive(value: false);
			TitleText.text = "";
			ContentText.text = "";
			RectTransform component = base.gameObject.GetComponent<RectTransform>();
			component.sizeDelta = new Vector2(component.sizeDelta.x, 21f);
		}

		public float GetHight()
		{
			return m_SelfRect.sizeDelta.y;
		}
	}
}
