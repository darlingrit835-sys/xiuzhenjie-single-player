using System;
using System.Diagnostics;
using Google.Protobuf;
using Google.Protobuf.Reflection;

public sealed class EnterWuDaoTaiResp_1005041 : IMessage<EnterWuDaoTaiResp_1005041>, IMessage, IEquatable<EnterWuDaoTaiResp_1005041>, IDeepCloneable<EnterWuDaoTaiResp_1005041>
{
	private static readonly MessageParser<EnterWuDaoTaiResp_1005041> _parser = new MessageParser<EnterWuDaoTaiResp_1005041>(() => new EnterWuDaoTaiResp_1005041());

	private UnknownFieldSet _unknownFields;

	public const int CodeFieldNumber = 1;

	private int code_;

	public const int IsSuccessFieldNumber = 2;

	private bool isSuccess_;

	public const int AchievementNameFieldNumber = 3;

	private string achievementName_ = "";

	public const int RechooseCountFieldNumber = 4;

	private int rechooseCount_;

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

	public const int StageFieldNumber = 11;

	private int stage_;

	[DebuggerNonUserCode]
	public static MessageParser<EnterWuDaoTaiResp_1005041> Parser => _parser;

	[DebuggerNonUserCode]
	public static MessageDescriptor Descriptor => HomeProtoReflection.Descriptor.MessageTypes[52];

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
	public string AchievementName
	{
		get
		{
			return achievementName_;
		}
		set
		{
			achievementName_ = ProtoPreconditions.CheckNotNull(value, "value");
		}
	}

	[DebuggerNonUserCode]
	public int RechooseCount
	{
		get
		{
			return rechooseCount_;
		}
		set
		{
			rechooseCount_ = value;
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
	public int Stage
	{
		get
		{
			return stage_;
		}
		set
		{
			stage_ = value;
		}
	}

	private MessageDescriptor pb_003A_003AGoogle_002EProtobuf_002EIMessage_002Eget_Descriptor()
	{
		return Descriptor;
	}

	[DebuggerNonUserCode]
	public EnterWuDaoTaiResp_1005041()
	{
	}

	[DebuggerNonUserCode]
	public EnterWuDaoTaiResp_1005041(EnterWuDaoTaiResp_1005041 other)
		: this()
	{
		code_ = other.code_;
		isSuccess_ = other.isSuccess_;
		achievementName_ = other.achievementName_;
		rechooseCount_ = other.rechooseCount_;
		effectType1_ = other.effectType1_;
		effectValue1_ = other.effectValue1_;
		effectType2_ = other.effectType2_;
		effectValue2_ = other.effectValue2_;
		effectType3_ = other.effectType3_;
		effectValue3_ = other.effectValue3_;
		stage_ = other.stage_;
		_unknownFields = UnknownFieldSet.Clone(other._unknownFields);
	}

	[DebuggerNonUserCode]
	public EnterWuDaoTaiResp_1005041 Clone()
	{
		return new EnterWuDaoTaiResp_1005041(this);
	}

	[DebuggerNonUserCode]
	public override bool Equals(object other)
	{
		return Equals(other as EnterWuDaoTaiResp_1005041);
	}

	[DebuggerNonUserCode]
	public bool Equals(EnterWuDaoTaiResp_1005041 other)
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
		if (AchievementName != other.AchievementName)
		{
			return false;
		}
		if (RechooseCount != other.RechooseCount)
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
		if (Stage != other.Stage)
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
		if (AchievementName.Length != 0)
		{
			num ^= AchievementName.GetHashCode();
		}
		if (RechooseCount != 0)
		{
			num ^= RechooseCount.GetHashCode();
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
		if (Stage != 0)
		{
			num ^= Stage.GetHashCode();
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
		if (AchievementName.Length != 0)
		{
			output.WriteRawTag(26);
			output.WriteString(AchievementName);
		}
		if (RechooseCount != 0)
		{
			output.WriteRawTag(32);
			output.WriteInt32(RechooseCount);
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
		if (Stage != 0)
		{
			output.WriteRawTag(88);
			output.WriteInt32(Stage);
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
		if (AchievementName.Length != 0)
		{
			num += 1 + CodedOutputStream.ComputeStringSize(AchievementName);
		}
		if (RechooseCount != 0)
		{
			num += 1 + CodedOutputStream.ComputeInt32Size(RechooseCount);
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
		if (Stage != 0)
		{
			num += 1 + CodedOutputStream.ComputeInt32Size(Stage);
		}
		if (_unknownFields != null)
		{
			num += _unknownFields.CalculateSize();
		}
		return num;
	}

	[DebuggerNonUserCode]
	public void MergeFrom(EnterWuDaoTaiResp_1005041 other)
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
			if (other.AchievementName.Length != 0)
			{
				AchievementName = other.AchievementName;
			}
			if (other.RechooseCount != 0)
			{
				RechooseCount = other.RechooseCount;
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
			if (other.Stage != 0)
			{
				Stage = other.Stage;
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
				AchievementName = input.ReadString();
				break;
			case 32u:
				RechooseCount = input.ReadInt32();
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
				Stage = input.ReadInt32();
				break;
			}
		}
	}
}
