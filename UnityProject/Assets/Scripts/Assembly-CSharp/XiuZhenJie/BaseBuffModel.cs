using System.Collections;
using UnityEngine;
using UnityEngine.Events;

namespace XiuZhenJie
{
	public class BaseBuffModel
	{
		public string BuffId;

		public BattleBuffType BuffType;

		public float Time;

		private float ShengYuTime;

		public float Num;

		public int FiveElement;

		public int Icon;

		private Timer TimerMgr;

		private UnityAction CompeleteAction;

		private MonoBehaviour Mono;

		private bool m_isCancle;

		public virtual void DoBuff(MonoBehaviour _mono, UnityAction _compeleteAction)
		{
			Mono = _mono;
			CompeleteAction = _compeleteAction;
			ShengYuTime = Time;
			if (BuffType != BattleBuffType.HuDun && Time != -1f)
			{
				Mono.StartCoroutine(TimerMthod());
			}
		}

		public void CancleBuff()
		{
			m_isCancle = true;
		}

		public virtual void ReflashTime()
		{
			ShengYuTime = Time;
		}

		public float GetShengYuTime()
		{
			return ShengYuTime;
		}

		private IEnumerator TimerMthod()
		{
			while (!m_isCancle)
			{
				ShengYuTime -= UnityEngine.Time.deltaTime;
				if (ShengYuTime <= 0f)
				{
					break;
				}
				yield return new WaitForEndOfFrame();
			}
			if (CompeleteAction != null)
			{
				CompeleteAction();
			}
		}
	}
}
