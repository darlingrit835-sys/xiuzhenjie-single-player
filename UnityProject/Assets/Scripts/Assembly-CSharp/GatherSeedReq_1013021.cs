using System;
using System.Diagnostics;
using Google.Protobuf;
using Google.Protobuf.Reflection;

public sealed class GatherSeedReq_1013021 : IMessage<GatherSeedReq_1013021>, IMessage, IEquatable<GatherSeedReq_1013021>, IDeepCloneable<GatherSeedReq_1013021>
{
	private static readonly MessageParser<GatherSeedReq_1013021> _parser = new MessageParser<GatherSeedReq_1013021>(() => new GatherSeedReq_1013021());

	private UnknownFieldSet _unknownFields;

	public const int SeedIdFieldNumber = 1;

	private string seedId_ = "";

	[DebuggerNonUserCode]
	public static MessageParser<GatherSeedReq_1013021> Parser => _parser;

	[DebuggerNonUserCode]
	public static MessageDescriptor Descriptor => PlantProtoReflection.Descriptor.MessageTypes[26];

	[DebuggerNonUserCode]
	MessageDescriptor IMessage.Descriptor => pb_003A_003AGoogle_002EProtobuf_002EIMessage_002Eget_Descriptor();

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
	public GatherSeedReq_1013021()
	{
	}

	[DebuggerNonUserCode]
	public GatherSeedReq_1013021(GatherSeedReq_1013021 other)
		: this()
	{
		seedId_ = other.seedId_;
		_unknownFields = UnknownFieldSet.Clone(other._unknownFields);
	}

	[DebuggerNonUserCode]
	public GatherSeedReq_1013021 Clone()
	{
		return new GatherSeedReq_1013021(this);
	}

	[DebuggerNonUserCode]
	public override bool Equals(object other)
	{
		return Equals(other as GatherSeedReq_1013021);
	}

	[DebuggerNonUserCode]
	public bool Equals(GatherSeedReq_1013021 other)
	{
		if (other == null)
		{
			return false;
		}
		if (other == this)
		{
			return true;
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
		if (SeedId.Length != 0)
		{
			output.WriteRawTag(10);
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
	public void MergeFrom(GatherSeedReq_1013021 other)
	{
		if (other != null)
		{
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
			if (num != 10)
			{
				_unknownFields = UnknownFieldSet.MergeFieldFrom(_unknownFields, input);
			}
			else
			{
				SeedId = input.ReadString();
			}
		}
	}
}
