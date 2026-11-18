using System;
using System.Diagnostics;
using Google.Protobuf;
using Google.Protobuf.Reflection;

public sealed class LiBaoBuyInfoResp_1002022 : IMessage<LiBaoBuyInfoResp_1002022>, IMessage, IEquatable<LiBaoBuyInfoResp_1002022>, IDeepCloneable<LiBaoBuyInfoResp_1002022>
{
	private static readonly MessageParser<LiBaoBuyInfoResp_1002022> _parser = new MessageParser<LiBaoBuyInfoResp_1002022>(() => new LiBaoBuyInfoResp_1002022());

	private UnknownFieldSet _unknownFields;

	public const int CodeFieldNumber = 1;

	private int code_;

	public const int IsSuccessFieldNumber = 2;

	private bool isSuccess_;

	public const int IsGetShouChongFieldNumber = 3;

	private int isGetShouChong_;

	public const int IsBuyXiuZhenFieldNumber = 4;

	private int isBuyXiuZhen_;

	public const int IsBuyMeiRiFieldNumber = 5;

	private int isBuyMeiRi_;

	public const int IsBuyMeiRiJinJieFieldNumber = 6;

	private int isBuyMeiRiJinJie_;

	public const int IsBuyMeiRiChaoZhiFieldNumber = 7;

	private int isBuyMeiRiChaoZhi_;

	public const int IsBuyMeiRiHaoHuaFieldNumber = 8;

	private int isBuyMeiRiHaoHua_;

	public const int IsBuyShouChongTeHuiFieldNumber = 9;

	private int isBuyShouChongTeHui_;

	public const int IsBuyJingJieTeHuiFieldNumber = 10;

	private int isBuyJingJieTeHui_;

	public const int IsBuyNew0FieldNumber = 11;

	private int isBuyNew0_;

	public const int IsBuyNew1FieldNumber = 12;

	private int isBuyNew1_;

	public const int IsBuyNew2FieldNumber = 13;

	private int isBuyNew2_;

	public const int IsBuyNew3FieldNumber = 14;

	private int isBuyNew3_;

	[DebuggerNonUserCode]
	public static MessageParser<LiBaoBuyInfoResp_1002022> Parser => _parser;

	[DebuggerNonUserCode]
	public static MessageDescriptor Descriptor => RoleProtoReflection.Descriptor.MessageTypes[24];

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
	public int IsGetShouChong
	{
		get
		{
			return isGetShouChong_;
		}
		set
		{
			isGetShouChong_ = value;
		}
	}

	[DebuggerNonUserCode]
	public int IsBuyXiuZhen
	{
		get
		{
			return isBuyXiuZhen_;
		}
		set
		{
			isBuyXiuZhen_ = value;
		}
	}

	[DebuggerNonUserCode]
	public int IsBuyMeiRi
	{
		get
		{
			return isBuyMeiRi_;
		}
		set
		{
			isBuyMeiRi_ = value;
		}
	}

	[DebuggerNonUserCode]
	public int IsBuyMeiRiJinJie
	{
		get
		{
			return isBuyMeiRiJinJie_;
		}
		set
		{
			isBuyMeiRiJinJie_ = value;
		}
	}

	[DebuggerNonUserCode]
	public int IsBuyMeiRiChaoZhi
	{
		get
		{
			return isBuyMeiRiChaoZhi_;
		}
		set
		{
			isBuyMeiRiChaoZhi_ = value;
		}
	}

	[DebuggerNonUserCode]
	public int IsBuyMeiRiHaoHua
	{
		get
		{
			return isBuyMeiRiHaoHua_;
		}
		set
		{
			isBuyMeiRiHaoHua_ = value;
		}
	}

	[DebuggerNonUserCode]
	public int IsBuyShouChongTeHui
	{
		get
		{
			return isBuyShouChongTeHui_;
		}
		set
		{
			isBuyShouChongTeHui_ = value;
		}
	}

	[DebuggerNonUserCode]
	public int IsBuyJingJieTeHui
	{
		get
		{
			return isBuyJingJieTeHui_;
		}
		set
		{
			isBuyJingJieTeHui_ = value;
		}
	}

