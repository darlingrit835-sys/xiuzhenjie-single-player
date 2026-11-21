using System;
using System.Diagnostics;
using Google.Protobuf;
using Google.Protobuf.Reflection;

public sealed class ExchangeMethodSkillReq_1008024 : IMessage<ExchangeMethodSkillReq_1008024>, IMessage, IEquatable<ExchangeMethodSkillReq_1008024>, IDeepCloneable<ExchangeMethodSkillReq_1008024>
{
	private static readonly MessageParser<ExchangeMethodSkillReq_1008024> _parser = new MessageParser<ExchangeMethodSkillReq_1008024>(() => new ExchangeMethodSkillReq_1008024());

	private UnknownFieldSet _unknownFields;

	public const int MethodSkillIdFieldNumber = 1;

	private string methodSkillId_ = "";

	[DebuggerNonUserCode]
	public static MessageParser<ExchangeMethodSkillReq_1008024> Parser => _parser;

	[DebuggerNonUserCode]
	public static MessageDescriptor Descriptor => UnrealWorldProtoReflection.Descriptor.MessageTypes[26];

	[DebuggerNonUserCode]
	MessageDescriptor IMessage.Descriptor => pb_003A_003AGoogle_002EProtobuf_002EIMessage_002Eget_Descriptor();

	[DebuggerNonUserCode]
	public string MethodSkillId
	{
		get
		{
			return methodSkillId_;
		}
		set
		{
			methodSkillId_ = ProtoPreconditions.CheckNotNull(value, "value");
		}
	}

	private MessageDescriptor pb_003A_003AGoogle_002EProtobuf_002EIMessage_002Eget_Descriptor()
	{
		return Descriptor;
	}

	[DebuggerNonUserCode]
	public ExchangeMethodSkillReq_1008024()
	{
	}

	[DebuggerNonUserCode]
	public ExchangeMethodSkillReq_1008024(ExchangeMethodSkillReq_1008024 other)
		: this()
	{
		methodSkillId_ = other.methodSkillId_;
		_unknownFields = UnknownFieldSet.Clone(other._unknownFields);
	}

	[DebuggerNonUserCode]
	public ExchangeMethodSkillReq_1008024 Clone()
	{
		return new ExchangeMethodSkillReq_1008024(this);
	}

	[DebuggerNonUserCode]
	public override bool Equals(object other)
	{
		return Equals(other as ExchangeMethodSkillReq_1008024);
	}

	[DebuggerNonUserCode]
	public bool Equals(ExchangeMethodSkillReq_1008024 other)
	{
		if (other == null)
		{
			return false;
		}
		if (other == this)
		{
			return true;
		}
		if (MethodSkillId != other.MethodSkillId)
		{
			return false;
		}
		return object.Equals(_unknownFields, other._unknownFields);
	}

	[DebuggerNonUserCode]
	public override int GetHashCode()
	{
		int num = 1;
		if (MethodSkillId.Length != 0)
		{
			num ^= MethodSkillId.GetHashCode();
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
		if (MethodSkillId.Length != 0)
		{
			output.WriteRawTag(10);
			output.WriteString(MethodSkillId);
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
		if (MethodSkillId.Length != 0)
		{
			num += 1 + CodedOutputStream.ComputeStringSize(MethodSkillId);
		}
		if (_unknownFields != null)
		{
			num += _unknownFields.CalculateSize();
		}
		return num;
	}

	[DebuggerNonUserCode]
	public void MergeFrom(ExchangeMethodSkillReq_1008024 other)
	{
		if (other != null)
		{
			if (other.MethodSkillId.Length != 0)
			{
				MethodSkillId = other.MethodSkillId;
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
				MethodSkillId = input.ReadString();
			}
		}
	}
}
