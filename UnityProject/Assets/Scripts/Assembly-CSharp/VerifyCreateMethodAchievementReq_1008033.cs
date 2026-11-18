using System;
using System.Diagnostics;
using Google.Protobuf;
using Google.Protobuf.Collections;
using Google.Protobuf.Reflection;

public sealed class VerifyCreateMethodAchievementReq_1008033 : IMessage<VerifyCreateMethodAchievementReq_1008033>, IMessage, IEquatable<VerifyCreateMethodAchievementReq_1008033>, IDeepCloneable<VerifyCreateMethodAchievementReq_1008033>
{
	private static readonly MessageParser<VerifyCreateMethodAchievementReq_1008033> _parser = new MessageParser<VerifyCreateMethodAchievementReq_1008033>(() => new VerifyCreateMethodAchievementReq_1008033());

	private UnknownFieldSet _unknownFields;

	public const int StageFieldNumber = 1;

	private int stage_;

	public const int AcupointIdFieldNumber = 2;

	private static readonly FieldCodec<string> _repeated_acupointId_codec = FieldCodec.ForString(18u);

	private readonly RepeatedField<string> acupointId_ = new RepeatedField<string>();

	[DebuggerNonUserCode]
	public static MessageParser<VerifyCreateMethodAchievementReq_1008033> Parser => _parser;

	[DebuggerNonUserCode]
	public static MessageDescriptor Descriptor => UnrealWorldProtoReflection.Descriptor.MessageTypes[39];

	[DebuggerNonUserCode]
	MessageDescriptor IMessage.Descriptor => pb_003A_003AGoogle_002EProtobuf_002EIMessage_002Eget_Descriptor();

	[DebuggerNonUserCode]
	public int Stage
	{
		get
		{
			return stage_;
		}
		set
		{
			stage_ = value;
		}
	}

	[DebuggerNonUserCode]
	public RepeatedField<string> AcupointId => acupointId_;

	private MessageDescriptor pb_003A_003AGoogle_002EProtobuf_002EIMessage_002Eget_Descriptor()
	{
		return Descriptor;
	}

	[DebuggerNonUserCode]
	public VerifyCreateMethodAchievementReq_1008033()
	{
	}

	[DebuggerNonUserCode]
	public VerifyCreateMethodAchievementReq_1008033(VerifyCreateMethodAchievementReq_1008033 other)
		: this()
	{
		stage_ = other.stage_;
		acupointId_ = other.acupointId_.Clone();
		_unknownFields = UnknownFieldSet.Clone(other._unknownFields);
	}

	[DebuggerNonUserCode]
	public VerifyCreateMethodAchievementReq_1008033 Clone()
	{
		return new VerifyCreateMethodAchievementReq_1008033(this);
	}

	[DebuggerNonUserCode]
	public override bool Equals(object other)
	{
		return Equals(other as VerifyCreateMethodAchievementReq_1008033);
	}

	[DebuggerNonUserCode]
	public bool Equals(VerifyCreateMethodAchievementReq_1008033 other)
	{
		if (other == null)
		{
			return false;
		}
		if (other == this)
		{
			return true;
		}
		if (Stage != other.Stage)
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
		if (Stage != 0)
		{
			num ^= Stage.GetHashCode();
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
		if (Stage != 0)
		{
			output.WriteRawTag(8);
			output.WriteInt32(Stage);
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
		if (Stage != 0)
		{
			num += 1 + CodedOutputStream.ComputeInt32Size(Stage);
		}
		num += acupointId_.CalculateSize(_repeated_acupointId_codec);
		if (_unknownFields != null)
		{
			num += _unknownFields.CalculateSize();
		}
		return num;
	}

	[DebuggerNonUserCode]
	public void MergeFrom(VerifyCreateMethodAchievementReq_1008033 other)
	{
		if (other != null)
		{
			if (other.Stage != 0)
			{
				Stage = other.Stage;
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
			case 8u:
				Stage = input.ReadInt32();
				break;
			case 18u:
				acupointId_.AddEntriesFrom(input, _repeated_acupointId_codec);
				break;
			}
		}
	}
}
