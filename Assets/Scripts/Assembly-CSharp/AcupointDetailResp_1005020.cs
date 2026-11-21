using System;
using System.Diagnostics;
using Google.Protobuf;
using Google.Protobuf.Collections;
using Google.Protobuf.Reflection;

public sealed class AcupointDetailResp_1005020 : IMessage<AcupointDetailResp_1005020>, IMessage, IEquatable<AcupointDetailResp_1005020>, IDeepCloneable<AcupointDetailResp_1005020>
{
	private static readonly MessageParser<AcupointDetailResp_1005020> _parser = new MessageParser<AcupointDetailResp_1005020>(() => new AcupointDetailResp_1005020());

	private UnknownFieldSet _unknownFields;

	public const int CodeFieldNumber = 1;

	private int code_;

	public const int IsSuccessFieldNumber = 2;

	private bool isSuccess_;

	public const int AirValueUpFieldNumber = 3;

	private double airValueUp_;

	public const int AirSpeedUpFieldNumber = 4;

	private double airSpeedUp_;

	public const int FiveElementAttackUpFieldNumber = 5;

	private double fiveElementAttackUp_;

	public const int FiveElementDefenseUpFieldNumber = 6;

	private double fiveElementDefenseUp_;

	public const int PhysiqueValueUpFieldNumber = 7;

	private double physiqueValueUp_;

	public const int PerceptionValueUpFieldNumber = 8;

	private double perceptionValueUp_;

	public const int PureBloodValueUpFieldNumber = 9;

	private double pureBloodValueUp_;

	public const int PureBloodRecoveryUpFieldNumber = 10;

	private double pureBloodRecoveryUp_;

	public const int ThreadType1CountFieldNumber = 11;

	private int threadType1Count_;

	public const int ThreadType2CountFieldNumber = 12;

	private int threadType2Count_;

	public const int ThreadType3CountFieldNumber = 13;

	private int threadType3Count_;

	public const int ThreadType4CountFieldNumber = 14;

	private int threadType4Count_;

	public const int ThreadType5CountFieldNumber = 15;

	private int threadType5Count_;

	public const int ThreadType6CountFieldNumber = 16;

	private int threadType6Count_;

	public const int ThreadType7CountFieldNumber = 17;

	private int threadType7Count_;

	public const int ThreadType8CountFieldNumber = 18;

	private int threadType8Count_;

	public const int ThreadType1LevelFieldNumber = 19;

	private int threadType1Level_;

	public const int ThreadType2LevelFieldNumber = 20;

	private int threadType2Level_;

	public const int ThreadType3LevelFieldNumber = 21;

	private int threadType3Level_;

	public const int ThreadType4LevelFieldNumber = 22;

	private int threadType4Level_;

	public const int ThreadType5LevelFieldNumber = 23;

	private int threadType5Level_;

	public const int ThreadType6LevelFieldNumber = 24;

	private int threadType6Level_;

	public const int ThreadType7LevelFieldNumber = 25;

	private int threadType7Level_;

	public const int ThreadType8LevelFieldNumber = 26;

	private int threadType8Level_;

	[DebuggerNonUserCode]
	public static MessageParser<AcupointDetailResp_1005020> Parser => _parser;

	[DebuggerNonUserCode]
	public static MessageDescriptor Descriptor => HomeProtoReflection.Descriptor.MessageTypes[23];

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
	public double AirValueUp
	{
		get
		{
			return airValueUp_;
		}
		set
		{
			airValueUp_ = value;
		}
	}

	[DebuggerNonUserCode]
	public double AirSpeedUp
	{
		get
		{
			return airSpeedUp_;
		}
		set
		{
			airSpeedUp_ = value;
		}
	}

	[DebuggerNonUserCode]
	public double FiveElementAttackUp
	{
		get
		{
			return fiveElementAttackUp_;
		}
		set
		{
			fiveElementAttackUp_ = value;
		}
	}

	[DebuggerNonUserCode]
	public double FiveElementDefenseUp
	{
		get
		{
			return fiveElementDefenseUp_;
		}
		set
		{
			fiveElementDefenseUp_ = value;
		}
	}

