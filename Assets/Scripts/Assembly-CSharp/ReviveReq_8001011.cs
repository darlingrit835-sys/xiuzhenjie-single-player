using System;
using System.Diagnostics;
using Google.Protobuf;
using Google.Protobuf.Reflection;

public sealed class ReviveReq_8001011 : IMessage<ReviveReq_8001011>, IMessage, IEquatable<ReviveReq_8001011>, IDeepCloneable<ReviveReq_8001011>
{
	private static readonly MessageParser<ReviveReq_8001011> _parser = new MessageParser<ReviveReq_8001011>(() => new ReviveReq_8001011());

	private UnknownFieldSet _unknownFields;

	public const int RoleIndexFieldNumber = 1;

	private int roleIndex_;

	[DebuggerNonUserCode]
	public static MessageParser<ReviveReq_8001011> Parser => _parser;

	[DebuggerNonUserCode]
	public static MessageDescriptor Descriptor => ClientBattleProtoReflection.Descriptor.MessageTypes[14];

	[DebuggerNonUserCode]
	MessageDescriptor IMessage.Descriptor => pb_003A_003AGoogle_002EProtobuf_002EIMessage_002Eget_Descriptor();

	[DebuggerNonUserCode]
	public int RoleIndex
	{
		get
		{
			return roleIndex_;
		}
		set
		{
			roleIndex_ = value;
		}
	}

	private MessageDescriptor pb_003A_003AGoogle_002EProtobuf_002EIMessage_002Eget_Descriptor()
	{
		return Descriptor;
	}

	[DebuggerNonUserCode]
	public ReviveReq_8001011()
	{
	}

	[DebuggerNonUserCode]
	public ReviveReq_8001011(ReviveReq_8001011 other)
		: this()
	{
		roleIndex_ = other.roleIndex_;
		_unknownFields = UnknownFieldSet.Clone(other._unknownFields);
	}

	[DebuggerNonUserCode]
	public ReviveReq_8001011 Clone()
	{
		return new ReviveReq_8001011(this);
	}

	[DebuggerNonUserCode]
	public override bool Equals(object other)
	{
		return Equals(other as ReviveReq_8001011);
	}

	[DebuggerNonUserCode]
	public bool Equals(ReviveReq_8001011 other)
	{
		if (other == null)
		{
			return false;
		}
		if (other == this)
		{
			return true;
		}
		if (RoleIndex != other.RoleIndex)
		{
			return false;
		}
		return object.Equals(_unknownFields, other._unknownFields);
	}

	[DebuggerNonUserCode]
	public override int GetHashCode()
	{
		int num = 1;
		if (RoleIndex != 0)
		{
			num ^= RoleIndex.GetHashCode();
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
		if (RoleIndex != 0)
		{
			output.WriteRawTag(8);
			output.WriteInt32(RoleIndex);
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
		if (RoleIndex != 0)
		{
			num += 1 + CodedOutputStream.ComputeInt32Size(RoleIndex);
		}
		if (_unknownFields != null)
		{
			num += _unknownFields.CalculateSize();
		}
		return num;
	}

	[DebuggerNonUserCode]
	public void MergeFrom(ReviveReq_8001011 other)
	{
		if (other != null)
		{
			if (other.RoleIndex != 0)
			{
				RoleIndex = other.RoleIndex;
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
				RoleIndex = input.ReadInt32();
			}
		}
	}
}
