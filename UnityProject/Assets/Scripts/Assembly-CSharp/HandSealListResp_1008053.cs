using System;
using System.Diagnostics;
using Google.Protobuf;
using Google.Protobuf.Collections;
using Google.Protobuf.Reflection;

public sealed class HandSealListResp_1008053 : IMessage<HandSealListResp_1008053>, IMessage, IEquatable<HandSealListResp_1008053>, IDeepCloneable<HandSealListResp_1008053>
{
	[DebuggerNonUserCode]
	public static class Types
	{
		public sealed class HandSealResp_1008064 : IMessage<HandSealResp_1008064>, IMessage, IEquatable<HandSealResp_1008064>, IDeepCloneable<HandSealResp_1008064>
		{
			private static readonly MessageParser<HandSealResp_1008064> _parser = new MessageParser<HandSealResp_1008064>(() => new HandSealResp_1008064());

			private UnknownFieldSet _unknownFields;

			public const int IdFieldNumber = 1;

			private string id_ = "";

			public const int NameFieldNumber = 2;

			private string name_ = "";

			public const int FiveElementFieldNumber = 3;

			private int fiveElement_;

			public const int RageFieldNumber = 4;

			private string rage_ = "";

			[DebuggerNonUserCode]
			public static MessageParser<HandSealResp_1008064> Parser => _parser;

			[DebuggerNonUserCode]
			public static MessageDescriptor Descriptor => HandSealListResp_1008053.Descriptor.NestedTypes[0];

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
			public string Rage
			{
				get
				{
					return rage_;
				}
				set
				{
					rage_ = ProtoPreconditions.CheckNotNull(value, "value");
				}
			}

			private MessageDescriptor pb_003A_003AGoogle_002EProtobuf_002EIMessage_002Eget_Descriptor()
			{
				return Descriptor;
			}

			[DebuggerNonUserCode]
			public HandSealResp_1008064()
			{
			}

			[DebuggerNonUserCode]
			public HandSealResp_1008064(HandSealResp_1008064 other)
				: this()
			{
				id_ = other.id_;
				name_ = other.name_;
				fiveElement_ = other.fiveElement_;
				rage_ = other.rage_;
				_unknownFields = UnknownFieldSet.Clone(other._unknownFields);
			}

			[DebuggerNonUserCode]
			public HandSealResp_1008064 Clone()
			{
				return new HandSealResp_1008064(this);
			}

			[DebuggerNonUserCode]
			public override bool Equals(object other)
			{
				return Equals(other as HandSealResp_1008064);
			}

			[DebuggerNonUserCode]
			public bool Equals(HandSealResp_1008064 other)
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
				if (Rage != other.Rage)
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
				if (Rage.Length != 0)
				{
					num ^= Rage.GetHashCode();
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
				if (Rage.Length != 0)
				{
					output.WriteRawTag(34);
					output.WriteString(Rage);
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
				if (Rage.Length != 0)
				{
					num += 1 + CodedOutputStream.ComputeStringSize(Rage);
				}
				if (_unknownFields != null)
				{
					num += _unknownFields.CalculateSize();
				}
				return num;
			}

			[DebuggerNonUserCode]
			public void MergeFrom(HandSealResp_1008064 other)
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
					if (other.Rage.Length != 0)
					{
						Rage = other.Rage;
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
					case 34u:
						Rage = input.ReadString();
						break;
					}
				}
			}
		}
	}

	private static readonly MessageParser<HandSealListResp_1008053> _parser = new MessageParser<HandSealListResp_1008053>(() => new HandSealListResp_1008053());

	private UnknownFieldSet _unknownFields;

	public const int CodeFieldNumber = 1;

	private int code_;

	public const int IsSuccessFieldNumber = 2;

	private bool isSuccess_;

	public const int HandSealFieldNumber = 3;

	private static readonly FieldCodec<Types.HandSealResp_1008064> _repeated_handSeal_codec = FieldCodec.ForMessage(26u, Types.HandSealResp_1008064.Parser);

	private readonly RepeatedField<Types.HandSealResp_1008064> handSeal_ = new RepeatedField<Types.HandSealResp_1008064>();

	[DebuggerNonUserCode]
	public static MessageParser<HandSealListResp_1008053> Parser => _parser;

	[DebuggerNonUserCode]
	public static MessageDescriptor Descriptor => UnrealWorldProtoReflection.Descriptor.MessageTypes[66];

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
	public RepeatedField<Types.HandSealResp_1008064> HandSeal => handSeal_;

	private MessageDescriptor pb_003A_003AGoogle_002EProtobuf_002EIMessage_002Eget_Descriptor()
	{
		return Descriptor;
	}

	[DebuggerNonUserCode]
	public HandSealListResp_1008053()
	{
	}

	[DebuggerNonUserCode]
	public HandSealListResp_1008053(HandSealListResp_1008053 other)
		: this()
	{
		code_ = other.code_;
		isSuccess_ = other.isSuccess_;
		handSeal_ = other.handSeal_.Clone();
		_unknownFields = UnknownFieldSet.Clone(other._unknownFields);
	}

	[DebuggerNonUserCode]
	public HandSealListResp_1008053 Clone()
	{
		return new HandSealListResp_1008053(this);
	}

	[DebuggerNonUserCode]
	public override bool Equals(object other)
	{
		return Equals(other as HandSealListResp_1008053);
	}

	[DebuggerNonUserCode]
	public bool Equals(HandSealListResp_1008053 other)
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
		if (!handSeal_.Equals(other.handSeal_))
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
		num ^= handSeal_.GetHashCode();
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
		handSeal_.WriteTo(output, _repeated_handSeal_codec);
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
		num += handSeal_.CalculateSize(_repeated_handSeal_codec);
		if (_unknownFields != null)
		{
			num += _unknownFields.CalculateSize();
		}
		return num;
	}

	[DebuggerNonUserCode]
	public void MergeFrom(HandSealListResp_1008053 other)
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
			handSeal_.Add(other.handSeal_);
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
				handSeal_.AddEntriesFrom(input, _repeated_handSeal_codec);
				break;
			}
		}
	}
}
