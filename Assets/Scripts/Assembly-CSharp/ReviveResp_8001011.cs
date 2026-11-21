using System;
using System.Diagnostics;
using Google.Protobuf;
using Google.Protobuf.Collections;
using Google.Protobuf.Reflection;

public sealed class ReviveResp_8001011 : IMessage<ReviveResp_8001011>, IMessage, IEquatable<ReviveResp_8001011>, IDeepCloneable<ReviveResp_8001011>
{
	private static readonly MessageParser<ReviveResp_8001011> _parser = new MessageParser<ReviveResp_8001011>(() => new ReviveResp_8001011());

	private UnknownFieldSet _unknownFields;

	public const int CodeFieldNumber = 1;

	private int code_;

	public const int IsSuccessFieldNumber = 2;

	private bool isSuccess_;

	public const int TiShenKuiLeiCountFieldNumber = 3;

	private int tiShenKuiLeiCount_;

	public const int PosXFieldNumber = 4;

	private double posX_;

	public const int PosYFieldNumber = 5;

	private double posY_;

	[DebuggerNonUserCode]
	public static MessageParser<ReviveResp_8001011> Parser => _parser;

	[DebuggerNonUserCode]
	public static MessageDescriptor Descriptor => ClientBattleProtoReflection.Descriptor.MessageTypes[15];

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
	public int TiShenKuiLeiCount
	{
		get
		{
			return tiShenKuiLeiCount_;
		}
		set
		{
			tiShenKuiLeiCount_ = value;
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

	private MessageDescriptor pb_003A_003AGoogle_002EProtobuf_002EIMessage_002Eget_Descriptor()
	{
		return Descriptor;
	}

	[DebuggerNonUserCode]
	public ReviveResp_8001011()
	{
	}

	[DebuggerNonUserCode]
	public ReviveResp_8001011(ReviveResp_8001011 other)
		: this()
	{
		code_ = other.code_;
		isSuccess_ = other.isSuccess_;
		tiShenKuiLeiCount_ = other.tiShenKuiLeiCount_;
		posX_ = other.posX_;
		posY_ = other.posY_;
		_unknownFields = UnknownFieldSet.Clone(other._unknownFields);
	}

	[DebuggerNonUserCode]
	public ReviveResp_8001011 Clone()
	{
		return new ReviveResp_8001011(this);
	}

	[DebuggerNonUserCode]
	public override bool Equals(object other)
	{
		return Equals(other as ReviveResp_8001011);
	}

	[DebuggerNonUserCode]
	public bool Equals(ReviveResp_8001011 other)
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
		if (TiShenKuiLeiCount != other.TiShenKuiLeiCount)
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
		if (TiShenKuiLeiCount != 0)
		{
			num ^= TiShenKuiLeiCount.GetHashCode();
		}
		if (PosX != 0.0)
		{
			num ^= ProtobufEqualityComparers.BitwiseDoubleEqualityComparer.GetHashCode(PosX);
		}
		if (PosY != 0.0)
		{
			num ^= ProtobufEqualityComparers.BitwiseDoubleEqualityComparer.GetHashCode(PosY);
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
		if (TiShenKuiLeiCount != 0)
		{
			output.WriteRawTag(24);
			output.WriteInt32(TiShenKuiLeiCount);
		}
		if (PosX != 0.0)
		{
			output.WriteRawTag(33);
			output.WriteDouble(PosX);
		}
		if (PosY != 0.0)
		{
			output.WriteRawTag(41);
			output.WriteDouble(PosY);
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
		if (TiShenKuiLeiCount != 0)
		{
			num += 1 + CodedOutputStream.ComputeInt32Size(TiShenKuiLeiCount);
		}
		if (PosX != 0.0)
		{
			num += 9;
		}
		if (PosY != 0.0)
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
	public void MergeFrom(ReviveResp_8001011 other)
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
			if (other.TiShenKuiLeiCount != 0)
			{
				TiShenKuiLeiCount = other.TiShenKuiLeiCount;
			}
			if (other.PosX != 0.0)
			{
				PosX = other.PosX;
			}
			if (other.PosY != 0.0)
			{
				PosY = other.PosY;
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
			case 24u:
				TiShenKuiLeiCount = input.ReadInt32();
				break;
			case 33u:
				PosX = input.ReadDouble();
				break;
			case 41u:
				PosY = input.ReadDouble();
				break;
			}
		}
	}
}
