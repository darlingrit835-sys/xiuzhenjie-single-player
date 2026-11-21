using System;
using System.Diagnostics;
using Google.Protobuf;
using Google.Protobuf.Reflection;

public sealed class TreasureBuyReq_1006020 : IMessage<TreasureBuyReq_1006020>, IMessage, IEquatable<TreasureBuyReq_1006020>, IDeepCloneable<TreasureBuyReq_1006020>
{
	private static readonly MessageParser<TreasureBuyReq_1006020> _parser = new MessageParser<TreasureBuyReq_1006020>(() => new TreasureBuyReq_1006020());

	private UnknownFieldSet _unknownFields;

	public const int PriceFieldNumber = 1;

	private long price_;

	public const int TreasureIdFieldNumber = 2;

	private int treasureId_;

	[DebuggerNonUserCode]
	public static MessageParser<TreasureBuyReq_1006020> Parser => _parser;

	[DebuggerNonUserCode]
	public static MessageDescriptor Descriptor => AuctionProtoReflection.Descriptor.MessageTypes[20];

	[DebuggerNonUserCode]
	MessageDescriptor IMessage.Descriptor => pb_003A_003AGoogle_002EProtobuf_002EIMessage_002Eget_Descriptor();

	[DebuggerNonUserCode]
	public long Price
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
	public int TreasureId
	{
		get
		{
			return treasureId_;
		}
		set
		{
			treasureId_ = value;
		}
	}

	private MessageDescriptor pb_003A_003AGoogle_002EProtobuf_002EIMessage_002Eget_Descriptor()
	{
		return Descriptor;
	}

	[DebuggerNonUserCode]
	public TreasureBuyReq_1006020()
	{
	}

	[DebuggerNonUserCode]
	public TreasureBuyReq_1006020(TreasureBuyReq_1006020 other)
		: this()
	{
		price_ = other.price_;
		treasureId_ = other.treasureId_;
		_unknownFields = UnknownFieldSet.Clone(other._unknownFields);
	}

	[DebuggerNonUserCode]
	public TreasureBuyReq_1006020 Clone()
	{
		return new TreasureBuyReq_1006020(this);
	}

	[DebuggerNonUserCode]
	public override bool Equals(object other)
	{
		return Equals(other as TreasureBuyReq_1006020);
	}

	[DebuggerNonUserCode]
	public bool Equals(TreasureBuyReq_1006020 other)
	{
		if (other == null)
		{
			return false;
		}
		if (other == this)
		{
			return true;
		}
		if (Price != other.Price)
		{
			return false;
		}
		if (TreasureId != other.TreasureId)
		{
			return false;
		}
		return object.Equals(_unknownFields, other._unknownFields);
	}

	[DebuggerNonUserCode]
	public override int GetHashCode()
	{
		int num = 1;
		if (Price != 0L)
		{
			num ^= Price.GetHashCode();
		}
		if (TreasureId != 0)
		{
			num ^= TreasureId.GetHashCode();
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
		if (Price != 0L)
		{
			output.WriteRawTag(8);
			output.WriteInt64(Price);
		}
		if (TreasureId != 0)
		{
			output.WriteRawTag(16);
			output.WriteInt32(TreasureId);
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
		if (Price != 0L)
		{
			num += 1 + CodedOutputStream.ComputeInt64Size(Price);
		}
		if (TreasureId != 0)
		{
			num += 1 + CodedOutputStream.ComputeInt32Size(TreasureId);
		}
		if (_unknownFields != null)
		{
			num += _unknownFields.CalculateSize();
		}
		return num;
	}

	[DebuggerNonUserCode]
	public void MergeFrom(TreasureBuyReq_1006020 other)
	{
		if (other != null)
		{
			if (other.Price != 0L)
			{
				Price = other.Price;
			}
			if (other.TreasureId != 0)
			{
				TreasureId = other.TreasureId;
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
				Price = input.ReadInt64();
				break;
			case 16u:
				TreasureId = input.ReadInt32();
				break;
			}
		}
	}
}
