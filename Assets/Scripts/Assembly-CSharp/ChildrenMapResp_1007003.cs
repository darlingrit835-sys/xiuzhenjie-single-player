using System;
using System.Diagnostics;
using Google.Protobuf;
using Google.Protobuf.Collections;
using Google.Protobuf.Reflection;

public sealed class ChildrenMapResp_1007003 : IMessage<ChildrenMapResp_1007003>, IMessage, IEquatable<ChildrenMapResp_1007003>, IDeepCloneable<ChildrenMapResp_1007003>
{
	[DebuggerNonUserCode]
	public static class Types
	{
		public sealed class MapObjectResp_1007004 : IMessage<MapObjectResp_1007004>, IMessage, IEquatable<MapObjectResp_1007004>, IDeepCloneable<MapObjectResp_1007004>
		{
			private static readonly MessageParser<MapObjectResp_1007004> _parser = new MessageParser<MapObjectResp_1007004>(() => new MapObjectResp_1007004());

			private UnknownFieldSet _unknownFields;

			public const int IdFieldNumber = 1;

			private string id_ = "";

			public const int ParentIdFieldNumber = 2;

			private string parentId_ = "";

			public const int ParentNameFieldNumber = 3;

			private string parentName_ = "";

			public const int NameFieldNumber = 4;

			private string name_ = "";

			public const int SibingsIdFieldNumber = 5;

			private string sibingsId_ = "";

			public const int ChildrenIdsFieldNumber = 6;

			private string childrenIds_ = "";

			public const int TypeFieldNumber = 7;

			private int type_;

			public const int MinLevelFieldNumber = 8;

			private int minLevel_;

			public const int MaxLevelFieldNumber = 9;

			private int maxLevel_;

			public const int StatusFieldNumber = 10;

			private int status_;

			public const int RestrictFieldNumber = 11;

			private string restrict_ = "";

			public const int BasedConcentrationFieldNumber = 12;

			private double basedConcentration_;

			public const int FiveElementsFieldNumber = 13;

			private int fiveElements_;

			public const int SortFieldNumber = 14;

			private int sort_;

			public const int LevelFieldNumber = 15;

			private int level_;

			[DebuggerNonUserCode]
			public static MessageParser<MapObjectResp_1007004> Parser => _parser;

			[DebuggerNonUserCode]
			public static MessageDescriptor Descriptor => ChildrenMapResp_1007003.Descriptor.NestedTypes[0];

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
			public string ParentId
			{
				get
				{
					return parentId_;
				}
				set
				{
					parentId_ = ProtoPreconditions.CheckNotNull(value, "value");
				}
			}

