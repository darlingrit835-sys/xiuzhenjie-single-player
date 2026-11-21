using System.Collections.Generic;
using UnityEngine;

namespace XiuZhenJie
{
	public class PeopleMonster_Normal_Entity : MonsterEntity
	{
		protected GameObject m_GongJiFaQi_InsObj;

		protected Dictionary<string, WuQi_Item> FlyWuQiDic = new Dictionary<string, WuQi_Item>();

		protected float m_GongFaPower;

		protected List<BaseSkillModel> m_SkillList;

		protected List<BaseSkillModel> m_WeaponList;

		protected float ShiFaJianGeTime;

		public PeopleMonster_Normal_Entity()
		{
			Entity_Type = EntityType.Monster;
		}

		public void SetGongFaPower(float power)
		{
			m_GongFaPower = power;
		}

		public void SetSkill(List<BaseSkillModel> skillVoList)
		{
			m_SkillList = skillVoList;
		}

		public void SetWepon(List<BaseSkillModel> weaponList)
		{
			m_WeaponList = weaponList;
			Timer.Register(Random.Range(0.1f, 20f), () =>
			{
				if (!((double)base.BattleModel.CurrentBlood <= 0.0))
				{
					MonsterShiFangWeapon();
				}
			});
		}

		public override double UnderAttack(int skillIndex, EntityBase _entityBase, double _damage, int _fiveElement)
		{
			if (_damage > 0.0 && _entityBase.Entity_Type == EntityType.Player)
			{
				UnderAttackSendMsg(skillIndex, _damage, _entityBase.BattleModel.ObjectIndex);
			}
			return base.UnderAttack(skillIndex, _entityBase, _damage, _fiveElement);
		}

		public override void TickAILogic()
		{
			base.TickAILogic();
			if (m_Target == null || m_SkillList.Count <= 0 || CurrentAniState == AniState.DingShen || CurrentAniState == AniState.Die || CurrentAniState == AniState.Attack)
			{
				return;
			}
			ShiFaJianGeTime -= Time.deltaTime;
			if (!(ShiFaJianGeTime < 0f))
			{
				return;
			}
			ShiFaJianGeTime = 3.1f;
			if (m_SkillList.Count <= 0)
			{
				return;
			}
			BaseSkillModel monsterJiNengModel = m_SkillList[0];
			float num = (float)(double)monsterJiNengModel.castTime / 1000f;
			float zhenYuanXiaoHao = num * m_GongFaPower;
			if ((double)base.BattleModel.CurrentSuperAir < (double)zhenYuanXiaoHao)
			{
				return;
			}
			ShiFa(num, () =>
			{
				if (m_Target != null && LockAreaObjDic.ContainsKey(m_Target.BaseController.Id) && !m_Target.BaseController.IsDie)
				{
					MonsterShiFangSkill(monsterJiNengModel);
					XiaoHaoSuperAir(zhenYuanXiaoHao);
				}
			});
		}

		protected virtual void MonsterShiFangSkill(BaseSkillModel skillModel)
		{
			switch ((FaShuType)(skillModel.skillType % 1000))
			{
			case FaShuType.gongji_suoding:
				DanTiFaShuInit(skillModel);
				break;
			case FaShuType.gongji_chixu:
				DotFaShuInit(skillModel);
				break;
			case FaShuType.gongji_fanwei_chixu:
				AoeFaShuInit(skillModel);
				break;
			case FaShuType.fangyu_fashu_hudun:
				HuDunFaShuInit(skillModel);
				break;
			case FaShuType.zengyi_shanghai:
				Buff_AddDamageInit(skillModel);
				break;
			case FaShuType.zengyi_fangyu:
				Buff_AddDefenceInit(skillModel);
				break;
			case FaShuType.dunshu_wuxing:
				ApplyHaste(skillModel);
				break;
			case FaShuType.kongzhi_yisu:
				DeBuff_AddJianSu(skillModel);
				break;
			case FaShuType.kongzhi_xingdong:
				DeBuff_AddDingShen(skillModel);
				break;
			}
			m_SkillList.Remove(skillModel);
			Timer.Register((float)(double)skillModel.coolDown / 1000f, () =>
			{
				m_SkillList.Add(skillModel);
			});
		}

		protected virtual void MonsterShiFangWeapon()
		{
			foreach (BaseSkillModel item in m_WeaponList)
			{
				if (!((double)base.BattleModel.CurrentPerception < (double)(int)item.usedPerception))
				{
					ShiFa(3f, () =>
					{
						FaQiInit(item);
						BattleLifeModel battleModel = base.BattleModel;
						battleModel.CurrentPerception = (double)battleModel.CurrentPerception - (double)(int)item.usedPerception;
					});
				}
			}
		}

