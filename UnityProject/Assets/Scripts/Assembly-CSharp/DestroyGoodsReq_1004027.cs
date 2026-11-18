using System;
using System.Diagnostics;
using Google.Protobuf;
using Google.Protobuf.Reflection;

public sealed class DestroyGoodsReq_1004027 : IMessage<DestroyGoodsReq_1004027>, IMessage, IEquatable<DestroyGoodsReq_1004027>, IDeepCloneable<DestroyGoodsReq_1004027>
{
	private static readonly MessageParser<DestroyGoodsReq_1004027> _parser = new MessageParser<DestroyGoodsReq_1004027>(() => new DestroyGoodsReq_1004027());

	private UnknownFieldSet _unknownFields;

	public const int GoodsIdFieldNumber = 1;

	private string goodsId_ = "";

	public const int CountFieldNumber = 2;

	private int count_;

	[DebuggerNonUserCode]
	public static MessageParser<DestroyGoodsReq_1004027> Parser => _parser;

	[DebuggerNonUserCode]
	public static MessageDescriptor Descriptor => GoodsProtoReflection.Descriptor.MessageTypes[47];

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
	public DestroyGoodsReq_1004027()
	{
	}

	[DebuggerNonUserCode]
	public DestroyGoodsReq_1004027(DestroyGoodsReq_1004027 other)
		: this()
	{
		goodsId_ = other.goodsId_;
		count_ = other.count_;
		_unknownFields = UnknownFieldSet.Clone(other._unknownFields);
	}

	[DebuggerNonUserCode]
	public DestroyGoodsReq_1004027 Clone()
	{
		return new DestroyGoodsReq_1004027(this);
	}

	[DebuggerNonUserCode]
	public override bool Equals(object other)
	{
		return Equals(other as DestroyGoodsReq_1004027);
	}

	[DebuggerNonUserCode]
	public bool Equals(DestroyGoodsReq_1004027 other)
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
	public void MergeFrom(DestroyGoodsReq_1004027 other)
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
