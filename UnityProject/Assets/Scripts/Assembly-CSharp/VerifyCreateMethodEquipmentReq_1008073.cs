using System;
using System.Diagnostics;
using Google.Protobuf;
using Google.Protobuf.Collections;
using Google.Protobuf.Reflection;

public sealed class VerifyCreateMethodEquipmentReq_1008073 : IMessage<VerifyCreateMethodEquipmentReq_1008073>, IMessage, IEquatable<VerifyCreateMethodEquipmentReq_1008073>, IDeepCloneable<VerifyCreateMethodEquipmentReq_1008073>
{
	[DebuggerNonUserCode]
	public static class Types
	{
		public sealed class EquipmentStepObjectReq_1008074 : IMessage<EquipmentStepObjectReq_1008074>, IMessage, IEquatable<EquipmentStepObjectReq_1008074>, IDeepCloneable<EquipmentStepObjectReq_1008074>
		{
			private static readonly MessageParser<EquipmentStepObjectReq_1008074> _parser = new MessageParser<EquipmentStepObjectReq_1008074>(() => new EquipmentStepObjectReq_1008074());

			private UnknownFieldSet _unknownFields;

			public const int TargetTypeFieldNumber = 2;

			private int targetType_;

			public const int TargetIdFieldNumber = 3;

			private string targetId_ = "";

			public const int OrderFieldNumber = 4;

			private int order_;

			[DebuggerNonUserCode]
			public static MessageParser<EquipmentStepObjectReq_1008074> Parser => _parser;

			[DebuggerNonUserCode]
			public static MessageDescriptor Descriptor => VerifyCreateMethodEquipmentReq_1008073.Descriptor.NestedTypes[0];

			[DebuggerNonUserCode]
			MessageDescriptor IMessage.Descriptor => pb_003A_003AGoogle_002EProtobuf_002EIMessage_002Eget_Descriptor();

			[DebuggerNonUserCode]
			public int TargetType
			{
				get
				{
					return targetType_;
				}
				set
				{
					targetType_ = value;
				}
			}

			[DebuggerNonUserCode]
			public string TargetId
			{
				get
				{
					return targetId_;
				}
				set
				{
					targetId_ = ProtoPreconditions.CheckNotNull(value, "value");
				}
			}

			[DebuggerNonUserCode]
			public int Order
			{
				get
				{
					return order_;
				}
				set
				{
					order_ = value;
				}
			}

			private MessageDescriptor pb_003A_003AGoogle_002EProtobuf_002EIMessage_002Eget_Descriptor()
			{
				return Descriptor;
			}

			[DebuggerNonUserCode]
			public EquipmentStepObjectReq_1008074()
			{
			}

			[DebuggerNonUserCode]
			public EquipmentStepObjectReq_1008074(EquipmentStepObjectReq_1008074 other)
				: this()
			{
				targetType_ = other.targetType_;
				targetId_ = other.targetId_;
				order_ = other.order_;
				_unknownFields = UnknownFieldSet.Clone(other._unknownFields);
			}

			[DebuggerNonUserCode]
			public EquipmentStepObjectReq_1008074 Clone()
			{
				return new EquipmentStepObjectReq_1008074(this);
			}

			[DebuggerNonUserCode]
			public override bool Equals(object other)
			{
				return Equals(other as EquipmentStepObjectReq_1008074);
			}

			[DebuggerNonUserCode]
			public bool Equals(EquipmentStepObjectReq_1008074 other)
			{
				if (other == null)
				{
					return false;
				}
				if (other == this)
				{
					return true;
				}
				if (TargetType != other.TargetType)
				{
					return false;
				}
				if (TargetId != other.TargetId)
				{
					return false;
				}
				if (Order != other.Order)
				{
					return false;
				}
				return object.Equals(_unknownFields, other._unknownFields);
			}

			[DebuggerNonUserCode]
			public override int GetHashCode()
			{
				int num = 1;
				if (TargetType != 0)
				{
					num ^= TargetType.GetHashCode();
				}
				if (TargetId.Length != 0)
				{
					num ^= TargetId.GetHashCode();
				}
				if (Order != 0)
				{
					num ^= Order.GetHashCode();
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
				if (TargetType != 0)
				{
					output.WriteRawTag(16);
					output.WriteInt32(TargetType);
				}
				if (TargetId.Length != 0)
				{
					output.WriteRawTag(26);
					output.WriteString(TargetId);
				}
				if (Order != 0)
				{
					output.WriteRawTag(32);
					output.WriteInt32(Order);
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
				if (TargetType != 0)
				{
					num += 1 + CodedOutputStream.ComputeInt32Size(TargetType);
				}
				if (TargetId.Length != 0)
				{
					num += 1 + CodedOutputStream.ComputeStringSize(TargetId);
				}
				if (Order != 0)
				{
					num += 1 + CodedOutputStream.ComputeInt32Size(Order);
				}
				if (_unknownFields != null)
				{
					num += _unknownFields.CalculateSize();
				}
				return num;
			}

			[DebuggerNonUserCode]
			public void MergeFrom(EquipmentStepObjectReq_1008074 other)
			{
				if (other != null)
				{
					if (other.TargetType != 0)
					{
						TargetType = other.TargetType;
					}
					if (other.TargetId.Length != 0)
					{
						TargetId = other.TargetId;
					}
					if (other.Order != 0)
					{
						Order = other.Order;
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
					case 16u:
						TargetType = input.ReadInt32();
						break;
					case 26u:
						TargetId = input.ReadString();
						break;
					case 32u:
						Order = input.ReadInt32();
						break;
					}
				}
			}
		}
	}

