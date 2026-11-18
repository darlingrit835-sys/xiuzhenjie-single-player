using System;
using System.Diagnostics;
using Google.Protobuf;
using Google.Protobuf.Collections;
using Google.Protobuf.Reflection;

public sealed class UpgradeAchievementConversionRateResp_1001020 : IMessage<UpgradeAchievementConversionRateResp_1001020>, IMessage, IEquatable<UpgradeAchievementConversionRateResp_1001020>, IDeepCloneable<UpgradeAchievementConversionRateResp_1001020>
{
	private static readonly MessageParser<UpgradeAchievementConversionRateResp_1001020> _parser = new MessageParser<UpgradeAchievementConversionRateResp_1001020>(() => new UpgradeAchievementConversionRateResp_1001020());

	private UnknownFieldSet _unknownFields;

	public const int CodeFieldNumber = 1;

	private int code_;

	public const int IsSuccessFieldNumber = 2;

	private bool isSuccess_;

	public const int RankFieldNumber = 3;

	private int rank_;

	public const int ConversionRateFieldNumber = 4;

	private double conversionRate_;

	public const int EffectType1FieldNumber = 5;

	private int effectType1_;

	public const int EffectValue1FieldNumber = 6;

	private int effectValue1_;

	public const int EffectType2FieldNumber = 7;

	private int effectType2_;

	public const int EffectValue2FieldNumber = 8;

	private int effectValue2_;

	public const int EffectType3FieldNumber = 9;

	private int effectType3_;

	public const int EffectValue3FieldNumber = 10;

	private int effectValue3_;

	public const int NowCapacityFieldNumber = 11;

	private int nowCapacity_;

	[DebuggerNonUserCode]
	public static MessageParser<UpgradeAchievementConversionRateResp_1001020> Parser => _parser;

