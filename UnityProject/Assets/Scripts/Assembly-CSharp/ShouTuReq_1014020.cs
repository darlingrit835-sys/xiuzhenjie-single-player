using System;
using System.Diagnostics;
using Google.Protobuf;
using Google.Protobuf.Reflection;

public sealed class ShouTuReq_1014020 : IMessage<ShouTuReq_1014020>, IMessage, IEquatable<ShouTuReq_1014020>, IDeepCloneable<ShouTuReq_1014020>
{
	private static readonly MessageParser<ShouTuReq_1014020> _parser = new MessageParser<ShouTuReq_1014020>(() => new ShouTuReq_1014020());

	private UnknownFieldSet _unknownFields;

	public const int RoleIdFieldNumber = 1;

	private string roleId_ = "";

	[DebuggerNonUserCode]
	public static MessageParser<ShouTuReq_1014020> Parser => _parser;

	[DebuggerNonUserCode]
	public static MessageDescriptor Descriptor => FriendProtoReflection.Descriptor.MessageTypes[21];

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

	private MessageDescriptor pb_003A_003AGoogle_002EProtobuf_002EIMessage_002Eget_Descriptor()
	{
		return Descriptor;
	}

	[DebuggerNonUserCode]
	public ShouTuReq_1014020()
	{
	}

	[DebuggerNonUserCode]
	public ShouTuReq_1014020(ShouTuReq_1014020 other)
		: this()
	{
		roleId_ = other.roleId_;
		_unknownFields = UnknownFieldSet.Clone(other._unknownFields);
	}

	[DebuggerNonUserCode]
	public ShouTuReq_1014020 Clone()
	{
		return new ShouTuReq_1014020(this);
	}

	[DebuggerNonUserCode]
	public override bool Equals(object other)
	{
		return Equals(other as ShouTuReq_1014020);
	}

	[DebuggerNonUserCode]
	public bool Equals(ShouTuReq_1014020 other)
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
		if (_unknownFields != null)
		{
			num += _unknownFields.CalculateSize();
		}
		return num;
	}

	[DebuggerNonUserCode]
	public void MergeFrom(ShouTuReq_1014020 other)
	{
		if (other != null)
		{
			if (other.RoleId.Length != 0)
			{
				RoleId = other.RoleId;
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
			if (num != 10)
			{
				_unknownFields = UnknownFieldSet.MergeFieldFrom(_unknownFields, input);
			}
			else
			{
				RoleId = input.ReadString();
			}
		}
	}
}
