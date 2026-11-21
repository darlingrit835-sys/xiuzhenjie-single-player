using System;
using System.Diagnostics;
using Google.Protobuf;
using Google.Protobuf.Reflection;

public sealed class UpgradeEquipmentJadeBuyTimeResp_1015092 : IMessage<UpgradeEquipmentJadeBuyTimeResp_1015092>, IMessage, IEquatable<UpgradeEquipmentJadeBuyTimeResp_1015092>, IDeepCloneable<UpgradeEquipmentJadeBuyTimeResp_1015092>
{
	private static readonly MessageParser<UpgradeEquipmentJadeBuyTimeResp_1015092> _parser = new MessageParser<UpgradeEquipmentJadeBuyTimeResp_1015092>(() => new UpgradeEquipmentJadeBuyTimeResp_1015092());

	private UnknownFieldSet _unknownFields;

	public const int CodeFieldNumber = 1;

	private int code_;

	public const int IsSuccessFieldNumber = 2;

	private bool isSuccess_;

	public const int ShengYuTimeFieldNumber = 3;

	private long shengYuTime_;

	[DebuggerNonUserCode]
	public static MessageParser<UpgradeEquipmentJadeBuyTimeResp_1015092> Parser => _parser;

	[DebuggerNonUserCode]
	public static MessageDescriptor Descriptor => GuildProtoReflection.Descriptor.MessageTypes[112];

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
	public long ShengYuTime
	{
		get
		{
			return shengYuTime_;
		}
		set
		{
			shengYuTime_ = value;
		}
	}

	private MessageDescriptor pb_003A_003AGoogle_002EProtobuf_002EIMessage_002Eget_Descriptor()
	{
		return Descriptor;
	}

	[DebuggerNonUserCode]
	public UpgradeEquipmentJadeBuyTimeResp_1015092()
	{
	}

	[DebuggerNonUserCode]
	public UpgradeEquipmentJadeBuyTimeResp_1015092(UpgradeEquipmentJadeBuyTimeResp_1015092 other)
		: this()
	{
		code_ = other.code_;
		isSuccess_ = other.isSuccess_;
		shengYuTime_ = other.shengYuTime_;
		_unknownFields = UnknownFieldSet.Clone(other._unknownFields);
	}

	[DebuggerNonUserCode]
	public UpgradeEquipmentJadeBuyTimeResp_1015092 Clone()
	{
		return new UpgradeEquipmentJadeBuyTimeResp_1015092(this);
	}

	[DebuggerNonUserCode]
	public override bool Equals(object other)
	{
		return Equals(other as UpgradeEquipmentJadeBuyTimeResp_1015092);
	}

	[DebuggerNonUserCode]
	public bool Equals(UpgradeEquipmentJadeBuyTimeResp_1015092 other)
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
		if (ShengYuTime != other.ShengYuTime)
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
		if (ShengYuTime != 0L)
		{
			num ^= ShengYuTime.GetHashCode();
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
		if (ShengYuTime != 0L)
		{
			output.WriteRawTag(24);
			output.WriteInt64(ShengYuTime);
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
		if (ShengYuTime != 0L)
		{
			num += 1 + CodedOutputStream.ComputeInt64Size(ShengYuTime);
		}
		if (_unknownFields != null)
		{
			num += _unknownFields.CalculateSize();
		}
		return num;
	}

	[DebuggerNonUserCode]
	public void MergeFrom(UpgradeEquipmentJadeBuyTimeResp_1015092 other)
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
			if (other.ShengYuTime != 0L)
			{
				ShengYuTime = other.ShengYuTime;
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
				ShengYuTime = input.ReadInt64();
				break;
			}
		}
	}
}
