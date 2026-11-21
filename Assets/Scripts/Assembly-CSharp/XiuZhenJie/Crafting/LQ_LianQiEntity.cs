namespace XiuZhenJie
{
	public class LQ_LianQiEntity
	{
		public string Id;

		public string Name;

		public int FiveElement;

		public int Rank;

		public int Level;

		public int ExchangeContribution;

		public int Type;

		public string Created;

		public LQ_LianQiEntity(EnterMethodEquipmentExchangeResp_1008067.Types.ExchangeMethodEquipmentObjectResp_1008068 data)
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
