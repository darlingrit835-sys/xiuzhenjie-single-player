using System;
using System.Diagnostics;
using Google.Protobuf;
using Google.Protobuf.Reflection;

public sealed class GuildJieSanRoleInformResp_1015159 : IMessage<GuildJieSanRoleInformResp_1015159>, IMessage, IEquatable<GuildJieSanRoleInformResp_1015159>, IDeepCloneable<GuildJieSanRoleInformResp_1015159>
{
	private static readonly MessageParser<GuildJieSanRoleInformResp_1015159> _parser = new MessageParser<GuildJieSanRoleInformResp_1015159>(() => new GuildJieSanRoleInformResp_1015159());

	private UnknownFieldSet _unknownFields;

	[DebuggerNonUserCode]
	public static MessageParser<GuildJieSanRoleInformResp_1015159> Parser => _parser;

	[DebuggerNonUserCode]
	public static MessageDescriptor Descriptor => GuildProtoReflection.Descriptor.MessageTypes[201];

	[DebuggerNonUserCode]
	MessageDescriptor IMessage.Descriptor => pb_003A_003AGoogle_002EProtobuf_002EIMessage_002Eget_Descriptor();

	private MessageDescriptor pb_003A_003AGoogle_002EProtobuf_002EIMessage_002Eget_Descriptor()
	{
		return Descriptor;
	}

	[DebuggerNonUserCode]
	public GuildJieSanRoleInformResp_1015159()
	{
	}

	[DebuggerNonUserCode]
	public GuildJieSanRoleInformResp_1015159(GuildJieSanRoleInformResp_1015159 other)
		: this()
	{
		_unknownFields = UnknownFieldSet.Clone(other._unknownFields);
	}

	[DebuggerNonUserCode]
	public GuildJieSanRoleInformResp_1015159 Clone()
	{
		return new GuildJieSanRoleInformResp_1015159(this);
	}

	[DebuggerNonUserCode]
	public override bool Equals(object other)
	{
		return Equals(other as GuildJieSanRoleInformResp_1015159);
	}

	[DebuggerNonUserCode]
	public bool Equals(GuildJieSanRoleInformResp_1015159 other)
	{
		if (other == null)
		{
			return false;
		}
		if (other == this)
		{
			return true;
		}
		return object.Equals(_unknownFields, other._unknownFields);
	}

	[DebuggerNonUserCode]
	public override int GetHashCode()
	{
		int num = 1;
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
		if (_unknownFields != null)
		{
			_unknownFields.WriteTo(output);
		}
	}

	[DebuggerNonUserCode]
	public int CalculateSize()
	{
		int num = 0;
		if (_unknownFields != null)
		{
			num += _unknownFields.CalculateSize();
		}
		return num;
	}

	[DebuggerNonUserCode]
	public void MergeFrom(GuildJieSanRoleInformResp_1015159 other)
	{
		if (other != null)
		{
			_unknownFields = UnknownFieldSet.MergeFrom(_unknownFields, other._unknownFields);
		}
	}

	[DebuggerNonUserCode]
	public void MergeFrom(CodedInputStream input)
	{
		while (input.ReadTag() != 0)
		{
			_unknownFields = UnknownFieldSet.MergeFieldFrom(_unknownFields, input);
		}
	}
}
