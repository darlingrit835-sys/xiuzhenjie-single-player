using System;
using System.Diagnostics;
using Google.Protobuf;
using Google.Protobuf.Collections;
using Google.Protobuf.Reflection;

public sealed class GoodsArticleElixirResp_1004015 : IMessage<GoodsArticleElixirResp_1004015>, IMessage, IEquatable<GoodsArticleElixirResp_1004015>, IDeepCloneable<GoodsArticleElixirResp_1004015>
{
	private static readonly MessageParser<GoodsArticleElixirResp_1004015> _parser = new MessageParser<GoodsArticleElixirResp_1004015>(() => new GoodsArticleElixirResp_1004015());

	private UnknownFieldSet _unknownFields;

	public const int CodeFieldNumber = 1;

	private int code_;

	public const int IsSuccessFieldNumber = 2;

	private bool isSuccess_;

	public const int EffectTypeFieldNumber = 3;

	private int effectType_;

	public const int SpiritualityValueFieldNumber = 4;

	private long spiritualityValue_;

	public const int ConversionRateFieldNumber = 5;

	private double conversionRate_;

	public const int PoisonValueFieldNumber = 6;

	private int poisonValue_;

	public const int CreatedByFieldNumber = 7;

	private string createdBy_ = "";

	public const int EffectValueFieldNumber = 8;

	private double effectValue_;

	public const int RealityEffectValueFieldNumber = 9;

	private double realityEffectValue_;

	public const int EatTimeFieldNumber = 10;

	private int eatTime_;

	public const int VolumeFieldNumber = 11;

	private double volume_;

	[DebuggerNonUserCode]
	public static MessageParser<GoodsArticleElixirResp_1004015> Parser => _parser;

