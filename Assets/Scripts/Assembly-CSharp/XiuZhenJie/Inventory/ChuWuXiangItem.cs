using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace XiuZhenJie
{
	public class ChuWuXiangItem : MonoBehaviour
	{
		public Button Btn;

		public Image ItemIcon;

		public Image LevelImg;

		public Text ItemName;

		public Text LevelText;

		public BaseNaJieGoodEntity Source;

		private int currentType;

		public void SetItem(BaseNaJieGoodEntity source, int type, UnityAction<BaseNaJieGoodEntity, int> _btnAction, Dictionary<int, Sprite> LevelSpriteDic, Dictionary<WuXing, Sprite> WuXingSpriteDic)
		{
			Source = source;
			Sprite value = null;
			WuXingSpriteDic.TryGetValue((WuXing)source.FiveElement, out value);
			Sprite value2 = null;
			int levelByRank = WorldSetting.GetLevelByRank(source.Rank);
			LevelSpriteDic.TryGetValue(levelByRank, out value2);
			if (value2 != null)
			{
				LevelImg.sprite = value2;
			}
			if (value != null)
			{
				ItemIcon.sprite = value;
			}
			LevelText.text = Source.Level.ToString();
			currentType = type;
			ItemName.text = Source.Name;
			Btn.onClick.AddListener(() =>
			{
				if (_btnAction != null)
				{
					_btnAction(Source, currentType);
				}
			});
		}
	}
}
