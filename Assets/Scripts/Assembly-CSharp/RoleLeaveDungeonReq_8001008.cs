using System;
using System.Diagnostics;
using Google.Protobuf;
using Google.Protobuf.Reflection;

public sealed class RoleLeaveDungeonReq_8001008 : IMessage<RoleLeaveDungeonReq_8001008>, IMessage, IEquatable<RoleLeaveDungeonReq_8001008>, IDeepCloneable<RoleLeaveDungeonReq_8001008>
{
	private static readonly MessageParser<RoleLeaveDungeonReq_8001008> _parser = new MessageParser<RoleLeaveDungeonReq_8001008>(() => new RoleLeaveDungeonReq_8001008());

	private UnknownFieldSet _unknownFields;

	public const int DungeonIdFieldNumber = 1;

	private string dungeonId_ = "";

	public const int RoleIndexFieldNumber = 2;

	private int roleIndex_;

	[DebuggerNonUserCode]
	public static MessageParser<RoleLeaveDungeonReq_8001008> Parser => _parser;

	[DebuggerNonUserCode]
	public static MessageDescriptor Descriptor => ClientBattleProtoReflection.Descriptor.MessageTypes[9];

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

	private MessageDescriptor pb_003A_003AGoogle_002EProtobuf_002EIMessage_002Eget_Descriptor()
	{
		return Descriptor;
	}

	[DebuggerNonUserCode]
	public RoleLeaveDungeonReq_8001008()
	{
	}

	[DebuggerNonUserCode]
	public RoleLeaveDungeonReq_8001008(RoleLeaveDungeonReq_8001008 other)
		: this()
	{
		dungeonId_ = other.dungeonId_;
		roleIndex_ = other.roleIndex_;
		_unknownFields = UnknownFieldSet.Clone(other._unknownFields);
	}

	[DebuggerNonUserCode]
	public RoleLeaveDungeonReq_8001008 Clone()
	{
		return new RoleLeaveDungeonReq_8001008(this);
	}

	[DebuggerNonUserCode]
	public override bool Equals(object other)
	{
		return Equals(other as RoleLeaveDungeonReq_8001008);
	}

	[DebuggerNonUserCode]
	public bool Equals(RoleLeaveDungeonReq_8001008 other)
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
		if (RoleIndex != other.RoleIndex)
		{
			return false;
		}
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
		if (RoleIndex != 0)
		{
			num ^= RoleIndex.GetHashCode();
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
		if (DungeonId.Length != 0)
		{
			output.WriteRawTag(10);
			output.WriteString(DungeonId);
		}
		if (RoleIndex != 0)
		{
			output.WriteRawTag(16);
			output.WriteInt32(RoleIndex);
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
		if (RoleIndex != 0)
		{
			num += 1 + CodedOutputStream.ComputeInt32Size(RoleIndex);
		}
		if (_unknownFields != null)
		{
			num += _unknownFields.CalculateSize();
		}
		return num;
	}

	[DebuggerNonUserCode]
	public void MergeFrom(RoleLeaveDungeonReq_8001008 other)
	{
		if (other != null)
		{
			if (other.DungeonId.Length != 0)
			{
				DungeonId = other.DungeonId;
			}
			if (other.RoleIndex != 0)
			{
				RoleIndex = other.RoleIndex;
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
			case 10u:
				DungeonId = input.ReadString();
				break;
			case 16u:
				RoleIndex = input.ReadInt32();
				break;
			}
		}
	}
}
