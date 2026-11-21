using System;
using System.Diagnostics;
using Google.Protobuf;
using Google.Protobuf.Collections;
using Google.Protobuf.Reflection;

public sealed class LetterQueryResp_1003001 : IMessage<LetterQueryResp_1003001>, IMessage, IEquatable<LetterQueryResp_1003001>, IDeepCloneable<LetterQueryResp_1003001>
{
	[DebuggerNonUserCode]
	public static class Types
	{
		public sealed class LetterInfoResp_1003002 : IMessage<LetterInfoResp_1003002>, IMessage, IEquatable<LetterInfoResp_1003002>, IDeepCloneable<LetterInfoResp_1003002>
		{
			[DebuggerNonUserCode]
			public static class Types
			{
				public sealed class LetterAwardResp_1003003 : IMessage<LetterAwardResp_1003003>, IMessage, IEquatable<LetterAwardResp_1003003>, IDeepCloneable<LetterAwardResp_1003003>
				{
					private static readonly MessageParser<LetterAwardResp_1003003> _parser = new MessageParser<LetterAwardResp_1003003>(() => new LetterAwardResp_1003003());

					private UnknownFieldSet _unknownFields;

					public const int IdFieldNumber = 1;

					private string id_ = "";

					public const int GoodsIdFieldNumber = 2;

					private string goodsId_ = "";

					public const int GoodsNameFieldNumber = 3;

					private string goodsName_ = "";

					public const int GoodsTypeFieldNumber = 4;

					private int goodsType_;

					public const int GoodsCountFieldNumber = 5;

					private long goodsCount_;

					public const int StatusFieldNumber = 6;

					private bool status_;

					public const int RankFieldNumber = 7;

					private int rank_;

					[DebuggerNonUserCode]
					public static MessageParser<LetterAwardResp_1003003> Parser => _parser;

					[DebuggerNonUserCode]
					public static MessageDescriptor Descriptor => LetterInfoResp_1003002.Descriptor.NestedTypes[0];

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
					public string GoodsId
					{
						get
						{
							return goodsId_;
						}
						set
						{
							goodsId_ = ProtoPreconditions.CheckNotNull(value, "value");
						}
					}

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
					public int GoodsType
					{
						get
						{
							return goodsType_;
						}
						set
						{
							goodsType_ = value;
						}
					}

					[DebuggerNonUserCode]
					public long GoodsCount
					{
						get
						{
							return goodsCount_;
						}
						set
						{
							goodsCount_ = value;
						}
					}

