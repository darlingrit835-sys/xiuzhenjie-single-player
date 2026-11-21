using System;
using System.Diagnostics;
using Google.Protobuf;
using Google.Protobuf.Reflection;

public sealed class GetShiKongMiJingResp_1002032 : IMessage<GetShiKongMiJingResp_1002032>, IMessage, IEquatable<GetShiKongMiJingResp_1002032>, IDeepCloneable<GetShiKongMiJingResp_1002032>
{
	private static readonly MessageParser<GetShiKongMiJingResp_1002032> _parser = new MessageParser<GetShiKongMiJingResp_1002032>(() => new GetShiKongMiJingResp_1002032());

	private UnknownFieldSet _unknownFields;

	public const int CodeFieldNumber = 1;

	private int code_;

	public const int IsSuccessFieldNumber = 2;

	private bool isSuccess_;

	public const int SuperAirFieldNumber = 3;

	private long superAir_;

	public const int CurrentSuperAirFieldNumber = 4;

	private long currentSuperAir_;

	public const int PerceptionFieldNumber = 5;

	private int perception_;

	public const int CurrentPerceptionFieldNumber = 6;

	private int currentPerception_;

	public const int PhysiqueFieldNumber = 7;

	private int physique_;

	public const int CurrentPhysiqueFieldNumber = 8;

	private int currentPhysique_;

	public const int AgeFieldNumber = 9;

	private int age_;

	public const int PureBloodFieldNumber = 10;

	private int pureBlood_;

	public const int CurrentPureBloodFieldNumber = 11;

	private int currentPureBlood_;

	public const int RefiningSuperAirFieldNumber = 12;

	private long refiningSuperAir_;

	public const int RefiningPerceptionFieldNumber = 13;

	private int refiningPerception_;

	public const int RefiningtPhysiqueFieldNumber = 14;

	private int refiningtPhysique_;

	[DebuggerNonUserCode]
	public static MessageParser<GetShiKongMiJingResp_1002032> Parser => _parser;

	[DebuggerNonUserCode]
	public static MessageDescriptor Descriptor => RoleProtoReflection.Descriptor.MessageTypes[39];

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
	public long SuperAir
	{
		get
		{
			return superAir_;
		}
		set
		{
			superAir_ = value;
		}
	}

	[DebuggerNonUserCode]
	public long CurrentSuperAir
	{
		get
		{
			return currentSuperAir_;
		}
		set
		{
			currentSuperAir_ = value;
		}
	}

	[DebuggerNonUserCode]
	public int Perception
	{
		get
		{
			return perception_;
		}
		set
		{
			perception_ = value;
		}
	}

	[DebuggerNonUserCode]
	public int CurrentPerception
	{
		get
		{
			return currentPerception_;
		}
		set
		{
			currentPerception_ = value;
		}
	}

	[DebuggerNonUserCode]
	public int Physique
	{
		get
		{
			return physique_;
		}
		set
		{
			physique_ = value;
		}
	}

	[DebuggerNonUserCode]
	public int CurrentPhysique
	{
		get
		{
			return currentPhysique_;
		}
		set
		{
			currentPhysique_ = value;
		}
	}

	[DebuggerNonUserCode]
	public int Age
	{
		get
		{
			return age_;
		}
		set
		{
			age_ = value;
		}
	}

	[DebuggerNonUserCode]
	public int PureBlood
	{
		get
		{
			return pureBlood_;
		}
		set
		{
			pureBlood_ = value;
		}
	}

	[DebuggerNonUserCode]
	public int CurrentPureBlood
	{
		get
		{
			return currentPureBlood_;
		}
		set
		{
			currentPureBlood_ = value;
		}
	}

	[DebuggerNonUserCode]
	public long RefiningSuperAir
	{
		get
		{
			return refiningSuperAir_;
		}
		set
		{
			refiningSuperAir_ = value;
		}
	}

	[DebuggerNonUserCode]
	public int RefiningPerception
	{
		get
		{
			return refiningPerception_;
		}
		set
		{
			refiningPerception_ = value;
		}
	}

	[DebuggerNonUserCode]
	public int RefiningtPhysique
	{
		get
		{
			return refiningtPhysique_;
		}
		set
		{
			refiningtPhysique_ = value;
		}
	}

	private MessageDescriptor pb_003A_003AGoogle_002EProtobuf_002EIMessage_002Eget_Descriptor()
	{
		return Descriptor;
	}

	[DebuggerNonUserCode]
	public GetShiKongMiJingResp_1002032()
	{
	}

