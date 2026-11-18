using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace XiuZhenJie
{
	public class AnimalMonster_Normal_Entity : MonsterEntity
	{
		protected Queue<AnimalMonsterSkillType> ShiFangAiQue;

		protected Dictionary<AnimalMonsterSkillType, BaseSkillModel> SkillDic;

		protected Dictionary<AnimalMonsterSkillType, UnityAction> SkillActionMethodDic;

		protected float ShiFaJianGeTime;

		protected bool m_IsChongZhuang;

		public AnimalMonster_Normal_Entity()
		{
			Entity_Type = EntityType.Monster;
			SkillDic = new Dictionary<AnimalMonsterSkillType, BaseSkillModel>();
			SkillActionMethodDic = new Dictionary<AnimalMonsterSkillType, UnityAction>();
			ShiFangAiQue = new Queue<AnimalMonsterSkillType>();
		}

		public override void SetBattleProps(BattleLifeModel _battleLifeModel)
		{
			base.SetBattleProps(_battleLifeModel);
			SkillInit();
		}

		protected virtual void SkillInit()
		{
			foreach (BaseSkillModel lifeSkill in m_BattleModel.LifeSkills)
			{
				AnimalMonsterSkillType animalMonsterSkillType = (AnimalMonsterSkillType)(lifeSkill.skillType % 1000);
				if (lifeSkill != null && !SkillDic.ContainsKey(animalMonsterSkillType))
				{
					SkillDic.Add(animalMonsterSkillType, lifeSkill);
					ShiFangAiQue.Enqueue(animalMonsterSkillType);
					switch (animalMonsterSkillType)
					{
					case AnimalMonsterSkillType.PuSha:
						SkillActionMethodDic.Add(animalMonsterSkillType, PuShaSkill);
						break;
					case AnimalMonsterSkillType.SiYao:
						SkillActionMethodDic.Add(animalMonsterSkillType, SiYaoSkill);
						break;
					case AnimalMonsterSkillType.ChongZhuang:
						SkillActionMethodDic.Add(animalMonsterSkillType, ChongZhuang);
						break;
					case AnimalMonsterSkillType.DanTiFaShu:
						SkillActionMethodDic.Add(animalMonsterSkillType, DanTiFaShu);
						break;
					case AnimalMonsterSkillType.HouPi:
						SkillActionMethodDic.Add(animalMonsterSkillType, HouPi);
						break;
					case AnimalMonsterSkillType.TunShi:
						SkillActionMethodDic.Add(animalMonsterSkillType, TunShi);
						break;
					case AnimalMonsterSkillType.ShuFu:
						SkillActionMethodDic.Add(animalMonsterSkillType, JianShuAttack);
						break;
					case AnimalMonsterSkillType.ZhenShe:
						SkillActionMethodDic.Add(animalMonsterSkillType, ZhenSheAttack);
						break;
					}
				}
			}
		}

		public override void TickAILogic()
		{
			base.TickAILogic();
			if (m_Target != null && ShiFangAiQue.Count > 0 && CurrentAniState != AniState.DingShen && CurrentAniState != AniState.Die && CurrentAniState != AniState.Attack)
			{
				ShiFaJianGeTime -= Time.deltaTime;
				if (ShiFaJianGeTime < 0f)
				{
					ShiFaJianGeTime = 3.1f;
					AnimalMonsterSkillType type = ShiFangAiQue.Dequeue();
					float time = 3f;
					BaseSkillModel value = null;
					if (SkillDic.TryGetValue(type, out value))
					{
						time = (float)(double)value.castTime / 1000f;
						if ((double)base.BattleModel.CurrentSuperAir < (double)(long)value.usedSuperAir)
						{
							ShiFangAiQue.Enqueue(type);
							return;
						}
					}
					if (!LockAreaObjDic.ContainsKey(m_Target.BaseController.Id) || m_Target.BaseController.IsDie)
					{
						ShiFangAiQue.Enqueue(type);
						return;
					}
					ShiFa(time, () =>
					{
						if (SkillDic.ContainsKey(type) && SkillActionMethodDic.ContainsKey(type))
						{
							MonsterShiFangSkill(type);
						}
					});
				}
			}
			if (m_IsChongZhuang && m_Target != null && CheckInKunMatrix())
			{
				Attack();
				if (Vector2.Distance(m_Target.GetPos(), GetPos()) <= 4f)
				{
					m_IsChongZhuang = false;
					ChongZhuangEffect();
					Idle();
				}
			}
		}

		public override double UnderAttack(int skillIndex, EntityBase _entityBase, double _damage, int _fiveElement)
		{
			if (_damage > 0.0 && _entityBase.Entity_Type == EntityType.Player)
			{
				UnderAttackSendMsg(skillIndex, _damage, _entityBase.BattleModel.ObjectIndex);
			}
			return base.UnderAttack(skillIndex, _entityBase, _damage, _fiveElement);
		}

		private bool CheckInKunMatrix()
		{
			BaseSkillModel baseSkillModel = CheckIfInMatrix(BattleZhenFaType.Kun);
			if (baseSkillModel != null)
			{
				float radiusValue = m_ZhenFaItemList[baseSkillModel.id].RadiusValue;
				if (Vector2.Distance(m_ZhenFaItemList[baseSkillModel.id].CenterPoint, m_Target.GetPos()) > radiusValue - 1.5f)
				{
					return false;
				}
			}
			return true;
		}

		protected virtual void MonsterShiFangSkill(AnimalMonsterSkillType _type)
		{
			UnityAction unityAction = SkillActionMethodDic[_type];
			BaseSkillModel baseSkillModel = SkillDic[_type];
			unityAction?.Invoke();
			_ = (long)SkillDic[_type].usedSuperAir;
			_ = (double)SkillDic[_type].castTime;
			XiaoHaoSuperAir((long)baseSkillModel.usedSuperAir);
			Timer.Register((float)(double)baseSkillModel.coolDown, () =>
			{
				ShiFangAiQue.Enqueue(_type);
			});
		}

		protected virtual void PuShaSkill()
		{
			if (m_Target == null)
			{
				return;
			}
			m_Target.GetPos();
			GetPos();
			if (CheckInKunMatrix())
			{
				BaseSkillModel value = null;
				if (SkillDic.TryGetValue(AnimalMonsterSkillType.PuSha, out value))
				{
					double damage = value.baseDamage;
					m_Target.UnderAttack(value.SkillIndex, this, damage, base.BattleModel.AttackFiveElement);
				}
			}
		}

		protected virtual void SiYaoSkill()
		{
			if (m_Target == null)
			{
				return;
			}
			Vector3 pos = m_Target.GetPos();
			if (Vector2.Distance(b: GetPos(), a: pos) <= 8f)
			{
				BaseSkillModel value = null;
				if (SkillDic.TryGetValue(AnimalMonsterSkillType.SiYao, out value))
				{
					float num = (float)(double)value.baseDamage;
					m_Target.DotUnderAttack(value.SkillIndex, this, num, SkillDic[AnimalMonsterSkillType.SiYao]);
				}
			}
		}

		protected virtual void DanTiFaShu()
		{
			if (m_Target == null)
			{
				return;
			}
			BaseSkillModel value = null;
			Vector3 pos = m_Target.GetPos();
			if (Vector2.Distance(b: GetPos(), a: pos) <= 10f && SkillDic.TryGetValue(AnimalMonsterSkillType.DanTiFaShu, out value))
			{
				GameObject gameObject = null;
				WuXing attackFiveElement = (WuXing)base.BattleModel.AttackFiveElement;
				GameObject gameObject2 = Object.Instantiate(attackFiveElement switch
				{
					WuXing.All => Singleton<ResourceManager>.Instance.LoadResource<GameObject>("Assets/GameData/Prefabs/Items/Battle/DanTi/WuXing_Danti_Ani_Prefab.prefab"), 
					WuXing.Jin => Singleton<ResourceManager>.Instance.LoadResource<GameObject>("Assets/GameData/Prefabs/Items/Battle/DanTi/Jin_Danti_Ani_Prefab.prefab"), 
					WuXing.Mu => Singleton<ResourceManager>.Instance.LoadResource<GameObject>("Assets/GameData/Prefabs/Items/Battle/DanTi/Mu_Danti_Ani_Prefab.prefab"), 
					WuXing.Shui => Singleton<ResourceManager>.Instance.LoadResource<GameObject>("Assets/GameData/Prefabs/Items/Battle/DanTi/Shui_Danti_Ani_Prefab.prefab"), 
					WuXing.Huo => Singleton<ResourceManager>.Instance.LoadResource<GameObject>("Assets/GameData/Prefabs/Items/Battle/DanTi/Huo_Danti_Ani_Prefab.prefab"), 
					WuXing.Tu => Singleton<ResourceManager>.Instance.LoadResource<GameObject>("Assets/GameData/Prefabs/Items/Battle/DanTi/Tu_Danti_Ani_Prefab.prefab"), 
					_ => Singleton<ResourceManager>.Instance.LoadResource<GameObject>("Assets/GameData/Prefabs/Items/Battle/DanTi/WuXing_Danti_Ani_Prefab.prefab"), 
				});
				gameObject2.transform.position = new Vector3(BaseController.transform.position.x, BaseController.transform.position.y, -3f);
				DanTiSkill_Item component = gameObject2.GetComponent<DanTiSkill_Item>();
				float num = (float)(double)value.baseDamage;
				if (m_Target != null)
				{
					component.Init(value.SkillIndex, this, num, attackFiveElement, m_Target);
				}
			}
		}

		protected virtual void ChongZhuang()
		{
			m_IsChongZhuang = true;
			BattleLifeModel battleModel = base.BattleModel;
			battleModel.CurrentMoveSpeed = (double)battleModel.CurrentMoveSpeed + 30.0;
		}

		protected virtual void ChongZhuangEffect()
		{
			BaseSkillModel value = null;
			if (SkillDic.TryGetValue(AnimalMonsterSkillType.ChongZhuang, out value))
			{
				m_Target.BeiDingShen(1.5f, base.BattleModel.Level, base.BattleModel.AttackFiveElement, 1, value.id);
				BattleLifeModel battleModel = base.BattleModel;
				battleModel.CurrentMoveSpeed = (double)battleModel.CurrentMoveSpeed - 30.0;
			}
		}

		protected virtual void HouPi()
		{
			BaseSkillModel value = null;
			if (SkillDic.TryGetValue(AnimalMonsterSkillType.HouPi, out value))
			{
				AddWuXingDefence(-1f, 0.2f, base.BattleModel.DefenseFiveElement, value.icon, value.id);
			}
		}

		protected virtual void TunShi()
		{
			BaseSkillModel value = null;
			if (SkillDic.TryGetValue(AnimalMonsterSkillType.TunShi, out value))
			{
				AddDiKangDamage((float)(double)value.continued, value.id, value.icon);
			}
		}

		protected virtual void JianShuAttack()
		{
			BaseSkillModel value = null;
			if (SkillDic.TryGetValue(AnimalMonsterSkillType.ShuFu, out value))
			{
				m_Target.JianShaoMoveSpeed((float)(double)value.continued, 0.5f, value.level, base.BattleModel.AttackFiveElement, value.icon, value.id);
			}
		}

		protected virtual void ZhenSheAttack()
		{
			BaseSkillModel value = null;
			if (SkillDic.TryGetValue(AnimalMonsterSkillType.ZhenShe, out value))
			{
				m_Target.ShiFaYanChang((float)(double)value.continued, 0.5f, value.level, base.BattleModel.AttackFiveElement, value.icon, value.id);
			}
		}

		protected virtual float BaseDamage(float time, float conversion)
		{
			int level = m_BattleModel.Level;
			float num = time * (float)(double)base.BattleModel.MaxSuperAir * 0.025f;
			return conversion * num * 15f / Mathf.Pow(level, 1.02f + (float)level / (100f + 0.25f * (float)level)) / 5f;
		}
	}
}
