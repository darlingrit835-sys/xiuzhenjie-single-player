using System;
using System.Diagnostics;
using Google.Protobuf;
using Google.Protobuf.Collections;
using Google.Protobuf.Reflection;

public sealed class EnterExerciseRoomResp_1005008 : IMessage<EnterExerciseRoomResp_1005008>, IMessage, IEquatable<EnterExerciseRoomResp_1005008>, IDeepCloneable<EnterExerciseRoomResp_1005008>
{
	private static readonly MessageParser<EnterExerciseRoomResp_1005008> _parser = new MessageParser<EnterExerciseRoomResp_1005008>(() => new EnterExerciseRoomResp_1005008());

	private UnknownFieldSet _unknownFields;

	public const int CodeFieldNumber = 1;

	private int code_;

	public const int IsSuccessFieldNumber = 2;

	private bool isSuccess_;

	public const int LocationTypeFieldNumber = 3;

	private int locationType_;

	public const int PhysiqueCircleFieldNumber = 4;

	private double physiqueCircle_;

	public const int PerceptionCircleFieldNumber = 5;

	private double perceptionCircle_;

	public const int SuperairCircleFieldNumber = 6;

	private double superairCircle_;

	public const int PhysiqueNameFieldNumber = 7;

	private string physiqueName_ = "";

	public const int PerceptionNameFieldNumber = 8;

	private string perceptionName_ = "";

	public const int SuperairNameFieldNumber = 9;

	private string superairName_ = "";

	public const int IsBrokenAcupointFieldNumber = 10;

	private bool isBrokenAcupoint_;

	public const int BrokenRateFieldNumber = 11;

	private int brokenRate_;

	public const int IsBrokenBodyFieldNumber = 12;

	private bool isBrokenBody_;

	public const int GatherMatrixRankFieldNumber = 13;

	private int gatherMatrixRank_;

	public const int ControllerMatrixRankFieldNumber = 14;

	private int controllerMatrixRank_;

	public const int GatherMatrixLessTimeFieldNumber = 15;

	private long gatherMatrixLessTime_;

	public const int ControllerMartixLessTimeFieldNumber = 16;

	private long controllerMartixLessTime_;

	public const int RefiningTypeFieldNumber = 17;

	private int refiningType_;

	public const int RefiningValueFieldNumber = 18;

	private long refiningValue_;

	public const int IsBreakFieldNumber = 19;

	private bool isBreak_;

	[DebuggerNonUserCode]
	public static MessageParser<EnterExerciseRoomResp_1005008> Parser => _parser;

	[DebuggerNonUserCode]
	public static MessageDescriptor Descriptor => HomeProtoReflection.Descriptor.MessageTypes[11];

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
	public int LocationType
	{
		get
		{
			return locationType_;
		}
		set
		{
			locationType_ = value;
		}
	}

	[DebuggerNonUserCode]
	public double PhysiqueCircle
	{
		get
		{
			return physiqueCircle_;
		}
		set
		{
			physiqueCircle_ = value;
		}
	}

	[DebuggerNonUserCode]
	public double PerceptionCircle
	{
		get
		{
			return perceptionCircle_;
		}
		set
		{
			perceptionCircle_ = value;
		}
	}

	[DebuggerNonUserCode]
	public double SuperairCircle
	{
		get
		{
			return superairCircle_;
		}
		set
		{
			superairCircle_ = value;
		}
	}

	[DebuggerNonUserCode]
	public string PhysiqueName
	{
		get
		{
			return physiqueName_;
		}
		set
		{
			physiqueName_ = ProtoPreconditions.CheckNotNull(value, "value");
		}
	}

	[DebuggerNonUserCode]
	public string PerceptionName
	{
		get
		{
			return perceptionName_;
		}
		set
		{
			perceptionName_ = ProtoPreconditions.CheckNotNull(value, "value");
		}
	}

