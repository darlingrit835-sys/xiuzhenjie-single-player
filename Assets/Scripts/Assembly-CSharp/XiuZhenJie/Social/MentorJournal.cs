using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Security.Cryptography;
using System.Text;
using UnityEngine;

namespace XiuZhenJie
{
	internal static class MentorJournal
	{
		private const int Capacity = 40;
		private const int MaxFileBytes = 256 * 1024;
		private static readonly object FileGate = new object();

		[Serializable]
		private sealed class Journal
		{
			public List<Entry> entries = new List<Entry>();
		}

		[Serializable]
		private sealed class Entry
		{
			public string atUtc;
			public string title;
			public string body;
		}

		internal static void Append(string title, string body)
		{
			if (string.IsNullOrWhiteSpace(title) || !TryGetPath(out string path))
				return;

			lock (FileGate)
			{
				try
				{
					Journal journal = Read(path);
					journal.entries.Insert(0, new Entry
					{
						atUtc = DateTime.UtcNow.ToString("o", CultureInfo.InvariantCulture),
						title = Clip(title, 80),
						body = Clip(body, 420)
					});
					if (journal.entries.Count > Capacity)
						journal.entries.RemoveRange(Capacity, journal.entries.Count - Capacity);
					Write(path, JsonUtility.ToJson(journal));
				}
				catch (Exception error) when (error is IOException || error is UnauthorizedAccessException || error is NotSupportedException)
				{
					Debug.LogWarning("无法写入本机师徒对话记录。");
				}
			}
		}

		internal static string ReadText()
		{
			if (!TryGetPath(out string path))
				return string.Empty;

			lock (FileGate)
			{
				try
				{
					var copy = new StringBuilder();
					List<Entry> entries = Read(path).entries;
					for (int index = 0; index < Math.Min(entries.Count, Capacity); index++)
					{
						Entry entry = entries[index];
						if (entry == null || string.IsNullOrEmpty(entry.title))
							continue;
						if (copy.Length > 0)
							copy.Append("\n\n");
						if (DateTime.TryParseExact(entry.atUtc, "o", CultureInfo.InvariantCulture, DateTimeStyles.RoundtripKind, out DateTime stamp))
							copy.Append(stamp.ToLocalTime().ToString("MM-dd HH:mm", CultureInfo.InvariantCulture)).Append("  ");
						copy.Append(Clip(entry.title, 80));
						if (!string.IsNullOrWhiteSpace(entry.body))
							copy.Append('\n').Append(Clip(entry.body, 420));
					}
					return copy.ToString();
				}
				catch (Exception error) when (error is IOException || error is UnauthorizedAccessException || error is NotSupportedException)
				{
					return string.Empty;
				}
			}
		}

		private static bool TryGetPath(out string path)
		{
			path = null;
			var role = Singleton<RoleManager>.Instance;
			string roleId = role.Role_Entity?.Id;
			string root = Application.persistentDataPath;
			if (string.IsNullOrEmpty(roleId) || string.IsNullOrEmpty(root) || string.IsNullOrEmpty(Constants.LoginServerIP))
				return false;

			string host = DirectCloudTrial.Enabled ? DirectCloudTrial.Host : Constants.SocketServerIP;
			int port = DirectCloudTrial.Enabled ? DirectCloudTrial.Port : Constants.SocketServerPort;
			if (string.IsNullOrEmpty(host) || port <= 0 || Constants.LoginServerPort <= 0)
				return false;

			string scope = string.Join("\n", Constants.LoginServerIP.ToLowerInvariant(),
				Constants.LoginServerPort.ToString(CultureInfo.InvariantCulture), host.ToLowerInvariant(),
				port.ToString(CultureInfo.InvariantCulture), role.ServerId, role.UserId, roleId);
			using var hash = SHA256.Create();
			string fileName = BitConverter.ToString(hash.ComputeHash(Encoding.UTF8.GetBytes(scope))).Replace("-", "");
			path = Path.Combine(root, "MentorJournal", fileName + ".json");
			return true;
		}

		private static Journal Read(string path)
		{
			var file = new FileInfo(path);
			if (file.Exists && file.Length <= MaxFileBytes)
			{
				try
				{
					var loaded = JsonUtility.FromJson<Journal>(File.ReadAllText(path, Encoding.UTF8));
					if (loaded?.entries != null)
						return loaded;
				}
				catch (ArgumentException)
				{
					// A malformed file should not stop the game or prevent recording new dialogue.
				}
			}
			return new Journal();
		}

		private static void Write(string path, string json)
		{
			Directory.CreateDirectory(Path.GetDirectoryName(path));
			string temporary = path + "." + Guid.NewGuid().ToString("N") + ".tmp";
			try
			{
				byte[] bytes = Encoding.UTF8.GetBytes(json);
				using (var file = new FileStream(temporary, FileMode.CreateNew, FileAccess.Write, FileShare.None))
				{
					file.Write(bytes, 0, bytes.Length);
					file.Flush(true);
				}
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

		private static string Clip(string text, int limit)
		{
			text = text?.Trim() ?? string.Empty;
			if (text.Length <= limit)
				return text;
			int length = char.IsHighSurrogate(text[limit - 1]) ? limit - 1 : limit;
			return text.Substring(0, length) + "…";
		}
	}
}
