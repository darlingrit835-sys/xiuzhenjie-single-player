using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace XiuZhenJie
{
	public class CaiLiaoShaiXuanItem : MonoBehaviour
	{
		public Text Name;

		public Toggle toggle;

		private UnityAction<string> Action;

		public void SetItem(string Name, UnityAction<string> action, ToggleGroup group)
		{
			this.Name.text = Name;
			toggle.group = group;
			Action = action;
			toggle.onValueChanged.AddListener(Select);
		}

		private void Select(bool val)
		{
			if (val)
			{
				Action(Name.text);
				Name.color = Color.white;
			}
			else
			{
				Name.color = Color.black;
			}
		}
	}
}
