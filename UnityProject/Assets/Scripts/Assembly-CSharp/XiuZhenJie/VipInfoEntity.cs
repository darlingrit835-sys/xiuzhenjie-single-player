using System.Collections.Generic;

namespace XiuZhenJie
{
	public class VipInfoEntity
	{
		public int Id;

		public string Name;

		public int TopPrice;

		public List<string> Effect;

		public List<VipLiBaoGoodsInfoEntity> LiBao;
	}
}
