using System;
using System.Diagnostics;
using Google.Protobuf;
using Google.Protobuf.Reflection;

public sealed class RemoveSkillReq_1009004 : IMessage<RemoveSkillReq_1009004>, IMessage, IEquatable<RemoveSkillReq_1009004>, IDeepCloneable<RemoveSkillReq_1009004>
{
	private static readonly MessageParser<RemoveSkillReq_1009004> _parser = new MessageParser<RemoveSkillReq_1009004>(() => new RemoveSkillReq_1009004());

	private UnknownFieldSet _unknownFields;

	public const int SkillTypeFieldNumber = 1;

	private int skillType_;

	public const int SkillIdFieldNumber = 2;

	private string skillId_ = "";

	[DebuggerNonUserCode]
	public static MessageParser<RemoveSkillReq_1009004> Parser => _parser;

	[DebuggerNonUserCode]
	public static MessageDescriptor Descriptor => BattleSettingProtoReflection.Descriptor.MessageTypes[6];

	[DebuggerNonUserCode]
	MessageDescriptor IMessage.Descriptor => pb_003A_003AGoogle_002EProtobuf_002EIMessage_002Eget_Descriptor();

	[DebuggerNonUserCode]
	public int SkillType
	{
		get
		{
			return skillType_;
		}
		set
		{
			skillType_ = value;
		}
	}

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
	public RemoveSkillReq_1009004()
	{
	}

	[DebuggerNonUserCode]
	public RemoveSkillReq_1009004(RemoveSkillReq_1009004 other)
		: this()
	{
		skillType_ = other.skillType_;
		skillId_ = other.skillId_;
		_unknownFields = UnknownFieldSet.Clone(other._unknownFields);
	}

	[DebuggerNonUserCode]
	public RemoveSkillReq_1009004 Clone()
	{
		return new RemoveSkillReq_1009004(this);
	}

	[DebuggerNonUserCode]
	public override bool Equals(object other)
	{
		return Equals(other as RemoveSkillReq_1009004);
	}

	[DebuggerNonUserCode]
	public bool Equals(RemoveSkillReq_1009004 other)
	{
		if (other == null)
		{
			return false;
		}
		if (other == this)
		{
			return true;
		}
		if (SkillType != other.SkillType)
		{
			return false;
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
		if (SkillType != 0)
		{
			num ^= SkillType.GetHashCode();
		}
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
		if (SkillType != 0)
		{
			output.WriteRawTag(8);
			output.WriteInt32(SkillType);
		}
		if (SkillId.Length != 0)
		{
			output.WriteRawTag(18);
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
		if (SkillType != 0)
		{
			num += 1 + CodedOutputStream.ComputeInt32Size(SkillType);
		}
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
	public void MergeFrom(RemoveSkillReq_1009004 other)
	{
		if (other != null)
		{
			if (other.SkillType != 0)
			{
				SkillType = other.SkillType;
			}
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
			switch (num)
			{
			default:
				_unknownFields = UnknownFieldSet.MergeFieldFrom(_unknownFields, input);
				break;
			case 8u:
				SkillType = input.ReadInt32();
				break;
			case 18u:
				SkillId = input.ReadString();
				break;
			}
		}
	}
}
