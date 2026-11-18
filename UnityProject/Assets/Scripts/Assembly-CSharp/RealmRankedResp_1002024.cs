using System;
using System.Diagnostics;
using Google.Protobuf;
using Google.Protobuf.Collections;
using Google.Protobuf.Reflection;

public sealed class RealmRankedResp_1002024 : IMessage<RealmRankedResp_1002024>, IMessage, IEquatable<RealmRankedResp_1002024>, IDeepCloneable<RealmRankedResp_1002024>
{
	[DebuggerNonUserCode]
	public static class Types
	{
		public sealed class RealmRankedObjectResp_1002025 : IMessage<RealmRankedObjectResp_1002025>, IMessage, IEquatable<RealmRankedObjectResp_1002025>, IDeepCloneable<RealmRankedObjectResp_1002025>
		{
			private static readonly MessageParser<RealmRankedObjectResp_1002025> _parser = new MessageParser<RealmRankedObjectResp_1002025>(() => new RealmRankedObjectResp_1002025());

			private UnknownFieldSet _unknownFields;

			public const int IdFieldNumber = 1;

			private string id_ = "";

			public const int NameFieldNumber = 2;

			private string name_ = "";

			public const int LevelFieldNumber = 3;

			private int level_;

			public const int HeatFieldNumber = 4;

			private int heat_;

			[DebuggerNonUserCode]
			public static MessageParser<RealmRankedObjectResp_1002025> Parser => _parser;

			[DebuggerNonUserCode]
			public static MessageDescriptor Descriptor => RealmRankedResp_1002024.Descriptor.NestedTypes[0];

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
			public int Heat
			{
				get
				{
					return heat_;
				}
				set
				{
					heat_ = value;
				}
			}

			private MessageDescriptor pb_003A_003AGoogle_002EProtobuf_002EIMessage_002Eget_Descriptor()
			{
				return Descriptor;
			}

			[DebuggerNonUserCode]
			public RealmRankedObjectResp_1002025()
			{
			}

			[DebuggerNonUserCode]
			public RealmRankedObjectResp_1002025(RealmRankedObjectResp_1002025 other)
				: this()
			{
				id_ = other.id_;
				name_ = other.name_;
				level_ = other.level_;
				heat_ = other.heat_;
				_unknownFields = UnknownFieldSet.Clone(other._unknownFields);
			}

			[DebuggerNonUserCode]
			public RealmRankedObjectResp_1002025 Clone()
			{
				return new RealmRankedObjectResp_1002025(this);
			}

			[DebuggerNonUserCode]
			public override bool Equals(object other)
			{
				return Equals(other as RealmRankedObjectResp_1002025);
			}

