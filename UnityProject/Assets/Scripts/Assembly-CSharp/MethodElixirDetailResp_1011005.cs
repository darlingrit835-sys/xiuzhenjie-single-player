using System;
using System.Diagnostics;
using Google.Protobuf;
using Google.Protobuf.Collections;
using Google.Protobuf.Reflection;

public sealed class MethodElixirDetailResp_1011005 : IMessage<MethodElixirDetailResp_1011005>, IMessage, IEquatable<MethodElixirDetailResp_1011005>, IDeepCloneable<MethodElixirDetailResp_1011005>
{
	private static readonly MessageParser<MethodElixirDetailResp_1011005> _parser = new MessageParser<MethodElixirDetailResp_1011005>(() => new MethodElixirDetailResp_1011005());

	private UnknownFieldSet _unknownFields;

	public const int CodeFieldNumber = 1;

	private int code_;

	public const int IsSuccessFieldNumber = 2;

	private bool isSuccess_;

	public const int ElixirLevelFieldNumber = 3;

	private int elixirLevel_;

	public const int EffectTypeFieldNumber = 4;

	private int effectType_;

	public const int EffectValueFieldNumber = 5;

	private long effectValue_;

	public const int SpiritualityValueFieldNumber = 6;

	private long spiritualityValue_;

	public const int RankFieldNumber = 7;

	private int rank_;

	public const int PoisonValueFieldNumber = 8;

	private int poisonValue_;

	public const int StepCountFieldNumber = 9;

	private int stepCount_;

	public const int MaterialNamesFieldNumber = 10;

	private static readonly FieldCodec<string> _repeated_materialNames_codec = FieldCodec.ForString(82u);

	private readonly RepeatedField<string> materialNames_ = new RepeatedField<string>();

	public const int CreatedByFieldNumber = 11;

	private string createdBy_ = "";

	public const int ConversionRateFieldNumber = 12;

	private double conversionRate_;

	[DebuggerNonUserCode]
	public static MessageParser<MethodElixirDetailResp_1011005> Parser => _parser;

