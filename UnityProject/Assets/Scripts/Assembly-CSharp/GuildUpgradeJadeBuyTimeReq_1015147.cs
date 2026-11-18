using System;
using System.Diagnostics;
using Google.Protobuf;
using Google.Protobuf.Reflection;

public sealed class GuildUpgradeJadeBuyTimeReq_1015147 : IMessage<GuildUpgradeJadeBuyTimeReq_1015147>, IMessage, IEquatable<GuildUpgradeJadeBuyTimeReq_1015147>, IDeepCloneable<GuildUpgradeJadeBuyTimeReq_1015147>
{
	private static readonly MessageParser<GuildUpgradeJadeBuyTimeReq_1015147> _parser = new MessageParser<GuildUpgradeJadeBuyTimeReq_1015147>(() => new GuildUpgradeJadeBuyTimeReq_1015147());

	private UnknownFieldSet _unknownFields;

	public const int JadeFieldNumber = 1;

	private long jade_;

	[DebuggerNonUserCode]
	public static MessageParser<GuildUpgradeJadeBuyTimeReq_1015147> Parser => _parser;

	[DebuggerNonUserCode]
	public static MessageDescriptor Descriptor => GuildProtoReflection.Descriptor.MessageTypes[190];

	[DebuggerNonUserCode]
	MessageDescriptor IMessage.Descriptor => pb_003A_003AGoogle_002EProtobuf_002EIMessage_002Eget_Descriptor();

	[DebuggerNonUserCode]
	public long Jade
	{
		get
		{
			return jade_;
		}
		set
		{
			jade_ = value;
		}
	}

	private MessageDescriptor pb_003A_003AGoogle_002EProtobuf_002EIMessage_002Eget_Descriptor()
	{
		return Descriptor;
	}

	[DebuggerNonUserCode]
	public GuildUpgradeJadeBuyTimeReq_1015147()
	{
	}

	[DebuggerNonUserCode]
	public GuildUpgradeJadeBuyTimeReq_1015147(GuildUpgradeJadeBuyTimeReq_1015147 other)
		: this()
	{
		jade_ = other.jade_;
		_unknownFields = UnknownFieldSet.Clone(other._unknownFields);
	}

	[DebuggerNonUserCode]
	public GuildUpgradeJadeBuyTimeReq_1015147 Clone()
	{
		return new GuildUpgradeJadeBuyTimeReq_1015147(this);
	}

	[DebuggerNonUserCode]
	public override bool Equals(object other)
	{
		return Equals(other as GuildUpgradeJadeBuyTimeReq_1015147);
	}

	[DebuggerNonUserCode]
	public bool Equals(GuildUpgradeJadeBuyTimeReq_1015147 other)
	{
		if (other == null)
		{
			return false;
		}
		if (other == this)
		{
			return true;
		}
		if (Jade != other.Jade)
		{
			return false;
		}
		return object.Equals(_unknownFields, other._unknownFields);
	}

	[DebuggerNonUserCode]
	public override int GetHashCode()
	{
		int num = 1;
		if (Jade != 0L)
		{
			num ^= Jade.GetHashCode();
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
		if (Jade != 0L)
		{
			output.WriteRawTag(8);
			output.WriteInt64(Jade);
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
		if (Jade != 0L)
		{
			num += 1 + CodedOutputStream.ComputeInt64Size(Jade);
		}
		if (_unknownFields != null)
		{
			num += _unknownFields.CalculateSize();
		}
		return num;
	}

	[DebuggerNonUserCode]
	public void MergeFrom(GuildUpgradeJadeBuyTimeReq_1015147 other)
	{
		if (other != null)
		{
			if (other.Jade != 0L)
			{
				Jade = other.Jade;
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
				Jade = input.ReadInt64();
			}
		}
	}
}
