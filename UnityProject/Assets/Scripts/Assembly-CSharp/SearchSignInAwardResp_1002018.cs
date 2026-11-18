using System;
using System.Diagnostics;
using Google.Protobuf;
using Google.Protobuf.Collections;
using Google.Protobuf.Reflection;

public sealed class SearchSignInAwardResp_1002018 : IMessage<SearchSignInAwardResp_1002018>, IMessage, IEquatable<SearchSignInAwardResp_1002018>, IDeepCloneable<SearchSignInAwardResp_1002018>
{
	[DebuggerNonUserCode]
	public static class Types
	{
		public sealed class SignInAwardResp_1002019 : IMessage<SignInAwardResp_1002019>, IMessage, IEquatable<SignInAwardResp_1002019>, IDeepCloneable<SignInAwardResp_1002019>
		{
			[DebuggerNonUserCode]
			public static class Types
			{
				public sealed class AwardResp_1002020 : IMessage<AwardResp_1002020>, IMessage, IEquatable<AwardResp_1002020>, IDeepCloneable<AwardResp_1002020>
				{
					private static readonly MessageParser<AwardResp_1002020> _parser = new MessageParser<AwardResp_1002020>(() => new AwardResp_1002020());

					private UnknownFieldSet _unknownFields;

					public const int GoodsNameFieldNumber = 1;

					private string goodsName_ = "";

					public const int NumFieldNumber = 2;

					private int num_;

					public const int TypeFieldNumber = 3;

					private int type_;

					public const int RankFieldNumber = 4;

					private int rank_;

					[DebuggerNonUserCode]
					public static MessageParser<AwardResp_1002020> Parser => _parser;

					[DebuggerNonUserCode]
					public static MessageDescriptor Descriptor => SignInAwardResp_1002019.Descriptor.NestedTypes[0];

					[DebuggerNonUserCode]
					MessageDescriptor IMessage.Descriptor => pb_003A_003AGoogle_002EProtobuf_002EIMessage_002Eget_Descriptor();

					[DebuggerNonUserCode]
					public string GoodsName
					{
						get
						{
							return goodsName_;
						}
						set
						{
							goodsName_ = ProtoPreconditions.CheckNotNull(value, "value");
						}
					}

					[DebuggerNonUserCode]
					public int Num
					{
						get
						{
							return num_;
						}
						set
						{
							num_ = value;
						}
					}

