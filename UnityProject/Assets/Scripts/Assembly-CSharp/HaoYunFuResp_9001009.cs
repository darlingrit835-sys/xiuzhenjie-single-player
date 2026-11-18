using System;
using System.Diagnostics;
using Google.Protobuf;
using Google.Protobuf.Reflection;

public sealed class HaoYunFuResp_9001009 : IMessage<HaoYunFuResp_9001009>, IMessage, IEquatable<HaoYunFuResp_9001009>, IDeepCloneable<HaoYunFuResp_9001009>
{
	private static readonly MessageParser<HaoYunFuResp_9001009> _parser = new MessageParser<HaoYunFuResp_9001009>(() => new HaoYunFuResp_9001009());

	private UnknownFieldSet _unknownFields;

	[DebuggerNonUserCode]
	public static MessageParser<HaoYunFuResp_9001009> Parser => _parser;

	[DebuggerNonUserCode]
	public static MessageDescriptor Descriptor => ServerInitiativeMsgProtoReflection.Descriptor.MessageTypes[9];

	[DebuggerNonUserCode]
	MessageDescriptor IMessage.Descriptor => pb_003A_003AGoogle_002EProtobuf_002EIMessage_002Eget_Descriptor();

	private MessageDescriptor pb_003A_003AGoogle_002EProtobuf_002EIMessage_002Eget_Descriptor()
	{
		return Descriptor;
	}

	[DebuggerNonUserCode]
	public HaoYunFuResp_9001009()
	{
	}

	[DebuggerNonUserCode]
	public HaoYunFuResp_9001009(HaoYunFuResp_9001009 other)
		: this()
	{
		_unknownFields = UnknownFieldSet.Clone(other._unknownFields);
	}

	[DebuggerNonUserCode]
	public HaoYunFuResp_9001009 Clone()
	{
		return new HaoYunFuResp_9001009(this);
	}

	[DebuggerNonUserCode]
	public override bool Equals(object other)
	{
		return Equals(other as HaoYunFuResp_9001009);
	}

	[DebuggerNonUserCode]
	public bool Equals(HaoYunFuResp_9001009 other)
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
	public void MergeFrom(HaoYunFuResp_9001009 other)
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
