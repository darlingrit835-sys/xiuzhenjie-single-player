using System;
using System.Diagnostics;
using Google.Protobuf;
using Google.Protobuf.Reflection;

public sealed class ChatJoinResp_4001001 : IMessage<ChatJoinResp_4001001>, IMessage, IEquatable<ChatJoinResp_4001001>, IDeepCloneable<ChatJoinResp_4001001>
{
	private static readonly MessageParser<ChatJoinResp_4001001> _parser = new MessageParser<ChatJoinResp_4001001>(() => new ChatJoinResp_4001001());

	private UnknownFieldSet _unknownFields;

	public const int CodeFieldNumber = 1;

	private int code_;

	public const int IsSuccessFieldNumber = 2;

	private bool isSuccess_;

	public const int CreditPointsFieldNumber = 6;

	private int creditPoints_;

	public const int TimeLagFieldNumber = 7;

	private int timeLag_;

	public const int NextSendTimeFieldNumber = 8;

	private long nextSendTime_;

	[DebuggerNonUserCode]
	public static MessageParser<ChatJoinResp_4001001> Parser => _parser;

	[DebuggerNonUserCode]
	public static MessageDescriptor Descriptor => ClientChatProtoReflection.Descriptor.MessageTypes[1];

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
	public int CreditPoints
	{
		get
		{
			return creditPoints_;
		}
		set
		{
			creditPoints_ = value;
		}
	}

	[DebuggerNonUserCode]
	public int TimeLag
	{
		get
		{
			return timeLag_;
		}
		set
		{
			timeLag_ = value;
		}
	}

	[DebuggerNonUserCode]
	public long NextSendTime
	{
		get
		{
			return nextSendTime_;
		}
		set
		{
			nextSendTime_ = value;
		}
	}

	private MessageDescriptor pb_003A_003AGoogle_002EProtobuf_002EIMessage_002Eget_Descriptor()
	{
		return Descriptor;
	}

	[DebuggerNonUserCode]
	public ChatJoinResp_4001001()
	{
	}

	[DebuggerNonUserCode]
	public ChatJoinResp_4001001(ChatJoinResp_4001001 other)
		: this()
	{
		code_ = other.code_;
		isSuccess_ = other.isSuccess_;
		creditPoints_ = other.creditPoints_;
		timeLag_ = other.timeLag_;
		nextSendTime_ = other.nextSendTime_;
		_unknownFields = UnknownFieldSet.Clone(other._unknownFields);
	}

	[DebuggerNonUserCode]
	public ChatJoinResp_4001001 Clone()
	{
		return new ChatJoinResp_4001001(this);
	}

	[DebuggerNonUserCode]
	public override bool Equals(object other)
	{
		return Equals(other as ChatJoinResp_4001001);
	}

	[DebuggerNonUserCode]
	public bool Equals(ChatJoinResp_4001001 other)
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
		if (CreditPoints != other.CreditPoints)
		{
			return false;
		}
		if (TimeLag != other.TimeLag)
		{
			return false;
		}
		if (NextSendTime != other.NextSendTime)
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
		if (CreditPoints != 0)
		{
			num ^= CreditPoints.GetHashCode();
		}
		if (TimeLag != 0)
		{
			num ^= TimeLag.GetHashCode();
		}
		if (NextSendTime != 0L)
		{
			num ^= NextSendTime.GetHashCode();
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
		if (CreditPoints != 0)
		{
			output.WriteRawTag(48);
			output.WriteInt32(CreditPoints);
		}
		if (TimeLag != 0)
		{
			output.WriteRawTag(56);
			output.WriteInt32(TimeLag);
		}
		if (NextSendTime != 0L)
		{
			output.WriteRawTag(64);
			output.WriteInt64(NextSendTime);
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
		if (CreditPoints != 0)
		{
			num += 1 + CodedOutputStream.ComputeInt32Size(CreditPoints);
		}
		if (TimeLag != 0)
		{
			num += 1 + CodedOutputStream.ComputeInt32Size(TimeLag);
		}
		if (NextSendTime != 0L)
		{
			num += 1 + CodedOutputStream.ComputeInt64Size(NextSendTime);
		}
		if (_unknownFields != null)
		{
			num += _unknownFields.CalculateSize();
		}
		return num;
	}

	[DebuggerNonUserCode]
	public void MergeFrom(ChatJoinResp_4001001 other)
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
			if (other.CreditPoints != 0)
			{
				CreditPoints = other.CreditPoints;
			}
			if (other.TimeLag != 0)
			{
				TimeLag = other.TimeLag;
			}
			if (other.NextSendTime != 0L)
			{
				NextSendTime = other.NextSendTime;
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
			case 48u:
				CreditPoints = input.ReadInt32();
				break;
			case 56u:
				TimeLag = input.ReadInt32();
				break;
			case 64u:
				NextSendTime = input.ReadInt64();
				break;
			}
		}
	}
}
