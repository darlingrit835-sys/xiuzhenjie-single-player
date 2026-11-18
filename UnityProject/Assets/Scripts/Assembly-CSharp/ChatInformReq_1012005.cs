using System;
using System.Diagnostics;
using Google.Protobuf;
using Google.Protobuf.Reflection;

public sealed class ChatInformReq_1012005 : IMessage<ChatInformReq_1012005>, IMessage, IEquatable<ChatInformReq_1012005>, IDeepCloneable<ChatInformReq_1012005>
{
	private static readonly MessageParser<ChatInformReq_1012005> _parser = new MessageParser<ChatInformReq_1012005>(() => new ChatInformReq_1012005());

	private UnknownFieldSet _unknownFields;

	public const int TypeFieldNumber = 1;

	private int type_;

	[DebuggerNonUserCode]
	public static MessageParser<ChatInformReq_1012005> Parser => _parser;

	[DebuggerNonUserCode]
	public static MessageDescriptor Descriptor => ChatProtoReflection.Descriptor.MessageTypes[4];

	[DebuggerNonUserCode]
	MessageDescriptor IMessage.Descriptor => pb_003A_003AGoogle_002EProtobuf_002EIMessage_002Eget_Descriptor();

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
	public ChatInformReq_1012005()
	{
	}

	[DebuggerNonUserCode]
	public ChatInformReq_1012005(ChatInformReq_1012005 other)
		: this()
	{
		type_ = other.type_;
		_unknownFields = UnknownFieldSet.Clone(other._unknownFields);
	}

	[DebuggerNonUserCode]
	public ChatInformReq_1012005 Clone()
	{
		return new ChatInformReq_1012005(this);
	}

	[DebuggerNonUserCode]
	public override bool Equals(object other)
	{
		return Equals(other as ChatInformReq_1012005);
	}

	[DebuggerNonUserCode]
	public bool Equals(ChatInformReq_1012005 other)
	{
		if (other == null)
		{
			return false;
		}
		if (other == this)
		{
			return true;
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
		if (Type != 0)
		{
			output.WriteRawTag(8);
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
	public void MergeFrom(ChatInformReq_1012005 other)
	{
		if (other != null)
		{
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
			if (num != 8)
			{
				_unknownFields = UnknownFieldSet.MergeFieldFrom(_unknownFields, input);
			}
			else
			{
				Type = input.ReadInt32();
			}
		}
	}
}
