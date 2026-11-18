using System;
using System.Diagnostics;
using Google.Protobuf;
using Google.Protobuf.Collections;
using Google.Protobuf.Reflection;

public sealed class EnterSoilResp_1013004 : IMessage<EnterSoilResp_1013004>, IMessage, IEquatable<EnterSoilResp_1013004>, IDeepCloneable<EnterSoilResp_1013004>
{
	[DebuggerNonUserCode]
	public static class Types
	{
		public sealed class SoilResp_1013005 : IMessage<SoilResp_1013005>, IMessage, IEquatable<SoilResp_1013005>, IDeepCloneable<SoilResp_1013005>
		{
			private static readonly MessageParser<SoilResp_1013005> _parser = new MessageParser<SoilResp_1013005>(() => new SoilResp_1013005());

			private UnknownFieldSet _unknownFields;

			public const int IdFieldNumber = 1;

			private string id_ = "";

			public const int FiveElementFieldNumber = 2;

			private int fiveElement_;

			public const int LevelFieldNumber = 3;

			private int level_;

			public const int NameFieldNumber = 4;

			private string name_ = "";

			public const int RankFieldNumber = 5;

			private int rank_;

			public const int ReceiveValFieldNumber = 6;

			private double receiveVal_;

			public const int ConversionRateFieldNumber = 7;

			private double conversionRate_;

			[DebuggerNonUserCode]
			public static MessageParser<SoilResp_1013005> Parser => _parser;

