using System;
using System.Diagnostics;
using Google.Protobuf;
using Google.Protobuf.Reflection;

public sealed class AchievementAcupointReq_1008083 : IMessage<AchievementAcupointReq_1008083>, IMessage, IEquatable<AchievementAcupointReq_1008083>, IDeepCloneable<AchievementAcupointReq_1008083>
{
	private static readonly MessageParser<AchievementAcupointReq_1008083> _parser = new MessageParser<AchievementAcupointReq_1008083>(() => new AchievementAcupointReq_1008083());

	private UnknownFieldSet _unknownFields;

	public const int MethodAchievementIdFieldNumber = 1;

	private string methodAchievementId_ = "";

	[DebuggerNonUserCode]
	public static MessageParser<AchievementAcupointReq_1008083> Parser => _parser;

	[DebuggerNonUserCode]
	public static MessageDescriptor Descriptor => UnrealWorldProtoReflection.Descriptor.MessageTypes[99];

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

	private MessageDescriptor pb_003A_003AGoogle_002EProtobuf_002EIMessage_002Eget_Descriptor()
	{
		return Descriptor;
	}

	[DebuggerNonUserCode]
	public AchievementAcupointReq_1008083()
	{
	}

	[DebuggerNonUserCode]
	public AchievementAcupointReq_1008083(AchievementAcupointReq_1008083 other)
		: this()
	{
		methodAchievementId_ = other.methodAchievementId_;
		_unknownFields = UnknownFieldSet.Clone(other._unknownFields);
	}

	[DebuggerNonUserCode]
	public AchievementAcupointReq_1008083 Clone()
	{
		return new AchievementAcupointReq_1008083(this);
	}

	[DebuggerNonUserCode]
	public override bool Equals(object other)
	{
		return Equals(other as AchievementAcupointReq_1008083);
	}

	[DebuggerNonUserCode]
	public bool Equals(AchievementAcupointReq_1008083 other)
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
		if (_unknownFields != null)
		{
			num += _unknownFields.CalculateSize();
		}
		return num;
	}

	[DebuggerNonUserCode]
	public void MergeFrom(AchievementAcupointReq_1008083 other)
	{
		if (other != null)
		{
			if (other.MethodAchievementId.Length != 0)
			{
				MethodAchievementId = other.MethodAchievementId;
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
				MethodAchievementId = input.ReadString();
			}
		}
	}
}
