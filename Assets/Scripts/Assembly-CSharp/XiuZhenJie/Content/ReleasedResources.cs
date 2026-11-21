using UnityEngine;
using XiuZhenJie.Client.Release;

namespace XiuZhenJie
{
    internal static class ReleasedResources
    {
        internal static T Load<T>(string path) where T : Object
        {
            return ReleaseRuntime.LoadContent("resources/" + path, typeof(T)) as T ?? Resources.Load<T>(path);
        }
    }

    // Existing gameplay calls resolve here. The recovered resource DLL stays behind the same API.
    public sealed class ResourceManager : Singleton<ResourceManager>
    {
        private static global::ResourceManager Legacy => Singleton<global::ResourceManager>.Instance;

        public T LoadResource<T>(string path) where T : Object
        {
            return ReleaseRuntime.LoadContent(path, typeof(T)) as T ?? Legacy.LoadResource<T>(path);
        }

        public void Init(MonoBehaviour host) => Legacy.Init(host);

        public void ClearCache() => Legacy.ClearCache();

        public void PreloadRes(string path)
        {
            if (ReleaseRuntime.LoadContent(path, typeof(Object)) == null)
                Legacy.PreloadRes(path);
        }

        public void AsyncLoadResource(string path, OnAsyncObjFinish completion, LoadResPriority priority, bool isSprite = false)
        {
            Object updated = ReleaseRuntime.LoadContent(path, isSprite ? typeof(Sprite) : typeof(Object));
            if (updated != null)
                completion(path, updated, null, null, null);
            else
                Legacy.AsyncLoadResource(path, completion, priority, isSprite);
        }
    }
}
