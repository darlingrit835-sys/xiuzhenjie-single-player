using System.Collections.Generic;
using UnityEngine;

namespace XiuZhenJie
{
	public class StateManager : MonoBehaviour
	{
		private Dictionary<AniState, IState> fsm = new Dictionary<AniState, IState>();

		public void Init()
		{
			fsm.Add(AniState.Idle, new StateIdle());
			fsm.Add(AniState.Move, new StateMove());
			fsm.Add(AniState.Attack, new StateAttack());
			fsm.Add(AniState.DingShen, new StateDingShen());
			fsm.Add(AniState.GoBack, new StateGoBack());
			fsm.Add(AniState.Die, new StateDie());
		}

		public void ChangeStatus(EntityBase entity, AniState targetState, params object[] args)
		{
			if (entity.CurrentAniState != targetState && fsm.ContainsKey(targetState))
			{
				if (entity.CurrentAniState != AniState.None)
				{
					fsm[entity.CurrentAniState].Exit(entity, args);
				}
				fsm[targetState].Enter(entity, args);
				fsm[targetState].Process(entity, args);
			}
		}
	}
}
