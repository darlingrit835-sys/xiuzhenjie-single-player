using System;
using System.Diagnostics;
using Google.Protobuf;
using Google.Protobuf.Reflection;

public sealed class RoleGetOfflineReq_1002004 : IMessage<RoleGetOfflineReq_1002004>, IMessage, IEquatable<RoleGetOfflineReq_1002004>, IDeepCloneable<RoleGetOfflineReq_1002004>
{
	private static readonly MessageParser<RoleGetOfflineReq_1002004> _parser = new MessageParser<RoleGetOfflineReq_1002004>(() => new RoleGetOfflineReq_1002004());

	private UnknownFieldSet _unknownFields;

	public const int IsGetOfflineFieldNumber = 1;

	private bool isGetOffline_;

	[DebuggerNonUserCode]
	public static MessageParser<RoleGetOfflineReq_1002004> Parser => _parser;

	[DebuggerNonUserCode]
	public static MessageDescriptor Descriptor => RoleProtoReflection.Descriptor.MessageTypes[5];

	[DebuggerNonUserCode]
	MessageDescriptor IMessage.Descriptor => pb_003A_003AGoogle_002EProtobuf_002EIMessage_002Eget_Descriptor();

	[DebuggerNonUserCode]
	public bool IsGetOffline
	{
		get
		{
			return isGetOffline_;
		}
		set
		{
			isGetOffline_ = value;
		}
	}

	private MessageDescriptor pb_003A_003AGoogle_002EProtobuf_002EIMessage_002Eget_Descriptor()
	{
		return Descriptor;
	}

	[DebuggerNonUserCode]
	public RoleGetOfflineReq_1002004()
	{
	}

	[DebuggerNonUserCode]
	public RoleGetOfflineReq_1002004(RoleGetOfflineReq_1002004 other)
		: this()
	{
		isGetOffline_ = other.isGetOffline_;
		_unknownFields = UnknownFieldSet.Clone(other._unknownFields);
	}

	[DebuggerNonUserCode]
	public RoleGetOfflineReq_1002004 Clone()
	{
		return new RoleGetOfflineReq_1002004(this);
	}

	[DebuggerNonUserCode]
	public override bool Equals(object other)
	{
		return Equals(other as RoleGetOfflineReq_1002004);
	}

	[DebuggerNonUserCode]
	public bool Equals(RoleGetOfflineReq_1002004 other)
	{
		if (other == null)
		{
			return false;
		}
		if (other == this)
		{
			return true;
		}
		if (IsGetOffline != other.IsGetOffline)
		{
			return false;
		}
		return object.Equals(_unknownFields, other._unknownFields);
	}

	[DebuggerNonUserCode]
	public override int GetHashCode()
	{
		int num = 1;
		if (IsGetOffline)
		{
			num ^= IsGetOffline.GetHashCode();
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
		if (IsGetOffline)
		{
			output.WriteRawTag(8);
			output.WriteBool(IsGetOffline);
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
		if (IsGetOffline)
		{
			num += 2;
		}
		if (_unknownFields != null)
		{
			num += _unknownFields.CalculateSize();
		}
		return num;
	}

	[DebuggerNonUserCode]
	public void MergeFrom(RoleGetOfflineReq_1002004 other)
	{
		if (other != null)
		{
			if (other.IsGetOffline)
			{
				IsGetOffline = other.IsGetOffline;
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
				IsGetOffline = input.ReadBool();
			}
		}
	}
}
