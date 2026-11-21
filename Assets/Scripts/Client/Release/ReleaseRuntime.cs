using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Security.Cryptography;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;
using HybridCLR;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.SceneManagement;

namespace XiuZhenJie.Client.Release
{
    public static class ReleaseRuntime
    {
        private static AssetBundle assets;
        private static AssetBundle scenes;
        private static Dictionary<string, string> prefabAliases;

        public static int Version { get; private set; }
        public static string LegacySourceRoot { get; private set; }
        public static Assembly ContentModule { get; private set; }
        public static ReleaseChannel Channel { get; private set; }
        public static Uri Endpoint { get; private set; }
        public static bool IsActive => Version > 0;

        internal static void Reset()
        {
            Version = 0;
            LegacySourceRoot = null;
            ContentModule = null;
            Channel = null;
            Endpoint = null;
            assets = null;
            scenes = null;
            prefabAliases = null;
        }

        internal static void SetContent(ReleaseIndex index, string directory, AssetBundle content,
            AssetBundle sceneBundle, Assembly mentor, ReleaseChannel channel, Uri endpoint)
        {
            assets = content;
            scenes = sceneBundle;
            prefabAliases = content.GetAllAssetNames().Where(path => path.StartsWith("assets/gamedata/prefabs/", StringComparison.Ordinal) &&
                path.EndsWith(".prefab", StringComparison.Ordinal)).GroupBy(Path.GetFileName)
                .Where(group => group.Count() == 1).ToDictionary(group => group.Key, group => group.First(), StringComparer.Ordinal);
            LegacySourceRoot = directory;
            Version = index.version;
            ContentModule = mentor;
            Channel = channel;
            Endpoint = endpoint;
        }

        public static UnityEngine.Object LoadContent(string key, Type type)
        {
            if (assets == null || string.IsNullOrEmpty(key))
                return null;
            string path = key.Replace('\\', '/').ToLowerInvariant();
            if (!assets.Contains(path))
            {
                if (!path.StartsWith("assets/gamedata/prefabs/", StringComparison.Ordinal) ||
                    !prefabAliases.TryGetValue(Path.GetFileName(path), out path))
                    return null;
            }
            return assets.LoadAsset(path, type);
        }

        public static AsyncOperation LoadScene(string name, LoadSceneMode mode = LoadSceneMode.Single)
        {
            if (scenes == null)
                return SceneManager.LoadSceneAsync(name, mode);
            string path = scenes.GetAllScenePaths().FirstOrDefault(scene =>
                string.Equals(Path.GetFileNameWithoutExtension(scene), name, StringComparison.OrdinalIgnoreCase));
            if (path == null)
            {
                Debug.LogError("发布版本缺少场景 " + name);
                return null;
            }
            return SceneManager.LoadSceneAsync(path, mode);
        }
    }

    internal sealed class ReleaseBootstrap : MonoBehaviour
    {
        private const int ManifestLimit = 256 << 10;
        private const int SignatureLimit = 1024;
        private ReleaseLoadingView view;
        private bool codeLoaded;

