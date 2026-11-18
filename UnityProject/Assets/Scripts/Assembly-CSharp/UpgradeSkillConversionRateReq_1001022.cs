using System;
using System.Diagnostics;
using Google.Protobuf;
using Google.Protobuf.Reflection;

public sealed class UpgradeSkillConversionRateReq_1001022 : IMessage<UpgradeSkillConversionRateReq_1001022>, IMessage, IEquatable<UpgradeSkillConversionRateReq_1001022>, IDeepCloneable<UpgradeSkillConversionRateReq_1001022>
{
	private static readonly MessageParser<UpgradeSkillConversionRateReq_1001022> _parser = new MessageParser<UpgradeSkillConversionRateReq_1001022>(() => new UpgradeSkillConversionRateReq_1001022());

	private UnknownFieldSet _unknownFields;

	public const int SkillIdFieldNumber = 1;

	private string skillId_ = "";

	[DebuggerNonUserCode]
	public static MessageParser<UpgradeSkillConversionRateReq_1001022> Parser => _parser;

	[DebuggerNonUserCode]
	public static MessageDescriptor Descriptor => SimpleActionProtoReflection.Descriptor.MessageTypes[27];

	[DebuggerNonUserCode]
	MessageDescriptor IMessage.Descriptor => pb_003A_003AGoogle_002EProtobuf_002EIMessage_002Eget_Descriptor();

	[DebuggerNonUserCode]
	public string SkillId
	{
		get
		{
			return skillId_;
		}
		set
		{
			skillId_ = ProtoPreconditions.CheckNotNull(value, "value");
		}
	}

	private MessageDescriptor pb_003A_003AGoogle_002EProtobuf_002EIMessage_002Eget_Descriptor()
	{
		return Descriptor;
	}

	[DebuggerNonUserCode]
	public UpgradeSkillConversionRateReq_1001022()
	{
	}

	[DebuggerNonUserCode]
	public UpgradeSkillConversionRateReq_1001022(UpgradeSkillConversionRateReq_1001022 other)
		: this()
	{
		skillId_ = other.skillId_;
		_unknownFields = UnknownFieldSet.Clone(other._unknownFields);
	}

	[DebuggerNonUserCode]
	public UpgradeSkillConversionRateReq_1001022 Clone()
	{
		return new UpgradeSkillConversionRateReq_1001022(this);
	}

	[DebuggerNonUserCode]
	public override bool Equals(object other)
	{
		return Equals(other as UpgradeSkillConversionRateReq_1001022);
	}

	[DebuggerNonUserCode]
	public bool Equals(UpgradeSkillConversionRateReq_1001022 other)
	{
		if (other == null)
		{
			return false;
		}
		if (other == this)
		{
			return true;
		}
		if (SkillId != other.SkillId)
		{
			return false;
		}
		return object.Equals(_unknownFields, other._unknownFields);
	}

	[DebuggerNonUserCode]
	public override int GetHashCode()
	{
		int num = 1;
		if (SkillId.Length != 0)
		{
			num ^= SkillId.GetHashCode();
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
		if (SkillId.Length != 0)
		{
			output.WriteRawTag(10);
			output.WriteString(SkillId);
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
		if (SkillId.Length != 0)
		{
			num += 1 + CodedOutputStream.ComputeStringSize(SkillId);
		}
		if (_unknownFields != null)
		{
			num += _unknownFields.CalculateSize();
		}
		return num;
	}

	[DebuggerNonUserCode]
	public void MergeFrom(UpgradeSkillConversionRateReq_1001022 other)
	{
		if (other != null)
		{
			if (other.SkillId.Length != 0)
			{
				SkillId = other.SkillId;
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
				SkillId = input.ReadString();
			}
		}
	}
}
