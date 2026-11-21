using System;
using System.Diagnostics;
using Google.Protobuf;
using Google.Protobuf.Collections;
using Google.Protobuf.Reflection;

public sealed class SearchIsOnLineReq_4001005 : IMessage<SearchIsOnLineReq_4001005>, IMessage, IEquatable<SearchIsOnLineReq_4001005>, IDeepCloneable<SearchIsOnLineReq_4001005>
{
	private static readonly MessageParser<SearchIsOnLineReq_4001005> _parser = new MessageParser<SearchIsOnLineReq_4001005>(() => new SearchIsOnLineReq_4001005());

	private UnknownFieldSet _unknownFields;

	public const int RoleIdFieldNumber = 1;

	private static readonly FieldCodec<string> _repeated_roleId_codec = FieldCodec.ForString(10u);

	private readonly RepeatedField<string> roleId_ = new RepeatedField<string>();

	[DebuggerNonUserCode]
	public static MessageParser<SearchIsOnLineReq_4001005> Parser => _parser;

	[DebuggerNonUserCode]
	public static MessageDescriptor Descriptor => ClientChatProtoReflection.Descriptor.MessageTypes[7];

	[DebuggerNonUserCode]
	MessageDescriptor IMessage.Descriptor => pb_003A_003AGoogle_002EProtobuf_002EIMessage_002Eget_Descriptor();

	[DebuggerNonUserCode]
	public RepeatedField<string> RoleId => roleId_;

	private MessageDescriptor pb_003A_003AGoogle_002EProtobuf_002EIMessage_002Eget_Descriptor()
	{
		return Descriptor;
	}

	[DebuggerNonUserCode]
	public SearchIsOnLineReq_4001005()
	{
	}

	[DebuggerNonUserCode]
	public SearchIsOnLineReq_4001005(SearchIsOnLineReq_4001005 other)
		: this()
	{
		roleId_ = other.roleId_.Clone();
		_unknownFields = UnknownFieldSet.Clone(other._unknownFields);
	}

	[DebuggerNonUserCode]
	public SearchIsOnLineReq_4001005 Clone()
	{
		return new SearchIsOnLineReq_4001005(this);
	}

	[DebuggerNonUserCode]
	public override bool Equals(object other)
	{
		return Equals(other as SearchIsOnLineReq_4001005);
	}

	[DebuggerNonUserCode]
	public bool Equals(SearchIsOnLineReq_4001005 other)
	{
		if (other == null)
		{
			return false;
		}
		if (other == this)
		{
			return true;
		}
		if (!roleId_.Equals(other.roleId_))
		{
			return false;
		}
		return object.Equals(_unknownFields, other._unknownFields);
	}

	[DebuggerNonUserCode]
	public override int GetHashCode()
	{
		int num = 1;
		num ^= roleId_.GetHashCode();
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
		roleId_.WriteTo(output, _repeated_roleId_codec);
		if (_unknownFields != null)
		{
			_unknownFields.WriteTo(output);
		}
	}

	[DebuggerNonUserCode]
	public int CalculateSize()
	{
		int num = 0;
		num += roleId_.CalculateSize(_repeated_roleId_codec);
		if (_unknownFields != null)
		{
			num += _unknownFields.CalculateSize();
		}
		return num;
	}

	[DebuggerNonUserCode]
	public void MergeFrom(SearchIsOnLineReq_4001005 other)
	{
		if (other != null)
		{
			roleId_.Add(other.roleId_);
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
				roleId_.AddEntriesFrom(input, _repeated_roleId_codec);
			}
		}
	}
}
