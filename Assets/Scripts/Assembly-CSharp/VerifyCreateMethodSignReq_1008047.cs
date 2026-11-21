using System;
using System.Diagnostics;
using Google.Protobuf;
using Google.Protobuf.Collections;
using Google.Protobuf.Reflection;

public sealed class VerifyCreateMethodSignReq_1008047 : IMessage<VerifyCreateMethodSignReq_1008047>, IMessage, IEquatable<VerifyCreateMethodSignReq_1008047>, IDeepCloneable<VerifyCreateMethodSignReq_1008047>
{
	[DebuggerNonUserCode]
	public static class Types
	{
		public sealed class SignPointReq_1008051 : IMessage<SignPointReq_1008051>, IMessage, IEquatable<SignPointReq_1008051>, IDeepCloneable<SignPointReq_1008051>
		{
			private static readonly MessageParser<SignPointReq_1008051> _parser = new MessageParser<SignPointReq_1008051>(() => new SignPointReq_1008051());

			private UnknownFieldSet _unknownFields;

			public const int XFieldNumber = 1;

			private double x_;

			public const int YFieldNumber = 2;

			private double y_;

			[DebuggerNonUserCode]
			public static MessageParser<SignPointReq_1008051> Parser => _parser;

			[DebuggerNonUserCode]
			public static MessageDescriptor Descriptor => VerifyCreateMethodSignReq_1008047.Descriptor.NestedTypes[0];

			[DebuggerNonUserCode]
			MessageDescriptor IMessage.Descriptor => pb_003A_003AGoogle_002EProtobuf_002EIMessage_002Eget_Descriptor();

			[DebuggerNonUserCode]
			public double X
			{
				get
				{
					return x_;
				}
				set
				{
					x_ = value;
				}
			}

			[DebuggerNonUserCode]
			public double Y
			{
				get
				{
					return y_;
				}
				set
				{
					y_ = value;
				}
			}

			private MessageDescriptor pb_003A_003AGoogle_002EProtobuf_002EIMessage_002Eget_Descriptor()
			{
				return Descriptor;
			}

			[DebuggerNonUserCode]
			public SignPointReq_1008051()
			{
			}

			[DebuggerNonUserCode]
			public SignPointReq_1008051(SignPointReq_1008051 other)
				: this()
			{
				x_ = other.x_;
				y_ = other.y_;
				_unknownFields = UnknownFieldSet.Clone(other._unknownFields);
			}

			[DebuggerNonUserCode]
			public SignPointReq_1008051 Clone()
			{
				return new SignPointReq_1008051(this);
			}

			[DebuggerNonUserCode]
			public override bool Equals(object other)
			{
				return Equals(other as SignPointReq_1008051);
			}

			[DebuggerNonUserCode]
			public bool Equals(SignPointReq_1008051 other)
			{
				if (other == null)
				{
					return false;
				}
				if (other == this)
				{
					return true;
				}
				if (!ProtobufEqualityComparers.BitwiseDoubleEqualityComparer.Equals(X, other.X))
				{
					return false;
				}
				if (!ProtobufEqualityComparers.BitwiseDoubleEqualityComparer.Equals(Y, other.Y))
				{
					return false;
				}
				return object.Equals(_unknownFields, other._unknownFields);
			}

