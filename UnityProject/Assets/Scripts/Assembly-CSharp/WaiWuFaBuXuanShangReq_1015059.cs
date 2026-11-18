using System;
using System.Diagnostics;
using Google.Protobuf;
using Google.Protobuf.Reflection;

public sealed class WaiWuFaBuXuanShangReq_1015059 : IMessage<WaiWuFaBuXuanShangReq_1015059>, IMessage, IEquatable<WaiWuFaBuXuanShangReq_1015059>, IDeepCloneable<WaiWuFaBuXuanShangReq_1015059>
{
	private static readonly MessageParser<WaiWuFaBuXuanShangReq_1015059> _parser = new MessageParser<WaiWuFaBuXuanShangReq_1015059>(() => new WaiWuFaBuXuanShangReq_1015059());

	private UnknownFieldSet _unknownFields;

	public const int GoodsTypeFieldNumber = 1;

	private int goodsType_;

	public const int GoodsTemplateIdFieldNumber = 2;

	private string goodsTemplateId_ = "";

	public const int AwardFieldNumber = 3;

	private int award_;

	[DebuggerNonUserCode]
	public static MessageParser<WaiWuFaBuXuanShangReq_1015059> Parser => _parser;

	[DebuggerNonUserCode]
	public static MessageDescriptor Descriptor => GuildProtoReflection.Descriptor.MessageTypes[50];

	[DebuggerNonUserCode]
	MessageDescriptor IMessage.Descriptor => pb_003A_003AGoogle_002EProtobuf_002EIMessage_002Eget_Descriptor();

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
	public int Award
	{
		get
		{
			return award_;
		}
		set
		{
			award_ = value;
		}
	}

	private MessageDescriptor pb_003A_003AGoogle_002EProtobuf_002EIMessage_002Eget_Descriptor()
	{
		return Descriptor;
	}

	[DebuggerNonUserCode]
	public WaiWuFaBuXuanShangReq_1015059()
	{
	}

	[DebuggerNonUserCode]
	public WaiWuFaBuXuanShangReq_1015059(WaiWuFaBuXuanShangReq_1015059 other)
		: this()
	{
		goodsType_ = other.goodsType_;
		goodsTemplateId_ = other.goodsTemplateId_;
		award_ = other.award_;
		_unknownFields = UnknownFieldSet.Clone(other._unknownFields);
	}

	[DebuggerNonUserCode]
	public WaiWuFaBuXuanShangReq_1015059 Clone()
	{
		return new WaiWuFaBuXuanShangReq_1015059(this);
	}

	[DebuggerNonUserCode]
	public override bool Equals(object other)
	{
		return Equals(other as WaiWuFaBuXuanShangReq_1015059);
	}

	[DebuggerNonUserCode]
	public bool Equals(WaiWuFaBuXuanShangReq_1015059 other)
	{
		if (other == null)
		{
			return false;
		}
		if (other == this)
		{
			return true;
		}
		if (GoodsType != other.GoodsType)
		{
			return false;
		}
		if (GoodsTemplateId != other.GoodsTemplateId)
		{
			return false;
		}
		if (Award != other.Award)
		{
			return false;
		}
		return object.Equals(_unknownFields, other._unknownFields);
	}

	[DebuggerNonUserCode]
	public override int GetHashCode()
	{
		int num = 1;
		if (GoodsType != 0)
		{
			num ^= GoodsType.GetHashCode();
		}
		if (GoodsTemplateId.Length != 0)
		{
			num ^= GoodsTemplateId.GetHashCode();
		}
		if (Award != 0)
		{
			num ^= Award.GetHashCode();
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
		if (GoodsType != 0)
		{
			output.WriteRawTag(8);
			output.WriteInt32(GoodsType);
		}
		if (GoodsTemplateId.Length != 0)
		{
			output.WriteRawTag(18);
			output.WriteString(GoodsTemplateId);
		}
		if (Award != 0)
		{
			output.WriteRawTag(24);
			output.WriteInt32(Award);
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
		if (GoodsType != 0)
		{
			num += 1 + CodedOutputStream.ComputeInt32Size(GoodsType);
		}
		if (GoodsTemplateId.Length != 0)
		{
			num += 1 + CodedOutputStream.ComputeStringSize(GoodsTemplateId);
		}
		if (Award != 0)
		{
			num += 1 + CodedOutputStream.ComputeInt32Size(Award);
		}
		if (_unknownFields != null)
		{
			num += _unknownFields.CalculateSize();
		}
		return num;
	}

	[DebuggerNonUserCode]
	public void MergeFrom(WaiWuFaBuXuanShangReq_1015059 other)
	{
		if (other != null)
		{
			if (other.GoodsType != 0)
			{
				GoodsType = other.GoodsType;
			}
			if (other.GoodsTemplateId.Length != 0)
			{
				GoodsTemplateId = other.GoodsTemplateId;
			}
			if (other.Award != 0)
			{
				Award = other.Award;
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
			case 8u:
				GoodsType = input.ReadInt32();
				break;
			case 18u:
				GoodsTemplateId = input.ReadString();
				break;
			case 24u:
				Award = input.ReadInt32();
				break;
			}
		}
	}
}
