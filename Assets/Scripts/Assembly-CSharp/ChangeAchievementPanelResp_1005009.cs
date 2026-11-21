using System;
using System.Diagnostics;
using Google.Protobuf;
using Google.Protobuf.Collections;
using Google.Protobuf.Reflection;

public sealed class ChangeAchievementPanelResp_1005009 : IMessage<ChangeAchievementPanelResp_1005009>, IMessage, IEquatable<ChangeAchievementPanelResp_1005009>, IDeepCloneable<ChangeAchievementPanelResp_1005009>
{
	[DebuggerNonUserCode]
	public static class Types
	{
		public sealed class AchievementObjectResp_1005010 : IMessage<AchievementObjectResp_1005010>, IMessage, IEquatable<AchievementObjectResp_1005010>, IDeepCloneable<AchievementObjectResp_1005010>
		{
			private static readonly MessageParser<AchievementObjectResp_1005010> _parser = new MessageParser<AchievementObjectResp_1005010>(() => new AchievementObjectResp_1005010());

			private UnknownFieldSet _unknownFields;

			public const int IdFieldNumber = 1;

			private string id_ = "";

			public const int NameFieldNumber = 2;

			private string name_ = "";

			public const int CircleFieldNumber = 3;

			private double circle_;

			public const int MaxCountFieldNumber = 4;

			private int maxCount_;

			public const int UsedCountFieldNumber = 5;

			private int usedCount_;

			public const int AchievementLevelFieldNumber = 6;

			private int achievementLevel_;

			public const int LevelFieldNumber = 7;

			private int level_;

			public const int RankFieldNumber = 8;

			private int rank_;

			public const int FiveElementFieldNumber = 9;

			private int fiveElement_;

			[DebuggerNonUserCode]
			public static MessageParser<AchievementObjectResp_1005010> Parser => _parser;

			[DebuggerNonUserCode]
			public static MessageDescriptor Descriptor => ChangeAchievementPanelResp_1005009.Descriptor.NestedTypes[0];

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
			public double Circle
			{
				get
				{
					return circle_;
				}
				set
				{
					circle_ = value;
				}
			}

			[DebuggerNonUserCode]
			public int MaxCount
			{
				get
				{
					return maxCount_;
				}
				set
				{
					maxCount_ = value;
				}
			}

			[DebuggerNonUserCode]
			public int UsedCount
			{
				get
				{
					return usedCount_;
				}
				set
				{
					usedCount_ = value;
				}
			}

