using System;
using System.Diagnostics;
using Google.Protobuf;
using Google.Protobuf.Reflection;

public sealed class GoodsCompoundReq_1004041 : IMessage<GoodsCompoundReq_1004041>, IMessage, IEquatable<GoodsCompoundReq_1004041>, IDeepCloneable<GoodsCompoundReq_1004041>
{
	private static readonly MessageParser<GoodsCompoundReq_1004041> _parser = new MessageParser<GoodsCompoundReq_1004041>(() => new GoodsCompoundReq_1004041());

	private UnknownFieldSet _unknownFields;

	public const int GoodsIdFieldNumber = 1;

	private string goodsId_ = "";

	public const int NumFieldNumber = 2;

	private int num_;

	[DebuggerNonUserCode]
	public static MessageParser<GoodsCompoundReq_1004041> Parser => _parser;

	[DebuggerNonUserCode]
	public static MessageDescriptor Descriptor => GoodsProtoReflection.Descriptor.MessageTypes[70];

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
	public int Num
	{
		get
		{
			return num_;
		}
		set
		{
			num_ = value;
		}
	}

	private MessageDescriptor pb_003A_003AGoogle_002EProtobuf_002EIMessage_002Eget_Descriptor()
	{
		return Descriptor;
	}

	[DebuggerNonUserCode]
	public GoodsCompoundReq_1004041()
	{
	}

	[DebuggerNonUserCode]
	public GoodsCompoundReq_1004041(GoodsCompoundReq_1004041 other)
		: this()
	{
		goodsId_ = other.goodsId_;
		num_ = other.num_;
		_unknownFields = UnknownFieldSet.Clone(other._unknownFields);
	}

	[DebuggerNonUserCode]
	public GoodsCompoundReq_1004041 Clone()
	{
		return new GoodsCompoundReq_1004041(this);
	}

	[DebuggerNonUserCode]
	public override bool Equals(object other)
	{
		return Equals(other as GoodsCompoundReq_1004041);
	}

	[DebuggerNonUserCode]
	public bool Equals(GoodsCompoundReq_1004041 other)
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
		if (Num != other.Num)
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
		if (Num != 0)
		{
			num ^= Num.GetHashCode();
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
		if (Num != 0)
		{
			output.WriteRawTag(16);
			output.WriteInt32(Num);
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
		if (Num != 0)
		{
			num += 1 + CodedOutputStream.ComputeInt32Size(Num);
		}
		if (_unknownFields != null)
		{
			num += _unknownFields.CalculateSize();
		}
		return num;
	}

	[DebuggerNonUserCode]
	public void MergeFrom(GoodsCompoundReq_1004041 other)
	{
		if (other != null)
		{
			if (other.GoodsId.Length != 0)
			{
				GoodsId = other.GoodsId;
			}
			if (other.Num != 0)
			{
				Num = other.Num;
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
				Num = input.ReadInt32();
				break;
			}
		}
	}
}
