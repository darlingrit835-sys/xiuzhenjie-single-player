using System;
using System.Diagnostics;
using Google.Protobuf;
using Google.Protobuf.Reflection;

public sealed class BattleEndRingOutGoodsResp_1004036 : IMessage<BattleEndRingOutGoodsResp_1004036>, IMessage, IEquatable<BattleEndRingOutGoodsResp_1004036>, IDeepCloneable<BattleEndRingOutGoodsResp_1004036>
{
	private static readonly MessageParser<BattleEndRingOutGoodsResp_1004036> _parser = new MessageParser<BattleEndRingOutGoodsResp_1004036>(() => new BattleEndRingOutGoodsResp_1004036());

	private UnknownFieldSet _unknownFields;

	public const int CodeFieldNumber = 1;

	private int code_;

	public const int IsSuccessFieldNumber = 2;

	private bool isSuccess_;

	public const int NowCapacityFieldNumber = 3;

	private int nowCapacity_;

	public const int GoodsIdFieldNumber = 4;

	private string goodsId_ = "";

	[DebuggerNonUserCode]
	public static MessageParser<BattleEndRingOutGoodsResp_1004036> Parser => _parser;

	[DebuggerNonUserCode]
	public static MessageDescriptor Descriptor => GoodsProtoReflection.Descriptor.MessageTypes[61];

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
	public int NowCapacity
	{
		get
		{
			return nowCapacity_;
		}
		set
		{
			nowCapacity_ = value;
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

	private MessageDescriptor pb_003A_003AGoogle_002EProtobuf_002EIMessage_002Eget_Descriptor()
	{
		return Descriptor;
	}

	[DebuggerNonUserCode]
	public BattleEndRingOutGoodsResp_1004036()
	{
	}

	[DebuggerNonUserCode]
	public BattleEndRingOutGoodsResp_1004036(BattleEndRingOutGoodsResp_1004036 other)
		: this()
	{
		code_ = other.code_;
		isSuccess_ = other.isSuccess_;
		nowCapacity_ = other.nowCapacity_;
		goodsId_ = other.goodsId_;
		_unknownFields = UnknownFieldSet.Clone(other._unknownFields);
	}

	[DebuggerNonUserCode]
	public BattleEndRingOutGoodsResp_1004036 Clone()
	{
		return new BattleEndRingOutGoodsResp_1004036(this);
	}

	[DebuggerNonUserCode]
	public override bool Equals(object other)
	{
		return Equals(other as BattleEndRingOutGoodsResp_1004036);
	}

	[DebuggerNonUserCode]
	public bool Equals(BattleEndRingOutGoodsResp_1004036 other)
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
		if (NowCapacity != other.NowCapacity)
		{
			return false;
		}
		if (GoodsId != other.GoodsId)
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
		if (NowCapacity != 0)
		{
			num ^= NowCapacity.GetHashCode();
		}
		if (GoodsId.Length != 0)
		{
			num ^= GoodsId.GetHashCode();
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
		if (NowCapacity != 0)
		{
			output.WriteRawTag(24);
			output.WriteInt32(NowCapacity);
		}
		if (GoodsId.Length != 0)
		{
			output.WriteRawTag(34);
			output.WriteString(GoodsId);
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
		if (NowCapacity != 0)
		{
			num += 1 + CodedOutputStream.ComputeInt32Size(NowCapacity);
		}
		if (GoodsId.Length != 0)
		{
			num += 1 + CodedOutputStream.ComputeStringSize(GoodsId);
		}
		if (_unknownFields != null)
		{
			num += _unknownFields.CalculateSize();
		}
		return num;
	}

	[DebuggerNonUserCode]
	public void MergeFrom(BattleEndRingOutGoodsResp_1004036 other)
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
			if (other.NowCapacity != 0)
			{
				NowCapacity = other.NowCapacity;
			}
			if (other.GoodsId.Length != 0)
			{
				GoodsId = other.GoodsId;
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
				NowCapacity = input.ReadInt32();
				break;
			case 34u:
				GoodsId = input.ReadString();
				break;
			}
		}
	}
}
