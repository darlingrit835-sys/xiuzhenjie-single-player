using System;
using System.Diagnostics;
using Google.Protobuf;
using Google.Protobuf.Reflection;

public sealed class SendMessageReq_4001003 : IMessage<SendMessageReq_4001003>, IMessage, IEquatable<SendMessageReq_4001003>, IDeepCloneable<SendMessageReq_4001003>
{
	private static readonly MessageParser<SendMessageReq_4001003> _parser = new MessageParser<SendMessageReq_4001003>(() => new SendMessageReq_4001003());

	private UnknownFieldSet _unknownFields;

	public const int MsgFieldNumber = 1;

	private string msg_ = "";

	public const int ChatIdFieldNumber = 2;

	private string chatId_ = "";

	[DebuggerNonUserCode]
	public static MessageParser<SendMessageReq_4001003> Parser => _parser;

	[DebuggerNonUserCode]
	public static MessageDescriptor Descriptor => ClientChatProtoReflection.Descriptor.MessageTypes[3];

	[DebuggerNonUserCode]
	MessageDescriptor IMessage.Descriptor => pb_003A_003AGoogle_002EProtobuf_002EIMessage_002Eget_Descriptor();

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
	public string ChatId
	{
		get
		{
			return chatId_;
		}
		set
		{
			chatId_ = ProtoPreconditions.CheckNotNull(value, "value");
		}
	}

	private MessageDescriptor pb_003A_003AGoogle_002EProtobuf_002EIMessage_002Eget_Descriptor()
	{
		return Descriptor;
	}

	[DebuggerNonUserCode]
	public SendMessageReq_4001003()
	{
	}

	[DebuggerNonUserCode]
	public SendMessageReq_4001003(SendMessageReq_4001003 other)
		: this()
	{
		msg_ = other.msg_;
		chatId_ = other.chatId_;
		_unknownFields = UnknownFieldSet.Clone(other._unknownFields);
	}

	[DebuggerNonUserCode]
	public SendMessageReq_4001003 Clone()
	{
		return new SendMessageReq_4001003(this);
	}

	[DebuggerNonUserCode]
	public override bool Equals(object other)
	{
		return Equals(other as SendMessageReq_4001003);
	}

	[DebuggerNonUserCode]
	public bool Equals(SendMessageReq_4001003 other)
	{
		if (other == null)
		{
			return false;
		}
		if (other == this)
		{
			return true;
		}
		if (Msg != other.Msg)
		{
			return false;
		}
		if (ChatId != other.ChatId)
		{
			return false;
		}
		return object.Equals(_unknownFields, other._unknownFields);
	}

	[DebuggerNonUserCode]
	public override int GetHashCode()
	{
		int num = 1;
		if (Msg.Length != 0)
		{
			num ^= Msg.GetHashCode();
		}
		if (ChatId.Length != 0)
		{
			num ^= ChatId.GetHashCode();
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
		if (Msg.Length != 0)
		{
			output.WriteRawTag(10);
			output.WriteString(Msg);
		}
		if (ChatId.Length != 0)
		{
			output.WriteRawTag(18);
			output.WriteString(ChatId);
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
		if (Msg.Length != 0)
		{
			num += 1 + CodedOutputStream.ComputeStringSize(Msg);
		}
		if (ChatId.Length != 0)
		{
			num += 1 + CodedOutputStream.ComputeStringSize(ChatId);
		}
		if (_unknownFields != null)
		{
			num += _unknownFields.CalculateSize();
		}
		return num;
	}

	[DebuggerNonUserCode]
	public void MergeFrom(SendMessageReq_4001003 other)
	{
		if (other != null)
		{
			if (other.Msg.Length != 0)
			{
				Msg = other.Msg;
			}
			if (other.ChatId.Length != 0)
			{
				ChatId = other.ChatId;
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
				Msg = input.ReadString();
				break;
			case 18u:
				ChatId = input.ReadString();
				break;
			}
		}
	}
}
