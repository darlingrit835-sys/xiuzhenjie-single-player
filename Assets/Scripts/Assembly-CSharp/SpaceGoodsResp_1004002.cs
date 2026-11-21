using System;
using System.Diagnostics;
using Google.Protobuf;
using Google.Protobuf.Collections;
using Google.Protobuf.Reflection;

public sealed class SpaceGoodsResp_1004002 : IMessage<SpaceGoodsResp_1004002>, IMessage, IEquatable<SpaceGoodsResp_1004002>, IDeepCloneable<SpaceGoodsResp_1004002>
{
	[DebuggerNonUserCode]
	public static class Types
	{
		public sealed class GoodsInfoResp_1004003 : IMessage<GoodsInfoResp_1004003>, IMessage, IEquatable<GoodsInfoResp_1004003>, IDeepCloneable<GoodsInfoResp_1004003>
		{
			private static readonly MessageParser<GoodsInfoResp_1004003> _parser = new MessageParser<GoodsInfoResp_1004003>(() => new GoodsInfoResp_1004003());

			private UnknownFieldSet _unknownFields;

			public const int GoodsIdFieldNumber = 1;

			private string goodsId_ = "";

			public const int NameFieldNumber = 2;

			private string name_ = "";

			public const int FiveElementFieldNumber = 3;

			private int fiveElement_;

			public const int RankFieldNumber = 4;

			private int rank_;

			public const int LevelFieldNumber = 5;

			private int level_;

			public const int TypeFieldNumber = 6;

			private int type_;

			public const int IsUsingFieldNumber = 7;

			private bool isUsing_;

			public const int IsBindingFieldNumber = 8;

			private bool isBinding_;

			public const int RecyclePriceFieldNumber = 9;

			private int recyclePrice_;

			public const int TemplateIdFieldNumber = 10;

			private string templateId_ = "";

			public const int ConversionRateFieldNumber = 11;

			private double conversionRate_;

			public const int VolumeFieldNumber = 12;

			private double volume_;

			public const int IsLockFieldNumber = 13;

			private bool isLock_;

			public const int CountFieldNumber = 14;

			private int count_;

			public const int OfferPendingFieldNumber = 16;

			public bool OfferPending { get; set; }

			public const int BlankIdFieldNumber = 15;

			private string blankId_ = "";

			public string BlankId
			{
				get => blankId_;
				set => blankId_ = ProtoPreconditions.CheckNotNull(value, "value");
			}

			[DebuggerNonUserCode]
			public static MessageParser<GoodsInfoResp_1004003> Parser => _parser;

			[DebuggerNonUserCode]
			public static MessageDescriptor Descriptor => SpaceGoodsResp_1004002.Descriptor.NestedTypes[0];

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
			public bool IsUsing
			{
				get
				{
					return isUsing_;
				}
				set
				{
					isUsing_ = value;
				}
			}

			[DebuggerNonUserCode]
			public bool IsBinding
			{
				get
				{
					return isBinding_;
				}
				set
				{
					isBinding_ = value;
				}
			}

			[DebuggerNonUserCode]
			public int RecyclePrice
			{
				get
				{
					return recyclePrice_;
				}
				set
				{
					recyclePrice_ = value;
				}
			}

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
			public double ConversionRate
			{
				get
				{
					return conversionRate_;
				}
				set
				{
					conversionRate_ = value;
				}
			}

			[DebuggerNonUserCode]
			public double Volume
			{
				get
				{
					return volume_;
				}
				set
				{
					volume_ = value;
				}
			}

