using System;
using System.Diagnostics;
using Google.Protobuf;
using Google.Protobuf.Collections;
using Google.Protobuf.Reflection;

public sealed class TemplateMaterialAnimalDetailResp_1011001 : IMessage<TemplateMaterialAnimalDetailResp_1011001>, IMessage, IEquatable<TemplateMaterialAnimalDetailResp_1011001>, IDeepCloneable<TemplateMaterialAnimalDetailResp_1011001>
{
	private static readonly MessageParser<TemplateMaterialAnimalDetailResp_1011001> _parser = new MessageParser<TemplateMaterialAnimalDetailResp_1011001>(() => new TemplateMaterialAnimalDetailResp_1011001());

	private UnknownFieldSet _unknownFields;

	public const int CodeFieldNumber = 1;

	private int code_;

	public const int IsSuccessFieldNumber = 2;

	private bool isSuccess_;

	public const int MapNameFieldNumber = 3;

	private string mapName_ = "";

	public const int StageFieldNumber = 4;

	private int stage_;

	public const int FiveElementFieldNumber = 5;

	private int fiveElement_;

	public const int FourGasFieldNumber = 6;

	private double fourGas_;

	public const int FiveFlavourFieldNumber = 7;

	private double fiveFlavour_;

	public const int ReinforcingDiarrhoeaFieldNumber = 8;

	private double reinforcingDiarrhoea_;

	public const int AffiliationAcupointFieldNumber = 9;

	private int affiliationAcupoint_;

	public const int IntensityFieldNumber = 10;

	private double intensity_;

	public const int TenacityFieldNumber = 11;

	private double tenacity_;

	[DebuggerNonUserCode]
	public static MessageParser<TemplateMaterialAnimalDetailResp_1011001> Parser => _parser;

	[DebuggerNonUserCode]
	public static MessageDescriptor Descriptor => TemplateProtoReflection.Descriptor.MessageTypes[1];

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
	public double Intensity
	{
		get
		{
			return intensity_;
		}
		set
		{
			intensity_ = value;
		}
	}

	[DebuggerNonUserCode]
	public double Tenacity
	{
		get
		{
			return tenacity_;
		}
		set
		{
			tenacity_ = value;
		}
	}

	private MessageDescriptor pb_003A_003AGoogle_002EProtobuf_002EIMessage_002Eget_Descriptor()
	{
		return Descriptor;
	}

	[DebuggerNonUserCode]
	public TemplateMaterialAnimalDetailResp_1011001()
	{
	}

	[DebuggerNonUserCode]
	public TemplateMaterialAnimalDetailResp_1011001(TemplateMaterialAnimalDetailResp_1011001 other)
		: this()
	{
		code_ = other.code_;
		isSuccess_ = other.isSuccess_;
		mapName_ = other.mapName_;
		stage_ = other.stage_;
		fiveElement_ = other.fiveElement_;
		fourGas_ = other.fourGas_;
		fiveFlavour_ = other.fiveFlavour_;
		reinforcingDiarrhoea_ = other.reinforcingDiarrhoea_;
		affiliationAcupoint_ = other.affiliationAcupoint_;
		intensity_ = other.intensity_;
		tenacity_ = other.tenacity_;
		_unknownFields = UnknownFieldSet.Clone(other._unknownFields);
	}

	[DebuggerNonUserCode]
	public TemplateMaterialAnimalDetailResp_1011001 Clone()
	{
		return new TemplateMaterialAnimalDetailResp_1011001(this);
	}

	[DebuggerNonUserCode]
	public override bool Equals(object other)
	{
		return Equals(other as TemplateMaterialAnimalDetailResp_1011001);
	}

	[DebuggerNonUserCode]
	public bool Equals(TemplateMaterialAnimalDetailResp_1011001 other)
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
		if (MapName != other.MapName)
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
		if (!ProtobufEqualityComparers.BitwiseDoubleEqualityComparer.Equals(Intensity, other.Intensity))
		{
			return false;
		}
		if (!ProtobufEqualityComparers.BitwiseDoubleEqualityComparer.Equals(Tenacity, other.Tenacity))
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
		if (MapName.Length != 0)
		{
			num ^= MapName.GetHashCode();
		}
		if (Stage != 0)
		{
			num ^= Stage.GetHashCode();
		}
		if (FiveElement != 0)
		{
			num ^= FiveElement.GetHashCode();
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
		if (Intensity != 0.0)
		{
			num ^= ProtobufEqualityComparers.BitwiseDoubleEqualityComparer.GetHashCode(Intensity);
		}
		if (Tenacity != 0.0)
		{
			num ^= ProtobufEqualityComparers.BitwiseDoubleEqualityComparer.GetHashCode(Tenacity);
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
		if (MapName.Length != 0)
		{
			output.WriteRawTag(26);
			output.WriteString(MapName);
		}
		if (Stage != 0)
		{
			output.WriteRawTag(32);
			output.WriteInt32(Stage);
		}
		if (FiveElement != 0)
		{
			output.WriteRawTag(40);
			output.WriteInt32(FiveElement);
		}
		if (FourGas != 0.0)
		{
			output.WriteRawTag(49);
			output.WriteDouble(FourGas);
		}
		if (FiveFlavour != 0.0)
		{
			output.WriteRawTag(57);
			output.WriteDouble(FiveFlavour);
		}
		if (ReinforcingDiarrhoea != 0.0)
		{
			output.WriteRawTag(65);
			output.WriteDouble(ReinforcingDiarrhoea);
		}
		if (AffiliationAcupoint != 0)
		{
			output.WriteRawTag(72);
			output.WriteInt32(AffiliationAcupoint);
		}
		if (Intensity != 0.0)
		{
			output.WriteRawTag(81);
			output.WriteDouble(Intensity);
		}
		if (Tenacity != 0.0)
		{
			output.WriteRawTag(89);
			output.WriteDouble(Tenacity);
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
		if (MapName.Length != 0)
		{
			num += 1 + CodedOutputStream.ComputeStringSize(MapName);
		}
		if (Stage != 0)
		{
			num += 1 + CodedOutputStream.ComputeInt32Size(Stage);
		}
		if (FiveElement != 0)
		{
			num += 1 + CodedOutputStream.ComputeInt32Size(FiveElement);
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
		if (Intensity != 0.0)
		{
			num += 9;
		}
		if (Tenacity != 0.0)
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
	public void MergeFrom(TemplateMaterialAnimalDetailResp_1011001 other)
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
			if (other.MapName.Length != 0)
			{
				MapName = other.MapName;
			}
			if (other.Stage != 0)
			{
				Stage = other.Stage;
			}
			if (other.FiveElement != 0)
			{
				FiveElement = other.FiveElement;
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
			if (other.Intensity != 0.0)
			{
				Intensity = other.Intensity;
			}
			if (other.Tenacity != 0.0)
			{
				Tenacity = other.Tenacity;
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
			case 26u:
				MapName = input.ReadString();
				break;
			case 32u:
				Stage = input.ReadInt32();
				break;
			case 40u:
				FiveElement = input.ReadInt32();
				break;
			case 49u:
				FourGas = input.ReadDouble();
				break;
			case 57u:
				FiveFlavour = input.ReadDouble();
				break;
			case 65u:
				ReinforcingDiarrhoea = input.ReadDouble();
				break;
			case 72u:
				AffiliationAcupoint = input.ReadInt32();
				break;
			case 81u:
				Intensity = input.ReadDouble();
				break;
			case 89u:
				Tenacity = input.ReadDouble();
				break;
			}
		}
	}
}
