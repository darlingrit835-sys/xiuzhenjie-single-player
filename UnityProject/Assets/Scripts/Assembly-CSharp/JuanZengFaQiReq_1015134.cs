using System;
using System.Diagnostics;
using Google.Protobuf;
using Google.Protobuf.Collections;
using Google.Protobuf.Reflection;

public sealed class JuanZengFaQiReq_1015134 : IMessage<JuanZengFaQiReq_1015134>, IMessage, IEquatable<JuanZengFaQiReq_1015134>, IDeepCloneable<JuanZengFaQiReq_1015134>
{
	[DebuggerNonUserCode]
	public static class Types
	{
		public sealed class GoodsReq_1015151 : IMessage<GoodsReq_1015151>, IMessage, IEquatable<GoodsReq_1015151>, IDeepCloneable<GoodsReq_1015151>
		{
			private static readonly MessageParser<GoodsReq_1015151> _parser = new MessageParser<GoodsReq_1015151>(() => new GoodsReq_1015151());

			private UnknownFieldSet _unknownFields;

			public const int GoodsIdFieldNumber = 1;

			private string goodsId_ = "";

			public const int CountFieldNumber = 2;

			private int count_;

			[DebuggerNonUserCode]
			public static MessageParser<GoodsReq_1015151> Parser => _parser;

			[DebuggerNonUserCode]
			public static MessageDescriptor Descriptor => JuanZengFaQiReq_1015134.Descriptor.NestedTypes[0];

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
			public GoodsReq_1015151()
			{
			}

			[DebuggerNonUserCode]
			public GoodsReq_1015151(GoodsReq_1015151 other)
				: this()
			{
				goodsId_ = other.goodsId_;
				count_ = other.count_;
				_unknownFields = UnknownFieldSet.Clone(other._unknownFields);
			}

			[DebuggerNonUserCode]
			public GoodsReq_1015151 Clone()
			{
				return new GoodsReq_1015151(this);
			}

			[DebuggerNonUserCode]
			public override bool Equals(object other)
			{
				return Equals(other as GoodsReq_1015151);
			}

			[DebuggerNonUserCode]
			public bool Equals(GoodsReq_1015151 other)
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
			public void MergeFrom(GoodsReq_1015151 other)
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

	private static readonly MessageParser<JuanZengFaQiReq_1015134> _parser = new MessageParser<JuanZengFaQiReq_1015134>(() => new JuanZengFaQiReq_1015134());

	private UnknownFieldSet _unknownFields;

	public const int GoodsFieldNumber = 1;

	private static readonly FieldCodec<Types.GoodsReq_1015151> _repeated_goods_codec = FieldCodec.ForMessage(10u, Types.GoodsReq_1015151.Parser);

	private readonly RepeatedField<Types.GoodsReq_1015151> goods_ = new RepeatedField<Types.GoodsReq_1015151>();

	[DebuggerNonUserCode]
	public static MessageParser<JuanZengFaQiReq_1015134> Parser => _parser;

	[DebuggerNonUserCode]
	public static MessageDescriptor Descriptor => GuildProtoReflection.Descriptor.MessageTypes[168];

	[DebuggerNonUserCode]
	MessageDescriptor IMessage.Descriptor => pb_003A_003AGoogle_002EProtobuf_002EIMessage_002Eget_Descriptor();

	[DebuggerNonUserCode]
	public RepeatedField<Types.GoodsReq_1015151> Goods => goods_;

	private MessageDescriptor pb_003A_003AGoogle_002EProtobuf_002EIMessage_002Eget_Descriptor()
	{
		return Descriptor;
	}

	[DebuggerNonUserCode]
	public JuanZengFaQiReq_1015134()
	{
	}

	[DebuggerNonUserCode]
	public JuanZengFaQiReq_1015134(JuanZengFaQiReq_1015134 other)
		: this()
	{
		goods_ = other.goods_.Clone();
		_unknownFields = UnknownFieldSet.Clone(other._unknownFields);
	}

	[DebuggerNonUserCode]
	public JuanZengFaQiReq_1015134 Clone()
	{
		return new JuanZengFaQiReq_1015134(this);
	}

	[DebuggerNonUserCode]
	public override bool Equals(object other)
	{
		return Equals(other as JuanZengFaQiReq_1015134);
	}

	[DebuggerNonUserCode]
	public bool Equals(JuanZengFaQiReq_1015134 other)
	{
		if (other == null)
		{
			return false;
		}
		if (other == this)
		{
			return true;
		}
		if (!goods_.Equals(other.goods_))
		{
			return false;
		}
		return object.Equals(_unknownFields, other._unknownFields);
	}

	[DebuggerNonUserCode]
	public override int GetHashCode()
	{
		int num = 1;
		num ^= goods_.GetHashCode();
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
		goods_.WriteTo(output, _repeated_goods_codec);
		if (_unknownFields != null)
		{
			_unknownFields.WriteTo(output);
		}
	}

	[DebuggerNonUserCode]
	public int CalculateSize()
	{
		int num = 0;
		num += goods_.CalculateSize(_repeated_goods_codec);
		if (_unknownFields != null)
		{
			num += _unknownFields.CalculateSize();
		}
		return num;
	}

	[DebuggerNonUserCode]
	public void MergeFrom(JuanZengFaQiReq_1015134 other)
	{
		if (other != null)
		{
			goods_.Add(other.goods_);
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
				goods_.AddEntriesFrom(input, _repeated_goods_codec);
			}
		}
	}
}
