using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace XiuZhenJie
{
	public class ZhanDouDiaoLuoItem : MonoBehaviour
	{
		public Button Btn;

		public Image ItemIcon;

		public Image WuXingImg;

		public Text ItemName;

		public Text LevelText;

		public BaseNaJieGoods Source;

		private int currentType;

		public void SetItem(BaseNaJieGoods source, int type, UnityAction<BaseNaJieGoods, int, bool> _btnAction, Dictionary<int, Sprite> LevelSpriteDic, Dictionary<WuXing, Sprite> WuXingSpriteDic)
		{
			if (source == null)
			{
				Debug.Log("实例化掉落物品Item失败，数据源是空");
				return;
			}
			Source = source;
			Sprite value = null;
			WuXingSpriteDic.TryGetValue((WuXing)source.p, out value);
			Sprite value2 = null;
			int levelByRank = WorldSetting.GetLevelByRank(source.c);
			LevelSpriteDic.TryGetValue(levelByRank, out value2);
			if (value2 != null)
			{
				WuXingImg.sprite = value2;
			}
			if (value != null)
			{
				ItemIcon.sprite = value;
			}
			LevelText.text = Source.level.ToString();
			currentType = type;
			ItemName.text = Source.e;
			Btn.onClick.AddListener(() =>
			{
				if (_btnAction != null)
				{
					_btnAction(Source, currentType, arg2: true);
				}
			});
		}
	}
}
