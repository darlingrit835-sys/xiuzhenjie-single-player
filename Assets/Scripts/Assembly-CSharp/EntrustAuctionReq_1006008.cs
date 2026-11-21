using System;
using System.Diagnostics;
using Google.Protobuf;
using Google.Protobuf.Reflection;

public sealed class EntrustAuctionReq_1006008 : IMessage<EntrustAuctionReq_1006008>, IMessage, IEquatable<EntrustAuctionReq_1006008>, IDeepCloneable<EntrustAuctionReq_1006008>
{
	private static readonly MessageParser<EntrustAuctionReq_1006008> _parser = new MessageParser<EntrustAuctionReq_1006008>(() => new EntrustAuctionReq_1006008());

	private UnknownFieldSet _unknownFields;

	public const int GoodsIdFieldNumber = 1;

	private string goodsId_ = "";

	public const int FixedPriceFieldNumber = 2;

	private long fixedPrice_;

	public const int ShopTypeFieldNumber = 3;

	private int shopType_;

	public const int CountFieldNumber = 4;

	private int count_;

	[DebuggerNonUserCode]
	public static MessageParser<EntrustAuctionReq_1006008> Parser => _parser;

	[DebuggerNonUserCode]
	public static MessageDescriptor Descriptor => AuctionProtoReflection.Descriptor.MessageTypes[4];

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
	public long FixedPrice
	{
		get
		{
			return fixedPrice_;
		}
		set
		{
			fixedPrice_ = value;
		}
	}

	[DebuggerNonUserCode]
	public int ShopType
	{
		get
		{
			return shopType_;
		}
		set
		{
			shopType_ = value;
		}
	}

	[DebuggerNonUserCode]
	public int Count
	{
		get
		{
			return count_;
		}
		set
		{
			count_ = value;
		}
	}

	private MessageDescriptor pb_003A_003AGoogle_002EProtobuf_002EIMessage_002Eget_Descriptor()
	{
		return Descriptor;
	}

	[DebuggerNonUserCode]
	public EntrustAuctionReq_1006008()
	{
	}

	[DebuggerNonUserCode]
	public EntrustAuctionReq_1006008(EntrustAuctionReq_1006008 other)
		: this()
	{
		goodsId_ = other.goodsId_;
		fixedPrice_ = other.fixedPrice_;
		shopType_ = other.shopType_;
		count_ = other.count_;
		_unknownFields = UnknownFieldSet.Clone(other._unknownFields);
	}

	[DebuggerNonUserCode]
	public EntrustAuctionReq_1006008 Clone()
	{
		return new EntrustAuctionReq_1006008(this);
	}

	[DebuggerNonUserCode]
	public override bool Equals(object other)
	{
		return Equals(other as EntrustAuctionReq_1006008);
	}

	[DebuggerNonUserCode]
	public bool Equals(EntrustAuctionReq_1006008 other)
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
		if (FixedPrice != other.FixedPrice)
		{
			return false;
		}
		if (ShopType != other.ShopType)
		{
			return false;
		}
		if (Count != other.Count)
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
		if (FixedPrice != 0L)
		{
			num ^= FixedPrice.GetHashCode();
		}
		if (ShopType != 0)
		{
			num ^= ShopType.GetHashCode();
		}
		if (Count != 0)
		{
			num ^= Count.GetHashCode();
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
		if (FixedPrice != 0L)
		{
			output.WriteRawTag(16);
			output.WriteInt64(FixedPrice);
		}
		if (ShopType != 0)
		{
			output.WriteRawTag(24);
			output.WriteInt32(ShopType);
		}
		if (Count != 0)
		{
			output.WriteRawTag(32);
			output.WriteInt32(Count);
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
		if (FixedPrice != 0L)
		{
			num += 1 + CodedOutputStream.ComputeInt64Size(FixedPrice);
		}
		if (ShopType != 0)
		{
			num += 1 + CodedOutputStream.ComputeInt32Size(ShopType);
		}
		if (Count != 0)
		{
			num += 1 + CodedOutputStream.ComputeInt32Size(Count);
		}
		if (_unknownFields != null)
		{
			num += _unknownFields.CalculateSize();
		}
		return num;
	}

	[DebuggerNonUserCode]
	public void MergeFrom(EntrustAuctionReq_1006008 other)
	{
		if (other != null)
		{
			if (other.GoodsId.Length != 0)
			{
				GoodsId = other.GoodsId;
			}
			if (other.FixedPrice != 0L)
			{
				FixedPrice = other.FixedPrice;
			}
			if (other.ShopType != 0)
			{
				ShopType = other.ShopType;
			}
			if (other.Count != 0)
			{
				Count = other.Count;
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
				FixedPrice = input.ReadInt64();
				break;
			case 24u:
				ShopType = input.ReadInt32();
				break;
			case 32u:
				Count = input.ReadInt32();
				break;
			}
		}
	}
}
