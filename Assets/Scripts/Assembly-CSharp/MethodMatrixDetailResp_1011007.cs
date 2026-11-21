using System;
using System.Diagnostics;
using Google.Protobuf;
using Google.Protobuf.Collections;
using Google.Protobuf.Reflection;

public sealed class MethodMatrixDetailResp_1011007 : IMessage<MethodMatrixDetailResp_1011007>, IMessage, IEquatable<MethodMatrixDetailResp_1011007>, IDeepCloneable<MethodMatrixDetailResp_1011007>
{
	private static readonly MessageParser<MethodMatrixDetailResp_1011007> _parser = new MessageParser<MethodMatrixDetailResp_1011007>(() => new MethodMatrixDetailResp_1011007());

	private UnknownFieldSet _unknownFields;

	public const int CodeFieldNumber = 1;

	private int code_;

	public const int IsSuccessFieldNumber = 2;

	private bool isSuccess_;

	public const int TypeFieldNumber = 3;

	private int type_;

	public const int TypeValFieldNumber = 4;

	private double typeVal_;

	public const int ConversionRateFieldNumber = 5;

	private double conversionRate_;

	public const int UnitConsumeFieldNumber = 6;

	private double unitConsume_;

	public const int DurationFieldNumber = 7;

	private int duration_;

	public const int ScopeFieldNumber = 8;

	private double scope_;

	public const int CreatedByFieldNumber = 9;

	private string createdBy_ = "";

	public const int WaiQuanFieldNumber = 10;

	private string waiQuan_ = "";

	public const int ZhongQuanFieldNumber = 11;

	private string zhongQuan_ = "";

	public const int NeiQuanFieldNumber = 12;

	private string neiQuan_ = "";

	public const int ZhongDianFieldNumber = 13;

	private string zhongDian_ = "";

	[DebuggerNonUserCode]
	public static MessageParser<MethodMatrixDetailResp_1011007> Parser => _parser;

	[DebuggerNonUserCode]
	public static MessageDescriptor Descriptor => TemplateProtoReflection.Descriptor.MessageTypes[13];

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
	public double TypeVal
	{
		get
		{
			return typeVal_;
		}
		set
		{
			typeVal_ = value;
		}
	}

	[DebuggerNonUserCode]
	public double ConversionRate
	{
		get
		{
			return conversionRate_;
		}
		set
		{
			conversionRate_ = value;
		}
	}

	[DebuggerNonUserCode]
	public double UnitConsume
	{
		get
		{
			return unitConsume_;
		}
		set
		{
			unitConsume_ = value;
		}
	}

	[DebuggerNonUserCode]
	public int Duration
	{
		get
		{
			return duration_;
		}
		set
		{
			duration_ = value;
		}
	}

	[DebuggerNonUserCode]
	public double Scope
	{
		get
		{
			return scope_;
		}
		set
		{
			scope_ = value;
		}
	}

	[DebuggerNonUserCode]
	public string CreatedBy
	{
		get
		{
			return createdBy_;
		}
		set
		{
			createdBy_ = ProtoPreconditions.CheckNotNull(value, "value");
		}
	}

	[DebuggerNonUserCode]
	public string WaiQuan
	{
		get
		{
			return waiQuan_;
		}
		set
		{
			waiQuan_ = ProtoPreconditions.CheckNotNull(value, "value");
		}
	}

	[DebuggerNonUserCode]
	public string ZhongQuan
	{
		get
		{
			return zhongQuan_;
		}
		set
		{
			zhongQuan_ = ProtoPreconditions.CheckNotNull(value, "value");
		}
	}

	[DebuggerNonUserCode]
	public string NeiQuan
	{
		get
		{
			return neiQuan_;
		}
		set
		{
			neiQuan_ = ProtoPreconditions.CheckNotNull(value, "value");
		}
	}

	[DebuggerNonUserCode]
	public string ZhongDian
	{
		get
		{
			return zhongDian_;
		}
		set
		{
			zhongDian_ = ProtoPreconditions.CheckNotNull(value, "value");
		}
	}

