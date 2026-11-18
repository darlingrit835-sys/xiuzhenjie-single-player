using System;
using System.Diagnostics;
using Google.Protobuf;
using Google.Protobuf.Collections;
using Google.Protobuf.Reflection;

public sealed class GoodsSellReq_1004022 : IMessage<GoodsSellReq_1004022>, IMessage, IEquatable<GoodsSellReq_1004022>, IDeepCloneable<GoodsSellReq_1004022>
{
	[DebuggerNonUserCode]
	public static class Types
	{
		public sealed class GoodsInfoReq_1004010 : IMessage<GoodsInfoReq_1004010>, IMessage, IEquatable<GoodsInfoReq_1004010>, IDeepCloneable<GoodsInfoReq_1004010>
		{
			private static readonly MessageParser<GoodsInfoReq_1004010> _parser = new MessageParser<GoodsInfoReq_1004010>(() => new GoodsInfoReq_1004010());

			private UnknownFieldSet _unknownFields;

			public const int GoodsIdFieldNumber = 1;

			private string goodsId_ = "";

			public const int CountFieldNumber = 2;

			private int count_;

			[DebuggerNonUserCode]
			public static MessageParser<GoodsInfoReq_1004010> Parser => _parser;

			[DebuggerNonUserCode]
			public static MessageDescriptor Descriptor => GoodsSellReq_1004022.Descriptor.NestedTypes[0];

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
			public GoodsInfoReq_1004010()
			{
			}

			[DebuggerNonUserCode]
			public GoodsInfoReq_1004010(GoodsInfoReq_1004010 other)
				: this()
			{
				goodsId_ = other.goodsId_;
				count_ = other.count_;
				_unknownFields = UnknownFieldSet.Clone(other._unknownFields);
			}

			[DebuggerNonUserCode]
			public GoodsInfoReq_1004010 Clone()
			{
				return new GoodsInfoReq_1004010(this);
			}

			[DebuggerNonUserCode]
			public override bool Equals(object other)
			{
				return Equals(other as GoodsInfoReq_1004010);
			}

			[DebuggerNonUserCode]
			public bool Equals(GoodsInfoReq_1004010 other)
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
				if (Count != 0)
				{
					output.WriteRawTag(16);
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
			public void MergeFrom(GoodsInfoReq_1004010 other)
			{
				if (other != null)
				{
					if (other.GoodsId.Length != 0)
					{
						GoodsId = other.GoodsId;
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
						Count = input.ReadInt32();
						break;
					}
				}
			}
		}
	}

	private static readonly MessageParser<GoodsSellReq_1004022> _parser = new MessageParser<GoodsSellReq_1004022>(() => new GoodsSellReq_1004022());

	private UnknownFieldSet _unknownFields;

	public const int GoodsInfosFieldNumber = 1;

	private static readonly FieldCodec<Types.GoodsInfoReq_1004010> _repeated_goodsInfos_codec = FieldCodec.ForMessage(10u, Types.GoodsInfoReq_1004010.Parser);

	private readonly RepeatedField<Types.GoodsInfoReq_1004010> goodsInfos_ = new RepeatedField<Types.GoodsInfoReq_1004010>();

	[DebuggerNonUserCode]
	public static MessageParser<GoodsSellReq_1004022> Parser => _parser;

	[DebuggerNonUserCode]
	public static MessageDescriptor Descriptor => GoodsProtoReflection.Descriptor.MessageTypes[38];

	[DebuggerNonUserCode]
	MessageDescriptor IMessage.Descriptor => pb_003A_003AGoogle_002EProtobuf_002EIMessage_002Eget_Descriptor();

	[DebuggerNonUserCode]
	public RepeatedField<Types.GoodsInfoReq_1004010> GoodsInfos => goodsInfos_;

	private MessageDescriptor pb_003A_003AGoogle_002EProtobuf_002EIMessage_002Eget_Descriptor()
	{
		return Descriptor;
	}

	[DebuggerNonUserCode]
	public GoodsSellReq_1004022()
	{
	}

	[DebuggerNonUserCode]
	public GoodsSellReq_1004022(GoodsSellReq_1004022 other)
		: this()
	{
		goodsInfos_ = other.goodsInfos_.Clone();
		_unknownFields = UnknownFieldSet.Clone(other._unknownFields);
	}

	[DebuggerNonUserCode]
	public GoodsSellReq_1004022 Clone()
	{
		return new GoodsSellReq_1004022(this);
	}

	[DebuggerNonUserCode]
	public override bool Equals(object other)
	{
		return Equals(other as GoodsSellReq_1004022);
	}

	[DebuggerNonUserCode]
	public bool Equals(GoodsSellReq_1004022 other)
	{
		if (other == null)
		{
			return false;
		}
		if (other == this)
		{
			return true;
		}
		if (!goodsInfos_.Equals(other.goodsInfos_))
		{
			return false;
		}
		return object.Equals(_unknownFields, other._unknownFields);
	}

	[DebuggerNonUserCode]
	public override int GetHashCode()
	{
		int num = 1;
		num ^= goodsInfos_.GetHashCode();
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
		goodsInfos_.WriteTo(output, _repeated_goodsInfos_codec);
		if (_unknownFields != null)
		{
			_unknownFields.WriteTo(output);
		}
	}

	[DebuggerNonUserCode]
	public int CalculateSize()
	{
		int num = 0;
		num += goodsInfos_.CalculateSize(_repeated_goodsInfos_codec);
		if (_unknownFields != null)
		{
			num += _unknownFields.CalculateSize();
		}
		return num;
	}

	[DebuggerNonUserCode]
	public void MergeFrom(GoodsSellReq_1004022 other)
	{
		if (other != null)
		{
			goodsInfos_.Add(other.goodsInfos_);
			_unknownFields = UnknownFieldSet.MergeFrom(_unknownFields, other._unknownFields);
		}
	}

	[DebuggerNonUserCode]
	public void MergeFrom(CodedInputStream input)
	{
		uint num;
		while ((num = input.ReadTag()) != 0)
		{
			if (num != 10)
			{
				_unknownFields = UnknownFieldSet.MergeFieldFrom(_unknownFields, input);
			}
			else
			{
				goodsInfos_.AddEntriesFrom(input, _repeated_goodsInfos_codec);
			}
		}
	}
}
