using System;
using UnityEngine;
using UnityEngine.UI;

namespace XiuZhenJie
{
	public class ChooseServerItem : MonoBehaviour
	{
		public Text NameText;

		public Image IsHaveRole;

		public Button ChooseBtn;

		public Text ChooseBtnText;

		public void SetValue(GameServerInfo serverInfo, Action _action)
		{
			NameText.text = serverInfo.name;
			if (serverInfo.currentRoleVO == null || string.IsNullOrEmpty(serverInfo.currentRoleVO.id))
			{
				IsHaveRole.gameObject.SetActive(value: false);
			}
			else
			{
				IsHaveRole.gameObject.SetActive(value: true);
			}
			if (serverInfo.status == -1)
			{
				ChooseBtnText.text = "维护中";
				ChooseBtn.interactable = false;
				return;
			}
			if (serverInfo.status == 2)
			{
				ChooseBtnText.text = "已满";
				ChooseBtn.interactable = false;
				return;
			}
			ChooseBtnText.text = "入界";
			ChooseBtn.interactable = true;
			ChooseBtn.onClick.AddListener(() =>
			{
				_action();
			});
		}
	}
}
