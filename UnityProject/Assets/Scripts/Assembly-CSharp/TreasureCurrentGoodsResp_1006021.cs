using System;
using System.Diagnostics;
using Google.Protobuf;
using Google.Protobuf.Reflection;

public sealed class TreasureCurrentGoodsResp_1006021 : IMessage<TreasureCurrentGoodsResp_1006021>, IMessage, IEquatable<TreasureCurrentGoodsResp_1006021>, IDeepCloneable<TreasureCurrentGoodsResp_1006021>
{
	private static readonly MessageParser<TreasureCurrentGoodsResp_1006021> _parser = new MessageParser<TreasureCurrentGoodsResp_1006021>(() => new TreasureCurrentGoodsResp_1006021());

	private UnknownFieldSet _unknownFields;

	public const int CodeFieldNumber = 1;

	private int code_;

	public const int IsSuccessFieldNumber = 2;

	private bool isSuccess_;

	public const int GoodsIdFieldNumber = 3;

	private string goodsId_ = "";

	public const int GoodsNameFieldNumber = 4;

	private string goodsName_ = "";

	public const int FiveElementFieldNumber = 5;

	private int fiveElement_;

	public const int CurrentPersonFieldNumber = 6;

	private string currentPerson_ = "";

	public const int StageFieldNumber = 7;

	private int stage_;

	public const int RankFieldNumber = 8;

	private int rank_;

	public const int LevelFieldNumber = 9;

	private int level_;

	public const int GoodsTypeFieldNumber = 10;

	private int goodsType_;

	public const int CurrentPriceFieldNumber = 11;

	private long currentPrice_;

	public const int PersonImageFieldNumber = 12;

	private int personImage_;

	public const int PersonSexFieldNumber = 13;

	private bool personSex_;

	public const int PersonNameFieldNumber = 14;

	private string personName_ = "";

	public const int IdFieldNumber = 15;

	private int id_;

	public const int StartPriceFieldNumber = 16;

	private long startPrice_;

	[DebuggerNonUserCode]
	public static MessageParser<TreasureCurrentGoodsResp_1006021> Parser => _parser;

