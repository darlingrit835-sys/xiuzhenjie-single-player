using System;
using System.Diagnostics;
using Google.Protobuf;
using Google.Protobuf.Collections;
using Google.Protobuf.Reflection;

public sealed class CreateMethodElixirRefiningResultResp_1008020 : IMessage<CreateMethodElixirRefiningResultResp_1008020>, IMessage, IEquatable<CreateMethodElixirRefiningResultResp_1008020>, IDeepCloneable<CreateMethodElixirRefiningResultResp_1008020>
{
	[DebuggerNonUserCode]
	public static class Types
	{
		public sealed class MethodElixirStepMaterialResp_1008063 : IMessage<MethodElixirStepMaterialResp_1008063>, IMessage, IEquatable<MethodElixirStepMaterialResp_1008063>, IDeepCloneable<MethodElixirStepMaterialResp_1008063>
		{
			private static readonly MessageParser<MethodElixirStepMaterialResp_1008063> _parser = new MessageParser<MethodElixirStepMaterialResp_1008063>(() => new MethodElixirStepMaterialResp_1008063());

			private UnknownFieldSet _unknownFields;

			public const int MaterialNameFieldNumber = 1;

			private string materialName_ = "";

			[DebuggerNonUserCode]
			public static MessageParser<MethodElixirStepMaterialResp_1008063> Parser => _parser;

			[DebuggerNonUserCode]
			public static MessageDescriptor Descriptor => CreateMethodElixirRefiningResultResp_1008020.Descriptor.NestedTypes[0];

			[DebuggerNonUserCode]
			MessageDescriptor IMessage.Descriptor => pb_003A_003AGoogle_002EProtobuf_002EIMessage_002Eget_Descriptor();

			[DebuggerNonUserCode]
			public string MaterialName
			{
				get
				{
					return materialName_;
				}
				set
				{
					materialName_ = ProtoPreconditions.CheckNotNull(value, "value");
				}
			}

			private MessageDescriptor pb_003A_003AGoogle_002EProtobuf_002EIMessage_002Eget_Descriptor()
			{
				return Descriptor;
			}

			[DebuggerNonUserCode]
			public MethodElixirStepMaterialResp_1008063()
			{
			}

			[DebuggerNonUserCode]
			public MethodElixirStepMaterialResp_1008063(MethodElixirStepMaterialResp_1008063 other)
				: this()
			{
				materialName_ = other.materialName_;
				_unknownFields = UnknownFieldSet.Clone(other._unknownFields);
			}

			[DebuggerNonUserCode]
			public MethodElixirStepMaterialResp_1008063 Clone()
			{
				return new MethodElixirStepMaterialResp_1008063(this);
			}

			[DebuggerNonUserCode]
			public override bool Equals(object other)
			{
				return Equals(other as MethodElixirStepMaterialResp_1008063);
			}

			[DebuggerNonUserCode]
			public bool Equals(MethodElixirStepMaterialResp_1008063 other)
			{
				if (other == null)
				{
					return false;
				}
				if (other == this)
				{
					return true;
				}
				if (MaterialName != other.MaterialName)
				{
					return false;
				}
				return object.Equals(_unknownFields, other._unknownFields);
			}

