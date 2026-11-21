using System;
using Google.Protobuf;
using Google.Protobuf.WellKnownTypes;

namespace XiuZhenJie
{
	internal static class ForgeWire
	{
		internal static bool WorkshopEnabled(ByteString reply)
		{
			var input = reply.CreateCodedInput();
			uint tag;
			while ((tag = input.ReadTag()) != 0)
			{
				if (tag == 32) return input.ReadBool();
				input.SkipLastField();
			}
			return false;
		}

		// Request and reply share the same length-delimited JSON field. A missing
		// reply is distinct from an explicit application-level rejection.
		internal static void Send(MsgType command, string json, Action<bool, string> done)
		{
			IMessage message = null;
			if (json != null)
			{
				var bytes = new byte[1 + CodedOutputStream.ComputeStringSize(json)];
				var output = new CodedOutputStream(bytes);
				output.WriteTag(3, WireFormat.WireType.LengthDelimited);
				output.WriteString(json);
				output.Flush();
				message = Empty.Parser.ParseFrom(bytes);
			}
			if (!Singleton<NetManager>.Instance.TrySendGameRequest(command, message, reply =>
			{
				bool success = false;
				string payload = null;
				if (reply != null)
				{
					try
					{
						var input = reply.CreateCodedInput();
						uint tag;
						while ((tag = input.ReadTag()) != 0)
						{
							if (tag == 16) success = input.ReadBool();
							else if (tag == 26) payload = input.ReadString();
							else input.SkipLastField();
						}
					}
					catch (InvalidProtocolBufferException) { payload = null; }
				}
				done(success && payload != null, payload);
			}))
				done(false, null);
		}
	}
}