					[DebuggerNonUserCode]
					public bool Status
					{
						get
						{
							return status_;
						}
						set
						{
							status_ = value;
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
					public LetterAwardResp_1003003()
					{
					}

					[DebuggerNonUserCode]
					public LetterAwardResp_1003003(LetterAwardResp_1003003 other)
						: this()
					{
						id_ = other.id_;
						goodsId_ = other.goodsId_;
						goodsName_ = other.goodsName_;
						goodsType_ = other.goodsType_;
						goodsCount_ = other.goodsCount_;
						status_ = other.status_;
						rank_ = other.rank_;
						_unknownFields = UnknownFieldSet.Clone(other._unknownFields);
					}

					[DebuggerNonUserCode]
					public LetterAwardResp_1003003 Clone()
					{
						return new LetterAwardResp_1003003(this);
					}

					[DebuggerNonUserCode]
					public override bool Equals(object other)
					{
						return Equals(other as LetterAwardResp_1003003);
					}

					[DebuggerNonUserCode]
					public bool Equals(LetterAwardResp_1003003 other)
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
						if (GoodsId != other.GoodsId)
						{
							return false;
						}
						if (GoodsName != other.GoodsName)
						{
							return false;
						}
						if (GoodsType != other.GoodsType)
						{
							return false;
						}
						if (GoodsCount != other.GoodsCount)
						{
							return false;
						}
						if (Status != other.Status)
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
						if (Id.Length != 0)
						{
							num ^= Id.GetHashCode();
						}
						if (GoodsId.Length != 0)
						{
							num ^= GoodsId.GetHashCode();
						}
						if (GoodsName.Length != 0)
						{
							num ^= GoodsName.GetHashCode();
						}
						if (GoodsType != 0)
						{
							num ^= GoodsType.GetHashCode();
						}
						if (GoodsCount != 0L)
						{
							num ^= GoodsCount.GetHashCode();
						}
						if (Status)
						{
							num ^= Status.GetHashCode();
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
						if (Id.Length != 0)
						{
							output.WriteRawTag(10);
							output.WriteString(Id);
						}
						if (GoodsId.Length != 0)
						{
							output.WriteRawTag(18);
							output.WriteString(GoodsId);
						}
						if (GoodsName.Length != 0)
						{
							output.WriteRawTag(26);
							output.WriteString(GoodsName);
						}
						if (GoodsType != 0)
						{
							output.WriteRawTag(32);
							output.WriteInt32(GoodsType);
						}
						if (GoodsCount != 0L)
						{
							output.WriteRawTag(40);
							output.WriteInt64(GoodsCount);
						}
						if (Status)
						{
							output.WriteRawTag(48);
							output.WriteBool(Status);
						}
						if (Rank != 0)
						{
							output.WriteRawTag(56);
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
						if (Id.Length != 0)
						{
							num += 1 + CodedOutputStream.ComputeStringSize(Id);
						}
						if (GoodsId.Length != 0)
						{
							num += 1 + CodedOutputStream.ComputeStringSize(GoodsId);
						}
						if (GoodsName.Length != 0)
						{
							num += 1 + CodedOutputStream.ComputeStringSize(GoodsName);
						}
						if (GoodsType != 0)
						{
							num += 1 + CodedOutputStream.ComputeInt32Size(GoodsType);
						}
						if (GoodsCount != 0L)
						{
							num += 1 + CodedOutputStream.ComputeInt64Size(GoodsCount);
						}
						if (Status)
						{
							num += 2;
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
					public void MergeFrom(LetterAwardResp_1003003 other)
					{
						if (other != null)
						{
							if (other.Id.Length != 0)
							{
								Id = other.Id;
							}
							if (other.GoodsId.Length != 0)
							{
								GoodsId = other.GoodsId;
							}
							if (other.GoodsName.Length != 0)
							{
								GoodsName = other.GoodsName;
							}
							if (other.GoodsType != 0)
							{
								GoodsType = other.GoodsType;
							}
							if (other.GoodsCount != 0L)
							{
								GoodsCount = other.GoodsCount;
							}
							if (other.Status)
							{
								Status = other.Status;
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
								Id = input.ReadString();
								break;
							case 18u:
								GoodsId = input.ReadString();
								break;
							case 26u:
								GoodsName = input.ReadString();
								break;
							case 32u:
								GoodsType = input.ReadInt32();
								break;
							case 40u:
								GoodsCount = input.ReadInt64();
								break;
							case 48u:
								Status = input.ReadBool();
								break;
							case 56u:
								Rank = input.ReadInt32();
								break;
							}
						}
					}
				}
			}

			private static readonly MessageParser<LetterInfoResp_1003002> _parser = new MessageParser<LetterInfoResp_1003002>(() => new LetterInfoResp_1003002());

			private UnknownFieldSet _unknownFields;

			public const int IdFieldNumber = 1;

			private string id_ = "";

			public const int ContentFieldNumber = 2;

			private string content_ = "";

			public const int HasAwardFieldNumber = 3;

			private bool hasAward_;

			public const int StatusFieldNumber = 4;

			private bool status_;

			public const int TitleFieldNumber = 5;

			private string title_ = "";

			public const int AwardsFieldNumber = 6;

			private static readonly FieldCodec<Types.LetterAwardResp_1003003> _repeated_awards_codec = FieldCodec.ForMessage(50u, Types.LetterAwardResp_1003003.Parser);

			private readonly RepeatedField<Types.LetterAwardResp_1003003> awards_ = new RepeatedField<Types.LetterAwardResp_1003003>();

			public const int IsGetAllFieldNumber = 7;

			private bool isGetAll_;

			[DebuggerNonUserCode]
			public static MessageParser<LetterInfoResp_1003002> Parser => _parser;

			[DebuggerNonUserCode]
			public static MessageDescriptor Descriptor => LetterQueryResp_1003001.Descriptor.NestedTypes[0];

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
			public string Content
			{
				get
				{
					return content_;
				}
				set
				{
					content_ = ProtoPreconditions.CheckNotNull(value, "value");
				}
			}

			[DebuggerNonUserCode]
			public bool HasAward
			{
				get
				{
					return hasAward_;
				}
				set
				{
					hasAward_ = value;
				}
			}

			[DebuggerNonUserCode]
			public bool Status
			{
				get
				{
					return status_;
				}
				set
				{
					status_ = value;
				}
			}

			[DebuggerNonUserCode]
			public string Title
			{
				get
				{
					return title_;
				}
				set
				{
					title_ = ProtoPreconditions.CheckNotNull(value, "value");
				}
			}

			[DebuggerNonUserCode]
			public RepeatedField<Types.LetterAwardResp_1003003> Awards => awards_;

			[DebuggerNonUserCode]
			public bool IsGetAll
			{
				get
				{
					return isGetAll_;
				}
				set
				{
					isGetAll_ = value;
				}
			}

			private MessageDescriptor pb_003A_003AGoogle_002EProtobuf_002EIMessage_002Eget_Descriptor()
			{
				return Descriptor;
			}

			[DebuggerNonUserCode]
			public LetterInfoResp_1003002()
			{
			}

			[DebuggerNonUserCode]
			public LetterInfoResp_1003002(LetterInfoResp_1003002 other)
				: this()
			{
				id_ = other.id_;
				content_ = other.content_;
				hasAward_ = other.hasAward_;
				status_ = other.status_;
				title_ = other.title_;
				awards_ = other.awards_.Clone();
				isGetAll_ = other.isGetAll_;
				_unknownFields = UnknownFieldSet.Clone(other._unknownFields);
			}

			[DebuggerNonUserCode]
			public LetterInfoResp_1003002 Clone()
			{
				return new LetterInfoResp_1003002(this);
			}

			[DebuggerNonUserCode]
			public override bool Equals(object other)
			{
				return Equals(other as LetterInfoResp_1003002);
			}

			[DebuggerNonUserCode]
			public bool Equals(LetterInfoResp_1003002 other)
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
				if (Content != other.Content)
				{
					return false;
				}
				if (HasAward != other.HasAward)
				{
					return false;
				}
				if (Status != other.Status)
				{
					return false;
				}
				if (Title != other.Title)
				{
					return false;
				}
				if (!awards_.Equals(other.awards_))
				{
					return false;
				}
				if (IsGetAll != other.IsGetAll)
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
				if (Content.Length != 0)
				{
					num ^= Content.GetHashCode();
				}
				if (HasAward)
				{
					num ^= HasAward.GetHashCode();
				}
				if (Status)
				{
					num ^= Status.GetHashCode();
				}
				if (Title.Length != 0)
				{
					num ^= Title.GetHashCode();
				}
				num ^= awards_.GetHashCode();
				if (IsGetAll)
				{
					num ^= IsGetAll.GetHashCode();
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
				if (Content.Length != 0)
				{
					output.WriteRawTag(18);
					output.WriteString(Content);
				}
				if (HasAward)
				{
					output.WriteRawTag(24);
					output.WriteBool(HasAward);
				}
				if (Status)
				{
					output.WriteRawTag(32);
					output.WriteBool(Status);
				}
				if (Title.Length != 0)
				{
					output.WriteRawTag(42);
					output.WriteString(Title);
				}
				awards_.WriteTo(output, _repeated_awards_codec);
				if (IsGetAll)
				{
					output.WriteRawTag(56);
					output.WriteBool(IsGetAll);
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
				if (Content.Length != 0)
				{
					num += 1 + CodedOutputStream.ComputeStringSize(Content);
				}
				if (HasAward)
				{
					num += 2;
				}
				if (Status)
				{
					num += 2;
				}
				if (Title.Length != 0)
				{
					num += 1 + CodedOutputStream.ComputeStringSize(Title);
				}
				num += awards_.CalculateSize(_repeated_awards_codec);
				if (IsGetAll)
				{
					num += 2;
				}
				if (_unknownFields != null)
				{
					num += _unknownFields.CalculateSize();
				}
				return num;
			}

			[DebuggerNonUserCode]
			public void MergeFrom(LetterInfoResp_1003002 other)
			{
				if (other != null)
				{
					if (other.Id.Length != 0)
					{
						Id = other.Id;
					}
					if (other.Content.Length != 0)
					{
						Content = other.Content;
					}
					if (other.HasAward)
					{
						HasAward = other.HasAward;
					}
					if (other.Status)
					{
						Status = other.Status;
					}
					if (other.Title.Length != 0)
					{
						Title = other.Title;
					}
					awards_.Add(other.awards_);
					if (other.IsGetAll)
					{
						IsGetAll = other.IsGetAll;
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
						Content = input.ReadString();
						break;
					case 24u:
						HasAward = input.ReadBool();
						break;
					case 32u:
						Status = input.ReadBool();
						break;
					case 42u:
						Title = input.ReadString();
						break;
					case 50u:
						awards_.AddEntriesFrom(input, _repeated_awards_codec);
						break;
					case 56u:
						IsGetAll = input.ReadBool();
						break;
					}
				}
			}
		}
	}

	private static readonly MessageParser<LetterQueryResp_1003001> _parser = new MessageParser<LetterQueryResp_1003001>(() => new LetterQueryResp_1003001());

	private UnknownFieldSet _unknownFields;

	public const int CodeFieldNumber = 1;

	private int code_;

	public const int IsSuccessFieldNumber = 2;

	private bool isSuccess_;

	public const int LettersFieldNumber = 3;

	private static readonly FieldCodec<Types.LetterInfoResp_1003002> _repeated_letters_codec = FieldCodec.ForMessage(26u, Types.LetterInfoResp_1003002.Parser);

	private readonly RepeatedField<Types.LetterInfoResp_1003002> letters_ = new RepeatedField<Types.LetterInfoResp_1003002>();

	[DebuggerNonUserCode]
	public static MessageParser<LetterQueryResp_1003001> Parser => _parser;

	[DebuggerNonUserCode]
	public static MessageDescriptor Descriptor => LetterProtoReflection.Descriptor.MessageTypes[0];

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
	public RepeatedField<Types.LetterInfoResp_1003002> Letters => letters_;

	private MessageDescriptor pb_003A_003AGoogle_002EProtobuf_002EIMessage_002Eget_Descriptor()
	{
		return Descriptor;
	}

	[DebuggerNonUserCode]
	public LetterQueryResp_1003001()
	{
	}

	[DebuggerNonUserCode]
	public LetterQueryResp_1003001(LetterQueryResp_1003001 other)
		: this()
	{
		code_ = other.code_;
		isSuccess_ = other.isSuccess_;
		letters_ = other.letters_.Clone();
		_unknownFields = UnknownFieldSet.Clone(other._unknownFields);
	}

	[DebuggerNonUserCode]
	public LetterQueryResp_1003001 Clone()
	{
		return new LetterQueryResp_1003001(this);
	}

	[DebuggerNonUserCode]
	public override bool Equals(object other)
	{
		return Equals(other as LetterQueryResp_1003001);
	}

	[DebuggerNonUserCode]
	public bool Equals(LetterQueryResp_1003001 other)
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
		if (!letters_.Equals(other.letters_))
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
		num ^= letters_.GetHashCode();
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
		letters_.WriteTo(output, _repeated_letters_codec);
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
		num += letters_.CalculateSize(_repeated_letters_codec);
		if (_unknownFields != null)
		{
			num += _unknownFields.CalculateSize();
		}
		return num;
	}

	[DebuggerNonUserCode]
	public void MergeFrom(LetterQueryResp_1003001 other)
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
			letters_.Add(other.letters_);
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
				letters_.AddEntriesFrom(input, _repeated_letters_codec);
				break;
			}
		}
	}
}