	[DebuggerNonUserCode]
	public double PhysiqueValueUp
	{
		get
		{
			return physiqueValueUp_;
		}
		set
		{
			physiqueValueUp_ = value;
		}
	}

	[DebuggerNonUserCode]
	public double PerceptionValueUp
	{
		get
		{
			return perceptionValueUp_;
		}
		set
		{
			perceptionValueUp_ = value;
		}
	}

	[DebuggerNonUserCode]
	public double PureBloodValueUp
	{
		get
		{
			return pureBloodValueUp_;
		}
		set
		{
			pureBloodValueUp_ = value;
		}
	}

	[DebuggerNonUserCode]
	public double PureBloodRecoveryUp
	{
		get
		{
			return pureBloodRecoveryUp_;
		}
		set
		{
			pureBloodRecoveryUp_ = value;
		}
	}

	[DebuggerNonUserCode]
	public int ThreadType1Count
	{
		get
		{
			return threadType1Count_;
		}
		set
		{
			threadType1Count_ = value;
		}
	}

	[DebuggerNonUserCode]
	public int ThreadType2Count
	{
		get
		{
			return threadType2Count_;
		}
		set
		{
			threadType2Count_ = value;
		}
	}

	[DebuggerNonUserCode]
	public int ThreadType3Count
	{
		get
		{
			return threadType3Count_;
		}
		set
		{
			threadType3Count_ = value;
		}
	}

	[DebuggerNonUserCode]
	public int ThreadType4Count
	{
		get
		{
			return threadType4Count_;
		}
		set
		{
			threadType4Count_ = value;
		}
	}

	[DebuggerNonUserCode]
	public int ThreadType5Count
	{
		get
		{
			return threadType5Count_;
		}
		set
		{
			threadType5Count_ = value;
		}
	}

	[DebuggerNonUserCode]
	public int ThreadType6Count
	{
		get
		{
			return threadType6Count_;
		}
		set
		{
			threadType6Count_ = value;
		}
	}

	[DebuggerNonUserCode]
	public int ThreadType7Count
	{
		get
		{
			return threadType7Count_;
		}
		set
		{
			threadType7Count_ = value;
		}
	}

	[DebuggerNonUserCode]
	public int ThreadType8Count
	{
		get
		{
			return threadType8Count_;
		}
		set
		{
			threadType8Count_ = value;
		}
	}

	[DebuggerNonUserCode]
	public int ThreadType1Level
	{
		get
		{
			return threadType1Level_;
		}
		set
		{
			threadType1Level_ = value;
		}
	}

	[DebuggerNonUserCode]
	public int ThreadType2Level
	{
		get
		{
			return threadType2Level_;
		}
		set
		{
			threadType2Level_ = value;
		}
	}

	[DebuggerNonUserCode]
	public int ThreadType3Level
	{
		get
		{
			return threadType3Level_;
		}
		set
		{
			threadType3Level_ = value;
		}
	}

	[DebuggerNonUserCode]
	public int ThreadType4Level
	{
		get
		{
			return threadType4Level_;
		}
		set
		{
			threadType4Level_ = value;
		}
	}

	[DebuggerNonUserCode]
	public int ThreadType5Level
	{
		get
		{
			return threadType5Level_;
		}
		set
		{
			threadType5Level_ = value;
		}
	}

	[DebuggerNonUserCode]
	public int ThreadType6Level
	{
		get
		{
			return threadType6Level_;
		}
		set
		{
			threadType6Level_ = value;
		}
	}

	[DebuggerNonUserCode]
	public int ThreadType7Level
	{
		get
		{
			return threadType7Level_;
		}
		set
		{
			threadType7Level_ = value;
		}
	}

	[DebuggerNonUserCode]
	public int ThreadType8Level
	{
		get
		{
			return threadType8Level_;
		}
		set
		{
			threadType8Level_ = value;
		}
	}

