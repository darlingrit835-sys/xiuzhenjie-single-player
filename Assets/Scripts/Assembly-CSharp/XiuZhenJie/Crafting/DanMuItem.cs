using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace XiuZhenJie
{
	public class DanMuItem : MonoBehaviour
	{
		public Text ContentText;

		public Text TitleText;

		public Image Title;

		public Text Vip_TitleText;

		public GameObject Vip_Title;

		public Button DanClickBtn;

		public RectTransform Content;

		private HorizontalLayoutGroup m_ContentLayoutGroup;

		private DanMuStatus m_DanMuStatus;

		private RectTransform m_SelfRect;

		private RectTransform m_TextRect;

		private RectTransform m_UiRoot;

		private RectTransform m_StandByRect;

		private float m_MinMoveX;

		private UnityAction<DanMuItem> m_CompeleteAction;

		private float m_AfterSecondStart;

		private float m_Time;

		private void Awake()
		{
			m_DanMuStatus = DanMuStatus.Waitting;
			m_SelfRect = base.gameObject.GetComponent<RectTransform>();
			m_TextRect = ContentText.gameObject.GetComponent<RectTransform>();
			m_ContentLayoutGroup = Content.gameObject.GetComponent<HorizontalLayoutGroup>();
			m_UiRoot = Singleton<UIManager>.Instance.m_UiRoot;
			Content.sizeDelta = new Vector2(ContentText.rectTransform.sizeDelta.x + 120f, Content.sizeDelta.y);
			m_SelfRect.sizeDelta = new Vector2(Content.sizeDelta.x, m_SelfRect.sizeDelta.y);
		}

		private void Update()
		{
			if (m_AfterSecondStart > 0f && m_DanMuStatus == DanMuStatus.Waitting)
			{
				m_Time += Time.deltaTime;
				if (m_Time >= m_AfterSecondStart)
				{
					StartRunning();
					m_AfterSecondStart = -1f;
					m_Time = 0f;
				}
			}
			if (m_DanMuStatus != DanMuStatus.Running)
			{
				return;
			}
			m_Time = 0f;
			m_SelfRect.Translate(Vector3.left * Time.deltaTime * 0.7f, Space.Self);
			if (m_SelfRect.anchoredPosition.x < m_MinMoveX)
			{
				m_DanMuStatus = DanMuStatus.Waitting;
				Clear();
				if (m_CompeleteAction != null)
				{
					m_CompeleteAction(this);
				}
			}
		}

		public void SetValueNoRole(string content, UnityAction<DanMuItem> compeleteAction, UnityAction<string> danMuClickAction)
		{
			m_CompeleteAction = compeleteAction;
			m_AfterSecondStart = CompuateStartTime();
			ContentText.text = content;
			Vip_Title.gameObject.SetActive(value: false);
			Title.gameObject.SetActive(value: false);
			m_ContentLayoutGroup.padding.left = 17;
			ContentText.rectTransform.sizeDelta = new Vector2(content.Length * 15, ContentText.rectTransform.sizeDelta.y);
			Content.sizeDelta = new Vector2(content.Length * 15, Content.sizeDelta.y);
			m_SelfRect.sizeDelta = new Vector2(Content.sizeDelta.x, m_SelfRect.sizeDelta.y);
			m_SelfRect.anchoredPosition = new Vector2(10f, 0f);
			m_MinMoveX = 0f - (m_UiRoot.sizeDelta.x + m_SelfRect.sizeDelta.x + 20f);
			if (danMuClickAction != null)
			{
				DanClickBtn.onClick.RemoveAllListeners();
				DanClickBtn.onClick.AddListener(() =>
				{
					danMuClickAction(content);
				});
			}
		}

		public void ReSetValueNoRole(string content, UnityAction<string> danMuClickAction)
		{
			m_AfterSecondStart = CompuateStartTime();
			ContentText.text = content;
			Vip_Title.gameObject.SetActive(value: false);
			Title.gameObject.SetActive(value: false);
			m_ContentLayoutGroup.padding.left = 17;
			ContentText.rectTransform.sizeDelta = new Vector2(content.Length * 15, ContentText.rectTransform.sizeDelta.y);
			Content.sizeDelta = new Vector2(content.Length * 15, Content.sizeDelta.y);
			m_SelfRect.sizeDelta = new Vector2(Content.sizeDelta.x, m_SelfRect.sizeDelta.y);
			m_SelfRect.anchoredPosition = new Vector2(10f, 0f);
			m_MinMoveX = 0f - (m_UiRoot.sizeDelta.x + m_SelfRect.sizeDelta.x + 20f);
			if (danMuClickAction != null)
			{
				DanClickBtn.onClick.RemoveAllListeners();
				DanClickBtn.onClick.AddListener(() =>
				{
					danMuClickAction(content);
				});
			}
		}

		public void SetValue(int headFrameId, int chengHaoFrameId, int sex, int level, string headId, string title, string roleName, string content, UnityAction<DanMuItem> compeleteAction, UnityAction<DanMuItemEntity> danMuClickAction)
		{
			m_CompeleteAction = compeleteAction;
			ReSetValue(headFrameId, chengHaoFrameId, sex, level, headId, title, roleName, content, danMuClickAction);
		}

		public void ReSetValue(int headFrameId, int chengHaoFrameId, int sex, int level, string headId, string title, string roleName, string content, UnityAction<DanMuItemEntity> danMuClickAction)
		{
			m_AfterSecondStart = CompuateStartTime();
			ContentText.text = $"<color=#FF8001>{roleName}：</color>{content}";
			if (string.IsNullOrEmpty(title))
			{
				Vip_Title.gameObject.SetActive(value: false);
				Title.gameObject.SetActive(value: false);
				m_ContentLayoutGroup.padding.left = 17;
				ContentText.rectTransform.sizeDelta = new Vector2((roleName.Length + content.Length) * 15, ContentText.rectTransform.sizeDelta.y);
				Content.sizeDelta = new Vector2((roleName.Length + content.Length) * 15, Content.sizeDelta.y);
			}
			else
			{
				if (chengHaoFrameId == 0)
				{
					Vip_Title.gameObject.SetActive(value: false);
					Title.gameObject.SetActive(value: true);
					TitleText.text = title;
				}
				else
				{
					Title.gameObject.SetActive(value: false);
					Vip_Title.gameObject.SetActive(value: true);
					Vip_TitleText.text = title;
				}
				m_ContentLayoutGroup.padding.left = 0;
				ContentText.rectTransform.sizeDelta = new Vector2(80 + (roleName.Length + content.Length) * 16, ContentText.rectTransform.sizeDelta.y);
				Content.sizeDelta = new Vector2(100 + (roleName.Length + content.Length) * 16, Content.sizeDelta.y);
			}
			m_SelfRect.sizeDelta = new Vector2(Content.sizeDelta.x, m_SelfRect.sizeDelta.y);
			m_SelfRect.anchoredPosition = new Vector2(10f, 0f);
			m_MinMoveX = 0f - (m_UiRoot.sizeDelta.x + m_SelfRect.sizeDelta.x + 20f);
			if (danMuClickAction != null)
			{
				DanClickBtn.onClick.RemoveAllListeners();
				DanClickBtn.onClick.AddListener(() =>
				{
					DanMuItemEntity arg = new DanMuItemEntity
					{
						ChengHaoFrameId = chengHaoFrameId,
						HeadId = headId,
						Level = level,
						RoleName = roleName,
						Sex = sex,
						HeadFrameId = headFrameId
					};
					danMuClickAction(arg);
				});
			}
		}

		private float CompuateStartTime()
		{
			Transform parent = base.gameObject.transform.parent;
			if (parent.childCount < 2)
			{
				return 0.01f;
			}
			if (parent.GetChild(parent.childCount - 2).gameObject.GetComponent<DanMuItem>().GetEndDistance() >= 1f)
			{
				return 0.01f;
			}
			return 2f;
		}

		public void StartRunning()
		{
			if (m_DanMuStatus == DanMuStatus.Waitting)
			{
				m_DanMuStatus = DanMuStatus.Running;
			}
		}

		public void PuaseRunning()
		{
			if (m_DanMuStatus == DanMuStatus.Running)
			{
				m_DanMuStatus = DanMuStatus.Waitting;
			}
		}

		public float GetEndDistance()
		{
			return 0f - m_SelfRect.anchoredPosition.x - m_SelfRect.sizeDelta.x;
		}

		public void Clear()
		{
			Title.gameObject.SetActive(value: false);
			m_ContentLayoutGroup.padding.left = 20;
		}
	}
}
