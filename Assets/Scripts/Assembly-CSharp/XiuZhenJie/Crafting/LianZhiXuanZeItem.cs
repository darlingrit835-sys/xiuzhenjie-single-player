using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace XiuZhenJie
{
	public class LianZhiXuanZeItem : MonoBehaviour
	{
		public Text MiaoShu;

		public Toggle toggle;

		public void SetItem(string MiaoShu, UnityAction<bool> action, ToggleGroup group)
		{
			this.MiaoShu.text = MiaoShu;
			toggle.onValueChanged.AddListener(action);
			toggle.group = group;
		}
	}
}
