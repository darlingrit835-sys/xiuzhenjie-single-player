using System;
using System.Diagnostics;
using Google.Protobuf;
using Google.Protobuf.Collections;
using Google.Protobuf.Reflection;

public sealed class SearchWaterAllResp_1013024 : IMessage<SearchWaterAllResp_1013024>, IMessage, IEquatable<SearchWaterAllResp_1013024>, IDeepCloneable<SearchWaterAllResp_1013024>
{
	[DebuggerNonUserCode]
	public static class Types
	{
		public sealed class WaterInfoResp_1013025 : IMessage<WaterInfoResp_1013025>, IMessage, IEquatable<WaterInfoResp_1013025>, IDeepCloneable<WaterInfoResp_1013025>
		{
			private static readonly MessageParser<WaterInfoResp_1013025> _parser = new MessageParser<WaterInfoResp_1013025>(() => new WaterInfoResp_1013025());

			private UnknownFieldSet _unknownFields;

			public const int IdFieldNumber = 1;

			private string id_ = "";

			public const int NameFieldNumber = 2;

			private string name_ = "";

			public const int FiveElementFieldNumber = 3;

			private int fiveElement_;

			public const int RankFieldNumber = 4;

			private int rank_;

			public const int LevelFieldNumber = 5;

			private int level_;

			public const int ReceiveValFieldNumber = 6;

			private double receiveVal_;

			public const int ConversionRateFieldNumber = 7;

			private double conversionRate_;

			[DebuggerNonUserCode]
			public static MessageParser<WaterInfoResp_1013025> Parser => _parser;

			[DebuggerNonUserCode]
			public static MessageDescriptor Descriptor => SearchWaterAllResp_1013024.Descriptor.NestedTypes[0];

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
			public WaterInfoResp_1013025()
			{
			}

			[DebuggerNonUserCode]
			public WaterInfoResp_1013025(WaterInfoResp_1013025 other)
				: this()
			{
				id_ = other.id_;
				name_ = other.name_;
				fiveElement_ = other.fiveElement_;
				rank_ = other.rank_;
				level_ = other.level_;
				receiveVal_ = other.receiveVal_;
				conversionRate_ = other.conversionRate_;
				_unknownFields = UnknownFieldSet.Clone(other._unknownFields);
			}

			[DebuggerNonUserCode]
			public WaterInfoResp_1013025 Clone()
			{
				return new WaterInfoResp_1013025(this);
			}

			[DebuggerNonUserCode]
			public override bool Equals(object other)
			{
				return Equals(other as WaterInfoResp_1013025);
			}

			[DebuggerNonUserCode]
			public bool Equals(WaterInfoResp_1013025 other)
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
				if (FiveElement != other.FiveElement)
				{
					return false;
				}
				if (Rank != other.Rank)
				{
					return false;
				}
				if (Level != other.Level)
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
				if (Name.Length != 0)
				{
					num ^= Name.GetHashCode();
				}
				if (FiveElement != 0)
				{
					num ^= FiveElement.GetHashCode();
				}
				if (Rank != 0)
				{
					num ^= Rank.GetHashCode();
				}
				if (Level != 0)
				{
					num ^= Level.GetHashCode();
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
				if (Name.Length != 0)
				{
					output.WriteRawTag(18);
					output.WriteString(Name);
				}
				if (FiveElement != 0)
				{
					output.WriteRawTag(24);
					output.WriteInt32(FiveElement);
				}
				if (Rank != 0)
				{
					output.WriteRawTag(32);
					output.WriteInt32(Rank);
				}
				if (Level != 0)
				{
					output.WriteRawTag(40);
					output.WriteInt32(Level);
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
				if (Name.Length != 0)
				{
					num += 1 + CodedOutputStream.ComputeStringSize(Name);
				}
				if (FiveElement != 0)
				{
					num += 1 + CodedOutputStream.ComputeInt32Size(FiveElement);
				}
				if (Rank != 0)
				{
					num += 1 + CodedOutputStream.ComputeInt32Size(Rank);
				}
				if (Level != 0)
				{
					num += 1 + CodedOutputStream.ComputeInt32Size(Level);
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
			public void MergeFrom(WaterInfoResp_1013025 other)
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
					if (other.FiveElement != 0)
					{
						FiveElement = other.FiveElement;
					}
					if (other.Rank != 0)
					{
						Rank = other.Rank;
					}
					if (other.Level != 0)
					{
						Level = other.Level;
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
					case 18u:
						Name = input.ReadString();
						break;
					case 24u:
						FiveElement = input.ReadInt32();
						break;
					case 32u:
						Rank = input.ReadInt32();
						break;
					case 40u:
						Level = input.ReadInt32();
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

	private static readonly MessageParser<SearchWaterAllResp_1013024> _parser = new MessageParser<SearchWaterAllResp_1013024>(() => new SearchWaterAllResp_1013024());

	private UnknownFieldSet _unknownFields;

	public const int CodeFieldNumber = 1;

	private int code_;

	public const int IsSuccessFieldNumber = 2;

	private bool isSuccess_;

	public const int WaterAllFieldNumber = 3;

	private static readonly FieldCodec<Types.WaterInfoResp_1013025> _repeated_waterAll_codec = FieldCodec.ForMessage(26u, Types.WaterInfoResp_1013025.Parser);

	private readonly RepeatedField<Types.WaterInfoResp_1013025> waterAll_ = new RepeatedField<Types.WaterInfoResp_1013025>();

	[DebuggerNonUserCode]
	public static MessageParser<SearchWaterAllResp_1013024> Parser => _parser;

	[DebuggerNonUserCode]
	public static MessageDescriptor Descriptor => PlantProtoReflection.Descriptor.MessageTypes[29];

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
	public RepeatedField<Types.WaterInfoResp_1013025> WaterAll => waterAll_;

	private MessageDescriptor pb_003A_003AGoogle_002EProtobuf_002EIMessage_002Eget_Descriptor()
	{
		return Descriptor;
	}

	[DebuggerNonUserCode]
	public SearchWaterAllResp_1013024()
	{
	}

	[DebuggerNonUserCode]
	public SearchWaterAllResp_1013024(SearchWaterAllResp_1013024 other)
		: this()
	{
		code_ = other.code_;
		isSuccess_ = other.isSuccess_;
		waterAll_ = other.waterAll_.Clone();
		_unknownFields = UnknownFieldSet.Clone(other._unknownFields);
	}

	[DebuggerNonUserCode]
	public SearchWaterAllResp_1013024 Clone()
	{
		return new SearchWaterAllResp_1013024(this);
	}

	[DebuggerNonUserCode]
	public override bool Equals(object other)
	{
		return Equals(other as SearchWaterAllResp_1013024);
	}

	[DebuggerNonUserCode]
	public bool Equals(SearchWaterAllResp_1013024 other)
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
		if (!waterAll_.Equals(other.waterAll_))
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
		num ^= waterAll_.GetHashCode();
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
		waterAll_.WriteTo(output, _repeated_waterAll_codec);
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
		num += waterAll_.CalculateSize(_repeated_waterAll_codec);
		if (_unknownFields != null)
		{
			num += _unknownFields.CalculateSize();
		}
		return num;
	}

	[DebuggerNonUserCode]
	public void MergeFrom(SearchWaterAllResp_1013024 other)
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
			waterAll_.Add(other.waterAll_);
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
				waterAll_.AddEntriesFrom(input, _repeated_waterAll_codec);
				break;
			}
		}
	}
}
