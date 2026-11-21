using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace XiuZhenJie
{
	public class RoleZhuangBeiShuXingContentItem : MonoBehaviour
	{
		public Sprite SuperSprite;

		public Sprite TiPoSprite;

		public Sprite ShenShiSprite;

		public Image TitleImg;

		public Text TitleValue;

		public VerticalLayoutGroup VerticalLayout;

		public void SetValue(GongFaType gongFaType, string value, Dictionary<string, string> dateail)
		{
			switch (gongFaType)
			{
			case GongFaType.LianShen:
				TitleImg.sprite = ShenShiSprite;
				TitleValue.text = "神识总值：" + value;
				break;
			case GongFaType.DuanTi:
				TitleImg.sprite = TiPoSprite;
				TitleValue.text = "体魄总值：" + value;
				break;
			case GongFaType.NaQi:
			{
				TitleImg.sprite = SuperSprite;
				int roleLevel = Singleton<RoleManager>.Instance.Role_Entity.RoleLevel;
				if (roleLevel < 31)
				{
					TitleValue.text = "灵气总值：" + value;
				}
				else if (roleLevel < 60)
				{
					TitleValue.text = "真气总值：" + value;
				}
				else
				{
					TitleValue.text = "真元总值：" + value;
				}
				break;
			}
			}
			GameObject original = Singleton<ResourceManager>.Instance.LoadResource<GameObject>("Assets/GameData/Prefabs/Items/RoleZhuangBeiXiangQingItem.prefab");
			RectTransform component = base.transform.GetComponent<RectTransform>();
			int num = 0;
			foreach (KeyValuePair<string, string> item in dateail)
			{
				Object.Instantiate(original, component).GetComponent<RoleZhuangBeiXiangQingItem>().ReSetValue(num, item.Key, item.Value);
				num++;
			}
			float num2 = VerticalLayout.padding.top;
			float spacing = VerticalLayout.spacing;
			float y = (float)((dateail.Count + 1) * 20) + (float)dateail.Count * spacing + 2f * num2;
			component.sizeDelta = new Vector2(component.sizeDelta.x, y);
		}
	}
}
