using System.Collections.Generic;
using Google.Protobuf;
using UnityEngine;

namespace XiuZhenJie
{
	public class PlayerEntity : EntityBase
	{
		private bool m_IsWuDi;

		private GameObject m_GongJiFaQi_InsObj;

		private GameObject m_ZhenFa_InsObj;

		private Dictionary<string, WuQi_Item> FlyWuQiDic = new Dictionary<string, WuQi_Item>();

		private Dictionary<string, DefenceWuQi_Item> m_DefenceWuQiDic = new Dictionary<string, DefenceWuQi_Item>();

		private Dictionary<string, Battle_SkillItem> m_WuQiUIDic = new Dictionary<string, Battle_SkillItem>();

		private List<RoleZhuangBeiModel> m_DefenceEquipmentList = new List<RoleZhuangBeiModel>();

		private BaseSkillModel m_UsingZhenFa;

		private float m_UsingZhenFaDuringTime = -10f;

		private float m_GongFaPower;

		public Dictionary<string, int> m_FuLuUseTimes = new Dictionary<string, int>();

		private int times;
		private readonly HashSet<string> pendingPickups = new HashSet<string>();
		private Vector3 lastReported;
		private float lastStepEffect;

		public BaseSkillModel UsingZhenFa
		{
			get
			{
				return m_UsingZhenFa;
			}
			private set
			{
				m_UsingZhenFa = value;
			}
		}

		private float lastAttackTime;

		public PlayerEntity()
		{
			Entity_Type = EntityType.Player;
			m_IsWuDi = false;
		}

		public override void SetBattleProps(BattleLifeModel _battleLifeModel)
		{
			base.SetBattleProps(_battleLifeModel);
		}

		public void SetGongFaPower(float power)
		{
			m_GongFaPower = power;
			double num = 0.0;
			m_GongFaPower += (float)num;
		}

		public override void FixUpdate()
		{
			base.FixUpdate();
			if (BattleMgr.FieldView != null && (m_Target == null || !CanTarget(m_Target))) ChangeTarget();
			times++;
			if (times >= 3)
			{
				SendRolePosInfoToNet(GetPos());
				times = 0;
			}
			if (m_UsingZhenFaDuringTime > 0f)
			{
				m_UsingZhenFaDuringTime -= Time.fixedDeltaTime;
			}
			else if (m_UsingZhenFaDuringTime <= 0f && m_UsingZhenFaDuringTime > -1f)
			{
				m_UsingZhenFaDuringTime = -10f;
				m_UsingZhenFa = null;
			}
		}

		public void RestorePosition(Vector3 confirmed)
		{
			GetTrans().position = confirmed;
			lastReported = confirmed;
			pendingPickups.Clear();
		}

		private void SendRolePosInfoToNet(Vector3 position)
		{
			if (!(BaseController == null) && !(BaseController.gameObject == null))
			{
				if (BattleMgr.Layout != null && (position - lastReported).sqrMagnitude < .000001f) return;
				lastReported = position;
				BattleRoleMoveReq_8001004 data = new BattleRoleMoveReq_8001004
				{
					PosX = position.x,
					PosY = position.y,
					Angle = BaseController.gameObject.transform.eulerAngles.z,
					RoleIndex = base.BattleModel.ObjectIndex
				};
				Singleton<NetManager>.Instance.SendMsg(MsgType.BATTLE_ROLE_MOVE, data, null, isShowWaitUI: false, ServerType.BattleServer);
			}
		}

		private double GongFaPowerAdd(GongFaEffectTypeEnum gongFaEffectTypeEnum, GongFaEffectValueEnum gongFaEffectValueEnum, int damageFiveElement, double baseDamage)
		{
			double result = 0.0;
			if (gongFaEffectTypeEnum == GongFaEffectTypeEnum.ATTACK_AIR_ADD_SPEED_EFFECT)
			{
				switch (damageFiveElement)
				{
				case 1:
					switch (gongFaEffectValueEnum)
					{
					case GongFaEffectValueEnum.JIN_AIR_SPEED_10:
						result = baseDamage * 0.10000000149011612;
						break;
					case GongFaEffectValueEnum.JIN_AIR_SPEED_20:
						result = baseDamage * 0.20000000298023224;
						break;
					case GongFaEffectValueEnum.JIN_AIR_SPEED_25:
						result = baseDamage * 0.25;
						break;
					}
					break;
				case 2:
					switch (gongFaEffectValueEnum)
					{
					case GongFaEffectValueEnum.MU_AIR_SPEED_10:
						result = baseDamage * 0.10000000149011612;
						break;
					case GongFaEffectValueEnum.MU_AIR_SPEED_20:
						result = baseDamage * 0.20000000298023224;
						break;
					case GongFaEffectValueEnum.MU_AIR_SPEED_25:
						result = baseDamage * 0.25;
						break;
					}
					break;
				case 3:
					switch (gongFaEffectValueEnum)
					{
					case GongFaEffectValueEnum.SHUI_AIR_SPEED_10:
						result = baseDamage * 0.10000000149011612;
						break;
					case GongFaEffectValueEnum.SHUI_AIR_SPEED_20:
						result = baseDamage * 0.20000000298023224;
						break;
					case GongFaEffectValueEnum.SHUI_AIR_SPEED_25:
						result = baseDamage * 0.25;
						break;
					}
					break;
				case 4:
					switch (gongFaEffectValueEnum)
					{
					case GongFaEffectValueEnum.HUO_AIR_SPEED_10:
						result = baseDamage * 0.10000000149011612;
						break;
					case GongFaEffectValueEnum.HUO_AIR_SPEED_20:
						result = baseDamage * 0.20000000298023224;
						break;
					case GongFaEffectValueEnum.HUO_AIR_SPEED_25:
						result = baseDamage * 0.25;
						break;
					}
					break;
				case 5:
					switch (gongFaEffectValueEnum)
					{
					case GongFaEffectValueEnum.TU_AIR_SPEED_10:
						result = baseDamage * 0.10000000149011612;
						break;
					case GongFaEffectValueEnum.TU_AIR_SPEED_20:
						result = baseDamage * 0.20000000298023224;
						break;
					case GongFaEffectValueEnum.TU_AIR_SPEED_25:
						result = baseDamage * 0.25;
						break;
					}
					break;
				}
			}
			return result;
		}

