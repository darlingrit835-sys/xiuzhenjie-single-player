using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Reflection;
using System.Text;
using HybridCLR;
using HybridCLR.Commands;
using HybridCLR.Settings;
using UnityEditor;
using UnityEditor.Android;
using UnityEditor.Build;
using UnityEngine;
using XiuZhenJie.Client.Release;
using Debug = UnityEngine.Debug;

internal static class ClientReleaseBuild
{
    private const string Bootstrap = "Assets/Scenes/ClientBootstrap.unity";
    private static string StreamRoot => Path.Combine(Application.dataPath, "StreamingAssets", "FullUpdate");

    [MenuItem("修真界/全量热更新/生成启动基线")]
    public static void PrepareBase()
    {
        BuildTarget target = EditorUserBuildSettings.activeBuildTarget;
        RequireIl2Cpp(target);
        WithFullProfile(() => GenerateBase(target));
    }

    public static void BuildAndroid()
    {
        BuildTarget target = EditorUserBuildSettings.activeBuildTarget;
        if (target != BuildTarget.Android)
            throw new BuildFailedException("全量 Android 打包要求 -buildTarget Android。");
        string output = ClientBuild.GetArtifactPath("XIU_RECOVERY_APK");
        string channelPath = ClientBuild.GetArtifactPath("XIU_FULL_UPDATE_CONFIG");
        string release = ClientBuild.GetArtifactPath("XIU_HOTUPDATE_OUTPUT");
        string key = ClientBuild.GetArtifactPath("XIU_FULL_UPDATE_SIGNING_KEY");
        string config = File.ReadAllText(channelPath);
        var channel = JsonUtility.FromJson<ReleaseChannel>(config);
        if (channel?.TryEndpoint(out _) != true)
            throw new BuildFailedException("全量更新频道必须包含有效的 HTTPS 地址和 RSA 公钥。");
        int version = ReleaseVersion();
        string[] gameScenes = ClientBuild.GetEnabledScenes();

        string clientVersion = PlayerSettings.bundleVersion;
        RecoveryAndroidBuild.ConfigurePlayer(arm64: true);
        PlayerSettings.bundleVersion = clientVersion;
        const string package = "com.lmgame.xiuxianjie.emailtrial";
        PlayerSettings.SetApplicationIdentifier(NamedBuildTarget.Android, package);
        if (!int.TryParse(Environment.GetEnvironmentVariable("XIU_ANDROID_VERSION_CODE"), NumberStyles.None,
                CultureInfo.InvariantCulture, out int buildNumber) || buildNumber < 29)
            throw new BuildFailedException("XIU_ANDROID_VERSION_CODE 必须大于已发布的 28。");
        PlayerSettings.Android.bundleVersionCode = buildNumber;
        ConfigureLauncherIcon();
        WithFullProfile(() => GenerateBase(target));

        var platform = ReadBase(Path.Combine(StreamRoot, "base.json"), target);
        WriteRelease(target, platform, version, release, gameScenes);
        SignRelease(release, key);
        byte[] manifest = File.ReadAllBytes(Path.Combine(release, "manifest.json"));
        byte[] signature = File.ReadAllBytes(Path.Combine(release, "manifest.sig"));
        if (!channel.Verify(manifest, signature))
            throw new BuildFailedException("签名私钥与更新频道的内置公钥不匹配。");
        File.Copy(Path.Combine(StreamRoot, "base.json"), Path.Combine(release, "base.json"));

        WithFullProfile(() =>
        {
            File.WriteAllText(Path.Combine(StreamRoot, "channel.json"), config, new UTF8Encoding(false));
            AssetDatabase.Refresh(ImportAssetOptions.ForceSynchronousImport);
            WithBundledRelease(release, () => WithoutLegacyStreamingAssets(() => ClientBuild.BuildPlayer(new BuildPlayerOptions
            {
                scenes = new[] { Bootstrap },
                target = target,
                locationPathName = output,
                options = BuildOptions.None
            }, "XZJ_FULL_ANDROID_BUILD", package)));
        });
    }

    private static void SignRelease(string directory, string key)
    {
        var start = new ProcessStartInfo
        {
            FileName = "python3",
            WorkingDirectory = Directory.GetParent(Application.dataPath).FullName,
            UseShellExecute = false,
        };
        foreach (string argument in new[] { "Tools/hot_update.py", "sign-full", "--patch-dir", directory, "--private-key", key })
            start.ArgumentList.Add(argument);
        using var signer = Process.Start(start);
        signer.WaitForExit();
        if (signer.ExitCode != 0)
            throw new BuildFailedException("无法签名安装包内的完整客户端。");
    }

