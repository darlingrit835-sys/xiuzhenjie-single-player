using System;
using System.Collections.Generic;

namespace XiuZhenJie
{
	[Serializable]
	public class RoleWuPinModel : BaseModel
	{
		public string id;

		public string name;

		public double volume;

		public double systemPrice;

		public double recyclePrice;

		public int goodsType;

		public int type;

		public int rank;

		public double conversionRate;

		public string unit;

		public string bookId;

		public int fiveElement;

		public string materialsId;

		public string isAuth;

		public List<string> wuPinProperty;

		public double usedPerception;
	}
}
