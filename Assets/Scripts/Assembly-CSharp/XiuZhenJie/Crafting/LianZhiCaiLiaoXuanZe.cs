using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace XiuZhenJie
{
	public class LianZhiCaiLiaoXuanZe : MonoBehaviour
	{
		public Text MiaoShu;

		public Toggle toggle;

		public void SetItem(string str, UnityAction<string> action, ToggleGroup group)
		{
			MiaoShu.text = str;
			toggle.group = group;
			toggle.onValueChanged.AddListener((bool val) =>
			{
				action(str);
			});
		}
	}
}
