using System;
using System.Diagnostics;
using Google.Protobuf;
using Google.Protobuf.Reflection;

public sealed class EnterAuctionReq_1006001 : IMessage<EnterAuctionReq_1006001>, IMessage, IEquatable<EnterAuctionReq_1006001>, IDeepCloneable<EnterAuctionReq_1006001>
{
	private static readonly MessageParser<EnterAuctionReq_1006001> _parser = new MessageParser<EnterAuctionReq_1006001>(() => new EnterAuctionReq_1006001());

	private UnknownFieldSet _unknownFields;

	public const int PageNumFieldNumber = 1;

	private int pageNum_;

	public const int PageSizeFieldNumber = 2;

	private int pageSize_;

	public const int NameFieldNumber = 3;

	private string name_ = "";

	public const int GoodsTypeFieldNumber = 4;

	private int goodsType_;

	public const int TypeFieldNumber = 5;

	private int type_;

	public const int FiveElementFieldNumber = 6;

	private int fiveElement_;

	public const int StageFieldNumber = 7;

	private int stage_;

	public const int MinFinalPriceFieldNumber = 8;

	private long minFinalPrice_;

	public const int MaxFinalPriceFieldNumber = 9;

	private long maxFinalPrice_;

	public const int FinalPriceSortFieldNumber = 10;

	private string finalPriceSort_ = "";

	public const int RankSortFieldNumber = 11;

	private string rankSort_ = "";

	public const int ShopTypeFieldNumber = 12;

	private int shopType_;

	public const int RankFieldNumber = 13;

	private int rank_;

	[DebuggerNonUserCode]
	public static MessageParser<EnterAuctionReq_1006001> Parser => _parser;

	[DebuggerNonUserCode]
	public static MessageDescriptor Descriptor => AuctionProtoReflection.Descriptor.MessageTypes[0];

	[DebuggerNonUserCode]
	MessageDescriptor IMessage.Descriptor => pb_003A_003AGoogle_002EProtobuf_002EIMessage_002Eget_Descriptor();

	[DebuggerNonUserCode]
	public int PageNum
	{
		get
		{
			return pageNum_;
		}
		set
		{
			pageNum_ = value;
		}
	}

	[DebuggerNonUserCode]
	public int PageSize
	{
		get
		{
			return pageSize_;
		}
		set
		{
			pageSize_ = value;
		}
	}

