using System;
using System.Diagnostics;
using Google.Protobuf;
using Google.Protobuf.Reflection;

public sealed class JuanZengFaQiResp_1015134 : IMessage<JuanZengFaQiResp_1015134>, IMessage, IEquatable<JuanZengFaQiResp_1015134>, IDeepCloneable<JuanZengFaQiResp_1015134>
{
	private static readonly MessageParser<JuanZengFaQiResp_1015134> _parser = new MessageParser<JuanZengFaQiResp_1015134>(() => new JuanZengFaQiResp_1015134());

	private UnknownFieldSet _unknownFields;

	public const int CodeFieldNumber = 1;

	private int code_;

	public const int IsSuccessFieldNumber = 2;

	private bool isSuccess_;

	public const int KuCunFieldNumber = 3;

	private int kuCun_;

	public const int SuccessCountFieldNumber = 4;

	private int successCount_;

	public const int FailedCountFieldNumber = 5;

	private int failedCount_;

	public const int FailGoodsIdsFieldNumber = 6;

	private string failGoodsIds_ = "";

	[DebuggerNonUserCode]
	public static MessageParser<JuanZengFaQiResp_1015134> Parser => _parser;

	[DebuggerNonUserCode]
	public static MessageDescriptor Descriptor => GuildProtoReflection.Descriptor.MessageTypes[169];

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
	public int KuCun
	{
		get
		{
			return kuCun_;
		}
		set
		{
			kuCun_ = value;
		}
	}

	[DebuggerNonUserCode]
	public int SuccessCount
	{
		get
		{
			return successCount_;
		}
		set
		{
			successCount_ = value;
		}
	}

	[DebuggerNonUserCode]
	public int FailedCount
	{
		get
		{
			return failedCount_;
		}
		set
		{
			failedCount_ = value;
		}
	}

	[DebuggerNonUserCode]
	public string FailGoodsIds
	{
		get
		{
			return failGoodsIds_;
		}
		set
		{
			failGoodsIds_ = ProtoPreconditions.CheckNotNull(value, "value");
		}
	}

	private MessageDescriptor pb_003A_003AGoogle_002EProtobuf_002EIMessage_002Eget_Descriptor()
	{
		return Descriptor;
	}

	[DebuggerNonUserCode]
	public JuanZengFaQiResp_1015134()
	{
	}

	[DebuggerNonUserCode]
	public JuanZengFaQiResp_1015134(JuanZengFaQiResp_1015134 other)
		: this()
	{
		code_ = other.code_;
		isSuccess_ = other.isSuccess_;
		kuCun_ = other.kuCun_;
		successCount_ = other.successCount_;
		failedCount_ = other.failedCount_;
		failGoodsIds_ = other.failGoodsIds_;
		_unknownFields = UnknownFieldSet.Clone(other._unknownFields);
	}

	[DebuggerNonUserCode]
	public JuanZengFaQiResp_1015134 Clone()
	{
		return new JuanZengFaQiResp_1015134(this);
	}

	[DebuggerNonUserCode]
	public override bool Equals(object other)
	{
		return Equals(other as JuanZengFaQiResp_1015134);
	}

	[DebuggerNonUserCode]
	public bool Equals(JuanZengFaQiResp_1015134 other)
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
		if (KuCun != other.KuCun)
		{
			return false;
		}
		if (SuccessCount != other.SuccessCount)
		{
			return false;
		}
		if (FailedCount != other.FailedCount)
		{
			return false;
		}
		if (FailGoodsIds != other.FailGoodsIds)
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
		if (KuCun != 0)
		{
			num ^= KuCun.GetHashCode();
		}
		if (SuccessCount != 0)
		{
			num ^= SuccessCount.GetHashCode();
		}
		if (FailedCount != 0)
		{
			num ^= FailedCount.GetHashCode();
		}
		if (FailGoodsIds.Length != 0)
		{
			num ^= FailGoodsIds.GetHashCode();
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
		if (KuCun != 0)
		{
			output.WriteRawTag(24);
			output.WriteInt32(KuCun);
		}
		if (SuccessCount != 0)
		{
			output.WriteRawTag(32);
			output.WriteInt32(SuccessCount);
		}
		if (FailedCount != 0)
		{
			output.WriteRawTag(40);
			output.WriteInt32(FailedCount);
		}
		if (FailGoodsIds.Length != 0)
		{
			output.WriteRawTag(50);
			output.WriteString(FailGoodsIds);
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
		if (KuCun != 0)
		{
			num += 1 + CodedOutputStream.ComputeInt32Size(KuCun);
		}
		if (SuccessCount != 0)
		{
			num += 1 + CodedOutputStream.ComputeInt32Size(SuccessCount);
		}
		if (FailedCount != 0)
		{
			num += 1 + CodedOutputStream.ComputeInt32Size(FailedCount);
		}
		if (FailGoodsIds.Length != 0)
		{
			num += 1 + CodedOutputStream.ComputeStringSize(FailGoodsIds);
		}
		if (_unknownFields != null)
		{
			num += _unknownFields.CalculateSize();
		}
		return num;
	}

	[DebuggerNonUserCode]
	public void MergeFrom(JuanZengFaQiResp_1015134 other)
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
			if (other.KuCun != 0)
			{
				KuCun = other.KuCun;
			}
			if (other.SuccessCount != 0)
			{
				SuccessCount = other.SuccessCount;
			}
			if (other.FailedCount != 0)
			{
				FailedCount = other.FailedCount;
			}
			if (other.FailGoodsIds.Length != 0)
			{
				FailGoodsIds = other.FailGoodsIds;
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
				KuCun = input.ReadInt32();
				break;
			case 32u:
				SuccessCount = input.ReadInt32();
				break;
			case 40u:
				FailedCount = input.ReadInt32();
				break;
			case 50u:
				FailGoodsIds = input.ReadString();
				break;
			}
		}
	}
}
