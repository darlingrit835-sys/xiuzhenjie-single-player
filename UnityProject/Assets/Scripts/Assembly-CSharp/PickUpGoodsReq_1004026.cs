using System;
using System.Diagnostics;
using Google.Protobuf;
using Google.Protobuf.Reflection;

public sealed class PickUpGoodsReq_1004026 : IMessage<PickUpGoodsReq_1004026>, IMessage, IEquatable<PickUpGoodsReq_1004026>, IDeepCloneable<PickUpGoodsReq_1004026>
{
	private static readonly MessageParser<PickUpGoodsReq_1004026> _parser = new MessageParser<PickUpGoodsReq_1004026>(() => new PickUpGoodsReq_1004026());

	private UnknownFieldSet _unknownFields;

	public const int GoodsIdFieldNumber = 1;

	private string goodsId_ = "";

	public const int CountFieldNumber = 2;

	private int count_;

	public const int BattleIdFieldNumber = 3;

	private string battleId_ = "";

	[DebuggerNonUserCode]
	public static MessageParser<PickUpGoodsReq_1004026> Parser => _parser;

	[DebuggerNonUserCode]
	public static MessageDescriptor Descriptor => GoodsProtoReflection.Descriptor.MessageTypes[45];

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

	private MessageDescriptor pb_003A_003AGoogle_002EProtobuf_002EIMessage_002Eget_Descriptor()
	{
		return Descriptor;
	}

	[DebuggerNonUserCode]
	public PickUpGoodsReq_1004026()
	{
	}

	[DebuggerNonUserCode]
	public PickUpGoodsReq_1004026(PickUpGoodsReq_1004026 other)
		: this()
	{
		goodsId_ = other.goodsId_;
		count_ = other.count_;
		battleId_ = other.battleId_;
		_unknownFields = UnknownFieldSet.Clone(other._unknownFields);
	}

	[DebuggerNonUserCode]
	public PickUpGoodsReq_1004026 Clone()
	{
		return new PickUpGoodsReq_1004026(this);
	}

	[DebuggerNonUserCode]
	public override bool Equals(object other)
	{
		return Equals(other as PickUpGoodsReq_1004026);
	}

	[DebuggerNonUserCode]
	public bool Equals(PickUpGoodsReq_1004026 other)
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
		if (Count != other.Count)
		{
			return false;
		}
		if (BattleId != other.BattleId)
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
		if (Count != 0)
		{
			num ^= Count.GetHashCode();
		}
		if (BattleId.Length != 0)
		{
			num ^= BattleId.GetHashCode();
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
		if (Count != 0)
		{
			output.WriteRawTag(16);
			output.WriteInt32(Count);
		}
		if (BattleId.Length != 0)
		{
			output.WriteRawTag(26);
			output.WriteString(BattleId);
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
		if (Count != 0)
		{
			num += 1 + CodedOutputStream.ComputeInt32Size(Count);
		}
		if (BattleId.Length != 0)
		{
			num += 1 + CodedOutputStream.ComputeStringSize(BattleId);
		}
		if (_unknownFields != null)
		{
			num += _unknownFields.CalculateSize();
		}
		return num;
	}

	[DebuggerNonUserCode]
	public void MergeFrom(PickUpGoodsReq_1004026 other)
	{
		if (other != null)
		{
			if (other.GoodsId.Length != 0)
			{
				GoodsId = other.GoodsId;
			}
			if (other.Count != 0)
			{
				Count = other.Count;
			}
			if (other.BattleId.Length != 0)
			{
				BattleId = other.BattleId;
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
				Count = input.ReadInt32();
				break;
			case 26u:
				BattleId = input.ReadString();
				break;
			}
		}
	}
}
