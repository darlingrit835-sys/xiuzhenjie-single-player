using System;
using System.Diagnostics;
using Google.Protobuf;
using Google.Protobuf.Collections;
using Google.Protobuf.Reflection;

public sealed class VerifyCompletionMethodAchievementReq_1008080 : IMessage<VerifyCompletionMethodAchievementReq_1008080>, IMessage, IEquatable<VerifyCompletionMethodAchievementReq_1008080>, IDeepCloneable<VerifyCompletionMethodAchievementReq_1008080>
{
	private static readonly MessageParser<VerifyCompletionMethodAchievementReq_1008080> _parser = new MessageParser<VerifyCompletionMethodAchievementReq_1008080>(() => new VerifyCompletionMethodAchievementReq_1008080());

	private UnknownFieldSet _unknownFields;

	public const int MethodAchievementIdFieldNumber = 1;

	private string methodAchievementId_ = "";

	public const int AcupointIdFieldNumber = 2;

	private static readonly FieldCodec<string> _repeated_acupointId_codec = FieldCodec.ForString(18u);

	private readonly RepeatedField<string> acupointId_ = new RepeatedField<string>();

	[DebuggerNonUserCode]
	public static MessageParser<VerifyCompletionMethodAchievementReq_1008080> Parser => _parser;

	[DebuggerNonUserCode]
	public static MessageDescriptor Descriptor => UnrealWorldProtoReflection.Descriptor.MessageTypes[95];

	[DebuggerNonUserCode]
	MessageDescriptor IMessage.Descriptor => pb_003A_003AGoogle_002EProtobuf_002EIMessage_002Eget_Descriptor();

	[DebuggerNonUserCode]
	public string MethodAchievementId
	{
		get
		{
			return methodAchievementId_;
		}
		set
		{
			methodAchievementId_ = ProtoPreconditions.CheckNotNull(value, "value");
		}
	}

	[DebuggerNonUserCode]
	public RepeatedField<string> AcupointId => acupointId_;

	private MessageDescriptor pb_003A_003AGoogle_002EProtobuf_002EIMessage_002Eget_Descriptor()
	{
		return Descriptor;
	}

	[DebuggerNonUserCode]
	public VerifyCompletionMethodAchievementReq_1008080()
	{
	}

	[DebuggerNonUserCode]
	public VerifyCompletionMethodAchievementReq_1008080(VerifyCompletionMethodAchievementReq_1008080 other)
		: this()
	{
		methodAchievementId_ = other.methodAchievementId_;
		acupointId_ = other.acupointId_.Clone();
		_unknownFields = UnknownFieldSet.Clone(other._unknownFields);
	}

	[DebuggerNonUserCode]
	public VerifyCompletionMethodAchievementReq_1008080 Clone()
	{
		return new VerifyCompletionMethodAchievementReq_1008080(this);
	}

	[DebuggerNonUserCode]
	public override bool Equals(object other)
	{
		return Equals(other as VerifyCompletionMethodAchievementReq_1008080);
	}

	[DebuggerNonUserCode]
	public bool Equals(VerifyCompletionMethodAchievementReq_1008080 other)
	{
		if (other == null)
		{
			return false;
		}
		if (other == this)
		{
			return true;
		}
		if (MethodAchievementId != other.MethodAchievementId)
		{
			return false;
		}
		if (!acupointId_.Equals(other.acupointId_))
		{
			return false;
		}
		return object.Equals(_unknownFields, other._unknownFields);
	}

	[DebuggerNonUserCode]
	public override int GetHashCode()
	{
		int num = 1;
		if (MethodAchievementId.Length != 0)
		{
			num ^= MethodAchievementId.GetHashCode();
		}
		num ^= acupointId_.GetHashCode();
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
		if (MethodAchievementId.Length != 0)
		{
			output.WriteRawTag(10);
			output.WriteString(MethodAchievementId);
		}
		acupointId_.WriteTo(output, _repeated_acupointId_codec);
		if (_unknownFields != null)
		{
			_unknownFields.WriteTo(output);
		}
	}

	[DebuggerNonUserCode]
	public int CalculateSize()
	{
		int num = 0;
		if (MethodAchievementId.Length != 0)
		{
			num += 1 + CodedOutputStream.ComputeStringSize(MethodAchievementId);
		}
		num += acupointId_.CalculateSize(_repeated_acupointId_codec);
		if (_unknownFields != null)
		{
			num += _unknownFields.CalculateSize();
		}
		return num;
	}

	[DebuggerNonUserCode]
	public void MergeFrom(VerifyCompletionMethodAchievementReq_1008080 other)
	{
		if (other != null)
		{
			if (other.MethodAchievementId.Length != 0)
			{
				MethodAchievementId = other.MethodAchievementId;
			}
			acupointId_.Add(other.acupointId_);
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
				MethodAchievementId = input.ReadString();
				break;
			case 18u:
				acupointId_.AddEntriesFrom(input, _repeated_acupointId_codec);
				break;
			}
		}
	}
}