					[DebuggerNonUserCode]
					public int Type
					{
						get
						{
							return type_;
						}
						set
						{
							type_ = value;
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

					private MessageDescriptor pb_003A_003AGoogle_002EProtobuf_002EIMessage_002Eget_Descriptor()
					{
						return Descriptor;
					}

					[DebuggerNonUserCode]
					public AwardResp_1002020()
					{
					}

					[DebuggerNonUserCode]
					public AwardResp_1002020(AwardResp_1002020 other)
						: this()
					{
						goodsName_ = other.goodsName_;
						num_ = other.num_;
						type_ = other.type_;
						rank_ = other.rank_;
						_unknownFields = UnknownFieldSet.Clone(other._unknownFields);
					}

					[DebuggerNonUserCode]
					public AwardResp_1002020 Clone()
					{
						return new AwardResp_1002020(this);
					}

					[DebuggerNonUserCode]
					public override bool Equals(object other)
					{
						return Equals(other as AwardResp_1002020);
					}

					[DebuggerNonUserCode]
					public bool Equals(AwardResp_1002020 other)
					{
						if (other == null)
						{
							return false;
						}
						if (other == this)
						{
							return true;
						}
						if (GoodsName != other.GoodsName)
						{
							return false;
						}
						if (Num != other.Num)
						{
							return false;
						}
						if (Type != other.Type)
						{
							return false;
						}
						if (Rank != other.Rank)
						{
							return false;
						}
						return object.Equals(_unknownFields, other._unknownFields);
					}

					[DebuggerNonUserCode]
					public override int GetHashCode()
					{
						int num = 1;
						if (GoodsName.Length != 0)
						{
							num ^= GoodsName.GetHashCode();
						}
						if (Num != 0)
						{
							num ^= Num.GetHashCode();
						}
						if (Type != 0)
						{
							num ^= Type.GetHashCode();
						}
						if (Rank != 0)
						{
							num ^= Rank.GetHashCode();
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
						if (GoodsName.Length != 0)
						{
							output.WriteRawTag(10);
							output.WriteString(GoodsName);
						}
						if (Num != 0)
						{
							output.WriteRawTag(16);
							output.WriteInt32(Num);
						}
						if (Type != 0)
						{
							output.WriteRawTag(24);
							output.WriteInt32(Type);
						}
						if (Rank != 0)
						{
							output.WriteRawTag(32);
							output.WriteInt32(Rank);
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
						if (GoodsName.Length != 0)
						{
							num += 1 + CodedOutputStream.ComputeStringSize(GoodsName);
						}
						if (Num != 0)
						{
							num += 1 + CodedOutputStream.ComputeInt32Size(Num);
						}
						if (Type != 0)
						{
							num += 1 + CodedOutputStream.ComputeInt32Size(Type);
						}
						if (Rank != 0)
						{
							num += 1 + CodedOutputStream.ComputeInt32Size(Rank);
						}
						if (_unknownFields != null)
						{
							num += _unknownFields.CalculateSize();
						}
						return num;
					}

					[DebuggerNonUserCode]
					public void MergeFrom(AwardResp_1002020 other)
					{
						if (other != null)
						{
							if (other.GoodsName.Length != 0)
							{
								GoodsName = other.GoodsName;
							}
							if (other.Num != 0)
							{
								Num = other.Num;
							}
							if (other.Type != 0)
							{
								Type = other.Type;
							}
							if (other.Rank != 0)
							{
								Rank = other.Rank;
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
								GoodsName = input.ReadString();
								break;
							case 16u:
								Num = input.ReadInt32();
								break;
							case 24u:
								Type = input.ReadInt32();
								break;
							case 32u:
								Rank = input.ReadInt32();
								break;
							}
						}
					}
				}
			}

			private static readonly MessageParser<SignInAwardResp_1002019> _parser = new MessageParser<SignInAwardResp_1002019>(() => new SignInAwardResp_1002019());

			private UnknownFieldSet _unknownFields;

			public const int SignInDayFieldNumber = 1;

			private int signInDay_;

			public const int AwardFieldNumber = 2;

			private static readonly FieldCodec<Types.AwardResp_1002020> _repeated_award_codec = FieldCodec.ForMessage(18u, Types.AwardResp_1002020.Parser);

			private readonly RepeatedField<Types.AwardResp_1002020> award_ = new RepeatedField<Types.AwardResp_1002020>();

			[DebuggerNonUserCode]
			public static MessageParser<SignInAwardResp_1002019> Parser => _parser;

			[DebuggerNonUserCode]
			public static MessageDescriptor Descriptor => SearchSignInAwardResp_1002018.Descriptor.NestedTypes[0];

			[DebuggerNonUserCode]
			MessageDescriptor IMessage.Descriptor => pb_003A_003AGoogle_002EProtobuf_002EIMessage_002Eget_Descriptor();

			[DebuggerNonUserCode]
			public int SignInDay
			{
				get
				{
					return signInDay_;
				}
				set
				{
					signInDay_ = value;
				}
			}

			[DebuggerNonUserCode]
			public RepeatedField<Types.AwardResp_1002020> Award => award_;

			private MessageDescriptor pb_003A_003AGoogle_002EProtobuf_002EIMessage_002Eget_Descriptor()
			{
				return Descriptor;
			}

			[DebuggerNonUserCode]
			public SignInAwardResp_1002019()
			{
			}

			[DebuggerNonUserCode]
			public SignInAwardResp_1002019(SignInAwardResp_1002019 other)
				: this()
			{
				signInDay_ = other.signInDay_;
				award_ = other.award_.Clone();
				_unknownFields = UnknownFieldSet.Clone(other._unknownFields);
			}

			[DebuggerNonUserCode]
			public SignInAwardResp_1002019 Clone()
			{
				return new SignInAwardResp_1002019(this);
			}

			[DebuggerNonUserCode]
			public override bool Equals(object other)
			{
				return Equals(other as SignInAwardResp_1002019);
			}

			[DebuggerNonUserCode]
			public bool Equals(SignInAwardResp_1002019 other)
			{
				if (other == null)
				{
					return false;
				}
				if (other == this)
				{
					return true;
				}
				if (SignInDay != other.SignInDay)
				{
					return false;
				}
				if (!award_.Equals(other.award_))
				{
					return false;
				}
				return object.Equals(_unknownFields, other._unknownFields);
			}

			[DebuggerNonUserCode]
			public override int GetHashCode()
			{
				int num = 1;
				if (SignInDay != 0)
				{
					num ^= SignInDay.GetHashCode();
				}
				num ^= award_.GetHashCode();
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
				if (SignInDay != 0)
				{
					output.WriteRawTag(8);
					output.WriteInt32(SignInDay);
				}
				award_.WriteTo(output, _repeated_award_codec);
				if (_unknownFields != null)
				{
					_unknownFields.WriteTo(output);
				}
			}

			[DebuggerNonUserCode]
			public int CalculateSize()
			{
				int num = 0;
				if (SignInDay != 0)
				{
					num += 1 + CodedOutputStream.ComputeInt32Size(SignInDay);
				}
				num += award_.CalculateSize(_repeated_award_codec);
				if (_unknownFields != null)
				{
					num += _unknownFields.CalculateSize();
				}
				return num;
			}

			[DebuggerNonUserCode]
			public void MergeFrom(SignInAwardResp_1002019 other)
			{
				if (other != null)
				{
					if (other.SignInDay != 0)
					{
						SignInDay = other.SignInDay;
					}
					award_.Add(other.award_);
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
						SignInDay = input.ReadInt32();
						break;
					case 18u:
						award_.AddEntriesFrom(input, _repeated_award_codec);
						break;
					}
				}
			}
		}
	}

