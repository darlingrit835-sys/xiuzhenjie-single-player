using System;
using System.Diagnostics;
using Google.Protobuf;
using Google.Protobuf.Collections;
using Google.Protobuf.Reflection;

public sealed class VerifyCreateTemplatePlantReq_1008058 : IMessage<VerifyCreateTemplatePlantReq_1008058>, IMessage, IEquatable<VerifyCreateTemplatePlantReq_1008058>, IDeepCloneable<VerifyCreateTemplatePlantReq_1008058>
{
	[DebuggerNonUserCode]
	public static class Types
	{
		public sealed class TemplatePlantReq_1008065 : IMessage<TemplatePlantReq_1008065>, IMessage, IEquatable<TemplatePlantReq_1008065>, IDeepCloneable<TemplatePlantReq_1008065>
		{
			private static readonly MessageParser<TemplatePlantReq_1008065> _parser = new MessageParser<TemplatePlantReq_1008065>(() => new TemplatePlantReq_1008065());

			private UnknownFieldSet _unknownFields;

			public const int LocationFieldNumber = 1;

			private int location_;

			public const int TemplatePlantIdFieldNumber = 2;

			private string templatePlantId_ = "";

			[DebuggerNonUserCode]
			public static MessageParser<TemplatePlantReq_1008065> Parser => _parser;

			[DebuggerNonUserCode]
			public static MessageDescriptor Descriptor => VerifyCreateTemplatePlantReq_1008058.Descriptor.NestedTypes[0];

			[DebuggerNonUserCode]
			MessageDescriptor IMessage.Descriptor => pb_003A_003AGoogle_002EProtobuf_002EIMessage_002Eget_Descriptor();

			[DebuggerNonUserCode]
			public int Location
			{
				get
				{
					return location_;
				}
				set
				{
					location_ = value;
				}
			}

			[DebuggerNonUserCode]
			public string TemplatePlantId
			{
				get
				{
					return templatePlantId_;
				}
				set
				{
					templatePlantId_ = ProtoPreconditions.CheckNotNull(value, "value");
				}
			}

			private MessageDescriptor pb_003A_003AGoogle_002EProtobuf_002EIMessage_002Eget_Descriptor()
			{
				return Descriptor;
			}

			[DebuggerNonUserCode]
			public TemplatePlantReq_1008065()
			{
			}

			[DebuggerNonUserCode]
			public TemplatePlantReq_1008065(TemplatePlantReq_1008065 other)
				: this()
			{
				location_ = other.location_;
				templatePlantId_ = other.templatePlantId_;
				_unknownFields = UnknownFieldSet.Clone(other._unknownFields);
			}

			[DebuggerNonUserCode]
			public TemplatePlantReq_1008065 Clone()
			{
				return new TemplatePlantReq_1008065(this);
			}

			[DebuggerNonUserCode]
			public override bool Equals(object other)
			{
				return Equals(other as TemplatePlantReq_1008065);
			}

			[DebuggerNonUserCode]
			public bool Equals(TemplatePlantReq_1008065 other)
			{
				if (other == null)
				{
					return false;
				}
				if (other == this)
				{
					return true;
				}
				if (Location != other.Location)
				{
					return false;
				}
				if (TemplatePlantId != other.TemplatePlantId)
				{
					return false;
				}
				return object.Equals(_unknownFields, other._unknownFields);
			}

			[DebuggerNonUserCode]
			public override int GetHashCode()
			{
				int num = 1;
				if (Location != 0)
				{
					num ^= Location.GetHashCode();
				}
				if (TemplatePlantId.Length != 0)
				{
					num ^= TemplatePlantId.GetHashCode();
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
				if (Location != 0)
				{
					output.WriteRawTag(8);
					output.WriteInt32(Location);
				}
				if (TemplatePlantId.Length != 0)
				{
					output.WriteRawTag(18);
					output.WriteString(TemplatePlantId);
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
				if (Location != 0)
				{
					num += 1 + CodedOutputStream.ComputeInt32Size(Location);
				}
				if (TemplatePlantId.Length != 0)
				{
					num += 1 + CodedOutputStream.ComputeStringSize(TemplatePlantId);
				}
				if (_unknownFields != null)
				{
					num += _unknownFields.CalculateSize();
				}
				return num;
			}

			[DebuggerNonUserCode]
			public void MergeFrom(TemplatePlantReq_1008065 other)
			{
				if (other != null)
				{
					if (other.Location != 0)
					{
						Location = other.Location;
					}
					if (other.TemplatePlantId.Length != 0)
					{
						TemplatePlantId = other.TemplatePlantId;
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
						Location = input.ReadInt32();
						break;
					case 18u:
						TemplatePlantId = input.ReadString();
						break;
					}
				}
			}
		}
	}

	private static readonly MessageParser<VerifyCreateTemplatePlantReq_1008058> _parser = new MessageParser<VerifyCreateTemplatePlantReq_1008058>(() => new VerifyCreateTemplatePlantReq_1008058());

	private UnknownFieldSet _unknownFields;

	public const int StageFieldNumber = 1;

	private int stage_;

	public const int PlantFieldNumber = 2;

	private static readonly FieldCodec<Types.TemplatePlantReq_1008065> _repeated_plant_codec = FieldCodec.ForMessage(18u, Types.TemplatePlantReq_1008065.Parser);

	private readonly RepeatedField<Types.TemplatePlantReq_1008065> plant_ = new RepeatedField<Types.TemplatePlantReq_1008065>();

	[DebuggerNonUserCode]
	public static MessageParser<VerifyCreateTemplatePlantReq_1008058> Parser => _parser;

	[DebuggerNonUserCode]
	public static MessageDescriptor Descriptor => UnrealWorldProtoReflection.Descriptor.MessageTypes[72];

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
	public RepeatedField<Types.TemplatePlantReq_1008065> Plant => plant_;

	private MessageDescriptor pb_003A_003AGoogle_002EProtobuf_002EIMessage_002Eget_Descriptor()
	{
		return Descriptor;
	}

	[DebuggerNonUserCode]
	public VerifyCreateTemplatePlantReq_1008058()
	{
	}

	[DebuggerNonUserCode]
	public VerifyCreateTemplatePlantReq_1008058(VerifyCreateTemplatePlantReq_1008058 other)
		: this()
	{
		stage_ = other.stage_;
		plant_ = other.plant_.Clone();
		_unknownFields = UnknownFieldSet.Clone(other._unknownFields);
	}

	[DebuggerNonUserCode]
	public VerifyCreateTemplatePlantReq_1008058 Clone()
	{
		return new VerifyCreateTemplatePlantReq_1008058(this);
	}

	[DebuggerNonUserCode]
	public override bool Equals(object other)
	{
		return Equals(other as VerifyCreateTemplatePlantReq_1008058);
	}

	[DebuggerNonUserCode]
	public bool Equals(VerifyCreateTemplatePlantReq_1008058 other)
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
		if (!plant_.Equals(other.plant_))
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
		num ^= plant_.GetHashCode();
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
		plant_.WriteTo(output, _repeated_plant_codec);
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
		num += plant_.CalculateSize(_repeated_plant_codec);
		if (_unknownFields != null)
		{
			num += _unknownFields.CalculateSize();
		}
		return num;
	}

	[DebuggerNonUserCode]
	public void MergeFrom(VerifyCreateTemplatePlantReq_1008058 other)
	{
		if (other != null)
		{
			if (other.Stage != 0)
			{
				Stage = other.Stage;
			}
			plant_.Add(other.plant_);
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
				plant_.AddEntriesFrom(input, _repeated_plant_codec);
				break;
			}
		}
	}
}
