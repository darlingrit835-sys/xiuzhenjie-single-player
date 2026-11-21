using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace XiuZhenJie
{
	public class UI_GoodsTogItem : MonoBehaviour
	{
		public Text GoodsName;

		public Text GoodsLevel;

		public Text GoodsFiveElement;

		public Toggle ClickTog;

		public Image GoodsIcon;

		public Image RankImg;

		public Image isUsingImg;

		public Image NumImg;

		public Text NumText;

		private UnityAction<bool, object, GameObject> m_ClickAction;

		private object m_DataSource;

		public object DataSource
		{
			get
			{
				return m_DataSource;
			}
			private set
			{
				m_DataSource = value;
			}
		}

		public void SetValue(object dataSource, string name, int fiveElement, int level, Sprite rankSpr, Sprite iconSpr, ToggleGroup group, UnityAction<bool, object, GameObject> ClickAction, bool isOn = false, int num = 1)
		{
			m_DataSource = dataSource;
			GoodsName.text = name;
			GoodsFiveElement.text = WorldSetting.GetSignleWuXingName((WuXing)fiveElement);
			GoodsLevel.text = level.ToString();
			InkInventoryArt.StyleItem(GoodsIcon, iconSpr, GoodsName, GoodsFiveElement, NumImg, NumText);
			InkInventoryArt.StyleRank(RankImg, rankSpr);
			m_ClickAction = ClickAction;
			if (group != null)
			{
				ClickTog.group = group;
			}
			ClickTog.isOn = isOn;
			ClickTog.onValueChanged.AddListener(ValueChange);
			RankImg.gameObject.SetActive(value: true);
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

		private void ValueChange(bool isOn)
		{
			if (m_ClickAction != null)
			{
				m_ClickAction(isOn, m_DataSource, base.gameObject);
			}
		}

		public void ReduceNumText()
		{
			int result = 1;
			if (int.TryParse(NumText.text, out result) && result > 1)
			{
				result--;
				NumText.text = result.ToString();
			}
		}
	}
}
