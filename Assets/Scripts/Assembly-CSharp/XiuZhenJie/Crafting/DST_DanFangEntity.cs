namespace XiuZhenJie
{
	public class DST_DanFangEntity
	{
		public string Id;

		public string Name;

		public int FiveElement;

		public int Rank;

		public int Level;

		public int ExchangeContribution;

		public string Created;

		public DST_DanFangEntity()
		{
		}

		public DST_DanFangEntity(EnterMethodElixirExchangeResp_1008012.Types.ExchangeMethodElixirObjectResp_1008013 data)
		{
			Id = data.Id;
			Name = data.Name;
			FiveElement = data.FiveElement;
			Rank = data.Rank;
			Level = data.Level;
			ExchangeContribution = data.ExchangeContribution;
			Created = data.Created;
		}
	}
}
