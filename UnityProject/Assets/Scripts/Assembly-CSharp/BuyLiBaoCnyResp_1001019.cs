using System;
using System.Diagnostics;
using Google.Protobuf;
using Google.Protobuf.Reflection;

public sealed class BuyLiBaoCnyResp_1001019 : IMessage<BuyLiBaoCnyResp_1001019>, IMessage, IEquatable<BuyLiBaoCnyResp_1001019>, IDeepCloneable<BuyLiBaoCnyResp_1001019>
{
	private static readonly MessageParser<BuyLiBaoCnyResp_1001019> _parser = new MessageParser<BuyLiBaoCnyResp_1001019>(() => new BuyLiBaoCnyResp_1001019());

	private UnknownFieldSet _unknownFields;

	public const int CodeFieldNumber = 1;

	private int code_;

	public const int IsSuccessFieldNumber = 2;

	private bool isSuccess_;

	public const int TypeFieldNumber = 3;

	private int type_;

	public const int LeiChongFieldNumber = 4;

	private int leiChong_;

	public const int VipLevelFieldNumber = 5;

	private int vipLevel_;

	[DebuggerNonUserCode]
	public static MessageParser<BuyLiBaoCnyResp_1001019> Parser => _parser;

	[DebuggerNonUserCode]
	public static MessageDescriptor Descriptor => SimpleActionProtoReflection.Descriptor.MessageTypes[22];

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
	public int Type
	{
		get
		{
			return type_;
		}
		set
		{
			type_ = value;
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
	public BuyLiBaoCnyResp_1001019()
	{
	}

	[DebuggerNonUserCode]
	public BuyLiBaoCnyResp_1001019(BuyLiBaoCnyResp_1001019 other)
		: this()
	{
		code_ = other.code_;
		isSuccess_ = other.isSuccess_;
		type_ = other.type_;
		leiChong_ = other.leiChong_;
		vipLevel_ = other.vipLevel_;
		_unknownFields = UnknownFieldSet.Clone(other._unknownFields);
	}

	[DebuggerNonUserCode]
	public BuyLiBaoCnyResp_1001019 Clone()
	{
		return new BuyLiBaoCnyResp_1001019(this);
	}

	[DebuggerNonUserCode]
	public override bool Equals(object other)
	{
		return Equals(other as BuyLiBaoCnyResp_1001019);
	}

	[DebuggerNonUserCode]
	public bool Equals(BuyLiBaoCnyResp_1001019 other)
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
		if (Type != other.Type)
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
		if (Type != 0)
		{
			num ^= Type.GetHashCode();
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
		if (Type != 0)
		{
			output.WriteRawTag(24);
			output.WriteInt32(Type);
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
		if (Type != 0)
		{
			num += 1 + CodedOutputStream.ComputeInt32Size(Type);
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
	public void MergeFrom(BuyLiBaoCnyResp_1001019 other)
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
			if (other.Type != 0)
			{
				Type = other.Type;
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
				Type = input.ReadInt32();
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
