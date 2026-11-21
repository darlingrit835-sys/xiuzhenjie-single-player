using System;
using System.Diagnostics;
using Google.Protobuf;
using Google.Protobuf.Collections;
using Google.Protobuf.Reflection;

public sealed class ExamineElixirStepResp_1008009 : IMessage<ExamineElixirStepResp_1008009>, IMessage, IEquatable<ExamineElixirStepResp_1008009>, IDeepCloneable<ExamineElixirStepResp_1008009>
{
	[DebuggerNonUserCode]
	public static class Types
	{
		public sealed class ElixirStepObjectResp_1008010 : IMessage<ElixirStepObjectResp_1008010>, IMessage, IEquatable<ElixirStepObjectResp_1008010>, IDeepCloneable<ElixirStepObjectResp_1008010>
		{
			private static readonly MessageParser<ElixirStepObjectResp_1008010> _parser = new MessageParser<ElixirStepObjectResp_1008010>(() => new ElixirStepObjectResp_1008010());

			private UnknownFieldSet _unknownFields;

			public const int ActionTypeFieldNumber = 1;

			private int actionType_;

			public const int TargetTypeFieldNumber = 2;

			private int targetType_;

			public const int OrderFieldNumber = 3;

			private int order_;

			public const int ContinueTimeFieldNumber = 4;

			private int continueTime_;

			public const int BestValFieldNumber = 5;

			private int bestVal_;

			public const int MinValFieldNumber = 6;

			private int minVal_;

			public const int MaxValFieldNumber = 7;

			private int maxVal_;

			public const int ValTypeFieldNumber = 8;

			private int valType_;

			[DebuggerNonUserCode]
			public static MessageParser<ElixirStepObjectResp_1008010> Parser => _parser;

			[DebuggerNonUserCode]
			public static MessageDescriptor Descriptor => ExamineElixirStepResp_1008009.Descriptor.NestedTypes[0];

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

			[DebuggerNonUserCode]
			public int ContinueTime
			{
				get
				{
					return continueTime_;
				}
				set
				{
					continueTime_ = value;
				}
			}

			[DebuggerNonUserCode]
			public int BestVal
			{
				get
				{
					return bestVal_;
				}
				set
				{
					bestVal_ = value;
				}
			}

			[DebuggerNonUserCode]
			public int MinVal
			{
				get
				{
					return minVal_;
				}
				set
				{
					minVal_ = value;
				}
			}

			[DebuggerNonUserCode]
			public int MaxVal
			{
				get
				{
					return maxVal_;
				}
				set
				{
					maxVal_ = value;
				}
			}

			[DebuggerNonUserCode]
			public int ValType
			{
				get
				{
					return valType_;
				}
				set
				{
					valType_ = value;
				}
			}

			private MessageDescriptor pb_003A_003AGoogle_002EProtobuf_002EIMessage_002Eget_Descriptor()
			{
				return Descriptor;
			}

			[DebuggerNonUserCode]
			public ElixirStepObjectResp_1008010()
			{
			}

			[DebuggerNonUserCode]
			public ElixirStepObjectResp_1008010(ElixirStepObjectResp_1008010 other)
				: this()
			{
				actionType_ = other.actionType_;
				targetType_ = other.targetType_;
				order_ = other.order_;
				continueTime_ = other.continueTime_;
				bestVal_ = other.bestVal_;
				minVal_ = other.minVal_;
				maxVal_ = other.maxVal_;
				valType_ = other.valType_;
				_unknownFields = UnknownFieldSet.Clone(other._unknownFields);
			}

			[DebuggerNonUserCode]
			public ElixirStepObjectResp_1008010 Clone()
			{
				return new ElixirStepObjectResp_1008010(this);
			}

			[DebuggerNonUserCode]
			public override bool Equals(object other)
			{
				return Equals(other as ElixirStepObjectResp_1008010);
			}

