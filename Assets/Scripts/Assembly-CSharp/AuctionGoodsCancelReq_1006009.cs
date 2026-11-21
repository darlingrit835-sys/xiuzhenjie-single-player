using System;
using System.Diagnostics;
using Google.Protobuf;
using Google.Protobuf.Reflection;

public sealed class AuctionGoodsCancelReq_1006009 : IMessage<AuctionGoodsCancelReq_1006009>, IMessage, IEquatable<AuctionGoodsCancelReq_1006009>, IDeepCloneable<AuctionGoodsCancelReq_1006009>
{
	private static readonly MessageParser<AuctionGoodsCancelReq_1006009> _parser = new MessageParser<AuctionGoodsCancelReq_1006009>(() => new AuctionGoodsCancelReq_1006009());

	private UnknownFieldSet _unknownFields;

	public const int AuctionIdFieldNumber = 1;

	private string auctionId_ = "";

	[DebuggerNonUserCode]
	public static MessageParser<AuctionGoodsCancelReq_1006009> Parser => _parser;

	[DebuggerNonUserCode]
	public static MessageDescriptor Descriptor => AuctionProtoReflection.Descriptor.MessageTypes[6];

	[DebuggerNonUserCode]
	MessageDescriptor IMessage.Descriptor => pb_003A_003AGoogle_002EProtobuf_002EIMessage_002Eget_Descriptor();

	[DebuggerNonUserCode]
	public string AuctionId
	{
		get
		{
			return auctionId_;
		}
		set
		{
			auctionId_ = ProtoPreconditions.CheckNotNull(value, "value");
		}
	}

	private MessageDescriptor pb_003A_003AGoogle_002EProtobuf_002EIMessage_002Eget_Descriptor()
	{
		return Descriptor;
	}

	[DebuggerNonUserCode]
	public AuctionGoodsCancelReq_1006009()
	{
	}

	[DebuggerNonUserCode]
	public AuctionGoodsCancelReq_1006009(AuctionGoodsCancelReq_1006009 other)
		: this()
	{
		auctionId_ = other.auctionId_;
		_unknownFields = UnknownFieldSet.Clone(other._unknownFields);
	}

	[DebuggerNonUserCode]
	public AuctionGoodsCancelReq_1006009 Clone()
	{
		return new AuctionGoodsCancelReq_1006009(this);
	}

	[DebuggerNonUserCode]
	public override bool Equals(object other)
	{
		return Equals(other as AuctionGoodsCancelReq_1006009);
	}

	[DebuggerNonUserCode]
	public bool Equals(AuctionGoodsCancelReq_1006009 other)
	{
		if (other == null)
		{
			return false;
		}
		if (other == this)
		{
			return true;
		}
		if (AuctionId != other.AuctionId)
		{
			return false;
		}
		return object.Equals(_unknownFields, other._unknownFields);
	}

	[DebuggerNonUserCode]
	public override int GetHashCode()
	{
		int num = 1;
		if (AuctionId.Length != 0)
		{
			num ^= AuctionId.GetHashCode();
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
		if (AuctionId.Length != 0)
		{
			output.WriteRawTag(10);
			output.WriteString(AuctionId);
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
		if (AuctionId.Length != 0)
		{
			num += 1 + CodedOutputStream.ComputeStringSize(AuctionId);
		}
		if (_unknownFields != null)
		{
			num += _unknownFields.CalculateSize();
		}
		return num;
	}

	[DebuggerNonUserCode]
	public void MergeFrom(AuctionGoodsCancelReq_1006009 other)
	{
		if (other != null)
		{
			if (other.AuctionId.Length != 0)
			{
				AuctionId = other.AuctionId;
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
			if (num != 10)
			{
				_unknownFields = UnknownFieldSet.MergeFieldFrom(_unknownFields, input);
			}
			else
			{
				AuctionId = input.ReadString();
			}
		}
	}
}
