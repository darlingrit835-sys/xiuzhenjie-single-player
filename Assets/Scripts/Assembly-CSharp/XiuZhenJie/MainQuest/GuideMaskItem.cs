using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Events;
using UnityEngine.UI;

namespace XiuZhenJie
{
	public class GuideMaskItem : MonoBehaviour, IPointerClickHandler, IEventSystemHandler
	{
		public int CenShu = 3;

		public GuideType GuideMaskType;

		public int GuideIndex;

		public Image TiaoGuoImg;

		private static readonly int FocusRectId = Shader.PropertyToID("_FocusRect");
		private static readonly int FocusMotionId = Shader.PropertyToID("_FocusMotion");
		private static readonly int DriftId = Shader.PropertyToID("_Drift");

		private Image m_Target;
		private Image m_CoverImage;
		private Canvas m_Canvas;
		private Material m_PreviousMaterial;
		private Material m_FogMaterial;
		private RectTransform m_Hint;
		private RectTransform m_FollowTarget;
		private Transform m_WorldTarget;
		private Camera m_WorldCamera;
		private Text m_HintText;
		private CanvasGroup m_HintGroup;
		private Vector2 m_HintPosition;
		private readonly Vector3[] m_Corners = new Vector3[4];
		private Vector4 m_VisibleRect;
		private float m_OpenedAt;
		private bool m_PanDuan;
		private UnityAction m_ClickCallBack;

		private Camera EventCamera => m_Canvas.renderMode == RenderMode.ScreenSpaceOverlay ? null : m_Canvas.worldCamera;

		private void Awake()
		{
			m_Target = GetComponent<Image>();
			m_CoverImage = transform.parent.GetComponent<Image>();
		}

		public void BeginShow(UnityAction clickCallBack = null)
		{
			m_ClickCallBack = clickCallBack;
			if (m_FogMaterial == null)
			{
				m_Canvas = GetComponentInParent<Canvas>().rootCanvas;
				m_FogMaterial = new Material(ReleasedResources.Load<Shader>("shaders/GuideFog"));
				m_Target.color = Color.clear;
				StyleHint();
			}
			if (m_CoverImage.material != m_FogMaterial)
			{
				m_PreviousMaterial = m_CoverImage.material;
				m_CoverImage.material = m_FogMaterial;
			}
			m_VisibleRect = Vector4.zero;
			m_OpenedAt = Time.unscaledTime;
			UpdateVisibleRect();
			UpdateAnimation();
		}

		private void LateUpdate()
		{
			if (m_FogMaterial != null && m_CoverImage.material == m_FogMaterial)
			{
				UpdateVisibleRect();
				UpdateAnimation();
			}
		}

		private void UpdateAnimation()
		{
			float elapsed = Time.unscaledTime - m_OpenedAt;
			float progress = Mathf.Clamp01(elapsed / 0.48f);
			float remaining = 1f - progress;
			float reveal = 1f - remaining * remaining * remaining;
			float pulse = 0.5f + 0.5f * Mathf.Sin(elapsed * (2f * Mathf.PI / 1.8f));
			float sweep = elapsed * (2f * Mathf.PI / 2.8f) + Mathf.PI * 0.5f;
			m_FogMaterial.SetVector(FocusMotionId, new Vector4(reveal, pulse, Mathf.Cos(sweep), Mathf.Sin(sweep)));
			m_FogMaterial.SetFloat(DriftId, Time.unscaledTime);

			const float hintFadeEnd = 0.36f;
			if (m_HintGroup != null && (elapsed < hintFadeEnd || m_HintGroup.alpha < 1f))
			{
				float hintReveal = Mathf.SmoothStep(0f, 1f, Mathf.InverseLerp(0.10f, hintFadeEnd, elapsed));
				m_HintGroup.alpha = hintReveal;
				m_Hint.anchoredPosition = m_HintPosition + Vector2.down * (5f * (1f - hintReveal));
			}
		}

