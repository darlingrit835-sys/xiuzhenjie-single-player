using System;
using System.Diagnostics;
using Google.Protobuf;
using Google.Protobuf.Collections;
using Google.Protobuf.Reflection;

public sealed class VerifyCreateMethodEquipmentResp_1008073 : IMessage<VerifyCreateMethodEquipmentResp_1008073>, IMessage, IEquatable<VerifyCreateMethodEquipmentResp_1008073>, IDeepCloneable<VerifyCreateMethodEquipmentResp_1008073>
{
	private static readonly MessageParser<VerifyCreateMethodEquipmentResp_1008073> _parser = new MessageParser<VerifyCreateMethodEquipmentResp_1008073>(() => new VerifyCreateMethodEquipmentResp_1008073());

	private UnknownFieldSet _unknownFields;

	public const int CodeFieldNumber = 1;

	private int code_;

	public const int IsSuccessFieldNumber = 2;

	private bool isSuccess_;

	public const int CreatedIsSuccessFieldNumber = 3;

	private bool createdIsSuccess_;

	public const int CurrentCleverFieldNumber = 4;

	private int currentClever_;

	public const int MinSpiritualityValueFieldNumber = 5;

	private long minSpiritualityValue_;

	public const int MaxSpiritualityValueFieldNumber = 6;

	private long maxSpiritualityValue_;

	public const int StageFieldNumber = 7;

	private int stage_;

	public const int FiveElementFieldNumber = 8;

	private int fiveElement_;

	public const int MinRankFieldNumber = 9;

	private int minRank_;

	public const int MaxRankFieldNumber = 10;

	private int maxRank_;

	public const int UsedPerceptionFieldNumber = 11;

	private long usedPerception_;

	public const int ToughTenacityFieldNumber = 12;

	private int toughTenacity_;

	public const int CreatedByFieldNumber = 13;

	private string createdBy_ = "";

	public const int VolumeFieldNumber = 14;

	private double volume_;

	public const int MinCoolDownFieldNumber = 15;

	private long minCoolDown_;

	public const int MaxCoolDownFieldNumber = 16;

	private long maxCoolDown_;

	public const int TypeFieldNumber = 17;

	private int type_;

	public const int UsedPhysiqueFieldNumber = 18;

	private long usedPhysique_;

	[DebuggerNonUserCode]
	public static MessageParser<VerifyCreateMethodEquipmentResp_1008073> Parser => _parser;

	[DebuggerNonUserCode]
	public static MessageDescriptor Descriptor => UnrealWorldProtoReflection.Descriptor.MessageTypes[88];

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

	private MessageDescriptor pb_003A_003AGoogle_002EProtobuf_002EIMessage_002Eget_Descriptor()
	{
		return Descriptor;
	}

	[DebuggerNonUserCode]
	public VerifyCreateMethodEquipmentResp_1008073()
	{
	}

	[DebuggerNonUserCode]
	public VerifyCreateMethodEquipmentResp_1008073(VerifyCreateMethodEquipmentResp_1008073 other)
		: this()
	{
		code_ = other.code_;
		isSuccess_ = other.isSuccess_;
		createdIsSuccess_ = other.createdIsSuccess_;
		currentClever_ = other.currentClever_;
		minSpiritualityValue_ = other.minSpiritualityValue_;
		maxSpiritualityValue_ = other.maxSpiritualityValue_;
		stage_ = other.stage_;
		fiveElement_ = other.fiveElement_;
		minRank_ = other.minRank_;
		maxRank_ = other.maxRank_;
		usedPerception_ = other.usedPerception_;
		toughTenacity_ = other.toughTenacity_;
		createdBy_ = other.createdBy_;
		volume_ = other.volume_;
		minCoolDown_ = other.minCoolDown_;
		maxCoolDown_ = other.maxCoolDown_;
		type_ = other.type_;
		usedPhysique_ = other.usedPhysique_;
		_unknownFields = UnknownFieldSet.Clone(other._unknownFields);
	}

	[DebuggerNonUserCode]
	public VerifyCreateMethodEquipmentResp_1008073 Clone()
	{
		return new VerifyCreateMethodEquipmentResp_1008073(this);
	}

	[DebuggerNonUserCode]
	public override bool Equals(object other)
	{
		return Equals(other as VerifyCreateMethodEquipmentResp_1008073);
	}

