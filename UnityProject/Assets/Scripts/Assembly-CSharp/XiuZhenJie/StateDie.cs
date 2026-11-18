namespace XiuZhenJie
{
	public class StateDie : IState
	{
		public void Enter(EntityBase entity, params object[] args)
		{
			entity.CurrentAniState = AniState.Die;
		}

		public void Exit(EntityBase entity, params object[] args)
		{
		}

		public void Process(EntityBase entity, params object[] args)
		{
		}
	}
}
