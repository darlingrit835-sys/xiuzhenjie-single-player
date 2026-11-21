using System;
using System.Diagnostics;
using Google.Protobuf;
using Google.Protobuf.Reflection;

public sealed class SetSkillReq_1009003 : IMessage<SetSkillReq_1009003>, IMessage, IEquatable<SetSkillReq_1009003>, IDeepCloneable<SetSkillReq_1009003>
{
	private static readonly MessageParser<SetSkillReq_1009003> _parser = new MessageParser<SetSkillReq_1009003>(() => new SetSkillReq_1009003());

	private UnknownFieldSet _unknownFields;

	public const int SkillTypeFieldNumber = 1;

	private int skillType_;

	public const int SkillIdFieldNumber = 2;

	private string skillId_ = "";

	public const int CombatSettingIdFieldNumber = 3;

	private int combatSettingId_;

	[DebuggerNonUserCode]
	public static MessageParser<SetSkillReq_1009003> Parser => _parser;

	[DebuggerNonUserCode]
	public static MessageDescriptor Descriptor => BattleSettingProtoReflection.Descriptor.MessageTypes[4];

	[DebuggerNonUserCode]
	MessageDescriptor IMessage.Descriptor => pb_003A_003AGoogle_002EProtobuf_002EIMessage_002Eget_Descriptor();

	[DebuggerNonUserCode]
	public int SkillType
	{
		get
		{
			return skillType_;
		}
		set
		{
			skillType_ = value;
		}
	}

	[DebuggerNonUserCode]
	public string SkillId
	{
		get
		{
			return skillId_;
		}
		set
		{
			skillId_ = ProtoPreconditions.CheckNotNull(value, "value");
		}
	}

	[DebuggerNonUserCode]
	public int CombatSettingId
	{
		get
		{
			return combatSettingId_;
		}
		set
		{
			combatSettingId_ = value;
		}
	}

	private MessageDescriptor pb_003A_003AGoogle_002EProtobuf_002EIMessage_002Eget_Descriptor()
	{
		return Descriptor;
	}

	[DebuggerNonUserCode]
	public SetSkillReq_1009003()
	{
	}

	[DebuggerNonUserCode]
	public SetSkillReq_1009003(SetSkillReq_1009003 other)
		: this()
	{
		skillType_ = other.skillType_;
		skillId_ = other.skillId_;
		combatSettingId_ = other.combatSettingId_;
		_unknownFields = UnknownFieldSet.Clone(other._unknownFields);
	}

	[DebuggerNonUserCode]
	public SetSkillReq_1009003 Clone()
	{
		return new SetSkillReq_1009003(this);
	}

	[DebuggerNonUserCode]
	public override bool Equals(object other)
	{
		return Equals(other as SetSkillReq_1009003);
	}

	[DebuggerNonUserCode]
	public bool Equals(SetSkillReq_1009003 other)
	{
		if (other == null)
		{
			return false;
		}
		if (other == this)
		{
			return true;
		}
		if (SkillType != other.SkillType)
		{
			return false;
		}
		if (SkillId != other.SkillId)
		{
			return false;
		}
		if (CombatSettingId != other.CombatSettingId)
		{
			return false;
		}
		return object.Equals(_unknownFields, other._unknownFields);
	}

	[DebuggerNonUserCode]
	public override int GetHashCode()
	{
		int num = 1;
		if (SkillType != 0)
		{
			num ^= SkillType.GetHashCode();
		}
		if (SkillId.Length != 0)
		{
			num ^= SkillId.GetHashCode();
		}
		if (CombatSettingId != 0)
		{
			num ^= CombatSettingId.GetHashCode();
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
		if (SkillType != 0)
		{
			output.WriteRawTag(8);
			output.WriteInt32(SkillType);
		}
		if (SkillId.Length != 0)
		{
			output.WriteRawTag(18);
			output.WriteString(SkillId);
		}
		if (CombatSettingId != 0)
		{
			output.WriteRawTag(24);
			output.WriteInt32(CombatSettingId);
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
		if (SkillType != 0)
		{
			num += 1 + CodedOutputStream.ComputeInt32Size(SkillType);
		}
		if (SkillId.Length != 0)
		{
			num += 1 + CodedOutputStream.ComputeStringSize(SkillId);
		}
		if (CombatSettingId != 0)
		{
			num += 1 + CodedOutputStream.ComputeInt32Size(CombatSettingId);
		}
		if (_unknownFields != null)
		{
			num += _unknownFields.CalculateSize();
		}
		return num;
	}

	[DebuggerNonUserCode]
	public void MergeFrom(SetSkillReq_1009003 other)
	{
		if (other != null)
		{
			if (other.SkillType != 0)
			{
				SkillType = other.SkillType;
			}
			if (other.SkillId.Length != 0)
			{
				SkillId = other.SkillId;
			}
			if (other.CombatSettingId != 0)
			{
				CombatSettingId = other.CombatSettingId;
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
				SkillType = input.ReadInt32();
				break;
			case 18u:
				SkillId = input.ReadString();
				break;
			case 24u:
				CombatSettingId = input.ReadInt32();
				break;
			}
		}
	}
}
