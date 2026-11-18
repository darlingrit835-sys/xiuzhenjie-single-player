using System;
using System.Diagnostics;
using Google.Protobuf;
using Google.Protobuf.Collections;
using Google.Protobuf.Reflection;

public sealed class OpenTianShuBeiResp_1016004 : IMessage<OpenTianShuBeiResp_1016004>, IMessage, IEquatable<OpenTianShuBeiResp_1016004>, IDeepCloneable<OpenTianShuBeiResp_1016004>
{
	[DebuggerNonUserCode]
	public static class Types
	{
		public sealed class TianShuBeiObjResp_1016005 : IMessage<TianShuBeiObjResp_1016005>, IMessage, IEquatable<TianShuBeiObjResp_1016005>, IDeepCloneable<TianShuBeiObjResp_1016005>
		{
			private static readonly MessageParser<TianShuBeiObjResp_1016005> _parser = new MessageParser<TianShuBeiObjResp_1016005>(() => new TianShuBeiObjResp_1016005());

			private UnknownFieldSet _unknownFields;

			public const int IdFieldNumber = 1;

			private int id_;

			public const int TypeFieldNumber = 2;

			private int type_;

			public const int MaxValFieldNumber = 3;

			private int maxVal_;

			public const int CurrentValFieldNumber = 4;

			private int currentVal_;

			public const int StatusFieldNumber = 5;

			private int status_;

			[DebuggerNonUserCode]
			public static MessageParser<TianShuBeiObjResp_1016005> Parser => _parser;

			[DebuggerNonUserCode]
			public static MessageDescriptor Descriptor => OpenTianShuBeiResp_1016004.Descriptor.NestedTypes[0];

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
			public int MaxVal
			{
				get
				{
					return maxVal_;
				}
				set
				{
					maxVal_ = value;
				}
			}

