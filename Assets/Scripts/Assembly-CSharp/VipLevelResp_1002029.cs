using System;
using System.Diagnostics;
using Google.Protobuf;
using Google.Protobuf.Reflection;

public sealed class VipLevelResp_1002029 : IMessage<VipLevelResp_1002029>, IMessage, IEquatable<VipLevelResp_1002029>, IDeepCloneable<VipLevelResp_1002029>
{
	private static readonly MessageParser<VipLevelResp_1002029> _parser = new MessageParser<VipLevelResp_1002029>(() => new VipLevelResp_1002029());

	private UnknownFieldSet _unknownFields;

	public const int CodeFieldNumber = 1;

	private int code_;

	public const int IsSuccessFieldNumber = 2;

	private bool isSuccess_;

	public const int LevelFieldNumber = 3;

	private int level_;

	public const int PayAllFieldNumber = 4;

	private int payAll_;

	public const int V1IsGetedFieldNumber = 5;

	private bool v1IsGeted_;

	public const int V2IsGetedFieldNumber = 6;

	private bool v2IsGeted_;

	public const int V3IsGetedFieldNumber = 7;

	private bool v3IsGeted_;

	public const int V4IsGetedFieldNumber = 8;

	private bool v4IsGeted_;

	public const int V5IsGetedFieldNumber = 9;

	private bool v5IsGeted_;

	public const int V6IsGetedFieldNumber = 10;

	private bool v6IsGeted_;

	public const int V7IsGetedFieldNumber = 11;

	private bool v7IsGeted_;

	public const int V8IsGetedFieldNumber = 12;

	private bool v8IsGeted_;

	public const int V9IsGetedFieldNumber = 13;

	private bool v9IsGeted_;

	public const int V10IsGetedFieldNumber = 14;

	private bool v10IsGeted_;

	public const int V11IsGetedFieldNumber = 15;

	private bool v11IsGeted_;

	public const int V12IsGetedFieldNumber = 16;

	private bool v12IsGeted_;

	[DebuggerNonUserCode]
	public static MessageParser<VipLevelResp_1002029> Parser => _parser;

	[DebuggerNonUserCode]
	public static MessageDescriptor Descriptor => RoleProtoReflection.Descriptor.MessageTypes[34];

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
	public int Level
	{
		get
		{
			return level_;
		}
		set
		{
			level_ = value;
		}
	}

	[DebuggerNonUserCode]
	public int PayAll
	{
		get
		{
			return payAll_;
		}
		set
		{
			payAll_ = value;
		}
	}

	[DebuggerNonUserCode]
	public bool V1IsGeted
	{
		get
		{
			return v1IsGeted_;
		}
		set
		{
			v1IsGeted_ = value;
		}
	}

	[DebuggerNonUserCode]
	public bool V2IsGeted
	{
		get
		{
			return v2IsGeted_;
		}
		set
		{
			v2IsGeted_ = value;
		}
	}

	[DebuggerNonUserCode]
	public bool V3IsGeted
	{
		get
		{
			return v3IsGeted_;
		}
		set
		{
			v3IsGeted_ = value;
		}
	}

	[DebuggerNonUserCode]
	public bool V4IsGeted
	{
		get
		{
			return v4IsGeted_;
		}
		set
		{
			v4IsGeted_ = value;
		}
	}

	[DebuggerNonUserCode]
	public bool V5IsGeted
	{
		get
		{
			return v5IsGeted_;
		}
		set
		{
			v5IsGeted_ = value;
		}
	}

	[DebuggerNonUserCode]
	public bool V6IsGeted
	{
		get
		{
			return v6IsGeted_;
		}
		set
		{
			v6IsGeted_ = value;
		}
	}

	[DebuggerNonUserCode]
	public bool V7IsGeted
	{
		get
		{
			return v7IsGeted_;
		}
		set
		{
			v7IsGeted_ = value;
		}
	}

	[DebuggerNonUserCode]
	public bool V8IsGeted
	{
		get
		{
			return v8IsGeted_;
		}
		set
		{
			v8IsGeted_ = value;
		}
	}

	[DebuggerNonUserCode]
	public bool V9IsGeted
	{
		get
		{
			return v9IsGeted_;
		}
		set
		{
			v9IsGeted_ = value;
		}
	}

	[DebuggerNonUserCode]
	public bool V10IsGeted
	{
		get
		{
			return v10IsGeted_;
		}
		set
		{
			v10IsGeted_ = value;
		}
	}

	[DebuggerNonUserCode]
	public bool V11IsGeted
	{
		get
		{
			return v11IsGeted_;
		}
		set
		{
			v11IsGeted_ = value;
		}
	}

	[DebuggerNonUserCode]
	public bool V12IsGeted
	{
		get
		{
			return v12IsGeted_;
		}
		set
		{
			v12IsGeted_ = value;
		}
	}

