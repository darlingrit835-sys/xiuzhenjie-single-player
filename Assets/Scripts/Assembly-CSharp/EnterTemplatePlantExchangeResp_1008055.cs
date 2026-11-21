using System;
using System.Diagnostics;
using Google.Protobuf;
using Google.Protobuf.Collections;
using Google.Protobuf.Reflection;

public sealed class EnterTemplatePlantExchangeResp_1008055 : IMessage<EnterTemplatePlantExchangeResp_1008055>, IMessage, IEquatable<EnterTemplatePlantExchangeResp_1008055>, IDeepCloneable<EnterTemplatePlantExchangeResp_1008055>
{
	[DebuggerNonUserCode]
	public static class Types
	{
		public sealed class ExchangeTemplatePlantObjectResp_1008056 : IMessage<ExchangeTemplatePlantObjectResp_1008056>, IMessage, IEquatable<ExchangeTemplatePlantObjectResp_1008056>, IDeepCloneable<ExchangeTemplatePlantObjectResp_1008056>
		{
			private static readonly MessageParser<ExchangeTemplatePlantObjectResp_1008056> _parser = new MessageParser<ExchangeTemplatePlantObjectResp_1008056>(() => new ExchangeTemplatePlantObjectResp_1008056());

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

			public const int ExchangeContributionFieldNumber = 6;

			private int exchangeContribution_;

			public const int CreatedFieldNumber = 7;

			private string created_ = "";

			[DebuggerNonUserCode]
			public static MessageParser<ExchangeTemplatePlantObjectResp_1008056> Parser => _parser;

			[DebuggerNonUserCode]
			public static MessageDescriptor Descriptor => EnterTemplatePlantExchangeResp_1008055.Descriptor.NestedTypes[0];

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

			[DebuggerNonUserCode]
			public int ExchangeContribution
			{
				get
				{
					return exchangeContribution_;
				}
				set
				{
					exchangeContribution_ = value;
				}
			}

			[DebuggerNonUserCode]
			public string Created
			{
				get
				{
					return created_;
				}
				set
				{
					created_ = ProtoPreconditions.CheckNotNull(value, "value");
				}
			}

			private MessageDescriptor pb_003A_003AGoogle_002EProtobuf_002EIMessage_002Eget_Descriptor()
			{
				return Descriptor;
			}

			[DebuggerNonUserCode]
			public ExchangeTemplatePlantObjectResp_1008056()
			{
			}

			[DebuggerNonUserCode]
			public ExchangeTemplatePlantObjectResp_1008056(ExchangeTemplatePlantObjectResp_1008056 other)
				: this()
			{
				id_ = other.id_;
				name_ = other.name_;
				fiveElement_ = other.fiveElement_;
				rank_ = other.rank_;
				level_ = other.level_;
				exchangeContribution_ = other.exchangeContribution_;
				created_ = other.created_;
				_unknownFields = UnknownFieldSet.Clone(other._unknownFields);
			}

			[DebuggerNonUserCode]
			public ExchangeTemplatePlantObjectResp_1008056 Clone()
			{
				return new ExchangeTemplatePlantObjectResp_1008056(this);
			}

			[DebuggerNonUserCode]
			public override bool Equals(object other)
			{
				return Equals(other as ExchangeTemplatePlantObjectResp_1008056);
			}