			[DebuggerNonUserCode]
			public bool Equals(RealmRankedObjectResp_1002025 other)
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
				if (Level != other.Level)
				{
					return false;
				}
				if (Heat != other.Heat)
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
				if (Level != 0)
				{
					num ^= Level.GetHashCode();
				}
				if (Heat != 0)
				{
					num ^= Heat.GetHashCode();
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
				if (Level != 0)
				{
					output.WriteRawTag(24);
					output.WriteInt32(Level);
				}
				if (Heat != 0)
				{
					output.WriteRawTag(32);
					output.WriteInt32(Heat);
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
				if (Level != 0)
				{
					num += 1 + CodedOutputStream.ComputeInt32Size(Level);
				}
				if (Heat != 0)
				{
					num += 1 + CodedOutputStream.ComputeInt32Size(Heat);
				}
				if (_unknownFields != null)
				{
					num += _unknownFields.CalculateSize();
				}
				return num;
			}

			[DebuggerNonUserCode]
			public void MergeFrom(RealmRankedObjectResp_1002025 other)
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
					if (other.Level != 0)
					{
						Level = other.Level;
					}
					if (other.Heat != 0)
					{
						Heat = other.Heat;
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
						Level = input.ReadInt32();
						break;
					case 32u:
						Heat = input.ReadInt32();
						break;
					}
				}
			}
		}
	}

	private static readonly MessageParser<RealmRankedResp_1002024> _parser = new MessageParser<RealmRankedResp_1002024>(() => new RealmRankedResp_1002024());

	private UnknownFieldSet _unknownFields;

	public const int CodeFieldNumber = 1;

	private int code_;

	public const int IsSuccessFieldNumber = 2;

	private bool isSuccess_;

	public const int MyRankedFieldNumber = 3;

	private int myRanked_;

	public const int MyHeatFieldNumber = 4;

	private int myHeat_;

	public const int ObjectFieldNumber = 5;

	private static readonly FieldCodec<Types.RealmRankedObjectResp_1002025> _repeated_object_codec = FieldCodec.ForMessage(42u, Types.RealmRankedObjectResp_1002025.Parser);

	private readonly RepeatedField<Types.RealmRankedObjectResp_1002025> object_ = new RepeatedField<Types.RealmRankedObjectResp_1002025>();

	public const int MyStageFieldNumber = 6;

	private int myStage_;

	[DebuggerNonUserCode]
	public static MessageParser<RealmRankedResp_1002024> Parser => _parser;

	[DebuggerNonUserCode]
	public static MessageDescriptor Descriptor => RoleProtoReflection.Descriptor.MessageTypes[27];

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
	public int MyRanked
	{
		get
		{
			return myRanked_;
		}
		set
		{
			myRanked_ = value;
		}
	}

	[DebuggerNonUserCode]
	public int MyHeat
	{
		get
		{
			return myHeat_;
		}
		set
		{
			myHeat_ = value;
		}
	}

	[DebuggerNonUserCode]
	public RepeatedField<Types.RealmRankedObjectResp_1002025> Object => object_;

	[DebuggerNonUserCode]
	public int MyStage
	{
		get
		{
			return myStage_;
		}
		set
		{
			myStage_ = value;
		}
	}

	private MessageDescriptor pb_003A_003AGoogle_002EProtobuf_002EIMessage_002Eget_Descriptor()
	{
		return Descriptor;
	}

	[DebuggerNonUserCode]
	public RealmRankedResp_1002024()
	{
	}

	[DebuggerNonUserCode]
	public RealmRankedResp_1002024(RealmRankedResp_1002024 other)
		: this()
	{
		code_ = other.code_;
		isSuccess_ = other.isSuccess_;
		myRanked_ = other.myRanked_;
		myHeat_ = other.myHeat_;
		object_ = other.object_.Clone();
		myStage_ = other.myStage_;
		_unknownFields = UnknownFieldSet.Clone(other._unknownFields);
	}

	[DebuggerNonUserCode]
	public RealmRankedResp_1002024 Clone()
	{
		return new RealmRankedResp_1002024(this);
	}

	[DebuggerNonUserCode]
	public override bool Equals(object other)
	{
		return Equals(other as RealmRankedResp_1002024);
	}

	[DebuggerNonUserCode]
	public bool Equals(RealmRankedResp_1002024 other)
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
		if (MyRanked != other.MyRanked)
		{
			return false;
		}
		if (MyHeat != other.MyHeat)
		{
			return false;
		}
		if (!object_.Equals(other.object_))
		{
			return false;
		}
		if (MyStage != other.MyStage)
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
		if (MyRanked != 0)
		{
			num ^= MyRanked.GetHashCode();
		}
		if (MyHeat != 0)
		{
			num ^= MyHeat.GetHashCode();
		}
		num ^= object_.GetHashCode();
		if (MyStage != 0)
		{
			num ^= MyStage.GetHashCode();
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
		if (MyRanked != 0)
		{
			output.WriteRawTag(24);
			output.WriteInt32(MyRanked);
		}
		if (MyHeat != 0)
		{
			output.WriteRawTag(32);
			output.WriteInt32(MyHeat);
		}
		object_.WriteTo(output, _repeated_object_codec);
		if (MyStage != 0)
		{
			output.WriteRawTag(48);
			output.WriteInt32(MyStage);
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
		if (MyRanked != 0)
		{
			num += 1 + CodedOutputStream.ComputeInt32Size(MyRanked);
		}
		if (MyHeat != 0)
		{
			num += 1 + CodedOutputStream.ComputeInt32Size(MyHeat);
		}
		num += object_.CalculateSize(_repeated_object_codec);
		if (MyStage != 0)
		{
			num += 1 + CodedOutputStream.ComputeInt32Size(MyStage);
		}
		if (_unknownFields != null)
		{
			num += _unknownFields.CalculateSize();
		}
		return num;
	}

	[DebuggerNonUserCode]
	public void MergeFrom(RealmRankedResp_1002024 other)
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
			if (other.MyRanked != 0)
			{
				MyRanked = other.MyRanked;
			}
			if (other.MyHeat != 0)
			{
				MyHeat = other.MyHeat;
			}
			object_.Add(other.object_);
			if (other.MyStage != 0)
			{
				MyStage = other.MyStage;
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
			case 24u:
				MyRanked = input.ReadInt32();
				break;
			case 32u:
				MyHeat = input.ReadInt32();
				break;
			case 42u:
				object_.AddEntriesFrom(input, _repeated_object_codec);
				break;
			case 48u:
				MyStage = input.ReadInt32();
				break;
			}
		}
	}
}
