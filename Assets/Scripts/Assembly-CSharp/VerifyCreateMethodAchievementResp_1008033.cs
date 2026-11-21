using System;
using System.Diagnostics;
using Google.Protobuf;
using Google.Protobuf.Collections;
using Google.Protobuf.Reflection;

public sealed class VerifyCreateMethodAchievementResp_1008033 : IMessage<VerifyCreateMethodAchievementResp_1008033>, IMessage, IEquatable<VerifyCreateMethodAchievementResp_1008033>, IDeepCloneable<VerifyCreateMethodAchievementResp_1008033>
{
	private static readonly MessageParser<VerifyCreateMethodAchievementResp_1008033> _parser = new MessageParser<VerifyCreateMethodAchievementResp_1008033>(() => new VerifyCreateMethodAchievementResp_1008033());

	private UnknownFieldSet _unknownFields;

	public const int CodeFieldNumber = 1;

	private int code_;

	public const int IsSuccessFieldNumber = 2;

	private bool isSuccess_;

	public const int CreatedIsSuccessFieldNumber = 3;

	private bool createdIsSuccess_;

	public const int CurrentCleverFieldNumber = 4;

	private int currentClever_;

	public const int ActionScopeFieldNumber = 5;

	private int actionScope_;

	public const int MinLevelFieldNumber = 6;

	private int minLevel_;

	public const int MaxLevelFieldNumber = 7;

	private int maxLevel_;

	public const int ConversionRateFieldNumber = 8;

	private double conversionRate_;

	public const int MaxStepFieldNumber = 9;

	private int maxStep_;

	public const int RunSpeedFieldNumber = 10;

	private int runSpeed_;

	public const int CreatedByFieldNumber = 11;

	private string createdBy_ = "";

	public const int EffectType1FieldNumber = 12;

	private int effectType1_;

	public const int EffectValue1FieldNumber = 13;

	private int effectValue1_;

	public const int EffectType2FieldNumber = 14;

	private int effectType2_;

	public const int EffectValue2FieldNumber = 15;

	private int effectValue2_;

	public const int StageFieldNumber = 16;

	private int stage_;

	public const int RankFieldNumber = 17;

	private int rank_;

	public const int FiveElementFieldNumber = 18;

	private int fiveElement_;

	public const int LiveLevelFieldNumber = 19;

	private int liveLevel_;

	public const int LiveProficiencyFieldNumber = 20;

	private int liveProficiency_;

	[DebuggerNonUserCode]
	public static MessageParser<VerifyCreateMethodAchievementResp_1008033> Parser => _parser;

	[DebuggerNonUserCode]
	public static MessageDescriptor Descriptor => UnrealWorldProtoReflection.Descriptor.MessageTypes[40];

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
	public int ActionScope
	{
		get
		{
			return actionScope_;
		}
		set
		{
			actionScope_ = value;
		}
	}

	[DebuggerNonUserCode]
	public int MinLevel
	{
		get
		{
			return minLevel_;
		}
		set
		{
			minLevel_ = value;
		}
	}

