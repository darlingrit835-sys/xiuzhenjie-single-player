using System;
using System.Diagnostics;
using Google.Protobuf;
using Google.Protobuf.Collections;
using Google.Protobuf.Reflection;

public sealed class CreateMethodSignRefiningResultResp_1008052 : IMessage<CreateMethodSignRefiningResultResp_1008052>, IMessage, IEquatable<CreateMethodSignRefiningResultResp_1008052>, IDeepCloneable<CreateMethodSignRefiningResultResp_1008052>
{
	private static readonly MessageParser<CreateMethodSignRefiningResultResp_1008052> _parser = new MessageParser<CreateMethodSignRefiningResultResp_1008052>(() => new CreateMethodSignRefiningResultResp_1008052());

	private UnknownFieldSet _unknownFields;

	public const int CodeFieldNumber = 1;

	private int code_;

	public const int IsSuccessFieldNumber = 2;

	private bool isSuccess_;

	public const int CurrentCleverFieldNumber = 3;

	private int currentClever_;

	public const int SkillTypeFieldNumber = 4;

	private int skillType_;

	public const int SpiritualityValueFieldNumber = 5;

	private long spiritualityValue_;

	public const int ConversionRateFieldNumber = 6;

	private double conversionRate_;

	public const int ContinueTimeFieldNumber = 7;

	private int continueTime_;

	public const int CastTimeFieldNumber = 8;

	private int castTime_;

	public const int CreatedByFieldNumber = 9;

	private string createdBy_ = "";

	public const int StageFieldNumber = 10;

	private int stage_;

	public const int RankFieldNumber = 11;

	private int rank_;

	public const int FiveElementFieldNumber = 12;

	private int fiveElement_;

	public const int LiveLevelFieldNumber = 13;

	private int liveLevel_;

	public const int LiveProficiencyFieldNumber = 14;

	private int liveProficiency_;

	[DebuggerNonUserCode]
	public static MessageParser<CreateMethodSignRefiningResultResp_1008052> Parser => _parser;

	[DebuggerNonUserCode]
	public static MessageDescriptor Descriptor => UnrealWorldProtoReflection.Descriptor.MessageTypes[65];

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

	private MessageDescriptor pb_003A_003AGoogle_002EProtobuf_002EIMessage_002Eget_Descriptor()
	{
		return Descriptor;
	}

	[DebuggerNonUserCode]
	public CreateMethodSignRefiningResultResp_1008052()
	{
	}

	[DebuggerNonUserCode]
	public CreateMethodSignRefiningResultResp_1008052(CreateMethodSignRefiningResultResp_1008052 other)
		: this()
	{
		code_ = other.code_;
		isSuccess_ = other.isSuccess_;
		currentClever_ = other.currentClever_;
		skillType_ = other.skillType_;
		spiritualityValue_ = other.spiritualityValue_;
		conversionRate_ = other.conversionRate_;
		continueTime_ = other.continueTime_;
		castTime_ = other.castTime_;
		createdBy_ = other.createdBy_;
		stage_ = other.stage_;
		rank_ = other.rank_;
		fiveElement_ = other.fiveElement_;
		liveLevel_ = other.liveLevel_;
		liveProficiency_ = other.liveProficiency_;
		_unknownFields = UnknownFieldSet.Clone(other._unknownFields);
	}

	[DebuggerNonUserCode]
	public CreateMethodSignRefiningResultResp_1008052 Clone()
	{
		return new CreateMethodSignRefiningResultResp_1008052(this);
	}

	[DebuggerNonUserCode]
	public override bool Equals(object other)
	{
		return Equals(other as CreateMethodSignRefiningResultResp_1008052);
	}

	[DebuggerNonUserCode]
	public bool Equals(CreateMethodSignRefiningResultResp_1008052 other)
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
		if (CurrentClever != other.CurrentClever)
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
		if (CurrentClever != 0)
		{
			num ^= CurrentClever.GetHashCode();
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
		if (CurrentClever != 0)
		{
			output.WriteRawTag(24);
			output.WriteInt32(CurrentClever);
		}
		if (SkillType != 0)
		{
			output.WriteRawTag(32);
			output.WriteInt32(SkillType);
		}
		if (SpiritualityValue != 0L)
		{
			output.WriteRawTag(40);
			output.WriteInt64(SpiritualityValue);
		}
		if (ConversionRate != 0.0)
		{
			output.WriteRawTag(49);
			output.WriteDouble(ConversionRate);
		}
		if (ContinueTime != 0)
		{
			output.WriteRawTag(56);
			output.WriteInt32(ContinueTime);
		}
		if (CastTime != 0)
		{
			output.WriteRawTag(64);
			output.WriteInt32(CastTime);
		}
		if (CreatedBy.Length != 0)
		{
			output.WriteRawTag(74);
			output.WriteString(CreatedBy);
		}
		if (Stage != 0)
		{
			output.WriteRawTag(80);
			output.WriteInt32(Stage);
		}
		if (Rank != 0)
		{
			output.WriteRawTag(88);
			output.WriteInt32(Rank);
		}
		if (FiveElement != 0)
		{
			output.WriteRawTag(96);
			output.WriteInt32(FiveElement);
		}
		if (LiveLevel != 0)
		{
			output.WriteRawTag(104);
			output.WriteInt32(LiveLevel);
		}
		if (LiveProficiency != 0)
		{
			output.WriteRawTag(112);
			output.WriteInt32(LiveProficiency);
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
		if (CurrentClever != 0)
		{
			num += 1 + CodedOutputStream.ComputeInt32Size(CurrentClever);
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
		if (_unknownFields != null)
		{
			num += _unknownFields.CalculateSize();
		}
		return num;
	}

	[DebuggerNonUserCode]
	public void MergeFrom(CreateMethodSignRefiningResultResp_1008052 other)
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
			if (other.CurrentClever != 0)
			{
				CurrentClever = other.CurrentClever;
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
				CurrentClever = input.ReadInt32();
				break;
			case 32u:
				SkillType = input.ReadInt32();
				break;
			case 40u:
				SpiritualityValue = input.ReadInt64();
				break;
			case 49u:
				ConversionRate = input.ReadDouble();
				break;
			case 56u:
				ContinueTime = input.ReadInt32();
				break;
			case 64u:
				CastTime = input.ReadInt32();
				break;
			case 74u:
				CreatedBy = input.ReadString();
				break;
			case 80u:
				Stage = input.ReadInt32();
				break;
			case 88u:
				Rank = input.ReadInt32();
				break;
			case 96u:
				FiveElement = input.ReadInt32();
				break;
			case 104u:
				LiveLevel = input.ReadInt32();
				break;
			case 112u:
				LiveProficiency = input.ReadInt32();
				break;
			}
		}
	}
}
