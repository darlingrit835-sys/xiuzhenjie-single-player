using System;
using System.Collections.Generic;

namespace XiuZhenJie
{
	[Serializable]
	public class CanWuModel : BaseModel
	{
		public List<GongFaModel> a;

		public List<DanFangModel> b;

		public List<FaShuModel> c;

		public List<FuShuModel> d;
	}
}
