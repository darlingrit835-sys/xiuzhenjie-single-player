using System;
using System.Diagnostics;
using Google.Protobuf;
using Google.Protobuf.Reflection;

public sealed class NeiMenDiZiNumResp_1015132 : IMessage<NeiMenDiZiNumResp_1015132>, IMessage, IEquatable<NeiMenDiZiNumResp_1015132>, IDeepCloneable<NeiMenDiZiNumResp_1015132>
{
	private static readonly MessageParser<NeiMenDiZiNumResp_1015132> _parser = new MessageParser<NeiMenDiZiNumResp_1015132>(() => new NeiMenDiZiNumResp_1015132());

	private UnknownFieldSet _unknownFields;

	public const int CodeFieldNumber = 1;

	private int code_;

	public const int IsSuccessFieldNumber = 2;

	private bool isSuccess_;

	public const int PingJunZhanLiFieldNumber = 3;

	private int pingJunZhanLi_;

	public const int KeYongDiZiNumFieldNumber = 4;

	private int keYongDiZiNum_;

	[DebuggerNonUserCode]
	public static MessageParser<NeiMenDiZiNumResp_1015132> Parser => _parser;

	[DebuggerNonUserCode]
	public static MessageDescriptor Descriptor => GuildProtoReflection.Descriptor.MessageTypes[165];

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
	public int PingJunZhanLi
	{
		get
		{
			return pingJunZhanLi_;
		}
		set
		{
			pingJunZhanLi_ = value;
		}
	}

	[DebuggerNonUserCode]
	public int KeYongDiZiNum
	{
		get
		{
			return keYongDiZiNum_;
		}
		set
		{
			keYongDiZiNum_ = value;
		}
	}

	private MessageDescriptor pb_003A_003AGoogle_002EProtobuf_002EIMessage_002Eget_Descriptor()
	{
		return Descriptor;
	}

	[DebuggerNonUserCode]
	public NeiMenDiZiNumResp_1015132()
	{
	}

	[DebuggerNonUserCode]
	public NeiMenDiZiNumResp_1015132(NeiMenDiZiNumResp_1015132 other)
		: this()
	{
		code_ = other.code_;
		isSuccess_ = other.isSuccess_;
		pingJunZhanLi_ = other.pingJunZhanLi_;
		keYongDiZiNum_ = other.keYongDiZiNum_;
		_unknownFields = UnknownFieldSet.Clone(other._unknownFields);
	}

	[DebuggerNonUserCode]
	public NeiMenDiZiNumResp_1015132 Clone()
	{
		return new NeiMenDiZiNumResp_1015132(this);
	}

	[DebuggerNonUserCode]
	public override bool Equals(object other)
	{
		return Equals(other as NeiMenDiZiNumResp_1015132);
	}

	[DebuggerNonUserCode]
	public bool Equals(NeiMenDiZiNumResp_1015132 other)
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
		if (PingJunZhanLi != other.PingJunZhanLi)
		{
			return false;
		}
		if (KeYongDiZiNum != other.KeYongDiZiNum)
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
		if (PingJunZhanLi != 0)
		{
			num ^= PingJunZhanLi.GetHashCode();
		}
		if (KeYongDiZiNum != 0)
		{
			num ^= KeYongDiZiNum.GetHashCode();
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
		if (PingJunZhanLi != 0)
		{
			output.WriteRawTag(24);
			output.WriteInt32(PingJunZhanLi);
		}
		if (KeYongDiZiNum != 0)
		{
			output.WriteRawTag(32);
			output.WriteInt32(KeYongDiZiNum);
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
		if (PingJunZhanLi != 0)
		{
			num += 1 + CodedOutputStream.ComputeInt32Size(PingJunZhanLi);
		}
		if (KeYongDiZiNum != 0)
		{
			num += 1 + CodedOutputStream.ComputeInt32Size(KeYongDiZiNum);
		}
		if (_unknownFields != null)
		{
			num += _unknownFields.CalculateSize();
		}
		return num;
	}

	[DebuggerNonUserCode]
	public void MergeFrom(NeiMenDiZiNumResp_1015132 other)
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
			if (other.PingJunZhanLi != 0)
			{
				PingJunZhanLi = other.PingJunZhanLi;
			}
			if (other.KeYongDiZiNum != 0)
			{
				KeYongDiZiNum = other.KeYongDiZiNum;
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
				PingJunZhanLi = input.ReadInt32();
				break;
			case 32u:
				KeYongDiZiNum = input.ReadInt32();
				break;
			}
		}
	}
}
