using System;
using System.Diagnostics;
using Google.Protobuf;
using Google.Protobuf.Collections;
using Google.Protobuf.Reflection;

public sealed class VerifyCreateTemplatePlantResp_1008058 : IMessage<VerifyCreateTemplatePlantResp_1008058>, IMessage, IEquatable<VerifyCreateTemplatePlantResp_1008058>, IDeepCloneable<VerifyCreateTemplatePlantResp_1008058>
{
	private static readonly MessageParser<VerifyCreateTemplatePlantResp_1008058> _parser = new MessageParser<VerifyCreateTemplatePlantResp_1008058>(() => new VerifyCreateTemplatePlantResp_1008058());

	private UnknownFieldSet _unknownFields;

	public const int CodeFieldNumber = 1;

	private int code_;

	public const int IsSuccessFieldNumber = 2;

	private bool isSuccess_;

	public const int CreatedIsSuccessFieldNumber = 3;

	private bool createdIsSuccess_;

	public const int CurrentCleverFieldNumber = 4;

	private int currentClever_;

	public const int FourGasFieldNumber = 5;

	private double fourGas_;

	public const int FiveFlavourFieldNumber = 6;

	private double fiveFlavour_;

	public const int ReinforcingDiarrhoeaFieldNumber = 7;

	private double reinforcingDiarrhoea_;

	public const int AffiliationAcupointFieldNumber = 8;

	private int affiliationAcupoint_;

	public const int MapNameFieldNumber = 9;

	private string mapName_ = "";

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

	private string createdBy_ = "";

	public const int TemplatePlantIdFieldNumber = 17;

	private string templatePlantId_ = "";

	[DebuggerNonUserCode]
	public static MessageParser<VerifyCreateTemplatePlantResp_1008058> Parser => _parser;

	[DebuggerNonUserCode]
	public static MessageDescriptor Descriptor => UnrealWorldProtoReflection.Descriptor.MessageTypes[73];

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
	public double FourGas
	{
		get
		{
			return fourGas_;
		}
		set
		{
			fourGas_ = value;
		}
	}

	[DebuggerNonUserCode]
	public double FiveFlavour
	{
		get
		{
			return fiveFlavour_;
		}
		set
		{
			fiveFlavour_ = value;
		}
	}

	[DebuggerNonUserCode]
	public double ReinforcingDiarrhoea
	{
		get
		{
			return reinforcingDiarrhoea_;
		}
		set
		{
			reinforcingDiarrhoea_ = value;
		}
	}

	[DebuggerNonUserCode]
	public int AffiliationAcupoint
	{
		get
		{
			return affiliationAcupoint_;
		}
		set
		{
			affiliationAcupoint_ = value;
		}
	}

