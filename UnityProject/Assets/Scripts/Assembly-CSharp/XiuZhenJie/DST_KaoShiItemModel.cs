using System;

namespace XiuZhenJie
{
	[Serializable]
	public class DST_KaoShiItemModel : BaseModel
	{
		public string id;

		public int assessElixirId;

		public string elixirTeacherId;

		public DanFangModel methodElixir;

		public string name;

		public double num;

		public double ask;
	}
}
