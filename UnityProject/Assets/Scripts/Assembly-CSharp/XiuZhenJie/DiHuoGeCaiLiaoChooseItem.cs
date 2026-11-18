using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace XiuZhenJie
{
	public class DiHuoGeCaiLiaoChooseItem : MonoBehaviour
	{
		public Text ZhuanHuanLvText;

		public Text ZhuanHuanLvValueText;

		public Toggle ChooseToggle;

		private int m_Index;

		public void SetValue(BaseNaJieGoodEntity baseData, bool isOn, UnityAction<bool, BaseNaJieGoodEntity, int> ToggleChangeAction, int index)
		{
			if (baseData == null)
			{
				return;
			}
			m_Index = index;
			switch ((GoodsTypeEnum)baseData.Type)
			{
			case GoodsTypeEnum.MATERIAL_ANIMAL:
			case GoodsTypeEnum.MATERIAL_ANIMAL_SKIN:
			case GoodsTypeEnum.MATERIAL_ANIMAL_BLOOD:
			case GoodsTypeEnum.MATERIAL_ANIMAL_BONE:
			case GoodsTypeEnum.MATERIAL_ANIMAL_PELLET:
				ZhuanHuanLvValueText.text = baseData.ConversionRate.ToString("0.0");
				break;
			case GoodsTypeEnum.MATERIAL_PLANT:
				ZhuanHuanLvValueText.text = baseData.ConversionRate.ToString("0.0");
				break;
			case GoodsTypeEnum.MATERIAL_MINERAL:
				ZhuanHuanLvValueText.text = baseData.ConversionRate.ToString("0.0");
				break;
			}
			ChooseToggle.onValueChanged.RemoveAllListeners();
			ChooseToggle.onValueChanged.AddListener((bool isChoose) =>
			{
				if (isChoose)
				{
					ZhuanHuanLvText.color = Color.white;
					ZhuanHuanLvValueText.color = Color.white;
				}
				else
				{
					ZhuanHuanLvText.color = new Color(0.6f, 0.6f, 0.6f, 1f);
					ZhuanHuanLvValueText.color = Color.black;
				}
				if (ToggleChangeAction != null)
				{
					ToggleChangeAction(isChoose, baseData, m_Index);
				}
			});
			ChooseToggle.isOn = isOn;
		}
	}
}
