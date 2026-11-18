using System;
using System.Diagnostics;
using Google.Protobuf;
using Google.Protobuf.Reflection;

public sealed class EnterHomeResp_1005001 : IMessage<EnterHomeResp_1005001>, IMessage, IEquatable<EnterHomeResp_1005001>, IDeepCloneable<EnterHomeResp_1005001>
{
	private static readonly MessageParser<EnterHomeResp_1005001> _parser = new MessageParser<EnterHomeResp_1005001>(() => new EnterHomeResp_1005001());

	private UnknownFieldSet _unknownFields;

	public const int CodeFieldNumber = 1;

	private int code_;

	public const int IsSuccessFieldNumber = 2;

	private bool isSuccess_;

	public const int NameFieldNumber = 3;

	private string name_ = "";

	public const int RankFieldNumber = 4;

	private int rank_;

	public const int Live1TypeFieldNumber = 5;

	private int live1Type_;

	public const int Live2TypeFieldNumber = 6;

	private int live2Type_;

	[DebuggerNonUserCode]
	public static MessageParser<EnterHomeResp_1005001> Parser => _parser;

	[DebuggerNonUserCode]
	public static MessageDescriptor Descriptor => HomeProtoReflection.Descriptor.MessageTypes[0];

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

	[DebuggerNonUserCode]
	public int Live1Type
	{
		get
		{
			return live1Type_;
		}
		set
		{
			live1Type_ = value;
		}
	}

	[DebuggerNonUserCode]
	public int Live2Type
	{
		get
		{
			return live2Type_;
		}
		set
		{
			live2Type_ = value;
		}
	}

	private MessageDescriptor pb_003A_003AGoogle_002EProtobuf_002EIMessage_002Eget_Descriptor()
	{
		return Descriptor;
	}

	[DebuggerNonUserCode]
	public EnterHomeResp_1005001()
	{
	}

	[DebuggerNonUserCode]
	public EnterHomeResp_1005001(EnterHomeResp_1005001 other)
		: this()
	{
		code_ = other.code_;
		isSuccess_ = other.isSuccess_;
		name_ = other.name_;
		rank_ = other.rank_;
		live1Type_ = other.live1Type_;
		live2Type_ = other.live2Type_;
		_unknownFields = UnknownFieldSet.Clone(other._unknownFields);
	}

	[DebuggerNonUserCode]
	public EnterHomeResp_1005001 Clone()
	{
		return new EnterHomeResp_1005001(this);
	}

	[DebuggerNonUserCode]
	public override bool Equals(object other)
	{
		return Equals(other as EnterHomeResp_1005001);
	}

	[DebuggerNonUserCode]
	public bool Equals(EnterHomeResp_1005001 other)
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
		if (Name != other.Name)
		{
			return false;
		}
		if (Rank != other.Rank)
		{
			return false;
		}
		if (Live1Type != other.Live1Type)
		{
			return false;
		}
		if (Live2Type != other.Live2Type)
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
		if (Name.Length != 0)
		{
			num ^= Name.GetHashCode();
		}
		if (Rank != 0)
		{
			num ^= Rank.GetHashCode();
		}
		if (Live1Type != 0)
		{
			num ^= Live1Type.GetHashCode();
		}
		if (Live2Type != 0)
		{
			num ^= Live2Type.GetHashCode();
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
		if (Name.Length != 0)
		{
			output.WriteRawTag(26);
			output.WriteString(Name);
		}
		if (Rank != 0)
		{
			output.WriteRawTag(32);
			output.WriteInt32(Rank);
		}
		if (Live1Type != 0)
		{
			output.WriteRawTag(40);
			output.WriteInt32(Live1Type);
		}
		if (Live2Type != 0)
		{
			output.WriteRawTag(48);
			output.WriteInt32(Live2Type);
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
		if (Name.Length != 0)
		{
			num += 1 + CodedOutputStream.ComputeStringSize(Name);
		}
		if (Rank != 0)
		{
			num += 1 + CodedOutputStream.ComputeInt32Size(Rank);
		}
		if (Live1Type != 0)
		{
			num += 1 + CodedOutputStream.ComputeInt32Size(Live1Type);
		}
		if (Live2Type != 0)
		{
			num += 1 + CodedOutputStream.ComputeInt32Size(Live2Type);
		}
		if (_unknownFields != null)
		{
			num += _unknownFields.CalculateSize();
		}
		return num;
	}

	[DebuggerNonUserCode]
	public void MergeFrom(EnterHomeResp_1005001 other)
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
			if (other.Name.Length != 0)
			{
				Name = other.Name;
			}
			if (other.Rank != 0)
			{
				Rank = other.Rank;
			}
			if (other.Live1Type != 0)
			{
				Live1Type = other.Live1Type;
			}
			if (other.Live2Type != 0)
			{
				Live2Type = other.Live2Type;
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
				Name = input.ReadString();
				break;
			case 32u:
				Rank = input.ReadInt32();
				break;
			case 40u:
				Live1Type = input.ReadInt32();
				break;
			case 48u:
				Live2Type = input.ReadInt32();
				break;
			}
		}
	}
}
