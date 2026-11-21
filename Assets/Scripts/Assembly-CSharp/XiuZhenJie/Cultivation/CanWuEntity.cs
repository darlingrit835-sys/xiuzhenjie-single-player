namespace XiuZhenJie
{
	public class CanWuEntity
	{
		public string Id;

		public int FiveElement;

		public string Name;

		public int Type;

		public int Level;

		public bool IsCanWu;

		public int Rank;

		public int GetStage()
		{
			return (Level - 1) / 10 + 1;
		}
	}
}
