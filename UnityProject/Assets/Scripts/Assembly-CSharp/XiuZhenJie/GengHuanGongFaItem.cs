using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace XiuZhenJie
{
	public class GengHuanGongFaItem : MonoBehaviour
	{
		public Image BgImg;

		public Image MaskImg;

		public Image WuXingTagImg;

		public Text GongFaNameText;

		public Text LevelNum;

		public Action<GengHuanGongFaItem> BtnClickEvent;

		public string Id;

		public string Name;

		public GongFaEntity gongFaModel;

		public void SetValue(GongFaEntity gongFaModel, Dictionary<WuXing, Sprite> wuXingTagImgDic, Dictionary<int, Sprite> levelImgDic, bool isChoose = false)
		{
			Sprite value = null;
			wuXingTagImgDic.TryGetValue((WuXing)gongFaModel.FiveElement, out value);
			int levelByRank = WorldSetting.GetLevelByRank(gongFaModel.Rank);
			Sprite value2 = null;
			levelImgDic.TryGetValue(levelByRank, out value2);
			if (value2 != null)
			{
				WuXingTagImg.sprite = value2;
			}
			if (value != null)
			{
				BgImg.sprite = value;
			}
			this.gongFaModel = gongFaModel;
			Id = gongFaModel.Id;
			LevelNum.text = gongFaModel.Level.ToString();
			Name = gongFaModel.Name;
			GongFaNameText.text = Name;
			Button component = GetComponent<Button>();
			component.onClick.RemoveAllListeners();
			component.onClick.AddListener(() =>
			{
				if (BtnClickEvent != null)
				{
					BtnClickEvent(this);
				}
			});
			if (isChoose)
			{
				ChooseItem();
			}
		}

		public void ChooseItem()
		{
			GetComponent<Button>().interactable = false;
		}

		public void RestoreItem()
		{
			GetComponent<Button>().interactable = true;
		}
	}
}
