using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace XiuZhenJie
{
	public class BoxItem : MonoBehaviour
	{
		public Button Btn;

		public Image ItemIcon;

		public Image LevelImg;

		public Text ItemName;

		public Text LevelText;

		public void SetItem(BaseNaJieGoods source, UnityAction<BaseNaJieGoods, object[]> btnAction, Dictionary<int, Sprite> levelSpriteDic, Dictionary<WuXing, Sprite> wuXingSpriteDic, params object[] argData)
		{
			if (source == null)
			{
				return;
			}
			Sprite value = null;
			wuXingSpriteDic.TryGetValue((WuXing)source.p, out value);
			Sprite value2 = null;
			int levelByRank = WorldSetting.GetLevelByRank(source.c);
			levelSpriteDic.TryGetValue(levelByRank, out value2);
			if (value2 != null)
			{
				LevelImg.sprite = value2;
			}
			if (value != null)
			{
				ItemIcon.sprite = value;
			}
			LevelText.text = source.level.ToString();
			ItemName.text = source.e;
			Btn.onClick.AddListener(() =>
			{
				if (btnAction != null)
				{
					btnAction(source, argData);
				}
			});
		}
	}
}
