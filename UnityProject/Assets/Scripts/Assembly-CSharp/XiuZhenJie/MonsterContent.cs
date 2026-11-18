using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace XiuZhenJie
{
	public class MonsterContent : MonoBehaviour
	{
		public List<ShouChao_MonsterItem> MonsterList;

		public List<UnderAttackEffectItem> EffectList;

		public void AddMonster(int monsterNum)
		{
			int num = monsterNum;
			if (num <= 0 || MonsterList == null || MonsterList.Count <= 0)
			{
				return;
			}
			foreach (ShouChao_MonsterItem monster in MonsterList)
			{
				if (monster.MonsterStatus == ShouChaoMonsterStatus.WaitingMove || monster.MonsterStatus == ShouChaoMonsterStatus.Dead)
				{
					monster.Init();
					monster.StartWalking();
					monster.MonsterAttackAction = (UnityAction)Delegate.Remove(monster.MonsterAttackAction, new UnityAction(MonsterAttack));
					monster.MonsterAttackAction = (UnityAction)Delegate.Combine(monster.MonsterAttackAction, new UnityAction(MonsterAttack));
					num--;
					if (num <= 0)
					{
						break;
					}
				}
			}
		}

		public void RemoveMonster(int monsterNum)
		{
			int num = monsterNum;
			if (num <= 0)
			{
				return;
			}
			foreach (ShouChao_MonsterItem monster in MonsterList)
			{
				monster.Dead();
				num--;
				if (num <= 0)
				{
					break;
				}
			}
		}

		public void FinishShouChao()
		{
			foreach (ShouChao_MonsterItem monster in MonsterList)
			{
				if (monster.MonsterStatus == ShouChaoMonsterStatus.WaitingMove)
				{
					monster.Dead();
				}
			}
		}

		private void MonsterAttack()
		{
			UnderAttackEffectItem underAttackEffectItem = null;
			foreach (UnderAttackEffectItem effect in EffectList)
			{
				if (effect.PlayOnce())
				{
					underAttackEffectItem = effect;
					break;
				}
			}
			if (underAttackEffectItem != null)
			{
				EffectList.Remove(underAttackEffectItem);
				EffectList.Add(underAttackEffectItem);
			}
		}
	}
}
