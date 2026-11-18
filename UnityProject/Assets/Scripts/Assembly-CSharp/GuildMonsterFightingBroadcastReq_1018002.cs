using System;
using System.Diagnostics;
using Google.Protobuf;
using Google.Protobuf.Reflection;

public sealed class GuildMonsterFightingBroadcastReq_1018002 : IMessage<GuildMonsterFightingBroadcastReq_1018002>, IMessage, IEquatable<GuildMonsterFightingBroadcastReq_1018002>, IDeepCloneable<GuildMonsterFightingBroadcastReq_1018002>
{
	private static readonly MessageParser<GuildMonsterFightingBroadcastReq_1018002> _parser = new MessageParser<GuildMonsterFightingBroadcastReq_1018002>(() => new GuildMonsterFightingBroadcastReq_1018002());

	private UnknownFieldSet _unknownFields;

	public const int GuildIdFieldNumber = 1;

	private int guildId_;

	[DebuggerNonUserCode]
	public static MessageParser<GuildMonsterFightingBroadcastReq_1018002> Parser => _parser;

	[DebuggerNonUserCode]
	public static MessageDescriptor Descriptor => GuildFightProtoReflection.Descriptor.MessageTypes[1];

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

	private MessageDescriptor pb_003A_003AGoogle_002EProtobuf_002EIMessage_002Eget_Descriptor()
	{
		return Descriptor;
	}

	[DebuggerNonUserCode]
	public GuildMonsterFightingBroadcastReq_1018002()
	{
	}

	[DebuggerNonUserCode]
	public GuildMonsterFightingBroadcastReq_1018002(GuildMonsterFightingBroadcastReq_1018002 other)
		: this()
	{
		guildId_ = other.guildId_;
		_unknownFields = UnknownFieldSet.Clone(other._unknownFields);
	}

	[DebuggerNonUserCode]
	public GuildMonsterFightingBroadcastReq_1018002 Clone()
	{
		return new GuildMonsterFightingBroadcastReq_1018002(this);
	}

	[DebuggerNonUserCode]
	public override bool Equals(object other)
	{
		return Equals(other as GuildMonsterFightingBroadcastReq_1018002);
	}

	[DebuggerNonUserCode]
	public bool Equals(GuildMonsterFightingBroadcastReq_1018002 other)
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
		if (_unknownFields != null)
		{
			num += _unknownFields.CalculateSize();
		}
		return num;
	}

	[DebuggerNonUserCode]
	public void MergeFrom(GuildMonsterFightingBroadcastReq_1018002 other)
	{
		if (other != null)
		{
			if (other.GuildId != 0)
			{
				GuildId = other.GuildId;
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
			if (num != 8)
			{
				_unknownFields = UnknownFieldSet.MergeFieldFrom(_unknownFields, input);
			}
			else
			{
				GuildId = input.ReadInt32();
			}
		}
	}
}
