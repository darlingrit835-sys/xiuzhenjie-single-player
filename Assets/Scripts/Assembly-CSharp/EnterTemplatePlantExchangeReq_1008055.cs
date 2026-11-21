using System;
using System.Diagnostics;
using Google.Protobuf;
using Google.Protobuf.Reflection;

public sealed class EnterTemplatePlantExchangeReq_1008055 : IMessage<EnterTemplatePlantExchangeReq_1008055>, IMessage, IEquatable<EnterTemplatePlantExchangeReq_1008055>, IDeepCloneable<EnterTemplatePlantExchangeReq_1008055>
{
	private static readonly MessageParser<EnterTemplatePlantExchangeReq_1008055> _parser = new MessageParser<EnterTemplatePlantExchangeReq_1008055>(() => new EnterTemplatePlantExchangeReq_1008055());

	private UnknownFieldSet _unknownFields;

	public const int FiveElementFieldNumber = 1;

	private int fiveElement_;

	public const int RankFieldNumber = 2;

	private int rank_;

	public const int StageFieldNumber = 3;

	private int stage_;

	public const int TypeFieldNumber = 4;

	private int type_;

	public const int PageNumFieldNumber = 5;

	private int pageNum_;

	public const int PageSizeFieldNumber = 6;

	private int pageSize_;

	public const int SiQiFieldNumber = 7;

	private int siQi_;

	public const int WuWeiFieldNumber = 8;

	private int wuWei_;

	public const int BuXieFieldNumber = 9;

	private int buXie_;

	public const int GuiJingFieldNumber = 10;

	private int guiJing_;

	[DebuggerNonUserCode]
	public static MessageParser<EnterTemplatePlantExchangeReq_1008055> Parser => _parser;

	[DebuggerNonUserCode]
	public static MessageDescriptor Descriptor => UnrealWorldProtoReflection.Descriptor.MessageTypes[68];

	[DebuggerNonUserCode]
	MessageDescriptor IMessage.Descriptor => pb_003A_003AGoogle_002EProtobuf_002EIMessage_002Eget_Descriptor();

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
	public int Type
	{
		get
		{
			return type_;
		}
		set
		{
			type_ = value;
		}
	}

	[DebuggerNonUserCode]
	public int PageNum
	{
		get
		{
			return pageNum_;
		}
		set
		{
			pageNum_ = value;
		}
	}

	[DebuggerNonUserCode]
	public int PageSize
	{
		get
		{
			return pageSize_;
		}
		set
		{
			pageSize_ = value;
		}
	}

	[DebuggerNonUserCode]
	public int SiQi
	{
		get
		{
			return siQi_;
		}
		set
		{
			siQi_ = value;
		}
	}

	[DebuggerNonUserCode]
	public int WuWei
	{
		get
		{
			return wuWei_;
		}
		set
		{
			wuWei_ = value;
		}
	}

	[DebuggerNonUserCode]
	public int BuXie
	{
		get
		{
			return buXie_;
		}
		set
		{
			buXie_ = value;
		}
	}

	[DebuggerNonUserCode]
	public int GuiJing
	{
		get
		{
			return guiJing_;
		}
		set
		{
			guiJing_ = value;
		}
	}

	private MessageDescriptor pb_003A_003AGoogle_002EProtobuf_002EIMessage_002Eget_Descriptor()
	{
		return Descriptor;
	}

	[DebuggerNonUserCode]
	public EnterTemplatePlantExchangeReq_1008055()
	{
	}

	[DebuggerNonUserCode]
	public EnterTemplatePlantExchangeReq_1008055(EnterTemplatePlantExchangeReq_1008055 other)
		: this()
	{
		fiveElement_ = other.fiveElement_;
		rank_ = other.rank_;
		stage_ = other.stage_;
		type_ = other.type_;
		pageNum_ = other.pageNum_;
		pageSize_ = other.pageSize_;
		siQi_ = other.siQi_;
		wuWei_ = other.wuWei_;
		buXie_ = other.buXie_;
		guiJing_ = other.guiJing_;
		_unknownFields = UnknownFieldSet.Clone(other._unknownFields);
	}

	[DebuggerNonUserCode]
	public EnterTemplatePlantExchangeReq_1008055 Clone()
	{
		return new EnterTemplatePlantExchangeReq_1008055(this);
	}

	[DebuggerNonUserCode]
	public override bool Equals(object other)
	{
		return Equals(other as EnterTemplatePlantExchangeReq_1008055);
	}

	[DebuggerNonUserCode]
	public bool Equals(EnterTemplatePlantExchangeReq_1008055 other)
	{
		if (other == null)
		{
			return false;
		}
		if (other == this)
		{
			return true;
		}
		if (FiveElement != other.FiveElement)
		{
			return false;
		}
		if (Rank != other.Rank)
		{
			return false;
		}
		if (Stage != other.Stage)
		{
			return false;
		}
		if (Type != other.Type)
		{
			return false;
		}
		if (PageNum != other.PageNum)
		{
			return false;
		}
		if (PageSize != other.PageSize)
		{
			return false;
		}
		if (SiQi != other.SiQi)
		{
			return false;
		}
		if (WuWei != other.WuWei)
		{
			return false;
		}
		if (BuXie != other.BuXie)
		{
			return false;
		}
		if (GuiJing != other.GuiJing)
		{
			return false;
		}
		return object.Equals(_unknownFields, other._unknownFields);
	}

