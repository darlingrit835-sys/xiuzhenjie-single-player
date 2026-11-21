using System.Collections;
using UnityEngine;

namespace XiuZhenJie
{
	public class CanWuZhongWnd : Window
	{
		public CanWuZhongPanel Panel;
		private Coroutine meditation;
		private bool finished;

		public override void Awake(params object[] paralist)
		{
			base.Awake(paralist);
			Panel = base.GameObject.GetComponent<CanWuZhongPanel>();
			InkCultivationPages.Meditation(Panel);
			AddButtonClickListener(Panel.CompeleteBtn, CompeleteBtnClick);
		}

		public override void OnShow(params object[] paralist)
		{
			base.OnShow(paralist);
			StopMeditation();
			finished = false;
			meditation = Panel.StartCoroutine(Meditate());
		}

		public override void OnDisable()
		{
			StopMeditation();
			base.OnDisable();
		}

		private void StopMeditation()
		{
			if (meditation != null && Panel != null) Panel.StopCoroutine(meditation);
			meditation = null;
		}

		private IEnumerator Meditate()
		{
			const float duration = 10f;
			Panel.CanWuTitleText.text = "凝神参悟";
			Panel.CompeleteBtn.gameObject.SetActive(false);
			Panel.ProgressBg.gameObject.SetActive(true);
			Panel.CanWuTitleImg.gameObject.SetActive(true);
			Panel.ProgressBar.interactable = false;
			float start = Time.time;
			while (Time.time - start < duration)
			{
				float elapsed = Time.time - start;
				Panel.ProgressBar.size = Mathf.Clamp01(elapsed / duration);
				Panel.ProgressText.text = (duration - elapsed).ToString("0.0");
				yield return new WaitForSeconds(.1f);
			}
			Panel.CanWuTitleText.text = "参悟完成";
			Panel.ProgressBg.gameObject.SetActive(false);
			Panel.CompeleteBtn.gameObject.SetActive(true);
			for (int remaining = 3; remaining > 0; remaining--)
			{
				Panel.DaoShuText.text = remaining + "秒";
				yield return new WaitForSeconds(1);
			}
			meditation = null;
			CompeleteBtnClick();
		}

		public void CompeleteBtnClick()
		{
			if (finished) return;
			finished = true;
			StopMeditation();
			var ui = Singleton<UIManager>.Instance;
			ui.PopUpWnd("DongFuWnd.prefab", true);
			ui.ShowWnd("DongFuWnd.prefab", true);
			ui.ShowWnd("CanWuWnd.prefab", true);
			ui.CloseWnd(this);
			MonoSingleton<GameStart>.Instance.StartCoroutine(ShowGuide());
		}

		private IEnumerator ShowGuide()
		{
			yield return new WaitForEndOfFrame();
			var guide = Singleton<GuideManager>.Instance;
			if (guide.IsCanShowGuide(GuideType.Learn, 7)) guide.ShowGuide(GuideType.Learn, 7);
		}
	}
}
