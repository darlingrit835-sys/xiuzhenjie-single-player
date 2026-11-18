using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace XiuZhenJie
{
	public class LingQuanJiuSiJieShiPanel : MonoBehaviour
	{
		public Button SendBtn;

		public Button CloseBtn;

		public Text RoleName;

		public Text LevelName;

		public Image TouXiang;

		public Image VIPTouXiang;

		private void Awake()
		{
			CloseBtn.onClick.AddListener(PanelHide);
		}

		public void SetValueAndShow(string name, int level, Sprite touSpirte, UnityAction<string> sendBtnAction, int headFrameId)
		{
			RoleName.text = name;
			LevelName.text = WorldSetting.GetRoleLevelNameByRoleLevel(level);
			TouXiang.sprite = touSpirte;
			if (sendBtnAction != null)
			{
				SendBtn.onClick.RemoveAllListeners();
				SendBtn.onClick.AddListener(() =>
				{
					sendBtnAction(name);
				});
			}
			base.gameObject.SetActive(value: true);
			if (headFrameId == 0)
			{
				VIPTouXiang.gameObject.SetActive(value: false);
			}
			else
			{
				VIPTouXiang.gameObject.SetActive(value: true);
			}
		}

		public void PanelHide()
		{
			base.gameObject.SetActive(value: false);
		}
	}
}
