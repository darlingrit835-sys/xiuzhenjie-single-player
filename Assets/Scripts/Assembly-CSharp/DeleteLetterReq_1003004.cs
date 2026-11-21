using System;
using System.Diagnostics;
using Google.Protobuf;
using Google.Protobuf.Collections;
using Google.Protobuf.Reflection;

public sealed class DeleteLetterReq_1003004 : IMessage<DeleteLetterReq_1003004>, IMessage, IEquatable<DeleteLetterReq_1003004>, IDeepCloneable<DeleteLetterReq_1003004>
{
	private static readonly MessageParser<DeleteLetterReq_1003004> _parser = new MessageParser<DeleteLetterReq_1003004>(() => new DeleteLetterReq_1003004());

	private UnknownFieldSet _unknownFields;

	public const int IdFieldNumber = 1;

	private static readonly FieldCodec<string> _repeated_id_codec = FieldCodec.ForString(10u);

	private readonly RepeatedField<string> id_ = new RepeatedField<string>();

	[DebuggerNonUserCode]
	public static MessageParser<DeleteLetterReq_1003004> Parser => _parser;

	[DebuggerNonUserCode]
	public static MessageDescriptor Descriptor => LetterProtoReflection.Descriptor.MessageTypes[1];

	[DebuggerNonUserCode]
	MessageDescriptor IMessage.Descriptor => pb_003A_003AGoogle_002EProtobuf_002EIMessage_002Eget_Descriptor();

	[DebuggerNonUserCode]
	public RepeatedField<string> Id => id_;

	private MessageDescriptor pb_003A_003AGoogle_002EProtobuf_002EIMessage_002Eget_Descriptor()
	{
		return Descriptor;
	}

	[DebuggerNonUserCode]
	public DeleteLetterReq_1003004()
	{
	}

	[DebuggerNonUserCode]
	public DeleteLetterReq_1003004(DeleteLetterReq_1003004 other)
		: this()
	{
		id_ = other.id_.Clone();
		_unknownFields = UnknownFieldSet.Clone(other._unknownFields);
	}

	[DebuggerNonUserCode]
	public DeleteLetterReq_1003004 Clone()
	{
		return new DeleteLetterReq_1003004(this);
	}

	[DebuggerNonUserCode]
	public override bool Equals(object other)
	{
		return Equals(other as DeleteLetterReq_1003004);
	}

	[DebuggerNonUserCode]
	public bool Equals(DeleteLetterReq_1003004 other)
	{
		if (other == null)
		{
			return false;
		}
		if (other == this)
		{
			return true;
		}
		if (!id_.Equals(other.id_))
		{
			return false;
		}
		return object.Equals(_unknownFields, other._unknownFields);
	}

	[DebuggerNonUserCode]
	public override int GetHashCode()
	{
		int num = 1;
		num ^= id_.GetHashCode();
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
		id_.WriteTo(output, _repeated_id_codec);
		if (_unknownFields != null)
		{
			_unknownFields.WriteTo(output);
		}
	}

	[DebuggerNonUserCode]
	public int CalculateSize()
	{
		int num = 0;
		num += id_.CalculateSize(_repeated_id_codec);
		if (_unknownFields != null)
		{
			num += _unknownFields.CalculateSize();
		}
		return num;
	}

	[DebuggerNonUserCode]
	public void MergeFrom(DeleteLetterReq_1003004 other)
	{
		if (other != null)
		{
			id_.Add(other.id_);
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
				id_.AddEntriesFrom(input, _repeated_id_codec);
			}
		}
	}
}
