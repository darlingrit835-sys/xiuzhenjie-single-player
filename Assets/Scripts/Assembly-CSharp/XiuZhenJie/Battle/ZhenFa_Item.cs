using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace XiuZhenJie
{
	public class ZhenFa_Item : MonoBehaviour
	{
		private Transform m_YuanArea;

		private Transform m_YuanAreaEffectImg;

		private TriggerCheck m_TriggerCheck;

		private int m_ZhenFaType;

		private int m_FiveElement;

		private float m_MaxDurationTime;

		private float m_DurationTime;

		private float m_KillMatrxTime;

		private bool m_selfIsInMatrx;

		private EntityBase m_SelfEntity;

		private Dictionary<string, Controller> enemyDic;

		private int m_SkillIndex;

		private BattleZhenFaType m_BattleZhenFaType;

		private CircleCollider2D m_CircleCollider;

		private Vector2 m_CenterPoint;

		private float m_RadiusValue;

		private BaseSkillModel m_SkillMode;

		private BattleZhenFaType m_CurrentZhenFaType;

		private float m_JianSuCoefficient = 0.15f;

		private float m_JianSuDegrees = 1.15f;

		private float m_RangeCoefficient = 36f;

		private float m_RangeDegrees = 1.25f;

		private float m_DamageCoefficient = 1f;

		private float m_DamageDegrees = 1.4f;

		private float m_RecoverCoefficient = 1.1f;

		private float m_RecoverDegrees = 1.08f;

		private float m_RotateSpeed = 5f;

		public Vector2 CenterPoint
		{
			get
			{
				return m_CenterPoint;
			}
			private set
			{
				m_CenterPoint = value;
			}
		}

		public float RadiusValue
		{
			get
			{
				return m_RadiusValue;
			}
			private set
			{
				m_RadiusValue = value;
			}
		}

		public BattleZhenFaType CurrentZhenFaType
		{
			get
			{
				return m_CurrentZhenFaType;
			}
			private set
			{
				m_CurrentZhenFaType = value;
			}
		}

		private void Awake()
		{
			enemyDic = new Dictionary<string, Controller>();
			m_YuanArea = base.transform.Find("YuanArea");
			m_TriggerCheck = base.transform.Find("YuanArea").gameObject.GetComponent<TriggerCheck>();
			m_YuanAreaEffectImg = base.transform.Find("Image");
			m_KillMatrxTime = 0f;
			TriggerCheck triggerCheck = m_TriggerCheck;
			triggerCheck.TriggerEnterAction = (UnityAction<Collider2D>)Delegate.Combine(triggerCheck.TriggerEnterAction, new UnityAction<Collider2D>(TriggerEnter));
			TriggerCheck triggerCheck2 = m_TriggerCheck;
			triggerCheck2.TriggerExitAction = (UnityAction<Collider2D>)Delegate.Combine(triggerCheck2.TriggerExitAction, new UnityAction<Collider2D>(TriggerExit));
			TriggerCheck triggerCheck3 = m_TriggerCheck;
			triggerCheck3.TriggerStayAction = (UnityAction<Collider2D>)Delegate.Combine(triggerCheck3.TriggerStayAction, new UnityAction<Collider2D>(TriggerStay));
		}

		private void FixedUpdate()
		{
			if (m_DurationTime > 0f)
			{
				m_DurationTime -= Time.fixedDeltaTime;
			}
			else if (m_DurationTime <= 0f && m_DurationTime > -1f)
			{
				m_DurationTime = -10f;
				if (m_selfIsInMatrx)
				{
					m_SelfEntity.ExitZhenFa(CurrentZhenFaType, m_SkillMode.id);
				}
				if (enemyDic.Count > 0)
				{
					foreach (KeyValuePair<string, Controller> item in enemyDic)
					{
						item.Value.ExitZhenFa(CurrentZhenFaType, m_SkillMode.id);
					}
				}
				UnityEngine.Object.Destroy(base.gameObject);
			}
			MatrxEffect();
			if (m_YuanAreaEffectImg != null)
			{
				m_YuanAreaEffectImg.Rotate(new Vector3(0f, 0f, Time.fixedDeltaTime * m_RotateSpeed), Space.Self);
			}
		}

		private void MatrxEffect()
		{
			m_KillMatrxTime += Time.fixedDeltaTime;
			if (!(m_KillMatrxTime >= 1f))
			{
				return;
			}
			m_KillMatrxTime = 0f;
			if (CurrentZhenFaType == BattleZhenFaType.Kill)
			{
				foreach (KeyValuePair<string, Controller> item in enemyDic)
				{
					if (!item.Value.IsDie)
					{
						item.Value.UnderAttack(m_SkillMode.SkillIndex, m_SelfEntity, m_SkillMode.baseDamage, m_SkillMode.fiveElement);
					}
				}
				return;
			}
			if (CurrentZhenFaType == BattleZhenFaType.AddSuperAir)
			{
				if (m_selfIsInMatrx)
				{
					m_SelfEntity.RecoverSuperAirValue((float)(double)m_SkillMode.baseDamage);
				}
			}
			else if (CurrentZhenFaType == BattleZhenFaType.AddTiPo && m_selfIsInMatrx)
			{
				m_SelfEntity.RecoverJianRenValue((float)(double)m_SkillMode.baseDamage);
			}
		}

		public void Init(BattleZhenFaType battleZhenFaType, EntityBase _selfEntity, BaseSkillModel baseSkillModel, int mapStage)
		{
			InitZhenFaValue(baseSkillModel, mapStage);
			m_MaxDurationTime = (float)(double)baseSkillModel.continued;
			m_DurationTime = m_MaxDurationTime;
			m_SkillMode = baseSkillModel;
			m_CurrentZhenFaType = battleZhenFaType;
			m_SkillIndex = baseSkillModel.SkillIndex;
			m_SelfEntity = _selfEntity;
			m_FiveElement = baseSkillModel.fiveElement;
			m_BattleZhenFaType = (BattleZhenFaType)baseSkillModel.skillType;
			m_CircleCollider = base.transform.Find("YuanArea").gameObject.GetComponent<CircleCollider2D>();
			RadiusValue = m_CircleCollider.radius * base.transform.Find("YuanArea").localScale.x;
			CenterPoint = base.transform.position;
			m_selfIsInMatrx = true;
		}

		private void InitZhenFaValue(BaseSkillModel baseSkillModel, int mapStage)
		{
			int num = WorldSetting.GetStageByLevel(baseSkillModel.level) - mapStage;
			float num2 = Mathf.Pow(m_RangeDegrees, num) * m_RangeCoefficient * (float)baseSkillModel.conversionRate;
			if (baseSkillModel.skillType == 2)
			{
				float num3 = m_JianSuCoefficient * Mathf.Pow(m_JianSuDegrees, num) * (float)baseSkillModel.conversionRate;
				baseSkillModel.baseDamage = num3;
				num2 /= 3f;
			}
			else if (baseSkillModel.skillType == 3)
			{
				float num4 = m_JianSuCoefficient * Mathf.Pow(m_JianSuDegrees, num) * (float)baseSkillModel.conversionRate;
				baseSkillModel.baseDamage = num4;
			}
			else if (baseSkillModel.skillType == 1)
			{
				float num5 = m_DamageCoefficient * Mathf.Pow(m_DamageDegrees, num) * (float)baseSkillModel.conversionRate;
				baseSkillModel.baseDamage = num5;
			}
			else if (baseSkillModel.skillType == 4 || baseSkillModel.skillType == 5)
			{
				float num6 = m_RecoverCoefficient * Mathf.Pow(m_RecoverDegrees, WorldSetting.GetStageByLevel(baseSkillModel.level)) * (float)baseSkillModel.conversionRate;
				baseSkillModel.baseDamage = num6;
			}
			base.transform.Find("YuanArea").localScale = new Vector2(num2, num2);
			m_YuanAreaEffectImg.localScale = new Vector2(num2, num2);
		}

		private void TriggerEnter(Collider2D collider2D)
		{
			GameObject gameObject = collider2D.gameObject;
			if (gameObject.tag != "monster" && gameObject.tag != "Player")
			{
				return;
			}
			Controller componentInParent = gameObject.GetComponentInParent<Controller>();
			CircleCollider2D circleCollider2D = collider2D as CircleCollider2D;
			if (componentInParent != null && IsEnterFaZhen(circleCollider2D.radius, circleCollider2D.gameObject.transform.position))
			{
				Debug.Log("enter" + collider2D.tag);
				if (gameObject.tag == "monster" && !enemyDic.ContainsKey(componentInParent.Id))
				{
					enemyDic.Add(componentInParent.Id, componentInParent);
				}
				if (gameObject.tag == "Player")
				{
					m_selfIsInMatrx = true;
				}
				componentInParent.EnterZhenFa(m_CurrentZhenFaType, m_SkillMode, this);
			}
		}

		private void TriggerStay(Collider2D collision)
		{
			GameObject gameObject = collision.gameObject;
			if (gameObject.tag != "monster" && gameObject.tag != "Player")
			{
				return;
			}
			Controller componentInParent = gameObject.GetComponentInParent<Controller>();
			CircleCollider2D circleCollider2D = collision as CircleCollider2D;
			if (componentInParent != null && IsEnterFaZhen(circleCollider2D.radius, circleCollider2D.gameObject.transform.position))
			{
				Debug.Log("stay:" + collision.tag);
				if (gameObject.tag == "monster" && !enemyDic.ContainsKey(componentInParent.Id))
				{
					enemyDic.Add(componentInParent.Id, componentInParent);
				}
				if (gameObject.tag == "Player")
				{
					m_selfIsInMatrx = true;
				}
				componentInParent.EnterZhenFa(m_CurrentZhenFaType, m_SkillMode, this);
			}
		}

		private void TriggerExit(Collider2D collider2D)
		{
			GameObject gameObject = collider2D.gameObject;
			Controller componentInParent = gameObject.GetComponentInParent<Controller>();
			if ((!(gameObject.tag != "monster") || !(gameObject.tag != "Player")) && componentInParent != null)
			{
				if (gameObject.tag == "monster" && enemyDic.ContainsKey(componentInParent.Id))
				{
					enemyDic.Remove(componentInParent.Id);
				}
				if (gameObject.tag == "Player")
				{
					m_selfIsInMatrx = false;
				}
				componentInParent.ExitZhenFa(m_CurrentZhenFaType, m_SkillMode.id);
			}
		}

		private bool IsEnterFaZhen(float targetRadius, Vector2 targetOffset)
		{
			if (Vector2.Distance(CenterPoint, targetOffset) <= Mathf.Abs(RadiusValue - targetRadius))
			{
				return true;
			}
			return false;
		}

		private void OnDisable()
		{
			TriggerCheck triggerCheck = m_TriggerCheck;
			triggerCheck.TriggerEnterAction = (UnityAction<Collider2D>)Delegate.Remove(triggerCheck.TriggerEnterAction, new UnityAction<Collider2D>(TriggerEnter));
			TriggerCheck triggerCheck2 = m_TriggerCheck;
			triggerCheck2.TriggerExitAction = (UnityAction<Collider2D>)Delegate.Remove(triggerCheck2.TriggerExitAction, new UnityAction<Collider2D>(TriggerExit));
			TriggerCheck triggerCheck3 = m_TriggerCheck;
			triggerCheck3.TriggerStayAction = (UnityAction<Collider2D>)Delegate.Remove(triggerCheck3.TriggerStayAction, new UnityAction<Collider2D>(TriggerStay));
		}
	}
}
