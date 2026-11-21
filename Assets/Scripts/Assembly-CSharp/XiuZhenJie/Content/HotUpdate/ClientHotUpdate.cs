using System;
using System.Collections;
using System.IO;
using System.Reflection;
using UnityEngine;
using UnityEngine.Networking;
using XiuZhenJie.Client.Release;

namespace XiuZhenJie
{
    internal static class ClientHotUpdate
    {
        private const string ModuleName = "XZJ.HotUpdate";
        private const string MentorEntry = "XiuZhenJie.HotUpdate.MentorText";
        private const string Folder = "HotUpdate";
        private static Func<string, string> mentorDialogue;
        private static Func<string, string> mentorCaption;
        private static bool started;

        internal static bool MeridianEntryEnabled { get; private set; } = true;

        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.SubsystemRegistration)]
        private static void Reset()
        {
            started = false;
            MeridianEntryEnabled = true;
            mentorDialogue = null;
            mentorCaption = null;
        }

        internal static string MentorDialogue(ShiTuToZiList action) => InvokeMentor(mentorDialogue, action);

        internal static string MentorCaption(ShiTuToZiList action) => InvokeMentor(mentorCaption, action);

        private static string InvokeMentor(Func<string, string> method, ShiTuToZiList action)
        {
            try
            {
                return method?.Invoke(action.ToString());
            }
            catch (Exception error)
            {
                Debug.LogWarning($"热更新对话不可用，使用主包文案（{error.GetType().Name}）。");
                return null;
            }
        }

        internal static IEnumerator Initialize(MonoBehaviour host)
        {
            if (started)
                yield break;
            started = true;

            // Select local code during loading. Stage any future code update in the background.
            if (ReleaseRuntime.IsActive)
            {
                Bind(ReleaseRuntime.ContentModule);
                yield break;
            }
            string target = BuildTargetName();
            string packaged = Application.streamingAssetsPath.TrimEnd('/') + "/" + Folder + "/";
            byte[] index = null;
            yield return ReadBytes(packaged + ClientHotUpdateManifest.ManifestFile, ClientHotUpdateManifest.MaxManifestBytes, 3, data => index = data);
            ClientHotUpdateManifest baseline = ClientHotUpdateManifest.Read(index, target);
            if (baseline == null)
                yield break;

            byte[] settings = null;
            yield return ReadBytes(packaged + "config.json", ClientHotUpdateManifest.MaxManifestBytes, 3, data => settings = data);
            ClientHotUpdateConfig config = ClientHotUpdateConfig.Read(settings, out Uri endpoint);
            string cachePath = Path.Combine(Application.persistentDataPath, Folder, target + ".patch");
            ClientHotUpdatePatch cached = config == null ? null : ClientHotUpdatePatch.Read(cachePath);
            ClientHotUpdateManifest selected = cached?.Verify(config, baseline, baseline.version);

#if !UNITY_EDITOR
            foreach (string name in baseline.metadata)
            {
                byte[] image = null;
                yield return ReadBytes(packaged + "AOT/" + name + ".bytes", ClientHotUpdateManifest.MaxMetadataBytes, 4, data => image = data);
                if (image == null || !LoadMetadata(name, image))
                    yield break;
            }
#endif

            byte[] payload = null;
            if (selected != null)
            {
                payload = cached.Dll;
            }
            else
            {
                yield return ReadBytes(packaged + ClientHotUpdateManifest.DllFile, ClientHotUpdateManifest.MaxDllBytes, 4, data => payload = data);
                if (!baseline.MatchesDll(payload))
                {
                    Debug.LogWarning("随包热更新模块校验失败，使用主包功能。");
                    yield break;
                }
                selected = baseline;
            }

            if (!Bind(payload))
            {
                if (selected != baseline)
                    DeleteCache(cachePath);
                yield break;
            }
            Debug.Log($"XZJ_HOTUPDATE_READY target={target} version={selected.version} source={(selected == baseline ? "bundled" : "cache")}");

            if (config != null)
                host.StartCoroutine(FetchNextVersion(config, endpoint, baseline, selected.version, cachePath));
        }

#if !UNITY_EDITOR
        private static bool LoadMetadata(string name, byte[] bytes)
        {
            try
            {
                var result = HybridCLR.RuntimeApi.LoadMetadataForAOTAssembly(bytes, HybridCLR.HomologousImageMode.SuperSet);
                if ((int)result == 0)
                    return true;
                Debug.LogWarning($"热更新 AOT 元数据加载失败 {name}（{result}）。");
            }
            catch (Exception error)
            {
                Debug.LogWarning($"热更新 AOT 元数据不可用 {name}（{error.GetType().Name}）。");
            }
            return false;
        }
