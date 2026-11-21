using System;
using System.Diagnostics;
using Google.Protobuf;
using Google.Protobuf.Reflection;

public sealed class BattleRoleEnterReq_8001001 : IMessage<BattleRoleEnterReq_8001001>, IMessage, IEquatable<BattleRoleEnterReq_8001001>, IDeepCloneable<BattleRoleEnterReq_8001001>
{
	private static readonly MessageParser<BattleRoleEnterReq_8001001> _parser = new MessageParser<BattleRoleEnterReq_8001001>(() => new BattleRoleEnterReq_8001001());

	private UnknownFieldSet _unknownFields;

	public const int WorldVersionFieldNumber = 3;
	public int WorldVersion { get; set; }


	public const int DungeonIdFieldNumber = 1;

	private string dungeonId_ = "";

	public const int RoleIdFieldNumber = 2;

	private string roleId_ = "";

	[DebuggerNonUserCode]
	public static MessageParser<BattleRoleEnterReq_8001001> Parser => _parser;

	[DebuggerNonUserCode]
	public static MessageDescriptor Descriptor => ClientBattleProtoReflection.Descriptor.MessageTypes[0];

	[DebuggerNonUserCode]
	MessageDescriptor IMessage.Descriptor => pb_003A_003AGoogle_002EProtobuf_002EIMessage_002Eget_Descriptor();

	[DebuggerNonUserCode]
	public string DungeonId
	{
		get
		{
			return dungeonId_;
		}
		set
		{
			dungeonId_ = ProtoPreconditions.CheckNotNull(value, "value");
		}
	}

	[DebuggerNonUserCode]
	public string RoleId
	{
		get
		{
			return roleId_;
		}
		set
		{
			roleId_ = ProtoPreconditions.CheckNotNull(value, "value");
		}
	}

	private MessageDescriptor pb_003A_003AGoogle_002EProtobuf_002EIMessage_002Eget_Descriptor()
	{
		return Descriptor;
	}

	[DebuggerNonUserCode]
	public BattleRoleEnterReq_8001001()
	{
	}

	[DebuggerNonUserCode]
	public BattleRoleEnterReq_8001001(BattleRoleEnterReq_8001001 other)
		: this()
	{
		dungeonId_ = other.dungeonId_;
		roleId_ = other.roleId_;
		WorldVersion = other.WorldVersion;
		_unknownFields = UnknownFieldSet.Clone(other._unknownFields);
	}

	[DebuggerNonUserCode]
	public BattleRoleEnterReq_8001001 Clone()
	{
		return new BattleRoleEnterReq_8001001(this);
	}

	[DebuggerNonUserCode]
	public override bool Equals(object other)
	{
		return Equals(other as BattleRoleEnterReq_8001001);
	}

	[DebuggerNonUserCode]
	public bool Equals(BattleRoleEnterReq_8001001 other)
	{
		if (other == null)
		{
			return false;
		}
		if (other == this)
		{
			return true;
		}
		if (DungeonId != other.DungeonId)
		{
			return false;
		}
		if (RoleId != other.RoleId)
		{
			return false;
		}
		if (WorldVersion != other.WorldVersion) return false;
		return object.Equals(_unknownFields, other._unknownFields);
	}

	[DebuggerNonUserCode]
	public override int GetHashCode()
	{
		int num = 1;
		if (DungeonId.Length != 0)
		{
			num ^= DungeonId.GetHashCode();
		}
		if (RoleId.Length != 0)
		{
			num ^= RoleId.GetHashCode();
		}
		if (WorldVersion != 0) num ^= WorldVersion.GetHashCode();
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
		if (DungeonId.Length != 0)
		{
			output.WriteRawTag(10);
			output.WriteString(DungeonId);
		}
		if (RoleId.Length != 0)
		{
			output.WriteRawTag(18);
			output.WriteString(RoleId);
		}
		if (WorldVersion != 0)
		{
			output.WriteRawTag(24);
			output.WriteInt32(WorldVersion);
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
		if (DungeonId.Length != 0)
		{
			num += 1 + CodedOutputStream.ComputeStringSize(DungeonId);
		}
		if (RoleId.Length != 0)
		{
			num += 1 + CodedOutputStream.ComputeStringSize(RoleId);
		}
		if (WorldVersion != 0) num += 1 + CodedOutputStream.ComputeInt32Size(WorldVersion);
		if (_unknownFields != null)
		{
			num += _unknownFields.CalculateSize();
		}
		return num;
	}

	[DebuggerNonUserCode]
	public void MergeFrom(BattleRoleEnterReq_8001001 other)
	{
		if (other != null)
		{
			if (other.DungeonId.Length != 0)
			{
				DungeonId = other.DungeonId;
			}
			if (other.RoleId.Length != 0)
			{
				RoleId = other.RoleId;
			}
			if (other.WorldVersion != 0) WorldVersion = other.WorldVersion;
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
			case 24u:
				WorldVersion = input.ReadInt32();
				break;
			default:
				_unknownFields = UnknownFieldSet.MergeFieldFrom(_unknownFields, input);
				break;
			case 10u:
				DungeonId = input.ReadString();
				break;
			case 18u:
				RoleId = input.ReadString();
				break;
			}
		}
	}
}
