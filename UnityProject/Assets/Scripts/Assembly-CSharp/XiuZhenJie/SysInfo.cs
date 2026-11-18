using System;

namespace XiuZhenJie
{
	[Serializable]
	public class SysInfo : BaseModel
	{
		public int httpCode { get; set; }

		public string msg { get; set; }

		public long timestamp { get; set; }
	}
}
