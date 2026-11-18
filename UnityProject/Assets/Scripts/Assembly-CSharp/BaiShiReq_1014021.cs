using System;
using System.Diagnostics;
using Google.Protobuf;
using Google.Protobuf.Reflection;

public sealed class BaiShiReq_1014021 : IMessage<BaiShiReq_1014021>, IMessage, IEquatable<BaiShiReq_1014021>, IDeepCloneable<BaiShiReq_1014021>
{
	private static readonly MessageParser<BaiShiReq_1014021> _parser = new MessageParser<BaiShiReq_1014021>(() => new BaiShiReq_1014021());

	private UnknownFieldSet _unknownFields;

	public const int RoleNameFieldNumber = 1;

	private string roleName_ = "";

	[DebuggerNonUserCode]
	public static MessageParser<BaiShiReq_1014021> Parser => _parser;

	[DebuggerNonUserCode]
	public static MessageDescriptor Descriptor => FriendProtoReflection.Descriptor.MessageTypes[23];

	[DebuggerNonUserCode]
	MessageDescriptor IMessage.Descriptor => pb_003A_003AGoogle_002EProtobuf_002EIMessage_002Eget_Descriptor();

	[DebuggerNonUserCode]
	public string RoleName
	{
		get
		{
			return roleName_;
		}
		set
		{
			roleName_ = ProtoPreconditions.CheckNotNull(value, "value");
		}
	}

	private MessageDescriptor pb_003A_003AGoogle_002EProtobuf_002EIMessage_002Eget_Descriptor()
	{
		return Descriptor;
	}

	[DebuggerNonUserCode]
	public BaiShiReq_1014021()
	{
	}

	[DebuggerNonUserCode]
	public BaiShiReq_1014021(BaiShiReq_1014021 other)
		: this()
	{
		roleName_ = other.roleName_;
		_unknownFields = UnknownFieldSet.Clone(other._unknownFields);
	}

	[DebuggerNonUserCode]
	public BaiShiReq_1014021 Clone()
	{
		return new BaiShiReq_1014021(this);
	}

	[DebuggerNonUserCode]
	public override bool Equals(object other)
	{
		return Equals(other as BaiShiReq_1014021);
	}

	[DebuggerNonUserCode]
	public bool Equals(BaiShiReq_1014021 other)
	{
		if (other == null)
		{
			return false;
		}
		if (other == this)
		{
			return true;
		}
		if (RoleName != other.RoleName)
		{
			return false;
		}
		return object.Equals(_unknownFields, other._unknownFields);
	}

	[DebuggerNonUserCode]
	public override int GetHashCode()
	{
		int num = 1;
		if (RoleName.Length != 0)
		{
			num ^= RoleName.GetHashCode();
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
		if (RoleName.Length != 0)
		{
			output.WriteRawTag(10);
			output.WriteString(RoleName);
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
		if (RoleName.Length != 0)
		{
			num += 1 + CodedOutputStream.ComputeStringSize(RoleName);
		}
		if (_unknownFields != null)
		{
			num += _unknownFields.CalculateSize();
		}
		return num;
	}

	[DebuggerNonUserCode]
	public void MergeFrom(BaiShiReq_1014021 other)
	{
		if (other != null)
		{
			if (other.RoleName.Length != 0)
			{
				RoleName = other.RoleName;
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
				RoleName = input.ReadString();
			}
		}
	}
}
