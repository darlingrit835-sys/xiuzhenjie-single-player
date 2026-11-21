using System;
using UnityEngine;
using UnityEngine.UI;

namespace XiuZhenJie
{
	public class ShangPuItem : MonoBehaviour
	{
		public Button ItemInformation;

		public Image LevelTagImg;

		public Text levelText;

		public Image WuXingBgImg;

		public Text ItemName;

		public Text ItemPrice;

		public ShangPuItemModel _modelSource;

		public void SetValue(ShangPuItemModel source, Action<ShangPuItemModel> _itemInformationAction)
		{
			_modelSource = source;
			if (source != null)
			{
				int levelByRank = WorldSetting.GetLevelByRank(source.rank);
				levelText.text = source.level.ToString();
				Sprite sprite = null;
				sprite = levelByRank switch
				{
					1 => Singleton<ResourceManager>.Instance.LoadResource<Sprite>("Assets/GameData/Textures/Item/Level_Tag_1.png"), 
					2 => Singleton<ResourceManager>.Instance.LoadResource<Sprite>("Assets/GameData/Textures/Item/Level_Tag_2.png"), 
					3 => Singleton<ResourceManager>.Instance.LoadResource<Sprite>("Assets/GameData/Textures/Item/Level_Tag_3.png"), 
					4 => Singleton<ResourceManager>.Instance.LoadResource<Sprite>("Assets/GameData/Textures/Item/Level_Tag_4.png"), 
					_ => Singleton<ResourceManager>.Instance.LoadResource<Sprite>("Assets/GameData/Textures/Item/Level_Tag_1.png"), 
				};
				LevelTagImg.sprite = sprite;
				WuXingBgImg.color = WorldSetting.WuXingColor((WuXing)source.fiveElement);
				ItemName.text = _modelSource.name;
				string arg = WorldSetting.simplifyNumber((ulong)_modelSource.systemPrice);
				ItemPrice.text = $"{arg}灵石";
				BtnOnClick(_itemInformationAction);
			}
		}

		public void BtnOnClick(Action<ShangPuItemModel> gouMaiAction)
		{
			ItemInformation.onClick.AddListener(() =>
			{
				if (gouMaiAction != null)
				{
					gouMaiAction(_modelSource);
				}
			});
		}
	}
}
