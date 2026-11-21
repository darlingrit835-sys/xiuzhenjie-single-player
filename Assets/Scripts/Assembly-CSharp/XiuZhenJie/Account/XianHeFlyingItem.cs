using UnityEngine;
using UnityEngine.UI;

namespace XiuZhenJie
{
	public class XianHeFlyingItem : MonoBehaviour
	{
		public Image XianHe_Yi;

		public Image XianHe_Er;

		private XianHeFlyingStatus m_Status;

		private float m_Timing;

		private float m_FixTiming;

		private float m_Speed;

		public Sprite[] XianHeSprites;

		private int BigXianHeIndex;

		private int SmallXianHeIndex;

		private RectTransform XianHe_YiRect;

		private RectTransform XianHe_ErRect;

		private void Awake()
		{
			m_Timing = 0f;
			m_FixTiming = 0f;
			m_Status = XianHeFlyingStatus.Go;
			m_Speed = 0.008f;
			BigXianHeIndex = 0;
			SmallXianHeIndex = 2;
			XianHe_Yi.sprite = XianHeSprites[BigXianHeIndex];
			XianHe_Er.sprite = XianHeSprites[SmallXianHeIndex];
			XianHe_YiRect = base.gameObject.GetComponent<RectTransform>();
			XianHe_ErRect = XianHe_Er.gameObject.GetComponent<RectTransform>();
		}

		private void Update()
		{
			m_Timing += Time.deltaTime;
			base.gameObject.transform.Translate(0f - m_Speed, 0f, 0f, Space.Self);
			if (m_Status == XianHeFlyingStatus.Go && base.gameObject.transform.localPosition.x <= -375f)
			{
				m_Status = XianHeFlyingStatus.Return;
				base.gameObject.transform.localEulerAngles = new Vector3(0f, 180f, 0f);
				XianHe_YiRect.anchoredPosition = new Vector3(-375f, 110f, -0.2f);
				XianHe_YiRect.sizeDelta = new Vector2(50f, 50f);
				XianHe_ErRect.sizeDelta = new Vector2(25f, 25f);
				m_Speed = 0.003f;
			}
			else if (m_Status == XianHeFlyingStatus.Return && base.gameObject.transform.localPosition.x > 375f)
			{
				m_Status = XianHeFlyingStatus.Go;
				base.gameObject.transform.localEulerAngles = new Vector3(0f, 0f, 0f);
				XianHe_YiRect.anchoredPosition = new Vector3(375f, 100f, -0.2f);
				XianHe_YiRect.sizeDelta = new Vector2(100f, 100f);
				XianHe_ErRect.sizeDelta = new Vector2(50f, 50f);
				m_Speed = 0.008f;
			}
		}

		private void FixedUpdate()
		{
			m_FixTiming += Time.fixedDeltaTime;
			if (m_FixTiming > 0.5f)
			{
				BigXianHeIndex++;
				SmallXianHeIndex++;
				if (BigXianHeIndex >= 3)
				{
					BigXianHeIndex = 0;
				}
				if (SmallXianHeIndex >= 3)
				{
					SmallXianHeIndex = 0;
				}
				XianHe_Yi.sprite = XianHeSprites[BigXianHeIndex];
				XianHe_Er.sprite = XianHeSprites[SmallXianHeIndex];
				m_FixTiming = 0f;
			}
		}
	}
}
