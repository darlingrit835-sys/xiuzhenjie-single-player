using System;
using System.Diagnostics;
using Google.Protobuf;
using Google.Protobuf.Reflection;

public sealed class UpgradeLingTianMatrixJadeBuyTimeReq_1015063 : IMessage<UpgradeLingTianMatrixJadeBuyTimeReq_1015063>, IMessage, IEquatable<UpgradeLingTianMatrixJadeBuyTimeReq_1015063>, IDeepCloneable<UpgradeLingTianMatrixJadeBuyTimeReq_1015063>
{
	private static readonly MessageParser<UpgradeLingTianMatrixJadeBuyTimeReq_1015063> _parser = new MessageParser<UpgradeLingTianMatrixJadeBuyTimeReq_1015063>(() => new UpgradeLingTianMatrixJadeBuyTimeReq_1015063());

	private UnknownFieldSet _unknownFields;

	public const int JadeFieldNumber = 1;

	private long jade_;

	[DebuggerNonUserCode]
	public static MessageParser<UpgradeLingTianMatrixJadeBuyTimeReq_1015063> Parser => _parser;

	[DebuggerNonUserCode]
	public static MessageDescriptor Descriptor => GuildProtoReflection.Descriptor.MessageTypes[72];

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
	public UpgradeLingTianMatrixJadeBuyTimeReq_1015063()
	{
	}

	[DebuggerNonUserCode]
	public UpgradeLingTianMatrixJadeBuyTimeReq_1015063(UpgradeLingTianMatrixJadeBuyTimeReq_1015063 other)
		: this()
	{
		jade_ = other.jade_;
		_unknownFields = UnknownFieldSet.Clone(other._unknownFields);
	}

	[DebuggerNonUserCode]
	public UpgradeLingTianMatrixJadeBuyTimeReq_1015063 Clone()
	{
		return new UpgradeLingTianMatrixJadeBuyTimeReq_1015063(this);
	}

	[DebuggerNonUserCode]
	public override bool Equals(object other)
	{
		return Equals(other as UpgradeLingTianMatrixJadeBuyTimeReq_1015063);
	}

	[DebuggerNonUserCode]
	public bool Equals(UpgradeLingTianMatrixJadeBuyTimeReq_1015063 other)
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
	public void MergeFrom(UpgradeLingTianMatrixJadeBuyTimeReq_1015063 other)
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