#endif

        private static bool Bind(byte[] bytes)
        {
            try { return Bind(Assembly.Load(bytes)); }
            catch (Exception error)
            {
                Debug.LogWarning($"热更新 DLL 无法加载（{error.GetType().Name}）。");
                return false;
            }
        }

        private static bool Bind(Assembly module)
        {
            try
            {
                if (module.GetName().Name != ModuleName)
                    return false;
                Type entry = module.GetType(MentorEntry, throwOnError: true);
                var dialogue = BindMentorMethod(entry, "Dialogue");
                var caption = BindMentorMethod(entry, "Caption");
                string probe = nameof(ShiTuToZiList.ChuangGong);
                if (string.IsNullOrEmpty(dialogue(probe)) || string.IsNullOrEmpty(caption(probe)))
                    throw new InvalidDataException("Missing mentor text in hot-update entry.");
                // Publish both callbacks only after both ABI checks succeed.
                mentorDialogue = dialogue;
                mentorCaption = caption;
                MeridianEntryEnabled = ReadMeridianAvailability(module);
                return true;
            }
            catch (Exception error)
            {
                Debug.LogWarning($"热更新模块启动失败，使用主包功能（{error.GetType().Name}）。");
                return false;
            }
        }

        private static bool ReadMeridianAvailability(Assembly module)
        {
            try
            {
                MethodInfo method = module.GetType("XiuZhenJie.HotUpdate.ClientFeatures")?.GetMethod(
                    "MeridianAtlasEnabled", BindingFlags.Public | BindingFlags.Static, null, Type.EmptyTypes, null);
                // A cached module predating the rebuilt atlas controls only the legacy entry.
                return method == null || method.ReturnType != typeof(bool) ||
                    ((Func<bool>)Delegate.CreateDelegate(typeof(Func<bool>), method))();
            }
            catch (Exception error)
            {
                Debug.LogWarning($"经脉功能配置不可用，使用随包经脉图（{error.GetType().Name}）。");
                return true;
            }
        }

        private static Func<string, string> BindMentorMethod(Type entry, string name)
        {
            MethodInfo method = entry.GetMethod(name, BindingFlags.Public | BindingFlags.Static,
                null, new[] { typeof(string) }, null);
            if (method == null || method.ReturnType != typeof(string))
                throw new MissingMethodException(entry.FullName, name);
            return (Func<string, string>)Delegate.CreateDelegate(typeof(Func<string, string>), method);
        }

        private static IEnumerator FetchNextVersion(ClientHotUpdateConfig config, Uri endpoint,
            ClientHotUpdateManifest baseline, int currentVersion, string cachePath)
        {
            byte[] index = null;
            yield return ReadBytes(endpoint.AbsoluteUri, ClientHotUpdateManifest.MaxManifestBytes, 4, data => index = data, remote: true);
            ClientHotUpdateManifest next = ClientHotUpdateManifest.Read(index, baseline.target);
            if (next == null || !next.IsNewerThan(baseline, currentVersion))
                yield break;

            byte[] signature = null;
            yield return ReadBytes(new Uri(endpoint, ClientHotUpdateManifest.SignatureFile).AbsoluteUri,
                ClientHotUpdateManifest.MaxSignatureBytes, 4, data => signature = data, remote: true);
            if (!config.Verifies(index, signature))
            {
                Debug.LogWarning("热更新清单签名校验失败。");
                yield break;
            }

            byte[] dll = null;
            yield return ReadBytes(new Uri(endpoint, ClientHotUpdateManifest.DllFile).AbsoluteUri,
                ClientHotUpdateManifest.MaxDllBytes, 12, data => dll = data, remote: true);
            if (!next.MatchesDll(dll))
            {
                Debug.LogWarning("热更新 DLL 校验失败。");
                yield break;
            }

            try
            {
                new ClientHotUpdatePatch(index, signature, dll).Save(cachePath);
                Debug.Log($"XZJ_HOTUPDATE_STAGED target={baseline.target} version={next.version}");
            }
            catch (Exception error) when (error is IOException || error is UnauthorizedAccessException || error is NotSupportedException)
            {
                Debug.LogWarning($"无法保存热更新文件（{error.GetType().Name}）。");
            }
        }

        private static void DeleteCache(string path)
        {
            try
            {
                File.Delete(path);
            }
            catch (Exception error) when (error is IOException || error is UnauthorizedAccessException)
            {
                Debug.LogWarning("无法清理失效的热更新缓存。");
            }
        }

        private static IEnumerator ReadBytes(string location, int limit, int timeout, Action<byte[]> complete, bool remote = false)
        {
            if (Path.IsPathRooted(location))
                location = new Uri(location).AbsoluteUri;
            using var bytes = new MemoryStream();
            using var request = new UnityWebRequest(location, UnityWebRequest.kHttpVerbGET,
                new BoundedDownload(bytes, limit), null);
            request.redirectLimit = 0;
            request.timeout = timeout;
            if (remote)
                DirectCloudTrial.ConfigureHotUpdateRequest(request);
            yield return request.SendWebRequest();
            if (request.result == UnityWebRequest.Result.Success)
                complete(bytes.ToArray());
        }

        private static string BuildTargetName()
        {
#if UNITY_EDITOR
            return UnityEditor.EditorUserBuildSettings.activeBuildTarget.ToString();
#elif UNITY_OPENHARMONY
            return "OpenHarmony";
#else
            return Application.platform switch
            {
                RuntimePlatform.OSXPlayer => "StandaloneOSX",
                RuntimePlatform.WindowsPlayer => "StandaloneWindows64",
                RuntimePlatform.LinuxPlayer => "StandaloneLinux64",
                _ => Application.platform.ToString()
            };
#endif
        }

        private sealed class BoundedDownload : DownloadHandlerScript
        {
            private readonly MemoryStream data;
            private readonly int limit;

            internal BoundedDownload(MemoryStream data, int limit) : base(new byte[16 * 1024])
            {
                this.data = data;
                this.limit = limit;
            }

            protected override bool ReceiveData(byte[] chunk, int length)
            {
                if (chunk == null || length <= 0)
                    return true;
                if (data.Length + length > limit)
                    return false;
                data.Write(chunk, 0, length);
                return true;
            }
        }
    }
}
