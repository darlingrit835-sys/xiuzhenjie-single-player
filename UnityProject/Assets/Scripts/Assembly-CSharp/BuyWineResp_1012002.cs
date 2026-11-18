using System;
using System.Diagnostics;
using Google.Protobuf;
using Google.Protobuf.Reflection;

public sealed class BuyWineResp_1012002 : IMessage<BuyWineResp_1012002>, IMessage, IEquatable<BuyWineResp_1012002>, IDeepCloneable<BuyWineResp_1012002>
{
	private static readonly MessageParser<BuyWineResp_1012002> _parser = new MessageParser<BuyWineResp_1012002>(() => new BuyWineResp_1012002());

	private UnknownFieldSet _unknownFields;

	public const int CodeFieldNumber = 1;

	private int code_;

	public const int IsSuccessFieldNumber = 2;

	private bool isSuccess_;

	public const int ChatNumFieldNumber = 3;

	private int chatNum_;

	public const int ShinyStoneFieldNumber = 4;

	private long shinyStone_;

	public const int ShinyJadeFieldNumber = 5;

	private long shinyJade_;

	[DebuggerNonUserCode]
	public static MessageParser<BuyWineResp_1012002> Parser => _parser;

	[DebuggerNonUserCode]
	public static MessageDescriptor Descriptor => ChatProtoReflection.Descriptor.MessageTypes[2];

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
	public long ShinyJade
	{
		get
		{
			return shinyJade_;
		}
		set
		{
			shinyJade_ = value;
		}
	}

	private MessageDescriptor pb_003A_003AGoogle_002EProtobuf_002EIMessage_002Eget_Descriptor()
	{
		return Descriptor;
	}

	[DebuggerNonUserCode]
	public BuyWineResp_1012002()
	{
	}

	[DebuggerNonUserCode]
	public BuyWineResp_1012002(BuyWineResp_1012002 other)
		: this()
	{
		code_ = other.code_;
		isSuccess_ = other.isSuccess_;
		chatNum_ = other.chatNum_;
		shinyStone_ = other.shinyStone_;
		shinyJade_ = other.shinyJade_;
		_unknownFields = UnknownFieldSet.Clone(other._unknownFields);
	}

	[DebuggerNonUserCode]
	public BuyWineResp_1012002 Clone()
	{
		return new BuyWineResp_1012002(this);
	}

	[DebuggerNonUserCode]
	public override bool Equals(object other)
	{
		return Equals(other as BuyWineResp_1012002);
	}

	[DebuggerNonUserCode]
	public bool Equals(BuyWineResp_1012002 other)
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
		if (ShinyStone != other.ShinyStone)
		{
			return false;
		}
		if (ShinyJade != other.ShinyJade)
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
		if (ShinyStone != 0L)
		{
			num ^= ShinyStone.GetHashCode();
		}
		if (ShinyJade != 0L)
		{
			num ^= ShinyJade.GetHashCode();
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
		if (ShinyStone != 0L)
		{
			output.WriteRawTag(32);
			output.WriteInt64(ShinyStone);
		}
		if (ShinyJade != 0L)
		{
			output.WriteRawTag(40);
			output.WriteInt64(ShinyJade);
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
		if (ShinyStone != 0L)
		{
			num += 1 + CodedOutputStream.ComputeInt64Size(ShinyStone);
		}
		if (ShinyJade != 0L)
		{
			num += 1 + CodedOutputStream.ComputeInt64Size(ShinyJade);
		}
		if (_unknownFields != null)
		{
			num += _unknownFields.CalculateSize();
		}
		return num;
	}

	[DebuggerNonUserCode]
	public void MergeFrom(BuyWineResp_1012002 other)
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
			if (other.ShinyStone != 0L)
			{
				ShinyStone = other.ShinyStone;
			}
			if (other.ShinyJade != 0L)
			{
				ShinyJade = other.ShinyJade;
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
				ShinyStone = input.ReadInt64();
				break;
			case 40u:
				ShinyJade = input.ReadInt64();
				break;
			}
		}
	}
}
