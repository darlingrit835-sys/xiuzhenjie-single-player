using System;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using dnlib.DotNet;
using HybridCLR;
using HybridCLR.Commands;
using HybridCLR.Installer;
using HybridCLR.Settings;
using UnityEditor;
using UnityEditor.Build;
using UnityEngine;

internal static class ClientHotUpdateBuild
{
    private const string AssemblyName = "XZJ.HotUpdate";
    private const string AssemblyFile = AssemblyName + ".dll";
    private const string EntryTypeName = "XiuZhenJie.HotUpdate.MentorText";
    private const string PayloadFile = AssemblyFile + ".bytes";
    private const string ManifestFile = "manifest.json";
    private static readonly string[] MetadataNames = { "mscorlib.dll", "System.dll", "System.Core.dll" };

    private static string BaselineDirectory => Path.Combine(Application.streamingAssetsPath, "HotUpdate");

    [Serializable]
    private sealed class PayloadManifest
    {
        public int schema;
        public string target;
        public string compatibility;
        public int version;
        public string dllSha256;
        public string[] metadata;
    }

    [MenuItem("修真界/代码热更新/1 安装项目内 HybridCLR")]
    public static void Install()
    {
        var installer = new InstallerController();
        if (!installer.HasInstalledHybridCLR() || installer.InstalledLibil2cppVersion != installer.PackageVersion)
            installer.InstallDefaultHybridCLR();
        RequireInstallation();
        Debug.Log("XZJ_HOTUPDATE_INSTALL_OK " + installer.PackageVersion);
    }

    [MenuItem("修真界/代码热更新/2 生成随包基线")]
    public static void PrepareBaseline()
    {
        PreparePlayerBaseline(EditorUserBuildSettings.activeBuildTarget, EditorUserBuildSettings.development);
    }

    internal static void PreparePlayerBaseline(BuildTarget target, bool development)
    {
        RequireActiveTarget(target);
        RequireInstallation();
        string stripped = SettingsUtil.GetAssembliesPostIl2CppStripDir(target);
        string dll = Path.Combine(SettingsUtil.GetHotUpdateDllsOutputDirByTarget(target), AssemblyFile);
        if (File.Exists(dll))
            File.Delete(dll);

        bool previousDevelopment = EditorUserBuildSettings.development;
        try
        {
            EditorUserBuildSettings.development = development;
            PrebuildCommand.GenerateAll();
        }
        finally
        {
            EditorUserBuildSettings.development = previousDevelopment;
        }

        ValidateDll(dll);
        string[] metadata = MetadataNames.Where(name => File.Exists(Path.Combine(stripped, name))).ToArray();
        if (!metadata.Contains("mscorlib.dll"))
            throw new BuildFailedException("HybridCLR 未生成 mscorlib.dll 剪裁元数据。");

        string directory = BaselineDirectory;
        string metadataDirectory = Path.Combine(directory, "AOT");
        Directory.CreateDirectory(metadataDirectory);
        foreach (string name in metadata)
            WriteFile(Path.Combine(metadataDirectory, name + ".bytes"), File.ReadAllBytes(Path.Combine(stripped, name)));
        byte[] dllBytes = File.ReadAllBytes(dll);
        WriteFile(Path.Combine(directory, PayloadFile), dllBytes);
        var manifest = new PayloadManifest
        {
            schema = 1,
            target = target.ToString(),
            compatibility = Fingerprint(target, stripped),
            version = 1,
            dllSha256 = Hash(dllBytes),
            metadata = metadata,
        };
        WriteManifest(directory, manifest);
        AssetDatabase.Refresh(ImportAssetOptions.ForceSynchronousImport);
        Debug.Log($"XZJ_HOTUPDATE_BASELINE_OK target={target} compatibility={manifest.compatibility} metadata={string.Join(",", metadata)}");
    }

    public static void ExportPatch()
    {
        BuildTarget target = EditorUserBuildSettings.activeBuildTarget;
        RequireActiveTarget(target);
        string baseline = Environment.GetEnvironmentVariable("XIU_HOTUPDATE_BASELINE");
        if (string.IsNullOrEmpty(baseline))
            baseline = Path.Combine(BaselineDirectory, ManifestFile);
        if (!Path.IsPathRooted(baseline) || !File.Exists(baseline))
            throw new BuildFailedException("XIU_HOTUPDATE_BASELINE 必须指向已发布基线 manifest.json 的绝对路径。");
        PayloadManifest manifest = JsonUtility.FromJson<PayloadManifest>(File.ReadAllText(baseline, Encoding.UTF8));
        if (manifest == null || manifest.schema != 1 || manifest.version < 1 || manifest.target != target.ToString()
            || manifest.compatibility == null || manifest.compatibility.Length != 64
            || !manifest.compatibility.All(character => character is >= '0' and <= '9' or >= 'a' and <= 'f')
            || manifest.metadata == null || !manifest.metadata.Contains("mscorlib.dll"))
            throw new BuildFailedException("基线清单无效，或与 Unity 的当前目标平台不一致。");
        if (!int.TryParse(Environment.GetEnvironmentVariable("XIU_HOTUPDATE_VERSION"), NumberStyles.None, CultureInfo.InvariantCulture, out int version)
            || version <= manifest.version)
            throw new BuildFailedException("XIU_HOTUPDATE_VERSION 必须大于基线版本。");

        string output = ClientBuild.GetArtifactPath("XIU_HOTUPDATE_OUTPUT");
        if (Directory.Exists(output) && Directory.EnumerateFileSystemEntries(output).Any())
            throw new BuildFailedException("XIU_HOTUPDATE_OUTPUT 必须是空目录或尚未创建的目录。");

        string scratch = Path.Combine(SettingsUtil.HybridCLRDataDir, "XZJPatch-" + Guid.NewGuid().ToString("N"));
        try
        {
            CompileDllCommand.CompileDll(scratch, target, false);
            string compiled = Path.Combine(scratch, AssemblyFile);
            ValidateDll(compiled);
            byte[] dll = File.ReadAllBytes(compiled);
            Directory.CreateDirectory(output);
            WriteFile(Path.Combine(output, PayloadFile), dll);
            manifest.version = version;
            manifest.dllSha256 = Hash(dll);
            WriteManifest(output, manifest);
            Debug.Log($"XZJ_HOTUPDATE_PATCH_OK target={target} version={version} output={output}");
        }
        finally
        {
            if (Directory.Exists(scratch))
                Directory.Delete(scratch, true);
        }
    }

