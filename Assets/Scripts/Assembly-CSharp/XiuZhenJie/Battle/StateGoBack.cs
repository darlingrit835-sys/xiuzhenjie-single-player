namespace XiuZhenJie
{
	public class StateGoBack : IState
	{
		public void Enter(EntityBase entity, params object[] args)
		{
			entity.CurrentAniState = AniState.GoBack;
		}

		public void Exit(EntityBase entity, params object[] args)
		{
		}

		public void Process(EntityBase entity, params object[] args)
		{
		}
	}
}
