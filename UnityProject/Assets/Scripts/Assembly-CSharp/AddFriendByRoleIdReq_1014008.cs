using System;
using System.Diagnostics;
using Google.Protobuf;
using Google.Protobuf.Reflection;

public sealed class AddFriendByRoleIdReq_1014008 : IMessage<AddFriendByRoleIdReq_1014008>, IMessage, IEquatable<AddFriendByRoleIdReq_1014008>, IDeepCloneable<AddFriendByRoleIdReq_1014008>
{
	private static readonly MessageParser<AddFriendByRoleIdReq_1014008> _parser = new MessageParser<AddFriendByRoleIdReq_1014008>(() => new AddFriendByRoleIdReq_1014008());

	private UnknownFieldSet _unknownFields;

	public const int TargetFriendIdFieldNumber = 1;

	private string targetFriendId_ = "";

	[DebuggerNonUserCode]
	public static MessageParser<AddFriendByRoleIdReq_1014008> Parser => _parser;

	[DebuggerNonUserCode]
	public static MessageDescriptor Descriptor => FriendProtoReflection.Descriptor.MessageTypes[8];

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
	public AddFriendByRoleIdReq_1014008()
	{
	}

	[DebuggerNonUserCode]
	public AddFriendByRoleIdReq_1014008(AddFriendByRoleIdReq_1014008 other)
		: this()
	{
		targetFriendId_ = other.targetFriendId_;
		_unknownFields = UnknownFieldSet.Clone(other._unknownFields);
	}

	[DebuggerNonUserCode]
	public AddFriendByRoleIdReq_1014008 Clone()
	{
		return new AddFriendByRoleIdReq_1014008(this);
	}

	[DebuggerNonUserCode]
	public override bool Equals(object other)
	{
		return Equals(other as AddFriendByRoleIdReq_1014008);
	}

	[DebuggerNonUserCode]
	public bool Equals(AddFriendByRoleIdReq_1014008 other)
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
	public void MergeFrom(AddFriendByRoleIdReq_1014008 other)
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
