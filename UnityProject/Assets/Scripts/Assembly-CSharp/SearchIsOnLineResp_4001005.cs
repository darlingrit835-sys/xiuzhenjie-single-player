using System;
using System.Diagnostics;
using Google.Protobuf;
using Google.Protobuf.Collections;
using Google.Protobuf.Reflection;

public sealed class SearchIsOnLineResp_4001005 : IMessage<SearchIsOnLineResp_4001005>, IMessage, IEquatable<SearchIsOnLineResp_4001005>, IDeepCloneable<SearchIsOnLineResp_4001005>
{
	[DebuggerNonUserCode]
	public static class Types
	{
		public sealed class RoleOnLineResp_4001006 : IMessage<RoleOnLineResp_4001006>, IMessage, IEquatable<RoleOnLineResp_4001006>, IDeepCloneable<RoleOnLineResp_4001006>
		{
			private static readonly MessageParser<RoleOnLineResp_4001006> _parser = new MessageParser<RoleOnLineResp_4001006>(() => new RoleOnLineResp_4001006());

			private UnknownFieldSet _unknownFields;

			public const int RoleIdFieldNumber = 1;

			private string roleId_ = "";

			public const int IsOnLineFieldNumber = 2;

			private bool isOnLine_;

			[DebuggerNonUserCode]
			public static MessageParser<RoleOnLineResp_4001006> Parser => _parser;

			[DebuggerNonUserCode]
			public static MessageDescriptor Descriptor => SearchIsOnLineResp_4001005.Descriptor.NestedTypes[0];

			[DebuggerNonUserCode]
			MessageDescriptor IMessage.Descriptor => pb_003A_003AGoogle_002EProtobuf_002EIMessage_002Eget_Descriptor();

			[DebuggerNonUserCode]
			public string RoleId
			{
				get
				{
					return roleId_;
				}
				set
				{
					roleId_ = ProtoPreconditions.CheckNotNull(value, "value");
				}
			}

			[DebuggerNonUserCode]
			public bool IsOnLine
			{
				get
				{
					return isOnLine_;
				}
				set
				{
					isOnLine_ = value;
				}
			}

			private MessageDescriptor pb_003A_003AGoogle_002EProtobuf_002EIMessage_002Eget_Descriptor()
			{
				return Descriptor;
			}

			[DebuggerNonUserCode]
			public RoleOnLineResp_4001006()
			{
			}

			[DebuggerNonUserCode]
			public RoleOnLineResp_4001006(RoleOnLineResp_4001006 other)
				: this()
			{
				roleId_ = other.roleId_;
				isOnLine_ = other.isOnLine_;
				_unknownFields = UnknownFieldSet.Clone(other._unknownFields);
			}

			[DebuggerNonUserCode]
			public RoleOnLineResp_4001006 Clone()
			{
				return new RoleOnLineResp_4001006(this);
			}

			[DebuggerNonUserCode]
			public override bool Equals(object other)
			{
				return Equals(other as RoleOnLineResp_4001006);
			}

			[DebuggerNonUserCode]
			public bool Equals(RoleOnLineResp_4001006 other)
			{
				if (other == null)
				{
					return false;
				}
				if (other == this)
				{
					return true;
				}
				if (RoleId != other.RoleId)
				{
					return false;
				}
				if (IsOnLine != other.IsOnLine)
				{
					return false;
				}
				return object.Equals(_unknownFields, other._unknownFields);
			}

			[DebuggerNonUserCode]
			public override int GetHashCode()
			{
				int num = 1;
				if (RoleId.Length != 0)
				{
					num ^= RoleId.GetHashCode();
				}
				if (IsOnLine)
				{
					num ^= IsOnLine.GetHashCode();
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
				if (RoleId.Length != 0)
				{
					output.WriteRawTag(10);
					output.WriteString(RoleId);
				}
				if (IsOnLine)
				{
					output.WriteRawTag(16);
					output.WriteBool(IsOnLine);
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
				if (RoleId.Length != 0)
				{
					num += 1 + CodedOutputStream.ComputeStringSize(RoleId);
				}
				if (IsOnLine)
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
			public void MergeFrom(RoleOnLineResp_4001006 other)
			{
				if (other != null)
				{
					if (other.RoleId.Length != 0)
					{
						RoleId = other.RoleId;
					}
					if (other.IsOnLine)
					{
						IsOnLine = other.IsOnLine;
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
						RoleId = input.ReadString();
						break;
					case 16u:
						IsOnLine = input.ReadBool();
						break;
					}
				}
			}
		}
	}

	private static readonly MessageParser<SearchIsOnLineResp_4001005> _parser = new MessageParser<SearchIsOnLineResp_4001005>(() => new SearchIsOnLineResp_4001005());

	private UnknownFieldSet _unknownFields;

	public const int CodeFieldNumber = 1;

	private int code_;

	public const int IsSuccessFieldNumber = 2;

	private bool isSuccess_;

	public const int RoleOnLineFieldNumber = 3;

	private static readonly FieldCodec<Types.RoleOnLineResp_4001006> _repeated_roleOnLine_codec = FieldCodec.ForMessage(26u, Types.RoleOnLineResp_4001006.Parser);

	private readonly RepeatedField<Types.RoleOnLineResp_4001006> roleOnLine_ = new RepeatedField<Types.RoleOnLineResp_4001006>();

	[DebuggerNonUserCode]
	public static MessageParser<SearchIsOnLineResp_4001005> Parser => _parser;

	[DebuggerNonUserCode]
	public static MessageDescriptor Descriptor => ClientChatProtoReflection.Descriptor.MessageTypes[8];

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
	public RepeatedField<Types.RoleOnLineResp_4001006> RoleOnLine => roleOnLine_;

	private MessageDescriptor pb_003A_003AGoogle_002EProtobuf_002EIMessage_002Eget_Descriptor()
	{
		return Descriptor;
	}

	[DebuggerNonUserCode]
	public SearchIsOnLineResp_4001005()
	{
	}

	[DebuggerNonUserCode]
	public SearchIsOnLineResp_4001005(SearchIsOnLineResp_4001005 other)
		: this()
	{
		code_ = other.code_;
		isSuccess_ = other.isSuccess_;
		roleOnLine_ = other.roleOnLine_.Clone();
		_unknownFields = UnknownFieldSet.Clone(other._unknownFields);
	}

	[DebuggerNonUserCode]
	public SearchIsOnLineResp_4001005 Clone()
	{
		return new SearchIsOnLineResp_4001005(this);
	}

	[DebuggerNonUserCode]
	public override bool Equals(object other)
	{
		return Equals(other as SearchIsOnLineResp_4001005);
	}

	[DebuggerNonUserCode]
	public bool Equals(SearchIsOnLineResp_4001005 other)
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
		if (!roleOnLine_.Equals(other.roleOnLine_))
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
		num ^= roleOnLine_.GetHashCode();
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
		roleOnLine_.WriteTo(output, _repeated_roleOnLine_codec);
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
		num += roleOnLine_.CalculateSize(_repeated_roleOnLine_codec);
		if (_unknownFields != null)
		{
			num += _unknownFields.CalculateSize();
		}
		return num;
	}

	[DebuggerNonUserCode]
	public void MergeFrom(SearchIsOnLineResp_4001005 other)
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
			roleOnLine_.Add(other.roleOnLine_);
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
				roleOnLine_.AddEntriesFrom(input, _repeated_roleOnLine_codec);
				break;
			}
		}
	}
}
