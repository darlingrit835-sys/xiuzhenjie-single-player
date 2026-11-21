using System;
using System.Diagnostics;
using Google.Protobuf;
using Google.Protobuf.Reflection;

public sealed class RefuseFriendReq_1014009 : IMessage<RefuseFriendReq_1014009>, IMessage, IEquatable<RefuseFriendReq_1014009>, IDeepCloneable<RefuseFriendReq_1014009>
{
	private static readonly MessageParser<RefuseFriendReq_1014009> _parser = new MessageParser<RefuseFriendReq_1014009>(() => new RefuseFriendReq_1014009());

	private UnknownFieldSet _unknownFields;

	public const int TargetFriendIdFieldNumber = 1;

	private string targetFriendId_ = "";

	[DebuggerNonUserCode]
	public static MessageParser<RefuseFriendReq_1014009> Parser => _parser;

	[DebuggerNonUserCode]
	public static MessageDescriptor Descriptor => FriendProtoReflection.Descriptor.MessageTypes[10];

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
	public RefuseFriendReq_1014009()
	{
	}

	[DebuggerNonUserCode]
	public RefuseFriendReq_1014009(RefuseFriendReq_1014009 other)
		: this()
	{
		targetFriendId_ = other.targetFriendId_;
		_unknownFields = UnknownFieldSet.Clone(other._unknownFields);
	}

	[DebuggerNonUserCode]
	public RefuseFriendReq_1014009 Clone()
	{
		return new RefuseFriendReq_1014009(this);
	}

	[DebuggerNonUserCode]
	public override bool Equals(object other)
	{
		return Equals(other as RefuseFriendReq_1014009);
	}

	[DebuggerNonUserCode]
	public bool Equals(RefuseFriendReq_1014009 other)
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
	public void MergeFrom(RefuseFriendReq_1014009 other)
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
