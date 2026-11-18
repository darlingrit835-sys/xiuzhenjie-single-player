using System;
using System.Diagnostics;
using Google.Protobuf;
using Google.Protobuf.Collections;
using Google.Protobuf.Reflection;

public sealed class GoodsBookMatrixDetailResp_1004044 : IMessage<GoodsBookMatrixDetailResp_1004044>, IMessage, IEquatable<GoodsBookMatrixDetailResp_1004044>, IDeepCloneable<GoodsBookMatrixDetailResp_1004044>
{
	private static readonly MessageParser<GoodsBookMatrixDetailResp_1004044> _parser = new MessageParser<GoodsBookMatrixDetailResp_1004044>(() => new GoodsBookMatrixDetailResp_1004044());

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

	public const int VolumeFieldNumber = 10;

	private double volume_;

	[DebuggerNonUserCode]
	public static MessageParser<GoodsBookMatrixDetailResp_1004044> Parser => _parser;

	[DebuggerNonUserCode]
	public static MessageDescriptor Descriptor => GoodsProtoReflection.Descriptor.MessageTypes[75];

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
	public double Volume
	{
		get
		{
			return volume_;
		}
		set
		{
			volume_ = value;
		}
	}

	private MessageDescriptor pb_003A_003AGoogle_002EProtobuf_002EIMessage_002Eget_Descriptor()
	{
		return Descriptor;
	}

	[DebuggerNonUserCode]
	public GoodsBookMatrixDetailResp_1004044()
	{
	}

	[DebuggerNonUserCode]
	public GoodsBookMatrixDetailResp_1004044(GoodsBookMatrixDetailResp_1004044 other)
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
		volume_ = other.volume_;
		_unknownFields = UnknownFieldSet.Clone(other._unknownFields);
	}

	[DebuggerNonUserCode]
	public GoodsBookMatrixDetailResp_1004044 Clone()
	{
		return new GoodsBookMatrixDetailResp_1004044(this);
	}

	[DebuggerNonUserCode]
	public override bool Equals(object other)
	{
		return Equals(other as GoodsBookMatrixDetailResp_1004044);
	}

	[DebuggerNonUserCode]
	public bool Equals(GoodsBookMatrixDetailResp_1004044 other)
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
		if (!ProtobufEqualityComparers.BitwiseDoubleEqualityComparer.Equals(Volume, other.Volume))
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
		if (Volume != 0.0)
		{
			num ^= ProtobufEqualityComparers.BitwiseDoubleEqualityComparer.GetHashCode(Volume);
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
		if (Volume != 0.0)
		{
			output.WriteRawTag(81);
			output.WriteDouble(Volume);
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
		if (Volume != 0.0)
		{
			num += 9;
		}
		if (_unknownFields != null)
		{
			num += _unknownFields.CalculateSize();
		}
		return num;
	}

	[DebuggerNonUserCode]
	public void MergeFrom(GoodsBookMatrixDetailResp_1004044 other)
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
			if (other.Volume != 0.0)
			{
				Volume = other.Volume;
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
			case 81u:
				Volume = input.ReadDouble();
				break;
			}
		}
	}
}