        [Serializable]
        private sealed class SignedIndex
        {
            public string channelUrl;
            public string manifest;
            public string signature;
            [NonSerialized] public ReleaseIndex Index;
        }

        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.SubsystemRegistration)]
        private static void Reset() => ReleaseRuntime.Reset();

        internal void Initialize(ReleaseLoadingView loadingView)
        {
            view = loadingView;
            Screen.sleepTimeout = SleepTimeout.NeverSleep;
            DontDestroyOnLoad(gameObject);
            Begin();
        }

        private void Begin()
        {
            view.Show("正在准备游戏…", 0f);
            StartCoroutine(Load());
        }

        private IEnumerator Load()
        {
            yield return null;
            string streamingAssets = Application.streamingAssetsPath.TrimEnd('/');
            string baseUrl = streamingAssets + "/FullUpdate/";
            byte[] buffer = null;
            yield return GetBytes(baseUrl + "base.json", null, 32 << 10, value => buffer = value);
            ReleaseBase platform = ReadJson<ReleaseBase>(buffer);
            buffer = null;
            yield return GetBytes(baseUrl + "channel.json", null, 32 << 10, value => buffer = value);
            ReleaseChannel channel = ReadJson<ReleaseChannel>(buffer);
            if (platform == null || !ReleaseIndex.Digest(platform.compatibility) || platform.metadata == null ||
                channel == null || !channel.TryEndpoint(out Uri endpoint))
            {
                Fail("全量更新配置不可用。");
                yield break;
            }

            string seedUrl = streamingAssets + "/" + ReleaseIndex.SeedDirectory + "/";
            SignedIndex bundled = null;
            yield return ReadIndex(seedUrl + "manifest.json", seedUrl + "manifest.sig", channel, platform,
                value => bundled = value);
            if (bundled == null)
            {
                Fail("安装包的客户端清单校验失败。");
                yield break;
            }

            string cache = Path.Combine(Application.persistentDataPath, "FullUpdate", platform.compatibility);
            string bundledDirectory = VersionDirectory(cache, bundled.Index);
            SignedIndex cached = ReadActiveIndex(cache, channel, platform);
            SignedIndex selected = bundled;
            bool ready = false;
            if (cached != null && cached.Index.version > bundled.Index.version)
            {
                yield return UseInstalled(cache, cached.Index, value => ready = value);
                if (ready)
                    selected = cached;
            }
            if (!ready)
                yield return UseInstalled(cache, bundled.Index, value => ready = value);

            string source = selected == bundled ? "installed" : "cache";
            if (cached != null || Directory.Exists(bundledDirectory))
            {
                view.Show("正在检查更新…", .08f);
                SignedIndex remote = null;
                yield return ReadIndex(endpoint.AbsoluteUri, new Uri(endpoint, "manifest.sig").AbsoluteUri,
                    channel, platform, value => remote = value, selected.Index.version);
                if (remote != null)
                {
                    string remoteDirectory = VersionDirectory(cache, remote.Index);
                    bool updated = false;
                    yield return UseInstalled(cache, remote.Index, value => updated = value);
                    if (updated)
                        source = "cache";
                    else
                    {
                        string activeDirectory = cached == null ? bundledDirectory : VersionDirectory(cache, cached.Index);
                        yield return InstallIncremental(endpoint, channel, remote.Index, remoteDirectory,
                            activeDirectory, bundledDirectory, value => updated = value);
                        if (updated)
                            source = "incremental";
                    }
                    if (updated)
                    {
                        ready = true;
                        selected = remote;
                        SaveActiveIndex(cache, selected);
                    }
                    else
                    {
                        view.Show("更新未完成，正在启动已安装版本…", .80f);
                    }
                }
            }

            ReleaseIndex index = selected.Index;
            string directory = VersionDirectory(cache, index);
            if (!ready)
                yield return UseInstalled(cache, index, value => ready = value);
            if (ready)
                Debug.Log($"XZJ_FULL_RELEASE_REUSED version={index.version}");
            if (!ready)
            {
                if (!PrepareDirectory(cache))
                    yield break;

                view.Show("正在准备安装包资源…", .20f);
                Task installed = Task.Run(() => ReleaseFiles.InstallFromPackage(streamingAssets, directory, index));
                yield return new WaitUntil(() => installed.IsCompleted);
                if (installed.Status != TaskStatus.RanToCompletion)
                {
                    Fail("游戏资源校验或安装失败，请重试。", installed.Exception);
                    yield break;
                }
            }

#if !UNITY_EDITOR
            view.Show("正在加载运行环境…", .85f);
            foreach (string entry in platform.metadata)
            {
                if (!ReleaseIndex.SimpleName(entry) || !entry.EndsWith(".dll", StringComparison.Ordinal))
                {
                    Fail("安装包的 AOT 清单有误。");
                    yield break;
                }
                byte[] metadata = null;
                yield return GetBytes(baseUrl + "AOT/" + entry + ".bytes", null, 64 << 20, value => metadata = value);
                if (!LoadMetadata(entry, metadata))
                    yield break;
            }
#endif

            view.Show("正在加载游戏…", .89f);
            codeLoaded = true;
            Assembly mentor = LoadCode(directory);
            if (mentor == null)
                yield break;

            var assetRequest = AssetBundle.LoadFromFileAsync(Path.Combine(directory, ReleaseIndex.AssetBundle));
            yield return assetRequest;
            var sceneRequest = AssetBundle.LoadFromFileAsync(Path.Combine(directory, ReleaseIndex.SceneBundle));
            yield return sceneRequest;
            if (assetRequest.assetBundle == null || sceneRequest.assetBundle == null)
            {
                Fail("游戏资源加载失败，请重新打开游戏。");
                yield break;
            }

            ReleaseRuntime.SetContent(index, directory, assetRequest.assetBundle, sceneRequest.assetBundle, mentor, channel, endpoint);
            view.Show("正在加载游戏…", .90f);
            Debug.Log($"XZJ_FULL_RELEASE_READY version={index.version} target={platform.target} source={source}");
            AsyncOperation entryScene = ReleaseRuntime.LoadScene("StartLoadScene");
            if (entryScene == null)
            {
                Fail("版本缺少游戏入口，请重新打开游戏。");
                yield break;
            }
            yield return entryScene;
            view.gameObject.SetActive(false);
            Screen.sleepTimeout = SleepTimeout.SystemSetting;
            Destroy(gameObject);
        }

        private static string VersionDirectory(string cache, ReleaseIndex index) =>
            Path.Combine(cache, index.version + "-" + index.archiveSha256);

        private static IEnumerator UseInstalled(string cache, ReleaseIndex index, Action<bool> receive)
        {
            string directory = VersionDirectory(cache, index);
            Task<bool> local = Task.Run(() => ReleaseFiles.UseInstalled(directory, index));
            yield return new WaitUntil(() => local.IsCompleted);
            receive(local.Status == TaskStatus.RanToCompletion && local.Result);
        }

        private static SignedIndex ReadActiveIndex(string cache, ReleaseChannel channel, ReleaseBase platform)
        {
            try
            {
                string path = Path.Combine(cache, "active.json");
                if (!File.Exists(path) || new FileInfo(path).Length > (384 << 10))
                    return null;
                var cached = ReadJson<SignedIndex>(File.ReadAllBytes(path));
                if (cached == null || cached.channelUrl != channel.manifestUrl ||
                    string.IsNullOrEmpty(cached.manifest) || string.IsNullOrEmpty(cached.signature))
                    return null;
                byte[] raw = Convert.FromBase64String(cached.manifest);
                byte[] signature = Convert.FromBase64String(cached.signature);
                ReleaseIndex index = ReadJson<ReleaseIndex>(raw);
                if (!ReleaseIndex.Valid(index, platform) || !channel.Verify(raw, signature))
                    return null;
                cached.Index = index;
                return cached;
            }
            catch (Exception error) when (error is IOException || error is UnauthorizedAccessException || error is FormatException)
            {
                return null;
            }
        }

        private static void SaveActiveIndex(string cache, SignedIndex selected)
        {
            try
            {
                ReleaseFiles.WriteAtomic(Path.Combine(cache, "active.json"), Encoding.UTF8.GetBytes(JsonUtility.ToJson(selected)));
            }
            catch (Exception error) when (error is IOException || error is UnauthorizedAccessException)
            {
                Debug.LogWarning($"无法记住已安装的游戏版本（{error.GetType().Name}）。");
            }
        }

        private static IEnumerator ReadIndex(string manifestUrl, string signatureUrl, ReleaseChannel channel,
            ReleaseBase platform, Action<SignedIndex> receive, int installedVersion = 0)
        {
            bool remote = installedVersion > 0;
            ReleaseChannel transport = remote ? channel : null;
            for (int attempt = 0; attempt < (remote ? 2 : 1); attempt++)
            {
                byte[] raw = null;
                yield return GetBytes(manifestUrl, transport, ManifestLimit, value => raw = value);
                var index = ReadJson<ReleaseIndex>(raw);
                if (remote && index != null && index.target == platform.target && index.compatibility == platform.compatibility)
                    Debug.Log($"XZJ_FULL_RELEASE_CHECK installed={installedVersion} remote={index.version}");
                // The remote version only decides whether to request the signature. Installation still requires verification.
                if (index == null || index.version <= installedVersion || !ReleaseIndex.Valid(index, platform))
                    yield break;

                byte[] signature = null;
                yield return GetBytes(signatureUrl, transport, SignatureLimit, value => signature = value);
                if (!channel.Verify(raw, signature))
                    continue;
                receive(new SignedIndex
                {
                    Index = index,
                    channelUrl = channel.manifestUrl,
                    manifest = Convert.ToBase64String(raw),
                    signature = Convert.ToBase64String(signature)
                });
                yield break;
            }
        }

        private IEnumerator InstallIncremental(Uri endpoint, ReleaseChannel channel, ReleaseIndex index, string directory,
            string activeDirectory, string bundledDirectory, Action<bool> complete)
        {
            view.Show("正在比对已安装的游戏文件…", .12f);
            var plan = Task.Run(() => ReleaseFiles.PrepareIncremental(directory, index,
                activeDirectory, activeDirectory == bundledDirectory ? null : bundledDirectory));
            yield return new WaitUntil(() => plan.IsCompleted);
            if (plan.Status != TaskStatus.RanToCompletion)
            {
                Debug.LogWarning("XZJ_INCREMENTAL_UPDATE could not prepare files: " + plan.Exception);
                complete(false);
                yield break;
            }

            var objects = plan.Result.GroupBy(item => item.sha256, StringComparer.Ordinal).Select(group => group.ToArray()).ToArray();
            long totalBytes = objects.Sum(group => group[0].size);
            long fetched = 0;
            long finished = 0;
            int filesReused = index.files.Length - plan.Result.Length;
            Debug.Log($"XZJ_INCREMENTAL_PLAN version={index.version} reused={filesReused} missing={plan.Result.Length} objects={objects.Length} bytes={totalBytes}");
            foreach (var group in objects)
            {
                var entry = group[0];
                string download = Path.Combine(ReleaseFiles.IncrementalWork(directory), ".objects", entry.sha256 + ".download");
                string url = new Uri(endpoint, "objects/" + entry.sha256).AbsoluteUri;
                bool downloaded = false;
                yield return DownloadObject(url, channel, entry.size, download,
                    bytes => ShowDownload(index.version, finished + bytes, totalBytes),
                    bytes => fetched += bytes, () => downloaded = true);
                if (!downloaded)
                {
                    complete(false);
                    yield break;
                }

                var installed = Task.Run(() => ReleaseFiles.InstallObject(directory, download, group));
                yield return new WaitUntil(() => installed.IsCompleted);
                if (installed.Status != TaskStatus.RanToCompletion)
                {
                    Debug.LogWarning("XZJ_INCREMENTAL_UPDATE file verification failed: " + installed.Exception);
                    complete(false);
                    yield break;
                }
                finished += entry.size;
            }

            view.Show("正在安装游戏更新…", .82f);
            var committed = Task.Run(() => ReleaseFiles.CommitIncremental(directory, index));
            yield return new WaitUntil(() => committed.IsCompleted);
            if (committed.Status != TaskStatus.RanToCompletion)
            {
                Debug.LogWarning("XZJ_INCREMENTAL_UPDATE could not install: " + committed.Exception);
                complete(false);
                yield break;
            }
            Debug.Log($"XZJ_INCREMENTAL_INSTALLED version={index.version} reused={filesReused} files={plan.Result.Length} bytes={fetched}");
            complete(true);
        }

        private IEnumerator DownloadObject(string url, ReleaseChannel channel, long size, string download,
            Action<double> progress, Action<long> transferred, Action complete)
        {
            const long chunkSize = 8L << 20;
            string chunk = download + ".part";
            long offset;
            try
            {
                offset = File.Exists(download) ? new FileInfo(download).Length : 0;
                if (offset > size)
                {
                    File.Delete(download);
                    offset = 0;
                }
            }
            catch (Exception error) when (error is IOException || error is UnauthorizedAccessException)
            {
                Debug.LogWarning("XZJ_INCREMENTAL_UPDATE could not read download progress: " + error.GetType().Name);
                yield break;
            }

            while (offset < size)
            {
                progress(offset);
                long end = Math.Min(offset + chunkSize, size) - 1;
                long count = end - offset + 1;
                using (var request = Request(url, channel, new DownloadHandlerFile(chunk) { removeFileOnAbort = true }, 120))
                {
                    request.SetRequestHeader("Range", FormattableString.Invariant($"bytes={offset}-{end}"));
                    var transfer = request.SendWebRequest();
                    while (!transfer.isDone)
                    {
                        if (request.downloadedBytes > (ulong)count)
                        {
                            request.Abort();
                            break;
                        }
                        progress(offset + (double)request.downloadedBytes);
                        yield return null;
                    }
                    string range = FormattableString.Invariant($"bytes {offset}-{end}/{size}");
                    transferred((long)request.downloadedBytes);
                    Debug.Log($"XZJ_INCREMENTAL_REQUEST object={Path.GetFileName(url)} http={request.responseCode} offset={offset} bytes={request.downloadedBytes}");
                    if (request.result != UnityWebRequest.Result.Success || request.responseCode != 206 ||
                        request.downloadedBytes != (ulong)count || request.GetResponseHeader("Content-Range") != range)
                    {
                        Debug.LogWarning("XZJ_INCREMENTAL_UPDATE download interrupted.");
                        yield break;
                    }
                }

                long start = offset;
                Task appended = Task.Run(() => ReleaseFiles.AppendChunk(download, chunk, start, count));
                yield return new WaitUntil(() => appended.IsCompleted);
                if (appended.Status != TaskStatus.RanToCompletion)
                {
                    Debug.LogWarning("XZJ_INCREMENTAL_UPDATE could not save download progress: " + appended.Exception);
                    yield break;
                }
                offset += count;
            }
            complete();
        }

        private void ShowDownload(int version, double bytes, long total)
        {
            float progress = (float)(bytes / total);
            view.Show($"正在下载版本 {version} 的更新文件  {progress:P0}", Mathf.Lerp(.12f, .80f, progress));
        }

        private Assembly LoadCode(string directory)
        {
            try
            {
                Assembly mentor = null;
                foreach (string name in ReleaseIndex.CodeAssemblies)
                {
#if UNITY_EDITOR
                    // Editor scene MonoScripts already refer to its compiled assembly instances.
                    Assembly module = name == ReleaseIndex.MentorAssembly
                        ? Assembly.Load(File.ReadAllBytes(Path.Combine(directory, name + ".dll")))
                        : AppDomain.CurrentDomain.GetAssemblies().Single(loaded => loaded.GetName().Name == name);
#else
                    Assembly module = Assembly.Load(File.ReadAllBytes(Path.Combine(directory, name + ".dll")));
#endif
                    if (name != module.GetName().Name)
                        throw new InvalidDataException("发布程序集名称不匹配。");
                    if (name == ReleaseIndex.MentorAssembly)
                        mentor = module;
                }
                return mentor;
            }
            catch (Exception error)
            {
                Fail("客户端程序集加载失败，请重新打开游戏。", error);
                return null;
            }
        }

        private bool PrepareDirectory(string path)
        {
            try { Directory.CreateDirectory(path); return true; }
            catch (Exception error) when (error is IOException || error is UnauthorizedAccessException)
            {
                Fail("无法保存客户端版本，请检查设备空间。", error);
                return false;
            }
        }

