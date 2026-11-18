using System;
using System.Diagnostics;
using Google.Protobuf;
using Google.Protobuf.Collections;
using Google.Protobuf.Reflection;

public sealed class BrokenStateResp_1005012 : IMessage<BrokenStateResp_1005012>, IMessage, IEquatable<BrokenStateResp_1005012>, IDeepCloneable<BrokenStateResp_1005012>
{
	private static readonly MessageParser<BrokenStateResp_1005012> _parser = new MessageParser<BrokenStateResp_1005012>(() => new BrokenStateResp_1005012());

	private UnknownFieldSet _unknownFields;

	public const int CodeFieldNumber = 1;

	private int code_;

	public const int IsSuccessFieldNumber = 2;

	private bool isSuccess_;

	public const int MaxAgeFieldNumber = 3;

	private int maxAge_;

	public const int BrokenAgeFieldNumber = 4;

	private int brokenAge_;

	public const int SuperAirFieldNumber = 5;

	private double superAir_;

	public const int PerceptionFieldNumber = 6;

	private double perception_;

	public const int PhysiqueFieldNumber = 7;

	private double physique_;

	public const int CurrentSuperAirFieldNumber = 8;

	private double currentSuperAir_;

	public const int CurrentPerceptionFieldNumber = 9;

	private double currentPerception_;

	public const int CurrentPhysiqueFieldNumber = 10;

	private double currentPhysique_;

	public const int PureBloodFieldNumber = 11;

	private double pureBlood_;

	public const int CurrentPureBloodFieldNumber = 12;

	private double currentPureBlood_;

	[DebuggerNonUserCode]
	public static MessageParser<BrokenStateResp_1005012> Parser => _parser;

	[DebuggerNonUserCode]
	public static MessageDescriptor Descriptor => HomeProtoReflection.Descriptor.MessageTypes[16];

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

	[DebuggerNonUserCode]
	public int BrokenAge
	{
		get
		{
			return brokenAge_;
		}
		set
		{
			brokenAge_ = value;
		}
	}

	[DebuggerNonUserCode]
	public double SuperAir
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
	public double Perception
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
	public double Physique
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
	public double CurrentSuperAir
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
	public double CurrentPerception
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
	public double CurrentPhysique
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
	public double PureBlood
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
	public double CurrentPureBlood
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

	private MessageDescriptor pb_003A_003AGoogle_002EProtobuf_002EIMessage_002Eget_Descriptor()
	{
		return Descriptor;
	}

	[DebuggerNonUserCode]
	public BrokenStateResp_1005012()
	{
	}

	[DebuggerNonUserCode]
	public BrokenStateResp_1005012(BrokenStateResp_1005012 other)
		: this()
	{
		code_ = other.code_;
		isSuccess_ = other.isSuccess_;
		maxAge_ = other.maxAge_;
		brokenAge_ = other.brokenAge_;
		superAir_ = other.superAir_;
		perception_ = other.perception_;
		physique_ = other.physique_;
		currentSuperAir_ = other.currentSuperAir_;
		currentPerception_ = other.currentPerception_;
		currentPhysique_ = other.currentPhysique_;
		pureBlood_ = other.pureBlood_;
		currentPureBlood_ = other.currentPureBlood_;
		_unknownFields = UnknownFieldSet.Clone(other._unknownFields);
	}

	[DebuggerNonUserCode]
	public BrokenStateResp_1005012 Clone()
	{
		return new BrokenStateResp_1005012(this);
	}

	[DebuggerNonUserCode]
	public override bool Equals(object other)
	{
		return Equals(other as BrokenStateResp_1005012);
	}

