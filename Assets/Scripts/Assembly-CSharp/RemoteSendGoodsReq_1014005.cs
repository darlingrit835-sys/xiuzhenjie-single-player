using System;
using System.Diagnostics;
using Google.Protobuf;
using Google.Protobuf.Collections;
using Google.Protobuf.Reflection;

public sealed class RemoteSendGoodsReq_1014005 : IMessage<RemoteSendGoodsReq_1014005>, IMessage, IEquatable<RemoteSendGoodsReq_1014005>, IDeepCloneable<RemoteSendGoodsReq_1014005>
{
	[DebuggerNonUserCode]
	public static class Types
	{
		public sealed class GoodsReq_1014011 : IMessage<GoodsReq_1014011>, IMessage, IEquatable<GoodsReq_1014011>, IDeepCloneable<GoodsReq_1014011>
		{
			private static readonly MessageParser<GoodsReq_1014011> _parser = new MessageParser<GoodsReq_1014011>(() => new GoodsReq_1014011());

			private UnknownFieldSet _unknownFields;

			public const int GoodsIdFieldNumber = 1;

			private string goodsId_ = "";

			public const int CountFieldNumber = 2;

			private int count_;

			[DebuggerNonUserCode]
			public static MessageParser<GoodsReq_1014011> Parser => _parser;

			[DebuggerNonUserCode]
			public static MessageDescriptor Descriptor => RemoteSendGoodsReq_1014005.Descriptor.NestedTypes[0];

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
			public GoodsReq_1014011()
			{
			}

			[DebuggerNonUserCode]
			public GoodsReq_1014011(GoodsReq_1014011 other)
				: this()
			{
				goodsId_ = other.goodsId_;
				count_ = other.count_;
				_unknownFields = UnknownFieldSet.Clone(other._unknownFields);
			}

			[DebuggerNonUserCode]
			public GoodsReq_1014011 Clone()
			{
				return new GoodsReq_1014011(this);
			}

			[DebuggerNonUserCode]
			public override bool Equals(object other)
			{
				return Equals(other as GoodsReq_1014011);
			}

			[DebuggerNonUserCode]
			public bool Equals(GoodsReq_1014011 other)
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
			public void MergeFrom(GoodsReq_1014011 other)
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

	private static readonly MessageParser<RemoteSendGoodsReq_1014005> _parser = new MessageParser<RemoteSendGoodsReq_1014005>(() => new RemoteSendGoodsReq_1014005());

	private UnknownFieldSet _unknownFields;

	public const int TargetFriendIdFieldNumber = 1;

	private string targetFriendId_ = "";

	public const int GoodsFieldNumber = 2;

	private static readonly FieldCodec<Types.GoodsReq_1014011> _repeated_goods_codec = FieldCodec.ForMessage(18u, Types.GoodsReq_1014011.Parser);

	private readonly RepeatedField<Types.GoodsReq_1014011> goods_ = new RepeatedField<Types.GoodsReq_1014011>();

	[DebuggerNonUserCode]
	public static MessageParser<RemoteSendGoodsReq_1014005> Parser => _parser;

	[DebuggerNonUserCode]
	public static MessageDescriptor Descriptor => FriendProtoReflection.Descriptor.MessageTypes[2];

	[DebuggerNonUserCode]
	MessageDescriptor IMessage.Descriptor => pb_003A_003AGoogle_002EProtobuf_002EIMessage_002Eget_Descriptor();

	[DebuggerNonUserCode]
	public string TargetFriendId
	{
		get
		{
			return targetFriendId_;
		}
		set
		{
			targetFriendId_ = ProtoPreconditions.CheckNotNull(value, "value");
		}
	}

	[DebuggerNonUserCode]
	public RepeatedField<Types.GoodsReq_1014011> Goods => goods_;

	private MessageDescriptor pb_003A_003AGoogle_002EProtobuf_002EIMessage_002Eget_Descriptor()
	{
		return Descriptor;
	}

	[DebuggerNonUserCode]
	public RemoteSendGoodsReq_1014005()
	{
	}

	[DebuggerNonUserCode]
	public RemoteSendGoodsReq_1014005(RemoteSendGoodsReq_1014005 other)
		: this()
	{
		targetFriendId_ = other.targetFriendId_;
		goods_ = other.goods_.Clone();
		_unknownFields = UnknownFieldSet.Clone(other._unknownFields);
	}

	[DebuggerNonUserCode]
	public RemoteSendGoodsReq_1014005 Clone()
	{
		return new RemoteSendGoodsReq_1014005(this);
	}

	[DebuggerNonUserCode]
	public override bool Equals(object other)
	{
		return Equals(other as RemoteSendGoodsReq_1014005);
	}

	[DebuggerNonUserCode]
	public bool Equals(RemoteSendGoodsReq_1014005 other)
	{
		if (other == null)
		{
			return false;
		}
		if (other == this)
		{
			return true;
		}
		if (TargetFriendId != other.TargetFriendId)
		{
			return false;
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
		if (TargetFriendId.Length != 0)
		{
			num ^= TargetFriendId.GetHashCode();
		}
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
		if (TargetFriendId.Length != 0)
		{
			output.WriteRawTag(10);
			output.WriteString(TargetFriendId);
		}
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
		if (TargetFriendId.Length != 0)
		{
			num += 1 + CodedOutputStream.ComputeStringSize(TargetFriendId);
		}
		num += goods_.CalculateSize(_repeated_goods_codec);
		if (_unknownFields != null)
		{
			num += _unknownFields.CalculateSize();
		}
		return num;
	}

	[DebuggerNonUserCode]
	public void MergeFrom(RemoteSendGoodsReq_1014005 other)
	{
		if (other != null)
		{
			if (other.TargetFriendId.Length != 0)
			{
				TargetFriendId = other.TargetFriendId;
			}
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
			switch (num)
			{
			default:
				_unknownFields = UnknownFieldSet.MergeFieldFrom(_unknownFields, input);
				break;
			case 10u:
				TargetFriendId = input.ReadString();
				break;
			case 18u:
				goods_.AddEntriesFrom(input, _repeated_goods_codec);
				break;
			}
		}
	}
}
