using System;
using System.Diagnostics;
using Google.Protobuf;
using Google.Protobuf.Collections;
using Google.Protobuf.Reflection;

public sealed class SearchCreateEquipmentPlantMaterialResp_1008075 : IMessage<SearchCreateEquipmentPlantMaterialResp_1008075>, IMessage, IEquatable<SearchCreateEquipmentPlantMaterialResp_1008075>, IDeepCloneable<SearchCreateEquipmentPlantMaterialResp_1008075>
{
	[DebuggerNonUserCode]
	public static class Types
	{
		public sealed class PlantMaterialResp_1008076 : IMessage<PlantMaterialResp_1008076>, IMessage, IEquatable<PlantMaterialResp_1008076>, IDeepCloneable<PlantMaterialResp_1008076>
		{
			private static readonly MessageParser<PlantMaterialResp_1008076> _parser = new MessageParser<PlantMaterialResp_1008076>(() => new PlantMaterialResp_1008076());

			private UnknownFieldSet _unknownFields;

			public const int IdFieldNumber = 1;

			private string id_ = "";

			public const int NameFieldNumber = 2;

			private string name_ = "";

			public const int RankFieldNumber = 3;

			private int rank_;

			[DebuggerNonUserCode]
			public static MessageParser<PlantMaterialResp_1008076> Parser => _parser;

			[DebuggerNonUserCode]
			public static MessageDescriptor Descriptor => SearchCreateEquipmentPlantMaterialResp_1008075.Descriptor.NestedTypes[0];

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
			public PlantMaterialResp_1008076()
			{
			}

			[DebuggerNonUserCode]
			public PlantMaterialResp_1008076(PlantMaterialResp_1008076 other)
				: this()
			{
				id_ = other.id_;
				name_ = other.name_;
				rank_ = other.rank_;
				_unknownFields = UnknownFieldSet.Clone(other._unknownFields);
			}

			[DebuggerNonUserCode]
			public PlantMaterialResp_1008076 Clone()
			{
				return new PlantMaterialResp_1008076(this);
			}

			[DebuggerNonUserCode]
			public override bool Equals(object other)
			{
				return Equals(other as PlantMaterialResp_1008076);
			}

			[DebuggerNonUserCode]
			public bool Equals(PlantMaterialResp_1008076 other)
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
				if (Name.Length != 0)
				{
					num ^= Name.GetHashCode();
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
				if (Name.Length != 0)
				{
					output.WriteRawTag(18);
					output.WriteString(Name);
				}
				if (Rank != 0)
				{
					output.WriteRawTag(24);
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
				if (Name.Length != 0)
				{
					num += 1 + CodedOutputStream.ComputeStringSize(Name);
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
			public void MergeFrom(PlantMaterialResp_1008076 other)
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
						Name = input.ReadString();
						break;
					case 24u:
						Rank = input.ReadInt32();
						break;
					}
				}
			}
		}
	}

	private static readonly MessageParser<SearchCreateEquipmentPlantMaterialResp_1008075> _parser = new MessageParser<SearchCreateEquipmentPlantMaterialResp_1008075>(() => new SearchCreateEquipmentPlantMaterialResp_1008075());

	private UnknownFieldSet _unknownFields;

	public const int CodeFieldNumber = 1;

	private int code_;

	public const int IsSuccessFieldNumber = 2;

	private bool isSuccess_;

	public const int PlantListFieldNumber = 3;

	private static readonly FieldCodec<Types.PlantMaterialResp_1008076> _repeated_plantList_codec = FieldCodec.ForMessage(26u, Types.PlantMaterialResp_1008076.Parser);

	private readonly RepeatedField<Types.PlantMaterialResp_1008076> plantList_ = new RepeatedField<Types.PlantMaterialResp_1008076>();

	[DebuggerNonUserCode]
	public static MessageParser<SearchCreateEquipmentPlantMaterialResp_1008075> Parser => _parser;

	[DebuggerNonUserCode]
	public static MessageDescriptor Descriptor => UnrealWorldProtoReflection.Descriptor.MessageTypes[90];

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
	public RepeatedField<Types.PlantMaterialResp_1008076> PlantList => plantList_;

	private MessageDescriptor pb_003A_003AGoogle_002EProtobuf_002EIMessage_002Eget_Descriptor()
	{
		return Descriptor;
	}

	[DebuggerNonUserCode]
	public SearchCreateEquipmentPlantMaterialResp_1008075()
	{
	}

	[DebuggerNonUserCode]
	public SearchCreateEquipmentPlantMaterialResp_1008075(SearchCreateEquipmentPlantMaterialResp_1008075 other)
		: this()
	{
		code_ = other.code_;
		isSuccess_ = other.isSuccess_;
		plantList_ = other.plantList_.Clone();
		_unknownFields = UnknownFieldSet.Clone(other._unknownFields);
	}

	[DebuggerNonUserCode]
	public SearchCreateEquipmentPlantMaterialResp_1008075 Clone()
	{
		return new SearchCreateEquipmentPlantMaterialResp_1008075(this);
	}

	[DebuggerNonUserCode]
	public override bool Equals(object other)
	{
		return Equals(other as SearchCreateEquipmentPlantMaterialResp_1008075);
	}

	[DebuggerNonUserCode]
	public bool Equals(SearchCreateEquipmentPlantMaterialResp_1008075 other)
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
		if (!plantList_.Equals(other.plantList_))
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
		num ^= plantList_.GetHashCode();
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
		plantList_.WriteTo(output, _repeated_plantList_codec);
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
		num += plantList_.CalculateSize(_repeated_plantList_codec);
		if (_unknownFields != null)
		{
			num += _unknownFields.CalculateSize();
		}
		return num;
	}

	[DebuggerNonUserCode]
	public void MergeFrom(SearchCreateEquipmentPlantMaterialResp_1008075 other)
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
			plantList_.Add(other.plantList_);
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
				plantList_.AddEntriesFrom(input, _repeated_plantList_codec);
				break;
			}
		}
	}
}
