using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace XiuZhenJie
{
	public class Common_ShaiXuanBigItem : MonoBehaviour
	{
		public Text TtitleText;

		public RectTransform Content;

		public Toggle AllItemObj;

		private UnityAction<ShaiXuanParamsModel> ChooseAction;

		private ShaiXuanParamsModel m_DataSource;

		public float SetItem(Window window, string titleName, List<ShaiXuanParamsModel> itemList, UnityAction<ShaiXuanParamsModel> unityAction)
		{
			TtitleText.text = titleName;
			ChooseAction = unityAction;
			if (itemList != null && itemList.Count > 0)
			{
				int num = 0;
				foreach (ShaiXuanParamsModel item in itemList)
				{
					Toggle toggle = Object.Instantiate(AllItemObj, Content);
					Text text = toggle.gameObject.transform.Find("Text").gameObject.GetComponent<Text>();
					text.text = item.ItemName;
					toggle.gameObject.SetActive(value: true);
					toggle.onValueChanged.RemoveAllListeners();
					toggle.onValueChanged.AddListener((bool isOn) =>
					{
						if (isOn)
						{
							text.color = Color.white;
						}
						else
						{
							text.color = Color.black;
						}
						ChooseItem(isOn, item);
					});
					if (num == 0)
					{
						toggle.isOn = true;
						num++;
					}
				}
				GridLayoutGroup component = Content.gameObject.GetComponent<GridLayoutGroup>();
				window.SetGrticalListContentHight(itemList.Count, Content, component);
				Content.anchoredPosition = new Vector2(0f, 0f - TtitleText.rectTransform.sizeDelta.y);
			}
			float num2 = TtitleText.rectTransform.sizeDelta.y + Content.sizeDelta.y;
			base.gameObject.GetComponent<RectTransform>().sizeDelta = new Vector2(base.gameObject.GetComponent<RectTransform>().sizeDelta.x, num2);
			return num2 + 10f;
		}

		private void ChooseItem(bool isOn, ShaiXuanParamsModel value)
		{
			if (isOn && ChooseAction != null)
			{
				ChooseAction(value);
			}
		}
	}
}
