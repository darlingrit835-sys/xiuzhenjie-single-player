using System;
using System.Diagnostics;
using Google.Protobuf;
using Google.Protobuf.Collections;
using Google.Protobuf.Reflection;

public sealed class VerifyCreateMethodSkillResp_1008025 : IMessage<VerifyCreateMethodSkillResp_1008025>, IMessage, IEquatable<VerifyCreateMethodSkillResp_1008025>, IDeepCloneable<VerifyCreateMethodSkillResp_1008025>
{
	private static readonly MessageParser<VerifyCreateMethodSkillResp_1008025> _parser = new MessageParser<VerifyCreateMethodSkillResp_1008025>(() => new VerifyCreateMethodSkillResp_1008025());

	private UnknownFieldSet _unknownFields;

	public const int CodeFieldNumber = 1;

	private int code_;

	public const int IsSuccessFieldNumber = 2;

	private bool isSuccess_;

	public const int CreatedIsSuccessFieldNumber = 3;

	private bool createdIsSuccess_;

	public const int CurrentCleverFieldNumber = 4;

	private int currentClever_;

	public const int SkillTypeFieldNumber = 5;

	private int skillType_;

	public const int MinSpiritualityValueFieldNumber = 6;

	private long minSpiritualityValue_;

	public const int MaxSpiritualityValueFieldNumber = 7;

	private long maxSpiritualityValue_;

	public const int ContinueTimeFieldNumber = 8;

	private int continueTime_;

	public const int CastTimeFieldNumber = 9;

	private int castTime_;

	public const int ConversionRateFieldNumber = 10;

	private double conversionRate_;

	public const int StageFieldNumber = 11;

	private int stage_;

	public const int RankFieldNumber = 12;

	private int rank_;

	public const int FiveElementFieldNumber = 13;

	private int fiveElement_;

	public const int LiveLevelFieldNumber = 14;

	private int liveLevel_;

	public const int LiveProficiencyFieldNumber = 15;

	private int liveProficiency_;

	public const int CreatedByFieldNumber = 16;

	private int createdBy_;

	[DebuggerNonUserCode]
	public static MessageParser<VerifyCreateMethodSkillResp_1008025> Parser => _parser;

	[DebuggerNonUserCode]
	public static MessageDescriptor Descriptor => UnrealWorldProtoReflection.Descriptor.MessageTypes[29];

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
	public int Stage
	{
		get
		{
			return stage_;
		}
		set
		{
			stage_ = value;
		}
	}

	[DebuggerNonUserCode]
	public int Rank
	{
		get
		{
			return rank_;
		}
		set
		{
			rank_ = value;
		}
	}

	[DebuggerNonUserCode]
	public int FiveElement
	{
		get
		{
			return fiveElement_;
		}
		set
		{
			fiveElement_ = value;
		}
	}

	[DebuggerNonUserCode]
	public int LiveLevel
	{
		get
		{
			return liveLevel_;
		}
		set
		{
			liveLevel_ = value;
		}
	}

	[DebuggerNonUserCode]
	public int LiveProficiency
	{
		get
		{
			return liveProficiency_;
		}
		set
		{
			liveProficiency_ = value;
		}
	}

	[DebuggerNonUserCode]
	public int CreatedBy
	{
		get
		{
			return createdBy_;
		}
		set
		{
			createdBy_ = value;
		}
	}

	private MessageDescriptor pb_003A_003AGoogle_002EProtobuf_002EIMessage_002Eget_Descriptor()
	{
		return Descriptor;
	}

	[DebuggerNonUserCode]
	public VerifyCreateMethodSkillResp_1008025()
	{
	}

	[DebuggerNonUserCode]
	public VerifyCreateMethodSkillResp_1008025(VerifyCreateMethodSkillResp_1008025 other)
		: this()
	{
		code_ = other.code_;
		isSuccess_ = other.isSuccess_;
		createdIsSuccess_ = other.createdIsSuccess_;
		currentClever_ = other.currentClever_;
		skillType_ = other.skillType_;
		minSpiritualityValue_ = other.minSpiritualityValue_;
		maxSpiritualityValue_ = other.maxSpiritualityValue_;
		continueTime_ = other.continueTime_;
		castTime_ = other.castTime_;
		conversionRate_ = other.conversionRate_;
		stage_ = other.stage_;
		rank_ = other.rank_;
		fiveElement_ = other.fiveElement_;
		liveLevel_ = other.liveLevel_;
		liveProficiency_ = other.liveProficiency_;
		createdBy_ = other.createdBy_;
		_unknownFields = UnknownFieldSet.Clone(other._unknownFields);
	}

	[DebuggerNonUserCode]
	public VerifyCreateMethodSkillResp_1008025 Clone()
	{
		return new VerifyCreateMethodSkillResp_1008025(this);
	}

	[DebuggerNonUserCode]
	public override bool Equals(object other)
	{
		return Equals(other as VerifyCreateMethodSkillResp_1008025);
	}

