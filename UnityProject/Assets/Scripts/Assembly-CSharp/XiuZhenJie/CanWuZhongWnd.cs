using System.Collections;
using UnityEngine;

namespace XiuZhenJie
{
	public class CanWuZhongWnd : Window
	{
		public CanWuZhongPanel Panel;

		private const int m_CanWuSeconds = 10;

		private bool m_IsWait = true;

		private IEnumerator m_DaoShuMethod;

		public override void Awake(params object[] paralist)
		{
			base.Awake(paralist);
			Panel = base.GameObject.GetComponent<CanWuZhongPanel>();
			AddButtonClickListener(Panel.CompeleteBtn, CompeleteBtnClick);
		}

		public override void OnShow(params object[] paralist)
		{
			base.OnShow(paralist);
			m_DaoShuMethod = DaoShuSanMiao();
			StartCanWu();
		}

		public override void OnDisable()
		{
			base.OnDisable();
			MonoSingleton<GameStart>.Instance.m_mono.StopCoroutine(m_DaoShuMethod);
		}

		private void ShowGuide(GuideType _type, int _index)
		{
			MonoSingleton<GameStart>.Instance.StartCoroutine(delayShowGuide(_type, _index));
		}

		private IEnumerator delayShowGuide(GuideType _type, int _index)
		{
			yield return new WaitForEndOfFrame();
			if (Singleton<GuideManager>.Instance.IsCanShowGuide(_type, _index))
			{
				Singleton<GuideManager>.Instance.ShowGuide(_type, _index);
			}
		}

		private void StartCanWu()
		{
			Panel.ProgressBg.gameObject.SetActive(value: true);
			Panel.CanWuTitleImg.gameObject.SetActive(value: true);
			Singleton<UIManager>.Instance.m_Mono.StartCoroutine(DaoShuSanShiMiao());
		}

		private void EndCanWu()
		{
			Singleton<UIManager>.Instance.PopUpWnd("DongFuWnd.prefab", true);
		}

		private IEnumerator DaoShuSanShiMiao()
		{
			Panel.ProgressText.text = "10";
			for (float time = 0f; time / 1000f <= 10f; time += 100f)
			{
				float size = time / 1000f / 10f;
				Panel.ProgressBar.size = size;
				Panel.ProgressText.text = (10f - time / 1000f).ToString("f1");
				yield return new WaitForSeconds(0.1f);
			}
			Panel.CanWuTitleText.text = "參悟完成";
			Panel.ProgressBg.gameObject.SetActive(value: false);
			Panel.CompeleteBtn.gameObject.SetActive(value: true);
			Singleton<UIManager>.Instance.m_Mono.StartCoroutine(m_DaoShuMethod);
		}

		private IEnumerator DaoShuSanMiao()
		{
			int i = 0;
			while (m_IsWait && i < 3)
			{
				i++;
				Panel.DaoShuText.text = 3 - i + "秒";
				yield return new WaitForSeconds(1f);
			}
			CompeleteBtnClick();
		}

		public void CompeleteBtnClick()
		{
			m_IsWait = false;
			EndCanWu();
			Singleton<UIManager>.Instance.ShowWnd("DongFuWnd.prefab", true);
			Singleton<UIManager>.Instance.ShowWnd("CanWuWnd.prefab", true);
			Singleton<UIManager>.Instance.CloseWnd(this);
			if (m_DaoShuMethod != null)
			{
				Singleton<UIManager>.Instance.m_Mono.StopCoroutine(m_DaoShuMethod);
			}
			ShowGuide(GuideType.Learn, 7);
		}
	}
}
