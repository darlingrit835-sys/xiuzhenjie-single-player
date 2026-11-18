using System.Collections;
using UnityEngine;
using UnityEngine.UI;

namespace XiuZhenJie
{
	public class XuLiProgressPanel : MonoBehaviour
	{
		public Image BiaoShiImg;

		public Image BiaoShiBg;

		private float maxValue;

		private float minValue;

		private bool IsStopXuLi;

		private float totalTime;

		private IEnumerator xuLiIE;

		private float leatestTime;

		private Monster _monster;

		public float TotalTime
		{
			get
			{
				return totalTime;
			}
			private set
			{
				totalTime = value;
			}
		}

		public float LeatestTime
		{
			get
			{
				return leatestTime;
			}
			private set
			{
				leatestTime = value;
			}
		}

		private void Start()
		{
			Singleton<UIManager>.Instance.FindWndByName<ZhanDouWnd>("ZhanDou.prefab");
		}

		private void Update()
		{
			if (_monster != null && !_monster.IsAlive && xuLiIE != null)
			{
				StopCoroutine(xuLiIE);
				xuLiIE = null;
				BiaoShiBg.fillAmount = 0f;
				base.gameObject.SetActive(value: false);
			}
		}

		private void FixedUpdate()
		{
			if (IsStopXuLi)
			{
				BiaoShiBg.fillAmount += Time.fixedDeltaTime * (1f / totalTime);
			}
		}

		public void SetValue(float maxValue, float minValue, float GongFaConversionRate)
		{
			BiaoShiBg.fillAmount = 0f;
			TotalTime = maxValue / (GongFaConversionRate * (float)Singleton<RoleManager>.Instance.Role_Entity.SuperAir / 10f);
			LeatestTime = minValue / (GongFaConversionRate * (float)Singleton<RoleManager>.Instance.Role_Entity.SuperAir / 10f);
			IsStopXuLi = true;
			this.maxValue = maxValue;
			this.minValue = minValue;
			float y = BiaoShiBg.gameObject.GetComponent<RectTransform>().sizeDelta.y;
			float num = minValue / maxValue;
			y *= num;
			Vector2 anchoredPosition = BiaoShiImg.gameObject.GetComponent<RectTransform>().anchoredPosition;
			BiaoShiImg.gameObject.GetComponent<RectTransform>().anchoredPosition = new Vector2(anchoredPosition.x, y);
			base.gameObject.SetActive(value: true);
		}

		public float GetZhuanYuan()
		{
			IsStopXuLi = false;
			float num = minValue / maxValue;
			float fillAmount = BiaoShiBg.fillAmount;
			BiaoShiBg.fillAmount = 0f;
			base.gameObject.SetActive(value: false);
			if (fillAmount >= num)
			{
				return minValue + (maxValue - minValue) * (fillAmount - num);
			}
			return 0f;
		}

		private void OnDisable()
		{
			if (xuLiIE != null)
			{
				StopCoroutine(xuLiIE);
			}
		}
	}
}
