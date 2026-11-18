using System;
using System.Collections.Generic;
using UnityEngine;

namespace ByteDance.Union
{
	[DisallowMultipleComponent]
	internal sealed class UnityDispatcher : MonoBehaviour
	{
		private static UnityDispatcher instance;

		private static List<Action> postTasks = new List<Action>();

		private static List<Action> executing = new List<Action>();

		private static UnityDispatcher Instance
		{
			get
			{
				CheckInstance();
				return instance;
			}
		}

		public static void PostTask(Action task, bool executeOnMainThread = true)
		{
			if (executeOnMainThread)
			{
				lock (postTasks)
				{
					postTasks.Add(task);
					return;
				}
			}
			task?.Invoke();
		}

		[RuntimeInitializeOnLoadMethod]
		private static void CheckInstance()
		{
			if (instance == null && Application.isPlaying)
			{
				GameObject obj = new GameObject("UnityDispatcher", typeof(UnityDispatcher));
				UnityEngine.Object.DontDestroyOnLoad(obj);
				instance = obj.GetComponent<UnityDispatcher>();
			}
		}

		private void Awake()
		{
			UnityEngine.Object.DontDestroyOnLoad(this);
		}

		private void OnDestroy()
		{
			postTasks.Clear();
			executing.Clear();
			instance = null;
		}

		private void Update()
		{
			lock (postTasks)
			{
				if (postTasks.Count > 0)
				{
					for (int i = 0; i < postTasks.Count; i++)
					{
						executing.Add(postTasks[i]);
					}
					postTasks.Clear();
				}
			}
			for (int j = 0; j < executing.Count; j++)
			{
				Action action = executing[j];
				try
				{
					action();
				}
				catch (Exception ex)
				{
					Debug.LogError("CSJM_Unity " + ex.Message, this);
				}
			}
			executing.Clear();
		}
	}
}
