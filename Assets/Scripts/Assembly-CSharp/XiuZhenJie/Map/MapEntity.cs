namespace XiuZhenJie
{
	public class MapEntity
	{
		public string Id;

		public string MapName;

		public string ParentId;

		public string ParentName;

		public int MapType;

		public int MinLevel;

		public int MaxLevel;

		public double MinX;

		public double MaxX;

		public double MinY;

		public double MaxY;

		public int Sort;

		public int Depth;

		public long MapPerception;

		public int FiveElement;

		public bool IsCleanMap;

		public MapEntity()
		{
		}

		public MapEntity(MapInfoResp_1007009 data)
		{
			Id = data.Id;
			MapName = data.Name;
			ParentId = data.ParentId;
			ParentName = data.ParentName;
			Sort = data.Sort;
			MapType = data.Type;
			FiveElement = data.FiveElements;
			Depth = data.Level;
			MinLevel = data.MinLevel;
			MaxLevel = data.MaxLevel;
		}
	}
}
