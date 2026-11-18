using System;
using System.Diagnostics;
using Google.Protobuf;
using Google.Protobuf.Collections;
using Google.Protobuf.Reflection;

public sealed class SearchSignRefiningStepListResp_1005034 : IMessage<SearchSignRefiningStepListResp_1005034>, IMessage, IEquatable<SearchSignRefiningStepListResp_1005034>, IDeepCloneable<SearchSignRefiningStepListResp_1005034>
{
	[DebuggerNonUserCode]
	public static class Types
	{
		public sealed class PointsResp_1005035 : IMessage<PointsResp_1005035>, IMessage, IEquatable<PointsResp_1005035>, IDeepCloneable<PointsResp_1005035>
		{
			private static readonly MessageParser<PointsResp_1005035> _parser = new MessageParser<PointsResp_1005035>(() => new PointsResp_1005035());

			private UnknownFieldSet _unknownFields;

			public const int XFieldNumber = 1;

			private double x_;

			public const int YFieldNumber = 2;

			private double y_;

			[DebuggerNonUserCode]
			public static MessageParser<PointsResp_1005035> Parser => _parser;

			[DebuggerNonUserCode]
			public static MessageDescriptor Descriptor => SearchSignRefiningStepListResp_1005034.Descriptor.NestedTypes[0];

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
			public PointsResp_1005035()
			{
			}

			[DebuggerNonUserCode]
			public PointsResp_1005035(PointsResp_1005035 other)
				: this()
			{
				x_ = other.x_;
				y_ = other.y_;
				_unknownFields = UnknownFieldSet.Clone(other._unknownFields);
			}

			[DebuggerNonUserCode]
			public PointsResp_1005035 Clone()
			{
				return new PointsResp_1005035(this);
			}

			[DebuggerNonUserCode]
			public override bool Equals(object other)
			{
				return Equals(other as PointsResp_1005035);
			}

			[DebuggerNonUserCode]
			public bool Equals(PointsResp_1005035 other)
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
			public void MergeFrom(PointsResp_1005035 other)
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

	private static readonly MessageParser<SearchSignRefiningStepListResp_1005034> _parser = new MessageParser<SearchSignRefiningStepListResp_1005034>(() => new SearchSignRefiningStepListResp_1005034());

	private UnknownFieldSet _unknownFields;

	public const int CodeFieldNumber = 1;

	private int code_;

	public const int IsSuccessFieldNumber = 2;

	private bool isSuccess_;

	public const int PointFieldNumber = 3;

	private static readonly FieldCodec<Types.PointsResp_1005035> _repeated_point_codec = FieldCodec.ForMessage(26u, Types.PointsResp_1005035.Parser);

	private readonly RepeatedField<Types.PointsResp_1005035> point_ = new RepeatedField<Types.PointsResp_1005035>();

	public const int RankFieldNumber = 4;

	private int rank_;

	[DebuggerNonUserCode]
	public static MessageParser<SearchSignRefiningStepListResp_1005034> Parser => _parser;

	[DebuggerNonUserCode]
	public static MessageDescriptor Descriptor => HomeProtoReflection.Descriptor.MessageTypes[44];

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
	public RepeatedField<Types.PointsResp_1005035> Point => point_;

	[DebuggerNonUserCode]
	public int Rank
	{
		get
		{
			return rank_;
		}
		set
		{
			rank_ = value;
		}
	}

	private MessageDescriptor pb_003A_003AGoogle_002EProtobuf_002EIMessage_002Eget_Descriptor()
	{
		return Descriptor;
	}

	[DebuggerNonUserCode]
	public SearchSignRefiningStepListResp_1005034()
	{
	}

	[DebuggerNonUserCode]
	public SearchSignRefiningStepListResp_1005034(SearchSignRefiningStepListResp_1005034 other)
		: this()
	{
		code_ = other.code_;
		isSuccess_ = other.isSuccess_;
		point_ = other.point_.Clone();
		rank_ = other.rank_;
		_unknownFields = UnknownFieldSet.Clone(other._unknownFields);
	}

	[DebuggerNonUserCode]
	public SearchSignRefiningStepListResp_1005034 Clone()
	{
		return new SearchSignRefiningStepListResp_1005034(this);
	}

	[DebuggerNonUserCode]
	public override bool Equals(object other)
	{
		return Equals(other as SearchSignRefiningStepListResp_1005034);
	}

	[DebuggerNonUserCode]
	public bool Equals(SearchSignRefiningStepListResp_1005034 other)
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
		if (!point_.Equals(other.point_))
		{
			return false;
		}
		if (Rank != other.Rank)
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
		num ^= point_.GetHashCode();
		if (Rank != 0)
		{
			num ^= Rank.GetHashCode();
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
		point_.WriteTo(output, _repeated_point_codec);
		if (Rank != 0)
		{
			output.WriteRawTag(32);
			output.WriteInt32(Rank);
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
		num += point_.CalculateSize(_repeated_point_codec);
		if (Rank != 0)
		{
			num += 1 + CodedOutputStream.ComputeInt32Size(Rank);
		}
		if (_unknownFields != null)
		{
			num += _unknownFields.CalculateSize();
		}
		return num;
	}

	[DebuggerNonUserCode]
	public void MergeFrom(SearchSignRefiningStepListResp_1005034 other)
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
			point_.Add(other.point_);
			if (other.Rank != 0)
			{
				Rank = other.Rank;
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
				point_.AddEntriesFrom(input, _repeated_point_codec);
				break;
			case 32u:
				Rank = input.ReadInt32();
				break;
			}
		}
	}
}
