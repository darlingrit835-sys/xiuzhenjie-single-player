using System;
using System.Diagnostics;
using Google.Protobuf;
using Google.Protobuf.Collections;
using Google.Protobuf.Reflection;

public sealed class VerifyCreateMethodMatrixResp_1008088 : IMessage<VerifyCreateMethodMatrixResp_1008088>, IMessage, IEquatable<VerifyCreateMethodMatrixResp_1008088>, IDeepCloneable<VerifyCreateMethodMatrixResp_1008088>
{
	private static readonly MessageParser<VerifyCreateMethodMatrixResp_1008088> _parser = new MessageParser<VerifyCreateMethodMatrixResp_1008088>(() => new VerifyCreateMethodMatrixResp_1008088());

	private UnknownFieldSet _unknownFields;

	public const int CodeFieldNumber = 1;

	private int code_;

	public const int IsSuccessFieldNumber = 2;

	private bool isSuccess_;

	public const int CreatedIsSuccessFieldNumber = 3;

	private bool createdIsSuccess_;

	public const int CurrentCleverFieldNumber = 4;

	private int currentClever_;

	public const int ConversionRateFieldNumber = 5;

	private double conversionRate_;

	public const int TypeFieldNumber = 6;

	private int type_;

	public const int UnitConsumeFieldNumber = 7;

	private int unitConsume_;

	public const int DurationFieldNumber = 8;

	private long duration_;

	public const int StageFieldNumber = 9;

	private int stage_;

	public const int RankFieldNumber = 10;

	private int rank_;

	public const int FiveElementFieldNumber = 11;

	private int fiveElement_;

	public const int LiveLevelFieldNumber = 12;

	private int liveLevel_;

	public const int LiveProficiencyFieldNumber = 13;

	private int liveProficiency_;

	public const int CreatedByFieldNumber = 14;

	private string createdBy_ = "";

	public const int MethodMatrixIdFieldNumber = 15;

	private string methodMatrixId_ = "";

	[DebuggerNonUserCode]
	public static MessageParser<VerifyCreateMethodMatrixResp_1008088> Parser => _parser;

	[DebuggerNonUserCode]
	public static MessageDescriptor Descriptor => UnrealWorldProtoReflection.Descriptor.MessageTypes[107];

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
	public int UnitConsume
	{
		get
		{
			return unitConsume_;
		}
		set
		{
			unitConsume_ = value;
		}
	}

