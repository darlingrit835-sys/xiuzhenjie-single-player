using System;
using UnityEngine;

namespace XiuZhenJie
{
	public class GameRoot : MonoSingleton<GameRoot>
	{
		private bool IsTiming;

		private float CountDown;

		public void Start()
		{
			Application.targetFrameRate = 60;
			UnityEngine.Object.DontDestroyOnLoad(this);
			Screen.sleepTimeout = -1;
		}

		private void Update()
		{
			EixtDetection();
		}

		private void EixtDetection()
		{
			if (Input.GetKeyDown(KeyCode.Escape))
			{
				if (CountDown == 0f)
				{
					CountDown += Time.time;
					IsTiming = true;
				}
				else
				{
					ExitApplication();
				}
			}
			if (IsTiming && (double)(Time.time - CountDown) > 2.0)
			{
				CountDown = 0f;
				IsTiming = false;
			}
		}

		public void SpeedHackDetector()
		{
			try
			{
				MonoSingleton<GameStart>.instance.PopUpAttention("检测到客户端异常", () =>
				{
					Debug.Log("检测到使用加速器");
					ExitApplication();
				});
			}
			catch (Exception ex)
			{
				Debug.Log("检测到使用加速器" + ex.Message);
				ExitApplication();
			}
		}

		public void AndroidTestReceive(string args)
		{
			string[] array = (string.IsNullOrEmpty(args) ? new string[0] : args.Split(','));
			if (array.Length != 0)
			{
				ReceivePlatformSdkMsg(array);
			}
		}

		private void ReceivePlatformSdkMsg(string[] platformParams)
		{
		}

		public void ExitApplication()
		{
			Application.Quit();
		}

		public void OnDisable()
		{
		}

		public void OnDestroy()
		{
		}
	}
}
