using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace XiuZhenJie
{
	public class EntityBase
	{
		public AniState CurrentAniState;

		public BattleSceneManager BattleMgr;

		public StateManager StateMgr;

		public SkillManager SkillMgr;

		public Controller BaseController;

		public bool CanControl = true;

		public bool CanRlsSkill = true;

		public EntityType Entity_Type;

		protected Dictionary<string, EntityBase> FoundAreaObjDic = new Dictionary<string, EntityBase>();

		protected Dictionary<string, EntityBase> LockAreaObjDic = new Dictionary<string, EntityBase>();

		public float OneScaleCircleSize = 1.3f;

		public float ShenShiScale = 1f;

		public float ShenShiSizeScale = 1.3f;

		protected Vector3 m_InitPos;

		protected bool m_IsInBattleStatus;

		protected bool m_CanleDotDamage;

		public List<int> SkActionCBLst = new List<int>();

		public Dictionary<string, BaseBuffModel> BuffList = new Dictionary<string, BaseBuffModel>();

		protected Dictionary<string, BaseSkillModel> m_ZhenFaList = new Dictionary<string, BaseSkillModel>();

		protected Dictionary<string, ZhenFa_Item> m_ZhenFaItemList = new Dictionary<string, ZhenFa_Item>();

		protected BattleLifeModel m_BattleModel;

		protected EntityBase m_Target;

		protected Timer m_ShiFaTimer;

		protected bool m_IsRecovering;

		public BattleLifeModel BattleModel
		{
			get
			{
				return m_BattleModel;
			}
			protected set
			{
				m_BattleModel = value;
			}
		}

		public EntityBase Target
		{
			get
			{
				return m_Target;
			}
			protected set
			{
				m_Target = value;
			}
		}

		public Timer ShiFaTimer
		{
			get
			{
				return m_ShiFaTimer;
			}
			protected set
			{
				m_ShiFaTimer = value;
			}
		}

		public void Born()
		{
			StateMgr.ChangeStatus(this, AniState.Born, null);
		}

		public void Move()
		{
			StateMgr.ChangeStatus(this, AniState.Move, null);
		}

		public void DingShen()
		{
			StateMgr.ChangeStatus(this, AniState.DingShen, null);
		}

		public void Idle()
		{
			StateMgr.ChangeStatus(this, AniState.Idle, null);
		}

		public void Attack()
		{
			StateMgr.ChangeStatus(this, AniState.Attack, null);
		}

		public void Hit()
		{
			StateMgr.ChangeStatus(this, AniState.Hit, null);
		}

		public void Die()
		{
			StateMgr.ChangeStatus(this, AniState.Die, null);
		}

		public void GoBack()
		{
			StateMgr.ChangeStatus(this, AniState.GoBack, null);
		}

		public virtual void TickAILogic()
		{
			if (!(BattleMgr == null))
			{
				if ((double)BattleModel.CurrentSuperAir < (double)BattleModel.MaxSuperAir)
				{
					RecoverSuperAir();
				}
				if ((double)BattleModel.CurrentJianRen < (double)BattleModel.Max_JianRen)
				{
					RecoverJianRen();
				}
				if (m_Target == null)
				{
					m_IsInBattleStatus = false;
				}
				else
				{
					m_IsInBattleStatus = true;
				}
			}
		}

		public virtual void FixUpdate()
		{
		}

		public virtual void SetCtrl(Controller ctrl, string id, Vector3 locationVector3)
		{
			BaseController = ctrl;
			ctrl.Init(id, this, ShenShiLockEnterTriggerCheck, ShenShiLockExitTriggerCheck, ShenShiFoundEnterTriggerCheck, ShenShiFoundExitTriggerCheck);
			BaseController.transform.position = locationVector3;
			m_InitPos = BaseController.transform.position;
		}

		public void SetActive(bool active = true)
		{
			if (BaseController != null)
			{
				BaseController.gameObject.SetActive(active);
			}
		}

		public virtual void SetBattleProps(BattleLifeModel _battleLifeModel)
		{
			m_BattleModel = _battleLifeModel;
			m_IsInBattleStatus = false;
		}

		public BattleLifeModel GetBattleProps()
		{
			return m_BattleModel;
		}

		public virtual void SetDodge()
		{
			_ = BaseController != null;
		}

		public virtual void SetCritical(int critical)
		{
			_ = BaseController != null;
		}

		public virtual void SetHurt(double hurt)
		{
			_ = BaseController != null;
		}

		public virtual void SetHPVal(double oldval, double newval)
		{
			_ = BaseController != null;
		}

		public virtual void RecoverSuperAir()
		{
			if (BattleModel != null && !BaseController.IsDie && !((double)BattleModel.CurrentSuperAir >= (double)BattleModel.MaxSuperAir))
			{
				BattleLifeModel battleModel = BattleModel;
				battleModel.CurrentSuperAir = (double)battleModel.CurrentSuperAir + (m_IsInBattleStatus ? ((double)BattleModel.RecoverSuperAirEverySecondBattle * (double)Time.deltaTime) : ((double)BattleModel.RecoverSuperAirEverySecondNormal * (double)Time.deltaTime));
				if ((double)BattleModel.CurrentSuperAir >= (double)BattleModel.MaxSuperAir)
				{
					BattleModel.CurrentSuperAir = BattleModel.MaxSuperAir;
				}
			}
		}

		public virtual void RecoverSuperAirValue(float value)
		{
			if (BattleModel != null && !BaseController.IsDie && !((double)BattleModel.CurrentSuperAir >= (double)BattleModel.MaxSuperAir))
			{
				BattleLifeModel battleModel = BattleModel;
				battleModel.CurrentSuperAir = (double)battleModel.CurrentSuperAir + (double)value;
				if ((double)BattleModel.CurrentSuperAir >= (double)BattleModel.MaxSuperAir)
				{
					BattleModel.CurrentSuperAir = BattleModel.MaxSuperAir;
				}
			}
		}

		public virtual void RecoverJianRen()
		{
			if (BattleModel != null && !BaseController.IsDie && !((double)BattleModel.CurrentJianRen >= (double)BattleModel.Max_JianRen))
			{
				BattleLifeModel battleModel = BattleModel;
				battleModel.CurrentJianRen = (double)battleModel.CurrentJianRen + (m_IsInBattleStatus ? ((double)BattleModel.RecoverJianRenEverySecondBattle * (double)Time.deltaTime) : ((double)BattleModel.RecoverJianRenEverySecondNormal * (double)Time.deltaTime));
				if ((double)BattleModel.CurrentJianRen >= (double)BattleModel.Max_JianRen)
				{
					BattleModel.CurrentJianRen = BattleModel.Max_JianRen;
				}
			}
		}

		public virtual void RecoverJianRenValue(float value)
		{
			if (BattleModel != null && !BaseController.IsDie && !((double)BattleModel.CurrentJianRen >= (double)BattleModel.Max_JianRen))
			{
				BattleLifeModel battleModel = BattleModel;
				battleModel.CurrentJianRen = (double)battleModel.CurrentJianRen + (double)value;
				if ((double)BattleModel.CurrentJianRen >= (double)BattleModel.Max_JianRen)
				{
					BattleModel.CurrentJianRen = BattleModel.Max_JianRen;
				}
			}
		}

		public virtual Vector3 GetPos()
		{
			if (BaseController == null)
			{
				return Vector3.zero;
			}
			return BaseController.transform.position;
		}

		public virtual Transform GetTrans()
		{
			if (BaseController == null)
			{
				return null;
			}
			return BaseController.transform;
		}

		public virtual void MoveX(Vector2 vector)
		{
			Transform trans = GetTrans();
			if (!(trans == null))
			{
				trans.position += new Vector3(vector.x * (float)(double)m_BattleModel.CurrentMoveSpeed * Time.deltaTime, 0f);
			}
		}

		public virtual void ChaseTarget(Vector3 vector)
		{
			if (m_BattleModel != null)
			{
				BaseController.SetDirection(vector);
				Transform trans = GetTrans();
				if (!(trans == null))
				{
					trans.position += new Vector3(vector.x * (float)(double)m_BattleModel.CurrentMoveSpeed * 0.01f, vector.y * (float)(double)m_BattleModel.CurrentMoveSpeed * 0.01f);
				}
			}
		}

		public virtual void SetShenShiArea(double scale)
		{
			ShenShiSizeScale = (float)scale * OneScaleCircleSize;
			BaseController.SetShenShiCircleScale((float)scale);
		}

		public virtual void SetTouXiangIcon(Sprite touXiangSprite)
		{
			BaseController.SetTouXiangSprite(touXiangSprite);
			BaseController.SetTouXiangColor();
		}

		public virtual bool XiaoHaoSuperAir(double superAir)
		{
			if ((double)BattleModel.CurrentSuperAir >= superAir)
			{
				BattleLifeModel battleModel = BattleModel;
				battleModel.CurrentSuperAir = (double)battleModel.CurrentSuperAir - superAir;
				return true;
			}
			return false;
		}

		public virtual double UnderAttack(int skillIndex, EntityBase _damageFromEntity, double _damage, int _fiveElement)
		{
			if (BaseController.IsDie)
			{
				return 0.0;
			}
			if (m_BattleModel == null)
			{
				return 0.0;
			}
			BaseSkillModel baseSkillModel = _damageFromEntity.CheckIfInMatrix(BattleZhenFaType.Huan);
			if (baseSkillModel != null && WorldSetting.GetIsSuccess((float)(double)baseSkillModel.baseDamage * 100f))
			{
				BaseController.ShowStateText("抵抗");
				return 0.0;
			}
			if (BattleModel.DiKangDamage)
			{
				string text = string.Empty;
				foreach (KeyValuePair<string, BaseBuffModel> buff in BuffList)
				{
					if (buff.Value.BuffType == BattleBuffType.DiDang)
					{
						text = buff.Key;
						break;
					}
				}
				if (!string.IsNullOrEmpty(text))
				{
					BuffList[text].CancleBuff();
				}
				BaseController.ShowStateText("吸收");
				return 0.0;
			}
			_damage -= _damage * (double)m_BattleModel.DefeneceAdd;
			double num = 0.0;
			DamageNumType damageNumType = DamageNumType.Yellow;
			if ((double)m_BattleModel.CurrentHuDun > 0.0)
			{
				switch (WorldSetting.GetWuXingComplement((WuXing)m_BattleModel.HuDunFiveElement, (WuXing)_fiveElement))
				{
				case 1:
					_damage += _damage * 0.5;
					damageNumType = DamageNumType.Red;
					break;
				case 2:
					_damage -= _damage * 0.5;
					damageNumType = DamageNumType.Gray;
					break;
				}
				if ((double)m_BattleModel.CurrentHuDun > _damage)
				{
					BattleLifeModel battleModel = m_BattleModel;
					battleModel.CurrentHuDun = (double)battleModel.CurrentHuDun - _damage;
					_damage = 0.0;
					num = _damage;
					BaseController.ShowStateText("吸收");
					return num;
				}
				_damage -= (double)m_BattleModel.CurrentHuDun;
				string text2 = "";
				foreach (KeyValuePair<string, BaseBuffModel> buff2 in BuffList)
				{
					if (buff2.Value.BuffType == BattleBuffType.HuDun)
					{
						text2 = buff2.Key;
						break;
					}
				}
				if (!string.IsNullOrEmpty(text2))
				{
					BuffList.Remove(text2);
				}
				RemoveHuDunAddEffect(text2);
				num += (double)m_BattleModel.CurrentHuDun;
			}
			_ = m_BattleModel.DefenseFiveElement;
			switch (WorldSetting.GetWuXingComplement((WuXing)m_BattleModel.DefenseFiveElement, (WuXing)_fiveElement))
			{
			case 1:
				_damage += _damage * 0.5;
				damageNumType = DamageNumType.Red;
				break;
			case 2:
				_damage -= _damage * 0.5;
				damageNumType = DamageNumType.Gray;
				break;
			}
			if (_damage > 0.0 && (double)m_BattleModel.CurrentJianRen > 0.0)
			{
				if ((double)m_BattleModel.CurrentJianRen >= _damage)
				{
					num += _damage;
					BattleLifeModel battleModel2 = m_BattleModel;
					battleModel2.CurrentJianRen = (double)battleModel2.CurrentJianRen - _damage;
					_damage = 0.0;
				}
				else
				{
					num += (double)m_BattleModel.CurrentJianRen;
					_damage -= (double)m_BattleModel.CurrentJianRen;
					m_BattleModel.CurrentJianRen = 0.0;
				}
			}
			if (_damage > 0.0)
			{
				_damage *= 0.5;
				num += _damage;
				BattleLifeModel battleModel3 = m_BattleModel;
				battleModel3.CurrentBlood = (double)battleModel3.CurrentBlood - _damage;
				if ((double)m_BattleModel.CurrentBlood >= _damage)
				{
					BattleLifeModel battleModel4 = m_BattleModel;
					battleModel4.CurrentBlood = (double)battleModel4.CurrentBlood - _damage;
					_damage = 0.0;
				}
				else
				{
					_damage -= (double)m_BattleModel.CurrentBlood;
					m_BattleModel.CurrentBlood = 0.0;
				}
				if ((double)m_BattleModel.CurrentBlood <= 0.0)
				{
					DeadMethod();
				}
			}
			BaseController.ShowDamageNum((ulong)num, damageNumType);
			return num;
		}

		public virtual void DotUnderAttack(int skillIndex, EntityBase _damageFromEntity, double _damage, BaseSkillModel _baseSkillModel)
		{
			if (!(BattleMgr == null))
			{
				BattleMgr.StartCoroutine(DotDamageAttack(skillIndex, _damageFromEntity, _damage, _baseSkillModel.fiveElement, 10f));
			}
		}

		private IEnumerator DotDamageAttack(int skillIndex, EntityBase _damageFromEntity, double _damage, int _fiveElement, float _continueTime, float _onceTime = 1f)
		{
			int times = (int)(_continueTime / _onceTime);
			while (!m_CanleDotDamage && !(BattleMgr == null))
			{
				double damage = _damage / (double)times;
				BaseController.UnderAttack(skillIndex, _damageFromEntity, damage, _fiveElement);
				yield return new WaitForSeconds(_onceTime);
				_continueTime -= _onceTime;
				if (_continueTime < _onceTime)
				{
					break;
				}
			}
		}

		protected virtual void DeadMethod()
		{
			m_BattleModel.CurrentHuDun = 0.0;
			m_BattleModel.CurrentJianRen = 0.0;
			m_BattleModel.CurrentBlood = 0.0;
			BaseController.Dead();
			Die();
		}

		protected virtual void AddBuff(BaseBuffModel _buffModel)
		{
			if (BattleMgr == null || (double)m_BattleModel.CurrentBlood <= 0.0)
			{
				return;
			}
			if (BuffList.ContainsKey(_buffModel.BuffId))
			{
				BuffList[_buffModel.BuffId].ReflashTime();
				return;
			}
			if (_buffModel.BuffType == BattleBuffType.HuDun)
			{
				string text = "";
				foreach (KeyValuePair<string, BaseBuffModel> buff in BuffList)
				{
					if (buff.Value.BuffType == BattleBuffType.HuDun)
					{
						text = buff.Key;
					}
				}
				if (!string.IsNullOrEmpty(text))
				{
					BuffList.Remove(text);
				}
			}
			BuffList.Add(_buffModel.BuffId, _buffModel);
			AddBuffValue(_buffModel);
			_buffModel.DoBuff(BaseController, () =>
			{
				RemoveBuff(_buffModel.BuffId);
			});
		}

		protected virtual void RemoveBuff(string id)
		{
			if (BattleMgr == null || (double)m_BattleModel.CurrentBlood <= 0.0)
			{
				return;
			}
			BaseBuffModel baseBuffModel = null;
			if (BuffList.ContainsKey(id))
			{
				baseBuffModel = BuffList[id];
				BuffList.Remove(id);
				if (baseBuffModel != null)
				{
					RemoveBuffValue(baseBuffModel);
				}
			}
		}

		public virtual void AddBuffValue(BaseBuffModel _buffModel)
		{
			if (!(BattleMgr == null))
			{
				switch (_buffModel.BuffType)
				{
				case BattleBuffType.JianSu:
					JianSuEffect(_buffModel);
					break;
				case BattleBuffType.DingShen:
					DingShenEffect();
					break;
				case BattleBuffType.DamageAdd:
					DamageAddEffect(_buffModel);
					break;
				case BattleBuffType.DefenseAdd:
					DefenceAddEffect(_buffModel);
					break;
				case BattleBuffType.HuDun:
					HuDunAddEffect(_buffModel);
					break;
				case BattleBuffType.ShiFaYanChang:
					ShiFangYanChangEffect(_buffModel);
					break;
				case BattleBuffType.DiDang:
					DiKangDamage();
					break;
				}
			}
		}

		public virtual void RemoveBuffValue(BaseBuffModel _buffModel)
		{
			if (!(BattleMgr == null))
			{
				switch (_buffModel.BuffType)
				{
				case BattleBuffType.JianSu:
					RemoveJianSuEffect(_buffModel);
					break;
				case BattleBuffType.DingShen:
					RemoveDingShenEffect(_buffModel);
					break;
				case BattleBuffType.DamageAdd:
					RemoveDamageAddEffect(_buffModel);
					break;
				case BattleBuffType.DefenseAdd:
					RemoveDefenceAddEffect(_buffModel);
					break;
				case BattleBuffType.ShiFaYanChang:
					RemoveShiFangYanChangEffect(_buffModel);
					break;
				case BattleBuffType.DiDang:
					CancleDiKangDamage();
					break;
				case BattleBuffType.HuDun:
					break;
				}
			}
		}

		protected virtual void JianSuEffect(BaseBuffModel _buffModel)
		{
			ReComputeMoveSpeed();
		}

		protected virtual void RemoveJianSuEffect(BaseBuffModel _buffModel)
		{
			ReComputeMoveSpeed();
		}

		protected virtual void ShiFangYanChangEffect(BaseBuffModel _buffModel)
		{
			BattleLifeModel battleModel = m_BattleModel;
			battleModel.ShiFaYanChang = (double)battleModel.ShiFaYanChang + (double)(1f * _buffModel.Num);
		}

		protected virtual void RemoveShiFangYanChangEffect(BaseBuffModel _buffModel)
		{
			BattleLifeModel battleModel = m_BattleModel;
			battleModel.ShiFaYanChang = (double)battleModel.ShiFaYanChang - (double)(1f * _buffModel.Num);
		}

		protected virtual void DingShenEffect()
		{
			CancleShiFa();
			DingShen();
		}

		protected virtual void RemoveDingShenEffect(BaseBuffModel _buffModel)
		{
			Idle();
		}

		protected virtual void DiKangDamage()
		{
			BattleModel.DiKangDamage = true;
		}

		protected virtual void CancleDiKangDamage()
		{
			foreach (KeyValuePair<string, BaseBuffModel> buff in BuffList)
			{
				if (buff.Value.BuffType == BattleBuffType.DiDang)
				{
					BattleModel.DiKangDamage = true;
					break;
				}
			}
			BattleModel.DiKangDamage = false;
		}

		protected virtual void DamageAddEffect(BaseBuffModel _buffModel)
		{
			BattleLifeModel battleModel = m_BattleModel;
			battleModel.DamageAdd = (double)battleModel.DamageAdd + (double)_buffModel.Num;
		}

		protected virtual void RemoveDamageAddEffect(BaseBuffModel _buffModel)
		{
			BattleLifeModel battleModel = m_BattleModel;
			battleModel.DamageAdd = (double)battleModel.DamageAdd - (double)_buffModel.Num;
			if ((double)m_BattleModel.DamageAdd < 0.0)
			{
				m_BattleModel.DamageAdd = 0.0;
			}
		}

		protected virtual void DefenceAddEffect(BaseBuffModel _buffModel)
		{
			BattleLifeModel battleModel = m_BattleModel;
			battleModel.DefeneceAdd = (double)battleModel.DefeneceAdd + (double)_buffModel.Num;
			if ((double)m_BattleModel.DefeneceAdd < 0.0)
			{
				m_BattleModel.DefeneceAdd = 0.0;
			}
		}

		protected virtual void RemoveDefenceAddEffect(BaseBuffModel _buffModel)
		{
			BattleLifeModel battleModel = m_BattleModel;
			battleModel.DefeneceAdd = (double)battleModel.DefeneceAdd - (double)_buffModel.Num;
		}

		protected virtual void HuDunAddEffect(BaseBuffModel _buffModel)
		{
			if ((double)_buffModel.Num > (double)m_BattleModel.CurrentHuDun)
			{
				m_BattleModel.CurrentHuDun = _buffModel.Num;
			}
		}

		protected virtual void RemoveHuDunAddEffect(string Id)
		{
			m_BattleModel.CurrentHuDun = 0.0;
			m_BattleModel.HuDunFiveElement = 0;
			BaseController.CancleHuDun();
		}

		public virtual void ReComputeMoveSpeed()
		{
			double num = BattleModel.InitMoveSpeed;
			foreach (KeyValuePair<string, BaseBuffModel> buff in BuffList)
			{
				if (buff.Value.BuffType == BattleBuffType.DingShen)
				{
					num = 0.0;
					break;
				}
				if (buff.Value.BuffType == BattleBuffType.JianSu)
				{
					num -= num * (double)buff.Value.Num;
				}
			}
			BattleModel.CurrentMoveSpeed = num;
		}

		protected virtual void ShenShiLockEnterTriggerCheck(Collider2D _collider2d)
		{
		}

		protected virtual void ShenShiFoundEnterTriggerCheck(Collider2D _collider2d)
		{
		}

		protected virtual void ShenShiLockExitTriggerCheck(Collider2D _collider2d)
		{
		}

		protected virtual void ShenShiFoundExitTriggerCheck(Collider2D _collider2d)
		{
		}

		public virtual void ShiFa(float time, UnityAction CompeleteAction)
		{
			if (CurrentAniState == AniState.Attack || CurrentAniState == AniState.Die || CurrentAniState == AniState.DingShen || CurrentAniState == AniState.GoBack)
			{
				return;
			}
			Attack();
			time *= (float)(double)m_BattleModel.ShiFaYanChang;
			BaseController.ShiFangSkill(time);
			m_ShiFaTimer = Timer.Register(time, () =>
			{
				if (CurrentAniState == AniState.Die)
				{
					m_ShiFaTimer = null;
				}
				else
				{
					m_ShiFaTimer = null;
					Idle();
					if (CompeleteAction != null)
					{
						CompeleteAction();
					}
				}
			});
		}

		protected virtual void CancleShiFa()
		{
			if (m_ShiFaTimer != null)
			{
				m_ShiFaTimer?.Cancel();
				BaseController.CancleShiFangSkill();
				m_ShiFaTimer = null;
				Idle();
			}
		}

		public virtual void BeiDingShen(float _time, int _skillLevel, int fiveElement, int icon, string _skillId)
		{
			if (CurrentAniState == AniState.GoBack)
			{
				BaseController.ShowStateText("抵抗");
				return;
			}
			if (WorldSetting.GetIsSuccess((float)(m_BattleModel.Level - _skillLevel) * 0.2f * 100f))
			{
				BaseController.ShowStateText("抵抗");
				return;
			}
			BaseController.ShowStateText("定身");
			BaseBuffModel baseBuffModel = new BaseBuffModel();
			baseBuffModel.BuffId = _skillId;
			baseBuffModel.BuffType = BattleBuffType.DingShen;
			baseBuffModel.FiveElement = fiveElement;
			baseBuffModel.Icon = icon;
			baseBuffModel.Num = 0f;
			baseBuffModel.Time = _time;
			AddBuff(baseBuffModel);
		}

		public virtual void ShiFaYanChang(float _time, float _value, int _skillLevel, int fiveElement, int icon, string _skillId)
		{
			if (CurrentAniState == AniState.GoBack)
			{
				BaseController.ShowStateText("抵抗");
				return;
			}
			if (WorldSetting.GetIsSuccess((float)(m_BattleModel.Level - _skillLevel) * 0.2f * 100f))
			{
				BaseController.ShowStateText("抵抗");
				return;
			}
			BaseBuffModel baseBuffModel = new BaseBuffModel();
			baseBuffModel.BuffId = _skillId;
			baseBuffModel.BuffType = BattleBuffType.ShiFaYanChang;
			baseBuffModel.FiveElement = fiveElement;
			baseBuffModel.Icon = icon;
			baseBuffModel.Num = _value;
			baseBuffModel.Time = _time;
			AddBuff(baseBuffModel);
		}

		public virtual void JianShaoMoveSpeed(float _time, float _value, int _skillLevel, int fiveElement, int icon, string _skillId)
		{
			if (CurrentAniState == AniState.GoBack)
			{
				BaseController.ShowStateText("抵抗");
				return;
			}
			if (WorldSetting.GetIsSuccess((float)(m_BattleModel.Level - _skillLevel) * 0.2f * 100f))
			{
				BaseController.ShowStateText("抵抗");
				return;
			}
			BaseController.ShowStateText("减速");
			BaseBuffModel baseBuffModel = new BaseBuffModel();
			baseBuffModel.BuffId = _skillId;
			baseBuffModel.BuffType = BattleBuffType.JianSu;
			baseBuffModel.FiveElement = fiveElement;
			baseBuffModel.Icon = icon;
			baseBuffModel.Num = _value;
			baseBuffModel.Time = _time;
			AddBuff(baseBuffModel);
		}

		public virtual void AddHuDun(float _value, int fiveElement, int icon, string _skillId)
		{
			BaseBuffModel baseBuffModel = new BaseBuffModel();
			baseBuffModel.BuffId = _skillId;
			baseBuffModel.BuffType = BattleBuffType.HuDun;
			baseBuffModel.FiveElement = fiveElement;
			baseBuffModel.Icon = icon;
			baseBuffModel.Num = _value;
			AddBuff(baseBuffModel);
		}

		public virtual void AddWuXingDamage(float _time, float _value, int fiveElement, int icon, string _skillId)
		{
			BaseBuffModel baseBuffModel = new BaseBuffModel();
			baseBuffModel.BuffId = _skillId;
			baseBuffModel.BuffType = BattleBuffType.DamageAdd;
			baseBuffModel.FiveElement = fiveElement;
			baseBuffModel.Icon = icon;
			baseBuffModel.Num = _value;
			baseBuffModel.Time = _time;
			AddBuff(baseBuffModel);
		}

		public virtual void AddWuXingDefence(float _time, float _value, int fiveElement, int icon, string _skillId)
		{
			BaseBuffModel baseBuffModel = new BaseBuffModel();
			baseBuffModel.BuffId = _skillId;
			baseBuffModel.BuffType = BattleBuffType.DefenseAdd;
			baseBuffModel.FiveElement = fiveElement;
			baseBuffModel.Icon = icon;
			baseBuffModel.Num = _value;
			baseBuffModel.Time = _time;
			AddBuff(baseBuffModel);
		}

		public virtual void AddDiKangDamage(float _time, string _skillId, int icon)
		{
			BaseBuffModel baseBuffModel = new BaseBuffModel();
			baseBuffModel.BuffId = _skillId;
			baseBuffModel.BuffType = BattleBuffType.DiDang;
			baseBuffModel.Icon = icon;
			baseBuffModel.Time = _time;
			AddBuff(baseBuffModel);
		}

		public virtual void EnterZhenFa(BattleZhenFaType battleZhenFaType, BaseSkillModel baseSkillModel, ZhenFa_Item zhenFa_Item)
		{
			if (!m_ZhenFaList.ContainsKey(baseSkillModel.id) && !m_ZhenFaItemList.ContainsKey(baseSkillModel.id))
			{
				m_ZhenFaList.Add(baseSkillModel.id, baseSkillModel);
				m_ZhenFaItemList.Add(baseSkillModel.id, zhenFa_Item);
			}
		}

		public virtual void ExitZhenFa(BattleZhenFaType battleZhenFaType, string skillId)
		{
			if (m_ZhenFaList.ContainsKey(skillId) && m_ZhenFaItemList.ContainsKey(skillId))
			{
				m_ZhenFaList.Remove(skillId);
				m_ZhenFaItemList.Remove(skillId);
			}
		}

		public BaseSkillModel CheckIfInMatrix(BattleZhenFaType battleZhenFaType)
		{
			foreach (KeyValuePair<string, BaseSkillModel> zhenFa in m_ZhenFaList)
			{
				if (zhenFa.Value.type == 4 && zhenFa.Value.skillType == (int)battleZhenFaType)
				{
					return zhenFa.Value;
				}
			}
			return null;
		}

		public virtual void ClearAllBuff()
		{
			if (!m_CanleDotDamage)
			{
				m_CanleDotDamage = true;
			}
			Timer.Register(1.5f, () =>
			{
				m_CanleDotDamage = false;
			});
			List<string> list = new List<string>();
			foreach (KeyValuePair<string, BaseBuffModel> buff in BuffList)
			{
				list.Add(buff.Key);
			}
			foreach (string item in list)
			{
				RemoveBuff(item);
			}
		}

		public virtual void ClearFoundAndLockMonster()
		{
			Target = null;
			LockAreaObjDic?.Clear();
			FoundAreaObjDic?.Clear();
		}
	}
}
