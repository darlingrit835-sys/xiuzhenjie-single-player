using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Events;

namespace XiuZhenJie
{
	public class CheckInput : MonoBehaviour, IPointerDownHandler, IEventSystemHandler, IPointerUpHandler, IPointerClickHandler
	{
		private Vector2 lastPos;

		private Vector2 currentPos;

		private float timer;

		public float offsetTime = 0.01f;

		private TouZhiType currentVector = TouZhiType.None;

		private bool m_MouseBtnInput;

		private float m_HuaDongDistance = 30f;

		private float m_AddTime;

		public UnityAction<PointerEventData> PointDownHandler;

		public UnityAction<PointerEventData> PointClickHandler;

		public UnityAction<PointerEventData> PointUpHandler;

		public UnityAction<TouZhiType> PointMoveHandler;

		private void Start()
		{
			m_MouseBtnInput = false;
			m_AddTime = Time.deltaTime;
		}

		private void Update()
		{
			CheckMouseInputPhase();
		}

		private void CheckInputPhase()
		{
			if (Input.touchCount != 1)
			{
				return;
			}
			if (Input.touches[0].phase == TouchPhase.Began)
			{
				lastPos = Input.touches[0].position;
				currentPos = Input.touches[0].position;
				timer = 0f;
			}
			if (Input.touches[0].phase == TouchPhase.Moved)
			{
				currentPos = Input.touches[0].position;
				timer += Time.deltaTime;
				if (timer > offsetTime)
				{
					if (lastPos.x - currentPos.x >= m_HuaDongDistance)
					{
						currentVector = TouZhiType.Left;
						if (PointMoveHandler != null)
						{
							PointMoveHandler(currentVector);
						}
					}
					else if (currentPos.x - lastPos.x >= m_HuaDongDistance)
					{
						currentVector = TouZhiType.Right;
						if (PointMoveHandler != null)
						{
							PointMoveHandler(currentVector);
						}
					}
					if (currentPos.y - lastPos.y >= m_HuaDongDistance)
					{
						currentVector = TouZhiType.Up;
						if (PointMoveHandler != null)
						{
							PointMoveHandler(currentVector);
						}
					}
					lastPos = currentPos;
					timer = 0f;
				}
			}
			if (Input.touches[0].phase == TouchPhase.Ended)
			{
				currentVector = TouZhiType.None;
			}
		}

		private void CheckMouseInputPhase()
		{
			if (Input.GetMouseButtonDown(0) && !m_MouseBtnInput)
			{
				lastPos = Input.mousePosition;
				currentPos = Input.mousePosition;
				timer = 0f;
				m_MouseBtnInput = true;
			}
			if (m_MouseBtnInput)
			{
				currentPos = Input.mousePosition;
				timer += ((Time.deltaTime == 0f) ? m_AddTime : Time.deltaTime);
				if (timer > offsetTime)
				{
					if (lastPos.x - currentPos.x >= m_HuaDongDistance)
					{
						currentVector = TouZhiType.Left;
						if (PointMoveHandler != null)
						{
							PointMoveHandler(currentVector);
						}
						if (Singleton<GuideManager>.Instance.YinDaoJilu1.YinDaoShu == 3)
						{
							LianDanYinDaoPanDuan();
						}
					}
					else if (currentPos.x - lastPos.x >= m_HuaDongDistance)
					{
						currentVector = TouZhiType.Right;
						if (PointMoveHandler != null)
						{
							PointMoveHandler(currentVector);
						}
					}
					if (currentPos.y - lastPos.y >= m_HuaDongDistance)
					{
						currentVector = TouZhiType.Up;
						if (PointMoveHandler != null)
						{
							PointMoveHandler(currentVector);
						}
						if (Singleton<GuideManager>.Instance.YinDaoJilu1.YinDaoShu != 3)
						{
							LianDanYinDaoPanDuan();
						}
					}
					lastPos = currentPos;
					timer = 0f;
				}
			}
			if (Input.GetMouseButtonUp(0))
			{
				currentVector = TouZhiType.None;
				m_MouseBtnInput = false;
			}
		}

		public void OnPointerClick(PointerEventData eventData)
		{
			if (PointClickHandler != null)
			{
				PointClickHandler(eventData);
			}
		}

		private void LianDanYinDaoPanDuan()
		{
			if (Singleton<RoleManager>.Instance.Role_Entity.ElixirGuideVal != 100)
			{
				Singleton<UIManager>.Instance.CloseWnd("GuideNewWnd.prefab");
			}
		}

		public void OnPointerDown(PointerEventData eventData)
		{
			if (PointDownHandler != null)
			{
				PointDownHandler(eventData);
			}
		}

		public void OnPointerUp(PointerEventData eventData)
		{
			if (PointUpHandler != null)
			{
				PointUpHandler(eventData);
			}
		}
	}
}
