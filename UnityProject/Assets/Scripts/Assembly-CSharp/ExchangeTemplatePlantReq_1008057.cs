using System;
using System.Diagnostics;
using Google.Protobuf;
using Google.Protobuf.Reflection;

public sealed class ExchangeTemplatePlantReq_1008057 : IMessage<ExchangeTemplatePlantReq_1008057>, IMessage, IEquatable<ExchangeTemplatePlantReq_1008057>, IDeepCloneable<ExchangeTemplatePlantReq_1008057>
{
	private static readonly MessageParser<ExchangeTemplatePlantReq_1008057> _parser = new MessageParser<ExchangeTemplatePlantReq_1008057>(() => new ExchangeTemplatePlantReq_1008057());

	private UnknownFieldSet _unknownFields;

	public const int TemplatePlantIdFieldNumber = 1;

	private string templatePlantId_ = "";

	[DebuggerNonUserCode]
	public static MessageParser<ExchangeTemplatePlantReq_1008057> Parser => _parser;

	[DebuggerNonUserCode]
	public static MessageDescriptor Descriptor => UnrealWorldProtoReflection.Descriptor.MessageTypes[70];

	[DebuggerNonUserCode]
	MessageDescriptor IMessage.Descriptor => pb_003A_003AGoogle_002EProtobuf_002EIMessage_002Eget_Descriptor();

	[DebuggerNonUserCode]
	public string TemplatePlantId
	{
		get
		{
			return templatePlantId_;
		}
		set
		{
			templatePlantId_ = ProtoPreconditions.CheckNotNull(value, "value");
		}
	}

	private MessageDescriptor pb_003A_003AGoogle_002EProtobuf_002EIMessage_002Eget_Descriptor()
	{
		return Descriptor;
	}

	[DebuggerNonUserCode]
	public ExchangeTemplatePlantReq_1008057()
	{
	}

	[DebuggerNonUserCode]
	public ExchangeTemplatePlantReq_1008057(ExchangeTemplatePlantReq_1008057 other)
		: this()
	{
		templatePlantId_ = other.templatePlantId_;
		_unknownFields = UnknownFieldSet.Clone(other._unknownFields);
	}

	[DebuggerNonUserCode]
	public ExchangeTemplatePlantReq_1008057 Clone()
	{
		return new ExchangeTemplatePlantReq_1008057(this);
	}

	[DebuggerNonUserCode]
	public override bool Equals(object other)
	{
		return Equals(other as ExchangeTemplatePlantReq_1008057);
	}

	[DebuggerNonUserCode]
	public bool Equals(ExchangeTemplatePlantReq_1008057 other)
	{
		if (other == null)
		{
			return false;
		}
		if (other == this)
		{
			return true;
		}
		if (TemplatePlantId != other.TemplatePlantId)
		{
			return false;
		}
		return object.Equals(_unknownFields, other._unknownFields);
	}

	[DebuggerNonUserCode]
	public override int GetHashCode()
	{
		int num = 1;
		if (TemplatePlantId.Length != 0)
		{
			num ^= TemplatePlantId.GetHashCode();
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
		if (TemplatePlantId.Length != 0)
		{
			output.WriteRawTag(10);
			output.WriteString(TemplatePlantId);
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
		if (TemplatePlantId.Length != 0)
		{
			num += 1 + CodedOutputStream.ComputeStringSize(TemplatePlantId);
		}
		if (_unknownFields != null)
		{
			num += _unknownFields.CalculateSize();
		}
		return num;
	}

	[DebuggerNonUserCode]
	public void MergeFrom(ExchangeTemplatePlantReq_1008057 other)
	{
		if (other != null)
		{
			if (other.TemplatePlantId.Length != 0)
			{
				TemplatePlantId = other.TemplatePlantId;
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
				TemplatePlantId = input.ReadString();
			}
		}
	}
}