	[DebuggerNonUserCode]
	public static MessageDescriptor Descriptor => SimpleActionProtoReflection.Descriptor.MessageTypes[24];

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
	public int Rank
	{
		get
		{
			return rank_;
		}
		set
		{
			rank_ = value;
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
	public int EffectType1
	{
		get
		{
			return effectType1_;
		}
		set
		{
			effectType1_ = value;
		}
	}

	[DebuggerNonUserCode]
	public int EffectValue1
	{
		get
		{
			return effectValue1_;
		}
		set
		{
			effectValue1_ = value;
		}
	}

	[DebuggerNonUserCode]
	public int EffectType2
	{
		get
		{
			return effectType2_;
		}
		set
		{
			effectType2_ = value;
		}
	}

	[DebuggerNonUserCode]
	public int EffectValue2
	{
		get
		{
			return effectValue2_;
		}
		set
		{
			effectValue2_ = value;
		}
	}

	[DebuggerNonUserCode]
	public int EffectType3
	{
		get
		{
			return effectType3_;
		}
		set
		{
			effectType3_ = value;
		}
	}

	[DebuggerNonUserCode]
	public int EffectValue3
	{
		get
		{
			return effectValue3_;
		}
		set
		{
			effectValue3_ = value;
		}
	}

	[DebuggerNonUserCode]
	public int NowCapacity
	{
		get
		{
			return nowCapacity_;
		}
		set
		{
			nowCapacity_ = value;
		}
	}

	private MessageDescriptor pb_003A_003AGoogle_002EProtobuf_002EIMessage_002Eget_Descriptor()
	{
		return Descriptor;
	}

	[DebuggerNonUserCode]
	public UpgradeAchievementConversionRateResp_1001020()
	{
	}

	[DebuggerNonUserCode]
	public UpgradeAchievementConversionRateResp_1001020(UpgradeAchievementConversionRateResp_1001020 other)
		: this()
	{
		code_ = other.code_;
		isSuccess_ = other.isSuccess_;
		rank_ = other.rank_;
		conversionRate_ = other.conversionRate_;
		effectType1_ = other.effectType1_;
		effectValue1_ = other.effectValue1_;
		effectType2_ = other.effectType2_;
		effectValue2_ = other.effectValue2_;
		effectType3_ = other.effectType3_;
		effectValue3_ = other.effectValue3_;
		nowCapacity_ = other.nowCapacity_;
		_unknownFields = UnknownFieldSet.Clone(other._unknownFields);
	}

	[DebuggerNonUserCode]
	public UpgradeAchievementConversionRateResp_1001020 Clone()
	{
		return new UpgradeAchievementConversionRateResp_1001020(this);
	}

	[DebuggerNonUserCode]
	public override bool Equals(object other)
	{
		return Equals(other as UpgradeAchievementConversionRateResp_1001020);
	}

	[DebuggerNonUserCode]
	public bool Equals(UpgradeAchievementConversionRateResp_1001020 other)
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
		if (Rank != other.Rank)
		{
			return false;
		}
		if (!ProtobufEqualityComparers.BitwiseDoubleEqualityComparer.Equals(ConversionRate, other.ConversionRate))
		{
			return false;
		}
		if (EffectType1 != other.EffectType1)
		{
			return false;
		}
		if (EffectValue1 != other.EffectValue1)
		{
			return false;
		}
		if (EffectType2 != other.EffectType2)
		{
			return false;
		}
		if (EffectValue2 != other.EffectValue2)
		{
			return false;
		}
		if (EffectType3 != other.EffectType3)
		{
			return false;
		}
		if (EffectValue3 != other.EffectValue3)
		{
			return false;
		}
		if (NowCapacity != other.NowCapacity)
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
		if (Rank != 0)
		{
			num ^= Rank.GetHashCode();
		}
		if (ConversionRate != 0.0)
		{
			num ^= ProtobufEqualityComparers.BitwiseDoubleEqualityComparer.GetHashCode(ConversionRate);
		}
		if (EffectType1 != 0)
		{
			num ^= EffectType1.GetHashCode();
		}
		if (EffectValue1 != 0)
		{
			num ^= EffectValue1.GetHashCode();
		}
		if (EffectType2 != 0)
		{
			num ^= EffectType2.GetHashCode();
		}
		if (EffectValue2 != 0)
		{
			num ^= EffectValue2.GetHashCode();
		}
		if (EffectType3 != 0)
		{
			num ^= EffectType3.GetHashCode();
		}
		if (EffectValue3 != 0)
		{
			num ^= EffectValue3.GetHashCode();
		}
		if (NowCapacity != 0)
		{
			num ^= NowCapacity.GetHashCode();
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
		if (Rank != 0)
		{
			output.WriteRawTag(24);
			output.WriteInt32(Rank);
		}
		if (ConversionRate != 0.0)
		{
			output.WriteRawTag(33);
			output.WriteDouble(ConversionRate);
		}
		if (EffectType1 != 0)
		{
			output.WriteRawTag(40);
			output.WriteInt32(EffectType1);
		}
		if (EffectValue1 != 0)
		{
			output.WriteRawTag(48);
			output.WriteInt32(EffectValue1);
		}
		if (EffectType2 != 0)
		{
			output.WriteRawTag(56);
			output.WriteInt32(EffectType2);
		}
		if (EffectValue2 != 0)
		{
			output.WriteRawTag(64);
			output.WriteInt32(EffectValue2);
		}
		if (EffectType3 != 0)
		{
			output.WriteRawTag(72);
			output.WriteInt32(EffectType3);
		}
		if (EffectValue3 != 0)
		{
			output.WriteRawTag(80);
			output.WriteInt32(EffectValue3);
		}
		if (NowCapacity != 0)
		{
			output.WriteRawTag(88);
			output.WriteInt32(NowCapacity);
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
		if (Rank != 0)
		{
			num += 1 + CodedOutputStream.ComputeInt32Size(Rank);
		}
		if (ConversionRate != 0.0)
		{
			num += 9;
		}
		if (EffectType1 != 0)
		{
			num += 1 + CodedOutputStream.ComputeInt32Size(EffectType1);
		}
		if (EffectValue1 != 0)
		{
			num += 1 + CodedOutputStream.ComputeInt32Size(EffectValue1);
		}
		if (EffectType2 != 0)
		{
			num += 1 + CodedOutputStream.ComputeInt32Size(EffectType2);
		}
		if (EffectValue2 != 0)
		{
			num += 1 + CodedOutputStream.ComputeInt32Size(EffectValue2);
		}
		if (EffectType3 != 0)
		{
			num += 1 + CodedOutputStream.ComputeInt32Size(EffectType3);
		}
		if (EffectValue3 != 0)
		{
			num += 1 + CodedOutputStream.ComputeInt32Size(EffectValue3);
		}
		if (NowCapacity != 0)
		{
			num += 1 + CodedOutputStream.ComputeInt32Size(NowCapacity);
		}
		if (_unknownFields != null)
		{
			num += _unknownFields.CalculateSize();
		}
		return num;
	}

	[DebuggerNonUserCode]
	public void MergeFrom(UpgradeAchievementConversionRateResp_1001020 other)
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
			if (other.Rank != 0)
			{
				Rank = other.Rank;
			}
			if (other.ConversionRate != 0.0)
			{
				ConversionRate = other.ConversionRate;
			}
			if (other.EffectType1 != 0)
			{
				EffectType1 = other.EffectType1;
			}
			if (other.EffectValue1 != 0)
			{
				EffectValue1 = other.EffectValue1;
			}
			if (other.EffectType2 != 0)
			{
				EffectType2 = other.EffectType2;
			}
			if (other.EffectValue2 != 0)
			{
				EffectValue2 = other.EffectValue2;
			}
			if (other.EffectType3 != 0)
			{
				EffectType3 = other.EffectType3;
			}
			if (other.EffectValue3 != 0)
			{
				EffectValue3 = other.EffectValue3;
			}
			if (other.NowCapacity != 0)
			{
				NowCapacity = other.NowCapacity;
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
				Rank = input.ReadInt32();
				break;
			case 33u:
				ConversionRate = input.ReadDouble();
				break;
			case 40u:
				EffectType1 = input.ReadInt32();
				break;
			case 48u:
				EffectValue1 = input.ReadInt32();
				break;
			case 56u:
				EffectType2 = input.ReadInt32();
				break;
			case 64u:
				EffectValue2 = input.ReadInt32();
				break;
			case 72u:
				EffectType3 = input.ReadInt32();
				break;
			case 80u:
				EffectValue3 = input.ReadInt32();
				break;
			case 88u:
				NowCapacity = input.ReadInt32();
				break;
			}
		}
	}
}
