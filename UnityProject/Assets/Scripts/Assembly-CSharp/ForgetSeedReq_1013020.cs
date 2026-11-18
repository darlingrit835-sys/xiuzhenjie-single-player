using System;
using System.Diagnostics;
using Google.Protobuf;
using Google.Protobuf.Reflection;

public sealed class ForgetSeedReq_1013020 : IMessage<ForgetSeedReq_1013020>, IMessage, IEquatable<ForgetSeedReq_1013020>, IDeepCloneable<ForgetSeedReq_1013020>
{
	private static readonly MessageParser<ForgetSeedReq_1013020> _parser = new MessageParser<ForgetSeedReq_1013020>(() => new ForgetSeedReq_1013020());

	private UnknownFieldSet _unknownFields;

	public const int SeedIdFieldNumber = 1;

	private string seedId_ = "";

	[DebuggerNonUserCode]
	public static MessageParser<ForgetSeedReq_1013020> Parser => _parser;

	[DebuggerNonUserCode]
	public static MessageDescriptor Descriptor => PlantProtoReflection.Descriptor.MessageTypes[24];

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
	public ForgetSeedReq_1013020()
	{
	}

	[DebuggerNonUserCode]
	public ForgetSeedReq_1013020(ForgetSeedReq_1013020 other)
		: this()
	{
		seedId_ = other.seedId_;
		_unknownFields = UnknownFieldSet.Clone(other._unknownFields);
	}

	[DebuggerNonUserCode]
	public ForgetSeedReq_1013020 Clone()
	{
		return new ForgetSeedReq_1013020(this);
	}

	[DebuggerNonUserCode]
	public override bool Equals(object other)
	{
		return Equals(other as ForgetSeedReq_1013020);
	}

	[DebuggerNonUserCode]
	public bool Equals(ForgetSeedReq_1013020 other)
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
	public void MergeFrom(ForgetSeedReq_1013020 other)
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