    private static void WithBundledRelease(string release, Action build)
    {
        string seed = Path.Combine(Application.streamingAssetsPath, ReleaseIndex.SeedDirectory);
        string stash = Path.Combine(SettingsUtil.HybridCLRDataDir, "XZJSeed-" + Guid.NewGuid().ToString("N"));
        if (Directory.Exists(seed))
            Directory.Move(seed, stash);
        try
        {
            Directory.CreateDirectory(seed);
            foreach (string name in new[] { "manifest.json", "manifest.sig", ReleaseIndex.ArchiveName })
                File.Copy(Path.Combine(release, name), Path.Combine(seed, name));
            AssetDatabase.Refresh(ImportAssetOptions.ForceSynchronousImport);
            build();
        }
        finally
        {
            if (Directory.Exists(seed))
                Directory.Delete(seed, true);
            if (Directory.Exists(stash))
                Directory.Move(stash, seed);
            AssetDatabase.Refresh(ImportAssetOptions.ForceSynchronousImport);
        }
    }

    private static void WithoutLegacyStreamingAssets(Action build)
    {
        string streaming = Application.streamingAssetsPath;
        string stash = Path.Combine(SettingsUtil.HybridCLRDataDir, "XZJStreaming-" + Guid.NewGuid().ToString("N"));
        var names = new[] { ReleaseIndex.LegacyDirectory, "HotUpdate" };
        Directory.CreateDirectory(stash);
        try
        {
            foreach (string name in names)
            {
                string path = Path.Combine(streaming, name);
                if (Directory.Exists(path))
                    Directory.Move(path, Path.Combine(stash, name));
                if (File.Exists(path + ".meta"))
                    File.Move(path + ".meta", Path.Combine(stash, name + ".meta"));
            }
            AssetDatabase.Refresh(ImportAssetOptions.ForceSynchronousImport);
            build();
        }
        finally
        {
            foreach (string name in names)
            {
                string saved = Path.Combine(stash, name);
                string path = Path.Combine(streaming, name);
                if (Directory.Exists(saved))
                    Directory.Move(saved, path);
                if (File.Exists(saved + ".meta"))
                    File.Move(saved + ".meta", path + ".meta");
            }
            Directory.Delete(stash);
            AssetDatabase.Refresh(ImportAssetOptions.ForceSynchronousImport);
        }
    }

    private static void ConfigureLauncherIcon()
    {
        var texture = AssetDatabase.LoadAssetAtPath<Texture2D>("Assets/Brand/LauncherIcon.png");
        if (texture == null)
            throw new BuildFailedException("缺少 Android 启动图标。");
        foreach (var kind in new[] { AndroidPlatformIconKind.Adaptive, AndroidPlatformIconKind.Round, AndroidPlatformIconKind.Legacy })
        {
            var icons = PlayerSettings.GetPlatformIcons(NamedBuildTarget.Android, kind);
            foreach (var icon in icons)
                icon.SetTextures(kind == AndroidPlatformIconKind.Adaptive ? new[] { texture, texture } : new[] { texture });
            PlayerSettings.SetPlatformIcons(NamedBuildTarget.Android, kind, icons);
        }
    }

    public static void ExportRelease()
    {
        BuildTarget target = EditorUserBuildSettings.activeBuildTarget;
        RequireIl2Cpp(target);
        var platform = ReadBase(ClientBuild.GetArtifactPath("XIU_FULL_UPDATE_BASELINE"), target);
        string output = ClientBuild.GetArtifactPath("XIU_HOTUPDATE_OUTPUT");
        WriteRelease(target, platform, ReleaseVersion(), output, ClientBuild.GetEnabledScenes());
    }

    private static ReleaseBase ReadBase(string path, BuildTarget target)
    {
        var platform = JsonUtility.FromJson<ReleaseBase>(File.ReadAllText(path));
        if (platform == null || platform.target != target.ToString() || !ReleaseIndex.Digest(platform.compatibility))
            throw new BuildFailedException("全量基线与当前目标平台不匹配。");
        return platform;
    }

    private static int ReleaseVersion()
    {
        if (!int.TryParse(Environment.GetEnvironmentVariable("XIU_HOTUPDATE_VERSION"), NumberStyles.None,
                CultureInfo.InvariantCulture, out int version) || version < 1)
            throw new BuildFailedException("XIU_HOTUPDATE_VERSION 必须是正整数。");
        return version;
    }

