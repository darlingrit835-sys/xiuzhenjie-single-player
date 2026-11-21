using System;
using System.Diagnostics;
using Google.Protobuf;
using Google.Protobuf.Reflection;

public sealed class SeedChooseReq_1013013 : IMessage<SeedChooseReq_1013013>, IMessage, IEquatable<SeedChooseReq_1013013>, IDeepCloneable<SeedChooseReq_1013013>
{
	private static readonly MessageParser<SeedChooseReq_1013013> _parser = new MessageParser<SeedChooseReq_1013013>(() => new SeedChooseReq_1013013());

	private UnknownFieldSet _unknownFields;

	public const int LandIdFieldNumber = 1;

	private int landId_;

	public const int SeedIdFieldNumber = 2;

	private string seedId_ = "";

	[DebuggerNonUserCode]
	public static MessageParser<SeedChooseReq_1013013> Parser => _parser;

	[DebuggerNonUserCode]
	public static MessageDescriptor Descriptor => PlantProtoReflection.Descriptor.MessageTypes[14];

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
	public string SeedId
	{
		get
		{
			return seedId_;
		}
		set
		{
			seedId_ = ProtoPreconditions.CheckNotNull(value, "value");
		}
	}

	private MessageDescriptor pb_003A_003AGoogle_002EProtobuf_002EIMessage_002Eget_Descriptor()
	{
		return Descriptor;
	}

	[DebuggerNonUserCode]
	public SeedChooseReq_1013013()
	{
	}

	[DebuggerNonUserCode]
	public SeedChooseReq_1013013(SeedChooseReq_1013013 other)
		: this()
	{
		landId_ = other.landId_;
		seedId_ = other.seedId_;
		_unknownFields = UnknownFieldSet.Clone(other._unknownFields);
	}

	[DebuggerNonUserCode]
	public SeedChooseReq_1013013 Clone()
	{
		return new SeedChooseReq_1013013(this);
	}

	[DebuggerNonUserCode]
	public override bool Equals(object other)
	{
		return Equals(other as SeedChooseReq_1013013);
	}

	[DebuggerNonUserCode]
	public bool Equals(SeedChooseReq_1013013 other)
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
		if (SeedId != other.SeedId)
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
		if (SeedId.Length != 0)
		{
			num ^= SeedId.GetHashCode();
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
		if (SeedId.Length != 0)
		{
			output.WriteRawTag(18);
			output.WriteString(SeedId);
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
		if (SeedId.Length != 0)
		{
			num += 1 + CodedOutputStream.ComputeStringSize(SeedId);
		}
		if (_unknownFields != null)
		{
			num += _unknownFields.CalculateSize();
		}
		return num;
	}

	[DebuggerNonUserCode]
	public void MergeFrom(SeedChooseReq_1013013 other)
	{
		if (other != null)
		{
			if (other.LandId != 0)
			{
				LandId = other.LandId;
			}
			if (other.SeedId.Length != 0)
			{
				SeedId = other.SeedId;
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
				SeedId = input.ReadString();
				break;
			}
		}
	}
}
