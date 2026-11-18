namespace XiuZhenJie
{
	public class DefenceWuQi_Item
	{
		public string Weapon_Id;

		public BaseSkillModel DataSource;

		public double GetDefenceDamage(double damage, int fiveElement)
		{
			return damage * (double)DataSource.usedUnit;
		}
	}
}
