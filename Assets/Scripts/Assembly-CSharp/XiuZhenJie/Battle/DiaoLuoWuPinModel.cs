using System;
using System.Collections.Generic;

namespace XiuZhenJie
{
	[Serializable]
	public class DiaoLuoWuPinModel : BaseModel
	{
		public string id;

		public string name;

		public int type;

		public int goodsType;

		public double volume;

		public int fiveElement;

		public int rank;

		public string bookId;

		public double recyclePrice;

		public List<string> wuPinProperty;

		public double usedPerception;
	}
}
