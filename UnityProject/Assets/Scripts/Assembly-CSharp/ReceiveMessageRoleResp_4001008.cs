using System;
using System.Diagnostics;
using Google.Protobuf;
using Google.Protobuf.Reflection;

public sealed class ReceiveMessageRoleResp_4001008 : IMessage<ReceiveMessageRoleResp_4001008>, IMessage, IEquatable<ReceiveMessageRoleResp_4001008>, IDeepCloneable<ReceiveMessageRoleResp_4001008>
{
	private static readonly MessageParser<ReceiveMessageRoleResp_4001008> _parser = new MessageParser<ReceiveMessageRoleResp_4001008>(() => new ReceiveMessageRoleResp_4001008());

	private UnknownFieldSet _unknownFields;

	public const int CodeFieldNumber = 1;

	private int code_;

	public const int IsSuccessFieldNumber = 2;

	private bool isSuccess_;

	public const int MsgFieldNumber = 5;

	private string msg_ = "";

	public const int RoleIdFieldNumber = 6;

	private string roleId_ = "";

	public const int MsgTimeFieldNumber = 4;

	private long msgTime_;

	[DebuggerNonUserCode]
	public static MessageParser<ReceiveMessageRoleResp_4001008> Parser => _parser;

	[DebuggerNonUserCode]
	public static MessageDescriptor Descriptor => ClientChatProtoReflection.Descriptor.MessageTypes[11];

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
	public string Msg
	{
		get
		{
			return msg_;
		}
		set
		{
			msg_ = ProtoPreconditions.CheckNotNull(value, "value");
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
	public long MsgTime
	{
		get
		{
			return msgTime_;
		}
		set
		{
			msgTime_ = value;
		}
	}

	private MessageDescriptor pb_003A_003AGoogle_002EProtobuf_002EIMessage_002Eget_Descriptor()
	{
		return Descriptor;
	}

	[DebuggerNonUserCode]
	public ReceiveMessageRoleResp_4001008()
	{
	}

	[DebuggerNonUserCode]
	public ReceiveMessageRoleResp_4001008(ReceiveMessageRoleResp_4001008 other)
		: this()
	{
		code_ = other.code_;
		isSuccess_ = other.isSuccess_;
		msg_ = other.msg_;
		roleId_ = other.roleId_;
		msgTime_ = other.msgTime_;
		_unknownFields = UnknownFieldSet.Clone(other._unknownFields);
	}

	[DebuggerNonUserCode]
	public ReceiveMessageRoleResp_4001008 Clone()
	{
		return new ReceiveMessageRoleResp_4001008(this);
	}

	[DebuggerNonUserCode]
	public override bool Equals(object other)
	{
		return Equals(other as ReceiveMessageRoleResp_4001008);
	}

	[DebuggerNonUserCode]
	public bool Equals(ReceiveMessageRoleResp_4001008 other)
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
		if (Msg != other.Msg)
		{
			return false;
		}
		if (RoleId != other.RoleId)
		{
			return false;
		}
		if (MsgTime != other.MsgTime)
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
		if (Msg.Length != 0)
		{
			num ^= Msg.GetHashCode();
		}
		if (RoleId.Length != 0)
		{
			num ^= RoleId.GetHashCode();
		}
		if (MsgTime != 0L)
		{
			num ^= MsgTime.GetHashCode();
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
		if (MsgTime != 0L)
		{
			output.WriteRawTag(32);
			output.WriteInt64(MsgTime);
		}
		if (Msg.Length != 0)
		{
			output.WriteRawTag(42);
			output.WriteString(Msg);
		}
		if (RoleId.Length != 0)
		{
			output.WriteRawTag(50);
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
		if (Code != 0)
		{
			num += 1 + CodedOutputStream.ComputeInt32Size(Code);
		}
		if (IsSuccess)
		{
			num += 2;
		}
		if (Msg.Length != 0)
		{
			num += 1 + CodedOutputStream.ComputeStringSize(Msg);
		}
		if (RoleId.Length != 0)
		{
			num += 1 + CodedOutputStream.ComputeStringSize(RoleId);
		}
		if (MsgTime != 0L)
		{
			num += 1 + CodedOutputStream.ComputeInt64Size(MsgTime);
		}
		if (_unknownFields != null)
		{
			num += _unknownFields.CalculateSize();
		}
		return num;
	}

	[DebuggerNonUserCode]
	public void MergeFrom(ReceiveMessageRoleResp_4001008 other)
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
			if (other.Msg.Length != 0)
			{
				Msg = other.Msg;
			}
			if (other.RoleId.Length != 0)
			{
				RoleId = other.RoleId;
			}
			if (other.MsgTime != 0L)
			{
				MsgTime = other.MsgTime;
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
			case 32u:
				MsgTime = input.ReadInt64();
				break;
			case 42u:
				Msg = input.ReadString();
				break;
			case 50u:
				RoleId = input.ReadString();
				break;
			}
		}
	}
}
