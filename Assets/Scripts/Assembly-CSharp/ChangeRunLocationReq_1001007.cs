using System;
using System.Diagnostics;
using Google.Protobuf;
using Google.Protobuf.Reflection;

public sealed class ChangeRunLocationReq_1001007 : IMessage<ChangeRunLocationReq_1001007>, IMessage, IEquatable<ChangeRunLocationReq_1001007>, IDeepCloneable<ChangeRunLocationReq_1001007>
{
	private static readonly MessageParser<ChangeRunLocationReq_1001007> _parser = new MessageParser<ChangeRunLocationReq_1001007>(() => new ChangeRunLocationReq_1001007());

	private UnknownFieldSet _unknownFields;

	public const int NewLocationFieldNumber = 1;

	private int newLocation_;

	[DebuggerNonUserCode]
	public static MessageParser<ChangeRunLocationReq_1001007> Parser => _parser;

	[DebuggerNonUserCode]
	public static MessageDescriptor Descriptor => SimpleActionProtoReflection.Descriptor.MessageTypes[7];

	[DebuggerNonUserCode]
	MessageDescriptor IMessage.Descriptor => pb_003A_003AGoogle_002EProtobuf_002EIMessage_002Eget_Descriptor();

	[DebuggerNonUserCode]
	public int NewLocation
	{
		get
		{
			return newLocation_;
		}
		set
		{
			newLocation_ = value;
		}
	}

	private MessageDescriptor pb_003A_003AGoogle_002EProtobuf_002EIMessage_002Eget_Descriptor()
	{
		return Descriptor;
	}

	[DebuggerNonUserCode]
	public ChangeRunLocationReq_1001007()
	{
	}

	[DebuggerNonUserCode]
	public ChangeRunLocationReq_1001007(ChangeRunLocationReq_1001007 other)
		: this()
	{
		newLocation_ = other.newLocation_;
		_unknownFields = UnknownFieldSet.Clone(other._unknownFields);
	}

	[DebuggerNonUserCode]
	public ChangeRunLocationReq_1001007 Clone()
	{
		return new ChangeRunLocationReq_1001007(this);
	}

	[DebuggerNonUserCode]
	public override bool Equals(object other)
	{
		return Equals(other as ChangeRunLocationReq_1001007);
	}

	[DebuggerNonUserCode]
	public bool Equals(ChangeRunLocationReq_1001007 other)
	{
		if (other == null)
		{
			return false;
		}
		if (other == this)
		{
			return true;
		}
		if (NewLocation != other.NewLocation)
		{
			return false;
		}
		return object.Equals(_unknownFields, other._unknownFields);
	}

	[DebuggerNonUserCode]
	public override int GetHashCode()
	{
		int num = 1;
		if (NewLocation != 0)
		{
			num ^= NewLocation.GetHashCode();
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
		if (NewLocation != 0)
		{
			output.WriteRawTag(8);
			output.WriteInt32(NewLocation);
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
		if (NewLocation != 0)
		{
			num += 1 + CodedOutputStream.ComputeInt32Size(NewLocation);
		}
		if (_unknownFields != null)
		{
			num += _unknownFields.CalculateSize();
		}
		return num;
	}

	[DebuggerNonUserCode]
	public void MergeFrom(ChangeRunLocationReq_1001007 other)
	{
		if (other != null)
		{
			if (other.NewLocation != 0)
			{
				NewLocation = other.NewLocation;
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
			if (num != 8)
			{
				_unknownFields = UnknownFieldSet.MergeFieldFrom(_unknownFields, input);
			}
			else
			{
				NewLocation = input.ReadInt32();
			}
		}
	}
}
