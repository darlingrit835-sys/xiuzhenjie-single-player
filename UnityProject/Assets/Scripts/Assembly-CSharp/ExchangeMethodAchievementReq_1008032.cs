using System;
using System.Diagnostics;
using Google.Protobuf;
using Google.Protobuf.Reflection;

public sealed class ExchangeMethodAchievementReq_1008032 : IMessage<ExchangeMethodAchievementReq_1008032>, IMessage, IEquatable<ExchangeMethodAchievementReq_1008032>, IDeepCloneable<ExchangeMethodAchievementReq_1008032>
{
	private static readonly MessageParser<ExchangeMethodAchievementReq_1008032> _parser = new MessageParser<ExchangeMethodAchievementReq_1008032>(() => new ExchangeMethodAchievementReq_1008032());

	private UnknownFieldSet _unknownFields;

	public const int MethodAchievementIdFieldNumber = 1;

	private string methodAchievementId_ = "";

	public const int CountFieldNumber = 2;

	private int count_;

	[DebuggerNonUserCode]
	public static MessageParser<ExchangeMethodAchievementReq_1008032> Parser => _parser;

	[DebuggerNonUserCode]
	public static MessageDescriptor Descriptor => UnrealWorldProtoReflection.Descriptor.MessageTypes[37];

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
	public int Count
	{
		get
		{
			return count_;
		}
		set
		{
			count_ = value;
		}
	}

	private MessageDescriptor pb_003A_003AGoogle_002EProtobuf_002EIMessage_002Eget_Descriptor()
	{
		return Descriptor;
	}

	[DebuggerNonUserCode]
	public ExchangeMethodAchievementReq_1008032()
	{
	}

	[DebuggerNonUserCode]
	public ExchangeMethodAchievementReq_1008032(ExchangeMethodAchievementReq_1008032 other)
		: this()
	{
		methodAchievementId_ = other.methodAchievementId_;
		count_ = other.count_;
		_unknownFields = UnknownFieldSet.Clone(other._unknownFields);
	}

	[DebuggerNonUserCode]
	public ExchangeMethodAchievementReq_1008032 Clone()
	{
		return new ExchangeMethodAchievementReq_1008032(this);
	}

	[DebuggerNonUserCode]
	public override bool Equals(object other)
	{
		return Equals(other as ExchangeMethodAchievementReq_1008032);
	}

	[DebuggerNonUserCode]
	public bool Equals(ExchangeMethodAchievementReq_1008032 other)
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
		if (Count != other.Count)
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
		if (Count != 0)
		{
			num ^= Count.GetHashCode();
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
		if (Count != 0)
		{
			output.WriteRawTag(16);
			output.WriteInt32(Count);
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
		if (Count != 0)
		{
			num += 1 + CodedOutputStream.ComputeInt32Size(Count);
		}
		if (_unknownFields != null)
		{
			num += _unknownFields.CalculateSize();
		}
		return num;
	}

	[DebuggerNonUserCode]
	public void MergeFrom(ExchangeMethodAchievementReq_1008032 other)
	{
		if (other != null)
		{
			if (other.MethodAchievementId.Length != 0)
			{
				MethodAchievementId = other.MethodAchievementId;
			}
			if (other.Count != 0)
			{
				Count = other.Count;
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
				Count = input.ReadInt32();
				break;
			}
		}
	}
}