	[DebuggerNonUserCode]
	public string MapName
	{
		get
		{
			return mapName_;
		}
		set
		{
			mapName_ = ProtoPreconditions.CheckNotNull(value, "value");
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
	public string TemplatePlantId
	{
		get
		{
			return templatePlantId_;
		}
		set
		{
			templatePlantId_ = ProtoPreconditions.CheckNotNull(value, "value");
		}
	}

	private MessageDescriptor pb_003A_003AGoogle_002EProtobuf_002EIMessage_002Eget_Descriptor()
	{
		return Descriptor;
	}

	[DebuggerNonUserCode]
	public VerifyCreateTemplatePlantResp_1008058()
	{
	}

	[DebuggerNonUserCode]
	public VerifyCreateTemplatePlantResp_1008058(VerifyCreateTemplatePlantResp_1008058 other)
		: this()
	{
		code_ = other.code_;
		isSuccess_ = other.isSuccess_;
		createdIsSuccess_ = other.createdIsSuccess_;
		currentClever_ = other.currentClever_;
		fourGas_ = other.fourGas_;
		fiveFlavour_ = other.fiveFlavour_;
		reinforcingDiarrhoea_ = other.reinforcingDiarrhoea_;
		affiliationAcupoint_ = other.affiliationAcupoint_;
		mapName_ = other.mapName_;
		conversionRate_ = other.conversionRate_;
		stage_ = other.stage_;
		rank_ = other.rank_;
		fiveElement_ = other.fiveElement_;
		liveLevel_ = other.liveLevel_;
		liveProficiency_ = other.liveProficiency_;
		createdBy_ = other.createdBy_;
		templatePlantId_ = other.templatePlantId_;
		_unknownFields = UnknownFieldSet.Clone(other._unknownFields);
	}

	[DebuggerNonUserCode]
	public VerifyCreateTemplatePlantResp_1008058 Clone()
	{
		return new VerifyCreateTemplatePlantResp_1008058(this);
	}

	[DebuggerNonUserCode]
	public override bool Equals(object other)
	{
		return Equals(other as VerifyCreateTemplatePlantResp_1008058);
	}

	[DebuggerNonUserCode]
	public bool Equals(VerifyCreateTemplatePlantResp_1008058 other)
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
		if (!ProtobufEqualityComparers.BitwiseDoubleEqualityComparer.Equals(FourGas, other.FourGas))
		{
			return false;
		}
		if (!ProtobufEqualityComparers.BitwiseDoubleEqualityComparer.Equals(FiveFlavour, other.FiveFlavour))
		{
			return false;
		}
		if (!ProtobufEqualityComparers.BitwiseDoubleEqualityComparer.Equals(ReinforcingDiarrhoea, other.ReinforcingDiarrhoea))
		{
			return false;
		}
		if (AffiliationAcupoint != other.AffiliationAcupoint)
		{
			return false;
		}
		if (MapName != other.MapName)
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
		if (TemplatePlantId != other.TemplatePlantId)
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
		if (FourGas != 0.0)
		{
			num ^= ProtobufEqualityComparers.BitwiseDoubleEqualityComparer.GetHashCode(FourGas);
		}
		if (FiveFlavour != 0.0)
		{
			num ^= ProtobufEqualityComparers.BitwiseDoubleEqualityComparer.GetHashCode(FiveFlavour);
		}
		if (ReinforcingDiarrhoea != 0.0)
		{
			num ^= ProtobufEqualityComparers.BitwiseDoubleEqualityComparer.GetHashCode(ReinforcingDiarrhoea);
		}
		if (AffiliationAcupoint != 0)
		{
			num ^= AffiliationAcupoint.GetHashCode();
		}
		if (MapName.Length != 0)
		{
			num ^= MapName.GetHashCode();
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
		if (CreatedBy.Length != 0)
		{
			num ^= CreatedBy.GetHashCode();
		}
		if (TemplatePlantId.Length != 0)
		{
			num ^= TemplatePlantId.GetHashCode();
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
		if (FourGas != 0.0)
		{
			output.WriteRawTag(41);
			output.WriteDouble(FourGas);
		}
		if (FiveFlavour != 0.0)
		{
			output.WriteRawTag(49);
			output.WriteDouble(FiveFlavour);
		}
		if (ReinforcingDiarrhoea != 0.0)
		{
			output.WriteRawTag(57);
			output.WriteDouble(ReinforcingDiarrhoea);
		}
		if (AffiliationAcupoint != 0)
		{
			output.WriteRawTag(64);
			output.WriteInt32(AffiliationAcupoint);
		}
		if (MapName.Length != 0)
		{
			output.WriteRawTag(74);
			output.WriteString(MapName);
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
		if (CreatedBy.Length != 0)
		{
			output.WriteRawTag(130, 1);
			output.WriteString(CreatedBy);
		}
		if (TemplatePlantId.Length != 0)
		{
			output.WriteRawTag(138, 1);
			output.WriteString(TemplatePlantId);
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
		if (FourGas != 0.0)
		{
			num += 9;
		}
		if (FiveFlavour != 0.0)
		{
			num += 9;
		}
		if (ReinforcingDiarrhoea != 0.0)
		{
			num += 9;
		}
		if (AffiliationAcupoint != 0)
		{
			num += 1 + CodedOutputStream.ComputeInt32Size(AffiliationAcupoint);
		}
		if (MapName.Length != 0)
		{
			num += 1 + CodedOutputStream.ComputeStringSize(MapName);
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
		if (CreatedBy.Length != 0)
		{
			num += 2 + CodedOutputStream.ComputeStringSize(CreatedBy);
		}
		if (TemplatePlantId.Length != 0)
		{
			num += 2 + CodedOutputStream.ComputeStringSize(TemplatePlantId);
		}
		if (_unknownFields != null)
		{
			num += _unknownFields.CalculateSize();
		}
		return num;
	}

	[DebuggerNonUserCode]
	public void MergeFrom(VerifyCreateTemplatePlantResp_1008058 other)
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
			if (other.FourGas != 0.0)
			{
				FourGas = other.FourGas;
			}
			if (other.FiveFlavour != 0.0)
			{
				FiveFlavour = other.FiveFlavour;
			}
			if (other.ReinforcingDiarrhoea != 0.0)
			{
				ReinforcingDiarrhoea = other.ReinforcingDiarrhoea;
			}
			if (other.AffiliationAcupoint != 0)
			{
				AffiliationAcupoint = other.AffiliationAcupoint;
			}
			if (other.MapName.Length != 0)
			{
				MapName = other.MapName;
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
			if (other.CreatedBy.Length != 0)
			{
				CreatedBy = other.CreatedBy;
			}
			if (other.TemplatePlantId.Length != 0)
			{
				TemplatePlantId = other.TemplatePlantId;
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
			case 41u:
				FourGas = input.ReadDouble();
				break;
			case 49u:
				FiveFlavour = input.ReadDouble();
				break;
			case 57u:
				ReinforcingDiarrhoea = input.ReadDouble();
				break;
			case 64u:
				AffiliationAcupoint = input.ReadInt32();
				break;
			case 74u:
				MapName = input.ReadString();
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
			case 130u:
				CreatedBy = input.ReadString();
				break;
			case 138u:
				TemplatePlantId = input.ReadString();
				break;
			}
		}
	}
}
