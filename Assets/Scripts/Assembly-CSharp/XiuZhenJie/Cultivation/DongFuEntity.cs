namespace XiuZhenJie
{
	public class DongFuEntity
	{
		public int LocationType;

		public double PhysiqueCircle;

		public double PerceptionCircle;

		public double SuperAirCircle;

		public string PhysiqueName;

		public string PerceptionName;

		public string SuperAirName;

		public bool IsBrokenAcupoint;

		public int BrokenRate;

		public bool IsBrokenBody;

		public int GatherMatrixRank;

		public int ControllerMatrixRank;

		public double GatherMatrixLessTime;

		public double ControllerMartixLessTime;

		public int RefiningType;

		public long RefiningValue;

		public bool isCanBreak;

		public void SetValue(EnterExerciseRoomResp_1005008 enterExerciseRoomResp_1005008)
		{
			LocationType = enterExerciseRoomResp_1005008.LocationType;
			PhysiqueCircle = enterExerciseRoomResp_1005008.PhysiqueCircle;
			PerceptionCircle = enterExerciseRoomResp_1005008.PerceptionCircle;
			SuperAirCircle = enterExerciseRoomResp_1005008.SuperairCircle;
			PhysiqueName = enterExerciseRoomResp_1005008.PhysiqueName;
			PerceptionName = enterExerciseRoomResp_1005008.PerceptionName;
			SuperAirName = enterExerciseRoomResp_1005008.SuperairName;
			IsBrokenAcupoint = enterExerciseRoomResp_1005008.IsBrokenAcupoint;
			BrokenRate = enterExerciseRoomResp_1005008.BrokenRate;
			IsBrokenBody = enterExerciseRoomResp_1005008.IsBrokenBody;
			GatherMatrixRank = enterExerciseRoomResp_1005008.GatherMatrixRank;
			ControllerMatrixRank = enterExerciseRoomResp_1005008.ControllerMatrixRank;
			GatherMatrixLessTime = enterExerciseRoomResp_1005008.GatherMatrixLessTime;
			ControllerMartixLessTime = enterExerciseRoomResp_1005008.ControllerMartixLessTime;
			RefiningType = enterExerciseRoomResp_1005008.RefiningType;
			RefiningValue = enterExerciseRoomResp_1005008.RefiningValue;
			isCanBreak = enterExerciseRoomResp_1005008.IsBreak;
		}

		public string GetGatherMatrixName()
		{
			return GatherMatrixRank switch
			{
				4 => "黄阶·聚灵阵", 
				8 => "玄阶·聚灵阵", 
				12 => "地阶·聚灵阵", 
				16 => "天阶·聚灵阵", 
				_ => "无·聚灵阵", 
			};
		}

		public string GetControllerMatrixName()
		{
			return ControllerMatrixRank switch
			{
				4 => "黄阶·控灵阵", 
				8 => "玄阶·控灵阵", 
				12 => "地阶·控灵阵", 
				16 => "天阶·控灵阵", 
				_ => "无·控灵阵", 
			};
		}
	}
}
