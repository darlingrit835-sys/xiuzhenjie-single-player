using System;
using System.Diagnostics;
using Google.Protobuf;
using Google.Protobuf.Reflection;

public sealed class GuildMonsterFightingRoleHelpReq_1018003 : IMessage<GuildMonsterFightingRoleHelpReq_1018003>, IMessage, IEquatable<GuildMonsterFightingRoleHelpReq_1018003>, IDeepCloneable<GuildMonsterFightingRoleHelpReq_1018003>
{
	private static readonly MessageParser<GuildMonsterFightingRoleHelpReq_1018003> _parser = new MessageParser<GuildMonsterFightingRoleHelpReq_1018003>(() => new GuildMonsterFightingRoleHelpReq_1018003());

	private UnknownFieldSet _unknownFields;

	public const int RoleIdFieldNumber = 1;

	private string roleId_ = "";

	public const int HelpIndexFieldNumber = 2;

	private int helpIndex_;

	[DebuggerNonUserCode]
	public static MessageParser<GuildMonsterFightingRoleHelpReq_1018003> Parser => _parser;

	[DebuggerNonUserCode]
	public static MessageDescriptor Descriptor => GuildFightProtoReflection.Descriptor.MessageTypes[3];

	[DebuggerNonUserCode]
	MessageDescriptor IMessage.Descriptor => pb_003A_003AGoogle_002EProtobuf_002EIMessage_002Eget_Descriptor();

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
	public int HelpIndex
	{
		get
		{
			return helpIndex_;
		}
		set
		{
			helpIndex_ = value;
		}
	}

	private MessageDescriptor pb_003A_003AGoogle_002EProtobuf_002EIMessage_002Eget_Descriptor()
	{
		return Descriptor;
	}

	[DebuggerNonUserCode]
	public GuildMonsterFightingRoleHelpReq_1018003()
	{
	}

	[DebuggerNonUserCode]
	public GuildMonsterFightingRoleHelpReq_1018003(GuildMonsterFightingRoleHelpReq_1018003 other)
		: this()
	{
		roleId_ = other.roleId_;
		helpIndex_ = other.helpIndex_;
		_unknownFields = UnknownFieldSet.Clone(other._unknownFields);
	}

	[DebuggerNonUserCode]
	public GuildMonsterFightingRoleHelpReq_1018003 Clone()
	{
		return new GuildMonsterFightingRoleHelpReq_1018003(this);
	}

	[DebuggerNonUserCode]
	public override bool Equals(object other)
	{
		return Equals(other as GuildMonsterFightingRoleHelpReq_1018003);
	}

	[DebuggerNonUserCode]
	public bool Equals(GuildMonsterFightingRoleHelpReq_1018003 other)
	{
		if (other == null)
		{
			return false;
		}
		if (other == this)
		{
			return true;
		}
		if (RoleId != other.RoleId)
		{
			return false;
		}
		if (HelpIndex != other.HelpIndex)
		{
			return false;
		}
		return object.Equals(_unknownFields, other._unknownFields);
	}

	[DebuggerNonUserCode]
	public override int GetHashCode()
	{
		int num = 1;
		if (RoleId.Length != 0)
		{
			num ^= RoleId.GetHashCode();
		}
		if (HelpIndex != 0)
		{
			num ^= HelpIndex.GetHashCode();
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
		if (RoleId.Length != 0)
		{
			output.WriteRawTag(10);
			output.WriteString(RoleId);
		}
		if (HelpIndex != 0)
		{
			output.WriteRawTag(16);
			output.WriteInt32(HelpIndex);
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
		if (RoleId.Length != 0)
		{
			num += 1 + CodedOutputStream.ComputeStringSize(RoleId);
		}
		if (HelpIndex != 0)
		{
			num += 1 + CodedOutputStream.ComputeInt32Size(HelpIndex);
		}
		if (_unknownFields != null)
		{
			num += _unknownFields.CalculateSize();
		}
		return num;
	}

	[DebuggerNonUserCode]
	public void MergeFrom(GuildMonsterFightingRoleHelpReq_1018003 other)
	{
		if (other != null)
		{
			if (other.RoleId.Length != 0)
			{
				RoleId = other.RoleId;
			}
			if (other.HelpIndex != 0)
			{
				HelpIndex = other.HelpIndex;
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
				RoleId = input.ReadString();
				break;
			case 16u:
				HelpIndex = input.ReadInt32();
				break;
			}
		}
	}
}
