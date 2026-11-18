using System;
using System.Diagnostics;
using Google.Protobuf;
using Google.Protobuf.Reflection;

public sealed class OnShelfTreasureReq_1006013 : IMessage<OnShelfTreasureReq_1006013>, IMessage, IEquatable<OnShelfTreasureReq_1006013>, IDeepCloneable<OnShelfTreasureReq_1006013>
{
	private static readonly MessageParser<OnShelfTreasureReq_1006013> _parser = new MessageParser<OnShelfTreasureReq_1006013>(() => new OnShelfTreasureReq_1006013());

	private UnknownFieldSet _unknownFields;

	public const int GoodsIdFieldNumber = 1;

	private string goodsId_ = "";

	public const int StartPriceFieldNumber = 2;

	private long startPrice_;

	[DebuggerNonUserCode]
	public static MessageParser<OnShelfTreasureReq_1006013> Parser => _parser;

	[DebuggerNonUserCode]
	public static MessageDescriptor Descriptor => AuctionProtoReflection.Descriptor.MessageTypes[12];

	[DebuggerNonUserCode]
	MessageDescriptor IMessage.Descriptor => pb_003A_003AGoogle_002EProtobuf_002EIMessage_002Eget_Descriptor();

	[DebuggerNonUserCode]
	public string GoodsId
	{
		get
		{
			return goodsId_;
		}
		set
		{
			goodsId_ = ProtoPreconditions.CheckNotNull(value, "value");
		}
	}

	[DebuggerNonUserCode]
	public long StartPrice
	{
		get
		{
			return startPrice_;
		}
		set
		{
			startPrice_ = value;
		}
	}

	private MessageDescriptor pb_003A_003AGoogle_002EProtobuf_002EIMessage_002Eget_Descriptor()
	{
		return Descriptor;
	}

	[DebuggerNonUserCode]
	public OnShelfTreasureReq_1006013()
	{
	}

	[DebuggerNonUserCode]
	public OnShelfTreasureReq_1006013(OnShelfTreasureReq_1006013 other)
		: this()
	{
		goodsId_ = other.goodsId_;
		startPrice_ = other.startPrice_;
		_unknownFields = UnknownFieldSet.Clone(other._unknownFields);
	}

	[DebuggerNonUserCode]
	public OnShelfTreasureReq_1006013 Clone()
	{
		return new OnShelfTreasureReq_1006013(this);
	}

	[DebuggerNonUserCode]
	public override bool Equals(object other)
	{
		return Equals(other as OnShelfTreasureReq_1006013);
	}

	[DebuggerNonUserCode]
	public bool Equals(OnShelfTreasureReq_1006013 other)
	{
		if (other == null)
		{
			return false;
		}
		if (other == this)
		{
			return true;
		}
		if (GoodsId != other.GoodsId)
		{
			return false;
		}
		if (StartPrice != other.StartPrice)
		{
			return false;
		}
		return object.Equals(_unknownFields, other._unknownFields);
	}

	[DebuggerNonUserCode]
	public override int GetHashCode()
	{
		int num = 1;
		if (GoodsId.Length != 0)
		{
			num ^= GoodsId.GetHashCode();
		}
		if (StartPrice != 0L)
		{
			num ^= StartPrice.GetHashCode();
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
		if (GoodsId.Length != 0)
		{
			output.WriteRawTag(10);
			output.WriteString(GoodsId);
		}
		if (StartPrice != 0L)
		{
			output.WriteRawTag(16);
			output.WriteInt64(StartPrice);
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
		if (GoodsId.Length != 0)
		{
			num += 1 + CodedOutputStream.ComputeStringSize(GoodsId);
		}
		if (StartPrice != 0L)
		{
			num += 1 + CodedOutputStream.ComputeInt64Size(StartPrice);
		}
		if (_unknownFields != null)
		{
			num += _unknownFields.CalculateSize();
		}
		return num;
	}

	[DebuggerNonUserCode]
	public void MergeFrom(OnShelfTreasureReq_1006013 other)
	{
		if (other != null)
		{
			if (other.GoodsId.Length != 0)
			{
				GoodsId = other.GoodsId;
			}
			if (other.StartPrice != 0L)
			{
				StartPrice = other.StartPrice;
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
				GoodsId = input.ReadString();
				break;
			case 16u:
				StartPrice = input.ReadInt64();
				break;
			}
		}
	}
}
