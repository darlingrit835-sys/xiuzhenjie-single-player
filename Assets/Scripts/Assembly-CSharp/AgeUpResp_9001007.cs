using System;
using System.Diagnostics;
using Google.Protobuf;
using Google.Protobuf.Reflection;

public sealed class AgeUpResp_9001007 : IMessage<AgeUpResp_9001007>, IMessage, IEquatable<AgeUpResp_9001007>, IDeepCloneable<AgeUpResp_9001007>
{
	private static readonly MessageParser<AgeUpResp_9001007> _parser = new MessageParser<AgeUpResp_9001007>(() => new AgeUpResp_9001007());

	private UnknownFieldSet _unknownFields;

	public const int AgeFieldNumber = 1;

	private int age_;

	public const int MaxAgeFieldNumber = 2;

	private int maxAge_;

	[DebuggerNonUserCode]
	public static MessageParser<AgeUpResp_9001007> Parser => _parser;

	[DebuggerNonUserCode]
	public static MessageDescriptor Descriptor => ServerInitiativeMsgProtoReflection.Descriptor.MessageTypes[7];

	[DebuggerNonUserCode]
	MessageDescriptor IMessage.Descriptor => pb_003A_003AGoogle_002EProtobuf_002EIMessage_002Eget_Descriptor();

	[DebuggerNonUserCode]
	public int Age
	{
		get
		{
			return age_;
		}
		set
		{
			age_ = value;
		}
	}

	[DebuggerNonUserCode]
	public int MaxAge
	{
		get
		{
			return maxAge_;
		}
		set
		{
			maxAge_ = value;
		}
	}

	private MessageDescriptor pb_003A_003AGoogle_002EProtobuf_002EIMessage_002Eget_Descriptor()
	{
		return Descriptor;
	}

	[DebuggerNonUserCode]
	public AgeUpResp_9001007()
	{
	}

	[DebuggerNonUserCode]
	public AgeUpResp_9001007(AgeUpResp_9001007 other)
		: this()
	{
		age_ = other.age_;
		maxAge_ = other.maxAge_;
		_unknownFields = UnknownFieldSet.Clone(other._unknownFields);
	}

	[DebuggerNonUserCode]
	public AgeUpResp_9001007 Clone()
	{
		return new AgeUpResp_9001007(this);
	}

	[DebuggerNonUserCode]
	public override bool Equals(object other)
	{
		return Equals(other as AgeUpResp_9001007);
	}

	[DebuggerNonUserCode]
	public bool Equals(AgeUpResp_9001007 other)
	{
		if (other == null)
		{
			return false;
		}
		if (other == this)
		{
			return true;
		}
		if (Age != other.Age)
		{
			return false;
		}
		if (MaxAge != other.MaxAge)
		{
			return false;
		}
		return object.Equals(_unknownFields, other._unknownFields);
	}

	[DebuggerNonUserCode]
	public override int GetHashCode()
	{
		int num = 1;
		if (Age != 0)
		{
			num ^= Age.GetHashCode();
		}
		if (MaxAge != 0)
		{
			num ^= MaxAge.GetHashCode();
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
		if (Age != 0)
		{
			output.WriteRawTag(8);
			output.WriteInt32(Age);
		}
		if (MaxAge != 0)
		{
			output.WriteRawTag(16);
			output.WriteInt32(MaxAge);
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
		if (Age != 0)
		{
			num += 1 + CodedOutputStream.ComputeInt32Size(Age);
		}
		if (MaxAge != 0)
		{
			num += 1 + CodedOutputStream.ComputeInt32Size(MaxAge);
		}
		if (_unknownFields != null)
		{
			num += _unknownFields.CalculateSize();
		}
		return num;
	}

	[DebuggerNonUserCode]
	public void MergeFrom(AgeUpResp_9001007 other)
	{
		if (other != null)
		{
			if (other.Age != 0)
			{
				Age = other.Age;
			}
			if (other.MaxAge != 0)
			{
				MaxAge = other.MaxAge;
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
			case 8u:
				Age = input.ReadInt32();
				break;
			case 16u:
				MaxAge = input.ReadInt32();
				break;
			}
		}
	}
}
