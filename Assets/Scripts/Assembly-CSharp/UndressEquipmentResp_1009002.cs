using System;
using System.Diagnostics;
using Google.Protobuf;
using Google.Protobuf.Collections;
using Google.Protobuf.Reflection;

public sealed class UndressEquipmentResp_1009002 : IMessage<UndressEquipmentResp_1009002>, IMessage, IEquatable<UndressEquipmentResp_1009002>, IDeepCloneable<UndressEquipmentResp_1009002>
{
	private static readonly MessageParser<UndressEquipmentResp_1009002> _parser = new MessageParser<UndressEquipmentResp_1009002>(() => new UndressEquipmentResp_1009002());

	private UnknownFieldSet _unknownFields;

	public const int CodeFieldNumber = 1;

	private int code_;

	public const int IsSuccessFieldNumber = 2;

	private bool isSuccess_;

	public const int CurrentPhysiqueDefenseFieldNumber = 3;

	private long currentPhysiqueDefense_;

	public const int CurrentPureBloodDefenseFieldNumber = 4;

	private int currentPureBloodDefense_;

	public const int CurrentMoveSpeedFieldNumber = 5;

	private double currentMoveSpeed_;

	public const int SuperAirRecoverValueFieldNumber = 6;

	private double superAirRecoverValue_;

	public const int DefenseRecoverValueFieldNumber = 7;

	private double defenseRecoverValue_;

	public const int SuperAirRecoverValueOnBattleFieldNumber = 8;

	private double superAirRecoverValueOnBattle_;

	public const int DefenseRecoverValueOnBattleFieldNumber = 9;

	private double defenseRecoverValueOnBattle_;

	public const int CanUsedPhysiqueFieldNumber = 10;

	private double canUsedPhysique_;

	[DebuggerNonUserCode]
	public static MessageParser<UndressEquipmentResp_1009002> Parser => _parser;

	[DebuggerNonUserCode]
	public static MessageDescriptor Descriptor => BattleSettingProtoReflection.Descriptor.MessageTypes[3];

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
	public long CurrentPhysiqueDefense
	{
		get
		{
			return currentPhysiqueDefense_;
		}
		set
		{
			currentPhysiqueDefense_ = value;
		}
	}

	[DebuggerNonUserCode]
	public int CurrentPureBloodDefense
	{
		get
		{
			return currentPureBloodDefense_;
		}
		set
		{
			currentPureBloodDefense_ = value;
		}
	}

	[DebuggerNonUserCode]
	public double CurrentMoveSpeed
	{
		get
		{
			return currentMoveSpeed_;
		}
		set
		{
			currentMoveSpeed_ = value;
		}
	}

	[DebuggerNonUserCode]
	public double SuperAirRecoverValue
	{
		get
		{
			return superAirRecoverValue_;
		}
		set
		{
			superAirRecoverValue_ = value;
		}
	}

	[DebuggerNonUserCode]
	public double DefenseRecoverValue
	{
		get
		{
			return defenseRecoverValue_;
		}
		set
		{
			defenseRecoverValue_ = value;
		}
	}

	[DebuggerNonUserCode]
	public double SuperAirRecoverValueOnBattle
	{
		get
		{
			return superAirRecoverValueOnBattle_;
		}
		set
		{
			superAirRecoverValueOnBattle_ = value;
		}
	}

	[DebuggerNonUserCode]
	public double DefenseRecoverValueOnBattle
	{
		get
		{
			return defenseRecoverValueOnBattle_;
		}
		set
		{
			defenseRecoverValueOnBattle_ = value;
		}
	}

	[DebuggerNonUserCode]
	public double CanUsedPhysique
	{
		get
		{
			return canUsedPhysique_;
		}
		set
		{
			canUsedPhysique_ = value;
		}
	}

	private MessageDescriptor pb_003A_003AGoogle_002EProtobuf_002EIMessage_002Eget_Descriptor()
	{
		return Descriptor;
	}

	[DebuggerNonUserCode]
	public UndressEquipmentResp_1009002()
	{
	}

