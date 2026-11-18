using System;
using System.Diagnostics;
using Google.Protobuf;
using Google.Protobuf.Reflection;

public sealed class BattleRoleUseSkillReq_8001005 : IMessage<BattleRoleUseSkillReq_8001005>, IMessage, IEquatable<BattleRoleUseSkillReq_8001005>, IDeepCloneable<BattleRoleUseSkillReq_8001005>
{
	private static readonly MessageParser<BattleRoleUseSkillReq_8001005> _parser = new MessageParser<BattleRoleUseSkillReq_8001005>(() => new BattleRoleUseSkillReq_8001005());

	private UnknownFieldSet _unknownFields;

	public const int RoleIndexFieldNumber = 1;

	private int roleIndex_;

	public const int SkillIndexFieldNumber = 2;

	private int skillIndex_;

	public const int TargetObjectIndexFieldNumber = 3;

	private int targetObjectIndex_;

	[DebuggerNonUserCode]
	public static MessageParser<BattleRoleUseSkillReq_8001005> Parser => _parser;

	[DebuggerNonUserCode]
	public static MessageDescriptor Descriptor => ClientBattleProtoReflection.Descriptor.MessageTypes[3];

	[DebuggerNonUserCode]
	MessageDescriptor IMessage.Descriptor => pb_003A_003AGoogle_002EProtobuf_002EIMessage_002Eget_Descriptor();

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
	public int TargetObjectIndex
	{
		get
		{
			return targetObjectIndex_;
		}
		set
		{
			targetObjectIndex_ = value;
		}
	}

	private MessageDescriptor pb_003A_003AGoogle_002EProtobuf_002EIMessage_002Eget_Descriptor()
	{
		return Descriptor;
	}

	[DebuggerNonUserCode]
	public BattleRoleUseSkillReq_8001005()
	{
	}

	[DebuggerNonUserCode]
	public BattleRoleUseSkillReq_8001005(BattleRoleUseSkillReq_8001005 other)
		: this()
	{
		roleIndex_ = other.roleIndex_;
		skillIndex_ = other.skillIndex_;
		targetObjectIndex_ = other.targetObjectIndex_;
		_unknownFields = UnknownFieldSet.Clone(other._unknownFields);
	}

	[DebuggerNonUserCode]
	public BattleRoleUseSkillReq_8001005 Clone()
	{
		return new BattleRoleUseSkillReq_8001005(this);
	}

	[DebuggerNonUserCode]
	public override bool Equals(object other)
	{
		return Equals(other as BattleRoleUseSkillReq_8001005);
	}

	[DebuggerNonUserCode]
	public bool Equals(BattleRoleUseSkillReq_8001005 other)
	{
		if (other == null)
		{
			return false;
		}
		if (other == this)
		{
			return true;
		}
		if (RoleIndex != other.RoleIndex)
		{
			return false;
		}
		if (SkillIndex != other.SkillIndex)
		{
			return false;
		}
		if (TargetObjectIndex != other.TargetObjectIndex)
		{
			return false;
		}
		return object.Equals(_unknownFields, other._unknownFields);
	}

	[DebuggerNonUserCode]
	public override int GetHashCode()
	{
		int num = 1;
		if (RoleIndex != 0)
		{
			num ^= RoleIndex.GetHashCode();
		}
		if (SkillIndex != 0)
		{
			num ^= SkillIndex.GetHashCode();
		}
		if (TargetObjectIndex != 0)
		{
			num ^= TargetObjectIndex.GetHashCode();
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
		if (RoleIndex != 0)
		{
			output.WriteRawTag(8);
			output.WriteInt32(RoleIndex);
		}
		if (SkillIndex != 0)
		{
			output.WriteRawTag(16);
			output.WriteInt32(SkillIndex);
		}
		if (TargetObjectIndex != 0)
		{
			output.WriteRawTag(24);
			output.WriteInt32(TargetObjectIndex);
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
		if (RoleIndex != 0)
		{
			num += 1 + CodedOutputStream.ComputeInt32Size(RoleIndex);
		}
		if (SkillIndex != 0)
		{
			num += 1 + CodedOutputStream.ComputeInt32Size(SkillIndex);
		}
		if (TargetObjectIndex != 0)
		{
			num += 1 + CodedOutputStream.ComputeInt32Size(TargetObjectIndex);
		}
		if (_unknownFields != null)
		{
			num += _unknownFields.CalculateSize();
		}
		return num;
	}

	[DebuggerNonUserCode]
	public void MergeFrom(BattleRoleUseSkillReq_8001005 other)
	{
		if (other != null)
		{
			if (other.RoleIndex != 0)
			{
				RoleIndex = other.RoleIndex;
			}
			if (other.SkillIndex != 0)
			{
				SkillIndex = other.SkillIndex;
			}
			if (other.TargetObjectIndex != 0)
			{
				TargetObjectIndex = other.TargetObjectIndex;
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
				RoleIndex = input.ReadInt32();
				break;
			case 16u:
				SkillIndex = input.ReadInt32();
				break;
			case 24u:
				TargetObjectIndex = input.ReadInt32();
				break;
			}
		}
	}
}
