namespace XiuZhenJie
{
	public class StateMove : IState
	{
		public void Enter(EntityBase entity, params object[] args)
		{
			entity.CurrentAniState = AniState.Move;
		}

		public void Exit(EntityBase entity, params object[] args)
		{
		}

		public void Process(EntityBase entity, params object[] args)
		{
		}
	}
}
