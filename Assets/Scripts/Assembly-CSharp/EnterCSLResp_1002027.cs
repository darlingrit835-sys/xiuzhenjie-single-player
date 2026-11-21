using System;
using System.Diagnostics;
using Google.Protobuf;
using Google.Protobuf.Collections;
using Google.Protobuf.Reflection;

public sealed class EnterCSLResp_1002027 : IMessage<EnterCSLResp_1002027>, IMessage, IEquatable<EnterCSLResp_1002027>, IDeepCloneable<EnterCSLResp_1002027>
{
	private static readonly MessageParser<EnterCSLResp_1002027> _parser = new MessageParser<EnterCSLResp_1002027>(() => new EnterCSLResp_1002027());

	private UnknownFieldSet _unknownFields;

	public const int CodeFieldNumber = 1;

	private int code_;

	public const int IsSuccessFieldNumber = 2;

	private bool isSuccess_;

	public const int IsBuyCSLFieldNumber = 3;

	private bool isBuyCSL_;

	public const int GetLevelsFieldNumber = 4;

	private static readonly FieldCodec<int> _repeated_getLevels_codec = FieldCodec.ForInt32(34u);

	private readonly RepeatedField<int> getLevels_ = new RepeatedField<int>();

	[DebuggerNonUserCode]
	public static MessageParser<EnterCSLResp_1002027> Parser => _parser;

	[DebuggerNonUserCode]
	public static MessageDescriptor Descriptor => RoleProtoReflection.Descriptor.MessageTypes[30];

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
	public bool IsBuyCSL
	{
		get
		{
			return isBuyCSL_;
		}
		set
		{
			isBuyCSL_ = value;
		}
	}

	[DebuggerNonUserCode]
	public RepeatedField<int> GetLevels => getLevels_;

	private MessageDescriptor pb_003A_003AGoogle_002EProtobuf_002EIMessage_002Eget_Descriptor()
	{
		return Descriptor;
	}

	[DebuggerNonUserCode]
	public EnterCSLResp_1002027()
	{
	}

	[DebuggerNonUserCode]
	public EnterCSLResp_1002027(EnterCSLResp_1002027 other)
		: this()
	{
		code_ = other.code_;
		isSuccess_ = other.isSuccess_;
		isBuyCSL_ = other.isBuyCSL_;
		getLevels_ = other.getLevels_.Clone();
		_unknownFields = UnknownFieldSet.Clone(other._unknownFields);
	}

	[DebuggerNonUserCode]
	public EnterCSLResp_1002027 Clone()
	{
		return new EnterCSLResp_1002027(this);
	}

	[DebuggerNonUserCode]
	public override bool Equals(object other)
	{
		return Equals(other as EnterCSLResp_1002027);
	}

	[DebuggerNonUserCode]
	public bool Equals(EnterCSLResp_1002027 other)
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
		if (IsBuyCSL != other.IsBuyCSL)
		{
			return false;
		}
		if (!getLevels_.Equals(other.getLevels_))
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
		if (IsBuyCSL)
		{
			num ^= IsBuyCSL.GetHashCode();
		}
		num ^= getLevels_.GetHashCode();
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
		if (IsBuyCSL)
		{
			output.WriteRawTag(24);
			output.WriteBool(IsBuyCSL);
		}
		getLevels_.WriteTo(output, _repeated_getLevels_codec);
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
		if (IsBuyCSL)
		{
			num += 2;
		}
		num += getLevels_.CalculateSize(_repeated_getLevels_codec);
		if (_unknownFields != null)
		{
			num += _unknownFields.CalculateSize();
		}
		return num;
	}

	[DebuggerNonUserCode]
	public void MergeFrom(EnterCSLResp_1002027 other)
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
			if (other.IsBuyCSL)
			{
				IsBuyCSL = other.IsBuyCSL;
			}
			getLevels_.Add(other.getLevels_);
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
				IsBuyCSL = input.ReadBool();
				break;
			case 32u:
			case 34u:
				getLevels_.AddEntriesFrom(input, _repeated_getLevels_codec);
				break;
			}
		}
	}
}
