using System;
using System.Diagnostics;
using Google.Protobuf;
using Google.Protobuf.Reflection;

public sealed class MonsterGetDamageReq_8001006 : IMessage<MonsterGetDamageReq_8001006>, IMessage, IEquatable<MonsterGetDamageReq_8001006>, IDeepCloneable<MonsterGetDamageReq_8001006>
{
	private static readonly MessageParser<MonsterGetDamageReq_8001006> _parser = new MessageParser<MonsterGetDamageReq_8001006>(() => new MonsterGetDamageReq_8001006());

	private UnknownFieldSet _unknownFields;

	public const int MonsterIndexFieldNumber = 1;

	private int monsterIndex_;

	public const int RoleIndexFieldNumber = 2;

	private int roleIndex_;

	public const int SkillIndexFieldNumber = 3;

	private int skillIndex_;

	public const int DamageFieldNumber = 4;

	private int damage_;

	[DebuggerNonUserCode]
	public static MessageParser<MonsterGetDamageReq_8001006> Parser => _parser;

	[DebuggerNonUserCode]
	public static MessageDescriptor Descriptor => ClientBattleProtoReflection.Descriptor.MessageTypes[5];

	[DebuggerNonUserCode]
	MessageDescriptor IMessage.Descriptor => pb_003A_003AGoogle_002EProtobuf_002EIMessage_002Eget_Descriptor();

	[DebuggerNonUserCode]
	public int MonsterIndex
	{
		get
		{
			return monsterIndex_;
		}
		set
		{
			monsterIndex_ = value;
		}
	}

	[DebuggerNonUserCode]
	public int RoleIndex
	{
		get
		{
			return roleIndex_;
		}
		set
		{
			roleIndex_ = value;
		}
	}

	[DebuggerNonUserCode]
	public int SkillIndex
	{
		get
		{
			return skillIndex_;
		}
		set
		{
			skillIndex_ = value;
		}
	}

	[DebuggerNonUserCode]
	public int Damage
	{
		get
		{
			return damage_;
		}
		set
		{
			damage_ = value;
		}
	}

	private MessageDescriptor pb_003A_003AGoogle_002EProtobuf_002EIMessage_002Eget_Descriptor()
	{
		return Descriptor;
	}

	[DebuggerNonUserCode]
	public MonsterGetDamageReq_8001006()
	{
	}

	[DebuggerNonUserCode]
	public MonsterGetDamageReq_8001006(MonsterGetDamageReq_8001006 other)
		: this()
	{
		monsterIndex_ = other.monsterIndex_;
		roleIndex_ = other.roleIndex_;
		skillIndex_ = other.skillIndex_;
		damage_ = other.damage_;
		_unknownFields = UnknownFieldSet.Clone(other._unknownFields);
	}

	[DebuggerNonUserCode]
	public MonsterGetDamageReq_8001006 Clone()
	{
		return new MonsterGetDamageReq_8001006(this);
	}

	[DebuggerNonUserCode]
	public override bool Equals(object other)
	{
		return Equals(other as MonsterGetDamageReq_8001006);
	}

	[DebuggerNonUserCode]
	public bool Equals(MonsterGetDamageReq_8001006 other)
	{
		if (other == null)
		{
			return false;
		}
		if (other == this)
		{
			return true;
		}
		if (MonsterIndex != other.MonsterIndex)
		{
			return false;
		}
		if (RoleIndex != other.RoleIndex)
		{
			return false;
		}
		if (SkillIndex != other.SkillIndex)
		{
			return false;
		}
		if (Damage != other.Damage)
		{
			return false;
		}
		return object.Equals(_unknownFields, other._unknownFields);
	}

	[DebuggerNonUserCode]
	public override int GetHashCode()
	{
		int num = 1;
		if (MonsterIndex != 0)
		{
			num ^= MonsterIndex.GetHashCode();
		}
		if (RoleIndex != 0)
		{
			num ^= RoleIndex.GetHashCode();
		}
		if (SkillIndex != 0)
		{
			num ^= SkillIndex.GetHashCode();
		}
		if (Damage != 0)
		{
			num ^= Damage.GetHashCode();
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
		if (MonsterIndex != 0)
		{
			output.WriteRawTag(8);
			output.WriteInt32(MonsterIndex);
		}
		if (RoleIndex != 0)
		{
			output.WriteRawTag(16);
			output.WriteInt32(RoleIndex);
		}
		if (SkillIndex != 0)
		{
			output.WriteRawTag(24);
			output.WriteInt32(SkillIndex);
		}
		if (Damage != 0)
		{
			output.WriteRawTag(32);
			output.WriteInt32(Damage);
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
		if (MonsterIndex != 0)
		{
			num += 1 + CodedOutputStream.ComputeInt32Size(MonsterIndex);
		}
		if (RoleIndex != 0)
		{
			num += 1 + CodedOutputStream.ComputeInt32Size(RoleIndex);
		}
		if (SkillIndex != 0)
		{
			num += 1 + CodedOutputStream.ComputeInt32Size(SkillIndex);
		}
		if (Damage != 0)
		{
			num += 1 + CodedOutputStream.ComputeInt32Size(Damage);
		}
		if (_unknownFields != null)
		{
			num += _unknownFields.CalculateSize();
		}
		return num;
	}

	[DebuggerNonUserCode]
	public void MergeFrom(MonsterGetDamageReq_8001006 other)
	{
		if (other != null)
		{
			if (other.MonsterIndex != 0)
			{
				MonsterIndex = other.MonsterIndex;
			}
			if (other.RoleIndex != 0)
			{
				RoleIndex = other.RoleIndex;
			}
			if (other.SkillIndex != 0)
			{
				SkillIndex = other.SkillIndex;
			}
			if (other.Damage != 0)
			{
				Damage = other.Damage;
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
				MonsterIndex = input.ReadInt32();
				break;
			case 16u:
				RoleIndex = input.ReadInt32();
				break;
			case 24u:
				SkillIndex = input.ReadInt32();
				break;
			case 32u:
				Damage = input.ReadInt32();
				break;
			}
		}
	}
}
