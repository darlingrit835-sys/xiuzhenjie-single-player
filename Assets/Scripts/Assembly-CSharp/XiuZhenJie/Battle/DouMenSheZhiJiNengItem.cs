using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace XiuZhenJie
{
	public class DouMenSheZhiJiNengItem : MonoBehaviour
	{
		public Button Button;

		public Sprite DefaultBgSprite;

		public Image LockImg;

		public Image IcomImg;

		public Text LevelRankText;

		public Text NameText;

		public BattleSettingItemModel resultValue;

		public void SetItem(UnityAction action, UnityAction unLockAction)
		{
			LockImg.enabled = false;
			IcomImg.enabled = true;
			Button.onClick.RemoveAllListeners();
			Button.onClick.AddListener(() =>
			{
				if (action != null)
				{
					action();
				}
			});
		}

		public void SetUsingValue(BattleSettingItemModel source, Sprite _imgSprite, Dictionary<int, Sprite> wuXingBgSprite)
		{
			if (source != null)
			{
				resultValue = source;
				IcomImg.gameObject.SetActive(value: true);
				IcomImg.sprite = _imgSprite;
				NameText.text = source.name;
				string levelByRankName = WorldSetting.GetLevelByRankName(source.rank);
				LevelRankText.text = $"{levelByRankName}·{source.level}";
				Sprite value = null;
				wuXingBgSprite.TryGetValue(source.fiveElement, out value);
				if (value != null)
				{
					Button.gameObject.GetComponent<Image>().sprite = value;
				}
			}
		}

		public void ClearValue()
		{
			resultValue = null;
			Button.gameObject.GetComponent<Image>().sprite = DefaultBgSprite;
			IcomImg.gameObject.SetActive(value: false);
			IcomImg.sprite = null;
		}
	}
}
