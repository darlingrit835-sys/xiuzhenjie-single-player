using System;
using System.Diagnostics;
using Google.Protobuf;
using Google.Protobuf.Collections;
using Google.Protobuf.Reflection;

public sealed class SheZhiZhiFaCeLueDiZiNumResp_1015028 : IMessage<SheZhiZhiFaCeLueDiZiNumResp_1015028>, IMessage, IEquatable<SheZhiZhiFaCeLueDiZiNumResp_1015028>, IDeepCloneable<SheZhiZhiFaCeLueDiZiNumResp_1015028>
{
	private static readonly MessageParser<SheZhiZhiFaCeLueDiZiNumResp_1015028> _parser = new MessageParser<SheZhiZhiFaCeLueDiZiNumResp_1015028>(() => new SheZhiZhiFaCeLueDiZiNumResp_1015028());

	private UnknownFieldSet _unknownFields;

	public const int CodeFieldNumber = 1;

	private int code_;

	public const int IsSuccessFieldNumber = 2;

	private bool isSuccess_;

	public const int ZongMenHunLuanDuFieldNumber = 3;

	private int zongMenHunLuanDu_;

	public const int SiWangLvFieldNumber = 4;

	private double siWangLv_;

	public const int KeYongDiZiNumFieldNumber = 5;

	private int keYongDiZiNum_;

	[DebuggerNonUserCode]
	public static MessageParser<SheZhiZhiFaCeLueDiZiNumResp_1015028> Parser => _parser;

	[DebuggerNonUserCode]
	public static MessageDescriptor Descriptor => GuildProtoReflection.Descriptor.MessageTypes[31];

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
	public int ZongMenHunLuanDu
	{
		get
		{
			return zongMenHunLuanDu_;
		}
		set
		{
			zongMenHunLuanDu_ = value;
		}
	}

	[DebuggerNonUserCode]
	public double SiWangLv
	{
		get
		{
			return siWangLv_;
		}
		set
		{
			siWangLv_ = value;
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
	public SheZhiZhiFaCeLueDiZiNumResp_1015028()
	{
	}

	[DebuggerNonUserCode]
	public SheZhiZhiFaCeLueDiZiNumResp_1015028(SheZhiZhiFaCeLueDiZiNumResp_1015028 other)
		: this()
	{
		code_ = other.code_;
		isSuccess_ = other.isSuccess_;
		zongMenHunLuanDu_ = other.zongMenHunLuanDu_;
		siWangLv_ = other.siWangLv_;
		keYongDiZiNum_ = other.keYongDiZiNum_;
		_unknownFields = UnknownFieldSet.Clone(other._unknownFields);
	}

	[DebuggerNonUserCode]
	public SheZhiZhiFaCeLueDiZiNumResp_1015028 Clone()
	{
		return new SheZhiZhiFaCeLueDiZiNumResp_1015028(this);
	}

	[DebuggerNonUserCode]
	public override bool Equals(object other)
	{
		return Equals(other as SheZhiZhiFaCeLueDiZiNumResp_1015028);
	}

	[DebuggerNonUserCode]
	public bool Equals(SheZhiZhiFaCeLueDiZiNumResp_1015028 other)
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
		if (ZongMenHunLuanDu != other.ZongMenHunLuanDu)
		{
			return false;
		}
		if (!ProtobufEqualityComparers.BitwiseDoubleEqualityComparer.Equals(SiWangLv, other.SiWangLv))
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
		if (ZongMenHunLuanDu != 0)
		{
			num ^= ZongMenHunLuanDu.GetHashCode();
		}
		if (SiWangLv != 0.0)
		{
			num ^= ProtobufEqualityComparers.BitwiseDoubleEqualityComparer.GetHashCode(SiWangLv);
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
		if (ZongMenHunLuanDu != 0)
		{
			output.WriteRawTag(24);
			output.WriteInt32(ZongMenHunLuanDu);
		}
		if (SiWangLv != 0.0)
		{
			output.WriteRawTag(33);
			output.WriteDouble(SiWangLv);
		}
		if (KeYongDiZiNum != 0)
		{
			output.WriteRawTag(40);
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
		if (ZongMenHunLuanDu != 0)
		{
			num += 1 + CodedOutputStream.ComputeInt32Size(ZongMenHunLuanDu);
		}
		if (SiWangLv != 0.0)
		{
			num += 9;
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
	public void MergeFrom(SheZhiZhiFaCeLueDiZiNumResp_1015028 other)
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
			if (other.ZongMenHunLuanDu != 0)
			{
				ZongMenHunLuanDu = other.ZongMenHunLuanDu;
			}
			if (other.SiWangLv != 0.0)
			{
				SiWangLv = other.SiWangLv;
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
				ZongMenHunLuanDu = input.ReadInt32();
				break;
			case 33u:
				SiWangLv = input.ReadDouble();
				break;
			case 40u:
				KeYongDiZiNum = input.ReadInt32();
				break;
			}
		}
	}
}
