using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace XiuZhenJie
{
	public class RoleZhuangBeiStageContentPanel : MonoBehaviour
	{
		public Toggle LianQiTog;

		public Toggle ZhuJiTog;

		public Toggle JieDanTog;

		public UnityAction<int> StageTogClickAction;

		private void Awake()
		{
			LianQiTog.onValueChanged.RemoveAllListeners();
			ZhuJiTog.onValueChanged.RemoveAllListeners();
			JieDanTog.onValueChanged.RemoveAllListeners();
			LianQiTog.onValueChanged.AddListener((bool isOn) =>
			{
				TogClick(1, isOn);
			});
			ZhuJiTog.onValueChanged.AddListener((bool isOn) =>
			{
				TogClick(2, isOn);
			});
			JieDanTog.onValueChanged.AddListener((bool isOn) =>
			{
				TogClick(3, isOn);
			});
		}

		private void TogClick(int stage, bool isOn)
		{
			if (isOn && StageTogClickAction != null)
			{
				StageTogClickAction(stage);
			}
		}
	}
}
