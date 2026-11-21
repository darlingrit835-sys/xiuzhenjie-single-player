namespace XiuZhenJie
{
	public class GongFaEntity
	{
		public string Id;

		public string Name;

		public double Circle;

		public int UpCount;

		public int Count;

		public int AchievementLevel;

		public int MaxAchievementLevel;

		public int Level;

		public int Rank;

		public int FiveElement;

		public GongFaEntity()
		{
		}

		public GongFaEntity(ChangeAchievementPanelResp_1005009.Types.AchievementObjectResp_1005010 data)
		{
			if (data != null)
			{
				Id = data.Id;
				Name = data.Name;
				Circle = data.Circle;
				AchievementLevel = data.AchievementLevel;
				Level = data.Level;
				Rank = data.Rank;
				FiveElement = data.FiveElement;
			}
		}
	}
}
