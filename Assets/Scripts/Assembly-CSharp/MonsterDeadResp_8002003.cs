using System;
using System.Diagnostics;
using Google.Protobuf;
using Google.Protobuf.Collections;
using Google.Protobuf.Reflection;

public sealed class MonsterDeadResp_8002003 : IMessage<MonsterDeadResp_8002003>, IMessage, IEquatable<MonsterDeadResp_8002003>, IDeepCloneable<MonsterDeadResp_8002003>
{
	private static readonly MessageParser<MonsterDeadResp_8002003> _parser = new MessageParser<MonsterDeadResp_8002003>(() => new MonsterDeadResp_8002003());

	private UnknownFieldSet _unknownFields;

	public const int DeadMonsterIndexFieldNumber = 1;

	private int deadMonsterIndex_;

	public const int GoodsIdFieldNumber = 2;

	private string goodsId_ = "";

	public const int PosXFieldNumber = 3;

	private double posX_;

	public const int PosYFieldNumber = 4;

	private double posY_;

	public const int FiveElementFieldNumber = 5;

	private int fiveElement_;

	public const int GoodsTypeFieldNumber = 6;

	private int goodsType_;

	public const int NameFieldNumber = 7;

	private string name_ = "";

	public const int LevelFieldNumber = 8;

	private int level_;

	public const int RankFieldNumber = 9;

	private int rank_;

	public const int ObjectIndexFieldNumber = 10;

	private int objectIndex_;

	public const int CountFieldNumber = 11;

	private int count_;

	[DebuggerNonUserCode]
	public static MessageParser<MonsterDeadResp_8002003> Parser => _parser;

	[DebuggerNonUserCode]
	public static MessageDescriptor Descriptor => ClientBattleProtoReflection.Descriptor.MessageTypes[18];

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

	[DebuggerNonUserCode]
	public int FiveElement
	{
		get
		{
			return fiveElement_;
		}
		set
		{
			fiveElement_ = value;
		}
	}

	[DebuggerNonUserCode]
	public int GoodsType
	{
		get
		{
			return goodsType_;
		}
		set
		{
			goodsType_ = value;
		}
	}

	[DebuggerNonUserCode]
	public string Name
	{
		get
		{
			return name_;
		}
		set
		{
			name_ = ProtoPreconditions.CheckNotNull(value, "value");
		}
	}

	[DebuggerNonUserCode]
	public int Level
	{
		get
		{
			return level_;
		}
		set
		{
			level_ = value;
		}
	}

	[DebuggerNonUserCode]
	public int Rank
	{
		get
		{
			return rank_;
		}
		set
		{
			rank_ = value;
		}
	}

