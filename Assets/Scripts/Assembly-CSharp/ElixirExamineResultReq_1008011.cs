using System;
using System.Diagnostics;
using Google.Protobuf;
using Google.Protobuf.Reflection;

public sealed class ElixirExamineResultReq_1008011 : IMessage<ElixirExamineResultReq_1008011>, IMessage, IEquatable<ElixirExamineResultReq_1008011>, IDeepCloneable<ElixirExamineResultReq_1008011>
{
	private static readonly MessageParser<ElixirExamineResultReq_1008011> _parser = new MessageParser<ElixirExamineResultReq_1008011>(() => new ElixirExamineResultReq_1008011());

	private UnknownFieldSet _unknownFields;

	public const int IsSuccessFieldNumber = 1;

	private bool isSuccess_;

	[DebuggerNonUserCode]
	public static MessageParser<ElixirExamineResultReq_1008011> Parser => _parser;

	[DebuggerNonUserCode]
	public static MessageDescriptor Descriptor => UnrealWorldProtoReflection.Descriptor.MessageTypes[9];

	[DebuggerNonUserCode]
	MessageDescriptor IMessage.Descriptor => pb_003A_003AGoogle_002EProtobuf_002EIMessage_002Eget_Descriptor();

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

	private MessageDescriptor pb_003A_003AGoogle_002EProtobuf_002EIMessage_002Eget_Descriptor()
	{
		return Descriptor;
	}

	[DebuggerNonUserCode]
	public ElixirExamineResultReq_1008011()
	{
	}

	[DebuggerNonUserCode]
	public ElixirExamineResultReq_1008011(ElixirExamineResultReq_1008011 other)
		: this()
	{
		isSuccess_ = other.isSuccess_;
		_unknownFields = UnknownFieldSet.Clone(other._unknownFields);
	}

	[DebuggerNonUserCode]
	public ElixirExamineResultReq_1008011 Clone()
	{
		return new ElixirExamineResultReq_1008011(this);
	}

	[DebuggerNonUserCode]
	public override bool Equals(object other)
	{
		return Equals(other as ElixirExamineResultReq_1008011);
	}

	[DebuggerNonUserCode]
	public bool Equals(ElixirExamineResultReq_1008011 other)
	{
		if (other == null)
		{
			return false;
		}
		if (other == this)
		{
			return true;
		}
		if (IsSuccess != other.IsSuccess)
		{
			return false;
		}
		return object.Equals(_unknownFields, other._unknownFields);
	}

	[DebuggerNonUserCode]
	public override int GetHashCode()
	{
		int num = 1;
		if (IsSuccess)
		{
			num ^= IsSuccess.GetHashCode();
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
		if (IsSuccess)
		{
			output.WriteRawTag(8);
			output.WriteBool(IsSuccess);
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
		if (IsSuccess)
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
	public void MergeFrom(ElixirExamineResultReq_1008011 other)
	{
		if (other != null)
		{
			if (other.IsSuccess)
			{
				IsSuccess = other.IsSuccess;
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
			if (num != 8)
			{
				_unknownFields = UnknownFieldSet.MergeFieldFrom(_unknownFields, input);
			}
			else
			{
				IsSuccess = input.ReadBool();
			}
		}
	}
}