    private static void RequireActiveTarget(BuildTarget target)
    {
        if (EditorUserBuildSettings.activeBuildTarget != target)
            throw new BuildFailedException("先将 Unity 切换到目标平台再生成代码热更新文件。");
        string platform = target.ToString();
        if (target != BuildTarget.Android && target != BuildTarget.StandaloneOSX && platform != "OpenHarmony")
            throw new BuildFailedException("当前代码热更新入口支持 Android、macOS 与团结引擎 OpenHarmony 目标。");
        NamedBuildTarget namedTarget = NamedBuildTarget.FromBuildTargetGroup(BuildPipeline.GetBuildTargetGroup(target));
        if (PlayerSettings.GetScriptingBackend(namedTarget) != ScriptingImplementation.IL2CPP)
            throw new BuildFailedException("在 Player Settings 中为当前平台选择 IL2CPP 后再生成热更新文件。");
    }

    private static void RequireInstallation()
    {
        if (!HybridCLRSettings.instance.enable || HybridCLRSettings.instance.useGlobalIl2cpp
            || !SettingsUtil.HotUpdateAssemblyNamesExcludePreserved.Contains(AssemblyName))
            throw new BuildFailedException("HybridCLR 必须使用项目内 IL2CPP，并在设置中登记 XZJ.HotUpdate。");
        var installer = new InstallerController();
        if (!installer.HasInstalledHybridCLR() || installer.InstalledLibil2cppVersion != installer.PackageVersion)
            throw new BuildFailedException("请先执行「修真界 → 代码热更新 → 安装项目内 HybridCLR」。");
    }

    private static void ValidateDll(string path)
    {
        if (!File.Exists(path) || System.Reflection.AssemblyName.GetAssemblyName(path).Name != AssemblyName)
            throw new BuildFailedException("本次编译没有产出 " + AssemblyFile + "。");
        using var module = ModuleDefMD.Load(path);
        TypeDef entry = module.Find(EntryTypeName, false);
        if (entry == null || !entry.IsPublic || !HasEntry(entry, "Dialogue") || !HasEntry(entry, "Caption"))
            throw new BuildFailedException("热更新程序集缺少 MentorText 的 Dialogue(string) 或 Caption(string) 入口。");
    }

    private static bool HasEntry(TypeDef entry, string name)
    {
        return entry.Methods.Any(method => method.Name == name && method.IsPublic && method.IsStatic
            && !method.HasGenericParameters && method.MethodSig.RetType.ElementType == ElementType.String
            && method.MethodSig.Params.Count == 1 && method.MethodSig.Params[0].ElementType == ElementType.String);
    }

    private static string Fingerprint(BuildTarget target, string stripped)
    {
        var source = new StringBuilder($"{Application.unityVersion}\n{target}\n");
        foreach (string file in Directory.EnumerateFiles(stripped, "*.dll").OrderBy(Path.GetFileName, StringComparer.Ordinal))
            source.Append(Path.GetFileName(file)).Append(' ').Append(Hash(File.ReadAllBytes(file))).Append('\n');
        foreach (string file in Directory.EnumerateFiles(SettingsUtil.GeneratedCppDir, "*.cpp").OrderBy(Path.GetFileName, StringComparer.Ordinal))
            source.Append(Path.GetFileName(file)).Append(' ').Append(Hash(File.ReadAllBytes(file))).Append('\n');
        return Hash(Encoding.UTF8.GetBytes(source.ToString()));
    }

    private static string Hash(byte[] bytes)
    {
        using var sha = SHA256.Create();
        return BitConverter.ToString(sha.ComputeHash(bytes)).Replace("-", "").ToLowerInvariant();
    }

    private static void WriteManifest(string directory, PayloadManifest manifest)
    {
        WriteFile(Path.Combine(directory, ManifestFile), Encoding.UTF8.GetBytes(JsonUtility.ToJson(manifest, true) + "\n"));
    }

    private static void WriteFile(string path, byte[] contents)
    {
        string temporary = path + ".tmp";
        try
        {
            File.WriteAllBytes(temporary, contents);
            if (File.Exists(path))
                File.Replace(temporary, path, null);
            else
                File.Move(temporary, path);
        }
        finally
        {
            if (File.Exists(temporary))
                File.Delete(temporary);
        }
    }
}
