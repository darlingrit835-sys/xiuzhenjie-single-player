using System;
using System.Diagnostics;
using Google.Protobuf;
using Google.Protobuf.Collections;
using Google.Protobuf.Reflection;

public sealed class GoodsArticleSetableDefenseEquipmentResp_1004019 : IMessage<GoodsArticleSetableDefenseEquipmentResp_1004019>, IMessage, IEquatable<GoodsArticleSetableDefenseEquipmentResp_1004019>, IDeepCloneable<GoodsArticleSetableDefenseEquipmentResp_1004019>
{
	private static readonly MessageParser<GoodsArticleSetableDefenseEquipmentResp_1004019> _parser = new MessageParser<GoodsArticleSetableDefenseEquipmentResp_1004019>(() => new GoodsArticleSetableDefenseEquipmentResp_1004019());

	private UnknownFieldSet _unknownFields;

	public const int CodeFieldNumber = 1;

	private int code_;

	public const int IsSuccessFieldNumber = 2;

	private bool isSuccess_;

	public const int MinSpiritualityValueFieldNumber = 3;

	private long minSpiritualityValue_;

	public const int MaxSpiritualityValueFieldNumber = 4;

	private long maxSpiritualityValue_;

	public const int ConversionRateFieldNumber = 5;

	private double conversionRate_;

	public const int UsedPerceptionFieldNumber = 6;

	private long usedPerception_;

	public const int ToughTenacityFieldNumber = 7;

	private int toughTenacity_;

	public const int EffectType1FieldNumber = 8;

	private int effectType1_;

	public const int EffectValue1FieldNumber = 9;

	private int effectValue1_;

	public const int EffectType2FieldNumber = 10;

	private int effectType2_;

	public const int EffectValue2FieldNumber = 11;

	private int effectValue2_;

	public const int EffectType3FieldNumber = 12;

	private int effectType3_;

	public const int EffectValue3FieldNumber = 13;

	private int effectValue3_;

	public const int CreatedByFieldNumber = 14;

	private string createdBy_ = "";

	public const int RechooseCountFieldNumber = 15;

	private int rechooseCount_;

	public const int VolumeFieldNumber = 16;

	private double volume_;

	[DebuggerNonUserCode]
	public static MessageParser<GoodsArticleSetableDefenseEquipmentResp_1004019> Parser => _parser;

	[DebuggerNonUserCode]
	public static MessageDescriptor Descriptor => GoodsProtoReflection.Descriptor.MessageTypes[33];

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
	public long UsedPerception
	{
		get
		{
			return usedPerception_;
		}
		set
		{
			usedPerception_ = value;
		}
	}

