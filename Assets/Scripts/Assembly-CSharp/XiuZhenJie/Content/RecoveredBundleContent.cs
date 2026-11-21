using System;
using System.Collections.Generic;
using UnityEngine;
using XiuZhenJie.Client.Release;

namespace XiuZhenJie
{
	internal sealed class RecoveredBundleContent : IContentBootstrap, IWindowContent
	{
		internal static readonly RecoveredBundleContent Default = new RecoveredBundleContent(null);

		private readonly Material[] m_FallbackMaterials;
		private readonly PackagedBundlePreparation m_Preparation = new PackagedBundlePreparation();
		private MonoBehaviour m_Host;
		private bool m_PackagedMounted;
		private readonly HashSet<GameObject> m_ReleasedWindows = new HashSet<GameObject>();

		internal RecoveredBundleContent(Material[] fallbackMaterials)
		{
			m_FallbackMaterials = fallbackMaterials;
		}

		public void Initialize(MonoBehaviour host, Transform recycleRoot, Transform sceneRoot)
		{
			m_Host = host;
			m_PackagedMounted = false;
			Singleton<ResourceManager>.Instance.Init(host);
			Singleton<ObjectManager>.Instance.Init(recycleRoot, sceneRoot);
		}

		public void Prepare(Action onReady, Action<string> onError)
		{
			if (m_Preparation.IsPreparing)
				return;

			// Returning to the login scene keeps the packaged index mounted in this process.
			if (Constants.UsesPackagedTrialData && m_PackagedMounted)
			{
				onReady();
				return;
			}

			m_PackagedMounted = false;
			m_Host.StartCoroutine(m_Preparation.Run(() =>
			{
				if (!Constants.UsesPackagedTrialData)
					Singleton<HotPatchManager>.Instance.Init(m_Host);
				onReady();
			}, onError));
		}

		public bool IsPreparing => m_Preparation.IsPreparing;
		public float PreparationProgress => m_Preparation.Progress;

		public void Mount()
		{
			bool packaged = Constants.UsesPackagedTrialData;
			if (packaged && m_PackagedMounted)
				return;

			m_PackagedMounted = false;
			AssetBundleManager bundles = Singleton<AssetBundleManager>.Instance;
			if (!bundles.LoadAssetBundleConfig())
				return;

			if (m_FallbackMaterials != null && m_FallbackMaterials.Length > 0)
			{
				// Keep these shared materials alive for the prefabs that reference them later.
				uint key = Crc32.GetCrc32(Constants.GUIDE_Material);
				AssetBundle bundle = bundles.LoadResouceAssetBundle(key)?.m_AssetBundle;
				if (bundle == null)
					return;

				foreach (Material fallback in m_FallbackMaterials)
				{
					if (fallback == null)
						continue;

					Material material = bundle.LoadAsset<Material>(fallback.name);
					if (material != null && (material.shader == null || !material.shader.isSupported || material.shader.name == "Hidden/InternalErrorShader"))
						material.shader = fallback.shader;
				}
			}

			m_PackagedMounted = packaged;
		}

		public GameObject Instantiate(string assetPrefix, string windowName, bool builtIn)
		{
			// The Resources variant inherits the authored street and its original shop Button bindings.
			var updated = windowName == Constants.ZHONGJIEFANGSHI_WINDOW
				? ReleasedResources.Load<GameObject>("prefab/window/ZhongJieFangShiWnd")
				: ReleaseRuntime.LoadContent(assetPrefix + windowName, typeof(GameObject)) as GameObject;
			if (updated != null)
			{
				GameObject view = UnityEngine.Object.Instantiate(updated);
				m_ReleasedWindows.Add(view);
				return view;
			}
			if (!builtIn)
				return Singleton<ObjectManager>.Instance.InstantiateObject(assetPrefix + windowName, setSceneObj: false, bClear: false);

			GameObject prefab = ReleasedResources.Load<GameObject>(("Prefab/Window/" + windowName).Replace(".prefab", ""));
			return prefab == null ? null : UnityEngine.Object.Instantiate(prefab);
		}

		public void Release(GameObject view, bool builtIn, bool destroy)
		{
			if (m_ReleasedWindows.Remove(view) || builtIn)
				UnityEngine.Object.Destroy(view);
			else if (destroy)
				Singleton<ObjectManager>.Instance.ReleaseObject(view, 0, destoryCache: true);
			else
				Singleton<ObjectManager>.Instance.ReleaseObject(view, -1, destoryCache: false, recycleParent: false);
		}

		public Sprite LoadSprite(string path) => Singleton<ResourceManager>.Instance.LoadResource<Sprite>(path);
		public void Preload(string assetPath) => Singleton<ResourceManager>.Instance.PreloadRes(assetPath);

		public void LoadSpriteAsync(string path, Action<Sprite> onLoaded)
		{
			Singleton<ResourceManager>.Instance.AsyncLoadResource(path, (_, asset, _, _, _) => onLoaded(asset as Sprite),
				LoadResPriority.RES_MIDDLE, isSprite: true);
		}
	}
}