	[DebuggerNonUserCode]
	public string Name
	{
		get
		{
			return name_;
		}
		set
		{
			name_ = ProtoPreconditions.CheckNotNull(value, "value");
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

	[DebuggerNonUserCode]
	public int FiveElement
	{
		get
		{
			return fiveElement_;
		}
		set
		{
			fiveElement_ = value;
		}
	}

	[DebuggerNonUserCode]
	public int Stage
	{
		get
		{
			return stage_;
		}
		set
		{
			stage_ = value;
		}
	}

	[DebuggerNonUserCode]
	public long MinFinalPrice
	{
		get
		{
			return minFinalPrice_;
		}
		set
		{
			minFinalPrice_ = value;
		}
	}

	[DebuggerNonUserCode]
	public long MaxFinalPrice
	{
		get
		{
			return maxFinalPrice_;
		}
		set
		{
			maxFinalPrice_ = value;
		}
	}

	[DebuggerNonUserCode]
	public string FinalPriceSort
	{
		get
		{
			return finalPriceSort_;
		}
		set
		{
			finalPriceSort_ = ProtoPreconditions.CheckNotNull(value, "value");
		}
	}

	[DebuggerNonUserCode]
	public string RankSort
	{
		get
		{
			return rankSort_;
		}
		set
		{
			rankSort_ = ProtoPreconditions.CheckNotNull(value, "value");
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
	public int Rank
	{
		get
		{
			return rank_;
		}
		set
		{
			rank_ = value;
		}
	}

	private MessageDescriptor pb_003A_003AGoogle_002EProtobuf_002EIMessage_002Eget_Descriptor()
	{
		return Descriptor;
	}

	[DebuggerNonUserCode]
	public EnterAuctionReq_1006001()
	{
	}

	[DebuggerNonUserCode]
	public EnterAuctionReq_1006001(EnterAuctionReq_1006001 other)
		: this()
	{
		pageNum_ = other.pageNum_;
		pageSize_ = other.pageSize_;
		name_ = other.name_;
		goodsType_ = other.goodsType_;
		type_ = other.type_;
		fiveElement_ = other.fiveElement_;
		stage_ = other.stage_;
		minFinalPrice_ = other.minFinalPrice_;
		maxFinalPrice_ = other.maxFinalPrice_;
		finalPriceSort_ = other.finalPriceSort_;
		rankSort_ = other.rankSort_;
		shopType_ = other.shopType_;
		rank_ = other.rank_;
		_unknownFields = UnknownFieldSet.Clone(other._unknownFields);
	}

	[DebuggerNonUserCode]
	public EnterAuctionReq_1006001 Clone()
	{
		return new EnterAuctionReq_1006001(this);
	}

	[DebuggerNonUserCode]
	public override bool Equals(object other)
	{
		return Equals(other as EnterAuctionReq_1006001);
	}

	[DebuggerNonUserCode]
	public bool Equals(EnterAuctionReq_1006001 other)
	{
		if (other == null)
		{
			return false;
		}
		if (other == this)
		{
			return true;
		}
		if (PageNum != other.PageNum)
		{
			return false;
		}
		if (PageSize != other.PageSize)
		{
			return false;
		}
		if (Name != other.Name)
		{
			return false;
		}
		if (GoodsType != other.GoodsType)
		{
			return false;
		}
		if (Type != other.Type)
		{
			return false;
		}
		if (FiveElement != other.FiveElement)
		{
			return false;
		}
		if (Stage != other.Stage)
		{
			return false;
		}
		if (MinFinalPrice != other.MinFinalPrice)
		{
			return false;
		}
		if (MaxFinalPrice != other.MaxFinalPrice)
		{
			return false;
		}
		if (FinalPriceSort != other.FinalPriceSort)
		{
			return false;
		}
		if (RankSort != other.RankSort)
		{
			return false;
		}
		if (ShopType != other.ShopType)
		{
			return false;
		}
		if (Rank != other.Rank)
		{
			return false;
		}
		return object.Equals(_unknownFields, other._unknownFields);
	}

	[DebuggerNonUserCode]
	public override int GetHashCode()
	{
		int num = 1;
		if (PageNum != 0)
		{
			num ^= PageNum.GetHashCode();
		}
		if (PageSize != 0)
		{
			num ^= PageSize.GetHashCode();
		}
		if (Name.Length != 0)
		{
			num ^= Name.GetHashCode();
		}
		if (GoodsType != 0)
		{
			num ^= GoodsType.GetHashCode();
		}
		if (Type != 0)
		{
			num ^= Type.GetHashCode();
		}
		if (FiveElement != 0)
		{
			num ^= FiveElement.GetHashCode();
		}
		if (Stage != 0)
		{
			num ^= Stage.GetHashCode();
		}
		if (MinFinalPrice != 0L)
		{
			num ^= MinFinalPrice.GetHashCode();
		}
		if (MaxFinalPrice != 0L)
		{
			num ^= MaxFinalPrice.GetHashCode();
		}
		if (FinalPriceSort.Length != 0)
		{
			num ^= FinalPriceSort.GetHashCode();
		}
		if (RankSort.Length != 0)
		{
			num ^= RankSort.GetHashCode();
		}
		if (ShopType != 0)
		{
			num ^= ShopType.GetHashCode();
		}
		if (Rank != 0)
		{
			num ^= Rank.GetHashCode();
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
		if (PageNum != 0)
		{
			output.WriteRawTag(8);
			output.WriteInt32(PageNum);
		}
		if (PageSize != 0)
		{
			output.WriteRawTag(16);
			output.WriteInt32(PageSize);
		}
		if (Name.Length != 0)
		{
			output.WriteRawTag(26);
			output.WriteString(Name);
		}
		if (GoodsType != 0)
		{
			output.WriteRawTag(32);
			output.WriteInt32(GoodsType);
		}
		if (Type != 0)
		{
			output.WriteRawTag(40);
			output.WriteInt32(Type);
		}
		if (FiveElement != 0)
		{
			output.WriteRawTag(48);
			output.WriteInt32(FiveElement);
		}
		if (Stage != 0)
		{
			output.WriteRawTag(56);
			output.WriteInt32(Stage);
		}
		if (MinFinalPrice != 0L)
		{
			output.WriteRawTag(64);
			output.WriteInt64(MinFinalPrice);
		}
		if (MaxFinalPrice != 0L)
		{
			output.WriteRawTag(72);
			output.WriteInt64(MaxFinalPrice);
		}
		if (FinalPriceSort.Length != 0)
		{
			output.WriteRawTag(82);
			output.WriteString(FinalPriceSort);
		}
		if (RankSort.Length != 0)
		{
			output.WriteRawTag(90);
			output.WriteString(RankSort);
		}
		if (ShopType != 0)
		{
			output.WriteRawTag(96);
			output.WriteInt32(ShopType);
		}
		if (Rank != 0)
		{
			output.WriteRawTag(104);
			output.WriteInt32(Rank);
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
		if (PageNum != 0)
		{
			num += 1 + CodedOutputStream.ComputeInt32Size(PageNum);
		}
		if (PageSize != 0)
		{
			num += 1 + CodedOutputStream.ComputeInt32Size(PageSize);
		}
		if (Name.Length != 0)
		{
			num += 1 + CodedOutputStream.ComputeStringSize(Name);
		}
		if (GoodsType != 0)
		{
			num += 1 + CodedOutputStream.ComputeInt32Size(GoodsType);
		}
		if (Type != 0)
		{
			num += 1 + CodedOutputStream.ComputeInt32Size(Type);
		}
		if (FiveElement != 0)
		{
			num += 1 + CodedOutputStream.ComputeInt32Size(FiveElement);
		}
		if (Stage != 0)
		{
			num += 1 + CodedOutputStream.ComputeInt32Size(Stage);
		}
		if (MinFinalPrice != 0L)
		{
			num += 1 + CodedOutputStream.ComputeInt64Size(MinFinalPrice);
		}
		if (MaxFinalPrice != 0L)
		{
			num += 1 + CodedOutputStream.ComputeInt64Size(MaxFinalPrice);
		}
		if (FinalPriceSort.Length != 0)
		{
			num += 1 + CodedOutputStream.ComputeStringSize(FinalPriceSort);
		}
		if (RankSort.Length != 0)
		{
			num += 1 + CodedOutputStream.ComputeStringSize(RankSort);
		}
		if (ShopType != 0)
		{
			num += 1 + CodedOutputStream.ComputeInt32Size(ShopType);
		}
		if (Rank != 0)
		{
			num += 1 + CodedOutputStream.ComputeInt32Size(Rank);
		}
		if (_unknownFields != null)
		{
			num += _unknownFields.CalculateSize();
		}
		return num;
	}

	[DebuggerNonUserCode]
	public void MergeFrom(EnterAuctionReq_1006001 other)
	{
		if (other != null)
		{
			if (other.PageNum != 0)
			{
				PageNum = other.PageNum;
			}
			if (other.PageSize != 0)
			{
				PageSize = other.PageSize;
			}
			if (other.Name.Length != 0)
			{
				Name = other.Name;
			}
			if (other.GoodsType != 0)
			{
				GoodsType = other.GoodsType;
			}
			if (other.Type != 0)
			{
				Type = other.Type;
			}
			if (other.FiveElement != 0)
			{
				FiveElement = other.FiveElement;
			}
			if (other.Stage != 0)
			{
				Stage = other.Stage;
			}
			if (other.MinFinalPrice != 0L)
			{
				MinFinalPrice = other.MinFinalPrice;
			}
			if (other.MaxFinalPrice != 0L)
			{
				MaxFinalPrice = other.MaxFinalPrice;
			}
			if (other.FinalPriceSort.Length != 0)
			{
				FinalPriceSort = other.FinalPriceSort;
			}
			if (other.RankSort.Length != 0)
			{
				RankSort = other.RankSort;
			}
			if (other.ShopType != 0)
			{
				ShopType = other.ShopType;
			}
			if (other.Rank != 0)
			{
				Rank = other.Rank;
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
				PageNum = input.ReadInt32();
				break;
			case 16u:
				PageSize = input.ReadInt32();
				break;
			case 26u:
				Name = input.ReadString();
				break;
			case 32u:
				GoodsType = input.ReadInt32();
				break;
			case 40u:
				Type = input.ReadInt32();
				break;
			case 48u:
				FiveElement = input.ReadInt32();
				break;
			case 56u:
				Stage = input.ReadInt32();
				break;
			case 64u:
				MinFinalPrice = input.ReadInt64();
				break;
			case 72u:
				MaxFinalPrice = input.ReadInt64();
				break;
			case 82u:
				FinalPriceSort = input.ReadString();
				break;
			case 90u:
				RankSort = input.ReadString();
				break;
			case 96u:
				ShopType = input.ReadInt32();
				break;
			case 104u:
				Rank = input.ReadInt32();
				break;
			}
		}
	}
}
