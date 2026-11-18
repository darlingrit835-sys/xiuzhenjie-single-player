using System;
using System.Diagnostics;
using Google.Protobuf;
using Google.Protobuf.Reflection;

public sealed class BaiShiHuLueReq_1014023 : IMessage<BaiShiHuLueReq_1014023>, IMessage, IEquatable<BaiShiHuLueReq_1014023>, IDeepCloneable<BaiShiHuLueReq_1014023>
{
	private static readonly MessageParser<BaiShiHuLueReq_1014023> _parser = new MessageParser<BaiShiHuLueReq_1014023>(() => new BaiShiHuLueReq_1014023());

	private UnknownFieldSet _unknownFields;

	public const int RoleIdFieldNumber = 1;

	private string roleId_ = "";

	[DebuggerNonUserCode]
	public static MessageParser<BaiShiHuLueReq_1014023> Parser => _parser;

	[DebuggerNonUserCode]
	public static MessageDescriptor Descriptor => FriendProtoReflection.Descriptor.MessageTypes[26];

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
	public BaiShiHuLueReq_1014023()
	{
	}

	[DebuggerNonUserCode]
	public BaiShiHuLueReq_1014023(BaiShiHuLueReq_1014023 other)
		: this()
	{
		roleId_ = other.roleId_;
		_unknownFields = UnknownFieldSet.Clone(other._unknownFields);
	}

	[DebuggerNonUserCode]
	public BaiShiHuLueReq_1014023 Clone()
	{
		return new BaiShiHuLueReq_1014023(this);
	}

	[DebuggerNonUserCode]
	public override bool Equals(object other)
	{
		return Equals(other as BaiShiHuLueReq_1014023);
	}

	[DebuggerNonUserCode]
	public bool Equals(BaiShiHuLueReq_1014023 other)
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
	public void MergeFrom(BaiShiHuLueReq_1014023 other)
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
