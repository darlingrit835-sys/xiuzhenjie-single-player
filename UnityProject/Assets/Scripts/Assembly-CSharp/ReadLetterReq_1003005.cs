using System;
using System.Diagnostics;
using Google.Protobuf;
using Google.Protobuf.Reflection;

public sealed class ReadLetterReq_1003005 : IMessage<ReadLetterReq_1003005>, IMessage, IEquatable<ReadLetterReq_1003005>, IDeepCloneable<ReadLetterReq_1003005>
{
	private static readonly MessageParser<ReadLetterReq_1003005> _parser = new MessageParser<ReadLetterReq_1003005>(() => new ReadLetterReq_1003005());

	private UnknownFieldSet _unknownFields;

	public const int IdFieldNumber = 1;

	private string id_ = "";

	[DebuggerNonUserCode]
	public static MessageParser<ReadLetterReq_1003005> Parser => _parser;

	[DebuggerNonUserCode]
	public static MessageDescriptor Descriptor => LetterProtoReflection.Descriptor.MessageTypes[2];

	[DebuggerNonUserCode]
	MessageDescriptor IMessage.Descriptor => pb_003A_003AGoogle_002EProtobuf_002EIMessage_002Eget_Descriptor();

	[DebuggerNonUserCode]
	public string Id
	{
		get
		{
			return id_;
		}
		set
		{
			id_ = ProtoPreconditions.CheckNotNull(value, "value");
		}
	}

	private MessageDescriptor pb_003A_003AGoogle_002EProtobuf_002EIMessage_002Eget_Descriptor()
	{
		return Descriptor;
	}

	[DebuggerNonUserCode]
	public ReadLetterReq_1003005()
	{
	}

	[DebuggerNonUserCode]
	public ReadLetterReq_1003005(ReadLetterReq_1003005 other)
		: this()
	{
		id_ = other.id_;
		_unknownFields = UnknownFieldSet.Clone(other._unknownFields);
	}

	[DebuggerNonUserCode]
	public ReadLetterReq_1003005 Clone()
	{
		return new ReadLetterReq_1003005(this);
	}

	[DebuggerNonUserCode]
	public override bool Equals(object other)
	{
		return Equals(other as ReadLetterReq_1003005);
	}

	[DebuggerNonUserCode]
	public bool Equals(ReadLetterReq_1003005 other)
	{
		if (other == null)
		{
			return false;
		}
		if (other == this)
		{
			return true;
		}
		if (Id != other.Id)
		{
			return false;
		}
		return object.Equals(_unknownFields, other._unknownFields);
	}

	[DebuggerNonUserCode]
	public override int GetHashCode()
	{
		int num = 1;
		if (Id.Length != 0)
		{
			num ^= Id.GetHashCode();
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
		if (Id.Length != 0)
		{
			output.WriteRawTag(10);
			output.WriteString(Id);
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
		if (Id.Length != 0)
		{
			num += 1 + CodedOutputStream.ComputeStringSize(Id);
		}
		if (_unknownFields != null)
		{
			num += _unknownFields.CalculateSize();
		}
		return num;
	}

	[DebuggerNonUserCode]
	public void MergeFrom(ReadLetterReq_1003005 other)
	{
		if (other != null)
		{
			if (other.Id.Length != 0)
			{
				Id = other.Id;
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
				Id = input.ReadString();
			}
		}
	}
}
