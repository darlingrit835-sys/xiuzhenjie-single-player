using System;
using System.Diagnostics;
using Google.Protobuf;
using Google.Protobuf.Collections;
using Google.Protobuf.Reflection;

public sealed class EnterElixirAuthResp_1008007 : IMessage<EnterElixirAuthResp_1008007>, IMessage, IEquatable<EnterElixirAuthResp_1008007>, IDeepCloneable<EnterElixirAuthResp_1008007>
{
	[DebuggerNonUserCode]
	public static class Types
	{
		public sealed class ExamineContentObjectResp_1008008 : IMessage<ExamineContentObjectResp_1008008>, IMessage, IEquatable<ExamineContentObjectResp_1008008>, IDeepCloneable<ExamineContentObjectResp_1008008>
		{
			private static readonly MessageParser<ExamineContentObjectResp_1008008> _parser = new MessageParser<ExamineContentObjectResp_1008008>(() => new ExamineContentObjectResp_1008008());

			private UnknownFieldSet _unknownFields;

			public const int NameFieldNumber = 1;

			private string name_ = "";

			public const int CountFieldNumber = 2;

			private int count_;

			public const int MethodElixirIdFieldNumber = 3;

			private string methodElixirId_ = "";

			public const int AskFieldNumber = 4;

			private int ask_;

			[DebuggerNonUserCode]
			public static MessageParser<ExamineContentObjectResp_1008008> Parser => _parser;

			[DebuggerNonUserCode]
			public static MessageDescriptor Descriptor => EnterElixirAuthResp_1008007.Descriptor.NestedTypes[0];

			[DebuggerNonUserCode]
			MessageDescriptor IMessage.Descriptor => pb_003A_003AGoogle_002EProtobuf_002EIMessage_002Eget_Descriptor();

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
			public int Count
			{
				get
				{
					return count_;
				}
				set
				{
					count_ = value;
				}
			}

			[DebuggerNonUserCode]
			public string MethodElixirId
			{
				get
				{
					return methodElixirId_;
				}
				set
				{
					methodElixirId_ = ProtoPreconditions.CheckNotNull(value, "value");
				}
			}

			[DebuggerNonUserCode]
			public int Ask
			{
				get
				{
					return ask_;
				}
				set
				{
					ask_ = value;
				}
			}

			private MessageDescriptor pb_003A_003AGoogle_002EProtobuf_002EIMessage_002Eget_Descriptor()
			{
				return Descriptor;
			}

			[DebuggerNonUserCode]
			public ExamineContentObjectResp_1008008()
			{
			}

			[DebuggerNonUserCode]
			public ExamineContentObjectResp_1008008(ExamineContentObjectResp_1008008 other)
				: this()
			{
				name_ = other.name_;
				count_ = other.count_;
				methodElixirId_ = other.methodElixirId_;
				ask_ = other.ask_;
				_unknownFields = UnknownFieldSet.Clone(other._unknownFields);
			}

			[DebuggerNonUserCode]
			public ExamineContentObjectResp_1008008 Clone()
			{
				return new ExamineContentObjectResp_1008008(this);
			}

			[DebuggerNonUserCode]
			public override bool Equals(object other)
			{
				return Equals(other as ExamineContentObjectResp_1008008);
			}

			[DebuggerNonUserCode]
			public bool Equals(ExamineContentObjectResp_1008008 other)
			{
				if (other == null)
				{
					return false;
				}
				if (other == this)
				{
					return true;
				}
				if (Name != other.Name)
				{
					return false;
				}
				if (Count != other.Count)
				{
					return false;
				}
				if (MethodElixirId != other.MethodElixirId)
				{
					return false;
				}
				if (Ask != other.Ask)
				{
					return false;
				}
				return object.Equals(_unknownFields, other._unknownFields);
			}