	[DebuggerNonUserCode]
	public long Duration
	{
		get
		{
			return duration_;
		}
		set
		{
			duration_ = value;
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
	public string MethodMatrixId
	{
		get
		{
			return methodMatrixId_;
		}
		set
		{
			methodMatrixId_ = ProtoPreconditions.CheckNotNull(value, "value");
		}
	}

	private MessageDescriptor pb_003A_003AGoogle_002EProtobuf_002EIMessage_002Eget_Descriptor()
	{
		return Descriptor;
	}

	[DebuggerNonUserCode]
	public VerifyCreateMethodMatrixResp_1008088()
	{
	}

	[DebuggerNonUserCode]
	public VerifyCreateMethodMatrixResp_1008088(VerifyCreateMethodMatrixResp_1008088 other)
		: this()
	{
		code_ = other.code_;
		isSuccess_ = other.isSuccess_;
		createdIsSuccess_ = other.createdIsSuccess_;
		currentClever_ = other.currentClever_;
		conversionRate_ = other.conversionRate_;
		type_ = other.type_;
		unitConsume_ = other.unitConsume_;
		duration_ = other.duration_;
		stage_ = other.stage_;
		rank_ = other.rank_;
		fiveElement_ = other.fiveElement_;
		liveLevel_ = other.liveLevel_;
		liveProficiency_ = other.liveProficiency_;
		createdBy_ = other.createdBy_;
		methodMatrixId_ = other.methodMatrixId_;
		_unknownFields = UnknownFieldSet.Clone(other._unknownFields);
	}

	[DebuggerNonUserCode]
	public VerifyCreateMethodMatrixResp_1008088 Clone()
	{
		return new VerifyCreateMethodMatrixResp_1008088(this);
	}

	[DebuggerNonUserCode]
	public override bool Equals(object other)
	{
		return Equals(other as VerifyCreateMethodMatrixResp_1008088);
	}

	[DebuggerNonUserCode]
	public bool Equals(VerifyCreateMethodMatrixResp_1008088 other)
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
		if (!ProtobufEqualityComparers.BitwiseDoubleEqualityComparer.Equals(ConversionRate, other.ConversionRate))
		{
			return false;
		}
		if (Type != other.Type)
		{
			return false;
		}
		if (UnitConsume != other.UnitConsume)
		{
			return false;
		}
		if (Duration != other.Duration)
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
		if (MethodMatrixId != other.MethodMatrixId)
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
		if (ConversionRate != 0.0)
		{
			num ^= ProtobufEqualityComparers.BitwiseDoubleEqualityComparer.GetHashCode(ConversionRate);
		}
		if (Type != 0)
		{
			num ^= Type.GetHashCode();
		}
		if (UnitConsume != 0)
		{
			num ^= UnitConsume.GetHashCode();
		}
		if (Duration != 0L)
		{
			num ^= Duration.GetHashCode();
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
		if (MethodMatrixId.Length != 0)
		{
			num ^= MethodMatrixId.GetHashCode();
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
		if (ConversionRate != 0.0)
		{
			output.WriteRawTag(41);
			output.WriteDouble(ConversionRate);
		}
		if (Type != 0)
		{
			output.WriteRawTag(48);
			output.WriteInt32(Type);
		}
		if (UnitConsume != 0)
		{
			output.WriteRawTag(56);
			output.WriteInt32(UnitConsume);
		}
		if (Duration != 0L)
		{
			output.WriteRawTag(64);
			output.WriteInt64(Duration);
		}
		if (Stage != 0)
		{
			output.WriteRawTag(72);
			output.WriteInt32(Stage);
		}
		if (Rank != 0)
		{
			output.WriteRawTag(80);
			output.WriteInt32(Rank);
		}
		if (FiveElement != 0)
		{
			output.WriteRawTag(88);
			output.WriteInt32(FiveElement);
		}
		if (LiveLevel != 0)
		{
			output.WriteRawTag(96);
			output.WriteInt32(LiveLevel);
		}
		if (LiveProficiency != 0)
		{
			output.WriteRawTag(104);
			output.WriteInt32(LiveProficiency);
		}
		if (CreatedBy.Length != 0)
		{
			output.WriteRawTag(114);
			output.WriteString(CreatedBy);
		}
		if (MethodMatrixId.Length != 0)
		{
			output.WriteRawTag(122);
			output.WriteString(MethodMatrixId);
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
		if (ConversionRate != 0.0)
		{
			num += 9;
		}
		if (Type != 0)
		{
			num += 1 + CodedOutputStream.ComputeInt32Size(Type);
		}
		if (UnitConsume != 0)
		{
			num += 1 + CodedOutputStream.ComputeInt32Size(UnitConsume);
		}
		if (Duration != 0L)
		{
			num += 1 + CodedOutputStream.ComputeInt64Size(Duration);
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
			num += 1 + CodedOutputStream.ComputeStringSize(CreatedBy);
		}
		if (MethodMatrixId.Length != 0)
		{
			num += 1 + CodedOutputStream.ComputeStringSize(MethodMatrixId);
		}
		if (_unknownFields != null)
		{
			num += _unknownFields.CalculateSize();
		}
		return num;
	}

	[DebuggerNonUserCode]
	public void MergeFrom(VerifyCreateMethodMatrixResp_1008088 other)
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
			if (other.ConversionRate != 0.0)
			{
				ConversionRate = other.ConversionRate;
			}
			if (other.Type != 0)
			{
				Type = other.Type;
			}
			if (other.UnitConsume != 0)
			{
				UnitConsume = other.UnitConsume;
			}
			if (other.Duration != 0L)
			{
				Duration = other.Duration;
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
			if (other.MethodMatrixId.Length != 0)
			{
				MethodMatrixId = other.MethodMatrixId;
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
				ConversionRate = input.ReadDouble();
				break;
			case 48u:
				Type = input.ReadInt32();
				break;
			case 56u:
				UnitConsume = input.ReadInt32();
				break;
			case 64u:
				Duration = input.ReadInt64();
				break;
			case 72u:
				Stage = input.ReadInt32();
				break;
			case 80u:
				Rank = input.ReadInt32();
				break;
			case 88u:
				FiveElement = input.ReadInt32();
				break;
			case 96u:
				LiveLevel = input.ReadInt32();
				break;
			case 104u:
				LiveProficiency = input.ReadInt32();
				break;
			case 114u:
				CreatedBy = input.ReadString();
				break;
			case 122u:
				MethodMatrixId = input.ReadString();
				break;
			}
		}
	}
}
