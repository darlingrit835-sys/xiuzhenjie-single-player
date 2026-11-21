using System;
using System.Diagnostics;
using Google.Protobuf;
using Google.Protobuf.Collections;
using Google.Protobuf.Reflection;

public sealed class BattleBroadcastPosResp_8002001 : IMessage<BattleBroadcastPosResp_8002001>, IMessage, IEquatable<BattleBroadcastPosResp_8002001>, IDeepCloneable<BattleBroadcastPosResp_8002001>
{
	[DebuggerNonUserCode]
	public static class Types
	{
		public sealed class ObjectPosionResp_8002002 : IMessage<ObjectPosionResp_8002002>, IMessage, IEquatable<ObjectPosionResp_8002002>, IDeepCloneable<ObjectPosionResp_8002002>
		{
			private static readonly MessageParser<ObjectPosionResp_8002002> _parser = new MessageParser<ObjectPosionResp_8002002>(() => new ObjectPosionResp_8002002());

			private UnknownFieldSet _unknownFields;

			public const int ObjectIndexFieldNumber = 1;

			private int objectIndex_;

			public const int PosXFieldNumber = 2;

			private double posX_;

			public const int PosYFieldNumber = 3;

			private double posY_;

			public const int AngleFieldNumber = 4;

			private double angle_;

			[DebuggerNonUserCode]
			public static MessageParser<ObjectPosionResp_8002002> Parser => _parser;

			[DebuggerNonUserCode]
			public static MessageDescriptor Descriptor => BattleBroadcastPosResp_8002001.Descriptor.NestedTypes[0];

			[DebuggerNonUserCode]
			MessageDescriptor IMessage.Descriptor => pb_003A_003AGoogle_002EProtobuf_002EIMessage_002Eget_Descriptor();

			[DebuggerNonUserCode]
			public int ObjectIndex
			{
				get
				{
					return objectIndex_;
				}
				set
				{
					objectIndex_ = value;
				}
			}

			[DebuggerNonUserCode]
			public double PosX
			{
				get
				{
					return posX_;
				}
				set
				{
					posX_ = value;
				}
			}

			[DebuggerNonUserCode]
			public double PosY
			{
				get
				{
					return posY_;
				}
				set
				{
					posY_ = value;
				}
			}

			[DebuggerNonUserCode]
			public double Angle
			{
				get
				{
					return angle_;
				}
				set
				{
					angle_ = value;
				}
			}

			private MessageDescriptor pb_003A_003AGoogle_002EProtobuf_002EIMessage_002Eget_Descriptor()
			{
				return Descriptor;
			}

			[DebuggerNonUserCode]
			public ObjectPosionResp_8002002()
			{
			}

			[DebuggerNonUserCode]
			public ObjectPosionResp_8002002(ObjectPosionResp_8002002 other)
				: this()
			{
				objectIndex_ = other.objectIndex_;
				posX_ = other.posX_;
				posY_ = other.posY_;
				angle_ = other.angle_;
				_unknownFields = UnknownFieldSet.Clone(other._unknownFields);
			}

			[DebuggerNonUserCode]
			public ObjectPosionResp_8002002 Clone()
			{
				return new ObjectPosionResp_8002002(this);
			}

			[DebuggerNonUserCode]
			public override bool Equals(object other)
			{
				return Equals(other as ObjectPosionResp_8002002);
			}

			[DebuggerNonUserCode]
			public bool Equals(ObjectPosionResp_8002002 other)
			{
				if (other == null)
				{
					return false;
				}
				if (other == this)
				{
					return true;
				}
				if (ObjectIndex != other.ObjectIndex)
				{
					return false;
				}
				if (!ProtobufEqualityComparers.BitwiseDoubleEqualityComparer.Equals(PosX, other.PosX))
				{
					return false;
				}
				if (!ProtobufEqualityComparers.BitwiseDoubleEqualityComparer.Equals(PosY, other.PosY))
				{
					return false;
				}
				if (!ProtobufEqualityComparers.BitwiseDoubleEqualityComparer.Equals(Angle, other.Angle))
				{
					return false;
				}
				return object.Equals(_unknownFields, other._unknownFields);
			}

