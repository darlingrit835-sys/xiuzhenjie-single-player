using System;
using System.Diagnostics;
using Google.Protobuf;
using Google.Protobuf.Collections;
using Google.Protobuf.Reflection;

public sealed class GoodsBookAchievementDetailResp_1004008 : IMessage<GoodsBookAchievementDetailResp_1004008>, IMessage, IEquatable<GoodsBookAchievementDetailResp_1004008>, IDeepCloneable<GoodsBookAchievementDetailResp_1004008>
{
	private static readonly MessageParser<GoodsBookAchievementDetailResp_1004008> _parser = new MessageParser<GoodsBookAchievementDetailResp_1004008>(() => new GoodsBookAchievementDetailResp_1004008());

	private UnknownFieldSet _unknownFields;

	public const int CodeFieldNumber = 1;

	private int code_;

	public const int IsSuccessFieldNumber = 2;

	private bool isSuccess_;

	public const int ActionScopeFieldNumber = 3;

	private int actionScope_;

	public const int LevelFieldNumber = 4;

	private int level_;

	public const int BasedConcentrationFieldNumber = 5;

	private double basedConcentration_;

	public const int ConversionRateFieldNumber = 6;

	private double conversionRate_;

	public const int RunSpeedFieldNumber = 7;

	private int runSpeed_;

	public const int CreatedByFieldNumber = 8;

	private string createdBy_ = "";

	public const int EffectType1FieldNumber = 9;

	private int effectType1_;

	public const int EffectValue1FieldNumber = 10;

	private int effectValue1_;

	public const int EffectType2FieldNumber = 11;

	private int effectType2_;

	public const int EffectValue2FieldNumber = 12;

	private int effectValue2_;

	public const int EffectType3FieldNumber = 13;

	private int effectType3_;

	public const int EffectValue3FieldNumber = 14;

	private int effectValue3_;

	public const int VolumeFieldNumber = 15;

	private double volume_;

	public const int MaxCountFieldNumber = 16;

	private int maxCount_;

	[DebuggerNonUserCode]
	public static MessageParser<GoodsBookAchievementDetailResp_1004008> Parser => _parser;

	[DebuggerNonUserCode]
	public static MessageDescriptor Descriptor => GoodsProtoReflection.Descriptor.MessageTypes[13];

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
	public int Level
	{
		get
		{
			return level_;
		}
		set
		{
			level_ = value;
		}
	}