			[DebuggerNonUserCode]
			public bool IsLock
			{
				get
				{
					return isLock_;
				}
				set
				{
					isLock_ = value;
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
			public GoodsInfoResp_1004003()
			{
			}

			[DebuggerNonUserCode]
			public GoodsInfoResp_1004003(GoodsInfoResp_1004003 other)
				: this()
			{
				goodsId_ = other.goodsId_;
				name_ = other.name_;
				fiveElement_ = other.fiveElement_;
				rank_ = other.rank_;
				level_ = other.level_;
				type_ = other.type_;
				isUsing_ = other.isUsing_;
				isBinding_ = other.isBinding_;
				recyclePrice_ = other.recyclePrice_;
				templateId_ = other.templateId_;
				conversionRate_ = other.conversionRate_;
				volume_ = other.volume_;
				isLock_ = other.isLock_;
				count_ = other.count_;
				blankId_ = other.blankId_;
				OfferPending = other.OfferPending;
				_unknownFields = UnknownFieldSet.Clone(other._unknownFields);
			}

			[DebuggerNonUserCode]
			public GoodsInfoResp_1004003 Clone()
			{
				return new GoodsInfoResp_1004003(this);
			}

			[DebuggerNonUserCode]
			public override bool Equals(object other)
			{
				return Equals(other as GoodsInfoResp_1004003);
			}

			[DebuggerNonUserCode]
			public bool Equals(GoodsInfoResp_1004003 other)
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
				if (Name != other.Name)
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
				if (Level != other.Level)
				{
					return false;
				}
				if (Type != other.Type)
				{
					return false;
				}
				if (IsUsing != other.IsUsing)
				{
					return false;
				}
				if (IsBinding != other.IsBinding)
				{
					return false;
				}
				if (RecyclePrice != other.RecyclePrice)
				{
					return false;
				}
				if (TemplateId != other.TemplateId)
				{
					return false;
				}
				if (!ProtobufEqualityComparers.BitwiseDoubleEqualityComparer.Equals(ConversionRate, other.ConversionRate))
				{
					return false;
				}
				if (!ProtobufEqualityComparers.BitwiseDoubleEqualityComparer.Equals(Volume, other.Volume))
				{
					return false;
				}
				if (IsLock != other.IsLock)
				{
					return false;
				}
				if (Count != other.Count || BlankId != other.BlankId)
				{
					return false;
				}
				if (OfferPending != other.OfferPending) return false;
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
				if (Name.Length != 0)
				{
					num ^= Name.GetHashCode();
				}
				if (FiveElement != 0)
				{
					num ^= FiveElement.GetHashCode();
				}
				if (Rank != 0)
				{
					num ^= Rank.GetHashCode();
				}
				if (Level != 0)
				{
					num ^= Level.GetHashCode();
				}
				if (Type != 0)
				{
					num ^= Type.GetHashCode();
				}
				if (IsUsing)
				{
					num ^= IsUsing.GetHashCode();
				}
				if (IsBinding)
				{
					num ^= IsBinding.GetHashCode();
				}
				if (RecyclePrice != 0)
				{
					num ^= RecyclePrice.GetHashCode();
				}
				if (TemplateId.Length != 0)
				{
					num ^= TemplateId.GetHashCode();
				}
				if (ConversionRate != 0.0)
				{
					num ^= ProtobufEqualityComparers.BitwiseDoubleEqualityComparer.GetHashCode(ConversionRate);
				}
				if (Volume != 0.0)
				{
					num ^= ProtobufEqualityComparers.BitwiseDoubleEqualityComparer.GetHashCode(Volume);
				}
				if (IsLock)
				{
					num ^= IsLock.GetHashCode();
				}
				if (Count != 0)
				{
					num ^= Count.GetHashCode();
				}
				if (OfferPending) num ^= OfferPending.GetHashCode();
				if (BlankId.Length != 0) num ^= BlankId.GetHashCode();
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
				if (Rank != 0)
				{
					output.WriteRawTag(32);
					output.WriteInt32(Rank);
				}
				if (Level != 0)
				{
					output.WriteRawTag(40);
					output.WriteInt32(Level);
				}
				if (Type != 0)
				{
					output.WriteRawTag(48);
					output.WriteInt32(Type);
				}
				if (IsUsing)
				{
					output.WriteRawTag(56);
					output.WriteBool(IsUsing);
				}
				if (IsBinding)
				{
					output.WriteRawTag(64);
					output.WriteBool(IsBinding);
				}
				if (RecyclePrice != 0)
				{
					output.WriteRawTag(72);
					output.WriteInt32(RecyclePrice);
				}
				if (TemplateId.Length != 0)
				{
					output.WriteRawTag(82);
					output.WriteString(TemplateId);
				}
				if (ConversionRate != 0.0)
				{
					output.WriteRawTag(89);
					output.WriteDouble(ConversionRate);
				}
				if (Volume != 0.0)
				{
					output.WriteRawTag(97);
					output.WriteDouble(Volume);
				}
				if (IsLock)
				{
					output.WriteRawTag(104);
					output.WriteBool(IsLock);
				}
				if (Count != 0)
				{
					output.WriteRawTag(112);
					output.WriteInt32(Count);
				}
				if (BlankId.Length != 0)
				{
					output.WriteRawTag(122);
					output.WriteString(BlankId);
				}
				if (OfferPending)
				{
					output.WriteTag(OfferPendingFieldNumber, WireFormat.WireType.Varint);
					output.WriteBool(true);
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
				if (Name.Length != 0)
				{
					num += 1 + CodedOutputStream.ComputeStringSize(Name);
				}
				if (FiveElement != 0)
				{
					num += 1 + CodedOutputStream.ComputeInt32Size(FiveElement);
				}
				if (Rank != 0)
				{
					num += 1 + CodedOutputStream.ComputeInt32Size(Rank);
				}
				if (Level != 0)
				{
					num += 1 + CodedOutputStream.ComputeInt32Size(Level);
				}
				if (Type != 0)
				{
					num += 1 + CodedOutputStream.ComputeInt32Size(Type);
				}
				if (IsUsing)
				{
					num += 2;
				}
				if (IsBinding)
				{
					num += 2;
				}
				if (RecyclePrice != 0)
				{
					num += 1 + CodedOutputStream.ComputeInt32Size(RecyclePrice);
				}
				if (TemplateId.Length != 0)
				{
					num += 1 + CodedOutputStream.ComputeStringSize(TemplateId);
				}
				if (ConversionRate != 0.0)
				{
					num += 9;
				}
				if (Volume != 0.0)
				{
					num += 9;
				}
				if (IsLock)
				{
					num += 2;
				}
				if (Count != 0)
				{
					num += 1 + CodedOutputStream.ComputeInt32Size(Count);
				}
				if (OfferPending) num += CodedOutputStream.ComputeTagSize(OfferPendingFieldNumber) + 1;
				if (BlankId.Length != 0) num += 1 + CodedOutputStream.ComputeStringSize(BlankId);
				if (_unknownFields != null)
				{
					num += _unknownFields.CalculateSize();
				}
				return num;
			}

			[DebuggerNonUserCode]
			public void MergeFrom(GoodsInfoResp_1004003 other)
			{
				if (other != null)
				{
					if (other.GoodsId.Length != 0)
					{
						GoodsId = other.GoodsId;
					}
					if (other.Name.Length != 0)
					{
						Name = other.Name;
					}
					if (other.FiveElement != 0)
					{
						FiveElement = other.FiveElement;
					}
					if (other.Rank != 0)
					{
						Rank = other.Rank;
					}
					if (other.Level != 0)
					{
						Level = other.Level;
					}
					if (other.Type != 0)
					{
						Type = other.Type;
					}
					if (other.IsUsing)
					{
						IsUsing = other.IsUsing;
					}
					if (other.IsBinding)
					{
						IsBinding = other.IsBinding;
					}
					if (other.RecyclePrice != 0)
					{
						RecyclePrice = other.RecyclePrice;
					}
					if (other.TemplateId.Length != 0)
					{
						TemplateId = other.TemplateId;
					}
					if (other.ConversionRate != 0.0)
					{
						ConversionRate = other.ConversionRate;
					}
					if (other.Volume != 0.0)
					{
						Volume = other.Volume;
					}
					if (other.IsLock)
					{
						IsLock = other.IsLock;
					}
					if (other.Count != 0)
					{
						Count = other.Count;
					}
					if (other.OfferPending) OfferPending = true;
					if (other.BlankId.Length != 0) BlankId = other.BlankId;
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
					case 18u:
						Name = input.ReadString();
						break;
					case 24u:
						FiveElement = input.ReadInt32();
						break;
					case 32u:
						Rank = input.ReadInt32();
						break;
					case 40u:
						Level = input.ReadInt32();
						break;
					case 48u:
						Type = input.ReadInt32();
						break;
					case 56u:
						IsUsing = input.ReadBool();
						break;
					case 64u:
						IsBinding = input.ReadBool();
						break;
					case 72u:
						RecyclePrice = input.ReadInt32();
						break;
					case 82u:
						TemplateId = input.ReadString();
						break;
					case 89u:
						ConversionRate = input.ReadDouble();
						break;
					case 97u:
						Volume = input.ReadDouble();
						break;
					case 104u:
						IsLock = input.ReadBool();
						break;
					case 128u:
						OfferPending = input.ReadBool();
						break;
					case 122u:
						BlankId = input.ReadString();
						break;
					case 112u:
						Count = input.ReadInt32();
						break;
					}
				}
			}
		}
	}