	[DebuggerNonUserCode]
	public override int GetHashCode()
	{
		int num = 1;
		if (FiveElement != 0)
		{
			num ^= FiveElement.GetHashCode();
		}
		if (Rank != 0)
		{
			num ^= Rank.GetHashCode();
		}
		if (Stage != 0)
		{
			num ^= Stage.GetHashCode();
		}
		if (Type != 0)
		{
			num ^= Type.GetHashCode();
		}
		if (PageNum != 0)
		{
			num ^= PageNum.GetHashCode();
		}
		if (PageSize != 0)
		{
			num ^= PageSize.GetHashCode();
		}
		if (SiQi != 0)
		{
			num ^= SiQi.GetHashCode();
		}
		if (WuWei != 0)
		{
			num ^= WuWei.GetHashCode();
		}
		if (BuXie != 0)
		{
			num ^= BuXie.GetHashCode();
		}
		if (GuiJing != 0)
		{
			num ^= GuiJing.GetHashCode();
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
		if (FiveElement != 0)
		{
			output.WriteRawTag(8);
			output.WriteInt32(FiveElement);
		}
		if (Rank != 0)
		{
			output.WriteRawTag(16);
			output.WriteInt32(Rank);
		}
		if (Stage != 0)
		{
			output.WriteRawTag(24);
			output.WriteInt32(Stage);
		}
		if (Type != 0)
		{
			output.WriteRawTag(32);
			output.WriteInt32(Type);
		}
		if (PageNum != 0)
		{
			output.WriteRawTag(40);
			output.WriteInt32(PageNum);
		}
		if (PageSize != 0)
		{
			output.WriteRawTag(48);
			output.WriteInt32(PageSize);
		}
		if (SiQi != 0)
		{
			output.WriteRawTag(56);
			output.WriteInt32(SiQi);
		}
		if (WuWei != 0)
		{
			output.WriteRawTag(64);
			output.WriteInt32(WuWei);
		}
		if (BuXie != 0)
		{
			output.WriteRawTag(72);
			output.WriteInt32(BuXie);
		}
		if (GuiJing != 0)
		{
			output.WriteRawTag(80);
			output.WriteInt32(GuiJing);
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
		if (FiveElement != 0)
		{
			num += 1 + CodedOutputStream.ComputeInt32Size(FiveElement);
		}
		if (Rank != 0)
		{
			num += 1 + CodedOutputStream.ComputeInt32Size(Rank);
		}
		if (Stage != 0)
		{
			num += 1 + CodedOutputStream.ComputeInt32Size(Stage);
		}
		if (Type != 0)
		{
			num += 1 + CodedOutputStream.ComputeInt32Size(Type);
		}
		if (PageNum != 0)
		{
			num += 1 + CodedOutputStream.ComputeInt32Size(PageNum);
		}
		if (PageSize != 0)
		{
			num += 1 + CodedOutputStream.ComputeInt32Size(PageSize);
		}
		if (SiQi != 0)
		{
			num += 1 + CodedOutputStream.ComputeInt32Size(SiQi);
		}
		if (WuWei != 0)
		{
			num += 1 + CodedOutputStream.ComputeInt32Size(WuWei);
		}
		if (BuXie != 0)
		{
			num += 1 + CodedOutputStream.ComputeInt32Size(BuXie);
		}
		if (GuiJing != 0)
		{
			num += 1 + CodedOutputStream.ComputeInt32Size(GuiJing);
		}
		if (_unknownFields != null)
		{
			num += _unknownFields.CalculateSize();
		}
		return num;
	}

	[DebuggerNonUserCode]
	public void MergeFrom(EnterTemplatePlantExchangeReq_1008055 other)
	{
		if (other != null)
		{
			if (other.FiveElement != 0)
			{
				FiveElement = other.FiveElement;
			}
			if (other.Rank != 0)
			{
				Rank = other.Rank;
			}
			if (other.Stage != 0)
			{
				Stage = other.Stage;
			}
			if (other.Type != 0)
			{
				Type = other.Type;
			}
			if (other.PageNum != 0)
			{
				PageNum = other.PageNum;
			}
			if (other.PageSize != 0)
			{
				PageSize = other.PageSize;
			}
			if (other.SiQi != 0)
			{
				SiQi = other.SiQi;
			}
			if (other.WuWei != 0)
			{
				WuWei = other.WuWei;
			}
			if (other.BuXie != 0)
			{
				BuXie = other.BuXie;
			}
			if (other.GuiJing != 0)
			{
				GuiJing = other.GuiJing;
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
				FiveElement = input.ReadInt32();
				break;
			case 16u:
				Rank = input.ReadInt32();
				break;
			case 24u:
				Stage = input.ReadInt32();
				break;
			case 32u:
				Type = input.ReadInt32();
				break;
			case 40u:
				PageNum = input.ReadInt32();
				break;
			case 48u:
				PageSize = input.ReadInt32();
				break;
			case 56u:
				SiQi = input.ReadInt32();
				break;
			case 64u:
				WuWei = input.ReadInt32();
				break;
			case 72u:
				BuXie = input.ReadInt32();
				break;
			case 80u:
				GuiJing = input.ReadInt32();
				break;
			}
		}
	}
}
