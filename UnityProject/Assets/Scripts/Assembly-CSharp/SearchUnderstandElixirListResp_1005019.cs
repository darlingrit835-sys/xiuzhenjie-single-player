using System;
using System.Diagnostics;
using Google.Protobuf;
using Google.Protobuf.Collections;
using Google.Protobuf.Reflection;

public sealed class SearchUnderstandElixirListResp_1005019 : IMessage<SearchUnderstandElixirListResp_1005019>, IMessage, IEquatable<SearchUnderstandElixirListResp_1005019>, IDeepCloneable<SearchUnderstandElixirListResp_1005019>
{
	[DebuggerNonUserCode]
	public static class Types
	{
		public sealed class UnderstandElixirResp_1005029 : IMessage<UnderstandElixirResp_1005029>, IMessage, IEquatable<UnderstandElixirResp_1005029>, IDeepCloneable<UnderstandElixirResp_1005029>
		{
			private static readonly MessageParser<UnderstandElixirResp_1005029> _parser = new MessageParser<UnderstandElixirResp_1005029>(() => new UnderstandElixirResp_1005029());

			private UnknownFieldSet _unknownFields;

			public const int IdFieldNumber = 1;

			private string id_ = "";

			public const int NameFieldNumber = 2;

			private string name_ = "";

			public const int FiveElementFieldNumber = 3;

			private int fiveElement_;

			public const int RankFieldNumber = 4;

			private int rank_;

			public const int LevelFieldNumber = 5;

			private int level_;

			[DebuggerNonUserCode]
			public static MessageParser<UnderstandElixirResp_1005029> Parser => _parser;

			[DebuggerNonUserCode]
			public static MessageDescriptor Descriptor => SearchUnderstandElixirListResp_1005019.Descriptor.NestedTypes[0];

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

			private MessageDescriptor pb_003A_003AGoogle_002EProtobuf_002EIMessage_002Eget_Descriptor()
			{
				return Descriptor;
			}

			[DebuggerNonUserCode]
			public UnderstandElixirResp_1005029()
			{
			}

			[DebuggerNonUserCode]
			public UnderstandElixirResp_1005029(UnderstandElixirResp_1005029 other)
				: this()
			{
				id_ = other.id_;
				name_ = other.name_;
				fiveElement_ = other.fiveElement_;
				rank_ = other.rank_;
				level_ = other.level_;
				_unknownFields = UnknownFieldSet.Clone(other._unknownFields);
			}

			[DebuggerNonUserCode]
			public UnderstandElixirResp_1005029 Clone()
			{
				return new UnderstandElixirResp_1005029(this);
			}

			[DebuggerNonUserCode]
			public override bool Equals(object other)
			{
				return Equals(other as UnderstandElixirResp_1005029);
			}

