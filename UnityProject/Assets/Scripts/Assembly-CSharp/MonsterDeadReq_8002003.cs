using System;
using System.Diagnostics;
using Google.Protobuf;
using Google.Protobuf.Collections;
using Google.Protobuf.Reflection;

public sealed class MonsterDeadReq_8002003 : IMessage<MonsterDeadReq_8002003>, IMessage, IEquatable<MonsterDeadReq_8002003>, IDeepCloneable<MonsterDeadReq_8002003>
{
	private static readonly MessageParser<MonsterDeadReq_8002003> _parser = new MessageParser<MonsterDeadReq_8002003>(() => new MonsterDeadReq_8002003());

	private UnknownFieldSet _unknownFields;

	public const int DeadMonsterIndexFieldNumber = 1;

	private int deadMonsterIndex_;

	public const int PosXFieldNumber = 2;

	private double posX_;

	public const int PosYFieldNumber = 3;

	private double posY_;

	[DebuggerNonUserCode]
	public static MessageParser<MonsterDeadReq_8002003> Parser => _parser;

	[DebuggerNonUserCode]
	public static MessageDescriptor Descriptor => ClientBattleProtoReflection.Descriptor.MessageTypes[17];

	[DebuggerNonUserCode]
	MessageDescriptor IMessage.Descriptor => pb_003A_003AGoogle_002EProtobuf_002EIMessage_002Eget_Descriptor();

	[DebuggerNonUserCode]
	public int DeadMonsterIndex
	{
		get
		{
			return deadMonsterIndex_;
		}
		set
		{
			deadMonsterIndex_ = value;
		}
	}

	[DebuggerNonUserCode]
	public double PosX
	{
		get
		{
			return posX_;
		}
		set
		{
			posX_ = value;
		}
	}

	[DebuggerNonUserCode]
	public double PosY
	{
		get
		{
			return posY_;
		}
		set
		{
			posY_ = value;
		}
	}

	private MessageDescriptor pb_003A_003AGoogle_002EProtobuf_002EIMessage_002Eget_Descriptor()
	{
		return Descriptor;
	}

	[DebuggerNonUserCode]
	public MonsterDeadReq_8002003()
	{
	}

	[DebuggerNonUserCode]
	public MonsterDeadReq_8002003(MonsterDeadReq_8002003 other)
		: this()
	{
		deadMonsterIndex_ = other.deadMonsterIndex_;
		posX_ = other.posX_;
		posY_ = other.posY_;
		_unknownFields = UnknownFieldSet.Clone(other._unknownFields);
	}

	[DebuggerNonUserCode]
	public MonsterDeadReq_8002003 Clone()
	{
		return new MonsterDeadReq_8002003(this);
	}

	[DebuggerNonUserCode]
	public override bool Equals(object other)
	{
		return Equals(other as MonsterDeadReq_8002003);
	}

	[DebuggerNonUserCode]
	public bool Equals(MonsterDeadReq_8002003 other)
	{
		if (other == null)
		{
			return false;
		}
		if (other == this)
		{
			return true;
		}
		if (DeadMonsterIndex != other.DeadMonsterIndex)
		{
			return false;
		}
		if (!ProtobufEqualityComparers.BitwiseDoubleEqualityComparer.Equals(PosX, other.PosX))
		{
			return false;
		}
		if (!ProtobufEqualityComparers.BitwiseDoubleEqualityComparer.Equals(PosY, other.PosY))
		{
			return false;
		}
		return object.Equals(_unknownFields, other._unknownFields);
	}

	[DebuggerNonUserCode]
	public override int GetHashCode()
	{
		int num = 1;
		if (DeadMonsterIndex != 0)
		{
			num ^= DeadMonsterIndex.GetHashCode();
		}
		if (PosX != 0.0)
		{
			num ^= ProtobufEqualityComparers.BitwiseDoubleEqualityComparer.GetHashCode(PosX);
		}
		if (PosY != 0.0)
		{
			num ^= ProtobufEqualityComparers.BitwiseDoubleEqualityComparer.GetHashCode(PosY);
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
		if (DeadMonsterIndex != 0)
		{
			output.WriteRawTag(8);
			output.WriteInt32(DeadMonsterIndex);
		}
		if (PosX != 0.0)
		{
			output.WriteRawTag(17);
			output.WriteDouble(PosX);
		}
		if (PosY != 0.0)
		{
			output.WriteRawTag(25);
			output.WriteDouble(PosY);
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
		if (DeadMonsterIndex != 0)
		{
			num += 1 + CodedOutputStream.ComputeInt32Size(DeadMonsterIndex);
		}
		if (PosX != 0.0)
		{
			num += 9;
		}
		if (PosY != 0.0)
		{
			num += 9;
		}
		if (_unknownFields != null)
		{
			num += _unknownFields.CalculateSize();
		}
		return num;
	}

	[DebuggerNonUserCode]
	public void MergeFrom(MonsterDeadReq_8002003 other)
	{
		if (other != null)
		{
			if (other.DeadMonsterIndex != 0)
			{
				DeadMonsterIndex = other.DeadMonsterIndex;
			}
			if (other.PosX != 0.0)
			{
				PosX = other.PosX;
			}
			if (other.PosY != 0.0)
			{
				PosY = other.PosY;
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
				DeadMonsterIndex = input.ReadInt32();
				break;
			case 17u:
				PosX = input.ReadDouble();
				break;
			case 25u:
				PosY = input.ReadDouble();
				break;
			}
		}
	}
}
