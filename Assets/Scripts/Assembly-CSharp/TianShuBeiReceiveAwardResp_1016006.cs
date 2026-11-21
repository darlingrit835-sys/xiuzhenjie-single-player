using System;
using System.Diagnostics;
using Google.Protobuf;
using Google.Protobuf.Reflection;

public sealed class TianShuBeiReceiveAwardResp_1016006 : IMessage<TianShuBeiReceiveAwardResp_1016006>, IMessage, IEquatable<TianShuBeiReceiveAwardResp_1016006>, IDeepCloneable<TianShuBeiReceiveAwardResp_1016006>
{
	private static readonly MessageParser<TianShuBeiReceiveAwardResp_1016006> _parser = new MessageParser<TianShuBeiReceiveAwardResp_1016006>(() => new TianShuBeiReceiveAwardResp_1016006());

	private UnknownFieldSet _unknownFields;

	public const int CodeFieldNumber = 1;

	private int code_;

	public const int IsSuccessFieldNumber = 2;

	private bool isSuccess_;

	public const int IdFieldNumber = 3;

	private int id_;

	public const int MaxValFieldNumber = 4;

	private int maxVal_;

	public const int StatusFieldNumber = 5;

	private int status_;

	[DebuggerNonUserCode]
	public static MessageParser<TianShuBeiReceiveAwardResp_1016006> Parser => _parser;

	[DebuggerNonUserCode]
	public static MessageDescriptor Descriptor => TaskProtoReflection.Descriptor.MessageTypes[6];

	[DebuggerNonUserCode]
	MessageDescriptor IMessage.Descriptor => pb_003A_003AGoogle_002EProtobuf_002EIMessage_002Eget_Descriptor();

	[DebuggerNonUserCode]
	public int Code
	{
		get
		{
			return code_;
		}
		set
		{
			code_ = value;
		}
	}

	[DebuggerNonUserCode]
	public bool IsSuccess
	{
		get
		{
			return isSuccess_;
		}
		set
		{
			isSuccess_ = value;
		}
	}

	[DebuggerNonUserCode]
	public int Id
	{
		get
		{
			return id_;
		}
		set
		{
			id_ = value;
		}
	}

	[DebuggerNonUserCode]
	public int MaxVal
	{
		get
		{
			return maxVal_;
		}
		set
		{
			maxVal_ = value;
		}
	}

	[DebuggerNonUserCode]
	public int Status
	{
		get
		{
			return status_;
		}
		set
		{
			status_ = value;
		}
	}

	private MessageDescriptor pb_003A_003AGoogle_002EProtobuf_002EIMessage_002Eget_Descriptor()
	{
		return Descriptor;
	}

	[DebuggerNonUserCode]
	public TianShuBeiReceiveAwardResp_1016006()
	{
	}

	[DebuggerNonUserCode]
	public TianShuBeiReceiveAwardResp_1016006(TianShuBeiReceiveAwardResp_1016006 other)
		: this()
	{
		code_ = other.code_;
		isSuccess_ = other.isSuccess_;
		id_ = other.id_;
		maxVal_ = other.maxVal_;
		status_ = other.status_;
		_unknownFields = UnknownFieldSet.Clone(other._unknownFields);
	}

	[DebuggerNonUserCode]
	public TianShuBeiReceiveAwardResp_1016006 Clone()
	{
		return new TianShuBeiReceiveAwardResp_1016006(this);
	}

	[DebuggerNonUserCode]
	public override bool Equals(object other)
	{
		return Equals(other as TianShuBeiReceiveAwardResp_1016006);
	}

	[DebuggerNonUserCode]
	public bool Equals(TianShuBeiReceiveAwardResp_1016006 other)
	{
		if (other == null)
		{
			return false;
		}
		if (other == this)
		{
			return true;
		}
		if (Code != other.Code)
		{
			return false;
		}
		if (IsSuccess != other.IsSuccess)
		{
			return false;
		}
		if (Id != other.Id)
		{
			return false;
		}
		if (MaxVal != other.MaxVal)
		{
			return false;
		}
		if (Status != other.Status)
		{
			return false;
		}
		return object.Equals(_unknownFields, other._unknownFields);
	}

	[DebuggerNonUserCode]
	public override int GetHashCode()
	{
		int num = 1;
		if (Code != 0)
		{
			num ^= Code.GetHashCode();
		}
		if (IsSuccess)
		{
			num ^= IsSuccess.GetHashCode();
		}
		if (Id != 0)
		{
			num ^= Id.GetHashCode();
		}
		if (MaxVal != 0)
		{
			num ^= MaxVal.GetHashCode();
		}
		if (Status != 0)
		{
			num ^= Status.GetHashCode();
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
		if (Code != 0)
		{
			output.WriteRawTag(8);
			output.WriteInt32(Code);
		}
		if (IsSuccess)
		{
			output.WriteRawTag(16);
			output.WriteBool(IsSuccess);
		}
		if (Id != 0)
		{
			output.WriteRawTag(24);
			output.WriteInt32(Id);
		}
		if (MaxVal != 0)
		{
			output.WriteRawTag(32);
			output.WriteInt32(MaxVal);
		}
		if (Status != 0)
		{
			output.WriteRawTag(40);
			output.WriteInt32(Status);
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
		if (Code != 0)
		{
			num += 1 + CodedOutputStream.ComputeInt32Size(Code);
		}
		if (IsSuccess)
		{
			num += 2;
		}
		if (Id != 0)
		{
			num += 1 + CodedOutputStream.ComputeInt32Size(Id);
		}
		if (MaxVal != 0)
		{
			num += 1 + CodedOutputStream.ComputeInt32Size(MaxVal);
		}
		if (Status != 0)
		{
			num += 1 + CodedOutputStream.ComputeInt32Size(Status);
		}
		if (_unknownFields != null)
		{
			num += _unknownFields.CalculateSize();
		}
		return num;
	}

	[DebuggerNonUserCode]
	public void MergeFrom(TianShuBeiReceiveAwardResp_1016006 other)
	{
		if (other != null)
		{
			if (other.Code != 0)
			{
				Code = other.Code;
			}
			if (other.IsSuccess)
			{
				IsSuccess = other.IsSuccess;
			}
			if (other.Id != 0)
			{
				Id = other.Id;
			}
			if (other.MaxVal != 0)
			{
				MaxVal = other.MaxVal;
			}
			if (other.Status != 0)
			{
				Status = other.Status;
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
				Code = input.ReadInt32();
				break;
			case 16u:
				IsSuccess = input.ReadBool();
				break;
			case 24u:
				Id = input.ReadInt32();
				break;
			case 32u:
				MaxVal = input.ReadInt32();
				break;
			case 40u:
				Status = input.ReadInt32();
				break;
			}
		}
	}
}
