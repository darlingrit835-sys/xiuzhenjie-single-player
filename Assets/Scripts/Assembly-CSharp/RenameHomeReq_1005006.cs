using System;
using System.Diagnostics;
using Google.Protobuf;
using Google.Protobuf.Reflection;

public sealed class RenameHomeReq_1005006 : IMessage<RenameHomeReq_1005006>, IMessage, IEquatable<RenameHomeReq_1005006>, IDeepCloneable<RenameHomeReq_1005006>
{
	private static readonly MessageParser<RenameHomeReq_1005006> _parser = new MessageParser<RenameHomeReq_1005006>(() => new RenameHomeReq_1005006());

	private UnknownFieldSet _unknownFields;

	public const int NewNameFieldNumber = 1;

	private string newName_ = "";

	[DebuggerNonUserCode]
	public static MessageParser<RenameHomeReq_1005006> Parser => _parser;

	[DebuggerNonUserCode]
	public static MessageDescriptor Descriptor => HomeProtoReflection.Descriptor.MessageTypes[7];

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

	private MessageDescriptor pb_003A_003AGoogle_002EProtobuf_002EIMessage_002Eget_Descriptor()
	{
		return Descriptor;
	}

	[DebuggerNonUserCode]
	public RenameHomeReq_1005006()
	{
	}

	[DebuggerNonUserCode]
	public RenameHomeReq_1005006(RenameHomeReq_1005006 other)
		: this()
	{
		newName_ = other.newName_;
		_unknownFields = UnknownFieldSet.Clone(other._unknownFields);
	}

	[DebuggerNonUserCode]
	public RenameHomeReq_1005006 Clone()
	{
		return new RenameHomeReq_1005006(this);
	}

	[DebuggerNonUserCode]
	public override bool Equals(object other)
	{
		return Equals(other as RenameHomeReq_1005006);
	}

	[DebuggerNonUserCode]
	public bool Equals(RenameHomeReq_1005006 other)
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
		if (_unknownFields != null)
		{
			num += _unknownFields.CalculateSize();
		}
		return num;
	}

	[DebuggerNonUserCode]
	public void MergeFrom(RenameHomeReq_1005006 other)
	{
		if (other != null)
		{
			if (other.NewName.Length != 0)
			{
				NewName = other.NewName;
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
				NewName = input.ReadString();
			}
		}
	}
}
