using System;
using System.Diagnostics;
using Google.Protobuf;
using Google.Protobuf.Reflection;

public sealed class GuildJoinReq_1015004 : IMessage<GuildJoinReq_1015004>, IMessage, IEquatable<GuildJoinReq_1015004>, IDeepCloneable<GuildJoinReq_1015004>
{
	private static readonly MessageParser<GuildJoinReq_1015004> _parser = new MessageParser<GuildJoinReq_1015004>(() => new GuildJoinReq_1015004());

	private UnknownFieldSet _unknownFields;

	public const int GuildIdFieldNumber = 1;

	private int guildId_;

	[DebuggerNonUserCode]
	public static MessageParser<GuildJoinReq_1015004> Parser => _parser;

	[DebuggerNonUserCode]
	public static MessageDescriptor Descriptor => GuildProtoReflection.Descriptor.MessageTypes[4];

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
	public GuildJoinReq_1015004()
	{
	}

	[DebuggerNonUserCode]
	public GuildJoinReq_1015004(GuildJoinReq_1015004 other)
		: this()
	{
		guildId_ = other.guildId_;
		_unknownFields = UnknownFieldSet.Clone(other._unknownFields);
	}

	[DebuggerNonUserCode]
	public GuildJoinReq_1015004 Clone()
	{
		return new GuildJoinReq_1015004(this);
	}

	[DebuggerNonUserCode]
	public override bool Equals(object other)
	{
		return Equals(other as GuildJoinReq_1015004);
	}

	[DebuggerNonUserCode]
	public bool Equals(GuildJoinReq_1015004 other)
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
	public void MergeFrom(GuildJoinReq_1015004 other)
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