	[DebuggerNonUserCode]
	public double BasedConcentration
	{
		get
		{
			return basedConcentration_;
		}
		set
		{
			basedConcentration_ = value;
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
	public int EffectType3
	{
		get
		{
			return effectType3_;
		}
		set
		{
			effectType3_ = value;
		}
	}

	[DebuggerNonUserCode]
	public int EffectValue3
	{
		get
		{
			return effectValue3_;
		}
		set
		{
			effectValue3_ = value;
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

	[DebuggerNonUserCode]
	public int MaxCount
	{
		get
		{
			return maxCount_;
		}
		set
		{
			maxCount_ = value;
		}
	}

	private MessageDescriptor pb_003A_003AGoogle_002EProtobuf_002EIMessage_002Eget_Descriptor()
	{
		return Descriptor;
	}

	[DebuggerNonUserCode]
	public GoodsBookAchievementDetailResp_1004008()
	{
	}

	[DebuggerNonUserCode]
	public GoodsBookAchievementDetailResp_1004008(GoodsBookAchievementDetailResp_1004008 other)
		: this()
	{
		code_ = other.code_;
		isSuccess_ = other.isSuccess_;
		actionScope_ = other.actionScope_;
		level_ = other.level_;
		basedConcentration_ = other.basedConcentration_;
		conversionRate_ = other.conversionRate_;
		runSpeed_ = other.runSpeed_;
		createdBy_ = other.createdBy_;
		effectType1_ = other.effectType1_;
		effectValue1_ = other.effectValue1_;
		effectType2_ = other.effectType2_;
		effectValue2_ = other.effectValue2_;
		effectType3_ = other.effectType3_;
		effectValue3_ = other.effectValue3_;
		volume_ = other.volume_;
		maxCount_ = other.maxCount_;
		_unknownFields = UnknownFieldSet.Clone(other._unknownFields);
	}

	[DebuggerNonUserCode]
	public GoodsBookAchievementDetailResp_1004008 Clone()
	{
		return new GoodsBookAchievementDetailResp_1004008(this);
	}

	[DebuggerNonUserCode]
	public override bool Equals(object other)
	{
		return Equals(other as GoodsBookAchievementDetailResp_1004008);
	}

	[DebuggerNonUserCode]
	public bool Equals(GoodsBookAchievementDetailResp_1004008 other)
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
		if (ActionScope != other.ActionScope)
		{
			return false;
		}
		if (Level != other.Level)
		{
			return false;
		}
		if (!ProtobufEqualityComparers.BitwiseDoubleEqualityComparer.Equals(BasedConcentration, other.BasedConcentration))
		{
			return false;
		}
		if (!ProtobufEqualityComparers.BitwiseDoubleEqualityComparer.Equals(ConversionRate, other.ConversionRate))
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
		if (EffectType3 != other.EffectType3)
		{
			return false;
		}
		if (EffectValue3 != other.EffectValue3)
		{
			return false;
		}
		if (!ProtobufEqualityComparers.BitwiseDoubleEqualityComparer.Equals(Volume, other.Volume))
		{
			return false;
		}
		if (MaxCount != other.MaxCount)
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
		if (ActionScope != 0)
		{
			num ^= ActionScope.GetHashCode();
		}
		if (Level != 0)
		{
			num ^= Level.GetHashCode();
		}
		if (BasedConcentration != 0.0)
		{
			num ^= ProtobufEqualityComparers.BitwiseDoubleEqualityComparer.GetHashCode(BasedConcentration);
		}
		if (ConversionRate != 0.0)
		{
			num ^= ProtobufEqualityComparers.BitwiseDoubleEqualityComparer.GetHashCode(ConversionRate);
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
		if (EffectType3 != 0)
		{
			num ^= EffectType3.GetHashCode();
		}
		if (EffectValue3 != 0)
		{
			num ^= EffectValue3.GetHashCode();
		}
		if (Volume != 0.0)
		{
			num ^= ProtobufEqualityComparers.BitwiseDoubleEqualityComparer.GetHashCode(Volume);
		}
		if (MaxCount != 0)
		{
			num ^= MaxCount.GetHashCode();
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
		if (ActionScope != 0)
		{
			output.WriteRawTag(24);
			output.WriteInt32(ActionScope);
		}
		if (Level != 0)
		{
			output.WriteRawTag(32);
			output.WriteInt32(Level);
		}
		if (BasedConcentration != 0.0)
		{
			output.WriteRawTag(41);
			output.WriteDouble(BasedConcentration);
		}
		if (ConversionRate != 0.0)
		{
			output.WriteRawTag(49);
			output.WriteDouble(ConversionRate);
		}
		if (RunSpeed != 0)
		{
			output.WriteRawTag(56);
			output.WriteInt32(RunSpeed);
		}
		if (CreatedBy.Length != 0)
		{
			output.WriteRawTag(66);
			output.WriteString(CreatedBy);
		}
		if (EffectType1 != 0)
		{
			output.WriteRawTag(72);
			output.WriteInt32(EffectType1);
		}
		if (EffectValue1 != 0)
		{
			output.WriteRawTag(80);
			output.WriteInt32(EffectValue1);
		}
		if (EffectType2 != 0)
		{
			output.WriteRawTag(88);
			output.WriteInt32(EffectType2);
		}
		if (EffectValue2 != 0)
		{
			output.WriteRawTag(96);
			output.WriteInt32(EffectValue2);
		}
		if (EffectType3 != 0)
		{
			output.WriteRawTag(104);
			output.WriteInt32(EffectType3);
		}
		if (EffectValue3 != 0)
		{
			output.WriteRawTag(112);
			output.WriteInt32(EffectValue3);
		}
		if (Volume != 0.0)
		{
			output.WriteRawTag(121);
			output.WriteDouble(Volume);
		}
		if (MaxCount != 0)
		{
			output.WriteRawTag(128, 1);
			output.WriteInt32(MaxCount);
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
		if (ActionScope != 0)
		{
			num += 1 + CodedOutputStream.ComputeInt32Size(ActionScope);
		}
		if (Level != 0)
		{
			num += 1 + CodedOutputStream.ComputeInt32Size(Level);
		}
		if (BasedConcentration != 0.0)
		{
			num += 9;
		}
		if (ConversionRate != 0.0)
		{
			num += 9;
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
		if (EffectType3 != 0)
		{
			num += 1 + CodedOutputStream.ComputeInt32Size(EffectType3);
		}
		if (EffectValue3 != 0)
		{
			num += 1 + CodedOutputStream.ComputeInt32Size(EffectValue3);
		}
		if (Volume != 0.0)
		{
			num += 9;
		}
		if (MaxCount != 0)
		{
			num += 2 + CodedOutputStream.ComputeInt32Size(MaxCount);
		}
		if (_unknownFields != null)
		{
			num += _unknownFields.CalculateSize();
		}
		return num;
	}

	[DebuggerNonUserCode]
	public void MergeFrom(GoodsBookAchievementDetailResp_1004008 other)
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
			if (other.ActionScope != 0)
			{
				ActionScope = other.ActionScope;
			}
			if (other.Level != 0)
			{
				Level = other.Level;
			}
			if (other.BasedConcentration != 0.0)
			{
				BasedConcentration = other.BasedConcentration;
			}
			if (other.ConversionRate != 0.0)
			{
				ConversionRate = other.ConversionRate;
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
			if (other.EffectType3 != 0)
			{
				EffectType3 = other.EffectType3;
			}
			if (other.EffectValue3 != 0)
			{
				EffectValue3 = other.EffectValue3;
			}
			if (other.Volume != 0.0)
			{
				Volume = other.Volume;
			}
			if (other.MaxCount != 0)
			{
				MaxCount = other.MaxCount;
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
				ActionScope = input.ReadInt32();
				break;
			case 32u:
				Level = input.ReadInt32();
				break;
			case 41u:
				BasedConcentration = input.ReadDouble();
				break;
			case 49u:
				ConversionRate = input.ReadDouble();
				break;
			case 56u:
				RunSpeed = input.ReadInt32();
				break;
			case 66u:
				CreatedBy = input.ReadString();
				break;
			case 72u:
				EffectType1 = input.ReadInt32();
				break;
			case 80u:
				EffectValue1 = input.ReadInt32();
				break;
			case 88u:
				EffectType2 = input.ReadInt32();
				break;
			case 96u:
				EffectValue2 = input.ReadInt32();
				break;
			case 104u:
				EffectType3 = input.ReadInt32();
				break;
			case 112u:
				EffectValue3 = input.ReadInt32();
				break;
			case 121u:
				Volume = input.ReadDouble();
				break;
			case 128u:
				MaxCount = input.ReadInt32();
				break;
			}
		}
	}
}
