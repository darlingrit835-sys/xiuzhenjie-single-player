using System;
using System.Diagnostics;
using Google.Protobuf;
using Google.Protobuf.Reflection;

public sealed class PracticeSkillResp_1005040 : IMessage<PracticeSkillResp_1005040>, IMessage, IEquatable<PracticeSkillResp_1005040>, IDeepCloneable<PracticeSkillResp_1005040>
{
	private static readonly MessageParser<PracticeSkillResp_1005040> _parser = new MessageParser<PracticeSkillResp_1005040>(() => new PracticeSkillResp_1005040());

	private UnknownFieldSet _unknownFields;

	public const int CodeFieldNumber = 1;

	private int code_;

	public const int IsSuccessFieldNumber = 2;

	private bool isSuccess_;

	public const int ProficiencyFieldNumber = 3;

	private int proficiency_;

	public const int MaxProficiencyFieldNumber = 4;

	private int maxProficiency_;

	public const int LevelFieldNumber = 5;

	private int level_;

	public const int CurrentVigorFieldNumber = 6;

	private int currentVigor_;

	public const int LiveLevelFieldNumber = 7;

	private int liveLevel_;

	public const int LiveProficiencyFieldNumber = 8;

	private int liveProficiency_;

	[DebuggerNonUserCode]
	public static MessageParser<PracticeSkillResp_1005040> Parser => _parser;

	[DebuggerNonUserCode]
	public static MessageDescriptor Descriptor => HomeProtoReflection.Descriptor.MessageTypes[51];

	[DebuggerNonUserCode]
	MessageDescriptor IMessage.Descriptor => pb_003A_003AGoogle_002EProtobuf_002EIMessage_002Eget_Descriptor();

	[DebuggerNonUserCode]
	public int Code
	{
		get
		{
			return code_;
		}
		set
		{
			code_ = value;
		}
	}

	[DebuggerNonUserCode]
	public bool IsSuccess
	{
		get
		{
			return isSuccess_;
		}
		set
		{
			isSuccess_ = value;
		}
	}

	[DebuggerNonUserCode]
	public int Proficiency
	{
		get
		{
			return proficiency_;
		}
		set
		{
			proficiency_ = value;
		}
	}

	[DebuggerNonUserCode]
	public int MaxProficiency
	{
		get
		{
			return maxProficiency_;
		}
		set
		{
			maxProficiency_ = value;
		}
	}

	[DebuggerNonUserCode]
	public int Level
	{
		get
		{
			return level_;
		}
		set
		{
			level_ = value;
		}
	}

	[DebuggerNonUserCode]
	public int CurrentVigor
	{
		get
		{
			return currentVigor_;
		}
		set
		{
			currentVigor_ = value;
		}
	}

	[DebuggerNonUserCode]
	public int LiveLevel
	{
		get
		{
			return liveLevel_;
		}
		set
		{
			liveLevel_ = value;
		}
	}

	[DebuggerNonUserCode]
	public int LiveProficiency
	{
		get
		{
			return liveProficiency_;
		}
		set
		{
			liveProficiency_ = value;
		}
	}

	private MessageDescriptor pb_003A_003AGoogle_002EProtobuf_002EIMessage_002Eget_Descriptor()
	{
		return Descriptor;
	}

	[DebuggerNonUserCode]
	public PracticeSkillResp_1005040()
	{
	}

	[DebuggerNonUserCode]
	public PracticeSkillResp_1005040(PracticeSkillResp_1005040 other)
		: this()
	{
		code_ = other.code_;
		isSuccess_ = other.isSuccess_;
		proficiency_ = other.proficiency_;
		maxProficiency_ = other.maxProficiency_;
		level_ = other.level_;
		currentVigor_ = other.currentVigor_;
		liveLevel_ = other.liveLevel_;
		liveProficiency_ = other.liveProficiency_;
		_unknownFields = UnknownFieldSet.Clone(other._unknownFields);
	}

	[DebuggerNonUserCode]
	public PracticeSkillResp_1005040 Clone()
	{
		return new PracticeSkillResp_1005040(this);
	}

	[DebuggerNonUserCode]
	public override bool Equals(object other)
	{
		return Equals(other as PracticeSkillResp_1005040);
	}

	[DebuggerNonUserCode]
	public bool Equals(PracticeSkillResp_1005040 other)
	{
		if (other == null)
		{
			return false;
		}
		if (other == this)
		{
			return true;
		}
		if (Code != other.Code)
		{
			return false;
		}
		if (IsSuccess != other.IsSuccess)
		{
			return false;
		}
		if (Proficiency != other.Proficiency)
		{
			return false;
		}
		if (MaxProficiency != other.MaxProficiency)
		{
			return false;
		}
		if (Level != other.Level)
		{
			return false;
		}
		if (CurrentVigor != other.CurrentVigor)
		{
			return false;
		}
		if (LiveLevel != other.LiveLevel)
		{
			return false;
		}
		if (LiveProficiency != other.LiveProficiency)
		{
			return false;
		}
		return object.Equals(_unknownFields, other._unknownFields);
	}

