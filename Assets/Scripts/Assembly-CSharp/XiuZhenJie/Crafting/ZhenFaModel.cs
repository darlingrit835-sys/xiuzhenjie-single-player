using System;
using System.Collections.Generic;

namespace XiuZhenJie
{
	[Serializable]
	public class ZhenFaModel : BaseModel
	{
		public string id;

		public string name;

		public string describe;

		public decimal conversionRate;

		public int fiveElements;

		public int level;

		public decimal restrict;

		public string createdBy;

		public long createdAt;

		public decimal continueSpiritualityValue;

		public int type;

		public decimal receiveVal;

		public string expression;

		private bool isUseing;

		private decimal proficiencyValue;

		private bool isUnderstand;

		public List<string> detailList;
	}
}
