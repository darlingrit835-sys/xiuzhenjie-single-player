using System;
using DG.Tweening;
using UnityEngine;

namespace XiuZhenJie
{
	public class DongFuSceneManager : MonoSingleton<DongFuSceneManager>
	{
		public Action SmallToBigCompeleteEvent;

		public Action BigToSmallCompeleteEvent;

		public Camera SceneCamera;

		public GameObject BackGround;

		public GameObject PeopleBg;

		public GameObject JingMaiTreeBg;

		private GameObject XueWeiBg;

		private DongFuWnd DongFuMainWnd;

		private int m_BallCount;

		private Tween smallToBigMoveAni;

		private Tween smallToBigScalAni;

		private Tween bigToSmallMoveAni;

		private Tween bigToSmallScalAni;

		private RectTransform UIRootRect;

		public Animator XiuXingAnimator;

		public void Start()
		{
			m_BallCount = 0;
		}

		public void Update()
		{
		}

		public void StartTuPo()
		{
			XiuXingAnimator.SetBool("StartTuPo", value: true);
			Timer.Register(0.5f, () =>
			{
				XiuXingAnimator.SetBool("StartTuPo", value: false);
			});
		}

		public void EndTuPo()
		{
			XiuXingAnimator.SetBool("EndTuPo", value: true);
			Timer.Register(0.5f, () =>
			{
				XiuXingAnimator.SetBool("EndTuPo", value: false);
			});
		}

		public void PeopleBgSmallToBigCompelete()
		{
			if (SmallToBigCompeleteEvent != null)
			{
				SmallToBigCompeleteEvent();
			}
		}

		public void PeopleBigToSmallCompelete()
		{
			if (BigToSmallCompeleteEvent != null)
			{
				BigToSmallCompeleteEvent();
			}
		}

		public void EnterJingMaiWnd()
		{
			XueWeiBg.gameObject.SetActive(value: true);
			PeopleBg.gameObject.SetActive(value: false);
			XueWeiBg.transform.DOScale(2f, 1f);
			DOTween.To(() => SceneCamera.orthographicSize, (float v) =>
			{
				SceneCamera.orthographicSize = v;
			}, 15f, 1f);
		}

		public void ExitJingMaiWnd()
		{
			XueWeiBg.gameObject.SetActive(value: false);
			PeopleBg.gameObject.SetActive(value: true);
			XueWeiBg.transform.DOScale(0.2914222f, 1f);
			SceneCamera.transform.localPosition = new Vector3(0f, 0f, -10f);
			SceneCamera.orthographicSize = 5f;
		}
	}
}