		public float GetGongFaPower()
		{
			return m_GongFaPower;
		}

		public override void TickAILogic()
		{
			base.TickAILogic();
			var life = BattleModel;
			if (BattleMgr == null || life == null || BaseController == null || BaseController.IsDie ||
				m_IsInBattleStatus || Time.time - lastAttackTime < 4f || life.RestBloodFractionPerSecond <= 0) return;
			double current = life.CurrentBlood, max = life.Max_Blood;
			if (current > 0 && current < max)
				life.CurrentBlood = System.Math.Min(max, current + max * life.RestBloodFractionPerSecond * Time.deltaTime);
		}

		public override void RecoverJianRen()
		{
			if (base.BattleModel != null && !BaseController.IsDie && !((double)base.BattleModel.CurrentJianRen >= (double)base.BattleModel.Max_JianRen))
			{
				double num = (m_IsInBattleStatus ? ((double)base.BattleModel.RecoverJianRenEverySecondBattle * (double)Time.deltaTime) : ((double)base.BattleModel.RecoverJianRenEverySecondNormal * (double)Time.deltaTime));
				BattleLifeModel battleModel = base.BattleModel;
				battleModel.CurrentJianRen = (double)battleModel.CurrentJianRen + num;
				if ((double)base.BattleModel.CurrentJianRen >= (double)base.BattleModel.Max_JianRen)
				{
					base.BattleModel.CurrentJianRen = base.BattleModel.Max_JianRen;
				}
			}
		}

		public override void RecoverSuperAir()
		{
			if (base.BattleModel != null && !BaseController.IsDie && !((double)base.BattleModel.CurrentSuperAir >= (double)base.BattleModel.MaxSuperAir))
			{
				float recoverSuperAirAdd = GetRecoverSuperAirAdd();
				float num = (float)(m_IsInBattleStatus ? ((double)base.BattleModel.RecoverSuperAirEverySecondBattle * (double)recoverSuperAirAdd * (double)Time.deltaTime) : ((double)base.BattleModel.RecoverSuperAirEverySecondNormal * (double)recoverSuperAirAdd * (double)Time.deltaTime));
				BattleLifeModel battleModel = base.BattleModel;
				battleModel.CurrentSuperAir = (double)battleModel.CurrentSuperAir + (double)num;
				if ((double)base.BattleModel.CurrentSuperAir >= (double)base.BattleModel.MaxSuperAir)
				{
					base.BattleModel.CurrentSuperAir = base.BattleModel.MaxSuperAir;
				}
			}
		}

		public void RecoverAllSuperAir()
		{
			base.BattleModel.CurrentSuperAir = base.BattleModel.MaxSuperAir;
		}

		private float GetRecoverSuperAirAdd()
		{
			float num = 1f;
			foreach (RoleZhuangBeiModel defenceEquipment in m_DefenceEquipmentList)
			{
				if (defenceEquipment.cc == 123)
				{
					num += 0.25f;
				}
				else if (defenceEquipment.cc == 124)
				{
					num += 0.4f;
				}
				if (defenceEquipment.dd == 123)
				{
					num += 0.25f;
				}
				else if (defenceEquipment.dd == 124)
				{
					num += 0.4f;
				}
			}
			return num;
		}

		protected bool CheckRecoverTiPoMatrix()
		{
			foreach (KeyValuePair<string, BaseSkillModel> zhenFa in m_ZhenFaList)
			{
				if (zhenFa.Value.type == 20 && zhenFa.Value.skillType == 5)
				{
					return true;
				}
			}
			return false;
		}

		protected bool CheckRecoverSuperAirMatrix()
		{
			foreach (KeyValuePair<string, BaseSkillModel> zhenFa in m_ZhenFaList)
			{
				if (zhenFa.Value.type == 20 && zhenFa.Value.skillType == 4)
				{
					return true;
				}
			}
			return false;
		}

		public bool IsCanUseFuLu(string id)
		{
			return false;
		}

