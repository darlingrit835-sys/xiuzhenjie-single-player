using System.Collections.Generic;
using UnityEngine;

namespace XiuZhenJie
{
	public class MonsterEntity : EntityBase
	{
		protected float m_MaxChaseDistance = 30f;

		protected bool m_IsAlwaysAttack;

		protected Vector3 m_TargetVector;

		private float m_ReactionTime;

		public MonsterEntity()
		{
			Entity_Type = EntityType.Monster;
		}

		public override void SetBattleProps(BattleLifeModel _battleLifeModel)
		{
			base.SetBattleProps(_battleLifeModel);
			m_MaxChaseDistance = (float)_battleLifeModel.Type * 30f / 2f;
		}

		public override void TickAILogic()
		{
			base.TickAILogic();
			if (m_Target != null && CurrentAniState != AniState.GoBack && CurrentAniState != AniState.Die)
			{
				m_TargetVector = m_Target.GetPos() - GetPos();
				double num = Vector2.Distance(m_Target.GetPos(), GetPos());
				double num2 = Vector2.Distance(m_InitPos, GetPos());
				if (num > 4.0)
				{
					ChaseTarget(m_TargetVector.normalized);
				}
				if (num2 >= (double)m_MaxChaseDistance && !m_IsAlwaysAttack && !CheckSufferKunMatrix() && CurrentAniState != AniState.Attack)
				{
					GoBack();
				}
			}
			if (CurrentAniState != AniState.GoBack)
			{
				return;
			}
			base.BattleModel.CurrentMoveSpeed = 20.0;
			m_TargetVector = m_InitPos - GetPos();
			double num3 = Vector2.Distance(m_InitPos, GetPos());
			ChaseTarget(m_TargetVector.normalized);
			if (num3 < 1.0)
			{
				ReComputeMoveSpeed();
				Idle();
				if (FoundAreaObjDic.Count == 0)
				{
					m_Target = null;
				}
			}
		}

		public override void FixUpdate()
		{
			base.FixUpdate();
		}

		public override void ChaseTarget(Vector3 vector)
		{
			if (m_BattleModel == null)
			{
				return;
			}
			BaseController.SetDirection(vector);
			Transform trans = GetTrans();
			if (trans == null)
			{
				return;
			}
			Vector3 vector2 = trans.position + new Vector3(vector.x * (float)(double)m_BattleModel.CurrentMoveSpeed * 0.01f, vector.y * (float)(double)m_BattleModel.CurrentMoveSpeed * 0.01f);
			if (CheckSufferKunMatrix())
			{
				ZhenFa_Item zhenFa_Item = FindKunMatrix();
				if (zhenFa_Item == null)
				{
					trans.position = vector2;
					return;
				}
				float num = Vector2.Distance(vector2, zhenFa_Item.CenterPoint);
				float num2 = Vector2.Distance(trans.position, zhenFa_Item.CenterPoint);
				if (num > num2)
				{
					if (!(num >= zhenFa_Item.RadiusValue - BaseController.SelfCollider2D.radius))
					{
						trans.position = vector2;
					}
				}
				else
				{
					trans.position = vector2;
				}
			}
			else
			{
				trans.position = vector2;
			}
		}

		protected bool CheckSufferKillMatrix(BaseSkillModel item)
		{
			if (item.type == 4 && item.skillType == 1)
			{
				return true;
			}
			return false;
		}

		protected void SufferKillMatrixDamage(BaseSkillModel item)
		{
			double damage = item.baseDamage;
			switch ((WuXing)base.BattleModel.DefenseFiveElement)
			{
			case WuXing.Jin:
				damage = (double)item.baseDamage * (double)item.GoldDamageAdd;
				break;
			case WuXing.Mu:
				damage = (double)item.baseDamage * (double)item.WoodDamageAdd;
				break;
			case WuXing.Shui:
				damage = (double)item.baseDamage * (double)item.WaterDamageAdd;
				break;
			case WuXing.Huo:
				damage = (double)item.baseDamage * (double)item.FireDamageAdd;
				break;
			case WuXing.Tu:
				damage = (double)item.baseDamage * (double)item.EarthDamageAdd;
				break;
			}
			UnderAttack(item.SkillIndex, BattleMgr.SelfPlayerEntity, damage, item.fiveElement);
		}

		protected bool CheckSufferKunMatrix()
		{
			foreach (KeyValuePair<string, BaseSkillModel> zhenFa in m_ZhenFaList)
			{
				if (zhenFa.Value.type == 4 && zhenFa.Value.skillType == 2)
				{
					return true;
				}
			}
			return false;
		}

		protected ZhenFa_Item FindKunMatrix()
		{
			string text = "";
			foreach (KeyValuePair<string, BaseSkillModel> zhenFa in m_ZhenFaList)
			{
				if (zhenFa.Value.type == 4 && zhenFa.Value.skillType == 2)
				{
					text = zhenFa.Key;
					break;
				}
			}
			if (string.IsNullOrEmpty(text))
			{
				return null;
			}
			return m_ZhenFaItemList[text];
		}

		protected bool CheckHuanMatrix()
		{
			foreach (KeyValuePair<string, BaseSkillModel> zhenFa in m_ZhenFaList)
			{
				if (zhenFa.Value.type == 4 && zhenFa.Value.skillType == 3)
				{
					return true;
				}
			}
			return false;
		}

		public virtual void SetAlwaysAttack(bool isAlways = true)
		{
			m_IsAlwaysAttack = isAlways;
		}