	[DebuggerNonUserCode]
	public static MessageDescriptor Descriptor => AuctionProtoReflection.Descriptor.MessageTypes[22];

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
	public string GoodsName
	{
		get
		{
			return goodsName_;
		}
		set
		{
			goodsName_ = ProtoPreconditions.CheckNotNull(value, "value");
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
	public string CurrentPerson
	{
		get
		{
			return currentPerson_;
		}
		set
		{
			currentPerson_ = ProtoPreconditions.CheckNotNull(value, "value");
		}
	}

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
	public long CurrentPrice
	{
		get
		{
			return currentPrice_;
		}
		set
		{
			currentPrice_ = value;
		}
	}

	[DebuggerNonUserCode]
	public int PersonImage
	{
		get
		{
			return personImage_;
		}
		set
		{
			personImage_ = value;
		}
	}

	[DebuggerNonUserCode]
	public bool PersonSex
	{
		get
		{
			return personSex_;
		}
		set
		{
			personSex_ = value;
		}
	}

	[DebuggerNonUserCode]
	public string PersonName
	{
		get
		{
			return personName_;
		}
		set
		{
			personName_ = ProtoPreconditions.CheckNotNull(value, "value");
		}
	}

	[DebuggerNonUserCode]
	public int Id
	{
		get
		{
			return id_;
		}
		set
		{
			id_ = value;
		}
	}

	[DebuggerNonUserCode]
	public long StartPrice
	{
		get
		{
			return startPrice_;
		}
		set
		{
			startPrice_ = value;
		}
	}

	private MessageDescriptor pb_003A_003AGoogle_002EProtobuf_002EIMessage_002Eget_Descriptor()
	{
		return Descriptor;
	}

	[DebuggerNonUserCode]
	public TreasureCurrentGoodsResp_1006021()
	{
	}

	[DebuggerNonUserCode]
	public TreasureCurrentGoodsResp_1006021(TreasureCurrentGoodsResp_1006021 other)
		: this()
	{
		code_ = other.code_;
		isSuccess_ = other.isSuccess_;
		goodsId_ = other.goodsId_;
		goodsName_ = other.goodsName_;
		fiveElement_ = other.fiveElement_;
		currentPerson_ = other.currentPerson_;
		stage_ = other.stage_;
		rank_ = other.rank_;
		level_ = other.level_;
		goodsType_ = other.goodsType_;
		currentPrice_ = other.currentPrice_;
		personImage_ = other.personImage_;
		personSex_ = other.personSex_;
		personName_ = other.personName_;
		id_ = other.id_;
		startPrice_ = other.startPrice_;
		_unknownFields = UnknownFieldSet.Clone(other._unknownFields);
	}

	[DebuggerNonUserCode]
	public TreasureCurrentGoodsResp_1006021 Clone()
	{
		return new TreasureCurrentGoodsResp_1006021(this);
	}

	[DebuggerNonUserCode]
	public override bool Equals(object other)
	{
		return Equals(other as TreasureCurrentGoodsResp_1006021);
	}

	[DebuggerNonUserCode]
	public bool Equals(TreasureCurrentGoodsResp_1006021 other)
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
		if (GoodsId != other.GoodsId)
		{
			return false;
		}
		if (GoodsName != other.GoodsName)
		{
			return false;
		}
		if (FiveElement != other.FiveElement)
		{
			return false;
		}
		if (CurrentPerson != other.CurrentPerson)
		{
			return false;
		}
		if (Stage != other.Stage)
		{
			return false;
		}
		if (Rank != other.Rank)
		{
			return false;
		}
		if (Level != other.Level)
		{
			return false;
		}
		if (GoodsType != other.GoodsType)
		{
			return false;
		}
		if (CurrentPrice != other.CurrentPrice)
		{
			return false;
		}
		if (PersonImage != other.PersonImage)
		{
			return false;
		}
		if (PersonSex != other.PersonSex)
		{
			return false;
		}
		if (PersonName != other.PersonName)
		{
			return false;
		}
		if (Id != other.Id)
		{
			return false;
		}
		if (StartPrice != other.StartPrice)
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
		if (GoodsId.Length != 0)
		{
			num ^= GoodsId.GetHashCode();
		}
		if (GoodsName.Length != 0)
		{
			num ^= GoodsName.GetHashCode();
		}
		if (FiveElement != 0)
		{
			num ^= FiveElement.GetHashCode();
		}
		if (CurrentPerson.Length != 0)
		{
			num ^= CurrentPerson.GetHashCode();
		}
		if (Stage != 0)
		{
			num ^= Stage.GetHashCode();
		}
		if (Rank != 0)
		{
			num ^= Rank.GetHashCode();
		}
		if (Level != 0)
		{
			num ^= Level.GetHashCode();
		}
		if (GoodsType != 0)
		{
			num ^= GoodsType.GetHashCode();
		}
		if (CurrentPrice != 0L)
		{
			num ^= CurrentPrice.GetHashCode();
		}
		if (PersonImage != 0)
		{
			num ^= PersonImage.GetHashCode();
		}
		if (PersonSex)
		{
			num ^= PersonSex.GetHashCode();
		}
		if (PersonName.Length != 0)
		{
			num ^= PersonName.GetHashCode();
		}
		if (Id != 0)
		{
			num ^= Id.GetHashCode();
		}
		if (StartPrice != 0L)
		{
			num ^= StartPrice.GetHashCode();
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
		if (GoodsId.Length != 0)
		{
			output.WriteRawTag(26);
			output.WriteString(GoodsId);
		}
		if (GoodsName.Length != 0)
		{
			output.WriteRawTag(34);
			output.WriteString(GoodsName);
		}
		if (FiveElement != 0)
		{
			output.WriteRawTag(40);
			output.WriteInt32(FiveElement);
		}
		if (CurrentPerson.Length != 0)
		{
			output.WriteRawTag(50);
			output.WriteString(CurrentPerson);
		}
		if (Stage != 0)
		{
			output.WriteRawTag(56);
			output.WriteInt32(Stage);
		}
		if (Rank != 0)
		{
			output.WriteRawTag(64);
			output.WriteInt32(Rank);
		}
		if (Level != 0)
		{
			output.WriteRawTag(72);
			output.WriteInt32(Level);
		}
		if (GoodsType != 0)
		{
			output.WriteRawTag(80);
			output.WriteInt32(GoodsType);
		}
		if (CurrentPrice != 0L)
		{
			output.WriteRawTag(88);
			output.WriteInt64(CurrentPrice);
		}
		if (PersonImage != 0)
		{
			output.WriteRawTag(96);
			output.WriteInt32(PersonImage);
		}
		if (PersonSex)
		{
			output.WriteRawTag(104);
			output.WriteBool(PersonSex);
		}
		if (PersonName.Length != 0)
		{
			output.WriteRawTag(114);
			output.WriteString(PersonName);
		}
		if (Id != 0)
		{
			output.WriteRawTag(120);
			output.WriteInt32(Id);
		}
		if (StartPrice != 0L)
		{
			output.WriteRawTag(128, 1);
			output.WriteInt64(StartPrice);
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
		if (GoodsId.Length != 0)
		{
			num += 1 + CodedOutputStream.ComputeStringSize(GoodsId);
		}
		if (GoodsName.Length != 0)
		{
			num += 1 + CodedOutputStream.ComputeStringSize(GoodsName);
		}
		if (FiveElement != 0)
		{
			num += 1 + CodedOutputStream.ComputeInt32Size(FiveElement);
		}
		if (CurrentPerson.Length != 0)
		{
			num += 1 + CodedOutputStream.ComputeStringSize(CurrentPerson);
		}
		if (Stage != 0)
		{
			num += 1 + CodedOutputStream.ComputeInt32Size(Stage);
		}
		if (Rank != 0)
		{
			num += 1 + CodedOutputStream.ComputeInt32Size(Rank);
		}
		if (Level != 0)
		{
			num += 1 + CodedOutputStream.ComputeInt32Size(Level);
		}
		if (GoodsType != 0)
		{
			num += 1 + CodedOutputStream.ComputeInt32Size(GoodsType);
		}
		if (CurrentPrice != 0L)
		{
			num += 1 + CodedOutputStream.ComputeInt64Size(CurrentPrice);
		}
		if (PersonImage != 0)
		{
			num += 1 + CodedOutputStream.ComputeInt32Size(PersonImage);
		}
		if (PersonSex)
		{
			num += 2;
		}
		if (PersonName.Length != 0)
		{
			num += 1 + CodedOutputStream.ComputeStringSize(PersonName);
		}
		if (Id != 0)
		{
			num += 1 + CodedOutputStream.ComputeInt32Size(Id);
		}
		if (StartPrice != 0L)
		{
			num += 2 + CodedOutputStream.ComputeInt64Size(StartPrice);
		}
		if (_unknownFields != null)
		{
			num += _unknownFields.CalculateSize();
		}
		return num;
	}

	[DebuggerNonUserCode]
	public void MergeFrom(TreasureCurrentGoodsResp_1006021 other)
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
			if (other.GoodsId.Length != 0)
			{
				GoodsId = other.GoodsId;
			}
			if (other.GoodsName.Length != 0)
			{
				GoodsName = other.GoodsName;
			}
			if (other.FiveElement != 0)
			{
				FiveElement = other.FiveElement;
			}
			if (other.CurrentPerson.Length != 0)
			{
				CurrentPerson = other.CurrentPerson;
			}
			if (other.Stage != 0)
			{
				Stage = other.Stage;
			}
			if (other.Rank != 0)
			{
				Rank = other.Rank;
			}
			if (other.Level != 0)
			{
				Level = other.Level;
			}
			if (other.GoodsType != 0)
			{
				GoodsType = other.GoodsType;
			}
			if (other.CurrentPrice != 0L)
			{
				CurrentPrice = other.CurrentPrice;
			}
			if (other.PersonImage != 0)
			{
				PersonImage = other.PersonImage;
			}
			if (other.PersonSex)
			{
				PersonSex = other.PersonSex;
			}
			if (other.PersonName.Length != 0)
			{
				PersonName = other.PersonName;
			}
			if (other.Id != 0)
			{
				Id = other.Id;
			}
			if (other.StartPrice != 0L)
			{
				StartPrice = other.StartPrice;
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
			case 26u:
				GoodsId = input.ReadString();
				break;
			case 34u:
				GoodsName = input.ReadString();
				break;
			case 40u:
				FiveElement = input.ReadInt32();
				break;
			case 50u:
				CurrentPerson = input.ReadString();
				break;
			case 56u:
				Stage = input.ReadInt32();
				break;
			case 64u:
				Rank = input.ReadInt32();
				break;
			case 72u:
				Level = input.ReadInt32();
				break;
			case 80u:
				GoodsType = input.ReadInt32();
				break;
			case 88u:
				CurrentPrice = input.ReadInt64();
				break;
			case 96u:
				PersonImage = input.ReadInt32();
				break;
			case 104u:
				PersonSex = input.ReadBool();
				break;
			case 114u:
				PersonName = input.ReadString();
				break;
			case 120u:
				Id = input.ReadInt32();
				break;
			case 128u:
				StartPrice = input.ReadInt64();
				break;
			}
		}
	}
}
