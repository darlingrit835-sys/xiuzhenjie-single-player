using System;
using System.Diagnostics;
using Google.Protobuf;
using Google.Protobuf.Reflection;

public sealed class SearchLMStockReq_1004028 : IMessage<SearchLMStockReq_1004028>, IMessage, IEquatable<SearchLMStockReq_1004028>, IDeepCloneable<SearchLMStockReq_1004028>
{
	private static readonly MessageParser<SearchLMStockReq_1004028> _parser = new MessageParser<SearchLMStockReq_1004028>(() => new SearchLMStockReq_1004028());

	private UnknownFieldSet _unknownFields;

	public const int TemplateIdFieldNumber = 1;

	private string templateId_ = "";

	public const int TypeFieldNumber = 2;

	private int type_;

	[DebuggerNonUserCode]
	public static MessageParser<SearchLMStockReq_1004028> Parser => _parser;

	[DebuggerNonUserCode]
	public static MessageDescriptor Descriptor => GoodsProtoReflection.Descriptor.MessageTypes[49];

	[DebuggerNonUserCode]
	MessageDescriptor IMessage.Descriptor => pb_003A_003AGoogle_002EProtobuf_002EIMessage_002Eget_Descriptor();

	[DebuggerNonUserCode]
	public string TemplateId
	{
		get
		{
			return templateId_;
		}
		set
		{
			templateId_ = ProtoPreconditions.CheckNotNull(value, "value");
		}
	}

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
	public SearchLMStockReq_1004028()
	{
	}

	[DebuggerNonUserCode]
	public SearchLMStockReq_1004028(SearchLMStockReq_1004028 other)
		: this()
	{
		templateId_ = other.templateId_;
		type_ = other.type_;
		_unknownFields = UnknownFieldSet.Clone(other._unknownFields);
	}

	[DebuggerNonUserCode]
	public SearchLMStockReq_1004028 Clone()
	{
		return new SearchLMStockReq_1004028(this);
	}

	[DebuggerNonUserCode]
	public override bool Equals(object other)
	{
		return Equals(other as SearchLMStockReq_1004028);
	}

	[DebuggerNonUserCode]
	public bool Equals(SearchLMStockReq_1004028 other)
	{
		if (other == null)
		{
			return false;
		}
		if (other == this)
		{
			return true;
		}
		if (TemplateId != other.TemplateId)
		{
			return false;
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
		if (TemplateId.Length != 0)
		{
			num ^= TemplateId.GetHashCode();
		}
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
		if (TemplateId.Length != 0)
		{
			output.WriteRawTag(10);
			output.WriteString(TemplateId);
		}
		if (Type != 0)
		{
			output.WriteRawTag(16);
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
		if (TemplateId.Length != 0)
		{
			num += 1 + CodedOutputStream.ComputeStringSize(TemplateId);
		}
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
	public void MergeFrom(SearchLMStockReq_1004028 other)
	{
		if (other != null)
		{
			if (other.TemplateId.Length != 0)
			{
				TemplateId = other.TemplateId;
			}
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
			switch (num)
			{
			default:
				_unknownFields = UnknownFieldSet.MergeFieldFrom(_unknownFields, input);
				break;
			case 10u:
				TemplateId = input.ReadString();
				break;
			case 16u:
				Type = input.ReadInt32();
				break;
			}
		}
	}
}
