using System;
using System.Diagnostics;
using Google.Protobuf;
using Google.Protobuf.Reflection;

public sealed class UpgradeYeLianSpaceJadeBuyTimeResp_1015070 : IMessage<UpgradeYeLianSpaceJadeBuyTimeResp_1015070>, IMessage, IEquatable<UpgradeYeLianSpaceJadeBuyTimeResp_1015070>, IDeepCloneable<UpgradeYeLianSpaceJadeBuyTimeResp_1015070>
{
	private static readonly MessageParser<UpgradeYeLianSpaceJadeBuyTimeResp_1015070> _parser = new MessageParser<UpgradeYeLianSpaceJadeBuyTimeResp_1015070>(() => new UpgradeYeLianSpaceJadeBuyTimeResp_1015070());

	private UnknownFieldSet _unknownFields;

	public const int CodeFieldNumber = 1;

	private int code_;

	public const int IsSuccessFieldNumber = 2;

	private bool isSuccess_;

	public const int ShengYuTimeFieldNumber = 3;

	private long shengYuTime_;

	[DebuggerNonUserCode]
	public static MessageParser<UpgradeYeLianSpaceJadeBuyTimeResp_1015070> Parser => _parser;

	[DebuggerNonUserCode]
	public static MessageDescriptor Descriptor => GuildProtoReflection.Descriptor.MessageTypes[82];

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
	public long ShengYuTime
	{
		get
		{
			return shengYuTime_;
		}
		set
		{
			shengYuTime_ = value;
		}
	}

	private MessageDescriptor pb_003A_003AGoogle_002EProtobuf_002EIMessage_002Eget_Descriptor()
	{
		return Descriptor;
	}

	[DebuggerNonUserCode]
	public UpgradeYeLianSpaceJadeBuyTimeResp_1015070()
	{
	}

	[DebuggerNonUserCode]
	public UpgradeYeLianSpaceJadeBuyTimeResp_1015070(UpgradeYeLianSpaceJadeBuyTimeResp_1015070 other)
		: this()
	{
		code_ = other.code_;
		isSuccess_ = other.isSuccess_;
		shengYuTime_ = other.shengYuTime_;
		_unknownFields = UnknownFieldSet.Clone(other._unknownFields);
	}

	[DebuggerNonUserCode]
	public UpgradeYeLianSpaceJadeBuyTimeResp_1015070 Clone()
	{
		return new UpgradeYeLianSpaceJadeBuyTimeResp_1015070(this);
	}

	[DebuggerNonUserCode]
	public override bool Equals(object other)
	{
		return Equals(other as UpgradeYeLianSpaceJadeBuyTimeResp_1015070);
	}

	[DebuggerNonUserCode]
	public bool Equals(UpgradeYeLianSpaceJadeBuyTimeResp_1015070 other)
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
		if (ShengYuTime != other.ShengYuTime)
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
		if (ShengYuTime != 0L)
		{
			num ^= ShengYuTime.GetHashCode();
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
		if (ShengYuTime != 0L)
		{
			output.WriteRawTag(24);
			output.WriteInt64(ShengYuTime);
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
		if (ShengYuTime != 0L)
		{
			num += 1 + CodedOutputStream.ComputeInt64Size(ShengYuTime);
		}
		if (_unknownFields != null)
		{
			num += _unknownFields.CalculateSize();
		}
		return num;
	}

	[DebuggerNonUserCode]
	public void MergeFrom(UpgradeYeLianSpaceJadeBuyTimeResp_1015070 other)
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
			if (other.ShengYuTime != 0L)
			{
				ShengYuTime = other.ShengYuTime;
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
				ShengYuTime = input.ReadInt64();
				break;
			}
		}
	}
}
