using System;
using System.Diagnostics;
using Google.Protobuf;
using Google.Protobuf.Collections;
using Google.Protobuf.Reflection;

public sealed class EnterAuctionResp_1006001 : IMessage<EnterAuctionResp_1006001>, IMessage, IEquatable<EnterAuctionResp_1006001>, IDeepCloneable<EnterAuctionResp_1006001>
{
	[DebuggerNonUserCode]
	public static class Types
	{
		public sealed class AuctionObjectResp_1006002 : IMessage<AuctionObjectResp_1006002>, IMessage, IEquatable<AuctionObjectResp_1006002>, IDeepCloneable<AuctionObjectResp_1006002>
		{
			private static readonly MessageParser<AuctionObjectResp_1006002> _parser = new MessageParser<AuctionObjectResp_1006002>(() => new AuctionObjectResp_1006002());

			private UnknownFieldSet _unknownFields;

			public const int IdFieldNumber = 1;

			private string id_ = "";

			public const int GoodsIdFieldNumber = 2;

			private string goodsId_ = "";

			public const int NameFieldNumber = 3;

			private string name_ = "";

			public const int GoodsTypeFieldNumber = 4;

			private int goodsType_;

			public const int FiveElementFieldNumber = 5;

			private int fiveElement_;

			public const int FixedPriceFieldNumber = 6;

			private long fixedPrice_;

			public const int RemainHoursFieldNumber = 7;

			private int remainHours_;

			public const int StageFieldNumber = 8;

			private int stage_;

			public const int RankFieldNumber = 9;

			private int rank_;

			public const int LevelFieldNumber = 10;

			private int level_;

			public const int CountFieldNumber = 11;

			private int count_;

			[DebuggerNonUserCode]
			public static MessageParser<AuctionObjectResp_1006002> Parser => _parser;

			[DebuggerNonUserCode]
			public static MessageDescriptor Descriptor => EnterAuctionResp_1006001.Descriptor.NestedTypes[0];

			[DebuggerNonUserCode]
			MessageDescriptor IMessage.Descriptor => pb_003A_003AGoogle_002EProtobuf_002EIMessage_002Eget_Descriptor();