	[DebuggerNonUserCode]
	public GetShiKongMiJingResp_1002032(GetShiKongMiJingResp_1002032 other)
		: this()
	{
		code_ = other.code_;
		isSuccess_ = other.isSuccess_;
		superAir_ = other.superAir_;
		currentSuperAir_ = other.currentSuperAir_;
		perception_ = other.perception_;
		currentPerception_ = other.currentPerception_;
		physique_ = other.physique_;
		currentPhysique_ = other.currentPhysique_;
		age_ = other.age_;
		pureBlood_ = other.pureBlood_;
		currentPureBlood_ = other.currentPureBlood_;
		refiningSuperAir_ = other.refiningSuperAir_;
		refiningPerception_ = other.refiningPerception_;
		refiningtPhysique_ = other.refiningtPhysique_;
		_unknownFields = UnknownFieldSet.Clone(other._unknownFields);
	}

	[DebuggerNonUserCode]
	public GetShiKongMiJingResp_1002032 Clone()
	{
		return new GetShiKongMiJingResp_1002032(this);
	}

	[DebuggerNonUserCode]
	public override bool Equals(object other)
	{
		return Equals(other as GetShiKongMiJingResp_1002032);
	}

	[DebuggerNonUserCode]
	public bool Equals(GetShiKongMiJingResp_1002032 other)
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
		if (SuperAir != other.SuperAir)
		{
			return false;
		}
		if (CurrentSuperAir != other.CurrentSuperAir)
		{
			return false;
		}
		if (Perception != other.Perception)
		{
			return false;
		}
		if (CurrentPerception != other.CurrentPerception)
		{
			return false;
		}
		if (Physique != other.Physique)
		{
			return false;
		}
		if (CurrentPhysique != other.CurrentPhysique)
		{
			return false;
		}
		if (Age != other.Age)
		{
			return false;
		}
		if (PureBlood != other.PureBlood)
		{
			return false;
		}
		if (CurrentPureBlood != other.CurrentPureBlood)
		{
			return false;
		}
		if (RefiningSuperAir != other.RefiningSuperAir)
		{
			return false;
		}
		if (RefiningPerception != other.RefiningPerception)
		{
			return false;
		}
		if (RefiningtPhysique != other.RefiningtPhysique)
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
		if (SuperAir != 0L)
		{
			num ^= SuperAir.GetHashCode();
		}
		if (CurrentSuperAir != 0L)
		{
			num ^= CurrentSuperAir.GetHashCode();
		}
		if (Perception != 0)
		{
			num ^= Perception.GetHashCode();
		}
		if (CurrentPerception != 0)
		{
			num ^= CurrentPerception.GetHashCode();
		}
		if (Physique != 0)
		{
			num ^= Physique.GetHashCode();
		}
		if (CurrentPhysique != 0)
		{
			num ^= CurrentPhysique.GetHashCode();
		}
		if (Age != 0)
		{
			num ^= Age.GetHashCode();
		}
		if (PureBlood != 0)
		{
			num ^= PureBlood.GetHashCode();
		}
		if (CurrentPureBlood != 0)
		{
			num ^= CurrentPureBlood.GetHashCode();
		}
		if (RefiningSuperAir != 0L)
		{
			num ^= RefiningSuperAir.GetHashCode();
		}
		if (RefiningPerception != 0)
		{
			num ^= RefiningPerception.GetHashCode();
		}
		if (RefiningtPhysique != 0)
		{
			num ^= RefiningtPhysique.GetHashCode();
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
		if (SuperAir != 0L)
		{
			output.WriteRawTag(24);
			output.WriteInt64(SuperAir);
		}
		if (CurrentSuperAir != 0L)
		{
			output.WriteRawTag(32);
			output.WriteInt64(CurrentSuperAir);
		}
		if (Perception != 0)
		{
			output.WriteRawTag(40);
			output.WriteInt32(Perception);
		}
		if (CurrentPerception != 0)
		{
			output.WriteRawTag(48);
			output.WriteInt32(CurrentPerception);
		}
		if (Physique != 0)
		{
			output.WriteRawTag(56);
			output.WriteInt32(Physique);
		}
		if (CurrentPhysique != 0)
		{
			output.WriteRawTag(64);
			output.WriteInt32(CurrentPhysique);
		}
		if (Age != 0)
		{
			output.WriteRawTag(72);
			output.WriteInt32(Age);
		}
		if (PureBlood != 0)
		{
			output.WriteRawTag(80);
			output.WriteInt32(PureBlood);
		}
		if (CurrentPureBlood != 0)
		{
			output.WriteRawTag(88);
			output.WriteInt32(CurrentPureBlood);
		}
		if (RefiningSuperAir != 0L)
		{
			output.WriteRawTag(96);
			output.WriteInt64(RefiningSuperAir);
		}
		if (RefiningPerception != 0)
		{
			output.WriteRawTag(104);
			output.WriteInt32(RefiningPerception);
		}
		if (RefiningtPhysique != 0)
		{
			output.WriteRawTag(112);
			output.WriteInt32(RefiningtPhysique);
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
		if (SuperAir != 0L)
		{
			num += 1 + CodedOutputStream.ComputeInt64Size(SuperAir);
		}
		if (CurrentSuperAir != 0L)
		{
			num += 1 + CodedOutputStream.ComputeInt64Size(CurrentSuperAir);
		}
		if (Perception != 0)
		{
			num += 1 + CodedOutputStream.ComputeInt32Size(Perception);
		}
		if (CurrentPerception != 0)
		{
			num += 1 + CodedOutputStream.ComputeInt32Size(CurrentPerception);
		}
		if (Physique != 0)
		{
			num += 1 + CodedOutputStream.ComputeInt32Size(Physique);
		}
		if (CurrentPhysique != 0)
		{
			num += 1 + CodedOutputStream.ComputeInt32Size(CurrentPhysique);
		}
		if (Age != 0)
		{
			num += 1 + CodedOutputStream.ComputeInt32Size(Age);
		}
		if (PureBlood != 0)
		{
			num += 1 + CodedOutputStream.ComputeInt32Size(PureBlood);
		}
		if (CurrentPureBlood != 0)
		{
			num += 1 + CodedOutputStream.ComputeInt32Size(CurrentPureBlood);
		}
		if (RefiningSuperAir != 0L)
		{
			num += 1 + CodedOutputStream.ComputeInt64Size(RefiningSuperAir);
		}
		if (RefiningPerception != 0)
		{
			num += 1 + CodedOutputStream.ComputeInt32Size(RefiningPerception);
		}
		if (RefiningtPhysique != 0)
		{
			num += 1 + CodedOutputStream.ComputeInt32Size(RefiningtPhysique);
		}
		if (_unknownFields != null)
		{
			num += _unknownFields.CalculateSize();
		}
		return num;
	}

	[DebuggerNonUserCode]
	public void MergeFrom(GetShiKongMiJingResp_1002032 other)
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
			if (other.SuperAir != 0L)
			{
				SuperAir = other.SuperAir;
			}
			if (other.CurrentSuperAir != 0L)
			{
				CurrentSuperAir = other.CurrentSuperAir;
			}
			if (other.Perception != 0)
			{
				Perception = other.Perception;
			}
			if (other.CurrentPerception != 0)
			{
				CurrentPerception = other.CurrentPerception;
			}
			if (other.Physique != 0)
			{
				Physique = other.Physique;
			}
			if (other.CurrentPhysique != 0)
			{
				CurrentPhysique = other.CurrentPhysique;
			}
			if (other.Age != 0)
			{
				Age = other.Age;
			}
			if (other.PureBlood != 0)
			{
				PureBlood = other.PureBlood;
			}
			if (other.CurrentPureBlood != 0)
			{
				CurrentPureBlood = other.CurrentPureBlood;
			}
			if (other.RefiningSuperAir != 0L)
			{
				RefiningSuperAir = other.RefiningSuperAir;
			}
			if (other.RefiningPerception != 0)
			{
				RefiningPerception = other.RefiningPerception;
			}
			if (other.RefiningtPhysique != 0)
			{
				RefiningtPhysique = other.RefiningtPhysique;
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
				SuperAir = input.ReadInt64();
				break;
			case 32u:
				CurrentSuperAir = input.ReadInt64();
				break;
			case 40u:
				Perception = input.ReadInt32();
				break;
			case 48u:
				CurrentPerception = input.ReadInt32();
				break;
			case 56u:
				Physique = input.ReadInt32();
				break;
			case 64u:
				CurrentPhysique = input.ReadInt32();
				break;
			case 72u:
				Age = input.ReadInt32();
				break;
			case 80u:
				PureBlood = input.ReadInt32();
				break;
			case 88u:
				CurrentPureBlood = input.ReadInt32();
				break;
			case 96u:
				RefiningSuperAir = input.ReadInt64();
				break;
			case 104u:
				RefiningPerception = input.ReadInt32();
				break;
			case 112u:
				RefiningtPhysique = input.ReadInt32();
				break;
			}
		}
	}
}
