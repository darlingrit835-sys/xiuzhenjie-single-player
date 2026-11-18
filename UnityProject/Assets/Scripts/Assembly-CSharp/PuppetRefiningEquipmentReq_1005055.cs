using System;
using System.Diagnostics;
using Google.Protobuf;
using Google.Protobuf.Reflection;

public sealed class PuppetRefiningEquipmentReq_1005055 : IMessage<PuppetRefiningEquipmentReq_1005055>, IMessage, IEquatable<PuppetRefiningEquipmentReq_1005055>, IDeepCloneable<PuppetRefiningEquipmentReq_1005055>
{
	private static readonly MessageParser<PuppetRefiningEquipmentReq_1005055> _parser = new MessageParser<PuppetRefiningEquipmentReq_1005055>(() => new PuppetRefiningEquipmentReq_1005055());

	private UnknownFieldSet _unknownFields;

	public const int MethodEquipmentIdFieldNumber = 1;

	private string methodEquipmentId_ = "";

	[DebuggerNonUserCode]
	public static MessageParser<PuppetRefiningEquipmentReq_1005055> Parser => _parser;

	[DebuggerNonUserCode]
	public static MessageDescriptor Descriptor => HomeProtoReflection.Descriptor.MessageTypes[72];

	[DebuggerNonUserCode]
	MessageDescriptor IMessage.Descriptor => pb_003A_003AGoogle_002EProtobuf_002EIMessage_002Eget_Descriptor();

	[DebuggerNonUserCode]
	public string MethodEquipmentId
	{
		get
		{
			return methodEquipmentId_;
		}
		set
		{
			methodEquipmentId_ = ProtoPreconditions.CheckNotNull(value, "value");
		}
	}

	private MessageDescriptor pb_003A_003AGoogle_002EProtobuf_002EIMessage_002Eget_Descriptor()
	{
		return Descriptor;
	}

	[DebuggerNonUserCode]
	public PuppetRefiningEquipmentReq_1005055()
	{
	}

	[DebuggerNonUserCode]
	public PuppetRefiningEquipmentReq_1005055(PuppetRefiningEquipmentReq_1005055 other)
		: this()
	{
		methodEquipmentId_ = other.methodEquipmentId_;
		_unknownFields = UnknownFieldSet.Clone(other._unknownFields);
	}

	[DebuggerNonUserCode]
	public PuppetRefiningEquipmentReq_1005055 Clone()
	{
		return new PuppetRefiningEquipmentReq_1005055(this);
	}

	[DebuggerNonUserCode]
	public override bool Equals(object other)
	{
		return Equals(other as PuppetRefiningEquipmentReq_1005055);
	}

	[DebuggerNonUserCode]
	public bool Equals(PuppetRefiningEquipmentReq_1005055 other)
	{
		if (other == null)
		{
			return false;
		}
		if (other == this)
		{
			return true;
		}
		if (MethodEquipmentId != other.MethodEquipmentId)
		{
			return false;
		}
		return object.Equals(_unknownFields, other._unknownFields);
	}

	[DebuggerNonUserCode]
	public override int GetHashCode()
	{
		int num = 1;
		if (MethodEquipmentId.Length != 0)
		{
			num ^= MethodEquipmentId.GetHashCode();
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
		if (MethodEquipmentId.Length != 0)
		{
			output.WriteRawTag(10);
			output.WriteString(MethodEquipmentId);
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
		if (MethodEquipmentId.Length != 0)
		{
			num += 1 + CodedOutputStream.ComputeStringSize(MethodEquipmentId);
		}
		if (_unknownFields != null)
		{
			num += _unknownFields.CalculateSize();
		}
		return num;
	}

	[DebuggerNonUserCode]
	public void MergeFrom(PuppetRefiningEquipmentReq_1005055 other)
	{
		if (other != null)
		{
			if (other.MethodEquipmentId.Length != 0)
			{
				MethodEquipmentId = other.MethodEquipmentId;
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
				MethodEquipmentId = input.ReadString();
			}
		}
	}
}