			[DebuggerNonUserCode]
			public override int GetHashCode()
			{
				int num = 1;
				if (X != 0.0)
				{
					num ^= ProtobufEqualityComparers.BitwiseDoubleEqualityComparer.GetHashCode(X);
				}
				if (Y != 0.0)
				{
					num ^= ProtobufEqualityComparers.BitwiseDoubleEqualityComparer.GetHashCode(Y);
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
				if (X != 0.0)
				{
					output.WriteRawTag(9);
					output.WriteDouble(X);
				}
				if (Y != 0.0)
				{
					output.WriteRawTag(17);
					output.WriteDouble(Y);
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
				if (X != 0.0)
				{
					num += 9;
				}
				if (Y != 0.0)
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
			public void MergeFrom(SignPointReq_1008051 other)
			{
				if (other != null)
				{
					if (other.X != 0.0)
					{
						X = other.X;
					}
					if (other.Y != 0.0)
					{
						Y = other.Y;
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
					case 9u:
						X = input.ReadDouble();
						break;
					case 17u:
						Y = input.ReadDouble();
						break;
					}
				}
			}
		}
	}

	private static readonly MessageParser<VerifyCreateMethodSignReq_1008047> _parser = new MessageParser<VerifyCreateMethodSignReq_1008047>(() => new VerifyCreateMethodSignReq_1008047());

	private UnknownFieldSet _unknownFields;

	public const int MethodSkillIdFieldNumber = 1;

	private string methodSkillId_ = "";

	public const int PointsFieldNumber = 2;

	private static readonly FieldCodec<Types.SignPointReq_1008051> _repeated_points_codec = FieldCodec.ForMessage(18u, Types.SignPointReq_1008051.Parser);

	private readonly RepeatedField<Types.SignPointReq_1008051> points_ = new RepeatedField<Types.SignPointReq_1008051>();

	[DebuggerNonUserCode]
	public static MessageParser<VerifyCreateMethodSignReq_1008047> Parser => _parser;

	[DebuggerNonUserCode]
	public static MessageDescriptor Descriptor => UnrealWorldProtoReflection.Descriptor.MessageTypes[58];

	[DebuggerNonUserCode]
	MessageDescriptor IMessage.Descriptor => pb_003A_003AGoogle_002EProtobuf_002EIMessage_002Eget_Descriptor();

	[DebuggerNonUserCode]
	public string MethodSkillId
	{
		get
		{
			return methodSkillId_;
		}
		set
		{
			methodSkillId_ = ProtoPreconditions.CheckNotNull(value, "value");
		}
	}

	[DebuggerNonUserCode]
	public RepeatedField<Types.SignPointReq_1008051> Points => points_;

	private MessageDescriptor pb_003A_003AGoogle_002EProtobuf_002EIMessage_002Eget_Descriptor()
	{
		return Descriptor;
	}

	[DebuggerNonUserCode]
	public VerifyCreateMethodSignReq_1008047()
	{
	}

	[DebuggerNonUserCode]
	public VerifyCreateMethodSignReq_1008047(VerifyCreateMethodSignReq_1008047 other)
		: this()
	{
		methodSkillId_ = other.methodSkillId_;
		points_ = other.points_.Clone();
		_unknownFields = UnknownFieldSet.Clone(other._unknownFields);
	}

	[DebuggerNonUserCode]
	public VerifyCreateMethodSignReq_1008047 Clone()
	{
		return new VerifyCreateMethodSignReq_1008047(this);
	}

	[DebuggerNonUserCode]
	public override bool Equals(object other)
	{
		return Equals(other as VerifyCreateMethodSignReq_1008047);
	}

	[DebuggerNonUserCode]
	public bool Equals(VerifyCreateMethodSignReq_1008047 other)
	{
		if (other == null)
		{
			return false;
		}
		if (other == this)
		{
			return true;
		}
		if (MethodSkillId != other.MethodSkillId)
		{
			return false;
		}
		if (!points_.Equals(other.points_))
		{
			return false;
		}
		return object.Equals(_unknownFields, other._unknownFields);
	}

	[DebuggerNonUserCode]
	public override int GetHashCode()
	{
		int num = 1;
		if (MethodSkillId.Length != 0)
		{
			num ^= MethodSkillId.GetHashCode();
		}
		num ^= points_.GetHashCode();
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
		if (MethodSkillId.Length != 0)
		{
			output.WriteRawTag(10);
			output.WriteString(MethodSkillId);
		}
		points_.WriteTo(output, _repeated_points_codec);
		if (_unknownFields != null)
		{
			_unknownFields.WriteTo(output);
		}
	}

	[DebuggerNonUserCode]
	public int CalculateSize()
	{
		int num = 0;
		if (MethodSkillId.Length != 0)
		{
			num += 1 + CodedOutputStream.ComputeStringSize(MethodSkillId);
		}
		num += points_.CalculateSize(_repeated_points_codec);
		if (_unknownFields != null)
		{
			num += _unknownFields.CalculateSize();
		}
		return num;
	}

	[DebuggerNonUserCode]
	public void MergeFrom(VerifyCreateMethodSignReq_1008047 other)
	{
		if (other != null)
		{
			if (other.MethodSkillId.Length != 0)
			{
				MethodSkillId = other.MethodSkillId;
			}
			points_.Add(other.points_);
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
				MethodSkillId = input.ReadString();
				break;
			case 18u:
				points_.AddEntriesFrom(input, _repeated_points_codec);
				break;
			}
		}
	}
}