	private MessageDescriptor pb_003A_003AGoogle_002EProtobuf_002EIMessage_002Eget_Descriptor()
	{
		return Descriptor;
	}

	[DebuggerNonUserCode]
	public AcupointDetailResp_1005020()
	{
	}

	[DebuggerNonUserCode]
	public AcupointDetailResp_1005020(AcupointDetailResp_1005020 other)
		: this()
	{
		code_ = other.code_;
		isSuccess_ = other.isSuccess_;
		airValueUp_ = other.airValueUp_;
		airSpeedUp_ = other.airSpeedUp_;
		fiveElementAttackUp_ = other.fiveElementAttackUp_;
		fiveElementDefenseUp_ = other.fiveElementDefenseUp_;
		physiqueValueUp_ = other.physiqueValueUp_;
		perceptionValueUp_ = other.perceptionValueUp_;
		pureBloodValueUp_ = other.pureBloodValueUp_;
		pureBloodRecoveryUp_ = other.pureBloodRecoveryUp_;
		threadType1Count_ = other.threadType1Count_;
		threadType2Count_ = other.threadType2Count_;
		threadType3Count_ = other.threadType3Count_;
		threadType4Count_ = other.threadType4Count_;
		threadType5Count_ = other.threadType5Count_;
		threadType6Count_ = other.threadType6Count_;
		threadType7Count_ = other.threadType7Count_;
		threadType8Count_ = other.threadType8Count_;
		threadType1Level_ = other.threadType1Level_;
		threadType2Level_ = other.threadType2Level_;
		threadType3Level_ = other.threadType3Level_;
		threadType4Level_ = other.threadType4Level_;
		threadType5Level_ = other.threadType5Level_;
		threadType6Level_ = other.threadType6Level_;
		threadType7Level_ = other.threadType7Level_;
		threadType8Level_ = other.threadType8Level_;
		_unknownFields = UnknownFieldSet.Clone(other._unknownFields);
	}

	[DebuggerNonUserCode]
	public AcupointDetailResp_1005020 Clone()
	{
		return new AcupointDetailResp_1005020(this);
	}

	[DebuggerNonUserCode]
	public override bool Equals(object other)
	{
		return Equals(other as AcupointDetailResp_1005020);
	}

