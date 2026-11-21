using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Text;

namespace XiuZhenJie.Client.Release
{
    internal static class ReleaseFiles
    {
        private const string ReceiptName = ".verified";

        internal static bool Matches(string path, ReleaseFile entry) =>
            File.Exists(path) && new FileInfo(path).Length == entry.size &&
            ReleaseIndex.HashFile(path) == entry.sha256;

        internal static bool UseInstalled(string directory, ReleaseIndex index)
        {
            if (!Directory.Exists(directory))
                return false;

            try
            {
                string receipt = CreateReceipt(directory, index);
                if (receipt == null)
                    return false;

                string path = Path.Combine(directory, ReceiptName);
                if (File.Exists(path) && new FileInfo(path).Length == receipt.Length &&
                    File.ReadAllText(path) == receipt)
                {
                    // Managed code is checked before it can be loaded.
                    return index.files.Where(entry => entry.name.EndsWith(".dll", StringComparison.Ordinal))
                        .All(entry => Matches(Path.Combine(directory, entry.name), entry));
                }

                // Adopt installations from older APKs and recheck files if their metadata changed.
                if (!index.files.All(entry => Matches(Path.Combine(directory, entry.name), entry)))
                    return false;
                WriteAtomic(path, Encoding.UTF8.GetBytes(receipt));
                return true;
            }
            catch (Exception error) when (error is IOException || error is UnauthorizedAccessException)
            {
                return false;
            }
        }

        private static string CreateReceipt(string directory, ReleaseIndex index)
        {
            var receipt = new StringBuilder(index.archiveSha256).Append('\n');
            foreach (ReleaseFile entry in index.files)
            {
                var file = new FileInfo(Path.Combine(directory, entry.name));
                if (!file.Exists || file.Length != entry.size)
                    return null;
                receipt.Append(file.LastWriteTimeUtc.Ticks).Append('\n');
            }
            return receipt.ToString();
        }

        internal static void WriteAtomic(string path, byte[] contents)
        {
            string staging = path + "-" + Guid.NewGuid().ToString("N") + ".tmp";
            try
            {
                File.WriteAllBytes(staging, contents);
                if (File.Exists(path))
                    File.Replace(staging, path, null);
                else
                    File.Move(staging, path);
            }
            finally
            {
                File.Delete(staging);
            }
        }

        internal static void AppendChunk(string download, string chunk, long offset, long count)
        {
            try
            {
                using var source = File.OpenRead(chunk);
                using var target = new FileStream(download, FileMode.OpenOrCreate, FileAccess.Write, FileShare.None);
                if (target.Length != offset || source.Length != count)
                    throw new InvalidDataException("更新分段大小不匹配。");
                target.Position = offset;
                CopyExact(source, target, count);
            }
            finally
            {
                File.Delete(chunk);
            }
        }

        internal static string IncrementalWork(string directory) => directory + ".installing";

        internal static ReleaseFile[] PrepareIncremental(string directory, ReleaseIndex index, params string[] sources)
        {
            string work = IncrementalWork(directory);
            Directory.CreateDirectory(Path.Combine(work, ".objects"));
            var missing = new List<ReleaseFile>();
            foreach (var entry in index.files)
            {
                string output = Path.Combine(work, entry.name);
                if (Matches(output, entry))
                    continue;

                bool reused = false;
                foreach (string source in sources)
                {
                    if (string.IsNullOrEmpty(source))
                        continue;
                    string candidate = Path.Combine(source, entry.name);
                    if (!File.Exists(candidate) || new FileInfo(candidate).Length != entry.size)
                        continue;
                    Directory.CreateDirectory(Path.GetDirectoryName(output));
                    File.Copy(candidate, output, overwrite: true);
                    if (Matches(output, entry))
                    {
                        reused = true;
                        break;
                    }
                }

                if (!reused)
                {
                    if (File.Exists(output))
                        File.Delete(output);
                    missing.Add(entry);
                }
            }
            return missing.ToArray();
        }

        internal static void InstallObject(string directory, string download, ReleaseFile[] entries)
        {
            if (!Matches(download, entries[0]))
            {
                File.Delete(download);
                throw new InvalidDataException("更新文件摘要不匹配。");
            }
            foreach (var entry in entries)
            {
                string output = Path.Combine(IncrementalWork(directory), entry.name);
                Directory.CreateDirectory(Path.GetDirectoryName(output));
                File.Copy(download, output, overwrite: true);
            }
            File.Delete(download);
        }

