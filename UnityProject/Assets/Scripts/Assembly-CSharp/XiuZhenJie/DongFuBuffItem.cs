using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace XiuZhenJie
{
	public class DongFuBuffItem : MonoBehaviour
	{
		public Image ZhuJiIcon;

		public Image LeiJieIcon;

		public Image DanDuIcon;

		public Image JingMaiShouSunIcon;

		public Image DuJieFailIcon;

		public Text ShengYuTimeText;

		private float m_ShengYuTime;

		public void SetItem(DongFuBuffType dongFuBuffType, UnityAction BtnCallBack)
		{
			switch (dongFuBuffType)
			{
			case DongFuBuffType.ZhuJi:
				ZhuJiIcon.gameObject.SetActive(value: true);
				break;
			case DongFuBuffType.DuJie:
				LeiJieIcon.gameObject.SetActive(value: true);
				break;
			case DongFuBuffType.DanDu:
				DanDuIcon.gameObject.SetActive(value: true);
				break;
			case DongFuBuffType.JingMai:
				JingMaiShouSunIcon.gameObject.SetActive(value: true);
				break;
			case DongFuBuffType.DuJieShiBai:
				DuJieFailIcon.gameObject.SetActive(value: true);
				break;
			}
			ShengYuTimeText.text = "无限制";
			Button component = base.gameObject.GetComponent<Button>();
			component.onClick.RemoveAllListeners();
			component.onClick.AddListener(() =>
			{
				if (BtnCallBack != null)
				{
					BtnCallBack();
				}
			});
		}

		private void Update()
		{
			if (m_ShengYuTime > 0f)
			{
				m_ShengYuTime -= Time.deltaTime;
				float num = Mathf.Floor(m_ShengYuTime / 3600f);
				float num2 = Mathf.Floor((m_ShengYuTime - num * 3600f) / 60f);
				ShengYuTimeText.text = string.Format("{0}:{1}", num.ToString("0"), num2.ToString("0"));
			}
		}
	}
}
