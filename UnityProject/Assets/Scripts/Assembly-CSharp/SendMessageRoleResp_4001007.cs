using System;
using System.Diagnostics;
using Google.Protobuf;
using Google.Protobuf.Reflection;

public sealed class SendMessageRoleResp_4001007 : IMessage<SendMessageRoleResp_4001007>, IMessage, IEquatable<SendMessageRoleResp_4001007>, IDeepCloneable<SendMessageRoleResp_4001007>
{
	private static readonly MessageParser<SendMessageRoleResp_4001007> _parser = new MessageParser<SendMessageRoleResp_4001007>(() => new SendMessageRoleResp_4001007());

	private UnknownFieldSet _unknownFields;

	public const int CodeFieldNumber = 1;

	private int code_;

	public const int IsSuccessFieldNumber = 2;

	private bool isSuccess_;

	public const int MsgTimeFieldNumber = 3;

	private long msgTime_;

	[DebuggerNonUserCode]
	public static MessageParser<SendMessageRoleResp_4001007> Parser => _parser;

	[DebuggerNonUserCode]
	public static MessageDescriptor Descriptor => ClientChatProtoReflection.Descriptor.MessageTypes[10];

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
	public SendMessageRoleResp_4001007()
	{
	}

	[DebuggerNonUserCode]
	public SendMessageRoleResp_4001007(SendMessageRoleResp_4001007 other)
		: this()
	{
		code_ = other.code_;
		isSuccess_ = other.isSuccess_;
		msgTime_ = other.msgTime_;
		_unknownFields = UnknownFieldSet.Clone(other._unknownFields);
	}

	[DebuggerNonUserCode]
	public SendMessageRoleResp_4001007 Clone()
	{
		return new SendMessageRoleResp_4001007(this);
	}

	[DebuggerNonUserCode]
	public override bool Equals(object other)
	{
		return Equals(other as SendMessageRoleResp_4001007);
	}

	[DebuggerNonUserCode]
	public bool Equals(SendMessageRoleResp_4001007 other)
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
			output.WriteRawTag(24);
			output.WriteInt64(MsgTime);
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
	public void MergeFrom(SendMessageRoleResp_4001007 other)
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
			case 24u:
				MsgTime = input.ReadInt64();
				break;
			}
		}
	}
}
