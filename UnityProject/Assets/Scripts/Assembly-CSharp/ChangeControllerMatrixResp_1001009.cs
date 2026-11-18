using System;
using System.Diagnostics;
using Google.Protobuf;
using Google.Protobuf.Collections;
using Google.Protobuf.Reflection;

public sealed class ChangeControllerMatrixResp_1001009 : IMessage<ChangeControllerMatrixResp_1001009>, IMessage, IEquatable<ChangeControllerMatrixResp_1001009>, IDeepCloneable<ChangeControllerMatrixResp_1001009>
{
	private static readonly MessageParser<ChangeControllerMatrixResp_1001009> _parser = new MessageParser<ChangeControllerMatrixResp_1001009>(() => new ChangeControllerMatrixResp_1001009());

	private UnknownFieldSet _unknownFields;

	public const int CodeFieldNumber = 1;

	private int code_;

	public const int IsSuccessFieldNumber = 2;

	private bool isSuccess_;

	public const int CurrentSuperAirRunValueFieldNumber = 3;

	private double currentSuperAirRunValue_;

	[DebuggerNonUserCode]
	public static MessageParser<ChangeControllerMatrixResp_1001009> Parser => _parser;

	[DebuggerNonUserCode]
	public static MessageDescriptor Descriptor => SimpleActionProtoReflection.Descriptor.MessageTypes[10];

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
	public double CurrentSuperAirRunValue
	{
		get
		{
			return currentSuperAirRunValue_;
		}
		set
		{
			currentSuperAirRunValue_ = value;
		}
	}

	private MessageDescriptor pb_003A_003AGoogle_002EProtobuf_002EIMessage_002Eget_Descriptor()
	{
		return Descriptor;
	}

	[DebuggerNonUserCode]
	public ChangeControllerMatrixResp_1001009()
	{
	}

	[DebuggerNonUserCode]
	public ChangeControllerMatrixResp_1001009(ChangeControllerMatrixResp_1001009 other)
		: this()
	{
		code_ = other.code_;
		isSuccess_ = other.isSuccess_;
		currentSuperAirRunValue_ = other.currentSuperAirRunValue_;
		_unknownFields = UnknownFieldSet.Clone(other._unknownFields);
	}

	[DebuggerNonUserCode]
	public ChangeControllerMatrixResp_1001009 Clone()
	{
		return new ChangeControllerMatrixResp_1001009(this);
	}

	[DebuggerNonUserCode]
	public override bool Equals(object other)
	{
		return Equals(other as ChangeControllerMatrixResp_1001009);
	}

	[DebuggerNonUserCode]
	public bool Equals(ChangeControllerMatrixResp_1001009 other)
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
		if (!ProtobufEqualityComparers.BitwiseDoubleEqualityComparer.Equals(CurrentSuperAirRunValue, other.CurrentSuperAirRunValue))
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
		if (CurrentSuperAirRunValue != 0.0)
		{
			num ^= ProtobufEqualityComparers.BitwiseDoubleEqualityComparer.GetHashCode(CurrentSuperAirRunValue);
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
		if (CurrentSuperAirRunValue != 0.0)
		{
			output.WriteRawTag(25);
			output.WriteDouble(CurrentSuperAirRunValue);
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
		if (CurrentSuperAirRunValue != 0.0)
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
	public void MergeFrom(ChangeControllerMatrixResp_1001009 other)
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
			if (other.CurrentSuperAirRunValue != 0.0)
			{
				CurrentSuperAirRunValue = other.CurrentSuperAirRunValue;
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
			case 25u:
				CurrentSuperAirRunValue = input.ReadDouble();
				break;
			}
		}
	}
}
