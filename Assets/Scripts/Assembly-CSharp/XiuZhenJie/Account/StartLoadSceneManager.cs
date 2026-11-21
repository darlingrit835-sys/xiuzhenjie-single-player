using System;
using System.Collections.Generic;
using UnityEngine;

namespace XiuZhenJie
{
	public class StartLoadSceneManager : MonoSingleton<StartLoadSceneManager>
	{
		public GameObject bigXianHe;

		public GameObject bigbigXianHe;

		public float bigXianHeSpeed;

		public GameObject smallsmallXianHe;

		public GameObject smallXianHe;

		public GameObject StartLoadWnd;

		public float smallXianHeSpeed;

		public AssetBundleManager abManager;

		public List<string> pathList;

		public Action<string> DownLoadResourceCompeleteEvent;

		public Action<float> DownLoadProgressFloatEvent;

		public int ResourceFileCount => pathList.Count;

		public new void Awake()
		{
			base.Awake();
		}

		public void Start()
		{
			abManager = Singleton<AssetBundleManager>.Instance;
			pathList = new List<string>();
			bigXianHeSpeed = -0.5f;
			smallXianHeSpeed = -0.1f;
		}

		public void StartLoad()
		{
			StartLoadWnd?.SetActive(value: true);
		}

		public void BigXianHeFlying()
		{
			bigXianHe.transform.Translate(new Vector2(Time.deltaTime * bigXianHeSpeed, 0f));
			bigbigXianHe.transform.Translate(new Vector2(Time.deltaTime * bigXianHeSpeed, 0f));
		}

		public void SmallXianHeFlying()
		{
			smallXianHe.transform.Translate(new Vector2(Time.deltaTime * smallXianHeSpeed, 0f));
			smallsmallXianHe.transform.Translate(new Vector2(Time.deltaTime * smallXianHeSpeed, 0f));
		}
	}
}
