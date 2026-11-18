using System;
using System.Diagnostics;
using Google.Protobuf;
using Google.Protobuf.Reflection;

public sealed class TreasureGoodsCancelReq_1006016 : IMessage<TreasureGoodsCancelReq_1006016>, IMessage, IEquatable<TreasureGoodsCancelReq_1006016>, IDeepCloneable<TreasureGoodsCancelReq_1006016>
{
	private static readonly MessageParser<TreasureGoodsCancelReq_1006016> _parser = new MessageParser<TreasureGoodsCancelReq_1006016>(() => new TreasureGoodsCancelReq_1006016());

	private UnknownFieldSet _unknownFields;

	public const int TreasureIdFieldNumber = 1;

	private int treasureId_;

	[DebuggerNonUserCode]
	public static MessageParser<TreasureGoodsCancelReq_1006016> Parser => _parser;

	[DebuggerNonUserCode]
	public static MessageDescriptor Descriptor => AuctionProtoReflection.Descriptor.MessageTypes[15];

	[DebuggerNonUserCode]
	MessageDescriptor IMessage.Descriptor => pb_003A_003AGoogle_002EProtobuf_002EIMessage_002Eget_Descriptor();

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
	public TreasureGoodsCancelReq_1006016()
	{
	}

	[DebuggerNonUserCode]
	public TreasureGoodsCancelReq_1006016(TreasureGoodsCancelReq_1006016 other)
		: this()
	{
		treasureId_ = other.treasureId_;
		_unknownFields = UnknownFieldSet.Clone(other._unknownFields);
	}

	[DebuggerNonUserCode]
	public TreasureGoodsCancelReq_1006016 Clone()
	{
		return new TreasureGoodsCancelReq_1006016(this);
	}

	[DebuggerNonUserCode]
	public override bool Equals(object other)
	{
		return Equals(other as TreasureGoodsCancelReq_1006016);
	}

	[DebuggerNonUserCode]
	public bool Equals(TreasureGoodsCancelReq_1006016 other)
	{
		if (other == null)
		{
			return false;
		}
		if (other == this)
		{
			return true;
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
		if (TreasureId != 0)
		{
			output.WriteRawTag(8);
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
	public void MergeFrom(TreasureGoodsCancelReq_1006016 other)
	{
		if (other != null)
		{
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
			if (num != 8)
			{
				_unknownFields = UnknownFieldSet.MergeFieldFrom(_unknownFields, input);
			}
			else
			{
				TreasureId = input.ReadInt32();
			}
		}
	}
}