	[DebuggerNonUserCode]
	public int IsBuyNew0
	{
		get
		{
			return isBuyNew0_;
		}
		set
		{
			isBuyNew0_ = value;
		}
	}

	[DebuggerNonUserCode]
	public int IsBuyNew1
	{
		get
		{
			return isBuyNew1_;
		}
		set
		{
			isBuyNew1_ = value;
		}
	}

	[DebuggerNonUserCode]
	public int IsBuyNew2
	{
		get
		{
			return isBuyNew2_;
		}
		set
		{
			isBuyNew2_ = value;
		}
	}

	[DebuggerNonUserCode]
	public int IsBuyNew3
	{
		get
		{
			return isBuyNew3_;
		}
		set
		{
			isBuyNew3_ = value;
		}
	}

	private MessageDescriptor pb_003A_003AGoogle_002EProtobuf_002EIMessage_002Eget_Descriptor()
	{
		return Descriptor;
	}

	[DebuggerNonUserCode]
	public LiBaoBuyInfoResp_1002022()
	{
	}

	[DebuggerNonUserCode]
	public LiBaoBuyInfoResp_1002022(LiBaoBuyInfoResp_1002022 other)
		: this()
	{
		code_ = other.code_;
		isSuccess_ = other.isSuccess_;
		isGetShouChong_ = other.isGetShouChong_;
		isBuyXiuZhen_ = other.isBuyXiuZhen_;
		isBuyMeiRi_ = other.isBuyMeiRi_;
		isBuyMeiRiJinJie_ = other.isBuyMeiRiJinJie_;
		isBuyMeiRiChaoZhi_ = other.isBuyMeiRiChaoZhi_;
		isBuyMeiRiHaoHua_ = other.isBuyMeiRiHaoHua_;
		isBuyShouChongTeHui_ = other.isBuyShouChongTeHui_;
		isBuyJingJieTeHui_ = other.isBuyJingJieTeHui_;
		isBuyNew0_ = other.isBuyNew0_;
		isBuyNew1_ = other.isBuyNew1_;
		isBuyNew2_ = other.isBuyNew2_;
		isBuyNew3_ = other.isBuyNew3_;
		_unknownFields = UnknownFieldSet.Clone(other._unknownFields);
	}

	[DebuggerNonUserCode]
	public LiBaoBuyInfoResp_1002022 Clone()
	{
		return new LiBaoBuyInfoResp_1002022(this);
	}

	[DebuggerNonUserCode]
	public override bool Equals(object other)
	{
		return Equals(other as LiBaoBuyInfoResp_1002022);
	}

