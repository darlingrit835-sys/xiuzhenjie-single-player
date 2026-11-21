namespace XiuZhenJie
{
	public class AnimalMonster_HighOrder_Entity : AnimalMonster_Normal_Entity
	{
		public AnimalMonster_HighOrder_Entity()
		{
			Entity_Type = EntityType.Monster;
		}

		public override void SetBattleProps(BattleLifeModel _battleLifeModel)
		{
			base.SetBattleProps(_battleLifeModel);
		}
	}
}