		private void OnDisable()
		{
			if (m_FogMaterial != null && m_CoverImage != null && m_CoverImage.material == m_FogMaterial)
			{
				m_CoverImage.material = m_PreviousMaterial;
			}
		}

		private void OnDestroy()
		{
			if (m_FogMaterial != null)
			{
				Destroy(m_FogMaterial);
			}
		}

		internal void FollowTarget(RectTransform target) => m_FollowTarget = target;

		internal void FollowWorldTarget(Transform target, Camera camera)
		{
			m_WorldTarget = target;
			m_WorldCamera = camera;
		}

		private void UpdateVisibleRect()
		{
			var rect = m_Target.rectTransform;
			if (m_WorldTarget != null && m_WorldCamera != null)
			{
				var cover = m_CoverImage.rectTransform;
				Vector2 screen = m_WorldCamera.WorldToScreenPoint(m_WorldTarget.position);
				if (RectTransformUtility.ScreenPointToLocalPointInRectangle(cover, screen, EventCamera, out var center))
				{
					rect.anchorMin = rect.anchorMax = cover.pivot;
					rect.pivot = Vector2.one * .5f;
					rect.sizeDelta = new Vector2(46f, 46f);
					rect.anchoredPosition = center;
				}
			}
			else if (m_FollowTarget != null)
			{
				// Align both the visible opening and its event receiver to the painted shop.
				var cover = m_CoverImage.rectTransform;
				m_FollowTarget.GetWorldCorners(m_Corners);
				Vector2 lower = cover.InverseTransformPoint(m_Corners[0]);
				Vector2 upper = cover.InverseTransformPoint(m_Corners[2]);
				if (rect.anchorMin != cover.pivot || rect.anchorMax != cover.pivot)
					rect.anchorMin = rect.anchorMax = cover.pivot;
				if (rect.offsetMin != lower) rect.offsetMin = lower;
				if (rect.offsetMax != upper) rect.offsetMax = upper;
			}
			rect.GetWorldCorners(m_Corners);
			Camera camera = EventCamera;
			Vector2 min = Vector2.one;
			Vector2 max = Vector2.zero;
			foreach (Vector3 corner in m_Corners)
			{
				// GuideFog compares against viewport UVs, including when the UI camera renders offscreen.
				Vector2 point = camera != null ? (Vector2)camera.WorldToViewportPoint(corner)
					: Rect.PointToNormalized(m_Canvas.pixelRect, RectTransformUtility.WorldToScreenPoint(null, corner));
				min = Vector2.Min(min, point);
				max = Vector2.Max(max, point);
			}
			Vector4 visibleRect = new Vector4(Mathf.Clamp01(min.x), Mathf.Clamp01(min.y),
				Mathf.Clamp01(max.x), Mathf.Clamp01(max.y));
			if (m_VisibleRect != visibleRect)
			{
				m_VisibleRect = visibleRect;
				m_FogMaterial.SetVector(FocusRectId, visibleRect);
				PlaceHint();
			}
		}

		private void StyleHint()
		{
			m_Hint = transform.Find("Img") as RectTransform;
			m_HintText = m_Hint != null ? m_Hint.Find("Text")?.GetComponent<Text>() : null;
			if (m_HintText == null)
			{
				m_Hint = null;
				return;
			}

			foreach (Transform child in m_Hint)
			{
				if (child.name == "DaShi" || child.name == "ZhiBiao")
				{
					child.gameObject.SetActive(false);
				}
			}
			Image background = m_Hint.GetComponent<Image>();
			InkInterfaceArt.Set(background, "LightPaper", false);
			background.raycastTarget = false;
			if (!m_Hint.TryGetComponent(out m_HintGroup))
			{
				m_HintGroup = m_Hint.gameObject.AddComponent<CanvasGroup>();
			}
			m_HintText.color = InkInterfaceArt.Ink;
			m_HintText.alignment = TextAnchor.MiddleLeft;
			m_HintText.raycastTarget = false;
			m_HintText.fontSize = 14;
			m_HintText.lineSpacing = 1.08f;
			m_HintText.resizeTextForBestFit = false;
			m_HintText.rectTransform.anchorMin = Vector2.zero;
			m_HintText.rectTransform.anchorMax = Vector2.one;
			m_HintText.rectTransform.offsetMin = new Vector2(16f, 11f);
			m_HintText.rectTransform.offsetMax = new Vector2(-16f, -11f);
			UIManager.FitTextToRect(m_HintText);
			m_HintText.resizeTextForBestFit = false;
		}

