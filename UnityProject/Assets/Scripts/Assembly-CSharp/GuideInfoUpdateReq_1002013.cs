using System;
using System.Diagnostics;
using Google.Protobuf;
using Google.Protobuf.Reflection;

public sealed class GuideInfoUpdateReq_1002013 : IMessage<GuideInfoUpdateReq_1002013>, IMessage, IEquatable<GuideInfoUpdateReq_1002013>, IDeepCloneable<GuideInfoUpdateReq_1002013>
{
	private static readonly MessageParser<GuideInfoUpdateReq_1002013> _parser = new MessageParser<GuideInfoUpdateReq_1002013>(() => new GuideInfoUpdateReq_1002013());

	private UnknownFieldSet _unknownFields;

	public const int TypeFieldNumber = 1;

	private int type_;

	[DebuggerNonUserCode]
	public static MessageParser<GuideInfoUpdateReq_1002013> Parser => _parser;

	[DebuggerNonUserCode]
	public static MessageDescriptor Descriptor => RoleProtoReflection.Descriptor.MessageTypes[14];

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
	public GuideInfoUpdateReq_1002013()
	{
	}

	[DebuggerNonUserCode]
	public GuideInfoUpdateReq_1002013(GuideInfoUpdateReq_1002013 other)
		: this()
	{
		type_ = other.type_;
		_unknownFields = UnknownFieldSet.Clone(other._unknownFields);
	}

	[DebuggerNonUserCode]
	public GuideInfoUpdateReq_1002013 Clone()
	{
		return new GuideInfoUpdateReq_1002013(this);
	}

	[DebuggerNonUserCode]
	public override bool Equals(object other)
	{
		return Equals(other as GuideInfoUpdateReq_1002013);
	}

	[DebuggerNonUserCode]
	public bool Equals(GuideInfoUpdateReq_1002013 other)
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
	public void MergeFrom(GuideInfoUpdateReq_1002013 other)
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
