using System;
using System.Diagnostics;
using Google.Protobuf;
using Google.Protobuf.Reflection;

public sealed class RoleBreakRealmReq_1001010 : IMessage<RoleBreakRealmReq_1001010>, IMessage, IEquatable<RoleBreakRealmReq_1001010>, IDeepCloneable<RoleBreakRealmReq_1001010>
{
	private static readonly MessageParser<RoleBreakRealmReq_1001010> _parser = new MessageParser<RoleBreakRealmReq_1001010>(() => new RoleBreakRealmReq_1001010());

	private UnknownFieldSet _unknownFields;

	public const int IsThunderSuccessFieldNumber = 1;

	private bool isThunderSuccess_;

	public const int FailThunderNumFieldNumber = 2;

	private int failThunderNum_;

	[DebuggerNonUserCode]
	public static MessageParser<RoleBreakRealmReq_1001010> Parser => _parser;

	[DebuggerNonUserCode]
	public static MessageDescriptor Descriptor => SimpleActionProtoReflection.Descriptor.MessageTypes[11];

	[DebuggerNonUserCode]
	MessageDescriptor IMessage.Descriptor => pb_003A_003AGoogle_002EProtobuf_002EIMessage_002Eget_Descriptor();

	[DebuggerNonUserCode]
	public bool IsThunderSuccess
	{
		get
		{
			return isThunderSuccess_;
		}
		set
		{
			isThunderSuccess_ = value;
		}
	}

	[DebuggerNonUserCode]
	public int FailThunderNum
	{
		get
		{
			return failThunderNum_;
		}
		set
		{
			failThunderNum_ = value;
		}
	}

	private MessageDescriptor pb_003A_003AGoogle_002EProtobuf_002EIMessage_002Eget_Descriptor()
	{
		return Descriptor;
	}

	[DebuggerNonUserCode]
	public RoleBreakRealmReq_1001010()
	{
	}

	[DebuggerNonUserCode]
	public RoleBreakRealmReq_1001010(RoleBreakRealmReq_1001010 other)
		: this()
	{
		isThunderSuccess_ = other.isThunderSuccess_;
		failThunderNum_ = other.failThunderNum_;
		_unknownFields = UnknownFieldSet.Clone(other._unknownFields);
	}

	[DebuggerNonUserCode]
	public RoleBreakRealmReq_1001010 Clone()
	{
		return new RoleBreakRealmReq_1001010(this);
	}

	[DebuggerNonUserCode]
	public override bool Equals(object other)
	{
		return Equals(other as RoleBreakRealmReq_1001010);
	}

	[DebuggerNonUserCode]
	public bool Equals(RoleBreakRealmReq_1001010 other)
	{
		if (other == null)
		{
			return false;
		}
		if (other == this)
		{
			return true;
		}
		if (IsThunderSuccess != other.IsThunderSuccess)
		{
			return false;
		}
		if (FailThunderNum != other.FailThunderNum)
		{
			return false;
		}
		return object.Equals(_unknownFields, other._unknownFields);
	}

	[DebuggerNonUserCode]
	public override int GetHashCode()
	{
		int num = 1;
		if (IsThunderSuccess)
		{
			num ^= IsThunderSuccess.GetHashCode();
		}
		if (FailThunderNum != 0)
		{
			num ^= FailThunderNum.GetHashCode();
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
		if (IsThunderSuccess)
		{
			output.WriteRawTag(8);
			output.WriteBool(IsThunderSuccess);
		}
		if (FailThunderNum != 0)
		{
			output.WriteRawTag(16);
			output.WriteInt32(FailThunderNum);
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
		if (IsThunderSuccess)
		{
			num += 2;
		}
		if (FailThunderNum != 0)
		{
			num += 1 + CodedOutputStream.ComputeInt32Size(FailThunderNum);
		}
		if (_unknownFields != null)
		{
			num += _unknownFields.CalculateSize();
		}
		return num;
	}

	[DebuggerNonUserCode]
	public void MergeFrom(RoleBreakRealmReq_1001010 other)
	{
		if (other != null)
		{
			if (other.IsThunderSuccess)
			{
				IsThunderSuccess = other.IsThunderSuccess;
			}
			if (other.FailThunderNum != 0)
			{
				FailThunderNum = other.FailThunderNum;
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
				IsThunderSuccess = input.ReadBool();
				break;
			case 16u:
				FailThunderNum = input.ReadInt32();
				break;
			}
		}
	}
}
