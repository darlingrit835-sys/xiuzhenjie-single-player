using System;
using System.Diagnostics;
using Google.Protobuf;
using Google.Protobuf.Collections;
using Google.Protobuf.Reflection;

public sealed class SearchXiRangResp_1013027 : IMessage<SearchXiRangResp_1013027>, IMessage, IEquatable<SearchXiRangResp_1013027>, IDeepCloneable<SearchXiRangResp_1013027>
{
	[DebuggerNonUserCode]
	public static class Types
	{
		public sealed class XiRangResp_1013028 : IMessage<XiRangResp_1013028>, IMessage, IEquatable<XiRangResp_1013028>, IDeepCloneable<XiRangResp_1013028>
		{
			private static readonly MessageParser<XiRangResp_1013028> _parser = new MessageParser<XiRangResp_1013028>(() => new XiRangResp_1013028());

			private UnknownFieldSet _unknownFields;

			public const int TypeFieldNumber = 1;

			private int type_;

			public const int NameFieldNumber = 2;

			private string name_ = "";

			public const int RankFieldNumber = 3;

			private int rank_;

			public const int ConversionRateFieldNumber = 4;

			private double conversionRate_;

			public const int PriceFieldNumber = 5;

			private int price_;

			[DebuggerNonUserCode]
			public static MessageParser<XiRangResp_1013028> Parser => _parser;

			[DebuggerNonUserCode]
			public static MessageDescriptor Descriptor => SearchXiRangResp_1013027.Descriptor.NestedTypes[0];

			[DebuggerNonUserCode]
			MessageDescriptor IMessage.Descriptor => pb_003A_003AGoogle_002EProtobuf_002EIMessage_002Eget_Descriptor();

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
			public int Price
			{
				get
				{
					return price_;
				}
				set
				{
					price_ = value;
				}
			}

			private MessageDescriptor pb_003A_003AGoogle_002EProtobuf_002EIMessage_002Eget_Descriptor()
			{
				return Descriptor;
			}

			[DebuggerNonUserCode]
			public XiRangResp_1013028()
			{
			}

			[DebuggerNonUserCode]
			public XiRangResp_1013028(XiRangResp_1013028 other)
				: this()
			{
				type_ = other.type_;
				name_ = other.name_;
				rank_ = other.rank_;
				conversionRate_ = other.conversionRate_;
				price_ = other.price_;
				_unknownFields = UnknownFieldSet.Clone(other._unknownFields);
			}

			[DebuggerNonUserCode]
			public XiRangResp_1013028 Clone()
			{
				return new XiRangResp_1013028(this);
			}

			[DebuggerNonUserCode]
			public override bool Equals(object other)
			{
				return Equals(other as XiRangResp_1013028);
			}

			[DebuggerNonUserCode]
			public bool Equals(XiRangResp_1013028 other)
			{
				if (other == null)
				{
					return false;
				}
				if (other == this)
				{
					return true;
				}
				if (Type != other.Type)
				{
					return false;
				}
				if (Name != other.Name)
				{
					return false;
				}
				if (Rank != other.Rank)
				{
					return false;
				}
				if (!ProtobufEqualityComparers.BitwiseDoubleEqualityComparer.Equals(ConversionRate, other.ConversionRate))
				{
					return false;
				}
				if (Price != other.Price)
				{
					return false;
				}
				return object.Equals(_unknownFields, other._unknownFields);
			}

