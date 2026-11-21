using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace XiuZhenJie
{
	public class DiHuoGeChooseSmallItem : MonoBehaviour
	{
		public Toggle ItemTog;

		public Text ItemText;

		public DanFangModel dataSource;

		public void SetValue(DanFangModel data, string name, int index, ToggleGroup toggleGroup, UnityAction<string, int, DanFangModel> ToggleClickAction, bool isChoose = false)
		{
			dataSource = data;
			ItemText.text = name;
			ItemTog.group = toggleGroup;
			ItemTog.onValueChanged.RemoveAllListeners();
			ItemTog.onValueChanged.AddListener((bool isOn) =>
			{
				if (isOn)
				{
					Singleton<VoiceManager>.Instance.PlayVoice(VoiceType.BtnClick);
					ItemText.color = Color.white;
					if (ToggleClickAction != null)
					{
						ToggleClickAction(name, index, dataSource);
					}
				}
				else
				{
					ItemText.color = Color.black;
				}
			});
			ItemTog.isOn = isChoose;
		}

		public void Clear()
		{
			Object.Destroy(base.gameObject);
		}
	}
}
