using System;
using System.Diagnostics;
using Google.Protobuf;
using Google.Protobuf.Collections;
using Google.Protobuf.Reflection;

public sealed class SoilDeployResp_1013007 : IMessage<SoilDeployResp_1013007>, IMessage, IEquatable<SoilDeployResp_1013007>, IDeepCloneable<SoilDeployResp_1013007>
{
	private static readonly MessageParser<SoilDeployResp_1013007> _parser = new MessageParser<SoilDeployResp_1013007>(() => new SoilDeployResp_1013007());

	private UnknownFieldSet _unknownFields;

	public const int CodeFieldNumber = 1;

	private int code_;

	public const int IsSuccessFieldNumber = 2;

	private bool isSuccess_;

	public const int SoilIdFieldNumber = 3;

	private string soilId_ = "";

	public const int FiveElementFieldNumber = 4;

	private int fiveElement_;

	public const int LevelFieldNumber = 5;

	private int level_;

	public const int NameFieldNumber = 6;

	private string name_ = "";

	public const int RankFieldNumber = 7;

	private int rank_;

	public const int ReceiveValFieldNumber = 8;

	private double receiveVal_;

	public const int ConversionRateFieldNumber = 9;

	private double conversionRate_;

	public const int CurrentShinyStoneFieldNumber = 10;

	private long currentShinyStone_;

	[DebuggerNonUserCode]
	public static MessageParser<SoilDeployResp_1013007> Parser => _parser;