	[DebuggerNonUserCode]
	public bool Equals(LiBaoBuyInfoResp_1002022 other)
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
		if (IsGetShouChong != other.IsGetShouChong)
		{
			return false;
		}
		if (IsBuyXiuZhen != other.IsBuyXiuZhen)
		{
			return false;
		}
		if (IsBuyMeiRi != other.IsBuyMeiRi)
		{
			return false;
		}
		if (IsBuyMeiRiJinJie != other.IsBuyMeiRiJinJie)
		{
			return false;
		}
		if (IsBuyMeiRiChaoZhi != other.IsBuyMeiRiChaoZhi)
		{
			return false;
		}
		if (IsBuyMeiRiHaoHua != other.IsBuyMeiRiHaoHua)
		{
			return false;
		}
		if (IsBuyShouChongTeHui != other.IsBuyShouChongTeHui)
		{
			return false;
		}
		if (IsBuyJingJieTeHui != other.IsBuyJingJieTeHui)
		{
			return false;
		}
		if (IsBuyNew0 != other.IsBuyNew0)
		{
			return false;
		}
		if (IsBuyNew1 != other.IsBuyNew1)
		{
			return false;
		}
		if (IsBuyNew2 != other.IsBuyNew2)
		{
			return false;
		}
		if (IsBuyNew3 != other.IsBuyNew3)
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
		if (IsGetShouChong != 0)
		{
			num ^= IsGetShouChong.GetHashCode();
		}
		if (IsBuyXiuZhen != 0)
		{
			num ^= IsBuyXiuZhen.GetHashCode();
		}
		if (IsBuyMeiRi != 0)
		{
			num ^= IsBuyMeiRi.GetHashCode();
		}
		if (IsBuyMeiRiJinJie != 0)
		{
			num ^= IsBuyMeiRiJinJie.GetHashCode();
		}
		if (IsBuyMeiRiChaoZhi != 0)
		{
			num ^= IsBuyMeiRiChaoZhi.GetHashCode();
		}
		if (IsBuyMeiRiHaoHua != 0)
		{
			num ^= IsBuyMeiRiHaoHua.GetHashCode();
		}
		if (IsBuyShouChongTeHui != 0)
		{
			num ^= IsBuyShouChongTeHui.GetHashCode();
		}
		if (IsBuyJingJieTeHui != 0)
		{
			num ^= IsBuyJingJieTeHui.GetHashCode();
		}
		if (IsBuyNew0 != 0)
		{
			num ^= IsBuyNew0.GetHashCode();
		}
		if (IsBuyNew1 != 0)
		{
			num ^= IsBuyNew1.GetHashCode();
		}
		if (IsBuyNew2 != 0)
		{
			num ^= IsBuyNew2.GetHashCode();
		}
		if (IsBuyNew3 != 0)
		{
			num ^= IsBuyNew3.GetHashCode();
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
		if (IsGetShouChong != 0)
		{
			output.WriteRawTag(24);
			output.WriteInt32(IsGetShouChong);
		}
		if (IsBuyXiuZhen != 0)
		{
			output.WriteRawTag(32);
			output.WriteInt32(IsBuyXiuZhen);
		}
		if (IsBuyMeiRi != 0)
		{
			output.WriteRawTag(40);
			output.WriteInt32(IsBuyMeiRi);
		}
		if (IsBuyMeiRiJinJie != 0)
		{
			output.WriteRawTag(48);
			output.WriteInt32(IsBuyMeiRiJinJie);
		}
		if (IsBuyMeiRiChaoZhi != 0)
		{
			output.WriteRawTag(56);
			output.WriteInt32(IsBuyMeiRiChaoZhi);
		}
		if (IsBuyMeiRiHaoHua != 0)
		{
			output.WriteRawTag(64);
			output.WriteInt32(IsBuyMeiRiHaoHua);
		}
		if (IsBuyShouChongTeHui != 0)
		{
			output.WriteRawTag(72);
			output.WriteInt32(IsBuyShouChongTeHui);
		}
		if (IsBuyJingJieTeHui != 0)
		{
			output.WriteRawTag(80);
			output.WriteInt32(IsBuyJingJieTeHui);
		}
		if (IsBuyNew0 != 0)
		{
			output.WriteRawTag(88);
			output.WriteInt32(IsBuyNew0);
		}
		if (IsBuyNew1 != 0)
		{
			output.WriteRawTag(96);
			output.WriteInt32(IsBuyNew1);
		}
		if (IsBuyNew2 != 0)
		{
			output.WriteRawTag(104);
			output.WriteInt32(IsBuyNew2);
		}
		if (IsBuyNew3 != 0)
		{
			output.WriteRawTag(112);
			output.WriteInt32(IsBuyNew3);
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
		if (IsGetShouChong != 0)
		{
			num += 1 + CodedOutputStream.ComputeInt32Size(IsGetShouChong);
		}
		if (IsBuyXiuZhen != 0)
		{
			num += 1 + CodedOutputStream.ComputeInt32Size(IsBuyXiuZhen);
		}
		if (IsBuyMeiRi != 0)
		{
			num += 1 + CodedOutputStream.ComputeInt32Size(IsBuyMeiRi);
		}
		if (IsBuyMeiRiJinJie != 0)
		{
			num += 1 + CodedOutputStream.ComputeInt32Size(IsBuyMeiRiJinJie);
		}
		if (IsBuyMeiRiChaoZhi != 0)
		{
			num += 1 + CodedOutputStream.ComputeInt32Size(IsBuyMeiRiChaoZhi);
		}
		if (IsBuyMeiRiHaoHua != 0)
		{
			num += 1 + CodedOutputStream.ComputeInt32Size(IsBuyMeiRiHaoHua);
		}
		if (IsBuyShouChongTeHui != 0)
		{
			num += 1 + CodedOutputStream.ComputeInt32Size(IsBuyShouChongTeHui);
		}
		if (IsBuyJingJieTeHui != 0)
		{
			num += 1 + CodedOutputStream.ComputeInt32Size(IsBuyJingJieTeHui);
		}
		if (IsBuyNew0 != 0)
		{
			num += 1 + CodedOutputStream.ComputeInt32Size(IsBuyNew0);
		}
		if (IsBuyNew1 != 0)
		{
			num += 1 + CodedOutputStream.ComputeInt32Size(IsBuyNew1);
		}
		if (IsBuyNew2 != 0)
		{
			num += 1 + CodedOutputStream.ComputeInt32Size(IsBuyNew2);
		}
		if (IsBuyNew3 != 0)
		{
			num += 1 + CodedOutputStream.ComputeInt32Size(IsBuyNew3);
		}
		if (_unknownFields != null)
		{
			num += _unknownFields.CalculateSize();
		}
		return num;
	}

	[DebuggerNonUserCode]
	public void MergeFrom(LiBaoBuyInfoResp_1002022 other)
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
			if (other.IsGetShouChong != 0)
			{
				IsGetShouChong = other.IsGetShouChong;
			}
			if (other.IsBuyXiuZhen != 0)
			{
				IsBuyXiuZhen = other.IsBuyXiuZhen;
			}
			if (other.IsBuyMeiRi != 0)
			{
				IsBuyMeiRi = other.IsBuyMeiRi;
			}
			if (other.IsBuyMeiRiJinJie != 0)
			{
				IsBuyMeiRiJinJie = other.IsBuyMeiRiJinJie;
			}
			if (other.IsBuyMeiRiChaoZhi != 0)
			{
				IsBuyMeiRiChaoZhi = other.IsBuyMeiRiChaoZhi;
			}
			if (other.IsBuyMeiRiHaoHua != 0)
			{
				IsBuyMeiRiHaoHua = other.IsBuyMeiRiHaoHua;
			}
			if (other.IsBuyShouChongTeHui != 0)
			{
				IsBuyShouChongTeHui = other.IsBuyShouChongTeHui;
			}
			if (other.IsBuyJingJieTeHui != 0)
			{
				IsBuyJingJieTeHui = other.IsBuyJingJieTeHui;
			}
			if (other.IsBuyNew0 != 0)
			{
				IsBuyNew0 = other.IsBuyNew0;
			}
			if (other.IsBuyNew1 != 0)
			{
				IsBuyNew1 = other.IsBuyNew1;
			}
			if (other.IsBuyNew2 != 0)
			{
				IsBuyNew2 = other.IsBuyNew2;
			}
			if (other.IsBuyNew3 != 0)
			{
				IsBuyNew3 = other.IsBuyNew3;
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
				IsGetShouChong = input.ReadInt32();
				break;
			case 32u:
				IsBuyXiuZhen = input.ReadInt32();
				break;
			case 40u:
				IsBuyMeiRi = input.ReadInt32();
				break;
			case 48u:
				IsBuyMeiRiJinJie = input.ReadInt32();
				break;
			case 56u:
				IsBuyMeiRiChaoZhi = input.ReadInt32();
				break;
			case 64u:
				IsBuyMeiRiHaoHua = input.ReadInt32();
				break;
			case 72u:
				IsBuyShouChongTeHui = input.ReadInt32();
				break;
			case 80u:
				IsBuyJingJieTeHui = input.ReadInt32();
				break;
			case 88u:
				IsBuyNew0 = input.ReadInt32();
				break;
			case 96u:
				IsBuyNew1 = input.ReadInt32();
				break;
			case 104u:
				IsBuyNew2 = input.ReadInt32();
				break;
			case 112u:
				IsBuyNew3 = input.ReadInt32();
				break;
			}
		}
	}
}
