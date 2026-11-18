using System;
using System.Collections.Generic;
using CodeStage.AntiCheat.ObscuredTypes;
using DG.Tweening;
using DG.Tweening.Core;
using DG.Tweening.Plugins.Options;
using UnityEngine;

namespace XiuZhenJie
{
	public class WuQi_Item : MonoBehaviour
	{
		private string m_WeponId;

		private GameObject m_WuQi_RangeAni;

		private string m_target_Id;

		private EntityBase m_EnemyEntity;

		private Controller m_EnemyController;

		private Controller m_SelfController;

		private Transform m_EnemyTran;

		private Transform m_SelfTran;

		private EntityBase m_SelfEntity;

		private WuQiState m_CurrentState;

		private Vector2 m_FangXiang;

		private int m_SpeedUp;

		private bool m_IsTouched;

		private int m_Index;

		private ObscuredDouble m_Damage;

		private int m_FiveElement;

		private bool m_IsEnd;

		private double m_SuperConsume;

		private double m_PercentUsed;

		private float m_AttackIntervalTime;

		private int m_SkillIndex;

		public ObscuredDouble GoldDamageAdd = 1.0;

		public ObscuredDouble WoodDamageAdd = 1.0;

		public ObscuredDouble WaterDamageAdd = 1.0;

		public ObscuredDouble FireDamageAdd = 1.0;

		public ObscuredDouble EarthDamageAdd = 1.0;

		private float m_Timing;

		private Vector2 m_LockPos;

		public string WeponId
		{
			get
			{
				return m_WeponId;
			}
			private set
			{
				m_WeponId = value;
			}
		}

		public string Target_Id
		{
			get
			{
				return m_target_Id;
			}
			private set
			{
				m_target_Id = value;
			}
		}

		public WuQiState CurrentState
		{
			get
			{
				return m_CurrentState;
			}
			private set
			{
				m_CurrentState = value;
			}
		}

		public double PercentUsed
		{
			get
			{
				return m_PercentUsed;
			}
			private set
			{
				m_PercentUsed = value;
			}
		}

		private void Awake()
		{
			m_SpeedUp = 10;
			m_AttackIntervalTime = 1.5f;
			m_IsTouched = false;
			m_IsEnd = false;
			CurrentState = WuQiState.None;
		}

		public void Init(int skillIndex, EntityBase _selfEntity, double _superAirConsume, string id, Controller _selfController, double damage, WuXing _wuXing, float _attackIntervalTime, int index, Dictionary<WuXing, GameObject> faQiInsDic, double percentUsed = 0.0)
		{
			if (faQiInsDic != null)
			{
				m_SkillIndex = skillIndex;
				m_SelfEntity = _selfEntity;
				m_SuperConsume = _superAirConsume;
				WeponId = id;
				m_Index = index;
				m_SelfController = _selfController;
				m_AttackIntervalTime = _attackIntervalTime;
				m_Damage = damage;
				m_FiveElement = (int)_wuXing;
				m_WuQi_RangeAni = null;
				m_PercentUsed = percentUsed;
				if (faQiInsDic.TryGetValue(_wuXing, out m_WuQi_RangeAni))
				{
					GameObject gameObject = UnityEngine.Object.Instantiate(m_WuQi_RangeAni, base.transform);
					gameObject.transform.localScale = new Vector3(0.5f, 0.5f, 0.5f);
					gameObject.transform.localPosition = Vector3.zero;
					m_SelfTran = m_SelfController.gameObject.transform;
					m_WuQi_RangeAni = gameObject;
					CurrentState = WuQiState.Waiting;
				}
				else
				{
					UnityEngine.Object.Destroy(base.gameObject);
				}
			}
		}

		public void InitDamageAdd(double goldDamageAdd, double woodDamageAdd, double waterDamageAdd, double fireDamageAdd, double earthDamageAdd)
		{
			GoldDamageAdd = ((goldDamageAdd == 0.0) ? 1.0 : goldDamageAdd);
			WoodDamageAdd = ((woodDamageAdd == 0.0) ? 1.0 : woodDamageAdd);
			WaterDamageAdd = ((waterDamageAdd == 0.0) ? 1.0 : waterDamageAdd);
			FireDamageAdd = ((fireDamageAdd == 0.0) ? 1.0 : fireDamageAdd);
			EarthDamageAdd = ((earthDamageAdd == 0.0) ? 1.0 : earthDamageAdd);
		}

		public void SetTarget(EntityBase _enemyEntity)
		{
			Target_Id = _enemyEntity.BaseController.Id;
			m_EnemyController = _enemyEntity.BaseController;
			m_EnemyEntity = _enemyEntity;
			m_EnemyTran = m_EnemyController.transform;
			m_IsTouched = false;
			CurrentState = WuQiState.Preparation;
		}

		public void CancleTarget()
		{
			Target_Id = null;
			m_EnemyController = null;
			m_EnemyEntity = null;
			m_EnemyTran = null;
		}

		public void ReSetTarget(EntityBase _enemyEntity)
		{
			Target_Id = _enemyEntity.BaseController.Id;
			m_EnemyController = _enemyEntity.BaseController;
			m_EnemyEntity = _enemyEntity;
			m_EnemyTran = m_EnemyController.transform;
			CurrentState = WuQiState.Preparation;
		}

