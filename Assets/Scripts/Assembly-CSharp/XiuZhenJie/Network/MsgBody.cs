using System;

namespace XiuZhenJie
{
	[Serializable]
	public class MsgBody : BaseModel
	{
		public SysInfo sysInfo;

		public BizInfo bizInfo;
	}
}