#if !UNITY_EDITOR
        private bool LoadMetadata(string entry, byte[] bytes)
        {
            if (bytes != null)
            {
                var result = RuntimeApi.LoadMetadataForAOTAssembly(bytes, HomologousImageMode.SuperSet);
                if ((int)result == 0)
                    return true;
                Debug.LogError($"XZJ_FULL_RELEASE_METADATA {entry} {result}");
            }
            Fail("安装包的 AOT 元数据不可用。");
            return false;
        }
#endif

        private void Fail(string message, Exception error = null)
        {
            view.ShowFailure(message, codeLoaded ? null : (Action)Begin);
            Debug.LogError(error == null ? message : $"{message} {error.GetType().Name}: {error.Message}");
        }

        private static T ReadJson<T>(byte[] bytes) where T : class
        {
            if (bytes == null || bytes.Length == 0)
                return null;
            try { return JsonUtility.FromJson<T>(Encoding.UTF8.GetString(bytes)); }
            catch (ArgumentException) { return null; }
        }

        private static IEnumerator GetBytes(string url, ReleaseChannel channel, int limit, Action<byte[]> receive)
        {
            using var request = Request(url, channel, new DownloadHandlerBuffer(), channel == null ? 12 : 4);
            var operation = request.SendWebRequest();
            while (!operation.isDone)
            {
                if (request.downloadedBytes > (ulong)limit)
                {
                    request.Abort();
                    yield break;
                }
                yield return null;
            }
            if (channel != null)
                Debug.Log($"XZJ_FULL_RELEASE_REQUEST file={Path.GetFileName(new Uri(url).AbsolutePath)} http={request.responseCode} bytes={request.downloadedBytes}");
            if (request.result == UnityWebRequest.Result.Success && request.downloadedBytes <= (ulong)limit)
                receive(request.downloadHandler.data);
        }

        private static UnityWebRequest Request(string url, ReleaseChannel channel, DownloadHandler handler, int timeout)
        {
            if (Path.IsPathRooted(url))
                url = new Uri(url).AbsoluteUri;
            var request = new UnityWebRequest(url, UnityWebRequest.kHttpVerbGET, handler, null)
            {
                timeout = timeout,
                redirectLimit = 0
            };
            if (channel != null && !string.IsNullOrEmpty(channel.certificateSha256))
                request.certificateHandler = new PinnedLeaf(channel.certificateSha256);
            return request;
        }

        private sealed class PinnedLeaf : CertificateHandler
        {
            private readonly string fingerprint;

            internal PinnedLeaf(string fingerprint) => this.fingerprint = fingerprint;

            protected override bool ValidateCertificate(byte[] certificateData)
            {
                try
                {
                    using var leaf = new X509Certificate2(certificateData);
                    using var sha = SHA256.Create();
                    var actual = BitConverter.ToString(sha.ComputeHash(leaf.RawData)).Replace("-", "");
                    var now = DateTime.UtcNow;
                    return now >= leaf.NotBefore.ToUniversalTime() && now <= leaf.NotAfter.ToUniversalTime() &&
                        string.Equals(actual, fingerprint, StringComparison.OrdinalIgnoreCase);
                }
                catch (CryptographicException)
                {
                    return false;
                }
            }
        }
    }
}
