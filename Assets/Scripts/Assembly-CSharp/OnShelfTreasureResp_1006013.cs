using System;
using System.Diagnostics;
using Google.Protobuf;
using Google.Protobuf.Reflection;

public sealed class OnShelfTreasureResp_1006013 : IMessage<OnShelfTreasureResp_1006013>, IMessage, IEquatable<OnShelfTreasureResp_1006013>, IDeepCloneable<OnShelfTreasureResp_1006013>
{
	private static readonly MessageParser<OnShelfTreasureResp_1006013> _parser = new MessageParser<OnShelfTreasureResp_1006013>(() => new OnShelfTreasureResp_1006013());

	private UnknownFieldSet _unknownFields;

	public const int CodeFieldNumber = 1;

	private int code_;

	public const int IsSuccessFieldNumber = 2;

	private bool isSuccess_;

	public const int NowRingCapacityFieldNumber = 3;

	private int nowRingCapacity_;

	[DebuggerNonUserCode]
	public static MessageParser<OnShelfTreasureResp_1006013> Parser => _parser;

	[DebuggerNonUserCode]
	public static MessageDescriptor Descriptor => AuctionProtoReflection.Descriptor.MessageTypes[13];

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
	public int NowRingCapacity
	{
		get
		{
			return nowRingCapacity_;
		}
		set
		{
			nowRingCapacity_ = value;
		}
	}

	private MessageDescriptor pb_003A_003AGoogle_002EProtobuf_002EIMessage_002Eget_Descriptor()
	{
		return Descriptor;
	}

	[DebuggerNonUserCode]
	public OnShelfTreasureResp_1006013()
	{
	}

	[DebuggerNonUserCode]
	public OnShelfTreasureResp_1006013(OnShelfTreasureResp_1006013 other)
		: this()
	{
		code_ = other.code_;
		isSuccess_ = other.isSuccess_;
		nowRingCapacity_ = other.nowRingCapacity_;
		_unknownFields = UnknownFieldSet.Clone(other._unknownFields);
	}

	[DebuggerNonUserCode]
	public OnShelfTreasureResp_1006013 Clone()
	{
		return new OnShelfTreasureResp_1006013(this);
	}

	[DebuggerNonUserCode]
	public override bool Equals(object other)
	{
		return Equals(other as OnShelfTreasureResp_1006013);
	}

	[DebuggerNonUserCode]
	public bool Equals(OnShelfTreasureResp_1006013 other)
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
		if (NowRingCapacity != other.NowRingCapacity)
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
		if (NowRingCapacity != 0)
		{
			num ^= NowRingCapacity.GetHashCode();
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
		if (NowRingCapacity != 0)
		{
			output.WriteRawTag(24);
			output.WriteInt32(NowRingCapacity);
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
		if (NowRingCapacity != 0)
		{
			num += 1 + CodedOutputStream.ComputeInt32Size(NowRingCapacity);
		}
		if (_unknownFields != null)
		{
			num += _unknownFields.CalculateSize();
		}
		return num;
	}

	[DebuggerNonUserCode]
	public void MergeFrom(OnShelfTreasureResp_1006013 other)
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
			if (other.NowRingCapacity != 0)
			{
				NowRingCapacity = other.NowRingCapacity;
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
				NowRingCapacity = input.ReadInt32();
				break;
			}
		}
	}
}
