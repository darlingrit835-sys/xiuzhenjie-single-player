using System;
using System.Diagnostics;
using Google.Protobuf;
using Google.Protobuf.Collections;
using Google.Protobuf.Reflection;

public sealed class RoleCreateReq_1002001 : IMessage<RoleCreateReq_1002001>, IMessage, IEquatable<RoleCreateReq_1002001>, IDeepCloneable<RoleCreateReq_1002001>
{
	private static readonly MessageParser<RoleCreateReq_1002001> _parser = new MessageParser<RoleCreateReq_1002001>(() => new RoleCreateReq_1002001());

	private UnknownFieldSet _unknownFields;

	public const int UserIdFieldNumber = 1;

	private string userId_ = "";

	public const int ClientIpFieldNumber = 2;

	private string clientIp_ = "";

	public const int ServerIdFieldNumber = 3;

	private string serverId_ = "";

	public const int MapIdFieldNumber = 4;

	private string mapId_ = "";

	public const int SexFieldNumber = 5;

	private bool sex_;

	public const int HeadPortraitIdFieldNumber = 6;

	private int headPortraitId_;

	public const int NameFieldNumber = 7;

	private string name_ = "";

	public const int GoldFieldNumber = 8;

	private double gold_;

	public const int WoodFieldNumber = 9;

	private double wood_;

	public const int WaterFieldNumber = 10;

	private double water_;

	public const int FireFieldNumber = 11;

	private double fire_;

	public const int EarthFieldNumber = 12;

	private double earth_;

	[DebuggerNonUserCode]
	public static MessageParser<RoleCreateReq_1002001> Parser => _parser;

	[DebuggerNonUserCode]
	public static MessageDescriptor Descriptor => RoleProtoReflection.Descriptor.MessageTypes[0];

	[DebuggerNonUserCode]
	MessageDescriptor IMessage.Descriptor => pb_003A_003AGoogle_002EProtobuf_002EIMessage_002Eget_Descriptor();

	[DebuggerNonUserCode]
	public string UserId
	{
		get
		{
			return userId_;
		}
		set
		{
			userId_ = ProtoPreconditions.CheckNotNull(value, "value");
		}
	}

	[DebuggerNonUserCode]
	public string ClientIp
	{
		get
		{
			return clientIp_;
		}
		set
		{
			clientIp_ = ProtoPreconditions.CheckNotNull(value, "value");
		}
	}

