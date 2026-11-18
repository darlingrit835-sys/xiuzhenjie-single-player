using System;
using System.Diagnostics;
using Google.Protobuf;
using Google.Protobuf.Collections;
using Google.Protobuf.Reflection;

public sealed class OnShelfListResp_1006014 : IMessage<OnShelfListResp_1006014>, IMessage, IEquatable<OnShelfListResp_1006014>, IDeepCloneable<OnShelfListResp_1006014>
{
	[DebuggerNonUserCode]
	public static class Types
	{
		public sealed class OnShelfGoodsResp_1006015 : IMessage<OnShelfGoodsResp_1006015>, IMessage, IEquatable<OnShelfGoodsResp_1006015>, IDeepCloneable<OnShelfGoodsResp_1006015>
		{
			private static readonly MessageParser<OnShelfGoodsResp_1006015> _parser = new MessageParser<OnShelfGoodsResp_1006015>(() => new OnShelfGoodsResp_1006015());

			private UnknownFieldSet _unknownFields;

			public const int IdFieldNumber = 1;

			private int id_;

			public const int NameFieldNumber = 2;

			private string name_ = "";

			public const int FiveElementFieldNumber = 3;

			private int fiveElement_;

			public const int AuctionTimeFieldNumber = 4;

			private long auctionTime_;

			public const int StageFieldNumber = 5;

			private int stage_;

			public const int RankFieldNumber = 6;

			private int rank_;

			public const int LevelFieldNumber = 7;

			private int level_;

			public const int GoodsTypeFieldNumber = 8;

			private int goodsType_;

			public const int StartPriceFieldNumber = 9;

			private long startPrice_;

			[DebuggerNonUserCode]
			public static MessageParser<OnShelfGoodsResp_1006015> Parser => _parser;

			[DebuggerNonUserCode]
			public static MessageDescriptor Descriptor => OnShelfListResp_1006014.Descriptor.NestedTypes[0];

			[DebuggerNonUserCode]
			MessageDescriptor IMessage.Descriptor => pb_003A_003AGoogle_002EProtobuf_002EIMessage_002Eget_Descriptor();

