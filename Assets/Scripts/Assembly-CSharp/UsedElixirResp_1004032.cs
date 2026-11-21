using System;
using System.Diagnostics;
using Google.Protobuf;
using Google.Protobuf.Reflection;

public sealed class UsedElixirResp_1004032 : IMessage<UsedElixirResp_1004032>, IMessage, IEquatable<UsedElixirResp_1004032>, IDeepCloneable<UsedElixirResp_1004032>
{
	private static readonly MessageParser<UsedElixirResp_1004032> _parser = new MessageParser<UsedElixirResp_1004032>(() => new UsedElixirResp_1004032());

	private UnknownFieldSet _unknownFields;

	public const int CodeFieldNumber = 1;

	private int code_;

	public const int IsSuccessFieldNumber = 2;

	private bool isSuccess_;

	public const int NowCapacityFieldNumber = 3;

	private int nowCapacity_;

	public const int PureBloodFieldNumber = 4;

	private int pureBlood_;

	public const int FlexibleRootFieldNumber = 5;

	private string flexibleRoot_ = "";

	public const int SuperAirFieldNumber = 6;

	private long superAir_;

	public const int PerceptionFieldNumber = 7;

	private long perception_;

	public const int PhysiqueFieldNumber = 8;

	private long physique_;

	public const int CurrentSuperAirFieldNumber = 9;

	private long currentSuperAir_;

	public const int CurrentPerceptionFieldNumber = 10;

	private long currentPerception_;

	public const int CurrentPhysiqueFieldNumber = 11;

	private long currentPhysique_;

	public const int CurrentVigorFieldNumber = 12;

	private int currentVigor_;

	public const int CurrentCleverFieldNumber = 13;

	private int currentClever_;

	public const int CurrentPoisonValFieldNumber = 14;

	private int currentPoisonVal_;

	public const int CurrentPureBloodFieldNumber = 15;

	private int currentPureBlood_;

	public const int HasWoodFieldNumber = 16;

	private bool hasWood_;

	public const int HasGoldFieldNumber = 17;

	private bool hasGold_;

	public const int HasWaterFieldNumber = 18;

	private bool hasWater_;

	public const int HasFireFieldNumber = 19;

	private bool hasFire_;

	public const int HasEarthFieldNumber = 20;

	private bool hasEarth_;

	public const int HunLiValFieldNumber = 21;

	private int hunLiVal_;

	public const int PowerFieldNumber = 22;

	private int power_;

	public const int CurrentPowerFieldNumber = 23;

	private int currentPower_;

	public const int AgeFieldNumber = 24;

	private int age_;

	public const int MaxAgeFieldNumber = 25;

	private int maxAge_;

	[DebuggerNonUserCode]
	public static MessageParser<UsedElixirResp_1004032> Parser => _parser;

