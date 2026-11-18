using System;

namespace XiuZhenJie
{
	[Serializable]
	public class ClientPracticeArticleElixirStepData : BaseModel
	{
		public string stepElixirId;

		public string goodsId;

		[NonSerialized]
		public int goodsType;

		public double bestTime;
	}
}
