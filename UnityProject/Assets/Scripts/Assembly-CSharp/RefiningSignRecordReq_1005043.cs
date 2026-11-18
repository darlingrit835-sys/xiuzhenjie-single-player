using System;
using System.Diagnostics;
using Google.Protobuf;
using Google.Protobuf.Reflection;

public sealed class RefiningSignRecordReq_1005043 : IMessage<RefiningSignRecordReq_1005043>, IMessage, IEquatable<RefiningSignRecordReq_1005043>, IDeepCloneable<RefiningSignRecordReq_1005043>
{
	private static readonly MessageParser<RefiningSignRecordReq_1005043> _parser = new MessageParser<RefiningSignRecordReq_1005043>(() => new RefiningSignRecordReq_1005043());

	private UnknownFieldSet _unknownFields;

	[DebuggerNonUserCode]
	public static MessageParser<RefiningSignRecordReq_1005043> Parser => _parser;

	[DebuggerNonUserCode]
	public static MessageDescriptor Descriptor => HomeProtoReflection.Descriptor.MessageTypes[55];

	[DebuggerNonUserCode]
	MessageDescriptor IMessage.Descriptor => pb_003A_003AGoogle_002EProtobuf_002EIMessage_002Eget_Descriptor();

	private MessageDescriptor pb_003A_003AGoogle_002EProtobuf_002EIMessage_002Eget_Descriptor()
	{
		return Descriptor;
	}

	[DebuggerNonUserCode]
	public RefiningSignRecordReq_1005043()
	{
	}

	[DebuggerNonUserCode]
	public RefiningSignRecordReq_1005043(RefiningSignRecordReq_1005043 other)
		: this()
	{
		_unknownFields = UnknownFieldSet.Clone(other._unknownFields);
	}

	[DebuggerNonUserCode]
	public RefiningSignRecordReq_1005043 Clone()
	{
		return new RefiningSignRecordReq_1005043(this);
	}

	[DebuggerNonUserCode]
	public override bool Equals(object other)
	{
		return Equals(other as RefiningSignRecordReq_1005043);
	}

	[DebuggerNonUserCode]
	public bool Equals(RefiningSignRecordReq_1005043 other)
	{
		if (other == null)
		{
			return false;
		}
		if (other == this)
		{
			return true;
		}
		return object.Equals(_unknownFields, other._unknownFields);
	}

	[DebuggerNonUserCode]
	public override int GetHashCode()
	{
		int num = 1;
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
		if (_unknownFields != null)
		{
			_unknownFields.WriteTo(output);
		}
	}

	[DebuggerNonUserCode]
	public int CalculateSize()
	{
		int num = 0;
		if (_unknownFields != null)
		{
			num += _unknownFields.CalculateSize();
		}
		return num;
	}

	[DebuggerNonUserCode]
	public void MergeFrom(RefiningSignRecordReq_1005043 other)
	{
		if (other != null)
		{
			_unknownFields = UnknownFieldSet.MergeFrom(_unknownFields, other._unknownFields);
		}
	}

	[DebuggerNonUserCode]
	public void MergeFrom(CodedInputStream input)
	{
		while (input.ReadTag() != 0)
		{
			_unknownFields = UnknownFieldSet.MergeFieldFrom(_unknownFields, input);
		}
	}
}
