using System;
using System.Diagnostics;
using Google.Protobuf;
using Google.Protobuf.Reflection;

public sealed class RefiningEquipmentReq_1005049 : IMessage<RefiningEquipmentReq_1005049>, IMessage, IEquatable<RefiningEquipmentReq_1005049>, IDeepCloneable<RefiningEquipmentReq_1005049>
{
	private static readonly MessageParser<RefiningEquipmentReq_1005049> _parser = new MessageParser<RefiningEquipmentReq_1005049>(() => new RefiningEquipmentReq_1005049());

	private UnknownFieldSet _unknownFields;

	public const int MethodEquipmentIdFieldNumber = 1;

	private string methodEquipmentId_ = "";

	public const int IsRefiningSuccessFieldNumber = 2;

	private bool isRefiningSuccess_;

	public const int FractionOfCoverageFieldNumber = 3;

	private int fractionOfCoverage_;

	[DebuggerNonUserCode]
	public static MessageParser<RefiningEquipmentReq_1005049> Parser => _parser;

	[DebuggerNonUserCode]
	public static MessageDescriptor Descriptor => HomeProtoReflection.Descriptor.MessageTypes[66];

	[DebuggerNonUserCode]
	MessageDescriptor IMessage.Descriptor => pb_003A_003AGoogle_002EProtobuf_002EIMessage_002Eget_Descriptor();

	[DebuggerNonUserCode]
	public string MethodEquipmentId
	{
		get
		{
			return methodEquipmentId_;
		}
		set
		{
			methodEquipmentId_ = ProtoPreconditions.CheckNotNull(value, "value");
		}
	}

	[DebuggerNonUserCode]
	public bool IsRefiningSuccess
	{
		get
		{
			return isRefiningSuccess_;
		}
		set
		{
			isRefiningSuccess_ = value;
		}
	}

	[DebuggerNonUserCode]
	public int FractionOfCoverage
	{
		get
		{
			return fractionOfCoverage_;
		}
		set
		{
			fractionOfCoverage_ = value;
		}
	}

	private MessageDescriptor pb_003A_003AGoogle_002EProtobuf_002EIMessage_002Eget_Descriptor()
	{
		return Descriptor;
	}

	[DebuggerNonUserCode]
	public RefiningEquipmentReq_1005049()
	{
	}

	[DebuggerNonUserCode]
	public RefiningEquipmentReq_1005049(RefiningEquipmentReq_1005049 other)
		: this()
	{
		methodEquipmentId_ = other.methodEquipmentId_;
		isRefiningSuccess_ = other.isRefiningSuccess_;
		fractionOfCoverage_ = other.fractionOfCoverage_;
		_unknownFields = UnknownFieldSet.Clone(other._unknownFields);
	}

	[DebuggerNonUserCode]
	public RefiningEquipmentReq_1005049 Clone()
	{
		return new RefiningEquipmentReq_1005049(this);
	}

	[DebuggerNonUserCode]
	public override bool Equals(object other)
	{
		return Equals(other as RefiningEquipmentReq_1005049);
	}

	[DebuggerNonUserCode]
	public bool Equals(RefiningEquipmentReq_1005049 other)
	{
		if (other == null)
		{
			return false;
		}
		if (other == this)
		{
			return true;
		}
		if (MethodEquipmentId != other.MethodEquipmentId)
		{
			return false;
		}
		if (IsRefiningSuccess != other.IsRefiningSuccess)
		{
			return false;
		}
		if (FractionOfCoverage != other.FractionOfCoverage)
		{
			return false;
		}
		return object.Equals(_unknownFields, other._unknownFields);
	}

	[DebuggerNonUserCode]
	public override int GetHashCode()
	{
		int num = 1;
		if (MethodEquipmentId.Length != 0)
		{
			num ^= MethodEquipmentId.GetHashCode();
		}
		if (IsRefiningSuccess)
		{
			num ^= IsRefiningSuccess.GetHashCode();
		}
		if (FractionOfCoverage != 0)
		{
			num ^= FractionOfCoverage.GetHashCode();
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
		if (MethodEquipmentId.Length != 0)
		{
			output.WriteRawTag(10);
			output.WriteString(MethodEquipmentId);
		}
		if (IsRefiningSuccess)
		{
			output.WriteRawTag(16);
			output.WriteBool(IsRefiningSuccess);
		}
		if (FractionOfCoverage != 0)
		{
			output.WriteRawTag(24);
			output.WriteInt32(FractionOfCoverage);
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
		if (MethodEquipmentId.Length != 0)
		{
			num += 1 + CodedOutputStream.ComputeStringSize(MethodEquipmentId);
		}
		if (IsRefiningSuccess)
		{
			num += 2;
		}
		if (FractionOfCoverage != 0)
		{
			num += 1 + CodedOutputStream.ComputeInt32Size(FractionOfCoverage);
		}
		if (_unknownFields != null)
		{
			num += _unknownFields.CalculateSize();
		}
		return num;
	}

	[DebuggerNonUserCode]
	public void MergeFrom(RefiningEquipmentReq_1005049 other)
	{
		if (other != null)
		{
			if (other.MethodEquipmentId.Length != 0)
			{
				MethodEquipmentId = other.MethodEquipmentId;
			}
			if (other.IsRefiningSuccess)
			{
				IsRefiningSuccess = other.IsRefiningSuccess;
			}
			if (other.FractionOfCoverage != 0)
			{
				FractionOfCoverage = other.FractionOfCoverage;
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
			case 10u:
				MethodEquipmentId = input.ReadString();
				break;
			case 16u:
				IsRefiningSuccess = input.ReadBool();
				break;
			case 24u:
				FractionOfCoverage = input.ReadInt32();
				break;
			}
		}
	}
}
