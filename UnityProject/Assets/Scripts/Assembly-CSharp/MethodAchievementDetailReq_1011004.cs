using System;
using System.Diagnostics;
using Google.Protobuf;
using Google.Protobuf.Reflection;

public sealed class MethodAchievementDetailReq_1011004 : IMessage<MethodAchievementDetailReq_1011004>, IMessage, IEquatable<MethodAchievementDetailReq_1011004>, IDeepCloneable<MethodAchievementDetailReq_1011004>
{
	private static readonly MessageParser<MethodAchievementDetailReq_1011004> _parser = new MessageParser<MethodAchievementDetailReq_1011004>(() => new MethodAchievementDetailReq_1011004());

	private UnknownFieldSet _unknownFields;

	public const int IdFieldNumber = 1;

	private string id_ = "";

	[DebuggerNonUserCode]
	public static MessageParser<MethodAchievementDetailReq_1011004> Parser => _parser;

	[DebuggerNonUserCode]
	public static MessageDescriptor Descriptor => TemplateProtoReflection.Descriptor.MessageTypes[6];

	[DebuggerNonUserCode]
	MessageDescriptor IMessage.Descriptor => pb_003A_003AGoogle_002EProtobuf_002EIMessage_002Eget_Descriptor();

	[DebuggerNonUserCode]
	public string Id
	{
		get
		{
			return id_;
		}
		set
		{
			id_ = ProtoPreconditions.CheckNotNull(value, "value");
		}
	}

	private MessageDescriptor pb_003A_003AGoogle_002EProtobuf_002EIMessage_002Eget_Descriptor()
	{
		return Descriptor;
	}

	[DebuggerNonUserCode]
	public MethodAchievementDetailReq_1011004()
	{
	}

	[DebuggerNonUserCode]
	public MethodAchievementDetailReq_1011004(MethodAchievementDetailReq_1011004 other)
		: this()
	{
		id_ = other.id_;
		_unknownFields = UnknownFieldSet.Clone(other._unknownFields);
	}

	[DebuggerNonUserCode]
	public MethodAchievementDetailReq_1011004 Clone()
	{
		return new MethodAchievementDetailReq_1011004(this);
	}

	[DebuggerNonUserCode]
	public override bool Equals(object other)
	{
		return Equals(other as MethodAchievementDetailReq_1011004);
	}

	[DebuggerNonUserCode]
	public bool Equals(MethodAchievementDetailReq_1011004 other)
	{
		if (other == null)
		{
			return false;
		}
		if (other == this)
		{
			return true;
		}
		if (Id != other.Id)
		{
			return false;
		}
		return object.Equals(_unknownFields, other._unknownFields);
	}

	[DebuggerNonUserCode]
	public override int GetHashCode()
	{
		int num = 1;
		if (Id.Length != 0)
		{
			num ^= Id.GetHashCode();
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
		if (Id.Length != 0)
		{
			output.WriteRawTag(10);
			output.WriteString(Id);
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
		if (Id.Length != 0)
		{
			num += 1 + CodedOutputStream.ComputeStringSize(Id);
		}
		if (_unknownFields != null)
		{
			num += _unknownFields.CalculateSize();
		}
		return num;
	}

	[DebuggerNonUserCode]
	public void MergeFrom(MethodAchievementDetailReq_1011004 other)
	{
		if (other != null)
		{
			if (other.Id.Length != 0)
			{
				Id = other.Id;
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
				Id = input.ReadString();
			}
		}
	}
}
