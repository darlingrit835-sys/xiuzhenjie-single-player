using System;

namespace XiuZhenJie
{
	[Serializable]
	public class BizInfo : BaseModel
	{
		public string code { get; set; }

		public string msg { get; set; }

		public string dataString { get; set; }
	}
}