    private static void WriteRelease(BuildTarget target, ReleaseBase platform, int version, string directory, string[] gameScenes)
    {
        if (Directory.Exists(directory) && Directory.EnumerateFileSystemEntries(directory).Any())
            throw new BuildFailedException("选择空目录输出完整客户端版本。");

        string scratch = Path.Combine(SettingsUtil.HybridCLRDataDir, "XZJFull-" + Guid.NewGuid().ToString("N"));
        string publish = directory + "." + Guid.NewGuid().ToString("N") + ".tmp";
        try
        {
            string payload = Path.Combine(scratch, "payload");
            string compiled = Path.Combine(scratch, "managed");
            Directory.CreateDirectory(payload);
            Directory.CreateDirectory(publish);
            CompileDllCommand.CompileDll(compiled, target, false);
            foreach (string name in ReleaseIndex.CodeAssemblies)
            {
                string source = Path.Combine(compiled, name + ".dll");
                if (!File.Exists(source) || AssemblyName.GetAssemblyName(source).Name != name)
                    throw new BuildFailedException("缺少热更新程序集 " + name);
                File.Copy(source, Path.Combine(payload, name + ".dll"));
            }
            BuildResources(target, gameScenes, scratch, payload);
            CopyRecoveredBundles(payload);

            string archive = Path.Combine(publish, ReleaseIndex.ArchiveName);
            ZipFile.CreateFromDirectory(payload, archive, System.IO.Compression.CompressionLevel.NoCompression, false);
            var index = new ReleaseIndex
            {
                schema = 2,
                target = platform.target,
                compatibility = platform.compatibility,
                version = version,
                archiveSize = new FileInfo(archive).Length,
                archiveSha256 = ReleaseIndex.HashFile(archive),
                files = Directory.GetFiles(payload, "*", SearchOption.AllDirectories).OrderBy(path => path, StringComparer.Ordinal)
                    .Select(path => new ReleaseFile
                    {
                        name = path.Substring(payload.Length + 1).Replace('\\', '/'),
                        size = new FileInfo(path).Length,
                        sha256 = ReleaseIndex.HashFile(path)
                    }).ToArray()
            };
            if (!ReleaseIndex.Valid(index, platform))
                throw new BuildFailedException("发布包超出约定范围或缺少必需的文件。");
            WriteJson(Path.Combine(publish, "manifest.json"), index);
            if (Directory.Exists(directory))
                Directory.Delete(directory);
            Directory.Move(publish, directory);
            Debug.Log($"XZJ_FULL_RELEASE_EXPORT_OK target={target} version={version} scenes={gameScenes.Length} files={index.files.Length} bytes={index.archiveSize}");
        }
        finally
        {
            if (Directory.Exists(scratch))
                Directory.Delete(scratch, true);
            if (Directory.Exists(publish))
                Directory.Delete(publish, true);
        }
    }

    private static void GenerateBase(BuildTarget target)
    {
        var installer = new HybridCLR.Installer.InstallerController();
        if (!installer.HasInstalledHybridCLR() || installer.InstalledLibil2cppVersion != installer.PackageVersion)
            throw new BuildFailedException("先执行项目内 HybridCLR 安装。");
        PrebuildCommand.GenerateAll();
        string stripped = SettingsUtil.GetAssembliesPostIl2CppStripDir(target);
        string[] assemblies = Directory.GetFiles(stripped, "*.dll").OrderBy(Path.GetFileName, StringComparer.Ordinal).ToArray();
        if (assemblies.Any(path => ReleaseIndex.CodeAssemblies.Contains(Path.GetFileNameWithoutExtension(path))))
            throw new BuildFailedException("玩法程序集仍出现在 AOT 产物，无法安全打包。");

        string[] metadata = assemblies.Select(Path.GetFileName).Where(name => name != "XiuZhenJie.Client.Release.dll").ToArray();
        if (!metadata.Contains("mscorlib.dll"))
            throw new BuildFailedException("缺少 AOT 基础元数据。");
        var stamp = new StringBuilder($"{Application.unityVersion}\n{target}\nfull-release-v2\n");
        foreach (string path in assemblies.Concat(Directory.GetFiles(SettingsUtil.GeneratedCppDir, "*.cpp").OrderBy(Path.GetFileName, StringComparer.Ordinal)))
            stamp.Append(Path.GetFileName(path)).Append(' ').Append(ReleaseIndex.HashFile(path)).Append('\n');
        using var sha = System.Security.Cryptography.SHA256.Create();
        string id = BitConverter.ToString(sha.ComputeHash(Encoding.UTF8.GetBytes(stamp.ToString()))).Replace("-", "").ToLowerInvariant();

        string dir = Path.Combine(StreamRoot, "AOT");
        Directory.CreateDirectory(dir);
        foreach (string name in metadata)
            File.Copy(Path.Combine(stripped, name), Path.Combine(dir, name + ".bytes"), true);
        WriteJson(Path.Combine(StreamRoot, "base.json"), new ReleaseBase { target = target.ToString(), compatibility = id, metadata = metadata });
        AssetDatabase.Refresh(ImportAssetOptions.ForceSynchronousImport);
        Debug.Log($"XZJ_FULL_AOT_BASE_OK target={target} compatibility={id} metadata={metadata.Length}");
    }

