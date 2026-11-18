using System;
using System.Diagnostics;
using Google.Protobuf;
using Google.Protobuf.Reflection;

public sealed class EnterSoilReq_1013004 : IMessage<EnterSoilReq_1013004>, IMessage, IEquatable<EnterSoilReq_1013004>, IDeepCloneable<EnterSoilReq_1013004>
{
	private static readonly MessageParser<EnterSoilReq_1013004> _parser = new MessageParser<EnterSoilReq_1013004>(() => new EnterSoilReq_1013004());

	private UnknownFieldSet _unknownFields;

	public const int LandIdFieldNumber = 1;

	private int landId_;

	[DebuggerNonUserCode]
	public static MessageParser<EnterSoilReq_1013004> Parser => _parser;

	[DebuggerNonUserCode]
	public static MessageDescriptor Descriptor => PlantProtoReflection.Descriptor.MessageTypes[3];

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
	public EnterSoilReq_1013004()
	{
	}

	[DebuggerNonUserCode]
	public EnterSoilReq_1013004(EnterSoilReq_1013004 other)
		: this()
	{
		landId_ = other.landId_;
		_unknownFields = UnknownFieldSet.Clone(other._unknownFields);
	}

	[DebuggerNonUserCode]
	public EnterSoilReq_1013004 Clone()
	{
		return new EnterSoilReq_1013004(this);
	}

	[DebuggerNonUserCode]
	public override bool Equals(object other)
	{
		return Equals(other as EnterSoilReq_1013004);
	}

	[DebuggerNonUserCode]
	public bool Equals(EnterSoilReq_1013004 other)
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
	public void MergeFrom(EnterSoilReq_1013004 other)
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