	[DebuggerNonUserCode]
	public string SuperairName
	{
		get
		{
			return superairName_;
		}
		set
		{
			superairName_ = ProtoPreconditions.CheckNotNull(value, "value");
		}
	}

	[DebuggerNonUserCode]
	public bool IsBrokenAcupoint
	{
		get
		{
			return isBrokenAcupoint_;
		}
		set
		{
			isBrokenAcupoint_ = value;
		}
	}

	[DebuggerNonUserCode]
	public int BrokenRate
	{
		get
		{
			return brokenRate_;
		}
		set
		{
			brokenRate_ = value;
		}
	}

	[DebuggerNonUserCode]
	public bool IsBrokenBody
	{
		get
		{
			return isBrokenBody_;
		}
		set
		{
			isBrokenBody_ = value;
		}
	}

	[DebuggerNonUserCode]
	public int GatherMatrixRank
	{
		get
		{
			return gatherMatrixRank_;
		}
		set
		{
			gatherMatrixRank_ = value;
		}
	}

	[DebuggerNonUserCode]
	public int ControllerMatrixRank
	{
		get
		{
			return controllerMatrixRank_;
		}
		set
		{
			controllerMatrixRank_ = value;
		}
	}

	[DebuggerNonUserCode]
	public long GatherMatrixLessTime
	{
		get
		{
			return gatherMatrixLessTime_;
		}
		set
		{
			gatherMatrixLessTime_ = value;
		}
	}

	[DebuggerNonUserCode]
	public long ControllerMartixLessTime
	{
		get
		{
			return controllerMartixLessTime_;
		}
		set
		{
			controllerMartixLessTime_ = value;
		}
	}

	[DebuggerNonUserCode]
	public int RefiningType
	{
		get
		{
			return refiningType_;
		}
		set
		{
			refiningType_ = value;
		}
	}

	[DebuggerNonUserCode]
	public long RefiningValue
	{
		get
		{
			return refiningValue_;
		}
		set
		{
			refiningValue_ = value;
		}
	}

	[DebuggerNonUserCode]
	public bool IsBreak
	{
		get
		{
			return isBreak_;
		}
		set
		{
			isBreak_ = value;
		}
	}

	private MessageDescriptor pb_003A_003AGoogle_002EProtobuf_002EIMessage_002Eget_Descriptor()
	{
		return Descriptor;
	}

	[DebuggerNonUserCode]
	public EnterExerciseRoomResp_1005008()
	{
	}

	[DebuggerNonUserCode]
	public EnterExerciseRoomResp_1005008(EnterExerciseRoomResp_1005008 other)
		: this()
	{
		code_ = other.code_;
		isSuccess_ = other.isSuccess_;
		locationType_ = other.locationType_;
		physiqueCircle_ = other.physiqueCircle_;
		perceptionCircle_ = other.perceptionCircle_;
		superairCircle_ = other.superairCircle_;
		physiqueName_ = other.physiqueName_;
		perceptionName_ = other.perceptionName_;
		superairName_ = other.superairName_;
		isBrokenAcupoint_ = other.isBrokenAcupoint_;
		brokenRate_ = other.brokenRate_;
		isBrokenBody_ = other.isBrokenBody_;
		gatherMatrixRank_ = other.gatherMatrixRank_;
		controllerMatrixRank_ = other.controllerMatrixRank_;
		gatherMatrixLessTime_ = other.gatherMatrixLessTime_;
		controllerMartixLessTime_ = other.controllerMartixLessTime_;
		refiningType_ = other.refiningType_;
		refiningValue_ = other.refiningValue_;
		isBreak_ = other.isBreak_;
		_unknownFields = UnknownFieldSet.Clone(other._unknownFields);
	}

	[DebuggerNonUserCode]
	public EnterExerciseRoomResp_1005008 Clone()
	{
		return new EnterExerciseRoomResp_1005008(this);
	}

	[DebuggerNonUserCode]
	public override bool Equals(object other)
	{
		return Equals(other as EnterExerciseRoomResp_1005008);
	}

