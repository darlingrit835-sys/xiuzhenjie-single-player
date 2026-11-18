using System;
using System.Diagnostics;
using Google.Protobuf;
using Google.Protobuf.Reflection;

public sealed class UpgradeAchievementConversionRateReq_1001020 : IMessage<UpgradeAchievementConversionRateReq_1001020>, IMessage, IEquatable<UpgradeAchievementConversionRateReq_1001020>, IDeepCloneable<UpgradeAchievementConversionRateReq_1001020>
{
	private static readonly MessageParser<UpgradeAchievementConversionRateReq_1001020> _parser = new MessageParser<UpgradeAchievementConversionRateReq_1001020>(() => new UpgradeAchievementConversionRateReq_1001020());

	private UnknownFieldSet _unknownFields;

	public const int AchievementIdFieldNumber = 1;

	private string achievementId_ = "";

	[DebuggerNonUserCode]
	public static MessageParser<UpgradeAchievementConversionRateReq_1001020> Parser => _parser;

	[DebuggerNonUserCode]
	public static MessageDescriptor Descriptor => SimpleActionProtoReflection.Descriptor.MessageTypes[23];

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

	private MessageDescriptor pb_003A_003AGoogle_002EProtobuf_002EIMessage_002Eget_Descriptor()
	{
		return Descriptor;
	}

	[DebuggerNonUserCode]
	public UpgradeAchievementConversionRateReq_1001020()
	{
	}

	[DebuggerNonUserCode]
	public UpgradeAchievementConversionRateReq_1001020(UpgradeAchievementConversionRateReq_1001020 other)
		: this()
	{
		achievementId_ = other.achievementId_;
		_unknownFields = UnknownFieldSet.Clone(other._unknownFields);
	}

	[DebuggerNonUserCode]
	public UpgradeAchievementConversionRateReq_1001020 Clone()
	{
		return new UpgradeAchievementConversionRateReq_1001020(this);
	}

	[DebuggerNonUserCode]
	public override bool Equals(object other)
	{
		return Equals(other as UpgradeAchievementConversionRateReq_1001020);
	}

	[DebuggerNonUserCode]
	public bool Equals(UpgradeAchievementConversionRateReq_1001020 other)
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
		if (_unknownFields != null)
		{
			num += _unknownFields.CalculateSize();
		}
		return num;
	}

	[DebuggerNonUserCode]
	public void MergeFrom(UpgradeAchievementConversionRateReq_1001020 other)
	{
		if (other != null)
		{
			if (other.AchievementId.Length != 0)
			{
				AchievementId = other.AchievementId;
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
				AchievementId = input.ReadString();
			}
		}
	}
}
