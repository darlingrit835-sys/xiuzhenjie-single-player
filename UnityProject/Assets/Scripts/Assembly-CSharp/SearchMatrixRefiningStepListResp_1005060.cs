using System;
using System.Diagnostics;
using Google.Protobuf;
using Google.Protobuf.Collections;
using Google.Protobuf.Reflection;

public sealed class SearchMatrixRefiningStepListResp_1005060 : IMessage<SearchMatrixRefiningStepListResp_1005060>, IMessage, IEquatable<SearchMatrixRefiningStepListResp_1005060>, IDeepCloneable<SearchMatrixRefiningStepListResp_1005060>
{
	private static readonly MessageParser<SearchMatrixRefiningStepListResp_1005060> _parser = new MessageParser<SearchMatrixRefiningStepListResp_1005060>(() => new SearchMatrixRefiningStepListResp_1005060());

	private UnknownFieldSet _unknownFields;

	public const int CodeFieldNumber = 1;

	private int code_;

	public const int IsSuccessFieldNumber = 2;

	private bool isSuccess_;

	public const int WaiQuanFieldNumber = 3;

	private static readonly FieldCodec<int> _repeated_waiQuan_codec = FieldCodec.ForInt32(26u);

	private readonly RepeatedField<int> waiQuan_ = new RepeatedField<int>();

	public const int ZhongQuanFieldNumber = 4;

	private static readonly FieldCodec<int> _repeated_zhongQuan_codec = FieldCodec.ForInt32(34u);

	private readonly RepeatedField<int> zhongQuan_ = new RepeatedField<int>();

	public const int NeiQuanFieldNumber = 5;

	private static readonly FieldCodec<int> _repeated_neiQuan_codec = FieldCodec.ForInt32(42u);

	private readonly RepeatedField<int> neiQuan_ = new RepeatedField<int>();

	public const int WaiQuanSuccessFieldNumber = 6;

	private int waiQuanSuccess_;

	public const int ZhongQuanSuccessFieldNumber = 7;

	private int zhongQuanSuccess_;

	public const int NeiQuanSuccessFieldNumber = 8;

	private int neiQuanSuccess_;

	public const int ZhongDianSuccessFieldNumber = 9;

	private int zhongDianSuccess_;

	[DebuggerNonUserCode]
	public static MessageParser<SearchMatrixRefiningStepListResp_1005060> Parser => _parser;

	[DebuggerNonUserCode]
	public static MessageDescriptor Descriptor => HomeProtoReflection.Descriptor.MessageTypes[82];

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
	public RepeatedField<int> WaiQuan => waiQuan_;

	[DebuggerNonUserCode]
	public RepeatedField<int> ZhongQuan => zhongQuan_;

	[DebuggerNonUserCode]
	public RepeatedField<int> NeiQuan => neiQuan_;

	[DebuggerNonUserCode]
	public int WaiQuanSuccess
	{
		get
		{
			return waiQuanSuccess_;
		}
		set
		{
			waiQuanSuccess_ = value;
		}
	}

	[DebuggerNonUserCode]
	public int ZhongQuanSuccess
	{
		get
		{
			return zhongQuanSuccess_;
		}
		set
		{
			zhongQuanSuccess_ = value;
		}
	}

	[DebuggerNonUserCode]
	public int NeiQuanSuccess
	{
		get
		{
			return neiQuanSuccess_;
		}
		set
		{
			neiQuanSuccess_ = value;
		}
	}

	[DebuggerNonUserCode]
	public int ZhongDianSuccess
	{
		get
		{
			return zhongDianSuccess_;
		}
		set
		{
			zhongDianSuccess_ = value;
		}
	}

	private MessageDescriptor pb_003A_003AGoogle_002EProtobuf_002EIMessage_002Eget_Descriptor()
	{
		return Descriptor;
	}

	[DebuggerNonUserCode]
	public SearchMatrixRefiningStepListResp_1005060()
	{
	}

	[DebuggerNonUserCode]
	public SearchMatrixRefiningStepListResp_1005060(SearchMatrixRefiningStepListResp_1005060 other)
		: this()
	{
		code_ = other.code_;
		isSuccess_ = other.isSuccess_;
		waiQuan_ = other.waiQuan_.Clone();
		zhongQuan_ = other.zhongQuan_.Clone();
		neiQuan_ = other.neiQuan_.Clone();
		waiQuanSuccess_ = other.waiQuanSuccess_;
		zhongQuanSuccess_ = other.zhongQuanSuccess_;
		neiQuanSuccess_ = other.neiQuanSuccess_;
		zhongDianSuccess_ = other.zhongDianSuccess_;
		_unknownFields = UnknownFieldSet.Clone(other._unknownFields);
	}

	[DebuggerNonUserCode]
	public SearchMatrixRefiningStepListResp_1005060 Clone()
	{
		return new SearchMatrixRefiningStepListResp_1005060(this);
	}

