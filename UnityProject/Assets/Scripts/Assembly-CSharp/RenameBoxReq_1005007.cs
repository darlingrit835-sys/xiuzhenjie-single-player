using System;
using System.Diagnostics;
using Google.Protobuf;
using Google.Protobuf.Reflection;

public sealed class RenameBoxReq_1005007 : IMessage<RenameBoxReq_1005007>, IMessage, IEquatable<RenameBoxReq_1005007>, IDeepCloneable<RenameBoxReq_1005007>
{
	private static readonly MessageParser<RenameBoxReq_1005007> _parser = new MessageParser<RenameBoxReq_1005007>(() => new RenameBoxReq_1005007());

	private UnknownFieldSet _unknownFields;

	public const int NewNameFieldNumber = 1;

	private string newName_ = "";

	public const int LocationFieldNumber = 2;

	private int location_;

	[DebuggerNonUserCode]
	public static MessageParser<RenameBoxReq_1005007> Parser => _parser;

	[DebuggerNonUserCode]
	public static MessageDescriptor Descriptor => HomeProtoReflection.Descriptor.MessageTypes[9];

	[DebuggerNonUserCode]
	MessageDescriptor IMessage.Descriptor => pb_003A_003AGoogle_002EProtobuf_002EIMessage_002Eget_Descriptor();

	[DebuggerNonUserCode]
	public string NewName
	{
		get
		{
			return newName_;
		}
		set
		{
			newName_ = ProtoPreconditions.CheckNotNull(value, "value");
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
	public RenameBoxReq_1005007()
	{
	}

	[DebuggerNonUserCode]
	public RenameBoxReq_1005007(RenameBoxReq_1005007 other)
		: this()
	{
		newName_ = other.newName_;
		location_ = other.location_;
		_unknownFields = UnknownFieldSet.Clone(other._unknownFields);
	}

	[DebuggerNonUserCode]
	public RenameBoxReq_1005007 Clone()
	{
		return new RenameBoxReq_1005007(this);
	}

	[DebuggerNonUserCode]
	public override bool Equals(object other)
	{
		return Equals(other as RenameBoxReq_1005007);
	}

	[DebuggerNonUserCode]
	public bool Equals(RenameBoxReq_1005007 other)
	{
		if (other == null)
		{
			return false;
		}
		if (other == this)
		{
			return true;
		}
		if (NewName != other.NewName)
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
		if (NewName.Length != 0)
		{
			num ^= NewName.GetHashCode();
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
		if (NewName.Length != 0)
		{
			output.WriteRawTag(10);
			output.WriteString(NewName);
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
		if (NewName.Length != 0)
		{
			num += 1 + CodedOutputStream.ComputeStringSize(NewName);
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
	public void MergeFrom(RenameBoxReq_1005007 other)
	{
		if (other != null)
		{
			if (other.NewName.Length != 0)
			{
				NewName = other.NewName;
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
			case 10u:
				NewName = input.ReadString();
				break;
			case 16u:
				Location = input.ReadInt32();
				break;
			}
		}
	}
}
