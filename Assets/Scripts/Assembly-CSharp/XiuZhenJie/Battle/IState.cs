namespace XiuZhenJie
{
	public interface IState
	{
		void Enter(EntityBase entity, params object[] args);

		void Process(EntityBase entity, params object[] args);

		void Exit(EntityBase entity, params object[] args);
	}
}
