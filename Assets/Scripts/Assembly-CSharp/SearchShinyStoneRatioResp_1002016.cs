using System;
using System.Diagnostics;
using Google.Protobuf;
using Google.Protobuf.Reflection;

public sealed class SearchShinyStoneRatioResp_1002016 : IMessage<SearchShinyStoneRatioResp_1002016>, IMessage, IEquatable<SearchShinyStoneRatioResp_1002016>, IDeepCloneable<SearchShinyStoneRatioResp_1002016>
{
	private static readonly MessageParser<SearchShinyStoneRatioResp_1002016> _parser = new MessageParser<SearchShinyStoneRatioResp_1002016>(() => new SearchShinyStoneRatioResp_1002016());

	private UnknownFieldSet _unknownFields;

	public const int CodeFieldNumber = 1;

	private int code_;

	public const int IsSuccessFieldNumber = 2;

	private bool isSuccess_;

	public const int ExchangeRatioFieldNumber = 3;

	private long exchangeRatio_;

	public const int ExchangeShinyStoneSumFieldNumber = 4;

	private long exchangeShinyStoneSum_;

	public const int UpdateAtFieldNumber = 5;

	private long updateAt_;

	[DebuggerNonUserCode]
	public static MessageParser<SearchShinyStoneRatioResp_1002016> Parser => _parser;

	[DebuggerNonUserCode]
	public static MessageDescriptor Descriptor => RoleProtoReflection.Descriptor.MessageTypes[19];

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
	public long ExchangeRatio
	{
		get
		{
			return exchangeRatio_;
		}
		set
		{
			exchangeRatio_ = value;
		}
	}

	[DebuggerNonUserCode]
	public long ExchangeShinyStoneSum
	{
		get
		{
			return exchangeShinyStoneSum_;
		}
		set
		{
			exchangeShinyStoneSum_ = value;
		}
	}

	[DebuggerNonUserCode]
	public long UpdateAt
	{
		get
		{
			return updateAt_;
		}
		set
		{
			updateAt_ = value;
		}
	}

	private MessageDescriptor pb_003A_003AGoogle_002EProtobuf_002EIMessage_002Eget_Descriptor()
	{
		return Descriptor;
	}

	[DebuggerNonUserCode]
	public SearchShinyStoneRatioResp_1002016()
	{
	}

	[DebuggerNonUserCode]
	public SearchShinyStoneRatioResp_1002016(SearchShinyStoneRatioResp_1002016 other)
		: this()
	{
		code_ = other.code_;
		isSuccess_ = other.isSuccess_;
		exchangeRatio_ = other.exchangeRatio_;
		exchangeShinyStoneSum_ = other.exchangeShinyStoneSum_;
		updateAt_ = other.updateAt_;
		_unknownFields = UnknownFieldSet.Clone(other._unknownFields);
	}

	[DebuggerNonUserCode]
	public SearchShinyStoneRatioResp_1002016 Clone()
	{
		return new SearchShinyStoneRatioResp_1002016(this);
	}

	[DebuggerNonUserCode]
	public override bool Equals(object other)
	{
		return Equals(other as SearchShinyStoneRatioResp_1002016);
	}

	[DebuggerNonUserCode]
	public bool Equals(SearchShinyStoneRatioResp_1002016 other)
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
		if (ExchangeRatio != other.ExchangeRatio)
		{
			return false;
		}
		if (ExchangeShinyStoneSum != other.ExchangeShinyStoneSum)
		{
			return false;
		}
		if (UpdateAt != other.UpdateAt)
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
		if (ExchangeRatio != 0L)
		{
			num ^= ExchangeRatio.GetHashCode();
		}
		if (ExchangeShinyStoneSum != 0L)
		{
			num ^= ExchangeShinyStoneSum.GetHashCode();
		}
		if (UpdateAt != 0L)
		{
			num ^= UpdateAt.GetHashCode();
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
		if (ExchangeRatio != 0L)
		{
			output.WriteRawTag(24);
			output.WriteInt64(ExchangeRatio);
		}
		if (ExchangeShinyStoneSum != 0L)
		{
			output.WriteRawTag(32);
			output.WriteInt64(ExchangeShinyStoneSum);
		}
		if (UpdateAt != 0L)
		{
			output.WriteRawTag(40);
			output.WriteInt64(UpdateAt);
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
		if (ExchangeRatio != 0L)
		{
			num += 1 + CodedOutputStream.ComputeInt64Size(ExchangeRatio);
		}
		if (ExchangeShinyStoneSum != 0L)
		{
			num += 1 + CodedOutputStream.ComputeInt64Size(ExchangeShinyStoneSum);
		}
		if (UpdateAt != 0L)
		{
			num += 1 + CodedOutputStream.ComputeInt64Size(UpdateAt);
		}
		if (_unknownFields != null)
		{
			num += _unknownFields.CalculateSize();
		}
		return num;
	}

	[DebuggerNonUserCode]
	public void MergeFrom(SearchShinyStoneRatioResp_1002016 other)
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
			if (other.ExchangeRatio != 0L)
			{
				ExchangeRatio = other.ExchangeRatio;
			}
			if (other.ExchangeShinyStoneSum != 0L)
			{
				ExchangeShinyStoneSum = other.ExchangeShinyStoneSum;
			}
			if (other.UpdateAt != 0L)
			{
				UpdateAt = other.UpdateAt;
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
				ExchangeRatio = input.ReadInt64();
				break;
			case 32u:
				ExchangeShinyStoneSum = input.ReadInt64();
				break;
			case 40u:
				UpdateAt = input.ReadInt64();
				break;
			}
		}
	}
}
