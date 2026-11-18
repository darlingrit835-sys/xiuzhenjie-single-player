using UnityEngine;
using UnityEngine.Events;

namespace XiuZhenJie
{
	public class ChuangZhaoGongFaLightItem : MonoBehaviour
	{
		public LineRenderer Line;

		public Vector3 endMarker;

		public float speed = 0.05f;

		private float startTime;

		private float journeyLength;

		public Transform target;

		public float smooth = 5f;

		private bool m_IsStart;

		private bool m_IsEnd;

		private UnityAction<int> compeleteCallBack;

		private SpriteRenderer m_SpriteRender;

		private int m_Index;

		private bool m_IsAddValue;

		private void Awake()
		{
			m_SpriteRender = base.gameObject.GetComponent<SpriteRenderer>();
		}

		public void SetLine(int index, Vector3 TargetPoint, UnityAction<int> unityAction, bool isEnd = false)
		{
			if (isEnd)
			{
				m_IsEnd = true;
				return;
			}
			m_Index = index;
			m_IsEnd = false;
			compeleteCallBack = unityAction;
			endMarker = TargetPoint;
			Line.SetPosition(0, base.transform.position);
			Line.SetPosition(1, base.transform.position);
			journeyLength = Vector3.Distance(base.transform.position, endMarker);
		}

		public void StartLine()
		{
			startTime = Time.fixedTime;
			m_IsStart = true;
		}

		private void Flashing()
		{
			float fixedDeltaTime = Time.fixedDeltaTime;
			if (m_SpriteRender.color.r >= 1f)
			{
				m_IsAddValue = false;
			}
			else if (m_SpriteRender.color.r <= 0.7f)
			{
				m_IsAddValue = true;
			}
			if (m_IsAddValue)
			{
				float num = m_SpriteRender.color.r + fixedDeltaTime;
				m_SpriteRender.color = new Color(num, num, num, 1f);
			}
			else
			{
				float num2 = m_SpriteRender.color.r - fixedDeltaTime;
				m_SpriteRender.color = new Color(num2, num2, num2, 1f);
			}
		}

		private void FixedUpdate()
		{
			if (!m_IsStart)
			{
				return;
			}
			Flashing();
			if (m_IsEnd)
			{
				return;
			}
			float t = (Time.fixedTime - startTime) * speed / journeyLength;
			Vector3 vector = Vector3.Lerp(base.transform.position, endMarker, t);
			Line.SetPosition(1, vector);
			if (Vector3.Distance(vector, endMarker) < 0.02f)
			{
				m_IsEnd = true;
				if (compeleteCallBack != null)
				{
					compeleteCallBack(m_Index);
				}
			}
		}
	}
}
