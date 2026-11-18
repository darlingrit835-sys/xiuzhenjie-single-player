using System;
using System.Diagnostics;
using Google.Protobuf;
using Google.Protobuf.Reflection;

public sealed class RoleJoinDungeonReq_1010002 : IMessage<RoleJoinDungeonReq_1010002>, IMessage, IEquatable<RoleJoinDungeonReq_1010002>, IDeepCloneable<RoleJoinDungeonReq_1010002>
{
	private static readonly MessageParser<RoleJoinDungeonReq_1010002> _parser = new MessageParser<RoleJoinDungeonReq_1010002>(() => new RoleJoinDungeonReq_1010002());

	private UnknownFieldSet _unknownFields;

	public const int DungeonIdFieldNumber = 1;

	private string dungeonId_ = "";

	public const int MapIdFieldNumber = 2;

	private string mapId_ = "";

	[DebuggerNonUserCode]
	public static MessageParser<RoleJoinDungeonReq_1010002> Parser => _parser;

	[DebuggerNonUserCode]
	public static MessageDescriptor Descriptor => BattleProtoReflection.Descriptor.MessageTypes[2];

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
	public string MapId
	{
		get
		{
			return mapId_;
		}
		set
		{
			mapId_ = ProtoPreconditions.CheckNotNull(value, "value");
		}
	}

	private MessageDescriptor pb_003A_003AGoogle_002EProtobuf_002EIMessage_002Eget_Descriptor()
	{
		return Descriptor;
	}

	[DebuggerNonUserCode]
	public RoleJoinDungeonReq_1010002()
	{
	}

	[DebuggerNonUserCode]
	public RoleJoinDungeonReq_1010002(RoleJoinDungeonReq_1010002 other)
		: this()
	{
		dungeonId_ = other.dungeonId_;
		mapId_ = other.mapId_;
		_unknownFields = UnknownFieldSet.Clone(other._unknownFields);
	}

	[DebuggerNonUserCode]
	public RoleJoinDungeonReq_1010002 Clone()
	{
		return new RoleJoinDungeonReq_1010002(this);
	}

	[DebuggerNonUserCode]
	public override bool Equals(object other)
	{
		return Equals(other as RoleJoinDungeonReq_1010002);
	}

	[DebuggerNonUserCode]
	public bool Equals(RoleJoinDungeonReq_1010002 other)
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
		if (MapId != other.MapId)
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
		if (MapId.Length != 0)
		{
			num ^= MapId.GetHashCode();
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
		if (MapId.Length != 0)
		{
			output.WriteRawTag(18);
			output.WriteString(MapId);
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
		if (MapId.Length != 0)
		{
			num += 1 + CodedOutputStream.ComputeStringSize(MapId);
		}
		if (_unknownFields != null)
		{
			num += _unknownFields.CalculateSize();
		}
		return num;
	}

	[DebuggerNonUserCode]
	public void MergeFrom(RoleJoinDungeonReq_1010002 other)
	{
		if (other != null)
		{
			if (other.DungeonId.Length != 0)
			{
				DungeonId = other.DungeonId;
			}
			if (other.MapId.Length != 0)
			{
				MapId = other.MapId;
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
			case 18u:
				MapId = input.ReadString();
				break;
			}
		}
	}
}
