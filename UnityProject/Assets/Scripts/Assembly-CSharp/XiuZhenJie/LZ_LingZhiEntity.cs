namespace XiuZhenJie
{
	public class LZ_LingZhiEntity
	{
		public string Id;

		public string Name;

		public int FiveElement;

		public int Rank;

		public int Level;

		public int ExchangeContribution;

		public string Created;

		public LZ_LingZhiEntity(EnterTemplatePlantExchangeResp_1008055.Types.ExchangeTemplatePlantObjectResp_1008056 data)
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
