using System;
using System.Diagnostics;
using Google.Protobuf;
using Google.Protobuf.Collections;
using Google.Protobuf.Reflection;

public sealed class MethodSignDetailResp_1011008 : IMessage<MethodSignDetailResp_1011008>, IMessage, IEquatable<MethodSignDetailResp_1011008>, IDeepCloneable<MethodSignDetailResp_1011008>
{
	private static readonly MessageParser<MethodSignDetailResp_1011008> _parser = new MessageParser<MethodSignDetailResp_1011008>(() => new MethodSignDetailResp_1011008());

	private UnknownFieldSet _unknownFields;

	public const int CodeFieldNumber = 1;

	private int code_;

	public const int IsSuccessFieldNumber = 2;

	private bool isSuccess_;

	public const int SkillTypeFieldNumber = 3;

	private int skillType_;

	public const int SpiritualityValueFieldNumber = 4;

	private long spiritualityValue_;

	public const int ConversionRateFieldNumber = 5;

	private double conversionRate_;

	public const int ContinueTimeFieldNumber = 6;

	private int continueTime_;

	public const int CastTimeFieldNumber = 7;

	private int castTime_;

	public const int CreatedByFieldNumber = 8;

	private string createdBy_ = "";

	[DebuggerNonUserCode]
	public static MessageParser<MethodSignDetailResp_1011008> Parser => _parser;

	[DebuggerNonUserCode]
	public static MessageDescriptor Descriptor => TemplateProtoReflection.Descriptor.MessageTypes[15];

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
	public int SkillType
	{
		get
		{
			return skillType_;
		}
		set
		{
			skillType_ = value;
		}
	}

	[DebuggerNonUserCode]
	public long SpiritualityValue
	{
		get
		{
			return spiritualityValue_;
		}
		set
		{
			spiritualityValue_ = value;
		}
	}

	[DebuggerNonUserCode]
	public double ConversionRate
	{
		get
		{
			return conversionRate_;
		}
		set
		{
			conversionRate_ = value;
		}
	}

	[DebuggerNonUserCode]
	public int ContinueTime
	{
		get
		{
			return continueTime_;
		}
		set
		{
			continueTime_ = value;
		}
	}

	[DebuggerNonUserCode]
	public int CastTime
	{
		get
		{
			return castTime_;
		}
		set
		{
			castTime_ = value;
		}
	}

	[DebuggerNonUserCode]
	public string CreatedBy
	{
		get
		{
			return createdBy_;
		}
		set
		{
			createdBy_ = ProtoPreconditions.CheckNotNull(value, "value");
		}
	}

	private MessageDescriptor pb_003A_003AGoogle_002EProtobuf_002EIMessage_002Eget_Descriptor()
	{
		return Descriptor;
	}

	[DebuggerNonUserCode]
	public MethodSignDetailResp_1011008()
	{
	}

	[DebuggerNonUserCode]
	public MethodSignDetailResp_1011008(MethodSignDetailResp_1011008 other)
		: this()
	{
		code_ = other.code_;
		isSuccess_ = other.isSuccess_;
		skillType_ = other.skillType_;
		spiritualityValue_ = other.spiritualityValue_;
		conversionRate_ = other.conversionRate_;
		continueTime_ = other.continueTime_;
		castTime_ = other.castTime_;
		createdBy_ = other.createdBy_;
		_unknownFields = UnknownFieldSet.Clone(other._unknownFields);
	}

	[DebuggerNonUserCode]
	public MethodSignDetailResp_1011008 Clone()
	{
		return new MethodSignDetailResp_1011008(this);
	}

	[DebuggerNonUserCode]
	public override bool Equals(object other)
	{
		return Equals(other as MethodSignDetailResp_1011008);
	}

