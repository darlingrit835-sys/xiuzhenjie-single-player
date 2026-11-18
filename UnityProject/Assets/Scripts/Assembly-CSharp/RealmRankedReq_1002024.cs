using System;
using System.Diagnostics;
using Google.Protobuf;
using Google.Protobuf.Reflection;

public sealed class RealmRankedReq_1002024 : IMessage<RealmRankedReq_1002024>, IMessage, IEquatable<RealmRankedReq_1002024>, IDeepCloneable<RealmRankedReq_1002024>
{
	private static readonly MessageParser<RealmRankedReq_1002024> _parser = new MessageParser<RealmRankedReq_1002024>(() => new RealmRankedReq_1002024());

	private UnknownFieldSet _unknownFields;

	public const int TypeFieldNumber = 1;

	private int type_;

	[DebuggerNonUserCode]
	public static MessageParser<RealmRankedReq_1002024> Parser => _parser;

	[DebuggerNonUserCode]
	public static MessageDescriptor Descriptor => RoleProtoReflection.Descriptor.MessageTypes[26];

	[DebuggerNonUserCode]
	MessageDescriptor IMessage.Descriptor => pb_003A_003AGoogle_002EProtobuf_002EIMessage_002Eget_Descriptor();

	[DebuggerNonUserCode]
	public int Type
	{
		get
		{
			return type_;
		}
		set
		{
			type_ = value;
		}
	}

	private MessageDescriptor pb_003A_003AGoogle_002EProtobuf_002EIMessage_002Eget_Descriptor()
	{
		return Descriptor;
	}

	[DebuggerNonUserCode]
	public RealmRankedReq_1002024()
	{
	}

	[DebuggerNonUserCode]
	public RealmRankedReq_1002024(RealmRankedReq_1002024 other)
		: this()
	{
		type_ = other.type_;
		_unknownFields = UnknownFieldSet.Clone(other._unknownFields);
	}

	[DebuggerNonUserCode]
	public RealmRankedReq_1002024 Clone()
	{
		return new RealmRankedReq_1002024(this);
	}

	[DebuggerNonUserCode]
	public override bool Equals(object other)
	{
		return Equals(other as RealmRankedReq_1002024);
	}

	[DebuggerNonUserCode]
	public bool Equals(RealmRankedReq_1002024 other)
	{
		if (other == null)
		{
			return false;
		}
		if (other == this)
		{
			return true;
		}
		if (Type != other.Type)
		{
			return false;
		}
		return object.Equals(_unknownFields, other._unknownFields);
	}

	[DebuggerNonUserCode]
	public override int GetHashCode()
	{
		int num = 1;
		if (Type != 0)
		{
			num ^= Type.GetHashCode();
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
		if (Type != 0)
		{
			output.WriteRawTag(8);
			output.WriteInt32(Type);
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
		if (Type != 0)
		{
			num += 1 + CodedOutputStream.ComputeInt32Size(Type);
		}
		if (_unknownFields != null)
		{
			num += _unknownFields.CalculateSize();
		}
		return num;
	}

	[DebuggerNonUserCode]
	public void MergeFrom(RealmRankedReq_1002024 other)
	{
		if (other != null)
		{
			if (other.Type != 0)
			{
				Type = other.Type;
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
				Type = input.ReadInt32();
			}
		}
	}
}
