using UnityEngine;
using UnityEngine.UI;

namespace XiuZhenJie
{
	public class RoleZhuangBeiXiangQingItem : MonoBehaviour
	{
		public Text ContentText;

		public Image BackGrounBg;

		public void SetValue(int index, string content)
		{
			if (index % 2 == 0)
			{
				BackGrounBg.enabled = true;
			}
			else
			{
				BackGrounBg.enabled = false;
			}
			ContentText.text = content;
		}

		public void ReSetValue(int index, string title, string value)
		{
			if (index % 2 == 0)
			{
				BackGrounBg.enabled = true;
			}
			else
			{
				BackGrounBg.enabled = false;
			}
			ContentText.text = $"{title}：{value}";
		}

		private void BtnClick()
		{
		}
	}
}
