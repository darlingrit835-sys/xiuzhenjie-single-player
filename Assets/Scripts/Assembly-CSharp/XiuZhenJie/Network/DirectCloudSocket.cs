using System;
using System.Collections.Generic;
using System.IO;
using System.Net.Security;
using System.Net.Sockets;
using System.Security.Authentication;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace XiuZhenJie
{
	internal sealed class DirectCloudSocket
	{
		private const int MaxFrame = 8 * 1024 * 1024;
		private const int MaxPendingFrames = 128;
		private const int MaxPendingBytes = 2 * MaxFrame + 10;
		private const int MaxUpgradeHeaders = 8192;
		private const int ConnectTimeoutMilliseconds = 8000;
		private const int WriteTimeoutMilliseconds = 8000;

		private readonly string endpoint;
		private readonly Action<DirectCloudSocket> connected;
		private readonly Action<DirectCloudSocket, byte[]> received;
		private readonly Action<DirectCloudSocket, bool> disconnected;
		private readonly TcpClient tcp = new TcpClient(AddressFamily.InterNetwork) { NoDelay = true };
		private readonly object gate = new object();
		private readonly Queue<byte[]> outbound = new Queue<byte[]>();
		private readonly SemaphoreSlim pending = new SemaphoreSlim(0);
		private readonly CancellationTokenSource stopping = new CancellationTokenSource();
		private bool stopped;
		private int queuedBytes;

		internal DirectCloudSocket(ServerType serverType, Action<DirectCloudSocket> connected,
			Action<DirectCloudSocket, byte[]> received, Action<DirectCloudSocket, bool> disconnected)
		{
			endpoint = serverType == ServerType.GamesNormalServer ? "/_xzj/game" : "/_xzj/battle";
			this.connected = connected;
			this.received = received;
			this.disconnected = disconnected;
		}

		internal void Start()
		{
			_ = Task.Run(RunAsync);
		}

		internal bool TrySend(byte[] frame)
		{
			lock (gate)
			{
				if (stopped || frame.Length > MaxFrame + 5 || outbound.Count >= MaxPendingFrames ||
					frame.Length > MaxPendingBytes - queuedBytes)
				{
					return false;
				}
				outbound.Enqueue(frame);
				queuedBytes += frame.Length;
				pending.Release();
				return true;
			}
		}

		internal void Close()
		{
			Stop();
		}

		private bool Stop()
		{
			lock (gate)
			{
				if (stopped)
				{
					return false;
				}
				stopped = true;
				outbound.Clear();
				queuedBytes = 0;
			}
			stopping.Cancel();
			tcp.Close();
			return true;
		}

		private async Task RunAsync()
		{
			SslStream tls = null;
			Task reader = null;
			Task writer = null;
			bool established = false;
			try
			{
				BufferedInput input;
				using (var timeout = new CancellationTokenSource(ConnectTimeoutMilliseconds))
				using (timeout.Token.Register(() => tcp.Close()))
				{
					await tcp.ConnectAsync(DirectCloudTrial.Host, DirectCloudTrial.Port).ConfigureAwait(false);
					tls = new SslStream(tcp.GetStream(), false, (_, certificate, __, ___) =>
						certificate != null && DirectCloudTrial.MatchesCertificate(certificate.GetRawCertData()));
					await tls.AuthenticateAsClientAsync(DirectCloudTrial.Host, null, SslProtocols.Tls12, false).ConfigureAwait(false);
					string request = $"GET {endpoint} HTTP/1.1\r\nHost: {DirectCloudTrial.Host}:{DirectCloudTrial.Port}\r\n" +
						"Connection: Upgrade\r\nUpgrade: xzj-pb\r\n" +
						$"{DirectCloudTrial.CapabilityHeader}: {DirectCloudTrial.Capability}\r\n\r\n";
					byte[] headers = Encoding.ASCII.GetBytes(request);
					await tls.WriteAsync(headers, 0, headers.Length, stopping.Token).ConfigureAwait(false);
					input = new BufferedInput(tls, stopping.Token);
					await ReadUpgradeAsync(input).ConfigureAwait(false);
					timeout.Token.ThrowIfCancellationRequested();
				}
				stopping.Token.ThrowIfCancellationRequested();
				established = true;
				connected(this);
				reader = ReadFramesAsync(input);
				writer = WriteFramesAsync(tls);
				Task ended = await Task.WhenAny(reader, writer).ConfigureAwait(false);
				await ended.ConfigureAwait(false);
			}
			catch (Exception ex) when (ex is IOException || ex is SocketException || ex is AuthenticationException ||
				ex is OperationCanceledException || ex is ObjectDisposedException || ex is InvalidOperationException)
			{
				// The callback reports a transport failure without copying request headers to the log.
			}
			finally
			{
				if (Stop())
				{
					disconnected(this, established);
				}
				if (reader != null && writer != null)
				{
					try
					{
						await Task.WhenAll(reader, writer).ConfigureAwait(false);
					}
					catch (Exception ex) when (ex is IOException || ex is SocketException ||
						ex is OperationCanceledException || ex is ObjectDisposedException)
					{
						// Closing the transport ends pending reads and writes.
					}
				}
				tls?.Dispose();
			}
		}

		private static async Task ReadUpgradeAsync(BufferedInput input)
		{
			var buffer = new byte[MaxUpgradeHeaders];
			for (int i = 0; i < buffer.Length; i++)
			{
				int value = await input.ReadByteAsync().ConfigureAwait(false);
				if (value < 0)
				{
					throw new EndOfStreamException("Incomplete cloud upgrade");
				}
				buffer[i] = (byte)value;
				if (i < 3 || buffer[i - 3] != '\r' || buffer[i - 2] != '\n' ||
					buffer[i - 1] != '\r' || buffer[i] != '\n')
				{
					continue;
				}
				string[] lines = Encoding.ASCII.GetString(buffer, 0, i + 1)
					.Split(new[] { "\r\n" }, StringSplitOptions.RemoveEmptyEntries);
				if (lines.Length == 0 || !lines[0].StartsWith("HTTP/1.1 101 ", StringComparison.Ordinal) ||
					!HasHeaderToken(lines, "Connection", "Upgrade") || !HasHeaderToken(lines, "Upgrade", "xzj-pb"))
				{
					throw new IOException("Cloud PB upgrade rejected");
				}
				return;
			}
			throw new IOException("Cloud upgrade headers exceed size limit");
		}

		private static bool HasHeaderToken(string[] lines, string header, string token)
		{
			foreach (string line in lines)
			{
				int separator = line.IndexOf(':');
				if (separator <= 0 || !line.Substring(0, separator).Equals(header, StringComparison.OrdinalIgnoreCase))
				{
					continue;
				}
				foreach (string value in line.Substring(separator + 1).Split(','))
				{
					if (value.Trim().Equals(token, StringComparison.OrdinalIgnoreCase))
					{
						return true;
					}
				}
			}
			return false;
		}

		private async Task ReadFramesAsync(BufferedInput input)
		{
			var prefix = new byte[5];
			while (!stopping.IsCancellationRequested)
			{
				uint length = 0;
				for (int i = 0; i < prefix.Length; i++)
				{
					int value = await input.ReadByteAsync().ConfigureAwait(false);
					if (value < 0)
					{
						if (i == 0)
						{
							return;
						}
						throw new EndOfStreamException("Incomplete PB frame length");
					}
					prefix[i] = (byte)value;
					length |= (uint)(value & 127) << (7 * i);
					if ((value & 128) == 0)
					{
						if (length > MaxFrame || i > 0 && (value == 0 || i == 4))
						{
							throw new IOException("Invalid PB frame length");
						}
						var frame = new byte[i + 1 + (int)length];
						Array.Copy(prefix, frame, i + 1);
						await input.ReadExactlyAsync(frame, i + 1).ConfigureAwait(false);
						received(this, frame);
						break;
					}
					if (i == prefix.Length - 1)
					{
						throw new IOException("Invalid PB frame prefix");
					}
				}
			}
		}

		private async Task WriteFramesAsync(Stream stream)
		{
			while (true)
			{
				await pending.WaitAsync(stopping.Token).ConfigureAwait(false);
				byte[] frame;
				lock (gate)
				{
					stopping.Token.ThrowIfCancellationRequested();
					if (stopped)
					{
						return;
					}
					frame = outbound.Dequeue();
					queuedBytes -= frame.Length;
				}
				using (var timeout = new CancellationTokenSource(WriteTimeoutMilliseconds))
				using (timeout.Token.Register(() => tcp.Close()))
				{
					await stream.WriteAsync(frame, 0, frame.Length, stopping.Token).ConfigureAwait(false);
					timeout.Token.ThrowIfCancellationRequested();
				}
			}
		}

		private sealed class BufferedInput
		{
			private readonly Stream stream;
			private readonly CancellationToken token;
			private readonly byte[] buffer = new byte[4096];
			private int cursor;
			private int limit;

			internal BufferedInput(Stream stream, CancellationToken token)
			{
				this.stream = stream;
				this.token = token;
			}

			internal async Task<int> ReadByteAsync()
			{
				if (cursor == limit)
				{
					cursor = 0;
					limit = await stream.ReadAsync(buffer, 0, buffer.Length, token).ConfigureAwait(false);
					if (limit == 0)
					{
						return -1;
					}
				}
				return buffer[cursor++];
			}

			internal async Task ReadExactlyAsync(byte[] target, int start)
			{
				int buffered = Math.Min(limit - cursor, target.Length - start);
				Array.Copy(buffer, cursor, target, start, buffered);
				cursor += buffered;
				start += buffered;
				while (start < target.Length)
				{
					int count = await stream.ReadAsync(target, start, target.Length - start, token).ConfigureAwait(false);
					if (count == 0)
					{
						throw new EndOfStreamException("Incomplete PB frame body");
					}
					start += count;
				}
			}
		}
	}
}
