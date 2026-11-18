using System;
using UnityEngine;
using UnityEngine.UI;

namespace XiuZhenJie
{
	public class DangFang_DuiHuanPanel : MonoBehaviour
	{
		public Text ItemName;

		public Text PlayerName;

		public Text Price;

		public Button Btn;

		public void SetValue(string _itemName, string _playerName, int _price, Action _action)
		{
			ItemName.text = _itemName;
			PlayerName.text = _playerName;
			Price.text = _price.ToString();
			ButtonClick(_action);
		}

		private void ButtonClick(Action action)
		{
			Btn.onClick.AddListener(() =>
			{
				Singleton<UIManager>.Instance.PopUpWnd("DanFangXinXiWnd.prefab", true, 1);
			});
		}
	}
}
