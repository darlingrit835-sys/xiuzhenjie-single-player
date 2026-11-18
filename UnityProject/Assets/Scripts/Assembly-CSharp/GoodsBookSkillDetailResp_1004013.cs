using System;
using System.Diagnostics;
using Google.Protobuf;
using Google.Protobuf.Collections;
using Google.Protobuf.Reflection;

public sealed class GoodsBookSkillDetailResp_1004013 : IMessage<GoodsBookSkillDetailResp_1004013>, IMessage, IEquatable<GoodsBookSkillDetailResp_1004013>, IDeepCloneable<GoodsBookSkillDetailResp_1004013>
{
	private static readonly MessageParser<GoodsBookSkillDetailResp_1004013> _parser = new MessageParser<GoodsBookSkillDetailResp_1004013>(() => new GoodsBookSkillDetailResp_1004013());

	private UnknownFieldSet _unknownFields;

	public const int CodeFieldNumber = 1;

	private int code_;

	public const int IsSuccessFieldNumber = 2;

	private bool isSuccess_;

	public const int SkillTypeFieldNumber = 3;

	private int skillType_;

	public const int MinSpiritualityValueFieldNumber = 4;

	private long minSpiritualityValue_;

	public const int MaxSpiritualityValueFieldNumber = 5;

	private long maxSpiritualityValue_;

	public const int ContinueTimeFieldNumber = 6;

	private int continueTime_;

	public const int CastTimeFieldNumber = 7;

	private int castTime_;

	public const int ConversionRateFieldNumber = 8;

	private double conversionRate_;

	public const int CreatedByFieldNumber = 9;

	private string createdBy_ = "";

	public const int VolumeFieldNumber = 10;

	private double volume_;

	[DebuggerNonUserCode]
	public static MessageParser<GoodsBookSkillDetailResp_1004013> Parser => _parser;

	[DebuggerNonUserCode]
	public static MessageDescriptor Descriptor => GoodsProtoReflection.Descriptor.MessageTypes[21];

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
	public long MinSpiritualityValue
	{
		get
		{
			return minSpiritualityValue_;
		}
		set
		{
			minSpiritualityValue_ = value;
		}
	}