		public virtual void FaQiInit(BaseSkillModel _playerFaQiModel)
		{
			if (FlyWuQiDic.ContainsKey(_playerFaQiModel.id))
			{
				FlyWuQiDic[_playerFaQiModel.id].Cancle(_isDestory: true);
				FlyWuQiDic.Remove(_playerFaQiModel.id);
				return;
			}
			m_GongJiFaQi_InsObj = BattleMgr.m_BattleSceneResourceManager.FaQiInsObj;
			GameObject gameObject = Object.Instantiate(m_GongJiFaQi_InsObj);
			WuQi_Item component = gameObject.GetComponent<WuQi_Item>();
			gameObject.transform.position = new Vector3(BaseController.gameObject.transform.position.x + (float)((FlyWuQiDic.Count + 1) * 5), BaseController.gameObject.transform.position.y, BaseController.gameObject.transform.position.z);
			component.Init(_playerFaQiModel.SkillIndex, this, m_GongFaPower * 3f, _playerFaQiModel.id, BaseController, _playerFaQiModel.baseDamage, (WuXing)_playerFaQiModel.fiveElement, (float)((double)_playerFaQiModel.castTime / 1000.0), FlyWuQiDic.Count + 1, BattleMgr.m_BattleSceneResourceManager.FaQiDic);
			component.InitDamageAdd(1.0, 1.0, 1.0, 1.0, 1.0);
			if (!FlyWuQiDic.ContainsKey(_playerFaQiModel.id))
			{
				FlyWuQiDic.Add(_playerFaQiModel.id, component);
			}
			if (m_Target != null)
			{
				component.SetTarget(m_Target);
			}
		}

		public void DanTiFaShuInit(BaseSkillModel _playerSkillModel)
		{
			if (m_Target != null)
			{
				Vector3 pos = m_Target.GetPos();
				if (Vector2.Distance(b: GetPos(), a: pos) <= 10f)
				{
					DanTiLocked(_playerSkillModel);
				}
			}
		}

		public void DotFaShuInit(BaseSkillModel _playerSkillModel)
		{
			if (m_Target != null)
			{
				Vector3 pos = m_Target.GetPos();
				if (Vector2.Distance(b: GetPos(), a: pos) <= 10f)
				{
					m_Target.DotUnderAttack(_playerSkillModel.SkillIndex, this, _playerSkillModel.baseDamage, _playerSkillModel);
				}
			}
		}

		public void AoeFaShuInit(BaseSkillModel _playerSkillModel)
		{
			if (m_Target == null)
			{
				return;
			}
			Vector3 pos = m_Target.GetPos();
			if (Vector2.Distance(b: GetPos(), a: pos) > 10f)
			{
				return;
			}
			AoeSkill_Item aoeSkill_Item = AoeContinuous(_playerSkillModel);
			EntityBase target = m_Target;
			if (m_Target != null)
			{
				if (m_Target != target)
				{
					aoeSkill_Item.Cancle();
				}
			}
			else if (m_Target == null)
			{
				aoeSkill_Item.Cancle();
			}
		}

		public void HuDunFaShuInit(BaseSkillModel _playerSkillModel)
		{
			WuXing fiveElement = (WuXing)_playerSkillModel.fiveElement;
			GameObject value = null;
			if (BattleMgr.m_BattleSceneResourceManager.HuDunFaShuDic.TryGetValue(fiveElement, out value))
			{
				GameObject huDun = Object.Instantiate(value, BaseController.transform);
				float value2 = (float)FaShuDamageCompute(_playerSkillModel);
				BaseController.SetHuDun(huDun);
				AddHuDun(value2, _playerSkillModel.fiveElement, _playerSkillModel.icon, _playerSkillModel.id);
			}
		}

		public void Buff_AddDamageInit(BaseSkillModel _playerSkillModel)
		{
			float num = 0.05f + (float)(WorldSetting.GetLevelByRank(_playerSkillModel.rank) - 1) * 0.015f - (float)(Singleton<RoleManager>.Instance.GetRoleStage() - ((int)_playerSkillModel.level - 1) / 10 + 1) * 0.02f;
			num = ((num < 0f) ? 0f : num);
			AddWuXingDamage(300f, num, _playerSkillModel.fiveElement, _playerSkillModel.icon, _playerSkillModel.id);
		}

		public void Buff_AddDefenceInit(BaseSkillModel _playerSkillModel)
		{
			float num = 0.05f + (float)(WorldSetting.GetLevelByRank(_playerSkillModel.rank) - 1) * 0.015f - (float)(Singleton<RoleManager>.Instance.GetRoleStage() - ((int)_playerSkillModel.level - 1) / 10 + 1) * 0.02f;
			num = ((num < 0f) ? 0f : num);
			AddWuXingDefence(300f, num, _playerSkillModel.fiveElement, _playerSkillModel.icon, _playerSkillModel.id);
		}

