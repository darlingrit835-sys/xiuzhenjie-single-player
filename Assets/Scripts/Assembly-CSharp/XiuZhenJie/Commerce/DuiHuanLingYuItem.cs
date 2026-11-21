using System;
using UnityEngine;
using UnityEngine.UI;

namespace XiuZhenJie
{
	public class DuiHuanLingYuItem : MonoBehaviour
	{
		public Text LingYuValueText;

		public Text GouMaiBtnText;

		public Button GouMaiBtn;

		public Image BackGroundImg;

		public string id;

		public int JiaGe;

		public void SetValue(string _id, int _jiage, string _lingyuValue, Action _gouMaiAction)
		{
			id = _id;
			JiaGe = _jiage;
			LingYuValueText.text = _lingyuValue + "個";
			GouMaiBtnText.text = "￥" + JiaGe + " 購買";
			SetBtnAction(_gouMaiAction);
		}

		public void SetBtnAction(Action _gouMaiAction)
		{
			object[] ActionParams = new object[2];
			ActionParams[0] = "是否確認購買";
			ActionParams[1] = _gouMaiAction;
			GouMaiBtn.onClick.AddListener(() =>
			{
				Singleton<UIManager>.Instance.PopUpWnd("AttentionWnd.prefab", bTop: true, ActionParams);
			});
		}
	}
}
