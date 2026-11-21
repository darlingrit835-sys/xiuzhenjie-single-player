using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

namespace XiuZhenJie
{
	public class JingMaiManager : MonoSingleton<JingMaiManager>
	{
		public Camera SceneCamera;

		public Transform PeopleXueWeiBg;

		private Dictionary<string, JingMaiItem> m_JingMaiDic;

		private Dictionary<string, AcupointModel> m_AcupointList;

		private readonly List<RaycastResult> m_UIHits = new List<RaycastResult>();
		private readonly List<Collider2D> m_PointHits = new List<Collider2D>();
		private NeiShiWnd m_NeiShiWnd;
		private Vector2 m_PointerOrigin;
		private Vector2 lastSingleTouchPosition;
		private bool m_TrackingPointer;
		private bool m_Dragged;

		internal InkMeridianSpectrum Spectrum { get; private set; }

		private void Start()
		{
			if (m_JingMaiDic == null)
				Init();
		}

		private void Init()
		{
			m_AcupointList = new Dictionary<string, AcupointModel>();
			foreach (AcupointModel item in Singleton<FacadeManager>.Instance.SelectAcupointModels())
			{
				m_AcupointList.Add(item.a, item);
			}
			m_JingMaiDic = new Dictionary<string, JingMaiItem>();
			foreach (Transform item2 in PeopleXueWeiBg)
			{
				JingMaiItem component = item2.gameObject.GetComponent<JingMaiItem>();
				if (component != null) m_JingMaiDic.Add(component.XueWeiId, component);
			}
			Spectrum = InkMeridianSpectrum.Create(this, m_JingMaiDic, m_AcupointList);
		}

		private void Update()
		{
			if (Input.touchCount > 0)
			{
				Touch touch = Input.GetTouch(0);
				if (touch.phase == TouchPhase.Canceled || Input.touchCount != 1)
				{
					m_TrackingPointer = false;
					return;
				}
				TrackPointer(touch.position, touch.phase == TouchPhase.Began, touch.phase == TouchPhase.Ended);
			}
#if UNITY_EDITOR || UNITY_STANDALONE
			else if (Input.GetMouseButton(0) || Input.GetMouseButtonUp(0))
			{
				TrackPointer(Input.mousePosition, Input.GetMouseButtonDown(0), Input.GetMouseButtonUp(0));
			}
#endif
		}

		private void TrackPointer(Vector2 position, bool pressed, bool released)
		{
			if (pressed)
			{
				m_TrackingPointer = !IsPointerOverGameObject(position);
				m_PointerOrigin = lastSingleTouchPosition = position;
				m_Dragged = false;
				return;
			}
			if (!m_TrackingPointer)
				return;
			float dragThreshold = EventSystem.current != null ? EventSystem.current.pixelDragThreshold : 8f;
			m_Dragged |= (position - m_PointerOrigin).sqrMagnitude > dragThreshold * dragThreshold;
			if (released)
			{
				m_TrackingPointer = false;
				if (!m_Dragged)
					SelectAtPosition(position);
			}
			else if (m_Dragged && position != lastSingleTouchPosition)
			{
				MoveCamera(position);
			}
		}

		private bool IsPointerOverGameObject(Vector2 position)
		{
			var events = EventSystem.current;
			if (events == null)
				return false;
			events.RaycastAll(new PointerEventData(events) { position = position }, m_UIHits);
			return m_UIHits.Count > 0 && !m_UIHits[0].gameObject.CompareTag("Guide");
		}

		internal void SelectAtPosition(Vector2 position)
		{
			if (IsPointerOverGameObject(position))
				return;
			var nearest = Spectrum?.Nearest(position);
			if (nearest != null) { ShowAcupoint(nearest); return; }
			Physics2D.OverlapPoint(SceneCamera.ScreenToWorldPoint(position), new ContactFilter2D().NoFilter(), m_PointHits);
			foreach (Collider2D hit in m_PointHits)
			{
				// Decorative colliders can cover the actual node.
				for (Transform node = hit.transform; node != null && node != PeopleXueWeiBg; node = node.parent)
				{
					if (node.TryGetComponent(out JingMaiItem item) && !string.IsNullOrEmpty(item.XueWeiId))
					{
						ShowAcupoint(item);
						return;
					}
				}
			}
		}

		internal void ShowAcupoint(JingMaiItem item)
		{
			if (!m_AcupointList.TryGetValue(item.XueWeiId, out AcupointModel point))
				return;
			if (m_NeiShiWnd == null)
				m_NeiShiWnd = Singleton<UIManager>.Instance.FindWndByName<NeiShiWnd>("NeiShiWnd.prefab");
			if (m_NeiShiWnd == null)
				return;

			var guide = Singleton<GuideManager>.Instance;
			if (guide.YinDaoJilu1.YinDaoName == 6 && guide.YinDaoJilu1.YinDaoShu == 2 &&
				Singleton<RoleManager>.Instance.Role_Entity.AcupointGuideVal == 1 && item.XueWeiId != "1")
				return;
			Spectrum?.Focus(item.XueWeiId);
			m_NeiShiWnd.ShowPanel(item, point, () =>
			{
				item.ChangeStatus(XueWeiStatus.Used);
				GetChilds(point.h);
			});
		}

		public void SetUsedXueWei(List<XueWeiModel> ids)
		{
			if (m_JingMaiDic == null)
				Init();
			if (m_JingMaiDic.TryGetValue("1", out JingMaiItem first) && first.CurrentStatus == XueWeiStatus.Disable)
				first.ChangeStatus(XueWeiStatus.Usable);
			foreach (XueWeiModel id in ids)
			{
				if (m_JingMaiDic.TryGetValue(id.a, out JingMaiItem item))
					item.ChangeStatus(XueWeiStatus.Used);
				if (m_AcupointList.TryGetValue(id.a, out AcupointModel point))
					GetChilds(point.h);
			}
		}

		private void GetChilds(string childIds)
		{
			if (string.IsNullOrEmpty(childIds))
			{
				return;
			}
			string[] array = childIds.Split(',');
			foreach (string key in array)
			{
				JingMaiItem value = null;
				if (m_JingMaiDic.TryGetValue(key, out value) && value.CurrentStatus == XueWeiStatus.Disable)
				{
					value.ChangeStatus(XueWeiStatus.Usable);
				}
			}
		}

		public void MoveCamera(Vector2 scenePos)
		{
			Spectrum?.CancelTravel();
			Vector2 vector = scenePos - lastSingleTouchPosition;
			float units = 2f * SceneCamera.orthographicSize / Mathf.Max(1, SceneCamera.pixelHeight);
			SceneCamera.transform.Translate(-vector.x * units, -vector.y * units, 0);
			SceneCamera.transform.localPosition = new Vector3(Mathf.Clamp(SceneCamera.transform.localPosition.x, -6f, 6f), Mathf.Clamp(SceneCamera.transform.localPosition.y, -3f, 7f), -10f);
			lastSingleTouchPosition = scenePos;
		}

		public void InitData()
		{
			foreach (Transform item in base.transform)
			{
				_ = item.Find("New Text").GetComponent<TextMesh>().text;
			}
		}
	}
}