		private void PlaceHint()
		{
			if (m_Hint == null)
			{
				return;
			}
			const float margin = 12f;
			RectTransform target = m_Target.rectTransform;
			RectTransform cover = m_CoverImage.rectTransform;
			cover.GetWorldCorners(m_Corners);
			Vector2 lower = target.InverseTransformPoint(m_Corners[0]);
			Vector2 upper = target.InverseTransformPoint(m_Corners[2]);
			float preferredWidth = Mathf.Clamp(m_HintText.preferredWidth + 33f, 112f, 282f);
			float width = Mathf.Max(1f, Mathf.Min(preferredWidth, upper.x - lower.x - 2f * margin));
			var measurement = m_HintText.GetGenerationSettings(new Vector2(Mathf.Max(1f, width - 32f), 0f));
			measurement.resizeTextForBestFit = false;
			measurement.verticalOverflow = VerticalWrapMode.Overflow;
			float textHeight = m_HintText.cachedTextGeneratorForLayout.GetPreferredHeight(m_HintText.text, measurement) / m_HintText.pixelsPerUnit;
			float preferredHeight = Mathf.Max(40f, textHeight + 24f);
			float height = Mathf.Max(1f, Mathf.Min(preferredHeight, upper.y - lower.y - 2f * margin));
			m_HintText.resizeTextForBestFit = height < preferredHeight;
			m_Hint.anchorMin = target.pivot;
			m_Hint.anchorMax = target.pivot;
			m_Hint.pivot = new Vector2(0.5f, 0.5f);
			m_Hint.sizeDelta = new Vector2(width, height);
			float below = target.rect.yMin - margin - height * 0.5f;
			float above = target.rect.yMax + margin + height * 0.5f;
			float y = below - height * 0.5f >= lower.y ? below : above;
			m_HintPosition = new Vector2(
				Mathf.Clamp(target.rect.center.x, lower.x + margin + width * 0.5f, upper.x - margin - width * 0.5f),
				Mathf.Clamp(y, lower.y + margin + height * 0.5f, upper.y - margin - height * 0.5f));
			m_Hint.anchoredPosition = m_HintPosition;
		}

		public void OnPointerDown(PointerEventData eventData)
		{
			if (GuideMaskType != GuideType.JingMai && ContainsTarget(eventData))
			{
				PassEvent(eventData, ExecuteEvents.pointerDownHandler, CenShu);
			}
		}

		public void OnPointerUp(PointerEventData eventData)
		{
			if (GuideMaskType != GuideType.JingMai && ContainsTarget(eventData))
			{
				PassEvent(eventData, ExecuteEvents.pointerUpHandler, CenShu);
			}
		}

		public void OnPointerClick(PointerEventData eventData)
		{
			if (!ContainsTarget(eventData))
			{
				return;
			}
			if (GuideMaskType != GuideType.JingMai)
				PassEvent(eventData, ExecuteEvents.pointerClickHandler, CenShu);
			m_ClickCallBack?.Invoke();
		}

		private bool ContainsTarget(PointerEventData data)
		{
			return RectTransformUtility.RectangleContainsScreenPoint(m_Target.rectTransform, data.position, data.pressEventCamera);
		}

