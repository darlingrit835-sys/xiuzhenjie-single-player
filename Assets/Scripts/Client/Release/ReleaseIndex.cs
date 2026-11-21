using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using UnityEngine;

namespace XiuZhenJie.Client.Release
{
    [Serializable]
    public sealed class ReleaseIndex
    {
        public int schema;
        public string target;
        public string compatibility;
        public int version;
        public long archiveSize;
        public string archiveSha256;
        public ReleaseFile[] files;

        public const string ArchiveName = "client-release.zip";
        public const string SeedDirectory = "FullUpdate/Seed";
        public const string SceneBundle = "xzj-scenes";
        public const string AssetBundle = "xzj-content";
        public const string LegacyDirectory = "Assetbundles";
        public const string GameAssembly = "Assembly-CSharp";
        public const string ProtocolAssembly = "XiuZhenJie.Client.Protocol";
        public const string MentorAssembly = "XZJ.HotUpdate";
        public const long MaxBytes = 1024L * 1024 * 1024;
        public static readonly string[] CodeAssemblies = { ProtocolAssembly, MentorAssembly, GameAssembly };

        public static bool Valid(ReleaseIndex index, ReleaseBase platform)
        {
            if (index == null || platform == null || index.schema != 2 || index.version < 1 ||
                index.target != platform.target || index.compatibility != platform.compatibility ||
                !Digest(index.archiveSha256) || index.archiveSize < 1 || index.archiveSize > MaxBytes ||
                index.files == null || index.files.Length > 1024)
                return false;

            var names = new HashSet<string>(StringComparer.OrdinalIgnoreCase);
            long total = 0;
            foreach (var file in index.files)
            {
                if (file == null || !KnownName(file.name) || !names.Add(file.name) || !Digest(file.sha256) ||
                    file.size <= 0 || file.size > MaxBytes)
                    return false;
                total += file.size;
                if (total > MaxBytes)
                    return false;
            }

            return CodeAssemblies.All(name => names.Contains(name + ".dll")) &&
                names.Contains(SceneBundle) && names.Contains(AssetBundle) &&
                names.Contains(LegacyDirectory + "/assetbundleconfig");
        }

        public static string HashFile(string path)
        {
            using var sha = SHA256.Create();
            using var contents = File.OpenRead(path);
            return BitConverter.ToString(sha.ComputeHash(contents)).Replace("-", "").ToLowerInvariant();
        }

        public static bool Digest(string value) =>
            value?.Length == 64 && value.All(c => c >= '0' && c <= '9' || c >= 'a' && c <= 'f');

        public static bool SimpleName(string name) =>
            !string.IsNullOrEmpty(name) && name != "." && name != ".." && name.Length <= 150 &&
            name.All(c => c >= 'a' && c <= 'z' || c >= 'A' && c <= 'Z' || c >= '0' && c <= '9' || c == '_' || c == '-' || c == '.');

        private static bool KnownName(string name) =>
            name == SceneBundle || name == AssetBundle ||
            name != null && CodeAssemblies.Any(assembly => name == assembly + ".dll") ||
            name != null && name.StartsWith(LegacyDirectory + "/", StringComparison.Ordinal) &&
            SimpleName(name.Substring(LegacyDirectory.Length + 1));
    }

    [Serializable]
    public sealed class ReleaseFile
    {
        public string name;
        public long size;
        public string sha256;
    }

    [Serializable]
    public sealed class ReleaseBase
    {
        public string target;
        public string compatibility;
        public string[] metadata;
    }

    [Serializable]
    public sealed class ReleaseChannel
    {
        public string manifestUrl;
        public string modulus;
        public string exponent;
        public string certificateSha256;
        public string gatewayCapability;
        public bool useEmailRegistration;

        public bool TryEndpoint(out Uri endpoint)
        {
            return Uri.TryCreate(manifestUrl, UriKind.Absolute, out endpoint) &&
                endpoint.Scheme == Uri.UriSchemeHttps && endpoint.UserInfo.Length == 0 &&
                endpoint.Query.Length == 0 && endpoint.Fragment.Length == 0 &&
                endpoint.AbsolutePath.EndsWith("/manifest.json", StringComparison.Ordinal) &&
                (string.IsNullOrEmpty(certificateSha256) || ReleaseIndex.Digest(certificateSha256)) &&
                (string.IsNullOrEmpty(gatewayCapability) || ReleaseIndex.Digest(certificateSha256) &&
                    gatewayCapability.Length >= 32 && gatewayCapability.Length <= 256 &&
                    gatewayCapability.All(c => c >= '!' && c <= '~')) &&
                HasPublicKey();
        }

        private bool HasPublicKey()
        {
            try
            {
                return ReadKey().Modulus.Length >= 256;
            }
            catch (FormatException)
            {
                return false;
            }
        }

        public bool Verify(byte[] content, byte[] signature)
        {
            if (content == null || signature == null || signature.Length == 0 || signature.Length > 1024)
                return false;
            try
            {
                using var rsa = RSA.Create();
                rsa.ImportParameters(ReadKey());
                return rsa.VerifyData(content, signature, HashAlgorithmName.SHA256, RSASignaturePadding.Pkcs1);
            }
            catch (Exception e) when (e is CryptographicException || e is ArgumentException ||
                e is FormatException || e is NotSupportedException)
            {
                return false;
            }
        }

        private RSAParameters ReadKey() => new RSAParameters
        {
            Modulus = Convert.FromBase64String(modulus ?? string.Empty),
            Exponent = Convert.FromBase64String(exponent ?? string.Empty)
        };
    }
}