	private MessageDescriptor pb_003A_003AGoogle_002EProtobuf_002EIMessage_002Eget_Descriptor()
	{
		return Descriptor;
	}

	[DebuggerNonUserCode]
	public VipLevelResp_1002029()
	{
	}

	[DebuggerNonUserCode]
	public VipLevelResp_1002029(VipLevelResp_1002029 other)
		: this()
	{
		code_ = other.code_;
		isSuccess_ = other.isSuccess_;
		level_ = other.level_;
		payAll_ = other.payAll_;
		v1IsGeted_ = other.v1IsGeted_;
		v2IsGeted_ = other.v2IsGeted_;
		v3IsGeted_ = other.v3IsGeted_;
		v4IsGeted_ = other.v4IsGeted_;
		v5IsGeted_ = other.v5IsGeted_;
		v6IsGeted_ = other.v6IsGeted_;
		v7IsGeted_ = other.v7IsGeted_;
		v8IsGeted_ = other.v8IsGeted_;
		v9IsGeted_ = other.v9IsGeted_;
		v10IsGeted_ = other.v10IsGeted_;
		v11IsGeted_ = other.v11IsGeted_;
		v12IsGeted_ = other.v12IsGeted_;
		_unknownFields = UnknownFieldSet.Clone(other._unknownFields);
	}

	[DebuggerNonUserCode]
	public VipLevelResp_1002029 Clone()
	{
		return new VipLevelResp_1002029(this);
	}

	[DebuggerNonUserCode]
	public override bool Equals(object other)
	{
		return Equals(other as VipLevelResp_1002029);
	}