			[DebuggerNonUserCode]
			public bool Equals(ExchangeTemplatePlantObjectResp_1008056 other)
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
				if (ExchangeContribution != other.ExchangeContribution)
				{
					return false;
				}
				if (Created != other.Created)
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
				if (ExchangeContribution != 0)
				{
					num ^= ExchangeContribution.GetHashCode();
				}
				if (Created.Length != 0)
				{
					num ^= Created.GetHashCode();
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
				if (ExchangeContribution != 0)
				{
					output.WriteRawTag(48);
					output.WriteInt32(ExchangeContribution);
				}
				if (Created.Length != 0)
				{
					output.WriteRawTag(58);
					output.WriteString(Created);
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
				if (ExchangeContribution != 0)
				{
					num += 1 + CodedOutputStream.ComputeInt32Size(ExchangeContribution);
				}
				if (Created.Length != 0)
				{
					num += 1 + CodedOutputStream.ComputeStringSize(Created);
				}
				if (_unknownFields != null)
				{
					num += _unknownFields.CalculateSize();
				}
				return num;
			}

			[DebuggerNonUserCode]
			public void MergeFrom(ExchangeTemplatePlantObjectResp_1008056 other)
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
					if (other.ExchangeContribution != 0)
					{
						ExchangeContribution = other.ExchangeContribution;
					}
					if (other.Created.Length != 0)
					{
						Created = other.Created;
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
					case 48u:
						ExchangeContribution = input.ReadInt32();
						break;
					case 58u:
						Created = input.ReadString();
						break;
					}
				}
			}
		}
	}

	private static readonly MessageParser<EnterTemplatePlantExchangeResp_1008055> _parser = new MessageParser<EnterTemplatePlantExchangeResp_1008055>(() => new EnterTemplatePlantExchangeResp_1008055());

	private UnknownFieldSet _unknownFields;

	public const int CodeFieldNumber = 1;

	private int code_;

	public const int IsSuccessFieldNumber = 2;

	private bool isSuccess_;

	public const int PageNumFieldNumber = 3;

	private int pageNum_;

	public const int PageSizeFieldNumber = 4;

	private int pageSize_;

	public const int PagesFieldNumber = 5;

	private int pages_;

	public const int ObjectFieldNumber = 6;

	private static readonly FieldCodec<Types.ExchangeTemplatePlantObjectResp_1008056> _repeated_object_codec = FieldCodec.ForMessage(50u, Types.ExchangeTemplatePlantObjectResp_1008056.Parser);

	private readonly RepeatedField<Types.ExchangeTemplatePlantObjectResp_1008056> object_ = new RepeatedField<Types.ExchangeTemplatePlantObjectResp_1008056>();

	[DebuggerNonUserCode]
	public static MessageParser<EnterTemplatePlantExchangeResp_1008055> Parser => _parser;

	[DebuggerNonUserCode]
	public static MessageDescriptor Descriptor => UnrealWorldProtoReflection.Descriptor.MessageTypes[69];

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
	public int PageNum
	{
		get
		{
			return pageNum_;
		}
		set
		{
			pageNum_ = value;
		}
	}

	[DebuggerNonUserCode]
	public int PageSize
	{
		get
		{
			return pageSize_;
		}
		set
		{
			pageSize_ = value;
		}
	}

	[DebuggerNonUserCode]
	public int Pages
	{
		get
		{
			return pages_;
		}
		set
		{
			pages_ = value;
		}
	}

	[DebuggerNonUserCode]
	public RepeatedField<Types.ExchangeTemplatePlantObjectResp_1008056> Object => object_;

	private MessageDescriptor pb_003A_003AGoogle_002EProtobuf_002EIMessage_002Eget_Descriptor()
	{
		return Descriptor;
	}

	[DebuggerNonUserCode]
	public EnterTemplatePlantExchangeResp_1008055()
	{
	}

	[DebuggerNonUserCode]
	public EnterTemplatePlantExchangeResp_1008055(EnterTemplatePlantExchangeResp_1008055 other)
		: this()
	{
		code_ = other.code_;
		isSuccess_ = other.isSuccess_;
		pageNum_ = other.pageNum_;
		pageSize_ = other.pageSize_;
		pages_ = other.pages_;
		object_ = other.object_.Clone();
		_unknownFields = UnknownFieldSet.Clone(other._unknownFields);
	}

	[DebuggerNonUserCode]
	public EnterTemplatePlantExchangeResp_1008055 Clone()
	{
		return new EnterTemplatePlantExchangeResp_1008055(this);
	}

	[DebuggerNonUserCode]
	public override bool Equals(object other)
	{
		return Equals(other as EnterTemplatePlantExchangeResp_1008055);
	}

	[DebuggerNonUserCode]
	public bool Equals(EnterTemplatePlantExchangeResp_1008055 other)
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
		if (PageNum != other.PageNum)
		{
			return false;
		}
		if (PageSize != other.PageSize)
		{
			return false;
		}
		if (Pages != other.Pages)
		{
			return false;
		}
		if (!object_.Equals(other.object_))
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
		if (PageNum != 0)
		{
			num ^= PageNum.GetHashCode();
		}
		if (PageSize != 0)
		{
			num ^= PageSize.GetHashCode();
		}
		if (Pages != 0)
		{
			num ^= Pages.GetHashCode();
		}
		num ^= object_.GetHashCode();
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
		if (PageNum != 0)
		{
			output.WriteRawTag(24);
			output.WriteInt32(PageNum);
		}
		if (PageSize != 0)
		{
			output.WriteRawTag(32);
			output.WriteInt32(PageSize);
		}
		if (Pages != 0)
		{
			output.WriteRawTag(40);
			output.WriteInt32(Pages);
		}
		object_.WriteTo(output, _repeated_object_codec);
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
		if (PageNum != 0)
		{
			num += 1 + CodedOutputStream.ComputeInt32Size(PageNum);
		}
		if (PageSize != 0)
		{
			num += 1 + CodedOutputStream.ComputeInt32Size(PageSize);
		}
		if (Pages != 0)
		{
			num += 1 + CodedOutputStream.ComputeInt32Size(Pages);
		}
		num += object_.CalculateSize(_repeated_object_codec);
		if (_unknownFields != null)
		{
			num += _unknownFields.CalculateSize();
		}
		return num;
	}

	[DebuggerNonUserCode]
	public void MergeFrom(EnterTemplatePlantExchangeResp_1008055 other)
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
			if (other.PageNum != 0)
			{
				PageNum = other.PageNum;
			}
			if (other.PageSize != 0)
			{
				PageSize = other.PageSize;
			}
			if (other.Pages != 0)
			{
				Pages = other.Pages;
			}
			object_.Add(other.object_);
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
				PageNum = input.ReadInt32();
				break;
			case 32u:
				PageSize = input.ReadInt32();
				break;
			case 40u:
				Pages = input.ReadInt32();
				break;
			case 50u:
				object_.AddEntriesFrom(input, _repeated_object_codec);
				break;
			}
		}
	}
}
