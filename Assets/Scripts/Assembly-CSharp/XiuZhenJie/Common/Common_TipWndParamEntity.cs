using System.Collections.Generic;
using UnityEngine.Events;

namespace XiuZhenJie
{
	public class Common_TipWndParamEntity
	{
		public UnityAction<object> SureBtnClick;

		public UnityAction<object> CloseBtnClick;

		public string TitleName;

		public object DataSource;

		public string BtnName;

		public string ID;

		public int Rank;

		public List<string> ContentList;
	}
}