	[DebuggerNonUserCode]
	public UndressEquipmentResp_1009002(UndressEquipmentResp_1009002 other)
		: this()
	{
		code_ = other.code_;
		isSuccess_ = other.isSuccess_;
		currentPhysiqueDefense_ = other.currentPhysiqueDefense_;
		currentPureBloodDefense_ = other.currentPureBloodDefense_;
		currentMoveSpeed_ = other.currentMoveSpeed_;
		superAirRecoverValue_ = other.superAirRecoverValue_;
		defenseRecoverValue_ = other.defenseRecoverValue_;
		superAirRecoverValueOnBattle_ = other.superAirRecoverValueOnBattle_;
		defenseRecoverValueOnBattle_ = other.defenseRecoverValueOnBattle_;
		canUsedPhysique_ = other.canUsedPhysique_;
		_unknownFields = UnknownFieldSet.Clone(other._unknownFields);
	}

	[DebuggerNonUserCode]
	public UndressEquipmentResp_1009002 Clone()
	{
		return new UndressEquipmentResp_1009002(this);
	}

	[DebuggerNonUserCode]
	public override bool Equals(object other)
	{
		return Equals(other as UndressEquipmentResp_1009002);
	}

	[DebuggerNonUserCode]
	public bool Equals(UndressEquipmentResp_1009002 other)
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
		if (CurrentPhysiqueDefense != other.CurrentPhysiqueDefense)
		{
			return false;
		}
		if (CurrentPureBloodDefense != other.CurrentPureBloodDefense)
		{
			return false;
		}
		if (!ProtobufEqualityComparers.BitwiseDoubleEqualityComparer.Equals(CurrentMoveSpeed, other.CurrentMoveSpeed))
		{
			return false;
		}
		if (!ProtobufEqualityComparers.BitwiseDoubleEqualityComparer.Equals(SuperAirRecoverValue, other.SuperAirRecoverValue))
		{
			return false;
		}
		if (!ProtobufEqualityComparers.BitwiseDoubleEqualityComparer.Equals(DefenseRecoverValue, other.DefenseRecoverValue))
		{
			return false;
		}
		if (!ProtobufEqualityComparers.BitwiseDoubleEqualityComparer.Equals(SuperAirRecoverValueOnBattle, other.SuperAirRecoverValueOnBattle))
		{
			return false;
		}
		if (!ProtobufEqualityComparers.BitwiseDoubleEqualityComparer.Equals(DefenseRecoverValueOnBattle, other.DefenseRecoverValueOnBattle))
		{
			return false;
		}
		if (!ProtobufEqualityComparers.BitwiseDoubleEqualityComparer.Equals(CanUsedPhysique, other.CanUsedPhysique))
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
		if (CurrentPhysiqueDefense != 0L)
		{
			num ^= CurrentPhysiqueDefense.GetHashCode();
		}
		if (CurrentPureBloodDefense != 0)
		{
			num ^= CurrentPureBloodDefense.GetHashCode();
		}
		if (CurrentMoveSpeed != 0.0)
		{
			num ^= ProtobufEqualityComparers.BitwiseDoubleEqualityComparer.GetHashCode(CurrentMoveSpeed);
		}
		if (SuperAirRecoverValue != 0.0)
		{
			num ^= ProtobufEqualityComparers.BitwiseDoubleEqualityComparer.GetHashCode(SuperAirRecoverValue);
		}
		if (DefenseRecoverValue != 0.0)
		{
			num ^= ProtobufEqualityComparers.BitwiseDoubleEqualityComparer.GetHashCode(DefenseRecoverValue);
		}
		if (SuperAirRecoverValueOnBattle != 0.0)
		{
			num ^= ProtobufEqualityComparers.BitwiseDoubleEqualityComparer.GetHashCode(SuperAirRecoverValueOnBattle);
		}
		if (DefenseRecoverValueOnBattle != 0.0)
		{
			num ^= ProtobufEqualityComparers.BitwiseDoubleEqualityComparer.GetHashCode(DefenseRecoverValueOnBattle);
		}
		if (CanUsedPhysique != 0.0)
		{
			num ^= ProtobufEqualityComparers.BitwiseDoubleEqualityComparer.GetHashCode(CanUsedPhysique);
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
		if (CurrentPhysiqueDefense != 0L)
		{
			output.WriteRawTag(24);
			output.WriteInt64(CurrentPhysiqueDefense);
		}
		if (CurrentPureBloodDefense != 0)
		{
			output.WriteRawTag(32);
			output.WriteInt32(CurrentPureBloodDefense);
		}
		if (CurrentMoveSpeed != 0.0)
		{
			output.WriteRawTag(41);
			output.WriteDouble(CurrentMoveSpeed);
		}
		if (SuperAirRecoverValue != 0.0)
		{
			output.WriteRawTag(49);
			output.WriteDouble(SuperAirRecoverValue);
		}
		if (DefenseRecoverValue != 0.0)
		{
			output.WriteRawTag(57);
			output.WriteDouble(DefenseRecoverValue);
		}
		if (SuperAirRecoverValueOnBattle != 0.0)
		{
			output.WriteRawTag(65);
			output.WriteDouble(SuperAirRecoverValueOnBattle);
		}
		if (DefenseRecoverValueOnBattle != 0.0)
		{
			output.WriteRawTag(73);
			output.WriteDouble(DefenseRecoverValueOnBattle);
		}
		if (CanUsedPhysique != 0.0)
		{
			output.WriteRawTag(81);
			output.WriteDouble(CanUsedPhysique);
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
		if (CurrentPhysiqueDefense != 0L)
		{
			num += 1 + CodedOutputStream.ComputeInt64Size(CurrentPhysiqueDefense);
		}
		if (CurrentPureBloodDefense != 0)
		{
			num += 1 + CodedOutputStream.ComputeInt32Size(CurrentPureBloodDefense);
		}
		if (CurrentMoveSpeed != 0.0)
		{
			num += 9;
		}
		if (SuperAirRecoverValue != 0.0)
		{
			num += 9;
		}
		if (DefenseRecoverValue != 0.0)
		{
			num += 9;
		}
		if (SuperAirRecoverValueOnBattle != 0.0)
		{
			num += 9;
		}
		if (DefenseRecoverValueOnBattle != 0.0)
		{
			num += 9;
		}
		if (CanUsedPhysique != 0.0)
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
	public void MergeFrom(UndressEquipmentResp_1009002 other)
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
			if (other.CurrentPhysiqueDefense != 0L)
			{
				CurrentPhysiqueDefense = other.CurrentPhysiqueDefense;
			}
			if (other.CurrentPureBloodDefense != 0)
			{
				CurrentPureBloodDefense = other.CurrentPureBloodDefense;
			}
			if (other.CurrentMoveSpeed != 0.0)
			{
				CurrentMoveSpeed = other.CurrentMoveSpeed;
			}
			if (other.SuperAirRecoverValue != 0.0)
			{
				SuperAirRecoverValue = other.SuperAirRecoverValue;
			}
			if (other.DefenseRecoverValue != 0.0)
			{
				DefenseRecoverValue = other.DefenseRecoverValue;
			}
			if (other.SuperAirRecoverValueOnBattle != 0.0)
			{
				SuperAirRecoverValueOnBattle = other.SuperAirRecoverValueOnBattle;
			}
			if (other.DefenseRecoverValueOnBattle != 0.0)
			{
				DefenseRecoverValueOnBattle = other.DefenseRecoverValueOnBattle;
			}
			if (other.CanUsedPhysique != 0.0)
			{
				CanUsedPhysique = other.CanUsedPhysique;
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
				CurrentPhysiqueDefense = input.ReadInt64();
				break;
			case 32u:
				CurrentPureBloodDefense = input.ReadInt32();
				break;
			case 41u:
				CurrentMoveSpeed = input.ReadDouble();
				break;
			case 49u:
				SuperAirRecoverValue = input.ReadDouble();
				break;
			case 57u:
				DefenseRecoverValue = input.ReadDouble();
				break;
			case 65u:
				SuperAirRecoverValueOnBattle = input.ReadDouble();
				break;
			case 73u:
				DefenseRecoverValueOnBattle = input.ReadDouble();
				break;
			case 81u:
				CanUsedPhysique = input.ReadDouble();
				break;
			}
		}
	}
}
