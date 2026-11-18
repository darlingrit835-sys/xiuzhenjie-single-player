using System;
using System.Diagnostics;
using Google.Protobuf;
using Google.Protobuf.Reflection;

public sealed class VerifyCreateMethodSignResp_1008047 : IMessage<VerifyCreateMethodSignResp_1008047>, IMessage, IEquatable<VerifyCreateMethodSignResp_1008047>, IDeepCloneable<VerifyCreateMethodSignResp_1008047>
{
	private static readonly MessageParser<VerifyCreateMethodSignResp_1008047> _parser = new MessageParser<VerifyCreateMethodSignResp_1008047>(() => new VerifyCreateMethodSignResp_1008047());

	private UnknownFieldSet _unknownFields;

	public const int CodeFieldNumber = 1;

	private int code_;

	public const int IsSuccessFieldNumber = 2;

	private bool isSuccess_;

	public const int CreatedIsSuccessFieldNumber = 3;

	private bool createdIsSuccess_;

	public const int MethodSignIdFieldNumber = 4;

	private string methodSignId_ = "";

	public const int CurrentCleverFieldNumber = 5;

	private int currentClever_;

	[DebuggerNonUserCode]
	public static MessageParser<VerifyCreateMethodSignResp_1008047> Parser => _parser;

	[DebuggerNonUserCode]
	public static MessageDescriptor Descriptor => UnrealWorldProtoReflection.Descriptor.MessageTypes[59];

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
	public bool CreatedIsSuccess
	{
		get
		{
			return createdIsSuccess_;
		}
		set
		{
			createdIsSuccess_ = value;
		}
	}

	[DebuggerNonUserCode]
	public string MethodSignId
	{
		get
		{
			return methodSignId_;
		}
		set
		{
			methodSignId_ = ProtoPreconditions.CheckNotNull(value, "value");
		}
	}

	[DebuggerNonUserCode]
	public int CurrentClever
	{
		get
		{
			return currentClever_;
		}
		set
		{
			currentClever_ = value;
		}
	}

	private MessageDescriptor pb_003A_003AGoogle_002EProtobuf_002EIMessage_002Eget_Descriptor()
	{
		return Descriptor;
	}

	[DebuggerNonUserCode]
	public VerifyCreateMethodSignResp_1008047()
	{
	}

	[DebuggerNonUserCode]
	public VerifyCreateMethodSignResp_1008047(VerifyCreateMethodSignResp_1008047 other)
		: this()
	{
		code_ = other.code_;
		isSuccess_ = other.isSuccess_;
		createdIsSuccess_ = other.createdIsSuccess_;
		methodSignId_ = other.methodSignId_;
		currentClever_ = other.currentClever_;
		_unknownFields = UnknownFieldSet.Clone(other._unknownFields);
	}

	[DebuggerNonUserCode]
	public VerifyCreateMethodSignResp_1008047 Clone()
	{
		return new VerifyCreateMethodSignResp_1008047(this);
	}

	[DebuggerNonUserCode]
	public override bool Equals(object other)
	{
		return Equals(other as VerifyCreateMethodSignResp_1008047);
	}

	[DebuggerNonUserCode]
	public bool Equals(VerifyCreateMethodSignResp_1008047 other)
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
		if (CreatedIsSuccess != other.CreatedIsSuccess)
		{
			return false;
		}
		if (MethodSignId != other.MethodSignId)
		{
			return false;
		}
		if (CurrentClever != other.CurrentClever)
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
		if (CreatedIsSuccess)
		{
			num ^= CreatedIsSuccess.GetHashCode();
		}
		if (MethodSignId.Length != 0)
		{
			num ^= MethodSignId.GetHashCode();
		}
		if (CurrentClever != 0)
		{
			num ^= CurrentClever.GetHashCode();
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
		if (CreatedIsSuccess)
		{
			output.WriteRawTag(24);
			output.WriteBool(CreatedIsSuccess);
		}
		if (MethodSignId.Length != 0)
		{
			output.WriteRawTag(34);
			output.WriteString(MethodSignId);
		}
		if (CurrentClever != 0)
		{
			output.WriteRawTag(40);
			output.WriteInt32(CurrentClever);
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
		if (CreatedIsSuccess)
		{
			num += 2;
		}
		if (MethodSignId.Length != 0)
		{
			num += 1 + CodedOutputStream.ComputeStringSize(MethodSignId);
		}
		if (CurrentClever != 0)
		{
			num += 1 + CodedOutputStream.ComputeInt32Size(CurrentClever);
		}
		if (_unknownFields != null)
		{
			num += _unknownFields.CalculateSize();
		}
		return num;
	}

	[DebuggerNonUserCode]
	public void MergeFrom(VerifyCreateMethodSignResp_1008047 other)
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
			if (other.CreatedIsSuccess)
			{
				CreatedIsSuccess = other.CreatedIsSuccess;
			}
			if (other.MethodSignId.Length != 0)
			{
				MethodSignId = other.MethodSignId;
			}
			if (other.CurrentClever != 0)
			{
				CurrentClever = other.CurrentClever;
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
				CreatedIsSuccess = input.ReadBool();
				break;
			case 34u:
				MethodSignId = input.ReadString();
				break;
			case 40u:
				CurrentClever = input.ReadInt32();
				break;
			}
		}
	}
}
