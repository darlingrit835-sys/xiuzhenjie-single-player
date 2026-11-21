using System;
using System.Diagnostics;
using Google.Protobuf;
using Google.Protobuf.Reflection;

public sealed class BuyYuekaResp_1001004 : IMessage<BuyYuekaResp_1001004>, IMessage, IEquatable<BuyYuekaResp_1001004>, IDeepCloneable<BuyYuekaResp_1001004>
{
	private static readonly MessageParser<BuyYuekaResp_1001004> _parser = new MessageParser<BuyYuekaResp_1001004>(() => new BuyYuekaResp_1001004());

	private UnknownFieldSet _unknownFields;

	public const int CodeFieldNumber = 1;

	private int code_;

	public const int IsSuccessFieldNumber = 2;

	private bool isSuccess_;

	public const int CurrentJadeFieldNumber = 3;

	private int currentJade_;

	public const int RemainingDayFieldNumber = 4;

	private int remainingDay_;

	[DebuggerNonUserCode]
	public static MessageParser<BuyYuekaResp_1001004> Parser => _parser;

	[DebuggerNonUserCode]
	public static MessageDescriptor Descriptor => SimpleActionProtoReflection.Descriptor.MessageTypes[3];

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
	public int CurrentJade
	{
		get
		{
			return currentJade_;
		}
		set
		{
			currentJade_ = value;
		}
	}

	[DebuggerNonUserCode]
	public int RemainingDay
	{
		get
		{
			return remainingDay_;
		}
		set
		{
			remainingDay_ = value;
		}
	}

	private MessageDescriptor pb_003A_003AGoogle_002EProtobuf_002EIMessage_002Eget_Descriptor()
	{
		return Descriptor;
	}

	[DebuggerNonUserCode]
	public BuyYuekaResp_1001004()
	{
	}

	[DebuggerNonUserCode]
	public BuyYuekaResp_1001004(BuyYuekaResp_1001004 other)
		: this()
	{
		code_ = other.code_;
		isSuccess_ = other.isSuccess_;
		currentJade_ = other.currentJade_;
		remainingDay_ = other.remainingDay_;
		_unknownFields = UnknownFieldSet.Clone(other._unknownFields);
	}

	[DebuggerNonUserCode]
	public BuyYuekaResp_1001004 Clone()
	{
		return new BuyYuekaResp_1001004(this);
	}

	[DebuggerNonUserCode]
	public override bool Equals(object other)
	{
		return Equals(other as BuyYuekaResp_1001004);
	}

	[DebuggerNonUserCode]
	public bool Equals(BuyYuekaResp_1001004 other)
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
		if (CurrentJade != other.CurrentJade)
		{
			return false;
		}
		if (RemainingDay != other.RemainingDay)
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
		if (CurrentJade != 0)
		{
			num ^= CurrentJade.GetHashCode();
		}
		if (RemainingDay != 0)
		{
			num ^= RemainingDay.GetHashCode();
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
		if (CurrentJade != 0)
		{
			output.WriteRawTag(24);
			output.WriteInt32(CurrentJade);
		}
		if (RemainingDay != 0)
		{
			output.WriteRawTag(32);
			output.WriteInt32(RemainingDay);
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
		if (CurrentJade != 0)
		{
			num += 1 + CodedOutputStream.ComputeInt32Size(CurrentJade);
		}
		if (RemainingDay != 0)
		{
			num += 1 + CodedOutputStream.ComputeInt32Size(RemainingDay);
		}
		if (_unknownFields != null)
		{
			num += _unknownFields.CalculateSize();
		}
		return num;
	}

	[DebuggerNonUserCode]
	public void MergeFrom(BuyYuekaResp_1001004 other)
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
			if (other.CurrentJade != 0)
			{
				CurrentJade = other.CurrentJade;
			}
			if (other.RemainingDay != 0)
			{
				RemainingDay = other.RemainingDay;
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
				CurrentJade = input.ReadInt32();
				break;
			case 32u:
				RemainingDay = input.ReadInt32();
				break;
			}
		}
	}
}
