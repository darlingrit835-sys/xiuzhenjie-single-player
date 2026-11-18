using System;
using System.Collections.Generic;

namespace XiuZhenJie
{
	[Serializable]
	public class ShangPuItemModel : BaseModel
	{
		public string goodsModelId;

		public int goodsType;

		public int rank;

		public int fiveElement;

		public int level;

		public string name;

		public double systemPrice;

		public string describe;

		public double goodsVolume;

		public List<string> wuPinProperty;
	}
}
