using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;

namespace XiuZhenJie
{
	public class GameMapManager : Singleton<GameMapManager>
	{
		public UnityAction LoadSceneOverCallBack;

		public UnityAction LoadSceneEnterCallBack;

		public static int LoadingProgress;

		private MonoBehaviour m_Mono;

		public string CurrentMapName { get; set; }

		public bool AlreadyLoadScene { get; set; }

		public void Init(MonoBehaviour mono)
		{
			m_Mono = mono;
		}

		public void LoadScene(string LoadWnd, string SceneName, UnityAction EnterCallBack = null, UnityAction EndCallBack = null)
		{
			LoadSceneEnterCallBack = EnterCallBack;
			LoadSceneOverCallBack = EndCallBack;
			Singleton<ResourceManager>.Instance.ClearCache();
			GC.Collect();
			PlayBgMusicBySceneName(SceneName);
			(Singleton<UIManager>.Instance.PopUpWnd("MaskWnd.prefab", true) as MaskWnd)?.ToBlack(0.3f, () =>
			{
				LoadingProgress = 0;
				Singleton<UIManager>.Instance.CloseWnd("MaskWnd.prefab");
				Singleton<UIManager>.Instance.PopUpWnd(LoadWnd, true, SceneName, (UnityAction)(() =>
				{
					m_Mono.StartCoroutine(LoadSceneAsync(SceneName));
				}));
			});
		}

		public void LoadScene(string LoadWnd, string SceneName, params object[] args)
		{
			LoadSceneEnterCallBack = null;
			LoadSceneOverCallBack = null;
			Singleton<ResourceManager>.Instance.ClearCache();
			GC.Collect();
			PlayBgMusicBySceneName(SceneName);
			(Singleton<UIManager>.Instance.PopUpWnd("MaskWnd.prefab", true) as MaskWnd)?.ToBlack(0.3f, () =>
			{
				LoadingProgress = 0;
				Singleton<UIManager>.Instance.CloseWnd("MaskWnd.prefab");
				Singleton<UIManager>.Instance.PopUpWnd(LoadWnd, true, SceneName, (UnityAction)(() =>
				{
					m_Mono.StartCoroutine(LoadSceneAsync(SceneName));
				}));
				Singleton<UIManager>.Instance.SendMessageToWnd(LoadWnd, UIMsgID.Data, args);
			});
		}

		private void PlayBgMusicBySceneName(string sceneName)
		{
			switch (sceneName)
			{
			case "DongFuScene":
				Singleton<VoiceManager>.Instance.SwitchBgMusic(BackGroundMusicType.DongFu);
				break;
			case "MainScene":
				Singleton<VoiceManager>.Instance.SwitchBgMusic(BackGroundMusicType.Main);
				break;
			case "FightingScene":
				Singleton<VoiceManager>.Instance.SwitchBgMusic(BackGroundMusicType.Battle);
				break;
			default:
				Singleton<VoiceManager>.Instance.StopMusic();
				break;
			}
		}

		private void SetSceneSetting(string name)
		{
			switch (name)
			{
			case "MainScene":
				Singleton<ResourceManager>.Instance.PreloadRes("Assets/GameData/Prefabs/Items/ChuanXinItem.prefab");
				Singleton<ResourceManager>.Instance.PreloadRes("Assets/GameData/Prefabs/Items/HuoDongItem.prefab");
				Singleton<ResourceManager>.Instance.PreloadRes("Assets/GameData/Prefabs/Items/DuiHuanXianYuItem.prefab");
				Singleton<ResourceManager>.Instance.PreloadRes("Assets/GameData/Prefabs/Items/ChuanXinGoodsItem.prefab");
				break;
			case "DongFuScene":
				Singleton<ResourceManager>.Instance.PreloadRes("Assets/GameData/Prefabs/Items/GengHuanGongFaItem.prefab");
				Singleton<ResourceManager>.Instance.PreloadRes("Assets/GameData/Prefabs/Items/DongFuItem.prefab");
				break;
			case "FightingScene":
				Singleton<BattleManager>.Instance.MapPrefabLoad();
				break;
			}
		}

		private IEnumerator LoadSceneAsync(string name)
		{
#if UNITY_EDITOR
			object nextFrame = null;
#else
			var nextFrame = new WaitForEndOfFrame();
#endif
			ClearCache();
			if (LoadSceneEnterCallBack != null)
			{
				LoadSceneEnterCallBack();
			}
			AlreadyLoadScene = false;
			AsyncOperation unLoadScene = SceneManager.LoadSceneAsync("Empty", LoadSceneMode.Single);
			while (unLoadScene != null && !unLoadScene.isDone)
			{
				yield return nextFrame;
			}
			LoadingProgress = 0;
			AsyncOperation asyncScene = SceneManager.LoadSceneAsync(name);
			if (asyncScene == null || asyncScene.isDone)
			{
				yield break;
			}
			asyncScene.allowSceneActivation = false;
			int targetProgress;
			while (asyncScene.progress < 0.9f)
			{
				targetProgress = (int)(asyncScene.progress * 100);
				yield return nextFrame;
				while (LoadingProgress < targetProgress)
				{
					LoadingProgress++;
					yield return nextFrame;
				}
			}
			SetSceneSetting(name);
			targetProgress = 100;
			while (LoadingProgress < targetProgress - 2)
			{
				LoadingProgress++;
				yield return nextFrame;
			}
			LoadingProgress = 100;
			asyncScene.allowSceneActivation = true;
			AlreadyLoadScene = true;
			CurrentMapName = name;
			yield return new WaitForSeconds(0.05f);
			if (LoadSceneOverCallBack != null)
			{
				LoadSceneOverCallBack();
			}
			yield return nextFrame;
		}

		private void ClearCache()
		{
			Singleton<ObjectManager>.Instance.ClearCache();
			Singleton<ResourceManager>.Instance.ClearCache();
		}
	}
}
