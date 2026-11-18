using System;
using System.Diagnostics;
using Google.Protobuf;
using Google.Protobuf.Collections;
using Google.Protobuf.Reflection;

public sealed class TreasureGoodsListResp_1006018 : IMessage<TreasureGoodsListResp_1006018>, IMessage, IEquatable<TreasureGoodsListResp_1006018>, IDeepCloneable<TreasureGoodsListResp_1006018>
{
	[DebuggerNonUserCode]
	public static class Types
	{
		public sealed class GoodsListResp_1006019 : IMessage<GoodsListResp_1006019>, IMessage, IEquatable<GoodsListResp_1006019>, IDeepCloneable<GoodsListResp_1006019>
		{
			private static readonly MessageParser<GoodsListResp_1006019> _parser = new MessageParser<GoodsListResp_1006019>(() => new GoodsListResp_1006019());

			private UnknownFieldSet _unknownFields;

			public const int GoodsNameFieldNumber = 1;

			private string goodsName_ = "";

			public const int FiveElementFieldNumber = 2;

			private int fiveElement_;

			public const int RankFieldNumber = 3;

			private int rank_;

			public const int FinalPriceFieldNumber = 4;

			private long finalPrice_;

			public const int StatusFieldNumber = 5;

			private int status_;

			[DebuggerNonUserCode]
			public static MessageParser<GoodsListResp_1006019> Parser => _parser;

			[DebuggerNonUserCode]
			public static MessageDescriptor Descriptor => TreasureGoodsListResp_1006018.Descriptor.NestedTypes[0];

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
			public long FinalPrice
			{
				get
				{
					return finalPrice_;
				}
				set
				{
					finalPrice_ = value;
				}
			}

			[DebuggerNonUserCode]
			public int Status
			{
				get
				{
					return status_;
				}
				set
				{
					status_ = value;
				}
			}

			private MessageDescriptor pb_003A_003AGoogle_002EProtobuf_002EIMessage_002Eget_Descriptor()
			{
				return Descriptor;
			}

			[DebuggerNonUserCode]
			public GoodsListResp_1006019()
			{
			}

			[DebuggerNonUserCode]
			public GoodsListResp_1006019(GoodsListResp_1006019 other)
				: this()
			{
				goodsName_ = other.goodsName_;
				fiveElement_ = other.fiveElement_;
				rank_ = other.rank_;
				finalPrice_ = other.finalPrice_;
				status_ = other.status_;
				_unknownFields = UnknownFieldSet.Clone(other._unknownFields);
			}

			[DebuggerNonUserCode]
			public GoodsListResp_1006019 Clone()
			{
				return new GoodsListResp_1006019(this);
			}

			[DebuggerNonUserCode]
			public override bool Equals(object other)
			{
				return Equals(other as GoodsListResp_1006019);
			}

			[DebuggerNonUserCode]
			public bool Equals(GoodsListResp_1006019 other)
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
				if (FinalPrice != other.FinalPrice)
				{
					return false;
				}
				if (Status != other.Status)
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
				if (FinalPrice != 0L)
				{
					num ^= FinalPrice.GetHashCode();
				}
				if (Status != 0)
				{
					num ^= Status.GetHashCode();
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
				if (FinalPrice != 0L)
				{
					output.WriteRawTag(32);
					output.WriteInt64(FinalPrice);
				}
				if (Status != 0)
				{
					output.WriteRawTag(40);
					output.WriteInt32(Status);
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
				if (FinalPrice != 0L)
				{
					num += 1 + CodedOutputStream.ComputeInt64Size(FinalPrice);
				}
				if (Status != 0)
				{
					num += 1 + CodedOutputStream.ComputeInt32Size(Status);
				}
				if (_unknownFields != null)
				{
					num += _unknownFields.CalculateSize();
				}
				return num;
			}

			[DebuggerNonUserCode]
			public void MergeFrom(GoodsListResp_1006019 other)
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
					if (other.FinalPrice != 0L)
					{
						FinalPrice = other.FinalPrice;
					}
					if (other.Status != 0)
					{
						Status = other.Status;
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
						FinalPrice = input.ReadInt64();
						break;
					case 40u:
						Status = input.ReadInt32();
						break;
					}
				}
			}
		}
	}

	private static readonly MessageParser<TreasureGoodsListResp_1006018> _parser = new MessageParser<TreasureGoodsListResp_1006018>(() => new TreasureGoodsListResp_1006018());

	private UnknownFieldSet _unknownFields;

	public const int CodeFieldNumber = 1;

	private int code_;

	public const int IsSuccessFieldNumber = 2;

	private bool isSuccess_;

	public const int StageFieldNumber = 3;

	private int stage_;

	public const int GoodsListFieldNumber = 4;

	private static readonly FieldCodec<Types.GoodsListResp_1006019> _repeated_goodsList_codec = FieldCodec.ForMessage(34u, Types.GoodsListResp_1006019.Parser);

	private readonly RepeatedField<Types.GoodsListResp_1006019> goodsList_ = new RepeatedField<Types.GoodsListResp_1006019>();

	[DebuggerNonUserCode]
	public static MessageParser<TreasureGoodsListResp_1006018> Parser => _parser;

	[DebuggerNonUserCode]
	public static MessageDescriptor Descriptor => AuctionProtoReflection.Descriptor.MessageTypes[19];

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
	public RepeatedField<Types.GoodsListResp_1006019> GoodsList => goodsList_;

	private MessageDescriptor pb_003A_003AGoogle_002EProtobuf_002EIMessage_002Eget_Descriptor()
	{
		return Descriptor;
	}

	[DebuggerNonUserCode]
	public TreasureGoodsListResp_1006018()
	{
	}

	[DebuggerNonUserCode]
	public TreasureGoodsListResp_1006018(TreasureGoodsListResp_1006018 other)
		: this()
	{
		code_ = other.code_;
		isSuccess_ = other.isSuccess_;
		stage_ = other.stage_;
		goodsList_ = other.goodsList_.Clone();
		_unknownFields = UnknownFieldSet.Clone(other._unknownFields);
	}

	[DebuggerNonUserCode]
	public TreasureGoodsListResp_1006018 Clone()
	{
		return new TreasureGoodsListResp_1006018(this);
	}

	[DebuggerNonUserCode]
	public override bool Equals(object other)
	{
		return Equals(other as TreasureGoodsListResp_1006018);
	}

	[DebuggerNonUserCode]
	public bool Equals(TreasureGoodsListResp_1006018 other)
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
		num += goodsList_.CalculateSize(_repeated_goodsList_codec);
		if (_unknownFields != null)
		{
			num += _unknownFields.CalculateSize();
		}
		return num;
	}

	[DebuggerNonUserCode]
	public void MergeFrom(TreasureGoodsListResp_1006018 other)
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
			case 34u:
				goodsList_.AddEntriesFrom(input, _repeated_goodsList_codec);
				break;
			}
		}
	}
}
