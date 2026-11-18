namespace XiuZhenJie
{
	public class AnimalMonster_Top_Entity : AnimalMonster_HighOrder_Entity
	{
		public AnimalMonster_Top_Entity()
		{
			Entity_Type = EntityType.Monster;
		}

		public override void SetBattleProps(BattleLifeModel _battleLifeModel)
		{
			base.SetBattleProps(_battleLifeModel);
		}
	}
}