	[DebuggerNonUserCode]
	public int MaxLevel
	{
		get
		{
			return maxLevel_;
		}
		set
		{
			maxLevel_ = value;
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
	public int MaxStep
	{
		get
		{
			return maxStep_;
		}
		set
		{
			maxStep_ = value;
		}
	}

	[DebuggerNonUserCode]
	public int RunSpeed
	{
		get
		{
			return runSpeed_;
		}
		set
		{
			runSpeed_ = value;
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
	public int EffectType1
	{
		get
		{
			return effectType1_;
		}
		set
		{
			effectType1_ = value;
		}
	}

	[DebuggerNonUserCode]
	public int EffectValue1
	{
		get
		{
			return effectValue1_;
		}
		set
		{
			effectValue1_ = value;
		}
	}

	[DebuggerNonUserCode]
	public int EffectType2
	{
		get
		{
			return effectType2_;
		}
		set
		{
			effectType2_ = value;
		}
	}

	[DebuggerNonUserCode]
	public int EffectValue2
	{
		get
		{
			return effectValue2_;
		}
		set
		{
			effectValue2_ = value;
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
	public VerifyCreateMethodAchievementResp_1008033()
	{
	}

	[DebuggerNonUserCode]
	public VerifyCreateMethodAchievementResp_1008033(VerifyCreateMethodAchievementResp_1008033 other)
		: this()
	{
		code_ = other.code_;
		isSuccess_ = other.isSuccess_;
		createdIsSuccess_ = other.createdIsSuccess_;
		currentClever_ = other.currentClever_;
		actionScope_ = other.actionScope_;
		minLevel_ = other.minLevel_;
		maxLevel_ = other.maxLevel_;
		conversionRate_ = other.conversionRate_;
		maxStep_ = other.maxStep_;
		runSpeed_ = other.runSpeed_;
		createdBy_ = other.createdBy_;
		effectType1_ = other.effectType1_;
		effectValue1_ = other.effectValue1_;
		effectType2_ = other.effectType2_;
		effectValue2_ = other.effectValue2_;
		stage_ = other.stage_;
		rank_ = other.rank_;
		fiveElement_ = other.fiveElement_;
		liveLevel_ = other.liveLevel_;
		liveProficiency_ = other.liveProficiency_;
		_unknownFields = UnknownFieldSet.Clone(other._unknownFields);
	}

	[DebuggerNonUserCode]
	public VerifyCreateMethodAchievementResp_1008033 Clone()
	{
		return new VerifyCreateMethodAchievementResp_1008033(this);
	}

	[DebuggerNonUserCode]
	public override bool Equals(object other)
	{
		return Equals(other as VerifyCreateMethodAchievementResp_1008033);
	}

	[DebuggerNonUserCode]
	public bool Equals(VerifyCreateMethodAchievementResp_1008033 other)
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
		if (ActionScope != other.ActionScope)
		{
			return false;
		}
		if (MinLevel != other.MinLevel)
		{
			return false;
		}
		if (MaxLevel != other.MaxLevel)
		{
			return false;
		}
		if (!ProtobufEqualityComparers.BitwiseDoubleEqualityComparer.Equals(ConversionRate, other.ConversionRate))
		{
			return false;
		}
		if (MaxStep != other.MaxStep)
		{
			return false;
		}
		if (RunSpeed != other.RunSpeed)
		{
			return false;
		}
		if (CreatedBy != other.CreatedBy)
		{
			return false;
		}
		if (EffectType1 != other.EffectType1)
		{
			return false;
		}
		if (EffectValue1 != other.EffectValue1)
		{
			return false;
		}
		if (EffectType2 != other.EffectType2)
		{
			return false;
		}
		if (EffectValue2 != other.EffectValue2)
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
		if (CreatedIsSuccess)
		{
			num ^= CreatedIsSuccess.GetHashCode();
		}
		if (CurrentClever != 0)
		{
			num ^= CurrentClever.GetHashCode();
		}
		if (ActionScope != 0)
		{
			num ^= ActionScope.GetHashCode();
		}
		if (MinLevel != 0)
		{
			num ^= MinLevel.GetHashCode();
		}
		if (MaxLevel != 0)
		{
			num ^= MaxLevel.GetHashCode();
		}
		if (ConversionRate != 0.0)
		{
			num ^= ProtobufEqualityComparers.BitwiseDoubleEqualityComparer.GetHashCode(ConversionRate);
		}
		if (MaxStep != 0)
		{
			num ^= MaxStep.GetHashCode();
		}
		if (RunSpeed != 0)
		{
			num ^= RunSpeed.GetHashCode();
		}
		if (CreatedBy.Length != 0)
		{
			num ^= CreatedBy.GetHashCode();
		}
		if (EffectType1 != 0)
		{
			num ^= EffectType1.GetHashCode();
		}
		if (EffectValue1 != 0)
		{
			num ^= EffectValue1.GetHashCode();
		}
		if (EffectType2 != 0)
		{
			num ^= EffectType2.GetHashCode();
		}
		if (EffectValue2 != 0)
		{
			num ^= EffectValue2.GetHashCode();
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
		if (ActionScope != 0)
		{
			output.WriteRawTag(40);
			output.WriteInt32(ActionScope);
		}
		if (MinLevel != 0)
		{
			output.WriteRawTag(48);
			output.WriteInt32(MinLevel);
		}
		if (MaxLevel != 0)
		{
			output.WriteRawTag(56);
			output.WriteInt32(MaxLevel);
		}
		if (ConversionRate != 0.0)
		{
			output.WriteRawTag(65);
			output.WriteDouble(ConversionRate);
		}
		if (MaxStep != 0)
		{
			output.WriteRawTag(72);
			output.WriteInt32(MaxStep);
		}
		if (RunSpeed != 0)
		{
			output.WriteRawTag(80);
			output.WriteInt32(RunSpeed);
		}
		if (CreatedBy.Length != 0)
		{
			output.WriteRawTag(90);
			output.WriteString(CreatedBy);
		}
		if (EffectType1 != 0)
		{
			output.WriteRawTag(96);
			output.WriteInt32(EffectType1);
		}
		if (EffectValue1 != 0)
		{
			output.WriteRawTag(104);
			output.WriteInt32(EffectValue1);
		}
		if (EffectType2 != 0)
		{
			output.WriteRawTag(112);
			output.WriteInt32(EffectType2);
		}
		if (EffectValue2 != 0)
		{
			output.WriteRawTag(120);
			output.WriteInt32(EffectValue2);
		}
		if (Stage != 0)
		{
			output.WriteRawTag(128, 1);
			output.WriteInt32(Stage);
		}
		if (Rank != 0)
		{
			output.WriteRawTag(136, 1);
			output.WriteInt32(Rank);
		}
		if (FiveElement != 0)
		{
			output.WriteRawTag(144, 1);
			output.WriteInt32(FiveElement);
		}
		if (LiveLevel != 0)
		{
			output.WriteRawTag(152, 1);
			output.WriteInt32(LiveLevel);
		}
		if (LiveProficiency != 0)
		{
			output.WriteRawTag(160, 1);
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
		if (CreatedIsSuccess)
		{
			num += 2;
		}
		if (CurrentClever != 0)
		{
			num += 1 + CodedOutputStream.ComputeInt32Size(CurrentClever);
		}
		if (ActionScope != 0)
		{
			num += 1 + CodedOutputStream.ComputeInt32Size(ActionScope);
		}
		if (MinLevel != 0)
		{
			num += 1 + CodedOutputStream.ComputeInt32Size(MinLevel);
		}
		if (MaxLevel != 0)
		{
			num += 1 + CodedOutputStream.ComputeInt32Size(MaxLevel);
		}
		if (ConversionRate != 0.0)
		{
			num += 9;
		}
		if (MaxStep != 0)
		{
			num += 1 + CodedOutputStream.ComputeInt32Size(MaxStep);
		}
		if (RunSpeed != 0)
		{
			num += 1 + CodedOutputStream.ComputeInt32Size(RunSpeed);
		}
		if (CreatedBy.Length != 0)
		{
			num += 1 + CodedOutputStream.ComputeStringSize(CreatedBy);
		}
		if (EffectType1 != 0)
		{
			num += 1 + CodedOutputStream.ComputeInt32Size(EffectType1);
		}
		if (EffectValue1 != 0)
		{
			num += 1 + CodedOutputStream.ComputeInt32Size(EffectValue1);
		}
		if (EffectType2 != 0)
		{
			num += 1 + CodedOutputStream.ComputeInt32Size(EffectType2);
		}
		if (EffectValue2 != 0)
		{
			num += 1 + CodedOutputStream.ComputeInt32Size(EffectValue2);
		}
		if (Stage != 0)
		{
			num += 2 + CodedOutputStream.ComputeInt32Size(Stage);
		}
		if (Rank != 0)
		{
			num += 2 + CodedOutputStream.ComputeInt32Size(Rank);
		}
		if (FiveElement != 0)
		{
			num += 2 + CodedOutputStream.ComputeInt32Size(FiveElement);
		}
		if (LiveLevel != 0)
		{
			num += 2 + CodedOutputStream.ComputeInt32Size(LiveLevel);
		}
		if (LiveProficiency != 0)
		{
			num += 2 + CodedOutputStream.ComputeInt32Size(LiveProficiency);
		}
		if (_unknownFields != null)
		{
			num += _unknownFields.CalculateSize();
		}
		return num;
	}

	[DebuggerNonUserCode]
	public void MergeFrom(VerifyCreateMethodAchievementResp_1008033 other)
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
			if (other.ActionScope != 0)
			{
				ActionScope = other.ActionScope;
			}
			if (other.MinLevel != 0)
			{
				MinLevel = other.MinLevel;
			}
			if (other.MaxLevel != 0)
			{
				MaxLevel = other.MaxLevel;
			}
			if (other.ConversionRate != 0.0)
			{
				ConversionRate = other.ConversionRate;
			}
			if (other.MaxStep != 0)
			{
				MaxStep = other.MaxStep;
			}
			if (other.RunSpeed != 0)
			{
				RunSpeed = other.RunSpeed;
			}
			if (other.CreatedBy.Length != 0)
			{
				CreatedBy = other.CreatedBy;
			}
			if (other.EffectType1 != 0)
			{
				EffectType1 = other.EffectType1;
			}
			if (other.EffectValue1 != 0)
			{
				EffectValue1 = other.EffectValue1;
			}
			if (other.EffectType2 != 0)
			{
				EffectType2 = other.EffectType2;
			}
			if (other.EffectValue2 != 0)
			{
				EffectValue2 = other.EffectValue2;
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
				CreatedIsSuccess = input.ReadBool();
				break;
			case 32u:
				CurrentClever = input.ReadInt32();
				break;
			case 40u:
				ActionScope = input.ReadInt32();
				break;
			case 48u:
				MinLevel = input.ReadInt32();
				break;
			case 56u:
				MaxLevel = input.ReadInt32();
				break;
			case 65u:
				ConversionRate = input.ReadDouble();
				break;
			case 72u:
				MaxStep = input.ReadInt32();
				break;
			case 80u:
				RunSpeed = input.ReadInt32();
				break;
			case 90u:
				CreatedBy = input.ReadString();
				break;
			case 96u:
				EffectType1 = input.ReadInt32();
				break;
			case 104u:
				EffectValue1 = input.ReadInt32();
				break;
			case 112u:
				EffectType2 = input.ReadInt32();
				break;
			case 120u:
				EffectValue2 = input.ReadInt32();
				break;
			case 128u:
				Stage = input.ReadInt32();
				break;
			case 136u:
				Rank = input.ReadInt32();
				break;
			case 144u:
				FiveElement = input.ReadInt32();
				break;
			case 152u:
				LiveLevel = input.ReadInt32();
				break;
			case 160u:
				LiveProficiency = input.ReadInt32();
				break;
			}
		}
	}
}
