using System;
using System.Diagnostics;
using Google.Protobuf;
using Google.Protobuf.Reflection;

public sealed class PracticeSkillReq_1005040 : IMessage<PracticeSkillReq_1005040>, IMessage, IEquatable<PracticeSkillReq_1005040>, IDeepCloneable<PracticeSkillReq_1005040>
{
	private static readonly MessageParser<PracticeSkillReq_1005040> _parser = new MessageParser<PracticeSkillReq_1005040>(() => new PracticeSkillReq_1005040());

	public int PracticeAction { get; set; }
	public int ExpectedRevision { get; set; }

	private UnknownFieldSet _unknownFields;

	public const int MethodSkillIdFieldNumber = 1;

	private string methodSkillId_ = "";

	public const int PracticeNumFieldNumber = 2;

	private int practiceNum_;

	[DebuggerNonUserCode]
	public static MessageParser<PracticeSkillReq_1005040> Parser => _parser;

	[DebuggerNonUserCode]
	public static MessageDescriptor Descriptor => HomeProtoReflection.Descriptor.MessageTypes[50];

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

	[DebuggerNonUserCode]
	public int PracticeNum
	{
		get
		{
			return practiceNum_;
		}
		set
		{
			practiceNum_ = value;
		}
	}

	private MessageDescriptor pb_003A_003AGoogle_002EProtobuf_002EIMessage_002Eget_Descriptor()
	{
		return Descriptor;
	}

	[DebuggerNonUserCode]
	public PracticeSkillReq_1005040()
	{
	}

	[DebuggerNonUserCode]
	public PracticeSkillReq_1005040(PracticeSkillReq_1005040 other)
		: this()
	{
		methodSkillId_ = other.methodSkillId_;
		practiceNum_ = other.practiceNum_;
		PracticeAction = other.PracticeAction;
		ExpectedRevision = other.ExpectedRevision;
		_unknownFields = UnknownFieldSet.Clone(other._unknownFields);
	}

	[DebuggerNonUserCode]
	public PracticeSkillReq_1005040 Clone()
	{
		return new PracticeSkillReq_1005040(this);
	}

	[DebuggerNonUserCode]
	public override bool Equals(object other)
	{
		return Equals(other as PracticeSkillReq_1005040);
	}

	[DebuggerNonUserCode]
	public bool Equals(PracticeSkillReq_1005040 other)
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
		if (PracticeNum != other.PracticeNum)
		{
			return false;
		}
		if (PracticeAction != other.PracticeAction) return false;
		if (ExpectedRevision != other.ExpectedRevision) return false;
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
		if (PracticeNum != 0)
		{
			num ^= PracticeNum.GetHashCode();
		}
		if (PracticeAction != 0) num ^= PracticeAction.GetHashCode();
		if (ExpectedRevision != 0) num ^= ExpectedRevision.GetHashCode();
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
		if (PracticeNum != 0)
		{
			output.WriteRawTag(16);
			output.WriteInt32(PracticeNum);
		}
		if (PracticeAction != 0) { output.WriteTag(3, WireFormat.WireType.Varint); output.WriteInt32(PracticeAction); }
		if (ExpectedRevision != 0) { output.WriteTag(4, WireFormat.WireType.Varint); output.WriteInt32(ExpectedRevision); }
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
		if (PracticeNum != 0)
		{
			num += 1 + CodedOutputStream.ComputeInt32Size(PracticeNum);
		}
		if (PracticeAction != 0) num += CodedOutputStream.ComputeTagSize(3) + CodedOutputStream.ComputeInt32Size(PracticeAction);
		if (ExpectedRevision != 0) num += CodedOutputStream.ComputeTagSize(4) + CodedOutputStream.ComputeInt32Size(ExpectedRevision);
		if (_unknownFields != null)
		{
			num += _unknownFields.CalculateSize();
		}
		return num;
	}

	[DebuggerNonUserCode]
	public void MergeFrom(PracticeSkillReq_1005040 other)
	{
		if (other != null)
		{
			if (other.MethodSkillId.Length != 0)
			{
				MethodSkillId = other.MethodSkillId;
			}
			if (other.PracticeNum != 0)
			{
				PracticeNum = other.PracticeNum;
			}
			if (other.PracticeAction != 0) PracticeAction = other.PracticeAction;
			if (other.ExpectedRevision != 0) ExpectedRevision = other.ExpectedRevision;
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
			case 24u: PracticeAction = input.ReadInt32(); break;
			case 32u: ExpectedRevision = input.ReadInt32(); break;
			default:
				_unknownFields = UnknownFieldSet.MergeFieldFrom(_unknownFields, input);
				break;
			case 10u:
				MethodSkillId = input.ReadString();
				break;
			case 16u:
				PracticeNum = input.ReadInt32();
				break;
			}
		}
	}
}
