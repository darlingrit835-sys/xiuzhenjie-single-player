using UnityEngine;
using UnityEngine.UI;

namespace XiuZhenJie
{
	public class AutoAttackMaskPanel : MonoBehaviour
	{
		public Button HuiFuShouDongBtn;

		public Button AutoAttackBtn;

		public RectTransform Mask;

		private void Awake()
		{
			HuiFuShouDongBtn.onClick.AddListener(() =>
			{
			});
			AutoAttackBtn.onClick.AddListener(OpenAutoAttack);
		}

		private void OpenAutoAttack()
		{
			Mask.gameObject.SetActive(value: true);
			AutoAttackBtn.interactable = false;
		}
	}
}