			[DebuggerNonUserCode]
			public override int GetHashCode()
			{
				int num = 1;
				if (Name.Length != 0)
				{
					num ^= Name.GetHashCode();
				}
				if (Count != 0)
				{
					num ^= Count.GetHashCode();
				}
				if (MethodElixirId.Length != 0)
				{
					num ^= MethodElixirId.GetHashCode();
				}
				if (Ask != 0)
				{
					num ^= Ask.GetHashCode();
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
				if (Name.Length != 0)
				{
					output.WriteRawTag(10);
					output.WriteString(Name);
				}
				if (Count != 0)
				{
					output.WriteRawTag(16);
					output.WriteInt32(Count);
				}
				if (MethodElixirId.Length != 0)
				{
					output.WriteRawTag(26);
					output.WriteString(MethodElixirId);
				}
				if (Ask != 0)
				{
					output.WriteRawTag(32);
					output.WriteInt32(Ask);
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
				if (Name.Length != 0)
				{
					num += 1 + CodedOutputStream.ComputeStringSize(Name);
				}
				if (Count != 0)
				{
					num += 1 + CodedOutputStream.ComputeInt32Size(Count);
				}
				if (MethodElixirId.Length != 0)
				{
					num += 1 + CodedOutputStream.ComputeStringSize(MethodElixirId);
				}
				if (Ask != 0)
				{
					num += 1 + CodedOutputStream.ComputeInt32Size(Ask);
				}
				if (_unknownFields != null)
				{
					num += _unknownFields.CalculateSize();
				}
				return num;
			}

			[DebuggerNonUserCode]
			public void MergeFrom(ExamineContentObjectResp_1008008 other)
			{
				if (other != null)
				{
					if (other.Name.Length != 0)
					{
						Name = other.Name;
					}
					if (other.Count != 0)
					{
						Count = other.Count;
					}
					if (other.MethodElixirId.Length != 0)
					{
						MethodElixirId = other.MethodElixirId;
					}
					if (other.Ask != 0)
					{
						Ask = other.Ask;
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
						Name = input.ReadString();
						break;
					case 16u:
						Count = input.ReadInt32();
						break;
					case 26u:
						MethodElixirId = input.ReadString();
						break;
					case 32u:
						Ask = input.ReadInt32();
						break;
					}
				}
			}
		}
	}

	private static readonly MessageParser<EnterElixirAuthResp_1008007> _parser = new MessageParser<EnterElixirAuthResp_1008007>(() => new EnterElixirAuthResp_1008007());

	private UnknownFieldSet _unknownFields;

	public const int CodeFieldNumber = 1;

	private int code_;

	public const int IsSuccessFieldNumber = 2;

	private bool isSuccess_;

	public const int ObjectFieldNumber = 3;

	private static readonly FieldCodec<Types.ExamineContentObjectResp_1008008> _repeated_object_codec = FieldCodec.ForMessage(26u, Types.ExamineContentObjectResp_1008008.Parser);

	private readonly RepeatedField<Types.ExamineContentObjectResp_1008008> object_ = new RepeatedField<Types.ExamineContentObjectResp_1008008>();

	[DebuggerNonUserCode]
	public static MessageParser<EnterElixirAuthResp_1008007> Parser => _parser;

	[DebuggerNonUserCode]
	public static MessageDescriptor Descriptor => UnrealWorldProtoReflection.Descriptor.MessageTypes[6];

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
	public RepeatedField<Types.ExamineContentObjectResp_1008008> Object => object_;

	private MessageDescriptor pb_003A_003AGoogle_002EProtobuf_002EIMessage_002Eget_Descriptor()
	{
		return Descriptor;
	}

	[DebuggerNonUserCode]
	public EnterElixirAuthResp_1008007()
	{
	}

	[DebuggerNonUserCode]
	public EnterElixirAuthResp_1008007(EnterElixirAuthResp_1008007 other)
		: this()
	{
		code_ = other.code_;
		isSuccess_ = other.isSuccess_;
		object_ = other.object_.Clone();
		_unknownFields = UnknownFieldSet.Clone(other._unknownFields);
	}

	[DebuggerNonUserCode]
	public EnterElixirAuthResp_1008007 Clone()
	{
		return new EnterElixirAuthResp_1008007(this);
	}

	[DebuggerNonUserCode]
	public override bool Equals(object other)
	{
		return Equals(other as EnterElixirAuthResp_1008007);
	}

	[DebuggerNonUserCode]
	public bool Equals(EnterElixirAuthResp_1008007 other)
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
		if (!object_.Equals(other.object_))
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
		num ^= object_.GetHashCode();
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
		object_.WriteTo(output, _repeated_object_codec);
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
		num += object_.CalculateSize(_repeated_object_codec);
		if (_unknownFields != null)
		{
			num += _unknownFields.CalculateSize();
		}
		return num;
	}

	[DebuggerNonUserCode]
	public void MergeFrom(EnterElixirAuthResp_1008007 other)
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
			object_.Add(other.object_);
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
				object_.AddEntriesFrom(input, _repeated_object_codec);
				break;
			}
		}
	}
}
