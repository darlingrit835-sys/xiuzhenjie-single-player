using UnityEngine;

namespace XiuZhenJie
{
	public class DiHuoGe_LianDanManager : MonoSingleton<DiHuoGe_LianDanManager>
	{
		public GameObject DanLuBg;

		public GameObject ChengDanFire;

		public GameObject SmallFire;

		public GameObject BigFire;

		public GameObject MiddleFire;

		public GameObject Mask;

		private FireStatus m_FireStatus;

		private float m_DevHeight = 9.6f;

		private float m_DevWidth = 6.4f;

		private void Start()
		{
			m_FireStatus = FireStatus.None;
			_ = Screen.height;
			float orthographicSize = GetComponent<Camera>().orthographicSize;
			float num = (float)Screen.width * 1f / (float)Screen.height;
			if (orthographicSize * 2f * num < m_DevWidth)
			{
				orthographicSize = m_DevWidth / (2f * num);
				GetComponent<Camera>().orthographicSize = orthographicSize;
			}
		}

		public void ChooseFire(FireStatus status)
		{
			m_FireStatus = status;
			switch (m_FireStatus)
			{
			case FireStatus.None:
				ChengDanFire.gameObject.SetActive(value: false);
				SmallFire.gameObject.SetActive(value: false);
				Mask.gameObject.SetActive(value: false);
				break;
			case FireStatus.ChengDan:
				ChengDanFire.gameObject.SetActive(value: true);
				SmallFire.gameObject.SetActive(value: false);
				Mask.gameObject.SetActive(value: true);
				break;
			case FireStatus.Big:
			case FireStatus.Small:
			case FireStatus.Middle:
				ChengDanFire.gameObject.SetActive(value: false);
				SmallFire.gameObject.SetActive(value: true);
				Mask.gameObject.SetActive(value: true);
				break;
			}
		}

		public void ShowOrHideDanLuBg(bool isShow)
		{
			DanLuBg.gameObject.SetActive(isShow);
		}
	}
}
