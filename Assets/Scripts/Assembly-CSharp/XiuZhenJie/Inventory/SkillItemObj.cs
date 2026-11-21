using UnityEngine;

namespace XiuZhenJie
{
	public class SkillItemObj : BaseItemObj
	{
		private bool PointIsSend;

		private float SendSpeed = 5f;

		private BaseSkillModel m_SkillDataSource;

		private void Update()
		{
			if (PointIsSend && m_TargetObj != null)
			{
				base.transform.position = Vector3.MoveTowards(base.transform.position, m_TargetObj.transform.position, Time.deltaTime * SendSpeed);
				if (Vector3.Distance(base.transform.position, m_TargetObj.transform.position) < 0.001f)
				{
					PointIsSend = false;
				}
			}
		}

		private void StartActivity(BaseSkillModel _baseSkillModel, params object[] args)
		{
			switch (base.AttackMoudle)
			{
			case AttackMoudleType.PointTo:
			{
				float time = (float)(double)_baseSkillModel.castTime / 1000f;
				SendToPoint(time);
				break;
			}
			case AttackMoudleType.Range:
				_ = (float)(double)_baseSkillModel.castTime / 1000f;
				break;
			case AttackMoudleType.None:
			case AttackMoudleType.LongPointTo:
			case AttackMoudleType.LongRange:
				break;
			}
		}

		public void SendToPoint(float time)
		{
			if (time != 0f)
			{
				if (m_TargetObj != null)
				{
					base.transform.LookAt(m_TargetObj.transform);
					PointIsSend = true;
				}
			}
			else
			{
				ToPointSkillAction(time);
			}
		}

		private void ToPointSkillAction(float time)
		{
			Timer.Register(time, () =>
			{
				if (base.FromType == EntityType.Monster)
				{
					if (m_TargetObj.GetComponent<PlayerEntity>() == null)
					{
					}
				}
				else if (base.FromType == EntityType.Player)
				{
					m_TargetObj.GetComponent<MonsterEntity>();
				}
				if (base.AttackMoudle == AttackMoudleType.PointTo && m_TargetObj != null)
				{
					base.transform.LookAt(m_TargetObj.transform);
					PointIsSend = true;
				}
			});
		}

		private void RangeSkill(float time, float range, float angle)
		{
			if (time != 0f)
			{
				RangeSkillAction(range, angle);
				return;
			}
			Timer.Register(time, () =>
			{
				RangeSkillAction(range, angle);
			});
		}

		private void RangeSkillAction(float range, float angle)
		{
		}

		private bool InRange(Vector3 from, Vector3 to, float range)
		{
			if (Vector3.Distance(from, to) <= range)
			{
				return true;
			}
			return false;
		}

		private bool InAngle(Transform trans, Vector3 to, float angle)
		{
			if (angle == 360f)
			{
				return true;
			}
			Vector3 forward = trans.forward;
			Vector3 normalized = (to - trans.position).normalized;
			if (Vector3.Angle(forward, normalized) <= angle / 2f)
			{
				return true;
			}
			return false;
		}
	}
}