			[DebuggerNonUserCode]
			public override int GetHashCode()
			{
				int num = 1;
				if (MaterialName.Length != 0)
				{
					num ^= MaterialName.GetHashCode();
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
				if (MaterialName.Length != 0)
				{
					output.WriteRawTag(10);
					output.WriteString(MaterialName);
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
				if (MaterialName.Length != 0)
				{
					num += 1 + CodedOutputStream.ComputeStringSize(MaterialName);
				}
				if (_unknownFields != null)
				{
					num += _unknownFields.CalculateSize();
				}
				return num;
			}

			[DebuggerNonUserCode]
			public void MergeFrom(MethodElixirStepMaterialResp_1008063 other)
			{
				if (other != null)
				{
					if (other.MaterialName.Length != 0)
					{
						MaterialName = other.MaterialName;
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
					if (num != 10)
					{
						_unknownFields = UnknownFieldSet.MergeFieldFrom(_unknownFields, input);
					}
					else
					{
						MaterialName = input.ReadString();
					}
				}
			}
		}
	}

	private static readonly MessageParser<CreateMethodElixirRefiningResultResp_1008020> _parser = new MessageParser<CreateMethodElixirRefiningResultResp_1008020>(() => new CreateMethodElixirRefiningResultResp_1008020());

	private UnknownFieldSet _unknownFields;

	public const int CodeFieldNumber = 1;

	private int code_;

	public const int IsSuccessFieldNumber = 2;

	private bool isSuccess_;

	public const int CurrentCleverFieldNumber = 3;

	private int currentClever_;

	public const int ElixirLevelFieldNumber = 4;

	private int elixirLevel_;

	public const int EffectTypeFieldNumber = 5;

	private int effectType_;

	public const int EffectValueFieldNumber = 6;

	private long effectValue_;

	public const int SpiritualityValueFieldNumber = 7;

	private long spiritualityValue_;

	public const int ConversionRateFieldNumber = 8;

	private double conversionRate_;

	public const int RankFieldNumber = 9;

	private int rank_;

	public const int PoisonValueFieldNumber = 10;

	private int poisonValue_;

	public const int MaterialsFieldNumber = 11;

	private static readonly FieldCodec<Types.MethodElixirStepMaterialResp_1008063> _repeated_materials_codec = FieldCodec.ForMessage(90u, Types.MethodElixirStepMaterialResp_1008063.Parser);

	private readonly RepeatedField<Types.MethodElixirStepMaterialResp_1008063> materials_ = new RepeatedField<Types.MethodElixirStepMaterialResp_1008063>();

	public const int CreatedByFieldNumber = 12;

	private string createdBy_ = "";

	public const int StageFieldNumber = 13;

	private int stage_;

	public const int FiveElementFieldNumber = 14;

	private int fiveElement_;

	public const int LiveLevelFieldNumber = 15;

	private int liveLevel_;

	public const int LiveProficiencyFieldNumber = 16;

	private int liveProficiency_;

	[DebuggerNonUserCode]
	public static MessageParser<CreateMethodElixirRefiningResultResp_1008020> Parser => _parser;

	[DebuggerNonUserCode]
	public static MessageDescriptor Descriptor => UnrealWorldProtoReflection.Descriptor.MessageTypes[22];

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
	public RepeatedField<Types.MethodElixirStepMaterialResp_1008063> Materials => materials_;

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
	public CreateMethodElixirRefiningResultResp_1008020()
	{
	}

	[DebuggerNonUserCode]
	public CreateMethodElixirRefiningResultResp_1008020(CreateMethodElixirRefiningResultResp_1008020 other)
		: this()
	{
		code_ = other.code_;
		isSuccess_ = other.isSuccess_;
		currentClever_ = other.currentClever_;
		elixirLevel_ = other.elixirLevel_;
		effectType_ = other.effectType_;
		effectValue_ = other.effectValue_;
		spiritualityValue_ = other.spiritualityValue_;
		conversionRate_ = other.conversionRate_;
		rank_ = other.rank_;
		poisonValue_ = other.poisonValue_;
		materials_ = other.materials_.Clone();
		createdBy_ = other.createdBy_;
		stage_ = other.stage_;
		fiveElement_ = other.fiveElement_;
		liveLevel_ = other.liveLevel_;
		liveProficiency_ = other.liveProficiency_;
		_unknownFields = UnknownFieldSet.Clone(other._unknownFields);
	}

	[DebuggerNonUserCode]
	public CreateMethodElixirRefiningResultResp_1008020 Clone()
	{
		return new CreateMethodElixirRefiningResultResp_1008020(this);
	}

	[DebuggerNonUserCode]
	public override bool Equals(object other)
	{
		return Equals(other as CreateMethodElixirRefiningResultResp_1008020);
	}

	[DebuggerNonUserCode]
	public bool Equals(CreateMethodElixirRefiningResultResp_1008020 other)
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
		if (!ProtobufEqualityComparers.BitwiseDoubleEqualityComparer.Equals(ConversionRate, other.ConversionRate))
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
		if (!materials_.Equals(other.materials_))
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
		if (ConversionRate != 0.0)
		{
			num ^= ProtobufEqualityComparers.BitwiseDoubleEqualityComparer.GetHashCode(ConversionRate);
		}
		if (Rank != 0)
		{
			num ^= Rank.GetHashCode();
		}
		if (PoisonValue != 0)
		{
			num ^= PoisonValue.GetHashCode();
		}
		num ^= materials_.GetHashCode();
		if (CreatedBy.Length != 0)
		{
			num ^= CreatedBy.GetHashCode();
		}
		if (Stage != 0)
		{
			num ^= Stage.GetHashCode();
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
		if (ElixirLevel != 0)
		{
			output.WriteRawTag(32);
			output.WriteInt32(ElixirLevel);
		}
		if (EffectType != 0)
		{
			output.WriteRawTag(40);
			output.WriteInt32(EffectType);
		}
		if (EffectValue != 0L)
		{
			output.WriteRawTag(48);
			output.WriteInt64(EffectValue);
		}
		if (SpiritualityValue != 0L)
		{
			output.WriteRawTag(56);
			output.WriteInt64(SpiritualityValue);
		}
		if (ConversionRate != 0.0)
		{
			output.WriteRawTag(65);
			output.WriteDouble(ConversionRate);
		}
		if (Rank != 0)
		{
			output.WriteRawTag(72);
			output.WriteInt32(Rank);
		}
		if (PoisonValue != 0)
		{
			output.WriteRawTag(80);
			output.WriteInt32(PoisonValue);
		}
		materials_.WriteTo(output, _repeated_materials_codec);
		if (CreatedBy.Length != 0)
		{
			output.WriteRawTag(98);
			output.WriteString(CreatedBy);
		}
		if (Stage != 0)
		{
			output.WriteRawTag(104);
			output.WriteInt32(Stage);
		}
		if (FiveElement != 0)
		{
			output.WriteRawTag(112);
			output.WriteInt32(FiveElement);
		}
		if (LiveLevel != 0)
		{
			output.WriteRawTag(120);
			output.WriteInt32(LiveLevel);
		}
		if (LiveProficiency != 0)
		{
			output.WriteRawTag(128, 1);
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
		if (ConversionRate != 0.0)
		{
			num += 9;
		}
		if (Rank != 0)
		{
			num += 1 + CodedOutputStream.ComputeInt32Size(Rank);
		}
		if (PoisonValue != 0)
		{
			num += 1 + CodedOutputStream.ComputeInt32Size(PoisonValue);
		}
		num += materials_.CalculateSize(_repeated_materials_codec);
		if (CreatedBy.Length != 0)
		{
			num += 1 + CodedOutputStream.ComputeStringSize(CreatedBy);
		}
		if (Stage != 0)
		{
			num += 1 + CodedOutputStream.ComputeInt32Size(Stage);
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
			num += 2 + CodedOutputStream.ComputeInt32Size(LiveProficiency);
		}
		if (_unknownFields != null)
		{
			num += _unknownFields.CalculateSize();
		}
		return num;
	}

	[DebuggerNonUserCode]
	public void MergeFrom(CreateMethodElixirRefiningResultResp_1008020 other)
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
			if (other.ConversionRate != 0.0)
			{
				ConversionRate = other.ConversionRate;
			}
			if (other.Rank != 0)
			{
				Rank = other.Rank;
			}
			if (other.PoisonValue != 0)
			{
				PoisonValue = other.PoisonValue;
			}
			materials_.Add(other.materials_);
			if (other.CreatedBy.Length != 0)
			{
				CreatedBy = other.CreatedBy;
			}
			if (other.Stage != 0)
			{
				Stage = other.Stage;
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
				ElixirLevel = input.ReadInt32();
				break;
			case 40u:
				EffectType = input.ReadInt32();
				break;
			case 48u:
				EffectValue = input.ReadInt64();
				break;
			case 56u:
				SpiritualityValue = input.ReadInt64();
				break;
			case 65u:
				ConversionRate = input.ReadDouble();
				break;
			case 72u:
				Rank = input.ReadInt32();
				break;
			case 80u:
				PoisonValue = input.ReadInt32();
				break;
			case 90u:
				materials_.AddEntriesFrom(input, _repeated_materials_codec);
				break;
			case 98u:
				CreatedBy = input.ReadString();
				break;
			case 104u:
				Stage = input.ReadInt32();
				break;
			case 112u:
				FiveElement = input.ReadInt32();
				break;
			case 120u:
				LiveLevel = input.ReadInt32();
				break;
			case 128u:
				LiveProficiency = input.ReadInt32();
				break;
			}
		}
	}
}
