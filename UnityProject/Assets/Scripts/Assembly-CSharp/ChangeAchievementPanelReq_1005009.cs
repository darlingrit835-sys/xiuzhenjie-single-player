using System;
using System.Diagnostics;
using Google.Protobuf;
using Google.Protobuf.Reflection;

public sealed class ChangeAchievementPanelReq_1005009 : IMessage<ChangeAchievementPanelReq_1005009>, IMessage, IEquatable<ChangeAchievementPanelReq_1005009>, IDeepCloneable<ChangeAchievementPanelReq_1005009>
{
	private static readonly MessageParser<ChangeAchievementPanelReq_1005009> _parser = new MessageParser<ChangeAchievementPanelReq_1005009>(() => new ChangeAchievementPanelReq_1005009());

	private UnknownFieldSet _unknownFields;

	public const int AchievementTypeFieldNumber = 1;

	private int achievementType_;

	[DebuggerNonUserCode]
	public static MessageParser<ChangeAchievementPanelReq_1005009> Parser => _parser;

	[DebuggerNonUserCode]
	public static MessageDescriptor Descriptor => HomeProtoReflection.Descriptor.MessageTypes[12];

	[DebuggerNonUserCode]
	MessageDescriptor IMessage.Descriptor => pb_003A_003AGoogle_002EProtobuf_002EIMessage_002Eget_Descriptor();

	[DebuggerNonUserCode]
	public int AchievementType
	{
		get
		{
			return achievementType_;
		}
		set
		{
			achievementType_ = value;
		}
	}

	private MessageDescriptor pb_003A_003AGoogle_002EProtobuf_002EIMessage_002Eget_Descriptor()
	{
		return Descriptor;
	}

	[DebuggerNonUserCode]
	public ChangeAchievementPanelReq_1005009()
	{
	}

	[DebuggerNonUserCode]
	public ChangeAchievementPanelReq_1005009(ChangeAchievementPanelReq_1005009 other)
		: this()
	{
		achievementType_ = other.achievementType_;
		_unknownFields = UnknownFieldSet.Clone(other._unknownFields);
	}

	[DebuggerNonUserCode]
	public ChangeAchievementPanelReq_1005009 Clone()
	{
		return new ChangeAchievementPanelReq_1005009(this);
	}

	[DebuggerNonUserCode]
	public override bool Equals(object other)
	{
		return Equals(other as ChangeAchievementPanelReq_1005009);
	}

	[DebuggerNonUserCode]
	public bool Equals(ChangeAchievementPanelReq_1005009 other)
	{
		if (other == null)
		{
			return false;
		}
		if (other == this)
		{
			return true;
		}
		if (AchievementType != other.AchievementType)
		{
			return false;
		}
		return object.Equals(_unknownFields, other._unknownFields);
	}

	[DebuggerNonUserCode]
	public override int GetHashCode()
	{
		int num = 1;
		if (AchievementType != 0)
		{
			num ^= AchievementType.GetHashCode();
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
		if (AchievementType != 0)
		{
			output.WriteRawTag(8);
			output.WriteInt32(AchievementType);
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
		if (AchievementType != 0)
		{
			num += 1 + CodedOutputStream.ComputeInt32Size(AchievementType);
		}
		if (_unknownFields != null)
		{
			num += _unknownFields.CalculateSize();
		}
		return num;
	}

	[DebuggerNonUserCode]
	public void MergeFrom(ChangeAchievementPanelReq_1005009 other)
	{
		if (other != null)
		{
			if (other.AchievementType != 0)
			{
				AchievementType = other.AchievementType;
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
				AchievementType = input.ReadInt32();
			}
		}
	}
}
