using System;
using System.Diagnostics;
using Google.Protobuf;
using Google.Protobuf.Reflection;

public sealed class RechooseAchievementEffectResp_1005042 : IMessage<RechooseAchievementEffectResp_1005042>, IMessage, IEquatable<RechooseAchievementEffectResp_1005042>, IDeepCloneable<RechooseAchievementEffectResp_1005042>
{
	private static readonly MessageParser<RechooseAchievementEffectResp_1005042> _parser = new MessageParser<RechooseAchievementEffectResp_1005042>(() => new RechooseAchievementEffectResp_1005042());

	private UnknownFieldSet _unknownFields;

	public const int CodeFieldNumber = 1;

	private int code_;

	public const int IsSuccessFieldNumber = 2;

	private bool isSuccess_;

	public const int EffectType1FieldNumber = 3;

	private int effectType1_;

	public const int EffectValue1FieldNumber = 4;

	private int effectValue1_;

	public const int EffectType2FieldNumber = 5;

	private int effectType2_;

	public const int EffectValue2FieldNumber = 6;

	private int effectValue2_;

	public const int EffectType3FieldNumber = 7;

	private int effectType3_;

	public const int EffectValue3FieldNumber = 8;

	private int effectValue3_;

	public const int ShinyStoneFieldNumber = 9;

	private long shinyStone_;

	public const int WudaoStoneFieldNumber = 10;

	private int wudaoStone_;

	[DebuggerNonUserCode]
	public static MessageParser<RechooseAchievementEffectResp_1005042> Parser => _parser;

	[DebuggerNonUserCode]
	public static MessageDescriptor Descriptor => HomeProtoReflection.Descriptor.MessageTypes[54];

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
	public long ShinyStone
	{
		get
		{
			return shinyStone_;
		}
		set
		{
			shinyStone_ = value;
		}
	}

	[DebuggerNonUserCode]
	public int WudaoStone
	{
		get
		{
			return wudaoStone_;
		}
		set
		{
			wudaoStone_ = value;
		}
	}

	private MessageDescriptor pb_003A_003AGoogle_002EProtobuf_002EIMessage_002Eget_Descriptor()
	{
		return Descriptor;
	}

	[DebuggerNonUserCode]
	public RechooseAchievementEffectResp_1005042()
	{
	}

	[DebuggerNonUserCode]
	public RechooseAchievementEffectResp_1005042(RechooseAchievementEffectResp_1005042 other)
		: this()
	{
		code_ = other.code_;
		isSuccess_ = other.isSuccess_;
		effectType1_ = other.effectType1_;
		effectValue1_ = other.effectValue1_;
		effectType2_ = other.effectType2_;
		effectValue2_ = other.effectValue2_;
		effectType3_ = other.effectType3_;
		effectValue3_ = other.effectValue3_;
		shinyStone_ = other.shinyStone_;
		wudaoStone_ = other.wudaoStone_;
		_unknownFields = UnknownFieldSet.Clone(other._unknownFields);
	}

	[DebuggerNonUserCode]
	public RechooseAchievementEffectResp_1005042 Clone()
	{
		return new RechooseAchievementEffectResp_1005042(this);
	}

	[DebuggerNonUserCode]
	public override bool Equals(object other)
	{
		return Equals(other as RechooseAchievementEffectResp_1005042);
	}

	[DebuggerNonUserCode]
	public bool Equals(RechooseAchievementEffectResp_1005042 other)
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
		if (ShinyStone != other.ShinyStone)
		{
			return false;
		}
		if (WudaoStone != other.WudaoStone)
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
		if (ShinyStone != 0L)
		{
			num ^= ShinyStone.GetHashCode();
		}
		if (WudaoStone != 0)
		{
			num ^= WudaoStone.GetHashCode();
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
		if (EffectType1 != 0)
		{
			output.WriteRawTag(24);
			output.WriteInt32(EffectType1);
		}
		if (EffectValue1 != 0)
		{
			output.WriteRawTag(32);
			output.WriteInt32(EffectValue1);
		}
		if (EffectType2 != 0)
		{
			output.WriteRawTag(40);
			output.WriteInt32(EffectType2);
		}
		if (EffectValue2 != 0)
		{
			output.WriteRawTag(48);
			output.WriteInt32(EffectValue2);
		}
		if (EffectType3 != 0)
		{
			output.WriteRawTag(56);
			output.WriteInt32(EffectType3);
		}
		if (EffectValue3 != 0)
		{
			output.WriteRawTag(64);
			output.WriteInt32(EffectValue3);
		}
		if (ShinyStone != 0L)
		{
			output.WriteRawTag(72);
			output.WriteInt64(ShinyStone);
		}
		if (WudaoStone != 0)
		{
			output.WriteRawTag(80);
			output.WriteInt32(WudaoStone);
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
		if (ShinyStone != 0L)
		{
			num += 1 + CodedOutputStream.ComputeInt64Size(ShinyStone);
		}
		if (WudaoStone != 0)
		{
			num += 1 + CodedOutputStream.ComputeInt32Size(WudaoStone);
		}
		if (_unknownFields != null)
		{
			num += _unknownFields.CalculateSize();
		}
		return num;
	}

	[DebuggerNonUserCode]
	public void MergeFrom(RechooseAchievementEffectResp_1005042 other)
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
			if (other.ShinyStone != 0L)
			{
				ShinyStone = other.ShinyStone;
			}
			if (other.WudaoStone != 0)
			{
				WudaoStone = other.WudaoStone;
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
				EffectType1 = input.ReadInt32();
				break;
			case 32u:
				EffectValue1 = input.ReadInt32();
				break;
			case 40u:
				EffectType2 = input.ReadInt32();
				break;
			case 48u:
				EffectValue2 = input.ReadInt32();
				break;
			case 56u:
				EffectType3 = input.ReadInt32();
				break;
			case 64u:
				EffectValue3 = input.ReadInt32();
				break;
			case 72u:
				ShinyStone = input.ReadInt64();
				break;
			case 80u:
				WudaoStone = input.ReadInt32();
				break;
			}
		}
	}
}
