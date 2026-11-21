using System;
using System.Diagnostics;
using Google.Protobuf;
using Google.Protobuf.Reflection;

public sealed class EnterShiKongMiJingResp_1015154 : IMessage<EnterShiKongMiJingResp_1015154>, IMessage, IEquatable<EnterShiKongMiJingResp_1015154>, IDeepCloneable<EnterShiKongMiJingResp_1015154>
{
	private static readonly MessageParser<EnterShiKongMiJingResp_1015154> _parser = new MessageParser<EnterShiKongMiJingResp_1015154>(() => new EnterShiKongMiJingResp_1015154());

	private UnknownFieldSet _unknownFields;

	public const int CodeFieldNumber = 1;

	private int code_;

	public const int IsSuccessFieldNumber = 2;

	private bool isSuccess_;

	public const int TodayCanUseFieldNumber = 3;

	private bool todayCanUse_;

	public const int NeedGongxianFieldNumber = 4;

	private int needGongxian_;

	[DebuggerNonUserCode]
	public static MessageParser<EnterShiKongMiJingResp_1015154> Parser => _parser;

	[DebuggerNonUserCode]
	public static MessageDescriptor Descriptor => GuildProtoReflection.Descriptor.MessageTypes[196];

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
	public bool TodayCanUse
	{
		get
		{
			return todayCanUse_;
		}
		set
		{
			todayCanUse_ = value;
		}
	}

	[DebuggerNonUserCode]
	public int NeedGongxian
	{
		get
		{
			return needGongxian_;
		}
		set
		{
			needGongxian_ = value;
		}
	}

	private MessageDescriptor pb_003A_003AGoogle_002EProtobuf_002EIMessage_002Eget_Descriptor()
	{
		return Descriptor;
	}

	[DebuggerNonUserCode]
	public EnterShiKongMiJingResp_1015154()
	{
	}

	[DebuggerNonUserCode]
	public EnterShiKongMiJingResp_1015154(EnterShiKongMiJingResp_1015154 other)
		: this()
	{
		code_ = other.code_;
		isSuccess_ = other.isSuccess_;
		todayCanUse_ = other.todayCanUse_;
		needGongxian_ = other.needGongxian_;
		_unknownFields = UnknownFieldSet.Clone(other._unknownFields);
	}

	[DebuggerNonUserCode]
	public EnterShiKongMiJingResp_1015154 Clone()
	{
		return new EnterShiKongMiJingResp_1015154(this);
	}

	[DebuggerNonUserCode]
	public override bool Equals(object other)
	{
		return Equals(other as EnterShiKongMiJingResp_1015154);
	}

	[DebuggerNonUserCode]
	public bool Equals(EnterShiKongMiJingResp_1015154 other)
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
		if (TodayCanUse != other.TodayCanUse)
		{
			return false;
		}
		if (NeedGongxian != other.NeedGongxian)
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
		if (TodayCanUse)
		{
			num ^= TodayCanUse.GetHashCode();
		}
		if (NeedGongxian != 0)
		{
			num ^= NeedGongxian.GetHashCode();
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
		if (TodayCanUse)
		{
			output.WriteRawTag(24);
			output.WriteBool(TodayCanUse);
		}
		if (NeedGongxian != 0)
		{
			output.WriteRawTag(32);
			output.WriteInt32(NeedGongxian);
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
		if (TodayCanUse)
		{
			num += 2;
		}
		if (NeedGongxian != 0)
		{
			num += 1 + CodedOutputStream.ComputeInt32Size(NeedGongxian);
		}
		if (_unknownFields != null)
		{
			num += _unknownFields.CalculateSize();
		}
		return num;
	}

	[DebuggerNonUserCode]
	public void MergeFrom(EnterShiKongMiJingResp_1015154 other)
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
			if (other.TodayCanUse)
			{
				TodayCanUse = other.TodayCanUse;
			}
			if (other.NeedGongxian != 0)
			{
				NeedGongxian = other.NeedGongxian;
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
				TodayCanUse = input.ReadBool();
				break;
			case 32u:
				NeedGongxian = input.ReadInt32();
				break;
			}
		}
	}
}