	[DebuggerNonUserCode]
	public static MessageDescriptor Descriptor => TemplateProtoReflection.Descriptor.MessageTypes[9];

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
	public int ElixirLevel
	{
		get
		{
			return elixirLevel_;
		}
		set
		{
			elixirLevel_ = value;
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
	public long EffectValue
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
	public int StepCount
	{
		get
		{
			return stepCount_;
		}
		set
		{
			stepCount_ = value;
		}
	}

	[DebuggerNonUserCode]
	public RepeatedField<string> MaterialNames => materialNames_;

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

	private MessageDescriptor pb_003A_003AGoogle_002EProtobuf_002EIMessage_002Eget_Descriptor()
	{
		return Descriptor;
	}

	[DebuggerNonUserCode]
	public MethodElixirDetailResp_1011005()
	{
	}

	[DebuggerNonUserCode]
	public MethodElixirDetailResp_1011005(MethodElixirDetailResp_1011005 other)
		: this()
	{
		code_ = other.code_;
		isSuccess_ = other.isSuccess_;
		elixirLevel_ = other.elixirLevel_;
		effectType_ = other.effectType_;
		effectValue_ = other.effectValue_;
		spiritualityValue_ = other.spiritualityValue_;
		rank_ = other.rank_;
		poisonValue_ = other.poisonValue_;
		stepCount_ = other.stepCount_;
		materialNames_ = other.materialNames_.Clone();
		createdBy_ = other.createdBy_;
		conversionRate_ = other.conversionRate_;
		_unknownFields = UnknownFieldSet.Clone(other._unknownFields);
	}

	[DebuggerNonUserCode]
	public MethodElixirDetailResp_1011005 Clone()
	{
		return new MethodElixirDetailResp_1011005(this);
	}

	[DebuggerNonUserCode]
	public override bool Equals(object other)
	{
		return Equals(other as MethodElixirDetailResp_1011005);
	}

	[DebuggerNonUserCode]
	public bool Equals(MethodElixirDetailResp_1011005 other)
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
		if (ElixirLevel != other.ElixirLevel)
		{
			return false;
		}
		if (EffectType != other.EffectType)
		{
			return false;
		}
		if (EffectValue != other.EffectValue)
		{
			return false;
		}
		if (SpiritualityValue != other.SpiritualityValue)
		{
			return false;
		}
		if (Rank != other.Rank)
		{
			return false;
		}
		if (PoisonValue != other.PoisonValue)
		{
			return false;
		}
		if (StepCount != other.StepCount)
		{
			return false;
		}
		if (!materialNames_.Equals(other.materialNames_))
		{
			return false;
		}
		if (CreatedBy != other.CreatedBy)
		{
			return false;
		}
		if (!ProtobufEqualityComparers.BitwiseDoubleEqualityComparer.Equals(ConversionRate, other.ConversionRate))
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
		if (ElixirLevel != 0)
		{
			num ^= ElixirLevel.GetHashCode();
		}
		if (EffectType != 0)
		{
			num ^= EffectType.GetHashCode();
		}
		if (EffectValue != 0L)
		{
			num ^= EffectValue.GetHashCode();
		}
		if (SpiritualityValue != 0L)
		{
			num ^= SpiritualityValue.GetHashCode();
		}
		if (Rank != 0)
		{
			num ^= Rank.GetHashCode();
		}
		if (PoisonValue != 0)
		{
			num ^= PoisonValue.GetHashCode();
		}
		if (StepCount != 0)
		{
			num ^= StepCount.GetHashCode();
		}
		num ^= materialNames_.GetHashCode();
		if (CreatedBy.Length != 0)
		{
			num ^= CreatedBy.GetHashCode();
		}
		if (ConversionRate != 0.0)
		{
			num ^= ProtobufEqualityComparers.BitwiseDoubleEqualityComparer.GetHashCode(ConversionRate);
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
		if (ElixirLevel != 0)
		{
			output.WriteRawTag(24);
			output.WriteInt32(ElixirLevel);
		}
		if (EffectType != 0)
		{
			output.WriteRawTag(32);
			output.WriteInt32(EffectType);
		}
		if (EffectValue != 0L)
		{
			output.WriteRawTag(40);
			output.WriteInt64(EffectValue);
		}
		if (SpiritualityValue != 0L)
		{
			output.WriteRawTag(48);
			output.WriteInt64(SpiritualityValue);
		}
		if (Rank != 0)
		{
			output.WriteRawTag(56);
			output.WriteInt32(Rank);
		}
		if (PoisonValue != 0)
		{
			output.WriteRawTag(64);
			output.WriteInt32(PoisonValue);
		}
		if (StepCount != 0)
		{
			output.WriteRawTag(72);
			output.WriteInt32(StepCount);
		}
		materialNames_.WriteTo(output, _repeated_materialNames_codec);
		if (CreatedBy.Length != 0)
		{
			output.WriteRawTag(90);
			output.WriteString(CreatedBy);
		}
		if (ConversionRate != 0.0)
		{
			output.WriteRawTag(97);
			output.WriteDouble(ConversionRate);
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
		if (ElixirLevel != 0)
		{
			num += 1 + CodedOutputStream.ComputeInt32Size(ElixirLevel);
		}
		if (EffectType != 0)
		{
			num += 1 + CodedOutputStream.ComputeInt32Size(EffectType);
		}
		if (EffectValue != 0L)
		{
			num += 1 + CodedOutputStream.ComputeInt64Size(EffectValue);
		}
		if (SpiritualityValue != 0L)
		{
			num += 1 + CodedOutputStream.ComputeInt64Size(SpiritualityValue);
		}
		if (Rank != 0)
		{
			num += 1 + CodedOutputStream.ComputeInt32Size(Rank);
		}
		if (PoisonValue != 0)
		{
			num += 1 + CodedOutputStream.ComputeInt32Size(PoisonValue);
		}
		if (StepCount != 0)
		{
			num += 1 + CodedOutputStream.ComputeInt32Size(StepCount);
		}
		num += materialNames_.CalculateSize(_repeated_materialNames_codec);
		if (CreatedBy.Length != 0)
		{
			num += 1 + CodedOutputStream.ComputeStringSize(CreatedBy);
		}
		if (ConversionRate != 0.0)
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
	public void MergeFrom(MethodElixirDetailResp_1011005 other)
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
			if (other.ElixirLevel != 0)
			{
				ElixirLevel = other.ElixirLevel;
			}
			if (other.EffectType != 0)
			{
				EffectType = other.EffectType;
			}
			if (other.EffectValue != 0L)
			{
				EffectValue = other.EffectValue;
			}
			if (other.SpiritualityValue != 0L)
			{
				SpiritualityValue = other.SpiritualityValue;
			}
			if (other.Rank != 0)
			{
				Rank = other.Rank;
			}
			if (other.PoisonValue != 0)
			{
				PoisonValue = other.PoisonValue;
			}
			if (other.StepCount != 0)
			{
				StepCount = other.StepCount;
			}
			materialNames_.Add(other.materialNames_);
			if (other.CreatedBy.Length != 0)
			{
				CreatedBy = other.CreatedBy;
			}
			if (other.ConversionRate != 0.0)
			{
				ConversionRate = other.ConversionRate;
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
				ElixirLevel = input.ReadInt32();
				break;
			case 32u:
				EffectType = input.ReadInt32();
				break;
			case 40u:
				EffectValue = input.ReadInt64();
				break;
			case 48u:
				SpiritualityValue = input.ReadInt64();
				break;
			case 56u:
				Rank = input.ReadInt32();
				break;
			case 64u:
				PoisonValue = input.ReadInt32();
				break;
			case 72u:
				StepCount = input.ReadInt32();
				break;
			case 82u:
				materialNames_.AddEntriesFrom(input, _repeated_materialNames_codec);
				break;
			case 90u:
				CreatedBy = input.ReadString();
				break;
			case 97u:
				ConversionRate = input.ReadDouble();
				break;
			}
		}
	}
}
