using System;
using System.Diagnostics;
using Google.Protobuf;
using Google.Protobuf.Collections;
using Google.Protobuf.Reflection;

public sealed class EnterPlantResp_1013001 : IMessage<EnterPlantResp_1013001>, IMessage, IEquatable<EnterPlantResp_1013001>, IDeepCloneable<EnterPlantResp_1013001>
{
	[DebuggerNonUserCode]
	public static class Types
	{
		public sealed class LandResp_1013002 : IMessage<LandResp_1013002>, IMessage, IEquatable<LandResp_1013002>, IDeepCloneable<LandResp_1013002>
		{
			private static readonly MessageParser<LandResp_1013002> _parser = new MessageParser<LandResp_1013002>(() => new LandResp_1013002());

			private UnknownFieldSet _unknownFields;

			public const int IdFieldNumber = 1;

			private int id_;

			public const int StatusFieldNumber = 2;

			private int status_;

			public const int TimeRemainingFieldNumber = 3;

			private long timeRemaining_;

			public const int TotalWasteTimeFieldNumber = 4;

			private long totalWasteTime_;

			public const int PlantIdFieldNumber = 5;

			private string plantId_ = "";

			public const int PlantNameFieldNumber = 6;

			private string plantName_ = "";

			[DebuggerNonUserCode]
			public static MessageParser<LandResp_1013002> Parser => _parser;

			[DebuggerNonUserCode]
			public static MessageDescriptor Descriptor => EnterPlantResp_1013001.Descriptor.NestedTypes[0];

			[DebuggerNonUserCode]
			MessageDescriptor IMessage.Descriptor => pb_003A_003AGoogle_002EProtobuf_002EIMessage_002Eget_Descriptor();

