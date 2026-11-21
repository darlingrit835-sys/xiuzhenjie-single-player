using UnityEngine;

public class GuideZhiBiaoItme : MonoBehaviour
{
	public int FangXiaog;

	public float ShuDo = -0.3f;

	private int Shu;

	private RectTransform m_this;

	private Vector2 TheInitialCoordinate;

	private bool m_IsBegin;

	private float m_Times = 1f;

	private float m_AddTime;

	private void Start()
	{
		m_this = base.gameObject.GetComponent<RectTransform>();
		TheInitialCoordinate = m_this.anchoredPosition;
		m_AddTime = Time.deltaTime;
	}

	private void Update()
	{
		if (m_IsBegin)
		{
			m_Times -= ((Time.deltaTime == 0f) ? m_AddTime : Time.deltaTime);
			if (m_Times <= 0f)
			{
				YansHi();
				m_IsBegin = false;
				m_Times = 1f;
			}
		}
		switch (FangXiaog)
		{
		case 1:
			WeiZhiY();
			break;
		case 2:
			WeiZhiX();
			break;
		case 3:
			DirectionalChange(3);
			break;
		case 4:
			DirectionalChange(4);
			break;
		}
	}

	private void WeiZhiY()
	{
		if (Shu < 40)
		{
			base.transform.Translate(Vector3.up * (0f - ShuDo), Space.World);
		}
		if (Shu > 40)
		{
			base.transform.Translate(Vector3.up * ShuDo, Space.World);
		}
		if (Shu == 80)
		{
			Shu = 0;
		}
		Shu++;
	}

	private void WeiZhiX()
	{
		if (Shu < 40)
		{
			base.transform.Translate(Vector3.right * (0f - ShuDo), Space.World);
		}
		if (Shu > 40)
		{
			base.transform.Translate(Vector3.right * ShuDo, Space.World);
		}
		if (Shu == 80)
		{
			Shu = 0;
		}
		Shu++;
	}

	public void DirectionalChange(int direction)
	{
		switch (direction)
		{
		case 3:
			if (Shu < 120)
			{
				RectTransform component3 = base.gameObject.GetComponent<RectTransform>();
				component3.anchoredPosition = new Vector2(component3.anchoredPosition.x + ShuDo, component3.anchoredPosition.y);
			}
			else
			{
				RectTransform component4 = base.gameObject.GetComponent<RectTransform>();
				component4.anchoredPosition = new Vector2(component4.anchoredPosition.x + 999f, component4.anchoredPosition.y + 999f);
				m_IsBegin = true;
			}
			break;
		case 4:
			if (Shu < 120)
			{
				RectTransform component = base.gameObject.GetComponent<RectTransform>();
				component.anchoredPosition = new Vector2(component.anchoredPosition.x, component.anchoredPosition.y + ShuDo);
			}
			else
			{
				RectTransform component2 = base.gameObject.GetComponent<RectTransform>();
				component2.anchoredPosition = new Vector2(component2.anchoredPosition.x + 999f, component2.anchoredPosition.y + 999f);
				m_IsBegin = true;
			}
			break;
		}
		Shu++;
	}

	private void YansHi()
	{
		base.gameObject.GetComponent<RectTransform>().anchoredPosition = TheInitialCoordinate;
		Shu = 0;
	}
}
