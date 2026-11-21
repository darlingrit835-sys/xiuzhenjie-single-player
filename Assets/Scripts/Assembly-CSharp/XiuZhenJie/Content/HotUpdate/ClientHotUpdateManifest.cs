using System;
using System.IO;
using System.Security.Cryptography;
using System.Text;
using UnityEngine;

namespace XiuZhenJie
{
    [Serializable]
    internal sealed class ClientHotUpdateManifest
    {
        public int schema = 0;
        public string target = null;
        public string compatibility = null;
        public int version = 0;
        public string dllSha256 = null;
        public string[] metadata = null;

        internal const string ManifestFile = "manifest.json";
        internal const string SignatureFile = "manifest.sig";
        internal const string DllFile = "XZJ.HotUpdate.dll.bytes";
        internal const int MaxManifestBytes = 16 * 1024;
        internal const int MaxSignatureBytes = 1024;
        internal const int MaxDllBytes = 8 * 1024 * 1024;
        internal const int MaxMetadataBytes = 32 * 1024 * 1024;

        internal static ClientHotUpdateManifest Read(byte[] bytes, string expectedTarget)
        {
            if (bytes == null || bytes.Length == 0 || bytes.Length > MaxManifestBytes)
                return null;

            ClientHotUpdateManifest manifest;
            try
            {
                manifest = JsonUtility.FromJson<ClientHotUpdateManifest>(Encoding.UTF8.GetString(bytes));
            }
            catch (ArgumentException)
            {
                return null;
            }

            if (manifest == null || manifest.schema != 1 || manifest.target != expectedTarget ||
                manifest.version < 1 || !IsDigest(manifest.compatibility) || !IsDigest(manifest.dllSha256))
                return null;
            string[] names = manifest.metadata ?? Array.Empty<string>();
            if (names.Length > 32)
                return null;
            for (int i = 0; i < names.Length; i++)
            {
                if (!IsMetadataFile(names[i]) || Array.IndexOf(names, names[i]) != i)
                    return null;
            }
            manifest.metadata = names;
            return manifest;
        }

        internal bool IsNewerThan(ClientHotUpdateManifest baseline, int currentVersion)
        {
            return target == baseline.target && compatibility == baseline.compatibility && version > currentVersion;
        }

        internal bool MatchesDll(byte[] bytes)
        {
            if (bytes == null || bytes.Length == 0 || bytes.Length > MaxDllBytes)
                return false;
            using var sha256 = SHA256.Create();
            return string.Equals(dllSha256, BitConverter.ToString(sha256.ComputeHash(bytes)).Replace("-", ""),
                StringComparison.OrdinalIgnoreCase);
        }

        private static bool IsDigest(string digest)
        {
            if (digest == null || digest.Length != 64)
                return false;
            foreach (char c in digest)
                if ((c < '0' || c > '9') && (c < 'a' || c > 'f'))
                    return false;
            return true;
        }

        private static bool IsMetadataFile(string name)
        {
            if (string.IsNullOrEmpty(name) || name.Length > 120 || !name.EndsWith(".dll", StringComparison.Ordinal))
                return false;
            foreach (char c in name)
                if (!(c >= 'a' && c <= 'z' || c >= 'A' && c <= 'Z' || c >= '0' && c <= '9') && c != '_' && c != '-' && c != '.')
                    return false;
            return true;
        }
    }

    [Serializable]
    internal sealed class ClientHotUpdateConfig
    {
        public string manifestUrl = null;
        public string modulus = null;
        public string exponent = null;

        internal static ClientHotUpdateConfig Read(byte[] bytes, out Uri endpoint)
        {
            endpoint = null;
            if (bytes == null || bytes.Length == 0)
                return null;
            try
            {
                var config = JsonUtility.FromJson<ClientHotUpdateConfig>(Encoding.UTF8.GetString(bytes));
                if (config == null || !Uri.TryCreate(config.manifestUrl, UriKind.Absolute, out Uri url) ||
                    url.Scheme != Uri.UriSchemeHttps || url.UserInfo.Length != 0 || url.Query.Length != 0 ||
                    url.Fragment.Length != 0 || !url.AbsolutePath.EndsWith("/manifest.json", StringComparison.Ordinal) ||
                    config.PublicKey().Modulus.Length < 256)
                    return null;
                endpoint = url;
                return config;
            }
            catch (Exception error) when (error is ArgumentException || error is FormatException)
            {
                return null;
            }
        }

