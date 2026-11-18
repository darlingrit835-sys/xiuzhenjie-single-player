using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace XiuZhenJie
{
	public class UI_GoodsItem : MonoBehaviour
	{
		public Text GoodsName;

		public Text GoodsLevel;

		public Text GoodsFiveElement;

		public Button ClickBtn;

		public Image GoodsIcon;

		public Image RankImg;

		public Image isUsingImg;

		public Image NumImg;

		public Text NumText;

		private UnityAction<object, GameObject> m_ClickAction;

		private object m_DataSource;

		private bool m_IsEmpty = true;

		public object DataSource => m_DataSource;

		public bool IsEmpty
		{
			get
			{
				return m_IsEmpty;
			}
			private set
			{
				m_IsEmpty = value;
			}
		}

		public void SetItem(object dataSource, string name, int fiveElement, int level, Sprite rankSpr, Sprite iconSpr, UnityAction<object, GameObject> ClickAction, bool isUsing = false, int num = 1)
		{
			m_DataSource = dataSource;
			GoodsName.text = name;
			string signleWuXingName = WorldSetting.GetSignleWuXingName((WuXing)fiveElement);
			GoodsFiveElement.text = signleWuXingName;
			GoodsLevel.text = level.ToString();
			GoodsIcon.sprite = iconSpr;
			RankImg.sprite = rankSpr;
			ClickBtn.onClick.RemoveAllListeners();
			if (ClickAction != null)
			{
				ClickBtn.onClick.AddListener(ClickItem);
			}
			m_ClickAction = ClickAction;
			IsEmpty = false;
			RankImg.gameObject.SetActive(value: true);
			NumText.text = num.ToString();
			if (isUsing)
			{
				GoodsName.text = $"<color=#7ED321>{GoodsName.text}</color>";
			}
			if (num > 1)
			{
				NumImg.gameObject.SetActive(value: true);
				NumText.text = num.ToString();
			}
			else
			{
				NumImg.gameObject.SetActive(value: false);
			}
		}

		public void ReduceNum(int num)
		{
			int result = 1;
			if (int.TryParse(NumText.text, out result) && result > 1)
			{
				result -= num;
				NumText.text = result.ToString();
				if (result <= 1)
				{
					NumImg.gameObject.SetActive(value: false);
				}
			}
		}

		public void AddNum(int num)
		{
			int result = 1;
			if (int.TryParse(NumText.text, out result) && result >= 1)
			{
				result += num;
				NumText.text = result.ToString();
				if (result > 1 && !NumImg.IsActive())
				{
					NumImg.gameObject.SetActive(value: true);
				}
			}
		}

		public void ResetNum(int num)
		{
			if (num > 0)
			{
				NumText.text = num.ToString();
				if (num <= 1)
				{
					NumImg.gameObject.SetActive(value: false);
				}
				else
				{
					NumImg.gameObject.SetActive(value: true);
				}
			}
		}

		private void ClickItem()
		{
			if (m_ClickAction != null)
			{
				m_ClickAction(m_DataSource, base.gameObject);
			}
		}

		public void ClearItem()
		{
			if (!(this == null) && !(ClickBtn == null) && !(RankImg == null) && !(GoodsName == null) && !(GoodsFiveElement == null) && !(GoodsLevel == null) && !(RankImg == null))
			{
				ClickBtn.onClick.RemoveAllListeners();
				RankImg.gameObject.SetActive(value: false);
				m_DataSource = null;
				GoodsName.text = "";
				GoodsFiveElement.text = "";
				GoodsLevel.text = "";
				GoodsIcon.sprite = null;
				RankImg.sprite = null;
				m_ClickAction = null;
				IsEmpty = true;
			}
		}

		public void ResetLevel(int newLevel)
		{
			GoodsLevel.text = newLevel.ToString();
		}
	}
}