		public void DeBuff_AddDingShen(BaseSkillModel _playerSkillModel)
		{
			if (m_Target != null)
			{
				float time = _playerSkillModel.MovementSeconds(1f);
				m_Target.BeiDingShen(time, _playerSkillModel.level, _playerSkillModel.fiveElement, _playerSkillModel.icon, _playerSkillModel.id);
			}
		}

		public void DeBuff_AddJianSu(BaseSkillModel _playerSkillModel)
		{
			if (m_Target != null)
			{
				m_Target.JianShaoMoveSpeed(_playerSkillModel.MovementSeconds(1f), _playerSkillModel.SlowFraction(),
					_playerSkillModel.level, _playerSkillModel.fiveElement, _playerSkillModel.icon, _playerSkillModel.id);
			}
		}

		protected void DanTiLocked(BaseSkillModel _playerSkillModel)
		{
			GameObject value = null;
			WuXing fiveElement = (WuXing)_playerSkillModel.fiveElement;
			if (BattleMgr.m_BattleSceneResourceManager.DanTiFaShuDic.TryGetValue(fiveElement, out value))
			{
				GameObject gameObject = Object.Instantiate(value);
				gameObject.transform.position = new Vector3(BaseController.transform.position.x, BaseController.transform.position.y, -3f);
				gameObject.GetComponent<DanTiSkill_Item>().Init(_playerSkillModel.SkillIndex, this, FaShuDamageCompute(_playerSkillModel), fiveElement, m_Target);
			}
		}

		protected AoeSkill_Item AoeContinuous(BaseSkillModel _playerSkillModel)
		{
			GameObject gameObject = Object.Instantiate(BattleMgr.m_BattleSceneResourceManager.AoeInsObj);
			gameObject.transform.position = new Vector3(m_Target.GetPos().x, m_Target.GetPos().y, -3f);
			AoeSkill_Item component = gameObject.GetComponent<AoeSkill_Item>();
			component.Init(_playerSkillModel.SkillIndex, this, FaShuDamageCompute(_playerSkillModel), _playerSkillModel.fiveElement, 15f, 1.5f, EntityType.Player, BattleMgr.m_BattleSceneResourceManager.AoeFaShuDic);
			return component;
		}

		protected double FaShuDamageCompute(BaseSkillModel data)
		{
			return data.baseDamage;
		}

		protected double FaShuAttackDamage(BaseSkillModel data, double zhenYuanXiaoHao, float multiple)
		{
			int level = m_BattleModel.Level;
			double num = (11 - level) / 2;
			num = ((num < 1.0) ? 1.0 : num);
			zhenYuanXiaoHao = ((zhenYuanXiaoHao > (double)(long)data.usedSuperAir) ? zhenYuanXiaoHao : ((double)(long)data.usedSuperAir));
			double num2 = zhenYuanXiaoHao * data.conversionRate * (double)multiple * 10.0 / (double)Mathf.Pow(level, 1.02f + (float)level / (100f + 0.25f * (float)level));
			return (num2 + num2 * (double)base.BattleModel.DamageAdd) / num;
		}

		protected override void ShenShiLockEnterTriggerCheck(Collider2D _collider2d)
		{
			base.ShenShiLockEnterTriggerCheck(_collider2d);
			Controller componentInParent = _collider2d.gameObject.GetComponentInParent<Controller>();
			if (!LockAreaObjDic.ContainsKey(componentInParent.Id))
			{
				LockAreaObjDic.Add(componentInParent.Id, componentInParent.EntityDataSource);
			}
			if (m_Target == null)
			{
				m_Target = componentInParent.EntityDataSource;
			}
			if (FlyWuQiDic == null)
			{
				return;
			}
			foreach (KeyValuePair<string, WuQi_Item> item in FlyWuQiDic)
			{
				if (item.Value.CurrentState == WuQiState.Waiting)
				{
					item.Value.SetTarget(m_Target);
				}
			}
		}

		protected override void ShenShiLockExitTriggerCheck(Collider2D _collider2d)
		{
			base.ShenShiLockExitTriggerCheck(_collider2d);
			if (FlyWuQiDic == null)
			{
				return;
			}
			foreach (KeyValuePair<string, WuQi_Item> item in FlyWuQiDic)
			{
				if (item.Value.CurrentState != WuQiState.Waiting)
				{
					item.Value.Cancle();
				}
			}
		}

		protected override void ShenShiFoundEnterTriggerCheck(Collider2D _collider2d)
		{
			base.ShenShiFoundEnterTriggerCheck(_collider2d);
		}

		protected override void ShenShiFoundExitTriggerCheck(Collider2D _collider2d)
		{
			base.ShenShiFoundExitTriggerCheck(_collider2d);
			if (FlyWuQiDic == null)
			{
				return;
			}
			foreach (KeyValuePair<string, WuQi_Item> item in FlyWuQiDic)
			{
				if (item.Value.CurrentState != WuQiState.Waiting)
				{
					item.Value.Cancle();
				}
			}
		}
	}
}
