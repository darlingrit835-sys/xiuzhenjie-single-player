using System;
using System.Diagnostics;
using Google.Protobuf;
using Google.Protobuf.Reflection;

public sealed class GoodsSellResp_1004022 : IMessage<GoodsSellResp_1004022>, IMessage, IEquatable<GoodsSellResp_1004022>, IDeepCloneable<GoodsSellResp_1004022>
{
	private static readonly MessageParser<GoodsSellResp_1004022> _parser = new MessageParser<GoodsSellResp_1004022>(() => new GoodsSellResp_1004022());

	private UnknownFieldSet _unknownFields;

	public const int CodeFieldNumber = 1;

	private int code_;

	public const int IsSuccessFieldNumber = 2;

	private bool isSuccess_;

	public const int ShinyStoneFieldNumber = 3;

	private long shinyStone_;

	public const int NowCapacityFieldNumber = 4;

	private int nowCapacity_;

	public const int SuccessCountFieldNumber = 5;

	private int successCount_;

	public const int FailedCountFieldNumber = 6;

	private int failedCount_;

	public const int AddShinyStoneFieldNumber = 7;

	private int addShinyStone_;

	public const int FailGoodsIdsFieldNumber = 8;

	private string failGoodsIds_ = "";

	[DebuggerNonUserCode]
	public static MessageParser<GoodsSellResp_1004022> Parser => _parser;

	[DebuggerNonUserCode]
	public static MessageDescriptor Descriptor => GoodsProtoReflection.Descriptor.MessageTypes[39];

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
	public long ShinyStone
	{
		get
		{
			return shinyStone_;
		}
		set
		{
			shinyStone_ = value;
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
	public int SuccessCount
	{
		get
		{
			return successCount_;
		}
		set
		{
			successCount_ = value;
		}
	}

	[DebuggerNonUserCode]
	public int FailedCount
	{
		get
		{
			return failedCount_;
		}
		set
		{
			failedCount_ = value;
		}
	}

	[DebuggerNonUserCode]
	public int AddShinyStone
	{
		get
		{
			return addShinyStone_;
		}
		set
		{
			addShinyStone_ = value;
		}
	}

	[DebuggerNonUserCode]
	public string FailGoodsIds
	{
		get
		{
			return failGoodsIds_;
		}
		set
		{
			failGoodsIds_ = ProtoPreconditions.CheckNotNull(value, "value");
		}
	}

	private MessageDescriptor pb_003A_003AGoogle_002EProtobuf_002EIMessage_002Eget_Descriptor()
	{
		return Descriptor;
	}

	[DebuggerNonUserCode]
	public GoodsSellResp_1004022()
	{
	}

	[DebuggerNonUserCode]
	public GoodsSellResp_1004022(GoodsSellResp_1004022 other)
		: this()
	{
		code_ = other.code_;
		isSuccess_ = other.isSuccess_;
		shinyStone_ = other.shinyStone_;
		nowCapacity_ = other.nowCapacity_;
		successCount_ = other.successCount_;
		failedCount_ = other.failedCount_;
		addShinyStone_ = other.addShinyStone_;
		failGoodsIds_ = other.failGoodsIds_;
		_unknownFields = UnknownFieldSet.Clone(other._unknownFields);
	}

	[DebuggerNonUserCode]
	public GoodsSellResp_1004022 Clone()
	{
		return new GoodsSellResp_1004022(this);
	}

	[DebuggerNonUserCode]
	public override bool Equals(object other)
	{
		return Equals(other as GoodsSellResp_1004022);
	}

	[DebuggerNonUserCode]
	public bool Equals(GoodsSellResp_1004022 other)
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
		if (ShinyStone != other.ShinyStone)
		{
			return false;
		}
		if (NowCapacity != other.NowCapacity)
		{
			return false;
		}
		if (SuccessCount != other.SuccessCount)
		{
			return false;
		}
		if (FailedCount != other.FailedCount)
		{
			return false;
		}
		if (AddShinyStone != other.AddShinyStone)
		{
			return false;
		}
		if (FailGoodsIds != other.FailGoodsIds)
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
		if (ShinyStone != 0L)
		{
			num ^= ShinyStone.GetHashCode();
		}
		if (NowCapacity != 0)
		{
			num ^= NowCapacity.GetHashCode();
		}
		if (SuccessCount != 0)
		{
			num ^= SuccessCount.GetHashCode();
		}
		if (FailedCount != 0)
		{
			num ^= FailedCount.GetHashCode();
		}
		if (AddShinyStone != 0)
		{
			num ^= AddShinyStone.GetHashCode();
		}
		if (FailGoodsIds.Length != 0)
		{
			num ^= FailGoodsIds.GetHashCode();
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
		if (ShinyStone != 0L)
		{
			output.WriteRawTag(24);
			output.WriteInt64(ShinyStone);
		}
		if (NowCapacity != 0)
		{
			output.WriteRawTag(32);
			output.WriteInt32(NowCapacity);
		}
		if (SuccessCount != 0)
		{
			output.WriteRawTag(40);
			output.WriteInt32(SuccessCount);
		}
		if (FailedCount != 0)
		{
			output.WriteRawTag(48);
			output.WriteInt32(FailedCount);
		}
		if (AddShinyStone != 0)
		{
			output.WriteRawTag(56);
			output.WriteInt32(AddShinyStone);
		}
		if (FailGoodsIds.Length != 0)
		{
			output.WriteRawTag(66);
			output.WriteString(FailGoodsIds);
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
		if (ShinyStone != 0L)
		{
			num += 1 + CodedOutputStream.ComputeInt64Size(ShinyStone);
		}
		if (NowCapacity != 0)
		{
			num += 1 + CodedOutputStream.ComputeInt32Size(NowCapacity);
		}
		if (SuccessCount != 0)
		{
			num += 1 + CodedOutputStream.ComputeInt32Size(SuccessCount);
		}
		if (FailedCount != 0)
		{
			num += 1 + CodedOutputStream.ComputeInt32Size(FailedCount);
		}
		if (AddShinyStone != 0)
		{
			num += 1 + CodedOutputStream.ComputeInt32Size(AddShinyStone);
		}
		if (FailGoodsIds.Length != 0)
		{
			num += 1 + CodedOutputStream.ComputeStringSize(FailGoodsIds);
		}
		if (_unknownFields != null)
		{
			num += _unknownFields.CalculateSize();
		}
		return num;
	}

	[DebuggerNonUserCode]
	public void MergeFrom(GoodsSellResp_1004022 other)
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
			if (other.ShinyStone != 0L)
			{
				ShinyStone = other.ShinyStone;
			}
			if (other.NowCapacity != 0)
			{
				NowCapacity = other.NowCapacity;
			}
			if (other.SuccessCount != 0)
			{
				SuccessCount = other.SuccessCount;
			}
			if (other.FailedCount != 0)
			{
				FailedCount = other.FailedCount;
			}
			if (other.AddShinyStone != 0)
			{
				AddShinyStone = other.AddShinyStone;
			}
			if (other.FailGoodsIds.Length != 0)
			{
				FailGoodsIds = other.FailGoodsIds;
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
				ShinyStone = input.ReadInt64();
				break;
			case 32u:
				NowCapacity = input.ReadInt32();
				break;
			case 40u:
				SuccessCount = input.ReadInt32();
				break;
			case 48u:
				FailedCount = input.ReadInt32();
				break;
			case 56u:
				AddShinyStone = input.ReadInt32();
				break;
			case 66u:
				FailGoodsIds = input.ReadString();
				break;
			}
		}
	}
}
