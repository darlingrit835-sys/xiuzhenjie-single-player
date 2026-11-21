using System;
using System.Diagnostics;
using Google.Protobuf;
using Google.Protobuf.Collections;
using Google.Protobuf.Reflection;

public sealed class EnterJiFenResp_1017004 : IMessage<EnterJiFenResp_1017004>, IMessage, IEquatable<EnterJiFenResp_1017004>, IDeepCloneable<EnterJiFenResp_1017004>
{
	[DebuggerNonUserCode]
	public static class Types
	{
		public sealed class JiFenResp_1017005 : IMessage<JiFenResp_1017005>, IMessage, IEquatable<JiFenResp_1017005>, IDeepCloneable<JiFenResp_1017005>
		{
			private static readonly MessageParser<JiFenResp_1017005> _parser = new MessageParser<JiFenResp_1017005>(() => new JiFenResp_1017005());

			private UnknownFieldSet _unknownFields;

			public const int IdFieldNumber = 1;

			private int id_;

			public const int JiFenFieldNumber = 2;

			private int jiFen_;

			public const int NameFieldNumber = 3;

			private string name_ = "";

			public const int CountFieldNumber = 4;

			private int count_;

			public const int IsGetFieldNumber = 5;

			private bool isGet_;

			[DebuggerNonUserCode]
			public static MessageParser<JiFenResp_1017005> Parser => _parser;