	private MessageDescriptor pb_003A_003AGoogle_002EProtobuf_002EIMessage_002Eget_Descriptor()
	{
		return Descriptor;
	}

	[DebuggerNonUserCode]
	public MethodMatrixDetailResp_1011007()
	{
	}

	[DebuggerNonUserCode]
	public MethodMatrixDetailResp_1011007(MethodMatrixDetailResp_1011007 other)
		: this()
	{
		code_ = other.code_;
		isSuccess_ = other.isSuccess_;
		type_ = other.type_;
		typeVal_ = other.typeVal_;
		conversionRate_ = other.conversionRate_;
		unitConsume_ = other.unitConsume_;
		duration_ = other.duration_;
		scope_ = other.scope_;
		createdBy_ = other.createdBy_;
		waiQuan_ = other.waiQuan_;
		zhongQuan_ = other.zhongQuan_;
		neiQuan_ = other.neiQuan_;
		zhongDian_ = other.zhongDian_;
		_unknownFields = UnknownFieldSet.Clone(other._unknownFields);
	}

	[DebuggerNonUserCode]
	public MethodMatrixDetailResp_1011007 Clone()
	{
		return new MethodMatrixDetailResp_1011007(this);
	}

	[DebuggerNonUserCode]
	public override bool Equals(object other)
	{
		return Equals(other as MethodMatrixDetailResp_1011007);
	}

