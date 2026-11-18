using System;
using System.Collections.Generic;

namespace XiuZhenJie
{
	[Serializable]
	public class DongFuInfoModel : BaseModel
	{
		public List<GongFaModel> u;

		public KongLingZhenModel c;

		public JuLingFaZhenModel g;
	}
}