	[DebuggerNonUserCode]
	public override bool Equals(object other)
	{
		return Equals(other as SearchMatrixRefiningStepListResp_1005060);
	}

	[DebuggerNonUserCode]
	public bool Equals(SearchMatrixRefiningStepListResp_1005060 other)
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
		if (!waiQuan_.Equals(other.waiQuan_))
		{
			return false;
		}
		if (!zhongQuan_.Equals(other.zhongQuan_))
		{
			return false;
		}
		if (!neiQuan_.Equals(other.neiQuan_))
		{
			return false;
		}
		if (WaiQuanSuccess != other.WaiQuanSuccess)
		{
			return false;
		}
		if (ZhongQuanSuccess != other.ZhongQuanSuccess)
		{
			return false;
		}
		if (NeiQuanSuccess != other.NeiQuanSuccess)
		{
			return false;
		}
		if (ZhongDianSuccess != other.ZhongDianSuccess)
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
		num ^= waiQuan_.GetHashCode();
		num ^= zhongQuan_.GetHashCode();
		num ^= neiQuan_.GetHashCode();
		if (WaiQuanSuccess != 0)
		{
			num ^= WaiQuanSuccess.GetHashCode();
		}
		if (ZhongQuanSuccess != 0)
		{
			num ^= ZhongQuanSuccess.GetHashCode();
		}
		if (NeiQuanSuccess != 0)
		{
			num ^= NeiQuanSuccess.GetHashCode();
		}
		if (ZhongDianSuccess != 0)
		{
			num ^= ZhongDianSuccess.GetHashCode();
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
		waiQuan_.WriteTo(output, _repeated_waiQuan_codec);
		zhongQuan_.WriteTo(output, _repeated_zhongQuan_codec);
		neiQuan_.WriteTo(output, _repeated_neiQuan_codec);
		if (WaiQuanSuccess != 0)
		{
			output.WriteRawTag(48);
			output.WriteInt32(WaiQuanSuccess);
		}
		if (ZhongQuanSuccess != 0)
		{
			output.WriteRawTag(56);
			output.WriteInt32(ZhongQuanSuccess);
		}
		if (NeiQuanSuccess != 0)
		{
			output.WriteRawTag(64);
			output.WriteInt32(NeiQuanSuccess);
		}
		if (ZhongDianSuccess != 0)
		{
			output.WriteRawTag(72);
			output.WriteInt32(ZhongDianSuccess);
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
		num += waiQuan_.CalculateSize(_repeated_waiQuan_codec);
		num += zhongQuan_.CalculateSize(_repeated_zhongQuan_codec);
		num += neiQuan_.CalculateSize(_repeated_neiQuan_codec);
		if (WaiQuanSuccess != 0)
		{
			num += 1 + CodedOutputStream.ComputeInt32Size(WaiQuanSuccess);
		}
		if (ZhongQuanSuccess != 0)
		{
			num += 1 + CodedOutputStream.ComputeInt32Size(ZhongQuanSuccess);
		}
		if (NeiQuanSuccess != 0)
		{
			num += 1 + CodedOutputStream.ComputeInt32Size(NeiQuanSuccess);
		}
		if (ZhongDianSuccess != 0)
		{
			num += 1 + CodedOutputStream.ComputeInt32Size(ZhongDianSuccess);
		}
		if (_unknownFields != null)
		{
			num += _unknownFields.CalculateSize();
		}
		return num;
	}

	[DebuggerNonUserCode]
	public void MergeFrom(SearchMatrixRefiningStepListResp_1005060 other)
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
			waiQuan_.Add(other.waiQuan_);
			zhongQuan_.Add(other.zhongQuan_);
			neiQuan_.Add(other.neiQuan_);
			if (other.WaiQuanSuccess != 0)
			{
				WaiQuanSuccess = other.WaiQuanSuccess;
			}
			if (other.ZhongQuanSuccess != 0)
			{
				ZhongQuanSuccess = other.ZhongQuanSuccess;
			}
			if (other.NeiQuanSuccess != 0)
			{
				NeiQuanSuccess = other.NeiQuanSuccess;
			}
			if (other.ZhongDianSuccess != 0)
			{
				ZhongDianSuccess = other.ZhongDianSuccess;
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
			case 26u:
				waiQuan_.AddEntriesFrom(input, _repeated_waiQuan_codec);
				break;
			case 32u:
			case 34u:
				zhongQuan_.AddEntriesFrom(input, _repeated_zhongQuan_codec);
				break;
			case 40u:
			case 42u:
				neiQuan_.AddEntriesFrom(input, _repeated_neiQuan_codec);
				break;
			case 48u:
				WaiQuanSuccess = input.ReadInt32();
				break;
			case 56u:
				ZhongQuanSuccess = input.ReadInt32();
				break;
			case 64u:
				NeiQuanSuccess = input.ReadInt32();
				break;
			case 72u:
				ZhongDianSuccess = input.ReadInt32();
				break;
			}
		}
	}
}