	[DebuggerNonUserCode]
	public bool Equals(VerifyCreateMethodEquipmentResp_1008073 other)
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
		if (MinSpiritualityValue != other.MinSpiritualityValue)
		{
			return false;
		}
		if (MaxSpiritualityValue != other.MaxSpiritualityValue)
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
		if (MinRank != other.MinRank)
		{
			return false;
		}
		if (MaxRank != other.MaxRank)
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
		if (CreatedBy != other.CreatedBy)
		{
			return false;
		}
		if (!ProtobufEqualityComparers.BitwiseDoubleEqualityComparer.Equals(Volume, other.Volume))
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
		if (Type != other.Type)
		{
			return false;
		}
		if (UsedPhysique != other.UsedPhysique)
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
		if (MinSpiritualityValue != 0L)
		{
			num ^= MinSpiritualityValue.GetHashCode();
		}
		if (MaxSpiritualityValue != 0L)
		{
			num ^= MaxSpiritualityValue.GetHashCode();
		}
		if (Stage != 0)
		{
			num ^= Stage.GetHashCode();
		}
		if (FiveElement != 0)
		{
			num ^= FiveElement.GetHashCode();
		}
		if (MinRank != 0)
		{
			num ^= MinRank.GetHashCode();
		}
		if (MaxRank != 0)
		{
			num ^= MaxRank.GetHashCode();
		}
		if (UsedPerception != 0L)
		{
			num ^= UsedPerception.GetHashCode();
		}
		if (ToughTenacity != 0)
		{
			num ^= ToughTenacity.GetHashCode();
		}
		if (CreatedBy.Length != 0)
		{
			num ^= CreatedBy.GetHashCode();
		}
		if (Volume != 0.0)
		{
			num ^= ProtobufEqualityComparers.BitwiseDoubleEqualityComparer.GetHashCode(Volume);
		}
		if (MinCoolDown != 0L)
		{
			num ^= MinCoolDown.GetHashCode();
		}
		if (MaxCoolDown != 0L)
		{
			num ^= MaxCoolDown.GetHashCode();
		}
		if (Type != 0)
		{
			num ^= Type.GetHashCode();
		}
		if (UsedPhysique != 0L)
		{
			num ^= UsedPhysique.GetHashCode();
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
		if (MinSpiritualityValue != 0L)
		{
			output.WriteRawTag(40);
			output.WriteInt64(MinSpiritualityValue);
		}
		if (MaxSpiritualityValue != 0L)
		{
			output.WriteRawTag(48);
			output.WriteInt64(MaxSpiritualityValue);
		}
		if (Stage != 0)
		{
			output.WriteRawTag(56);
			output.WriteInt32(Stage);
		}
		if (FiveElement != 0)
		{
			output.WriteRawTag(64);
			output.WriteInt32(FiveElement);
		}
		if (MinRank != 0)
		{
			output.WriteRawTag(72);
			output.WriteInt32(MinRank);
		}
		if (MaxRank != 0)
		{
			output.WriteRawTag(80);
			output.WriteInt32(MaxRank);
		}
		if (UsedPerception != 0L)
		{
			output.WriteRawTag(88);
			output.WriteInt64(UsedPerception);
		}
		if (ToughTenacity != 0)
		{
			output.WriteRawTag(96);
			output.WriteInt32(ToughTenacity);
		}
		if (CreatedBy.Length != 0)
		{
			output.WriteRawTag(106);
			output.WriteString(CreatedBy);
		}
		if (Volume != 0.0)
		{
			output.WriteRawTag(113);
			output.WriteDouble(Volume);
		}
		if (MinCoolDown != 0L)
		{
			output.WriteRawTag(120);
			output.WriteInt64(MinCoolDown);
		}
		if (MaxCoolDown != 0L)
		{
			output.WriteRawTag(128, 1);
			output.WriteInt64(MaxCoolDown);
		}
		if (Type != 0)
		{
			output.WriteRawTag(136, 1);
			output.WriteInt32(Type);
		}
		if (UsedPhysique != 0L)
		{
			output.WriteRawTag(144, 1);
			output.WriteInt64(UsedPhysique);
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
		if (MinSpiritualityValue != 0L)
		{
			num += 1 + CodedOutputStream.ComputeInt64Size(MinSpiritualityValue);
		}
		if (MaxSpiritualityValue != 0L)
		{
			num += 1 + CodedOutputStream.ComputeInt64Size(MaxSpiritualityValue);
		}
		if (Stage != 0)
		{
			num += 1 + CodedOutputStream.ComputeInt32Size(Stage);
		}
		if (FiveElement != 0)
		{
			num += 1 + CodedOutputStream.ComputeInt32Size(FiveElement);
		}
		if (MinRank != 0)
		{
			num += 1 + CodedOutputStream.ComputeInt32Size(MinRank);
		}
		if (MaxRank != 0)
		{
			num += 1 + CodedOutputStream.ComputeInt32Size(MaxRank);
		}
		if (UsedPerception != 0L)
		{
			num += 1 + CodedOutputStream.ComputeInt64Size(UsedPerception);
		}
		if (ToughTenacity != 0)
		{
			num += 1 + CodedOutputStream.ComputeInt32Size(ToughTenacity);
		}
		if (CreatedBy.Length != 0)
		{
			num += 1 + CodedOutputStream.ComputeStringSize(CreatedBy);
		}
		if (Volume != 0.0)
		{
			num += 9;
		}
		if (MinCoolDown != 0L)
		{
			num += 1 + CodedOutputStream.ComputeInt64Size(MinCoolDown);
		}
		if (MaxCoolDown != 0L)
		{
			num += 2 + CodedOutputStream.ComputeInt64Size(MaxCoolDown);
		}
		if (Type != 0)
		{
			num += 2 + CodedOutputStream.ComputeInt32Size(Type);
		}
		if (UsedPhysique != 0L)
		{
			num += 2 + CodedOutputStream.ComputeInt64Size(UsedPhysique);
		}
		if (_unknownFields != null)
		{
			num += _unknownFields.CalculateSize();
		}
		return num;
	}

	[DebuggerNonUserCode]
	public void MergeFrom(VerifyCreateMethodEquipmentResp_1008073 other)
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
			if (other.MinSpiritualityValue != 0L)
			{
				MinSpiritualityValue = other.MinSpiritualityValue;
			}
			if (other.MaxSpiritualityValue != 0L)
			{
				MaxSpiritualityValue = other.MaxSpiritualityValue;
			}
			if (other.Stage != 0)
			{
				Stage = other.Stage;
			}
			if (other.FiveElement != 0)
			{
				FiveElement = other.FiveElement;
			}
			if (other.MinRank != 0)
			{
				MinRank = other.MinRank;
			}
			if (other.MaxRank != 0)
			{
				MaxRank = other.MaxRank;
			}
			if (other.UsedPerception != 0L)
			{
				UsedPerception = other.UsedPerception;
			}
			if (other.ToughTenacity != 0)
			{
				ToughTenacity = other.ToughTenacity;
			}
			if (other.CreatedBy.Length != 0)
			{
				CreatedBy = other.CreatedBy;
			}
			if (other.Volume != 0.0)
			{
				Volume = other.Volume;
			}
			if (other.MinCoolDown != 0L)
			{
				MinCoolDown = other.MinCoolDown;
			}
			if (other.MaxCoolDown != 0L)
			{
				MaxCoolDown = other.MaxCoolDown;
			}
			if (other.Type != 0)
			{
				Type = other.Type;
			}
			if (other.UsedPhysique != 0L)
			{
				UsedPhysique = other.UsedPhysique;
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
				MinSpiritualityValue = input.ReadInt64();
				break;
			case 48u:
				MaxSpiritualityValue = input.ReadInt64();
				break;
			case 56u:
				Stage = input.ReadInt32();
				break;
			case 64u:
				FiveElement = input.ReadInt32();
				break;
			case 72u:
				MinRank = input.ReadInt32();
				break;
			case 80u:
				MaxRank = input.ReadInt32();
				break;
			case 88u:
				UsedPerception = input.ReadInt64();
				break;
			case 96u:
				ToughTenacity = input.ReadInt32();
				break;
			case 106u:
				CreatedBy = input.ReadString();
				break;
			case 113u:
				Volume = input.ReadDouble();
				break;
			case 120u:
				MinCoolDown = input.ReadInt64();
				break;
			case 128u:
				MaxCoolDown = input.ReadInt64();
				break;
			case 136u:
				Type = input.ReadInt32();
				break;
			case 144u:
				UsedPhysique = input.ReadInt64();
				break;
			}
		}
	}
}
