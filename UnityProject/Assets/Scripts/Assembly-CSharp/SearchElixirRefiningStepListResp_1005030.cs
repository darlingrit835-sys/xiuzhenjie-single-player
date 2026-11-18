using System;
using System.Diagnostics;
using Google.Protobuf;
using Google.Protobuf.Collections;
using Google.Protobuf.Reflection;

public sealed class SearchElixirRefiningStepListResp_1005030 : IMessage<SearchElixirRefiningStepListResp_1005030>, IMessage, IEquatable<SearchElixirRefiningStepListResp_1005030>, IDeepCloneable<SearchElixirRefiningStepListResp_1005030>
{
	[DebuggerNonUserCode]
	public static class Types
	{
		public sealed class ElixirRefiningStepResp_1005031 : IMessage<ElixirRefiningStepResp_1005031>, IMessage, IEquatable<ElixirRefiningStepResp_1005031>, IDeepCloneable<ElixirRefiningStepResp_1005031>
		{
			private static readonly MessageParser<ElixirRefiningStepResp_1005031> _parser = new MessageParser<ElixirRefiningStepResp_1005031>(() => new ElixirRefiningStepResp_1005031());

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

			public const int StepIdFieldNumber = 9;

			private string stepId_ = "";

			public const int TargetNameFieldNumber = 10;

			private string targetName_ = "";

			public const int MapIdFieldNumber = 11;

			private string mapId_ = "";

			public const int MapNameFieldNumber = 12;

			private string mapName_ = "";

			public const int TargetIdFieldNumber = 13;

			private string targetId_ = "";

			[DebuggerNonUserCode]
			public static MessageParser<ElixirRefiningStepResp_1005031> Parser => _parser;

			[DebuggerNonUserCode]
			public static MessageDescriptor Descriptor => SearchElixirRefiningStepListResp_1005030.Descriptor.NestedTypes[0];

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

			[DebuggerNonUserCode]
			public string StepId
			{
				get
				{
					return stepId_;
				}
				set
				{
					stepId_ = ProtoPreconditions.CheckNotNull(value, "value");
				}
			}

			[DebuggerNonUserCode]
			public string TargetName
			{
				get
				{
					return targetName_;
				}
				set
				{
					targetName_ = ProtoPreconditions.CheckNotNull(value, "value");
				}
			}

			[DebuggerNonUserCode]
			public string MapId
			{
				get
				{
					return mapId_;
				}
				set
				{
					mapId_ = ProtoPreconditions.CheckNotNull(value, "value");
				}
			}

			[DebuggerNonUserCode]
			public string MapName
			{
				get
				{
					return mapName_;
				}
				set
				{
					mapName_ = ProtoPreconditions.CheckNotNull(value, "value");
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

			private MessageDescriptor pb_003A_003AGoogle_002EProtobuf_002EIMessage_002Eget_Descriptor()
			{
				return Descriptor;
			}

			[DebuggerNonUserCode]
			public ElixirRefiningStepResp_1005031()
			{
			}

			[DebuggerNonUserCode]
			public ElixirRefiningStepResp_1005031(ElixirRefiningStepResp_1005031 other)
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
				stepId_ = other.stepId_;
				targetName_ = other.targetName_;
				mapId_ = other.mapId_;
				mapName_ = other.mapName_;
				targetId_ = other.targetId_;
				_unknownFields = UnknownFieldSet.Clone(other._unknownFields);
			}

			[DebuggerNonUserCode]
			public ElixirRefiningStepResp_1005031 Clone()
			{
				return new ElixirRefiningStepResp_1005031(this);
			}

			[DebuggerNonUserCode]
			public override bool Equals(object other)
			{
				return Equals(other as ElixirRefiningStepResp_1005031);
			}

			[DebuggerNonUserCode]
			public bool Equals(ElixirRefiningStepResp_1005031 other)
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
				if (StepId != other.StepId)
				{
					return false;
				}
				if (TargetName != other.TargetName)
				{
					return false;
				}
				if (MapId != other.MapId)
				{
					return false;
				}
				if (MapName != other.MapName)
				{
					return false;
				}
				if (TargetId != other.TargetId)
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
				if (StepId.Length != 0)
				{
					num ^= StepId.GetHashCode();
				}
				if (TargetName.Length != 0)
				{
					num ^= TargetName.GetHashCode();
				}
				if (MapId.Length != 0)
				{
					num ^= MapId.GetHashCode();
				}
				if (MapName.Length != 0)
				{
					num ^= MapName.GetHashCode();
				}
				if (TargetId.Length != 0)
				{
					num ^= TargetId.GetHashCode();
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
				if (StepId.Length != 0)
				{
					output.WriteRawTag(74);
					output.WriteString(StepId);
				}
				if (TargetName.Length != 0)
				{
					output.WriteRawTag(82);
					output.WriteString(TargetName);
				}
				if (MapId.Length != 0)
				{
					output.WriteRawTag(90);
					output.WriteString(MapId);
				}
				if (MapName.Length != 0)
				{
					output.WriteRawTag(98);
					output.WriteString(MapName);
				}
				if (TargetId.Length != 0)
				{
					output.WriteRawTag(106);
					output.WriteString(TargetId);
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
				if (StepId.Length != 0)
				{
					num += 1 + CodedOutputStream.ComputeStringSize(StepId);
				}
				if (TargetName.Length != 0)
				{
					num += 1 + CodedOutputStream.ComputeStringSize(TargetName);
				}
				if (MapId.Length != 0)
				{
					num += 1 + CodedOutputStream.ComputeStringSize(MapId);
				}
				if (MapName.Length != 0)
				{
					num += 1 + CodedOutputStream.ComputeStringSize(MapName);
				}
				if (TargetId.Length != 0)
				{
					num += 1 + CodedOutputStream.ComputeStringSize(TargetId);
				}
				if (_unknownFields != null)
				{
					num += _unknownFields.CalculateSize();
				}
				return num;
			}

			[DebuggerNonUserCode]
			public void MergeFrom(ElixirRefiningStepResp_1005031 other)
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
					if (other.StepId.Length != 0)
					{
						StepId = other.StepId;
					}
					if (other.TargetName.Length != 0)
					{
						TargetName = other.TargetName;
					}
					if (other.MapId.Length != 0)
					{
						MapId = other.MapId;
					}
					if (other.MapName.Length != 0)
					{
						MapName = other.MapName;
					}
					if (other.TargetId.Length != 0)
					{
						TargetId = other.TargetId;
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
					case 74u:
						StepId = input.ReadString();
						break;
					case 82u:
						TargetName = input.ReadString();
						break;
					case 90u:
						MapId = input.ReadString();
						break;
					case 98u:
						MapName = input.ReadString();
						break;
					case 106u:
						TargetId = input.ReadString();
						break;
					}
				}
			}
		}
	}

