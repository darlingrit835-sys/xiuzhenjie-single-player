using System;
using System.Diagnostics;
using Google.Protobuf;
using Google.Protobuf.Reflection;

public sealed class GuildMonsterFightMessageBroadcastResp_1018005 : IMessage<GuildMonsterFightMessageBroadcastResp_1018005>, IMessage, IEquatable<GuildMonsterFightMessageBroadcastResp_1018005>, IDeepCloneable<GuildMonsterFightMessageBroadcastResp_1018005>
{
	private static readonly MessageParser<GuildMonsterFightMessageBroadcastResp_1018005> _parser = new MessageParser<GuildMonsterFightMessageBroadcastResp_1018005>(() => new GuildMonsterFightMessageBroadcastResp_1018005());

	private UnknownFieldSet _unknownFields;

	public const int GuildIdFieldNumber = 1;

	private int guildId_;

	public const int MessageFieldNumber = 2;

	private string message_ = "";

	[DebuggerNonUserCode]
	public static MessageParser<GuildMonsterFightMessageBroadcastResp_1018005> Parser => _parser;

	[DebuggerNonUserCode]
	public static MessageDescriptor Descriptor => GuildFightProtoReflection.Descriptor.MessageTypes[6];

	[DebuggerNonUserCode]
	MessageDescriptor IMessage.Descriptor => pb_003A_003AGoogle_002EProtobuf_002EIMessage_002Eget_Descriptor();

	[DebuggerNonUserCode]
	public int GuildId
	{
		get
		{
			return guildId_;
		}
		set
		{
			guildId_ = value;
		}
	}

	[DebuggerNonUserCode]
	public string Message
	{
		get
		{
			return message_;
		}
		set
		{
			message_ = ProtoPreconditions.CheckNotNull(value, "value");
		}
	}

	private MessageDescriptor pb_003A_003AGoogle_002EProtobuf_002EIMessage_002Eget_Descriptor()
	{
		return Descriptor;
	}

	[DebuggerNonUserCode]
	public GuildMonsterFightMessageBroadcastResp_1018005()
	{
	}

	[DebuggerNonUserCode]
	public GuildMonsterFightMessageBroadcastResp_1018005(GuildMonsterFightMessageBroadcastResp_1018005 other)
		: this()
	{
		guildId_ = other.guildId_;
		message_ = other.message_;
		_unknownFields = UnknownFieldSet.Clone(other._unknownFields);
	}

	[DebuggerNonUserCode]
	public GuildMonsterFightMessageBroadcastResp_1018005 Clone()
	{
		return new GuildMonsterFightMessageBroadcastResp_1018005(this);
	}

	[DebuggerNonUserCode]
	public override bool Equals(object other)
	{
		return Equals(other as GuildMonsterFightMessageBroadcastResp_1018005);
	}

	[DebuggerNonUserCode]
	public bool Equals(GuildMonsterFightMessageBroadcastResp_1018005 other)
	{
		if (other == null)
		{
			return false;
		}
		if (other == this)
		{
			return true;
		}
		if (GuildId != other.GuildId)
		{
			return false;
		}
		if (Message != other.Message)
		{
			return false;
		}
		return object.Equals(_unknownFields, other._unknownFields);
	}

	[DebuggerNonUserCode]
	public override int GetHashCode()
	{
		int num = 1;
		if (GuildId != 0)
		{
			num ^= GuildId.GetHashCode();
		}
		if (Message.Length != 0)
		{
			num ^= Message.GetHashCode();
		}
		if (_unknownFields != null)
		{
			num ^= _unknownFields.GetHashCode();
		}
		return num;
	}

	[DebuggerNonUserCode]
	public override string ToString()
	{
		return JsonFormatter.ToDiagnosticString(this);
	}

	[DebuggerNonUserCode]
	public void WriteTo(CodedOutputStream output)
	{
		if (GuildId != 0)
		{
			output.WriteRawTag(8);
			output.WriteInt32(GuildId);
		}
		if (Message.Length != 0)
		{
			output.WriteRawTag(18);
			output.WriteString(Message);
		}
		if (_unknownFields != null)
		{
			_unknownFields.WriteTo(output);
		}
	}

	[DebuggerNonUserCode]
	public int CalculateSize()
	{
		int num = 0;
		if (GuildId != 0)
		{
			num += 1 + CodedOutputStream.ComputeInt32Size(GuildId);
		}
		if (Message.Length != 0)
		{
			num += 1 + CodedOutputStream.ComputeStringSize(Message);
		}
		if (_unknownFields != null)
		{
			num += _unknownFields.CalculateSize();
		}
		return num;
	}

	[DebuggerNonUserCode]
	public void MergeFrom(GuildMonsterFightMessageBroadcastResp_1018005 other)
	{
		if (other != null)
		{
			if (other.GuildId != 0)
			{
				GuildId = other.GuildId;
			}
			if (other.Message.Length != 0)
			{
				Message = other.Message;
			}
			_unknownFields = UnknownFieldSet.MergeFrom(_unknownFields, other._unknownFields);
		}
	}

	[DebuggerNonUserCode]
	public void MergeFrom(CodedInputStream input)
	{
		uint num;
		while ((num = input.ReadTag()) != 0)
		{
			switch (num)
			{
			default:
				_unknownFields = UnknownFieldSet.MergeFieldFrom(_unknownFields, input);
				break;
			case 8u:
				GuildId = input.ReadInt32();
				break;
			case 18u:
				Message = input.ReadString();
				break;
			}
		}
	}
}