        internal static void CommitIncremental(string directory, ReleaseIndex index)
        {
            string work = IncrementalWork(directory);
            string receipt = CreateReceipt(work, index);
            if (receipt == null)
                throw new InvalidDataException("更新版本缺少文件。");
            File.WriteAllBytes(Path.Combine(work, ReceiptName), Encoding.UTF8.GetBytes(receipt));
            Directory.Delete(Path.Combine(work, ".objects"), true);
            if (Directory.Exists(directory))
                Directory.Delete(directory, true);
            Directory.Move(work, directory);
        }

        internal static void Install(string zipPath, string directory, ReleaseIndex index)
        {
            if (new FileInfo(zipPath).Length != index.archiveSize || ReleaseIndex.HashFile(zipPath) != index.archiveSha256)
                throw new InvalidDataException("发布包摘要不匹配。");

            string work = directory + "-" + Guid.NewGuid().ToString("N") + ".tmp";
            Directory.CreateDirectory(work);
            try
            {
                using (var zip = ZipFile.OpenRead(zipPath))
                {
                    if (zip.Entries.Count != index.files.Length)
                        throw new InvalidDataException("发布包包含未登记文件。");

                    foreach (var item in index.files)
                    {
                        var entry = zip.GetEntry(item.name);
                        if (entry == null || entry.Length != item.size)
                            throw new InvalidDataException("发布包文件大小不匹配。");
                        string output = Path.Combine(work, item.name);
                        Directory.CreateDirectory(Path.GetDirectoryName(output));
                        using (var source = entry.Open())
                        using (var target = File.Create(output))
                            CopyExact(source, target, item.size);
                        if (!Matches(output, item))
                            throw new InvalidDataException("发布包文件摘要不匹配。");
                    }
                }
                File.WriteAllBytes(Path.Combine(work, ReceiptName), Encoding.UTF8.GetBytes(CreateReceipt(work, index)));
                if (Directory.Exists(directory))
                    Directory.Delete(directory, true);
                Directory.Move(work, directory);
            }
            finally
            {
                if (Directory.Exists(work))
                    Directory.Delete(work, true);
            }
        }

        internal static void InstallFromPackage(string streamingAssets, string directory, ReleaseIndex index)
        {
            string asset = ReleaseIndex.SeedDirectory + "/" + ReleaseIndex.ArchiveName;
            if (Path.IsPathRooted(streamingAssets))
            {
                Install(Path.Combine(streamingAssets, asset), directory, index);
                return;
            }

            int separator = streamingAssets.IndexOf("!/", StringComparison.Ordinal);
            if (!streamingAssets.StartsWith("jar:", StringComparison.Ordinal) || separator <= 4 ||
                !Uri.TryCreate(streamingAssets.Substring(4, separator - 4), UriKind.Absolute, out var uri) || !uri.IsFile)
                throw new InvalidDataException("无法读取安装包中的客户端资源。");

            string temporary = directory + "-" + Guid.NewGuid().ToString("N") + ".zip.tmp";
            try
            {
                using (var apk = ZipFile.OpenRead(uri.LocalPath))
                {
                    var member = apk.GetEntry(streamingAssets.Substring(separator + 2).TrimEnd('/') + "/" + asset);
                    if (member == null || member.Length != index.archiveSize)
                        throw new InvalidDataException("安装包中的完整客户端缺失或大小不符。");
                    using var source = member.Open();
                    using var target = File.Create(temporary);
                    CopyExact(source, target, member.Length);
                }
                Install(temporary, directory, index);
            }
            finally
            {
                File.Delete(temporary);
            }
        }

        private static void CopyExact(Stream input, Stream output, long length)
        {
            byte[] buffer = new byte[64 * 1024];
            while (length != 0)
            {
                int read = input.Read(buffer, 0, (int)Math.Min(length, buffer.Length));
                if (read <= 0)
                    throw new EndOfStreamException();
                output.Write(buffer, 0, read);
                length -= read;
            }
            if (input.ReadByte() != -1)
                throw new InvalidDataException("发布包文件比清单更大。");
        }
    }
}
