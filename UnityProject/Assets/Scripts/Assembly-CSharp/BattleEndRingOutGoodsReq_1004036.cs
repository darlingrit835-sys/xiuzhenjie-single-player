using System;
using System.Diagnostics;
using Google.Protobuf;
using Google.Protobuf.Reflection;

public sealed class BattleEndRingOutGoodsReq_1004036 : IMessage<BattleEndRingOutGoodsReq_1004036>, IMessage, IEquatable<BattleEndRingOutGoodsReq_1004036>, IDeepCloneable<BattleEndRingOutGoodsReq_1004036>
{
	private static readonly MessageParser<BattleEndRingOutGoodsReq_1004036> _parser = new MessageParser<BattleEndRingOutGoodsReq_1004036>(() => new BattleEndRingOutGoodsReq_1004036());

	private UnknownFieldSet _unknownFields;

	public const int GoodsIdFieldNumber = 1;

	private string goodsId_ = "";

	public const int BattleIdFieldNumber = 2;

	private string battleId_ = "";

	public const int CountFieldNumber = 3;

	private int count_;

	[DebuggerNonUserCode]
	public static MessageParser<BattleEndRingOutGoodsReq_1004036> Parser => _parser;

	[DebuggerNonUserCode]
	public static MessageDescriptor Descriptor => GoodsProtoReflection.Descriptor.MessageTypes[60];

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
	public string BattleId
	{
		get
		{
			return battleId_;
		}
		set
		{
			battleId_ = ProtoPreconditions.CheckNotNull(value, "value");
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
	public BattleEndRingOutGoodsReq_1004036()
	{
	}

	[DebuggerNonUserCode]
	public BattleEndRingOutGoodsReq_1004036(BattleEndRingOutGoodsReq_1004036 other)
		: this()
	{
		goodsId_ = other.goodsId_;
		battleId_ = other.battleId_;
		count_ = other.count_;
		_unknownFields = UnknownFieldSet.Clone(other._unknownFields);
	}

	[DebuggerNonUserCode]
	public BattleEndRingOutGoodsReq_1004036 Clone()
	{
		return new BattleEndRingOutGoodsReq_1004036(this);
	}

	[DebuggerNonUserCode]
	public override bool Equals(object other)
	{
		return Equals(other as BattleEndRingOutGoodsReq_1004036);
	}

	[DebuggerNonUserCode]
	public bool Equals(BattleEndRingOutGoodsReq_1004036 other)
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
		if (BattleId != other.BattleId)
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
		if (BattleId.Length != 0)
		{
			num ^= BattleId.GetHashCode();
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
		if (BattleId.Length != 0)
		{
			output.WriteRawTag(18);
			output.WriteString(BattleId);
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
		if (GoodsId.Length != 0)
		{
			num += 1 + CodedOutputStream.ComputeStringSize(GoodsId);
		}
		if (BattleId.Length != 0)
		{
			num += 1 + CodedOutputStream.ComputeStringSize(BattleId);
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
	public void MergeFrom(BattleEndRingOutGoodsReq_1004036 other)
	{
		if (other != null)
		{
			if (other.GoodsId.Length != 0)
			{
				GoodsId = other.GoodsId;
			}
			if (other.BattleId.Length != 0)
			{
				BattleId = other.BattleId;
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
			case 18u:
				BattleId = input.ReadString();
				break;
			case 24u:
				Count = input.ReadInt32();
				break;
			}
		}
	}
}
