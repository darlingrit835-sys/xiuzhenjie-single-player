using System;
using System.Diagnostics;
using Google.Protobuf;
using Google.Protobuf.Reflection;

public sealed class RefiningMatrixReq_1005057 : IMessage<RefiningMatrixReq_1005057>, IMessage, IEquatable<RefiningMatrixReq_1005057>, IDeepCloneable<RefiningMatrixReq_1005057>
{
	private static readonly MessageParser<RefiningMatrixReq_1005057> _parser = new MessageParser<RefiningMatrixReq_1005057>(() => new RefiningMatrixReq_1005057());

	private UnknownFieldSet _unknownFields;

	public const int QuanTypeFieldNumber = 1;

	private int quanType_;

	public const int FangXiangFieldNumber = 2;

	private int fangXiang_;

	[DebuggerNonUserCode]
	public static MessageParser<RefiningMatrixReq_1005057> Parser => _parser;

	[DebuggerNonUserCode]
	public static MessageDescriptor Descriptor => HomeProtoReflection.Descriptor.MessageTypes[77];

	[DebuggerNonUserCode]
	MessageDescriptor IMessage.Descriptor => pb_003A_003AGoogle_002EProtobuf_002EIMessage_002Eget_Descriptor();

	[DebuggerNonUserCode]
	public int QuanType
	{
		get
		{
			return quanType_;
		}
		set
		{
			quanType_ = value;
		}
	}

	[DebuggerNonUserCode]
	public int FangXiang
	{
		get
		{
			return fangXiang_;
		}
		set
		{
			fangXiang_ = value;
		}
	}

	private MessageDescriptor pb_003A_003AGoogle_002EProtobuf_002EIMessage_002Eget_Descriptor()
	{
		return Descriptor;
	}

	[DebuggerNonUserCode]
	public RefiningMatrixReq_1005057()
	{
	}

	[DebuggerNonUserCode]
	public RefiningMatrixReq_1005057(RefiningMatrixReq_1005057 other)
		: this()
	{
		quanType_ = other.quanType_;
		fangXiang_ = other.fangXiang_;
		_unknownFields = UnknownFieldSet.Clone(other._unknownFields);
	}

	[DebuggerNonUserCode]
	public RefiningMatrixReq_1005057 Clone()
	{
		return new RefiningMatrixReq_1005057(this);
	}

	[DebuggerNonUserCode]
	public override bool Equals(object other)
	{
		return Equals(other as RefiningMatrixReq_1005057);
	}

	[DebuggerNonUserCode]
	public bool Equals(RefiningMatrixReq_1005057 other)
	{
		if (other == null)
		{
			return false;
		}
		if (other == this)
		{
			return true;
		}
		if (QuanType != other.QuanType)
		{
			return false;
		}
		if (FangXiang != other.FangXiang)
		{
			return false;
		}
		return object.Equals(_unknownFields, other._unknownFields);
	}

	[DebuggerNonUserCode]
	public override int GetHashCode()
	{
		int num = 1;
		if (QuanType != 0)
		{
			num ^= QuanType.GetHashCode();
		}
		if (FangXiang != 0)
		{
			num ^= FangXiang.GetHashCode();
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
		if (QuanType != 0)
		{
			output.WriteRawTag(8);
			output.WriteInt32(QuanType);
		}
		if (FangXiang != 0)
		{
			output.WriteRawTag(16);
			output.WriteInt32(FangXiang);
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
		if (QuanType != 0)
		{
			num += 1 + CodedOutputStream.ComputeInt32Size(QuanType);
		}
		if (FangXiang != 0)
		{
			num += 1 + CodedOutputStream.ComputeInt32Size(FangXiang);
		}
		if (_unknownFields != null)
		{
			num += _unknownFields.CalculateSize();
		}
		return num;
	}

	[DebuggerNonUserCode]
	public void MergeFrom(RefiningMatrixReq_1005057 other)
	{
		if (other != null)
		{
			if (other.QuanType != 0)
			{
				QuanType = other.QuanType;
			}
			if (other.FangXiang != 0)
			{
				FangXiang = other.FangXiang;
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
				QuanType = input.ReadInt32();
				break;
			case 16u:
				FangXiang = input.ReadInt32();
				break;
			}
		}
	}
}
