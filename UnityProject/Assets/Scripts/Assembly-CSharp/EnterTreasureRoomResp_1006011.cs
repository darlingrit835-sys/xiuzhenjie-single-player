using System;
using System.Diagnostics;
using Google.Protobuf;
using Google.Protobuf.Collections;
using Google.Protobuf.Reflection;

public sealed class EnterTreasureRoomResp_1006011 : IMessage<EnterTreasureRoomResp_1006011>, IMessage, IEquatable<EnterTreasureRoomResp_1006011>, IDeepCloneable<EnterTreasureRoomResp_1006011>
{
	[DebuggerNonUserCode]
	public static class Types
	{
		public sealed class GoodsListResp_1006012 : IMessage<GoodsListResp_1006012>, IMessage, IEquatable<GoodsListResp_1006012>, IDeepCloneable<GoodsListResp_1006012>
		{
			private static readonly MessageParser<GoodsListResp_1006012> _parser = new MessageParser<GoodsListResp_1006012>(() => new GoodsListResp_1006012());

			private UnknownFieldSet _unknownFields;

			public const int GoodsNameFieldNumber = 1;

			private string goodsName_ = "";

			public const int FiveElementFieldNumber = 2;

			private int fiveElement_;

			public const int RankFieldNumber = 3;

			private int rank_;

			public const int StartPriceFieldNumber = 4;

			private int startPrice_;

			[DebuggerNonUserCode]
			public static MessageParser<GoodsListResp_1006012> Parser => _parser;

			[DebuggerNonUserCode]
			public static MessageDescriptor Descriptor => EnterTreasureRoomResp_1006011.Descriptor.NestedTypes[0];

			[DebuggerNonUserCode]
			MessageDescriptor IMessage.Descriptor => pb_003A_003AGoogle_002EProtobuf_002EIMessage_002Eget_Descriptor();

			[DebuggerNonUserCode]
			public string GoodsName
			{
				get
				{
					return goodsName_;
				}
				set
				{
					goodsName_ = ProtoPreconditions.CheckNotNull(value, "value");
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
			public int StartPrice
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
			public GoodsListResp_1006012()
			{
			}

			[DebuggerNonUserCode]
			public GoodsListResp_1006012(GoodsListResp_1006012 other)
				: this()
			{
				goodsName_ = other.goodsName_;
				fiveElement_ = other.fiveElement_;
				rank_ = other.rank_;
				startPrice_ = other.startPrice_;
				_unknownFields = UnknownFieldSet.Clone(other._unknownFields);
			}

			[DebuggerNonUserCode]
			public GoodsListResp_1006012 Clone()
			{
				return new GoodsListResp_1006012(this);
			}

			[DebuggerNonUserCode]
			public override bool Equals(object other)
			{
				return Equals(other as GoodsListResp_1006012);
			}

			[DebuggerNonUserCode]
			public bool Equals(GoodsListResp_1006012 other)
			{
				if (other == null)
				{
					return false;
				}
				if (other == this)
				{
					return true;
				}
				if (GoodsName != other.GoodsName)
				{
					return false;
				}
				if (FiveElement != other.FiveElement)
				{
					return false;
				}
				if (Rank != other.Rank)
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
				if (GoodsName.Length != 0)
				{
					num ^= GoodsName.GetHashCode();
				}
				if (FiveElement != 0)
				{
					num ^= FiveElement.GetHashCode();
				}
				if (Rank != 0)
				{
					num ^= Rank.GetHashCode();
				}
				if (StartPrice != 0)
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
				if (GoodsName.Length != 0)
				{
					output.WriteRawTag(10);
					output.WriteString(GoodsName);
				}
				if (FiveElement != 0)
				{
					output.WriteRawTag(16);
					output.WriteInt32(FiveElement);
				}
				if (Rank != 0)
				{
					output.WriteRawTag(24);
					output.WriteInt32(Rank);
				}
				if (StartPrice != 0)
				{
					output.WriteRawTag(32);
					output.WriteInt32(StartPrice);
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
				if (GoodsName.Length != 0)
				{
					num += 1 + CodedOutputStream.ComputeStringSize(GoodsName);
				}
				if (FiveElement != 0)
				{
					num += 1 + CodedOutputStream.ComputeInt32Size(FiveElement);
				}
				if (Rank != 0)
				{
					num += 1 + CodedOutputStream.ComputeInt32Size(Rank);
				}
				if (StartPrice != 0)
				{
					num += 1 + CodedOutputStream.ComputeInt32Size(StartPrice);
				}
				if (_unknownFields != null)
				{
					num += _unknownFields.CalculateSize();
				}
				return num;
			}

			[DebuggerNonUserCode]
			public void MergeFrom(GoodsListResp_1006012 other)
			{
				if (other != null)
				{
					if (other.GoodsName.Length != 0)
					{
						GoodsName = other.GoodsName;
					}
					if (other.FiveElement != 0)
					{
						FiveElement = other.FiveElement;
					}
					if (other.Rank != 0)
					{
						Rank = other.Rank;
					}
					if (other.StartPrice != 0)
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
						GoodsName = input.ReadString();
						break;
					case 16u:
						FiveElement = input.ReadInt32();
						break;
					case 24u:
						Rank = input.ReadInt32();
						break;
					case 32u:
						StartPrice = input.ReadInt32();
						break;
					}
				}
			}
		}
	}

	private static readonly MessageParser<EnterTreasureRoomResp_1006011> _parser = new MessageParser<EnterTreasureRoomResp_1006011>(() => new EnterTreasureRoomResp_1006011());

