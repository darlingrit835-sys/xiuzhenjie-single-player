using System;
using System.Diagnostics;
using Google.Protobuf;
using Google.Protobuf.Reflection;

public sealed class EnterTreasureRoomReq_1006011 : IMessage<EnterTreasureRoomReq_1006011>, IMessage, IEquatable<EnterTreasureRoomReq_1006011>, IDeepCloneable<EnterTreasureRoomReq_1006011>
{
	private static readonly MessageParser<EnterTreasureRoomReq_1006011> _parser = new MessageParser<EnterTreasureRoomReq_1006011>(() => new EnterTreasureRoomReq_1006011());

	private UnknownFieldSet _unknownFields;

	public const int StageFieldNumber = 1;

	private int stage_;

	[DebuggerNonUserCode]
	public static MessageParser<EnterTreasureRoomReq_1006011> Parser => _parser;

	[DebuggerNonUserCode]
	public static MessageDescriptor Descriptor => AuctionProtoReflection.Descriptor.MessageTypes[10];

	[DebuggerNonUserCode]
	MessageDescriptor IMessage.Descriptor => pb_003A_003AGoogle_002EProtobuf_002EIMessage_002Eget_Descriptor();

	[DebuggerNonUserCode]
	public int Stage
	{
		get
		{
			return stage_;
		}
		set
		{
			stage_ = value;
		}
	}

	private MessageDescriptor pb_003A_003AGoogle_002EProtobuf_002EIMessage_002Eget_Descriptor()
	{
		return Descriptor;
	}

	[DebuggerNonUserCode]
	public EnterTreasureRoomReq_1006011()
	{
	}

	[DebuggerNonUserCode]
	public EnterTreasureRoomReq_1006011(EnterTreasureRoomReq_1006011 other)
		: this()
	{
		stage_ = other.stage_;
		_unknownFields = UnknownFieldSet.Clone(other._unknownFields);
	}

	[DebuggerNonUserCode]
	public EnterTreasureRoomReq_1006011 Clone()
	{
		return new EnterTreasureRoomReq_1006011(this);
	}

	[DebuggerNonUserCode]
	public override bool Equals(object other)
	{
		return Equals(other as EnterTreasureRoomReq_1006011);
	}

	[DebuggerNonUserCode]
	public bool Equals(EnterTreasureRoomReq_1006011 other)
	{
		if (other == null)
		{
			return false;
		}
		if (other == this)
		{
			return true;
		}
		if (Stage != other.Stage)
		{
			return false;
		}
		return object.Equals(_unknownFields, other._unknownFields);
	}

	[DebuggerNonUserCode]
	public override int GetHashCode()
	{
		int num = 1;
		if (Stage != 0)
		{
			num ^= Stage.GetHashCode();
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
		if (Stage != 0)
		{
			output.WriteRawTag(8);
			output.WriteInt32(Stage);
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
		if (Stage != 0)
		{
			num += 1 + CodedOutputStream.ComputeInt32Size(Stage);
		}
		if (_unknownFields != null)
		{
			num += _unknownFields.CalculateSize();
		}
		return num;
	}

	[DebuggerNonUserCode]
	public void MergeFrom(EnterTreasureRoomReq_1006011 other)
	{
		if (other != null)
		{
			if (other.Stage != 0)
			{
				Stage = other.Stage;
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
				Stage = input.ReadInt32();
			}
		}
	}
}
