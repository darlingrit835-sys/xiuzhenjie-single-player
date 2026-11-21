namespace XiuZhenJie
{
	public class ZS_ZhenTuEntity
	{
		public string Id;

		public string Name;

		public int FiveElement;

		public int Rank;

		public int Level;

		public int ExchangeContribution;

		public string Created;

		public ZS_ZhenTuEntity(EnterMethodMatrixExchangeResp_1008085.Types.ExchangeMethodMatrixObjectResp_1008086 data)
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
