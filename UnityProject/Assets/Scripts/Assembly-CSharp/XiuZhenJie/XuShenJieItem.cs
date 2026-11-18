using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace XiuZhenJie
{
	public class XuShenJieItem : MonoBehaviour
	{
		public UI_GoodsItem GoodsItem;

		public Text TitleNameText;

		public Text WanZhenDuNameText;

		public Text JinDuNameText;

		public Button CompletionButton;

		public Button RegisterButton;

		private string m_ID;

		private UnityAction<object, GameObject> m_ClickAction;

		private object m_DataSource;

		private DST_BuQuanEntity m_DstBuQuanEntity;

		private int m_state;

		public void SetItems(DST_BuQuanEntity dataSource, string names, int fiveElement, int level, Sprite rankSpr, Sprite iconSpr, UnityAction<object, GameObject> clickAction)
		{
			m_ID = dataSource.Id;
			GoodsItem.gameObject.GetComponent<UI_GoodsItem>().SetItem(dataSource, names, fiveElement, level, rankSpr, iconSpr, clickAction);
			TitleNameText.text = names;
			WanZhenDuNameText.text = "完整度：";
			JinDuNameText.text = Progress(dataSource.Progress, panDuan: true);
			CompletionButton.gameObject.SetActive(value: true);
			RegisterButton.gameObject.SetActive(value: false);
			CompletionButton.onClick.AddListener(Completion);
		}

		public void SetItemRegister(DST_BuQuanEntity dataSource, string names, int fiveElement, int level, Sprite rankSpr, Sprite iconSpr, UnityAction<object, GameObject> clickAction, int state)
		{
			m_ID = dataSource.Id;
			UI_GoodsItem component = GoodsItem.gameObject.GetComponent<UI_GoodsItem>();
			m_state = state;
			component.SetItem(dataSource, names, fiveElement, level, rankSpr, iconSpr, clickAction);
			m_DstBuQuanEntity = dataSource;
			TitleNameText.text = names;
			WanZhenDuNameText.text = "当前篇:";
			JinDuNameText.text = Progress(dataSource.Progress, panDuan: false);
			RegisterButton.onClick.RemoveAllListeners();
			CompletionButton.gameObject.SetActive(value: false);
			RegisterButton.gameObject.SetActive(value: true);
			RegisterButton.onClick.AddListener(Register);
		}

		private void Completion()
		{
			Singleton<GameMapManager>.Instance.LoadScene("LoadingWnd.prefab", "ChuangZhaoGongFaScene", m_ID);
		}

		private void Register()
		{
			if (m_DstBuQuanEntity.Progress == 1)
			{
				Singleton<UIManager>.Instance.PopUpWnd("DJSD_ZhuCeTipWnd.prefab", true, m_DstBuQuanEntity, m_state);
			}
			else
			{
				Singleton<UIManager>.Instance.PopUpWnd("DJSD_ZhuCeTipWnd.prefab", true, m_DstBuQuanEntity, 3);
			}
		}

		private static string Progress(int shu, bool panDuan)
		{
			string result = "";
			if (panDuan)
			{
				switch (shu)
				{
				case 1:
					result = "33%";
					break;
				case 2:
					result = "66%";
					break;
				case 3:
					result = "100%";
					break;
				}
			}
			else
			{
				switch (shu)
				{
				case 1:
					result = "上篇";
					break;
				case 2:
					result = "中篇";
					break;
				case 3:
					result = "完整";
					break;
				}
			}
			return result;
		}
	}
}
