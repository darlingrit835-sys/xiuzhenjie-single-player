using System;
using System.Diagnostics;
using Google.Protobuf;
using Google.Protobuf.Collections;
using Google.Protobuf.Reflection;

public sealed class VerifyCreateMethodElixirReq_1008018 : IMessage<VerifyCreateMethodElixirReq_1008018>, IMessage, IEquatable<VerifyCreateMethodElixirReq_1008018>, IDeepCloneable<VerifyCreateMethodElixirReq_1008018>
{
	[DebuggerNonUserCode]
	public static class Types
	{
		public sealed class ElixirStepObjectReq_1008062 : IMessage<ElixirStepObjectReq_1008062>, IMessage, IEquatable<ElixirStepObjectReq_1008062>, IDeepCloneable<ElixirStepObjectReq_1008062>
		{
			private static readonly MessageParser<ElixirStepObjectReq_1008062> _parser = new MessageParser<ElixirStepObjectReq_1008062>(() => new ElixirStepObjectReq_1008062());

			private UnknownFieldSet _unknownFields;

			public const int ActionTypeFieldNumber = 1;

			private int actionType_;

			public const int TargetTypeFieldNumber = 2;

			private int targetType_;

			public const int TargetIdFieldNumber = 3;

			private string targetId_ = "";

			public const int OrderFieldNumber = 4;

			private int order_;

			[DebuggerNonUserCode]
			public static MessageParser<ElixirStepObjectReq_1008062> Parser => _parser;

			[DebuggerNonUserCode]
			public static MessageDescriptor Descriptor => VerifyCreateMethodElixirReq_1008018.Descriptor.NestedTypes[0];

			[DebuggerNonUserCode]
			MessageDescriptor IMessage.Descriptor => pb_003A_003AGoogle_002EProtobuf_002EIMessage_002Eget_Descriptor();

			[DebuggerNonUserCode]
			public int ActionType
			{
				get
				{
					return actionType_;
				}
				set
				{
					actionType_ = value;
				}
			}

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
			public ElixirStepObjectReq_1008062()
			{
			}

			[DebuggerNonUserCode]
			public ElixirStepObjectReq_1008062(ElixirStepObjectReq_1008062 other)
				: this()
			{
				actionType_ = other.actionType_;
				targetType_ = other.targetType_;
				targetId_ = other.targetId_;
				order_ = other.order_;
				_unknownFields = UnknownFieldSet.Clone(other._unknownFields);
			}

			[DebuggerNonUserCode]
			public ElixirStepObjectReq_1008062 Clone()
			{
				return new ElixirStepObjectReq_1008062(this);
			}

			[DebuggerNonUserCode]
			public override bool Equals(object other)
			{
				return Equals(other as ElixirStepObjectReq_1008062);
			}

			[DebuggerNonUserCode]
			public bool Equals(ElixirStepObjectReq_1008062 other)
			{
				if (other == null)
				{
					return false;
				}
				if (other == this)
				{
					return true;
				}
				if (ActionType != other.ActionType)
				{
					return false;
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
				if (ActionType != 0)
				{
					num ^= ActionType.GetHashCode();
				}
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
				if (ActionType != 0)
				{
					output.WriteRawTag(8);
					output.WriteInt32(ActionType);
				}
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
				if (ActionType != 0)
				{
					num += 1 + CodedOutputStream.ComputeInt32Size(ActionType);
				}
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
			public void MergeFrom(ElixirStepObjectReq_1008062 other)
			{
				if (other != null)
				{
					if (other.ActionType != 0)
					{
						ActionType = other.ActionType;
					}
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
					case 8u:
						ActionType = input.ReadInt32();
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

	private static readonly MessageParser<VerifyCreateMethodElixirReq_1008018> _parser = new MessageParser<VerifyCreateMethodElixirReq_1008018>(() => new VerifyCreateMethodElixirReq_1008018());

	private UnknownFieldSet _unknownFields;

	public const int StageFieldNumber = 1;

	private int stage_;

	public const int ObjectFieldNumber = 2;

	private static readonly FieldCodec<Types.ElixirStepObjectReq_1008062> _repeated_object_codec = FieldCodec.ForMessage(18u, Types.ElixirStepObjectReq_1008062.Parser);

	private readonly RepeatedField<Types.ElixirStepObjectReq_1008062> object_ = new RepeatedField<Types.ElixirStepObjectReq_1008062>();

	[DebuggerNonUserCode]
	public static MessageParser<VerifyCreateMethodElixirReq_1008018> Parser => _parser;

	[DebuggerNonUserCode]
	public static MessageDescriptor Descriptor => UnrealWorldProtoReflection.Descriptor.MessageTypes[19];

	[DebuggerNonUserCode]
	MessageDescriptor IMessage.Descriptor => pb_003A_003AGoogle_002EProtobuf_002EIMessage_002Eget_Descriptor();

	[DebuggerNonUserCode]
	public int Stage
	{
		get
		{
			return stage_;
		}
		set
		{
			stage_ = value;
		}
	}

	[DebuggerNonUserCode]
	public RepeatedField<Types.ElixirStepObjectReq_1008062> Object => object_;

	private MessageDescriptor pb_003A_003AGoogle_002EProtobuf_002EIMessage_002Eget_Descriptor()
	{
		return Descriptor;
	}

	[DebuggerNonUserCode]
	public VerifyCreateMethodElixirReq_1008018()
	{
	}

	[DebuggerNonUserCode]
	public VerifyCreateMethodElixirReq_1008018(VerifyCreateMethodElixirReq_1008018 other)
		: this()
	{
		stage_ = other.stage_;
		object_ = other.object_.Clone();
		_unknownFields = UnknownFieldSet.Clone(other._unknownFields);
	}

	[DebuggerNonUserCode]
	public VerifyCreateMethodElixirReq_1008018 Clone()
	{
		return new VerifyCreateMethodElixirReq_1008018(this);
	}

	[DebuggerNonUserCode]
	public override bool Equals(object other)
	{
		return Equals(other as VerifyCreateMethodElixirReq_1008018);
	}

	[DebuggerNonUserCode]
	public bool Equals(VerifyCreateMethodElixirReq_1008018 other)
	{
		if (other == null)
		{
			return false;
		}
		if (other == this)
		{
			return true;
		}
		if (Stage != other.Stage)
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
		if (Stage != 0)
		{
			num ^= Stage.GetHashCode();
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
		if (Stage != 0)
		{
			output.WriteRawTag(8);
			output.WriteInt32(Stage);
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
		if (Stage != 0)
		{
			num += 1 + CodedOutputStream.ComputeInt32Size(Stage);
		}
		num += object_.CalculateSize(_repeated_object_codec);
		if (_unknownFields != null)
		{
			num += _unknownFields.CalculateSize();
		}
		return num;
	}

	[DebuggerNonUserCode]
	public void MergeFrom(VerifyCreateMethodElixirReq_1008018 other)
	{
		if (other != null)
		{
			if (other.Stage != 0)
			{
				Stage = other.Stage;
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
				Stage = input.ReadInt32();
				break;
			case 18u:
				object_.AddEntriesFrom(input, _repeated_object_codec);
				break;
			}
		}
	}
}
