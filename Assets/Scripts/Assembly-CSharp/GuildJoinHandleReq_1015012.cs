using System;
using System.Diagnostics;
using Google.Protobuf;
using Google.Protobuf.Reflection;

public sealed class GuildJoinHandleReq_1015012 : IMessage<GuildJoinHandleReq_1015012>, IMessage, IEquatable<GuildJoinHandleReq_1015012>, IDeepCloneable<GuildJoinHandleReq_1015012>
{
	private static readonly MessageParser<GuildJoinHandleReq_1015012> _parser = new MessageParser<GuildJoinHandleReq_1015012>(() => new GuildJoinHandleReq_1015012());

	private UnknownFieldSet _unknownFields;

	public const int RoleIdFieldNumber = 1;

	private string roleId_ = "";

	public const int IsAgreeFieldNumber = 2;

	private bool isAgree_;

	[DebuggerNonUserCode]
	public static MessageParser<GuildJoinHandleReq_1015012> Parser => _parser;

	[DebuggerNonUserCode]
	public static MessageDescriptor Descriptor => GuildProtoReflection.Descriptor.MessageTypes[11];

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
	public bool IsAgree
	{
		get
		{
			return isAgree_;
		}
		set
		{
			isAgree_ = value;
		}
	}

	private MessageDescriptor pb_003A_003AGoogle_002EProtobuf_002EIMessage_002Eget_Descriptor()
	{
		return Descriptor;
	}

	[DebuggerNonUserCode]
	public GuildJoinHandleReq_1015012()
	{
	}

	[DebuggerNonUserCode]
	public GuildJoinHandleReq_1015012(GuildJoinHandleReq_1015012 other)
		: this()
	{
		roleId_ = other.roleId_;
		isAgree_ = other.isAgree_;
		_unknownFields = UnknownFieldSet.Clone(other._unknownFields);
	}

	[DebuggerNonUserCode]
	public GuildJoinHandleReq_1015012 Clone()
	{
		return new GuildJoinHandleReq_1015012(this);
	}

	[DebuggerNonUserCode]
	public override bool Equals(object other)
	{
		return Equals(other as GuildJoinHandleReq_1015012);
	}

	[DebuggerNonUserCode]
	public bool Equals(GuildJoinHandleReq_1015012 other)
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
		if (IsAgree != other.IsAgree)
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
		if (IsAgree)
		{
			num ^= IsAgree.GetHashCode();
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
		if (IsAgree)
		{
			output.WriteRawTag(16);
			output.WriteBool(IsAgree);
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
		if (IsAgree)
		{
			num += 2;
		}
		if (_unknownFields != null)
		{
			num += _unknownFields.CalculateSize();
		}
		return num;
	}

	[DebuggerNonUserCode]
	public void MergeFrom(GuildJoinHandleReq_1015012 other)
	{
		if (other != null)
		{
			if (other.RoleId.Length != 0)
			{
				RoleId = other.RoleId;
			}
			if (other.IsAgree)
			{
				IsAgree = other.IsAgree;
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
				IsAgree = input.ReadBool();
				break;
			}
		}
	}
}
