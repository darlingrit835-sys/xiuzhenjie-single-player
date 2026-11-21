using System;
using System.Diagnostics;
using Google.Protobuf;
using Google.Protobuf.Reflection;

public sealed class UpgradeControllerMatrixResp_1005015 : IMessage<UpgradeControllerMatrixResp_1005015>, IMessage, IEquatable<UpgradeControllerMatrixResp_1005015>, IDeepCloneable<UpgradeControllerMatrixResp_1005015>
{
	private static readonly MessageParser<UpgradeControllerMatrixResp_1005015> _parser = new MessageParser<UpgradeControllerMatrixResp_1005015>(() => new UpgradeControllerMatrixResp_1005015());

	private UnknownFieldSet _unknownFields;

	public const int CodeFieldNumber = 1;

	private int code_;

	public const int IsSuccessFieldNumber = 2;

	private bool isSuccess_;

	public const int ShinyStoneFieldNumber = 3;

	private long shinyStone_;

	public const int RankFieldNumber = 4;

	private int rank_;

	public const int NewNameFieldNumber = 5;

	private string newName_ = "";

	public const int ControllerMatrixLessTimeFieldNumber = 6;

	private long controllerMatrixLessTime_;

	[DebuggerNonUserCode]
	public static MessageParser<UpgradeControllerMatrixResp_1005015> Parser => _parser;

	[DebuggerNonUserCode]
	public static MessageDescriptor Descriptor => HomeProtoReflection.Descriptor.MessageTypes[19];

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
	public string NewName
	{
		get
		{
			return newName_;
		}
		set
		{
			newName_ = ProtoPreconditions.CheckNotNull(value, "value");
		}
	}

	[DebuggerNonUserCode]
	public long ControllerMatrixLessTime
	{
		get
		{
			return controllerMatrixLessTime_;
		}
		set
		{
			controllerMatrixLessTime_ = value;
		}
	}

	private MessageDescriptor pb_003A_003AGoogle_002EProtobuf_002EIMessage_002Eget_Descriptor()
	{
		return Descriptor;
	}

	[DebuggerNonUserCode]
	public UpgradeControllerMatrixResp_1005015()
	{
	}

	[DebuggerNonUserCode]
	public UpgradeControllerMatrixResp_1005015(UpgradeControllerMatrixResp_1005015 other)
		: this()
	{
		code_ = other.code_;
		isSuccess_ = other.isSuccess_;
		shinyStone_ = other.shinyStone_;
		rank_ = other.rank_;
		newName_ = other.newName_;
		controllerMatrixLessTime_ = other.controllerMatrixLessTime_;
		_unknownFields = UnknownFieldSet.Clone(other._unknownFields);
	}

	[DebuggerNonUserCode]
	public UpgradeControllerMatrixResp_1005015 Clone()
	{
		return new UpgradeControllerMatrixResp_1005015(this);
	}

	[DebuggerNonUserCode]
	public override bool Equals(object other)
	{
		return Equals(other as UpgradeControllerMatrixResp_1005015);
	}

	[DebuggerNonUserCode]
	public bool Equals(UpgradeControllerMatrixResp_1005015 other)
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
		if (ShinyStone != other.ShinyStone)
		{
			return false;
		}
		if (Rank != other.Rank)
		{
			return false;
		}
		if (NewName != other.NewName)
		{
			return false;
		}
		if (ControllerMatrixLessTime != other.ControllerMatrixLessTime)
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
		if (ShinyStone != 0L)
		{
			num ^= ShinyStone.GetHashCode();
		}
		if (Rank != 0)
		{
			num ^= Rank.GetHashCode();
		}
		if (NewName.Length != 0)
		{
			num ^= NewName.GetHashCode();
		}
		if (ControllerMatrixLessTime != 0L)
		{
			num ^= ControllerMatrixLessTime.GetHashCode();
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
		if (ShinyStone != 0L)
		{
			output.WriteRawTag(24);
			output.WriteInt64(ShinyStone);
		}
		if (Rank != 0)
		{
			output.WriteRawTag(32);
			output.WriteInt32(Rank);
		}
		if (NewName.Length != 0)
		{
			output.WriteRawTag(42);
			output.WriteString(NewName);
		}
		if (ControllerMatrixLessTime != 0L)
		{
			output.WriteRawTag(48);
			output.WriteInt64(ControllerMatrixLessTime);
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
		if (ShinyStone != 0L)
		{
			num += 1 + CodedOutputStream.ComputeInt64Size(ShinyStone);
		}
		if (Rank != 0)
		{
			num += 1 + CodedOutputStream.ComputeInt32Size(Rank);
		}
		if (NewName.Length != 0)
		{
			num += 1 + CodedOutputStream.ComputeStringSize(NewName);
		}
		if (ControllerMatrixLessTime != 0L)
		{
			num += 1 + CodedOutputStream.ComputeInt64Size(ControllerMatrixLessTime);
		}
		if (_unknownFields != null)
		{
			num += _unknownFields.CalculateSize();
		}
		return num;
	}

	[DebuggerNonUserCode]
	public void MergeFrom(UpgradeControllerMatrixResp_1005015 other)
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
			if (other.ShinyStone != 0L)
			{
				ShinyStone = other.ShinyStone;
			}
			if (other.Rank != 0)
			{
				Rank = other.Rank;
			}
			if (other.NewName.Length != 0)
			{
				NewName = other.NewName;
			}
			if (other.ControllerMatrixLessTime != 0L)
			{
				ControllerMatrixLessTime = other.ControllerMatrixLessTime;
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
				ShinyStone = input.ReadInt64();
				break;
			case 32u:
				Rank = input.ReadInt32();
				break;
			case 42u:
				NewName = input.ReadString();
				break;
			case 48u:
				ControllerMatrixLessTime = input.ReadInt64();
				break;
			}
		}
	}
}
