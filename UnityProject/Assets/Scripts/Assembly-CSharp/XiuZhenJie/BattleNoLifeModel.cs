namespace XiuZhenJie
{
	public class BattleNoLifeModel
	{
		public int ObjectIndex;

		public string GoodsId;

		public float PosX;

		public float PosY;

		public int FiveElement;

		public int ObjectType;

		public string Name;

		public int Level;

		public int Rank;

		public BattleNoLifeModel()
		{
		}

		public BattleNoLifeModel(object data)
		{
		}

		public BattleNoLifeModel(BattleRoleEnterResp_8001001.Types.DropGoodsInfoResp_8001013 data)
		{
			ObjectIndex = data.ObjectIndex;
			GoodsId = data.GoodsId;
			PosX = (float)data.PosX;
			PosY = (float)data.PosY;
			FiveElement = data.FiveElement;
			ObjectType = data.ObjectType;
			Name = data.Name;
			Level = data.Level;
			Rank = data.Rank;
		}
	}
}
