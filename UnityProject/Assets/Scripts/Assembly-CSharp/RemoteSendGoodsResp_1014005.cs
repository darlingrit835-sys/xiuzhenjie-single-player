using System;
using System.Diagnostics;
using Google.Protobuf;
using Google.Protobuf.Reflection;

public sealed class RemoteSendGoodsResp_1014005 : IMessage<RemoteSendGoodsResp_1014005>, IMessage, IEquatable<RemoteSendGoodsResp_1014005>, IDeepCloneable<RemoteSendGoodsResp_1014005>
{
	private static readonly MessageParser<RemoteSendGoodsResp_1014005> _parser = new MessageParser<RemoteSendGoodsResp_1014005>(() => new RemoteSendGoodsResp_1014005());

	private UnknownFieldSet _unknownFields;

	public const int CodeFieldNumber = 1;

	private int code_;

	public const int IsSuccessFieldNumber = 2;

	private bool isSuccess_;

	public const int CurrentShinyStoneFieldNumber = 3;

	private long currentShinyStone_;

	[DebuggerNonUserCode]
	public static MessageParser<RemoteSendGoodsResp_1014005> Parser => _parser;

	[DebuggerNonUserCode]
	public static MessageDescriptor Descriptor => FriendProtoReflection.Descriptor.MessageTypes[3];

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
	public long CurrentShinyStone
	{
		get
		{
			return currentShinyStone_;
		}
		set
		{
			currentShinyStone_ = value;
		}
	}

	private MessageDescriptor pb_003A_003AGoogle_002EProtobuf_002EIMessage_002Eget_Descriptor()
	{
		return Descriptor;
	}

	[DebuggerNonUserCode]
	public RemoteSendGoodsResp_1014005()
	{
	}

	[DebuggerNonUserCode]
	public RemoteSendGoodsResp_1014005(RemoteSendGoodsResp_1014005 other)
		: this()
	{
		code_ = other.code_;
		isSuccess_ = other.isSuccess_;
		currentShinyStone_ = other.currentShinyStone_;
		_unknownFields = UnknownFieldSet.Clone(other._unknownFields);
	}

	[DebuggerNonUserCode]
	public RemoteSendGoodsResp_1014005 Clone()
	{
		return new RemoteSendGoodsResp_1014005(this);
	}

	[DebuggerNonUserCode]
	public override bool Equals(object other)
	{
		return Equals(other as RemoteSendGoodsResp_1014005);
	}

	[DebuggerNonUserCode]
	public bool Equals(RemoteSendGoodsResp_1014005 other)
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
		if (CurrentShinyStone != other.CurrentShinyStone)
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
		if (CurrentShinyStone != 0L)
		{
			num ^= CurrentShinyStone.GetHashCode();
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
		if (CurrentShinyStone != 0L)
		{
			output.WriteRawTag(24);
			output.WriteInt64(CurrentShinyStone);
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
		if (CurrentShinyStone != 0L)
		{
			num += 1 + CodedOutputStream.ComputeInt64Size(CurrentShinyStone);
		}
		if (_unknownFields != null)
		{
			num += _unknownFields.CalculateSize();
		}
		return num;
	}

	[DebuggerNonUserCode]
	public void MergeFrom(RemoteSendGoodsResp_1014005 other)
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
			if (other.CurrentShinyStone != 0L)
			{
				CurrentShinyStone = other.CurrentShinyStone;
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
				CurrentShinyStone = input.ReadInt64();
				break;
			}
		}
	}
}
