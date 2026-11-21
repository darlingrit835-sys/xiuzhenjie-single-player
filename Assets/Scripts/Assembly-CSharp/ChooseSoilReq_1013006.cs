using System;
using System.Diagnostics;
using Google.Protobuf;
using Google.Protobuf.Reflection;

public sealed class ChooseSoilReq_1013006 : IMessage<ChooseSoilReq_1013006>, IMessage, IEquatable<ChooseSoilReq_1013006>, IDeepCloneable<ChooseSoilReq_1013006>
{
	private static readonly MessageParser<ChooseSoilReq_1013006> _parser = new MessageParser<ChooseSoilReq_1013006>(() => new ChooseSoilReq_1013006());

	private UnknownFieldSet _unknownFields;

	public const int LandIdFieldNumber = 1;

	private int landId_;

	public const int SoilIdFieldNumber = 2;

	private string soilId_ = "";

	[DebuggerNonUserCode]
	public static MessageParser<ChooseSoilReq_1013006> Parser => _parser;

	[DebuggerNonUserCode]
	public static MessageDescriptor Descriptor => PlantProtoReflection.Descriptor.MessageTypes[5];

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

	[DebuggerNonUserCode]
	public string SoilId
	{
		get
		{
			return soilId_;
		}
		set
		{
			soilId_ = ProtoPreconditions.CheckNotNull(value, "value");
		}
	}

	private MessageDescriptor pb_003A_003AGoogle_002EProtobuf_002EIMessage_002Eget_Descriptor()
	{
		return Descriptor;
	}

	[DebuggerNonUserCode]
	public ChooseSoilReq_1013006()
	{
	}

	[DebuggerNonUserCode]
	public ChooseSoilReq_1013006(ChooseSoilReq_1013006 other)
		: this()
	{
		landId_ = other.landId_;
		soilId_ = other.soilId_;
		_unknownFields = UnknownFieldSet.Clone(other._unknownFields);
	}

	[DebuggerNonUserCode]
	public ChooseSoilReq_1013006 Clone()
	{
		return new ChooseSoilReq_1013006(this);
	}

	[DebuggerNonUserCode]
	public override bool Equals(object other)
	{
		return Equals(other as ChooseSoilReq_1013006);
	}

	[DebuggerNonUserCode]
	public bool Equals(ChooseSoilReq_1013006 other)
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
		if (SoilId != other.SoilId)
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
		if (SoilId.Length != 0)
		{
			num ^= SoilId.GetHashCode();
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
		if (SoilId.Length != 0)
		{
			output.WriteRawTag(18);
			output.WriteString(SoilId);
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
		if (SoilId.Length != 0)
		{
			num += 1 + CodedOutputStream.ComputeStringSize(SoilId);
		}
		if (_unknownFields != null)
		{
			num += _unknownFields.CalculateSize();
		}
		return num;
	}

	[DebuggerNonUserCode]
	public void MergeFrom(ChooseSoilReq_1013006 other)
	{
		if (other != null)
		{
			if (other.LandId != 0)
			{
				LandId = other.LandId;
			}
			if (other.SoilId.Length != 0)
			{
				SoilId = other.SoilId;
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
				LandId = input.ReadInt32();
				break;
			case 18u:
				SoilId = input.ReadString();
				break;
			}
		}
	}
}
