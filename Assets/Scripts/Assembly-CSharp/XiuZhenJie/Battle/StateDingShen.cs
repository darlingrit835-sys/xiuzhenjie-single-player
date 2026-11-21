namespace XiuZhenJie
{
	public class StateDingShen : IState
	{
		public void Enter(EntityBase entity, params object[] args)
		{
			entity.CurrentAniState = AniState.DingShen;
			entity.ReComputeMoveSpeed();
		}

		public void Exit(EntityBase entity, params object[] args)
		{
			entity.ReComputeMoveSpeed();
		}

		public void Process(EntityBase entity, params object[] args)
		{
			entity.ReComputeMoveSpeed();
		}
	}
}
