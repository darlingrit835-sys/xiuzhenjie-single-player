using System;
using System.Diagnostics;
using Google.Protobuf;
using Google.Protobuf.Reflection;

public sealed class RoleBreakRealmResp_1001010 : IMessage<RoleBreakRealmResp_1001010>, IMessage, IEquatable<RoleBreakRealmResp_1001010>, IDeepCloneable<RoleBreakRealmResp_1001010>
{
	private static readonly MessageParser<RoleBreakRealmResp_1001010> _parser = new MessageParser<RoleBreakRealmResp_1001010>(() => new RoleBreakRealmResp_1001010());

	private UnknownFieldSet _unknownFields;

	public const int CodeFieldNumber = 1;

	private int code_;

	public const int IsSuccessFieldNumber = 2;

	private bool isSuccess_;

	public const int PureBloodFieldNumber = 3;

	private int pureBlood_;

	public const int FlexibleRootFieldNumber = 4;

	private string flexibleRoot_ = "";

	public const int SuperAirFieldNumber = 5;

	private long superAir_;

	public const int PerceptionFieldNumber = 6;

	private long perception_;

	public const int PhysiqueFieldNumber = 7;

	private long physique_;

	public const int CurrentSuperAirFieldNumber = 8;

	private long currentSuperAir_;

	public const int CurrentPerceptionFieldNumber = 9;

	private long currentPerception_;

	public const int CurrentPhysiqueFieldNumber = 10;

	private long currentPhysique_;

	public const int CurrentVigorFieldNumber = 11;

	private int currentVigor_;

	public const int CurrentCleverFieldNumber = 12;

	private int currentClever_;

	public const int CurrentPoisonValFieldNumber = 13;

	private int currentPoisonVal_;

	public const int CurrentPureBloodFieldNumber = 14;

	private int currentPureBlood_;

	public const int AgeFieldNumber = 15;

	private int age_;

	public const int CurrentPowerFieldNumber = 16;

	private int currentPower_;

	[DebuggerNonUserCode]
	public static MessageParser<RoleBreakRealmResp_1001010> Parser => _parser;

