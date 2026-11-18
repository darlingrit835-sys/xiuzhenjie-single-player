using System;
using System.Diagnostics;
using Google.Protobuf;
using Google.Protobuf.Collections;
using Google.Protobuf.Reflection;

public sealed class GuildChouJiangResp_1015156 : IMessage<GuildChouJiangResp_1015156>, IMessage, IEquatable<GuildChouJiangResp_1015156>, IDeepCloneable<GuildChouJiangResp_1015156>
{
	[DebuggerNonUserCode]
	public static class Types
	{
		public sealed class GoodsInfoResp_1015157 : IMessage<GoodsInfoResp_1015157>, IMessage, IEquatable<GoodsInfoResp_1015157>, IDeepCloneable<GoodsInfoResp_1015157>
		{
			private static readonly MessageParser<GoodsInfoResp_1015157> _parser = new MessageParser<GoodsInfoResp_1015157>(() => new GoodsInfoResp_1015157());

			private UnknownFieldSet _unknownFields;

			public const int GoodsIdFieldNumber = 1;

			private string goodsId_ = "";

			public const int NameFieldNumber = 2;

			private string name_ = "";

			public const int FiveElementFieldNumber = 3;

			private int fiveElement_;

			public const int RankFieldNumber = 4;

			private int rank_;

			public const int LevelFieldNumber = 5;

			private int level_;

			public const int TypeFieldNumber = 6;

			private int type_;

			public const int IdFieldNumber = 7;

			private int id_;

			public const int CountFieldNumber = 8;

			private int count_;

			public const int TemplateIdFieldNumber = 9;

			private string templateId_ = "";

			[DebuggerNonUserCode]
			public static MessageParser<GoodsInfoResp_1015157> Parser => _parser;

			[DebuggerNonUserCode]
			public static MessageDescriptor Descriptor => GuildChouJiangResp_1015156.Descriptor.NestedTypes[0];

			[DebuggerNonUserCode]
			MessageDescriptor IMessage.Descriptor => pb_003A_003AGoogle_002EProtobuf_002EIMessage_002Eget_Descriptor();

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
			public int Id
			{
				get
				{
					return id_;
				}
				set
				{
					id_ = value;
				}
			}

			[DebuggerNonUserCode]
			public int Count
			{
				get
				{
					return count_;
				}
				set
				{
					count_ = value;
				}
			}

			[DebuggerNonUserCode]
			public string TemplateId
			{
				get
				{
					return templateId_;
				}
				set
				{
					templateId_ = ProtoPreconditions.CheckNotNull(value, "value");
				}
			}

			private MessageDescriptor pb_003A_003AGoogle_002EProtobuf_002EIMessage_002Eget_Descriptor()
			{
				return Descriptor;
			}

			[DebuggerNonUserCode]
			public GoodsInfoResp_1015157()
			{
			}

			[DebuggerNonUserCode]
			public GoodsInfoResp_1015157(GoodsInfoResp_1015157 other)
				: this()
			{
				goodsId_ = other.goodsId_;
				name_ = other.name_;
				fiveElement_ = other.fiveElement_;
				rank_ = other.rank_;
				level_ = other.level_;
				type_ = other.type_;
				id_ = other.id_;
				count_ = other.count_;
				templateId_ = other.templateId_;
				_unknownFields = UnknownFieldSet.Clone(other._unknownFields);
			}

			[DebuggerNonUserCode]
			public GoodsInfoResp_1015157 Clone()
			{
				return new GoodsInfoResp_1015157(this);
			}

			[DebuggerNonUserCode]
			public override bool Equals(object other)
			{
				return Equals(other as GoodsInfoResp_1015157);
			}

			[DebuggerNonUserCode]
			public bool Equals(GoodsInfoResp_1015157 other)
			{
				if (other == null)
				{
					return false;
				}
				if (other == this)
				{
					return true;
				}
				if (GoodsId != other.GoodsId)
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
				if (Type != other.Type)
				{
					return false;
				}
				if (Id != other.Id)
				{
					return false;
				}
				if (Count != other.Count)
				{
					return false;
				}
				if (TemplateId != other.TemplateId)
				{
					return false;
				}
				return object.Equals(_unknownFields, other._unknownFields);
			}