		private void PassEvent<T>(PointerEventData data, ExecuteEvents.EventFunction<T> function, int cenShuYi) where T : IEventSystemHandler
		{
			List<RaycastResult> list = new List<RaycastResult>();
			EventSystem.current.RaycastAll(data, list);
			m_PanDuan = false;
			int searchLimit = Mathf.Min(list.Count, cenShuYi);
			for (int i = 0; i < searchLimit; i++)
			{
				if (list[i].gameObject.CompareTag("Guide"))
				{
					m_PanDuan = true;
					cenShuYi = i;
					break;
				}
				if (list[i].gameObject.CompareTag("JingMaiItem"))
				{
					m_PanDuan = true;
					cenShuYi = i;
					break;
				}
			}
			if (m_PanDuan)
			{
				switch (Singleton<GuideManager>.Instance.YinDaoJilu1.YinDaoName)
				{
				case 2:
					Singleton<GuideManager>.Instance.YinDaoJilu1.YinDaoShu++;
					ExecuteEvents.Execute(list[cenShuYi].gameObject, data, function);
					break;
				case 6:
					Singleton<GuideManager>.Instance.YinDaoJilu1.YinDaoShu++;
					int acupointHit = Singleton<GuideManager>.Instance.YinDaoJilu1.YinDaoShu == 3 && list.Count > 2 ? 2 : cenShuYi;
					ExecuteEvents.Execute(list[acupointHit].gameObject, data, function);
					break;
				case 8:
				{
					for (int j = 2; j < Mathf.Min(5, list.Count); j++)
					{
						ExecuteEvents.Execute(list[j].gameObject, data, function);
					}
					break;
				}
				default:
					if (Singleton<GuideManager>.Instance.YinDaoJilu1.YinDaoName >= 5 && Singleton<GuideManager>.Instance.YinDaoJilu1.YinDaoName <= 10)
					{
						Singleton<GuideManager>.Instance.YinDaoJilu1.YinDaoShu++;
						ExecuteEvents.Execute(list[cenShuYi].gameObject, data, function);
					}
					break;
				}
				int yinDaoShu = Singleton<GuideManager>.Instance.YinDaoJilu1.YinDaoShu;
				switch (Singleton<GuideManager>.Instance.YinDaoJilu1.YinDaoName)
				{
				case 2:
					if (yinDaoShu != 1 && yinDaoShu != 4 && yinDaoShu != 11 && yinDaoShu != 16 && yinDaoShu != 20 && yinDaoShu < 22)
					{
						Singleton<UIManager>.Instance.CloseWnd("GuideNewWnd.prefab");
						Singleton<UIManager>.Instance.PopUpWnd("GuideNewWnd.prefab", true);
						break;
					}
					switch (yinDaoShu)
					{
					case 4:
						gameObject.gameObject.SetActive(value: false);
						Singleton<UIManager>.Instance.CloseWnd("GuideNewWnd.prefab");
						Singleton<UIManager>.Instance.PopUpWnd("GuideNewWnd.prefab", true);
						break;
					case 11:
					case 16:
					case 20:
						Singleton<UIManager>.Instance.CloseWnd("GuideNewWnd.prefab");
						if (yinDaoShu == 16)
						{
							ShangChuan(2);
						}
						break;
					case 22:
						gameObject.gameObject.SetActive(value: false);
						Time.timeScale = 1f;
						Singleton<UIManager>.Instance.CloseWnd("GuideNewWnd.prefab");
						ShangChuan(2);
						Singleton<GuideManager>.Instance.YinDaoJilu1.YinDaoName = 0;
						Singleton<GuideManager>.Instance.YinDaoJilu1.YinDaoShu = 1;
						break;
					}
					break;
				case 10:
					gameObject.gameObject.SetActive(value: false);
					if (yinDaoShu <= 2)
					{
						Singleton<UIManager>.Instance.CloseWnd("GuideNewWnd.prefab");
						Singleton<UIManager>.Instance.PopUpWnd("GuideNewWnd.prefab", true);
					}
					break;
				case 5:
					if (Singleton<GuideManager>.Instance.YinDaoJilu1.YinDaoShu < 5)
					{
						Singleton<UIManager>.Instance.CloseWnd("GuideNewWnd.prefab");
						Singleton<UIManager>.Instance.PopUpWnd("GuideNewWnd.prefab", true);
						break;
					}
					Singleton<UIManager>.Instance.CloseWnd("GuideNewWnd.prefab");
					ShangChuan(5);
					Singleton<RoleManager>.Instance.Role_Entity.ShopGuideVal = 100;
					Singleton<GuideManager>.Instance.YinDaoJilu1.YinDaoName = 2;
					Singleton<GuideManager>.Instance.YinDaoJilu1.YinDaoShu = 1;
					break;
				case 6:
					switch (Singleton<GuideManager>.Instance.YinDaoJilu1.YinDaoShu)
					{
					case 1:
					case 5:
						Singleton<UIManager>.Instance.CloseWnd("GuideNewWnd.prefab");
						Singleton<UIManager>.Instance.PopUpWnd("GuideNewWnd.prefab", true);
						break;
					case 2:
					case 3:
						Singleton<UIManager>.Instance.CloseWnd("GuideNewWnd.prefab");
						break;
					case 4:
						Singleton<UIManager>.Instance.CloseWnd("GuideNewWnd.prefab");
						ShangChuan(6);
						Singleton<UIManager>.Instance.PopUpWnd("GuideNewWnd.prefab", true);
						break;
					case 6:
						Singleton<UIManager>.Instance.CloseWnd("GuideNewWnd.prefab");
						ShangChuan(6);
						Singleton<RoleManager>.Instance.Role_Entity.AcupointGuideVal = 100;
						Singleton<GuideManager>.Instance.YinDaoJilu1.YinDaoName = 2;
						Singleton<GuideManager>.Instance.YinDaoJilu1.YinDaoShu = 1;
						break;
					}
					break;
				case 7:
					if (Singleton<GuideManager>.Instance.YinDaoJilu1.YinDaoShu <= 2)
					{
						Singleton<UIManager>.Instance.CloseWnd("GuideNewWnd.prefab");
						Singleton<UIManager>.Instance.PopUpWnd("GuideNewWnd.prefab", true);
					}
					break;
				case 8:
					Singleton<UIManager>.Instance.CloseWnd("GuideNewWnd.prefab");
					if (Singleton<GuideManager>.Instance.YinDaoJilu1.YinDaoShu == 8)
					{
						ShangChuan(8);
						Singleton<RoleManager>.Instance.Role_Entity.ElixirGuideVal = 100;
					}
					break;
				case 9:
					Singleton<UIManager>.Instance.CloseWnd("GuideNewWnd.prefab");
					Time.timeScale = 1f;
					break;
				case 3:
				case 4:
					break;
				}
				return;
			}
			switch (Singleton<GuideManager>.Instance.YinDaoJilu1.YinDaoName)
			{
			case 2:
			{
				int yinDaoShu3 = Singleton<GuideManager>.Instance.YinDaoJilu1.YinDaoShu;
				if ((yinDaoShu3 >= 11) & (yinDaoShu3 <= 13))
				{
					Singleton<GuideManager>.Instance.YinDaoJilu1.YinDaoShu++;
					gameObject.gameObject.SetActive(value: false);
					Singleton<UIManager>.Instance.CloseWnd("GuideNewWnd.prefab");
					Singleton<UIManager>.Instance.PopUpWnd("GuideNewWnd.prefab", true);
					break;
				}
				switch (yinDaoShu3)
				{
				case 14:
					Singleton<GuideManager>.Instance.YinDaoJilu1.YinDaoShu++;
					gameObject.gameObject.SetActive(value: false);
					Singleton<UIManager>.Instance.CloseWnd("GuideNewWnd.prefab");
					Time.timeScale = 1f;
					break;
				case 23:
					gameObject.gameObject.SetActive(value: false);
					Time.timeScale = 1f;
					Singleton<UIManager>.Instance.CloseWnd("GuideNewWnd.prefab");
					ShangChuan(2);
					Singleton<GuideManager>.Instance.YinDaoJilu1.YinDaoName = 0;
					Singleton<GuideManager>.Instance.YinDaoJilu1.YinDaoShu = 1;
					break;
				}
				break;
			}
			case 10:
			{
				int yinDaoShu2 = Singleton<GuideManager>.Instance.YinDaoJilu1.YinDaoShu;
				if ((yinDaoShu2 > 3) & (yinDaoShu2 < 7))
				{
					gameObject.gameObject.SetActive(value: false);
					Singleton<UIManager>.Instance.CloseWnd("GuideNewWnd.prefab");
					Singleton<UIManager>.Instance.PopUpWnd("GuideNewWnd.prefab", true);
				}
				if (yinDaoShu2 == 7)
				{
					Singleton<UIManager>.Instance.CloseWnd("GuideNewWnd.prefab");
					Singleton<GuideManager>.Instance.YinDaoJilu1.YinDaoShu = 100;
				}
				break;
			}
			case 5:
				if (Singleton<GuideManager>.Instance.YinDaoJilu1.YinDaoShu >= 4)
				{
					Singleton<UIManager>.Instance.CloseWnd("GuideNewWnd.prefab");
					ShangChuan(5);
					Singleton<RoleManager>.Instance.Role_Entity.ShopGuideVal = 100;
					Singleton<GuideManager>.Instance.YinDaoJilu1.YinDaoName = 2;
					Singleton<GuideManager>.Instance.YinDaoJilu1.YinDaoShu = 1;
				}
				break;
			case 6:
				if (Singleton<GuideManager>.Instance.YinDaoJilu1.YinDaoShu == 2)
				{
					Singleton<GuideManager>.Instance.YinDaoJilu1.YinDaoShu++;
					Singleton<UIManager>.Instance.CloseWnd("GuideNewWnd.prefab");
					Singleton<UIManager>.Instance.PopUpWnd("GuideNewWnd.prefab", true);
				}
				break;
			case 7:
				if (Singleton<GuideManager>.Instance.YinDaoJilu1.YinDaoShu == 7)
				{
					Singleton<UIManager>.Instance.CloseWnd("GuideNewWnd.prefab");
					ShangChuan(7);
					Singleton<RoleManager>.Instance.Role_Entity.SignGuideVal = 100;
				}
				else if (Singleton<GuideManager>.Instance.YinDaoJilu1.YinDaoShu == 2)
				{
					Singleton<GuideManager>.Instance.YinDaoJilu1.YinDaoShu++;
					Singleton<UIManager>.Instance.CloseWnd("GuideNewWnd.prefab");
					Singleton<UIManager>.Instance.PopUpWnd("GuideNewWnd.prefab", true);
				}
				break;
			case 8:
				Singleton<UIManager>.Instance.CloseWnd("GuideNewWnd.prefab");
				if (Singleton<GuideManager>.Instance.YinDaoJilu1.YinDaoShu == 5)
				{
					ShangChuan(8);
					Singleton<RoleManager>.Instance.Role_Entity.ElixirGuideVal = 100;
				}
				break;
			case 3:
			case 4:
			case 9:
				break;
			}
		}

		private void ShangChuan(int guide)
		{
			GuideInfoUpdateReq_1002013 data = new GuideInfoUpdateReq_1002013
			{
				Type = guide
			};
			Singleton<NetManager>.Instance.SendMsg(MsgType.UPDATE_GUIDE, data, (object obj) =>
			{
			});
			switch (guide)
			{
			case 2:
				Singleton<RoleManager>.Instance.Role_Entity.BattleGuideVal++;
				if (Singleton<RoleManager>.Instance.Role_Entity.BattleGuideVal >= 5)
				{
					Singleton<RoleManager>.Instance.Role_Entity.BattleGuideVal = 100;
				}
				break;
			case 5:
				Singleton<RoleManager>.Instance.Role_Entity.ShopGuideVal = 100;
				break;
			case 6:
				Singleton<RoleManager>.Instance.Role_Entity.AcupointGuideVal++;
				if (Singleton<RoleManager>.Instance.Role_Entity.AcupointGuideVal >= 3)
				{
					Singleton<RoleManager>.Instance.Role_Entity.AcupointGuideVal = 100;
				}
				break;
			}
		}
	}
}
