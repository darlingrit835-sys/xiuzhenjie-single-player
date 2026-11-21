using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEditor;
using UnityEngine;

internal static class ClientBuildPreflight
{
    private const string LegacyBundleDirectory = "StreamingAssets/Assetbundles";
    private const string LegacyBundleIndex = "assetbundleconfig";
    private const string HarmonyPluginDirectory = "Plugins/OpenHarmony";

    public static void Android()
    {
        var report = new Report("Android");
        RecordSceneCheck(report, RecoveryAndroidBuild.ExpectedSceneCount);
        BuildTarget target = BuildTarget.Android;
        bool targetAvailable = HasBuildSupport(target);
        report.Add("editor-target", targetAvailable ? "ok" : "blocked",
            targetAvailable ? "Android Build Support available." : "Install Android Build Support for this editor.");
        bool hasIl2Cpp = PlayerSettings.GetScriptingBackend(UnityEditor.Build.NamedBuildTarget.Android) == ScriptingImplementation.IL2CPP;
        report.Add("il2cpp", hasIl2Cpp ? "ok" : "blocked",
            hasIl2Cpp ? "Android uses the IL2CPP backend." : "Switch Android to IL2CPP before generating the HybridCLR baseline.");
        var seed = GetLegacySeedInventory();
        bool hasSeed = seed.HasIndex && seed.FileCount > 1;
        report.Add("platform-content", hasSeed ? "ok" : "blocked",
            hasSeed ? $"Assets/{LegacyBundleDirectory} contains the {LegacyBundleIndex} index and {seed.FileCount - 1} other nonempty payload files."
                : $"Assets/{LegacyBundleDirectory} requires a nonempty {LegacyBundleIndex} and at least one other payload file.");
        foreach (string pluginPath in RecoveryAndroidBuild.NativePluginPaths())
        {
            var importer = AssetImporter.GetAtPath(pluginPath) as PluginImporter;
            bool configured = importer != null && RecoveryAndroidBuild.HasExpectedPluginSettings(importer);
            string cpu = RecoveryAndroidBuild.NativePluginCpu(pluginPath);
            report.Add(Path.GetFileNameWithoutExtension(pluginPath) + "-" + cpu, configured ? "ok" : "blocked",
                configured ? $"{pluginPath} imported for Android {cpu}."
                    : $"{pluginPath} must be imported for Android {cpu} and excluded from Editor and Any Platform.");
        }
        report.Write();
        if (report.HasBlockers)
            throw new InvalidOperationException("Android preflight blocked. See CLIENT_BUILD_PREFLIGHT in the editor log.");
    }

    public static void OpenHarmony()
    {
        var report = new Report("OpenHarmony");
        RecordSceneCheck(report);
        // The OpenHarmony enum value is available in Tuanjie but absent from stock Unity 2022 LTS.
        bool targetAvailable = Enum.TryParse("OpenHarmony", out BuildTarget target) && HasBuildSupport(target);
        report.Add("editor-target", targetAvailable ? "ok" : "blocked",
            targetAvailable ? "OpenHarmony Build Support available."
                : "Open this project in Tuanjie Editor with OpenHarmony Build Support.");

        var legacySeed = GetLegacySeedInventory();
        report.Add("platform-content", "blocked",
            $"Assets/{LegacyBundleDirectory} has {legacySeed.FileCount} nonempty files in the legacy Android seed location. " +
            "Prepare an OpenHarmony-built seed and resource index and verify startup loading on that target.");

        RecordLegacyNativeDependency(report, "libsqlite3.so", target, targetAvailable,
            "Confirm whether the legacy DbAccess path is exercised. If required, supply and verify a target SQLite implementation.");
        report.Add("runtime-compatibility", "review",
            "Verify the IL2CPP boot path, generated bindings and reflection, and isolate Android Java integrations before enabling platform services.");
        report.Write();
    }

    private static (int FileCount, bool HasIndex) GetLegacySeedInventory()
    {
        var directory = new DirectoryInfo(Path.Combine(Application.dataPath, LegacyBundleDirectory));
        if (!directory.Exists)
            return (0, false);

        FileInfo[] files = directory.EnumerateFiles().Where(file => file.Length > 0
            && !file.Name.StartsWith(".", StringComparison.Ordinal)
            && !file.Extension.Equals(".meta", StringComparison.OrdinalIgnoreCase)).ToArray();
        return (files.Length, files.Any(file => file.Name == LegacyBundleIndex));
    }

    private static void RecordLegacyNativeDependency(Report report, string library, BuildTarget target, bool targetAvailable, string action)
    {
        string root = Path.Combine(Application.dataPath, HarmonyPluginDirectory);
        string[] candidates = Directory.Exists(root) ? Directory.GetFiles(root, library, SearchOption.AllDirectories) : Array.Empty<string>();
        bool imported = targetAvailable && candidates.Any(file =>
        {
            string assetPath = "Assets" + file.Substring(Application.dataPath.Length).Replace('\\', '/');
            return AssetImporter.GetAtPath(assetPath) is PluginImporter importer
                && !importer.GetCompatibleWithAnyPlatform() && importer.GetCompatibleWithPlatform(target);
        });
        string result = imported ? "A target importer is configured. Confirm its ABI and native linkage. "
            : $"No target importer verified under Assets/{HarmonyPluginDirectory}. The recovery library under Assets/Plugins/Android is for Android. ";
        report.Add(Path.GetFileNameWithoutExtension(library), "review", result + action);
    }

    private static void RecordSceneCheck(Report report, int? expectedSceneCount = null)
    {
        try
        {
            string[] scenes = ClientBuild.GetEnabledScenes(expectedSceneCount);
            report.Add("startup-scenes", "ok", $"{scenes.Length} scenes, bootstrap {scenes[0]}.");
        }
        catch (InvalidOperationException failure)
        {
            report.Add("startup-scenes", "blocked", failure.Message);
        }
    }

    private static bool HasBuildSupport(BuildTarget target)
    {
        BuildTargetGroup group = BuildPipeline.GetBuildTargetGroup(target);
        return group != BuildTargetGroup.Unknown && BuildPipeline.IsBuildTargetSupported(group, target);
    }

    [Serializable]
    private sealed class Report
    {
        public string target;
        public string editor = Application.unityVersion;
        public string activeTarget = EditorUserBuildSettings.activeBuildTarget.ToString();
        public List<Check> checks = new List<Check>();

        public bool HasBlockers => checks.Any(check => check.status == "blocked");

        public Report(string target)
        {
            this.target = target;
        }

        public void Add(string id, string status, string detail)
        {
            checks.Add(new Check { id = id, status = status, detail = detail });
        }

        public void Write()
        {
            Debug.Log("CLIENT_BUILD_PREFLIGHT " + JsonUtility.ToJson(this));
        }
    }

    [Serializable]
    private sealed class Check
    {
        public string id;
        public string status;
        public string detail;
    }
}