		public void Cancle(bool _isDestory = false)
		{
			if (m_SelfTran == null)
			{
				UnityEngine.Object.Destroy(base.gameObject);
				return;
			}
			if (!_isDestory && m_SelfTran != null)
			{
				float num = 2 * m_Index + 2;
				base.transform.position = new Vector3(m_SelfTran.position.x + num, m_SelfTran.position.y, m_SelfTran.position.z);
				CurrentState = WuQiState.Waiting;
			}
			else
			{
				CurrentState = WuQiState.None;
				base.gameObject.SetActive(value: false);
				Timer.Register(5f, () =>
				{
					if (!base.gameObject.Equals(null))
					{
						UnityEngine.Object.Destroy(base.gameObject);
					}
				});
			}
			Target_Id = string.Empty;
			m_EnemyController = null;
			m_EnemyTran = null;
		}

		private void Update()
		{
			if (!m_IsEnd)
			{
				if (m_SelfController.IsDie && !m_IsEnd)
				{
					m_IsEnd = true;
					Cancle(_isDestory: true);
				}
				if (string.IsNullOrEmpty(Target_Id) && m_SelfEntity.Target != null && CurrentState == WuQiState.Waiting)
				{
					ReSetTarget(m_SelfEntity.Target);
				}
				if (m_EnemyEntity == null && CurrentState != WuQiState.Waiting)
				{
					Cancle();
				}
				RotateWithPlayer();
				ItemRotate();
			}
		}

		private void FixedUpdate()
		{
			if (!m_IsEnd)
			{
				ItemFlying();
			}
		}

		private void ItemRotate()
		{
			if (CurrentState == WuQiState.Preparation && m_EnemyTran != null && (double)m_SelfEntity.BattleModel.CurrentSuperAir >= m_SuperConsume && !CheckTargeState())
			{
				Vector3 vector = m_EnemyTran.transform.position - base.transform.position;
				float angle = GetAngle(base.transform.position, m_EnemyTran.transform.position, vector);
				m_WuQi_RangeAni.transform.eulerAngles = new Vector3(0f, 0f, angle);
				m_Timing += Time.deltaTime;
				if (m_Timing >= m_AttackIntervalTime)
				{
					m_SelfEntity.XiaoHaoSuperAir(m_SuperConsume);
					CurrentState = WuQiState.Ready;
					m_FangXiang = vector;
					m_LockPos = m_EnemyTran.transform.position;
					m_Timing = 0f;
				}
			}
		}

		private void RotateWithPlayer()
		{
			if (CurrentState == WuQiState.Waiting && !(m_SelfTran == null))
			{
				base.transform.RotateAround(m_SelfTran.position, new Vector3(0f, 0f, 1f), 200f * Time.deltaTime);
				float angle = GetAngle(m_SelfTran.position, base.transform.position, base.transform.position - m_SelfTran.position);
				m_WuQi_RangeAni.transform.eulerAngles = new Vector3(0f, 0f, angle + 90f);
			}
		}

		private void ItemFlying()
		{
			if (CurrentState != WuQiState.Ready && CurrentState != WuQiState.Flying)
			{
				return;
			}
			_ = m_LockPos;
			if (CheckTargeState())
			{
				return;
			}
			if (m_EnemyTran == null)
			{
				Cancle();
				return;
			}
			Vector3 vector = new Vector3(m_FangXiang.x * Time.fixedDeltaTime * (float)m_SpeedUp + base.transform.position.x, m_FangXiang.y * Time.fixedDeltaTime * (float)m_SpeedUp + base.transform.position.y, 0f);
			base.transform.position = new Vector3(vector.x, vector.y, base.transform.position.z);
			if (!m_IsTouched && Vector2.Distance(base.transform.position, m_LockPos) <= 1f)
			{
				m_IsTouched = true;
				Attack();
			}
			float num = Vector2.Distance(base.transform.position, m_LockPos);
			if (!m_IsTouched || !(num > 10f))
			{
				return;
			}
			CurrentState = WuQiState.None;
			m_IsTouched = false;
			m_FangXiang = Vector2.zero;
			Vector3 moveNormalized = new Vector3(m_LockPos.x, m_LockPos.y) - base.transform.position;
			float angle = GetAngle(base.transform.position, m_LockPos, moveNormalized);
			TweenerCore<Quaternion, Vector3, QuaternionOptions> tweenerCore = m_WuQi_RangeAni.transform.DORotate(new Vector3(0f, 0f, angle), 1.5f);
			tweenerCore.onComplete = (TweenCallback)Delegate.Combine(tweenerCore.onComplete, (TweenCallback)(() =>
			{
				if (CurrentState == WuQiState.None)
				{
					CurrentState = WuQiState.Preparation;
				}
			}));
		}

		private void Attack()
		{
			double damage = m_Damage;
			switch ((WuXing)m_EnemyEntity.BattleModel.DefenseFiveElement)
			{
			case WuXing.Jin:
				damage = (double)m_Damage * (double)GoldDamageAdd;
				break;
			case WuXing.Mu:
				damage = (double)m_Damage * (double)WoodDamageAdd;
				break;
			case WuXing.Shui:
				damage = (double)m_Damage * (double)WaterDamageAdd;
				break;
			case WuXing.Huo:
				damage = (double)m_Damage * (double)FireDamageAdd;
				break;
			case WuXing.Tu:
				damage = (double)m_Damage * (double)EarthDamageAdd;
				break;
			}
			m_EnemyController.UnderAttack(m_SkillIndex, m_SelfEntity, damage, m_FiveElement);
		}

		private float GetAngle(Vector3 v1, Vector3 v2, Vector3 MoveNormalized)
		{
			if (MoveNormalized.normalized.y < 0f)
			{
				return 0f - Vector3.Angle(Vector3.right, MoveNormalized);
			}
			return Vector3.Angle(Vector3.right, MoveNormalized);
		}

		private bool CheckTargeState()
		{
			if (m_EnemyController != null && m_EnemyController.IsDie)
			{
				if (m_SelfTran != null)
				{
					Cancle();
				}
				return true;
			}
			return false;
		}
	}
}
