using System;
using System.Collections.Generic;

namespace XiuZhenJie
{
	[Serializable]
	public class RoleBatchXiaoHuiWuPinModel : BaseModel
	{
		public List<GoodsData> data;

		public string roleId;
	}
}
