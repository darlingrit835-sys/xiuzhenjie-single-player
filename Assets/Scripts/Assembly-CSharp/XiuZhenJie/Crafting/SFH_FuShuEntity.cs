namespace XiuZhenJie
{
	public class SFH_FuShuEntity
	{
		public string Id;

		public string Name;

		public int FiveElement;

		public int Rank;

		public int Level;

		public int ExchangeContribution;

		public string Created;

		public SFH_FuShuEntity()
		{
		}

		public SFH_FuShuEntity(EnterMethodSignExchangeResp_1008044.Types.ExchangeMethodSignObjectResp_1008045 data)
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
