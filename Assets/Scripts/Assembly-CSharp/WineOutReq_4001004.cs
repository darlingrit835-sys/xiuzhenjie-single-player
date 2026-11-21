using System;
using System.Diagnostics;
using Google.Protobuf;
using Google.Protobuf.Reflection;

public sealed class WineOutReq_4001004 : IMessage<WineOutReq_4001004>, IMessage, IEquatable<WineOutReq_4001004>, IDeepCloneable<WineOutReq_4001004>
{
	private static readonly MessageParser<WineOutReq_4001004> _parser = new MessageParser<WineOutReq_4001004>(() => new WineOutReq_4001004());

	private UnknownFieldSet _unknownFields;

	public const int ChatIdFieldNumber = 1;

	private string chatId_ = "";

	[DebuggerNonUserCode]
	public static MessageParser<WineOutReq_4001004> Parser => _parser;

	[DebuggerNonUserCode]
	public static MessageDescriptor Descriptor => ClientChatProtoReflection.Descriptor.MessageTypes[5];

	[DebuggerNonUserCode]
	MessageDescriptor IMessage.Descriptor => pb_003A_003AGoogle_002EProtobuf_002EIMessage_002Eget_Descriptor();

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
	public WineOutReq_4001004()
	{
	}

	[DebuggerNonUserCode]
	public WineOutReq_4001004(WineOutReq_4001004 other)
		: this()
	{
		chatId_ = other.chatId_;
		_unknownFields = UnknownFieldSet.Clone(other._unknownFields);
	}

	[DebuggerNonUserCode]
	public WineOutReq_4001004 Clone()
	{
		return new WineOutReq_4001004(this);
	}

	[DebuggerNonUserCode]
	public override bool Equals(object other)
	{
		return Equals(other as WineOutReq_4001004);
	}

	[DebuggerNonUserCode]
	public bool Equals(WineOutReq_4001004 other)
	{
		if (other == null)
		{
			return false;
		}
		if (other == this)
		{
			return true;
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
		if (ChatId.Length != 0)
		{
			output.WriteRawTag(10);
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
	public void MergeFrom(WineOutReq_4001004 other)
	{
		if (other != null)
		{
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
			if (num != 10)
			{
				_unknownFields = UnknownFieldSet.MergeFieldFrom(_unknownFields, input);
			}
			else
			{
				ChatId = input.ReadString();
			}
		}
	}
}
