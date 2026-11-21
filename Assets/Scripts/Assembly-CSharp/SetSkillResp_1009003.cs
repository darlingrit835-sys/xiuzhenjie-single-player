using System;
using System.Diagnostics;
using Google.Protobuf;
using Google.Protobuf.Reflection;

public sealed class SetSkillResp_1009003 : IMessage<SetSkillResp_1009003>, IMessage, IEquatable<SetSkillResp_1009003>, IDeepCloneable<SetSkillResp_1009003>
{
	private static readonly MessageParser<SetSkillResp_1009003> _parser = new MessageParser<SetSkillResp_1009003>(() => new SetSkillResp_1009003());

	private UnknownFieldSet _unknownFields;

	public const int CodeFieldNumber = 1;

	private int code_;

	public const int IsSuccessFieldNumber = 2;

	private bool isSuccess_;

	public const int CurrentPerceptionFieldNumber = 3;

	private int currentPerception_;

	public const int FullLoadPerceptionFieldNumber = 4;

	private int fullLoadPerception_;

	[DebuggerNonUserCode]
	public static MessageParser<SetSkillResp_1009003> Parser => _parser;

	[DebuggerNonUserCode]
	public static MessageDescriptor Descriptor => BattleSettingProtoReflection.Descriptor.MessageTypes[5];

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
	public int CurrentPerception
	{
		get
		{
			return currentPerception_;
		}
		set
		{
			currentPerception_ = value;
		}
	}

	[DebuggerNonUserCode]
	public int FullLoadPerception
	{
		get
		{
			return fullLoadPerception_;
		}
		set
		{
			fullLoadPerception_ = value;
		}
	}

	private MessageDescriptor pb_003A_003AGoogle_002EProtobuf_002EIMessage_002Eget_Descriptor()
	{
		return Descriptor;
	}

	[DebuggerNonUserCode]
	public SetSkillResp_1009003()
	{
	}

	[DebuggerNonUserCode]
	public SetSkillResp_1009003(SetSkillResp_1009003 other)
		: this()
	{
		code_ = other.code_;
		isSuccess_ = other.isSuccess_;
		currentPerception_ = other.currentPerception_;
		fullLoadPerception_ = other.fullLoadPerception_;
		_unknownFields = UnknownFieldSet.Clone(other._unknownFields);
	}

	[DebuggerNonUserCode]
	public SetSkillResp_1009003 Clone()
	{
		return new SetSkillResp_1009003(this);
	}

	[DebuggerNonUserCode]
	public override bool Equals(object other)
	{
		return Equals(other as SetSkillResp_1009003);
	}

	[DebuggerNonUserCode]
	public bool Equals(SetSkillResp_1009003 other)
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
		if (CurrentPerception != other.CurrentPerception)
		{
			return false;
		}
		if (FullLoadPerception != other.FullLoadPerception)
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
		if (CurrentPerception != 0)
		{
			num ^= CurrentPerception.GetHashCode();
		}
		if (FullLoadPerception != 0)
		{
			num ^= FullLoadPerception.GetHashCode();
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
		if (CurrentPerception != 0)
		{
			output.WriteRawTag(24);
			output.WriteInt32(CurrentPerception);
		}
		if (FullLoadPerception != 0)
		{
			output.WriteRawTag(32);
			output.WriteInt32(FullLoadPerception);
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
		if (CurrentPerception != 0)
		{
			num += 1 + CodedOutputStream.ComputeInt32Size(CurrentPerception);
		}
		if (FullLoadPerception != 0)
		{
			num += 1 + CodedOutputStream.ComputeInt32Size(FullLoadPerception);
		}
		if (_unknownFields != null)
		{
			num += _unknownFields.CalculateSize();
		}
		return num;
	}

	[DebuggerNonUserCode]
	public void MergeFrom(SetSkillResp_1009003 other)
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
			if (other.CurrentPerception != 0)
			{
				CurrentPerception = other.CurrentPerception;
			}
			if (other.FullLoadPerception != 0)
			{
				FullLoadPerception = other.FullLoadPerception;
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
				CurrentPerception = input.ReadInt32();
				break;
			case 32u:
				FullLoadPerception = input.ReadInt32();
				break;
			}
		}
	}
}