	private UnknownFieldSet _unknownFields;

	public const int CodeFieldNumber = 1;

	private int code_;

	public const int IsSuccessFieldNumber = 2;

	private bool isSuccess_;

	public const int StageFieldNumber = 3;

	private int stage_;

	public const int ToOnTheShelfTimeFieldNumber = 4;

	private int toOnTheShelfTime_;

	public const int ToEnterAuctionTimeFieldNumber = 5;

	private int toEnterAuctionTime_;

	public const int GoodsListFieldNumber = 6;

	private static readonly FieldCodec<Types.GoodsListResp_1006012> _repeated_goodsList_codec = FieldCodec.ForMessage(50u, Types.GoodsListResp_1006012.Parser);

	private readonly RepeatedField<Types.GoodsListResp_1006012> goodsList_ = new RepeatedField<Types.GoodsListResp_1006012>();

	[DebuggerNonUserCode]
	public static MessageParser<EnterTreasureRoomResp_1006011> Parser => _parser;

	[DebuggerNonUserCode]
	public static MessageDescriptor Descriptor => AuctionProtoReflection.Descriptor.MessageTypes[11];

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
	public int ToOnTheShelfTime
	{
		get
		{
			return toOnTheShelfTime_;
		}
		set
		{
			toOnTheShelfTime_ = value;
		}
	}

	[DebuggerNonUserCode]
	public int ToEnterAuctionTime
	{
		get
		{
			return toEnterAuctionTime_;
		}
		set
		{
			toEnterAuctionTime_ = value;
		}
	}

	[DebuggerNonUserCode]
	public RepeatedField<Types.GoodsListResp_1006012> GoodsList => goodsList_;

	private MessageDescriptor pb_003A_003AGoogle_002EProtobuf_002EIMessage_002Eget_Descriptor()
	{
		return Descriptor;
	}

	[DebuggerNonUserCode]
	public EnterTreasureRoomResp_1006011()
	{
	}

	[DebuggerNonUserCode]
	public EnterTreasureRoomResp_1006011(EnterTreasureRoomResp_1006011 other)
		: this()
	{
		code_ = other.code_;
		isSuccess_ = other.isSuccess_;
		stage_ = other.stage_;
		toOnTheShelfTime_ = other.toOnTheShelfTime_;
		toEnterAuctionTime_ = other.toEnterAuctionTime_;
		goodsList_ = other.goodsList_.Clone();
		_unknownFields = UnknownFieldSet.Clone(other._unknownFields);
	}

	[DebuggerNonUserCode]
	public EnterTreasureRoomResp_1006011 Clone()
	{
		return new EnterTreasureRoomResp_1006011(this);
	}

	[DebuggerNonUserCode]
	public override bool Equals(object other)
	{
		return Equals(other as EnterTreasureRoomResp_1006011);
	}

	[DebuggerNonUserCode]
	public bool Equals(EnterTreasureRoomResp_1006011 other)
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
		if (Stage != other.Stage)
		{
			return false;
		}
		if (ToOnTheShelfTime != other.ToOnTheShelfTime)
		{
			return false;
		}
		if (ToEnterAuctionTime != other.ToEnterAuctionTime)
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
		if (Stage != 0)
		{
			num ^= Stage.GetHashCode();
		}
		if (ToOnTheShelfTime != 0)
		{
			num ^= ToOnTheShelfTime.GetHashCode();
		}
		if (ToEnterAuctionTime != 0)
		{
			num ^= ToEnterAuctionTime.GetHashCode();
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
		if (Stage != 0)
		{
			output.WriteRawTag(24);
			output.WriteInt32(Stage);
		}
		if (ToOnTheShelfTime != 0)
		{
			output.WriteRawTag(32);
			output.WriteInt32(ToOnTheShelfTime);
		}
		if (ToEnterAuctionTime != 0)
		{
			output.WriteRawTag(40);
			output.WriteInt32(ToEnterAuctionTime);
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
		if (Stage != 0)
		{
			num += 1 + CodedOutputStream.ComputeInt32Size(Stage);
		}
		if (ToOnTheShelfTime != 0)
		{
			num += 1 + CodedOutputStream.ComputeInt32Size(ToOnTheShelfTime);
		}
		if (ToEnterAuctionTime != 0)
		{
			num += 1 + CodedOutputStream.ComputeInt32Size(ToEnterAuctionTime);
		}
		num += goodsList_.CalculateSize(_repeated_goodsList_codec);
		if (_unknownFields != null)
		{
			num += _unknownFields.CalculateSize();
		}
		return num;
	}

	[DebuggerNonUserCode]
	public void MergeFrom(EnterTreasureRoomResp_1006011 other)
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
			if (other.Stage != 0)
			{
				Stage = other.Stage;
			}
			if (other.ToOnTheShelfTime != 0)
			{
				ToOnTheShelfTime = other.ToOnTheShelfTime;
			}
			if (other.ToEnterAuctionTime != 0)
			{
				ToEnterAuctionTime = other.ToEnterAuctionTime;
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
			case 24u:
				Stage = input.ReadInt32();
				break;
			case 32u:
				ToOnTheShelfTime = input.ReadInt32();
				break;
			case 40u:
				ToEnterAuctionTime = input.ReadInt32();
				break;
			case 50u:
				goodsList_.AddEntriesFrom(input, _repeated_goodsList_codec);
				break;
			}
		}
	}
}