		public virtual void SetTarget(EntityBase entityBase)
		{
			m_Target = entityBase;
		}

		public override double UnderAttack(int skillIndex, EntityBase _entityBase, double _damage, int _fiveElement)
		{
			if (CurrentAniState == AniState.GoBack && !CheckSufferKunMatrix())
			{
				if (BaseController == null)
				{
					return 0.0;
				}
				BaseController.ShowStateText("抵抗");
				if (m_Target == null)
				{
					m_Target = _entityBase;
				}
				return 0.0;
			}
			double result = base.UnderAttack(skillIndex, _entityBase, _damage, _fiveElement);
			if (m_Target == null)
			{
				m_Target = _entityBase;
			}
			return result;
		}

		protected void UnderAttackSendMsg(int skillIndex, double damage, int roleIndex)
		{
			MonsterGetDamageReq_8001006 data = new MonsterGetDamageReq_8001006
			{
				Damage = (int)damage,
				MonsterIndex = base.BattleModel.ObjectIndex,
				RoleIndex = roleIndex,
				SkillIndex = skillIndex
			};
			Singleton<NetManager>.Instance.SendMsg(MsgType.MONSTER_GET_DAMAGE, data, null, isShowWaitUI: false, ServerType.BattleServer);
		}

		public override void DotUnderAttack(int skillIndex, EntityBase _damageFromEntity, double _damage, BaseSkillModel _baseSkillModel)
		{
			base.DotUnderAttack(skillIndex, _damageFromEntity, _damage, _baseSkillModel);
		}

		public override void EnterZhenFa(BattleZhenFaType battleZhenFaType, BaseSkillModel baseSkillModel, ZhenFa_Item zhenFa_Item)
		{
			if (m_ZhenFaList.ContainsKey(baseSkillModel.id) || m_ZhenFaItemList.ContainsKey(baseSkillModel.id))
			{
				return;
			}
			base.EnterZhenFa(battleZhenFaType, baseSkillModel, zhenFa_Item);
			if (battleZhenFaType == BattleZhenFaType.Kun)
			{
				BattleLifeModel battleModel = m_BattleModel;
				battleModel.CurrentMoveSpeed = (double)battleModel.CurrentMoveSpeed - (double)m_BattleModel.InitMoveSpeed * 0.30000001192092896;
				if ((double)m_BattleModel.CurrentMoveSpeed < 0.0)
				{
					m_BattleModel.CurrentMoveSpeed = 0.0;
				}
			}
		}

		public override void ExitZhenFa(BattleZhenFaType battleZhenFaType, string skillId)
		{
			if (m_ZhenFaList.ContainsKey(skillId) && battleZhenFaType == BattleZhenFaType.Kun)
			{
				BattleLifeModel battleModel = m_BattleModel;
				battleModel.CurrentMoveSpeed = (double)battleModel.CurrentMoveSpeed + (double)m_BattleModel.InitMoveSpeed * 0.30000001192092896;
			}
			base.ExitZhenFa(battleZhenFaType, skillId);
		}

		protected override void DeadMethod()
		{
			base.DeadMethod();
			MonsterDeadReq_8002003 data = new MonsterDeadReq_8002003
			{
				DeadMonsterIndex = m_BattleModel.ObjectIndex,
				PosX = BaseController.transform.position.x,
				PosY = BaseController.transform.position.y
			};
			Singleton<NetManager>.Instance.SendMsg(MsgType.MONSTER_DEAD, data, null, isShowWaitUI: false, ServerType.BattleServer);
			BattleMgr.MonsterDead(BaseController.Id);
		}

		protected override void ShenShiFoundEnterTriggerCheck(Collider2D _collider2d)
		{
			base.ShenShiFoundEnterTriggerCheck(_collider2d);
			Controller componentInParent = _collider2d.gameObject.GetComponentInParent<Controller>();
			if (!FoundAreaObjDic.ContainsKey(componentInParent.Id))
			{
				FoundAreaObjDic.Add(componentInParent.Id, componentInParent.EntityDataSource);
			}
			if (m_Target == null)
			{
				m_Target = componentInParent.EntityDataSource;
				m_InitPos = GetPos();
			}
		}

		protected override void ShenShiFoundExitTriggerCheck(Collider2D _collider2d)
		{
			base.ShenShiFoundExitTriggerCheck(_collider2d);
			Controller componentInParent = _collider2d.gameObject.GetComponentInParent<Controller>();
			if (componentInParent != null && FoundAreaObjDic.ContainsKey(componentInParent.Id))
			{
				FoundAreaObjDic.Remove(componentInParent.Id);
			}
		}

		protected override void ShenShiLockEnterTriggerCheck(Collider2D _collider2d)
		{
			base.ShenShiLockEnterTriggerCheck(_collider2d);
			Controller componentInParent = _collider2d.gameObject.GetComponentInParent<Controller>();
			if (!LockAreaObjDic.ContainsKey(componentInParent.Id))
			{
				LockAreaObjDic.Add(componentInParent.Id, componentInParent.EntityDataSource);
			}
		}

		protected override void ShenShiLockExitTriggerCheck(Collider2D _collider2d)
		{
			base.ShenShiLockExitTriggerCheck(_collider2d);
			Controller componentInParent = _collider2d.gameObject.GetComponentInParent<Controller>();
			if (componentInParent != null && !LockAreaObjDic.ContainsKey(componentInParent.Id))
			{
				LockAreaObjDic.Remove(componentInParent.Id);
			}
		}
	}
}
