using System;
using System.Diagnostics;
using Google.Protobuf;
using Google.Protobuf.Reflection;

public sealed class GetOutSpaceBoxResp_1005023 : IMessage<GetOutSpaceBoxResp_1005023>, IMessage, IEquatable<GetOutSpaceBoxResp_1005023>, IDeepCloneable<GetOutSpaceBoxResp_1005023>
{
	private static readonly MessageParser<GetOutSpaceBoxResp_1005023> _parser = new MessageParser<GetOutSpaceBoxResp_1005023>(() => new GetOutSpaceBoxResp_1005023());

	private UnknownFieldSet _unknownFields;

	public const int CodeFieldNumber = 1;

	private int code_;

	public const int IsSuccessFieldNumber = 2;

	private bool isSuccess_;

	public const int NowRingCapacityFieldNumber = 3;

	private int nowRingCapacity_;

	public const int NowBoxCapacityFieldNumber = 4;

	private int nowBoxCapacity_;

	public const int GoodsIdFieldNumber = 5;

	private string goodsId_ = "";

	[DebuggerNonUserCode]
	public static MessageParser<GetOutSpaceBoxResp_1005023> Parser => _parser;

	[DebuggerNonUserCode]
	public static MessageDescriptor Descriptor => HomeProtoReflection.Descriptor.MessageTypes[28];

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
	public int NowRingCapacity
	{
		get
		{
			return nowRingCapacity_;
		}
		set
		{
			nowRingCapacity_ = value;
		}
	}

	[DebuggerNonUserCode]
	public int NowBoxCapacity
	{
		get
		{
			return nowBoxCapacity_;
		}
		set
		{
			nowBoxCapacity_ = value;
		}
	}

	[DebuggerNonUserCode]
	public string GoodsId
	{
		get
		{
			return goodsId_;
		}
		set
		{
			goodsId_ = ProtoPreconditions.CheckNotNull(value, "value");
		}
	}

	private MessageDescriptor pb_003A_003AGoogle_002EProtobuf_002EIMessage_002Eget_Descriptor()
	{
		return Descriptor;
	}

	[DebuggerNonUserCode]
	public GetOutSpaceBoxResp_1005023()
	{
	}

	[DebuggerNonUserCode]
	public GetOutSpaceBoxResp_1005023(GetOutSpaceBoxResp_1005023 other)
		: this()
	{
		code_ = other.code_;
		isSuccess_ = other.isSuccess_;
		nowRingCapacity_ = other.nowRingCapacity_;
		nowBoxCapacity_ = other.nowBoxCapacity_;
		goodsId_ = other.goodsId_;
		_unknownFields = UnknownFieldSet.Clone(other._unknownFields);
	}

	[DebuggerNonUserCode]
	public GetOutSpaceBoxResp_1005023 Clone()
	{
		return new GetOutSpaceBoxResp_1005023(this);
	}

	[DebuggerNonUserCode]
	public override bool Equals(object other)
	{
		return Equals(other as GetOutSpaceBoxResp_1005023);
	}

	[DebuggerNonUserCode]
	public bool Equals(GetOutSpaceBoxResp_1005023 other)
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
		if (NowRingCapacity != other.NowRingCapacity)
		{
			return false;
		}
		if (NowBoxCapacity != other.NowBoxCapacity)
		{
			return false;
		}
		if (GoodsId != other.GoodsId)
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
		if (NowRingCapacity != 0)
		{
			num ^= NowRingCapacity.GetHashCode();
		}
		if (NowBoxCapacity != 0)
		{
			num ^= NowBoxCapacity.GetHashCode();
		}
		if (GoodsId.Length != 0)
		{
			num ^= GoodsId.GetHashCode();
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
		if (NowRingCapacity != 0)
		{
			output.WriteRawTag(24);
			output.WriteInt32(NowRingCapacity);
		}
		if (NowBoxCapacity != 0)
		{
			output.WriteRawTag(32);
			output.WriteInt32(NowBoxCapacity);
		}
		if (GoodsId.Length != 0)
		{
			output.WriteRawTag(42);
			output.WriteString(GoodsId);
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
		if (NowRingCapacity != 0)
		{
			num += 1 + CodedOutputStream.ComputeInt32Size(NowRingCapacity);
		}
		if (NowBoxCapacity != 0)
		{
			num += 1 + CodedOutputStream.ComputeInt32Size(NowBoxCapacity);
		}
		if (GoodsId.Length != 0)
		{
			num += 1 + CodedOutputStream.ComputeStringSize(GoodsId);
		}
		if (_unknownFields != null)
		{
			num += _unknownFields.CalculateSize();
		}
		return num;
	}

	[DebuggerNonUserCode]
	public void MergeFrom(GetOutSpaceBoxResp_1005023 other)
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
			if (other.NowRingCapacity != 0)
			{
				NowRingCapacity = other.NowRingCapacity;
			}
			if (other.NowBoxCapacity != 0)
			{
				NowBoxCapacity = other.NowBoxCapacity;
			}
			if (other.GoodsId.Length != 0)
			{
				GoodsId = other.GoodsId;
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
				NowRingCapacity = input.ReadInt32();
				break;
			case 32u:
				NowBoxCapacity = input.ReadInt32();
				break;
			case 42u:
				GoodsId = input.ReadString();
				break;
			}
		}
	}
}
