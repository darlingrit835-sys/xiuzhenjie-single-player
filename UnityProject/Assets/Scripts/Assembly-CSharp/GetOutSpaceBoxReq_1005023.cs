using System;
using System.Diagnostics;
using Google.Protobuf;
using Google.Protobuf.Reflection;

public sealed class GetOutSpaceBoxReq_1005023 : IMessage<GetOutSpaceBoxReq_1005023>, IMessage, IEquatable<GetOutSpaceBoxReq_1005023>, IDeepCloneable<GetOutSpaceBoxReq_1005023>
{
	private static readonly MessageParser<GetOutSpaceBoxReq_1005023> _parser = new MessageParser<GetOutSpaceBoxReq_1005023>(() => new GetOutSpaceBoxReq_1005023());

	private UnknownFieldSet _unknownFields;

	public const int LocationFieldNumber = 1;

	private int location_;

	public const int GoodsIdFieldNumber = 2;

	private string goodsId_ = "";

	public const int CountFieldNumber = 3;

	private int count_;

	[DebuggerNonUserCode]
	public static MessageParser<GetOutSpaceBoxReq_1005023> Parser => _parser;

	[DebuggerNonUserCode]
	public static MessageDescriptor Descriptor => HomeProtoReflection.Descriptor.MessageTypes[27];

	[DebuggerNonUserCode]
	MessageDescriptor IMessage.Descriptor => pb_003A_003AGoogle_002EProtobuf_002EIMessage_002Eget_Descriptor();

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
	public GetOutSpaceBoxReq_1005023()
	{
	}

	[DebuggerNonUserCode]
	public GetOutSpaceBoxReq_1005023(GetOutSpaceBoxReq_1005023 other)
		: this()
	{
		location_ = other.location_;
		goodsId_ = other.goodsId_;
		count_ = other.count_;
		_unknownFields = UnknownFieldSet.Clone(other._unknownFields);
	}

	[DebuggerNonUserCode]
	public GetOutSpaceBoxReq_1005023 Clone()
	{
		return new GetOutSpaceBoxReq_1005023(this);
	}

	[DebuggerNonUserCode]
	public override bool Equals(object other)
	{
		return Equals(other as GetOutSpaceBoxReq_1005023);
	}

	[DebuggerNonUserCode]
	public bool Equals(GetOutSpaceBoxReq_1005023 other)
	{
		if (other == null)
		{
			return false;
		}
		if (other == this)
		{
			return true;
		}
		if (Location != other.Location)
		{
			return false;
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
		if (Location != 0)
		{
			num ^= Location.GetHashCode();
		}
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
		if (Location != 0)
		{
			output.WriteRawTag(8);
			output.WriteInt32(Location);
		}
		if (GoodsId.Length != 0)
		{
			output.WriteRawTag(18);
			output.WriteString(GoodsId);
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
		if (Location != 0)
		{
			num += 1 + CodedOutputStream.ComputeInt32Size(Location);
		}
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
	public void MergeFrom(GetOutSpaceBoxReq_1005023 other)
	{
		if (other != null)
		{
			if (other.Location != 0)
			{
				Location = other.Location;
			}
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
			case 8u:
				Location = input.ReadInt32();
				break;
			case 18u:
				GoodsId = input.ReadString();
				break;
			case 24u:
				Count = input.ReadInt32();
				break;
			}
		}
	}
}
