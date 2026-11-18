namespace XiuZhenJie
{
	public class AnimalMonster_Boss_Entity : AnimalMonster_Top_Entity
	{
		public AnimalMonster_Boss_Entity()
		{
			Entity_Type = EntityType.Monster;
		}

		public override void SetBattleProps(BattleLifeModel _battleLifeModel)
		{
			base.SetBattleProps(_battleLifeModel);
		}
	}
}
