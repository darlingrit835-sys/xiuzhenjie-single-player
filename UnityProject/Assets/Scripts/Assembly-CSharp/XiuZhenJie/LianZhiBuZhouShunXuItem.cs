using UnityEngine;
using UnityEngine.UI;

namespace XiuZhenJie
{
	public class LianZhiBuZhouShunXuItem : MonoBehaviour
	{
		public Text MiaoShu;

		public void SetItem(string str)
		{
			MiaoShu.text = str;
		}
	}
}
