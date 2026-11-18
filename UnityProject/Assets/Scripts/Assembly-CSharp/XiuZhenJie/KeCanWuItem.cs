using System;
using UnityEngine;
using UnityEngine.UI;

namespace XiuZhenJie
{
	public class KeCanWuItem : MonoBehaviour
	{
		public Text ItemName;

		public Text LevelText;

		public Image LevelImg;

		public Image CanWuImg;

		public Button CanWuBtn;

		public void SetValue(string _name, int level, Sprite LevelTagSprite, Sprite wuXingSprite, Action CanWuBtnAction)
		{
			if (LevelTagSprite != null)
			{
				LevelImg.sprite = LevelTagSprite;
			}
			if (wuXingSprite != null)
			{
				CanWuImg.sprite = wuXingSprite;
			}
			LevelText.text = level.ToString();
			ItemName.text = _name;
			CanWuBtn?.onClick.AddListener(() =>
			{
				CanWuBtnAction?.Invoke();
			});
		}
	}
}
