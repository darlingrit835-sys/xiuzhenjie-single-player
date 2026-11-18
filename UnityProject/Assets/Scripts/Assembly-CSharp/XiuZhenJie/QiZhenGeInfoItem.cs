using UnityEngine;
using UnityEngine.UI;

namespace XiuZhenJie
{
	public class QiZhenGeInfoItem : MonoBehaviour
	{
		public Text TitleText;

		public Text ContentText;

		public void SetItem(string title, string content)
		{
			TitleText.text = title;
			ContentText.text = content;
		}
	}
}
