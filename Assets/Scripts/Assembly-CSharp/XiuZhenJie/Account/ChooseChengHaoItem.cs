using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace XiuZhenJie
{
	public class ChooseChengHaoItem : MonoBehaviour
	{
		public Button ChooseBtn;

		public Text NameText;

		public void SetValue(int id, UnityAction<ChengHaoInfoEntity> _btnClickAction)
		{
			Dictionary<int, ChengHaoInfoEntity> dictionary = Singleton<FacadeManager>.Instance.SelectChengHaoInfo();
			ChengHaoInfoEntity data = null;
			if (dictionary.TryGetValue(id, out data))
			{
				if (string.IsNullOrEmpty(data.EffectStr))
				{
					NameText.text = string.Format("{0}({1}<color=#A00200>{2}</color>)", data.Name, "无", "");
				}
				else if (data.EffectValue.Equals(0))
				{
					NameText.text = string.Format("{0}({1}<color=#A00200>{2}</color>)", data.Name, data.EffectStr, "");
				}
				else if (data.TypeName == "师徒" || data.TypeName == "宗门" || data.TypeName == "抽奖" || data.Type == 21)
				{
					NameText.text = $"{data.Name}({data.EffectStr}<color=#A00200>{data.EffectValue}</color>)";
				}
				else
				{
					NameText.text = string.Format("{0}({1}<color=#A00200>{2}</color>)", data.Name, data.EffectStr, data.EffectValue + "%");
				}
			}
			ChooseBtn.onClick.RemoveAllListeners();
			ChooseBtn.onClick.AddListener(() =>
			{
				if (_btnClickAction != null)
				{
					_btnClickAction(data);
				}
			});
		}
	}
}