			[DebuggerNonUserCode]
			public int Id
			{
				get
				{
					return id_;
				}
				set
				{
					id_ = value;
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
			public long AuctionTime
			{
				get
				{
					return auctionTime_;
				}
				set
				{
					auctionTime_ = value;
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
			public OnShelfGoodsResp_1006015()
			{
			}

			[DebuggerNonUserCode]
			public OnShelfGoodsResp_1006015(OnShelfGoodsResp_1006015 other)
				: this()
			{
				id_ = other.id_;
				name_ = other.name_;
				fiveElement_ = other.fiveElement_;
				auctionTime_ = other.auctionTime_;
				stage_ = other.stage_;
				rank_ = other.rank_;
				level_ = other.level_;
				goodsType_ = other.goodsType_;
				startPrice_ = other.startPrice_;
				_unknownFields = UnknownFieldSet.Clone(other._unknownFields);
			}

			[DebuggerNonUserCode]
			public OnShelfGoodsResp_1006015 Clone()
			{
				return new OnShelfGoodsResp_1006015(this);
			}

			[DebuggerNonUserCode]
			public override bool Equals(object other)
			{
				return Equals(other as OnShelfGoodsResp_1006015);
			}

			[DebuggerNonUserCode]
			public bool Equals(OnShelfGoodsResp_1006015 other)
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
				if (Name != other.Name)
				{
					return false;
				}
				if (FiveElement != other.FiveElement)
				{
					return false;
				}
				if (AuctionTime != other.AuctionTime)
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
				if (GoodsType != other.GoodsType)
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
				if (Id != 0)
				{
					num ^= Id.GetHashCode();
				}
				if (Name.Length != 0)
				{
					num ^= Name.GetHashCode();
				}
				if (FiveElement != 0)
				{
					num ^= FiveElement.GetHashCode();
				}
				if (AuctionTime != 0L)
				{
					num ^= AuctionTime.GetHashCode();
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
				if (GoodsType != 0)
				{
					num ^= GoodsType.GetHashCode();
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
				if (Id != 0)
				{
					output.WriteRawTag(8);
					output.WriteInt32(Id);
				}
				if (Name.Length != 0)
				{
					output.WriteRawTag(18);
					output.WriteString(Name);
				}
				if (FiveElement != 0)
				{
					output.WriteRawTag(24);
					output.WriteInt32(FiveElement);
				}
				if (AuctionTime != 0L)
				{
					output.WriteRawTag(32);
					output.WriteInt64(AuctionTime);
				}
				if (Stage != 0)
				{
					output.WriteRawTag(40);
					output.WriteInt32(Stage);
				}
				if (Rank != 0)
				{
					output.WriteRawTag(48);
					output.WriteInt32(Rank);
				}
				if (Level != 0)
				{
					output.WriteRawTag(56);
					output.WriteInt32(Level);
				}
				if (GoodsType != 0)
				{
					output.WriteRawTag(64);
					output.WriteInt32(GoodsType);
				}
				if (StartPrice != 0L)
				{
					output.WriteRawTag(72);
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
				if (Id != 0)
				{
					num += 1 + CodedOutputStream.ComputeInt32Size(Id);
				}
				if (Name.Length != 0)
				{
					num += 1 + CodedOutputStream.ComputeStringSize(Name);
				}
				if (FiveElement != 0)
				{
					num += 1 + CodedOutputStream.ComputeInt32Size(FiveElement);
				}
				if (AuctionTime != 0L)
				{
					num += 1 + CodedOutputStream.ComputeInt64Size(AuctionTime);
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
				if (GoodsType != 0)
				{
					num += 1 + CodedOutputStream.ComputeInt32Size(GoodsType);
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
			public void MergeFrom(OnShelfGoodsResp_1006015 other)
			{
				if (other != null)
				{
					if (other.Id != 0)
					{
						Id = other.Id;
					}
					if (other.Name.Length != 0)
					{
						Name = other.Name;
					}
					if (other.FiveElement != 0)
					{
						FiveElement = other.FiveElement;
					}
					if (other.AuctionTime != 0L)
					{
						AuctionTime = other.AuctionTime;
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
					if (other.GoodsType != 0)
					{
						GoodsType = other.GoodsType;
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
					case 8u:
						Id = input.ReadInt32();
						break;
					case 18u:
						Name = input.ReadString();
						break;
					case 24u:
						FiveElement = input.ReadInt32();
						break;
					case 32u:
						AuctionTime = input.ReadInt64();
						break;
					case 40u:
						Stage = input.ReadInt32();
						break;
					case 48u:
						Rank = input.ReadInt32();
						break;
					case 56u:
						Level = input.ReadInt32();
						break;
					case 64u:
						GoodsType = input.ReadInt32();
						break;
					case 72u:
						StartPrice = input.ReadInt64();
						break;
					}
				}
			}
		}
	}

	private static readonly MessageParser<OnShelfListResp_1006014> _parser = new MessageParser<OnShelfListResp_1006014>(() => new OnShelfListResp_1006014());

	private UnknownFieldSet _unknownFields;

	public const int CodeFieldNumber = 1;

	private int code_;

	public const int IsSuccessFieldNumber = 2;

	private bool isSuccess_;

	public const int GoodsListFieldNumber = 3;

	private static readonly FieldCodec<Types.OnShelfGoodsResp_1006015> _repeated_goodsList_codec = FieldCodec.ForMessage(26u, Types.OnShelfGoodsResp_1006015.Parser);

	private readonly RepeatedField<Types.OnShelfGoodsResp_1006015> goodsList_ = new RepeatedField<Types.OnShelfGoodsResp_1006015>();

	[DebuggerNonUserCode]
	public static MessageParser<OnShelfListResp_1006014> Parser => _parser;

	[DebuggerNonUserCode]
	public static MessageDescriptor Descriptor => AuctionProtoReflection.Descriptor.MessageTypes[14];

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
	public RepeatedField<Types.OnShelfGoodsResp_1006015> GoodsList => goodsList_;

	private MessageDescriptor pb_003A_003AGoogle_002EProtobuf_002EIMessage_002Eget_Descriptor()
	{
		return Descriptor;
	}

	[DebuggerNonUserCode]
	public OnShelfListResp_1006014()
	{
	}

	[DebuggerNonUserCode]
	public OnShelfListResp_1006014(OnShelfListResp_1006014 other)
		: this()
	{
		code_ = other.code_;
		isSuccess_ = other.isSuccess_;
		goodsList_ = other.goodsList_.Clone();
		_unknownFields = UnknownFieldSet.Clone(other._unknownFields);
	}

	[DebuggerNonUserCode]
	public OnShelfListResp_1006014 Clone()
	{
		return new OnShelfListResp_1006014(this);
	}

	[DebuggerNonUserCode]
	public override bool Equals(object other)
	{
		return Equals(other as OnShelfListResp_1006014);
	}

	[DebuggerNonUserCode]
	public bool Equals(OnShelfListResp_1006014 other)
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
		if (!goodsList_.Equals(other.goodsList_))
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
		num ^= goodsList_.GetHashCode();
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
		goodsList_.WriteTo(output, _repeated_goodsList_codec);
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
		num += goodsList_.CalculateSize(_repeated_goodsList_codec);
		if (_unknownFields != null)
		{
			num += _unknownFields.CalculateSize();
		}
		return num;
	}

	[DebuggerNonUserCode]
	public void MergeFrom(OnShelfListResp_1006014 other)
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
			goodsList_.Add(other.goodsList_);
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
			case 26u:
				goodsList_.AddEntriesFrom(input, _repeated_goodsList_codec);
				break;
			}
		}
	}
}
