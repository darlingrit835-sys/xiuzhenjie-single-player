using System;
using System.Diagnostics;
using Google.Protobuf;
using Google.Protobuf.Collections;
using Google.Protobuf.Reflection;

public sealed class GoodsBookEquipmentDetailResp_1004011 : IMessage<GoodsBookEquipmentDetailResp_1004011>, IMessage, IEquatable<GoodsBookEquipmentDetailResp_1004011>, IDeepCloneable<GoodsBookEquipmentDetailResp_1004011>
{
	private static readonly MessageParser<GoodsBookEquipmentDetailResp_1004011> _parser = new MessageParser<GoodsBookEquipmentDetailResp_1004011>(() => new GoodsBookEquipmentDetailResp_1004011());

	private UnknownFieldSet _unknownFields;

	public const int CodeFieldNumber = 1;

	private int code_;

	public const int IsSuccessFieldNumber = 2;

	private bool isSuccess_;

	public const int EquipmentLevelFieldNumber = 3;

	private int equipmentLevel_;

	public const int MinSpiritualityValueFieldNumber = 4;

	private long minSpiritualityValue_;

	public const int MaxSpiritualityValueFieldNumber = 5;

	private long maxSpiritualityValue_;

	public const int MinRankFieldNumber = 6;

	private int minRank_;

	public const int MaxRankFieldNumber = 7;

	private int maxRank_;

	public const int MinCoolDownFieldNumber = 8;

	private long minCoolDown_;

	public const int MaxCoolDownFieldNumber = 9;

	private long maxCoolDown_;

	public const int UsedPhysiqueFieldNumber = 10;

	private long usedPhysique_;

	public const int ToughTenacityFieldNumber = 11;

	private int toughTenacity_;

	public const int UsedPerceptionFieldNumber = 12;

	private long usedPerception_;

	public const int MaterialNamesFieldNumber = 13;

	private static readonly FieldCodec<string> _repeated_materialNames_codec = FieldCodec.ForString(106u);

	private readonly RepeatedField<string> materialNames_ = new RepeatedField<string>();

	public const int CreatedByFieldNumber = 14;

	private string createdBy_ = "";

	public const int TypeFieldNumber = 15;

	private int type_;

	public const int VolumeFieldNumber = 16;

	private double volume_;

	[DebuggerNonUserCode]
	public static MessageParser<GoodsBookEquipmentDetailResp_1004011> Parser => _parser;

