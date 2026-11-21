using System;
using UnityEngine;

namespace XiuZhenJie
{
	internal interface IContentBootstrap
	{
		void Initialize(MonoBehaviour host, Transform recycleRoot, Transform sceneRoot);
		void Prepare(Action onReady, Action<string> onError);
		bool IsPreparing { get; }
		float PreparationProgress { get; }
		void Mount();
	}
}