	[DebuggerNonUserCode]
	public override int GetHashCode()
	{
		int num = 1;
		if (Code != 0)
		{
			num ^= Code.GetHashCode();
		}
		if (IsSuccess)
		{
			num ^= IsSuccess.GetHashCode();
		}
		if (Proficiency != 0)
		{
			num ^= Proficiency.GetHashCode();
		}
		if (MaxProficiency != 0)
		{
			num ^= MaxProficiency.GetHashCode();
		}
		if (Level != 0)
		{
			num ^= Level.GetHashCode();
		}
		if (CurrentVigor != 0)
		{
			num ^= CurrentVigor.GetHashCode();
		}
		if (LiveLevel != 0)
		{
			num ^= LiveLevel.GetHashCode();
		}
		if (LiveProficiency != 0)
		{
			num ^= LiveProficiency.GetHashCode();
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
		if (Code != 0)
		{
			output.WriteRawTag(8);
			output.WriteInt32(Code);
		}
		if (IsSuccess)
		{
			output.WriteRawTag(16);
			output.WriteBool(IsSuccess);
		}
		if (Proficiency != 0)
		{
			output.WriteRawTag(24);
			output.WriteInt32(Proficiency);
		}
		if (MaxProficiency != 0)
		{
			output.WriteRawTag(32);
			output.WriteInt32(MaxProficiency);
		}
		if (Level != 0)
		{
			output.WriteRawTag(40);
			output.WriteInt32(Level);
		}
		if (CurrentVigor != 0)
		{
			output.WriteRawTag(48);
			output.WriteInt32(CurrentVigor);
		}
		if (LiveLevel != 0)
		{
			output.WriteRawTag(56);
			output.WriteInt32(LiveLevel);
		}
		if (LiveProficiency != 0)
		{
			output.WriteRawTag(64);
			output.WriteInt32(LiveProficiency);
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
		if (Code != 0)
		{
			num += 1 + CodedOutputStream.ComputeInt32Size(Code);
		}
		if (IsSuccess)
		{
			num += 2;
		}
		if (Proficiency != 0)
		{
			num += 1 + CodedOutputStream.ComputeInt32Size(Proficiency);
		}
		if (MaxProficiency != 0)
		{
			num += 1 + CodedOutputStream.ComputeInt32Size(MaxProficiency);
		}
		if (Level != 0)
		{
			num += 1 + CodedOutputStream.ComputeInt32Size(Level);
		}
		if (CurrentVigor != 0)
		{
			num += 1 + CodedOutputStream.ComputeInt32Size(CurrentVigor);
		}
		if (LiveLevel != 0)
		{
			num += 1 + CodedOutputStream.ComputeInt32Size(LiveLevel);
		}
		if (LiveProficiency != 0)
		{
			num += 1 + CodedOutputStream.ComputeInt32Size(LiveProficiency);
		}
		if (_unknownFields != null)
		{
			num += _unknownFields.CalculateSize();
		}
		return num;
	}

	[DebuggerNonUserCode]
	public void MergeFrom(PracticeSkillResp_1005040 other)
	{
		if (other != null)
		{
			if (other.Code != 0)
			{
				Code = other.Code;
			}
			if (other.IsSuccess)
			{
				IsSuccess = other.IsSuccess;
			}
			if (other.Proficiency != 0)
			{
				Proficiency = other.Proficiency;
			}
			if (other.MaxProficiency != 0)
			{
				MaxProficiency = other.MaxProficiency;
			}
			if (other.Level != 0)
			{
				Level = other.Level;
			}
			if (other.CurrentVigor != 0)
			{
				CurrentVigor = other.CurrentVigor;
			}
			if (other.LiveLevel != 0)
			{
				LiveLevel = other.LiveLevel;
			}
			if (other.LiveProficiency != 0)
			{
				LiveProficiency = other.LiveProficiency;
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
				Code = input.ReadInt32();
				break;
			case 16u:
				IsSuccess = input.ReadBool();
				break;
			case 24u:
				Proficiency = input.ReadInt32();
				break;
			case 32u:
				MaxProficiency = input.ReadInt32();
				break;
			case 40u:
				Level = input.ReadInt32();
				break;
			case 48u:
				CurrentVigor = input.ReadInt32();
				break;
			case 56u:
				LiveLevel = input.ReadInt32();
				break;
			case 64u:
				LiveProficiency = input.ReadInt32();
				break;
			}
		}
	}
}
