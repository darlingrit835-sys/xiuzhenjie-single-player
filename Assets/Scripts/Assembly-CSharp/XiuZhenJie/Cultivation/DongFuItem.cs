using System;
using UnityEngine;
using UnityEngine.UI;

namespace XiuZhenJie
{
	public class DongFuItem : MonoBehaviour
	{
		public Button GouMaiBtn;

		private string dongFuLevel;

		public Text DongFuName;

		public Text NongYuDongText;

		public Text NeiShiText;

		public Text YouXiaoQiText;

		public Text LingShiJiaGeText;

		public Text MiaoShuText;

		public long LingShiJiaGe;

		public Image BgImg;

		public string DongFuLevel => dongFuLevel;

		public void SetValue(Sprite bgSprite, string level, string _dongFuNameText, string _nongYuDuText, string _youXiaoQiText, long _lingShiJiaGe, string _describe, Action GouMaiAction)
		{
			string text = "0间";
			BgImg.sprite = bgSprite;
			dongFuLevel = level;
			switch (dongFuLevel)
			{
			case "4":
				text = "1间";
				break;
			case "8":
				text = "2间";
				break;
			case "12":
				text = "2间";
				break;
			case "16":
				text = "2间";
				break;
			}
			MiaoShuText.text = _describe;
			DongFuName.text = _dongFuNameText;
			NongYuDongText.text = _nongYuDuText;
			NeiShiText.text = text;
			YouXiaoQiText.text = "无限期";
			if (Singleton<RoleManager>.Instance.Role_Entity.VipLevel >= 4)
			{
				_lingShiJiaGe = (int)(_lingShiJiaGe - _lingShiJiaGe / 10);
			}
			LingShiJiaGeText.text = CommonMethod.HuoBiLongToString(_lingShiJiaGe, 4);
			LingShiJiaGe = _lingShiJiaGe;
			BtnOnClick(GouMaiAction, _lingShiJiaGe);
		}

		public void BtnOnClick(Action _action, long price)
		{
			GouMaiBtn.onClick.AddListener(() =>
			{
				MonoSingleton<GameStart>.Instance.PopUpAttention($"确认是否花费{price}灵石购买", _action);
			});
		}
	}
}