			[DebuggerNonUserCode]
			public override int GetHashCode()
			{
				int num = 1;
				if (Type != 0)
				{
					num ^= Type.GetHashCode();
				}
				if (Name.Length != 0)
				{
					num ^= Name.GetHashCode();
				}
				if (Rank != 0)
				{
					num ^= Rank.GetHashCode();
				}
				if (ConversionRate != 0.0)
				{
					num ^= ProtobufEqualityComparers.BitwiseDoubleEqualityComparer.GetHashCode(ConversionRate);
				}
				if (Price != 0)
				{
					num ^= Price.GetHashCode();
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
				if (Type != 0)
				{
					output.WriteRawTag(8);
					output.WriteInt32(Type);
				}
				if (Name.Length != 0)
				{
					output.WriteRawTag(18);
					output.WriteString(Name);
				}
				if (Rank != 0)
				{
					output.WriteRawTag(24);
					output.WriteInt32(Rank);
				}
				if (ConversionRate != 0.0)
				{
					output.WriteRawTag(33);
					output.WriteDouble(ConversionRate);
				}
				if (Price != 0)
				{
					output.WriteRawTag(40);
					output.WriteInt32(Price);
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
				if (Type != 0)
				{
					num += 1 + CodedOutputStream.ComputeInt32Size(Type);
				}
				if (Name.Length != 0)
				{
					num += 1 + CodedOutputStream.ComputeStringSize(Name);
				}
				if (Rank != 0)
				{
					num += 1 + CodedOutputStream.ComputeInt32Size(Rank);
				}
				if (ConversionRate != 0.0)
				{
					num += 9;
				}
				if (Price != 0)
				{
					num += 1 + CodedOutputStream.ComputeInt32Size(Price);
				}
				if (_unknownFields != null)
				{
					num += _unknownFields.CalculateSize();
				}
				return num;
			}

			[DebuggerNonUserCode]
			public void MergeFrom(XiRangResp_1013028 other)
			{
				if (other != null)
				{
					if (other.Type != 0)
					{
						Type = other.Type;
					}
					if (other.Name.Length != 0)
					{
						Name = other.Name;
					}
					if (other.Rank != 0)
					{
						Rank = other.Rank;
					}
					if (other.ConversionRate != 0.0)
					{
						ConversionRate = other.ConversionRate;
					}
					if (other.Price != 0)
					{
						Price = other.Price;
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
						Type = input.ReadInt32();
						break;
					case 18u:
						Name = input.ReadString();
						break;
					case 24u:
						Rank = input.ReadInt32();
						break;
					case 33u:
						ConversionRate = input.ReadDouble();
						break;
					case 40u:
						Price = input.ReadInt32();
						break;
					}
				}
			}
		}
	}

	private static readonly MessageParser<SearchXiRangResp_1013027> _parser = new MessageParser<SearchXiRangResp_1013027>(() => new SearchXiRangResp_1013027());

	private UnknownFieldSet _unknownFields;

	public const int CodeFieldNumber = 1;

	private int code_;

	public const int IsSuccessFieldNumber = 2;

	private bool isSuccess_;

	public const int XiRangFieldNumber = 3;

	private static readonly FieldCodec<Types.XiRangResp_1013028> _repeated_xiRang_codec = FieldCodec.ForMessage(26u, Types.XiRangResp_1013028.Parser);

	private readonly RepeatedField<Types.XiRangResp_1013028> xiRang_ = new RepeatedField<Types.XiRangResp_1013028>();

	[DebuggerNonUserCode]
	public static MessageParser<SearchXiRangResp_1013027> Parser => _parser;

	[DebuggerNonUserCode]
	public static MessageDescriptor Descriptor => PlantProtoReflection.Descriptor.MessageTypes[32];

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
	public RepeatedField<Types.XiRangResp_1013028> XiRang => xiRang_;

	private MessageDescriptor pb_003A_003AGoogle_002EProtobuf_002EIMessage_002Eget_Descriptor()
	{
		return Descriptor;
	}

	[DebuggerNonUserCode]
	public SearchXiRangResp_1013027()
	{
	}

	[DebuggerNonUserCode]
	public SearchXiRangResp_1013027(SearchXiRangResp_1013027 other)
		: this()
	{
		code_ = other.code_;
		isSuccess_ = other.isSuccess_;
		xiRang_ = other.xiRang_.Clone();
		_unknownFields = UnknownFieldSet.Clone(other._unknownFields);
	}

	[DebuggerNonUserCode]
	public SearchXiRangResp_1013027 Clone()
	{
		return new SearchXiRangResp_1013027(this);
	}

	[DebuggerNonUserCode]
	public override bool Equals(object other)
	{
		return Equals(other as SearchXiRangResp_1013027);
	}

	[DebuggerNonUserCode]
	public bool Equals(SearchXiRangResp_1013027 other)
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
		if (!xiRang_.Equals(other.xiRang_))
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
		num ^= xiRang_.GetHashCode();
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
		xiRang_.WriteTo(output, _repeated_xiRang_codec);
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
		num += xiRang_.CalculateSize(_repeated_xiRang_codec);
		if (_unknownFields != null)
		{
			num += _unknownFields.CalculateSize();
		}
		return num;
	}

	[DebuggerNonUserCode]
	public void MergeFrom(SearchXiRangResp_1013027 other)
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
			xiRang_.Add(other.xiRang_);
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
				xiRang_.AddEntriesFrom(input, _repeated_xiRang_codec);
				break;
			}
		}
	}
}
