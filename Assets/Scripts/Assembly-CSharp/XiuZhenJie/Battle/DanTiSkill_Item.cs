using UnityEngine;

namespace XiuZhenJie
{
	public class DanTiSkill_Item : MonoBehaviour
	{
		private GameObject m_AniObj;

		private int m_SkillIndex;

		private string m_target_Id;

		private EntityBase m_SelfEntity;

		private EntityBase m_EnemyEntity;

		private Controller m_EnemyController;

		private Transform m_EnemyTran;

		private Vector2 m_FangXiang;

		private int m_SpeedUp;

		private double m_Damage;

		private WuXing m_FiveElement;

		private bool m_IsEnd;

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

		public void Init(int skillIndex, EntityBase _selfEntity, double _damage, WuXing _wuXing, EntityBase _enemyEntity)
		{
			m_SkillIndex = skillIndex;
			m_SelfEntity = _selfEntity;
			m_Damage = _damage;
			m_FiveElement = _wuXing;
			Target_Id = _enemyEntity.BaseController.Id;
			m_EnemyController = _enemyEntity.BaseController;
			m_EnemyEntity = _enemyEntity;
			m_EnemyTran = m_EnemyController.gameObject.transform;
			m_IsEnd = false;
		}

		private void Awake()
		{
			m_SpeedUp = 6;
			m_IsEnd = true;
			m_AniObj = base.transform.Find("Ani_Obj").gameObject;
		}

		private void Update()
		{
			if (!m_IsEnd && m_EnemyTran != null)
			{
				Vector3 moveNormalized = m_EnemyTran.transform.position - base.transform.position;
				Vector3 vector = new Vector3(moveNormalized.x * Time.deltaTime * (float)m_SpeedUp + base.transform.position.x, moveNormalized.y * Time.fixedDeltaTime * (float)m_SpeedUp + base.transform.position.y, 0f);
				base.transform.position = new Vector3(vector.x, vector.y, base.transform.position.z);
				float angle = GetAngle(base.transform.position, m_EnemyTran.transform.position, moveNormalized);
				m_AniObj.transform.eulerAngles = new Vector3(0f, 0f, angle + 90f);
				if (Vector2.Distance(base.transform.position, m_EnemyTran.transform.position) <= 1f)
				{
					Attack();
					m_IsEnd = true;
				}
			}
		}

		private void Attack()
		{
			if (!m_EnemyController.IsDie)
			{
				m_EnemyController?.UnderAttack(m_SkillIndex, m_SelfEntity, m_Damage, (int)m_FiveElement);
			}
			Object.Destroy(base.gameObject);
		}

		private float GetAngle(Vector3 v1, Vector3 v2, Vector3 MoveNormalized)
		{
			Vector3 normalized = MoveNormalized.normalized;
			if (normalized.y < 0f)
			{
				return 0f - Vector2.Angle(Vector2.right, new Vector2(normalized.x, normalized.y));
			}
			return Vector2.Angle(Vector2.right, new Vector2(normalized.x, normalized.y));
		}
	}
}
