using System;
using System.Diagnostics;
using Google.Protobuf;
using Google.Protobuf.Collections;
using Google.Protobuf.Reflection;

public sealed class VerifyCreateMethodMatrixReq_1008088 : IMessage<VerifyCreateMethodMatrixReq_1008088>, IMessage, IEquatable<VerifyCreateMethodMatrixReq_1008088>, IDeepCloneable<VerifyCreateMethodMatrixReq_1008088>
{
	private static readonly MessageParser<VerifyCreateMethodMatrixReq_1008088> _parser = new MessageParser<VerifyCreateMethodMatrixReq_1008088>(() => new VerifyCreateMethodMatrixReq_1008088());

	private UnknownFieldSet _unknownFields;

	public const int StageFieldNumber = 1;

	private int stage_;

	public const int WaiQuanFieldNumber = 2;

	private static readonly FieldCodec<int> _repeated_waiQuan_codec = FieldCodec.ForInt32(18u);

	private readonly RepeatedField<int> waiQuan_ = new RepeatedField<int>();

	public const int ZhongQuanFieldNumber = 3;

	private static readonly FieldCodec<int> _repeated_zhongQuan_codec = FieldCodec.ForInt32(26u);

	private readonly RepeatedField<int> zhongQuan_ = new RepeatedField<int>();

	public const int NeiQuanFieldNumber = 4;

	private static readonly FieldCodec<int> _repeated_neiQuan_codec = FieldCodec.ForInt32(34u);

	private readonly RepeatedField<int> neiQuan_ = new RepeatedField<int>();

	public const int ZhongDianFieldNumber = 5;

	private int zhongDian_;

	[DebuggerNonUserCode]
	public static MessageParser<VerifyCreateMethodMatrixReq_1008088> Parser => _parser;

	[DebuggerNonUserCode]
	public static MessageDescriptor Descriptor => UnrealWorldProtoReflection.Descriptor.MessageTypes[106];

	[DebuggerNonUserCode]
	MessageDescriptor IMessage.Descriptor => pb_003A_003AGoogle_002EProtobuf_002EIMessage_002Eget_Descriptor();

	[DebuggerNonUserCode]
	public int Stage
	{
		get
		{
			return stage_;
		}
		set
		{
			stage_ = value;
		}
	}

	[DebuggerNonUserCode]
	public RepeatedField<int> WaiQuan => waiQuan_;

	[DebuggerNonUserCode]
	public RepeatedField<int> ZhongQuan => zhongQuan_;

	[DebuggerNonUserCode]
	public RepeatedField<int> NeiQuan => neiQuan_;

	[DebuggerNonUserCode]
	public int ZhongDian
	{
		get
		{
			return zhongDian_;
		}
		set
		{
			zhongDian_ = value;
		}
	}

	private MessageDescriptor pb_003A_003AGoogle_002EProtobuf_002EIMessage_002Eget_Descriptor()
	{
		return Descriptor;
	}

	[DebuggerNonUserCode]
	public VerifyCreateMethodMatrixReq_1008088()
	{
	}

	[DebuggerNonUserCode]
	public VerifyCreateMethodMatrixReq_1008088(VerifyCreateMethodMatrixReq_1008088 other)
		: this()
	{
		stage_ = other.stage_;
		waiQuan_ = other.waiQuan_.Clone();
		zhongQuan_ = other.zhongQuan_.Clone();
		neiQuan_ = other.neiQuan_.Clone();
		zhongDian_ = other.zhongDian_;
		_unknownFields = UnknownFieldSet.Clone(other._unknownFields);
	}

	[DebuggerNonUserCode]
	public VerifyCreateMethodMatrixReq_1008088 Clone()
	{
		return new VerifyCreateMethodMatrixReq_1008088(this);
	}

	[DebuggerNonUserCode]
	public override bool Equals(object other)
	{
		return Equals(other as VerifyCreateMethodMatrixReq_1008088);
	}

	[DebuggerNonUserCode]
	public bool Equals(VerifyCreateMethodMatrixReq_1008088 other)
	{
		if (other == null)
		{
			return false;
		}
		if (other == this)
		{
			return true;
		}
		if (Stage != other.Stage)
		{
			return false;
		}
		if (!waiQuan_.Equals(other.waiQuan_))
		{
			return false;
		}
		if (!zhongQuan_.Equals(other.zhongQuan_))
		{
			return false;
		}
		if (!neiQuan_.Equals(other.neiQuan_))
		{
			return false;
		}
		if (ZhongDian != other.ZhongDian)
		{
			return false;
		}
		return object.Equals(_unknownFields, other._unknownFields);
	}

	[DebuggerNonUserCode]
	public override int GetHashCode()
	{
		int num = 1;
		if (Stage != 0)
		{
			num ^= Stage.GetHashCode();
		}
		num ^= waiQuan_.GetHashCode();
		num ^= zhongQuan_.GetHashCode();
		num ^= neiQuan_.GetHashCode();
		if (ZhongDian != 0)
		{
			num ^= ZhongDian.GetHashCode();
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
		if (Stage != 0)
		{
			output.WriteRawTag(8);
			output.WriteInt32(Stage);
		}
		waiQuan_.WriteTo(output, _repeated_waiQuan_codec);
		zhongQuan_.WriteTo(output, _repeated_zhongQuan_codec);
		neiQuan_.WriteTo(output, _repeated_neiQuan_codec);
		if (ZhongDian != 0)
		{
			output.WriteRawTag(40);
			output.WriteInt32(ZhongDian);
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
		if (Stage != 0)
		{
			num += 1 + CodedOutputStream.ComputeInt32Size(Stage);
		}
		num += waiQuan_.CalculateSize(_repeated_waiQuan_codec);
		num += zhongQuan_.CalculateSize(_repeated_zhongQuan_codec);
		num += neiQuan_.CalculateSize(_repeated_neiQuan_codec);
		if (ZhongDian != 0)
		{
			num += 1 + CodedOutputStream.ComputeInt32Size(ZhongDian);
		}
		if (_unknownFields != null)
		{
			num += _unknownFields.CalculateSize();
		}
		return num;
	}

	[DebuggerNonUserCode]
	public void MergeFrom(VerifyCreateMethodMatrixReq_1008088 other)
	{
		if (other != null)
		{
			if (other.Stage != 0)
			{
				Stage = other.Stage;
			}
			waiQuan_.Add(other.waiQuan_);
			zhongQuan_.Add(other.zhongQuan_);
			neiQuan_.Add(other.neiQuan_);
			if (other.ZhongDian != 0)
			{
				ZhongDian = other.ZhongDian;
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
				Stage = input.ReadInt32();
				break;
			case 16u:
			case 18u:
				waiQuan_.AddEntriesFrom(input, _repeated_waiQuan_codec);
				break;
			case 24u:
			case 26u:
				zhongQuan_.AddEntriesFrom(input, _repeated_zhongQuan_codec);
				break;
			case 32u:
			case 34u:
				neiQuan_.AddEntriesFrom(input, _repeated_neiQuan_codec);
				break;
			case 40u:
				ZhongDian = input.ReadInt32();
				break;
			}
		}
	}
}