			[DebuggerNonUserCode]
			public static MessageDescriptor Descriptor => EnterJiFenResp_1017004.Descriptor.NestedTypes[0];

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
			public int JiFen
			{
				get
				{
					return jiFen_;
				}
				set
				{
					jiFen_ = value;
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
			public bool IsGet
			{
				get
				{
					return isGet_;
				}
				set
				{
					isGet_ = value;
				}
			}

			private MessageDescriptor pb_003A_003AGoogle_002EProtobuf_002EIMessage_002Eget_Descriptor()
			{
				return Descriptor;
			}

			[DebuggerNonUserCode]
			public JiFenResp_1017005()
			{
			}

			[DebuggerNonUserCode]
			public JiFenResp_1017005(JiFenResp_1017005 other)
				: this()
			{
				id_ = other.id_;
				jiFen_ = other.jiFen_;
				name_ = other.name_;
				count_ = other.count_;
				isGet_ = other.isGet_;
				_unknownFields = UnknownFieldSet.Clone(other._unknownFields);
			}

			[DebuggerNonUserCode]
			public JiFenResp_1017005 Clone()
			{
				return new JiFenResp_1017005(this);
			}

			[DebuggerNonUserCode]
			public override bool Equals(object other)
			{
				return Equals(other as JiFenResp_1017005);
			}

			[DebuggerNonUserCode]
			public bool Equals(JiFenResp_1017005 other)
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
				if (JiFen != other.JiFen)
				{
					return false;
				}
				if (Name != other.Name)
				{
					return false;
				}
				if (Count != other.Count)
				{
					return false;
				}
				if (IsGet != other.IsGet)
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
				if (JiFen != 0)
				{
					num ^= JiFen.GetHashCode();
				}
				if (Name.Length != 0)
				{
					num ^= Name.GetHashCode();
				}
				if (Count != 0)
				{
					num ^= Count.GetHashCode();
				}
				if (IsGet)
				{
					num ^= IsGet.GetHashCode();
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
				if (JiFen != 0)
				{
					output.WriteRawTag(16);
					output.WriteInt32(JiFen);
				}
				if (Name.Length != 0)
				{
					output.WriteRawTag(26);
					output.WriteString(Name);
				}
				if (Count != 0)
				{
					output.WriteRawTag(32);
					output.WriteInt32(Count);
				}
				if (IsGet)
				{
					output.WriteRawTag(40);
					output.WriteBool(IsGet);
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
				if (JiFen != 0)
				{
					num += 1 + CodedOutputStream.ComputeInt32Size(JiFen);
				}
				if (Name.Length != 0)
				{
					num += 1 + CodedOutputStream.ComputeStringSize(Name);
				}
				if (Count != 0)
				{
					num += 1 + CodedOutputStream.ComputeInt32Size(Count);
				}
				if (IsGet)
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
			public void MergeFrom(JiFenResp_1017005 other)
			{
				if (other != null)
				{
					if (other.Id != 0)
					{
						Id = other.Id;
					}
					if (other.JiFen != 0)
					{
						JiFen = other.JiFen;
					}
					if (other.Name.Length != 0)
					{
						Name = other.Name;
					}
					if (other.Count != 0)
					{
						Count = other.Count;
					}
					if (other.IsGet)
					{
						IsGet = other.IsGet;
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
						JiFen = input.ReadInt32();
						break;
					case 26u:
						Name = input.ReadString();
						break;
					case 32u:
						Count = input.ReadInt32();
						break;
					case 40u:
						IsGet = input.ReadBool();
						break;
					}
				}
			}
		}
	}

	private static readonly MessageParser<EnterJiFenResp_1017004> _parser = new MessageParser<EnterJiFenResp_1017004>(() => new EnterJiFenResp_1017004());

	private UnknownFieldSet _unknownFields;

	public const int CodeFieldNumber = 1;

	private int code_;

	public const int IsSuccessFieldNumber = 2;

	private bool isSuccess_;

	public const int JiFenFieldNumber = 3;

	private static readonly FieldCodec<Types.JiFenResp_1017005> _repeated_jiFen_codec = FieldCodec.ForMessage(26u, Types.JiFenResp_1017005.Parser);

	private readonly RepeatedField<Types.JiFenResp_1017005> jiFen_ = new RepeatedField<Types.JiFenResp_1017005>();

	[DebuggerNonUserCode]
	public static MessageParser<EnterJiFenResp_1017004> Parser => _parser;

	[DebuggerNonUserCode]
	public static MessageDescriptor Descriptor => HuoDongProtoReflection.Descriptor.MessageTypes[4];

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
	public RepeatedField<Types.JiFenResp_1017005> JiFen => jiFen_;

	private MessageDescriptor pb_003A_003AGoogle_002EProtobuf_002EIMessage_002Eget_Descriptor()
	{
		return Descriptor;
	}

	[DebuggerNonUserCode]
	public EnterJiFenResp_1017004()
	{
	}

	[DebuggerNonUserCode]
	public EnterJiFenResp_1017004(EnterJiFenResp_1017004 other)
		: this()
	{
		code_ = other.code_;
		isSuccess_ = other.isSuccess_;
		jiFen_ = other.jiFen_.Clone();
		_unknownFields = UnknownFieldSet.Clone(other._unknownFields);
	}

	[DebuggerNonUserCode]
	public EnterJiFenResp_1017004 Clone()
	{
		return new EnterJiFenResp_1017004(this);
	}

	[DebuggerNonUserCode]
	public override bool Equals(object other)
	{
		return Equals(other as EnterJiFenResp_1017004);
	}

	[DebuggerNonUserCode]
	public bool Equals(EnterJiFenResp_1017004 other)
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
		if (!jiFen_.Equals(other.jiFen_))
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
		num ^= jiFen_.GetHashCode();
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
		jiFen_.WriteTo(output, _repeated_jiFen_codec);
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
		num += jiFen_.CalculateSize(_repeated_jiFen_codec);
		if (_unknownFields != null)
		{
			num += _unknownFields.CalculateSize();
		}
		return num;
	}

	[DebuggerNonUserCode]
	public void MergeFrom(EnterJiFenResp_1017004 other)
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
			jiFen_.Add(other.jiFen_);
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
				jiFen_.AddEntriesFrom(input, _repeated_jiFen_codec);
				break;
			}
		}
	}
}
