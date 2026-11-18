using System;
using System.Diagnostics;
using Google.Protobuf;
using Google.Protobuf.Collections;
using Google.Protobuf.Reflection;

public sealed class EnterRenMingResp_1015023 : IMessage<EnterRenMingResp_1015023>, IMessage, IEquatable<EnterRenMingResp_1015023>, IDeepCloneable<EnterRenMingResp_1015023>
{
	[DebuggerNonUserCode]
	public static class Types
	{
		public sealed class HillZhangLaoInfoResp_1015024 : IMessage<HillZhangLaoInfoResp_1015024>, IMessage, IEquatable<HillZhangLaoInfoResp_1015024>, IDeepCloneable<HillZhangLaoInfoResp_1015024>
		{
			private static readonly MessageParser<HillZhangLaoInfoResp_1015024> _parser = new MessageParser<HillZhangLaoInfoResp_1015024>(() => new HillZhangLaoInfoResp_1015024());

			private UnknownFieldSet _unknownFields;

			public const int TypeFieldNumber = 1;

			private int type_;

			public const int RoleNameFieldNumber = 2;

			private string roleName_ = "";

			[DebuggerNonUserCode]
			public static MessageParser<HillZhangLaoInfoResp_1015024> Parser => _parser;

			[DebuggerNonUserCode]
			public static MessageDescriptor Descriptor => EnterRenMingResp_1015023.Descriptor.NestedTypes[0];

			[DebuggerNonUserCode]
			MessageDescriptor IMessage.Descriptor => pb_003A_003AGoogle_002EProtobuf_002EIMessage_002Eget_Descriptor();

			[DebuggerNonUserCode]
			public int Type
			{
				get
				{
					return type_;
				}
				set
				{
					type_ = value;
				}
			}

			[DebuggerNonUserCode]
			public string RoleName
			{
				get
				{
					return roleName_;
				}
				set
				{
					roleName_ = ProtoPreconditions.CheckNotNull(value, "value");
				}
			}

			private MessageDescriptor pb_003A_003AGoogle_002EProtobuf_002EIMessage_002Eget_Descriptor()
			{
				return Descriptor;
			}

			[DebuggerNonUserCode]
			public HillZhangLaoInfoResp_1015024()
			{
			}

			[DebuggerNonUserCode]
			public HillZhangLaoInfoResp_1015024(HillZhangLaoInfoResp_1015024 other)
				: this()
			{
				type_ = other.type_;
				roleName_ = other.roleName_;
				_unknownFields = UnknownFieldSet.Clone(other._unknownFields);
			}

			[DebuggerNonUserCode]
			public HillZhangLaoInfoResp_1015024 Clone()
			{
				return new HillZhangLaoInfoResp_1015024(this);
			}

			[DebuggerNonUserCode]
			public override bool Equals(object other)
			{
				return Equals(other as HillZhangLaoInfoResp_1015024);
			}

			[DebuggerNonUserCode]
			public bool Equals(HillZhangLaoInfoResp_1015024 other)
			{
				if (other == null)
				{
					return false;
				}
				if (other == this)
				{
					return true;
				}
				if (Type != other.Type)
				{
					return false;
				}
				if (RoleName != other.RoleName)
				{
					return false;
				}
				return object.Equals(_unknownFields, other._unknownFields);
			}

