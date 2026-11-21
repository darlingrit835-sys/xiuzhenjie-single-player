using System;
using System.Diagnostics;
using Google.Protobuf;
using Google.Protobuf.Reflection;

public sealed class BuildLiveTypeReq_1005003 : IMessage<BuildLiveTypeReq_1005003>, IMessage, IEquatable<BuildLiveTypeReq_1005003>, IDeepCloneable<BuildLiveTypeReq_1005003>
{
	private static readonly MessageParser<BuildLiveTypeReq_1005003> _parser = new MessageParser<BuildLiveTypeReq_1005003>(() => new BuildLiveTypeReq_1005003());

	private UnknownFieldSet _unknownFields;

	public const int LiveTypeFieldNumber = 1;

	private int liveType_;

	public const int LocationFieldNumber = 2;

	private int location_;

	[DebuggerNonUserCode]
	public static MessageParser<BuildLiveTypeReq_1005003> Parser => _parser;

	[DebuggerNonUserCode]
	public static MessageDescriptor Descriptor => HomeProtoReflection.Descriptor.MessageTypes[2];

	[DebuggerNonUserCode]
	MessageDescriptor IMessage.Descriptor => pb_003A_003AGoogle_002EProtobuf_002EIMessage_002Eget_Descriptor();

	[DebuggerNonUserCode]
	public int LiveType
	{
		get
		{
			return liveType_;
		}
		set
		{
			liveType_ = value;
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
	public BuildLiveTypeReq_1005003()
	{
	}

	[DebuggerNonUserCode]
	public BuildLiveTypeReq_1005003(BuildLiveTypeReq_1005003 other)
		: this()
	{
		liveType_ = other.liveType_;
		location_ = other.location_;
		_unknownFields = UnknownFieldSet.Clone(other._unknownFields);
	}

	[DebuggerNonUserCode]
	public BuildLiveTypeReq_1005003 Clone()
	{
		return new BuildLiveTypeReq_1005003(this);
	}

	[DebuggerNonUserCode]
	public override bool Equals(object other)
	{
		return Equals(other as BuildLiveTypeReq_1005003);
	}

	[DebuggerNonUserCode]
	public bool Equals(BuildLiveTypeReq_1005003 other)
	{
		if (other == null)
		{
			return false;
		}
		if (other == this)
		{
			return true;
		}
		if (LiveType != other.LiveType)
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
		if (LiveType != 0)
		{
			num ^= LiveType.GetHashCode();
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
		if (LiveType != 0)
		{
			output.WriteRawTag(8);
			output.WriteInt32(LiveType);
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
		if (LiveType != 0)
		{
			num += 1 + CodedOutputStream.ComputeInt32Size(LiveType);
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
	public void MergeFrom(BuildLiveTypeReq_1005003 other)
	{
		if (other != null)
		{
			if (other.LiveType != 0)
			{
				LiveType = other.LiveType;
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
				LiveType = input.ReadInt32();
				break;
			case 16u:
				Location = input.ReadInt32();
				break;
			}
		}
	}
}
