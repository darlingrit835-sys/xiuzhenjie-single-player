using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace XiuZhenJie
{
	public class AoeSkill_Item : MonoBehaviour
	{
		private Transform m_YuanArea;

		private TriggerCheck m_TriggerCheck;

		private double m_Damage;

		private int m_FiveElement;

		private float m_MaxDurationTime;

		private float m_DurationTime;

		private float m_AttackIntervalTime;

		private float m_CurrentAttackIntervalTime;

		private bool m_isStart;

		private EntityType m_TargetType;

		private EntityBase m_SelfEntity;

		private Dictionary<string, Controller> enemyDic;

		private int m_SkillIndex;

		private Timer m_TimerToAttack;

		private void Awake()
		{
			enemyDic = new Dictionary<string, Controller>();
			m_YuanArea = base.transform.Find("YuanArea");
			m_TriggerCheck = base.transform.Find("CircleCollider").gameObject.GetComponent<TriggerCheck>();
			m_isStart = false;
			TriggerCheck triggerCheck = m_TriggerCheck;
			triggerCheck.TriggerEnterAction = (UnityAction<Collider2D>)Delegate.Combine(triggerCheck.TriggerEnterAction, new UnityAction<Collider2D>(TriggerEnter));
			TriggerCheck triggerCheck2 = m_TriggerCheck;
			triggerCheck2.TriggerExitAction = (UnityAction<Collider2D>)Delegate.Combine(triggerCheck2.TriggerExitAction, new UnityAction<Collider2D>(TriggerExit));
		}

		private void Update()
		{
			if (m_isStart && m_DurationTime > 0f)
			{
				m_DurationTime -= Time.deltaTime;
				m_CurrentAttackIntervalTime -= Time.deltaTime;
				if (m_CurrentAttackIntervalTime <= 0f)
				{
					Attack();
					m_CurrentAttackIntervalTime = m_AttackIntervalTime;
				}
			}
			else if (m_isStart && m_DurationTime <= 0f)
			{
				m_isStart = false;
				UnityEngine.Object.Destroy(base.gameObject);
			}
		}

		private void OnDisable()
		{
			TriggerCheck triggerCheck = m_TriggerCheck;
			triggerCheck.TriggerEnterAction = (UnityAction<Collider2D>)Delegate.Remove(triggerCheck.TriggerEnterAction, new UnityAction<Collider2D>(TriggerEnter));
			TriggerCheck triggerCheck2 = m_TriggerCheck;
			triggerCheck2.TriggerExitAction = (UnityAction<Collider2D>)Delegate.Remove(triggerCheck2.TriggerExitAction, new UnityAction<Collider2D>(TriggerExit));
		}

		public void Init(int skillIndex, EntityBase _selfEntity, double _damage, int _fiveElement, float _durationTime, float _attackIntervalTime, EntityType _targetType, Dictionary<WuXing, GameObject> AoeInsObjDic)
		{
			m_SkillIndex = skillIndex;
			m_SelfEntity = _selfEntity;
			m_Damage = _damage;
			m_FiveElement = _fiveElement;
			m_DurationTime = _durationTime;
			m_MaxDurationTime = _durationTime;
			m_AttackIntervalTime = _attackIntervalTime;
			m_CurrentAttackIntervalTime = 0f;
			m_TargetType = _targetType;
			GameObject insObj = null;
			if (AoeInsObjDic.TryGetValue((WuXing)_fiveElement, out insObj))
			{
				m_YuanArea.gameObject.SetActive(value: true);
				m_TimerToAttack = Timer.Register(3f, () =>
				{
					UnityEngine.Object.Instantiate(insObj, base.transform);
					m_YuanArea.gameObject.SetActive(value: false);
					m_isStart = true;
				});
			}
		}

		public void Cancle()
		{
			if (!m_isStart && m_TimerToAttack != null)
			{
				m_TimerToAttack.Cancel();
				UnityEngine.Object.Destroy(base.gameObject);
			}
		}

		private void TriggerEnter(Collider2D collider2D)
		{
			GameObject gameObject = collider2D.gameObject;
			if (!(gameObject.tag != "monster") || !(gameObject.tag != "Player"))
			{
				Controller componentInParent = gameObject.GetComponentInParent<Controller>();
				if (componentInParent != null && componentInParent.EntityDataSource != null &&
					componentInParent.EntityDataSource.Entity_Type == m_TargetType && !enemyDic.ContainsKey(componentInParent.Id))
				{
					enemyDic.Add(componentInParent.Id, componentInParent);
				}
			}
		}

		private void TriggerExit(Collider2D collider2D)
		{
			Controller componentInParent = collider2D.gameObject.GetComponentInParent<Controller>();
			if (componentInParent != null && enemyDic.ContainsKey(componentInParent.Id))
			{
				enemyDic.Remove(componentInParent.Id);
			}
		}

		private void Attack()
		{
			foreach (KeyValuePair<string, Controller> item in enemyDic)
			{
				float num = m_MaxDurationTime / m_AttackIntervalTime;
				double damage = m_Damage / (double)num * 0.75;
				item.Value.UnderAttack(m_SkillIndex, m_SelfEntity, damage, m_FiveElement);
			}
		}
	}
}