	[DebuggerNonUserCode]
	public static MessageDescriptor Descriptor => GoodsProtoReflection.Descriptor.MessageTypes[56];

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
	public int NowCapacity
	{
		get
		{
			return nowCapacity_;
		}
		set
		{
			nowCapacity_ = value;
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
	public bool HasWood
	{
		get
		{
			return hasWood_;
		}
		set
		{
			hasWood_ = value;
		}
	}

	[DebuggerNonUserCode]
	public bool HasGold
	{
		get
		{
			return hasGold_;
		}
		set
		{
			hasGold_ = value;
		}
	}

	[DebuggerNonUserCode]
	public bool HasWater
	{
		get
		{
			return hasWater_;
		}
		set
		{
			hasWater_ = value;
		}
	}

	[DebuggerNonUserCode]
	public bool HasFire
	{
		get
		{
			return hasFire_;
		}
		set
		{
			hasFire_ = value;
		}
	}

	[DebuggerNonUserCode]
	public bool HasEarth
	{
		get
		{
			return hasEarth_;
		}
		set
		{
			hasEarth_ = value;
		}
	}

	[DebuggerNonUserCode]
	public int HunLiVal
	{
		get
		{
			return hunLiVal_;
		}
		set
		{
			hunLiVal_ = value;
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
	public int MaxAge
	{
		get
		{
			return maxAge_;
		}
		set
		{
			maxAge_ = value;
		}
	}

	private MessageDescriptor pb_003A_003AGoogle_002EProtobuf_002EIMessage_002Eget_Descriptor()
	{
		return Descriptor;
	}

	[DebuggerNonUserCode]
	public UsedElixirResp_1004032()
	{
	}

	[DebuggerNonUserCode]
	public UsedElixirResp_1004032(UsedElixirResp_1004032 other)
		: this()
	{
		code_ = other.code_;
		isSuccess_ = other.isSuccess_;
		nowCapacity_ = other.nowCapacity_;
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
		hasWood_ = other.hasWood_;
		hasGold_ = other.hasGold_;
		hasWater_ = other.hasWater_;
		hasFire_ = other.hasFire_;
		hasEarth_ = other.hasEarth_;
		hunLiVal_ = other.hunLiVal_;
		power_ = other.power_;
		currentPower_ = other.currentPower_;
		age_ = other.age_;
		maxAge_ = other.maxAge_;
		_unknownFields = UnknownFieldSet.Clone(other._unknownFields);
	}

	[DebuggerNonUserCode]
	public UsedElixirResp_1004032 Clone()
	{
		return new UsedElixirResp_1004032(this);
	}

	[DebuggerNonUserCode]
	public override bool Equals(object other)
	{
		return Equals(other as UsedElixirResp_1004032);
	}

	[DebuggerNonUserCode]
	public bool Equals(UsedElixirResp_1004032 other)
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
		if (NowCapacity != other.NowCapacity)
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
		if (HasWood != other.HasWood)
		{
			return false;
		}
		if (HasGold != other.HasGold)
		{
			return false;
		}
		if (HasWater != other.HasWater)
		{
			return false;
		}
		if (HasFire != other.HasFire)
		{
			return false;
		}
		if (HasEarth != other.HasEarth)
		{
			return false;
		}
		if (HunLiVal != other.HunLiVal)
		{
			return false;
		}
		if (Power != other.Power)
		{
			return false;
		}
		if (CurrentPower != other.CurrentPower)
		{
			return false;
		}
		if (Age != other.Age)
		{
			return false;
		}
		if (MaxAge != other.MaxAge)
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
		if (NowCapacity != 0)
		{
			num ^= NowCapacity.GetHashCode();
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
		if (HasWood)
		{
			num ^= HasWood.GetHashCode();
		}
		if (HasGold)
		{
			num ^= HasGold.GetHashCode();
		}
		if (HasWater)
		{
			num ^= HasWater.GetHashCode();
		}
		if (HasFire)
		{
			num ^= HasFire.GetHashCode();
		}
		if (HasEarth)
		{
			num ^= HasEarth.GetHashCode();
		}
		if (HunLiVal != 0)
		{
			num ^= HunLiVal.GetHashCode();
		}
		if (Power != 0)
		{
			num ^= Power.GetHashCode();
		}
		if (CurrentPower != 0)
		{
			num ^= CurrentPower.GetHashCode();
		}
		if (Age != 0)
		{
			num ^= Age.GetHashCode();
		}
		if (MaxAge != 0)
		{
			num ^= MaxAge.GetHashCode();
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
		if (NowCapacity != 0)
		{
			output.WriteRawTag(24);
			output.WriteInt32(NowCapacity);
		}
		if (PureBlood != 0)
		{
			output.WriteRawTag(32);
			output.WriteInt32(PureBlood);
		}
		if (FlexibleRoot.Length != 0)
		{
			output.WriteRawTag(42);
			output.WriteString(FlexibleRoot);
		}
		if (SuperAir != 0L)
		{
			output.WriteRawTag(48);
			output.WriteInt64(SuperAir);
		}
		if (Perception != 0L)
		{
			output.WriteRawTag(56);
			output.WriteInt64(Perception);
		}
		if (Physique != 0L)
		{
			output.WriteRawTag(64);
			output.WriteInt64(Physique);
		}
		if (CurrentSuperAir != 0L)
		{
			output.WriteRawTag(72);
			output.WriteInt64(CurrentSuperAir);
		}
		if (CurrentPerception != 0L)
		{
			output.WriteRawTag(80);
			output.WriteInt64(CurrentPerception);
		}
		if (CurrentPhysique != 0L)
		{
			output.WriteRawTag(88);
			output.WriteInt64(CurrentPhysique);
		}
		if (CurrentVigor != 0)
		{
			output.WriteRawTag(96);
			output.WriteInt32(CurrentVigor);
		}
		if (CurrentClever != 0)
		{
			output.WriteRawTag(104);
			output.WriteSInt32(CurrentClever);
		}
		if (CurrentPoisonVal != 0)
		{
			output.WriteRawTag(112);
			output.WriteInt32(CurrentPoisonVal);
		}
		if (CurrentPureBlood != 0)
		{
			output.WriteRawTag(120);
			output.WriteInt32(CurrentPureBlood);
		}
		if (HasWood)
		{
			output.WriteRawTag(128, 1);
			output.WriteBool(HasWood);
		}
		if (HasGold)
		{
			output.WriteRawTag(136, 1);
			output.WriteBool(HasGold);
		}
		if (HasWater)
		{
			output.WriteRawTag(144, 1);
			output.WriteBool(HasWater);
		}
		if (HasFire)
		{
			output.WriteRawTag(152, 1);
			output.WriteBool(HasFire);
		}
		if (HasEarth)
		{
			output.WriteRawTag(160, 1);
			output.WriteBool(HasEarth);
		}
		if (HunLiVal != 0)
		{
			output.WriteRawTag(168, 1);
			output.WriteInt32(HunLiVal);
		}
		if (Power != 0)
		{
			output.WriteRawTag(176, 1);
			output.WriteInt32(Power);
		}
		if (CurrentPower != 0)
		{
			output.WriteRawTag(184, 1);
			output.WriteInt32(CurrentPower);
		}
		if (Age != 0)
		{
			output.WriteRawTag(192, 1);
			output.WriteInt32(Age);
		}
		if (MaxAge != 0)
		{
			output.WriteRawTag(200, 1);
			output.WriteInt32(MaxAge);
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
		if (NowCapacity != 0)
		{
			num += 1 + CodedOutputStream.ComputeInt32Size(NowCapacity);
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
		if (HasWood)
		{
			num += 3;
		}
		if (HasGold)
		{
			num += 3;
		}
		if (HasWater)
		{
			num += 3;
		}
		if (HasFire)
		{
			num += 3;
		}
		if (HasEarth)
		{
			num += 3;
		}
		if (HunLiVal != 0)
		{
			num += 2 + CodedOutputStream.ComputeInt32Size(HunLiVal);
		}
		if (Power != 0)
		{
			num += 2 + CodedOutputStream.ComputeInt32Size(Power);
		}
		if (CurrentPower != 0)
		{
			num += 2 + CodedOutputStream.ComputeInt32Size(CurrentPower);
		}
		if (Age != 0)
		{
			num += 2 + CodedOutputStream.ComputeInt32Size(Age);
		}
		if (MaxAge != 0)
		{
			num += 2 + CodedOutputStream.ComputeInt32Size(MaxAge);
		}
		if (_unknownFields != null)
		{
			num += _unknownFields.CalculateSize();
		}
		return num;
	}

	[DebuggerNonUserCode]
	public void MergeFrom(UsedElixirResp_1004032 other)
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
			if (other.NowCapacity != 0)
			{
				NowCapacity = other.NowCapacity;
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
			if (other.HasWood)
			{
				HasWood = other.HasWood;
			}
			if (other.HasGold)
			{
				HasGold = other.HasGold;
			}
			if (other.HasWater)
			{
				HasWater = other.HasWater;
			}
			if (other.HasFire)
			{
				HasFire = other.HasFire;
			}
			if (other.HasEarth)
			{
				HasEarth = other.HasEarth;
			}
			if (other.HunLiVal != 0)
			{
				HunLiVal = other.HunLiVal;
			}
			if (other.Power != 0)
			{
				Power = other.Power;
			}
			if (other.CurrentPower != 0)
			{
				CurrentPower = other.CurrentPower;
			}
			if (other.Age != 0)
			{
				Age = other.Age;
			}
			if (other.MaxAge != 0)
			{
				MaxAge = other.MaxAge;
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
				NowCapacity = input.ReadInt32();
				break;
			case 32u:
				PureBlood = input.ReadInt32();
				break;
			case 42u:
				FlexibleRoot = input.ReadString();
				break;
			case 48u:
				SuperAir = input.ReadInt64();
				break;
			case 56u:
				Perception = input.ReadInt64();
				break;
			case 64u:
				Physique = input.ReadInt64();
				break;
			case 72u:
				CurrentSuperAir = input.ReadInt64();
				break;
			case 80u:
				CurrentPerception = input.ReadInt64();
				break;
			case 88u:
				CurrentPhysique = input.ReadInt64();
				break;
			case 96u:
				CurrentVigor = input.ReadInt32();
				break;
			case 104u:
				CurrentClever = input.ReadSInt32();
				break;
			case 112u:
				CurrentPoisonVal = input.ReadInt32();
				break;
			case 120u:
				CurrentPureBlood = input.ReadInt32();
				break;
			case 128u:
				HasWood = input.ReadBool();
				break;
			case 136u:
				HasGold = input.ReadBool();
				break;
			case 144u:
				HasWater = input.ReadBool();
				break;
			case 152u:
				HasFire = input.ReadBool();
				break;
			case 160u:
				HasEarth = input.ReadBool();
				break;
			case 168u:
				HunLiVal = input.ReadInt32();
				break;
			case 176u:
				Power = input.ReadInt32();
				break;
			case 184u:
				CurrentPower = input.ReadInt32();
				break;
			case 192u:
				Age = input.ReadInt32();
				break;
			case 200u:
				MaxAge = input.ReadInt32();
				break;
			}
		}
	}
}
