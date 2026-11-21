using System;
using System.Diagnostics;
using Google.Protobuf;
using Google.Protobuf.Reflection;

public sealed class AddTimeControllerMatrixReq_1005024 : IMessage<AddTimeControllerMatrixReq_1005024>, IMessage, IEquatable<AddTimeControllerMatrixReq_1005024>, IDeepCloneable<AddTimeControllerMatrixReq_1005024>
{
	private static readonly MessageParser<AddTimeControllerMatrixReq_1005024> _parser = new MessageParser<AddTimeControllerMatrixReq_1005024>(() => new AddTimeControllerMatrixReq_1005024());

	private UnknownFieldSet _unknownFields;

	public const int HoursFieldNumber = 1;

	private int hours_;

	[DebuggerNonUserCode]
	public static MessageParser<AddTimeControllerMatrixReq_1005024> Parser => _parser;

	[DebuggerNonUserCode]
	public static MessageDescriptor Descriptor => HomeProtoReflection.Descriptor.MessageTypes[29];

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
	public AddTimeControllerMatrixReq_1005024()
	{
	}

	[DebuggerNonUserCode]
	public AddTimeControllerMatrixReq_1005024(AddTimeControllerMatrixReq_1005024 other)
		: this()
	{
		hours_ = other.hours_;
		_unknownFields = UnknownFieldSet.Clone(other._unknownFields);
	}

	[DebuggerNonUserCode]
	public AddTimeControllerMatrixReq_1005024 Clone()
	{
		return new AddTimeControllerMatrixReq_1005024(this);
	}

	[DebuggerNonUserCode]
	public override bool Equals(object other)
	{
		return Equals(other as AddTimeControllerMatrixReq_1005024);
	}

	[DebuggerNonUserCode]
	public bool Equals(AddTimeControllerMatrixReq_1005024 other)
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
	public void MergeFrom(AddTimeControllerMatrixReq_1005024 other)
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