			[DebuggerNonUserCode]
			public override int GetHashCode()
			{
				int num = 1;
				if (GoodsId.Length != 0)
				{
					num ^= GoodsId.GetHashCode();
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
				if (Type != 0)
				{
					num ^= Type.GetHashCode();
				}
				if (Id != 0)
				{
					num ^= Id.GetHashCode();
				}
				if (Count != 0)
				{
					num ^= Count.GetHashCode();
				}
				if (TemplateId.Length != 0)
				{
					num ^= TemplateId.GetHashCode();
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
				if (GoodsId.Length != 0)
				{
					output.WriteRawTag(10);
					output.WriteString(GoodsId);
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
				if (Type != 0)
				{
					output.WriteRawTag(48);
					output.WriteInt32(Type);
				}
				if (Id != 0)
				{
					output.WriteRawTag(56);
					output.WriteInt32(Id);
				}
				if (Count != 0)
				{
					output.WriteRawTag(64);
					output.WriteInt32(Count);
				}
				if (TemplateId.Length != 0)
				{
					output.WriteRawTag(74);
					output.WriteString(TemplateId);
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
				if (GoodsId.Length != 0)
				{
					num += 1 + CodedOutputStream.ComputeStringSize(GoodsId);
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
				if (Type != 0)
				{
					num += 1 + CodedOutputStream.ComputeInt32Size(Type);
				}
				if (Id != 0)
				{
					num += 1 + CodedOutputStream.ComputeInt32Size(Id);
				}
				if (Count != 0)
				{
					num += 1 + CodedOutputStream.ComputeInt32Size(Count);
				}
				if (TemplateId.Length != 0)
				{
					num += 1 + CodedOutputStream.ComputeStringSize(TemplateId);
				}
				if (_unknownFields != null)
				{
					num += _unknownFields.CalculateSize();
				}
				return num;
			}

			[DebuggerNonUserCode]
			public void MergeFrom(GoodsInfoResp_1015157 other)
			{
				if (other != null)
				{
					if (other.GoodsId.Length != 0)
					{
						GoodsId = other.GoodsId;
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
					if (other.Type != 0)
					{
						Type = other.Type;
					}
					if (other.Id != 0)
					{
						Id = other.Id;
					}
					if (other.Count != 0)
					{
						Count = other.Count;
					}
					if (other.TemplateId.Length != 0)
					{
						TemplateId = other.TemplateId;
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
						GoodsId = input.ReadString();
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
						Type = input.ReadInt32();
						break;
					case 56u:
						Id = input.ReadInt32();
						break;
					case 64u:
						Count = input.ReadInt32();
						break;
					case 74u:
						TemplateId = input.ReadString();
						break;
					}
				}
			}
		}
	}

	private static readonly MessageParser<GuildChouJiangResp_1015156> _parser = new MessageParser<GuildChouJiangResp_1015156>(() => new GuildChouJiangResp_1015156());

	private UnknownFieldSet _unknownFields;

	public const int CodeFieldNumber = 1;

	private int code_;

	public const int IsSuccessFieldNumber = 2;

	private bool isSuccess_;

	public const int GoodsInfoFieldNumber = 3;

	private static readonly FieldCodec<Types.GoodsInfoResp_1015157> _repeated_goodsInfo_codec = FieldCodec.ForMessage(26u, Types.GoodsInfoResp_1015157.Parser);

	private readonly RepeatedField<Types.GoodsInfoResp_1015157> goodsInfo_ = new RepeatedField<Types.GoodsInfoResp_1015157>();

	public const int CountFieldNumber = 4;

	private int count_;

	[DebuggerNonUserCode]
	public static MessageParser<GuildChouJiangResp_1015156> Parser => _parser;

	[DebuggerNonUserCode]
	public static MessageDescriptor Descriptor => GuildProtoReflection.Descriptor.MessageTypes[199];

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
	public RepeatedField<Types.GoodsInfoResp_1015157> GoodsInfo => goodsInfo_;

	[DebuggerNonUserCode]
	public int Count
	{
		get
		{
			return count_;
		}
		set
		{
			count_ = value;
		}
	}

	private MessageDescriptor pb_003A_003AGoogle_002EProtobuf_002EIMessage_002Eget_Descriptor()
	{
		return Descriptor;
	}

	[DebuggerNonUserCode]
	public GuildChouJiangResp_1015156()
	{
	}

	[DebuggerNonUserCode]
	public GuildChouJiangResp_1015156(GuildChouJiangResp_1015156 other)
		: this()
	{
		code_ = other.code_;
		isSuccess_ = other.isSuccess_;
		goodsInfo_ = other.goodsInfo_.Clone();
		count_ = other.count_;
		_unknownFields = UnknownFieldSet.Clone(other._unknownFields);
	}

	[DebuggerNonUserCode]
	public GuildChouJiangResp_1015156 Clone()
	{
		return new GuildChouJiangResp_1015156(this);
	}

	[DebuggerNonUserCode]
	public override bool Equals(object other)
	{
		return Equals(other as GuildChouJiangResp_1015156);
	}

	[DebuggerNonUserCode]
	public bool Equals(GuildChouJiangResp_1015156 other)
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
		if (!goodsInfo_.Equals(other.goodsInfo_))
		{
			return false;
		}
		if (Count != other.Count)
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
		num ^= goodsInfo_.GetHashCode();
		if (Count != 0)
		{
			num ^= Count.GetHashCode();
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
		goodsInfo_.WriteTo(output, _repeated_goodsInfo_codec);
		if (Count != 0)
		{
			output.WriteRawTag(32);
			output.WriteInt32(Count);
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
		num += goodsInfo_.CalculateSize(_repeated_goodsInfo_codec);
		if (Count != 0)
		{
			num += 1 + CodedOutputStream.ComputeInt32Size(Count);
		}
		if (_unknownFields != null)
		{
			num += _unknownFields.CalculateSize();
		}
		return num;
	}

	[DebuggerNonUserCode]
	public void MergeFrom(GuildChouJiangResp_1015156 other)
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
			goodsInfo_.Add(other.goodsInfo_);
			if (other.Count != 0)
			{
				Count = other.Count;
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
			case 26u:
				goodsInfo_.AddEntriesFrom(input, _repeated_goodsInfo_codec);
				break;
			case 32u:
				Count = input.ReadInt32();
				break;
			}
		}
	}
}
