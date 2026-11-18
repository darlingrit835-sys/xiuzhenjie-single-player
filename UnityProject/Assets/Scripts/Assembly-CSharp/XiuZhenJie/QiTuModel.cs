using System;
using System.Collections.Generic;

namespace XiuZhenJie
{
	[Serializable]
	public class QiTuModel : BaseModel
	{
		public string goodsId;

		public string name;

		public bool isUseing;

		public bool isUnderstand;

		public List<string> detailList;
	}
}
