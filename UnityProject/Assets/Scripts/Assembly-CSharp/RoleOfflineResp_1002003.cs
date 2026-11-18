using System;
using System.Diagnostics;
using Google.Protobuf;
using Google.Protobuf.Collections;
using Google.Protobuf.Reflection;

public sealed class RoleOfflineResp_1002003 : IMessage<RoleOfflineResp_1002003>, IMessage, IEquatable<RoleOfflineResp_1002003>, IDeepCloneable<RoleOfflineResp_1002003>
{
	private static readonly MessageParser<RoleOfflineResp_1002003> _parser = new MessageParser<RoleOfflineResp_1002003>(() => new RoleOfflineResp_1002003());

	private UnknownFieldSet _unknownFields;

	public const int CodeFieldNumber = 1;

	private int code_;

	public const int IsSuccessFieldNumber = 2;

	private bool isSuccess_;

	public const int SuperAirFieldNumber = 3;

	private long superAir_;

	public const int PerceptionFieldNumber = 4;

	private long perception_;

	public const int PhysiqueFieldNumber = 5;

	private long physique_;

	public const int AgeFieldNumber = 6;

	private double age_;

	public const int OfflineTimeFieldNumber = 7;

	private int offlineTime_;

	public const int OfflineVigorFieldNumber = 8;

	private int offlineVigor_;

	public const int OfflineCleverFieldNumber = 9;

	private int offlineClever_;

	public const int IsFullTimeFieldNumber = 10;

	private bool isFullTime_;

	public const int OfflinePowerFieldNumber = 11;

	private int offlinePower_;

	[DebuggerNonUserCode]
	public static MessageParser<RoleOfflineResp_1002003> Parser => _parser;

	[DebuggerNonUserCode]
	public static MessageDescriptor Descriptor => RoleProtoReflection.Descriptor.MessageTypes[4];

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
	public double Age
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
	public int OfflineTime
	{
		get
		{
			return offlineTime_;
		}
		set
		{
			offlineTime_ = value;
		}
	}

	[DebuggerNonUserCode]
	public int OfflineVigor
	{
		get
		{
			return offlineVigor_;
		}
		set
		{
			offlineVigor_ = value;
		}
	}

	[DebuggerNonUserCode]
	public int OfflineClever
	{
		get
		{
			return offlineClever_;
		}
		set
		{
			offlineClever_ = value;
		}
	}

	[DebuggerNonUserCode]
	public bool IsFullTime
	{
		get
		{
			return isFullTime_;
		}
		set
		{
			isFullTime_ = value;
		}
	}

	[DebuggerNonUserCode]
	public int OfflinePower
	{
		get
		{
			return offlinePower_;
		}
		set
		{
			offlinePower_ = value;
		}
	}

	private MessageDescriptor pb_003A_003AGoogle_002EProtobuf_002EIMessage_002Eget_Descriptor()
	{
		return Descriptor;
	}

	[DebuggerNonUserCode]
	public RoleOfflineResp_1002003()
	{
	}

	[DebuggerNonUserCode]
	public RoleOfflineResp_1002003(RoleOfflineResp_1002003 other)
		: this()
	{
		code_ = other.code_;
		isSuccess_ = other.isSuccess_;
		superAir_ = other.superAir_;
		perception_ = other.perception_;
		physique_ = other.physique_;
		age_ = other.age_;
		offlineTime_ = other.offlineTime_;
		offlineVigor_ = other.offlineVigor_;
		offlineClever_ = other.offlineClever_;
		isFullTime_ = other.isFullTime_;
		offlinePower_ = other.offlinePower_;
		_unknownFields = UnknownFieldSet.Clone(other._unknownFields);
	}

	[DebuggerNonUserCode]
	public RoleOfflineResp_1002003 Clone()
	{
		return new RoleOfflineResp_1002003(this);
	}

	[DebuggerNonUserCode]
	public override bool Equals(object other)
	{
		return Equals(other as RoleOfflineResp_1002003);
	}

