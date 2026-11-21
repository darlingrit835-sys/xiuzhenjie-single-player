using System;
using System.Diagnostics;
using Google.Protobuf;
using Google.Protobuf.Collections;
using Google.Protobuf.Reflection;

public sealed class VerifyCreateMethodSkillReq_1008025 : IMessage<VerifyCreateMethodSkillReq_1008025>, IMessage, IEquatable<VerifyCreateMethodSkillReq_1008025>, IDeepCloneable<VerifyCreateMethodSkillReq_1008025>
{
	private static readonly MessageParser<VerifyCreateMethodSkillReq_1008025> _parser = new MessageParser<VerifyCreateMethodSkillReq_1008025>(() => new VerifyCreateMethodSkillReq_1008025());

	private UnknownFieldSet _unknownFields;

	public const int StageFieldNumber = 1;

	private int stage_;

	public const int HandSealIdFieldNumber = 2;

	private static readonly FieldCodec<string> _repeated_handSealId_codec = FieldCodec.ForString(18u);

	private readonly RepeatedField<string> handSealId_ = new RepeatedField<string>();

	[DebuggerNonUserCode]
	public static MessageParser<VerifyCreateMethodSkillReq_1008025> Parser => _parser;

	[DebuggerNonUserCode]
	public static MessageDescriptor Descriptor => UnrealWorldProtoReflection.Descriptor.MessageTypes[28];

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
	public RepeatedField<string> HandSealId => handSealId_;

	private MessageDescriptor pb_003A_003AGoogle_002EProtobuf_002EIMessage_002Eget_Descriptor()
	{
		return Descriptor;
	}

	[DebuggerNonUserCode]
	public VerifyCreateMethodSkillReq_1008025()
	{
	}

	[DebuggerNonUserCode]
	public VerifyCreateMethodSkillReq_1008025(VerifyCreateMethodSkillReq_1008025 other)
		: this()
	{
		stage_ = other.stage_;
		handSealId_ = other.handSealId_.Clone();
		_unknownFields = UnknownFieldSet.Clone(other._unknownFields);
	}

	[DebuggerNonUserCode]
	public VerifyCreateMethodSkillReq_1008025 Clone()
	{
		return new VerifyCreateMethodSkillReq_1008025(this);
	}

	[DebuggerNonUserCode]
	public override bool Equals(object other)
	{
		return Equals(other as VerifyCreateMethodSkillReq_1008025);
	}

	[DebuggerNonUserCode]
	public bool Equals(VerifyCreateMethodSkillReq_1008025 other)
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
		if (!handSealId_.Equals(other.handSealId_))
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
		num ^= handSealId_.GetHashCode();
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
		handSealId_.WriteTo(output, _repeated_handSealId_codec);
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
		num += handSealId_.CalculateSize(_repeated_handSealId_codec);
		if (_unknownFields != null)
		{
			num += _unknownFields.CalculateSize();
		}
		return num;
	}

	[DebuggerNonUserCode]
	public void MergeFrom(VerifyCreateMethodSkillReq_1008025 other)
	{
		if (other != null)
		{
			if (other.Stage != 0)
			{
				Stage = other.Stage;
			}
			handSealId_.Add(other.handSealId_);
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
				handSealId_.AddEntriesFrom(input, _repeated_handSealId_codec);
				break;
			}
		}
	}
}
