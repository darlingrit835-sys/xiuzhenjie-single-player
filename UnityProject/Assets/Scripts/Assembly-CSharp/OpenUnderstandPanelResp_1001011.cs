using System;
using System.Diagnostics;
using Google.Protobuf;
using Google.Protobuf.Collections;
using Google.Protobuf.Reflection;

public sealed class OpenUnderstandPanelResp_1001011 : IMessage<OpenUnderstandPanelResp_1001011>, IMessage, IEquatable<OpenUnderstandPanelResp_1001011>, IDeepCloneable<OpenUnderstandPanelResp_1001011>
{
	[DebuggerNonUserCode]
	public static class Types
	{
		public sealed class UnderstandMethodObjectResp_1001012 : IMessage<UnderstandMethodObjectResp_1001012>, IMessage, IEquatable<UnderstandMethodObjectResp_1001012>, IDeepCloneable<UnderstandMethodObjectResp_1001012>
		{
			private static readonly MessageParser<UnderstandMethodObjectResp_1001012> _parser = new MessageParser<UnderstandMethodObjectResp_1001012>(() => new UnderstandMethodObjectResp_1001012());

			private UnknownFieldSet _unknownFields;

			public const int IsUnderstandFieldNumber = 1;

			private bool isUnderstand_;

			public const int NameFieldNumber = 2;

			private string name_ = "";

			public const int FiveElementFieldNumber = 3;

			private int fiveElement_;

			public const int LevelFieldNumber = 4;

			private int level_;

			public const int RankFieldNumber = 5;

			private int rank_;

			public const int TypeFieldNumber = 6;

			private int type_;

			public const int IdFieldNumber = 7;

			private string id_ = "";

			[DebuggerNonUserCode]
			public static MessageParser<UnderstandMethodObjectResp_1001012> Parser => _parser;

			[DebuggerNonUserCode]
			public static MessageDescriptor Descriptor => OpenUnderstandPanelResp_1001011.Descriptor.NestedTypes[0];

			[DebuggerNonUserCode]
			MessageDescriptor IMessage.Descriptor => pb_003A_003AGoogle_002EProtobuf_002EIMessage_002Eget_Descriptor();

