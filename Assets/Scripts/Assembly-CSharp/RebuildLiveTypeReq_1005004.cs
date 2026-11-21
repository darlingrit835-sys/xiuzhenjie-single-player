using System;
using System.Diagnostics;
using Google.Protobuf;
using Google.Protobuf.Reflection;

public sealed class RebuildLiveTypeReq_1005004 : IMessage<RebuildLiveTypeReq_1005004>, IMessage, IEquatable<RebuildLiveTypeReq_1005004>, IDeepCloneable<RebuildLiveTypeReq_1005004>
{
	private static readonly MessageParser<RebuildLiveTypeReq_1005004> _parser = new MessageParser<RebuildLiveTypeReq_1005004>(() => new RebuildLiveTypeReq_1005004());

	private UnknownFieldSet _unknownFields;

	public const int NewLiveTypeFieldNumber = 1;

	private int newLiveType_;

	public const int LocationFieldNumber = 2;

	private int location_;

	[DebuggerNonUserCode]
	public static MessageParser<RebuildLiveTypeReq_1005004> Parser => _parser;

	[DebuggerNonUserCode]
	public static MessageDescriptor Descriptor => HomeProtoReflection.Descriptor.MessageTypes[4];

	[DebuggerNonUserCode]
	MessageDescriptor IMessage.Descriptor => pb_003A_003AGoogle_002EProtobuf_002EIMessage_002Eget_Descriptor();

	[DebuggerNonUserCode]
	public int NewLiveType
	{
		get
		{
			return newLiveType_;
		}
		set
		{
			newLiveType_ = value;
		}
	}

	[DebuggerNonUserCode]
	public int Location
	{
		get
		{
			return location_;
		}
		set
		{
			location_ = value;
		}
	}

	private MessageDescriptor pb_003A_003AGoogle_002EProtobuf_002EIMessage_002Eget_Descriptor()
	{
		return Descriptor;
	}

	[DebuggerNonUserCode]
	public RebuildLiveTypeReq_1005004()
	{
	}

	[DebuggerNonUserCode]
	public RebuildLiveTypeReq_1005004(RebuildLiveTypeReq_1005004 other)
		: this()
	{
		newLiveType_ = other.newLiveType_;
		location_ = other.location_;
		_unknownFields = UnknownFieldSet.Clone(other._unknownFields);
	}

	[DebuggerNonUserCode]
	public RebuildLiveTypeReq_1005004 Clone()
	{
		return new RebuildLiveTypeReq_1005004(this);
	}

	[DebuggerNonUserCode]
	public override bool Equals(object other)
	{
		return Equals(other as RebuildLiveTypeReq_1005004);
	}

	[DebuggerNonUserCode]
	public bool Equals(RebuildLiveTypeReq_1005004 other)
	{
		if (other == null)
		{
			return false;
		}
		if (other == this)
		{
			return true;
		}
		if (NewLiveType != other.NewLiveType)
		{
			return false;
		}
		if (Location != other.Location)
		{
			return false;
		}
		return object.Equals(_unknownFields, other._unknownFields);
	}

	[DebuggerNonUserCode]
	public override int GetHashCode()
	{
		int num = 1;
		if (NewLiveType != 0)
		{
			num ^= NewLiveType.GetHashCode();
		}
		if (Location != 0)
		{
			num ^= Location.GetHashCode();
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
		if (NewLiveType != 0)
		{
			output.WriteRawTag(8);
			output.WriteInt32(NewLiveType);
		}
		if (Location != 0)
		{
			output.WriteRawTag(16);
			output.WriteInt32(Location);
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
		if (NewLiveType != 0)
		{
			num += 1 + CodedOutputStream.ComputeInt32Size(NewLiveType);
		}
		if (Location != 0)
		{
			num += 1 + CodedOutputStream.ComputeInt32Size(Location);
		}
		if (_unknownFields != null)
		{
			num += _unknownFields.CalculateSize();
		}
		return num;
	}

	[DebuggerNonUserCode]
	public void MergeFrom(RebuildLiveTypeReq_1005004 other)
	{
		if (other != null)
		{
			if (other.NewLiveType != 0)
			{
				NewLiveType = other.NewLiveType;
			}
			if (other.Location != 0)
			{
				Location = other.Location;
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
				NewLiveType = input.ReadInt32();
				break;
			case 16u:
				Location = input.ReadInt32();
				break;
			}
		}
	}
}