		public void UseFuLu(string id)
		{
		}

		public bool FaQiIsActive(string id)
		{
			if (FlyWuQiDic.ContainsKey(id) || m_DefenceWuQiDic.ContainsKey(id))
			{
				return true;
			}
			return false;
		}

		public void CloseFaQq(string id)
		{
			if (FlyWuQiDic.ContainsKey(id))
			{
				FlyWuQiDic[id].Cancle(_isDestory: true);
				FlyWuQiDic.Remove(id);
			}
			else if (m_DefenceWuQiDic.ContainsKey(id))
			{
				m_DefenceWuQiDic.Remove(id);
				if (m_DefenceWuQiDic.Count <= 0)
				{
					BaseController.CancleShowDefenceEquipmentEffect();
				}
			}
		}

		public void FaQiInit(BaseSkillModel _BaseSkillModel, Battle_SkillItem UI_FaQiItem)
		{
			if (FlyWuQiDic.ContainsKey(_BaseSkillModel.id))
			{
				FlyWuQiDic[_BaseSkillModel.id].Cancle(_isDestory: true);
				FlyWuQiDic.Remove(_BaseSkillModel.id);
				return;
			}
			if (m_DefenceWuQiDic.ContainsKey(_BaseSkillModel.id))
			{
				m_DefenceWuQiDic.Remove(_BaseSkillModel.id);
				if (m_DefenceWuQiDic.Count <= 0)
				{
					BaseController.CancleShowDefenceEquipmentEffect();
				}
				return;
			}
			if (_BaseSkillModel.skillType == 15)
			{
				AttackFaQiInit(_BaseSkillModel);
			}
			else if (_BaseSkillModel.skillType == 26)
			{
				m_DefenceWuQiDic.Add(_BaseSkillModel.id, new DefenceWuQi_Item
				{
					DataSource = _BaseSkillModel,
					Weapon_Id = _BaseSkillModel.id
				});
				BaseController.ShowDefenceEquipmentEffect();
			}
			if (!m_WuQiUIDic.ContainsKey(_BaseSkillModel.id))
			{
				m_WuQiUIDic.Add(_BaseSkillModel.id, UI_FaQiItem);
			}
		}

		private void AttackFaQiInit(BaseSkillModel _BaseSkillModel)
		{
			m_GongJiFaQi_InsObj = BattleMgr.m_BattleSceneResourceManager.FaQiInsObj;
			GameObject gameObject = Object.Instantiate(m_GongJiFaQi_InsObj);
			WuQi_Item component = gameObject.GetComponent<WuQi_Item>();
			float num = 2 * (FlyWuQiDic.Count + 1) + 2;
			gameObject.transform.position = new Vector3(BaseController.gameObject.transform.position.x + num, BaseController.gameObject.transform.position.y, BaseController.gameObject.transform.position.z);
			component.Init(_BaseSkillModel.SkillIndex, this, (long)_BaseSkillModel.usedSuperAir, _BaseSkillModel.id, BaseController, _BaseSkillModel.baseDamage, (WuXing)_BaseSkillModel.fiveElement, (float)FaQiAttackOnceTime(_BaseSkillModel), FlyWuQiDic.Count + 1, BattleMgr.m_BattleSceneResourceManager.FaQiDic, (int)_BaseSkillModel.usedPerception);
			component.InitDamageAdd(_BaseSkillModel.GoldDamageAdd, _BaseSkillModel.WoodDamageAdd, _BaseSkillModel.WaterDamageAdd, _BaseSkillModel.FireDamageAdd, _BaseSkillModel.EarthDamageAdd);
			if (!FlyWuQiDic.ContainsKey(_BaseSkillModel.id))
			{
				FlyWuQiDic.Add(_BaseSkillModel.id, component);
			}
			if (m_Target != null)
			{
				component.SetTarget(m_Target);
			}
		}

		private double FaQiAttackOnceTime(BaseSkillModel data)
		{
			return (double)data.castTime / 1000.0;
		}

		public bool DanTiFaShuInit(BaseSkillModel _BaseSkillModel, bool isFromFuLu = false)
		{
			if (m_Target == null)
			{
				MonoSingleton<GameStart>.Instance.PopRollingTextShow("需要一个目标！");
				return false;
			}
			BattleMgr.Flourish?.Launch(this, m_Target, _BaseSkillModel.fiveElement);
			DanTiLocked(_BaseSkillModel, isFromFuLu);
			return true;
		}

		public bool DotFaShuInit(BaseSkillModel _BaseSkillModel, bool isFromFuLu = false)
		{
			if (m_Target == null)
			{
				MonoSingleton<GameStart>.Instance.PopRollingTextShow("需要一个目标！");
				return false;
			}
			double damage = _BaseSkillModel.baseDamage;
			BattleMgr.Flourish?.Launch(this, m_Target, _BaseSkillModel.fiveElement);
			m_Target.DotUnderAttack(_BaseSkillModel.SkillIndex, this, damage, _BaseSkillModel);
			return true;
		}

