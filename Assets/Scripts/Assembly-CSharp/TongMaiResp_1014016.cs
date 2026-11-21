using System;
using System.Diagnostics;
using Google.Protobuf;
using Google.Protobuf.Reflection;

public sealed class TongMaiResp_1014016 : IMessage<TongMaiResp_1014016>, IMessage, IEquatable<TongMaiResp_1014016>, IDeepCloneable<TongMaiResp_1014016>
{
	private static readonly MessageParser<TongMaiResp_1014016> _parser = new MessageParser<TongMaiResp_1014016>(() => new TongMaiResp_1014016());

	private UnknownFieldSet _unknownFields;

	public const int CodeFieldNumber = 1;

	private int code_;

	public const int IsSuccessFieldNumber = 2;

	private bool isSuccess_;

	public const int IntimacyFieldNumber = 3;

	private int intimacy_;

	public const int CurrentVigorFieldNumber = 4;

	private int currentVigor_;

	public const int TuDiAcupointPointFieldNumber = 5;

	private int tuDiAcupointPoint_;

	[DebuggerNonUserCode]
	public static MessageParser<TongMaiResp_1014016> Parser => _parser;

	[DebuggerNonUserCode]
	public static MessageDescriptor Descriptor => FriendProtoReflection.Descriptor.MessageTypes[17];

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
	public int Intimacy
	{
		get
		{
			return intimacy_;
		}
		set
		{
			intimacy_ = value;
		}
	}

	[DebuggerNonUserCode]
	public int CurrentVigor
	{
		get
		{
			return currentVigor_;
		}
		set
		{
			currentVigor_ = value;
		}
	}

	[DebuggerNonUserCode]
	public int TuDiAcupointPoint
	{
		get
		{
			return tuDiAcupointPoint_;
		}
		set
		{
			tuDiAcupointPoint_ = value;
		}
	}

	private MessageDescriptor pb_003A_003AGoogle_002EProtobuf_002EIMessage_002Eget_Descriptor()
	{
		return Descriptor;
	}

	[DebuggerNonUserCode]
	public TongMaiResp_1014016()
	{
	}

	[DebuggerNonUserCode]
	public TongMaiResp_1014016(TongMaiResp_1014016 other)
		: this()
	{
		code_ = other.code_;
		isSuccess_ = other.isSuccess_;
		intimacy_ = other.intimacy_;
		currentVigor_ = other.currentVigor_;
		tuDiAcupointPoint_ = other.tuDiAcupointPoint_;
		_unknownFields = UnknownFieldSet.Clone(other._unknownFields);
	}

	[DebuggerNonUserCode]
	public TongMaiResp_1014016 Clone()
	{
		return new TongMaiResp_1014016(this);
	}

	[DebuggerNonUserCode]
	public override bool Equals(object other)
	{
		return Equals(other as TongMaiResp_1014016);
	}

	[DebuggerNonUserCode]
	public bool Equals(TongMaiResp_1014016 other)
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
		if (Intimacy != other.Intimacy)
		{
			return false;
		}
		if (CurrentVigor != other.CurrentVigor)
		{
			return false;
		}
		if (TuDiAcupointPoint != other.TuDiAcupointPoint)
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
		if (Intimacy != 0)
		{
			num ^= Intimacy.GetHashCode();
		}
		if (CurrentVigor != 0)
		{
			num ^= CurrentVigor.GetHashCode();
		}
		if (TuDiAcupointPoint != 0)
		{
			num ^= TuDiAcupointPoint.GetHashCode();
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
		if (Intimacy != 0)
		{
			output.WriteRawTag(24);
			output.WriteInt32(Intimacy);
		}
		if (CurrentVigor != 0)
		{
			output.WriteRawTag(32);
			output.WriteInt32(CurrentVigor);
		}
		if (TuDiAcupointPoint != 0)
		{
			output.WriteRawTag(40);
			output.WriteInt32(TuDiAcupointPoint);
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
		if (Intimacy != 0)
		{
			num += 1 + CodedOutputStream.ComputeInt32Size(Intimacy);
		}
		if (CurrentVigor != 0)
		{
			num += 1 + CodedOutputStream.ComputeInt32Size(CurrentVigor);
		}
		if (TuDiAcupointPoint != 0)
		{
			num += 1 + CodedOutputStream.ComputeInt32Size(TuDiAcupointPoint);
		}
		if (_unknownFields != null)
		{
			num += _unknownFields.CalculateSize();
		}
		return num;
	}

	[DebuggerNonUserCode]
	public void MergeFrom(TongMaiResp_1014016 other)
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
			if (other.Intimacy != 0)
			{
				Intimacy = other.Intimacy;
			}
			if (other.CurrentVigor != 0)
			{
				CurrentVigor = other.CurrentVigor;
			}
			if (other.TuDiAcupointPoint != 0)
			{
				TuDiAcupointPoint = other.TuDiAcupointPoint;
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
				Intimacy = input.ReadInt32();
				break;
			case 32u:
				CurrentVigor = input.ReadInt32();
				break;
			case 40u:
				TuDiAcupointPoint = input.ReadInt32();
				break;
			}
		}
	}
}