			[DebuggerNonUserCode]
			public override int GetHashCode()
			{
				int num = 1;
				if (ObjectIndex != 0)
				{
					num ^= ObjectIndex.GetHashCode();
				}
				if (PosX != 0.0)
				{
					num ^= ProtobufEqualityComparers.BitwiseDoubleEqualityComparer.GetHashCode(PosX);
				}
				if (PosY != 0.0)
				{
					num ^= ProtobufEqualityComparers.BitwiseDoubleEqualityComparer.GetHashCode(PosY);
				}
				if (Angle != 0.0)
				{
					num ^= ProtobufEqualityComparers.BitwiseDoubleEqualityComparer.GetHashCode(Angle);
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
				if (ObjectIndex != 0)
				{
					output.WriteRawTag(8);
					output.WriteInt32(ObjectIndex);
				}
				if (PosX != 0.0)
				{
					output.WriteRawTag(17);
					output.WriteDouble(PosX);
				}
				if (PosY != 0.0)
				{
					output.WriteRawTag(25);
					output.WriteDouble(PosY);
				}
				if (Angle != 0.0)
				{
					output.WriteRawTag(33);
					output.WriteDouble(Angle);
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
				if (ObjectIndex != 0)
				{
					num += 1 + CodedOutputStream.ComputeInt32Size(ObjectIndex);
				}
				if (PosX != 0.0)
				{
					num += 9;
				}
				if (PosY != 0.0)
				{
					num += 9;
				}
				if (Angle != 0.0)
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
			public void MergeFrom(ObjectPosionResp_8002002 other)
			{
				if (other != null)
				{
					if (other.ObjectIndex != 0)
					{
						ObjectIndex = other.ObjectIndex;
					}
					if (other.PosX != 0.0)
					{
						PosX = other.PosX;
					}
					if (other.PosY != 0.0)
					{
						PosY = other.PosY;
					}
					if (other.Angle != 0.0)
					{
						Angle = other.Angle;
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
						ObjectIndex = input.ReadInt32();
						break;
					case 17u:
						PosX = input.ReadDouble();
						break;
					case 25u:
						PosY = input.ReadDouble();
						break;
					case 33u:
						Angle = input.ReadDouble();
						break;
					}
				}
			}
		}
	}

	private static readonly MessageParser<BattleBroadcastPosResp_8002001> _parser = new MessageParser<BattleBroadcastPosResp_8002001>(() => new BattleBroadcastPosResp_8002001());

	private UnknownFieldSet _unknownFields;

	public const int ObjectPosinsFieldNumber = 1;

	private static readonly FieldCodec<Types.ObjectPosionResp_8002002> _repeated_objectPosins_codec = FieldCodec.ForMessage(10u, Types.ObjectPosionResp_8002002.Parser);

	private readonly RepeatedField<Types.ObjectPosionResp_8002002> objectPosins_ = new RepeatedField<Types.ObjectPosionResp_8002002>();

	[DebuggerNonUserCode]
	public static MessageParser<BattleBroadcastPosResp_8002001> Parser => _parser;

	[DebuggerNonUserCode]
	public static MessageDescriptor Descriptor => ClientBattleProtoReflection.Descriptor.MessageTypes[16];

	[DebuggerNonUserCode]
	MessageDescriptor IMessage.Descriptor => pb_003A_003AGoogle_002EProtobuf_002EIMessage_002Eget_Descriptor();

	[DebuggerNonUserCode]
	public RepeatedField<Types.ObjectPosionResp_8002002> ObjectPosins => objectPosins_;

	private MessageDescriptor pb_003A_003AGoogle_002EProtobuf_002EIMessage_002Eget_Descriptor()
	{
		return Descriptor;
	}

	[DebuggerNonUserCode]
	public BattleBroadcastPosResp_8002001()
	{
	}

	[DebuggerNonUserCode]
	public BattleBroadcastPosResp_8002001(BattleBroadcastPosResp_8002001 other)
		: this()
	{
		objectPosins_ = other.objectPosins_.Clone();
		_unknownFields = UnknownFieldSet.Clone(other._unknownFields);
	}

	[DebuggerNonUserCode]
	public BattleBroadcastPosResp_8002001 Clone()
	{
		return new BattleBroadcastPosResp_8002001(this);
	}

	[DebuggerNonUserCode]
	public override bool Equals(object other)
	{
		return Equals(other as BattleBroadcastPosResp_8002001);
	}

	[DebuggerNonUserCode]
	public bool Equals(BattleBroadcastPosResp_8002001 other)
	{
		if (other == null)
		{
			return false;
		}
		if (other == this)
		{
			return true;
		}
		if (!objectPosins_.Equals(other.objectPosins_))
		{
			return false;
		}
		return object.Equals(_unknownFields, other._unknownFields);
	}

	[DebuggerNonUserCode]
	public override int GetHashCode()
	{
		int num = 1;
		num ^= objectPosins_.GetHashCode();
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
		objectPosins_.WriteTo(output, _repeated_objectPosins_codec);
		if (_unknownFields != null)
		{
			_unknownFields.WriteTo(output);
		}
	}

	[DebuggerNonUserCode]
	public int CalculateSize()
	{
		int num = 0;
		num += objectPosins_.CalculateSize(_repeated_objectPosins_codec);
		if (_unknownFields != null)
		{
			num += _unknownFields.CalculateSize();
		}
		return num;
	}

	[DebuggerNonUserCode]
	public void MergeFrom(BattleBroadcastPosResp_8002001 other)
	{
		if (other != null)
		{
			objectPosins_.Add(other.objectPosins_);
			_unknownFields = UnknownFieldSet.MergeFrom(_unknownFields, other._unknownFields);
		}
	}

	[DebuggerNonUserCode]
	public void MergeFrom(CodedInputStream input)
	{
		uint num;
		while ((num = input.ReadTag()) != 0)
		{
			if (num != 10)
			{
				_unknownFields = UnknownFieldSet.MergeFieldFrom(_unknownFields, input);
			}
			else
			{
				objectPosins_.AddEntriesFrom(input, _repeated_objectPosins_codec);
			}
		}
	}
}
