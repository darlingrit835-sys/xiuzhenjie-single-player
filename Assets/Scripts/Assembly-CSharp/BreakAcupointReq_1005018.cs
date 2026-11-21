using System;
using System.Diagnostics;
using Google.Protobuf;
using Google.Protobuf.Reflection;

public sealed class BreakAcupointReq_1005018 : IMessage<BreakAcupointReq_1005018>, IMessage, IEquatable<BreakAcupointReq_1005018>, IDeepCloneable<BreakAcupointReq_1005018>
{
	private static readonly MessageParser<BreakAcupointReq_1005018> _parser = new MessageParser<BreakAcupointReq_1005018>(() => new BreakAcupointReq_1005018());

	private UnknownFieldSet _unknownFields;

	public const int AcupointIdFieldNumber = 1;

	private string acupointId_ = "";

	[DebuggerNonUserCode]
	public static MessageParser<BreakAcupointReq_1005018> Parser => _parser;

	[DebuggerNonUserCode]
	public static MessageDescriptor Descriptor => HomeProtoReflection.Descriptor.MessageTypes[21];

	[DebuggerNonUserCode]
	MessageDescriptor IMessage.Descriptor => pb_003A_003AGoogle_002EProtobuf_002EIMessage_002Eget_Descriptor();

	[DebuggerNonUserCode]
	public string AcupointId
	{
		get
		{
			return acupointId_;
		}
		set
		{
			acupointId_ = ProtoPreconditions.CheckNotNull(value, "value");
		}
	}

	private MessageDescriptor pb_003A_003AGoogle_002EProtobuf_002EIMessage_002Eget_Descriptor()
	{
		return Descriptor;
	}

	[DebuggerNonUserCode]
	public BreakAcupointReq_1005018()
	{
	}

	[DebuggerNonUserCode]
	public BreakAcupointReq_1005018(BreakAcupointReq_1005018 other)
		: this()
	{
		acupointId_ = other.acupointId_;
		_unknownFields = UnknownFieldSet.Clone(other._unknownFields);
	}

	[DebuggerNonUserCode]
	public BreakAcupointReq_1005018 Clone()
	{
		return new BreakAcupointReq_1005018(this);
	}

	[DebuggerNonUserCode]
	public override bool Equals(object other)
	{
		return Equals(other as BreakAcupointReq_1005018);
	}

	[DebuggerNonUserCode]
	public bool Equals(BreakAcupointReq_1005018 other)
	{
		if (other == null)
		{
			return false;
		}
		if (other == this)
		{
			return true;
		}
		if (AcupointId != other.AcupointId)
		{
			return false;
		}
		return object.Equals(_unknownFields, other._unknownFields);
	}

	[DebuggerNonUserCode]
	public override int GetHashCode()
	{
		int num = 1;
		if (AcupointId.Length != 0)
		{
			num ^= AcupointId.GetHashCode();
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
		if (AcupointId.Length != 0)
		{
			output.WriteRawTag(10);
			output.WriteString(AcupointId);
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
		if (AcupointId.Length != 0)
		{
			num += 1 + CodedOutputStream.ComputeStringSize(AcupointId);
		}
		if (_unknownFields != null)
		{
			num += _unknownFields.CalculateSize();
		}
		return num;
	}

	[DebuggerNonUserCode]
	public void MergeFrom(BreakAcupointReq_1005018 other)
	{
		if (other != null)
		{
			if (other.AcupointId.Length != 0)
			{
				AcupointId = other.AcupointId;
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
			if (num != 10)
			{
				_unknownFields = UnknownFieldSet.MergeFieldFrom(_unknownFields, input);
			}
			else
			{
				AcupointId = input.ReadString();
			}
		}
	}
}