			[DebuggerNonUserCode]
			public bool Equals(ElixirStepObjectResp_1008010 other)
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
				if (Order != other.Order)
				{
					return false;
				}
				if (ContinueTime != other.ContinueTime)
				{
					return false;
				}
				if (BestVal != other.BestVal)
				{
					return false;
				}
				if (MinVal != other.MinVal)
				{
					return false;
				}
				if (MaxVal != other.MaxVal)
				{
					return false;
				}
				if (ValType != other.ValType)
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
				if (Order != 0)
				{
					num ^= Order.GetHashCode();
				}
				if (ContinueTime != 0)
				{
					num ^= ContinueTime.GetHashCode();
				}
				if (BestVal != 0)
				{
					num ^= BestVal.GetHashCode();
				}
				if (MinVal != 0)
				{
					num ^= MinVal.GetHashCode();
				}
				if (MaxVal != 0)
				{
					num ^= MaxVal.GetHashCode();
				}
				if (ValType != 0)
				{
					num ^= ValType.GetHashCode();
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
				if (Order != 0)
				{
					output.WriteRawTag(24);
					output.WriteInt32(Order);
				}
				if (ContinueTime != 0)
				{
					output.WriteRawTag(32);
					output.WriteInt32(ContinueTime);
				}
				if (BestVal != 0)
				{
					output.WriteRawTag(40);
					output.WriteInt32(BestVal);
				}
				if (MinVal != 0)
				{
					output.WriteRawTag(48);
					output.WriteInt32(MinVal);
				}
				if (MaxVal != 0)
				{
					output.WriteRawTag(56);
					output.WriteInt32(MaxVal);
				}
				if (ValType != 0)
				{
					output.WriteRawTag(64);
					output.WriteInt32(ValType);
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
				if (Order != 0)
				{
					num += 1 + CodedOutputStream.ComputeInt32Size(Order);
				}
				if (ContinueTime != 0)
				{
					num += 1 + CodedOutputStream.ComputeInt32Size(ContinueTime);
				}
				if (BestVal != 0)
				{
					num += 1 + CodedOutputStream.ComputeInt32Size(BestVal);
				}
				if (MinVal != 0)
				{
					num += 1 + CodedOutputStream.ComputeInt32Size(MinVal);
				}
				if (MaxVal != 0)
				{
					num += 1 + CodedOutputStream.ComputeInt32Size(MaxVal);
				}
				if (ValType != 0)
				{
					num += 1 + CodedOutputStream.ComputeInt32Size(ValType);
				}
				if (_unknownFields != null)
				{
					num += _unknownFields.CalculateSize();
				}
				return num;
			}

			[DebuggerNonUserCode]
			public void MergeFrom(ElixirStepObjectResp_1008010 other)
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
					if (other.Order != 0)
					{
						Order = other.Order;
					}
					if (other.ContinueTime != 0)
					{
						ContinueTime = other.ContinueTime;
					}
					if (other.BestVal != 0)
					{
						BestVal = other.BestVal;
					}
					if (other.MinVal != 0)
					{
						MinVal = other.MinVal;
					}
					if (other.MaxVal != 0)
					{
						MaxVal = other.MaxVal;
					}
					if (other.ValType != 0)
					{
						ValType = other.ValType;
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
					case 24u:
						Order = input.ReadInt32();
						break;
					case 32u:
						ContinueTime = input.ReadInt32();
						break;
					case 40u:
						BestVal = input.ReadInt32();
						break;
					case 48u:
						MinVal = input.ReadInt32();
						break;
					case 56u:
						MaxVal = input.ReadInt32();
						break;
					case 64u:
						ValType = input.ReadInt32();
						break;
					}
				}
			}
		}
	}

	private static readonly MessageParser<ExamineElixirStepResp_1008009> _parser = new MessageParser<ExamineElixirStepResp_1008009>(() => new ExamineElixirStepResp_1008009());

	private UnknownFieldSet _unknownFields;

	public const int CodeFieldNumber = 1;

	private int code_;

	public const int IsSuccessFieldNumber = 2;

	private bool isSuccess_;

	public const int ObjectFieldNumber = 3;

	private static readonly FieldCodec<Types.ElixirStepObjectResp_1008010> _repeated_object_codec = FieldCodec.ForMessage(26u, Types.ElixirStepObjectResp_1008010.Parser);

	private readonly RepeatedField<Types.ElixirStepObjectResp_1008010> object_ = new RepeatedField<Types.ElixirStepObjectResp_1008010>();

	[DebuggerNonUserCode]
	public static MessageParser<ExamineElixirStepResp_1008009> Parser => _parser;

	[DebuggerNonUserCode]
	public static MessageDescriptor Descriptor => UnrealWorldProtoReflection.Descriptor.MessageTypes[8];

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
	public RepeatedField<Types.ElixirStepObjectResp_1008010> Object => object_;

	private MessageDescriptor pb_003A_003AGoogle_002EProtobuf_002EIMessage_002Eget_Descriptor()
	{
		return Descriptor;
	}

	[DebuggerNonUserCode]
	public ExamineElixirStepResp_1008009()
	{
	}

	[DebuggerNonUserCode]
	public ExamineElixirStepResp_1008009(ExamineElixirStepResp_1008009 other)
		: this()
	{
		code_ = other.code_;
		isSuccess_ = other.isSuccess_;
		object_ = other.object_.Clone();
		_unknownFields = UnknownFieldSet.Clone(other._unknownFields);
	}

	[DebuggerNonUserCode]
	public ExamineElixirStepResp_1008009 Clone()
	{
		return new ExamineElixirStepResp_1008009(this);
	}

	[DebuggerNonUserCode]
	public override bool Equals(object other)
	{
		return Equals(other as ExamineElixirStepResp_1008009);
	}

	[DebuggerNonUserCode]
	public bool Equals(ExamineElixirStepResp_1008009 other)
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
	public void MergeFrom(ExamineElixirStepResp_1008009 other)
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
