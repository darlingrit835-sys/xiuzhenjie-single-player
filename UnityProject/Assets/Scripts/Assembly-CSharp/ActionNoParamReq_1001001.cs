using System;
using System.Diagnostics;
using Google.Protobuf;
using Google.Protobuf.Reflection;

public sealed class ActionNoParamReq_1001001 : IMessage<ActionNoParamReq_1001001>, IMessage, IEquatable<ActionNoParamReq_1001001>, IDeepCloneable<ActionNoParamReq_1001001>
{
	private static readonly MessageParser<ActionNoParamReq_1001001> _parser = new MessageParser<ActionNoParamReq_1001001>(() => new ActionNoParamReq_1001001());

	private UnknownFieldSet _unknownFields;

	[DebuggerNonUserCode]
	public static MessageParser<ActionNoParamReq_1001001> Parser => _parser;

	[DebuggerNonUserCode]
	public static MessageDescriptor Descriptor => SimpleActionProtoReflection.Descriptor.MessageTypes[0];

	[DebuggerNonUserCode]
	MessageDescriptor IMessage.Descriptor => pb_003A_003AGoogle_002EProtobuf_002EIMessage_002Eget_Descriptor();

	private MessageDescriptor pb_003A_003AGoogle_002EProtobuf_002EIMessage_002Eget_Descriptor()
	{
		return Descriptor;
	}

	[DebuggerNonUserCode]
	public ActionNoParamReq_1001001()
	{
	}

	[DebuggerNonUserCode]
	public ActionNoParamReq_1001001(ActionNoParamReq_1001001 other)
		: this()
	{
		_unknownFields = UnknownFieldSet.Clone(other._unknownFields);
	}

	[DebuggerNonUserCode]
	public ActionNoParamReq_1001001 Clone()
	{
		return new ActionNoParamReq_1001001(this);
	}

	[DebuggerNonUserCode]
	public override bool Equals(object other)
	{
		return Equals(other as ActionNoParamReq_1001001);
	}

	[DebuggerNonUserCode]
	public bool Equals(ActionNoParamReq_1001001 other)
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
	public void MergeFrom(ActionNoParamReq_1001001 other)
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
