using System;
using System.Diagnostics;
using Google.Protobuf;
using Google.Protobuf.Reflection;

public sealed class EnterIrrigateReq_1013014 : IMessage<EnterIrrigateReq_1013014>, IMessage, IEquatable<EnterIrrigateReq_1013014>, IDeepCloneable<EnterIrrigateReq_1013014>
{
	private static readonly MessageParser<EnterIrrigateReq_1013014> _parser = new MessageParser<EnterIrrigateReq_1013014>(() => new EnterIrrigateReq_1013014());

	private UnknownFieldSet _unknownFields;

	public const int LandIdFieldNumber = 1;

	private int landId_;

	[DebuggerNonUserCode]
	public static MessageParser<EnterIrrigateReq_1013014> Parser => _parser;

	[DebuggerNonUserCode]
	public static MessageDescriptor Descriptor => PlantProtoReflection.Descriptor.MessageTypes[16];

	[DebuggerNonUserCode]
	MessageDescriptor IMessage.Descriptor => pb_003A_003AGoogle_002EProtobuf_002EIMessage_002Eget_Descriptor();

	[DebuggerNonUserCode]
	public int LandId
	{
		get
		{
			return landId_;
		}
		set
		{
			landId_ = value;
		}
	}

	private MessageDescriptor pb_003A_003AGoogle_002EProtobuf_002EIMessage_002Eget_Descriptor()
	{
		return Descriptor;
	}

	[DebuggerNonUserCode]
	public EnterIrrigateReq_1013014()
	{
	}

	[DebuggerNonUserCode]
	public EnterIrrigateReq_1013014(EnterIrrigateReq_1013014 other)
		: this()
	{
		landId_ = other.landId_;
		_unknownFields = UnknownFieldSet.Clone(other._unknownFields);
	}

	[DebuggerNonUserCode]
	public EnterIrrigateReq_1013014 Clone()
	{
		return new EnterIrrigateReq_1013014(this);
	}

	[DebuggerNonUserCode]
	public override bool Equals(object other)
	{
		return Equals(other as EnterIrrigateReq_1013014);
	}

	[DebuggerNonUserCode]
	public bool Equals(EnterIrrigateReq_1013014 other)
	{
		if (other == null)
		{
			return false;
		}
		if (other == this)
		{
			return true;
		}
		if (LandId != other.LandId)
		{
			return false;
		}
		return object.Equals(_unknownFields, other._unknownFields);
	}

	[DebuggerNonUserCode]
	public override int GetHashCode()
	{
		int num = 1;
		if (LandId != 0)
		{
			num ^= LandId.GetHashCode();
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
		if (LandId != 0)
		{
			output.WriteRawTag(8);
			output.WriteInt32(LandId);
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
		if (LandId != 0)
		{
			num += 1 + CodedOutputStream.ComputeInt32Size(LandId);
		}
		if (_unknownFields != null)
		{
			num += _unknownFields.CalculateSize();
		}
		return num;
	}

	[DebuggerNonUserCode]
	public void MergeFrom(EnterIrrigateReq_1013014 other)
	{
		if (other != null)
		{
			if (other.LandId != 0)
			{
				LandId = other.LandId;
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
				LandId = input.ReadInt32();
			}
		}
	}
}
