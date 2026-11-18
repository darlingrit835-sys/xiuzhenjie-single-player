using System;
using System.Diagnostics;
using Google.Protobuf;
using Google.Protobuf.Collections;
using Google.Protobuf.Reflection;

public sealed class RefiningElixirReq_1005027 : IMessage<RefiningElixirReq_1005027>, IMessage, IEquatable<RefiningElixirReq_1005027>, IDeepCloneable<RefiningElixirReq_1005027>
{
	[DebuggerNonUserCode]
	public static class Types
	{
		public sealed class StepDetailReq_1005028 : IMessage<StepDetailReq_1005028>, IMessage, IEquatable<StepDetailReq_1005028>, IDeepCloneable<StepDetailReq_1005028>
		{
			private static readonly MessageParser<StepDetailReq_1005028> _parser = new MessageParser<StepDetailReq_1005028>(() => new StepDetailReq_1005028());

			private UnknownFieldSet _unknownFields;

			public const int StepIdFieldNumber = 1;

			private string stepId_ = "";

			public const int GoodsIdFieldNumber = 2;

			private string goodsId_ = "";

			public const int BestTimeFieldNumber = 3;

			private double bestTime_;

			[DebuggerNonUserCode]
			public static MessageParser<StepDetailReq_1005028> Parser => _parser;

			[DebuggerNonUserCode]
			public static MessageDescriptor Descriptor => RefiningElixirReq_1005027.Descriptor.NestedTypes[0];

			[DebuggerNonUserCode]
			MessageDescriptor IMessage.Descriptor => pb_003A_003AGoogle_002EProtobuf_002EIMessage_002Eget_Descriptor();

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
			public string GoodsId
			{
				get
				{
					return goodsId_;
				}
				set
				{
					goodsId_ = ProtoPreconditions.CheckNotNull(value, "value");
				}
			}

			[DebuggerNonUserCode]
			public double BestTime
			{
				get
				{
					return bestTime_;
				}
				set
				{
					bestTime_ = value;
				}
			}

			private MessageDescriptor pb_003A_003AGoogle_002EProtobuf_002EIMessage_002Eget_Descriptor()
			{
				return Descriptor;
			}

			[DebuggerNonUserCode]
			public StepDetailReq_1005028()
			{
			}

			[DebuggerNonUserCode]
			public StepDetailReq_1005028(StepDetailReq_1005028 other)
				: this()
			{
				stepId_ = other.stepId_;
				goodsId_ = other.goodsId_;
				bestTime_ = other.bestTime_;
				_unknownFields = UnknownFieldSet.Clone(other._unknownFields);
			}

			[DebuggerNonUserCode]
			public StepDetailReq_1005028 Clone()
			{
				return new StepDetailReq_1005028(this);
			}

			[DebuggerNonUserCode]
			public override bool Equals(object other)
			{
				return Equals(other as StepDetailReq_1005028);
			}

			[DebuggerNonUserCode]
			public bool Equals(StepDetailReq_1005028 other)
			{
				if (other == null)
				{
					return false;
				}
				if (other == this)
				{
					return true;
				}
				if (StepId != other.StepId)
				{
					return false;
				}
				if (GoodsId != other.GoodsId)
				{
					return false;
				}
				if (!ProtobufEqualityComparers.BitwiseDoubleEqualityComparer.Equals(BestTime, other.BestTime))
				{
					return false;
				}
				return object.Equals(_unknownFields, other._unknownFields);
			}

			[DebuggerNonUserCode]
			public override int GetHashCode()
			{
				int num = 1;
				if (StepId.Length != 0)
				{
					num ^= StepId.GetHashCode();
				}
				if (GoodsId.Length != 0)
				{
					num ^= GoodsId.GetHashCode();
				}
				if (BestTime != 0.0)
				{
					num ^= ProtobufEqualityComparers.BitwiseDoubleEqualityComparer.GetHashCode(BestTime);
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
				if (StepId.Length != 0)
				{
					output.WriteRawTag(10);
					output.WriteString(StepId);
				}
				if (GoodsId.Length != 0)
				{
					output.WriteRawTag(18);
					output.WriteString(GoodsId);
				}
				if (BestTime != 0.0)
				{
					output.WriteRawTag(25);
					output.WriteDouble(BestTime);
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
				if (StepId.Length != 0)
				{
					num += 1 + CodedOutputStream.ComputeStringSize(StepId);
				}
				if (GoodsId.Length != 0)
				{
					num += 1 + CodedOutputStream.ComputeStringSize(GoodsId);
				}
				if (BestTime != 0.0)
				{
					num += 9;
				}
				if (_unknownFields != null)
				{
					num += _unknownFields.CalculateSize();
				}
				return num;
			}

			[DebuggerNonUserCode]
			public void MergeFrom(StepDetailReq_1005028 other)
			{
				if (other != null)
				{
					if (other.StepId.Length != 0)
					{
						StepId = other.StepId;
					}
					if (other.GoodsId.Length != 0)
					{
						GoodsId = other.GoodsId;
					}
					if (other.BestTime != 0.0)
					{
						BestTime = other.BestTime;
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
						StepId = input.ReadString();
						break;
					case 18u:
						GoodsId = input.ReadString();
						break;
					case 25u:
						BestTime = input.ReadDouble();
						break;
					}
				}
			}
		}
	}

