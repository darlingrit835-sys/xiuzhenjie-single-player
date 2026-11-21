using System;
using System.Diagnostics;
using Google.Protobuf;
using Google.Protobuf.Reflection;

public sealed class GouMaiWuPinReq_1015146 : IMessage<GouMaiWuPinReq_1015146>, IMessage, IEquatable<GouMaiWuPinReq_1015146>, IDeepCloneable<GouMaiWuPinReq_1015146>
{
	private static readonly MessageParser<GouMaiWuPinReq_1015146> _parser = new MessageParser<GouMaiWuPinReq_1015146>(() => new GouMaiWuPinReq_1015146());

	private UnknownFieldSet _unknownFields;

	public const int GoodsTemplateIdFieldNumber = 1;

	private string goodsTemplateId_ = "";

	public const int GoodsTypeFieldNumber = 2;

	private int goodsType_;

	[DebuggerNonUserCode]
	public static MessageParser<GouMaiWuPinReq_1015146> Parser => _parser;

	[DebuggerNonUserCode]
	public static MessageDescriptor Descriptor => GuildProtoReflection.Descriptor.MessageTypes[186];

	[DebuggerNonUserCode]
	MessageDescriptor IMessage.Descriptor => pb_003A_003AGoogle_002EProtobuf_002EIMessage_002Eget_Descriptor();

	[DebuggerNonUserCode]
	public string GoodsTemplateId
	{
		get
		{
			return goodsTemplateId_;
		}
		set
		{
			goodsTemplateId_ = ProtoPreconditions.CheckNotNull(value, "value");
		}
	}

	[DebuggerNonUserCode]
	public int GoodsType
	{
		get
		{
			return goodsType_;
		}
		set
		{
			goodsType_ = value;
		}
	}

	private MessageDescriptor pb_003A_003AGoogle_002EProtobuf_002EIMessage_002Eget_Descriptor()
	{
		return Descriptor;
	}

	[DebuggerNonUserCode]
	public GouMaiWuPinReq_1015146()
	{
	}

	[DebuggerNonUserCode]
	public GouMaiWuPinReq_1015146(GouMaiWuPinReq_1015146 other)
		: this()
	{
		goodsTemplateId_ = other.goodsTemplateId_;
		goodsType_ = other.goodsType_;
		_unknownFields = UnknownFieldSet.Clone(other._unknownFields);
	}

	[DebuggerNonUserCode]
	public GouMaiWuPinReq_1015146 Clone()
	{
		return new GouMaiWuPinReq_1015146(this);
	}

	[DebuggerNonUserCode]
	public override bool Equals(object other)
	{
		return Equals(other as GouMaiWuPinReq_1015146);
	}

	[DebuggerNonUserCode]
	public bool Equals(GouMaiWuPinReq_1015146 other)
	{
		if (other == null)
		{
			return false;
		}
		if (other == this)
		{
			return true;
		}
		if (GoodsTemplateId != other.GoodsTemplateId)
		{
			return false;
		}
		if (GoodsType != other.GoodsType)
		{
			return false;
		}
		return object.Equals(_unknownFields, other._unknownFields);
	}

	[DebuggerNonUserCode]
	public override int GetHashCode()
	{
		int num = 1;
		if (GoodsTemplateId.Length != 0)
		{
			num ^= GoodsTemplateId.GetHashCode();
		}
		if (GoodsType != 0)
		{
			num ^= GoodsType.GetHashCode();
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
		if (GoodsTemplateId.Length != 0)
		{
			output.WriteRawTag(10);
			output.WriteString(GoodsTemplateId);
		}
		if (GoodsType != 0)
		{
			output.WriteRawTag(16);
			output.WriteInt32(GoodsType);
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
		if (GoodsTemplateId.Length != 0)
		{
			num += 1 + CodedOutputStream.ComputeStringSize(GoodsTemplateId);
		}
		if (GoodsType != 0)
		{
			num += 1 + CodedOutputStream.ComputeInt32Size(GoodsType);
		}
		if (_unknownFields != null)
		{
			num += _unknownFields.CalculateSize();
		}
		return num;
	}

	[DebuggerNonUserCode]
	public void MergeFrom(GouMaiWuPinReq_1015146 other)
	{
		if (other != null)
		{
			if (other.GoodsTemplateId.Length != 0)
			{
				GoodsTemplateId = other.GoodsTemplateId;
			}
			if (other.GoodsType != 0)
			{
				GoodsType = other.GoodsType;
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
				GoodsTemplateId = input.ReadString();
				break;
			case 16u:
				GoodsType = input.ReadInt32();
				break;
			}
		}
	}
}