		public bool AoeFaShuInit(BaseSkillModel _BaseSkillModel, bool isFromFuLu = false)
		{
			if (m_Target != null)
			{
				AoeContinuous(_BaseSkillModel, isFromFuLu);
				return true;
			}
			MonoSingleton<GameStart>.Instance.PopRollingTextShow("需要一个目标！");
			return false;
		}

		public void HuDunFaShuInit(BaseSkillModel _BaseSkillModel, bool isFromFuLu = false)
		{
			WuXing fiveElement = (WuXing)_BaseSkillModel.fiveElement;
			GameObject value = null;
			if (BattleMgr.m_BattleSceneResourceManager.HuDunFaShuDic.TryGetValue(fiveElement, out value))
			{
				GameObject huDun = Object.Instantiate(value, BaseController.transform);
				float value2 = (float)(double)_BaseSkillModel.baseDamage;
				BaseController.SetHuDun(huDun);
				AddHuDun(value2, _BaseSkillModel.fiveElement, _BaseSkillModel.icon, _BaseSkillModel.id);
			}
			BattleMgr.AddBuff(_BaseSkillModel.id, _BaseSkillModel.fiveElement, 0, -1f, 20);
		}

		public void Buff_AddDamageInit(BaseSkillModel _BaseSkillModel, bool isFromFuLu = false)
		{
			float num = 0.05f + (float)(WorldSetting.GetLevelByRank(_BaseSkillModel.rank) - 1) * 0.015f - (float)(Singleton<RoleManager>.Instance.GetRoleStage() - ((int)_BaseSkillModel.level - 1) / 10 + 1) * 0.02f;
			num = ((num < 0f) ? 0f : num);
			if (!isFromFuLu)
				num *= Mathf.Max(1f, (float)(double)_BaseSkillModel.baseDamage);
			AddWuXingDamage(!isFromFuLu && (double)_BaseSkillModel.continued > 0 ? (float)(double)_BaseSkillModel.continued : 300f, num, _BaseSkillModel.fiveElement, _BaseSkillModel.icon, _BaseSkillModel.id);
		}

		public void Buff_AddDefenceInit(BaseSkillModel _BaseSkillModel, bool isFromFuLu = false)
		{
			float num = 0.05f + (float)(WorldSetting.GetLevelByRank(_BaseSkillModel.rank) - 1) * 0.015f - (float)(Singleton<RoleManager>.Instance.GetRoleStage() - ((int)_BaseSkillModel.level - 1) / 10 + 1) * 0.02f;
			num = ((num < 0f) ? 0f : num);
			if (!isFromFuLu)
				num *= Mathf.Max(1f, (float)(double)_BaseSkillModel.baseDamage);
			AddWuXingDefence(!isFromFuLu && (double)_BaseSkillModel.continued > 0 ? (float)(double)_BaseSkillModel.continued : 300f, num, _BaseSkillModel.fiveElement, _BaseSkillModel.icon, _BaseSkillModel.id);
		}

		public bool DeBuff_AddDingShen(BaseSkillModel _BaseSkillModel, bool isFromFuLu = false)
		{
			if (m_Target == null)
			{
				MonoSingleton<GameStart>.Instance.PopRollingTextShow("需要一个目标！");
				return false;
			}
			float time = _BaseSkillModel.MovementSeconds(1f);
			m_Target.BeiDingShen(time, _BaseSkillModel.level, _BaseSkillModel.fiveElement, _BaseSkillModel.icon, _BaseSkillModel.id);
			return true;
		}

		public bool DeBuff_AddJianSu(BaseSkillModel _BaseSkillModel, bool isFromFuLu = false)
		{
			if (m_Target == null)
			{
				MonoSingleton<GameStart>.Instance.PopRollingTextShow("需要一个目标！");
				return false;
			}
			m_Target.JianShaoMoveSpeed(_BaseSkillModel.MovementSeconds(2f), _BaseSkillModel.SlowFraction(),
				_BaseSkillModel.level, _BaseSkillModel.fiveElement, _BaseSkillModel.icon, _BaseSkillModel.id);
			return true;
		}

		private void DanTiLocked(BaseSkillModel _BaseSkillModel, bool isFromFuLu)
		{
			GameObject value = null;
			WuXing fiveElement = (WuXing)_BaseSkillModel.fiveElement;
			if (BattleMgr.m_BattleSceneResourceManager.DanTiFaShuDic.TryGetValue(fiveElement, out value))
			{
				GameObject gameObject = Object.Instantiate(value);
				gameObject.transform.position = new Vector3(BaseController.transform.position.x, BaseController.transform.position.y, -3f);
				gameObject.GetComponent<DanTiSkill_Item>().Init(_BaseSkillModel.SkillIndex, this, _BaseSkillModel.baseDamage, fiveElement, m_Target);
			}
		}

		private AoeSkill_Item AoeContinuous(BaseSkillModel _BaseSkillModel, bool isFromFuLu)
		{
			GameObject gameObject = Object.Instantiate(BattleMgr.m_BattleSceneResourceManager.AoeInsObj);
			gameObject.transform.position = new Vector3(m_Target.GetPos().x, m_Target.GetPos().y, -3f);
			AoeSkill_Item component = gameObject.GetComponent<AoeSkill_Item>();
			float duration = !isFromFuLu && (double)_BaseSkillModel.continued > 0 ? (float)(double)_BaseSkillModel.continued : 15f;
			component.Init(_BaseSkillModel.SkillIndex, this, _BaseSkillModel.baseDamage, _BaseSkillModel.fiveElement, duration, 1.5f, EntityType.Monster, BattleMgr.m_BattleSceneResourceManager.AoeFaShuDic);
			return component;
		}