	private static readonly MessageParser<VerifyCreateMethodEquipmentReq_1008073> _parser = new MessageParser<VerifyCreateMethodEquipmentReq_1008073>(() => new VerifyCreateMethodEquipmentReq_1008073());

	private UnknownFieldSet _unknownFields;

	public const int ObjectFieldNumber = 1;

	private static readonly FieldCodec<Types.EquipmentStepObjectReq_1008074> _repeated_object_codec = FieldCodec.ForMessage(10u, Types.EquipmentStepObjectReq_1008074.Parser);

	private readonly RepeatedField<Types.EquipmentStepObjectReq_1008074> object_ = new RepeatedField<Types.EquipmentStepObjectReq_1008074>();

	public const int EquipmentTypeFieldNumber = 2;

	private int equipmentType_;

	[DebuggerNonUserCode]
	public static MessageParser<VerifyCreateMethodEquipmentReq_1008073> Parser => _parser;

	[DebuggerNonUserCode]
	public static MessageDescriptor Descriptor => UnrealWorldProtoReflection.Descriptor.MessageTypes[87];

	[DebuggerNonUserCode]
	MessageDescriptor IMessage.Descriptor => pb_003A_003AGoogle_002EProtobuf_002EIMessage_002Eget_Descriptor();

	[DebuggerNonUserCode]
	public RepeatedField<Types.EquipmentStepObjectReq_1008074> Object => object_;

	[DebuggerNonUserCode]
	public int EquipmentType
	{
		get
		{
			return equipmentType_;
		}
		set
		{
			equipmentType_ = value;
		}
	}

	private MessageDescriptor pb_003A_003AGoogle_002EProtobuf_002EIMessage_002Eget_Descriptor()
	{
		return Descriptor;
	}

	[DebuggerNonUserCode]
	public VerifyCreateMethodEquipmentReq_1008073()
	{
	}

	[DebuggerNonUserCode]
	public VerifyCreateMethodEquipmentReq_1008073(VerifyCreateMethodEquipmentReq_1008073 other)
		: this()
	{
		object_ = other.object_.Clone();
		equipmentType_ = other.equipmentType_;
		_unknownFields = UnknownFieldSet.Clone(other._unknownFields);
	}

	[DebuggerNonUserCode]
	public VerifyCreateMethodEquipmentReq_1008073 Clone()
	{
		return new VerifyCreateMethodEquipmentReq_1008073(this);
	}

	[DebuggerNonUserCode]
	public override bool Equals(object other)
	{
		return Equals(other as VerifyCreateMethodEquipmentReq_1008073);
	}

	[DebuggerNonUserCode]
	public bool Equals(VerifyCreateMethodEquipmentReq_1008073 other)
	{
		if (other == null)
		{
			return false;
		}
		if (other == this)
		{
			return true;
		}
		if (!object_.Equals(other.object_))
		{
			return false;
		}
		if (EquipmentType != other.EquipmentType)
		{
			return false;
		}
		return object.Equals(_unknownFields, other._unknownFields);
	}

	[DebuggerNonUserCode]
	public override int GetHashCode()
	{
		int num = 1;
		num ^= object_.GetHashCode();
		if (EquipmentType != 0)
		{
			num ^= EquipmentType.GetHashCode();
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
		object_.WriteTo(output, _repeated_object_codec);
		if (EquipmentType != 0)
		{
			output.WriteRawTag(16);
			output.WriteInt32(EquipmentType);
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
		num += object_.CalculateSize(_repeated_object_codec);
		if (EquipmentType != 0)
		{
			num += 1 + CodedOutputStream.ComputeInt32Size(EquipmentType);
		}
		if (_unknownFields != null)
		{
			num += _unknownFields.CalculateSize();
		}
		return num;
	}

	[DebuggerNonUserCode]
	public void MergeFrom(VerifyCreateMethodEquipmentReq_1008073 other)
	{
		if (other != null)
		{
			object_.Add(other.object_);
			if (other.EquipmentType != 0)
			{
				EquipmentType = other.EquipmentType;
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
				object_.AddEntriesFrom(input, _repeated_object_codec);
				break;
			case 16u:
				EquipmentType = input.ReadInt32();
				break;
			}
		}
	}
}