	[DebuggerNonUserCode]
	public bool Equals(EnterExerciseRoomResp_1005008 other)
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
		if (LocationType != other.LocationType)
		{
			return false;
		}
		if (!ProtobufEqualityComparers.BitwiseDoubleEqualityComparer.Equals(PhysiqueCircle, other.PhysiqueCircle))
		{
			return false;
		}
		if (!ProtobufEqualityComparers.BitwiseDoubleEqualityComparer.Equals(PerceptionCircle, other.PerceptionCircle))
		{
			return false;
		}
		if (!ProtobufEqualityComparers.BitwiseDoubleEqualityComparer.Equals(SuperairCircle, other.SuperairCircle))
		{
			return false;
		}
		if (PhysiqueName != other.PhysiqueName)
		{
			return false;
		}
		if (PerceptionName != other.PerceptionName)
		{
			return false;
		}
		if (SuperairName != other.SuperairName)
		{
			return false;
		}
		if (IsBrokenAcupoint != other.IsBrokenAcupoint)
		{
			return false;
		}
		if (BrokenRate != other.BrokenRate)
		{
			return false;
		}
		if (IsBrokenBody != other.IsBrokenBody)
		{
			return false;
		}
		if (GatherMatrixRank != other.GatherMatrixRank)
		{
			return false;
		}
		if (ControllerMatrixRank != other.ControllerMatrixRank)
		{
			return false;
		}
		if (GatherMatrixLessTime != other.GatherMatrixLessTime)
		{
			return false;
		}
		if (ControllerMartixLessTime != other.ControllerMartixLessTime)
		{
			return false;
		}
		if (RefiningType != other.RefiningType)
		{
			return false;
		}
		if (RefiningValue != other.RefiningValue)
		{
			return false;
		}
		if (IsBreak != other.IsBreak)
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
		if (LocationType != 0)
		{
			num ^= LocationType.GetHashCode();
		}
		if (PhysiqueCircle != 0.0)
		{
			num ^= ProtobufEqualityComparers.BitwiseDoubleEqualityComparer.GetHashCode(PhysiqueCircle);
		}
		if (PerceptionCircle != 0.0)
		{
			num ^= ProtobufEqualityComparers.BitwiseDoubleEqualityComparer.GetHashCode(PerceptionCircle);
		}
		if (SuperairCircle != 0.0)
		{
			num ^= ProtobufEqualityComparers.BitwiseDoubleEqualityComparer.GetHashCode(SuperairCircle);
		}
		if (PhysiqueName.Length != 0)
		{
			num ^= PhysiqueName.GetHashCode();
		}
		if (PerceptionName.Length != 0)
		{
			num ^= PerceptionName.GetHashCode();
		}
		if (SuperairName.Length != 0)
		{
			num ^= SuperairName.GetHashCode();
		}
		if (IsBrokenAcupoint)
		{
			num ^= IsBrokenAcupoint.GetHashCode();
		}
		if (BrokenRate != 0)
		{
			num ^= BrokenRate.GetHashCode();
		}
		if (IsBrokenBody)
		{
			num ^= IsBrokenBody.GetHashCode();
		}
		if (GatherMatrixRank != 0)
		{
			num ^= GatherMatrixRank.GetHashCode();
		}
		if (ControllerMatrixRank != 0)
		{
			num ^= ControllerMatrixRank.GetHashCode();
		}
		if (GatherMatrixLessTime != 0L)
		{
			num ^= GatherMatrixLessTime.GetHashCode();
		}
		if (ControllerMartixLessTime != 0L)
		{
			num ^= ControllerMartixLessTime.GetHashCode();
		}
		if (RefiningType != 0)
		{
			num ^= RefiningType.GetHashCode();
		}
		if (RefiningValue != 0L)
		{
			num ^= RefiningValue.GetHashCode();
		}
		if (IsBreak)
		{
			num ^= IsBreak.GetHashCode();
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
		if (LocationType != 0)
		{
			output.WriteRawTag(24);
			output.WriteInt32(LocationType);
		}
		if (PhysiqueCircle != 0.0)
		{
			output.WriteRawTag(33);
			output.WriteDouble(PhysiqueCircle);
		}
		if (PerceptionCircle != 0.0)
		{
			output.WriteRawTag(41);
			output.WriteDouble(PerceptionCircle);
		}
		if (SuperairCircle != 0.0)
		{
			output.WriteRawTag(49);
			output.WriteDouble(SuperairCircle);
		}
		if (PhysiqueName.Length != 0)
		{
			output.WriteRawTag(58);
			output.WriteString(PhysiqueName);
		}
		if (PerceptionName.Length != 0)
		{
			output.WriteRawTag(66);
			output.WriteString(PerceptionName);
		}
		if (SuperairName.Length != 0)
		{
			output.WriteRawTag(74);
			output.WriteString(SuperairName);
		}
		if (IsBrokenAcupoint)
		{
			output.WriteRawTag(80);
			output.WriteBool(IsBrokenAcupoint);
		}
		if (BrokenRate != 0)
		{
			output.WriteRawTag(88);
			output.WriteInt32(BrokenRate);
		}
		if (IsBrokenBody)
		{
			output.WriteRawTag(96);
			output.WriteBool(IsBrokenBody);
		}
		if (GatherMatrixRank != 0)
		{
			output.WriteRawTag(104);
			output.WriteInt32(GatherMatrixRank);
		}
		if (ControllerMatrixRank != 0)
		{
			output.WriteRawTag(112);
			output.WriteInt32(ControllerMatrixRank);
		}
		if (GatherMatrixLessTime != 0L)
		{
			output.WriteRawTag(120);
			output.WriteInt64(GatherMatrixLessTime);
		}
		if (ControllerMartixLessTime != 0L)
		{
			output.WriteRawTag(128, 1);
			output.WriteInt64(ControllerMartixLessTime);
		}
		if (RefiningType != 0)
		{
			output.WriteRawTag(136, 1);
			output.WriteInt32(RefiningType);
		}
		if (RefiningValue != 0L)
		{
			output.WriteRawTag(144, 1);
			output.WriteInt64(RefiningValue);
		}
		if (IsBreak)
		{
			output.WriteRawTag(152, 1);
			output.WriteBool(IsBreak);
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
		if (LocationType != 0)
		{
			num += 1 + CodedOutputStream.ComputeInt32Size(LocationType);
		}
		if (PhysiqueCircle != 0.0)
		{
			num += 9;
		}
		if (PerceptionCircle != 0.0)
		{
			num += 9;
		}
		if (SuperairCircle != 0.0)
		{
			num += 9;
		}
		if (PhysiqueName.Length != 0)
		{
			num += 1 + CodedOutputStream.ComputeStringSize(PhysiqueName);
		}
		if (PerceptionName.Length != 0)
		{
			num += 1 + CodedOutputStream.ComputeStringSize(PerceptionName);
		}
		if (SuperairName.Length != 0)
		{
			num += 1 + CodedOutputStream.ComputeStringSize(SuperairName);
		}
		if (IsBrokenAcupoint)
		{
			num += 2;
		}
		if (BrokenRate != 0)
		{
			num += 1 + CodedOutputStream.ComputeInt32Size(BrokenRate);
		}
		if (IsBrokenBody)
		{
			num += 2;
		}
		if (GatherMatrixRank != 0)
		{
			num += 1 + CodedOutputStream.ComputeInt32Size(GatherMatrixRank);
		}
		if (ControllerMatrixRank != 0)
		{
			num += 1 + CodedOutputStream.ComputeInt32Size(ControllerMatrixRank);
		}
		if (GatherMatrixLessTime != 0L)
		{
			num += 1 + CodedOutputStream.ComputeInt64Size(GatherMatrixLessTime);
		}
		if (ControllerMartixLessTime != 0L)
		{
			num += 2 + CodedOutputStream.ComputeInt64Size(ControllerMartixLessTime);
		}
		if (RefiningType != 0)
		{
			num += 2 + CodedOutputStream.ComputeInt32Size(RefiningType);
		}
		if (RefiningValue != 0L)
		{
			num += 2 + CodedOutputStream.ComputeInt64Size(RefiningValue);
		}
		if (IsBreak)
		{
			num += 3;
		}
		if (_unknownFields != null)
		{
			num += _unknownFields.CalculateSize();
		}
		return num;
	}

	[DebuggerNonUserCode]
	public void MergeFrom(EnterExerciseRoomResp_1005008 other)
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
			if (other.LocationType != 0)
			{
				LocationType = other.LocationType;
			}
			if (other.PhysiqueCircle != 0.0)
			{
				PhysiqueCircle = other.PhysiqueCircle;
			}
			if (other.PerceptionCircle != 0.0)
			{
				PerceptionCircle = other.PerceptionCircle;
			}
			if (other.SuperairCircle != 0.0)
			{
				SuperairCircle = other.SuperairCircle;
			}
			if (other.PhysiqueName.Length != 0)
			{
				PhysiqueName = other.PhysiqueName;
			}
			if (other.PerceptionName.Length != 0)
			{
				PerceptionName = other.PerceptionName;
			}
			if (other.SuperairName.Length != 0)
			{
				SuperairName = other.SuperairName;
			}
			if (other.IsBrokenAcupoint)
			{
				IsBrokenAcupoint = other.IsBrokenAcupoint;
			}
			if (other.BrokenRate != 0)
			{
				BrokenRate = other.BrokenRate;
			}
			if (other.IsBrokenBody)
			{
				IsBrokenBody = other.IsBrokenBody;
			}
			if (other.GatherMatrixRank != 0)
			{
				GatherMatrixRank = other.GatherMatrixRank;
			}
			if (other.ControllerMatrixRank != 0)
			{
				ControllerMatrixRank = other.ControllerMatrixRank;
			}
			if (other.GatherMatrixLessTime != 0L)
			{
				GatherMatrixLessTime = other.GatherMatrixLessTime;
			}
			if (other.ControllerMartixLessTime != 0L)
			{
				ControllerMartixLessTime = other.ControllerMartixLessTime;
			}
			if (other.RefiningType != 0)
			{
				RefiningType = other.RefiningType;
			}
			if (other.RefiningValue != 0L)
			{
				RefiningValue = other.RefiningValue;
			}
			if (other.IsBreak)
			{
				IsBreak = other.IsBreak;
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
				LocationType = input.ReadInt32();
				break;
			case 33u:
				PhysiqueCircle = input.ReadDouble();
				break;
			case 41u:
				PerceptionCircle = input.ReadDouble();
				break;
			case 49u:
				SuperairCircle = input.ReadDouble();
				break;
			case 58u:
				PhysiqueName = input.ReadString();
				break;
			case 66u:
				PerceptionName = input.ReadString();
				break;
			case 74u:
				SuperairName = input.ReadString();
				break;
			case 80u:
				IsBrokenAcupoint = input.ReadBool();
				break;
			case 88u:
				BrokenRate = input.ReadInt32();
				break;
			case 96u:
				IsBrokenBody = input.ReadBool();
				break;
			case 104u:
				GatherMatrixRank = input.ReadInt32();
				break;
			case 112u:
				ControllerMatrixRank = input.ReadInt32();
				break;
			case 120u:
				GatherMatrixLessTime = input.ReadInt64();
				break;
			case 128u:
				ControllerMartixLessTime = input.ReadInt64();
				break;
			case 136u:
				RefiningType = input.ReadInt32();
				break;
			case 144u:
				RefiningValue = input.ReadInt64();
				break;
			case 152u:
				IsBreak = input.ReadBool();
				break;
			}
		}
	}
}