			[DebuggerNonUserCode]
			public bool IsUnderstand
			{
				get
				{
					return isUnderstand_;
				}
				set
				{
					isUnderstand_ = value;
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

			private MessageDescriptor pb_003A_003AGoogle_002EProtobuf_002EIMessage_002Eget_Descriptor()
			{
				return Descriptor;
			}

			[DebuggerNonUserCode]
			public UnderstandMethodObjectResp_1001012()
			{
			}

			[DebuggerNonUserCode]
			public UnderstandMethodObjectResp_1001012(UnderstandMethodObjectResp_1001012 other)
				: this()
			{
				isUnderstand_ = other.isUnderstand_;
				name_ = other.name_;
				fiveElement_ = other.fiveElement_;
				level_ = other.level_;
				rank_ = other.rank_;
				type_ = other.type_;
				id_ = other.id_;
				_unknownFields = UnknownFieldSet.Clone(other._unknownFields);
			}

			[DebuggerNonUserCode]
			public UnderstandMethodObjectResp_1001012 Clone()
			{
				return new UnderstandMethodObjectResp_1001012(this);
			}

			[DebuggerNonUserCode]
			public override bool Equals(object other)
			{
				return Equals(other as UnderstandMethodObjectResp_1001012);
			}

			[DebuggerNonUserCode]
			public bool Equals(UnderstandMethodObjectResp_1001012 other)
			{
				if (other == null)
				{
					return false;
				}
				if (other == this)
				{
					return true;
				}
				if (IsUnderstand != other.IsUnderstand)
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
				if (Level != other.Level)
				{
					return false;
				}
				if (Rank != other.Rank)
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
				return object.Equals(_unknownFields, other._unknownFields);
			}

			[DebuggerNonUserCode]
			public override int GetHashCode()
			{
				int num = 1;
				if (IsUnderstand)
				{
					num ^= IsUnderstand.GetHashCode();
				}
				if (Name.Length != 0)
				{
					num ^= Name.GetHashCode();
				}
				if (FiveElement != 0)
				{
					num ^= FiveElement.GetHashCode();
				}
				if (Level != 0)
				{
					num ^= Level.GetHashCode();
				}
				if (Rank != 0)
				{
					num ^= Rank.GetHashCode();
				}
				if (Type != 0)
				{
					num ^= Type.GetHashCode();
				}
				if (Id.Length != 0)
				{
					num ^= Id.GetHashCode();
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
				if (IsUnderstand)
				{
					output.WriteRawTag(8);
					output.WriteBool(IsUnderstand);
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
				if (Level != 0)
				{
					output.WriteRawTag(32);
					output.WriteInt32(Level);
				}
				if (Rank != 0)
				{
					output.WriteRawTag(40);
					output.WriteInt32(Rank);
				}
				if (Type != 0)
				{
					output.WriteRawTag(48);
					output.WriteInt32(Type);
				}
				if (Id.Length != 0)
				{
					output.WriteRawTag(58);
					output.WriteString(Id);
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
				if (IsUnderstand)
				{
					num += 2;
				}
				if (Name.Length != 0)
				{
					num += 1 + CodedOutputStream.ComputeStringSize(Name);
				}
				if (FiveElement != 0)
				{
					num += 1 + CodedOutputStream.ComputeInt32Size(FiveElement);
				}
				if (Level != 0)
				{
					num += 1 + CodedOutputStream.ComputeInt32Size(Level);
				}
				if (Rank != 0)
				{
					num += 1 + CodedOutputStream.ComputeInt32Size(Rank);
				}
				if (Type != 0)
				{
					num += 1 + CodedOutputStream.ComputeInt32Size(Type);
				}
				if (Id.Length != 0)
				{
					num += 1 + CodedOutputStream.ComputeStringSize(Id);
				}
				if (_unknownFields != null)
				{
					num += _unknownFields.CalculateSize();
				}
				return num;
			}

			[DebuggerNonUserCode]
			public void MergeFrom(UnderstandMethodObjectResp_1001012 other)
			{
				if (other != null)
				{
					if (other.IsUnderstand)
					{
						IsUnderstand = other.IsUnderstand;
					}
					if (other.Name.Length != 0)
					{
						Name = other.Name;
					}
					if (other.FiveElement != 0)
					{
						FiveElement = other.FiveElement;
					}
					if (other.Level != 0)
					{
						Level = other.Level;
					}
					if (other.Rank != 0)
					{
						Rank = other.Rank;
					}
					if (other.Type != 0)
					{
						Type = other.Type;
					}
					if (other.Id.Length != 0)
					{
						Id = other.Id;
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
						IsUnderstand = input.ReadBool();
						break;
					case 18u:
						Name = input.ReadString();
						break;
					case 24u:
						FiveElement = input.ReadInt32();
						break;
					case 32u:
						Level = input.ReadInt32();
						break;
					case 40u:
						Rank = input.ReadInt32();
						break;
					case 48u:
						Type = input.ReadInt32();
						break;
					case 58u:
						Id = input.ReadString();
						break;
					}
				}
			}
		}
	}

	private static readonly MessageParser<OpenUnderstandPanelResp_1001011> _parser = new MessageParser<OpenUnderstandPanelResp_1001011>(() => new OpenUnderstandPanelResp_1001011());

	private UnknownFieldSet _unknownFields;

	public const int CodeFieldNumber = 1;

	private int code_;

	public const int IsSuccessFieldNumber = 2;

	private bool isSuccess_;

	public const int ObjectFieldNumber = 3;

	private static readonly FieldCodec<Types.UnderstandMethodObjectResp_1001012> _repeated_object_codec = FieldCodec.ForMessage(26u, Types.UnderstandMethodObjectResp_1001012.Parser);

	private readonly RepeatedField<Types.UnderstandMethodObjectResp_1001012> object_ = new RepeatedField<Types.UnderstandMethodObjectResp_1001012>();

	[DebuggerNonUserCode]
	public static MessageParser<OpenUnderstandPanelResp_1001011> Parser => _parser;

	[DebuggerNonUserCode]
	public static MessageDescriptor Descriptor => SimpleActionProtoReflection.Descriptor.MessageTypes[13];

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
	public RepeatedField<Types.UnderstandMethodObjectResp_1001012> Object => object_;

	private MessageDescriptor pb_003A_003AGoogle_002EProtobuf_002EIMessage_002Eget_Descriptor()
	{
		return Descriptor;
	}

	[DebuggerNonUserCode]
	public OpenUnderstandPanelResp_1001011()
	{
	}

	[DebuggerNonUserCode]
	public OpenUnderstandPanelResp_1001011(OpenUnderstandPanelResp_1001011 other)
		: this()
	{
		code_ = other.code_;
		isSuccess_ = other.isSuccess_;
		object_ = other.object_.Clone();
		_unknownFields = UnknownFieldSet.Clone(other._unknownFields);
	}

	[DebuggerNonUserCode]
	public OpenUnderstandPanelResp_1001011 Clone()
	{
		return new OpenUnderstandPanelResp_1001011(this);
	}

	[DebuggerNonUserCode]
	public override bool Equals(object other)
	{
		return Equals(other as OpenUnderstandPanelResp_1001011);
	}

	[DebuggerNonUserCode]
	public bool Equals(OpenUnderstandPanelResp_1001011 other)
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
		num += object_.CalculateSize(_repeated_object_codec);
		if (_unknownFields != null)
		{
			num += _unknownFields.CalculateSize();
		}
		return num;
	}

	[DebuggerNonUserCode]
	public void MergeFrom(OpenUnderstandPanelResp_1001011 other)
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
			case 26u:
				object_.AddEntriesFrom(input, _repeated_object_codec);
				break;
			}
		}
	}
}
