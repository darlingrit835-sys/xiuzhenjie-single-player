using UnityEngine;
using UnityEngine.UI;

namespace XiuZhenJie
{
	public class DanFang_YaoCaiItem : MonoBehaviour
	{
		public Text ItemName;

		public void SetValue(string _itemName)
		{
			ItemName.text = _itemName;
		}
	}
}
