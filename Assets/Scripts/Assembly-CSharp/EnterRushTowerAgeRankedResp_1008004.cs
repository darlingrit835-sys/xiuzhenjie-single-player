using System;
using System.Diagnostics;
using Google.Protobuf;
using Google.Protobuf.Collections;
using Google.Protobuf.Reflection;

public sealed class EnterRushTowerAgeRankedResp_1008004 : IMessage<EnterRushTowerAgeRankedResp_1008004>, IMessage, IEquatable<EnterRushTowerAgeRankedResp_1008004>, IDeepCloneable<EnterRushTowerAgeRankedResp_1008004>
{
	[DebuggerNonUserCode]
	public static class Types
	{
		public sealed class RushTowerRankedObjectResp_1008005 : IMessage<RushTowerRankedObjectResp_1008005>, IMessage, IEquatable<RushTowerRankedObjectResp_1008005>, IDeepCloneable<RushTowerRankedObjectResp_1008005>
		{
			private static readonly MessageParser<RushTowerRankedObjectResp_1008005> _parser = new MessageParser<RushTowerRankedObjectResp_1008005>(() => new RushTowerRankedObjectResp_1008005());

			private UnknownFieldSet _unknownFields;

			public const int NameFieldNumber = 1;

			private string name_ = "";

			public const int AgeFieldNumber = 2;

			private int age_;

			public const int FactionFieldNumber = 3;

			private string faction_ = "";

			public const int TierNumFieldNumber = 4;

			private int tierNum_;

			[DebuggerNonUserCode]
			public static MessageParser<RushTowerRankedObjectResp_1008005> Parser => _parser;

			[DebuggerNonUserCode]
			public static MessageDescriptor Descriptor => EnterRushTowerAgeRankedResp_1008004.Descriptor.NestedTypes[0];

			[DebuggerNonUserCode]
			MessageDescriptor IMessage.Descriptor => pb_003A_003AGoogle_002EProtobuf_002EIMessage_002Eget_Descriptor();

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
			public int Age
			{
				get
				{
					return age_;
				}
				set
				{
					age_ = value;
				}
			}

			[DebuggerNonUserCode]
			public string Faction
			{
				get
				{
					return faction_;
				}
				set
				{
					faction_ = ProtoPreconditions.CheckNotNull(value, "value");
				}
			}

			[DebuggerNonUserCode]
			public int TierNum
			{
				get
				{
					return tierNum_;
				}
				set
				{
					tierNum_ = value;
				}
			}

			private MessageDescriptor pb_003A_003AGoogle_002EProtobuf_002EIMessage_002Eget_Descriptor()
			{
				return Descriptor;
			}

			[DebuggerNonUserCode]
			public RushTowerRankedObjectResp_1008005()
			{
			}

			[DebuggerNonUserCode]
			public RushTowerRankedObjectResp_1008005(RushTowerRankedObjectResp_1008005 other)
				: this()
			{
				name_ = other.name_;
				age_ = other.age_;
				faction_ = other.faction_;
				tierNum_ = other.tierNum_;
				_unknownFields = UnknownFieldSet.Clone(other._unknownFields);
			}

			[DebuggerNonUserCode]
			public RushTowerRankedObjectResp_1008005 Clone()
			{
				return new RushTowerRankedObjectResp_1008005(this);
			}

			[DebuggerNonUserCode]
			public override bool Equals(object other)
			{
				return Equals(other as RushTowerRankedObjectResp_1008005);
			}

			[DebuggerNonUserCode]
			public bool Equals(RushTowerRankedObjectResp_1008005 other)
			{
				if (other == null)
				{
					return false;
				}
				if (other == this)
				{
					return true;
				}
				if (Name != other.Name)
				{
					return false;
				}
				if (Age != other.Age)
				{
					return false;
				}
				if (Faction != other.Faction)
				{
					return false;
				}
				if (TierNum != other.TierNum)
				{
					return false;
				}
				return object.Equals(_unknownFields, other._unknownFields);
			}

