using System;
using System.Diagnostics;
using Google.Protobuf;
using Google.Protobuf.Reflection;

public sealed class SendMessageResp_4001003 : IMessage<SendMessageResp_4001003>, IMessage, IEquatable<SendMessageResp_4001003>, IDeepCloneable<SendMessageResp_4001003>
{
	private static readonly MessageParser<SendMessageResp_4001003> _parser = new MessageParser<SendMessageResp_4001003>(() => new SendMessageResp_4001003());

	private UnknownFieldSet _unknownFields;

	public const int CodeFieldNumber = 1;

	private int code_;

	public const int IsSuccessFieldNumber = 2;

	private bool isSuccess_;

	public const int ChatNumFieldNumber = 3;

	private int chatNum_;

	public const int NextSendTimeFieldNumber = 4;

	private long nextSendTime_;

	public const int CreditPointsFieldNumber = 5;

	private int creditPoints_;

	public const int TimeLagFieldNumber = 6;

	private int timeLag_;

	[DebuggerNonUserCode]
	public static MessageParser<SendMessageResp_4001003> Parser => _parser;

	[DebuggerNonUserCode]
	public static MessageDescriptor Descriptor => ClientChatProtoReflection.Descriptor.MessageTypes[4];

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
	public int ChatNum
	{
		get
		{
			return chatNum_;
		}
		set
		{
			chatNum_ = value;
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

	private MessageDescriptor pb_003A_003AGoogle_002EProtobuf_002EIMessage_002Eget_Descriptor()
	{
		return Descriptor;
	}

	[DebuggerNonUserCode]
	public SendMessageResp_4001003()
	{
	}

	[DebuggerNonUserCode]
	public SendMessageResp_4001003(SendMessageResp_4001003 other)
		: this()
	{
		code_ = other.code_;
		isSuccess_ = other.isSuccess_;
		chatNum_ = other.chatNum_;
		nextSendTime_ = other.nextSendTime_;
		creditPoints_ = other.creditPoints_;
		timeLag_ = other.timeLag_;
		_unknownFields = UnknownFieldSet.Clone(other._unknownFields);
	}

	[DebuggerNonUserCode]
	public SendMessageResp_4001003 Clone()
	{
		return new SendMessageResp_4001003(this);
	}

	[DebuggerNonUserCode]
	public override bool Equals(object other)
	{
		return Equals(other as SendMessageResp_4001003);
	}

	[DebuggerNonUserCode]
	public bool Equals(SendMessageResp_4001003 other)
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
		if (ChatNum != other.ChatNum)
		{
			return false;
		}
		if (NextSendTime != other.NextSendTime)
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
		if (ChatNum != 0)
		{
			num ^= ChatNum.GetHashCode();
		}
		if (NextSendTime != 0L)
		{
			num ^= NextSendTime.GetHashCode();
		}
		if (CreditPoints != 0)
		{
			num ^= CreditPoints.GetHashCode();
		}
		if (TimeLag != 0)
		{
			num ^= TimeLag.GetHashCode();
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
		if (ChatNum != 0)
		{
			output.WriteRawTag(24);
			output.WriteInt32(ChatNum);
		}
		if (NextSendTime != 0L)
		{
			output.WriteRawTag(32);
			output.WriteInt64(NextSendTime);
		}
		if (CreditPoints != 0)
		{
			output.WriteRawTag(40);
			output.WriteInt32(CreditPoints);
		}
		if (TimeLag != 0)
		{
			output.WriteRawTag(48);
			output.WriteInt32(TimeLag);
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
		if (ChatNum != 0)
		{
			num += 1 + CodedOutputStream.ComputeInt32Size(ChatNum);
		}
		if (NextSendTime != 0L)
		{
			num += 1 + CodedOutputStream.ComputeInt64Size(NextSendTime);
		}
		if (CreditPoints != 0)
		{
			num += 1 + CodedOutputStream.ComputeInt32Size(CreditPoints);
		}
		if (TimeLag != 0)
		{
			num += 1 + CodedOutputStream.ComputeInt32Size(TimeLag);
		}
		if (_unknownFields != null)
		{
			num += _unknownFields.CalculateSize();
		}
		return num;
	}

	[DebuggerNonUserCode]
	public void MergeFrom(SendMessageResp_4001003 other)
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
			if (other.ChatNum != 0)
			{
				ChatNum = other.ChatNum;
			}
			if (other.NextSendTime != 0L)
			{
				NextSendTime = other.NextSendTime;
			}
			if (other.CreditPoints != 0)
			{
				CreditPoints = other.CreditPoints;
			}
			if (other.TimeLag != 0)
			{
				TimeLag = other.TimeLag;
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
				ChatNum = input.ReadInt32();
				break;
			case 32u:
				NextSendTime = input.ReadInt64();
				break;
			case 40u:
				CreditPoints = input.ReadInt32();
				break;
			case 48u:
				TimeLag = input.ReadInt32();
				break;
			}
		}
	}
}