	[DebuggerNonUserCode]
	public int ToughTenacity
	{
		get
		{
			return toughTenacity_;
		}
		set
		{
			toughTenacity_ = value;
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
	public int RechooseCount
	{
		get
		{
			return rechooseCount_;
		}
		set
		{
			rechooseCount_ = value;
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
	public GoodsArticleSetableDefenseEquipmentResp_1004019()
	{
	}

	[DebuggerNonUserCode]
	public GoodsArticleSetableDefenseEquipmentResp_1004019(GoodsArticleSetableDefenseEquipmentResp_1004019 other)
		: this()
	{
		code_ = other.code_;
		isSuccess_ = other.isSuccess_;
		minSpiritualityValue_ = other.minSpiritualityValue_;
		maxSpiritualityValue_ = other.maxSpiritualityValue_;
		conversionRate_ = other.conversionRate_;
		usedPerception_ = other.usedPerception_;
		toughTenacity_ = other.toughTenacity_;
		effectType1_ = other.effectType1_;
		effectValue1_ = other.effectValue1_;
		effectType2_ = other.effectType2_;
		effectValue2_ = other.effectValue2_;
		effectType3_ = other.effectType3_;
		effectValue3_ = other.effectValue3_;
		createdBy_ = other.createdBy_;
		rechooseCount_ = other.rechooseCount_;
		volume_ = other.volume_;
		_unknownFields = UnknownFieldSet.Clone(other._unknownFields);
	}

	[DebuggerNonUserCode]
	public GoodsArticleSetableDefenseEquipmentResp_1004019 Clone()
	{
		return new GoodsArticleSetableDefenseEquipmentResp_1004019(this);
	}

	[DebuggerNonUserCode]
	public override bool Equals(object other)
	{
		return Equals(other as GoodsArticleSetableDefenseEquipmentResp_1004019);
	}

	[DebuggerNonUserCode]
	public bool Equals(GoodsArticleSetableDefenseEquipmentResp_1004019 other)
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
		if (MinSpiritualityValue != other.MinSpiritualityValue)
		{
			return false;
		}
		if (MaxSpiritualityValue != other.MaxSpiritualityValue)
		{
			return false;
		}
		if (!ProtobufEqualityComparers.BitwiseDoubleEqualityComparer.Equals(ConversionRate, other.ConversionRate))
		{
			return false;
		}
		if (UsedPerception != other.UsedPerception)
		{
			return false;
		}
		if (ToughTenacity != other.ToughTenacity)
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
		if (CreatedBy != other.CreatedBy)
		{
			return false;
		}
		if (RechooseCount != other.RechooseCount)
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
		if (MinSpiritualityValue != 0L)
		{
			num ^= MinSpiritualityValue.GetHashCode();
		}
		if (MaxSpiritualityValue != 0L)
		{
			num ^= MaxSpiritualityValue.GetHashCode();
		}
		if (ConversionRate != 0.0)
		{
			num ^= ProtobufEqualityComparers.BitwiseDoubleEqualityComparer.GetHashCode(ConversionRate);
		}
		if (UsedPerception != 0L)
		{
			num ^= UsedPerception.GetHashCode();
		}
		if (ToughTenacity != 0)
		{
			num ^= ToughTenacity.GetHashCode();
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
		if (CreatedBy.Length != 0)
		{
			num ^= CreatedBy.GetHashCode();
		}
		if (RechooseCount != 0)
		{
			num ^= RechooseCount.GetHashCode();
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
		if (MinSpiritualityValue != 0L)
		{
			output.WriteRawTag(24);
			output.WriteInt64(MinSpiritualityValue);
		}
		if (MaxSpiritualityValue != 0L)
		{
			output.WriteRawTag(32);
			output.WriteInt64(MaxSpiritualityValue);
		}
		if (ConversionRate != 0.0)
		{
			output.WriteRawTag(41);
			output.WriteDouble(ConversionRate);
		}
		if (UsedPerception != 0L)
		{
			output.WriteRawTag(48);
			output.WriteInt64(UsedPerception);
		}
		if (ToughTenacity != 0)
		{
			output.WriteRawTag(56);
			output.WriteInt32(ToughTenacity);
		}
		if (EffectType1 != 0)
		{
			output.WriteRawTag(64);
			output.WriteInt32(EffectType1);
		}
		if (EffectValue1 != 0)
		{
			output.WriteRawTag(72);
			output.WriteInt32(EffectValue1);
		}
		if (EffectType2 != 0)
		{
			output.WriteRawTag(80);
			output.WriteInt32(EffectType2);
		}
		if (EffectValue2 != 0)
		{
			output.WriteRawTag(88);
			output.WriteInt32(EffectValue2);
		}
		if (EffectType3 != 0)
		{
			output.WriteRawTag(96);
			output.WriteInt32(EffectType3);
		}
		if (EffectValue3 != 0)
		{
			output.WriteRawTag(104);
			output.WriteInt32(EffectValue3);
		}
		if (CreatedBy.Length != 0)
		{
			output.WriteRawTag(114);
			output.WriteString(CreatedBy);
		}
		if (RechooseCount != 0)
		{
			output.WriteRawTag(120);
			output.WriteInt32(RechooseCount);
		}
		if (Volume != 0.0)
		{
			output.WriteRawTag(129, 1);
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
		if (MinSpiritualityValue != 0L)
		{
			num += 1 + CodedOutputStream.ComputeInt64Size(MinSpiritualityValue);
		}
		if (MaxSpiritualityValue != 0L)
		{
			num += 1 + CodedOutputStream.ComputeInt64Size(MaxSpiritualityValue);
		}
		if (ConversionRate != 0.0)
		{
			num += 9;
		}
		if (UsedPerception != 0L)
		{
			num += 1 + CodedOutputStream.ComputeInt64Size(UsedPerception);
		}
		if (ToughTenacity != 0)
		{
			num += 1 + CodedOutputStream.ComputeInt32Size(ToughTenacity);
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
		if (CreatedBy.Length != 0)
		{
			num += 1 + CodedOutputStream.ComputeStringSize(CreatedBy);
		}
		if (RechooseCount != 0)
		{
			num += 1 + CodedOutputStream.ComputeInt32Size(RechooseCount);
		}
		if (Volume != 0.0)
		{
			num += 10;
		}
		if (_unknownFields != null)
		{
			num += _unknownFields.CalculateSize();
		}
		return num;
	}

	[DebuggerNonUserCode]
	public void MergeFrom(GoodsArticleSetableDefenseEquipmentResp_1004019 other)
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
			if (other.MinSpiritualityValue != 0L)
			{
				MinSpiritualityValue = other.MinSpiritualityValue;
			}
			if (other.MaxSpiritualityValue != 0L)
			{
				MaxSpiritualityValue = other.MaxSpiritualityValue;
			}
			if (other.ConversionRate != 0.0)
			{
				ConversionRate = other.ConversionRate;
			}
			if (other.UsedPerception != 0L)
			{
				UsedPerception = other.UsedPerception;
			}
			if (other.ToughTenacity != 0)
			{
				ToughTenacity = other.ToughTenacity;
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
			if (other.CreatedBy.Length != 0)
			{
				CreatedBy = other.CreatedBy;
			}
			if (other.RechooseCount != 0)
			{
				RechooseCount = other.RechooseCount;
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
				MinSpiritualityValue = input.ReadInt64();
				break;
			case 32u:
				MaxSpiritualityValue = input.ReadInt64();
				break;
			case 41u:
				ConversionRate = input.ReadDouble();
				break;
			case 48u:
				UsedPerception = input.ReadInt64();
				break;
			case 56u:
				ToughTenacity = input.ReadInt32();
				break;
			case 64u:
				EffectType1 = input.ReadInt32();
				break;
			case 72u:
				EffectValue1 = input.ReadInt32();
				break;
			case 80u:
				EffectType2 = input.ReadInt32();
				break;
			case 88u:
				EffectValue2 = input.ReadInt32();
				break;
			case 96u:
				EffectType3 = input.ReadInt32();
				break;
			case 104u:
				EffectValue3 = input.ReadInt32();
				break;
			case 114u:
				CreatedBy = input.ReadString();
				break;
			case 120u:
				RechooseCount = input.ReadInt32();
				break;
			case 129u:
				Volume = input.ReadDouble();
				break;
			}
		}
	}
}
