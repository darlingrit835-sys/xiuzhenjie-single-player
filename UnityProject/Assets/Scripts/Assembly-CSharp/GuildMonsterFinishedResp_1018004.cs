using System;
using System.Diagnostics;
using Google.Protobuf;
using Google.Protobuf.Reflection;

public sealed class GuildMonsterFinishedResp_1018004 : IMessage<GuildMonsterFinishedResp_1018004>, IMessage, IEquatable<GuildMonsterFinishedResp_1018004>, IDeepCloneable<GuildMonsterFinishedResp_1018004>
{
	private static readonly MessageParser<GuildMonsterFinishedResp_1018004> _parser = new MessageParser<GuildMonsterFinishedResp_1018004>(() => new GuildMonsterFinishedResp_1018004());

	private UnknownFieldSet _unknownFields;

	public const int GuildIdFieldNumber = 1;

	private int guildId_;

	public const int IsVictoryFieldNumber = 2;

	private bool isVictory_;

	public const int LingCaiCountFieldNumber = 3;

	private int lingCaiCount_;

	public const int LingKuangCountFieldNumber = 4;

	private int lingKuangCount_;

	public const int LingZhiCountFieldNumber = 5;

	private int lingZhiCount_;

	public const int StoneCountFieldNumber = 6;

	private long stoneCount_;

	public const int JadeCountFieldNumber = 7;

	private int jadeCount_;

	public const int ChouJiangCishuFieldNumber = 8;

	private int chouJiangCishu_;

	public const int DeathCountFieldNumber = 9;

	private int deathCount_;

	public const int KillCountFieldNumber = 10;

	private int killCount_;

	[DebuggerNonUserCode]
	public static MessageParser<GuildMonsterFinishedResp_1018004> Parser => _parser;

	[DebuggerNonUserCode]
	public static MessageDescriptor Descriptor => GuildFightProtoReflection.Descriptor.MessageTypes[5];

	[DebuggerNonUserCode]
	MessageDescriptor IMessage.Descriptor => pb_003A_003AGoogle_002EProtobuf_002EIMessage_002Eget_Descriptor();

	[DebuggerNonUserCode]
	public int GuildId
	{
		get
		{
			return guildId_;
		}
		set
		{
			guildId_ = value;
		}
	}

	[DebuggerNonUserCode]
	public bool IsVictory
	{
		get
		{
			return isVictory_;
		}
		set
		{
			isVictory_ = value;
		}
	}

	[DebuggerNonUserCode]
	public int LingCaiCount
	{
		get
		{
			return lingCaiCount_;
		}
		set
		{
			lingCaiCount_ = value;
		}
	}

	[DebuggerNonUserCode]
	public int LingKuangCount
	{
		get
		{
			return lingKuangCount_;
		}
		set
		{
			lingKuangCount_ = value;
		}
	}

	[DebuggerNonUserCode]
	public int LingZhiCount
	{
		get
		{
			return lingZhiCount_;
		}
		set
		{
			lingZhiCount_ = value;
		}
	}

	[DebuggerNonUserCode]
	public long StoneCount
	{
		get
		{
			return stoneCount_;
		}
		set
		{
			stoneCount_ = value;
		}
	}

	[DebuggerNonUserCode]
	public int JadeCount
	{
		get
		{
			return jadeCount_;
		}
		set
		{
			jadeCount_ = value;
		}
	}

	[DebuggerNonUserCode]
	public int ChouJiangCishu
	{
		get
		{
			return chouJiangCishu_;
		}
		set
		{
			chouJiangCishu_ = value;
		}
	}

	[DebuggerNonUserCode]
	public int DeathCount
	{
		get
		{
			return deathCount_;
		}
		set
		{
			deathCount_ = value;
		}
	}

	[DebuggerNonUserCode]
	public int KillCount
	{
		get
		{
			return killCount_;
		}
		set
		{
			killCount_ = value;
		}
	}

