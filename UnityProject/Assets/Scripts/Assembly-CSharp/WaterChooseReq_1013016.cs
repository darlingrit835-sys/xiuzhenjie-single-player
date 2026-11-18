using System;
using System.Diagnostics;
using Google.Protobuf;
using Google.Protobuf.Reflection;

public sealed class WaterChooseReq_1013016 : IMessage<WaterChooseReq_1013016>, IMessage, IEquatable<WaterChooseReq_1013016>, IDeepCloneable<WaterChooseReq_1013016>
{
	private static readonly MessageParser<WaterChooseReq_1013016> _parser = new MessageParser<WaterChooseReq_1013016>(() => new WaterChooseReq_1013016());

	private UnknownFieldSet _unknownFields;

	public const int LandIdFieldNumber = 1;

	private int landId_;

	public const int WaterIdFieldNumber = 2;

	private string waterId_ = "";

	[DebuggerNonUserCode]
	public static MessageParser<WaterChooseReq_1013016> Parser => _parser;

	[DebuggerNonUserCode]
	public static MessageDescriptor Descriptor => PlantProtoReflection.Descriptor.MessageTypes[18];

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
	public string WaterId
	{
		get
		{
			return waterId_;
		}
		set
		{
			waterId_ = ProtoPreconditions.CheckNotNull(value, "value");
		}
	}

	private MessageDescriptor pb_003A_003AGoogle_002EProtobuf_002EIMessage_002Eget_Descriptor()
	{
		return Descriptor;
	}

	[DebuggerNonUserCode]
	public WaterChooseReq_1013016()
	{
	}

	[DebuggerNonUserCode]
	public WaterChooseReq_1013016(WaterChooseReq_1013016 other)
		: this()
	{
		landId_ = other.landId_;
		waterId_ = other.waterId_;
		_unknownFields = UnknownFieldSet.Clone(other._unknownFields);
	}

	[DebuggerNonUserCode]
	public WaterChooseReq_1013016 Clone()
	{
		return new WaterChooseReq_1013016(this);
	}

	[DebuggerNonUserCode]
	public override bool Equals(object other)
	{
		return Equals(other as WaterChooseReq_1013016);
	}

	[DebuggerNonUserCode]
	public bool Equals(WaterChooseReq_1013016 other)
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
		if (WaterId != other.WaterId)
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
		if (WaterId.Length != 0)
		{
			num ^= WaterId.GetHashCode();
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
		if (WaterId.Length != 0)
		{
			output.WriteRawTag(18);
			output.WriteString(WaterId);
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
		if (WaterId.Length != 0)
		{
			num += 1 + CodedOutputStream.ComputeStringSize(WaterId);
		}
		if (_unknownFields != null)
		{
			num += _unknownFields.CalculateSize();
		}
		return num;
	}

	[DebuggerNonUserCode]
	public void MergeFrom(WaterChooseReq_1013016 other)
	{
		if (other != null)
		{
			if (other.LandId != 0)
			{
				LandId = other.LandId;
			}
			if (other.WaterId.Length != 0)
			{
				WaterId = other.WaterId;
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
				WaterId = input.ReadString();
				break;
			}
		}
	}
}