	private static readonly MessageParser<SearchElixirRefiningStepListResp_1005030> _parser = new MessageParser<SearchElixirRefiningStepListResp_1005030>(() => new SearchElixirRefiningStepListResp_1005030());

	private UnknownFieldSet _unknownFields;

	public const int CodeFieldNumber = 1;

	private int code_;

	public const int IsSuccessFieldNumber = 2;

	private bool isSuccess_;

	public const int StepFieldNumber = 3;

	private static readonly FieldCodec<Types.ElixirRefiningStepResp_1005031> _repeated_step_codec = FieldCodec.ForMessage(26u, Types.ElixirRefiningStepResp_1005031.Parser);

	private readonly RepeatedField<Types.ElixirRefiningStepResp_1005031> step_ = new RepeatedField<Types.ElixirRefiningStepResp_1005031>();

	public const int RankFieldNumber = 4;

	private int rank_;

	[DebuggerNonUserCode]
	public static MessageParser<SearchElixirRefiningStepListResp_1005030> Parser => _parser;

	[DebuggerNonUserCode]
	public static MessageDescriptor Descriptor => HomeProtoReflection.Descriptor.MessageTypes[40];

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
	public RepeatedField<Types.ElixirRefiningStepResp_1005031> Step => step_;

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
	public SearchElixirRefiningStepListResp_1005030()
	{
	}

	[DebuggerNonUserCode]
	public SearchElixirRefiningStepListResp_1005030(SearchElixirRefiningStepListResp_1005030 other)
		: this()
	{
		code_ = other.code_;
		isSuccess_ = other.isSuccess_;
		step_ = other.step_.Clone();
		rank_ = other.rank_;
		_unknownFields = UnknownFieldSet.Clone(other._unknownFields);
	}

	[DebuggerNonUserCode]
	public SearchElixirRefiningStepListResp_1005030 Clone()
	{
		return new SearchElixirRefiningStepListResp_1005030(this);
	}

	[DebuggerNonUserCode]
	public override bool Equals(object other)
	{
		return Equals(other as SearchElixirRefiningStepListResp_1005030);
	}

	[DebuggerNonUserCode]
	public bool Equals(SearchElixirRefiningStepListResp_1005030 other)
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
		if (!step_.Equals(other.step_))
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
		if (Code != 0)
		{
			num ^= Code.GetHashCode();
		}
		if (IsSuccess)
		{
			num ^= IsSuccess.GetHashCode();
		}
		num ^= step_.GetHashCode();
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
		step_.WriteTo(output, _repeated_step_codec);
		if (Rank != 0)
		{
			output.WriteRawTag(32);
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
		if (Code != 0)
		{
			num += 1 + CodedOutputStream.ComputeInt32Size(Code);
		}
		if (IsSuccess)
		{
			num += 2;
		}
		num += step_.CalculateSize(_repeated_step_codec);
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
	public void MergeFrom(SearchElixirRefiningStepListResp_1005030 other)
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
			step_.Add(other.step_);
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
			case 8u:
				Code = input.ReadInt32();
				break;
			case 16u:
				IsSuccess = input.ReadBool();
				break;
			case 26u:
				step_.AddEntriesFrom(input, _repeated_step_codec);
				break;
			case 32u:
				Rank = input.ReadInt32();
				break;
			}
		}
	}
}
