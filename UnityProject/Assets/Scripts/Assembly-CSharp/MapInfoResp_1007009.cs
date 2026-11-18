using System;
using System.Diagnostics;
using Google.Protobuf;
using Google.Protobuf.Collections;
using Google.Protobuf.Reflection;

public sealed class MapInfoResp_1007009 : IMessage<MapInfoResp_1007009>, IMessage, IEquatable<MapInfoResp_1007009>, IDeepCloneable<MapInfoResp_1007009>
{
	private static readonly MessageParser<MapInfoResp_1007009> _parser = new MessageParser<MapInfoResp_1007009>(() => new MapInfoResp_1007009());

	private UnknownFieldSet _unknownFields;

	public const int CodeFieldNumber = 1;

	private int code_;

	public const int IsSuccessFieldNumber = 2;

	private bool isSuccess_;

	public const int IdFieldNumber = 3;

	private string id_ = "";

	public const int ParentIdFieldNumber = 4;

	private string parentId_ = "";

	public const int ParentNameFieldNumber = 5;

	private string parentName_ = "";

	public const int NameFieldNumber = 6;

	private string name_ = "";

	public const int SibingsIdFieldNumber = 7;

	private string sibingsId_ = "";

	public const int ChildrenIdsFieldNumber = 8;

	private string childrenIds_ = "";

	public const int TypeFieldNumber = 9;

	private int type_;

	public const int MinLevelFieldNumber = 10;

	private int minLevel_;

	public const int MaxLevelFieldNumber = 11;

	private int maxLevel_;

	public const int StatusFieldNumber = 12;

	private int status_;

	public const int RestrictFieldNumber = 13;

	private string restrict_ = "";

	public const int BasedConcentrationFieldNumber = 14;

	private double basedConcentration_;

	public const int FiveElementsFieldNumber = 15;

	private int fiveElements_;

	public const int SortFieldNumber = 16;

	private int sort_;

	public const int LevelFieldNumber = 17;

	private int level_;

	[DebuggerNonUserCode]
	public static MessageParser<MapInfoResp_1007009> Parser => _parser;

	[DebuggerNonUserCode]
	public static MessageDescriptor Descriptor => MapProtoReflection.Descriptor.MessageTypes[10];

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
	public MapInfoResp_1007009()
	{
	}

	[DebuggerNonUserCode]
	public MapInfoResp_1007009(MapInfoResp_1007009 other)
		: this()
	{
		code_ = other.code_;
		isSuccess_ = other.isSuccess_;
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
	public MapInfoResp_1007009 Clone()
	{
		return new MapInfoResp_1007009(this);
	}

	[DebuggerNonUserCode]
	public override bool Equals(object other)
	{
		return Equals(other as MapInfoResp_1007009);
	}

	[DebuggerNonUserCode]
	public bool Equals(MapInfoResp_1007009 other)
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
		if (Code != 0)
		{
			num ^= Code.GetHashCode();
		}
		if (IsSuccess)
		{
			num ^= IsSuccess.GetHashCode();
		}
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
		if (Id.Length != 0)
		{
			output.WriteRawTag(26);
			output.WriteString(Id);
		}
		if (ParentId.Length != 0)
		{
			output.WriteRawTag(34);
			output.WriteString(ParentId);
		}
		if (ParentName.Length != 0)
		{
			output.WriteRawTag(42);
			output.WriteString(ParentName);
		}
		if (Name.Length != 0)
		{
			output.WriteRawTag(50);
			output.WriteString(Name);
		}
		if (SibingsId.Length != 0)
		{
			output.WriteRawTag(58);
			output.WriteString(SibingsId);
		}
		if (ChildrenIds.Length != 0)
		{
			output.WriteRawTag(66);
			output.WriteString(ChildrenIds);
		}
		if (Type != 0)
		{
			output.WriteRawTag(72);
			output.WriteInt32(Type);
		}
		if (MinLevel != 0)
		{
			output.WriteRawTag(80);
			output.WriteInt32(MinLevel);
		}
		if (MaxLevel != 0)
		{
			output.WriteRawTag(88);
			output.WriteInt32(MaxLevel);
		}
		if (Status != 0)
		{
			output.WriteRawTag(96);
			output.WriteInt32(Status);
		}
		if (Restrict.Length != 0)
		{
			output.WriteRawTag(106);
			output.WriteString(Restrict);
		}
		if (BasedConcentration != 0.0)
		{
			output.WriteRawTag(113);
			output.WriteDouble(BasedConcentration);
		}
		if (FiveElements != 0)
		{
			output.WriteRawTag(120);
			output.WriteInt32(FiveElements);
		}
		if (Sort != 0)
		{
			output.WriteRawTag(128, 1);
			output.WriteInt32(Sort);
		}
		if (Level != 0)
		{
			output.WriteRawTag(136, 1);
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
		if (Code != 0)
		{
			num += 1 + CodedOutputStream.ComputeInt32Size(Code);
		}
		if (IsSuccess)
		{
			num += 2;
		}
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
			num += 2 + CodedOutputStream.ComputeInt32Size(Sort);
		}
		if (Level != 0)
		{
			num += 2 + CodedOutputStream.ComputeInt32Size(Level);
		}
		if (_unknownFields != null)
		{
			num += _unknownFields.CalculateSize();
		}
		return num;
	}

	[DebuggerNonUserCode]
	public void MergeFrom(MapInfoResp_1007009 other)
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
			case 8u:
				Code = input.ReadInt32();
				break;
			case 16u:
				IsSuccess = input.ReadBool();
				break;
			case 26u:
				Id = input.ReadString();
				break;
			case 34u:
				ParentId = input.ReadString();
				break;
			case 42u:
				ParentName = input.ReadString();
				break;
			case 50u:
				Name = input.ReadString();
				break;
			case 58u:
				SibingsId = input.ReadString();
				break;
			case 66u:
				ChildrenIds = input.ReadString();
				break;
			case 72u:
				Type = input.ReadInt32();
				break;
			case 80u:
				MinLevel = input.ReadInt32();
				break;
			case 88u:
				MaxLevel = input.ReadInt32();
				break;
			case 96u:
				Status = input.ReadInt32();
				break;
			case 106u:
				Restrict = input.ReadString();
				break;
			case 113u:
				BasedConcentration = input.ReadDouble();
				break;
			case 120u:
				FiveElements = input.ReadInt32();
				break;
			case 128u:
				Sort = input.ReadInt32();
				break;
			case 136u:
				Level = input.ReadInt32();
				break;
			}
		}
	}
}