	[DebuggerNonUserCode]
	public bool Equals(VipLevelResp_1002029 other)
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
		if (Level != other.Level)
		{
			return false;
		}
		if (PayAll != other.PayAll)
		{
			return false;
		}
		if (V1IsGeted != other.V1IsGeted)
		{
			return false;
		}
		if (V2IsGeted != other.V2IsGeted)
		{
			return false;
		}
		if (V3IsGeted != other.V3IsGeted)
		{
			return false;
		}
		if (V4IsGeted != other.V4IsGeted)
		{
			return false;
		}
		if (V5IsGeted != other.V5IsGeted)
		{
			return false;
		}
		if (V6IsGeted != other.V6IsGeted)
		{
			return false;
		}
		if (V7IsGeted != other.V7IsGeted)
		{
			return false;
		}
		if (V8IsGeted != other.V8IsGeted)
		{
			return false;
		}
		if (V9IsGeted != other.V9IsGeted)
		{
			return false;
		}
		if (V10IsGeted != other.V10IsGeted)
		{
			return false;
		}
		if (V11IsGeted != other.V11IsGeted)
		{
			return false;
		}
		if (V12IsGeted != other.V12IsGeted)
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
		if (Level != 0)
		{
			num ^= Level.GetHashCode();
		}
		if (PayAll != 0)
		{
			num ^= PayAll.GetHashCode();
		}
		if (V1IsGeted)
		{
			num ^= V1IsGeted.GetHashCode();
		}
		if (V2IsGeted)
		{
			num ^= V2IsGeted.GetHashCode();
		}
		if (V3IsGeted)
		{
			num ^= V3IsGeted.GetHashCode();
		}
		if (V4IsGeted)
		{
			num ^= V4IsGeted.GetHashCode();
		}
		if (V5IsGeted)
		{
			num ^= V5IsGeted.GetHashCode();
		}
		if (V6IsGeted)
		{
			num ^= V6IsGeted.GetHashCode();
		}
		if (V7IsGeted)
		{
			num ^= V7IsGeted.GetHashCode();
		}
		if (V8IsGeted)
		{
			num ^= V8IsGeted.GetHashCode();
		}
		if (V9IsGeted)
		{
			num ^= V9IsGeted.GetHashCode();
		}
		if (V10IsGeted)
		{
			num ^= V10IsGeted.GetHashCode();
		}
		if (V11IsGeted)
		{
			num ^= V11IsGeted.GetHashCode();
		}
		if (V12IsGeted)
		{
			num ^= V12IsGeted.GetHashCode();
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
		if (Level != 0)
		{
			output.WriteRawTag(24);
			output.WriteInt32(Level);
		}
		if (PayAll != 0)
		{
			output.WriteRawTag(32);
			output.WriteInt32(PayAll);
		}
		if (V1IsGeted)
		{
			output.WriteRawTag(40);
			output.WriteBool(V1IsGeted);
		}
		if (V2IsGeted)
		{
			output.WriteRawTag(48);
			output.WriteBool(V2IsGeted);
		}
		if (V3IsGeted)
		{
			output.WriteRawTag(56);
			output.WriteBool(V3IsGeted);
		}
		if (V4IsGeted)
		{
			output.WriteRawTag(64);
			output.WriteBool(V4IsGeted);
		}
		if (V5IsGeted)
		{
			output.WriteRawTag(72);
			output.WriteBool(V5IsGeted);
		}
		if (V6IsGeted)
		{
			output.WriteRawTag(80);
			output.WriteBool(V6IsGeted);
		}
		if (V7IsGeted)
		{
			output.WriteRawTag(88);
			output.WriteBool(V7IsGeted);
		}
		if (V8IsGeted)
		{
			output.WriteRawTag(96);
			output.WriteBool(V8IsGeted);
		}
		if (V9IsGeted)
		{
			output.WriteRawTag(104);
			output.WriteBool(V9IsGeted);
		}
		if (V10IsGeted)
		{
			output.WriteRawTag(112);
			output.WriteBool(V10IsGeted);
		}
		if (V11IsGeted)
		{
			output.WriteRawTag(120);
			output.WriteBool(V11IsGeted);
		}
		if (V12IsGeted)
		{
			output.WriteRawTag(128, 1);
			output.WriteBool(V12IsGeted);
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
		if (Level != 0)
		{
			num += 1 + CodedOutputStream.ComputeInt32Size(Level);
		}
		if (PayAll != 0)
		{
			num += 1 + CodedOutputStream.ComputeInt32Size(PayAll);
		}
		if (V1IsGeted)
		{
			num += 2;
		}
		if (V2IsGeted)
		{
			num += 2;
		}
		if (V3IsGeted)
		{
			num += 2;
		}
		if (V4IsGeted)
		{
			num += 2;
		}
		if (V5IsGeted)
		{
			num += 2;
		}
		if (V6IsGeted)
		{
			num += 2;
		}
		if (V7IsGeted)
		{
			num += 2;
		}
		if (V8IsGeted)
		{
			num += 2;
		}
		if (V9IsGeted)
		{
			num += 2;
		}
		if (V10IsGeted)
		{
			num += 2;
		}
		if (V11IsGeted)
		{
			num += 2;
		}
		if (V12IsGeted)
		{
			num += 3;
		}
		if (_unknownFields != null)
		{
			num += _unknownFields.CalculateSize();
		}
		return num;
	}

	[DebuggerNonUserCode]
	public void MergeFrom(VipLevelResp_1002029 other)
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
			if (other.Level != 0)
			{
				Level = other.Level;
			}
			if (other.PayAll != 0)
			{
				PayAll = other.PayAll;
			}
			if (other.V1IsGeted)
			{
				V1IsGeted = other.V1IsGeted;
			}
			if (other.V2IsGeted)
			{
				V2IsGeted = other.V2IsGeted;
			}
			if (other.V3IsGeted)
			{
				V3IsGeted = other.V3IsGeted;
			}
			if (other.V4IsGeted)
			{
				V4IsGeted = other.V4IsGeted;
			}
			if (other.V5IsGeted)
			{
				V5IsGeted = other.V5IsGeted;
			}
			if (other.V6IsGeted)
			{
				V6IsGeted = other.V6IsGeted;
			}
			if (other.V7IsGeted)
			{
				V7IsGeted = other.V7IsGeted;
			}
			if (other.V8IsGeted)
			{
				V8IsGeted = other.V8IsGeted;
			}
			if (other.V9IsGeted)
			{
				V9IsGeted = other.V9IsGeted;
			}
			if (other.V10IsGeted)
			{
				V10IsGeted = other.V10IsGeted;
			}
			if (other.V11IsGeted)
			{
				V11IsGeted = other.V11IsGeted;
			}
			if (other.V12IsGeted)
			{
				V12IsGeted = other.V12IsGeted;
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
				Level = input.ReadInt32();
				break;
			case 32u:
				PayAll = input.ReadInt32();
				break;
			case 40u:
				V1IsGeted = input.ReadBool();
				break;
			case 48u:
				V2IsGeted = input.ReadBool();
				break;
			case 56u:
				V3IsGeted = input.ReadBool();
				break;
			case 64u:
				V4IsGeted = input.ReadBool();
				break;
			case 72u:
				V5IsGeted = input.ReadBool();
				break;
			case 80u:
				V6IsGeted = input.ReadBool();
				break;
			case 88u:
				V7IsGeted = input.ReadBool();
				break;
			case 96u:
				V8IsGeted = input.ReadBool();
				break;
			case 104u:
				V9IsGeted = input.ReadBool();
				break;
			case 112u:
				V10IsGeted = input.ReadBool();
				break;
			case 120u:
				V11IsGeted = input.ReadBool();
				break;
			case 128u:
				V12IsGeted = input.ReadBool();
				break;
			}
		}
	}
}
