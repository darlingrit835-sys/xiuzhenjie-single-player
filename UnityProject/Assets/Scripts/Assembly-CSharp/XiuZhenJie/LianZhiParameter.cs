using System;
using System.Collections.Generic;

namespace XiuZhenJie
{
	[Serializable]
	public class LianZhiParameter : BaseModel
	{
		public string elixirId;

		public List<ClientPracticeArticleElixirStepData> list;

		public bool isSuccess;
	}
}