	private static readonly MessageParser<RefiningElixirReq_1005027> _parser = new MessageParser<RefiningElixirReq_1005027>(() => new RefiningElixirReq_1005027());

	private UnknownFieldSet _unknownFields;

	public const int MethodElixirIdFieldNumber = 1;

	private string methodElixirId_ = "";

	public const int IsRefiningSuccessFieldNumber = 2;

	private bool isRefiningSuccess_;

	public const int StepDetailListFieldNumber = 3;

	private static readonly FieldCodec<Types.StepDetailReq_1005028> _repeated_stepDetailList_codec = FieldCodec.ForMessage(26u, Types.StepDetailReq_1005028.Parser);

	private readonly RepeatedField<Types.StepDetailReq_1005028> stepDetailList_ = new RepeatedField<Types.StepDetailReq_1005028>();

	[DebuggerNonUserCode]
	public static MessageParser<RefiningElixirReq_1005027> Parser => _parser;

	[DebuggerNonUserCode]
	public static MessageDescriptor Descriptor => HomeProtoReflection.Descriptor.MessageTypes[35];

	[DebuggerNonUserCode]
	MessageDescriptor IMessage.Descriptor => pb_003A_003AGoogle_002EProtobuf_002EIMessage_002Eget_Descriptor();

	[DebuggerNonUserCode]
	public string MethodElixirId
	{
		get
		{
			return methodElixirId_;
		}
		set
		{
			methodElixirId_ = ProtoPreconditions.CheckNotNull(value, "value");
		}
	}

	[DebuggerNonUserCode]
	public bool IsRefiningSuccess
	{
		get
		{
			return isRefiningSuccess_;
		}
		set
		{
			isRefiningSuccess_ = value;
		}
	}

	[DebuggerNonUserCode]
	public RepeatedField<Types.StepDetailReq_1005028> StepDetailList => stepDetailList_;

	private MessageDescriptor pb_003A_003AGoogle_002EProtobuf_002EIMessage_002Eget_Descriptor()
	{
		return Descriptor;
	}

	[DebuggerNonUserCode]
	public RefiningElixirReq_1005027()
	{
	}

	[DebuggerNonUserCode]
	public RefiningElixirReq_1005027(RefiningElixirReq_1005027 other)
		: this()
	{
		methodElixirId_ = other.methodElixirId_;
		isRefiningSuccess_ = other.isRefiningSuccess_;
		stepDetailList_ = other.stepDetailList_.Clone();
		_unknownFields = UnknownFieldSet.Clone(other._unknownFields);
	}

	[DebuggerNonUserCode]
	public RefiningElixirReq_1005027 Clone()
	{
		return new RefiningElixirReq_1005027(this);
	}

	[DebuggerNonUserCode]
	public override bool Equals(object other)
	{
		return Equals(other as RefiningElixirReq_1005027);
	}

	[DebuggerNonUserCode]
	public bool Equals(RefiningElixirReq_1005027 other)
	{
		if (other == null)
		{
			return false;
		}
		if (other == this)
		{
			return true;
		}
		if (MethodElixirId != other.MethodElixirId)
		{
			return false;
		}
		if (IsRefiningSuccess != other.IsRefiningSuccess)
		{
			return false;
		}
		if (!stepDetailList_.Equals(other.stepDetailList_))
		{
			return false;
		}
		return object.Equals(_unknownFields, other._unknownFields);
	}

	[DebuggerNonUserCode]
	public override int GetHashCode()
	{
		int num = 1;
		if (MethodElixirId.Length != 0)
		{
			num ^= MethodElixirId.GetHashCode();
		}
		if (IsRefiningSuccess)
		{
			num ^= IsRefiningSuccess.GetHashCode();
		}
		num ^= stepDetailList_.GetHashCode();
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
		if (MethodElixirId.Length != 0)
		{
			output.WriteRawTag(10);
			output.WriteString(MethodElixirId);
		}
		if (IsRefiningSuccess)
		{
			output.WriteRawTag(16);
			output.WriteBool(IsRefiningSuccess);
		}
		stepDetailList_.WriteTo(output, _repeated_stepDetailList_codec);
		if (_unknownFields != null)
		{
			_unknownFields.WriteTo(output);
		}
	}

	[DebuggerNonUserCode]
	public int CalculateSize()
	{
		int num = 0;
		if (MethodElixirId.Length != 0)
		{
			num += 1 + CodedOutputStream.ComputeStringSize(MethodElixirId);
		}
		if (IsRefiningSuccess)
		{
			num += 2;
		}
		num += stepDetailList_.CalculateSize(_repeated_stepDetailList_codec);
		if (_unknownFields != null)
		{
			num += _unknownFields.CalculateSize();
		}
		return num;
	}

	[DebuggerNonUserCode]
	public void MergeFrom(RefiningElixirReq_1005027 other)
	{
		if (other != null)
		{
			if (other.MethodElixirId.Length != 0)
			{
				MethodElixirId = other.MethodElixirId;
			}
			if (other.IsRefiningSuccess)
			{
				IsRefiningSuccess = other.IsRefiningSuccess;
			}
			stepDetailList_.Add(other.stepDetailList_);
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
				MethodElixirId = input.ReadString();
				break;
			case 16u:
				IsRefiningSuccess = input.ReadBool();
				break;
			case 26u:
				stepDetailList_.AddEntriesFrom(input, _repeated_stepDetailList_codec);
				break;
			}
		}
	}
}