			[DebuggerNonUserCode]
			public override int GetHashCode()
			{
				int num = 1;
				if (Name.Length != 0)
				{
					num ^= Name.GetHashCode();
				}
				if (Age != 0)
				{
					num ^= Age.GetHashCode();
				}
				if (Faction.Length != 0)
				{
					num ^= Faction.GetHashCode();
				}
				if (TierNum != 0)
				{
					num ^= TierNum.GetHashCode();
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
				if (Name.Length != 0)
				{
					output.WriteRawTag(10);
					output.WriteString(Name);
				}
				if (Age != 0)
				{
					output.WriteRawTag(16);
					output.WriteInt32(Age);
				}
				if (Faction.Length != 0)
				{
					output.WriteRawTag(26);
					output.WriteString(Faction);
				}
				if (TierNum != 0)
				{
					output.WriteRawTag(32);
					output.WriteInt32(TierNum);
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
				if (Name.Length != 0)
				{
					num += 1 + CodedOutputStream.ComputeStringSize(Name);
				}
				if (Age != 0)
				{
					num += 1 + CodedOutputStream.ComputeInt32Size(Age);
				}
				if (Faction.Length != 0)
				{
					num += 1 + CodedOutputStream.ComputeStringSize(Faction);
				}
				if (TierNum != 0)
				{
					num += 1 + CodedOutputStream.ComputeInt32Size(TierNum);
				}
				if (_unknownFields != null)
				{
					num += _unknownFields.CalculateSize();
				}
				return num;
			}

			[DebuggerNonUserCode]
			public void MergeFrom(RushTowerRankedObjectResp_1008005 other)
			{
				if (other != null)
				{
					if (other.Name.Length != 0)
					{
						Name = other.Name;
					}
					if (other.Age != 0)
					{
						Age = other.Age;
					}
					if (other.Faction.Length != 0)
					{
						Faction = other.Faction;
					}
					if (other.TierNum != 0)
					{
						TierNum = other.TierNum;
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
						Name = input.ReadString();
						break;
					case 16u:
						Age = input.ReadInt32();
						break;
					case 26u:
						Faction = input.ReadString();
						break;
					case 32u:
						TierNum = input.ReadInt32();
						break;
					}
				}
			}
		}
	}

	private static readonly MessageParser<EnterRushTowerAgeRankedResp_1008004> _parser = new MessageParser<EnterRushTowerAgeRankedResp_1008004>(() => new EnterRushTowerAgeRankedResp_1008004());

	private UnknownFieldSet _unknownFields;

	public const int CodeFieldNumber = 1;

	private int code_;

	public const int IsSuccessFieldNumber = 2;

	private bool isSuccess_;

	public const int MyRankedFieldNumber = 3;

	private int myRanked_;

	public const int ObjectFieldNumber = 4;

	private static readonly FieldCodec<Types.RushTowerRankedObjectResp_1008005> _repeated_object_codec = FieldCodec.ForMessage(34u, Types.RushTowerRankedObjectResp_1008005.Parser);

	private readonly RepeatedField<Types.RushTowerRankedObjectResp_1008005> object_ = new RepeatedField<Types.RushTowerRankedObjectResp_1008005>();

	[DebuggerNonUserCode]
	public static MessageParser<EnterRushTowerAgeRankedResp_1008004> Parser => _parser;

	[DebuggerNonUserCode]
	public static MessageDescriptor Descriptor => UnrealWorldProtoReflection.Descriptor.MessageTypes[4];

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
	public RepeatedField<Types.RushTowerRankedObjectResp_1008005> Object => object_;

	private MessageDescriptor pb_003A_003AGoogle_002EProtobuf_002EIMessage_002Eget_Descriptor()
	{
		return Descriptor;
	}

	[DebuggerNonUserCode]
	public EnterRushTowerAgeRankedResp_1008004()
	{
	}

	[DebuggerNonUserCode]
	public EnterRushTowerAgeRankedResp_1008004(EnterRushTowerAgeRankedResp_1008004 other)
		: this()
	{
		code_ = other.code_;
		isSuccess_ = other.isSuccess_;
		myRanked_ = other.myRanked_;
		object_ = other.object_.Clone();
		_unknownFields = UnknownFieldSet.Clone(other._unknownFields);
	}

	[DebuggerNonUserCode]
	public EnterRushTowerAgeRankedResp_1008004 Clone()
	{
		return new EnterRushTowerAgeRankedResp_1008004(this);
	}

	[DebuggerNonUserCode]
	public override bool Equals(object other)
	{
		return Equals(other as EnterRushTowerAgeRankedResp_1008004);
	}

	[DebuggerNonUserCode]
	public bool Equals(EnterRushTowerAgeRankedResp_1008004 other)
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
		if (MyRanked != 0)
		{
			num ^= MyRanked.GetHashCode();
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
		if (MyRanked != 0)
		{
			output.WriteRawTag(24);
			output.WriteInt32(MyRanked);
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
		if (MyRanked != 0)
		{
			num += 1 + CodedOutputStream.ComputeInt32Size(MyRanked);
		}
		num += object_.CalculateSize(_repeated_object_codec);
		if (_unknownFields != null)
		{
			num += _unknownFields.CalculateSize();
		}
		return num;
	}

	[DebuggerNonUserCode]
	public void MergeFrom(EnterRushTowerAgeRankedResp_1008004 other)
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
				MyRanked = input.ReadInt32();
				break;
			case 34u:
				object_.AddEntriesFrom(input, _repeated_object_codec);
				break;
			}
		}
	}
}
