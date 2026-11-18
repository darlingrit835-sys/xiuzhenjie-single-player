using System;
using System.Diagnostics;
using Google.Protobuf;
using Google.Protobuf.Collections;
using Google.Protobuf.Reflection;

public sealed class WineListResp_1012003 : IMessage<WineListResp_1012003>, IMessage, IEquatable<WineListResp_1012003>, IDeepCloneable<WineListResp_1012003>
{
	[DebuggerNonUserCode]
	public static class Types
	{
		public sealed class WineResp_1012004 : IMessage<WineResp_1012004>, IMessage, IEquatable<WineResp_1012004>, IDeepCloneable<WineResp_1012004>
		{
			private static readonly MessageParser<WineResp_1012004> _parser = new MessageParser<WineResp_1012004>(() => new WineResp_1012004());

			private UnknownFieldSet _unknownFields;

			public const int ChatNumFieldNumber = 1;

			private int chatNum_;

			public const int ShinyStoneFieldNumber = 2;

			private long shinyStone_;

			public const int ShinyJadeFieldNumber = 3;

			private long shinyJade_;

			public const int IdFieldNumber = 4;

			private int id_;

			[DebuggerNonUserCode]
			public static MessageParser<WineResp_1012004> Parser => _parser;

			[DebuggerNonUserCode]
			public static MessageDescriptor Descriptor => WineListResp_1012003.Descriptor.NestedTypes[0];

			[DebuggerNonUserCode]
			MessageDescriptor IMessage.Descriptor => pb_003A_003AGoogle_002EProtobuf_002EIMessage_002Eget_Descriptor();

			[DebuggerNonUserCode]
			public int ChatNum
			{
				get
				{
					return chatNum_;
				}
				set
				{
					chatNum_ = value;
				}
			}

			[DebuggerNonUserCode]
			public long ShinyStone
			{
				get
				{
					return shinyStone_;
				}
				set
				{
					shinyStone_ = value;
				}
			}

			[DebuggerNonUserCode]
			public long ShinyJade
			{
				get
				{
					return shinyJade_;
				}
				set
				{
					shinyJade_ = value;
				}
			}

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

			private MessageDescriptor pb_003A_003AGoogle_002EProtobuf_002EIMessage_002Eget_Descriptor()
			{
				return Descriptor;
			}

			[DebuggerNonUserCode]
			public WineResp_1012004()
			{
			}

			[DebuggerNonUserCode]
			public WineResp_1012004(WineResp_1012004 other)
				: this()
			{
				chatNum_ = other.chatNum_;
				shinyStone_ = other.shinyStone_;
				shinyJade_ = other.shinyJade_;
				id_ = other.id_;
				_unknownFields = UnknownFieldSet.Clone(other._unknownFields);
			}

			[DebuggerNonUserCode]
			public WineResp_1012004 Clone()
			{
				return new WineResp_1012004(this);
			}

			[DebuggerNonUserCode]
			public override bool Equals(object other)
			{
				return Equals(other as WineResp_1012004);
			}

			[DebuggerNonUserCode]
			public bool Equals(WineResp_1012004 other)
			{
				if (other == null)
				{
					return false;
				}
				if (other == this)
				{
					return true;
				}
				if (ChatNum != other.ChatNum)
				{
					return false;
				}
				if (ShinyStone != other.ShinyStone)
				{
					return false;
				}
				if (ShinyJade != other.ShinyJade)
				{
					return false;
				}
				if (Id != other.Id)
				{
					return false;
				}
				return object.Equals(_unknownFields, other._unknownFields);
			}

