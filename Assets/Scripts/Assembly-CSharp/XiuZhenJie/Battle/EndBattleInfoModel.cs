using System;
using System.Collections.Generic;

namespace XiuZhenJie
{
	[Serializable]
	public class EndBattleInfoModel : BaseModel
	{
		public List<DiaoLuoGoodsData> getGoodsIds;

		public List<DiaoLuoGoodsData> discardGoodsIds;

		public int pureBlood;

		public List<ClientSignData> signList;
	}
}