	[DebuggerNonUserCode]
	public static MessageDescriptor Descriptor => SimpleActionProtoReflection.Descriptor.MessageTypes[12];

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
	public string FlexibleRoot
	{
		get
		{
			return flexibleRoot_;
		}
		set
		{
			flexibleRoot_ = ProtoPreconditions.CheckNotNull(value, "value");
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
	public long Perception
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
	public long Physique
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
	public long CurrentPerception
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
	public long CurrentPhysique
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
	public int CurrentVigor
	{
		get
		{
			return currentVigor_;
		}
		set
		{
			currentVigor_ = value;
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
	public int CurrentPoisonVal
	{
		get
		{
			return currentPoisonVal_;
		}
		set
		{
			currentPoisonVal_ = value;
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
	public int CurrentPower
	{
		get
		{
			return currentPower_;
		}
		set
		{
			currentPower_ = value;
		}
	}

	private MessageDescriptor pb_003A_003AGoogle_002EProtobuf_002EIMessage_002Eget_Descriptor()
	{
		return Descriptor;
	}

	[DebuggerNonUserCode]
	public RoleBreakRealmResp_1001010()
	{
	}

	[DebuggerNonUserCode]
	public RoleBreakRealmResp_1001010(RoleBreakRealmResp_1001010 other)
		: this()
	{
		code_ = other.code_;
		isSuccess_ = other.isSuccess_;
		pureBlood_ = other.pureBlood_;
		flexibleRoot_ = other.flexibleRoot_;
		superAir_ = other.superAir_;
		perception_ = other.perception_;
		physique_ = other.physique_;
		currentSuperAir_ = other.currentSuperAir_;
		currentPerception_ = other.currentPerception_;
		currentPhysique_ = other.currentPhysique_;
		currentVigor_ = other.currentVigor_;
		currentClever_ = other.currentClever_;
		currentPoisonVal_ = other.currentPoisonVal_;
		currentPureBlood_ = other.currentPureBlood_;
		age_ = other.age_;
		currentPower_ = other.currentPower_;
		_unknownFields = UnknownFieldSet.Clone(other._unknownFields);
	}

	[DebuggerNonUserCode]
	public RoleBreakRealmResp_1001010 Clone()
	{
		return new RoleBreakRealmResp_1001010(this);
	}

	[DebuggerNonUserCode]
	public override bool Equals(object other)
	{
		return Equals(other as RoleBreakRealmResp_1001010);
	}

	[DebuggerNonUserCode]
	public bool Equals(RoleBreakRealmResp_1001010 other)
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
		if (PureBlood != other.PureBlood)
		{
			return false;
		}
		if (FlexibleRoot != other.FlexibleRoot)
		{
			return false;
		}
		if (SuperAir != other.SuperAir)
		{
			return false;
		}
		if (Perception != other.Perception)
		{
			return false;
		}
		if (Physique != other.Physique)
		{
			return false;
		}
		if (CurrentSuperAir != other.CurrentSuperAir)
		{
			return false;
		}
		if (CurrentPerception != other.CurrentPerception)
		{
			return false;
		}
		if (CurrentPhysique != other.CurrentPhysique)
		{
			return false;
		}
		if (CurrentVigor != other.CurrentVigor)
		{
			return false;
		}
		if (CurrentClever != other.CurrentClever)
		{
			return false;
		}
		if (CurrentPoisonVal != other.CurrentPoisonVal)
		{
			return false;
		}
		if (CurrentPureBlood != other.CurrentPureBlood)
		{
			return false;
		}
		if (Age != other.Age)
		{
			return false;
		}
		if (CurrentPower != other.CurrentPower)
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
		if (PureBlood != 0)
		{
			num ^= PureBlood.GetHashCode();
		}
		if (FlexibleRoot.Length != 0)
		{
			num ^= FlexibleRoot.GetHashCode();
		}
		if (SuperAir != 0L)
		{
			num ^= SuperAir.GetHashCode();
		}
		if (Perception != 0L)
		{
			num ^= Perception.GetHashCode();
		}
		if (Physique != 0L)
		{
			num ^= Physique.GetHashCode();
		}
		if (CurrentSuperAir != 0L)
		{
			num ^= CurrentSuperAir.GetHashCode();
		}
		if (CurrentPerception != 0L)
		{
			num ^= CurrentPerception.GetHashCode();
		}
		if (CurrentPhysique != 0L)
		{
			num ^= CurrentPhysique.GetHashCode();
		}
		if (CurrentVigor != 0)
		{
			num ^= CurrentVigor.GetHashCode();
		}
		if (CurrentClever != 0)
		{
			num ^= CurrentClever.GetHashCode();
		}
		if (CurrentPoisonVal != 0)
		{
			num ^= CurrentPoisonVal.GetHashCode();
		}
		if (CurrentPureBlood != 0)
		{
			num ^= CurrentPureBlood.GetHashCode();
		}
		if (Age != 0)
		{
			num ^= Age.GetHashCode();
		}
		if (CurrentPower != 0)
		{
			num ^= CurrentPower.GetHashCode();
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
		if (PureBlood != 0)
		{
			output.WriteRawTag(24);
			output.WriteInt32(PureBlood);
		}
		if (FlexibleRoot.Length != 0)
		{
			output.WriteRawTag(34);
			output.WriteString(FlexibleRoot);
		}
		if (SuperAir != 0L)
		{
			output.WriteRawTag(40);
			output.WriteInt64(SuperAir);
		}
		if (Perception != 0L)
		{
			output.WriteRawTag(48);
			output.WriteInt64(Perception);
		}
		if (Physique != 0L)
		{
			output.WriteRawTag(56);
			output.WriteInt64(Physique);
		}
		if (CurrentSuperAir != 0L)
		{
			output.WriteRawTag(64);
			output.WriteInt64(CurrentSuperAir);
		}
		if (CurrentPerception != 0L)
		{
			output.WriteRawTag(72);
			output.WriteInt64(CurrentPerception);
		}
		if (CurrentPhysique != 0L)
		{
			output.WriteRawTag(80);
			output.WriteInt64(CurrentPhysique);
		}
		if (CurrentVigor != 0)
		{
			output.WriteRawTag(88);
			output.WriteInt32(CurrentVigor);
		}
		if (CurrentClever != 0)
		{
			output.WriteRawTag(96);
			output.WriteSInt32(CurrentClever);
		}
		if (CurrentPoisonVal != 0)
		{
			output.WriteRawTag(104);
			output.WriteInt32(CurrentPoisonVal);
		}
		if (CurrentPureBlood != 0)
		{
			output.WriteRawTag(112);
			output.WriteInt32(CurrentPureBlood);
		}
		if (Age != 0)
		{
			output.WriteRawTag(120);
			output.WriteInt32(Age);
		}
		if (CurrentPower != 0)
		{
			output.WriteRawTag(128, 1);
			output.WriteInt32(CurrentPower);
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
		if (PureBlood != 0)
		{
			num += 1 + CodedOutputStream.ComputeInt32Size(PureBlood);
		}
		if (FlexibleRoot.Length != 0)
		{
			num += 1 + CodedOutputStream.ComputeStringSize(FlexibleRoot);
		}
		if (SuperAir != 0L)
		{
			num += 1 + CodedOutputStream.ComputeInt64Size(SuperAir);
		}
		if (Perception != 0L)
		{
			num += 1 + CodedOutputStream.ComputeInt64Size(Perception);
		}
		if (Physique != 0L)
		{
			num += 1 + CodedOutputStream.ComputeInt64Size(Physique);
		}
		if (CurrentSuperAir != 0L)
		{
			num += 1 + CodedOutputStream.ComputeInt64Size(CurrentSuperAir);
		}
		if (CurrentPerception != 0L)
		{
			num += 1 + CodedOutputStream.ComputeInt64Size(CurrentPerception);
		}
		if (CurrentPhysique != 0L)
		{
			num += 1 + CodedOutputStream.ComputeInt64Size(CurrentPhysique);
		}
		if (CurrentVigor != 0)
		{
			num += 1 + CodedOutputStream.ComputeInt32Size(CurrentVigor);
		}
		if (CurrentClever != 0)
		{
			num += 1 + CodedOutputStream.ComputeSInt32Size(CurrentClever);
		}
		if (CurrentPoisonVal != 0)
		{
			num += 1 + CodedOutputStream.ComputeInt32Size(CurrentPoisonVal);
		}
		if (CurrentPureBlood != 0)
		{
			num += 1 + CodedOutputStream.ComputeInt32Size(CurrentPureBlood);
		}
		if (Age != 0)
		{
			num += 1 + CodedOutputStream.ComputeInt32Size(Age);
		}
		if (CurrentPower != 0)
		{
			num += 2 + CodedOutputStream.ComputeInt32Size(CurrentPower);
		}
		if (_unknownFields != null)
		{
			num += _unknownFields.CalculateSize();
		}
		return num;
	}

	[DebuggerNonUserCode]
	public void MergeFrom(RoleBreakRealmResp_1001010 other)
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
			if (other.PureBlood != 0)
			{
				PureBlood = other.PureBlood;
			}
			if (other.FlexibleRoot.Length != 0)
			{
				FlexibleRoot = other.FlexibleRoot;
			}
			if (other.SuperAir != 0L)
			{
				SuperAir = other.SuperAir;
			}
			if (other.Perception != 0L)
			{
				Perception = other.Perception;
			}
			if (other.Physique != 0L)
			{
				Physique = other.Physique;
			}
			if (other.CurrentSuperAir != 0L)
			{
				CurrentSuperAir = other.CurrentSuperAir;
			}
			if (other.CurrentPerception != 0L)
			{
				CurrentPerception = other.CurrentPerception;
			}
			if (other.CurrentPhysique != 0L)
			{
				CurrentPhysique = other.CurrentPhysique;
			}
			if (other.CurrentVigor != 0)
			{
				CurrentVigor = other.CurrentVigor;
			}
			if (other.CurrentClever != 0)
			{
				CurrentClever = other.CurrentClever;
			}
			if (other.CurrentPoisonVal != 0)
			{
				CurrentPoisonVal = other.CurrentPoisonVal;
			}
			if (other.CurrentPureBlood != 0)
			{
				CurrentPureBlood = other.CurrentPureBlood;
			}
			if (other.Age != 0)
			{
				Age = other.Age;
			}
			if (other.CurrentPower != 0)
			{
				CurrentPower = other.CurrentPower;
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
				PureBlood = input.ReadInt32();
				break;
			case 34u:
				FlexibleRoot = input.ReadString();
				break;
			case 40u:
				SuperAir = input.ReadInt64();
				break;
			case 48u:
				Perception = input.ReadInt64();
				break;
			case 56u:
				Physique = input.ReadInt64();
				break;
			case 64u:
				CurrentSuperAir = input.ReadInt64();
				break;
			case 72u:
				CurrentPerception = input.ReadInt64();
				break;
			case 80u:
				CurrentPhysique = input.ReadInt64();
				break;
			case 88u:
				CurrentVigor = input.ReadInt32();
				break;
			case 96u:
				CurrentClever = input.ReadSInt32();
				break;
			case 104u:
				CurrentPoisonVal = input.ReadInt32();
				break;
			case 112u:
				CurrentPureBlood = input.ReadInt32();
				break;
			case 120u:
				Age = input.ReadInt32();
				break;
			case 128u:
				CurrentPower = input.ReadInt32();
				break;
			}
		}
	}
}
