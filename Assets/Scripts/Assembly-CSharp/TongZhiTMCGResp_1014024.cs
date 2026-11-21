using System;
using System.Diagnostics;
using Google.Protobuf;
using Google.Protobuf.Reflection;

public sealed class TongZhiTMCGResp_1014024 : IMessage<TongZhiTMCGResp_1014024>, IMessage, IEquatable<TongZhiTMCGResp_1014024>, IDeepCloneable<TongZhiTMCGResp_1014024>
{
	private static readonly MessageParser<TongZhiTMCGResp_1014024> _parser = new MessageParser<TongZhiTMCGResp_1014024>(() => new TongZhiTMCGResp_1014024());

	private UnknownFieldSet _unknownFields;

	public const int CodeFieldNumber = 1;

	private int code_;

	public const int IsSuccessFieldNumber = 2;

	private bool isSuccess_;

	public const int TmcgTypeFieldNumber = 3;

	private int tmcgType_;

	public const int TmcgNumFieldNumber = 4;

	private int tmcgNum_;

	[DebuggerNonUserCode]
	public static MessageParser<TongZhiTMCGResp_1014024> Parser => _parser;

	[DebuggerNonUserCode]
	public static MessageDescriptor Descriptor => FriendProtoReflection.Descriptor.MessageTypes[28];

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
	public int TmcgType
	{
		get
		{
			return tmcgType_;
		}
		set
		{
			tmcgType_ = value;
		}
	}

	[DebuggerNonUserCode]
	public int TmcgNum
	{
		get
		{
			return tmcgNum_;
		}
		set
		{
			tmcgNum_ = value;
		}
	}

	private MessageDescriptor pb_003A_003AGoogle_002EProtobuf_002EIMessage_002Eget_Descriptor()
	{
		return Descriptor;
	}

	[DebuggerNonUserCode]
	public TongZhiTMCGResp_1014024()
	{
	}

	[DebuggerNonUserCode]
	public TongZhiTMCGResp_1014024(TongZhiTMCGResp_1014024 other)
		: this()
	{
		code_ = other.code_;
		isSuccess_ = other.isSuccess_;
		tmcgType_ = other.tmcgType_;
		tmcgNum_ = other.tmcgNum_;
		_unknownFields = UnknownFieldSet.Clone(other._unknownFields);
	}

	[DebuggerNonUserCode]
	public TongZhiTMCGResp_1014024 Clone()
	{
		return new TongZhiTMCGResp_1014024(this);
	}

	[DebuggerNonUserCode]
	public override bool Equals(object other)
	{
		return Equals(other as TongZhiTMCGResp_1014024);
	}

	[DebuggerNonUserCode]
	public bool Equals(TongZhiTMCGResp_1014024 other)
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
		if (TmcgType != other.TmcgType)
		{
			return false;
		}
		if (TmcgNum != other.TmcgNum)
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
		if (TmcgType != 0)
		{
			num ^= TmcgType.GetHashCode();
		}
		if (TmcgNum != 0)
		{
			num ^= TmcgNum.GetHashCode();
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
		if (TmcgType != 0)
		{
			output.WriteRawTag(24);
			output.WriteInt32(TmcgType);
		}
		if (TmcgNum != 0)
		{
			output.WriteRawTag(32);
			output.WriteInt32(TmcgNum);
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
		if (TmcgType != 0)
		{
			num += 1 + CodedOutputStream.ComputeInt32Size(TmcgType);
		}
		if (TmcgNum != 0)
		{
			num += 1 + CodedOutputStream.ComputeInt32Size(TmcgNum);
		}
		if (_unknownFields != null)
		{
			num += _unknownFields.CalculateSize();
		}
		return num;
	}

	[DebuggerNonUserCode]
	public void MergeFrom(TongZhiTMCGResp_1014024 other)
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
			if (other.TmcgType != 0)
			{
				TmcgType = other.TmcgType;
			}
			if (other.TmcgNum != 0)
			{
				TmcgNum = other.TmcgNum;
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
				TmcgType = input.ReadInt32();
				break;
			case 32u:
				TmcgNum = input.ReadInt32();
				break;
			}
		}
	}
}