			[DebuggerNonUserCode]
			public int Id
			{
				get
				{
					return id_;
				}
				set
				{
					id_ = value;
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
			public long TimeRemaining
			{
				get
				{
					return timeRemaining_;
				}
				set
				{
					timeRemaining_ = value;
				}
			}

			[DebuggerNonUserCode]
			public long TotalWasteTime
			{
				get
				{
					return totalWasteTime_;
				}
				set
				{
					totalWasteTime_ = value;
				}
			}

			[DebuggerNonUserCode]
			public string PlantId
			{
				get
				{
					return plantId_;
				}
				set
				{
					plantId_ = ProtoPreconditions.CheckNotNull(value, "value");
				}
			}

			[DebuggerNonUserCode]
			public string PlantName
			{
				get
				{
					return plantName_;
				}
				set
				{
					plantName_ = ProtoPreconditions.CheckNotNull(value, "value");
				}
			}

			private MessageDescriptor pb_003A_003AGoogle_002EProtobuf_002EIMessage_002Eget_Descriptor()
			{
				return Descriptor;
			}

			[DebuggerNonUserCode]
			public LandResp_1013002()
			{
			}

			[DebuggerNonUserCode]
			public LandResp_1013002(LandResp_1013002 other)
				: this()
			{
				id_ = other.id_;
				status_ = other.status_;
				timeRemaining_ = other.timeRemaining_;
				totalWasteTime_ = other.totalWasteTime_;
				plantId_ = other.plantId_;
				plantName_ = other.plantName_;
				_unknownFields = UnknownFieldSet.Clone(other._unknownFields);
			}

			[DebuggerNonUserCode]
			public LandResp_1013002 Clone()
			{
				return new LandResp_1013002(this);
			}

			[DebuggerNonUserCode]
			public override bool Equals(object other)
			{
				return Equals(other as LandResp_1013002);
			}

			[DebuggerNonUserCode]
			public bool Equals(LandResp_1013002 other)
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
				if (Status != other.Status)
				{
					return false;
				}
				if (TimeRemaining != other.TimeRemaining)
				{
					return false;
				}
				if (TotalWasteTime != other.TotalWasteTime)
				{
					return false;
				}
				if (PlantId != other.PlantId)
				{
					return false;
				}
				if (PlantName != other.PlantName)
				{
					return false;
				}
				return object.Equals(_unknownFields, other._unknownFields);
			}

			[DebuggerNonUserCode]
			public override int GetHashCode()
			{
				int num = 1;
				if (Id != 0)
				{
					num ^= Id.GetHashCode();
				}
				if (Status != 0)
				{
					num ^= Status.GetHashCode();
				}
				if (TimeRemaining != 0L)
				{
					num ^= TimeRemaining.GetHashCode();
				}
				if (TotalWasteTime != 0L)
				{
					num ^= TotalWasteTime.GetHashCode();
				}
				if (PlantId.Length != 0)
				{
					num ^= PlantId.GetHashCode();
				}
				if (PlantName.Length != 0)
				{
					num ^= PlantName.GetHashCode();
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
				if (Id != 0)
				{
					output.WriteRawTag(8);
					output.WriteInt32(Id);
				}
				if (Status != 0)
				{
					output.WriteRawTag(16);
					output.WriteInt32(Status);
				}
				if (TimeRemaining != 0L)
				{
					output.WriteRawTag(24);
					output.WriteInt64(TimeRemaining);
				}
				if (TotalWasteTime != 0L)
				{
					output.WriteRawTag(32);
					output.WriteInt64(TotalWasteTime);
				}
				if (PlantId.Length != 0)
				{
					output.WriteRawTag(42);
					output.WriteString(PlantId);
				}
				if (PlantName.Length != 0)
				{
					output.WriteRawTag(50);
					output.WriteString(PlantName);
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
				if (Id != 0)
				{
					num += 1 + CodedOutputStream.ComputeInt32Size(Id);
				}
				if (Status != 0)
				{
					num += 1 + CodedOutputStream.ComputeInt32Size(Status);
				}
				if (TimeRemaining != 0L)
				{
					num += 1 + CodedOutputStream.ComputeInt64Size(TimeRemaining);
				}
				if (TotalWasteTime != 0L)
				{
					num += 1 + CodedOutputStream.ComputeInt64Size(TotalWasteTime);
				}
				if (PlantId.Length != 0)
				{
					num += 1 + CodedOutputStream.ComputeStringSize(PlantId);
				}
				if (PlantName.Length != 0)
				{
					num += 1 + CodedOutputStream.ComputeStringSize(PlantName);
				}
				if (_unknownFields != null)
				{
					num += _unknownFields.CalculateSize();
				}
				return num;
			}

			[DebuggerNonUserCode]
			public void MergeFrom(LandResp_1013002 other)
			{
				if (other != null)
				{
					if (other.Id != 0)
					{
						Id = other.Id;
					}
					if (other.Status != 0)
					{
						Status = other.Status;
					}
					if (other.TimeRemaining != 0L)
					{
						TimeRemaining = other.TimeRemaining;
					}
					if (other.TotalWasteTime != 0L)
					{
						TotalWasteTime = other.TotalWasteTime;
					}
					if (other.PlantId.Length != 0)
					{
						PlantId = other.PlantId;
					}
					if (other.PlantName.Length != 0)
					{
						PlantName = other.PlantName;
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
						Id = input.ReadInt32();
						break;
					case 16u:
						Status = input.ReadInt32();
						break;
					case 24u:
						TimeRemaining = input.ReadInt64();
						break;
					case 32u:
						TotalWasteTime = input.ReadInt64();
						break;
					case 42u:
						PlantId = input.ReadString();
						break;
					case 50u:
						PlantName = input.ReadString();
						break;
					}
				}
			}
		}
	}

	private static readonly MessageParser<EnterPlantResp_1013001> _parser = new MessageParser<EnterPlantResp_1013001>(() => new EnterPlantResp_1013001());

	private UnknownFieldSet _unknownFields;

	public const int CodeFieldNumber = 1;

	private int code_;

	public const int IsSuccessFieldNumber = 2;

	private bool isSuccess_;

	public const int LandFieldNumber = 3;

	private static readonly FieldCodec<Types.LandResp_1013002> _repeated_land_codec = FieldCodec.ForMessage(26u, Types.LandResp_1013002.Parser);

	private readonly RepeatedField<Types.LandResp_1013002> land_ = new RepeatedField<Types.LandResp_1013002>();

	public const int IsOneKeyKaiKenFieldNumber = 4;

	private bool isOneKeyKaiKen_;

	[DebuggerNonUserCode]
	public static MessageParser<EnterPlantResp_1013001> Parser => _parser;

	[DebuggerNonUserCode]
	public static MessageDescriptor Descriptor => PlantProtoReflection.Descriptor.MessageTypes[0];

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
	public RepeatedField<Types.LandResp_1013002> Land => land_;

	[DebuggerNonUserCode]
	public bool IsOneKeyKaiKen
	{
		get
		{
			return isOneKeyKaiKen_;
		}
		set
		{
			isOneKeyKaiKen_ = value;
		}
	}

	private MessageDescriptor pb_003A_003AGoogle_002EProtobuf_002EIMessage_002Eget_Descriptor()
	{
		return Descriptor;
	}

	[DebuggerNonUserCode]
	public EnterPlantResp_1013001()
	{
	}

	[DebuggerNonUserCode]
	public EnterPlantResp_1013001(EnterPlantResp_1013001 other)
		: this()
	{
		code_ = other.code_;
		isSuccess_ = other.isSuccess_;
		land_ = other.land_.Clone();
		isOneKeyKaiKen_ = other.isOneKeyKaiKen_;
		_unknownFields = UnknownFieldSet.Clone(other._unknownFields);
	}

	[DebuggerNonUserCode]
	public EnterPlantResp_1013001 Clone()
	{
		return new EnterPlantResp_1013001(this);
	}

	[DebuggerNonUserCode]
	public override bool Equals(object other)
	{
		return Equals(other as EnterPlantResp_1013001);
	}

	[DebuggerNonUserCode]
	public bool Equals(EnterPlantResp_1013001 other)
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
		if (!land_.Equals(other.land_))
		{
			return false;
		}
		if (IsOneKeyKaiKen != other.IsOneKeyKaiKen)
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
		num ^= land_.GetHashCode();
		if (IsOneKeyKaiKen)
		{
			num ^= IsOneKeyKaiKen.GetHashCode();
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
		land_.WriteTo(output, _repeated_land_codec);
		if (IsOneKeyKaiKen)
		{
			output.WriteRawTag(32);
			output.WriteBool(IsOneKeyKaiKen);
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
		num += land_.CalculateSize(_repeated_land_codec);
		if (IsOneKeyKaiKen)
		{
			num += 2;
		}
		if (_unknownFields != null)
		{
			num += _unknownFields.CalculateSize();
		}
		return num;
	}

	[DebuggerNonUserCode]
	public void MergeFrom(EnterPlantResp_1013001 other)
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
			land_.Add(other.land_);
			if (other.IsOneKeyKaiKen)
			{
				IsOneKeyKaiKen = other.IsOneKeyKaiKen;
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
				land_.AddEntriesFrom(input, _repeated_land_codec);
				break;
			case 32u:
				IsOneKeyKaiKen = input.ReadBool();
				break;
			}
		}
	}
}
