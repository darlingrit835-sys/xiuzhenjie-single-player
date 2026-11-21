using System;
using System.Diagnostics;
using Google.Protobuf;
using Google.Protobuf.Reflection;

public sealed class AddTimeGatherMatrixReq_1005025 : IMessage<AddTimeGatherMatrixReq_1005025>, IMessage, IEquatable<AddTimeGatherMatrixReq_1005025>, IDeepCloneable<AddTimeGatherMatrixReq_1005025>
{
	private static readonly MessageParser<AddTimeGatherMatrixReq_1005025> _parser = new MessageParser<AddTimeGatherMatrixReq_1005025>(() => new AddTimeGatherMatrixReq_1005025());

	private UnknownFieldSet _unknownFields;

	public const int HoursFieldNumber = 1;

	private int hours_;

	[DebuggerNonUserCode]
	public static MessageParser<AddTimeGatherMatrixReq_1005025> Parser => _parser;

	[DebuggerNonUserCode]
	public static MessageDescriptor Descriptor => HomeProtoReflection.Descriptor.MessageTypes[31];

	[DebuggerNonUserCode]
	MessageDescriptor IMessage.Descriptor => pb_003A_003AGoogle_002EProtobuf_002EIMessage_002Eget_Descriptor();

	[DebuggerNonUserCode]
	public int Hours
	{
		get
		{
			return hours_;
		}
		set
		{
			hours_ = value;
		}
	}

	private MessageDescriptor pb_003A_003AGoogle_002EProtobuf_002EIMessage_002Eget_Descriptor()
	{
		return Descriptor;
	}

	[DebuggerNonUserCode]
	public AddTimeGatherMatrixReq_1005025()
	{
	}

	[DebuggerNonUserCode]
	public AddTimeGatherMatrixReq_1005025(AddTimeGatherMatrixReq_1005025 other)
		: this()
	{
		hours_ = other.hours_;
		_unknownFields = UnknownFieldSet.Clone(other._unknownFields);
	}

	[DebuggerNonUserCode]
	public AddTimeGatherMatrixReq_1005025 Clone()
	{
		return new AddTimeGatherMatrixReq_1005025(this);
	}

	[DebuggerNonUserCode]
	public override bool Equals(object other)
	{
		return Equals(other as AddTimeGatherMatrixReq_1005025);
	}

	[DebuggerNonUserCode]
	public bool Equals(AddTimeGatherMatrixReq_1005025 other)
	{
		if (other == null)
		{
			return false;
		}
		if (other == this)
		{
			return true;
		}
		if (Hours != other.Hours)
		{
			return false;
		}
		return object.Equals(_unknownFields, other._unknownFields);
	}

	[DebuggerNonUserCode]
	public override int GetHashCode()
	{
		int num = 1;
		if (Hours != 0)
		{
			num ^= Hours.GetHashCode();
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
		if (Hours != 0)
		{
			output.WriteRawTag(8);
			output.WriteInt32(Hours);
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
		if (Hours != 0)
		{
			num += 1 + CodedOutputStream.ComputeInt32Size(Hours);
		}
		if (_unknownFields != null)
		{
			num += _unknownFields.CalculateSize();
		}
		return num;
	}

	[DebuggerNonUserCode]
	public void MergeFrom(AddTimeGatherMatrixReq_1005025 other)
	{
		if (other != null)
		{
			if (other.Hours != 0)
			{
				Hours = other.Hours;
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
			if (num != 8)
			{
				_unknownFields = UnknownFieldSet.MergeFieldFrom(_unknownFields, input);
			}
			else
			{
				Hours = input.ReadInt32();
			}
		}
	}
}
