using System;
using System.Diagnostics;
using Google.Protobuf;
using Google.Protobuf.Collections;
using Google.Protobuf.Reflection;

public sealed class OpenAcupointResp_1005016 : IMessage<OpenAcupointResp_1005016>, IMessage, IEquatable<OpenAcupointResp_1005016>, IDeepCloneable<OpenAcupointResp_1005016>
{
	[DebuggerNonUserCode]
	public static class Types
	{
		public sealed class RoleAcupointResp_1005017 : IMessage<RoleAcupointResp_1005017>, IMessage, IEquatable<RoleAcupointResp_1005017>, IDeepCloneable<RoleAcupointResp_1005017>
		{
			private static readonly MessageParser<RoleAcupointResp_1005017> _parser = new MessageParser<RoleAcupointResp_1005017>(() => new RoleAcupointResp_1005017());

			private UnknownFieldSet _unknownFields;

			public const int IdFieldNumber = 1;

			private string id_ = "";

			public const int BrokenLevelFieldNumber = 2;

			private int brokenLevel_;

			[DebuggerNonUserCode]
			public static MessageParser<RoleAcupointResp_1005017> Parser => _parser;

			[DebuggerNonUserCode]
			public static MessageDescriptor Descriptor => OpenAcupointResp_1005016.Descriptor.NestedTypes[0];

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
			public int BrokenLevel
			{
				get
				{
					return brokenLevel_;
				}
				set
				{
					brokenLevel_ = value;
				}
			}

			private MessageDescriptor pb_003A_003AGoogle_002EProtobuf_002EIMessage_002Eget_Descriptor()
			{
				return Descriptor;
			}

			[DebuggerNonUserCode]
			public RoleAcupointResp_1005017()
			{
			}

			[DebuggerNonUserCode]
			public RoleAcupointResp_1005017(RoleAcupointResp_1005017 other)
				: this()
			{
				id_ = other.id_;
				brokenLevel_ = other.brokenLevel_;
				_unknownFields = UnknownFieldSet.Clone(other._unknownFields);
			}

			[DebuggerNonUserCode]
			public RoleAcupointResp_1005017 Clone()
			{
				return new RoleAcupointResp_1005017(this);
			}

			[DebuggerNonUserCode]
			public override bool Equals(object other)
			{
				return Equals(other as RoleAcupointResp_1005017);
			}

			[DebuggerNonUserCode]
			public bool Equals(RoleAcupointResp_1005017 other)
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
				if (BrokenLevel != other.BrokenLevel)
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
				if (BrokenLevel != 0)
				{
					num ^= BrokenLevel.GetHashCode();
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
				if (BrokenLevel != 0)
				{
					output.WriteRawTag(16);
					output.WriteInt32(BrokenLevel);
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
				if (BrokenLevel != 0)
				{
					num += 1 + CodedOutputStream.ComputeInt32Size(BrokenLevel);
				}
				if (_unknownFields != null)
				{
					num += _unknownFields.CalculateSize();
				}
				return num;
			}

			[DebuggerNonUserCode]
			public void MergeFrom(RoleAcupointResp_1005017 other)
			{
				if (other != null)
				{
					if (other.Id.Length != 0)
					{
						Id = other.Id;
					}
					if (other.BrokenLevel != 0)
					{
						BrokenLevel = other.BrokenLevel;
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
						BrokenLevel = input.ReadInt32();
						break;
					}
				}
			}
		}
	}

	private static readonly MessageParser<OpenAcupointResp_1005016> _parser = new MessageParser<OpenAcupointResp_1005016>(() => new OpenAcupointResp_1005016());

	private UnknownFieldSet _unknownFields;

	public const int CodeFieldNumber = 1;

	private int code_;

	public const int IsSuccessFieldNumber = 2;

	private bool isSuccess_;

	public const int AcupointsFieldNumber = 3;

	private static readonly FieldCodec<Types.RoleAcupointResp_1005017> _repeated_acupoints_codec = FieldCodec.ForMessage(26u, Types.RoleAcupointResp_1005017.Parser);

	private readonly RepeatedField<Types.RoleAcupointResp_1005017> acupoints_ = new RepeatedField<Types.RoleAcupointResp_1005017>();

	public const int AcupointValueFieldNumber = 4;

	private int acupointValue_;

	[DebuggerNonUserCode]
	public static MessageParser<OpenAcupointResp_1005016> Parser => _parser;

	[DebuggerNonUserCode]
	public static MessageDescriptor Descriptor => HomeProtoReflection.Descriptor.MessageTypes[20];

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
	public RepeatedField<Types.RoleAcupointResp_1005017> Acupoints => acupoints_;

	[DebuggerNonUserCode]
	public int AcupointValue
	{
		get
		{
			return acupointValue_;
		}
		set
		{
			acupointValue_ = value;
		}
	}

	private MessageDescriptor pb_003A_003AGoogle_002EProtobuf_002EIMessage_002Eget_Descriptor()
	{
		return Descriptor;
	}

	[DebuggerNonUserCode]
	public OpenAcupointResp_1005016()
	{
	}

	[DebuggerNonUserCode]
	public OpenAcupointResp_1005016(OpenAcupointResp_1005016 other)
		: this()
	{
		code_ = other.code_;
		isSuccess_ = other.isSuccess_;
		acupoints_ = other.acupoints_.Clone();
		acupointValue_ = other.acupointValue_;
		_unknownFields = UnknownFieldSet.Clone(other._unknownFields);
	}

	[DebuggerNonUserCode]
	public OpenAcupointResp_1005016 Clone()
	{
		return new OpenAcupointResp_1005016(this);
	}

	[DebuggerNonUserCode]
	public override bool Equals(object other)
	{
		return Equals(other as OpenAcupointResp_1005016);
	}

	[DebuggerNonUserCode]
	public bool Equals(OpenAcupointResp_1005016 other)
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
		if (!acupoints_.Equals(other.acupoints_))
		{
			return false;
		}
		if (AcupointValue != other.AcupointValue)
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
		num ^= acupoints_.GetHashCode();
		if (AcupointValue != 0)
		{
			num ^= AcupointValue.GetHashCode();
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
		acupoints_.WriteTo(output, _repeated_acupoints_codec);
		if (AcupointValue != 0)
		{
			output.WriteRawTag(32);
			output.WriteInt32(AcupointValue);
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
		num += acupoints_.CalculateSize(_repeated_acupoints_codec);
		if (AcupointValue != 0)
		{
			num += 1 + CodedOutputStream.ComputeInt32Size(AcupointValue);
		}
		if (_unknownFields != null)
		{
			num += _unknownFields.CalculateSize();
		}
		return num;
	}

	[DebuggerNonUserCode]
	public void MergeFrom(OpenAcupointResp_1005016 other)
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
			acupoints_.Add(other.acupoints_);
			if (other.AcupointValue != 0)
			{
				AcupointValue = other.AcupointValue;
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
				acupoints_.AddEntriesFrom(input, _repeated_acupoints_codec);
				break;
			case 32u:
				AcupointValue = input.ReadInt32();
				break;
			}
		}
	}
}
