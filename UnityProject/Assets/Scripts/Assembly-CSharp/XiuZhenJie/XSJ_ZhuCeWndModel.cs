using System.Collections.Generic;
using UnityEngine.Events;

namespace XiuZhenJie
{
	public class XSJ_ZhuCeWndModel<T> where T : class
	{
		public string TitleText;

		public string LevelInfo;

		public string GongXianDian;

		public string YiZhuCeTitleName;

		public List<XSJ_YiZhuCeItemModel> YiZhuCeItemList;

		public List<T> YiZhuCeDataSource;

		public List<string> DaiShenHeItemList;

		public List<T> DaiShenHeDataSource;

		public string DaiShenHeTitleName;

		public List<string> KeZhuCeItemList;

		public List<T> KeZhuCeDataSource;

		public string KeZhuCeTitleName;

		public UnityAction<T> YiZhuCeClickAction;

		public UnityAction<T> DaiShenHeClickAction;

		public UnityAction<T> KeZhuCeClickAction;
	}
}