	private MessageDescriptor pb_003A_003AGoogle_002EProtobuf_002EIMessage_002Eget_Descriptor()
	{
		return Descriptor;
	}

	[DebuggerNonUserCode]
	public GuildMonsterFinishedResp_1018004()
	{
	}

	[DebuggerNonUserCode]
	public GuildMonsterFinishedResp_1018004(GuildMonsterFinishedResp_1018004 other)
		: this()
	{
		guildId_ = other.guildId_;
		isVictory_ = other.isVictory_;
		lingCaiCount_ = other.lingCaiCount_;
		lingKuangCount_ = other.lingKuangCount_;
		lingZhiCount_ = other.lingZhiCount_;
		stoneCount_ = other.stoneCount_;
		jadeCount_ = other.jadeCount_;
		chouJiangCishu_ = other.chouJiangCishu_;
		deathCount_ = other.deathCount_;
		killCount_ = other.killCount_;
		_unknownFields = UnknownFieldSet.Clone(other._unknownFields);
	}

	[DebuggerNonUserCode]
	public GuildMonsterFinishedResp_1018004 Clone()
	{
		return new GuildMonsterFinishedResp_1018004(this);
	}

	[DebuggerNonUserCode]
	public override bool Equals(object other)
	{
		return Equals(other as GuildMonsterFinishedResp_1018004);
	}

	[DebuggerNonUserCode]
	public bool Equals(GuildMonsterFinishedResp_1018004 other)
	{
		if (other == null)
		{
			return false;
		}
		if (other == this)
		{
			return true;
		}
		if (GuildId != other.GuildId)
		{
			return false;
		}
		if (IsVictory != other.IsVictory)
		{
			return false;
		}
		if (LingCaiCount != other.LingCaiCount)
		{
			return false;
		}
		if (LingKuangCount != other.LingKuangCount)
		{
			return false;
		}
		if (LingZhiCount != other.LingZhiCount)
		{
			return false;
		}
		if (StoneCount != other.StoneCount)
		{
			return false;
		}
		if (JadeCount != other.JadeCount)
		{
			return false;
		}
		if (ChouJiangCishu != other.ChouJiangCishu)
		{
			return false;
		}
		if (DeathCount != other.DeathCount)
		{
			return false;
		}
		if (KillCount != other.KillCount)
		{
			return false;
		}
		return object.Equals(_unknownFields, other._unknownFields);
	}