	[DebuggerNonUserCode]
	public static MessageDescriptor Descriptor => PlantProtoReflection.Descriptor.MessageTypes[8];

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
	public string SoilId
	{
		get
		{
			return soilId_;
		}
		set
		{
			soilId_ = ProtoPreconditions.CheckNotNull(value, "value");
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
	public string Name
	{
		get
		{
			return name_;
		}
		set
		{
			name_ = ProtoPreconditions.CheckNotNull(value, "value");
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
	public double ReceiveVal
	{
		get
		{
			return receiveVal_;
		}
		set
		{
			receiveVal_ = value;
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
	public long CurrentShinyStone
	{
		get
		{
			return currentShinyStone_;
		}
		set
		{
			currentShinyStone_ = value;
		}
	}

	private MessageDescriptor pb_003A_003AGoogle_002EProtobuf_002EIMessage_002Eget_Descriptor()
	{
		return Descriptor;
	}

	[DebuggerNonUserCode]
	public SoilDeployResp_1013007()
	{
	}

	[DebuggerNonUserCode]
	public SoilDeployResp_1013007(SoilDeployResp_1013007 other)
		: this()
	{
		code_ = other.code_;
		isSuccess_ = other.isSuccess_;
		soilId_ = other.soilId_;
		fiveElement_ = other.fiveElement_;
		level_ = other.level_;
		name_ = other.name_;
		rank_ = other.rank_;
		receiveVal_ = other.receiveVal_;
		conversionRate_ = other.conversionRate_;
		currentShinyStone_ = other.currentShinyStone_;
		_unknownFields = UnknownFieldSet.Clone(other._unknownFields);
	}

	[DebuggerNonUserCode]
	public SoilDeployResp_1013007 Clone()
	{
		return new SoilDeployResp_1013007(this);
	}

	[DebuggerNonUserCode]
	public override bool Equals(object other)
	{
		return Equals(other as SoilDeployResp_1013007);
	}

	[DebuggerNonUserCode]
	public bool Equals(SoilDeployResp_1013007 other)
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
		if (SoilId != other.SoilId)
		{
			return false;
		}
		if (FiveElement != other.FiveElement)
		{
			return false;
		}
		if (Level != other.Level)
		{
			return false;
		}
		if (Name != other.Name)
		{
			return false;
		}
		if (Rank != other.Rank)
		{
			return false;
		}
		if (!ProtobufEqualityComparers.BitwiseDoubleEqualityComparer.Equals(ReceiveVal, other.ReceiveVal))
		{
			return false;
		}
		if (!ProtobufEqualityComparers.BitwiseDoubleEqualityComparer.Equals(ConversionRate, other.ConversionRate))
		{
			return false;
		}
		if (CurrentShinyStone != other.CurrentShinyStone)
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
		if (SoilId.Length != 0)
		{
			num ^= SoilId.GetHashCode();
		}
		if (FiveElement != 0)
		{
			num ^= FiveElement.GetHashCode();
		}
		if (Level != 0)
		{
			num ^= Level.GetHashCode();
		}
		if (Name.Length != 0)
		{
			num ^= Name.GetHashCode();
		}
		if (Rank != 0)
		{
			num ^= Rank.GetHashCode();
		}
		if (ReceiveVal != 0.0)
		{
			num ^= ProtobufEqualityComparers.BitwiseDoubleEqualityComparer.GetHashCode(ReceiveVal);
		}
		if (ConversionRate != 0.0)
		{
			num ^= ProtobufEqualityComparers.BitwiseDoubleEqualityComparer.GetHashCode(ConversionRate);
		}
		if (CurrentShinyStone != 0L)
		{
			num ^= CurrentShinyStone.GetHashCode();
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
		if (SoilId.Length != 0)
		{
			output.WriteRawTag(26);
			output.WriteString(SoilId);
		}
		if (FiveElement != 0)
		{
			output.WriteRawTag(32);
			output.WriteInt32(FiveElement);
		}
		if (Level != 0)
		{
			output.WriteRawTag(40);
			output.WriteInt32(Level);
		}
		if (Name.Length != 0)
		{
			output.WriteRawTag(50);
			output.WriteString(Name);
		}
		if (Rank != 0)
		{
			output.WriteRawTag(56);
			output.WriteInt32(Rank);
		}
		if (ReceiveVal != 0.0)
		{
			output.WriteRawTag(65);
			output.WriteDouble(ReceiveVal);
		}
		if (ConversionRate != 0.0)
		{
			output.WriteRawTag(73);
			output.WriteDouble(ConversionRate);
		}
		if (CurrentShinyStone != 0L)
		{
			output.WriteRawTag(80);
			output.WriteInt64(CurrentShinyStone);
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
		if (SoilId.Length != 0)
		{
			num += 1 + CodedOutputStream.ComputeStringSize(SoilId);
		}
		if (FiveElement != 0)
		{
			num += 1 + CodedOutputStream.ComputeInt32Size(FiveElement);
		}
		if (Level != 0)
		{
			num += 1 + CodedOutputStream.ComputeInt32Size(Level);
		}
		if (Name.Length != 0)
		{
			num += 1 + CodedOutputStream.ComputeStringSize(Name);
		}
		if (Rank != 0)
		{
			num += 1 + CodedOutputStream.ComputeInt32Size(Rank);
		}
		if (ReceiveVal != 0.0)
		{
			num += 9;
		}
		if (ConversionRate != 0.0)
		{
			num += 9;
		}
		if (CurrentShinyStone != 0L)
		{
			num += 1 + CodedOutputStream.ComputeInt64Size(CurrentShinyStone);
		}
		if (_unknownFields != null)
		{
			num += _unknownFields.CalculateSize();
		}
		return num;
	}

	[DebuggerNonUserCode]
	public void MergeFrom(SoilDeployResp_1013007 other)
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
			if (other.SoilId.Length != 0)
			{
				SoilId = other.SoilId;
			}
			if (other.FiveElement != 0)
			{
				FiveElement = other.FiveElement;
			}
			if (other.Level != 0)
			{
				Level = other.Level;
			}
			if (other.Name.Length != 0)
			{
				Name = other.Name;
			}
			if (other.Rank != 0)
			{
				Rank = other.Rank;
			}
			if (other.ReceiveVal != 0.0)
			{
				ReceiveVal = other.ReceiveVal;
			}
			if (other.ConversionRate != 0.0)
			{
				ConversionRate = other.ConversionRate;
			}
			if (other.CurrentShinyStone != 0L)
			{
				CurrentShinyStone = other.CurrentShinyStone;
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
				SoilId = input.ReadString();
				break;
			case 32u:
				FiveElement = input.ReadInt32();
				break;
			case 40u:
				Level = input.ReadInt32();
				break;
			case 50u:
				Name = input.ReadString();
				break;
			case 56u:
				Rank = input.ReadInt32();
				break;
			case 65u:
				ReceiveVal = input.ReadDouble();
				break;
			case 73u:
				ConversionRate = input.ReadDouble();
				break;
			case 80u:
				CurrentShinyStone = input.ReadInt64();
				break;
			}
		}
	}
}
