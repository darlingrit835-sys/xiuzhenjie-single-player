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

		private bool m_IsOpen;

		private NeiShiWnd m_NeiShiWnd;

		private Vector2 lastSingleTouchPosition;

		private void Start()
		{
			m_IsOpen = true;
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
				m_JingMaiDic.Add(component.XueWeiId, component);
			}
		}

		private void Update()
		{
			if (m_IsOpen)
			{
				FingerJianCe();
			}
		}

		private void FingerJianCe()
		{
			if (Input.touchCount > 0 && Input.GetTouch(0).phase == TouchPhase.Began)
			{
				if (!EventSystem.current.IsPointerOverGameObject(Input.GetTouch(0).fingerId))
				{
					lastSingleTouchPosition = Input.mousePosition;
				}
			}
			else if (Input.touchCount > 0 && Input.GetTouch(0).phase == TouchPhase.Moved)
			{
				if (!EventSystem.current.IsPointerOverGameObject(Input.GetTouch(0).fingerId))
				{
					MoveCamera(Input.mousePosition);
				}
			}
			else if (Input.touchCount > 0 && Input.GetTouch(0).phase == TouchPhase.Ended)
			{
				if (!EventSystem.current.IsPointerOverGameObject(Input.GetTouch(0).fingerId))
				{
					lastSingleTouchPosition = Input.mousePosition;
				}
			}
			else
			{
				if (Input.touchCount <= 0 || Input.GetTouch(0).phase != TouchPhase.Stationary || IsPointerOverGameObject(Input.mousePosition))
				{
					return;
				}
				RaycastHit2D raycastHit2D = Physics2D.Raycast(Camera.main.ScreenToWorldPoint(Input.mousePosition), Vector2.zero);
				if (raycastHit2D.collider != null)
				{
					GameObject gameObject = raycastHit2D.collider.gameObject;
					if (gameObject.CompareTag("JingMaiItem"))
					{
						SheXianJianCe(gameObject);
					}
				}
			}
		}

		private bool IsPointerOverGameObject(Vector2 mousePosition)
		{
			PointerEventData pointerEventData = new PointerEventData(EventSystem.current);
			pointerEventData.position = mousePosition;
			List<RaycastResult> list = new List<RaycastResult>();
			EventSystem.current.RaycastAll(pointerEventData, list);
			if (list.Count > 0)
			{
				foreach (RaycastResult item in list)
				{
					if (item.gameObject.CompareTag("Guide"))
					{
						return false;
					}
				}
				return true;
			}
			return false;
		}

		private void SheXianJianCe(GameObject hitObj)
		{
			JingMaiItem item = hitObj.GetComponent<JingMaiItem>();
			if (m_NeiShiWnd == null)
			{
				m_NeiShiWnd = Singleton<UIManager>.Instance.FindWndByName<NeiShiWnd>("NeiShiWnd.prefab");
			}
			AcupointModel jingMai = null;
			if (!m_AcupointList.TryGetValue(item.XueWeiId, out jingMai))
			{
				return;
			}
			if (Singleton<RoleManager>.Instance.Role_Entity.AcupointGuideVal != 100 && Singleton<GuideManager>.Instance.YinDaoJilu1.YinDaoShu == 2)
			{
				if (!(item.XueWeiId == "1"))
				{
					return;
				}
				Singleton<GuideManager>.Instance.YinDaoJilu1.YinDaoShu++;
				Singleton<GuideManager>.Instance.ShowGuideNew(6);
				m_NeiShiWnd.ShowPanel(item, jingMai, () =>
				{
					JingMaiItem value = null;
					if (m_JingMaiDic.TryGetValue(item.XueWeiId, out value))
					{
						value.ChangeStatus(XueWeiStatus.Used);
					}
					string h = jingMai.h;
					GetChilds(h);
				});
			}
			else
			{
				if (EventSystem.current.IsPointerOverGameObject(Input.GetTouch(0).fingerId))
				{
					return;
				}
				m_NeiShiWnd.ShowPanel(item, jingMai, () =>
				{
					JingMaiItem value = null;
					if (m_JingMaiDic.TryGetValue(item.XueWeiId, out value))
					{
						value.ChangeStatus(XueWeiStatus.Used);
					}
					string h = jingMai.h;
					GetChilds(h);
				});
			}
		}

		public void SetUsedXueWei(List<XueWeiModel> ids)
		{
			if (ids == null || ids.Count == 0)
			{
				JingMaiItem value = null;
				if (m_JingMaiDic.TryGetValue("1", out value))
				{
					value.ChangeStatus(XueWeiStatus.Usable);
				}
				return;
			}
			foreach (XueWeiModel id in ids)
			{
				JingMaiItem value2 = null;
				if (!m_JingMaiDic.TryGetValue(id.a, out value2))
				{
					continue;
				}
				value2.ChangeStatus(XueWeiStatus.Used);
				AcupointModel value3 = null;
				if (!m_AcupointList.TryGetValue(id.a, out value3) || string.IsNullOrEmpty(value3.h))
				{
					continue;
				}
				string[] array = value3.h.Split(',');
				foreach (string key in array)
				{
					JingMaiItem value4 = null;
					if (m_JingMaiDic.TryGetValue(key, out value4) && value4.CurrentStatus == XueWeiStatus.Disable)
					{
						value4.ChangeStatus(XueWeiStatus.Usable);
					}
				}
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
			Vector2 vector = scenePos - lastSingleTouchPosition;
			SceneCamera.transform.Translate(new Vector3(vector.x * -0.002f, vector.y * -0.002f));
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
