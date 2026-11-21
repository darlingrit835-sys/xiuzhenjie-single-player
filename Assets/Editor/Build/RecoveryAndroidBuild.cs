using System;
using System.IO;
using UnityEditor;
using UnityEditor.Build;
using UnityEngine;
using UnityEngine.Rendering;

internal static class RecoveryAndroidBuild
{
    private const string Package = "com.LMGame.XiuXianJie.recovery";
    private const string NativeSqliteArm = "Assets/Plugins/Android/libsqlite3.so";
    private const string NativeSqliteArm64 = "Assets/Plugins/Android/arm64-v8a/libsqlite3.so";
    internal const int ExpectedSceneCount = 15;

    public static void Run()
    {
        if (EditorUserBuildSettings.activeBuildTarget != BuildTarget.Android)
            throw new InvalidOperationException("Open the project with -buildTarget Android.");

        string output = ClientBuild.GetArtifactPath("XIU_RECOVERY_APK");
        string[] scenes = ClientBuild.GetEnabledScenes(ExpectedSceneCount);

        ConfigurePlayer();
        ClientHotUpdateBuild.PreparePlayerBaseline(BuildTarget.Android, false);
        ClientBuild.BuildPlayer(new BuildPlayerOptions
        {
            scenes = scenes,
            locationPathName = output,
            target = BuildTarget.Android,
            options = BuildOptions.None,
        }, "RECOVERY_ANDROID_BUILD", Package);
    }

    public static void RunFull() => ClientReleaseBuild.BuildAndroid();

    internal static void ConfigurePlayer(bool arm64 = false)
    {
        PlayerSettings.SetApplicationIdentifier(NamedBuildTarget.Android, Package);
        PlayerSettings.SetScriptingBackend(NamedBuildTarget.Android, ScriptingImplementation.IL2CPP);
        PlayerSettings.SetManagedStrippingLevel(NamedBuildTarget.Android, ManagedStrippingLevel.Minimal);
        // The recovered Android bundles contain OpenGL ES shader variants.
        PlayerSettings.SetUseDefaultGraphicsAPIs(BuildTarget.Android, false);
        PlayerSettings.SetGraphicsAPIs(BuildTarget.Android, new[] { GraphicsDeviceType.OpenGLES3 });
        PlayerSettings.Android.targetArchitectures = arm64 ? AndroidArchitecture.ARM64 : AndroidArchitecture.ARMv7;
        PlayerSettings.Android.targetSdkVersion = (AndroidSdkVersions)36;
        PlayerSettings.Android.bundleVersionCode = 21;
        PlayerSettings.Android.useCustomKeystore = false;
        PlayerSettings.bundleVersion = "1.3.5";
        PlayerSettings.Android.forceInternetPermission = true;
        EditorUserBuildSettings.buildAppBundle = false;
        EditorUserBuildSettings.exportAsGoogleAndroidProject = false;

        foreach (string asset in NativePluginPaths())
        {
            var importer = AssetImporter.GetAtPath(asset) as PluginImporter;
            if (importer == null)
                throw new FileNotFoundException("Missing Android plugin", asset);
            if (!HasExpectedPluginSettings(importer))
            {
                importer.SetCompatibleWithAnyPlatform(false);
                importer.SetCompatibleWithEditor(false);
                importer.SetCompatibleWithPlatform(BuildTarget.Android, true);
                importer.SetPlatformData(BuildTarget.Android, "CPU", NativePluginCpu(asset));
                importer.SaveAndReimport();
            }
        }

    }

    internal static string[] NativePluginPaths()
    {
        return new[] { NativeSqliteArm, NativeSqliteArm64 };
    }

    internal static string NativePluginCpu(string asset) => asset == NativeSqliteArm64 ? "ARM64" : "ARMv7";

    internal static bool HasExpectedPluginSettings(PluginImporter importer)
    {
        return !importer.GetCompatibleWithAnyPlatform() && !importer.GetCompatibleWithEditor()
            && importer.GetCompatibleWithPlatform(BuildTarget.Android)
            && importer.GetPlatformData(BuildTarget.Android, "CPU") == NativePluginCpu(importer.assetPath);
    }
}
