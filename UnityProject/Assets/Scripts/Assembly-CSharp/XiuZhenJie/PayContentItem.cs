using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace XiuZhenJie
{
	public class PayContentItem : MonoBehaviour
	{
		public Text ChatCountText;

		public Text PriceText;

		public Button GouMaiBtn;

		public Image BgImg;

		private UnityAction<int, int> m_BtnClickAction;

		private int m_ChatCount;

		private int m_Type;

		public void SetValue(int type, int ChatCount, long Price, UnityAction<int, int> BtnClickAction)
		{
			m_Type = type;
			m_ChatCount = ChatCount;
			Sprite sprite = Singleton<ResourceManager>.Instance.LoadResource<Sprite>("Assets/GameData/Textures/LingQuanJiuShi/ChatJiu_1.png");
			switch (type)
			{
			case 2:
				sprite = Singleton<ResourceManager>.Instance.LoadResource<Sprite>("Assets/GameData/Textures/LingQuanJiuShi/ChatJiu_1.png");
				break;
			case 3:
				sprite = Singleton<ResourceManager>.Instance.LoadResource<Sprite>("Assets/GameData/Textures/LingQuanJiuShi/ChatJiu_1.png");
				break;
			}
			BgImg.sprite = sprite;
			BgImg.SetNativeSize();
			ChatCountText.text = string.Format(ChatCountText.text, ChatCount);
			PriceText.text = string.Format(PriceText.text, Price);
			m_BtnClickAction = BtnClickAction;
			GouMaiBtn.onClick.RemoveAllListeners();
			GouMaiBtn.onClick.AddListener(BtnClick);
		}

		private void BtnClick()
		{
			if (m_BtnClickAction != null)
			{
				m_BtnClickAction(m_Type, m_ChatCount);
			}
		}
	}
}