	private static readonly MessageParser<SearchSignInAwardResp_1002018> _parser = new MessageParser<SearchSignInAwardResp_1002018>(() => new SearchSignInAwardResp_1002018());

	private UnknownFieldSet _unknownFields;

	public const int CodeFieldNumber = 1;

	private int code_;

	public const int IsSuccessFieldNumber = 2;

	private bool isSuccess_;

	public const int SignInAwardFieldNumber = 3;

	private static readonly FieldCodec<Types.SignInAwardResp_1002019> _repeated_signInAward_codec = FieldCodec.ForMessage(26u, Types.SignInAwardResp_1002019.Parser);

	private readonly RepeatedField<Types.SignInAwardResp_1002019> signInAward_ = new RepeatedField<Types.SignInAwardResp_1002019>();

	[DebuggerNonUserCode]
	public static MessageParser<SearchSignInAwardResp_1002018> Parser => _parser;

	[DebuggerNonUserCode]
	public static MessageDescriptor Descriptor => RoleProtoReflection.Descriptor.MessageTypes[22];

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
	public RepeatedField<Types.SignInAwardResp_1002019> SignInAward => signInAward_;

	private MessageDescriptor pb_003A_003AGoogle_002EProtobuf_002EIMessage_002Eget_Descriptor()
	{
		return Descriptor;
	}

	[DebuggerNonUserCode]
	public SearchSignInAwardResp_1002018()
	{
	}

	[DebuggerNonUserCode]
	public SearchSignInAwardResp_1002018(SearchSignInAwardResp_1002018 other)
		: this()
	{
		code_ = other.code_;
		isSuccess_ = other.isSuccess_;
		signInAward_ = other.signInAward_.Clone();
		_unknownFields = UnknownFieldSet.Clone(other._unknownFields);
	}

	[DebuggerNonUserCode]
	public SearchSignInAwardResp_1002018 Clone()
	{
		return new SearchSignInAwardResp_1002018(this);
	}

	[DebuggerNonUserCode]
	public override bool Equals(object other)
	{
		return Equals(other as SearchSignInAwardResp_1002018);
	}

	[DebuggerNonUserCode]
	public bool Equals(SearchSignInAwardResp_1002018 other)
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
		if (!signInAward_.Equals(other.signInAward_))
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
		num ^= signInAward_.GetHashCode();
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
		signInAward_.WriteTo(output, _repeated_signInAward_codec);
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
		num += signInAward_.CalculateSize(_repeated_signInAward_codec);
		if (_unknownFields != null)
		{
			num += _unknownFields.CalculateSize();
		}
		return num;
	}

	[DebuggerNonUserCode]
	public void MergeFrom(SearchSignInAwardResp_1002018 other)
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
			signInAward_.Add(other.signInAward_);
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
			case 26u:
				signInAward_.AddEntriesFrom(input, _repeated_signInAward_codec);
				break;
			}
		}
	}
}