	[DebuggerNonUserCode]
	public string ServerId
	{
		get
		{
			return serverId_;
		}
		set
		{
			serverId_ = ProtoPreconditions.CheckNotNull(value, "value");
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

	[DebuggerNonUserCode]
	public bool Sex
	{
		get
		{
			return sex_;
		}
		set
		{
			sex_ = value;
		}
	}

	[DebuggerNonUserCode]
	public int HeadPortraitId
	{
		get
		{
			return headPortraitId_;
		}
		set
		{
			headPortraitId_ = value;
		}
	}

	[DebuggerNonUserCode]
	public string Name
	{
		get
		{
			return name_;
		}
		set
		{
			name_ = ProtoPreconditions.CheckNotNull(value, "value");
		}
	}

	[DebuggerNonUserCode]
	public double Gold
	{
		get
		{
			return gold_;
		}
		set
		{
			gold_ = value;
		}
	}

	[DebuggerNonUserCode]
	public double Wood
	{
		get
		{
			return wood_;
		}
		set
		{
			wood_ = value;
		}
	}

	[DebuggerNonUserCode]
	public double Water
	{
		get
		{
			return water_;
		}
		set
		{
			water_ = value;
		}
	}

	[DebuggerNonUserCode]
	public double Fire
	{
		get
		{
			return fire_;
		}
		set
		{
			fire_ = value;
		}
	}

	[DebuggerNonUserCode]
	public double Earth
	{
		get
		{
			return earth_;
		}
		set
		{
			earth_ = value;
		}
	}

	private MessageDescriptor pb_003A_003AGoogle_002EProtobuf_002EIMessage_002Eget_Descriptor()
	{
		return Descriptor;
	}

	[DebuggerNonUserCode]
	public RoleCreateReq_1002001()
	{
	}

	[DebuggerNonUserCode]
	public RoleCreateReq_1002001(RoleCreateReq_1002001 other)
		: this()
	{
		userId_ = other.userId_;
		clientIp_ = other.clientIp_;
		serverId_ = other.serverId_;
		mapId_ = other.mapId_;
		sex_ = other.sex_;
		headPortraitId_ = other.headPortraitId_;
		name_ = other.name_;
		gold_ = other.gold_;
		wood_ = other.wood_;
		water_ = other.water_;
		fire_ = other.fire_;
		earth_ = other.earth_;
		_unknownFields = UnknownFieldSet.Clone(other._unknownFields);
	}

	[DebuggerNonUserCode]
	public RoleCreateReq_1002001 Clone()
	{
		return new RoleCreateReq_1002001(this);
	}

	[DebuggerNonUserCode]
	public override bool Equals(object other)
	{
		return Equals(other as RoleCreateReq_1002001);
	}

	[DebuggerNonUserCode]
	public bool Equals(RoleCreateReq_1002001 other)
	{
		if (other == null)
		{
			return false;
		}
		if (other == this)
		{
			return true;
		}
		if (UserId != other.UserId)
		{
			return false;
		}
		if (ClientIp != other.ClientIp)
		{
			return false;
		}
		if (ServerId != other.ServerId)
		{
			return false;
		}
		if (MapId != other.MapId)
		{
			return false;
		}
		if (Sex != other.Sex)
		{
			return false;
		}
		if (HeadPortraitId != other.HeadPortraitId)
		{
			return false;
		}
		if (Name != other.Name)
		{
			return false;
		}
		if (!ProtobufEqualityComparers.BitwiseDoubleEqualityComparer.Equals(Gold, other.Gold))
		{
			return false;
		}
		if (!ProtobufEqualityComparers.BitwiseDoubleEqualityComparer.Equals(Wood, other.Wood))
		{
			return false;
		}
		if (!ProtobufEqualityComparers.BitwiseDoubleEqualityComparer.Equals(Water, other.Water))
		{
			return false;
		}
		if (!ProtobufEqualityComparers.BitwiseDoubleEqualityComparer.Equals(Fire, other.Fire))
		{
			return false;
		}
		if (!ProtobufEqualityComparers.BitwiseDoubleEqualityComparer.Equals(Earth, other.Earth))
		{
			return false;
		}
		return object.Equals(_unknownFields, other._unknownFields);
	}

	[DebuggerNonUserCode]
	public override int GetHashCode()
	{
		int num = 1;
		if (UserId.Length != 0)
		{
			num ^= UserId.GetHashCode();
		}
		if (ClientIp.Length != 0)
		{
			num ^= ClientIp.GetHashCode();
		}
		if (ServerId.Length != 0)
		{
			num ^= ServerId.GetHashCode();
		}
		if (MapId.Length != 0)
		{
			num ^= MapId.GetHashCode();
		}
		if (Sex)
		{
			num ^= Sex.GetHashCode();
		}
		if (HeadPortraitId != 0)
		{
			num ^= HeadPortraitId.GetHashCode();
		}
		if (Name.Length != 0)
		{
			num ^= Name.GetHashCode();
		}
		if (Gold != 0.0)
		{
			num ^= ProtobufEqualityComparers.BitwiseDoubleEqualityComparer.GetHashCode(Gold);
		}
		if (Wood != 0.0)
		{
			num ^= ProtobufEqualityComparers.BitwiseDoubleEqualityComparer.GetHashCode(Wood);
		}
		if (Water != 0.0)
		{
			num ^= ProtobufEqualityComparers.BitwiseDoubleEqualityComparer.GetHashCode(Water);
		}
		if (Fire != 0.0)
		{
			num ^= ProtobufEqualityComparers.BitwiseDoubleEqualityComparer.GetHashCode(Fire);
		}
		if (Earth != 0.0)
		{
			num ^= ProtobufEqualityComparers.BitwiseDoubleEqualityComparer.GetHashCode(Earth);
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
		if (UserId.Length != 0)
		{
			output.WriteRawTag(10);
			output.WriteString(UserId);
		}
		if (ClientIp.Length != 0)
		{
			output.WriteRawTag(18);
			output.WriteString(ClientIp);
		}
		if (ServerId.Length != 0)
		{
			output.WriteRawTag(26);
			output.WriteString(ServerId);
		}
		if (MapId.Length != 0)
		{
			output.WriteRawTag(34);
			output.WriteString(MapId);
		}
		if (Sex)
		{
			output.WriteRawTag(40);
			output.WriteBool(Sex);
		}
		if (HeadPortraitId != 0)
		{
			output.WriteRawTag(48);
			output.WriteInt32(HeadPortraitId);
		}
		if (Name.Length != 0)
		{
			output.WriteRawTag(58);
			output.WriteString(Name);
		}
		if (Gold != 0.0)
		{
			output.WriteRawTag(65);
			output.WriteDouble(Gold);
		}
		if (Wood != 0.0)
		{
			output.WriteRawTag(73);
			output.WriteDouble(Wood);
		}
		if (Water != 0.0)
		{
			output.WriteRawTag(81);
			output.WriteDouble(Water);
		}
		if (Fire != 0.0)
		{
			output.WriteRawTag(89);
			output.WriteDouble(Fire);
		}
		if (Earth != 0.0)
		{
			output.WriteRawTag(97);
			output.WriteDouble(Earth);
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
		if (UserId.Length != 0)
		{
			num += 1 + CodedOutputStream.ComputeStringSize(UserId);
		}
		if (ClientIp.Length != 0)
		{
			num += 1 + CodedOutputStream.ComputeStringSize(ClientIp);
		}
		if (ServerId.Length != 0)
		{
			num += 1 + CodedOutputStream.ComputeStringSize(ServerId);
		}
		if (MapId.Length != 0)
		{
			num += 1 + CodedOutputStream.ComputeStringSize(MapId);
		}
		if (Sex)
		{
			num += 2;
		}
		if (HeadPortraitId != 0)
		{
			num += 1 + CodedOutputStream.ComputeInt32Size(HeadPortraitId);
		}
		if (Name.Length != 0)
		{
			num += 1 + CodedOutputStream.ComputeStringSize(Name);
		}
		if (Gold != 0.0)
		{
			num += 9;
		}
		if (Wood != 0.0)
		{
			num += 9;
		}
		if (Water != 0.0)
		{
			num += 9;
		}
		if (Fire != 0.0)
		{
			num += 9;
		}
		if (Earth != 0.0)
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
	public void MergeFrom(RoleCreateReq_1002001 other)
	{
		if (other != null)
		{
			if (other.UserId.Length != 0)
			{
				UserId = other.UserId;
			}
			if (other.ClientIp.Length != 0)
			{
				ClientIp = other.ClientIp;
			}
			if (other.ServerId.Length != 0)
			{
				ServerId = other.ServerId;
			}
			if (other.MapId.Length != 0)
			{
				MapId = other.MapId;
			}
			if (other.Sex)
			{
				Sex = other.Sex;
			}
			if (other.HeadPortraitId != 0)
			{
				HeadPortraitId = other.HeadPortraitId;
			}
			if (other.Name.Length != 0)
			{
				Name = other.Name;
			}
			if (other.Gold != 0.0)
			{
				Gold = other.Gold;
			}
			if (other.Wood != 0.0)
			{
				Wood = other.Wood;
			}
			if (other.Water != 0.0)
			{
				Water = other.Water;
			}
			if (other.Fire != 0.0)
			{
				Fire = other.Fire;
			}
			if (other.Earth != 0.0)
			{
				Earth = other.Earth;
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
				UserId = input.ReadString();
				break;
			case 18u:
				ClientIp = input.ReadString();
				break;
			case 26u:
				ServerId = input.ReadString();
				break;
			case 34u:
				MapId = input.ReadString();
				break;
			case 40u:
				Sex = input.ReadBool();
				break;
			case 48u:
				HeadPortraitId = input.ReadInt32();
				break;
			case 58u:
				Name = input.ReadString();
				break;
			case 65u:
				Gold = input.ReadDouble();
				break;
			case 73u:
				Wood = input.ReadDouble();
				break;
			case 81u:
				Water = input.ReadDouble();
				break;
			case 89u:
				Fire = input.ReadDouble();
				break;
			case 97u:
				Earth = input.ReadDouble();
				break;
			}
		}
	}
}