			[DebuggerNonUserCode]
			public static MessageDescriptor Descriptor => EnterSoilResp_1013004.Descriptor.NestedTypes[0];

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
			public int FiveElement
			{
				get
				{
					return fiveElement_;
				}
				set
				{
					fiveElement_ = value;
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

			[DebuggerNonUserCode]
			public double ReceiveVal
			{
				get
				{
					return receiveVal_;
				}
				set
				{
					receiveVal_ = value;
				}
			}

			[DebuggerNonUserCode]
			public double ConversionRate
			{
				get
				{
					return conversionRate_;
				}
				set
				{
					conversionRate_ = value;
				}
			}

			private MessageDescriptor pb_003A_003AGoogle_002EProtobuf_002EIMessage_002Eget_Descriptor()
			{
				return Descriptor;
			}

			[DebuggerNonUserCode]
			public SoilResp_1013005()
			{
			}

			[DebuggerNonUserCode]
			public SoilResp_1013005(SoilResp_1013005 other)
				: this()
			{
				id_ = other.id_;
				fiveElement_ = other.fiveElement_;
				level_ = other.level_;
				name_ = other.name_;
				rank_ = other.rank_;
				receiveVal_ = other.receiveVal_;
				conversionRate_ = other.conversionRate_;
				_unknownFields = UnknownFieldSet.Clone(other._unknownFields);
			}

			[DebuggerNonUserCode]
			public SoilResp_1013005 Clone()
			{
				return new SoilResp_1013005(this);
			}

			[DebuggerNonUserCode]
			public override bool Equals(object other)
			{
				return Equals(other as SoilResp_1013005);
			}

			[DebuggerNonUserCode]
			public bool Equals(SoilResp_1013005 other)
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
				if (FiveElement != other.FiveElement)
				{
					return false;
				}
				if (Level != other.Level)
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
				if (!ProtobufEqualityComparers.BitwiseDoubleEqualityComparer.Equals(ReceiveVal, other.ReceiveVal))
				{
					return false;
				}
				if (!ProtobufEqualityComparers.BitwiseDoubleEqualityComparer.Equals(ConversionRate, other.ConversionRate))
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
				if (FiveElement != 0)
				{
					num ^= FiveElement.GetHashCode();
				}
				if (Level != 0)
				{
					num ^= Level.GetHashCode();
				}
				if (Name.Length != 0)
				{
					num ^= Name.GetHashCode();
				}
				if (Rank != 0)
				{
					num ^= Rank.GetHashCode();
				}
				if (ReceiveVal != 0.0)
				{
					num ^= ProtobufEqualityComparers.BitwiseDoubleEqualityComparer.GetHashCode(ReceiveVal);
				}
				if (ConversionRate != 0.0)
				{
					num ^= ProtobufEqualityComparers.BitwiseDoubleEqualityComparer.GetHashCode(ConversionRate);
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
				if (FiveElement != 0)
				{
					output.WriteRawTag(16);
					output.WriteInt32(FiveElement);
				}
				if (Level != 0)
				{
					output.WriteRawTag(24);
					output.WriteInt32(Level);
				}
				if (Name.Length != 0)
				{
					output.WriteRawTag(34);
					output.WriteString(Name);
				}
				if (Rank != 0)
				{
					output.WriteRawTag(40);
					output.WriteInt32(Rank);
				}
				if (ReceiveVal != 0.0)
				{
					output.WriteRawTag(49);
					output.WriteDouble(ReceiveVal);
				}
				if (ConversionRate != 0.0)
				{
					output.WriteRawTag(57);
					output.WriteDouble(ConversionRate);
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
				if (FiveElement != 0)
				{
					num += 1 + CodedOutputStream.ComputeInt32Size(FiveElement);
				}
				if (Level != 0)
				{
					num += 1 + CodedOutputStream.ComputeInt32Size(Level);
				}
				if (Name.Length != 0)
				{
					num += 1 + CodedOutputStream.ComputeStringSize(Name);
				}
				if (Rank != 0)
				{
					num += 1 + CodedOutputStream.ComputeInt32Size(Rank);
				}
				if (ReceiveVal != 0.0)
				{
					num += 9;
				}
				if (ConversionRate != 0.0)
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
			public void MergeFrom(SoilResp_1013005 other)
			{
				if (other != null)
				{
					if (other.Id.Length != 0)
					{
						Id = other.Id;
					}
					if (other.FiveElement != 0)
					{
						FiveElement = other.FiveElement;
					}
					if (other.Level != 0)
					{
						Level = other.Level;
					}
					if (other.Name.Length != 0)
					{
						Name = other.Name;
					}
					if (other.Rank != 0)
					{
						Rank = other.Rank;
					}
					if (other.ReceiveVal != 0.0)
					{
						ReceiveVal = other.ReceiveVal;
					}
					if (other.ConversionRate != 0.0)
					{
						ConversionRate = other.ConversionRate;
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
					case 16u:
						FiveElement = input.ReadInt32();
						break;
					case 24u:
						Level = input.ReadInt32();
						break;
					case 34u:
						Name = input.ReadString();
						break;
					case 40u:
						Rank = input.ReadInt32();
						break;
					case 49u:
						ReceiveVal = input.ReadDouble();
						break;
					case 57u:
						ConversionRate = input.ReadDouble();
						break;
					}
				}
			}
		}
	}

	private static readonly MessageParser<EnterSoilResp_1013004> _parser = new MessageParser<EnterSoilResp_1013004>(() => new EnterSoilResp_1013004());

	private UnknownFieldSet _unknownFields;

	public const int CodeFieldNumber = 1;

	private int code_;

	public const int IsSuccessFieldNumber = 2;

	private bool isSuccess_;

	public const int CurrentSoilFieldNumber = 3;

	private Types.SoilResp_1013005 currentSoil_;

	public const int SoilAllFieldNumber = 4;

	private static readonly FieldCodec<Types.SoilResp_1013005> _repeated_soilAll_codec = FieldCodec.ForMessage(34u, Types.SoilResp_1013005.Parser);

	private readonly RepeatedField<Types.SoilResp_1013005> soilAll_ = new RepeatedField<Types.SoilResp_1013005>();

	[DebuggerNonUserCode]
	public static MessageParser<EnterSoilResp_1013004> Parser => _parser;

	[DebuggerNonUserCode]
	public static MessageDescriptor Descriptor => PlantProtoReflection.Descriptor.MessageTypes[4];

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
	public Types.SoilResp_1013005 CurrentSoil
	{
		get
		{
			return currentSoil_;
		}
		set
		{
			currentSoil_ = value;
		}
	}

	[DebuggerNonUserCode]
	public RepeatedField<Types.SoilResp_1013005> SoilAll => soilAll_;

	private MessageDescriptor pb_003A_003AGoogle_002EProtobuf_002EIMessage_002Eget_Descriptor()
	{
		return Descriptor;
	}

	[DebuggerNonUserCode]
	public EnterSoilResp_1013004()
	{
	}

	[DebuggerNonUserCode]
	public EnterSoilResp_1013004(EnterSoilResp_1013004 other)
		: this()
	{
		code_ = other.code_;
		isSuccess_ = other.isSuccess_;
		currentSoil_ = ((other.currentSoil_ != null) ? other.currentSoil_.Clone() : null);
		soilAll_ = other.soilAll_.Clone();
		_unknownFields = UnknownFieldSet.Clone(other._unknownFields);
	}

	[DebuggerNonUserCode]
	public EnterSoilResp_1013004 Clone()
	{
		return new EnterSoilResp_1013004(this);
	}

	[DebuggerNonUserCode]
	public override bool Equals(object other)
	{
		return Equals(other as EnterSoilResp_1013004);
	}

	[DebuggerNonUserCode]
	public bool Equals(EnterSoilResp_1013004 other)
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
		if (!object.Equals(CurrentSoil, other.CurrentSoil))
		{
			return false;
		}
		if (!soilAll_.Equals(other.soilAll_))
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
		if (currentSoil_ != null)
		{
			num ^= CurrentSoil.GetHashCode();
		}
		num ^= soilAll_.GetHashCode();
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
		if (currentSoil_ != null)
		{
			output.WriteRawTag(26);
			output.WriteMessage(CurrentSoil);
		}
		soilAll_.WriteTo(output, _repeated_soilAll_codec);
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
		if (currentSoil_ != null)
		{
			num += 1 + CodedOutputStream.ComputeMessageSize(CurrentSoil);
		}
		num += soilAll_.CalculateSize(_repeated_soilAll_codec);
		if (_unknownFields != null)
		{
			num += _unknownFields.CalculateSize();
		}
		return num;
	}

	[DebuggerNonUserCode]
	public void MergeFrom(EnterSoilResp_1013004 other)
	{
		if (other == null)
		{
			return;
		}
		if (other.Code != 0)
		{
			Code = other.Code;
		}
		if (other.IsSuccess)
		{
			IsSuccess = other.IsSuccess;
		}
		if (other.currentSoil_ != null)
		{
			if (currentSoil_ == null)
			{
				CurrentSoil = new Types.SoilResp_1013005();
			}
			CurrentSoil.MergeFrom(other.CurrentSoil);
		}
		soilAll_.Add(other.soilAll_);
		_unknownFields = UnknownFieldSet.MergeFrom(_unknownFields, other._unknownFields);
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
				if (currentSoil_ == null)
				{
					CurrentSoil = new Types.SoilResp_1013005();
				}
				input.ReadMessage(CurrentSoil);
				break;
			case 34u:
				soilAll_.AddEntriesFrom(input, _repeated_soilAll_codec);
				break;
			}
		}
	}
}
