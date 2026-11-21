using System;
using System.IO;
using System.Linq;
using UnityEditor;
using UnityEditor.Build.Reporting;
using UnityEngine;

internal static class ClientBuild
{
    private const string StartupScene = "Assets/Scenes/StartLoadScene.unity";

    internal static string[] GetEnabledScenes(int? expectedSceneCount = null)
    {
        string[] scenes = EditorBuildSettings.scenes.Where(scene => scene.enabled).Select(scene => scene.path).ToArray();
        if (scenes.Length == 0 || scenes[0] != StartupScene)
            throw new InvalidOperationException("The first enabled scene must be " + StartupScene + ".");
        if (expectedSceneCount.HasValue && scenes.Length != expectedSceneCount.Value)
            throw new InvalidOperationException($"Expected {expectedSceneCount.Value} enabled scenes, found {scenes.Length}.");
        if (scenes.Distinct(StringComparer.Ordinal).Count() != scenes.Length)
            throw new InvalidOperationException("Duplicate enabled scenes in the build settings.");
        foreach (string scene in scenes)
        {
            if (AssetDatabase.LoadAssetAtPath<SceneAsset>(scene) == null)
                throw new InvalidOperationException("Enabled scene cannot be loaded: " + scene);
        }
        return scenes;
    }

    internal static string GetArtifactPath(string environmentVariable)
    {
        string output = Environment.GetEnvironmentVariable(environmentVariable);
        if (string.IsNullOrEmpty(output) || !Path.IsPathRooted(output))
            throw new ArgumentException(environmentVariable + " must contain an absolute build output path.");
        return output;
    }

    internal static void BuildPlayer(BuildPlayerOptions options, string logPrefix, string package)
    {
        Directory.CreateDirectory(Path.GetDirectoryName(options.locationPathName));
        Debug.Log(logPrefix + "_START " + package);
        BuildSummary result = BuildPipeline.BuildPlayer(options).summary;
        Debug.Log($"{logPrefix}_RESULT result={result.result} errors={result.totalErrors} bytes={result.totalSize}");
        if (result.result != BuildResult.Succeeded || result.totalErrors != 0)
            throw new InvalidOperationException($"{options.target} test build failed: {result.result}");
        AssetDatabase.SaveAssets();
    }
}
