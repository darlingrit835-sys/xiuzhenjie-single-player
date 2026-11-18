using System;
using System.Diagnostics;
using Google.Protobuf;
using Google.Protobuf.Reflection;

public sealed class DeleteMethodAchievementReq_1008093 : IMessage<DeleteMethodAchievementReq_1008093>, IMessage, IEquatable<DeleteMethodAchievementReq_1008093>, IDeepCloneable<DeleteMethodAchievementReq_1008093>
{
	private static readonly MessageParser<DeleteMethodAchievementReq_1008093> _parser = new MessageParser<DeleteMethodAchievementReq_1008093>(() => new DeleteMethodAchievementReq_1008093());

	private UnknownFieldSet _unknownFields;

	public const int MethodAchievementIdFieldNumber = 1;

	private string methodAchievementId_ = "";

	public const int PartFieldNumber = 2;

	private int part_;

	[DebuggerNonUserCode]
	public static MessageParser<DeleteMethodAchievementReq_1008093> Parser => _parser;

	[DebuggerNonUserCode]
	public static MessageDescriptor Descriptor => UnrealWorldProtoReflection.Descriptor.MessageTypes[114];

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
	public int Part
	{
		get
		{
			return part_;
		}
		set
		{
			part_ = value;
		}
	}

	private MessageDescriptor pb_003A_003AGoogle_002EProtobuf_002EIMessage_002Eget_Descriptor()
	{
		return Descriptor;
	}

	[DebuggerNonUserCode]
	public DeleteMethodAchievementReq_1008093()
	{
	}

	[DebuggerNonUserCode]
	public DeleteMethodAchievementReq_1008093(DeleteMethodAchievementReq_1008093 other)
		: this()
	{
		methodAchievementId_ = other.methodAchievementId_;
		part_ = other.part_;
		_unknownFields = UnknownFieldSet.Clone(other._unknownFields);
	}

	[DebuggerNonUserCode]
	public DeleteMethodAchievementReq_1008093 Clone()
	{
		return new DeleteMethodAchievementReq_1008093(this);
	}

	[DebuggerNonUserCode]
	public override bool Equals(object other)
	{
		return Equals(other as DeleteMethodAchievementReq_1008093);
	}

	[DebuggerNonUserCode]
	public bool Equals(DeleteMethodAchievementReq_1008093 other)
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
		if (Part != other.Part)
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
		if (Part != 0)
		{
			num ^= Part.GetHashCode();
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
		if (MethodAchievementId.Length != 0)
		{
			output.WriteRawTag(10);
			output.WriteString(MethodAchievementId);
		}
		if (Part != 0)
		{
			output.WriteRawTag(16);
			output.WriteInt32(Part);
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
		if (MethodAchievementId.Length != 0)
		{
			num += 1 + CodedOutputStream.ComputeStringSize(MethodAchievementId);
		}
		if (Part != 0)
		{
			num += 1 + CodedOutputStream.ComputeInt32Size(Part);
		}
		if (_unknownFields != null)
		{
			num += _unknownFields.CalculateSize();
		}
		return num;
	}

	[DebuggerNonUserCode]
	public void MergeFrom(DeleteMethodAchievementReq_1008093 other)
	{
		if (other != null)
		{
			if (other.MethodAchievementId.Length != 0)
			{
				MethodAchievementId = other.MethodAchievementId;
			}
			if (other.Part != 0)
			{
				Part = other.Part;
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
				MethodAchievementId = input.ReadString();
				break;
			case 16u:
				Part = input.ReadInt32();
				break;
			}
		}
	}
}
