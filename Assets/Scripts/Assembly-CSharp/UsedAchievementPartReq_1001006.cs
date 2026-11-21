using System;
using System.Diagnostics;
using Google.Protobuf;
using Google.Protobuf.Reflection;

public sealed class UsedAchievementPartReq_1001006 : IMessage<UsedAchievementPartReq_1001006>, IMessage, IEquatable<UsedAchievementPartReq_1001006>, IDeepCloneable<UsedAchievementPartReq_1001006>
{
	private static readonly MessageParser<UsedAchievementPartReq_1001006> _parser = new MessageParser<UsedAchievementPartReq_1001006>(() => new UsedAchievementPartReq_1001006());

	private UnknownFieldSet _unknownFields;

	public const int IdFieldNumber = 1;

	private string id_ = "";

	public const int PartCountFieldNumber = 2;

	private int partCount_;

	public const int DaoJingCountFieldNumber = 3;

	private int daoJingCount_;

	[DebuggerNonUserCode]
	public static MessageParser<UsedAchievementPartReq_1001006> Parser => _parser;

	[DebuggerNonUserCode]
	public static MessageDescriptor Descriptor => SimpleActionProtoReflection.Descriptor.MessageTypes[5];

	[DebuggerNonUserCode]
	MessageDescriptor IMessage.Descriptor => pb_003A_003AGoogle_002EProtobuf_002EIMessage_002Eget_Descriptor();

	[DebuggerNonUserCode]
	public string Id
	{
		get
		{
			return id_;
		}
		set
		{
			id_ = ProtoPreconditions.CheckNotNull(value, "value");
		}
	}

	[DebuggerNonUserCode]
	public int PartCount
	{
		get
		{
			return partCount_;
		}
		set
		{
			partCount_ = value;
		}
	}

	[DebuggerNonUserCode]
	public int DaoJingCount
	{
		get
		{
			return daoJingCount_;
		}
		set
		{
			daoJingCount_ = value;
		}
	}

	private MessageDescriptor pb_003A_003AGoogle_002EProtobuf_002EIMessage_002Eget_Descriptor()
	{
		return Descriptor;
	}

	[DebuggerNonUserCode]
	public UsedAchievementPartReq_1001006()
	{
	}

	[DebuggerNonUserCode]
	public UsedAchievementPartReq_1001006(UsedAchievementPartReq_1001006 other)
		: this()
	{
		id_ = other.id_;
		partCount_ = other.partCount_;
		daoJingCount_ = other.daoJingCount_;
		_unknownFields = UnknownFieldSet.Clone(other._unknownFields);
	}

	[DebuggerNonUserCode]
	public UsedAchievementPartReq_1001006 Clone()
	{
		return new UsedAchievementPartReq_1001006(this);
	}

	[DebuggerNonUserCode]
	public override bool Equals(object other)
	{
		return Equals(other as UsedAchievementPartReq_1001006);
	}

	[DebuggerNonUserCode]
	public bool Equals(UsedAchievementPartReq_1001006 other)
	{
		if (other == null)
		{
			return false;
		}
		if (other == this)
		{
			return true;
		}
		if (Id != other.Id)
		{
			return false;
		}
		if (PartCount != other.PartCount)
		{
			return false;
		}
		if (DaoJingCount != other.DaoJingCount)
		{
			return false;
		}
		return object.Equals(_unknownFields, other._unknownFields);
	}

	[DebuggerNonUserCode]
	public override int GetHashCode()
	{
		int num = 1;
		if (Id.Length != 0)
		{
			num ^= Id.GetHashCode();
		}
		if (PartCount != 0)
		{
			num ^= PartCount.GetHashCode();
		}
		if (DaoJingCount != 0)
		{
			num ^= DaoJingCount.GetHashCode();
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
		if (Id.Length != 0)
		{
			output.WriteRawTag(10);
			output.WriteString(Id);
		}
		if (PartCount != 0)
		{
			output.WriteRawTag(16);
			output.WriteInt32(PartCount);
		}
		if (DaoJingCount != 0)
		{
			output.WriteRawTag(24);
			output.WriteInt32(DaoJingCount);
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
		if (Id.Length != 0)
		{
			num += 1 + CodedOutputStream.ComputeStringSize(Id);
		}
		if (PartCount != 0)
		{
			num += 1 + CodedOutputStream.ComputeInt32Size(PartCount);
		}
		if (DaoJingCount != 0)
		{
			num += 1 + CodedOutputStream.ComputeInt32Size(DaoJingCount);
		}
		if (_unknownFields != null)
		{
			num += _unknownFields.CalculateSize();
		}
		return num;
	}

	[DebuggerNonUserCode]
	public void MergeFrom(UsedAchievementPartReq_1001006 other)
	{
		if (other != null)
		{
			if (other.Id.Length != 0)
			{
				Id = other.Id;
			}
			if (other.PartCount != 0)
			{
				PartCount = other.PartCount;
			}
			if (other.DaoJingCount != 0)
			{
				DaoJingCount = other.DaoJingCount;
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
				Id = input.ReadString();
				break;
			case 16u:
				PartCount = input.ReadInt32();
				break;
			case 24u:
				DaoJingCount = input.ReadInt32();
				break;
			}
		}
	}
}
