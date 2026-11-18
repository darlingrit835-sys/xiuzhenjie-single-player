using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace XiuZhenJie
{
	public class RoleZhuangBeiRankContentPanel : MonoBehaviour
	{
		public Toggle TianTog;

		public Toggle DiTog;

		public Toggle XuanTog;

		public Toggle HuangTog;

		public UnityAction<int> RankTogClickAction;

		private void Awake()
		{
			TianTog.onValueChanged.RemoveAllListeners();
			DiTog.onValueChanged.RemoveAllListeners();
			XuanTog.onValueChanged.RemoveAllListeners();
			HuangTog.onValueChanged.RemoveAllListeners();
			HuangTog.onValueChanged.AddListener((bool isOn) =>
			{
				TogClick(1, isOn);
			});
			XuanTog.onValueChanged.AddListener((bool isOn) =>
			{
				TogClick(2, isOn);
			});
			DiTog.onValueChanged.AddListener((bool isOn) =>
			{
				TogClick(3, isOn);
			});
			TianTog.onValueChanged.AddListener((bool isOn) =>
			{
				TogClick(4, isOn);
			});
		}

		private void TogClick(int rank, bool isOn)
		{
			if (isOn && RankTogClickAction != null)
			{
				RankTogClickAction(rank);
			}
		}
	}
}