	[DebuggerNonUserCode]
	public bool Equals(AcupointDetailResp_1005020 other)
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
		if (!ProtobufEqualityComparers.BitwiseDoubleEqualityComparer.Equals(AirValueUp, other.AirValueUp))
		{
			return false;
		}
		if (!ProtobufEqualityComparers.BitwiseDoubleEqualityComparer.Equals(AirSpeedUp, other.AirSpeedUp))
		{
			return false;
		}
		if (!ProtobufEqualityComparers.BitwiseDoubleEqualityComparer.Equals(FiveElementAttackUp, other.FiveElementAttackUp))
		{
			return false;
		}
		if (!ProtobufEqualityComparers.BitwiseDoubleEqualityComparer.Equals(FiveElementDefenseUp, other.FiveElementDefenseUp))
		{
			return false;
		}
		if (!ProtobufEqualityComparers.BitwiseDoubleEqualityComparer.Equals(PhysiqueValueUp, other.PhysiqueValueUp))
		{
			return false;
		}
		if (!ProtobufEqualityComparers.BitwiseDoubleEqualityComparer.Equals(PerceptionValueUp, other.PerceptionValueUp))
		{
			return false;
		}
		if (!ProtobufEqualityComparers.BitwiseDoubleEqualityComparer.Equals(PureBloodValueUp, other.PureBloodValueUp))
		{
			return false;
		}
		if (!ProtobufEqualityComparers.BitwiseDoubleEqualityComparer.Equals(PureBloodRecoveryUp, other.PureBloodRecoveryUp))
		{
			return false;
		}
		if (ThreadType1Count != other.ThreadType1Count)
		{
			return false;
		}
		if (ThreadType2Count != other.ThreadType2Count)
		{
			return false;
		}
		if (ThreadType3Count != other.ThreadType3Count)
		{
			return false;
		}
		if (ThreadType4Count != other.ThreadType4Count)
		{
			return false;
		}
		if (ThreadType5Count != other.ThreadType5Count)
		{
			return false;
		}
		if (ThreadType6Count != other.ThreadType6Count)
		{
			return false;
		}
		if (ThreadType7Count != other.ThreadType7Count)
		{
			return false;
		}
		if (ThreadType8Count != other.ThreadType8Count)
		{
			return false;
		}
		if (ThreadType1Level != other.ThreadType1Level)
		{
			return false;
		}
		if (ThreadType2Level != other.ThreadType2Level)
		{
			return false;
		}
		if (ThreadType3Level != other.ThreadType3Level)
		{
			return false;
		}
		if (ThreadType4Level != other.ThreadType4Level)
		{
			return false;
		}
		if (ThreadType5Level != other.ThreadType5Level)
		{
			return false;
		}
		if (ThreadType6Level != other.ThreadType6Level)
		{
			return false;
		}
		if (ThreadType7Level != other.ThreadType7Level)
		{
			return false;
		}
		if (ThreadType8Level != other.ThreadType8Level)
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
		if (AirValueUp != 0.0)
		{
			num ^= ProtobufEqualityComparers.BitwiseDoubleEqualityComparer.GetHashCode(AirValueUp);
		}
		if (AirSpeedUp != 0.0)
		{
			num ^= ProtobufEqualityComparers.BitwiseDoubleEqualityComparer.GetHashCode(AirSpeedUp);
		}
		if (FiveElementAttackUp != 0.0)
		{
			num ^= ProtobufEqualityComparers.BitwiseDoubleEqualityComparer.GetHashCode(FiveElementAttackUp);
		}
		if (FiveElementDefenseUp != 0.0)
		{
			num ^= ProtobufEqualityComparers.BitwiseDoubleEqualityComparer.GetHashCode(FiveElementDefenseUp);
		}
		if (PhysiqueValueUp != 0.0)
		{
			num ^= ProtobufEqualityComparers.BitwiseDoubleEqualityComparer.GetHashCode(PhysiqueValueUp);
		}
		if (PerceptionValueUp != 0.0)
		{
			num ^= ProtobufEqualityComparers.BitwiseDoubleEqualityComparer.GetHashCode(PerceptionValueUp);
		}
		if (PureBloodValueUp != 0.0)
		{
			num ^= ProtobufEqualityComparers.BitwiseDoubleEqualityComparer.GetHashCode(PureBloodValueUp);
		}
		if (PureBloodRecoveryUp != 0.0)
		{
			num ^= ProtobufEqualityComparers.BitwiseDoubleEqualityComparer.GetHashCode(PureBloodRecoveryUp);
		}
		if (ThreadType1Count != 0)
		{
			num ^= ThreadType1Count.GetHashCode();
		}
		if (ThreadType2Count != 0)
		{
			num ^= ThreadType2Count.GetHashCode();
		}
		if (ThreadType3Count != 0)
		{
			num ^= ThreadType3Count.GetHashCode();
		}
		if (ThreadType4Count != 0)
		{
			num ^= ThreadType4Count.GetHashCode();
		}
		if (ThreadType5Count != 0)
		{
			num ^= ThreadType5Count.GetHashCode();
		}
		if (ThreadType6Count != 0)
		{
			num ^= ThreadType6Count.GetHashCode();
		}
		if (ThreadType7Count != 0)
		{
			num ^= ThreadType7Count.GetHashCode();
		}
		if (ThreadType8Count != 0)
		{
			num ^= ThreadType8Count.GetHashCode();
		}
		if (ThreadType1Level != 0)
		{
			num ^= ThreadType1Level.GetHashCode();
		}
		if (ThreadType2Level != 0)
		{
			num ^= ThreadType2Level.GetHashCode();
		}
		if (ThreadType3Level != 0)
		{
			num ^= ThreadType3Level.GetHashCode();
		}
		if (ThreadType4Level != 0)
		{
			num ^= ThreadType4Level.GetHashCode();
		}
		if (ThreadType5Level != 0)
		{
			num ^= ThreadType5Level.GetHashCode();
		}
		if (ThreadType6Level != 0)
		{
			num ^= ThreadType6Level.GetHashCode();
		}
		if (ThreadType7Level != 0)
		{
			num ^= ThreadType7Level.GetHashCode();
		}
		if (ThreadType8Level != 0)
		{
			num ^= ThreadType8Level.GetHashCode();
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
		if (AirValueUp != 0.0)
		{
			output.WriteRawTag(25);
			output.WriteDouble(AirValueUp);
		}
		if (AirSpeedUp != 0.0)
		{
			output.WriteRawTag(33);
			output.WriteDouble(AirSpeedUp);
		}
		if (FiveElementAttackUp != 0.0)
		{
			output.WriteRawTag(41);
			output.WriteDouble(FiveElementAttackUp);
		}
		if (FiveElementDefenseUp != 0.0)
		{
			output.WriteRawTag(49);
			output.WriteDouble(FiveElementDefenseUp);
		}
		if (PhysiqueValueUp != 0.0)
		{
			output.WriteRawTag(57);
			output.WriteDouble(PhysiqueValueUp);
		}
		if (PerceptionValueUp != 0.0)
		{
			output.WriteRawTag(65);
			output.WriteDouble(PerceptionValueUp);
		}
		if (PureBloodValueUp != 0.0)
		{
			output.WriteRawTag(73);
			output.WriteDouble(PureBloodValueUp);
		}
		if (PureBloodRecoveryUp != 0.0)
		{
			output.WriteRawTag(81);
			output.WriteDouble(PureBloodRecoveryUp);
		}
		if (ThreadType1Count != 0)
		{
			output.WriteRawTag(88);
			output.WriteInt32(ThreadType1Count);
		}
		if (ThreadType2Count != 0)
		{
			output.WriteRawTag(96);
			output.WriteInt32(ThreadType2Count);
		}
		if (ThreadType3Count != 0)
		{
			output.WriteRawTag(104);
			output.WriteInt32(ThreadType3Count);
		}
		if (ThreadType4Count != 0)
		{
			output.WriteRawTag(112);
			output.WriteInt32(ThreadType4Count);
		}
		if (ThreadType5Count != 0)
		{
			output.WriteRawTag(120);
			output.WriteInt32(ThreadType5Count);
		}
		if (ThreadType6Count != 0)
		{
			output.WriteRawTag(128, 1);
			output.WriteInt32(ThreadType6Count);
		}
		if (ThreadType7Count != 0)
		{
			output.WriteRawTag(136, 1);
			output.WriteInt32(ThreadType7Count);
		}
		if (ThreadType8Count != 0)
		{
			output.WriteRawTag(144, 1);
			output.WriteInt32(ThreadType8Count);
		}
		if (ThreadType1Level != 0)
		{
			output.WriteRawTag(152, 1);
			output.WriteInt32(ThreadType1Level);
		}
		if (ThreadType2Level != 0)
		{
			output.WriteRawTag(160, 1);
			output.WriteInt32(ThreadType2Level);
		}
		if (ThreadType3Level != 0)
		{
			output.WriteRawTag(168, 1);
			output.WriteInt32(ThreadType3Level);
		}
		if (ThreadType4Level != 0)
		{
			output.WriteRawTag(176, 1);
			output.WriteInt32(ThreadType4Level);
		}
		if (ThreadType5Level != 0)
		{
			output.WriteRawTag(184, 1);
			output.WriteInt32(ThreadType5Level);
		}
		if (ThreadType6Level != 0)
		{
			output.WriteRawTag(192, 1);
			output.WriteInt32(ThreadType6Level);
		}
		if (ThreadType7Level != 0)
		{
			output.WriteRawTag(200, 1);
			output.WriteInt32(ThreadType7Level);
		}
		if (ThreadType8Level != 0)
		{
			output.WriteRawTag(208, 1);
			output.WriteInt32(ThreadType8Level);
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
		if (AirValueUp != 0.0)
		{
			num += 9;
		}
		if (AirSpeedUp != 0.0)
		{
			num += 9;
		}
		if (FiveElementAttackUp != 0.0)
		{
			num += 9;
		}
		if (FiveElementDefenseUp != 0.0)
		{
			num += 9;
		}
		if (PhysiqueValueUp != 0.0)
		{
			num += 9;
		}
		if (PerceptionValueUp != 0.0)
		{
			num += 9;
		}
		if (PureBloodValueUp != 0.0)
		{
			num += 9;
		}
		if (PureBloodRecoveryUp != 0.0)
		{
			num += 9;
		}
		if (ThreadType1Count != 0)
		{
			num += 1 + CodedOutputStream.ComputeInt32Size(ThreadType1Count);
		}
		if (ThreadType2Count != 0)
		{
			num += 1 + CodedOutputStream.ComputeInt32Size(ThreadType2Count);
		}
		if (ThreadType3Count != 0)
		{
			num += 1 + CodedOutputStream.ComputeInt32Size(ThreadType3Count);
		}
		if (ThreadType4Count != 0)
		{
			num += 1 + CodedOutputStream.ComputeInt32Size(ThreadType4Count);
		}
		if (ThreadType5Count != 0)
		{
			num += 1 + CodedOutputStream.ComputeInt32Size(ThreadType5Count);
		}
		if (ThreadType6Count != 0)
		{
			num += 2 + CodedOutputStream.ComputeInt32Size(ThreadType6Count);
		}
		if (ThreadType7Count != 0)
		{
			num += 2 + CodedOutputStream.ComputeInt32Size(ThreadType7Count);
		}
		if (ThreadType8Count != 0)
		{
			num += 2 + CodedOutputStream.ComputeInt32Size(ThreadType8Count);
		}
		if (ThreadType1Level != 0)
		{
			num += 2 + CodedOutputStream.ComputeInt32Size(ThreadType1Level);
		}
		if (ThreadType2Level != 0)
		{
			num += 2 + CodedOutputStream.ComputeInt32Size(ThreadType2Level);
		}
		if (ThreadType3Level != 0)
		{
			num += 2 + CodedOutputStream.ComputeInt32Size(ThreadType3Level);
		}
		if (ThreadType4Level != 0)
		{
			num += 2 + CodedOutputStream.ComputeInt32Size(ThreadType4Level);
		}
		if (ThreadType5Level != 0)
		{
			num += 2 + CodedOutputStream.ComputeInt32Size(ThreadType5Level);
		}
		if (ThreadType6Level != 0)
		{
			num += 2 + CodedOutputStream.ComputeInt32Size(ThreadType6Level);
		}
		if (ThreadType7Level != 0)
		{
			num += 2 + CodedOutputStream.ComputeInt32Size(ThreadType7Level);
		}
		if (ThreadType8Level != 0)
		{
			num += 2 + CodedOutputStream.ComputeInt32Size(ThreadType8Level);
		}
		if (_unknownFields != null)
		{
			num += _unknownFields.CalculateSize();
		}
		return num;
	}

	[DebuggerNonUserCode]
	public void MergeFrom(AcupointDetailResp_1005020 other)
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
			if (other.AirValueUp != 0.0)
			{
				AirValueUp = other.AirValueUp;
			}
			if (other.AirSpeedUp != 0.0)
			{
				AirSpeedUp = other.AirSpeedUp;
			}
			if (other.FiveElementAttackUp != 0.0)
			{
				FiveElementAttackUp = other.FiveElementAttackUp;
			}
			if (other.FiveElementDefenseUp != 0.0)
			{
				FiveElementDefenseUp = other.FiveElementDefenseUp;
			}
			if (other.PhysiqueValueUp != 0.0)
			{
				PhysiqueValueUp = other.PhysiqueValueUp;
			}
			if (other.PerceptionValueUp != 0.0)
			{
				PerceptionValueUp = other.PerceptionValueUp;
			}
			if (other.PureBloodValueUp != 0.0)
			{
				PureBloodValueUp = other.PureBloodValueUp;
			}
			if (other.PureBloodRecoveryUp != 0.0)
			{
				PureBloodRecoveryUp = other.PureBloodRecoveryUp;
			}
			if (other.ThreadType1Count != 0)
			{
				ThreadType1Count = other.ThreadType1Count;
			}
			if (other.ThreadType2Count != 0)
			{
				ThreadType2Count = other.ThreadType2Count;
			}
			if (other.ThreadType3Count != 0)
			{
				ThreadType3Count = other.ThreadType3Count;
			}
			if (other.ThreadType4Count != 0)
			{
				ThreadType4Count = other.ThreadType4Count;
			}
			if (other.ThreadType5Count != 0)
			{
				ThreadType5Count = other.ThreadType5Count;
			}
			if (other.ThreadType6Count != 0)
			{
				ThreadType6Count = other.ThreadType6Count;
			}
			if (other.ThreadType7Count != 0)
			{
				ThreadType7Count = other.ThreadType7Count;
			}
			if (other.ThreadType8Count != 0)
			{
				ThreadType8Count = other.ThreadType8Count;
			}
			if (other.ThreadType1Level != 0)
			{
				ThreadType1Level = other.ThreadType1Level;
			}
			if (other.ThreadType2Level != 0)
			{
				ThreadType2Level = other.ThreadType2Level;
			}
			if (other.ThreadType3Level != 0)
			{
				ThreadType3Level = other.ThreadType3Level;
			}
			if (other.ThreadType4Level != 0)
			{
				ThreadType4Level = other.ThreadType4Level;
			}
			if (other.ThreadType5Level != 0)
			{
				ThreadType5Level = other.ThreadType5Level;
			}
			if (other.ThreadType6Level != 0)
			{
				ThreadType6Level = other.ThreadType6Level;
			}
			if (other.ThreadType7Level != 0)
			{
				ThreadType7Level = other.ThreadType7Level;
			}
			if (other.ThreadType8Level != 0)
			{
				ThreadType8Level = other.ThreadType8Level;
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
			case 25u:
				AirValueUp = input.ReadDouble();
				break;
			case 33u:
				AirSpeedUp = input.ReadDouble();
				break;
			case 41u:
				FiveElementAttackUp = input.ReadDouble();
				break;
			case 49u:
				FiveElementDefenseUp = input.ReadDouble();
				break;
			case 57u:
				PhysiqueValueUp = input.ReadDouble();
				break;
			case 65u:
				PerceptionValueUp = input.ReadDouble();
				break;
			case 73u:
				PureBloodValueUp = input.ReadDouble();
				break;
			case 81u:
				PureBloodRecoveryUp = input.ReadDouble();
				break;
			case 88u:
				ThreadType1Count = input.ReadInt32();
				break;
			case 96u:
				ThreadType2Count = input.ReadInt32();
				break;
			case 104u:
				ThreadType3Count = input.ReadInt32();
				break;
			case 112u:
				ThreadType4Count = input.ReadInt32();
				break;
			case 120u:
				ThreadType5Count = input.ReadInt32();
				break;
			case 128u:
				ThreadType6Count = input.ReadInt32();
				break;
			case 136u:
				ThreadType7Count = input.ReadInt32();
				break;
			case 144u:
				ThreadType8Count = input.ReadInt32();
				break;
			case 152u:
				ThreadType1Level = input.ReadInt32();
				break;
			case 160u:
				ThreadType2Level = input.ReadInt32();
				break;
			case 168u:
				ThreadType3Level = input.ReadInt32();
				break;
			case 176u:
				ThreadType4Level = input.ReadInt32();
				break;
			case 184u:
				ThreadType5Level = input.ReadInt32();
				break;
			case 192u:
				ThreadType6Level = input.ReadInt32();
				break;
			case 200u:
				ThreadType7Level = input.ReadInt32();
				break;
			case 208u:
				ThreadType8Level = input.ReadInt32();
				break;
			}
		}
	}
}
