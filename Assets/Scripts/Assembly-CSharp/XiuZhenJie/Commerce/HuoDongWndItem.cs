using System;
using UnityEngine;
using UnityEngine.UI;

namespace XiuZhenJie
{
	public class HuoDongWndItem : MonoBehaviour
	{
		public Image TitleImg;

		public Text TitleText;

		public Text ValueText;

		public Text ContentText;

		public Button GouMaiBtn;

		public Text GouMaiBtnText;

		public string id;

		public int JiaGe;

		public void SetValue(string _id, int _jiage, string _titleText, string _valueText, string _contentText, Action _buyClickAction)
		{
			id = _id;
			JiaGe = _jiage;
			TitleText.text = _titleText;
			ValueText.text = _valueText;
			ContentText.text = _contentText;
			SetBuyBtnAction(_buyClickAction);
		}

		public void SetBuyBtnAction(Action _btnClickAction)
		{
			object[] ActionParams = new object[2];
			ActionParams[0] = "是否確認購買";
			ActionParams[1] = _btnClickAction;
			GouMaiBtn.onClick.AddListener(() =>
			{
				Singleton<UIManager>.Instance.PopUpWnd("AttentionWnd.prefab", bTop: true, ActionParams);
			});
		}
	}
}
