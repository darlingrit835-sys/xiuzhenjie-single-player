using System;
using System.Diagnostics;
using Google.Protobuf;
using Google.Protobuf.Reflection;

public sealed class RoleLoginReq_1002002 : IMessage<RoleLoginReq_1002002>, IMessage, IEquatable<RoleLoginReq_1002002>, IDeepCloneable<RoleLoginReq_1002002>
{
	private static readonly MessageParser<RoleLoginReq_1002002> _parser = new MessageParser<RoleLoginReq_1002002>(() => new RoleLoginReq_1002002());

	private UnknownFieldSet _unknownFields;

	public const int LoginIpFieldNumber = 1;

	private string loginIp_ = "";

	public const int GameServerIpManageIdFieldNumber = 2;

	private string gameServerIpManageId_ = "";

	public const int RoleIdFieldNumber = 3;

	private string roleId_ = "";

	public const int GameServerIdFieldNumber = 4;

	private string gameServerId_ = "";

	[DebuggerNonUserCode]
	public static MessageParser<RoleLoginReq_1002002> Parser => _parser;

	[DebuggerNonUserCode]
	public static MessageDescriptor Descriptor => RoleProtoReflection.Descriptor.MessageTypes[2];

	[DebuggerNonUserCode]
	MessageDescriptor IMessage.Descriptor => pb_003A_003AGoogle_002EProtobuf_002EIMessage_002Eget_Descriptor();

	[DebuggerNonUserCode]
	public string LoginIp
	{
		get
		{
			return loginIp_;
		}
		set
		{
			loginIp_ = ProtoPreconditions.CheckNotNull(value, "value");
		}
	}

	[DebuggerNonUserCode]
	public string GameServerIpManageId
	{
		get
		{
			return gameServerIpManageId_;
		}
		set
		{
			gameServerIpManageId_ = ProtoPreconditions.CheckNotNull(value, "value");
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

	[DebuggerNonUserCode]
	public string GameServerId
	{
		get
		{
			return gameServerId_;
		}
		set
		{
			gameServerId_ = ProtoPreconditions.CheckNotNull(value, "value");
		}
	}

	private MessageDescriptor pb_003A_003AGoogle_002EProtobuf_002EIMessage_002Eget_Descriptor()
	{
		return Descriptor;
	}

	[DebuggerNonUserCode]
	public RoleLoginReq_1002002()
	{
	}

	[DebuggerNonUserCode]
	public RoleLoginReq_1002002(RoleLoginReq_1002002 other)
		: this()
	{
		loginIp_ = other.loginIp_;
		gameServerIpManageId_ = other.gameServerIpManageId_;
		roleId_ = other.roleId_;
		gameServerId_ = other.gameServerId_;
		_unknownFields = UnknownFieldSet.Clone(other._unknownFields);
	}

	[DebuggerNonUserCode]
	public RoleLoginReq_1002002 Clone()
	{
		return new RoleLoginReq_1002002(this);
	}

	[DebuggerNonUserCode]
	public override bool Equals(object other)
	{
		return Equals(other as RoleLoginReq_1002002);
	}

	[DebuggerNonUserCode]
	public bool Equals(RoleLoginReq_1002002 other)
	{
		if (other == null)
		{
			return false;
		}
		if (other == this)
		{
			return true;
		}
		if (LoginIp != other.LoginIp)
		{
			return false;
		}
		if (GameServerIpManageId != other.GameServerIpManageId)
		{
			return false;
		}
		if (RoleId != other.RoleId)
		{
			return false;
		}
		if (GameServerId != other.GameServerId)
		{
			return false;
		}
		return object.Equals(_unknownFields, other._unknownFields);
	}

	[DebuggerNonUserCode]
	public override int GetHashCode()
	{
		int num = 1;
		if (LoginIp.Length != 0)
		{
			num ^= LoginIp.GetHashCode();
		}
		if (GameServerIpManageId.Length != 0)
		{
			num ^= GameServerIpManageId.GetHashCode();
		}
		if (RoleId.Length != 0)
		{
			num ^= RoleId.GetHashCode();
		}
		if (GameServerId.Length != 0)
		{
			num ^= GameServerId.GetHashCode();
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
		if (LoginIp.Length != 0)
		{
			output.WriteRawTag(10);
			output.WriteString(LoginIp);
		}
		if (GameServerIpManageId.Length != 0)
		{
			output.WriteRawTag(18);
			output.WriteString(GameServerIpManageId);
		}
		if (RoleId.Length != 0)
		{
			output.WriteRawTag(26);
			output.WriteString(RoleId);
		}
		if (GameServerId.Length != 0)
		{
			output.WriteRawTag(34);
			output.WriteString(GameServerId);
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
		if (LoginIp.Length != 0)
		{
			num += 1 + CodedOutputStream.ComputeStringSize(LoginIp);
		}
		if (GameServerIpManageId.Length != 0)
		{
			num += 1 + CodedOutputStream.ComputeStringSize(GameServerIpManageId);
		}
		if (RoleId.Length != 0)
		{
			num += 1 + CodedOutputStream.ComputeStringSize(RoleId);
		}
		if (GameServerId.Length != 0)
		{
			num += 1 + CodedOutputStream.ComputeStringSize(GameServerId);
		}
		if (_unknownFields != null)
		{
			num += _unknownFields.CalculateSize();
		}
		return num;
	}

	[DebuggerNonUserCode]
	public void MergeFrom(RoleLoginReq_1002002 other)
	{
		if (other != null)
		{
			if (other.LoginIp.Length != 0)
			{
				LoginIp = other.LoginIp;
			}
			if (other.GameServerIpManageId.Length != 0)
			{
				GameServerIpManageId = other.GameServerIpManageId;
			}
			if (other.RoleId.Length != 0)
			{
				RoleId = other.RoleId;
			}
			if (other.GameServerId.Length != 0)
			{
				GameServerId = other.GameServerId;
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
				LoginIp = input.ReadString();
				break;
			case 18u:
				GameServerIpManageId = input.ReadString();
				break;
			case 26u:
				RoleId = input.ReadString();
				break;
			case 34u:
				GameServerId = input.ReadString();
				break;
			}
		}
	}
}
