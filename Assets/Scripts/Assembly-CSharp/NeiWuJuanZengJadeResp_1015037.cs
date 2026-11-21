using System;
using System.Diagnostics;
using Google.Protobuf;
using Google.Protobuf.Reflection;

public sealed class NeiWuJuanZengJadeResp_1015037 : IMessage<NeiWuJuanZengJadeResp_1015037>, IMessage, IEquatable<NeiWuJuanZengJadeResp_1015037>, IDeepCloneable<NeiWuJuanZengJadeResp_1015037>
{
	private static readonly MessageParser<NeiWuJuanZengJadeResp_1015037> _parser = new MessageParser<NeiWuJuanZengJadeResp_1015037>(() => new NeiWuJuanZengJadeResp_1015037());

	private UnknownFieldSet _unknownFields;

	public const int CodeFieldNumber = 1;

	private int code_;

	public const int IsSuccessFieldNumber = 2;

	private bool isSuccess_;

	public const int CurrentContributionFieldNumber = 3;

	private long currentContribution_;

	public const int CurrentJadeFieldNumber = 4;

	private long currentJade_;

	[DebuggerNonUserCode]
	public static MessageParser<NeiWuJuanZengJadeResp_1015037> Parser => _parser;

	[DebuggerNonUserCode]
	public static MessageDescriptor Descriptor => GuildProtoReflection.Descriptor.MessageTypes[46];

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
	public long CurrentContribution
	{
		get
		{
			return currentContribution_;
		}
		set
		{
			currentContribution_ = value;
		}
	}

	[DebuggerNonUserCode]
	public long CurrentJade
	{
		get
		{
			return currentJade_;
		}
		set
		{
			currentJade_ = value;
		}
	}

	private MessageDescriptor pb_003A_003AGoogle_002EProtobuf_002EIMessage_002Eget_Descriptor()
	{
		return Descriptor;
	}

	[DebuggerNonUserCode]
	public NeiWuJuanZengJadeResp_1015037()
	{
	}

	[DebuggerNonUserCode]
	public NeiWuJuanZengJadeResp_1015037(NeiWuJuanZengJadeResp_1015037 other)
		: this()
	{
		code_ = other.code_;
		isSuccess_ = other.isSuccess_;
		currentContribution_ = other.currentContribution_;
		currentJade_ = other.currentJade_;
		_unknownFields = UnknownFieldSet.Clone(other._unknownFields);
	}

	[DebuggerNonUserCode]
	public NeiWuJuanZengJadeResp_1015037 Clone()
	{
		return new NeiWuJuanZengJadeResp_1015037(this);
	}

	[DebuggerNonUserCode]
	public override bool Equals(object other)
	{
		return Equals(other as NeiWuJuanZengJadeResp_1015037);
	}

	[DebuggerNonUserCode]
	public bool Equals(NeiWuJuanZengJadeResp_1015037 other)
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
		if (CurrentContribution != other.CurrentContribution)
		{
			return false;
		}
		if (CurrentJade != other.CurrentJade)
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
		if (CurrentContribution != 0L)
		{
			num ^= CurrentContribution.GetHashCode();
		}
		if (CurrentJade != 0L)
		{
			num ^= CurrentJade.GetHashCode();
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
		if (CurrentContribution != 0L)
		{
			output.WriteRawTag(24);
			output.WriteInt64(CurrentContribution);
		}
		if (CurrentJade != 0L)
		{
			output.WriteRawTag(32);
			output.WriteInt64(CurrentJade);
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
		if (CurrentContribution != 0L)
		{
			num += 1 + CodedOutputStream.ComputeInt64Size(CurrentContribution);
		}
		if (CurrentJade != 0L)
		{
			num += 1 + CodedOutputStream.ComputeInt64Size(CurrentJade);
		}
		if (_unknownFields != null)
		{
			num += _unknownFields.CalculateSize();
		}
		return num;
	}

	[DebuggerNonUserCode]
	public void MergeFrom(NeiWuJuanZengJadeResp_1015037 other)
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
			if (other.CurrentContribution != 0L)
			{
				CurrentContribution = other.CurrentContribution;
			}
			if (other.CurrentJade != 0L)
			{
				CurrentJade = other.CurrentJade;
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
				CurrentContribution = input.ReadInt64();
				break;
			case 32u:
				CurrentJade = input.ReadInt64();
				break;
			}
		}
	}
}
