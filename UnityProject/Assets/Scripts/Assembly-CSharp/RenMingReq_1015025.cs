using System;
using System.Diagnostics;
using Google.Protobuf;
using Google.Protobuf.Reflection;

public sealed class RenMingReq_1015025 : IMessage<RenMingReq_1015025>, IMessage, IEquatable<RenMingReq_1015025>, IDeepCloneable<RenMingReq_1015025>
{
	private static readonly MessageParser<RenMingReq_1015025> _parser = new MessageParser<RenMingReq_1015025>(() => new RenMingReq_1015025());

	private UnknownFieldSet _unknownFields;

	public const int RoleIdFieldNumber = 1;

	private string roleId_ = "";

	public const int TypeFieldNumber = 2;

	private int type_;

	[DebuggerNonUserCode]
	public static MessageParser<RenMingReq_1015025> Parser => _parser;

	[DebuggerNonUserCode]
	public static MessageDescriptor Descriptor => GuildProtoReflection.Descriptor.MessageTypes[27];

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
	public int Type
	{
		get
		{
			return type_;
		}
		set
		{
			type_ = value;
		}
	}

	private MessageDescriptor pb_003A_003AGoogle_002EProtobuf_002EIMessage_002Eget_Descriptor()
	{
		return Descriptor;
	}

	[DebuggerNonUserCode]
	public RenMingReq_1015025()
	{
	}

	[DebuggerNonUserCode]
	public RenMingReq_1015025(RenMingReq_1015025 other)
		: this()
	{
		roleId_ = other.roleId_;
		type_ = other.type_;
		_unknownFields = UnknownFieldSet.Clone(other._unknownFields);
	}

	[DebuggerNonUserCode]
	public RenMingReq_1015025 Clone()
	{
		return new RenMingReq_1015025(this);
	}

	[DebuggerNonUserCode]
	public override bool Equals(object other)
	{
		return Equals(other as RenMingReq_1015025);
	}

	[DebuggerNonUserCode]
	public bool Equals(RenMingReq_1015025 other)
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
		if (Type != other.Type)
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
		if (Type != 0)
		{
			num ^= Type.GetHashCode();
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
		if (Type != 0)
		{
			output.WriteRawTag(16);
			output.WriteInt32(Type);
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
		if (Type != 0)
		{
			num += 1 + CodedOutputStream.ComputeInt32Size(Type);
		}
		if (_unknownFields != null)
		{
			num += _unknownFields.CalculateSize();
		}
		return num;
	}

	[DebuggerNonUserCode]
	public void MergeFrom(RenMingReq_1015025 other)
	{
		if (other != null)
		{
			if (other.RoleId.Length != 0)
			{
				RoleId = other.RoleId;
			}
			if (other.Type != 0)
			{
				Type = other.Type;
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
				Type = input.ReadInt32();
				break;
			}
		}
	}
}
