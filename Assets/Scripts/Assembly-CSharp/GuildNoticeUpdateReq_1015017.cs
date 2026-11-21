using System;
using System.Diagnostics;
using Google.Protobuf;
using Google.Protobuf.Reflection;

public sealed class GuildNoticeUpdateReq_1015017 : IMessage<GuildNoticeUpdateReq_1015017>, IMessage, IEquatable<GuildNoticeUpdateReq_1015017>, IDeepCloneable<GuildNoticeUpdateReq_1015017>
{
	private static readonly MessageParser<GuildNoticeUpdateReq_1015017> _parser = new MessageParser<GuildNoticeUpdateReq_1015017>(() => new GuildNoticeUpdateReq_1015017());

	private UnknownFieldSet _unknownFields;

	public const int RecruitNoticeFieldNumber = 1;

	private string recruitNotice_ = "";

	public const int GuildNoticeFieldNumber = 2;

	private string guildNotice_ = "";

	[DebuggerNonUserCode]
	public static MessageParser<GuildNoticeUpdateReq_1015017> Parser => _parser;

	[DebuggerNonUserCode]
	public static MessageDescriptor Descriptor => GuildProtoReflection.Descriptor.MessageTypes[19];

	[DebuggerNonUserCode]
	MessageDescriptor IMessage.Descriptor => pb_003A_003AGoogle_002EProtobuf_002EIMessage_002Eget_Descriptor();

	[DebuggerNonUserCode]
	public string RecruitNotice
	{
		get
		{
			return recruitNotice_;
		}
		set
		{
			recruitNotice_ = ProtoPreconditions.CheckNotNull(value, "value");
		}
	}

	[DebuggerNonUserCode]
	public string GuildNotice
	{
		get
		{
			return guildNotice_;
		}
		set
		{
			guildNotice_ = ProtoPreconditions.CheckNotNull(value, "value");
		}
	}

	private MessageDescriptor pb_003A_003AGoogle_002EProtobuf_002EIMessage_002Eget_Descriptor()
	{
		return Descriptor;
	}

	[DebuggerNonUserCode]
	public GuildNoticeUpdateReq_1015017()
	{
	}

	[DebuggerNonUserCode]
	public GuildNoticeUpdateReq_1015017(GuildNoticeUpdateReq_1015017 other)
		: this()
	{
		recruitNotice_ = other.recruitNotice_;
		guildNotice_ = other.guildNotice_;
		_unknownFields = UnknownFieldSet.Clone(other._unknownFields);
	}

	[DebuggerNonUserCode]
	public GuildNoticeUpdateReq_1015017 Clone()
	{
		return new GuildNoticeUpdateReq_1015017(this);
	}

	[DebuggerNonUserCode]
	public override bool Equals(object other)
	{
		return Equals(other as GuildNoticeUpdateReq_1015017);
	}

	[DebuggerNonUserCode]
	public bool Equals(GuildNoticeUpdateReq_1015017 other)
	{
		if (other == null)
		{
			return false;
		}
		if (other == this)
		{
			return true;
		}
		if (RecruitNotice != other.RecruitNotice)
		{
			return false;
		}
		if (GuildNotice != other.GuildNotice)
		{
			return false;
		}
		return object.Equals(_unknownFields, other._unknownFields);
	}

	[DebuggerNonUserCode]
	public override int GetHashCode()
	{
		int num = 1;
		if (RecruitNotice.Length != 0)
		{
			num ^= RecruitNotice.GetHashCode();
		}
		if (GuildNotice.Length != 0)
		{
			num ^= GuildNotice.GetHashCode();
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
		if (RecruitNotice.Length != 0)
		{
			output.WriteRawTag(10);
			output.WriteString(RecruitNotice);
		}
		if (GuildNotice.Length != 0)
		{
			output.WriteRawTag(18);
			output.WriteString(GuildNotice);
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
		if (RecruitNotice.Length != 0)
		{
			num += 1 + CodedOutputStream.ComputeStringSize(RecruitNotice);
		}
		if (GuildNotice.Length != 0)
		{
			num += 1 + CodedOutputStream.ComputeStringSize(GuildNotice);
		}
		if (_unknownFields != null)
		{
			num += _unknownFields.CalculateSize();
		}
		return num;
	}

	[DebuggerNonUserCode]
	public void MergeFrom(GuildNoticeUpdateReq_1015017 other)
	{
		if (other != null)
		{
			if (other.RecruitNotice.Length != 0)
			{
				RecruitNotice = other.RecruitNotice;
			}
			if (other.GuildNotice.Length != 0)
			{
				GuildNotice = other.GuildNotice;
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
				RecruitNotice = input.ReadString();
				break;
			case 18u:
				GuildNotice = input.ReadString();
				break;
			}
		}
	}
}