			[DebuggerNonUserCode]
			public override int GetHashCode()
			{
				int num = 1;
				if (Type != 0)
				{
					num ^= Type.GetHashCode();
				}
				if (RoleName.Length != 0)
				{
					num ^= RoleName.GetHashCode();
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
				if (Type != 0)
				{
					output.WriteRawTag(8);
					output.WriteInt32(Type);
				}
				if (RoleName.Length != 0)
				{
					output.WriteRawTag(18);
					output.WriteString(RoleName);
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
				if (Type != 0)
				{
					num += 1 + CodedOutputStream.ComputeInt32Size(Type);
				}
				if (RoleName.Length != 0)
				{
					num += 1 + CodedOutputStream.ComputeStringSize(RoleName);
				}
				if (_unknownFields != null)
				{
					num += _unknownFields.CalculateSize();
				}
				return num;
			}

			[DebuggerNonUserCode]
			public void MergeFrom(HillZhangLaoInfoResp_1015024 other)
			{
				if (other != null)
				{
					if (other.Type != 0)
					{
						Type = other.Type;
					}
					if (other.RoleName.Length != 0)
					{
						RoleName = other.RoleName;
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
						Type = input.ReadInt32();
						break;
					case 18u:
						RoleName = input.ReadString();
						break;
					}
				}
			}
		}
	}

	private static readonly MessageParser<EnterRenMingResp_1015023> _parser = new MessageParser<EnterRenMingResp_1015023>(() => new EnterRenMingResp_1015023());

	private UnknownFieldSet _unknownFields;

	public const int CodeFieldNumber = 1;

	private int code_;

	public const int IsSuccessFieldNumber = 2;

	private bool isSuccess_;

	public const int HillZhangLaoInfoFieldNumber = 3;

	private static readonly FieldCodec<Types.HillZhangLaoInfoResp_1015024> _repeated_hillZhangLaoInfo_codec = FieldCodec.ForMessage(26u, Types.HillZhangLaoInfoResp_1015024.Parser);

	private readonly RepeatedField<Types.HillZhangLaoInfoResp_1015024> hillZhangLaoInfo_ = new RepeatedField<Types.HillZhangLaoInfoResp_1015024>();

	[DebuggerNonUserCode]
	public static MessageParser<EnterRenMingResp_1015023> Parser => _parser;

	[DebuggerNonUserCode]
	public static MessageDescriptor Descriptor => GuildProtoReflection.Descriptor.MessageTypes[26];

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
	public RepeatedField<Types.HillZhangLaoInfoResp_1015024> HillZhangLaoInfo => hillZhangLaoInfo_;

	private MessageDescriptor pb_003A_003AGoogle_002EProtobuf_002EIMessage_002Eget_Descriptor()
	{
		return Descriptor;
	}

	[DebuggerNonUserCode]
	public EnterRenMingResp_1015023()
	{
	}

	[DebuggerNonUserCode]
	public EnterRenMingResp_1015023(EnterRenMingResp_1015023 other)
		: this()
	{
		code_ = other.code_;
		isSuccess_ = other.isSuccess_;
		hillZhangLaoInfo_ = other.hillZhangLaoInfo_.Clone();
		_unknownFields = UnknownFieldSet.Clone(other._unknownFields);
	}

	[DebuggerNonUserCode]
	public EnterRenMingResp_1015023 Clone()
	{
		return new EnterRenMingResp_1015023(this);
	}

	[DebuggerNonUserCode]
	public override bool Equals(object other)
	{
		return Equals(other as EnterRenMingResp_1015023);
	}

	[DebuggerNonUserCode]
	public bool Equals(EnterRenMingResp_1015023 other)
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
		if (!hillZhangLaoInfo_.Equals(other.hillZhangLaoInfo_))
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
		num ^= hillZhangLaoInfo_.GetHashCode();
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
		hillZhangLaoInfo_.WriteTo(output, _repeated_hillZhangLaoInfo_codec);
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
		num += hillZhangLaoInfo_.CalculateSize(_repeated_hillZhangLaoInfo_codec);
		if (_unknownFields != null)
		{
			num += _unknownFields.CalculateSize();
		}
		return num;
	}

	[DebuggerNonUserCode]
	public void MergeFrom(EnterRenMingResp_1015023 other)
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
			hillZhangLaoInfo_.Add(other.hillZhangLaoInfo_);
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
				hillZhangLaoInfo_.AddEntriesFrom(input, _repeated_hillZhangLaoInfo_codec);
				break;
			}
		}
	}
}