        internal bool Verifies(byte[] manifest, byte[] signature)
        {
            if (manifest == null || signature == null || signature.Length > ClientHotUpdateManifest.MaxSignatureBytes)
                return false;
            try
            {
                using RSA key = RSA.Create();
                key.ImportParameters(PublicKey());
                return key.VerifyData(manifest, signature, HashAlgorithmName.SHA256, RSASignaturePadding.Pkcs1);
            }
            catch (Exception error) when (error is CryptographicException || error is ArgumentException ||
                error is FormatException || error is NotSupportedException)
            {
                return false;
            }
        }

        private RSAParameters PublicKey()
        {
            return new RSAParameters
            {
                Modulus = Convert.FromBase64String(modulus ?? string.Empty),
                Exponent = Convert.FromBase64String(exponent ?? string.Empty)
            };
        }
    }

    internal sealed class ClientHotUpdatePatch
    {
        internal byte[] Manifest { get; }
        internal byte[] Signature { get; }
        internal byte[] Dll { get; }

        internal ClientHotUpdatePatch(byte[] manifest, byte[] signature, byte[] dll)
        {
            Manifest = manifest;
            Signature = signature;
            Dll = dll;
        }

        internal ClientHotUpdateManifest Verify(ClientHotUpdateConfig config, ClientHotUpdateManifest baseline, int currentVersion)
        {
            var manifest = ClientHotUpdateManifest.Read(Manifest, baseline.target);
            if (manifest == null || !manifest.IsNewerThan(baseline, currentVersion) ||
                !config.Verifies(Manifest, Signature) || !manifest.MatchesDll(Dll))
                return null;
            return manifest;
        }

        internal static ClientHotUpdatePatch Read(string path)
        {
            try
            {
                if (!File.Exists(path))
                    return null;
                using var reader = new BinaryReader(File.OpenRead(path));
                byte[] manifest = ReadBytes(reader, ClientHotUpdateManifest.MaxManifestBytes);
                byte[] signature = ReadBytes(reader, ClientHotUpdateManifest.MaxSignatureBytes);
                byte[] dll = ReadBytes(reader, ClientHotUpdateManifest.MaxDllBytes);
                return reader.BaseStream.Position == reader.BaseStream.Length
                    ? new ClientHotUpdatePatch(manifest, signature, dll) : null;
            }
            catch (Exception error) when (error is IOException || error is UnauthorizedAccessException || error is InvalidDataException)
            {
                return null;
            }
        }

        internal void Save(string path)
        {
            string staging = path + ".tmp";
            try
            {
                Directory.CreateDirectory(Path.GetDirectoryName(path));
                using (var writer = new BinaryWriter(File.Create(staging)))
                {
                    WriteBytes(writer, Manifest);
                    WriteBytes(writer, Signature);
                    WriteBytes(writer, Dll);
                    writer.Flush();
                    ((FileStream)writer.BaseStream).Flush(true);
                }
                if (File.Exists(path))
                    File.Replace(staging, path, null);
                else
                    File.Move(staging, path);
            }
            finally
            {
                if (File.Exists(staging))
                    File.Delete(staging);
            }
        }

        private static byte[] ReadBytes(BinaryReader reader, int limit)
        {
            int length = reader.ReadInt32();
            if (length <= 0 || length > limit)
                throw new InvalidDataException("Invalid patch blob size.");
            byte[] data = reader.ReadBytes(length);
            if (data.Length != length)
                throw new EndOfStreamException();
            return data;
        }

        private static void WriteBytes(BinaryWriter writer, byte[] data)
        {
            writer.Write(data.Length);
            writer.Write(data);
        }
    }
}
