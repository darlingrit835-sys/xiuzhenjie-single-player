using System;
using System.Diagnostics;
using Google.Protobuf;
using Google.Protobuf.Reflection;

public sealed class GoodsBuyReq_1004001 : IMessage<GoodsBuyReq_1004001>, IMessage, IEquatable<GoodsBuyReq_1004001>, IDeepCloneable<GoodsBuyReq_1004001>
{
	private static readonly MessageParser<GoodsBuyReq_1004001> _parser = new MessageParser<GoodsBuyReq_1004001>(() => new GoodsBuyReq_1004001());

	private UnknownFieldSet _unknownFields;

	public const int ModalGoodsIdFieldNumber = 1;

	private string modalGoodsId_ = "";

	public const int GoodsTypeFieldNumber = 2;

	private int goodsType_;

	public const int MapIdFieldNumber = 3;

	private string mapId_ = "";

	[DebuggerNonUserCode]
	public static MessageParser<GoodsBuyReq_1004001> Parser => _parser;

	[DebuggerNonUserCode]
	public static MessageDescriptor Descriptor => GoodsProtoReflection.Descriptor.MessageTypes[0];

	[DebuggerNonUserCode]
	MessageDescriptor IMessage.Descriptor => pb_003A_003AGoogle_002EProtobuf_002EIMessage_002Eget_Descriptor();

	[DebuggerNonUserCode]
	public string ModalGoodsId
	{
		get
		{
			return modalGoodsId_;
		}
		set
		{
			modalGoodsId_ = ProtoPreconditions.CheckNotNull(value, "value");
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
	public string MapId
	{
		get
		{
			return mapId_;
		}
		set
		{
			mapId_ = ProtoPreconditions.CheckNotNull(value, "value");
		}
	}

	private MessageDescriptor pb_003A_003AGoogle_002EProtobuf_002EIMessage_002Eget_Descriptor()
	{
		return Descriptor;
	}

	[DebuggerNonUserCode]
	public GoodsBuyReq_1004001()
	{
	}

	[DebuggerNonUserCode]
	public GoodsBuyReq_1004001(GoodsBuyReq_1004001 other)
		: this()
	{
		modalGoodsId_ = other.modalGoodsId_;
		goodsType_ = other.goodsType_;
		mapId_ = other.mapId_;
		_unknownFields = UnknownFieldSet.Clone(other._unknownFields);
	}

	[DebuggerNonUserCode]
	public GoodsBuyReq_1004001 Clone()
	{
		return new GoodsBuyReq_1004001(this);
	}

	[DebuggerNonUserCode]
	public override bool Equals(object other)
	{
		return Equals(other as GoodsBuyReq_1004001);
	}

	[DebuggerNonUserCode]
	public bool Equals(GoodsBuyReq_1004001 other)
	{
		if (other == null)
		{
			return false;
		}
		if (other == this)
		{
			return true;
		}
		if (ModalGoodsId != other.ModalGoodsId)
		{
			return false;
		}
		if (GoodsType != other.GoodsType)
		{
			return false;
		}
		if (MapId != other.MapId)
		{
			return false;
		}
		return object.Equals(_unknownFields, other._unknownFields);
	}

	[DebuggerNonUserCode]
	public override int GetHashCode()
	{
		int num = 1;
		if (ModalGoodsId.Length != 0)
		{
			num ^= ModalGoodsId.GetHashCode();
		}
		if (GoodsType != 0)
		{
			num ^= GoodsType.GetHashCode();
		}
		if (MapId.Length != 0)
		{
			num ^= MapId.GetHashCode();
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
		if (ModalGoodsId.Length != 0)
		{
			output.WriteRawTag(10);
			output.WriteString(ModalGoodsId);
		}
		if (GoodsType != 0)
		{
			output.WriteRawTag(16);
			output.WriteInt32(GoodsType);
		}
		if (MapId.Length != 0)
		{
			output.WriteRawTag(26);
			output.WriteString(MapId);
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
		if (ModalGoodsId.Length != 0)
		{
			num += 1 + CodedOutputStream.ComputeStringSize(ModalGoodsId);
		}
		if (GoodsType != 0)
		{
			num += 1 + CodedOutputStream.ComputeInt32Size(GoodsType);
		}
		if (MapId.Length != 0)
		{
			num += 1 + CodedOutputStream.ComputeStringSize(MapId);
		}
		if (_unknownFields != null)
		{
			num += _unknownFields.CalculateSize();
		}
		return num;
	}

	[DebuggerNonUserCode]
	public void MergeFrom(GoodsBuyReq_1004001 other)
	{
		if (other != null)
		{
			if (other.ModalGoodsId.Length != 0)
			{
				ModalGoodsId = other.ModalGoodsId;
			}
			if (other.GoodsType != 0)
			{
				GoodsType = other.GoodsType;
			}
			if (other.MapId.Length != 0)
			{
				MapId = other.MapId;
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
				ModalGoodsId = input.ReadString();
				break;
			case 16u:
				GoodsType = input.ReadInt32();
				break;
			case 26u:
				MapId = input.ReadString();
				break;
			}
		}
	}
}
