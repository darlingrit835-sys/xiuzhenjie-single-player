using System;
using System.Diagnostics;
using Google.Protobuf;
using Google.Protobuf.Collections;
using Google.Protobuf.Reflection;

public sealed class SearchLMStockResp_1004028 : IMessage<SearchLMStockResp_1004028>, IMessage, IEquatable<SearchLMStockResp_1004028>, IDeepCloneable<SearchLMStockResp_1004028>
{
	[DebuggerNonUserCode]
	public static class Types
	{
		public sealed class MaterialObjReap_1004029 : IMessage<MaterialObjReap_1004029>, IMessage, IEquatable<MaterialObjReap_1004029>, IDeepCloneable<MaterialObjReap_1004029>
		{
			private static readonly MessageParser<MaterialObjReap_1004029> _parser = new MessageParser<MaterialObjReap_1004029>(() => new MaterialObjReap_1004029());

			private UnknownFieldSet _unknownFields;

			public const int TemplateIdFieldNumber = 1;

			private string templateId_ = "";

			public const int PriceFieldNumber = 2;

			private int price_;

			public const int GoodsNameFieldNumber = 3;

			private string goodsName_ = "";

			[DebuggerNonUserCode]
			public static MessageParser<MaterialObjReap_1004029> Parser => _parser;

			[DebuggerNonUserCode]
			public static MessageDescriptor Descriptor => SearchLMStockResp_1004028.Descriptor.NestedTypes[0];

			[DebuggerNonUserCode]
			MessageDescriptor IMessage.Descriptor => pb_003A_003AGoogle_002EProtobuf_002EIMessage_002Eget_Descriptor();

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

			private MessageDescriptor pb_003A_003AGoogle_002EProtobuf_002EIMessage_002Eget_Descriptor()
			{
				return Descriptor;
			}

			[DebuggerNonUserCode]
			public MaterialObjReap_1004029()
			{
			}

			[DebuggerNonUserCode]
			public MaterialObjReap_1004029(MaterialObjReap_1004029 other)
				: this()
			{
				templateId_ = other.templateId_;
				price_ = other.price_;
				goodsName_ = other.goodsName_;
				_unknownFields = UnknownFieldSet.Clone(other._unknownFields);
			}

			[DebuggerNonUserCode]
			public MaterialObjReap_1004029 Clone()
			{
				return new MaterialObjReap_1004029(this);
			}

			[DebuggerNonUserCode]
			public override bool Equals(object other)
			{
				return Equals(other as MaterialObjReap_1004029);
			}

			[DebuggerNonUserCode]
			public bool Equals(MaterialObjReap_1004029 other)
			{
				if (other == null)
				{
					return false;
				}
				if (other == this)
				{
					return true;
				}
				if (TemplateId != other.TemplateId)
				{
					return false;
				}
				if (Price != other.Price)
				{
					return false;
				}
				if (GoodsName != other.GoodsName)
				{
					return false;
				}
				return object.Equals(_unknownFields, other._unknownFields);
			}

			[DebuggerNonUserCode]
			public override int GetHashCode()
			{
				int num = 1;
				if (TemplateId.Length != 0)
				{
					num ^= TemplateId.GetHashCode();
				}
				if (Price != 0)
				{
					num ^= Price.GetHashCode();
				}
				if (GoodsName.Length != 0)
				{
					num ^= GoodsName.GetHashCode();
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
				if (TemplateId.Length != 0)
				{
					output.WriteRawTag(10);
					output.WriteString(TemplateId);
				}
				if (Price != 0)
				{
					output.WriteRawTag(16);
					output.WriteInt32(Price);
				}
				if (GoodsName.Length != 0)
				{
					output.WriteRawTag(26);
					output.WriteString(GoodsName);
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
				if (TemplateId.Length != 0)
				{
					num += 1 + CodedOutputStream.ComputeStringSize(TemplateId);
				}
				if (Price != 0)
				{
					num += 1 + CodedOutputStream.ComputeInt32Size(Price);
				}
				if (GoodsName.Length != 0)
				{
					num += 1 + CodedOutputStream.ComputeStringSize(GoodsName);
				}
				if (_unknownFields != null)
				{
					num += _unknownFields.CalculateSize();
				}
				return num;
			}

			[DebuggerNonUserCode]
			public void MergeFrom(MaterialObjReap_1004029 other)
			{
				if (other != null)
				{
					if (other.TemplateId.Length != 0)
					{
						TemplateId = other.TemplateId;
					}
					if (other.Price != 0)
					{
						Price = other.Price;
					}
					if (other.GoodsName.Length != 0)
					{
						GoodsName = other.GoodsName;
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
						TemplateId = input.ReadString();
						break;
					case 16u:
						Price = input.ReadInt32();
						break;
					case 26u:
						GoodsName = input.ReadString();
						break;
					}
				}
			}
		}
	}

	private static readonly MessageParser<SearchLMStockResp_1004028> _parser = new MessageParser<SearchLMStockResp_1004028>(() => new SearchLMStockResp_1004028());

	private UnknownFieldSet _unknownFields;

	public const int CodeFieldNumber = 1;

	private int code_;

	public const int IsSuccessFieldNumber = 2;

	private bool isSuccess_;

	public const int MaterialListFieldNumber = 3;

	private static readonly FieldCodec<Types.MaterialObjReap_1004029> _repeated_materialList_codec = FieldCodec.ForMessage(26u, Types.MaterialObjReap_1004029.Parser);

	private readonly RepeatedField<Types.MaterialObjReap_1004029> materialList_ = new RepeatedField<Types.MaterialObjReap_1004029>();

	[DebuggerNonUserCode]
	public static MessageParser<SearchLMStockResp_1004028> Parser => _parser;

	[DebuggerNonUserCode]
	public static MessageDescriptor Descriptor => GoodsProtoReflection.Descriptor.MessageTypes[50];

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
	public RepeatedField<Types.MaterialObjReap_1004029> MaterialList => materialList_;

	private MessageDescriptor pb_003A_003AGoogle_002EProtobuf_002EIMessage_002Eget_Descriptor()
	{
		return Descriptor;
	}

	[DebuggerNonUserCode]
	public SearchLMStockResp_1004028()
	{
	}

	[DebuggerNonUserCode]
	public SearchLMStockResp_1004028(SearchLMStockResp_1004028 other)
		: this()
	{
		code_ = other.code_;
		isSuccess_ = other.isSuccess_;
		materialList_ = other.materialList_.Clone();
		_unknownFields = UnknownFieldSet.Clone(other._unknownFields);
	}

	[DebuggerNonUserCode]
	public SearchLMStockResp_1004028 Clone()
	{
		return new SearchLMStockResp_1004028(this);
	}

	[DebuggerNonUserCode]
	public override bool Equals(object other)
	{
		return Equals(other as SearchLMStockResp_1004028);
	}

	[DebuggerNonUserCode]
	public bool Equals(SearchLMStockResp_1004028 other)
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
		if (!materialList_.Equals(other.materialList_))
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
		num ^= materialList_.GetHashCode();
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
		materialList_.WriteTo(output, _repeated_materialList_codec);
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
		num += materialList_.CalculateSize(_repeated_materialList_codec);
		if (_unknownFields != null)
		{
			num += _unknownFields.CalculateSize();
		}
		return num;
	}

	[DebuggerNonUserCode]
	public void MergeFrom(SearchLMStockResp_1004028 other)
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
			materialList_.Add(other.materialList_);
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
				materialList_.AddEntriesFrom(input, _repeated_materialList_codec);
				break;
			}
		}
	}
}
