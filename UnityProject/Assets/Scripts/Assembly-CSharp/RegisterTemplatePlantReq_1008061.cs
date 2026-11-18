using System;
using System.Diagnostics;
using Google.Protobuf;
using Google.Protobuf.Reflection;

public sealed class RegisterTemplatePlantReq_1008061 : IMessage<RegisterTemplatePlantReq_1008061>, IMessage, IEquatable<RegisterTemplatePlantReq_1008061>, IDeepCloneable<RegisterTemplatePlantReq_1008061>
{
	private static readonly MessageParser<RegisterTemplatePlantReq_1008061> _parser = new MessageParser<RegisterTemplatePlantReq_1008061>(() => new RegisterTemplatePlantReq_1008061());

	private UnknownFieldSet _unknownFields;

	public const int TemplatePlantIdFieldNumber = 1;

	private string templatePlantId_ = "";

	public const int RenameFieldNumber = 2;

	private string rename_ = "";

	[DebuggerNonUserCode]
	public static MessageParser<RegisterTemplatePlantReq_1008061> Parser => _parser;

	[DebuggerNonUserCode]
	public static MessageDescriptor Descriptor => UnrealWorldProtoReflection.Descriptor.MessageTypes[76];

	[DebuggerNonUserCode]
	MessageDescriptor IMessage.Descriptor => pb_003A_003AGoogle_002EProtobuf_002EIMessage_002Eget_Descriptor();

	[DebuggerNonUserCode]
	public string TemplatePlantId
	{
		get
		{
			return templatePlantId_;
		}
		set
		{
			templatePlantId_ = ProtoPreconditions.CheckNotNull(value, "value");
		}
	}

	[DebuggerNonUserCode]
	public string Rename
	{
		get
		{
			return rename_;
		}
		set
		{
			rename_ = ProtoPreconditions.CheckNotNull(value, "value");
		}
	}

	private MessageDescriptor pb_003A_003AGoogle_002EProtobuf_002EIMessage_002Eget_Descriptor()
	{
		return Descriptor;
	}

	[DebuggerNonUserCode]
	public RegisterTemplatePlantReq_1008061()
	{
	}

	[DebuggerNonUserCode]
	public RegisterTemplatePlantReq_1008061(RegisterTemplatePlantReq_1008061 other)
		: this()
	{
		templatePlantId_ = other.templatePlantId_;
		rename_ = other.rename_;
		_unknownFields = UnknownFieldSet.Clone(other._unknownFields);
	}

	[DebuggerNonUserCode]
	public RegisterTemplatePlantReq_1008061 Clone()
	{
		return new RegisterTemplatePlantReq_1008061(this);
	}

	[DebuggerNonUserCode]
	public override bool Equals(object other)
	{
		return Equals(other as RegisterTemplatePlantReq_1008061);
	}

	[DebuggerNonUserCode]
	public bool Equals(RegisterTemplatePlantReq_1008061 other)
	{
		if (other == null)
		{
			return false;
		}
		if (other == this)
		{
			return true;
		}
		if (TemplatePlantId != other.TemplatePlantId)
		{
			return false;
		}
		if (Rename != other.Rename)
		{
			return false;
		}
		return object.Equals(_unknownFields, other._unknownFields);
	}

	[DebuggerNonUserCode]
	public override int GetHashCode()
	{
		int num = 1;
		if (TemplatePlantId.Length != 0)
		{
			num ^= TemplatePlantId.GetHashCode();
		}
		if (Rename.Length != 0)
		{
			num ^= Rename.GetHashCode();
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
		if (TemplatePlantId.Length != 0)
		{
			output.WriteRawTag(10);
			output.WriteString(TemplatePlantId);
		}
		if (Rename.Length != 0)
		{
			output.WriteRawTag(18);
			output.WriteString(Rename);
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
		if (TemplatePlantId.Length != 0)
		{
			num += 1 + CodedOutputStream.ComputeStringSize(TemplatePlantId);
		}
		if (Rename.Length != 0)
		{
			num += 1 + CodedOutputStream.ComputeStringSize(Rename);
		}
		if (_unknownFields != null)
		{
			num += _unknownFields.CalculateSize();
		}
		return num;
	}

	[DebuggerNonUserCode]
	public void MergeFrom(RegisterTemplatePlantReq_1008061 other)
	{
		if (other != null)
		{
			if (other.TemplatePlantId.Length != 0)
			{
				TemplatePlantId = other.TemplatePlantId;
			}
			if (other.Rename.Length != 0)
			{
				Rename = other.Rename;
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
			case 10u:
				TemplatePlantId = input.ReadString();
				break;
			case 18u:
				Rename = input.ReadString();
				break;
			}
		}
	}
}