	[DebuggerNonUserCode]
	public int ObjectIndex
	{
		get
		{
			return objectIndex_;
		}
		set
		{
			objectIndex_ = value;
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
	public MonsterDeadResp_8002003()
	{
	}

	[DebuggerNonUserCode]
	public MonsterDeadResp_8002003(MonsterDeadResp_8002003 other)
		: this()
	{
		deadMonsterIndex_ = other.deadMonsterIndex_;
		goodsId_ = other.goodsId_;
		posX_ = other.posX_;
		posY_ = other.posY_;
		fiveElement_ = other.fiveElement_;
		goodsType_ = other.goodsType_;
		name_ = other.name_;
		level_ = other.level_;
		rank_ = other.rank_;
		objectIndex_ = other.objectIndex_;
		count_ = other.count_;
		_unknownFields = UnknownFieldSet.Clone(other._unknownFields);
	}

	[DebuggerNonUserCode]
	public MonsterDeadResp_8002003 Clone()
	{
		return new MonsterDeadResp_8002003(this);
	}

	[DebuggerNonUserCode]
	public override bool Equals(object other)
	{
		return Equals(other as MonsterDeadResp_8002003);
	}

	[DebuggerNonUserCode]
	public bool Equals(MonsterDeadResp_8002003 other)
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
		if (GoodsId != other.GoodsId)
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
		if (FiveElement != other.FiveElement)
		{
			return false;
		}
		if (GoodsType != other.GoodsType)
		{
			return false;
		}
		if (Name != other.Name)
		{
			return false;
		}
		if (Level != other.Level)
		{
			return false;
		}
		if (Rank != other.Rank)
		{
			return false;
		}
		if (ObjectIndex != other.ObjectIndex)
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
		if (DeadMonsterIndex != 0)
		{
			num ^= DeadMonsterIndex.GetHashCode();
		}
		if (GoodsId.Length != 0)
		{
			num ^= GoodsId.GetHashCode();
		}
		if (PosX != 0.0)
		{
			num ^= ProtobufEqualityComparers.BitwiseDoubleEqualityComparer.GetHashCode(PosX);
		}
		if (PosY != 0.0)
		{
			num ^= ProtobufEqualityComparers.BitwiseDoubleEqualityComparer.GetHashCode(PosY);
		}
		if (FiveElement != 0)
		{
			num ^= FiveElement.GetHashCode();
		}
		if (GoodsType != 0)
		{
			num ^= GoodsType.GetHashCode();
		}
		if (Name.Length != 0)
		{
			num ^= Name.GetHashCode();
		}
		if (Level != 0)
		{
			num ^= Level.GetHashCode();
		}
		if (Rank != 0)
		{
			num ^= Rank.GetHashCode();
		}
		if (ObjectIndex != 0)
		{
			num ^= ObjectIndex.GetHashCode();
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
		if (DeadMonsterIndex != 0)
		{
			output.WriteRawTag(8);
			output.WriteInt32(DeadMonsterIndex);
		}
		if (GoodsId.Length != 0)
		{
			output.WriteRawTag(18);
			output.WriteString(GoodsId);
		}
		if (PosX != 0.0)
		{
			output.WriteRawTag(25);
			output.WriteDouble(PosX);
		}
		if (PosY != 0.0)
		{
			output.WriteRawTag(33);
			output.WriteDouble(PosY);
		}
		if (FiveElement != 0)
		{
			output.WriteRawTag(40);
			output.WriteInt32(FiveElement);
		}
		if (GoodsType != 0)
		{
			output.WriteRawTag(48);
			output.WriteInt32(GoodsType);
		}
		if (Name.Length != 0)
		{
			output.WriteRawTag(58);
			output.WriteString(Name);
		}
		if (Level != 0)
		{
			output.WriteRawTag(64);
			output.WriteInt32(Level);
		}
		if (Rank != 0)
		{
			output.WriteRawTag(72);
			output.WriteInt32(Rank);
		}
		if (ObjectIndex != 0)
		{
			output.WriteRawTag(80);
			output.WriteInt32(ObjectIndex);
		}
		if (Count != 0)
		{
			output.WriteRawTag(88);
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
		if (DeadMonsterIndex != 0)
		{
			num += 1 + CodedOutputStream.ComputeInt32Size(DeadMonsterIndex);
		}
		if (GoodsId.Length != 0)
		{
			num += 1 + CodedOutputStream.ComputeStringSize(GoodsId);
		}
		if (PosX != 0.0)
		{
			num += 9;
		}
		if (PosY != 0.0)
		{
			num += 9;
		}
		if (FiveElement != 0)
		{
			num += 1 + CodedOutputStream.ComputeInt32Size(FiveElement);
		}
		if (GoodsType != 0)
		{
			num += 1 + CodedOutputStream.ComputeInt32Size(GoodsType);
		}
		if (Name.Length != 0)
		{
			num += 1 + CodedOutputStream.ComputeStringSize(Name);
		}
		if (Level != 0)
		{
			num += 1 + CodedOutputStream.ComputeInt32Size(Level);
		}
		if (Rank != 0)
		{
			num += 1 + CodedOutputStream.ComputeInt32Size(Rank);
		}
		if (ObjectIndex != 0)
		{
			num += 1 + CodedOutputStream.ComputeInt32Size(ObjectIndex);
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
	public void MergeFrom(MonsterDeadResp_8002003 other)
	{
		if (other != null)
		{
			if (other.DeadMonsterIndex != 0)
			{
				DeadMonsterIndex = other.DeadMonsterIndex;
			}
			if (other.GoodsId.Length != 0)
			{
				GoodsId = other.GoodsId;
			}
			if (other.PosX != 0.0)
			{
				PosX = other.PosX;
			}
			if (other.PosY != 0.0)
			{
				PosY = other.PosY;
			}
			if (other.FiveElement != 0)
			{
				FiveElement = other.FiveElement;
			}
			if (other.GoodsType != 0)
			{
				GoodsType = other.GoodsType;
			}
			if (other.Name.Length != 0)
			{
				Name = other.Name;
			}
			if (other.Level != 0)
			{
				Level = other.Level;
			}
			if (other.Rank != 0)
			{
				Rank = other.Rank;
			}
			if (other.ObjectIndex != 0)
			{
				ObjectIndex = other.ObjectIndex;
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
			case 8u:
				DeadMonsterIndex = input.ReadInt32();
				break;
			case 18u:
				GoodsId = input.ReadString();
				break;
			case 25u:
				PosX = input.ReadDouble();
				break;
			case 33u:
				PosY = input.ReadDouble();
				break;
			case 40u:
				FiveElement = input.ReadInt32();
				break;
			case 48u:
				GoodsType = input.ReadInt32();
				break;
			case 58u:
				Name = input.ReadString();
				break;
			case 64u:
				Level = input.ReadInt32();
				break;
			case 72u:
				Rank = input.ReadInt32();
				break;
			case 80u:
				ObjectIndex = input.ReadInt32();
				break;
			case 88u:
				Count = input.ReadInt32();
				break;
			}
		}
	}
}
