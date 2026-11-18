namespace XiuZhenJie
{
	public class WFG_FaShuEntity
	{
		public string Id;

		public string Name;

		public int FiveElement;

		public int Rank;

		public int Level;

		public int ExchangeContribution;

		public string Created;

		public WFG_FaShuEntity()
		{
		}

		public WFG_FaShuEntity(EnterMethodSkillExchangeResp_1008022.Types.ExchangeMethodSkillObjectResp_1008023 data)
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