	[DebuggerNonUserCode]
	public bool Equals(BrokenStateResp_1005012 other)
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
		if (MaxAge != other.MaxAge)
		{
			return false;
		}
		if (BrokenAge != other.BrokenAge)
		{
			return false;
		}
		if (!ProtobufEqualityComparers.BitwiseDoubleEqualityComparer.Equals(SuperAir, other.SuperAir))
		{
			return false;
		}
		if (!ProtobufEqualityComparers.BitwiseDoubleEqualityComparer.Equals(Perception, other.Perception))
		{
			return false;
		}
		if (!ProtobufEqualityComparers.BitwiseDoubleEqualityComparer.Equals(Physique, other.Physique))
		{
			return false;
		}
		if (!ProtobufEqualityComparers.BitwiseDoubleEqualityComparer.Equals(CurrentSuperAir, other.CurrentSuperAir))
		{
			return false;
		}
		if (!ProtobufEqualityComparers.BitwiseDoubleEqualityComparer.Equals(CurrentPerception, other.CurrentPerception))
		{
			return false;
		}
		if (!ProtobufEqualityComparers.BitwiseDoubleEqualityComparer.Equals(CurrentPhysique, other.CurrentPhysique))
		{
			return false;
		}
		if (!ProtobufEqualityComparers.BitwiseDoubleEqualityComparer.Equals(PureBlood, other.PureBlood))
		{
			return false;
		}
		if (!ProtobufEqualityComparers.BitwiseDoubleEqualityComparer.Equals(CurrentPureBlood, other.CurrentPureBlood))
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
		if (MaxAge != 0)
		{
			num ^= MaxAge.GetHashCode();
		}
		if (BrokenAge != 0)
		{
			num ^= BrokenAge.GetHashCode();
		}
		if (SuperAir != 0.0)
		{
			num ^= ProtobufEqualityComparers.BitwiseDoubleEqualityComparer.GetHashCode(SuperAir);
		}
		if (Perception != 0.0)
		{
			num ^= ProtobufEqualityComparers.BitwiseDoubleEqualityComparer.GetHashCode(Perception);
		}
		if (Physique != 0.0)
		{
			num ^= ProtobufEqualityComparers.BitwiseDoubleEqualityComparer.GetHashCode(Physique);
		}
		if (CurrentSuperAir != 0.0)
		{
			num ^= ProtobufEqualityComparers.BitwiseDoubleEqualityComparer.GetHashCode(CurrentSuperAir);
		}
		if (CurrentPerception != 0.0)
		{
			num ^= ProtobufEqualityComparers.BitwiseDoubleEqualityComparer.GetHashCode(CurrentPerception);
		}
		if (CurrentPhysique != 0.0)
		{
			num ^= ProtobufEqualityComparers.BitwiseDoubleEqualityComparer.GetHashCode(CurrentPhysique);
		}
		if (PureBlood != 0.0)
		{
			num ^= ProtobufEqualityComparers.BitwiseDoubleEqualityComparer.GetHashCode(PureBlood);
		}
		if (CurrentPureBlood != 0.0)
		{
			num ^= ProtobufEqualityComparers.BitwiseDoubleEqualityComparer.GetHashCode(CurrentPureBlood);
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
		if (MaxAge != 0)
		{
			output.WriteRawTag(24);
			output.WriteInt32(MaxAge);
		}
		if (BrokenAge != 0)
		{
			output.WriteRawTag(32);
			output.WriteInt32(BrokenAge);
		}
		if (SuperAir != 0.0)
		{
			output.WriteRawTag(41);
			output.WriteDouble(SuperAir);
		}
		if (Perception != 0.0)
		{
			output.WriteRawTag(49);
			output.WriteDouble(Perception);
		}
		if (Physique != 0.0)
		{
			output.WriteRawTag(57);
			output.WriteDouble(Physique);
		}
		if (CurrentSuperAir != 0.0)
		{
			output.WriteRawTag(65);
			output.WriteDouble(CurrentSuperAir);
		}
		if (CurrentPerception != 0.0)
		{
			output.WriteRawTag(73);
			output.WriteDouble(CurrentPerception);
		}
		if (CurrentPhysique != 0.0)
		{
			output.WriteRawTag(81);
			output.WriteDouble(CurrentPhysique);
		}
		if (PureBlood != 0.0)
		{
			output.WriteRawTag(89);
			output.WriteDouble(PureBlood);
		}
		if (CurrentPureBlood != 0.0)
		{
			output.WriteRawTag(97);
			output.WriteDouble(CurrentPureBlood);
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
		if (MaxAge != 0)
		{
			num += 1 + CodedOutputStream.ComputeInt32Size(MaxAge);
		}
		if (BrokenAge != 0)
		{
			num += 1 + CodedOutputStream.ComputeInt32Size(BrokenAge);
		}
		if (SuperAir != 0.0)
		{
			num += 9;
		}
		if (Perception != 0.0)
		{
			num += 9;
		}
		if (Physique != 0.0)
		{
			num += 9;
		}
		if (CurrentSuperAir != 0.0)
		{
			num += 9;
		}
		if (CurrentPerception != 0.0)
		{
			num += 9;
		}
		if (CurrentPhysique != 0.0)
		{
			num += 9;
		}
		if (PureBlood != 0.0)
		{
			num += 9;
		}
		if (CurrentPureBlood != 0.0)
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
	public void MergeFrom(BrokenStateResp_1005012 other)
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
			if (other.MaxAge != 0)
			{
				MaxAge = other.MaxAge;
			}
			if (other.BrokenAge != 0)
			{
				BrokenAge = other.BrokenAge;
			}
			if (other.SuperAir != 0.0)
			{
				SuperAir = other.SuperAir;
			}
			if (other.Perception != 0.0)
			{
				Perception = other.Perception;
			}
			if (other.Physique != 0.0)
			{
				Physique = other.Physique;
			}
			if (other.CurrentSuperAir != 0.0)
			{
				CurrentSuperAir = other.CurrentSuperAir;
			}
			if (other.CurrentPerception != 0.0)
			{
				CurrentPerception = other.CurrentPerception;
			}
			if (other.CurrentPhysique != 0.0)
			{
				CurrentPhysique = other.CurrentPhysique;
			}
			if (other.PureBlood != 0.0)
			{
				PureBlood = other.PureBlood;
			}
			if (other.CurrentPureBlood != 0.0)
			{
				CurrentPureBlood = other.CurrentPureBlood;
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
				MaxAge = input.ReadInt32();
				break;
			case 32u:
				BrokenAge = input.ReadInt32();
				break;
			case 41u:
				SuperAir = input.ReadDouble();
				break;
			case 49u:
				Perception = input.ReadDouble();
				break;
			case 57u:
				Physique = input.ReadDouble();
				break;
			case 65u:
				CurrentSuperAir = input.ReadDouble();
				break;
			case 73u:
				CurrentPerception = input.ReadDouble();
				break;
			case 81u:
				CurrentPhysique = input.ReadDouble();
				break;
			case 89u:
				PureBlood = input.ReadDouble();
				break;
			case 97u:
				CurrentPureBlood = input.ReadDouble();
				break;
			}
		}
	}
}
