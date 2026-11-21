using System;
using System.Diagnostics;
using Google.Protobuf;
using Google.Protobuf.Reflection;

public sealed class ChangeAchievementReq_1005011 : IMessage<ChangeAchievementReq_1005011>, IMessage, IEquatable<ChangeAchievementReq_1005011>, IDeepCloneable<ChangeAchievementReq_1005011>
{
	private static readonly MessageParser<ChangeAchievementReq_1005011> _parser = new MessageParser<ChangeAchievementReq_1005011>(() => new ChangeAchievementReq_1005011());

	private UnknownFieldSet _unknownFields;

	public const int AchievementIdFieldNumber = 1;

	private string achievementId_ = "";

	public const int ScopeTypeFieldNumber = 2;

	private int scopeType_;

	[DebuggerNonUserCode]
	public static MessageParser<ChangeAchievementReq_1005011> Parser => _parser;

	[DebuggerNonUserCode]
	public static MessageDescriptor Descriptor => HomeProtoReflection.Descriptor.MessageTypes[14];

	[DebuggerNonUserCode]
	MessageDescriptor IMessage.Descriptor => pb_003A_003AGoogle_002EProtobuf_002EIMessage_002Eget_Descriptor();

	[DebuggerNonUserCode]
	public string AchievementId
	{
		get
		{
			return achievementId_;
		}
		set
		{
			achievementId_ = ProtoPreconditions.CheckNotNull(value, "value");
		}
	}

	[DebuggerNonUserCode]
	public int ScopeType
	{
		get
		{
			return scopeType_;
		}
		set
		{
			scopeType_ = value;
		}
	}

	private MessageDescriptor pb_003A_003AGoogle_002EProtobuf_002EIMessage_002Eget_Descriptor()
	{
		return Descriptor;
	}

	[DebuggerNonUserCode]
	public ChangeAchievementReq_1005011()
	{
	}

	[DebuggerNonUserCode]
	public ChangeAchievementReq_1005011(ChangeAchievementReq_1005011 other)
		: this()
	{
		achievementId_ = other.achievementId_;
		scopeType_ = other.scopeType_;
		_unknownFields = UnknownFieldSet.Clone(other._unknownFields);
	}

	[DebuggerNonUserCode]
	public ChangeAchievementReq_1005011 Clone()
	{
		return new ChangeAchievementReq_1005011(this);
	}

	[DebuggerNonUserCode]
	public override bool Equals(object other)
	{
		return Equals(other as ChangeAchievementReq_1005011);
	}

	[DebuggerNonUserCode]
	public bool Equals(ChangeAchievementReq_1005011 other)
	{
		if (other == null)
		{
			return false;
		}
		if (other == this)
		{
			return true;
		}
		if (AchievementId != other.AchievementId)
		{
			return false;
		}
		if (ScopeType != other.ScopeType)
		{
			return false;
		}
		return object.Equals(_unknownFields, other._unknownFields);
	}

	[DebuggerNonUserCode]
	public override int GetHashCode()
	{
		int num = 1;
		if (AchievementId.Length != 0)
		{
			num ^= AchievementId.GetHashCode();
		}
		if (ScopeType != 0)
		{
			num ^= ScopeType.GetHashCode();
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
		if (AchievementId.Length != 0)
		{
			output.WriteRawTag(10);
			output.WriteString(AchievementId);
		}
		if (ScopeType != 0)
		{
			output.WriteRawTag(16);
			output.WriteInt32(ScopeType);
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
		if (AchievementId.Length != 0)
		{
			num += 1 + CodedOutputStream.ComputeStringSize(AchievementId);
		}
		if (ScopeType != 0)
		{
			num += 1 + CodedOutputStream.ComputeInt32Size(ScopeType);
		}
		if (_unknownFields != null)
		{
			num += _unknownFields.CalculateSize();
		}
		return num;
	}

	[DebuggerNonUserCode]
	public void MergeFrom(ChangeAchievementReq_1005011 other)
	{
		if (other != null)
		{
			if (other.AchievementId.Length != 0)
			{
				AchievementId = other.AchievementId;
			}
			if (other.ScopeType != 0)
			{
				ScopeType = other.ScopeType;
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
				AchievementId = input.ReadString();
				break;
			case 16u:
				ScopeType = input.ReadInt32();
				break;
			}
		}
	}
}