	[DebuggerNonUserCode]
	public override int GetHashCode()
	{
		int num = 1;
		if (GuildId != 0)
		{
			num ^= GuildId.GetHashCode();
		}
		if (IsVictory)
		{
			num ^= IsVictory.GetHashCode();
		}
		if (LingCaiCount != 0)
		{
			num ^= LingCaiCount.GetHashCode();
		}
		if (LingKuangCount != 0)
		{
			num ^= LingKuangCount.GetHashCode();
		}
		if (LingZhiCount != 0)
		{
			num ^= LingZhiCount.GetHashCode();
		}
		if (StoneCount != 0L)
		{
			num ^= StoneCount.GetHashCode();
		}
		if (JadeCount != 0)
		{
			num ^= JadeCount.GetHashCode();
		}
		if (ChouJiangCishu != 0)
		{
			num ^= ChouJiangCishu.GetHashCode();
		}
		if (DeathCount != 0)
		{
			num ^= DeathCount.GetHashCode();
		}
		if (KillCount != 0)
		{
			num ^= KillCount.GetHashCode();
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
		if (GuildId != 0)
		{
			output.WriteRawTag(8);
			output.WriteInt32(GuildId);
		}
		if (IsVictory)
		{
			output.WriteRawTag(16);
			output.WriteBool(IsVictory);
		}
		if (LingCaiCount != 0)
		{
			output.WriteRawTag(24);
			output.WriteInt32(LingCaiCount);
		}
		if (LingKuangCount != 0)
		{
			output.WriteRawTag(32);
			output.WriteInt32(LingKuangCount);
		}
		if (LingZhiCount != 0)
		{
			output.WriteRawTag(40);
			output.WriteInt32(LingZhiCount);
		}
		if (StoneCount != 0L)
		{
			output.WriteRawTag(48);
			output.WriteInt64(StoneCount);
		}
		if (JadeCount != 0)
		{
			output.WriteRawTag(56);
			output.WriteInt32(JadeCount);
		}
		if (ChouJiangCishu != 0)
		{
			output.WriteRawTag(64);
			output.WriteInt32(ChouJiangCishu);
		}
		if (DeathCount != 0)
		{
			output.WriteRawTag(72);
			output.WriteInt32(DeathCount);
		}
		if (KillCount != 0)
		{
			output.WriteRawTag(80);
			output.WriteInt32(KillCount);
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
		if (GuildId != 0)
		{
			num += 1 + CodedOutputStream.ComputeInt32Size(GuildId);
		}
		if (IsVictory)
		{
			num += 2;
		}
		if (LingCaiCount != 0)
		{
			num += 1 + CodedOutputStream.ComputeInt32Size(LingCaiCount);
		}
		if (LingKuangCount != 0)
		{
			num += 1 + CodedOutputStream.ComputeInt32Size(LingKuangCount);
		}
		if (LingZhiCount != 0)
		{
			num += 1 + CodedOutputStream.ComputeInt32Size(LingZhiCount);
		}
		if (StoneCount != 0L)
		{
			num += 1 + CodedOutputStream.ComputeInt64Size(StoneCount);
		}
		if (JadeCount != 0)
		{
			num += 1 + CodedOutputStream.ComputeInt32Size(JadeCount);
		}
		if (ChouJiangCishu != 0)
		{
			num += 1 + CodedOutputStream.ComputeInt32Size(ChouJiangCishu);
		}
		if (DeathCount != 0)
		{
			num += 1 + CodedOutputStream.ComputeInt32Size(DeathCount);
		}
		if (KillCount != 0)
		{
			num += 1 + CodedOutputStream.ComputeInt32Size(KillCount);
		}
		if (_unknownFields != null)
		{
			num += _unknownFields.CalculateSize();
		}
		return num;
	}

	[DebuggerNonUserCode]
	public void MergeFrom(GuildMonsterFinishedResp_1018004 other)
	{
		if (other != null)
		{
			if (other.GuildId != 0)
			{
				GuildId = other.GuildId;
			}
			if (other.IsVictory)
			{
				IsVictory = other.IsVictory;
			}
			if (other.LingCaiCount != 0)
			{
				LingCaiCount = other.LingCaiCount;
			}
			if (other.LingKuangCount != 0)
			{
				LingKuangCount = other.LingKuangCount;
			}
			if (other.LingZhiCount != 0)
			{
				LingZhiCount = other.LingZhiCount;
			}
			if (other.StoneCount != 0L)
			{
				StoneCount = other.StoneCount;
			}
			if (other.JadeCount != 0)
			{
				JadeCount = other.JadeCount;
			}
			if (other.ChouJiangCishu != 0)
			{
				ChouJiangCishu = other.ChouJiangCishu;
			}
			if (other.DeathCount != 0)
			{
				DeathCount = other.DeathCount;
			}
			if (other.KillCount != 0)
			{
				KillCount = other.KillCount;
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
				GuildId = input.ReadInt32();
				break;
			case 16u:
				IsVictory = input.ReadBool();
				break;
			case 24u:
				LingCaiCount = input.ReadInt32();
				break;
			case 32u:
				LingKuangCount = input.ReadInt32();
				break;
			case 40u:
				LingZhiCount = input.ReadInt32();
				break;
			case 48u:
				StoneCount = input.ReadInt64();
				break;
			case 56u:
				JadeCount = input.ReadInt32();
				break;
			case 64u:
				ChouJiangCishu = input.ReadInt32();
				break;
			case 72u:
				DeathCount = input.ReadInt32();
				break;
			case 80u:
				KillCount = input.ReadInt32();
				break;
			}
		}
	}
}
