namespace XiuZhenJie
{
	public class DJSD_GongFaEntity
	{
		public string Id;

		public string Name;

		public int FiveElement;

		public int Rank;

		public int Level;

		public int ExchangeContribution;

		public string Created;

		public int Progress;

		public int HasChange;

		public int Convertible;

		public DJSD_GongFaEntity()
		{
		}

		public DJSD_GongFaEntity(EnterMethodAchievementExchangeResp_1008030.Types.ExchangeMethodAchievementObjectResp_1008031 data)
		{
			Id = data.Id;
			Name = data.Name;
			FiveElement = data.FiveElement;
			Rank = data.Rank;
			Level = data.Level;
			ExchangeContribution = data.ExchangeContribution;
			Created = data.Created;
			Progress = data.Progress;
			HasChange = data.HasChange;
			Convertible = data.DidNotChange;
		}
	}
}
