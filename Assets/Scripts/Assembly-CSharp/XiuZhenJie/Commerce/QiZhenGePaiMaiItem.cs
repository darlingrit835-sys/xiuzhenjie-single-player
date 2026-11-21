using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace XiuZhenJie
{
	public class QiZhenGePaiMaiItem : MonoBehaviour
	{
		public Text ItemName;

		public Button btn;

		public Image itemBg;

		public Image LevelImg;

		public Text LevelText;

		private BaseNaJieGoodEntity dataSource;

		public void SetValue(BaseNaJieGoodEntity item, Dictionary<int, Sprite> LevelSpriteDic, Dictionary<WuXing, Sprite> WuXingSpriteDic, UnityAction<BaseNaJieGoodEntity> _action)
		{
			dataSource = item;
			if (dataSource != null)
			{
				ItemName.text = item.Name;
				Sprite value = null;
				WuXingSpriteDic.TryGetValue((WuXing)dataSource.FiveElement, out value);
				Sprite value2 = null;
				int levelByRank = WorldSetting.GetLevelByRank(dataSource.Rank);
				LevelSpriteDic.TryGetValue(levelByRank, out value2);
				if (value != null)
				{
					itemBg.sprite = value;
				}
				if (value2 != null)
				{
					LevelImg.sprite = value2;
				}
				LevelText.text = dataSource.Level.ToString();
				btn.onClick.AddListener(() =>
				{
					_action?.Invoke(dataSource);
				});
			}
		}
	}
}
