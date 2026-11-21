using System;
using System.Diagnostics;
using Google.Protobuf;
using Google.Protobuf.Reflection;

public sealed class RoleGetOfflineResp_1002004 : IMessage<RoleGetOfflineResp_1002004>, IMessage, IEquatable<RoleGetOfflineResp_1002004>, IDeepCloneable<RoleGetOfflineResp_1002004>
{
	private static readonly MessageParser<RoleGetOfflineResp_1002004> _parser = new MessageParser<RoleGetOfflineResp_1002004>(() => new RoleGetOfflineResp_1002004());

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

	public const int VigorFieldNumber = 10;

	private int vigor_;

	public const int CleverFieldNumber = 11;

	private int clever_;

	public const int PoisonValFieldNumber = 12;

	private int poisonVal_;

	public const int PureBloodFieldNumber = 13;

	private int pureBlood_;

	public const int CurrentPureBloodFieldNumber = 14;

	private int currentPureBlood_;

	public const int PowerFieldNumber = 15;

	private int power_;

	[DebuggerNonUserCode]
	public static MessageParser<RoleGetOfflineResp_1002004> Parser => _parser;

	[DebuggerNonUserCode]
	public static MessageDescriptor Descriptor => RoleProtoReflection.Descriptor.MessageTypes[6];

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
	public int Vigor
	{
		get
		{
			return vigor_;
		}
		set
		{
			vigor_ = value;
		}
	}

	[DebuggerNonUserCode]
	public int Clever
	{
		get
		{
			return clever_;
		}
		set
		{
			clever_ = value;
		}
	}

	[DebuggerNonUserCode]
	public int PoisonVal
	{
		get
		{
			return poisonVal_;
		}
		set
		{
			poisonVal_ = value;
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
	public int Power
	{
		get
		{
			return power_;
		}
		set
		{
			power_ = value;
		}
	}

	private MessageDescriptor pb_003A_003AGoogle_002EProtobuf_002EIMessage_002Eget_Descriptor()
	{
		return Descriptor;
	}

	[DebuggerNonUserCode]
	public RoleGetOfflineResp_1002004()
	{
	}

	[DebuggerNonUserCode]
	public RoleGetOfflineResp_1002004(RoleGetOfflineResp_1002004 other)
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
		vigor_ = other.vigor_;
		clever_ = other.clever_;
		poisonVal_ = other.poisonVal_;
		pureBlood_ = other.pureBlood_;
		currentPureBlood_ = other.currentPureBlood_;
		power_ = other.power_;
		_unknownFields = UnknownFieldSet.Clone(other._unknownFields);
	}

	[DebuggerNonUserCode]
	public RoleGetOfflineResp_1002004 Clone()
	{
		return new RoleGetOfflineResp_1002004(this);
	}

	[DebuggerNonUserCode]
	public override bool Equals(object other)
	{
		return Equals(other as RoleGetOfflineResp_1002004);
	}

	[DebuggerNonUserCode]
	public bool Equals(RoleGetOfflineResp_1002004 other)
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
		if (Vigor != other.Vigor)
		{
			return false;
		}
		if (Clever != other.Clever)
		{
			return false;
		}
		if (PoisonVal != other.PoisonVal)
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
		if (Power != other.Power)
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
		if (Vigor != 0)
		{
			num ^= Vigor.GetHashCode();
		}
		if (Clever != 0)
		{
			num ^= Clever.GetHashCode();
		}
		if (PoisonVal != 0)
		{
			num ^= PoisonVal.GetHashCode();
		}
		if (PureBlood != 0)
		{
			num ^= PureBlood.GetHashCode();
		}
		if (CurrentPureBlood != 0)
		{
			num ^= CurrentPureBlood.GetHashCode();
		}
		if (Power != 0)
		{
			num ^= Power.GetHashCode();
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
		if (Vigor != 0)
		{
			output.WriteRawTag(80);
			output.WriteInt32(Vigor);
		}
		if (Clever != 0)
		{
			output.WriteRawTag(88);
			output.WriteInt32(Clever);
		}
		if (PoisonVal != 0)
		{
			output.WriteRawTag(96);
			output.WriteInt32(PoisonVal);
		}
		if (PureBlood != 0)
		{
			output.WriteRawTag(104);
			output.WriteInt32(PureBlood);
		}
		if (CurrentPureBlood != 0)
		{
			output.WriteRawTag(112);
			output.WriteInt32(CurrentPureBlood);
		}
		if (Power != 0)
		{
			output.WriteRawTag(120);
			output.WriteInt32(Power);
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
		if (Vigor != 0)
		{
			num += 1 + CodedOutputStream.ComputeInt32Size(Vigor);
		}
		if (Clever != 0)
		{
			num += 1 + CodedOutputStream.ComputeInt32Size(Clever);
		}
		if (PoisonVal != 0)
		{
			num += 1 + CodedOutputStream.ComputeInt32Size(PoisonVal);
		}
		if (PureBlood != 0)
		{
			num += 1 + CodedOutputStream.ComputeInt32Size(PureBlood);
		}
		if (CurrentPureBlood != 0)
		{
			num += 1 + CodedOutputStream.ComputeInt32Size(CurrentPureBlood);
		}
		if (Power != 0)
		{
			num += 1 + CodedOutputStream.ComputeInt32Size(Power);
		}
		if (_unknownFields != null)
		{
			num += _unknownFields.CalculateSize();
		}
		return num;
	}

	[DebuggerNonUserCode]
	public void MergeFrom(RoleGetOfflineResp_1002004 other)
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
			if (other.Vigor != 0)
			{
				Vigor = other.Vigor;
			}
			if (other.Clever != 0)
			{
				Clever = other.Clever;
			}
			if (other.PoisonVal != 0)
			{
				PoisonVal = other.PoisonVal;
			}
			if (other.PureBlood != 0)
			{
				PureBlood = other.PureBlood;
			}
			if (other.CurrentPureBlood != 0)
			{
				CurrentPureBlood = other.CurrentPureBlood;
			}
			if (other.Power != 0)
			{
				Power = other.Power;
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
				Vigor = input.ReadInt32();
				break;
			case 88u:
				Clever = input.ReadInt32();
				break;
			case 96u:
				PoisonVal = input.ReadInt32();
				break;
			case 104u:
				PureBlood = input.ReadInt32();
				break;
			case 112u:
				CurrentPureBlood = input.ReadInt32();
				break;
			case 120u:
				Power = input.ReadInt32();
				break;
			}
		}
	}
}
