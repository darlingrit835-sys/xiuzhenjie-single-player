using System;
using System.Diagnostics;
using Google.Protobuf;
using Google.Protobuf.Reflection;

public sealed class RechooseAchievementEffectReq_1005042 : IMessage<RechooseAchievementEffectReq_1005042>, IMessage, IEquatable<RechooseAchievementEffectReq_1005042>, IDeepCloneable<RechooseAchievementEffectReq_1005042>
{
	private static readonly MessageParser<RechooseAchievementEffectReq_1005042> _parser = new MessageParser<RechooseAchievementEffectReq_1005042>(() => new RechooseAchievementEffectReq_1005042());

	private UnknownFieldSet _unknownFields;

	public const int PayTypeFieldNumber = 1;

	private int payType_;

	public const int IsLock1FieldNumber = 2;

	private bool isLock1_;

	public const int IsLock2FieldNumber = 3;

	private bool isLock2_;

	public const int IsLock3FieldNumber = 4;

	private bool isLock3_;

	[DebuggerNonUserCode]
	public static MessageParser<RechooseAchievementEffectReq_1005042> Parser => _parser;

	[DebuggerNonUserCode]
	public static MessageDescriptor Descriptor => HomeProtoReflection.Descriptor.MessageTypes[53];

	[DebuggerNonUserCode]
	MessageDescriptor IMessage.Descriptor => pb_003A_003AGoogle_002EProtobuf_002EIMessage_002Eget_Descriptor();

	[DebuggerNonUserCode]
	public int PayType
	{
		get
		{
			return payType_;
		}
		set
		{
			payType_ = value;
		}
	}

	[DebuggerNonUserCode]
	public bool IsLock1
	{
		get
		{
			return isLock1_;
		}
		set
		{
			isLock1_ = value;
		}
	}

	[DebuggerNonUserCode]
	public bool IsLock2
	{
		get
		{
			return isLock2_;
		}
		set
		{
			isLock2_ = value;
		}
	}

	[DebuggerNonUserCode]
	public bool IsLock3
	{
		get
		{
			return isLock3_;
		}
		set
		{
			isLock3_ = value;
		}
	}

	private MessageDescriptor pb_003A_003AGoogle_002EProtobuf_002EIMessage_002Eget_Descriptor()
	{
		return Descriptor;
	}

	[DebuggerNonUserCode]
	public RechooseAchievementEffectReq_1005042()
	{
	}

	[DebuggerNonUserCode]
	public RechooseAchievementEffectReq_1005042(RechooseAchievementEffectReq_1005042 other)
		: this()
	{
		payType_ = other.payType_;
		isLock1_ = other.isLock1_;
		isLock2_ = other.isLock2_;
		isLock3_ = other.isLock3_;
		_unknownFields = UnknownFieldSet.Clone(other._unknownFields);
	}

	[DebuggerNonUserCode]
	public RechooseAchievementEffectReq_1005042 Clone()
	{
		return new RechooseAchievementEffectReq_1005042(this);
	}

	[DebuggerNonUserCode]
	public override bool Equals(object other)
	{
		return Equals(other as RechooseAchievementEffectReq_1005042);
	}

	[DebuggerNonUserCode]
	public bool Equals(RechooseAchievementEffectReq_1005042 other)
	{
		if (other == null)
		{
			return false;
		}
		if (other == this)
		{
			return true;
		}
		if (PayType != other.PayType)
		{
			return false;
		}
		if (IsLock1 != other.IsLock1)
		{
			return false;
		}
		if (IsLock2 != other.IsLock2)
		{
			return false;
		}
		if (IsLock3 != other.IsLock3)
		{
			return false;
		}
		return object.Equals(_unknownFields, other._unknownFields);
	}

	[DebuggerNonUserCode]
	public override int GetHashCode()
	{
		int num = 1;
		if (PayType != 0)
		{
			num ^= PayType.GetHashCode();
		}
		if (IsLock1)
		{
			num ^= IsLock1.GetHashCode();
		}
		if (IsLock2)
		{
			num ^= IsLock2.GetHashCode();
		}
		if (IsLock3)
		{
			num ^= IsLock3.GetHashCode();
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
		if (PayType != 0)
		{
			output.WriteRawTag(8);
			output.WriteInt32(PayType);
		}
		if (IsLock1)
		{
			output.WriteRawTag(16);
			output.WriteBool(IsLock1);
		}
		if (IsLock2)
		{
			output.WriteRawTag(24);
			output.WriteBool(IsLock2);
		}
		if (IsLock3)
		{
			output.WriteRawTag(32);
			output.WriteBool(IsLock3);
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
		if (PayType != 0)
		{
			num += 1 + CodedOutputStream.ComputeInt32Size(PayType);
		}
		if (IsLock1)
		{
			num += 2;
		}
		if (IsLock2)
		{
			num += 2;
		}
		if (IsLock3)
		{
			num += 2;
		}
		if (_unknownFields != null)
		{
			num += _unknownFields.CalculateSize();
		}
		return num;
	}

	[DebuggerNonUserCode]
	public void MergeFrom(RechooseAchievementEffectReq_1005042 other)
	{
		if (other != null)
		{
			if (other.PayType != 0)
			{
				PayType = other.PayType;
			}
			if (other.IsLock1)
			{
				IsLock1 = other.IsLock1;
			}
			if (other.IsLock2)
			{
				IsLock2 = other.IsLock2;
			}
			if (other.IsLock3)
			{
				IsLock3 = other.IsLock3;
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
				PayType = input.ReadInt32();
				break;
			case 16u:
				IsLock1 = input.ReadBool();
				break;
			case 24u:
				IsLock2 = input.ReadBool();
				break;
			case 32u:
				IsLock3 = input.ReadBool();
				break;
			}
		}
	}
}
