using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace XiuZhenJie
{
	public class RoleZhuangBeiWuXingContentPanel : MonoBehaviour
	{
		public Toggle JinTog;

		public Toggle MuTog;

		public Toggle ShuiTog;

		public Toggle HuoTog;

		public Toggle TuTog;

		public Toggle WuXingTog;

		public UnityAction<int> WuXingTogClickAction;

		private void Awake()
		{
			JinTog.onValueChanged.RemoveAllListeners();
			MuTog.onValueChanged.RemoveAllListeners();
			ShuiTog.onValueChanged.RemoveAllListeners();
			HuoTog.onValueChanged.RemoveAllListeners();
			TuTog.onValueChanged.RemoveAllListeners();
			WuXingTog.onValueChanged.RemoveAllListeners();
			JinTog.onValueChanged.AddListener((bool isOn) =>
			{
				TogClick(1, isOn);
			});
			MuTog.onValueChanged.AddListener((bool isOn) =>
			{
				TogClick(2, isOn);
			});
			ShuiTog.onValueChanged.AddListener((bool isOn) =>
			{
				TogClick(3, isOn);
			});
			HuoTog.onValueChanged.AddListener((bool isOn) =>
			{
				TogClick(4, isOn);
			});
			TuTog.onValueChanged.AddListener((bool isOn) =>
			{
				TogClick(5, isOn);
			});
			WuXingTog.onValueChanged.AddListener((bool isOn) =>
			{
				TogClick(0, isOn);
			});
		}

		private void TogClick(int stage, bool isOn)
		{
			if (isOn && WuXingTogClickAction != null)
			{
				WuXingTogClickAction(stage);
			}
		}
	}
}
