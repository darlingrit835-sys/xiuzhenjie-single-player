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
			ShowLoadingWindow(LoadWnd, SceneName);
		}

		public void LoadScene(string LoadWnd, string SceneName, params object[] args)
		{
			LoadSceneEnterCallBack = null;
			LoadSceneOverCallBack = null;
			ShowLoadingWindow(LoadWnd, SceneName);
			Singleton<UIManager>.Instance.SendMessageToWnd(LoadWnd, UIMsgID.Data, args);
		}

		private void ShowLoadingWindow(string windowName, string sceneName)
		{
			LoadingProgress = 0;
			UnityAction beginLoad = () => m_Mono.StartCoroutine(LoadSceneAsync(sceneName));
			Singleton<UIManager>.Instance.PopUpWnd(windowName, true, sceneName, beginLoad);
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
			// Give the loading view a frame before synchronous cache cleanup and scene preparation.
			yield return null;
			ClearCache();
			PlayBgMusicBySceneName(name);
			if (LoadSceneEnterCallBack != null)
			{
				LoadSceneEnterCallBack();
			}
			AlreadyLoadScene = false;
			AsyncOperation unLoadScene = XiuZhenJie.Client.Release.ReleaseRuntime.LoadScene("Empty", LoadSceneMode.Single);
			if (unLoadScene != null)
				yield return unLoadScene;
			LoadingProgress = 0;
			AsyncOperation asyncScene = XiuZhenJie.Client.Release.ReleaseRuntime.LoadScene(name);
			if (asyncScene == null)
				yield break;
			if (!asyncScene.isDone)
				asyncScene.allowSceneActivation = false;
			while (!asyncScene.isDone && asyncScene.progress < 0.9f)
			{
				LoadingProgress = Mathf.FloorToInt(asyncScene.progress * 100f);
				yield return null;
			}
			LoadingProgress = 90;
			SetSceneSetting(name);
			if (!asyncScene.isDone)
			{
				asyncScene.allowSceneActivation = true;
				yield return asyncScene;
			}
			CurrentMapName = name;
			AlreadyLoadScene = true;
			LoadingProgress = 100;
			// Finish scene-owned setup, including the battle HUD, before the next window update.
			LoadSceneOverCallBack?.Invoke();
		}

		private void ClearCache()
		{
			Singleton<ObjectManager>.Instance.ClearCache();
			Singleton<ResourceManager>.Instance.ClearCache();
		}
	}
}