	private static readonly MessageParser<SpaceGoodsResp_1004002> _parser = new MessageParser<SpaceGoodsResp_1004002>(() => new SpaceGoodsResp_1004002());

	private UnknownFieldSet _unknownFields;

	public const int CodeFieldNumber = 1;

	private int code_;

	public const int IsSuccessFieldNumber = 2;

	private bool isSuccess_;

	public const int GoodsInfosFieldNumber = 3;

	private static readonly FieldCodec<Types.GoodsInfoResp_1004003> _repeated_goodsInfos_codec = FieldCodec.ForMessage(26u, Types.GoodsInfoResp_1004003.Parser);

	private readonly RepeatedField<Types.GoodsInfoResp_1004003> goodsInfos_ = new RepeatedField<Types.GoodsInfoResp_1004003>();

	public const int NowCapacityFieldNumber = 4;

	private int nowCapacity_;

	public const int MaxCapacityFieldNumber = 5;

	private int maxCapacity_;

	public const int LocationFieldNumber = 6;

	private int location_;

	[DebuggerNonUserCode]
	public static MessageParser<SpaceGoodsResp_1004002> Parser => _parser;

	[DebuggerNonUserCode]
	public static MessageDescriptor Descriptor => GoodsProtoReflection.Descriptor.MessageTypes[3];

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
	public RepeatedField<Types.GoodsInfoResp_1004003> GoodsInfos => goodsInfos_;

