using System.Collections.Generic;
using UnityEngine.Events;

namespace XiuZhenJie
{
	public class NaJieTipModel
	{
		public string LeftBtnText;

		public string MiddleBtnText;

		public string RightBtnText;

		public int JiaGe;

		public UnityAction<int, object> LeftBtnClickEvent;

		public UnityAction<int, object> MiddleBtnClickEvent;

		public UnityAction<int, object> RightBtnClickEvent;

		public string Name;

		public List<string> ContentList;

		public object dataSource;

		public int type;

		public bool IsLock;
	}
}
