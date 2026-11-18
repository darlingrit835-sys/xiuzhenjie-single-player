using System;
using System.Diagnostics;
using Google.Protobuf;
using Google.Protobuf.Reflection;

public sealed class BreakAcupointResp_1005018 : IMessage<BreakAcupointResp_1005018>, IMessage, IEquatable<BreakAcupointResp_1005018>, IDeepCloneable<BreakAcupointResp_1005018>
{
	private static readonly MessageParser<BreakAcupointResp_1005018> _parser = new MessageParser<BreakAcupointResp_1005018>(() => new BreakAcupointResp_1005018());

	private UnknownFieldSet _unknownFields;

	public const int CodeFieldNumber = 1;

	private int code_;

	public const int IsSuccessFieldNumber = 2;

	private bool isSuccess_;

	public const int BreakSuccessFieldNumber = 3;

	private bool breakSuccess_;

	public const int BrokenLevelFieldNumber = 4;

	private int brokenLevel_;

	public const int AcupointValueFieldNumber = 5;

	private int acupointValue_;

	[DebuggerNonUserCode]
	public static MessageParser<BreakAcupointResp_1005018> Parser => _parser;

	[DebuggerNonUserCode]
	public static MessageDescriptor Descriptor => HomeProtoReflection.Descriptor.MessageTypes[22];

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
	public bool BreakSuccess
	{
		get
		{
			return breakSuccess_;
		}
		set
		{
			breakSuccess_ = value;
		}
	}

	[DebuggerNonUserCode]
	public int BrokenLevel
	{
		get
		{
			return brokenLevel_;
		}
		set
		{
			brokenLevel_ = value;
		}
	}

	[DebuggerNonUserCode]
	public int AcupointValue
	{
		get
		{
			return acupointValue_;
		}
		set
		{
			acupointValue_ = value;
		}
	}

	private MessageDescriptor pb_003A_003AGoogle_002EProtobuf_002EIMessage_002Eget_Descriptor()
	{
		return Descriptor;
	}

	[DebuggerNonUserCode]
	public BreakAcupointResp_1005018()
	{
	}

	[DebuggerNonUserCode]
	public BreakAcupointResp_1005018(BreakAcupointResp_1005018 other)
		: this()
	{
		code_ = other.code_;
		isSuccess_ = other.isSuccess_;
		breakSuccess_ = other.breakSuccess_;
		brokenLevel_ = other.brokenLevel_;
		acupointValue_ = other.acupointValue_;
		_unknownFields = UnknownFieldSet.Clone(other._unknownFields);
	}

	[DebuggerNonUserCode]
	public BreakAcupointResp_1005018 Clone()
	{
		return new BreakAcupointResp_1005018(this);
	}

	[DebuggerNonUserCode]
	public override bool Equals(object other)
	{
		return Equals(other as BreakAcupointResp_1005018);
	}

	[DebuggerNonUserCode]
	public bool Equals(BreakAcupointResp_1005018 other)
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
		if (BreakSuccess != other.BreakSuccess)
		{
			return false;
		}
		if (BrokenLevel != other.BrokenLevel)
		{
			return false;
		}
		if (AcupointValue != other.AcupointValue)
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
		if (BreakSuccess)
		{
			num ^= BreakSuccess.GetHashCode();
		}
		if (BrokenLevel != 0)
		{
			num ^= BrokenLevel.GetHashCode();
		}
		if (AcupointValue != 0)
		{
			num ^= AcupointValue.GetHashCode();
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
		if (BreakSuccess)
		{
			output.WriteRawTag(24);
			output.WriteBool(BreakSuccess);
		}
		if (BrokenLevel != 0)
		{
			output.WriteRawTag(32);
			output.WriteInt32(BrokenLevel);
		}
		if (AcupointValue != 0)
		{
			output.WriteRawTag(40);
			output.WriteInt32(AcupointValue);
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
		if (BreakSuccess)
		{
			num += 2;
		}
		if (BrokenLevel != 0)
		{
			num += 1 + CodedOutputStream.ComputeInt32Size(BrokenLevel);
		}
		if (AcupointValue != 0)
		{
			num += 1 + CodedOutputStream.ComputeInt32Size(AcupointValue);
		}
		if (_unknownFields != null)
		{
			num += _unknownFields.CalculateSize();
		}
		return num;
	}

	[DebuggerNonUserCode]
	public void MergeFrom(BreakAcupointResp_1005018 other)
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
			if (other.BreakSuccess)
			{
				BreakSuccess = other.BreakSuccess;
			}
			if (other.BrokenLevel != 0)
			{
				BrokenLevel = other.BrokenLevel;
			}
			if (other.AcupointValue != 0)
			{
				AcupointValue = other.AcupointValue;
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
				BreakSuccess = input.ReadBool();
				break;
			case 32u:
				BrokenLevel = input.ReadInt32();
				break;
			case 40u:
				AcupointValue = input.ReadInt32();
				break;
			}
		}
	}
}
