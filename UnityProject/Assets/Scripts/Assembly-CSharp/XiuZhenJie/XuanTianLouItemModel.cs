using System;
using System.Collections.Generic;

namespace XiuZhenJie
{
	[Serializable]
	public class XuanTianLouItemModel : BaseModel
	{
		public string id;

		public int goodsType;

		public string goodsModelId;

		public double goodsVolume;

		public string name;

		public double systemPrice;

		public string danwei;

		public string describe;

		public int level;

		public int fiveElement;

		public int rank;

		public int sellType;

		public List<string> wuPinProperty;
	}
}
