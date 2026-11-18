using System;
using System.Diagnostics;
using Google.Protobuf;
using Google.Protobuf.Reflection;

public sealed class SearchEquipmentRefiningStepListReq_1005053 : IMessage<SearchEquipmentRefiningStepListReq_1005053>, IMessage, IEquatable<SearchEquipmentRefiningStepListReq_1005053>, IDeepCloneable<SearchEquipmentRefiningStepListReq_1005053>
{
	private static readonly MessageParser<SearchEquipmentRefiningStepListReq_1005053> _parser = new MessageParser<SearchEquipmentRefiningStepListReq_1005053>(() => new SearchEquipmentRefiningStepListReq_1005053());

	private UnknownFieldSet _unknownFields;

	public const int MethodIdFieldNumber = 1;

	private string methodId_ = "";

	[DebuggerNonUserCode]
	public static MessageParser<SearchEquipmentRefiningStepListReq_1005053> Parser => _parser;

	[DebuggerNonUserCode]
	public static MessageDescriptor Descriptor => HomeProtoReflection.Descriptor.MessageTypes[70];

	[DebuggerNonUserCode]
	MessageDescriptor IMessage.Descriptor => pb_003A_003AGoogle_002EProtobuf_002EIMessage_002Eget_Descriptor();

	[DebuggerNonUserCode]
	public string MethodId
	{
		get
		{
			return methodId_;
		}
		set
		{
			methodId_ = ProtoPreconditions.CheckNotNull(value, "value");
		}
	}

	private MessageDescriptor pb_003A_003AGoogle_002EProtobuf_002EIMessage_002Eget_Descriptor()
	{
		return Descriptor;
	}

	[DebuggerNonUserCode]
	public SearchEquipmentRefiningStepListReq_1005053()
	{
	}

	[DebuggerNonUserCode]
	public SearchEquipmentRefiningStepListReq_1005053(SearchEquipmentRefiningStepListReq_1005053 other)
		: this()
	{
		methodId_ = other.methodId_;
		_unknownFields = UnknownFieldSet.Clone(other._unknownFields);
	}

	[DebuggerNonUserCode]
	public SearchEquipmentRefiningStepListReq_1005053 Clone()
	{
		return new SearchEquipmentRefiningStepListReq_1005053(this);
	}

	[DebuggerNonUserCode]
	public override bool Equals(object other)
	{
		return Equals(other as SearchEquipmentRefiningStepListReq_1005053);
	}

	[DebuggerNonUserCode]
	public bool Equals(SearchEquipmentRefiningStepListReq_1005053 other)
	{
		if (other == null)
		{
			return false;
		}
		if (other == this)
		{
			return true;
		}
		if (MethodId != other.MethodId)
		{
			return false;
		}
		return object.Equals(_unknownFields, other._unknownFields);
	}

	[DebuggerNonUserCode]
	public override int GetHashCode()
	{
		int num = 1;
		if (MethodId.Length != 0)
		{
			num ^= MethodId.GetHashCode();
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
		if (MethodId.Length != 0)
		{
			output.WriteRawTag(10);
			output.WriteString(MethodId);
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
		if (MethodId.Length != 0)
		{
			num += 1 + CodedOutputStream.ComputeStringSize(MethodId);
		}
		if (_unknownFields != null)
		{
			num += _unknownFields.CalculateSize();
		}
		return num;
	}

	[DebuggerNonUserCode]
	public void MergeFrom(SearchEquipmentRefiningStepListReq_1005053 other)
	{
		if (other != null)
		{
			if (other.MethodId.Length != 0)
			{
				MethodId = other.MethodId;
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
				MethodId = input.ReadString();
			}
		}
	}
}