	[DebuggerNonUserCode]
	public int NowCapacity
	{
		get
		{
			return nowCapacity_;
		}
		set
		{
			nowCapacity_ = value;
		}
	}

	[DebuggerNonUserCode]
	public int MaxCapacity
	{
		get
		{
			return maxCapacity_;
		}
		set
		{
			maxCapacity_ = value;
		}
	}

	[DebuggerNonUserCode]
	public int Location
	{
		get
		{
			return location_;
		}
		set
		{
			location_ = value;
		}
	}

	private MessageDescriptor pb_003A_003AGoogle_002EProtobuf_002EIMessage_002Eget_Descriptor()
	{
		return Descriptor;
	}

	[DebuggerNonUserCode]
	public SpaceGoodsResp_1004002()
	{
	}

	[DebuggerNonUserCode]
	public SpaceGoodsResp_1004002(SpaceGoodsResp_1004002 other)
		: this()
	{
		code_ = other.code_;
		isSuccess_ = other.isSuccess_;
		goodsInfos_ = other.goodsInfos_.Clone();
		nowCapacity_ = other.nowCapacity_;
		maxCapacity_ = other.maxCapacity_;
		location_ = other.location_;
		_unknownFields = UnknownFieldSet.Clone(other._unknownFields);
	}

	[DebuggerNonUserCode]
	public SpaceGoodsResp_1004002 Clone()
	{
		return new SpaceGoodsResp_1004002(this);
	}

	[DebuggerNonUserCode]
	public override bool Equals(object other)
	{
		return Equals(other as SpaceGoodsResp_1004002);
	}

	[DebuggerNonUserCode]
	public bool Equals(SpaceGoodsResp_1004002 other)
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
		if (!goodsInfos_.Equals(other.goodsInfos_))
		{
			return false;
		}
		if (NowCapacity != other.NowCapacity)
		{
			return false;
		}
		if (MaxCapacity != other.MaxCapacity)
		{
			return false;
		}
		if (Location != other.Location)
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
		num ^= goodsInfos_.GetHashCode();
		if (NowCapacity != 0)
		{
			num ^= NowCapacity.GetHashCode();
		}
		if (MaxCapacity != 0)
		{
			num ^= MaxCapacity.GetHashCode();
		}
		if (Location != 0)
		{
			num ^= Location.GetHashCode();
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
		goodsInfos_.WriteTo(output, _repeated_goodsInfos_codec);
		if (NowCapacity != 0)
		{
			output.WriteRawTag(32);
			output.WriteInt32(NowCapacity);
		}
		if (MaxCapacity != 0)
		{
			output.WriteRawTag(40);
			output.WriteInt32(MaxCapacity);
		}
		if (Location != 0)
		{
			output.WriteRawTag(48);
			output.WriteInt32(Location);
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
		if (Code != 0)
		{
			num += 1 + CodedOutputStream.ComputeInt32Size(Code);
		}
		if (IsSuccess)
		{
			num += 2;
		}
		num += goodsInfos_.CalculateSize(_repeated_goodsInfos_codec);
		if (NowCapacity != 0)
		{
			num += 1 + CodedOutputStream.ComputeInt32Size(NowCapacity);
		}
		if (MaxCapacity != 0)
		{
			num += 1 + CodedOutputStream.ComputeInt32Size(MaxCapacity);
		}
		if (Location != 0)
		{
			num += 1 + CodedOutputStream.ComputeInt32Size(Location);
		}
		if (_unknownFields != null)
		{
			num += _unknownFields.CalculateSize();
		}
		return num;
	}

	[DebuggerNonUserCode]
	public void MergeFrom(SpaceGoodsResp_1004002 other)
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
			goodsInfos_.Add(other.goodsInfos_);
			if (other.NowCapacity != 0)
			{
				NowCapacity = other.NowCapacity;
			}
			if (other.MaxCapacity != 0)
			{
				MaxCapacity = other.MaxCapacity;
			}
			if (other.Location != 0)
			{
				Location = other.Location;
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
				Code = input.ReadInt32();
				break;
			case 16u:
				IsSuccess = input.ReadBool();
				break;
			case 26u:
				goodsInfos_.AddEntriesFrom(input, _repeated_goodsInfos_codec);
				break;
			case 32u:
				NowCapacity = input.ReadInt32();
				break;
			case 40u:
				MaxCapacity = input.ReadInt32();
				break;
			case 48u:
				Location = input.ReadInt32();
				break;
			}
		}
	}
}