	[DebuggerNonUserCode]
	public bool Equals(MethodSignDetailResp_1011008 other)
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
		if (SkillType != other.SkillType)
		{
			return false;
		}
		if (SpiritualityValue != other.SpiritualityValue)
		{
			return false;
		}
		if (!ProtobufEqualityComparers.BitwiseDoubleEqualityComparer.Equals(ConversionRate, other.ConversionRate))
		{
			return false;
		}
		if (ContinueTime != other.ContinueTime)
		{
			return false;
		}
		if (CastTime != other.CastTime)
		{
			return false;
		}
		if (CreatedBy != other.CreatedBy)
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
		if (SkillType != 0)
		{
			num ^= SkillType.GetHashCode();
		}
		if (SpiritualityValue != 0L)
		{
			num ^= SpiritualityValue.GetHashCode();
		}
		if (ConversionRate != 0.0)
		{
			num ^= ProtobufEqualityComparers.BitwiseDoubleEqualityComparer.GetHashCode(ConversionRate);
		}
		if (ContinueTime != 0)
		{
			num ^= ContinueTime.GetHashCode();
		}
		if (CastTime != 0)
		{
			num ^= CastTime.GetHashCode();
		}
		if (CreatedBy.Length != 0)
		{
			num ^= CreatedBy.GetHashCode();
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
		if (SkillType != 0)
		{
			output.WriteRawTag(24);
			output.WriteInt32(SkillType);
		}
		if (SpiritualityValue != 0L)
		{
			output.WriteRawTag(32);
			output.WriteInt64(SpiritualityValue);
		}
		if (ConversionRate != 0.0)
		{
			output.WriteRawTag(41);
			output.WriteDouble(ConversionRate);
		}
		if (ContinueTime != 0)
		{
			output.WriteRawTag(48);
			output.WriteInt32(ContinueTime);
		}
		if (CastTime != 0)
		{
			output.WriteRawTag(56);
			output.WriteInt32(CastTime);
		}
		if (CreatedBy.Length != 0)
		{
			output.WriteRawTag(66);
			output.WriteString(CreatedBy);
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
		if (SkillType != 0)
		{
			num += 1 + CodedOutputStream.ComputeInt32Size(SkillType);
		}
		if (SpiritualityValue != 0L)
		{
			num += 1 + CodedOutputStream.ComputeInt64Size(SpiritualityValue);
		}
		if (ConversionRate != 0.0)
		{
			num += 9;
		}
		if (ContinueTime != 0)
		{
			num += 1 + CodedOutputStream.ComputeInt32Size(ContinueTime);
		}
		if (CastTime != 0)
		{
			num += 1 + CodedOutputStream.ComputeInt32Size(CastTime);
		}
		if (CreatedBy.Length != 0)
		{
			num += 1 + CodedOutputStream.ComputeStringSize(CreatedBy);
		}
		if (_unknownFields != null)
		{
			num += _unknownFields.CalculateSize();
		}
		return num;
	}

	[DebuggerNonUserCode]
	public void MergeFrom(MethodSignDetailResp_1011008 other)
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
			if (other.SkillType != 0)
			{
				SkillType = other.SkillType;
			}
			if (other.SpiritualityValue != 0L)
			{
				SpiritualityValue = other.SpiritualityValue;
			}
			if (other.ConversionRate != 0.0)
			{
				ConversionRate = other.ConversionRate;
			}
			if (other.ContinueTime != 0)
			{
				ContinueTime = other.ContinueTime;
			}
			if (other.CastTime != 0)
			{
				CastTime = other.CastTime;
			}
			if (other.CreatedBy.Length != 0)
			{
				CreatedBy = other.CreatedBy;
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
				SkillType = input.ReadInt32();
				break;
			case 32u:
				SpiritualityValue = input.ReadInt64();
				break;
			case 41u:
				ConversionRate = input.ReadDouble();
				break;
			case 48u:
				ContinueTime = input.ReadInt32();
				break;
			case 56u:
				CastTime = input.ReadInt32();
				break;
			case 66u:
				CreatedBy = input.ReadString();
				break;
			}
		}
	}
}
