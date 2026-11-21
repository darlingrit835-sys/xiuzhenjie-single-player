using System;
using System.Diagnostics;
using Google.Protobuf;
using Google.Protobuf.Reflection;

public sealed class GuildCreateReq_1015007 : IMessage<GuildCreateReq_1015007>, IMessage, IEquatable<GuildCreateReq_1015007>, IDeepCloneable<GuildCreateReq_1015007>
{
	private static readonly MessageParser<GuildCreateReq_1015007> _parser = new MessageParser<GuildCreateReq_1015007>(() => new GuildCreateReq_1015007());

	private UnknownFieldSet _unknownFields;

	public const int NameFieldNumber = 1;

	private string name_ = "";

	public const int SimpleNameFieldNumber = 2;

	private string simpleName_ = "";

	public const int LastNameIndexFieldNumber = 3;

	private int lastNameIndex_;

	public const int TokenIndexFieldNumber = 4;

	private int tokenIndex_;

	[DebuggerNonUserCode]
	public static MessageParser<GuildCreateReq_1015007> Parser => _parser;

	[DebuggerNonUserCode]
	public static MessageDescriptor Descriptor => GuildProtoReflection.Descriptor.MessageTypes[7];

	[DebuggerNonUserCode]
	MessageDescriptor IMessage.Descriptor => pb_003A_003AGoogle_002EProtobuf_002EIMessage_002Eget_Descriptor();

	[DebuggerNonUserCode]
	public string Name
	{
		get
		{
			return name_;
		}
		set
		{
			name_ = ProtoPreconditions.CheckNotNull(value, "value");
		}
	}

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

	[DebuggerNonUserCode]
	public int LastNameIndex
	{
		get
		{
			return lastNameIndex_;
		}
		set
		{
			lastNameIndex_ = value;
		}
	}

	[DebuggerNonUserCode]
	public int TokenIndex
	{
		get
		{
			return tokenIndex_;
		}
		set
		{
			tokenIndex_ = value;
		}
	}

	private MessageDescriptor pb_003A_003AGoogle_002EProtobuf_002EIMessage_002Eget_Descriptor()
	{
		return Descriptor;
	}

	[DebuggerNonUserCode]
	public GuildCreateReq_1015007()
	{
	}

	[DebuggerNonUserCode]
	public GuildCreateReq_1015007(GuildCreateReq_1015007 other)
		: this()
	{
		name_ = other.name_;
		simpleName_ = other.simpleName_;
		lastNameIndex_ = other.lastNameIndex_;
		tokenIndex_ = other.tokenIndex_;
		_unknownFields = UnknownFieldSet.Clone(other._unknownFields);
	}

	[DebuggerNonUserCode]
	public GuildCreateReq_1015007 Clone()
	{
		return new GuildCreateReq_1015007(this);
	}

	[DebuggerNonUserCode]
	public override bool Equals(object other)
	{
		return Equals(other as GuildCreateReq_1015007);
	}

	[DebuggerNonUserCode]
	public bool Equals(GuildCreateReq_1015007 other)
	{
		if (other == null)
		{
			return false;
		}
		if (other == this)
		{
			return true;
		}
		if (Name != other.Name)
		{
			return false;
		}
		if (SimpleName != other.SimpleName)
		{
			return false;
		}
		if (LastNameIndex != other.LastNameIndex)
		{
			return false;
		}
		if (TokenIndex != other.TokenIndex)
		{
			return false;
		}
		return object.Equals(_unknownFields, other._unknownFields);
	}

	[DebuggerNonUserCode]
	public override int GetHashCode()
	{
		int num = 1;
		if (Name.Length != 0)
		{
			num ^= Name.GetHashCode();
		}
		if (SimpleName.Length != 0)
		{
			num ^= SimpleName.GetHashCode();
		}
		if (LastNameIndex != 0)
		{
			num ^= LastNameIndex.GetHashCode();
		}
		if (TokenIndex != 0)
		{
			num ^= TokenIndex.GetHashCode();
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
		if (Name.Length != 0)
		{
			output.WriteRawTag(10);
			output.WriteString(Name);
		}
		if (SimpleName.Length != 0)
		{
			output.WriteRawTag(18);
			output.WriteString(SimpleName);
		}
		if (LastNameIndex != 0)
		{
			output.WriteRawTag(24);
			output.WriteInt32(LastNameIndex);
		}
		if (TokenIndex != 0)
		{
			output.WriteRawTag(32);
			output.WriteInt32(TokenIndex);
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
		if (Name.Length != 0)
		{
			num += 1 + CodedOutputStream.ComputeStringSize(Name);
		}
		if (SimpleName.Length != 0)
		{
			num += 1 + CodedOutputStream.ComputeStringSize(SimpleName);
		}
		if (LastNameIndex != 0)
		{
			num += 1 + CodedOutputStream.ComputeInt32Size(LastNameIndex);
		}
		if (TokenIndex != 0)
		{
			num += 1 + CodedOutputStream.ComputeInt32Size(TokenIndex);
		}
		if (_unknownFields != null)
		{
			num += _unknownFields.CalculateSize();
		}
		return num;
	}

	[DebuggerNonUserCode]
	public void MergeFrom(GuildCreateReq_1015007 other)
	{
		if (other != null)
		{
			if (other.Name.Length != 0)
			{
				Name = other.Name;
			}
			if (other.SimpleName.Length != 0)
			{
				SimpleName = other.SimpleName;
			}
			if (other.LastNameIndex != 0)
			{
				LastNameIndex = other.LastNameIndex;
			}
			if (other.TokenIndex != 0)
			{
				TokenIndex = other.TokenIndex;
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
				Name = input.ReadString();
				break;
			case 18u:
				SimpleName = input.ReadString();
				break;
			case 24u:
				LastNameIndex = input.ReadInt32();
				break;
			case 32u:
				TokenIndex = input.ReadInt32();
				break;
			}
		}
	}
}