			[DebuggerNonUserCode]
			public string ParentName
			{
				get
				{
					return parentName_;
				}
				set
				{
					parentName_ = ProtoPreconditions.CheckNotNull(value, "value");
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
			public string SibingsId
			{
				get
				{
					return sibingsId_;
				}
				set
				{
					sibingsId_ = ProtoPreconditions.CheckNotNull(value, "value");
				}
			}

			[DebuggerNonUserCode]
			public string ChildrenIds
			{
				get
				{
					return childrenIds_;
				}
				set
				{
					childrenIds_ = ProtoPreconditions.CheckNotNull(value, "value");
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
			public int MinLevel
			{
				get
				{
					return minLevel_;
				}
				set
				{
					minLevel_ = value;
				}
			}

			[DebuggerNonUserCode]
			public int MaxLevel
			{
				get
				{
					return maxLevel_;
				}
				set
				{
					maxLevel_ = value;
				}
			}

			[DebuggerNonUserCode]
			public int Status
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
			public string Restrict
			{
				get
				{
					return restrict_;
				}
				set
				{
					restrict_ = ProtoPreconditions.CheckNotNull(value, "value");
				}
			}

			[DebuggerNonUserCode]
			public double BasedConcentration
			{
				get
				{
					return basedConcentration_;
				}
				set
				{
					basedConcentration_ = value;
				}
			}

			[DebuggerNonUserCode]
			public int FiveElements
			{
				get
				{
					return fiveElements_;
				}
				set
				{
					fiveElements_ = value;
				}
			}

			[DebuggerNonUserCode]
			public int Sort
			{
				get
				{
					return sort_;
				}
				set
				{
					sort_ = value;
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
			public MapObjectResp_1007004()
			{
			}

			[DebuggerNonUserCode]
			public MapObjectResp_1007004(MapObjectResp_1007004 other)
				: this()
			{
				id_ = other.id_;
				parentId_ = other.parentId_;
				parentName_ = other.parentName_;
				name_ = other.name_;
				sibingsId_ = other.sibingsId_;
				childrenIds_ = other.childrenIds_;
				type_ = other.type_;
				minLevel_ = other.minLevel_;
				maxLevel_ = other.maxLevel_;
				status_ = other.status_;
				restrict_ = other.restrict_;
				basedConcentration_ = other.basedConcentration_;
				fiveElements_ = other.fiveElements_;
				sort_ = other.sort_;
				level_ = other.level_;
				_unknownFields = UnknownFieldSet.Clone(other._unknownFields);
			}

			[DebuggerNonUserCode]
			public MapObjectResp_1007004 Clone()
			{
				return new MapObjectResp_1007004(this);
			}

			[DebuggerNonUserCode]
			public override bool Equals(object other)
			{
				return Equals(other as MapObjectResp_1007004);
			}

			[DebuggerNonUserCode]
			public bool Equals(MapObjectResp_1007004 other)
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
				if (ParentId != other.ParentId)
				{
					return false;
				}
				if (ParentName != other.ParentName)
				{
					return false;
				}
				if (Name != other.Name)
				{
					return false;
				}
				if (SibingsId != other.SibingsId)
				{
					return false;
				}
				if (ChildrenIds != other.ChildrenIds)
				{
					return false;
				}
				if (Type != other.Type)
				{
					return false;
				}
				if (MinLevel != other.MinLevel)
				{
					return false;
				}
				if (MaxLevel != other.MaxLevel)
				{
					return false;
				}
				if (Status != other.Status)
				{
					return false;
				}
				if (Restrict != other.Restrict)
				{
					return false;
				}
				if (!ProtobufEqualityComparers.BitwiseDoubleEqualityComparer.Equals(BasedConcentration, other.BasedConcentration))
				{
					return false;
				}
				if (FiveElements != other.FiveElements)
				{
					return false;
				}
				if (Sort != other.Sort)
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
				if (ParentId.Length != 0)
				{
					num ^= ParentId.GetHashCode();
				}
				if (ParentName.Length != 0)
				{
					num ^= ParentName.GetHashCode();
				}
				if (Name.Length != 0)
				{
					num ^= Name.GetHashCode();
				}
				if (SibingsId.Length != 0)
				{
					num ^= SibingsId.GetHashCode();
				}
				if (ChildrenIds.Length != 0)
				{
					num ^= ChildrenIds.GetHashCode();
				}
				if (Type != 0)
				{
					num ^= Type.GetHashCode();
				}
				if (MinLevel != 0)
				{
					num ^= MinLevel.GetHashCode();
				}
				if (MaxLevel != 0)
				{
					num ^= MaxLevel.GetHashCode();
				}
				if (Status != 0)
				{
					num ^= Status.GetHashCode();
				}
				if (Restrict.Length != 0)
				{
					num ^= Restrict.GetHashCode();
				}
				if (BasedConcentration != 0.0)
				{
					num ^= ProtobufEqualityComparers.BitwiseDoubleEqualityComparer.GetHashCode(BasedConcentration);
				}
				if (FiveElements != 0)
				{
					num ^= FiveElements.GetHashCode();
				}
				if (Sort != 0)
				{
					num ^= Sort.GetHashCode();
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
				if (ParentId.Length != 0)
				{
					output.WriteRawTag(18);
					output.WriteString(ParentId);
				}
				if (ParentName.Length != 0)
				{
					output.WriteRawTag(26);
					output.WriteString(ParentName);
				}
				if (Name.Length != 0)
				{
					output.WriteRawTag(34);
					output.WriteString(Name);
				}
				if (SibingsId.Length != 0)
				{
					output.WriteRawTag(42);
					output.WriteString(SibingsId);
				}
				if (ChildrenIds.Length != 0)
				{
					output.WriteRawTag(50);
					output.WriteString(ChildrenIds);
				}
				if (Type != 0)
				{
					output.WriteRawTag(56);
					output.WriteInt32(Type);
				}
				if (MinLevel != 0)
				{
					output.WriteRawTag(64);
					output.WriteInt32(MinLevel);
				}
				if (MaxLevel != 0)
				{
					output.WriteRawTag(72);
					output.WriteInt32(MaxLevel);
				}
				if (Status != 0)
				{
					output.WriteRawTag(80);
					output.WriteInt32(Status);
				}
				if (Restrict.Length != 0)
				{
					output.WriteRawTag(90);
					output.WriteString(Restrict);
				}
				if (BasedConcentration != 0.0)
				{
					output.WriteRawTag(97);
					output.WriteDouble(BasedConcentration);
				}
				if (FiveElements != 0)
				{
					output.WriteRawTag(104);
					output.WriteInt32(FiveElements);
				}
				if (Sort != 0)
				{
					output.WriteRawTag(112);
					output.WriteInt32(Sort);
				}
				if (Level != 0)
				{
					output.WriteRawTag(120);
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
				if (ParentId.Length != 0)
				{
					num += 1 + CodedOutputStream.ComputeStringSize(ParentId);
				}
				if (ParentName.Length != 0)
				{
					num += 1 + CodedOutputStream.ComputeStringSize(ParentName);
				}
				if (Name.Length != 0)
				{
					num += 1 + CodedOutputStream.ComputeStringSize(Name);
				}
				if (SibingsId.Length != 0)
				{
					num += 1 + CodedOutputStream.ComputeStringSize(SibingsId);
				}
				if (ChildrenIds.Length != 0)
				{
					num += 1 + CodedOutputStream.ComputeStringSize(ChildrenIds);
				}
				if (Type != 0)
				{
					num += 1 + CodedOutputStream.ComputeInt32Size(Type);
				}
				if (MinLevel != 0)
				{
					num += 1 + CodedOutputStream.ComputeInt32Size(MinLevel);
				}
				if (MaxLevel != 0)
				{
					num += 1 + CodedOutputStream.ComputeInt32Size(MaxLevel);
				}
				if (Status != 0)
				{
					num += 1 + CodedOutputStream.ComputeInt32Size(Status);
				}
				if (Restrict.Length != 0)
				{
					num += 1 + CodedOutputStream.ComputeStringSize(Restrict);
				}
				if (BasedConcentration != 0.0)
				{
					num += 9;
				}
				if (FiveElements != 0)
				{
					num += 1 + CodedOutputStream.ComputeInt32Size(FiveElements);
				}
				if (Sort != 0)
				{
					num += 1 + CodedOutputStream.ComputeInt32Size(Sort);
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
			public void MergeFrom(MapObjectResp_1007004 other)
			{
				if (other != null)
				{
					if (other.Id.Length != 0)
					{
						Id = other.Id;
					}
					if (other.ParentId.Length != 0)
					{
						ParentId = other.ParentId;
					}
					if (other.ParentName.Length != 0)
					{
						ParentName = other.ParentName;
					}
					if (other.Name.Length != 0)
					{
						Name = other.Name;
					}
					if (other.SibingsId.Length != 0)
					{
						SibingsId = other.SibingsId;
					}
					if (other.ChildrenIds.Length != 0)
					{
						ChildrenIds = other.ChildrenIds;
					}
					if (other.Type != 0)
					{
						Type = other.Type;
					}
					if (other.MinLevel != 0)
					{
						MinLevel = other.MinLevel;
					}
					if (other.MaxLevel != 0)
					{
						MaxLevel = other.MaxLevel;
					}
					if (other.Status != 0)
					{
						Status = other.Status;
					}
					if (other.Restrict.Length != 0)
					{
						Restrict = other.Restrict;
					}
					if (other.BasedConcentration != 0.0)
					{
						BasedConcentration = other.BasedConcentration;
					}
					if (other.FiveElements != 0)
					{
						FiveElements = other.FiveElements;
					}
					if (other.Sort != 0)
					{
						Sort = other.Sort;
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
						ParentId = input.ReadString();
						break;
					case 26u:
						ParentName = input.ReadString();
						break;
					case 34u:
						Name = input.ReadString();
						break;
					case 42u:
						SibingsId = input.ReadString();
						break;
					case 50u:
						ChildrenIds = input.ReadString();
						break;
					case 56u:
						Type = input.ReadInt32();
						break;
					case 64u:
						MinLevel = input.ReadInt32();
						break;
					case 72u:
						MaxLevel = input.ReadInt32();
						break;
					case 80u:
						Status = input.ReadInt32();
						break;
					case 90u:
						Restrict = input.ReadString();
						break;
					case 97u:
						BasedConcentration = input.ReadDouble();
						break;
					case 104u:
						FiveElements = input.ReadInt32();
						break;
					case 112u:
						Sort = input.ReadInt32();
						break;
					case 120u:
						Level = input.ReadInt32();
						break;
					}
				}
			}
		}
	}

	private static readonly MessageParser<ChildrenMapResp_1007003> _parser = new MessageParser<ChildrenMapResp_1007003>(() => new ChildrenMapResp_1007003());

	private UnknownFieldSet _unknownFields;

	public const int CodeFieldNumber = 1;

	private int code_;

	public const int IsSuccessFieldNumber = 2;

	private bool isSuccess_;

	public const int ObjectFieldNumber = 3;

	private static readonly FieldCodec<Types.MapObjectResp_1007004> _repeated_object_codec = FieldCodec.ForMessage(26u, Types.MapObjectResp_1007004.Parser);

	private readonly RepeatedField<Types.MapObjectResp_1007004> object_ = new RepeatedField<Types.MapObjectResp_1007004>();

	[DebuggerNonUserCode]
	public static MessageParser<ChildrenMapResp_1007003> Parser => _parser;

	[DebuggerNonUserCode]
	public static MessageDescriptor Descriptor => MapProtoReflection.Descriptor.MessageTypes[3];

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
	public RepeatedField<Types.MapObjectResp_1007004> Object => object_;

	private MessageDescriptor pb_003A_003AGoogle_002EProtobuf_002EIMessage_002Eget_Descriptor()
	{
		return Descriptor;
	}

	[DebuggerNonUserCode]
	public ChildrenMapResp_1007003()
	{
	}

	[DebuggerNonUserCode]
	public ChildrenMapResp_1007003(ChildrenMapResp_1007003 other)
		: this()
	{
		code_ = other.code_;
		isSuccess_ = other.isSuccess_;
		object_ = other.object_.Clone();
		_unknownFields = UnknownFieldSet.Clone(other._unknownFields);
	}

	[DebuggerNonUserCode]
	public ChildrenMapResp_1007003 Clone()
	{
		return new ChildrenMapResp_1007003(this);
	}

	[DebuggerNonUserCode]
	public override bool Equals(object other)
	{
		return Equals(other as ChildrenMapResp_1007003);
	}

	[DebuggerNonUserCode]
	public bool Equals(ChildrenMapResp_1007003 other)
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
	public void MergeFrom(ChildrenMapResp_1007003 other)
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
