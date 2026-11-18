using System;
using System.Diagnostics;
using Google.Protobuf;
using Google.Protobuf.Reflection;

public sealed class WaterChooseResp_1013016 : IMessage<WaterChooseResp_1013016>, IMessage, IEquatable<WaterChooseResp_1013016>, IDeepCloneable<WaterChooseResp_1013016>
{
	private static readonly MessageParser<WaterChooseResp_1013016> _parser = new MessageParser<WaterChooseResp_1013016>(() => new WaterChooseResp_1013016());

	private UnknownFieldSet _unknownFields;

	public const int CodeFieldNumber = 1;

	private int code_;

	public const int IsSuccessFieldNumber = 2;

	private bool isSuccess_;

	public const int LandStatusFieldNumber = 3;

	private int landStatus_;

	public const int TimeRemainingFieldNumber = 4;

	private long timeRemaining_;

	public const int TotalWasteTimeFieldNumber = 5;

	private long totalWasteTime_;

	[DebuggerNonUserCode]
	public static MessageParser<WaterChooseResp_1013016> Parser => _parser;

	[DebuggerNonUserCode]
	public static MessageDescriptor Descriptor => PlantProtoReflection.Descriptor.MessageTypes[19];

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
	public int LandStatus
	{
		get
		{
			return landStatus_;
		}
		set
		{
			landStatus_ = value;
		}
	}

	[DebuggerNonUserCode]
	public long TimeRemaining
	{
		get
		{
			return timeRemaining_;
		}
		set
		{
			timeRemaining_ = value;
		}
	}

	[DebuggerNonUserCode]
	public long TotalWasteTime
	{
		get
		{
			return totalWasteTime_;
		}
		set
		{
			totalWasteTime_ = value;
		}
	}

	private MessageDescriptor pb_003A_003AGoogle_002EProtobuf_002EIMessage_002Eget_Descriptor()
	{
		return Descriptor;
	}

	[DebuggerNonUserCode]
	public WaterChooseResp_1013016()
	{
	}

	[DebuggerNonUserCode]
	public WaterChooseResp_1013016(WaterChooseResp_1013016 other)
		: this()
	{
		code_ = other.code_;
		isSuccess_ = other.isSuccess_;
		landStatus_ = other.landStatus_;
		timeRemaining_ = other.timeRemaining_;
		totalWasteTime_ = other.totalWasteTime_;
		_unknownFields = UnknownFieldSet.Clone(other._unknownFields);
	}

	[DebuggerNonUserCode]
	public WaterChooseResp_1013016 Clone()
	{
		return new WaterChooseResp_1013016(this);
	}

	[DebuggerNonUserCode]
	public override bool Equals(object other)
	{
		return Equals(other as WaterChooseResp_1013016);
	}

	[DebuggerNonUserCode]
	public bool Equals(WaterChooseResp_1013016 other)
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
		if (LandStatus != other.LandStatus)
		{
			return false;
		}
		if (TimeRemaining != other.TimeRemaining)
		{
			return false;
		}
		if (TotalWasteTime != other.TotalWasteTime)
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
		if (LandStatus != 0)
		{
			num ^= LandStatus.GetHashCode();
		}
		if (TimeRemaining != 0L)
		{
			num ^= TimeRemaining.GetHashCode();
		}
		if (TotalWasteTime != 0L)
		{
			num ^= TotalWasteTime.GetHashCode();
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
		if (LandStatus != 0)
		{
			output.WriteRawTag(24);
			output.WriteInt32(LandStatus);
		}
		if (TimeRemaining != 0L)
		{
			output.WriteRawTag(32);
			output.WriteInt64(TimeRemaining);
		}
		if (TotalWasteTime != 0L)
		{
			output.WriteRawTag(40);
			output.WriteInt64(TotalWasteTime);
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
		if (LandStatus != 0)
		{
			num += 1 + CodedOutputStream.ComputeInt32Size(LandStatus);
		}
		if (TimeRemaining != 0L)
		{
			num += 1 + CodedOutputStream.ComputeInt64Size(TimeRemaining);
		}
		if (TotalWasteTime != 0L)
		{
			num += 1 + CodedOutputStream.ComputeInt64Size(TotalWasteTime);
		}
		if (_unknownFields != null)
		{
			num += _unknownFields.CalculateSize();
		}
		return num;
	}

	[DebuggerNonUserCode]
	public void MergeFrom(WaterChooseResp_1013016 other)
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
			if (other.LandStatus != 0)
			{
				LandStatus = other.LandStatus;
			}
			if (other.TimeRemaining != 0L)
			{
				TimeRemaining = other.TimeRemaining;
			}
			if (other.TotalWasteTime != 0L)
			{
				TotalWasteTime = other.TotalWasteTime;
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
				LandStatus = input.ReadInt32();
				break;
			case 32u:
				TimeRemaining = input.ReadInt64();
				break;
			case 40u:
				TotalWasteTime = input.ReadInt64();
				break;
			}
		}
	}
}