			[DebuggerNonUserCode]
			public bool Equals(UnderstandElixirResp_1005029 other)
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
				if (FiveElement != other.FiveElement)
				{
					return false;
				}
				if (Rank != other.Rank)
				{
					return false;
				}
				if (Level != other.Level)
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
				if (FiveElement != 0)
				{
					num ^= FiveElement.GetHashCode();
				}
				if (Rank != 0)
				{
					num ^= Rank.GetHashCode();
				}
				if (Level != 0)
				{
					num ^= Level.GetHashCode();
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
				if (FiveElement != 0)
				{
					output.WriteRawTag(24);
					output.WriteInt32(FiveElement);
				}
				if (Rank != 0)
				{
					output.WriteRawTag(32);
					output.WriteInt32(Rank);
				}
				if (Level != 0)
				{
					output.WriteRawTag(40);
					output.WriteInt32(Level);
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
				if (FiveElement != 0)
				{
					num += 1 + CodedOutputStream.ComputeInt32Size(FiveElement);
				}
				if (Rank != 0)
				{
					num += 1 + CodedOutputStream.ComputeInt32Size(Rank);
				}
				if (Level != 0)
				{
					num += 1 + CodedOutputStream.ComputeInt32Size(Level);
				}
				if (_unknownFields != null)
				{
					num += _unknownFields.CalculateSize();
				}
				return num;
			}

			[DebuggerNonUserCode]
			public void MergeFrom(UnderstandElixirResp_1005029 other)
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
					if (other.FiveElement != 0)
					{
						FiveElement = other.FiveElement;
					}
					if (other.Rank != 0)
					{
						Rank = other.Rank;
					}
					if (other.Level != 0)
					{
						Level = other.Level;
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
					case 24u:
						FiveElement = input.ReadInt32();
						break;
					case 32u:
						Rank = input.ReadInt32();
						break;
					case 40u:
						Level = input.ReadInt32();
						break;
					}
				}
			}
		}
	}

	private static readonly MessageParser<SearchUnderstandElixirListResp_1005019> _parser = new MessageParser<SearchUnderstandElixirListResp_1005019>(() => new SearchUnderstandElixirListResp_1005019());

	private UnknownFieldSet _unknownFields;

	public const int CodeFieldNumber = 1;

	private int code_;

	public const int IsSuccessFieldNumber = 2;

	private bool isSuccess_;

	public const int UnderstandElixirFieldNumber = 3;

	private static readonly FieldCodec<Types.UnderstandElixirResp_1005029> _repeated_understandElixir_codec = FieldCodec.ForMessage(26u, Types.UnderstandElixirResp_1005029.Parser);

	private readonly RepeatedField<Types.UnderstandElixirResp_1005029> understandElixir_ = new RepeatedField<Types.UnderstandElixirResp_1005029>();

	[DebuggerNonUserCode]
	public static MessageParser<SearchUnderstandElixirListResp_1005019> Parser => _parser;

	[DebuggerNonUserCode]
	public static MessageDescriptor Descriptor => HomeProtoReflection.Descriptor.MessageTypes[38];

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
	public RepeatedField<Types.UnderstandElixirResp_1005029> UnderstandElixir => understandElixir_;

	private MessageDescriptor pb_003A_003AGoogle_002EProtobuf_002EIMessage_002Eget_Descriptor()
	{
		return Descriptor;
	}

	[DebuggerNonUserCode]
	public SearchUnderstandElixirListResp_1005019()
	{
	}

	[DebuggerNonUserCode]
	public SearchUnderstandElixirListResp_1005019(SearchUnderstandElixirListResp_1005019 other)
		: this()
	{
		code_ = other.code_;
		isSuccess_ = other.isSuccess_;
		understandElixir_ = other.understandElixir_.Clone();
		_unknownFields = UnknownFieldSet.Clone(other._unknownFields);
	}

	[DebuggerNonUserCode]
	public SearchUnderstandElixirListResp_1005019 Clone()
	{
		return new SearchUnderstandElixirListResp_1005019(this);
	}

	[DebuggerNonUserCode]
	public override bool Equals(object other)
	{
		return Equals(other as SearchUnderstandElixirListResp_1005019);
	}

	[DebuggerNonUserCode]
	public bool Equals(SearchUnderstandElixirListResp_1005019 other)
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
		if (!understandElixir_.Equals(other.understandElixir_))
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
		num ^= understandElixir_.GetHashCode();
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
		understandElixir_.WriteTo(output, _repeated_understandElixir_codec);
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
		num += understandElixir_.CalculateSize(_repeated_understandElixir_codec);
		if (_unknownFields != null)
		{
			num += _unknownFields.CalculateSize();
		}
		return num;
	}

	[DebuggerNonUserCode]
	public void MergeFrom(SearchUnderstandElixirListResp_1005019 other)
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
			understandElixir_.Add(other.understandElixir_);
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
				understandElixir_.AddEntriesFrom(input, _repeated_understandElixir_codec);
				break;
			}
		}
	}
}