	[DebuggerNonUserCode]
	public static MessageDescriptor Descriptor => GoodsProtoReflection.Descriptor.MessageTypes[25];

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
	public int EffectType
	{
		get
		{
			return effectType_;
		}
		set
		{
			effectType_ = value;
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
	public int PoisonValue
	{
		get
		{
			return poisonValue_;
		}
		set
		{
			poisonValue_ = value;
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
	public double EffectValue
	{
		get
		{
			return effectValue_;
		}
		set
		{
			effectValue_ = value;
		}
	}

	[DebuggerNonUserCode]
	public double RealityEffectValue
	{
		get
		{
			return realityEffectValue_;
		}
		set
		{
			realityEffectValue_ = value;
		}
	}

	[DebuggerNonUserCode]
	public int EatTime
	{
		get
		{
			return eatTime_;
		}
		set
		{
			eatTime_ = value;
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
	public GoodsArticleElixirResp_1004015()
	{
	}

	[DebuggerNonUserCode]
	public GoodsArticleElixirResp_1004015(GoodsArticleElixirResp_1004015 other)
		: this()
	{
		code_ = other.code_;
		isSuccess_ = other.isSuccess_;
		effectType_ = other.effectType_;
		spiritualityValue_ = other.spiritualityValue_;
		conversionRate_ = other.conversionRate_;
		poisonValue_ = other.poisonValue_;
		createdBy_ = other.createdBy_;
		effectValue_ = other.effectValue_;
		realityEffectValue_ = other.realityEffectValue_;
		eatTime_ = other.eatTime_;
		volume_ = other.volume_;
		_unknownFields = UnknownFieldSet.Clone(other._unknownFields);
	}

	[DebuggerNonUserCode]
	public GoodsArticleElixirResp_1004015 Clone()
	{
		return new GoodsArticleElixirResp_1004015(this);
	}

	[DebuggerNonUserCode]
	public override bool Equals(object other)
	{
		return Equals(other as GoodsArticleElixirResp_1004015);
	}

	[DebuggerNonUserCode]
	public bool Equals(GoodsArticleElixirResp_1004015 other)
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
		if (EffectType != other.EffectType)
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
		if (PoisonValue != other.PoisonValue)
		{
			return false;
		}
		if (CreatedBy != other.CreatedBy)
		{
			return false;
		}
		if (!ProtobufEqualityComparers.BitwiseDoubleEqualityComparer.Equals(EffectValue, other.EffectValue))
		{
			return false;
		}
		if (!ProtobufEqualityComparers.BitwiseDoubleEqualityComparer.Equals(RealityEffectValue, other.RealityEffectValue))
		{
			return false;
		}
		if (EatTime != other.EatTime)
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
		if (EffectType != 0)
		{
			num ^= EffectType.GetHashCode();
		}
		if (SpiritualityValue != 0L)
		{
			num ^= SpiritualityValue.GetHashCode();
		}
		if (ConversionRate != 0.0)
		{
			num ^= ProtobufEqualityComparers.BitwiseDoubleEqualityComparer.GetHashCode(ConversionRate);
		}
		if (PoisonValue != 0)
		{
			num ^= PoisonValue.GetHashCode();
		}
		if (CreatedBy.Length != 0)
		{
			num ^= CreatedBy.GetHashCode();
		}
		if (EffectValue != 0.0)
		{
			num ^= ProtobufEqualityComparers.BitwiseDoubleEqualityComparer.GetHashCode(EffectValue);
		}
		if (RealityEffectValue != 0.0)
		{
			num ^= ProtobufEqualityComparers.BitwiseDoubleEqualityComparer.GetHashCode(RealityEffectValue);
		}
		if (EatTime != 0)
		{
			num ^= EatTime.GetHashCode();
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
		if (EffectType != 0)
		{
			output.WriteRawTag(24);
			output.WriteInt32(EffectType);
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
		if (PoisonValue != 0)
		{
			output.WriteRawTag(48);
			output.WriteInt32(PoisonValue);
		}
		if (CreatedBy.Length != 0)
		{
			output.WriteRawTag(58);
			output.WriteString(CreatedBy);
		}
		if (EffectValue != 0.0)
		{
			output.WriteRawTag(65);
			output.WriteDouble(EffectValue);
		}
		if (RealityEffectValue != 0.0)
		{
			output.WriteRawTag(73);
			output.WriteDouble(RealityEffectValue);
		}
		if (EatTime != 0)
		{
			output.WriteRawTag(80);
			output.WriteInt32(EatTime);
		}
		if (Volume != 0.0)
		{
			output.WriteRawTag(89);
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
		if (EffectType != 0)
		{
			num += 1 + CodedOutputStream.ComputeInt32Size(EffectType);
		}
		if (SpiritualityValue != 0L)
		{
			num += 1 + CodedOutputStream.ComputeInt64Size(SpiritualityValue);
		}
		if (ConversionRate != 0.0)
		{
			num += 9;
		}
		if (PoisonValue != 0)
		{
			num += 1 + CodedOutputStream.ComputeInt32Size(PoisonValue);
		}
		if (CreatedBy.Length != 0)
		{
			num += 1 + CodedOutputStream.ComputeStringSize(CreatedBy);
		}
		if (EffectValue != 0.0)
		{
			num += 9;
		}
		if (RealityEffectValue != 0.0)
		{
			num += 9;
		}
		if (EatTime != 0)
		{
			num += 1 + CodedOutputStream.ComputeInt32Size(EatTime);
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
	public void MergeFrom(GoodsArticleElixirResp_1004015 other)
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
			if (other.EffectType != 0)
			{
				EffectType = other.EffectType;
			}
			if (other.SpiritualityValue != 0L)
			{
				SpiritualityValue = other.SpiritualityValue;
			}
			if (other.ConversionRate != 0.0)
			{
				ConversionRate = other.ConversionRate;
			}
			if (other.PoisonValue != 0)
			{
				PoisonValue = other.PoisonValue;
			}
			if (other.CreatedBy.Length != 0)
			{
				CreatedBy = other.CreatedBy;
			}
			if (other.EffectValue != 0.0)
			{
				EffectValue = other.EffectValue;
			}
			if (other.RealityEffectValue != 0.0)
			{
				RealityEffectValue = other.RealityEffectValue;
			}
			if (other.EatTime != 0)
			{
				EatTime = other.EatTime;
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
				EffectType = input.ReadInt32();
				break;
			case 32u:
				SpiritualityValue = input.ReadInt64();
				break;
			case 41u:
				ConversionRate = input.ReadDouble();
				break;
			case 48u:
				PoisonValue = input.ReadInt32();
				break;
			case 58u:
				CreatedBy = input.ReadString();
				break;
			case 65u:
				EffectValue = input.ReadDouble();
				break;
			case 73u:
				RealityEffectValue = input.ReadDouble();
				break;
			case 80u:
				EatTime = input.ReadInt32();
				break;
			case 89u:
				Volume = input.ReadDouble();
				break;
			}
		}
	}
}
