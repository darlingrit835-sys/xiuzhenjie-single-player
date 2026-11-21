using System;

namespace XiuZhenJie
{
	[Serializable]
	public class QZG_SearchWuPinListParamModel : BaseModel
	{
		public string name;

		public int type;

		public int smallType;

		public int stage;

		public int auctionType;

		public int fiveElement;

		public int rank;

		public long minFinalPrice;

		public long maxFinalPrice;

		public string finalPriceSort;

		public string rankSort;

		public int pageSize;

		public int pageNum;
	}
}