		public void ZhenFaInit(BaseSkillModel _BaseSkillModel)
		{
			if (m_UsingZhenFa == null)
			{
				m_UsingZhenFa = _BaseSkillModel;
				m_ZhenFa_InsObj = BattleMgr.m_BattleSceneResourceManager.ZhenFaInsObj;
				GameObject gameObject = Object.Instantiate(m_ZhenFa_InsObj);
				gameObject.transform.position = new Vector3(BaseController.gameObject.transform.position.x, BaseController.gameObject.transform.position.y, BaseController.gameObject.transform.position.z);
				ZhenFa_Item component = gameObject.GetComponent<ZhenFa_Item>();
				if (!(component == null))
				{
					component.Init((BattleZhenFaType)_BaseSkillModel.skillType, this, _BaseSkillModel, BattleMgr.GetMapStage());
					m_UsingZhenFaDuringTime = (float)(double)_BaseSkillModel.continued;
				}
			}
			else
			{
				MonoSingleton<GameStart>.Instance.PopRollingTextShow("已有其他运行中的阵法");
			}
		}

		protected override void ShenShiFoundEnterTriggerCheck(Collider2D _collider2d)
		{
			base.ShenShiFoundEnterTriggerCheck(_collider2d);
			Controller componentInParent = _collider2d.gameObject.GetComponentInParent<Controller>();
			if (componentInParent != null && !FoundAreaObjDic.ContainsKey(componentInParent.Id))
			{
				FoundAreaObjDic.Add(componentInParent.Id, componentInParent.EntityDataSource);
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
			if (componentInParent == null) return;
			if (!LockAreaObjDic.ContainsKey(componentInParent.Id))
			{
				LockAreaObjDic.Add(componentInParent.Id, componentInParent.EntityDataSource);
			}
			if (m_Target != null || !CanTarget(componentInParent.EntityDataSource)) return;
			m_Target = componentInParent.EntityDataSource;
			if (FlyWuQiDic != null)
			{
				foreach (KeyValuePair<string, WuQi_Item> item in FlyWuQiDic)
				{
					if (item.Value.CurrentState == WuQiState.Waiting)
					{
						item.Value.SetTarget(m_Target);
					}
				}
			}
			(componentInParent as NormalMonsterController).Locked();
			BattleMgr.PlayerTargetLocked(m_Target.GetBattleProps());
		}

		protected override void ShenShiLockExitTriggerCheck(Collider2D _collider2d)
		{
			base.ShenShiLockExitTriggerCheck(_collider2d);
			Controller componentInParent = _collider2d.gameObject.GetComponentInParent<Controller>();
			if (componentInParent == null)
			{
				return;
			}
			if (m_Target == null)
			{
				if (LockAreaObjDic.ContainsKey(componentInParent.Id))
				{
					LockAreaObjDic.Remove(componentInParent.Id);
				}
			}
			else if (componentInParent.Id == m_Target.BaseController.Id)
			{
				(componentInParent as NormalMonsterController).CancleLocked();
				if (LockAreaObjDic.ContainsKey(componentInParent.Id))
				{
					LockAreaObjDic.Remove(componentInParent.Id);
					ChangeTarget();
				}
			}
			else if (LockAreaObjDic.ContainsKey(componentInParent.Id))
			{
				LockAreaObjDic.Remove(componentInParent.Id);
			}
		}

		private bool CanTarget(EntityBase unit)
		{
			return unit != null && unit.BaseController != null && !unit.BaseController.IsDie && unit.VisibleToPlayer();
		}

		private void ChangeTarget()
		{
			EntityBase next = null;
			float nearest = float.PositiveInfinity;
			foreach (var candidate in LockAreaObjDic.Values)
			{
				if (!CanTarget(candidate)) continue;
				float distance = (candidate.GetPos() - GetPos()).sqrMagnitude;
				if (distance >= nearest) continue;
				nearest = distance;
				next = candidate;
			}
			if (next == m_Target) return;
			if (m_Target != null && m_Target.BaseController is NormalMonsterController old && old != null) old.CancleLocked();
			m_Target = next;
			foreach (var weapon in FlyWuQiDic.Values)
			{
				if (next != null) weapon.ReSetTarget(next);
				else if (weapon.CurrentState != WuQiState.Waiting) weapon.CancleTarget();
			}
			if (next == null) BattleMgr.PlayerTargetCancleLocked();
			else
			{
				(next.BaseController as NormalMonsterController)?.Locked();
				BattleMgr.PlayerTargetLocked(next.GetBattleProps());
			}
		}

		public void MonsterDead(string monsterId)
		{
			if (LockAreaObjDic.ContainsKey(monsterId))
			{
				if (m_Target == LockAreaObjDic[monsterId])
				{
					(LockAreaObjDic[monsterId].BaseController as NormalMonsterController).CancleLocked();
					LockAreaObjDic.Remove(monsterId);
					ChangeTarget();
				}
				else
				{
					LockAreaObjDic.Remove(monsterId);
				}
			}
		}

		protected override void AddBuff(BaseBuffModel buff)
		{
			base.AddBuff(buff);
			if (BattleMgr == null || (double)BattleModel.CurrentBlood <= 0 || !BuffList.TryGetValue(buff.BuffId, out var active)) return;
			int skillType;
			switch (active.BuffType)
			{
				case BattleBuffType.DingShen: skillType = 41; break;
				case BattleBuffType.JianSu: skillType = 40; break;
				case BattleBuffType.MoveHaste: skillType = 50; break;
				default: return;
			}
			BattleMgr.AddBuff(active.BuffId, active.FiveElement, active.Icon, active.GetShengYuTime(), skillType);
		}

		public override void ShiFaYanChang(float _time, float _value, int _skillLevel, int fiveElement, int icon, string _skillId)
		{
			base.ShiFaYanChang(_time, _value, _skillLevel, fiveElement, icon, _skillId);
			BattleMgr.AddBuff(_skillId, fiveElement, icon, _time, 40);
		}

		public override void AddWuXingDamage(float _time, float _value, int fiveElement, int icon, string _skillId)
		{
			base.AddWuXingDamage(_time, _value, fiveElement, icon, _skillId);
		}

		public override void AddWuXingDefence(float _time, float _value, int fiveElement, int icon, string _skillId)
		{
			base.AddWuXingDefence(_time, _value, fiveElement, icon, _skillId);
		}

		public override void AddDiKangDamage(float _time, string _skillId, int icon)
		{
			base.AddDiKangDamage(_time, _skillId, icon);
		}

		public override void DotUnderAttack(int skillIndex, EntityBase _damageFromEntity, double _damage, BaseSkillModel _baseSkillModel)
		{
			base.DotUnderAttack(skillIndex, _damageFromEntity, _damage, _baseSkillModel);
			BattleMgr.AddBuff(_baseSkillModel.id, 0, _baseSkillModel.icon, (float)(double)_baseSkillModel.continued, 12);
		}

		public override double UnderAttack(int skillIndex, EntityBase _damageFromEntity, double _damage, int _fiveElement)
		{
			lastAttackTime = Time.time;
			double num = (double)m_BattleModel.CurrentBlood + (double)m_BattleModel.CurrentJianRen;
			if (m_IsWuDi)
			{
				BaseController.ShowStateText("吸收");
				return 0.0;
			}
			switch ((WuXing)_fiveElement)
			{
			case WuXing.Jin:
				_damage *= (double)base.BattleModel.GoldDamageAvoidance;
				break;
			case WuXing.Mu:
				_damage *= (double)base.BattleModel.WoodDamageAvoidance;
				break;
			case WuXing.Shui:
				_damage *= (double)base.BattleModel.WaterDamageAvoidance;
				break;
			case WuXing.Huo:
				_damage *= (double)base.BattleModel.FireDamageAvoidance;
				break;
			case WuXing.Tu:
				_damage *= (double)base.BattleModel.WoodDamageAvoidance;
				break;
			}
			double num2 = 0.0;
			_damage -= num2;
			_damage = GetDamageAfterDefenceFaQi(_damage, _fiveElement);
			if (_damage <= 0.0)
			{
				BaseController.ShowStateText("吸收");
				return 0.0;
			}
			double num3 = base.UnderAttack(skillIndex, _damageFromEntity, _damage, _fiveElement);
			RoleGetDamageReq_8001014 data = new RoleGetDamageReq_8001014
			{
				Before = (long)num,
				After = (long)((double)m_BattleModel.CurrentBlood + (double)m_BattleModel.CurrentJianRen),
				Damage = (long)num3,
				RoleIndex = m_BattleModel.ObjectIndex
			};
			Singleton<NetManager>.Instance.SendMsg(MsgType.RoleGetDamage, data, null, isShowWaitUI: false, ServerType.BattleServer);
			return num3;
		}

		protected override void RemoveHuDunAddEffect(string Id)
		{
			base.RemoveHuDunAddEffect(Id);
			if (!string.IsNullOrEmpty(Id))
			{
				BattleMgr.CancleBuff(Id);
			}
		}

		private double GetDamageAfterDefenceFaQi(double _damage, int _fiveElement)
		{
			double num = 0.0;
			double num2 = 0.0;
			using (Dictionary<string, DefenceWuQi_Item>.Enumerator enumerator = m_DefenceWuQiDic.GetEnumerator())
			{
				if (enumerator.MoveNext())
				{
					KeyValuePair<string, DefenceWuQi_Item> current = enumerator.Current;
					num2 = ((num2 > 0.4) ? 0.4 : num2);
					_damage -= _damage * num2;
					num = current.Value.GetDefenceDamage(_damage, _fiveElement);
					if ((double)m_BattleModel.CurrentSuperAir >= num)
					{
						BattleLifeModel battleModel = m_BattleModel;
						battleModel.CurrentSuperAir = (double)battleModel.CurrentSuperAir - num;
						return 0.0;
					}
					_ = current.Value;
					_damage *= (num - (double)m_BattleModel.CurrentSuperAir) / num;
					m_BattleModel.CurrentSuperAir = 0.0;
					if (m_WuQiUIDic.ContainsKey(current.Key))
					{
						m_WuQiUIDic[current.Key].SetFaQiRunningAni(isShowAni: false);
					}
					BattleLifeModel battleModel2 = m_BattleModel;
					battleModel2.CurrentPerception = (double)battleModel2.CurrentPerception + (double)(int)current.Value.DataSource.usedPerception;
					m_BattleModel.CurrentPerception = (((double)m_BattleModel.CurrentPerception > (double)m_BattleModel.MaxPerception) ? m_BattleModel.MaxPerception : m_BattleModel.CurrentPerception);
				}
			}
			m_DefenceWuQiDic.Clear();
			BaseController.CancleShowDefenceEquipmentEffect();
			return _damage;
		}

		public override void ChaseTarget(Vector3 vector)
		{
			Vector3 previous = GetPos();
			base.ChaseTarget(vector);
			if (BattleMgr.Layout != null)
			{
				Vector3 current = GetPos();
				if (BattleMgr.Layout.Index(previous) != BattleMgr.Layout.Index(current))
				{
					// A batched diagonal spanning two turns could otherwise cross a blocked corner.
					SendRolePosInfoToNet(previous);
					SendRolePosInfoToNet(current);
				}
				Vector3 moved = current - previous;
				if (moved.sqrMagnitude > .000001f && Time.time >= lastStepEffect && BattleMgr.Flourish != null
					&& (BattleMgr.FieldView == null || !BattleMgr.FieldView.AnimatesPlayerSteps))
				{
					lastStepEffect = Time.time + .16f;
					BattleMgr.Flourish.Footfall(this, moved.normalized);
				}
				return;
			}
			maxMin mapMaxAndMin = BattleMgr.GetMapMaxAndMin();
			Transform trans = GetTrans();
			if (!(trans == null))
			{
				if (GetPos().x > (float)mapMaxAndMin.maxX)
				{
					trans.position = new Vector3(mapMaxAndMin.maxX, GetPos().y, GetPos().z);
				}
				if (GetPos().x < (float)mapMaxAndMin.minX)
				{
					trans.position = new Vector3(mapMaxAndMin.minX, GetPos().y, GetPos().z);
				}
				if (GetPos().y < (float)mapMaxAndMin.minY)
				{
					trans.position = new Vector3(GetPos().x, mapMaxAndMin.minY, GetPos().z);
				}
				if (GetPos().y > (float)mapMaxAndMin.maxY)
				{
					trans.position = new Vector3(GetPos().x, mapMaxAndMin.maxY, GetPos().z);
				}
			}
		}

		protected override void DeadMethod()
		{
			if (Time.timeScale == 0f)
			{
				return;
			}
			BattleMgr?.ClearAllMonsterTarget();
			SetController(isShow: false);
			ClearAllBuff();
			ClearFaQi();
			List<BaseNaJieGoodEntity> data = Singleton<RoleManager>.Instance.RoleBoxDic[10].SelectGoods(37);
			if (data != null && data.Count != 0 && BattleMgr.CurrentBattleType != BattleType.TongTianTaBattle)
			{
				m_BattleModel.CurrentBlood = 1.0;
				MonoSingleton<GameStart>.Instance.PopUpAttention("您已经进入重伤状态，是否使用替身傀儡进行逃遁?", () =>
				{
					UseTiShenKuiLei(data);
					Time.timeScale = 1f;
				}, () =>
				{
					Time.timeScale = 1f;
					m_BattleModel.CurrentBlood = 0.0;
					base.DeadMethod();
					BattleMgr.PlayerDead();
					BaseController?.Dead();
				});
			}
			else
			{
				m_BattleModel.CurrentBlood = 0.0;
				base.DeadMethod();
				BattleMgr.PlayerDead();
			}
		}

		private void UseTiShenKuiLei(List<BaseNaJieGoodEntity> data)
		{
			ReviveReq_8001011 data2 = new ReviveReq_8001011
			{
				RoleIndex = m_BattleModel.ObjectIndex
			};
			Singleton<NetManager>.Instance.SendMsg(MsgType.Revive, data2, (object _obj) =>
			{
				MonoSingleton<GameStart>.Instance.HideWaitReauestWnd();
				ByteString byteString = _obj as ByteString;
				if (!(byteString == null))
				{
					ReviveResp_8001011 reviveResp_ = new ReviveResp_8001011();
					reviveResp_.MergeFrom(byteString);
					if (reviveResp_.IsSuccess)
					{
						m_IsWuDi = true;
						Timer.Register(3f, () =>
						{
							m_IsWuDi = false;
						});
						m_BattleModel.CurrentJianRen = m_BattleModel.Max_JianRen;
						m_BattleModel.CurrentBlood = m_BattleModel.Max_Blood;
						if (CurrentAniState == AniState.DingShen) Idle();
						BaseController.gameObject.transform.position = new Vector3((float)reviveResp_.PosX, (float)reviveResp_.PosY, 0f);
						BattleMgr?.ClearAllMonsterTarget();
						SetController(isShow: true);
						if (data != null && data.Count > 0)
						{
							Singleton<RoleManager>.Instance.RoleBoxDic[10].RemoveGoods(data[0].GoodsId, 1);
						}
					}
					else
					{
						string hintMsgByCode = MonoSingleton<GameStart>.Instance.GetHintMsgByCode(reviveResp_.Code);
						MonoSingleton<GameStart>.Instance.PopUpAttention(hintMsgByCode, () =>
						{
							base.DeadMethod();
							BattleMgr.PlayerDead();
						}, () =>
						{
							base.DeadMethod();
							BattleMgr.PlayerDead();
						});
					}
				}
			}, isShowWaitUI: true, ServerType.BattleServer);
		}

		private void ClearFaQi()
		{
			List<DefenceWuQi_Item> list = new List<DefenceWuQi_Item>();
			foreach (DefenceWuQi_Item value in m_DefenceWuQiDic.Values)
			{
				list.Add(value);
			}
			List<WuQi_Item> list2 = new List<WuQi_Item>();
			foreach (WuQi_Item value2 in FlyWuQiDic.Values)
			{
				list2.Add(value2);
			}
			foreach (DefenceWuQi_Item item in list)
			{
				if (m_WuQiUIDic.ContainsKey(item.DataSource.id))
				{
					BattleMgr.CloseFaQi(item.DataSource.id, (int)item.DataSource.usedPerception, m_WuQiUIDic[item.DataSource.id]);
				}
			}
			foreach (WuQi_Item item2 in list2)
			{
				if (m_WuQiUIDic.ContainsKey(item2.WeponId))
				{
					BattleMgr.CloseFaQi(item2.WeponId, item2.PercentUsed, m_WuQiUIDic[item2.WeponId]);
				}
			}
		}

		private void SetController(bool isShow)
		{
			(BaseController as PlayerController).SetCollider2D(isShow);
		}

		public override void SetShenShiArea(double scale)
		{
			float num = (float)scale;
			ShenShiSizeScale = num * OneScaleCircleSize;
			BaseController.SetShenShiCircleScale(num);
		}

		public void PickUpGoods(string goodsId, int objectId, string name, GameObject gameObj, int stones = 0)
		{
			if (string.IsNullOrEmpty(goodsId))
			{
				Object.Destroy(gameObj.gameObject);
				Debug.Log("GoodsId为空" + name);
				return;
			}
			if (!pendingPickups.Add(goodsId)) return;
			SendRolePosInfoToNet(GetPos());
			var pickup = gameObj.GetComponent<DiaoLuoItem>();
			int quantity = stones > 0 ? stones : (pickup ? Mathf.Max(1, pickup.Count) : 1);
			var ledger = BattleMgr.Ledger;
			PickUpGoodsReq_1004026 data = new PickUpGoodsReq_1004026
			{
				GoodsId = goodsId,
				Count = 1,
				BattleId = BattleMgr.DungenonId
			};
			bool sent = Singleton<NetManager>.Instance.TrySendGameRequest(MsgType.PICK_UP_GOODS, data, byteString =>
			{
				pendingPickups.Remove(goodsId);
				if (byteString != null)
				{
					PickUpGoodsResp_1004026 pickUpGoodsResp_ = new PickUpGoodsResp_1004026();
					pickUpGoodsResp_.MergeFrom(byteString);
					if (pickUpGoodsResp_.IsSuccess)
					{
						ledger.Record(goodsId, name, quantity);
						if (BattleMgr == null || gameObj == null) return;
						if (BattleMgr.Flourish != null) BattleMgr.Flourish.Gather(this, gameObj.transform.position, stones > 0 || gameObj.CompareTag("Mineral"));
						if (stones > 0) Singleton<RoleManager>.Instance.Role_Entity.ShinyStone += stones;
						PickGoods(objectId);
						Object.Destroy(gameObj);
						MonoSingleton<GameStart>.Instance.PopRollingTextShow($"获得{name} ×{quantity}");
					}
					else if (BattleMgr != null && gameObj != null)
					{
						string hintMsgByCode = MonoSingleton<GameStart>.Instance.GetHintMsgByCode(pickUpGoodsResp_.Code);
						MonoSingleton<GameStart>.Instance.PopRollingTextShow(hintMsgByCode);
					}
				}
			});
			if (!sent) pendingPickups.Remove(goodsId);
		}

		public void PickUpLingShi(string goodsId, int objectId, int count, GameObject gameObj)
		{
			PickUpGoods(goodsId, objectId, "灵石", gameObj, count);
		}

		public override void ClearAllBuff()
		{
			BattleMgr.ClearAllBuff();
			base.ClearAllBuff();
		}

		private void PickGoods(int objectId)
		{
			RolePickGoodsReq_8001007 data = new RolePickGoodsReq_8001007
			{
				ObjectIndex = objectId
			};
			Singleton<NetManager>.Instance.SendMsg(MsgType.PICKGOODS, data, null, isShowWaitUI: false, ServerType.BattleServer);
		}
	}
}
