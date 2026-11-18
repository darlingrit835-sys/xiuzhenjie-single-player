using System;
using System.Diagnostics;
using Google.Protobuf;
using Google.Protobuf.Reflection;

public sealed class EnterMethodEquipmentExchangeReq_1008067 : IMessage<EnterMethodEquipmentExchangeReq_1008067>, IMessage, IEquatable<EnterMethodEquipmentExchangeReq_1008067>, IDeepCloneable<EnterMethodEquipmentExchangeReq_1008067>
{
	private static readonly MessageParser<EnterMethodEquipmentExchangeReq_1008067> _parser = new MessageParser<EnterMethodEquipmentExchangeReq_1008067>(() => new EnterMethodEquipmentExchangeReq_1008067());

	private UnknownFieldSet _unknownFields;

	public const int FiveElementFieldNumber = 1;

	private int fiveElement_;

	public const int RankFieldNumber = 2;

	private int rank_;

	public const int StageFieldNumber = 3;

	private int stage_;

	public const int EffectFieldNumber = 4;

	private int effect_;

	public const int PageNumFieldNumber = 5;

	private int pageNum_;

	public const int PageSizeFieldNumber = 6;

	private int pageSize_;

	[DebuggerNonUserCode]
	public static MessageParser<EnterMethodEquipmentExchangeReq_1008067> Parser => _parser;

	[DebuggerNonUserCode]
	public static MessageDescriptor Descriptor => UnrealWorldProtoReflection.Descriptor.MessageTypes[79];

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
	public int Effect
	{
		get
		{
			return effect_;
		}
		set
		{
			effect_ = value;
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

	private MessageDescriptor pb_003A_003AGoogle_002EProtobuf_002EIMessage_002Eget_Descriptor()
	{
		return Descriptor;
	}

	[DebuggerNonUserCode]
	public EnterMethodEquipmentExchangeReq_1008067()
	{
	}

	[DebuggerNonUserCode]
	public EnterMethodEquipmentExchangeReq_1008067(EnterMethodEquipmentExchangeReq_1008067 other)
		: this()
	{
		fiveElement_ = other.fiveElement_;
		rank_ = other.rank_;
		stage_ = other.stage_;
		effect_ = other.effect_;
		pageNum_ = other.pageNum_;
		pageSize_ = other.pageSize_;
		_unknownFields = UnknownFieldSet.Clone(other._unknownFields);
	}

	[DebuggerNonUserCode]
	public EnterMethodEquipmentExchangeReq_1008067 Clone()
	{
		return new EnterMethodEquipmentExchangeReq_1008067(this);
	}

	[DebuggerNonUserCode]
	public override bool Equals(object other)
	{
		return Equals(other as EnterMethodEquipmentExchangeReq_1008067);
	}

	[DebuggerNonUserCode]
	public bool Equals(EnterMethodEquipmentExchangeReq_1008067 other)
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
		if (Effect != other.Effect)
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
		if (Effect != 0)
		{
			num ^= Effect.GetHashCode();
		}
		if (PageNum != 0)
		{
			num ^= PageNum.GetHashCode();
		}
		if (PageSize != 0)
		{
			num ^= PageSize.GetHashCode();
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
		if (Effect != 0)
		{
			output.WriteRawTag(32);
			output.WriteInt32(Effect);
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
		if (Effect != 0)
		{
			num += 1 + CodedOutputStream.ComputeInt32Size(Effect);
		}
		if (PageNum != 0)
		{
			num += 1 + CodedOutputStream.ComputeInt32Size(PageNum);
		}
		if (PageSize != 0)
		{
			num += 1 + CodedOutputStream.ComputeInt32Size(PageSize);
		}
		if (_unknownFields != null)
		{
			num += _unknownFields.CalculateSize();
		}
		return num;
	}

	[DebuggerNonUserCode]
	public void MergeFrom(EnterMethodEquipmentExchangeReq_1008067 other)
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
			if (other.Effect != 0)
			{
				Effect = other.Effect;
			}
			if (other.PageNum != 0)
			{
				PageNum = other.PageNum;
			}
			if (other.PageSize != 0)
			{
				PageSize = other.PageSize;
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
				Effect = input.ReadInt32();
				break;
			case 40u:
				PageNum = input.ReadInt32();
				break;
			case 48u:
				PageSize = input.ReadInt32();
				break;
			}
		}
	}
}