	[DebuggerNonUserCode]
	public bool Equals(VerifyCreateMethodSkillResp_1008025 other)
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
		if (CurrentClever != other.CurrentClever)
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
		if (Stage != other.Stage)
		{
			return false;
		}
		if (Rank != other.Rank)
		{
			return false;
		}
		if (FiveElement != other.FiveElement)
		{
			return false;
		}
		if (LiveLevel != other.LiveLevel)
		{
			return false;
		}
		if (LiveProficiency != other.LiveProficiency)
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
		if (CreatedIsSuccess)
		{
			num ^= CreatedIsSuccess.GetHashCode();
		}
		if (CurrentClever != 0)
		{
			num ^= CurrentClever.GetHashCode();
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
		if (Stage != 0)
		{
			num ^= Stage.GetHashCode();
		}
		if (Rank != 0)
		{
			num ^= Rank.GetHashCode();
		}
		if (FiveElement != 0)
		{
			num ^= FiveElement.GetHashCode();
		}
		if (LiveLevel != 0)
		{
			num ^= LiveLevel.GetHashCode();
		}
		if (LiveProficiency != 0)
		{
			num ^= LiveProficiency.GetHashCode();
		}
		if (CreatedBy != 0)
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
		if (CreatedIsSuccess)
		{
			output.WriteRawTag(24);
			output.WriteBool(CreatedIsSuccess);
		}
		if (CurrentClever != 0)
		{
			output.WriteRawTag(32);
			output.WriteInt32(CurrentClever);
		}
		if (SkillType != 0)
		{
			output.WriteRawTag(40);
			output.WriteInt32(SkillType);
		}
		if (MinSpiritualityValue != 0L)
		{
			output.WriteRawTag(48);
			output.WriteInt64(MinSpiritualityValue);
		}
		if (MaxSpiritualityValue != 0L)
		{
			output.WriteRawTag(56);
			output.WriteInt64(MaxSpiritualityValue);
		}
		if (ContinueTime != 0)
		{
			output.WriteRawTag(64);
			output.WriteInt32(ContinueTime);
		}
		if (CastTime != 0)
		{
			output.WriteRawTag(72);
			output.WriteInt32(CastTime);
		}
		if (ConversionRate != 0.0)
		{
			output.WriteRawTag(81);
			output.WriteDouble(ConversionRate);
		}
		if (Stage != 0)
		{
			output.WriteRawTag(88);
			output.WriteInt32(Stage);
		}
		if (Rank != 0)
		{
			output.WriteRawTag(96);
			output.WriteInt32(Rank);
		}
		if (FiveElement != 0)
		{
			output.WriteRawTag(104);
			output.WriteInt32(FiveElement);
		}
		if (LiveLevel != 0)
		{
			output.WriteRawTag(112);
			output.WriteInt32(LiveLevel);
		}
		if (LiveProficiency != 0)
		{
			output.WriteRawTag(120);
			output.WriteInt32(LiveProficiency);
		}
		if (CreatedBy != 0)
		{
			output.WriteRawTag(128, 1);
			output.WriteInt32(CreatedBy);
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
		if (CurrentClever != 0)
		{
			num += 1 + CodedOutputStream.ComputeInt32Size(CurrentClever);
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
		if (Stage != 0)
		{
			num += 1 + CodedOutputStream.ComputeInt32Size(Stage);
		}
		if (Rank != 0)
		{
			num += 1 + CodedOutputStream.ComputeInt32Size(Rank);
		}
		if (FiveElement != 0)
		{
			num += 1 + CodedOutputStream.ComputeInt32Size(FiveElement);
		}
		if (LiveLevel != 0)
		{
			num += 1 + CodedOutputStream.ComputeInt32Size(LiveLevel);
		}
		if (LiveProficiency != 0)
		{
			num += 1 + CodedOutputStream.ComputeInt32Size(LiveProficiency);
		}
		if (CreatedBy != 0)
		{
			num += 2 + CodedOutputStream.ComputeInt32Size(CreatedBy);
		}
		if (_unknownFields != null)
		{
			num += _unknownFields.CalculateSize();
		}
		return num;
	}

	[DebuggerNonUserCode]
	public void MergeFrom(VerifyCreateMethodSkillResp_1008025 other)
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
			if (other.CurrentClever != 0)
			{
				CurrentClever = other.CurrentClever;
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
			if (other.Stage != 0)
			{
				Stage = other.Stage;
			}
			if (other.Rank != 0)
			{
				Rank = other.Rank;
			}
			if (other.FiveElement != 0)
			{
				FiveElement = other.FiveElement;
			}
			if (other.LiveLevel != 0)
			{
				LiveLevel = other.LiveLevel;
			}
			if (other.LiveProficiency != 0)
			{
				LiveProficiency = other.LiveProficiency;
			}
			if (other.CreatedBy != 0)
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
				CreatedIsSuccess = input.ReadBool();
				break;
			case 32u:
				CurrentClever = input.ReadInt32();
				break;
			case 40u:
				SkillType = input.ReadInt32();
				break;
			case 48u:
				MinSpiritualityValue = input.ReadInt64();
				break;
			case 56u:
				MaxSpiritualityValue = input.ReadInt64();
				break;
			case 64u:
				ContinueTime = input.ReadInt32();
				break;
			case 72u:
				CastTime = input.ReadInt32();
				break;
			case 81u:
				ConversionRate = input.ReadDouble();
				break;
			case 88u:
				Stage = input.ReadInt32();
				break;
			case 96u:
				Rank = input.ReadInt32();
				break;
			case 104u:
				FiveElement = input.ReadInt32();
				break;
			case 112u:
				LiveLevel = input.ReadInt32();
				break;
			case 120u:
				LiveProficiency = input.ReadInt32();
				break;
			case 128u:
				CreatedBy = input.ReadInt32();
				break;
			}
		}
	}
}
