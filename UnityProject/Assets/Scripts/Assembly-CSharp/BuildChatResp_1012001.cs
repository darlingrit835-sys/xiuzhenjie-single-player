using System;
using System.Diagnostics;
using Google.Protobuf;
using Google.Protobuf.Reflection;

public sealed class BuildChatResp_1012001 : IMessage<BuildChatResp_1012001>, IMessage, IEquatable<BuildChatResp_1012001>, IDeepCloneable<BuildChatResp_1012001>
{
	private static readonly MessageParser<BuildChatResp_1012001> _parser = new MessageParser<BuildChatResp_1012001>(() => new BuildChatResp_1012001());

	private UnknownFieldSet _unknownFields;

	public const int CodeFieldNumber = 1;

	private int code_;

	public const int IsSuccessFieldNumber = 2;

	private bool isSuccess_;

	public const int AddressFieldNumber = 3;

	private string address_ = "";

	public const int PortFieldNumber = 4;

	private int port_;

	[DebuggerNonUserCode]
	public static MessageParser<BuildChatResp_1012001> Parser => _parser;

	[DebuggerNonUserCode]
	public static MessageDescriptor Descriptor => ChatProtoReflection.Descriptor.MessageTypes[0];

	[DebuggerNonUserCode]
	MessageDescriptor IMessage.Descriptor => pb_003A_003AGoogle_002EProtobuf_002EIMessage_002Eget_Descriptor();

	[DebuggerNonUserCode]
	public int Code
	{
		get
		{
			return code_;
		}
		set
		{
			code_ = value;
		}
	}

	[DebuggerNonUserCode]
	public bool IsSuccess
	{
		get
		{
			return isSuccess_;
		}
		set
		{
			isSuccess_ = value;
		}
	}

	[DebuggerNonUserCode]
	public string Address
	{
		get
		{
			return address_;
		}
		set
		{
			address_ = ProtoPreconditions.CheckNotNull(value, "value");
		}
	}

	[DebuggerNonUserCode]
	public int Port
	{
		get
		{
			return port_;
		}
		set
		{
			port_ = value;
		}
	}

	private MessageDescriptor pb_003A_003AGoogle_002EProtobuf_002EIMessage_002Eget_Descriptor()
	{
		return Descriptor;
	}

	[DebuggerNonUserCode]
	public BuildChatResp_1012001()
	{
	}

	[DebuggerNonUserCode]
	public BuildChatResp_1012001(BuildChatResp_1012001 other)
		: this()
	{
		code_ = other.code_;
		isSuccess_ = other.isSuccess_;
		address_ = other.address_;
		port_ = other.port_;
		_unknownFields = UnknownFieldSet.Clone(other._unknownFields);
	}

	[DebuggerNonUserCode]
	public BuildChatResp_1012001 Clone()
	{
		return new BuildChatResp_1012001(this);
	}

	[DebuggerNonUserCode]
	public override bool Equals(object other)
	{
		return Equals(other as BuildChatResp_1012001);
	}

	[DebuggerNonUserCode]
	public bool Equals(BuildChatResp_1012001 other)
	{
		if (other == null)
		{
			return false;
		}
		if (other == this)
		{
			return true;
		}
		if (Code != other.Code)
		{
			return false;
		}
		if (IsSuccess != other.IsSuccess)
		{
			return false;
		}
		if (Address != other.Address)
		{
			return false;
		}
		if (Port != other.Port)
		{
			return false;
		}
		return object.Equals(_unknownFields, other._unknownFields);
	}

	[DebuggerNonUserCode]
	public override int GetHashCode()
	{
		int num = 1;
		if (Code != 0)
		{
			num ^= Code.GetHashCode();
		}
		if (IsSuccess)
		{
			num ^= IsSuccess.GetHashCode();
		}
		if (Address.Length != 0)
		{
			num ^= Address.GetHashCode();
		}
		if (Port != 0)
		{
			num ^= Port.GetHashCode();
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
		if (Code != 0)
		{
			output.WriteRawTag(8);
			output.WriteInt32(Code);
		}
		if (IsSuccess)
		{
			output.WriteRawTag(16);
			output.WriteBool(IsSuccess);
		}
		if (Address.Length != 0)
		{
			output.WriteRawTag(26);
			output.WriteString(Address);
		}
		if (Port != 0)
		{
			output.WriteRawTag(32);
			output.WriteInt32(Port);
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
		if (Code != 0)
		{
			num += 1 + CodedOutputStream.ComputeInt32Size(Code);
		}
		if (IsSuccess)
		{
			num += 2;
		}
		if (Address.Length != 0)
		{
			num += 1 + CodedOutputStream.ComputeStringSize(Address);
		}
		if (Port != 0)
		{
			num += 1 + CodedOutputStream.ComputeInt32Size(Port);
		}
		if (_unknownFields != null)
		{
			num += _unknownFields.CalculateSize();
		}
		return num;
	}

	[DebuggerNonUserCode]
	public void MergeFrom(BuildChatResp_1012001 other)
	{
		if (other != null)
		{
			if (other.Code != 0)
			{
				Code = other.Code;
			}
			if (other.IsSuccess)
			{
				IsSuccess = other.IsSuccess;
			}
			if (other.Address.Length != 0)
			{
				Address = other.Address;
			}
			if (other.Port != 0)
			{
				Port = other.Port;
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
			case 8u:
				Code = input.ReadInt32();
				break;
			case 16u:
				IsSuccess = input.ReadBool();
				break;
			case 26u:
				Address = input.ReadString();
				break;
			case 32u:
				Port = input.ReadInt32();
				break;
			}
		}
	}
}