	[DebuggerNonUserCode]
	public long MaxSpiritualityValue
	{
		get
		{
			return maxSpiritualityValue_;
		}
		set
		{
			maxSpiritualityValue_ = value;
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

	[DebuggerNonUserCode]
	public double Volume
	{
		get
		{
			return volume_;
		}
		set
		{
			volume_ = value;
		}
	}

	private MessageDescriptor pb_003A_003AGoogle_002EProtobuf_002EIMessage_002Eget_Descriptor()
	{
		return Descriptor;
	}

	[DebuggerNonUserCode]
	public GoodsBookSkillDetailResp_1004013()
	{
	}

	[DebuggerNonUserCode]
	public GoodsBookSkillDetailResp_1004013(GoodsBookSkillDetailResp_1004013 other)
		: this()
	{
		code_ = other.code_;
		isSuccess_ = other.isSuccess_;
		skillType_ = other.skillType_;
		minSpiritualityValue_ = other.minSpiritualityValue_;
		maxSpiritualityValue_ = other.maxSpiritualityValue_;
		continueTime_ = other.continueTime_;
		castTime_ = other.castTime_;
		conversionRate_ = other.conversionRate_;
		createdBy_ = other.createdBy_;
		volume_ = other.volume_;
		_unknownFields = UnknownFieldSet.Clone(other._unknownFields);
	}

	[DebuggerNonUserCode]
	public GoodsBookSkillDetailResp_1004013 Clone()
	{
		return new GoodsBookSkillDetailResp_1004013(this);
	}

	[DebuggerNonUserCode]
	public override bool Equals(object other)
	{
		return Equals(other as GoodsBookSkillDetailResp_1004013);
	}

	[DebuggerNonUserCode]
	public bool Equals(GoodsBookSkillDetailResp_1004013 other)
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
		if (MinSpiritualityValue != other.MinSpiritualityValue)
		{
			return false;
		}
		if (MaxSpiritualityValue != other.MaxSpiritualityValue)
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
		if (!ProtobufEqualityComparers.BitwiseDoubleEqualityComparer.Equals(ConversionRate, other.ConversionRate))
		{
			return false;
		}
		if (CreatedBy != other.CreatedBy)
		{
			return false;
		}
		if (!ProtobufEqualityComparers.BitwiseDoubleEqualityComparer.Equals(Volume, other.Volume))
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
		if (MinSpiritualityValue != 0L)
		{
			num ^= MinSpiritualityValue.GetHashCode();
		}
		if (MaxSpiritualityValue != 0L)
		{
			num ^= MaxSpiritualityValue.GetHashCode();
		}
		if (ContinueTime != 0)
		{
			num ^= ContinueTime.GetHashCode();
		}
		if (CastTime != 0)
		{
			num ^= CastTime.GetHashCode();
		}
		if (ConversionRate != 0.0)
		{
			num ^= ProtobufEqualityComparers.BitwiseDoubleEqualityComparer.GetHashCode(ConversionRate);
		}
		if (CreatedBy.Length != 0)
		{
			num ^= CreatedBy.GetHashCode();
		}
		if (Volume != 0.0)
		{
			num ^= ProtobufEqualityComparers.BitwiseDoubleEqualityComparer.GetHashCode(Volume);
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
		if (MinSpiritualityValue != 0L)
		{
			output.WriteRawTag(32);
			output.WriteInt64(MinSpiritualityValue);
		}
		if (MaxSpiritualityValue != 0L)
		{
			output.WriteRawTag(40);
			output.WriteInt64(MaxSpiritualityValue);
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
		if (ConversionRate != 0.0)
		{
			output.WriteRawTag(65);
			output.WriteDouble(ConversionRate);
		}
		if (CreatedBy.Length != 0)
		{
			output.WriteRawTag(74);
			output.WriteString(CreatedBy);
		}
		if (Volume != 0.0)
		{
			output.WriteRawTag(81);
			output.WriteDouble(Volume);
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
		if (MinSpiritualityValue != 0L)
		{
			num += 1 + CodedOutputStream.ComputeInt64Size(MinSpiritualityValue);
		}
		if (MaxSpiritualityValue != 0L)
		{
			num += 1 + CodedOutputStream.ComputeInt64Size(MaxSpiritualityValue);
		}
		if (ContinueTime != 0)
		{
			num += 1 + CodedOutputStream.ComputeInt32Size(ContinueTime);
		}
		if (CastTime != 0)
		{
			num += 1 + CodedOutputStream.ComputeInt32Size(CastTime);
		}
		if (ConversionRate != 0.0)
		{
			num += 9;
		}
		if (CreatedBy.Length != 0)
		{
			num += 1 + CodedOutputStream.ComputeStringSize(CreatedBy);
		}
		if (Volume != 0.0)
		{
			num += 9;
		}
		if (_unknownFields != null)
		{
			num += _unknownFields.CalculateSize();
		}
		return num;
	}

	[DebuggerNonUserCode]
	public void MergeFrom(GoodsBookSkillDetailResp_1004013 other)
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
			if (other.MinSpiritualityValue != 0L)
			{
				MinSpiritualityValue = other.MinSpiritualityValue;
			}
			if (other.MaxSpiritualityValue != 0L)
			{
				MaxSpiritualityValue = other.MaxSpiritualityValue;
			}
			if (other.ContinueTime != 0)
			{
				ContinueTime = other.ContinueTime;
			}
			if (other.CastTime != 0)
			{
				CastTime = other.CastTime;
			}
			if (other.ConversionRate != 0.0)
			{
				ConversionRate = other.ConversionRate;
			}
			if (other.CreatedBy.Length != 0)
			{
				CreatedBy = other.CreatedBy;
			}
			if (other.Volume != 0.0)
			{
				Volume = other.Volume;
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
				MinSpiritualityValue = input.ReadInt64();
				break;
			case 40u:
				MaxSpiritualityValue = input.ReadInt64();
				break;
			case 48u:
				ContinueTime = input.ReadInt32();
				break;
			case 56u:
				CastTime = input.ReadInt32();
				break;
			case 65u:
				ConversionRate = input.ReadDouble();
				break;
			case 74u:
				CreatedBy = input.ReadString();
				break;
			case 81u:
				Volume = input.ReadDouble();
				break;
			}
		}
	}
}
