using System;
using System.Diagnostics;
using Google.Protobuf;
using Google.Protobuf.Reflection;

public sealed class BuyJadeCnyResp_1001017 : IMessage<BuyJadeCnyResp_1001017>, IMessage, IEquatable<BuyJadeCnyResp_1001017>, IDeepCloneable<BuyJadeCnyResp_1001017>
{
	private static readonly MessageParser<BuyJadeCnyResp_1001017> _parser = new MessageParser<BuyJadeCnyResp_1001017>(() => new BuyJadeCnyResp_1001017());

	private UnknownFieldSet _unknownFields;

	public const int CodeFieldNumber = 1;

	private int code_;

	public const int IsSuccessFieldNumber = 2;

	private bool isSuccess_;

	public const int CurrentJadeFieldNumber = 3;

	private int currentJade_;

	public const int LeiChongFieldNumber = 4;

	private int leiChong_;

	public const int VipLevelFieldNumber = 5;

	private int vipLevel_;

	[DebuggerNonUserCode]
	public static MessageParser<BuyJadeCnyResp_1001017> Parser => _parser;

	[DebuggerNonUserCode]
	public static MessageDescriptor Descriptor => SimpleActionProtoReflection.Descriptor.MessageTypes[19];

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
	public int CurrentJade
	{
		get
		{
			return currentJade_;
		}
		set
		{
			currentJade_ = value;
		}
	}

	[DebuggerNonUserCode]
	public int LeiChong
	{
		get
		{
			return leiChong_;
		}
		set
		{
			leiChong_ = value;
		}
	}

	[DebuggerNonUserCode]
	public int VipLevel
	{
		get
		{
			return vipLevel_;
		}
		set
		{
			vipLevel_ = value;
		}
	}

	private MessageDescriptor pb_003A_003AGoogle_002EProtobuf_002EIMessage_002Eget_Descriptor()
	{
		return Descriptor;
	}

	[DebuggerNonUserCode]
	public BuyJadeCnyResp_1001017()
	{
	}

	[DebuggerNonUserCode]
	public BuyJadeCnyResp_1001017(BuyJadeCnyResp_1001017 other)
		: this()
	{
		code_ = other.code_;
		isSuccess_ = other.isSuccess_;
		currentJade_ = other.currentJade_;
		leiChong_ = other.leiChong_;
		vipLevel_ = other.vipLevel_;
		_unknownFields = UnknownFieldSet.Clone(other._unknownFields);
	}

	[DebuggerNonUserCode]
	public BuyJadeCnyResp_1001017 Clone()
	{
		return new BuyJadeCnyResp_1001017(this);
	}

	[DebuggerNonUserCode]
	public override bool Equals(object other)
	{
		return Equals(other as BuyJadeCnyResp_1001017);
	}

	[DebuggerNonUserCode]
	public bool Equals(BuyJadeCnyResp_1001017 other)
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
		if (CurrentJade != other.CurrentJade)
		{
			return false;
		}
		if (LeiChong != other.LeiChong)
		{
			return false;
		}
		if (VipLevel != other.VipLevel)
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
		if (CurrentJade != 0)
		{
			num ^= CurrentJade.GetHashCode();
		}
		if (LeiChong != 0)
		{
			num ^= LeiChong.GetHashCode();
		}
		if (VipLevel != 0)
		{
			num ^= VipLevel.GetHashCode();
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
		if (CurrentJade != 0)
		{
			output.WriteRawTag(24);
			output.WriteInt32(CurrentJade);
		}
		if (LeiChong != 0)
		{
			output.WriteRawTag(32);
			output.WriteInt32(LeiChong);
		}
		if (VipLevel != 0)
		{
			output.WriteRawTag(40);
			output.WriteInt32(VipLevel);
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
		if (CurrentJade != 0)
		{
			num += 1 + CodedOutputStream.ComputeInt32Size(CurrentJade);
		}
		if (LeiChong != 0)
		{
			num += 1 + CodedOutputStream.ComputeInt32Size(LeiChong);
		}
		if (VipLevel != 0)
		{
			num += 1 + CodedOutputStream.ComputeInt32Size(VipLevel);
		}
		if (_unknownFields != null)
		{
			num += _unknownFields.CalculateSize();
		}
		return num;
	}

	[DebuggerNonUserCode]
	public void MergeFrom(BuyJadeCnyResp_1001017 other)
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
			if (other.CurrentJade != 0)
			{
				CurrentJade = other.CurrentJade;
			}
			if (other.LeiChong != 0)
			{
				LeiChong = other.LeiChong;
			}
			if (other.VipLevel != 0)
			{
				VipLevel = other.VipLevel;
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
				CurrentJade = input.ReadInt32();
				break;
			case 32u:
				LeiChong = input.ReadInt32();
				break;
			case 40u:
				VipLevel = input.ReadInt32();
				break;
			}
		}
	}
}
