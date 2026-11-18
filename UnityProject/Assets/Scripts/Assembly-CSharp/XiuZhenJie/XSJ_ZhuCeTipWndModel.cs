using System.Collections.Generic;
using UnityEngine.Events;

namespace XiuZhenJie
{
	public class XSJ_ZhuCeTipWndModel
	{
		public string TiShiName;

		public string NameMingGuiZe;

		public UnityAction<string> QueRenBtnClickAction;

		public bool isCanZhuCe;

		public List<string> detailList;

		public object DataSource;

		public void SetValue(string tiShiName, string nameGuiZe, bool isCanZhuCe, List<string> xiangQing, UnityAction<string> QueRenCallBack, object dataSource)
		{
			DataSource = dataSource;
			TiShiName = tiShiName;
			this.isCanZhuCe = isCanZhuCe;
			detailList = xiangQing;
			QueRenBtnClickAction = QueRenCallBack;
			NameMingGuiZe = nameGuiZe;
		}
	}
}