	[DebuggerNonUserCode]
	public bool Equals(RoleOfflineResp_1002003 other)
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
		if (Perception != other.Perception)
		{
			return false;
		}
		if (Physique != other.Physique)
		{
			return false;
		}
		if (!ProtobufEqualityComparers.BitwiseDoubleEqualityComparer.Equals(Age, other.Age))
		{
			return false;
		}
		if (OfflineTime != other.OfflineTime)
		{
			return false;
		}
		if (OfflineVigor != other.OfflineVigor)
		{
			return false;
		}
		if (OfflineClever != other.OfflineClever)
		{
			return false;
		}
		if (IsFullTime != other.IsFullTime)
		{
			return false;
		}
		if (OfflinePower != other.OfflinePower)
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
		if (Perception != 0L)
		{
			num ^= Perception.GetHashCode();
		}
		if (Physique != 0L)
		{
			num ^= Physique.GetHashCode();
		}
		if (Age != 0.0)
		{
			num ^= ProtobufEqualityComparers.BitwiseDoubleEqualityComparer.GetHashCode(Age);
		}
		if (OfflineTime != 0)
		{
			num ^= OfflineTime.GetHashCode();
		}
		if (OfflineVigor != 0)
		{
			num ^= OfflineVigor.GetHashCode();
		}
		if (OfflineClever != 0)
		{
			num ^= OfflineClever.GetHashCode();
		}
		if (IsFullTime)
		{
			num ^= IsFullTime.GetHashCode();
		}
		if (OfflinePower != 0)
		{
			num ^= OfflinePower.GetHashCode();
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
		if (Perception != 0L)
		{
			output.WriteRawTag(32);
			output.WriteInt64(Perception);
		}
		if (Physique != 0L)
		{
			output.WriteRawTag(40);
			output.WriteInt64(Physique);
		}
		if (Age != 0.0)
		{
			output.WriteRawTag(49);
			output.WriteDouble(Age);
		}
		if (OfflineTime != 0)
		{
			output.WriteRawTag(56);
			output.WriteInt32(OfflineTime);
		}
		if (OfflineVigor != 0)
		{
			output.WriteRawTag(64);
			output.WriteInt32(OfflineVigor);
		}
		if (OfflineClever != 0)
		{
			output.WriteRawTag(72);
			output.WriteInt32(OfflineClever);
		}
		if (IsFullTime)
		{
			output.WriteRawTag(80);
			output.WriteBool(IsFullTime);
		}
		if (OfflinePower != 0)
		{
			output.WriteRawTag(88);
			output.WriteInt32(OfflinePower);
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
		if (Perception != 0L)
		{
			num += 1 + CodedOutputStream.ComputeInt64Size(Perception);
		}
		if (Physique != 0L)
		{
			num += 1 + CodedOutputStream.ComputeInt64Size(Physique);
		}
		if (Age != 0.0)
		{
			num += 9;
		}
		if (OfflineTime != 0)
		{
			num += 1 + CodedOutputStream.ComputeInt32Size(OfflineTime);
		}
		if (OfflineVigor != 0)
		{
			num += 1 + CodedOutputStream.ComputeInt32Size(OfflineVigor);
		}
		if (OfflineClever != 0)
		{
			num += 1 + CodedOutputStream.ComputeInt32Size(OfflineClever);
		}
		if (IsFullTime)
		{
			num += 2;
		}
		if (OfflinePower != 0)
		{
			num += 1 + CodedOutputStream.ComputeInt32Size(OfflinePower);
		}
		if (_unknownFields != null)
		{
			num += _unknownFields.CalculateSize();
		}
		return num;
	}

	[DebuggerNonUserCode]
	public void MergeFrom(RoleOfflineResp_1002003 other)
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
			if (other.Perception != 0L)
			{
				Perception = other.Perception;
			}
			if (other.Physique != 0L)
			{
				Physique = other.Physique;
			}
			if (other.Age != 0.0)
			{
				Age = other.Age;
			}
			if (other.OfflineTime != 0)
			{
				OfflineTime = other.OfflineTime;
			}
			if (other.OfflineVigor != 0)
			{
				OfflineVigor = other.OfflineVigor;
			}
			if (other.OfflineClever != 0)
			{
				OfflineClever = other.OfflineClever;
			}
			if (other.IsFullTime)
			{
				IsFullTime = other.IsFullTime;
			}
			if (other.OfflinePower != 0)
			{
				OfflinePower = other.OfflinePower;
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
				Perception = input.ReadInt64();
				break;
			case 40u:
				Physique = input.ReadInt64();
				break;
			case 49u:
				Age = input.ReadDouble();
				break;
			case 56u:
				OfflineTime = input.ReadInt32();
				break;
			case 64u:
				OfflineVigor = input.ReadInt32();
				break;
			case 72u:
				OfflineClever = input.ReadInt32();
				break;
			case 80u:
				IsFullTime = input.ReadBool();
				break;
			case 88u:
				OfflinePower = input.ReadInt32();
				break;
			}
		}
	}
}
