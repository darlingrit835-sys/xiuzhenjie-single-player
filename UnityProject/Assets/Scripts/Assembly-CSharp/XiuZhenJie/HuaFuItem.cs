using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace XiuZhenJie
{
	public class HuaFuItem : MonoBehaviour
	{
		public Image ChooseImg;

		public Text ItemName;

		public RectTransform ListContent;

		public RectTransform Content;

		public List<HuaFuItemChild> childList = new List<HuaFuItemChild>();

		public void SetValue(Sprite chooseImgSprite, string name)
		{
			ChooseImg.sprite = chooseImgSprite;
			ItemName.text = name;
		}

		public void AddItemChild(GameObject itemChild, string childName, Sprite BGChooseSprite, Sprite BGNoChooseSprite, bool isChoose, Sprite LogoSprite, Action BtnAction)
		{
			itemChild.transform.SetParent(Content);
			HuaFuItemChild itemHuaFuItemChild = itemChild.GetComponent<HuaFuItemChild>();
			if (isChoose)
			{
				itemHuaFuItemChild.childBg.sprite = BGChooseSprite;
			}
			else
			{
				itemHuaFuItemChild.childBg.sprite = BGNoChooseSprite;
			}
			itemHuaFuItemChild.childName = childName;
			itemHuaFuItemChild.childBtn.onClick.AddListener(() =>
			{
				if (BtnAction != null)
				{
					BtnAction();
				}
				ChooseImg.sprite = LogoSprite;
				ItemName.text = childName;
				foreach (HuaFuItemChild child in childList)
				{
					if (child.gameObject != itemHuaFuItemChild.gameObject)
					{
						child.gameObject.GetComponent<HuaFuItemChild>().childBg.sprite = BGNoChooseSprite;
					}
				}
			});
			itemHuaFuItemChild.childLogo.sprite = LogoSprite;
			childList.Add(itemHuaFuItemChild);
		}
	}
}
