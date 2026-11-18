namespace XiuZhenJie
{
	public class JingMaiShouYiModel : BaseModel
	{
		public double a;

		public double b;

		public double c;

		public double d;

		public double e;

		public double f;

		public double g;

		public double h;

		public JingMaiShouYiModel()
		{
		}

		public JingMaiShouYiModel(AcupointDetailResp_1005020 data)
		{
			a = data.AirValueUp;
			b = data.AirSpeedUp;
			c = data.FiveElementAttackUp;
			d = data.FiveElementDefenseUp;
			e = data.PerceptionValueUp;
			f = data.PhysiqueValueUp;
			g = data.PureBloodValueUp;
			h = data.PureBloodRecoveryUp;
		}
	}
}
