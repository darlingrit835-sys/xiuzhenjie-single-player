using System;
using System.Diagnostics;
using Google.Protobuf;
using Google.Protobuf.Collections;
using Google.Protobuf.Reflection;

public sealed class EnterXuanTianLouResp_1004033 : IMessage<EnterXuanTianLouResp_1004033>, IMessage, IEquatable<EnterXuanTianLouResp_1004033>, IDeepCloneable<EnterXuanTianLouResp_1004033>
{
	[DebuggerNonUserCode]
	public static class Types
	{
		public sealed class GoodsInfoResp_1004034 : IMessage<GoodsInfoResp_1004034>, IMessage, IEquatable<GoodsInfoResp_1004034>, IDeepCloneable<GoodsInfoResp_1004034>
		{
			private static readonly MessageParser<GoodsInfoResp_1004034> _parser = new MessageParser<GoodsInfoResp_1004034>(() => new GoodsInfoResp_1004034());

			private UnknownFieldSet _unknownFields;

			public const int IdFieldNumber = 1;

			private int id_;

			public const int SystemPriceFieldNumber = 2;

			private int systemPrice_;

			public const int CountFieldNumber = 3;

			private int count_;

			[DebuggerNonUserCode]
			public static MessageParser<GoodsInfoResp_1004034> Parser => _parser;

			[DebuggerNonUserCode]
			public static MessageDescriptor Descriptor => EnterXuanTianLouResp_1004033.Descriptor.NestedTypes[0];

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
			public int SystemPrice
			{
				get
				{
					return systemPrice_;
				}
				set
				{
					systemPrice_ = value;
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
			public GoodsInfoResp_1004034()
			{
			}

			[DebuggerNonUserCode]
			public GoodsInfoResp_1004034(GoodsInfoResp_1004034 other)
				: this()
			{
				id_ = other.id_;
				systemPrice_ = other.systemPrice_;
				count_ = other.count_;
				_unknownFields = UnknownFieldSet.Clone(other._unknownFields);
			}

			[DebuggerNonUserCode]
			public GoodsInfoResp_1004034 Clone()
			{
				return new GoodsInfoResp_1004034(this);
			}

			[DebuggerNonUserCode]
			public override bool Equals(object other)
			{
				return Equals(other as GoodsInfoResp_1004034);
			}

			[DebuggerNonUserCode]
			public bool Equals(GoodsInfoResp_1004034 other)
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
				if (SystemPrice != other.SystemPrice)
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
				if (Id != 0)
				{
					num ^= Id.GetHashCode();
				}
				if (SystemPrice != 0)
				{
					num ^= SystemPrice.GetHashCode();
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
				if (Id != 0)
				{
					output.WriteRawTag(8);
					output.WriteInt32(Id);
				}
				if (SystemPrice != 0)
				{
					output.WriteRawTag(16);
					output.WriteInt32(SystemPrice);
				}
				if (Count != 0)
				{
					output.WriteRawTag(24);
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
				if (Id != 0)
				{
					num += 1 + CodedOutputStream.ComputeInt32Size(Id);
				}
				if (SystemPrice != 0)
				{
					num += 1 + CodedOutputStream.ComputeInt32Size(SystemPrice);
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
			public void MergeFrom(GoodsInfoResp_1004034 other)
			{
				if (other != null)
				{
					if (other.Id != 0)
					{
						Id = other.Id;
					}
					if (other.SystemPrice != 0)
					{
						SystemPrice = other.SystemPrice;
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
					case 8u:
						Id = input.ReadInt32();
						break;
					case 16u:
						SystemPrice = input.ReadInt32();
						break;
					case 24u:
						Count = input.ReadInt32();
						break;
					}
				}
			}
		}
	}

	private static readonly MessageParser<EnterXuanTianLouResp_1004033> _parser = new MessageParser<EnterXuanTianLouResp_1004033>(() => new EnterXuanTianLouResp_1004033());

	private UnknownFieldSet _unknownFields;

	public const int CodeFieldNumber = 1;

	private int code_;

	public const int IsSuccessFieldNumber = 2;

	private bool isSuccess_;

	public const int GoodsFieldNumber = 3;

	private static readonly FieldCodec<Types.GoodsInfoResp_1004034> _repeated_goods_codec = FieldCodec.ForMessage(26u, Types.GoodsInfoResp_1004034.Parser);

	private readonly RepeatedField<Types.GoodsInfoResp_1004034> goods_ = new RepeatedField<Types.GoodsInfoResp_1004034>();

	public const int ReXiaoGoodsIdFieldNumber = 4;

	private static readonly FieldCodec<int> _repeated_reXiaoGoodsId_codec = FieldCodec.ForInt32(34u);

	private readonly RepeatedField<int> reXiaoGoodsId_ = new RepeatedField<int>();

	[DebuggerNonUserCode]
	public static MessageParser<EnterXuanTianLouResp_1004033> Parser => _parser;

	[DebuggerNonUserCode]
	public static MessageDescriptor Descriptor => GoodsProtoReflection.Descriptor.MessageTypes[57];

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
	public RepeatedField<Types.GoodsInfoResp_1004034> Goods => goods_;

	[DebuggerNonUserCode]
	public RepeatedField<int> ReXiaoGoodsId => reXiaoGoodsId_;

	private MessageDescriptor pb_003A_003AGoogle_002EProtobuf_002EIMessage_002Eget_Descriptor()
	{
		return Descriptor;
	}

	[DebuggerNonUserCode]
	public EnterXuanTianLouResp_1004033()
	{
	}

	[DebuggerNonUserCode]
	public EnterXuanTianLouResp_1004033(EnterXuanTianLouResp_1004033 other)
		: this()
	{
		code_ = other.code_;
		isSuccess_ = other.isSuccess_;
		goods_ = other.goods_.Clone();
		reXiaoGoodsId_ = other.reXiaoGoodsId_.Clone();
		_unknownFields = UnknownFieldSet.Clone(other._unknownFields);
	}

	[DebuggerNonUserCode]
	public EnterXuanTianLouResp_1004033 Clone()
	{
		return new EnterXuanTianLouResp_1004033(this);
	}

	[DebuggerNonUserCode]
	public override bool Equals(object other)
	{
		return Equals(other as EnterXuanTianLouResp_1004033);
	}

	[DebuggerNonUserCode]
	public bool Equals(EnterXuanTianLouResp_1004033 other)
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
		if (!goods_.Equals(other.goods_))
		{
			return false;
		}
		if (!reXiaoGoodsId_.Equals(other.reXiaoGoodsId_))
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
		num ^= goods_.GetHashCode();
		num ^= reXiaoGoodsId_.GetHashCode();
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
		goods_.WriteTo(output, _repeated_goods_codec);
		reXiaoGoodsId_.WriteTo(output, _repeated_reXiaoGoodsId_codec);
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
		num += goods_.CalculateSize(_repeated_goods_codec);
		num += reXiaoGoodsId_.CalculateSize(_repeated_reXiaoGoodsId_codec);
		if (_unknownFields != null)
		{
			num += _unknownFields.CalculateSize();
		}
		return num;
	}

	[DebuggerNonUserCode]
	public void MergeFrom(EnterXuanTianLouResp_1004033 other)
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
			goods_.Add(other.goods_);
			reXiaoGoodsId_.Add(other.reXiaoGoodsId_);
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
				goods_.AddEntriesFrom(input, _repeated_goods_codec);
				break;
			case 32u:
			case 34u:
				reXiaoGoodsId_.AddEntriesFrom(input, _repeated_reXiaoGoodsId_codec);
				break;
			}
		}
	}
}
