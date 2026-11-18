using System;
using System.Diagnostics;
using Google.Protobuf;
using Google.Protobuf.Reflection;

public sealed class GuildSimpleNameUpdateReq_1015016 : IMessage<GuildSimpleNameUpdateReq_1015016>, IMessage, IEquatable<GuildSimpleNameUpdateReq_1015016>, IDeepCloneable<GuildSimpleNameUpdateReq_1015016>
{
	private static readonly MessageParser<GuildSimpleNameUpdateReq_1015016> _parser = new MessageParser<GuildSimpleNameUpdateReq_1015016>(() => new GuildSimpleNameUpdateReq_1015016());

	private UnknownFieldSet _unknownFields;

	public const int SimpleNameFieldNumber = 1;

	private string simpleName_ = "";

	[DebuggerNonUserCode]
	public static MessageParser<GuildSimpleNameUpdateReq_1015016> Parser => _parser;

	[DebuggerNonUserCode]
	public static MessageDescriptor Descriptor => GuildProtoReflection.Descriptor.MessageTypes[17];

	[DebuggerNonUserCode]
	MessageDescriptor IMessage.Descriptor => pb_003A_003AGoogle_002EProtobuf_002EIMessage_002Eget_Descriptor();

	[DebuggerNonUserCode]
	public string SimpleName
	{
		get
		{
			return simpleName_;
		}
		set
		{
			simpleName_ = ProtoPreconditions.CheckNotNull(value, "value");
		}
	}

	private MessageDescriptor pb_003A_003AGoogle_002EProtobuf_002EIMessage_002Eget_Descriptor()
	{
		return Descriptor;
	}

	[DebuggerNonUserCode]
	public GuildSimpleNameUpdateReq_1015016()
	{
	}

	[DebuggerNonUserCode]
	public GuildSimpleNameUpdateReq_1015016(GuildSimpleNameUpdateReq_1015016 other)
		: this()
	{
		simpleName_ = other.simpleName_;
		_unknownFields = UnknownFieldSet.Clone(other._unknownFields);
	}

	[DebuggerNonUserCode]
	public GuildSimpleNameUpdateReq_1015016 Clone()
	{
		return new GuildSimpleNameUpdateReq_1015016(this);
	}

	[DebuggerNonUserCode]
	public override bool Equals(object other)
	{
		return Equals(other as GuildSimpleNameUpdateReq_1015016);
	}

	[DebuggerNonUserCode]
	public bool Equals(GuildSimpleNameUpdateReq_1015016 other)
	{
		if (other == null)
		{
			return false;
		}
		if (other == this)
		{
			return true;
		}
		if (SimpleName != other.SimpleName)
		{
			return false;
		}
		return object.Equals(_unknownFields, other._unknownFields);
	}

	[DebuggerNonUserCode]
	public override int GetHashCode()
	{
		int num = 1;
		if (SimpleName.Length != 0)
		{
			num ^= SimpleName.GetHashCode();
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
		if (SimpleName.Length != 0)
		{
			output.WriteRawTag(10);
			output.WriteString(SimpleName);
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
		if (SimpleName.Length != 0)
		{
			num += 1 + CodedOutputStream.ComputeStringSize(SimpleName);
		}
		if (_unknownFields != null)
		{
			num += _unknownFields.CalculateSize();
		}
		return num;
	}

	[DebuggerNonUserCode]
	public void MergeFrom(GuildSimpleNameUpdateReq_1015016 other)
	{
		if (other != null)
		{
			if (other.SimpleName.Length != 0)
			{
				SimpleName = other.SimpleName;
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
				SimpleName = input.ReadString();
			}
		}
	}
}
