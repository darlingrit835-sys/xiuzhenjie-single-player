using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
using System.Runtime.Serialization.Formatters.Binary;
using System.Security.Cryptography;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.Networking;

namespace XiuZhenJie
{
	internal sealed class PackagedBundlePreparation
	{
		private const string BundleDirectory = "Assetbundles";
		private const int CopyBufferSize = 64 * 1024;
		private volatile int m_BundleCount;
		private volatile int m_PreparedCount;

		internal bool IsPreparing { get; private set; }
		internal float Progress => m_BundleCount == 0 ? 0f : (float)m_PreparedCount / m_BundleCount;

		internal IEnumerator Run(Action onReady, Action<string> onError)
		{
			IsPreparing = true;
			m_BundleCount = m_PreparedCount = 0;

			// Read Unity data on the main thread. Only file and checksum work runs on a worker.
			TextAsset manifest = ReleasedResources.Load<TextAsset>("ABMD5");
			if (manifest == null)
			{
				Fail("缺少本地资源清单，请检查安装包。", onError);
				yield break;
			}

			byte[] manifestBytes = manifest.bytes;
			string streamingRoot = XiuZhenJie.Client.Release.ReleaseRuntime.LegacySourceRoot ?? Application.streamingAssetsPath;
			string origin = Path.Combine(Application.persistentDataPath, "Origin");
			Task<List<ABMD5Base>> prepare = Task.Run(() => PrepareFromPackage(manifestBytes, streamingRoot, origin));
			yield return new WaitUntil(() => prepare.IsCompleted);
			if (prepare.IsFaulted)
			{
				Fail("本地资源校验或写入失败，请检查可用空间后重试。", onError, prepare.Exception);
				yield break;
			}

			// Some Android distributions expose StreamingAssets through a provider other than a readable APK.
			var pending = prepare.Result;
			byte[] providerBuffer = pending.Count == 0 ? null : new byte[CopyBufferSize];
			foreach (ABMD5Base entry in pending)
			{
				string url = streamingRoot.TrimEnd('/') + "/" + BundleDirectory + "/" + Uri.EscapeDataString(entry.Name);
				using (UnityWebRequest request = UnityWebRequest.Get(url))
				{
					request.timeout = 30;
					yield return request.SendWebRequest();
					if (request.result != UnityWebRequest.Result.Success)
					{
						Fail("安装包中的本地资源读取失败，请重试。", onError, request.error);
						yield break;
					}

					byte[] data = request.downloadHandler.data;
					Task repair = Task.Run(() =>
					{
						using (var input = new MemoryStream(data, writable: false))
						using (MD5 checksum = MD5.Create())
							InstallBundle(input, Path.Combine(origin, entry.Name), entry, checksum, providerBuffer);
						m_PreparedCount++;
					});
					yield return new WaitUntil(() => repair.IsCompleted);
					if (repair.IsFaulted)
					{
						Fail("本地资源校验或写入失败，请检查可用空间后重试。", onError, repair.Exception);
						yield break;
					}
				}
			}

			IsPreparing = false;
			onReady();
		}

		private List<ABMD5Base> PrepareFromPackage(byte[] manifestBytes, string streamingRoot, string origin)
		{
			// This is the same bundled manifest consumed by the recovered resource library.
			List<ABMD5Base> entries;
			using (var data = new MemoryStream(manifestBytes, writable: false))
				entries = ((ABMD5)new BinaryFormatter().Deserialize(data)).ABMD5List;
			if (entries == null || entries.Count == 0)
				throw new InvalidDataException("Empty packaged bundle manifest.");

			m_BundleCount = entries.Count;
			Directory.CreateDirectory(origin);
			bool fromDirectory = Path.IsPathRooted(streamingRoot);
			var pending = new List<ABMD5Base>();
			byte[] copyBuffer = null;
			using (ZipArchive apk = OpenApk(streamingRoot, out string zipDirectory))
			using (MD5 checksum = MD5.Create())
			{
				foreach (ABMD5Base entry in entries)
				{
					string destination = Path.Combine(origin, entry.Name);
					if (File.Exists(destination) && Matches(destination, entry.Md5, checksum))
					{
						m_PreparedCount++;
						continue;
					}

					ZipArchiveEntry packageEntry = apk?.GetEntry(zipDirectory + entry.Name);
					if (!fromDirectory && packageEntry == null)
					{
						pending.Add(entry);
						continue;
					}

					using (Stream input = fromDirectory
						? File.OpenRead(Path.Combine(streamingRoot, BundleDirectory, entry.Name))
						: packageEntry.Open())
						InstallBundle(input, destination, entry, checksum, copyBuffer ??= new byte[CopyBufferSize]);
					m_PreparedCount++;
				}
			}
			return pending;
		}

		private static ZipArchive OpenApk(string streamingRoot, out string bundleDirectory)
		{
			bundleDirectory = null;
			int entryStart = streamingRoot.IndexOf("!/", StringComparison.Ordinal);
			if (!streamingRoot.StartsWith("jar:", StringComparison.Ordinal) || entryStart < 4 ||
				!Uri.TryCreate(streamingRoot.Substring(4, entryStart - 4), UriKind.Absolute, out Uri apkUri) || !apkUri.IsFile)
				return null;

			try
			{
				bundleDirectory = streamingRoot.Substring(entryStart + 2).TrimEnd('/') + "/" + BundleDirectory + "/";
				return ZipFile.OpenRead(apkUri.LocalPath);
			}
			catch (Exception ex) when (ex is IOException || ex is UnauthorizedAccessException || ex is InvalidDataException)
			{
				return null;
			}
		}

		private static void InstallBundle(Stream source, string destination, ABMD5Base entry, MD5 checksum, byte[] buffer)
		{
			string temporary = destination + ".unpacking";
			try
			{
				checksum.Initialize();
				using (var file = File.Create(temporary))
				{
					int count;
					while ((count = source.Read(buffer, 0, buffer.Length)) != 0)
					{
						checksum.TransformBlock(buffer, 0, count, null, 0);
						file.Write(buffer, 0, count);
					}
				}
				checksum.TransformFinalBlock(Array.Empty<byte>(), 0, 0);
				if (!MatchesChecksum(checksum.Hash, entry.Md5))
					throw new InvalidDataException("Unexpected checksum for packaged bundle " + entry.Name);

				if (File.Exists(destination))
					File.Replace(temporary, destination, null);
				else
					File.Move(temporary, destination);
			}
			finally
			{
				if (File.Exists(temporary))
					File.Delete(temporary);
			}
		}

		private static bool Matches(string path, string expected, MD5 checksum)
		{
			using (var file = File.OpenRead(path))
				return MatchesChecksum(checksum.ComputeHash(file), expected);
		}

		private static bool MatchesChecksum(byte[] digest, string expected) =>
			string.Equals(expected, BitConverter.ToString(digest).Replace("-", ""), StringComparison.OrdinalIgnoreCase);

		private void Fail(string message, Action<string> onError, object detail = null)
		{
			IsPreparing = false;
			if (detail != null)
				Debug.LogWarning("Packaged bundle preparation failed: " + detail);
			onError(message);
		}
	}
}
