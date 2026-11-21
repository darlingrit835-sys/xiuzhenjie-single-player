using System;
using System.Diagnostics;
using Google.Protobuf;
using Google.Protobuf.Reflection;

public sealed class BattleEndLeaveReq_1004037 : IMessage<BattleEndLeaveReq_1004037>, IMessage, IEquatable<BattleEndLeaveReq_1004037>, IDeepCloneable<BattleEndLeaveReq_1004037>
{
	private static readonly MessageParser<BattleEndLeaveReq_1004037> _parser = new MessageParser<BattleEndLeaveReq_1004037>(() => new BattleEndLeaveReq_1004037());

	private UnknownFieldSet _unknownFields;

	public const int BattleIdFieldNumber = 1;

	private string battleId_ = "";

	[DebuggerNonUserCode]
	public static MessageParser<BattleEndLeaveReq_1004037> Parser => _parser;

	[DebuggerNonUserCode]
	public static MessageDescriptor Descriptor => GoodsProtoReflection.Descriptor.MessageTypes[62];

	[DebuggerNonUserCode]
	MessageDescriptor IMessage.Descriptor => pb_003A_003AGoogle_002EProtobuf_002EIMessage_002Eget_Descriptor();

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
	public BattleEndLeaveReq_1004037()
	{
	}

	[DebuggerNonUserCode]
	public BattleEndLeaveReq_1004037(BattleEndLeaveReq_1004037 other)
		: this()
	{
		battleId_ = other.battleId_;
		_unknownFields = UnknownFieldSet.Clone(other._unknownFields);
	}

	[DebuggerNonUserCode]
	public BattleEndLeaveReq_1004037 Clone()
	{
		return new BattleEndLeaveReq_1004037(this);
	}

	[DebuggerNonUserCode]
	public override bool Equals(object other)
	{
		return Equals(other as BattleEndLeaveReq_1004037);
	}

	[DebuggerNonUserCode]
	public bool Equals(BattleEndLeaveReq_1004037 other)
	{
		if (other == null)
		{
			return false;
		}
		if (other == this)
		{
			return true;
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
		if (BattleId.Length != 0)
		{
			output.WriteRawTag(10);
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
	public void MergeFrom(BattleEndLeaveReq_1004037 other)
	{
		if (other != null)
		{
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
			if (num != 10)
			{
				_unknownFields = UnknownFieldSet.MergeFieldFrom(_unknownFields, input);
			}
			else
			{
				BattleId = input.ReadString();
			}
		}
	}
}