			[DebuggerNonUserCode]
			public int CurrentVal
			{
				get
				{
					return currentVal_;
				}
				set
				{
					currentVal_ = value;
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

			private MessageDescriptor pb_003A_003AGoogle_002EProtobuf_002EIMessage_002Eget_Descriptor()
			{
				return Descriptor;
			}

			[DebuggerNonUserCode]
			public TianShuBeiObjResp_1016005()
			{
			}

			[DebuggerNonUserCode]
			public TianShuBeiObjResp_1016005(TianShuBeiObjResp_1016005 other)
				: this()
			{
				id_ = other.id_;
				type_ = other.type_;
				maxVal_ = other.maxVal_;
				currentVal_ = other.currentVal_;
				status_ = other.status_;
				_unknownFields = UnknownFieldSet.Clone(other._unknownFields);
			}

			[DebuggerNonUserCode]
			public TianShuBeiObjResp_1016005 Clone()
			{
				return new TianShuBeiObjResp_1016005(this);
			}

			[DebuggerNonUserCode]
			public override bool Equals(object other)
			{
				return Equals(other as TianShuBeiObjResp_1016005);
			}

			[DebuggerNonUserCode]
			public bool Equals(TianShuBeiObjResp_1016005 other)
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
				if (Type != other.Type)
				{
					return false;
				}
				if (MaxVal != other.MaxVal)
				{
					return false;
				}
				if (CurrentVal != other.CurrentVal)
				{
					return false;
				}
				if (Status != other.Status)
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
				if (Type != 0)
				{
					num ^= Type.GetHashCode();
				}
				if (MaxVal != 0)
				{
					num ^= MaxVal.GetHashCode();
				}
				if (CurrentVal != 0)
				{
					num ^= CurrentVal.GetHashCode();
				}
				if (Status != 0)
				{
					num ^= Status.GetHashCode();
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
				if (Type != 0)
				{
					output.WriteRawTag(16);
					output.WriteInt32(Type);
				}
				if (MaxVal != 0)
				{
					output.WriteRawTag(24);
					output.WriteInt32(MaxVal);
				}
				if (CurrentVal != 0)
				{
					output.WriteRawTag(32);
					output.WriteInt32(CurrentVal);
				}
				if (Status != 0)
				{
					output.WriteRawTag(40);
					output.WriteInt32(Status);
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
				if (Type != 0)
				{
					num += 1 + CodedOutputStream.ComputeInt32Size(Type);
				}
				if (MaxVal != 0)
				{
					num += 1 + CodedOutputStream.ComputeInt32Size(MaxVal);
				}
				if (CurrentVal != 0)
				{
					num += 1 + CodedOutputStream.ComputeInt32Size(CurrentVal);
				}
				if (Status != 0)
				{
					num += 1 + CodedOutputStream.ComputeInt32Size(Status);
				}
				if (_unknownFields != null)
				{
					num += _unknownFields.CalculateSize();
				}
				return num;
			}

			[DebuggerNonUserCode]
			public void MergeFrom(TianShuBeiObjResp_1016005 other)
			{
				if (other != null)
				{
					if (other.Id != 0)
					{
						Id = other.Id;
					}
					if (other.Type != 0)
					{
						Type = other.Type;
					}
					if (other.MaxVal != 0)
					{
						MaxVal = other.MaxVal;
					}
					if (other.CurrentVal != 0)
					{
						CurrentVal = other.CurrentVal;
					}
					if (other.Status != 0)
					{
						Status = other.Status;
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
						Type = input.ReadInt32();
						break;
					case 24u:
						MaxVal = input.ReadInt32();
						break;
					case 32u:
						CurrentVal = input.ReadInt32();
						break;
					case 40u:
						Status = input.ReadInt32();
						break;
					}
				}
			}
		}
	}

	private static readonly MessageParser<OpenTianShuBeiResp_1016004> _parser = new MessageParser<OpenTianShuBeiResp_1016004>(() => new OpenTianShuBeiResp_1016004());

	private UnknownFieldSet _unknownFields;

	public const int CodeFieldNumber = 1;

	private int code_;

	public const int IsSuccessFieldNumber = 2;

	private bool isSuccess_;

	public const int TianShuBeiObjFieldNumber = 3;

	private static readonly FieldCodec<Types.TianShuBeiObjResp_1016005> _repeated_tianShuBeiObj_codec = FieldCodec.ForMessage(26u, Types.TianShuBeiObjResp_1016005.Parser);

	private readonly RepeatedField<Types.TianShuBeiObjResp_1016005> tianShuBeiObj_ = new RepeatedField<Types.TianShuBeiObjResp_1016005>();

	[DebuggerNonUserCode]
	public static MessageParser<OpenTianShuBeiResp_1016004> Parser => _parser;

	[DebuggerNonUserCode]
	public static MessageDescriptor Descriptor => TaskProtoReflection.Descriptor.MessageTypes[4];

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
	public RepeatedField<Types.TianShuBeiObjResp_1016005> TianShuBeiObj => tianShuBeiObj_;

	private MessageDescriptor pb_003A_003AGoogle_002EProtobuf_002EIMessage_002Eget_Descriptor()
	{
		return Descriptor;
	}

	[DebuggerNonUserCode]
	public OpenTianShuBeiResp_1016004()
	{
	}

	[DebuggerNonUserCode]
	public OpenTianShuBeiResp_1016004(OpenTianShuBeiResp_1016004 other)
		: this()
	{
		code_ = other.code_;
		isSuccess_ = other.isSuccess_;
		tianShuBeiObj_ = other.tianShuBeiObj_.Clone();
		_unknownFields = UnknownFieldSet.Clone(other._unknownFields);
	}

	[DebuggerNonUserCode]
	public OpenTianShuBeiResp_1016004 Clone()
	{
		return new OpenTianShuBeiResp_1016004(this);
	}

	[DebuggerNonUserCode]
	public override bool Equals(object other)
	{
		return Equals(other as OpenTianShuBeiResp_1016004);
	}

	[DebuggerNonUserCode]
	public bool Equals(OpenTianShuBeiResp_1016004 other)
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
		if (!tianShuBeiObj_.Equals(other.tianShuBeiObj_))
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
		num ^= tianShuBeiObj_.GetHashCode();
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
		tianShuBeiObj_.WriteTo(output, _repeated_tianShuBeiObj_codec);
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
		num += tianShuBeiObj_.CalculateSize(_repeated_tianShuBeiObj_codec);
		if (_unknownFields != null)
		{
			num += _unknownFields.CalculateSize();
		}
		return num;
	}

	[DebuggerNonUserCode]
	public void MergeFrom(OpenTianShuBeiResp_1016004 other)
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
			tianShuBeiObj_.Add(other.tianShuBeiObj_);
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
				tianShuBeiObj_.AddEntriesFrom(input, _repeated_tianShuBeiObj_codec);
				break;
			}
		}
	}
}