			[DebuggerNonUserCode]
			public override int GetHashCode()
			{
				int num = 1;
				if (ChatNum != 0)
				{
					num ^= ChatNum.GetHashCode();
				}
				if (ShinyStone != 0L)
				{
					num ^= ShinyStone.GetHashCode();
				}
				if (ShinyJade != 0L)
				{
					num ^= ShinyJade.GetHashCode();
				}
				if (Id != 0)
				{
					num ^= Id.GetHashCode();
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
				if (ChatNum != 0)
				{
					output.WriteRawTag(8);
					output.WriteInt32(ChatNum);
				}
				if (ShinyStone != 0L)
				{
					output.WriteRawTag(16);
					output.WriteInt64(ShinyStone);
				}
				if (ShinyJade != 0L)
				{
					output.WriteRawTag(24);
					output.WriteInt64(ShinyJade);
				}
				if (Id != 0)
				{
					output.WriteRawTag(32);
					output.WriteInt32(Id);
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
				if (ChatNum != 0)
				{
					num += 1 + CodedOutputStream.ComputeInt32Size(ChatNum);
				}
				if (ShinyStone != 0L)
				{
					num += 1 + CodedOutputStream.ComputeInt64Size(ShinyStone);
				}
				if (ShinyJade != 0L)
				{
					num += 1 + CodedOutputStream.ComputeInt64Size(ShinyJade);
				}
				if (Id != 0)
				{
					num += 1 + CodedOutputStream.ComputeInt32Size(Id);
				}
				if (_unknownFields != null)
				{
					num += _unknownFields.CalculateSize();
				}
				return num;
			}

			[DebuggerNonUserCode]
			public void MergeFrom(WineResp_1012004 other)
			{
				if (other != null)
				{
					if (other.ChatNum != 0)
					{
						ChatNum = other.ChatNum;
					}
					if (other.ShinyStone != 0L)
					{
						ShinyStone = other.ShinyStone;
					}
					if (other.ShinyJade != 0L)
					{
						ShinyJade = other.ShinyJade;
					}
					if (other.Id != 0)
					{
						Id = other.Id;
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
						ChatNum = input.ReadInt32();
						break;
					case 16u:
						ShinyStone = input.ReadInt64();
						break;
					case 24u:
						ShinyJade = input.ReadInt64();
						break;
					case 32u:
						Id = input.ReadInt32();
						break;
					}
				}
			}
		}
	}

	private static readonly MessageParser<WineListResp_1012003> _parser = new MessageParser<WineListResp_1012003>(() => new WineListResp_1012003());

	private UnknownFieldSet _unknownFields;

	public const int CodeFieldNumber = 1;

	private int code_;

	public const int IsSuccessFieldNumber = 2;

	private bool isSuccess_;

	public const int WineFieldNumber = 3;

	private static readonly FieldCodec<Types.WineResp_1012004> _repeated_wine_codec = FieldCodec.ForMessage(26u, Types.WineResp_1012004.Parser);

	private readonly RepeatedField<Types.WineResp_1012004> wine_ = new RepeatedField<Types.WineResp_1012004>();

	[DebuggerNonUserCode]
	public static MessageParser<WineListResp_1012003> Parser => _parser;

	[DebuggerNonUserCode]
	public static MessageDescriptor Descriptor => ChatProtoReflection.Descriptor.MessageTypes[3];

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
	public RepeatedField<Types.WineResp_1012004> Wine => wine_;

	private MessageDescriptor pb_003A_003AGoogle_002EProtobuf_002EIMessage_002Eget_Descriptor()
	{
		return Descriptor;
	}

	[DebuggerNonUserCode]
	public WineListResp_1012003()
	{
	}

	[DebuggerNonUserCode]
	public WineListResp_1012003(WineListResp_1012003 other)
		: this()
	{
		code_ = other.code_;
		isSuccess_ = other.isSuccess_;
		wine_ = other.wine_.Clone();
		_unknownFields = UnknownFieldSet.Clone(other._unknownFields);
	}

	[DebuggerNonUserCode]
	public WineListResp_1012003 Clone()
	{
		return new WineListResp_1012003(this);
	}

	[DebuggerNonUserCode]
	public override bool Equals(object other)
	{
		return Equals(other as WineListResp_1012003);
	}

	[DebuggerNonUserCode]
	public bool Equals(WineListResp_1012003 other)
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
		if (!wine_.Equals(other.wine_))
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
		num ^= wine_.GetHashCode();
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
		wine_.WriteTo(output, _repeated_wine_codec);
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
		num += wine_.CalculateSize(_repeated_wine_codec);
		if (_unknownFields != null)
		{
			num += _unknownFields.CalculateSize();
		}
		return num;
	}

	[DebuggerNonUserCode]
	public void MergeFrom(WineListResp_1012003 other)
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
			wine_.Add(other.wine_);
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
				wine_.AddEntriesFrom(input, _repeated_wine_codec);
				break;
			}
		}
	}
}