			[DebuggerNonUserCode]
			public int AchievementLevel
			{
				get
				{
					return achievementLevel_;
				}
				set
				{
					achievementLevel_ = value;
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
			public int Rank
			{
				get
				{
					return rank_;
				}
				set
				{
					rank_ = value;
				}
			}

			[DebuggerNonUserCode]
			public int FiveElement
			{
				get
				{
					return fiveElement_;
				}
				set
				{
					fiveElement_ = value;
				}
			}

			private MessageDescriptor pb_003A_003AGoogle_002EProtobuf_002EIMessage_002Eget_Descriptor()
			{
				return Descriptor;
			}

			[DebuggerNonUserCode]
			public AchievementObjectResp_1005010()
			{
			}

			[DebuggerNonUserCode]
			public AchievementObjectResp_1005010(AchievementObjectResp_1005010 other)
				: this()
			{
				id_ = other.id_;
				name_ = other.name_;
				circle_ = other.circle_;
				maxCount_ = other.maxCount_;
				usedCount_ = other.usedCount_;
				achievementLevel_ = other.achievementLevel_;
				level_ = other.level_;
				rank_ = other.rank_;
				fiveElement_ = other.fiveElement_;
				_unknownFields = UnknownFieldSet.Clone(other._unknownFields);
			}

			[DebuggerNonUserCode]
			public AchievementObjectResp_1005010 Clone()
			{
				return new AchievementObjectResp_1005010(this);
			}

			[DebuggerNonUserCode]
			public override bool Equals(object other)
			{
				return Equals(other as AchievementObjectResp_1005010);
			}

			[DebuggerNonUserCode]
			public bool Equals(AchievementObjectResp_1005010 other)
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
				if (Name != other.Name)
				{
					return false;
				}
				if (!ProtobufEqualityComparers.BitwiseDoubleEqualityComparer.Equals(Circle, other.Circle))
				{
					return false;
				}
				if (MaxCount != other.MaxCount)
				{
					return false;
				}
				if (UsedCount != other.UsedCount)
				{
					return false;
				}
				if (AchievementLevel != other.AchievementLevel)
				{
					return false;
				}
				if (Level != other.Level)
				{
					return false;
				}
				if (Rank != other.Rank)
				{
					return false;
				}
				if (FiveElement != other.FiveElement)
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
				if (Name.Length != 0)
				{
					num ^= Name.GetHashCode();
				}
				if (Circle != 0.0)
				{
					num ^= ProtobufEqualityComparers.BitwiseDoubleEqualityComparer.GetHashCode(Circle);
				}
				if (MaxCount != 0)
				{
					num ^= MaxCount.GetHashCode();
				}
				if (UsedCount != 0)
				{
					num ^= UsedCount.GetHashCode();
				}
				if (AchievementLevel != 0)
				{
					num ^= AchievementLevel.GetHashCode();
				}
				if (Level != 0)
				{
					num ^= Level.GetHashCode();
				}
				if (Rank != 0)
				{
					num ^= Rank.GetHashCode();
				}
				if (FiveElement != 0)
				{
					num ^= FiveElement.GetHashCode();
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
				if (Name.Length != 0)
				{
					output.WriteRawTag(18);
					output.WriteString(Name);
				}
				if (Circle != 0.0)
				{
					output.WriteRawTag(25);
					output.WriteDouble(Circle);
				}
				if (MaxCount != 0)
				{
					output.WriteRawTag(32);
					output.WriteInt32(MaxCount);
				}
				if (UsedCount != 0)
				{
					output.WriteRawTag(40);
					output.WriteInt32(UsedCount);
				}
				if (AchievementLevel != 0)
				{
					output.WriteRawTag(48);
					output.WriteInt32(AchievementLevel);
				}
				if (Level != 0)
				{
					output.WriteRawTag(56);
					output.WriteInt32(Level);
				}
				if (Rank != 0)
				{
					output.WriteRawTag(64);
					output.WriteInt32(Rank);
				}
				if (FiveElement != 0)
				{
					output.WriteRawTag(72);
					output.WriteInt32(FiveElement);
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
				if (Name.Length != 0)
				{
					num += 1 + CodedOutputStream.ComputeStringSize(Name);
				}
				if (Circle != 0.0)
				{
					num += 9;
				}
				if (MaxCount != 0)
				{
					num += 1 + CodedOutputStream.ComputeInt32Size(MaxCount);
				}
				if (UsedCount != 0)
				{
					num += 1 + CodedOutputStream.ComputeInt32Size(UsedCount);
				}
				if (AchievementLevel != 0)
				{
					num += 1 + CodedOutputStream.ComputeInt32Size(AchievementLevel);
				}
				if (Level != 0)
				{
					num += 1 + CodedOutputStream.ComputeInt32Size(Level);
				}
				if (Rank != 0)
				{
					num += 1 + CodedOutputStream.ComputeInt32Size(Rank);
				}
				if (FiveElement != 0)
				{
					num += 1 + CodedOutputStream.ComputeInt32Size(FiveElement);
				}
				if (_unknownFields != null)
				{
					num += _unknownFields.CalculateSize();
				}
				return num;
			}

			[DebuggerNonUserCode]
			public void MergeFrom(AchievementObjectResp_1005010 other)
			{
				if (other != null)
				{
					if (other.Id.Length != 0)
					{
						Id = other.Id;
					}
					if (other.Name.Length != 0)
					{
						Name = other.Name;
					}
					if (other.Circle != 0.0)
					{
						Circle = other.Circle;
					}
					if (other.MaxCount != 0)
					{
						MaxCount = other.MaxCount;
					}
					if (other.UsedCount != 0)
					{
						UsedCount = other.UsedCount;
					}
					if (other.AchievementLevel != 0)
					{
						AchievementLevel = other.AchievementLevel;
					}
					if (other.Level != 0)
					{
						Level = other.Level;
					}
					if (other.Rank != 0)
					{
						Rank = other.Rank;
					}
					if (other.FiveElement != 0)
					{
						FiveElement = other.FiveElement;
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
					case 18u:
						Name = input.ReadString();
						break;
					case 25u:
						Circle = input.ReadDouble();
						break;
					case 32u:
						MaxCount = input.ReadInt32();
						break;
					case 40u:
						UsedCount = input.ReadInt32();
						break;
					case 48u:
						AchievementLevel = input.ReadInt32();
						break;
					case 56u:
						Level = input.ReadInt32();
						break;
					case 64u:
						Rank = input.ReadInt32();
						break;
					case 72u:
						FiveElement = input.ReadInt32();
						break;
					}
				}
			}
		}
	}

	private static readonly MessageParser<ChangeAchievementPanelResp_1005009> _parser = new MessageParser<ChangeAchievementPanelResp_1005009>(() => new ChangeAchievementPanelResp_1005009());

	private UnknownFieldSet _unknownFields;

	public const int CodeFieldNumber = 1;

	private int code_;

	public const int IsSuccessFieldNumber = 2;

	private bool isSuccess_;

	public const int CurrentAchievementFieldNumber = 3;

	private Types.AchievementObjectResp_1005010 currentAchievement_;

	public const int AchievementsFieldNumber = 4;

	private static readonly FieldCodec<Types.AchievementObjectResp_1005010> _repeated_achievements_codec = FieldCodec.ForMessage(34u, Types.AchievementObjectResp_1005010.Parser);

	private readonly RepeatedField<Types.AchievementObjectResp_1005010> achievements_ = new RepeatedField<Types.AchievementObjectResp_1005010>();

	[DebuggerNonUserCode]
	public static MessageParser<ChangeAchievementPanelResp_1005009> Parser => _parser;

	[DebuggerNonUserCode]
	public static MessageDescriptor Descriptor => HomeProtoReflection.Descriptor.MessageTypes[13];

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
	public Types.AchievementObjectResp_1005010 CurrentAchievement
	{
		get
		{
			return currentAchievement_;
		}
		set
		{
			currentAchievement_ = value;
		}
	}

	[DebuggerNonUserCode]
	public RepeatedField<Types.AchievementObjectResp_1005010> Achievements => achievements_;

	private MessageDescriptor pb_003A_003AGoogle_002EProtobuf_002EIMessage_002Eget_Descriptor()
	{
		return Descriptor;
	}

	[DebuggerNonUserCode]
	public ChangeAchievementPanelResp_1005009()
	{
	}

	[DebuggerNonUserCode]
	public ChangeAchievementPanelResp_1005009(ChangeAchievementPanelResp_1005009 other)
		: this()
	{
		code_ = other.code_;
		isSuccess_ = other.isSuccess_;
		currentAchievement_ = ((other.currentAchievement_ != null) ? other.currentAchievement_.Clone() : null);
		achievements_ = other.achievements_.Clone();
		_unknownFields = UnknownFieldSet.Clone(other._unknownFields);
	}

	[DebuggerNonUserCode]
	public ChangeAchievementPanelResp_1005009 Clone()
	{
		return new ChangeAchievementPanelResp_1005009(this);
	}

	[DebuggerNonUserCode]
	public override bool Equals(object other)
	{
		return Equals(other as ChangeAchievementPanelResp_1005009);
	}

	[DebuggerNonUserCode]
	public bool Equals(ChangeAchievementPanelResp_1005009 other)
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
		if (!object.Equals(CurrentAchievement, other.CurrentAchievement))
		{
			return false;
		}
		if (!achievements_.Equals(other.achievements_))
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
		if (currentAchievement_ != null)
		{
			num ^= CurrentAchievement.GetHashCode();
		}
		num ^= achievements_.GetHashCode();
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
		if (currentAchievement_ != null)
		{
			output.WriteRawTag(26);
			output.WriteMessage(CurrentAchievement);
		}
		achievements_.WriteTo(output, _repeated_achievements_codec);
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
		if (currentAchievement_ != null)
		{
			num += 1 + CodedOutputStream.ComputeMessageSize(CurrentAchievement);
		}
		num += achievements_.CalculateSize(_repeated_achievements_codec);
		if (_unknownFields != null)
		{
			num += _unknownFields.CalculateSize();
		}
		return num;
	}

	[DebuggerNonUserCode]
	public void MergeFrom(ChangeAchievementPanelResp_1005009 other)
	{
		if (other == null)
		{
			return;
		}
		if (other.Code != 0)
		{
			Code = other.Code;
		}
		if (other.IsSuccess)
		{
			IsSuccess = other.IsSuccess;
		}
		if (other.currentAchievement_ != null)
		{
			if (currentAchievement_ == null)
			{
				CurrentAchievement = new Types.AchievementObjectResp_1005010();
			}
			CurrentAchievement.MergeFrom(other.CurrentAchievement);
		}
		achievements_.Add(other.achievements_);
		_unknownFields = UnknownFieldSet.MergeFrom(_unknownFields, other._unknownFields);
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
			case 26u:
				if (currentAchievement_ == null)
				{
					CurrentAchievement = new Types.AchievementObjectResp_1005010();
				}
				input.ReadMessage(CurrentAchievement);
				break;
			case 34u:
				achievements_.AddEntriesFrom(input, _repeated_achievements_codec);
				break;
			}
		}
	}
}
