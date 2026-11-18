using System;
using System.Diagnostics;
using Google.Protobuf;
using Google.Protobuf.Collections;
using Google.Protobuf.Reflection;

public sealed class GoodsMaterialMineralDetailResp_1004006 : IMessage<GoodsMaterialMineralDetailResp_1004006>, IMessage, IEquatable<GoodsMaterialMineralDetailResp_1004006>, IDeepCloneable<GoodsMaterialMineralDetailResp_1004006>
{
	private static readonly MessageParser<GoodsMaterialMineralDetailResp_1004006> _parser = new MessageParser<GoodsMaterialMineralDetailResp_1004006>(() => new GoodsMaterialMineralDetailResp_1004006());

	private UnknownFieldSet _unknownFields;

	public const int CodeFieldNumber = 1;

	private int code_;

	public const int IsSuccessFieldNumber = 2;

	private bool isSuccess_;

	public const int MapNameFieldNumber = 3;

	private string mapName_ = "";

	public const int ConversionRateFieldNumber = 4;

	private double conversionRate_;

	public const int IntensityFieldNumber = 5;

	private double intensity_;

	public const int TenacityFieldNumber = 6;

	private double tenacity_;

	public const int AffinityFieldNumber = 7;

	private double affinity_;

	public const int WeightFieldNumber = 8;

	private double weight_;

	public const int VolumeFieldNumber = 9;

	private double volume_;

	public const int UnitFieldNumber = 10;

	private string unit_ = "";

	public const int ReceiveValFieldNumber = 11;

	private double receiveVal_;

	[DebuggerNonUserCode]
	public static MessageParser<GoodsMaterialMineralDetailResp_1004006> Parser => _parser;