    private static void WithFullProfile(Action action)
    {
        var settings = HybridCLRSettings.instance;
        string[] original = settings.hotUpdateAssemblies;
        EditorBuildSettingsScene[] scenes = EditorBuildSettings.scenes;
        bool dev = EditorUserBuildSettings.development;
        try
        {
            settings.hotUpdateAssemblies = ReleaseIndex.CodeAssemblies;
            EditorBuildSettings.scenes = new[] { new EditorBuildSettingsScene(Bootstrap, true) };
            EditorUserBuildSettings.development = false;
            action();
        }
        finally
        {
            settings.hotUpdateAssemblies = original;
            EditorBuildSettings.scenes = scenes;
            EditorUserBuildSettings.development = dev;
        }
    }

    private static void BuildResources(BuildTarget target, string[] scenes, string scratch, string payload)
    {
        var sources = AssetDatabase.GetAllAssetPaths().Where(path =>
                (path.StartsWith("Assets/Resources/", StringComparison.OrdinalIgnoreCase) ||
                 path.StartsWith("Assets/GameData/", StringComparison.OrdinalIgnoreCase)) &&
                !AssetDatabase.IsValidFolder(path) && IsRuntimeAsset(path))
            .OrderBy(path => path, StringComparer.Ordinal).ToArray();
        string[] addresses = sources.Select(ContentAddress).ToArray();
        if (addresses.Distinct(StringComparer.Ordinal).Count() != addresses.Length)
            throw new BuildFailedException("全量资源包含重复的运行时地址。");

        string directory = Path.Combine(scratch, "bundles");
        Directory.CreateDirectory(directory);
        var packs = new[]
        {
            new AssetBundleBuild { assetBundleName = ReleaseIndex.AssetBundle, assetNames = sources, addressableNames = addresses },
            new AssetBundleBuild { assetBundleName = ReleaseIndex.SceneBundle, assetNames = scenes }
        };
        if (BuildPipeline.BuildAssetBundles(directory, packs,
            BuildAssetBundleOptions.ChunkBasedCompression | BuildAssetBundleOptions.StrictMode, target) == null)
            throw new BuildFailedException("场景或资源打包失败。");
        foreach (var pack in packs)
            File.Copy(Path.Combine(directory, pack.assetBundleName), Path.Combine(payload, pack.assetBundleName));
        Debug.Log($"XZJ_FULL_CONTENT_OK assets={sources.Length} scenes={scenes.Length}");
    }

    private static bool IsRuntimeAsset(string path)
    {
        Type type = AssetDatabase.GetMainAssetTypeAtPath(path);
        return type != null && type != typeof(DefaultAsset) && type != typeof(MonoScript) &&
            !path.EndsWith(".dll", StringComparison.OrdinalIgnoreCase) &&
            !path.EndsWith(".unity", StringComparison.OrdinalIgnoreCase);
    }

    private static string ContentAddress(string path)
    {
        const string resources = "Assets/Resources/";
        return (path.StartsWith(resources, StringComparison.OrdinalIgnoreCase)
            ? "resources/" + Path.ChangeExtension(path.Substring(resources.Length), null)
            : path).ToLowerInvariant();
    }

    private static void CopyRecoveredBundles(string payload)
    {
        string legacy = Path.Combine(payload, ReleaseIndex.LegacyDirectory);
        Directory.CreateDirectory(legacy);
        foreach (string file in Directory.GetFiles(Path.Combine(Application.streamingAssetsPath, ReleaseIndex.LegacyDirectory)))
        {
            string name = Path.GetFileName(file);
            if (ReleaseIndex.SimpleName(name) && !name.EndsWith(".meta", StringComparison.Ordinal))
                File.Copy(file, Path.Combine(legacy, name));
        }
    }

    private static void RequireIl2Cpp(BuildTarget target)
    {
        var named = NamedBuildTarget.FromBuildTargetGroup(BuildPipeline.GetBuildTargetGroup(target));
        if (PlayerSettings.GetScriptingBackend(named) != ScriptingImplementation.IL2CPP)
            throw new BuildFailedException("全量热更新需要 IL2CPP。");
    }

    private static void WriteJson(string path, object value) =>
        File.WriteAllText(path, JsonUtility.ToJson(value, true) + "\n", new UTF8Encoding(false));
}
