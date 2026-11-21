using System;
using System.Diagnostics;
using Google.Protobuf;
using Google.Protobuf.Reflection;

public sealed class EnterChouJiangChiResp_1017001 : IMessage<EnterChouJiangChiResp_1017001>, IMessage, IEquatable<EnterChouJiangChiResp_1017001>, IDeepCloneable<EnterChouJiangChiResp_1017001>
{
	private static readonly MessageParser<EnterChouJiangChiResp_1017001> _parser = new MessageParser<EnterChouJiangChiResp_1017001>(() => new EnterChouJiangChiResp_1017001());

	private UnknownFieldSet _unknownFields;

	public const int CodeFieldNumber = 1;

	private int code_;

	public const int IsSuccessFieldNumber = 2;

	private bool isSuccess_;

	public const int CurrentChouJiangJiFenFieldNumber = 3;

	private long currentChouJiangJiFen_;

	public const int CountFieldNumber = 4;

	private long count_;

	[DebuggerNonUserCode]
	public static MessageParser<EnterChouJiangChiResp_1017001> Parser => _parser;

	[DebuggerNonUserCode]
	public static MessageDescriptor Descriptor => HuoDongProtoReflection.Descriptor.MessageTypes[1];

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
	public long CurrentChouJiangJiFen
	{
		get
		{
			return currentChouJiangJiFen_;
		}
		set
		{
			currentChouJiangJiFen_ = value;
		}
	}

	[DebuggerNonUserCode]
	public long Count
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
	public EnterChouJiangChiResp_1017001()
	{
	}

	[DebuggerNonUserCode]
	public EnterChouJiangChiResp_1017001(EnterChouJiangChiResp_1017001 other)
		: this()
	{
		code_ = other.code_;
		isSuccess_ = other.isSuccess_;
		currentChouJiangJiFen_ = other.currentChouJiangJiFen_;
		count_ = other.count_;
		_unknownFields = UnknownFieldSet.Clone(other._unknownFields);
	}

	[DebuggerNonUserCode]
	public EnterChouJiangChiResp_1017001 Clone()
	{
		return new EnterChouJiangChiResp_1017001(this);
	}

	[DebuggerNonUserCode]
	public override bool Equals(object other)
	{
		return Equals(other as EnterChouJiangChiResp_1017001);
	}

	[DebuggerNonUserCode]
	public bool Equals(EnterChouJiangChiResp_1017001 other)
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
		if (CurrentChouJiangJiFen != other.CurrentChouJiangJiFen)
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
		if (Code != 0)
		{
			num ^= Code.GetHashCode();
		}
		if (IsSuccess)
		{
			num ^= IsSuccess.GetHashCode();
		}
		if (CurrentChouJiangJiFen != 0L)
		{
			num ^= CurrentChouJiangJiFen.GetHashCode();
		}
		if (Count != 0L)
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
		if (CurrentChouJiangJiFen != 0L)
		{
			output.WriteRawTag(24);
			output.WriteInt64(CurrentChouJiangJiFen);
		}
		if (Count != 0L)
		{
			output.WriteRawTag(32);
			output.WriteInt64(Count);
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
		if (CurrentChouJiangJiFen != 0L)
		{
			num += 1 + CodedOutputStream.ComputeInt64Size(CurrentChouJiangJiFen);
		}
		if (Count != 0L)
		{
			num += 1 + CodedOutputStream.ComputeInt64Size(Count);
		}
		if (_unknownFields != null)
		{
			num += _unknownFields.CalculateSize();
		}
		return num;
	}

	[DebuggerNonUserCode]
	public void MergeFrom(EnterChouJiangChiResp_1017001 other)
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
			if (other.CurrentChouJiangJiFen != 0L)
			{
				CurrentChouJiangJiFen = other.CurrentChouJiangJiFen;
			}
			if (other.Count != 0L)
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
				Code = input.ReadInt32();
				break;
			case 16u:
				IsSuccess = input.ReadBool();
				break;
			case 24u:
				CurrentChouJiangJiFen = input.ReadInt64();
				break;
			case 32u:
				Count = input.ReadInt64();
				break;
			}
		}
	}
}