	[DebuggerNonUserCode]
	public static MessageDescriptor Descriptor => GoodsProtoReflection.Descriptor.MessageTypes[9];

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
	public string MapName
	{
		get
		{
			return mapName_;
		}
		set
		{
			mapName_ = ProtoPreconditions.CheckNotNull(value, "value");
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
	public double Intensity
	{
		get
		{
			return intensity_;
		}
		set
		{
			intensity_ = value;
		}
	}

	[DebuggerNonUserCode]
	public double Tenacity
	{
		get
		{
			return tenacity_;
		}
		set
		{
			tenacity_ = value;
		}
	}

	[DebuggerNonUserCode]
	public double Affinity
	{
		get
		{
			return affinity_;
		}
		set
		{
			affinity_ = value;
		}
	}

	[DebuggerNonUserCode]
	public double Weight
	{
		get
		{
			return weight_;
		}
		set
		{
			weight_ = value;
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

	[DebuggerNonUserCode]
	public string Unit
	{
		get
		{
			return unit_;
		}
		set
		{
			unit_ = ProtoPreconditions.CheckNotNull(value, "value");
		}
	}

	[DebuggerNonUserCode]
	public double ReceiveVal
	{
		get
		{
			return receiveVal_;
		}
		set
		{
			receiveVal_ = value;
		}
	}

	private MessageDescriptor pb_003A_003AGoogle_002EProtobuf_002EIMessage_002Eget_Descriptor()
	{
		return Descriptor;
	}

	[DebuggerNonUserCode]
	public GoodsMaterialMineralDetailResp_1004006()
	{
	}

	[DebuggerNonUserCode]
	public GoodsMaterialMineralDetailResp_1004006(GoodsMaterialMineralDetailResp_1004006 other)
		: this()
	{
		code_ = other.code_;
		isSuccess_ = other.isSuccess_;
		mapName_ = other.mapName_;
		conversionRate_ = other.conversionRate_;
		intensity_ = other.intensity_;
		tenacity_ = other.tenacity_;
		affinity_ = other.affinity_;
		weight_ = other.weight_;
		volume_ = other.volume_;
		unit_ = other.unit_;
		receiveVal_ = other.receiveVal_;
		_unknownFields = UnknownFieldSet.Clone(other._unknownFields);
	}

	[DebuggerNonUserCode]
	public GoodsMaterialMineralDetailResp_1004006 Clone()
	{
		return new GoodsMaterialMineralDetailResp_1004006(this);
	}

	[DebuggerNonUserCode]
	public override bool Equals(object other)
	{
		return Equals(other as GoodsMaterialMineralDetailResp_1004006);
	}

	[DebuggerNonUserCode]
	public bool Equals(GoodsMaterialMineralDetailResp_1004006 other)
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
		if (MapName != other.MapName)
		{
			return false;
		}
		if (!ProtobufEqualityComparers.BitwiseDoubleEqualityComparer.Equals(ConversionRate, other.ConversionRate))
		{
			return false;
		}
		if (!ProtobufEqualityComparers.BitwiseDoubleEqualityComparer.Equals(Intensity, other.Intensity))
		{
			return false;
		}
		if (!ProtobufEqualityComparers.BitwiseDoubleEqualityComparer.Equals(Tenacity, other.Tenacity))
		{
			return false;
		}
		if (!ProtobufEqualityComparers.BitwiseDoubleEqualityComparer.Equals(Affinity, other.Affinity))
		{
			return false;
		}
		if (!ProtobufEqualityComparers.BitwiseDoubleEqualityComparer.Equals(Weight, other.Weight))
		{
			return false;
		}
		if (!ProtobufEqualityComparers.BitwiseDoubleEqualityComparer.Equals(Volume, other.Volume))
		{
			return false;
		}
		if (Unit != other.Unit)
		{
			return false;
		}
		if (!ProtobufEqualityComparers.BitwiseDoubleEqualityComparer.Equals(ReceiveVal, other.ReceiveVal))
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
		if (MapName.Length != 0)
		{
			num ^= MapName.GetHashCode();
		}
		if (ConversionRate != 0.0)
		{
			num ^= ProtobufEqualityComparers.BitwiseDoubleEqualityComparer.GetHashCode(ConversionRate);
		}
		if (Intensity != 0.0)
		{
			num ^= ProtobufEqualityComparers.BitwiseDoubleEqualityComparer.GetHashCode(Intensity);
		}
		if (Tenacity != 0.0)
		{
			num ^= ProtobufEqualityComparers.BitwiseDoubleEqualityComparer.GetHashCode(Tenacity);
		}
		if (Affinity != 0.0)
		{
			num ^= ProtobufEqualityComparers.BitwiseDoubleEqualityComparer.GetHashCode(Affinity);
		}
		if (Weight != 0.0)
		{
			num ^= ProtobufEqualityComparers.BitwiseDoubleEqualityComparer.GetHashCode(Weight);
		}
		if (Volume != 0.0)
		{
			num ^= ProtobufEqualityComparers.BitwiseDoubleEqualityComparer.GetHashCode(Volume);
		}
		if (Unit.Length != 0)
		{
			num ^= Unit.GetHashCode();
		}
		if (ReceiveVal != 0.0)
		{
			num ^= ProtobufEqualityComparers.BitwiseDoubleEqualityComparer.GetHashCode(ReceiveVal);
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
		if (MapName.Length != 0)
		{
			output.WriteRawTag(26);
			output.WriteString(MapName);
		}
		if (ConversionRate != 0.0)
		{
			output.WriteRawTag(33);
			output.WriteDouble(ConversionRate);
		}
		if (Intensity != 0.0)
		{
			output.WriteRawTag(41);
			output.WriteDouble(Intensity);
		}
		if (Tenacity != 0.0)
		{
			output.WriteRawTag(49);
			output.WriteDouble(Tenacity);
		}
		if (Affinity != 0.0)
		{
			output.WriteRawTag(57);
			output.WriteDouble(Affinity);
		}
		if (Weight != 0.0)
		{
			output.WriteRawTag(65);
			output.WriteDouble(Weight);
		}
		if (Volume != 0.0)
		{
			output.WriteRawTag(73);
			output.WriteDouble(Volume);
		}
		if (Unit.Length != 0)
		{
			output.WriteRawTag(82);
			output.WriteString(Unit);
		}
		if (ReceiveVal != 0.0)
		{
			output.WriteRawTag(89);
			output.WriteDouble(ReceiveVal);
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
		if (MapName.Length != 0)
		{
			num += 1 + CodedOutputStream.ComputeStringSize(MapName);
		}
		if (ConversionRate != 0.0)
		{
			num += 9;
		}
		if (Intensity != 0.0)
		{
			num += 9;
		}
		if (Tenacity != 0.0)
		{
			num += 9;
		}
		if (Affinity != 0.0)
		{
			num += 9;
		}
		if (Weight != 0.0)
		{
			num += 9;
		}
		if (Volume != 0.0)
		{
			num += 9;
		}
		if (Unit.Length != 0)
		{
			num += 1 + CodedOutputStream.ComputeStringSize(Unit);
		}
		if (ReceiveVal != 0.0)
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
	public void MergeFrom(GoodsMaterialMineralDetailResp_1004006 other)
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
			if (other.MapName.Length != 0)
			{
				MapName = other.MapName;
			}
			if (other.ConversionRate != 0.0)
			{
				ConversionRate = other.ConversionRate;
			}
			if (other.Intensity != 0.0)
			{
				Intensity = other.Intensity;
			}
			if (other.Tenacity != 0.0)
			{
				Tenacity = other.Tenacity;
			}
			if (other.Affinity != 0.0)
			{
				Affinity = other.Affinity;
			}
			if (other.Weight != 0.0)
			{
				Weight = other.Weight;
			}
			if (other.Volume != 0.0)
			{
				Volume = other.Volume;
			}
			if (other.Unit.Length != 0)
			{
				Unit = other.Unit;
			}
			if (other.ReceiveVal != 0.0)
			{
				ReceiveVal = other.ReceiveVal;
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
			case 26u:
				MapName = input.ReadString();
				break;
			case 33u:
				ConversionRate = input.ReadDouble();
				break;
			case 41u:
				Intensity = input.ReadDouble();
				break;
			case 49u:
				Tenacity = input.ReadDouble();
				break;
			case 57u:
				Affinity = input.ReadDouble();
				break;
			case 65u:
				Weight = input.ReadDouble();
				break;
			case 73u:
				Volume = input.ReadDouble();
				break;
			case 82u:
				Unit = input.ReadString();
				break;
			case 89u:
				ReceiveVal = input.ReadDouble();
				break;
			}
		}
	}
}
