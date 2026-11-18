using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace XiuZhenJie
{
	public class DouMenXuanZeJiNengItem : MonoBehaviour
	{
		public Toggle ChooseTog;

		public Text JiNengName;

		public Text JiNengLevelRankText;

		public Image JiNengImag;

		public Image SettingImg;

		public BattleSettingItemModel m_DataSource;

		public void SetItem(Sprite spr, BattleSettingItemModel dataSource, Action<BattleSettingItemModel, bool> action, Dictionary<int, Sprite> WuXingBgSprite, ToggleGroup toggleGroup)
		{
			m_DataSource = dataSource;
			ChooseTog.group = toggleGroup;
			JiNengName.text = dataSource.name;
			JiNengImag.sprite = spr;
			Sprite value = null;
			string levelByRankName = WorldSetting.GetLevelByRankName(dataSource.rank);
			JiNengLevelRankText.text = $"{levelByRankName}·{m_DataSource.level}";
			WuXingBgSprite.TryGetValue(dataSource.fiveElement, out value);
			ChooseTog.isOn = false;
			if (value != null)
			{
				ChooseTog.gameObject.GetComponent<Image>().sprite = value;
			}
			ChooseTog.onValueChanged.AddListener((bool isOn) =>
			{
				if (action != null)
				{
					action(dataSource, isOn);
				}
			});
		}

		public void SetUsed()
		{
			ChooseTog.enabled = false;
			SettingImg.gameObject.SetActive(value: true);
		}
	}
}
