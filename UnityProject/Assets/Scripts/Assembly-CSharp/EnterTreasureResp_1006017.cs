using System;
using System.Diagnostics;
using Google.Protobuf;
using Google.Protobuf.Reflection;

public sealed class EnterTreasureResp_1006017 : IMessage<EnterTreasureResp_1006017>, IMessage, IEquatable<EnterTreasureResp_1006017>, IDeepCloneable<EnterTreasureResp_1006017>
{
	private static readonly MessageParser<EnterTreasureResp_1006017> _parser = new MessageParser<EnterTreasureResp_1006017>(() => new EnterTreasureResp_1006017());

	private UnknownFieldSet _unknownFields;

	public const int CodeFieldNumber = 1;

	private int code_;

	public const int IsSuccessFieldNumber = 2;

	private bool isSuccess_;

	public const int ShinyStoneFieldNumber = 3;

	private int shinyStone_;

	public const int TreasureIdFieldNumber = 4;

	private int treasureId_;

	public const int IsMayAddPriceFieldNumber = 5;

	private bool isMayAddPrice_;

	[DebuggerNonUserCode]
	public static MessageParser<EnterTreasureResp_1006017> Parser => _parser;

	[DebuggerNonUserCode]
	public static MessageDescriptor Descriptor => AuctionProtoReflection.Descriptor.MessageTypes[18];

	[DebuggerNonUserCode]
	MessageDescriptor IMessage.Descriptor => pb_003A_003AGoogle_002EProtobuf_002EIMessage_002Eget_Descriptor();

	[DebuggerNonUserCode]
	public int Code
	{
		get
		{
			return code_;
		}
		set
		{
			code_ = value;
		}
	}

	[DebuggerNonUserCode]
	public bool IsSuccess
	{
		get
		{
			return isSuccess_;
		}
		set
		{
			isSuccess_ = value;
		}
	}

	[DebuggerNonUserCode]
	public int ShinyStone
	{
		get
		{
			return shinyStone_;
		}
		set
		{
			shinyStone_ = value;
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

	[DebuggerNonUserCode]
	public bool IsMayAddPrice
	{
		get
		{
			return isMayAddPrice_;
		}
		set
		{
			isMayAddPrice_ = value;
		}
	}

	private MessageDescriptor pb_003A_003AGoogle_002EProtobuf_002EIMessage_002Eget_Descriptor()
	{
		return Descriptor;
	}

	[DebuggerNonUserCode]
	public EnterTreasureResp_1006017()
	{
	}

	[DebuggerNonUserCode]
	public EnterTreasureResp_1006017(EnterTreasureResp_1006017 other)
		: this()
	{
		code_ = other.code_;
		isSuccess_ = other.isSuccess_;
		shinyStone_ = other.shinyStone_;
		treasureId_ = other.treasureId_;
		isMayAddPrice_ = other.isMayAddPrice_;
		_unknownFields = UnknownFieldSet.Clone(other._unknownFields);
	}

	[DebuggerNonUserCode]
	public EnterTreasureResp_1006017 Clone()
	{
		return new EnterTreasureResp_1006017(this);
	}

	[DebuggerNonUserCode]
	public override bool Equals(object other)
	{
		return Equals(other as EnterTreasureResp_1006017);
	}

	[DebuggerNonUserCode]
	public bool Equals(EnterTreasureResp_1006017 other)
	{
		if (other == null)
		{
			return false;
		}
		if (other == this)
		{
			return true;
		}
		if (Code != other.Code)
		{
			return false;
		}
		if (IsSuccess != other.IsSuccess)
		{
			return false;
		}
		if (ShinyStone != other.ShinyStone)
		{
			return false;
		}
		if (TreasureId != other.TreasureId)
		{
			return false;
		}
		if (IsMayAddPrice != other.IsMayAddPrice)
		{
			return false;
		}
		return object.Equals(_unknownFields, other._unknownFields);
	}

	[DebuggerNonUserCode]
	public override int GetHashCode()
	{
		int num = 1;
		if (Code != 0)
		{
			num ^= Code.GetHashCode();
		}
		if (IsSuccess)
		{
			num ^= IsSuccess.GetHashCode();
		}
		if (ShinyStone != 0)
		{
			num ^= ShinyStone.GetHashCode();
		}
		if (TreasureId != 0)
		{
			num ^= TreasureId.GetHashCode();
		}
		if (IsMayAddPrice)
		{
			num ^= IsMayAddPrice.GetHashCode();
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
		if (Code != 0)
		{
			output.WriteRawTag(8);
			output.WriteInt32(Code);
		}
		if (IsSuccess)
		{
			output.WriteRawTag(16);
			output.WriteBool(IsSuccess);
		}
		if (ShinyStone != 0)
		{
			output.WriteRawTag(24);
			output.WriteInt32(ShinyStone);
		}
		if (TreasureId != 0)
		{
			output.WriteRawTag(32);
			output.WriteInt32(TreasureId);
		}
		if (IsMayAddPrice)
		{
			output.WriteRawTag(40);
			output.WriteBool(IsMayAddPrice);
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
		if (Code != 0)
		{
			num += 1 + CodedOutputStream.ComputeInt32Size(Code);
		}
		if (IsSuccess)
		{
			num += 2;
		}
		if (ShinyStone != 0)
		{
			num += 1 + CodedOutputStream.ComputeInt32Size(ShinyStone);
		}
		if (TreasureId != 0)
		{
			num += 1 + CodedOutputStream.ComputeInt32Size(TreasureId);
		}
		if (IsMayAddPrice)
		{
			num += 2;
		}
		if (_unknownFields != null)
		{
			num += _unknownFields.CalculateSize();
		}
		return num;
	}

	[DebuggerNonUserCode]
	public void MergeFrom(EnterTreasureResp_1006017 other)
	{
		if (other != null)
		{
			if (other.Code != 0)
			{
				Code = other.Code;
			}
			if (other.IsSuccess)
			{
				IsSuccess = other.IsSuccess;
			}
			if (other.ShinyStone != 0)
			{
				ShinyStone = other.ShinyStone;
			}
			if (other.TreasureId != 0)
			{
				TreasureId = other.TreasureId;
			}
			if (other.IsMayAddPrice)
			{
				IsMayAddPrice = other.IsMayAddPrice;
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
				Code = input.ReadInt32();
				break;
			case 16u:
				IsSuccess = input.ReadBool();
				break;
			case 24u:
				ShinyStone = input.ReadInt32();
				break;
			case 32u:
				TreasureId = input.ReadInt32();
				break;
			case 40u:
				IsMayAddPrice = input.ReadBool();
				break;
			}
		}
	}
}
