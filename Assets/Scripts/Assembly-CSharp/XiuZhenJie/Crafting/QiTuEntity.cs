using System.Collections.Generic;

namespace XiuZhenJie
{
	public class QiTuEntity
	{
		public string Id;

		public string Name;

		public int Stage;

		public int Icon;

		public int Rank;

		public int Type;

		public int FiveElement;

		public int MaxChuiLianTimes;

		public List<QiTuStepModel> Points;

		public List<QiTuCaiLiaoEntity> CaiLiaoList;
	}
}
