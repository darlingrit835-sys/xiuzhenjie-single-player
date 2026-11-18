using System;
using System.Diagnostics;
using Google.Protobuf;
using Google.Protobuf.Reflection;

public sealed class BuyYuekaCnyResp_1001016 : IMessage<BuyYuekaCnyResp_1001016>, IMessage, IEquatable<BuyYuekaCnyResp_1001016>, IDeepCloneable<BuyYuekaCnyResp_1001016>
{
	private static readonly MessageParser<BuyYuekaCnyResp_1001016> _parser = new MessageParser<BuyYuekaCnyResp_1001016>(() => new BuyYuekaCnyResp_1001016());

	private UnknownFieldSet _unknownFields;

	public const int CodeFieldNumber = 1;

	private int code_;

	public const int IsSuccessFieldNumber = 2;

	private bool isSuccess_;

	public const int CurrentJadeFieldNumber = 3;

	private int currentJade_;

	public const int RemainingDayFieldNumber = 4;

	private int remainingDay_;

	public const int LeiChongFieldNumber = 5;

	private int leiChong_;

	public const int VipLevelFieldNumber = 6;

	private int vipLevel_;

	[DebuggerNonUserCode]
	public static MessageParser<BuyYuekaCnyResp_1001016> Parser => _parser;

	[DebuggerNonUserCode]
	public static MessageDescriptor Descriptor => SimpleActionProtoReflection.Descriptor.MessageTypes[18];

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

	[DebuggerNonUserCode]
	public int LeiChong
	{
		get
		{
			return leiChong_;
		}
		set
		{
			leiChong_ = value;
		}
	}

	[DebuggerNonUserCode]
	public int VipLevel
	{
		get
		{
			return vipLevel_;
		}
		set
		{
			vipLevel_ = value;
		}
	}

	private MessageDescriptor pb_003A_003AGoogle_002EProtobuf_002EIMessage_002Eget_Descriptor()
	{
		return Descriptor;
	}

	[DebuggerNonUserCode]
	public BuyYuekaCnyResp_1001016()
	{
	}

	[DebuggerNonUserCode]
	public BuyYuekaCnyResp_1001016(BuyYuekaCnyResp_1001016 other)
		: this()
	{
		code_ = other.code_;
		isSuccess_ = other.isSuccess_;
		currentJade_ = other.currentJade_;
		remainingDay_ = other.remainingDay_;
		leiChong_ = other.leiChong_;
		vipLevel_ = other.vipLevel_;
		_unknownFields = UnknownFieldSet.Clone(other._unknownFields);
	}

	[DebuggerNonUserCode]
	public BuyYuekaCnyResp_1001016 Clone()
	{
		return new BuyYuekaCnyResp_1001016(this);
	}

	[DebuggerNonUserCode]
	public override bool Equals(object other)
	{
		return Equals(other as BuyYuekaCnyResp_1001016);
	}

	[DebuggerNonUserCode]
	public bool Equals(BuyYuekaCnyResp_1001016 other)
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
		if (LeiChong != other.LeiChong)
		{
			return false;
		}
		if (VipLevel != other.VipLevel)
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
		if (LeiChong != 0)
		{
			num ^= LeiChong.GetHashCode();
		}
		if (VipLevel != 0)
		{
			num ^= VipLevel.GetHashCode();
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
		if (LeiChong != 0)
		{
			output.WriteRawTag(40);
			output.WriteInt32(LeiChong);
		}
		if (VipLevel != 0)
		{
			output.WriteRawTag(48);
			output.WriteInt32(VipLevel);
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
		if (LeiChong != 0)
		{
			num += 1 + CodedOutputStream.ComputeInt32Size(LeiChong);
		}
		if (VipLevel != 0)
		{
			num += 1 + CodedOutputStream.ComputeInt32Size(VipLevel);
		}
		if (_unknownFields != null)
		{
			num += _unknownFields.CalculateSize();
		}
		return num;
	}

	[DebuggerNonUserCode]
	public void MergeFrom(BuyYuekaCnyResp_1001016 other)
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
			if (other.LeiChong != 0)
			{
				LeiChong = other.LeiChong;
			}
			if (other.VipLevel != 0)
			{
				VipLevel = other.VipLevel;
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
			case 40u:
				LeiChong = input.ReadInt32();
				break;
			case 48u:
				VipLevel = input.ReadInt32();
				break;
			}
		}
	}
}