			[DebuggerNonUserCode]
			public string Id
			{
				get
				{
					return id_;
				}
				set
				{
					id_ = ProtoPreconditions.CheckNotNull(value, "value");
				}
			}

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
			public int RemainHours
			{
				get
				{
					return remainHours_;
				}
				set
				{
					remainHours_ = value;
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

			[DebuggerNonUserCode]
			public int Level
			{
				get
				{
					return level_;
				}
				set
				{
					level_ = value;
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
			public AuctionObjectResp_1006002()
			{
			}

			[DebuggerNonUserCode]
			public AuctionObjectResp_1006002(AuctionObjectResp_1006002 other)
				: this()
			{
				id_ = other.id_;
				goodsId_ = other.goodsId_;
				name_ = other.name_;
				goodsType_ = other.goodsType_;
				fiveElement_ = other.fiveElement_;
				fixedPrice_ = other.fixedPrice_;
				remainHours_ = other.remainHours_;
				stage_ = other.stage_;
				rank_ = other.rank_;
				level_ = other.level_;
				count_ = other.count_;
				_unknownFields = UnknownFieldSet.Clone(other._unknownFields);
			}

			[DebuggerNonUserCode]
			public AuctionObjectResp_1006002 Clone()
			{
				return new AuctionObjectResp_1006002(this);
			}

			[DebuggerNonUserCode]
			public override bool Equals(object other)
			{
				return Equals(other as AuctionObjectResp_1006002);
			}

			[DebuggerNonUserCode]
			public bool Equals(AuctionObjectResp_1006002 other)
			{
				if (other == null)
				{
					return false;
				}
				if (other == this)
				{
					return true;
				}
				if (Id != other.Id)
				{
					return false;
				}
				if (GoodsId != other.GoodsId)
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
				if (FiveElement != other.FiveElement)
				{
					return false;
				}
				if (FixedPrice != other.FixedPrice)
				{
					return false;
				}
				if (RemainHours != other.RemainHours)
				{
					return false;
				}
				if (Stage != other.Stage)
				{
					return false;
				}
				if (Rank != other.Rank)
				{
					return false;
				}
				if (Level != other.Level)
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
				if (Id.Length != 0)
				{
					num ^= Id.GetHashCode();
				}
				if (GoodsId.Length != 0)
				{
					num ^= GoodsId.GetHashCode();
				}
				if (Name.Length != 0)
				{
					num ^= Name.GetHashCode();
				}
				if (GoodsType != 0)
				{
					num ^= GoodsType.GetHashCode();
				}
				if (FiveElement != 0)
				{
					num ^= FiveElement.GetHashCode();
				}
				if (FixedPrice != 0L)
				{
					num ^= FixedPrice.GetHashCode();
				}
				if (RemainHours != 0)
				{
					num ^= RemainHours.GetHashCode();
				}
				if (Stage != 0)
				{
					num ^= Stage.GetHashCode();
				}
				if (Rank != 0)
				{
					num ^= Rank.GetHashCode();
				}
				if (Level != 0)
				{
					num ^= Level.GetHashCode();
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
				if (Id.Length != 0)
				{
					output.WriteRawTag(10);
					output.WriteString(Id);
				}
				if (GoodsId.Length != 0)
				{
					output.WriteRawTag(18);
					output.WriteString(GoodsId);
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
				if (FiveElement != 0)
				{
					output.WriteRawTag(40);
					output.WriteInt32(FiveElement);
				}
				if (FixedPrice != 0L)
				{
					output.WriteRawTag(48);
					output.WriteInt64(FixedPrice);
				}
				if (RemainHours != 0)
				{
					output.WriteRawTag(56);
					output.WriteInt32(RemainHours);
				}
				if (Stage != 0)
				{
					output.WriteRawTag(64);
					output.WriteInt32(Stage);
				}
				if (Rank != 0)
				{
					output.WriteRawTag(72);
					output.WriteInt32(Rank);
				}
				if (Level != 0)
				{
					output.WriteRawTag(80);
					output.WriteInt32(Level);
				}
				if (Count != 0)
				{
					output.WriteRawTag(88);
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
				if (Id.Length != 0)
				{
					num += 1 + CodedOutputStream.ComputeStringSize(Id);
				}
				if (GoodsId.Length != 0)
				{
					num += 1 + CodedOutputStream.ComputeStringSize(GoodsId);
				}
				if (Name.Length != 0)
				{
					num += 1 + CodedOutputStream.ComputeStringSize(Name);
				}
				if (GoodsType != 0)
				{
					num += 1 + CodedOutputStream.ComputeInt32Size(GoodsType);
				}
				if (FiveElement != 0)
				{
					num += 1 + CodedOutputStream.ComputeInt32Size(FiveElement);
				}
				if (FixedPrice != 0L)
				{
					num += 1 + CodedOutputStream.ComputeInt64Size(FixedPrice);
				}
				if (RemainHours != 0)
				{
					num += 1 + CodedOutputStream.ComputeInt32Size(RemainHours);
				}
				if (Stage != 0)
				{
					num += 1 + CodedOutputStream.ComputeInt32Size(Stage);
				}
				if (Rank != 0)
				{
					num += 1 + CodedOutputStream.ComputeInt32Size(Rank);
				}
				if (Level != 0)
				{
					num += 1 + CodedOutputStream.ComputeInt32Size(Level);
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
			public void MergeFrom(AuctionObjectResp_1006002 other)
			{
				if (other != null)
				{
					if (other.Id.Length != 0)
					{
						Id = other.Id;
					}
					if (other.GoodsId.Length != 0)
					{
						GoodsId = other.GoodsId;
					}
					if (other.Name.Length != 0)
					{
						Name = other.Name;
					}
					if (other.GoodsType != 0)
					{
						GoodsType = other.GoodsType;
					}
					if (other.FiveElement != 0)
					{
						FiveElement = other.FiveElement;
					}
					if (other.FixedPrice != 0L)
					{
						FixedPrice = other.FixedPrice;
					}
					if (other.RemainHours != 0)
					{
						RemainHours = other.RemainHours;
					}
					if (other.Stage != 0)
					{
						Stage = other.Stage;
					}
					if (other.Rank != 0)
					{
						Rank = other.Rank;
					}
					if (other.Level != 0)
					{
						Level = other.Level;
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
						Id = input.ReadString();
						break;
					case 18u:
						GoodsId = input.ReadString();
						break;
					case 26u:
						Name = input.ReadString();
						break;
					case 32u:
						GoodsType = input.ReadInt32();
						break;
					case 40u:
						FiveElement = input.ReadInt32();
						break;
					case 48u:
						FixedPrice = input.ReadInt64();
						break;
					case 56u:
						RemainHours = input.ReadInt32();
						break;
					case 64u:
						Stage = input.ReadInt32();
						break;
					case 72u:
						Rank = input.ReadInt32();
						break;
					case 80u:
						Level = input.ReadInt32();
						break;
					case 88u:
						Count = input.ReadInt32();
						break;
					}
				}
			}
		}
	}

	private static readonly MessageParser<EnterAuctionResp_1006001> _parser = new MessageParser<EnterAuctionResp_1006001>(() => new EnterAuctionResp_1006001());

	private UnknownFieldSet _unknownFields;

	public const int CodeFieldNumber = 1;

	private int code_;

	public const int IsSuccessFieldNumber = 2;

	private bool isSuccess_;

	public const int PageNumFieldNumber = 3;

	private int pageNum_;

	public const int PageSizeFieldNumber = 4;

	private int pageSize_;

	public const int PagesFieldNumber = 5;

	private int pages_;

	public const int ObjectFieldNumber = 6;

	private static readonly FieldCodec<Types.AuctionObjectResp_1006002> _repeated_object_codec = FieldCodec.ForMessage(50u, Types.AuctionObjectResp_1006002.Parser);

	private readonly RepeatedField<Types.AuctionObjectResp_1006002> object_ = new RepeatedField<Types.AuctionObjectResp_1006002>();

	[DebuggerNonUserCode]
	public static MessageParser<EnterAuctionResp_1006001> Parser => _parser;

	[DebuggerNonUserCode]
	public static MessageDescriptor Descriptor => AuctionProtoReflection.Descriptor.MessageTypes[1];

	[DebuggerNonUserCode]
	MessageDescriptor IMessage.Descriptor => pb_003A_003AGoogle_002EProtobuf_002EIMessage_002Eget_Descriptor();

	[DebuggerNonUserCode]
	public int Code
	{
		get
		{
			return code_;
		}
		set
		{
			code_ = value;
		}
	}

	[DebuggerNonUserCode]
	public bool IsSuccess
	{
		get
		{
			return isSuccess_;
		}
		set
		{
			isSuccess_ = value;
		}
	}

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
	public int Pages
	{
		get
		{
			return pages_;
		}
		set
		{
			pages_ = value;
		}
	}

	[DebuggerNonUserCode]
	public RepeatedField<Types.AuctionObjectResp_1006002> Object => object_;

	private MessageDescriptor pb_003A_003AGoogle_002EProtobuf_002EIMessage_002Eget_Descriptor()
	{
		return Descriptor;
	}

	[DebuggerNonUserCode]
	public EnterAuctionResp_1006001()
	{
	}

	[DebuggerNonUserCode]
	public EnterAuctionResp_1006001(EnterAuctionResp_1006001 other)
		: this()
	{
		code_ = other.code_;
		isSuccess_ = other.isSuccess_;
		pageNum_ = other.pageNum_;
		pageSize_ = other.pageSize_;
		pages_ = other.pages_;
		object_ = other.object_.Clone();
		_unknownFields = UnknownFieldSet.Clone(other._unknownFields);
	}

	[DebuggerNonUserCode]
	public EnterAuctionResp_1006001 Clone()
	{
		return new EnterAuctionResp_1006001(this);
	}

	[DebuggerNonUserCode]
	public override bool Equals(object other)
	{
		return Equals(other as EnterAuctionResp_1006001);
	}

	[DebuggerNonUserCode]
	public bool Equals(EnterAuctionResp_1006001 other)
	{
		if (other == null)
		{
			return false;
		}
		if (other == this)
		{
			return true;
		}
		if (Code != other.Code)
		{
			return false;
		}
		if (IsSuccess != other.IsSuccess)
		{
			return false;
		}
		if (PageNum != other.PageNum)
		{
			return false;
		}
		if (PageSize != other.PageSize)
		{
			return false;
		}
		if (Pages != other.Pages)
		{
			return false;
		}
		if (!object_.Equals(other.object_))
		{
			return false;
		}
		return object.Equals(_unknownFields, other._unknownFields);
	}

	[DebuggerNonUserCode]
	public override int GetHashCode()
	{
		int num = 1;
		if (Code != 0)
		{
			num ^= Code.GetHashCode();
		}
		if (IsSuccess)
		{
			num ^= IsSuccess.GetHashCode();
		}
		if (PageNum != 0)
		{
			num ^= PageNum.GetHashCode();
		}
		if (PageSize != 0)
		{
			num ^= PageSize.GetHashCode();
		}
		if (Pages != 0)
		{
			num ^= Pages.GetHashCode();
		}
		num ^= object_.GetHashCode();
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
		if (Code != 0)
		{
			output.WriteRawTag(8);
			output.WriteInt32(Code);
		}
		if (IsSuccess)
		{
			output.WriteRawTag(16);
			output.WriteBool(IsSuccess);
		}
		if (PageNum != 0)
		{
			output.WriteRawTag(24);
			output.WriteInt32(PageNum);
		}
		if (PageSize != 0)
		{
			output.WriteRawTag(32);
			output.WriteInt32(PageSize);
		}
		if (Pages != 0)
		{
			output.WriteRawTag(40);
			output.WriteInt32(Pages);
		}
		object_.WriteTo(output, _repeated_object_codec);
		if (_unknownFields != null)
		{
			_unknownFields.WriteTo(output);
		}
	}

	[DebuggerNonUserCode]
	public int CalculateSize()
	{
		int num = 0;
		if (Code != 0)
		{
			num += 1 + CodedOutputStream.ComputeInt32Size(Code);
		}
		if (IsSuccess)
		{
			num += 2;
		}
		if (PageNum != 0)
		{
			num += 1 + CodedOutputStream.ComputeInt32Size(PageNum);
		}
		if (PageSize != 0)
		{
			num += 1 + CodedOutputStream.ComputeInt32Size(PageSize);
		}
		if (Pages != 0)
		{
			num += 1 + CodedOutputStream.ComputeInt32Size(Pages);
		}
		num += object_.CalculateSize(_repeated_object_codec);
		if (_unknownFields != null)
		{
			num += _unknownFields.CalculateSize();
		}
		return num;
	}

	[DebuggerNonUserCode]
	public void MergeFrom(EnterAuctionResp_1006001 other)
	{
		if (other != null)
		{
			if (other.Code != 0)
			{
				Code = other.Code;
			}
			if (other.IsSuccess)
			{
				IsSuccess = other.IsSuccess;
			}
			if (other.PageNum != 0)
			{
				PageNum = other.PageNum;
			}
			if (other.PageSize != 0)
			{
				PageSize = other.PageSize;
			}
			if (other.Pages != 0)
			{
				Pages = other.Pages;
			}
			object_.Add(other.object_);
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
				Code = input.ReadInt32();
				break;
			case 16u:
				IsSuccess = input.ReadBool();
				break;
			case 24u:
				PageNum = input.ReadInt32();
				break;
			case 32u:
				PageSize = input.ReadInt32();
				break;
			case 40u:
				Pages = input.ReadInt32();
				break;
			case 50u:
				object_.AddEntriesFrom(input, _repeated_object_codec);
				break;
			}
		}
	}
}