	[DebuggerNonUserCode]
	public static MessageDescriptor Descriptor => GoodsProtoReflection.Descriptor.MessageTypes[17];

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
	public int EquipmentLevel
	{
		get
		{
			return equipmentLevel_;
		}
		set
		{
			equipmentLevel_ = value;
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
	public int MinRank
	{
		get
		{
			return minRank_;
		}
		set
		{
			minRank_ = value;
		}
	}

	[DebuggerNonUserCode]
	public int MaxRank
	{
		get
		{
			return maxRank_;
		}
		set
		{
			maxRank_ = value;
		}
	}

	[DebuggerNonUserCode]
	public long MinCoolDown
	{
		get
		{
			return minCoolDown_;
		}
		set
		{
			minCoolDown_ = value;
		}
	}

	[DebuggerNonUserCode]
	public long MaxCoolDown
	{
		get
		{
			return maxCoolDown_;
		}
		set
		{
			maxCoolDown_ = value;
		}
	}

	[DebuggerNonUserCode]
	public long UsedPhysique
	{
		get
		{
			return usedPhysique_;
		}
		set
		{
			usedPhysique_ = value;
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
	public int Type
	{
		get
		{
			return type_;
		}
		set
		{
			type_ = value;
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
	public GoodsBookEquipmentDetailResp_1004011()
	{
	}

	[DebuggerNonUserCode]
	public GoodsBookEquipmentDetailResp_1004011(GoodsBookEquipmentDetailResp_1004011 other)
		: this()
	{
		code_ = other.code_;
		isSuccess_ = other.isSuccess_;
		equipmentLevel_ = other.equipmentLevel_;
		minSpiritualityValue_ = other.minSpiritualityValue_;
		maxSpiritualityValue_ = other.maxSpiritualityValue_;
		minRank_ = other.minRank_;
		maxRank_ = other.maxRank_;
		minCoolDown_ = other.minCoolDown_;
		maxCoolDown_ = other.maxCoolDown_;
		usedPhysique_ = other.usedPhysique_;
		toughTenacity_ = other.toughTenacity_;
		usedPerception_ = other.usedPerception_;
		materialNames_ = other.materialNames_.Clone();
		createdBy_ = other.createdBy_;
		type_ = other.type_;
		volume_ = other.volume_;
		_unknownFields = UnknownFieldSet.Clone(other._unknownFields);
	}

	[DebuggerNonUserCode]
	public GoodsBookEquipmentDetailResp_1004011 Clone()
	{
		return new GoodsBookEquipmentDetailResp_1004011(this);
	}

	[DebuggerNonUserCode]
	public override bool Equals(object other)
	{
		return Equals(other as GoodsBookEquipmentDetailResp_1004011);
	}

	[DebuggerNonUserCode]
	public bool Equals(GoodsBookEquipmentDetailResp_1004011 other)
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
		if (EquipmentLevel != other.EquipmentLevel)
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
		if (MinRank != other.MinRank)
		{
			return false;
		}
		if (MaxRank != other.MaxRank)
		{
			return false;
		}
		if (MinCoolDown != other.MinCoolDown)
		{
			return false;
		}
		if (MaxCoolDown != other.MaxCoolDown)
		{
			return false;
		}
		if (UsedPhysique != other.UsedPhysique)
		{
			return false;
		}
		if (ToughTenacity != other.ToughTenacity)
		{
			return false;
		}
		if (UsedPerception != other.UsedPerception)
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
		if (Type != other.Type)
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
		if (EquipmentLevel != 0)
		{
			num ^= EquipmentLevel.GetHashCode();
		}
		if (MinSpiritualityValue != 0L)
		{
			num ^= MinSpiritualityValue.GetHashCode();
		}
		if (MaxSpiritualityValue != 0L)
		{
			num ^= MaxSpiritualityValue.GetHashCode();
		}
		if (MinRank != 0)
		{
			num ^= MinRank.GetHashCode();
		}
		if (MaxRank != 0)
		{
			num ^= MaxRank.GetHashCode();
		}
		if (MinCoolDown != 0L)
		{
			num ^= MinCoolDown.GetHashCode();
		}
		if (MaxCoolDown != 0L)
		{
			num ^= MaxCoolDown.GetHashCode();
		}
		if (UsedPhysique != 0L)
		{
			num ^= UsedPhysique.GetHashCode();
		}
		if (ToughTenacity != 0)
		{
			num ^= ToughTenacity.GetHashCode();
		}
		if (UsedPerception != 0L)
		{
			num ^= UsedPerception.GetHashCode();
		}
		num ^= materialNames_.GetHashCode();
		if (CreatedBy.Length != 0)
		{
			num ^= CreatedBy.GetHashCode();
		}
		if (Type != 0)
		{
			num ^= Type.GetHashCode();
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
		if (EquipmentLevel != 0)
		{
			output.WriteRawTag(24);
			output.WriteInt32(EquipmentLevel);
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
		if (MinRank != 0)
		{
			output.WriteRawTag(48);
			output.WriteInt32(MinRank);
		}
		if (MaxRank != 0)
		{
			output.WriteRawTag(56);
			output.WriteInt32(MaxRank);
		}
		if (MinCoolDown != 0L)
		{
			output.WriteRawTag(64);
			output.WriteInt64(MinCoolDown);
		}
		if (MaxCoolDown != 0L)
		{
			output.WriteRawTag(72);
			output.WriteInt64(MaxCoolDown);
		}
		if (UsedPhysique != 0L)
		{
			output.WriteRawTag(80);
			output.WriteInt64(UsedPhysique);
		}
		if (ToughTenacity != 0)
		{
			output.WriteRawTag(88);
			output.WriteInt32(ToughTenacity);
		}
		if (UsedPerception != 0L)
		{
			output.WriteRawTag(96);
			output.WriteInt64(UsedPerception);
		}
		materialNames_.WriteTo(output, _repeated_materialNames_codec);
		if (CreatedBy.Length != 0)
		{
			output.WriteRawTag(114);
			output.WriteString(CreatedBy);
		}
		if (Type != 0)
		{
			output.WriteRawTag(120);
			output.WriteInt32(Type);
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
		if (EquipmentLevel != 0)
		{
			num += 1 + CodedOutputStream.ComputeInt32Size(EquipmentLevel);
		}
		if (MinSpiritualityValue != 0L)
		{
			num += 1 + CodedOutputStream.ComputeInt64Size(MinSpiritualityValue);
		}
		if (MaxSpiritualityValue != 0L)
		{
			num += 1 + CodedOutputStream.ComputeInt64Size(MaxSpiritualityValue);
		}
		if (MinRank != 0)
		{
			num += 1 + CodedOutputStream.ComputeInt32Size(MinRank);
		}
		if (MaxRank != 0)
		{
			num += 1 + CodedOutputStream.ComputeInt32Size(MaxRank);
		}
		if (MinCoolDown != 0L)
		{
			num += 1 + CodedOutputStream.ComputeInt64Size(MinCoolDown);
		}
		if (MaxCoolDown != 0L)
		{
			num += 1 + CodedOutputStream.ComputeInt64Size(MaxCoolDown);
		}
		if (UsedPhysique != 0L)
		{
			num += 1 + CodedOutputStream.ComputeInt64Size(UsedPhysique);
		}
		if (ToughTenacity != 0)
		{
			num += 1 + CodedOutputStream.ComputeInt32Size(ToughTenacity);
		}
		if (UsedPerception != 0L)
		{
			num += 1 + CodedOutputStream.ComputeInt64Size(UsedPerception);
		}
		num += materialNames_.CalculateSize(_repeated_materialNames_codec);
		if (CreatedBy.Length != 0)
		{
			num += 1 + CodedOutputStream.ComputeStringSize(CreatedBy);
		}
		if (Type != 0)
		{
			num += 1 + CodedOutputStream.ComputeInt32Size(Type);
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
	public void MergeFrom(GoodsBookEquipmentDetailResp_1004011 other)
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
			if (other.EquipmentLevel != 0)
			{
				EquipmentLevel = other.EquipmentLevel;
			}
			if (other.MinSpiritualityValue != 0L)
			{
				MinSpiritualityValue = other.MinSpiritualityValue;
			}
			if (other.MaxSpiritualityValue != 0L)
			{
				MaxSpiritualityValue = other.MaxSpiritualityValue;
			}
			if (other.MinRank != 0)
			{
				MinRank = other.MinRank;
			}
			if (other.MaxRank != 0)
			{
				MaxRank = other.MaxRank;
			}
			if (other.MinCoolDown != 0L)
			{
				MinCoolDown = other.MinCoolDown;
			}
			if (other.MaxCoolDown != 0L)
			{
				MaxCoolDown = other.MaxCoolDown;
			}
			if (other.UsedPhysique != 0L)
			{
				UsedPhysique = other.UsedPhysique;
			}
			if (other.ToughTenacity != 0)
			{
				ToughTenacity = other.ToughTenacity;
			}
			if (other.UsedPerception != 0L)
			{
				UsedPerception = other.UsedPerception;
			}
			materialNames_.Add(other.materialNames_);
			if (other.CreatedBy.Length != 0)
			{
				CreatedBy = other.CreatedBy;
			}
			if (other.Type != 0)
			{
				Type = other.Type;
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
				EquipmentLevel = input.ReadInt32();
				break;
			case 32u:
				MinSpiritualityValue = input.ReadInt64();
				break;
			case 40u:
				MaxSpiritualityValue = input.ReadInt64();
				break;
			case 48u:
				MinRank = input.ReadInt32();
				break;
			case 56u:
				MaxRank = input.ReadInt32();
				break;
			case 64u:
				MinCoolDown = input.ReadInt64();
				break;
			case 72u:
				MaxCoolDown = input.ReadInt64();
				break;
			case 80u:
				UsedPhysique = input.ReadInt64();
				break;
			case 88u:
				ToughTenacity = input.ReadInt32();
				break;
			case 96u:
				UsedPerception = input.ReadInt64();
				break;
			case 106u:
				materialNames_.AddEntriesFrom(input, _repeated_materialNames_codec);
				break;
			case 114u:
				CreatedBy = input.ReadString();
				break;
			case 120u:
				Type = input.ReadInt32();
				break;
			case 129u:
				Volume = input.ReadDouble();
				break;
			}
		}
	}
}