	[DebuggerNonUserCode]
	public bool Equals(MethodMatrixDetailResp_1011007 other)
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
		if (!ProtobufEqualityComparers.BitwiseDoubleEqualityComparer.Equals(TypeVal, other.TypeVal))
		{
			return false;
		}
		if (!ProtobufEqualityComparers.BitwiseDoubleEqualityComparer.Equals(ConversionRate, other.ConversionRate))
		{
			return false;
		}
		if (!ProtobufEqualityComparers.BitwiseDoubleEqualityComparer.Equals(UnitConsume, other.UnitConsume))
		{
			return false;
		}
		if (Duration != other.Duration)
		{
			return false;
		}
		if (!ProtobufEqualityComparers.BitwiseDoubleEqualityComparer.Equals(Scope, other.Scope))
		{
			return false;
		}
		if (CreatedBy != other.CreatedBy)
		{
			return false;
		}
		if (WaiQuan != other.WaiQuan)
		{
			return false;
		}
		if (ZhongQuan != other.ZhongQuan)
		{
			return false;
		}
		if (NeiQuan != other.NeiQuan)
		{
			return false;
		}
		if (ZhongDian != other.ZhongDian)
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
		if (TypeVal != 0.0)
		{
			num ^= ProtobufEqualityComparers.BitwiseDoubleEqualityComparer.GetHashCode(TypeVal);
		}
		if (ConversionRate != 0.0)
		{
			num ^= ProtobufEqualityComparers.BitwiseDoubleEqualityComparer.GetHashCode(ConversionRate);
		}
		if (UnitConsume != 0.0)
		{
			num ^= ProtobufEqualityComparers.BitwiseDoubleEqualityComparer.GetHashCode(UnitConsume);
		}
		if (Duration != 0)
		{
			num ^= Duration.GetHashCode();
		}
		if (Scope != 0.0)
		{
			num ^= ProtobufEqualityComparers.BitwiseDoubleEqualityComparer.GetHashCode(Scope);
		}
		if (CreatedBy.Length != 0)
		{
			num ^= CreatedBy.GetHashCode();
		}
		if (WaiQuan.Length != 0)
		{
			num ^= WaiQuan.GetHashCode();
		}
		if (ZhongQuan.Length != 0)
		{
			num ^= ZhongQuan.GetHashCode();
		}
		if (NeiQuan.Length != 0)
		{
			num ^= NeiQuan.GetHashCode();
		}
		if (ZhongDian.Length != 0)
		{
			num ^= ZhongDian.GetHashCode();
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
		if (TypeVal != 0.0)
		{
			output.WriteRawTag(33);
			output.WriteDouble(TypeVal);
		}
		if (ConversionRate != 0.0)
		{
			output.WriteRawTag(41);
			output.WriteDouble(ConversionRate);
		}
		if (UnitConsume != 0.0)
		{
			output.WriteRawTag(49);
			output.WriteDouble(UnitConsume);
		}
		if (Duration != 0)
		{
			output.WriteRawTag(56);
			output.WriteInt32(Duration);
		}
		if (Scope != 0.0)
		{
			output.WriteRawTag(65);
			output.WriteDouble(Scope);
		}
		if (CreatedBy.Length != 0)
		{
			output.WriteRawTag(74);
			output.WriteString(CreatedBy);
		}
		if (WaiQuan.Length != 0)
		{
			output.WriteRawTag(82);
			output.WriteString(WaiQuan);
		}
		if (ZhongQuan.Length != 0)
		{
			output.WriteRawTag(90);
			output.WriteString(ZhongQuan);
		}
		if (NeiQuan.Length != 0)
		{
			output.WriteRawTag(98);
			output.WriteString(NeiQuan);
		}
		if (ZhongDian.Length != 0)
		{
			output.WriteRawTag(106);
			output.WriteString(ZhongDian);
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
		if (TypeVal != 0.0)
		{
			num += 9;
		}
		if (ConversionRate != 0.0)
		{
			num += 9;
		}
		if (UnitConsume != 0.0)
		{
			num += 9;
		}
		if (Duration != 0)
		{
			num += 1 + CodedOutputStream.ComputeInt32Size(Duration);
		}
		if (Scope != 0.0)
		{
			num += 9;
		}
		if (CreatedBy.Length != 0)
		{
			num += 1 + CodedOutputStream.ComputeStringSize(CreatedBy);
		}
		if (WaiQuan.Length != 0)
		{
			num += 1 + CodedOutputStream.ComputeStringSize(WaiQuan);
		}
		if (ZhongQuan.Length != 0)
		{
			num += 1 + CodedOutputStream.ComputeStringSize(ZhongQuan);
		}
		if (NeiQuan.Length != 0)
		{
			num += 1 + CodedOutputStream.ComputeStringSize(NeiQuan);
		}
		if (ZhongDian.Length != 0)
		{
			num += 1 + CodedOutputStream.ComputeStringSize(ZhongDian);
		}
		if (_unknownFields != null)
		{
			num += _unknownFields.CalculateSize();
		}
		return num;
	}

	[DebuggerNonUserCode]
	public void MergeFrom(MethodMatrixDetailResp_1011007 other)
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
			if (other.TypeVal != 0.0)
			{
				TypeVal = other.TypeVal;
			}
			if (other.ConversionRate != 0.0)
			{
				ConversionRate = other.ConversionRate;
			}
			if (other.UnitConsume != 0.0)
			{
				UnitConsume = other.UnitConsume;
			}
			if (other.Duration != 0)
			{
				Duration = other.Duration;
			}
			if (other.Scope != 0.0)
			{
				Scope = other.Scope;
			}
			if (other.CreatedBy.Length != 0)
			{
				CreatedBy = other.CreatedBy;
			}
			if (other.WaiQuan.Length != 0)
			{
				WaiQuan = other.WaiQuan;
			}
			if (other.ZhongQuan.Length != 0)
			{
				ZhongQuan = other.ZhongQuan;
			}
			if (other.NeiQuan.Length != 0)
			{
				NeiQuan = other.NeiQuan;
			}
			if (other.ZhongDian.Length != 0)
			{
				ZhongDian = other.ZhongDian;
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
			case 33u:
				TypeVal = input.ReadDouble();
				break;
			case 41u:
				ConversionRate = input.ReadDouble();
				break;
			case 49u:
				UnitConsume = input.ReadDouble();
				break;
			case 56u:
				Duration = input.ReadInt32();
				break;
			case 65u:
				Scope = input.ReadDouble();
				break;
			case 74u:
				CreatedBy = input.ReadString();
				break;
			case 82u:
				WaiQuan = input.ReadString();
				break;
			case 90u:
				ZhongQuan = input.ReadString();
				break;
			case 98u:
				NeiQuan = input.ReadString();
				break;
			case 106u:
				ZhongDian = input.ReadString();
				break;
			}
		}
	}
}
