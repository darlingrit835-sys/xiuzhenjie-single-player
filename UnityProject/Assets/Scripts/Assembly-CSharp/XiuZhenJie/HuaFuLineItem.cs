using UnityEngine;
using UnityEngine.Events;

namespace XiuZhenJie
{
	public class HuaFuLineItem : MonoBehaviour
	{
		private LineRenderer m_LineRender;

		private TextMesh m_TextMesh;

		public Vector3 endMarker;

		public float speed = 0.001f;

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
			m_LineRender = base.gameObject.GetComponent<LineRenderer>();
			m_TextMesh = base.transform.Find("Text").gameObject.GetComponent<TextMesh>();
		}

		public void SetPos(Vector3 selfPos, int index, bool isEnd = false)
		{
			base.transform.position = selfPos;
			if (isEnd)
			{
				m_IsEnd = true;
				m_TextMesh.text = index.ToString();
			}
			else
			{
				m_Index = index;
				m_TextMesh.text = m_Index.ToString();
			}
		}

		public void SetLine(int index, Vector3 selfPos, Vector3 TargetPoint, UnityAction<int> unityAction, bool isEnd = false)
		{
			base.transform.position = selfPos;
			if (isEnd)
			{
				m_IsEnd = true;
				m_TextMesh.text = (index + 1).ToString();
				return;
			}
			m_Index = index;
			m_TextMesh.text = (m_Index + 1).ToString();
			m_IsEnd = false;
			compeleteCallBack = unityAction;
			endMarker = TargetPoint;
			m_LineRender.SetPosition(0, new Vector3(selfPos.x, selfPos.y, selfPos.z + 0.1f));
			m_LineRender.SetPosition(1, new Vector3(selfPos.x, selfPos.y, selfPos.z + 0.1f));
			journeyLength = Vector3.Distance(base.transform.position, endMarker);
		}

		public void StartLine()
		{
			startTime = Time.fixedTime;
			m_IsStart = true;
			speed = Random.Range(0.5f, 2f);
		}

		public void End()
		{
			m_IsEnd = true;
		}

		private void FixedUpdate()
		{
			if (!m_IsStart || m_IsEnd)
			{
				return;
			}
			float t = (Time.fixedTime - startTime) * speed / journeyLength;
			Vector3 a = Vector3.Lerp(base.transform.position, endMarker, t);
			m_LineRender.SetPosition(1, new Vector3(a.x, a.y, a.z + 0.1f));
			if (Vector3.Distance(a, endMarker) < 0.1f)
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
