using System;
using System.Diagnostics;
using Google.Protobuf;
using Google.Protobuf.Reflection;

public sealed class AddFriendReq_1014006 : IMessage<AddFriendReq_1014006>, IMessage, IEquatable<AddFriendReq_1014006>, IDeepCloneable<AddFriendReq_1014006>
{
	private static readonly MessageParser<AddFriendReq_1014006> _parser = new MessageParser<AddFriendReq_1014006>(() => new AddFriendReq_1014006());

	private UnknownFieldSet _unknownFields;

	public const int TargetFriendIdFieldNumber = 1;

	private string targetFriendId_ = "";

	[DebuggerNonUserCode]
	public static MessageParser<AddFriendReq_1014006> Parser => _parser;

	[DebuggerNonUserCode]
	public static MessageDescriptor Descriptor => FriendProtoReflection.Descriptor.MessageTypes[4];

	[DebuggerNonUserCode]
	MessageDescriptor IMessage.Descriptor => pb_003A_003AGoogle_002EProtobuf_002EIMessage_002Eget_Descriptor();

	[DebuggerNonUserCode]
	public string TargetFriendId
	{
		get
		{
			return targetFriendId_;
		}
		set
		{
			targetFriendId_ = ProtoPreconditions.CheckNotNull(value, "value");
		}
	}

	private MessageDescriptor pb_003A_003AGoogle_002EProtobuf_002EIMessage_002Eget_Descriptor()
	{
		return Descriptor;
	}

	[DebuggerNonUserCode]
	public AddFriendReq_1014006()
	{
	}

	[DebuggerNonUserCode]
	public AddFriendReq_1014006(AddFriendReq_1014006 other)
		: this()
	{
		targetFriendId_ = other.targetFriendId_;
		_unknownFields = UnknownFieldSet.Clone(other._unknownFields);
	}

	[DebuggerNonUserCode]
	public AddFriendReq_1014006 Clone()
	{
		return new AddFriendReq_1014006(this);
	}

	[DebuggerNonUserCode]
	public override bool Equals(object other)
	{
		return Equals(other as AddFriendReq_1014006);
	}

	[DebuggerNonUserCode]
	public bool Equals(AddFriendReq_1014006 other)
	{
		if (other == null)
		{
			return false;
		}
		if (other == this)
		{
			return true;
		}
		if (TargetFriendId != other.TargetFriendId)
		{
			return false;
		}
		return object.Equals(_unknownFields, other._unknownFields);
	}

	[DebuggerNonUserCode]
	public override int GetHashCode()
	{
		int num = 1;
		if (TargetFriendId.Length != 0)
		{
			num ^= TargetFriendId.GetHashCode();
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
		if (TargetFriendId.Length != 0)
		{
			output.WriteRawTag(10);
			output.WriteString(TargetFriendId);
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
		if (TargetFriendId.Length != 0)
		{
			num += 1 + CodedOutputStream.ComputeStringSize(TargetFriendId);
		}
		if (_unknownFields != null)
		{
			num += _unknownFields.CalculateSize();
		}
		return num;
	}

	[DebuggerNonUserCode]
	public void MergeFrom(AddFriendReq_1014006 other)
	{
		if (other != null)
		{
			if (other.TargetFriendId.Length != 0)
			{
				TargetFriendId = other.TargetFriendId;
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
				TargetFriendId = input.ReadString();
			}
		}
	}
}
