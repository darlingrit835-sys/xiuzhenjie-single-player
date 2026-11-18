using System;
using System.IO;
using System.Linq;
using UnityEditor;
using UnityEditor.Build;
using UnityEditor.Build.Reporting;
using UnityEngine;

internal static class RecoveryAndroidBuild
{
    private const string Package = "com.LMGame.XiuXianJie.recovery";

    public static void Run()
    {
        if (EditorUserBuildSettings.activeBuildTarget != BuildTarget.Android)
            throw new InvalidOperationException("Open the project with -buildTarget Android.");

        string output = Environment.GetEnvironmentVariable("XIU_RECOVERY_APK");
        if (string.IsNullOrEmpty(output) || !Path.IsPathRooted(output))
            throw new ArgumentException("XIU_RECOVERY_APK must contain the absolute output APK path.");

        PlayerSettings.SetApplicationIdentifier(NamedBuildTarget.Android, Package);
        PlayerSettings.SetScriptingBackend(NamedBuildTarget.Android, ScriptingImplementation.Mono2x);
        PlayerSettings.SetManagedStrippingLevel(NamedBuildTarget.Android, ManagedStrippingLevel.Disabled);
        PlayerSettings.Android.targetArchitectures = AndroidArchitecture.ARMv7;
        PlayerSettings.Android.targetSdkVersion = (AndroidSdkVersions)36;
        PlayerSettings.Android.bundleVersionCode = 13;
        PlayerSettings.Android.useCustomKeystore = false;
        PlayerSettings.bundleVersion = "1.3.5";
        PlayerSettings.Android.forceInternetPermission = true;
        EditorUserBuildSettings.buildAppBundle = false;
        EditorUserBuildSettings.exportAsGoogleAndroidProject = false;

        foreach (string library in new[] { "libxlua.so", "libsqlite3.so" })
        {
            string asset = "Assets/Plugins/Android/" + library;
            var importer = AssetImporter.GetAtPath(asset) as PluginImporter;
            if (importer == null)
                throw new FileNotFoundException("Missing Android plugin", asset);
            if (importer.GetCompatibleWithAnyPlatform() || importer.GetCompatibleWithEditor()
                || !importer.GetCompatibleWithPlatform(BuildTarget.Android) || importer.GetPlatformData(BuildTarget.Android, "CPU") != "ARMv7")
            {
                importer.SetCompatibleWithAnyPlatform(false);
                importer.SetCompatibleWithEditor(false);
                importer.SetCompatibleWithPlatform(BuildTarget.Android, true);
                importer.SetPlatformData(BuildTarget.Android, "CPU", "ARMv7");
                importer.SaveAndReimport();
            }
        }

        string[] scenes = EditorBuildSettings.scenes.Where(scene => scene.enabled).Select(scene => scene.path).ToArray();
        if (scenes.Length != 15 || scenes[0] != "Assets/Scenes/StartLoadScene.unity")
            throw new InvalidOperationException("Unexpected build scene list.");

        Directory.CreateDirectory(Path.GetDirectoryName(output));
        Debug.Log("RECOVERY_ANDROID_BUILD_START " + Package);
        BuildReport report = BuildPipeline.BuildPlayer(new BuildPlayerOptions
        {
            scenes = scenes,
            locationPathName = output,
            target = BuildTarget.Android,
            options = BuildOptions.None,
            extraScriptingDefines = new[] { "XIUZHENJIE_RECOVERY_BUILD" },
        });
        BuildSummary result = report.summary;
        Debug.Log($"RECOVERY_ANDROID_BUILD_RESULT result={result.result} errors={result.totalErrors} bytes={result.totalSize}");
        if (result.result != BuildResult.Succeeded || result.totalErrors != 0)
            throw new InvalidOperationException("Android test build failed: " + result.result);
        AssetDatabase.SaveAssets();
    }
}
